
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Reflection;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: TableColumnMapper
//
/// <summary>
/// Object that maps the numbers in one table column to values in another
/// column.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class TableColumnMapper : Object
{
    //*************************************************************************
    //  Method: MapToNumericRange()
    //
    /// <summary>
	///	Maps the numbers in a source column to a range of numbers in a
	/// destination column.
    /// </summary>
    ///
	/// <param name="table">
	///	Table containing the two columns.
	/// </param>
	///
	/// <param name="sourceColumnName">
	/// Name of the source column.
	/// </param>
	///
	/// <param name="destinationColumnName">
	/// Name of the destination column.
	/// </param>
	///
	/// <param name="useSourceNumber1">
	/// If true, <paramref name="sourceNumber1" /> should be used.  If false,
	/// the smallest number in the source column should be used.
	/// </param>
	///
	/// <param name="useSourceNumber2">
	/// If true, <paramref name="sourceNumber2" /> should be used.  If false,
	/// the largest number in the source column should be used.
	/// </param>
	///
	/// <param name="sourceNumber1">
	/// The first number to use in the source column.  Does not have to be less
	/// than <paramref name="sourceNumber2" />.  Not valid if <paramref
	/// name="useSourceNumber1" /> is false.
	/// </param>
	///
	/// <param name="sourceNumber2">
	/// The second number to use in the source column.  Does not have to be
	/// greater than <paramref name="sourceNumber1" />.  Not valid if <paramref
	/// name="useSourceNumber2" /> is false.
	/// </param>
	///
	/// <param name="destinationNumber1">
	/// The first number to use in the destination column.  Does not have to be
	/// less than <paramref name="destinationNumber2" />.
	/// </param>
	///
	/// <param name="destinationNumber2">
	/// The second number to use in the destination column.  Does not have to
	/// be greater than <paramref name="destinationNumber2" />.
	/// </param>
	///
	/// <param name="ignoreOutliers">
    /// true if outliers should be ignored in the source column.  Valid only if
	/// <paramref name="useSourceNumber1" /> and <paramref
	/// name="useSourceNumber2" /> are false.
	/// </param>
	///
    /// <remarks>
	/// Each numeric cell in the source column is mapped to a number in the
	/// destination column.
	///
	/// <para>
	/// If the source or destination column doesn't exist, this method does
	/// nothing.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

	public static void
	MapToNumericRange
	(
		ListObject table,
		String sourceColumnName,
		String destinationColumnName,
		Boolean useSourceNumber1,
		Boolean useSourceNumber2,
		Double sourceNumber1,
		Double sourceNumber2,
		Double destinationNumber1,
		Double destinationNumber2,
		Boolean ignoreOutliers
	)
    {
		Debug.Assert(table != null);
		Debug.Assert( !String.IsNullOrEmpty(sourceColumnName) );
		Debug.Assert( !String.IsNullOrEmpty(destinationColumnName) );

		Range oVisibleSourceRange, oVisibleDestinationRange;

		if ( !TryGetVisibleSourceAndDestination(table, sourceColumnName,
			destinationColumnName, out oVisibleSourceRange,
			out oVisibleDestinationRange) )
		{
			return;
		}

		// Get the source calculation range, which is the range of source
		// numbers used in calculating the mapping of source numbers to
		// destination numbers.

		Double dSourceCalculationNumber1, dSourceCalculationNumber2;

		if ( !TryGetSourceCalculationRange(oVisibleSourceRange,
			useSourceNumber1, useSourceNumber2, sourceNumber1, sourceNumber2,
			ignoreOutliers, out dSourceCalculationNumber1,
			out dSourceCalculationNumber2) )
		{
			return;
		}

		// Loop through the areas.

		Int32 iAreas = oVisibleSourceRange.Areas.Count;

		Debug.Assert(iAreas == oVisibleDestinationRange.Areas.Count);

		for (Int32 iArea = 1; iArea <= iAreas; iArea++)
		{
			Range oSourceArea = oVisibleSourceRange.Areas[iArea];
			Range oDestinationArea = oVisibleDestinationRange.Areas[iArea];

			Debug.Assert(oSourceArea.Rows.Count ==
				oDestinationArea.Rows.Count);

			Object [,] aoSourceAreaValues =
				ExcelUtil.GetRangeValues(oSourceArea);

			Object [,] aoDestinationAreaValues =
				ExcelUtil.GetRangeValues(oDestinationArea);

			Int32 iRows = oSourceArea.Rows.Count;

			for (Int32 iRow = 1; iRow <= iRows; iRow++)
			{
				// If the source cell doesn't contain a number, skip it.

				Double dSourceNumber;

				if ( !TryGetNumber( aoSourceAreaValues[iRow, 1], 
						out dSourceNumber) )
				{
					continue;
				}

				// Map the source number to a destination number.

				Double dDestinationNumber;

				if (dSourceCalculationNumber2 == dSourceCalculationNumber1)
				{
					dDestinationNumber = destinationNumber1;
				}
				else
				{
					dDestinationNumber =
						destinationNumber1
						+ (dSourceNumber - dSourceCalculationNumber1)
						* (destinationNumber2 - destinationNumber1)

						/ (dSourceCalculationNumber2 -
							dSourceCalculationNumber1)
						;
				}

				// Pin the destination number.

				if (destinationNumber2 >= destinationNumber1)
				{
					dDestinationNumber =
						Math.Max(dDestinationNumber, destinationNumber1);

					dDestinationNumber =
						Math.Min(dDestinationNumber, destinationNumber2);
				}
				else
				{
					dDestinationNumber =
						Math.Max(dDestinationNumber, destinationNumber2);

					dDestinationNumber =
						Math.Min(dDestinationNumber, destinationNumber1);
				}

				aoDestinationAreaValues[iRow, 1] = dDestinationNumber;
			}

			oDestinationArea.set_Value(Missing.Value, aoDestinationAreaValues);
		}
    }

    //*************************************************************************
    //  Method: MapToColor()
    //
    /// <summary>
	///	Maps the numbers in a source column to colors in a destination column.
    /// </summary>
    ///
	/// <param name="table">
	///	Table containing the two columns.
	/// </param>
	///
	/// <param name="sourceColumnName">
	/// Name of the source column.
	/// </param>
	///
	/// <param name="destinationColumnName">
	/// Name of the destination column.
	/// </param>
	///
	/// <param name="useSourceNumber1">
	/// If true, <paramref name="sourceNumber1" /> should be used.  If false,
	/// the smallest number in the source column should be used.
	/// </param>
	///
	/// <param name="useSourceNumber2">
	/// If true, <paramref name="sourceNumber2" /> should be used.  If false,
	/// the largest number in the source column should be used.
	/// </param>
	///
	/// <param name="sourceNumber1">
	/// The first number to use in the source column.  Does not have to be less
	/// than <paramref name="sourceNumber2" />.  Not valid if <paramref
	/// name="useSourceNumber1" /> is false.
	/// </param>
	///
	/// <param name="sourceNumber2">
	/// The second number to use in the source column.  Does not have to be
	/// greater than <paramref name="sourceNumber1" />.  Not valid if <paramref
	/// name="useSourceNumber2" /> is false.
	/// </param>
	///
	/// <param name="destinationColor1">
	/// The first color to use in the destination column.
	/// </param>
	///
	/// <param name="destinationColor2">
	/// The second color to use in the destination column.
	/// </param>
	///
	/// <param name="ignoreOutliers">
    /// true if outliers should be ignored in the source column.  Valid only if
	/// <paramref name="useSourceNumber1" /> and <paramref
	/// name="useSourceNumber2" /> are false.
	/// </param>
	///
    /// <remarks>
	/// Each numeric cell in the source column is mapped to a color in the
	/// destination column.
	///
	/// <para>
	/// If the source or destination column doesn't exist, this method does
	/// nothing.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

	public static void
	MapToColor
	(
		ListObject table,
		String sourceColumnName,
		String destinationColumnName,
		Boolean useSourceNumber1,
		Boolean useSourceNumber2,
		Double sourceNumber1,
		Double sourceNumber2,
		Color destinationColor1,
		Color destinationColor2,
		Boolean ignoreOutliers
	)
	{
		Debug.Assert(table != null);
		Debug.Assert( !String.IsNullOrEmpty(sourceColumnName) );
		Debug.Assert( !String.IsNullOrEmpty(destinationColumnName) );

		Range oVisibleSourceRange, oVisibleDestinationRange;

		if ( !TryGetVisibleSourceAndDestination(table, sourceColumnName,
			destinationColumnName, out oVisibleSourceRange,
			out oVisibleDestinationRange) )
		{
			return;
		}

		// Get the source calculation range, which is the range of source
		// numbers used in calculating the mapping of source numbers to
		// destination numbers.

		Double dSourceCalculationNumber1, dSourceCalculationNumber2;

		if ( !TryGetSourceCalculationRange(oVisibleSourceRange,
			useSourceNumber1, useSourceNumber2, sourceNumber1, sourceNumber2,
			ignoreOutliers, out dSourceCalculationNumber1,
			out dSourceCalculationNumber2) )
		{
			return;
		}

		// Create an object that maps a range of numbers to a range of colors.

		ColorGradientMapper oColorGradientMapper = GetColorGradientMapper(
			dSourceCalculationNumber1, dSourceCalculationNumber2,
			destinationColor1, destinationColor2);

		ColorConverter oColorConverter = new ColorConverter();

		// Loop through the areas.

		Int32 iAreas = oVisibleSourceRange.Areas.Count;

		Debug.Assert(iAreas == oVisibleDestinationRange.Areas.Count);

		for (Int32 iArea = 1; iArea <= iAreas; iArea++)
		{
			Range oSourceArea = oVisibleSourceRange.Areas[iArea];
			Range oDestinationArea = oVisibleDestinationRange.Areas[iArea];

			Debug.Assert(oSourceArea.Rows.Count ==
				oDestinationArea.Rows.Count);

			Object [,] aoSourceAreaValues =
				ExcelUtil.GetRangeValues(oSourceArea);

			Object [,] aoDestinationAreaValues =
				ExcelUtil.GetRangeValues(oDestinationArea);

			Int32 iRows = oSourceArea.Rows.Count;

			for (Int32 iRow = 1; iRow <= iRows; iRow++)
			{
				// If the source cell doesn't contain a number, skip it.

				Double dSourceNumber;

				if ( !TryGetNumber( aoSourceAreaValues[iRow, 1],
					out dSourceNumber) )
				{
					continue;
				}

				Color oDestinationColor =
					oColorGradientMapper.ColorMetricToColor(dSourceNumber);

				// Write the color in a format that is understood by
				// ColorConverter.ConvertFromString(), which is what
				// WorksheetReaderBase uses.

				aoDestinationAreaValues[iRow, 1] =
					oColorConverter.ConvertToString(oDestinationColor);
			}

			oDestinationArea.set_Value(Missing.Value, aoDestinationAreaValues);
		}
	}

    //*************************************************************************
    //  Method: MapToTwoStrings()
    //
    /// <summary>
	///	Maps the numbers in a source column to one of two strings in a
	/// destination column.
    /// </summary>
    ///
	/// <param name="table">
	///	Table containing the two columns.
	/// </param>
	///
	/// <param name="sourceColumnName">
	/// Name of the source column.  The column must exist.
	/// </param>
	///
	/// <param name="destinationColumnName">
	/// Name of the destination column.  The column must exist.
	/// </param>
	///
	/// <param name="comparisonOperator">
	/// Operator to use when comparing a cell in the source column.
	/// </param>
	///
	/// <param name="sourceNumberToCompareTo">
	/// Number to use when comparing a cell in the source column.
	/// </param>
	///
	/// <param name="destinationString1">
	/// String to write to a destination cell if the source cell satisfies the
	/// comparison criteria, or null to not write a string.
	/// </param>
	///
	/// <param name="destinationString2">
	/// String to write to a destination cell if the source cell does not
	/// satisfy the comparison criteria, or null to not write a string.
	/// </param>
	///
    /// <remarks>
	/// Each numeric cell in the source column is compared to <paramref
	/// name="sourceNumberToCompareTo" /> using <paramref
	/// name="comparisonOperator" />.  If the comparison succeeds and <paramref
	/// name="destinationString1" /> is not null, <paramref
	/// name="destinationString1" /> is written to the corresponding
	/// destination cell.  If the comparison fails and <paramref
	/// name="destinationString2" /> is not null, <paramref
	/// name="destinationString2" /> is written to the corresponding
	/// destination cell.
	///
	/// <para>
	/// If the source or destination column doesn't exist, this method does
	/// nothing.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

	public static void
	MapToTwoStrings
	(
		ListObject table,
		String sourceColumnName,
		String destinationColumnName,
		ComparisonOperator comparisonOperator,
		Double sourceNumberToCompareTo,
		String destinationString1,
		String destinationString2
	)
	{
		Debug.Assert(table != null);
		Debug.Assert( !String.IsNullOrEmpty(sourceColumnName) );
		Debug.Assert( !String.IsNullOrEmpty(destinationColumnName) );

		Range oVisibleSourceRange, oVisibleDestinationRange;

		if ( !TryGetVisibleSourceAndDestination(table, sourceColumnName,
				destinationColumnName, out oVisibleSourceRange,
				out oVisibleDestinationRange) )
		{
			return;
		}

		// Loop through the areas.

		Int32 iAreas = oVisibleSourceRange.Areas.Count;

		Debug.Assert(iAreas == oVisibleDestinationRange.Areas.Count);

		for (Int32 iArea = 1; iArea <= iAreas; iArea++)
		{
			Range oSourceArea = oVisibleSourceRange.Areas[iArea];
			Range oDestinationArea = oVisibleDestinationRange.Areas[iArea];

			Debug.Assert(oSourceArea.Rows.Count ==
				oDestinationArea.Rows.Count);

			Object [,] aoSourceAreaValues =
				ExcelUtil.GetRangeValues(oSourceArea);

			Object [,] aoDestinationAreaValues =
				ExcelUtil.GetRangeValues(oDestinationArea);

			Int32 iRows = oSourceArea.Rows.Count;

			for (Int32 iRow = 1; iRow <= iRows; iRow++)
			{
				// If the source cell doesn't contain a number, skip it.

				Double dSourceNumber;

				if ( !TryGetNumber(aoSourceAreaValues[iRow, 1],
					out dSourceNumber) )
				{
					continue;
				}

				Boolean bComparisonPassed = false;

				switch (comparisonOperator)
				{
					case ComparisonOperator.LessThan:

						bComparisonPassed =
							(dSourceNumber < sourceNumberToCompareTo);

						break;

					case ComparisonOperator.LessThanOrEqual:

						bComparisonPassed =
							(dSourceNumber <= sourceNumberToCompareTo);

						break;

					case ComparisonOperator.Equal:

						bComparisonPassed =
							(dSourceNumber == sourceNumberToCompareTo);

						break;

					case ComparisonOperator.NotEqual:

						bComparisonPassed =
							(dSourceNumber != sourceNumberToCompareTo);

						break;

					case ComparisonOperator.GreaterThan:

						bComparisonPassed =
							(dSourceNumber > sourceNumberToCompareTo);

						break;

					case ComparisonOperator.GreaterThanOrEqual:

						bComparisonPassed =
							(dSourceNumber >= sourceNumberToCompareTo);

						break;

					default:

						Debug.Assert(false);
						break;
				}

				if (bComparisonPassed)
				{
					if (destinationString1 != null)
					{
						aoDestinationAreaValues[iRow, 1] = destinationString1;
					}
				}
				else
				{
					if (destinationString2 != null)
					{
						aoDestinationAreaValues[iRow, 1] = destinationString2;
					}
				}
			}

			oDestinationArea.set_Value(Missing.Value, aoDestinationAreaValues);
		}
	}

    //*************************************************************************
    //  Method: MapViaCopy()
    //
    /// <summary>
	///	Maps the values in a source column to a destination column by copying
	/// them.
    /// </summary>
    ///
	/// <param name="table">
	///	Table containing the two columns.
	/// </param>
	///
	/// <param name="sourceColumnName">
	/// Name of the source column.
	/// </param>
	///
	/// <param name="destinationColumnName">
	/// Name of the destination column.
	/// </param>
	///
    /// <remarks>
	/// Each cell in the source column is copied to the destination column.
	///
	/// <para>
	/// If the source or destination column doesn't exist, this method does
	/// nothing.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

	public static void
	MapViaCopy
	(
		ListObject table,
		String sourceColumnName,
		String destinationColumnName
	)
	{
		Debug.Assert(table != null);
		Debug.Assert( !String.IsNullOrEmpty(sourceColumnName) );
		Debug.Assert( !String.IsNullOrEmpty(destinationColumnName) );

		Range oVisibleSourceRange, oVisibleDestinationRange;

		if ( !TryGetVisibleSourceAndDestination(table, sourceColumnName,
			destinationColumnName, out oVisibleSourceRange,
			out oVisibleDestinationRange) )
		{
			return;
		}

		// Loop through the areas.

		Int32 iAreas = oVisibleSourceRange.Areas.Count;

		Debug.Assert(iAreas == oVisibleDestinationRange.Areas.Count);

		for (Int32 iArea = 1; iArea <= iAreas; iArea++)
		{
			Range oSourceArea = oVisibleSourceRange.Areas[iArea];
			Range oDestinationArea = oVisibleDestinationRange.Areas[iArea];

			Debug.Assert(oSourceArea.Rows.Count ==
				oDestinationArea.Rows.Count);

			Object [,] aoSourceAreaValues =
				ExcelUtil.GetRangeValues(oSourceArea);

			oDestinationArea.set_Value(Missing.Value, aoSourceAreaValues);
		}
	}

    //*************************************************************************
    //  Method: TryGetVisibleSourceAndDestination()
    //
    /// <summary>
	///	Attempts to get the visible source and destination column ranges.
    /// </summary>
    ///
	/// <param name="oTable">
	///	Table containing the two columns.
	/// </param>
	///
	/// <param name="sSourceColumnName">
	/// Name of the source column.  The column must exist.
	/// </param>
	///
	/// <param name="sDestinationColumnName">
	/// Name of the destination column.  The column must exist.
	/// </param>
	///
	/// <param name="oVisibleSourceRange">
	/// Where the visible source range gets stored if true is returned.  May
	/// contain multiple areas.
	/// </param>
	///
	/// <param name="oVisibleDestinationRange">
	/// Where the visible destination range gets stored if true is returned.
	/// May contain multiple areas.
	/// </param>
	///
    /// <returns>
	/// true if the ranges were obtained.
    /// </returns>
    //*************************************************************************

	private static Boolean
	TryGetVisibleSourceAndDestination
	(
		ListObject oTable,
		String sSourceColumnName,
		String sDestinationColumnName,
		out Range oVisibleSourceRange,
		out Range oVisibleDestinationRange
	)
	{
		Debug.Assert(oTable != null);
		Debug.Assert( !String.IsNullOrEmpty(sSourceColumnName) );
		Debug.Assert( !String.IsNullOrEmpty(sDestinationColumnName) );

		oVisibleSourceRange = oVisibleDestinationRange = null;

		Range oSourceRange, oDestinationRange;

		return (
			ExcelUtil.TryGetTableColumnData(oTable, sSourceColumnName,
				out oSourceRange)
			&&
			ExcelUtil.TryGetTableColumnData(oTable, sDestinationColumnName,
				out oDestinationRange)
			&&
			ExcelUtil.TryGetVisibleRange(oSourceRange,
				out oVisibleSourceRange)
			&&
			ExcelUtil.TryGetVisibleRange(oDestinationRange,
				out oVisibleDestinationRange)
			);
	}

    //*************************************************************************
    //  Method: TryGetNumber()
    //
    /// <summary>
	///	Attempts to get a number from a cell value.
    /// </summary>
    ///
	/// <param name="oValue">
	///	Cell value.  Can be null.
	/// </param>
	///
	/// <param name="dNumber">
	/// Where the number gets stored if true is returned.
	/// </param>
	///
    /// <returns>
	/// true if the cell value was a number.
    /// </returns>
    //*************************************************************************

	private static Boolean
	TryGetNumber
	(
		Object oValue,
		out Double dNumber
	)
	{
		dNumber = Double.MinValue;

		if (oValue == null)
		{
			return (false);
		}

		switch ( oValue.GetType().ToString() )
		{
			case "System.String":

				return ( Double.TryParse( (String)oValue, out dNumber) );

			case "System.Double":

				dNumber = (Double)oValue;
				return (true);

			default:

				break;
		}

		return (false);
	}

    //*************************************************************************
    //  Method: TryGetSourceCalculationRange()
    //
    /// <summary>
	///	Attempts to get the range of source numbers to use when mapping a
	/// source column to a destination column.
    /// </summary>
    ///
	/// <param name="oVisibleSourceRange">
	///	Visible source column range.  May contain multiple areas.
	/// </param>
	///
	/// <param name="bUseSourceNumber1">
	/// If true, <paramref name="dSourceNumber1" /> should be used.  If false,
	/// the smallest number in the source column should be used.
	/// </param>
	///
	/// <param name="bUseSourceNumber2">
	/// If true, <paramref name="dSourceNumber2" /> should be used.  If false,
	/// the largest number in the source column should be used.
	/// </param>
	///
	/// <param name="dSourceNumber1">
	/// The first number to use in the source column.  Does not have to be less
	/// than <paramref name="dSourceNumber2" />.  Not valid if <paramref
	/// name="bUseSourceNumber1" /> is false.
	/// </param>
	///
	/// <param name="dSourceNumber2">
	/// The second number to use in the source column.  Does not have to be
	/// greater than <paramref name="dSourceNumber1" />.  Not valid if
	/// <paramref name="bUseSourceNumber2" /> is false.
	/// </param>
	///
	/// <param name="bIgnoreOutliers">
    /// true if outliers should be ignored in the source column.  Valid only if
	/// <paramref name="bUseSourceNumber1" /> and <paramref
	/// name="bUseSourceNumber2" /> are false.
	/// </param>
	///
	/// <param name="dSourceCalculationNumber1">
	/// Where the first number in the source calculation range gets stored if
	/// true is returned.  Does not have to be less than <paramref
	/// name="dSourceCalculationNumber2" />.
	/// </param>
	///
	/// <param name="dSourceCalculationNumber2">
	/// Where the second number in the source calculation range gets stored if
	/// true is returned.  Does not have to be greater than <paramref
	/// name="dSourceCalculationNumber1" />.
	/// </param>
	///
	/// <returns>
	/// true if the source calculation range was obtained.
	/// </returns>
    //*************************************************************************

	private static Boolean
	TryGetSourceCalculationRange
	(
		Range oVisibleSourceRange,
		Boolean bUseSourceNumber1,
		Boolean bUseSourceNumber2,
		Double dSourceNumber1,
		Double dSourceNumber2,
		Boolean bIgnoreOutliers,
		out Double dSourceCalculationNumber1,
		out Double dSourceCalculationNumber2
	)
    {
		Debug.Assert(oVisibleSourceRange != null);

		dSourceCalculationNumber1 = dSourceCalculationNumber2 =
			Double.MinValue;

		// If the caller wants to use the minimum or maximum values in the
		// source range, get those values.

		Int32 iSourceNumbers = Int32.MinValue;
		Double dMinimumSourceNumber = Double.MinValue;
		Double dMaximumSourceNumber = Double.MinValue;
		Double dMeanSourceNumber = Double.MinValue;

		if (!bUseSourceNumber1 || !bUseSourceNumber2)
		{
			if ( !TryGetSourceStatistics(oVisibleSourceRange,
				out iSourceNumbers, out dMinimumSourceNumber,
				out dMaximumSourceNumber, out dMeanSourceNumber) )
			{
				return (false);
			}
		}

		Boolean bIgnoringOutliers =
			!bUseSourceNumber1 && !bUseSourceNumber2 && bIgnoreOutliers;

		if (bIgnoringOutliers)
		{
			Debug.Assert(iSourceNumbers != Int32.MinValue);

			GetSourceRangeWithoutOutliers(oVisibleSourceRange, iSourceNumbers,
				dMinimumSourceNumber, dMaximumSourceNumber, dMeanSourceNumber,
				out dSourceCalculationNumber1, out dSourceCalculationNumber2);
		}
		else
		{
			dSourceCalculationNumber1 =
				bUseSourceNumber1 ? dSourceNumber1 : dMinimumSourceNumber;

			dSourceCalculationNumber2 =
				bUseSourceNumber2 ? dSourceNumber2 : dMaximumSourceNumber;
		}

		return (true);
    }

    //*************************************************************************
    //  Method: TryGetSourceStatistics()
    //
    /// <summary>
	///	Attempts to get statistics for the source column.
    /// </summary>
    ///
	/// <param name="oVisibleSourceRange">
	///	Visible source column range.  May contain multiple areas.
	/// </param>
	///
	/// <param name="iSourceNumbers">
	/// Where the number of numbers found in <paramref
	/// name="oVisibleSourceRange" /> gets stored if true is returned.
	/// </param>
	///
	/// <param name="dMinimumSourceNumber">
	/// Where the minimum number found in <paramref
	/// name="oVisibleSourceRange" /> gets stored if true is returned.
	/// </param>
	///
	/// <param name="dMaximumSourceNumber">
	/// Where the maximum number found in <paramref
	/// name="oVisibleSourceRange" /> gets stored if true is returned.
	/// </param>
	///
	/// <param name="dMeanSourceNumber">
	/// Where the mean of the numbers found in <paramref
	/// name="oVisibleSourceRange" /> gets stored if true is returned.
	/// </param>
	///
    /// <returns>
	/// true if at least one number was found in <paramref
	/// name="oVisibleSourceRange" />.
    /// </returns>
    //*************************************************************************

	private static Boolean
	TryGetSourceStatistics
	(
		Range oVisibleSourceRange,
		out Int32 iSourceNumbers,
		out Double dMinimumSourceNumber,
		out Double dMaximumSourceNumber,
		out Double dMeanSourceNumber
	)
	{
		Debug.Assert(oVisibleSourceRange != null);

		iSourceNumbers = 0;
		dMinimumSourceNumber = Double.MaxValue;
		dMaximumSourceNumber = Double.MinValue;
		dMeanSourceNumber = Double.MinValue;

		Double dSum = 0;

		foreach ( Double dSourceNumber in
			GetVisibleNumbers(oVisibleSourceRange) )
		{
			iSourceNumbers++;

			dSum += dSourceNumber;

			dMinimumSourceNumber =
				Math.Min(dMinimumSourceNumber, dSourceNumber);

			dMaximumSourceNumber =
				Math.Max(dMaximumSourceNumber, dSourceNumber);
		}

		if (iSourceNumbers > 0)
		{
			dMeanSourceNumber = dSum / iSourceNumbers;

			return (true);
		}

		return (false);
	}

    //*************************************************************************
    //  Method: GetSourceStandardDeviation()
    //
    /// <summary>
	///	Gets the standard deviation of the of numbers in the source column.
    /// </summary>
    ///
	/// <param name="oVisibleSourceRange">
	///	Visible source column range.  May contain multiple areas.
	/// </param>
	///
	/// <param name="iSourceNumbers">
	/// The number of numbers found in <paramref name="oVisibleSourceRange" />.
	/// Must be greater than zero.
	/// </param>
	///
	/// <param name="dMinimumSourceNumber">
	/// The minimum number found in <paramref name="oVisibleSourceRange" />.
	/// </param>
	///
	/// <param name="dMaximumSourceNumber">
	/// The maximum number found in <paramref name="oVisibleSourceRange" />.
	/// </param>
	///
	/// <param name="dMeanSourceNumber">
	/// The mean of the numbers found in <paramref
	/// name="oVisibleSourceRange" />.
	/// </param>
	///
	/// <returns>
	/// The standard deviation of the numbers in the source column.
	/// </returns>
    //*************************************************************************

	private static Double
	GetSourceStandardDeviation
	(
		Range oVisibleSourceRange,
		Int32 iSourceNumbers,
		Double dMinimumSourceNumber,
		Double dMaximumSourceNumber,
		Double dMeanSourceNumber
	)
	{
		Debug.Assert(oVisibleSourceRange != null);
		Debug.Assert(iSourceNumbers > 0);
		Debug.Assert(dMaximumSourceNumber >= dMinimumSourceNumber);

		Double dSumOfSquares = 0;

		foreach ( Double dSourceNumber in
			GetVisibleNumbers(oVisibleSourceRange) )
		{
			Double dDifferenceFromMean = dSourceNumber - dMeanSourceNumber;

			dSumOfSquares += (dDifferenceFromMean * dDifferenceFromMean);
		}

		return ( Math.Sqrt(dSumOfSquares / (Double)iSourceNumbers) );
	}

    //*************************************************************************
    //  Method: GetSourceRangeWithoutOutliers()
    //
    /// <summary>
	///	Gets a range of numbers in the source column that excludes outliers.
    /// </summary>
    ///
	/// <param name="oVisibleSourceRange">
	///	Visible source column range.  May contain multiple areas.
	/// </param>
	///
	/// <param name="iSourceNumbers">
	/// The number of numbers found in <paramref name="oVisibleSourceRange" />.
	/// Must be greater than zero.
	/// </param>
	///
	/// <param name="dMinimumSourceNumber">
	/// The minimum number found in <paramref name="oVisibleSourceRange" />.
	/// </param>
	///
	/// <param name="dMaximumSourceNumber">
	/// The maximum number found in <paramref name="oVisibleSourceRange" />.
	/// </param>
	///
	/// <param name="dMeanSourceNumber">
	/// The mean of the numbers found in <paramref
	/// name="oVisibleSourceRange" />.
	/// </param>
	///
	/// <param name="dSourceRangeMinimum">
	/// Where the minimum number in the computed range gets stored.
	/// </param>
	///
	/// <param name="dSourceRangeMaximum">
	/// Where the maximum number in the computed range gets stored.
	/// </param>
    //*************************************************************************

	private static void
	GetSourceRangeWithoutOutliers
	(
		Range oVisibleSourceRange,
		Int32 iSourceNumbers,
		Double dMinimumSourceNumber,
		Double dMaximumSourceNumber,
		Double dMeanSourceNumber,
		out Double dSourceRangeMinimum,
		out Double dSourceRangeMaximum
	)
	{
		Debug.Assert(oVisibleSourceRange != null);
		Debug.Assert(iSourceNumbers > 0);
		Debug.Assert(dMaximumSourceNumber >= dMinimumSourceNumber);

		dSourceRangeMinimum = Double.MaxValue;
		dSourceRangeMaximum = Double.MinValue;

		// Include only those source numbers that are within one standard
		// deviation of the mean.

		Double dStandardDeviation = GetSourceStandardDeviation(
			oVisibleSourceRange, iSourceNumbers, dMinimumSourceNumber,
				dMaximumSourceNumber, dMeanSourceNumber);

		Double dHalfRange = 1.0 * dStandardDeviation;

		Double dWithinRangeMinimum = dMeanSourceNumber - dHalfRange;
		Double dWithinRangeMaximum = dMeanSourceNumber + dHalfRange;

		foreach ( Double dSourceNumber in
			GetVisibleNumbers(oVisibleSourceRange) )
		{
			if (dSourceNumber >= dWithinRangeMinimum &&
				dSourceNumber <= dWithinRangeMaximum)
			{
				dSourceRangeMinimum =
					Math.Min(dSourceRangeMinimum, dSourceNumber);

				dSourceRangeMaximum =
					Math.Max(dSourceRangeMaximum, dSourceNumber);
			}
		}

		if (dSourceRangeMinimum == Double.MaxValue)
		{
			// There were no numbers within range.

			dSourceRangeMinimum = dWithinRangeMinimum;
		}

		if (dSourceRangeMaximum == Double.MinValue)
		{
			dSourceRangeMaximum = dWithinRangeMaximum;
		}
	}

    //*************************************************************************
    //  Method: GetVisibleNumbers()
    //
    /// <summary>
	///	Gets an enumerator for enumerating the numbers in a visible range.
    /// </summary>
    ///
	/// <param name="oVisibleRange">
	///	Visible range.  May contain multiple areas.
	/// </param>
	///
	/// <returns>
	///	An enumerator for enumerating the numbers in <paramref
	/// name="oVisibleRange" />.
	/// </returns>
    //*************************************************************************

	private static IEnumerable<Double>
	GetVisibleNumbers
	(
		Range oVisibleRange
	)
	{
		Debug.Assert(oVisibleRange != null);

		// Loop through the areas.

		foreach (Range oArea in oVisibleRange.Areas)
		{
			Object [,] aoAreaValues = ExcelUtil.GetRangeValues(oArea);

			Int32 iRows = aoAreaValues.GetUpperBound(0);

			for (Int32 iRow = 1; iRow <= iRows; iRow++)
			{
				Double dNumber;

				if ( TryGetNumber(aoAreaValues[iRow, 1], out dNumber) )
				{
					yield return (dNumber);
				}
			}
		}
	}

    //*************************************************************************
    //  Method: GetColorGradientMapper()
    //
    /// <summary>
	/// Gets an object that maps a range of numbers to a range of colors.
    /// </summary>
    ///
	/// <param name="dSourceCalculationNumber1">
	/// The first number in the source calculation range.  Does not have to be
	/// less than <paramref name="dSourceCalculationNumber2" />.
	/// </param>
	///
	/// <param name="dSourceCalculationNumber2">
	/// The second number in the source calculation range.  Does not have to be
	/// greater than <paramref name="dSourceCalculationNumber1" />.
	/// </param>
	///
	/// <param name="oDestinationColor1">
	/// The first color to use in the destination column.
	/// </param>
	///
	/// <param name="oDestinationColor2">
	/// The second color to use in the destination column.
	/// </param>
	///
    /// <returns>
	/// A new ColorGradientMapper object.
    /// </returns>
    //*************************************************************************

	private static ColorGradientMapper
	GetColorGradientMapper
	(
		Double dSourceCalculationNumber1,
		Double dSourceCalculationNumber2,
		Color oDestinationColor1,
		Color oDestinationColor2
	)
	{
		ColorGradientMapper oColorGradientMapper = new ColorGradientMapper();

		if (dSourceCalculationNumber1 == dSourceCalculationNumber2)
		{
			// ColorGradientMapper doesn't allow a zero-width number range.

			dSourceCalculationNumber2 += 0.1;
		}

		const Int32 DiscreteColorCount = 40;

		if (dSourceCalculationNumber1 < dSourceCalculationNumber2)
		{
			oColorGradientMapper.Initialize(dSourceCalculationNumber1,
				dSourceCalculationNumber2, oDestinationColor1,
				oDestinationColor2, DiscreteColorCount, false);
		}
		else
		{
			// ColorGradientMapper doesn't allow a backwards number range.
			// Reverse both the number range and colors.

			oColorGradientMapper.Initialize(dSourceCalculationNumber2,
				dSourceCalculationNumber1, oDestinationColor2,
				oDestinationColor1, DiscreteColorCount, false);
		}

		return (oColorGradientMapper);
	}


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
