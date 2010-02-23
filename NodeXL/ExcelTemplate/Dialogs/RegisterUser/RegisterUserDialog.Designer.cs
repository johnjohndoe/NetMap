

//  Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class RegisterUserDialog
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
            this.usrRegisterUser = new Microsoft.NodeXL.Common.RegisterUserControl();
            this.SuspendLayout();
            // 
            // usrRegisterUser
            // 
            this.usrRegisterUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.usrRegisterUser.Location = new System.Drawing.Point(0, 0);
            this.usrRegisterUser.Name = "usrRegisterUser";
            this.usrRegisterUser.Size = new System.Drawing.Size(428, 212);
            this.usrRegisterUser.TabIndex = 0;
            this.usrRegisterUser.Done += new System.EventHandler(this.usrRegisterUser_Done);
            // 
            // RegisterUserDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 212);
            this.Controls.Add(this.usrRegisterUser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegisterUserDialog";
            this.Text = "Register";
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.NodeXL.Common.RegisterUserControl usrRegisterUser;
    }
}
