
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;
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
    //  Method: TryGetValuesInAllRows()
    //
    /// <summary>
    /// Attempts to get a dictionary containing a column value from each row in
    /// the table.
    /// </summary>
    ///
    /// <typeparam name="TValue">
    /// Type of the column values to read.
    /// </typeparam>
    ///
    /// <param name="sColumnName">
    /// Name of the column to read the values from.
    /// </param>
    ///
    /// <param name="oTryGetValueFromCell">
    /// Method that attempts to get a value of a specified type from a
    /// worksheet cell given an array of cell values read from the worksheet.
    /// </param>
    ///
    /// <param name="oValueDictionary">
    /// Where a dictionary gets stored if true is returned.  There is one
    /// dictionary entry for each row in the table that has a value in the
    /// specified column.  The dictionary key is the cell value and the
    /// dictionary value is the one-based row number relative to the worksheet.
    /// </param>
    ///
    /// <returns>
    /// true if the dictionary was obtained, false if there is no such column.
    /// </returns>
    ///
    /// <remarks>
    /// The returned dictionary includes the values in hidden rows.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryGetValuesInAllRows<TValue>
    (
        String sColumnName,
        ExcelUtil.TryGetValueFromCell<TValue> oTryGetValueFromCell,
        out Dictionary<TValue, Int32> oValueDictionary
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );
        Debug.Assert(oTryGetValueFromCell != null);
        AssertValid();

        oValueDictionary = null;

        if (!TableExists)
        {
            return (false);
        }

        Range oDataBodyRange = m_oTable.DataBodyRange;

        if (oDataBodyRange == null)
        {
            return (false);
        }

        Range oColumnData;
        Object [,] aoColumnValues;

        // Get the values in the column.  This includes hidden rows but
        // excludes the header row.

        if ( !ExcelUtil.TryGetTableColumnDataAndValues(m_oTable.InnerObject,
            sColumnName, out oColumnData, out aoColumnValues) )
        {
            return (false);
        }

        oValueDictionary = new Dictionary<TValue, Int32>();

        Int32 iDataBodyRangeRow = oDataBodyRange.Row;
        Int32 iRows = oColumnData.Rows.Count;

        for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
        {
            TValue tValue;

            if ( !oTryGetValueFromCell(aoColumnValues, iRowOneBased, 1,
                out tValue) )
            {
                continue;
            }

            oValueDictionary[tValue] = iRowOneBased + iDataBodyRangeRow - 1;
        }

        return (true);
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
    //  Method: GetSelectedStringColumnValues()
    //
    /// <summary>
    /// Gets a collection of unique string values from one column for all rows
    /// in the table that have at least one selected cell.
    /// </summary>
    ///
    /// <param name="columnName">
    /// Name of the column to read the values from.
    /// </param>
    ///
    /// <returns>
    /// A collection of zero or more string cell values.  The collection can be
    /// empty but is never null.  The collection values are the cell values,
    /// which are guaranteed to be non-null, non-empty, and unique.
    /// </returns>
    ///
    /// <remarks>
    /// This method activates the worksheet if it isn't already activated.
    /// Then, for each row in the table that has at least one selected cell, it
    /// reads the string value from the row's <paramref name="columnName" />
    /// cell and stores it in the returned collection.
    /// </remarks>
    //*************************************************************************

    public ICollection<String>
    GetSelectedStringColumnValues
    (
        String columnName
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(columnName) );
        AssertValid();

        return ( GetSelectedColumnValues<String>(columnName,
            ExcelUtil.TryGetNonEmptyStringFromCell) );
    }

    //*************************************************************************
    //  Method: GetSelectedColumnValues()
    //
    /// <summary>
    /// Gets a collection of unique values from one column for all rows in the
    /// table that have at least one selected cell.
    /// </summary>
    ///
    /// <typeparam name="TValue">
    /// Type of the column values to read.
    /// </typeparam>
    ///
    /// <param name="columnName">
    /// Name of the column to read the values from.
    /// </param>
    ///
    /// <param name="tryGetValueFromCell">
    /// Method that attempts to get a value of a specified type from a
    /// worksheet cell given an array of cell values read from the worksheet.
    /// </param>
    ///
    /// <returns>
    /// A collection of zero or more cell values.  The collection can be empty
    /// but is never null.  The collection values are the cell values, which
    /// are guaranteed to be non-null, non-empty, and unique.
    /// </returns>
    ///
    /// <remarks>
    /// This method activates the worksheet if it isn't already activated.
    /// Then, for each row in the table that has at least one selected cell, it
    /// reads the value from the row's <paramref name="columnName" /> cell and
    /// stores it in the returned collection.
    /// </remarks>
    //*************************************************************************

    public ICollection<TValue>
    GetSelectedColumnValues<TValue>
    (
        String columnName,
        ExcelUtil.TryGetValueFromCell<TValue> tryGetValueFromCell
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(columnName) );
        Debug.Assert(tryGetValueFromCell != null);
        AssertValid();

        // Create a HashSet for the selected values.  The HashSet key is the
        // cell value.

        HashSet<TValue> oSelectedValues = new HashSet<TValue>();

        if (!this.TableExists)
        {
            goto Done;
        }

        // The selected range can extend outside the table.  Get the
        // intersection of the table with the selection.  Note that
        // ExcelUtil.TryGetSelectedTableRange() activates the worksheet.

        ListObject oTable;
        Range oSelectedTableRange;

        if ( !ExcelUtil.TryGetSelectedTableRange(
            (Workbook)m_oWorksheet.Parent, m_oWorksheet.Name,
            m_oTable.Name, out oTable, out oSelectedTableRange) )
        {
            goto Done;
        }

        Range oDataBodyRange = m_oTable.DataBodyRange;

        Debug.Assert(oDataBodyRange != null);

        // Get data for the specified column.  This includes hidden rows but
        // excludes the header row.

        Range oColumnData;
        Object [,] aoColumnValues;

        if ( !ExcelUtil.TryGetTableColumnDataAndValues(m_oTable.InnerObject,
            columnName, out oColumnData, out aoColumnValues) )
        {
            goto Done;
        }

        // Read the column.

        foreach (Range oSelectedTableRangeArea in oSelectedTableRange.Areas)
        {
            Int32 iFirstRowOneBased =
                oSelectedTableRangeArea.Row - oDataBodyRange.Row + 1;

            Int32 iLastRowOneBased =
                iFirstRowOneBased + oSelectedTableRangeArea.Rows.Count - 1;

            for (Int32 iRowOneBased = iFirstRowOneBased;
                iRowOneBased <= iLastRowOneBased; iRowOneBased++)
            {
                TValue tValue;

                if ( tryGetValueFromCell(aoColumnValues, iRowOneBased, 1,
                    out tValue) )
                {
                    oSelectedValues.Add(tValue);
                }
            }
        }

        Done:

        return (oSelectedValues);
    }

    //*************************************************************************
    //  Method: SelectTableRowsByColumnValues()
    //
    /// <summary>
    /// Selects the rows in the table that contain one of a collection of
    /// values in a specified column.
    /// </summary>
    ///
    /// <typeparam name="TValue">
    /// Type of the values in the specified column.
    /// </typeparam>
    ///
    /// <param name="columnName">
    /// Name of the column to read.
    /// </param>
    ///
    /// <param name="valuesToSelect">
    /// Collection of values to look for.
    /// </param>
    ///
    /// <param name="tryGetValueFromCell">
    /// Method that attempts to get a value of a specified type from a
    /// worksheet cell given an array of cell values read from the worksheet.
    /// </param>
    ///
    /// <remarks>
    /// This method activates the worksheet if it isn't already activated, then
    /// selects each row that contains one of the values in <paramref
    /// name="valuesToSelect" /> in the column named <paramref
    /// name="columnName" />.  The values are of type TValue.
    ///
    /// <para>
    /// Any row that doesn't contain one of the specified values gets
    /// deselected.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    SelectTableRowsByColumnValues<TValue>
    (
        String columnName,
        ICollection<TValue> valuesToSelect,
        ExcelUtil.TryGetValueFromCell<TValue> tryGetValueFromCell
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(columnName) );
        Debug.Assert(valuesToSelect != null);
        Debug.Assert(tryGetValueFromCell != null);
        AssertValid();

        if (!TableExists)
        {
            return;
        }

        ExcelUtil.ActivateWorksheet(m_oWorksheet.InnerObject);

        Int32 iValuesToSelect = valuesToSelect.Count;

        if (iValuesToSelect == 0)
        {
            // Unselect any currently selected rows.

            m_oTable.HeaderRowRange.Select();

            return;
        }

        // Get a dictionary containing the value of each row in the table.  The
        // key is the value from the specified column and the value is the
        // one-based row number relative to the worksheet.

        Dictionary<TValue, Int32> oValueDictionary;

        if ( !TryGetValuesInAllRows<TValue>(columnName, tryGetValueFromCell,
            out oValueDictionary) )
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

        Int32 i = 0;

        foreach (TValue tValueToSelect in valuesToSelect)
        {
            Int32 iRow;

            if ( oValueDictionary.TryGetValue(tValueToSelect, out iRow) )
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
                (i == iValuesToSelect - 1 && iBuiltRangeAddressLength > 0)
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

        i++;
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
        Debug.Assert(m_oWorksheet != null);
        Debug.Assert(m_oTable != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The worksheet that owns this object.

    protected Microsoft.Office.Tools.Excel.Worksheet m_oWorksheet;

    /// The NodeXL table in the worksheet.

    protected Microsoft.Office.Tools.Excel.ListObject m_oTable;
}

}
