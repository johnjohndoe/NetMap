

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: Sheet2
//
/// <summary>
/// Represents the vertex worksheet.
/// </summary>
//*****************************************************************************

public partial class Sheet2
{
    //*************************************************************************
    //  Method: GetSelectedVertexNames()
    //
    /// <summary>
	/// Gets an array of vertex names for all rows in the table that have at
	/// least one cell selected.
    /// </summary>
    ///
	/// <param name="oSelectedRange">
	/// Range that contains the selected cells in the table.  The range may
	/// contain multiple areas.
	/// </param>
	///
	/// <returns>
	/// An array of vertex names.
	/// </returns>
    //*************************************************************************

    public String []
	GetSelectedVertexNames
	(
		Microsoft.Office.Interop.Excel.Range oSelectedRange
	)
    {
		Debug.Assert(oSelectedRange != null);
		AssertValid();

		return ( CollectionUtil.DictionaryKeysToArray<String, Char>(
			m_oSheets1And2Helper.GetSelectedColumnValues(oSelectedRange,
				VertexTableColumnNames.VertexName)
			) );
    }

	//*************************************************************************
	//	Event: VertexSelectionChanged
	//
	/// <summary>
	///	Occurs when the selection state of the vertex table changes.
	/// </summary>
	//*************************************************************************

	public event TableSelectionChangedEventHandler VertexSelectionChanged;


	//*************************************************************************
	//	Method: OnGraphDrawn()
	//
	/// <summary>
	/// Handles the GraphDrawn event on ThisWorkbook.
	/// </summary>
	///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
	//*************************************************************************

	private void
	OnGraphDrawn
	(
		GraphDrawnEventArgs e
	)
	{
		Debug.Assert(e != null);
		AssertValid();

		// This method gathers some necessary information and returns without
		// doing anything else if some of the information is missing.

		Microsoft.Office.Interop.Excel.ListObject oVertexTable =
			Vertices.InnerObject;

		// Get the ID column data from the vertex table.  This excludes the
		// header.

		Microsoft.Office.Interop.Excel.Range oIDColumnData;

		if ( !ExcelUtil.TryGetTableColumnData(oVertexTable,
			CommonTableColumnNames.ID, out oIDColumnData) )
		{
			return;
		}

		// Get the location columns from the vertex table.  These are the
		// entire table columns, including the headers.

		Microsoft.Office.Interop.Excel.ListColumn oXColumn, oYColumn;

		if (
			!ExcelUtil.TryGetTableColumn(oVertexTable,
				VertexTableColumnNames.X, out oXColumn)
			||
			!ExcelUtil.TryGetTableColumn(oVertexTable,
				VertexTableColumnNames.Y, out oYColumn)
			)
		{
			return;
		}

		// The vertex table may be filtered.  Get the visible range, which may
		// contain multiple areas.

		Microsoft.Office.Interop.Excel.Range oDataBodyRange =
			Vertices.DataBodyRange;

		if (oDataBodyRange == null)
		{
			return;
		}

		Microsoft.Office.Interop.Excel.Range oVisibleDataBodyRange;

		if ( !ExcelUtil.TryGetVisibleRange(oDataBodyRange,
			out oVisibleDataBodyRange) )
		{
			return;
		}

		// The necessary information has been gathered.  Move on to the actual
		// work.  Activate this worksheet first, because writing to an inactive
        // worksheet causes problems with the selection in Excel.

		ExcelActiveWorksheetRestorer oExcelActiveWorksheetRestorer =
			GetExcelActiveWorksheetRestorer();

		ExcelActiveWorksheetState oExcelActiveWorksheetState =
			oExcelActiveWorksheetRestorer.ActivateWorksheet(this.InnerObject);

		try
		{
			OnGraphDrawn(e, oVertexTable, oDataBodyRange, oVisibleDataBodyRange,
				oIDColumnData, oXColumn, oYColumn);
		}
		finally
		{
			oExcelActiveWorksheetRestorer.Restore(oExcelActiveWorksheetState);
		}
	}

	//*************************************************************************
	//	Method: OnGraphDrawn()
	//
	/// <summary>
	/// Handles the GraphDrawn event on ThisWorkbook.
	/// </summary>
	///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
	///
	/// <param name="oVertexTable">
	/// Vertex table.
	/// </param>
	///
	/// <param name="oDataBodyRange">
	/// Data body range within the vertex table.
	/// </param>
	///
	/// <param name="oVisibleDataBodyRange">
	/// Visible data body range, which may contain multiple areas.
	/// </param>
	///
	/// <param name="oIDColumnData">
	/// ID column data from the vertex table.  This excludes the header.
	/// </param>
	///
	/// <param name="oXColumn">
	/// X-coordinate column from the vertex table.  This is the entire column,
	/// including the header.
	/// </param>
	///
	/// <param name="oYColumn">
	/// Y-coordinate column from the vertex table.  This is the entire column,
	/// including the header.
	/// </param>
	//*************************************************************************

	private void
	OnGraphDrawn
	(
		GraphDrawnEventArgs e,
		Microsoft.Office.Interop.Excel.ListObject oVertexTable,
		Microsoft.Office.Interop.Excel.Range oDataBodyRange,
		Microsoft.Office.Interop.Excel.Range oVisibleDataBodyRange,
		Microsoft.Office.Interop.Excel.Range oIDColumnData,
		Microsoft.Office.Interop.Excel.ListColumn oXColumn,
		Microsoft.Office.Interop.Excel.ListColumn oYColumn
	)
	{
		Debug.Assert(e != null);
		Debug.Assert(oVertexTable != null);
		Debug.Assert(oDataBodyRange != null);
		Debug.Assert(oVisibleDataBodyRange != null);
		Debug.Assert(oIDColumnData != null);
		Debug.Assert(oXColumn != null);
		Debug.Assert(oYColumn != null);
		AssertValid();

		// Read the ID column.

		Object [,] aoIDValues = ExcelUtil.GetRangeValues(oIDColumnData);

		// Read the lock column if it's available.  This excludes the header.
		// (Locked vertices should not have their locations changed.)

		Microsoft.Office.Interop.Excel.Range oLockedColumnData;
		Object [,] aoLockedValues = null;

		if ( ExcelUtil.TryGetTableColumnData(oVertexTable,
			VertexTableColumnNames.Locked, out oLockedColumnData) )
		{
			aoLockedValues = ExcelUtil.GetRangeValues(oLockedColumnData);
		}

		Int32 iDataBodyRangeRow = oDataBodyRange.Row;

		Int32 iXColumnOneBased = oXColumn.Range.Column;
		Int32 iYColumnOneBased = oYColumn.Range.Column;

		// Create an object that converts a vertex location between coordinates
		// used in the NodeXL graph and coordinates used in the worksheet.

		VertexLocationConverter oVertexLocationConverter =
			new VertexLocationConverter(e.GraphRectangle);

		// Loop through the areas in the visible range.

		foreach (Microsoft.Office.Interop.Excel.Range oVisibleDataArea in
			oVisibleDataBodyRange.Areas)
		{
			// Get the row numbers for the ID and lock columns.  (These are
			// with respect to the ID and lock values.)

			Int32 iFirstIDRowOneBased =
				oVisibleDataArea.Row - iDataBodyRangeRow + 1;

			Int32 iLastIDRowOneBased =
				iFirstIDRowOneBased + oVisibleDataArea.Rows.Count - 1;

			Int32 iRows = iLastIDRowOneBased - iFirstIDRowOneBased + 1;

			// Get the row numbers for the location values within the visible
			// area.  (These are with respect to the worksheet.)

			Int32 iFirstXYRowOneBased = oVisibleDataArea.Row;
			Int32 iLastXYRowOneBased = iFirstXYRowOneBased + iRows - 1;

			// Read the old location formulas within the visible area.  (To
			// avoid overwriting locked X,Y values that are formulas, read
			// formulas and not values.)

			Microsoft.Office.Interop.Excel.Range oXRange = this.get_Range(
				this.Cells[iFirstXYRowOneBased, iXColumnOneBased],
				this.Cells[iLastXYRowOneBased, iXColumnOneBased]
				);

			Microsoft.Office.Interop.Excel.Range oYRange = this.get_Range(
				this.Cells[iFirstXYRowOneBased, iYColumnOneBased],
				this.Cells[iLastXYRowOneBased, iYColumnOneBased]
				);

			Object [,] aoXFormulas = ExcelUtil.GetRangeFormulas(oXRange);
			Object [,] aoYFormulas = ExcelUtil.GetRangeFormulas(oYRange);

			for (Int32 iIDRowOneBased = iFirstIDRowOneBased;
				iIDRowOneBased <= iLastIDRowOneBased; iIDRowOneBased++)
			{
				if ( aoLockedValues != null &&
					VertexIsLocked(aoLockedValues, iIDRowOneBased) )
				{
					// The old location shouldn't be changed.

					continue;
				}

				// Get this row's ID.

				Int32 iID;

				if ( !m_oSheets1And2Helper.TryGetID(
					aoIDValues, iIDRowOneBased, out iID) )
				{
					// The user may have added a row since the workbook was
					// read into the graph.  Ignore this row.

					continue;
				}

				// Get the vertex object corresponding to the ID.

				IIdentityProvider oVertex;

				if ( !e.VertexIDDictionary.TryGetValue(iID, out oVertex) )
				{
					continue;
				}

				// Convert the vertex location to workbook coordinates.

				Single fWorkbookX, fWorkbookY;

				Debug.Assert(oVertex is IVertex);

				oVertexLocationConverter.GraphToWorkbook(
					( (IVertex)oVertex ).Location,
					out fWorkbookX, out fWorkbookY);

				// Store the vertex locations in the arrays.

				Int32 iXYFormulasRowOneBased =
					iIDRowOneBased - iFirstIDRowOneBased + 1;

				aoXFormulas[iXYFormulasRowOneBased, 1] = fWorkbookX;
				aoYFormulas[iXYFormulasRowOneBased, 1] = fWorkbookY;
			}

			// Set the new vertex locations in the worksheet.

			oXRange.Formula = aoXFormulas;
			oYRange.Formula = aoYFormulas;
		}
	}

	//*************************************************************************
	//	Method: OnVertexMoved()
	//
	/// <summary>
	/// Handles the VertexMoved event on ThisWorkbook.
	/// </summary>
	///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
	//*************************************************************************

	private void
	OnVertexMoved
	(
		VertexMovedEventArgs e
	)
	{
		Debug.Assert(e != null);
		AssertValid();

		Microsoft.Office.Interop.Excel.ListObject oVertexTable =
			Vertices.InnerObject;

		// Get the ID and location columns from the vertex table.  These are
		// the entire table columns, including the headers.

		Microsoft.Office.Interop.Excel.ListColumn
			oIDColumn, oXColumn, oYColumn;

		if (
			!ExcelUtil.TryGetTableColumn(oVertexTable,
				CommonTableColumnNames.ID, out oIDColumn)
			||
			!ExcelUtil.TryGetTableColumn(oVertexTable,
				VertexTableColumnNames.X, out oXColumn)
			||
			!ExcelUtil.TryGetTableColumn(oVertexTable,
				VertexTableColumnNames.Y, out oYColumn)
			)
		{
			return;
		}

		// Look for the cell in the ID column that contains the specified ID.

		Int32 iRowOneBased;

		if ( !m_oSheets1And2Helper.TryGetIDRow(oIDColumn, e.ID,
			out iRowOneBased) )
		{
			return;
		}

		// Create an object that converts a vertex location between coordinates
		// used in the NodeXL graph and coordinates used in the worksheet.

		VertexLocationConverter oVertexLocationConverter =
			new VertexLocationConverter(e.GraphRectangle);

		// Convert the vertex location to workbook coordinates.

		Single fWorkbookX, fWorkbookY;

		oVertexLocationConverter.GraphToWorkbook(
			e.Vertex.Location, out fWorkbookX, out fWorkbookY);

		// Update the X and Y cells.

		this.Cells[iRowOneBased, oXColumn.Range.Column] = fWorkbookX;
		this.Cells[iRowOneBased, oYColumn.Range.Column] = fWorkbookY;
	}

	//*************************************************************************
	//	Method: OnVertexAttributesEditedInGraph()
	//
	/// <summary>
	/// Handles the VertexAttributesEditedInGraph event on ThisWorkbook.
	/// </summary>
	///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
	//*************************************************************************

	private void
	OnVertexAttributesEditedInGraph
	(
		VertexAttributesEditedEventArgs e
	)
	{
		Debug.Assert(e != null);
		AssertValid();

		Microsoft.Office.Interop.Excel.ListObject oVertexTable =
			Vertices.InnerObject;

		// Get a dictionary containing the ID of each visible row in the table.

		Dictionary<Int32, Int32> oRowIDDictionary;

		if ( !m_oSheets1And2Helper.TryGetIDsOfVisibleRows(
			out oRowIDDictionary) )
		{
			// Nothing can be done without the IDs.

			return;
		}

		this.Application.Cursor =
			Microsoft.Office.Interop.Excel.XlMousePointer.xlWait;

		// Get the columns that might need to be updated.  These columns are
		// not required.

		Microsoft.Office.Interop.Excel.Range oColorColumnData,
			oShapeColumnData, oRadiusColumnData, oAlphaColumnData,
			oVertexDrawerPrecedenceColumnData, oVisibilityColumnData,
			oLockedColumnData, oMarkedColumnData;

		Object [,] aoColorValues = null;
		Object [,] aoShapeValues = null;
		Object [,] aoRadiusValues = null;
		Object [,] aoAlphaValues = null;
		Object [,] aoVertexDrawerPrecedenceValues = null;
		Object [,] aoVisibilityValues = null;
		Object [,] aoLockedValues = null;
		Object [,] aoMarkedValues = null;

		ExcelUtil.TryGetTableColumnDataAndValues(oVertexTable,
			VertexTableColumnNames.Color, out oColorColumnData,
			out aoColorValues);

		ExcelUtil.TryGetTableColumnDataAndValues(oVertexTable,
			VertexTableColumnNames.Shape, out oShapeColumnData,
			out aoShapeValues);

		ExcelUtil.TryGetTableColumnDataAndValues(oVertexTable,
			VertexTableColumnNames.Radius, out oRadiusColumnData,
			out aoRadiusValues);

		ExcelUtil.TryGetTableColumnDataAndValues(oVertexTable,
			VertexTableColumnNames.Alpha, out oAlphaColumnData,
			out aoAlphaValues);

		ExcelUtil.TryGetTableColumnDataAndValues(oVertexTable,
			VertexTableColumnNames.VertexDrawerPrecedence,
			out oVertexDrawerPrecedenceColumnData,
			out aoVertexDrawerPrecedenceValues);

		ExcelUtil.TryGetTableColumnDataAndValues(oVertexTable,
			VertexTableColumnNames.Visibility, out oVisibilityColumnData,
			out aoVisibilityValues);

		ExcelUtil.TryGetTableColumnDataAndValues(oVertexTable,
			VertexTableColumnNames.Locked, out oLockedColumnData,
			out aoLockedValues);

		if ( TryGetMarkedColumnData(out oMarkedColumnData) )
		{
			aoMarkedValues = ExcelUtil.GetRangeValues(oMarkedColumnData);
		}

		ColorConverter2 oColorConverter2 = new ColorConverter2();

		VertexShapeConverter oVertexShapeConverter =
			new VertexShapeConverter();

		VertexDrawerPrecedenceConverter oVertexDrawerPrecedenceConverter =
			new VertexDrawerPrecedenceConverter();

		VertexVisibilityConverter oVertexVisibilityConverter =
			new VertexVisibilityConverter();

		BooleanConverter oBooleanConverter = new BooleanConverter();

		// Loop through the IDs of the vertices whose attributes were edited
		// in the graph.

		EditedVertexAttributes oEditedVertexAttributes =
			e.EditedVertexAttributes;

		foreach (Int32 iID in e.VertexIDs)
		{
			// Look for the row that contains the ID.

			Int32 iRowOneBased;

			if ( !oRowIDDictionary.TryGetValue(iID, out iRowOneBased) )
			{
				continue;
			}

			iRowOneBased -= oVertexTable.Range.Row;

			if (oEditedVertexAttributes.Color.HasValue &&
				aoColorValues != null)
			{
				aoColorValues[iRowOneBased, 1] =
                    oColorConverter2.GraphToWorkbook(
					    oEditedVertexAttributes.Color.Value);
			}

			if (oEditedVertexAttributes.Shape.HasValue &&
				aoShapeValues != null)
			{
				aoShapeValues[iRowOneBased, 1] =
					oVertexShapeConverter.GraphToWorkbook(
						oEditedVertexAttributes.Shape.Value);
			}

			if (oEditedVertexAttributes.Radius.HasValue &&
				aoRadiusValues != null)
			{
				aoRadiusValues[iRowOneBased, 1] =
					oEditedVertexAttributes.Radius.Value.ToString();
			}

			if (oEditedVertexAttributes.Alpha.HasValue &&
				aoAlphaValues != null)
			{
				aoAlphaValues[iRowOneBased, 1] =
					oEditedVertexAttributes.Alpha.Value.ToString();
			}

			if (oEditedVertexAttributes.VertexDrawerPrecedence.HasValue &&
				aoVertexDrawerPrecedenceValues != null)
			{
				aoVertexDrawerPrecedenceValues[iRowOneBased, 1] =
					oVertexDrawerPrecedenceConverter.GraphToWorkbook(
						oEditedVertexAttributes.VertexDrawerPrecedence.Value);
			}

			if (oEditedVertexAttributes.Visibility.HasValue &&
				aoVisibilityValues != null)
			{
				aoVisibilityValues[iRowOneBased, 1] =
					oVertexVisibilityConverter.GraphToWorkbook(
						oEditedVertexAttributes.Visibility.Value);
			}

			if (oEditedVertexAttributes.Locked.HasValue &&
				aoLockedValues != null)
			{
				aoLockedValues[iRowOneBased, 1] =
					oBooleanConverter.GraphToWorkbook(
						oEditedVertexAttributes.Locked.Value);
			}

			if (oEditedVertexAttributes.Marked.HasValue &&
				aoMarkedValues != null)
			{
				aoMarkedValues[iRowOneBased, 1] =
					oBooleanConverter.GraphToWorkbook(
						oEditedVertexAttributes.Marked.Value);
			}
		}

		// Activate this worksheet first, because writing to an inactive
        // worksheet causes problems with the selection in Excel.

		ExcelActiveWorksheetRestorer oExcelActiveWorksheetRestorer =
			GetExcelActiveWorksheetRestorer();

		ExcelActiveWorksheetState oExcelActiveWorksheetState =
			oExcelActiveWorksheetRestorer.ActivateWorksheet(this.InnerObject);

		try
		{
			if (aoColorValues != null)
			{
				oColorColumnData.set_Value(Missing.Value, aoColorValues);
			}

			if (aoShapeValues != null)
			{
				oShapeColumnData.set_Value(Missing.Value, aoShapeValues);
			}

			if (aoRadiusValues != null)
			{
				oRadiusColumnData.set_Value(Missing.Value, aoRadiusValues);
			}

			if (aoAlphaValues != null)
			{
				oAlphaColumnData.set_Value(Missing.Value, aoAlphaValues);
			}

			if (aoVertexDrawerPrecedenceValues != null)
			{
				oVertexDrawerPrecedenceColumnData.set_Value(Missing.Value,
					aoVertexDrawerPrecedenceValues);
			}

			if (aoVisibilityValues != null)
			{
				oVisibilityColumnData.set_Value(Missing.Value,
					aoVisibilityValues);
			}

			if (aoLockedValues != null)
			{
				oLockedColumnData.set_Value(Missing.Value, aoLockedValues);
			}

			if (aoMarkedValues != null)
			{
				oMarkedColumnData.set_Value(Missing.Value, aoMarkedValues);
			}
		}
		finally
		{
			oExcelActiveWorksheetRestorer.Restore(oExcelActiveWorksheetState);
		}

		this.Application.Cursor =
			Microsoft.Office.Interop.Excel.XlMousePointer.xlDefault;
	}

    //*************************************************************************
    //  Method: TryGetMarkedColumnData()
    //
    /// <summary>
	/// Attempts to add a Marked column to the vertex table if it doesn't
	/// already exist.
    /// </summary>
    ///
    /// <param name="oMarkedColumnData">
	/// Where the column data gets stored if true is returned.
    /// </param>
	///
	/// <returns>
	/// true if the column was added.
	/// </returns>
    //*************************************************************************

	private Boolean
	TryGetMarkedColumnData
	(
		out Microsoft.Office.Interop.Excel.Range oMarkedColumnData
	)
	{
		AssertValid();

		Microsoft.Office.Interop.Excel.ListObject oVertexTable =
			Vertices.InnerObject;

		if ( !ExcelUtil.TryGetTableColumnData(oVertexTable,
			VertexTableColumnNames.Marked, out oMarkedColumnData) )
		{
			Microsoft.Office.Interop.Excel.ListColumn oMarkedColumn;

			if ( !ExcelUtil.TryAddTableColumn(oVertexTable,
					VertexTableColumnNames.Marked,
					ExcelUtil.AutoColumnWidth, null, out oMarkedColumn)
				||
				!ExcelUtil.TryGetTableColumnData(oVertexTable,
					VertexTableColumnNames.Marked, out oMarkedColumnData)
				)
			{
				return (false);
			}
		}

		return (true);
	}

    //*************************************************************************
    //  Method: VertexIsLocked()
    //
    /// <summary>
	/// Returns a flag indicating whether a vertex is locked.
    /// </summary>
    ///
    /// <param name="aoLockedValues">
	/// Values read from the vertex table's lock column.
    /// </param>
    ///
    /// <param name="iRowOneBased">
	/// One-based row index to check.
    /// </param>
	///
	/// <returns>
	/// true if the vertex on row <paramref name="iRowOneBased" /> is locked.
	/// </returns>
    //*************************************************************************

	private Boolean
	VertexIsLocked
	(
		Object [,] aoLockedValues,
		Int32 iRowOneBased
	)
	{
		Debug.Assert(aoLockedValues != null);
		Debug.Assert(iRowOneBased >= 1);
        AssertValid();

		String sLock;
		Boolean bLock;

		return (
			ExcelUtil.TryGetNonEmptyStringFromCell(aoLockedValues,
				iRowOneBased, 1, out sLock)
			&&
			( new BooleanConverter() ).TryWorkbookToGraph(sLock, out bLock)
			&&
			bLock
			);
	}

	//*************************************************************************
	//	Method: GetExcelActiveWorksheetRestorer()
	//
	/// <summary>
	/// Creates an ExcelActiveWorksheetRestorer object.
	/// </summary>
	///
	/// <remarks>
	/// The returned object can be used to activate this worksheet and later
	/// restore the previously active worksheet.
	/// </remarks>
	//*************************************************************************

	private ExcelActiveWorksheetRestorer
	GetExcelActiveWorksheetRestorer()
	{
		AssertValid();

		Debug.Assert(this.InnerObject.Parent is
			Microsoft.Office.Interop.Excel.Workbook);

		return ( new ExcelActiveWorksheetRestorer(
			(Microsoft.Office.Interop.Excel.Workbook)this.InnerObject.Parent)
			);
	}

    //*************************************************************************
    //  Method: FireVertexSelectionChanged()
    //
    /// <summary>
	/// Fires the <see cref="VertexSelectionChanged" /> event if appropriate.
    /// </summary>
    ///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

    private void
	FireVertexSelectionChanged
	(
		TableSelectionChangedEventArgs e
	)
    {
		Debug.Assert(e != null);
		AssertValid();

		TableSelectionChangedEventHandler oVertexSelectionChanged =
			this.VertexSelectionChanged;

		if (oVertexSelectionChanged != null)
		{
			try
			{
				oVertexSelectionChanged(this, e);
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
    //  Method: Sheet2_Startup()
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

	private void
	Sheet2_Startup
	(
		object sender,
		System.EventArgs e
	)
	{
		ThisWorkbook oThisWorkbook = Globals.ThisWorkbook;

		oThisWorkbook.GraphDrawn += new GraphDrawnEventHandler(
			this.ThisWorkbook_GraphDrawn);

		oThisWorkbook.VertexMoved += new VertexMovedEventHandler(
			this.ThisWorkbook_VertexMoved);

		oThisWorkbook.VertexAttributesEditedInGraph +=
			new VertexAttributesEditedEventHandler(
				this.ThisWorkbook_VertexAttributesEditedInGraph);

		// Create the object that does most of the work for this class.

        m_oSheets1And2Helper = new Sheets1And2Helper(this, this.Vertices);

        m_oSheets1And2Helper.TableSelectionChanged +=
			new TableSelectionChangedEventHandler(
				m_oSheets1And2Helper_TableSelectionChanged);

        m_oSheets1And2Helper.Sheet_Startup(sender, e);

        AssertValid();
	}

    //*************************************************************************
    //  Method: m_oSheets1And2Helper_TableSelectionChanged()
    //
    /// <summary>
	/// Handles the TableSelectionChanged event on the m_oSheets1And2Helper
	/// object.
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
	m_oSheets1And2Helper_TableSelectionChanged
	(
		object sender,
		TableSelectionChangedEventArgs e
	)
	{
		AssertValid();

		FireVertexSelectionChanged(e);
	}

    //*************************************************************************
    //  Method: Sheet2_Shutdown()
    //
    /// <summary>
	/// Handles the Shutdown event on the worksheet.
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
	Sheet2_Shutdown
	(
		object sender,
		System.EventArgs e
	)
	{
		AssertValid();

        m_oSheets1And2Helper.Sheet_Shutdown(sender, e);
	}

	//*************************************************************************
	//	Method: ThisWorkbook_GraphDrawn()
	//
	/// <summary>
	/// Handles the GraphDrawn event on ThisWorkbook.
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
	/// Graph drawing occurs asynchronously.  This event fires when the graph
	/// is completely drawn.
	/// </remarks>
	//*************************************************************************

	private void
	ThisWorkbook_GraphDrawn
	(
		Object sender,
		GraphDrawnEventArgs e
	)
	{
		Debug.Assert(e != null);
		AssertValid();

		if (m_oSheets1And2Helper.TableExists)
		{
			OnGraphDrawn(e);
		}
	}

	//*************************************************************************
	//	Method: ThisWorkbook_VertexMoved()
	//
	/// <summary>
	/// Handles the VertexMoved event on ThisWorkbook.
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
	ThisWorkbook_VertexMoved
	(
		Object sender,
		VertexMovedEventArgs e
	)
	{
		Debug.Assert(e != null);
		AssertValid();

		if (m_oSheets1And2Helper.TableExists)
		{
			OnVertexMoved(e);
		}
	}

	//*************************************************************************
	//	Method: ThisWorkbook_VertexAttributesEditedInGraph()
	//
	/// <summary>
	/// Handles the VertexAttributesEditedInGraph event on ThisWorkbook.
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
	ThisWorkbook_VertexAttributesEditedInGraph
	(
		Object sender,
		VertexAttributesEditedEventArgs e
	)
	{
		Debug.Assert(e != null);
		AssertValid();

		if (m_oSheets1And2Helper.TableExists)
		{
			OnVertexAttributesEditedInGraph(e);
		}
	}


	#region VSTO Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InternalStartup()
	{
		this.Startup += new System.EventHandler(Sheet2_Startup);
		this.Shutdown += new System.EventHandler(Sheet2_Shutdown);
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
		Debug.Assert(m_oSheets1And2Helper != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Object that does most of the work for this class.

	private Sheets1And2Helper m_oSheets1And2Helper;
}

}
