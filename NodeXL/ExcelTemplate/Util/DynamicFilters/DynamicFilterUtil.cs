
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: DynamicFilterUtil
//
/// <summary>
/// Utility methods for working with dynamic filters.
/// </summary>
///
/// <remarks>
/// A dynamic filter is a control that can be adjusted to selectively show and
/// hide edges and vertices in the graph in real time.
///
/// <para>
/// Call the <see cref="GetDynamicFilterParameters" /> method to get filter
/// parameters for all filterable columns in a specified table.
/// </para>
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class DynamicFilterUtil : Object
{
    //*************************************************************************
    //  Method: GetDynamicFilterParameters()
    //
    /// <summary>
	/// Gets a collection of all dynamic filter parameters for one table.
    /// </summary>
	///
    /// <param name="workbook">
	/// Workbook containing the graph data.
    /// </param>
	///
    /// <param name="worksheetName">
	/// Name of the worksheet containing the table.
    /// </param>
	///
    /// <param name="tableName">
	/// Name of the table to get filter parameters for.
    /// </param>
	///
	/// <returns>
	/// One <see cref="DynamicFilterParameters" /> object for each filterable
	/// column in the specified table.
	/// </returns>
	///
	/// <remarks>
	/// The returned collection may be empty, but it is never null.
	/// </remarks>
    //*************************************************************************

    public static ICollection<DynamicFilterParameters>
    GetDynamicFilterParameters
	(
        Microsoft.Office.Interop.Excel.Workbook workbook,
		String worksheetName,
		String tableName
	)
    {
		Debug.Assert(workbook != null);
		Debug.Assert( !String.IsNullOrEmpty(worksheetName) );
		Debug.Assert( !String.IsNullOrEmpty(tableName) );

		#if false  // For testing.
		return ( GetRandomDynamicFilterParameters(tableName) );
		#endif

		LinkedList<DynamicFilterParameters> oDynamicFilterParameters = 
			new LinkedList<DynamicFilterParameters>();

		// Get the specified table and loop through its columns.

		ListObject oTable;

		if ( ExcelUtil.TryGetTable(workbook, worksheetName, tableName,
			out oTable) )
		{
			Application oApplication = workbook.Application;

			foreach (ListColumn oColumn in oTable.ListColumns)
			{
				if ( ColumnShouldBeExcluded(oColumn) )
				{
					continue;
				}

				ExcelColumnFormat eColumnFormat = GetColumnFormat(oColumn);

				switch (eColumnFormat)
				{
					case ExcelColumnFormat.Number:
					case ExcelColumnFormat.Date:
					case ExcelColumnFormat.Time:
					case ExcelColumnFormat.DateAndTime:

						// Get the range of values in the column.

						Double dMinimumCellValue, dMaximumCellValue;

						if ( TryGetNumericRange(worksheetName, oColumn,
							out dMinimumCellValue, out dMaximumCellValue) )
						{
							if (eColumnFormat == ExcelColumnFormat.Number)
							{
								oDynamicFilterParameters.AddLast(
									new NumericFilterParameters(oColumn.Name,
										dMinimumCellValue,
										dMaximumCellValue,
                                        GetDecimalPlaces(oColumn) ) );
							}
							else
							{
								oDynamicFilterParameters.AddLast(
									new DateTimeFilterParameters(oColumn.Name,
										dMinimumCellValue, dMaximumCellValue,
										eColumnFormat) );
							}
						}

						break;

					case ExcelColumnFormat.Other:

						// Skip the column.

						break;

					default:

						Debug.Assert(false);
						break;
				}
			}
		}

		return (oDynamicFilterParameters);
    }

    //*************************************************************************
    //  Method: ColumnShouldBeExcluded()
    //
    /// <summary>
	/// Determines whether a table column should be excluded.
    /// </summary>
	///
    /// <param name="oColumn">
	/// The table column to check.
    /// </param>
	///
	/// <returns>
	/// true if the column should be excluded.
	/// </returns>
    //*************************************************************************

    private static Boolean
    ColumnShouldBeExcluded
	(
		ListColumn oColumn
	)
    {
		Debug.Assert(oColumn != null);

		switch (oColumn.Name)
		{
			case CommonTableColumnNames.ID:

				// It makes no sense to filter on the NodeXL-generated ID
				// column.

				return (true);

			default:

				break;
		}

		Range oColumnData = oColumn.DataBodyRange;

		// Exclude columns with no data or with an empty first data cell.

		if (
			oColumnData == null
			||
			oColumnData.Rows.Count < 1
			||
			!(oColumnData.Cells[1, 1] is Range)
			||
			( (Range)oColumnData.Cells[1, 1] ).get_Value(Missing.Value) == null
			)
		{
			return (true);
		}

		return (false);
    }

    //*************************************************************************
    //  Method: GetColumnFormat()
    //
    /// <summary>
	/// Determines the format of a table column.
    /// </summary>
	///
    /// <param name="oColumn">
	/// The table column to check.
    /// </param>
	///
	/// <returns>
	/// The column format.
	/// </returns>
    //*************************************************************************

    private static ExcelColumnFormat
    GetColumnFormat
	(
		ListColumn oColumn
	)
    {
		Debug.Assert(oColumn != null);

		Range oColumnData = oColumn.DataBodyRange;

		Debug.Assert(oColumnData != null);
		Debug.Assert(oColumnData.Rows.Count > 0);

		// Look at the type of the value in the first cell.

		Debug.Assert(oColumnData.Cells[1, 1] is Range);

		Range oFirstDataCell = (Range)oColumnData.Cells[1, 1];
		Object oFirstDataCellValue = oFirstDataCell.get_Value(Missing.Value);

		Debug.Assert(oFirstDataCellValue != null);

		if (oFirstDataCellValue is DateTime)
		{
			if ( CellContainsTime(oFirstDataCell) )
			{
				// Sample: 1/1/2008 3:40 pm.

				return (ExcelColumnFormat.DateAndTime);
			}
			else
			{
				// Sample: 1/1/2008.

				return (ExcelColumnFormat.Date);
			}
		}
		else if (oFirstDataCellValue is Double)
		{
			// Cells formatted as a time are returned as Double.  Another test
			// is required to distinguish times from real Doubles.

			if ( CellContainsTime(oFirstDataCell) )
			{
				// Sample: 3:40 pm.

				return (ExcelColumnFormat.Time);
			}
			else
			{
				// Sample: 123.

				return (ExcelColumnFormat.Number);
			}
		}
		else
		{
			return (ExcelColumnFormat.Other);
		}
    }

    //*************************************************************************
    //  Method: GetDecimalPlaces()
    //
    /// <summary>
	/// Gets the number of decimal places displayed in a table column of format
	/// ExcelColumnFormat.Number.
    /// </summary>
	///
    /// <param name="oColumn">
	/// The table column to check.  The column must be of format
	/// ExcelColumnFormat.Number.
    /// </param>
	///
	/// <returns>
	/// The number of decimal places displayed in <paramref name="oColumn" />.
	/// </returns>
    //*************************************************************************

    private static Int32
    GetDecimalPlaces
	(
		ListColumn oColumn
	)
    {
		Debug.Assert(oColumn != null);

		Range oColumnData = oColumn.DataBodyRange;

		Debug.Assert(oColumnData != null);
		Debug.Assert(oColumnData.Rows.Count > 0);

		// It would be nice if there were a Range.DecimalPlaces property, but
		// there isn't.  All that Excel provides is Range.NumberFormat, which
		// is actually a string that needs to be parsed.  Parsing that is
		// guaranteed to be correct is difficult, because NumberFormat can be
		// simple ("0.00") or complicated ("$#,##0.00_);[Red]($#,##0.00)").  As
		// an approximation that will be correct most of the time (for "0.00",
		// for example), count the characters after the last decimal place.
		//
		// Note: Don't use the Text property and count decimal places in that,
		// because Range.Text can be "###" if the column is too narrow.

		Debug.Assert(oColumnData.Cells[1, 1] is Range);

		Range oFirstDataCell = (Range)oColumnData.Cells[1, 1];

		String sFirstDataCellNumberFormat =
			(String)oFirstDataCell.NumberFormat;

		Int32 iIndexOfLastDecimalPoint =
			sFirstDataCellNumberFormat.LastIndexOf('.');

		if (iIndexOfLastDecimalPoint < 0)
		{
			return (0);
		}

		Int32 iDecimalPlaces =
			sFirstDataCellNumberFormat.Length - iIndexOfLastDecimalPoint - 1;

		Debug.Assert(iDecimalPlaces >= 0);

		return (iDecimalPlaces);
    }

    //*************************************************************************
    //  Method: CellContainsTime()
    //
    /// <summary>
	/// Determines whether a cell contains a time.
    /// </summary>
	///
    /// <param name="oCell">
	/// The cell to check.
    /// </param>
	///
	/// <returns>
	/// true if the cell contains a time.
	/// </returns>
    //*************************************************************************

    private static Boolean
    CellContainsTime
	(
		Range oCell
	)
    {
		Debug.Assert(oCell != null);

		// The easiest (but not perfect) test is to check the number format for
		// hours.

		Object oNumberFormat = oCell.NumberFormat;

		Debug.Assert(oNumberFormat is String);

		return ( ( (String)oCell.NumberFormat ).Contains("h") );
    }

    //*************************************************************************
    //  Method: TryGetNumericRange()
    //
    /// <summary>
	/// Attempts to get the range of values in a numeric column.
    /// </summary>
	///
    /// <param name="sWorksheetName">
	/// Name of the worksheet containing the table.
    /// </param>
	///
    /// <param name="oColumn">
	/// The numeric column.
    /// </param>
	///
    /// <param name="dMinimumCellValue">
	/// Where the minimum value in the column gets stored if true is returned.
    /// </param>
	///
    /// <param name="dMaximumCellValue">
	/// Where the maximum value in the column gets stored if true is returned.
    /// </param>
	///
	/// <returns>
	/// true if the column contains a range of numeric values.
	/// </returns>
    //*************************************************************************

    private static Boolean
    TryGetNumericRange
	(
		String sWorksheetName,
		ListColumn oColumn,
		out Double dMinimumCellValue,
		out Double dMaximumCellValue
	)
    {
		Debug.Assert( !String.IsNullOrEmpty(sWorksheetName) );
		Debug.Assert(oColumn != null);
		Debug.Assert(oColumn.DataBodyRange != null);

		Application oApplication = oColumn.Application;

		String sFunctionCall = String.Format(

			"=MIN({0}!{1})"
			,
			sWorksheetName,
			ExcelUtil.GetRangeAddress(oColumn.DataBodyRange)
			);

		dMinimumCellValue = (Double)oApplication.Evaluate(sFunctionCall);

		sFunctionCall = sFunctionCall.Replace("MIN", "MAX");

		dMaximumCellValue = (Double)oApplication.Evaluate(sFunctionCall);

		return (dMaximumCellValue > dMinimumCellValue);
	}


	#if false  // For testing.

    //*************************************************************************
    //  Method: GetRandomDynamicFilterParameters()
    //
    /// <summary>
	/// Gets a random collection of dynamic filter parameters for testing.
    /// </summary>
	///
    /// <param name="sTableName">
	/// Name of the table to get filter parameters for.
    /// </param>
	///
	/// <returns>
	/// A collection of random <see cref="DynamicFilterParameters" /> objects
	/// for testing.
	/// </returns>
    //*************************************************************************

    private static ICollection<DynamicFilterParameters>
    GetRandomDynamicFilterParameters
	(
		String sTableName
	)
    {
		Debug.Assert( !String.IsNullOrEmpty(sTableName) );

		LinkedList<DynamicFilterParameters> oDynamicFilterParameters = 
			new LinkedList<DynamicFilterParameters>();

		Random oRandom = new Random();

		Int32 iDynamicFilters = oRandom.Next(40);

		for (Int32 i = 0; i < iDynamicFilters; i++)
		{
			String sColumnName = sTableName + " "
				+ new String('A', oRandom.Next(100) );

			Double dMinimumCellValue = -1000 + oRandom.NextDouble() * (20000);

			Double dMaximumCellValue =
				dMinimumCellValue + oRandom.NextDouble() * (20000);

			oDynamicFilterParameters.AddLast(
				new NumericFilterParameters(sColumnName, dMinimumCellValue,
					dMaximumCellValue) );
		}

		return (oDynamicFilterParameters);
    }

	#endif
}

}
