
//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NetMap.DesktopApplication
{
    partial class GraphSettingsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphSettingsDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.nudEdgeWidth = new System.Windows.Forms.NumericUpDown();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpApplyTo = new System.Windows.Forms.GroupBox();
            this.radAllGraphs = new System.Windows.Forms.RadioButton();
            this.radActiveGraph = new System.Windows.Forms.RadioButton();
            this.nudSelectedEdgeWidth = new System.Windows.Forms.NumericUpDown();
            this.usrEdgeColor = new Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker();
            this.label3 = new System.Windows.Forms.Label();
            this.usrSelectedEdgeColor = new Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker();
            this.usrVertexColor = new Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker();
            this.label6 = new System.Windows.Forms.Label();
            this.usrSelectedVertexColor = new Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker();
            this.label7 = new System.Windows.Forms.Label();
            this.nudVertexRadius = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.cbxVertexShape = new Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus();
            this.label9 = new System.Windows.Forms.Label();
            this.usrBackColor = new Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker();
            this.label10 = new System.Windows.Forms.Label();
            this.nudMargin = new System.Windows.Forms.NumericUpDown();
            this.trbVertexAlpha = new System.Windows.Forms.TrackBar();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.nudRelativeArrowSize = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.trbEdgeAlpha = new System.Windows.Forms.TrackBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnResetAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudEdgeWidth)).BeginInit();
            this.grpApplyTo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSelectedEdgeWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVertexRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMargin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbVertexAlpha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRelativeArrowSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbEdgeAlpha)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "W&idth:";
            // 
            // nudEdgeWidth
            // 
            this.nudEdgeWidth.Location = new System.Drawing.Point(72, 56);
            this.nudEdgeWidth.Name = "nudEdgeWidth";
            this.nudEdgeWidth.Size = new System.Drawing.Size(56, 20);
            this.nudEdgeWidth.TabIndex = 3;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(218, 408);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(304, 408);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // grpApplyTo
            // 
            this.grpApplyTo.Controls.Add(this.radAllGraphs);
            this.grpApplyTo.Controls.Add(this.radActiveGraph);
            this.grpApplyTo.Location = new System.Drawing.Point(208, 311);
            this.grpApplyTo.Name = "grpApplyTo";
            this.grpApplyTo.Size = new System.Drawing.Size(176, 73);
            this.grpApplyTo.TabIndex = 8;
            this.grpApplyTo.TabStop = false;
            this.grpApplyTo.Text = "Appl&y to";
            // 
            // radAllGraphs
            // 
            this.radAllGraphs.AutoSize = true;
            this.radAllGraphs.Checked = true;
            this.radAllGraphs.Location = new System.Drawing.Point(15, 42);
            this.radAllGraphs.Name = "radAllGraphs";
            this.radAllGraphs.Size = new System.Drawing.Size(71, 17);
            this.radAllGraphs.TabIndex = 1;
            this.radAllGraphs.TabStop = true;
            this.radAllGraphs.Text = "All graphs";
            this.radAllGraphs.UseVisualStyleBackColor = true;
            // 
            // radActiveGraph
            // 
            this.radActiveGraph.AutoSize = true;
            this.radActiveGraph.Location = new System.Drawing.Point(15, 19);
            this.radActiveGraph.Name = "radActiveGraph";
            this.radActiveGraph.Size = new System.Drawing.Size(97, 17);
            this.radActiveGraph.TabIndex = 0;
            this.radActiveGraph.Text = "This graph only";
            this.radActiveGraph.UseVisualStyleBackColor = true;
            // 
            // nudSelectedEdgeWidth
            // 
            this.nudSelectedEdgeWidth.Location = new System.Drawing.Point(72, 56);
            this.nudSelectedEdgeWidth.Name = "nudSelectedEdgeWidth";
            this.nudSelectedEdgeWidth.Size = new System.Drawing.Size(56, 20);
            this.nudSelectedEdgeWidth.TabIndex = 3;
            // 
            // usrEdgeColor
            // 
            this.usrEdgeColor.Color = System.Drawing.Color.White;
            this.usrEdgeColor.Location = new System.Drawing.Point(69, 18);
            this.usrEdgeColor.Name = "usrEdgeColor";
            this.usrEdgeColor.ShowButton = true;
            this.usrEdgeColor.Size = new System.Drawing.Size(64, 32);
            this.usrEdgeColor.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "C&olor:";
            // 
            // usrSelectedEdgeColor
            // 
            this.usrSelectedEdgeColor.Color = System.Drawing.Color.White;
            this.usrSelectedEdgeColor.Location = new System.Drawing.Point(69, 18);
            this.usrSelectedEdgeColor.Name = "usrSelectedEdgeColor";
            this.usrSelectedEdgeColor.ShowButton = true;
            this.usrSelectedEdgeColor.Size = new System.Drawing.Size(64, 32);
            this.usrSelectedEdgeColor.TabIndex = 1;
            // 
            // usrVertexColor
            // 
            this.usrVertexColor.Color = System.Drawing.Color.White;
            this.usrVertexColor.Location = new System.Drawing.Point(69, 18);
            this.usrVertexColor.Name = "usrVertexColor";
            this.usrVertexColor.ShowButton = true;
            this.usrVertexColor.Size = new System.Drawing.Size(64, 32);
            this.usrVertexColor.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Co&lor:";
            // 
            // usrSelectedVertexColor
            // 
            this.usrSelectedVertexColor.Color = System.Drawing.Color.White;
            this.usrSelectedVertexColor.Location = new System.Drawing.Point(69, 18);
            this.usrSelectedVertexColor.Name = "usrSelectedVertexColor";
            this.usrSelectedVertexColor.ShowButton = true;
            this.usrSelectedVertexColor.Size = new System.Drawing.Size(64, 32);
            this.usrSelectedVertexColor.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 92);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Radi&us:";
            // 
            // nudVertexRadius
            // 
            this.nudVertexRadius.DecimalPlaces = 1;
            this.nudVertexRadius.Location = new System.Drawing.Point(73, 90);
            this.nudVertexRadius.Name = "nudVertexRadius";
            this.nudVertexRadius.Size = new System.Drawing.Size(56, 20);
            this.nudVertexRadius.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 58);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "&Shape:";
            // 
            // cbxVertexShape
            // 
            this.cbxVertexShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxVertexShape.FormattingEnabled = true;
            this.cbxVertexShape.Location = new System.Drawing.Point(73, 55);
            this.cbxVertexShape.Name = "cbxVertexShape";
            this.cbxVertexShape.Size = new System.Drawing.Size(86, 21);
            this.cbxVertexShape.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 321);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Bac&kground color:";
            // 
            // usrBackColor
            // 
            this.usrBackColor.Color = System.Drawing.Color.White;
            this.usrBackColor.Location = new System.Drawing.Point(109, 311);
            this.usrBackColor.Name = "usrBackColor";
            this.usrBackColor.ShowButton = true;
            this.usrBackColor.Size = new System.Drawing.Size(64, 32);
            this.usrBackColor.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 357);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "&Margin:";
            // 
            // nudMargin
            // 
            this.nudMargin.Location = new System.Drawing.Point(113, 355);
            this.nudMargin.Name = "nudMargin";
            this.nudMargin.Size = new System.Drawing.Size(56, 20);
            this.nudMargin.TabIndex = 7;
            // 
            // trbVertexAlpha
            // 
            this.trbVertexAlpha.Location = new System.Drawing.Point(9, 127);
            this.trbVertexAlpha.Maximum = 255;
            this.trbVertexAlpha.Name = "trbVertexAlpha";
            this.trbVertexAlpha.Size = new System.Drawing.Size(161, 42);
            this.trbVertexAlpha.TabIndex = 7;
            this.trbVertexAlpha.TickFrequency = 32;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 168);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(64, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "&Transparent";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(118, 168);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(45, 13);
            this.label13.TabIndex = 8;
            this.label13.Text = "Opaque";
            // 
            // nudRelativeArrowSize
            // 
            this.nudRelativeArrowSize.DecimalPlaces = 1;
            this.nudRelativeArrowSize.Location = new System.Drawing.Point(73, 90);
            this.nudRelativeArrowSize.Name = "nudRelativeArrowSize";
            this.nudRelativeArrowSize.Size = new System.Drawing.Size(56, 20);
            this.nudRelativeArrowSize.TabIndex = 5;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(9, 92);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(58, 13);
            this.label17.TabIndex = 4;
            this.label17.Text = "&Arrow size:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(117, 168);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(45, 13);
            this.label18.TabIndex = 8;
            this.label18.Text = "Opaque";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(9, 168);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(64, 13);
            this.label19.TabIndex = 6;
            this.label19.Text = "Tra&nsparent";
            // 
            // trbEdgeAlpha
            // 
            this.trbEdgeAlpha.Location = new System.Drawing.Point(9, 127);
            this.trbEdgeAlpha.Maximum = 255;
            this.trbEdgeAlpha.Name = "trbEdgeAlpha";
            this.trbEdgeAlpha.Size = new System.Drawing.Size(161, 42);
            this.trbEdgeAlpha.TabIndex = 7;
            this.trbEdgeAlpha.TickFrequency = 32;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.nudEdgeWidth);
            this.groupBox1.Controls.Add(this.usrEdgeColor);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.nudRelativeArrowSize);
            this.groupBox1.Controls.Add(this.trbEdgeAlpha);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Location = new System.Drawing.Point(208, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(176, 198);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Edges";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label24);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.nudSelectedEdgeWidth);
            this.groupBox2.Controls.Add(this.usrSelectedEdgeColor);
            this.groupBox2.Location = new System.Drawing.Point(208, 213);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(176, 90);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Selected edges";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(9, 28);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(34, 13);
            this.label24.TabIndex = 0;
            this.label24.Text = "Colo&r:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(9, 58);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(38, 13);
            this.label20.TabIndex = 2;
            this.label20.Text = "&Width:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.nudVertexRadius);
            this.groupBox3.Controls.Add(this.usrVertexColor);
            this.groupBox3.Controls.Add(this.trbVertexAlpha);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.cbxVertexShape);
            this.groupBox3.Location = new System.Drawing.Point(18, 9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(176, 198);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Vertices";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "&Color:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.usrSelectedVertexColor);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Location = new System.Drawing.Point(18, 213);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(176, 90);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Selected vertices";
            // 
            // btnResetAll
            // 
            this.btnResetAll.Location = new System.Drawing.Point(18, 408);
            this.btnResetAll.Name = "btnResetAll";
            this.btnResetAll.Size = new System.Drawing.Size(80, 23);
            this.btnResetAll.TabIndex = 11;
            this.btnResetAll.Text = "Reset All";
            this.btnResetAll.UseVisualStyleBackColor = true;
            this.btnResetAll.Click += new System.EventHandler(this.btnResetAll_Click);
            // 
            // GraphSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(402, 443);
            this.Controls.Add(this.btnResetAll);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.nudMargin);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.usrBackColor);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.grpApplyTo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GraphSettingsDialog";
            this.Text = "Options";
            ((System.ComponentModel.ISupportInitialize)(this.nudEdgeWidth)).EndInit();
            this.grpApplyTo.ResumeLayout(false);
            this.grpApplyTo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSelectedEdgeWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVertexRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMargin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbVertexAlpha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRelativeArrowSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbEdgeAlpha)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudEdgeWidth;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpApplyTo;
        private System.Windows.Forms.RadioButton radAllGraphs;
        private System.Windows.Forms.RadioButton radActiveGraph;
        private System.Windows.Forms.NumericUpDown nudSelectedEdgeWidth;
        private Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker usrEdgeColor;
        private System.Windows.Forms.Label label3;
        private Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker usrSelectedEdgeColor;
        private Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker usrVertexColor;
        private System.Windows.Forms.Label label6;
        private Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker usrSelectedVertexColor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudVertexRadius;
        private System.Windows.Forms.Label label8;
        private Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus cbxVertexShape;
        private System.Windows.Forms.Label label9;
        private Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker usrBackColor;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nudMargin;
        private System.Windows.Forms.TrackBar trbVertexAlpha;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown nudRelativeArrowSize;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TrackBar trbEdgeAlpha;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnResetAll;
    }
}
