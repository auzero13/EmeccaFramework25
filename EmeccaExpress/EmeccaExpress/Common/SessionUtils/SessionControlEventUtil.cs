using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Emecca.Express.Common.SessionUtils
{
   public class SessionControlEventUtil
    {

        public void setTimeOut(Control formControl)
        {
            foreach (Control controll in formControl.Controls)
            {
                if (controll.Controls.Count > 0)
                {
                    setTimeOut(controll);
                }

                controll.Leave += new EventHandler(controll_Leave);
                controll.MouseUp += new MouseEventHandler(controll_MouseUp);
                controll.KeyUp += new KeyEventHandler(controll_KeyUp);
                controll.MouseMove += new MouseEventHandler(controll_MouseMove);
            }
        }

        void controll_MouseMove(object sender, MouseEventArgs e)
        {
            UserSession.ResetTimer();
        }

        void controll_KeyUp(object sender, KeyEventArgs e)
        {
            UserSession.ResetTimer();
        }

        void controll_MouseUp(object sender, MouseEventArgs e)
        {
            UserSession.ResetTimer();
        }

        void controll_Leave(object sender, EventArgs e)
        {
            UserSession.ResetTimer();
        }
    }
}
