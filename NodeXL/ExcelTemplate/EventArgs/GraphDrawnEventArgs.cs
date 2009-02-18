
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphDrawnEventArgs
//
/// <summary>
/// Provides event information for the <see cref="TaskPane.GraphDrawn" /> and
/// <see cref="ThisWorkbook.GraphDrawn" /> events.
/// </summary>
//*****************************************************************************

public class GraphDrawnEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: GraphDrawnEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphDrawnEventArgs" />
    /// class.
    /// </summary>
    ///
    /// <param name="graphRectangle">
    /// The rectange the graph was drawn within.
    /// </param>
    ///
    /// <param name="edgeIDDictionary">
    /// Dictionary that maps edge IDs stored in the edge worksheet to edge
    /// objects in the graph.
    /// </param>
    ///
    /// <param name="vertexIDDictionary">
    /// Dictionary that maps vertex IDs stored in the vertex worksheet to
    /// vertex objects in the graph.
    /// </param>
    //*************************************************************************

    public GraphDrawnEventArgs
    (
        Rectangle graphRectangle,
        Dictionary<Int32, IIdentityProvider> edgeIDDictionary,
        Dictionary<Int32, IIdentityProvider> vertexIDDictionary
    )
    {
        m_oGraphRectangle = graphRectangle;
        m_oEdgeIDDictionary = edgeIDDictionary;
        m_oVertexIDDictionary = vertexIDDictionary;

        AssertValid();
    }

    //*************************************************************************
    //  Property: GraphRectangle
    //
    /// <summary>
    /// Gets the rectangle the graph was drawn within.
    /// </summary>
    ///
    /// <value>
    /// The rectangle the graph was drawn within.
    /// </value>
    //*************************************************************************

    public Rectangle
    GraphRectangle
    {
        get
        {
            AssertValid();

            return (m_oGraphRectangle);
        }
    }

    //*************************************************************************
    //  Property: EdgeIDDictionary
    //
    /// <summary>
    /// Gets a dictionary that maps edge IDs stored in the edge worksheet to
    /// edge objects in the graph.
    /// </summary>
    ///
    /// <value>
    /// A dictionary that maps edge IDs stored in the edge worksheet to edge
    /// objects in the graph.
    /// </value>
    ///
    /// <remarks>
    /// The IDs are those that are stored in the edge worksheet's ID column and
    /// are different from the IEdge.ID values in the graph, which the edge
    /// worksheet knows nothing about.
    /// </remarks>
    //*************************************************************************

    public Dictionary<Int32, IIdentityProvider>
    EdgeIDDictionary
    {
        get
        {
            AssertValid();

            return (m_oEdgeIDDictionary);
        }
    }

    //*************************************************************************
    //  Property: VertexIDDictionary
    //
    /// <summary>
    /// Gets a dictionary that maps vertex IDs stored in the vertex worksheet
    /// to vertex objects in the graph.
    /// </summary>
    ///
    /// <value>
    /// A dictionary that maps vertex IDs stored in the vertex worksheet to
    /// vertex objects in the graph.
    /// </value>
    ///
    /// <remarks>
    /// The IDs are those that are stored in the vertex worksheet's ID column
    /// and are different from the IVertex.ID values in the graph, which the
    /// vertex worksheet knows nothing about.
    /// </remarks>
    //*************************************************************************

    public Dictionary<Int32, IIdentityProvider>
    VertexIDDictionary
    {
        get
        {
            AssertValid();

            return (m_oVertexIDDictionary);
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
        // m_oGraphRectangle
        Debug.Assert(m_oEdgeIDDictionary != null);
        Debug.Assert(m_oVertexIDDictionary != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The rectangle the graph was drawn within.

    protected Rectangle m_oGraphRectangle;

    /// Dictionary that maps edge IDs stored in the edge worksheet to edge
    /// objects in the graph.  The edge IDs stored in the worksheet are
    /// different from IEdge.ID, which the edge worksheet knows nothing about.

    protected Dictionary<Int32, IIdentityProvider> m_oEdgeIDDictionary;

    /// Ditto for vertices.

    protected Dictionary<Int32, IIdentityProvider> m_oVertexIDDictionary;
}


//*****************************************************************************
//  Delegate: GraphDrawnEventHandler
//
/// <summary>
/// Represents a method that will handle the <see cref="TaskPane.GraphDrawn" />
/// and <see cref="ThisWorkbook.GraphDrawn" /> events.
/// </summary>
///
/// <param name="sender">
/// The source of the event.
/// </param>
///
/// <param name="e">
/// An <see cref="GraphDrawnEventArgs" /> object that contains the event data.
/// </param>
//*****************************************************************************

public delegate void
GraphDrawnEventHandler
(
    Object sender,
    GraphDrawnEventArgs e
);

}
