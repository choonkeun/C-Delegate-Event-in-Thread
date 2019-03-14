using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EventTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Adder a = new Adder();
            a.OnEventFired += callback_method;       //set callback address

            txtEventFired.Text = string.Empty;
            int iAnswer = a.Add(int.Parse(txtLeft.Text), int.Parse(txtRight.Text));
            txtValue.Text = iAnswer.ToString();

            Console.WriteLine("iAnswer = {0}", iAnswer);
        }

        //callback can not access Form Field(label, textbox, ...), so, Use delegate
        private void callback_method(object sender, PassingValueByEventArgs e)
        {
            string sumValue = e.Total.ToString() + " from callback";
            UpdateStatus(sumValue);     //Invokde delegate
            //Console.WriteLine("Multiple of five reached: ", e.Total);
        }

#region delegate to display value to the TextBox
        private delegate void UpdateStatusDelegate(string status);
        void UpdateStatus(string status)
        {
            if (this.txtEventFired.InvokeRequired)
            {
                this.Invoke(new UpdateStatusDelegate(this.UpdateStatus), new object[] { status });
                return;
            }
            this.txtEventFired.Text = status;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtEventFired.Text = string.Empty;
        }
#endregion

    }

    public class Adder
    {
        public event EventHandler<PassingValueByEventArgs> OnEventFired;
        public int Add(int x, int y)
        {
            int iSum = x + y;
            if ((iSum % 5 == 0) && (OnEventFired != null))
            { 
                OnEventFired(this, new PassingValueByEventArgs(iSum)); 
            }
            return iSum;
        }
    }

    public class PassingValueByEventArgs : EventArgs
    {
        public PassingValueByEventArgs(int iTotal)
        { 
            Total = iTotal; 
        }
        public int Total { get; set; }
    }

}
