using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Emecca.Framework.Common;
using Emecca.Express.Common;
using System.Threading;
using Emecca.Main;
using log4net;
using System.IO;
using Emecca.Medical.Dicom;
using Emecca.Medical.Dicom.Client;
using Emecca.Medical.Dicom.Command;
using Emecca.Medical.Dicom.Model;
using System.Collections;
using Leadtools.Dicom.Scu;
using Leadtools.Dicom.Scu.Common;
using System.Net;
using Leadtools.Dicom;
using Leadtools.Dicom.AddIn.Common;
using Leadtools.Medical.Workstation.Client.Pacs;
using Leadtools.Medical.Workstation.Client;

namespace EmeccaMain
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            EmeccaDicomEngine.doStartup();
            //DemoEmeccaDicomQueryClient();
            //DemoEmeccaCFindQuery();
            //demoPacsQueryClient();
            //StoreDirectory();
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Emecca.Demo.FrmDicomPing());
            EmeccaDicomEngine.doShutdown();
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
            
        //    try
        //    {
        //        EmeccaRuntime runtime = EmeccaRuntime.getInstance();
        //        // TODO : plan to set some environment variable for application setting
        //        MessageBox.Show(Environment.GetEnvironmentVariable("EmeccaHome"));
        //        EmeccaProfile profile = runtime.doInitial(@"C:\Emecca", @"DEMOv2011", @"Emecca.DEMOv2011");

        //        EmeccaExpressLoader loader = new EmeccaExpressLoader();

        //        EmeccaMainFrame mainFrame = EmeccaMainFrame.getInstance();

        //        if ("PASS".Equals(loader.doExecute()))
        //        {
        //            EmeccaLongJob job = new EmeccaLongJob();
        //            job.doExecute(null, "JUST FOR DEMO ^^", 10000);
        //            Application.Run(new Emecca.Demo.FrmMain());
        //        }
        //        else
        //        {
        //            Application.Exit();
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee);
        //    }



        }

        /// <summary>
        /// 用EmeccaDicomQueryClient進行Query
        /// see:http://www.leadtools.com/help/leadtools/v175/dh/to/leadtools.topics~leadtools.topics.programmingwithpacsclientframework.html
        /// </summary>
        public static void DemoEmeccaDicomQueryClient() 
        {
            try
            {
                EmeccaPacesQueryClient client = new EmeccaPacesQueryClient("TEST210", "127.0.0.1", 104, "QC", "192.168.8.82", 104, false);
                DicomDataSet[] retStudies = client.FindStudyByPatientId("16954039");
                //DicomDataSet[] retStudies = client.FindStudyBetweenStudyDate("20110802", "20120831");
                Console.WriteLine("Study Count:--->" + retStudies.Length);
                string studyInstanceUid = null;
                string seriesInstanceUid = null;

                foreach (DicomDataSet study in retStudies)
                {
                    studyInstanceUid = EmeccaDicomUtility.GetStringValue(study, DicomTag.StudyInstanceUID);
                    Console.WriteLine("StudyInstanceUid-->"+studyInstanceUid);
                    DicomDataSet[] retSeries = client.FindSeriesByStudyInstanceUid(studyInstanceUid);
                    Console.WriteLine("Series Count:--->" + retSeries.Length);
                    int imageCountInStudy = client.countImagesByStudyInstanceUid(studyInstanceUid);
                    Console.WriteLine("Image Count:--->" + imageCountInStudy);
                    foreach (DicomDataSet series in retSeries)
                    {
                        seriesInstanceUid = EmeccaDicomUtility.GetStringValue(series, DicomTag.SeriesInstanceUID);
                        Console.WriteLine("SeriesInstanceUID-->" + seriesInstanceUid);
                        DicomDataSet[] retImages = client.FindImagesInSeries(studyInstanceUid, seriesInstanceUid);
                        Console.WriteLine("Images Count:--->" + retImages.Length);
                        int count = 0;
                        foreach (DicomDataSet image in retImages)
                        {
                            count++;
                            Console.WriteLine("#{0}: SOPInstanceUID: {1}", count,EmeccaDicomUtility.GetStringValue(image, DicomTag.SOPInstanceUID));
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 直接用C-Find進行Query
        /// </summary>
        public static void DemoEmeccaCFindQuery()
        {
            try
            {
                CFind find = new CFind("TEST210", "127.0.0.1", 104, "QC", "192.168.8.82", 104, false);
                CFindStudyRootDicomDataSet query = new CFindStudyRootDicomDataSet(@"C:\DWSv2011\CFindMinimumQueryTemplate.xml");
                EmeccaDicomUtility.SetTag(query, DicomTag.StudyDate, "20120831-20120831"); // 檢查日期
                EmeccaDicomUtility.SetTag(query, DicomTag.StudyInstanceUID, "1.2.68.0.1.6.20100001.21801205051027461");
                EmeccaDicomUtility.SetTag(query, DicomTag.SeriesInstanceUID, "1.2.840.114257.20120827133933.762824494.238136658");
                ArrayList result = find.doExecute(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void StoreDirectory()
        {
            try
            {
                StoreScu storeDirectory = new StoreScu();
                storeDirectory.AETitle = "ALAN";
                storeDirectory.HostPort = 104;
                storeDirectory.HostAddress = IPAddress.Parse("192.168.8.160");
                DicomScp scp = new DicomScp(IPAddress.Parse("192.168.8.82"), "QC", 104);
                scp.Timeout = 30;
                storeDirectory.BeforeAssociateRequest += new BeforeAssociationRequestDelegate(storeDirectory_BeforeAssociateRequest);
                storeDirectory.AfterAssociateRequest += new AfterAssociateRequestDelegate(storeDirectory_AfterAssociateRequest);
                storeDirectory.BeforeCStore += new BeforeCStoreDelegate(storeDirectory_BeforeCStore);
                storeDirectory.AfterCStore += new AfterCStoreDelegate(storeDirectory_AfterCStore);
                storeDirectory.Compression = Compression.Native;
                //storeDirectory.Store(scp,@"C:\DWSv2011\Dicom\1.2.840.114257.634578351840156250.1245930406.905705180","*.*", false);
                //storeDirectory.Store(scp, @"C:\DWSv2011\Dicom\1.2.840.114257.634578351840156250.1245930406.905705180", "*.*", true);
                DicomDataSet ds = new DicomDataSet();
                for (int i = 0; i < 2; i++) 
                {
                    ds.Load(@"C:\DWSv2011\Dicom\1.2.840.114257.634578351840156250.1245930406.905705180\1.2.840.114257.634578352014062500.1880949687.1904867773.DCM", DicomDataSetLoadFlags.ExplicitVR | DicomDataSetLoadFlags.LittleEndian);
                    storeDirectory.Store(scp, ds, true);
                    ds.Load(@"C:\DWSv2011\Dicom\1.2.840.114257.634578351840156250.1245930406.905705180\1.2.840.114257.634578352047187500.877033354.1277095449.DCM", DicomDataSetLoadFlags.ExplicitVR | DicomDataSetLoadFlags.LittleEndian);
                    storeDirectory.Store(scp, ds, true);
                    ds.Load(@"C:\DWSv2011\Dicom\1.2.840.114257.634578351840156250.1245930406.905705180\1.2.840.114257.634578352049843750.1394179380.1712977928.DCM", DicomDataSetLoadFlags.ExplicitVR | DicomDataSetLoadFlags.LittleEndian);
                    storeDirectory.Store(scp, ds, true);
                    ds.Load(@"C:\DWSv2011\Dicom\1.2.840.114257.634578351840156250.1245930406.905705180\1.2.840.114257.634578352052187500.2103129832.1465967278.DCM", DicomDataSetLoadFlags.ExplicitVR | DicomDataSetLoadFlags.LittleEndian);
                    storeDirectory.Store(scp, ds, true);
                    ds.Load(@"C:\DWSv2011\Dicom\1.2.840.114257.634578351840156250.1245930406.905705180\1.2.840.114257.634578352053750000.1144107702.585465629.DCM", DicomDataSetLoadFlags.ExplicitVR | DicomDataSetLoadFlags.LittleEndian);
                    storeDirectory.Store(scp, ds, true);
                    ds.Load(@"C:\DWSv2011\Dicom\1.2.840.114257.634578351840156250.1245930406.905705180\1.2.840.114257.634578352055468750.89183359.1120152368.DCM", DicomDataSetLoadFlags.ExplicitVR | DicomDataSetLoadFlags.LittleEndian);
                    storeDirectory.Store(scp, ds, true);
                    ds.Load(@"C:\DWSv2011\Dicom\1.2.840.114257.634578351840156250.1245930406.905705180\1.2.840.114257.634578352056875000.1373547089.971945978.DCM", DicomDataSetLoadFlags.ExplicitVR | DicomDataSetLoadFlags.LittleEndian);
                    storeDirectory.Store(scp, ds, true);
                    ds.Load(@"C:\DWSv2011\Dicom\1.2.840.114257.634578351840156250.1245930406.905705180\1.2.840.114257.634578352058125000.606329385.1556034847.DCM", DicomDataSetLoadFlags.ExplicitVR | DicomDataSetLoadFlags.LittleEndian);
                    storeDirectory.Store(scp, ds, true);
                    ds.Load(@"C:\DWSv2011\Dicom\1.2.840.114257.634578351840156250.1245930406.905705180\1.2.840.114257.634578352059531250.1890693115.1407828457.DCM", DicomDataSetLoadFlags.ExplicitVR | DicomDataSetLoadFlags.LittleEndian);
                    storeDirectory.Store(scp, ds, false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void storeDirectory_BeforeAssociateRequest(object sender, BeforeAssociateRequestEventArgs e)
        {
            Console.WriteLine("Before AssociateRequest");
        }

        static void storeDirectory_AfterAssociateRequest(object sender, AfterAssociateRequestEventArgs e)
        {
            StoreScu scu = sender as StoreScu;

            Console.WriteLine("After AssociateRequest");
            for (int i = 0; i < e.Associate.PresentationContextCount; i++)
            {
                byte pid = e.Associate.GetPresentationContextID(i);
                string absSyntax = e.Associate.GetAbstract(pid);
                DicomAssociateAcceptResultType result = e.Associate.GetResult(pid);
                DicomUid uid = DicomUidTable.Instance.Find(absSyntax);

                Console.WriteLine("\tPresentationContext ({0})", pid);
                Console.WriteLine("\t\tAbstractSyntax: {0}", absSyntax);
                if (uid != null)
                    Console.WriteLine("\t\tDescription: {0}", uid.Name);
                Console.WriteLine("\t\tResult: {0}", result);
            }
        }

        static void storeDirectory_BeforeCStore(object sender, BeforeCStoreEventArgs e)
        {
            if (e.Error != null)
            {
                 e.Skip = SkipMethod.AllFiles;
            }
        }

        static void storeDirectory_AfterCStore(object sender, AfterCStoreEventArgs e)
        {
            /*
            string msg;
            msg = string.Format("{0} store complete. Status: {1}", e.FileInfo.FullName, e.Status);
            Console.WriteLine(msg);
            */
        }

        /// <summary>
        /// 直接用C-Find進行Query
        /// </summary>
        public static void DemoStoreClient()
        {
            try
            {
                AeInfo clientInfo = new AeInfo();
                clientInfo.Address = "127.0.0.1"; //local machine
                clientInfo.AETitle = "TEST_CLIENT";
                clientInfo.Port = 104;
                
                EmeccaPacesQueryClient client = new EmeccaPacesQueryClient("TEST210", "127.0.0.1", 104, "QC", "192.168.8.82", 104, false);
                DicomDataSet[] series = client.FindSeriesByStudyInstanceUid("1.2.68.0.1.6.20100001.21801205052400811");
                StoreScu storeFile = new StoreScu();
                
                if ( series.Length > 0 )
                {
                    DicomScp scpInfo = new DicomScp(IPAddress.Parse("192.168.8.82"), "QC", 104);
                    scpInfo.Timeout = 30;
                    //StoreClient storeClient = new StoreClient(clientInfo, scpInfo, Compression.Native, dataAccess);
                    //storeClient.StoreSeries(series[0].GetValue<string>(DicomTag.StudyInstanceUID, string.Empty), series[0].GetValue <string> ( DicomTag.SeriesInstanceUID, string.Empty)) ;

                    Console.WriteLine ( "Series {0} has been stored successfully.", series [ 0 ].GetValue <string> ( DicomTag.SeriesInstanceUID, string.Empty ) ) ;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// C Find PacsQueryClient
        /// Note By Alan 20120902 用Leadtool的PacsQueryClinet竟查不到資料...原因待查
        /// </summary>
        public static void demoPacsQueryClient()
        {
            try
            {
                AeInfo clientInfo = new AeInfo();
                clientInfo.Address = "127.0.0.1"; //local machine
                clientInfo.AETitle = "TEST210";
                clientInfo.Port = 104;
                DicomScp scpInfo = new DicomScp(IPAddress.Parse("192.168.8.82"), "QC", 104);
                scpInfo.Timeout = 30;
                PacsQueryClient pacsClient = new PacsQueryClient(clientInfo, scpInfo);
                pacsClient.LogFileName = @"C:\DWSv2011\log.txt";
                pacsClient.EnableLog = true;
                /*
                 SOPInstanceUID: 1.2.840.114257.20120801100826.407814622.1045390504
                 SeriesInstanceUID: 1.2.840.114257.20120801100819.17981732.1734363608
                 StudyInstanceUID: 1.2.68.0.1.6.20100001.21801205052400811
                 */
                FindQuery imageQuery = new FindQuery();
                //studiesQuery.PatientId = "16954039";
                imageQuery.StudyInstanceUID = "1.2.68.0.1.6.20100001.21801205052400811";
                imageQuery.SeriesInstanceUID = "1.2.840.114257.20120801100819.17981732.1734363608";
                imageQuery.QueryLevel = QueryLevel.Image;
                DicomDataSet[] studies = pacsClient.FindImages(imageQuery);
                if (studies.Length > 0)
                {
                    DicomDataSet study = studies[0];
                    FindQuery seriesQuery = new FindQuery();
                    seriesQuery.StudyInstanceUID = study.GetValue<string>(DicomTag.StudyInstanceUID, string.Empty);
                    DicomDataSet[] series = pacsClient.FindSeries(seriesQuery);
                    foreach (DicomDataSet seriesDS in series)
                    {
                        FindQuery imagesQuery = new FindQuery();
                        imagesQuery.SeriesInstanceUID = seriesDS.GetValue<string>(DicomTag.SeriesInstanceUID, string.Empty);
                        DicomDataSet[] images = pacsClient.FindImages(imagesQuery);
                        foreach (DicomDataSet instance in images)
                        {
                            Console.WriteLine("SOPInstanceUID: {0}", instance.GetValue<string>(DicomTag.SOPInstanceUID, string.Empty));

                            Console.WriteLine("SeriesInstanceUID: {0}", instance.GetValue<string>(DicomTag.SeriesInstanceUID, string.Empty));

                            Console.WriteLine("StudyInstanceUID: {0}", instance.GetValue<string>(DicomTag.StudyInstanceUID, string.Empty));
                        }
                    }
                }


                /*
                QueryRetrieveScu findSeries = new QueryRetrieveScu();
                findSeries.AETitle = "ALAN";
                findSeries.HostPort = 104;
                findSeries.HostAddress = IPAddress.Parse("192.168.8.160");
                FindQuery query = new FindQuery();
                DicomScp scp = new DicomScp(IPAddress.Parse("192.168.8.82"), "QC", 104);
                scp.Timeout = 180;
                DicomDataSet ds = new DicomDataSet();
                EmeccaDicomUtility.SetTag(ds, DicomTag.QueryRetrieveLevel, "SERIES");
                EmeccaDicomUtility.SetTag(ds, DicomTag.StudyInstanceUID, studyInstanceUid);
                //CFindStudyRootDicomDataSet ds1 = new CFindStudyRootDicomDataSet(@"C:\DWSv2011\CFindMinimumQueryTemplate.xml");


                //
                // Load a dataset that has information needed for a C-FIND-REQ at series level.  Change
                // this to reflect a dataset on your computer.
                //
                //ds.Load(Path.Combine(LEAD_VARS.ImagesDir, "image1.dcm"), DicomDataSetLoadFlags.LoadAndClose);
                query.QueryLevel = QueryLevel.Series;

                // Query for a specific study instance uid.  Change this to a study instance
                // that is available on your Dicom Server
                //
                //Study:1.2.68.0.1.6.20100001.21801205051027461
                //Series:1.2.840.114257.20120827133933.762824494.238136658
                query.StudyInstanceUID = studyInstanceUid;
                //query.SeriesInstanceUID = "1.2.840.114257.20120827133933.762824494.238136658";
                findSeries.BeforeCFind += new BeforeCFindDelegate(find_BeforeCFind);
                findSeries.MatchSeries += new MatchSeriesDelegate(find_Match);
                findSeries.AfterCFind += new AfterCFindDelegate(find_AfterCFind);
                //findSeries.Find(scp, query, new DicomMatchDelegate<Object>(FoundMatch), ds);
                findSeries.Find(scp, query, false, ds);
                Console.WriteLine("Result---->" + result.Count);
                */
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static ArrayList result = new ArrayList();

        private static void FoundMatch(Object record, DicomDataSet ds)
        {
            result.Add(ds);
        }

        
        static void find_BeforeCFind(object sender, BeforeCFindEventArgs e)
        {
            Console.WriteLine("Before CFind: " + e.QueryLevel.ToString());
        }

        static void find_Match(object sender, MatchEventArgs<Series> e)
        {
            Console.Write("Series Instance UID: " + e.Info.InstanceUID);
            Console.WriteLine("Series Number: " + e.Info.Number != null ? e.Info.Number.ToString() : string.Empty);
            Console.WriteLine("Series Date: " + e.Info.Date);
            Console.WriteLine("Series Description: " + e.Info.Description);
            Console.WriteLine("Modality: " + e.Info.Modality);
            Console.WriteLine("Number of Related Instances: " + e.Info.NumberOfRelatedInstances);
            Console.WriteLine("Performed Procedure Step ID:" + e.Info.PerformedProcStepId);
            Console.WriteLine("Performed Procedure Step Start Date: " + e.Info.PerfProcStepStartDate);
            Console.WriteLine("Performed Procedure Step Start Time: " + e.Info.PerfProcStepStartTime);
            Console.WriteLine("Requested Procedure ID: " + e.Info.RequestedProcId);
            Console.WriteLine("Scheduled Procedure Step ID: " + e.Info.SchedProcStepId);
        }

        static void find_AfterCFind(object sender, AfterCFindEventArgs e)
        {
            Console.WriteLine(e.Status == DicomCommandStatusType.Success);
        }


        public static void FindStudy()
        {
            QueryRetrieveScu findStudy = new QueryRetrieveScu();
            FindQuery query = new FindQuery();
            DicomScp scp = new DicomScp();
            
            //
            // Change these parameters to reflect the calling AETitle.
            //

            findStudy.AETitle = "Test";
            findStudy.HostPort = 104;
            findStudy.HostAddress = IPAddress.Parse("127.0.0.1");

            //
            // Change these parameters to reflect the called AETitle (server).
            //


            scp.AETitle = "QC";
            scp.Port = 104;
            scp.Timeout = 60;
            scp.PeerAddress = IPAddress.Parse("192.168.8.82");

            //
            // Find all studies
            //

            query.QueryLevel = QueryLevel.Study;
            findStudy.BeforeConnect += new BeforeConnectDelegate(findStudy_BeforeConnect);
            findStudy.AfterConnect += new AfterConnectDelegate(findStudy_AfterConnect);
            findStudy.BeforeCFind += new BeforeCFindDelegate(findStudy_BeforeCFind);
            findStudy.MatchStudy += new MatchStudyDelegate(findStudy_MatchStudy);
            findStudy.AfterCFind += new AfterCFindDelegate(findStudy_AfterCFind);
            findStudy.Find(scp, query);

            findStudy = null;
            query = null;
            scp = null;

        }

        static void findStudy_BeforeConnect(object sender, BeforeConnectEventArgs e)
        {
            Console.WriteLine("Connecting to: " + e.Scp.PeerAddress.ToString());
        }

        static void findStudy_AfterConnect(object sender, AfterConnectEventArgs e)
        {
            Console.WriteLine("Connection status: " + e.Error);
        }

        static void findStudy_BeforeCFind(object sender, BeforeCFindEventArgs e)
        {
            Console.WriteLine("Before CFind: " + e.QueryLevel.ToString());
            Console.WriteLine("Association supports relational queries: {0}", e.Scp.Relational);
            Console.WriteLine("Association Information");
            for (int i = 0; i < e.Scp.Association.PresentationContextCount; i++)
            {
                byte pid = e.Scp.Association.GetPresentationContextID(i);
                string absSyntax = e.Scp.Association.GetAbstract(pid);
                DicomAssociateAcceptResultType result = e.Scp.Association.GetResult(pid);
                DicomUid uid = DicomUidTable.Instance.Find(absSyntax);

                Console.WriteLine("\tPresentationContext ({0})", pid);
                Console.WriteLine("\t\tAbstractSyntax: {0}", absSyntax);
                if (uid != null)
                    Console.WriteLine("\t\tDescription: {0}", uid.Name);
                Console.WriteLine("\t\tResult: {0}", result);
            }
        }

        static void findStudy_MatchStudy(object sender, MatchEventArgs<Study> e)
        {
            Console.WriteLine("Accession #: " + e.Info.AccessionNumber);
            Console.WriteLine("Admitting Diagnosis Description: " + e.Info.AdmitDiagDescrp);
            Console.WriteLine("Age: " + (e.Info.Age.HasValue ? e.Info.Age.Value.Number.ToString() + e.Info.Age.Value.Reference : "No Age"));
            Console.WriteLine("Study Date: " + e.Info.Date);
            Console.WriteLine("Study Time: " + e.Info.Time);
            Console.WriteLine("Study Description: " + e.Info.Description);
            Console.WriteLine("Study Id: " + e.Info.Id);
            Console.WriteLine("Study Instance: " + e.Info.InstanceUID);
            if (e.Info.ModalitiesInStudy != null)
                Console.WriteLine("Modalities in Study: " + e.Info.ModalitiesInStudy.ToString());
            if (e.Info.NameOfDrsReading != null)
                Console.WriteLine("Name of drs reading Study: " + e.Info.NameOfDrsReading.ToString());
            Console.WriteLine("Number of Related Instances: " + e.Info.NumberOfRelatedInstances);
            Console.WriteLine("Number of Related Series: " + e.Info.NumberofRelatedSeries);
            Console.WriteLine("Referring Dr Name: " + e.Info.ReferringPhysiciansName.Full);
            Console.WriteLine("Patient Size: " + e.Info.Size);
            Console.WriteLine("Patient Weight: " + e.Info.Weight);
            Console.WriteLine("Patient Birth Date: " + e.Info.Patient.BirthDate);
            Console.WriteLine("Patient Comments: " + e.Info.Patient.Comments);
            Console.WriteLine("Patient Id: " + e.Info.Patient.Id);
            Console.WriteLine("Patient Sex: " + e.Info.Patient.Sex);
            Console.WriteLine("Patient Name");
            Console.WriteLine("\tFamily Name: " + e.Info.Patient.Name.Family);
            Console.WriteLine("\tGiven Name: " + e.Info.Patient.Name.Given);
            Console.WriteLine("\tMiddle Name: " + e.Info.Patient.Name.Middle);
            Console.WriteLine("\tName Prefix: " + e.Info.Patient.Name.Prefix);
            Console.WriteLine("\tName Suffix: " + e.Info.Patient.Name.Suffix);
            Console.WriteLine("\tFull Name: " + e.Info.Patient.Name.Full);
            Console.WriteLine("\tFull Name Dicom Encoded: " + e.Info.Patient.Name.FullDicomEncoded);
            Console.WriteLine("==========================================================\r\n");
        }

        static void findStudy_AfterCFind(object sender, AfterCFindEventArgs e)
        {
            Console.WriteLine(e.Status == DicomCommandStatusType.Success);
        }
    }
    static class LEAD_VARS
    {
        public const string ImagesDir = @"C:\DWSv2011\Dicom";
    }

}
