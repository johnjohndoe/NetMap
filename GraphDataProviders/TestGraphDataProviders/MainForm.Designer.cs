namespace Microsoft.NodeXL.TestGraphDataProviders
{
    partial class MainForm
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
            this.wbWebBrowser = new System.Windows.Forms.WebBrowser();
            this.btnFlickrRelatedTags = new System.Windows.Forms.Button();
            this.btnTwitterUsers = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // wbWebBrowser
            // 
            this.wbWebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.wbWebBrowser.Location = new System.Drawing.Point(12, 51);
            this.wbWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbWebBrowser.Name = "wbWebBrowser";
            this.wbWebBrowser.Size = new System.Drawing.Size(519, 320);
            this.wbWebBrowser.TabIndex = 0;
            // 
            // btnFlickrRelatedTags
            // 
            this.btnFlickrRelatedTags.Location = new System.Drawing.Point(12, 12);
            this.btnFlickrRelatedTags.Name = "btnFlickrRelatedTags";
            this.btnFlickrRelatedTags.Size = new System.Drawing.Size(120, 23);
            this.btnFlickrRelatedTags.TabIndex = 1;
            this.btnFlickrRelatedTags.Text = "Flickr Related Tags";
            this.btnFlickrRelatedTags.UseVisualStyleBackColor = true;
            this.btnFlickrRelatedTags.Click += new System.EventHandler(this.btnFlickrRelatedTags_Click);
            // 
            // btnTwitterUsers
            // 
            this.btnTwitterUsers.Location = new System.Drawing.Point(138, 12);
            this.btnTwitterUsers.Name = "btnTwitterUsers";
            this.btnTwitterUsers.Size = new System.Drawing.Size(120, 23);
            this.btnTwitterUsers.TabIndex = 2;
            this.btnTwitterUsers.Text = "Twitter Users";
            this.btnTwitterUsers.UseVisualStyleBackColor = true;
            this.btnTwitterUsers.Click += new System.EventHandler(this.btnTwitterUsers_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 394);
            this.Controls.Add(this.btnTwitterUsers);
            this.Controls.Add(this.btnFlickrRelatedTags);
            this.Controls.Add(this.wbWebBrowser);
            this.Name = "MainForm";
            this.Text = "Test Graph Data Provider Plug-Ins";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser wbWebBrowser;
        private System.Windows.Forms.Button btnFlickrRelatedTags;
        private System.Windows.Forms.Button btnTwitterUsers;
    }
}

