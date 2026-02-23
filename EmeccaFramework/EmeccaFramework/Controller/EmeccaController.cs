using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emecca.Framework.Common;
using System.IO;

namespace Emecca.Framework.Controller
{
    public class EmeccaController
    {
        private static EmeccaController instance = new EmeccaController();
        public static EmeccaController Instance
        {
            get { return instance; }
        }
        public string getSystemDirectory(string path, bool create)
        {
            string dir =EmeccaRuntime.getInstance().getWorkDirectory();
          
            if (path != null)
            {
                if (!dir.EndsWith("\\"))
                {
                    dir = dir + "\\";
                }
                dir = dir + path;

                if (!Directory.Exists(dir) && create)
                {
                    if (create)
                    {
                        Directory.CreateDirectory(dir);
                    }
                    else
                    {
                        dir = EmeccaRuntime.getInstance().getWorkDirectory();
                    }
                }
            }
            return dir;
        }
    }
}
