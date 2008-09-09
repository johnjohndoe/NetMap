
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: TableColumnAdder
//
/// <summary>
/// Adds columns to an Excel table (ListObject), and finds the columns that
/// were added.
/// </summary>
///
/// <remarks>
/// Use <see cref="AddColumnPair" /> to add a pair of columns to a table.  Use
/// <see cref="GetColumnPairIndexes" /> to get a collection of all the columns
/// pairs that were added.
/// </remarks>
//*****************************************************************************

public class TableColumnAdder : Object
{
    //*************************************************************************
    //  Constructor: TableColumnAdder()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="TableColumnAdder" />
	/// class.
    /// </summary>
    //*************************************************************************

    public TableColumnAdder()
    {
        // (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Method: AddColumnPair()
    //
    /// <summary>
	/// Adds a pair of columns to an Excel table.
    /// </summary>
    ///
    /// <param name="workbook">
	/// Workbook containing the table.
    /// </param>
    ///
    /// <param name="worksheetName">
	/// Worksheet containing the table.
    /// </param>
    ///
    /// <param name="tableName">
	/// Name of the table.
    /// </param>
    ///
    /// <param name="column1NameBase">
	/// Base name of the first column.  Sample: "Custom Menu Item Text".
    /// </param>
    ///
    /// <param name="column1WidthChars">
	/// Width of the first column, in characters.
    /// </param>
	///
    /// <param name="column2NameBase">
	/// Base name of the second column.  Sample: "Custom Menu Item Action".
    /// </param>
    ///
    /// <param name="column2WidthChars">
	/// Width of the second column, in characters.
    /// </param>
	///
    /// <remarks>
	/// This method adds two columns to the specified table.  If columns with
	/// the specified names already exist, a number is appended to the names --
	/// "Custom Menu Item Text 2" and "Custom Menu Item Action 2", for example.
	///
	/// <para>
	/// If the worksheet or table doesn't exist, a <see
	/// cref="WorkbookFormatException" /> is thrown.  If the columns couldn't
	/// be added because the user declined to unmerge a merged cell, a message
	/// is displayed.
	/// </para>
	///
	/// <para>
	/// The column pairs that were added by calls to this method can be
	/// retrieved with <see cref="GetColumnPairIndexes" />.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    public void
    AddColumnPair
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
		String worksheetName,
		String tableName,
		String column1NameBase,
		Single column1WidthChars,
		String column2NameBase,
		Single column2WidthChars
    )
    {
		Debug.Assert(workbook != null);
		Debug.Assert( !String.IsNullOrEmpty(worksheetName) );
		Debug.Assert( !String.IsNullOrEmpty(tableName) );
		Debug.Assert( !String.IsNullOrEmpty(column1NameBase) );
		Debug.Assert(column1WidthChars > 0);
		Debug.Assert( !String.IsNullOrEmpty(column2NameBase) );
		Debug.Assert(column2WidthChars > 0);
        AssertValid();

		ListObject oTable;

		if ( !ExcelUtil.TryGetTable(workbook, worksheetName, tableName,
			out oTable) )
		{
			throw new WorkbookFormatException(String.Format(

				"To use this feature, there must be a worksheet named \"{0}\""
				+ " that contains a table named \"{1}\"."
				+ "\r\n\r\n"
				+ "{2}"
				,
				worksheetName,
				tableName,
				ErrorUtil.GetTemplateMessage()
				) );
		}

		Int32 iMaximumAppendedNumber = Math.Max(
			GetMaximumAppendedNumber(oTable, column1NameBase),
			GetMaximumAppendedNumber(oTable, column2NameBase)
			);

		if (iMaximumAppendedNumber != 0)
		{
			String sStringToAppend =
				" " + (iMaximumAppendedNumber + 1).ToString();

			column1NameBase += sStringToAppend;
			column2NameBase += sStringToAppend;
		}

		ListColumn oListColumn1, oListColumn2;

		if (
			ExcelUtil.TryAddTableColumn(oTable, column1NameBase,
				column1WidthChars, null, out oListColumn1)
			&&
			ExcelUtil.TryAddTableColumn(oTable, column2NameBase,
				column2WidthChars, null, out oListColumn2)
			)
		{
			oListColumn1.Range.WrapText = true;
			oListColumn2.Range.WrapText = true;
		}
		else
		{
			FormUtil.ShowWarning("The columns weren't added.");
		}

		ExcelUtil.ActivateWorksheet(oTable);
    }

    //*************************************************************************
    //  Method: GetColumnPairIndexes()
    //
    /// <summary>
	/// Gets the indexes of all the column pairs that were added to a table.
    /// </summary>
    ///
    /// <param name="table">
	/// Table to get the column pairs from.
    /// </param>
    ///
    /// <param name="column1NameBase">
	/// Base name of the first column.  Sample: "Custom Menu Item Text".
    /// </param>
    ///
    /// <param name="column2NameBase">
	/// Base name of the second column.  Sample: "Custom Menu Item Action".
    /// </param>
    ///
	/// <returns>
	/// An array of KeyValuePairs, one for each pair of columns that were
	/// added to the table by <see cref="AddColumnPair" />.  The key is the
	/// one-based index of the column corresponding to <paramref
	/// name="column1NameBase" /> and the value is the one-based index of the
	/// column corresponding to <paramref name="column2NameBase" />.  The array
	/// may be empty but is never null.
	/// </returns>
    //*************************************************************************

	public KeyValuePair<Int32, Int32> []
	GetColumnPairIndexes
	(
		ListObject table,
		String column1NameBase,
		String column2NameBase
	)
	{
		Debug.Assert(table != null);
		AssertValid();

		List<KeyValuePair<Int32, Int32>> oColumnPairIndexes =
			new List<KeyValuePair<Int32, Int32>>();

		Regex oRegex = GetColumnNameRegex(column1NameBase);

		// Loop through all the columns looking for a column name based on
		// column1NameBase.  If such a column is found, look for the
		// corresponding column name based on column2NameBase.

		foreach (ListColumn oColumn1 in table.ListColumns)
		{
			String sColumn1Name = oColumn1.Name;

			ListColumn oColumn2 = null;

			if ( String.IsNullOrEmpty(sColumn1Name) )
			{
				continue;
			}

			// Look for an exact match.

			if (sColumn1Name == column1NameBase)
			{
				// Found column 1.  Look for column 2.

				if ( !ExcelUtil.TryGetTableColumn(table, column2NameBase,
					out oColumn2) )
				{
					oColumn2 = null;
				}
			}
			else
			{
				// Look for a "starts with" match.

				Match oMatch = oRegex.Match(sColumn1Name);

				Int32 iAppendedNumber;

				if (
					oMatch.Success
					&&
					Int32.TryParse(
						oMatch.Groups[AppendedNumberGroupName].Value,
						out iAppendedNumber)
					)
				{
					// Found column 1.  Look for column 2.

					if ( !ExcelUtil.TryGetTableColumn(table,
						column2NameBase + " " + iAppendedNumber.ToString(),
						out oColumn2) )
					{
						oColumn2 = null;
					}
				}
			}

			if (oColumn2 != null)
			{
				oColumnPairIndexes.Add(new KeyValuePair<Int32, Int32>(
					oColumn1.Index, oColumn2.Index) );
			}
		}

		return ( oColumnPairIndexes.ToArray() );
	}

    //*************************************************************************
    //  Method: GetMaximumAppendedNumber()
    //
    /// <summary>
	/// Gets the largest number that was appended to a column name base.
    /// </summary>
    ///
    /// <param name="oTable">
	/// Table containing the columns.
    /// </param>
    ///
    /// <param name="sColumnNameBase">
	/// Base column name without an appended number.  Sample:
	/// "Custom Menu Item Text".
    /// </param>
    ///
	/// <returns>
	/// 0 if there are no columns that start with the base name; 1 if there is
	/// a column with the exact base name; or N, where N is found in a column
	/// named "Custom Menu Item Text N", for example, and N is the largest such
	/// number.
	/// </returns>
	///
    /// <remarks>
	/// This method looks at all column names that start with <paramref
	/// name="sColumnNameBase" /> and returns the largest number that was
	/// appended to the base name.  For example, if there are columns named
	/// "Custom Menu Item Text" and "Custom Menu Item Text 2", this method
	/// returns 2.
	///
	/// <para>
	/// The column name sequence runs "Custom Menu Item Text", "Custom Menu
	/// Item Text 2", "Custom Menu Item Text 3", and so on.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

	protected Int32
	GetMaximumAppendedNumber
	(
		ListObject oTable,
		String sColumnNameBase
	)
	{
		Debug.Assert(oTable != null);
		Debug.Assert( !String.IsNullOrEmpty(sColumnNameBase) );
		AssertValid();

		Int32 iMaximumAppendedNumber = 0;

		Regex oRegex = GetColumnNameRegex(sColumnNameBase);

		foreach (ListColumn oColumn in oTable.ListColumns)
		{
			String sColumnName = oColumn.Name;
			Int32 iAppendedNumber = -1;

			if ( String.IsNullOrEmpty(sColumnName) )
			{
				continue;
			}

			// Look for an exact match.

			if (sColumnName == sColumnNameBase)
			{
				iAppendedNumber = 1;
			}
			else
			{
				// Look for a "starts with" match.

				Match oMatch = oRegex.Match(sColumnName);

				if (
					!oMatch.Success
					||
					!Int32.TryParse(
						oMatch.Groups[AppendedNumberGroupName].Value,
							out iAppendedNumber)
					)
				{
					continue;
				}
			}

			Debug.Assert(iAppendedNumber != -1);

			iMaximumAppendedNumber =
				Math.Max(iAppendedNumber, iMaximumAppendedNumber);
		}

		return (iMaximumAppendedNumber);
	}

    //*************************************************************************
    //  Method: GetColumnNameRegex()
    //
    /// <summary>
	/// Gets the Regex to use to match a base column name appended with a
	/// number.
    /// </summary>
    ///
    /// <param name="sColumnNameBase">
	/// Base column name without an appended number.  Sample:
	/// "Custom Menu Item Text".
    /// </param>
    ///
	/// <returns>
	/// The Regex to match "Custom Menu Item Text N", for example.
    /// </returns>
    //*************************************************************************

	protected Regex
	GetColumnNameRegex
	(
		String sColumnNameBase
	)
	{
		Debug.Assert( !String.IsNullOrEmpty(sColumnNameBase) );
		AssertValid();

		return ( new Regex(
			sColumnNameBase + " " + @"(?<AppendedNumber>\d+)$") );
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
    //  Protected constants
    //*************************************************************************

	/// Name of the Regex group that matches an appended number.

	protected const String AppendedNumberGroupName = "AppendedNumber";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
