
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization.Wpf;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VerticesMovedEventArgs2
//
/// <summary>
/// Provides event information for the <see cref="TaskPane.VerticesMoved" />
/// and <see cref="ThisWorkbook.VerticesMoved" /> events.
/// </summary>
//*****************************************************************************

public class VerticesMovedEventArgs2 : VerticesMovedEventArgs
{
    //*************************************************************************
    //  Constructor: VerticesMovedEventArgs2()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VerticesMovedEventArgs2" /> class.
    /// </summary>
    ///
    /// <param name="verticesMovedEventArgs">
    /// Event arguments from the <see cref="NodeXLControl.VerticesMoved" />
    /// event.
    /// </param>
    ///
    /// <param name="movedVertexIDs">
    /// Array of the IDs of the vertices that were moved.  The IDs are from the
    /// vertex table.  There must be a one-to-one correspondence between these
    /// IDs and the vertices in <paramref name="verticesMovedEventArgs" />.
    /// </param>
    ///
    /// <param name="graphRectangle">
    /// The rectange the graph was drawn within.
    /// </param>
    //*************************************************************************

    public VerticesMovedEventArgs2
    (
        VerticesMovedEventArgs verticesMovedEventArgs,
        Int32 [] movedVertexIDs,
        Rectangle graphRectangle
    )
    : base (verticesMovedEventArgs.MovedVertices)
    {
        m_aiMovedVertexIDs = movedVertexIDs;
        m_oGraphRectangle = graphRectangle;

        AssertValid();
    }

    //*************************************************************************
    //  Property: MovedVertexIDs
    //
    /// <summary>
    /// Gets an array of the IDs of the vertices that were moved.
    /// </summary>
    ///
    /// <value>
    /// The IDs of the vertices that were moved.  The IDs are from the
    /// vertices' rows in the vertex table.  There is a one-to-one
    /// correspondence between these IDs and the vertices in <see
    /// cref="VerticesMovedEventArgs.MovedVertices" />.
    /// </value>
    //*************************************************************************

    public Int32 []
    MovedVertexIDs
    {
        get
        {
            AssertValid();

            return (m_aiMovedVertexIDs);
        }
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

        Debug.Assert(m_aiMovedVertexIDs != null);
        Debug.Assert(m_aiMovedVertexIDs.Length > 0);
        Debug.Assert(m_aiMovedVertexIDs.Length == m_aoMovedVertices.Length);
        // m_oGraphRectangle
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Vertex IDs from the vertex table.

    protected Int32 [] m_aiMovedVertexIDs;

    /// The rectangle the graph was drawn within.

    protected Rectangle m_oGraphRectangle;
}


//*****************************************************************************
//  Delegate: VerticesMovedEventHandler2
//
/// <summary>
/// Represents a method that will handle the <see
/// cref="TaskPane.VerticesMoved" /> and <see
/// cref="ThisWorkbook.VerticesMoved" /> events.
/// </summary>
///
/// <param name="sender">
/// The source of the event.
/// </param>
///
/// <param name="e">
/// A <see cref="VerticesMovedEventArgs2" /> object that contains the event
/// data.
/// </param>
//*****************************************************************************

public delegate void
VerticesMovedEventHandler2
(
    Object sender,
    VerticesMovedEventArgs2 e
);

}
