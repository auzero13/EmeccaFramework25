using System;
using System.ComponentModel;
using System.Threading;
using Emecca.Framework.Common;
using System.IO;

namespace Emecca.Express.Common
{
    public partial class EmeccaLongJob : EmeccaForm
    {
        private DateTime startTimestamp = DateTime.Now;

        private int timeout = 15 * 1000;

        private object ret = null;

        Thread threadToKill = null;

        /*
         * 定義四種委託類型，適應各種方法
         * 1.有返回值，無參數
         * 2.有返回值，有參數
         * 3.無返回值，無參數
         * 4.無返回值，有參數
         */
        //public delegate void Executor();
        public delegate object Executor();
       
        /// <param name="objs">object[]</param>
        //public delegate void Executor(object[] objs);
        //public delegate object Executor(object[] objs);


        public Executor executor = null;

        public EmeccaLongJob()
        {
            InitializeComponent();
            EmeccaProfile profile = EmeccaRuntime.getInstance().getVariable((EmeccaRuntime.FrameworkReservedVariableLeading + ".EmeccaProfile")) as EmeccaProfile;
           string workingDir =EmeccaRuntime.getInstance().getVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".WorkingDir") as string ;
           string assemblyName = EmeccaRuntime.getInstance().getVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".AssemblyName") as string;
            string imgPath = workingDir + "\\" + assemblyName + "\\RES\\LOADING.GIF";
            if (File.Exists(imgPath))
            {
                picLoading.Image = new System.Drawing.Bitmap(imgPath);
            }
        }
        public object doExecute(Executor method,string message,int timeout)
        {
            startTimestamp = DateTime.Now;
            tmDuration.Start();
            lblMessage.Text = message;
            this.timeout = timeout;
            executor = method;
            backgroundWorker1.RunWorkerAsync();
            ShowDialog();
            return ret;
        }
        private void doReportProgressive(string message, int progress)
        {
            backgroundWorker1.ReportProgress(progress);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (executor == null)
            {
                executor = NullExecutor;
            }

            CallWithTimeout(executor, timeout);

        }

        private object NullExecutor()
        {
            Thread.Sleep(timeout);
            return null;
        }

        private object NullExecutor(object[] objs){
            Thread.Sleep(timeout);
            return null;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BringToFront();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close();
        }

        private void CallWithTimeout(Executor action, int timeoutMilliseconds)
        {

            Action wrappedAction = () =>
            {
                threadToKill = Thread.CurrentThread;
                ret = action();
            };

            IAsyncResult result = wrappedAction.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                wrappedAction.EndInvoke(result);
            }
            else
            {
                //threadToKill.Abort();
                backgroundWorker1.CancelAsync();
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            //threadToKill.Abort();
            backgroundWorker1.CancelAsync();
            Close();
        }

        private void tmDuration_Tick(object sender, EventArgs e)
        {
            lblTimer.Text = System.DateTime.Now.Subtract(startTimestamp).Seconds.ToString("0000");
        }
    }
}