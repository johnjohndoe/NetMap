namespace Microsoft.NetMap.ExcelTemplate
{
    partial class SelectSubgraphsDialog
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radUseClickedVertexOnly = new System.Windows.Forms.RadioButton();
            this.radUseAllSelectedVertices = new System.Windows.Forms.RadioButton();
            this.cbxSelectConnectingEdges = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.usrSubgraphLevels = new Microsoft.NetMap.ExcelTemplate.SubgraphLevelsControl();
            this.lnkHelp = new Microsoft.NetMap.ExcelTemplate.SubgraphLinkLabel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radUseClickedVertexOnly);
            this.groupBox1.Controls.Add(this.radUseAllSelectedVertices);
            this.groupBox1.Location = new System.Drawing.Point(12, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(205, 74);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select subgraphs for these vertices";
            // 
            // radUseClickedVertexOnly
            // 
            this.radUseClickedVertexOnly.AutoSize = true;
            this.radUseClickedVertexOnly.Location = new System.Drawing.Point(13, 42);
            this.radUseClickedVertexOnly.Name = "radUseClickedVertexOnly";
            this.radUseClickedVertexOnly.Size = new System.Drawing.Size(135, 17);
            this.radUseClickedVertexOnly.TabIndex = 1;
            this.radUseClickedVertexOnly.TabStop = true;
            this.radUseClickedVertexOnly.Text = "The &clicked vertex only";
            this.radUseClickedVertexOnly.UseVisualStyleBackColor = true;
            // 
            // radUseAllSelectedVertices
            // 
            this.radUseAllSelectedVertices.AutoSize = true;
            this.radUseAllSelectedVertices.Location = new System.Drawing.Point(13, 19);
            this.radUseAllSelectedVertices.Name = "radUseAllSelectedVertices";
            this.radUseAllSelectedVertices.Size = new System.Drawing.Size(119, 17);
            this.radUseAllSelectedVertices.TabIndex = 0;
            this.radUseAllSelectedVertices.TabStop = true;
            this.radUseAllSelectedVertices.Text = "&All selected vertices";
            this.radUseAllSelectedVertices.UseVisualStyleBackColor = true;
            // 
            // cbxSelectConnectingEdges
            // 
            this.cbxSelectConnectingEdges.AutoSize = true;
            this.cbxSelectConnectingEdges.Location = new System.Drawing.Point(12, 148);
            this.cbxSelectConnectingEdges.Name = "cbxSelectConnectingEdges";
            this.cbxSelectConnectingEdges.Size = new System.Drawing.Size(162, 17);
            this.cbxSelectConnectingEdges.TabIndex = 3;
            this.cbxSelectConnectingEdges.Text = "&Select the connecting edges";
            this.cbxSelectConnectingEdges.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(268, 179);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(71, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(191, 179);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(71, 23);
            this.btnSelect.TabIndex = 4;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // usrSubgraphLevels
            // 
            this.usrSubgraphLevels.Levels = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.usrSubgraphLevels.Location = new System.Drawing.Point(12, 89);
            this.usrSubgraphLevels.Name = "usrSubgraphLevels";
            this.usrSubgraphLevels.Size = new System.Drawing.Size(319, 53);
            this.usrSubgraphLevels.TabIndex = 2;
            // 
            // lnkHelp
            // 
            this.lnkHelp.Location = new System.Drawing.Point(226, 9);
            this.lnkHelp.Name = "lnkHelp";
            this.lnkHelp.Size = new System.Drawing.Size(113, 23);
            this.lnkHelp.TabIndex = 1;
            this.lnkHelp.TabStop = true;
            this.lnkHelp.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // SelectSubgraphsDialog
            // 
            this.AcceptButton = this.btnSelect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(351, 214);
            this.Controls.Add(this.lnkHelp);
            this.Controls.Add(this.usrSubgraphLevels);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.cbxSelectConnectingEdges);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectSubgraphsDialog";
            this.Text = "Select Subgraphs";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radUseClickedVertexOnly;
        private System.Windows.Forms.RadioButton radUseAllSelectedVertices;
        private System.Windows.Forms.CheckBox cbxSelectConnectingEdges;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSelect;
        private SubgraphLevelsControl usrSubgraphLevels;
        private Microsoft.NetMap.ExcelTemplate.SubgraphLinkLabel lnkHelp;
    }
}
