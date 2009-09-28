namespace Microsoft.NodeXL.GraphDataProviders
{
    partial class LimitToNPeopleControl
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
            this.label5 = new System.Windows.Forms.Label();
            this.chkLimitToNPeople = new System.Windows.Forms.CheckBox();
            this.cbxN = new Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(122, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "people";
            // 
            // chkLimitToNPeople
            // 
            this.chkLimitToNPeople.AutoSize = true;
            this.chkLimitToNPeople.Location = new System.Drawing.Point(0, 2);
            this.chkLimitToNPeople.Name = "chkLimitToNPeople";
            this.chkLimitToNPeople.Size = new System.Drawing.Size(59, 17);
            this.chkLimitToNPeople.TabIndex = 0;
            this.chkLimitToNPeople.Text = "Limi&t to";
            this.chkLimitToNPeople.UseVisualStyleBackColor = true;
            this.chkLimitToNPeople.CheckedChanged += new System.EventHandler(this.chkLimitToNPeople_CheckedChanged);
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
            // LimitToNPeopleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbxN);
            this.Controls.Add(this.chkLimitToNPeople);
            this.Name = "LimitToNPeopleControl";
            this.Size = new System.Drawing.Size(168, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus cbxN;
        private System.Windows.Forms.CheckBox chkLimitToNPeople;
    }
}
