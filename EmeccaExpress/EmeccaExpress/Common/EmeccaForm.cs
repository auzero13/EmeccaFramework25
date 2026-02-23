using System;
using System.Collections;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using DevExpress.Utils;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Emecca.Framework.Common;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Repository;
using System.Drawing;
using DevExpress.XtraGrid.Columns;
using Emecca.Express.Common.SessionUtils;

namespace Emecca.Express.Common
{
    public partial class EmeccaForm : DevExpress.XtraEditors.XtraForm, EmeccaObject
    {
        public static string SessionTimeoutMiuntes = null;
        public static string EnableChangeFont = string.Empty;
        public static int GridViewFontSize = 12;

        public const int DIALOG = 0;
        public const int MDI = 1;
        public const int MDI_ONCE = 2;
        public const int WINDOW = 3;
        public const int WINDOW_ONCE = 4;

        //public const string PASS = "PASS";

        protected int showMode = 0;
        public string objectId = null;
        public string objectName = null;
        public EmeccaForm()
        {
            InitializeComponent();
            getFormLocation();
        }
       
        private void getFormLocation()
        {
            EmeccaProfile profile = EmeccaRuntime.getInstance().getVariable((EmeccaRuntime.FrameworkReservedVariableLeading + ".EmeccaProfile")) as EmeccaProfile;

            if (profile != null)
            {
                //多銀幕時，設置DWS顯示的銀幕位置
                Screen[] screen = Screen.AllScreens;

                if (screen.Length >= 2)
                {
                    int index = profile.DisplayScreen;
                    if (index >= screen.Length)
                    {//如果設定的螢幕超過螢幕數，自動分配給最後一個螢幕
                        index = screen.Length-1;
                    }

                    this.Location = new Point(screen[index].Bounds.Left, screen[index].Bounds.Top);  
                }
            }
        }

        public virtual string getEmeccaObjectName()
        {
            if (objectName == null)
            {
                objectName = this.GetType().Name;
            }
            return objectName;
        }
        public virtual string setEmeccaObjectName(string objectName)
        {
            this.objectName = objectName;
            return this.objectName;
        }

        public virtual string getEmeccaObjectId()
        {
            return objectId;
        }
        public virtual string setEmeccaObjectId(string objectId)
        {
            this.objectId = objectId;
            return this.objectId;
        }

        //public int showForm()
        //{
        //    EmeccaMainFrame.getInstance().showEmeccaForm(this);
        //    return getShowMode();
        //}

        public virtual int getShowMode()
        {
            return this.showMode;
        }

        public virtual void setShowMode(int mode)
        {
            this.showMode = mode;
        }
        //public virtual string doInitial()
        //{
        //    return this.Name;
        //}
        //public virtual string doUnInitial()
        //{
        //    return this.Name;
        //}

        //new public void Close()
        //{
        //    base.Close();
        //}

        //new public void Show()
        //{
        //    base.Show();
        //}

        //new public void ShowDialog()
        //{
        //    base.ShowDialog();
        //}

        //public void doClose()
        //{
        //    doUnInitial();
        //    base.Close();
        //}

        //public void doShow()//params object[] args)
        //{
        //    doInitial();
        //    base.Show();
        //}

        //public void doShowDialog()//params object[] args)
        //{
        //    doInitial();
        //    base.ShowDialog();
        //}

        public static void clearXPCollection(XPCollection xpc)
        {
            xpc.SelectDeleted = true;
            xpc.DeleteObjectOnRemove = false;

            xpc.BindingBehavior = DevExpress.Xpo.CollectionBindingBehavior.AllowNone;
            xpc.Criteria = CriteriaOperator.Parse("1=2");

            xpc.Reload();

        }

        public void cloneXPListObject(XPLiteObject target, XPLiteObject original)
        {
            foreach (DBColumn info in original.ClassInfo.Table.Columns)
            {
                target.SetMemberValue(info.Name.ToString(), original.GetMemberValue(info.Name.ToString()));
            }
        }

        public ArrayList calcGridViewSelections(DevExpress.XtraGrid.Views.Grid.GridView view)
        {
            ArrayList selections = new ArrayList();

            if (view != null)
            {
                for (int i = 0; i < view.SelectedRowsCount; i++)
                {
                    if (view.GetSelectedRows()[i] >= 0)
                        selections.Add(view.GetRow(view.GetSelectedRows()[i]));

                }
            }
            return selections;
        }

        //public ArrayList getCodes(string name)
        //{
        //    ArrayList codes = EmeccaRuntime.getInstance().getVariable(name) as ArrayList;
        //    if (codes == null)
        //        codes = new ArrayList();
        //    return codes;
        //}

        //public void addCodes(string name, ArrayList codes)
        //{
        //    EmeccaRuntime.getInstance().setVariable(name, codes);
        //}


        //private void EmeccaForm_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    doUnInitial();
        //}

        //private void EmeccaForm_Load(object sender, EventArgs e)
        //{
        //    doInitial();
        //}

        protected void restoreGridLayout(GridView grid)
        {
            if (grid == null)
                return;
            try
            {
                grid.RestoreLayoutFromXml(this.GetType().Name + "." + grid.Name, OptionsLayoutBase.FullLayout);

                grid.MouseDown += new MouseEventHandler(grid_MouseDown);
                grid.KeyDown += new KeyEventHandler(grid_KeyDown);
                grid.OptionsMenu.EnableColumnMenu = true;
            }
            catch (Exception ee)
            {
                EmeccaLogger.getEmeccaLogger().Error(ee);
            }
        }

        //void grid_KeyUp(object sender, KeyEventArgs e)
        //{
        //    DevExpress.XtraGrid.Views.Grid.GridView gv = sender as DevExpress.XtraGrid.Views.Grid.GridView;
        //    if (!e.Control)
        //    {
        //        gv.OptionsMenu.EnableColumnMenu = false;
        //    }
        //}

        void grid_KeyDown(object sender, KeyEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView gv = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if (e.Control && e.Alt)
            {
                gv.OptionsMenu.EnableColumnMenu = !(gv.OptionsMenu.EnableColumnMenu);
            }
        }


        private void grid_MouseDown(object sender, MouseEventArgs e)
        {

            DevExpress.XtraGrid.Views.Grid.GridView gv = sender as DevExpress.XtraGrid.Views.Grid.GridView;

            //Point p = gv.GridControl.PointToClient();
            GridHitInfo info = gv.CalcHitInfo(e.Location);
            if (info.HitTest == GridHitTest.Column && e.Button == MouseButtons.Right && !gv.OptionsMenu.EnableColumnMenu)
            {
                EmeccaInputBox frm = new EmeccaInputBox();
                string caption = frm.doExecute(info.Column.Caption);
                if (caption != null)
                {
                    info.Column.Caption = caption;
                }
            }
        }

        protected void saveGridLayout(GridView grid)
        {
            grid.SaveLayoutToXml(this.GetType().Name + "." + grid.Name, OptionsLayoutBase.FullLayout);
        }

        public void setFont(Control formControl, float gridViewFontSize)
        {
            float font = 14.0f;
            string fontName = "Microsoft JhengHei UI"; // Default modern font

            try
            {
                font = Convert.ToInt32(Emecca.Framework.Common.EmeccaRuntime.getInstance().getVariable("FontSize"));
                
                // Attempt to get font name from runtime variable, fallback to default if missing
                string configFontName = Emecca.Framework.Common.EmeccaRuntime.getInstance().getVariable("DefaultSystemFontType") as string;
                if (!string.IsNullOrEmpty(configFontName))
                {
                    fontName = configFontName;
                }
            }
            catch
            {
                return;
            }

            foreach (Control controll in formControl.Controls)
            {
                if (controll.Controls.Count > 0)
                {
                    setFont(controll, gridViewFontSize);
                }

                // Use the configured font name instead of hardcoded PMingLiU
                try 
                {
                    controll.Font = new Font(fontName, font);
                }
                catch
                {
                    // Fallback to generic sans-serif if font not found
                    controll.Font = new Font(FontFamily.GenericSansSerif, font);
                }

                if (gridViewFontSize <= 0)
                {
                    return;
                }
                if (controll is GridControl)
                {

                    ViewRepositoryCollection views = (controll as GridControl).ViewCollection;
                    foreach (GridView item in views)
                    {
                        item.Appearance.HideSelectionRow.Font = new Font(fontName, gridViewFontSize);
                        item.Appearance.Row.Font = new Font(fontName, gridViewFontSize);
                        
                        // Header font larger than content for hierarchy (+2pt, Bold)
                        item.Appearance.HeaderPanel.Font = new Font(fontName, gridViewFontSize + 2, FontStyle.Bold);
                        
                        ////不需要放大
                        //item.Appearance.FocusedRow.Font = new Font(fontName, gridViewFontSize + 2);
                        //item.Appearance.FocusedCell.Font = new Font(fontName, gridViewFontSize + 2);
                        item.Appearance.FocusedRow.Font = new Font(fontName, gridViewFontSize );
                        item.Appearance.FocusedCell.Font = new Font(fontName, gridViewFontSize);
                        foreach (GridColumn column in item.Columns)
                        {
                            // Sync column header appearance with the view header setting
                            column.AppearanceHeader.Font = new Font(fontName, gridViewFontSize + 2, FontStyle.Bold);
                        }
                    }
                }
            }
        }

        protected void EmeccaForm_Load(object sender, EventArgs e)
        {
            //20130109 marked by chris 看一下是不是SessionTimeOut引發當機的
            //if (string.IsNullOrEmpty(SessionTimeoutMiuntes))
            //{
            //    string sessionTimeoutMinutes = EmeccaRuntime.getInstance().getVariable("SessionTimeOut") as string;
            //    if (!string.IsNullOrEmpty(sessionTimeoutMinutes))
            //    {
            //        try
            //        {
            //            int minutes = Convert.ToInt32(sessionTimeoutMinutes);
            //            SessionUtils.UserSession.SessionTimeoutMinutes = minutes;
            //            SessionTimeoutMiuntes = sessionTimeoutMinutes;
            //            SessionControlEventUtil sessionEvent = new SessionControlEventUtil();
            //            sessionEvent.setTimeOut(this);
            //        }
            //        catch (Exception ex)
            //        {
            //            EmeccaLogger.getEmeccaLogger().Error("SystemXML中SessionTimeout變數設定錯誤，" + ex);
            //        }
            //    }
            //}
            EmeccaFormSetFont();

        }

        protected void EmeccaFormSetFont()
        {
            string enableChangeFont = EmeccaRuntime.getInstance().getVariable("EnableChangeFont") as string;
            if (!string.IsNullOrEmpty(enableChangeFont))
            {
                if (string.Equals(enableChangeFont, "true", StringComparison.CurrentCultureIgnoreCase))
                {
                    float gridViewFontSize = 0;
                    try
                    {
                        gridViewFontSize = Convert.ToInt32(EmeccaRuntime.getInstance().getVariable("GridViewFontSize"));
                    }
                    catch (Exception ee)
                    {
                        EmeccaLogger.getEmeccaLogger().Error("SystemXML 中變數(GridViewFontSize)設定錯誤,或不存在，" + ee);
                        gridViewFontSize = 0;
                    }

                    setFont(this, gridViewFontSize);
                }
            }
        }

        private void EmeccaForm_Deactivate(object sender, EventArgs e)
        {
           // UserSession.ResetTimer();
        }

        private void EmeccaForm_Activated(object sender, EventArgs e)
        {
            string name = this.Name;
            //20130109 marked by chris 看一下是不是SessionTimeOut引發當機的
            //SessionControlEventUtil sessionEvent = new SessionControlEventUtil();
            //sessionEvent.setTimeOut(this);
        }

       
    }
}