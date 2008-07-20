
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NetMap.Adapters
{
public partial class PajekGraphAdapter : GraphAdapterBase, IGraphAdapter
{
//*****************************************************************************
//  Embedded struct: PajekEdgeData
//
/// <summary>
/// Stores information about one edge in a Pajek file.
/// </summary>
///
/// <remarks>
/// This structure is embedded within the <see cref="PajekGraphAdapter" />
/// class and is meant for use only within the class.
/// </remarks>
//*****************************************************************************

protected struct PajekEdgeData
{
    //*************************************************************************
    //  Constructor: PajekEdgeData()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="PajekEdgeData" /> class.
    /// </summary>
	///
	/// <param name="firstVertexNumber">
	/// Vertex number of the edge's first vertex.  The vertex number is the
	/// first field of the lines within the *vertices section and ranges from
	/// 1 to the number of vertices.
	/// </param>
	///
	/// <param name="secondVertexNumber">
	/// Vertex number of the edge's second vertex.
	/// </param>
	///
	/// <param name="weight">
	/// Edge weight.
	/// </param>
    //*************************************************************************

    public PajekEdgeData
	(
		Int32 firstVertexNumber,
		Int32 secondVertexNumber,
		Single weight
	)
    {
		m_iFirstVertexNumber = firstVertexNumber;
		m_iSecondVertexNumber = secondVertexNumber;
		m_fWeight = weight;

		AssertValid();
    }

    //*************************************************************************
    //  Property: FirstVertexNumber
    //
    /// <summary>
    /// Gets the vertex number of the first vertex.
    /// </summary>
    ///
    /// <value>
    /// The vertex number of the edges's first vertex.
    /// </value>
    //*************************************************************************

    public Int32
    FirstVertexNumber
    {
        get
        {
            AssertValid();

            return (m_iFirstVertexNumber);
        }
    }

    //*************************************************************************
    //  Property: SecondVertexNumber
    //
    /// <summary>
    /// Gets the vertex number of the second vertex.
    /// </summary>
    ///
    /// <value>
    /// The vertex number of the edges's second vertex.
    /// </value>
    //*************************************************************************

    public Int32
    SecondVertexNumber
    {
        get
        {
            AssertValid();

            return (m_iSecondVertexNumber);
        }
    }

    //*************************************************************************
    //  Property: Weight
    //
    /// <summary>
    /// Gets the edge's weight.
    /// </summary>
    ///
    /// <value>
    /// The edge's weight.
    /// </value>
    //*************************************************************************

    public Single
    Weight
    {
        get
        {
            AssertValid();

            return (m_fWeight);
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

    public void
    AssertValid()
    {
		Debug.Assert(m_iFirstVertexNumber > 0);
		Debug.Assert(m_iSecondVertexNumber > 0);
		// m_fWeight
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Number of the first vertex.

	private Int32 m_iFirstVertexNumber;

	/// Number of the second vertex.

	private Int32 m_iSecondVertexNumber;

	/// Edge weight.

	private Single m_fWeight;
}

}

}
