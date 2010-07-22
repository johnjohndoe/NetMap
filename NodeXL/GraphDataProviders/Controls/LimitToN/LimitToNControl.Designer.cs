namespace Microsoft.NodeXL.GraphDataProviders
{
    partial class LimitToNControl
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
            this.lblObjectName = new System.Windows.Forms.Label();
            this.chkLimitToN = new System.Windows.Forms.CheckBox();
            this.cbxN = new Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus();
            this.SuspendLayout();
            // 
            // lblObjectName
            // 
            this.lblObjectName.AutoSize = true;
            this.lblObjectName.Location = new System.Drawing.Point(122, 3);
            this.lblObjectName.Name = "lblObjectName";
            this.lblObjectName.Size = new System.Drawing.Size(39, 13);
            this.lblObjectName.TabIndex = 2;
            this.lblObjectName.Text = "people";
            // 
            // chkLimitToN
            // 
            this.chkLimitToN.AutoSize = true;
            this.chkLimitToN.Location = new System.Drawing.Point(0, 2);
            this.chkLimitToN.Name = "chkLimitToN";
            this.chkLimitToN.Size = new System.Drawing.Size(59, 17);
            this.chkLimitToN.TabIndex = 0;
            this.chkLimitToN.Text = "Limi&t to";
            this.chkLimitToN.UseVisualStyleBackColor = true;
            this.chkLimitToN.CheckedChanged += new System.EventHandler(this.chkLimitToN_CheckedChanged);
            // 
            // cbxN
            // 
            this.cbxN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxN.Enabled = false;
            this.cbxN.FormattingEnabled = true;
            this.cbxN.Location = new System.Drawing.Point(60, 0);
            this.cbxN.Name = "cbxN";
            this.cbxN.Size = new System.Drawing.Size(56, 21);
            this.cbxN.TabIndex = 1;
            // 
            // LimitToNControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblObjectName);
            this.Controls.Add(this.cbxN);
            this.Controls.Add(this.chkLimitToN);
            this.Name = "LimitToNControl";
            this.Size = new System.Drawing.Size(168, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblObjectName;
        private Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus cbxN;
        private System.Windows.Forms.CheckBox chkLimitToN;
    }
}
