
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: PerWorkbookSettings
//
/// <summary>
/// Provides access to settings that are stored on a per-workbook basis.
/// </summary>
///
/// <remarks>
/// The settings are stored in a table in a hidden worksheet.
/// </remarks>
//*****************************************************************************

public class PerWorkbookSettings : WorksheetReaderBase
{
    //*************************************************************************
    //  Constructor: PerWorkbookSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="PerWorkbookSettings" />
	/// class.
    /// </summary>
	///
    /// <param name="workbook">
	///	The workbook containing the settings.
    /// </param>
    //*************************************************************************

    public PerWorkbookSettings
	(
        Microsoft.Office.Interop.Excel.Workbook workbook
	)
    {
		m_oWorkbook = workbook;
		m_oSettings = null;

		AssertValid();
    }

	//*************************************************************************
	//	Property: TemplateVersion
	//
	/// <summary>
	/// Gets the version number of the template the workbook is based on.
	/// </summary>
	///
	/// <value>
	/// A template version number.
	/// </value>
	//*************************************************************************

	public Int32
	TemplateVersion
	{
		get
		{
			AssertValid();

            // The template version is stored in the workbook as a Double with
			// zero decimal places.  Sample: 51.

			Object oTemplateVersion;

			if ( TryGetValue(TemplateVersionSettingName, typeof(Double),
				out oTemplateVersion) )
			{
				return ( (Int32)(Double)oTemplateVersion );
			}

			// Return a default value.

			return (1);
		}
	}

	//*************************************************************************
	//	Property: GraphDirectedness
	//
	/// <summary>
	/// Gets or sets the graph directedness of the workbook.
	/// </summary>
	///
	/// <value>
	/// A GraphDirectedness value.
	/// </value>
	//*************************************************************************

	public GraphDirectedness
	GraphDirectedness
	{
		get
		{
			AssertValid();

            // The directedness is stored in the workbook as a String.  Sample:
			// "Undirected".

			Object oGraphDirectedness;

			if ( TryGetValue(GraphDirectednessSettingName, typeof(String),
				out oGraphDirectedness) )
			{
				try
				{
					return ( (GraphDirectedness)Enum.Parse(
						typeof(GraphDirectedness),
						(String)oGraphDirectedness) );
				}
				catch (ArgumentException)
				{
				}
			}

			// Return a default value.

			return (GraphDirectedness.Directed);
		}

		set
		{
			SetValue( GraphDirectednessSettingName, value.ToString() );

			AssertValid();
		}
	}

	//*************************************************************************
	//	Method: SetValue
	//
	/// <summary>
	/// Sets the value of a setting.
	/// </summary>
	///
	/// <param name="settingName">
	/// The setting's name.
	/// </param>
	///
	/// <param name="value">
	/// The value to set.  Can be null.
	/// </param>
	//*************************************************************************

	protected void
	SetValue
	(
		String settingName,
		Object value
	)
	{
		AssertValid();

		Dictionary<String, Object> oSettings = GetAllSettings();

		oSettings[settingName] = value;

		WriteAllSettings();
	}

	//*************************************************************************
	//	Method: TryGetValue
	//
	/// <summary>
	/// Attempts to get the value of a specified setting.
	/// </summary>
	///
	/// <param name="settingName">
	/// The setting's name.
	/// </param>
	///
	/// <param name="valueType">
	/// Expected type of the requested value.  Sample: typeof(String).
	/// </param>
	///
	/// <param name="value">
	/// Where the value gets stored if true is returned, as an <see
	/// cref="Object" />.
	/// </param>
	///
	/// <returns>
	/// true if the specified value was found, or false if not.
	/// </returns>
	//*************************************************************************

	protected Boolean
	TryGetValue
	(
		String settingName,
		Type valueType,
		out Object value
	)
	{
		AssertValid();

		value = null;

		Dictionary<String, Object> oSettings = GetAllSettings();

		// Although the worksheet that contains the settings is hidden, the
		// user may have unhidden it and edited the settings.  Therefore, don't
		// throw an exception if the value type is incorrect.

		if (oSettings.TryGetValue(settingName, out value) &&
			value.GetType() == valueType)
		{
			return (true);
		}

		return (false);
	}

	//*************************************************************************
	//	Method: GetAllSettings()
	//
	/// <summary>
	/// Gets all settings from the workbook.
	/// </summary>
	///
	/// <returns>
	/// A dictionary of all settings.  The key is the setting name and the
	/// value is the setting value.
	/// </returns>
	///
	/// <remarks>
	/// The settings are read once and then cached.
	///
	/// <para>
	/// If the settings can't be read, the returned dictionary is empty.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	protected Dictionary<String, Object>
	GetAllSettings()
	{
		AssertValid();

		if (m_oSettings == null)
		{
			m_oSettings = new Dictionary<String, Object>();

			// Attempt to get the optional table and table columns that contain
			// the settings.

			ListObject oPerWorkbookSettingsTable;
			Range oNameColumnData, oValueColumnData;
			Object [,] aoNameColumnValues, aoValueColumnValues;

			if (
				TryGetPerWorkbookSettingsTable(out oPerWorkbookSettingsTable)
				&&
				ExcelUtil.TryGetTableColumnDataAndValues(
					oPerWorkbookSettingsTable,
					PerWorkbookSettingsTableColumnNames.Name,
					out oNameColumnData, out aoNameColumnValues)
				&&
				ExcelUtil.TryGetTableColumnDataAndValues(
					oPerWorkbookSettingsTable,
					PerWorkbookSettingsTableColumnNames.Value,
					out oValueColumnData, out aoValueColumnValues)
				)
			{
				Int32 iRows = oNameColumnData.Rows.Count;

				for (Int32 iRowOneBased = 1; iRowOneBased <= iRows;
					iRowOneBased++)
				{
					String sName;

					if ( ExcelUtil.TryGetNonEmptyStringFromCell(
						aoNameColumnValues, iRowOneBased, 1, out sName) )
					{
						m_oSettings[sName] =
							aoValueColumnValues[iRowOneBased, 1];
					}
				}
			}
		}

		return (m_oSettings);
	}

	//*************************************************************************
	//	Method: WriteAllSettings()
	//
	/// <summary>
	/// Writes all settings from the workbook.
	/// </summary>
	//*************************************************************************

	protected void
	WriteAllSettings()
	{
		AssertValid();
		Debug.Assert(m_oSettings != null);

		ListObject oPerWorkbookSettingsTable;

		if ( !TryGetPerWorkbookSettingsTable(out oPerWorkbookSettingsTable) )
		{
			return;
		}

		// Clear the table.

		ExcelUtil.ClearTable(oPerWorkbookSettingsTable);

		// Attempt to get the optional table columns that contain the settings.

		Range oNameColumnData, oValueColumnData;

		if (
			!ExcelUtil.TryGetTableColumnData(oPerWorkbookSettingsTable,
				PerWorkbookSettingsTableColumnNames.Name, out oNameColumnData)
			||
			!ExcelUtil.TryGetTableColumnData(oPerWorkbookSettingsTable,
				PerWorkbookSettingsTableColumnNames.Value,
				out oValueColumnData)
			)
		{
			return;
		}

		// Copy the settings to arrays.

		Int32 iSettings = m_oSettings.Count;

		Object [,] aoNameColumnValues =
			ExcelUtil.GetSingleColumn2DArray(iSettings);

		Object [,] aoValueColumnValues =
			ExcelUtil.GetSingleColumn2DArray(iSettings);

		Int32 i = 1;

		foreach (KeyValuePair<String, Object> oKeyValuePair in m_oSettings)
		{
			aoNameColumnValues[i, 1] = oKeyValuePair.Key;
			aoValueColumnValues[i, 1] = oKeyValuePair.Value;
			i++;
		}

		// Write the arrays to the columns.

		ExcelUtil.SetRangeValues(oNameColumnData, aoNameColumnValues);
		ExcelUtil.SetRangeValues(oValueColumnData, aoValueColumnValues);
	}

    //*************************************************************************
    //  Method: TryGetPerWorkbookSettingsTable()
    //
    /// <summary>
	/// Attempts to get the per-workbook settings table.
    /// </summary>
    ///
    /// <param name="oPerWorkbookSettingsTable">
	/// Where the table gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
	/// true if successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetPerWorkbookSettingsTable
    (
		out ListObject oPerWorkbookSettingsTable
    )
    {
		AssertValid();

		return( ExcelUtil.TryGetTable(m_oWorkbook,
			WorksheetNames.Miscellaneous, TableNames.PerWorkbookSettings,
			out oPerWorkbookSettingsTable) );
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
		// m_oSettings
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Name of the template version setting.

	protected const String TemplateVersionSettingName = "Template Version";

	/// Name of the graph directedness setting.

	protected const String GraphDirectednessSettingName = "Graph Directedness";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	///	The workbook containing the settings.

	protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

	/// Dictionary of settings, or null if the settings haven't been read from
	/// the workbook yet.  The key is the setting name and the value is the
	/// setting value.
	///
	/// Do not use this directly.  Use GetAllSettings() and WriteAllSettings()
	/// instead.

	protected Dictionary<String, Object> m_oSettings;
}
}
