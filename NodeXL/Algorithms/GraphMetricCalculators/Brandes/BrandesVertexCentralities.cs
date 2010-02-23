
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: BrandesVertexCentralities
//
/// <summary>
/// Stores the centrality metrics that are calculated by <see
/// cref="BrandesFastCentralityCalculator" /> for one vertex.
/// </summary>
//*****************************************************************************

public class BrandesVertexCentralities : Object
{
    //*************************************************************************
    //  Constructor: BrandesVertexCentralities()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="BrandesVertexCentralities" /> class.
    /// </summary>
    //*************************************************************************

    public BrandesVertexCentralities()
    {
        m_dBetweennessCentrality = 0;
        m_dClosenessCentrality = 0;

        AssertValid();
    }

    //*************************************************************************
    //  Property: BetweennessCentrality
    //
    /// <summary>
    /// Gets or sets the vertex's betweenness centrality.
    /// </summary>
    ///
    /// <value>
    /// The vertex's betweenness centrality, as a Double.  The default is 0.
    /// </value>
    //*************************************************************************

    public Double
    BetweennessCentrality
    {
        get
        {
            AssertValid();

            return (m_dBetweennessCentrality);
        }

        set
        {
            m_dBetweennessCentrality = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ClosenessCentrality
    //
    /// <summary>
    /// Gets or sets the vertex's closeness centrality.
    /// </summary>
    ///
    /// <value>
    /// The vertex's closeness centrality, as a Double.  The default is 0.
    /// </value>
    //*************************************************************************

    public Double
    ClosenessCentrality
    {
        get
        {
            AssertValid();

            return (m_dClosenessCentrality);
        }

        set
        {
            m_dClosenessCentrality = value;

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

    public void
    AssertValid()
    {
        Debug.Assert(m_dBetweennessCentrality >= 0);
        Debug.Assert(m_dClosenessCentrality >= 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Betweenness centrality.

    protected Double m_dBetweennessCentrality;

    /// Closeness centrality.

    protected Double m_dClosenessCentrality;
}

}
