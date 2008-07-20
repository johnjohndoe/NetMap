
//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace TestNetMapControl
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
            this.txbStatus = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClearStatus = new System.Windows.Forms.Button();
            this.btnDeselectAll = new System.Windows.Forms.Button();
            this.btnSelectedVertices = new System.Windows.Forms.Button();
            this.btnSelectedEdges = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSetVertexSelected = new System.Windows.Forms.Button();
            this.chkAlsoIncidentEdges = new System.Windows.Forms.CheckBox();
            this.chkVertexSelected = new System.Windows.Forms.CheckBox();
            this.txbVertexID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSetEdgeSelected = new System.Windows.Forms.Button();
            this.chkAlsoAdjacentVertices = new System.Windows.Forms.CheckBox();
            this.chkEdgeSelected = new System.Windows.Forms.CheckBox();
            this.txbEdgeID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkShowToolTips = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbxMouseSelectionMode = new Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus();
            this.oNetMapControl = new Microsoft.NetMap.Visualization.NetMapControl();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txbStatus
            // 
            this.txbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbStatus.Location = new System.Drawing.Point(12, 466);
            this.txbStatus.Multiline = true;
            this.txbStatus.Name = "txbStatus";
            this.txbStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txbStatus.Size = new System.Drawing.Size(620, 102);
            this.txbStatus.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 450);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Status:";
            // 
            // btnClearStatus
            // 
            this.btnClearStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearStatus.Location = new System.Drawing.Point(648, 523);
            this.btnClearStatus.Name = "btnClearStatus";
            this.btnClearStatus.Size = new System.Drawing.Size(104, 23);
            this.btnClearStatus.TabIndex = 9;
            this.btnClearStatus.Text = "Clear Status";
            this.btnClearStatus.UseVisualStyleBackColor = true;
            this.btnClearStatus.Click += new System.EventHandler(this.btnClearStatus_Click);
            // 
            // btnDeselectAll
            // 
            this.btnDeselectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeselectAll.Location = new System.Drawing.Point(648, 408);
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.Size = new System.Drawing.Size(104, 23);
            this.btnDeselectAll.TabIndex = 6;
            this.btnDeselectAll.Text = "DeselectAll()";
            this.btnDeselectAll.UseVisualStyleBackColor = true;
            this.btnDeselectAll.Click += new System.EventHandler(this.btnDeselectAll_Click);
            // 
            // btnSelectedVertices
            // 
            this.btnSelectedVertices.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectedVertices.Location = new System.Drawing.Point(648, 339);
            this.btnSelectedVertices.Name = "btnSelectedVertices";
            this.btnSelectedVertices.Size = new System.Drawing.Size(104, 23);
            this.btnSelectedVertices.TabIndex = 4;
            this.btnSelectedVertices.Text = "SelectedVertices";
            this.btnSelectedVertices.UseVisualStyleBackColor = true;
            this.btnSelectedVertices.Click += new System.EventHandler(this.btnSelectedVertices_Click);
            // 
            // btnSelectedEdges
            // 
            this.btnSelectedEdges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectedEdges.Location = new System.Drawing.Point(648, 369);
            this.btnSelectedEdges.Name = "btnSelectedEdges";
            this.btnSelectedEdges.Size = new System.Drawing.Size(104, 23);
            this.btnSelectedEdges.TabIndex = 5;
            this.btnSelectedEdges.Text = "SelectedEdges";
            this.btnSelectedEdges.UseVisualStyleBackColor = true;
            this.btnSelectedEdges.Click += new System.EventHandler(this.btnSelectedEdges_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnSetVertexSelected);
            this.groupBox1.Controls.Add(this.chkAlsoIncidentEdges);
            this.groupBox1.Controls.Add(this.chkVertexSelected);
            this.groupBox1.Controls.Add(this.txbVertexID);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(648, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(160, 139);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SetVertexSelected()";
            // 
            // btnSetVertexSelected
            // 
            this.btnSetVertexSelected.Location = new System.Drawing.Point(22, 103);
            this.btnSetVertexSelected.Name = "btnSetVertexSelected";
            this.btnSetVertexSelected.Size = new System.Drawing.Size(75, 23);
            this.btnSetVertexSelected.TabIndex = 4;
            this.btnSetVertexSelected.Text = "Go";
            this.btnSetVertexSelected.UseVisualStyleBackColor = true;
            this.btnSetVertexSelected.Click += new System.EventHandler(this.btnSetVertexSelected_Click);
            // 
            // chkAlsoIncidentEdges
            // 
            this.chkAlsoIncidentEdges.AutoSize = true;
            this.chkAlsoIncidentEdges.Location = new System.Drawing.Point(22, 74);
            this.chkAlsoIncidentEdges.Name = "chkAlsoIncidentEdges";
            this.chkAlsoIncidentEdges.Size = new System.Drawing.Size(118, 17);
            this.chkAlsoIncidentEdges.TabIndex = 3;
            this.chkAlsoIncidentEdges.Text = "Also incident edges";
            this.chkAlsoIncidentEdges.UseVisualStyleBackColor = true;
            // 
            // chkVertexSelected
            // 
            this.chkVertexSelected.AutoSize = true;
            this.chkVertexSelected.Location = new System.Drawing.Point(22, 51);
            this.chkVertexSelected.Name = "chkVertexSelected";
            this.chkVertexSelected.Size = new System.Drawing.Size(68, 17);
            this.chkVertexSelected.TabIndex = 2;
            this.chkVertexSelected.Text = "Selected";
            this.chkVertexSelected.UseVisualStyleBackColor = true;
            // 
            // txbVertexID
            // 
            this.txbVertexID.Location = new System.Drawing.Point(79, 25);
            this.txbVertexID.Name = "txbVertexID";
            this.txbVertexID.Size = new System.Drawing.Size(55, 20);
            this.txbVertexID.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Vertex ID:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnSetEdgeSelected);
            this.groupBox2.Controls.Add(this.chkAlsoAdjacentVertices);
            this.groupBox2.Controls.Add(this.chkEdgeSelected);
            this.groupBox2.Controls.Add(this.txbEdgeID);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(648, 186);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(160, 139);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SetEdgeSelected()";
            // 
            // btnSetEdgeSelected
            // 
            this.btnSetEdgeSelected.Location = new System.Drawing.Point(22, 100);
            this.btnSetEdgeSelected.Name = "btnSetEdgeSelected";
            this.btnSetEdgeSelected.Size = new System.Drawing.Size(75, 23);
            this.btnSetEdgeSelected.TabIndex = 4;
            this.btnSetEdgeSelected.Text = "Go";
            this.btnSetEdgeSelected.UseVisualStyleBackColor = true;
            this.btnSetEdgeSelected.Click += new System.EventHandler(this.btnSetEdgeSelected_Click);
            // 
            // chkAlsoAdjacentVertices
            // 
            this.chkAlsoAdjacentVertices.AutoSize = true;
            this.chkAlsoAdjacentVertices.Location = new System.Drawing.Point(22, 71);
            this.chkAlsoAdjacentVertices.Name = "chkAlsoAdjacentVertices";
            this.chkAlsoAdjacentVertices.Size = new System.Drawing.Size(130, 17);
            this.chkAlsoAdjacentVertices.TabIndex = 3;
            this.chkAlsoAdjacentVertices.Text = "Also adjacent vertices";
            this.chkAlsoAdjacentVertices.UseVisualStyleBackColor = true;
            // 
            // chkEdgeSelected
            // 
            this.chkEdgeSelected.AutoSize = true;
            this.chkEdgeSelected.Location = new System.Drawing.Point(22, 48);
            this.chkEdgeSelected.Name = "chkEdgeSelected";
            this.chkEdgeSelected.Size = new System.Drawing.Size(68, 17);
            this.chkEdgeSelected.TabIndex = 2;
            this.chkEdgeSelected.Text = "Selected";
            this.chkEdgeSelected.UseVisualStyleBackColor = true;
            // 
            // txbEdgeID
            // 
            this.txbEdgeID.Location = new System.Drawing.Point(79, 22);
            this.txbEdgeID.Name = "txbEdgeID";
            this.txbEdgeID.Size = new System.Drawing.Size(55, 20);
            this.txbEdgeID.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Edge ID:";
            // 
            // chkShowToolTips
            // 
            this.chkShowToolTips.AutoSize = true;
            this.chkShowToolTips.Location = new System.Drawing.Point(648, 12);
            this.chkShowToolTips.Name = "chkShowToolTips";
            this.chkShowToolTips.Size = new System.Drawing.Size(94, 17);
            this.chkShowToolTips.TabIndex = 1;
            this.chkShowToolTips.Text = "ShowToolTips";
            this.chkShowToolTips.UseVisualStyleBackColor = true;
            this.chkShowToolTips.CheckedChanged += new System.EventHandler(this.chkShowToolTips_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(645, 450);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "MouseSelectionMode:";
            // 
            // cbxMouseSelectionMode
            // 
            this.cbxMouseSelectionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxMouseSelectionMode.FormattingEnabled = true;
            this.cbxMouseSelectionMode.Location = new System.Drawing.Point(648, 470);
            this.cbxMouseSelectionMode.Name = "cbxMouseSelectionMode";
            this.cbxMouseSelectionMode.Size = new System.Drawing.Size(121, 21);
            this.cbxMouseSelectionMode.TabIndex = 8;
            this.cbxMouseSelectionMode.SelectedIndexChanged += new System.EventHandler(this.cbxMouseSelectionMode_SelectedIndexChanged);
            // 
            // oNetMapControl
            // 
            this.oNetMapControl.AllowVertexDrag = true;
            this.oNetMapControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.oNetMapControl.BackColor = System.Drawing.SystemColors.Window;
            this.oNetMapControl.Location = new System.Drawing.Point(12, 12);
            this.oNetMapControl.MouseSelectionMode = Microsoft.NetMap.Visualization.MouseSelectionMode.SelectVertexAndIncidentEdges;
            this.oNetMapControl.Name = "oNetMapControl";
            this.oNetMapControl.ShowToolTips = false;
            this.oNetMapControl.Size = new System.Drawing.Size(620, 426);
            this.oNetMapControl.TabIndex = 0;
            this.oNetMapControl.SelectionChanged += new System.EventHandler(this.oNetMapControl_SelectionChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 586);
            this.Controls.Add(this.cbxMouseSelectionMode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkShowToolTips);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSelectedEdges);
            this.Controls.Add(this.btnSelectedVertices);
            this.Controls.Add(this.btnDeselectAll);
            this.Controls.Add(this.btnClearStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txbStatus);
            this.Controls.Add(this.oNetMapControl);
            this.Name = "MainForm";
            this.Text = "Test NetMapControl";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.NetMap.Visualization.NetMapControl oNetMapControl;
        private System.Windows.Forms.TextBox txbStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClearStatus;
        private System.Windows.Forms.Button btnDeselectAll;
        private System.Windows.Forms.Button btnSelectedVertices;
        private System.Windows.Forms.Button btnSelectedEdges;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txbVertexID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSetVertexSelected;
        private System.Windows.Forms.CheckBox chkAlsoIncidentEdges;
        private System.Windows.Forms.CheckBox chkVertexSelected;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkEdgeSelected;
        private System.Windows.Forms.TextBox txbEdgeID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSetEdgeSelected;
        private System.Windows.Forms.CheckBox chkAlsoAdjacentVertices;
        private System.Windows.Forms.CheckBox chkShowToolTips;
        private System.Windows.Forms.Label label4;
        private Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus cbxMouseSelectionMode;
    }
}

