using Leadtools.Dicom;
using Leadtools.Dicom.Common.Attributes;
using Leadtools.Dicom.Common.Extensions;

namespace Emecca.Medical.Dicom.Model
{
    [Instance(DicomClassType.StudyRootQuerySeries, "1.2.840.10008.5.1.4.1.2.2.1")]
    public class CFindSeriesRootDicomDataSet : DicomDataSet
    {
        public CFindSeriesRootDicomDataSet(string xml)
        {
            this.Initialize(DicomClassType.StudyRootQuerySeries, DicomDataSetInitializeType.ExplicitVRLittleEndian);
            this.LoadXml(xml, DicomDataSetLoadXmlFlags.None);
            EmeccaDicomUtility.SetTag(this, DicomTag.QueryRetrieveLevel, "SERIES");
        }
    }
}
