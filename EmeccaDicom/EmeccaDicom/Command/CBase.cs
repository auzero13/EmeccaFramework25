using System.Net;
using Leadtools.Dicom;
using Leadtools.Dicom.Scu;
using System;

namespace Emecca.Medical.Dicom.Command
{
    public class CBase : QueryRetrieveScu
    {

        public const string _sConfigurationImplementationClass = "1.2.840.114257.1123456";
        public const string _sConfigurationImplementationVersionName = "Emecca";
        public const string _sConfigurationProtocolVersion = "2011";
        public const string _sSpecificCharacterSet_UTF8 = "ISO_IR 192";
        
        protected bool useTls = false;
        protected string callingAET = null;
        protected string callingHost = null;
        protected int callingPort = 0;

        protected string calledAET = null;
        protected string calledHost = null;
        protected int calledPort = 0;



        //protected DicomScp calledScp = null;

        public CBase(string callingAET, string callingHost, int callingPort, string calledAET, string calledHost, int calledPort, bool useTls)
        {
            this.callingAET = callingAET;
            this.callingHost = callingHost;
            this.callingPort = callingPort;

            
            this.calledAET = calledAET;
            this.calledHost = calledHost;
            this.calledPort = calledPort;
            this.useTls = useTls;

            
            base.AETitle = callingAET;
        }

        protected DicomScp getCalledScp()
        {
            DicomScp calledScp = new DicomScp(IPAddress.Parse(calledHost), calledAET, calledPort);
            return calledScp;
        }

        public virtual bool doVerify()
        {
            EmeccaDicomUtility.Log("DOING DICOM VERIFY!!");
            bool result = false;
            result = base.Verify(getCalledScp());
            return result;
        }

        //protected override void OnReceiveCFindResponse(byte presentationID, int messageID, string affectedClass, DicomCommandStatusType status, DicomDataSet dataSet)
        //{
        //    if (status == DicomCommandStatusType.PendingWarning)
        //        status = DicomCommandStatusType.Pending;
        //}
        //protected override void OnReceiveAssociateAccept(DicomAssociate association)
        //{
        //    base.OnReceiveAssociateAccept(association);
        //}
        //protected override void OnReceiveAssociateRequest(DicomAssociate association)
        //{
        //    base.OnReceiveAssociateRequest(association);
        //}
        //protected override void OnReceiveReleaseRequest()
        //{
        //    base.OnReceiveReleaseRequest();
        //}
    }
}
