

//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class BackgroundDialog
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.usrBackColor = new Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker();
            this.radUseBackColor = new System.Windows.Forms.RadioButton();
            this.radUseDefaultBackColor = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txbImageUri = new System.Windows.Forms.TextBox();
            this.radUseImage = new System.Windows.Forms.RadioButton();
            this.radNoImage = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(262, 257);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(176, 257);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.usrBackColor);
            this.groupBox1.Controls.Add(this.radUseBackColor);
            this.groupBox1.Controls.Add(this.radUseDefaultBackColor);
            this.groupBox1.Location = new System.Drawing.Point(15, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(330, 126);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Background color";
            // 
            // usrBackColor
            // 
            this.usrBackColor.Color = System.Drawing.Color.White;
            this.usrBackColor.Location = new System.Drawing.Point(36, 82);
            this.usrBackColor.Name = "usrBackColor";
            this.usrBackColor.ShowButton = true;
            this.usrBackColor.Size = new System.Drawing.Size(64, 32);
            this.usrBackColor.TabIndex = 2;
            // 
            // radUseBackColor
            // 
            this.radUseBackColor.AutoSize = true;
            this.radUseBackColor.Location = new System.Drawing.Point(15, 59);
            this.radUseBackColor.Name = "radUseBackColor";
            this.radUseBackColor.Size = new System.Drawing.Size(152, 17);
            this.radUseBackColor.TabIndex = 1;
            this.radUseBackColor.TabStop = true;
            this.radUseBackColor.Text = "Use &this background color:";
            this.radUseBackColor.UseVisualStyleBackColor = true;
            this.radUseBackColor.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // radUseDefaultBackColor
            // 
            this.radUseDefaultBackColor.Location = new System.Drawing.Point(15, 19);
            this.radUseDefaultBackColor.Name = "radUseDefaultBackColor";
            this.radUseDefaultBackColor.Size = new System.Drawing.Size(302, 34);
            this.radUseDefaultBackColor.TabIndex = 0;
            this.radUseDefaultBackColor.TabStop = true;
            this.radUseDefaultBackColor.Text = "Use the &default background color specified in Options (at the top of the graph p" +
                "ane)";
            this.radUseDefaultBackColor.UseVisualStyleBackColor = true;
            this.radUseDefaultBackColor.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnBrowse);
            this.groupBox2.Controls.Add(this.txbImageUri);
            this.groupBox2.Controls.Add(this.radUseImage);
            this.groupBox2.Controls.Add(this.radNoImage);
            this.groupBox2.Location = new System.Drawing.Point(15, 144);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(330, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Background image";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(242, 64);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "&Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txbImageUri
            // 
            this.txbImageUri.Location = new System.Drawing.Point(36, 65);
            this.txbImageUri.MaxLength = 256;
            this.txbImageUri.Name = "txbImageUri";
            this.txbImageUri.Size = new System.Drawing.Size(198, 20);
            this.txbImageUri.TabIndex = 2;
            // 
            // radUseImage
            // 
            this.radUseImage.AutoSize = true;
            this.radUseImage.Location = new System.Drawing.Point(15, 42);
            this.radUseImage.Name = "radUseImage";
            this.radUseImage.Size = new System.Drawing.Size(113, 17);
            this.radUseImage.TabIndex = 1;
            this.radUseImage.TabStop = true;
            this.radUseImage.Text = "Use this &image file:";
            this.radUseImage.UseVisualStyleBackColor = true;
            this.radUseImage.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // radNoImage
            // 
            this.radNoImage.AutoSize = true;
            this.radNoImage.Location = new System.Drawing.Point(15, 19);
            this.radNoImage.Name = "radNoImage";
            this.radNoImage.Size = new System.Drawing.Size(51, 17);
            this.radNoImage.TabIndex = 0;
            this.radNoImage.TabStop = true;
            this.radNoImage.Text = "&None";
            this.radNoImage.UseVisualStyleBackColor = true;
            this.radNoImage.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // BackgroundDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(361, 294);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BackgroundDialog";
            this.Text = "Background";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radUseBackColor;
        private System.Windows.Forms.RadioButton radUseDefaultBackColor;
        private Microsoft.Research.CommunityTechnologies.AppLib.ColorPicker usrBackColor;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radUseImage;
        private System.Windows.Forms.RadioButton radNoImage;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txbImageUri;
    }
}
