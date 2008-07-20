
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//	Class: ExcelUtil
//
/// <summary>
///	Static utility methods for working with Excel from Visual Studio Tools for
/// Office (VSTO) projects.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class ExcelUtil
{
    //*************************************************************************
    //  Method: ExcelApplicationIsReady
    //
    /// <summary>
    /// Determines whether the Excel application is ready to accept method
	/// calls.
    /// </summary>
	///
    /// <param name="application">
	/// Excel Application object.
    /// </param>
    ///
    /// <returns>
	/// true if the Excel application is ready to accept method calls.
    /// </returns>
    //*************************************************************************

	public static Boolean
	ExcelApplicationIsReady
	(
        Microsoft.Office.Interop.Excel.Application application
	)
    {
		Debug.Assert(application != null);

		// Calling into Excel while a cell is in edit mode or while Excel is
		// displaying a modal dialog (see notes below) can raise an exception.
		// There is an Application.Ready property that is supposed to determine
		// whether the application is ready to accept calls, but it always
		// returns true while a cell is in edit mode.  The following workaround
		// was suggested in several online postings.  It checks whether the
		// File,Open command is enabled.  Excel disables this command while a
		// cell is in edit mode.

		return (application.CommandBars.FindControl(
			Missing.Value, 23, Missing.Value, Missing.Value).Enabled);


		// Repro case for the problem with modal dialogs:
		//
		//   1. Open .NetMap workbook Test2.xlsx and read the workbook into
		//      the graph.
		//
		//   2. Open .NetMap workbook Test1.xlsx and read the workbook into
		//      the graph.
		//
		//   3. Close Excel.  Excel asks if you want to save the changes to
		//      Test1.xlsx, which is on top.  Say no.
		//
		//   4. Closing Test1.xlsx uncovers Test2.xlsx, which then redraws
		//      its graph.  This causes TaskPane_GraphDrawn() to run, which
		//      makes various calls into Excel.
		//
		//   5. Excel is busy with "do you want to save..." dialogs, and
		//      at least one Excel call ( Application.Intersection() )
		//      throws a COMException with error code 0x800AC472.
    }

    //*************************************************************************
    //  Method: TryGetWorksheet()
    //
    /// <summary>
	/// Attempts to get a worksheet by name.
    /// </summary>
    ///
    /// <param name="workbook">
	/// Workbook to get a worksheet from.
    /// </param>
    ///
    /// <param name="worksheetName">
	/// Name of the worksheet to get.
    /// </param>
    ///
    /// <param name="worksheet">
	/// Where the requested worksheet gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
	/// true if successful.
    /// </returns>
	///
    /// <remarks>
	/// If <paramref name="workbook" /> contains a worksheet named <paramref
	/// name="worksheetName" />, the worksheet is stored at <paramref
	/// name="worksheet" /> and true is returned.  false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetWorksheet
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
		String worksheetName,
		out Worksheet worksheet
    )
    {
		Debug.Assert(workbook != null);
		Debug.Assert( !String.IsNullOrEmpty(worksheetName) );

		worksheet = null;

        Object oSheet;

        try
        {
            oSheet = workbook.Sheets[worksheetName];
        }
        catch (COMException)
        {
            return (false);
        }

		if ( !(oSheet is Worksheet) )
		{
			return (false);
		}

		worksheet = (Worksheet)oSheet;

		return (true);
    }

    //*************************************************************************
    //  Method: ActivateWorksheet()
    //
    /// <overloads>
	/// Activates a worksheet.
    /// </overloads>
    ///
    /// <summary>
	/// Activates a worksheet.
    /// </summary>
    ///
	/// <param name="worksheet">
	/// The worksheet to activate.
	/// </param>
    //*************************************************************************

	public static void
	ActivateWorksheet
	(
        Microsoft.Office.Interop.Excel.Worksheet worksheet
	)
	{
		Debug.Assert(worksheet != null);

		// See this posting for an explanation of the strange cast:
		//
		// http://blogs.officezealot.com/maarten/archive/2006/01/02/8918.aspx

		( (Microsoft.Office.Interop.Excel._Worksheet)worksheet ).Activate();
	}

    //*************************************************************************
    //  Method: ActivateWorksheet()
    //
    /// <summary>
	/// Activates a worksheet that contains a table (ListObject).
    /// </summary>
    ///
	/// <param name="table">
	/// The table whose parent worksheet should be activated.
	/// </param>
    //*************************************************************************

	public static void
	ActivateWorksheet
	(
		ListObject table
	)
	{
		Debug.Assert(table != null);

        Debug.Assert(table.Parent is Worksheet);

		ActivateWorksheet( (Worksheet)table.Parent );
	}

    //*************************************************************************
    //  Method: WorksheetIsActive()
    //
    /// <summary>
	/// Determines whether a worksheet is the active worksheet.
    /// </summary>
    ///
	/// <param name="worksheet">
	/// Worksheet to test.
	/// </param>
	///
	/// <returns>
	/// true if <paramref name="worksheet" /> is the active worksheet in its
	/// workbook.
	/// </returns>
    //*************************************************************************

    public static Boolean
	WorksheetIsActive
	(
        Microsoft.Office.Interop.Excel.Worksheet worksheet
	)
	{
		Debug.Assert(worksheet != null);
		Debug.Assert(worksheet.Parent is Workbook);

		return ( ( (Workbook)worksheet.Parent ).ActiveSheet == worksheet );
	}

    //*************************************************************************
    //  Method: GetRangeAddress()
    //
    /// <summary>
	/// Gets a Range's address in A1 style.
    /// </summary>
    ///
    /// <param name="range">
	/// Range to get the address of.
    /// </param>
    ///
	/// <returns>
	/// The A1-style address of <paramref name="range" />.  Sample: "A1:B2".
	/// </returns>
    //*************************************************************************

    public static String
    GetRangeAddress
    (
		Range range
    )
    {
		Debug.Assert(range != null);

		return ( range.get_Address(
			false, false, XlReferenceStyle.xlA1, Missing.Value, Missing.Value
			) );
    }

    //*************************************************************************
    //  Method: TryGetNamedRange()
    //
    /// <summary>
	/// Attempts to get a named range from a worksheet.
    /// </summary>
    ///
    /// <param name="worksheet">
	/// Worksheet to get the named range from.
    /// </param>
    ///
    /// <param name="rangeName">
	/// Name of the range to get.
    /// </param>
    ///
    /// <param name="namedRange">
	/// Where the named range gets stored if true is returned.
    /// </param>
	///
	/// <returns>
	/// true if successful.
	/// </returns>
	///
    /// <remarks>
	/// If a range named <paramref name="rangeName" /> exists in <paramref
	/// name="worksheet" />, the named range is stored at <paramref
	/// name="namedRange" /> and true is returned.  false is returned
	/// otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetNamedRange
    (
        Microsoft.Office.Interop.Excel.Worksheet worksheet,
		String rangeName,
		out Range namedRange
    )
    {
		Debug.Assert(worksheet != null);
		Debug.Assert( !String.IsNullOrEmpty(rangeName) );

        namedRange = null;

		try
		{
			namedRange = worksheet.get_Range(rangeName, Missing.Value);

			return (true);
		}
		catch (System.Runtime.InteropServices.COMException)
		{
			return (false);
		}
    }

    //*************************************************************************
    //  Method: TryGetVisibleRange()
    //
    /// <summary>
	/// Attempts to reduce a range to visible cells only.
    /// </summary>
    ///
    /// <param name="range">
	/// The range to reduce.
    /// </param>
	///
    /// <param name="visibleRange">
	/// Where the visible range gets stored if true is returned.  The range
	/// may consist of multiple areas.
    /// </param>
	///
    /// <returns>
	/// true if the visible range was obtained.
    /// </returns>
	///
	/// <remarks>
	/// If <paramref name="range" /> contains at least one cell that is
	/// visible, the range of visible cells is stored at <paramref
	/// name="visibleRange" /> and true is returned.  false is returned
	/// otherwise.
	///
	/// <param>
	/// WARNING: Due to an apparent bug in Excel, this method can cause the
	/// Microsoft.Office.Tools.Excel.ListObject.SelectionChange event to fire.
	/// </param>
	///
	/// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetVisibleRange
    (
		Range range,
		out Range visibleRange
    )
    {
		Debug.Assert(range != null);

		// WARNING: If the range contains hidden cells, range.SpecialCells()
		// causes the Microsoft.Office.Tools.Excel.ListObject.SelectionChange
		// event to fire.  It shouldn't, but it does.

		visibleRange = range.SpecialCells(
			XlCellType.xlCellTypeVisible, Missing.Value);

		return (visibleRange != null);
    }

    //*************************************************************************
    //  Method: TryGetLastNonEmptyRow()
    //
    /// <summary>
	/// Attempts to get the one-based row number of the last row in a range
	/// has a non-empty cell.
    /// </summary>
    ///
    /// <param name="range">
	/// The range to use.
    /// </param>
	///
    /// <param name="rowOneBased">
	/// Where the one-based row number gets stored if true is returned.
    /// </param>
	///
    /// <returns>
	/// true if the row number was obtained.
    /// </returns>
	///
	/// <remarks>
	/// This method searches <paramref name="range" /> from the bottom up for
	/// the first non-empty cell.  If it finds one, it stores the cell's
	/// one-based row number at <paramref name="rowOneBased" /> and returns
	/// true.  false is returned otherwise.
	/// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetLastNonEmptyRow
    (
		Range range,
		out Int32 rowOneBased
    )
    {
		Debug.Assert(range != null);

		rowOneBased = Int32.MinValue;

		#if false

		This should really be done with the following code:

		{
		Range oCell = range.Find("*", MissingValue, XlFindLookIn.xlFormulas,
			XlLookAt.xlWhole, XlSearchOrder.xlByRows,
			XlSearchDirection.xlPrevious, true, true, Missing.Value
			);

		if (oCell == null)
		{
			return (false);
		}

		rowOneBased = oCell.Row;
		return (true);
		}

		However, Range.Find() has problems finding things when cells are
		hidden.  If the third parameter is XlFindLookIn.xlValues, it doesn't
		look in hidden cells at all.  If the third parameter is xlFormulas,
		it sometimes, but not always, looks in hidden cells.

		To work around these oddities, use a brute-force technique of reading
		all the cells and searching manually.  This is wasteful and slow, but
		it works.

		#endif

		Object [,] aoValues = GetRangeValues(range);

		Int32 iRows = aoValues.GetUpperBound(0);
		Int32 iColumns = aoValues.GetUpperBound(1);

		for (Int32 iRowOneBased = iRows; iRowOneBased >= 1; iRowOneBased--)
		{
			for (Int32 iColumnOneBased = 1; iColumnOneBased <= iColumns;
				iColumnOneBased++)
			{
				if (aoValues[iRowOneBased, iColumnOneBased] != null)
				{
					rowOneBased = range.Row + iRowOneBased - 1;

					return (true);
				}
			}
		}

		return (false);
    }

    //*************************************************************************
    //  Method: GetRangeValues()
    //
    /// <summary>
	/// Gets a range's values as a two-dimensional, one-based array.
    /// </summary>
    ///
    /// <param name="range">
	/// Range to get the values for.
    /// </param>
    ///
    /// <returns>
	/// A two-dimensional array of Objects.  Each dimension is one-based.
    /// </returns>
    ///
    /// <remarks>
	/// This is an alternative to Range.get_value(), which doesn't return a
	/// consistent type.  When a range contains two or more cells,
	/// Range.get_value() returns a two-dimensional array of Objects.  When a
	/// range contains just one cell, however, Range.get_value() returns an
	/// Object.
	///
	/// <para>
	/// In contrast, this method always returns a two-dimensional array of
	/// Objects.  In the one-cell case, the array contains a single Object.
	/// </para>
	///
	/// <para>
	/// The returned array has one-based dimensions, so the Object
	/// corresponding to the first cell in the range is at [1,1].
	/// </para>
	/// 
    /// </remarks>
    //*************************************************************************

    public static Object [,]
    GetRangeValues
    (
		Range range
    )
    {
		Debug.Assert(range != null);

		if (range.Rows.Count > 1 || range.Columns.Count > 1)
		{
			return ( (Object[,] )range.get_Value(Missing.Value) );
		}

		Object [,] aoCellValues = GetSingleElement2DArray();

		aoCellValues[1, 1] = range.get_Value(Missing.Value);

		return (aoCellValues);
    }

    //*************************************************************************
    //  Method: SetRangeValues()
    //
    /// <summary>
	/// Sets the values on a range.
    /// </summary>
    ///
    /// <param name="upperLeftCornerMarker">
	/// The values are set on the parent worksheet starting at this range's
	/// upper-left corner.  Only those cells that correspond to <paramref
	/// name="values" /> are set, so the only requirement for this range is
	/// that it's upper-left corner be in the desired location.  It's size is
	/// unimportant.
    /// </param>
    ///
    /// <param name="values">
	/// The values to set.  The array indexes can be either zero-based or
	/// one-based.
    /// </param>
	///
	/// <remarks>
	/// This method copies <paramref name="values" /> to the parent worksheet
	/// of the <paramref name="upperLeftCornerMarker" /> range, starting at the 
	/// range's upper-left corner.  If the upper-left corner is B2 and
	/// <paramref name="values" /> is 3 rows by 2 columns, for example, then
	/// the values are copied to B2:C4.
	/// </remarks>
    //*************************************************************************

	public static void
	SetRangeValues
	(
		Range upperLeftCornerMarker,
		Object [,] values
	)
	{
		Debug.Assert(upperLeftCornerMarker != null);
		Debug.Assert(values != null);

        Debug.Assert(upperLeftCornerMarker.Parent is Worksheet);

        Worksheet oWorksheet = (Worksheet)upperLeftCornerMarker.Parent;

		Int32 iRow = upperLeftCornerMarker.Row;
		Int32 iColumn = upperLeftCornerMarker.Column;

        Range oRangeToSet = oWorksheet.get_Range(

            oWorksheet.Cells[iRow, iColumn],

            oWorksheet.Cells[
				iRow + values.GetUpperBound(0) - values.GetLowerBound(0),
                iColumn + values.GetUpperBound(1) - values.GetLowerBound(1)
				]
            );

		oRangeToSet.set_Value(Missing.Value, values);
	}

    //*************************************************************************
    //  Method: SetVisibleRangeValue()
    //
    /// <summary>
	/// Sets the values in the visible cells of a range to a specified value.
    /// </summary>
    ///
    /// <param name="range">
	/// Range whose visible cells should be set to a value.
    /// </param>
    ///
    /// <param name="value">
	/// The value to set.
    /// </param>
	///
	/// <remarks>
	/// This method sets the value of every cell in <paramref name="range" />
	/// to <paramref name="value" />.
	/// </remarks>
    //*************************************************************************

	public static void
	SetVisibleRangeValue
	(
		Range range,
		Object value
	)
	{
		Debug.Assert(range != null);

		Range oVisibleRange;

		if ( !ExcelUtil.TryGetVisibleRange(range, out oVisibleRange) )
		{
			return;
		}

		// Loop through the areas.

		foreach (Range oVisibleArea in oVisibleRange.Areas)
		{
			Int32 iRows = oVisibleArea.Rows.Count;
			Int32 iColumns = oVisibleArea.Columns.Count;

			Object [,] oVisibleAreaValues = ( Object [,] )
				Array.CreateInstance(
					typeof(Object), new Int32[] {iRows, iColumns},
					new Int32[] {1,1} 
					);

			for (Int32 iRow = 1; iRow <= iRows; iRow++)
			{
				for (Int32 iColumn = 1; iColumn <= iColumns; iColumn++)
				{
					oVisibleAreaValues[iRow, iColumn] = value;
				}
			}

			oVisibleArea.set_Value(Missing.Value, oVisibleAreaValues);
		}
	}

    //*************************************************************************
    //  Method: GetRangeFormulas()
    //
    /// <summary>
	/// Gets a range's formulas as a two-dimensional, one-based array.
    /// </summary>
    ///
    /// <param name="range">
	/// Range to get the formulas for.
    /// </param>
    ///
    /// <returns>
	/// A two-dimensional array of Objects.  Each dimension is one-based.
    /// </returns>
    ///
    /// <remarks>
	/// This is an alternative to Range.Formula, which doesn't return a
	/// consistent type.  When a range contains two or more cells,
	/// Range.Formula returns a two-dimensional array of Objects.  When a
	/// range contains just one cell, however, Range.Formula() returns an
	/// Object.
	///
	/// <para>
	/// In contrast, this method always returns a two-dimensional array of
	/// Objects.  In the one-cell case, the array contains a single Object.
	/// </para>
	///
	/// <para>
	/// The returned array has one-based dimensions, so the Object
	/// corresponding to the first cell in the range is at [1,1].
	/// </para>
	/// 
    /// </remarks>
    //*************************************************************************

    public static Object [,]
    GetRangeFormulas
    (
		Range range
    )
    {
		Debug.Assert(range != null);

		if (range.Rows.Count > 1 || range.Columns.Count > 1)
		{
			return ( (Object[,] )range.Formula );
		}

		Object [,] aoCellFormulas = GetSingleElement2DArray();

		aoCellFormulas[1, 1] = range.Formula;

		return (aoCellFormulas);
    }

    //*************************************************************************
    //  Method: SelectRange()
    //
    /// <summary>
	/// Selects a range.
    /// </summary>
    ///
	/// <param name="range">
	/// The range to select.
	/// </param>
	///
	/// <remarks>
	/// This method activates the range's worksheet before selecting the
	/// range.
	/// </remarks>
    //*************************************************************************

	public static void
	SelectRange
	(
		Range range
	)
	{
		Debug.Assert(range != null);

		range.Application.Goto(range, false);
	}

    //*************************************************************************
    //  Method: TryGetNonEmptyStringFromCell()
    //
    /// <summary>
	/// Attempts to get a non-empty string from a worksheet cell.
    /// </summary>
    ///
    /// <param name="cellValues">
	/// Two-dimensional array of values read from the worksheet.
    /// </param>
    ///
    /// <param name="rowOneBased">
	/// One-based row number to read.
    /// </param>
    ///
    /// <param name="columnOneBased">
	/// One-based column number to read.
    /// </param>
    ///
    /// <param name="nonEmptyString">
	/// Where a string gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
	/// true if successful.
    /// </returns>
    ///
    /// <remarks>
	/// If the specified cell value contains anything besides spaces, the cell
	/// value is trimmed and stored at <paramref name="nonEmptyString" />, and
	/// true is returned.  false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetNonEmptyStringFromCell
    (
		Object [,] cellValues,
		Int32 rowOneBased,
		Int32 columnOneBased,
		out String nonEmptyString
    )
    {
		Debug.Assert(cellValues != null);
		Debug.Assert(rowOneBased >= 1);
		Debug.Assert(columnOneBased >= 1);

		nonEmptyString = null;

		Object oObject = cellValues[rowOneBased, columnOneBased];

		if (oObject == null)
		{
			return (false);
		}

		String sString = oObject.ToString().Trim();

		if (sString.Length == 0)
		{
			return (false);
		}

		nonEmptyString = sString;

		return (true);
    }

	//*************************************************************************
	//	Method: CellContainsFormula()
	//
	/// <summary>
	/// Determines whether a cell contains a formula.
	/// </summary>
	///
	/// <param name="cellValues">
	/// A two-dimensional array of Objects.  Each dimension is one-based.
	/// </param>
	///
    /// <param name="rowOneBased">
	/// One-based row number to check.
    /// </param>
    ///
    /// <param name="columnOneBased">
	/// One-based column number to check.
    /// </param>
	///
	/// <returns>
	/// true if the cell at the specified location in <paramref
	/// name="cellValues" /> contains a formula.
	/// </returns>
	//*************************************************************************

	public static Boolean
	CellContainsFormula
	(
		Object [,] cellValues,
		Int32 rowOneBased,
		Int32 columnOneBased
	)
	{
		Debug.Assert(cellValues != null);
		Debug.Assert(rowOneBased >= 1);
		Debug.Assert(columnOneBased >= 1);

		String sNonEmptyString;

		return (
			TryGetNonEmptyStringFromCell(cellValues, rowOneBased,
				columnOneBased, out sNonEmptyString)
			&&
			sNonEmptyString.IndexOf('=') >= 0
			);

	}

    //*************************************************************************
    //  Method: TryGetTable()
    //
    /// <overloads>
	/// Attempts to get a table (ListObject) by name.
    /// </overloads>
	///
    /// <summary>
	/// Attempts to get a table (ListObject) from a workbook by worksheet name
	/// and table name.
    /// </summary>
    ///
    /// <param name="workbook">
	/// Workbook to get the table from.
    /// </param>
	///
    /// <param name="worksheetName">
	/// Name of the worksheet containing the table.
    /// </param>
    ///
    /// <param name="tableName">
	/// Name of the table to get.
    /// </param>
    ///
    /// <param name="table">
	/// Where the requested table gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
	/// true if successful.
    /// </returns>
	///
    /// <remarks>
	/// If <paramref name="workbook" /> contains a worksheet named <paramref
	/// name="worksheetName" /> that has a table (ListObject) named
	/// <paramref name="tableName" />, the ListObject is stored at <paramref
	/// name="table" /> and true is returned.  false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetTable
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
		String worksheetName,
		String tableName,
		out ListObject table
    )
    {
		Debug.Assert(workbook != null);
		Debug.Assert( !String.IsNullOrEmpty(worksheetName) );
		Debug.Assert( !String.IsNullOrEmpty(tableName) );

		table = null;

        Microsoft.Office.Interop.Excel.Worksheet oWorksheet;

		return ( TryGetWorksheet(workbook, worksheetName, out oWorksheet) &&
			TryGetTable(oWorksheet, tableName, out table) );
    }

    //*************************************************************************
    //  Method: TryGetTable()
    //
    /// <summary>
	/// Attempts to get a table (ListObject) from a worksheet by table name.
    /// </summary>
    ///
    /// <param name="worksheet">
	/// Worksheet to get the table from.
    /// </param>
    ///
    /// <param name="tableName">
	/// Name of the table to get.
    /// </param>
    ///
    /// <param name="table">
	/// Where the requested table gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
	/// true if successful.
    /// </returns>
	///
    /// <remarks>
	/// If <paramref name="worksheet" /> contains a table (ListObject) named
	/// <paramref name="tableName" />, the ListObject is stored at <paramref
	/// name="table" /> and true is returned.  false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetTable
    (
        Microsoft.Office.Interop.Excel.Worksheet worksheet,
		String tableName,
		out ListObject table
    )
    {
		Debug.Assert(worksheet != null);
		Debug.Assert( !String.IsNullOrEmpty(tableName) );

        table = null;

        try
        {
            table = worksheet.ListObjects[tableName];

            return (true);
        }
        catch (COMException)
        {
            return (false);
        }
    }

    //*************************************************************************
    //  Method: TryGetVisibleTableRange()
    //
    /// <summary>
	/// Attempts to get a range containing visible data rows from a table.
    /// </summary>
    ///
    /// <param name="table">
	/// Table to get a range from.
    /// </param>
	///
    /// <param name="visibleTableRange">
	/// Where the range gets stored if true is returned.  The range contains at
	/// least one visible row and may consist of multiple areas.
    /// </param>
	///
    /// <returns>
	/// true if the range was obtained.
    /// </returns>
	///
	/// <remarks>
	/// If <paramref name="table" /> contains at least one data row that is
	/// visible, the range of visible rows is stored at <paramref
	/// name="visibleTableRange" /> and true is returned.  false is returned
	/// otherwise.
	/// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetVisibleTableRange
    (
		ListObject table,
		out Range visibleTableRange
    )
    {
		Debug.Assert(table != null);

		visibleTableRange = null;

		// Read the range that contains the table data.  If the table is
		// filtered, the range may contain multiple areas.

        Range oDataBodyRange = table.DataBodyRange;

		// Reduce the range to visible rows.

		return ( oDataBodyRange != null &&
			TryGetVisibleRange(oDataBodyRange, out visibleTableRange) );
    }

    //*************************************************************************
    //  Method: TryGetTableColumn()
    //
    /// <summary>
	/// Attempts to get a table column given the column name.
    /// </summary>
    ///
    /// <param name="table">
	/// ListObject to get the column from.
    /// </param>
    ///
    /// <param name="columnName">
	/// Name of the column to get.
    /// </param>
    ///
    /// <param name="column">
	/// Where the column named <paramref name="columnName" /> gets stored if
	/// true is returned.
    /// </param>
    ///
    /// <returns>
	/// true if successful.
    /// </returns>
    ///
    /// <remarks>
	/// If <paramref name="table" /> contains a column named <paramref
	/// name="columnName" />, the column is stored at <paramref
	/// name="column" /> and true is returned.  false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetTableColumn
    (
        ListObject table,
		String columnName,
		out ListColumn column
    )
    {
		Debug.Assert(table != null);
		Debug.Assert( !String.IsNullOrEmpty(columnName) );

		column = null;

		try
		{
			column = table.ListColumns[columnName];

			return (true);
		}
		catch (COMException)
		{
			return (false);
		}
    }

    //*************************************************************************
    //  Method: TryGetTableColumnDataAndValues()
    //
    /// <summary>
	/// Attempts to get the data range and values of one column of a table.
    /// </summary>
    ///
    /// <param name="table">
	/// ListObject to get the column data range and values from.
    /// </param>
	///
    /// <param name="columnName">
	/// Name of the column to get data range and values for.
    /// </param>
	///
    /// <param name="tableColumnData">
	/// Where the column data range gets stored if true is returned.  The data
	/// range includes only that part of the column within the table's data
	/// body range.  This excludes any header or totals row.
    /// </param>
    ///
    /// <param name="tableColumnDataValues">
	/// Where the values for <paramref name="tableColumnData" /> get stored if
	/// true is returned.
    /// </param>
	///
    /// <returns>
	/// true if successful.
    /// </returns>
    ///
    /// <remarks>
	/// If <paramref name="table" /> contains a column named <paramref
	/// name="columnName" />, the column's data range is stored at <paramref
	/// name="tableColumnData" />, the values for the data range are stored at
	/// <paramref name="tableColumnDataValues" />, and true is returned.  false
	/// is returned otherwise.
    /// </remarks>
	//
	//  TODO: This hasn't been tested with a totals row.
    //*************************************************************************

    public static Boolean
    TryGetTableColumnDataAndValues
    (
		ListObject table,
		String columnName,
		out Range tableColumnData,
		out Object [,] tableColumnDataValues
    )
    {
		Debug.Assert(table != null);
		Debug.Assert( !String.IsNullOrEmpty(columnName) );

		tableColumnData = null;
		tableColumnDataValues = null;

		if ( !ExcelUtil.TryGetTableColumnData(table, columnName,
			out tableColumnData) )
		{
			return (false);
		}

		tableColumnDataValues = ExcelUtil.GetRangeValues(tableColumnData);

		return (true);
    }

    //*************************************************************************
    //  Method: TryGetTableColumnData()
    //
    /// <summary>
	/// Attempts to get the data range of one column of a table.
    /// </summary>
    ///
    /// <param name="table">
	/// ListObject to get the column data range from.
    /// </param>
	///
    /// <param name="columnName">
	/// Name of the column to get data range for.
    /// </param>
	///
    /// <param name="tableColumnData">
	/// Where the column data range gets stored if true is returned.  The data
	/// range includes only that part of the column within the table's data
	/// body range.  This excludes any header or totals row.
    /// </param>
    ///
    /// <returns>
	/// true if successful.
    /// </returns>
    ///
    /// <remarks>
	/// If <paramref name="table" /> contains a column named <paramref
	/// name="columnName" />, the column's data range is stored at <paramref
	/// name="tableColumnData" /> and true is returned.  false is returned
	/// otherwise.
    /// </remarks>
	//
	//  TODO: This hasn't been tested with a totals row.
    //*************************************************************************

    public static Boolean
    TryGetTableColumnData
    (
		ListObject table,
		String columnName,
		out Range tableColumnData
    )
    {
		Debug.Assert(table != null);
		Debug.Assert( !String.IsNullOrEmpty(columnName) );

		tableColumnData = null;

		ListColumn oColumn;

		if ( !TryGetTableColumn(table, columnName, out oColumn) )
		{
			return (false);
		}

		Range oDataBodyRange = table.DataBodyRange;

		if (oDataBodyRange == null)
		{
			// This happens when the user deletes the first data row of a one-
			// row table.  It looks like an empty row is there, but the
			// DataBodyRange is actually null.

			Int32 iRow;

			// Is there a header row?

			Range oRangeToUse = table.HeaderRowRange;

			if (oRangeToUse != null)
			{
				// Yes.  Use the row after the header row.

				iRow = oRangeToUse.Row + 1;
			}
			else
			{
				// No.  Use the first row of the table.

				oRangeToUse = table.Range;

				iRow = oRangeToUse.Row;
			}

			Debug.Assert(table.Parent is Worksheet);

			Worksheet oWorksheet = (Worksheet)table.Parent;

			oDataBodyRange = oWorksheet.get_Range(

				oWorksheet.Cells[iRow, oRangeToUse.Column],

				oWorksheet.Cells[iRow,
					oRangeToUse.Column + oRangeToUse.Columns.Count - 1]
				);
		}

		return ( ExcelUtil.TryIntersectRanges(oDataBodyRange, oColumn.Range,
			out tableColumnData) );
    }

    //*************************************************************************
    //  Method: TryAddTableColumn()
    //
    /// <summary>
	/// Attempts to add a column to the end of a table.
    /// </summary>
    ///
    /// <param name="table">
	/// Table to add a column to.
    /// </param>
    ///
    /// <param name="columnName">
	/// Name of the column to add.
    /// </param>
    ///
    /// <param name="columnWidthChars">
	/// Width of the new column, in characters.
    /// </param>
	///
    /// <param name="listColumn">
	/// Where the added column gets stored if true is returned.
    /// </param>
	///
	/// <returns>
	/// true if the column was added.
	/// </returns>
    //*************************************************************************

	public static Boolean
	TryAddTableColumn
	(
		ListObject table,
		String columnName,
		Single columnWidthChars,
		out ListColumn listColumn
	)
	{
		Debug.Assert(table!= null);
		Debug.Assert( !String.IsNullOrEmpty(columnName) );
		Debug.Assert(columnWidthChars >= 0);

		return ( TryAddOrInsertTableColumn(table, columnName, -1,
			columnWidthChars, out listColumn) );
	}

    //*************************************************************************
    //  Method: TryInsertTableColumn()
    //
    /// <summary>
	/// Attempts to insert a column into a table.
    /// </summary>
    ///
    /// <param name="table">
	/// Table to insert a column into.
    /// </param>
    ///
    /// <param name="columnName">
	/// Name of the column to insert.
    /// </param>
    ///
    /// <param name="oneBasedColumnIndex">
	/// One-based index of the column after it is inserted.
    /// </param>
    ///
    /// <param name="columnWidthChars">
	/// Width of the new column, in characters.
    /// </param>
	///
    /// <param name="listColumn">
	/// Where the inserted column gets stored if true is returned.
    /// </param>
	///
	/// <returns>
	/// true if the column was inserted.
	/// </returns>
    //*************************************************************************

	public static Boolean
	TryInsertTableColumn
	(
		ListObject table,
		String columnName,
		Int32 oneBasedColumnIndex,
		Single columnWidthChars,
		out ListColumn listColumn
	)
	{
		Debug.Assert(table!= null);
		Debug.Assert( !String.IsNullOrEmpty(columnName) );
		Debug.Assert(oneBasedColumnIndex >= 1);
		Debug.Assert(columnWidthChars >= 0);

		return ( TryAddOrInsertTableColumn(table, columnName,
			oneBasedColumnIndex, columnWidthChars, out listColumn) );
	}

	//*************************************************************************
	//	Method: GetTableColumnNames()
	//
	/// <summary>
	///	Gets the names of the columns in a table.
	/// </summary>
	///
	/// <param name="table">
	/// Table to get the column names from.
	/// </param>
	///
	/// <param name="columnNamesToExclude">
	/// An array of zero or more column names to exclude from the returned
	/// array.
	/// </param>
	///
	/// <param name="columnNameBasesToExclude">
	/// An array of zero or more column name bases to exclude from the returned
	/// array.  If the array includes "Custom", for example, then a column
	/// named "Custom 1" will be excluded.
	/// </param>
	///
	/// <returns>
	/// An array of zero or more column names.
	/// </returns>
	///
	/// <remarks>
	/// This method returns the names of all the columns in <paramref
	/// name="table" />, except for those names contained in <paramref
	/// name="columnNamesToExclude" /> and those names that start with the
	/// bases contained in <paramref name="columnNameBasesToExclude" />.
	/// </remarks>
	//*************************************************************************

	public static String []
	GetTableColumnNames
	(
		ListObject table,
		String [] columnNamesToExclude,
		String [] columnNameBasesToExclude
	)
	{
		Debug.Assert(table != null);
		Debug.Assert(columnNamesToExclude != null);
		Debug.Assert(columnNameBasesToExclude != null);

		List<String> oTableColumnNames = new List<String>();

		// Loop through the table's columns.

		foreach (ListColumn oColumn in table.ListColumns)
		{
			String sColumnName = oColumn.Name;

			if ( String.IsNullOrEmpty(sColumnName) )
			{
				continue;
			}

			if (Array.IndexOf(columnNamesToExclude, sColumnName) >= 0)
			{
				continue;
			}

			foreach (String sColumnNameBaseToExclude in
				columnNameBasesToExclude)
			{
				if ( sColumnName.StartsWith(sColumnNameBaseToExclude) )
				{
					continue;
				}
			}

			oTableColumnNames.Add(sColumnName);
		}

		return ( oTableColumnNames.ToArray() );
	}

    //*************************************************************************
    //  Method: ClearTable()
    //
    /// <summary>
	/// Clears a table of all contents.
    /// </summary>
    ///
    /// <param name="table">
	/// Table to clear.
    /// </param>
	///
	/// <remarks>
	/// This method reduces a table to its header row and totals row, if
	/// present, and one data body row.  The data body row contains the
	/// original formatting and data validation, but its contents are cleared.
	/// </remarks>
    //*************************************************************************

	public static void
	ClearTable
	(
		ListObject table
	)
	{
		Debug.Assert(table!= null);

		// First, clear the data body rows.  This clears the contents but
		// retains formatting and data validation.

		Range oDataBodyRange = table.DataBodyRange;

		if (oDataBodyRange != null)
		{
			oDataBodyRange.ClearContents();
		}

		// Reduce the size of the table to get rid of the formatting.

		Int32 iRows = 1;

		if (table.HeaderRowRange != null)
		{
			iRows++;
		}

		if (table.TotalsRowRange != null)
		{
			iRows++;
		}

		Range oTableRange = table.Range;

		oTableRange = oTableRange.get_Resize(iRows, oTableRange.Columns.Count);

		table.Resize(oTableRange);
	}

	//*************************************************************************
	//	Method: ClearTableAutoFilters()
	//
	/// <summary>
	///	Clears any AutoFiltering from a table.
	/// </summary>
	///
	/// <param name="table">
	/// The table to clear AutoFiltering on.
	/// </param>
	//*************************************************************************

	public static void
	ClearTableAutoFilters
	(
		ListObject table
	)
	{
		Debug.Assert(table != null);

        AutoFilter oAutoFilter = table.AutoFilter;

		if (oAutoFilter == null)
		{
			return;
		}

		Filters oFilters = oAutoFilter.Filters;
		Int32 iFilters = oFilters.Count;
		Range oTableRange = table.Range;

        for (Int32 i = 1; i <= iFilters; i++)
        {
            if (oFilters[i].On)
            {
                oTableRange.AutoFilter(i, Missing.Value,
					XlAutoFilterOperator.xlAnd, Missing.Value, Missing.Value);
            }
        }
	}

    //*************************************************************************
    //  Method: TryIntersectRanges()
    //
    /// <summary>
	/// Attempts to get the intersection of two ranges.
    /// </summary>
    ///
    /// <param name="range1">
	/// First range.  Can be null.
    /// </param>
    ///
    /// <param name="range2">
	/// Range to intersect with <paramref name="range1" />.  Can be null.
    /// </param>
    ///
    /// <param name="intersection">
	/// Where the intersection of <paramref name="range1" /> and <paramref
	/// name="range2" /> gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
	/// true if the intersection is not null.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryIntersectRanges
    (
        Range range1,
		Range range2,
		out Range intersection
    )
    {
        intersection = null;

		if (range1 != null && range2 != null)
		{
			intersection = range1.Application.Intersect(range1, range2,

				Missing.Value, Missing.Value, Missing.Value, Missing.Value,
				Missing.Value, Missing.Value, Missing.Value, Missing.Value,
				Missing.Value, Missing.Value, Missing.Value, Missing.Value,
				Missing.Value, Missing.Value, Missing.Value, Missing.Value,
				Missing.Value, Missing.Value, Missing.Value, Missing.Value,
				Missing.Value, Missing.Value, Missing.Value, Missing.Value,
				Missing.Value, Missing.Value, Missing.Value, Missing.Value
				);
		}

		return (intersection != null);
    }

    //*************************************************************************
    //  Method: TryUnionRanges()
    //
    /// <summary>
	/// Attempts to get the union of two ranges.
    /// </summary>
    ///
    /// <param name="range1">
	/// First range.  Can be null.
    /// </param>
    ///
    /// <param name="range2">
	/// Range to union with <paramref name="range1" />.  Can be null.
    /// </param>
    ///
    /// <param name="union">
	/// Where the union of <paramref name="range1" /> and <paramref
	/// name="range2" /> gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
	/// true if the union is not null.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryUnionRanges
    (
        Range range1,
		Range range2,
		out Range union
    )
    {
        union = null;

		if (range1 != null && range2 != null)
		{
			union = range1.Application.Union(range1, range2,

				Missing.Value, Missing.Value, Missing.Value, Missing.Value,
				Missing.Value, Missing.Value, Missing.Value, Missing.Value,
				Missing.Value, Missing.Value, Missing.Value, Missing.Value,
				Missing.Value, Missing.Value, Missing.Value, Missing.Value,
				Missing.Value, Missing.Value, Missing.Value, Missing.Value,
				Missing.Value, Missing.Value, Missing.Value, Missing.Value,
				Missing.Value, Missing.Value, Missing.Value, Missing.Value
				);
		}
		else if (range1 != null)
		{
			union = range1;
		}
		else if (range2 != null)
		{
			union = range2;
		}

		return (union != null);
    }

    //*************************************************************************
    //  Method: GetOneBasedRowNumbers()
    //
    /// <summary>
	/// Gets the one-based row number of every row in a range.
    /// </summary>
    ///
    /// <param name="range">
	/// Range from which to get the one-based row numbers.
    /// </param>
    ///
    /// <returns>
	/// An array of one-based row numbers, one for each row in <paramref
	/// name="range" />.  The order of row numbers in the array is undefined.
	/// The returned array is never null.
    /// </returns>
    //*************************************************************************

    public static Int32 []
    GetOneBasedRowNumbers
    (
        Range range
    )
	{
		Debug.Assert(range != null);

		// One way to do this is to loop through the areas in range, then loop
		// through the rows in each area.  This wouldn't scale well, however,
		// because all the calls to get areas and rows would have to be
		// marshalled, and marshalling is slow.
		//
		// Instead, let Excel do all the work in a single marshalled call that
		// converts the range to an address, then parse the address.

		String sAddress = GetRangeAddress(range);

		return ( GetOneBasedRowNumbers(sAddress) );
	}

    //*************************************************************************
    //  Method: GetOneBasedRowNumbers()
    //
    /// <summary>
	/// Gets the one-based row number of every row in a range.
    /// </summary>
    ///
    /// <param name="rangeAddress">
	/// Range address from which to get the one-based row numbers.  Can be
	/// empty, but can't be null.  Sample range address:
	/// "A3,B16,A30,A24:F24,C2:C244,A5:F5".
    /// </param>
    ///
    /// <returns>
	/// An array of one-based row numbers, one for each row in <paramref
	/// name="range" />.  The order of row numbers in the array is undefined.
	/// The returned array is never null.
    /// </returns>
    //*************************************************************************

    public static Int32 []
    GetOneBasedRowNumbers
    (
        String rangeAddress
    )
	{
		Debug.Assert(rangeAddress != null);

		// Excel delimites areas with commas.  Split the address into area
		// addresses.

		String [] asAreaAddresses = rangeAddress.Split(
			new char[] {','}, StringSplitOptions.RemoveEmptyEntries);

		// Create a dictionary to keep track of unique row numbers.  The key
		// is the row number and the value is unused.

		Dictionary<Int32, Char> oUniqueRowNumbers =
			new Dictionary<Int32, Char>();

		foreach (String sAreaAddress in asAreaAddresses)
		{
			// If the area address includes a colon, split it further.

			String [] asAreaSubaddresses = sAreaAddress.Split(
				new Char[] {':'}, StringSplitOptions.RemoveEmptyEntries);

			Debug.Assert(asAreaSubaddresses.Length >= 1);

			Int32 iFirstRowNumber =
				GetOneBasedRowNumber( asAreaSubaddresses[0] );

			if (asAreaSubaddresses.Length == 1)
			{
				oUniqueRowNumbers[iFirstRowNumber] = ' ';
			}
			else
			{
				Int32 iSecondRowNumber =
					GetOneBasedRowNumber( asAreaSubaddresses[1] );

				for (Int32 iRowNumber = iFirstRowNumber;
					iRowNumber <= iSecondRowNumber; iRowNumber++)
				{
					oUniqueRowNumbers[iRowNumber] = ' ';
				}
			}
		}

		// Copy the unique row numbers to an array.

		return ( CollectionUtil.DictionaryKeysToArray<Int32, Char>(
			oUniqueRowNumbers) );
	}

    //*************************************************************************
    //  Method: GetOneBasedRowNumber()
    //
    /// <summary>
	/// Gets the one-based row number from an A1-style cell address.
    /// </summary>
    ///
    /// <param name="cellAddressA1Style">
	/// Cell address from which to get the one-based row number.  Sample cell
	/// address: "B16".
    /// </param>
    ///
    /// <returns>
	/// The one-based row number for <paramref name="cellAddressA1Style" />.
	/// Sample return value: 16.
    /// </returns>
    //*************************************************************************

	public static Int32
	GetOneBasedRowNumber
	(
		String cellAddressA1Style
	)
	{
		Debug.Assert( !String.IsNullOrEmpty(cellAddressA1Style) );
		Debug.Assert(cellAddressA1Style.IndexOf(':') == -1);

		// Don't use regular expressions here.  They are too slow.

		Int32 iLength = cellAddressA1Style.Length;

		for (Int32 i = 0; i < iLength; i++)
		{
			if ( Char.IsDigit( cellAddressA1Style[i] ) )
			{
				return ( Int32.Parse(cellAddressA1Style.Substring(i) ) );
			}
		}

		Debug.Assert(false);

		return (Int32.MinValue);
	}

	//*************************************************************************
	//	Method: GetTableContextCommandBar()
	//
	/// <summary>
	/// Gets the table context CommandBar.
	/// </summary>
	///
    /// <param name="application">
	/// Excel Application object.
    /// </param>
	///
	/// <remarks>
	/// The context menu that appears when you right-click a table (ListObject)
	/// cell.
	/// </remarks>
	//*************************************************************************

	public static Microsoft.Office.Core.CommandBar
	GetTableContextCommandBar
	(
        Microsoft.Office.Interop.Excel.Application application
	)
	{
		Debug.Assert(application != null);

		return ( application.CommandBars[TableContextCommandBarID] );
	}

    //*************************************************************************
    //  Method: TryAddOrInsertTableColumn()
    //
    /// <summary>
	/// Attempts to add or insert a column into a table.
    /// </summary>
    ///
    /// <param name="table">
	/// Table to add or insert a column into.
    /// </param>
    ///
    /// <param name="columnName">
	/// Name of the column to add or insert.
    /// </param>
    ///
    /// <param name="oneBasedColumnIndex">
	/// One-based index of the column after it is inserted, or -1 to add the
	/// column to the end of the table.
    /// </param>
    ///
    /// <param name="columnWidthChars">
	/// Width of the new column, in characters.
    /// </param>
	///
    /// <param name="listColumn">
	/// Where the new column gets stored if true is returned.
    /// </param>
	///
	/// <returns>
	/// true if the column was added or inserted.
	/// </returns>
    //*************************************************************************

	public static Boolean
	TryAddOrInsertTableColumn
	(
		ListObject table,
		String columnName,
		Int32 oneBasedColumnIndex,
		Single columnWidthChars,
		out ListColumn listColumn
	)
	{
		Debug.Assert(table!= null);
		Debug.Assert( !String.IsNullOrEmpty(columnName) );
		Debug.Assert(oneBasedColumnIndex == -1 || oneBasedColumnIndex >= 1);
		Debug.Assert(columnWidthChars >= 0);

		listColumn = null;

		Object oPosition;

		if (oneBasedColumnIndex == -1)
		{
			oPosition = Missing.Value;
		}
		else
		{
			oPosition = oneBasedColumnIndex;
		}

		try
		{
			listColumn = table.ListColumns.Add(oPosition);
		}
		catch (COMException oCOMException)
		{
			if (oCOMException.ErrorCode == -2146827284)
			{
				// This can happen, for example, if adding a table column
				// would cause a merged cells to unmerge, the user is asked
				// if he wants to allow the unmerge, and he says no.

				return (false);
			}

			throw;
		}

		listColumn.Name = columnName;

		Range oColumnRange = listColumn.Range;

		oColumnRange.ColumnWidth = columnWidthChars;
		oColumnRange.Validation.Delete();

		return (true);
	}

    //*************************************************************************
    //  Method: GetSingleElement2DArray()
    //
    /// <summary>
	/// Gets a two-dimensional Object array with one row and one column.
    /// </summary>
    ///
    /// <returns>
	/// A two-dimensional Object array with one row and one column.  Each
	/// dimension is one-based.  The single element in the array is initialized
	/// to null.
    /// </returns>
    //*************************************************************************

    public static Object [,]
    GetSingleElement2DArray()
    {
		return ( GetSingleColumn2DArray(1) );
    }

    //*************************************************************************
    //  Method: GetSingleColumn2DArray()
    //
    /// <summary>
	/// Gets a two-dimensional Object array with N rows and one column.
    /// </summary>
    ///
    /// <param name="rows">
	/// Number of rows to include in the array.
    /// </param>
	///
    /// <returns>
	/// A two-dimensional Object array with one row and one column.  Each
	/// dimension is one-based.  The elements in the array are initialized to
	/// null.
    /// </returns>
    //*************************************************************************

    public static Object [,]
    GetSingleColumn2DArray
	(
		Int32 rows
	)
    {
		Debug.Assert(rows > 0);

		Object [,] aoSingleColumn2DArray = ( Object [,] )Array.CreateInstance(
			typeof(Object), new Int32[] {rows, 1}, new Int32[] {1,1} );

		return (aoSingleColumn2DArray);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

	/// ID of the command bar that appears when you right-click a table
	/// (ListObject) cell.  The English name of this command bar is
	/// "List Range Popup".

	public static readonly Int32 TableContextCommandBarID = 72;
}

}
