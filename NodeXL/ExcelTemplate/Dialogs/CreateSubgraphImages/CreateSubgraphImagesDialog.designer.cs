

//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class CreateSubgraphImagesDialog
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
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlDisableWhileCreating = new System.Windows.Forms.Panel();
            this.usrSubgraphLevels = new Microsoft.NodeXL.ExcelTemplate.SubgraphLevelsControl();
            this.grpThumbnailSize = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.nudThumbnailWidthPx = new System.Windows.Forms.NumericUpDown();
            this.nudThumbnailHeightPx = new System.Windows.Forms.NumericUpDown();
            this.chkSelectIncidentEdges = new System.Windows.Forms.CheckBox();
            this.chkSelectVertex = new System.Windows.Forms.CheckBox();
            this.pnlSaveToFolder = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.cbxImageFormat = new Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus();
            this.txbFolder = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.grpImageSize = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nudImageWidthPx = new System.Windows.Forms.NumericUpDown();
            this.nudImageHeightPx = new System.Windows.Forms.NumericUpDown();
            this.chkSaveToFolder = new System.Windows.Forms.CheckBox();
            this.lnkHelp = new System.Windows.Forms.LinkLabel();
            this.chkSelectedVerticesOnly = new System.Windows.Forms.CheckBox();
            this.chkInsertThumbnails = new System.Windows.Forms.CheckBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pnlDisableWhileCreating.SuspendLayout();
            this.grpThumbnailSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudThumbnailWidthPx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThumbnailHeightPx)).BeginInit();
            this.pnlSaveToFolder.SuspendLayout();
            this.grpImageSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudImageWidthPx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudImageHeightPx)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(400, 429);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 1;
            this.btnCreate.Text = "[Set in code]";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(481, 429);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // pnlDisableWhileCreating
            // 
            this.pnlDisableWhileCreating.Controls.Add(this.usrSubgraphLevels);
            this.pnlDisableWhileCreating.Controls.Add(this.grpThumbnailSize);
            this.pnlDisableWhileCreating.Controls.Add(this.chkSelectIncidentEdges);
            this.pnlDisableWhileCreating.Controls.Add(this.chkSelectVertex);
            this.pnlDisableWhileCreating.Controls.Add(this.pnlSaveToFolder);
            this.pnlDisableWhileCreating.Controls.Add(this.chkSaveToFolder);
            this.pnlDisableWhileCreating.Controls.Add(this.lnkHelp);
            this.pnlDisableWhileCreating.Controls.Add(this.chkSelectedVerticesOnly);
            this.pnlDisableWhileCreating.Controls.Add(this.chkInsertThumbnails);
            this.pnlDisableWhileCreating.Location = new System.Drawing.Point(8, 5);
            this.pnlDisableWhileCreating.Name = "pnlDisableWhileCreating";
            this.pnlDisableWhileCreating.Size = new System.Drawing.Size(551, 414);
            this.pnlDisableWhileCreating.TabIndex = 0;
            // 
            // usrSubgraphLevels
            // 
            this.usrSubgraphLevels.Levels = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.usrSubgraphLevels.Location = new System.Drawing.Point(3, 0);
            this.usrSubgraphLevels.Name = "usrSubgraphLevels";
            this.usrSubgraphLevels.Size = new System.Drawing.Size(319, 53);
            this.usrSubgraphLevels.TabIndex = 0;
            // 
            // grpThumbnailSize
            // 
            this.grpThumbnailSize.Controls.Add(this.label5);
            this.grpThumbnailSize.Controls.Add(this.label6);
            this.grpThumbnailSize.Controls.Add(this.nudThumbnailWidthPx);
            this.grpThumbnailSize.Controls.Add(this.nudThumbnailHeightPx);
            this.grpThumbnailSize.Location = new System.Drawing.Point(22, 242);
            this.grpThumbnailSize.Name = "grpThumbnailSize";
            this.grpThumbnailSize.Size = new System.Drawing.Size(169, 88);
            this.grpThumbnailSize.TabIndex = 5;
            this.grpThumbnailSize.TabStop = false;
            this.grpThumbnailSize.Text = "Thumbnail size (pixels)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Wi&dth:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Hei&ght:";
            // 
            // nudThumbnailWidthPx
            // 
            this.nudThumbnailWidthPx.Location = new System.Drawing.Point(68, 22);
            this.nudThumbnailWidthPx.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.nudThumbnailWidthPx.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudThumbnailWidthPx.Name = "nudThumbnailWidthPx";
            this.nudThumbnailWidthPx.Size = new System.Drawing.Size(82, 20);
            this.nudThumbnailWidthPx.TabIndex = 1;
            this.nudThumbnailWidthPx.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // nudThumbnailHeightPx
            // 
            this.nudThumbnailHeightPx.Location = new System.Drawing.Point(68, 53);
            this.nudThumbnailHeightPx.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.nudThumbnailHeightPx.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudThumbnailHeightPx.Name = "nudThumbnailHeightPx";
            this.nudThumbnailHeightPx.Size = new System.Drawing.Size(82, 20);
            this.nudThumbnailHeightPx.TabIndex = 3;
            this.nudThumbnailHeightPx.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // chkSelectIncidentEdges
            // 
            this.chkSelectIncidentEdges.AutoSize = true;
            this.chkSelectIncidentEdges.Location = new System.Drawing.Point(3, 388);
            this.chkSelectIncidentEdges.Name = "chkSelectIncidentEdges";
            this.chkSelectIncidentEdges.Size = new System.Drawing.Size(311, 17);
            this.chkSelectIncidentEdges.TabIndex = 8;
            this.chkSelectIncidentEdges.Text = "In e&ach vertex\'s subgraph, select the vertex\'s incident edges";
            this.chkSelectIncidentEdges.UseVisualStyleBackColor = true;
            // 
            // chkSelectVertex
            // 
            this.chkSelectVertex.AutoSize = true;
            this.chkSelectVertex.Location = new System.Drawing.Point(3, 365);
            this.chkSelectVertex.Name = "chkSelectVertex";
            this.chkSelectVertex.Size = new System.Drawing.Size(232, 17);
            this.chkSelectVertex.TabIndex = 7;
            this.chkSelectVertex.Text = "I&n each vertex\'s subgraph, select the vertex";
            this.chkSelectVertex.UseVisualStyleBackColor = true;
            // 
            // pnlSaveToFolder
            // 
            this.pnlSaveToFolder.Controls.Add(this.label4);
            this.pnlSaveToFolder.Controls.Add(this.cbxImageFormat);
            this.pnlSaveToFolder.Controls.Add(this.txbFolder);
            this.pnlSaveToFolder.Controls.Add(this.btnBrowse);
            this.pnlSaveToFolder.Controls.Add(this.grpImageSize);
            this.pnlSaveToFolder.Enabled = false;
            this.pnlSaveToFolder.Location = new System.Drawing.Point(0, 87);
            this.pnlSaveToFolder.Name = "pnlSaveToFolder";
            this.pnlSaveToFolder.Size = new System.Drawing.Size(551, 123);
            this.pnlSaveToFolder.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(204, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Image &format:";
            // 
            // cbxImageFormat
            // 
            this.cbxImageFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxImageFormat.FormattingEnabled = true;
            this.cbxImageFormat.Location = new System.Drawing.Point(207, 54);
            this.cbxImageFormat.Name = "cbxImageFormat";
            this.cbxImageFormat.Size = new System.Drawing.Size(88, 21);
            this.cbxImageFormat.TabIndex = 4;
            // 
            // txbFolder
            // 
            this.txbFolder.Location = new System.Drawing.Point(22, 4);
            this.txbFolder.MaxLength = 247;
            this.txbFolder.Name = "txbFolder";
            this.txbFolder.Size = new System.Drawing.Size(442, 20);
            this.txbFolder.TabIndex = 0;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(470, 1);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "&Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // grpImageSize
            // 
            this.grpImageSize.Controls.Add(this.label2);
            this.grpImageSize.Controls.Add(this.label3);
            this.grpImageSize.Controls.Add(this.nudImageWidthPx);
            this.grpImageSize.Controls.Add(this.nudImageHeightPx);
            this.grpImageSize.Location = new System.Drawing.Point(22, 34);
            this.grpImageSize.Name = "grpImageSize";
            this.grpImageSize.Size = new System.Drawing.Size(169, 88);
            this.grpImageSize.TabIndex = 2;
            this.grpImageSize.TabStop = false;
            this.grpImageSize.Text = "Image size (pixels)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "&Width:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "&Height:";
            // 
            // nudImageWidthPx
            // 
            this.nudImageWidthPx.Location = new System.Drawing.Point(68, 22);
            this.nudImageWidthPx.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudImageWidthPx.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudImageWidthPx.Name = "nudImageWidthPx";
            this.nudImageWidthPx.Size = new System.Drawing.Size(82, 20);
            this.nudImageWidthPx.TabIndex = 1;
            this.nudImageWidthPx.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // nudImageHeightPx
            // 
            this.nudImageHeightPx.Location = new System.Drawing.Point(68, 53);
            this.nudImageHeightPx.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudImageHeightPx.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudImageHeightPx.Name = "nudImageHeightPx";
            this.nudImageHeightPx.Size = new System.Drawing.Size(82, 20);
            this.nudImageHeightPx.TabIndex = 3;
            this.nudImageHeightPx.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // chkSaveToFolder
            // 
            this.chkSaveToFolder.AutoSize = true;
            this.chkSaveToFolder.Location = new System.Drawing.Point(3, 66);
            this.chkSaveToFolder.Name = "chkSaveToFolder";
            this.chkSaveToFolder.Size = new System.Drawing.Size(178, 17);
            this.chkSaveToFolder.TabIndex = 2;
            this.chkSaveToFolder.Text = "&Save subgraph images to folder:";
            this.chkSaveToFolder.UseVisualStyleBackColor = true;
            this.chkSaveToFolder.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // lnkHelp
            // 
            this.lnkHelp.Location = new System.Drawing.Point(353, 11);
            this.lnkHelp.Name = "lnkHelp";
            this.lnkHelp.Size = new System.Drawing.Size(195, 13);
            this.lnkHelp.TabIndex = 1;
            this.lnkHelp.TabStop = true;
            this.lnkHelp.Text = "How subgraph images are created";
            this.lnkHelp.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lnkHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelp_LinkClicked);
            // 
            // chkSelectedVerticesOnly
            // 
            this.chkSelectedVerticesOnly.AutoSize = true;
            this.chkSelectedVerticesOnly.Location = new System.Drawing.Point(3, 342);
            this.chkSelectedVerticesOnly.Name = "chkSelectedVerticesOnly";
            this.chkSelectedVerticesOnly.Size = new System.Drawing.Size(260, 17);
            this.chkSelectedVerticesOnly.TabIndex = 6;
            this.chkSelectedVerticesOnly.Text = "&Create subgraph images for selected vertices only";
            this.chkSelectedVerticesOnly.UseVisualStyleBackColor = true;
            // 
            // chkInsertThumbnails
            // 
            this.chkInsertThumbnails.AutoSize = true;
            this.chkInsertThumbnails.Location = new System.Drawing.Point(3, 216);
            this.chkInsertThumbnails.Name = "chkInsertThumbnails";
            this.chkInsertThumbnails.Size = new System.Drawing.Size(349, 17);
            this.chkInsertThumbnails.TabIndex = 4;
            this.chkInsertThumbnails.Text = "&Insert subgraph thumbnails into the Vertices worksheet (can be slow)";
            this.chkInsertThumbnails.UseVisualStyleBackColor = true;
            this.chkInsertThumbnails.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoEllipsis = true;
            this.lblStatus.Location = new System.Drawing.Point(11, 433);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(370, 13);
            this.lblStatus.TabIndex = 3;
            // 
            // CreateSubgraphImagesDialog
            // 
            this.AcceptButton = this.btnCreate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(568, 465);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pnlDisableWhileCreating);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCreate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateSubgraphImagesDialog";
            this.Text = "Create Subgraph Images";
            this.pnlDisableWhileCreating.ResumeLayout(false);
            this.pnlDisableWhileCreating.PerformLayout();
            this.grpThumbnailSize.ResumeLayout(false);
            this.grpThumbnailSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudThumbnailWidthPx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThumbnailHeightPx)).EndInit();
            this.pnlSaveToFolder.ResumeLayout(false);
            this.pnlSaveToFolder.PerformLayout();
            this.grpImageSize.ResumeLayout(false);
            this.grpImageSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudImageWidthPx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudImageHeightPx)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlDisableWhileCreating;
        private System.Windows.Forms.LinkLabel lnkHelp;
        private System.Windows.Forms.CheckBox chkSaveToFolder;
        private System.Windows.Forms.CheckBox chkSelectedVerticesOnly;
        private System.Windows.Forms.CheckBox chkInsertThumbnails;
        private System.Windows.Forms.NumericUpDown nudImageWidthPx;
        private System.Windows.Forms.NumericUpDown nudImageHeightPx;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbFolder;
        private System.Windows.Forms.GroupBox grpImageSize;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblStatus;
        private Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus cbxImageFormat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlSaveToFolder;
        private System.Windows.Forms.CheckBox chkSelectIncidentEdges;
        private System.Windows.Forms.CheckBox chkSelectVertex;
        private System.Windows.Forms.GroupBox grpThumbnailSize;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudThumbnailWidthPx;
        private System.Windows.Forms.NumericUpDown nudThumbnailHeightPx;
        private SubgraphLevelsControl usrSubgraphLevels;
    }
}
