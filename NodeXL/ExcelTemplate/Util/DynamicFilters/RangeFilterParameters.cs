
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: RangeFilterParameters
//
/// <summary>
/// Stores parameters for a dynamic filter that spans a range of values.
/// </summary>
///
/// <remarks>
/// This is a generic base class.  The derived type must specify the type of
/// the values spanned by the dynamic filter.
///
/// <para>
/// A dynamic filter is a control that can be adjusted to selectively show and
/// hide edges and vertices in the graph in real time.  There is one control
/// for each filterable column in the workbook.  If the column spans a range of
/// values, an instance of a class derived from this one is used to store the
/// control's parameters.
/// </para>
///
/// <para>
/// Use the <see cref="DynamicFilterUtil" /> class to get collections of filter
/// parameters.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class RangeFilterParameters<T> : DynamicFilterParameters
    where T : IComparable
{
    //*************************************************************************
    //  Constructor: RangeFilterParameters()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="T:RangeFilterParameters" /> class.
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

    public RangeFilterParameters
    (
        String columnName,
        T minimumCellValue,
        T maximumCellValue
    )
    : base(columnName)
    {
        m_oMinimumCellValue = minimumCellValue;
        m_oMaximumCellValue = maximumCellValue;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: MinimumCellValue
    //
    /// <summary>
    /// Gets the minimum cell value in the column.
    /// </summary>
    ///
    /// <value>
    /// The minimum cell value in the column.
    /// </value>
    //*************************************************************************

    public T
    MinimumCellValue
    {
        get
        {
            AssertValid();

            return (m_oMinimumCellValue);
        }
    }

    //*************************************************************************
    //  Property: MaximumCellValue
    //
    /// <summary>
    /// Gets the maximum cell value in the column.
    /// </summary>
    ///
    /// <value>
    /// The maximum cell value in the column.
    /// </value>
    //*************************************************************************

    public T
    MaximumCellValue
    {
        get
        {
            AssertValid();

            return (m_oMaximumCellValue);
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

        Debug.Assert(m_oMaximumCellValue.CompareTo(m_oMinimumCellValue) >= 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Minimum cell value in the column.

    protected T m_oMinimumCellValue;

    /// Maximum cell value in the column.

    protected T m_oMaximumCellValue;
}

}
