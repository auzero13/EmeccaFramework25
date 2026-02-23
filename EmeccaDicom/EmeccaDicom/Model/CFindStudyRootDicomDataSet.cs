using Leadtools.Dicom;
using Leadtools.Dicom.Common.Attributes;
using Leadtools.Dicom.Common.Extensions;

namespace Emecca.Medical.Dicom.Model
{
    [Instance(DicomClassType.StudyRootQueryStudy, "1.2.840.10008.5.1.4.1.2.2.1")]
    public class CFindStudyRootDicomDataSet : DicomDataSet
    { 
        public CFindStudyRootDicomDataSet(string xml)
        {
            this.Initialize(DicomClassType.StudyRootQueryStudy, DicomDataSetInitializeType.ExplicitVRLittleEndian);
            
            this.LoadXml(xml, DicomDataSetLoadXmlFlags.None);
            EmeccaDicomUtility.SetTag(this, 0x00080052, "STUDY");
        }
    }
}
