
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: GraphVisualCollection
//
/// <summary>
/// Contains a collection of Visual objects representing a graph's vertices and
/// edges.
/// </summary>
///
/// <remarks>
/// This is used to render a NodeXL graph in a WPF application.  It contains a
/// collection of WPF Visual objects that represent the graph's vertices and
/// edges.
///
/// <para>
/// Pass the NodeXL graph to the constructor, then call <see cref="Populate" />
/// to create the Visual objects from the graph.  To render the graph, host the
/// contained <see cref="GraphVisualCollection.VisualCollection" /> object in a
/// FrameworkElement.
/// </para>
///
/// <para>
/// If the graph is laid out again, call <see cref="UpdateLocations" /> to
/// update the locations of the contained Visual objects that represent the
/// vertices.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class GraphVisualCollection : Object
{
    //*************************************************************************
    //  Constructor: GraphVisualCollection()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphVisualCollection" />
	/// class.
    /// </summary>
	///
	/// <param name="graph">
	/// The graph whose vertices and edges will be represented by the contained
	/// collection.
	/// </param>
	///
	/// <param name="parentVisual">
	/// The parent of the contained collection.  This is usually a
	/// FrameworkElement that is hosting the collection.
	/// collection.
	/// </param>
    //*************************************************************************

    public GraphVisualCollection
	(
		IGraph graph,
		Visual parentVisual
	)
    {
		Debug.Assert(parentVisual != null);

		m_oGraph = graph;
		m_oVisualCollection = new VisualCollection(parentVisual);
		m_oVertexDrawingVisual = null;

		AssertValid();
    }

    //*************************************************************************
    //  Property: VisualCollection
    //
    /// <summary>
    /// Gets the contained VisualCollection that contains the Visual objects
	/// representing the graph's vertices and edges.
    /// </summary>
    ///
    /// <value>
    /// This should be treated as read-only and only to host the graph in a
	/// FrameworkElement.
    /// </value>
    //*************************************************************************

    public VisualCollection
    VisualCollection
    {
        get
        {
            AssertValid();

            return (m_oVisualCollection);
        }
    }

    //*************************************************************************
    //  Method: Populate()
    //
    /// <summary>
    /// Populates the collection with Visual objects that represent the graph.
    /// </summary>
	///
	/// <param name="graphRectangle">
	/// The rectangle that should contain the Visual objects representing the
	/// graph.
	/// </param>
    //*************************************************************************

    public void
    Populate
	(
		System.Drawing.RectangleF graphRectangle
	)
    {
        AssertValid();

		// TODO

		m_oVisualCollection.Clear();

		// Create some drawing tools.

		Brush oBrush = Brushes.Red;

		Pen oVertexPen = new Pen(oBrush, 1);

		if (oVertexPen.CanFreeze)
		{
			oVertexPen.Freeze();
		}

		Pen oEdgePen = new Pen(Brushes.Black, 1);

		if (oEdgePen.CanFreeze)
		{
			oEdgePen.Freeze();
		}

		// Draw the lines.  The lines do not need to be individually
		// hit-tested, so put them all in the same DrawingVisual.  (The lines
		// are drawn first to make the vertices cover the ends of the lines.)

		DrawingVisual oLineDrawingVisual = new DrawingVisual();
		DrawingContext oDrawingContext = oLineDrawingVisual.RenderOpen();

		foreach (IEdge oEdge in m_oGraph.Edges)
		{
			IVertex [] aoVertices = oEdge.Vertices;

			oDrawingContext.DrawLine(oEdgePen,
				new Point(aoVertices[0].Location.X, aoVertices[0].Location.Y),
				new Point(aoVertices[1].Location.X, aoVertices[1].Location.Y)
				);
		}

		oDrawingContext.Close();
		m_oVisualCollection.Add(oLineDrawingVisual);

		// Draw the vertices.  Each vertex needs to be individually hit-tested,
		// so put each one in its own DrawingVisual.

		m_oVertexDrawingVisual = new DrawingVisual();

		foreach (IVertex oVertex in m_oGraph.Vertices)
		{
			DrawingVisual oVertexChildDrawingVisual = new DrawingVisual();
			oDrawingContext = oVertexChildDrawingVisual.RenderOpen();

			oDrawingContext.DrawEllipse(oBrush, oVertexPen,
				new Point(oVertex.Location.X, oVertex.Location.Y),
				VertexRadius, VertexRadius);

			oDrawingContext.Close();
			m_oVertexDrawingVisual.Children.Add(oVertexChildDrawingVisual);
		}

		m_oVisualCollection.Add(m_oVertexDrawingVisual);
    }

    //*************************************************************************
    //  Method: UpdateLocations()
    //
    /// <summary>
    /// Updates the locations of the Visual object that represent the graph's
	/// vertices.
    /// </summary>
	///
	/// <param name="graphRectangle">
	/// The rectangle that should contain the Visual objects representing the
	/// graph.
	/// </param>
    //*************************************************************************

    public void
    UpdateLocations
	(
		System.Drawing.RectangleF graphRectangle
	)
    {
        AssertValid();

		// TODO
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
		Debug.Assert(m_oGraph != null);
		Debug.Assert(m_oVisualCollection != null);
		// m_oVertexDrawingVisual
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// TODO
	protected const Single VertexRadius = 3F;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The graph this collection represents.

	protected IGraph m_oGraph;

	/// Collection of Visual objects that represent the graph's vertices and
	/// edges.

	protected VisualCollection m_oVisualCollection;

	/// Visual that contains all the vertex visuals, or null if
	/// m_oVisualCollection hasn't been populated yet.

	protected DrawingVisual m_oVertexDrawingVisual;
}

}
