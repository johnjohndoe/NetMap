
namespace Microsoft.NodeXL.Common
{
    partial class RegisterUserControl
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.lnkPrivacy = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.lnkRegister = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(343, 82);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lnkPrivacy
            // 
            this.lnkPrivacy.AutoSize = true;
            this.lnkPrivacy.Location = new System.Drawing.Point(9, 82);
            this.lnkPrivacy.Name = "lnkPrivacy";
            this.lnkPrivacy.Size = new System.Drawing.Size(73, 13);
            this.lnkPrivacy.TabIndex = 2;
            this.lnkPrivacy.TabStop = true;
            this.lnkPrivacy.Text = "Privacy Policy";
            this.lnkPrivacy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPrivacy_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(384, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "To register and join the Microsoft NodeXL email list, please go to this Web page:" +
                "";
            // 
            // lnkRegister
            // 
            this.lnkRegister.AutoSize = true;
            this.lnkRegister.Location = new System.Drawing.Point(9, 34);
            this.lnkRegister.Name = "lnkRegister";
            this.lnkRegister.Size = new System.Drawing.Size(187, 13);
            this.lnkRegister.TabIndex = 1;
            this.lnkRegister.TabStop = true;
            this.lnkRegister.Text = "NodeXL Registration and User Survey";
            this.lnkRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRegister_LinkClicked);
            // 
            // RegisterUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lnkRegister);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lnkPrivacy);
            this.Controls.Add(this.btnCancel);
            this.Name = "RegisterUserControl";
            this.Size = new System.Drawing.Size(430, 120);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.LinkLabel lnkPrivacy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel lnkRegister;
    }
}
