

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//	Class: GraphMetricWriter
//
/// <summary>
///	Writes GraphMetricColumn objects to the workbook.
/// </summary>
///
/// <remarks>
/// Call <see cref="WriteGraphMetricColumnsToWorkbook" /> to write one or more
/// <see cref="GraphMetricColumn" /> objects to a workbook.
/// </remarks>
//*****************************************************************************

public partial class GraphMetricWriter : Object
{
	//*************************************************************************
	//	Constructor: GraphMetricWriter()
	//
	/// <summary>
	///	Initializes a new instance of the <see cref="GraphMetricWriter" />
	/// class.
	/// </summary>
	//*************************************************************************

	public GraphMetricWriter()
	{
		// (Do nothing.)

		AssertValid();
	}

	//*************************************************************************
	//	Method: WriteGraphMetricColumnsToWorkbook()
	//
	/// <summary>
	///	Writes an array of GraphMetricColumn objects to the workbook.
	/// </summary>
	///
	/// <param name="graphMetricColumns">
	/// An array of GraphMetricColumn objects, one for each column of metrics
	/// that should be written to the workbook.
	/// </param>
	///
	/// <param name="workbook">
    /// Workbook containing the graph contents.
	/// </param>
	//*************************************************************************

	public void
	WriteGraphMetricColumnsToWorkbook
	(
		GraphMetricColumn [] graphMetricColumns,
        Microsoft.Office.Interop.Excel.Workbook workbook
	)
	{
		Debug.Assert(graphMetricColumns != null);
		Debug.Assert(workbook != null);
		AssertValid();

		// (Note: Don't sort grapMetricColumns by worksheet name/table name in
		// an effort to minimize worksheet switches in the code below.  That
		// would interfere with the column order specified by the
		// IGraphMetricCalculator implementations.

		// Create a dictionary of tables that have been cleared.  The key is
		// the worksheet name + table name, and the value isn't used.
		// (GraphMetricColumnOrdered columns require that the table be cleared
		// before any graph metric values are written to it.)

		Dictionary<String, Char> oClearedTables =
			new Dictionary<String, Char>();

		// Loop through the columns.

		String sCurrentWorksheetPlusTable = String.Empty;
		ListObject oTable = null;

		foreach (GraphMetricColumn oGraphMetricColumn in graphMetricColumns)
		{
			String sThisWorksheetPlusTable = oGraphMetricColumn.WorksheetName
				+ oGraphMetricColumn.TableName;

			if (sThisWorksheetPlusTable != sCurrentWorksheetPlusTable)
			{
				// This is a different table.  Get its ListObject.

				if ( !ExcelUtil.TryGetTable(workbook,
					oGraphMetricColumn.WorksheetName,
					oGraphMetricColumn.TableName, out oTable) )
				{
					// The table couldn't be found.

					continue;
				}

				sCurrentWorksheetPlusTable = sThisWorksheetPlusTable;
			}

			// Apparent Excel bug: Adding a column when the header row is not
			// the default row height increases the header row height.  Work
			// around this by saving the height and restoring it below.

            Double dHeaderRowHeight = (Double)oTable.HeaderRowRange.RowHeight;

			// Write the column.

			Debug.Assert(oTable != null);

			if (oGraphMetricColumn is GraphMetricColumnWithID)
			{
				WriteGraphMetricColumnWithIDToWorkbook(
					(GraphMetricColumnWithID)oGraphMetricColumn, oTable);
			}
			else if (oGraphMetricColumn is GraphMetricColumnOrdered)
			{
				if ( !oClearedTables.ContainsKey(sThisWorksheetPlusTable) )
				{
					// Clear the table.

					ExcelUtil.ClearTable(oTable);

					oClearedTables.Add(sThisWorksheetPlusTable, ' ');

					// Clear AutoFiltering, which interferes with writing an
					// ordered list to the column.

					ExcelUtil.ClearTableAutoFilters(oTable);
				}

				WriteGraphMetricColumnOrderedToWorkbook(
					(GraphMetricColumnOrdered)oGraphMetricColumn, oTable);
			}
			else
			{
				Debug.Assert(false);
			}

            oTable.HeaderRowRange.RowHeight = dHeaderRowHeight;
		}
	}

	//*************************************************************************
	//	Method: WriteGraphMetricColumnWithIDToWorkbook()
	//
	/// <summary>
	///	Writes a GraphMetricColumnWithID object to the workbook.
	/// </summary>
	///
	/// <param name="oGraphMetricColumnWithID">
	/// The GraphMetricColumnWithID object to write to the workbook.
	/// </param>
	///
	/// <param name="oTable">
	/// The table containing the column.
	/// </param>
	//*************************************************************************

	protected void
	WriteGraphMetricColumnWithIDToWorkbook
	(
		GraphMetricColumnWithID oGraphMetricColumnWithID,
		ListObject oTable
	)
	{
		Debug.Assert(oGraphMetricColumnWithID != null);
		Debug.Assert(oTable != null);
		AssertValid();

		// Get the required column information.

		Range oVisibleColumnData, oIDColumnData;

		if ( !TryGetRequiredColumnWithIDInformation(oGraphMetricColumnWithID,
			oTable, out oVisibleColumnData, out oIDColumnData) )
		{
			return;
		}

		// Store the column's GraphMetricValueWithID objects in a dictionary.
		// The key is the GraphMetricValueWithID.RowID and the value is the
		// GraphMetricValueWithID.

		Dictionary<Int32, GraphMetricValueWithID> oIDDictionary =
			new Dictionary<Int32, GraphMetricValueWithID>();

		foreach (GraphMetricValueWithID oGraphMetricValueWithID in
			oGraphMetricColumnWithID.GraphMetricValuesWithID)
		{
			oIDDictionary.Add(oGraphMetricValueWithID.RowID,
				oGraphMetricValueWithID);
		}

		Debug.Assert(oTable.Parent is Worksheet);
		Worksheet oWorksheet = (Worksheet)oTable.Parent;

		Int32 iIDColumnNumberOneBased = oIDColumnData.Column;

		// Loop through the areas, and split each area into subranges if the
		// area contains too many rows.

		foreach ( Range oColumnSubrange in
			ExcelRangeSplitter.SplitRange(oVisibleColumnData) )
		{
			Int32 iRows = oColumnSubrange.Rows.Count;

			Range oIDColumnSubrange = ExcelRangeSplitter.GetParallelSubrange(
				oColumnSubrange, iIDColumnNumberOneBased);

			Debug.Assert(oIDColumnSubrange.Rows.Count == iRows);

			Object [,] aoColumnValues =
				ExcelUtil.GetSingleColumn2DArray(iRows);

			Object [,] aoIDColumnValues =
				ExcelUtil.GetRangeValues(oIDColumnSubrange);

			// Loop through the rows.

			for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
			{
				String sID;
				Int32 iID;

				// Get the ID stored in the row.

				if (
					!ExcelUtil.TryGetNonEmptyStringFromCell(
						aoIDColumnValues, iRowOneBased, 1, out sID)
					||
					!Int32.TryParse(sID, out iID)
				   )
				{
					continue;
				}

				// Is the ID one of the IDs specified within the
				// GraphMetricColumn object?

				GraphMetricValueWithID oGraphMetricValueWithID;

				if ( !oIDDictionary.TryGetValue(iID,
					out oGraphMetricValueWithID) )
				{
					// No.

					continue;
				}

				// Set the column cell in this row to the specified value.

				aoColumnValues[iRowOneBased, 1] =
					oGraphMetricValueWithID.Value;
			}

			oColumnSubrange.set_Value(Missing.Value, aoColumnValues);
		}
	}

	//*************************************************************************
	//	Method: WriteGraphMetricColumnOrderedToWorkbook()
	//
	/// <summary>
	///	Writes a GraphMetricColumnOrdered object to the workbook.
	/// </summary>
	///
	/// <param name="oGraphMetricColumnOrdered">
	/// The GraphMetricColumnOrdered object to write to the workbook.
	/// </param>
	///
	/// <param name="oTable">
	/// The table containing the column.
	/// </param>
	//*************************************************************************

	protected void
	WriteGraphMetricColumnOrderedToWorkbook
	(
		GraphMetricColumnOrdered oGraphMetricColumnOrdered,
		ListObject oTable
	)
	{
		Debug.Assert(oGraphMetricColumnOrdered != null);
		Debug.Assert(oTable != null);
		AssertValid();

		Range oVisibleColumnData;

		// Get the specified column.

		if ( !TryGetRequiredColumnInformation(oGraphMetricColumnOrdered,
			oTable, out oVisibleColumnData) )
		{
			return;
		}

		// Copy the graph metric values to an array.

		GraphMetricValueOrdered [] aoGraphMetricValuesOrdered =
			oGraphMetricColumnOrdered.GraphMetricValuesOrdered;

		Int32 iRows = aoGraphMetricValuesOrdered.Length;
		Object [,] aoValues = ExcelUtil.GetSingleColumn2DArray(iRows);
		Boolean bStyleSpecified = false;

		for (Int32 i = 0; i < iRows; i++)
		{
			GraphMetricValueOrdered oGraphMetricValueOrdered =
				aoGraphMetricValuesOrdered[i];

			aoValues[i + 1, 1] = oGraphMetricValueOrdered.Value;

			if (oGraphMetricValueOrdered.Style != null)
			{
				// A style was specified for this row.  It will need to be
				// applied later.  (It should be possible to apply the style
				// here, but that does not work reliably when values are
				// written to the cells afterwards, as they are after this
				// loop.)

				bStyleSpecified = true;
			}
		}

		// Write the array to the column.

		ExcelUtil.SetRangeValues(oVisibleColumnData, aoValues);

		if (bStyleSpecified)
		{
			for (Int32 i = 0; i < iRows; i++)
			{
				String sStyle = aoGraphMetricValuesOrdered[i].Style;

				if (sStyle != null)
				{
					// This row has a style.  Apply it.

					ExcelUtil.SetRangeStyle(
						(Range)oVisibleColumnData.Cells[i + 1, 1], sStyle);
				}
			}
		}
	}

	//*************************************************************************
	//	Method: TryGetRequiredColumnWithIDInformation()
	//
	/// <summary>
	///	Gets the column information required to write a GraphMetricColumnWithID
	/// object to the workbook.
	/// </summary>
	///
	/// <param name="oGraphMetricColumnWithID">
	/// The GraphMetricColumnWithID object to write to the workbook.
	/// </param>
	///
	/// <param name="oTable">
	/// The table containing the column.
	/// </param>
	///
	/// <param name="oVisibleColumnData">
	/// Where the visible range of the specified column gets stored if true is
	/// returned.
	/// </param>
	///
	/// <param name="oIDColumnData">
	/// Where the ID column gets stored if true is returned.  The column may
	/// contain hidden rows.
	/// </param>
	///
	/// <returns>
	/// true if the column information was obtained.
	/// </returns>
	//*************************************************************************

	protected Boolean
	TryGetRequiredColumnWithIDInformation
	(
		GraphMetricColumnWithID oGraphMetricColumnWithID,
		ListObject oTable,
		out Range oVisibleColumnData,
		out Range oIDColumnData
	)
	{
		Debug.Assert(oGraphMetricColumnWithID != null);
		Debug.Assert(oTable != null);
		AssertValid();

		oVisibleColumnData = null;
		oIDColumnData = null;

		// Get the specified column.

		if ( !TryGetRequiredColumnInformation(oGraphMetricColumnWithID, oTable,
			out oVisibleColumnData) )
		{
			return (false);
		}

		// Get the ID column.

		if ( !ExcelUtil.TryGetTableColumnData(oTable,
			CommonTableColumnNames.ID, out oIDColumnData) )
		{
			return (false);
		}

		return (true);
	}

	//*************************************************************************
	//	Method: TryGetRequiredColumnInformation()
	//
	/// <summary>
	///	Gets the column information required to write a GraphMetricColumn
	/// object to the workbook.
	/// </summary>
	///
	/// <param name="oGraphMetricColumn">
	/// The GraphMetricColumn object to write to the workbook.
	/// </param>
	///
	/// <param name="oTable">
	/// The table containing the column.
	/// </param>
	///
	/// <param name="oVisibleColumnData">
	/// Where the visible range of the specified column gets stored if true is
	/// returned.
	/// </param>
	///
	/// <returns>
	/// true if the column information was obtained.
	/// </returns>
	//*************************************************************************

	protected Boolean
	TryGetRequiredColumnInformation
	(
		GraphMetricColumn oGraphMetricColumn,
		ListObject oTable,
		out Range oVisibleColumnData
	)
	{
		Debug.Assert(oGraphMetricColumn != null);
		Debug.Assert(oTable != null);
		AssertValid();

		oVisibleColumnData = null;

		// Add the specified column if it's not already present.

		String sColumnName = oGraphMetricColumn.ColumnName;
		String sColumnStyle = oGraphMetricColumn.Style;

		Microsoft.Office.Interop.Excel.ListColumn oColumn;

		if (
			!ExcelUtil.TryGetTableColumn(oTable, sColumnName, out oColumn)
			&&
			!ExcelUtil.TryAddTableColumn(oTable, sColumnName,
				oGraphMetricColumn.ColumnWidthChars, sColumnStyle, out oColumn)
			)
		{
			// Give up.

			return (false);
		}

		Range oColumnData;

		if ( !ExcelUtil.TryGetTableColumnData(oTable, sColumnName,
			out oColumnData) )
		{
			return (false);
		}

		ExcelUtil.SetRangeStyle(oColumn.Range, sColumnStyle);

		String sNumberFormat = oGraphMetricColumn.NumberFormat;

		if (sNumberFormat != null)
		{
			oColumnData.NumberFormat = sNumberFormat;
		}

		// Wrapping text makes Range.set_Value() very slow, so turn it off.

		oColumn.Range.WrapText = false;

		// But wrap the text in the column's header.

		( (Range)oColumn.Range.Cells[1, 1] ).WrapText = true;

		// Get the visible range.

		if ( !ExcelUtil.TryGetVisibleRange(oColumnData,
			out oVisibleColumnData) )
		{
			return (false);
		}

		return (true);
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
		// (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}
}
