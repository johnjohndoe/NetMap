
//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ApplicationUtil
{
    partial class GraphInfoViewer
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
            this.spcSplitContainer = new System.Windows.Forms.SplitContainer();
            this.lvwVertices = new Microsoft.Research.CommunityTechnologies.AppLib.ListViewPlus();
            this.lblVertexCount = new System.Windows.Forms.Label();
            this.lnkDefinitions = new System.Windows.Forms.LinkLabel();
            this.lvwEdges = new Microsoft.Research.CommunityTechnologies.AppLib.ListViewPlus();
            this.lblEdgeCount = new System.Windows.Forms.Label();
            this.spcSplitContainer.Panel1.SuspendLayout();
            this.spcSplitContainer.Panel2.SuspendLayout();
            this.spcSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // spcSplitContainer
            // 
            this.spcSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spcSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.spcSplitContainer.Name = "spcSplitContainer";
            this.spcSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spcSplitContainer.Panel1
            // 
            this.spcSplitContainer.Panel1.Controls.Add(this.lvwVertices);
            this.spcSplitContainer.Panel1.Controls.Add(this.lblVertexCount);
            // 
            // spcSplitContainer.Panel2
            // 
            this.spcSplitContainer.Panel2.Controls.Add(this.lnkDefinitions);
            this.spcSplitContainer.Panel2.Controls.Add(this.lvwEdges);
            this.spcSplitContainer.Panel2.Controls.Add(this.lblEdgeCount);
            this.spcSplitContainer.Panel2MinSize = 50;
            this.spcSplitContainer.Size = new System.Drawing.Size(366, 543);
            this.spcSplitContainer.SplitterDistance = 273;
            this.spcSplitContainer.TabIndex = 0;
            // 
            // lvwVertices
            // 
            this.lvwVertices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwVertices.FullRowSelect = true;
            this.lvwVertices.HideSelection = false;
            this.lvwVertices.Location = new System.Drawing.Point(16, 27);
            this.lvwVertices.Name = "lvwVertices";
            this.lvwVertices.Size = new System.Drawing.Size(333, 229);
            this.lvwVertices.TabIndex = 1;
            this.lvwVertices.UseCompatibleStateImageBehavior = false;
            this.lvwVertices.UseStandardContextMenu = false;
            this.lvwVertices.View = System.Windows.Forms.View.Details;
            this.lvwVertices.SelectedIndexChanged += new System.EventHandler(this.lvwVertices_SelectedIndexChanged);
            this.lvwVertices.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvwVertices_MouseUp);
            // 
            // lblVertexCount
            // 
            this.lblVertexCount.AutoSize = true;
            this.lblVertexCount.Location = new System.Drawing.Point(13, 9);
            this.lblVertexCount.Name = "lblVertexCount";
            this.lblVertexCount.Size = new System.Drawing.Size(90, 13);
            this.lblVertexCount.TabIndex = 0;
            this.lblVertexCount.Text = "(Gets set in code)";
            // 
            // lnkDefinitions
            // 
            this.lnkDefinitions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkDefinitions.AutoSize = true;
            this.lnkDefinitions.Location = new System.Drawing.Point(13, 246);
            this.lnkDefinitions.Name = "lnkDefinitions";
            this.lnkDefinitions.Size = new System.Drawing.Size(56, 13);
            this.lnkDefinitions.TabIndex = 4;
            this.lnkDefinitions.TabStop = true;
            this.lnkDefinitions.Text = "Definitions";
            this.lnkDefinitions.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDefinitions_LinkClicked);
            // 
            // lvwEdges
            // 
            this.lvwEdges.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwEdges.FullRowSelect = true;
            this.lvwEdges.HideSelection = false;
            this.lvwEdges.Location = new System.Drawing.Point(16, 27);
            this.lvwEdges.Name = "lvwEdges";
            this.lvwEdges.Size = new System.Drawing.Size(333, 203);
            this.lvwEdges.TabIndex = 1;
            this.lvwEdges.UseCompatibleStateImageBehavior = false;
            this.lvwEdges.UseStandardContextMenu = false;
            this.lvwEdges.View = System.Windows.Forms.View.Details;
            this.lvwEdges.SelectedIndexChanged += new System.EventHandler(this.lvwEdges_SelectedIndexChanged);
            this.lvwEdges.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvwEdges_MouseUp);
            // 
            // lblEdgeCount
            // 
            this.lblEdgeCount.AutoSize = true;
            this.lblEdgeCount.Location = new System.Drawing.Point(13, 9);
            this.lblEdgeCount.Name = "lblEdgeCount";
            this.lblEdgeCount.Size = new System.Drawing.Size(90, 13);
            this.lblEdgeCount.TabIndex = 0;
            this.lblEdgeCount.Text = "(Gets set in code)";
            // 
            // GraphInfoViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.spcSplitContainer);
            this.Name = "GraphInfoViewer";
            this.Size = new System.Drawing.Size(366, 543);
            this.spcSplitContainer.Panel1.ResumeLayout(false);
            this.spcSplitContainer.Panel1.PerformLayout();
            this.spcSplitContainer.Panel2.ResumeLayout(false);
            this.spcSplitContainer.Panel2.PerformLayout();
            this.spcSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer spcSplitContainer;
        private Microsoft.Research.CommunityTechnologies.AppLib.ListViewPlus lvwVertices;
        private System.Windows.Forms.Label lblVertexCount;
        private System.Windows.Forms.Label lblEdgeCount;
        private Microsoft.Research.CommunityTechnologies.AppLib.ListViewPlus lvwEdges;
        private System.Windows.Forms.LinkLabel lnkDefinitions;
    }
}
