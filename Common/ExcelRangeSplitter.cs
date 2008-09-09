
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//	Class: ExcelRangeSplitter
//
/// <summary>
///	Supports splitting an Excel range into subranges via enumeration.
/// </summary>
///
/// <remarks>
/// Use <see cref="SplitRange(Range, Int32)" /> to split an Excel range into
/// subranges that are no larger than a specified number of rows.  It can be
/// used to read cell values from a large worksheet without running into
/// out-of-memory exceptions.
/// </remarks>
///
/// <example>
/// Sample code:
///
/// <code>
/// foreach ( Range oSubrange in ExcelRangeSplitter.SplitRange(oRangeToSplit) )
/// {
///     ...
/// }
/// </code>
///
/// </example>
//*****************************************************************************

public static class ExcelRangeSplitter
{
	//*************************************************************************
	//	Method: SplitRange()
	//
	/// <remarks>
	/// Splits a range into areas and subranges.
	/// </remarks>
	///
	/// <summary>
	/// Splits a range into areas and subranges using a default maximum number
	/// of rows per subrange.
	/// </summary>
	///
	/// <param name="rangeToSplit">
	/// Excel range to split into areas and subranges via enumeration.  Must
	/// contain at least one row.  May contain multiple areas, but each area
	/// must have the same number of columns.
	/// </param>
	//*************************************************************************

	public static IEnumerable<Range>
	SplitRange
	(
		Range rangeToSplit
	)
	{
		return ( SplitRange(rangeToSplit, DefaultMaximumRowsPerSubrange) );
	}

	//*************************************************************************
	//	Method: SplitRange()
	//
	/// <summary>
	/// Splits a range into areas and subranges using a specified maximum
	/// number of rows per subrange.
	/// </summary>
	///
	/// <param name="rangeToSplit">
	/// Excel range to split into areas and subranges via enumeration.  Must
	/// contain at least one row.  May contain multiple areas, but each area
	/// must have the same number of columns.
	/// </param>
	///
	/// <param name="maximumRowsPerSubrange">
	/// Maximum number of rows to include in each subrange.
	/// </param>
	//*************************************************************************

	public static IEnumerable<Range>
	SplitRange
	(
		Range rangeToSplit,
		Int32 maximumRowsPerSubrange
	)
	{
		Debug.Assert(rangeToSplit != null);

		#if DEBUG
		Int32 iColumns = Int32.MinValue;
		#endif

		foreach (Range oArea in rangeToSplit.Areas)
		{
			#if DEBUG

			// Verify that all areas contain the same number of columns.

			Int32 iColumnsInArea = oArea.Columns.Count;

			if (iColumns == Int32.MinValue)
			{
				iColumns = iColumnsInArea;
			}
			else
			{
				Debug.Assert(iColumnsInArea == iColumns);
			}

			#endif

			foreach ( Range oSubrange in
				SplitSingleAreaRange(oArea, maximumRowsPerSubrange) )
			{
				yield return (oSubrange);
			}
		}
	}

	//*************************************************************************
	//	Method: SplitSingleAreaRange()
	//
	/// <summary>
	/// Splits a single-area range into subranges using a specified maximum
	/// number of rows per subrange.
	/// </summary>
	///
	/// <param name="singleAreaRangeToSplit">
	/// Excel range to split into subranges via enumeration.  Must contain at
	/// least one row.  Cannot contain multiple areas.
	/// </param>
	///
	/// <param name="maximumRowsPerSubrange">
	/// Maximum number of rows to include in each subrange.
	/// </param>
	//*************************************************************************

	public static IEnumerable<Range>
	SplitSingleAreaRange
	(
		Range singleAreaRangeToSplit,
		Int32 maximumRowsPerSubrange
	)
	{
		Debug.Assert(singleAreaRangeToSplit != null);
		Debug.Assert(singleAreaRangeToSplit.Areas.Count == 1);

		Int32 iTotalRows = singleAreaRangeToSplit.Rows.Count;

		Int32 iLastRowNumberOneBased =
			singleAreaRangeToSplit.Row + iTotalRows - 1;

		Range oCurrentSubrange = null;

		if (iTotalRows <= maximumRowsPerSubrange)
		{
			// There is no need to split the range into subranges.

			yield return (singleAreaRangeToSplit);

			yield break;
		}

		// Resize the range to the maximum number of rows.

		oCurrentSubrange = singleAreaRangeToSplit.get_Resize(
			maximumRowsPerSubrange, Missing.Value);

		yield return (oCurrentSubrange);

		while (true)
		{
			Int32 iLastSubrangeRowNumberOneBased =
				oCurrentSubrange.Row + oCurrentSubrange.Rows.Count - 1;

			if (iLastSubrangeRowNumberOneBased == iLastRowNumberOneBased)
			{
				// The subrange is at the end of the range.

				yield break;
			}

			// The current subrange needs to be shifted down.

			Int32 iRemainingRows =
				iLastRowNumberOneBased - iLastSubrangeRowNumberOneBased;

			Int32 iRowsInNextSubrange =
				Math.Min(iRemainingRows, maximumRowsPerSubrange);

			oCurrentSubrange = oCurrentSubrange.get_Resize(
				iRowsInNextSubrange, Missing.Value);

			oCurrentSubrange = oCurrentSubrange.get_Offset(
				maximumRowsPerSubrange, Missing.Value);

			yield return (oCurrentSubrange);
		}
	}

	//*************************************************************************
	//	Method: GetParallelSubrange()
	//
	/// <summary>
	/// Given a subrange in one column, returns a subrange in a second column
	/// with the same start and end rows.
	/// </summary>
	///
	/// <param name="column1Subrange">
	/// The subrange in one column.
	/// </param>
	///
	/// <param name="column2NumberOneBased">
	/// The one-based column number of the second column.
	/// </param>
	///
	/// <returns>
	/// The range in <paramref name="column2NumberOneBased" /> that has the
	/// same start and end rows as <paramref name="column1Subrange" />.
	/// </returns>
	//*************************************************************************

	public static Range
	GetParallelSubrange
	(
		Range column1Subrange,
		Int32 column2NumberOneBased
	)
	{
		Debug.Assert(column1Subrange != null);
		Debug.Assert(column2NumberOneBased >= 1);

		Int32 iColumn1NumberOneBased = column1Subrange.Column;

		Range oColumn2Subrange = column1Subrange.get_Offset(
			Missing.Value, column2NumberOneBased - iColumn1NumberOneBased);

		return (oColumn2Subrange);
	}


	//*************************************************************************
	//	Private constants
	//*************************************************************************

	/// Default maximum number of rows per subrange.

	private static readonly Int32 DefaultMaximumRowsPerSubrange = 5000;
}
}
