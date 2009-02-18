using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphMetricValueWithID
//
/// <summary>
/// Stores one graph metric value for one worksheet row, where the value is
/// mapped to a row by a row ID.
/// </summary>
///
/// <remarks>
/// The graph metric value should be written to the worksheet row identified
/// by the <see cref="RowID" /> property.  (Row IDs get written to the
/// worksheet when the worksheet is read by <see cref="WorkbookReader" />.)
/// </remarks>
//*****************************************************************************

public class GraphMetricValueWithID : GraphMetricValue
{
    //*************************************************************************
    //  Constructor: GraphMetricValueWithID()
    //
    /// <overloads>
    /// Initializes a new instance of the <see cref="GraphMetricValueWithID" />
    /// class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMetricValueWithID" />
    /// class with default values.
    /// </summary>
    //*************************************************************************

    public GraphMetricValueWithID()
    :
    this (1, null)
    {
        // (Do nothing else.)
    }

    //*************************************************************************
    //  Constructor: GraphMetricValueWithID()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMetricValueWithID" />
    /// class with specified values.
    /// </summary>
    ///
    /// <param name="rowID">
    /// ID stored in the worksheet row.
    /// </param>
    ///
    /// <param name="value">
    /// Graph metric value for the row.
    /// </param>
    //*************************************************************************

    public GraphMetricValueWithID
    (
        Int32 rowID,
        Object value
    )
    : base (value)
    {
        m_iRowID = rowID;

        AssertValid();
    }

    //*************************************************************************
    //  Property: RowID
    //
    /// <summary>
    /// Gets or sets the ID stored in the worksheet row.
    /// </summary>
    ///
    /// <value>
    /// The ID stored in the worksheet row.  The default is 1.
    /// </value>
    ///
    /// <remarks>
    /// The ID stored in the edge worksheet row is also stored in the
    /// IEdge.Tag property, as an Int32.  The ID stored in the vertex worksheet
    /// is also stored in the IVertex.Tag property, as an Int32.
    /// </remarks>
    //*************************************************************************

    public Int32
    RowID
    {
        get
        {
            AssertValid();

            return (m_iRowID);
        }

        set
        {
            m_iRowID = value;

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

        Debug.Assert(m_iRowID >= 1);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// ID stored in the worksheet row.

    protected Int32 m_iRowID;
}

}
