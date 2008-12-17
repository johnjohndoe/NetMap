
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: NodeXLControl
//
/// <summary>
/// TODO: This control is not yet ready for use.
/// </summary>
//*****************************************************************************

public class NodeXLControl : FrameworkElement
{
    //*************************************************************************
    //  Constructor: NodeXLControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="NodeXLControl" /> class.
    /// </summary>
    //*************************************************************************

    public NodeXLControl()
    {
		m_oGraph = new Graph();
        m_oGraphVisualCollection = new GraphVisualCollection(m_oGraph, this);
		m_oAsyncLayout = new FruchtermanReingoldLayout();

		m_oAsyncLayout.LayOutGraphIterationCompleted +=
			new EventHandler(this.AsyncLayout_LayOutGraphIterationCompleted);

		m_oAsyncLayout.LayOutGraphCompleted +=
			new AsyncCompletedEventHandler(
				this.AsyncLayout_LayOutGraphCompleted);

		m_eLayoutState = LayoutState.Stable;

		this.AddLogicalChild(m_oGraphVisualCollection.VisualCollection);

		if (
			this.SelectionChanged != null &&
			this.GraphMouseDown != null &&
			this.GraphMouseUp != null &&
			this.VertexClick != null &&
			this.VertexDoubleClick != null &&
			this.VertexMouseHover != null &&
			this.DrawingGraph != null &&
			this.GraphDrawn != null &&
			this.VertexMoved != null
			)
		{
			// TODO: Avoid compiler warnings for now.
		}

		AssertValid();
    }

    //*************************************************************************
    //  Property: Graph
    //
    /// <summary>
    /// Gets or sets the graph to draw.
    /// </summary>
    ///
    /// <value>
    /// The graph to draw, as an <see cref="IGraph" />.
    /// </value>
	///
	/// <remarks>
	///	Do not set this property to a graph that is already owned by another
	/// graph drawer.  If you want to simultaneously draw the same graph with
	/// two different graph drawers, make a copy of the graph using
	/// IGraph.<see cref="IGraph.Clone(Boolean, Boolean)" />.
	///
	/// <para>
	/// An exception is thrown if this property is set while an asynchronous
	/// drawing is in progress.  Check <see cref="IsDrawing" /> before using
	/// this property.
	/// </para>
	///
	/// </remarks>
    //*************************************************************************

	[System.ComponentModel.Browsable(false)]
	[System.ComponentModel.ReadOnly(true)]

    public IGraph
    Graph
    {
        get
		{
			AssertValid();

			return (m_oGraph);
		}

        set
		{
			const String PropertyName = "Graph";

			this.ArgumentChecker.CheckPropertyNotNull(PropertyName, value);

			// TODO

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: Layout
    //
    /// <summary>
    /// Gets or sets the object to use to lay out the graph.
    /// </summary>
    ///
    /// <value>
    /// The object to use to lay out the graph, as an <see cref="ILayout" />.
	/// The default value is a <see cref="FruchtermanReingoldLayout" /> object.
    /// </value>
	///
	/// <remarks>
	/// An exception is thrown if this property is set while an asynchronous
	/// drawing is in progress.  Check <see cref="IsDrawing" /> before using
	/// this property.
	/// </remarks>
    //*************************************************************************

	[System.ComponentModel.Browsable(false)]
	[System.ComponentModel.ReadOnly(true)]

    public ILayout
    Layout
    {
        get
		{
			AssertValid();

			return (null);  // TODO
		}

        set
		{
			const String PropertyName = "Layout";

			this.ArgumentChecker.CheckPropertyNotNull(PropertyName, value);

			// TODO

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: MouseSelectionMode
    //
    /// <summary>
	/// Gets or sets a value that determines what gets selected when a vertex
	/// is clicked with the mouse.
    /// </summary>
    ///
    /// <value>
    /// A <see cref="Microsoft.NodeXL.Visualization.MouseSelectionMode" />
	/// value.  The default value is <see cref="Microsoft.NodeXL.
	/// Visualization.MouseSelectionMode.SelectVertexAndIncidentEdges" />.
    /// </value>
	///
	/// <remarks>
	/// When this property is set to SelectVertexAndIncidentEdges, one or more
	/// vertices can be selected with the mouse.  If the Control key is not
	/// pressed, clicking a vertex clears the current selection, then selects
	/// the clicked vertex and its incident edges.  If the Control key is
	/// pressed, clicking a vertex toggles the selection state of the vertex
	/// and its incident edges without affecting the other vertices and edges.
	/// Clicking without the Control key on an area not occupied by a vertex
	/// clears the current selection.
	///
	/// <para>
	/// When this property is set to SelectVertexAndIncidentEdges, vertices can
	/// also be selected by dragging a marquee over them.  If no key is
	/// pressed, the current selection is cleared, then any vertices within the
	/// marquee are selected, along with their incident edges.  If the Shift
	/// key is pressed, the vertices and their incident edges are added to the
	/// current selection.  If the Alt key is pressed, the vertices and their
	/// incident edges are subtracted from the current selection.
	/// </para>
	///
	/// <para>
	/// When this property is set to SelectVertex, the behavior is the same as
	/// with SelectVertexAndIncidentEdges, except that the incident edges are
	/// not selected.
	/// </para>
	///
	/// <para>
	/// When this property is set to SelectVertexAndIncidentEdges or
	/// SelectVertex, clicking on a vertex results in the following sequence:
	/// </para>
	///
	///	<list type="bullet">
	///
	/// <item><description>
	/// The <see cref="GraphMouseDown" /> event fires.
	/// </description></item>
	///
	/// <item><description>
	/// The <see cref="VertexClick" /> event fires.
	/// </description></item>
	///
	/// <item><description>
	/// The vertex and possibly its incident edges are redrawn as selected or
	/// unselected.
	/// </description></item>
	///
	/// <item><description>
	/// The <see cref="SelectionChanged" /> event fires.
	/// </description></item>
	///
	/// <item><description>
	/// The <see cref="GraphMouseUp" /> event fires.
	/// </description></item>
	///
	///	</list>
	///
	/// <para>
	/// When this property is set to SelectNothing, clicking on a vertex
	/// results in the following sequence:
	/// </para>
	///
	///	<list type="bullet">
	///
	/// <item><description>
	/// The <see cref="GraphMouseDown" /> event fires.
	/// </description></item>
	///
	/// <item><description>
	/// The <see cref="VertexClick" /> event fires.
	/// </description></item>
	///
	/// <item><description>
	/// The <see cref="GraphMouseUp" /> event fires.
	/// </description></item>
	///
	///	</list>
	///
	/// <para>
	/// Set this property to SelectNothing if you want mouse clicks to have no
	/// effect, or if you want to customize the click behavior.
	/// </para>
	///
	/// </remarks>
	///
	/// <seealso cref="SetVertexSelected" />
	/// <seealso cref="SetEdgeSelected" />
	/// <seealso cref="SetSelected" />
	/// <seealso cref="SelectAll" />
	/// <seealso cref="DeselectAll" />
    /// <seealso cref="SelectedVertices" />
    /// <seealso cref="SelectedEdges" />
	/// <seealso cref="SelectionChanged" />
    //*************************************************************************

	[Category("Behavior")]

    public MouseSelectionMode
    MouseSelectionMode
    {
        get
		{
			AssertValid();

			return (MouseSelectionMode.SelectVertexAndIncidentEdges);  // TODO
		}

        set
		{
			// TODO

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: AllowVertexDrag
    //
    /// <summary>
    /// Gets or sets a flag indicating whether a vertex can be moved by
	/// dragging it with the mouse.
    /// </summary>
    ///
    /// <value>
	/// true if a vertex can be moved by dragging it with the mouse, false
	/// otherwise.  The default value is true.
    /// </value>
	///
	/// <remarks>
	/// When this property is true, the user can click on a vertex and move it
	/// while dragging the mouse.  The vertex and its incident edges are
	/// redrawn, but no other vertices or edges are affected.
	/// </remarks>
    //*************************************************************************

	[Category("Behavior")]

    public Boolean
    AllowVertexDrag
    {
        get
		{
			AssertValid();

			return (true);  // TODO
		}

        set
		{
			// TODO

			AssertValid();
		}
    }

	//*************************************************************************
	//	Property: ShowToolTips
	//
	/// <summary>
	///	Gets or sets a value indicating whether vertex tooltips should be
	/// shown.
	/// </summary>
	///
	/// <value>
	///	true to show vertex tooltips.  The default value is false.
	/// </value>
	///
	/// <remarks>
	/// To show a tooltip when the mouse is hovered over a vertex, you must set
	/// this property to true (the default is false) and assign a tooltip
	/// string to each of the graph's vertices.
	///
	/// <para>
	/// Use Vertex.<see cref="IMetadataProvider.SetValue" /> to assign tooltip
	/// strings to the graph's vertices.  The key must be the reserved key
	/// ReservedMetadataKeys.<see cref="ReservedMetadataKeys.ToolTip" /> and
	/// the value must be the tooltip string.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	[Category("Mouse")]

	public Boolean
	ShowToolTips
	{
		get
		{
			AssertValid();

			return (false);  // TODO
		}

		set
		{
			// TODO

			AssertValid();
		}
	}

    //*************************************************************************
    //  Property: SelectedVertices
    //
    /// <summary>
    /// Gets an array of the graph's selected vertices.
    /// </summary>
    ///
    /// <value>
	/// An array of the graph's selected vertices.
    /// </value>
	///
	/// <remarks>
	/// If there are no selected vertices, the returned array has zero
	/// elements.  The returned value is never null.
	/// </remarks>
	///
    /// <seealso cref="MouseSelectionMode" />
	/// <seealso cref="SetVertexSelected" />
	/// <seealso cref="SetEdgeSelected" />
	/// <seealso cref="SetSelected" />
	/// <seealso cref="SelectAll" />
	/// <seealso cref="DeselectAll" />
    /// <seealso cref="SelectedEdges" />
	/// <seealso cref="SelectionChanged" />
    //*************************************************************************

	[System.ComponentModel.Browsable(false)]
	[System.ComponentModel.ReadOnly(true)]

    public IVertex []
    SelectedVertices
    {
        get
		{
			AssertValid();

			return (null);  // TODO
		}
    }

    //*************************************************************************
    //  Property: SelectedEdges
    //
    /// <summary>
    /// Gets an array of the graph's selected edges.
    /// </summary>
    ///
    /// <value>
	/// An array of the graph's selected edges.
    /// </value>
	///
	/// <remarks>
	/// If there are no selected edges, the returned array has zero elements.
	/// The returned value is never null.
	/// </remarks>
	///
    /// <seealso cref="MouseSelectionMode" />
	/// <seealso cref="SetVertexSelected" />
	/// <seealso cref="SetEdgeSelected" />
	/// <seealso cref="SetSelected" />
	/// <seealso cref="SelectAll" />
	/// <seealso cref="DeselectAll" />
    /// <seealso cref="SelectedVertices" />
	/// <seealso cref="SelectionChanged" />
    //*************************************************************************

	[System.ComponentModel.Browsable(false)]
	[System.ComponentModel.ReadOnly(true)]

    public IEdge []
    SelectedEdges
    {
        get
		{
			AssertValid();

			return (null);  // TODO
		}
    }

    //*************************************************************************
    //  Property: IsDrawing
    //
    /// <summary>
	/// Gets a value indicating whether an asynchronous drawing operation is in
	/// progress.
    /// </summary>
    ///
    /// <value>
    /// true if an asynchronous drawing operation is in progress.
    /// </value>
	///
	/// <remarks>
	/// The <see cref="DrawingGraph" /> event fires before graph drawing
	/// begins.  The <see cref="GraphDrawn" /> event fires after graph drawing
	/// completes.
	/// </remarks>
    //*************************************************************************

    public Boolean
    IsDrawing
    {
        get
        {
            AssertValid();

			return (false);  // TODO
        }
    }

	//*************************************************************************
	//	Method: BeginUpdate()
	//
	/// <summary>
	/// Disables any redrawing of the graph.
	/// </summary>
	///
	/// <remarks>
	/// To improve performance, call <see cref="BeginUpdate" /> before adding
	///	vertices and edges to the graph.  Call <see cref="EndUpdate()" /> when
	///	you are done.
	/// </remarks>
	///
	///	<seealso cref="EndUpdate()" />
	//*************************************************************************

	public void
	BeginUpdate()
	{
		AssertValid();

		// TODO
	}

	//*************************************************************************
	//	Method: EndUpdate()
	//
	/// <summary>
	/// Enables the redrawing of the graph.
	/// </summary>
	///
	/// <remarks>
	/// To improve performance, call <see cref="BeginUpdate" /> before adding
	///	vertices and edges to the graph.  Call <see cref="EndUpdate()" /> when
	///	you are done.
	/// </remarks>
	///
	///	<seealso cref="BeginUpdate" />
	//*************************************************************************

	public void
	EndUpdate()
	{
		AssertValid();

		// TODO: Guard against IsBusy.

		// Start laying out the graph.

		m_eLayoutState = LayoutState.LayoutRequired;
		this.InvalidateVisual();
	}

	//*************************************************************************
	//	Method: SetVertexSelected()
	//
	/// <summary>
	///	Selects or deselects a vertex.
	/// </summary>
	///
	/// <param name="vertex">
	/// Vertex to select or deselect.  Can't be null.
	/// </param>
	///
	/// <param name="selected">
	/// true to select <paramref name="vertex" />, false to deselect it.
	/// </param>
	///
	/// <param name="alsoIncidentEdges">
	/// true to also select or deselect the vertex's incident edges, false to
	/// leave the incident edges alone.
	/// </param>
	///
	/// <remarks>
	/// Selecting or deselecting a vertex does not affect the selected state of
	/// the other vertices.
	///
	/// <para>
	/// To select a set of vertices and edges, use the <see
	/// cref="SetSelected" /> method instead.
	/// </para>
	///
	/// </remarks>
	///
    /// <seealso cref="MouseSelectionMode" />
	/// <seealso cref="SetEdgeSelected" />
	/// <seealso cref="SetSelected" />
	/// <seealso cref="SelectAll" />
	/// <seealso cref="DeselectAll" />
    /// <seealso cref="SelectedVertices" />
    /// <seealso cref="SelectedEdges" />
	/// <seealso cref="SelectionChanged" />
	//*************************************************************************

	public void
	SetVertexSelected
	(
		IVertex vertex,
		Boolean selected,
		Boolean alsoIncidentEdges
	)
	{
		AssertValid();

		// TODO
	}

	//*************************************************************************
	//	Method: SetEdgeSelected()
	//
	/// <summary>
	///	Selects or deselects an edge.
	/// </summary>
	///
	/// <param name="edge">
	/// Edge to select or deselect.  Can't be null.
	/// </param>
	///
	/// <param name="selected">
	/// true to select <paramref name="edge" />, false to deselect it.
	/// </param>
	///
	/// <param name="alsoAdjacentVertices">
	/// true to also select or deselect the edge's adjacent vertices, false to
	/// leave the adjacent vertices alone.
	/// </param>
	///
	/// <remarks>
	/// Selecting or deselecting an edge does not affect the selected state of
	/// the other edges.
	///
	/// <para>
	/// To select a set of vertices and edges, use the <see
	/// cref="SetSelected" /> method instead.
	/// </para>
	///
	/// </remarks>
	///
    /// <seealso cref="MouseSelectionMode" />
	/// <seealso cref="SetVertexSelected" />
	/// <seealso cref="SetSelected" />
	/// <seealso cref="SelectAll" />
	/// <seealso cref="DeselectAll" />
    /// <seealso cref="SelectedVertices" />
    /// <seealso cref="SelectedEdges" />
	/// <seealso cref="SelectionChanged" />
	//*************************************************************************

	public void
	SetEdgeSelected
	(
		IEdge edge,
		Boolean selected,
		Boolean alsoAdjacentVertices
	)
	{
		AssertValid();

		// TODO
	}

	//*************************************************************************
	//	Method: SetSelected()
	//
	/// <summary>
	///	Selects a set of vertices and edges.
	/// </summary>
	///
	/// <param name="vertices">
	/// Array of zero or more vertices to select.
	/// </param>
	///
	/// <param name="edges">
	/// Array of zero or more edges to select.
	/// </param>
	///
	/// <remarks>
	/// This method deselects any selected vertices and edges, then selects the
	/// vertices and edges specified in <paramref name="vertices" /> and
	/// <paramref name="edges" />.  It is more efficient than making multiple
	/// calls to <see cref="SetVertexSelected" /> and <see
	/// cref="SetEdgeSelected" />.
	/// </remarks>
	///
    /// <seealso cref="MouseSelectionMode" />
	/// <seealso cref="SetVertexSelected" />
	/// <seealso cref="SetEdgeSelected" />
	/// <seealso cref="DeselectAll" />
    /// <seealso cref="SelectedVertices" />
    /// <seealso cref="SelectedEdges" />
	/// <seealso cref="SelectionChanged" />
	//*************************************************************************

	public void
	SetSelected
	(
		IVertex [] vertices,
		IEdge [] edges
	)
	{
		AssertValid();

		// TODO
	}

	//*************************************************************************
	//	Method: SelectAll()
	//
	/// <summary>
	///	Selects all vertices and edges.
	/// </summary>
	///
    /// <seealso cref="MouseSelectionMode" />
	/// <seealso cref="SetVertexSelected" />
	/// <seealso cref="SetEdgeSelected" />
	/// <seealso cref="SetSelected" />
	/// <seealso cref="DeselectAll" />
    /// <seealso cref="SelectedVertices" />
    /// <seealso cref="SelectedEdges" />
	/// <seealso cref="SelectionChanged" />
	//*************************************************************************

	public void
	SelectAll()
	{
		AssertValid();

		// TODO
	}

	//*************************************************************************
	//	Method: DeselectAll()
	//
	/// <summary>
	///	Deselects all vertices and edges.
	/// </summary>
	///
    /// <seealso cref="MouseSelectionMode" />
	/// <seealso cref="SetVertexSelected" />
	/// <seealso cref="SetEdgeSelected" />
	/// <seealso cref="SetSelected" />
	/// <seealso cref="SelectAll" />
    /// <seealso cref="SelectedVertices" />
    /// <seealso cref="SelectedEdges" />
	/// <seealso cref="SelectionChanged" />
	//*************************************************************************

	public void
	DeselectAll()
	{
		AssertValid();

		// TODO
	}

	//*************************************************************************
	//	Event: SelectionChanged
	//
	/// <summary>
	///	Occurs when the selection state of a vertex or edge changes.
	/// </summary>
	///
	/// <remarks>
	/// This event occurs when one or more of the graph's vertices or edges are
	/// selected or deselected.  Updated arrays of the graph's selected
	/// vertices and edges can be obtained from the <see
	/// cref="SelectedVertices" /> and <see cref="SelectedEdges" /> properties.
	/// </remarks>
	///
    /// <seealso cref="MouseSelectionMode" />
	/// <seealso cref="SetVertexSelected" />
	/// <seealso cref="SetEdgeSelected" />
	/// <seealso cref="SetSelected" />
	/// <seealso cref="SelectAll" />
	/// <seealso cref="DeselectAll" />
    /// <seealso cref="SelectedVertices" />
    /// <seealso cref="SelectedEdges" />
	//*************************************************************************

	[Category("Property Changed")]

	public event EventHandler SelectionChanged;


	//*************************************************************************
	//	Event: GraphMouseDown
	//
	/// <summary>
	///	Occurs when the mouse pointer is within the graph and a mouse button
	///	is pressed.
	/// </summary>
	///
	/// <remarks>
	/// See <see cref="MouseSelectionMode" /> for details on how vertices are
	/// selected with the mouse.
	/// </remarks>
	///
    /// <seealso cref="MouseSelectionMode" />
	/// <seealso cref="SetVertexSelected" />
	/// <seealso cref="SetEdgeSelected" />
	/// <seealso cref="SetSelected" />
	/// <seealso cref="SelectAll" />
	/// <seealso cref="DeselectAll" />
    /// <seealso cref="SelectedVertices" />
    /// <seealso cref="SelectedEdges" />
	//*************************************************************************

	[Category("Mouse")]

	public event GraphMouseEventHandler GraphMouseDown;


	//*************************************************************************
	//	Event: GraphMouseUp
	//
	/// <summary>
	///	Occurs when the mouse pointer is within the graph and a mouse button
	///	is released.
	/// </summary>
	///
    /// <seealso cref="MouseSelectionMode" />
	///	<seealso cref="SelectedVertices" />
	//*************************************************************************

	[Category("Mouse")]

	public event GraphMouseEventHandler GraphMouseUp;


	//*************************************************************************
	//	Event: VertexClick
	//
	/// <summary>
	///	Occurs when a vertex is clicked.
	/// </summary>
	///
    /// <seealso cref="MouseSelectionMode" />
	///	<seealso cref="SelectedVertices" />
	//*************************************************************************

	[Category("Mouse")]

	public event VertexEventHandler VertexClick;


	//*************************************************************************
	//	Event: VertexDoubleClick
	//
	/// <summary>
	///	Occurs when a vertex is double-clicked.
	/// </summary>
	///
    /// <seealso cref="MouseSelectionMode" />
	///	<seealso cref="SelectedVertices" />
	//*************************************************************************

	[Category("Mouse")]

	public event VertexEventHandler VertexDoubleClick;


	//*************************************************************************
	//	Event: VertexMouseHover
	//
	/// <summary>
	///	Occurs when the mouse pointer hovers over a vertex and tooltips are
	/// enabled.
	/// </summary>
	///
	/// <remarks>
	/// If <see cref="ShowToolTips" /> is true, this event fires after the
	/// vertex's tooltip is shown.  If <see cref="ShowToolTips" /> is false,
	/// this event does not fire.
	///
	/// <para>
	/// To handle this event without tooltips being shown, set <see
	/// cref="ShowToolTips" /> to true but do not set tooltip strings on any of
	/// the graph's vertices.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	[Category("Mouse")]

	public event VertexEventHandler VertexMouseHover;


	//*************************************************************************
	//	Event: DrawingGraph
	//
	/// <summary>
	///	Occurs before graph drawing begins.
	/// </summary>
	///
	/// <remarks>
	/// Graph drawing occurs asynchronously.  This event fires before graph
	/// drawing begins.
	///
	/// <para>
	/// The <see cref="GraphDrawn" /> event fires after drawing is complete.
	/// </para>
	///
	/// <para>
	/// The <see cref="IsDrawing" /> property can also be used to determine
	/// whether a graph is being drawn.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	[Category("Action")]

	public event EventHandler DrawingGraph;


	//*************************************************************************
	//	Event: GraphDrawn
	//
	/// <summary>
	///	Occurs after graph drawing completes.
	/// </summary>
	///
	/// <remarks>
	/// Graph drawing occurs asynchronously.  This event fires when the graph
	/// is completely drawn.
	///
	/// <para>
	/// The <see cref="DrawingGraph" /> event fires before graph drawing
	/// begins.
	/// </para>
	///
	/// <para>
	/// The <see cref="IsDrawing" /> property can also be used to determine
	/// whether a graph is being drawn.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	[Category("Action")]

	public event EventHandler GraphDrawn;


	//*************************************************************************
	//	Event: VertexMoved
	//
	/// <summary>
	///	Occurs after a vertex is moved to a new location with the mouse.
	/// </summary>
	///
	/// <remarks>
	/// This event is fired when the user releases the mouse button after
	/// dragging a vertex to a new location.
	/// </remarks>
	//*************************************************************************

	[Category("Action")]

	public event VertexEventHandler VertexMoved;


	//*************************************************************************
	//  Enum: LayoutState
	//
	/// <summary>
	/// Indicates the state of the graph's layout.
	/// </summary>
	//*************************************************************************

	protected enum
	LayoutState
	{
		/// <summary>
		/// The graph is empty, or it's layout is complete and
		/// m_oGraphVisualCollection has been populated with the graph's
		/// contents.
		/// </summary>

		Stable,

		/// <summary>
		/// The graph needs to be laid out.
		/// </summary>

		LayoutRequired,

		/// <summary>
		/// The graph is being asynchronously laid out.
		/// </summary>

		LayingOut,

		/// <summary>
		/// An iteration of the asynchronous layout has completed and now the
		/// m_oGraphVisualCollection needs to be populated with the graph's
		/// contents.
		/// </summary>

		LayoutIterationCompleted,

		/// <summary>
		/// The asynchronous layout has completed and now the
		/// m_oGraphVisualCollection needs to be populated with the graph's
		/// contents.
		/// </summary>

		LayoutCompleted,
	}

	//*************************************************************************
	//	Property: ClassName
	//
	/// <summary>
	/// Gets the full name of the class.
	/// </summary>
	///
	/// <value>
	/// The full name of the class, suitable for use in error messages.
	/// </value>
	//*************************************************************************

	protected String
	ClassName
	{
		get
		{
			return (this.GetType().FullName);
		}
	}

	//*************************************************************************
	//	Property: ArgumentChecker
	//
	/// <summary>
	/// Gets a new initialized <see cref="ArgumentChecker" /> object.
	/// </summary>
	///
	/// <value>
	/// A new initialized <see cref="ArgumentChecker" /> object.
	/// </value>
	///
	/// <remarks>
	/// The returned object can be used to check the validity of property
	/// values and method parameters.
	/// </remarks>
	//*************************************************************************

	internal ArgumentChecker
	ArgumentChecker
	{
		get
		{
			return ( new ArgumentChecker(this.ClassName) );
		}
	}

	//*************************************************************************
	//	Property: VisualChildrenCount
	//
	/// <summary>
	/// Gets the number of visual child elements within this element.
	/// </summary>
	///
	/// <value>
	/// The number of visual child elements for this element.
	/// </value>
	//*************************************************************************

	protected override Int32
	VisualChildrenCount
	{
		get
		{
			return (m_oGraphVisualCollection.VisualCollection.Count);
		}
	}

    //*************************************************************************
    //  Method: GetVisualChild()
    //
    /// <summary>
	/// Returns a child at the specified index from a collection of child
	/// elements. 
    /// </summary>
	///
	/// <param name="index">
	/// The zero-based index of the requested child element in the collection.
	/// </param>
	///
	/// <returns>
	/// The requested child element.
	/// </returns>
    //*************************************************************************

	protected override Visual
	GetVisualChild
	(
		Int32 index
	)
	{
		Debug.Assert(index >= 0);

        return ( m_oGraphVisualCollection.VisualCollection[index] );
	}

    //*************************************************************************
    //  Method: OnInitialized()
    //
    /// <summary>
	/// Raises the Initialized event. 
    /// </summary>
	///
	/// <param name="e">
	/// Standard event arguments.
	/// </param>
    //*************************************************************************

	protected override void
	OnInitialized
	(
		EventArgs e
	)
	{
		AssertValid();

		Debug.WriteLine("TODO: OnInitialized()");
	}

    //*************************************************************************
    //  Method: OnRenderSizeChanged()
    //
    /// <summary>
	/// Raises the SizeChanged event, using the specified information as part
	/// of the eventual event data.
    /// </summary>
	///
	/// <param name="sizeInfo">
	/// Details of the old and new size involved in the change.
	/// </param>
    //*************************************************************************

	protected override void
	OnRenderSizeChanged
	(
		SizeChangedInfo sizeInfo
	)
	{
		AssertValid();

		Debug.WriteLine("TODO: OnRenderSizeChanged()");
		Debug.WriteLine("TODO: sizeInfo.NewSize = " + sizeInfo.NewSize);

		base.OnRenderSizeChanged(sizeInfo);
	}

    //*************************************************************************
    //  Method: OnRender()
    //
    /// <summary>
	/// Renders the control.
    /// </summary>
	///
	/// <param name="drawingContext">
	/// The drawing instructions for a specific element. This context is
	/// provided to the layout system.
	/// </param>
    //*************************************************************************

	protected override void
	OnRender
	(
		DrawingContext drawingContext
	)
	{
		AssertValid();

		Debug.WriteLine("TODO: OnRender(), m_eLayoutState = " + m_eLayoutState);

		RectangleF oGraphRectangleF = new RectangleF(
			System.Drawing.PointF.Empty,
			new System.Drawing.SizeF(
				(Single)this.ActualWidth, (Single)this.ActualHeight)
			);

		switch (m_eLayoutState)
		{
			case LayoutState.Stable:

				break;

			case LayoutState.LayoutRequired:

				Debug.Assert(!m_oAsyncLayout.IsBusy);

				Rectangle oGraphRectangle =
					Rectangle.Truncate(oGraphRectangleF);

				LayoutContext oLayoutContext =
					new LayoutContext(oGraphRectangle);

				m_eLayoutState = LayoutState.LayingOut;

				m_oAsyncLayout.LayOutGraphAsync(m_oGraph, oLayoutContext);

				break;

			case LayoutState.LayingOut:

				break;

			case LayoutState.LayoutIterationCompleted:

				m_oGraphVisualCollection.Populate(oGraphRectangleF);

				m_eLayoutState = LayoutState.LayingOut;

				break;

			case LayoutState.LayoutCompleted:

				m_oGraphVisualCollection.Populate(oGraphRectangleF);

				m_eLayoutState = LayoutState.Stable;

				break;

			default:

				Debug.Assert(false);
				break;
		}
	}

    //*************************************************************************
    //  Method: AsyncLayout_LayOutGraphIterationCompleted()
    //
    /// <summary>
	/// Handles the LayOutGraphIterationCompleted event on the m_oAsyncLayout
	/// object.
    /// </summary>
	///
	/// <param name="oSender">
	/// Standard event argument.
	/// </param>
	///
	/// <param name="oEventArgs">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	protected void
    AsyncLayout_LayOutGraphIterationCompleted
	(
		Object oSender,
		EventArgs oEventArgs
	)
	{
		AssertValid();

		Debug.WriteLine("TODO: AsyncLayout_LayOutGraphIterationCompleted()");

		// An iteration of the asynchronous layout has completed and now the
		// m_oGraphVisualCollection needs to be populated with the graph's
		// contents.

		m_eLayoutState = LayoutState.LayoutIterationCompleted;
		this.InvalidateVisual();
	}

    //*************************************************************************
    //  Method: AsyncLayout_LayOutGraphCompleted()
    //
    /// <summary>
	/// Handles the LayOutGraphCompleted event on the m_oAsyncLayout object.
    /// </summary>
	///
	/// <param name="oSender">
	/// Standard event argument.
	/// </param>
	///
	/// <param name="oAsyncCompletedEventArgs">
	/// Standard event argument.
	/// </param>
    //*************************************************************************

	protected void
    AsyncLayout_LayOutGraphCompleted
	(
		Object oSender,
		AsyncCompletedEventArgs oAsyncCompletedEventArgs
	)
	{
		AssertValid();

		Debug.WriteLine("TODO: AsyncLayout_LayOutGraphCompleted()");

		if (oAsyncCompletedEventArgs.Error != null)
		{
			// TODO
		}
		else if (oAsyncCompletedEventArgs.Cancelled)
		{
			// TODO
		}
		else
		{
			// The asynchronous layout has completed and now the
			// m_oGraphVisualCollection needs to be populated with the graph's
			// contents.

			m_eLayoutState = LayoutState.LayoutCompleted;
			this.InvalidateVisual();
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
		Debug.Assert(m_oGraph != null);
		Debug.Assert(m_oGraphVisualCollection != null);
		Debug.Assert(m_oAsyncLayout != null);
		// m_eLayoutState
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The graph being drawn.

	protected IGraph m_oGraph;

	/// Contains a collection of Visual objects representing the graph's
	/// vertices and edges.

	protected GraphVisualCollection m_oGraphVisualCollection;

	/// Object used to lay out the graph.

	protected IAsyncLayout m_oAsyncLayout;

	/// Indicates the state of the graph's layout.

	protected LayoutState m_eLayoutState;
}

}
