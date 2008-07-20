using System;
using System.Diagnostics;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphMetricValueOrdered
//
/// <summary>
/// Stores one graph metric value for one worksheet row, where the value is
/// mapped to the next empty row.
/// </summary>
///
/// <remarks>
/// The array of <see cref="GraphMetricValueOrdered" /> objects within <see
/// cref="GraphMetricColumnOrdered" /> should be written to the worksheet in
/// sequential row order -- the first value in the first row, the second value
/// in the second row, and so on.
///
/// <para>
/// This class does not add anything to the <see cref="GraphMetricValue" />
/// base class.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class GraphMetricValueOrdered : GraphMetricValue
{
    //*************************************************************************
    //  Constructor: GraphMetricValueOrdered()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
	/// cref="GraphMetricValueOrdered" /> class.
    /// </overloads>
	///
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="GraphMetricValueOrdered" /> class with default values.
    /// </summary>
    //*************************************************************************

    public GraphMetricValueOrdered()
	:
	this (null)
    {
		// (Do nothing else.)
    }

    //*************************************************************************
    //  Constructor: GraphMetricValueOrdered()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="GraphMetricValueOrdered" /> class with specified values.
    /// </summary>
	///
	/// <param name="value">
	/// Graph metric value for the row.
	/// </param>
    //*************************************************************************

    public GraphMetricValueOrdered
	(
		Object value
	)
	: base(value)
    {
		// (Do nothing else.)

		AssertValid();
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

		// (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
