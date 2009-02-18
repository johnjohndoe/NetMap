

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DynamicFilterDialog));
            this.toolStrip1 = new Microsoft.Research.CommunityTechnologies.AppLib.ToolStripPlus();
            this.tsbResetAllDynamicFilters = new System.Windows.Forms.ToolStripButton();
            this.tsbReadWorkbook = new System.Windows.Forms.ToolStripButton();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.grpEdgeFilters = new System.Windows.Forms.GroupBox();
            this.lblNoEdgeFilters = new System.Windows.Forms.Label();
            this.grpVertexFilters = new System.Windows.Forms.GroupBox();
            this.lblNoVertexFilters = new System.Windows.Forms.Label();
            this.pnlForScrollBars = new System.Windows.Forms.Panel();
            this.toolStrip1.SuspendLayout();
            this.grpEdgeFilters.SuspendLayout();
            this.grpVertexFilters.SuspendLayout();
            this.pnlForScrollBars.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ClickThrough = true;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbResetAllDynamicFilters,
            this.tsbReadWorkbook,
            this.tsbClose,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(382, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbResetAllDynamicFilters
            // 
            this.tsbResetAllDynamicFilters.Image = ((System.Drawing.Image)(resources.GetObject("tsbResetAllDynamicFilters.Image")));
            this.tsbResetAllDynamicFilters.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbResetAllDynamicFilters.Name = "tsbResetAllDynamicFilters";
            this.tsbResetAllDynamicFilters.Size = new System.Drawing.Size(101, 22);
            this.tsbResetAllDynamicFilters.Text = "Reset All Filters";
            this.tsbResetAllDynamicFilters.ToolTipText = "Reset all filters to their widest settings";
            this.tsbResetAllDynamicFilters.Click += new System.EventHandler(this.tsbResetAllDynamicFilters_Click);
            // 
            // tsbReadWorkbook
            // 
            this.tsbReadWorkbook.Image = ((System.Drawing.Image)(resources.GetObject("tsbReadWorkbook.Image")));
            this.tsbReadWorkbook.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReadWorkbook.Name = "tsbReadWorkbook";
            this.tsbReadWorkbook.Size = new System.Drawing.Size(141, 22);
            this.tsbReadWorkbook.Text = "Refresh from Workbook";
            this.tsbReadWorkbook.ToolTipText = "Refresh the filter list from the workbook after the workbook is edited";
            this.tsbReadWorkbook.Click += new System.EventHandler(this.tsbReadWorkbook_Click);
            // 
            // tsbClose
            // 
            this.tsbClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbClose.Image = ((System.Drawing.Image)(resources.GetObject("tsbClose.Image")));
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(53, 22);
            this.tsbClose.Text = "Close";
            this.tsbClose.ToolTipText = "Close the Dynamic Filters pane";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // grpEdgeFilters
            // 
            this.grpEdgeFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpEdgeFilters.Controls.Add(this.lblNoEdgeFilters);
            this.grpEdgeFilters.Location = new System.Drawing.Point(12, 7);
            this.grpEdgeFilters.MinimumSize = new System.Drawing.Size(250, 79);
            this.grpEdgeFilters.Name = "grpEdgeFilters";
            this.grpEdgeFilters.Size = new System.Drawing.Size(356, 85);
            this.grpEdgeFilters.TabIndex = 2;
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
            this.lblNoEdgeFilters.Size = new System.Drawing.Size(336, 49);
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
            this.grpVertexFilters.Size = new System.Drawing.Size(356, 85);
            this.grpVertexFilters.TabIndex = 3;
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
            this.lblNoVertexFilters.Size = new System.Drawing.Size(336, 49);
            this.lblNoVertexFilters.TabIndex = 1;
            this.lblNoVertexFilters.Text = "The Vertices worksheet doesn\'t have any numeric, date or time columns that can be" +
                " used for dynamic filtering.";
            // 
            // pnlForScrollBars
            // 
            this.pnlForScrollBars.AutoScroll = true;
            this.pnlForScrollBars.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlForScrollBars.Controls.Add(this.grpEdgeFilters);
            this.pnlForScrollBars.Controls.Add(this.grpVertexFilters);
            this.pnlForScrollBars.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlForScrollBars.Location = new System.Drawing.Point(0, 25);
            this.pnlForScrollBars.Name = "pnlForScrollBars";
            this.pnlForScrollBars.Size = new System.Drawing.Size(382, 196);
            this.pnlForScrollBars.TabIndex = 4;
            // 
            // DynamicFilterDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 221);
            this.Controls.Add(this.pnlForScrollBars);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(390, 249);
            this.Name = "DynamicFilterDialog";
            this.Text = "Dynamic Filters";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.grpEdgeFilters.ResumeLayout(false);
            this.grpVertexFilters.ResumeLayout(false);
            this.pnlForScrollBars.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Research.CommunityTechnologies.AppLib.ToolStripPlus toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbReadWorkbook;
        private System.Windows.Forms.ToolStripButton tsbResetAllDynamicFilters;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.GroupBox grpEdgeFilters;
        private System.Windows.Forms.GroupBox grpVertexFilters;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Label lblNoEdgeFilters;
        private System.Windows.Forms.Label lblNoVertexFilters;
        private System.Windows.Forms.Panel pnlForScrollBars;
    }
}
