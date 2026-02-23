using System;
using System.Collections;
using Emecca.Medical.Dicom.Model;
using Leadtools.Dicom;
using Leadtools.Dicom.Scu;
using Leadtools.Dicom.Common.Attributes;
using Leadtools.Dicom.Common.Extensions;
using Leadtools.Dicom.Scu.Common;
using Emecca.Framework.Common;
/**
 * Usage :
 * 1. Create CFind Instance
 * 2. Pass Target Scp
 * 3. Pass Criteria
 * 4. Return Status While executeing complete
 * 5. Retrun Dicom Find Result
 * */
namespace Emecca.Medical.Dicom.Command
{
    public class CFind : CBase
    {
        private ArrayList result = new ArrayList();
        private ArrayList Seriesresult = new ArrayList();
        public int imagescount = 0,movecount=0;

        public CFind(string callingAET, string callingHost, int callingPort, string calledAET, string calledHost, int calledPort, bool useTls)
            : base(callingAET, callingHost, callingPort, calledAET, calledHost, calledPort, useTls)
        {
            this.MatchSeries += new Leadtools.Dicom.Scu.Common.MatchSeriesDelegate(_find_MatchSeries);
            this.MatchStudy += new Leadtools.Dicom.Scu.Common.MatchStudyDelegate(_find_MatchStudy);
            this.Moved += new Leadtools.Dicom.Scu.Common.MovedDelegate(_find_Moved);
        }
        public ArrayList doExecute(DicomDataSet query)
        {
            result.Clear();
            Find(getCalledScp(), query, new DicomMatchDelegate<Object>(FoundMatch), query);
            return result;
        }
        private void FoundMatch(Object record, DicomDataSet ds)
        {
            result.Add(ds);
        }
        //20130807 Added by POGI Query Study
        public ArrayList doQuery(String accessNo)
        {
            result.Clear();
            FindQuery query = new FindQuery();

            query.AccessionNumber = accessNo;
            try
            {
                Find(getCalledScp(), query);
            }
            catch(Exception ee)
            {
                throw new EmeccaException("查詢pacs資料出錯" + ee.Message);
            }
            return result;
        }
        //20130807 Added by POGI Query Series
        public ArrayList doQuery(String StudyUID, String accessNo)
        {
            Seriesresult.Clear();
            FindQuery query = new FindQuery();

            query.QueryLevel = QueryLevel.Series;
            query.StudyInstanceUID = StudyUID;            
            query.AccessionNumber = accessNo;
            try
            {
                Find(getCalledScp(), query);
            }
            catch (Exception ee)
            {
                throw new EmeccaException("查詢pacs資料出錯" + ee.Message);
            }
            return Seriesresult;
        }
        public void _find_MatchStudy(object sender, Leadtools.Dicom.Scu.Common.MatchEventArgs<Study> e)
        {
            result.Add(e.Dataset);
        }
        public void _find_MatchSeries(object sender, Leadtools.Dicom.Scu.Common.MatchEventArgs<Series> e)
        {
            AddSeriesItem(e);
            Seriesresult.Add(e.Dataset);
        }
        private void AddSeriesItem(MatchEventArgs<Series> e)
        {
            if (e.Info.NumberOfRelatedInstances>0)
                imagescount += e.Info.NumberOfRelatedInstances;
        }
        void _find_Moved(object sender, Leadtools.Dicom.Scu.Common.MovedEventArgs e)
        {
            result.Add(e.Dataset);
            ReceiveFindMoved(e);
            movecount++;
        }
        public delegate void ReceiveFindMovedDelegate(Leadtools.Dicom.Scu.Common.MovedEventArgs e);
        void ReceiveFindMoved(Leadtools.Dicom.Scu.Common.MovedEventArgs e)
        {
            if (e.Instance.InstanceType == InstanceLevel.Image)
            {
                movecount++;
            }
        }
    }

}
