

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
            this.tab1 = new Microsoft.Office.Tools.Ribbon.RibbonTab();
            this.NetMapGraph = new Microsoft.Office.Tools.Ribbon.RibbonGroup();
            this.btnToggleGraphVisibility = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnPopulateVertexWorksheet = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.EditAutoFillUserSettings = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnCustomizeVertexMenu = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnAbout = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.NetMapAnalysis = new Microsoft.Office.Tools.Ribbon.RibbonGroup();
            this.spltCalculateGraphMetrics = new Microsoft.Office.Tools.Ribbon.RibbonSplitButton();
            this.btnEditGraphMetricUserSettings = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnCreateSubgraphImages = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnAnalyzeEmailNetwork = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.tab1.SuspendLayout();
            this.NetMapGraph.SuspendLayout();
            this.NetMapAnalysis.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.NetMapGraph);
            this.tab1.Groups.Add(this.NetMapAnalysis);
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            // 
            // NetMapGraph
            // 
            this.NetMapGraph.Items.Add(this.btnToggleGraphVisibility);
            this.NetMapGraph.Items.Add(this.btnPopulateVertexWorksheet);
            this.NetMapGraph.Items.Add(this.EditAutoFillUserSettings);
            this.NetMapGraph.Items.Add(this.btnCustomizeVertexMenu);
            this.NetMapGraph.Items.Add(this.btnAbout);
            this.NetMapGraph.Label = "Microsoft .NetMap Graph";
            this.NetMapGraph.Name = "NetMapGraph";
            // 
            // btnToggleGraphVisibility
            // 
            this.btnToggleGraphVisibility.Label = "Show/Hide Graph";
            this.btnToggleGraphVisibility.Name = "btnToggleGraphVisibility";
            this.btnToggleGraphVisibility.OfficeImageId = "ObjectEditPoints";
            this.btnToggleGraphVisibility.ScreenTip = "Show or hide the .NetMap graph pane.";
            this.btnToggleGraphVisibility.ShowImage = true;
            this.btnToggleGraphVisibility.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnToggleGraphVisibility_Click);
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
            // btnCustomizeVertexMenu
            // 
            this.btnCustomizeVertexMenu.Label = "Customize Vertex Menus";
            this.btnCustomizeVertexMenu.Name = "btnCustomizeVertexMenu";
            this.btnCustomizeVertexMenu.OfficeImageId = "SlideMasterTextPlaceholderInsert";
            this.btnCustomizeVertexMenu.ScreenTip = "Customize the right-click vertex menus in the .NetMap graph.";
            this.btnCustomizeVertexMenu.ShowImage = true;
            this.btnCustomizeVertexMenu.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnCustomizeVertexMenu_Click);
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
            // NetMapAnalysis
            // 
            this.NetMapAnalysis.Items.Add(this.spltCalculateGraphMetrics);
            this.NetMapAnalysis.Items.Add(this.btnCreateSubgraphImages);
            this.NetMapAnalysis.Items.Add(this.btnAnalyzeEmailNetwork);
            this.NetMapAnalysis.Label = "Microsoft .NetMap Analysis";
            this.NetMapAnalysis.Name = "NetMapAnalysis";
            // 
            // spltCalculateGraphMetrics
            // 
            this.spltCalculateGraphMetrics.Items.Add(this.btnEditGraphMetricUserSettings);
            this.spltCalculateGraphMetrics.Label = "Calculate Graph Metrics";
            this.spltCalculateGraphMetrics.Name = "spltCalculateGraphMetrics";
            this.spltCalculateGraphMetrics.OfficeImageId = "SelectCurrentRegion";
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
            // btnCreateSubgraphImages
            // 
            this.btnCreateSubgraphImages.Label = "Create Subgraph Images";
            this.btnCreateSubgraphImages.Name = "btnCreateSubgraphImages";
            this.btnCreateSubgraphImages.OfficeImageId = "SaveAll";
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
            this.Tabs.Add(this.tab1);
            this.Load += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonUIEventArgs>(this.Ribbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.NetMapGraph.ResumeLayout(false);
            this.NetMapGraph.PerformLayout();
            this.NetMapAnalysis.ResumeLayout(false);
            this.NetMapAnalysis.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup NetMapGraph;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnToggleGraphVisibility;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnPopulateVertexWorksheet;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAnalyzeEmailNetwork;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup NetMapAnalysis;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCustomizeVertexMenu;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAbout;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton EditAutoFillUserSettings;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCreateSubgraphImages;
        internal Microsoft.Office.Tools.Ribbon.RibbonSplitButton spltCalculateGraphMetrics;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnEditGraphMetricUserSettings;
    }

    partial class ThisRibbonCollection : Microsoft.Office.Tools.Ribbon.RibbonReadOnlyCollection
    {
        internal Ribbon Ribbon
        {
            get { return this.GetRibbon<Ribbon>(); }
        }
    }
}
