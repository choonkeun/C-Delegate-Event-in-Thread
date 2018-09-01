using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string dateFormat = "yyyy-MM-dd HH:mm:ss";

        #region btnDelegate1
        private void btnDelegate1_Click(object sender, EventArgs e)
        {
            Foo1 fooExample = Bar;
            string time = fooExample(DateTime.UtcNow);
            Console.WriteLine(time);
            MessageBox.Show(time);
        }

        public delegate string Foo1(DateTime time);
        public string Bar(DateTime value)
        {
            return value.ToString("t");
        }
        #endregion

        #region btnDelegate2
        private void btnDelegate2_Click(object sender, EventArgs e)
        {
            var now = DateTime.UtcNow;
            Foo2 fooTime, fooDate, fooMulticast;     //delegate

            fooTime = BarTime;                  //instance
            fooDate = BarDate;                  //instance
            fooMulticast = fooTime + fooDate;   //instance

            Console.Write("FooTime: \n----\n");
            MessageBox.Show("FooTime: " + fooTime(now));
            //Console.WriteLine();

            Console.Write("FooDate: \n----\n");
            MessageBox.Show("FooDate: " + fooDate(now));
            //Console.WriteLine();

            Console.Write("FooMulticast: \n----\n");
            MessageBox.Show("FooMulticast: " + fooMulticast(now));      //not working
        }

        public delegate string Foo2(DateTime time);
        public string BarTime(DateTime value)
        {
            var str = value.ToString("t");
            Console.WriteLine("{0}", str);
            return str;
        }
        public string BarDate(DateTime value)
        {
            var str = value.ToString("dd/MM/yyyy");
            Console.WriteLine("{0}", str);
            return str;
        }
        #endregion

        #region btnDelegate3
        //no callback
        private void btnDelegate3_Click(object sender, EventArgs e)
        {
            SomeClass3 someClass3 = new SomeClass3();
            someClass3.DoBackgroundWork();  //start thread
        }
        #endregion

        #region btnDelegate4
        private void btnDelegate4_Click(object sender, EventArgs e)
        {
            SomeClass4 someClass4 = new SomeClass4();
            someClass4.CompletedCallback = new Task4CallbackCompleted(btnDelegate4Callback);    //delegate
            for (int i = 0; i < 5; i++)
            {
                int tempId = i;
                someClass4.DoWork(tempId);
                Application.DoEvents();     //this line will screen update for each delegate
            }
            MessageBox.Show("Done");
        }
        private void btnDelegate4Callback(string results)
        {
            txtResults.BeginInvoke((MethodInvoker)delegate
            {
                txtResults.Text = results;
            });
        }
        #endregion

        #region btnDelegate5
        //callback from 1 thread
        private void btnDelegate5_Click(object sender, EventArgs e)
        {
            SomeClass5 someClass5 = new SomeClass5();
            someClass5.CompletedCallback = new Task5CallbackCompleted(btnDelegate5Callback);    //delegate
            someClass5.DoWork();
            MessageBox.Show("btnDelegate5 is Done and Thread result will come later");
        }
        private void btnDelegate5Callback(string results)
        {
            rtbResults.BeginInvoke((MethodInvoker)delegate
            {
                rtbResults.Text += results + "\n";
            });
            txtResults.BeginInvoke((MethodInvoker)delegate
            {
                txtResults.Text = results;
            });
            //label1.BeginInvoke((MethodInvoker)delegate
            //{
            //    label1.Text = results;
            //});
        }
        #endregion

        #region btnDelegate6

        private void btnDelegate6_Click(object sender, EventArgs e)
        {
            delegateList.Clear();
            txtActionResults.Text = string.Empty;
            for (int i = 0; i < 10; i++)
            {
                SomeClass6 someClass = new SomeClass6(i);
                Thread newThread = new Thread(someClass.DoWork);
                newThread.Start();
            }
            txtActionResults.Text = string.Join(", ", delegateList);
        }

        static public List<string> delegateList = new List<string>();

        static public void NamedMethod_Odd(string text, int digit)
        {
            delegateList.Add(text);
            //Console.WriteLine ("Named said: {0} {1}", text, digit);
        }
        static public void NamedMethod_Even(string text, int digit)
        {
            //Console.WriteLine("Named said: {0} {1}", text, digit);
        }
        #endregion

        #region btnDelegate7
        private void btnDelegate7_Click(object sender, EventArgs e)
        {
            funcList.Clear();
            txtFuncResults.Text = string.Empty;
            for (int i = 0; i < 10; i++)
            {
                string result = null;
                SomeClass7 someClass = new SomeClass7(i);
                Thread newThread = new Thread(() =>
                {
                    result = someClass.DoWork();
                    if (result.Length > 0)
                        funcList.Add(result);
                });
                newThread.Start();
            }
            txtFuncResults.Text = string.Join(", ", funcList);
        }
        public List<string> funcList = new List<string>();

        static public string NamedMethod_Odd(int digit)
        {
            return "Odd" + digit.ToString();
        }
        static public string NamedMethod_Even(int digit)
        {
            return string.Empty;
            //Console.WriteLine("Named said: {0} {1}", text, digit);
        }
        #endregion

        #region btnDelegate8 Delegate & Task
        private void btnDelegate8_Click(object sender, EventArgs e)
        {
            SomeClass8 someClass8 = new SomeClass8();
            someClass8.CompletedCallback = new Task8Completed(btnDelegate8Callback);    //set delegate

            for (int i = 0; i < 5; i++)
            {
                int tempId = i;
                var task = Task.Factory.StartNew(() =>
                {
                    someClass8.DoWork(tempId);
                });
            }
            MessageBox.Show("Done");
        }

        private void btnDelegate8Callback(string results)
        {
            txtResults.BeginInvoke((MethodInvoker)delegate
            {
                txtTaskResults.Text += results;
                //rtbResults.Text += results + "\n";
            });
        }
        #endregion

        #region btnTaskEventArg Worker

        private List<Task> TaskList = new List<Task>();

        //callback from multiple thread
        private void btnTaskEventArg_Click(object sender, EventArgs e)
        {
            label1.Text = string.Empty;

            //working
            for (int i = 0; i < 5; i++)
            {
                int tempId = i;     // Make a temporary here to pass parameter to Task
                var task = Task.Factory.StartNew(() =>
                {
                    WorkerEventArgs worker = new WorkerEventArgs(tempId);
                    worker.EventArgsFinished += worker_Finished;

                    if (rdoUse.Checked)
                        worker.DoWork();        //When use WaitAll(), no need to use sleepSeconds
                    else
                        worker.DoWork(tempId);

                    worker.Dispose();
                });
                TaskList.Add(task);
            }
            if (rdoUse.Checked)
            {
                Task.WaitAll(TaskList.ToArray());
            }

            //working
            //Parallel.For(0, 5, id =>
            //{
            //    WorkerEventArgs worker = new WorkerEventArgs(id);
            //    worker.EventArgsFinished += worker_Finished;
            //    worker.DoWork();
            //});        

        }

        //thread callback
        void worker_Finished(object sender, WorkerEventArgs.FinishedEventArgs e)
        {
            label1.BeginInvoke((MethodInvoker)delegate
            {
                label1.Text += e.message + "\n";
            });
        }
        #endregion


    }

    #region btnDelegate3
    public delegate void Task3Completed(string taskId, bool isError);
    public class SomeClass3
    {
        public void DoBackgroundWork()
        {
            //instance class
            SomeThreadTask3 t = new SomeThreadTask3();

            //set value to incstance class property
            t.TaskId = "MyTask: " + DateTime.Now.Ticks.ToString();
            t.CompletedCallback = new Task3Completed(SomeThreadTask3Callback);    //delegate

            //create thread & call
            Thread thread = new Thread(t.ExecuteThreadTask);
            thread.Start();

        }

        //callback 
        public void SomeThreadTask3Callback(string taskId, bool isError)
        {
            // Do post background work here.
            // Cleanup the thread and task object references, etc.
            MessageBox.Show("Task Id: " + taskId + "\nError: " + (isError ? "Yes" : "No"));
        }
    }

    internal class SomeThreadTask3
    {
        private string _taskId;
        private Task3Completed _completedCallback;     //DELEGATE

        internal string TaskId
        {
            get { return _taskId; }
            set { _taskId = value; }
        }

        internal Task3Completed CompletedCallback
        {
            get { return _completedCallback; }
            set { _completedCallback = value; }
        }

        //method
        internal void ExecuteThreadTask()
        {
            bool isError = false;

            //do something
            System.Threading.Thread.Sleep(3000);

            //Invoke callback
            _completedCallback.Invoke(_taskId, isError);
        }

    }
    #endregion

    #region btnDelegate4
    public delegate void Task4CallbackCompleted(string results);
    public delegate void Task4Completed(string results);

    public class SomeClass4
    {
        private string _results;

        private Task4CallbackCompleted _completedCallback;     //DELEGATE
        internal Task4CallbackCompleted CompletedCallback
        {
            get { return _completedCallback; }
            set { _completedCallback = value; }
        }

        public void DoWork(int index)
        {
            _results = "call: " + index.ToString() + ", Date: " + DateTime.Now.ToShortDateString();
            _completedCallback.Invoke(_results);        //invoke delegate and come back to here
            Console.WriteLine(_results);
            System.Threading.Thread.Sleep(1000);
        }
    }
    #endregion

    #region btnDelegate5 Threads
    public delegate void Task5CallbackCompleted(string results);
    public delegate void Task5Completed(string results);

    public class SomeClass5
    {
        private Task5CallbackCompleted _completedCallback;     //DELEGATE
        internal Task5CallbackCompleted CompletedCallback
        {
            get { return _completedCallback; }
            set { _completedCallback = value; }
        }
        //callback 
        public void SomeThreadTask5Callback(string results)
        {
            _completedCallback.Invoke(results);
        }


        public void DoWork()
        {
            //instance class
            SomeThreadTask5 t = new SomeThreadTask5();

            //set value to incstance class property
            t.CompletedCallback = new Task5Completed(SomeThreadTask5Callback);    //delegate

            //create thread & call
            for (int i = 0; i < 5; i++)
            {
                int delaySeconds = i;
                t.inParameter = "call: " + delaySeconds.ToString();
                Thread thread = new Thread(() => t.ExecuteThreadTask(delaySeconds));
                thread.Start();
            }

        }
    }

    internal class SomeThreadTask5
    {
        private string _localValue;
        private Task5Completed _completedCallback;     //DELEGATE

        internal string inParameter
        {
            get { return _localValue; }
            set { _localValue = value; }
        }

        internal Task5Completed CompletedCallback
        {
            get { return _completedCallback; }
            set { _completedCallback = value; }
        }

        //method
        internal void ExecuteThreadTask(int delaySeconds)
        {
            //do something
            System.Threading.Thread.Sleep(delaySeconds * 1000);

            _localValue = delaySeconds.ToString() + " run";
            //_localValue += ", Date: " + DateTime.Now.ToShortDateString();

            //Invoke callback ith parameter: Invoke "SomeThreadTask5Callback"
            _completedCallback.Invoke(_localValue);
        }
    }
    #endregion

    #region btnDelegate6 Action<string, int>
    public class SomeClass6
    {
        private int obj;
        public SomeClass6(int value)
        {
            obj = value;
        }

        //"Action" does not need "delegate"
        public Action<string, int> NamedActionDelegate { get; set; }

        public void DoWork()
        {
            NamedActionDelegate = (obj % 2 != 0)
                ? (Action<string, int>)Form1.NamedMethod_Odd
                : (Action<string, int>)Form1.NamedMethod_Even;
            //Invoke
            NamedActionDelegate.Invoke("Hi " + obj.ToString(), obj);
        }
    }
    #endregion

    #region btnDelegate7 Func<int, string>
    public class SomeClass7
    {
        private int obj;
        public SomeClass7(int value)
        {
            obj = value;
        }

        //Func<> that takes an int as input and returns a string
        public Func<int, string> NamedFuncDelegate { get; set; }

        //Func<> that takes an string as input and returns a int result
        public Func<string, int> NamedFuncDelegate2 { get; set; }

        public string DoWork()
        {
            this.NamedFuncDelegate = (obj % 2 != 0)
                ? (Func<int, string>)Form1.NamedMethod_Odd
                : (Func<int, string>)Form1.NamedMethod_Even;
            //Invoke
            return this.NamedFuncDelegate.Invoke(obj);
        }
    }
    #endregion

    #region btnDelegate8 Delegate & Task

    //btnDelegate8 is simillar to btnDelegate4,
    //Difference is that btnDelegate8 is multi-threading but btnDelegate4 is multitple times calling

    public delegate void Task8Completed(string results);

    public class SomeClass8
    {
        private string _results;

        private Task8Completed _completedCallback;     //DELEGATE
        internal Task8Completed CompletedCallback
        {
            get { return _completedCallback; }
            set { _completedCallback = value; }
        }

        public void DoWork(int index)
        {
            _results = "call: " + index.ToString() + ", ";  // ", Date: " + DateTime.Now.ToShortDateString();

            _completedCallback.Invoke(_results);        //invoke delegate and come back to here
        }
    }
    #endregion

}

