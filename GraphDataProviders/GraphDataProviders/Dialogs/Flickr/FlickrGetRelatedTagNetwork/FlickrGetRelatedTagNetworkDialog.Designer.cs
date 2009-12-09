

//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.GraphDataProviders.Flickr
{
    partial class FlickrGetRelatedTagNetworkDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.txbTag = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlUserInputs = new System.Windows.Forms.Panel();
            this.chkIncludeSampleThumbnails = new System.Windows.Forms.CheckBox();
            this.usrNetworkLevel = new Microsoft.NodeXL.GraphDataProviders.NetworkLevelControl();
            this.usrFlickrApiKey = new Microsoft.NodeXL.GraphDataProviders.Flickr.FlickrApiKeyControl();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlUserInputs.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Get the network of Flickr tags related to this tag:\r\n";
            // 
            // txbTag
            // 
            this.txbTag.Location = new System.Drawing.Point(0, 19);
            this.txbTag.MaxLength = 100;
            this.txbTag.Name = "txbTag";
            this.txbTag.Size = new System.Drawing.Size(274, 20);
            this.txbTag.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(120, 229);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(206, 229);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlUserInputs
            // 
            this.pnlUserInputs.Controls.Add(this.chkIncludeSampleThumbnails);
            this.pnlUserInputs.Controls.Add(this.usrNetworkLevel);
            this.pnlUserInputs.Controls.Add(this.usrFlickrApiKey);
            this.pnlUserInputs.Controls.Add(this.label1);
            this.pnlUserInputs.Controls.Add(this.txbTag);
            this.pnlUserInputs.Location = new System.Drawing.Point(12, 12);
            this.pnlUserInputs.Name = "pnlUserInputs";
            this.pnlUserInputs.Size = new System.Drawing.Size(280, 211);
            this.pnlUserInputs.TabIndex = 0;
            // 
            // chkIncludeSampleThumbnails
            // 
            this.chkIncludeSampleThumbnails.Location = new System.Drawing.Point(125, 134);
            this.chkIncludeSampleThumbnails.Name = "chkIncludeSampleThumbnails";
            this.chkIncludeSampleThumbnails.Size = new System.Drawing.Size(142, 43);
            this.chkIncludeSampleThumbnails.TabIndex = 4;
            this.chkIncludeSampleThumbnails.Text = "&Add sample image files to the Vertices worksheet (slower)";
            this.chkIncludeSampleThumbnails.UseVisualStyleBackColor = true;
            // 
            // usrNetworkLevel
            // 
            this.usrNetworkLevel.Level = Microsoft.SocialNetworkLib.NetworkLevel.One;
            this.usrNetworkLevel.Location = new System.Drawing.Point(0, 125);
            this.usrNetworkLevel.Name = "usrNetworkLevel";
            this.usrNetworkLevel.Size = new System.Drawing.Size(119, 79);
            this.usrNetworkLevel.TabIndex = 3;
            // 
            // usrFlickrApiKey
            // 
            this.usrFlickrApiKey.ApiKey = "";
            this.usrFlickrApiKey.Location = new System.Drawing.Point(0, 46);
            this.usrFlickrApiKey.Name = "usrFlickrApiKey";
            this.usrFlickrApiKey.Size = new System.Drawing.Size(273, 73);
            this.usrFlickrApiKey.TabIndex = 2;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 264);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(298, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slStatusLabel
            // 
            this.slStatusLabel.Name = "slStatusLabel";
            this.slStatusLabel.Size = new System.Drawing.Size(283, 17);
            this.slStatusLabel.Spring = true;
            this.slStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FlickrGetRelatedTagNetworkDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(298, 286);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlUserInputs);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FlickrGetRelatedTagNetworkDialog";
            this.Text = "[Gets set in code]";
            this.pnlUserInputs.ResumeLayout(false);
            this.pnlUserInputs.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbTag;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlUserInputs;
        private FlickrApiKeyControl usrFlickrApiKey;
        private NetworkLevelControl usrNetworkLevel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slStatusLabel;
        private System.Windows.Forms.CheckBox chkIncludeSampleThumbnails;
    }
}
