
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: DynamicFilterSettings
//
/// <summary>
/// Provides access to settings for dynamic filtering.
/// </summary>
///
/// <remarks>
/// A dynamic filter is a control that can be adjusted to selectively show and
/// hide edges and vertices in the graph in real time.  This class maintains
/// the filter settings, which are stored in a table in a hidden worksheet.
///
/// <para>
/// Call the <see cref="Open" /> method to gain access to the settings, then
/// use <see cref="TryGetSettings" /> and <see cref="SetSettings" /> to get and
/// set the settings for one filter.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class DynamicFilterSettings : WorksheetReaderBase
{
    //*************************************************************************
    //  Constructor: DynamicFilterSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicFilterSettings" />
	/// class.
    /// </summary>
	///
    /// <param name="workbook">
	///	The workbook containing the settings.
    /// </param>
    //*************************************************************************

    public DynamicFilterSettings
	(
        Microsoft.Office.Interop.Excel.Workbook workbook
	)
    {
		m_oWorkbook = workbook;
		m_oDynamicFilterSettingsTable = null;

		m_oDynamicFilterSettingsDictionary =
			new Dictionary<String, SettingsForOneFilter>();

		AssertValid();
    }

	//*************************************************************************
	//	Method: Open()
	//
	/// <summary>
	/// Attempts to gain access to all the filter settings.
	/// </summary>
	///
	/// <remarks>
	/// Call this before calling any other method.
	///
	/// <para>
	/// A <see cref="WorkbookFormatException" /> is thrown if the dynamic
	/// filter settings table can't be read.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public void
	Open()
	{
		Debug.Assert(m_oDynamicFilterSettingsTable == null);
		AssertValid();

		// Get the table that contains the dynamic filter settings.

		if ( !ExcelUtil.TryGetTable(m_oWorkbook, WorksheetNames.Miscellaneous,
			TableNames.DynamicFilterSettings,
			out m_oDynamicFilterSettingsTable) )
		{
			OnWorkbookFormatError( String.Format(

				"A table that is required to use this feature is missing."
				+ "\r\n\r\n{0}"
				,
				ErrorUtil.GetTemplateMessage()
				) );
		}

		// Read the table.

        Range oDataBodyRange;

		if ( !ExcelUtil.TryGetVisibleTableRange(
            m_oDynamicFilterSettingsTable, out oDataBodyRange) )
		{
			// The table is empty.  This is not an error.

			return;
		}

		// The table is hidden, so no one should ever be able to filter its
		// rows.

		Debug.Assert(oDataBodyRange.Areas.Count == 1);

		// Get the indexes of the columns within the table.

		DynamicFilterSettingsTableColumnIndexes
			oDynamicFilterSettingsTableColumnIndexes =
			GetDynamicFilterSettingsTableColumnIndexes(
				m_oDynamicFilterSettingsTable);

		// The table contains one row per dynamic filter.  Loop through the
		// rows.

		Int32 iRows = oDataBodyRange.Rows.Count;
		Object [,] aoValues = ExcelUtil.GetRangeValues(oDataBodyRange);

		Range oSelectedMinimumCell =
			( (Range)oDataBodyRange.Cells[1, 1] ).get_Offset(0,
				oDynamicFilterSettingsTableColumnIndexes.SelectedMinimum - 1);

		Range oSelectedMaximumCell =
			( (Range)oDataBodyRange.Cells[1, 1] ).get_Offset(0,
				oDynamicFilterSettingsTableColumnIndexes.SelectedMaximum - 1);

		for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
		{
			String sTableName, sColumnName;
			Double dSelectedMinimum, dSelectedMaximum;

			if (
				ExcelUtil.TryGetNonEmptyStringFromCell(aoValues, iRowOneBased,
					oDynamicFilterSettingsTableColumnIndexes.TableName,
					out sTableName)
				&&
				ExcelUtil.TryGetNonEmptyStringFromCell(aoValues, iRowOneBased,
					oDynamicFilterSettingsTableColumnIndexes.ColumnName,
					out sColumnName)
				&&
				ExcelUtil.TryGetDoubleFromCell(aoValues, iRowOneBased,
					oDynamicFilterSettingsTableColumnIndexes.SelectedMinimum,
					out dSelectedMinimum)
				&&
				ExcelUtil.TryGetDoubleFromCell(aoValues, iRowOneBased,
					oDynamicFilterSettingsTableColumnIndexes.SelectedMaximum,
					out dSelectedMaximum)
				)
			{
				// Create a SettingsForOneFilter object for each filter and
				// store it in a dictionary.

				SettingsForOneFilter oSettingsForOneFilter =
					new SettingsForOneFilter();

				oSettingsForOneFilter.SelectedMinimum =
					(Decimal)dSelectedMinimum;

				oSettingsForOneFilter.SelectedMaximum =
					(Decimal)dSelectedMaximum;

				oSettingsForOneFilter.SelectedMinimumAddress =
					ExcelUtil.GetRangeAddressAbsolute(oSelectedMinimumCell);

				oSettingsForOneFilter.SelectedMaximumAddress =
					ExcelUtil.GetRangeAddressAbsolute(oSelectedMaximumCell);

				m_oDynamicFilterSettingsDictionary.Add(
					GetDictionaryKey(sTableName, sColumnName),
					oSettingsForOneFilter
					);
			}

			oSelectedMinimumCell = oSelectedMinimumCell.get_Offset(1, 0);
			oSelectedMaximumCell = oSelectedMaximumCell.get_Offset(1, 0);
		}
	}

	//*************************************************************************
	//	Method: TryGetSettings()
	//
	/// <summary>
	/// Attempts to get the settings for one filter.
	/// </summary>
	///
	/// <param name="tableName">
	/// Name of the table containing the column being filtered on.
	/// </param>
	///
	/// <param name="columnName">
	/// Name of the column being filtered on.
	/// </param>
	///
	/// <param name="selectedMinimum">
	/// Where the filter's minimum value gets stored if true is returned.
	/// </param>
	///
	/// <param name="selectedMaximum">
	/// Where the filter's maximum value gets stored if true is returned.
	/// </param>
	///
	/// <param name="selectedMinimumAddress">
	/// Where the address of the settings table cell containing the filter's
	/// minimum value gets stored if true is returned.  The address is
	/// absolute ("$G$5", for example.)
	/// </param>
	///
	/// <param name="selectedMaximumAddress">
	/// Where the address of the settings table cell containing the filter's
	/// maximum value gets stored if true is returned.  The address is
	/// absolute ("$G$6", for example.)
	/// </param>
	///
	/// <returns>
	/// true if the settings were obtained, false if the settings weren't in
	/// the settings table.
	/// </returns>
	///
	/// <remarks>
	/// Call <see cref="Open" /> before calling this method.
	/// </remarks>
	//*************************************************************************

	public Boolean
	TryGetSettings
	(
		String tableName,
		String columnName,
		out Decimal selectedMinimum,
		out Decimal selectedMaximum,
		out String selectedMinimumAddress,
		out String selectedMaximumAddress
	)
	{
        Debug.Assert( !String.IsNullOrEmpty(tableName) );
        Debug.Assert( !String.IsNullOrEmpty(columnName) );
		AssertValid();

		CheckOpen();

		selectedMinimum = selectedMaximum = Decimal.MinValue;
		selectedMinimumAddress = selectedMaximumAddress = null;

		SettingsForOneFilter oSettingsForOneFilter;

		if ( m_oDynamicFilterSettingsDictionary.TryGetValue(
			GetDictionaryKey(tableName, columnName),
			out oSettingsForOneFilter) )
		{
			selectedMinimum = oSettingsForOneFilter.SelectedMinimum;
			selectedMaximum = oSettingsForOneFilter.SelectedMaximum;

			selectedMinimumAddress =
				oSettingsForOneFilter.SelectedMinimumAddress;

			selectedMaximumAddress =
				oSettingsForOneFilter.SelectedMaximumAddress;

			return (true);
		}

		return (false);
	}

	//*************************************************************************
	//	Method: SetSettings()
	//
	/// <summary>
	/// Sets the settings for one filter.
	/// </summary>
	///
	/// <param name="tableName">
	/// Name of the table containing the column being filtered on.
	/// </param>
	///
	/// <param name="columnName">
	/// Name of the column being filtered on.
	/// </param>
	///
	/// <param name="selectedMinimum">
	/// The filter's minimum value.
	/// </param>
	///
	/// <param name="selectedMaximum">
	/// The filter's maximum value.
	/// </param>
	///
	/// <param name="selectedMinimumAddress">
	/// Where the address of the settings table cell containing the filter's
	/// minimum value gets stored.  The address is absolute ("$G$5", for
	/// example.)
	/// </param>
	///
	/// <param name="selectedMaximumAddress">
	/// Where the address of the settings table cell containing the filter's
	/// maximum value gets stored.  The address is absolute ("$G$6", for
	/// example.)
	/// </param>
	///
	/// <remarks>
	/// Call <see cref="Open" /> before calling this method.
	/// </remarks>
	//*************************************************************************

	public void
	SetSettings
	(
		String tableName,
		String columnName,
		Decimal selectedMinimum,
		Decimal selectedMaximum,
		out String selectedMinimumAddress,
		out String selectedMaximumAddress
	)
	{
        Debug.Assert( !String.IsNullOrEmpty(tableName) );
        Debug.Assert( !String.IsNullOrEmpty(columnName) );
        Debug.Assert(selectedMaximum >= selectedMinimum);
        AssertValid();

		CheckOpen();

		selectedMinimumAddress = selectedMaximumAddress = null;

		SettingsForOneFilter oSettingsForOneFilter;

		String sDictionaryKey = GetDictionaryKey(tableName, columnName);

		Range oSelectedMinimumCell, oSelectedMaximumCell;

		if ( m_oDynamicFilterSettingsDictionary.TryGetValue(
			sDictionaryKey, out oSettingsForOneFilter) )
		{
			// The settings are already in the table.  Update the table row.

			Debug.Assert(m_oDynamicFilterSettingsTable.Parent is Worksheet);

			Worksheet oWorksheet =
				(Worksheet)m_oDynamicFilterSettingsTable.Parent;

			oSelectedMinimumCell = oWorksheet.get_Range(
				oSettingsForOneFilter.SelectedMinimumAddress, Missing.Value);

			oSelectedMinimumCell.set_Value(Missing.Value, selectedMinimum);

			oSelectedMaximumCell = oWorksheet.get_Range(
				oSettingsForOneFilter.SelectedMaximumAddress, Missing.Value);

			oSelectedMaximumCell.set_Value(Missing.Value, selectedMaximum);

			// Update the dictionary entry.

			oSettingsForOneFilter.SelectedMinimum = selectedMinimum;
			oSettingsForOneFilter.SelectedMaximum = selectedMaximum;
		}
		else
		{
			// The settings aren't in the table yet.  Add a row to the table.

			Range oNewRow = m_oDynamicFilterSettingsTable.ListRows.Add(
				Missing.Value).Range;

			// Get the indexes of the columns within the table.

			DynamicFilterSettingsTableColumnIndexes
				oDynamicFilterSettingsTableColumnIndexes =
				GetDynamicFilterSettingsTableColumnIndexes(
					m_oDynamicFilterSettingsTable);

			// Set the values of the row's cells.

			( (Range)oNewRow.Cells[1,
				oDynamicFilterSettingsTableColumnIndexes.TableName] ).
					set_Value(Missing.Value, tableName);

			( (Range)oNewRow.Cells[1,
				oDynamicFilterSettingsTableColumnIndexes.ColumnName] ).
					set_Value(Missing.Value, columnName);

			oSelectedMinimumCell = (Range)oNewRow.Cells[1,
				oDynamicFilterSettingsTableColumnIndexes.SelectedMinimum];

			oSelectedMaximumCell = (Range)oNewRow.Cells[1,
				oDynamicFilterSettingsTableColumnIndexes.SelectedMaximum];

			oSelectedMinimumCell.set_Value(Missing.Value, selectedMinimum);
			oSelectedMaximumCell.set_Value(Missing.Value, selectedMaximum);

			// Add a dictionary entry.

			oSettingsForOneFilter = new SettingsForOneFilter();

			oSettingsForOneFilter.SelectedMinimum = selectedMinimum;
			oSettingsForOneFilter.SelectedMaximum = selectedMaximum;

			oSettingsForOneFilter.SelectedMinimumAddress =
				ExcelUtil.GetRangeAddressAbsolute(oSelectedMinimumCell);

			oSettingsForOneFilter.SelectedMaximumAddress =
				ExcelUtil.GetRangeAddressAbsolute(oSelectedMaximumCell);

			m_oDynamicFilterSettingsDictionary.Add(sDictionaryKey,
				oSettingsForOneFilter);
		}

		selectedMinimumAddress = oSettingsForOneFilter.SelectedMinimumAddress;
		selectedMaximumAddress = oSettingsForOneFilter.SelectedMaximumAddress;

		Debug.Assert( !String.IsNullOrEmpty(selectedMinimumAddress) );
		Debug.Assert( !String.IsNullOrEmpty(selectedMaximumAddress) );
	}

    //*************************************************************************
    //  Method: GetDynamicFilterSettingsTableColumnIndexes()
    //
    /// <summary>
	/// Gets the one-based indexes of the columns within the table that
	/// contains the dynamic filter settings.
    /// </summary>
    ///
    /// <param name="oDynamicFilterSettingsTable">
	/// Table that contains the dynamic filter settings.
    /// </param>
    ///
	/// <returns>
	/// The column indexes, as a <see
	/// cref="DynamicFilterSettingsTableColumnIndexes" />.
	/// </returns>
    //*************************************************************************

	protected DynamicFilterSettingsTableColumnIndexes
	GetDynamicFilterSettingsTableColumnIndexes
	(
		ListObject oDynamicFilterSettingsTable
	)
	{
		Debug.Assert(oDynamicFilterSettingsTable != null);
		AssertValid();

		DynamicFilterSettingsTableColumnIndexes
			oDynamicFilterSettingsTableColumnIndexes =
			new DynamicFilterSettingsTableColumnIndexes();

		oDynamicFilterSettingsTableColumnIndexes.TableName =
			GetTableColumnIndex(oDynamicFilterSettingsTable,
			DynamicFilterSettingsTableColumnNames.TableName, true);

		oDynamicFilterSettingsTableColumnIndexes.ColumnName =
			GetTableColumnIndex(oDynamicFilterSettingsTable,
			DynamicFilterSettingsTableColumnNames.ColumnName, true);

		oDynamicFilterSettingsTableColumnIndexes.SelectedMinimum =
			GetTableColumnIndex(oDynamicFilterSettingsTable,
			DynamicFilterSettingsTableColumnNames.SelectedMinimum, true);

		oDynamicFilterSettingsTableColumnIndexes.SelectedMaximum =
			GetTableColumnIndex(oDynamicFilterSettingsTable,
			DynamicFilterSettingsTableColumnNames.SelectedMaximum, true);

		return (oDynamicFilterSettingsTableColumnIndexes);
	}

    //*************************************************************************
    //  Method: GetDictionaryKey()
    //
    /// <summary>
	/// Gets a key to use for the m_oDynamicFilterSettingsDictionary.
    /// </summary>
    ///
    /// <param name="sTableName">
	/// Name of the table containing the column being filtered on.
    /// </param>
    ///
    /// <param name="sColumnName">
	/// Name of the column being filtered on.
    /// </param>
    ///
	/// <returns>
	/// A key to use for the m_oDynamicFilterSettingsDictionary.
	/// </returns>
    //*************************************************************************

	protected String
	GetDictionaryKey
	(
		String sTableName,
		String sColumnName
	)
	{
		Debug.Assert( !String.IsNullOrEmpty(sTableName) );
		Debug.Assert( !String.IsNullOrEmpty(sColumnName) );
		AssertValid();

		// A tab can't occur in either a table or column name, so create the
		// key by concatenating them with a tab.

		return (sTableName + '\t' + sColumnName);
	}

    //*************************************************************************
    //  Method: CheckOpen()
    //
    /// <summary>
	/// Throws an exception if <see cref="Open" /> hasn't been successfully
	/// called.
    /// </summary>
    //*************************************************************************

	protected void
	CheckOpen()
	{
		AssertValid();

		if (m_oDynamicFilterSettingsTable == null)
		{
			Debug.Assert(false);

			throw new InvalidOperationException( String.Format(

				"{0}.Open() must be called before any other methods are"
				+ " called."
				,
				this.ClassName
				) );
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

		Debug.Assert(m_oWorkbook != null);
		// m_oDynamicFilterSettingsTable
		Debug.Assert(m_oDynamicFilterSettingsDictionary != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	///	The workbook containing the settings.

	protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

	/// Table that stores the settings for all dynamic filters, or null if
	/// Open() hasn't been successfully called.

	protected ListObject m_oDynamicFilterSettingsTable;

	/// Dictionary of filter settings, with one key/value pair for each filter.
	/// The key is the concatenation of the table and column names as returned
	/// by GetDictionaryKey(), and the value is a SettingsForOneFilter object.

	protected Dictionary<String, SettingsForOneFilter>
		m_oDynamicFilterSettingsDictionary;


	//*************************************************************************
	//  Embedded class: DynamicFilterSettingsTableColumnIndexes
	//
	/// <summary>
	/// Contains the one-based indexes of the columns in the dynamic filter
	/// settings table.
	/// </summary>
	//*************************************************************************

	protected class DynamicFilterSettingsTableColumnIndexes
	{
		/// Name of the table containing the column being filtered on.

		public Int32 TableName;

		/// Name of the column being filtered on.

		public Int32 ColumnName;

		/// The filter's minimum value.

		public Int32 SelectedMinimum;

		/// The filter's maximum value.

		public Int32 SelectedMaximum;
	}


	//*************************************************************************
	//  Embedded class: SettingsForOneFilter
	//
	/// <summary>
	/// Contains the settings for one dynamic filter.
	/// </summary>
	//*************************************************************************

	protected class SettingsForOneFilter
	{
		/// The filter's minimum value.

		public Decimal SelectedMinimum;

		/// The filter's maximum value.

		public Decimal SelectedMaximum;

		/// The absolute address of the settings table cell containing the
		/// filter's minimum value.

		public String SelectedMinimumAddress;

		/// The absolute address of the settings table cell containing the
		/// filter's maximum value.

		public String SelectedMaximumAddress;
	}
}
}
