
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: SheetHelper
//
/// <summary>
/// Helper class used by several sheet classes.
/// </summary>
///
/// <remarks>
/// Several sheet classes (such as Sheet1 and Sheet2) implement similar
/// functionality.  To avoid duplicate code, the ideal solution would be to
/// have the sheet classes derive from a base class that implements the common
/// code.  The Visual Studio designer makes this difficult, if not impossible,
/// however, because the base class for each sheet
/// (Microsoft.Office.Tools.Excel.Worksheet) is specified in the
/// SheetX.Designer.cs file, and that file is dynamically generated.
/// 
/// <para>
/// As a workaround, the common code is implemented in this helper class, an
/// instance of which is owned by each sheet.  The sheets delegate some of
/// their method calls to the methods in this class.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class SheetHelper : Object
{
    //*************************************************************************
    //  Constructor: SheetHelper()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="SheetHelper" /> class.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// The worksheet that owns this object.
    /// </param>
    ///
    /// <param name="table">
    /// The NodeXL table in the worksheet.
    /// </param>
    //*************************************************************************

    public SheetHelper
    (
        Microsoft.Office.Tools.Excel.Worksheet worksheet,
        Microsoft.Office.Tools.Excel.ListObject table
    )
    {
        m_bIgnoreSelectionEvents = false;
        m_oWorksheet = worksheet;
        m_oTable = table;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: TableExists
    //
    /// <summary>
    /// Gets a flag indicating whether the NodeXL table still exists.
    /// </summary>
    ///
    /// <value>
    /// true if the NodeXL table still exists.
    /// </value>
    ///
    /// <remarks>
    /// The user can delete the worksheet after the workbook has been read.  If
    /// he has done so, this property returns false.
    /// </remarks>
    //*************************************************************************

    public Boolean
    TableExists
    {
        get
        {
            AssertValid();

            try
            {
                // Most of the ListObject properties and methods throw an
                // exception once the ListObject has been deleted.
                // ListObject.Name is one of them.

                String sName = m_oTable.Name;

                return (true);
            }
            catch (COMException)
            {
                return (false);
            }
        }
    }

    //*************************************************************************
    //  Method: Sheet_Startup()
    //
    /// <summary>
    /// Handles the Startup event on the worksheet.
    /// </summary>
    //*************************************************************************

    public void
    Sheet_Startup()
    {
        AssertValid();

        m_oTable.SelectionChange += new DocEvents_SelectionChangeEventHandler(
            Table_SelectionChange);

        m_oTable.Deselected += new DocEvents_SelectionChangeEventHandler(
            Table_Deselected);
    }

    //*************************************************************************
    //  Event: TableSelectionChanged
    //
    /// <summary>
    /// Occurs when the selection state of the table changes.
    /// </summary>
    //*************************************************************************

    public event TableSelectionChangedEventHandler TableSelectionChanged;


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

    public void
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

        if (e.VisualAttribute == VisualAttributes.Color &&
            colorColumnName != null)
        {
            String sColor;

            // Get a color from the user.

            if ( NodeXLWorkbookUtil.TryGetColor(out sColor) )
            {
                ExcelUtil.SetVisibleSelectedTableColumnData(
                    m_oTable.InnerObject, selectedRange, colorColumnName,
                    sColor);

                e.VisualAttributeSet = true;
            }
        }
    }

    //*************************************************************************
    //  Method: TryGetSelectedRange()
    //
    /// <summary>
    /// Gets the selected range in the worksheet.
    /// </summary>
    ///
    /// <param name="selectedRange">
    /// Where the selected range in the worksheet gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if the worksheet is active and the selected range was obtained.
    /// </returns>
    ///
    /// <remarks>
    /// If the worksheet is active and it contains a selected range, that range
    /// gets stored at <paramref name="selectedRange" /> and true is returned.
    /// false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryGetSelectedRange
    (
        out Range selectedRange
    )
    {
        AssertValid();

        selectedRange = null;

        if (!ExcelUtil.WorksheetIsActive(m_oWorksheet.InnerObject) ||
            !TableExists)
        {
            return (false);
        }

        Object oSelection = m_oWorksheet.Application.Selection;

        if ( oSelection == null || !(oSelection is Range) )
        {
            return (false);
        }

        selectedRange = (Range)oSelection;

        return (true);
    }

    //*************************************************************************
    //  Method: GetSelectedRowIDs()
    //
    /// <summary>
    /// Gets an array of IDs for all rows in the table that have at least one
    /// cell selected.
    /// </summary>
    ///
    /// <param name="oSelectedRange">
    /// Range that contains the selected cells in the table.  The range may
    /// contain multiple areas.
    /// </param>
    ///
    /// <returns>
    /// An array of unique IDs.  The IDs are the values stored in the table's
    /// ID column and are different from the IVertex.ID and IEdge.ID values in
    /// the graph.
    /// </returns>
    //*************************************************************************

    protected Int32 []
    GetSelectedRowIDs
    (
        Range oSelectedRange
    )
    {
        Debug.Assert(oSelectedRange != null);
        AssertValid();

        // Get the IDs as strings.

        Dictionary<String, Char> oSelectedValues =
            GetSelectedColumnValues(oSelectedRange, CommonTableColumnNames.ID);

        // Transfer all numbers to an array.

        List<Int32> oSelectedRowIDs = new List<Int32>();

        foreach (String sSelectedValue in oSelectedValues.Keys)
        {
            Int32 iSelectedRowID;

            if ( Int32.TryParse(sSelectedValue, out iSelectedRowID) )
            {
                oSelectedRowIDs.Add(iSelectedRowID);
            }
        }

        return ( oSelectedRowIDs.ToArray() );
    }

    //*************************************************************************
    //  Method: GetSelectedColumnValues()
    //
    /// <summary>
    /// Gets a dictionary of values from one column for all rows in the table
    /// that have at least one selected cell.
    /// </summary>
    ///
    /// <param name="selectedRange">
    /// Range that contains the selected cells in the table.  The range may
    /// contain multiple areas.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to get the values from.
    /// </param>
    ///
    /// <returns>
    /// A dictionary of zero or more cell values.  The dictionary can be empty
    /// but is never null.  The dictionary keys are the cell values, which are
    /// guaranteed to be non-null, non-empty, and unique.  The dictionary
    /// values aren't used.
    /// </returns>
    ///
    /// <remarks>
    /// For each row in the table that has at least one selected cell, this
    /// method reads the string from the row's <paramref name="columnName" />
    /// cell and stores it in the returned dictionary.
    /// </remarks>
    //*************************************************************************

    public Dictionary<String, Char>
    GetSelectedColumnValues
    (
        Range selectedRange,
        String columnName
    )
    {
        Debug.Assert(selectedRange != null);
        Debug.Assert( !String.IsNullOrEmpty(columnName) );
        AssertValid();

        // Create a dictionary for the selected values.  The dictionary key is
        // the cell value and the dictionary value is not used.

        Dictionary<String, Char> oSelectedValues =
            new Dictionary<String, Char>();

        if (!this.TableExists)
        {
            goto Done;
        }

        // The selected range can extend outside the table.  Get the
        // intersection of the table with the selection.

        Range oSelectedTableRange;

        if ( !ExcelUtil.TryGetSelectedTableRange(m_oTable.InnerObject,
            selectedRange, out oSelectedTableRange) )
        {
            goto Done;
        }

        Range oDataBodyRange = m_oTable.DataBodyRange;

        Debug.Assert(oDataBodyRange != null);

        // Get data for the specified column.  This includes hidden rows but
        // excludes the header row.

        Range oColumnData;

        if ( !ExcelUtil.TryGetTableColumnData(m_oTable.InnerObject, columnName,
            out oColumnData) )
        {
            goto Done;
        }

        Int32 iRows = oDataBodyRange.Rows.Count;

        // Read the column.

        Object [,] aoValues = ExcelUtil.GetRangeValues(oColumnData);

        foreach (Range oSelectedTableRangeArea in oSelectedTableRange.Areas)
        {
            Int32 iFirstRowOneBased =
                oSelectedTableRangeArea.Row - oDataBodyRange.Row + 1;

            Int32 iLastRowOneBased =
                iFirstRowOneBased + oSelectedTableRangeArea.Rows.Count - 1;

            for (Int32 iRowOneBased = iFirstRowOneBased;
                iRowOneBased <= iLastRowOneBased; iRowOneBased++)
            {
                String sValue;

                if ( ExcelUtil.TryGetNonEmptyStringFromCell(aoValues,
                    iRowOneBased, 1, out sValue) )
                {
                    // There may be two or more areas that include the same
                    // row, so use Dictionary.Item and not Dictionary.Add().

                    oSelectedValues[sValue] = ' ';
                }
            }
        }

        Done:

        return (oSelectedValues);
    }

    //*************************************************************************
    //  Method: OnSelectionChangedInTable()
    //
    /// <summary>
    /// Handles the SelectionChange event on the table.
    /// </summary>
    ///
    /// <param name="Target">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    OnSelectionChangedInTable
    (
        Range Target
    )
    {
        AssertValid();

        if (!TableExists)
        {
            return;
        }

        TableSelectionChangedEventHandler oTableSelectionChanged =
            this.TableSelectionChanged;

        if (oTableSelectionChanged == null)
        {
            // No one is handling the event, so there is nothing to do.

            return;
        }

        // Get an array of unique row IDs for all rows that have at least one
        // cell selected.

        Int32 [] aiSelectedRowIDs = GetSelectedRowIDs(Target);

        // Forward the event.

        FireTableSelectionChanged(aiSelectedRowIDs,
            TableSelectionChangedEventOrigin.SelectionChangedInTable);
    }

    //*************************************************************************
    //  Method: FireTableSelectionChanged()
    //
    /// <summary>
    /// Fires the <see cref="TableSelectionChanged" /> event if appropriate.
    /// </summary>
    ///
    /// <param name="aiSelectedIDs">
    /// Array of IDs of selected rows.
    /// </param>
    ///
    /// <param name="eEventOrigin">
    /// Specifies how the event originated.
    /// </param>
    //*************************************************************************

    protected void
    FireTableSelectionChanged
    (
        Int32 [] aiSelectedIDs,
        TableSelectionChangedEventOrigin eEventOrigin
    )
    {
        AssertValid();

        TableSelectionChangedEventHandler oTableSelectionChanged =
            this.TableSelectionChanged;

        if (oTableSelectionChanged != null)
        {
            try
            {
                oTableSelectionChanged( this,
                    new TableSelectionChangedEventArgs(
                        aiSelectedIDs, eEventOrigin)
                    );
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
    //  Method: Table_SelectionChange()
    //
    /// <summary>
    /// Handles the SelectionChange event on the table.
    /// </summary>
    ///
    /// <param name="Target">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    Table_SelectionChange
    (
        Range Target
    )
    {
        AssertValid();

        if (ExcelUtil.SpecialCellsBeingCalled)
        {
            // Work around an Excel bug.  See the
            // ExcelUtil.SpecialCellsBeingCalled property for details.

            return;
        }

        if (m_bIgnoreSelectionEvents)
        {
            // Prevent an endless loop.

            return;
        }

        m_bIgnoreSelectionEvents = true;

        try
        {
            OnSelectionChangedInTable(Target);
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
    //  Method: Table_Deselected()
    //
    /// <summary>
    /// Handles the Deselected event on the table.
    /// </summary>
    ///
    /// <param name="Target">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    Table_Deselected
    (
        Range Target
    )
    {
        AssertValid();

        if (m_bIgnoreSelectionEvents)
        {
            // Prevent an endless loop.

            return;
        }

        // When the user clicks outside of the table,
        // ListObject.SelectionChange does not fire.  That's why Deselected
        // must be handled.

        // Let the event handler know that there are no table rows selected.

        m_bIgnoreSelectionEvents = true;

        try
        {
            FireTableSelectionChanged(WorksheetReaderBase.EmptyIDArray,
                TableSelectionChangedEventOrigin.SelectionChangedInTable);
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

    [Conditional("DEBUG")]

    public virtual void
    AssertValid()
    {
        // m_bIgnoreSelectionEvents
        Debug.Assert(m_oWorksheet != null);
        Debug.Assert(m_oTable != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// true if selection events should be ignored.

    protected Boolean m_bIgnoreSelectionEvents;

    /// The worksheet that owns this object.

    protected Microsoft.Office.Tools.Excel.Worksheet m_oWorksheet;

    /// The NodeXL table in the worksheet.

    protected Microsoft.Office.Tools.Excel.ListObject m_oTable;
}

}
