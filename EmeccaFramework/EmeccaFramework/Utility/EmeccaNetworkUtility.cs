using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;

namespace Emecca.Framework.Utility
{
    public class EmeccaNetworkUtility
    {
        /// <summary>
        /// 取得網路存取權限
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static NetworkCredential getAuthouizedNetworkCredential(string userName, string password, string domain)
        {
            NetworkCredential networkCredential = null;
            //Added By Alan 20121214 需判斷是否需要經過domain認證
            if (string.IsNullOrEmpty(domain))
            {
                networkCredential = new NetworkCredential(userName, password);
            }
            else
            {
                networkCredential = new NetworkCredential(userName, password, domain);
            }
            return networkCredential;
        }

        /// <summary>
        /// 動態檢查網路狀態是否連線
        /// </summary>
        /// <returns></returns>
        public static bool CheckNetworkIsAvailable()
        {
            bool isAvailable = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            //都不能用，則不需要繼續判斷哪些是真正有效
            if (isAvailable == false)
            {
                return false;
            }
            
            bool networkIsAvailable = false; //預設有網路
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces(); //電腦所有網卡
            foreach (NetworkInterface nic in nics)
            {
                //先判斷非Loopback或Tunnel或是名稱沒有Virtual字眼(避免有些像VMWare裝的虛擬網卡，但是他的NetworkInterfaceType是Ethernet，就算Up，也不送)
                if (nic.NetworkInterfaceType != NetworkInterfaceType.Loopback && nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel && !nic.Description.Contains("Virtual"))
                {
                    if (nic.OperationalStatus == OperationalStatus.Up)
                    {
                        networkIsAvailable = true;
                        break;
                    }
                }
            }
            return networkIsAvailable;
        }

        /// <summary>
        /// 取得所有有效的網路卡名稱
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAvailableNicDescriptionList()
        {
            List<string> list = new List<string>();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface nic in nics)
            {
                if (nic.NetworkInterfaceType != NetworkInterfaceType.Loopback && nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                {
                    list.Add(nic.Description);
                }
            }
            return list;
        }
    }
}
