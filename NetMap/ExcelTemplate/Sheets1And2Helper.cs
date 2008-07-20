
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: Sheets1And2Helper
//
/// <summary>
/// Helper class used by Sheet1 and Sheet2.
/// </summary>
///
/// <remarks>
/// Sheet1 (edges) and Sheet2 (vertices) implement very similar functionality.
/// They differ in that one has an edge table and the other has a vertex table,
/// but the code involving those tables is nearly identical.
///
/// <para>
/// To avoid duplicate code, the ideal solution would be to have Sheet1 and
/// Sheet2 derive from a base class that implements most of the code.  The
/// Visual Studio designer makes this difficult if not impossible, however,
/// because the base class for each sheet
/// (Microsoft.Office.Tools.Excel.Worksheet) is specified in the
/// SheetX.Designer.cs file, and that file is dynamically generated.
/// </para>
/// 
/// <para>
/// As a workaround, the shared code is implemented in this helper class, an
/// instance of which is owned by each sheet.  The sheets delegate most of
/// their method calls to the methods in this class.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class Sheets1And2Helper : Object
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
    {
		m_oWorksheet = worksheet;
		m_oTable = table;
		m_bIgnoreSelectionEvents = false;
		m_aiRowIDsToSelectOnActivation = null;

		AssertValid();
    }

    //*************************************************************************
    //  Property: TableExists
    //
    /// <summary>
	/// Gets a flag indicating whether the edge or vertex table still exists.
    /// </summary>
    ///
    /// <value>
    /// true if the edge or vertex table still exists.
    /// </value>
	///
	/// <remarks>
	/// The user can close the edge or vertex worksheet after the workbook has
	/// been read.  If he has done so, this property returns false.
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

		m_oWorksheet.ActivateEvent += new DocEvents_ActivateEventHandler(
			this.Sheet_ActivateEvent);

		m_oTable.SelectionChange += new DocEvents_SelectionChangeEventHandler(
			Table_SelectionChange);

		m_oTable.Deselected += new DocEvents_SelectionChangeEventHandler(
			Table_Deselected);

		Globals.ThisWorkbook.SelectionChangedInGraph +=
            new SelectionChangedEventHandler(
				this.ThisWorkbook_SelectionChangedInGraph);
	}

    //*************************************************************************
    //  Method: Sheet_Shutdown()
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

	public void
	Sheet_Shutdown
	(
		object sender,
		System.EventArgs e
	)
	{
		AssertValid();

		// (Do nothing.)
	}

    //*************************************************************************
    //  Method: TryGetIDsOfVisibleRows()
    //
    /// <summary>
	/// Attempts to get a dictionary containing the ID of each visible row in
	/// the table.
    /// </summary>
    ///
	/// <param name="rowIDDictionary">
	/// Where a dictionary gets stored if true is returned.  There is one
	/// dictionary entry for each visible row in the table that has an ID.  The
	/// key is the ID stored in the table's ID column and the value is the
	/// one-based row number relative to the worksheet.
	/// </param>
	///
	/// <returns>
	/// true if the dictionary was obtained, false if there is no ID column or
	/// all IDs are hidden.
	/// </returns>
    //*************************************************************************

    public Boolean
	TryGetIDsOfVisibleRows
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

		// Remove hidden rows, which occur when the table is filtered.
		//
		// Excel wierdness: SpecialCells() causes the SelectionChange event on
		// the table to fire.  Why?  In any case, the m_bIgnoreSelectionEvents
		// flag prevents this from causing an endless loop.

        Range oVisibleIDColumnData;

		if ( !ExcelUtil.TryGetVisibleRange(
			oIDColumnData, out oVisibleIDColumnData) )
		{
			return (false);
		}

		// Read the ID column.

		Object [,] aoIDValues = ExcelUtil.GetRangeValues(oIDColumnData);

		// Create a dictionary that maps row IDs to row numbers.

		rowIDDictionary = new Dictionary<Int32, Int32>();

		Int32 iDataBodyRangeRow = m_oTable.DataBodyRange.Row;

		// Loop through the visible areas.

		foreach (Range oVisibleIDColumnDataArea in oVisibleIDColumnData.Areas)
		{
			Int32 iFirstRowOneBased =
				oVisibleIDColumnDataArea.Row - iDataBodyRangeRow + 1;

			Int32 iLastRowOneBased =
				iFirstRowOneBased + oVisibleIDColumnDataArea.Rows.Count - 1;

			for (Int32 iRowOneBased = iFirstRowOneBased;
				iRowOneBased <= iLastRowOneBased; iRowOneBased++)
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
	//	Method: TryGetIDRow()
	//
	/// <summary>
	/// Attempts to get the row number of an ID within an ID column.
	/// </summary>
	///
	/// <param name="IDColumn">
	/// Column containing the IDs.
	/// </param>
	///
	/// <param name="ID">
	/// The ID to look for.
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
	/// This method looks for <paramref name="ID" /> in <paramref
	/// name="IDColumn" />.  If found, the one-based row number of the row
	/// containing the ID is stored at <paramref name="rowOneBased" /> and
	/// true is returned.  false is returned otherwise.
	/// </remarks>
	//*************************************************************************

	public Boolean
	TryGetIDRow
	(
		Microsoft.Office.Interop.Excel.ListColumn IDColumn,
		Int32 ID,
		out Int32 rowOneBased
	)
	{
		Debug.Assert(IDColumn != null);
		AssertValid();

		rowOneBased = Int32.MinValue;

		// Look for the cell in the ID column that contains the specified ID.

		Microsoft.Office.Interop.Excel.Range oIDCell = IDColumn.Range.Find(
			ID.ToString(),
			Missing.Value,
			Microsoft.Office.Interop.Excel.XlFindLookIn.xlValues,
			Microsoft.Office.Interop.Excel.XlLookAt.xlWhole,
			Microsoft.Office.Interop.Excel.XlSearchOrder.xlByRows,
			Microsoft.Office.Interop.Excel.XlSearchDirection.xlNext,
			true,
			true,
			Missing.Value
			);

		if (oIDCell == null)
		{
			return (false);
		}

		rowOneBased = oIDCell.Row;

		return (true);
	}

	//*************************************************************************
	//	Event: TableSelectionChanged
	//
	/// <summary>
	///	Occurs when the selection state of the table changes.
	/// </summary>
	//*************************************************************************

	public event TableSelectionChangedEventHandler TableSelectionChanged;


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

		// Get a dictionary containing the ID of each visible row in the table.
		// The key is the ID stored in the table's ID column and the value is
		// the one-based row number relative to the worksheet.

		Dictionary<Int32, Int32> oRowIDDictionary;

		if ( !TryGetIDsOfVisibleRows(out oRowIDDictionary) )
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

		for (Int32 i = 0; i < iRowIDsToSelect; i++)
		{
			Int32 iRowIDToSelect = aiRowIDsToSelect[i];
			Int32 iRow;

			if ( oRowIDDictionary.TryGetValue(iRowIDToSelect, out iRow) )
			{
				if (oBuiltRangeAddress.Length != 0)
				{
					oBuiltRangeAddress.Append(",");
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
	/// <param name="oSelectedRange">
	/// Range that contains the selected cells in the table.  The range may
	/// contain multiple areas.
	/// </param>
	///
	/// <param name="sColumnName">
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
	/// method reads the string from the row's <paramref name="sColumnName" />
	/// cell and stores it in the returned dictionary.
	/// </remarks>
    //*************************************************************************

    public Dictionary<String, Char>
	GetSelectedColumnValues
	(
		Range oSelectedRange,
		String sColumnName
	)
    {
		Debug.Assert(oSelectedRange != null);
		Debug.Assert( !String.IsNullOrEmpty(sColumnName) );
		AssertValid();

		// Create a dictionary for the selected values.  The dictionary key is
		// the cell value and the dictionary value is not used.

		Dictionary<String, Char> oSelectedValues =
			new Dictionary<String, Char>();

		if (!TableExists)
		{
			goto Done;
		}

        Range oDataBodyRange = m_oTable.DataBodyRange;

		if (oDataBodyRange == null)
		{
			goto Done;
		}

		// The selected range can extend outside the table.  Get the
		// intersection of the table with the selection.

        Range oSelectedTableRange;

        if ( !ExcelUtil.TryIntersectRanges(oSelectedRange,
			oDataBodyRange, out oSelectedTableRange) )
        {
			goto Done;
        }

		// Get data for the specified column.  This includes hidden rows but
		// excludes the header row.

		Range oColumnData;

		if ( !ExcelUtil.TryGetTableColumnData(m_oTable.InnerObject,
			sColumnName, out oColumnData) )
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
	//	Method: ThisWorkbook_SelectionChangedInGraph()
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
	/// This event is fired when the user clicks on the NetMap graph.
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

		Boolean bTableHasEdges = (m_oTable.Name == TableNames.Edges);

		Int32 [] aiRowIDsToSelect =
			(bTableHasEdges ? e.SelectedEdgeIDs : e.SelectedVertexIDs);

		if ( !ExcelUtil.WorksheetIsActive(m_oWorksheet.InnerObject) )
		{
			// You can't select rows in a worksheet that isn't active, so store
			// the IDs and let Sheet_ActivateEvent() select them when the sheet
			// is activated.

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

    public void
    AssertValid()
    {
		Debug.Assert(m_oWorksheet != null);
		Debug.Assert(m_oTable != null);
		// m_bIgnoreSelectionEvents
		// m_aiRowIDsToSelectOnActivation
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// The worksheet that owns this object.

	protected Microsoft.Office.Tools.Excel.Worksheet m_oWorksheet;

	/// The edge or vertex table.

	protected Microsoft.Office.Tools.Excel.ListObject m_oTable;

	/// true if selection events should be ignored.

	protected Boolean m_bIgnoreSelectionEvents;

	/// The IDs of the rows that should be selected when the worksheet is
	/// activated, or null if no rows should be selected.

	protected Int32 [] m_aiRowIDsToSelectOnActivation;
}

}
