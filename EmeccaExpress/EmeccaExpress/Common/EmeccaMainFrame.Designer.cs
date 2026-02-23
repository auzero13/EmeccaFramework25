namespace Emecca.Express.Common
{
    partial class EmeccaMainFrame
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmeccaMainFrame));
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.lnfMain = new DevExpress.LookAndFeel.DefaultLookAndFeel();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barMgr = new DevExpress.XtraBars.BarManager();
            this.barTool = new DevExpress.XtraBars.Bar();
            this.barMenu = new DevExpress.XtraBars.Bar();
            this.barStatus = new DevExpress.XtraBars.Bar();
            this.barDateTime = new DevExpress.XtraBars.BarToolbarsListItem();
            this.barUserInfo = new DevExpress.XtraBars.BarToolbarsListItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.tmdiLayoutMain = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager();
            this.barDB = new DevExpress.XtraBars.BarToolbarsListItem();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.timerMain = new System.Windows.Forms.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barMgr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tmdiLayoutMain)).BeginInit();
            this.SuspendLayout();
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // lnfMain
            // 
            this.lnfMain.LookAndFeel.SkinName = "MySkin_DevExpress Style1";
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // barMgr
            // 
            this.barMgr.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.barTool,
            this.barMenu,
            this.barStatus});
            this.barMgr.DockControls.Add(this.barDockControlTop);
            this.barMgr.DockControls.Add(this.barDockControlBottom);
            this.barMgr.DockControls.Add(this.barDockControlLeft);
            this.barMgr.DockControls.Add(this.barDockControlRight);
            this.barMgr.Form = this;
            this.barMgr.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barDateTime,
            this.barUserInfo});
            this.barMgr.MainMenu = this.barMenu;
            this.barMgr.MaxItemId = 3;
            this.barMgr.StatusBar = this.barStatus;
            // 
            // barTool
            // 
            this.barTool.BarName = "barTool";
            this.barTool.DockCol = 0;
            this.barTool.DockRow = 1;
            this.barTool.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barTool.Text = "Tools";
            // 
            // barMenu
            // 
            this.barMenu.BarName = "barMenu";
            this.barMenu.DockCol = 0;
            this.barMenu.DockRow = 0;
            this.barMenu.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barMenu.OptionsBar.MultiLine = true;
            this.barMenu.OptionsBar.UseWholeRow = true;
            this.barMenu.Text = "Main menu";
            // 
            // barStatus
            // 
            this.barStatus.BarName = "barStatus";
            this.barStatus.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.barStatus.DockCol = 0;
            this.barStatus.DockRow = 0;
            this.barStatus.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.barStatus.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barDateTime),
            new DevExpress.XtraBars.LinkPersistInfo(this.barUserInfo)});
            this.barStatus.OptionsBar.AllowQuickCustomization = false;
            this.barStatus.OptionsBar.DisableClose = true;
            this.barStatus.OptionsBar.DisableCustomization = true;
            this.barStatus.OptionsBar.DrawDragBorder = false;
            this.barStatus.OptionsBar.UseWholeRow = true;
            this.barStatus.Text = "Status bar";
            // 
            // barDateTime
            // 
            this.barDateTime.Appearance.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Bold);
            this.barDateTime.Appearance.ForeColor = System.Drawing.Color.Green;
            this.barDateTime.Appearance.Options.UseFont = true;
            this.barDateTime.Appearance.Options.UseForeColor = true;
            this.barDateTime.Caption = "barDateTime";
            this.barDateTime.Id = 1;
            this.barDateTime.Name = "barDateTime";
            this.barDateTime.ShowCustomizationItem = false;
            this.barDateTime.ShowToolbars = false;
            // 
            // barUserInfo
            // 
            this.barUserInfo.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barUserInfo.Appearance.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Bold);
            this.barUserInfo.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.barUserInfo.Appearance.Options.UseFont = true;
            this.barUserInfo.Appearance.Options.UseForeColor = true;
            this.barUserInfo.Caption = "用戶資訊";
            this.barUserInfo.Id = 2;
            this.barUserInfo.Name = "barUserInfo";
            this.barUserInfo.ShowCustomizationItem = false;
            this.barUserInfo.ShowToolbars = false;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.barDockControlTop.Size = new System.Drawing.Size(984, 51);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 676);
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.barDockControlBottom.Size = new System.Drawing.Size(984, 26);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 51);
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 625);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(984, 51);
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 625);
            // 
            // tmdiLayoutMain
            // 
            this.tmdiLayoutMain.Appearance.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tmdiLayoutMain.Appearance.Options.UseFont = true;
            this.tmdiLayoutMain.AppearancePage.Header.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tmdiLayoutMain.AppearancePage.Header.Options.UseFont = true;
            this.tmdiLayoutMain.AppearancePage.HeaderActive.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tmdiLayoutMain.AppearancePage.HeaderActive.Options.UseFont = true;
            this.tmdiLayoutMain.AppearancePage.HeaderHotTracked.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tmdiLayoutMain.AppearancePage.HeaderHotTracked.Options.UseFont = true;
            this.tmdiLayoutMain.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Bottom;
            this.tmdiLayoutMain.MdiParent = this;
            // 
            // barDB
            // 
            this.barDB.Appearance.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Bold);
            this.barDB.Appearance.ForeColor = System.Drawing.Color.Green;
            this.barDB.Appearance.Options.UseFont = true;
            this.barDB.Appearance.Options.UseForeColor = true;
            this.barDB.Caption = "正在連接資料庫...";
            this.barDB.Id = 2;
            this.barDB.Name = "barDB";
            // 
            // bar1
            // 
            this.bar1.BarName = "barStatus";
            this.bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barDB)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Status bar";
            // 
            // timerMain
            // 
            this.timerMain.Enabled = true;
            this.timerMain.Interval = 1000;
            this.timerMain.Tick += new System.EventHandler(this.timerMain_Tick);
            // 
            // EmeccaMainFrame
            // 
            this.Appearance.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 702);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "EmeccaMainFrame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EmeccaMainFrame";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.EmeccaMainFrame_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EmeccaMainFrame_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EmeccaMainFrame_FormClosed);
            this.Load += new System.EventHandler(this.EmeccaMainFrame_Load);
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barMgr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tmdiLayoutMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraBars.BarManager barMgr;
        private DevExpress.XtraBars.Bar barTool;
        private DevExpress.XtraBars.Bar barMenu;
        private DevExpress.XtraBars.Bar barStatus;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager tmdiLayoutMain;
        private DevExpress.XtraBars.BarToolbarsListItem barDateTime;
        private DevExpress.XtraBars.BarToolbarsListItem barUserInfo;
        private DevExpress.XtraBars.BarToolbarsListItem barDB;
        private DevExpress.XtraBars.Bar bar1;
        public System.Windows.Forms.Timer timerMain;
        public DevExpress.LookAndFeel.DefaultLookAndFeel lnfMain;
    }
}