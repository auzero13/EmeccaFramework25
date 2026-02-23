using System;

namespace Emecca.Framework.Common
{
    public class EmeccaException : Exception
    {
        public EmeccaException(string message)
            : base(message)
        {
            GetCallStack(this);
        }
        public static string GetCallStack(Exception ee)
        {
            System.Diagnostics.StackTrace callStack = new
            System.Diagnostics.StackTrace( ee);
            string s = "";
            int index = 0;
            while (true)
            {
                System.Diagnostics.StackFrame frame =
                callStack.GetFrame(index);
                if (frame == null) break;
                System.Reflection.MethodBase method = frame.GetMethod();

                if (index > 0) s = " --> " + s;
                s = method.DeclaringType.Name + "." + method.Name + "()" + s;
                index++;
            }
            EmeccaLogger.getEmeccaLogger().Fatal(GetCallStack(ee));
            return (s);
        }


    }
}
