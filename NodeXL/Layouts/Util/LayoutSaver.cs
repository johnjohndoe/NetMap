
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Layouts
{
//*****************************************************************************
//  Class: LayoutSaver
//
/// <summary>
/// Saves and restores a layout.
/// </summary>
///
/// <remarks>
/// Pass a laid-out graph to the constructor, which saves the graph's vertex
/// locations.   Call <see cref="RestoreLayout" /> to restore the vertices to
/// their saved locations.
/// </remarks>
//*****************************************************************************

public class LayoutSaver : LayoutsBase
{
    //*************************************************************************
    //  Constructor: LayoutSaver()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="LayoutSaver" /> class.
    /// </summary>
    //*************************************************************************

    public LayoutSaver
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);

        m_oGraph = graph;
        IVertexCollection oVertices = graph.Vertices;
        m_oVertexLocations = new Dictionary<Int32, PointF>(oVertices.Count);

        foreach (IVertex oVertex in oVertices)
        {
            m_oVertexLocations[oVertex.ID] = oVertex.Location;
        }

        AssertValid();
    }

    //*************************************************************************
    //  Method: RestoreLayout()
    //
    /// <summary>
    /// Restores the graph's vertices to their saved locations.
    /// </summary>
    //*************************************************************************

    public void
    RestoreLayout()
    {
        AssertValid();

        foreach (IVertex oVertex in m_oGraph.Vertices)
        {
            PointF oSavedVertexLocation;

            if ( m_oVertexLocations.TryGetValue(oVertex.ID,
                out oSavedVertexLocation) )
            {
                oVertex.Location = oSavedVertexLocation;
            }
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

        Debug.Assert(m_oGraph != null);
        Debug.Assert(m_oVertexLocations != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Graph whose vertex locations have been saved in m_oVertexLocations.

    protected IGraph m_oGraph;

    /// Dictionary that contains one key/value pair for each of the graph's
    /// vertices.  The key is the IVertex.ID and the value is the vertex
    /// location, as a PointF.

    protected Dictionary<Int32, PointF> m_oVertexLocations;
}

}
