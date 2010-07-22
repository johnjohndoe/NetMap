

//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.GraphDataProviders.YouTube
{
    partial class YouTubeGetVideoNetworkDialog
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlUserInputs = new System.Windows.Forms.Panel();
            this.usrLimitToN = new Microsoft.NodeXL.GraphDataProviders.LimitToNControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkIncludeSharedVideoResponderEdges = new System.Windows.Forms.CheckBox();
            this.chkIncludeSharedTagEdges = new System.Windows.Forms.CheckBox();
            this.chkIncludeSharedCommenterEdges = new System.Windows.Forms.CheckBox();
            this.txbSearchTerm = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlUserInputs.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(166, 215);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(252, 215);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlUserInputs
            // 
            this.pnlUserInputs.Controls.Add(this.usrLimitToN);
            this.pnlUserInputs.Controls.Add(this.groupBox2);
            this.pnlUserInputs.Controls.Add(this.txbSearchTerm);
            this.pnlUserInputs.Controls.Add(this.label1);
            this.pnlUserInputs.Location = new System.Drawing.Point(12, 12);
            this.pnlUserInputs.Name = "pnlUserInputs";
            this.pnlUserInputs.Size = new System.Drawing.Size(327, 200);
            this.pnlUserInputs.TabIndex = 0;
            // 
            // usrLimitToN
            // 
            this.usrLimitToN.Location = new System.Drawing.Point(0, 169);
            this.usrLimitToN.N = 2147483647;
            this.usrLimitToN.Name = "usrLimitToN";
            this.usrLimitToN.ObjectName = "videos";
            this.usrLimitToN.Size = new System.Drawing.Size(168, 27);
            this.usrLimitToN.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkIncludeSharedVideoResponderEdges);
            this.groupBox2.Controls.Add(this.chkIncludeSharedTagEdges);
            this.groupBox2.Controls.Add(this.chkIncludeSharedCommenterEdges);
            this.groupBox2.Location = new System.Drawing.Point(0, 60);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(320, 101);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Add an edge for each";
            // 
            // chkIncludeSharedVideoResponderEdges
            // 
            this.chkIncludeSharedVideoResponderEdges.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkIncludeSharedVideoResponderEdges.Location = new System.Drawing.Point(15, 60);
            this.chkIncludeSharedVideoResponderEdges.Name = "chkIncludeSharedVideoResponderEdges";
            this.chkIncludeSharedVideoResponderEdges.Size = new System.Drawing.Size(284, 37);
            this.chkIncludeSharedVideoResponderEdges.TabIndex = 2;
            this.chkIncludeSharedVideoResponderEdges.Text = "Pair of &videos responded to with another video by the same user (slower)";
            this.chkIncludeSharedVideoResponderEdges.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkIncludeSharedVideoResponderEdges.UseVisualStyleBackColor = true;
            // 
            // chkIncludeSharedTagEdges
            // 
            this.chkIncludeSharedTagEdges.AutoSize = true;
            this.chkIncludeSharedTagEdges.Location = new System.Drawing.Point(15, 20);
            this.chkIncludeSharedTagEdges.Name = "chkIncludeSharedTagEdges";
            this.chkIncludeSharedTagEdges.Size = new System.Drawing.Size(237, 17);
            this.chkIncludeSharedTagEdges.TabIndex = 0;
            this.chkIncludeSharedTagEdges.Text = "Pair &of videos tagged with the same keyword";
            this.chkIncludeSharedTagEdges.UseVisualStyleBackColor = true;
            // 
            // chkIncludeSharedCommenterEdges
            // 
            this.chkIncludeSharedCommenterEdges.AutoSize = true;
            this.chkIncludeSharedCommenterEdges.Location = new System.Drawing.Point(15, 40);
            this.chkIncludeSharedCommenterEdges.Name = "chkIncludeSharedCommenterEdges";
            this.chkIncludeSharedCommenterEdges.Size = new System.Drawing.Size(285, 17);
            this.chkIncludeSharedCommenterEdges.TabIndex = 1;
            this.chkIncludeSharedCommenterEdges.Text = "&Pair of videos commented on by the same user (slower)";
            this.chkIncludeSharedCommenterEdges.UseVisualStyleBackColor = true;
            // 
            // txbSearchTerm
            // 
            this.txbSearchTerm.Location = new System.Drawing.Point(0, 32);
            this.txbSearchTerm.MaxLength = 100;
            this.txbSearchTerm.Name = "txbSearchTerm";
            this.txbSearchTerm.Size = new System.Drawing.Size(320, 20);
            this.txbSearchTerm.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(305, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Search for videos whose title, keywords, description, categories, or author\'s us" +
                "ername contain:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 249);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(348, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slStatusLabel
            // 
            this.slStatusLabel.Name = "slStatusLabel";
            this.slStatusLabel.Size = new System.Drawing.Size(333, 17);
            this.slStatusLabel.Spring = true;
            this.slStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // YouTubeGetVideoNetworkDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(348, 271);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlUserInputs);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "YouTubeGetVideoNetworkDialog";
            this.Text = "[Gets set in code]";
            this.pnlUserInputs.ResumeLayout(false);
            this.pnlUserInputs.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlUserInputs;
        private System.Windows.Forms.TextBox txbSearchTerm;
        private System.Windows.Forms.Label label1;
        private Microsoft.NodeXL.GraphDataProviders.LimitToNControl usrLimitToN;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slStatusLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkIncludeSharedVideoResponderEdges;
        private System.Windows.Forms.CheckBox chkIncludeSharedTagEdges;
        private System.Windows.Forms.CheckBox chkIncludeSharedCommenterEdges;
    }
}
