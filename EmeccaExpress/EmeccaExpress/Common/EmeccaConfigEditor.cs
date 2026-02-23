using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Emecca.Framework.Common;
using Emecca.Express.Common;

namespace Emecca.Framework.UI
{
    public partial class EmeccaConfigEditor : DevExpress.XtraEditors.XtraForm
    {
        private object ret = null;
        public EmeccaConfigEditor()
        {
            InitializeComponent();
        }

        public object execute(object instance,string title)
        {
            this.Text = title;
            propertyEditor.SelectedObject = instance;
            if (Control.ModifierKeys == Keys.Control || instance == null)
            {
                ShowDialog();
            }
            else
            {
                ret = instance;
            }
            return ret;
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            EmeccaProfile profile = propertyEditor.SelectedObject as EmeccaProfile;
            if (profile != null)
            {
                //一定要指定SYSTEM.XML的位置
                if (string.IsNullOrEmpty(profile.SystemXml))
                {
                    EmeccaMessageBox.Show("[SystemXml]必填!", "承啓醫系科室管理系統");
                    return;
                }
                //一定要指定WorkDirectory的位置，固定為C:\Emecca
                if (string.IsNullOrEmpty(profile.WorkDirectory))
                {
                    EmeccaMessageBox.Show("[WorkDirectory]必填!", "承啓醫系科室管理系統");
                    return;
                }

                //一定要指定LogXml的位置，Runtime啟動時就初始Log
                if (string.IsNullOrEmpty(profile.LogXml))
                {
                    EmeccaMessageBox.Show("[LogXml]必填!", "承啓醫系科室管理系統");
                    return;
                }
            }
            ret = propertyEditor.SelectedObject;
      
            Close();
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            ret = null;
            Close();
        }
    }
}