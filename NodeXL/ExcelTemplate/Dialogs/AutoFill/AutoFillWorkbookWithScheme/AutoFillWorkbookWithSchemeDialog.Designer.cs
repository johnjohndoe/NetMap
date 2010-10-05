

//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class AutoFillWorkbookWithSchemeDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoFillWorkbookWithSchemeDialog));
            this.picVertexCategories = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.radVertexCategories = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.lblVertexCategoryColumnName = new System.Windows.Forms.Label();
            this.cbxVertexCategoryColumnName = new Microsoft.NodeXL.ExcelTemplate.VertexColumnComboBox();
            this.cbxEdgeWeightColumnName = new Microsoft.NodeXL.ExcelTemplate.EdgeColumnComboBox();
            this.lblEdgeWeightColumnName = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.radEdgeWeights = new System.Windows.Forms.RadioButton();
            this.picEdgeWeights = new System.Windows.Forms.PictureBox();
            this.cbxEdgeTimestampColumnName = new Microsoft.NodeXL.ExcelTemplate.EdgeColumnComboBox();
            this.lblEdgeTimestampColumnName = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.radEdgeTimestamps = new System.Windows.Forms.RadioButton();
            this.picEdgeTimestamps = new System.Windows.Forms.PictureBox();
            this.chkShowVertexLabels = new System.Windows.Forms.CheckBox();
            this.lblVertexLabelColumnName = new System.Windows.Forms.Label();
            this.cbxVertexLabelColumnName = new Microsoft.NodeXL.ExcelTemplate.VertexColumnComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.pnlVisualSchemes = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picVertexCategories)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEdgeWeights)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEdgeTimestamps)).BeginInit();
            this.pnlVisualSchemes.SuspendLayout();
            this.SuspendLayout();
            // 
            // picVertexCategories
            // 
            this.picVertexCategories.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picVertexCategories.Image = ((System.Drawing.Image)(resources.GetObject("picVertexCategories.Image")));
            this.picVertexCategories.Location = new System.Drawing.Point(15, 69);
            this.picVertexCategories.Name = "picVertexCategories";
            this.picVertexCategories.Size = new System.Drawing.Size(96, 96);
            this.picVertexCategories.TabIndex = 0;
            this.picVertexCategories.TabStop = false;
            this.picVertexCategories.Click += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Use this to apply a scheme of visual properties to the graph.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(186, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Which scheme would you like to use?";
            // 
            // radVertexCategories
            // 
            this.radVertexCategories.AutoSize = true;
            this.radVertexCategories.Checked = true;
            this.radVertexCategories.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radVertexCategories.Location = new System.Drawing.Point(3, 4);
            this.radVertexCategories.Name = "radVertexCategories";
            this.radVertexCategories.Size = new System.Drawing.Size(192, 20);
            this.radVertexCategories.TabIndex = 0;
            this.radVertexCategories.TabStop = true;
            this.radVertexCategories.Text = "&Categorized Graph Scheme";
            this.radVertexCategories.UseVisualStyleBackColor = true;
            this.radVertexCategories.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(20, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(221, 87);
            this.label3.TabIndex = 1;
            this.label3.Text = "The graph\'s vertices are grouped into categories, which are specified by a catego" +
                "ry column in the Vertices worksheet.  All the vertices in a category will have t" +
                "he same color, shape, and size.";
            // 
            // lblVertexCategoryColumnName
            // 
            this.lblVertexCategoryColumnName.AutoSize = true;
            this.lblVertexCategoryColumnName.Location = new System.Drawing.Point(367, 86);
            this.lblVertexCategoryColumnName.Name = "lblVertexCategoryColumnName";
            this.lblVertexCategoryColumnName.Size = new System.Drawing.Size(89, 13);
            this.lblVertexCategoryColumnName.TabIndex = 3;
            this.lblVertexCategoryColumnName.Text = "C&ategory column:";
            // 
            // cbxVertexCategoryColumnName
            // 
            this.cbxVertexCategoryColumnName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxVertexCategoryColumnName.FormattingEnabled = true;
            this.cbxVertexCategoryColumnName.Location = new System.Drawing.Point(370, 102);
            this.cbxVertexCategoryColumnName.Name = "cbxVertexCategoryColumnName";
            this.cbxVertexCategoryColumnName.Size = new System.Drawing.Size(149, 21);
            this.cbxVertexCategoryColumnName.TabIndex = 4;
            // 
            // cbxEdgeWeightColumnName
            // 
            this.cbxEdgeWeightColumnName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxEdgeWeightColumnName.FormattingEnabled = true;
            this.cbxEdgeWeightColumnName.Location = new System.Drawing.Point(370, 214);
            this.cbxEdgeWeightColumnName.Name = "cbxEdgeWeightColumnName";
            this.cbxEdgeWeightColumnName.Size = new System.Drawing.Size(149, 21);
            this.cbxEdgeWeightColumnName.TabIndex = 6;
            // 
            // lblEdgeWeightColumnName
            // 
            this.lblEdgeWeightColumnName.AutoSize = true;
            this.lblEdgeWeightColumnName.Location = new System.Drawing.Point(367, 198);
            this.lblEdgeWeightColumnName.Name = "lblEdgeWeightColumnName";
            this.lblEdgeWeightColumnName.Size = new System.Drawing.Size(81, 13);
            this.lblEdgeWeightColumnName.TabIndex = 5;
            this.lblEdgeWeightColumnName.Text = "W&eight column:";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(20, 140);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(221, 87);
            this.label6.TabIndex = 3;
            this.label6.Text = "The graph\'s edges have weights, which are specified by a numerical weight column " +
                "in the Edges worksheet.  The width of each edge will be proportional to its weig" +
                "ht.";
            // 
            // radEdgeWeights
            // 
            this.radEdgeWeights.AutoSize = true;
            this.radEdgeWeights.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radEdgeWeights.Location = new System.Drawing.Point(3, 117);
            this.radEdgeWeights.Name = "radEdgeWeights";
            this.radEdgeWeights.Size = new System.Drawing.Size(177, 20);
            this.radEdgeWeights.TabIndex = 2;
            this.radEdgeWeights.Text = "&Weighted Graph Scheme";
            this.radEdgeWeights.UseVisualStyleBackColor = true;
            this.radEdgeWeights.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // picEdgeWeights
            // 
            this.picEdgeWeights.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picEdgeWeights.Image = ((System.Drawing.Image)(resources.GetObject("picEdgeWeights.Image")));
            this.picEdgeWeights.Location = new System.Drawing.Point(15, 182);
            this.picEdgeWeights.Name = "picEdgeWeights";
            this.picEdgeWeights.Size = new System.Drawing.Size(96, 96);
            this.picEdgeWeights.TabIndex = 7;
            this.picEdgeWeights.TabStop = false;
            this.picEdgeWeights.Click += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // cbxEdgeTimestampColumnName
            // 
            this.cbxEdgeTimestampColumnName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxEdgeTimestampColumnName.FormattingEnabled = true;
            this.cbxEdgeTimestampColumnName.Location = new System.Drawing.Point(370, 327);
            this.cbxEdgeTimestampColumnName.Name = "cbxEdgeTimestampColumnName";
            this.cbxEdgeTimestampColumnName.Size = new System.Drawing.Size(149, 21);
            this.cbxEdgeTimestampColumnName.TabIndex = 8;
            // 
            // lblEdgeTimestampColumnName
            // 
            this.lblEdgeTimestampColumnName.AutoSize = true;
            this.lblEdgeTimestampColumnName.Location = new System.Drawing.Point(367, 311);
            this.lblEdgeTimestampColumnName.Name = "lblEdgeTimestampColumnName";
            this.lblEdgeTimestampColumnName.Size = new System.Drawing.Size(98, 13);
            this.lblEdgeTimestampColumnName.TabIndex = 7;
            this.lblEdgeTimestampColumnName.Text = "Ti&mestamp column:";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(20, 253);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(221, 80);
            this.label8.TabIndex = 5;
            this.label8.Text = "The graph\'s edges have timestamps, which are specified by a date, time, or numeri" +
                "cal timestamp column in the Edges worksheet.  The color of each edge will be bas" +
                "ed on its timestamp.";
            // 
            // radEdgeTimestamps
            // 
            this.radEdgeTimestamps.AutoSize = true;
            this.radEdgeTimestamps.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radEdgeTimestamps.Location = new System.Drawing.Point(3, 230);
            this.radEdgeTimestamps.Name = "radEdgeTimestamps";
            this.radEdgeTimestamps.Size = new System.Drawing.Size(178, 20);
            this.radEdgeTimestamps.TabIndex = 4;
            this.radEdgeTimestamps.Text = "&Temporal Graph Scheme";
            this.radEdgeTimestamps.UseVisualStyleBackColor = true;
            this.radEdgeTimestamps.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // picEdgeTimestamps
            // 
            this.picEdgeTimestamps.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picEdgeTimestamps.Image = ((System.Drawing.Image)(resources.GetObject("picEdgeTimestamps.Image")));
            this.picEdgeTimestamps.Location = new System.Drawing.Point(15, 295);
            this.picEdgeTimestamps.Name = "picEdgeTimestamps";
            this.picEdgeTimestamps.Size = new System.Drawing.Size(96, 96);
            this.picEdgeTimestamps.TabIndex = 12;
            this.picEdgeTimestamps.TabStop = false;
            this.picEdgeTimestamps.Click += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // chkShowVertexLabels
            // 
            this.chkShowVertexLabels.AutoSize = true;
            this.chkShowVertexLabels.Location = new System.Drawing.Point(15, 408);
            this.chkShowVertexLabels.Name = "chkShowVertexLabels";
            this.chkShowVertexLabels.Size = new System.Drawing.Size(332, 17);
            this.chkShowVertexLabels.TabIndex = 9;
            this.chkShowVertexLabels.Text = "&Label each vertex using a label column in the Vertices worksheet";
            this.chkShowVertexLabels.UseVisualStyleBackColor = true;
            this.chkShowVertexLabels.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // lblVertexLabelColumnName
            // 
            this.lblVertexLabelColumnName.AutoSize = true;
            this.lblVertexLabelColumnName.Location = new System.Drawing.Point(49, 432);
            this.lblVertexLabelColumnName.Name = "lblVertexLabelColumnName";
            this.lblVertexLabelColumnName.Size = new System.Drawing.Size(73, 13);
            this.lblVertexLabelColumnName.TabIndex = 10;
            this.lblVertexLabelColumnName.Text = "La&bel column:";
            // 
            // cbxVertexLabelColumnName
            // 
            this.cbxVertexLabelColumnName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxVertexLabelColumnName.FormattingEnabled = true;
            this.cbxVertexLabelColumnName.Location = new System.Drawing.Point(52, 448);
            this.cbxVertexLabelColumnName.Name = "cbxVertexLabelColumnName";
            this.cbxVertexLabelColumnName.Size = new System.Drawing.Size(149, 21);
            this.cbxVertexLabelColumnName.TabIndex = 11;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(444, 448);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(363, 448);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pnlVisualSchemes
            // 
            this.pnlVisualSchemes.Controls.Add(this.label3);
            this.pnlVisualSchemes.Controls.Add(this.radVertexCategories);
            this.pnlVisualSchemes.Controls.Add(this.radEdgeWeights);
            this.pnlVisualSchemes.Controls.Add(this.label6);
            this.pnlVisualSchemes.Controls.Add(this.radEdgeTimestamps);
            this.pnlVisualSchemes.Controls.Add(this.label8);
            this.pnlVisualSchemes.Location = new System.Drawing.Point(117, 69);
            this.pnlVisualSchemes.Name = "pnlVisualSchemes";
            this.pnlVisualSchemes.Size = new System.Drawing.Size(244, 333);
            this.pnlVisualSchemes.TabIndex = 2;
            // 
            // AutoFillWorkbookWithSchemeDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(532, 488);
            this.Controls.Add(this.pnlVisualSchemes);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbxVertexLabelColumnName);
            this.Controls.Add(this.lblVertexLabelColumnName);
            this.Controls.Add(this.chkShowVertexLabels);
            this.Controls.Add(this.cbxEdgeTimestampColumnName);
            this.Controls.Add(this.lblEdgeTimestampColumnName);
            this.Controls.Add(this.picEdgeTimestamps);
            this.Controls.Add(this.cbxEdgeWeightColumnName);
            this.Controls.Add(this.lblEdgeWeightColumnName);
            this.Controls.Add(this.picEdgeWeights);
            this.Controls.Add(this.cbxVertexCategoryColumnName);
            this.Controls.Add(this.lblVertexCategoryColumnName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picVertexCategories);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutoFillWorkbookWithSchemeDialog";
            this.Text = "Schemes";
            ((System.ComponentModel.ISupportInitialize)(this.picVertexCategories)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEdgeWeights)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEdgeTimestamps)).EndInit();
            this.pnlVisualSchemes.ResumeLayout(false);
            this.pnlVisualSchemes.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picVertexCategories;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radVertexCategories;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblVertexCategoryColumnName;
        private Microsoft.NodeXL.ExcelTemplate.VertexColumnComboBox cbxVertexCategoryColumnName;
        private Microsoft.NodeXL.ExcelTemplate.EdgeColumnComboBox cbxEdgeWeightColumnName;
        private System.Windows.Forms.Label lblEdgeWeightColumnName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton radEdgeWeights;
        private System.Windows.Forms.PictureBox picEdgeWeights;
        private Microsoft.NodeXL.ExcelTemplate.EdgeColumnComboBox cbxEdgeTimestampColumnName;
        private System.Windows.Forms.Label lblEdgeTimestampColumnName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton radEdgeTimestamps;
        private System.Windows.Forms.PictureBox picEdgeTimestamps;
        private System.Windows.Forms.CheckBox chkShowVertexLabels;
        private System.Windows.Forms.Label lblVertexLabelColumnName;
        private Microsoft.NodeXL.ExcelTemplate.VertexColumnComboBox cbxVertexLabelColumnName;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel pnlVisualSchemes;
    }
}
