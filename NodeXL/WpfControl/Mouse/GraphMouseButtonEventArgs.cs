
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Input;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: GraphMouseButtonEventArgs
//
/// <summary>
/// Provides information for events fired when a graph is clicked.
/// </summary>
//*****************************************************************************

public class GraphMouseButtonEventArgs : MouseButtonEventArgs
{
    //*************************************************************************
    //  Constructor: GraphMouseButtonEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the GraphMouseButtonEventArgs class.
    /// </summary>
    ///
    /// <param name="mouseButtonEventArgs">
    /// Mouse event arguments.
    /// </param>
    ///
    /// <param name="vertex">
    /// Vertex under the mouse, or null if the user clicked on a part of the
    /// graph not covered by a vertex.
    /// </param>
    //*************************************************************************

    public GraphMouseButtonEventArgs
    (
        MouseButtonEventArgs mouseButtonEventArgs,
        IVertex vertex

    )
    : base(mouseButtonEventArgs.MouseDevice, mouseButtonEventArgs.Timestamp,
        mouseButtonEventArgs.ChangedButton, mouseButtonEventArgs.StylusDevice)
    {
        m_oVertex = vertex;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Vertex
    //
    /// <summary>
    /// Gets the vertex under the mouse.
    /// </summary>
    ///
    /// <value>
    /// The vertex under the mouse, as an <see cref="IVertex" />, or null if
    /// the user clicked a point on the graph not covered by a vertex.
    /// </value>
    //*************************************************************************

    public IVertex
    Vertex
    {
        get
        {
            AssertValid();

            return (m_oVertex);
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
        // m_oVertex
    }


    //*************************************************************************
    //  Protected member data
    //*************************************************************************

    /// The vertex under the mouse, or null.

    protected IVertex m_oVertex;
}


//*****************************************************************************
//  Delegate: GraphMouseButtonEventHandler
//
/// <summary>
/// Represents a method that will handle an event fired when a graph is
/// clicked.
/// </summary>
///
/// <param name="sender">
/// The object that fired the event.
/// </param>
///
/// <param name="e">
/// Provides information about the mouse and the part of the graph that was
/// clicked.
/// </param>
//*****************************************************************************

public delegate void
GraphMouseButtonEventHandler
(
    Object sender,
    GraphMouseButtonEventArgs e
);

}
