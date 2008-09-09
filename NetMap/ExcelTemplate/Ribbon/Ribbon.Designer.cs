

//	Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.NetMap.ExcelTemplate
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
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItem1 = new Microsoft.Office.Tools.Ribbon.RibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItem2 = new Microsoft.Office.Tools.Ribbon.RibbonDropDownItem();
            this.NetMap = new Microsoft.Office.Tools.Ribbon.RibbonTab();
            this.grpGraph = new Microsoft.Office.Tools.Ribbon.RibbonGroup();
            this.rddGraphDirectedness = new Microsoft.Office.Tools.Ribbon.RibbonDropDown();
            this.btnToggleGraphVisibility = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.EditAutoFillUserSettings = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnAbout = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.grpEdges = new Microsoft.Office.Tools.Ribbon.RibbonGroup();
            this.btnImportEdges = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnMergeDuplicateEdges = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.grpVertices = new Microsoft.Office.Tools.Ribbon.RibbonGroup();
            this.btnPopulateVertexWorksheet = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnCustomizeVertexMenu = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.grpAnalysis = new Microsoft.Office.Tools.Ribbon.RibbonGroup();
            this.spltCalculateGraphMetrics = new Microsoft.Office.Tools.Ribbon.RibbonSplitButton();
            this.btnEditGraphMetricUserSettings = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnCreateClusters = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnCreateSubgraphImages = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnAnalyzeEmailNetwork = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.NetMap.SuspendLayout();
            this.grpGraph.SuspendLayout();
            this.grpEdges.SuspendLayout();
            this.grpVertices.SuspendLayout();
            this.grpAnalysis.SuspendLayout();
            this.SuspendLayout();
            // 
            // NetMap
            // 
            this.NetMap.Groups.Add(this.grpGraph);
            this.NetMap.Groups.Add(this.grpEdges);
            this.NetMap.Groups.Add(this.grpVertices);
            this.NetMap.Groups.Add(this.grpAnalysis);
            this.NetMap.Label = ".NetMap";
            this.NetMap.Name = "NetMap";
            // 
            // grpGraph
            // 
            this.grpGraph.Items.Add(this.rddGraphDirectedness);
            this.grpGraph.Items.Add(this.btnToggleGraphVisibility);
            this.grpGraph.Items.Add(this.EditAutoFillUserSettings);
            this.grpGraph.Items.Add(this.btnAbout);
            this.grpGraph.Label = "Graph";
            this.grpGraph.Name = "grpGraph";
            // 
            // rddGraphDirectedness
            // 
            this.rddGraphDirectedness.Enabled = false;
            ribbonDropDownItem1.Label = "Directed";
            ribbonDropDownItem1.OfficeImageId = "ShapeStraightConnectorArrow";
            ribbonDropDownItem1.ScreenTip = "Edges start at Vertex 1 and end at Vertex 2.";
            ribbonDropDownItem1.Tag = Microsoft.NetMap.Core.GraphDirectedness.Directed;
            ribbonDropDownItem2.Label = "Undirected";
            ribbonDropDownItem2.OfficeImageId = "ShapeStraightConnector";
            ribbonDropDownItem2.ScreenTip = "Edges do not have a start or end.  Vertex 1 and Vertex 2 are interchangable.";
            ribbonDropDownItem2.Tag = Microsoft.NetMap.Core.GraphDirectedness.Undirected;
            this.rddGraphDirectedness.Items.Add(ribbonDropDownItem1);
            this.rddGraphDirectedness.Items.Add(ribbonDropDownItem2);
            this.rddGraphDirectedness.Label = "Graph Type:";
            this.rddGraphDirectedness.Name = "rddGraphDirectedness";
            this.rddGraphDirectedness.OfficeImageId = "DiagramReverseClassic";
            this.rddGraphDirectedness.ScreenTip = "Specify whether the graph is directed or undirected.";
            this.rddGraphDirectedness.ShowImage = true;
            this.rddGraphDirectedness.SelectionChanged += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.rddGraphDirectedness_SelectionChanged);
            // 
            // btnToggleGraphVisibility
            // 
            this.btnToggleGraphVisibility.Label = "Show/Hide Graph Pane";
            this.btnToggleGraphVisibility.Name = "btnToggleGraphVisibility";
            this.btnToggleGraphVisibility.OfficeImageId = "ObjectEditPoints";
            this.btnToggleGraphVisibility.ScreenTip = "Show or hide the .NetMap graph pane.";
            this.btnToggleGraphVisibility.ShowImage = true;
            this.btnToggleGraphVisibility.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnToggleGraphVisibility_Click);
            // 
            // EditAutoFillUserSettings
            // 
            this.EditAutoFillUserSettings.Label = "AutoFill Columns";
            this.EditAutoFillUserSettings.Name = "EditAutoFillUserSettings";
            this.EditAutoFillUserSettings.OfficeImageId = "TableSharePointListsRefreshList";
            this.EditAutoFillUserSettings.ScreenTip = "Automatically fill in the edge and vertex attribute columns when the workbook is " +
                "read into the graph.";
            this.EditAutoFillUserSettings.ShowImage = true;
            this.EditAutoFillUserSettings.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.EditAutoFillUserSettings_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Label = "About";
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.OfficeImageId = "Info";
            this.btnAbout.ScreenTip = "Get information about .NetMap.";
            this.btnAbout.ShowImage = true;
            this.btnAbout.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnAbout_Click);
            // 
            // grpEdges
            // 
            this.grpEdges.Items.Add(this.btnImportEdges);
            this.grpEdges.Items.Add(this.btnMergeDuplicateEdges);
            this.grpEdges.Label = "Edges";
            this.grpEdges.Name = "grpEdges";
            // 
            // btnImportEdges
            // 
            this.btnImportEdges.Label = "Import Edges";
            this.btnImportEdges.Name = "btnImportEdges";
            this.btnImportEdges.OfficeImageId = "ImportExcel";
            this.btnImportEdges.ScreenTip = "Import edges from another open workbook.";
            this.btnImportEdges.ShowImage = true;
            this.btnImportEdges.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnImportEdges_Click);
            // 
            // btnMergeDuplicateEdges
            // 
            this.btnMergeDuplicateEdges.Label = "Merge Duplicate Edges";
            this.btnMergeDuplicateEdges.Name = "btnMergeDuplicateEdges";
            this.btnMergeDuplicateEdges.OfficeImageId = "Consolidate";
            this.btnMergeDuplicateEdges.ScreenTip = "Merge edges that connect the same vertices.  (This removes any filters from the E" +
                "dges worksheet.)";
            this.btnMergeDuplicateEdges.ShowImage = true;
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
            this.btnPopulateVertexWorksheet.ScreenTip = "Copy each unique vertex name from the Edges worksheet to the Vertices worksheet.";
            this.btnPopulateVertexWorksheet.ShowImage = true;
            this.btnPopulateVertexWorksheet.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnPopulateVertexWorksheet_Click);
            // 
            // btnCustomizeVertexMenu
            // 
            this.btnCustomizeVertexMenu.Label = "Customize Vertex Menus";
            this.btnCustomizeVertexMenu.Name = "btnCustomizeVertexMenu";
            this.btnCustomizeVertexMenu.OfficeImageId = "SlideMasterTextPlaceholderInsert";
            this.btnCustomizeVertexMenu.ScreenTip = "Customize the right-click vertex menus in the .NetMap graph.";
            this.btnCustomizeVertexMenu.ShowImage = true;
            this.btnCustomizeVertexMenu.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnCustomizeVertexMenu_Click);
            // 
            // grpAnalysis
            // 
            this.grpAnalysis.Items.Add(this.spltCalculateGraphMetrics);
            this.grpAnalysis.Items.Add(this.btnCreateClusters);
            this.grpAnalysis.Items.Add(this.btnCreateSubgraphImages);
            this.grpAnalysis.Items.Add(this.btnAnalyzeEmailNetwork);
            this.grpAnalysis.Label = "Analysis";
            this.grpAnalysis.Name = "grpAnalysis";
            // 
            // spltCalculateGraphMetrics
            // 
            this.spltCalculateGraphMetrics.Items.Add(this.btnEditGraphMetricUserSettings);
            this.spltCalculateGraphMetrics.Label = "Calculate Graph Metrics";
            this.spltCalculateGraphMetrics.Name = "spltCalculateGraphMetrics";
            this.spltCalculateGraphMetrics.OfficeImageId = "Calculator";
            this.spltCalculateGraphMetrics.ScreenTip = "Calculate the selected graph metrics and insert the results into the workbook.";
            this.spltCalculateGraphMetrics.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.spltCalculateGraphMetrics_Click);
            // 
            // btnEditGraphMetricUserSettings
            // 
            this.btnEditGraphMetricUserSettings.Label = "Select Graph Metrics";
            this.btnEditGraphMetricUserSettings.Name = "btnEditGraphMetricUserSettings";
            this.btnEditGraphMetricUserSettings.ScreenTip = "Select which graph metrics to calculate.";
            this.btnEditGraphMetricUserSettings.ShowImage = true;
            this.btnEditGraphMetricUserSettings.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnEditGraphMetricUserSettings_Click);
            // 
            // btnCreateClusters
            // 
            this.btnCreateClusters.Label = "Create Clusters";
            this.btnCreateClusters.Name = "btnCreateClusters";
            this.btnCreateClusters.OfficeImageId = "TableSelectVisibleCells";
            this.btnCreateClusters.ScreenTip = "Partition the graph into clusters.";
            this.btnCreateClusters.ShowImage = true;
            this.btnCreateClusters.Visible = false;
            this.btnCreateClusters.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnCreateClusters_Click);
            // 
            // btnCreateSubgraphImages
            // 
            this.btnCreateSubgraphImages.Label = "Create Subgraph Images";
            this.btnCreateSubgraphImages.Name = "btnCreateSubgraphImages";
            this.btnCreateSubgraphImages.OfficeImageId = "PhotoAlbumInsert";
            this.btnCreateSubgraphImages.ScreenTip = "Create an image of each vertex\'s subgraph.";
            this.btnCreateSubgraphImages.ShowImage = true;
            this.btnCreateSubgraphImages.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnCreateSubgraphImages_Click);
            // 
            // btnAnalyzeEmailNetwork
            // 
            this.btnAnalyzeEmailNetwork.Label = "Analyze Email Network";
            this.btnAnalyzeEmailNetwork.Name = "btnAnalyzeEmailNetwork";
            this.btnAnalyzeEmailNetwork.OfficeImageId = "FileCreateDocumentWorkspace";
            this.btnAnalyzeEmailNetwork.ScreenTip = "Analyze the relationships between the people in my email network.";
            this.btnAnalyzeEmailNetwork.ShowImage = true;
            this.btnAnalyzeEmailNetwork.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnAnalyzeEmailNetwork_Click);
            // 
            // Ribbon
            // 
            this.Name = "Ribbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.NetMap);
            this.Load += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonUIEventArgs>(this.Ribbon_Load);
            this.NetMap.ResumeLayout(false);
            this.NetMap.PerformLayout();
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

        internal Microsoft.Office.Tools.Ribbon.RibbonTab NetMap;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnToggleGraphVisibility;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnPopulateVertexWorksheet;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAnalyzeEmailNetwork;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpAnalysis;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCustomizeVertexMenu;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAbout;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton EditAutoFillUserSettings;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCreateSubgraphImages;
        internal Microsoft.Office.Tools.Ribbon.RibbonSplitButton spltCalculateGraphMetrics;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnEditGraphMetricUserSettings;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown rddGraphDirectedness;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnMergeDuplicateEdges;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnImportEdges;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpGraph;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpEdges;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpVertices;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCreateClusters;
    }

    partial class ThisRibbonCollection : Microsoft.Office.Tools.Ribbon.RibbonReadOnlyCollection
    {
        internal Ribbon Ribbon
        {
            get { return this.GetRibbon<Ribbon>(); }
        }
    }
}
