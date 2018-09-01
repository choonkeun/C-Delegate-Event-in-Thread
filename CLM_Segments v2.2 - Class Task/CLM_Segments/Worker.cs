using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CLM_Segments
{
    public delegate void CallbackCompleted(string results);
    public delegate void Completed(string results);

    public class Worker
    {
        private CallbackCompleted _completedCallback;     //DELEGATE
        internal CallbackCompleted CompletedCallback
        {
            get { return _completedCallback; }
            set { _completedCallback = value; }
        }
        //callback 
        public void WorkerCallback(string results)
        {
            _completedCallback.Invoke(results);
        }

        private List<string> _SourceFiles = new List<string>();
        internal List<string> SourceFiles
        {
            get { return _SourceFiles; }
            set { _SourceFiles = value; }
        }

        //Way 1
        //private string _OutputFile;
        //internal string OutputFile
        //{
        //    get { return _OutputFile; }
        //    set { _OutputFile = value; }
        //}

        //Way 2
        public string _OutputFile;

        List<Task> TaskList = new List<Task>();

        public void DoWork()
        {
            //----------------------------------------------------------------------------------------------
            // parallel will not release until all task is finished, so no invoke delegate('WorkerCallback')
            //----------------------------------------------------------------------------------------------
            //Parallel.ForEach(
            //    _SourceFiles,
            //    new ParallelOptions { MaxDegreeOfParallelism = 50 },
            //    item =>
            //    {
            //        InternalThreadTask t = new InternalThreadTask();
            //        t.CompletedCallback = new Completed(WorkerCallback);    //delegate
            //        t.OutputFile = _OutputFile;
            //        t.Parse(item);
            //    }
            //);

            InternalThreadTask t = new InternalThreadTask();
            t.CompletedCallback = new Completed(WorkerCallback);    //delegate

            foreach (var item in _SourceFiles)
            {
                var task = Task.Factory.StartNew(() =>
                {
                    //t.OutputFile = _OutputFile;       //Way 1
                    t._OutputFile = _OutputFile;        //Way 2
                    t.Parse(item);
                });
                TaskList.Add(task);
            }
            //-------------------------------------------------------------------------------------------
            // this will not allow invoke delegate('WorkerCallback') and Form control will not be updated
            //-------------------------------------------------------------------------------------------
            //Task.WaitAll(TaskList.ToArray());     //no form control update
        }
    }

    internal class InternalThreadTask
    {
        private Completed _completedCallback;       //delegate "worker_Finished"     
        internal Completed CompletedCallback
        {
            get { return _completedCallback; }
            set { _completedCallback = value; }
        }

        //Way 1
        //private string _OutputFile;
        //internal string OutputFile
        //{
        //    get { return _OutputFile; }
        //    set { _OutputFile = value; }
        //}

        //Way 2
        public string _OutputFile;

        //main method
        internal void Parse(string fileName)
        {
            try
            {
                int selectLength = 200;

                string readFile = Path.GetFileName(fileName);
                string readText = File.ReadAllText(fileName);

                _completedCallback.Invoke(readFile);    //delegate invoke

                do
                {
                    if (readText.Substring(0, 3) != "ISA") break;

                    string firstTag = readText.Substring(0, selectLength);
                    string seperateChar = firstTag.Split('~').First().Last().ToString();    // "<"
                    string[] clmMatches = { "B" + seperateChar + "6", "B" + seperateChar + "7" };

                    MatchCollection foundMatch = Regex.Matches(readText, @"CLM", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    string searchStr = string.Empty;
                    bool hasPattern = false;
                    int length = 0;

                    if (foundMatch.Count > 0)
                    {
                        for (int i = 0; i < foundMatch.Count; i++)
                        {
                            length = readText.Length - (foundMatch[i].Index + selectLength) > 0 ? selectLength : readText.Length - foundMatch[i].Index;
                            searchStr = readText.Substring(foundMatch[i].Index, length);
                            hasPattern = clmMatches.Any(s => searchStr.Contains(s));
                            if (hasPattern)
                            {
                                Form1.Layout837 l = new Form1.Layout837();
                                l.Filename = readFile;

                                string[] seg = searchStr.Split('~');
                                for (int t = 0; t < seg.Length - 1; t++)
                                {
                                    string[] e1 = seg[t].Split('*');
                                    switch (t)
                                    {
                                        case 0:
                                            l.SubmitterId = e1[1];
                                            l.FrequencyType = e1[5].Last().ToString();
                                            break;
                                        default:
                                            if (e1[0] == "REF" && e1[1] == "D9") l.ClaimNumber = e1[2];
                                            if (e1[0] == "REF" && e1[1] == "F8") l.ReferenceNumber = e1[2];
                                            break;
                                    }
                                }
                                //layouts.Add(l);
                                WriteToFileThreadSafe(_OutputFile, l.ToString());
                            }
                        }
                    }
                } while (false);
            }
            catch (Exception ex)
            {
                //
            }
        }

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

    }
}

