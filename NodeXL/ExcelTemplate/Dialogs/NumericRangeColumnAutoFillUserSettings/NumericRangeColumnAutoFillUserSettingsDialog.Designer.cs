

//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class NumericRangeColumnAutoFillUserSettingsDialog
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
            this.radUseMaximumSourceNumber = new System.Windows.Forms.RadioButton();
            this.txbSourceNumber1 = new System.Windows.Forms.TextBox();
            this.radUseMinimumSourceNumber = new System.Windows.Forms.RadioButton();
            this.txbSourceNumber2 = new System.Windows.Forms.TextBox();
            this.radUseSourceNumber1 = new System.Windows.Forms.RadioButton();
            this.nudDestinationNumber2 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.lnkIgnoreOutliers = new System.Windows.Forms.LinkLabel();
            this.lblDestinationNumber1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudDestinationNumber1 = new System.Windows.Forms.NumericUpDown();
            this.chkIgnoreOutliers = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblDestinationNumber2 = new System.Windows.Forms.Label();
            this.radUseSourceNumber2 = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudDestinationNumber2)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDestinationNumber1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(400, 196);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(314, 196);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // radUseMaximumSourceNumber
            // 
            this.radUseMaximumSourceNumber.AutoSize = true;
            this.radUseMaximumSourceNumber.Checked = true;
            this.radUseMaximumSourceNumber.Location = new System.Drawing.Point(24, 38);
            this.radUseMaximumSourceNumber.Name = "radUseMaximumSourceNumber";
            this.radUseMaximumSourceNumber.Size = new System.Drawing.Size(182, 17);
            this.radUseMaximumSourceNumber.TabIndex = 1;
            this.radUseMaximumSourceNumber.TabStop = true;
            this.radUseMaximumSourceNumber.Text = "The largest number in the column";
            this.radUseMaximumSourceNumber.UseVisualStyleBackColor = true;
            this.radUseMaximumSourceNumber.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // txbSourceNumber1
            // 
            this.txbSourceNumber1.Enabled = false;
            this.txbSourceNumber1.Location = new System.Drawing.Point(44, 61);
            this.txbSourceNumber1.MaxLength = 50;
            this.txbSourceNumber1.Name = "txbSourceNumber1";
            this.txbSourceNumber1.Size = new System.Drawing.Size(86, 20);
            this.txbSourceNumber1.TabIndex = 3;
            // 
            // radUseMinimumSourceNumber
            // 
            this.radUseMinimumSourceNumber.AutoSize = true;
            this.radUseMinimumSourceNumber.Checked = true;
            this.radUseMinimumSourceNumber.Location = new System.Drawing.Point(24, 38);
            this.radUseMinimumSourceNumber.Name = "radUseMinimumSourceNumber";
            this.radUseMinimumSourceNumber.Size = new System.Drawing.Size(188, 17);
            this.radUseMinimumSourceNumber.TabIndex = 1;
            this.radUseMinimumSourceNumber.TabStop = true;
            this.radUseMinimumSourceNumber.Text = "The smallest number in the column";
            this.radUseMinimumSourceNumber.UseVisualStyleBackColor = true;
            this.radUseMinimumSourceNumber.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // txbSourceNumber2
            // 
            this.txbSourceNumber2.Enabled = false;
            this.txbSourceNumber2.Location = new System.Drawing.Point(44, 61);
            this.txbSourceNumber2.MaxLength = 50;
            this.txbSourceNumber2.Name = "txbSourceNumber2";
            this.txbSourceNumber2.Size = new System.Drawing.Size(85, 20);
            this.txbSourceNumber2.TabIndex = 3;
            // 
            // radUseSourceNumber1
            // 
            this.radUseSourceNumber1.AutoSize = true;
            this.radUseSourceNumber1.Location = new System.Drawing.Point(24, 64);
            this.radUseSourceNumber1.Name = "radUseSourceNumber1";
            this.radUseSourceNumber1.Size = new System.Drawing.Size(14, 13);
            this.radUseSourceNumber1.TabIndex = 2;
            this.radUseSourceNumber1.UseVisualStyleBackColor = true;
            this.radUseSourceNumber1.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // nudDestinationNumber2
            // 
            this.nudDestinationNumber2.DecimalPlaces = 1;
            this.nudDestinationNumber2.Location = new System.Drawing.Point(11, 122);
            this.nudDestinationNumber2.Name = "nudDestinationNumber2";
            this.nudDestinationNumber2.Size = new System.Drawing.Size(69, 20);
            this.nudDestinationNumber2.TabIndex = 5;
            this.nudDestinationNumber2.ThousandsSeparator = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "M&ap this source column number:";
            // 
            // lnkIgnoreOutliers
            // 
            this.lnkIgnoreOutliers.AutoSize = true;
            this.lnkIgnoreOutliers.Location = new System.Drawing.Point(110, 176);
            this.lnkIgnoreOutliers.Name = "lnkIgnoreOutliers";
            this.lnkIgnoreOutliers.Size = new System.Drawing.Size(68, 13);
            this.lnkIgnoreOutliers.TabIndex = 3;
            this.lnkIgnoreOutliers.TabStop = true;
            this.lnkIgnoreOutliers.Text = "What is this?";
            this.lnkIgnoreOutliers.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkIgnoreOutliers_LinkClicked);
            // 
            // lblDestinationNumber1
            // 
            this.lblDestinationNumber1.AutoSize = true;
            this.lblDestinationNumber1.Location = new System.Drawing.Point(11, 101);
            this.lblDestinationNumber1.Name = "lblDestinationNumber1";
            this.lblDestinationNumber1.Size = new System.Drawing.Size(90, 13);
            this.lblDestinationNumber1.TabIndex = 4;
            this.lblDestinationNumber1.Text = "[Gets set in code]";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txbSourceNumber1);
            this.groupBox1.Controls.Add(this.radUseMinimumSourceNumber);
            this.groupBox1.Controls.Add(this.radUseSourceNumber1);
            this.groupBox1.Controls.Add(this.nudDestinationNumber1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lblDestinationNumber1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(230, 154);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // nudDestinationNumber1
            // 
            this.nudDestinationNumber1.DecimalPlaces = 1;
            this.nudDestinationNumber1.Location = new System.Drawing.Point(11, 122);
            this.nudDestinationNumber1.Name = "nudDestinationNumber1";
            this.nudDestinationNumber1.Size = new System.Drawing.Size(69, 20);
            this.nudDestinationNumber1.TabIndex = 5;
            this.nudDestinationNumber1.ThousandsSeparator = true;
            // 
            // chkIgnoreOutliers
            // 
            this.chkIgnoreOutliers.AutoSize = true;
            this.chkIgnoreOutliers.Location = new System.Drawing.Point(12, 175);
            this.chkIgnoreOutliers.Name = "chkIgnoreOutliers";
            this.chkIgnoreOutliers.Size = new System.Drawing.Size(92, 17);
            this.chkIgnoreOutliers.TabIndex = 2;
            this.chkIgnoreOutliers.Text = "&Ignore outliers";
            this.chkIgnoreOutliers.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Ma&p this source column number:";
            // 
            // lblDestinationNumber2
            // 
            this.lblDestinationNumber2.AutoSize = true;
            this.lblDestinationNumber2.Location = new System.Drawing.Point(11, 101);
            this.lblDestinationNumber2.Name = "lblDestinationNumber2";
            this.lblDestinationNumber2.Size = new System.Drawing.Size(90, 13);
            this.lblDestinationNumber2.TabIndex = 4;
            this.lblDestinationNumber2.Text = "[Gets set in code]";
            // 
            // radUseSourceNumber2
            // 
            this.radUseSourceNumber2.AutoSize = true;
            this.radUseSourceNumber2.Location = new System.Drawing.Point(24, 64);
            this.radUseSourceNumber2.Name = "radUseSourceNumber2";
            this.radUseSourceNumber2.Size = new System.Drawing.Size(14, 13);
            this.radUseSourceNumber2.TabIndex = 2;
            this.radUseSourceNumber2.UseVisualStyleBackColor = true;
            this.radUseSourceNumber2.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txbSourceNumber2);
            this.groupBox2.Controls.Add(this.radUseMaximumSourceNumber);
            this.groupBox2.Controls.Add(this.nudDestinationNumber2);
            this.groupBox2.Controls.Add(this.radUseSourceNumber2);
            this.groupBox2.Controls.Add(this.lblDestinationNumber2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(250, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(230, 154);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // NumericRangeColumnAutoFillUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(492, 232);
            this.Controls.Add(this.lnkIgnoreOutliers);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkIgnoreOutliers);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NumericRangeColumnAutoFillUserSettingsDialog";
            this.Text = "[Gets set in code]";
            ((System.ComponentModel.ISupportInitialize)(this.nudDestinationNumber2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDestinationNumber1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RadioButton radUseMaximumSourceNumber;
        private System.Windows.Forms.TextBox txbSourceNumber1;
        private System.Windows.Forms.RadioButton radUseMinimumSourceNumber;
        private System.Windows.Forms.TextBox txbSourceNumber2;
        private System.Windows.Forms.RadioButton radUseSourceNumber1;
        private System.Windows.Forms.NumericUpDown nudDestinationNumber2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel lnkIgnoreOutliers;
        private System.Windows.Forms.Label lblDestinationNumber1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nudDestinationNumber1;
        private System.Windows.Forms.CheckBox chkIgnoreOutliers;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblDestinationNumber2;
        private System.Windows.Forms.RadioButton radUseSourceNumber2;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}
