using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreadApp2_ThreadPool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //1. All ThreadPool threads are background threads
        //2. All ThreadPool threads are in the multithreaded apartment

        #region btnThreadPool1 - console
        private void btnThreadPool1_Click(object sender, EventArgs e)
        {
            rtbResults.Text = string.Empty;
            label1.Text = string.Empty;

            ThreadPool.SetMaxThreads(1, 5);     //set minimum and maximum ThreadPool Count

            for (int i = 0; i < 100; i++)
                ThreadPool.QueueUserWorkItem(new WaitCallback(TaskCallBack), i);    //Threads are auto Start and reused

            Console.WriteLine("Done...");
        }

        //Thread Method
        private void TaskCallBack(Object state)
        {
            string ThreadName = "Thread " + state.ToString();

            for (int i = 0; i < 5; i++)
                Console.WriteLine(ThreadName + ": " + i.ToString());

            Console.WriteLine(ThreadName + " Finished...");
        }
        #endregion

        #region btnThreadPool2 - Class as Thread parameter

        public delegate void LabelDelegate(string state);

        class ThreadInfo2
        {
            public LabelDelegate FuncName { get; set; }
            public int SelectedIndex { get; set; }
        }

        private void btnThreadPool2_Click(object sender, EventArgs e)
        {
            rtbResults.Text = string.Empty;

            ThreadInfo2 threadInfo = new ThreadInfo2();
            threadInfo.FuncName = UpdateLabel;
            threadInfo.SelectedIndex = 3;

            //ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessFile2), threadInfo);   //1 thread & include Join()

            //var thread = new Thread(() => { ProcessFile2(threadInfo) });
            //thread.Start();
            //thread.Join();    //do no use if you want to label to be updated 

            var task = Task.Factory.StartNew(() =>
            {
                ProcessFile2(threadInfo);
            });
            //task.Wait();      //do no use if you want to label to be updated 

            label1.Text = "Done";
        }

        //Thread Method
        private void ProcessFile2(object state)
        {
            ThreadInfo2 threadInfo = state as ThreadInfo2;
            LabelDelegate FuncName = threadInfo.FuncName;
            int index = threadInfo.SelectedIndex;

            //this.Invoke(new LabelDelegate(UpdateLabel));        //Invoke delegate function
            FuncName.Invoke("ProcessFile2 is invoked");
        }

        //update Form1.Lable
        private void UpdateLabel(string state)
        {
            label1.BeginInvoke((MethodInvoker) delegate
            {
                //label1.Text = state;
                rtbResults.Text += state;
                System.Threading.Thread.Sleep(1000);
            });
        }

        #endregion

        #region btnThreadPool3 - Form ProgressBar Display

        public delegate void BarDelegate();

        class ThreadInfo3
        {
            public int index { get; set; }
        }

        private void btnThreadPool3_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = 100;
            progressBar1.Minimum = 0;
            progressBar1.Value = 0;

            ThreadInfo3 threadInfo = new ThreadInfo3();
            threadInfo.index = 70;

            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessFile3), threadInfo);   //1 thread
            MessageBox.Show("Done");
        }

        //Thread Method
        private void ProcessFile3(object state)
        {
            ThreadInfo3 threadInfo = state as ThreadInfo3;
            int index = threadInfo.index;

            //UI Update
            try
            {
                for (int i = 0; i < index; i++)
                {
                    this.Invoke(new BarDelegate(UpdateBar));        //Invoke delegate function

                    //label1.Text = i.ToString();
                    //Cross-thread operation not valid: Control 'label1' accessed from a thread other than the thread it was created on.
                    //'ProcessFile3' is Thread callback
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Update the graphical bar: delegate is belong to Form1
        private void UpdateBar()
        {
            progressBar1.Value++;

            rtbResults.Text += progressBar1.Value.ToString() + "\n";

            if (progressBar1.Value == progressBar1.Maximum)
            {
                // We are finished and the progress bar is full.
                //return true;
            }
        }

        #endregion



    }
}
