using Leadtools.Dicom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emecca.Medical.Dicom.Model
{
    public class TmpDicomDataSet: DicomDataSet
    {
        public static long GetSeriesSeriesInstanceUID()
        {
            return DicomTag.SeriesInstanceUID;
        }
    }
}
