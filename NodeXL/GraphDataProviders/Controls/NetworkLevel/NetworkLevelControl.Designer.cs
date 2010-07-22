namespace Microsoft.NodeXL.GraphDataProviders
{
    partial class NetworkLevelControl
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
            this.picLevel = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbxLevel = new Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus();
            ((System.ComponentModel.ISupportInitialize)(this.picLevel)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // picLevel
            // 
            this.picLevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picLevel.Location = new System.Drawing.Point(63, 18);
            this.picLevel.Name = "picLevel";
            this.picLevel.Size = new System.Drawing.Size(39, 48);
            this.picLevel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picLevel.TabIndex = 14;
            this.picLevel.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbxLevel);
            this.groupBox3.Controls.Add(this.picLevel);
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(116, 78);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "&Levels to include";
            // 
            // cbxLevel
            // 
            this.cbxLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxLevel.FormattingEnabled = true;
            this.cbxLevel.Location = new System.Drawing.Point(11, 29);
            this.cbxLevel.Name = "cbxLevel";
            this.cbxLevel.Size = new System.Drawing.Size(44, 21);
            this.cbxLevel.TabIndex = 0;
            this.cbxLevel.SelectedIndexChanged += new System.EventHandler(this.cbxLevel_SelectedIndexChanged);
            // 
            // NetworkLevelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Name = "NetworkLevelControl";
            this.Size = new System.Drawing.Size(121, 81);
            ((System.ComponentModel.ISupportInitialize)(this.picLevel)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picLevel;
        private Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus cbxLevel;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}
