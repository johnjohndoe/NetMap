namespace Microsoft.NodeXL.ApplicationUtil
{
    partial class GraphZoomAndScaleControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbGraphZoom = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.tbGraphScale = new System.Windows.Forms.TrackBar();
            this.lnkHelp = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.tbGraphZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbGraphScale)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Zoom:";
            // 
            // tbGraphZoom
            // 
            this.tbGraphZoom.Location = new System.Drawing.Point(43, 0);
            this.tbGraphZoom.Name = "tbGraphZoom";
            this.tbGraphZoom.Size = new System.Drawing.Size(92, 42);
            this.tbGraphZoom.TabIndex = 1;
            this.tbGraphZoom.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbGraphZoom.Scroll += new System.EventHandler(this.tbGraphZoom_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(141, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Scale:";
            // 
            // tbGraphScale
            // 
            this.tbGraphScale.Location = new System.Drawing.Point(184, 0);
            this.tbGraphScale.Name = "tbGraphScale";
            this.tbGraphScale.Size = new System.Drawing.Size(92, 42);
            this.tbGraphScale.TabIndex = 3;
            this.tbGraphScale.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbGraphScale.Scroll += new System.EventHandler(this.tbGraphScale_Scroll);
            // 
            // lnkHelp
            // 
            this.lnkHelp.AutoSize = true;
            this.lnkHelp.Location = new System.Drawing.Point(306, 3);
            this.lnkHelp.Name = "lnkHelp";
            this.lnkHelp.Size = new System.Drawing.Size(116, 13);
            this.lnkHelp.TabIndex = 4;
            this.lnkHelp.TabStop = true;
            this.lnkHelp.Text = "About Zoom and Scale";
            this.lnkHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelp_LinkClicked);
            // 
            // GraphZoomAndScaleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lnkHelp);
            this.Controls.Add(this.tbGraphScale);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbGraphZoom);
            this.Controls.Add(this.label1);
            this.Name = "GraphZoomAndScaleControl";
            this.Size = new System.Drawing.Size(440, 30);
            ((System.ComponentModel.ISupportInitialize)(this.tbGraphZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbGraphScale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar tbGraphZoom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar tbGraphScale;
        private System.Windows.Forms.LinkLabel lnkHelp;
    }
}
