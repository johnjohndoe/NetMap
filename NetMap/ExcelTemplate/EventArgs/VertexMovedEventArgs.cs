
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexMovedEventArgs
//
/// <summary>
/// Provides event information for the <see cref="TaskPane.VertexMoved" /> and
/// <see cref="ThisWorkbook.VertexMoved" /> events.
/// </summary>
//*****************************************************************************

public class VertexMovedEventArgs : VertexEventArgs
{
    //*************************************************************************
    //  Constructor: VertexMovedEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexMovedEventArgs" />
	/// class.
    /// </summary>
	///
	/// <param name="vertex">
    /// The vertex that was moved.
	/// </param>
	///
	/// <param name="ID">
    /// Vertex ID from the vertex table.
	/// </param>
	///
	/// <param name="graphRectangle">
    /// The rectange the graph was drawn within.
	/// </param>
    //*************************************************************************

    public VertexMovedEventArgs
	(
		IVertex vertex,
		Int32 ID,
		Rectangle graphRectangle
	)
	: base (vertex)
    {
		m_iID = ID;
        m_oGraphRectangle = graphRectangle;

		AssertValid();
    }

    //*************************************************************************
    //  Property: ID
    //
    /// <summary>
    /// Gets the ID of the vertex that was moved.
    /// </summary>
    ///
    /// <value>
    /// The ID of the vertex that was moved.  The ID is from the vertex's row
	/// in the vertex table.
    /// </value>
    //*************************************************************************

    public Int32
    ID
    {
        get
        {
            AssertValid();

            return (m_iID);
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

		Debug.Assert(m_iID > 0);
		// m_oGraphRectangle
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Vertex ID from the vertex table.

	protected Int32 m_iID;

    /// The rectangle the graph was drawn within.

	protected Rectangle m_oGraphRectangle;
}


//*****************************************************************************
//  Delegate: VertexMovedEventHandler
//
/// <summary>
/// Represents a method that will handle the <see
/// cref="TaskPane.VertexMoved" /> and <see cref="ThisWorkbook.VertexMoved" />
/// events.
/// </summary>
///
/// <param name="sender">
/// The source of the event.
/// </param>
///
/// <param name="e">
///	A <see cref="VertexMovedEventArgs" /> object that contains the event data.
/// </param>
//*****************************************************************************

public delegate void
VertexMovedEventHandler
(
	Object sender,
	VertexMovedEventArgs e
);

}
