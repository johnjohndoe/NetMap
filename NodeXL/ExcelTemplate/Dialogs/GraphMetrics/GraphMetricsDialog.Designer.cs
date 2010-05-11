

//  Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class GraphMetricsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphMetricsDialog));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.btnUncheckAll = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkBrandesFastCentralities = new System.Windows.Forms.CheckBox();
            this.lnkBrandesFastCentralities = new System.Windows.Forms.LinkLabel();
            this.lnkEigenvectorCentrality = new System.Windows.Forms.LinkLabel();
            this.chkEigenvectorCentrality = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lnkDegree = new System.Windows.Forms.LinkLabel();
            this.chkDegree = new System.Windows.Forms.CheckBox();
            this.lnkOutDegree = new System.Windows.Forms.LinkLabel();
            this.chkOutDegree = new System.Windows.Forms.CheckBox();
            this.lnkInDegree = new System.Windows.Forms.LinkLabel();
            this.chkInDegree = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkOverallMetrics = new System.Windows.Forms.CheckBox();
            this.lnkOverallMetrics = new System.Windows.Forms.LinkLabel();
            this.chkClusteringCoefficient = new System.Windows.Forms.CheckBox();
            this.lnkClusteringCoefficient = new System.Windows.Forms.LinkLabel();
            this.chkPageRank = new System.Windows.Forms.CheckBox();
            this.lnkPageRank = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(183, 398);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(71, 398);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(106, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "Compute Metrics";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(251, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select the metrics to compute and insert into the workbook:";
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Location = new System.Drawing.Point(15, 356);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(75, 23);
            this.btnCheckAll.TabIndex = 4;
            this.btnCheckAll.Text = "&Select All";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // btnUncheckAll
            // 
            this.btnUncheckAll.Location = new System.Drawing.Point(96, 356);
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.Size = new System.Drawing.Size(75, 23);
            this.btnUncheckAll.TabIndex = 5;
            this.btnUncheckAll.Text = "&Deselect All";
            this.btnUncheckAll.UseVisualStyleBackColor = true;
            this.btnUncheckAll.Click += new System.EventHandler(this.btnUncheckAll_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkBrandesFastCentralities);
            this.groupBox1.Controls.Add(this.lnkBrandesFastCentralities);
            this.groupBox1.Controls.Add(this.lnkEigenvectorCentrality);
            this.groupBox1.Controls.Add(this.chkEigenvectorCentrality);
            this.groupBox1.Location = new System.Drawing.Point(15, 150);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 88);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Centrality";
            // 
            // chkBrandesFastCentralities
            // 
            this.chkBrandesFastCentralities.Location = new System.Drawing.Point(13, 14);
            this.chkBrandesFastCentralities.Name = "chkBrandesFastCentralities";
            this.chkBrandesFastCentralities.Size = new System.Drawing.Size(174, 39);
            this.chkBrandesFastCentralities.TabIndex = 0;
            this.chkBrandesFastCentralities.Text = "&Betweenness and closeness centralities";
            this.chkBrandesFastCentralities.UseVisualStyleBackColor = true;
            // 
            // lnkBrandesFastCentralities
            // 
            this.lnkBrandesFastCentralities.AutoSize = true;
            this.lnkBrandesFastCentralities.Location = new System.Drawing.Point(193, 18);
            this.lnkBrandesFastCentralities.Name = "lnkBrandesFastCentralities";
            this.lnkBrandesFastCentralities.Size = new System.Drawing.Size(39, 13);
            this.lnkBrandesFastCentralities.TabIndex = 1;
            this.lnkBrandesFastCentralities.TabStop = true;
            this.lnkBrandesFastCentralities.Tag = resources.GetString("lnkBrandesFastCentralities.Tag");
            this.lnkBrandesFastCentralities.Text = "Details";
            this.lnkBrandesFastCentralities.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelpLink_LinkClicked);
            // 
            // lnkEigenvectorCentrality
            // 
            this.lnkEigenvectorCentrality.AutoSize = true;
            this.lnkEigenvectorCentrality.Location = new System.Drawing.Point(193, 60);
            this.lnkEigenvectorCentrality.Name = "lnkEigenvectorCentrality";
            this.lnkEigenvectorCentrality.Size = new System.Drawing.Size(39, 13);
            this.lnkEigenvectorCentrality.TabIndex = 3;
            this.lnkEigenvectorCentrality.TabStop = true;
            this.lnkEigenvectorCentrality.Tag = resources.GetString("lnkEigenvectorCentrality.Tag");
            this.lnkEigenvectorCentrality.Text = "Details";
            this.lnkEigenvectorCentrality.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelpLink_LinkClicked);
            // 
            // chkEigenvectorCentrality
            // 
            this.chkEigenvectorCentrality.AutoSize = true;
            this.chkEigenvectorCentrality.Location = new System.Drawing.Point(13, 59);
            this.chkEigenvectorCentrality.Name = "chkEigenvectorCentrality";
            this.chkEigenvectorCentrality.Size = new System.Drawing.Size(128, 17);
            this.chkEigenvectorCentrality.TabIndex = 2;
            this.chkEigenvectorCentrality.Text = "&Eigenvector centrality";
            this.chkEigenvectorCentrality.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lnkDegree);
            this.groupBox2.Controls.Add(this.chkDegree);
            this.groupBox2.Controls.Add(this.lnkOutDegree);
            this.groupBox2.Controls.Add(this.chkOutDegree);
            this.groupBox2.Controls.Add(this.lnkInDegree);
            this.groupBox2.Controls.Add(this.chkInDegree);
            this.groupBox2.Location = new System.Drawing.Point(15, 47);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(248, 96);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Degree";
            // 
            // lnkDegree
            // 
            this.lnkDegree.AutoSize = true;
            this.lnkDegree.Location = new System.Drawing.Point(193, 22);
            this.lnkDegree.Name = "lnkDegree";
            this.lnkDegree.Size = new System.Drawing.Size(39, 13);
            this.lnkDegree.TabIndex = 1;
            this.lnkDegree.TabStop = true;
            this.lnkDegree.Tag = resources.GetString("lnkDegree.Tag");
            this.lnkDegree.Text = "Details";
            this.lnkDegree.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelpLink_LinkClicked);
            // 
            // chkDegree
            // 
            this.chkDegree.AutoSize = true;
            this.chkDegree.Location = new System.Drawing.Point(13, 21);
            this.chkDegree.Name = "chkDegree";
            this.chkDegree.Size = new System.Drawing.Size(61, 17);
            this.chkDegree.TabIndex = 0;
            this.chkDegree.Text = "De&gree";
            this.chkDegree.UseVisualStyleBackColor = true;
            // 
            // lnkOutDegree
            // 
            this.lnkOutDegree.AutoSize = true;
            this.lnkOutDegree.Location = new System.Drawing.Point(193, 68);
            this.lnkOutDegree.Name = "lnkOutDegree";
            this.lnkOutDegree.Size = new System.Drawing.Size(39, 13);
            this.lnkOutDegree.TabIndex = 5;
            this.lnkOutDegree.TabStop = true;
            this.lnkOutDegree.Tag = resources.GetString("lnkOutDegree.Tag");
            this.lnkOutDegree.Text = "Details";
            this.lnkOutDegree.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelpLink_LinkClicked);
            // 
            // chkOutDegree
            // 
            this.chkOutDegree.AutoSize = true;
            this.chkOutDegree.Location = new System.Drawing.Point(13, 67);
            this.chkOutDegree.Name = "chkOutDegree";
            this.chkOutDegree.Size = new System.Drawing.Size(81, 17);
            this.chkOutDegree.TabIndex = 4;
            this.chkOutDegree.Text = "&Out-Degree";
            this.chkOutDegree.UseVisualStyleBackColor = true;
            // 
            // lnkInDegree
            // 
            this.lnkInDegree.AutoSize = true;
            this.lnkInDegree.Location = new System.Drawing.Point(193, 45);
            this.lnkInDegree.Name = "lnkInDegree";
            this.lnkInDegree.Size = new System.Drawing.Size(39, 13);
            this.lnkInDegree.TabIndex = 3;
            this.lnkInDegree.TabStop = true;
            this.lnkInDegree.Tag = resources.GetString("lnkInDegree.Tag");
            this.lnkInDegree.Text = "Details";
            this.lnkInDegree.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelpLink_LinkClicked);
            // 
            // chkInDegree
            // 
            this.chkInDegree.AutoSize = true;
            this.chkInDegree.Location = new System.Drawing.Point(13, 44);
            this.chkInDegree.Name = "chkInDegree";
            this.chkInDegree.Size = new System.Drawing.Size(73, 17);
            this.chkInDegree.TabIndex = 2;
            this.chkInDegree.Text = "&In-Degree";
            this.chkInDegree.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lnkPageRank);
            this.groupBox3.Controls.Add(this.chkPageRank);
            this.groupBox3.Controls.Add(this.chkOverallMetrics);
            this.groupBox3.Controls.Add(this.lnkOverallMetrics);
            this.groupBox3.Controls.Add(this.chkClusteringCoefficient);
            this.groupBox3.Controls.Add(this.lnkClusteringCoefficient);
            this.groupBox3.Location = new System.Drawing.Point(15, 245);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(248, 96);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Miscellaneous";
            // 
            // chkOverallMetrics
            // 
            this.chkOverallMetrics.AutoSize = true;
            this.chkOverallMetrics.Location = new System.Drawing.Point(13, 67);
            this.chkOverallMetrics.Name = "chkOverallMetrics";
            this.chkOverallMetrics.Size = new System.Drawing.Size(95, 17);
            this.chkOverallMetrics.TabIndex = 4;
            this.chkOverallMetrics.Text = "Overall &metrics";
            this.chkOverallMetrics.UseVisualStyleBackColor = true;
            // 
            // lnkOverallMetrics
            // 
            this.lnkOverallMetrics.AutoSize = true;
            this.lnkOverallMetrics.Location = new System.Drawing.Point(193, 68);
            this.lnkOverallMetrics.Name = "lnkOverallMetrics";
            this.lnkOverallMetrics.Size = new System.Drawing.Size(39, 13);
            this.lnkOverallMetrics.TabIndex = 5;
            this.lnkOverallMetrics.TabStop = true;
            this.lnkOverallMetrics.Tag = "Overall metrics include vertex counts, edge counts, geodesic distances, and graph" +
                " density.  They get inserted into the Overall Metrics worksheet.";
            this.lnkOverallMetrics.Text = "Details";
            this.lnkOverallMetrics.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelpLink_LinkClicked);
            // 
            // chkClusteringCoefficient
            // 
            this.chkClusteringCoefficient.AutoSize = true;
            this.chkClusteringCoefficient.Location = new System.Drawing.Point(13, 44);
            this.chkClusteringCoefficient.Name = "chkClusteringCoefficient";
            this.chkClusteringCoefficient.Size = new System.Drawing.Size(124, 17);
            this.chkClusteringCoefficient.TabIndex = 2;
            this.chkClusteringCoefficient.Text = "Cl&ustering coefficient";
            this.chkClusteringCoefficient.UseVisualStyleBackColor = true;
            // 
            // lnkClusteringCoefficient
            // 
            this.lnkClusteringCoefficient.AutoSize = true;
            this.lnkClusteringCoefficient.Location = new System.Drawing.Point(193, 45);
            this.lnkClusteringCoefficient.Name = "lnkClusteringCoefficient";
            this.lnkClusteringCoefficient.Size = new System.Drawing.Size(39, 13);
            this.lnkClusteringCoefficient.TabIndex = 3;
            this.lnkClusteringCoefficient.TabStop = true;
            this.lnkClusteringCoefficient.Tag = resources.GetString("lnkClusteringCoefficient.Tag");
            this.lnkClusteringCoefficient.Text = "Details";
            this.lnkClusteringCoefficient.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelpLink_LinkClicked);
            // 
            // chkPageRank
            // 
            this.chkPageRank.AutoSize = true;
            this.chkPageRank.Location = new System.Drawing.Point(13, 21);
            this.chkPageRank.Name = "chkPageRank";
            this.chkPageRank.Size = new System.Drawing.Size(77, 17);
            this.chkPageRank.TabIndex = 0;
            this.chkPageRank.Text = "&PageRank";
            this.chkPageRank.UseVisualStyleBackColor = true;
            // 
            // lnkPageRank
            // 
            this.lnkPageRank.AutoSize = true;
            this.lnkPageRank.Location = new System.Drawing.Point(193, 22);
            this.lnkPageRank.Name = "lnkPageRank";
            this.lnkPageRank.Size = new System.Drawing.Size(39, 13);
            this.lnkPageRank.TabIndex = 1;
            this.lnkPageRank.TabStop = true;
            this.lnkPageRank.Tag = resources.GetString("lnkPageRank.Tag");
            this.lnkPageRank.Text = "Details";
            this.lnkPageRank.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelpLink_LinkClicked);
            // 
            // 
            // GraphMetricsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(280, 440);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnUncheckAll);
            this.Controls.Add(this.btnCheckAll);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GraphMetricsDialog";
            this.Text = "Graph Metrics";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.Button btnUncheckAll;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.LinkLabel lnkBrandesFastCentralities;
        private System.Windows.Forms.LinkLabel lnkEigenvectorCentrality;
        private System.Windows.Forms.CheckBox chkEigenvectorCentrality;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.LinkLabel lnkOutDegree;
        private System.Windows.Forms.CheckBox chkOutDegree;
        private System.Windows.Forms.LinkLabel lnkInDegree;
        private System.Windows.Forms.CheckBox chkInDegree;
        private System.Windows.Forms.LinkLabel lnkDegree;
        private System.Windows.Forms.CheckBox chkDegree;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkOverallMetrics;
        private System.Windows.Forms.LinkLabel lnkOverallMetrics;
        private System.Windows.Forms.CheckBox chkClusteringCoefficient;
        private System.Windows.Forms.LinkLabel lnkClusteringCoefficient;
        private System.Windows.Forms.CheckBox chkBrandesFastCentralities;
        private System.Windows.Forms.CheckBox chkPageRank;
        private System.Windows.Forms.LinkLabel lnkPageRank;
    }
}
