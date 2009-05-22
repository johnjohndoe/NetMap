
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Globalization;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: Sheets1And2Helper
//
/// <summary>
/// Helper class used by Sheet1 and Sheet2.
/// </summary>
///
/// <remarks>
/// See the base-class comments for details on how sheet helper classes are
/// used.
/// </remarks>
//*****************************************************************************

public class Sheets1And2Helper : SheetHelper
{
    //*************************************************************************
    //  Constructor: Sheets1And2Helper()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="Sheets1And2Helper" />
    /// class.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// The worksheet that owns this object.
    /// </param>
    ///
    /// <param name="table">
    /// The edge or vertex table.
    /// </param>
    //*************************************************************************

    public Sheets1And2Helper
    (
        Microsoft.Office.Tools.Excel.Worksheet worksheet,
        Microsoft.Office.Tools.Excel.ListObject table
    )
    : base(worksheet, table)
    {
        m_aiRowIDsToSelectOnActivation = null;

        AssertValid();
    }

    //*************************************************************************
    //  Method: Sheet_Startup()
    //
    /// <summary>
    /// Handles the Startup event on the worksheet.
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

    public void
    Sheet_Startup
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        base.Sheet_Startup();

        m_oWorksheet.ActivateEvent += new DocEvents_ActivateEventHandler(
            this.Sheet_ActivateEvent);

        Globals.ThisWorkbook.SelectionChangedInGraph +=
            new SelectionChangedEventHandler(
                this.ThisWorkbook_SelectionChangedInGraph);
    }

    //*************************************************************************
    //  Method: TryGetIDsOfAllRows()
    //
    /// <summary>
    /// Attempts to get a dictionary containing the ID of each row in the
    /// table.
    /// </summary>
    ///
    /// <param name="rowIDDictionary">
    /// Where a dictionary gets stored if true is returned.  There is one
    /// dictionary entry for each row in the table that has an ID.  The key is
    /// the ID stored in the table's ID column and the value is the one-based
    /// row number relative to the worksheet.
    /// </param>
    ///
    /// <returns>
    /// true if the dictionary was obtained, false if there is no ID column.
    /// </returns>
    ///
    /// <remarks>
    /// The returned dictionary includes the IDs of hidden rows.
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryGetIDsOfAllRows
    (
        out Dictionary<Int32, Int32> rowIDDictionary
    )
    {
        AssertValid();

        rowIDDictionary = null;

        if (!TableExists)
        {
            return (false);
        }

        Range oIDColumnData;

        // Get data for the ID column.  This includes hidden rows but excludes
        // the header row.

        if ( !TryGetIDColumnData(out oIDColumnData) )
        {
            return (false);
        }

        Object [,] aoIDValues = ExcelUtil.GetRangeValues(oIDColumnData);

        // Create a dictionary that maps row IDs to row numbers.

        rowIDDictionary = new Dictionary<Int32, Int32>();

        Range oDataBodyRange = m_oTable.DataBodyRange;

        if (oDataBodyRange == null)
        {
            return (false);
        }

        Int32 iDataBodyRangeRow = oDataBodyRange.Row;
        Int32 iRows = oIDColumnData.Rows.Count;

        for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
        {
            Int32 iID;

            if ( !TryGetID(aoIDValues, iRowOneBased, out iID) )
            {
                // The user may have added a row since the workbook was
                // read into the graph.  Ignore this row.

                continue;
            }

            rowIDDictionary[iID] = iRowOneBased + iDataBodyRangeRow - 1;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryGetID()
    //
    /// <summary>
    /// Attempts to get an ID from an ID column.
    /// </summary>
    ///
    /// <param name="IDValues">
    /// Two-dimensional array of ID values read from an ID column.  The array
    /// is one column wide.
    /// </param>
    ///
    /// <param name="rowOneBased">
    /// One-based row number to read.
    /// </param>
    ///
    /// <param name="ID">
    /// Where an ID gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// If the specified cell value contains a valid ID, the ID is stored at
    /// <paramref name="ID" />, and true is returned.  false is returned
    /// otherwise.
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryGetID
    (
        Object [,] IDValues,
        Int32 rowOneBased,
        out Int32 ID
    )
    {
        Debug.Assert(IDValues != null);
        Debug.Assert(rowOneBased >= 1);
        AssertValid();

        ID = Int32.MinValue;

        String sID;

        return (
            ExcelUtil.TryGetNonEmptyStringFromCell(IDValues, rowOneBased, 1,
                out sID)
            &&
            Int32.TryParse(sID, out ID)
            );
    }

    //*************************************************************************
    //  Method: SetVisualAttribute()
    //
    /// <summary>
    /// Sets a visual attribute in the selected rows of the worksheet.
    /// </summary>
    ///
    /// <param name="e">
    /// ThisWorkbook_SetVisualAttribute2 event arguments.
    /// </param>
    ///
    /// <param name="selectedRange">
    /// The selected range in the worksheet.
    /// </param>
    ///
    /// <param name="colorColumnName">
    /// Name of the worksheet's color column, or null if the worksheet doesn't
    /// have a color column.
    /// </param>
    ///
    /// <param name="alphaColumnName">
    /// Name of the worksheet's alpha column, or null if the worksheet doesn't
    /// have an alpha column.
    /// </param>
    ///
    /// <remarks>
    /// If the visual attribute specified by <paramref name="e" /> is handled
    /// by this method or its base-class implementation, the visual attribute
    /// is set in the selected rows of the worksheet and e.VisualAttributeSet
    /// is set to true.
    ///
    /// <para>
    /// The worksheet must be active.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public new void
    SetVisualAttribute
    (
        SetVisualAttributeEventArgs e,
        Range selectedRange,
        String colorColumnName,
        String alphaColumnName
    )
    {
        Debug.Assert(e != null);
        Debug.Assert(selectedRange != null);
        Debug.Assert( ExcelUtil.WorksheetIsActive(m_oWorksheet.InnerObject) );
        AssertValid();

        base.SetVisualAttribute(e, selectedRange, colorColumnName,
            alphaColumnName);

        if (e.VisualAttributeSet)
        {
            return;
        }

        if (e.VisualAttribute == VisualAttributes.Alpha &&
            alphaColumnName != null)
        {
            AlphaDialog oAlphaDialog = new AlphaDialog();

            if (oAlphaDialog.ShowDialog() == DialogResult.OK)
            {
                ExcelUtil.SetVisibleSelectedTableColumnData(
                    m_oTable.InnerObject, selectedRange, alphaColumnName,
                    oAlphaDialog.Alpha);

                e.VisualAttributeSet = true;
            }
        }
    }


    //*************************************************************************
    //  Property: TableIsEdgeTable
    //
    /// <summary>
    /// Gets a flag indicating whether the table is the edge table.
    /// </summary>
    ///
    /// <value>
    /// true if the table is the edge table, false if it is the vertex table.
    /// </value>
    //*************************************************************************

    public Boolean
    TableIsEdgeTable
    {
        get
        {
            AssertValid();

            return (m_oTable.Name == TableNames.Edges);
        }
    }

    //*************************************************************************
    //  Method: SelectTableRows()
    //
    /// <summary>
    /// Selects rows in the table.
    /// </summary>
    ///
    /// <param name="aiRowIDsToSelect">
    /// Array of IDs of rows to select.  Any row not included in the array
    /// gets deselected.
    /// </param>
    ///
    /// <remarks>
    /// This should be called only when the worksheet is active.
    /// </remarks>
    //*************************************************************************

    protected void
    SelectTableRows
    (
        Int32 [] aiRowIDsToSelect
    )
    {
        Debug.Assert(aiRowIDsToSelect != null);
        Debug.Assert( ExcelUtil.WorksheetIsActive(m_oWorksheet.InnerObject) );
        AssertValid();

        if (!TableExists)
        {
            return;
        }

        Int32 iRowIDsToSelect = aiRowIDsToSelect.Length;

        if (iRowIDsToSelect == 0)
        {
            // Unselect any currently selected rows.

            m_oTable.HeaderRowRange.Select();

            return;
        }

        // Get a dictionary containing the ID of each row in the table.  The
        // key is the ID stored in the table's ID column and the value is the
        // one-based row number relative to the worksheet.

        Dictionary<Int32, Int32> oRowIDDictionary;

        if ( !TryGetIDsOfAllRows(out oRowIDDictionary) )
        {
            return;
        }

        // Build a range address string that is the union of the rows to
        // select.  Sample: "3:3,6:6,12:12".  Excel allows this for an address
        // up to MaximumBuiltRangeAddressLength characters.  (This was
        // determined experimentally.)  Building a union via a string address
        // is much more efficient than creating one range per row and using
        // Application.Union() on all of them.

        StringBuilder oBuiltRangeAddress = new StringBuilder();

        const Int32 MaximumBuiltRangeAddressLength = 250;

        Range oAccumulatedRange = null;

        // The ExcelLocale1033(true) attribute in AssemblyInfo.cs is supposed
        // to make the Excel object model act the same in all locales, so a
        // hard-coded comma should always work as the list separator for a
        // union range address.  That's not the case, though; Excel uses the
        // locale-specified list separator instead.  Is this a bug in the Excel
        // PIAs?  Here is a posting from someone else who found the same
        // problem:
        //
        // http://social.msdn.microsoft.com/Forums/en-US/vsto/thread/
        // 0e4bd7dc-37d3-42ea-9ce4-53b9e5a53719/

        String sListSeparator =
            CultureInfo.CurrentCulture.TextInfo.ListSeparator;

        for (Int32 i = 0; i < iRowIDsToSelect; i++)
        {
            Int32 iRowIDToSelect = aiRowIDsToSelect[i];
            Int32 iRow;

            if ( oRowIDDictionary.TryGetValue(iRowIDToSelect, out iRow) )
            {
                if (oBuiltRangeAddress.Length != 0)
                {
                    oBuiltRangeAddress.Append(sListSeparator);
                }

                oBuiltRangeAddress.Append(String.Format(
                    "{0}:{0}",
                    iRow
                    ) );
            }

            // In the following test, assume that the next appended address
            // would be ",1048576:1048576".

            Int32 iBuiltRangeAddressLength = oBuiltRangeAddress.Length;

            if (
                iBuiltRangeAddressLength + 1 + 7 + 1 + 7 >
                    MaximumBuiltRangeAddressLength
                ||
                (i == iRowIDsToSelect - 1 && iBuiltRangeAddressLength > 0)
                )
            {
                // Get the range specified by the StringBuilder.

                Range oBuiltRange = m_oWorksheet.InnerObject.get_Range(
                    oBuiltRangeAddress.ToString(), Missing.Value);

                Debug.Assert(oBuiltRange != null);

                // Add it to the total.

                if ( !ExcelUtil.TryUnionRanges(
                    oAccumulatedRange, oBuiltRange, out oAccumulatedRange) )
                {
                    Debug.Assert(false);
                }

                oBuiltRangeAddress.Length = 0;
            }
        }

        // Intersect the accumulated range with the table and select the
        // intersection.

        Range oRangeToSelect;

        if ( ExcelUtil.TryIntersectRanges(oAccumulatedRange,
            m_oTable.DataBodyRange, out oRangeToSelect) )
        {
            oRangeToSelect.Select();
        }
    }

    //*************************************************************************
    //  Method: TryGetIDColumnData()
    //
    /// <summary>
    /// Attempts to get the ID column data from the table.
    /// </summary>
    ///
    /// <param name="oIDColumnData">
    /// Where the ID column data gets stored if true is returned.  The data
    /// includes only that part of the ID column within the table's data body
    /// range.  This excludes any header or totals row.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetIDColumnData
    (
        out Range oIDColumnData
    )
    {
        AssertValid();

        oIDColumnData = null;

        return ( ExcelUtil.TryGetTableColumnData(m_oTable.InnerObject,
            CommonTableColumnNames.ID, out oIDColumnData) );
    }

    //*************************************************************************
    //  Method: Sheet_ActivateEvent()
    //
    /// <summary>
    /// Handles the ActivateEvent event on the worksheet.
    /// </summary>
    //*************************************************************************

    protected void
    Sheet_ActivateEvent()
    {
        AssertValid();

        if (m_aiRowIDsToSelectOnActivation == null || !TableExists)
        {
            return;
        }

        // ThisWorkbook_SelectionChangedInGraph() defered the selection of
        // table rows until the worksheet was activated.  Select the rows now.

        m_bIgnoreSelectionEvents = true;

        try
        {
            SelectTableRows(m_aiRowIDsToSelectOnActivation);
        }
        catch (Exception oException)
        {
            // If exceptions aren't caught here, Excel consumes them without
            // indicating that anything is wrong.

            ErrorUtil.OnException(oException);
        }
        finally
        {
            m_aiRowIDsToSelectOnActivation = null;
            m_bIgnoreSelectionEvents = false;
        }
    }

    //*************************************************************************
    //  Method: ThisWorkbook_SelectionChangedInGraph()
    //
    /// <summary>
    /// Handles the SelectionChanged event on ThisWorkbook.
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

    protected void
    ThisWorkbook_SelectionChangedInGraph
    (
        Object sender,
        SelectionChangedEventArgs e
    )
    {
        Debug.Assert(e != null);
        AssertValid();

        if (m_bIgnoreSelectionEvents || !TableExists)
        {
            // Prevent an endless loop.

            return;
        }

        // Get the IDs of the table rows to select.

        Int32 [] aiRowIDsToSelect =
            (this.TableIsEdgeTable ? e.SelectedEdgeIDs : e.SelectedVertexIDs);

        if ( !ExcelUtil.WorksheetIsActive(m_oWorksheet.InnerObject) )
        {
            // You can't select rows in a worksheet that isn't active, so store
            // the IDs and let Sheet_ActivateEvent() select them when the sheet
            // is activated.
            //
            // It's possible to avoid deferring the selection by turning off
            // screen updating and temporarily selecting m_oWorksheet.
            // Selecting a worksheet is slow, however, even with screen
            // updating turned off.  It takes about 250ms on a fast machine.
            // That's too slow to keep up with the user if he is scrolling
            // through the table with the down-arrow key, for example.

            m_aiRowIDsToSelectOnActivation = aiRowIDsToSelect;

            return;
        }

        m_bIgnoreSelectionEvents = true;

        try
        {
            SelectTableRows(aiRowIDsToSelect);
        }
        catch (Exception oException)
        {
            // If exceptions aren't caught here, Excel consumes them without
            // indicating that anything is wrong.

            ErrorUtil.OnException(oException);
        }
        finally
        {
            m_bIgnoreSelectionEvents = false;
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        // m_bIgnoreSelectionEvents
        // m_aiRowIDsToSelectOnActivation
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The IDs of the rows that should be selected when the worksheet is
    /// activated, or null if no rows should be selected.

    protected Int32 [] m_aiRowIDsToSelectOnActivation;
}

}
