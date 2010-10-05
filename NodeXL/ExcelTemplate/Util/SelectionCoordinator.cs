
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: SelectionCoordinator
//
/// <summary>
/// Coordinates the edge and vertex selection between the workbook and the
/// TaskPane.
/// </summary>
///
/// <remarks>
/// This class is responsible for selecting edges and vertices in the TaskPane
/// when those edges and vertices are selected in the workbook, and vice versa.
/// </remarks>
//*****************************************************************************

public class SelectionCoordinator : Object
{
    //*************************************************************************
    //  Constructor: SelectionCoordinator()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="SelectionCoordinator" />
    /// class.
    /// </summary>
    ///
    /// <param name="thisWorkbook">
    /// The Excel workbook.
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
    ///
    /// <param name="groupWorksheet">
    /// The group worksheet in the Excel workbook.
    /// </param>
    ///
    /// <param name="groupTable">
    /// The group table on the group worksheet.
    /// </param>
    ///
    /// <param name="groupVertexWorksheet">
    /// The group-vertex worksheet in the Excel workbook.
    /// </param>
    ///
    /// <param name="taskPane">
    /// The TaskPane.
    /// </param>
    //*************************************************************************

    public SelectionCoordinator
    (
        ThisWorkbook thisWorkbook,
        Sheet1 edgeWorksheet,
        Microsoft.Office.Tools.Excel.ListObject edgeTable,
        Sheet2 vertexWorksheet,
        Microsoft.Office.Tools.Excel.ListObject vertexTable,
        Sheet5 groupWorksheet,
        Microsoft.Office.Tools.Excel.ListObject groupTable,
        Sheet6 groupVertexWorksheet,
        TaskPane taskPane
    )
    {
        Debug.Assert(thisWorkbook != null);
        Debug.Assert(edgeWorksheet != null);
        Debug.Assert(edgeTable != null);
        Debug.Assert(vertexWorksheet != null);
        Debug.Assert(vertexTable != null);
        Debug.Assert(groupWorksheet != null);
        Debug.Assert(groupTable != null);
        Debug.Assert(groupVertexWorksheet != null);
        Debug.Assert(taskPane != null);

        m_oThisWorkbook = thisWorkbook;
        m_oEdgeWorksheet = edgeWorksheet;
        m_oVertexWorksheet = vertexWorksheet;
        m_oGroupWorksheet = groupWorksheet;
        m_oGroupTable = groupTable;
        m_oGroupVertexWorksheet = groupVertexWorksheet;
        m_oTaskPane = taskPane;

        m_bIgnoreSelectionEvents = false;
        m_bUpdateVertexSelectionOnActivation = false;
        m_bUpdateEdgeSelectionOnActivation = false;
        m_bUpdateGroupSelectionOnActivation = false;


        edgeTable.SelectionChange += new DocEvents_SelectionChangeEventHandler(
            EdgeTable_SelectionChange);

        edgeTable.Deselected += new DocEvents_SelectionChangeEventHandler(
            EdgeTable_Deselected);

        m_oEdgeWorksheet.ActivateEvent += new DocEvents_ActivateEventHandler(
            EdgeWorksheet_ActivateEvent);


        vertexTable.SelectionChange +=
            new DocEvents_SelectionChangeEventHandler(
                VertexTable_SelectionChange);

        vertexTable.Deselected += new DocEvents_SelectionChangeEventHandler(
            VertexTable_Deselected);

        m_oVertexWorksheet.ActivateEvent += new DocEvents_ActivateEventHandler(
            VertexWorksheet_ActivateEvent);


        m_oGroupTable.SelectionChange +=
            new DocEvents_SelectionChangeEventHandler(
                GroupTable_SelectionChange);

        m_oGroupTable.Deselected += new DocEvents_SelectionChangeEventHandler(
            GroupTable_Deselected);

        m_oGroupWorksheet.ActivateEvent += new DocEvents_ActivateEventHandler(
            GroupWorksheet_ActivateEvent);


        m_oTaskPane.SelectionChangedInGraph +=
            new EventHandler(this.TaskPane_SelectionChangedInGraph);
    }

    //*************************************************************************
    //  Property: IgnoreSelectionEvents
    //
    /// <summary>
    /// Sets a flag specifying whether selection events should be ignored.
    /// </summary>
    ///
    /// <value>
    /// true if selection events should be ignored.
    /// </value>
    //*************************************************************************

    public Boolean
    IgnoreSelectionEvents
    {
        set
        {
            m_bIgnoreSelectionEvents = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: OnEdgeOrVertexTableSelectionChange()
    //
    /// <summary>
    /// Handles the SelectionChange event on the edge or vertex table.
    /// </summary>
    ///
    /// <param name="bChangeInEdgeTable">
    /// true if the selection changed in the edge table, false if it changed in
    /// the vertex table.
    /// </param>
    //*************************************************************************

    protected void
    OnEdgeOrVertexTableSelectionChange
    (
        Boolean bChangeInEdgeTable
    )
    {
        AssertValid();

        Sheets1And2Helper oSheets1And2Helper = bChangeInEdgeTable ?
            m_oEdgeWorksheet.Sheets1And2Helper :
            m_oVertexWorksheet.Sheets1And2Helper;

        if ( IgnoreTableSelectionChange(oSheets1And2Helper) )
        {
            return;
        }

        m_bIgnoreSelectionEvents = true;

        try
        {
            // (The following comments are for the bChangeInEdgeTable=true
            // case.

            // Get an array of unique row IDs for all edge rows that have at
            // least one cell selected.

            ICollection<Int32> oSelectedRowIDs =
                oSheets1And2Helper.GetSelectedRowIDs();

            // Select those edges (and possibly their adjacent vertices) in the
            // TaskPane.  This will cause the TaskPane.SelectionChangedInGraph
            // event to fire, but this class will ignore it because
            // m_bIgnoreSelectionEvents is currently set to true.

            if (bChangeInEdgeTable)
            {
                m_oTaskPane.SetSelectedEdgesByRowID(oSelectedRowIDs);
            }
            else
            {
                m_oTaskPane.SetSelectedVerticesByRowID(oSelectedRowIDs);
            }

            // The selection in the vertex table may now be out of sync with
            // the TaskPane.  Ideally, the vertex table would be updated right
            // now.  However, you can't select rows in a worksheet that isn't
            // active.  As a workaround, set a flag that will cause the
            // selection in the vertex table to be updated the next time the
            // vertex worksheet is activated.
            //
            // It would be possible to avoid deferring the selection by turning
            // off screen updating and temporarily selecting the vertex
            // worksheet.  Selecting a worksheet is slow, however, even with
            // screen updating turned off.  It takes about 250ms on a fast
            // machine.  That's too slow to keep up with the user if he is
            // scrolling through a table with the down-arrow key, for example.

            if (bChangeInEdgeTable)
            {
                m_bUpdateVertexSelectionOnActivation = true;
            }
            else
            {
                m_bUpdateEdgeSelectionOnActivation = true;
            }

            // Selecting an edge or vertex invalidates any selected groups.

            m_bUpdateGroupSelectionOnActivation = true;

            // Enable the "set visual attribute" buttons in the Ribbon.

            m_oThisWorkbook.EnableSetVisualAttributes();
        }
        catch (COMException)
        {
            // A user reported a bug in which Application.Intersect() throws a
            // COMException with an HRESULT of 0x800AC472.
            // (Application.Intersect() gets called indirectly by
            // OnSelectionChangedInTable().)  The bug occurred while switching
            // windows.  I couldn't reproduce the bug, but the following post
            // suggests that the HRESULT, which is VBA_E_IGNORE, occurs when an
            // object model call is made while the object model is "suspended."
            //
            // http://social.msdn.microsoft.com/forums/en-US/vsto/thread/
            // 9168f9f2-e5bc-4535-8d7d-4e374ab8ff09/
            //
            // Other posts also mention that it can occur during window
            // switches.
            //
            // I can't reproduce the bug and I'm not sure of the root cause,
            // but catching and ignoring the error should lead to nothing worse
            // than a mouse click being ignored.
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
        }
        finally
        {
            m_bIgnoreSelectionEvents = false;
        }
    }

    //*************************************************************************
    //  Method: OnEdgeOrVertexWorksheetActivated()
    //
    /// <summary>
    /// Handles the ActivateEvent event on the edge or vertex worksheet.
    /// </summary>
    ///
    /// <param name="bEdgeWorksheetActivated">
    /// true if the edge worksheet was activated, false if the vertex worksheet
    /// was activated.
    /// </param>
    //*************************************************************************

    protected void
    OnEdgeOrVertexWorksheetActivated
    (
        Boolean bEdgeWorksheetActivated
    )
    {
        AssertValid();

        if (bEdgeWorksheetActivated)
        {
            if (m_bUpdateEdgeSelectionOnActivation)
            {
                m_bUpdateEdgeSelectionOnActivation = false;
            }
            else
            {
                return;
            }
        }
        else
        {
            if (m_bUpdateVertexSelectionOnActivation)
            {
                m_bUpdateVertexSelectionOnActivation = false;
            }
            else
            {
                return;
            }
        }

        SelectEdgeOrVertexTableRows(bEdgeWorksheetActivated);
    }

    //*************************************************************************
    //  Method: OnGroupTableSelectionChange()
    //
    /// <summary>
    /// Handles the SelectionChange event on the group table.
    /// </summary>
    //*************************************************************************

    protected void
    OnGroupTableSelectionChange()
    {
        AssertValid();

        SheetHelper oSheetHelper = m_oGroupWorksheet.SheetHelper;

        if ( IgnoreTableSelectionChange(oSheetHelper) )
        {
            return;
        }

        // Enable the "set visual attribute" buttons in the Ribbon.

        m_oThisWorkbook.EnableSetVisualAttributes();

        LinkedList<Int32> oVertexRowIDsToSelect = new LinkedList<Int32>();

        LinkedList<String> oCollapsedGroupNamesToSelect =
            new LinkedList<String>();

        foreach (String sSelectedGroupName in
            oSheetHelper.GetSelectedStringColumnValues(
                GroupTableColumnNames.Name) )
        {
            if (m_oTaskPane.IsCollapsedGroup(sSelectedGroupName) )
            {
                oCollapsedGroupNamesToSelect.AddLast(sSelectedGroupName);
            }
            else
            {
                foreach ( Int32 iVertexIDInGroup in
                    NodeXLWorkbookUtil.GetVertexIDsInGroup(
                        m_oThisWorkbook.InnerObject, sSelectedGroupName) )
                {
                    oVertexRowIDsToSelect.AddLast(iVertexIDInGroup);
                }
            }
        }

        m_bIgnoreSelectionEvents = true;

        // Select the vertices in the graph, then defer the selection of the
        // corresponding rows in the edge and vertex worksheets until those
        // worksheets are activated.

        m_oTaskPane.SetSelectedVerticesByRowID(oVertexRowIDsToSelect);

        m_bUpdateEdgeSelectionOnActivation = true;
        m_bUpdateVertexSelectionOnActivation = true;

        // Select the vertices that represent collapsed groups.  This has to be
        // done after selecting the other vertices, because selecting the other
        // vertices clears the selection.

        foreach (String sCollapsedSelectedGroupName in
            oCollapsedGroupNamesToSelect)
        {
            m_oTaskPane.SelectCollapsedGroup(sCollapsedSelectedGroupName);
        }

        m_bIgnoreSelectionEvents = false;
    }

    //*************************************************************************
    //  Method: OnGroupWorksheetActivated()
    //
    /// <summary>
    /// Handles the ActivateEvent event on the group worksheet.
    /// </summary>
    //*************************************************************************

    protected void
    OnGroupWorksheetActivated()
    {
        AssertValid();

        if (m_bUpdateGroupSelectionOnActivation)
        {
            SelectGroupTableRows();
            m_bUpdateGroupSelectionOnActivation = false;
        }
    }

    //*************************************************************************
    //  Method: OnSelectionChangedInGraphEdgeOrVertex()
    //
    /// <summary>
    /// Handles the SelectionChangedInGraph event on the TaskPane for the edge
    /// or vertex worksheet.
    /// </summary>
    ///
    /// <param name="bProcessEdgeWorksheet">
    /// true to process the event for the edge worksheet, false to process it
    /// for the vertex worksheet.
    /// </param>
    ///
    /// <remarks>
    /// This method gets called twice: once for the edge worksheet, and again
    /// for the vertex worksheet.
    /// </remarks>
    //*************************************************************************

    protected void
    OnSelectionChangedInGraphEdgeOrVertex
    (
        Boolean bProcessEdgeWorksheet
    )
    {
        AssertValid();

        if ( ExcelUtil.WorksheetIsActive(bProcessEdgeWorksheet ?
            m_oEdgeWorksheet.InnerObject : m_oVertexWorksheet.InnerObject) )
        {
            SelectEdgeOrVertexTableRows(bProcessEdgeWorksheet);
        }
        else
        {
            // Defer the selection of the edges or vertices in the edge or
            // vertex table until the edge or vertex worksheet is activated.

            if (bProcessEdgeWorksheet)
            {
                m_bUpdateEdgeSelectionOnActivation = true;
            }
            else
            {
                m_bUpdateVertexSelectionOnActivation = true;
            }
        }
    }

    //*************************************************************************
    //  Method: OnSelectionChangedInGraphGroup()
    //
    /// <summary>
    /// Handles the SelectionChangedInGraph event on the TaskPane for the group
    /// worksheet.
    /// </summary>
    //*************************************************************************

    protected void
    OnSelectionChangedInGraphGroup()
    {
        AssertValid();

        if ( ExcelUtil.WorksheetIsActive(m_oGroupWorksheet.InnerObject) )
        {
            SelectGroupTableRows();
        }
        else
        {
            // Defer the selection of the groups the group table until the
            // group worksheet is activated.

            m_bUpdateGroupSelectionOnActivation = true;
        }
    }

    //*************************************************************************
    //  Method: SelectEdgeOrVertexTableRows()
    //
    /// <summary>
    /// Select the edges or vertices in the edge or vertex table.
    /// </summary>
    ///
    /// <param name="bSelectEdgeTableRows">
    /// true to select the edges in the edge table, false to select the
    /// vertices in the vertex table.
    /// </param>
    ///
    /// <remarks>
    /// The edge or vertex worksheet is activated if it isn't already
    /// activated.
    /// </remarks>
    //*************************************************************************

    protected void
    SelectEdgeOrVertexTableRows
    (
        Boolean bSelectEdgeTableRows
    )
    {
        AssertValid();

        Sheets1And2Helper oSheets1And2Helper = bSelectEdgeTableRows ?
            m_oEdgeWorksheet.Sheets1And2Helper :
            m_oVertexWorksheet.Sheets1And2Helper;

        if (!oSheets1And2Helper.TableExists)
        {
            return;
        }

        m_bIgnoreSelectionEvents = true;

        try
        {
            oSheets1And2Helper.SelectTableRowsByRowIDs( bSelectEdgeTableRows ?
                m_oTaskPane.GetSelectedEdgeRowIDs() :
                m_oTaskPane.GetSelectedVertexRowIDs()
                );
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
        }
        finally
        {
            m_bIgnoreSelectionEvents = false;
        }
    }

    //*************************************************************************
    //  Method: SelectGroupTableRows()
    //
    /// <summary>
    /// Clears the selection in the group table.
    /// </summary>
    //*************************************************************************

    protected void
    SelectGroupTableRows()
    {
        AssertValid();

        SheetHelper oSheetHelper = m_oGroupWorksheet.SheetHelper;

        if (!oSheetHelper.TableExists)
        {
            return;
        }

        m_bIgnoreSelectionEvents = true;

        try
        {
            oSheetHelper.SelectTableRowsByColumnValues<String>(
                GroupTableColumnNames.Name,
                m_oTaskPane.GetSelectedCollapsedGroupNames(),
                ExcelUtil.TryGetNonEmptyStringFromCell
                );
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
        }
        finally
        {
            m_bIgnoreSelectionEvents = false;
        }
    }

    //*************************************************************************
    //  Method: IgnoreTableSelectionChange()
    //
    /// <summary>
    /// Gets a flag indicating whether the SelectionChange event on a table 
    /// should be ignored.
    /// </summary>
    ///
    /// <param name="oSheetHelper">
    /// SheetHelper object for the worksheet that contains the table.
    /// </param>
    ///
    /// <returns>
    /// true if the SelectionChange event should be ignored.
    /// </returns>
    //*************************************************************************

    protected Boolean
    IgnoreTableSelectionChange
    (
        SheetHelper oSheetHelper
    )
    {
        Debug.Assert(oSheetHelper != null);
        AssertValid();

        return (
            // Work around an Excel bug.  See the
            // ExcelUtil.SpecialCellsBeingCalled property for details.

            ExcelUtil.SpecialCellsBeingCalled
            ||
            m_bIgnoreSelectionEvents

            ||
            !oSheetHelper.TableExists
            );
    }

    //*************************************************************************
    //  Method: EdgeTable_SelectionChange()
    //
    /// <summary>
    /// Handles the SelectionChange event on the edge table.
    /// </summary>
    ///
    /// <param name="Target">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    EdgeTable_SelectionChange
    (
        Range Target
    )
    {
        AssertValid();

        OnEdgeOrVertexTableSelectionChange(true);
    }

    //*************************************************************************
    //  Method: EdgeTable_Deselected()
    //
    /// <summary>
    /// Handles the Deselected event on the edge table.
    /// </summary>
    ///
    /// <param name="Target">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    EdgeTable_Deselected
    (
        Range Target
    )
    {
        AssertValid();

        // When the user clicks outside of the table,
        // ListObject.SelectionChange does not fire.  That's why Deselected
        // must be handled.

        OnEdgeOrVertexTableSelectionChange(true);
    }

    //*************************************************************************
    //  Method: EdgeWorksheet_ActivateEvent()
    //
    /// <summary>
    /// Handles the ActivateEvent event on the edge worksheet.
    /// </summary>
    //*************************************************************************

    protected void
    EdgeWorksheet_ActivateEvent()
    {
        AssertValid();

        OnEdgeOrVertexWorksheetActivated(true);
    }

    //*************************************************************************
    //  Method: VertexTable_SelectionChange()
    //
    /// <summary>
    /// Handles the SelectionChange event on the vertex table.
    /// </summary>
    ///
    /// <param name="Target">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    VertexTable_SelectionChange
    (
        Range Target
    )
    {
        AssertValid();

        OnEdgeOrVertexTableSelectionChange(false);
    }

    //*************************************************************************
    //  Method: VertexTable_Deselected()
    //
    /// <summary>
    /// Handles the Deselected event on the vertex table.
    /// </summary>
    ///
    /// <param name="Target">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    VertexTable_Deselected
    (
        Range Target
    )
    {
        AssertValid();

        // When the user clicks outside of the table,
        // ListObject.SelectionChange does not fire.  That's why Deselected
        // must be handled.

        OnEdgeOrVertexTableSelectionChange(false);
    }

    //*************************************************************************
    //  Method: VertexWorksheet_ActivateEvent()
    //
    /// <summary>
    /// Handles the ActivateEvent event on the vertex worksheet.
    /// </summary>
    //*************************************************************************

    protected void
    VertexWorksheet_ActivateEvent()
    {
        AssertValid();

        OnEdgeOrVertexWorksheetActivated(false);
    }

    //*************************************************************************
    //  Method: GroupTable_SelectionChange()
    //
    /// <summary>
    /// Handles the SelectionChange event on the group table.
    /// </summary>
    ///
    /// <param name="Target">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    GroupTable_SelectionChange
    (
        Range Target
    )
    {
        AssertValid();

        OnGroupTableSelectionChange();
    }

    //*************************************************************************
    //  Method: GroupTable_Deselected()
    //
    /// <summary>
    /// Handles the Deselected event on the group table.
    /// </summary>
    ///
    /// <param name="Target">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    GroupTable_Deselected
    (
        Range Target
    )
    {
        AssertValid();

        // When the user clicks outside of the table,
        // ListObject.SelectionChange does not fire.  That's why Deselected
        // must be handled.

        OnGroupTableSelectionChange();
    }

    //*************************************************************************
    //  Method: GroupWorksheet_ActivateEvent()
    //
    /// <summary>
    /// Handles the ActivateEvent event on the group worksheet.
    /// </summary>
    //*************************************************************************

    protected void
    GroupWorksheet_ActivateEvent()
    {
        AssertValid();

        OnGroupWorksheetActivated();
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
    //*************************************************************************

    protected void
    TaskPane_SelectionChangedInGraph
    (
        Object sender,
        EventArgs e
    )
    {
        Debug.Assert(e != null);
        AssertValid();

        if (m_bIgnoreSelectionEvents)
        {
            return;
        }

        OnSelectionChangedInGraphEdgeOrVertex(true);
        OnSelectionChangedInGraphEdgeOrVertex(false);
        OnSelectionChangedInGraphGroup();
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
        Debug.Assert(m_oThisWorkbook != null);
        Debug.Assert(m_oEdgeWorksheet != null);
        Debug.Assert(m_oVertexWorksheet != null);
        Debug.Assert(m_oGroupWorksheet != null);
        Debug.Assert(m_oGroupTable != null);
        Debug.Assert(m_oGroupVertexWorksheet != null);
        Debug.Assert(m_oTaskPane != null);

        // m_bIgnoreSelectionEvents
        // m_bUpdateVertexSelectionOnActivation
        // m_bUpdateEdgeSelectionOnActivation
        // m_bUpdateGroupSelectionOnActivation
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The workbook.

    protected ThisWorkbook m_oThisWorkbook;

    /// The edge worksheet.

    protected Sheet1 m_oEdgeWorksheet;

    /// The vertex worksheet.

    protected Sheet2 m_oVertexWorksheet;

    /// The group worksheet.

    protected Sheet5 m_oGroupWorksheet;

    /// The group table on the group worksheet.

    protected Microsoft.Office.Tools.Excel.ListObject m_oGroupTable;

    /// The group-vertex worksheet.

    protected Sheet6 m_oGroupVertexWorksheet;

    /// The TaskPane.

    protected TaskPane m_oTaskPane;

    /// true if selection events should be ignored.

    protected Boolean m_bIgnoreSelectionEvents;

    /// true if the selection in the vertex table should be updated the next
    /// time the vertex worksheet is activated.

    protected Boolean m_bUpdateVertexSelectionOnActivation;

    /// true if the selection in the edge table should be updated the next time
    /// the edge worksheet is activated.

    protected Boolean m_bUpdateEdgeSelectionOnActivation;

    /// true if the selection in the group table should be updated the next
    /// time the group worksheet is activated.

    protected Boolean m_bUpdateGroupSelectionOnActivation;
}

}
