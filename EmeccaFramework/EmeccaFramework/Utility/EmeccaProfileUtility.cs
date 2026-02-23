using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Management;

namespace Emecca.Framework.Utility
{
    public class EmeccaProfileUtility
    {
        #region 32位程序读写64注册表

        static UIntPtr HKEY_CLASSES_ROOT = (UIntPtr)0x80000000;
        static UIntPtr HKEY_CURRENT_USER = (UIntPtr)0x80000001;
        static UIntPtr HKEY_LOCAL_MACHINE = (UIntPtr)0x80000002;
        static UIntPtr HKEY_USERS = (UIntPtr)0x80000003;
        static UIntPtr HKEY_CURRENT_CONFIG = (UIntPtr)0x80000005;

        // 关闭64位（文件系统）的操作转向   
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);
        // 开启64位（文件系统）的操作转向   
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

        // 获取操作Key值句柄   
        [DllImport("Advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint RegOpenKeyEx(UIntPtr hKey, string lpSubKey, uint ulOptions,
                               int samDesired, out IntPtr phkResult);
        //关闭注册表转向（禁用特定项的注册表反射）   
        [DllImport("Advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern long RegDisableReflectionKey(IntPtr hKey);
        //使能注册表转向（开启特定项的注册表反射）   
        [DllImport("Advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern long RegEnableReflectionKey(IntPtr hKey);
        //获取Key值（即：Key值句柄所标志的Key对象的值）   
        [DllImport("Advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int RegQueryValueEx(IntPtr hKey, string lpValueName, int lpReserved,
                                                  out uint lpType, System.Text.StringBuilder lpData,
                                                  ref uint lpcbData);

        private static UIntPtr TransferKeyName(string keyName)
        {
            switch (keyName)
            {
                case "HKEY_CLASSES_ROOT":
                    return HKEY_CLASSES_ROOT;
                case "HKEY_CURRENT_USER":
                    return HKEY_CURRENT_USER;
                case "HKEY_LOCAL_MACHINE":
                    return HKEY_LOCAL_MACHINE;
                case "HKEY_USERS":
                    return HKEY_USERS;
                case "HKEY_CURRENT_CONFIG":
                    return HKEY_CURRENT_CONFIG;
            }

            return HKEY_CLASSES_ROOT;
        }

        public static string Get64BitRegistryKey(string parentKeyName, string subKeyName, string keyName)
        {
            int KEY_QUERY_VALUE = (0x0001);
            int KEY_WOW64_64KEY = (0x0100);
            int KEY_ALL_WOW64 = (KEY_QUERY_VALUE | KEY_WOW64_64KEY);

            try
            {
                //将Windows注册表主键名转化成为不带正负号的整形句柄（与平台是32或者64位有关）   
                UIntPtr hKey = TransferKeyName(parentKeyName);

                //声明将要获取Key值的句柄   
                IntPtr pHKey = IntPtr.Zero;
                int i=IntPtr.Size;
                //记录读取到的Key值   
                StringBuilder result = new StringBuilder("".PadLeft(1024));
                uint resultSize = 1024;
                uint lpType = 0;

                //关闭文件系统转向    
                IntPtr oldWOW64State = new IntPtr();
                if (Wow64DisableWow64FsRedirection(ref oldWOW64State))
                {
                    //获得操作Key值的句柄   
                    RegOpenKeyEx(hKey, subKeyName, 0, KEY_ALL_WOW64, out pHKey);

                    //关闭注册表转向（禁止特定项的注册表反射）   
                    RegDisableReflectionKey(pHKey);

                    //获取访问的Key值   
                    RegQueryValueEx(pHKey, keyName, 0, out lpType, result, ref resultSize);

                    //打开注册表转向（开启特定项的注册表反射）   
                    RegEnableReflectionKey(pHKey);
                }
                byte[] bytes = System.Text.Encoding.Default.GetBytes(result.ToString());
                //打开文件系统转向   
                Wow64RevertWow64FsRedirection(oldWOW64State);

                //返回Key值   
                return result.ToString().Trim();
            }
            catch
            {
                return null;
            }
        }
        public static string GetRegistryValueSafe(string subKeyName, string keyName)
        {
            string value = null;

            // 步驟 1: 先嘗試讀取 64 位元路徑 (Native Path)
            // 對應路徑: HKLM\SOFTWARE\Emecca\IMSv2012
            value = ReadFromRegistryView(RegistryView.Registry64, subKeyName, keyName);

            // 如果 64 位元區有值，直接回傳
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            // 步驟 2: 如果找不到，嘗試讀取 32 位元路徑 (Legacy/WoW64 Path)
            // 對應路徑: HKLM\SOFTWARE\WOW6432Node\Emecca\IMSv2012
            value = ReadFromRegistryView(RegistryView.Registry32, subKeyName, keyName);

            return value;
        }

        private static string ReadFromRegistryView(RegistryView view, string subKeyName, string keyName)
        {
            try
            {
                // 關鍵：使用 OpenBaseKey 並指定 RegistryView，完全取代複雜的 API 呼叫
                using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, view))
                {
                    using (var subKey = baseKey.OpenSubKey(subKeyName))
                    {
                        if (subKey != null)
                        {
                            object result = subKey.GetValue(keyName);
                            return result?.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 建議記錄 Log: Console.WriteLine($"Registry Read Error: {ex.Message}");
            }
            return null;
        }
        /// <summary>
        /// 讀取 HKEY_LOCAL_MACHINE 的SOFTWARE\Emecca\節點下的登入程式
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static string getRegistryData(string assemblyName, string keyName)
        {
            string keyValue = "";
            string subKeyName = "";
            try
            {
                subKeyName = @"SOFTWARE\" + assemblyName;
                if ("32".Equals(Distinguish64or32System()))
                {
                    RegistryKey registrKey = Registry.LocalMachine;
                    registrKey = registrKey.OpenSubKey(subKeyName);
                    keyValue = registrKey.GetValue(keyName) as string;
                }
                else
                {
                    keyValue = GetRegistryValueSafe( subKeyName, keyName); //Get64BitRegistryKey("HKEY_LOCAL_MACHINE",)
                }
            }
            catch
            {
                keyValue = null;
            }
            return keyValue;
        }

        /// <summary>
        /// 獲取當前系統位數
        /// </summary>
        /// <returns></returns>
        public static string Distinguish64or32System()
        {
            try
            {
                string addressWidth = String.Empty;
                ConnectionOptions mConnOption = new ConnectionOptions();
                ManagementScope mMs = new ManagementScope("\\\\localhost", mConnOption);
                ObjectQuery mQuery = new ObjectQuery("select AddressWidth from Win32_Processor");
                ManagementObjectSearcher mSearcher = new ManagementObjectSearcher(mMs, mQuery);
                ManagementObjectCollection mObjectCollection = mSearcher.Get();
                foreach (ManagementObject mObject in mObjectCollection)
                {
                    addressWidth = mObject["AddressWidth"].ToString();
                }
                return addressWidth;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()); return String.Empty;
            }
        }
        #endregion
    }
}
