
//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NetMap.DesktopApplication
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
            this.mnsTopMenu = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFileRecentDocuments = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLayoutRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniTools = new System.Windows.Forms.ToolStripMenuItem();
            this.mniToolsOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mniWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.mniWindowCascade = new System.Windows.Forms.ToolStripMenuItem();
            this.mniWindowTileHorizontally = new System.Windows.Forms.ToolStripMenuItem();
            this.mniWindowTileVertically = new System.Windows.Forms.ToolStripMenuItem();
            this.mniWindowSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniWindowCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpContents = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnsTopMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnsTopMenu
            // 
            this.mnsTopMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFile,
            this.mniEdit,
            this.mniLayout,
            this.mniTools,
            this.mniWindow,
            this.mniHelp});
            this.mnsTopMenu.Location = new System.Drawing.Point(0, 0);
            this.mnsTopMenu.MdiWindowListItem = this.mniWindow;
            this.mnsTopMenu.Name = "mnsTopMenu";
            this.mnsTopMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.mnsTopMenu.Size = new System.Drawing.Size(692, 24);
            this.mnsTopMenu.TabIndex = 0;
            this.mnsTopMenu.Text = "menuStrip1";
            // 
            // mniFile
            // 
            this.mniFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFileOpen,
            this.mniFileClose,
            this.mniFileSeparator1,
            this.mniFileSave,
            this.mniFileSaveAs,
            this.mniFileSeparator2,
            this.mniFileRecentDocuments,
            this.mniFileSeparator3,
            this.mniFileExit});
            this.mniFile.Name = "mniFile";
            this.mniFile.Size = new System.Drawing.Size(35, 20);
            this.mniFile.Text = "&File";
            this.mniFile.DropDownClosed += new System.EventHandler(this.OnDropDownClosed);
            this.mniFile.DropDownOpening += new System.EventHandler(this.mniFile_DropDownOpening);
            // 
            // mniFileOpen
            // 
            this.mniFileOpen.Name = "mniFileOpen";
            this.mniFileOpen.Size = new System.Drawing.Size(157, 22);
            this.mniFileOpen.Text = "&Open Graph...";
            this.mniFileOpen.Click += new System.EventHandler(this.mniFileOpen_Click);
            // 
            // mniFileClose
            // 
            this.mniFileClose.Name = "mniFileClose";
            this.mniFileClose.Size = new System.Drawing.Size(157, 22);
            this.mniFileClose.Text = "&Close Graph";
            this.mniFileClose.Click += new System.EventHandler(this.mniFileClose_Click);
            // 
            // mniFileSeparator1
            // 
            this.mniFileSeparator1.Name = "mniFileSeparator1";
            this.mniFileSeparator1.Size = new System.Drawing.Size(154, 6);
            // 
            // mniFileSave
            // 
            this.mniFileSave.Name = "mniFileSave";
            this.mniFileSave.Size = new System.Drawing.Size(157, 22);
            this.mniFileSave.Text = "&Save Graph";
            this.mniFileSave.Click += new System.EventHandler(this.mniFileSave_Click);
            // 
            // mniFileSaveAs
            // 
            this.mniFileSaveAs.Name = "mniFileSaveAs";
            this.mniFileSaveAs.Size = new System.Drawing.Size(157, 22);
            this.mniFileSaveAs.Text = "Save Graph &As...";
            this.mniFileSaveAs.Click += new System.EventHandler(this.mniFileSaveAs_Click);
            // 
            // mniFileSeparator2
            // 
            this.mniFileSeparator2.Name = "mniFileSeparator2";
            this.mniFileSeparator2.Size = new System.Drawing.Size(154, 6);
            // 
            // mniFileRecentDocuments
            // 
            this.mniFileRecentDocuments.Name = "mniFileRecentDocuments";
            this.mniFileRecentDocuments.Size = new System.Drawing.Size(157, 22);
            this.mniFileRecentDocuments.Text = "&Recent Graphs";
            // 
            // mniFileSeparator3
            // 
            this.mniFileSeparator3.Name = "mniFileSeparator3";
            this.mniFileSeparator3.Size = new System.Drawing.Size(154, 6);
            // 
            // mniFileExit
            // 
            this.mniFileExit.Name = "mniFileExit";
            this.mniFileExit.Size = new System.Drawing.Size(157, 22);
            this.mniFileExit.Text = "E&xit";
            this.mniFileExit.Click += new System.EventHandler(this.mniFileExit_Click);
            // 
            // mniEdit
            // 
            this.mniEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniEditCopy});
            this.mniEdit.Name = "mniEdit";
            this.mniEdit.Size = new System.Drawing.Size(37, 20);
            this.mniEdit.Text = "&Edit";
            this.mniEdit.DropDownClosed += new System.EventHandler(this.OnDropDownClosed);
            this.mniEdit.DropDownOpening += new System.EventHandler(this.mniEdit_DropDownOpening);
            // 
            // mniEditCopy
            // 
            this.mniEditCopy.Name = "mniEditCopy";
            this.mniEditCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.mniEditCopy.Size = new System.Drawing.Size(203, 22);
            this.mniEditCopy.Text = "&Copy Graph Image";
            this.mniEditCopy.Click += new System.EventHandler(this.mniEditCopy_Click);
            // 
            // mniLayout
            // 
            this.mniLayout.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniLayoutRefresh,
            this.toolStripSeparator1});
            this.mniLayout.Name = "mniLayout";
            this.mniLayout.Size = new System.Drawing.Size(52, 20);
            this.mniLayout.Text = "&Layout";
            this.mniLayout.DropDownClosed += new System.EventHandler(this.OnDropDownClosed);
            this.mniLayout.DropDownOpening += new System.EventHandler(this.mniLayout_DropDownOpening);
            // 
            // mniLayoutRefresh
            // 
            this.mniLayoutRefresh.Name = "mniLayoutRefresh";
            this.mniLayoutRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.mniLayoutRefresh.Size = new System.Drawing.Size(131, 22);
            this.mniLayoutRefresh.Text = "R&efresh";
            this.mniLayoutRefresh.Click += new System.EventHandler(this.mniLayoutRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(128, 6);
            // 
            // mniTools
            // 
            this.mniTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniToolsOptions});
            this.mniTools.Name = "mniTools";
            this.mniTools.Size = new System.Drawing.Size(44, 20);
            this.mniTools.Text = "&Tools";
            this.mniTools.DropDownClosed += new System.EventHandler(this.OnDropDownClosed);
            this.mniTools.DropDownOpening += new System.EventHandler(this.mniTools_DropDownOpening);
            // 
            // mniToolsOptions
            // 
            this.mniToolsOptions.Name = "mniToolsOptions";
            this.mniToolsOptions.Size = new System.Drawing.Size(152, 22);
            this.mniToolsOptions.Text = "&Options...";
            this.mniToolsOptions.Click += new System.EventHandler(this.mniToolsOptions_Click);
            // 
            // mniWindow
            // 
            this.mniWindow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniWindowCascade,
            this.mniWindowTileHorizontally,
            this.mniWindowTileVertically,
            this.mniWindowSeparator1,
            this.mniWindowCloseAll});
            this.mniWindow.Name = "mniWindow";
            this.mniWindow.Size = new System.Drawing.Size(57, 20);
            this.mniWindow.Text = "&Window";
            this.mniWindow.DropDownClosed += new System.EventHandler(this.OnDropDownClosed);
            this.mniWindow.DropDownOpening += new System.EventHandler(this.mniWindow_DropDownOpening);
            // 
            // mniWindowCascade
            // 
            this.mniWindowCascade.Name = "mniWindowCascade";
            this.mniWindowCascade.Size = new System.Drawing.Size(149, 22);
            this.mniWindowCascade.Tag = System.Windows.Forms.MdiLayout.Cascade;
            this.mniWindowCascade.Text = "&Cascade";
            this.mniWindowCascade.Click += new System.EventHandler(this.mniWindowArrange_Click);
            // 
            // mniWindowTileHorizontally
            // 
            this.mniWindowTileHorizontally.Name = "mniWindowTileHorizontally";
            this.mniWindowTileHorizontally.Size = new System.Drawing.Size(149, 22);
            this.mniWindowTileHorizontally.Tag = System.Windows.Forms.MdiLayout.TileHorizontal;
            this.mniWindowTileHorizontally.Text = "Tile &Horizontally";
            this.mniWindowTileHorizontally.Click += new System.EventHandler(this.mniWindowArrange_Click);
            // 
            // mniWindowTileVertically
            // 
            this.mniWindowTileVertically.Name = "mniWindowTileVertically";
            this.mniWindowTileVertically.Size = new System.Drawing.Size(149, 22);
            this.mniWindowTileVertically.Tag = System.Windows.Forms.MdiLayout.TileVertical;
            this.mniWindowTileVertically.Text = "Tile &Vertically";
            this.mniWindowTileVertically.Click += new System.EventHandler(this.mniWindowArrange_Click);
            // 
            // mniWindowSeparator1
            // 
            this.mniWindowSeparator1.Name = "mniWindowSeparator1";
            this.mniWindowSeparator1.Size = new System.Drawing.Size(146, 6);
            // 
            // mniWindowCloseAll
            // 
            this.mniWindowCloseAll.Name = "mniWindowCloseAll";
            this.mniWindowCloseAll.Size = new System.Drawing.Size(149, 22);
            this.mniWindowCloseAll.Text = "Close &All";
            this.mniWindowCloseAll.Click += new System.EventHandler(this.mniWindowCloseAll_Click);
            // 
            // mniHelp
            // 
            this.mniHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniHelpContents,
            this.toolStripSeparator4,
            this.mniHelpAbout});
            this.mniHelp.Name = "mniHelp";
            this.mniHelp.Size = new System.Drawing.Size(40, 20);
            this.mniHelp.Text = "&Help";
            this.mniHelp.DropDownClosed += new System.EventHandler(this.OnDropDownClosed);
            // 
            // mniHelpContents
            // 
            this.mniHelpContents.Name = "mniHelpContents";
            this.mniHelpContents.Size = new System.Drawing.Size(154, 22);
            this.mniHelpContents.Text = "&Contents";
            this.mniHelpContents.Click += new System.EventHandler(this.mniHelpContents_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(151, 6);
            // 
            // mniHelpAbout
            // 
            this.mniHelpAbout.Name = "mniHelpAbout";
            this.mniHelpAbout.Size = new System.Drawing.Size(154, 22);
            this.mniHelpAbout.Text = "&About .NetMap...";
            this.mniHelpAbout.Click += new System.EventHandler(this.mniHelpAbout_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 473);
            this.Controls.Add(this.mnsTopMenu);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mnsTopMenu;
            this.Name = "MainForm";
            this.mnsTopMenu.ResumeLayout(false);
            this.mnsTopMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnsTopMenu;
        private System.Windows.Forms.ToolStripMenuItem mniFile;
        private System.Windows.Forms.ToolStripMenuItem mniFileOpen;
        private System.Windows.Forms.ToolStripMenuItem mniFileClose;
        private System.Windows.Forms.ToolStripSeparator mniFileSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniFileSave;
        private System.Windows.Forms.ToolStripMenuItem mniFileSaveAs;
        private System.Windows.Forms.ToolStripSeparator mniFileSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mniFileRecentDocuments;
        private System.Windows.Forms.ToolStripSeparator mniFileSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mniFileExit;
        private System.Windows.Forms.ToolStripMenuItem mniWindow;
        private System.Windows.Forms.ToolStripMenuItem mniWindowCascade;
        private System.Windows.Forms.ToolStripMenuItem mniWindowTileHorizontally;
        private System.Windows.Forms.ToolStripMenuItem mniWindowTileVertically;
        private System.Windows.Forms.ToolStripSeparator mniWindowSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniWindowCloseAll;
        private System.Windows.Forms.ToolStripMenuItem mniHelp;
        private System.Windows.Forms.ToolStripMenuItem mniHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem mniEdit;
        private System.Windows.Forms.ToolStripMenuItem mniEditCopy;
        private System.Windows.Forms.ToolStripMenuItem mniLayout;
        private System.Windows.Forms.ToolStripMenuItem mniLayoutRefresh;
        private System.Windows.Forms.ToolStripMenuItem mniTools;
        private System.Windows.Forms.ToolStripMenuItem mniToolsOptions;
        private System.Windows.Forms.ToolStripMenuItem mniHelpContents;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

