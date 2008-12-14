
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//  Class: DraggedVertex
//
/// <summary>
/// Represents a vertex that might be dragged with the mouse.
/// </summary>
///
/// <remarks>
/// Create an instance of this class when a vertex is clicked.  When the mouse
/// is moved, check <see cref="MouseDrag.ShouldBeginDrag" /> to determine
/// whether the mouse has moved far enough to begin a vertex drag.
/// </remarks>
//*****************************************************************************

public class DraggedVertex : MouseDrag
{
    //*************************************************************************
    //  Constructor: DraggedVertex()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="DraggedVertex" /> class.
    /// </summary>
	///
	/// <param name="vertex">
	/// The vertex that was clicked.  Can't be null.
	/// </param>
	///
	/// <param name="mouseDownLocation">
	/// Location where the vertex was clicked, in client coordinates.
	/// </param>
    //*************************************************************************

    public DraggedVertex
	(
		IVertex vertex,
		Point mouseDownLocation
	)
	: base(mouseDownLocation)
    {
		Debug.Assert(vertex != null);

		m_oVertex = vertex;

		AssertValid();
    }

    //*************************************************************************
    //  Property: Vertex
    //
    /// <summary>
    /// Gets the vertex that was clicked.
    /// </summary>
    ///
    /// <value>
	/// The vertex that was clicked, as an <see cref="IVertex" />.
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

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
		base.AssertValid();

		Debug.Assert(m_oVertex != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// The vertex that was clicked.  Can't be null.

	protected IVertex m_oVertex;
}

}
