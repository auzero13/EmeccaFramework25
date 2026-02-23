
using System;
using System.Collections;
using System.Net;
using Leadtools.Dicom;
/**
 * Usage :
 * 1. Create CStore Instance
 * 2. Pass Target Scp
 * 3. Pass List of DicomDataSet
 * 4. Return Status While executeing complete
 * */
using Leadtools.Dicom.Scu;
using Leadtools.Dicom.Scu.Common;
using System.IO;

namespace Emecca.Medical.Dicom.Command
{
    public class CStore : StoreScu
    {
        public const string _sConfigurationImplementationClass = "1.2.840.114257.1123456";
        public const string _sConfigurationImplementationVersionName = "Emecca";
        public const string _sConfigurationProtocolVersion = "2012";
        //private const string _sConfigurationImplementationClass = "1.2.840.114257.1123456";
        //private const string _sConfigurationImplementationVersionName = "1";
        //private const string _sConfigurationProtocolVersion = "1";
        protected bool useTls = false;
        protected string callingAET = null;
        protected string callingHost = null;
        protected int callingPort = 0;
        protected string calledAET = null;
        protected string calledHost = null;
        protected int calledPort = 0;
        protected DicomScp calledScp = null;
        public const string releaseDate = "20121225";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callingAET"></param>
        /// <param name="calledAET"></param>
        /// <param name="calledHost"></param>
        /// <param name="calledPort"></param>
        /// <param name="useTls"></param>
        /// <param name="logFile">如果有值且存在，則輸出DebugLog</param>
        public CStore(string callingAET, string calledAET, string calledHost, int calledPort, bool useTls, string logFile)
        {
            this.callingAET = callingAET;
            this.calledAET = calledAET;
            this.calledHost = calledHost;
            this.calledPort = calledPort;
            this.useTls = useTls;
            calledScp = new DicomScp(IPAddress.Parse(calledHost), calledAET, calledPort);
            AETitle = callingAET;
            MaxLength = 46726;
            EnableDebugLog = false;
            //Added By Alan 如果需要，指定輸出的Debug檔案
            if (!string.IsNullOrEmpty(logFile))
            {
                if (File.Exists(logFile))
                {
                    EnableDebugLog = true;
                    DebugLogFilename = logFile;
                }
            }
            //General Optimizations for sending and receiving data using our Dicom tools
            //1.Avoid compressing or decompressing during the image transfer. To do this, have the image compressed at the modality when possible. Set the Compression property to “Native” (meaning send as is).
            //Modified By Alan 20120316 把Compression.Lossless改成Native
            Compression = Leadtools.Dicom.Scu.Common.Compression.Native;
            ImplementationClass = _sConfigurationImplementationClass;
            ImplementationVersionName = _sConfigurationImplementationVersionName;
            ProtocolVersion = _sConfigurationProtocolVersion;

            BeforeCStore += beforeCStore;
            AfterCStore += afterCStore;
        }

        bool result = false;
        public bool doVerify()
        {
            result = false;
            result = base.Verify(calledScp);
            return result;
        }

        /// <summary>
        /// If the stayConnected parameter is true multiple datasets can be sent to the Scp without disconnecting. 
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="stayConnected"></param>
        /// <returns></returns>
        public bool doExecute(DicomDataSet ds, bool stayConnected)
        {
            int retry = 0;
            result = false;
            while ( retry < 3 && result == false )
            {
                try
                {
                    Store(calledScp, ds, stayConnected);
                }
                catch (Exception e)
                {
                    Log("Retry Store:{0}, Exception Message:{1}", new string[2] { retry.ToString(), e.Message });
                    Console.WriteLine(string.Format("Retry Store:{0}, Exception Message:{1}", retry.ToString(), e.Message));
                }
                retry++;
            }
            return result;
        }

        /// <summary>
        /// If the stayConnected parameter is true multiple datasets can be sent to the Scp without disconnecting. 
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="stayConnected"></param>
        /// <returns></returns>
        public bool doExecuteWithLog(DicomDataSet ds, bool stayConnected, string logName)
        {
            if (File.Exists(logName))
            {
                DebugLogFilename = logName;
            }
            doExecute(ds, stayConnected);
            
            return result;
        }


        void beforeCStore(object sender, BeforeCStoreEventArgs e)
        {
        }

        void afterCStore(object sender, AfterCStoreEventArgs e)
        {
            if (e.Status == DicomCommandStatusType.Success )
            {
                result = true;
            }
        }
    }
}
