using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leadtools.Dicom;
using System.IO;
using System.Collections;
using Leadtools.MediaWriter;


namespace Emecca.Medical.Dicom
{
    public class EmeccaMediaUtility
    {
        //燒錄完成后，暫存檔案要刪除
        public bool destoryDicomDir(string id, string dirCache)
        {
            bool ret = false;
            string fpDicomdir = dirCache + "\\" + id;
            if (Directory.Exists(fpDicomdir))
            {
                Directory.Delete(fpDicomdir, true);
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 创建DCM影像和DicomDir  n Study--1 Series-n Object
        /// </summary>
        /// <param name="id">本次暫存的路徑，建議用DcmStudy.StudyInstanceUid標識</param>
        /// <param name="dirCache">暫存檔案統一存放路徑 如：C:\users\tempFolder,實際存放路徑是dirCache\\id </param>
        /// <param name="dcmList">DCM影像的存放路徑列表，string</param>
        /// <returns>成功錄入影像的張數</returns>
        public int createDicomDir(string id, string dirCache, ICollection dcmList)
        {
            if (!Directory.Exists(dirCache))
            {
                try
                {
                    Directory.CreateDirectory(dirCache);
                }
                catch
                {
                    throw;
                }
            }
            DicomDir dicomDir = new DicomDir();

            string fpDicomdir = dirCache + "\\" + id;
            string dirDicomData = fpDicomdir + "\\DCM";

            dicomDir.ResetOptions();
            dicomDir.Reset(fpDicomdir);
            if (Directory.Exists(fpDicomdir))
            {
                Directory.Delete(fpDicomdir, true);
            }
            Directory.CreateDirectory(fpDicomdir);
            Directory.CreateDirectory(dirDicomData);
            // If it is desired to change the values of the Implementation Class
            // UID (0002,0012) and the Implementation Version Name (0002,0013)...
            DicomElement element;
            element = dicomDir.DataSet.FindFirstElement(null, DicomTag.ImplementationClassUID, false);
            if (element != null)
            {
                dicomDir.DataSet.SetStringValue(element, "1.2.528.1.1001.2.20060808.1", DicomCharacterSetType.Default); // Must be a UID
            }
            element = dicomDir.DataSet.FindFirstElement(null, DicomTag.ImplementationVersionName, false);
            if (element != null)
            {
                dicomDir.DataSet.SetStringValue(element, "DVNET PACS (Emecca)", DicomCharacterSetType.Default); // Must be a UID
            }

            // Set options
            DicomDirOptions options;
            options = dicomDir.Options;
            options.IncludeSubfolders = true;
            options.InsertIconImageSequence = true;
            dicomDir.Options = options;

            dicomDir.InsertFile(null);

            int count = 0;
             DicomDataSet ds;
             foreach (string dcmFile in dcmList)
             {
                 string newDcmFile = dirDicomData + "\\IMG" + EmeccaDicomUtility.format(count, "00000") + ".DCM"; // dcmFile.Substring(dcmFile.LastIndexOf("\\"));
                 File.Copy(dcmFile, newDcmFile);

                 ds = new DicomDataSet();
                 ds.Load(newDcmFile, DicomDataSetLoadFlags.None);
                 switch (ds.InformationClass)
                 {
                     case DicomClassType.BasicDirectory:
                     case DicomClassType.BasicTextSR:
                     case DicomClassType.KeyObjectSelectionDocument:
                         continue;
                     default:
                         break;
                 }

                 count++;
                 dicomDir.InsertDataSet(ds, newDcmFile);
                 ds.Dispose();
             }
            dicomDir.Save();

            return count;
        }

        public int createDicomDir(string id, int studyIndex,int seriesIndex, string dirCache, ICollection dcmList, ref DicomDir dicomDir)
        {
            if (!Directory.Exists(dirCache))
            {
                try
                {
                    Directory.CreateDirectory(dirCache);
                }
                catch
                {
                    throw;
                }
            }

            string fpDicomdir = dirCache + "\\" + id;
            string dirDicomData = fpDicomdir + "\\DCM\\SDY"+EmeccaDicomUtility.format(studyIndex, "00000") + "\\SRS"+EmeccaDicomUtility.format(seriesIndex, "00000");

            //dicomDir.ResetOptions();
            //dicomDir.Reset(fpDicomdir);
            //if (Directory.Exists(fpDicomdir))
            //{
            //    Directory.Delete(fpDicomdir, true);
            //}
            Directory.CreateDirectory(fpDicomdir);
            Directory.CreateDirectory(dirDicomData);
            // If it is desired to change the values of the Implementation Class
            // UID (0002,0012) and the Implementation Version Name (0002,0013)...
            DicomElement element;
            element = dicomDir.DataSet.FindFirstElement(null, DicomTag.ImplementationClassUID, false);
            if (element != null)
            {
                dicomDir.DataSet.SetStringValue(element, "1.2.528.1.1001.2.20060808.1", DicomCharacterSetType.Default); // Must be a UID
            }
            element = dicomDir.DataSet.FindFirstElement(null, DicomTag.ImplementationVersionName, false);
            if (element != null)
            {
                dicomDir.DataSet.SetStringValue(element, "DVNET PACS (Emecca)", DicomCharacterSetType.Default); // Must be a UID
            }

            // Set options
            DicomDirOptions options;
            options = dicomDir.Options;
            options.IncludeSubfolders = true;
            options.InsertIconImageSequence = true;
            dicomDir.Options = options;

            dicomDir.InsertFile(null);

            int count = 0;
            DicomDataSet ds;
            foreach (string dcmFile in dcmList)
            {
                string newDcmFile = dirDicomData + "\\IMG" + EmeccaDicomUtility.format(count, "00000") + ".DCM"; // dcmFile.Substring(dcmFile.LastIndexOf("\\"));
                File.Copy(dcmFile, newDcmFile);

                ds = new DicomDataSet();
                ds.Load(newDcmFile, DicomDataSetLoadFlags.None);
                switch (ds.InformationClass)
                {
                    case DicomClassType.BasicDirectory:
                    case DicomClassType.BasicTextSR:
                    case DicomClassType.KeyObjectSelectionDocument:
                        continue;
                    default:
                        break;
                }

                count++;
                dicomDir.InsertDataSet(ds, newDcmFile);
                ds.Dispose();
            }
          

            return count;
        }

        public void doBurnDemo(string name,string dirSource)
        {
            MediaWriter writer = new MediaWriter();
            MediaWriterDrive drive = writer.CurrentDrive;
            MediaWriterDisc disk = drive.CreateDisc();

            foreach (MediaWriterDrive selDrive in writer.Drives)
            {
                Console.WriteLine(selDrive.Name);
                foreach (MediaWriterSpeed supportSpeed in selDrive.Speeds)
                {
                    Console.WriteLine(supportSpeed.Speed);
                }
            }


            disk.SourcePathName = dirSource;
            disk.VolumeName = name;

            drive.BurnDisc(disk);
        }

        /// <summary>
        /// 燒錄成光碟
        /// </summary>
        /// <param name="drive">光碟機</param>
        /// <param name="name">碟的名稱</param>
        /// <param name="dirSource">燒錄的檔案存放路徑。如dirCache\\id</param>
        public void burnMedia(MediaWriterDrive drive,string name, string dirSource)
        {
            try
            {
                using (MediaWriterDisc disk = drive.CreateDisc())
                {
                    disk.SourcePathName = dirSource;
                    disk.VolumeName = name;
                    drive.BurnDisc(disk);
                }
            }
            catch
            {
                throw;
            }
        } 

        /// <summary>
        ///  燒錄成光碟
        /// </summary>
        /// <param name="drive">光碟機,可以是None,不可以為NULL</param>
        /// <param name="name">碟的名稱</param>
        /// <param name="dirSource">燒錄的檔案存放路徑。如dirCache\\id</param>
        /// <param name="fpISO">ISO檔案存放路徑 如：C;\users\DICOM_Image.ISO</param>
        public void burnMedia(MediaWriterDrive drive, string name, string dirSource,string fpISO)
        {
            try
            {
                using (MediaWriterDisc disk = drive.CreateDisc())
                {
                    disk.OutputPathName = fpISO;
                    disk.SourcePathName = dirSource;
                    disk.VolumeName = name;

                    drive.CreateISO(disk);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
