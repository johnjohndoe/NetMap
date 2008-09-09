

//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NetMap.ExcelTemplate
{
    partial class AboutDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lnkContact = new System.Windows.Forms.LinkLabel();
            this.btnEnableAllNotifications = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Location = new System.Drawing.Point(495, 228);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(277, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Microsoft .NetMap";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(254, 239);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(277, 60);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(126, 13);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "Version [gets set in code]";
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Location = new System.Drawing.Point(277, 85);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(135, 13);
            this.lblCopyright.TabIndex = 2;
            this.lblCopyright.Text = "Copyright [gets set in code]";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(277, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(293, 54);
            this.label2.TabIndex = 3;
            this.label2.Text = "The Microsoft .NetMap team includes Marc Smith, Tony Capone, Natasa Milic-Fraylin" +
                "g, Eduarda Mendes Rodrigues, Eric Gleave, Adam Perer, and Ben Shneiderman";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(277, 185);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Questions, comments?  Contact";
            // 
            // lnkContact
            // 
            this.lnkContact.AutoSize = true;
            this.lnkContact.Location = new System.Drawing.Point(277, 201);
            this.lnkContact.Name = "lnkContact";
            this.lnkContact.Size = new System.Drawing.Size(90, 13);
            this.lnkContact.TabIndex = 5;
            this.lnkContact.TabStop = true;
            this.lnkContact.Text = "[Gets set in code]";
            this.lnkContact.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkContact_LinkClicked);
            // 
            // btnEnableAllNotifications
            // 
            this.btnEnableAllNotifications.Location = new System.Drawing.Point(280, 228);
            this.btnEnableAllNotifications.Name = "btnEnableAllNotifications";
            this.btnEnableAllNotifications.Size = new System.Drawing.Size(154, 23);
            this.btnEnableAllNotifications.TabIndex = 6;
            this.btnEnableAllNotifications.Text = "&Turn On All Notifications";
            this.btnEnableAllNotifications.UseVisualStyleBackColor = true;
            this.btnEnableAllNotifications.Click += new System.EventHandler(this.btnEnableAllNotifications_Click);
            // 
            // AboutDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(582, 263);
            this.Controls.Add(this.btnEnableAllNotifications);
            this.Controls.Add(this.lnkContact);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.Text = "About Microsoft .NetMap";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel lnkContact;
        private System.Windows.Forms.Button btnEnableAllNotifications;
    }
}
