
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.Office.Core;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: WorksheetContextMenuManager
//
/// <summary>
/// Adds custom menu items to the Excel context menus that appear when the
/// vertex or edge table is clicked in the workbook.
/// </summary>
///
/// <remarks>
/// This class takes care of all of the details involved in adding custom
/// NodeXL menu items to the Excel context menus, including handling and
/// forwarding the events that are fired when the custom menu items are
/// clicked.
///
/// <para>
/// Several events are fired by this class.  <see
/// cref="RequestEdgeCommandEnable" /> or <see
/// cref="RequestVertexCommandEnable" /> occurs before an edge or vertex table
/// context menu is displayed and this class needs to know which menu items to
/// enable.  <see cref="RunEdgeCommand" /> or <see cref="RunVertexCommand" />
/// occurs when the user clicks one of the custom menu items.
/// </para>
/// 
/// </remarks>
//*****************************************************************************

public class WorksheetContextMenuManager : Object
{
    //*************************************************************************
    //  Constructor: WorksheetContextMenuManager()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="WorksheetContextMenuManager" /> class.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Excel workbook.
    /// </param>
    ///
    /// <param name="edgeWorksheet">
    /// The edge worksheet in the Excel workbook.
    /// </param>
    ///
    /// <param name="edgeTable">
    /// The edge table on the edge worksheet.
    /// </param>
    ///
    /// <param name="vertexWorksheet">
    /// The vertex worksheet in the Excel workbook.
    /// </param>
    ///
    /// <param name="vertexTable">
    /// The vertex table on the vertex worksheet.
    /// </param>
    //*************************************************************************

    public WorksheetContextMenuManager
    (
        Microsoft.Office.Tools.Excel.Workbook workbook,
        Microsoft.Office.Tools.Excel.Worksheet edgeWorksheet,
        Microsoft.Office.Tools.Excel.ListObject edgeTable,
        Microsoft.Office.Tools.Excel.Worksheet vertexWorksheet,
        Microsoft.Office.Tools.Excel.ListObject vertexTable
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert(edgeWorksheet != null);
        Debug.Assert(edgeTable != null);
        Debug.Assert(vertexWorksheet != null);
        Debug.Assert(vertexTable != null);

        m_oWorkbook = workbook;
        m_oEdgeTable = edgeTable;
        m_oVertexTable = vertexTable;

        // Handle the events involved in adding, handling, and removing custom
        // menu items.

        workbook.Deactivate += new Microsoft.Office.Interop.
            Excel.WorkbookEvents_DeactivateEventHandler(
                this.Workbook_Deactivate);

        edgeWorksheet.Deactivate += new Microsoft.Office.Interop.
            Excel.DocEvents_DeactivateEventHandler(this.Worksheet_Deactivate);

        vertexWorksheet.Deactivate += new Microsoft.Office.Interop.
            Excel.DocEvents_DeactivateEventHandler(this.Worksheet_Deactivate);

        vertexTable.BeforeRightClick += new Microsoft.Office.Interop.Excel.
            DocEvents_BeforeRightClickEventHandler(
                VertexTable_BeforeRightClick);

        edgeTable.BeforeRightClick += new Microsoft.Office.Interop.Excel.
            DocEvents_BeforeRightClickEventHandler(EdgeTable_BeforeRightClick);
    }

    //*************************************************************************
    //  Enum: VertexCommand
    //
    /// <summary>
    /// Commands that can be run from the context menu that appears when the
    /// vertex table is right-clicked.
    /// </summary>
    //*************************************************************************

    public enum
    VertexCommand
    {
        /// <summary>
        /// Select all vertices.
        /// </summary>

        SelectAllVertices,

        /// <summary>
        /// Deselect all vertices.
        /// </summary>

        DeselectAllVertices,

        /// <summary>
        /// Select the clicked vertex's adjacent vertices.
        /// </summary>

        SelectAdjacentVertices,

        /// <summary>
        /// Deselect the clicked vertex's adjacent vertices.
        /// </summary>

        DeselectAdjacentVertices,

        /// <summary>
        /// Select the clicked vertex's incident edges.
        /// </summary>

        SelectIncidentEdges,

        /// <summary>
        /// Deselect the clicked vertex's incident edges.
        /// </summary>

        DeselectIncidentEdges,

        /// <summary>
        /// Edit the attributes of the selected vertices.
        /// </summary>

        EditVertexAttributes,

        /// <summary>
        /// Shows a dialog for selecting subgraphs.
        /// </summary>

        SelectSubgraphs,
    }

    //*************************************************************************
    //  Enum: EdgeCommand
    //
    /// <summary>
    /// Commands that can be run from the context menu that appears when the
    /// edge table is right-clicked.
    /// </summary>
    //*************************************************************************

    public enum
    EdgeCommand
    {
        /// <summary>
        /// Select all edges.
        /// </summary>

        SelectAllEdges,

        /// <summary>
        /// Deselect all edges.
        /// </summary>

        DeselectAllEdges,

        /// <summary>
        /// Select the clicked edges's adjacent vertices.
        /// </summary>

        SelectAdjacentVertices,

        /// <summary>
        /// Deselect the clicked edges's adjacent vertices.
        /// </summary>

        DeselectAdjacentVertices,
    }

    //*************************************************************************
    //  Event: RequestVertexCommandEnable
    //
    /// <summary>
    /// Occurs before a context menu is displayed for the vertex table and this
    /// class needs to know which custom menu items to enable.
    /// </summary>
    ///
    /// <remarks>
    /// By default, none of the custom menu items that this class adds to the
    /// vertex table's context menu are enabled.  To enable them, handle this
    /// event and selectively set the flags in the event argument to true.
    /// </remarks>
    //*************************************************************************

    public event RequestVertexCommandEnableEventHandler
        RequestVertexCommandEnable;


    //*************************************************************************
    //  Event: RequestEdgeCommandEnable
    //
    /// <summary>
    /// Occurs before a context menu is displayed for the edge table and this
    /// class needs to know which custom menu items to enable.
    /// </summary>
    ///
    /// <remarks>
    /// By default, none of the custom menu items that this class adds to the
    /// edge table's context menu are enabled.  To enable them, handle this
    /// event and selectively set the flags in the event argument to true.
    /// </remarks>
    //*************************************************************************

    public event RequestEdgeCommandEnableEventHandler RequestEdgeCommandEnable;


    //*************************************************************************
    //  Event: RunVertexCommand
    //
    /// <summary>
    /// Occurs when the user clicks a custom menu item that appears when the
    /// vertex table is right-clicked.
    /// </summary>
    //*************************************************************************

    public event RunVertexCommandEventHandler RunVertexCommand;


    //*************************************************************************
    //  Event: RunEdgeCommand
    //
    /// <summary>
    /// Occurs when the user clicks a custom menu item that appears when the
    /// edge table is right-clicked.
    /// </summary>
    //*************************************************************************

    public event RunEdgeCommandEventHandler RunEdgeCommand;


    //*************************************************************************
    //  Method: AddVertexContextMenuItems()
    //
    /// <summary>
    /// Adds custom menu items to the context menu that appears when a cell is
    /// right-clicked in the vertex table.
    /// </summary>
    ///
    /// <param name="oClickedRange">
    /// Range that was right-clicked.
    /// </param>
    //*************************************************************************

    protected void
    AddVertexContextMenuItems
    (
        Microsoft.Office.Interop.Excel.Range oClickedRange
    )
    {
        Debug.Assert(oClickedRange != null);
        AssertValid();

        Int32 iClickedVertexID;
        CommandBarPopup oTopLevelPopup;

        // Add a top-level NodeXL popup menu item.

        PrepareToAddChildMenuItems(oClickedRange, m_oVertexTable,
            CommonTableColumnNames.ID, out iClickedVertexID,
            out oTopLevelPopup);

        // Add child menu items.

        CommandBarButton oSelectAllVerticesButton = AddContextMenuItem(
            oTopLevelPopup, "Select &All Vertices", iClickedVertexID,
            new _CommandBarButtonEvents_ClickEventHandler(
                oSelectAllVerticesButton_Click) );

        CommandBarButton oDeselectAllVerticesButton = AddContextMenuItem(
            oTopLevelPopup, "&Deselect All Vertices", iClickedVertexID,
            new _CommandBarButtonEvents_ClickEventHandler(
                oDeselectAllVerticesButton_Click) );

        CommandBarButton oSelectAdjacentVerticesOfVertexButton =
            AddContextMenuItem(oTopLevelPopup, "Select Ad&jacent Vertices",
            iClickedVertexID,
            new _CommandBarButtonEvents_ClickEventHandler(
                oSelectAdjacentVerticesOfVertexButton_Click) );

        CommandBarButton oDeselectAdjacentVerticesOfVertexButton =
            AddContextMenuItem(oTopLevelPopup, "Deselect Adjace&nt Vertices",
            iClickedVertexID,
            new _CommandBarButtonEvents_ClickEventHandler(
                oDeselectAdjacentVerticesOfVertexButton_Click) );

        CommandBarButton oSelectIncidentEdgesButton = AddContextMenuItem(
            oTopLevelPopup, "Select &Incident Edges", iClickedVertexID,
            new _CommandBarButtonEvents_ClickEventHandler(
                oSelectIncidentEdgesButton_Click) );

        CommandBarButton oDeselectIncidentEdgesButton = AddContextMenuItem(
            oTopLevelPopup, "Deselec&t Incident Edges", iClickedVertexID,
            new _CommandBarButtonEvents_ClickEventHandler(
                oDeselectIncidentEdgesButton_Click) );

        CommandBarButton oSelectSubgraphsButton = AddContextMenuItem(
            oTopLevelPopup, "Select S&ubgraphs...", iClickedVertexID,
            new _CommandBarButtonEvents_ClickEventHandler(
                oSelectSubgraphsButton_Click) );

        CommandBarButton oEditVertexAttributesButton = AddContextMenuItem(
            oTopLevelPopup, "&Edit Selected Vertex Attributes...",
            iClickedVertexID,
            new _CommandBarButtonEvents_ClickEventHandler(
                oEditVertexAttributesButton_Click) );

        oEditVertexAttributesButton.BeginGroup = true;

        // The custom menu items are disabled by default.  To enable them, the
        // RequestVertexCommandEnable event must be handled.

        RequestVertexCommandEnableEventHandler oRequestVertexCommandEnable =
            this.RequestVertexCommandEnable;

        if (oRequestVertexCommandEnable == null)
        {
            return;
        }

        RequestVertexCommandEnableEventArgs oEventArgs =
            new RequestVertexCommandEnableEventArgs(iClickedVertexID);

        oRequestVertexCommandEnable(this, oEventArgs);

        oSelectAllVerticesButton.Enabled = oEventArgs.EnableSelectAllVertices;

        oDeselectAllVerticesButton.Enabled =
            oEventArgs.EnableDeselectAllVertices;

        oSelectAdjacentVerticesOfVertexButton.Enabled =
            oEventArgs.EnableSelectAdjacentVertices;

        oDeselectAdjacentVerticesOfVertexButton.Enabled =
            oEventArgs.EnableDeselectAdjacentVertices;

        oSelectIncidentEdgesButton.Enabled =
            oEventArgs.EnableSelectIncidentEdges;

        oDeselectIncidentEdgesButton.Enabled =
            oEventArgs.EnableDeselectIncidentEdges;

        oEditVertexAttributesButton.Enabled =
            oEventArgs.EnableEditVertexAttributes;

        oSelectSubgraphsButton.Enabled = oEventArgs.EnableSelectSubgraphs;
    }

    //*************************************************************************
    //  Method: AddEdgeContextMenuItems()
    //
    /// <summary>
    /// Adds custom menu items to the context menu that appears when a cell is
    /// right-clicked in the edge table.
    /// </summary>
    ///
    /// <param name="oClickedRange">
    /// Range that was right-clicked.
    /// </param>
    //*************************************************************************

    protected void
    AddEdgeContextMenuItems
    (
        Microsoft.Office.Interop.Excel.Range oClickedRange
    )
    {
        Debug.Assert(oClickedRange != null);
        AssertValid();

        Int32 iClickedEdgeID;
        CommandBarPopup oTopLevelPopup;

        // Add a top-level NodeXL popup menu item.

        PrepareToAddChildMenuItems(oClickedRange, m_oEdgeTable,
            CommonTableColumnNames.ID, out iClickedEdgeID,
            out oTopLevelPopup);

        // Add child menu items.

        CommandBarButton oSelectAllEdgesButton = AddContextMenuItem(
            oTopLevelPopup, "Select &All Edges", iClickedEdgeID,
            new _CommandBarButtonEvents_ClickEventHandler(
                oSelectAllEdgesButton_Click) );

        CommandBarButton oDeselectAllEdgesButton = AddContextMenuItem(
            oTopLevelPopup, "&Deselect All Edges", iClickedEdgeID,
            new _CommandBarButtonEvents_ClickEventHandler(
                oDeselectAllEdgesButton_Click) );

        CommandBarButton oSelectAdjacentVerticesOfEdgeButton =
            AddContextMenuItem(oTopLevelPopup, "Select Ad&jacent Vertices",
            iClickedEdgeID,
            new _CommandBarButtonEvents_ClickEventHandler(
                oSelectAdjacentVerticesOfEdgeButton_Click) );

        CommandBarButton oDeselectAdjacentVerticesOfEdgeButton =
            AddContextMenuItem(oTopLevelPopup, "Deselect Adjace&nt Vertices",
            iClickedEdgeID,
            new _CommandBarButtonEvents_ClickEventHandler(
                oDeselectAdjacentVerticesOfEdgeButton_Click) );

        // The custom menu items are disabled by default.  To enable them, the
        // RequestEdgeCommandEnable event must be handled.

        RequestEdgeCommandEnableEventHandler oRequestEdgeCommandEnable =
            this.RequestEdgeCommandEnable;

        if (oRequestEdgeCommandEnable == null)
        {
            return;
        }

        RequestEdgeCommandEnableEventArgs oEventArgs =
            new RequestEdgeCommandEnableEventArgs(iClickedEdgeID);

        oRequestEdgeCommandEnable(this, oEventArgs);

        oSelectAllEdgesButton.Enabled = oEventArgs.EnableSelectAllEdges;

        oDeselectAllEdgesButton.Enabled = oEventArgs.EnableDeselectAllEdges;

        oSelectAdjacentVerticesOfEdgeButton.Enabled =
            oEventArgs.EnableSelectAdjacentVertices;

        oDeselectAdjacentVerticesOfEdgeButton.Enabled =
            oEventArgs.EnableDeselectAdjacentVertices;
    }

    //*************************************************************************
    //  Method: PrepareToAddChildMenuItems()
    //
    /// <summary>
    /// Performs tasks required before custom menu items can be added to the
    /// context menu that appears when a cell is right-clicked in the edge or
    /// vertex table.
    /// </summary>
    ///
    /// <param name="oClickedRange">
    /// Range that was right-clicked.
    /// </param>
    ///
    /// <param name="oTable">
    /// Table that was right-clicked.
    /// </param>
    ///
    /// <param name="sIDColumnName">
    /// Name of the table's ID column.
    /// </param>
    ///
    /// <param name="iClickedID">
    /// Where the ID of the edge or vertex that was right-clicked gets stored.
    /// Can be NoID.
    /// </param>
    ///
    /// <param name="oTopLevelPopup">
    /// Where the added top-level NodeXL popup menu item gets stored.
    /// </param>
    //*************************************************************************

    protected void
    PrepareToAddChildMenuItems
    (
        Microsoft.Office.Interop.Excel.Range oClickedRange,
        Microsoft.Office.Tools.Excel.ListObject oTable,
        String sIDColumnName,
        out Int32 iClickedID,
        out CommandBarPopup oTopLevelPopup
    )
    {
        Debug.Assert(oClickedRange != null);
        Debug.Assert(oTable != null);
        Debug.Assert( !String.IsNullOrEmpty(sIDColumnName) );
        AssertValid();

        iClickedID = NoID;

        // Start with a clean slate.

        RemoveContextMenuItems();

        // Was a single row selected?

        if ( !TryGetClickedID(oClickedRange, oTable.InnerObject, sIDColumnName,
            out iClickedID) )
        {
            // No.

            iClickedID = NoID;
        }

        // Get the context menu.

        CommandBar oTableContextCommandBar =
            ExcelUtil.GetTableContextCommandBar(m_oWorkbook.Application);

        // Add a separator at the top of the menu.

        AddContextMenuSeparator(oTableContextCommandBar, true);

        // Add a top-level NodeXL popup menu item.

        oTopLevelPopup = AddTopLevelPopup(oTableContextCommandBar);
    }

    //*************************************************************************
    //  Method: AddContextMenuItem()
    //
    /// <summary>
    /// Adds a custom menu item to a parent CommandBarPopup.
    /// </summary>
    ///
    /// <param name="oCommandBarPopup">
    /// Parent CommandBarPopup to add the custom menu item to.
    /// </param>
    ///
    /// <param name="sText">
    /// Menu item text.
    /// </param>
    ///
    /// <param name="iClickedID">
    /// ID of the single edge or vertex that was selected and right-clicked in
    /// the edge or vertex table.  Can be NoID.
    /// </param>
    ///
    /// <param name="oEventHandler">
    /// The menu item's event handler.
    /// </param>
    //*************************************************************************

    protected CommandBarButton
    AddContextMenuItem
    (
        CommandBarPopup oCommandBarPopup,
        String sText,
        Int32 iClickedID,
        _CommandBarButtonEvents_ClickEventHandler oEventHandler
    )
    {
        Debug.Assert(oCommandBarPopup != null);
        Debug.Assert( !String.IsNullOrEmpty(sText) );
        Debug.Assert(oEventHandler != null);
        AssertValid();

        // Important Note:
        //
        // Do not use CommandBarButton.Tag to store the clicked ID.  There is
        // a bug in VSTO 2008 that causes all CommandBarButtons with the same
        // Tag to fire their Click events when one such CommandBarButton is
        // clicked.  Here is an online posting discussing this:
        //
        // http://excelusergroup.org/forums/p/700/2153.aspx
        //
        // Instead of the Tag, use the CommandBarButton.Parameter property.

        CommandBarButton oCommandBarButton =
            (CommandBarButton)oCommandBarPopup.Controls.Add(
                MsoControlType.msoControlButton, 1, iClickedID.ToString(),
                Missing.Value, true);

        oCommandBarButton.Caption = sText;
        oCommandBarButton.Style = MsoButtonStyle.msoButtonCaption;
        oCommandBarButton.Enabled = false;
        oCommandBarButton.Click += oEventHandler;

        return (oCommandBarButton);
    }

    //*************************************************************************
    //  Method: AddContextMenuSeparator()
    //
    /// <summary>
    /// Adds or removes a separator at the top of the context menu that appears
    /// when a cell is right-clicked in the edge or vertex table.
    /// </summary>
    ///
    /// <param name="oTableContextCommandBar">
    /// The context menu that appears when you right-click a table (ListObject)
    /// cell.
    /// </param>
    ///
    /// <param name="bAdd">
    /// true to add the separator, false to remove it.
    /// </param>
    //*************************************************************************

    protected void
    AddContextMenuSeparator
    (
        CommandBar oTableContextCommandBar,
        Boolean bAdd
    )
    {
        Debug.Assert(oTableContextCommandBar != null);
        AssertValid();

        // The button at the top of Excel's standard menu is Cut.

        const Int32 CutCommandID = 21;

        CommandBarButton oCutButton =
            (CommandBarButton)oTableContextCommandBar.FindControl(
                Missing.Value, CutCommandID, Missing.Value, Missing.Value,
                false);

        if (oCutButton != null)
        {
            oCutButton.BeginGroup = bAdd;
        }
    }

    //*************************************************************************
    //  Method: AddTopLevelPopup()
    //
    /// <summary>
    /// Adds a top-level NodeXL popup menu item to the context menu that
    /// appears when a cell is right-clicked in the edge or vertex table.
    /// </summary>
    ///
    /// <param name="oTableContextCommandBar">
    /// The context menu that appears when you right-click a table (ListObject)
    /// cell.
    /// </param>
    ///
    /// <returns>
    /// The new NodeXL top-level popup menu item.
    /// </returns>
    //*************************************************************************

    protected CommandBarPopup
    AddTopLevelPopup
    (
        CommandBar oTableContextCommandBar
    )
    {
        Debug.Assert(oTableContextCommandBar != null);
        AssertValid();

        CommandBarPopup oTopLevelPopup = (CommandBarPopup)
            oTableContextCommandBar.Controls.Add(
                MsoControlType.msoControlPopup, 1, Missing.Value, 1, true);

        oTopLevelPopup.Caption = TopLevelMenuCaption;

        return (oTopLevelPopup);
    }

    //*************************************************************************
    //  Method: RemoveContextMenuItems()
    //
    /// <summary>
    /// Removes the custom menu items added by <see
    /// cref="AddEdgeContextMenuItems" /> and <see
    /// cref="AddVertexContextMenuItems" />.
    /// </summary>
    ///
    /// <remarks>
    /// If the custom menu items don't exist, this method does nothing.
    /// </remarks>
    //*************************************************************************

    protected void
    RemoveContextMenuItems()
    {
        AssertValid();

        CommandBar oTableContextCommandBar =
            ExcelUtil.GetTableContextCommandBar(m_oWorkbook.Application);

        // Remove the separator at the top of the menu.

        AddContextMenuSeparator(oTableContextCommandBar, false);

        try
        {
            // Delete the top-level NodeXL popup menu item, which also deletes
            // its child menu items.

            oTableContextCommandBar.Controls[TopLevelMenuCaption].Delete(false);
        }
        catch (ArgumentException)
        {
        }
    }

    //*************************************************************************
    //  Method: TryGetClickedID()
    //
    /// <summary>
    /// Attempts to get the ID of the edge or vertex that was right-clicked in
    /// the edge or vertex table.
    /// </summary>
    ///
    /// <param name="oClickedRange">
    /// Range that was right-clicked.
    /// </param>
    ///
    /// <param name="oTable">
    /// Table containing the range.
    /// </param>
    ///
    /// <param name="sIDColumnName">
    /// Name of the ID column in the table.
    /// </param>
    ///
    /// <param name="iClickedID">
    /// Where the ID gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if an edge or vertex was right-clicked and its ID was obtained.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetClickedID
    (
        Microsoft.Office.Interop.Excel.Range oClickedRange,
        Microsoft.Office.Interop.Excel.ListObject oTable,
        String sIDColumnName,
        out Int32 iClickedID
    )
    {
        Debug.Assert(oClickedRange != null);
        AssertValid();

        iClickedID = Int32.MinValue;

        // Note: Don't use oClickedRange to get the clicked cell.  This works
        // if only one cell is selected, but if the selection is larger than
        // one cell, oClickedRange is the entire selection.
        //
        // Instead, get the clicked cell via the Window.RangeFromPoint()
        // method.

        Point oClickedPoint = Control.MousePosition;

        Microsoft.Office.Interop.Excel.Range oClickedCell =
            (Microsoft.Office.Interop.Excel.Range)
            oClickedRange.Application.ActiveWindow.RangeFromPoint(
                oClickedPoint.X, oClickedPoint.Y);

        if (oClickedCell == null)
        {
            return (false);
        }

        // Attempt to get the table's optional ID column.

        Microsoft.Office.Interop.Excel.ListColumn oIDColumn;

        if ( !ExcelUtil.TryGetTableColumn(oTable, sIDColumnName,
            out oIDColumn) )
        {
            return (false);
        }

        // Attempt to get an ID from the clicked row.

        Debug.Assert(oClickedRange.Parent is
            Microsoft.Office.Interop.Excel.Worksheet);

        Microsoft.Office.Interop.Excel.Worksheet oWorksheet =
            (Microsoft.Office.Interop.Excel.Worksheet)oClickedRange.Parent;

        Microsoft.Office.Interop.Excel.Range oIDRange = 
            (Microsoft.Office.Interop.Excel.Range)oWorksheet.Cells[
            oClickedCell.Row, oIDColumn.Range.Column];

        String sID;

        if ( !ExcelUtil.TryGetNonEmptyStringFromCell(
            ExcelUtil.GetRangeValues(oIDRange), 1, 1, out sID)
            ||
            !Int32.TryParse(sID, out iClickedID) )
        {
            return (false);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: RunAVertexCommand()
    //
    /// <summary>
    /// Runs a vertex command.
    /// </summary>
    ///
    /// <param name="oCommandBarButton">
    /// CommandBarButton whose Parameter property contains a vertex ID.
    /// </param>
    ///
    /// <param name="eVertexCommand">
    /// Vertex command to run.
    /// </param>
    //*************************************************************************

    protected void
    RunAVertexCommand
    (
        CommandBarButton oCommandBarButton,
        VertexCommand eVertexCommand
    )
    {
        Debug.Assert(oCommandBarButton != null);
        AssertValid();

        RunVertexCommandEventHandler oRunVertexCommand = this.RunVertexCommand;

        if (oRunVertexCommand == null)
        {
            // There is no event handler, so the command can't be run.

            return;
        }

        // Retrieve the ID of the vertex that was right-clicked.  This can be
        // NoID.

        Int32 iClickedVertexID =
            CommandBarButtonToClickedID(oCommandBarButton);

        oRunVertexCommand( this,
            new RunVertexCommandEventArgs(iClickedVertexID, eVertexCommand) );
    }

    //*************************************************************************
    //  Method: RunAnEdgeCommand()
    //
    /// <summary>
    /// Runs an edge command.
    /// </summary>
    ///
    /// <param name="oCommandBarButton">
    /// CommandBarButton whose Parameter property contains an edge ID.
    /// </param>
    ///
    /// <param name="eEdgeCommand">
    /// Edge command to run.
    /// </param>
    //*************************************************************************

    protected void
    RunAnEdgeCommand
    (
        CommandBarButton oCommandBarButton,
        EdgeCommand eEdgeCommand
    )
    {
        Debug.Assert(oCommandBarButton != null);
        AssertValid();

        RunEdgeCommandEventHandler oRunEdgeCommand = this.RunEdgeCommand;

        if (oRunEdgeCommand == null)
        {
            // There is no event handler, so the command can't be run.

            return;
        }

        // Retrieve the ID of the edge that was right-clicked.  This can be
        // NoID.

        Int32 iClickedEdgeID = CommandBarButtonToClickedID(oCommandBarButton);

        oRunEdgeCommand( this,
            new RunEdgeCommandEventArgs(iClickedEdgeID, eEdgeCommand) );
    }

    //*************************************************************************
    //  Method: CommandBarButtonToClickedID()
    //
    /// <summary>
    /// Retrieves the edge or vertex ID stored in a CommandBarButton's
    /// Parameter property.
    /// </summary>
    ///
    /// <param name="oCommandBarButton">
    /// CommandBarButton whose Parameter property contains an ID.
    /// </param>
    ///
    /// <returns>
    /// The retreived ID, or NoID if a single edge or vertex wasn't
    /// right-clicked.
    /// </returns>
    //*************************************************************************

    protected Int32
    CommandBarButtonToClickedID
    (
        CommandBarButton oCommandBarButton
    )
    {
        Debug.Assert(oCommandBarButton != null);
        AssertValid();

        Int32 iClickedID;

        if ( !Int32.TryParse( oCommandBarButton.Parameter, out iClickedID) )
        {
            Debug.Assert(false);
        }

        return (iClickedID);
    }

    //*************************************************************************
    //  Method: VertexTable_BeforeRightClick()
    //
    /// <summary>
    /// Handles the BeforeRightClick event on the vertex table.
    /// </summary>
    ///
    /// <param name="Target">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="Cancel">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    VertexTable_BeforeRightClick
    (
        Microsoft.Office.Interop.Excel.Range Target,
        ref bool Cancel
    )
    {
        AssertValid();

        try
        {
            AddVertexContextMenuItems(Target);
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
        }
    }

    //*************************************************************************
    //  Method: EdgeTable_BeforeRightClick()
    //
    /// <summary>
    /// Handles the BeforeRightClick event on the edge table.
    /// </summary>
    ///
    /// <param name="Target">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="Cancel">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    EdgeTable_BeforeRightClick
    (
        Microsoft.Office.Interop.Excel.Range Target,
        ref bool Cancel
    )
    {
        AssertValid();

        try
        {
            AddEdgeContextMenuItems(Target);
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
        }
    }

    //*************************************************************************
    //  Method: Worksheet_Deactivate()
    //
    /// <summary>
    /// Handles the Deactivate event on the edge and vertex worksheets.
    /// </summary>
    //*************************************************************************

    protected void
    Worksheet_Deactivate()
    {
        AssertValid();

        try
        {
            RemoveContextMenuItems();
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
        }
    }

    //*************************************************************************
    //  Method: Workbook_Deactivate()
    //
    /// <summary>
    /// Handles the Deactivate event on the workbook.
    /// </summary>
    //*************************************************************************

    protected void
    Workbook_Deactivate()
    {
        AssertValid();

        try
        {
            RemoveContextMenuItems();
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
        }
    }

    //*************************************************************************
    //  Method: oSelectAllVerticesButton_Click()
    //
    /// <summary>
    /// Handles the Click event on the oSelectAllVerticesButton
    /// CommandBarButton.
    /// </summary>
    ///
    /// <param name="Ctrl">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="CancelDefault">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    oSelectAllVerticesButton_Click
    (
        CommandBarButton Ctrl,
        ref bool CancelDefault
    )
    {
        AssertValid();

        RunAVertexCommand(Ctrl, VertexCommand.SelectAllVertices);
    }

    //*************************************************************************
    //  Method: oDeselectAllVerticesButton_Click()
    //
    /// <summary>
    /// Handles the Click event on the oDeselectAllVerticesButton
    /// CommandBarButton.
    /// </summary>
    ///
    /// <param name="Ctrl">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="CancelDefault">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    oDeselectAllVerticesButton_Click
    (
        CommandBarButton Ctrl,
        ref bool CancelDefault
    )
    {
        AssertValid();

        RunAVertexCommand(Ctrl, VertexCommand.DeselectAllVertices);
    }

    //*************************************************************************
    //  Method: oSelectAdjacentVerticesOfVertexButton_Click()
    //
    /// <summary>
    /// Handles the Click event on the oSelectAdjacentVerticesOfVertexButton
    /// CommandBarButton.
    /// </summary>
    ///
    /// <param name="Ctrl">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="CancelDefault">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    oSelectAdjacentVerticesOfVertexButton_Click
    (
        CommandBarButton Ctrl,
        ref bool CancelDefault
    )
    {
        AssertValid();

        RunAVertexCommand(Ctrl, VertexCommand.SelectAdjacentVertices);
    }

    //*************************************************************************
    //  Method: oDeselectAdjacentVerticesOfVertexButton_Click()
    //
    /// <summary>
    /// Handles the Click event on the oDeselectAdjacentVerticesOfVertexButton
    /// CommandBarButton.
    /// </summary>
    ///
    /// <param name="Ctrl">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="CancelDefault">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    oDeselectAdjacentVerticesOfVertexButton_Click
    (
        CommandBarButton Ctrl,
        ref bool CancelDefault
    )
    {
        AssertValid();

        RunAVertexCommand(Ctrl, VertexCommand.DeselectAdjacentVertices);
    }

    //*************************************************************************
    //  Method: oSelectIncidentEdgesButton_Click()
    //
    /// <summary>
    /// Handles the Click event on the oSelectIncidentEdgesButton
    /// CommandBarButton.
    /// </summary>
    ///
    /// <param name="Ctrl">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="CancelDefault">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    oSelectIncidentEdgesButton_Click
    (
        CommandBarButton Ctrl,
        ref bool CancelDefault
    )
    {
        AssertValid();

        RunAVertexCommand(Ctrl, VertexCommand.SelectIncidentEdges);
    }

    //*************************************************************************
    //  Method: oDeselectIncidentEdgesButton_Click()
    //
    /// <summary>
    /// Handles the Click event on the oDeselectIncidentEdgesButton
    /// CommandBarButton.
    /// </summary>
    ///
    /// <param name="Ctrl">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="CancelDefault">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    oDeselectIncidentEdgesButton_Click
    (
        CommandBarButton Ctrl,
        ref bool CancelDefault
    )
    {
        AssertValid();

        RunAVertexCommand(Ctrl, VertexCommand.DeselectIncidentEdges);
    }

    //*************************************************************************
    //  Method: oEditVertexAttributesButton_Click()
    //
    /// <summary>
    /// Handles the Click event on the oEditVertexAttributesButton
    /// CommandBarButton.
    /// </summary>
    ///
    /// <param name="Ctrl">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="CancelDefault">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    oEditVertexAttributesButton_Click
    (
        CommandBarButton Ctrl,
        ref bool CancelDefault
    )
    {
        AssertValid();

        RunAVertexCommand(Ctrl, VertexCommand.EditVertexAttributes);
    }

    //*************************************************************************
    //  Method: oSelectSubgraphsButton_Click()
    //
    /// <summary>
    /// Handles the Click event on the oSelectSubgraphsButton CommandBarButton.
    /// </summary>
    ///
    /// <param name="Ctrl">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="CancelDefault">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    oSelectSubgraphsButton_Click
    (
        CommandBarButton Ctrl,
        ref bool CancelDefault
    )
    {
        AssertValid();

        RunAVertexCommand(Ctrl, VertexCommand.SelectSubgraphs);
    }

    //*************************************************************************
    //  Method: oSelectAllEdgesButton_Click()
    //
    /// <summary>
    /// Handles the Click event on the oSelectAllEdgesButton CommandBarButton.
    /// </summary>
    ///
    /// <param name="Ctrl">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="CancelDefault">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    oSelectAllEdgesButton_Click
    (
        CommandBarButton Ctrl,
        ref bool CancelDefault
    )
    {
        AssertValid();

        RunAnEdgeCommand(Ctrl, EdgeCommand.SelectAllEdges);
    }

    //*************************************************************************
    //  Method: oDeselectAllEdgesButton_Click()
    //
    /// <summary>
    /// Handles the Click event on the oDeselectAllEdgesButton
    /// CommandBarButton.
    /// </summary>
    ///
    /// <param name="Ctrl">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="CancelDefault">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    oDeselectAllEdgesButton_Click
    (
        CommandBarButton Ctrl,
        ref bool CancelDefault
    )
    {
        AssertValid();

        RunAnEdgeCommand(Ctrl, EdgeCommand.DeselectAllEdges);
    }

    //*************************************************************************
    //  Method: oSelectAdjacentVerticesOfEdgeButton_Click()
    //
    /// <summary>
    /// Handles the Click event on the oSelectAdjacentVerticesOfEdgeButton
    /// CommandBarButton.
    /// </summary>
    ///
    /// <param name="Ctrl">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="CancelDefault">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    oSelectAdjacentVerticesOfEdgeButton_Click
    (
        CommandBarButton Ctrl,
        ref bool CancelDefault
    )
    {
        AssertValid();

        RunAnEdgeCommand(Ctrl, EdgeCommand.SelectAdjacentVertices);
    }

    //*************************************************************************
    //  Method: oDeselectAdjacentVerticesOfEdgeButton_Click()
    //
    /// <summary>
    /// Handles the Click event on the oDeselectAdjacentVerticesOfEdgeButton
    /// CommandBarButton.
    /// </summary>
    ///
    /// <param name="Ctrl">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="CancelDefault">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    oDeselectAdjacentVerticesOfEdgeButton_Click
    (
        CommandBarButton Ctrl,
        ref bool CancelDefault
    )
    {
        AssertValid();

        RunAnEdgeCommand(Ctrl, EdgeCommand.DeselectAdjacentVertices);
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
        Debug.Assert(m_oWorkbook != null);
        Debug.Assert(m_oEdgeTable != null);
        Debug.Assert(m_oVertexTable != null);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// Indicates that an edge or vertex ID isn't available.
    /// </summary>

    public static readonly Int32 NoID = Int32.MinValue;


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Caption of the top-level menu added to context menus.

    protected const String TopLevelMenuCaption = ".Net&Map";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Excel workbook.

    protected Microsoft.Office.Tools.Excel.Workbook m_oWorkbook;

    /// The edge table on the edge worksheet.

    protected Microsoft.Office.Tools.Excel.ListObject m_oEdgeTable;

    /// The vertex table on the vertex worksheet.

    protected Microsoft.Office.Tools.Excel.ListObject m_oVertexTable;
}

}
