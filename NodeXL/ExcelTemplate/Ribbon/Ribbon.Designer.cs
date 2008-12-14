

//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NodeXL.ExcelTemplate
{
    partial class Ribbon
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
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItem3 = new Microsoft.Office.Tools.Ribbon.RibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItem4 = new Microsoft.Office.Tools.Ribbon.RibbonDropDownItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ribbon));
            this.NodeXL = new Microsoft.Office.Tools.Ribbon.RibbonTab();
            this.grpGraph = new Microsoft.Office.Tools.Ribbon.RibbonGroup();
            this.rddGraphDirectedness = new Microsoft.Office.Tools.Ribbon.RibbonDropDown();
            this.btnToggleGraphVisibility = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnEditAutoFillUserSettings = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.mnuImport = new Microsoft.Office.Tools.Ribbon.RibbonMenu();
            this.btnImportPajekFile = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnImportEdgesFromWorkbook = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnAnalyzeEmailNetwork = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnAnalyzeTwitterNetwork = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.mnuExport = new Microsoft.Office.Tools.Ribbon.RibbonMenu();
            this.btnExportSelectionToNewWorkbook = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.mnuHelp = new Microsoft.Office.Tools.Ribbon.RibbonMenu();
            this.btnOpenHomePage = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnRegisterUser = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnCheckForUpdate = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnAbout = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.grpEdges = new Microsoft.Office.Tools.Ribbon.RibbonGroup();
            this.btnMergeDuplicateEdges = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.grpVertices = new Microsoft.Office.Tools.Ribbon.RibbonGroup();
            this.btnPopulateVertexWorksheet = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnCustomizeVertexMenu = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.grpAnalysis = new Microsoft.Office.Tools.Ribbon.RibbonGroup();
            this.spltCalculateGraphMetrics = new Microsoft.Office.Tools.Ribbon.RibbonSplitButton();
            this.btnEditGraphMetricUserSettings = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnCreateSubgraphImages = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.separator1 = new Microsoft.Office.Tools.Ribbon.RibbonSeparator();
            this.btnCreateClusters = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.chkReadClusters = new Microsoft.Office.Tools.Ribbon.RibbonCheckBox();
            this.btnConvertOldWorkbook = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.NodeXL.SuspendLayout();
            this.grpGraph.SuspendLayout();
            this.grpEdges.SuspendLayout();
            this.grpVertices.SuspendLayout();
            this.grpAnalysis.SuspendLayout();
            this.SuspendLayout();
            // 
            // NodeXL
            // 
            this.NodeXL.Groups.Add(this.grpGraph);
            this.NodeXL.Groups.Add(this.grpEdges);
            this.NodeXL.Groups.Add(this.grpVertices);
            this.NodeXL.Groups.Add(this.grpAnalysis);
            this.NodeXL.Label = "NodeXL";
            this.NodeXL.Name = "NodeXL";
            // 
            // grpGraph
            // 
            this.grpGraph.Items.Add(this.rddGraphDirectedness);
            this.grpGraph.Items.Add(this.btnToggleGraphVisibility);
            this.grpGraph.Items.Add(this.btnEditAutoFillUserSettings);
            this.grpGraph.Items.Add(this.mnuImport);
            this.grpGraph.Items.Add(this.mnuExport);
            this.grpGraph.Items.Add(this.btnConvertOldWorkbook);
            this.grpGraph.Items.Add(this.mnuHelp);
            this.grpGraph.Label = "Graph";
            this.grpGraph.Name = "grpGraph";
            // 
            // rddGraphDirectedness
            // 
            this.rddGraphDirectedness.Enabled = false;
            ribbonDropDownItem3.Label = "Directed";
            ribbonDropDownItem3.OfficeImageId = "ShapeStraightConnectorArrow";
            ribbonDropDownItem3.ScreenTip = "Edges start at Vertex 1 and end at Vertex 2.";
            ribbonDropDownItem3.Tag = Microsoft.NodeXL.Core.GraphDirectedness.Directed;
            ribbonDropDownItem4.Label = "Undirected";
            ribbonDropDownItem4.OfficeImageId = "ShapeStraightConnector";
            ribbonDropDownItem4.ScreenTip = "Edges do not have a start or end.  Vertex 1 and Vertex 2 are interchangable.";
            ribbonDropDownItem4.Tag = Microsoft.NodeXL.Core.GraphDirectedness.Undirected;
            this.rddGraphDirectedness.Items.Add(ribbonDropDownItem3);
            this.rddGraphDirectedness.Items.Add(ribbonDropDownItem4);
            this.rddGraphDirectedness.Label = "Graph Type:";
            this.rddGraphDirectedness.Name = "rddGraphDirectedness";
            this.rddGraphDirectedness.OfficeImageId = "DiagramReverseClassic";
            this.rddGraphDirectedness.ScreenTip = "Graph Type";
            this.rddGraphDirectedness.ShowImage = true;
            this.rddGraphDirectedness.SuperTip = resources.GetString("rddGraphDirectedness.SuperTip");
            this.rddGraphDirectedness.SelectionChanged += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.rddGraphDirectedness_SelectionChanged);
            // 
            // btnToggleGraphVisibility
            // 
            this.btnToggleGraphVisibility.Label = "Show/Hide Graph Pane";
            this.btnToggleGraphVisibility.Name = "btnToggleGraphVisibility";
            this.btnToggleGraphVisibility.OfficeImageId = "ObjectEditPoints";
            this.btnToggleGraphVisibility.ScreenTip = "Show/Hide Graph Pane";
            this.btnToggleGraphVisibility.ShowImage = true;
            this.btnToggleGraphVisibility.SuperTip = resources.GetString("btnToggleGraphVisibility.SuperTip");
            this.btnToggleGraphVisibility.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnToggleGraphVisibility_Click);
            // 
            // btnEditAutoFillUserSettings
            // 
            this.btnEditAutoFillUserSettings.Label = "AutoFill Columns";
            this.btnEditAutoFillUserSettings.Name = "btnEditAutoFillUserSettings";
            this.btnEditAutoFillUserSettings.OfficeImageId = "TableSharePointListsRefreshList";
            this.btnEditAutoFillUserSettings.ScreenTip = "AutoFill Columns";
            this.btnEditAutoFillUserSettings.ShowImage = true;
            this.btnEditAutoFillUserSettings.SuperTip = resources.GetString("btnEditAutoFillUserSettings.SuperTip");
            this.btnEditAutoFillUserSettings.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnEditAutoFillUserSettings_Click);
            // 
            // mnuImport
            // 
            this.mnuImport.Items.Add(this.btnImportPajekFile);
            this.mnuImport.Items.Add(this.btnImportEdgesFromWorkbook);
            this.mnuImport.Items.Add(this.btnAnalyzeEmailNetwork);
            this.mnuImport.Items.Add(this.btnAnalyzeTwitterNetwork);
            this.mnuImport.Label = "Import";
            this.mnuImport.Name = "mnuImport";
            this.mnuImport.OfficeImageId = "ImportSavedImports";
            this.mnuImport.ScreenTip = "Import";
            this.mnuImport.ShowImage = true;
            this.mnuImport.SuperTip = "Import graph data into this workbook.";
            // 
            // btnImportPajekFile
            // 
            this.btnImportPajekFile.Label = "From Pajek File...";
            this.btnImportPajekFile.Name = "btnImportPajekFile";
            this.btnImportPajekFile.OfficeImageId = "ImportTextFile";
            this.btnImportPajekFile.ScreenTip = "Import from Pajek File";
            this.btnImportPajekFile.ShowImage = true;
            this.btnImportPajekFile.SuperTip = resources.GetString("btnImportPajekFile.SuperTip");
            this.btnImportPajekFile.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnImportPajekFile_Click);
            // 
            // btnImportEdgesFromWorkbook
            // 
            this.btnImportEdgesFromWorkbook.Label = "From Another Open Workbook...";
            this.btnImportEdgesFromWorkbook.Name = "btnImportEdgesFromWorkbook";
            this.btnImportEdgesFromWorkbook.OfficeImageId = "ImportExcel";
            this.btnImportEdgesFromWorkbook.ScreenTip = "Import from Another Open Workbook";
            this.btnImportEdgesFromWorkbook.ShowImage = true;
            this.btnImportEdgesFromWorkbook.SuperTip = resources.GetString("btnImportEdgesFromWorkbook.SuperTip");
            this.btnImportEdgesFromWorkbook.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnImportEdgesFromWorkbook_Click);
            // 
            // btnAnalyzeEmailNetwork
            // 
            this.btnAnalyzeEmailNetwork.Label = "From Email Network Analysis...";
            this.btnAnalyzeEmailNetwork.Name = "btnAnalyzeEmailNetwork";
            this.btnAnalyzeEmailNetwork.OfficeImageId = "EnvelopesAndLabelsDialog";
            this.btnAnalyzeEmailNetwork.ScreenTip = "Import from Email Network Analysis";
            this.btnAnalyzeEmailNetwork.ShowImage = true;
            this.btnAnalyzeEmailNetwork.SuperTip = resources.GetString("btnAnalyzeEmailNetwork.SuperTip");
            this.btnAnalyzeEmailNetwork.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnAnalyzeEmailNetwork_Click);
            // 
            // btnAnalyzeTwitterNetwork
            // 
            this.btnAnalyzeTwitterNetwork.Label = "From Twitter Network Analysis...";
            this.btnAnalyzeTwitterNetwork.Name = "btnAnalyzeTwitterNetwork";
            this.btnAnalyzeTwitterNetwork.OfficeImageId = "AutoDial";
            this.btnAnalyzeTwitterNetwork.ScreenTip = "Import from Twtitter Network Analysis";
            this.btnAnalyzeTwitterNetwork.ShowImage = true;
            this.btnAnalyzeTwitterNetwork.SuperTip = "Show the friends of a Twitter user and their latest postings.";
            this.btnAnalyzeTwitterNetwork.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnAnalyzeTwitterNetwork_Click);
            // 
            // mnuExport
            // 
            this.mnuExport.Items.Add(this.btnExportSelectionToNewWorkbook);
            this.mnuExport.Label = "Export";
            this.mnuExport.Name = "mnuExport";
            this.mnuExport.OfficeImageId = "ExportSavedExports";
            this.mnuExport.ScreenTip = "Export";
            this.mnuExport.ShowImage = true;
            this.mnuExport.SuperTip = "Export graph data from this workbook.";
            // 
            // btnExportSelectionToNewWorkbook
            // 
            this.btnExportSelectionToNewWorkbook.Label = "Selection to New Workbook";
            this.btnExportSelectionToNewWorkbook.Name = "btnExportSelectionToNewWorkbook";
            this.btnExportSelectionToNewWorkbook.OfficeImageId = "RecordsMoreRecordsMenu";
            this.btnExportSelectionToNewWorkbook.ScreenTip = "Export Selection to New Workbook";
            this.btnExportSelectionToNewWorkbook.ShowImage = true;
            this.btnExportSelectionToNewWorkbook.SuperTip = "Export the selected edges and vertices to a new workbook.";
            this.btnExportSelectionToNewWorkbook.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnExportSelectionToNewWorkbook_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.Items.Add(this.btnOpenHomePage);
            this.mnuHelp.Items.Add(this.btnRegisterUser);
            this.mnuHelp.Items.Add(this.btnCheckForUpdate);
            this.mnuHelp.Items.Add(this.btnAbout);
            this.mnuHelp.Label = "Help";
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.OfficeImageId = "Help";
            this.mnuHelp.ScreenTip = "Help";
            this.mnuHelp.ShowImage = true;
            this.mnuHelp.SuperTip = "Find out more about Microsoft NodeXL.";
            // 
            // btnOpenHomePage
            // 
            this.btnOpenHomePage.Label = "Open Home Page in Browser";
            this.btnOpenHomePage.Name = "btnOpenHomePage";
            this.btnOpenHomePage.OfficeImageId = "OpenStartPage";
            this.btnOpenHomePage.ScreenTip = "Open Home Page in Browser";
            this.btnOpenHomePage.ShowImage = true;
            this.btnOpenHomePage.SuperTip = "Open the Microsoft NodeXL home page in a browser window.";
            this.btnOpenHomePage.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnOpenHomePage_Click);
            // 
            // btnRegisterUser
            // 
            this.btnRegisterUser.Label = "Register";
            this.btnRegisterUser.Name = "btnRegisterUser";
            this.btnRegisterUser.OfficeImageId = "EnvelopesAndLabelsDialog";
            this.btnRegisterUser.ScreenTip = "Register";
            this.btnRegisterUser.ShowImage = true;
            this.btnRegisterUser.SuperTip = "Register to join the Microsoft NodeXL email list.";
            this.btnRegisterUser.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnRegisterUser_Click);
            // 
            // btnCheckForUpdate
            // 
            this.btnCheckForUpdate.Label = "Check for Updates";
            this.btnCheckForUpdate.Name = "btnCheckForUpdate";
            this.btnCheckForUpdate.OfficeImageId = "FindDialog";
            this.btnCheckForUpdate.ScreenTip = "Check for Updates";
            this.btnCheckForUpdate.ShowImage = true;
            this.btnCheckForUpdate.SuperTip = "Check for a newer version of Microsoft NodeXL.";
            this.btnCheckForUpdate.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnCheckForUpdate_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Label = "About Microsoft NodeXL";
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.OfficeImageId = "Info";
            this.btnAbout.ScreenTip = "About Microsoft NodeXL";
            this.btnAbout.ShowImage = true;
            this.btnAbout.SuperTip = "View information about Microsoft NodeXL.";
            this.btnAbout.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnAbout_Click);
            // 
            // grpEdges
            // 
            this.grpEdges.Items.Add(this.btnMergeDuplicateEdges);
            this.grpEdges.Label = "Edges";
            this.grpEdges.Name = "grpEdges";
            // 
            // btnMergeDuplicateEdges
            // 
            this.btnMergeDuplicateEdges.Label = "Merge Duplicate Edges";
            this.btnMergeDuplicateEdges.Name = "btnMergeDuplicateEdges";
            this.btnMergeDuplicateEdges.OfficeImageId = "Consolidate";
            this.btnMergeDuplicateEdges.ScreenTip = "Merge Duplicate Edges";
            this.btnMergeDuplicateEdges.ShowImage = true;
            this.btnMergeDuplicateEdges.SuperTip = resources.GetString("btnMergeDuplicateEdges.SuperTip");
            this.btnMergeDuplicateEdges.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnMergeDuplicateEdges_Click);
            // 
            // grpVertices
            // 
            this.grpVertices.Items.Add(this.btnPopulateVertexWorksheet);
            this.grpVertices.Items.Add(this.btnCustomizeVertexMenu);
            this.grpVertices.Label = "Vertices";
            this.grpVertices.Name = "grpVertices";
            // 
            // btnPopulateVertexWorksheet
            // 
            this.btnPopulateVertexWorksheet.Label = "Copy Vertex Names";
            this.btnPopulateVertexWorksheet.Name = "btnPopulateVertexWorksheet";
            this.btnPopulateVertexWorksheet.OfficeImageId = "CacheListData";
            this.btnPopulateVertexWorksheet.ScreenTip = "Copy Vertex Names";
            this.btnPopulateVertexWorksheet.ShowImage = true;
            this.btnPopulateVertexWorksheet.SuperTip = resources.GetString("btnPopulateVertexWorksheet.SuperTip");
            this.btnPopulateVertexWorksheet.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnPopulateVertexWorksheet_Click);
            // 
            // btnCustomizeVertexMenu
            // 
            this.btnCustomizeVertexMenu.Label = "Customize Vertex Menus";
            this.btnCustomizeVertexMenu.Name = "btnCustomizeVertexMenu";
            this.btnCustomizeVertexMenu.OfficeImageId = "SlideMasterTextPlaceholderInsert";
            this.btnCustomizeVertexMenu.ScreenTip = "Customize Vertex Menus";
            this.btnCustomizeVertexMenu.ShowImage = true;
            this.btnCustomizeVertexMenu.SuperTip = "Customize the right-click vertex menus in the NodeXL graph.\r\n\r\nUse this to add cu" +
                "stom menu items to the menu that appears when you right-click a vertex in the .N" +
                "odeXL graph.";
            this.btnCustomizeVertexMenu.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnCustomizeVertexMenu_Click);
            // 
            // grpAnalysis
            // 
            this.grpAnalysis.Items.Add(this.spltCalculateGraphMetrics);
            this.grpAnalysis.Items.Add(this.btnCreateSubgraphImages);
            this.grpAnalysis.Items.Add(this.separator1);
            this.grpAnalysis.Items.Add(this.btnCreateClusters);
            this.grpAnalysis.Items.Add(this.chkReadClusters);
            this.grpAnalysis.Label = "Analysis";
            this.grpAnalysis.Name = "grpAnalysis";
            // 
            // spltCalculateGraphMetrics
            // 
            this.spltCalculateGraphMetrics.Items.Add(this.btnEditGraphMetricUserSettings);
            this.spltCalculateGraphMetrics.Label = "Calculate Graph Metrics";
            this.spltCalculateGraphMetrics.Name = "spltCalculateGraphMetrics";
            this.spltCalculateGraphMetrics.OfficeImageId = "Calculator";
            this.spltCalculateGraphMetrics.ScreenTip = "Calculate Graph Metrics";
            this.spltCalculateGraphMetrics.SuperTip = resources.GetString("spltCalculateGraphMetrics.SuperTip");
            this.spltCalculateGraphMetrics.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.spltCalculateGraphMetrics_Click);
            // 
            // btnEditGraphMetricUserSettings
            // 
            this.btnEditGraphMetricUserSettings.Description = "Select Graph Metrics";
            this.btnEditGraphMetricUserSettings.Label = "Select Graph Metrics";
            this.btnEditGraphMetricUserSettings.Name = "btnEditGraphMetricUserSettings";
            this.btnEditGraphMetricUserSettings.ScreenTip = "Select Graph Metrics";
            this.btnEditGraphMetricUserSettings.ShowImage = true;
            this.btnEditGraphMetricUserSettings.SuperTip = "Select the graph metrics to calculate when Calculate Graph Metrics is clicked.";
            this.btnEditGraphMetricUserSettings.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnEditGraphMetricUserSettings_Click);
            // 
            // btnCreateSubgraphImages
            // 
            this.btnCreateSubgraphImages.Label = "Create Subgraph Images";
            this.btnCreateSubgraphImages.Name = "btnCreateSubgraphImages";
            this.btnCreateSubgraphImages.OfficeImageId = "PhotoAlbumInsert";
            this.btnCreateSubgraphImages.ScreenTip = "Create Subgraph Images";
            this.btnCreateSubgraphImages.ShowImage = true;
            this.btnCreateSubgraphImages.SuperTip = "Create an image of each vertex\'s subgraph.\r\n\r\nThe images can be saved as image fi" +
                "les in a folder or inserted into the Vertices worksheet.";
            this.btnCreateSubgraphImages.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnCreateSubgraphImages_Click);
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            // 
            // btnCreateClusters
            // 
            this.btnCreateClusters.Label = "Create Clusters";
            this.btnCreateClusters.Name = "btnCreateClusters";
            this.btnCreateClusters.OfficeImageId = "TableSelectVisibleCells";
            this.btnCreateClusters.ScreenTip = "Create Clusters";
            this.btnCreateClusters.ShowImage = true;
            this.btnCreateClusters.SuperTip = resources.GetString("btnCreateClusters.SuperTip");
            this.btnCreateClusters.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnCreateClusters_Click);
            // 
            // chkReadClusters
            // 
            this.chkReadClusters.Label = "Show Clusters";
            this.chkReadClusters.Name = "chkReadClusters";
            this.chkReadClusters.ScreenTip = "Show Clusters";
            this.chkReadClusters.SuperTip = resources.GetString("chkReadClusters.SuperTip");
            // 
            // btnConvertOldWorkbook
            // 
            this.btnConvertOldWorkbook.Label = "Convert Old Workbook";
            this.btnConvertOldWorkbook.Name = "btnConvertOldWorkbook";
            this.btnConvertOldWorkbook.OfficeImageId = "RecoverInviteToMeeting";
            this.btnConvertOldWorkbook.ScreenTip = "Convert Old Workbook";
            this.btnConvertOldWorkbook.ShowImage = true;
            this.btnConvertOldWorkbook.SuperTip = "Convert an old workbook created with an earlier version of NodeXL.";
            this.btnConvertOldWorkbook.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnConvertOldWorkbook_Click);
            // 
            // Ribbon
            // 
            this.Name = "Ribbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.NodeXL);
            this.Load += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonUIEventArgs>(this.Ribbon_Load);
            this.Close += new System.EventHandler(this.Ribbon_Close);
            this.NodeXL.ResumeLayout(false);
            this.NodeXL.PerformLayout();
            this.grpGraph.ResumeLayout(false);
            this.grpGraph.PerformLayout();
            this.grpEdges.ResumeLayout(false);
            this.grpEdges.PerformLayout();
            this.grpVertices.ResumeLayout(false);
            this.grpVertices.PerformLayout();
            this.grpAnalysis.ResumeLayout(false);
            this.grpAnalysis.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab NodeXL;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnToggleGraphVisibility;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnPopulateVertexWorksheet;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAnalyzeEmailNetwork;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpAnalysis;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCustomizeVertexMenu;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAbout;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnEditAutoFillUserSettings;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCreateSubgraphImages;
        internal Microsoft.Office.Tools.Ribbon.RibbonSplitButton spltCalculateGraphMetrics;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnEditGraphMetricUserSettings;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown rddGraphDirectedness;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnMergeDuplicateEdges;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnImportEdgesFromWorkbook;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpGraph;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpEdges;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpVertices;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCreateClusters;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator1;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox chkReadClusters;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnImportPajekFile;
        internal Microsoft.Office.Tools.Ribbon.RibbonMenu mnuHelp;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCheckForUpdate;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnOpenHomePage;
        internal Microsoft.Office.Tools.Ribbon.RibbonMenu mnuImport;
        internal Microsoft.Office.Tools.Ribbon.RibbonMenu mnuExport;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnExportSelectionToNewWorkbook;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAnalyzeTwitterNetwork;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnRegisterUser;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnConvertOldWorkbook;
    }

    partial class ThisRibbonCollection : Microsoft.Office.Tools.Ribbon.RibbonReadOnlyCollection
    {
        internal Ribbon Ribbon
        {
            get { return this.GetRibbon<Ribbon>(); }
        }
    }
}
