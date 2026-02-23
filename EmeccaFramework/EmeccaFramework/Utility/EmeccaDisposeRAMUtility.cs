using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Emecca.Framework.Common;

namespace Emecca.Framework.Utility
{
   public class EmeccaDisposeRAMUtility
    {
        private static EmeccaDisposeRAMUtility instance = new EmeccaDisposeRAMUtility();
        public static EmeccaDisposeRAMUtility Instance
        {
            get { return instance; }
        }

        private EmeccaDisposeRAMUtility() { }

        [DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(
            IntPtr process,
            int minSize,
            int maxSize
            );


       /// <summary>
       /// 
       /// </summary>
       /// <param name="procName">process Name</param>
       /// <returns></returns>
        public bool MinOfProcess(string procName)
        {
            Process[] proc = Process.GetProcessesByName(procName);
            if (null == proc || proc.Length <= 0)
                return false;

            foreach (Process pc in proc)
            {
                long memorySize = pc.PagedMemorySize64 / 1024;//(KB)
                SetProcessWorkingSetSize(pc.Handle, -1, -1);//直接回收。不设限
                //if (memorySize > 200000)//大于约200M,就回收
                //{
                //    SetProcessWorkingSetSize(pc.Handle, -1, -1);
                //}
            }
            return true;

        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="procName"> process Name</param>
        /// <param name="baseline">达到这个数（以Megabite（MB）计算），就执行</param>
       /// <returns></returns>
        public bool MinOfProcess(string procName, int baseline)
        {
            Process[] proc = Process.GetProcessesByName(procName);
            if (null == proc || proc.Length <= 0)
                return false;

            foreach (Process pc in proc)
            {
                long memorySize = pc.PagedMemorySize64 / 1024;//(KB)

                if (memorySize >= baseline*1024)
                {
                    SetProcessWorkingSetSize(pc.Handle, -1, -1);
                }
            }
            return true;
        }

       //釋放當前程式RAM.
        public bool disposeRAM()
        {
            bool isUpdateSuccess = false;
            try
            {
                string processFullName = Process.GetCurrentProcess().MainModule.FileName;

                string processName = processFullName.Substring(processFullName.LastIndexOf("\\") + 1, processFullName.LastIndexOf(".exe") - processFullName.LastIndexOf("\\") - 1);
              isUpdateSuccess= MinOfProcess(processName);
            }
            catch (Exception ee)
            {
                EmeccaLogger.getEmeccaLogger().Error("記憶體回收發生錯誤:"+ee.Message, ee);
                throw ee;
            }

            return isUpdateSuccess;
        }
    }
}
