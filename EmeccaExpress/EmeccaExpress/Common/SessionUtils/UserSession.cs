using System.Timers;
using System;
using Emecca.Framework.Common;

namespace Emecca.Express.Common.SessionUtils
{
    public class UserSession
    {
        public static int SessionTimeoutMinutes;

        delegate void SimpleDelegate();
        private static bool sessionAlive;
        private static Timer usertimer;

        public static bool SessionAlive
        {
            get { return sessionAlive; }
            set { sessionAlive = value; }
        }


        public static void BeginTimer()
        {
            try
            {
                SessionAlive = true;
                //usertimer.Start();
                if (SessionTimeoutMinutes > 0)
                {
                    usertimer = new Timer(1000 * 60 * SessionTimeoutMinutes);
                    usertimer.Enabled = true;
                    usertimer.AutoReset = false;
                    EmeccaForm.SessionTimeoutMiuntes = SessionTimeoutMinutes.ToString();
                    usertimer.Elapsed += new ElapsedEventHandler(DisposeSession);
                }
            }
            catch
            {
                return;
            }
        }

        private static void DisposeSession(object source, ElapsedEventArgs e)
        {
            try
            {
                SessionAlive = false;
              
                EmeccaMainFrame.getInstance().showLockForm("");
                }
                catch (Exception ee)
                {
                    EmeccaLogger.getEmeccaLogger().Error("getEmeccaForm " + ee.Message, ee);
                    EmeccaMessageBox.Show("初始化視窗錯誤:" + ee.Message, "承啓醫系科室管理系統");
                }
                //frm.ShowDialog();
        }

        public static void ResetTimer()
        {
            try
            {
                if (SessionTimeoutMinutes > 0)
                {
                    SessionAlive = true;
                    usertimer.Stop();
                    usertimer.Start();
                }
            }
            catch
            {
                return;
            }
        }

        public void StartSession()
        {
            try
            {
                UserSession.BeginTimer();
            }
            catch
            {

            }
        }

        public static void SuspendTimer()
        {
            if (usertimer != null)
            {
                usertimer.Stop();
            }
        }

        public static void ResetInterval(int minutes)
        {
           
            SessionTimeoutMinutes = minutes;
            if (minutes > 0)
            {
                if (usertimer != null)
                {
                    usertimer.Interval = 1000 * 60 * SessionTimeoutMinutes;
                }
                else
                {
                    BeginTimer();
                }
            }
            else
            {
                if (usertimer != null)
                {
                    usertimer.Stop();
                }
            }
        }
    }
}
