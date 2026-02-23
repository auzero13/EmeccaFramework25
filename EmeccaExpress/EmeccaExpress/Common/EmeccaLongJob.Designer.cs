namespace Emecca.Express.Common
{
    partial class EmeccaLongJob
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
            this.components = new System.ComponentModel.Container();
            this.picLoading = new System.Windows.Forms.PictureBox();
            this.cmdCancel = new DevExpress.XtraEditors.SimpleButton();
            this.lblTimer = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tmDuration = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // picLoading
            // 
            this.picLoading.Location = new System.Drawing.Point(12, 12);
            this.picLoading.Name = "picLoading";
            this.picLoading.Size = new System.Drawing.Size(33, 33);
            this.picLoading.TabIndex = 0;
            this.picLoading.TabStop = false;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Appearance.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.cmdCancel.Appearance.Options.UseFont = true;
            this.cmdCancel.Location = new System.Drawing.Point(169, 74);
            this.cmdCancel.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(120, 26);
            this.cmdCancel.TabIndex = 37;
            this.cmdCancel.Text = "取消";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimer.Location = new System.Drawing.Point(9, 74);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(44, 18);
            this.lblTimer.TabIndex = 38;
            this.lblTimer.Text = "0000";
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(56, 21);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(72, 16);
            this.lblMessage.TabIndex = 39;
            this.lblMessage.Text = "提示訊息";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // tmDuration
            // 
            this.tmDuration.Interval = 1000;
            this.tmDuration.Tick += new System.EventHandler(this.tmDuration_Tick);
            // 
            // EmeccaLongJob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 103);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblTimer);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.picLoading);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "EmeccaLongJob";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EmeccaLongJob";
            ((System.ComponentModel.ISupportInitialize)(this.picLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picLoading;
        private DevExpress.XtraEditors.SimpleButton cmdCancel;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.Label lblMessage;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer tmDuration;

    }
}