using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Leadtools.Dicom;

namespace Emecca.Medical.Dicom
{
    public class EmeccaDicomUtility
    {

        /// <summary>
        /// Helper method to get string value from a DICOM dataset.
        /// </summary>
        /// <param name="dcm">The DICOM dataset.</param>
        /// <param name="tag">Dicom tag.</param>
        /// <returns>String value of the specified DICOM tag.</returns>
        public static string GetStringValue(DicomDataSet dcm, long tag, bool tree)
        {
            DicomElement element;

            element = dcm.FindFirstElement(null, tag, tree);
            if (element != null)
            {
                if (dcm.GetElementValueCount(element) > 0)
                {
                    return dcm.GetConvertValue(element);
                }
            }

            return "";
        }

        public static string GetStringDate(DicomDataSet dcm, long tag)
        {
            string strDate = GetStringValue(dcm, tag, false);
            if (strDate == null || "".Equals(strDate))
            {
                return "";
            }

            DateTime dt = DateTime.Parse(strDate);

            if (dt == null)
            {
                return "";
            }
            return dt.ToString("yyyyMMdd");
        }
        public static string GetStringTime(DicomDataSet dcm, long tag)
        {
            string strTime = GetStringValue(dcm, tag, false);
            if (strTime == null || "".Equals(strTime))
            {
                return "";
            }

            DateTime dt = DateTime.Parse(strTime);

            if (dt == null)
            {
                return "";
            }
            return dt.ToString("HHmmss");
        }

        public static string GetStringValue(DicomDataSet dcm, long tag)
        {
            return GetStringValue(dcm, tag, false);
        }


        public static StringCollection GetStringValues(DicomDataSet dcm, long tag)
        {
            DicomElement element;
            StringCollection sc = new StringCollection();

            element = dcm.FindFirstElement(null, tag, false);
            if (element != null)
            {
                if (dcm.GetElementValueCount(element) > 0)
                {
                    string s = dcm.GetConvertValue(element);
                    string[] items = s.Split('\\');

                    foreach (string value in items)
                    {
                        sc.Add(value);
                    }
                }
            }

            return sc;
        }

        public static Byte[] GetByteValue(DicomDataSet dcm, long tag)
        {
            DicomElement element;

            element = dcm.FindFirstElement(null, tag, false);
            if (element != null)
            {
                if (dcm.GetElementValueCount(element) > 0)
                {
                    //return dcm.GetBinaryValue(element, dcm.GetConvertValue(element).Length);
                    return dcm.GetByteValue(element, 0, dcm.GetConvertValue(element).Length);
                }
            }
            return null;
        }

        public static byte[] GetBinaryValues(DicomDataSet dcm, long tag)
        {
            DicomElement element;

            element = dcm.FindFirstElement(null, tag, false);
            if (element != null)
            {
                if (element.Length > 0)
                {
                    return dcm.GetBinaryValue(element, (int)element.Length);
                }
            }

            return null;
        }

        public static bool IsTagPresent(DicomDataSet dcm, long tag)
        {
            DicomElement element;

            element = dcm.FindFirstElement(null, tag, false);
            return (element != null);
        }

        public static DicomExceptionCode SetTag(DicomDataSet dcm, long tag, string tagValue)
        {
            return SetTag(dcm, tag, tagValue, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dcm"></param>
        /// <param name="tag"></param>
        /// <param name="tagValue"></param>
        /// <returns></returns>
        public static DicomExceptionCode SetTag(DicomDataSet dcm, long tag, string tagValue, bool tree)
        {
            DicomExceptionCode ret = DicomExceptionCode.Success;
            DicomElement element;

            if (tagValue == null)
                return DicomExceptionCode.Parameter;

            element = dcm.FindFirstElement(null, tag, tree);
            if (element == null)
            {
                element = dcm.InsertElement(null, false, tag, DicomVRType.UN, false, 0);
            }

            if (element == null)
                return DicomExceptionCode.Parameter;

            try
            {
                byte[] value = null;
                if (tagValue != null && !"".Equals(tagValue))
                {
                    value = formatStringBytes(tagValue, ReadCharacterSet(dcm));
                    dcm.SetBinaryValue(element, value, value.Length);
                }
                
            }
            catch (DicomException de)
            {
                ret = de.Code;
            }

            return ret;
        }

        //public static void SetTag(DicomDataSet dcm, long Sequence, long Tag, object TagValue)
        //{
        //    DicomElement seqElement = dcm.FindFirstElement(null, Sequence, true);
        //    DicomElement seqItem = null;
        //    DicomElement item = null;

        //    if (seqElement == null)
        //    {
        //        seqElement = dcm.InsertElement(null, false, Tag, DicomVRType.SQ, true, -1);
        //    }

        //    seqItem = dcm.GetChildElement(seqElement, false);
        //    if (seqItem == null)
        //    {
        //        seqItem = dcm.InsertElement(seqElement, true, DicomTag.SequenceDelimitationItem, DicomVRType.SQ, true, -1);
        //    }

        //    item = dcm.GetChildElement(seqItem, true);
        //    while (item != null)
        //    {
        //        if (item.Tag == Tag)
        //        break;


        //        item = dcm.GetNextElement(item, true, true);
        //    }

        //    if (item == null)
        //    {
        //        item = dcm.InsertElement(seqItem, true, Tag, DicomVRType.UN, false, -1);
        //    }
        //    dcm.SetConvertValue(item, TagValue.ToString(), 1);
        //}


        //public static DicomExceptionCode SetTag(DicomDataSet dcm, long tag, byte[] tagValue)
        //{
        //    DicomExceptionCode ret = DicomExceptionCode.Success;
        //    DicomElement element;

        //    if (tagValue == null)
        //        return DicomExceptionCode.Parameter;

        //    element = dcm.FindFirstElement(null, tag, true);
        //    if (element == null)
        //    {
        //        element = dcm.InsertElement(null, false, tag, DicomVRType.UN, false, 0);
        //    }

        //    dcm.SetBinaryValue(element, tagValue, tagValue.Length);

        //    return ret;
        //}

        public static DicomExceptionCode InsertKeyElement(DicomDataSet dcmRsp, DicomDataSet dcmReq, long tag)
        {
            DicomExceptionCode ret = DicomExceptionCode.Success;
            DicomElement element;

            try
            {
                element = dcmReq.FindFirstElement(null, tag, false);
                if (element != null)
                {
                    dcmRsp.InsertElement(null, false, tag, DicomVRType.UN, false, 0);
                }
            }
            catch (DicomException de)
            {
                ret = de.Code;
            }

            return ret;
        }


#if (LTV15_CONFIG)
       public static DicomExceptionCode SetKeyElement(DicomDataSet dcmRsp, DicomTagType tag, object tagValue)
       {
           return SetKeyElement(dcmRsp, (long)tag, tagValue);
       }

       public static DicomExceptionCode SetKeyElement(DicomDataSet dcmRsp, DicomTagType tag, object tagValue, bool tree)
       {
           return SetKeyElement(dcmRsp, (long)tag, tagValue, tree);
       }
#endif

        public static DicomExceptionCode SetKeyElement(DicomDataSet dcmRsp, long tag, object tagValue, bool tree)
        {
            DicomExceptionCode ret = DicomExceptionCode.Success;
            DicomElement element;

            if (tagValue == null)
                return DicomExceptionCode.Parameter;

            try
            {
                element = dcmRsp.FindFirstElement(null, tag, tree);
                if (element != null)
                {
                    dcmRsp.SetConvertValue(element, tagValue.ToString(), 1);
                }
            }
            catch (DicomException de)
            {
                ret = de.Code;
            }

            return ret;
        }

        public static DicomExceptionCode SetKeyElement(DicomDataSet dcmRsp, long tag, object tagValue)
        {
            return SetKeyElement(dcmRsp, tag, tagValue, false);
        }

        public static UInt16 GetGroup(long tag)
        {
            return ((UInt16)(tag >> 16));
        }

        public static int GetElement(long tag)
        {
            return ((UInt16)(tag & 0xFFFF));
        }

        // Creates a properly formatted Dicom Unique Identifier (VR type of UI) value
        /* Marked By Alan 20120220 處理SeriesUID會重覆問題
        public static string GenerateDicomUniqueIdentifier()
        {
            string strGUID = "";

            try
            {
                DateTime SystemTime = DateTime.Now;
                Random rand = new Random((int)SystemTime.Ticks);
                strGUID = String.Format("1.2.840.114257.{0}.{1}.{2}", SystemTime.Ticks, rand.Next(), rand.Next());
                // max length for this field is 64 so cut it off if too long
                if (strGUID.Length > 64)
                    strGUID = strGUID.Substring(0, 64);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strGUID;
        }
        */
        /* Marked By Alan 20121122 處理臨時檢查SeriesUID會重覆問題
        private static long randTicks = DateTime.Now.Ticks;
        public static string GenerateDicomUniqueIdentifier()
        {
            string strGUID = "";

            try
            {
                if (randTicks == DateTime.Now.Ticks)
                {
                    randTicks = randTicks + 1;
                }
                else
                {
                    randTicks = DateTime.Now.Ticks;
                }
                Random rand = new Random((int)randTicks);
                strGUID = String.Format("1.2.840.114257.{0}.{1}.{2}", DateTime.Now.ToString("yyyyMMddHHmmss"), rand.Next(), rand.Next());
                // max length for this field is 64 so cut it off if too long
                if (strGUID.Length > 64)
                    strGUID = strGUID.Substring(0, 64);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strGUID;
        }
        */

        private static int myTickNum = new Random().Next();
        public static string GenerateDicomUniqueIdentifier()
        {
            myTickNum = ((myTickNum + 1) % (int.MinValue));
            string strGUID = "";
            try
            {
                Random rand = new Random(myTickNum);
                strGUID = String.Format("1.2.840.114257.{0}.{1}.{2}", DateTime.Now.ToString("yyyyMMddHHmmss"), rand.Next(), rand.Next());
                // max length for this field is 64 so cut it off if too long
                if (strGUID.Length > 64)
                {
                    strGUID = strGUID.Substring(0, 64);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strGUID;
        }

        public static System.Net.IPAddress ResolveIPAddress(string hostNameOrAddress)
        {
            IPAddress[] addresses;
            addresses = Dns.GetHostAddresses(hostNameOrAddress);
            if (addresses == null || addresses.Length == 0)
            {
                throw new ArgumentException("Invalid hostNameOrAddress parameter.");
            }
            else
            {
                foreach (IPAddress address in addresses)
                {
                    if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return address;
                    }
                }
                throw new ArgumentException("Couldn't resolve a valid host Address. Address must conform to IP version 4");
            }
        }

        //private static DicomCharacterSetType ReadCharacterSet(DicomDataSet ds)
        //{
        //    // If we do not find one of the character sets below, use the default
        //    DicomCharacterSetType characterSet = DicomCharacterSetType.UnicodeInUtf8;
        //    DicomElement element = ds.FindFirstElement(null, DicomTag.SpecificCharacterSet, false);
        //    if (element != null)
        //    {
        //        string sValue = ds.GetStringValue(element, 0);
        //        if (sValue != null)
        //        {
        //            if (sValue.Trim().Contains("ISO_IR 6"))
        //                characterSet = DicomCharacterSetType.Default;
        //            else if (sValue.Trim().Contains("ISO_IR 100"))
        //                characterSet = DicomCharacterSetType.LatinAlphabetNo1;
        //            else if (sValue.Trim().Contains("ISO_IR 101"))
        //                characterSet = DicomCharacterSetType.LatinAlphabetNo2;
        //            else if (sValue.Trim().Contains("ISO_IR 109"))
        //                characterSet = DicomCharacterSetType.LatinAlphabetNo3;
        //            else if (sValue.Trim().Contains("ISO_IR 110"))
        //                characterSet = DicomCharacterSetType.LatinAlphabetNo4;
        //            else if (sValue.Trim().Contains("ISO_IR 144"))
        //                characterSet = DicomCharacterSetType.Cyrillic;
        //            else if (sValue.Trim().Contains("ISO_IR 127"))
        //                characterSet = DicomCharacterSetType.Arabic;
        //            else if (sValue.Trim().Contains("ISO_IR 126"))
        //                characterSet = DicomCharacterSetType.Greek;
        //            else if (sValue.Trim().Contains("ISO_IR 138"))
        //                characterSet = DicomCharacterSetType.Hebrew;
        //            else if (sValue.Trim().Contains("ISO_IR 148"))
        //                characterSet = DicomCharacterSetType.LatinAlphabetNo5;
        //            else if (sValue.Trim().Contains("ISO_IR 13"))
        //                characterSet = DicomCharacterSetType.JapaneseJisX0201;
        //            else if (sValue.Trim().Contains("ISO_IR 166"))
        //                characterSet = DicomCharacterSetType.Thai;
        //            else if (sValue.Trim().Contains("ISO_IR 149"))
        //                characterSet = DicomCharacterSetType.Korean;
        //            else if (sValue.Trim().Contains("ISO_IR 192"))
        //                characterSet = DicomCharacterSetType.UnicodeInUtf8;
        //            else if (sValue.Trim().Contains("GB18030"))
        //                characterSet = DicomCharacterSetType.Gb18030;
        //        }
        //    }
        //    return characterSet;
        //}

        private static Encoding ReadCharacterSet(DicomDataSet ds)
        {
            // If we do not find one of the character sets below, use the default
            Encoding characterSet = Encoding.UTF8;
            DicomElement element = ds.FindFirstElement(null, DicomTag.SpecificCharacterSet, false);
            if (element != null)
            {
                string sValue = ds.GetStringValue(element, 0);
                if (sValue != null)
                {
                    if (sValue.Trim().Contains("ISO_IR 6"))
                        characterSet = Encoding.ASCII;
                    else if (sValue.Trim().Contains("ISO_IR 100"))
                        characterSet = Encoding.GetEncoding("ISO-8859-1");
                    else if (sValue.Trim().Contains("ISO_IR 101"))
                        characterSet = Encoding.GetEncoding("ISO-8859-2");
                    else if (sValue.Trim().Contains("ISO_IR 109"))
                        characterSet = Encoding.GetEncoding("ISO-8859-3");
                    else if (sValue.Trim().Contains("ISO_IR 110"))
                        characterSet = Encoding.GetEncoding("ISO-8859-4");
                    else if (sValue.Trim().Contains("ISO_IR 144"))
                        characterSet = Encoding.GetEncoding("ISO-8859-5");
                    else if (sValue.Trim().Contains("ISO_IR 127"))
                        characterSet = Encoding.GetEncoding("ISO-8859-6");
                    else if (sValue.Trim().Contains("ISO_IR 126"))
                        characterSet = Encoding.GetEncoding("ISO-8859-7");
                    else if (sValue.Trim().Contains("ISO_IR 138"))
                        characterSet = Encoding.GetEncoding("ISO-8859-8");
                    else if (sValue.Trim().Contains("ISO_IR 148"))
                        characterSet = Encoding.GetEncoding("ISO-8859-9");
                    else if (sValue.Trim().Contains("ISO_IR 13"))
                        characterSet = Encoding.GetEncoding("JIS0201");
                    else if (sValue.Trim().Contains("ISO_IR 166"))
                        characterSet = Encoding.GetEncoding("TIS620");
                    else if (sValue.Trim().Contains("ISO_IR 149"))
                        characterSet = Encoding.GetEncoding("ISO-2022-KR");
                    else if (sValue.Trim().Contains("ISO_IR 192"))
                        characterSet = Encoding.GetEncoding("UTF-8");
                    else if (sValue.Trim().Contains("GB18030"))
                        characterSet = Encoding.GetEncoding("GB18030");
                }
            }
            return characterSet;
        }

        public static void Log(string message){
            Console.WriteLine(DateTime.Now.ToString("*yyyyMMdd HHmmss ") + message);
        }

        public static byte[] formatStringBytes(string value)
        {
            return formatStringBytes(value, Encoding.UTF8);
        }
        public static byte[] formatStringBytes(string value, Encoding encoding)
        {
            byte[] ret = null;
            if (value != null){
                ret = encoding.GetBytes(value);
            }
            return ret;
        }

        public static string format(string value, string pattern)
        {
            string ret = pattern + value;

            ret = ret.Substring(value.Length, pattern.Length);

            return ret;
        }

        public static string format(int value, string pattern)
        {
            string ret = pattern + value;

            ret = ret.Substring(value.ToString().Length, pattern.Length);

            return ret;
        }
    }
}