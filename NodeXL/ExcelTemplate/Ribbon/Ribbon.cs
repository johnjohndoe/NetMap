

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Algorithms;
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
/// Represents the template's ribbon tab.
/// </summary>
///
/// <remarks>
/// How the Ribbon is Connected to the Rest of the Application
///
/// <para>
/// The ribbon has direct access to the public methods and properties on the
/// ThisWorkbook object.  It does not have direct access to the TaskPane.
/// </para>
///
/// <para>
/// When the user clicks a ribbon control that is best handled by ThisWorkbook,
/// the ribbon calls the appropriate public method on ThisWorkbook, passing the
/// method whatever arguments are required to perform the requested task.
/// </para>
///
/// <para>
/// When the user clicks a ribbon control that is best handled by the TaskPane,
/// the ribbon fires an event that gets handled by the TaskPane.  The
/// arguments required to perform the requested task get passed to the TaskPane
/// via event arguments.
/// </para>
///
/// </remarks>
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

        // Populate the rddLayout RibbonDropDown.

        m_oLayoutManagerForRibbonDropDown =
            new LayoutManagerForRibbonDropDown();

        m_oLayoutManagerForRibbonDropDown.AddRibbonDropDownItems(
            this.rddLayout);

        m_oLayoutManagerForRibbonDropDown.LayoutChanged += new
            EventHandler(this.m_oLayoutManagerForRibbonDropDown_LayoutChanged);

        // Read the general user settings directly controlled by the ribbon.

        GeneralUserSettings oGeneralUserSettings = new GeneralUserSettings();

        this.ClusterAlgorithm = oGeneralUserSettings.ClusterAlgorithm;
        this.ReadGroups = oGeneralUserSettings.ReadGroups;
        this.ReadVertexLabels = oGeneralUserSettings.ReadVertexLabels;
        this.ReadEdgeLabels = oGeneralUserSettings.ReadEdgeLabels;
        this.ShowGraphLegend = oGeneralUserSettings.ShowGraphLegend;
        this.ShowGraphAxes = oGeneralUserSettings.ShowGraphAxes;

        this.ClearTablesBeforeImport =
            oGeneralUserSettings.ClearTablesBeforeImport;

        AssertValid();
    }

    //*************************************************************************
    //  Property: ReadGroups
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the group worksheets should be
    /// read when the workbook is read into the graph.
    /// </summary>
    ///
    /// <value>
    /// true to read the group worksheets.
    /// </value>
    //*************************************************************************

    public Boolean
    ReadGroups
    {
        get
        {
            AssertValid();

            return (chkReadGroups.Checked);
        }

        set
        {
            chkReadGroups.Checked = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ReadVertexLabels
    //
    /// <summary>
    /// Gets or sets a flag indicating whether vertex labels should be read
    /// from the vertex worksheet.
    /// </summary>
    ///
    /// <value>
    /// true to read vertex labels.
    /// </value>
    //*************************************************************************

    public Boolean
    ReadVertexLabels
    {
        get
        {
            AssertValid();

            return (chkReadVertexLabels.Checked);
        }

        set
        {
            chkReadVertexLabels.Checked = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ReadEdgeLabels
    //
    /// <summary>
    /// Gets or sets a flag indicating whether edge labels should be read from
    /// the edge worksheet.
    /// </summary>
    ///
    /// <value>
    /// true to read edge labels.
    /// </value>
    //*************************************************************************

    public Boolean
    ReadEdgeLabels
    {
        get
        {
            AssertValid();

            return (chkReadEdgeLabels.Checked);
        }

        set
        {
            chkReadEdgeLabels.Checked = value;

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

            return (chkShowGraphLegend.Checked);
        }

        set
        {
            chkShowGraphLegend.Checked = value;

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

            return (chkShowGraphAxes.Checked);
        }

        set
        {
            chkShowGraphAxes.Checked = value;

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

            return (chkClearTablesBeforeImport.Checked);
        }

        set
        {
            chkClearTablesBeforeImport.Checked = value;

            AssertValid();
        }
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
    //  Method: AutomateTasksOnOpen()
    //
    /// <summary>
    /// Runs task automation on the workbook immediately after it is opened.
    /// </summary>
    //*************************************************************************

    private void
    AutomateTasksOnOpen()
    {
        AssertValid();

        // TaskAutomator.AutomateFolder() is automating this workbook.

        this.ThisWorkbook.AutomateTasksOnOpen();
    }

    //*************************************************************************
    //  Method: OnMergeDuplicateEdgesClick()
    //
    /// <summary>
    /// Handles the Click event on the btnMergeDuplicateEdges button.
    /// </summary>
    ///
    /// <param name="requestMergeApproval">
    /// true to request approval from the user to merge duplicate edges.
    /// </param>
    ///
    /// <returns>
    /// true if the duplicate edges were successfully merged.
    /// </returns>
    ///
    /// <remarks>
    /// This method can be called from outside the class to simulate a button
    /// click.
    /// </remarks>
    //*************************************************************************

    public Boolean
    OnMergeDuplicateEdgesClick
    (
        Boolean requestMergeApproval
    )
    {
        AssertValid();

        return ( this.ThisWorkbook.MergeDuplicateEdges(requestMergeApproval) );
    }

    //*************************************************************************
    //  Method: OnReadWorkbookClick()
    //
    /// <summary>
    /// Handles the Click event on the btnReadWorkbook button.
    /// </summary>
    ///
    /// <remarks>
    /// This method can be called from outside the class to simulate a button
    /// click.
    /// </remarks>
    //*************************************************************************

    public void
    OnReadWorkbookClick()
    {
        AssertValid();

        // Make sure the graph is showing, then tell the TaskPane to read the
        // workbook.

        if ( this.ThisWorkbook.ShowGraph() )
        {
            FireRunRibbonCommandEvent(RibbonCommand.ReadWorkbook);
        }
    }

    //*************************************************************************
    //  Method: OnCalculateClustersClick()
    //
    /// <summary>
    /// Handles the Click event on the btnCalculateClusters button.
    /// </summary>
    ///
    /// <returns>
    /// true if the graph was successfully partitioned into clusters.
    /// </returns>
    ///
    /// <remarks>
    /// This method can be called from outside the class to simulate a button
    /// click.
    /// </remarks>
    //*************************************************************************

    public Boolean
    OnCalculateClustersClick()
    {
        AssertValid();

        return ( this.ThisWorkbook.CalculateClusters(this.ClusterAlgorithm) );
    }

    //*************************************************************************
    //  Method: OnCreateSubgraphImagesClick()
    //
    /// <summary>
    /// Handles the Click event on the btnCreateSubgraphImages button.
    /// </summary>
    ///
    /// <param name="mode">
    /// Indicates the mode in which the CreateSubgraphImagesDialog is being
    /// used.
    /// </param>
    ///
    /// <remarks>
    /// This method can be called from outside the class to simulate a button
    /// click.
    /// </remarks>
    //*************************************************************************

    public void
    OnCreateSubgraphImagesClick
    (
        CreateSubgraphImagesDialog.DialogMode mode
    )
    {
        AssertValid();

        this.ThisWorkbook.CreateSubgraphImages(mode);
    }

    //*************************************************************************
    //  Method: OnShowGraphMetricsClick()
    //
    /// <summary>
    /// Handles the Click event on the btnShowGraphMetrics button.
    /// </summary>
    ///
    /// <param name="mode">
    /// Indicates the mode in which the GraphMetricsDialog is being used.
    /// </param>
    ///
    /// <remarks>
    /// This method can be called from outside the class to simulate a button
    /// click.
    /// </remarks>
    //*************************************************************************

    public void
    OnShowGraphMetricsClick
    (
        GraphMetricsDialog.DialogMode mode
    )
    {
        AssertValid();

        this.ThisWorkbook.ShowGraphMetrics(mode);
    }

    //*************************************************************************
    //  Method: OnAutoFillWorkbookClick()
    //
    /// <summary>
    /// Handles the Click event on the btnAutoFillWorkbook button.
    /// </summary>
    ///
    /// <param name="mode">
    /// Indicates the mode in which the AutoFillWorkbookDialog is being used.
    /// </param>
    ///
    /// <remarks>
    /// This method can be called from outside the class to simulate a button
    /// click.
    /// </remarks>
    //*************************************************************************

    public void
    OnAutoFillWorkbookClick
    (
        AutoFillWorkbookDialog.DialogMode mode
    )
    {
        AssertValid();

        this.ThisWorkbook.AutoFillWorkbook(mode);
    }

    //*************************************************************************
    //  Method: OnWorkbookAutoFilled()
    //
    /// <summary>
    /// Performs tasks required after the workbook is autofilled.
    /// </summary>
    ///
    /// <param name="readWorkbook">
    /// true to read the workbook.
    /// </param>
    //*************************************************************************

    public void
    OnWorkbookAutoFilled
    (
        Boolean readWorkbook
    )
    {
        AssertValid();

        if (GetPerWorkbookSettings().AutoFillWorkbookResults.VertexXResults
            .ColumnAutoFilled )
        {
            // When the X and Y columns are autofilled, the graph shouldn't be
            // laid out again.

            this.Layout = LayoutType.Null;
        }

        if (readWorkbook)
        {
            OnReadWorkbookClick();
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
    //  Property: ClusterAlgorithm
    //
    /// <summary>
    /// Gets or sets the algorithm used to to partition the graph into
    /// clusters.
    /// </summary>
    ///
    /// <value>
    /// The algorithm used to to partition the graph into clusters.
    /// </value>
    //*************************************************************************

    protected ClusterAlgorithm
    ClusterAlgorithm
    {
        get
        {
            AssertValid();

            // The cluster algorithm is selected with a set of RibbonCheckBox
            // controls that are children of mnuClusterOptions.

            foreach (RibbonControl oControl in mnuClusterOptions.Items)
            {
                if (oControl is RibbonCheckBox)
                {
                    RibbonCheckBox oRibbonCheckBox = (RibbonCheckBox)oControl;

                    if (oRibbonCheckBox.Checked &&
                        oRibbonCheckBox.Tag is ClusterAlgorithm)
                    {
                        return ( (ClusterAlgorithm)oRibbonCheckBox.Tag );
                    }
                }
            }

            Debug.Assert(false);
            return (ClusterAlgorithm.GirvanNewman);
        }

        set
        {
            foreach (RibbonControl oControl in mnuClusterOptions.Items)
            {
                if (oControl is RibbonCheckBox)
                {
                    RibbonCheckBox oRibbonCheckBox = (RibbonCheckBox)oControl;

                    if (oRibbonCheckBox.Tag is ClusterAlgorithm)
                    {
                        oRibbonCheckBox.Checked =
                            ( ( (ClusterAlgorithm)oRibbonCheckBox.Tag ) ==
                                value );
                    }
                }
            }

            AssertValid();
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

        PerWorkbookSettings oPerWorkbookSettings = GetPerWorkbookSettings();

        Int32 iTemplateVersion = oPerWorkbookSettings.TemplateVersion;

        rddGraphDirectedness.Enabled = (iTemplateVersion >= 51);

        // The ability to create groups (which used to be called clusters)
        // was introduced in version 54.

        mnuGroups.Enabled = (iTemplateVersion >= 54);

        // Should the layout automatically be set and the workbook read?  (This
        // feature was added in January 2010 for the Microsoft Biology
        // Foundation project, which "drives" the NodeXL template
        // programatically and needs to be able to set the layout and read the
        // workbook by writing to workbook cells.  It may be useful for other
        // purposes as well.)

        Nullable<LayoutType> oAutoLayoutOnOpen =
            oPerWorkbookSettings.AutoLayoutOnOpen;

        if (oAutoLayoutOnOpen.HasValue)
        {
            // Yes.

            this.Layout = oAutoLayoutOnOpen.Value;

            // Unfortunately, there is no way to programatically click a
            // RibbonButton.  Simulate a click.

            OnReadWorkbookClick();
        }

        // Should task automation be run on the workbook when it is opened?

        if (oPerWorkbookSettings.AutomateTasksOnOpen)
        {
            // Yes.

            AutomateTasksOnOpen();
        }
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

        oGeneralUserSettings.ClusterAlgorithm = this.ClusterAlgorithm;
        oGeneralUserSettings.ReadGroups = this.ReadGroups;
        oGeneralUserSettings.ReadVertexLabels = this.ReadVertexLabels;
        oGeneralUserSettings.ReadEdgeLabels = this.ReadEdgeLabels;
        oGeneralUserSettings.ShowGraphLegend = this.ShowGraphLegend;
        oGeneralUserSettings.ShowGraphAxes = this.ShowGraphAxes;

        oGeneralUserSettings.ClearTablesBeforeImport =
            this.ClearTablesBeforeImport;

        oGeneralUserSettings.Save();
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
    //  Method: btnExportToGraphMLFile_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnExportToGraphMLFile button.
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
    btnExportToGraphMLFile_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.ExportToGraphMLFile();
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
    //  Method: rddLayout_ButtonClick()
    //
    /// <summary>
    /// Handles the ButtonClick event on the rddLayout RibbonDropDown.
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
    rddLayout_ButtonClick
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        if (sender == this.btnEditLayoutUserSettings)
        {
            FireRunRibbonCommandEvent(RibbonCommand.EditLayoutUserSettings);
        }
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

        OnReadWorkbookClick();
    }

    //*************************************************************************
    //  Method: btnAutomateTasks_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnAutomateTasks button.
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
    btnAutomateTasks_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.AutomateTasks();
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

        OnMergeDuplicateEdgesClick(true);
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

        OnShowGraphMetricsClick(GraphMetricsDialog.DialogMode.Normal);
    }

    //*************************************************************************
    //  Method: btnGroupByVertexAttribute_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnGroupByVertexAttribute button.
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
    btnGroupByVertexAttribute_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.GroupByVertexAttribute();
    }

    //*************************************************************************
    //  Method: btnCalculateClusters_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnCalculateClusters button.
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
    btnCalculateClusters_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        OnCalculateClustersClick();
    }

    //*************************************************************************
    //  Method: btnCalculateConnectedComponents_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnCalculateConnectedComponents button.
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
    btnCalculateConnectedComponents_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.CalculateConnectedComponents();
    }

    //*************************************************************************
    //  Method: chkClusterAlgorithm_Click()
    //
    /// <summary>
    /// Handles the Click event on the RibbonCheckBox controls that select the
    /// cluster algorithm to use.
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
    chkClusterAlgorithm_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        // The ClusterAlgorithm value is stored in the RibbonCheckBox's Tag.

        Debug.Assert(sender is RibbonCheckBox);
        RibbonCheckBox oRibbonCheckBox = (RibbonCheckBox)sender;

        Debug.Assert(oRibbonCheckBox.Tag is ClusterAlgorithm);
        this.ClusterAlgorithm = (ClusterAlgorithm)oRibbonCheckBox.Tag;
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

        OnCreateSubgraphImagesClick(
            CreateSubgraphImagesDialog.DialogMode.Normal);
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

        OnAutoFillWorkbookClick(AutoFillWorkbookDialog.DialogMode.Normal);
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
    //  Method: btnConvertNodeXLWorkbook_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnConvertNodeXLWorkbook button.
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
    btnConvertNodeXLWorkbook_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.ConvertNodeXLWorkbook();
    }

    //*************************************************************************
    //  Method: mnuGroups_ItemsLoading()
    //
    /// <summary>
    /// Handles the ItemsLoading event on the mnuGroups RibbonMenu.
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
    mnuGroups_ItemsLoading
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        GroupCommands [] aeGroupCommands = {
            GroupCommands.CollapseSelectedGroups,
            GroupCommands.ExpandSelectedGroups,
            GroupCommands.CollapseAllGroups,
            GroupCommands.ExpandAllGroups,
            GroupCommands.SelectGroupsWithSelectedVertices,
            GroupCommands.SelectAllGroups,
            GroupCommands.AddSelectedVerticesToGroup,
            GroupCommands.RemoveSelectedVerticesFromGroups,
            GroupCommands.RemoveSelectedGroups,
            GroupCommands.RemoveAllGroups,
            };

        RibbonButton [] aoCorrespondingRibbonButtons = {
            btnCollapseSelectedGroups,
            btnExpandSelectedGroups,
            btnCollapseAllGroups,
            btnExpandAllGroups,
            btnSelectGroupsWithSelectedVertices,
            btnSelectAllGroups,
            btnAddSelectedVerticesToGroup,
            btnRemoveSelectedVerticesFromGroups,
            btnRemoveSelectedGroups,
            btnRemoveAllGroups,
            };

        Debug.Assert(aeGroupCommands.Length ==
            aoCorrespondingRibbonButtons.Length);

        // Various worksheets must be activated to read their selection.  Save
        // the active worksheet state.

        Microsoft.Office.Interop.Excel.Workbook oWorkbook =
            this.ThisWorkbook.InnerObject;

        ExcelActiveWorksheetRestorer oExcelActiveWorksheetRestorer =
            new ExcelActiveWorksheetRestorer(oWorkbook);

        ExcelActiveWorksheetState oExcelActiveWorksheetState =
            oExcelActiveWorksheetRestorer.GetActiveWorksheetState();

        GroupCommands eGroupCommandsToEnable;

        try
        {
            eGroupCommandsToEnable =
                GroupManager.GetGroupCommandsToEnable(oWorkbook);
        }
        finally
        {
            oExcelActiveWorksheetRestorer.Restore(oExcelActiveWorksheetState);
        }

        for (Int32 i = 0; i < aeGroupCommands.Length; i++)
        {
            aoCorrespondingRibbonButtons[i].Enabled =
                ( (eGroupCommandsToEnable & aeGroupCommands[i] ) != 0);
        }

        // This is required to force the ItemsLoading event to fire the next
        // time the mnuGroups menu is opened.

        this.RibbonUI.InvalidateControl(this.mnuGroups.Name);
    }

    //*************************************************************************
    //  Method: GroupCommandButton_Click()
    //
    /// <summary>
    /// Handles the Click event on the buttons corresponding to the
    /// GroupCommands enumeration values.
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
    GroupCommandButton_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        // The buttons corresponding to the GroupCommands enumeration values
        // have the enumeration value stored in their Tag.

        Debug.Assert(sender is RibbonButton);
        Debug.Assert( ( (RibbonButton)sender ).Tag is GroupCommands );

        this.ThisWorkbook.RunGroupCommand(
            (GroupCommands)( (RibbonButton)sender ).Tag );
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

        // Note that the chkShowVisualAttributeColumnGroups RibbonCheckBox
        // controls the visibility of three column groups:
        // ColumnGroup.EdgeVisualAttributes,
        // ColumnGroup.VertexVisualAttributes, and
        // ColumnGroup.GroupVisualAttributes.

        chkShowVisualAttributeColumnGroups.Checked =
            oPerWorkbookSettings.GetColumnGroupVisibility(
                ColumnGroup.EdgeVisualAttributes);

        // Note that the chkShowLabelColumnGroups RibbonCheckBox controls the
        // visibility of two column groups: ColumnGroup.EdgeLabels and
        // ColumnGroup.VertexLabels.

        chkShowLabelColumnGroups.Checked =
            oPerWorkbookSettings.GetColumnGroupVisibility(
                ColumnGroup.EdgeLabels);

        chkShowVertexLayoutColumnGroup.Checked =
            oPerWorkbookSettings.GetColumnGroupVisibility(
                ColumnGroup.VertexLayout);

        // Note that the chkShowGraphMetricColumnGroups RibbonCheckBox
        // controls the visibility of two column groups:
        // ColumnGroup.VertexGraphMetrics and ColumnGroup.GroupGraphMetrics.

        chkShowGraphMetricColumnGroups.Checked =
            oPerWorkbookSettings.GetColumnGroupVisibility(
                ColumnGroup.VertexGraphMetrics);

        // Note that the chkShowOtherColumns RibbonCheckBox controls the
        // visibility of three column groups: ColumnGroup.EdgeOtherColumns,
        // ColumnGroup.VertexOtherColumns, and ColumnGroup.GroupOtherColumns.

        chkShowOtherColumns.Checked =
            oPerWorkbookSettings.GetColumnGroupVisibility(
                ColumnGroup.EdgeOtherColumns);

        // This is required to force the ItemsLoading event to fire the next
        // time the mnuShowColumnGroups menu is opened.

        this.RibbonUI.InvalidateControl(this.mnuShowColumnGroups.Name);
    }

    //*************************************************************************
    //  Method: chkShowVisualAttributesColumnGroup_Click()
    //
    /// <summary>
    /// Handles the Click event on the chkShowVisualASttributesColumnGroup
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
    chkShowVisualAttributeColumnGroups_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        Boolean bShow = this.chkShowVisualAttributeColumnGroups.Checked;
        ThisWorkbook oThisWorkbook = this.ThisWorkbook;

        oThisWorkbook.ShowColumnGroup(ColumnGroup.EdgeVisualAttributes,
            bShow, false);

        oThisWorkbook.ShowColumnGroup(ColumnGroup.VertexVisualAttributes,
            bShow, false);

        oThisWorkbook.ShowColumnGroup(ColumnGroup.GroupVisualAttributes,
            bShow, false);
    }

    //*************************************************************************
    //  Method: chkShowLabelColumnGroups_Click()
    //
    /// <summary>
    /// Handles the Click event on the chkShowLabelColumnGroups RibbonCheckBox.
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
    chkShowLabelColumnGroups_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        Boolean bShow = this.chkShowLabelColumnGroups.Checked;
        ThisWorkbook oThisWorkbook = this.ThisWorkbook;

        oThisWorkbook.ShowColumnGroup(ColumnGroup.EdgeLabels, bShow, false);
        oThisWorkbook.ShowColumnGroup(ColumnGroup.VertexLabels, bShow, false);
    }

    //*************************************************************************
    //  Method: chkShowVertexLayoutColumnGroup_Click()
    //
    /// <summary>
    /// Handles the Click event on the chkShowVertexLayoutColumnGroup
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
    chkShowVertexLayoutColumnGroup_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.ShowColumnGroup(ColumnGroup.VertexLayout,
            this.chkShowVertexLayoutColumnGroup.Checked, true);
    }

    //*************************************************************************
    //  Method: chkShowGraphMetricColumnGroups_Click()
    //
    /// <summary>
    /// Handles the Click event on the chkShowGraphMetricColumnGroups
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
    chkShowGraphMetricColumnGroups_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        Boolean bShow = this.chkShowGraphMetricColumnGroups.Checked;
        ThisWorkbook oThisWorkbook = this.ThisWorkbook;

        this.ThisWorkbook.ShowColumnGroup(ColumnGroup.VertexGraphMetrics,
            bShow, false);

        this.ThisWorkbook.ShowColumnGroup(ColumnGroup.GroupGraphMetrics,
            bShow, false);
    }

    //*************************************************************************
    //  Method: chkShowOtherColumns_Click()
    //
    /// <summary>
    /// Handles the Click event on the chkShowOtherColumns RibbonCheckBox.
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
    chkShowOtherColumns_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        Boolean bShow = this.chkShowOtherColumns.Checked;

        this.ThisWorkbook.ShowColumnGroup(ColumnGroup.EdgeOtherColumns,
            bShow, false);

        this.ThisWorkbook.ShowColumnGroup(ColumnGroup.VertexOtherColumns,
            bShow, false);

        this.ThisWorkbook.ShowColumnGroup(ColumnGroup.GroupOtherColumns,
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
    //  Method: chkShowGraphLegend_Click()
    //
    /// <summary>
    /// Handles the Click event on the chkShowGraphLegend CheckBox.
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
    chkShowGraphLegend_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        FireRibbonControlsChangedEvent(RibbonControls.ShowGraphLegend);
    }

    //*************************************************************************
    //  Method: chkShowGraphAxes_Click()
    //
    /// <summary>
    /// Handles the Click event on the chkShowGraphAxes CheckBox.
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
    chkShowGraphAxes_Click
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

        chkReadGroups.Checked = chkReadVertexLabels.Checked =
            chkReadEdgeLabels.Checked = chkShowGraphLegend.Checked =
            chkShowGraphAxes.Checked = bShow;

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

    /// <summary>
    /// The user wants to edit his layout settings.
    /// </summary>

    EditLayoutUserSettings,
}
}
