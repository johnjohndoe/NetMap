
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using System.Reflection;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization.Wpf;
using Microsoft.NodeXL.Algorithms;
using Microsoft.NodeXL.Adapters;
using Microsoft.NodeXL.ApplicationUtil;
using Microsoft.NodeXL.ExcelTemplatePlugIns;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ThisWorkbook
//
/// <summary>
/// Represent's a workbook created from the template.
/// </summary>
//*****************************************************************************

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
    //  Property: ShowWaitCursor
    //
    /// <summary>
    /// Sets a flag specifying whether the wait cursor should be shown.
    /// </summary>
    ///
    /// <value>
    /// true to show the wait cursor.
    /// </value>
    //*************************************************************************

    public Boolean
    ShowWaitCursor
    {
        set
        {
            this.Application.Cursor = value ?
                Microsoft.Office.Interop.Excel.XlMousePointer.xlWait
                :
                Microsoft.Office.Interop.Excel.XlMousePointer.xlDefault;

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
            new ImportFromMatrixWorkbookDialog(this.InnerObject,
                this.Ribbon.ClearTablesBeforeImport);

        if (oImportFromMatrixWorkbookDialog.ShowDialog() == DialogResult.OK)
        {
            GraphDirectedness eGraphDirectedness =
                oImportFromMatrixWorkbookDialog.SourceWorkbookDirectedness;

            this.GraphDirectedness = eGraphDirectedness;
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
            new ImportFromEdgeWorkbookDialog(this.InnerObject,
                this.Ribbon.ClearTablesBeforeImport);

        oImportFromEdgeWorkbookDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: ImportFromGraphDataProvider()
    //
    /// <summary>
    /// Imports a graph from an IGraphDataProvider implementation.
    /// </summary>
    //*************************************************************************

    public void
    ImportFromGraphDataProvider
    (
        IGraphDataProvider graphDataProvider
    )
    {
        Debug.Assert(graphDataProvider != null);
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        ShowWaitCursor = true;

        try
        {
            String sGraphDataAsGraphML;

            if (!graphDataProvider.TryGetGraphData(out sGraphDataAsGraphML) )
            {
                return;
            }

            IGraph oGraph = ( new GraphMLGraphAdapter() ).LoadGraphFromString(
                sGraphDataAsGraphML);

            ImportGraph(oGraph, 

                ( String[] )oGraph.GetRequiredValue(
                    ReservedMetadataKeys.AllEdgeMetadataKeys,
                    typeof( String[] ) ),

                ( String[] )oGraph.GetRequiredValue(
                    ReservedMetadataKeys.AllVertexMetadataKeys,
                    typeof( String[] ) )
                );
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
        }
        finally
        {
            ShowWaitCursor = false;
        }
    }

    //*************************************************************************
    //  Method: ExportToUcinetFile()
    //
    /// <summary>
    /// Exports the edge and vertex tables to a new UCINET full matrix DL file.
    /// </summary>
    //*************************************************************************

    public void
    ExportToUcinetFile()
    {
        AssertValid();

        if (
            !this.ExcelApplicationIsReady(true)
            ||
            !MergeIsApproved(
                "add an Edge Weight column, and export the edges to a new"
                + " UCINET full matrix DL file.")
            )
        {
            return;
        }

        ReadWorkbookContext oReadWorkbookContext = new ReadWorkbookContext();
        oReadWorkbookContext.ReadEdgeWeights = true;

        SaveUcinetFileDialog oSaveUcinetFileDialog =
            new SaveUcinetFileDialog(String.Empty, String.Empty);

        ExportToFile(oReadWorkbookContext, oSaveUcinetFileDialog);
    }

    //*************************************************************************
    //  Method: ExportToGraphMLFile()
    //
    /// <summary>
    /// Exports the edge and vertex tables to a new GraphML file.
    /// </summary>
    //*************************************************************************

    public void
    ExportToGraphMLFile()
    {
        AssertValid();

        if (
            !this.ExcelApplicationIsReady(true)
            ||
            !MergeIsApproved(
                "add an Edge Weight column, and export the edges and vertices"
                + " to a new GraphML file.")
            )
        {
            return;
        }

        ReadWorkbookContext oReadWorkbookContext = new ReadWorkbookContext();
        oReadWorkbookContext.ReadAllEdgeAndVertexColumns = true;

        SaveGraphMLFileDialog oSaveGraphMLFileDialog =
            new SaveGraphMLFileDialog(String.Empty, String.Empty);

        ExportToFile(oReadWorkbookContext, oSaveGraphMLFileDialog);
    }

    //*************************************************************************
    //  Method: ExportToPajekFile()
    //
    /// <summary>
    /// Exports the edge and vertex tables to a new Pajek text file.
    /// </summary>
    //*************************************************************************

    public void
    ExportToPajekFile()
    {
        AssertValid();

        if (
            !this.ExcelApplicationIsReady(true)
            ||
            !MergeIsApproved(
                "add an Edge Weight column, and export the edges and vertices"
                + " to a new Pajek file.")
            )
        {
            return;
        }

        ReadWorkbookContext oReadWorkbookContext = new ReadWorkbookContext();
        oReadWorkbookContext.ReadEdgeWeights = true;

        // Map any vertex coordinates stored in the workbook to an arbitrary
        // rectangle.  PajekGraphAdapter will in turn map these to Pajek
        // coordinates.

        oReadWorkbookContext.IgnoreVertexLocations = false;

        oReadWorkbookContext.GraphRectangle =
            new System.Drawing.Rectangle(0, 0, 10000, 10000);

        SavePajekFileDialog oSavePajekFileDialog =
            new SavePajekFileDialog(String.Empty, String.Empty);

        ExportToFile(oReadWorkbookContext, oSavePajekFileDialog);
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

        if (
            !this.ExcelApplicationIsReady(true)
            ||
            !MergeIsApproved(
                "add an Edge Weight column, and export the edges to a new"
                + " workbook as an adjacency matrix.")
            )
        {
            return;
        }

        ShowWaitCursor = true;

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

        ShowWaitCursor = false;
    }

    //*************************************************************************
    //  Method: ShowGraph()
    //
    /// <summary>
    /// Shows the NodeXL graph.
    /// </summary>
    ///
    /// <returns>
    /// true if the graph was shown, false if the application isn't ready.
    /// </returns>
    //*************************************************************************

    public Boolean
    ShowGraph()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return (false);
        }

        this.GraphVisibility = true;
        return (true);
    }

    //*************************************************************************
    //  Method: ConvertNodeXLWorkbook()
    //
    /// <summary>
    /// Copies a NodeXL workbook created on another machine and converts the
    /// copy to work on this machine.
    /// </summary>
    //*************************************************************************

    public void
    ConvertNodeXLWorkbook()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        ConvertNodeXLWorkbookDialog oConvertNodeXLWorkbookDialog =
            new ConvertNodeXLWorkbookDialog(this.Application);

        oConvertNodeXLWorkbookDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: MergeDuplicateEdges()
    //
    /// <summary>
    /// Merges duplicate edges in the edge worksheet.
    /// </summary>
    ///
    /// <param name="requestMergeApproval">
    /// true to request approval from the user to merge duplicate edges.
    /// </param>
    ///
    /// <returns>
    /// true if the duplicate edges were successfully merged.
    /// </returns>
    //*************************************************************************

    public Boolean
    MergeDuplicateEdges
    (
        Boolean requestMergeApproval
    )
    {
        AssertValid();

        if (
            !this.ExcelApplicationIsReady(true)
            ||
                (
                requestMergeApproval &&
                !MergeIsApproved("and add an Edge Weight column.")
                )
            )
        {
            return (false);
        }

        // Create and use the object that merges duplicate edges.

        DuplicateEdgeMerger oDuplicateEdgeMerger = new DuplicateEdgeMerger();

        ShowWaitCursor = true;

        this.ScreenUpdating = false;

        try
        {
            oDuplicateEdgeMerger.MergeDuplicateEdges(this.InnerObject);
            this.ScreenUpdating = true;

            return (true);
        }
        catch (Exception oException)
        {
            // Don't let Excel handle unhandled exceptions.

            this.ScreenUpdating = true;
            ErrorUtil.OnException(oException);

            return (false);
        }
        finally
        {
            ShowWaitCursor = false;
        }
    }

    //*************************************************************************
    //  Method: AutomateTasks()
    //
    /// <summary>
    /// Opens a dialog box that lets the user run multiple tasks.
    /// </summary>
    //*************************************************************************

    public void
    AutomateTasks()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        AutomateTasksDialog oAutomateTasksDialog =
            new AutomateTasksDialog(this, this.Ribbon);

        oAutomateTasksDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: AutomateTasksOnOpen()
    //
    /// <summary>
    /// Runs task automation on the workbook immediately after it is opened.
    /// </summary>
    //*************************************************************************

    public void
    AutomateTasksOnOpen()
    {
        AssertValid();

        AutomateTasksUserSettings oAutomateTasksUserSettings =
            new AutomateTasksUserSettings();

        try
        {
            TaskAutomator.AutomateThisWorkbook(this,
                oAutomateTasksUserSettings.TasksToRun, this.Ribbon);
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
            return;
        }

        if ( (oAutomateTasksUserSettings.TasksToRun &
            AutomationTasks.ReadWorkbook) != 0 )
        {
            // The graph is being asynchronously laid out in the graph pane.
            // Wait for the layout to complete.

            GraphLaidOutEventHandler oGraphLaidOutEventHandler = null;

            oGraphLaidOutEventHandler =
                delegate(Object sender, GraphLaidOutEventArgs e)
            {
                // In case something went wrong while automating this workbook,
                // prevent this delegate from being called again.

                this.GraphLaidOut -= oGraphLaidOutEventHandler;

                OnTasksAutomatedOnOpen();
            };

            this.GraphLaidOut += oGraphLaidOutEventHandler;
        }
        else
        {
            OnTasksAutomatedOnOpen();
        }
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
    ///
    /// <param name="mode">
    /// Indicates the mode in which the CreateSubgraphImagesDialog is being
    /// used.
    /// </param>
    //*************************************************************************

    public void
    CreateSubgraphImages
    (
        CreateSubgraphImagesDialog.DialogMode mode
    )
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        // Populate the vertex worksheet.  This is necessary in case the user
        // opts to insert images into the vertex worksheet.  Note that
        // PopulateVertexWorksheet() returns false if the vertex worksheet
        // or table is missing, and that it activates the vertex worksheet.

        if ( !PopulateVertexWorksheet(true, true) )
        {
            return;
        }

        ICollection<String> oSelectedVertexNames = new String[0];

        if (mode == CreateSubgraphImagesDialog.DialogMode.Normal)
        {
            // (PopulateVertexWorksheet() should have selected the vertex
            // worksheet.)

            Debug.Assert(this.Application.ActiveSheet is Worksheet);

            Debug.Assert( ( (Worksheet)this.Application.ActiveSheet ).Name ==
                WorksheetNames.Vertices);

            // Get an array of vertex names that are selected in the vertex
            // worksheet.

            oSelectedVertexNames = Globals.Sheet2.GetSelectedVertexNames();
        }

        CreateSubgraphImagesDialog oCreateSubgraphImagesDialog =
            new CreateSubgraphImagesDialog(this.InnerObject,
                oSelectedVertexNames, mode);

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
            new AnalyzeEmailNetworkDialog(this.InnerObject,
                this.Ribbon.ClearTablesBeforeImport);

        oAnalyzeEmailNetworkDialog.AnalysisSuccessful +=
            delegate(Object sender, EventArgs e)
            {
                // Note that the ribbon won't actually update until the modal
                // dialog closes.  I haven't found a way to work around this
                // odd Excel behavior.

                this.Ribbon.GraphDirectedness = this.GraphDirectedness =
                    GraphDirectedness.Directed;
            };

        oAnalyzeEmailNetworkDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: AutoFillWorkbook()
    //
    /// <summary>
    /// Shows the dialog that fills edge and vertex attribute columns using
    /// values from user-specified source columns.
    /// </summary>
    ///
    /// <param name="mode">
    /// Indicates the mode in which the AutoFillWorkbookDialog is being used.
    /// </param>
    //*************************************************************************

    public void
    AutoFillWorkbook
    (
        AutoFillWorkbookDialog.DialogMode mode
    )
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        AutoFillWorkbookDialog oAutoFillWorkbookDialog =
            new AutoFillWorkbookDialog(this.InnerObject, mode);

        Int32 iHwnd = this.Application.Hwnd;

        oAutoFillWorkbookDialog.WorkbookAutoFilled += delegate
        {
            this.Ribbon.OnWorkbookAutoFilled(
                ( new GeneralUserSettings() ).AutoReadWorkbook );
        };

        oAutoFillWorkbookDialog.Closed += delegate
        {
            // Activate the Excel window.
            //
            // This is a workaround for an annoying and frustrating bug
            // involving the AutoFillWorkbookDialog when it runs modeless.  If
            // the user takes no action in AutoFillWorkbookDialog before
            // closing it, the Excel window gets activated when
            // AutoFillWorkbookDialog closes, as expected.  However, if he
            // opens one of AutoFillWorkbookDialog's modal dialogs, such as
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

        if (mode == AutoFillWorkbookDialog.DialogMode.Normal)
        {
            oAutoFillWorkbookDialog.Show( new Win32Window(iHwnd) );
        }
        else
        {
            oAutoFillWorkbookDialog.ShowDialog();
        }
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

        if (
            oAutoFillWorkbookWithSchemeDialog.ShowDialog() == DialogResult.OK
            &&
            ( new GeneralUserSettings() ).AutoReadWorkbook
            )
        {
            this.Ribbon.OnReadWorkbookClick();
        }
    }

    //*************************************************************************
    //  Method: ImportFromUcinetFile()
    //
    /// <summary>
    /// Imports the contents of a UCINET full matrix DL file into the workbook.
    /// </summary>
    //*************************************************************************

    public void
    ImportFromUcinetFile()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        // Create a graph from a Ucinet file selected by the user.

        OpenUcinetFileDialog oOpenUcinetFileDialog =
            new OpenUcinetFileDialog();

        if (oOpenUcinetFileDialog.ShowDialog() == DialogResult.OK)
        {
            // Import the graph's edges and vertices into the workbook.
            //
            // To accommodate the requirements of the general-purpose
            // ImportGraph() method, duplicate the standard
            // ReservedMetadataKeys.EdgeWeight key, which is some arbitrary
            // string, with the name of the Excel column name where the key's
            // values should get written.

            foreach (IEdge oEdge in oOpenUcinetFileDialog.Graph.Edges)
            {
                Object oEdgeWeight;

                if ( oEdge.TryGetValue(ReservedMetadataKeys.EdgeWeight,
                    typeof(Double), out oEdgeWeight) )
                {
                    oEdge.SetValue(EdgeTableColumnNames.EdgeWeight,
                        (Double)oEdgeWeight);
                }
            }

            ImportGraph(oOpenUcinetFileDialog.Graph,
                new String[] {EdgeTableColumnNames.EdgeWeight}, null);
        }
    }

    //*************************************************************************
    //  Method: ImportFromPajekFile()
    //
    /// <summary>
    /// Imports the contents of a Pajek file into the workbook.
    /// </summary>
    //*************************************************************************

    public void
    ImportFromPajekFile()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        // Create a graph from a Pajek file selected by the user.

        IGraph oGraph;
        OpenPajekFileDialog oDialog = new OpenPajekFileDialog();

        if (oDialog.ShowDialogAndOpenPajekFile(out oGraph) == DialogResult.OK)
        {
            // Import the graph's edges and vertices into the workbook.

            ImportGraph(oGraph, null, null);
        }
    }

    //*************************************************************************
    //  Method: ImportFromGraphMLFile()
    //
    /// <summary>
    /// Imports the contents of a GraphML file into the workbook.
    /// </summary>
    //*************************************************************************

    public void
    ImportFromGraphMLFile()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        // Create a graph from a GraphML file selected by the user.

        IGraph oGraph;
        OpenGraphMLFileDialog oDialog = new OpenGraphMLFileDialog();

        if (oDialog.ShowDialogAndOpenGraphMLFile(out oGraph) ==
            DialogResult.OK)
        {
            // Import the graph's edges and vertices into the workbook.

            ImportGraph(oGraph,

                ( String[] )oGraph.GetRequiredValue(
                    ReservedMetadataKeys.AllEdgeMetadataKeys,
                    typeof( String[] ) ),

                ( String[] )oGraph.GetRequiredValue(
                    ReservedMetadataKeys.AllVertexMetadataKeys,
                    typeof( String[] ) )
                );
        }
    }

    //*************************************************************************
    //  Method: ShowGraphMetrics()
    //
    /// <summary>
    /// Shows the dialog that lists available graph metrics and calculates them
    /// if requested by the user.
    /// </summary>
    ///
    /// <param name="mode">
    /// Indicates the mode in which the GraphMetricsDialog is being used.
    /// </param>
    //*************************************************************************

    public void
    ShowGraphMetrics
    (
        GraphMetricsDialog.DialogMode mode
    )
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        // Create the object that might be edited by the dialog.

        GraphMetricUserSettings oGraphMetricUserSettings =
            new GraphMetricUserSettings();

        GraphMetricsDialog oGraphMetricsDialog = new GraphMetricsDialog(
            this.InnerObject, oGraphMetricUserSettings, mode);

        if (oGraphMetricsDialog.ShowDialog() == DialogResult.OK)
        {
            // Save the edited object.

            oGraphMetricUserSettings.Save();
        }
    }

    //*************************************************************************
    //  Method: GroupByVertexAttribute()
    //
    /// <summary>
    /// Partitions the graph into groups based on the values in a vertex
    /// worksheet column.
    /// </summary>
    //*************************************************************************

    public void
    GroupByVertexAttribute()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        GroupByVertexAttributeDialog oGroupByVertexAttributeDialog =
            new GroupByVertexAttributeDialog(this.InnerObject);

        oGroupByVertexAttributeDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: CalculateClusters()
    //
    /// <summary>
    /// Partitions the graph into clusters.
    /// </summary>
    ///
    /// <param name="clusterAlgorithm">
    /// The cluster algorithm to use.
    /// </param>
    ///
    /// <returns>
    /// true if the graph was successfully partitioned into clusters.
    /// </returns>
    //*************************************************************************

    public Boolean
    CalculateClusters
    (
        ClusterAlgorithm clusterAlgorithm
    )
    {
        AssertValid();

        ClusterCalculator2 oClusterCalculator2 = new ClusterCalculator2();
        oClusterCalculator2.Algorithm = clusterAlgorithm;

        return ( CalculateGroups(oClusterCalculator2, "Finding Clusters") );
    }

    //*************************************************************************
    //  Method: CalculateConnectedComponents()
    //
    /// <summary>
    /// Partitions the graph into connected components.
    /// </summary>
    ///
    /// <returns>
    /// true if the graph was successfully partitioned into connected
    /// components.
    /// </returns>
    //*************************************************************************

    public Boolean
    CalculateConnectedComponents()
    {
        AssertValid();

        return ( CalculateGroups(new ConnectedComponentCalculator2(),
            "Finding Connected Components") );
    }

    //*************************************************************************
    //  Method: RunGroupCommand()
    //
    /// <summary>
    /// Runs a group command.
    /// </summary>
    ///
    /// <param name="groupCommand">
    /// One of the flags in the <see cref="GroupCommands" /> enumeration.
    /// </param>
    //*************************************************************************

    public void
    RunGroupCommand
    (
        GroupCommands groupCommand
    )
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        // Various worksheets must be activated for reading and writing.  Save
        // the active worksheet state.

        ExcelActiveWorksheetRestorer oExcelActiveWorksheetRestorer =
            new ExcelActiveWorksheetRestorer(this.InnerObject);

        ExcelActiveWorksheetState oExcelActiveWorksheetState =
            oExcelActiveWorksheetRestorer.GetActiveWorksheetState();

        try
        {
            if ( !GroupManager.TryRunGroupCommand(groupCommand,
                this.InnerObject, Globals.Sheet2, Globals.Sheet5) )
            {
                return;
            }

            switch (groupCommand)
            {
                case GroupCommands.CollapseSelectedGroups:
                case GroupCommands.ExpandSelectedGroups:

                    // Let the TaskPane know about the collapsed or expanded
                    // groups.

                    FireCollapseOrExpandGroups(
                        groupCommand == GroupCommands.CollapseSelectedGroups,
                        Globals.Sheet5.GetSelectedGroupNames() );

                    break;

                case GroupCommands.CollapseAllGroups:
                case GroupCommands.ExpandAllGroups:

                    ICollection<String> oUniqueGroupNames;

                    if ( ExcelUtil.TryGetUniqueTableColumnStringValues(
                            this.InnerObject, WorksheetNames.Groups,
                            TableNames.Groups, GroupTableColumnNames.Name,
                            out oUniqueGroupNames) )
                    {
                        FireCollapseOrExpandGroups(
                            groupCommand == GroupCommands.CollapseAllGroups,
                            oUniqueGroupNames);
                    }

                    break;

                default:

                    break;
            }
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
        }
        finally
        {
            oExcelActiveWorksheetRestorer.Restore(oExcelActiveWorksheetState);
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
    /// Sets one visual attribute that gets stored in the workbook.
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
    ///
    /// <remarks>
    /// If the specified visual attribute is a graph-level attribute (such as
    /// VisualAttribute.GraphBackground), the visual attribute value gets
    /// stored in the workbook's per-workbook settings.  Otherwise, the visual
    /// attribute value gets stored in the selected rows of the active
    /// worksheet.
    /// </remarks>
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

        if (visualAttribute == VisualAttributes.GraphBackground)
        {
            BackgroundDialog oBackgroundDialog = new BackgroundDialog(
                this.PerWorkbookSettings);

            if (oBackgroundDialog.ShowDialog() == DialogResult.OK)
            {
                FireVisualAttributeSetInWorkbook();
            }
        }
        else
        {
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
    }

    //*************************************************************************
    //  Method: EnableSetVisualAttributes
    //
    /// <summary>
    /// Enables the "set visual attribute" buttons in the Ribbon.
    /// </summary>
    ///
    /// <remarks>
    /// This method tells the Ribbon to enable or disable its visual attribute
    /// buttons when something happens in the workbook that might change their
    /// enabled state.  Note that this information is "pushed" into the Ribbon.
    /// The Ribbon can't pull the information from the workbook when it needs
    /// it because unlike menu items that appear only when a menu is opened,
    /// the visual attribute buttons are visible in the Ribbon at all times.
    /// </remarks>
    //*************************************************************************

    public void
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

            case WorksheetNames.Groups:

                sTableName = TableNames.Groups;
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
            else if (sWorksheetName == WorksheetNames.Groups)
            {
                eVisualAttributes |= VisualAttributes.Color;
                eVisualAttributes |= VisualAttributes.VertexShape;
            }
        }

        this.Ribbon.EnableSetVisualAttributes(eVisualAttributes);
    }

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
    //  Event: VertexAttributesEditedInGraph
    //
    /// <summary>
    /// Occurs when vertex attributes are edited in the NodeXL graph.
    /// </summary>
    //*************************************************************************

    public event VertexAttributesEditedEventHandler
        VertexAttributesEditedInGraph;


    //*************************************************************************
    //  Event: GraphLaidOut
    //
    /// <summary>
    /// Occurs after graph layout completes.
    /// </summary>
    ///
    /// <remarks>
    /// Graph layout occurs asynchronously.  This event fires when the graph
    /// is successfully laid out.
    /// </remarks>
    //*************************************************************************

    public event GraphLaidOutEventHandler GraphLaidOut;


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
    //  Event: SetVisualAttribute2
    //
    /// <summary>
    /// Occurs when one of the visual attribute buttons in the Ribbon is
    /// clicked and the click isn't handled directly by ThisWorkbook.
    /// </summary>
    ///
    /// <remarks>
    /// This is named SetVisualAttribute2 instead of SetVisualAttribute to
    /// avoid a conflict with the SetVisualAttribute() method.
    /// </remarks>
    //*************************************************************************

    public event SetVisualAttributeEventHandler SetVisualAttribute2;


    //*************************************************************************
    //  Event: CollapseOrExpandGroups
    //
    /// <summary>
    /// Occurs when one or more vertex groups should be collapsed or expanded.
    /// </summary>
    //*************************************************************************

    public event CollapseOrExpandGroupsEventHandler CollapseOrExpandGroups;


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

                oTaskPane.VertexAttributesEditedInGraph +=
                    new VertexAttributesEditedEventHandler(
                        this.TaskPane_VertexAttributesEditedInGraph);

                oTaskPane.GraphLaidOut += new GraphLaidOutEventHandler(
                    this.TaskPane_GraphLaidOut);

                oTaskPane.VerticesMoved += new VerticesMovedEventHandler2(
                    this.TaskPane_VerticesMoved);

                Sheet1 oSheet1 = Globals.Sheet1;
                Sheet2 oSheet2 = Globals.Sheet2;
                Sheet5 oSheet5 = Globals.Sheet5;

                m_oSelectionCoordinator = new SelectionCoordinator(this,
                    oSheet1, oSheet1.Edges, oSheet2, oSheet2.Vertices,
                    oSheet5, oSheet5.Groups, Globals.Sheet6, oTaskPane);

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
    //  Method: ExportToFile()
    //
    /// <summary>
    /// Merges duplicate edges and exports the edge and vertex tables to a file
    /// using a provided dialog.
    /// </summary>
    ///
    /// <param name="oReadWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <param name="oSaveGraphFileDialog">
    /// The dialog to use to save the graph.
    /// </param>
    //*************************************************************************

    private void
    ExportToFile
    (
        ReadWorkbookContext oReadWorkbookContext,
        SaveGraphFileDialog oSaveGraphFileDialog
    )
    {
        Debug.Assert(oReadWorkbookContext != null);
        Debug.Assert(oSaveGraphFileDialog != null);
        AssertValid();

        WorkbookReader oWorkbookReader = new WorkbookReader();

        ShowWaitCursor = true;
        this.ScreenUpdating = false;

        try
        {
            ( new DuplicateEdgeMerger() ).MergeDuplicateEdges(
                this.InnerObject);

            this.ScreenUpdating = true;

            // Read the workbook and let the user save it.

            IGraph oGraph = oWorkbookReader.ReadWorkbook(
                this.InnerObject, oReadWorkbookContext);

            oSaveGraphFileDialog.ShowDialogAndSaveGraph(oGraph);
        }
        catch (Exception oException)
        {
            this.ScreenUpdating = true;
            ErrorUtil.OnException(oException);
        }
        finally
        {
            this.ScreenUpdating = true;
            ShowWaitCursor = false;
        }
    }

    //*************************************************************************
    //  Method: ImportGraph()
    //
    /// <summary>
    /// Imports a graph's edges and vertices into the workbook.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to import.
    /// </param>
    ///
    /// <param name="oEdgeAttributes">
    /// Array of edge attribute keys that have been added to the metadata of
    /// the graph's vertices.  Can be null.
    /// </param>
    ///
    /// <param name="oVertexAttributes">
    /// Array of vertex attribute keys that have been added to the metadata of
    /// the graph's vertices.  Can be null.
    /// </param>
    //*************************************************************************

    private void
    ImportGraph
    (
        IGraph oGraph,
        String [] oEdgeAttributes,
        String [] oVertexAttributes
    )
    {
        Debug.Assert(oGraph != null);
        AssertValid();

        GraphImporter oGraphImporter = new GraphImporter();

        this.ScreenUpdating = false;

        try
        {
            oGraphImporter.ImportGraph(oGraph, oEdgeAttributes,
                oVertexAttributes, this.Ribbon.ClearTablesBeforeImport,
                this.InnerObject);

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

                    "The file contains both undirected and directed edges,"
                    + " which {0} does not allow.  All edges are being"
                    + " converted to directed edges."
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
        this.Ribbon.GraphDirectedness = eGraphDirectedness;
    }

    //*************************************************************************
    //  Method: CalculateGroups()
    //
    /// <summary>
    /// Group's the graph's vertices using a graph metric calculator.
    /// </summary>
    ///
    /// <param name="oGraphMetricCalculator">
    /// The IGraphMetricCalculator2 to use.
    /// </param>
    ///
    /// <param name="sDialogTitle">
    /// Title for the CalculateGraphMetricsDialog, or null to use a default
    /// title.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were successfully calculated, or false if
    /// they were cancelled or the application isn't ready.
    /// </returns>
    //*************************************************************************

    private Boolean
    CalculateGroups
    (
        IGraphMetricCalculator2 oGraphMetricCalculator,
        String sDialogTitle
    )
    {
        Debug.Assert(oGraphMetricCalculator != null);
        Debug.Assert( !String.IsNullOrEmpty(sDialogTitle) );
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return (false);
        }

        // The CalculateGraphMetricsDialog does the work.

        CalculateGraphMetricsDialog oCalculateGraphMetricsDialog =
            new CalculateGraphMetricsDialog(
                this.InnerObject,
                new GraphMetricUserSettings(),
                new IGraphMetricCalculator2[] {oGraphMetricCalculator},
                sDialogTitle
                );

        if (oCalculateGraphMetricsDialog.ShowDialog() == DialogResult.OK)
        {
            // Check the "read groups" checkbox in the ribbon.

            this.Ribbon.ReadGroups = true;

            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: MergeIsApproved()
    //
    /// <summary>
    /// Requests approval from the user to merge duplicate edges and perform
    /// other tasks.
    /// </summary>
    ///
    /// <param name="sMessageSuffix">
    /// Suffix to append to base approval message.
    /// </param>
    ///
    /// <returns>
    /// true if the user approved the merge.
    /// </returns>
    //*************************************************************************

    private Boolean
    MergeIsApproved
    (
        String sMessageSuffix
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sMessageSuffix) );
        AssertValid();

        String sMessage =
            "This will remove any filters that are applied to the Edges"
            + " worksheet, merge any duplicate edges, " + sMessageSuffix
            + "\r\n\r\n"
            + "Do you want to continue?"
            ;

        return (MessageBox.Show(sMessage, FormUtil.ApplicationName,
            MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            == DialogResult.Yes);
    }

    //*************************************************************************
    //  Method: OnTasksAutomatedOnOpen()
    //
    /// <summary>
    /// Gets called after task automation has been run on the workbook
    /// immediately after it was opened.
    /// </summary>
    //*************************************************************************

    private void
    OnTasksAutomatedOnOpen()
    {
        AssertValid();

        // Prevent the workbook from being automated the next time it is
        // opened.

        PerWorkbookSettings oPerWorkbookSettings = this.PerWorkbookSettings;
        oPerWorkbookSettings.AutomateTasksOnOpen = false;
        this.Save();

        // The workbook and Excel need to be closed.  If this method is being
        // called from the GraphLaidOut event, that shouldn't be done before
        // the event completes.  Use a timer to delay the closing.
        //
        // The timer interval isn't critical, because Windows gives low
        // priority to timer events in its message queue, allowing the event
        // to complete before the timer is serviced.

        Timer oTimer = new Timer();
        oTimer.Interval = 1;

        oTimer.Tick += delegate(Object sender2, EventArgs e2)
        {
            oTimer.Stop();
            oTimer = null;

            // Unfortunately, calling Application.Quit() directly after closing
            // the workbook doesn't do anything -- an empty Excel window
            // remains.  It does close the application if done in the
            // workbook's Shutdown event, however, so set a flag that the
            // Shutdown event handler will detect.
            //
            // (This could be implemented inline using another delegate, this
            // one on the Shutdown event, but the flag makes things more
            // explicit.)

            m_bCloseExcelWhenWorkbookCloses = true;

            this.Close(false, System.Reflection.Missing.Value,
                System.Reflection.Missing.Value);
        };

        oTimer.Start();
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

        // The TaskPane handles this event by reading the workbook, which
        // clears the selection in the graph.  That fires the TaskPane's
        // SelectionChangedInGraph event, which SelectionCoorindator handles by
        // clearing the selection in the workbook.  That can be jarring to the
        // user, who just made a selection, set a visual attribute on the
        // selection, and then saw the selection disappear.
        //
        // Fix this by temporarily igoring the SelectionChangedInGraph event,
        // which causes the selection in the workbook to be retained.  The
        // selection in the graph (which is empty) will then be out of sync
        // with the selection in the workbook, but I think that is tolerable.

        if (m_oSelectionCoordinator != null)
        {
            m_oSelectionCoordinator.IgnoreSelectionEvents = true;
        }

        EventUtil.FireEvent(this, this.VisualAttributeSetInWorkbook);

        if (m_oSelectionCoordinator != null)
        {
            m_oSelectionCoordinator.IgnoreSelectionEvents = false;
        }
    }

    //*************************************************************************
    //  Method: FireCollapseOrExpandGroups()
    //
    /// <summary>
    /// Fires the <see cref="CollapseOrExpandGroups" /> event if appropriate.
    /// </summary>
    ///
    /// <param name="bCollapse">
    /// true to collapse the groups, false to expand them.
    /// </param>
    ///
    /// <param name="oGroupNames">
    /// Collection of group names, one for each group that needs to be
    /// collapsed or expanded.
    /// </param>
    //*************************************************************************

    private void
    FireCollapseOrExpandGroups
    (
        Boolean bCollapse,
        ICollection<String> oGroupNames
    )
    {
        Debug.Assert(oGroupNames != null);
        AssertValid();

        CollapseOrExpandGroupsEventHandler oCollapseOrExpandGroups =
            this.CollapseOrExpandGroups;

        if (oCollapseOrExpandGroups != null)
        {
            // Let the TaskPane know about the collapsed or expanded groups.

            oCollapseOrExpandGroups( this,
                new CollapseOrExpandGroupsEventArgs(bCollapse, oGroupNames) );
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
        m_bCloseExcelWhenWorkbookCloses = false;

        // If this is an older NodeXL workbook, update the worksheet, table,
        // and column names if neccessary.

        NodeXLWorkbookUpdater.UpdateNames(this.InnerObject);

        Sheet1 oSheet1 = Globals.Sheet1;
        Sheet2 oSheet2 = Globals.Sheet2;
        Sheet5 oSheet5 = Globals.Sheet5;

        m_oWorksheetContextMenuManager = new WorksheetContextMenuManager(
            this, oSheet1, oSheet1.Edges, oSheet2, oSheet2.Vertices, oSheet5,
            oSheet5.Groups);

        // In message boxes, show the name of this document customization
        // instead of the default, which is the name of the Excel application.

        FormUtil.ApplicationName = ApplicationUtil.ApplicationName;

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

        if (m_bCloseExcelWhenWorkbookCloses)
        {
            this.Application.Quit();
        }
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
    //  Method: TaskPane_GraphLaidOut()
    //
    /// <summary>
    /// Handles the GraphLaidOut event on the TaskPane.
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
    /// Graph layout occurs asynchronously.  This event fires when the graph
    /// is successfully laid out.
    /// </remarks>
    //*************************************************************************

    private void
    TaskPane_GraphLaidOut
    (
        Object sender,
        GraphLaidOutEventArgs e
    )
    {
        Debug.Assert(e != null);
        AssertValid();

        if ( !this.ExcelApplicationIsReady(false) )
        {
            return;
        }

        // Forward the event.

        GraphLaidOutEventHandler oGraphLaidOut = this.GraphLaidOut;

        if (oGraphLaidOut != null)
        {
            try
            {
                oGraphLaidOut(this, e);
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
        Debug.Assert(!m_bTaskPaneCreated || m_oSelectionCoordinator != null);
        // m_bCloseExcelWhenWorkbookCloses
    }


    //*************************************************************************
    //  Private fields
    //*************************************************************************

    /// Object that adds custom menu items to the Excel context menus that
    /// appear when the vertex or edge table is right-clicked.

    private WorksheetContextMenuManager m_oWorksheetContextMenuManager;

    /// true if the task pane has been created.

    private Boolean m_bTaskPaneCreated;

    /// If m_bTaskPaneCreated is true, this is the object that coordinates the
    /// edge and vertex selection between the workbook and the TaskPane.  It is
    /// null otherwise.

    private SelectionCoordinator m_oSelectionCoordinator;

    /// true to close Excel when the workbook closes.

    private Boolean m_bCloseExcelWhenWorkbookCloses;
}

}
