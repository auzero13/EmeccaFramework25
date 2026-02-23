namespace Emecca.Express.Common
{
    partial class EmeccaMessageBox
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cmdCancel = new DevExpress.XtraEditors.SimpleButton();
            this.cmdConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.txtMessage = new DevExpress.XtraEditors.MemoEdit();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMessage.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.cmdCancel);
            this.flowLayoutPanel1.Controls.Add(this.cmdConfirm);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 147);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(422, 36);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Appearance.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.cmdCancel.Appearance.Options.UseFont = true;
            this.cmdCancel.Location = new System.Drawing.Point(297, 3);
            this.cmdCancel.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(120, 30);
            this.cmdCancel.TabIndex = 35;
            this.cmdCancel.Text = "取消";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdConfirm
            // 
            this.cmdConfirm.Appearance.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.cmdConfirm.Appearance.Options.UseFont = true;
            this.cmdConfirm.Location = new System.Drawing.Point(167, 3);
            this.cmdConfirm.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.cmdConfirm.Name = "cmdConfirm";
            this.cmdConfirm.Size = new System.Drawing.Size(120, 30);
            this.cmdConfirm.TabIndex = 34;
            this.cmdConfirm.Text = "確認";
            this.cmdConfirm.Click += new System.EventHandler(this.cmdConfirm_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessage.Location = new System.Drawing.Point(0, 0);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtMessage.Properties.Appearance.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Properties.Appearance.Options.UseBackColor = true;
            this.txtMessage.Properties.Appearance.Options.UseFont = true;
            this.txtMessage.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.Properties.AppearanceReadOnly.BackColor2 = System.Drawing.Color.Transparent;
            this.txtMessage.Properties.AppearanceReadOnly.BorderColor = System.Drawing.Color.White;
            this.txtMessage.Properties.AppearanceReadOnly.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.txtMessage.Properties.AppearanceReadOnly.Options.UseBorderColor = true;
            this.txtMessage.Properties.AppearanceReadOnly.Options.UseFont = true;
            this.txtMessage.Properties.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(422, 147);
            this.txtMessage.TabIndex = 2;
            // 
            // EmeccaMessageBox
            // 
            this.Appearance.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 183);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "EmeccaMessageBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EmeccaMessageBox";
            this.TopMost = true;
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtMessage.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private DevExpress.XtraEditors.SimpleButton cmdCancel;
        private DevExpress.XtraEditors.SimpleButton cmdConfirm;
        private DevExpress.XtraEditors.MemoEdit txtMessage;

    }
}