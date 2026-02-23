
/**
 * Usage :
 * 1. Create CMove Instance
 * 2. Pass Target Scp
 * 3. Pass Study indentity / Critertia
 * 4. Return Status While executeing complete
 * */
using System.Collections;
using Leadtools.Dicom.Scu;
using Leadtools.Dicom.Scu.Common;
using System;
namespace Emecca.Medical.Dicom.Command
{
    public class CMove : CFind
    {
        private ArrayList result = new ArrayList();

        public CMove(string callingAET, string callingHost, int callingPort, string calledAET, string calledHost, int calledPort, bool useTls)
            : base(callingAET, callingHost, callingPort, calledAET, calledHost, calledPort, useTls)
        {
        }
        public void doExecute(string destTitle,string studyInstanceUid)
        {
            Move(getCalledScp(), destTitle, studyInstanceUid);
        }
        public void doExecute(string destTitle, string studyInstanceUid,string seriesInstanceUid)
        {
            Move(getCalledScp(), destTitle, studyInstanceUid, seriesInstanceUid);
        }
        public void doExecute(string destTitle, string studyInstanceUid, string seriesInstanceUid, string sopInstanceUid)
        {
            Move(getCalledScp(), destTitle, studyInstanceUid, seriesInstanceUid, sopInstanceUid);
        }
    }
}
