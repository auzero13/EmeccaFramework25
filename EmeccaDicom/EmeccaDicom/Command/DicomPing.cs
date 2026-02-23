using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leadtools.Dicom;
using Leadtools.Dicom.Scu;

using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net.NetworkInformation;
namespace Emecca.Medical.Dicom.Command
{
    public class DicomPing
    {
        public int SendCEcho(string CallingAEtitle,string peerAddress, int peerPort, string AEtitle)
        {//CallingAEtitle:我們的AETitle, peerAddress:遠端IP, peerPort:遠端port, AEtitle:遠端AEtitle
            int misec = 150;//傳輸資料間延遲毫秒
            try
            {
                using (BClient client = new BClient())
                {
                    // Connect to server
                    if (!client.IsConnected())
                        client.Connect(null, 104, peerAddress, peerPort); // connect to server
                    if (!client.Wait()) // wait for connection to finish
                    {
                        return Convert.ToInt16("0");
                        //Debug.Fail("Connection timed out");
                    }
                    Thread.Sleep(misec); //Delay 

                    // Send associate request
                    DicomAssociate associate = new DicomAssociate(true);
                    associate.Called = AEtitle;
                    associate.Calling = CallingAEtitle;
                    associate.ImplementClass = "1.2.840.114257.1";
                    associate.ImplementationVersionName = "1";
                    associate.MaxLength = 0x100000;
                    associate.AddPresentationContext(1, 0, DicomUidType.VerificationClass);
                    associate.AddTransfer(1, DicomUidType.ImplicitVRLittleEndian);
                    client.SendAssociateRequest(associate);
                    if (!client.Wait()) // wait for connection to finish
                    {
                        return Convert.ToInt16("1");
                        //Debug.Fail("SendAssociateRequest timed out");
                    }
                    Thread.Sleep(misec); //Delay

                    // Send Echo Request
                    byte pid = client.Association.FindAbstract(DicomUidType.VerificationClass);
                    client.SendCEchoRequest(pid, 1, DicomUidType.VerificationClass);
                    if (!client.Wait()) // wait for connection to finish
                    {
                        return Convert.ToInt16("2");
                        //Debug.Fail("SendCEchoRequest timed out");
                    }
                    Thread.Sleep(misec); //Delay 

                    // SendReleaseRequest
                    client.SendReleaseRequest();
                    if (!client.Wait()) // wait for connection to finish
                    {
                        return Convert.ToInt16("3");
                        //Debug.Fail("SendReleaseRequest timed out");
                    }
                    Thread.Sleep(misec); //Delay 
                    client.CloseForced(true);
                }
            }
            catch (Exception e)
            {
                return Convert.ToInt16("4");
            }

            return Convert.ToInt16("5");
        }
    }
    class Server : DicomNet
    {
        ServerConnection client;

        public Server()
            : base(null, DicomNetSecurityeMode.None)
        {
        }

        protected override void OnAccept(DicomExceptionCode error)
        {
            client = new ServerConnection();

            Accept(client);
        }
    }
    class ServerConnection : DicomNet
    {
        public ServerConnection()
            : base(null, DicomNetSecurityeMode.None)
        {
        }

        protected override void OnReceiveAssociateRequest(DicomAssociate association)
        {
            DicomAssociate retAssociation = new DicomAssociate(false);

            //Debug.Assert(association.Called == "ServerTest", "Called AETitle should be ServerTest");
            //Debug.Assert(association.Calling == "ClientTest", "Calling AETitle should be ClientTest");
            //Debug.Assert(association.ImplementClass == "1.2.840.114257.1", "Implementation class should be 1.2.840.114257.1");
            //Debug.Assert(association.ImplementationVersionName == "1", "Implementation version should be 1");
            //Debug.Assert(association.MaxLength == 0x100000, "Max length should be 0x100000");
            //Debug.Assert(association.GetAbstract(1) == DicomUidType.VerificationClass, "Presentation index 1 should be " + DicomUidType.VerificationClass);
            //Debug.Assert(association.GetTransfer(1, 0) == DicomUidType.ImplicitVRLittleEndian, "Abstract Syntax (1,0) should be " + DicomUidType.ImplicitVRLittleEndian);

            //
            // Build our Association Accept
            //

            retAssociation.Called = association.Called;
            retAssociation.Calling = association.Calling;
            retAssociation.ImplementClass = association.ImplementClass;
            retAssociation.ImplementationVersionName = association.ImplementationVersionName;
            for (int x = 0; x < association.PresentationContextCount; x++)
            {
                byte id = association.GetPresentationContextID(x);
                string abSyntax = association.GetAbstract(id);
                string trSyntax = DicomUidType.ImplicitVRLittleEndian;

                retAssociation.AddPresentationContext(id, 0, abSyntax);
                if (association.GetTransferCount(id) > 0)
                {
                    trSyntax = association.GetTransfer(id, 0);
                }

                retAssociation.AddTransfer(id, trSyntax);
            }
            SendAssociateAccept(retAssociation);
        }

        protected override void OnReceiveReleaseRequest()
        {
            SendReleaseResponse();
        }

        protected override void OnReceiveCEchoRequest(byte presentationID, int messageID, string affectedClass)
        {
            //Debug.Assert(presentationID == 1, "Presentation id should be 1");
            //Debug.Assert(messageID == 1, "Message ID should be 1");
            //Debug.Assert(affectedClass == DicomUidType.VerificationClass, "Affected class should be " + DicomUidType.VerificationClass);

            SendCEchoResponse(presentationID, messageID, DicomUidType.VerificationClass, DicomCommandStatusType.Success);
        }
    }
    class BClient : DicomNet
    {
        DicomExceptionCode _LastError = DicomExceptionCode.Success;
        AutoResetEvent waitEvent = new AutoResetEvent(false);

        public BClient()
            : base(null, DicomNetSecurityeMode.None)
        {
        }

        public DicomExceptionCode LastError
        {
            get
            {
                return _LastError;
            }
        }

        public bool Wait()
        {
            WaitReturn ret;

            _LastError = DicomExceptionCode.Success;
            ret = Utils.WaitForComplete(5 * 1000, waitEvent);

            return (ret == WaitReturn.Complete);
        }

        protected override void OnConnect(DicomExceptionCode error)
        {
            _LastError = error;
            waitEvent.Set();
        }

        protected override void OnReceiveAssociateAccept(DicomAssociate association)
        {
            waitEvent.Set();
        }

        protected override void OnReceiveReleaseResponse()
        {
            waitEvent.Set();
        }
        protected override void OnReceiveCEchoResponse(byte presentationID, int messageID, string affectedClass, DicomCommandStatusType status)
        {
            //Debug.Assert(presentationID == 1, "Presentation id should be 1");
            //Debug.Assert(messageID == 1, "Message ID should be 1");
            //Debug.Assert(affectedClass == DicomUidType.VerificationClass, "Affected class should be " + DicomUidType.VerificationClass);
            waitEvent.Set();
        }
    }
    public enum WaitReturn
    {
        Complete,
        Timeout,
    }
    class Utils
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PeekMessage(out MSG lpMsg, IntPtr hWnd,
                                       uint wMsgFilterMin, uint wMsgFilterMax,
                                       uint wRemoveMsg);

        [DllImport("user32.dll")]
        static extern bool TranslateMessage([In] ref MSG lpMsg);
        [DllImport("user32.dll")]
        static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

        const uint PM_REMOVE = 1;

        public static WaitReturn WaitForComplete(double mill, WaitHandle wh)
        {
            TimeSpan goal = new TimeSpan(DateTime.Now.AddMilliseconds(mill).Ticks);

            do
            {
                MSG msg = new MSG();

                if (PeekMessage(out msg, IntPtr.Zero, 0, 0, PM_REMOVE))
                {
                    TranslateMessage(ref msg);
                    DispatchMessage(ref msg);
                }

                if (wh.WaitOne(new TimeSpan(0, 0, 0), false))
                {
                    return WaitReturn.Complete;
                }

                if (goal.CompareTo(new TimeSpan(DateTime.Now.Ticks)) < 0)
                {
                    return WaitReturn.Timeout;
                }

            } while (true);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        public IntPtr hwnd;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public System.Drawing.Point p;
    }
}
