using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using Emecca.Framework.Common;

namespace Emecca.Express.Common
{
    public partial class EmeccaSplashScreen : DevExpress.XtraEditors.XtraForm
    {
        private string message = "";
        private int progress = 0;
        private int timeout = 60 * 1000; 

        private object ret = null;

        private Thread threadToKill = null;

        public delegate object Executor();
        public Executor executor = null;

        public EmeccaSplashScreen()
        {
            InitializeComponent();

            EmeccaProfile profile = EmeccaRuntime.getInstance().getVariable((EmeccaRuntime.FrameworkReservedVariableLeading + ".EmeccaProfile")) as EmeccaProfile;
            string imgPath = profile.WorkDirectory + "\\RES\\ICONS\\SPLASHSCREEN.JPG";
            if (File.Exists(imgPath))
            {
                picSplashScreen.Image = new System.Drawing.Bitmap(imgPath);
            }


            //tmdiMain.MdiParent = null;

            
            lnfMain.LookAndFeel.SkinName = profile.LookAndFeel;

        }

        public object doExecute(Executor method,int timeout,bool supportCancel)
        {
            executor = method;
            this.cmdCancel.Visible = supportCancel;
            backgroundWorker1.RunWorkerAsync();
            ShowDialog();
            return ret;
        }
        public void doReportProgressive(string message, int progress)
        {
            this.message = message;
            this.progress = progress;
            backgroundWorker1.ReportProgress(progress);
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            CallWithTimeout(executor, timeout);
        }


        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BringToFront();
            messageInTask.Text = message;
            progressBarInTask.Value = progress;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            ret = "CANCEL";
            backgroundWorker1.CancelAsync();
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
                threadToKill.Abort();
                throw new TimeoutException();
            }
        } 

    }


}