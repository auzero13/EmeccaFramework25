using System;
using System.Collections;
using Emecca.Medical.Dicom.Command;
using Leadtools;
using Leadtools.Codecs;
using Leadtools.Dicom;
using Leadtools.Dicom.Scu;

namespace Emecca.Medical.Dicom
{
    public class EmeccaDicomEngine
    {

        public ArrayList findWorklist(DicomScp scp)
        {
            ArrayList records = new ArrayList();
            return records;
        }

        public ArrayList loadLocalStudies()
        {
            ArrayList records = new ArrayList();
            return records;
        }

        public Study loadLocalStudy(string instanceUid)
        {
            Study record = new Study();
            return record;
        }

        public ArrayList loadRemoteStudies(DicomScp scp)
        {
            ArrayList records = new ArrayList();
            return records;
        }

        public Study loadRemoteStudy(DicomScp scp, string instanceUid)
        {
            Study record = new Study();
            return record;
        }

        public static void doStartup()
        {

            RasterSupport.Unlock(RasterSupportType.Abc, "");
            RasterSupport.Unlock(RasterSupportType.AbicRead, "");
            RasterSupport.Unlock(RasterSupportType.AbicSave, "");
            RasterSupport.Unlock(RasterSupportType.Barcodes1D, "");
            RasterSupport.Unlock(RasterSupportType.Barcodes1DSilver, "");
            RasterSupport.Unlock(RasterSupportType.BarcodesDataMatrixRead, "");
            RasterSupport.Unlock(RasterSupportType.BarcodesDataMatrixWrite, "");
            RasterSupport.Unlock(RasterSupportType.BarcodesPdfRead, "");
            RasterSupport.Unlock(RasterSupportType.BarcodesPdfWrite, "");
            RasterSupport.Unlock(RasterSupportType.BarcodesQRRead, "");
            RasterSupport.Unlock(RasterSupportType.BarcodesQRWrite, "");
            RasterSupport.Unlock(RasterSupportType.Bitonal, "");
            RasterSupport.Unlock(RasterSupportType.Ccow, "");
            RasterSupport.Unlock(RasterSupportType.Cmw, "");
            RasterSupport.Unlock(RasterSupportType.Dicom, "");
            RasterSupport.Unlock(RasterSupportType.Document, "");
            RasterSupport.Unlock(RasterSupportType.DocumentWriters, "");
            RasterSupport.Unlock(RasterSupportType.DocumentWritersPdf, "");
            RasterSupport.Unlock(RasterSupportType.ExtGray, "");
            RasterSupport.Unlock(RasterSupportType.Forms, "");
            RasterSupport.Unlock(RasterSupportType.IcrPlus, "");
            RasterSupport.Unlock(RasterSupportType.IcrProfessional, "");
            RasterSupport.Unlock(RasterSupportType.J2k, "");
            RasterSupport.Unlock(RasterSupportType.Jbig2, "");
            RasterSupport.Unlock(RasterSupportType.Jpip, "");
            RasterSupport.Unlock(RasterSupportType.Pro, "");
            RasterSupport.Unlock(RasterSupportType.LeadOmr, "");
            RasterSupport.Unlock(RasterSupportType.MediaWriter, "");
            RasterSupport.Unlock(RasterSupportType.Medical, "ZhyFRnk3sY");
            RasterSupport.Unlock(RasterSupportType.Medical3d, "");
            RasterSupport.Unlock(RasterSupportType.MedicalNet, "b4nBinY7tv");
            RasterSupport.Unlock(RasterSupportType.MedicalServer, "QbXwuZxA3h");
            RasterSupport.Unlock(RasterSupportType.Mobile, "");
            RasterSupport.Unlock(RasterSupportType.Nitf, "");
            RasterSupport.Unlock(RasterSupportType.OcrAdvantage, "");
            RasterSupport.Unlock(RasterSupportType.OcrAdvantagePdfLeadOutput, "");
            RasterSupport.Unlock(RasterSupportType.OcrArabic, "");
            RasterSupport.Unlock(RasterSupportType.OcrArabicPdfLeadOutput, "");
            RasterSupport.Unlock(RasterSupportType.OcrPlus, "");
            RasterSupport.Unlock(RasterSupportType.OcrPlusPdfOutput, "");
            RasterSupport.Unlock(RasterSupportType.OcrPlusPdfLeadOutput, "");
            RasterSupport.Unlock(RasterSupportType.OcrProfessional, "");
            RasterSupport.Unlock(RasterSupportType.OcrProfessionalAsian, "");
            RasterSupport.Unlock(RasterSupportType.OcrProfessionalPdfOutput, "");
            RasterSupport.Unlock(RasterSupportType.OcrProfessionalPdfLeadOutput, "");
            RasterSupport.Unlock(RasterSupportType.PdfAdvanced, "");
            RasterSupport.Unlock(RasterSupportType.PdfRead, "");
            RasterSupport.Unlock(RasterSupportType.PdfSave, "");
            RasterSupport.Unlock(RasterSupportType.PrintDriver, "YvMsmzECAE");
            RasterSupport.Unlock(RasterSupportType.PrintDriverServer, "v37Ry49tHN");
            RasterSupport.Unlock(RasterSupportType.Vector, "");

            if (RasterSupport.IsLocked(RasterSupportType.Medical))
            {
                //MessageBox.Show(String.Format("{0} Support is locked!", RasterSupportType.Medical.ToString()), "Warning");
                return;
            }

            RasterCodecs.Startup();
            DicomEngine.Startup();
            DicomNet.Startup();

        }

        public static void doShutdown()
        {
            RasterCodecs.Shutdown();
            DicomNet.Shutdown();
            DicomEngine.Shutdown();
        }

        static void Main()
        {
            doStartup();

           // MwlFind mf = new MwlFind("TEST01", "NEOWL", "192.168.126.99", 5000,false);

            //CFind find = new CFind("TEST01","EMECCA","10.200.5.210",104,false);
            //int now = DateTime.Now.Millisecond;
            //Console.WriteLine("TEST START");
            //find.doTest();
            //Console.WriteLine("TIME COST (mSec) : " + (DateTime.Now.Millisecond - now) );

            Console.ReadKey();
            doShutdown();
        }
    }
}
