

//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class ImportFromMatrixWorkbookDialog
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lbxSourceWorkbook = new Microsoft.Research.CommunityTechnologies.AppLib.ExcelWorkbookListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radAssignVertexNames = new System.Windows.Forms.RadioButton();
            this.radSourceWorkbookHasVertexNames = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radUndirected = new System.Windows.Forms.RadioButton();
            this.radDirected = new System.Windows.Forms.RadioButton();
            this.lnkMatrixWorkbookSamples = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(243, 429);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(162, 429);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "Import";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lbxSourceWorkbook
            // 
            this.lbxSourceWorkbook.FormattingEnabled = true;
            this.lbxSourceWorkbook.Location = new System.Drawing.Point(15, 101);
            this.lbxSourceWorkbook.Name = "lbxSourceWorkbook";
            this.lbxSourceWorkbook.Size = new System.Drawing.Size(303, 69);
            this.lbxSourceWorkbook.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(214, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "&Open matrix workbook to import edges from:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(306, 43);
            this.label1.TabIndex = 0;
            this.label1.Text = "Use this to import edges from an open workbook that contains a graph represented " +
                "as an adjacency matrix.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radAssignVertexNames);
            this.groupBox1.Controls.Add(this.radSourceWorkbookHasVertexNames);
            this.groupBox1.Location = new System.Drawing.Point(15, 188);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(303, 112);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Vertex names";
            // 
            // radAssignVertexNames
            // 
            this.radAssignVertexNames.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radAssignVertexNames.Location = new System.Drawing.Point(18, 65);
            this.radAssignVertexNames.Name = "radAssignVertexNames";
            this.radAssignVertexNames.Size = new System.Drawing.Size(279, 41);
            this.radAssignVertexNames.TabIndex = 1;
            this.radAssignVertexNames.TabStop = true;
            this.radAssignVertexNames.Text = "The matrix workbook does &not have vertex names, so names will be assigned during" +
                " importation\r\n";
            this.radAssignVertexNames.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radAssignVertexNames.UseVisualStyleBackColor = true;
            // 
            // radSourceWorkbookHasVertexNames
            // 
            this.radSourceWorkbookHasVertexNames.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radSourceWorkbookHasVertexNames.Location = new System.Drawing.Point(18, 20);
            this.radSourceWorkbookHasVertexNames.Name = "radSourceWorkbookHasVertexNames";
            this.radSourceWorkbookHasVertexNames.Size = new System.Drawing.Size(279, 39);
            this.radSourceWorkbookHasVertexNames.TabIndex = 0;
            this.radSourceWorkbookHasVertexNames.TabStop = true;
            this.radSourceWorkbookHasVertexNames.Text = "The matrix workbook &has vertex names in row 1 and column A";
            this.radSourceWorkbookHasVertexNames.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radSourceWorkbookHasVertexNames.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radUndirected);
            this.groupBox2.Controls.Add(this.radDirected);
            this.groupBox2.Location = new System.Drawing.Point(15, 312);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(303, 100);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Graph type";
            // 
            // radUndirected
            // 
            this.radUndirected.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radUndirected.Location = new System.Drawing.Point(18, 49);
            this.radUndirected.Name = "radUndirected";
            this.radUndirected.Size = new System.Drawing.Size(279, 48);
            this.radUndirected.TabIndex = 1;
            this.radUndirected.TabStop = true;
            this.radUndirected.Text = "The matrix workbook represents an &undirected graph, so the matrix is symmetric";
            this.radUndirected.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radUndirected.UseVisualStyleBackColor = true;
            // 
            // radDirected
            // 
            this.radDirected.AutoSize = true;
            this.radDirected.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radDirected.Location = new System.Drawing.Point(18, 19);
            this.radDirected.Name = "radDirected";
            this.radDirected.Size = new System.Drawing.Size(256, 17);
            this.radDirected.TabIndex = 0;
            this.radDirected.TabStop = true;
            this.radDirected.Text = "The matrix workbook represents a &directed graph";
            this.radDirected.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radDirected.UseVisualStyleBackColor = true;
            // 
            // lnkMatrixWorkbookSamples
            // 
            this.lnkMatrixWorkbookSamples.AutoSize = true;
            this.lnkMatrixWorkbookSamples.Location = new System.Drawing.Point(12, 52);
            this.lnkMatrixWorkbookSamples.Name = "lnkMatrixWorkbookSamples";
            this.lnkMatrixWorkbookSamples.Size = new System.Drawing.Size(169, 13);
            this.lnkMatrixWorkbookSamples.TabIndex = 7;
            this.lnkMatrixWorkbookSamples.TabStop = true;
            this.lnkMatrixWorkbookSamples.Text = "What a matrix workbook looks like";
            this.lnkMatrixWorkbookSamples.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkMatrixWorkbookSamples_LinkClicked);
            // 
            // ImportFromMatrixWorkbookDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(334, 471);
            this.Controls.Add(this.lnkMatrixWorkbookSamples);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbxSourceWorkbook);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportFromMatrixWorkbookDialog";
            this.Text = "Import from Open Matrix Workbook";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private Microsoft.Research.CommunityTechnologies.AppLib.ExcelWorkbookListBox lbxSourceWorkbook;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radAssignVertexNames;
        private System.Windows.Forms.RadioButton radSourceWorkbookHasVertexNames;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radUndirected;
        private System.Windows.Forms.RadioButton radDirected;
        private System.Windows.Forms.LinkLabel lnkMatrixWorkbookSamples;
    }
}
