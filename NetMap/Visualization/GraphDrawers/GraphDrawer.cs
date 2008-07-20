
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//	Class: GraphDrawer
//
/// <summary>
///	Draws a graph onto a <see cref="Bitmap" /> or <see cref="Graphics" />
/// object.
/// </summary>
///
/// <remarks>
///	<see cref="GraphDrawer" /> is one of several classes provided with the
/// NetMap system that draw a graph, which is a set of vertices connected by
/// edges.
///
/// <para>
///	The following table summarizes the graph-drawing classes:
/// </para>
///
///	<list type="table">
///
///	<listheader>
/// <term>Class</term>
/// <term>For Use In</term>
/// <term>Features</term>
/// <term>Required NetMap Assemblies</term>
///	</listheader>
///
///	<item>
///	<term><see cref="GraphDrawer" /></term>
///	<term>
///	Any application that wants to draw a graph onto a <see cref="Bitmap" /> or
/// a <see cref="Graphics" /> object in a synchronous manner.
///	</term>
///	<term>
/// Can use custom layouts, vertex drawers, and edge drawers.
///	</term>
///	<term>
///	Core.dll, Visualization.dll
///	</term>
///	</item>
///
///	<item>
///	<term><see cref="AsyncGraphDrawer" /></term>
///	<term>
///	Any application that wants to draw a graph onto a <see cref="Bitmap" /> or
/// a <see cref="Graphics" /> object in a synchronous or asynchronous manner.
///	</term>
///	<term>
/// Can use custom layouts, vertex drawers, and edge drawers.
///	</term>
///	<term>
///	Core.dll, Visualization.dll
///	</term>
///	</item>
///
///	<item>
///	<term><see cref="MultiSelectionGraphDrawer" /></term>
///	<term>
///	Any application that wants to draw a graph onto a <see cref="Bitmap" /> or
/// a <see cref="Graphics" /> object in a synchronous or asynchronous manner.
///	</term>
///	<term>
/// Same as <see cref="AsyncGraphDrawer" />, plus vertices and edges can be
/// drawn as selected.
///	</term>
///	<term>
///	Core.dll, Visualization.dll
///	</term>
///	</item>
///
///	<item>
///	<term>NetMapControl</term>
///	<term>
///	Windows Forms applications
///	</term>
///	<term>
/// Wraps a <see cref="MultiSelectionGraphDrawer" /> in a Windows Forms
/// control.
///	</term>
///	<term>
///	Core.dll, Visualization.dll, Control.dll
///	</term>
///	</item>
///
///	</list>
///
/// <para>
/// <see cref="GraphDrawer" /> draws a graph onto a <see cref="Bitmap" /> or
/// <see cref="Graphics" /> object using <see
/// cref="FruchtermanReingoldLayout" />, <see cref="VertexDrawer" />, and <see
/// cref="EdgeDrawer" /> objects by default.  The graph can be
/// customized by setting the <see cref="GraphDrawerBase.Layout" />, <see
/// cref="VertexDrawer" />, and <see cref="EdgeDrawer" /> properties to
/// alterative objects, or by deriving a class from this class and overriding
/// any of its methods.
/// </para>
///
/// </remarks>
///
///	<example>
///	Here is sample C# code that populates a graph with several vertices and
/// edges, adds metadata to the vertices and edges, sets the graph colors, and
/// draws the graph onto a bitmap.
///
/// <code>
///
/// // Create an object for drawing graphs.
///
/// GraphDrawer oGraphDrawer = new GraphDrawer();
///
/// // The object owns an empty graph.  Get the graph's vertex and edge
/// // collections.
///
/// IGraph oGraph = oGraphDrawer.Graph;
/// IVertexCollection oVertices = oGraph.Vertices;
/// IEdgeCollection oEdges = oGraph.Edges;
///
/// // Create some vertices and add them to the graph.
///
/// IVertex oVertex1 = oVertices.Add();
/// IVertex oVertex2 = oVertices.Add();
/// IVertex oVertex3 = oVertices.Add();
///
/// // Add some metadata to the vertices.  You can use the Key property to
/// // add a single piece of metadata, or the SetValue() method to add
/// // arbitrary key/value pairs.
///
/// oVertex1.Tag = "This is vertex 1.";
/// oVertex2.Tag = "This is vertex 2.";
/// oVertex3.Tag = "This is vertex 3.";
///
/// // Connect some of the vertices with undirected edges.
///
/// IEdge oEdge1 = oEdges.Add(oVertex1, oVertex2);
/// IEdge oEdge2 = oEdges.Add(oVertex2, oVertex3);
///
/// // Add some metadata to the edges.
///
/// oEdge1.SetValue("Weight", 5);
/// oEdge2.SetValue("Weight", 3);
///
/// // Set the graph's colors.
///
/// oGraphDrawer.BackColor = Color.DarkBlue;
///
/// // The GraphDrawer's default vertex drawer is a VertexDrawer object.
///
/// VertexDrawer oVertexDrawer = (VertexDrawer)oGraphDrawer.VertexDrawer;
///
/// oVertexDrawer.Color = Color.White;
///
/// // The GraphDrawer's default edge drawer is an EdgeDrawer object.
///
/// EdgeDrawer oEdgeDrawer = (EdgeDrawer)oGraphDrawer.EdgeDrawer;
///
/// oEdgeDrawer.Color = Color.White;
///
/// // Create a bitmap and draw the graph onto it.
///
/// Bitmap oBitmap = new Bitmap(400, 200);
///
/// oGraphDrawer.Draw(oBitmap);
///
/// // Do something with the bitmap...
///
/// </code>
///
/// </example>
//*****************************************************************************

public class GraphDrawer : GraphDrawerBase, IGraphDrawer
{
    //*************************************************************************
    //  Constructor: GraphDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the GraphDrawer class.
    /// </summary>
    //*************************************************************************

    public GraphDrawer()
    {
		// (Do nothing.)

		AssertValid();
    }

	//*************************************************************************
	//	Method: DrawCurrentLayout()
	//
	/// <summary>
	/// Draws the graph without laying it out.
	/// </summary>
	///
	/// <param name="graphics">
	/// <see cref="Graphics" /> object to draw onto.
	/// </param>
	///
	/// <param name="graphRectangle">
	/// <see cref="Rectangle" /> to draw within.
	/// </param>
	///
	/// <remarks>
	/// It's assumed that the graph has been laid out before this method is
	/// called.
	/// </remarks>
	//*************************************************************************

	public void
	DrawCurrentLayout
	(
		Graphics graphics,
		Rectangle graphRectangle
	)
	{
		AssertValid();

		const String MethodName = "DrawCurrentLayout";

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "graphics", graphics);

		DrawAfterOptionalLayout(graphics, graphRectangle, false);
	}

	//*************************************************************************
	//	Method: DrawCore()
	//
	/// <summary>
	/// Lays out and draws the graph within a specified rectangle of a <see
	/// cref="Graphics" /> object.
	/// </summary>
	///
	/// <param name="graphics">
	/// <see cref="Graphics" /> object to draw onto.
	/// </param>
	///
	/// <param name="graphRectangle">
	/// <see cref="Rectangle" /> to draw within.
	/// </param>
	///
	/// <remarks>
	/// The arguments have already been checked for validity.
	/// </remarks>
	//*************************************************************************

	protected override void
	DrawCore
	(
		Graphics graphics,
		Rectangle graphRectangle
	)
	{
		Debug.Assert(graphics != null);
		AssertValid();

		DrawAfterOptionalLayout(graphics, graphRectangle, true);
	}

	//*************************************************************************
	//	Method: DrawAfterOptionalLayout()
	//
	/// <summary>
	/// Optionally lays out and then draws the graph within a specified
	/// rectangle of a <see cref="Graphics" /> object.
	/// </summary>
	///
	/// <param name="graphics">
	/// <see cref="Graphics" /> object to draw onto.
	/// </param>
	///
	/// <param name="graphRectangle">
	/// <see cref="Rectangle" /> to draw within.
	/// </param>
	///
	/// <param name="layOutGraph">
	/// true to lay out the graph before drawing it.
	/// </param>
	//*************************************************************************

	protected void
	DrawAfterOptionalLayout
	(
		Graphics graphics,
		Rectangle graphRectangle,
		Boolean layOutGraph
	)
	{
		Debug.Assert(graphics != null);
		AssertValid();

		GraphicsState oGraphicsState = graphics.Save();

		try
		{
			graphics.Clip = new Region(graphRectangle);

			if (layOutGraph)
			{
				// Lay out the graph.

				LayoutContext oLayoutContext =
					new LayoutContext(graphRectangle, this);

				m_oLayout.LayOutGraph(m_oGraph, oLayoutContext);
			}

			// Draw it.

            DrawContext oDrawContext = new DrawContext(
				this, graphics, graphRectangle, this.Layout.Margin);

			DrawNoLayout(oDrawContext);
		}
		finally
		{
			graphics.Restore(oGraphicsState);
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

		// (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
