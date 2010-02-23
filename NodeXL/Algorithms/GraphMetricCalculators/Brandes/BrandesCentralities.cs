
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: BrandesCentralities
//
/// <summary>
/// Stores the centrality metrics that are calculated by <see
/// cref="BrandesFastCentralityCalculator" />.
/// </summary>
//*****************************************************************************

public class BrandesCentralities : Object
{
    //*************************************************************************
    //  Constructor: BrandesCentralities()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="BrandesCentralities" />
    /// class.
    /// </summary>
    ///
    /// <param name="vertexCentralities">
    /// There is one key/value pair for each vertex in the graph.  The key is
    /// the IVertex.ID and the value is a <see
    /// cref="BrandesVertexCentralities" /> object containing the
    /// vertex's centralities.
    /// </param>
    ///
    /// <param name="maximumGeodesicDistance">
    /// The maximum geodesic distance in the graph, or null if not available.
    /// </param>
    ///
    /// <param name="averageGeodesicDistance">
    /// The average geodesic distance in the graph, or null if not available.
    /// </param>
    //*************************************************************************

    public BrandesCentralities
    (
        Dictionary<Int32, BrandesVertexCentralities> vertexCentralities,
        Nullable<Int32> maximumGeodesicDistance,
        Nullable<Single> averageGeodesicDistance
    )
    {
        m_oVertexCentralities = vertexCentralities;
        m_iMaximumGeodesicDistance = maximumGeodesicDistance;
        m_fAverageGeodesicDistance = averageGeodesicDistance;

        AssertValid();
    }

    //*************************************************************************
    //  Property: VertexCentralities
    //
    /// <summary>
    /// Gets the centralities for the graph's vertices.
    /// </summary>
    ///
    /// <value>
    /// The centralities for the graph's vertices.  There is one key/value pair
    /// for each vertex in the graph.  The key is the IVertex.ID and the value
    /// is a <see cref="BrandesVertexCentralities" /> object containing the
    /// vertex's centralities.
    /// </value>
    //*************************************************************************

    public Dictionary<Int32, BrandesVertexCentralities>
    VertexCentralities
    {
        get
        {
            AssertValid();

            return (m_oVertexCentralities);
        }
    }

    //*************************************************************************
    //  Property: MaximumGeodesicDistance
    //
    /// <summary>
    /// Gets the maximum geodesic distance in the graph.
    /// </summary>
    ///
    /// <value>
    /// The maximum geodesic distance in the graph, or null if not available.
    /// </value>
    //*************************************************************************

    public Nullable<Int32>
    MaximumGeodesicDistance
    {
        get
        {
            AssertValid();

            return (m_iMaximumGeodesicDistance);
        }
    }

    //*************************************************************************
    //  Property: AverageGeodesicDistance
    //
    /// <summary>
    /// Gets the average geodesic distance in the graph.
    /// </summary>
    ///
    /// <value>
    /// The average geodesic distance in the graph, or null if not available.
    /// </value>
    //*************************************************************************

    public Nullable<Single>
    AverageGeodesicDistance
    {
        get
        {
            AssertValid();

            return (m_fAverageGeodesicDistance);
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
        Debug.Assert(m_oVertexCentralities != null);

        Debug.Assert(!m_iMaximumGeodesicDistance.HasValue ||
            m_iMaximumGeodesicDistance.Value >= 1);

        Debug.Assert(!m_fAverageGeodesicDistance.HasValue ||
            m_fAverageGeodesicDistance.Value >= 1);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// There is one key/value pair for each vertex in the graph.  The key is
    /// the IVertex.ID and the value is a BrandesVertexCentralities object
    /// containing the vertex's centralities.

    protected Dictionary<Int32, BrandesVertexCentralities>
        m_oVertexCentralities;

    /// The maximum geodesic distance in the graph, or null if not available.

    protected Nullable<Int32> m_iMaximumGeodesicDistance;

    /// The average geodesic distance in the graph, or null if not available.

    protected Nullable<Single> m_fAverageGeodesicDistance;
}

}
