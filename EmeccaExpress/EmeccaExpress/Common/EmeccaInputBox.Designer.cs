namespace Emecca.Express.Common
{
    partial class EmeccaInputBox
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
            this.txtValue = new DevExpress.XtraEditors.TextEdit();
            this.cmdCancel = new DevExpress.XtraEditors.SimpleButton();
            this.cmdConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.txtOriginal = new DevExpress.XtraEditors.TextEdit();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.txtValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOriginal.Properties)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(3, 35);
            this.txtValue.Name = "txtValue";
            this.txtValue.Properties.Appearance.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValue.Properties.Appearance.Options.UseFont = true;
            this.txtValue.Size = new System.Drawing.Size(202, 26);
            this.txtValue.TabIndex = 0;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Appearance.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.cmdCancel.Appearance.Options.UseFont = true;
            this.cmdCancel.Location = new System.Drawing.Point(5, 35);
            this.cmdCancel.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(120, 26);
            this.cmdCancel.TabIndex = 37;
            this.cmdCancel.Text = "取消";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdConfirm
            // 
            this.cmdConfirm.Appearance.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.cmdConfirm.Appearance.Options.UseFont = true;
            this.cmdConfirm.Location = new System.Drawing.Point(5, 3);
            this.cmdConfirm.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.cmdConfirm.Name = "cmdConfirm";
            this.cmdConfirm.Size = new System.Drawing.Size(120, 26);
            this.cmdConfirm.TabIndex = 36;
            this.cmdConfirm.Text = "確認";
            this.cmdConfirm.Click += new System.EventHandler(this.cmdConfirm_Click);
            // 
            // txtOriginal
            // 
            this.txtOriginal.Enabled = false;
            this.txtOriginal.Location = new System.Drawing.Point(3, 3);
            this.txtOriginal.Name = "txtOriginal";
            this.txtOriginal.Properties.Appearance.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOriginal.Properties.Appearance.Options.UseFont = true;
            this.txtOriginal.Size = new System.Drawing.Size(202, 26);
            this.txtOriginal.TabIndex = 38;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.cmdConfirm);
            this.flowLayoutPanel1.Controls.Add(this.cmdCancel);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(210, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(129, 69);
            this.flowLayoutPanel1.TabIndex = 39;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.txtOriginal);
            this.flowLayoutPanel2.Controls.Add(this.txtValue);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(210, 69);
            this.flowLayoutPanel2.TabIndex = 40;
            // 
            // EmeccaInputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 69);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "EmeccaInputBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "輸入視窗";
            ((System.ComponentModel.ISupportInitialize)(this.txtValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOriginal.Properties)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtValue;
        private DevExpress.XtraEditors.SimpleButton cmdCancel;
        private DevExpress.XtraEditors.SimpleButton cmdConfirm;
        private DevExpress.XtraEditors.TextEdit txtOriginal;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
    }
}