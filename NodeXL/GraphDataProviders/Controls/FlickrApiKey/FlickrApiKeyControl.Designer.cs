namespace Microsoft.NodeXL.GraphDataProviders.Flickr
{
    partial class FlickrApiKeyControl
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
            this.lnkRequestFlickrApiKey = new Microsoft.Research.CommunityTechnologies.AppLib.FileNameLinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.txbApiKey = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lnkRequestFlickrApiKey
            // 
            this.lnkRequestFlickrApiKey.AutoSize = true;
            this.lnkRequestFlickrApiKey.FileName = null;
            this.lnkRequestFlickrApiKey.Location = new System.Drawing.Point(0, 50);
            this.lnkRequestFlickrApiKey.Name = "lnkRequestFlickrApiKey";
            this.lnkRequestFlickrApiKey.Size = new System.Drawing.Size(125, 13);
            this.lnkRequestFlickrApiKey.TabIndex = 2;
            this.lnkRequestFlickrApiKey.TabStop = true;
            this.lnkRequestFlickrApiKey.Text = "Apply for a Flickr API key";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "&Flickr API key:";
            // 
            // txbApiKey
            // 
            this.txbApiKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbApiKey.Location = new System.Drawing.Point(0, 20);
            this.txbApiKey.MaxLength = 200;
            this.txbApiKey.Name = "txbApiKey";
            this.txbApiKey.Size = new System.Drawing.Size(273, 20);
            this.txbApiKey.TabIndex = 1;
            // 
            // FlickrApiKeyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lnkRequestFlickrApiKey);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txbApiKey);
            this.Name = "FlickrApiKeyControl";
            this.Size = new System.Drawing.Size(273, 73);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Research.CommunityTechnologies.AppLib.FileNameLinkLabel lnkRequestFlickrApiKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbApiKey;

    }
}
