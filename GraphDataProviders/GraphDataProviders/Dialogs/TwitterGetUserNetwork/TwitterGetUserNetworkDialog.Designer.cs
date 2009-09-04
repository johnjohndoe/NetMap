

//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
    partial class TwitterGetUserNetworkDialog
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
            this.usrTwitterCredentials = new Microsoft.NodeXL.GraphDataProviders.TwitterCredentialsControl();
            this.usrLimitToNPeople = new Microsoft.NodeXL.GraphDataProviders.Controls.LimitToNPeople.LimitToNPeopleControl();
            this.chkIncludeLatestStatus = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.radIncludeFollowedAndFollowers = new System.Windows.Forms.RadioButton();
            this.radIncludeFollowers = new System.Windows.Forms.RadioButton();
            this.radIncludeFollowed = new System.Windows.Forms.RadioButton();
            this.txbScreenNameToAnalyze = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.usrNetworkLevel = new Microsoft.NodeXL.GraphDataProviders.NetworkLevelControl();
            this.pnlUserInputs.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(151, 509);
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
            this.btnCancel.Location = new System.Drawing.Point(237, 509);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlUserInputs
            // 
            this.pnlUserInputs.Controls.Add(this.usrTwitterCredentials);
            this.pnlUserInputs.Controls.Add(this.usrLimitToNPeople);
            this.pnlUserInputs.Controls.Add(this.chkIncludeLatestStatus);
            this.pnlUserInputs.Controls.Add(this.groupBox1);
            this.pnlUserInputs.Controls.Add(this.txbScreenNameToAnalyze);
            this.pnlUserInputs.Controls.Add(this.label1);
            this.pnlUserInputs.Controls.Add(this.usrNetworkLevel);
            this.pnlUserInputs.Location = new System.Drawing.Point(12, 12);
            this.pnlUserInputs.Name = "pnlUserInputs";
            this.pnlUserInputs.Size = new System.Drawing.Size(312, 491);
            this.pnlUserInputs.TabIndex = 0;
            // 
            // usrTwitterCredentials
            // 
            this.usrTwitterCredentials.Location = new System.Drawing.Point(0, 259);
            this.usrTwitterCredentials.Name = "usrTwitterCredentials";
            this.usrTwitterCredentials.Password = "";
            this.usrTwitterCredentials.ScreenName = "";
            this.usrTwitterCredentials.Size = new System.Drawing.Size(312, 228);
            this.usrTwitterCredentials.TabIndex = 6;
            // 
            // usrLimitToNPeople
            // 
            this.usrLimitToNPeople.Location = new System.Drawing.Point(128, 219);
            this.usrLimitToNPeople.Name = "usrLimitToNPeople";
            this.usrLimitToNPeople.Size = new System.Drawing.Size(168, 27);
            this.usrLimitToNPeople.TabIndex = 5;
            // 
            // chkIncludeLatestStatus
            // 
            this.chkIncludeLatestStatus.Location = new System.Drawing.Point(128, 175);
            this.chkIncludeLatestStatus.Name = "chkIncludeLatestStatus";
            this.chkIncludeLatestStatus.Size = new System.Drawing.Size(176, 43);
            this.chkIncludeLatestStatus.TabIndex = 4;
            this.chkIncludeLatestStatus.Text = "&Include a Latest Tweet column on the Vertices worksheet";
            this.chkIncludeLatestStatus.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.radIncludeFollowedAndFollowers);
            this.groupBox1.Controls.Add(this.radIncludeFollowers);
            this.groupBox1.Controls.Add(this.radIncludeFollowed);
            this.groupBox1.Location = new System.Drawing.Point(0, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(305, 115);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Whom to include";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(246, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "* Requires your screen name and password, below";
            // 
            // radIncludeFollowedAndFollowers
            // 
            this.radIncludeFollowedAndFollowers.AutoSize = true;
            this.radIncludeFollowedAndFollowers.Location = new System.Drawing.Point(15, 66);
            this.radIncludeFollowedAndFollowers.Name = "radIncludeFollowedAndFollowers";
            this.radIncludeFollowedAndFollowers.Size = new System.Drawing.Size(51, 17);
            this.radIncludeFollowedAndFollowers.TabIndex = 2;
            this.radIncludeFollowedAndFollowers.TabStop = true;
            this.radIncludeFollowedAndFollowers.Text = "&Both*";
            this.radIncludeFollowedAndFollowers.UseVisualStyleBackColor = true;
            // 
            // radIncludeFollowers
            // 
            this.radIncludeFollowers.AutoSize = true;
            this.radIncludeFollowers.Location = new System.Drawing.Point(15, 44);
            this.radIncludeFollowers.Name = "radIncludeFollowers";
            this.radIncludeFollowers.Size = new System.Drawing.Size(220, 17);
            this.radIncludeFollowers.TabIndex = 1;
            this.radIncludeFollowers.Text = "F&ollowers*  (the people following the user)";
            this.radIncludeFollowers.UseVisualStyleBackColor = true;
            // 
            // radIncludeFollowed
            // 
            this.radIncludeFollowed.AutoSize = true;
            this.radIncludeFollowed.Checked = true;
            this.radIncludeFollowed.Location = new System.Drawing.Point(15, 22);
            this.radIncludeFollowed.Name = "radIncludeFollowed";
            this.radIncludeFollowed.Size = new System.Drawing.Size(226, 17);
            this.radIncludeFollowed.TabIndex = 0;
            this.radIncludeFollowed.TabStop = true;
            this.radIncludeFollowed.Text = "&Followed  (the people followed by the user)";
            this.radIncludeFollowed.UseVisualStyleBackColor = true;
            // 
            // txbScreenNameToAnalyze
            // 
            this.txbScreenNameToAnalyze.Location = new System.Drawing.Point(0, 19);
            this.txbScreenNameToAnalyze.MaxLength = 100;
            this.txbScreenNameToAnalyze.Name = "txbScreenNameToAnalyze";
            this.txbScreenNameToAnalyze.Size = new System.Drawing.Size(305, 20);
            this.txbScreenNameToAnalyze.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(279, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Get the Twitter network of the user with this screen name:";
            // 
            // usrNetworkLevel
            // 
            this.usrNetworkLevel.Level = Microsoft.SocialNetworkLib.NetworkLevel.One;
            this.usrNetworkLevel.Location = new System.Drawing.Point(0, 170);
            this.usrNetworkLevel.Name = "usrNetworkLevel";
            this.usrNetworkLevel.Size = new System.Drawing.Size(119, 79);
            this.usrNetworkLevel.TabIndex = 3;
            // 
            // TwitterGetUserNetworkDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(333, 547);
            this.Controls.Add(this.pnlUserInputs);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TwitterGetUserNetworkDialog";
            this.Text = "[Gets set in code]";
            this.pnlUserInputs.ResumeLayout(false);
            this.pnlUserInputs.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlUserInputs;
        private System.Windows.Forms.CheckBox chkIncludeLatestStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radIncludeFollowedAndFollowers;
        private System.Windows.Forms.RadioButton radIncludeFollowers;
        private System.Windows.Forms.RadioButton radIncludeFollowed;
        private System.Windows.Forms.TextBox txbScreenNameToAnalyze;
        private System.Windows.Forms.Label label1;
        private NetworkLevelControl usrNetworkLevel;
        private Microsoft.NodeXL.GraphDataProviders.Controls.LimitToNPeople.LimitToNPeopleControl usrLimitToNPeople;
        private TwitterCredentialsControl usrTwitterCredentials;
        private System.Windows.Forms.Label label2;
    }
}
