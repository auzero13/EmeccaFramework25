using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit.API.Native;

namespace Emecca.Demo
{
    public partial class FrmHyperLinkSetting : DevExpress.XtraEditors.XtraForm
    {
        public FrmHyperLinkSetting()
        {
            InitializeComponent();
        }

        public void doExecute(Hyperlink hyperlink)
        {
            textEdit1.Text = hyperlink.ToolTip;
            textEdit2.Text = hyperlink.Target;
            textEdit3.Text = hyperlink.NavigateUri;
            ShowDialog();
        }
    }
}