
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
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

public class VerticesMovedEventArgs2 : EventArgs
{
    //*************************************************************************
    //  Constructor: VerticesMovedEventArgs2()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VerticesMovedEventArgs2" /> class.
    /// </summary>
    ///
    /// <param name="verticesAndRowIDs">
    /// Collection of <see cref="VertexAndRowID" /> objects, one for each
    /// vertex that was moved.
    /// </param>
    ///
    /// <param name="graphRectangle">
    /// The rectangle the graph was drawn within.
    /// </param>
    //*************************************************************************

    public VerticesMovedEventArgs2
    (
        ICollection<VertexAndRowID> verticesAndRowIDs,
        Rectangle graphRectangle
    )
    {
        m_oVerticesAndRowIDs = verticesAndRowIDs;
        m_oGraphRectangle = graphRectangle;

        AssertValid();
    }

    //*************************************************************************
    //  Property: VerticesAndRowIDs
    //
    /// <summary>
    /// Gets the vertices that were moved.
    /// </summary>
    ///
    /// <value>
    /// Collection of <see cref="VertexAndRowID" /> objects, one for each
    /// vertex that was moved.
    /// </value>
    //*************************************************************************

    public ICollection<VertexAndRowID>
    VerticesAndRowIDs
    {
        get
        {
            AssertValid();

            return (m_oVerticesAndRowIDs);
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

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        Debug.Assert(m_oVerticesAndRowIDs != null);
        // m_oGraphRectangle
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Collection of VertexAndRowID objects, one for each vertex that was
    /// moved.

    protected ICollection<VertexAndRowID> m_oVerticesAndRowIDs;

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
