
namespace Microsoft.Research.CommunityTechnologies.AppLib
{
    partial class ImageFormatControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label4 = new System.Windows.Forms.Label();
            this.cbxImageFormat = new Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus();
            this.grpImageSize = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nudImageWidthPx = new System.Windows.Forms.NumericUpDown();
            this.nudImageHeightPx = new System.Windows.Forms.NumericUpDown();
            this.grpImageSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudImageWidthPx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudImageHeightPx)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(182, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Image &format:";
            // 
            // cbxImageFormat
            // 
            this.cbxImageFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxImageFormat.FormattingEnabled = true;
            this.cbxImageFormat.Location = new System.Drawing.Point(185, 20);
            this.cbxImageFormat.Name = "cbxImageFormat";
            this.cbxImageFormat.Size = new System.Drawing.Size(88, 21);
            this.cbxImageFormat.TabIndex = 2;
            // 
            // grpImageSize
            // 
            this.grpImageSize.Controls.Add(this.label2);
            this.grpImageSize.Controls.Add(this.label3);
            this.grpImageSize.Controls.Add(this.nudImageWidthPx);
            this.grpImageSize.Controls.Add(this.nudImageHeightPx);
            this.grpImageSize.Location = new System.Drawing.Point(0, 0);
            this.grpImageSize.Name = "grpImageSize";
            this.grpImageSize.Size = new System.Drawing.Size(169, 88);
            this.grpImageSize.TabIndex = 0;
            this.grpImageSize.TabStop = false;
            this.grpImageSize.Text = "Image size (pixels)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "&Width:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "&Height:";
            // 
            // nudImageWidthPx
            // 
            this.nudImageWidthPx.Location = new System.Drawing.Point(68, 22);
            this.nudImageWidthPx.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudImageWidthPx.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudImageWidthPx.Name = "nudImageWidthPx";
            this.nudImageWidthPx.Size = new System.Drawing.Size(82, 20);
            this.nudImageWidthPx.TabIndex = 1;
            this.nudImageWidthPx.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // nudImageHeightPx
            // 
            this.nudImageHeightPx.Location = new System.Drawing.Point(68, 53);
            this.nudImageHeightPx.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudImageHeightPx.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudImageHeightPx.Name = "nudImageHeightPx";
            this.nudImageHeightPx.Size = new System.Drawing.Size(82, 20);
            this.nudImageHeightPx.TabIndex = 3;
            this.nudImageHeightPx.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // ImageFormatControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbxImageFormat);
            this.Controls.Add(this.grpImageSize);
            this.Name = "ImageFormatControl";
            this.Size = new System.Drawing.Size(282, 89);
            this.grpImageSize.ResumeLayout(false);
            this.grpImageSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudImageWidthPx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudImageHeightPx)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private ComboBoxPlus cbxImageFormat;
        private System.Windows.Forms.GroupBox grpImageSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudImageWidthPx;
        private System.Windows.Forms.NumericUpDown nudImageHeightPx;

    }
}
