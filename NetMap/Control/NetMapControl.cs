
//	Copyright (c) Microsoft Corporation.  All rights reserved.

// Define TRACEEVENT to write trace messages when important events occur.

// #define TRACEEVENT

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.ControlLib;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//  Class: NetMapControl
//
/// <summary>
/// NetMap graph-drawing control.
/// </summary>
///
/// <remarks>
/// <see cref="NetMapControl" /> is one of several classes provided with the
/// NetMap system that draw a graph, which is a set of vertices connected by
/// edges.
///
/// <para>
/// The following table summarizes the graph-drawing classes:
/// </para>
///
/// <list type="table">
///
/// <listheader>
/// <term>Class</term>
/// <term>For Use In</term>
/// <term>Features</term>
/// <term>Required NetMap Assemblies</term>
/// </listheader>
///
/// <item>
/// <term><see cref="GraphDrawer" /></term>
/// <term>
/// Any application that wants to draw a graph onto a <see cref="Bitmap" /> or
/// a <see cref="Graphics" /> object in a synchronous manner.
/// </term>
/// <term>
/// Can use custom layouts, vertex drawers, and edge drawers.
/// </term>
/// <term>
/// Core.dll, Visualization.dll
/// </term>
/// </item>
///
/// <item>
/// <term><see cref="AsyncGraphDrawer" /></term>
/// <term>
/// Any application that wants to draw a graph onto a <see cref="Bitmap" /> or
/// a <see cref="Graphics" /> object in a synchronous or asynchronous manner.
/// </term>
/// <term>
/// Can use custom layouts, vertex drawers, and edge drawers.
/// </term>
/// <term>
/// Core.dll, Visualization.dll
/// </term>
/// </item>
///
/// <item>
/// <term><see cref="MultiSelectionGraphDrawer" /></term>
/// <term>
/// Any application that wants to draw a graph onto a <see cref="Bitmap" /> or
/// a <see cref="Graphics" /> object in a synchronous or asynchronous manner.
/// </term>
/// <term>
/// Same as <see cref="AsyncGraphDrawer" />, plus vertices and edges can be
/// drawn as selected.
/// </term>
/// <term>
/// Core.dll, Visualization.dll
/// </term>
/// </item>
///
/// <item>
/// <term><see cref="NetMapControl" /></term>
/// <term>
/// Windows Forms applications
/// </term>
/// <term>
/// Wraps a <see cref="MultiSelectionGraphDrawer" /> in a Windows Forms
/// control.
/// </term>
/// <term>
/// Core.dll, Visualization.dll, Control.dll
/// </term>
/// </item>
///
/// </list>
///
/// <para>
/// Using <see cref="NetMapControl" /> in an application involves two steps:
/// <list type="bullet">
///
/// <item>
/// <description>
/// Set properties that determine how the graph is drawn
/// </description>
/// </item>
///
/// <item>
/// <description>
/// Populate the graph
/// </description>
/// </item>
///
/// </list>
/// </para>
///
/// <para>
/// To improve performance, call <see cref="BeginUpdate" /> before populating
/// the graph.  This prevents the graph from being immediately updated.  Call
/// <see cref="EndUpdate()" /> when you are done.
/// </para>
///
/// <para>
/// One or more vertices and their incident edges can be selected with the
/// mouse.  See the <see cref="MouseSelectionMode" /> property for details.
/// </para>
///
/// <para>
/// To programatically select and deselect vertices, use the <see
/// cref="SetVertexSelected" />, <see cref="SetEdgeSelected" />, <see
/// cref="SetSelected" />, <see cref="SelectAll" />, and <see
/// cref="DeselectAll" /> methods.  To determine which vertices and edges are
/// selected, use the <see cref="SelectedVertices" /> and <see
/// cref="SelectedEdges" /> properties.
/// </para>
///
/// <para>
/// Selected vertices and edges are drawn in colors different from unselected
/// vertices and edges.  See the <see cref="SetVertexSelected" /> and <see
/// cref="SetEdgeSelected" /> methods for details.
/// </para>
///
/// <para>
/// A tooltip can be displayed when the mouse hovers over a vertex.  See <see
/// cref="ShowToolTips" /> for details.
/// </para>
///
/// </remarks>
///
/// <example>
/// Here is sample C# code that populates a <see cref="NetMapControl" /> graph
/// with several vertices and edges.  Note that <see cref="BeginUpdate" />
/// should be called before populating the graph, and <see
/// cref="EndUpdate()" /> should be called when you are done.
///
/// <code>
/**

using System;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.NetMap.Visualization;
using Microsoft.NetMap.Core;

namespace WindowsFormsApplication1
{
public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        netMapControl1.BeginUpdate();

        // The default vertex drawer draws all vertices with the same
        // color, radius, and shape.  Replace it with one that will vary
        // the appearance of each vertex based on metadata values stored in
        // the vertices.

        netMapControl1.VertexDrawer = new PerVertexWithLabelDrawer();

        // Replace the default edge drawer with one that will vary the color
        // and width of each edge based on metadata values stored in the edges.

        netMapControl1.EdgeDrawer = new PerEdgeWithLabelDrawer();

        // Get the graph's vertex collection.

        IVertexCollection oVertices = netMapControl1.Graph.Vertices;

        // Add three vertices.

        IVertex oVertexA = oVertices.Add();
        IVertex oVertexB = oVertices.Add();
        IVertex oVertexC = oVertices.Add();

        // Change the color, radius, and shape of vertex A.

        oVertexB.SetValue(ReservedMetadataKeys.PerColor, Color.Orange);
        oVertexB.SetValue(ReservedMetadataKeys.PerVertexRadius, 20F);
        oVertexB.SetValue(ReservedMetadataKeys.PerVertexShape,
            VertexDrawer.VertexShape.Sphere);

        // Draw vertex B as a primary label instead of a shape.  A primary
        // label is a box containing text.

        oVertexA.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabel,
            "Primary Label");

        // Set the primary label's text and fill colors.

        oVertexA.SetValue(ReservedMetadataKeys.PerColor, Color.White);
        oVertexA.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabelFillColor,
            Color.Black);

        // Add a secondary label to vertex C.  A secondary label is text that
        // is drawn outside the vertex.  It can be added to a shape, image, or
        // primary label.

        oVertexC.SetValue(ReservedMetadataKeys.PerVertexSecondaryLabel,
            "Secondary Label");

        // Get the graph's edge collection.

        IEdgeCollection oEdges = netMapControl1.Graph.Edges;

        // Connect the vertices with directed edges.

        IEdge oEdge1 = oEdges.Add(oVertexA, oVertexB, true);
        IEdge oEdge2 = oEdges.Add(oVertexB, oVertexC, true);
        IEdge oEdge3 = oEdges.Add(oVertexC, oVertexA, true);

        // Customize their appearance.

        oEdge1.SetValue(ReservedMetadataKeys.PerColor, Color.Chartreuse);
        oEdge1.SetValue(ReservedMetadataKeys.PerEdgeWidth, 3);

        oEdge2.SetValue(ReservedMetadataKeys.PerEdgeWidth, 5);

        oEdge3.SetValue(ReservedMetadataKeys.PerColor, Color.ForestGreen);

        netMapControl1.EndUpdate();
    }
}
}

*/
/// </code>
///
/// </example>
//*****************************************************************************

public class NetMapControl : Panel
{
    //*************************************************************************
    //  Constructor: NetMapControl()
    //
    /// <summary>
    /// Initializes a new instance of the NetMapControl class.
    /// </summary>
    //*************************************************************************

    public NetMapControl()
    {
		m_oBitmapNoSelection = null;
		m_oBitmapWithSelection = null;

		// Create the MultiSelectionGraphDrawer that will draw the graph.

		CreateMultiSelectionGraphDrawer();

		// Create the PictureBox that will display the graph.

		CreatePictureBox();

		// Create the Timer that is used to start asynchronous drawings.

		CreateDrawAsyncTimer();

		// Create a helper object for displaying tooltips.

		CreateToolTipTracker();

		// Create the Panel used to display tooltips.

		CreateToolTipPanel();

		base.BackColor = m_oMultiSelectionGraphDrawer.BackColor;

		m_eMouseSelectionMode =
			MouseSelectionMode.SelectVertexAndIncidentEdges;

		m_bAllowVertexDrag = true;

		m_bShowToolTips = false;

		m_oVertexBeingDragged = null;

		m_oMarqueeBeingDragged = null;

		m_oLastMouseMovePoint = new Point(-1, -1);

		m_oSelectedVertices = new Dictionary<IVertex, Char>();

		m_oSelectedEdges = new Dictionary<IEdge, Char>();

		m_bInBeginUpdate = false;

		m_eDrawType = DrawType.AsyncLayoutAndDraw;

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

			return (m_oMultiSelectionGraphDrawer.Graph);
		}

        set
		{
			const String PropertyName = "Graph";

			this.ArgumentChecker.CheckPropertyNotNull(PropertyName, value);
			CheckBusy(PropertyName);

			DeselectAll();

			m_oMultiSelectionGraphDrawer.Graph = value;

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

    public new ILayout
    Layout
    {
        get
		{
			AssertValid();

			return (m_oMultiSelectionGraphDrawer.Layout);
		}

        set
		{
			const String PropertyName = "Layout";

			this.ArgumentChecker.CheckPropertyNotNull(PropertyName, value);
			CheckBusy(PropertyName);

			m_oMultiSelectionGraphDrawer.Layout = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: VertexDrawer
    //
    /// <summary>
    /// Gets or sets the object to use to draw the graph's vertices.
    /// </summary>
    ///
    /// <value>
    /// The object to use to draw the graph's vertices, as an <see
	///	cref="IVertexDrawer" />.  The default value is a <see
	/// cref="VertexDrawer" /> object.
    /// </value>
    //*************************************************************************

	[System.ComponentModel.Browsable(false)]
	[System.ComponentModel.ReadOnly(true)]

    public IVertexDrawer
    VertexDrawer
	{
        get
		{
			AssertValid();

			return (m_oMultiSelectionGraphDrawer.VertexDrawer);
		}

        set
		{
			const String PropertyName = "VertexDrawer";

			this.ArgumentChecker.CheckPropertyNotNull(PropertyName, value);
			CheckBusy(PropertyName);

			m_oMultiSelectionGraphDrawer.VertexDrawer = value;

			AssertValid();
		}
	}

    //*************************************************************************
    //  Property: EdgeDrawer
    //
    /// <summary>
    /// Gets or sets the object to use to draw the graph's edges.
    /// </summary>
    ///
    /// <value>
    /// The object to use to draw the graph's edges, as an <see
	///	cref="IEdgeDrawer" />.  The default value is an <see
	/// cref="EdgeDrawer" /> object.
    /// </value>
    //*************************************************************************

	[System.ComponentModel.Browsable(false)]
	[System.ComponentModel.ReadOnly(true)]

    public IEdgeDrawer
    EdgeDrawer
    {
        get
		{
			AssertValid();

			return (m_oMultiSelectionGraphDrawer.EdgeDrawer);
		}

        set
		{
			const String PropertyName = "EdgeDrawer";

			this.ArgumentChecker.CheckPropertyNotNull(PropertyName, value);
			CheckBusy(PropertyName);

			m_oMultiSelectionGraphDrawer.EdgeDrawer = value;

			AssertValid();
		}
    }

	//*************************************************************************
	//	Property: BackColor
	//
	/// <summary>
	/// Gets or sets the graph's background color.
	/// </summary>
	///
	/// <value>
	/// The graph's background color, as a <see cref="Color" />.  The default
	/// value is SystemColors.<see cref="SystemColors.Window" />.
	/// </value>
	//*************************************************************************

	public new Color
	BackColor
	{
		get
		{
			AssertValid();

			return (m_oMultiSelectionGraphDrawer.BackColor);
		}

		set
		{
			const String PropertyName = "BackColor";

			CheckBusy(PropertyName);

			m_oMultiSelectionGraphDrawer.BackColor = value;

			base.BackColor = value;

			m_oPictureBox.BackColor = value;

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
    /// A <see cref="Microsoft.NetMap.Visualization.MouseSelectionMode" />
	/// value.  The default value is <see cref="Microsoft.NetMap.
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

			return (m_eMouseSelectionMode);
		}

        set
		{
			m_eMouseSelectionMode = value;

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

			return (m_bAllowVertexDrag);
		}

        set
		{
			m_bAllowVertexDrag = value;

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

			return (m_bShowToolTips);
		}

		set
		{
			const String PropertyName = "ShowToolTips";

			CheckBusy(PropertyName);

			if (!value)
			{
				// Hide any tooltip that might be showing and reset the helper
				// object that figures out when to show tooltips.

				ResetToolTipTracker();
			}

			m_bShowToolTips = value;

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

			const String PropertyName = "SelectedVertices";

			CheckBusy(PropertyName);

			return ( CollectionUtil.DictionaryKeysToArray<IVertex, Char>(
				m_oSelectedVertices) );
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

			const String PropertyName = "SelectedEdges";

			CheckBusy(PropertyName);

			return ( CollectionUtil.DictionaryKeysToArray<IEdge, Char>(
				m_oSelectedEdges) );
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

			return (m_oMultiSelectionGraphDrawer.IsBusy);
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

		const String MethodName = "BeginUpdate";

		CheckBusy(MethodName);

		m_bInBeginUpdate = true;
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

		// Assume that changes have been made to the graph that require it to
		// be layed out and redrawn.

		EndUpdate(true);
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

		const String MethodName = "SetVertexSelected";

        this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "vertex", vertex);

		CheckBusy(MethodName);

		// Update the selected state of the vertex and its incident edges.

		SetVertexSelectedState(vertex, selected);

		if (alsoIncidentEdges)
		{
			foreach (IEdge oEdge in vertex.IncidentEdges)
			{
				SetEdgeSelectedState(oEdge, selected);
			}
		}

		// Stop if the graph hasn't been drawn yet.

		if (m_oBitmapNoSelection == null) 
		{
			return;
		}

		// Copy the "no selection" bitmap to the "with selection" bitmap,
		// redraw the selected vertices and edges, and update the PictureBox.

		UpdateBitmapWithSelection();

		// Fire a SelectionChanged event if appropriate.

		FireSelectionChanged();
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

		const String MethodName = "SetEdgeSelected";

        this.ArgumentChecker.CheckArgumentNotNull(MethodName, "edge", edge);
		CheckBusy(MethodName);

		// Update the selected state of the edge and its adjacent vertices.

		SetEdgeSelectedState(edge, selected);

		if (alsoAdjacentVertices)
		{
			SetVertexSelectedState(edge.Vertices[0], selected);
			SetVertexSelectedState(edge.Vertices[1], selected);
		}

		// Stop if the graph hasn't been drawn yet.

		if (m_oBitmapNoSelection == null) 
		{
			return;
		}

		// Copy the "no selection" bitmap to the "with selection" bitmap,
		// redraw the selected vertices and edges, and update the PictureBox.

		UpdateBitmapWithSelection();

		// Fire a SelectionChanged event if appropriate.

		FireSelectionChanged();
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

		const String MethodName = "SetSelected";

        this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "vertices", vertices);

        this.ArgumentChecker.CheckArgumentNotNull(MethodName, "edges", edges);

		CheckBusy(MethodName);

		// Clear the selection.

		SetVertexSelectedStateAll(false);
		SetEdgeSelectedStateAll(false);

		// Update the selected state of the specified vertices and edges.

		foreach (IVertex oVertex in vertices)
		{
			SetVertexSelectedState(oVertex, true);
		}

		foreach (IEdge oEdge in edges)
		{
			SetEdgeSelectedState(oEdge, true);
		}

		// Stop if the graph hasn't been drawn yet.

		if (m_oBitmapNoSelection == null) 
		{
			return;
		}

		// Copy the "no selection" bitmap to the "with selection" bitmap,
		// redraw the selected vertices and edges, and update the PictureBox.

		UpdateBitmapWithSelection();

		// Fire a SelectionChanged event if appropriate.

		FireSelectionChanged();
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

		const String MethodName = "SelectAll";

		CheckBusy(MethodName);

		SelectOrDeselectAll(true);
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

		const String MethodName = "DeselectAll";

		CheckBusy(MethodName);

		// Do nothing if nothing is selected.

		if (m_oSelectedVertices.Count == 0 && m_oSelectedEdges.Count == 0)
		{
			return;
		}

		SelectOrDeselectAll(false);
	}

	//*************************************************************************
	//	Method: GetVertexFromPoint()
	//
	/// <overloads>
	/// Gets the vertex containing a specified point.
	/// </overloads>
	///
	/// <summary>
	/// Gets the vertex containing a specified <see cref="Point" />.
	/// </summary>
	///
	/// <param name="point">
	/// Point to get a vertex for.
	/// </param>
	///
	/// <param name="vertex">
	/// Where the <see cref="IVertex" /> object gets stored.
	/// </param>
	///
	/// <returns>
	///	true if a vertex containing the point was found, false if not.
	/// </returns>
	///
	/// <remarks>
	/// This method looks for a vertex that contains <paramref name="point" />.
	/// If there is such a vertex, the vertex is stored at <paramref
	///	name="vertex" /> and true is returned.  Otherwise, <paramref
	/// name="vertex" /> is set to null and false is returned.
	///
	///	<para>
	/// If the graph hasn't been drawn yet, false is returned.
	///	</para>
	///
	/// <para>
	///	The <see cref="IVertexDrawer.VertexContainsPoint" /> method on the <see
	///	cref="VertexDrawer" /> object is used to determine which vertex, if
	///	any, contains the point.
	/// </para>
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is in progress.
	/// Check <see cref="IsDrawing" /> before calling this method.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public Boolean
	GetVertexFromPoint
	(
		Point point,
		out IVertex vertex
	)
	{
		AssertValid();

		const String MethodName = "GetVertexFromPoint";

		CheckBusy(MethodName);

		return ( m_oMultiSelectionGraphDrawer.GetVertexFromPoint(
			point, out vertex) );
	}

	//*************************************************************************
	//	Method: GetVertexFromPoint()
	//
	/// <summary>
	/// Gets the vertex containing a specified coordinate pair.
	/// </summary>
	///
	/// <param name="x">
	/// X-coordinate of the point to get a vertex for.
	/// </param>
	///
	/// <param name="y">
	/// Y-coordinate of the point to get a vertex for.
	/// </param>
	///
	/// <param name="vertex">
	/// Where the <see cref="IVertex" /> object gets stored.
	/// </param>
	///
	/// <returns>
	///	true if a vertex containing the point was found, false if not.
	/// </returns>
	///
	/// <remarks>
	/// This method looks for a vertex that contains the point
	/// (<paramref name="x" />, <paramref name="y" />).  If there is such a
	/// vertex, the vertex is stored at <paramref name="vertex" /> and true is
	///	returned.  Otherwise, <paramref name="vertex" /> is set to null and
	///	false is returned.
	///
	///	<para>
	/// If the graph hasn't been drawn yet, false is returned.
	///	</para>
	///
	/// <para>
	///	The <see cref="IVertexDrawer.VertexContainsPoint" /> method on the <see
	///	cref="VertexDrawer" /> object is used to determine which vertex, if
	///	any, contains the point.
	/// </para>
	///
	/// <para>
	/// An exception is thrown if an asynchronous drawing is in progress.
	/// Check <see cref="IsDrawing" /> before calling this method.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public Boolean
	GetVertexFromPoint
	(
		Int32 x,
		Int32 y,
		out IVertex vertex
	)
	{
		AssertValid();

		const String MethodName = "GetVertexFromPoint";

		CheckBusy(MethodName);

		return ( GetVertexFromPoint(new Point(x, y), out vertex) );
	}

	//*************************************************************************
	//	Method: CopyBitmap()
	//
	/// <summary>
	/// Gets a copy of the <see cref="Bitmap" /> displayed within the control.
	/// </summary>
	///
	/// <returns>
	/// A copy of the <see cref="Bitmap" /> displayed within the control.  The
	/// bitmap is the size of the control's client area.
	/// </returns>
	///
	/// <remarks>
	/// If the graph hasn't been drawn yet, a <see cref="Bitmap" /> filled with
	/// white is returned.
	/// </remarks>
	//*************************************************************************

	public Bitmap
	CopyBitmap()
	{
		AssertValid();

		const String MethodName = "CopyBitmap";

		CheckBusy(MethodName);

		Image oImage = m_oPictureBox.Image;

		Bitmap oBitmapCopy = null;

		if (oImage == null)
		{
			Size oSize = m_oPictureBox.ClientSize;

			oBitmapCopy = new Bitmap(oSize.Width, oSize.Height);

			Graphics oGraphics = Graphics.FromImage(oBitmapCopy);

			oGraphics.Clear(Color.White);

			GraphicsUtil.DisposeGraphics(ref oGraphics);
		}
		else
		{
			Debug.Assert(oImage is Bitmap);

			oBitmapCopy = (Bitmap)( (Bitmap)oImage ).Clone();
		}

		return (oBitmapCopy);
	}

	//*************************************************************************
	//	Method: ForceLayout()
	//
	/// <summary>
	/// Forces the graph to be laid out again.
	/// </summary>
	///
	/// <remarks>
	/// To improve performance, resizing the control typically does not lay out
	/// the graph again.  Instead, the current layout is just be transformed to
	/// fit the new control rectangle.  Use this method to force the graph to
	/// be laid out again.
	///
	/// <para>
	/// If you want to redraw the graph without laying it out again, use <see
	/// cref="ForceRedraw" />.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public void
	ForceLayout()
	{
		AssertValid();

		const String MethodName = "ForceLayout";

		CheckBusy(MethodName);

		this.BeginUpdate();
		this.EndUpdate(true);
	}

	//*************************************************************************
	//	Method: ForceRedraw()
	//
	/// <summary>
	/// Forces the graph to be drawn again.
	/// </summary>
	///
	/// <remarks>
	/// This method does not lay out the graph again.  If you want to lay out
	/// the graph before redrawing it, use <see cref="ForceLayout" />.
	/// </remarks>
	//*************************************************************************

	public void
	ForceRedraw()
	{
		AssertValid();

		const String MethodName = "ForceRedraw";

		CheckBusy(MethodName);

		this.BeginUpdate();
		this.EndUpdate(false);
	}

	//*************************************************************************
	//	Delegate: GraphMouseEventHandler
	//
	/// <summary>
	///	Represents a method that will handle the <see cref="GraphMouseDown" />
	///	or <see cref="GraphMouseUp" /> event fired by the <see
	///	cref="NetMapControl" />.
	/// </summary>
	///
	/// <param name="sender">
	/// The <see cref="NetMapControl" /> that fired the event.
	/// </param>
	///
	/// <param name="e">
	/// Provides information about the mouse and the part of the graph that was
	/// clicked.
	/// </param>
	//*************************************************************************

	public delegate void
	GraphMouseEventHandler
	(
		Object sender,
		GraphMouseEventArgs e
	);


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
	//  Enum: DrawType
	//
	/// <summary>
	/// Specifies the type of draw operation performed by <see
	/// cref="StartDraw" />.
	/// </summary>
	//*************************************************************************

	protected enum
	DrawType
	{
		/// <summary>
		/// Synchronously draw the graph only.  Do not modify the layout.
		/// </summary>

		DrawOnly,

		/// <summary>
		/// Synchronously transform the previous layout and draw the graph.
		/// </summary>

		TransformLayoutAndDraw,

		/// <summary>
		/// Asynchronously lay out and draw the graph.
		/// </summary>

		AsyncLayoutAndDraw,
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
	//	Method: CheckBusy()
	//
	/// <summary>
	/// Throws an exception if a drawing is in progress.
	/// </summary>
	///
	/// <param name="sMethodOrPropertyName">
	/// Name of the method or property calling this method.
	/// </param>
	//*************************************************************************

	protected void
	CheckBusy
	(
		String sMethodOrPropertyName
	)
	{
		Debug.Assert( !String.IsNullOrEmpty(sMethodOrPropertyName) );
		AssertValid();

		if (this.IsDrawing)
		{
			throw new InvalidOperationException(String.Format(

				"{0}.{1}: An asynchronous drawing is in progress.  Check the"
				+ " IsBusy property before calling this."
				,
				this.ClassName,
				sMethodOrPropertyName
				) );
		}
	}

    //*************************************************************************
    //  Method: CreateMultiSelectionGraphDrawer()
    //
    /// <summary>
	/// Creates the MultiSelectionGraphDrawer that will draw the graph.
    /// </summary>
    //*************************************************************************

    protected void
	CreateMultiSelectionGraphDrawer()
    {
		m_oMultiSelectionGraphDrawer = new MultiSelectionGraphDrawer();

		m_oMultiSelectionGraphDrawer.DrawAsyncIterationCompleted +=
			new EventHandler(
				this.MultiSelectionGraphDrawer_DrawAsyncIterationCompleted);

		m_oMultiSelectionGraphDrawer.DrawAsyncCompleted +=
			new AsyncCompletedEventHandler(
				this.MultiSelectionGraphDrawer_DrawAsyncCompleted);

		m_oMultiSelectionGraphDrawer.LayoutRequired +=
			new EventHandler(this.MultiSelectionGraphDrawer_LayoutRequired);

		m_oMultiSelectionGraphDrawer.RedrawRequired +=
			new EventHandler(this.MultiSelectionGraphDrawer_RedrawRequired);
    }

	//*************************************************************************
	//	Method: CreatePictureBox()
	//
	/// <summary>
	/// Creates the PictureBox that will display the graph.
	/// </summary>
	//*************************************************************************

	protected void
	CreatePictureBox()
	{
		m_oPictureBox = new PictureBox();

		m_oPictureBox.Name = "m_oPictureBox";

		Debug.Assert(m_oMultiSelectionGraphDrawer != null);

		m_oPictureBox.BackColor = m_oMultiSelectionGraphDrawer.BackColor;

		m_oPictureBox.MouseUp +=
			new MouseEventHandler(this.m_oPictureBox_MouseUp);

		m_oPictureBox.DoubleClick +=
			new EventHandler(this.m_oPictureBox_DoubleClick);

		m_oPictureBox.MouseMove +=
			new MouseEventHandler(this.m_oPictureBox_MouseMove);

		m_oPictureBox.MouseDown +=
			new MouseEventHandler(this.m_oPictureBox_MouseDown);

		m_oPictureBox.MouseLeave +=
			new EventHandler(this.m_oPictureBox_MouseLeave);

		this.Controls.Add(m_oPictureBox);
	}

    //*************************************************************************
    //  Method: CreateToolTipTracker()
    //
    /// <summary>
	/// Creates a helper object for displaying tooltips and registers event
	/// handlers with it.
    /// </summary>
    //*************************************************************************

    protected void
	CreateToolTipTracker()
    {
		m_oToolTipTracker = new ToolTipTracker();

		m_oToolTipTracker.ShowToolTip +=
			new ToolTipTracker.ToolTipTrackerEvent(
				this.oToolTipTracker_ShowToolTip);

		m_oToolTipTracker.HideToolTip +=
			new ToolTipTracker.ToolTipTrackerEvent(
				this.oToolTipTracker_HideToolTip);
    }

    //*************************************************************************
    //  Method: CreateToolTipPanel()
    //
    /// <summary>
	/// Creates the Panel used to display tooltips.
    /// </summary>
    //*************************************************************************

    protected void
	CreateToolTipPanel()
    {
		m_oToolTipPanel = new ToolTipPanel();

		m_oToolTipPanel.BackColor = SystemColors.Window;
		m_oToolTipPanel.BorderStyle = BorderStyle.FixedSingle;
		m_oToolTipPanel.Name = "m_oToolTipPanel";

		Controls.Add(m_oToolTipPanel);

		m_oToolTipPanel.BringToFront();
    }

	//*************************************************************************
	//	Method: CreateDrawAsyncTimer()
	//
	/// <summary>
	/// Creates the Timer that is used to start asynchronous drawings.
	/// </summary>
	//*************************************************************************

    protected void
	CreateDrawAsyncTimer()
    {
		m_oDrawAsyncTimer = new Timer();

		m_oDrawAsyncTimer.Tick += new EventHandler(this.DrawAsyncTimer_Tick);

		m_oDrawAsyncTimer.Interval = DrawAsyncTimerIntervalMs;
    }

	//*************************************************************************
	//	Method: EndUpdate()
	//
	/// <summary>
	/// Enables the redrawing of the graph.
	/// </summary>
	///
	/// <param name="bLayOutAgain">
	/// true to lay out the graph again and redraw it, false to just redraw it.
	/// </param>
	//*************************************************************************

	protected void
	EndUpdate
	(
		Boolean bLayOutAgain
	)
	{
		AssertValid();

		const String MethodName = "EndUpdate";

		CheckBusy(MethodName);

		m_bInBeginUpdate = false;

		TraceEvent(MethodName, "Calling StartOrScheduleDraw.");

		// Start or schedule a drawing operation.

		StartOrScheduleDraw(bLayOutAgain ?
			DrawType.AsyncLayoutAndDraw : DrawType.DrawOnly);
	}

	//*************************************************************************
	//	Method: TryGetDrawingSize()
	//
	/// <summary>
	/// Attempts to get the size to use for a graph bitmap, taking autoscroll
	/// settings into consideration.
	/// </summary>
	///
	/// <param name="oSize">
	/// Where the size gets stored if true is returned.
	/// </param>
	///
	/// <returns>
	///	true if a non-zero size was obtained, false if the client window is too
	/// small to display a bitmap.
	/// </returns>
	//*************************************************************************

	protected Boolean
	TryGetDrawingSize
	(
		out Size oSize
	)
	{
		// Temporarily turn off the scrollbars to get the full size of the
		// client window.

		Boolean bOldAutoScroll = this.AutoScroll;
		this.AutoScroll = false;
		Size oClientSize = this.ClientSize;
		this.AutoScroll = bOldAutoScroll;

		if (!this.AutoScroll)
		{
			// The bitmap should be the same size as the client window.

			oSize = oClientSize;
		}
		else
		{
			// The bitmap should be the size of the scrolling area.

			oSize = this.AutoScrollMinSize;
		}

		return (oSize.Width > 0 && oSize.Height > 0);
	}

	//*************************************************************************
	//	Method: CreateBitmap()
	//
	/// <summary>
	///	Creates a Bitmap of a specified size.
	/// </summary>
	///
	/// <param name="oSize">
	/// Size of the Bitmap to create.
	/// </param>
	///
	/// <returns>
	///	A new Bitmap.
	/// </returns>
	//*************************************************************************

	protected Bitmap
	CreateBitmap
	(
		Size oSize
	)
	{
		AssertValid();

		const String MethodName = "CreateBitmap";

		try
		{
			return ( new Bitmap(oSize.Width, oSize.Height) );
		}
		catch (System.ArgumentException oArgumentException)
		{
			// The exception that Bitmap() throws when the size is too large
			// isn't very helpful.  Rethrow something more informative.

			throw new ArgumentException(String.Format(

				"{0}.{1}: The graph bitmap could not be created.  It may be"
				+ " too large. (Its size is {2}.)"
				,
				this.ClassName,
				MethodName,
				oSize
				),

				oArgumentException
				);
		}
	}

	//*************************************************************************
	//	Method: DrawWaitMessage()
	//
	/// <summary>
	/// Draws a string on a bitmap telling the user to wait while the graph is
	/// being laid out.
	/// </summary>
	///
	/// <param name="oBitmap">
	/// Bitmap to draw onto.
	/// </param>
	//*************************************************************************

	protected void
	DrawWaitMessage
	(
		Bitmap oBitmap
	)
	{
		Debug.Assert(oBitmap != null);
		AssertValid();

		// To avoid flashing, use the control's background color as the bitmap
		// color.

		Graphics oGraphics = Graphics.FromImage(oBitmap);

		Color oBackColor = this.BackColor;

		oGraphics.Clear(oBackColor);

		Font oFont = new Font(this.Font.FontFamily, WaitMessageFontSize);

		Brush oBrush = new SolidBrush( GetBackgroundContrastColor() );

		oGraphics.DrawString(WaitMessage, oFont, oBrush,
			WaitMessageX, WaitMessageY);

		oFont.Dispose();

        GraphicsUtil.DisposeBrush(ref oBrush);
		GraphicsUtil.DisposeGraphics(ref oGraphics);
	}

	//*************************************************************************
	//	Method: ResetToolTipTracker()
	//
	/// <summary>
	///	Hides any tooltip that might be showing and resets the helper object
	/// that figures out when to show tooltips.
	/// </summary>
	//*************************************************************************

	protected void
	ResetToolTipTracker()
	{
		AssertValid();

		m_oToolTipPanel.HideToolTip();
		m_oToolTipTracker.Reset();
		m_oLastMouseMovePoint = new Point(-1, -1);
	}

	//*************************************************************************
	//	Method: FireSelectionChanged()
	//
	/// <summary>
	///	Fires the <see cref="SelectionChanged" /> event if appropriate.
	/// </summary>
	//*************************************************************************

	protected void
	FireSelectionChanged()
	{
		AssertValid();

		EventUtil.FireEvent(this, this.SelectionChanged);
	}

	//*************************************************************************
	//	Method: FireGraphMouseDown()
	//
	/// <summary>
	///	Fires the <see cref="GraphMouseDown" /> event if appropriate.
	/// </summary>
	///
	/// <param name="oMouseEventArgs">
	/// Standard mouse event arguments.
	/// </param>
	///
	/// <param name="oVertex">
	/// Clicked vertex if the user clicked on a vertex, or null if he didn't.
	/// </param>
	//*************************************************************************

	protected void
	FireGraphMouseDown
	(
		MouseEventArgs oMouseEventArgs,
		IVertex oVertex
	)
	{
		Debug.Assert(oMouseEventArgs != null);
		AssertValid();

		FireGraphMouseEvent(this.GraphMouseDown, oMouseEventArgs, oVertex);
	}

	//*************************************************************************
	//	Method: FireGraphMouseUp()
	//
	/// <summary>
	///	Fires the <see cref="GraphMouseUp" /> event if appropriate.
	/// </summary>
	///
	/// <param name="oMouseEventArgs">
	/// Standard mouse event arguments.
	/// </param>
	///
	/// <param name="oVertex">
	/// Clicked vertex if the user clicked on a vertex, or null if he didn't.
	/// </param>
	//*************************************************************************

	protected void
	FireGraphMouseUp
	(
		MouseEventArgs oMouseEventArgs,
		IVertex oVertex
	)
	{
		Debug.Assert(oMouseEventArgs != null);
		AssertValid();

		FireGraphMouseEvent(this.GraphMouseUp, oMouseEventArgs, oVertex);
	}

	//*************************************************************************
	//	Method: FireVertexClick()
	//
	/// <summary>
	///	Fires the <see cref="VertexClick" /> event if appropriate.
	/// </summary>
	///
	/// <param name="oVertex">
	/// Clicked vertex.  Can't be null.
	/// </param>
	//*************************************************************************

	protected void
	FireVertexClick
	(
		IVertex oVertex
	)
	{
		Debug.Assert(oVertex != null);
		AssertValid();

		FireVertexEvent(this.VertexClick, oVertex);
	}

	//*************************************************************************
	//	Method: FireVertexDoubleClick()
	//
	/// <summary>
	///	Fires the <see cref="VertexDoubleClick" /> event if appropriate.
	/// </summary>
	///
	/// <param name="oVertex">
	/// Double-clicked vertex.  Can't be null.
	/// </param>
	//*************************************************************************

	protected void
	FireVertexDoubleClick
	(
		IVertex oVertex
	)
	{
		Debug.Assert(oVertex != null);
		AssertValid();

		FireVertexEvent(this.VertexDoubleClick, oVertex);
	}

	//*************************************************************************
	//	Method: FireVertexMouseHover()
	//
	/// <summary>
	///	Fires the <see cref="VertexMouseHover" /> event if appropriate.
	/// </summary>
	///
	/// <param name="oVertex">
	/// Hovered vertex.  Can't be null.
	/// </param>
	//*************************************************************************

	protected void
	FireVertexMouseHover
	(
		IVertex oVertex
	)
	{
		Debug.Assert(oVertex != null);
		AssertValid();

		FireVertexEvent(this.VertexMouseHover, oVertex);
	}

	//*************************************************************************
	//	Method: FireVertexMoved()
	//
	/// <summary>
	///	Fires the <see cref="VertexMoved" /> event if appropriate.
	/// </summary>
	///
	/// <param name="oVertex">
	/// Moved vertex.  Can't be null.
	/// </param>
	//*************************************************************************

	protected void
	FireVertexMoved
	(
		IVertex oVertex
	)
	{
		Debug.Assert(oVertex != null);
		AssertValid();

		FireVertexEvent(this.VertexMoved, oVertex);
	}

	//*************************************************************************
	//	Method: FireVertexEvent()
	//
	/// <summary>
	///	Fires an event with the signature VertexEventHandler.
	/// </summary>
	///
	/// <param name="oVertexEventHandler">
	/// Event handler, or null if the event isn't being handled.
	/// </param>
	///
	/// <param name="oVertex">
	/// Vertex associated with the event.  Can't be null.
	/// </param>
	//*************************************************************************

	protected void
	FireVertexEvent
	(
		VertexEventHandler oVertexEventHandler,
		IVertex oVertex
	)
	{
		Debug.Assert(oVertex != null);
		AssertValid();

		if (oVertexEventHandler != null)
		{
			oVertexEventHandler( this, new VertexEventArgs(oVertex) );
		}
	}

	//*************************************************************************
	//	Method: FireGraphMouseEvent()
	//
	/// <summary>
	///	Fires an event with the signature GroupMouseEventHandler.
	/// </summary>
	///
	/// <param name="oGraphMouseEventHandler">
	/// Event handler, or null if the event isn't being handled.
	/// </param>
	///
	/// <param name="oMouseEventArgs">
	/// Standard mouse event arguments.
	/// </param>
	///
	/// <param name="oVertex">
	/// Vertex associated with the event.  Can be null.
	/// </param>
	//*************************************************************************

	protected void
	FireGraphMouseEvent
	(
		GraphMouseEventHandler oGraphMouseEventHandler,
		MouseEventArgs oMouseEventArgs,
		IVertex oVertex
	)
	{
		Debug.Assert(oMouseEventArgs != null);
		AssertValid();

		if (oGraphMouseEventHandler != null)
		{
			oGraphMouseEventHandler(
				this, new GraphMouseEventArgs(oMouseEventArgs, oVertex) );
		}
	}

	//*************************************************************************
	//	Method: FireDrawingGraph()
	//
	/// <summary>
	///	Fires the <see cref="DrawingGraph" /> event if appropriate.
	/// </summary>
	//*************************************************************************

	protected void
	FireDrawingGraph()
	{
		AssertValid();

		EventUtil.FireEvent(this, this.DrawingGraph);
	}

	//*************************************************************************
	//	Method: FireGraphDrawn()
	//
	/// <summary>
	///	Fires the <see cref="GraphDrawn" /> event if appropriate.
	/// </summary>
	//*************************************************************************

	protected void
	FireGraphDrawn()
	{
		AssertValid();

		EventUtil.FireEvent(this, this.GraphDrawn);
	}

	//*************************************************************************
	//	Method: StartOrScheduleDraw()
	//
	/// <summary>
    /// Starts or schedules a drawing operation.
	/// </summary>
	///
	/// <param name="eDrawType">
	/// Specifies the type of draw operation.
	/// </param>
	//*************************************************************************

	protected void
	StartOrScheduleDraw
	(
		DrawType eDrawType
	)
	{
		AssertValid();

		if (m_oMultiSelectionGraphDrawer.IsBusy)
		{
			ScheduleDraw(eDrawType);
		}
		else
		{
			StartDraw(eDrawType);
		}
	}

	//*************************************************************************
	//	Method: StartDraw()
	//
	/// <summary>
	///	Starts an asynchronous draw operation.
	/// </summary>
	///
	/// <param name="eDrawType">
	/// Specifies the type of draw operation.
	/// </param>
	///
	/// <remarks>
	///	Do not call this if the MultiSelectionGraphDrawer.IsBusy flag is true.
	/// </remarks>
	//*************************************************************************

	protected void
	StartDraw
	(
		DrawType eDrawType
	)
	{
		AssertValid();

		const String MethodName = "StartDraw";

		TraceEvent(MethodName, "Enter.");

		if (m_oMultiSelectionGraphDrawer.IsBusy)
		{
			// This should never occur.

			Debug.Assert(false);

			throw new InvalidOperationException(String.Format(

				"{0}.{1}: MultiSelectionGraphDrawer is busy."
				,
				this.ClassName,
				MethodName
				) );
		}

		// Hide any tooltip that might be showing and reset the helper object
		// that figures out when to show tooltips.

		ResetToolTipTracker();

		m_oDrawAsyncTimer.Enabled = false;

		// Get the size to use for the bitmaps, taking autoscroll settings into
		// consideration.

		Size oBitmapSizeToDraw;

		if ( !TryGetDrawingSize(out oBitmapSizeToDraw) )
		{
			// The client size is too small.  Don't do anything.

			return;
		}

		m_oPictureBox.Image = null;

		// Dispose of the existing bitmaps, if they exist.

		GraphicsUtil.DisposeBitmap(ref m_oBitmapNoSelection);
		GraphicsUtil.DisposeBitmap(ref m_oBitmapWithSelection);

		// Create a new "no selection" bitmap.

		m_oBitmapNoSelection = CreateBitmap(oBitmapSizeToDraw);

		// Draw a wait message on the bitmap.

		DrawWaitMessage(m_oBitmapNoSelection);

		// Display the bitmap in the PictureBox.

		m_oPictureBox.Image = m_oBitmapNoSelection;
		m_oPictureBox.Size = oBitmapSizeToDraw;
		m_oPictureBox.Location = new Point(0);

		IGraph oGraph = m_oMultiSelectionGraphDrawer.Graph;

		Object oGraphRectangleAsObject = null;

		if (eDrawType == DrawType.TransformLayoutAndDraw)
		{
			// After a graph is laid out, LayoutBase adds a key to the graph
			// to mark it as such.  The key's value is the graph rectangle that
			// was used to lay out the graph.  Look for the key.

			if ( !oGraph.TryGetValue(
					ReservedMetadataKeys.LayoutBaseLayoutComplete,
					typeof(Rectangle), out oGraphRectangleAsObject)
				)
			{
				// The key wasn't found.  Revert to a full asynchronous layout
				// and draw.

				eDrawType = DrawType.AsyncLayoutAndDraw;
			}
		}

		if (eDrawType == DrawType.AsyncLayoutAndDraw)
		{
			// Start an asynchronous draw operation, which will draw
			// iteratively onto the bitmap.  The DrawAsyncIterationCompleted
			// and DrawAsyncCompleted events on the MultiSelectionGraphDrawer
			// will be used to invalidate the PictureBox on each iteration.

			FireDrawingGraph();

			#if true

			// TODO:
			//
			// AsyncLayoutBase uses SynchronizationContext.Current to fire
			// events across thread boundaries.  In standard Windows Forms
			// applications, this works fine.  When NetMapControl is used within
			// Excel 2007 via VSTO 2005 SE, however, Synchronization.Current is
			// sometimes null, and other times it is a
			// System.Threading.SynchronizationContext instead of a
			// WindowsFormsSynchronizationContext.  In the latter case, an
			// exception is thrown when the background worker attempts to
			// communicate with NetMapControl on the wrong thread.  Why does
			// SynchronizationContext.Current behave like this?
			//
			// To work around this problem, create a new object if necessary.

            System.Threading.SynchronizationContext
				oOldSynchronizationContext = 
				System.Threading.SynchronizationContext.Current;

            if (
				oOldSynchronizationContext == null
				||
				!(oOldSynchronizationContext is
					WindowsFormsSynchronizationContext)
				)
            {
                System.Threading.SynchronizationContext.
					SetSynchronizationContext(
						new WindowsFormsSynchronizationContext()
						);
            }

			#endif


			m_oMultiSelectionGraphDrawer.DrawAsync(m_oBitmapNoSelection, false);


			#if true

			// TODO

			if (oOldSynchronizationContext != 
                System.Threading.SynchronizationContext.Current)
			{
				// Restore the previous object.

                System.Threading.SynchronizationContext.
					SetSynchronizationContext(oOldSynchronizationContext);
						
			}

			#endif

			return;
		}

		Rectangle oNewGraphRectangle = new Rectangle(
			Point.Empty, oBitmapSizeToDraw
			);

		if (eDrawType == DrawType.TransformLayoutAndDraw)
		{
			Debug.Assert(oGraphRectangleAsObject != null);

			// The original graph rectangle was found.  Instead of laying out
			// the graph again, tell the ILayout implementation to transform
			// the original layout to a new layout within the resized
			// rectangle.

			FireDrawingGraph();

			Rectangle oOriginalGraphRectangle =
				(Rectangle)oGraphRectangleAsObject;

			LayoutContext oOriginalLayoutContext =
				new LayoutContext(oOriginalGraphRectangle,
					m_oMultiSelectionGraphDrawer
					);

			LayoutContext oNewLayoutContext =
				new LayoutContext(oNewGraphRectangle,
					m_oMultiSelectionGraphDrawer
					);

			m_oMultiSelectionGraphDrawer.Layout.TransformLayout(
				oGraph, oOriginalLayoutContext, oNewLayoutContext
				);

			// Replace the layout rectangle in the graph's key.

			oGraph.SetValue(ReservedMetadataKeys.LayoutBaseLayoutComplete,
				oNewGraphRectangle);

			// Fall through and synchronously draw the transformed layout onto
			// the bitmap.
		}

		// Synchronously draw the "no selection" layout onto the bitmap.

		Graphics oGraphics = Graphics.FromImage(m_oBitmapNoSelection);

		m_oMultiSelectionGraphDrawer.DrawCurrentLayout(
			oGraphics, oNewGraphRectangle);

		GraphicsUtil.DisposeGraphics(ref oGraphics);

		OnBitmapNoSelectionDrawn();
	}

    //*************************************************************************
    //  Method: ScheduleDraw()
    //
    /// <summary>
    /// Schedules a drawing operation.
    /// </summary>
	///
	/// <param name="eDrawType">
	/// Specifies the type of draw operation.
	/// </param>
    //*************************************************************************

	protected void
	ScheduleDraw
	(
		DrawType eDrawType
	)
	{
		AssertValid();

		const String MethodName = "ScheduleDraw";

		TraceEvent(MethodName, "Enter, calling DrawAsyncCancel.");

		// In case an asynchronous drawing is progress, cancel it.  (If no
		// asynchronous drawing is in progress, cancelling does nothing.)

		m_oMultiSelectionGraphDrawer.DrawAsyncCancel();

		// The cancellation won't be immediate, so start a timer and check
		// the MultiSelectionGraphDrawer.IsBusy flag in the timer's Tick
		// handler.

		TraceEvent(MethodName, "Starting timer.");

		m_eDrawType = eDrawType;

		m_oDrawAsyncTimer.Enabled = true;
	}

	//*************************************************************************
	//	Method: UpdateBitmapWithSelection()
	//
	/// <summary>
	///	Copies the "no selection" bitmap to the "with selection" bitmap,
	/// redraws the selected vertices and edges, and updates the PictureBox.
	/// </summary>
	//*************************************************************************

	protected void
	UpdateBitmapWithSelection()
	{
		AssertValid();

		// Save the bitmap currently in the PictureBox, which is
		// m_oBitmapWithSelection.

		Bitmap oOldBitmap = (Bitmap)m_oPictureBox.Image;

		Debug.Assert(oOldBitmap == m_oBitmapWithSelection);

		// Copy the "no selection" bitmap to the "with selection" bitmap.

		m_oBitmapWithSelection = (Bitmap)m_oBitmapNoSelection.Clone();

		// Redraw the selected vertices and edges.

		DrawSelection(m_oBitmapWithSelection);

		// Load the PictureBox with the new bitmap.

		m_oPictureBox.Image = m_oBitmapWithSelection;

		m_oPictureBox.Invalidate();

		// Dispose of the old bitmap.

		GraphicsUtil.DisposeBitmap(ref oOldBitmap);
	}

	//*************************************************************************
	//	Method: SelectOrDeselectAll()
	//
	/// <summary>
	///	Selects or deselects all vertices and edges.
	/// </summary>
	///
	/// <param name="bSelect">
	/// true to select, false to deselect.
	/// </param>
	//*************************************************************************

	protected void
	SelectOrDeselectAll
	(
		Boolean bSelect
	)
	{
		AssertValid();

		SetVertexSelectedStateAll(bSelect);
		SetEdgeSelectedStateAll(bSelect);

		// Stop if the graph hasn't been drawn yet.

		if (m_oBitmapNoSelection == null)
		{
			return;
		}

		// Copy the "no selection" bitmap to the "with selection" bitmap and
		// update the PictureBox.

		UpdateBitmapWithSelection();

		// Fire a SelectionChanged event if appropriate.

		FireSelectionChanged();
	}

	//*************************************************************************
	//	Method: SetVertexSelectedState()
	//
	/// <summary>
	/// Sets the selected state of a vertex and updates the internal collection
	/// of selected vertices.
	/// </summary>
	///
	/// <param name="oVertex">
	/// Vertex to select or deselect.  Can't be null.
	/// </param>
	///
	/// <param name="bSelected">
	/// true to select <paramref name="oVertex" />, false to deselect it.
	/// </param>
	//*************************************************************************

	protected void
	SetVertexSelectedState
	(
		IVertex oVertex,
		Boolean bSelected
	)
	{
		Debug.Assert(oVertex != null);
		AssertValid();

		MultiSelectionGraphDrawer.SelectVertex(oVertex, bSelected);

		if (bSelected)
		{
			m_oSelectedVertices[oVertex] = ' ';
		}
		else
		{
			m_oSelectedVertices.Remove(oVertex);
		}
	}

	//*************************************************************************
	//	Method: SetVertexSelectedStateAll()
	//
	/// <summary>
	/// Sets the selected state of all vertices and updates the internal
	/// collection of selected vertices.
	/// </summary>
	///
	/// <param name="bSelected">
	/// true to select all vertices, false to deselect them.
	/// </param>
	//*************************************************************************

	protected void
	SetVertexSelectedStateAll
	(
		Boolean bSelected
	)
	{
		AssertValid();

		if (bSelected)
		{
			foreach (IVertex oVertex in this.Graph.Vertices)
			{
				SetVertexSelectedState(oVertex, true);
			}
		}
		else
		{
			// Do not directly iterate m_oSelectedVertices.Keys here.  Keys may
			// be removed by SetVertexSelectedState() and you can't modify a
			// collection while it's being iterated.  this.SelectedVertices
			// copies the collection to an array.

			foreach (IVertex oSelectedVertex in this.SelectedVertices)
			{
				SetVertexSelectedState(oSelectedVertex, false);
			}
		}
	}

	//*************************************************************************
	//	Method: SetEdgeSelectedState()
	//
	/// <summary>
	/// Sets the selected state of an edge and updates the internal collection
	/// of selected edges.
	/// </summary>
	///
	/// <param name="oEdge">
	/// Edge to select or deselect.  Can't be null.
	/// </param>
	///
	/// <param name="bSelected">
	/// true to select <paramref name="oEdge" />, false to deselect it.
	/// </param>
	//*************************************************************************

	protected void
	SetEdgeSelectedState
	(
		IEdge oEdge,
		Boolean bSelected
	)
	{
		Debug.Assert(oEdge != null);
		AssertValid();

		MultiSelectionGraphDrawer.SelectEdge(oEdge, bSelected);

		if (bSelected)
		{
			m_oSelectedEdges[oEdge] = ' ';
		}
		else
		{
			m_oSelectedEdges.Remove(oEdge);
		}
	}

	//*************************************************************************
	//	Method: SetEdgeSelectedStateAll()
	//
	/// <summary>
	/// Sets the selected state of all edges and updates the internal
	/// collection of selected edges.
	/// </summary>
	///
	/// <param name="bSelected">
	/// true to select all edges, false to deselect them.
	/// </param>
	//*************************************************************************

	protected void
	SetEdgeSelectedStateAll
	(
		Boolean bSelected
	)
	{
		AssertValid();

		if (bSelected)
		{
			foreach (IEdge oEdge in this.Graph.Edges)
			{
				SetEdgeSelectedState(oEdge, true);
			}
		}
		else
		{
			// Do not directly iterate m_oSelectedEdges.Keys here.  Keys may be
			// removed by SetEdgeSelectedState() and you can't modify a
			// collection while it's being iterated.  this.SelectedEdges copies
			// the collection to an array.

			foreach (IEdge oSelectedEdge in this.SelectedEdges)
			{
				SetEdgeSelectedState(oSelectedEdge, false);
			}
		}
	}

	//*************************************************************************
	//	Method: DrawSelection()
	//
	/// <summary>
	/// Draws the selected vertices and edges.
	/// </summary>
	///
	/// <param name="oBitmap">
	/// Bitmap to draw on.
	/// </param>
	//*************************************************************************

	protected void
	DrawSelection
	(
		Bitmap oBitmap
	)
	{
		Debug.Assert(oBitmap != null);
		AssertValid();

		foreach (IEdge oEdge in m_oSelectedEdges.Keys)
		{
			m_oMultiSelectionGraphDrawer.RedrawEdge(oEdge, oBitmap, true);
		}

		foreach (IVertex oVertex in m_oSelectedVertices.Keys)
		{
			m_oMultiSelectionGraphDrawer.RedrawVertex(oVertex, oBitmap, true);
		}
	}

	//*************************************************************************
	//	Method: LeftButtonIsPressed()
	//
	/// <summary>
	///	Determines whether the left mouse button is pressed.
	/// </summary>
	///
	/// <param name="oMouseEventArgs">
	/// Standard mouse event arguments.
	/// </param>
	///
	/// <returns>
	/// true if the left mouse button is pressed.
	/// </returns>
	//*************************************************************************

	protected Boolean
	LeftButtonIsPressed
	(
		MouseEventArgs oMouseEventArgs
	)
	{
		Debug.Assert(oMouseEventArgs != null);
		AssertValid();

		return ( (oMouseEventArgs.Button & MouseButtons.Left) != 0 );
	}

	//*************************************************************************
	//	Method: RightButtonIsPressed()
	//
	/// <summary>
	///	Determines whether the right mouse button is pressed.
	/// </summary>
	///
	/// <param name="oMouseEventArgs">
	/// Standard mouse event arguments.
	/// </param>
	///
	/// <returns>
	/// true if the right mouse button is pressed.
	/// </returns>
	//*************************************************************************

	protected Boolean
	RightButtonIsPressed
	(
		MouseEventArgs oMouseEventArgs
	)
	{
		Debug.Assert(oMouseEventArgs != null);
		AssertValid();

		return ( (oMouseEventArgs.Button & MouseButtons.Right) != 0 );
	}

	//*************************************************************************
	//	Method: ForcePointToBeWithinMargins()
	//
	/// <summary>
	/// Forces a point to fall within the graph's margins.
	/// </summary>
	///
	/// <param name="iX">
	/// X-coordinate of the point to force.
	/// </param>
	///
	/// <param name="iY">
	/// Y-coordinate of the point to force.
	/// </param>
	//*************************************************************************

	protected void
	ForcePointToBeWithinMargins
	(
		ref Int32 iX,
		ref Int32 iY
	)
	{
		AssertValid();

		Rectangle oClientRectangle = this.ClientRectangle;

		Int32 iMargin = m_oMultiSelectionGraphDrawer.Layout.Margin;

		oClientRectangle.Inflate(-iMargin, -iMargin);

		iX = Math.Max(iX, oClientRectangle.Left);
		iX = Math.Min(iX, oClientRectangle.Right);
		iY = Math.Max(iY, oClientRectangle.Top);
		iY = Math.Min(iY, oClientRectangle.Bottom);
	}

	//*************************************************************************
	//	Method: GetCursorForMarqueeDrag()
	//
	/// <summary>
	/// Returns a cursor to use for the control while a marquee drag operation
	/// is occurring.
	/// </summary>
	///
	/// <returns>
	/// The cursor to use.
	/// </returns>
	//*************************************************************************

	protected Cursor
	GetCursorForMarqueeDrag()
	{
		AssertValid();

		Boolean bShiftKeyPressed = (Control.ModifierKeys == Keys.Shift);
		Boolean bAltKeyPressed = (Control.ModifierKeys == Keys.Alt);

		String sResourceName = null;

		if (bShiftKeyPressed)
		{
			sResourceName = "MarqueeAdd.cur";
		}
		else if (bAltKeyPressed)
		{
			sResourceName = "MarqueeSubtract.cur";
		}
		else
		{
			sResourceName = "Marquee.cur";
		}

        return ( new Cursor(this.GetType(), "Images." + sResourceName) );
	}

	//*************************************************************************
	//	Method: GetMarqueeRectangle()
	//
	/// <summary>
	///	Gets the marquee rectangle to use during a marquee drag operation.
	/// </summary>
	///
	/// <param name="oMouseEventArgs">
	/// Standard mouse event arguments.
	/// </param>
	//*************************************************************************

	protected Rectangle
	GetMarqueeRectangle
	(
		MouseEventArgs oMouseEventArgs
	)
	{
		AssertValid();
		Debug.Assert(oMouseEventArgs != null);
		Debug.Assert(m_oMarqueeBeingDragged != null);
		Debug.Assert(m_oMarqueeBeingDragged.DragHasBegun);

		Int32 iNewLocationX = oMouseEventArgs.X;
		Int32 iNewLocationY = oMouseEventArgs.Y;

		// Limit the drag to the graph margins.

		ForcePointToBeWithinMargins(ref iNewLocationX, ref iNewLocationY);

		Point oMouseDownLocation = m_oMarqueeBeingDragged.MouseDownLocation;
		Int32 iMouseDownLocationX = oMouseDownLocation.X;
		Int32 iMouseDownLocationY = oMouseDownLocation.Y;

		return ( new Rectangle(
			Math.Min(iMouseDownLocationX, iNewLocationX),
			Math.Min(iMouseDownLocationY, iNewLocationY),
			Math.Abs(iNewLocationX - iMouseDownLocationX),
			Math.Abs(iNewLocationY - iMouseDownLocationY)
			) );
	}

	//*************************************************************************
	//	Method: GetBackgroundContrastColor()
	//
	/// <summary>
	/// Gets a color that contrasts with the background.
	/// </summary>
	///
	/// <returns>
	/// A contrastring color.
	/// </returns>
	//*************************************************************************

	protected Color
	GetBackgroundContrastColor()
	{
		AssertValid();

		Color oBackColor = this.BackColor;

		return ( Color.FromArgb(
			Math.Abs(oBackColor.R - 255),
			Math.Abs(oBackColor.G - 255),
			Math.Abs(oBackColor.B - 255)
			) );
	}

	//*************************************************************************
	//	Method: CheckForVertexDragOnMouseMove()
	//
	/// <summary>
	///	Checks for a vertex drag operation during the MouseMove event on the
	/// m_oPictureBox object.
	/// </summary>
	///
	/// <param name="oMouseEventArgs">
	/// Standard mouse event arguments.
	/// </param>
	//*************************************************************************

	protected void
	CheckForVertexDragOnMouseMove
	(
		MouseEventArgs oMouseEventArgs
	)
	{
		Debug.Assert(oMouseEventArgs != null);
		AssertValid();

		if (
			m_oVertexBeingDragged == null ||
			!LeftButtonIsPressed(oMouseEventArgs) ||

			( !m_oVertexBeingDragged.DragHasBegun &&
				!m_oVertexBeingDragged.ShouldBeginDrag(oMouseEventArgs) )
			)
		{
			return;
		}

		Int32 iNewLocationX = oMouseEventArgs.X;
		Int32 iNewLocationY = oMouseEventArgs.Y;

		// Limit the drag to the graph margins.

		ForcePointToBeWithinMargins(ref iNewLocationX, ref iNewLocationY);

		// Erase any temporary circle and edge lines drawn during the previous
		// MouseMove event.  Calling PictureBox.Refresh causes the PictureBox
		// control to copy its bitmap (m_oBitmapWithSelection) to its drawing
		// surface.

		m_oPictureBox.Refresh();

		// Change the location of the dragged vertex.

		IVertex oVertex = m_oVertexBeingDragged.Vertex;

		PointF oNewLocation = new PointF(iNewLocationX, iNewLocationY);

		oVertex.Location = oNewLocation;

		// Draw a temporary circle and edge lines from the dragged vertex to
		// its adjacent vertices.  The drawing is done directly on the
		// PictureBox's surface and does not affect the PictureBox's image.

		Graphics oGraphics = m_oPictureBox.CreateGraphics();

		oGraphics.SmoothingMode = SmoothingMode.AntiAlias;

		Color oTemporaryContrastColor = GetBackgroundContrastColor();

		Pen oPen = new Pen(oTemporaryContrastColor);

		oPen.DashStyle = DashStyle.Dash;
		oPen.DashPattern = new Single [] {5F, 2.5F};
        oPen.Width = DraggedEdgePenWidth;

		foreach (IVertex oAdjacentVertex in oVertex.AdjacentVertices)
		{
			oGraphics.DrawLine(oPen, oAdjacentVertex.Location, oNewLocation);
		}

		GraphicsUtil.DisposePen(ref oPen);

		Brush oBrush = new SolidBrush(oTemporaryContrastColor);

		GraphicsUtil.FillCircle(oGraphics, oBrush, oNewLocation.X,
			oNewLocation.Y, DraggedVertexRadius);

		GraphicsUtil.DisposeBrush(ref oBrush);

		GraphicsUtil.DisposeGraphics(ref oGraphics);

		// Allow the layout to modify any geometry metadata it added to the
		// graph, vertex, or edges when the graph was laid out.

		m_oMultiSelectionGraphDrawer.Layout.OnVertexMove(oVertex);
	}

	//*************************************************************************
	//	Method: CheckForVertexDragOnMouseUp()
	//
	/// <summary>
	///	Checks for a vertex drag operation during the MouseUp event on the
	/// m_oPictureBox object.
	/// </summary>
	///
	/// <param name="oMouseEventArgs">
	/// Standard mouse event arguments.
	/// </param>
	//*************************************************************************

	protected void
	CheckForVertexDragOnMouseUp
	(
		MouseEventArgs oMouseEventArgs
	)
	{
		Debug.Assert(oMouseEventArgs != null);
		AssertValid();

		if (m_oVertexBeingDragged != null &&
            m_oVertexBeingDragged.DragHasBegun)
		{
			Debug.Assert(m_oBitmapNoSelection != null);
			Debug.Assert(m_oBitmapWithSelection != null);

			Debug.Assert( (Bitmap)m_oPictureBox.Image ==
				m_oBitmapWithSelection );

			// Prepare the PictureBox for having its image updated.  (If the
			// following line is omitted, the PictureBox doesn't refresh
			// properly after its image is changed.)

			m_oPictureBox.Image = null;

			// Redraw the graph onto m_oBitmapNoSelection without laying it
			// out.

			Graphics oGraphics = Graphics.FromImage(m_oBitmapNoSelection);

			m_oMultiSelectionGraphDrawer.DrawCurrentLayout(

				oGraphics,

				new Rectangle(0, 0,
					m_oBitmapNoSelection.Width, m_oBitmapNoSelection.Height)
				);

			GraphicsUtil.DisposeGraphics(ref oGraphics);

			// Dispose of the old m_oBitmapWithSelection.

			GraphicsUtil.DisposeBitmap(ref m_oBitmapWithSelection);

			// Copy m_oBitmapNoSelection to m_oBitmapWithSelection.

			m_oBitmapWithSelection = (Bitmap)m_oBitmapNoSelection.Clone();

			// Redraw the selected vertices and edges.

			DrawSelection(m_oBitmapWithSelection);

			// Update the image in the PictureBox.

			m_oPictureBox.Image = m_oBitmapWithSelection;

			m_oPictureBox.Invalidate();

			FireVertexMoved(m_oVertexBeingDragged.Vertex);
		}

		m_oVertexBeingDragged = null;
	}

	//*************************************************************************
	//	Method: CheckForMarqueeDragOnMouseMove()
	//
	/// <summary>
	///	Checks for a marquee drag operation during the MouseMove event on the
	/// m_oPictureBox object.
	/// </summary>
	///
	/// <param name="oMouseEventArgs">
	/// Standard mouse event arguments.
	/// </param>
	//*************************************************************************

	protected void
	CheckForMarqueeDragOnMouseMove
	(
		MouseEventArgs oMouseEventArgs
	)
	{
		Debug.Assert(oMouseEventArgs != null);
		AssertValid();

		if (
			m_oMarqueeBeingDragged == null ||
			!LeftButtonIsPressed(oMouseEventArgs) ||

			( !m_oMarqueeBeingDragged.DragHasBegun &&
				!m_oMarqueeBeingDragged.ShouldBeginDrag(oMouseEventArgs) )
			)
		{
			return;
		}

		Int32 iNewLocationX = oMouseEventArgs.X;
		Int32 iNewLocationY = oMouseEventArgs.Y;

		// Limit the drag to the graph margins.

		ForcePointToBeWithinMargins(ref iNewLocationX, ref iNewLocationY);

		// Erase any marquee drawn during the previous MouseMove event.
		// Calling PictureBox.Refresh causes the PictureBox control to copy its
		// bitmap (m_oBitmapWithSelection) to its drawing surface.

		m_oPictureBox.Refresh();

		// Draw a marquee.  The drawing is done directly on the PictureBox's
		// surface and does not affect the PictureBox's image.

		Graphics oGraphics = m_oPictureBox.CreateGraphics();

		oGraphics.SmoothingMode = SmoothingMode.AntiAlias;

		Pen oPen = new Pen( GetBackgroundContrastColor() );

		oPen.DashStyle = DashStyle.Dash;
		oPen.DashPattern = new Single [] {5F, 2.5F};
        oPen.Width = DraggedMarqueePenWidth;

		oGraphics.DrawRectangle( oPen, GetMarqueeRectangle(oMouseEventArgs) );

		GraphicsUtil.DisposePen(ref oPen);

		GraphicsUtil.DisposeGraphics(ref oGraphics);

		// Set the cursor.

		#if false

		// TODO: This doesn't work when NetMapControl is used in ExcelTemplate.
		// The cursor changes to the specified cursor while the mouse is
		// moving, but switches back to the default cursor when the mouse
		// stops.  However, the cursor works fine in DesktopApplication and
		// TestNetMapControl.
		//
		// Is this a bug in the ExcelTemplate code, or a bug in VSTO?

		this.Cursor = GetCursorForMarqueeDrag();

		#endif
	}

	//*************************************************************************
	//	Method: CheckForMarqueeDragOnMouseUp()
	//
	/// <summary>
	///	Checks for a marquee drag operation during the MouseUp event on the
	/// m_oPictureBox object.
	/// </summary>
	///
	/// <param name="oMouseEventArgs">
	/// Standard mouse event arguments.
	/// </param>
	//*************************************************************************

	protected void
	CheckForMarqueeDragOnMouseUp
	(
		MouseEventArgs oMouseEventArgs
	)
	{
		Debug.Assert(oMouseEventArgs != null);
		AssertValid();

		if (m_oMarqueeBeingDragged != null &&
            m_oMarqueeBeingDragged.DragHasBegun)
		{
			#if false
			// See the note in CheckForMarqueeDragOnMouseMove().
			this.Cursor = Cursors.Default;
			#endif

			// Erase any marquee drawn during the previous MouseMove event.
			// Calling PictureBox.Refresh causes the PictureBox control to copy
			// its bitmap (m_oBitmapWithSelection) to its drawing surface.

			m_oPictureBox.Refresh();

			IVertexDrawer oVertexDrawer = this.VertexDrawer;

			Rectangle oMarqueeRectangle = GetMarqueeRectangle(oMouseEventArgs) ;

			// Get a dictionary of vertices that intersect the marquee's
			// interior, along with their incident edges.  Dictionaries are
			// used instead of lists or arrays to prevent the same vertex or
			// edge from being added twice.

			Dictionary<IVertex, Char> oIntersectingVertices;
			Dictionary<IEdge, Char> oIncidentEdges;

			Boolean bShiftKeyPressed = (Control.ModifierKeys == Keys.Shift);
			Boolean bAltKeyPressed = (Control.ModifierKeys == Keys.Alt);

			if (bShiftKeyPressed || bAltKeyPressed)
			{
				// The new selection gets added to or subtracted from the old
				// selection.

				oIntersectingVertices =
					new Dictionary<IVertex, Char>(m_oSelectedVertices);

				oIncidentEdges = new Dictionary<IEdge, Char>(m_oSelectedEdges);
			}
			else
			{
				// The new selection replaces the old selection.

				oIntersectingVertices = new Dictionary<IVertex, Char>();
				oIncidentEdges = new Dictionary<IEdge, Char>();
			}

			foreach (IVertex oVertex in this.Graph.Vertices)
			{
				if ( !oVertexDrawer.VertexIntersectsWithRectangle(oVertex,
					oMarqueeRectangle) )
				{
					continue;
				}

				if (bAltKeyPressed)
				{
					oIntersectingVertices.Remove(oVertex);
				}
				else
				{
					oIntersectingVertices[oVertex] = ' ';
				}

				if (m_eMouseSelectionMode ==
					MouseSelectionMode.SelectVertexAndIncidentEdges)
				{
					// Also select the vertex's incident edges.

					foreach (IEdge oEdge in oVertex.IncidentEdges)
					{
						if (bAltKeyPressed)
						{
							oIncidentEdges.Remove(oEdge);
						}
						else
						{
							oIncidentEdges[oEdge] = ' ';
						}
					}
				}
			}

			SetSelected(
				CollectionUtil.DictionaryKeysToArray<IVertex, Char>(
					oIntersectingVertices),

				CollectionUtil.DictionaryKeysToArray<IEdge, Char>(
					oIncidentEdges)
				);
		}

		m_oMarqueeBeingDragged = null;
	}

	//*************************************************************************
	//	Method: CheckForToolTipsOnMouseMove()
	//
	/// <summary>
	///	Checks whether tooltip-related actions need to be taken during the
	/// MouseMove event on the m_oPictureBox object.
	/// </summary>
	///
	/// <param name="oMouseEventArgs">
	/// Standard mouse event arguments.
	/// </param>
	//*************************************************************************

	protected void
	CheckForToolTipsOnMouseMove
	(
		MouseEventArgs oMouseEventArgs
	)
	{
		Debug.Assert(oMouseEventArgs != null);
		AssertValid();

		// Don't show a tooltip if they shouldn't be shown or if the mouse is
		// being dragged.

        if (!m_bShowToolTips || m_oVertexBeingDragged != null ||
			m_oMarqueeBeingDragged != null)
        {
            return;
        }

		if (oMouseEventArgs.X == m_oLastMouseMovePoint.X &&
			oMouseEventArgs.Y == m_oLastMouseMovePoint.Y)
		{
			// ShowToolTip() and HideToolTip() do their work by making the
			// m_oToolTipPanel visible or hidden.  This causes MouseMove events
			// to fire, even though the mouse hasn't moved.  To work around
			// this oddity, ignore the MouseMove event if the mouse hasn't
			// really moved.

			return;
		}

		m_oLastMouseMovePoint =
			new Point(oMouseEventArgs.X, oMouseEventArgs.Y);

		if (m_oToolTipPanel.Visible
			&&
			new Rectangle(m_oToolTipPanel.Location, m_oToolTipPanel.Size).
				Contains(m_oLastMouseMovePoint)
			)
		{
			// The tooltip panel is showing and the mouse is inside it.  Ignore
			// the event.

			return;
		}

		// If the mouse is over a vertex, get the vertex object.

		IVertex oVertex;

		m_oMultiSelectionGraphDrawer.GetVertexFromPoint(oMouseEventArgs.X,
			oMouseEventArgs.Y, out oVertex);

		// (oVertex is null if the mouse is not over a vertex.)

		// Update the tooltip tracker.

		m_oToolTipTracker.OnMouseMoveOverObject(oVertex);
	}

	//*************************************************************************
	//	Method: OnBitmapNoSelectionDrawn()
	//
	/// <summary>
	///	Performs tasks required after the graph is drawn onto
	/// m_oBitmapNoSelection.
	/// </summary>
	///
	/// <remarks>
	///	It's assumed that the bitmap in m_oPictureBox is the "no selection"
	/// bitmap, m_oBitmapNoSelection.
	/// </remarks>
	//*************************************************************************

	protected void
	OnBitmapNoSelectionDrawn()
	{
		Debug.Assert( (Bitmap)m_oPictureBox.Image == m_oBitmapNoSelection );

		// Dispose of the old m_oBitmapWithSelection, if there was one.

		GraphicsUtil.DisposeBitmap(ref m_oBitmapWithSelection);

		// Copy m_oBitmapNoSelection to m_oBitmapWithSelection.

		m_oBitmapWithSelection = (Bitmap)m_oBitmapNoSelection.Clone();

		// Redraw the selected vertices and edges.

		DrawSelection(m_oBitmapWithSelection);

		// Load m_oBitmapWithSelection into the PictureBox.

		m_oPictureBox.Image = m_oBitmapWithSelection;

		m_oPictureBox.Refresh();

		// Notify any listeners that drawing is complete.

		FireGraphDrawn();
	}

    //*************************************************************************
    //  Method: OnResize()
    //
    /// <summary>
    /// Handles the Resize event.
    /// </summary>
	///
	/// <param name="oEventArgs">
	/// Standard event arguments.
	/// </param>
    //*************************************************************************

	protected override void
	OnResize
	(
		EventArgs oEventArgs
	)
	{
		AssertValid();
		Debug.Assert(oEventArgs != null);

		const String MethodName = "OnResize";

		if (this.Graph.Vertices.Count == 0)
		{
			return;
		}

		TraceEvent(MethodName, "Calling StartOrScheduleDraw.");

		// The graph needs to be redrawn without laying it out.  Start or
		// schedule a drawing operation.

		StartOrScheduleDraw(DrawType.TransformLayoutAndDraw);
	}

	//*************************************************************************
	//	Method: OnLayoutOrRedrawRequired()
	//
	/// <summary>
	///	Handles the LayoutRequired and RedrawRequired events on the
	/// m_oMultiSelectionGraphDrawer
	/// </summary>
	///
	/// <param name="bOnLayoutRequired">
	/// true if this is being called in response to a LayoutRequired event,
	/// false if it is being called in response to a RedrawRequired event.
	/// </param>
	//*************************************************************************

	protected void
	OnLayoutOrRedrawRequired
	(
		Boolean bOnLayoutRequired
	)
	{
        if (this.IsDisposed)
        {
			// This can occur if the form containing this control is closed
			// while drawing is in progress.

            return;
        }

		AssertValid();

		const String MethodName = "OnLayoutOrRedrawRequired";

		if (m_bInBeginUpdate)
		{
			// Wait until EndUpdate() is called to do anything.

			return;
		}

		// Start or schedule a drawing operation.

		TraceEvent(MethodName, "Calling StartOrScheduleDraw.");

		StartOrScheduleDraw(bOnLayoutRequired ?
            DrawType.AsyncLayoutAndDraw : DrawType.DrawOnly);
	}

	//*************************************************************************
	//	Method: Dispose()
	//
	/// <summary>
	///	Frees resources.  Call this when you are done with the object.
	/// </summary>
	///
	/// <param name="bDisposing">
	/// See Component.Dispose().
	/// </param>
	//*************************************************************************

	protected override void
	Dispose
	(
		Boolean bDisposing
	)
	{
		if (bDisposing)
		{
			GraphicsUtil.DisposeBitmap(ref m_oBitmapNoSelection);

			if (m_oToolTipTracker != null)
			{
				m_oToolTipTracker.Dispose();
				m_oToolTipTracker = null;
			}
		}

		base.Dispose(bDisposing);
	}

	//*************************************************************************
	//	Method: m_oPictureBox_MouseDown()
	//
	/// <summary>
	///	Handles the MouseDown event on the m_oPictureBox object.
	/// </summary>
	///
	/// <param name="oSource">
	/// Source of the event.
	/// </param>
	///
	/// <param name="oMouseEventArgs">
	/// Standard mouse event arguments.
	/// </param>
	//*************************************************************************

	protected void
	m_oPictureBox_MouseDown
	(
		Object oSource,
		MouseEventArgs oMouseEventArgs
	)
	{
		Debug.Assert(oMouseEventArgs != null);
		AssertValid();

		// Do nothing if an asynchronous draw is in progress.

		if (m_oMultiSelectionGraphDrawer.IsBusy)
		{
			return;
		}

		// Check whether the user clicked on a vertex.

		IVertex oVertex;

		Boolean bClickedVertex =
			m_oMultiSelectionGraphDrawer.GetVertexFromPoint(
				oMouseEventArgs.Location, out oVertex);

		// Fire a GraphMouseDown event if appropriate.

		FireGraphMouseDown(oMouseEventArgs, oVertex);

		Boolean bControlKeyPressed = (Control.ModifierKeys == Keys.Control);
		Boolean bRightButtonPressed = RightButtonIsPressed(oMouseEventArgs);

		if (!bClickedVertex)
		{
			// The user clicked on part of the graph not covered by a
			// vertex.

			if (m_eMouseSelectionMode != MouseSelectionMode.SelectNothing)
			{
				Boolean bShiftKeyPressed = (Control.ModifierKeys == Keys.Shift);
				Boolean bAltKeyPressed = (Control.ModifierKeys == Keys.Alt);

				if (!bShiftKeyPressed && !bControlKeyPressed &&
					!bAltKeyPressed && !bRightButtonPressed)
				{
					DeselectAll();
				}

				if (this.Graph.Vertices.Count > 0)
				{
					// The user might want to drag a marquee.  Save the mouse
					// location for use within the MouseMove event.

					m_oMarqueeBeingDragged = new MouseDrag(
						new Point(oMouseEventArgs.X, oMouseEventArgs.Y) );
				}

				// Hide any tooltip that might be showing and reset the helper
				// object that figures out when to show tooltips.

				ResetToolTipTracker();
			}

			return;
		}

		// The user clicked on a vertex.  Fire a VertexClick event if
		// appropriate.

		FireVertexClick(oVertex);

		if (m_eMouseSelectionMode != MouseSelectionMode.SelectNothing)
		{
			Boolean bSelectVertex = true;

			if (bControlKeyPressed)
			{
				// Toggle the vertex's selected state.

				bSelectVertex =
					!MultiSelectionGraphDrawer.VertexIsSelected(oVertex);
			}
			else if (!bRightButtonPressed)
			{
				// Clear the selection.

				SetVertexSelectedStateAll(false);
				SetEdgeSelectedStateAll(false);

				Debug.Assert(m_oSelectedVertices.Count == 0);
				Debug.Assert(m_oSelectedEdges.Count == 0);
			}

			// Select the clicked vertex and possibly its incident edges.

			SetVertexSelected(oVertex, bSelectVertex,
				m_eMouseSelectionMode ==
					MouseSelectionMode.SelectVertexAndIncidentEdges
				);
		}

		if ( m_bAllowVertexDrag && LeftButtonIsPressed(oMouseEventArgs) )
		{
            // The user might want to drag the clicked vertex.  Save the
			// clicked vertex for use within the MouseMove event.

			m_oVertexBeingDragged = new DraggedVertex(
				oVertex, new Point(oMouseEventArgs.X, oMouseEventArgs.Y)
				);

			// Hide any tooltip that might be showing and reset the helper
			// object that figures out when to show tooltips.

			ResetToolTipTracker();
		}
	}

	//*************************************************************************
	//	Method: m_oPictureBox_MouseMove()
	//
	/// <summary>
	///	Handles the MouseMove event on the m_oPictureBox object.
	/// </summary>
	///
	/// <param name="oSource">
	/// Source of the event.
	/// </param>
	///
	/// <param name="oMouseEventArgs">
	/// Standard mouse event arguments.
	/// </param>
	//*************************************************************************

	protected void
	m_oPictureBox_MouseMove
	(
		Object oSource,
		MouseEventArgs oMouseEventArgs
	)
	{
		AssertValid();

		// Do nothing if an asynchronous draw is in progress.

		if (m_oMultiSelectionGraphDrawer.IsBusy)
		{
			return;
		}

		// Check for a vertex drag operation.

		CheckForVertexDragOnMouseMove(oMouseEventArgs);

		// Check for a marquee drag operation.

		CheckForMarqueeDragOnMouseMove(oMouseEventArgs);

		// Check whether tooltip-related actions need to be taken.

		CheckForToolTipsOnMouseMove(oMouseEventArgs);
	}

	//*************************************************************************
	//	Method: m_oPictureBox_MouseUp()
	//
	/// <summary>
	///	Handles the MouseUp event on the m_oPictureBox object.
	/// </summary>
	///
	/// <param name="oSource">
	/// Source of the event.
	/// </param>
	///
	/// <param name="oMouseEventArgs">
	/// Standard mouse event arguments.
	/// </param>
	//*************************************************************************

	protected void
	m_oPictureBox_MouseUp
	(
		Object oSource,
		MouseEventArgs oMouseEventArgs
	)
	{
		AssertValid();

		// Do nothing if an asynchronous draw is in progress.

		if (m_oMultiSelectionGraphDrawer.IsBusy)
		{
			return;
		}

		// Check whether the user clicked on a vertex.

		IVertex oVertex;

		m_oMultiSelectionGraphDrawer.GetVertexFromPoint(
			oMouseEventArgs.Location, out oVertex);

		// Fire a GraphMouseUp event if appropriate.

		FireGraphMouseUp(oMouseEventArgs, oVertex);

		// Check for a vertex drag operation.

		CheckForVertexDragOnMouseUp(oMouseEventArgs);

		// Check for a marquee drag operation.

		CheckForMarqueeDragOnMouseUp(oMouseEventArgs);
	}

	//*************************************************************************
	//	Method: m_oPictureBox_DoubleClick()
	//
	/// <summary>
	///	Handles the DoubleClick event on the m_oPictureBox object.
	/// </summary>
	///
	/// <param name="oSource">
	/// Source of the event.
	/// </param>
	///
	/// <param name="oEventArgs">
	/// Standard event arguments.
	/// </param>
	//*************************************************************************

	protected void
	m_oPictureBox_DoubleClick
	(
		Object oSource,
		System.EventArgs oEventArgs
	)
	{
		AssertValid();

		// Do nothing if an asynchronous draw is in progress.

		if (m_oMultiSelectionGraphDrawer.IsBusy)
		{
			return;
		}

		// Check whether the user double-clicked on a vertex.

		IVertex oVertex;

		Boolean bDoubleClickedVertex =
			m_oMultiSelectionGraphDrawer.GetVertexFromPoint(
				ControlUtil.GetClientMousePosition(this.m_oPictureBox),
                out oVertex);

		if (bDoubleClickedVertex)
		{
			FireVertexDoubleClick(oVertex);
		}
	}

	//*************************************************************************
	//	Method: m_oPictureBox_MouseLeave()
	//
	/// <summary>
	///	Handles the MouseLeave event on the m_oPictureBox object.
	/// </summary>
	///
	/// <param name="oSource">
	/// Standard event arguments.
	/// </param>
	///
	/// <param name="oEventArgs">
	/// Standard event arguments.
	/// </param>
	//*************************************************************************

	private void
	m_oPictureBox_MouseLeave
	(
		Object oSource,
		EventArgs oEventArgs
	)
	{
		// Do nothing if an asynchronous draw is in progress.

		if (m_oMultiSelectionGraphDrawer.IsBusy)
		{
			return;
		}

		// Hide any tooltip that might be showing and reset the helper object
		// that figures out when to show tooltips.

		ResetToolTipTracker();
	}

	//*************************************************************************
	//	Method: MultiSelectionGraphDrawer_DrawAsyncIterationCompleted()
	//
	/// <summary>
	///	Handles the DrawAsyncIterationCompleted event on the
	/// m_oMultiSelectionGraphDrawer object.
	/// </summary>
	///
	/// <param name="oSender">
	/// Source of the event.
	/// </param>
	///
	/// <param name="oEventArgs">
	/// Standard event arguments.
	/// </param>
	//*************************************************************************

	protected void
	MultiSelectionGraphDrawer_DrawAsyncIterationCompleted
	(
		Object oSender,
		EventArgs oEventArgs
	)
	{
        if (this.IsDisposed)
        {
			// This can occur if the form containing this control is closed
			// while drawing is in progress.

            return;
        }

		Debug.Assert(!this.InvokeRequired);

		const String MethodName =
			"MultiSelectionGraphDrawer_DrawAsyncIterationCompleted";

		TraceEvent(MethodName, "Enter.");

		// Although the MultiSelectionGraphDrawer has been drawing on the
		// PictureBox's bitmap, the changes won't be visible until the
		// PictureBox refreshes itself.

		m_oPictureBox.Refresh();
	}

	//*************************************************************************
	//	Method: MultiSelectionGraphDrawer_DrawAsyncCompleted()
	//
	/// <summary>
	///	Handles the DrawAsyncCompleted event on the
	/// m_oMultiSelectionGraphDrawer object.
	/// </summary>
	///
	/// <param name="oSender">
	/// Source of the event.
	/// </param>
	///
	/// <param name="oAsyncCompletedEventArgs">
	/// Standard event arguments.
	/// </param>
	//*************************************************************************

	protected void
	MultiSelectionGraphDrawer_DrawAsyncCompleted
	(
		Object oSender,
		AsyncCompletedEventArgs oAsyncCompletedEventArgs
	)
	{
        if (this.IsDisposed)
        {
			// This can occur if the form containing this control is closed
			// while drawing is in progress.

            return;
        }

		Debug.Assert(!this.InvokeRequired);

		const String MethodName =
			"MultiSelectionGraphDrawer_DrawAsyncCompleted";

		TraceEvent(MethodName, "Enter.");

        if (oAsyncCompletedEventArgs.Error != null)
        {
			// TODO: What to do here?  Throwing an exception has no effect.

			return;
        }
        else if (oAsyncCompletedEventArgs.Cancelled)
        {
			TraceEvent(MethodName, "Cancelled.");

			// Asynchronous drawing gets cancelled when the control is resized.
			// Do nothing here.

            FireGraphDrawn();

			return;
        }

		TraceEvent(MethodName, "Completed.");

		OnBitmapNoSelectionDrawn();
	}

	//*************************************************************************
	//	Method: MultiSelectionGraphDrawer_LayoutRequired()
	//
	/// <summary>
	///	Handles the LayoutRequired event on the m_oMultiSelectionGraphDrawer
	/// object.
	/// </summary>
	///
	/// <param name="oSender">
	/// Source of the event.
	/// </param>
	///
	/// <param name="oEventArgs">
	/// Standard event arguments.
	/// </param>
	//*************************************************************************

	protected void
	MultiSelectionGraphDrawer_LayoutRequired
	(
		Object oSender,
		EventArgs oEventArgs
	)
	{
		Debug.Assert(!this.InvokeRequired);

		OnLayoutOrRedrawRequired(true);
	}

	//*************************************************************************
	//	Method: MultiSelectionGraphDrawer_RedrawRequired()
	//
	/// <summary>
	///	Handles the RedrawRequired event on the m_oMultiSelectionGraphDrawer
	/// object.
	/// </summary>
	///
	/// <param name="oSender">
	/// Source of the event.
	/// </param>
	///
	/// <param name="oEventArgs">
	/// Standard event arguments.
	/// </param>
	//*************************************************************************

	protected void
	MultiSelectionGraphDrawer_RedrawRequired
	(
		Object oSender,
		EventArgs oEventArgs
	)
	{
		Debug.Assert(!this.InvokeRequired);

		OnLayoutOrRedrawRequired(false);
	}

	//*************************************************************************
	//	Method: DrawAsyncTimer_Tick()
	//
	/// <summary>
	///	Handles the Tick event on the m_oDrawAsyncTimer object.
	/// </summary>
	///
	/// <param name="oSender">
	/// Source of the event.
	/// </param>
	///
	/// <param name="oEventArgs">
	/// Standard event arguments.
	/// </param>
	//*************************************************************************

    protected void
	DrawAsyncTimer_Tick
	(
		Object oSender,
		EventArgs oEventArgs
	)
    {
		const String MethodName = "DrawAsyncTimer_Tick";

		if (m_oMultiSelectionGraphDrawer.IsBusy)
		{
			// Check again on the next timer tick.

			return;
		}

		// Stop the timer.

		TraceEvent(MethodName, "Stopping timer.");

		m_oDrawAsyncTimer.Enabled = false;

		// Start a drawing operation.

		TraceEvent(MethodName, "Calling StartDraw.");

		StartDraw(m_eDrawType);
    }

	//*************************************************************************
	//	Method: oToolTipTracker_ShowToolTip()
	//
	/// <summary>
	///	Handles the ShowToolTip event on the oToolTipTracker object.
	/// </summary>
	///
	/// <param name="oSource">
	/// Standard event arguments.
	/// </param>
	///
	/// <param name="oToolTipTrackerEventArgs">
	/// Standard event arguments.
	/// </param>
	//*************************************************************************

	private void
	oToolTipTracker_ShowToolTip
	(
		Object oSource,
		ToolTipTrackerEventArgs oToolTipTrackerEventArgs
	)
	{
		AssertValid();

		// Get the vertex object that needs a tooltip shown.

		IVertex oVertex = (IVertex)oToolTipTrackerEventArgs.Object;
		Debug.Assert(oVertex != null);

		Object oToolTip;
			
		if ( m_bShowToolTips && oVertex.TryGetValue(
			ReservedMetadataKeys.ToolTip, typeof(String), out oToolTip) )
		{
			String sToolTip = (String)oToolTip;

			if ( !String.IsNullOrEmpty(sToolTip) )
			{
				m_oToolTipPanel.ShowToolTip(sToolTip, this);
			}
		}

		// Fire a NodeMouseHover event in case the application wants to take
		// additional action.

		FireVertexMouseHover(oVertex);
	}

	//*************************************************************************
	//	Method: oToolTipTracker_HideToolTip()
	//
	/// <summary>
	///	Handles the HideToolTip event on the oToolTipTracker object.
	/// </summary>
	///
	/// <param name="oSource">
	/// Standard event arguments.
	/// </param>
	///
	/// <param name="oToolTipTrackerEventArgs">
	/// Standard event arguments.
	/// </param>
	//*************************************************************************

	private void
	oToolTipTracker_HideToolTip
	(
		Object oSource,
		ToolTipTrackerEventArgs oToolTipTrackerEventArgs
	)
	{
		AssertValid();

		m_oToolTipPanel.HideToolTip();
	}

    //*************************************************************************
    //  Method: TraceEvent()
    //
    /// <summary>
	/// Writes a message describing the state of the control.
    /// </summary>
	///
	/// <param name="sMethodOrPropertyName">
	/// Name of the method or property calling this method.
	/// </param>
	///
	/// <param name="sEventDescription">
	/// Description of the event.
	/// </param>
    //*************************************************************************

    [Conditional("TRACEEVENT")]

    protected void
    TraceEvent
	(
		String sMethodOrPropertyName,
		String sEventDescription
	)
    {
		Debug.Assert( !String.IsNullOrEmpty(sMethodOrPropertyName) );
		Debug.Assert( !String.IsNullOrEmpty(sEventDescription) );

		String sCompleteDescription = String.Format(

			"{0}: {1}: {2}  IsBusy={3}. TimerEnabled={4}."
			,
			DateTime.Now.ToLongTimeString(),
			sMethodOrPropertyName,
			sEventDescription,
			m_oMultiSelectionGraphDrawer.IsBusy,
			m_oDrawAsyncTimer.Enabled
			);

		Trace.WriteLine(sCompleteDescription);
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
		Debug.Assert(m_oMultiSelectionGraphDrawer != null);
		// m_oBitmapNoSelection
		// m_oBitmapWithSelection
		Debug.Assert(m_oPictureBox != null);
		// m_eMouseSelectionMode
		// m_bAllowVertexDrag
		// m_oVertexBeingDragged
		// m_oMarqueeBeingDragged
		// m_oLastMouseMovePoint
		// m_bShowToolTips
		Debug.Assert(m_oSelectedVertices != null);
		Debug.Assert(m_oSelectedEdges != null);
		// m_bInBeginUpdate
		// m_eDrawType
		Debug.Assert(m_oDrawAsyncTimer != null);
		Debug.Assert(m_oToolTipTracker != null);
		Debug.Assert(m_oToolTipPanel != null);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Interval used by m_oDrawAsyncTimer, in milliseconds.

	protected const Int32 DrawAsyncTimerIntervalMs = 100;

	/// Message drawn by DrawWaitMessage().

	protected const String WaitMessage = "Laying out graph...";

	/// Font size used by DrawWaitMessage().

	protected const Single WaitMessageFontSize = 12;

	/// Coordinates of the message drawn by DrawWaitMessage().

	protected const Single WaitMessageX = 10;
	///
	protected const Single WaitMessageY = 10;


	/// Radius of a vertex as it is dragged with the mouse.

	protected const Single DraggedVertexRadius = 3.0F;

	/// Pen width of edge lines connected to a vertex as the vertex is dragged
	/// with the mouse.

	protected const Single DraggedEdgePenWidth = 1.0F;

	/// Pen width of a dragged marquee.

	protected const Single DraggedMarqueePenWidth = 1.0F;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object that does all of the drawing.

	protected MultiSelectionGraphDrawer m_oMultiSelectionGraphDrawer;

	/// Here is how the control's drawing scheme works:
	///
	/// When StartDraw() is called, m_oBitmapNoSelection is created with
	/// an appropriate size, and m_oMultiSelectionGraphDrawer asynchronously
	/// draws the graph on it without showing any vertices or edges as
	/// selected.  When the drawing is done,
	/// MultiSelectionGraphDrawer_DrawAsyncCompleted() copies
	/// m_oBitmapNoSelection to m_oBitmapWithSelection.  If there are any
	/// selected vertices or edges, m_oMultiSelectionGraphDrawer is told to
	/// redraw the selected vertices and edges as selected on
	/// m_oBitmapWithSelection.  m_oBitmapWithSelection is then loaded into
	/// m_oPictureBox.
	///
	/// If the selection changes, either programatically or with the mouse,
	/// m_oBitmapNoSelection is again copied to m_oBitmapWithSelection, and
	/// m_oMultiSelectionGraphDrawer is told to redraw the selected vertices
	/// and edges as selected on m_oBitmapWithSelection.
	///
	/// This scheme allows the selection to be quickly changed without
	/// redrawing the entire graph, at the expense of keeping two bitmaps in
	/// memory.
	///
	/// An alternative scheme would be to use just one bitmap, and have
	/// m_oMultiSelectionGraphDrawer redraw the old selected vertices as
	/// unselected before drawing the new selected vertices as selected.  This
	/// doesn't work, for several reasons.  First, MultiSelectionGraphDrawer
	/// uses anti-aliasing to smooth the vertices and edges.  It also uses
	/// different colors and line widths for selected vs.  unselected.  Even
	/// without the different widths, the anti-aliasing is a problem.  If you
	/// draw a red anti-aliased line on top of a black anti-aliased line, for
	/// example, the resulting line is not as red as it should be.  And if you
	/// draw black on top of the red, the red doesn't all disappear.
	///
	protected Bitmap m_oBitmapNoSelection;
	///
	protected Bitmap m_oBitmapWithSelection;


	/// PictureBox that contains m_oBitmapWithSelection.

	protected PictureBox m_oPictureBox;

	/// Determines what gets selected when a vertex is clicked with the mouse.

	protected MouseSelectionMode m_eMouseSelectionMode;

	/// true if a vertex can be moved by dragging it with the mouse.

	protected Boolean m_bAllowVertexDrag;

	/// Vertex the user is dragging with the mouse, or null if a vertex isn't
	/// being dragged.

	protected DraggedVertex m_oVertexBeingDragged;

	/// Marquee the user is dragging with the mouse, or null if a marquee isn't
	/// being dragged.

	protected MouseDrag m_oMarqueeBeingDragged;

	/// Most recent point passed into picPictureBox_MouseMove(), or (-1,-1) if
	/// that event hasn't fired yet.

	protected Point m_oLastMouseMovePoint;

	/// true to show vertex tooltips.

	protected Boolean m_bShowToolTips;

	/// Selected vertices and edges.  Dictionaries are used instead of lists or
	/// arrays to prevent the same vertex or edge from being added twice.  The
	/// keys are IVertex or IEdge and the values aren't used.

	Dictionary<IVertex, Char> m_oSelectedVertices;
	///
	Dictionary<IEdge, Char> m_oSelectedEdges;

	/// Gets set to true by BeginUpdate(), false by EndUpdate().

	protected Boolean m_bInBeginUpdate;

	/// Specifies the type of draw operation performed by StartDraw().

	protected DrawType m_eDrawType;

	/// Used by the ScheduleDraw() method.

	protected Timer m_oDrawAsyncTimer;

	/// Helper object for figuring out when to show tooltips.

	private ToolTipTracker m_oToolTipTracker;

	/// Panel used to display tooltips.

	protected ToolTipPanel m_oToolTipPanel;
}

}
