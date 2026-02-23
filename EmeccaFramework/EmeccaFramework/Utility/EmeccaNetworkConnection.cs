using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Emecca.Framework.Utility
{
    /// <summary>
    /// 用來登入網路磁碟機，使用mpr.dll，配合網域、帳號和密碼進行登入、解決讀取權限問題
    /// networkName example : \\\\192.168.8.100\\sharefolder
    /// </summary>

    public class EmeccaNetworkConnection : IDisposable
    {
        string _networkName;
        string _userName;
        string _userPassword;
        string _userDomain;

        public EmeccaNetworkConnection(string networkName, NetworkCredential credentials)
        {
            _networkName = networkName;
            _userName = credentials.UserName;
            _userPassword = credentials.Password;
            _userDomain = credentials.Domain;
        }

        public int connectToRemote()
        {
            NetResource netResource = new NetResource()
            {
                Scope = ResourceScope.GlobalNetwork,
                ResourceType = ResourceType.Disk,
                DisplayType = ResourceDisplaytype.Share,
                RemoteName = _networkName
            };

            string userName = string.IsNullOrEmpty(_userDomain) ? _userName : string.Format(@"{0}\{1}", _userDomain, _userName);

            int result = WNetAddConnection2(netResource, _userPassword, userName, 0);

            if (result != 0)
            {
                string errorMessage = string.Format("Error connecting to remote share '{0}' using Domain = {1}, UserName = '{2}' return Code = {3}", _networkName, _userDomain, _userName, result);
                throw new Win32Exception(result, errorMessage);
            }

            return result;
        }


        public static int disconnectFromRemote(string networkName, int disConnectType, bool fourceFlag)
        {
            int result = WNetCancelConnection2(networkName, disConnectType, fourceFlag);
            
            if (result != 0)
            {
                string errorMessage = string.Format("Error disconnecting from remote share '{0}', disConnectType = '{1}' return Code = {2}", networkName, disConnectType, result);
                throw new Win32Exception(result, errorMessage);
            }

            return result;
        }


        ~EmeccaNetworkConnection()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            WNetCancelConnection2(_networkName, 0, true);
        }

        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(NetResource netResource,string password, string username, int flags);

        [DllImport("mpr.dll")]
        
        private static extern int WNetCancelConnection2(string name, int flags,bool force);
    }


    [StructLayout(LayoutKind.Sequential)]
    public class NetResource
    {
        public ResourceScope Scope;
        public ResourceType ResourceType;
        public ResourceDisplaytype DisplayType;
        public int Usage;
        public string LocalName;
        public string RemoteName;
        public string Comment;
        public string Provider;
    }

    public enum ResourceScope : int
    {
        Connected = 1,
        GlobalNetwork,
        Remembered,
        Recent,
        Context
    };

    public enum ResourceType : int
    {
        Any = 0,
        Disk = 1,
        Print = 2,
        Reserved = 8,
    }

    public enum ResourceDisplaytype : int
    {
        Generic = 0x0,
        Domain = 0x01,
        Server = 0x02,
        Share = 0x03,
        File = 0x04,
        Group = 0x05,
        Network = 0x06,
        Root = 0x07,
        Shareadmin = 0x08,
        Directory = 0x09,
        Tree = 0x0a,
        Ndscontainer = 0x0b
    }
}
