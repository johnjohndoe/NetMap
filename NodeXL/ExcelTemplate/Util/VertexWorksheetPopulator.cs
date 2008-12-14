
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexWorksheetPopulator
//
/// <summary>
/// Populates the vertex worksheet with the name of each unique vertex in the
/// edge worksheet.
/// </summary>
///
/// <remarks>
/// Use <see cref="PopulateVertexWorksheet" /> to populate the vertex
/// worksheet.
/// </remarks>
//*****************************************************************************

public class VertexWorksheetPopulator : Object
{
    //*************************************************************************
    //  Constructor: VertexWorksheetPopulator()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="VertexWorksheetPopulator" /> class.
    /// </summary>
    //*************************************************************************

    public VertexWorksheetPopulator()
    {
        // (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Method: PopulateVertexWorksheet()
    //
    /// <summary>
	/// Populates the vertex worksheet with the name of each unique vertex in
	/// the edge worksheet.
    /// </summary>
    ///
    /// <param name="workbook">
	/// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="activateVertexWorksheetWhenDone">
	/// true to activate the vertex worksheet after it is populated.
    /// </param>
	///
	/// <returns>
	/// The vertex table.
	/// </returns>
    ///
    /// <remarks>
	/// This method reads the graph's unique vertex names from the edge
	/// worksheet and populates the vertex worksheet with them.  If a vertex
	/// name already exists in the vertex worksheet, it is not added again.
	///
	/// <para>
	/// If a workbook format error is encountered, a <see
	/// cref="WorkbookFormatException" /> is thrown.  A missing vertex
	/// worksheet or table is considered a workbook format error.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    public Microsoft.Office.Interop.Excel.ListObject
    PopulateVertexWorksheet
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
		Boolean activateVertexWorksheetWhenDone
    )
    {
		Debug.Assert(workbook != null);
        AssertValid();

		// Get the required tables.

		ListObject oEdgeTable;
		ListObject oVertexTable;

		GetRequiredTables(workbook, out oEdgeTable, out oVertexTable);

		// Create a dictionary to keep track of vertices.  The key is the
		// vertex name and the value is unused.

		Dictionary<String, Char> oVertexNameDictionary =
			new Dictionary<String, Char>();

		// Populate the dictionary with unique vertex names from the edge
		// table.

		ReadEdgeTable(oEdgeTable, oVertexNameDictionary);

		if (oVertexNameDictionary.Count > 0)
		{
			// Fill the vertex table with the unique vertex names.

			FillVertexTable(oVertexTable, oVertexNameDictionary);
		}

		if (activateVertexWorksheetWhenDone)
		{
			ExcelUtil.ActivateWorksheet(oVertexTable);
		}

		return (oVertexTable);
    }

    //*************************************************************************
    //  Method: GetRequiredTables()
    //
    /// <summary>
	/// Gets the tables required for populating the vertex worksheet.
    /// </summary>
    ///
    /// <param name="oWorkbook">
	/// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="oEdgeTable">
	/// Where the edge table gets stored.
    /// </param>
    ///
    /// <param name="oVertexTable">
	/// Where the vertex table gets stored.
    /// </param>
    ///
	/// <remarks>
	/// This method checks for tables and table columns that are required for
	/// vertex worksheet population.
	///
	/// <para>
	/// If there is a problem with the workbook, a <see
	/// cref="WorkbookFormatException" /> is thrown.
	/// </para>
	///
	/// </remarks>
    //*************************************************************************

    protected void
    GetRequiredTables
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
		out ListObject oEdgeTable,
		out ListObject oVertexTable
    )
    {
		Debug.Assert(oWorkbook != null);
        AssertValid();

		// Get the required table that contains edge data.  GetEdgeTable()
		// checks for the required vertex name columns.

		EdgeWorksheetReader oEdgeWorksheetReader = new EdgeWorksheetReader();

		oEdgeTable = oEdgeWorksheetReader.GetEdgeTable(oWorkbook);

		// Normally, the vertex table isn't required, but to avoid having to
		// create the table in code if it's missing, require it here.

		if ( ExcelUtil.TryGetTable(oWorkbook, WorksheetNames.Vertices,
			TableNames.Vertices, out oVertexTable) )
		{
			// Get the indexes of the columns within the table.

			VertexWorksheetReader oVertexWorksheetReader =
				new VertexWorksheetReader();

			VertexWorksheetReader.VertexTableColumnIndexes
				oVertexTableColumnIndexes =
					oVertexWorksheetReader.GetVertexTableColumnIndexes(
						oVertexTable);

			// Make sure the vertex name column exists.

			if (oVertexTableColumnIndexes.VertexName ==
				WorksheetReaderBase.NoSuchColumn)
			{
				oVertexTable = null;
			}
		}
		else
		{
			oVertexTable = null;
		}

		if (oVertexTable == null)
		{
			throw new WorkbookFormatException(String.Format(

				"To use this feature, there must be a worksheet named \"{0}\""
				+ " that contains a table named \"{1}\", and that table must"
				+ " contain a column named \"{2}\"."
				+ "\r\n\r\n"
				+ "{3}"
				,
				WorksheetNames.Vertices,
				TableNames.Vertices,
				VertexTableColumnNames.VertexName,
				ErrorUtil.GetTemplateMessage()
				) );
		}
    }

    //*************************************************************************
    //  Method: ReadEdgeTable()
    //
    /// <summary>
	/// Reads the edge table and populates a dictionary with unique vertex
	/// names.
    /// </summary>
    ///
    /// <param name="oEdgeTable">
	/// Edge table.
    /// </param>
    ///
    /// <param name="oVertexNameDictionary">
	/// Dictionary to populate.  The key is the vertex name and the value is
	/// not used.
    /// </param>
    //*************************************************************************

    protected void
    ReadEdgeTable
    (
		ListObject oEdgeTable,
		Dictionary<String, Char> oVertexNameDictionary
    )
    {
		Debug.Assert(oEdgeTable != null);
		Debug.Assert(oVertexNameDictionary != null);
		Debug.Assert(oVertexNameDictionary.Count == 0);
        AssertValid();

		// Get the vertex name column ranges.

		Range oVertex1NameRange, oVertex2NameRange;
		
		if ( !ExcelUtil.TryGetTableColumnData(oEdgeTable,
				EdgeTableColumnNames.Vertex1Name, out oVertex1NameRange)
			||
			!ExcelUtil.TryGetTableColumnData(oEdgeTable,
				EdgeTableColumnNames.Vertex2Name, out oVertex2NameRange)
			)
		{
			return;
		}

		Int32 iRows = oVertex1NameRange.Rows.Count;

		Debug.Assert(oVertex2NameRange.Rows.Count == iRows);

		// Read the vertex names all at once.

		Object [,] aoVertex1NameValues =
			ExcelUtil.GetRangeValues(oVertex1NameRange);

		Object [,] aoVertex2NameValues =
			ExcelUtil.GetRangeValues(oVertex2NameRange);

		// Loop through the edges.

		for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
		{
			// Get the vertex names and add them to the dictionary.  Use
			// Dictionary.Item() instead of Dictionary.Add() to allow for
			// duplicate vertex names.

			String sVertex1Name, sVertex2Name;

			if ( ExcelUtil.TryGetNonEmptyStringFromCell(aoVertex1NameValues,
				iRowOneBased, 1, out sVertex1Name) )
			{
				oVertexNameDictionary[sVertex1Name] = ' ';
			}

			if ( ExcelUtil.TryGetNonEmptyStringFromCell(aoVertex2NameValues,
				iRowOneBased, 1, out sVertex2Name) )
			{
				oVertexNameDictionary[sVertex2Name] = ' ';
			}
		}
    }

    //*************************************************************************
    //  Method: FillVertexTable()
    //
    /// <summary>
	/// Fills in the vertex name column with specified vertex names.
    /// </summary>
    ///
    /// <param name="oVertexTable">
	/// Vertex table.
    /// </param>
    ///
    /// <param name="oVertexNameDictionary">
	/// Dictionary of vertex names.  The key is the vertex name and the value
	/// is unused.  IMPORTANT: This method might remove some entries from the
	/// dictionary.
    /// </param>
	///
    /// <remarks>
	/// IMPORTANT: This method might remove some entries from the dictionary.
    /// </remarks>
    //*************************************************************************

	protected void
	FillVertexTable
	(
		ListObject oVertexTable,
		Dictionary<String, Char> oVertexNameDictionary
	)
	{
		Debug.Assert(oVertexTable != null);
		Debug.Assert(oVertexNameDictionary != null);
		AssertValid();

		// There may already be some vertex names in the table.  For each
		// existing name, remove the name from the dictionary.

		Int32 iExistingRows = 0;

		Range oVertexNameRange;
		
		if ( ExcelUtil.TryGetTableColumnData(oVertexTable,
			VertexTableColumnNames.VertexName, out oVertexNameRange) )
		{
			iExistingRows = oVertexNameRange.Rows.Count;

			// Read the vertex names all at once.

			Object [,] aoVertexNameValues =
				ExcelUtil.GetRangeValues(oVertexNameRange);

			// Loop through the vertices.

			for (Int32 iRowOneBased = 1; iRowOneBased <= iExistingRows;
				iRowOneBased++)
			{
				// Get the vertex name and remove it from the dictionary.

				String sVertexName;

				if ( ExcelUtil.TryGetNonEmptyStringFromCell(
						aoVertexNameValues, iRowOneBased, 1, out sVertexName) )
				{
					oVertexNameDictionary.Remove(sVertexName);
				}
			}
		}

		// Now create an array for the vertices that remain in the dictionary.
		// These are vertices that were in the edge table but not the vertex
		// table.

		Int32 iRowsToAdd = oVertexNameDictionary.Count;

		if (iRowsToAdd == 0)
		{
			return;
		}

		String [,] asAddedVertexNameValues = new String [iRowsToAdd, 1];

		Int32 iIndex = 0;

		foreach (KeyValuePair<String, Char> oKeyValuePair in
			oVertexNameDictionary)
		{
            asAddedVertexNameValues[iIndex, 0] = oKeyValuePair.Key;

			iIndex++;
		}

		// The table may be empty or contain empty rows.  If so, the remaining
		// vertices should be appended after the last non-empty row.

		Int32 iLastNonEmptyRowOneBased;

        Range oDataBodyRange = oVertexTable.DataBodyRange;

		if (
			oDataBodyRange == null
			||
			!ExcelUtil.TryGetLastNonEmptyRow(oDataBodyRange,
				out iLastNonEmptyRowOneBased)
			)
		{
			// There were no non-empty data rows in the table.  Use an offset
			// of 1 from the header row instead.

            oDataBodyRange = oVertexTable.HeaderRowRange;
            iExistingRows = 1;
		}
		else
		{
			iExistingRows = iLastNonEmptyRowOneBased - oDataBodyRange.Row + 1;
		}

		// Get the index of the vertex name column.

		ListColumn oVertexNameColumn;

		if ( !ExcelUtil.TryGetTableColumn(oVertexTable,
			VertexTableColumnNames.VertexName, out oVertexNameColumn) )
		{
			// This can't happen, because GetRequiredTables() has
			// verified that the column exists.

			Debug.Assert(false);
		}

		Int32 iVertexNameColumnIndexOneBased = oVertexNameColumn.Index;

		Debug.Assert(oVertexTable.Parent is Worksheet);

		Worksheet oVertexWorksheet = (Worksheet)oVertexTable.Parent;

		Range oAddedVertexNameRange = oVertexWorksheet.get_Range(

			oDataBodyRange.Cells[iExistingRows + 1,
				iVertexNameColumnIndexOneBased],

			oDataBodyRange.Cells[iExistingRows + iRowsToAdd,
				iVertexNameColumnIndexOneBased]
			);

		oAddedVertexNameRange.set_Value(
			Missing.Value, asAddedVertexNameValues);
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
