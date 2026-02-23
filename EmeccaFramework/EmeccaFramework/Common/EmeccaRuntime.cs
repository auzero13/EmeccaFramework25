using System;
using System.Collections;
using System.IO;
using System.Xml;
using Microsoft.Win32;
using Emecca.Framework.Utility;
using log4net;
using EnterpriseDT.Net.Ftp;
namespace Emecca.Framework.Common
{
    public class EmeccaRuntime
    {
        public const string FrameworkReservedVariableLeading = "@EmeccaFramework";

        private static EmeccaRuntime instance;
        private string workstationId = "00";
        private Hashtable dataSources = new Hashtable();
        private Hashtable loggers = new Hashtable();
        private Hashtable variables = new Hashtable();
        private Hashtable emeccaObjects = new Hashtable();
        
        private XmlDocument systemXml = null;

        public static EmeccaRuntime getInstance()
        {
            if (instance == null)
            {
                instance = new EmeccaRuntime();
            }
            return instance;
        }

        /// <summary>
        /// 系統初始化設定
        /// </summary>
        /// <param name="workDirectory">工作目錄，固定為C:\Emecca</param>
        /// <param name="assemblyName">產品名稱，舉例IMSV2012</param>
        /// <param name="nameSpace">該產品下面所有Form create時的DefaultNameSpace</param>
        /// <returns></returns>
        public EmeccaProfile doInitial(string workDirectory, string assemblyName,string nameSpace)
        {
            setVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".WorkDirectory", workDirectory);
            setVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".AssemblyName", assemblyName);
            setVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".NameSpace", nameSpace);
            /**
             * 在Emecca Framework 發展的所有系統因可能有相同的Home Directory 所以各系統利用 NameSpace 做為區分
             * 因此系統一開始必須先指定 NameSpace
             * 
             * 建議一開始指定相關所有的變數
             * .WorkDirectory = Emecca 相關系統的主路徑 預設為 C:\Emecca
             * .SystemXml = SystemXml 設定路徑,預設為 WorkDirectory + "\\" + AssemblyName + "\\CONF\\SYSTEM.XML"
             * .LogXml = SystemXml 設定路徑,預設為 WorkDirectory + "\\" + AssemblyName + "\\CONF\\LogConfig.xml"
             * 
             * .SplashScreenImage = 啟動畫面的路徑 預設為 WorkDirectory + "\\" + AssemblyName + "\\RES\\" + SplashScreen.JPG

             * .NameSpace = 預設設為Project NameSpace
             * .AssemblyName = 預設設為Project AssemblyName
             * 
             * .EmeccaProfile = 由 DeSerialize 取得的物件用以紀錄以上相關設定路徑,預設為 Windows User Home + "\\" + NameSpace + "." + EmeccaProfile
             *  Note By Alan 20120417 因為客戶端可能登入的User都不一樣，所以會造成每個User都要重新建立Profile的問題，目前增加
             *  讀取Registry變數=
             * */
            EmeccaProfile profile = getEmeccaProfile();
            
            if (profile.CreateDateTime == null)
            {
                profile = new EmeccaProfile();
                profile.WorkDirectory = workDirectory;
                profile.CreateDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                profile.LookAndFeel = "";
                profile.LogXml = getVariable(FrameworkReservedVariableLeading + ".LogXml") as string;
                profile.SystemXml = getVariable(FrameworkReservedVariableLeading + ".SystemXml") as string;

                
                setEmeccaProfile(profile);
            }
            setVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".EmeccaProfile", profile);
            return profile;
        }
        public string checkLicense()
        {
            return "PASS";
        }

        public string checkPermission(string objectName)
        {
            return "PASS";
        }

        public string getWorkDirectory()
        {
            string dir = getVariable(FrameworkReservedVariableLeading + ".WorkDirectory") as string;
            if (dir == null)
            {
                dir = System.AppDomain.CurrentDomain.BaseDirectory;
                setVariable(FrameworkReservedVariableLeading + ".WorkDirectory", dir);
            }
            return dir;
        }


        public EmeccaProfile getEmeccaProfile()
        {
           // EmeccaProfile profile = getVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".EmeccaProfile") as EmeccaProfile;
            EmeccaProfile profile  = doDeserialize(typeof(EmeccaProfile)) as EmeccaProfile;
            
            // still can not get EmeccaProfile then create a new one
            if (profile == null)
            {
                profile = new EmeccaProfile();
            }

           // setVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".EmeccaProfile",profile);

            return profile;
        }

        public void setEmeccaProfile(EmeccaProfile profile)
        {
            doSerialize(profile);
        }
        

        private string _objId = "0000000000000000";
        /* Marked By Alan 20121203 目前workstationId都固定
        public string createObjectId()
        {
            string objId = DateTime.Now.ToString("yyyyMMddHHmmss") + this.workstationId;
            long nObjId = Convert.ToInt64(objId);
            long _nObjId = Convert.ToInt64(_objId);
            if (nObjId > _nObjId)
            {
                _objId = objId;
                return objId;
            }
            else
            {
                _nObjId++;
                _objId = _nObjId.ToString();
                return _nObjId.ToString();
            }
        }
        */
        /// <summary>
        /// 產生系統時間Timestamp，最後面加2碼,00-99
        /// </summary>
        /// <returns></returns>
        public string createObjectId()
        {
            string objId = DateTime.Now.ToString("yyyyMMddHHmmssff");
            long nObjId = Convert.ToInt64(objId);
            long _nObjId = Convert.ToInt64(_objId);
            if (nObjId > _nObjId)
            {
                _objId = objId;
                return objId;
            }
            else
            {
                _nObjId++;
                _objId = _nObjId.ToString();
                return _nObjId.ToString();
            }
        }


        public string getDataSourceString(string name)
        {
            if (name == null)
                return null;

            string key = name.ToUpper().Trim();
            string strDataSource = dataSources[key] as string;

            return strDataSource;
        }

        public void setDataSourceString(string name, string strDataSource)
        {
            if (name == null)
                return;

            string key = name.ToUpper().Trim();
            if (dataSources.ContainsKey(key))
            {
                dataSources.Remove(key);
            }

            dataSources.Add(key, strDataSource);
        }

        public object getVariable(string key)
        {
            return variables[key];
        }

        public void setVariable(string key, object variable)
        {
            if (variables.ContainsKey(key))
            {
                variables.Remove(key);
            }
            variables.Add(key, variable);
        }

        public EmeccaObject getEmeccaObject(string key)
        {
            EmeccaObject emecaObjectInstance = emeccaObjects[key] as EmeccaObject;

            return emecaObjectInstance;
        }

        public void setEmeccaObject(string key, EmeccaObject instance)
        {
            if (emeccaObjects.ContainsKey(key))
            {
                emeccaObjects.Remove(key);
            }
            emeccaObjects.Add(key, instance);
        }


        //private static string getSettingPath(Type clazz)
        //{
        //    string nameSpace = EmeccaRuntime.getInstance().getVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".DefaultNameSpace") as string;
        //    string userHomePath = Environment.GetEnvironmentVariable("USERPROFILE"); //Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        //    return userHomePath + "\\" + nameSpace + "." + clazz.Name;
        //}
        /// <summary>
        /// Serialize EmeccaProfile
        /// </summary>
        /// <param name="instance"></param>
        public void doSerialize(object instance)
        {
            System.Xml.Serialization.XmlSerializer xs
               = new System.Xml.Serialization.XmlSerializer(instance.GetType());
            string path = EmeccaRuntime.getInstance().getVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".NameSpace") as string;
            string file = AppDomain.CurrentDomain.BaseDirectory + path + ".EmeccaProfile";
            StreamWriter writer = File.CreateText(file);
            xs.Serialize(writer, instance);
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// EmeccaProfile的Serialize 
        /// chris  2012.05.29
        /// </summary>
        /// <param name="instance"></param>
        public void doEmeccaProfileSerialize(object instance)
        {
            string path = EmeccaRuntime.getInstance().getVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".NameSpace") as string;
            string file = AppDomain.CurrentDomain.BaseDirectory + path + ".EmeccaProfile";
            doSerialize(instance, file);
        }

        /// <summary>
        /// 通用的Serialize 
        /// chris  2012.05.29
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="filename"></param>
        public void doSerialize(object instance,string filename)
        {
            System.Xml.Serialization.XmlSerializer xs
               = new System.Xml.Serialization.XmlSerializer(instance.GetType());
            StreamWriter writer = File.CreateText(filename);
            xs.Serialize(writer, instance);
            writer.Flush();
            writer.Close();
        }


        /// <summary>
        /// 讀取EmeccaProfile
        /// </summary>
        /// <param name="clazz">IMSv2012.EmeccaProfile</param>
        /// <returns></returns>
        public object doDeserialize(Type clazz)
        {
            object instance = null;
            try
            {
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(clazz);
                string nameSpace = getVariable(FrameworkReservedVariableLeading + ".NameSpace") as string;

                using (StreamReader reader = File.OpenText(String.Format("{0}\\{1}.{2}", AppDomain.CurrentDomain.BaseDirectory, nameSpace, clazz.Name)))
                {
                    instance = xs.Deserialize(reader);
                }
            }
            catch (Exception ee)
            {
               EmeccaLogger.getEmeccaLogger().Error(string.Format("取得EmeccaProfile發生錯誤{0}",ee.Message), ee);
                instance = null;
            }
            return instance;
        }


        /// <summary>
        /// EmeccaProfile的Deserialize
        /// chris 2012.05.29
        /// </summary>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public object doEmeccaProfileDeserialize(Type clazz)
        {
            string nameSpace = getVariable(FrameworkReservedVariableLeading + ".NameSpace") as string;
            string filePath = String.Format("{0}\\{1}.{2}", AppDomain.CurrentDomain.BaseDirectory, nameSpace, clazz.Name);
           return doDeserialize(clazz, filePath);
        }

        /// <summary>
        /// 通用Deserialize
        /// chris 2012.05.29
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public object doDeserialize(Type clazz,string filePath)
        {
            object instance = null;
            bool isExists = true;

            try
            {
                isExists = File.Exists(filePath);
            }
            catch (Exception ee)
            {
                EmeccaLogger.getEmeccaLogger().Error(string.Format("doDeserialize 验证路径發生錯誤{0}", ee.Message), ee);
                instance = null;
            }
             if (isExists)
             {
                 try
                 {
                     System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(clazz);

                     using (StreamReader reader = File.OpenText(filePath))
                     {
                         instance = xs.Deserialize(reader);
                     }
                 }
                 catch (Exception ee)
                 {
                     EmeccaLogger.getEmeccaLogger().Error(string.Format("doDeserialize發生錯誤{0}", ee.Message), ee);
                     instance = null;
                 }
             }
            return instance;
        }

        /*
        public void doSerializeEmeccaProfile(EmeccaProfile instance, string applicationAndKey)
        {
            System.Xml.Serialization.XmlSerializer xs
               = new System.Xml.Serialization.XmlSerializer(instance.GetType());
            StreamWriter writer = File.CreateText(getRegistryData(applicationAndKey));
            xs.Serialize(writer, instance);
            writer.Flush();
            writer.Close();
        }
        */
        /*
        public EmeccaProfile doDeserializeEmeccaProfile(string applicationAndKey)
        {
            EmeccaProfile instance = null;
            try
            {
                System.Xml.Serialization.XmlSerializer xs
                   = new System.Xml.Serialization.XmlSerializer(typeof(EmeccaProfile));
                StreamReader reader = File.OpenText(getRegistryData(applicationAndKey));
                instance = xs.Deserialize(reader) as EmeccaProfile;
                reader.Close();
            }
            catch (Exception ee)
            {
                throw new Exception(string.Format("無法取得系統設定{0}錯誤", applicationAndKey), ee);
            }
            return instance;
        }
        */
        //public string getRegistryData(string nameSpace, string keyName)
        //{
        //    string keyValue = "";
        //    string subKeyName = "";
        //    try
        //    {
        //        subKeyName = @"SOFTWARE\Emecca\" + nameSpace;
        //        keyValue = EmeccaProfileUtility.Get64BitRegistryKey("HKEY_LOCAL_MACHINE", subKeyName, keyName);
        //    }
        //    catch
        //    {
        //        keyValue =  null;
        //    }
        //    return keyValue; 
        //}

        public XmlDocument getSystemXML()
        {
            try
            {
                if (systemXml == null)
                {
                    systemXml = new XmlDocument();

                    string systemXmlPath = EmeccaRuntime.getInstance().getVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".SystemXml") as string;
                    
                    if (systemXmlPath == null)
                    {
                        throw new EmeccaException(string.Format("無法載入System.xml={0}，請確認!", systemXmlPath));
                    }

                    XmlReader reader=null;
                    if (systemXmlPath.StartsWith("ftp://"))
                    {
                        string[] array = systemXmlPath.Split(  new char[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
                        string host = array[1];
                        string remoteFile = systemXmlPath.Split(new string[]{host+"/"},StringSplitOptions.RemoveEmptyEntries)[1];
                        Byte[] xmlBytes = GetFTPFile(host, "endo-public", "endo2161", remoteFile);
                        //Byte[] xmlBytes = GetFTPFile("array[1]", "Chris", "123", "CONF/SYSTEM.XML");
                        MemoryStream xmlstream = new MemoryStream(xmlBytes);
                        xmlstream.Position = 0;
                        reader = XmlReader.Create(xmlstream);
                    }
                    else
                    {
                        reader = XmlReader.Create(systemXmlPath);
                    }
                    systemXml.Load(reader);
                    return systemXml;
                }
                else
                {
                    return systemXml;
                }
            }
            catch (Exception ex)
            {

                EmeccaLogger.getEmeccaLogger().Fatal("取得System.xml發生錯誤" + ex.Message);
                return null;
            }
        }

        public byte[] GetFTPFile(string host, string user, string password, string remoteFile)
        {
            byte[] xmlFile = null;
            FTPClient ftp = new FTPClient(host);
            ftp.Login(user, password);  //登陸FTP服務
            ftp.ConnectMode = FTPConnectMode.PASV; //設置為被動模式
            ftp.TransferType = FTPTransferType.BINARY;    //傳輸類型為BINARY
            xmlFile = ftp.Get(remoteFile);
            ftp.Quit();
            return xmlFile;
        }

        /**
         * processException designed by only using in framework to handle some exception 
         * will cause whole system crash and need to dump enviropnment to debug
         * 
         * TODO : dump application environment and exit
         * */
        public static void processException(Exception ee, string message)
        {
            throw new Exception("系統設定錯誤", ee);
        }

        /// <summary>
        /// 讀取登入程式，并驗證目錄或檔案是否存在，不存在就拋出異常
        /// </summary>
        /// <param name="FileOrDirectory">檔案路徑或者目錄</param>
        /// <param name="isFileOrDirectory">true :file ; false:Directory</param>
        /// <returns></returns>
        public string loadRegistryKeyAndInitial(string regSubKey, string key, bool isFileOrDirectory, bool needThrowException)
        {
            bool exists = false;

            string fileOrDirectory = EmeccaProfileUtility.getRegistryData(regSubKey, key);
           
            exists = !string.IsNullOrEmpty(fileOrDirectory);

            if (exists && !fileOrDirectory.StartsWith("ftp", StringComparison.CurrentCultureIgnoreCase))
            {
                if (isFileOrDirectory)
                {
                    exists = File.Exists(fileOrDirectory);
                }
                else
                {
                    exists = Directory.Exists(fileOrDirectory);
                }
            }
            if (needThrowException && exists == false)
            {
                EmeccaLogger.getEmeccaLogger().Fatal("登入程式配置錯誤,找不到檔案路徑：錯誤項為：" + key);
                throw new FileNotFoundException("登入程式配置錯誤,找不到檔案路徑：錯誤項為：" + key);
            }
            EmeccaRuntime.getInstance().setVariable(EmeccaRuntime.FrameworkReservedVariableLeading + "." + key, fileOrDirectory);
            return fileOrDirectory;
        }
    }
}
