
//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.MultithreadingTests
{
    partial class MainForm
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
            this.btnLayOutGraphAsync = new System.Windows.Forms.Button();
            this.picGraph = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAsyncGraphDrawerDrawAsync = new System.Windows.Forms.Button();
            this.txbStatus = new Microsoft.Research.CommunityTechnologies.AppLib.TextBoxTraceListener();
            this.btnLayOutGraphAsyncCancel = new System.Windows.Forms.Button();
            this.btnAsyncGraphDrawerDrawAsyncCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnMultiSelectionGraphDrawerDrawAsyncCancel = new System.Windows.Forms.Button();
            this.bntMultiSelectionGraphDrawerDrawAsync = new System.Windows.Forms.Button();
            this.chkDrawSelection = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picGraph)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 240);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Status:";
            // 
            // btnLayOutGraphAsync
            // 
            this.btnLayOutGraphAsync.Location = new System.Drawing.Point(15, 28);
            this.btnLayOutGraphAsync.Name = "btnLayOutGraphAsync";
            this.btnLayOutGraphAsync.Size = new System.Drawing.Size(161, 23);
            this.btnLayOutGraphAsync.TabIndex = 0;
            this.btnLayOutGraphAsync.Text = "LayOutGraphAsync";
            this.btnLayOutGraphAsync.UseVisualStyleBackColor = true;
            this.btnLayOutGraphAsync.Click += new System.EventHandler(this.btnLayOutGraphAsync_Click);
            // 
            // picGraph
            // 
            this.picGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.picGraph.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picGraph.Location = new System.Drawing.Point(416, 256);
            this.picGraph.Name = "picGraph";
            this.picGraph.Size = new System.Drawing.Size(316, 249);
            this.picGraph.TabIndex = 3;
            this.picGraph.TabStop = false;
            this.picGraph.Resize += new System.EventHandler(this.picGraph_Resize);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(413, 240);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Graph:";
            // 
            // btnAsyncGraphDrawerDrawAsync
            // 
            this.btnAsyncGraphDrawerDrawAsync.Location = new System.Drawing.Point(15, 28);
            this.btnAsyncGraphDrawerDrawAsync.Name = "btnAsyncGraphDrawerDrawAsync";
            this.btnAsyncGraphDrawerDrawAsync.Size = new System.Drawing.Size(161, 23);
            this.btnAsyncGraphDrawerDrawAsync.TabIndex = 0;
            this.btnAsyncGraphDrawerDrawAsync.Text = "DrawAsync";
            this.btnAsyncGraphDrawerDrawAsync.UseVisualStyleBackColor = true;
            this.btnAsyncGraphDrawerDrawAsync.Click += new System.EventHandler(this.btnAsyncGraphDrawerDrawAsync_Click);
            // 
            // txbStatus
            // 
            this.txbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.txbStatus.Filter = null;
            this.txbStatus.Location = new System.Drawing.Point(12, 256);
            this.txbStatus.MaxLength = 0;
            this.txbStatus.MaxLines = 100;
            this.txbStatus.Multiline = true;
            this.txbStatus.Name = "txbStatus";
            this.txbStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbStatus.Size = new System.Drawing.Size(387, 249);
            this.txbStatus.TabIndex = 4;
            // 
            // btnLayOutGraphAsyncCancel
            // 
            this.btnLayOutGraphAsyncCancel.Location = new System.Drawing.Point(15, 66);
            this.btnLayOutGraphAsyncCancel.Name = "btnLayOutGraphAsyncCancel";
            this.btnLayOutGraphAsyncCancel.Size = new System.Drawing.Size(161, 23);
            this.btnLayOutGraphAsyncCancel.TabIndex = 1;
            this.btnLayOutGraphAsyncCancel.Text = "LayOutGraphAsyncCancel";
            this.btnLayOutGraphAsyncCancel.UseVisualStyleBackColor = true;
            this.btnLayOutGraphAsyncCancel.Click += new System.EventHandler(this.btnLayOutGraphAsyncCancel_Click);
            // 
            // btnAsyncGraphDrawerDrawAsyncCancel
            // 
            this.btnAsyncGraphDrawerDrawAsyncCancel.Location = new System.Drawing.Point(15, 66);
            this.btnAsyncGraphDrawerDrawAsyncCancel.Name = "btnAsyncGraphDrawerDrawAsyncCancel";
            this.btnAsyncGraphDrawerDrawAsyncCancel.Size = new System.Drawing.Size(161, 23);
            this.btnAsyncGraphDrawerDrawAsyncCancel.TabIndex = 1;
            this.btnAsyncGraphDrawerDrawAsyncCancel.Text = "DrawAsyncCancel";
            this.btnAsyncGraphDrawerDrawAsyncCancel.UseVisualStyleBackColor = true;
            this.btnAsyncGraphDrawerDrawAsyncCancel.Click += new System.EventHandler(this.btnAsyncGraphDrawerDrawAsyncCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLayOutGraphAsync);
            this.groupBox1.Controls.Add(this.btnLayOutGraphAsyncCancel);
            this.groupBox1.Location = new System.Drawing.Point(15, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(188, 104);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "KamadaKawaiiLayout";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnAsyncGraphDrawerDrawAsync);
            this.groupBox2.Controls.Add(this.btnAsyncGraphDrawerDrawAsyncCancel);
            this.groupBox2.Location = new System.Drawing.Point(215, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(188, 104);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "AsyncGraphDrawer";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkDrawSelection);
            this.groupBox3.Controls.Add(this.btnMultiSelectionGraphDrawerDrawAsyncCancel);
            this.groupBox3.Controls.Add(this.bntMultiSelectionGraphDrawerDrawAsync);
            this.groupBox3.Location = new System.Drawing.Point(415, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(188, 135);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "MultiSelectionGraphDrawer";
            // 
            // btnMultiSelectionGraphDrawerDrawAsyncCancel
            // 
            this.btnMultiSelectionGraphDrawerDrawAsyncCancel.Location = new System.Drawing.Point(15, 66);
            this.btnMultiSelectionGraphDrawerDrawAsyncCancel.Name = "btnMultiSelectionGraphDrawerDrawAsyncCancel";
            this.btnMultiSelectionGraphDrawerDrawAsyncCancel.Size = new System.Drawing.Size(161, 23);
            this.btnMultiSelectionGraphDrawerDrawAsyncCancel.TabIndex = 1;
            this.btnMultiSelectionGraphDrawerDrawAsyncCancel.Text = "DrawAsyncCancel";
            this.btnMultiSelectionGraphDrawerDrawAsyncCancel.UseVisualStyleBackColor = true;
            this.btnMultiSelectionGraphDrawerDrawAsyncCancel.Click += new System.EventHandler(this.btnMultiSelectionGraphDrawerDrawAsyncCancel_Click);
            // 
            // bntMultiSelectionGraphDrawerDrawAsync
            // 
            this.bntMultiSelectionGraphDrawerDrawAsync.Location = new System.Drawing.Point(15, 28);
            this.bntMultiSelectionGraphDrawerDrawAsync.Name = "bntMultiSelectionGraphDrawerDrawAsync";
            this.bntMultiSelectionGraphDrawerDrawAsync.Size = new System.Drawing.Size(161, 23);
            this.bntMultiSelectionGraphDrawerDrawAsync.TabIndex = 0;
            this.bntMultiSelectionGraphDrawerDrawAsync.Text = "DrawAsync";
            this.bntMultiSelectionGraphDrawerDrawAsync.UseVisualStyleBackColor = true;
            this.bntMultiSelectionGraphDrawerDrawAsync.Click += new System.EventHandler(this.bntMultiSelectionGraphDrawerDrawAsync_Click);
            // 
            // chkDrawSelection
            // 
            this.chkDrawSelection.AutoSize = true;
            this.chkDrawSelection.Checked = true;
            this.chkDrawSelection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDrawSelection.Location = new System.Drawing.Point(15, 105);
            this.chkDrawSelection.Name = "chkDrawSelection";
            this.chkDrawSelection.Size = new System.Drawing.Size(96, 17);
            this.chkDrawSelection.TabIndex = 2;
            this.chkDrawSelection.Text = "Draw selection";
            this.chkDrawSelection.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 517);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.picGraph);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txbStatus);
            this.Name = "MainForm";
            this.Text = "Test Multithreaded Classes";
            ((System.ComponentModel.ISupportInitialize)(this.picGraph)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Research.CommunityTechnologies.AppLib.TextBoxTraceListener txbStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLayOutGraphAsync;
        private System.Windows.Forms.PictureBox picGraph;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAsyncGraphDrawerDrawAsync;
        private System.Windows.Forms.Button btnLayOutGraphAsyncCancel;
        private System.Windows.Forms.Button btnAsyncGraphDrawerDrawAsyncCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnMultiSelectionGraphDrawerDrawAsyncCancel;
        private System.Windows.Forms.Button bntMultiSelectionGraphDrawerDrawAsync;
        private System.Windows.Forms.CheckBox chkDrawSelection;
    }
}

