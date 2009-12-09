

//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.GraphDataProviders.Flickr
{
    partial class FlickrGetUserNetworkDialog
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlUserInputs = new System.Windows.Forms.Panel();
            this.chkIncludeUserInformation = new System.Windows.Forms.CheckBox();
            this.usrFlickrApiKey = new Microsoft.NodeXL.GraphDataProviders.Flickr.FlickrApiKeyControl();
            this.usrLimitToN = new Microsoft.NodeXL.GraphDataProviders.LimitToNControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radIncludeContactsAndCommenters = new System.Windows.Forms.RadioButton();
            this.radIncludeCommenterVertices = new System.Windows.Forms.RadioButton();
            this.radIncludeContactVertices = new System.Windows.Forms.RadioButton();
            this.txbScreenName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.usrNetworkLevel = new Microsoft.NodeXL.GraphDataProviders.NetworkLevelControl();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlUserInputs.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(161, 319);
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
            this.btnCancel.Location = new System.Drawing.Point(247, 319);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlUserInputs
            // 
            this.pnlUserInputs.Controls.Add(this.chkIncludeUserInformation);
            this.pnlUserInputs.Controls.Add(this.usrFlickrApiKey);
            this.pnlUserInputs.Controls.Add(this.usrLimitToN);
            this.pnlUserInputs.Controls.Add(this.groupBox1);
            this.pnlUserInputs.Controls.Add(this.txbScreenName);
            this.pnlUserInputs.Controls.Add(this.label1);
            this.pnlUserInputs.Controls.Add(this.usrNetworkLevel);
            this.pnlUserInputs.Location = new System.Drawing.Point(12, 12);
            this.pnlUserInputs.Name = "pnlUserInputs";
            this.pnlUserInputs.Size = new System.Drawing.Size(324, 303);
            this.pnlUserInputs.TabIndex = 0;
            // 
            // chkIncludeUserInformation
            // 
            this.chkIncludeUserInformation.Location = new System.Drawing.Point(129, 217);
            this.chkIncludeUserInformation.Name = "chkIncludeUserInformation";
            this.chkIncludeUserInformation.Size = new System.Drawing.Size(168, 43);
            this.chkIncludeUserInformation.TabIndex = 5;
            this.chkIncludeUserInformation.Text = "&Add user information to the Vertices worksheet (slower)";
            this.chkIncludeUserInformation.UseVisualStyleBackColor = true;
            // 
            // usrFlickrApiKey
            // 
            this.usrFlickrApiKey.ApiKey = "";
            this.usrFlickrApiKey.Location = new System.Drawing.Point(0, 46);
            this.usrFlickrApiKey.Name = "usrFlickrApiKey";
            this.usrFlickrApiKey.Size = new System.Drawing.Size(315, 73);
            this.usrFlickrApiKey.TabIndex = 2;
            // 
            // usrLimitToN
            // 
            this.usrLimitToN.Location = new System.Drawing.Point(129, 265);
            this.usrLimitToN.N = 2147483647;
            this.usrLimitToN.Name = "usrLimitToN";
            this.usrLimitToN.ObjectName = "people";
            this.usrLimitToN.Size = new System.Drawing.Size(168, 27);
            this.usrLimitToN.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radIncludeContactsAndCommenters);
            this.groupBox1.Controls.Add(this.radIncludeCommenterVertices);
            this.groupBox1.Controls.Add(this.radIncludeContactVertices);
            this.groupBox1.Location = new System.Drawing.Point(0, 120);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(315, 90);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add a vertex for each";
            // 
            // radIncludeContactsAndCommenters
            // 
            this.radIncludeContactsAndCommenters.AutoSize = true;
            this.radIncludeContactsAndCommenters.Location = new System.Drawing.Point(15, 60);
            this.radIncludeContactsAndCommenters.Name = "radIncludeContactsAndCommenters";
            this.radIncludeContactsAndCommenters.Size = new System.Drawing.Size(47, 17);
            this.radIncludeContactsAndCommenters.TabIndex = 2;
            this.radIncludeContactsAndCommenters.TabStop = true;
            this.radIncludeContactsAndCommenters.Text = "&Both";
            this.radIncludeContactsAndCommenters.UseVisualStyleBackColor = true;
            // 
            // radIncludeCommenterVertices
            // 
            this.radIncludeCommenterVertices.AutoSize = true;
            this.radIncludeCommenterVertices.Location = new System.Drawing.Point(15, 40);
            this.radIncludeCommenterVertices.Name = "radIncludeCommenterVertices";
            this.radIncludeCommenterVertices.Size = new System.Drawing.Size(276, 17);
            this.radIncludeCommenterVertices.TabIndex = 1;
            this.radIncludeCommenterVertices.Text = "Person &who commented on the user\'s photos (slower)";
            this.radIncludeCommenterVertices.UseVisualStyleBackColor = true;
            // 
            // radIncludeContactVertices
            // 
            this.radIncludeContactVertices.AutoSize = true;
            this.radIncludeContactVertices.Checked = true;
            this.radIncludeContactVertices.Location = new System.Drawing.Point(15, 20);
            this.radIncludeContactVertices.Name = "radIncludeContactVertices";
            this.radIncludeContactVertices.Size = new System.Drawing.Size(115, 17);
            this.radIncludeContactVertices.TabIndex = 0;
            this.radIncludeContactVertices.TabStop = true;
            this.radIncludeContactVertices.Text = "&Contact of the user";
            this.radIncludeContactVertices.UseVisualStyleBackColor = true;
            // 
            // txbScreenName
            // 
            this.txbScreenName.Location = new System.Drawing.Point(0, 19);
            this.txbScreenName.MaxLength = 20;
            this.txbScreenName.Name = "txbScreenName";
            this.txbScreenName.Size = new System.Drawing.Size(315, 20);
            this.txbScreenName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(272, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Get the Flickr network of the user with this screen name:";
            // 
            // usrNetworkLevel
            // 
            this.usrNetworkLevel.Level = Microsoft.SocialNetworkLib.NetworkLevel.One;
            this.usrNetworkLevel.Location = new System.Drawing.Point(0, 217);
            this.usrNetworkLevel.Name = "usrNetworkLevel";
            this.usrNetworkLevel.Size = new System.Drawing.Size(119, 79);
            this.usrNetworkLevel.TabIndex = 4;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 353);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(339, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slStatusLabel
            // 
            this.slStatusLabel.Name = "slStatusLabel";
            this.slStatusLabel.Size = new System.Drawing.Size(324, 17);
            this.slStatusLabel.Spring = true;
            this.slStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FlickrGetUserNetworkDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(339, 375);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlUserInputs);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FlickrGetUserNetworkDialog";
            this.Text = "[Gets set in code]";
            this.pnlUserInputs.ResumeLayout(false);
            this.pnlUserInputs.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlUserInputs;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radIncludeContactsAndCommenters;
        private System.Windows.Forms.RadioButton radIncludeCommenterVertices;
        private System.Windows.Forms.RadioButton radIncludeContactVertices;
        private System.Windows.Forms.TextBox txbScreenName;
        private System.Windows.Forms.Label label1;
        private NetworkLevelControl usrNetworkLevel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slStatusLabel;
        private LimitToNControl usrLimitToN;
        private FlickrApiKeyControl usrFlickrApiKey;
        private System.Windows.Forms.CheckBox chkIncludeUserInformation;
    }
}
