

//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.GraphDataProviders.Flickr
{
    partial class FlickrGetRelatedTagsDialog
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
            this.lnkRequestFlickrApiKey = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.txbFlickrApiKey = new System.Windows.Forms.TextBox();
            this.txbTag = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlUserInputs = new System.Windows.Forms.Panel();
            this.pnlUserInputs.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(267, 47);
            this.label1.TabIndex = 0;
            this.label1.Text = "This will get the 1.5-level network of Flickr tags related to a specified tag.  T" +
                "o use this, you will need what Flickr calls an \"API key.\"";
            // 
            // lnkRequestFlickrApiKey
            // 
            this.lnkRequestFlickrApiKey.AutoSize = true;
            this.lnkRequestFlickrApiKey.Location = new System.Drawing.Point(0, 49);
            this.lnkRequestFlickrApiKey.Name = "lnkRequestFlickrApiKey";
            this.lnkRequestFlickrApiKey.Size = new System.Drawing.Size(125, 13);
            this.lnkRequestFlickrApiKey.TabIndex = 1;
            this.lnkRequestFlickrApiKey.TabStop = true;
            this.lnkRequestFlickrApiKey.Text = "Apply for a Flickr API key";
            this.lnkRequestFlickrApiKey.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRequestFlickrApiKey_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Flickr API key:";
            // 
            // txbFlickrApiKey
            // 
            this.txbFlickrApiKey.Location = new System.Drawing.Point(0, 97);
            this.txbFlickrApiKey.MaxLength = 200;
            this.txbFlickrApiKey.Name = "txbFlickrApiKey";
            this.txbFlickrApiKey.Size = new System.Drawing.Size(267, 20);
            this.txbFlickrApiKey.TabIndex = 3;
            // 
            // txbTag
            // 
            this.txbTag.Location = new System.Drawing.Point(0, 149);
            this.txbTag.MaxLength = 100;
            this.txbTag.Name = "txbTag";
            this.txbTag.Size = new System.Drawing.Size(267, 20);
            this.txbTag.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-3, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "&Tag:";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(113, 199);
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
            this.btnCancel.Location = new System.Drawing.Point(199, 199);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlUserInputs
            // 
            this.pnlUserInputs.Controls.Add(this.label1);
            this.pnlUserInputs.Controls.Add(this.lnkRequestFlickrApiKey);
            this.pnlUserInputs.Controls.Add(this.label2);
            this.pnlUserInputs.Controls.Add(this.label3);
            this.pnlUserInputs.Controls.Add(this.txbFlickrApiKey);
            this.pnlUserInputs.Controls.Add(this.txbTag);
            this.pnlUserInputs.Location = new System.Drawing.Point(12, 12);
            this.pnlUserInputs.Name = "pnlUserInputs";
            this.pnlUserInputs.Size = new System.Drawing.Size(267, 177);
            this.pnlUserInputs.TabIndex = 0;
            // 
            // FlickrGetRelatedTagsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(291, 239);
            this.Controls.Add(this.pnlUserInputs);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FlickrGetRelatedTagsDialog";
            this.Text = "[Gets set in code]";
            this.pnlUserInputs.ResumeLayout(false);
            this.pnlUserInputs.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel lnkRequestFlickrApiKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbFlickrApiKey;
        private System.Windows.Forms.TextBox txbTag;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlUserInputs;
    }
}
