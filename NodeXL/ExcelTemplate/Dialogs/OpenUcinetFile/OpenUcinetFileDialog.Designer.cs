

//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class OpenUcinetFileDialog
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radUndirected = new System.Windows.Forms.RadioButton();
            this.radDirected = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txbFileName = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lnkHelp = new System.Windows.Forms.LinkLabel();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(327, 204);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(246, 204);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "Import";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radUndirected);
            this.groupBox2.Controls.Add(this.radDirected);
            this.groupBox2.Location = new System.Drawing.Point(15, 114);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(387, 75);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Graph type";
            // 
            // radUndirected
            // 
            this.radUndirected.AutoSize = true;
            this.radUndirected.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radUndirected.Location = new System.Drawing.Point(18, 42);
            this.radUndirected.Name = "radUndirected";
            this.radUndirected.Size = new System.Drawing.Size(253, 17);
            this.radUndirected.TabIndex = 1;
            this.radUndirected.TabStop = true;
            this.radUndirected.Text = "The UCINET file represents an &undirected graph";
            this.radUndirected.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radUndirected.UseVisualStyleBackColor = true;
            // 
            // radDirected
            // 
            this.radDirected.AutoSize = true;
            this.radDirected.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radDirected.Location = new System.Drawing.Point(18, 19);
            this.radDirected.Name = "radDirected";
            this.radDirected.Size = new System.Drawing.Size(235, 17);
            this.radDirected.TabIndex = 0;
            this.radDirected.TabStop = true;
            this.radDirected.Text = "The UCINET file represents a &directed graph";
            this.radDirected.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radDirected.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(330, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "This will clear the workbook, then import a UCINET full matrix DL file.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "UCINET &full matrix DL file:";
            // 
            // txbFileName
            // 
            this.txbFileName.Location = new System.Drawing.Point(15, 79);
            this.txbFileName.Name = "txbFileName";
            this.txbFileName.Size = new System.Drawing.Size(303, 20);
            this.txbFileName.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(327, 79);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "&Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lnkHelp
            // 
            this.lnkHelp.AutoSize = true;
            this.lnkHelp.Location = new System.Drawing.Point(12, 33);
            this.lnkHelp.Name = "lnkHelp";
            this.lnkHelp.Size = new System.Drawing.Size(256, 13);
            this.lnkHelp.TabIndex = 7;
            this.lnkHelp.TabStop = true;
            this.lnkHelp.Text = "What if my UCINET file is not in full matrix DL format?";
            this.lnkHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelp_LinkClicked);
            // 
            // OpenUcinetFileDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(414, 245);
            this.Controls.Add(this.lnkHelp);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txbFileName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenUcinetFileDialog";
            this.Text = "Import from UCINET Full Matrix DL File";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radUndirected;
        private System.Windows.Forms.RadioButton radDirected;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbFileName;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.LinkLabel lnkHelp;
    }
}
