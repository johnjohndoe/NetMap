
namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class MaximumLabelLengthControl
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
            this.label3 = new System.Windows.Forms.Label();
            this.chkUseMaximumLabelLength = new System.Windows.Forms.CheckBox();
            this.nudMaximumLabelLength = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaximumLabelLength)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(84, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "characters";
            // 
            // chkUseMaximumLabelLength
            // 
            this.chkUseMaximumLabelLength.AutoSize = true;
            this.chkUseMaximumLabelLength.Location = new System.Drawing.Point(0, 0);
            this.chkUseMaximumLabelLength.Name = "chkUseMaximumLabelLength";
            this.chkUseMaximumLabelLength.Size = new System.Drawing.Size(155, 17);
            this.chkUseMaximumLabelLength.TabIndex = 0;
            this.chkUseMaximumLabelLength.Text = "Truncate labels longer than";
            this.chkUseMaximumLabelLength.UseVisualStyleBackColor = true;
            this.chkUseMaximumLabelLength.CheckedChanged += new System.EventHandler(this.chkUseMaximumLabelLength_CheckedChanged);
            // 
            // nudMaximumLabelLength
            // 
            this.nudMaximumLabelLength.Location = new System.Drawing.Point(21, 23);
            this.nudMaximumLabelLength.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudMaximumLabelLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMaximumLabelLength.Name = "nudMaximumLabelLength";
            this.nudMaximumLabelLength.Size = new System.Drawing.Size(60, 20);
            this.nudMaximumLabelLength.TabIndex = 1;
            this.nudMaximumLabelLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // MaximumLabelLengthControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkUseMaximumLabelLength);
            this.Controls.Add(this.nudMaximumLabelLength);
            this.Name = "MaximumLabelLengthControl";
            this.Size = new System.Drawing.Size(180, 52);
            ((System.ComponentModel.ISupportInitialize)(this.nudMaximumLabelLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkUseMaximumLabelLength;
        private System.Windows.Forms.NumericUpDown nudMaximumLabelLength;
    }
}
