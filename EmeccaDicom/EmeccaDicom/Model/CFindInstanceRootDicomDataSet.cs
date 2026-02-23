using Leadtools.Dicom;
using Leadtools.Dicom.Common.Attributes;
using Leadtools.Dicom.Common.Extensions;

namespace Emecca.Medical.Dicom.Model
{
    [Instance(DicomClassType.StudyRootQueryImage, "1.2.840.10008.5.1.4.1.2.2.1")]
    public class CFindInstanceRootDicomDataSet : DicomDataSet
    {
        public CFindInstanceRootDicomDataSet(string xml)
        {
            this.Initialize(DicomClassType.StudyRootQueryImage, DicomDataSetInitializeType.ExplicitVRLittleEndian);
            this.LoadXml(xml, DicomDataSetLoadXmlFlags.None);
            EmeccaDicomUtility.SetTag(this, DicomTag.QueryRetrieveLevel, "IMAGE");
        }
    }
}
