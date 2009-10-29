

//  Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class VertexAttributesDialog
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbxShape = new Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.nudRadius = new System.Windows.Forms.NumericUpDown();
            this.nudAlpha = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.lblAlpha = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbxLocked = new Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus();
            this.label6 = new System.Windows.Forms.Label();
            this.cbxMarked = new Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus();
            this.lnkMarked = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.cbxVisibility = new Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus();
            this.lnkVisibility = new System.Windows.Forms.LinkLabel();
            this.usrColor = new Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker();
            this.label8 = new System.Windows.Forms.Label();
            this.cbxLabelPosition = new Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus();
            ((System.ComponentModel.ISupportInitialize)(this.nudRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAlpha)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(175, 289);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 19;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(256, 289);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cbxShape
            // 
            this.cbxShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxShape.FormattingEnabled = true;
            this.cbxShape.Location = new System.Drawing.Point(94, 49);
            this.cbxShape.Name = "cbxShape";
            this.cbxShape.Size = new System.Drawing.Size(118, 21);
            this.cbxShape.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "&Shape:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "&Color:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 85);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Si&ze:";
            // 
            // nudRadius
            // 
            this.nudRadius.DecimalPlaces = 1;
            this.nudRadius.Location = new System.Drawing.Point(94, 83);
            this.nudRadius.Name = "nudRadius";
            this.nudRadius.Size = new System.Drawing.Size(56, 20);
            this.nudRadius.TabIndex = 5;
            // 
            // nudAlpha
            // 
            this.nudAlpha.DecimalPlaces = 1;
            this.nudAlpha.Location = new System.Drawing.Point(94, 116);
            this.nudAlpha.Name = "nudAlpha";
            this.nudAlpha.Size = new System.Drawing.Size(56, 20);
            this.nudAlpha.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "O&pacity:";
            // 
            // lblAlpha
            // 
            this.lblAlpha.AutoSize = true;
            this.lblAlpha.Location = new System.Drawing.Point(153, 118);
            this.lblAlpha.Name = "lblAlpha";
            this.lblAlpha.Size = new System.Drawing.Size(15, 13);
            this.lblAlpha.TabIndex = 8;
            this.lblAlpha.Text = "%";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 217);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "&Locked?";
            // 
            // cbxLocked
            // 
            this.cbxLocked.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxLocked.FormattingEnabled = true;
            this.cbxLocked.Location = new System.Drawing.Point(94, 214);
            this.cbxLocked.Name = "cbxLocked";
            this.cbxLocked.Size = new System.Drawing.Size(94, 21);
            this.cbxLocked.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 250);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "&Marked?";
            // 
            // cbxMarked
            // 
            this.cbxMarked.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxMarked.FormattingEnabled = true;
            this.cbxMarked.Location = new System.Drawing.Point(94, 247);
            this.cbxMarked.Name = "cbxMarked";
            this.cbxMarked.Size = new System.Drawing.Size(94, 21);
            this.cbxMarked.TabIndex = 17;
            // 
            // lnkMarked
            // 
            this.lnkMarked.AutoSize = true;
            this.lnkMarked.Location = new System.Drawing.Point(201, 250);
            this.lnkMarked.Name = "lnkMarked";
            this.lnkMarked.Size = new System.Drawing.Size(68, 13);
            this.lnkMarked.TabIndex = 18;
            this.lnkMarked.TabStop = true;
            this.lnkMarked.Text = "What is this?";
            this.lnkMarked.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkMarked_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "&Visibility:";
            // 
            // cbxVisibility
            // 
            this.cbxVisibility.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxVisibility.FormattingEnabled = true;
            this.cbxVisibility.Location = new System.Drawing.Point(94, 148);
            this.cbxVisibility.Name = "cbxVisibility";
            this.cbxVisibility.Size = new System.Drawing.Size(94, 21);
            this.cbxVisibility.TabIndex = 10;
            // 
            // lnkVisibility
            // 
            this.lnkVisibility.AutoSize = true;
            this.lnkVisibility.Location = new System.Drawing.Point(201, 151);
            this.lnkVisibility.Name = "lnkVisibility";
            this.lnkVisibility.Size = new System.Drawing.Size(127, 13);
            this.lnkVisibility.TabIndex = 11;
            this.lnkVisibility.TabStop = true;
            this.lnkVisibility.Text = "Workbook may be reread";
            this.lnkVisibility.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkVisibility_LinkClicked);
            // 
            // usrColor
            // 
            this.usrColor.Color = System.Drawing.Color.White;
            this.usrColor.Location = new System.Drawing.Point(92, 9);
            this.usrColor.Name = "usrColor";
            this.usrColor.ShowButton = true;
            this.usrColor.Size = new System.Drawing.Size(64, 32);
            this.usrColor.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 184);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "La&bel position:";
            // 
            // cbxLabelPosition
            // 
            this.cbxLabelPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxLabelPosition.FormattingEnabled = true;
            this.cbxLabelPosition.Location = new System.Drawing.Point(94, 181);
            this.cbxLabelPosition.Name = "cbxLabelPosition";
            this.cbxLabelPosition.Size = new System.Drawing.Size(118, 21);
            this.cbxLabelPosition.TabIndex = 13;
            // 
            // VertexAttributesDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(343, 327);
            this.Controls.Add(this.cbxLabelPosition);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.usrColor);
            this.Controls.Add(this.lnkVisibility);
            this.Controls.Add(this.cbxVisibility);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lnkMarked);
            this.Controls.Add(this.cbxMarked);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbxLocked);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblAlpha);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudAlpha);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.nudRadius);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbxShape);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VertexAttributesDialog";
            this.Text = "Vertex Properties";
            ((System.ComponentModel.ISupportInitialize)(this.nudRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAlpha)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus cbxShape;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudRadius;
        private System.Windows.Forms.NumericUpDown nudAlpha;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblAlpha;
        private System.Windows.Forms.Label label5;
        private Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus cbxLocked;
        private System.Windows.Forms.Label label6;
        private Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus cbxMarked;
        private System.Windows.Forms.LinkLabel lnkMarked;
        private System.Windows.Forms.Label label4;
        private Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus cbxVisibility;
        private System.Windows.Forms.LinkLabel lnkVisibility;
        private Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker usrColor;
        private System.Windows.Forms.Label label8;
        private Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus cbxLabelPosition;
    }
}
