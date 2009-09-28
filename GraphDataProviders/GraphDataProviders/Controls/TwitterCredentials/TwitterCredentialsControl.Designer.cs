namespace Microsoft.NodeXL.GraphDataProviders
{
    partial class TwitterCredentialsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txbPassword = new System.Windows.Forms.TextBox();
            this.txbScreenName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.lnkRequestWhitelist = new Microsoft.Research.CommunityTechnologies.AppLib.FileNameLinkLabel();
            this.lnkRateLimiting = new System.Windows.Forms.LinkLabel();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txbPassword);
            this.groupBox2.Controls.Add(this.txbScreenName);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.lblPrompt);
            this.groupBox2.Location = new System.Drawing.Point(0, 46);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(304, 176);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Your Twitter account";
            // 
            // txbPassword
            // 
            this.txbPassword.Location = new System.Drawing.Point(11, 138);
            this.txbPassword.MaxLength = 100;
            this.txbPassword.Name = "txbPassword";
            this.txbPassword.PasswordChar = '*';
            this.txbPassword.Size = new System.Drawing.Size(282, 20);
            this.txbPassword.TabIndex = 4;
            // 
            // txbScreenName
            // 
            this.txbScreenName.Location = new System.Drawing.Point(11, 92);
            this.txbScreenName.MaxLength = 100;
            this.txbScreenName.Name = "txbScreenName";
            this.txbScreenName.Size = new System.Drawing.Size(282, 20);
            this.txbScreenName.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Your p&assword:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Your screen &name:";
            // 
            // lblPrompt
            // 
            this.lblPrompt.Location = new System.Drawing.Point(11, 21);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(282, 47);
            this.lblPrompt.TabIndex = 0;
            this.lblPrompt.Text = "[Gets set in code]";
            // 
            // lnkRequestWhitelist
            // 
            this.lnkRequestWhitelist.AutoSize = true;
            this.lnkRequestWhitelist.FileName = null;
            this.lnkRequestWhitelist.Location = new System.Drawing.Point(0, 22);
            this.lnkRequestWhitelist.Name = "lnkRequestWhitelist";
            this.lnkRequestWhitelist.Size = new System.Drawing.Size(176, 13);
            this.lnkRequestWhitelist.TabIndex = 1;
            this.lnkRequestWhitelist.TabStop = true;
            this.lnkRequestWhitelist.Text = "Request lifting of Twitter rate limiting";
            // 
            // lnkRateLimiting
            // 
            this.lnkRateLimiting.AutoSize = true;
            this.lnkRateLimiting.Location = new System.Drawing.Point(0, 0);
            this.lnkRateLimiting.Name = "lnkRateLimiting";
            this.lnkRateLimiting.Size = new System.Drawing.Size(90, 13);
            this.lnkRateLimiting.TabIndex = 0;
            this.lnkRateLimiting.TabStop = true;
            this.lnkRateLimiting.Text = "[Gets set in code]";
            this.lnkRateLimiting.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRateLimiting_LinkClicked);
            // 
            // TwitterCredentialsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lnkRequestWhitelist);
            this.Controls.Add(this.lnkRateLimiting);
            this.Name = "TwitterCredentialsControl";
            this.Size = new System.Drawing.Size(307, 226);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txbPassword;
        private System.Windows.Forms.TextBox txbScreenName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblPrompt;
        private Microsoft.Research.CommunityTechnologies.AppLib.FileNameLinkLabel lnkRequestWhitelist;
        private System.Windows.Forms.LinkLabel lnkRateLimiting;

    }
}
