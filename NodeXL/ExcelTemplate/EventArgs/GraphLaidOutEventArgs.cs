
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization.Wpf;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphLaidOutEventArgs
//
/// <summary>
/// Provides event information for the <see cref="TaskPane.GraphLaidOut" /> and
/// <see cref="ThisWorkbook.GraphLaidOut" /> events.
/// </summary>
//*****************************************************************************

public class GraphLaidOutEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: GraphLaidOutEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphLaidOutEventArgs" />
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
    ///
    /// <param name="nodeXLControl">
    /// The control in which the graph was laid out.
    /// </param>
    //*************************************************************************

    public GraphLaidOutEventArgs
    (
        Rectangle graphRectangle,
        Dictionary<Int32, IIdentityProvider> edgeIDDictionary,
        Dictionary<Int32, IIdentityProvider> vertexIDDictionary,
        NodeXLControl nodeXLControl
    )
    {
        m_oGraphRectangle = graphRectangle;
        m_oEdgeIDDictionary = edgeIDDictionary;
        m_oVertexIDDictionary = vertexIDDictionary;
        m_oNodeXLControl = nodeXLControl;

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
    //  Property: NodeXLControl
    //
    /// <summary>
    /// Gets the control in which the graph was laid out.
    /// </summary>
    ///
    /// <value>
    /// The control in which the graph was laid out.
    /// </value>
    ///
    /// <remarks>
    /// This can be used to copy the graph image to a bitmap using the
    /// NodeXLControl.CopyGraphToBitmap() method after the graph is laid out,
    /// for example.
    /// </remarks>
    //*************************************************************************

    public NodeXLControl
    NodeXLControl
    {
        get
        {
            AssertValid();

            return (m_oNodeXLControl);
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
        Debug.Assert(m_oNodeXLControl != null);
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

    /// The control in which the graph was laid out.

    protected NodeXLControl m_oNodeXLControl;
}


//*****************************************************************************
//  Delegate: GraphLaidOutEventHandler
//
/// <summary>
/// Represents a method that will handle the <see
/// cref="TaskPane.GraphLaidOut" /> and <see
/// cref="ThisWorkbook.GraphLaidOut" /> events.
/// </summary>
///
/// <param name="sender">
/// The source of the event.
/// </param>
///
/// <param name="e">
/// A <see cref="GraphLaidOutEventArgs" /> object that contains the event
/// data.
/// </param>
//*****************************************************************************

public delegate void
GraphLaidOutEventHandler
(
    Object sender,
    GraphLaidOutEventArgs e
);

}
