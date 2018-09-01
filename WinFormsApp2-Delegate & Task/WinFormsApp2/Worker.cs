using System;

namespace WinFormsApp2
{

    #region WorkerEventArgs
    public class WorkerEventArgs : IDisposable
    {
        private int obj;

        //constructor with input parameter
        public WorkerEventArgs(int value)
        {
            obj = value;
            //Console.WriteLine(value);
        }

        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(this);
        }

        //main method - overload
        public void DoWork()
        {
            //do job


            //set event arguments values to send back to caller
            FinishedEventArgs args = new FinishedEventArgs();
            args.message = "WorkerEventArgs: " + obj.ToString();
            OnFinished(args);
        }
        public void DoWork(int sleepSeconds)
        {
            //do job
            System.Threading.Thread.Sleep(sleepSeconds * 1000);

            //set event arguments values to send back to caller
            FinishedEventArgs args = new FinishedEventArgs();
            args.message = "WorkerEventArgs: " + obj.ToString();
            OnFinished(args);
        }


        //---event
        public event EventHandler<FinishedEventArgs> EventArgsFinished;
        protected virtual void OnFinished(FinishedEventArgs e)
        {
            EventHandler<FinishedEventArgs> handler = EventArgsFinished;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        //event results as arguments
        public class FinishedEventArgs : EventArgs
        {
            public string message { get; set; }
        }
    }
    #endregion

    #region WorkerEventNoArgs
    public class WorkerEventNoArgs
    {
        //constructor
        public WorkerEventNoArgs() { }

        //main method
        public void DoWork()
        {
            //do job
            System.Threading.Thread.Sleep(1000);

            OnFinished(EventArgs.Empty);
        }

        //---event
        public event EventHandler<EventArgs> Finished;
        protected virtual void OnFinished(EventArgs e)
        {
            EventHandler<EventArgs> handler = Finished;
            if (handler != null)
            {
                handler(this, e);
            }
        }

    }
    #endregion


}
