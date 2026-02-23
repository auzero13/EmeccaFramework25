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
    public partial class FrmMain : DevExpress.XtraEditors.XtraForm
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void richEditControl1_HyperlinkFormShowing(object sender, DevExpress.XtraRichEdit.HyperlinkFormShowingEventArgs e)
        {
            e.Handled = true;
            bool isEditing = false;
            foreach (Hyperlink hyperlink in richEditControl1.Document.Hyperlinks)
            {
                if (hyperlink.Range.Contains(richEditControl1.Document.CaretPosition))
                {
                    MessageBox.Show("Editing Hyperlink!!");
                    new FrmHyperLinkSetting().doExecute(hyperlink);
                    isEditing = true;
                }
            }

            if (!isEditing)
            {

                Hyperlink hyperlink = richEditControl1.Document.Hyperlinks.Create(richEditControl1.Document.Selection);

                new FrmHyperLinkSetting().doExecute(hyperlink);
                hyperlink.NavigateUri = "mailto:" ;
                MessageBox.Show("Creating Hyperlink!!");
            }
        }

        private void richEditControl1_MouseClick(object sender, MouseEventArgs e)
        {
            /*
             *        collection = richEditControl1.Document.GetImages(richEditControl1.Document.Selection)
        If collection.Count <> 0 Then
            Dim item As New DXMenuItem("Edit Image", New EventHandler(AddressOf EditImage))
            e.Menu.Items.Add(item)
        End If

             * */
            labelControl1.Text = DateTime.Now.ToString() + "\tMouseClick:" + e.Clicks;
            if (e.Clicks == 2)
            {
                var images = richEditControl1.Document.Images.Get(richEditControl1.Document.Selection);
                if (images.Count != 0)
                {
                    labelControl1.Text = DateTime.Now.ToString() + "\tMouseClick:" + sender + " Result : Is IMAGE!";
                }
                else
                {
                    labelControl1.Text = DateTime.Now.ToString() + "\tMouseClick:" + sender + " Result : Not an IMAGE!";
                }
            }

            
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void richEditControl1_HyperlinkClick(object sender, DevExpress.XtraRichEdit.HyperlinkClickEventArgs e)
        {
            MessageBox.Show("TT");
        }
    }
}