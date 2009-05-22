

//  Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class AnalyzeTwitterNetworkDialog
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
            this.txbScreenNameToAnalyze = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlUserInputs = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txbCredentialsPassword = new System.Windows.Forms.TextBox();
            this.txbCredentialsScreenName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lnkRequestWhitelist = new System.Windows.Forms.LinkLabel();
            this.lnkRateLimiting = new System.Windows.Forms.LinkLabel();
            this.cbxAnalyzeTwoLevels = new System.Windows.Forms.CheckBox();
            this.cbxPopulatePrimaryLabels = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.pnlUserInputs.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txbScreenNameToAnalyze
            // 
            this.txbScreenNameToAnalyze.Location = new System.Drawing.Point(12, 28);
            this.txbScreenNameToAnalyze.MaxLength = 100;
            this.txbScreenNameToAnalyze.Name = "txbScreenNameToAnalyze";
            this.txbScreenNameToAnalyze.Size = new System.Drawing.Size(304, 20);
            this.txbScreenNameToAnalyze.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(282, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Show the friends of the Twitter user with this screen name:";
            // 
            // pnlUserInputs
            // 
            this.pnlUserInputs.Controls.Add(this.groupBox1);
            this.pnlUserInputs.Controls.Add(this.lnkRequestWhitelist);
            this.pnlUserInputs.Controls.Add(this.lnkRateLimiting);
            this.pnlUserInputs.Controls.Add(this.cbxAnalyzeTwoLevels);
            this.pnlUserInputs.Controls.Add(this.cbxPopulatePrimaryLabels);
            this.pnlUserInputs.Controls.Add(this.txbScreenNameToAnalyze);
            this.pnlUserInputs.Controls.Add(this.label1);
            this.pnlUserInputs.Location = new System.Drawing.Point(0, 0);
            this.pnlUserInputs.Name = "pnlUserInputs";
            this.pnlUserInputs.Size = new System.Drawing.Size(330, 347);
            this.pnlUserInputs.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txbCredentialsPassword);
            this.groupBox1.Controls.Add(this.txbCredentialsScreenName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 129);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(304, 171);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Optional: Your Twitter account";
            // 
            // txbCredentialsPassword
            // 
            this.txbCredentialsPassword.Location = new System.Drawing.Point(11, 134);
            this.txbCredentialsPassword.MaxLength = 100;
            this.txbCredentialsPassword.Name = "txbCredentialsPassword";
            this.txbCredentialsPassword.PasswordChar = '*';
            this.txbCredentialsPassword.Size = new System.Drawing.Size(282, 20);
            this.txbCredentialsPassword.TabIndex = 4;
            // 
            // txbCredentialsScreenName
            // 
            this.txbCredentialsScreenName.Location = new System.Drawing.Point(11, 88);
            this.txbCredentialsScreenName.MaxLength = 100;
            this.txbCredentialsScreenName.Name = "txbCredentialsScreenName";
            this.txbCredentialsScreenName.Size = new System.Drawing.Size(282, 20);
            this.txbCredentialsScreenName.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Your &password:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Your screen &name:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(11, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(282, 47);
            this.label2.TabIndex = 0;
            this.label2.Text = "This is optional, and useful only if you have requested and been granted a liftin" +
                "g of Twitter rate limiting";
            // 
            // lnkRequestWhitelist
            // 
            this.lnkRequestWhitelist.AutoSize = true;
            this.lnkRequestWhitelist.Location = new System.Drawing.Point(31, 102);
            this.lnkRequestWhitelist.Name = "lnkRequestWhitelist";
            this.lnkRequestWhitelist.Size = new System.Drawing.Size(176, 13);
            this.lnkRequestWhitelist.TabIndex = 4;
            this.lnkRequestWhitelist.TabStop = true;
            this.lnkRequestWhitelist.Text = "Request lifting of Twitter rate limiting";
            this.lnkRequestWhitelist.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRequestWhitelist_LinkClicked);
            // 
            // lnkRateLimiting
            // 
            this.lnkRateLimiting.AutoSize = true;
            this.lnkRateLimiting.Location = new System.Drawing.Point(31, 80);
            this.lnkRateLimiting.Name = "lnkRateLimiting";
            this.lnkRateLimiting.Size = new System.Drawing.Size(213, 13);
            this.lnkRateLimiting.TabIndex = 3;
            this.lnkRateLimiting.TabStop = true;
            this.lnkRateLimiting.Text = "Why this might not work: Twitter rate limiting";
            this.lnkRateLimiting.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRateLimiting_LinkClicked);
            // 
            // cbxAnalyzeTwoLevels
            // 
            this.cbxAnalyzeTwoLevels.AutoSize = true;
            this.cbxAnalyzeTwoLevels.Location = new System.Drawing.Point(12, 58);
            this.cbxAnalyzeTwoLevels.Name = "cbxAnalyzeTwoLevels";
            this.cbxAnalyzeTwoLevels.Size = new System.Drawing.Size(162, 17);
            this.cbxAnalyzeTwoLevels.TabIndex = 2;
            this.cbxAnalyzeTwoLevels.Text = "&Also show the friends\' friends";
            this.cbxAnalyzeTwoLevels.UseVisualStyleBackColor = true;
            // 
            // cbxPopulatePrimaryLabels
            // 
            this.cbxPopulatePrimaryLabels.AutoSize = true;
            this.cbxPopulatePrimaryLabels.Location = new System.Drawing.Point(12, 315);
            this.cbxPopulatePrimaryLabels.Name = "cbxPopulatePrimaryLabels";
            this.cbxPopulatePrimaryLabels.Size = new System.Drawing.Size(265, 17);
            this.cbxPopulatePrimaryLabels.TabIndex = 6;
            this.cbxPopulatePrimaryLabels.Text = "&Include everyone\'s latest postings as primary labels";
            this.cbxPopulatePrimaryLabels.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(241, 353);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Location = new System.Drawing.Point(160, 353);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(75, 23);
            this.btnAnalyze.TabIndex = 1;
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // AnalyzeTwitterNetworkDialog
            // 
            this.AcceptButton = this.btnAnalyze;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(328, 389);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAnalyze);
            this.Controls.Add(this.pnlUserInputs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AnalyzeTwitterNetworkDialog";
            this.Text = "Import from Twitter Network";
            this.pnlUserInputs.ResumeLayout(false);
            this.pnlUserInputs.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txbScreenNameToAnalyze;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlUserInputs;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.CheckBox cbxPopulatePrimaryLabels;
        private System.Windows.Forms.LinkLabel lnkRateLimiting;
        private System.Windows.Forms.CheckBox cbxAnalyzeTwoLevels;
        private System.Windows.Forms.LinkLabel lnkRequestWhitelist;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txbCredentialsScreenName;
        private System.Windows.Forms.TextBox txbCredentialsPassword;
    }
}
