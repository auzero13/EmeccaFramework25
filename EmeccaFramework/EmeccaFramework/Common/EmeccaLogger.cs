using System.IO;
using log4net;

namespace Emecca.Framework.Common
{
    public class EmeccaLogger 
    {
        private static string configXml = null;
        
        /// <summary>
        /// 各系統在系統啟動時呼叫，傳入Log的XML設定檔位置
        /// EmeccaLogger.doInitial(systemData.WorkDirectory + "\\CONF\\ImsLog.config");
        /// </summary>
        /// <param name="file"></param>
        public static void doInitial(string configXmlPath)
        {
            if (configXml == null)
            {
                configXml = configXmlPath;
                log4net.Config.XmlConfigurator.Configure(new FileInfo(configXml));
            }
        }

        public static ILog getLogger(string name)
        {
            return LogManager.GetLogger(name); 
        }

        public static ILog getEmeccaLogger()
        {
            const string loggerName = "EmeccaLogger";
            return LogManager.GetLogger(loggerName);
        }
    }
}
