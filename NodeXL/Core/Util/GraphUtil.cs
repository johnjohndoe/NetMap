
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Class: GraphUtil
//
/// <summary>
/// Utility methods for dealing with <see cref="IGraph" /> objects.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class GraphUtil
{
    //*************************************************************************
    //  Method: GetNonIsolatedVertices()
    //
    /// <summary>
    /// Gets a List of a graph's non-isolated vertices.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to get the non-isolated vertices from.
    /// </param>
    ///
    /// <returns>
    /// A List of non-isolated vertices.
    /// </returns>
    //*************************************************************************

    public static List<IVertex>
    GetNonIsolatedVertices
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);

        List<IVertex> oNonIsolatedVertices = new List<IVertex>();

        foreach (IVertex oVertex in graph.Vertices)
        {
            if (oVertex.IncidentEdges.Count > 0)
            {
                oNonIsolatedVertices.Add(oVertex);
            }
        }

        return (oNonIsolatedVertices);
    }
}

}
