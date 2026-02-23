using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Emecca.Main
{
    public partial class Form1 : Form
    {
        private string message = DateTime.Now.ToString("mmss");
        private bool running = true;
        private object ret = null;

        public delegate object Executor();
        public Executor executor = null;
        public Form1()
        {
            InitializeComponent();
        }

        public void doStartup(Executor method)
        {
            executor = method;
            backgroundWorker1.RunWorkerAsync();
            ShowDialog();
        }
        public void doReportProgressive(string message,int progress)
        {
            this.message = message;
            backgroundWorker1.ReportProgress(progress);
        }
        public void doShutdown()
        {
            running = false;
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label1.Text = message;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            running = false;
            Close();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //while (running)
            //{

            //if (executor != null)
            //{
            //    ret = executor();
            //}

            CallWithTimeout(executor, 11 * 1000);
            //    else
            //    {
            //        Thread.Sleep(1000);
            //    }
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            threadToKill.Abort();
            //backgroundWorker1.CancelAsync();
            //backgroundWorker1.CancelAsync();

        }

        Thread threadToKill = null; 
        void CallWithTimeout(Executor action, int timeoutMilliseconds) 
        { 
            
            Action wrappedAction = () => { 
                threadToKill = Thread.CurrentThread; 
                action(); 
            }; 
            
            IAsyncResult result = wrappedAction.BeginInvoke(null, null); 
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds)) 
            { wrappedAction.EndInvoke(result); 
            } else { 
                threadToKill.Abort(); 
                throw new TimeoutException(); 
            } 
        } 
    }
}
