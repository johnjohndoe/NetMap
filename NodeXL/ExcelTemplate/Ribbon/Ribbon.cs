

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization.Wpf;
using Microsoft.NodeXL.ApplicationUtil;
using Microsoft.NodeXL.ExcelTemplatePlugIns;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: Ribbon
//
/// <summary>
/// Implements the template's ribbon customizations.
/// </summary>
//*****************************************************************************

public partial class Ribbon : OfficeRibbon
{
    //*************************************************************************
    //  Constructor: Ribbon()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="Ribbon" /> class.
    /// </summary>
    //*************************************************************************

    public Ribbon()
    {
        InitializeComponent();

        // Initialize the application's user settings.

        UserSettingsManager.Initialize();

        // Populate the rddLayouts RibbonDropDown.

        m_oLayoutManagerForRibbonDropDown =
            new LayoutManagerForRibbonDropDown();

        m_oLayoutManagerForRibbonDropDown.AddRibbonDropDownItems(
            this.rddLayout);

        m_oLayoutManagerForRibbonDropDown.LayoutChanged += new
            EventHandler(this.m_oLayoutManagerForRibbonDropDown_LayoutChanged);

        // Read the general user settings directly controlled by the ribbon.

        GeneralUserSettings oGeneralUserSettings = new GeneralUserSettings();

        this.ReadClusters = oGeneralUserSettings.ReadClusters;
        this.ShowGraphLegend = oGeneralUserSettings.ShowGraphLegend;
        this.ShowGraphAxes = oGeneralUserSettings.ShowGraphAxes;

        this.ClearTablesBeforeImport =
            oGeneralUserSettings.ClearTablesBeforeImport;

        AssertValid();
    }

    //*************************************************************************
    //  Property: ReadClusters
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the cluster worksheets should be
    /// read when the workbook is read into the graph.
    /// </summary>
    ///
    /// <value>
    /// true to read the cluster worksheets.
    /// </value>
    //*************************************************************************

    public Boolean
    ReadClusters
    {
        get
        {
            AssertValid();

            return (cbxReadClusters.Checked);
        }

        set
        {
            cbxReadClusters.Checked = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: GraphDirectedness
    //
    /// <summary>
    /// Gets or sets the graph directedness of the active workbook.
    /// </summary>
    ///
    /// <value>
    /// A GraphDirectedness value.
    /// </value>
    //*************************************************************************

    public GraphDirectedness
    GraphDirectedness
    {
        get
        {
            AssertValid();

            // The user selects the directedness with the rddGraphDirectedness
            // RibbonDropDown.  The GraphDirectedness for each item is stored
            // in the item's Tag.

            return ( (GraphDirectedness)rddGraphDirectedness.SelectedItem.Tag );
        }

        set
        {
            foreach (RibbonDropDownItem oItem in rddGraphDirectedness.Items)
            {
                if ( (GraphDirectedness)oItem.Tag == value )
                {
                    rddGraphDirectedness.SelectedItem = oItem;

                    break;
                }
            }

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Layout
    //
    /// <summary>
    /// Gets or sets the layout type to use.
    /// </summary>
    ///
    /// <value>
    /// The layout type to use, as a <see cref="LayoutType" />.
    /// </value>
    //*************************************************************************

    public LayoutType
    Layout
    {
        get
        {
            AssertValid();

            return (m_oLayoutManagerForRibbonDropDown.Layout);
        }

        set
        {
            m_oLayoutManagerForRibbonDropDown.Layout = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ShowGraphLegend
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the graph legend should be shown
    /// in the graph pane.
    /// </summary>
    ///
    /// <value>
    /// true to show the graph legend in the graph pane.
    /// </value>
    //*************************************************************************

    public Boolean
    ShowGraphLegend
    {
        get
        {
            AssertValid();

            return (cbxShowGraphLegend.Checked);
        }

        set
        {
            cbxShowGraphLegend.Checked = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ShowGraphAxes
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the graph axes should be shown
    /// in the graph pane.
    /// </summary>
    ///
    /// <value>
    /// true to show the graph axes in the graph pane.
    /// </value>
    //*************************************************************************

    public Boolean
    ShowGraphAxes
    {
        get
        {
            AssertValid();

            return (cbxShowGraphAxes.Checked);
        }

        set
        {
            cbxShowGraphAxes.Checked = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ReadWorkbookButtonText
    //
    /// <summary>
    /// Sets the text for the read workbook button.
    /// </summary>
    ///
    /// <value>
    /// The text for the read workbook button.
    /// </value>
    //*************************************************************************

    public String
    ReadWorkbookButtonText
    {
        set
        {
            btnReadWorkbook.Label = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ClearTablesBeforeImport
    //
    /// <summary>
    /// Gets or sets a flag indicating whether NodeXL tables should be cleared
    /// before anything is imported into the workbook.
    /// </summary>
    ///
    /// <value>
    /// If true, the NodeXL tables are cleared before anything is imported into
    /// the workbook.  If false, the imported contents are appended to the
    /// workbook.
    /// </value>
    //*************************************************************************

    public Boolean
    ClearTablesBeforeImport
    {
        get
        {
            AssertValid();

            return (cbxClearTablesBeforeImport.Checked);
        }

        set
        {
            cbxClearTablesBeforeImport.Checked = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: EnableSetVisualAttributes
    //
    /// <summary>
    /// Enables the "set visual attribute" buttons.
    /// </summary>
    ///
    /// <param name="visualAttributes">
    /// ORed flags specifying which buttons to enable.
    /// </param>
    //*************************************************************************

    public void
    EnableSetVisualAttributes
    (
        VisualAttributes visualAttributes
    )
    {
        AssertValid();

        this.btnSetColor.Enabled =
            ( (visualAttributes & VisualAttributes.Color) != 0 );

        this.btnSetAlpha.Enabled =
            ( (visualAttributes & VisualAttributes.Alpha) != 0 );

        this.btnSetEdgeWidth.Enabled =
            ( (visualAttributes & VisualAttributes.EdgeWidth) != 0 );

        this.mnuSetVertexShape.Enabled =
            ( (visualAttributes & VisualAttributes.VertexShape) != 0 );

        this.btnSetVertexRadius.Enabled =
            ( (visualAttributes & VisualAttributes.VertexRadius) != 0 );

        // The mnuSetVisibility menu has a set of child buttons for edge
        // visibility and another set for vertex visibility.  Show only one
        // of the sets.

        Boolean bShowSetEdgeVisibility =
            ( (visualAttributes & VisualAttributes.EdgeVisibility) != 0 );

        Boolean bShowSetVertexVisibility =
            ( (visualAttributes & VisualAttributes.VertexVisibility) != 0 );

        this.mnuSetVisibility.Enabled =
            (bShowSetEdgeVisibility || bShowSetVertexVisibility);

        this.btnSetEdgeVisibilityShow.Visible =
            this.btnSetEdgeVisibilitySkip.Visible =
            this.btnSetEdgeVisibilityHide.Visible =
            bShowSetEdgeVisibility;

        this.btnSetVertexVisibilityShowIfInAnEdge.Visible =
            this.btnSetVertexVisibilitySkip.Visible =
            this.btnSetVertexVisibilityHide.Visible =
            this.btnSetVertexVisibilityShow.Visible =
            bShowSetVertexVisibility;
    }

    //*************************************************************************
    //  Property: EnableShowDynamicFilters
    //
    /// <summary>
    /// Sets a flag indicating whether the "show dynamic filters" button should
    /// be enabled.
    /// </summary>
    ///
    /// <value>
    /// true to enable the dynamic filters button.
    /// </value>
    //*************************************************************************

    public Boolean
    EnableShowDynamicFilters
    {
        set
        {
            btnShowDynamicFilters.Enabled = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Event: RunRibbonCommand
    //
    /// <summary>
    /// Occurs when a ribbon command must be executed elsewhere in the
    /// application.
    /// </summary>
    //*************************************************************************

    public event RunRibbonCommandEventHandler RunRibbonCommand;


    //*************************************************************************
    //  Event: RibbonControlsChanged
    //
    /// <summary>
    /// Occurs when the state of one or more controls in the ribbon changes.
    /// </summary>
    ///
    /// <remarks>
    /// This event fires only for controls in the <see cref="RibbonControl" />
    /// enumeration.  It does not fire for ribbon controls in general.
    /// </remarks>
    //*************************************************************************

    public event RibbonControlsChangedEventHandler RibbonControlsChanged;


    //*************************************************************************
    //  Property: ThisWorkbook
    //
    /// <summary>
    /// Gets the workbook this ribbon is attached to.
    /// </summary>
    ///
    /// <value>
    /// The workbook this ribbon is attached to.
    /// </value>
    //*************************************************************************

    protected ThisWorkbook
    ThisWorkbook
    {
        get
        {
            AssertValid();

            return (Globals.ThisWorkbook);
        }
    }

    //*************************************************************************
    //  Method: GetPerWorkbookSettings()
    //
    /// <summary>
    /// Gets a new PerWorkbookSettings object.
    /// </summary>
    ///
    /// <returns>
    /// A new PerWorkbookSettings object.
    /// </returns>
    //*************************************************************************

    protected PerWorkbookSettings
    GetPerWorkbookSettings()
    {
        AssertValid();

        return ( new PerWorkbookSettings(this.ThisWorkbook.InnerObject) );
    }

    //*************************************************************************
    //  Method: FireRibbonControlsChangedEvent()
    //
    /// <summary>
    /// Fires the <see cref="RibbonControlsChanged" /> event if appropriate.
    /// </summary>
    ///
    /// <param name="eRibbonControls">
    /// The controls in the ribbon whose states have changed.
    /// </param>
    //*************************************************************************

    protected void
    FireRibbonControlsChangedEvent
    (
        RibbonControls eRibbonControls
    )
    {
        AssertValid();

        RibbonControlsChangedEventHandler oRibbonControlsChanged =
            this.RibbonControlsChanged;

        if (oRibbonControlsChanged != null)
        {
            oRibbonControlsChanged( this,
                new RibbonControlsChangedEventArgs(eRibbonControls) );
        }
    }

    //*************************************************************************
    //  Method: FireRunRibbonCommandEvent()
    //
    /// <summary>
    /// Fires the <see cref="RunRibbonCommand" /> event if appropriate.
    /// </summary>
    ///
    /// <param name="eRibbonCommand">
    /// The ribbon command to run.
    /// </param>
    //*************************************************************************

    protected void
    FireRunRibbonCommandEvent
    (
        RibbonCommand eRibbonCommand
    )
    {
        AssertValid();

        RunRibbonCommandEventHandler oRunRibbonCommand = this.RunRibbonCommand;

        if (oRunRibbonCommand != null)
        {
            oRunRibbonCommand( this,
                new RunRibbonCommandEventArgs(eRibbonCommand) );
        }
    }

    //*************************************************************************
    //  Method: Ribbon_Load()
    //
    /// <summary>
    /// Handles the Load event on the ribbon.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    Ribbon_Load
    (
        object sender,
        RibbonUIEventArgs e
    )
    {
        AssertValid();

        // The graph directedness RibbonDropDown should be enabled only if the
        // template the workbook is based on supports changing the
        // directedness.
        //
        // This is done here rather than in the constructor because a
        // PerWorkbookSettings object can't be created until this.ThisWorkbook
        // is available, which doesn't happen until the Ribbon is fully loaded.

        Int32 iTemplateVersion = GetPerWorkbookSettings().TemplateVersion;

        rddGraphDirectedness.Enabled = (iTemplateVersion >= 51);

        // The ability to create clusters depends on the template version.

        btnCreateClusters.Enabled = (iTemplateVersion >= 54);
    }

    //*************************************************************************
    //  Method: Ribbon_Close()
    //
    /// <summary>
    /// Handles the Close event on the ribbon.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    Ribbon_Close
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        // Set the general user settings directly controlled by the ribbon.

        GeneralUserSettings oGeneralUserSettings = new GeneralUserSettings();

        oGeneralUserSettings.ReadClusters = this.ReadClusters;
        oGeneralUserSettings.ShowGraphLegend = this.ShowGraphLegend;
        oGeneralUserSettings.ShowGraphAxes = this.ShowGraphAxes;

        oGeneralUserSettings.ClearTablesBeforeImport =
            this.ClearTablesBeforeImport;

        oGeneralUserSettings.Save();

        // Close the application's user settings.

        UserSettingsManager.Close();
    }

    //*************************************************************************
    //  Method: mnuImport_ItemsLoading()
    //
    /// <summary>
    /// Handles the ItemsLoading event on the mnuImport RibbonMenu.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    mnuImport_ItemsLoading
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        if (mnuImport.Tag is Boolean)
        {
            // This method has already been called.

            return;
        }

        // Get an array of plug-in classes that implement IGraphDataProvider.

        IGraphDataProvider [] oGraphDataProviders;
        this.ThisWorkbook.ShowWaitCursor = true;

        try
        {
            oGraphDataProviders = PlugInManager.GetGraphDataProviders();
        }
        finally
        {
            this.ThisWorkbook.ShowWaitCursor = false;
        }

        Int32 iGraphDataProviders = oGraphDataProviders.Length;

        if (iGraphDataProviders == 0)
        {
            return;
        }

        // For each such class, add a child menu item to the Import menu.

        RibbonComponentCollection<RibbonControl> oImportItems =
            mnuImport.Items;

        Int32 iInsertionIndex =
            oImportItems.IndexOf(btnAnalyzeEmailNetwork) + 1;

        Debug.Assert(iInsertionIndex > 0);

        oImportItems.Insert( iInsertionIndex, new RibbonSeparator() );
        iInsertionIndex++;

        foreach (IGraphDataProvider oGraphDataProvider in oGraphDataProviders)
        {
            RibbonButton oRibbonButton = new RibbonButton();

            String sGraphDataProviderName = oGraphDataProvider.Name;
            oRibbonButton.Label = "From " + sGraphDataProviderName + "...";
            oRibbonButton.ScreenTip = "Import from " + sGraphDataProviderName;
            oRibbonButton.Tag = oGraphDataProvider;

            oRibbonButton.SuperTip =
                "Optionally clear the NodeXL workbook, then "
                + oGraphDataProvider.Description + "."
                ;

            oRibbonButton.Click += new EventHandler<RibbonControlEventArgs>(
                this.btnImportFromGraphDataProvider_Click);

            oImportItems.Insert(iInsertionIndex, oRibbonButton);
            iInsertionIndex++;
        }

        // Prevent menu items from being added again by using the Tag to store
        // a flag.

        mnuImport.Tag = true;
    }

    //*************************************************************************
    //  Method: btnImportFromGraphDataProvider_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnImportFromGraphDataProvider buttons.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnImportFromGraphDataProvider_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        // The plug-in class that implements IGraphDataProvider was stored in
        // the Tag of the RibbonButton by mnuImport_ItemsLoading().

        Debug.Assert(sender is RibbonControl);
        Debug.Assert( ( (RibbonControl)sender ).Tag is IGraphDataProvider );

        this.ThisWorkbook.ImportFromGraphDataProvider(
            (IGraphDataProvider)( (RibbonControl)sender ).Tag);
    }

    //*************************************************************************
    //  Method: btnImportFromMatrixWorkbook_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnImportFromMatrixWorkbook button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnImportFromMatrixWorkbook_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.ImportFromMatrixWorkbook();
    }

    //*************************************************************************
    //  Method: btnImportFromEdgeWorkbook_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnImportFromEdgeWorkbook button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnImportFromEdgeWorkbook_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.ImportFromEdgeWorkbook();
    }

    //*************************************************************************
    //  Method: btnExportSelectionToNewNodeXLWorkbook_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnExportSelectionToNewNodeXLWorkbook
    /// button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnExportSelectionToNewNodeXLWorkbook_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.ExportSelectionToNewNodeXLWorkbook();
    }

    //*************************************************************************
    //  Method: btnExportToUcinetFile_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnExportToUcinetFile button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnExportToUcinetFile_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.ExportToUcinetFile();
    }

    //*************************************************************************
    //  Method: btnExportToPajekFile_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnExportToPajekFile button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnExportToPajekFile_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.ExportToPajekFile();
    }

    //*************************************************************************
    //  Method: btnExportToNewMatrixWorkbook()
    //
    /// <summary>
    /// Handles the Click event on the btnExportToNewMatrixWorkbook button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnExportToNewMatrixWorkbook_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.ExportToNewMatrixWorkbook();
    }

    //*************************************************************************
    //  Method: rddGraphDirectedness_SelectionChanged()
    //
    /// <summary>
    /// Handles the SelectionChanged event on the rddGraphDirectedness
    /// RibbonDropDown.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    rddGraphDirectedness_SelectionChanged
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        GraphDirectedness eGraphDirectedness = this.GraphDirectedness;

        if ( !this.ThisWorkbook.ExcelApplicationIsReady(true) )
        {
            // Cancel the change.

            this.GraphDirectedness =
                (eGraphDirectedness == GraphDirectedness.Directed) ?
                GraphDirectedness.Undirected : GraphDirectedness.Directed;
        }
        else
        {
            // Notify the workbook.

            this.ThisWorkbook.GraphDirectedness = eGraphDirectedness;
        }
    }

    //*************************************************************************
    //  Method: m_oLayoutManagerForRibbonDropDown_LayoutChanged()
    //
    /// <summary>
    /// Handles the LayoutChanged event on the
    /// m_oLayoutManagerForRibbonDropDown.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    m_oLayoutManagerForRibbonDropDown_LayoutChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        FireRibbonControlsChangedEvent(RibbonControls.Layout);
    }

    //*************************************************************************
    //  Method: btnReadWorkbook_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnReadWorkbook button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnReadWorkbook_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        // Make sure the graph is showing, then tell the TaskPane to read the
        // workbook.

        this.ThisWorkbook.ShowGraph();

        FireRunRibbonCommandEvent(RibbonCommand.ReadWorkbook);
    }

    //*************************************************************************
    //  Method: btnShowDynamicFilters_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnShowDynamicFilters button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnShowDynamicFilters_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        FireRunRibbonCommandEvent(RibbonCommand.ShowDynamicFilters);
    }

    //*************************************************************************
    //  Method: btnMergeDuplicateEdges_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnMergeDuplicateEdges button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnMergeDuplicateEdges_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.MergeDuplicateEdges();
    }

    //*************************************************************************
    //  Method: btnPopulateVertexWorksheet_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnPopulateVertexWorksheet button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnPopulateVertexWorksheet_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.PopulateVertexWorksheet(true, true);
    }

    //*************************************************************************
    //  Method: btnShowGraphMetrics_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnShowGraphMetrics button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnShowGraphMetrics_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.ShowGraphMetrics();
    }

    //*************************************************************************
    //  Method: btnCreateClusters_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnCreateClusters button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnCreateClusters_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.CreateClusters();
    }

    //*************************************************************************
    //  Method: btnCreateSubgraphImages_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnCreateSubgraphImages Button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnCreateSubgraphImages_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.CreateSubgraphImages();
    }

    //*************************************************************************
    //  Method: btnDeleteSubgraphThumbnails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnDeleteSubgraphThumbnails Button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnDeleteSubgraphThumbnails_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        if ( MessageBox.Show(

                "This will delete any subgraph thumbnails in the Vertices"
                + " worksheet.  It will not delete any subgraph image files"
                + " you saved in a folder."
                + "\r\n\r\n"
                + "Do you want to delete subgraph thumbnails?"
                ,
                ApplicationUtil.ApplicationName, MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                == DialogResult.Yes
            )
        {
            this.ThisWorkbook.DeleteSubgraphThumbnails();
        }
    }

    //*************************************************************************
    //  Method: btnAnalyzeEmailNetwork_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnAnalyzeEmailNetwork button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnAnalyzeEmailNetwork_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.AnalyzeEmailNetwork();
    }

    //*************************************************************************
    //  Method: btnAutoFillWorkbook_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnAutoFillWorkbook button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnAutoFillWorkbook_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.AutoFillWorkbook();
    }

    //*************************************************************************
    //  Method: btnAutoFillWorkbookWithScheme_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnAutoFillWorkbookWithScheme button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnAutoFillWorkbookWithScheme_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.AutoFillWorkbookWithScheme();
    }

    //*************************************************************************
    //  Method: btnSetVisualAttribute_Click()
    //
    /// <summary>
    /// Handles the Click event on several of the "set visual attribute"
    /// buttons.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    ///
    /// <remarks>
    /// This handles the click event for all "set visual attribute" buttons
    /// for which the visual attribute value isn't known yet and must be
    /// obtained from the user.
    /// </remarks>
    //*************************************************************************

    private void
    btnSetVisualAttribute_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        // A VisualAttributes flag is stored in the Tag of each visual
        // attribute button.

        Debug.Assert(sender is RibbonComponent);
        Debug.Assert( ( (RibbonComponent)sender ).Tag is VisualAttributes );

        this.ThisWorkbook.SetVisualAttribute(
            (VisualAttributes)( (RibbonComponent)sender ).Tag, null);
    }

    //*************************************************************************
    //  Method: btnSetEdgeVisibility_Click()
    //
    /// <summary>
    /// Handles the Click event on all of the "set edge visibility" buttons.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnSetEdgeVisibility_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        // An EdgeVisibility value is stored in the Tag of each "set edge
        // visibility" button.

        Debug.Assert(sender is RibbonComponent);

        Debug.Assert( ( (RibbonComponent)sender ).Tag is
            EdgeWorksheetReader.Visibility );

        this.ThisWorkbook.SetVisualAttribute(VisualAttributes.EdgeVisibility,
            (EdgeWorksheetReader.Visibility)( (RibbonComponent)sender ).Tag);
    }

    //*************************************************************************
    //  Method: btnSetVertexVisibility_Click()
    //
    /// <summary>
    /// Handles the Click event on all of the "set vertex visibility" buttons.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnSetVertexVisibility_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        // An VertexVisibility value is stored in the Tag of each "set vertex
        // visibility" button.

        Debug.Assert(sender is RibbonComponent);

        Debug.Assert( ( (RibbonComponent)sender ).Tag is
            VertexWorksheetReader.Visibility );

        this.ThisWorkbook.SetVisualAttribute(VisualAttributes.VertexVisibility,
            (VertexWorksheetReader.Visibility)( (RibbonComponent)sender ).Tag);
    }

    //*************************************************************************
    //  Method: btnSetVertexShape_Click()
    //
    /// <summary>
    /// Handles the Click event on all of the "set vertex shape" buttons.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnSetVertexShape_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        // A VertexShape value is stored in the Tag of each "set vertex shape"
        // button.

        Debug.Assert(sender is RibbonComponent);
        Debug.Assert( ( (RibbonComponent)sender ).Tag is VertexShape );

        this.ThisWorkbook.SetVisualAttribute(VisualAttributes.VertexShape,
            (VertexShape)( (RibbonComponent)sender ).Tag);
    }

    //*************************************************************************
    //  Method: btnImportFromUcinetFile_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnImportFromUcinetFile button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnImportFromUcinetFile_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.ImportFromUcinetFile();
    }

    //*************************************************************************
    //  Method: btnImportFromGraphMLFile_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnImportFromGraphMLFile button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnImportFromGraphMLFile_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.ImportFromGraphMLFile();
    }

    //*************************************************************************
    //  Method: btnImportFromPajekFile_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnImportFromPajekFile button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnImportFromPajekFile_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.ImportFromPajekFile();
    }

    //*************************************************************************
    //  Method: btnRegisterUser_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnRegisterUser button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnRegisterUser_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        RegisterUserDialog oRegisterUserDialog = new RegisterUserDialog();

        oRegisterUserDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: btnCheckForUpdate_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnCheckForUpdate button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnCheckForUpdate_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        ApplicationUtil.CheckForUpdate();
    }

    //*************************************************************************
    //  Method: btnOpenHomePage_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnOpenHomePage button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnOpenHomePage_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        ApplicationUtil.OpenHomePage();
    }

    //*************************************************************************
    //  Method: btnConvertOldWorkbook_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnConvertOldWorkbook button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnConvertOldWorkbook_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.ConvertOldWorkbook();
    }

    //*************************************************************************
    //  Method: mnuShowColumnGroups_ItemsLoading()
    //
    /// <summary>
    /// Handles the ItemsLoading event on the mnuShowColumnGroups RibbonMenu.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    mnuShowColumnGroups_ItemsLoading
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        PerWorkbookSettings oPerWorkbookSettings = GetPerWorkbookSettings();

        // Note that the cbxShowVisualAttributeColumnGroups RibbonCheckBox
        // controls the visibility of two column groups:
        // ColumnGroup.EdgeVisualAttributes and
        // ColumnGroup.VertexVisualAttributes.

        cbxShowVisualAttributeColumnGroups.Checked =
            oPerWorkbookSettings.GetColumnGroupVisibility(
                ColumnGroup.EdgeVisualAttributes);

        // Note that the cbxShowLabelColumnGroups RibbonCheckBox controls the
        // visibility of two column groups: ColumnGroup.EdgeLabels and
        // ColumnGroup.VertexLabels.

        cbxShowLabelColumnGroups.Checked =
            oPerWorkbookSettings.GetColumnGroupVisibility(
                ColumnGroup.EdgeLabels);

        cbxShowVertexLayoutColumnGroup.Checked =
            oPerWorkbookSettings.GetColumnGroupVisibility(
                ColumnGroup.VertexLayout);

        cbxShowVertexGraphMetricsColumnGroup.Checked =
            oPerWorkbookSettings.GetColumnGroupVisibility(
                ColumnGroup.VertexGraphMetrics);

        // Note that the cbxShowOtherColumns RibbonCheckBox controls the
        // visibility of two column groups: ColumnGroup.EdgeOtherColumns and
        // ColumnGroup.VertexOtherColumns.

        cbxShowOtherColumns.Checked =
            oPerWorkbookSettings.GetColumnGroupVisibility(
                ColumnGroup.EdgeOtherColumns);

        // This is required to force the ItemsLoading event to fire the next
        // time the mnuShowColumnGroups menu is opened.

        this.RibbonUI.InvalidateControl(this.mnuShowColumnGroups.Name);
    }

    //*************************************************************************
    //  Method: cbxShowVisualASttributesColumnGroup_Click()
    //
    /// <summary>
    /// Handles the Click event on the cbxShowVisualASttributesColumnGroup
    /// RibbonCheckBox.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    cbxShowVisualAttributeColumnGroups_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        Boolean bShow = this.cbxShowVisualAttributeColumnGroups.Checked;
        ThisWorkbook oThisWorkbook = this.ThisWorkbook;

        oThisWorkbook.ShowColumnGroup(ColumnGroup.EdgeVisualAttributes,
            bShow, false);

        oThisWorkbook.ShowColumnGroup(ColumnGroup.VertexVisualAttributes,
            bShow, false);
    }

    //*************************************************************************
    //  Method: cbxShowLabelColumnGroups_Click()
    //
    /// <summary>
    /// Handles the Click event on the cbxShowLabelColumnGroups RibbonCheckBox.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    cbxShowLabelColumnGroups_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        Boolean bShow = this.cbxShowLabelColumnGroups.Checked;
        ThisWorkbook oThisWorkbook = this.ThisWorkbook;

        oThisWorkbook.ShowColumnGroup(ColumnGroup.EdgeLabels, bShow, false);
        oThisWorkbook.ShowColumnGroup(ColumnGroup.VertexLabels, bShow, false);
    }

    //*************************************************************************
    //  Method: cbxShowVertexLayoutColumnGroup_Click()
    //
    /// <summary>
    /// Handles the Click event on the cbxShowVertexLayoutColumnGroup
    /// RibbonCheckBox.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    cbxShowVertexLayoutColumnGroup_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.ShowColumnGroup(ColumnGroup.VertexLayout,
            this.cbxShowVertexLayoutColumnGroup.Checked, true);
    }

    //*************************************************************************
    //  Method: cbxShowVertexGraphMetricsColumnGroup_Click()
    //
    /// <summary>
    /// Handles the Click event on the cbxShowVertexGraphMetricsColumnGroup
    /// RibbonCheckBox.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    cbxShowVertexGraphMetricsColumnGroup_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.ShowColumnGroup(ColumnGroup.VertexGraphMetrics,
            this.cbxShowVertexGraphMetricsColumnGroup.Checked, true);
    }

    //*************************************************************************
    //  Method: cbxShowOtherColumns_Click()
    //
    /// <summary>
    /// Handles the Click event on the cbxShowOtherColumns RibbonCheckBox.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    cbxShowOtherColumns_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        Boolean bShow = this.cbxShowOtherColumns.Checked;

        this.ThisWorkbook.ShowColumnGroup(ColumnGroup.EdgeOtherColumns,
            bShow, false);

        this.ThisWorkbook.ShowColumnGroup(ColumnGroup.VertexOtherColumns,
            bShow, false);
    }

    //*************************************************************************
    //  Method: btnShowOrHideAllColumnGroups_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnShowAllColumnGroups and
    /// btnHideAllColumnGroups buttons.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnShowOrHideAllColumnGroups_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        // This method handles the Click event for two buttons.  The buttons
        // are distinguished by their Boolean Tags, which specify true to show
        // or false to hide.

        Debug.Assert(sender is RibbonButton);
        RibbonButton oButton = (RibbonButton)sender;
        Debug.Assert(oButton.Tag is Boolean);

        this.ThisWorkbook.ShowOrHideAllColumnGroups( (Boolean)oButton.Tag );
    }

    //*************************************************************************
    //  Method: cbxShowGraphLegend_Click()
    //
    /// <summary>
    /// Handles the Click event on the cbxShowGraphLegend CheckBox.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    cbxShowGraphLegend_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        FireRibbonControlsChangedEvent(RibbonControls.ShowGraphLegend);
    }

    //*************************************************************************
    //  Method: cbxShowGraphAxes_Click()
    //
    /// <summary>
    /// Handles the Click event on the cbxShowGraphAxes CheckBox.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    cbxShowGraphAxes_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        FireRibbonControlsChangedEvent(RibbonControls.ShowGraphAxes);
    }

    //*************************************************************************
    //  Method: btnShowOrHideAllGraphElements_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnShowAllGraphElements and
    /// btnHideAllGraphElements buttons.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnShowOrHideAllGraphElements_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        // This method handles the Click event for two buttons.  The buttons
        // are distinguished by their Boolean Tags, which specify true to show
        // or false to hide.

        Debug.Assert(sender is RibbonButton);
        RibbonButton oButton = (RibbonButton)sender;
        Debug.Assert(oButton.Tag is Boolean);

        Boolean bShow = (Boolean)oButton.Tag;

        cbxReadClusters.Checked = cbxShowGraphLegend.Checked
            = cbxShowGraphAxes.Checked = bShow;

        FireRibbonControlsChangedEvent(RibbonControls.ShowGraphLegend |
            RibbonControls.ShowGraphAxes);
    }

    //*************************************************************************
    //  Method: btnAbout_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnAbout button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnAbout_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        AboutDialog oAboutDialog = new AboutDialog();

        oAboutDialog.ShowDialog();
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        Debug.Assert(m_oLayoutManagerForRibbonDropDown != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Helper objects for managing layouts.

    protected LayoutManagerForRibbonDropDown m_oLayoutManagerForRibbonDropDown;
}


//*****************************************************************************
//  Enum: RibbonControls
//
/// <summary>
/// Specifies one or more controls in the NodeXL ribbon.
/// </summary>
//*****************************************************************************

[System.FlagsAttribute]

public enum
RibbonControls
{
    /// <summary>
    /// The layout RibbonDropDown control.  The selected layout can be obtained
    /// from the <see cref="Ribbon.Layout" /> property.
    /// </summary>

    Layout = 1,

    /// <summary>
    /// The "show graph legend" CheckBox.  The "show graph legend" flag can be
    /// obtained from the <see cref="Ribbon.ShowGraphLegend" /> property.
    /// </summary>

    ShowGraphLegend = 2,

    /// <summary>
    /// The "show graph axes" CheckBox.  The "show graph axes" flag can be
    /// obtained from the <see cref="Ribbon.ShowGraphAxes" /> property.
    /// </summary>

    ShowGraphAxes = 4,
}


//*****************************************************************************
//  Enum: RibbonCommand
//
/// <summary>
/// Specifies a command run from the NodeXL ribbon.
/// </summary>
//*****************************************************************************

public enum
RibbonCommand
{
    /// <summary>
    /// Show the dynamic filters dialog.
    /// </summary>

    ShowDynamicFilters,

    /// <summary>
    /// The workbook should be read into the graph pane.
    /// </summary>

    ReadWorkbook,
}
}
