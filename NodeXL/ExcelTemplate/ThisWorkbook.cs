
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using System.Reflection;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization.Wpf;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
public partial class ThisWorkbook
{
    //*************************************************************************
    //  Property: WorksheetContextMenuManager
    //
    /// <summary>
    /// Gets the object that adds custom menu items to the Excel context menus
    /// that appear when the vertex or edge table is right-clicked.
    /// </summary>
    ///
    /// <value>
    /// A WorksheetContextMenuManager object.
    /// </value>
    //*************************************************************************

    public WorksheetContextMenuManager
    WorksheetContextMenuManager
    {
        get
        {
            AssertValid();

            return (m_oWorksheetContextMenuManager);
        }
    }

    //*************************************************************************
    //  Property: GraphDirectedness
    //
    /// <summary>
    /// Gets or sets the graph directedness of the workbook.
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

            // Retrive the directedness from the per-workbook settings.

            return (this.PerWorkbookSettings.GraphDirectedness);
        }

        set
        {
            // Store the directedness in the per-workbook settings.

            this.PerWorkbookSettings.GraphDirectedness = value;

            // Update the user settings.

            GeneralUserSettings oGeneralUserSettings =
                new GeneralUserSettings();

            oGeneralUserSettings.NewWorkbookGraphDirectedness = value;
            oGeneralUserSettings.Save();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: ExcelApplicationIsReady
    //
    /// <summary>
    /// Determines whether the Excel application is ready to accept method
    /// calls.
    /// </summary>
    ///
    /// <param name="showBusyMessage">
    /// true if a busy message should be displayed if the application is not
    /// ready.
    /// </param>
    ///
    /// <returns>
    /// true if the Excel application is ready to accept method calls.
    /// </returns>
    //*************************************************************************

    public Boolean
    ExcelApplicationIsReady
    (
        Boolean showBusyMessage
    )
    {
        AssertValid();

        if ( !ExcelUtil.ExcelApplicationIsReady(this.Application) )
        {
            if (showBusyMessage)
            {
                FormUtil.ShowWarning(
                    "This feature isn't available while a worksheet cell is"
                    + " being edited.  Press Enter to finish editing the cell,"
                    + " then try again."
                    );
            }

            return (false);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: ImportFromMatrixWorkbook()
    //
    /// <summary>
    /// Imports edges from another open workbook that contains a graph
    /// represented as an adjacency matrix.
    /// </summary>
    //*************************************************************************

    public void
    ImportFromMatrixWorkbook()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        // The ImportFromMatrixWorkbookDialog does all the work.

        ImportFromMatrixWorkbookDialog oImportFromMatrixWorkbookDialog =
            new ImportFromMatrixWorkbookDialog(this.InnerObject);

        if (oImportFromMatrixWorkbookDialog.ShowDialog() == DialogResult.OK)
        {
            GraphDirectedness eGraphDirectedness =
                oImportFromMatrixWorkbookDialog.SourceWorkbookDirectedness;

            this.GraphDirectedness = eGraphDirectedness;

            // Pass the workbook's directedness to the Ribbon.

            this.Ribbon.GraphDirectedness = eGraphDirectedness;
        }
    }

    //*************************************************************************
    //  Method: ImportFromEdgeWorkbook()
    //
    /// <summary>
    /// Imports two edge columns from another open workbook.
    /// </summary>
    //*************************************************************************

    public void
    ImportFromEdgeWorkbook()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        // The ImportFromEdgeWorkbookDialog does all the work.

        ImportFromEdgeWorkbookDialog oImportFromEdgeWorkbookDialog =
            new ImportFromEdgeWorkbookDialog(this.InnerObject);

        oImportFromEdgeWorkbookDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: ExportSelectionToNewNodeXLWorkbook()
    //
    /// <summary>
    /// Exports the selected rows of the edge and vertex tables to a new NodeXL
    /// workbook.
    /// </summary>
    //*************************************************************************

    public void
    ExportSelectionToNewNodeXLWorkbook()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        // Exporting the workbook changes the active worksheet several times.
        // Save the current active worksheet so it can be restored later.

        Object oOldActiveSheet = this.Application.ActiveSheet;

        this.ScreenUpdating = false;

        try
        {
            WorkbookExporter oWorkbookExporter =
                new WorkbookExporter(this.InnerObject);

            Workbook oNewWorkbook =
                oWorkbookExporter.ExportSelectionToNewNodeXLWorkbook();

            // Reactivate the original active worksheet.

            if (oOldActiveSheet is Worksheet)
            {
                ExcelUtil.ActivateWorksheet( (Worksheet)oOldActiveSheet );
            }

            // Activate the edge worksheet in the new workbook.

            // Note: When run in the debugger, activating the new workbook
            // causes a "System.Runtime.InteropServices.ExternalException
            // crossed a native/managed boundary" error.  There is no inner
            // exception.  This does not occur outside the debugger.  Does this
            // have something to do with Visual Studio security contexts?`

            ExcelUtil.ActivateWorkbook(oNewWorkbook);

            Worksheet oNewEdgeWorksheet;

            if ( ExcelUtil.TryGetWorksheet(oNewWorkbook, WorksheetNames.Edges,
                out oNewEdgeWorksheet) )
            {
                ExcelUtil.ActivateWorksheet(oNewEdgeWorksheet);
            }

            this.ScreenUpdating = true;
        }
        catch (ExportWorkbookException oExportWorkbookException)
        {
            this.ScreenUpdating = true;

            FormUtil.ShowWarning(oExportWorkbookException.Message);
        }
        catch (Exception oException)
        {
            this.ScreenUpdating = true;

            ErrorUtil.OnException(oException);
        }
    }

    //*************************************************************************
    //  Method: ExportToNewMatrixWorkbook()
    //
    /// <summary>
    /// Exports the edge table to a new workbook as an adjacency matrix.
    /// </summary>
    //*************************************************************************

    public void
    ExportToNewMatrixWorkbook()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        const String Message =
            "This will remove any filters that are applied to the Edges"
            + " worksheet, merge any duplicate edges, add an Edge Weight"
            + " column, and export the edges to a new workbook as an adjacency"
            + " matrix."
            + "\r\n\r\n"
            + "Do you want to continue?"
            ;

        if (MessageBox.Show(Message, FormUtil.ApplicationName,
                MessageBoxButtons.YesNo, MessageBoxIcon.Information) !=
                DialogResult.Yes)
        {
            return;
        }

        this.Application.Cursor =
            Microsoft.Office.Interop.Excel.XlMousePointer.xlWait;

        this.ScreenUpdating = false;

        try
        {
            WorkbookExporter oWorkbookExporter =
                new WorkbookExporter(this.InnerObject);

            oWorkbookExporter.ExportToNewMatrixWorkbook();

            this.ScreenUpdating = true;
        }
        catch (ExportWorkbookException oExportWorkbookException)
        {
            this.ScreenUpdating = true;

            FormUtil.ShowWarning(oExportWorkbookException.Message);
        }
        catch (Exception oException)
        {
            this.ScreenUpdating = true;

            ErrorUtil.OnException(oException);
        }

        this.Application.Cursor =
            Microsoft.Office.Interop.Excel.XlMousePointer.xlDefault;
    }

    //*************************************************************************
    //  Method: ShowGraph()
    //
    /// <summary>
    /// Shows the NodeXL graph.
    /// </summary>
    //*************************************************************************

    public void
    ShowGraph()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        this.GraphVisibility = true;
    }

    //*************************************************************************
    //  Method: ConvertOldWorkbook()
    //
    /// <summary>
    /// Converts an old workbook created with an earlier version of the program
    /// to work with the current version.
    /// </summary>
    //*************************************************************************

    public void
    ConvertOldWorkbook()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        ConvertOldWorkbookDialog oConvertOldWorkbookDialog =
            new ConvertOldWorkbookDialog(this.Application);

        oConvertOldWorkbookDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: MergeDuplicateEdges()
    //
    /// <summary>
    /// Merges duplicate edges in the edge worksheet.
    /// </summary>
    //*************************************************************************

    public void
    MergeDuplicateEdges()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        const String Message =
            "This will remove any filters that are applied to the Edges"
            + " worksheet, merge any duplicate edges, and add an Edge Weight"
            + " column."
            + "\r\n\r\n"
            + "Do you want to continue?"
            ;

        if (MessageBox.Show(Message, FormUtil.ApplicationName,
                MessageBoxButtons.YesNo, MessageBoxIcon.Information) !=
                DialogResult.Yes)
        {
            return;
        }

        // Create and use the object that merges duplicate edges.

        DuplicateEdgeMerger oDuplicateEdgeMerger = new DuplicateEdgeMerger();

        this.Application.Cursor =
            Microsoft.Office.Interop.Excel.XlMousePointer.xlWait;

        this.ScreenUpdating = false;

        try
        {
            oDuplicateEdgeMerger.MergeDuplicateEdges(this.InnerObject);

            this.ScreenUpdating = true;
        }
        catch (Exception oException)
        {
            // Don't let Excel handle unhandled exceptions.

            this.ScreenUpdating = true;

            ErrorUtil.OnException(oException);
        }

        this.Application.Cursor =
            Microsoft.Office.Interop.Excel.XlMousePointer.xlDefault;
    }

    //*************************************************************************
    //  Method: PopulateVertexWorksheet()
    //
    /// <summary>
    /// Populates the vertex worksheet with the name of each unique vertex in
    /// the edge worksheet.
    /// </summary>
    ///
    /// <param name="activateVertexWorksheetWhenDone">
    /// true to activate the vertex worksheet after it is populated.
    /// </param>
    ///
    /// <param name="notifyUserOnError">
    /// If true, the user is notified when an error occurs.  If false, an
    /// exception is thrown when an error occurs.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    public Boolean
    PopulateVertexWorksheet
    (
        Boolean activateVertexWorksheetWhenDone,
        Boolean notifyUserOnError
    )
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return (false);
        }

        // Create and use the object that fills in the vertex worksheet.

        VertexWorksheetPopulator oVertexWorksheetPopulator =
            new VertexWorksheetPopulator();

        this.ScreenUpdating = false;

        try
        {
            oVertexWorksheetPopulator.PopulateVertexWorksheet(
                this.InnerObject, activateVertexWorksheetWhenDone);

            this.ScreenUpdating = true;

            return (true);
        }
        catch (Exception oException)
        {
            // Don't let Excel handle unhandled exceptions.

            this.ScreenUpdating = true;

            if (notifyUserOnError)
            {
                ErrorUtil.OnException(oException);

                return (false);
            }
            else
            {
                throw oException;
            }
        }
    }

    //*************************************************************************
    //  Method: CreateSubgraphImages()
    //
    /// <summary>
    /// Creates a subgraph of each of the graph's vertices and saves the images
    /// to disk or the workbook.
    /// </summary>
    //*************************************************************************

    public void
    CreateSubgraphImages()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        // Populate the vertex worksheet.  This is necessary in case the user
        // opts to insert images into the vertex worksheet.  Note that
        // PopulateVertexWorksheet() returns false if the vertex worksheet or
        // table is missing, and that it activates the vertex worksheet.

        if ( !PopulateVertexWorksheet(true, true) )
        {
            return;
        }

        // Get an array of vertex names that are selected in the vertex
        // worksheet.

        String [] asSelectedVertexNames = new String[0];

        Debug.Assert(this.Application.ActiveSheet is Worksheet);

        Debug.Assert( ( (Worksheet)this.Application.ActiveSheet ).Name ==
            WorksheetNames.Vertices);

        Object oSelection = this.Application.Selection;

        if (oSelection != null && oSelection is Range)
        {
            asSelectedVertexNames =
                Globals.Sheet2.GetSelectedVertexNames( (Range)oSelection );
        }

        CreateSubgraphImagesDialog oCreateSubgraphImagesDialog =
            new CreateSubgraphImagesDialog(this.InnerObject,
                asSelectedVertexNames);

        oCreateSubgraphImagesDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: DeleteSubgraphThumbnails()
    //
    /// <summary>
    /// Deletes any subgraph image thumbnails in the vertex worksheet.
    /// </summary>
    //*************************************************************************

    public void
    DeleteSubgraphThumbnails()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        TableImagePopulator.DeleteImagesInColumn(this.InnerObject,
            WorksheetNames.Vertices, VertexTableColumnNames.SubgraphImage);
    }

    //*************************************************************************
    //  Method: AnalyzeEmailNetwork()
    //
    /// <summary>
    /// Shows the dialog that analyzes a user's email network and writes the
    /// results to the edge worksheet.
    /// </summary>
    //*************************************************************************

    public void
    AnalyzeEmailNetwork()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        AnalyzeEmailNetworkDialog oAnalyzeEmailNetworkDialog =
            new AnalyzeEmailNetworkDialog(this.InnerObject);

        oAnalyzeEmailNetworkDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: AnalyzeTwitterNetwork()
    //
    /// <summary>
    /// Shows the dialog that analyzes a Twitter network and writes the results
    /// to the edge worksheet.
    /// </summary>
    //*************************************************************************

    public void
    AnalyzeTwitterNetwork()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        AnalyzeTwitterNetworkDialog oAnalyzeTwitterNetworkDialog =
            new AnalyzeTwitterNetworkDialog(this.InnerObject);

        oAnalyzeTwitterNetworkDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: AutoFillWorkbook()
    //
    /// <summary>
    /// Shows the dialog that fills edge and vertex attribute columns using
    /// values from user-specified source columns.
    /// </summary>
    //*************************************************************************

    public void
    AutoFillWorkbook()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        AutoFillWorkbookDialog oAutoFillWorkbookDialog =
            new AutoFillWorkbookDialog(this.InnerObject);

        Int32 iHwnd = this.Application.Hwnd;

        oAutoFillWorkbookDialog.WorkbookAutoFilled += delegate
        {
            FireWorkbookAutoFilled();
        };

        oAutoFillWorkbookDialog.Closed += delegate
        {
            // Activate the Excel window.
            //
            // This is a workaround for an annoying and frustrating bug
            // involving the modeless AutoFillWorkbookDialog.  If the user
            // takes no action in AutoFillWorkbookDialog before closing it, the
            // Excel window gets activated when AutoFillWorkbookDialog closes,
            // as expected.  However, if he opens one of
            // AutoFillWorkbookDialog's modal dialogs, such as
            // NumericRangeColumnAutoFillUserSettingsDialog, and then closes
            // the modal dialog and AutoFillWorkbookDialog, some other
            // application besides Excel gets activated.
            //
            // Setting the owner of the modal dialog to the Excel window
            // (this.Application.Hwnd) didn't help.  Neither did setting the
            // owner of the modal dialog to AutoFillWorkbookDialog.  Nothing
            // worked except explicitly activating the Excel window when the
            // AutoFillWorkbookDialog closes.

            Win32Functions.SetForegroundWindow( new IntPtr(iHwnd) );
        };

        oAutoFillWorkbookDialog.Show( new Win32Window(iHwnd) );
    }

    //*************************************************************************
    //  Method: AutoFillWorkbookWithScheme()
    //
    /// <summary>
    /// Shows the dialog that fills attribute columns with a visual scheme.
    /// </summary>
    //*************************************************************************

    public void
    AutoFillWorkbookWithScheme()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        AutoFillWorkbookWithSchemeDialog oAutoFillWorkbookWithSchemeDialog =
            new AutoFillWorkbookWithSchemeDialog(this.InnerObject);

        if (oAutoFillWorkbookWithSchemeDialog.ShowDialog() == DialogResult.OK)
        {
            FireWorkbookAutoFilled();
        }
    }

    //*************************************************************************
    //  Method: ImportPajekFile()
    //
    /// <summary>
    /// Imports the contents of a Pajek file into the workbook.
    /// </summary>
    //*************************************************************************

    public void
    ImportPajekFile()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        // Create a graph from a Pajek file selected by the user.

        IGraph oGraph;
        OpenPajekFileDialog oDialog = new OpenPajekFileDialog();

        if (oDialog.ShowDialogAndOpenPajekFile(out oGraph) != DialogResult.OK)
        {
            return;
        }

        // Import the graph's edges and vertices into the workbook.

        PajekGraphImporter oPajekGraphImporter = new PajekGraphImporter();

        this.ScreenUpdating = false;

        try
        {
            oPajekGraphImporter.ImportPajekGraph(oGraph, this.InnerObject);

            this.ScreenUpdating = true;
        }
        catch (Exception oException)
        {
            this.ScreenUpdating = true;

            ErrorUtil.OnException(oException);

            return;
        }

        GraphDirectedness eGraphDirectedness = GraphDirectedness.Undirected;

        switch (oGraph.Directedness)
        {
            case GraphDirectedness.Undirected:

                break;

            case GraphDirectedness.Directed:

                eGraphDirectedness = GraphDirectedness.Directed;
                break;

            case GraphDirectedness.Mixed:

                FormUtil.ShowInformation( String.Format(

                    "The Pajek file contains both undirected and directed"
                    + " edges, which {0} does not allow.  All edges"
                    + " are being converted to directed edges."
                    ,
                    FormUtil.ApplicationName
                    ) );

                eGraphDirectedness = GraphDirectedness.Directed;
                break;

            default:

                Debug.Assert(false);
                break;
        }

        this.GraphDirectedness = eGraphDirectedness;

        // Pass the workbook's directedness to the Ribbon.

        this.Ribbon.GraphDirectedness = eGraphDirectedness;
    }

    //*************************************************************************
    //  Method: ShowGraphMetrics()
    //
    /// <summary>
    /// Shows the dialog that lists available graph metrics and calculates them
    /// if requested by the user.
    /// </summary>
    //*************************************************************************

    public void
    ShowGraphMetrics()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        // Create the object that might be edited by the dialog.

        GraphMetricUserSettings oGraphMetricUserSettings =
            new GraphMetricUserSettings();

        GraphMetricsDialog oGraphMetricsDialog =
            new GraphMetricsDialog(this.InnerObject, oGraphMetricUserSettings);

        if (oGraphMetricsDialog.ShowDialog() == DialogResult.OK)
        {
            // Save the edited object.

            oGraphMetricUserSettings.Save();
        }
    }

    //*************************************************************************
    //  Method: CreateClusters()
    //
    /// <summary>
    /// Partitions the graph into clusters.
    /// </summary>
    //*************************************************************************

    public void
    CreateClusters()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        // The CalculateGraphMetricsDialog does all the work.  (Clusters are
        // just another set of graph metrics.)  Use the constructor overload
        // that accepts a list of graph metric calculators.

        CalculateGraphMetricsDialog oCalculateGraphMetricsDialog =
            new CalculateGraphMetricsDialog( this.InnerObject,
                new IGraphMetricCalculator2 [] { new ClusterCalculator2() },
                new GraphMetricUserSettings(),
                new NotificationUserSettings(),
                true,
                "Create Clusters"
                );

        if (oCalculateGraphMetricsDialog.ShowDialog() == DialogResult.OK)
        {
            // Check the "read clusters" checkbox in the ribbon.

            this.Ribbon.ReadClusters = true;
            
            Worksheet oClusterWorksheet;

            if ( ExcelUtil.TryGetWorksheet(this.InnerObject,
                WorksheetNames.Clusters, out oClusterWorksheet) )
            {
                // Let the user know that something happened.

                ExcelUtil.ActivateWorksheet(oClusterWorksheet);
            }
        }
    }

    //*************************************************************************
    //  Method: ShowColumnGroup()
    //
    /// <summary>
    /// Shows or hides a specified group of related columns in a single table.
    /// </summary>
    ///
    /// <param name="columnGroup">
    /// The group of columns to show or hide.
    /// </param>
    ///
    /// <param name="show">
    /// true to show the column group, false to hide it.
    /// </param>
    ///
    /// <param name="activateWorksheet">
    /// true to activate the worksheet containing the column group.
    /// </param>
    ///
    /// <remarks>
    /// Columns that don't exist are ignored.
    ///
    /// <para>
    /// The column group's new show/hide state is stored in the workbook using
    /// the <see cref="PerWorkbookSettings" /> class.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    ShowColumnGroup
    (
        ColumnGroup columnGroup,
        Boolean show,
        Boolean activateWorksheet
    )
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        this.ScreenUpdating = false;

        try
        {
            ColumnGroupManager.ShowOrHideColumnGroup(this.InnerObject,
                columnGroup, show, activateWorksheet);
        }
        finally
        {
            this.ScreenUpdating = true;
        }
    }

    //*************************************************************************
    //  Method: ShowOrHideAllColumnGroups()
    //
    /// <summary>
    /// Shows or hides all column groups in the workbook.
    /// </summary>
    ///
    /// <param name="show">
    /// true to show all column groups, false to hide them.
    /// </param>
    ///
    /// <remarks>
    /// Hidden column groups that are only for internal use by NodeXL are not
    /// shown.
    ///
    /// <para>
    /// Columns that don't exist are ignored.
    /// </para>
    ///
    /// <para>
    /// The column groups' new show/hide states are stored in the workbook
    /// using the <see cref="PerWorkbookSettings" /> class.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    ShowOrHideAllColumnGroups
    (
        Boolean show
    )
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        // Uncomment the ScreenUpdating calls to prevent screen updates as the
        // groups are shown.  The updates may provide useful feedback, so show
        // the updates for now.

        // this.ScreenUpdating = false;

        try
        {
            ColumnGroupManager.ShowOrHideAllColumnGroups(
                this.InnerObject, show);
        }
        finally
        {
            // this.ScreenUpdating = true;
        }
    }

    //*************************************************************************
    //  Method: SetVisualAttribute()
    //
    /// <summary>
    /// Sets a visual attribute in the selected rows of the active worksheet.
    /// </summary>
    ///
    /// <param name="visualAttribute">
    /// Specifies the visual attribute to set.  Must be only one of the flags
    /// in the <see cref="VisualAttributes" /> enumeration; it cannot be an
    /// ORed combination.
    /// </param>
    ///
    /// <param name="attributeValue">
    /// The visual attribute value, or null if the value isn't known yet and
    /// must be obtained from the user.
    /// </param>
    //*************************************************************************

    public void
    SetVisualAttribute
    (
        VisualAttributes visualAttribute,
        Object attributeValue
    )
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        SetVisualAttributeEventHandler oSetVisualAttributeEventHandler =
            this.SetVisualAttribute2;

        if (oSetVisualAttributeEventHandler != null)
        {
            SetVisualAttributeEventArgs oSetVisualAttributeEventArgs =
                new SetVisualAttributeEventArgs(visualAttribute,
                    attributeValue);

            oSetVisualAttributeEventHandler(this,
                oSetVisualAttributeEventArgs);

            if (oSetVisualAttributeEventArgs.VisualAttributeSet)
            {
                FireVisualAttributeSetInWorkbook();
            }
        }
    }

    //*************************************************************************
    //  Event: SelectionChangedInWorkbook
    //
    /// <summary>
    /// Occurs when the selection state of the edge or vertex table changes.
    /// </summary>
    ///
    /// <remarks>
    /// If the selection state of the edge table changes, the <see
    /// cref="SelectionChangedEventArgs.SelectedEdgeIDs" /> property of the
    /// <see cref="SelectionChangedEventArgs" /> object passed to the event
    /// handler contains the IDs of the selected edges.  The <see
    /// cref="SelectionChangedEventArgs.SelectedVertexIDs" /> property is an
    /// empty array in this case.
    ///
    /// <para>
    /// If the selection state of the vertex table changes, the <see
    /// cref="SelectionChangedEventArgs.SelectedVertexIDs" /> property of the
    /// <see cref="SelectionChangedEventArgs" /> object passed to the event
    /// handler contains the IDs of the selected vertices.  The <see
    /// cref="SelectionChangedEventArgs.SelectedEdgeIDs" /> property is an
    /// empty array in this case.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public event SelectionChangedEventHandler SelectionChangedInWorkbook;


    //*************************************************************************
    //  Event: VisualAttributeSetInWorkbook
    //
    /// <summary>
    /// Occurs when a visual attribute is set in the selected rows of the
    /// active worksheet.
    /// </summary>
    //*************************************************************************

    public event EventHandler VisualAttributeSetInWorkbook;


    //*************************************************************************
    //  Event: SelectionChangedInGraph
    //
    /// <summary>
    /// Occurs when the selection state of the NodeXL graph changes.
    /// </summary>
    ///
    /// <remarks>
    /// The <see cref="SelectionChangedEventArgs" /> object passed to the event
    /// handler contains the IDs of all vertices and edges that are currently
    /// selected in the graph.
    /// </remarks>
    //*************************************************************************

    public event SelectionChangedEventHandler SelectionChangedInGraph;


    //*************************************************************************
    //  Event: VertexAttributesEditedInGraph
    //
    /// <summary>
    /// Occurs when vertex attributes are edited in the NodeXL graph.
    /// </summary>
    //*************************************************************************

    public event VertexAttributesEditedEventHandler
        VertexAttributesEditedInGraph;


    //*************************************************************************
    //  Event: GraphDrawn
    //
    /// <summary>
    /// Occurs after graph drawing completes.
    /// </summary>
    ///
    /// <remarks>
    /// Graph drawing occurs asynchronously.  This event fires when the graph
    /// is completely drawn.
    /// </remarks>
    //*************************************************************************

    public event GraphDrawnEventHandler GraphDrawn;


    //*************************************************************************
    //  Event: VerticesMoved
    //
    /// <summary>
    /// Occurs after one or more vertices are moved to new locations in the
    /// graph.
    /// </summary>
    ///
    /// <remarks>
    /// This event is fired when the user releases the mouse button after
    /// dragging one or more vertices to new locations in the graph.
    /// </remarks>
    //*************************************************************************

    public event VerticesMovedEventHandler2 VerticesMoved;


    //*************************************************************************
    //  Event: WorkbookAutoFilled
    //
    /// <summary>
    /// Occurs when the workbook is autofilled by the <see
    /// cref="AutoFillWorkbookDialog" /> or <see
    /// cref="AutoFillWorkbookWithSchemeDialog" />.
    /// </summary>
    //*************************************************************************

    public event EventHandler WorkbookAutoFilled;


    //*************************************************************************
    //  Event: SetVisualAttribute2
    //
    /// <summary>
    /// Occurs when one of the visual attribute buttons in the Ribbon is
    /// clicked.
    /// </summary>
    ///
    /// <remarks>
    /// This is named SetVisualAttribute2 to avoid a conflict with the method
    /// of the same name.
    /// </remarks>
    //*************************************************************************

    public event SetVisualAttributeEventHandler SetVisualAttribute2;


    //*************************************************************************
    //  Property: Ribbon
    //
    /// <summary>
    /// Gets the application's ribbon.
    /// </summary>
    ///
    /// <value>
    /// The application's ribbon.
    /// </value>
    //*************************************************************************

    private Ribbon
    Ribbon
    {
        get
        {
            AssertValid();

            return (Globals.Ribbons.Ribbon);
        }
    }

    //*************************************************************************
    //  Property: GraphVisibility
    //
    /// <summary>
    /// Gets or sets the visibility of the NodeXL graph.
    /// </summary>
    ///
    /// <value>
    /// true if the NodeXL graph is visible.
    /// </value>
    //*************************************************************************

    private Boolean
    GraphVisibility
    {
        get
        {
            AssertValid();

            return (this.DocumentActionsCommandBar.Visible &&
                m_bTaskPaneCreated);
        }

        set
        {
            if (value && !m_bTaskPaneCreated)
            {
                // The NodeXL task pane is created in a lazy manner.

                TaskPane oTaskPane = new TaskPane(this, this.Ribbon);

                this.ActionsPane.Clear();
                this.ActionsPane.Controls.Add(oTaskPane);

                oTaskPane.Dock = DockStyle.Fill;

                oTaskPane.SelectionChangedInGraph +=
                    new SelectionChangedEventHandler(
                        this.TaskPane_SelectionChangedInGraph);

                oTaskPane.VertexAttributesEditedInGraph +=
                    new VertexAttributesEditedEventHandler(
                        this.TaskPane_VertexAttributesEditedInGraph);

                oTaskPane.GraphDrawn += new GraphDrawnEventHandler(
                    this.TaskPane_GraphDrawn);

                oTaskPane.VerticesMoved += new VerticesMovedEventHandler2(
                    this.TaskPane_VerticesMoved);

                m_bTaskPaneCreated = true;
            }

            this.DocumentActionsCommandBar.Visible = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DocumentActionsCommandBar
    //
    /// <summary>
    /// Gets the document actions CommandBar.
    /// </summary>
    ///
    /// <value>
    /// The document actions CommandBar, which is where the NodeXL graph is
    /// displayed.
    /// </value>
    //*************************************************************************

    private Microsoft.Office.Core.CommandBar
    DocumentActionsCommandBar
    {
        get
        {
            AssertValid();

            return ( Application.CommandBars["Document Actions"] );
        }
    }

    //*************************************************************************
    //  Property: PerWorkbookSettings
    //
    /// <summary>
    /// Gets a new PerWorkbookSettings object.
    /// </summary>
    ///
    /// <value>
    /// A new PerWorkbookSettings object.
    /// </value>
    //*************************************************************************

    private PerWorkbookSettings
    PerWorkbookSettings
    {
        get
        {
            AssertValid();

            return ( new PerWorkbookSettings(this.InnerObject) );
        }
    }

    //*************************************************************************
    //  Property: ScreenUpdating
    //
    /// <summary>
    /// Gets or sets a flag specifying whether Excel's screen updating is on or
    /// off.
    /// </summary>
    ///
    /// <value>
    /// true to turn on screen updating.
    /// </value>
    //*************************************************************************

    private Boolean
    ScreenUpdating
    {
        get
        {
            AssertValid();

            return (this.Application.ScreenUpdating);
        }

        set
        {
            this.Application.ScreenUpdating = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: EnableSetVisualAttributes
    //
    /// <summary>
    /// Enables the "set visual attribute" buttons in the Ribbon.
    /// </summary>
    //*************************************************************************

    private void
    EnableSetVisualAttributes()
    {
        AssertValid();

        VisualAttributes eVisualAttributes = VisualAttributes.None;
        String sWorksheetName = null;
        Object oActiveSheet = this.ActiveSheet;

        if (oActiveSheet is Worksheet)
        {
            sWorksheetName = ( (Worksheet)oActiveSheet ).Name;
        }

        // Determine whether the active worksheet has a NodeXL table that
        // includes a selection.

        String sTableName = null;

        switch (sWorksheetName)
        {
            case WorksheetNames.Edges:

                sTableName = TableNames.Edges;
                break;

            case WorksheetNames.Vertices:

                sTableName = TableNames.Vertices;
                break;

            case WorksheetNames.Clusters:

                sTableName = TableNames.Clusters;
                break;

            default:

                break;
        }

        ListObject oTable;
        Range oSelectedTableRange;

        if (
            sTableName != null
            &&
            ExcelUtil.TryGetSelectedTableRange(this.InnerObject,
                sWorksheetName, sTableName, out oTable,
                out oSelectedTableRange)
            )
        {
            if (sWorksheetName == WorksheetNames.Edges)
            {
                eVisualAttributes |= VisualAttributes.Color;
                eVisualAttributes |= VisualAttributes.Alpha;
                eVisualAttributes |= VisualAttributes.EdgeWidth;
                eVisualAttributes |= VisualAttributes.EdgeVisibility;
            }
            else if (sWorksheetName == WorksheetNames.Vertices)
            {
                eVisualAttributes |= VisualAttributes.Color;
                eVisualAttributes |= VisualAttributes.Alpha;
                eVisualAttributes |= VisualAttributes.VertexShape;
                eVisualAttributes |= VisualAttributes.VertexRadius;
                eVisualAttributes |= VisualAttributes.VertexVisibility;
            }
            else if (sWorksheetName == WorksheetNames.Clusters)
            {
                eVisualAttributes |= VisualAttributes.Color;
                eVisualAttributes |= VisualAttributes.VertexShape;
            }
        }

        this.Ribbon.EnableSetVisualAttributes(eVisualAttributes);
    }

    //*************************************************************************
    //  Method: UpdateColumnNames()
    //
    /// <summary>
    /// Updates any columns whose names have been changed since earlier NodeXL
    /// versions.
    /// </summary>
    //*************************************************************************

    private void
    UpdateColumnNames()
    {
        AssertValid();

        Globals.Sheet2.UpdateColumnNames();
    }

    //*************************************************************************
    //  Method: FireSelectionChangedInWorkbook()
    //
    /// <summary>
    /// Fires the <see cref="SelectionChangedInWorkbook" /> event if
    /// appropriate.
    /// </summary>
    ///
    /// <param name="aiSelectedEdgeIDs">
    /// Array of unique IDs of edges that have at least one selected cell.  Can
    /// be empty but not null.
    /// </param>
    ///
    /// <param name="aiSelectedVertexIDs">
    /// Array of unique IDs of vertices that have at least one selected cell.
    /// Can be empty but not null.
    /// </param>
    //*************************************************************************

    private void
    FireSelectionChangedInWorkbook
    (
        Int32 [] aiSelectedEdgeIDs,
        Int32 [] aiSelectedVertexIDs
    )
    {
        Debug.Assert(aiSelectedEdgeIDs != null);
        Debug.Assert(aiSelectedVertexIDs != null);
        AssertValid();

        SelectionChangedEventHandler oSelectionChangedInWorkbook =
            this.SelectionChangedInWorkbook;

        if (oSelectionChangedInWorkbook != null)
        {
            try
            {
                oSelectionChangedInWorkbook(this,
                    new SelectionChangedEventArgs(
                        aiSelectedEdgeIDs, aiSelectedVertexIDs) );
            }
            catch (Exception oException)
            {
                // If exceptions aren't caught here, Excel consumes them
                // without indicating that anything is wrong.

                ErrorUtil.OnException(oException);
            }
        }
    }

    //*************************************************************************
    //  Method: FireVisualAttributeSetInWorkbook()
    //
    /// <summary>
    /// Fires the <see cref="VisualAttributeSetInWorkbook" /> event if
    /// appropriate.
    /// </summary>
    //*************************************************************************

    private void
    FireVisualAttributeSetInWorkbook()
    {
        AssertValid();

        if (this.Ribbon.AutoReadWorkbook)
        {
            // The TaskPane handles this event by reading the workbook, which
            // clears the selection in the graph.  That fires the TaskPane's
            // SelectionChangedInGraph event, which this class handles by
            // clearing the selection in the workbook.  That can be jarring to
            // the user, who just made a selection, set a visual attribute on
            // the selection, and then saw the selection disappear.
            //
            // Fix this by temporarily igoring the SelectionChangedInGraph
            // event, which causes the selection in the workbook to be
            // retained.  The selection in the graph (which is empty) will then
            // be out of sync with the selection in the workbook, but I think
            // that is tolerable.

            m_bIgnoreSelectionChangedInGraph = true;
            EventUtil.FireEvent(this, this.VisualAttributeSetInWorkbook);
            m_bIgnoreSelectionChangedInGraph = false;
        }
    }

    //*************************************************************************
    //  Method: FireWorkbookAutoFilled()
    //
    /// <summary>
    /// Fires the <see cref="WorkbookAutoFilled" /> event if appropriate.
    /// </summary>
    //*************************************************************************

    private void
    FireWorkbookAutoFilled()
    {
        AssertValid();

        if (this.Ribbon.AutoReadWorkbook)
        {
            EventUtil.FireEvent(this, this.WorkbookAutoFilled);
        }
    }

    //*************************************************************************
    //  Method: Workbook_Startup()
    //
    /// <summary>
    /// Handles the Startup event on the workbook.
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
    ThisWorkbook_Startup
    (
        object sender,
        System.EventArgs e
    )
    {
        m_bTaskPaneCreated = false;
        m_bIgnoreSelectionChangedInGraph = false;

        Sheet1 oSheet1 = Globals.Sheet1;
        Sheet2 oSheet2 = Globals.Sheet2;
        Sheet5 oSheet5 = Globals.Sheet5;

        m_oWorksheetContextMenuManager = new WorksheetContextMenuManager(
            this, oSheet1, oSheet1.Edges, oSheet2, oSheet2.Vertices, oSheet5,
            oSheet5.Clusters);

        // In message boxes, show the name of this document customization
        // instead of the default, which is the name of the Excel application.

        FormUtil.ApplicationName = ApplicationUtil.ApplicationName;

        oSheet1.EdgeSelectionChanged += new TableSelectionChangedEventHandler(
            this.Sheet1_EdgeSelectionChanged);

        oSheet2.VertexSelectionChanged += new TableSelectionChangedEventHandler(
            this.Sheet2_VertexSelectionChanged);

        oSheet5.ClusterSelectionChanged +=
            new TableSelectionChangedEventHandler(
                this.Sheet5_ClusterSelectionChanged);

        this.New += new
            Microsoft.Office.Tools.Excel.WorkbookEvents_NewEventHandler(
                ThisWorkbook_New);

        this.ActivateEvent += new
            Microsoft.Office.Interop.Excel.WorkbookEvents_ActivateEventHandler(
                ThisWorkbook_ActivateEvent);

        this.SheetActivate += new WorkbookEvents_SheetActivateEventHandler(
            ThisWorkbook_SheetActivate);

        // Show the NodeXL graph by default.

        this.GraphVisibility = true;

        // Update any columns whose names have been changed since earlier
        // NodeXL versions.

        UpdateColumnNames();

        AssertValid();
    }

    //*************************************************************************
    //  Method: ThisWorkbook_New()
    //
    /// <summary>
    /// Handles the New event on the workbook.
    /// </summary>
    //*************************************************************************

    private void
    ThisWorkbook_New()
    {
        AssertValid();

        // Get the graph directedness for new workbooks and store it in the
        // per-workbook settings.

        GeneralUserSettings oGeneralUserSettings = new GeneralUserSettings();

        this.PerWorkbookSettings.GraphDirectedness =
            oGeneralUserSettings.NewWorkbookGraphDirectedness;
    }

    //*************************************************************************
    //  Method: ThisWorkbook_ActivateEvent()
    //
    /// <summary>
    /// Handles the ActivateEvent event on the workbook.
    /// </summary>
    //*************************************************************************

    private void
    ThisWorkbook_ActivateEvent()
    {
        AssertValid();

        // Update the Ribbon.

        this.Ribbon.GraphDirectedness = this.GraphDirectedness;
        EnableSetVisualAttributes();
    }

    //*************************************************************************
    //  Method: ThisWorkbook_SheetActivate()
    //
    /// <summary>
    /// Handles the SheetActivate event on the workbook.
    /// </summary>
    ///
    /// <param name="Sh">
    /// The activated sheet.
    /// </param>
    //*************************************************************************

    private void
    ThisWorkbook_SheetActivate
    (
        object Sh
    )
    {
        AssertValid();
        Debug.Assert(Sh != null);

        EnableSetVisualAttributes();
    }

    //*************************************************************************
    //  Method: ThisWorkbook_Shutdown()
    //
    /// <summary>
    /// Handles the Shutdown event on the workbook.
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
    ThisWorkbook_Shutdown
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: Sheet1_EdgeSelectionChanged()
    //
    /// <summary>
    /// Handles the EdgeSelectionChanged event on the Sheet1 (edge) worksheet.
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
    Sheet1_EdgeSelectionChanged
    (
        Object sender,
        TableSelectionChangedEventArgs e
    )
    {
        Debug.Assert(e != null);
        Debug.Assert(e.SelectedIDs != null);
        AssertValid();

        if ( !this.ExcelApplicationIsReady(false) )
        {
            return;
        }

        // If the event was caused by the user selecting edges in the edge
        // worksheet, forward the event to the TaskPane.  Otherwise, avoid an
        // endless loop by doing nothing.

        if (e.EventOrigin ==
            TableSelectionChangedEventOrigin.SelectionChangedInTable)
        {
            FireSelectionChangedInWorkbook(e.SelectedIDs,
                WorksheetReaderBase.EmptyIDArray);
        }

        EnableSetVisualAttributes();
    }

    //*************************************************************************
    //  Method: Sheet2_VertexSelectionChanged()
    //
    /// <summary>
    /// Handles the VertexSelectionChanged event on the Sheet2 (vertex)
    /// worksheet.
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
    Sheet2_VertexSelectionChanged
    (
        Object sender,
        TableSelectionChangedEventArgs e
    )
    {
        Debug.Assert(e != null);
        Debug.Assert(e.SelectedIDs != null);
        AssertValid();

        if ( !this.ExcelApplicationIsReady(false) )
        {
            return;
        }

        // If the event was caused by the user selecting vertices in the vertex
        // worksheet, forward the event to the TaskPane.  Otherwise, avoid an
        // endless loop and do nothing.

        if (e.EventOrigin == TableSelectionChangedEventOrigin.
            SelectionChangedInTable)
        {
            FireSelectionChangedInWorkbook(WorksheetReaderBase.EmptyIDArray,
                e.SelectedIDs);
        }

        EnableSetVisualAttributes();
    }

    //*************************************************************************
    //  Method: Sheet5_ClusterSelectionChanged()
    //
    /// <summary>
    /// Handles the ClusterSelectionChanged event on the Sheet5 (cluster)
    /// worksheet.
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
    Sheet5_ClusterSelectionChanged
    (
        Object sender,
        TableSelectionChangedEventArgs e
    )
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(false) )
        {
            return;
        }

        EnableSetVisualAttributes();
    }

    //*************************************************************************
    //  Method: TaskPane_SelectionChangedInGraph()
    //
    /// <summary>
    /// Handles the SelectionChangedInGraph event on the TaskPane.
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
    /// This event is fired when the user clicks on the NodeXL graph.
    /// </remarks>
    //*************************************************************************

    private void
    TaskPane_SelectionChangedInGraph
    (
        Object sender,
        SelectionChangedEventArgs e
    )
    {
        Debug.Assert(e != null);
        AssertValid();

        if (!this.ExcelApplicationIsReady(false) ||
            m_bIgnoreSelectionChangedInGraph)
        {
            return;
        }

        // Forward the event.

        SelectionChangedEventHandler oSelectionChangedInGraph =
            this.SelectionChangedInGraph;

        if (oSelectionChangedInGraph != null)
        {
            try
            {
                oSelectionChangedInGraph(this, e);
            }
            catch (Exception oException)
            {
                ErrorUtil.OnException(oException);
            }
        }

        EnableSetVisualAttributes();
    }

    //*************************************************************************
    //  Method: TaskPane_VertexAttributesEditedInGraph()
    //
    /// <summary>
    /// Handles the VertexAttributesEditedInGraph event on the TaskPane.
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
    /// This event is fired when the user edits vertex attributes in the NodeXL
    /// graph.
    /// </remarks>
    //*************************************************************************

    private void
    TaskPane_VertexAttributesEditedInGraph
    (
        Object sender,
        VertexAttributesEditedEventArgs e
    )
    {
        Debug.Assert(e != null);
        AssertValid();

        if ( !this.ExcelApplicationIsReady(false) )
        {
            return;
        }

        // Forward the event.

        VertexAttributesEditedEventHandler oVertexAttributesEditedInGraph =
            this.VertexAttributesEditedInGraph;

        if (oVertexAttributesEditedInGraph != null)
        {
            try
            {
                oVertexAttributesEditedInGraph(this, e);
            }
            catch (Exception oException)
            {
                ErrorUtil.OnException(oException);
            }
        }
    }

    //*************************************************************************
    //  Method: TaskPane_GraphDrawn()
    //
    /// <summary>
    /// Handles the GraphDrawn event on the TaskPane.
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
    /// Graph drawing occurs asynchronously.  This event fires when the graph
    /// is completely drawn.
    /// </remarks>
    //*************************************************************************

    private void
    TaskPane_GraphDrawn
    (
        Object sender,
        GraphDrawnEventArgs e
    )
    {
        Debug.Assert(e != null);
        AssertValid();

        if ( !this.ExcelApplicationIsReady(false) )
        {
            return;
        }

        // Forward the event.

        GraphDrawnEventHandler oGraphDrawn = this.GraphDrawn;

        if (oGraphDrawn != null)
        {
            try
            {
                oGraphDrawn(this, e);
            }
            catch (Exception oException)
            {
                ErrorUtil.OnException(oException);
            }
        }
    }

    //*************************************************************************
    //  Method: TaskPane_VerticesMoved()
    //
    /// <summary>
    /// Handles the VerticesMoved event on the TaskPane.
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
    TaskPane_VerticesMoved
    (
        Object sender,
        VerticesMovedEventArgs2 e
    )
    {
        Debug.Assert(e != null);
        AssertValid();

        if ( !this.ExcelApplicationIsReady(false) )
        {
            return;
        }

        // Forward the event.

        VerticesMovedEventHandler2 oVerticesMoved = this.VerticesMoved;

        if (oVerticesMoved != null)
        {
            try
            {
                oVerticesMoved(this, e);
            }
            catch (Exception oException)
            {
                ErrorUtil.OnException(oException);
            }
        }
    }


    #region VSTO Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InternalStartup()
    {
        this.Startup += new System.EventHandler(ThisWorkbook_Startup);

        this.Shutdown += new System.EventHandler(ThisWorkbook_Shutdown);
    }
        
    #endregion


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
        Debug.Assert(m_oWorksheetContextMenuManager != null);
        // m_bTaskPaneCreated
    }


    //*************************************************************************
    //  Private fields
    //*************************************************************************

    /// Object that adds custom menu items to the Excel context menus that
    /// appear when the vertex or edge table is right-clicked.

    private WorksheetContextMenuManager m_oWorksheetContextMenuManager;

    /// true if the task pane has been created.

    private Boolean m_bTaskPaneCreated;

    /// true to ignore the TaskPane.SelectionChangedInGraph event.

    private Boolean m_bIgnoreSelectionChangedInGraph;
}

}
