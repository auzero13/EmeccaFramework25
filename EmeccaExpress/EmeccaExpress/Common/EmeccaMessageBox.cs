using System;
using System.Windows.Forms;
using Emecca.Framework.Common;

namespace Emecca.Express.Common
{
    public partial class EmeccaMessageBox : DevExpress.XtraEditors.XtraForm
    {
        private DialogResult result = DialogResult.None;
        public EmeccaMessageBox()
        {
            InitializeComponent();

        }

        public static DialogResult Show(string text)
        {
            EmeccaMessageBox msgBox = new EmeccaMessageBox();
            msgBox.Text = "承啟醫系科室管理系統";
            return msgBox.execute(text);
        }


        public static DialogResult Show(string strMessage, string strTitle)
        {
            EmeccaMessageBox msgBox = new EmeccaMessageBox();
            msgBox.Text = strTitle;
            msgBox.cmdCancel.Visible = false;

            msgBox.ShowInTaskbar = false;

            return msgBox.execute(strMessage);
        }

        public static DialogResult Show(EmeccaException ee)
        {
            EmeccaMessageBox msgBox = new EmeccaMessageBox();
            string strMessage = "EmeccaException : \r\n";
            strMessage = strMessage + ee.Message;
            msgBox.Text = "EmeccaException";
            msgBox.cmdCancel.Visible = false;

            msgBox.ShowInTaskbar = false;

            return msgBox.execute(strMessage);
        }

        public static DialogResult Show(Exception ee)
        {
            EmeccaMessageBox msgBox = new EmeccaMessageBox();
            string strMessage = "Exception : \r\n";
            strMessage = strMessage + ee.Message;
            msgBox.Text = "Exception";
            msgBox.cmdCancel.Visible = false;

            msgBox.ShowInTaskbar = false;

            return msgBox.execute(strMessage);
        }

        public DialogResult execute(string text)
        {
            result = DialogResult.None;
            txtMessage.Text = text;
            ShowDialog();
            return result;
        }

        private void cmdConfirm_Click(object sender, EventArgs e)
        {
            result = DialogResult.OK;
            Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            result = DialogResult.Cancel;
            Close();
        }

    }
}