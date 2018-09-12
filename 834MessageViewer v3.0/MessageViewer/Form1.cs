//#define TEST

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace MessageViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CreateStatusBar();
            SetImageList();
            SetExpandIcon();
            SetCollapseIcon();
        }

        public string SchemaFile = string.Empty;
        XmlDocument doc = new XmlDocument();

        private void Form1_Load(object sender, EventArgs e)
        {
            SchemaFile = System.Configuration.ConfigurationManager.AppSettings["SchemaFile"];
            //doc.Load(SchemaFile);
        }

        private void btnSource_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK)
            {
                txtSource.Text = openFileDialog1.FileName;
                try
                {
                    //string text = File.ReadAllText(txtSource.Text);
                    //size = text.Length;
                }
                catch (IOException)
                {
                }
            }
        }

        private void btnTarget_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog2.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK)
            {
                txtTarget.Text = openFileDialog2.FileName;
                try
                {
                    //string text = File.ReadAllText(txtTarget);
                    //size = text.Length;
                }
                catch (IOException)
                {
                }
            }
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            bool action = true;

            if (!File.Exists(txtSource.Text))
            {
                MessageBox.Show("File not found");
                action = false;
            }

            if (action)
            {
                int lines = Analyze_Message();
                if (lines > 0)
                    statusPanel.Text = lines.ToString() + " segments analyzed";
                //MessageBox.Show(lines.ToString() + " analyzed");
            }

        }

        public struct nodeSegment
        {
            public long id { get; set; }
            public TreeNode node { get; set; }
        }

        public struct MessagePosition
        {
            public int gsID { get; set; }
            public int stID { get; set; }
            public int from { get; set; }
            public int to { get; set; }
            public int count { get; set; }
        }

        private int Analyze_Message()
        {
            int lines = 0;

            FileStream inFS = new FileStream(txtSource.Text, FileMode.Open, FileAccess.Read);
            var sourceFileLength = inFS.Length;
            inFS.Close();

            fileSizePanel.Text = "FileSize: " + sourceFileLength.ToString("##,###,###,###");
            statusPanel.Text = "Processing";

            //calculate loop count for the input file
            int unitSize = 1048576;                         //1 Mega Bytes: 1,048,576 bytes
            int loopCount = (int)(sourceFileLength / unitSize);
            if (sourceFileLength % unitSize > 0)
                loopCount++;

            treeView1.Nodes.Clear();
            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                //1.read file into List<>
                List<string[]> fileChunk = new List<string[]>();

                long filePoint = 0;
                int readCount = 0;
                byte[] bufferBytes = new byte[unitSize];
                string remainder = string.Empty;

                using (var mmf = MemoryMappedFile.CreateFromFile(txtSource.Text, FileMode.Open, "Map1"))
                {
                    for (int t = 0; t < loopCount; t++)
                    {
                        int readSize = sourceFileLength - filePoint > unitSize ? unitSize : (int)(sourceFileLength - filePoint);
                        using (MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor(filePoint, readSize))
                        {
                            string result = string.Empty;
                            readCount = accessor.ReadArray(0, bufferBytes, 0, readSize);
                            if (readCount > 0)
                            {
                                result = remainder + System.Text.Encoding.UTF8.GetString(bufferBytes);
                                result = result.Replace("\r\n", string.Empty);
                                string[] readArray = result.Split('~');
                                int len = readArray[readArray.Length - 1].Trim().Length;
                                if (len > 0)
                                {
                                    remainder = readArray[readArray.Length - 1];
                                }
                                fileChunk.Add(readArray.Take(readArray.Length - 1).ToArray());
                            }
                            Array.Clear(bufferBytes, 0, unitSize);
                        }
                        filePoint += readCount;
                    }
                }

                //2.build nodes
                List<nodeSegment> nodeChunk = new List<nodeSegment>();

                for (int i = 0; i < fileChunk.Count; i++)
                {
                    nodeSegment[] nc = new nodeSegment[fileChunk[i].Length];
                    nodeChunk.AddRange(nc.ToList());
                }

                //3.parsing
                List<MessagePosition> messagePosition = new List<MessagePosition>();

                bool newMessage = true;
                int gsCount = 0;
                int stCount = 0;
                int seq = 0;
                MessagePosition mp = new MessagePosition();

                for (int x = 0; x < fileChunk.Count; x++)
                {
                    for (int i = 0; i < fileChunk[x].Length; i++)
                    {
                        TreeNode tn = new TreeNode();
                        //string[] segments = fileChunk[x][i].Split('*').First().ToUpper();
                        tn.Name = fileChunk[x][i].Split('*').First().ToUpper();
                        tn.Text = fileChunk[x][i];
                        nodeSegment ns = new nodeSegment();
                        ns.node = tn;
                        ns.id = seq;
                        nodeChunk[seq] = ns;
                        //nodeChunk[x * i + i] = ns;

                        if (newMessage)
                        {
                            mp = new MessagePosition();
                            newMessage = false;
                        }
                        if (tn.Name == "GS")
                        {
                            gsCount++;
                            stCount = 0;
                        }

                        if (tn.Name == "ST")
                        {
                            stCount++;
                            mp.stID = stCount;
                            mp.gsID = gsCount;
                            mp.from = i;
                        }
                        else if (tn.Name == "SE")
                        {
                            mp.to = i;
                            mp.count = mp.to - mp.from + 1;
                            messagePosition.Add(mp);
                            newMessage = true;
                        }
                        seq++;
                    }
                }

                //4.build treeView1

                string nodeName = string.Empty;
                string nodeText = string.Empty;
                int groupCount = 0;     //group  Count

                statusPanel.Text = "Build Tree";
                lines = nodeChunk.Count;
                treeView1.BeginUpdate();

                for (int x = 0; x < nodeChunk.Count; x++)
                {
                    TreeNode tn = nodeChunk[x].node;
                    nodeName = tn.Name;
                    tn.Name = nodeChunk[x].id.ToString() + "." + nodeName;


                    if (nodeName == "ISA")
                    {
                        TreeNode newNode = new TreeNode();
                        newNode.Name = "Interchange";
                        newNode.Text = "Interchange";
                        newNode.ImageIndex = 0;
                        newNode.BackColor = System.Drawing.Color.Yellow;
                        treeView1.Nodes.Add(newNode);
                        treeView1.SelectedNode = newNode;

                        treeView1.SelectedNode.Nodes.Add(tn);
                        treeView1.SelectedNode = tn;            //Move down to child level and Set cursor
                    }
                    else if (nodeName == "IEA")
                    {
                        treeView1.SelectedNode.Parent.Nodes.Add(tn);
                        treeView1.SelectedNode = treeView1.SelectedNode.Parent;     //Moving up and Set cursor
                    }
                    else if (nodeName == "GS")
                    {
                        groupCount++;
                        TreeNode newNode = new TreeNode();
                        string groupName = "Group - " + groupCount.ToString();
                        newNode.Name = groupName;
                        newNode.Text = groupName;
                        newNode.ImageIndex = 0;
                        newNode.BackColor = System.Drawing.Color.LightCyan;
                        treeView1.SelectedNode.Parent.Nodes.Add(newNode);
                        treeView1.SelectedNode = newNode;

                        treeView1.SelectedNode.Nodes.Add(tn);
                        treeView1.SelectedNode = tn;

                        //Start Threading
                        List<TreeView> listTreeView = new List<TreeView>();
                        List<Thread> taskList = new List<Thread>();
                        var gsList = messagePosition.Where(i => i.gsID == groupCount).ToList();

#if TEST
                        //TEST: before Threading
                        var taskChunk = nodeChunk.GetRange(gsList[0].from, gsList[0].count);
                        TreeView taskNode = Build_Trans_Tree(gsList[0].stID, taskChunk);
                        CopyTreeNodes(taskNode, treeView1);
#else
                        for (int i = 0; i < gsList.Count; i++)
                        {
                            int temp = i;
                            var task = new Thread(() =>
                            {
                                var taskChunk = nodeChunk.GetRange(gsList[temp].from, gsList[temp].count);
                                TreeView taskNode = Build_Trans_Tree(gsList[temp].stID, taskChunk);
                                listTreeView.Add(taskNode);
                            });
                            task.Start();
                            taskList.Add(task);
                        }
                        foreach (var item in taskList)
                        {
                            item.Join();
                        }

                        //List<TreeView>
                        foreach (var item in listTreeView)
                        {
                            CopyTreeNodes(item, treeView1);     //attach ST TreeView to Form.TreeView1
                        }
#endif
                        x = gsList[gsList.Count - 1].to;    //reset loop index
                    }
                    else if (nodeName == "GE")
                    {
                        treeView1.SelectedNode.Parent.Nodes.Add(tn);
                        treeView1.SelectedNode = treeView1.SelectedNode.Parent;
                    }

                    if (nodeChunk[x].id % 100 == 0)
                        statusPanel.Text = string.Format("{0:0,0}", nodeChunk[x].id);
                }
                //End-4.build treeView1

                Cursor.Current = Cursors.Default;
                treeView1.EndUpdate();
                treeView1.CollapseAll();
            }
            catch (Exception ex)
            {
                statusPanel.Text = "Exception Error";
                MessageBox.Show(ex.Message);
            }
            finally
            {
                GC.Collect();
                System.Threading.Thread.Sleep(1000);     //give disk hardware time to recover
            }

            TimeSpan stopwatchElapsed = stopwatch.Elapsed;
            Console.WriteLine(Convert.ToInt32(stopwatchElapsed.TotalMilliseconds));

            //string elapsed = Convert.ToInt32(stopwatchElapsed.TotalMinutes).ToString() + " minutes";
            //string elapsed = stopwatch.Elapsed.TotalMinutes.ToString() + " minutes";
            string elapsed = stopwatch.Elapsed.TotalSeconds.ToString() + " seconds";
            MessageBox.Show(elapsed);
            return lines;
        }

        // Attach sourceTree to the node under TargetTree
        public void CopyTreeNodes(TreeView treeview1, TreeView treeview2)
        {
            TreeNode newTn;
            foreach (TreeNode tn in treeview1.Nodes)
            {
                newTn = new TreeNode(tn.Text, tn.ImageIndex, tn.SelectedImageIndex);
                newTn.Name = tn.Name;
                CopyChildren(newTn, tn);
                treeview2.SelectedNode.Nodes.Add(newTn);
            }
        }
        public void CopyChildren(TreeNode parent, TreeNode original)
        {
            TreeNode newTn;
            foreach (TreeNode tn in original.Nodes)
            {
                newTn = new TreeNode(tn.Text, tn.ImageIndex, tn.SelectedImageIndex);
                newTn.Name = tn.Name;
                parent.Nodes.Add(newTn);
                CopyChildren(newTn, tn);
            }
        }

        private TreeView Build_Trans_Tree(int stID, List<nodeSegment> taskChunk)
        {
            TreeView treeView1 = new TreeView();    //Create TreeView
            //this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);

            string nodeName = string.Empty;
            int Loop2000Count = 0;
            int Loop2100ACount = 0;
            int Loop2300Count = 0;

            TreeNode transNode = new TreeNode();
            string transName = "Transaction - " + stID.ToString();
            transNode.Name = transName;
            transNode.Text = transName;
            transNode.ImageIndex = 0;

            treeView1.Nodes.Add(transNode);
            treeView1.SelectedNode = transNode;

            try
            {
                for (int i = 0; i < taskChunk.Count; i++)
                {
                    TreeNode tn = taskChunk[i].node;
                    nodeName = tn.Name;
                    tn.Name = taskChunk[i].id.ToString() + "." + nodeName;

                    if (nodeName == "ST")
                    {
                        treeView1.SelectedNode.Nodes.Add(tn);
                        treeView1.SelectedNode = tn;
                    }
                    else if (nodeName == "SE")
                    {
                        if (Loop2300Count > 0)
                            treeView1.SelectedNode = treeView1.SelectedNode.Parent;     //Moving up

                        if (Loop2000Count > 0)
                            treeView1.SelectedNode = treeView1.SelectedNode.Parent;     //Moving up

                        treeView1.SelectedNode.Parent.Nodes.Add(tn);
                        treeView1.SelectedNode = treeView1.SelectedNode.Parent;         //Moving up to trans level
                    }
                    else
                    {
                        //--------------------------
                        //Message Segment Processing
                        //--------------------------

                        //Loop1000A - 1 time
                        if (nodeName == "N1" && tn.Text.Split('*')[1] == "P5")
                        {
                            TreeNode newNode = new TreeNode();
                            string Loop1000A = "Loop1000A";
                            newNode.Name = Loop1000A;
                            newNode.Text = Loop1000A;
                            treeView1.SelectedNode.Parent.Nodes.Add(newNode);               //Add sibling
                            treeView1.SelectedNode = newNode;

                            treeView1.SelectedNode.Nodes.Add(tn);                           //Add child
                            treeView1.SelectedNode = tn;                                    //Move cursor to child level
                        }
                        //Loop1000B - 1 time
                        else if (nodeName == "N1" && tn.Text.Split('*')[1] == "IN")
                        {
                            treeView1.SelectedNode = treeView1.SelectedNode.Parent;         //Moving up

                            TreeNode newNode = new TreeNode();
                            string Loop1000B = "Loop1000B";
                            newNode.Name = Loop1000B;
                            newNode.Text = Loop1000B;
                            treeView1.SelectedNode.Parent.Nodes.Add(newNode);
                            treeView1.SelectedNode = newNode;

                            treeView1.SelectedNode.Nodes.Add(tn);
                            treeView1.SelectedNode = tn;                                    //Move cursor to child level
                        }
                        //Loop2000
                        else if (nodeName == "INS" && tn.Text.Split('*')[1] == "Y")
                        {
                            treeView1.SelectedNode = treeView1.SelectedNode.Parent;         //Moving up
                            if (Loop2300Count > 0)
                                treeView1.SelectedNode = treeView1.SelectedNode.Parent;     //Moving up

                            Loop2000Count++;
                            TreeNode newNode = new TreeNode();
                            string Loop2000 = "Loop2000 - " + Loop2000Count.ToString();
                            newNode.Name = Loop2000;
                            newNode.Text = Loop2000;
                            treeView1.SelectedNode.Parent.Nodes.Add(newNode);
                            treeView1.SelectedNode = newNode;

                            treeView1.SelectedNode.Nodes.Add(tn);
                            treeView1.SelectedNode = tn;                                    //move to child level

                            Loop2300Count = 0;
                        }
                        //Loop2100A - 1 time
                        else if (nodeName == "NM1" && tn.Text.Split('*')[1] == "IL")
                        {
                            Loop2100ACount++;
                            TreeNode newNode = new TreeNode();
                            string Loop2100A = "Loop2100A";
                            newNode.Name = Loop2100A;
                            newNode.Text = Loop2100A;
                            treeView1.SelectedNode.Parent.Nodes.Add(newNode);
                            treeView1.SelectedNode = newNode;

                            treeView1.SelectedNode.Nodes.Add(tn);
                            treeView1.SelectedNode = tn;                                    //move to child level
                        }
                        //Loop2300
                        else if (nodeName == "HD" && (tn.Text.Split('*')[1] == "001" || tn.Text.Split('*')[1] == "021"))
                        {
                            if (Loop2100ACount > 0 || Loop2300Count > 0)
                                treeView1.SelectedNode = treeView1.SelectedNode.Parent;

                            Loop2300Count++;
                            TreeNode newNode = new TreeNode();
                            string Loop2300 = "Loop2300 - " + Loop2300Count.ToString();
                            newNode.Name = Loop2300;
                            newNode.Text = Loop2300;
                            treeView1.SelectedNode.Parent.Nodes.Add(newNode);
                            treeView1.SelectedNode = newNode;

                            treeView1.SelectedNode.Nodes.Add(tn);
                            treeView1.SelectedNode = tn;                                    //move to child level
                        }
                        else
                        {
                            treeView1.SelectedNode.Parent.Nodes.Add(tn);                    //Add sibling
                            treeView1.SelectedNode = tn;                                    //Set curser position
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }

            return treeView1;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNode node = e.Node;
                string[] name = node.Name.Split('.');
                rtbContent.Text = "node.Id: " + name[0] + "\n";
                rtbContent.Text += "node.Name: " + name[1] + "\n";
                rtbContent.Text += "node.Text: " + node.Text;
                //MessageBox.Show(node.Name + "\n" + node.Text);
            }
            catch (Exception)
            {
                //throw;
            }
        }

        protected StatusBar mainStatusBar = new StatusBar();
        protected StatusBarPanel statusPanel = new StatusBarPanel();
        protected StatusBarPanel fileSizePanel = new StatusBarPanel();
        protected StatusBarPanel datetimePanel = new StatusBarPanel();

        private void CreateStatusBar()
        {

            // Set first panel properties and add to StatusBar
            statusPanel.BorderStyle = StatusBarPanelBorderStyle.Sunken;
            statusPanel.ToolTipText = "";
            statusPanel.Text = "Ready";
            statusPanel.AutoSize = StatusBarPanelAutoSize.Spring;
            mainStatusBar.Panels.Add(statusPanel);

            // Set second panel properties and add to StatusBar
            fileSizePanel.BorderStyle = StatusBarPanelBorderStyle.Raised;
            fileSizePanel.ToolTipText = "";
            fileSizePanel.Text = "";
            fileSizePanel.AutoSize = StatusBarPanelAutoSize.Contents;
            mainStatusBar.Panels.Add(fileSizePanel);

            // Set third panel properties and add to StatusBar
            datetimePanel.BorderStyle = StatusBarPanelBorderStyle.Raised;
            datetimePanel.ToolTipText = "DateTime: " + System.DateTime.Today.ToString();
            datetimePanel.Text = System.DateTime.Today.ToLongDateString();
            datetimePanel.AutoSize = StatusBarPanelAutoSize.Contents;
            mainStatusBar.Panels.Add(datetimePanel);

            mainStatusBar.ShowPanels = true;

            // Add StatusBar to Form controls
            this.Controls.Add(mainStatusBar);
        }

        private void SetImageList()
        {
            var MyImages = new ImageList();

            MyImages.Images.Add("folder", new Icon("if_folder.ico"));
            MyImages.Images.Add("folder_open", new Icon("if_folder-open.ico"));

            //MyImages.ImageSize = new Size(16, 16);
            //MyImages.Images.Add(new Icon(System.Drawing.SystemIcons.Error, new Size(16, 16)));

            this.treeView1.ImageList = MyImages;
            this.treeView1.SelectedImageIndex = 1;      //when selected
        }

        private void button1_Click(object sender, EventArgs e)
        {
            treeView1.ExpandAll();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            treeView1.CollapseAll();
        }

        private void SetExpandIcon()
        {
            // Assign an image to the button.
            button1.Image = Image.FromFile("icons8-plus-50.png").GetThumbnailImage(16, 16, null, IntPtr.Zero);
            // Align the image and text on the button.
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.TextAlign = ContentAlignment.MiddleRight;
            //button1.Text = "Expand";
            button1.Text = string.Empty;
        }

        private void SetCollapseIcon()
        {
            // Assign an image to the button.
            button2.Image = Image.FromFile("icons8-minus-50.png").GetThumbnailImage(16, 16, null, IntPtr.Zero);
            // Align the image and text on the button.
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.TextAlign = ContentAlignment.MiddleRight;
            //button2.Text = "Collapse";
            button2.Text = string.Empty;
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 1;
        }

        private void treeView1_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 0;
        }

    }
}
