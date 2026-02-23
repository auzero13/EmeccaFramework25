using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Emecca.Main
{
    public partial class Form2 : Form
    {
        private Form1 frm = new Form1();
        private int i = 0;
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frm.Show();
            //frm.doStartup(null);
        }

        private void button2_Click(object sender, EventArgs e)
        {

                frm.doReportProgressive("Value is " + i, i++);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //frm.doShutdown();
            frm.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            //frm.executor = test;
            frm.doStartup(test);
            frm.Close();
        }

        private object test()
        {
            for ( int i=0;i<10;i++){
                frm.doReportProgressive("Value is " + i, i);
                Console.WriteLine("Value is " + i);
                Thread.Sleep(1000);
            }
            return null;
        }

    }
}
