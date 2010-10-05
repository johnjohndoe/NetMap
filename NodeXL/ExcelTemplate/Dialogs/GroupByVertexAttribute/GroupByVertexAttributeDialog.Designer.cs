

//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class GroupByVertexAttributeDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbxVertexColumnName = new Microsoft.NodeXL.ExcelTemplate.VertexColumnComboBox();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.btnRemoveSelected = new System.Windows.Forms.Button();
            this.btnAddMinimumValue = new System.Windows.Forms.Button();
            this.lbxMinimumValues = new Microsoft.NodeXL.ExcelTemplate.MinimumValueListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.grpMinimumValues = new System.Windows.Forms.GroupBox();
            this.txbMinimumValueToAdd = new System.Windows.Forms.TextBox();
            this.dtpMinimumValueToAdd = new Microsoft.Research.CommunityTechnologies.AppLib.SimpleDateTimePicker();
            this.cbxVertexColumnFormat = new Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus();
            this.label2 = new System.Windows.Forms.Label();
            this.grpMinimumValues.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(407, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Group the graph\'s vertices using the values in this column on the Vertices works" +
                "heet:";
            // 
            // cbxVertexColumnName
            // 
            this.cbxVertexColumnName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxVertexColumnName.FormattingEnabled = true;
            this.cbxVertexColumnName.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cbxVertexColumnName.Location = new System.Drawing.Point(12, 27);
            this.cbxVertexColumnName.Name = "cbxVertexColumnName";
            this.cbxVertexColumnName.Size = new System.Drawing.Size(246, 21);
            this.cbxVertexColumnName.TabIndex = 1;
            this.cbxVertexColumnName.SelectedIndexChanged += new System.EventHandler(this.cbxVertexColumnName_SelectedIndexChanged);
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Location = new System.Drawing.Point(350, 78);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(96, 23);
            this.btnRemoveAll.TabIndex = 5;
            this.btnRemoveAll.Text = "Remo&ve All =>";
            this.btnRemoveAll.UseVisualStyleBackColor = true;
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.Location = new System.Drawing.Point(350, 49);
            this.btnRemoveSelected.Name = "btnRemoveSelected";
            this.btnRemoveSelected.Size = new System.Drawing.Size(96, 23);
            this.btnRemoveSelected.TabIndex = 4;
            this.btnRemoveSelected.Text = "&Remove =>";
            this.btnRemoveSelected.UseVisualStyleBackColor = true;
            this.btnRemoveSelected.Click += new System.EventHandler(this.btnRemoveSelected_Click);
            // 
            // btnAddMinimumValue
            // 
            this.btnAddMinimumValue.Location = new System.Drawing.Point(83, 66);
            this.btnAddMinimumValue.Name = "btnAddMinimumValue";
            this.btnAddMinimumValue.Size = new System.Drawing.Size(75, 23);
            this.btnAddMinimumValue.TabIndex = 2;
            this.btnAddMinimumValue.Text = "&Add =>";
            this.btnAddMinimumValue.UseVisualStyleBackColor = true;
            this.btnAddMinimumValue.Click += new System.EventHandler(this.btnAddMinimumValue_Click);
            // 
            // lbxMinimumValues
            // 
            this.lbxMinimumValues.FormattingEnabled = true;
            this.lbxMinimumValues.Location = new System.Drawing.Point(168, 20);
            this.lbxMinimumValues.Name = "lbxMinimumValues";
            this.lbxMinimumValues.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbxMinimumValues.Size = new System.Drawing.Size(171, 108);
            this.lbxMinimumValues.TabIndex = 3;
            this.lbxMinimumValues.SelectedIndexChanged += new System.EventHandler(this.lbxMinimumValues_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(398, 272);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(74, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(318, 272);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(74, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // grpMinimumValues
            // 
            this.grpMinimumValues.Controls.Add(this.txbMinimumValueToAdd);
            this.grpMinimumValues.Controls.Add(this.dtpMinimumValueToAdd);
            this.grpMinimumValues.Controls.Add(this.btnRemoveAll);
            this.grpMinimumValues.Controls.Add(this.lbxMinimumValues);
            this.grpMinimumValues.Controls.Add(this.btnRemoveSelected);
            this.grpMinimumValues.Controls.Add(this.btnAddMinimumValue);
            this.grpMinimumValues.Location = new System.Drawing.Point(12, 112);
            this.grpMinimumValues.Name = "grpMinimumValues";
            this.grpMinimumValues.Size = new System.Drawing.Size(460, 146);
            this.grpMinimumValues.TabIndex = 4;
            this.grpMinimumValues.TabStop = false;
            this.grpMinimumValues.Text = "&Start new groups at these values:";
            // 
            // txbMinimumValueToAdd
            // 
            this.txbMinimumValueToAdd.Location = new System.Drawing.Point(10, 44);
            this.txbMinimumValueToAdd.MaxLength = 20;
            this.txbMinimumValueToAdd.Name = "txbMinimumValueToAdd";
            this.txbMinimumValueToAdd.Size = new System.Drawing.Size(148, 20);
            this.txbMinimumValueToAdd.TabIndex = 1;
            // 
            // dtpMinimumValueToAdd
            // 
            this.dtpMinimumValueToAdd.CustomFormat = "h:mm tt";
            this.dtpMinimumValueToAdd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMinimumValueToAdd.Location = new System.Drawing.Point(10, 40);
            this.dtpMinimumValueToAdd.Name = "dtpMinimumValueToAdd";
            this.dtpMinimumValueToAdd.ShowUpDown = true;
            this.dtpMinimumValueToAdd.SimpleFormat = Microsoft.Research.CommunityTechnologies.AppLib.SimpleDateTimeFormat.Time;
            this.dtpMinimumValueToAdd.Size = new System.Drawing.Size(148, 20);
            this.dtpMinimumValueToAdd.TabIndex = 0;
            // 
            // cbxVertexColumnFormat
            // 
            this.cbxVertexColumnFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxVertexColumnFormat.FormattingEnabled = true;
            this.cbxVertexColumnFormat.Location = new System.Drawing.Point(12, 79);
            this.cbxVertexColumnFormat.Name = "cbxVertexColumnFormat";
            this.cbxVertexColumnFormat.Size = new System.Drawing.Size(121, 21);
            this.cbxVertexColumnFormat.TabIndex = 3;
            this.cbxVertexColumnFormat.SelectedIndexChanged += new System.EventHandler(this.cbxVertexColumnFormat_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "The &column\'s values are:";
            // 
            // GroupByVertexAttributeDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(484, 307);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbxVertexColumnFormat);
            this.Controls.Add(this.grpMinimumValues);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbxVertexColumnName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GroupByVertexAttributeDialog";
            this.Text = "Group by Vertex Attribute";
            this.grpMinimumValues.ResumeLayout(false);
            this.grpMinimumValues.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.NodeXL.ExcelTemplate.MinimumValueListBox lbxMinimumValues;
        private System.Windows.Forms.Button btnRemoveAll;
        private System.Windows.Forms.Button btnRemoveSelected;
        private System.Windows.Forms.Button btnAddMinimumValue;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private Microsoft.NodeXL.ExcelTemplate.VertexColumnComboBox cbxVertexColumnName;
        private System.Windows.Forms.GroupBox grpMinimumValues;
        private System.Windows.Forms.Label label1;
        private Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus cbxVertexColumnFormat;
        private System.Windows.Forms.Label label2;
        private Microsoft.Research.CommunityTechnologies.AppLib.SimpleDateTimePicker dtpMinimumValueToAdd;
        private System.Windows.Forms.TextBox txbMinimumValueToAdd;
    }
}
