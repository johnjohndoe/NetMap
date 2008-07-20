
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//	Class: GraphMouseEventArgs
//
/// <summary>
///	Provides information for the <see cref="NetMapControl.GraphMouseDown" /> and
///	<see cref="NetMapControl.GraphMouseUp" /> events fired by the <see
///	cref="NetMapControl" />.
/// </summary>
//*****************************************************************************

public class GraphMouseEventArgs : MouseEventArgs
{
	//*************************************************************************
	//	Constructor: GraphMouseEventArgs()
	//
	/// <summary>
	///	Initializes a new instance of the GraphMouseEventArgs class.
	/// </summary>
	///
	/// <param name="oMouseEventArgs">
	/// Mouse event arguments.
	/// </param>
	///
	/// <param name="vertex">
	/// Vertex under the mouse, or null if the user clicked on a part of the
	///	graph not covered by a vertex.
	/// </param>
	//*************************************************************************

	public GraphMouseEventArgs
	(
		MouseEventArgs oMouseEventArgs,
		IVertex vertex

	) : base(oMouseEventArgs.Button, oMouseEventArgs.Clicks, oMouseEventArgs.X,
			oMouseEventArgs.Y, oMouseEventArgs.Delta)
	{
		m_oVertex = vertex;

		AssertValid();
	}

	//*************************************************************************
	//	Property: Vertex
	//
	/// <summary>
	///	Gets the vertex under the mouse.
	/// </summary>
	///
	/// <value>
	///	The vertex under the mouse, as an <see cref="IVertex" />, or null if
	///	the user clicked a point on the graph not covered by a vertex.
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
	//	Protected member data
	//*************************************************************************

	/// Vertex, or null.

	protected IVertex m_oVertex;
}

}
