

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
            this.pnlUseBinning = new System.Windows.Forms.Panel();
            this.nudBinLength = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nudMaximumVerticesPerBin = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnResetAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudFruchtermanReingoldIterations)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFruchtermanReingoldC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMargin)).BeginInit();
            this.pnlUseBinning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBinLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaximumVerticesPerBin)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(242, 296);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(156, 296);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 5;
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
            this.nudFruchtermanReingoldIterations.Location = new System.Drawing.Point(181, 68);
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
            this.groupBox1.Location = new System.Drawing.Point(12, 180);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 101);
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
            this.nudFruchtermanReingoldC.Location = new System.Drawing.Point(181, 28);
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
            this.chkUseBinning.Location = new System.Drawing.Point(9, 19);
            this.chkUseBinning.Name = "chkUseBinning";
            this.chkUseBinning.Size = new System.Drawing.Size(297, 39);
            this.chkUseBinning.TabIndex = 0;
            this.chkUseBinning.Text = "Mo&ve the graph\'s smaller components into boxes at the bottom of the graph";
            this.chkUseBinning.UseVisualStyleBackColor = true;
            this.chkUseBinning.CheckedChanged += new System.EventHandler(this.chkUseBinning_CheckedChanged);
            // 
            // pnlUseBinning
            // 
            this.pnlUseBinning.Controls.Add(this.nudBinLength);
            this.pnlUseBinning.Controls.Add(this.label4);
            this.pnlUseBinning.Controls.Add(this.label3);
            this.pnlUseBinning.Controls.Add(this.nudMaximumVerticesPerBin);
            this.pnlUseBinning.Controls.Add(this.label2);
            this.pnlUseBinning.Location = new System.Drawing.Point(9, 64);
            this.pnlUseBinning.Name = "pnlUseBinning";
            this.pnlUseBinning.Size = new System.Drawing.Size(297, 61);
            this.pnlUseBinning.TabIndex = 0;
            // 
            // nudBinLength
            // 
            this.nudBinLength.Location = new System.Drawing.Point(181, 36);
            this.nudBinLength.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudBinLength.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudBinLength.Name = "nudBinLength";
            this.nudBinLength.Size = new System.Drawing.Size(49, 20);
            this.nudBinLength.TabIndex = 4;
            this.nudBinLength.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Box si&ze:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(236, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "vertices";
            // 
            // nudMaximumVerticesPerBin
            // 
            this.nudMaximumVerticesPerBin.Location = new System.Drawing.Point(181, 10);
            this.nudMaximumVerticesPerBin.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudMaximumVerticesPerBin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMaximumVerticesPerBin.Name = "nudMaximumVerticesPerBin";
            this.nudMaximumVerticesPerBin.Size = new System.Drawing.Size(49, 20);
            this.nudMaximumVerticesPerBin.TabIndex = 1;
            this.nudMaximumVerticesPerBin.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(172, 32);
            this.label2.TabIndex = 0;
            this.label2.Text = "Ma&ximum size of the components to move:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkUseBinning);
            this.groupBox2.Controls.Add(this.pnlUseBinning);
            this.groupBox2.Location = new System.Drawing.Point(12, 42);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(310, 133);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Components";
            // 
            // btnResetAll
            // 
            this.btnResetAll.Location = new System.Drawing.Point(12, 296);
            this.btnResetAll.Name = "btnResetAll";
            this.btnResetAll.Size = new System.Drawing.Size(80, 23);
            this.btnResetAll.TabIndex = 4;
            this.btnResetAll.Text = "Reset All";
            this.btnResetAll.UseVisualStyleBackColor = true;
            this.btnResetAll.Click += new System.EventHandler(this.btnResetAll_Click);
            // 
            // LayoutUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(335, 333);
            this.Controls.Add(this.btnResetAll);
            this.Controls.Add(this.groupBox2);
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
            this.pnlUseBinning.ResumeLayout(false);
            this.pnlUseBinning.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBinLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaximumVerticesPerBin)).EndInit();
            this.groupBox2.ResumeLayout(false);
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
        private System.Windows.Forms.Panel pnlUseBinning;
        private System.Windows.Forms.NumericUpDown nudBinLength;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudMaximumVerticesPerBin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnResetAll;
    }
}
