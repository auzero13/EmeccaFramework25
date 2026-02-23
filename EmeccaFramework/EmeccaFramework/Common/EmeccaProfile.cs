
namespace Emecca.Framework.Common
{
    public class EmeccaProfile
    {
        private string createDateTime = null;

        public string CreateDateTime
        {
            get { return createDateTime; }
            set { createDateTime = value; }
        }

        private string systemXml = null;

        public string SystemXml
        {
            get { return systemXml; }
            set { systemXml = value; }
        }

        private string logXml = null;

        public string LogXml
        {
            get { return logXml; }
            set { logXml = value; }
        }

        private string lookAndFeel = null;

        public string LookAndFeel
        {
            get { return lookAndFeel; }
            set { lookAndFeel = value; }
        }

        private string workDirectory = null;

        public string WorkDirectory
        {
            get { return workDirectory; }
            set { workDirectory = value; }
        }

        private string defaultLoginAccount = null;

        public string DefaultLoginAccount
        {
            get { return defaultLoginAccount; }
            set { defaultLoginAccount = value; }
        }
        private string defaultLoginPassword = null;

        public string DefaultLoginPassword
        {
            get { return defaultLoginPassword; }
            set { defaultLoginPassword = value; }
        }

        //顯示在哪塊銀幕上
        private int displayScreen;

        public int DisplayScreen
        {
            get { return displayScreen; }
            set { displayScreen = value; }
        }

        private string enableAuthByAD;

        public string EnableAuthByAD
        {
            get { return enableAuthByAD; }
            set { enableAuthByAD = value; }
        }
    }
}
