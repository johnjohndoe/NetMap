namespace Microsoft.NetMap.ExcelTemplate
{
    partial class SubgraphLevelsControl
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
            this.picSampleSubgraph = new System.Windows.Forms.PictureBox();
            this.cbxLevels = new Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picSampleSubgraph)).BeginInit();
            this.SuspendLayout();
            // 
            // picSampleSubgraph
            // 
            this.picSampleSubgraph.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picSampleSubgraph.Location = new System.Drawing.Point(243, 0);
            this.picSampleSubgraph.Name = "picSampleSubgraph";
            this.picSampleSubgraph.Size = new System.Drawing.Size(72, 50);
            this.picSampleSubgraph.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picSampleSubgraph.TabIndex = 14;
            this.picSampleSubgraph.TabStop = false;
            // 
            // cbxLevels
            // 
            this.cbxLevels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxLevels.FormattingEnabled = true;
            this.cbxLevels.Location = new System.Drawing.Point(192, 13);
            this.cbxLevels.Name = "cbxLevels";
            this.cbxLevels.Size = new System.Drawing.Size(44, 21);
            this.cbxLevels.TabIndex = 13;
            this.cbxLevels.SelectedIndexChanged += new System.EventHandler(this.cbxLevels_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 40);
            this.label1.TabIndex = 12;
            this.label1.Text = "&Levels of adjacent vertices to include in each subgraph:";
            // 
            // SubgraphLevelsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.picSampleSubgraph);
            this.Controls.Add(this.cbxLevels);
            this.Controls.Add(this.label1);
            this.Name = "SubgraphLevelsControl";
            this.Size = new System.Drawing.Size(319, 53);
            ((System.ComponentModel.ISupportInitialize)(this.picSampleSubgraph)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picSampleSubgraph;
        private Microsoft.Research.CommunityTechnologies.AppLib.ComboBoxPlus cbxLevels;
        private System.Windows.Forms.Label label1;
    }
}
