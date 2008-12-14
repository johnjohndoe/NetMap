
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NumericFilterParameters
//
/// <summary>
/// Stores parameters for a numeric dynamic filter.
/// </summary>
///
/// <remarks>
/// A dynamic filter is a control that can be adjusted to selectively show and
/// hide edges and vertices in the graph in real time.  There is one control
/// for each filterable column in the workbook.  If the column is numeric, an
/// instance of this class is used to store the control's parameters.
///
/// <para>
/// Use the <see cref="DynamicFilterUtil" /> class to get collections of filter
/// parameters.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class NumericFilterParameters : RangeFilterParameters<Double>
{
    //*************************************************************************
    //  Constructor: NumericFilterParameters()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="NumericFilterParameters" /> class.
    /// </summary>
	///
    /// <param name="columnName">
    /// Name of the column that can be filtered on.
    /// </param>
	///
    /// <param name="minimumCellValue">
    /// Minimum cell value in the column.
    /// </param>
	///
    /// <param name="maximumCellValue">
    /// Maximum cell value in the column.
    /// </param>
    //*************************************************************************

    public NumericFilterParameters
	(
		String columnName,
		Double minimumCellValue,
		Double maximumCellValue
	)
	: base(columnName, minimumCellValue, maximumCellValue)
    {
		// (Do nothing else.)

		AssertValid();
    }
}

}
