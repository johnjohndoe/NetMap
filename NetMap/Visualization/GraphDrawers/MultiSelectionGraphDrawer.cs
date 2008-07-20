
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//	Class: MultiSelectionGraphDrawer
//
/// <summary>
///	Draws a graph onto a <see cref="Bitmap" /> or <see cref="Graphics" />
/// object.  Allows vertices and edges to be drawn as selected.
/// </summary>
///
/// <remarks>
///	<see cref="MultiSelectionGraphDrawer" /> is one of several classes provided
/// with the NetMap system that draw a graph, which is a set of vertices
/// connected by edges.
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
/// <see cref="MultiSelectionGraphDrawer" /> works in conjunction with its
/// default <see cref="Visualization.VertexDrawer" /> and <see
/// cref="Visualization.EdgeDrawer" /> objects to allow vertices and edges to
/// be selected, and to draw selected vertices and edges in a special manner.
/// <see cref="Visualization.VertexDrawer" /> uses a different color and
/// radius, and <see cref="Visualization.EdgeDrawer" /> uses a different color
/// and width.
/// </para>
///
/// <para>
/// Use <see cref="SelectVertex(IVertex)" /> and <see
/// cref="SelectEdge(IEdge)" /> to mark vertices and edges as selected.  Use
/// <see cref="DeselectVertex" /> and <see cref="DeselectEdge" /> to unmark
/// them.  Call one of the <see cref="Draw(Bitmap, Boolean)" /> or <see
/// cref="DrawAsync(Bitmap, Boolean)" /> methods to draw the entire graph.  If
/// you change the selected state of a vertex or edge, you can either draw the
/// entire graph again or call <see
/// cref="RedrawVertex(IVertex, Bitmap, Boolean)" /> or <see
/// cref="RedrawEdge(IEdge, Bitmap, Boolean)" /> to redraw only the vertices
/// and edges that you changed.
/// </para>
///
/// </remarks>
///
///	<example>
///	Here is sample C# code that populates a graph with several vertices and
/// edges, adds metadata to the vertices and edges, selects a vertex and edge,
/// sets the graph colors, and draws the graph onto a bitmap.
///
/// <code>
///
/// // Create an object for drawing graphs.
///
/// MultiSelectionGraphDrawer oMultiSelectionGraphDrawer =
///     new MultiSelectionGraphDrawer();
///
/// // The object owns an empty graph.  Get the graph's vertex and edge
/// // collections.
///
/// IGraph oGraph = oMultiSelectionGraphDrawer.Graph;
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
/// // Select a vertex and an edge.
///
/// MultiSelectionGraphDrawer.SelectVertex(oVertex1);
/// MultiSelectionGraphDrawer.SelectEdge(oEdge2);
///
/// // Set the graph's colors.
///
/// oMultiSelectionGraphDrawer.BackColor = Color.White;
///
/// // The MultiSelectionGraphDrawer's default vertex drawer is a
/// // VertexDrawer object.
///
/// VertexDrawer oVertexDrawer =
///     (VertexDrawer)oMultiSelectionGraphDrawer.VertexDrawer;
///
/// oVertexDrawer.Color = Color.Black;
/// oVertexDrawer.SelectedColor = Color.Red;
///
/// // The MultiSelectionGraphDrawer's default edge drawer is an EdgeDrawer
/// // object.
///
/// EdgeDrawer oEdgeDrawer = (EdgeDrawer)oMultiSelectionGraphDrawer.EdgeDrawer;
///
/// oEdgeDrawer.Color = Color.Black;
/// oEdgeDrawer.SelectedColor = Color.Red;
///
/// // Create a bitmap and draw the graph onto it.
///
/// Bitmap oBitmap = new Bitmap(400, 200);
///
/// oMultiSelectionGraphDrawer.Draw(oBitmap, true);
///
/// // Do something with the bitmap...
///
/// </code>
///
/// </example>
//*****************************************************************************

public class MultiSelectionGraphDrawer : AsyncGraphDrawer
{
    //*************************************************************************
    //  Constructor: MultiSelectionGraphDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="MultiSelectionGraphDrawer" /> class.
    /// </summary>
    //*************************************************************************

    public MultiSelectionGraphDrawer()
    {
		m_bDrawSelection = false;

		AssertValid();
    }

	//*************************************************************************
	//	Method: Draw()
	//
	/// <overloads>
	/// Draws the graph.
	/// </overloads>
	///
	/// <summary>
	/// Draws the graph onto the entire rectangle of a <see cref="Bitmap" />,
	/// as unselected.
	/// </summary>
	///
	/// <param name="bitmap">
	/// <see cref="Bitmap" /> to draw onto.
	/// </param>
	///
	/// <remarks>
	/// All vertices and edges are drawn as unselected.
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is in progress.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public new void
	Draw
	(
		Bitmap bitmap
	)
	{
		AssertValid();

		// Set a flag that will be checked within the virtual methods
		// DrawVertices() and DrawEdges().

		m_bDrawSelection = false;

		base.Draw(bitmap);
	}

	//*************************************************************************
	//	Method: Draw()
	//
	/// <summary>
	/// Draws the graph within a specified rectangle of a <see
	/// cref="Bitmap" />, as unselected.
	/// </summary>
	///
	/// <param name="bitmap">
	/// <see cref="Bitmap" /> to draw onto.
	/// </param>
	///
	/// <param name="graphRectangle">
	///	<see cref="Rectangle" /> to draw within.
	/// </param>
	///
	/// <remarks>
	/// All vertices and edges are drawn as unselected.
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is in progress.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public new void
	Draw
	(
		Bitmap bitmap,
		Rectangle graphRectangle
	)
	{
		AssertValid();

		// Set a flag that will be checked within the virtual methods
		// DrawVertices() and DrawEdges().

		m_bDrawSelection = false;

		base.Draw(bitmap, graphRectangle);
	}

	//*************************************************************************
	//	Method: Draw()
	//
	/// <summary>
	/// Draws the graph within a specified rectangle of a <see
	/// cref="Graphics" /> object, as unselected.
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
	/// All vertices and edges are drawn as unselected.
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is in progress.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public new void
	Draw
	(
		Graphics graphics,
		Rectangle graphRectangle
	)
	{
		AssertValid();

		// Set a flag that will be checked within the virtual methods
		// DrawVertices() and DrawEdges().

		m_bDrawSelection = false;

		base.Draw(graphics, graphRectangle);
	}

	//*************************************************************************
	//	Method: Draw()
	//
	/// <summary>
	/// Draws the graph onto the entire rectangle of a <see cref="Bitmap" />.
	/// </summary>
	///
	/// <param name="bitmap">
	/// <see cref="Bitmap" /> to draw onto.
	/// </param>
	///
	/// <param name="drawSelection">
	///	If true and there are selected vertices or edges, those vertices and
	/// edges are drawn as selected.  Otherwise, all vertices and edges are
	/// drawn as unselected.
	/// </param>
	///
	/// <remarks>
	/// An exception is thrown if an asynchronous drawing is in progress.
	/// </remarks>
	//*************************************************************************

	public void
	Draw
	(
		Bitmap bitmap,
		Boolean drawSelection
	)
	{
		AssertValid();

		// Set a flag that will be checked within the virtual methods
		// DrawVertices() and DrawEdges().

		m_bDrawSelection = drawSelection;

		base.Draw(bitmap);
	}

	//*************************************************************************
	//	Method: Draw()
	//
	/// <summary>
	/// Draws the graph within a specified rectangle of a <see
	/// cref="Bitmap" />.
	/// </summary>
	///
	/// <param name="bitmap">
	/// <see cref="Bitmap" /> to draw onto.
	/// </param>
	///
	/// <param name="graphRectangle">
	///	Rectangle to draw within.
	/// </param>
	///
	/// <param name="drawSelection">
	///	If true and there are selected vertices or edges, those vertices and
	/// edges are drawn as selected.  Otherwise, all vertices and edges are
	/// drawn as unselected.
	/// </param>
	///
	/// <remarks>
	/// An exception is thrown if an asynchronous drawing is in progress.
	/// </remarks>
	//*************************************************************************

	public void
	Draw
	(
		Bitmap bitmap,
		Rectangle graphRectangle,
		Boolean drawSelection
	)
	{
		AssertValid();

		// Set a flag that will be checked within the virtual methods
		// DrawVertices() and DrawEdges().

		m_bDrawSelection = drawSelection;

		base.Draw(bitmap, graphRectangle);
	}

	//*************************************************************************
	//	Method: Draw()
	//
	/// <summary>
	/// Draws the graph onto a <see cref="Graphics" /> object.
	/// </summary>
	///
	/// <param name="graphics">
	/// <see cref="Graphics" /> object to draw onto.
	/// </param>
	///
	/// <param name="graphRectangle">
	/// Rectangle to draw within.
	/// </param>
	///
	/// <param name="drawSelection">
	///	If true and there are selected vertices or edges, those vertices and
	/// edges are drawn as selected.  Otherwise, all vertices and edges are
	/// drawn as unselected.
	/// </param>
	///
	/// <remarks>
	/// An exception is thrown if an asynchronous drawing is in progress.
	/// </remarks>
	//*************************************************************************

	public void
	Draw
	(
		Graphics graphics,
		Rectangle graphRectangle,
		Boolean drawSelection
	)
	{
		AssertValid();

		// Set a flag that will be checked within the virtual methods
		// DrawVertices() and DrawEdges().

		m_bDrawSelection = drawSelection;

		base.Draw(graphics, graphRectangle);
	}

	//*************************************************************************
	//	Method: DrawAsync()
	//
	/// <overloads>
	/// Asynchronously draws the graph.
	/// </overloads>
	///
	/// <summary>
	/// Draws the graph within the entire rectangle of a <see cref="Bitmap" />,
	/// as unselected.
	/// </summary>
	///
	/// <param name="bitmap">
	/// <see cref="Bitmap" /> to draw onto.
	/// </param>
	///
	/// <remarks>
	/// This method starts drawing the graph on a worker thread and returns
	/// immediately.  The <see
	/// cref="AsyncGraphDrawer.DrawAsyncIterationCompleted" /> event may fire
	/// repeatedly while the drawing is occurring.  The <see
	/// cref="AsyncGraphDrawer.DrawAsyncCompleted" /> event fires when the
	/// drawing is complete, an error occurs, or the drawing is cancelled.
	/// <see cref="AsyncGraphDrawer.DrawAsyncCancel" /> cancels the drawing.
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is already in
	/// progress.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public new void
	DrawAsync
	(
		Bitmap bitmap
	)
	{
		AssertValid();

		// Set a flag that will be checked within the virtual methods
		// DrawVertices() and DrawEdges().

		m_bDrawSelection = false;

		base.DrawAsync(bitmap);
	}

	//*************************************************************************
	//	Method: DrawAsync()
	//
	/// <summary>
	/// Asynchronously draws the graph within a specified rectangle of a <see
	/// cref="Bitmap" />, as unselected.
	/// </summary>
	///
	/// <param name="bitmap">
	/// <see cref="Bitmap" /> to draw onto.
	/// </param>
	///
	/// <param name="graphRectangle">
	///	<see cref="Rectangle" /> to draw within.
	/// </param>
	///
	/// <remarks>
	/// This method starts drawing the graph on a worker thread and returns
	/// immediately.  The <see
	/// cref="AsyncGraphDrawer.DrawAsyncIterationCompleted" /> event may fire
	/// repeatedly while the drawing is occurring.  The <see
	/// cref="AsyncGraphDrawer.DrawAsyncCompleted" /> event fires when the
	/// drawing is complete, an error occurs, or the drawing is cancelled.
	/// <see cref="AsyncGraphDrawer.DrawAsyncCancel" /> cancels the drawing.
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is already in
	/// progress.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public new void
	DrawAsync
	(
		Bitmap bitmap,
		Rectangle graphRectangle
	)
	{
		AssertValid();

		// Set a flag that will be checked within the virtual methods
		// DrawVertices() and DrawEdges().

		m_bDrawSelection = false;

		base.DrawAsync(bitmap, graphRectangle);
	}

	//*************************************************************************
	//	Method: DrawAsync()
	//
	/// <summary>
	/// Asynchronously draws the graph within a specified rectangle of a <see
	/// cref="Graphics" /> object, as unselected.
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
	/// This method starts drawing the graph on a worker thread and returns
	/// immediately.  The <see
	/// cref="AsyncGraphDrawer.DrawAsyncIterationCompleted" /> event may fire
	/// repeatedly while the drawing is occurring.  The <see
	/// cref="AsyncGraphDrawer.DrawAsyncCompleted" /> event fires when the
	/// drawing is complete, an error occurs, or the drawing is cancelled.
	/// <see cref="AsyncGraphDrawer.DrawAsyncCancel" /> cancels the drawing.
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is already in
	/// progress.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public new void
	DrawAsync
	(
		Graphics graphics,
		Rectangle graphRectangle
	)
	{
		AssertValid();

		// Set a flag that will be checked within the virtual methods
		// DrawVertices() and DrawEdges().

		m_bDrawSelection = false;

		base.DrawAsync(graphics, graphRectangle);
	}

	//*************************************************************************
	//	Method: DrawAsync()
	//
	/// <summary>
	/// Asynchronously draws the graph onto the entire rectangle of a <see
	/// cref="Bitmap" />.
	/// </summary>
	///
	/// <param name="bitmap">
	/// <see cref="Bitmap" /> to draw onto.
	/// </param>
	///
	/// <param name="drawSelection">
	///	If true and there are selected vertices or edges, those vertices and
	/// edges are drawn as selected.  Otherwise, all vertices and edges are
	/// drawn as unselected.
	/// </param>
	///
	/// <remarks>
	/// This method starts drawing the graph on a worker thread and returns
	/// immediately.  The <see
	/// cref="AsyncGraphDrawer.DrawAsyncIterationCompleted" /> event may fire
	/// repeatedly while the drawing is occurring.  The <see
	/// cref="AsyncGraphDrawer.DrawAsyncCompleted" /> event fires when the
	/// drawing is complete, an error occurs, or the drawing is cancelled.
	/// <see cref="AsyncGraphDrawer.DrawAsyncCancel" /> cancels the drawing.
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is already in
	/// progress.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public void
	DrawAsync
	(
		Bitmap bitmap,
		Boolean drawSelection
	)
	{
		AssertValid();

		// Set a flag that will be checked within the virtual methods
		// DrawVertices() and DrawEdges().

		m_bDrawSelection = drawSelection;

		base.DrawAsync(bitmap);
	}

	//*************************************************************************
	//	Method: DrawAsync()
	//
	/// <summary>
	/// Asynchronously draws the graph within a specified rectangle of a <see
	/// cref="Bitmap" />.
	/// </summary>
	///
	/// <param name="bitmap">
	/// <see cref="Bitmap" /> to draw onto.
	/// </param>
	///
	/// <param name="graphRectangle">
	///	Rectangle to draw within.
	/// </param>
	///
	/// <param name="drawSelection">
	///	If true and there are selected vertices or edges, those vertices and
	/// edges are drawn as selected.  Otherwise, all vertices and edges are
	/// drawn as unselected.
	/// </param>
	///
	/// <remarks>
	/// This method starts drawing the graph on a worker thread and returns
	/// immediately.  The <see
	/// cref="AsyncGraphDrawer.DrawAsyncIterationCompleted" /> event may fire
	/// repeatedly while the drawing is occurring.  The <see
	/// cref="AsyncGraphDrawer.DrawAsyncCompleted" /> event fires when the
	/// drawing is complete, an error occurs, or the drawing is cancelled.
	/// <see cref="AsyncGraphDrawer.DrawAsyncCancel" /> cancels the drawing.
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is already in
	/// progress.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public void
	DrawAsync
	(
		Bitmap bitmap,
		Rectangle graphRectangle,
		Boolean drawSelection
	)
	{
		AssertValid();

		// Set a flag that will be checked within the virtual methods
		// DrawVertices() and DrawEdges().

		m_bDrawSelection = drawSelection;

		base.DrawAsync(bitmap, graphRectangle);
	}

	//*************************************************************************
	//	Method: DrawAsync()
	//
	/// <summary>
	/// Asynchronously draws the graph onto a <see cref="Graphics" /> object.
	/// </summary>
	///
	/// <param name="graphics">
	/// <see cref="Graphics" /> object to draw onto.
	/// </param>
	///
	/// <param name="graphRectangle">
	/// Rectangle to draw within.
	/// </param>
	///
	/// <param name="drawSelection">
	///	If true and there are selected vertices or edges, those vertices and
	/// edges are drawn as selected.  Otherwise, all vertices and edges are
	/// drawn as unselected.
	/// </param>
	///
	/// <remarks>
	/// This method starts drawing the graph on a worker thread and returns
	/// immediately.  The <see
	/// cref="AsyncGraphDrawer.DrawAsyncIterationCompleted" /> event may fire
	/// repeatedly while the drawing is occurring.  The <see
	/// cref="AsyncGraphDrawer.DrawAsyncCompleted" /> event fires when the
	/// drawing is complete, an error occurs, or the drawing is cancelled.
	/// <see cref="AsyncGraphDrawer.DrawAsyncCancel" /> cancels the drawing.
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is already in
	/// progress.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public void
	DrawAsync
	(
		Graphics graphics,
		Rectangle graphRectangle,
		Boolean drawSelection
	)
	{
		AssertValid();

		// Set a flag that will be checked within the virtual methods
		// DrawVertices() and DrawEdges().

		m_bDrawSelection = drawSelection;

		base.DrawAsync(graphics, graphRectangle);
	}

	//*************************************************************************
	//	Method: RedrawVertex()
	//
	/// <overloads>
	/// Redraws a vertex.
	/// </overloads>
	///
	/// <summary>
	/// Redraws a vertex onto the entire rectangle of a <see cref="Bitmap" />.
	/// </summary>
	///
	/// <param name="vertex">
	/// Vertex to redraw.
	/// </param>
	///
	/// <param name="bitmap">
	/// <see cref="Bitmap" /> to draw onto.  Should be the same bitmap that was
	/// used when the entire graph was drawn by <see
	/// cref="Draw(Bitmap, Boolean)" />.
	/// </param>
	///
	/// <param name="drawSelection">
	///	If true and <paramref name="vertex" /> is selected, it is drawn as
	/// selected.  Otherwise, it is drawn as unselected.
	/// </param>
	///
	/// <remarks>
	/// Use this method to redraw a single vertex within a graph that was
	/// drawn by <see cref="Draw(Bitmap, Boolean)" />.  This is useful when you
	/// change the selected state of a vertex and you don't want to redraw the
	/// entire graph.
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is in progress.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public void
	RedrawVertex
	(
		IVertex vertex,
		Bitmap bitmap,
		Boolean drawSelection
	)
	{
		AssertValid();

		const String MethodName = "RedrawVertex";

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "vertex", vertex);

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "bitmap", bitmap);

		Rectangle oGraphRectangle = new Rectangle(Point.Empty, bitmap.Size);

		RedrawVertex(vertex, bitmap, oGraphRectangle, drawSelection);
	}

	//*************************************************************************
	//	Method: RedrawVertex()
	//
	/// <summary>
	/// Redraws a vertex within a specified rectangle of a <see
	/// cref="Bitmap" />.
	/// </summary>
	///
	/// <param name="vertex">
	/// Vertex to redraw.
	/// </param>
	///
	/// <param name="bitmap">
	/// <see cref="Bitmap" /> to draw onto.  Should be the same bitmap that was
	/// used when the entire graph was drawn by <see
	/// cref="Draw(Bitmap, Rectangle, Boolean)" />.
	/// </param>
	///
	/// <param name="graphRectangle">
	/// Rectangle to draw within.  Should be the same rectangle that was used
	/// when the entire graph was drawn by <see
	/// cref="Draw(Bitmap, Rectangle, Boolean)" />.
	/// </param>
	///
	/// <param name="drawSelection">
	///	If true and <paramref name="vertex" /> is selected, it is drawn as
	/// selected.  Otherwise, it is drawn as unselected.
	/// </param>
	///
	/// <remarks>
	/// Use this method to redraw a single vertex within a graph that was
	/// drawn by <see cref="Draw(Bitmap, Rectangle, Boolean)" />.  This is
	/// useful when you change the selected state of a vertex and you don't
	/// want to redraw the entire graph.
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is in progress.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public void
	RedrawVertex
	(
		IVertex vertex,
		Bitmap bitmap,
		Rectangle graphRectangle,
		Boolean drawSelection
	)
	{
		AssertValid();

		const String MethodName = "RedrawVertex";

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "vertex", vertex);

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "bitmap", bitmap);

		Graphics oGraphics = null;

		try
		{
			// Get a Graphics object to use.

			oGraphics = Graphics.FromImage(bitmap);

			// Redraw the vertex onto the Graphics object.

			RedrawVertex(vertex, oGraphics, graphRectangle, drawSelection);
		}
		finally
		{
			GraphicsUtil.DisposeGraphics(ref oGraphics);
		}
	}

	//*************************************************************************
	//	Method: RedrawVertex()
	//
	/// <summary>
	/// Redraws a vertex onto a <see cref="Graphics" /> object.
	/// </summary>
	///
	/// <param name="vertex">
	/// Vertex to redraw.
	/// </param>
	///
	/// <param name="graphics">
	/// <see cref="Graphics" /> object to draw onto.  Should be connected to
	/// the same drawing surface that was used when the entire graph was drawn
	/// by <see cref="Draw(Graphics, Rectangle, Boolean)" />.
	/// </param>
	///
	/// <param name="graphRectangle">
	/// Rectangle to draw within.  Should be the same rectangle that was used
	/// when the entire graph was drawn by <see
	/// cref="Draw(Graphics, Rectangle, Boolean)" />.
	/// </param>
	///
	/// <param name="drawSelection">
	///	If true and <paramref name="vertex" /> is selected, it is drawn as
	/// selected.  Otherwise, it is drawn as unselected.
	/// </param>
	///
	/// <remarks>
	/// Use this method to redraw a single vertex within a graph that was
	/// drawn by <see cref="Draw(Graphics, Rectangle, Boolean)" />.  This is
	/// useful when you change the selected state of a vertex and you don't
	/// want to redraw the entire graph.
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is in progress.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public void
	RedrawVertex
	(
		IVertex vertex,
		Graphics graphics,
		Rectangle graphRectangle,
		Boolean drawSelection
	)
	{
		AssertValid();

		const String MethodName = "RedrawVertex";

		CheckBusy(MethodName);

		ArgumentChecker oArgumentChecker = this.ArgumentChecker;

		oArgumentChecker.CheckArgumentNotNull(
			MethodName, "vertex", vertex);

		if (vertex.ParentGraph != m_oGraph)
		{
			oArgumentChecker.ThrowArgumentException(MethodName, "vertex",
				"The specified vertex does not belong to the graph."
				);
		}

		oArgumentChecker.CheckArgumentNotNull(
			MethodName, "graphics", graphics);

		GraphicsState oGraphicsState = graphics.Save();

		// Set the VertexDrawer.UseSelection and EdgeDrawer.UseSelection
		// properties if possible.

		SetUseSelection(drawSelection);

		try
		{
			m_oVertexDrawer.DrawVertex(
				vertex,
				CreateDrawContext(graphics, graphRectangle)
				);
		}
		finally
		{
			graphics.Restore(oGraphicsState);
		}
	}

	//*************************************************************************
	//	Method: RedrawEdge()
	//
	/// <overloads>
	/// Redraws an edge.
	/// </overloads>
	///
	/// <summary>
	/// Redraws an edge onto the entire rectangle of a <see cref="Bitmap" />.
	/// </summary>
	///
	/// <param name="edge">
	/// Edge to redraw.
	/// </param>
	///
	/// <param name="bitmap">
	/// <see cref="Bitmap" /> to draw onto.  Should be the same bitmap that was
	/// used when the entire graph was drawn by <see
	/// cref="Draw(Bitmap, Boolean)" />.
	/// </param>
	///
	/// <param name="drawSelection">
	///	If true and <paramref name="edge" /> or its vertices are selected, they
	/// are drawn as selected.  Otherwise, they are drawn as unselected.
	/// </param>
	///
	/// <remarks>
	/// Use this method to redraw a single edge within a graph that was
	/// drawn by <see cref="Draw(Bitmap, Boolean)" />.  This is useful when you
	/// change the selected state of an edge and you don't want to redraw the
	/// entire graph.
	///
	/// <para>
	/// Because drawing a single edge may cover up the edge's vertices, this
	/// method redraws the edge's vertices after redrawing the edge.
	/// </para>
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is in progress.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public void
	RedrawEdge
	(
		IEdge edge,
		Bitmap bitmap,
		Boolean drawSelection
	)
	{
		AssertValid();

		const String MethodName = "RedrawEdge";

		this.ArgumentChecker.CheckArgumentNotNull(MethodName, "edge", edge);

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "bitmap", bitmap);

		Rectangle oGraphRectangle = new Rectangle(Point.Empty, bitmap.Size);

		RedrawEdge(edge, bitmap, oGraphRectangle, drawSelection);
	}

	//*************************************************************************
	//	Method: RedrawEdge()
	//
	/// <summary>
	/// Redraws an edge within a specified rectangle of a
	/// <see cref="Bitmap" />.
	/// </summary>
	///
	/// <param name="edge">
	/// Edge to redraw.
	/// </param>
	///
	/// <param name="bitmap">
	/// <see cref="Bitmap" /> to draw onto.  Should be the same bitmap that was
	/// used when the entire graph was drawn by <see
	/// cref="Draw(Bitmap, Rectangle, Boolean)" />.
	/// </param>
	///
	/// <param name="graphRectangle">
	/// Rectangle to draw within.  Should be the same rectangle that was used
	/// when the entire graph was drawn by <see
	/// cref="Draw(Bitmap, Rectangle, Boolean)" />.
	/// </param>
	///
	/// <param name="drawSelection">
	///	If true and <paramref name="edge" /> or its vertices are selected, they
	/// are drawn as selected.  Otherwise, they are drawn as unselected.
	/// </param>
	///
	/// <remarks>
	/// Use this method to redraw a single edge within a graph that was drawn
	/// by <see cref="Draw(Bitmap, Rectangle, Boolean)" />.  This is useful
	/// when you change the selected state of an edge and you don't want to
	/// redraw the entire graph.
	///
	/// <para>
	/// Because drawing a single edge may cover up the edge's vertices, this
	/// method redraws the edge's vertices after redrawing the edge.
	/// </para>
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is in progress.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public void
	RedrawEdge
	(
		IEdge edge,
		Bitmap bitmap,
		Rectangle graphRectangle,
		Boolean drawSelection
	)
	{
		AssertValid();

		const String MethodName = "RedrawEdge";

		this.ArgumentChecker.CheckArgumentNotNull(MethodName, "edge", edge);

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "bitmap", bitmap);

		Graphics oGraphics = null;

		try
		{
			// Get a Graphics object to use.

			oGraphics = Graphics.FromImage(bitmap);

			// Redraw the edge onto the Graphics object.

			RedrawEdge(edge, oGraphics, graphRectangle, drawSelection);
		}
		finally
		{
			GraphicsUtil.DisposeGraphics(ref oGraphics);
		}
	}

	//*************************************************************************
	//	Method: RedrawEdge()
	//
	/// <summary>
	/// Redraws an edge onto a <see cref="Graphics" /> object.
	/// </summary>
	///
	/// <param name="edge">
	/// Edge to redraw.
	/// </param>
	///
	/// <param name="graphics">
	/// <see cref="Graphics" /> object to draw onto.  Should be connected to
	/// the same drawing surface that was used when the entire graph was drawn
	/// by <see cref="Draw(Graphics, Rectangle, Boolean)" />.
	/// </param>
	///
	/// <param name="graphRectangle">
	/// Rectangle to draw within.  Should be the same rectangle that was used
	/// when the entire graph was drawn by <see
	/// cref="Draw(Graphics, Rectangle, Boolean)" />.
	/// </param>
	///
	/// <param name="drawSelection">
	///	If true and <paramref name="edge" /> or its vertices are selected, they
	/// are drawn as selected.  Otherwise, they are drawn as unselected.
	/// </param>
	///
	/// <remarks>
	/// Use this method to redraw a single edge within a graph that was
	/// drawn by <see cref="Draw(Graphics, Rectangle, Boolean)" />.  This is
	/// useful when you change the selected state of an edge and you don't
	/// want to redraw the entire graph.
	///
	/// <para>
	/// Because drawing a single edge may cover up the edge's vertices, this
	/// method redraws the edge's vertices after redrawing the edge.
	/// </para>
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is in progress.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public void
	RedrawEdge
	(
		IEdge edge,
		Graphics graphics,
		Rectangle graphRectangle,
		Boolean drawSelection
	)
	{
		AssertValid();

		const String MethodName = "RedrawEdge";

		CheckBusy(MethodName);

		ArgumentChecker oArgumentChecker = this.ArgumentChecker;

		oArgumentChecker.CheckArgumentNotNull(MethodName, "edge", edge);

		if (edge.ParentGraph != m_oGraph)
		{
			oArgumentChecker.ThrowArgumentException(MethodName, "edge",
				"The specified edge does not belong to the graph."
				);
		}

		oArgumentChecker.CheckArgumentNotNull(
			MethodName, "graphics", graphics);

		GraphicsState oGraphicsState = graphics.Save();

		// Set the VertexDrawer.UseSelection and EdgeDrawer.UseSelection
		// properties if possible.

		SetUseSelection(drawSelection);

		try
		{
			DrawContext oDrawContext =
				CreateDrawContext(graphics, graphRectangle);

			// Redraw the edge.

			m_oEdgeDrawer.DrawEdge(edge, oDrawContext);

			// Get the edge's vertices.

			IVertex oVertex1, oVertex2;

			EdgeUtil.EdgeToVertices(edge, this.ClassName, MethodName,
				out oVertex1, out oVertex2);

			// Redraw the vertices.

			m_oVertexDrawer.DrawVertex(oVertex1, oDrawContext);

			m_oVertexDrawer.DrawVertex(oVertex2, oDrawContext);
		}
		finally
		{
			graphics.Restore(oGraphicsState);
		}
	}

	//*************************************************************************
	//	Method: SelectVertex()
	//
	/// <overloads>
	/// Changes the selected state of a vertex.
	/// </overloads>
	///
	/// <summary>
	/// Selects a vertex.
	/// </summary>
	///
	/// <param name="vertex">
	/// Vertex to select, as an <see cref="IVertex" />.
	/// </param>
	///
	/// <remarks>
	/// This method adds a "selected" flag to <paramref name="vertex" />.  It
	/// does not redraw <paramref name="vertex" />.  You must either draw the
	/// entire graph again, or call <see
	/// cref="RedrawVertex(IVertex, Bitmap, Boolean)" /> to redraw only
	/// <paramref name="vertex" />.
	/// </remarks>
	///
	/// <seealso cref="DeselectVertex" />
	/// <seealso cref="VertexIsSelected" />
	//*************************************************************************

	public static void
	SelectVertex
	(
		IVertex vertex
	)
	{
		const String MethodName = "SelectVertex";

		CheckVertexOrEdge(MethodName, "vertex", vertex);

		SelectVertexOrEdge(vertex);
	}

	//*************************************************************************
	//	Method: SelectVertex()
	//
	/// <summary>
	/// Selects or deselects a vertex.
	/// </summary>
	///
	/// <param name="vertex">
	/// Vertex to select or deselect, as an <see cref="IVertex" />.
	/// </param>
	///
	/// <param name="select">
	/// true to select <paramref name="vertex" />, false to deselect it.
	/// </param>
	///
	/// <remarks>
	/// If <paramref name="select" /> is true, this method adds a "selected"
	/// flag to <paramref name="vertex" />.  Otherwise, the flag is removed if
	/// it exists.
	///
	/// <para>
	/// This method does not redraw <paramref name="vertex" />.  You must
	/// either draw the entire graph again, or call <see
	/// cref="RedrawVertex(IVertex, Bitmap, Boolean)" /> to redraw only
	/// <paramref name="vertex" />.
	/// </para>
	///
	/// </remarks>
	///
	/// <seealso cref="DeselectVertex" />
	/// <seealso cref="VertexIsSelected" />
	//*************************************************************************

	public static void
	SelectVertex
	(
		IVertex vertex,
		Boolean select
	)
	{
		const String MethodName = "SelectVertex";

		CheckVertexOrEdge(MethodName, "vertex", vertex);

		SelectVertexOrEdge(vertex, select);
	}

	//*************************************************************************
	//	Method: DeselectVertex()
	//
	/// <summary>
	/// Deselects a vertex.
	/// </summary>
	///
	/// <param name="vertex">
	/// Vertex to deselect, as an <see cref="IVertex" />.
	/// </param>
	///
	/// <remarks>
	/// This method removes the "selected" flag from <paramref
	/// name="vertex" />, if the flag exists.  It does not redraw <paramref
	/// name="vertex" />.  You must either draw the entire graph again, or call
	/// <see cref="RedrawVertex(IVertex, Bitmap, Boolean)" /> to redraw only
	/// <paramref name="vertex" />.
	///
	/// <para>
	/// If <paramref name="vertex" /> is not selected, this method does
	/// nothing.
	/// </para>
	///
	/// </remarks>
	///
	/// <seealso cref="SelectVertex(IVertex)" />
	/// <seealso cref="VertexIsSelected" />
	//*************************************************************************

	public static void
	DeselectVertex
	(
		IVertex vertex
	)
	{
		const String MethodName = "DeselectVertex";

		CheckVertexOrEdge(MethodName, "vertex", vertex);

		DeselectVertexOrEdge(vertex);
	}

	//*************************************************************************
	//	Method: VertexIsSelected()
	//
	/// <summary>
	/// Determines whether a vertex is selected.
	/// </summary>
	///
	/// <param name="vertex">
	/// Vertex to check, as an <see cref="IVertex" />.
	/// </param>
	///
	/// <returns>
	/// true if <paramref name="vertex" /> is selected, false if it is not.
	/// </returns>
	///
	/// <seealso cref="SelectVertex(IVertex)" />
	/// <seealso cref="DeselectVertex" />
	//*************************************************************************

	public static Boolean
	VertexIsSelected
	(
		IVertex vertex
	)
	{
		const String MethodName = "VertexIsSelected";

		CheckVertexOrEdge(MethodName, "vertex", vertex);

		return ( VertexOrEdgeIsSelected(vertex) );
	}

	//*************************************************************************
	//	Method: SelectEdge()
	//
	/// <overloads>
	/// Changes the selected state of an edge.
	/// </overloads>
	///
	/// <summary>
	/// Selects an edge.
	/// </summary>
	///
	/// <param name="edge">
	/// Edge to select, as an <see cref="IEdge" />.
	/// </param>
	///
	/// <remarks>
	/// This method adds a "selected" flag to <paramref name="edge" />.  It
	/// does not redraw <paramref name="edge" />.  You must either draw the
	/// entire graph again, or call <see
	/// cref="RedrawEdge(IEdge, Bitmap, Boolean)" /> to redraw only
	/// <paramref name="edge" />.
	/// </remarks>
	///
	/// <seealso cref="DeselectEdge" />
	/// <seealso cref="EdgeIsSelected" />
	//*************************************************************************

	public static void
	SelectEdge
	(
		IEdge edge
	)
	{
		const String MethodName = "SelectEdge";

		CheckVertexOrEdge(MethodName, "edge", edge);

		SelectVertexOrEdge(edge);
	}

	//*************************************************************************
	//	Method: SelectEdge()
	//
	/// <summary>
	/// Selects or deselects an edge.
	/// </summary>
	///
	/// <param name="edge">
	/// Edge to select or deselect, as an <see cref="IEdge" />.
	/// </param>
	///
	/// <param name="select">
	/// true to select <paramref name="edge" />, false to deselect it.
	/// </param>
	///
	/// <remarks>
	/// If <paramref name="select" /> is true, this method adds a "selected"
	/// flag to <paramref name="edge" />.  Otherwise, the flag is removed if
	/// it exists.
	///
	/// <para>
	/// This method does not redraw <paramref name="edge" />.  You must either
	/// draw the entire graph again, or call <see
	/// cref="RedrawEdge(IEdge, Bitmap, Boolean)" /> to redraw only <paramref
	/// name="edge" />.
	/// </para>
	///
	/// </remarks>
	///
	/// <seealso cref="DeselectEdge" />
	/// <seealso cref="EdgeIsSelected" />
	//*************************************************************************

	public static void
	SelectEdge
	(
		IEdge edge,
		Boolean select
	)
	{
		const String MethodName = "SelectEdge";

		CheckVertexOrEdge(MethodName, "edge", edge);

		SelectVertexOrEdge(edge, select);
	}

	//*************************************************************************
	//	Method: DeselectEdge()
	//
	/// <summary>
	/// Deselects an edge.
	/// </summary>
	///
	/// <param name="edge">
	/// Edge to deselect, as an <see cref="IEdge" />.
	/// </param>
	///
	/// <remarks>
	/// This method removes the "selected" flag from <paramref
	/// name="edge" />, if the flag exists.  It does not redraw <paramref
	/// name="edge" />.  You must either draw the entire graph again, or call
	/// <see cref="RedrawEdge(IEdge, Bitmap, Boolean)" /> to redraw only
	/// <paramref name="edge" />.
	///
	/// <para>
	/// If <paramref name="edge" /> is not selected, this method does nothing.
	/// </para>
	///
	/// </remarks>
	///
	/// <seealso cref="SelectEdge(IEdge)" />
	/// <seealso cref="EdgeIsSelected" />
	//*************************************************************************

	public static void
	DeselectEdge
	(
		IEdge edge
	)
	{
		const String MethodName = "DeselectEdge";

		CheckVertexOrEdge(MethodName, "edge", edge);

		DeselectVertexOrEdge(edge);
	}

	//*************************************************************************
	//	Method: EdgeIsSelected()
	//
	/// <summary>
	/// Determines whether an edge is selected.
	/// </summary>
	///
	/// <param name="edge">
	/// Edge to check, as an <see cref="IEdge" />.
	/// </param>
	///
	/// <returns>
	/// true if <paramref name="edge" /> is selected, false if it is not.
	/// </returns>
	///
	/// <seealso cref="SelectEdge(IEdge)" />
	/// <seealso cref="DeselectEdge" />
	//*************************************************************************

	public static Boolean
	EdgeIsSelected
	(
		IEdge edge
	)
	{
		const String MethodName = "EdgeIsSelected";

		CheckVertexOrEdge(MethodName, "edge", edge);

		return ( VertexOrEdgeIsSelected(edge) );
	}

	//*************************************************************************
	//	Method: SelectVertexOrEdge()
	//
	/// <summary>
	/// Selects or deselects a vertex or edge.
	/// </summary>
	///
	/// <param name="vertexOrEdge">
	/// Vertex or edge to select or deselect, as an <see
	/// cref="IMetadataProvider" />.
	/// </param>
	///
	/// <param name="select">
	/// true to select <paramref name="vertexOrEdge" />, false to deselect it.
	/// </param>
	//*************************************************************************

	private static void
	SelectVertexOrEdge
	(
		IMetadataProvider vertexOrEdge,
		Boolean select
	)
	{
		Debug.Assert(vertexOrEdge != null);

		if (select)
		{
			SelectVertexOrEdge(vertexOrEdge);
		}
		else
		{
			DeselectVertexOrEdge(vertexOrEdge);
		}
	}

	//*************************************************************************
	//	Method: SelectVertexOrEdge()
	//
	/// <overloads>
	/// Changes the selected state of a vertex or edge.
	/// </overloads>
	///
	/// <summary>
	/// Selects a vertex or edge.
	/// </summary>
	///
	/// <param name="vertexOrEdge">
	/// Vertex or edge to select, as an <see cref="IMetadataProvider" />.
	/// </param>
	//*************************************************************************

	private static void
	SelectVertexOrEdge
	(
		IMetadataProvider vertexOrEdge
	)
	{
		Debug.Assert(vertexOrEdge != null);

		vertexOrEdge.SetValue(ReservedMetadataKeys.IsSelected, SelectedValue);
	}

	//*************************************************************************
	//	Method: DeselectVertexOrEdge()
	//
	/// <summary>
	/// Deselects a vertex or edge.
	/// </summary>
	///
	/// <param name="vertexOrEdge">
	/// Vertex or edge to deselect, as an <see cref="IMetadataProvider" />.
	/// </param>
	//*************************************************************************

	private static void
	DeselectVertexOrEdge
	(
		IMetadataProvider vertexOrEdge
	)
	{
		Debug.Assert(vertexOrEdge != null);

		vertexOrEdge.RemoveKey(ReservedMetadataKeys.IsSelected);
	}

	//*************************************************************************
	//	Method: VertexOrEdgeIsSelected()
	//
	/// <summary>
	/// Determines whether a vertex or edge is selected.
	/// </summary>
	///
	/// <param name="vertexOrEdge">
	/// Vertex or edge to check, as an <see cref="IMetadataProvider" />.
	/// </param>
	///
	/// <returns>
	/// true if <paramref name="vertexOrEdge" /> is selected, false if it is
	/// not.
	/// </returns>
	//*************************************************************************

	private static Boolean
	VertexOrEdgeIsSelected
	(
		IMetadataProvider vertexOrEdge
	)
	{
		Debug.Assert(vertexOrEdge != null);

		return ( vertexOrEdge.ContainsKey(ReservedMetadataKeys.IsSelected) );
	}

	//*************************************************************************
	//	Method: CheckVertexOrEdge()
	//
	/// <summary>
	/// Throws an exception if a vertex or edge argument passed to one of the
	/// static vertex/edge selection methods is null.
	/// </summary>
	///
	/// <param name="sMethodName">
	/// Name of the method calling this method.
	/// </param>
	///
	/// <param name="sArgumentName">
	/// Name of the argument.
	/// </param>
	///
	/// <param name="oVertexOrEdge">
	/// Vertex or edge to check, as an <see cref="IMetadataProvider" />.
	/// </param>
	//*************************************************************************

	private static void
	CheckVertexOrEdge
	(
		String sMethodName,
		String sArgumentName,
		IMetadataProvider oVertexOrEdge
	)
	{
		Debug.Assert( !String.IsNullOrEmpty(sMethodName) );
		Debug.Assert( !String.IsNullOrEmpty(sArgumentName) );

		// This method is static, so it can't use the ArgumentChecker property
		// defined in the base class.  Create a new object.

		ArgumentChecker oArgumentChecker =
			new ArgumentChecker(
				typeof(MultiSelectionGraphDrawer).FullName);

		oArgumentChecker.CheckArgumentNotNull(
			sMethodName, sArgumentName, oVertexOrEdge);
	}

	//*************************************************************************
	//	Method: DrawEdges()
	//
	/// <summary>
	/// Draws the graph's edges.
	/// </summary>
	///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
	///
	/// <remarks>
	/// This method draws each edge in the graph's Edges collection.
	/// </remarks>
	//*************************************************************************

	protected override void
	DrawEdges
	(
		DrawContext oDrawContext
	)
	{
		AssertValid();
		Debug.Assert(oDrawContext != null);

		// Set the VertexDrawer.UseSelection and EdgeDrawer.UseSelection
		// properties if possible.

		SetUseSelection(m_bDrawSelection);

		base.DrawEdges(oDrawContext);
	}

	//*************************************************************************
	//	Method: DrawVertices()
	//
	/// <summary>
	/// Draws the graph's vertices.
	/// </summary>
	///
    /// <param name="oDrawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
	///
	/// <remarks>
	/// This method draws each vertex in the graph's Vertices collection.
	/// </remarks>
	//*************************************************************************

	protected override void
	DrawVertices
	(
		DrawContext oDrawContext
	)
	{
		AssertValid();
		Debug.Assert(oDrawContext != null);

		// Set the VertexDrawer.UseSelection and EdgeDrawer.UseSelection
		// properties if possible.

		SetUseSelection(m_bDrawSelection);

		base.DrawVertices(oDrawContext);
	}

	//*************************************************************************
	//	Method: CreateDrawContext()
	//
	/// <summary>
	///	Creates a <see cref="DrawContext" /> object for use when redrawing a
	/// vertex or edge.
	/// </summary>
	///
	/// <param name="oGraphics">
	/// <see cref="Graphics" /> object to draw onto.
	/// </param>
	///
	/// <param name="oGraphRectangle">
	/// <see cref="Rectangle" /> to draw within.
	/// </param>
	///
	/// <returns>
	/// A new <see cref="DrawContext" /> object.
	/// </returns>
	//*************************************************************************

	protected DrawContext
	CreateDrawContext
	(
		Graphics oGraphics,
		Rectangle oGraphRectangle
	)
	{
		Debug.Assert(oGraphics != null);
		AssertValid();

		// Set the clip region, then let the base class prepare the Graphics
		// object in the same way it prepares Graphics objects it uses for its
		// own drawing operations.

		oGraphics.Clip = new Region(oGraphRectangle);

		SetSmoothingModeForVerticesAndEdges(oGraphics);

		return ( new DrawContext(
			this, oGraphics, oGraphRectangle, this.Layout.Margin) );
	}

	//*************************************************************************
	//	Method: SetUseSelection()
	//
	/// <summary>
	/// Sets the <see cref="VertexDrawer.UseSelection" /> and <see
	/// cref="EdgeDrawer.UseSelection" /> properties if possible.
	/// </summary>
	///
    /// <param name="bUseSelection">
	/// true if the selected state of a vertex or edge should be used.
    /// </param>
	///
	/// <remarks>
	/// If the current vertex drawer is a <see
	/// cref="Visualization.VertexDrawer" />, this method sets its <see
	/// cref="VertexDrawer.UseSelection" /> property to <paramref
	/// name="bUseSelection" />.
	///
	/// <para>
	/// If the current edge drawer is an <see
	/// cref="Visualization.EdgeDrawer" />, this method sets its <see
	/// cref="EdgeDrawer.UseSelection" /> property to <paramref
	/// name="bUseSelection" />.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	protected void
	SetUseSelection
	(
		Boolean bUseSelection
	)
	{
		AssertValid();

		if (m_oVertexDrawer is VertexDrawer)
		{
			VertexDrawer oVertexDrawer = (VertexDrawer)m_oVertexDrawer;
			oVertexDrawer.UseSelection = bUseSelection;
		}

		if (m_oEdgeDrawer is EdgeDrawer)
		{
			EdgeDrawer oEdgeDrawer = (EdgeDrawer)m_oEdgeDrawer;
			oEdgeDrawer.UseSelection = bUseSelection;
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

		// m_bDrawSelection
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// <summary>
	/// Value to pass to <see cref="GraphVertexEdgeBase.SetValue" /> to select
	/// a vertex or edge.
	/// </summary>

	protected const Object SelectedValue = null;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// true if selected vertices and edges should be drawn as selected, false
	/// if all vertices and edges should be drawn as unselected.

	protected Boolean m_bDrawSelection;
}

}
