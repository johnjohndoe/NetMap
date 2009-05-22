

//  Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class NotificationDialog
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
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.chkDisableFutureNotifications = new System.Windows.Forms.CheckBox();
            this.picNotification = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picNotification)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(60, 9);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(269, 123);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "[Set in code]";
            // 
            // btnYes
            // 
            this.btnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnYes.Location = new System.Drawing.Point(173, 152);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(75, 23);
            this.btnYes.TabIndex = 2;
            this.btnYes.Text = "&Yes";
            this.btnYes.UseVisualStyleBackColor = true;
            // 
            // btnNo
            // 
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnNo.Location = new System.Drawing.Point(254, 152);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(75, 23);
            this.btnNo.TabIndex = 3;
            this.btnNo.Text = "&No";
            this.btnNo.UseVisualStyleBackColor = true;
            // 
            // chkDisableFutureNotifications
            // 
            this.chkDisableFutureNotifications.AutoSize = true;
            this.chkDisableFutureNotifications.Location = new System.Drawing.Point(12, 152);
            this.chkDisableFutureNotifications.Name = "chkDisableFutureNotifications";
            this.chkDisableFutureNotifications.Size = new System.Drawing.Size(125, 17);
            this.chkDisableFutureNotifications.TabIndex = 1;
            this.chkDisableFutureNotifications.Text = "&Don\'t notify me again";
            this.chkDisableFutureNotifications.UseVisualStyleBackColor = true;
            // 
            // picNotification
            // 
            this.picNotification.Location = new System.Drawing.Point(12, 9);
            this.picNotification.Name = "picNotification";
            this.picNotification.Size = new System.Drawing.Size(42, 47);
            this.picNotification.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picNotification.TabIndex = 4;
            this.picNotification.TabStop = false;
            // 
            // NotificationDialog
            // 
            this.AcceptButton = this.btnNo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnNo;
            this.ClientSize = new System.Drawing.Size(341, 189);
            this.Controls.Add(this.picNotification);
            this.Controls.Add(this.chkDisableFutureNotifications);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NotificationDialog";
            ((System.ComponentModel.ISupportInitialize)(this.picNotification)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.CheckBox chkDisableFutureNotifications;
        private System.Windows.Forms.PictureBox picNotification;
    }
}
