

//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
    partial class TwitterGetSearchNetworkDialog
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
            this.chkIncludeStatuses = new System.Windows.Forms.CheckBox();
            this.linkLabel1 = new Microsoft.Research.CommunityTechnologies.AppLib.FileNameLinkLabel();
            this.chkIncludeStatistics = new System.Windows.Forms.CheckBox();
            this.usrTwitterCredentials = new Microsoft.NodeXL.GraphDataProviders.TwitterCredentialsControl();
            this.usrLimitToNPeople = new Microsoft.NodeXL.GraphDataProviders.LimitToNPeopleControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkIncludeMentionsEdges = new System.Windows.Forms.CheckBox();
            this.chkIncludeRepliesToEdges = new System.Windows.Forms.CheckBox();
            this.chkIncludeFollowedEdges = new System.Windows.Forms.CheckBox();
            this.txbSearchTerm = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlUserInputs.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(151, 492);
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
            this.btnCancel.Location = new System.Drawing.Point(237, 492);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlUserInputs
            // 
            this.pnlUserInputs.Controls.Add(this.chkIncludeStatuses);
            this.pnlUserInputs.Controls.Add(this.linkLabel1);
            this.pnlUserInputs.Controls.Add(this.chkIncludeStatistics);
            this.pnlUserInputs.Controls.Add(this.usrTwitterCredentials);
            this.pnlUserInputs.Controls.Add(this.usrLimitToNPeople);
            this.pnlUserInputs.Controls.Add(this.groupBox2);
            this.pnlUserInputs.Controls.Add(this.txbSearchTerm);
            this.pnlUserInputs.Controls.Add(this.label1);
            this.pnlUserInputs.Location = new System.Drawing.Point(12, 12);
            this.pnlUserInputs.Name = "pnlUserInputs";
            this.pnlUserInputs.Size = new System.Drawing.Size(312, 473);
            this.pnlUserInputs.TabIndex = 0;
            // 
            // chkIncludeStatuses
            // 
            this.chkIncludeStatuses.AutoSize = true;
            this.chkIncludeStatuses.Location = new System.Drawing.Point(0, 194);
            this.chkIncludeStatuses.Name = "chkIncludeStatuses";
            this.chkIncludeStatuses.Size = new System.Drawing.Size(247, 17);
            this.chkIncludeStatuses.TabIndex = 5;
            this.chkIncludeStatuses.Text = "A&dd a Tweet column to the Vertices worksheet";
            this.chkIncludeStatuses.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.FileName = "http://search.twitter.com/operators";
            this.linkLabel1.Location = new System.Drawing.Point(0, 45);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(114, 13);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Advanced search help";
            // 
            // chkIncludeStatistics
            // 
            this.chkIncludeStatistics.AutoSize = true;
            this.chkIncludeStatistics.Location = new System.Drawing.Point(0, 218);
            this.chkIncludeStatistics.Name = "chkIncludeStatistics";
            this.chkIncludeStatistics.Size = new System.Drawing.Size(287, 17);
            this.chkIncludeStatistics.TabIndex = 6;
            this.chkIncludeStatistics.Text = "Add s&tatistic columns to the Vertices worksheet (slower)";
            this.chkIncludeStatistics.UseVisualStyleBackColor = true;
            // 
            // usrTwitterCredentials
            // 
            this.usrTwitterCredentials.Location = new System.Drawing.Point(0, 246);
            this.usrTwitterCredentials.Name = "usrTwitterCredentials";
            this.usrTwitterCredentials.Password = "";
            this.usrTwitterCredentials.ScreenName = "";
            this.usrTwitterCredentials.Size = new System.Drawing.Size(312, 228);
            this.usrTwitterCredentials.TabIndex = 7;
            // 
            // usrLimitToNPeople
            // 
            this.usrLimitToNPeople.Location = new System.Drawing.Point(0, 165);
            this.usrLimitToNPeople.N = 2147483647;
            this.usrLimitToNPeople.Name = "usrLimitToNPeople";
            this.usrLimitToNPeople.Size = new System.Drawing.Size(168, 27);
            this.usrLimitToNPeople.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkIncludeMentionsEdges);
            this.groupBox2.Controls.Add(this.chkIncludeRepliesToEdges);
            this.groupBox2.Controls.Add(this.chkIncludeFollowedEdges);
            this.groupBox2.Location = new System.Drawing.Point(0, 72);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(305, 85);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Add an edge for each";
            // 
            // chkIncludeMentionsEdges
            // 
            this.chkIncludeMentionsEdges.AutoSize = true;
            this.chkIncludeMentionsEdges.Location = new System.Drawing.Point(15, 60);
            this.chkIncludeMentionsEdges.Name = "chkIncludeMentionsEdges";
            this.chkIncludeMentionsEdges.Size = new System.Drawing.Size(180, 17);
            this.chkIncludeMentionsEdges.TabIndex = 2;
            this.chkIncludeMentionsEdges.Text = "\"&Mentions\" relationship in tweets";
            this.chkIncludeMentionsEdges.UseVisualStyleBackColor = true;
            // 
            // chkIncludeRepliesToEdges
            // 
            this.chkIncludeRepliesToEdges.AutoSize = true;
            this.chkIncludeRepliesToEdges.Location = new System.Drawing.Point(15, 40);
            this.chkIncludeRepliesToEdges.Name = "chkIncludeRepliesToEdges";
            this.chkIncludeRepliesToEdges.Size = new System.Drawing.Size(184, 17);
            this.chkIncludeRepliesToEdges.TabIndex = 1;
            this.chkIncludeRepliesToEdges.Text = "\"&Replies-to\" relationship in tweets";
            this.chkIncludeRepliesToEdges.UseVisualStyleBackColor = true;
            // 
            // chkIncludeFollowedEdges
            // 
            this.chkIncludeFollowedEdges.AutoSize = true;
            this.chkIncludeFollowedEdges.Location = new System.Drawing.Point(15, 20);
            this.chkIncludeFollowedEdges.Name = "chkIncludeFollowedEdges";
            this.chkIncludeFollowedEdges.Size = new System.Drawing.Size(163, 17);
            this.chkIncludeFollowedEdges.TabIndex = 0;
            this.chkIncludeFollowedEdges.Text = "&Followed relationship (slower)";
            this.chkIncludeFollowedEdges.UseVisualStyleBackColor = true;
            // 
            // txbSearchTerm
            // 
            this.txbSearchTerm.Location = new System.Drawing.Point(0, 19);
            this.txbSearchTerm.MaxLength = 100;
            this.txbSearchTerm.Name = "txbSearchTerm";
            this.txbSearchTerm.Size = new System.Drawing.Size(305, 20);
            this.txbSearchTerm.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Search for people whose tweets contain:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 525);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(333, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slStatusLabel
            // 
            this.slStatusLabel.Name = "slStatusLabel";
            this.slStatusLabel.Size = new System.Drawing.Size(318, 17);
            this.slStatusLabel.Spring = true;
            this.slStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TwitterGetSearchNetworkDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(333, 547);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlUserInputs);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TwitterGetSearchNetworkDialog";
            this.Text = "[Gets set in code]";
            this.pnlUserInputs.ResumeLayout(false);
            this.pnlUserInputs.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlUserInputs;
        private System.Windows.Forms.TextBox txbSearchTerm;
        private System.Windows.Forms.Label label1;
        private Microsoft.NodeXL.GraphDataProviders.LimitToNPeopleControl usrLimitToNPeople;
        private TwitterCredentialsControl usrTwitterCredentials;
        private System.Windows.Forms.CheckBox chkIncludeStatistics;
        private Microsoft.Research.CommunityTechnologies.AppLib.FileNameLinkLabel linkLabel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slStatusLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkIncludeMentionsEdges;
        private System.Windows.Forms.CheckBox chkIncludeRepliesToEdges;
        private System.Windows.Forms.CheckBox chkIncludeFollowedEdges;
        private System.Windows.Forms.CheckBox chkIncludeStatuses;
    }
}
