
//	Copyright (c) Microsoft Corporation.  All rights reserved.

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
    /// <param name="minimumWorkbookValue">
	/// Minimum value that can be specified in the workbook.
    /// </param>
	///
    /// <param name="maximumWorkbookValue">
	/// Maximum value that can be specified in the workbook.  Must be greater
	/// than <paramref name="minimumWorkbookValue" />.
    /// </param>
	///
    /// <param name="minimumGraphValue">
	/// Minimum value in the NodeXL graph.
    /// </param>
	///
    /// <param name="maximumGraphValue">
	/// Maximum value in the NodeXL graph.  Must be greater than <paramref
	/// name="minimumGraphValue" />.
    /// </param>
    //*************************************************************************

    public NumericValueConverterBase
	(
		Single minimumWorkbookValue,
		Single maximumWorkbookValue,
		Single minimumGraphValue,
		Single maximumGraphValue
	)
    {
		m_fMinimumWorkbookValue = minimumWorkbookValue;
		m_fMaximumWorkbookValue = maximumWorkbookValue;
		m_fMinimumGraphValue = minimumGraphValue;
		m_fMaximumGraphValue = maximumGraphValue;

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
    /// <param name="workbookValue">
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
		Single workbookValue
    )
    {
        AssertValid();

		Single fGraphValue =

			m_fMinimumGraphValue + (workbookValue - m_fMinimumWorkbookValue) *
			
			(m_fMaximumGraphValue - m_fMinimumGraphValue) /
				(m_fMaximumWorkbookValue - m_fMinimumWorkbookValue);

		// Pin the value to the graph limits.

		fGraphValue = Math.Max(fGraphValue, m_fMinimumGraphValue);
		fGraphValue = Math.Min(fGraphValue, m_fMaximumGraphValue);

		return (fGraphValue);
    }

    //*************************************************************************
    //  Method: GraphToWorkbook()
    //
    /// <summary>
	/// Converts a NodeXL graph value to a value suitable for use in an Excel
	/// workbook.
    /// </summary>
    ///
    /// <param name="graphValue">
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
		Single graphValue
    )
    {
        AssertValid();

		Single fWorkbookValue =

			m_fMinimumWorkbookValue + (graphValue - m_fMinimumGraphValue) *
			
			(m_fMaximumWorkbookValue - m_fMinimumWorkbookValue) /
				(m_fMaximumGraphValue - m_fMinimumGraphValue);

		// Pin the value to the graph limits.

		fWorkbookValue = Math.Max(fWorkbookValue, m_fMinimumWorkbookValue);
		fWorkbookValue = Math.Min(fWorkbookValue, m_fMaximumWorkbookValue);

		return (fWorkbookValue);
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
		Debug.Assert(m_fMaximumWorkbookValue > m_fMinimumWorkbookValue);
		Debug.Assert(m_fMaximumGraphValue > m_fMinimumGraphValue);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Minimum value that can be specified in the workbook.

	protected Single m_fMinimumWorkbookValue;

	/// Maximum value that can be specified in the workbook.

	protected Single m_fMaximumWorkbookValue;

	/// Minimum value in the NodeXL graph.

	protected Single m_fMinimumGraphValue;

	/// Maximum value in the NodeXL graph.

	protected Single m_fMaximumGraphValue;
}

}
