using System;
using System.Collections;
using Emecca.Medical.Dicom.Model;
using Leadtools.Dicom;
using Leadtools.Dicom.Scu;

namespace Emecca.Medical.Dicom.Command
{
    public class MwlFind : CBase
    {
        private ArrayList result = new ArrayList();

        public override bool doVerify()
        {
            throw new NotSupportedException();
        }
        public ArrayList doExecute(ModalityWorklistDicomDataSet query)
        {
            result.Clear();
            Find(getCalledScp(), query, new DicomMatchDelegate<Object>(FoundMatch), query);
            return result;
        }
        public MwlFind(string callingAET,string callingHost,int callingPort, string calledAET, string calledHost, int calledPort, bool useTls)
            : base(callingAET,callingHost,callingPort, calledAET, calledHost, calledPort, useTls)
        {
        }

        private void FoundMatch(Object record, DicomDataSet ds)
        {
            result.Add(ds);
        }
    }
}
