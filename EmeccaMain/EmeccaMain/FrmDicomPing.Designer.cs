namespace Emecca.Demo
{
    partial class FrmDicomPing
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
            this.LocalAEtitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.HostName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DicomAEtitle = new System.Windows.Forms.TextBox();
            this.DicomPort = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LocalAEtitle
            // 
            this.LocalAEtitle.Location = new System.Drawing.Point(161, 12);
            this.LocalAEtitle.Margin = new System.Windows.Forms.Padding(5);
            this.LocalAEtitle.Name = "LocalAEtitle";
            this.LocalAEtitle.Size = new System.Drawing.Size(161, 33);
            this.LocalAEtitle.TabIndex = 0;
            this.LocalAEtitle.Text = "ClientTest";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "本機AETile:";
            // 
            // HostName
            // 
            this.HostName.Location = new System.Drawing.Point(161, 53);
            this.HostName.Name = "HostName";
            this.HostName.Size = new System.Drawing.Size(161, 33);
            this.HostName.TabIndex = 2;
            this.HostName.Text = "192.168.8.137";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "Host Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 24);
            this.label3.TabIndex = 4;
            this.label3.Text = "DICOM AE Title:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 24);
            this.label4.TabIndex = 5;
            this.label4.Text = "DICOM Port:";
            // 
            // DicomAEtitle
            // 
            this.DicomAEtitle.Location = new System.Drawing.Point(162, 92);
            this.DicomAEtitle.Name = "DicomAEtitle";
            this.DicomAEtitle.Size = new System.Drawing.Size(160, 33);
            this.DicomAEtitle.TabIndex = 6;
            this.DicomAEtitle.Text = "DEMO_QC129";
            // 
            // DicomPort
            // 
            this.DicomPort.Location = new System.Drawing.Point(162, 131);
            this.DicomPort.Name = "DicomPort";
            this.DicomPort.Size = new System.Drawing.Size(160, 33);
            this.DicomPort.TabIndex = 7;
            this.DicomPort.Text = "104";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(249, 168);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 31);
            this.button1.TabIndex = 8;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmDicomPing
            // 
            this.Appearance.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 203);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.DicomPort);
            this.Controls.Add(this.DicomAEtitle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.HostName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LocalAEtitle);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FrmDicomPing";
            this.Text = "FrmDicomPing";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox LocalAEtitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox HostName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox DicomAEtitle;
        private System.Windows.Forms.TextBox DicomPort;
        private System.Windows.Forms.Button button1;
    }
}