using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
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
	/// cref="GraphMetricValueOrdered" /> class with a specified value.
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
	: this(value, null)
    {
		// (Do nothing else.)
    }

    //*************************************************************************
    //  Constructor: GraphMetricValueOrdered()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="GraphMetricValueOrdered" /> class with a specified value and
	/// style.
    /// </summary>
	///
	/// <param name="value">
	/// Graph metric value for the row.
	/// </param>
	///
	/// <param name="style">
	/// Style of the row, or null to not apply a style.  Sample: "Bad".  The
	/// style overrides any style applied to the column.
	/// </param>
	///
	/// <remarks>
	/// If a style is ever applied to a row, a style should always be applied
	/// to the row.  If the "Bad" style is applied when the row is bad, for
	/// example, using a default null style when the row is good would always
	/// leave the bad style in place.  Instead, the "Normal" style should be
	/// applied when the row is good.
	/// </remarks>
    //*************************************************************************

    public GraphMetricValueOrdered
	(
		Object value,
		String style
	)
	: base(value)
    {
		m_sStyle = style;

		AssertValid();
    }

    //*************************************************************************
    //  Property: Style
    //
    /// <summary>
    /// Gets or sets the style of the row.
    /// </summary>
    ///
    /// <value>
	/// The style of the row, or null to not apply a style.  Sample: "Bad".
    /// </value>
	///
	/// <remarks>
	/// If a style is ever applied to a row, a style should always be applied
	/// to the row.  If the "Bad" style is applied when the row is bad, for
	/// example, using a default null style when the row is good would always
	/// leave the bad style in place.  Instead, the "Normal" style should be
	/// applied when the row is good.
	/// </remarks>
    //*************************************************************************

    public String
    Style
    {
        get
        {
            AssertValid();

            return (m_sStyle);
        }

        set
        {
			m_sStyle = value;

            AssertValid();
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

		// m_sStyle
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Style of the cell, or null to not apply a style.  Sample: "Bad".

	protected String m_sStyle;
}

}
