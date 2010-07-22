
namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class AutomateTasksDialog
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
            this.components = new System.ComponentModel.Container();
            this.chkReadWorkbook = new System.Windows.Forms.CheckBox();
            this.chkMergeDuplicateEdges = new System.Windows.Forms.CheckBox();
            this.chkCalculateGraphMetrics = new System.Windows.Forms.CheckBox();
            this.chkAutoFillWorkbook = new System.Windows.Forms.CheckBox();
            this.chkCreateSubgraphImages = new System.Windows.Forms.CheckBox();
            this.chkCalculateClusters = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGraphMetricUserSettings = new System.Windows.Forms.Button();
            this.btnAutoFillUserSettings = new System.Windows.Forms.Button();
            this.btnCreateSubgraphImagesUserSettings = new System.Windows.Forms.Button();
            this.btnUncheckAll = new System.Windows.Forms.Button();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnGeneralUserSettings = new System.Windows.Forms.Button();
            this.btnSaveGraphImageFile = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkSaveGraphImageFile = new System.Windows.Forms.CheckBox();
            this.usrFolderToAutomate = new Microsoft.Research.CommunityTechnologies.AppLib.FolderPathControl();
            this.lblNote = new System.Windows.Forms.Label();
            this.radAutomateThisWorkbookOnly = new System.Windows.Forms.RadioButton();
            this.radAutomateFolder = new System.Windows.Forms.RadioButton();
            this.grpTasksToRun = new System.Windows.Forms.GroupBox();
            this.groupBox2.SuspendLayout();
            this.grpTasksToRun.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkReadWorkbook
            // 
            this.chkReadWorkbook.AutoSize = true;
            this.chkReadWorkbook.Checked = true;
            this.chkReadWorkbook.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReadWorkbook.Location = new System.Drawing.Point(17, 203);
            this.chkReadWorkbook.Name = "chkReadWorkbook";
            this.chkReadWorkbook.Size = new System.Drawing.Size(83, 17);
            this.chkReadWorkbook.TabIndex = 8;
            this.chkReadWorkbook.Tag = Microsoft.NodeXL.ExcelTemplate.AutomationTasks.ReadWorkbook;
            this.chkReadWorkbook.Text = "Show &graph";
            this.chkReadWorkbook.UseVisualStyleBackColor = true;
            // 
            // chkMergeDuplicateEdges
            // 
            this.chkMergeDuplicateEdges.AutoSize = true;
            this.chkMergeDuplicateEdges.Location = new System.Drawing.Point(18, 28);
            this.chkMergeDuplicateEdges.Name = "chkMergeDuplicateEdges";
            this.chkMergeDuplicateEdges.Size = new System.Drawing.Size(134, 17);
            this.chkMergeDuplicateEdges.TabIndex = 0;
            this.chkMergeDuplicateEdges.Tag = Microsoft.NodeXL.ExcelTemplate.AutomationTasks.MergeDuplicateEdges;
            this.chkMergeDuplicateEdges.Text = "&Merge duplicate edges";
            this.chkMergeDuplicateEdges.UseVisualStyleBackColor = true;
            // 
            // chkCalculateGraphMetrics
            // 
            this.chkCalculateGraphMetrics.AutoSize = true;
            this.chkCalculateGraphMetrics.Location = new System.Drawing.Point(18, 63);
            this.chkCalculateGraphMetrics.Name = "chkCalculateGraphMetrics";
            this.chkCalculateGraphMetrics.Size = new System.Drawing.Size(91, 17);
            this.chkCalculateGraphMetrics.TabIndex = 1;
            this.chkCalculateGraphMetrics.Tag = Microsoft.NodeXL.ExcelTemplate.AutomationTasks.CalculateGraphMetrics;
            this.chkCalculateGraphMetrics.Text = "Graph m&etrics";
            this.chkCalculateGraphMetrics.UseVisualStyleBackColor = true;
            // 
            // chkAutoFillWorkbook
            // 
            this.chkAutoFillWorkbook.AutoSize = true;
            this.chkAutoFillWorkbook.Location = new System.Drawing.Point(18, 98);
            this.chkAutoFillWorkbook.Name = "chkAutoFillWorkbook";
            this.chkAutoFillWorkbook.Size = new System.Drawing.Size(99, 17);
            this.chkAutoFillWorkbook.TabIndex = 3;
            this.chkAutoFillWorkbook.Tag = Microsoft.NodeXL.ExcelTemplate.AutomationTasks.AutoFillWorkbook;
            this.chkAutoFillWorkbook.Text = "&Autofill columns";
            this.chkAutoFillWorkbook.UseVisualStyleBackColor = true;
            // 
            // chkCreateSubgraphImages
            // 
            this.chkCreateSubgraphImages.AutoSize = true;
            this.chkCreateSubgraphImages.Location = new System.Drawing.Point(18, 133);
            this.chkCreateSubgraphImages.Name = "chkCreateSubgraphImages";
            this.chkCreateSubgraphImages.Size = new System.Drawing.Size(108, 17);
            this.chkCreateSubgraphImages.TabIndex = 5;
            this.chkCreateSubgraphImages.Tag = Microsoft.NodeXL.ExcelTemplate.AutomationTasks.CreateSubgraphImages;
            this.chkCreateSubgraphImages.Text = "Subgraph &images";
            this.chkCreateSubgraphImages.UseVisualStyleBackColor = true;
            // 
            // chkCalculateClusters
            // 
            this.chkCalculateClusters.AutoSize = true;
            this.chkCalculateClusters.Location = new System.Drawing.Point(17, 168);
            this.chkCalculateClusters.Name = "chkCalculateClusters";
            this.chkCalculateClusters.Size = new System.Drawing.Size(85, 17);
            this.chkCalculateClusters.TabIndex = 7;
            this.chkCalculateClusters.Tag = Microsoft.NodeXL.ExcelTemplate.AutomationTasks.CalculateClusters;
            this.chkCalculateClusters.Text = "Find &clusters";
            this.chkCalculateClusters.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(188, 441);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "Run";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(269, 441);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnGraphMetricUserSettings
            // 
            this.btnGraphMetricUserSettings.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGraphMetricUserSettings.Image = global::Microsoft.NodeXL.ExcelTemplate.Properties.Resources.Options;
            this.btnGraphMetricUserSettings.Location = new System.Drawing.Point(165, 59);
            this.btnGraphMetricUserSettings.Name = "btnGraphMetricUserSettings";
            this.btnGraphMetricUserSettings.Size = new System.Drawing.Size(23, 23);
            this.btnGraphMetricUserSettings.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnGraphMetricUserSettings, "Graph metrics options");
            this.btnGraphMetricUserSettings.UseVisualStyleBackColor = true;
            this.btnGraphMetricUserSettings.Click += new System.EventHandler(this.btnGraphMetricUserSettings_Click);
            // 
            // btnAutoFillUserSettings
            // 
            this.btnAutoFillUserSettings.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAutoFillUserSettings.Image = global::Microsoft.NodeXL.ExcelTemplate.Properties.Resources.Options;
            this.btnAutoFillUserSettings.Location = new System.Drawing.Point(165, 94);
            this.btnAutoFillUserSettings.Name = "btnAutoFillUserSettings";
            this.btnAutoFillUserSettings.Size = new System.Drawing.Size(23, 23);
            this.btnAutoFillUserSettings.TabIndex = 4;
            this.toolTip1.SetToolTip(this.btnAutoFillUserSettings, "Autofill columns options");
            this.btnAutoFillUserSettings.UseVisualStyleBackColor = true;
            this.btnAutoFillUserSettings.Click += new System.EventHandler(this.btnAutoFillUserSettings_Click);
            // 
            // btnCreateSubgraphImagesUserSettings
            // 
            this.btnCreateSubgraphImagesUserSettings.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateSubgraphImagesUserSettings.Image = global::Microsoft.NodeXL.ExcelTemplate.Properties.Resources.Options;
            this.btnCreateSubgraphImagesUserSettings.Location = new System.Drawing.Point(165, 129);
            this.btnCreateSubgraphImagesUserSettings.Name = "btnCreateSubgraphImagesUserSettings";
            this.btnCreateSubgraphImagesUserSettings.Size = new System.Drawing.Size(23, 23);
            this.btnCreateSubgraphImagesUserSettings.TabIndex = 6;
            this.toolTip1.SetToolTip(this.btnCreateSubgraphImagesUserSettings, "Subgraph images options");
            this.btnCreateSubgraphImagesUserSettings.UseVisualStyleBackColor = true;
            this.btnCreateSubgraphImagesUserSettings.Click += new System.EventHandler(this.btnCreateSubgraphImagesUserSettings_Click);
            // 
            // btnUncheckAll
            // 
            this.btnUncheckAll.Location = new System.Drawing.Point(98, 268);
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.Size = new System.Drawing.Size(75, 23);
            this.btnUncheckAll.TabIndex = 13;
            this.btnUncheckAll.Text = "&Deselect All";
            this.btnUncheckAll.UseVisualStyleBackColor = true;
            this.btnUncheckAll.Click += new System.EventHandler(this.btnUncheckAll_Click);
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Location = new System.Drawing.Point(17, 268);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(75, 23);
            this.btnCheckAll.TabIndex = 12;
            this.btnCheckAll.Text = "&Select All";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // btnGeneralUserSettings
            // 
            this.btnGeneralUserSettings.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGeneralUserSettings.Image = global::Microsoft.NodeXL.ExcelTemplate.Properties.Resources.Options;
            this.btnGeneralUserSettings.Location = new System.Drawing.Point(165, 199);
            this.btnGeneralUserSettings.Name = "btnGeneralUserSettings";
            this.btnGeneralUserSettings.Size = new System.Drawing.Size(23, 23);
            this.btnGeneralUserSettings.TabIndex = 9;
            this.toolTip1.SetToolTip(this.btnGeneralUserSettings, "Options that control the graph\'s appearance");
            this.btnGeneralUserSettings.UseVisualStyleBackColor = true;
            this.btnGeneralUserSettings.Click += new System.EventHandler(this.btnGeneralUserSettings_Click);
            // 
            // btnSaveGraphImageFile
            // 
            this.btnSaveGraphImageFile.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveGraphImageFile.Image = global::Microsoft.NodeXL.ExcelTemplate.Properties.Resources.Options;
            this.btnSaveGraphImageFile.Location = new System.Drawing.Point(165, 234);
            this.btnSaveGraphImageFile.Name = "btnSaveGraphImageFile";
            this.btnSaveGraphImageFile.Size = new System.Drawing.Size(23, 23);
            this.btnSaveGraphImageFile.TabIndex = 11;
            this.toolTip1.SetToolTip(this.btnSaveGraphImageFile, "Image Options");
            this.btnSaveGraphImageFile.UseVisualStyleBackColor = true;
            this.btnSaveGraphImageFile.Click += new System.EventHandler(this.btnSaveGraphImageFile_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.usrFolderToAutomate);
            this.groupBox2.Controls.Add(this.lblNote);
            this.groupBox2.Controls.Add(this.radAutomateThisWorkbookOnly);
            this.groupBox2.Controls.Add(this.radAutomateFolder);
            this.groupBox2.Location = new System.Drawing.Point(9, 316);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(335, 115);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // chkSaveGraphImageFile
            // 
            this.chkSaveGraphImageFile.AutoSize = true;
            this.chkSaveGraphImageFile.Location = new System.Drawing.Point(17, 238);
            this.chkSaveGraphImageFile.Name = "chkSaveGraphImageFile";
            this.chkSaveGraphImageFile.Size = new System.Drawing.Size(110, 17);
            this.chkSaveGraphImageFile.TabIndex = 10;
            this.chkSaveGraphImageFile.Tag = Microsoft.NodeXL.ExcelTemplate.AutomationTasks.SaveGraphImageFile;
            this.chkSaveGraphImageFile.Text = "Sa&ve image to file";
            this.chkSaveGraphImageFile.UseVisualStyleBackColor = true;
            // 
            // usrFolderToAutomate
            // 
            this.usrFolderToAutomate.BrowsePrompt = "Browse for a folder containing NodeXL workbooks.";
            this.usrFolderToAutomate.FolderPath = "";
            this.usrFolderToAutomate.Location = new System.Drawing.Point(36, 64);
            this.usrFolderToAutomate.Name = "usrFolderToAutomate";
            this.usrFolderToAutomate.Size = new System.Drawing.Size(290, 24);
            this.usrFolderToAutomate.TabIndex = 2;
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(36, 88);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(138, 13);
            this.lblNote.TabIndex = 3;
            this.lblNote.Text = "(Excludes open workbooks)";
            // 
            // radAutomateThisWorkbookOnly
            // 
            this.radAutomateThisWorkbookOnly.AutoSize = true;
            this.radAutomateThisWorkbookOnly.Location = new System.Drawing.Point(17, 16);
            this.radAutomateThisWorkbookOnly.Name = "radAutomateThisWorkbookOnly";
            this.radAutomateThisWorkbookOnly.Size = new System.Drawing.Size(108, 17);
            this.radAutomateThisWorkbookOnly.TabIndex = 0;
            this.radAutomateThisWorkbookOnly.TabStop = true;
            this.radAutomateThisWorkbookOnly.Text = "&On this workbook";
            this.radAutomateThisWorkbookOnly.UseVisualStyleBackColor = true;
            this.radAutomateThisWorkbookOnly.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // radAutomateFolder
            // 
            this.radAutomateFolder.AutoSize = true;
            this.radAutomateFolder.Location = new System.Drawing.Point(17, 41);
            this.radAutomateFolder.Name = "radAutomateFolder";
            this.radAutomateFolder.Size = new System.Drawing.Size(222, 17);
            this.radAutomateFolder.TabIndex = 1;
            this.radAutomateFolder.TabStop = true;
            this.radAutomateFolder.Text = "On every &NodeXL workbook in this folder:";
            this.radAutomateFolder.UseVisualStyleBackColor = true;
            this.radAutomateFolder.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // grpTasksToRun
            // 
            this.grpTasksToRun.Controls.Add(this.btnSaveGraphImageFile);
            this.grpTasksToRun.Controls.Add(this.btnGeneralUserSettings);
            this.grpTasksToRun.Controls.Add(this.chkSaveGraphImageFile);
            this.grpTasksToRun.Controls.Add(this.chkMergeDuplicateEdges);
            this.grpTasksToRun.Controls.Add(this.chkCalculateClusters);
            this.grpTasksToRun.Controls.Add(this.btnUncheckAll);
            this.grpTasksToRun.Controls.Add(this.chkCreateSubgraphImages);
            this.grpTasksToRun.Controls.Add(this.chkReadWorkbook);
            this.grpTasksToRun.Controls.Add(this.btnGraphMetricUserSettings);
            this.grpTasksToRun.Controls.Add(this.btnCheckAll);
            this.grpTasksToRun.Controls.Add(this.chkAutoFillWorkbook);
            this.grpTasksToRun.Controls.Add(this.btnAutoFillUserSettings);
            this.grpTasksToRun.Controls.Add(this.btnCreateSubgraphImagesUserSettings);
            this.grpTasksToRun.Controls.Add(this.chkCalculateGraphMetrics);
            this.grpTasksToRun.Location = new System.Drawing.Point(9, 9);
            this.grpTasksToRun.Name = "grpTasksToRun";
            this.grpTasksToRun.Size = new System.Drawing.Size(335, 302);
            this.grpTasksToRun.TabIndex = 0;
            this.grpTasksToRun.TabStop = false;
            this.grpTasksToRun.Text = "Run these tasks";
            // 
            // AutomateTasksDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(354, 476);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpTasksToRun);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutomateTasksDialog";
            this.Text = "Automate";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpTasksToRun.ResumeLayout(false);
            this.grpTasksToRun.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkReadWorkbook;
        private System.Windows.Forms.CheckBox chkMergeDuplicateEdges;
        private System.Windows.Forms.CheckBox chkCalculateGraphMetrics;
        private System.Windows.Forms.CheckBox chkAutoFillWorkbook;
        private System.Windows.Forms.CheckBox chkCreateSubgraphImages;
        private System.Windows.Forms.CheckBox chkCalculateClusters;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGraphMetricUserSettings;
        private System.Windows.Forms.Button btnAutoFillUserSettings;
        private System.Windows.Forms.Button btnCreateSubgraphImagesUserSettings;
        private System.Windows.Forms.Button btnUncheckAll;
        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.RadioButton radAutomateFolder;
        private System.Windows.Forms.RadioButton radAutomateThisWorkbookOnly;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.GroupBox grpTasksToRun;
        private System.Windows.Forms.GroupBox groupBox2;
        private Microsoft.Research.CommunityTechnologies.AppLib.FolderPathControl usrFolderToAutomate;
        private System.Windows.Forms.Button btnGeneralUserSettings;
        private System.Windows.Forms.CheckBox chkSaveGraphImageFile;
        private System.Windows.Forms.Button btnSaveGraphImageFile;
    }
}

