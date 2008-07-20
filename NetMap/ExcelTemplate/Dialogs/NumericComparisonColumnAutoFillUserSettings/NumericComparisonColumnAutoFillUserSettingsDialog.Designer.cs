

//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NetMap.ExcelTemplate
{
    partial class NumericComparisonColumnAutoFillUserSettingsDialog
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
            this.txbSourceNumber = new System.Windows.Forms.TextBox();
            this.lblLabel1 = new System.Windows.Forms.Label();
            this.cbxComparisonOperator = new Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus();
            this.lblLabel2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txbSourceNumber
            // 
            this.txbSourceNumber.Location = new System.Drawing.Point(175, 29);
            this.txbSourceNumber.MaxLength = 50;
            this.txbSourceNumber.Name = "txbSourceNumber";
            this.txbSourceNumber.Size = new System.Drawing.Size(80, 20);
            this.txbSourceNumber.TabIndex = 2;
            // 
            // lblLabel1
            // 
            this.lblLabel1.AutoSize = true;
            this.lblLabel1.Location = new System.Drawing.Point(12, 9);
            this.lblLabel1.Name = "lblLabel1";
            this.lblLabel1.Size = new System.Drawing.Size(90, 13);
            this.lblLabel1.TabIndex = 0;
            this.lblLabel1.Text = "[Gets set in code]";
            // 
            // cbxComparisonOperator
            // 
            this.cbxComparisonOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxComparisonOperator.FormattingEnabled = true;
            this.cbxComparisonOperator.Location = new System.Drawing.Point(12, 29);
            this.cbxComparisonOperator.Name = "cbxComparisonOperator";
            this.cbxComparisonOperator.Size = new System.Drawing.Size(157, 21);
            this.cbxComparisonOperator.TabIndex = 1;
            // 
            // lblLabel2
            // 
            this.lblLabel2.AutoSize = true;
            this.lblLabel2.Location = new System.Drawing.Point(12, 64);
            this.lblLabel2.Name = "lblLabel2";
            this.lblLabel2.Size = new System.Drawing.Size(90, 13);
            this.lblLabel2.TabIndex = 3;
            this.lblLabel2.Text = "[Gets set in code]";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(294, 108);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(208, 108);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // NumericComparisonColumnAutoFillUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(386, 143);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txbSourceNumber);
            this.Controls.Add(this.lblLabel1);
            this.Controls.Add(this.cbxComparisonOperator);
            this.Controls.Add(this.lblLabel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NumericComparisonColumnAutoFillUserSettingsDialog";
            this.Text = "[Gets set in code]";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txbSourceNumber;
        private System.Windows.Forms.Label lblLabel1;
        private Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus cbxComparisonOperator;
        private System.Windows.Forms.Label lblLabel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}
