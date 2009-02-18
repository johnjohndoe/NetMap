using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphMetricValue
//
/// <summary>
/// Base class for a family of classes that store one graph metric value for
/// one worksheet row.
/// </summary>
///
/// <remarks>
/// The derived classes differ in how the graph metric values are mapped to
/// worksheet rows.
/// </remarks>
//*****************************************************************************

public abstract class GraphMetricValue : Object
{
    //*************************************************************************
    //  Constructor: GraphMetricValue()
    //
    /// <overloads>
    /// Initializes a new instance of the <see cref="GraphMetricValue" />
    /// class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMetricValue" />
    /// class with default values.
    /// </summary>
    //*************************************************************************

    public GraphMetricValue()
    :
    this (null)
    {
        // (Do nothing else.)
    }

    //*************************************************************************
    //  Constructor: GraphMetricValue()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMetricValue" />
    /// class with specified values.
    /// </summary>
    ///
    /// <param name="value">
    /// Graph metric value for the row.
    /// </param>
    //*************************************************************************

    public GraphMetricValue
    (
        Object value
    )
    {
        m_oValue = value;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: Value
    //
    /// <summary>
    /// Gets or sets the graph metric value for the worksheet row.
    /// </summary>
    ///
    /// <value>
    /// The graph metric value for the worksheet row.  The default is null.
    /// </value>
    //*************************************************************************

    public Object
    Value
    {
        get
        {
            AssertValid();

            return (m_oValue);
        }

        set
        {
            m_oValue = value;

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

    [Conditional("DEBUG")]

    public virtual void
    AssertValid()
    {
        // m_oValue
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Graph metric value for the worksheet row.  This is an Object because
    /// Excel cell values can be one of several types.   This does not
    /// introduce boxing, because Excel's API uses the Object type for cell
    /// values and so boxing occurs no matter what.

    protected Object m_oValue;
}

}
