

//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class GraphImageUserSettingsDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpThisSize = new System.Windows.Forms.GroupBox();
            this.nudHeightPx = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.nudWidthPx = new System.Windows.Forms.NumericUpDown();
            this.grpUseControlSize = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblControlHeight = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblControlWidth = new System.Windows.Forms.Label();
            this.radThisSize = new System.Windows.Forms.RadioButton();
            this.radUseControlSize = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.grpThisSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeightPx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidthPx)).BeginInit();
            this.grpUseControlSize.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(139, 268);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(53, 268);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "When saving an image to a file, make the image this size:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grpThisSize);
            this.panel1.Controls.Add(this.grpUseControlSize);
            this.panel1.Controls.Add(this.radThisSize);
            this.panel1.Controls.Add(this.radUseControlSize);
            this.panel1.Location = new System.Drawing.Point(23, 51);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(198, 202);
            this.panel1.TabIndex = 1;
            // 
            // grpThisSize
            // 
            this.grpThisSize.Controls.Add(this.nudHeightPx);
            this.grpThisSize.Controls.Add(this.label13);
            this.grpThisSize.Controls.Add(this.label4);
            this.grpThisSize.Controls.Add(this.label12);
            this.grpThisSize.Controls.Add(this.label6);
            this.grpThisSize.Controls.Add(this.nudWidthPx);
            this.grpThisSize.Location = new System.Drawing.Point(22, 125);
            this.grpThisSize.Name = "grpThisSize";
            this.grpThisSize.Size = new System.Drawing.Size(174, 72);
            this.grpThisSize.TabIndex = 3;
            this.grpThisSize.TabStop = false;
            // 
            // nudHeightPx
            // 
            this.nudHeightPx.Location = new System.Drawing.Point(55, 38);
            this.nudHeightPx.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudHeightPx.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudHeightPx.Name = "nudHeightPx";
            this.nudHeightPx.Size = new System.Drawing.Size(62, 20);
            this.nudHeightPx.TabIndex = 4;
            this.nudHeightPx.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 16);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(38, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "&Width:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(127, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "pixels";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(127, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(33, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "pixels";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "&Height:";
            // 
            // nudWidthPx
            // 
            this.nudWidthPx.Location = new System.Drawing.Point(55, 12);
            this.nudWidthPx.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudWidthPx.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudWidthPx.Name = "nudWidthPx";
            this.nudWidthPx.Size = new System.Drawing.Size(62, 20);
            this.nudWidthPx.TabIndex = 1;
            this.nudWidthPx.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // grpUseControlSize
            // 
            this.grpUseControlSize.Controls.Add(this.label7);
            this.grpUseControlSize.Controls.Add(this.label2);
            this.grpUseControlSize.Controls.Add(this.lblControlHeight);
            this.grpUseControlSize.Controls.Add(this.label3);
            this.grpUseControlSize.Controls.Add(this.label5);
            this.grpUseControlSize.Controls.Add(this.lblControlWidth);
            this.grpUseControlSize.Location = new System.Drawing.Point(22, 26);
            this.grpUseControlSize.Name = "grpUseControlSize";
            this.grpUseControlSize.Size = new System.Drawing.Size(174, 64);
            this.grpUseControlSize.TabIndex = 1;
            this.grpUseControlSize.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(125, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "pixels";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Width:";
            // 
            // lblControlHeight
            // 
            this.lblControlHeight.AutoSize = true;
            this.lblControlHeight.Location = new System.Drawing.Point(55, 40);
            this.lblControlHeight.Name = "lblControlHeight";
            this.lblControlHeight.Size = new System.Drawing.Size(60, 13);
            this.lblControlHeight.TabIndex = 4;
            this.lblControlHeight.Text = "lblHeightPx";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(125, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "pixels";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Height:";
            // 
            // lblControlWidth
            // 
            this.lblControlWidth.AutoSize = true;
            this.lblControlWidth.Location = new System.Drawing.Point(55, 14);
            this.lblControlWidth.Name = "lblControlWidth";
            this.lblControlWidth.Size = new System.Drawing.Size(57, 13);
            this.lblControlWidth.TabIndex = 1;
            this.lblControlWidth.Text = "lblWidthPx";
            // 
            // radThisSize
            // 
            this.radThisSize.AutoSize = true;
            this.radThisSize.Location = new System.Drawing.Point(3, 102);
            this.radThisSize.Name = "radThisSize";
            this.radThisSize.Size = new System.Drawing.Size(69, 17);
            this.radThisSize.TabIndex = 2;
            this.radThisSize.TabStop = true;
            this.radThisSize.Text = "&This size:";
            this.radThisSize.UseVisualStyleBackColor = true;
            this.radThisSize.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // radUseControlSize
            // 
            this.radUseControlSize.AutoSize = true;
            this.radUseControlSize.Location = new System.Drawing.Point(3, 3);
            this.radUseControlSize.Name = "radUseControlSize";
            this.radUseControlSize.Size = new System.Drawing.Size(155, 17);
            this.radUseControlSize.TabIndex = 0;
            this.radUseControlSize.TabStop = true;
            this.radUseControlSize.Text = "The size of the &graph pane:";
            this.radUseControlSize.UseVisualStyleBackColor = true;
            this.radUseControlSize.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // GraphImageUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(236, 304);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GraphImageUserSettingsDialog";
            this.Text = "Image Size";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grpThisSize.ResumeLayout(false);
            this.grpThisSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeightPx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidthPx)).EndInit();
            this.grpUseControlSize.ResumeLayout(false);
            this.grpUseControlSize.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radThisSize;
        private System.Windows.Forms.RadioButton radUseControlSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblControlWidth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblControlHeight;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudWidthPx;
        private System.Windows.Forms.NumericUpDown nudHeightPx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox grpUseControlSize;
        private System.Windows.Forms.GroupBox grpThisSize;
    }
}
