using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leadtools.Dicom.Scu.Common;
using Leadtools.Dicom;
using Emecca.Medical.Dicom.Command;
using Emecca.Medical.Dicom.Model;
using System.Collections;

namespace Emecca.Medical.Dicom.Client
{
    /// <summary>
    /// Performs query operations against DICOM servers.
    /// Refer to DICOM standard part 4 Annex C for understanding of how Query/Retrieve Service works
    /// Author: Alan Jiang
    /// Last Modifieid Fate:2012/9/2
    /// Version:V1.0
    /// As for example the Patient Root Query/Retrieve Information Model is based upon a four level hierarchy:
    /// Patient,Study,Series,Composite object instance(Image)
    /// </summary>
    public class EmeccaPacesQueryClient : CFind
    {
        public EmeccaPacesQueryClient(string callingAET, string callingHost, int callingPort, string calledAET, string calledHost, int calledPort, bool useTls)
            : base(callingAET, callingHost, callingPort, calledAET, calledHost, calledPort, useTls)
        {
        
        }

        /// <summary>
        /// 依照studyInstanceUID查詢,唯一
        /// </summary>
        /// <param name="studyInstanceUID"></param>
        /// <returns></returns>
        public DicomDataSet FindStudyByStudyInstanceUid(string studyInstanceUID)
        {
            DicomDataSet[] dataSets = FindStudiesByParameters(studyInstanceUID, null, null, null, null, null, null, null, null);
            return dataSets[0];
        }

        /// <summary>
        /// 依照StudyInstanceUid查出該Study下所有的影像數量
        /// </summary>
        /// <param name="studyInstanceUid"></param>
        /// <returns></returns>
        public int countImagesByStudyInstanceUid(string studyInstanceUid)
        {
            DicomDataSet[] retSeries = FindSeriesByStudyInstanceUid(studyInstanceUid);
            string seriesInstanceUid = null;
            DicomDataSet[] retImages = null;
            int imageCount = 0;
            foreach (DicomDataSet series in retSeries)
            {
                seriesInstanceUid = EmeccaDicomUtility.GetStringValue(series, DicomTag.SeriesInstanceUID);
                retImages = FindImagesInSeries(studyInstanceUid, seriesInstanceUid);
                imageCount += retImages.Length;
            }
            return imageCount;
        }


        /// <summary>
        /// 依照studyInstanceUID查詢Series，多筆
        /// </summary>
        /// <param name="studyInstanceUID"></param>
        /// <returns></returns>
        public DicomDataSet[] FindSeriesByStudyInstanceUid(string studyInstanceUID)
        {
            DicomDataSet[] dataSets = FindSeriesByParameters(studyInstanceUID, null);
            return dataSets;
        }

        /// <summary>
        /// 依照studyInstanceUID, seriesInstanceUID查詢Instance，多筆
        /// </summary>
        /// <param name="studyInstanceUID"></param>
        /// <param name="seriesInstanceUID"></param>
        /// <returns></returns>
        public DicomDataSet[] FindImagesInSeries(string studyInstanceUID, string seriesInstanceUID)
        {
            DicomDataSet[] dataSets = FindImagesByParameters(studyInstanceUID, seriesInstanceUID, null, null);
            return dataSets;
        }

        /// <summary>
        /// 依照病歷號碼查詢Study，多筆
        /// </summary>
        /// <param name="patientid"></param>
        /// <returns></returns>
        public DicomDataSet[] FindStudyByPatientId(string patientid)
        {
            DicomDataSet[] dataSets = FindStudiesByParameters(null, patientid, null, null, null, null, null, null, null);
            return dataSets;
        }

        /// <summary>
        /// 依照起迄StudyDate查詢所有的Study
        /// </summary>
        /// <param name="beginStudyDate"></param>
        /// <param name="endStudyDate"></param>
        /// <returns></returns>
        public DicomDataSet[] FindStudyBetweenStudyDate(string beginStudyDate, string endStudyDate)
        {
            DicomDataSet[] dataSets = FindStudiesByParameters(null, null, null, null, null, null, beginStudyDate, endStudyDate, null);
            return dataSets;
        }

        /// <summary>
        /// This value can be null or System.String.Empty.
        /// </summary>
        /// <param name="studyInstanceUID">The Study Instance UID System.String value to match the queried study against. This value can be null or System.String.Empty.</param>
        /// <param name="patientID">病歷號 The patient ID System.String value to match the queried study against. This value can be null or System.String.Empty.</param>
        /// <param name="otherPatientID">身分證號 The OtherPatient ID System.String value to match the queried study against. This value can be null or System.String.Empty.</param>
        /// <param name="patientName">The patient name System.String value to match the queried study against. This value can be null or System.String.Empty.</param>
        /// <param name="modalitiy">The modality System.String to match the queried study against. This value can be null or System.String.Empty.</param>
        /// <param name="accessionNumber">The accession number System.String value to match the queried study against. This value can be null or System.String.Empty.</param>
        /// <param name="studyDateStart">yyyyMMdd:The study starting date of a System.DateTime value type to match the queried study against. This value can be null or System.String.Empty.</param>
        /// <param name="studyDateEnd">yyyyMMdd:The study end date of a System.DateTime value type to match the queried study against. This value can be null or System.String.Empty.</param>
        /// <returns>DicomDataSet[]</returns>
        public DicomDataSet[] FindStudiesByParameters
        (
           string studyInstanceUID,
           string patientID,
           string otherPatientId,
           string patientName,
           string modality,
           string accessionNumber,
           string studyDateStart,
           string studyDateEnd,
           string characterSet
        )
        {
            DicomDataSet queryStudy = new CFindStudyRootDicomDataSet(@"C:\DWSv2011\CFindMinimumQueryTemplate.xml");
            if (!string.IsNullOrEmpty(studyInstanceUID))
            {
                 EmeccaDicomUtility.SetTag(queryStudy, DicomTag.StudyInstanceUID, studyInstanceUID);
            }
            if (!string.IsNullOrEmpty(patientID))
            {
                 EmeccaDicomUtility.SetTag(queryStudy, DicomTag.PatientID, patientID);
            }
            if (!string.IsNullOrEmpty(otherPatientId))
            {
                 EmeccaDicomUtility.SetTag(queryStudy, DicomTag.OtherPatientIDs, otherPatientId);
            }
            if (!string.IsNullOrEmpty(patientName))
            {
                 EmeccaDicomUtility.SetTag(queryStudy, DicomTag.PatientName, patientName);
            }
            if (!string.IsNullOrEmpty(modality))
            {
                 EmeccaDicomUtility.SetTag(queryStudy, DicomTag.Modality, modality);
            }
            if (!string.IsNullOrEmpty(accessionNumber))
            {
                 EmeccaDicomUtility.SetTag(queryStudy, DicomTag.AccessionNumber, accessionNumber);
            }
            if (!string.IsNullOrEmpty(studyDateStart) || !string.IsNullOrEmpty(studyDateEnd))
            {
                string studyDate = (studyDateStart + "-" + studyDateEnd).Trim();
                if (studyDate.StartsWith("-") || studyDate.EndsWith("-"))
                {
                    studyDate = studyDate.Replace("-", "");
                }
                EmeccaDicomUtility.SetTag(queryStudy, DicomTag.StudyDate, studyDate);
            }
            if (string.IsNullOrEmpty(characterSet))
            {
                characterSet = _sSpecificCharacterSet_UTF8; //預設UTF-8
            }
            EmeccaDicomUtility.SetTag(queryStudy, DicomTag.SpecificCharacterSet, characterSet);
            ArrayList result = doExecute(queryStudy);
            DicomDataSet[] studyDataSets = result.ToArray(typeof(DicomDataSet)) as DicomDataSet[];
            return studyDataSets;
        }

        /// <summary>
        /// </summary>
        /// <param name="studyInstanceUID">The Study Instance UID System.String value to match the queried study against. This value can be null or System.String.Empty.</param>
        /// <returns></returns>
        public DicomDataSet[] FindSeriesByParameters(string studyInstanceUID, string characterSet)
        {
            DicomDataSet querySeries = new CFindSeriesRootDicomDataSet(@"C:\DWSv2011\CFindMinimumQueryTemplate.xml");
            if (!string.IsNullOrEmpty(studyInstanceUID))
            {
                EmeccaDicomUtility.SetTag(querySeries, DicomTag.StudyInstanceUID, studyInstanceUID);
            }
            if (string.IsNullOrEmpty(characterSet))
            {
                characterSet = _sSpecificCharacterSet_UTF8; //預設UTF-8
            }
            EmeccaDicomUtility.SetTag(querySeries, DicomTag.SpecificCharacterSet, characterSet);
            ArrayList result = doExecute(querySeries);
            DicomDataSet[] seriesDataSets = result.ToArray(typeof(DicomDataSet)) as DicomDataSet[];
            return seriesDataSets;
        }

        /// <summary>
        /// 查詢Study,Series下的所有Image，或指定Instance，取得唯一影像
        /// </summary>
        /// <param name="studyInstanceUID">The Study Instance UID System.String value to match the queried study against. This value can be null or System.String.Empty.</param>
        /// <param name="seriesInstanceUID">The Series Instance UID System.String value to match the queried study against. This value can be null or System.String.Empty.</param>
        /// <param name="sopInstanceUID">The SOP Instance UID System.String value to match the queried study against. This value can be null or System.String.Empty.</param>
        /// <returns></returns>
        public DicomDataSet[] FindImagesByParameters(string studyInstanceUID, string seriesInstanceUID, string sopInstanceUID, string characterSet)
        {
            DicomDataSet queryInstance = new CFindInstanceRootDicomDataSet(@"C:\DWSv2011\CFindMinimumQueryTemplate.xml");
            if (!string.IsNullOrEmpty(studyInstanceUID))
            {
                EmeccaDicomUtility.SetTag(queryInstance, DicomTag.StudyInstanceUID, studyInstanceUID);
            }
            if (!string.IsNullOrEmpty(seriesInstanceUID))
            {
                EmeccaDicomUtility.SetTag(queryInstance, DicomTag.SeriesInstanceUID, seriesInstanceUID);
            }
            if (!string.IsNullOrEmpty(sopInstanceUID))
            {
                EmeccaDicomUtility.SetTag(queryInstance, DicomTag.SOPInstanceUID, sopInstanceUID);
            }
            if (string.IsNullOrEmpty(characterSet))
            {
                characterSet = _sSpecificCharacterSet_UTF8; //預設UTF-8
            }
            EmeccaDicomUtility.SetTag(queryInstance, DicomTag.SpecificCharacterSet, characterSet);
            ArrayList result = doExecute(queryInstance);
            DicomDataSet[] instanceDataSets = result.ToArray(typeof(DicomDataSet)) as DicomDataSet[];
            return instanceDataSets;
        }

    }
}
