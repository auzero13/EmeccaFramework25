namespace Emecca.Express.Common
{
    partial class EmeccaSplashScreen
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBarInTask = new System.Windows.Forms.ProgressBar();
            this.messageInTask = new DevExpress.XtraEditors.LabelControl();
            this.cmdCancel = new DevExpress.XtraEditors.SimpleButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.picSplashScreen = new DevExpress.XtraEditors.PictureEdit();
            this.lnfMain = new DevExpress.LookAndFeel.DefaultLookAndFeel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSplashScreen.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // progressBarInTask
            // 
            this.progressBarInTask.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBarInTask.Location = new System.Drawing.Point(0, 0);
            this.progressBarInTask.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.progressBarInTask.Name = "progressBarInTask";
            this.progressBarInTask.Size = new System.Drawing.Size(560, 25);
            this.progressBarInTask.TabIndex = 0;
            // 
            // messageInTask
            // 
            this.messageInTask.Appearance.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageInTask.Location = new System.Drawing.Point(4, 30);
            this.messageInTask.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.messageInTask.Name = "messageInTask";
            this.messageInTask.Size = new System.Drawing.Size(168, 16);
            this.messageInTask.TabIndex = 1;
            this.messageInTask.Text = "Emecca Framework v2011";
            // 
            // cmdCancel
            // 
            this.cmdCancel.Appearance.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancel.Appearance.Options.UseFont = true;
            this.cmdCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.cmdCancel.Location = new System.Drawing.Point(406, 25);
            this.cmdCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(154, 34);
            this.cmdCancel.TabIndex = 3;
            this.cmdCancel.Text = "取消";
            this.cmdCancel.Visible = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Controls.Add(this.messageInTask);
            this.panel1.Controls.Add(this.progressBarInTask);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 330);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(560, 59);
            this.panel1.TabIndex = 4;
            // 
            // picSplashScreen
            // 
            this.picSplashScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picSplashScreen.Location = new System.Drawing.Point(0, 0);
            this.picSplashScreen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picSplashScreen.Name = "picSplashScreen";
            this.picSplashScreen.Size = new System.Drawing.Size(560, 330);
            this.picSplashScreen.TabIndex = 5;
            // 
            // lnfMain
            // 
            this.lnfMain.LookAndFeel.SkinName = "Dark Side";
            // 
            // EmeccaSplashScreen
            // 
            this.Appearance.Font = new System.Drawing.Font("PMingLiU", 14F);
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 389);
            this.Controls.Add(this.picSplashScreen);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "EmeccaSplashScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EmeccaSplashScreen";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSplashScreen.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ProgressBar progressBarInTask;
        private DevExpress.XtraEditors.LabelControl messageInTask;
        private DevExpress.XtraEditors.SimpleButton cmdCancel;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.PictureEdit picSplashScreen;
        private DevExpress.LookAndFeel.DefaultLookAndFeel lnfMain;
    }
}