
//  Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class LabelUserSettingsDialog
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnFont = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.usrVertexLabelMaximumLength = new Microsoft.NodeXL.ExcelTemplate.MaximumLabelLengthControl();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxVertexLabelPosition = new Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus();
            this.usrVertexLabelFillColor = new Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.usrEdgeLabelMaximumLength = new Microsoft.NodeXL.ExcelTemplate.MaximumLabelLengthControl();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(274, 179);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(360, 179);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnFont
            // 
            this.btnFont.Location = new System.Drawing.Point(18, 179);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(85, 23);
            this.btnFont.TabIndex = 2;
            this.btnFont.Text = "F&ont...";
            this.btnFont.UseVisualStyleBackColor = true;
            this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.usrVertexLabelMaximumLength);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.cbxVertexLabelPosition);
            this.groupBox3.Controls.Add(this.usrVertexLabelFillColor);
            this.groupBox3.Location = new System.Drawing.Point(18, 9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(203, 154);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Vertex Labels";
            // 
            // usrVertexLabelMaximumLength
            // 
            this.usrVertexLabelMaximumLength.Location = new System.Drawing.Point(12, 23);
            this.usrVertexLabelMaximumLength.Name = "usrVertexLabelMaximumLength";
            this.usrVertexLabelMaximumLength.Size = new System.Drawing.Size(183, 52);
            this.usrVertexLabelMaximumLength.TabIndex = 0;
            this.usrVertexLabelMaximumLength.Value = 2147483647;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "&Position:";
            this.toolTip1.SetToolTip(this.label2, "Select a position for labels that annotate vertices.  This is not used for vertic" +
                    "es that have the shape Label.");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "&Fill color:";
            this.toolTip1.SetToolTip(this.label1, "Select a fill color to use for vertices with the shape Label.  This is not used f" +
                    "or labels that annotate other vertex shapes.");
            // 
            // cbxVertexLabelPosition
            // 
            this.cbxVertexLabelPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxVertexLabelPosition.FormattingEnabled = true;
            this.cbxVertexLabelPosition.Location = new System.Drawing.Point(62, 115);
            this.cbxVertexLabelPosition.Name = "cbxVertexLabelPosition";
            this.cbxVertexLabelPosition.Size = new System.Drawing.Size(118, 21);
            this.cbxVertexLabelPosition.TabIndex = 4;
            // 
            // usrVertexLabelFillColor
            // 
            this.usrVertexLabelFillColor.Color = System.Drawing.Color.White;
            this.usrVertexLabelFillColor.Location = new System.Drawing.Point(62, 75);
            this.usrVertexLabelFillColor.Name = "usrVertexLabelFillColor";
            this.usrVertexLabelFillColor.ShowButton = true;
            this.usrVertexLabelFillColor.Size = new System.Drawing.Size(64, 32);
            this.usrVertexLabelFillColor.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.usrEdgeLabelMaximumLength);
            this.groupBox1.Location = new System.Drawing.Point(237, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(203, 154);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Edge Labels";
            // 
            // usrEdgeLabelMaximumLength
            // 
            this.usrEdgeLabelMaximumLength.Location = new System.Drawing.Point(16, 23);
            this.usrEdgeLabelMaximumLength.Name = "usrEdgeLabelMaximumLength";
            this.usrEdgeLabelMaximumLength.Size = new System.Drawing.Size(183, 52);
            this.usrEdgeLabelMaximumLength.TabIndex = 0;
            this.usrEdgeLabelMaximumLength.Value = 2147483647;
            // 
            // LabelUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(460, 217);
            this.Controls.Add(this.btnFont);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LabelUserSettingsDialog";
            this.Text = "Label Options";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnFont;
        private Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker usrVertexLabelFillColor;
        private System.Windows.Forms.ToolTip toolTip1;
        private Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus cbxVertexLabelPosition;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private MaximumLabelLengthControl usrVertexLabelMaximumLength;
        private MaximumLabelLengthControl usrEdgeLabelMaximumLength;
    }
}
