using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace CLM_Segments
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CreateStatusBar();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtSourceFolder.Text = System.Configuration.ConfigurationManager.AppSettings["SourceFolder"];
            txtFilter.Text = System.Configuration.ConfigurationManager.AppSettings["Ext"];
            txtOutputFile.Text = System.Configuration.ConfigurationManager.AppSettings["OutputFile"];
            label1.Text = "AAA";
        }

        List<string> sourceFiles = new List<string>();

        private List<string> GetFiles(string sourceFolder, string filters, System.IO.SearchOption searchOption)
        {
            filters = filters.Replace(" ", string.Empty);
            try
            {
                return filters.Split('|').SelectMany(filter => System.IO.Directory.GetFiles(txtSourceFolder.Text, filter, searchOption)).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        Stopwatch stopwatch = Stopwatch.StartNew();

        private void btnProcess_Click(object sender, EventArgs e)
        {
            label1.Text = string.Empty;
            statusPanel.Text = "Processing";

            if (rdoOverwrite.Checked)
                File.Delete(txtOutputFile.Text);

            sourceFiles = GetFiles(SourceFolder.Trim(), txtFilter.Text, System.IO.SearchOption.AllDirectories);
            if (sourceFiles != null && sourceFiles.Count > 0)
            {
                sourceFiles.Sort();

                Worker worker = new Worker();
                worker.CompletedCallback = new CallbackCompleted(worker_Finished);    //delegate
                worker.SourceFiles = sourceFiles;
                //worker.OutputFile = txtOutputFile.Text;       //Way 1
                worker._OutputFile = txtOutputFile.Text;        //Way 2
                worker.DoWork();

                //if (layouts.Count > 0)
                //{
                //    var layout = layouts.OrderBy(x => x.Filename).ThenBy(x => x.SubmitterId);
                //    string outText = String.Join("\n", layout.ToArray());
                //    File.AppendAllText(txtOutputFile.Text, outText);
                //}
                //stopwatch.Stop();
                //statusPanel.Text = stopwatch.Elapsed.TotalSeconds.ToString() + " seconds";
                //MessageBox.Show("Finished");
            }
            else
            {
                stopwatch.Stop();
                statusPanel.Text = string.Empty;
                MessageBox.Show("No file found");
            }

        }

        private static Object _obj = new Object();
        int count = 0;

        //Task/Thread callback
        void worker_Finished(string results)
        {
            lock (_obj)
            {
                count++;
            }

            rtbResults.BeginInvoke((MethodInvoker)delegate
            {
                rtbResults.Text += results + "\n";
            });

            label1.BeginInvoke((MethodInvoker)delegate
            {
                //label1.Text = "File: " + count.ToString() + " of " + sourceFiles.Count.ToString("###,###");
                fileSizePanel.Text = "File: " + count.ToString() + " of " + sourceFiles.Count.ToString("###,###");
            });

            if (count == sourceFiles.Count)
            {
                label1.BeginInvoke((MethodInvoker)delegate
                {
                    stopwatch.Stop();
                    statusPanel.Text = stopwatch.Elapsed.TotalSeconds.ToString() + " seconds";
                });
                MessageBox.Show("Finished");
            }
        }

        //working but slow because threads are waiting to write
        private ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        public void WriteToFileThreadSafe(string path, string text)
        {
            _readWriteLock.EnterWriteLock();
            try
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(text);
                    sw.Close();
                }
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
        }

        public struct Layout837
        {
            public string Filename { get; set; }
            public string SubmitterId { get; set; }
            public string FrequencyType { get; set; }
            public string ClaimNumber { get; set; }
            public string ReferenceNumber { get; set; }

            public override string ToString()
            {
                return Filename + "|" + SubmitterId + "|" + FrequencyType + "|" + ClaimNumber + "|" + ReferenceNumber;
            }
        }
        static List<Layout837> layouts = new List<Layout837>();

        #region base

        string SourceFolder = string.Empty;
        string Ext = string.Empty;
        string OutputFile = string.Empty;

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

        #endregion
    }





}

