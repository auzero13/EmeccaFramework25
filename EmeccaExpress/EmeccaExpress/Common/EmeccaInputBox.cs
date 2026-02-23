using System;

namespace Emecca.Express.Common
{
    public partial class EmeccaInputBox : DevExpress.XtraEditors.XtraForm
    {
        private string ret = null;
        public EmeccaInputBox()
        {
            InitializeComponent();
        }

        public string doExecute(string value)
        {
            txtOriginal.Text = value;
            txtValue.Text = value;
            ShowDialog();
            return ret;
        }

        private void cmdConfirm_Click(object sender, EventArgs e)
        {
            ret = txtValue.Text;
            Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            ret = null;
            Close();
        }
    }
}