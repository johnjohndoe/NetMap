

//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class VertexShapeColumnAutoFillUserSettingsDialog
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
            this.label2 = new System.Windows.Forms.Label();
            this.cbxVertexShape = new Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus();
            this.label1 = new System.Windows.Forms.Label();
            this.txbSourceNumber = new System.Windows.Forms.TextBox();
            this.lblLabel1 = new System.Windows.Forms.Label();
            this.cbxComparisonOperator = new Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(182, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Otherwise, don\'t set the vertex shape";
            // 
            // cbxVertexShape
            // 
            this.cbxVertexShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxVertexShape.FormattingEnabled = true;
            this.cbxVertexShape.Location = new System.Drawing.Point(12, 82);
            this.cbxVertexShape.Name = "cbxVertexShape";
            this.cbxVertexShape.Size = new System.Drawing.Size(86, 21);
            this.cbxVertexShape.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Then &set the vertex shape to:";
            // 
            // txbSourceNumber
            // 
            this.txbSourceNumber.Location = new System.Drawing.Point(175, 32);
            this.txbSourceNumber.MaxLength = 50;
            this.txbSourceNumber.Name = "txbSourceNumber";
            this.txbSourceNumber.Size = new System.Drawing.Size(72, 20);
            this.txbSourceNumber.TabIndex = 2;
            // 
            // lblLabel1
            // 
            this.lblLabel1.AutoSize = true;
            this.lblLabel1.Location = new System.Drawing.Point(12, 12);
            this.lblLabel1.Name = "lblLabel1";
            this.lblLabel1.Size = new System.Drawing.Size(154, 13);
            this.lblLabel1.TabIndex = 0;
            this.lblLabel1.Text = "&If the source column number is:";
            // 
            // cbxComparisonOperator
            // 
            this.cbxComparisonOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxComparisonOperator.FormattingEnabled = true;
            this.cbxComparisonOperator.Location = new System.Drawing.Point(12, 32);
            this.cbxComparisonOperator.Name = "cbxComparisonOperator";
            this.cbxComparisonOperator.Size = new System.Drawing.Size(157, 21);
            this.cbxComparisonOperator.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(167, 154);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(81, 154);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // VertexShapeColumnAutoFillUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(261, 195);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbxVertexShape);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txbSourceNumber);
            this.Controls.Add(this.lblLabel1);
            this.Controls.Add(this.cbxComparisonOperator);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VertexShapeColumnAutoFillUserSettingsDialog";
            this.Text = "Vertex Shape Details";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus cbxVertexShape;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbSourceNumber;
        private System.Windows.Forms.Label lblLabel1;
        private Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus cbxComparisonOperator;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}
