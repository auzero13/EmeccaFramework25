using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Emecca.Framework.Common
{
    public class EmeccaLicense
    {

        private XmlDocument xml = new XmlDocument();
        private ArrayList coreList = new ArrayList();
        private ArrayList keyList = new ArrayList();
        private ArrayList infoList = new ArrayList();
        private IPAddress[] ipAddress = null;
        private string[] macAddress = null;
        private string mode = "RUNTIME";


        public void setMode(string mode)
        {
            this.mode = mode;
        }

        public string getMode()
        {
            return this.mode;
        }

        public ArrayList getInfoList()
        {
            return infoList;
        }

        public void addBase(string core)
        {
            if (!coreList.Contains(core))
            {
                coreList.Add(core);
            }
        }

        public string checkLicense(string uri)
        {
            string ret = "INCORRECT LICENSE";
            generateLicense(uri);
            string licenseExpired = getLicenseExpired();
            string licenseKey = getLicenseKey();
            if (keyList.Contains(licenseKey))
            {
                ret = "PASS";
            }

            return ret;
        }


        public void setIpAddress(byte a1, byte a2, byte a3, byte a4)
        {
            byte[] ip = new byte[4];
            ip[0] = a1;
            ip[1] = a2;
            ip[2] = a3;
            ip[3] = a4;
            ipAddress = new IPAddress[1];
            ipAddress[0] = new IPAddress(ip);

        }

        public IPAddress[] getIpAddressList()
        {
            if (ipAddress == null)
            {
                ipAddress = Dns.GetHostByName(Dns.GetHostName()).AddressList;
            }

            return ipAddress;
        }

        public void setMacAddress(string mac)
        {
            macAddress = new string[1];
            macAddress[0] = mac;

        }

        public string[] getMacAddressList()
        {
            if (macAddress == null)
            {
                NetworkInterface[] nif = NetworkInterface.GetAllNetworkInterfaces();
                int count = nif.Count();
                for (int i = 0; i < count; i++)
                {
                    string mac = nif[i].ToString();
                    macAddress[i] = mac;
                }
            }

            return macAddress;
        }

        public void generateLicense(string uri)//string ipAddress, string macAddress, string expiredDate, string uri)
        {
            coreList.Clear();
            keyList.Clear();
            infoList.Clear();
            XmlReader reader = null;

            try
            {
                reader = XmlReader.Create(uri);
                xml.Load(reader);
            }
            catch (Exception ee)
            {
                return;
            }
            finally
            {
                reader.Close();
            }

            if ("RUNTIME".Equals(mode) || "IPADDRESS".Equals(mode))
            {
                IPAddress[] a = getIpAddressList();

                for (int i = 0; i < a.Length; i++)
                {
                    byte[] address = a[i].GetAddressBytes();
                    addBase(string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", address[0], address[1], address[2], address[3]));
                    addBase(string.Format("{0:X2}{1:X2}{2:X2}00", address[0], address[1], address[2]));
                    addBase(string.Format("{0:X2}{1:X2}0000", address[0], address[1]));
                    addBase(string.Format("{0:X2}000000", address[0]));
                }
            }

            if ("RUNTIME".Equals(mode) || "MACADDRESS".Equals(mode))
            {
                string[] m = getMacAddressList();


            }
            string expired = getLicenseExpired();
            string information = string.Format("{0}{1}", getApplicationInfo(), getActionsInfo());

            foreach (string core in coreList)
            {
                string digest = string.Format("{0}-{1}-{2}", core, expired, information);
                string key = GetMd5Str(digest);
                keyList.Add(key);
                infoList.Add(string.Format("CORE:{0} KEY:>>{1}<<", core, key));
                Console.WriteLine("\r\nCORE:{0}\r\nKEY:{1}", core, key);
            }

        }

        public string GetMd5Str(string ConvertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)));
            t2 = t2.Replace("-", "");
            return t2;

        }

        public string getLicenseKey()
        {
            XmlNodeList licenses = xml.SelectNodes("//License");
            XmlNode license = licenses[0];

            string licenseKey = GetAttributeValue(license, "key");

            return licenseKey;
        }

        public string getLicenseExpired()
        {
            XmlNodeList licenses = xml.SelectNodes("//License");
            XmlNode license = licenses[0];

            string licenseExpired = GetAttributeValue(license, "expired");

            return licenseExpired;
        }

        public string getLicenseType()
        {
            XmlNodeList licenses = xml.SelectNodes("//License");
            XmlNode license = licenses[0];

            string licenseType = GetAttributeValue(license, "type");

            return licenseType;
        }

        public string getActionsInfo()
        {
            string actionsInfo = "";
            XmlNodeList actions = xml.SelectNodes("//Actions/*");

            foreach (XmlNode action in actions)
            {
                switch (action.Name)
                {
                    case "Process":
                    case "Form":
                        string clazz = GetAttributeValue(action, "class");
                        actionsInfo = actionsInfo + string.Format("\r\n{0}", clazz);
                        Console.WriteLine(clazz);
                        break;
                    default:
                        break;
                }

            }

            return actionsInfo;
        }

        public string getApplicationInfo()
        {
            XmlNodeList applications = xml.SelectNodes("//Application");
            XmlNode application = applications[0];

            string applicationVersion = GetAttributeValue(application, "version");
            string applicationTitle = GetAttributeValue(application, "title");
            string applicationName = GetAttributeValue(application, "name");

            return string.Format("{0}:{1}:{2}", applicationName, applicationTitle, applicationVersion);
        }


        private string GetAttributeValue(XmlNode node, string name)
        {
            try
            {
                return node.Attributes[name].Value;
            }
            catch
            {
                return "";
            }
        }
    }
}
