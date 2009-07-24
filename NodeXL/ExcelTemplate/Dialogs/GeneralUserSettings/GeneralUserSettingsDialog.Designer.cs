
//  Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class GeneralUserSettingsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeneralUserSettingsDialog));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnCustomizeVertexMenu = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnLayout = new System.Windows.Forms.Button();
            this.btnLabelFont = new System.Windows.Forms.Button();
            this.btnResetAll = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.usrSelectedVertexColor = new Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker();
            this.chkAutoSelect = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.usrPrimaryLabelFillColor = new Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker();
            this.usrVertexColor = new Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker();
            this.label4 = new System.Windows.Forms.Label();
            this.lblVertexAlpha = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nudVertexAlpha = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.nudVertexRadius = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.cbxVertexShape = new Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.usrSelectedEdgeColor = new Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker();
            this.label24 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.nudSelectedEdgeWidth = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.usrEdgeColor = new Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker();
            this.label11 = new System.Windows.Forms.Label();
            this.lblEdgeAlpha = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.nudEdgeAlpha = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nudEdgeWidth = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudRelativeArrowSize = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.usrBackColor = new Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker();
            this.chkAutoReadWorkbook = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnAxisFont = new System.Windows.Forms.Button();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVertexAlpha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVertexRadius)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSelectedEdgeWidth)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEdgeAlpha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEdgeWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRelativeArrowSize)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(326, 470);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(412, 470);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnCustomizeVertexMenu);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Location = new System.Drawing.Point(17, 324);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(223, 133);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Menus";
            // 
            // btnCustomizeVertexMenu
            // 
            this.btnCustomizeVertexMenu.Location = new System.Drawing.Point(119, 96);
            this.btnCustomizeVertexMenu.Name = "btnCustomizeVertexMenu";
            this.btnCustomizeVertexMenu.Size = new System.Drawing.Size(90, 23);
            this.btnCustomizeVertexMenu.TabIndex = 2;
            this.btnCustomizeVertexMenu.Text = "Custo&mize...";
            this.btnCustomizeVertexMenu.UseVisualStyleBackColor = true;
            this.btnCustomizeVertexMenu.Click += new System.EventHandler(this.btnCustomizeVertexMenu_Click);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(10, 46);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(199, 47);
            this.label14.TabIndex = 1;
            this.label14.Text = "Add custom menu items to the menu that appears when you right-click a vertex in t" +
                "he NodeXL graph";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(10, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 16);
            this.label10.TabIndex = 0;
            this.label10.Text = "Vertex Menus";
            // 
            // btnLayout
            // 
            this.btnLayout.Location = new System.Drawing.Point(412, 334);
            this.btnLayout.Name = "btnLayout";
            this.btnLayout.Size = new System.Drawing.Size(80, 23);
            this.btnLayout.TabIndex = 7;
            this.btnLayout.Text = "L&ayout...";
            this.btnLayout.UseVisualStyleBackColor = true;
            this.btnLayout.Click += new System.EventHandler(this.btnLayout_Click);
            // 
            // btnLabelFont
            // 
            this.btnLabelFont.Location = new System.Drawing.Point(258, 372);
            this.btnLabelFont.Name = "btnLabelFont";
            this.btnLabelFont.Size = new System.Drawing.Size(80, 23);
            this.btnLabelFont.TabIndex = 8;
            this.btnLabelFont.Text = "Label Fo&nt...";
            this.btnLabelFont.UseVisualStyleBackColor = true;
            this.btnLabelFont.Click += new System.EventHandler(this.btnLabelFont_Click);
            // 
            // btnResetAll
            // 
            this.btnResetAll.Location = new System.Drawing.Point(17, 470);
            this.btnResetAll.Name = "btnResetAll";
            this.btnResetAll.Size = new System.Drawing.Size(80, 23);
            this.btnResetAll.TabIndex = 11;
            this.btnResetAll.Text = "Reset All";
            this.btnResetAll.UseVisualStyleBackColor = true;
            this.btnResetAll.Click += new System.EventHandler(this.btnResetAll_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.usrSelectedVertexColor);
            this.groupBox4.Controls.Add(this.chkAutoSelect);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Location = new System.Drawing.Point(18, 216);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(222, 100);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Selected vertices";
            // 
            // usrSelectedVertexColor
            // 
            this.usrSelectedVertexColor.Color = System.Drawing.Color.White;
            this.usrSelectedVertexColor.Location = new System.Drawing.Point(87, 17);
            this.usrSelectedVertexColor.Name = "usrSelectedVertexColor";
            this.usrSelectedVertexColor.ShowButton = true;
            this.usrSelectedVertexColor.Size = new System.Drawing.Size(64, 32);
            this.usrSelectedVertexColor.TabIndex = 1;
            // 
            // chkAutoSelect
            // 
            this.chkAutoSelect.Location = new System.Drawing.Point(12, 54);
            this.chkAutoSelect.Name = "chkAutoSelect";
            this.chkAutoSelect.Size = new System.Drawing.Size(196, 38);
            this.chkAutoSelect.TabIndex = 2;
            this.chkAutoSelect.Text = "W&hen a vertex is clicked, select its incident edges";
            this.chkAutoSelect.UseVisualStyleBackColor = true;
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
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.usrPrimaryLabelFillColor);
            this.groupBox3.Controls.Add(this.usrVertexColor);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.lblVertexAlpha);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.nudVertexAlpha);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.nudVertexRadius);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.cbxVertexShape);
            this.groupBox3.Location = new System.Drawing.Point(18, 9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(222, 199);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Vertices";
            // 
            // usrPrimaryLabelFillColor
            // 
            this.usrPrimaryLabelFillColor.Color = System.Drawing.Color.White;
            this.usrPrimaryLabelFillColor.Location = new System.Drawing.Point(87, 159);
            this.usrPrimaryLabelFillColor.Name = "usrPrimaryLabelFillColor";
            this.usrPrimaryLabelFillColor.ShowButton = true;
            this.usrPrimaryLabelFillColor.Size = new System.Drawing.Size(64, 32);
            this.usrPrimaryLabelFillColor.TabIndex = 10;
            // 
            // usrVertexColor
            // 
            this.usrVertexColor.Color = System.Drawing.Color.White;
            this.usrVertexColor.Location = new System.Drawing.Point(87, 17);
            this.usrVertexColor.Name = "usrVertexColor";
            this.usrVertexColor.ShowButton = true;
            this.usrVertexColor.Size = new System.Drawing.Size(64, 32);
            this.usrVertexColor.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 35);
            this.label4.TabIndex = 9;
            this.label4.Text = "Primary label &fill color:";
            // 
            // lblVertexAlpha
            // 
            this.lblVertexAlpha.AutoSize = true;
            this.lblVertexAlpha.Location = new System.Drawing.Point(148, 129);
            this.lblVertexAlpha.Name = "lblVertexAlpha";
            this.lblVertexAlpha.Size = new System.Drawing.Size(15, 13);
            this.lblVertexAlpha.TabIndex = 8;
            this.lblVertexAlpha.Text = "%";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "O&pacity:";
            // 
            // nudVertexAlpha
            // 
            this.nudVertexAlpha.Location = new System.Drawing.Point(89, 127);
            this.nudVertexAlpha.Name = "nudVertexAlpha";
            this.nudVertexAlpha.Size = new System.Drawing.Size(56, 20);
            this.nudVertexAlpha.TabIndex = 7;
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
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 92);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Si&ze:";
            // 
            // nudVertexRadius
            // 
            this.nudVertexRadius.DecimalPlaces = 1;
            this.nudVertexRadius.Location = new System.Drawing.Point(89, 90);
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
            this.cbxVertexShape.Location = new System.Drawing.Point(89, 55);
            this.cbxVertexShape.Name = "cbxVertexShape";
            this.cbxVertexShape.Size = new System.Drawing.Size(119, 21);
            this.cbxVertexShape.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.usrSelectedEdgeColor);
            this.groupBox2.Controls.Add(this.label24);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.nudSelectedEdgeWidth);
            this.groupBox2.Location = new System.Drawing.Point(255, 216);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(237, 100);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Selected edges";
            // 
            // usrSelectedEdgeColor
            // 
            this.usrSelectedEdgeColor.Color = System.Drawing.Color.White;
            this.usrSelectedEdgeColor.Location = new System.Drawing.Point(73, 17);
            this.usrSelectedEdgeColor.Name = "usrSelectedEdgeColor";
            this.usrSelectedEdgeColor.ShowButton = true;
            this.usrSelectedEdgeColor.Size = new System.Drawing.Size(64, 32);
            this.usrSelectedEdgeColor.TabIndex = 1;
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
            // nudSelectedEdgeWidth
            // 
            this.nudSelectedEdgeWidth.DecimalPlaces = 1;
            this.nudSelectedEdgeWidth.Location = new System.Drawing.Point(74, 56);
            this.nudSelectedEdgeWidth.Name = "nudSelectedEdgeWidth";
            this.nudSelectedEdgeWidth.Size = new System.Drawing.Size(56, 20);
            this.nudSelectedEdgeWidth.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.usrEdgeColor);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.lblEdgeAlpha);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.nudEdgeAlpha);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.nudEdgeWidth);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.nudRelativeArrowSize);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Location = new System.Drawing.Point(256, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(236, 199);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Edges";
            // 
            // usrEdgeColor
            // 
            this.usrEdgeColor.Color = System.Drawing.Color.White;
            this.usrEdgeColor.Location = new System.Drawing.Point(72, 17);
            this.usrEdgeColor.Name = "usrEdgeColor";
            this.usrEdgeColor.ShowButton = true;
            this.usrEdgeColor.Size = new System.Drawing.Size(64, 32);
            this.usrEdgeColor.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(136, 89);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(90, 31);
            this.label11.TabIndex = 6;
            this.label11.Text = "(Directed graphs only)";
            // 
            // lblEdgeAlpha
            // 
            this.lblEdgeAlpha.AutoSize = true;
            this.lblEdgeAlpha.Location = new System.Drawing.Point(133, 129);
            this.lblEdgeAlpha.Name = "lblEdgeAlpha";
            this.lblEdgeAlpha.Size = new System.Drawing.Size(15, 13);
            this.lblEdgeAlpha.TabIndex = 9;
            this.lblEdgeAlpha.Text = "%";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 129);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(46, 13);
            this.label12.TabIndex = 7;
            this.label12.Text = "Opacit&y:";
            // 
            // nudEdgeAlpha
            // 
            this.nudEdgeAlpha.Location = new System.Drawing.Point(74, 127);
            this.nudEdgeAlpha.Name = "nudEdgeAlpha";
            this.nudEdgeAlpha.Size = new System.Drawing.Size(56, 20);
            this.nudEdgeAlpha.TabIndex = 8;
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
            this.nudEdgeWidth.DecimalPlaces = 1;
            this.nudEdgeWidth.Location = new System.Drawing.Point(74, 58);
            this.nudEdgeWidth.Name = "nudEdgeWidth";
            this.nudEdgeWidth.Size = new System.Drawing.Size(56, 20);
            this.nudEdgeWidth.TabIndex = 3;
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
            // nudRelativeArrowSize
            // 
            this.nudRelativeArrowSize.DecimalPlaces = 1;
            this.nudRelativeArrowSize.Location = new System.Drawing.Point(74, 90);
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
            this.label17.Text = "Arrow siz&e:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(255, 339);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "Bac&kground:";
            // 
            // usrBackColor
            // 
            this.usrBackColor.Color = System.Drawing.Color.White;
            this.usrBackColor.Location = new System.Drawing.Point(332, 329);
            this.usrBackColor.Name = "usrBackColor";
            this.usrBackColor.ShowButton = true;
            this.usrBackColor.Size = new System.Drawing.Size(64, 32);
            this.usrBackColor.TabIndex = 6;
            // 
            // chkAutoReadWorkbook
            // 
            this.chkAutoReadWorkbook.Location = new System.Drawing.Point(255, 405);
            this.chkAutoReadWorkbook.Name = "chkAutoReadWorkbook";
            this.chkAutoReadWorkbook.Size = new System.Drawing.Size(237, 52);
            this.chkAutoReadWorkbook.TabIndex = 10;
            this.chkAutoReadWorkbook.Text = "A&utomatically refresh the graph after setting a visual property in the Ribbon, a" +
                "pplying a scheme, or autofilling columns";
            this.toolTip1.SetToolTip(this.chkAutoReadWorkbook, resources.GetString("chkAutoReadWorkbook.ToolTip"));
            this.chkAutoReadWorkbook.UseVisualStyleBackColor = true;
            // 
            // btnAxisFont
            // 
            this.btnAxisFont.Location = new System.Drawing.Point(344, 372);
            this.btnAxisFont.Name = "btnAxisFont";
            this.btnAxisFont.Size = new System.Drawing.Size(80, 23);
            this.btnAxisFont.TabIndex = 9;
            this.btnAxisFont.Text = "A&xis Font...";
            this.btnAxisFont.UseVisualStyleBackColor = true;
            this.btnAxisFont.Click += new System.EventHandler(this.btnAxisFont_Click);
            // 
            // GeneralUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(509, 511);
            this.Controls.Add(this.btnAxisFont);
            this.Controls.Add(this.chkAutoReadWorkbook);
            this.Controls.Add(this.usrBackColor);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.btnLayout);
            this.Controls.Add(this.btnLabelFont);
            this.Controls.Add(this.btnResetAll);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GeneralUserSettingsDialog";
            this.Text = "Options";
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVertexAlpha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVertexRadius)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSelectedEdgeWidth)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEdgeAlpha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEdgeWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRelativeArrowSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudEdgeWidth;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown nudSelectedEdgeWidth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudVertexRadius;
        private System.Windows.Forms.Label label8;
        private Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus cbxVertexShape;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudRelativeArrowSize;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnResetAll;
        private System.Windows.Forms.Label lblVertexAlpha;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudVertexAlpha;
        private System.Windows.Forms.Label lblEdgeAlpha;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown nudEdgeAlpha;
        private System.Windows.Forms.CheckBox chkAutoSelect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnLabelFont;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnLayout;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnCustomizeVertexMenu;
        private Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker usrBackColor;
        private Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker usrVertexColor;
        private Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker usrEdgeColor;
        private Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker usrPrimaryLabelFillColor;
        private Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker usrSelectedVertexColor;
        private Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker usrSelectedEdgeColor;
        private System.Windows.Forms.CheckBox chkAutoReadWorkbook;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnAxisFont;
    }
}
