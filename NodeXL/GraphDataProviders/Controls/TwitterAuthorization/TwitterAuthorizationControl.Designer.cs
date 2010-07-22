namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
    partial class TwitterAuthorizationControl
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
            this.radHasTwitterAccountAuthorized = new System.Windows.Forms.RadioButton();
            this.radHasTwitterAccountNotAuthorized = new System.Windows.Forms.RadioButton();
            this.radNoTwitterAccount = new System.Windows.Forms.RadioButton();
            this.lnkRateLimiting = new System.Windows.Forms.LinkLabel();
            this.lnkRequestWhitelist = new Microsoft.Research.CommunityTechnologies.AppLib.FileNameLinkLabel();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radHasTwitterAccountAuthorized);
            this.groupBox2.Controls.Add(this.radHasTwitterAccountNotAuthorized);
            this.groupBox2.Controls.Add(this.radNoTwitterAccount);
            this.groupBox2.Location = new System.Drawing.Point(0, 46);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(304, 183);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Your Twitter account";
            // 
            // radHasTwitterAccountAuthorized
            // 
            this.radHasTwitterAccountAuthorized.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radHasTwitterAccountAuthorized.Location = new System.Drawing.Point(11, 131);
            this.radHasTwitterAccountAuthorized.Name = "radHasTwitterAccountAuthorized";
            this.radHasTwitterAccountAuthorized.Size = new System.Drawing.Size(287, 46);
            this.radHasTwitterAccountAuthorized.TabIndex = 2;
            this.radHasTwitterAccountAuthorized.Text = "I have a T&witter account, and I have authorized NodeXL to use my account to impo" +
                "rt Twitter networks.";
            this.radHasTwitterAccountAuthorized.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radHasTwitterAccountAuthorized.UseVisualStyleBackColor = true;
            // 
            // radHasTwitterAccountNotAuthorized
            // 
            this.radHasTwitterAccountNotAuthorized.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radHasTwitterAccountNotAuthorized.Location = new System.Drawing.Point(11, 71);
            this.radHasTwitterAccountNotAuthorized.Name = "radHasTwitterAccountNotAuthorized";
            this.radHasTwitterAccountNotAuthorized.Size = new System.Drawing.Size(287, 68);
            this.radHasTwitterAccountNotAuthorized.TabIndex = 1;
            this.radHasTwitterAccountNotAuthorized.Text = "I ha&ve a Twitter account, but I have not yet authorized NodeXL to use my account" +
                " to import Twitter networks.  Take me to Twitter\'s authorization Web page.";
            this.radHasTwitterAccountNotAuthorized.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radHasTwitterAccountNotAuthorized.UseVisualStyleBackColor = true;
            // 
            // radNoTwitterAccount
            // 
            this.radNoTwitterAccount.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radNoTwitterAccount.Checked = true;
            this.radNoTwitterAccount.Location = new System.Drawing.Point(11, 26);
            this.radNoTwitterAccount.Name = "radNoTwitterAccount";
            this.radNoTwitterAccount.Size = new System.Drawing.Size(287, 53);
            this.radNoTwitterAccount.TabIndex = 0;
            this.radNoTwitterAccount.TabStop = true;
            this.radNoTwitterAccount.Text = "&I don\'t have a Twitter account.  I will be able to import only a limited number " +
                "of Twitter networks into NodeXL.";
            this.radNoTwitterAccount.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radNoTwitterAccount.UseVisualStyleBackColor = true;
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
            // TwitterAuthorizationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lnkRequestWhitelist);
            this.Controls.Add(this.lnkRateLimiting);
            this.Name = "TwitterAuthorizationControl";
            this.Size = new System.Drawing.Size(307, 229);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private Microsoft.Research.CommunityTechnologies.AppLib.FileNameLinkLabel lnkRequestWhitelist;
        private System.Windows.Forms.LinkLabel lnkRateLimiting;
        private System.Windows.Forms.RadioButton radNoTwitterAccount;
        private System.Windows.Forms.RadioButton radHasTwitterAccountNotAuthorized;
        private System.Windows.Forms.RadioButton radHasTwitterAccountAuthorized;

    }
}
