
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NumericValueConverterBase
//
/// <summary>
/// Base class for a family of classes that convert numeric values in the Excel
/// workbook to and from numeric values in the NodeXL graph.
/// </summary>
//*****************************************************************************

public class NumericValueConverterBase : Object
{
    //*************************************************************************
    //  Constructor: NumericValueConverterBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="NumericValueConverterBase" /> class.
    /// </summary>
    ///
    /// <param name="minimumValueWorkbook">
    /// Minimum value that can be specified in the workbook.
    /// </param>
    ///
    /// <param name="maximumValueWorkbook">
    /// Maximum value that can be specified in the workbook.  Must be greater
    /// than <paramref name="minimumValueWorkbook" />.
    /// </param>
    ///
    /// <param name="minimumValueGraph">
    /// Minimum value in the NodeXL graph.
    /// </param>
    ///
    /// <param name="maximumValueGraph">
    /// Maximum value in the NodeXL graph.  Must be greater than <paramref
    /// name="minimumValueGraph" />.
    /// </param>
    //*************************************************************************

    public NumericValueConverterBase
    (
        Single minimumValueWorkbook,
        Single maximumValueWorkbook,
        Single minimumValueGraph,
        Single maximumValueGraph
    )
    {
        m_fMinimumValueWorkbook = minimumValueWorkbook;
        m_fMaximumValueWorkbook = maximumValueWorkbook;
        m_fMinimumValueGraph = minimumValueGraph;
        m_fMaximumValueGraph = maximumValueGraph;

        // AssertValid();
    }

    //*************************************************************************
    //  Method: WorkbookToGraph()
    //
    /// <summary>
    /// Converts an Excel workbook value to a value suitable for use in a NodeXL
    /// graph.
    /// </summary>
    ///
    /// <param name="valueWorkbook">
    /// Value read from the Excel workbook.
    /// </param>
    ///
    /// <returns>
    /// A value suitable for use in a NodeXL graph.  The value is pinned to the
    /// graph limits specified in the constructor.
    /// </returns>
    //*************************************************************************

    public Single
    WorkbookToGraph
    (
        Single valueWorkbook
    )
    {
        AssertValid();

        return ( RangeAToRangeB(valueWorkbook, m_fMinimumValueWorkbook,
            m_fMaximumValueWorkbook, m_fMinimumValueGraph,
            m_fMaximumValueGraph) );
    }

    //*************************************************************************
    //  Method: GraphToWorkbook()
    //
    /// <summary>
    /// Converts a NodeXL graph value to a value suitable for use in an Excel
    /// workbook.
    /// </summary>
    ///
    /// <param name="valueGraph">
    /// Value stored in a NodeXL graph.
    /// </param>
    ///
    /// <returns>
    /// A value suitable for use in an Excel workbook.  The value is pinned to
    /// the workbook limits specified in the constructor.
    /// </returns>
    //*************************************************************************

    public Single
    GraphToWorkbook
    (
        Single valueGraph
    )
    {
        AssertValid();

        return ( RangeAToRangeB(valueGraph, m_fMinimumValueGraph,
            m_fMaximumValueGraph, m_fMinimumValueWorkbook,
            m_fMaximumValueWorkbook) );
    }

    //*************************************************************************
    //  Method: RangeAToRangeB()
    //
    /// <summary>
    /// Linearly transforms a value from one range to another.
    /// </summary>
    ///
    /// <param name="fValueRangeA">
    /// Value to transform, in range A.
    /// </param>
    ///
    /// <param name="fMinimumValueRangeA">
    /// Minimum value in range A.
    /// </param>
    ///
    /// <param name="fMaximumValueRangeA">
    /// Maximum value in range A.
    /// </param>
    ///
    /// <param name="fMinimumValueRangeB">
    /// Minimum value in range B.
    /// </param>
    ///
    /// <param name="fMaximumValueRangeB">
    /// Maximum value in range B.
    /// </param>
    ///
    /// <returns>
    /// The corresponding value in range B.
    /// </returns>
    //*************************************************************************

    protected Single
    RangeAToRangeB
    (
        Single fValueRangeA,
        Single fMinimumValueRangeA,
        Single fMaximumValueRangeA,
        Single fMinimumValueRangeB,
        Single fMaximumValueRangeB
    )
    {
        Debug.Assert(fMaximumValueRangeA != fMinimumValueRangeA);
        AssertValid();

        Single fValueRangeB =

            fMinimumValueRangeB + (fValueRangeA - fMinimumValueRangeA) *
            
            (fMaximumValueRangeB - fMinimumValueRangeB) /
                (fMaximumValueRangeA - fMinimumValueRangeA);

        // Pin the value to the range B limits.

        fValueRangeB = Math.Max(fValueRangeB, fMinimumValueRangeB);
        fValueRangeB = Math.Min(fValueRangeB, fMaximumValueRangeB);

        return (fValueRangeB);
    }

    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public virtual void
    AssertValid()
    {
        Debug.Assert(m_fMaximumValueWorkbook > m_fMinimumValueWorkbook);
        Debug.Assert(m_fMaximumValueGraph > m_fMinimumValueGraph);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Minimum value that can be specified in the workbook.

    protected Single m_fMinimumValueWorkbook;

    /// Maximum value that can be specified in the workbook.

    protected Single m_fMaximumValueWorkbook;

    /// Minimum value in the NodeXL graph.

    protected Single m_fMinimumValueGraph;

    /// Maximum value in the NodeXL graph.

    protected Single m_fMaximumValueGraph;
}

}
