

//  Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class LayoutUserSettingsDialog
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
            this.label7 = new System.Windows.Forms.Label();
            this.nudFruchtermanReingoldIterations = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudFruchtermanReingoldC = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nudMargin = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.chkUseBinning = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudFruchtermanReingoldIterations)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFruchtermanReingoldC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMargin)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(160, 214);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(74, 214);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 75);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "&Iterations per layout:";
            // 
            // nudFruchtermanReingoldIterations
            // 
            this.nudFruchtermanReingoldIterations.Location = new System.Drawing.Point(155, 73);
            this.nudFruchtermanReingoldIterations.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFruchtermanReingoldIterations.Name = "nudFruchtermanReingoldIterations";
            this.nudFruchtermanReingoldIterations.Size = new System.Drawing.Size(56, 20);
            this.nudFruchtermanReingoldIterations.TabIndex = 3;
            this.nudFruchtermanReingoldIterations.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudFruchtermanReingoldC);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.nudFruchtermanReingoldIterations);
            this.groupBox1.Location = new System.Drawing.Point(12, 89);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(228, 109);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fruchterman-Reingold layout";
            // 
            // nudFruchtermanReingoldC
            // 
            this.nudFruchtermanReingoldC.DecimalPlaces = 1;
            this.nudFruchtermanReingoldC.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudFruchtermanReingoldC.Location = new System.Drawing.Point(155, 28);
            this.nudFruchtermanReingoldC.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudFruchtermanReingoldC.Name = "nudFruchtermanReingoldC";
            this.nudFruchtermanReingoldC.Size = new System.Drawing.Size(56, 20);
            this.nudFruchtermanReingoldC.TabIndex = 1;
            this.nudFruchtermanReingoldC.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Strength of the repulsive force between vertices:";
            // 
            // nudMargin
            // 
            this.nudMargin.Location = new System.Drawing.Point(74, 7);
            this.nudMargin.Name = "nudMargin";
            this.nudMargin.Size = new System.Drawing.Size(56, 20);
            this.nudMargin.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "&Margin:";
            // 
            // chkUseBinning
            // 
            this.chkUseBinning.Location = new System.Drawing.Point(15, 33);
            this.chkUseBinning.Name = "chkUseBinning";
            this.chkUseBinning.Size = new System.Drawing.Size(224, 51);
            this.chkUseBinning.TabIndex = 2;
            this.chkUseBinning.Text = "&Put the graph\'s smaller components at the bottom of the graph";
            this.chkUseBinning.UseVisualStyleBackColor = true;
            // 
            // LayoutUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(253, 253);
            this.Controls.Add(this.chkUseBinning);
            this.Controls.Add(this.nudMargin);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LayoutUserSettingsDialog";
            this.Text = "Layout Options";
            ((System.ComponentModel.ISupportInitialize)(this.nudFruchtermanReingoldIterations)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFruchtermanReingoldC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMargin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudFruchtermanReingoldIterations;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudFruchtermanReingoldC;
        private System.Windows.Forms.NumericUpDown nudMargin;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkUseBinning;
    }
}
