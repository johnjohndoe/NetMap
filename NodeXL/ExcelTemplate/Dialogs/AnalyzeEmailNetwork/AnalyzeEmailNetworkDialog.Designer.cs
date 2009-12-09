

//  Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class AnalyzeEmailNetworkDialog
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
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lnkHelp = new System.Windows.Forms.LinkLabel();
            this.pnlWhichEmails = new System.Windows.Forms.Panel();
            this.radFilteredEmail = new System.Windows.Forms.RadioButton();
            this.radAllEmail = new System.Windows.Forms.RadioButton();
            this.grpFilters = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txbSubjectText = new System.Windows.Forms.TextBox();
            this.cbxUseSubjectText = new System.Windows.Forms.CheckBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.lnkEmailHelp = new System.Windows.Forms.LinkLabel();
            this.dgvParticipants = new System.Windows.Forms.DataGridView();
            this.colParticipant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFrom = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colTo = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colCc = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colBcc = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cbxUseParticipants = new System.Windows.Forms.CheckBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.radHasAttachmentFromParticipant1 = new System.Windows.Forms.RadioButton();
            this.radNoAttachment = new System.Windows.Forms.RadioButton();
            this.radHasAttachment = new System.Windows.Forms.RadioButton();
            this.cbxUseAttachmentFilter = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.lnkFolderHelp = new System.Windows.Forms.LinkLabel();
            this.txbFolder = new System.Windows.Forms.TextBox();
            this.cbxUseFolder = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txbBodyText = new System.Windows.Forms.TextBox();
            this.cbxUseBodyText = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.radNoBcc = new System.Windows.Forms.RadioButton();
            this.radHasBcc = new System.Windows.Forms.RadioButton();
            this.cbxUseBcc = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radNoCc = new System.Windows.Forms.RadioButton();
            this.radHasCc = new System.Windows.Forms.RadioButton();
            this.cbxUseCc = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbxUseMinimumSize = new System.Windows.Forms.CheckBox();
            this.nudMinimumSize = new System.Windows.Forms.NumericUpDown();
            this.cbxUseMaximumSize = new System.Windows.Forms.CheckBox();
            this.nudMaximumSize = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbxUseEndTime = new System.Windows.Forms.CheckBox();
            this.cbxUseStartTime = new System.Windows.Forms.CheckBox();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.cbxUseCcForEdgeWeights = new System.Windows.Forms.CheckBox();
            this.cbxUseBccForEdgeWeights = new System.Windows.Forms.CheckBox();
            this.pnlWhichEmails.SuspendLayout();
            this.grpFilters.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParticipants)).BeginInit();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinimumSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaximumSize)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Location = new System.Drawing.Point(613, 414);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(75, 23);
            this.btnAnalyze.TabIndex = 4;
            this.btnAnalyze.Text = "Start";
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(694, 414);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // lnkHelp
            // 
            this.lnkHelp.Location = new System.Drawing.Point(533, 10);
            this.lnkHelp.Name = "lnkHelp";
            this.lnkHelp.Size = new System.Drawing.Size(236, 17);
            this.lnkHelp.TabIndex = 6;
            this.lnkHelp.TabStop = true;
            this.lnkHelp.Text = "How email is analyzed and imported";
            this.lnkHelp.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lnkHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelp_LinkClicked);
            // 
            // pnlWhichEmails
            // 
            this.pnlWhichEmails.Controls.Add(this.radFilteredEmail);
            this.pnlWhichEmails.Controls.Add(this.radAllEmail);
            this.pnlWhichEmails.Location = new System.Drawing.Point(9, 7);
            this.pnlWhichEmails.Name = "pnlWhichEmails";
            this.pnlWhichEmails.Size = new System.Drawing.Size(190, 50);
            this.pnlWhichEmails.TabIndex = 0;
            // 
            // radFilteredEmail
            // 
            this.radFilteredEmail.AutoSize = true;
            this.radFilteredEmail.Location = new System.Drawing.Point(2, 26);
            this.radFilteredEmail.Name = "radFilteredEmail";
            this.radFilteredEmail.Size = new System.Drawing.Size(150, 17);
            this.radFilteredEmail.TabIndex = 1;
            this.radFilteredEmail.Text = "Analyze &filtered emails only";
            this.radFilteredEmail.UseVisualStyleBackColor = true;
            this.radFilteredEmail.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // radAllEmail
            // 
            this.radAllEmail.AutoSize = true;
            this.radAllEmail.Checked = true;
            this.radAllEmail.Location = new System.Drawing.Point(3, 3);
            this.radAllEmail.Name = "radAllEmail";
            this.radAllEmail.Size = new System.Drawing.Size(107, 17);
            this.radAllEmail.TabIndex = 0;
            this.radAllEmail.TabStop = true;
            this.radAllEmail.Text = "Analy&ze all emails";
            this.radAllEmail.UseVisualStyleBackColor = true;
            this.radAllEmail.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // grpFilters
            // 
            this.grpFilters.Controls.Add(this.groupBox2);
            this.grpFilters.Controls.Add(this.groupBox9);
            this.grpFilters.Controls.Add(this.groupBox8);
            this.grpFilters.Controls.Add(this.groupBox7);
            this.grpFilters.Controls.Add(this.groupBox6);
            this.grpFilters.Controls.Add(this.groupBox5);
            this.grpFilters.Controls.Add(this.groupBox4);
            this.grpFilters.Controls.Add(this.groupBox3);
            this.grpFilters.Controls.Add(this.groupBox1);
            this.grpFilters.Enabled = false;
            this.grpFilters.Location = new System.Drawing.Point(11, 58);
            this.grpFilters.Name = "grpFilters";
            this.grpFilters.Size = new System.Drawing.Size(758, 335);
            this.grpFilters.TabIndex = 1;
            this.grpFilters.TabStop = false;
            this.grpFilters.Text = "Filters";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txbSubjectText);
            this.groupBox2.Controls.Add(this.cbxUseSubjectText);
            this.groupBox2.Location = new System.Drawing.Point(11, 176);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(284, 71);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Subject";
            // 
            // txbSubjectText
            // 
            this.txbSubjectText.Location = new System.Drawing.Point(16, 40);
            this.txbSubjectText.MaxLength = 500;
            this.txbSubjectText.Name = "txbSubjectText";
            this.txbSubjectText.Size = new System.Drawing.Size(252, 20);
            this.txbSubjectText.TabIndex = 1;
            // 
            // cbxUseSubjectText
            // 
            this.cbxUseSubjectText.AutoSize = true;
            this.cbxUseSubjectText.Location = new System.Drawing.Point(16, 17);
            this.cbxUseSubjectText.Name = "cbxUseSubjectText";
            this.cbxUseSubjectText.Size = new System.Drawing.Size(89, 17);
            this.cbxUseSubjectText.TabIndex = 0;
            this.cbxUseSubjectText.Text = "Inc&ludes text:";
            this.cbxUseSubjectText.UseVisualStyleBackColor = true;
            this.cbxUseSubjectText.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.lnkEmailHelp);
            this.groupBox9.Controls.Add(this.dgvParticipants);
            this.groupBox9.Controls.Add(this.cbxUseParticipants);
            this.groupBox9.Location = new System.Drawing.Point(11, 13);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(284, 158);
            this.groupBox9.TabIndex = 0;
            this.groupBox9.TabStop = false;
            // 
            // lnkEmailHelp
            // 
            this.lnkEmailHelp.AutoSize = true;
            this.lnkEmailHelp.Location = new System.Drawing.Point(13, 48);
            this.lnkEmailHelp.Name = "lnkEmailHelp";
            this.lnkEmailHelp.Size = new System.Drawing.Size(113, 13);
            this.lnkEmailHelp.TabIndex = 1;
            this.lnkEmailHelp.TabStop = true;
            this.lnkEmailHelp.Text = "About email addresses";
            this.lnkEmailHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkEmailHelp_LinkClicked);
            // 
            // dgvParticipants
            // 
            this.dgvParticipants.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParticipants.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colParticipant,
            this.colFrom,
            this.colTo,
            this.colCc,
            this.colBcc});
            this.dgvParticipants.Location = new System.Drawing.Point(17, 73);
            this.dgvParticipants.Name = "dgvParticipants";
            this.dgvParticipants.RowHeadersWidth = 25;
            this.dgvParticipants.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvParticipants.Size = new System.Drawing.Size(251, 76);
            this.dgvParticipants.TabIndex = 2;
            // 
            // colParticipant
            // 
            this.colParticipant.HeaderText = "Email Address";
            this.colParticipant.MaxInputLength = 200;
            this.colParticipant.Name = "colParticipant";
            // 
            // colFrom
            // 
            this.colFrom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colFrom.HeaderText = "From";
            this.colFrom.Name = "colFrom";
            this.colFrom.Width = 36;
            // 
            // colTo
            // 
            this.colTo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colTo.HeaderText = "To";
            this.colTo.Name = "colTo";
            this.colTo.Width = 26;
            // 
            // colCc
            // 
            this.colCc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colCc.HeaderText = "Cc";
            this.colCc.Name = "colCc";
            this.colCc.Width = 26;
            // 
            // colBcc
            // 
            this.colBcc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colBcc.HeaderText = "Bcc";
            this.colBcc.Name = "colBcc";
            this.colBcc.Width = 32;
            // 
            // cbxUseParticipants
            // 
            this.cbxUseParticipants.Location = new System.Drawing.Point(16, 10);
            this.cbxUseParticipants.Name = "cbxUseParticipants";
            this.cbxUseParticipants.Size = new System.Drawing.Size(242, 34);
            this.cbxUseParticipants.TabIndex = 0;
            this.cbxUseParticipants.Text = "&Includes these email addresses on the From, To, Cc, or Bcc lines";
            this.cbxUseParticipants.UseVisualStyleBackColor = true;
            this.cbxUseParticipants.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.radHasAttachmentFromParticipant1);
            this.groupBox8.Controls.Add(this.radNoAttachment);
            this.groupBox8.Controls.Add(this.radHasAttachment);
            this.groupBox8.Controls.Add(this.cbxUseAttachmentFilter);
            this.groupBox8.Location = new System.Drawing.Point(303, 207);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(269, 116);
            this.groupBox8.TabIndex = 5;
            this.groupBox8.TabStop = false;
            // 
            // radHasAttachmentFromParticipant1
            // 
            this.radHasAttachmentFromParticipant1.AutoSize = true;
            this.radHasAttachmentFromParticipant1.Location = new System.Drawing.Point(16, 86);
            this.radHasAttachmentFromParticipant1.Name = "radHasAttachmentFromParticipant1";
            this.radHasAttachmentFromParticipant1.Size = new System.Drawing.Size(236, 17);
            this.radHasAttachmentFromParticipant1.TabIndex = 3;
            this.radHasAttachmentFromParticipant1.Text = "From first email address and ha&s attachments";
            this.radHasAttachmentFromParticipant1.UseVisualStyleBackColor = true;
            // 
            // radNoAttachment
            // 
            this.radNoAttachment.AutoSize = true;
            this.radNoAttachment.Location = new System.Drawing.Point(16, 63);
            this.radNoAttachment.Name = "radNoAttachment";
            this.radNoAttachment.Size = new System.Drawing.Size(149, 17);
            this.radNoAttachment.TabIndex = 2;
            this.radNoAttachment.Text = "&Doesn\'t have attachments";
            this.radNoAttachment.UseVisualStyleBackColor = true;
            // 
            // radHasAttachment
            // 
            this.radHasAttachment.AutoSize = true;
            this.radHasAttachment.Checked = true;
            this.radHasAttachment.Location = new System.Drawing.Point(16, 40);
            this.radHasAttachment.Name = "radHasAttachment";
            this.radHasAttachment.Size = new System.Drawing.Size(105, 17);
            this.radHasAttachment.TabIndex = 1;
            this.radHasAttachment.TabStop = true;
            this.radHasAttachment.Text = "&Has attachments";
            this.radHasAttachment.UseVisualStyleBackColor = true;
            // 
            // cbxUseAttachmentFilter
            // 
            this.cbxUseAttachmentFilter.AutoSize = true;
            this.cbxUseAttachmentFilter.Location = new System.Drawing.Point(16, 17);
            this.cbxUseAttachmentFilter.Name = "cbxUseAttachmentFilter";
            this.cbxUseAttachmentFilter.Size = new System.Drawing.Size(85, 17);
            this.cbxUseAttachmentFilter.TabIndex = 0;
            this.cbxUseAttachmentFilter.Text = "A&ttachments";
            this.cbxUseAttachmentFilter.UseVisualStyleBackColor = true;
            this.cbxUseAttachmentFilter.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.lnkFolderHelp);
            this.groupBox7.Controls.Add(this.txbFolder);
            this.groupBox7.Controls.Add(this.cbxUseFolder);
            this.groupBox7.Location = new System.Drawing.Point(580, 13);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(166, 92);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Folder";
            // 
            // lnkFolderHelp
            // 
            this.lnkFolderHelp.AutoSize = true;
            this.lnkFolderHelp.Location = new System.Drawing.Point(13, 68);
            this.lnkFolderHelp.Name = "lnkFolderHelp";
            this.lnkFolderHelp.Size = new System.Drawing.Size(76, 13);
            this.lnkFolderHelp.TabIndex = 2;
            this.lnkFolderHelp.TabStop = true;
            this.lnkFolderHelp.Text = "Sample folders";
            this.lnkFolderHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFolderHelp_LinkClicked);
            // 
            // txbFolder
            // 
            this.txbFolder.Location = new System.Drawing.Point(16, 45);
            this.txbFolder.MaxLength = 200;
            this.txbFolder.Name = "txbFolder";
            this.txbFolder.Size = new System.Drawing.Size(134, 20);
            this.txbFolder.TabIndex = 1;
            // 
            // cbxUseFolder
            // 
            this.cbxUseFolder.AutoSize = true;
            this.cbxUseFolder.Location = new System.Drawing.Point(16, 22);
            this.cbxUseFolder.Name = "cbxUseFolder";
            this.cbxUseFolder.Size = new System.Drawing.Size(67, 17);
            this.cbxUseFolder.TabIndex = 0;
            this.cbxUseFolder.Text = "In f&older:";
            this.cbxUseFolder.UseVisualStyleBackColor = true;
            this.cbxUseFolder.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.txbBodyText);
            this.groupBox6.Controls.Add(this.cbxUseBodyText);
            this.groupBox6.Location = new System.Drawing.Point(11, 252);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(284, 71);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Message body";
            // 
            // txbBodyText
            // 
            this.txbBodyText.Location = new System.Drawing.Point(16, 40);
            this.txbBodyText.MaxLength = 500;
            this.txbBodyText.Name = "txbBodyText";
            this.txbBodyText.Size = new System.Drawing.Size(252, 20);
            this.txbBodyText.TabIndex = 1;
            // 
            // cbxUseBodyText
            // 
            this.cbxUseBodyText.AutoSize = true;
            this.cbxUseBodyText.Location = new System.Drawing.Point(16, 17);
            this.cbxUseBodyText.Name = "cbxUseBodyText";
            this.cbxUseBodyText.Size = new System.Drawing.Size(89, 17);
            this.cbxUseBodyText.TabIndex = 0;
            this.cbxUseBodyText.Text = "Includes te&xt:";
            this.cbxUseBodyText.UseVisualStyleBackColor = true;
            this.cbxUseBodyText.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.radNoBcc);
            this.groupBox5.Controls.Add(this.radHasBcc);
            this.groupBox5.Controls.Add(this.cbxUseBcc);
            this.groupBox5.Location = new System.Drawing.Point(580, 207);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(166, 116);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            // 
            // radNoBcc
            // 
            this.radNoBcc.AutoSize = true;
            this.radNoBcc.Location = new System.Drawing.Point(15, 66);
            this.radNoBcc.Name = "radNoBcc";
            this.radNoBcc.Size = new System.Drawing.Size(110, 17);
            this.radNoBcc.TabIndex = 2;
            this.radNoBcc.Text = "Does&n\'t have Bcc";
            this.radNoBcc.UseVisualStyleBackColor = true;
            // 
            // radHasBcc
            // 
            this.radHasBcc.AutoSize = true;
            this.radHasBcc.Checked = true;
            this.radHasBcc.Location = new System.Drawing.Point(15, 45);
            this.radHasBcc.Name = "radHasBcc";
            this.radHasBcc.Size = new System.Drawing.Size(66, 17);
            this.radHasBcc.TabIndex = 1;
            this.radHasBcc.TabStop = true;
            this.radHasBcc.Text = "&Has Bcc";
            this.radHasBcc.UseVisualStyleBackColor = true;
            // 
            // cbxUseBcc
            // 
            this.cbxUseBcc.AutoSize = true;
            this.cbxUseBcc.Location = new System.Drawing.Point(16, 22);
            this.cbxUseBcc.Name = "cbxUseBcc";
            this.cbxUseBcc.Size = new System.Drawing.Size(45, 17);
            this.cbxUseBcc.TabIndex = 0;
            this.cbxUseBcc.Text = "&Bcc";
            this.cbxUseBcc.UseVisualStyleBackColor = true;
            this.cbxUseBcc.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radNoCc);
            this.groupBox4.Controls.Add(this.radHasCc);
            this.groupBox4.Controls.Add(this.cbxUseCc);
            this.groupBox4.Location = new System.Drawing.Point(580, 111);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(166, 92);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            // 
            // radNoCc
            // 
            this.radNoCc.AutoSize = true;
            this.radNoCc.Location = new System.Drawing.Point(16, 66);
            this.radNoCc.Name = "radNoCc";
            this.radNoCc.Size = new System.Drawing.Size(104, 17);
            this.radNoCc.TabIndex = 2;
            this.radNoCc.Text = "Doesn\'t ha&ve Cc";
            this.radNoCc.UseVisualStyleBackColor = true;
            // 
            // radHasCc
            // 
            this.radHasCc.AutoSize = true;
            this.radHasCc.Checked = true;
            this.radHasCc.Location = new System.Drawing.Point(16, 43);
            this.radHasCc.Name = "radHasCc";
            this.radHasCc.Size = new System.Drawing.Size(60, 17);
            this.radHasCc.TabIndex = 1;
            this.radHasCc.TabStop = true;
            this.radHasCc.Text = "H&as Cc";
            this.radHasCc.UseVisualStyleBackColor = true;
            // 
            // cbxUseCc
            // 
            this.cbxUseCc.AutoSize = true;
            this.cbxUseCc.Location = new System.Drawing.Point(16, 22);
            this.cbxUseCc.Name = "cbxUseCc";
            this.cbxUseCc.Size = new System.Drawing.Size(39, 17);
            this.cbxUseCc.TabIndex = 0;
            this.cbxUseCc.Text = "&Cc";
            this.cbxUseCc.UseVisualStyleBackColor = true;
            this.cbxUseCc.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbxUseMinimumSize);
            this.groupBox3.Controls.Add(this.nudMinimumSize);
            this.groupBox3.Controls.Add(this.cbxUseMaximumSize);
            this.groupBox3.Controls.Add(this.nudMaximumSize);
            this.groupBox3.Location = new System.Drawing.Point(303, 110);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(269, 92);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Size range (bytes)";
            // 
            // cbxUseMinimumSize
            // 
            this.cbxUseMinimumSize.AutoSize = true;
            this.cbxUseMinimumSize.Location = new System.Drawing.Point(16, 22);
            this.cbxUseMinimumSize.Name = "cbxUseMinimumSize";
            this.cbxUseMinimumSize.Size = new System.Drawing.Size(70, 17);
            this.cbxUseMinimumSize.TabIndex = 0;
            this.cbxUseMinimumSize.Text = "&Minimum:";
            this.cbxUseMinimumSize.UseVisualStyleBackColor = true;
            this.cbxUseMinimumSize.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // nudMinimumSize
            // 
            this.nudMinimumSize.Location = new System.Drawing.Point(16, 45);
            this.nudMinimumSize.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudMinimumSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMinimumSize.Name = "nudMinimumSize";
            this.nudMinimumSize.Size = new System.Drawing.Size(84, 20);
            this.nudMinimumSize.TabIndex = 1;
            this.nudMinimumSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbxUseMaximumSize
            // 
            this.cbxUseMaximumSize.AutoSize = true;
            this.cbxUseMaximumSize.Location = new System.Drawing.Point(129, 22);
            this.cbxUseMaximumSize.Name = "cbxUseMaximumSize";
            this.cbxUseMaximumSize.Size = new System.Drawing.Size(73, 17);
            this.cbxUseMaximumSize.TabIndex = 2;
            this.cbxUseMaximumSize.Text = "Maxim&um:";
            this.cbxUseMaximumSize.UseVisualStyleBackColor = true;
            this.cbxUseMaximumSize.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // nudMaximumSize
            // 
            this.nudMaximumSize.Location = new System.Drawing.Point(129, 45);
            this.nudMaximumSize.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudMaximumSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMaximumSize.Name = "nudMaximumSize";
            this.nudMaximumSize.Size = new System.Drawing.Size(84, 20);
            this.nudMaximumSize.TabIndex = 3;
            this.nudMaximumSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbxUseEndTime);
            this.groupBox1.Controls.Add(this.cbxUseStartTime);
            this.groupBox1.Controls.Add(this.dtpStartTime);
            this.groupBox1.Controls.Add(this.dtpEndTime);
            this.groupBox1.Location = new System.Drawing.Point(303, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(269, 92);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Date range";
            // 
            // cbxUseEndTime
            // 
            this.cbxUseEndTime.AutoSize = true;
            this.cbxUseEndTime.Location = new System.Drawing.Point(129, 22);
            this.cbxUseEndTime.Name = "cbxUseEndTime";
            this.cbxUseEndTime.Size = new System.Drawing.Size(111, 17);
            this.cbxUseEndTime.TabIndex = 2;
            this.cbxUseEndTime.Text = "S&ent on or before:";
            this.cbxUseEndTime.UseVisualStyleBackColor = true;
            this.cbxUseEndTime.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // cbxUseStartTime
            // 
            this.cbxUseStartTime.AutoSize = true;
            this.cbxUseStartTime.Location = new System.Drawing.Point(16, 22);
            this.cbxUseStartTime.Name = "cbxUseStartTime";
            this.cbxUseStartTime.Size = new System.Drawing.Size(102, 17);
            this.cbxUseStartTime.TabIndex = 0;
            this.cbxUseStartTime.Text = "Sent on o&r after:";
            this.cbxUseStartTime.UseVisualStyleBackColor = true;
            this.cbxUseStartTime.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartTime.Location = new System.Drawing.Point(15, 45);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.Size = new System.Drawing.Size(95, 20);
            this.dtpStartTime.TabIndex = 1;
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndTime.Location = new System.Drawing.Point(129, 45);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(95, 20);
            this.dtpEndTime.TabIndex = 3;
            // 
            // cbxUseCcForEdgeWeights
            // 
            this.cbxUseCcForEdgeWeights.AutoSize = true;
            this.cbxUseCcForEdgeWeights.Location = new System.Drawing.Point(12, 399);
            this.cbxUseCcForEdgeWeights.Name = "cbxUseCcForEdgeWeights";
            this.cbxUseCcForEdgeWeights.Size = new System.Drawing.Size(229, 17);
            this.cbxUseCcForEdgeWeights.TabIndex = 2;
            this.cbxUseCcForEdgeWeights.Text = "Use Cc line &when calculating edge weights";
            this.cbxUseCcForEdgeWeights.UseVisualStyleBackColor = true;
            // 
            // cbxUseBccForEdgeWeights
            // 
            this.cbxUseBccForEdgeWeights.AutoSize = true;
            this.cbxUseBccForEdgeWeights.Location = new System.Drawing.Point(12, 422);
            this.cbxUseBccForEdgeWeights.Name = "cbxUseBccForEdgeWeights";
            this.cbxUseBccForEdgeWeights.Size = new System.Drawing.Size(235, 17);
            this.cbxUseBccForEdgeWeights.TabIndex = 3;
            this.cbxUseBccForEdgeWeights.Text = "Use Bcc line when calculatin&g edge weights";
            this.cbxUseBccForEdgeWeights.UseVisualStyleBackColor = true;
            // 
            // AnalyzeEmailNetworkDialog
            // 
            this.AcceptButton = this.btnAnalyze;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(782, 451);
            this.Controls.Add(this.cbxUseBccForEdgeWeights);
            this.Controls.Add(this.cbxUseCcForEdgeWeights);
            this.Controls.Add(this.grpFilters);
            this.Controls.Add(this.pnlWhichEmails);
            this.Controls.Add(this.lnkHelp);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAnalyze);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AnalyzeEmailNetworkDialog";
            this.Text = "Import from Email Network";
            this.pnlWhichEmails.ResumeLayout(false);
            this.pnlWhichEmails.PerformLayout();
            this.grpFilters.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParticipants)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinimumSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaximumSize)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.LinkLabel lnkHelp;
        private System.Windows.Forms.Panel pnlWhichEmails;
        private System.Windows.Forms.RadioButton radFilteredEmail;
        private System.Windows.Forms.RadioButton radAllEmail;
        private System.Windows.Forms.GroupBox grpFilters;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.CheckBox cbxUseStartTime;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.CheckBox cbxUseEndTime;
        private System.Windows.Forms.CheckBox cbxUseMinimumSize;
        private System.Windows.Forms.NumericUpDown nudMaximumSize;
        private System.Windows.Forms.CheckBox cbxUseMaximumSize;
        private System.Windows.Forms.NumericUpDown nudMinimumSize;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radHasCc;
        private System.Windows.Forms.CheckBox cbxUseCc;
        private System.Windows.Forms.RadioButton radNoCc;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton radNoBcc;
        private System.Windows.Forms.RadioButton radHasBcc;
        private System.Windows.Forms.CheckBox cbxUseBcc;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox txbBodyText;
        private System.Windows.Forms.CheckBox cbxUseBodyText;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox txbFolder;
        private System.Windows.Forms.CheckBox cbxUseFolder;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.RadioButton radNoAttachment;
        private System.Windows.Forms.RadioButton radHasAttachment;
        private System.Windows.Forms.CheckBox cbxUseAttachmentFilter;
        private System.Windows.Forms.RadioButton radHasAttachmentFromParticipant1;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.CheckBox cbxUseParticipants;
        private System.Windows.Forms.DataGridView dgvParticipants;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParticipant;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colFrom;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colTo;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCc;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colBcc;
        private System.Windows.Forms.CheckBox cbxUseCcForEdgeWeights;
        private System.Windows.Forms.CheckBox cbxUseBccForEdgeWeights;
        private System.Windows.Forms.LinkLabel lnkFolderHelp;
        private System.Windows.Forms.LinkLabel lnkEmailHelp;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txbSubjectText;
        private System.Windows.Forms.CheckBox cbxUseSubjectText;
    }
}
