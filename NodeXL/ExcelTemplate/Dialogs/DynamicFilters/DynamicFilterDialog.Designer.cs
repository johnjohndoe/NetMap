

//  Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class DynamicFilterDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DynamicFilterDialog));
            this.grpEdgeFilters = new System.Windows.Forms.GroupBox();
            this.lblNoEdgeFilters = new System.Windows.Forms.Label();
            this.grpVertexFilters = new System.Windows.Forms.GroupBox();
            this.lblNoVertexFilters = new System.Windows.Forms.Label();
            this.pnlForScrollBars = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnReadWorkbook = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnResetAllDynamicFilters = new System.Windows.Forms.Button();
            this.lblFilteredAlpha = new System.Windows.Forms.Label();
            this.nudFilteredAlpha = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.grpEdgeFilters.SuspendLayout();
            this.grpVertexFilters.SuspendLayout();
            this.pnlForScrollBars.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilteredAlpha)).BeginInit();
            this.SuspendLayout();
            // 
            // grpEdgeFilters
            // 
            this.grpEdgeFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpEdgeFilters.Controls.Add(this.lblNoEdgeFilters);
            this.grpEdgeFilters.Location = new System.Drawing.Point(12, 7);
            this.grpEdgeFilters.MinimumSize = new System.Drawing.Size(250, 79);
            this.grpEdgeFilters.Name = "grpEdgeFilters";
            this.grpEdgeFilters.Size = new System.Drawing.Size(432, 85);
            this.grpEdgeFilters.TabIndex = 0;
            this.grpEdgeFilters.TabStop = false;
            this.grpEdgeFilters.Text = "Edge Filters";
            this.grpEdgeFilters.Visible = false;
            // 
            // lblNoEdgeFilters
            // 
            this.lblNoEdgeFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNoEdgeFilters.Location = new System.Drawing.Point(10, 20);
            this.lblNoEdgeFilters.Name = "lblNoEdgeFilters";
            this.lblNoEdgeFilters.Size = new System.Drawing.Size(412, 49);
            this.lblNoEdgeFilters.TabIndex = 0;
            this.lblNoEdgeFilters.Text = "The Edges worksheet doesn\'t have any numeric, date or time columns that can be us" +
                "ed for dynamic filtering.";
            // 
            // grpVertexFilters
            // 
            this.grpVertexFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpVertexFilters.Controls.Add(this.lblNoVertexFilters);
            this.grpVertexFilters.Location = new System.Drawing.Point(12, 97);
            this.grpVertexFilters.MinimumSize = new System.Drawing.Size(250, 79);
            this.grpVertexFilters.Name = "grpVertexFilters";
            this.grpVertexFilters.Size = new System.Drawing.Size(432, 85);
            this.grpVertexFilters.TabIndex = 1;
            this.grpVertexFilters.TabStop = false;
            this.grpVertexFilters.Text = "Vertex Filters";
            this.grpVertexFilters.Visible = false;
            // 
            // lblNoVertexFilters
            // 
            this.lblNoVertexFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNoVertexFilters.Location = new System.Drawing.Point(10, 20);
            this.lblNoVertexFilters.Name = "lblNoVertexFilters";
            this.lblNoVertexFilters.Size = new System.Drawing.Size(412, 49);
            this.lblNoVertexFilters.TabIndex = 0;
            this.lblNoVertexFilters.Text = "The Vertices worksheet doesn\'t have any numeric, date or time columns that can be" +
                " used for dynamic filtering.";
            // 
            // pnlForScrollBars
            // 
            this.pnlForScrollBars.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlForScrollBars.AutoScroll = true;
            this.pnlForScrollBars.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlForScrollBars.Controls.Add(this.grpEdgeFilters);
            this.pnlForScrollBars.Controls.Add(this.grpVertexFilters);
            this.pnlForScrollBars.Location = new System.Drawing.Point(0, 0);
            this.pnlForScrollBars.Name = "pnlForScrollBars";
            this.pnlForScrollBars.Size = new System.Drawing.Size(458, 196);
            this.pnlForScrollBars.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(370, 209);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.toolTip1.SetToolTip(this.btnClose, "Close the Dynamic Filters pane");
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnReadWorkbook
            // 
            this.btnReadWorkbook.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReadWorkbook.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReadWorkbook.ImageIndex = 0;
            this.btnReadWorkbook.ImageList = this.imageList1;
            this.btnReadWorkbook.Location = new System.Drawing.Point(249, 209);
            this.btnReadWorkbook.Name = "btnReadWorkbook";
            this.btnReadWorkbook.Size = new System.Drawing.Size(115, 23);
            this.btnReadWorkbook.TabIndex = 5;
            this.btnReadWorkbook.Text = "Read &Workbook";
            this.btnReadWorkbook.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.btnReadWorkbook, "Refresh the filter list from the workbook after the workbook is edited");
            this.btnReadWorkbook.UseVisualStyleBackColor = true;
            this.btnReadWorkbook.Click += new System.EventHandler(this.btnReadWorkbook_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList1.Images.SetKeyName(0, "Table.bmp");
            // 
            // btnResetAllDynamicFilters
            // 
            this.btnResetAllDynamicFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetAllDynamicFilters.Location = new System.Drawing.Point(168, 209);
            this.btnResetAllDynamicFilters.Name = "btnResetAllDynamicFilters";
            this.btnResetAllDynamicFilters.Size = new System.Drawing.Size(75, 23);
            this.btnResetAllDynamicFilters.TabIndex = 4;
            this.btnResetAllDynamicFilters.Text = "Reset &All";
            this.toolTip1.SetToolTip(this.btnResetAllDynamicFilters, "Reset all filters to their widest settings");
            this.btnResetAllDynamicFilters.UseVisualStyleBackColor = true;
            this.btnResetAllDynamicFilters.Click += new System.EventHandler(this.btnResetAllDynamicFilters_Click);
            // 
            // lblFilteredAlpha
            // 
            this.lblFilteredAlpha.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFilteredAlpha.AutoSize = true;
            this.lblFilteredAlpha.Location = new System.Drawing.Point(146, 212);
            this.lblFilteredAlpha.Name = "lblFilteredAlpha";
            this.lblFilteredAlpha.Size = new System.Drawing.Size(15, 13);
            this.lblFilteredAlpha.TabIndex = 3;
            this.lblFilteredAlpha.Text = "%";
            // 
            // nudFilteredAlpha
            // 
            this.nudFilteredAlpha.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudFilteredAlpha.Location = new System.Drawing.Point(87, 210);
            this.nudFilteredAlpha.Name = "nudFilteredAlpha";
            this.nudFilteredAlpha.Size = new System.Drawing.Size(56, 20);
            this.nudFilteredAlpha.TabIndex = 2;
            this.nudFilteredAlpha.ValueChanged += new System.EventHandler(this.nudFilteredAlpha_ValueChanged);
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(10, 212);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(69, 13);
            this.label13.TabIndex = 1;
            this.label13.Text = "Filter &opacity:";
            // 
            // DynamicFilterDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(458, 244);
            this.Controls.Add(this.lblFilteredAlpha);
            this.Controls.Add(this.nudFilteredAlpha);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.btnResetAllDynamicFilters);
            this.Controls.Add(this.btnReadWorkbook);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlForScrollBars);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(466, 272);
            this.Name = "DynamicFilterDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Dynamic Filters";
            this.grpEdgeFilters.ResumeLayout(false);
            this.grpVertexFilters.ResumeLayout(false);
            this.pnlForScrollBars.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudFilteredAlpha)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpEdgeFilters;
        private System.Windows.Forms.GroupBox grpVertexFilters;
        private System.Windows.Forms.Label lblNoEdgeFilters;
        private System.Windows.Forms.Label lblNoVertexFilters;
        private System.Windows.Forms.Panel pnlForScrollBars;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnReadWorkbook;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnResetAllDynamicFilters;
        private System.Windows.Forms.Label lblFilteredAlpha;
        private System.Windows.Forms.NumericUpDown nudFilteredAlpha;
        private System.Windows.Forms.Label label13;
    }
}
