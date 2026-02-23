using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emecca.Medical.HookManager;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            EmeccaKeyboardHook.Enabled = true;
            EmeccaKeyboardHook.GlobalKeyDown += new EventHandler<EmeccaKeyboardHook.KeyEventArgs>(EmeccaKeyboardHook_GlobalKeyDown);
        }

        void EmeccaKeyboardHook_GlobalKeyDown(object sender, EmeccaKeyboardHook.KeyEventArgs e)
        {
            if (e.Key== System.Windows.Input.Key.A)
            {
                MessageBox.Show("A");
            }
        }
    }
}
