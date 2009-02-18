

//  Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class GraphMetricUserSettingsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphMetricUserSettingsDialog));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.btnUncheckAll = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lnkClosenessCentrality = new System.Windows.Forms.LinkLabel();
            this.cbxBetweennessCentrality = new System.Windows.Forms.CheckBox();
            this.lnkBetweennessCentrality = new System.Windows.Forms.LinkLabel();
            this.cbxClosenessCentrality = new System.Windows.Forms.CheckBox();
            this.lnkEigenvectorCentrality = new System.Windows.Forms.LinkLabel();
            this.cbxEigenvectorCentrality = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lnkDegree = new System.Windows.Forms.LinkLabel();
            this.cbxDegree = new System.Windows.Forms.CheckBox();
            this.lnkOutDegree = new System.Windows.Forms.LinkLabel();
            this.cbxOutDegree = new System.Windows.Forms.CheckBox();
            this.lnkInDegree = new System.Windows.Forms.LinkLabel();
            this.cbxInDegree = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbxOverallMetrics = new System.Windows.Forms.CheckBox();
            this.lnkOverallMetrics = new System.Windows.Forms.LinkLabel();
            this.cbxClusteringCoefficient = new System.Windows.Forms.CheckBox();
            this.lnkClusteringCoefficient = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(269, 342);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(183, 342);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(337, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "&When the Calculate Graph Metrics button is clicked, calculate these metrics and " +
                "insert the results into the workbook:";
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Location = new System.Drawing.Point(274, 71);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(75, 23);
            this.btnCheckAll.TabIndex = 4;
            this.btnCheckAll.Text = "&Select All";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // btnUncheckAll
            // 
            this.btnUncheckAll.Location = new System.Drawing.Point(274, 100);
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.Size = new System.Drawing.Size(75, 23);
            this.btnUncheckAll.TabIndex = 5;
            this.btnUncheckAll.Text = "&Deselect All";
            this.btnUncheckAll.UseVisualStyleBackColor = true;
            this.btnUncheckAll.Click += new System.EventHandler(this.btnUncheckAll_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lnkClosenessCentrality);
            this.groupBox1.Controls.Add(this.cbxBetweennessCentrality);
            this.groupBox1.Controls.Add(this.lnkBetweennessCentrality);
            this.groupBox1.Controls.Add(this.cbxClosenessCentrality);
            this.groupBox1.Controls.Add(this.lnkEigenvectorCentrality);
            this.groupBox1.Controls.Add(this.cbxEigenvectorCentrality);
            this.groupBox1.Location = new System.Drawing.Point(15, 153);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 96);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Centrality";
            // 
            // lnkClosenessCentrality
            // 
            this.lnkClosenessCentrality.AutoSize = true;
            this.lnkClosenessCentrality.Location = new System.Drawing.Point(193, 45);
            this.lnkClosenessCentrality.Name = "lnkClosenessCentrality";
            this.lnkClosenessCentrality.Size = new System.Drawing.Size(39, 13);
            this.lnkClosenessCentrality.TabIndex = 3;
            this.lnkClosenessCentrality.TabStop = true;
            this.lnkClosenessCentrality.Tag = resources.GetString("lnkClosenessCentrality.Tag");
            this.lnkClosenessCentrality.Text = "Details";
            this.lnkClosenessCentrality.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelpLink_LinkClicked);
            // 
            // cbxBetweennessCentrality
            // 
            this.cbxBetweennessCentrality.AutoSize = true;
            this.cbxBetweennessCentrality.Location = new System.Drawing.Point(13, 21);
            this.cbxBetweennessCentrality.Name = "cbxBetweennessCentrality";
            this.cbxBetweennessCentrality.Size = new System.Drawing.Size(168, 17);
            this.cbxBetweennessCentrality.TabIndex = 0;
            this.cbxBetweennessCentrality.Text = "&Betweenness centrality  (slow)";
            this.cbxBetweennessCentrality.UseVisualStyleBackColor = true;
            // 
            // lnkBetweennessCentrality
            // 
            this.lnkBetweennessCentrality.AutoSize = true;
            this.lnkBetweennessCentrality.Location = new System.Drawing.Point(193, 22);
            this.lnkBetweennessCentrality.Name = "lnkBetweennessCentrality";
            this.lnkBetweennessCentrality.Size = new System.Drawing.Size(39, 13);
            this.lnkBetweennessCentrality.TabIndex = 1;
            this.lnkBetweennessCentrality.TabStop = true;
            this.lnkBetweennessCentrality.Tag = resources.GetString("lnkBetweennessCentrality.Tag");
            this.lnkBetweennessCentrality.Text = "Details";
            this.lnkBetweennessCentrality.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelpLink_LinkClicked);
            // 
            // cbxClosenessCentrality
            // 
            this.cbxClosenessCentrality.AutoSize = true;
            this.cbxClosenessCentrality.Location = new System.Drawing.Point(13, 44);
            this.cbxClosenessCentrality.Name = "cbxClosenessCentrality";
            this.cbxClosenessCentrality.Size = new System.Drawing.Size(152, 17);
            this.cbxClosenessCentrality.TabIndex = 2;
            this.cbxClosenessCentrality.Text = "&Closeness centrality  (slow)";
            this.cbxClosenessCentrality.UseVisualStyleBackColor = true;
            // 
            // lnkEigenvectorCentrality
            // 
            this.lnkEigenvectorCentrality.AutoSize = true;
            this.lnkEigenvectorCentrality.Location = new System.Drawing.Point(193, 68);
            this.lnkEigenvectorCentrality.Name = "lnkEigenvectorCentrality";
            this.lnkEigenvectorCentrality.Size = new System.Drawing.Size(39, 13);
            this.lnkEigenvectorCentrality.TabIndex = 5;
            this.lnkEigenvectorCentrality.TabStop = true;
            this.lnkEigenvectorCentrality.Tag = resources.GetString("lnkEigenvectorCentrality.Tag");
            this.lnkEigenvectorCentrality.Text = "Details";
            this.lnkEigenvectorCentrality.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelpLink_LinkClicked);
            // 
            // cbxEigenvectorCentrality
            // 
            this.cbxEigenvectorCentrality.AutoSize = true;
            this.cbxEigenvectorCentrality.Location = new System.Drawing.Point(13, 67);
            this.cbxEigenvectorCentrality.Name = "cbxEigenvectorCentrality";
            this.cbxEigenvectorCentrality.Size = new System.Drawing.Size(128, 17);
            this.cbxEigenvectorCentrality.TabIndex = 4;
            this.cbxEigenvectorCentrality.Text = "&Eigenvector centrality";
            this.cbxEigenvectorCentrality.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lnkDegree);
            this.groupBox2.Controls.Add(this.cbxDegree);
            this.groupBox2.Controls.Add(this.lnkOutDegree);
            this.groupBox2.Controls.Add(this.cbxOutDegree);
            this.groupBox2.Controls.Add(this.lnkInDegree);
            this.groupBox2.Controls.Add(this.cbxInDegree);
            this.groupBox2.Location = new System.Drawing.Point(15, 50);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(248, 96);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Degree";
            // 
            // lnkDegree
            // 
            this.lnkDegree.AutoSize = true;
            this.lnkDegree.Location = new System.Drawing.Point(193, 68);
            this.lnkDegree.Name = "lnkDegree";
            this.lnkDegree.Size = new System.Drawing.Size(39, 13);
            this.lnkDegree.TabIndex = 5;
            this.lnkDegree.TabStop = true;
            this.lnkDegree.Tag = resources.GetString("lnkDegree.Tag");
            this.lnkDegree.Text = "Details";
            this.lnkDegree.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelpLink_LinkClicked);
            // 
            // cbxDegree
            // 
            this.cbxDegree.AutoSize = true;
            this.cbxDegree.Location = new System.Drawing.Point(13, 67);
            this.cbxDegree.Name = "cbxDegree";
            this.cbxDegree.Size = new System.Drawing.Size(61, 17);
            this.cbxDegree.TabIndex = 4;
            this.cbxDegree.Text = "De&gree";
            this.cbxDegree.UseVisualStyleBackColor = true;
            // 
            // lnkOutDegree
            // 
            this.lnkOutDegree.AutoSize = true;
            this.lnkOutDegree.Location = new System.Drawing.Point(193, 45);
            this.lnkOutDegree.Name = "lnkOutDegree";
            this.lnkOutDegree.Size = new System.Drawing.Size(39, 13);
            this.lnkOutDegree.TabIndex = 3;
            this.lnkOutDegree.TabStop = true;
            this.lnkOutDegree.Tag = resources.GetString("lnkOutDegree.Tag");
            this.lnkOutDegree.Text = "Details";
            this.lnkOutDegree.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelpLink_LinkClicked);
            // 
            // cbxOutDegree
            // 
            this.cbxOutDegree.AutoSize = true;
            this.cbxOutDegree.Location = new System.Drawing.Point(13, 44);
            this.cbxOutDegree.Name = "cbxOutDegree";
            this.cbxOutDegree.Size = new System.Drawing.Size(81, 17);
            this.cbxOutDegree.TabIndex = 2;
            this.cbxOutDegree.Text = "&Out-Degree";
            this.cbxOutDegree.UseVisualStyleBackColor = true;
            // 
            // lnkInDegree
            // 
            this.lnkInDegree.AutoSize = true;
            this.lnkInDegree.Location = new System.Drawing.Point(193, 22);
            this.lnkInDegree.Name = "lnkInDegree";
            this.lnkInDegree.Size = new System.Drawing.Size(39, 13);
            this.lnkInDegree.TabIndex = 1;
            this.lnkInDegree.TabStop = true;
            this.lnkInDegree.Tag = resources.GetString("lnkInDegree.Tag");
            this.lnkInDegree.Text = "Details";
            this.lnkInDegree.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelpLink_LinkClicked);
            // 
            // cbxInDegree
            // 
            this.cbxInDegree.AutoSize = true;
            this.cbxInDegree.Location = new System.Drawing.Point(13, 21);
            this.cbxInDegree.Name = "cbxInDegree";
            this.cbxInDegree.Size = new System.Drawing.Size(73, 17);
            this.cbxInDegree.TabIndex = 0;
            this.cbxInDegree.Text = "&In-Degree";
            this.cbxInDegree.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbxOverallMetrics);
            this.groupBox3.Controls.Add(this.lnkOverallMetrics);
            this.groupBox3.Controls.Add(this.cbxClusteringCoefficient);
            this.groupBox3.Controls.Add(this.lnkClusteringCoefficient);
            this.groupBox3.Location = new System.Drawing.Point(15, 255);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(248, 73);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Miscellaneous";
            // 
            // cbxOverallMetrics
            // 
            this.cbxOverallMetrics.AutoSize = true;
            this.cbxOverallMetrics.Location = new System.Drawing.Point(13, 44);
            this.cbxOverallMetrics.Name = "cbxOverallMetrics";
            this.cbxOverallMetrics.Size = new System.Drawing.Size(95, 17);
            this.cbxOverallMetrics.TabIndex = 2;
            this.cbxOverallMetrics.Text = "Overall &metrics";
            this.cbxOverallMetrics.UseVisualStyleBackColor = true;
            // 
            // lnkOverallMetrics
            // 
            this.lnkOverallMetrics.AutoSize = true;
            this.lnkOverallMetrics.Location = new System.Drawing.Point(193, 45);
            this.lnkOverallMetrics.Name = "lnkOverallMetrics";
            this.lnkOverallMetrics.Size = new System.Drawing.Size(39, 13);
            this.lnkOverallMetrics.TabIndex = 3;
            this.lnkOverallMetrics.TabStop = true;
            this.lnkOverallMetrics.Tag = resources.GetString("lnkOverallMetrics.Tag");
            this.lnkOverallMetrics.Text = "Details";
            this.lnkOverallMetrics.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelpLink_LinkClicked);
            // 
            // cbxClusteringCoefficient
            // 
            this.cbxClusteringCoefficient.AutoSize = true;
            this.cbxClusteringCoefficient.Location = new System.Drawing.Point(13, 21);
            this.cbxClusteringCoefficient.Name = "cbxClusteringCoefficient";
            this.cbxClusteringCoefficient.Size = new System.Drawing.Size(124, 17);
            this.cbxClusteringCoefficient.TabIndex = 0;
            this.cbxClusteringCoefficient.Text = "Cl&ustering coefficient";
            this.cbxClusteringCoefficient.UseVisualStyleBackColor = true;
            // 
            // lnkClusteringCoefficient
            // 
            this.lnkClusteringCoefficient.AutoSize = true;
            this.lnkClusteringCoefficient.Location = new System.Drawing.Point(193, 22);
            this.lnkClusteringCoefficient.Name = "lnkClusteringCoefficient";
            this.lnkClusteringCoefficient.Size = new System.Drawing.Size(39, 13);
            this.lnkClusteringCoefficient.TabIndex = 1;
            this.lnkClusteringCoefficient.TabStop = true;
            this.lnkClusteringCoefficient.Tag = resources.GetString("lnkClusteringCoefficient.Tag");
            this.lnkClusteringCoefficient.Text = "Details";
            this.lnkClusteringCoefficient.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelpLink_LinkClicked);
            // 
            // GraphMetricUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(364, 380);
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
            this.Name = "GraphMetricUserSettingsDialog";
            this.Text = "Select Graph Metrics";
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
        private System.Windows.Forms.LinkLabel lnkBetweennessCentrality;
        private System.Windows.Forms.CheckBox cbxClosenessCentrality;
        private System.Windows.Forms.LinkLabel lnkEigenvectorCentrality;
        private System.Windows.Forms.CheckBox cbxEigenvectorCentrality;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.LinkLabel lnkOutDegree;
        private System.Windows.Forms.CheckBox cbxOutDegree;
        private System.Windows.Forms.LinkLabel lnkInDegree;
        private System.Windows.Forms.CheckBox cbxInDegree;
        private System.Windows.Forms.LinkLabel lnkClosenessCentrality;
        private System.Windows.Forms.CheckBox cbxBetweennessCentrality;
        private System.Windows.Forms.LinkLabel lnkDegree;
        private System.Windows.Forms.CheckBox cbxDegree;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cbxOverallMetrics;
        private System.Windows.Forms.LinkLabel lnkOverallMetrics;
        private System.Windows.Forms.CheckBox cbxClusteringCoefficient;
        private System.Windows.Forms.LinkLabel lnkClusteringCoefficient;
    }
}
