
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
            this.label3 = new System.Windows.Forms.Label();
            this.btnCreateRegistrationEmail = new System.Windows.Forms.Button();
            this.btnCopyRegistrationAddress = new System.Windows.Forms.Button();
            this.grpCreateRegistrationEmail = new System.Windows.Forms.GroupBox();
            this.grpCreateRegistrationEmail.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(341, 177);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lnkPrivacy
            // 
            this.lnkPrivacy.AutoSize = true;
            this.lnkPrivacy.Location = new System.Drawing.Point(9, 50);
            this.lnkPrivacy.Name = "lnkPrivacy";
            this.lnkPrivacy.Size = new System.Drawing.Size(73, 13);
            this.lnkPrivacy.TabIndex = 1;
            this.lnkPrivacy.TabStop = true;
            this.lnkPrivacy.Text = "Privacy Policy";
            this.lnkPrivacy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPrivacy_LinkClicked);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(407, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "To join the Microsoft NodeXL email list, send an email with \"Register\" in the sub" +
                "ject line to registration@nodexl.org.";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(174, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(215, 58);
            this.label3.TabIndex = 1;
            this.label3.Text = "Use this if you have an email program such as Outlook, Windows Live Mail, Outlook" +
                " Express, or Thunderbird installed on your computer.";
            // 
            // btnCreateRegistrationEmail
            // 
            this.btnCreateRegistrationEmail.Location = new System.Drawing.Point(13, 19);
            this.btnCreateRegistrationEmail.Name = "btnCreateRegistrationEmail";
            this.btnCreateRegistrationEmail.Size = new System.Drawing.Size(151, 23);
            this.btnCreateRegistrationEmail.TabIndex = 0;
            this.btnCreateRegistrationEmail.Text = "Create &Email";
            this.btnCreateRegistrationEmail.UseVisualStyleBackColor = true;
            this.btnCreateRegistrationEmail.Click += new System.EventHandler(this.btnCreateRegistrationEmail_Click);
            // 
            // btnCopyRegistrationAddress
            // 
            this.btnCopyRegistrationAddress.Location = new System.Drawing.Point(25, 177);
            this.btnCopyRegistrationAddress.Name = "btnCopyRegistrationAddress";
            this.btnCopyRegistrationAddress.Size = new System.Drawing.Size(151, 23);
            this.btnCopyRegistrationAddress.TabIndex = 3;
            this.btnCopyRegistrationAddress.Text = "Copy &Registration Address";
            this.btnCopyRegistrationAddress.UseVisualStyleBackColor = true;
            this.btnCopyRegistrationAddress.Click += new System.EventHandler(this.btnCopyRegistrationAddress_Click);
            // 
            // grpCreateRegistrationEmail
            // 
            this.grpCreateRegistrationEmail.Controls.Add(this.label3);
            this.grpCreateRegistrationEmail.Controls.Add(this.btnCreateRegistrationEmail);
            this.grpCreateRegistrationEmail.Location = new System.Drawing.Point(12, 73);
            this.grpCreateRegistrationEmail.Name = "grpCreateRegistrationEmail";
            this.grpCreateRegistrationEmail.Size = new System.Drawing.Size(404, 87);
            this.grpCreateRegistrationEmail.TabIndex = 2;
            this.grpCreateRegistrationEmail.TabStop = false;
            // 
            // RegisterUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpCreateRegistrationEmail);
            this.Controls.Add(this.btnCopyRegistrationAddress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lnkPrivacy);
            this.Controls.Add(this.btnCancel);
            this.Name = "RegisterUserControl";
            this.Size = new System.Drawing.Size(430, 213);
            this.grpCreateRegistrationEmail.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.LinkLabel lnkPrivacy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCreateRegistrationEmail;
        private System.Windows.Forms.Button btnCopyRegistrationAddress;
        private System.Windows.Forms.GroupBox grpCreateRegistrationEmail;
    }
}
