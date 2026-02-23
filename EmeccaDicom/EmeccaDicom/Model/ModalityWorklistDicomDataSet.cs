using Leadtools.Dicom;
using Leadtools.Dicom.Common.Attributes;
using Leadtools.Dicom.Common.Extensions;
using System;

namespace Emecca.Medical.Dicom.Model
{
    [Instance(DicomClassType.ModalityWorklist, "1.2.840.10008.5.1.4.31")]
    public class ModalityWorklistDicomDataSet : DicomDataSet
    {
        public ModalityWorklistDicomDataSet(string xml)
        {
            this.Initialize(DicomClassType.ModalityWorklist, DicomDataSetInitializeType.ExplicitVRLittleEndian);
            this.LoadXml(xml, DicomDataSetLoadXmlFlags.None);
        }
    }
}
