

//  Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class ImportFromEdgeWorkbookDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbxSourceWorkbook = new Microsoft.Research.CommunityTechnologies.AppLib.ExcelWorkbookListBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbxSourceColumnsHaveHeaders = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.clbSourceColumns = new System.Windows.Forms.CheckedListBox();
            this.lblVertex1 = new System.Windows.Forms.Label();
            this.cbxVertex1 = new System.Windows.Forms.ComboBox();
            this.cbxVertex2 = new System.Windows.Forms.ComboBox();
            this.lblVertex2 = new System.Windows.Forms.Label();
            this.pnlVertices = new System.Windows.Forms.Panel();
            this.btnUncheckAllSourceColumns = new System.Windows.Forms.Button();
            this.btnCheckAllSourceColumns = new System.Windows.Forms.Button();
            this.pnlVertices.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(306, 49);
            this.label1.TabIndex = 0;
            this.label1.Text = "Use this to import edges from an open workbook that contains an edge list.  At le" +
                "ast two columns must be imported, one for Vertex 1 and one for Vertex 2.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(179, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "&Open edge workbook to import from:";
            // 
            // lbxSourceWorkbook
            // 
            this.lbxSourceWorkbook.FormattingEnabled = true;
            this.lbxSourceWorkbook.Location = new System.Drawing.Point(15, 90);
            this.lbxSourceWorkbook.Name = "lbxSourceWorkbook";
            this.lbxSourceWorkbook.Size = new System.Drawing.Size(303, 69);
            this.lbxSourceWorkbook.TabIndex = 2;
            this.lbxSourceWorkbook.SelectedIndexChanged += new System.EventHandler(this.lbxSourceWorkbook_SelectedIndexChanged);
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(162, 474);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "Import";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(243, 474);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cbxSourceColumnsHaveHeaders
            // 
            this.cbxSourceColumnsHaveHeaders.AutoSize = true;
            this.cbxSourceColumnsHaveHeaders.Location = new System.Drawing.Point(15, 172);
            this.cbxSourceColumnsHaveHeaders.Name = "cbxSourceColumnsHaveHeaders";
            this.cbxSourceColumnsHaveHeaders.Size = new System.Drawing.Size(134, 17);
            this.cbxSourceColumnsHaveHeaders.TabIndex = 3;
            this.cbxSourceColumnsHaveHeaders.Text = "&Columns have headers";
            this.cbxSourceColumnsHaveHeaders.UseVisualStyleBackColor = true;
            this.cbxSourceColumnsHaveHeaders.CheckedChanged += new System.EventHandler(this.cbxSourceColumnsHaveHeaders_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 202);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Columns to &import:";
            // 
            // clbSourceColumns
            // 
            this.clbSourceColumns.CheckOnClick = true;
            this.clbSourceColumns.FormattingEnabled = true;
            this.clbSourceColumns.Location = new System.Drawing.Point(15, 222);
            this.clbSourceColumns.Name = "clbSourceColumns";
            this.clbSourceColumns.Size = new System.Drawing.Size(303, 94);
            this.clbSourceColumns.TabIndex = 5;
            this.clbSourceColumns.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbSourceColumns_ItemCheck);
            // 
            // lblVertex1
            // 
            this.lblVertex1.AutoSize = true;
            this.lblVertex1.Location = new System.Drawing.Point(0, 0);
            this.lblVertex1.Name = "lblVertex1";
            this.lblVertex1.Size = new System.Drawing.Size(176, 13);
            this.lblVertex1.TabIndex = 0;
            this.lblVertex1.Text = "Which imported column is Vertex &1?";
            // 
            // cbxVertex1
            // 
            this.cbxVertex1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxVertex1.FormattingEnabled = true;
            this.cbxVertex1.Location = new System.Drawing.Point(0, 20);
            this.cbxVertex1.Name = "cbxVertex1";
            this.cbxVertex1.Size = new System.Drawing.Size(303, 21);
            this.cbxVertex1.TabIndex = 1;
            this.cbxVertex1.SelectedIndexChanged += new System.EventHandler(this.cbxVertex_SelectedIndexChanged);
            // 
            // cbxVertex2
            // 
            this.cbxVertex2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxVertex2.FormattingEnabled = true;
            this.cbxVertex2.Location = new System.Drawing.Point(0, 75);
            this.cbxVertex2.Name = "cbxVertex2";
            this.cbxVertex2.Size = new System.Drawing.Size(303, 21);
            this.cbxVertex2.TabIndex = 3;
            this.cbxVertex2.SelectedIndexChanged += new System.EventHandler(this.cbxVertex_SelectedIndexChanged);
            // 
            // lblVertex2
            // 
            this.lblVertex2.AutoSize = true;
            this.lblVertex2.Location = new System.Drawing.Point(0, 55);
            this.lblVertex2.Name = "lblVertex2";
            this.lblVertex2.Size = new System.Drawing.Size(176, 13);
            this.lblVertex2.TabIndex = 2;
            this.lblVertex2.Text = "Which imported column is Vertex &2?";
            // 
            // pnlVertices
            // 
            this.pnlVertices.Controls.Add(this.lblVertex1);
            this.pnlVertices.Controls.Add(this.cbxVertex2);
            this.pnlVertices.Controls.Add(this.cbxVertex1);
            this.pnlVertices.Controls.Add(this.lblVertex2);
            this.pnlVertices.Location = new System.Drawing.Point(15, 358);
            this.pnlVertices.Name = "pnlVertices";
            this.pnlVertices.Size = new System.Drawing.Size(308, 103);
            this.pnlVertices.TabIndex = 8;
            // 
            // btnUncheckAllSourceColumns
            // 
            this.btnUncheckAllSourceColumns.Location = new System.Drawing.Point(243, 322);
            this.btnUncheckAllSourceColumns.Name = "btnUncheckAllSourceColumns";
            this.btnUncheckAllSourceColumns.Size = new System.Drawing.Size(75, 23);
            this.btnUncheckAllSourceColumns.TabIndex = 7;
            this.btnUncheckAllSourceColumns.Text = "&Deselect All";
            this.btnUncheckAllSourceColumns.UseVisualStyleBackColor = true;
            this.btnUncheckAllSourceColumns.Click += new System.EventHandler(this.btnUncheckAllSourceColumns_Click);
            // 
            // btnCheckAllSourceColumns
            // 
            this.btnCheckAllSourceColumns.Location = new System.Drawing.Point(162, 322);
            this.btnCheckAllSourceColumns.Name = "btnCheckAllSourceColumns";
            this.btnCheckAllSourceColumns.Size = new System.Drawing.Size(75, 23);
            this.btnCheckAllSourceColumns.TabIndex = 6;
            this.btnCheckAllSourceColumns.Text = "&Select All";
            this.btnCheckAllSourceColumns.UseVisualStyleBackColor = true;
            this.btnCheckAllSourceColumns.Click += new System.EventHandler(this.btnCheckAllSourceColumns_Click);
            // 
            // ImportFromEdgeWorkbookDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(330, 511);
            this.Controls.Add(this.btnUncheckAllSourceColumns);
            this.Controls.Add(this.pnlVertices);
            this.Controls.Add(this.btnCheckAllSourceColumns);
            this.Controls.Add(this.clbSourceColumns);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbxSourceColumnsHaveHeaders);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lbxSourceWorkbook);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportFromEdgeWorkbookDialog";
            this.Text = "Import from Open Edge Workbook";
            this.pnlVertices.ResumeLayout(false);
            this.pnlVertices.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Microsoft.Research.CommunityTechnologies.AppLib.ExcelWorkbookListBox lbxSourceWorkbook;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox cbxSourceColumnsHaveHeaders;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox clbSourceColumns;
        private System.Windows.Forms.Label lblVertex1;
        private System.Windows.Forms.ComboBox cbxVertex1;
        private System.Windows.Forms.ComboBox cbxVertex2;
        private System.Windows.Forms.Label lblVertex2;
        private System.Windows.Forms.Panel pnlVertices;
        private System.Windows.Forms.Button btnUncheckAllSourceColumns;
        private System.Windows.Forms.Button btnCheckAllSourceColumns;
    }
}
