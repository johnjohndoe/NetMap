
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: DateTimeFilterParameters
//
/// <summary>
/// Stores parameters for a date/time dynamic filter.
/// </summary>
///
/// <remarks>
/// A dynamic filter is a control that can be adjusted to selectively show and
/// hide edges and vertices in the graph in real time.  There is one control
/// for each filterable column in the workbook.  If the column is date, time,
/// or date and time, an instance of this class is used to store the control's
/// parameters.
///
/// <para>
/// Use the <see cref="DynamicFilterUtil" /> class to get collections of filter
/// parameters.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class DateTimeFilterParameters : NumericFilterParameters
{
    //*************************************************************************
    //  Constructor: DateTimeFilterParameters()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="DateTimeFilterParameters" /> class.
    /// </summary>
	///
    /// <param name="columnName">
    /// Name of the column that can be filtered on.
    /// </param>
	///
    /// <param name="minimumCellValue">
    /// Minimum cell value in the column, in Excel's date/time format.  Sample:
	/// 39448.583333 (1/1/2008, 2 PM).
    /// </param>
	///
    /// <param name="maximumCellValue">
    /// Maximum cell value in the column, in Excel's date/time format.
    /// </param>
	///
    /// <param name="format">
    /// The column format.
    /// </param>
    //*************************************************************************

    public DateTimeFilterParameters
	(
		String columnName,
		Double minimumCellValue,
		Double maximumCellValue,
		ExcelColumnFormat format
	)
	: base(columnName, minimumCellValue, maximumCellValue, 0)
    {
		m_eFormat = format;

		AssertValid();
    }

    //*************************************************************************
    //  Property: Format
    //
    /// <summary>
    /// Gets the format of the column.
    /// </summary>
    ///
    /// <value>
    /// The format of the column, as an <see cref="ExcelColumnFormat" />.
    /// </value>
    //*************************************************************************

    public ExcelColumnFormat
    Format
    {
        get
        {
            AssertValid();

            return (m_eFormat);
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
		// m_eFormat
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The format of the column.

	protected ExcelColumnFormat m_eFormat;
}

}
