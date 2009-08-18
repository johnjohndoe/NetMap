
// Define TRACE_LAYOUT_AND_DRAW to write layout and drawing information to the
// debug output.

// #define TRACE_LAYOUT_AND_DRAW


// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Layouts;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using Microsoft.WpfGraphicsLib;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: NodeXLControl
//
/// <summary>
/// Lays out and renders a NodeXL graph in a WPF or Windows Forms desktop
/// application.
/// </summary>
///
/// <remarks>
///
/// <h3>Overview</h3>
///
/// <see cref="NodeXLControl" /> is one of several classes that render a NodeXL
/// graph.  It derives from FrameworkElement and is meant for use in WPF
/// desktop applications.
///
/// <para>
/// If you need a graph rendered as a Visual without the overhead of
/// FrameworkElement, use <see cref="NodeXLVisual" /> instead.  Note that <see
/// cref="NodeXLVisual" /> does not lay out the graph before drawing it.
/// </para>
///
/// <para>
/// You can also use <see cref="Wpf.GraphDrawer" />, which is a low-level class
/// used by both <see cref="NodeXLControl" /> and <see cref="NodeXLVisual" />.
/// <see cref="Wpf.GraphDrawer" /> cannot be directly rendered, however, and
/// requires a custom wrapper that hosts its GraphDrawer.<see
/// cref="Wpf.GraphDrawer.VisualCollection" /> object.  Also, it does not lay
/// out the graph before drawing it.
/// </para>
///
/// <para>
/// To use <see cref="NodeXLControl" />, populate the graph exposed by the <see
/// cref="NodeXLControl.Graph" /> property, then call <see
/// cref="DrawGraph(Boolean)" />.  See the sample code below.
/// </para>
///
/// <h3>Vertex and Edge Appearance</h3>
///
/// <para>
/// The default appearance of the graph's vertices is determined by the
/// following properties:
/// </para>
///
/// <list type="bullet">
/// <item><see cref="VertexColor" /></item>
/// <item><see cref="VertexSelectedColor" /></item>
/// <item><see cref="VertexShape" /></item>
/// <item><see cref="VertexRadius" /></item>
/// <item><see cref="VertexPrimaryLabelFillColor" /></item>
/// </list>
///
/// <para>
/// The default appearance of the graph's edges is determined by the following
/// properties:
/// </para>
///
/// <list type="bullet">
/// <item><see cref="EdgeColor" /></item>
/// <item><see cref="EdgeSelectedColor" /></item>
/// <item><see cref="EdgeWidth" /></item>
/// <item><see cref="EdgeSelectedWidth" /></item>
/// <item><see cref="EdgeRelativeArrowSize" /></item>
/// </list>
///
/// <para>
/// The appearance of an individual vertex can be overridden by adding
/// appropriate metadata to the vertex via <see
/// cref="IMetadataProvider.SetValue" />.  The following metadata keys can be
/// used:
/// </para>
///
/// <list type="bullet">
/// <item><see cref="ReservedMetadataKeys.Visibility" /></item>
/// <item><see cref="ReservedMetadataKeys.PerColor" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexShape" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexRadius" /></item>
/// <item><see cref="ReservedMetadataKeys.PerAlpha" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexPrimaryLabel" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexPrimaryLabelFillColor" />
///     </item>
/// <item><see cref="ReservedMetadataKeys.PerVertexSecondaryLabel" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexImage" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexDrawingPrecedence" /></item>
/// </list>
///
/// <para>
/// Similarly, the appearance of an individual edge can be overridden by adding
/// appropriate metadata to the edge.  The following metadata keys can be used:
/// </para>
///
/// <list type="bullet">
/// <item><see cref="ReservedMetadataKeys.Visibility" /></item>
/// <item><see cref="ReservedMetadataKeys.PerColor" /></item>
/// <item><see cref="ReservedMetadataKeys.PerAlpha" /></item>
/// <item><see cref="ReservedMetadataKeys.PerEdgeWidth" /></item>
/// <item><see cref="ReservedMetadataKeys.PerEdgeLabel" /></item>
/// </list>
///
/// <h3>Shapes, Labels, and Images</h3>
///
/// <para>
/// By default, vertices are drawn as the shape specified by the <see
/// cref="VertexShape" /> property.  The shape of an individual vertex can be
/// overridden with the <see cref="ReservedMetadataKeys.PerVertexShape" />
/// metadata key.
/// </para>
///
/// <para>
/// To draw an individual vertex as a rectangle-enclosed label instead of a
/// shape, use the <see cref="ReservedMetadataKeys.PerVertexPrimaryLabel" />
/// key.  The rectangle's fill color can be controlled with the <see
/// cref="ReservedMetadataKeys.PerVertexPrimaryLabelFillColor" /> key.
/// </para>
///
/// <para>
/// To draw an individual vertex as an image instead of a shape, use the <see
/// cref="ReservedMetadataKeys.PerVertexImage" /> key.
/// </para>
///
/// <para>
/// To control the precedence of the shape, primary label, and image keys, use
/// the <see cref="ReservedMetadataKeys.PerVertexDrawingPrecedence" /> key.
/// </para>
///
/// <para>
/// To add a secondary label to a vertex, use the the <see
/// cref="ReservedMetadataKeys.PerVertexSecondaryLabel" /> key.  Secondary
/// labels can be added to any vertex, regardless of whether it is drawn as a
/// shape, primary label, or image.  The secondary label is drawn above the
/// vertex.
/// </para>
///
/// <h3>Selecting Vertices and Edges</h3>
///
/// <para>
/// One or more vertices and their incident edges can be selected with the
/// mouse.  See the <see cref="MouseSelectionMode" /> property for details.
/// </para>
///
/// <para>
/// To programatically select and deselect vertices and edges, use the <see
/// cref="SetVertexSelected" />, <see cref="SetEdgeSelected" />, <see
/// cref="SetSelected" />, <see cref="SelectAll" />, and <see
/// cref="DeselectAll" /> methods.  To determine which vertices and edges are
/// selected, use the <see cref="SelectedVertices" /> and <see
/// cref="SelectedEdges" /> properties.
/// </para>
///
/// <para>
/// <b>Important Note:</b>: Do not use the <see
/// cref="ReservedMetadataKeys.IsSelected" /> metadata key to select vertex or
/// edges.  Use the selection methods on this control instead.
/// </para>
///
/// <h3>Zoom and Scale</h3>
///
/// <para>
/// The graph can be zoomed either programatically or with the mouse.  See
/// <see cref="GraphZoom" /> for details.
/// </para>
///
/// <para>
/// The size of the graph can controlled with <see cref="GraphScale" />.
/// </para>
///
/// <h3>Vertex Tooltips</h3>
///
/// <para>
/// A tooltip can be displayed when the mouse hovers over a vertex.  The
/// tooltip can be simple text or a custom UIElement containing arbitrary
/// content.  See <see cref="ShowVertexToolTips" /> for details.
/// </para>
///
/// <h3>Graph Layout Algorithm</h3>
///
/// <para>
/// By default, the control uses a force-directed Fruchterman-Reingold
/// algorithm to lay out the graph.  Use the <see cref="Layout" /> property to
/// specify a different layout.
/// </para>
///
/// <h3>Using NodeXLControl in WPF Applications</h3>
///
/// <example>
/// Here is sample C# code that populates a <see cref="NodeXLControl" /> graph
/// with several vertices and edges.  It's assumed that a <see
/// cref="NodeXLControl" /> named nodeXLControl1 has been added to the WPF
/// Window in the Visual Studio designer.
///
/// <code>
/**
using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization.Wpf;

namespace WpfApplication1
{
public partial class Window1 : Window
{
    public Window1()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        PopulateAndDrawGraph();
    }

    protected void PopulateAndDrawGraph()
    {
        // Get the graph's vertex collection.

        IVertexCollection oVertices = nodeXLControl1.Graph.Vertices;

        // Add three vertices.

        IVertex oVertexA = oVertices.Add();
        IVertex oVertexB = oVertices.Add();
        IVertex oVertexC = oVertices.Add();

        // Change the color, radius, and shape of vertex A.

        oVertexB.SetValue(ReservedMetadataKeys.PerColor,
            Color.FromArgb(255, 255, 0, 255));

        oVertexB.SetValue(ReservedMetadataKeys.PerVertexRadius, 20F);

        oVertexB.SetValue(ReservedMetadataKeys.PerVertexShape,
            VertexShape.Sphere);

        // Draw vertex B as a primary label instead of a shape.  A primary
        // label is a rectangle containing text.

        oVertexA.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabel,
            "Primary Label");

        // Set the primary label's text and fill colors.

        oVertexA.SetValue(ReservedMetadataKeys.PerColor,
            Color.FromArgb(255, 220, 220, 220));

        oVertexA.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabelFillColor,
            Color.FromArgb(255, 0, 0, 0));

        // Add a secondary label to vertex C.  A secondary label is text that
        // is drawn outside the vertex.  It can be added to a shape, image, or
        // primary label.

        oVertexC.SetValue(ReservedMetadataKeys.PerVertexSecondaryLabel,
            "Secondary Label");

        // Get the graph's edge collection.

        IEdgeCollection oEdges = nodeXLControl1.Graph.Edges;

        // Connect the vertices with directed edges.

        IEdge oEdge1 = oEdges.Add(oVertexA, oVertexB, true);
        IEdge oEdge2 = oEdges.Add(oVertexB, oVertexC, true);
        IEdge oEdge3 = oEdges.Add(oVertexC, oVertexA, true);

        // Customize their appearance.

        oEdge1.SetValue(ReservedMetadataKeys.PerColor,
            Color.FromArgb(255, 55, 125, 98));

        oEdge1.SetValue(ReservedMetadataKeys.PerEdgeWidth, 3F);
        oEdge1.SetValue(ReservedMetadataKeys.PerEdgeLabel, "This is edge 1");

        oEdge2.SetValue(ReservedMetadataKeys.PerEdgeWidth, 5F);
        oEdge2.SetValue(ReservedMetadataKeys.PerEdgeLabel, "This is edge 2");

        oEdge3.SetValue(ReservedMetadataKeys.PerColor,
            Color.FromArgb(255, 0, 255, 0));

        nodeXLControl1.DrawGraph(true);
    }
}
}
*/
/// </code>
/// </example>
///
/// <h3>Using NodeXLControl in Windows Forms Applications</h3>
///
/// <example>
/// <see cref="NodeXLControl" /> can be used in Windows Forms applications by
/// embedding it within a Windows.Forms.Integration.ElementHost control, as in
/// the following sample code:
///
/// <code>
/**
using System;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization.Wpf;

namespace WindowsFormsApplication1
{
public partial class Form1 : Form
{
    private NodeXLControl nodeXLControl1;

    public Form1()
    {
        InitializeComponent();

        nodeXLControl1 = new NodeXLControl();
        elementHost1.Child = nodeXLControl1;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        PopulateAndDrawGraph();
    }

    protected void PopulateAndDrawGraph()
    {
        // Get the graph's vertex collection.

        IVertexCollection oVertices = nodeXLControl1.Graph.Vertices;

        // Add three vertices.

        IVertex oVertexA = oVertices.Add();
        IVertex oVertexB = oVertices.Add();
        IVertex oVertexC = oVertices.Add();

        // Change the color, radius, and shape of vertex A.

        oVertexB.SetValue(ReservedMetadataKeys.PerColor,
            Color.FromArgb(255, 255, 0, 255));

        oVertexB.SetValue(ReservedMetadataKeys.PerVertexRadius, 20F);

        oVertexB.SetValue(ReservedMetadataKeys.PerVertexShape,
            VertexShape.Sphere);

        // Draw vertex B as a primary label instead of a shape.  A primary
        // label is a rectangle containing text.

        oVertexA.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabel,
            "Primary Label");

        // Set the primary label's text and fill colors.

        oVertexA.SetValue(ReservedMetadataKeys.PerColor,
            Color.FromArgb(255, 220, 220, 220));

        oVertexA.SetValue(ReservedMetadataKeys.PerVertexPrimaryLabelFillColor,
            Color.FromArgb(255, 0, 0, 0));

        // Add a secondary label to vertex C.  A secondary label is text that
        // is drawn outside the vertex.  It can be added to a shape, image, or
        // primary label.

        oVertexC.SetValue(ReservedMetadataKeys.PerVertexSecondaryLabel,
            "Secondary Label");

        // Get the graph's edge collection.

        IEdgeCollection oEdges = nodeXLControl1.Graph.Edges;

        // Connect the vertices with directed edges.

        IEdge oEdge1 = oEdges.Add(oVertexA, oVertexB, true);
        IEdge oEdge2 = oEdges.Add(oVertexB, oVertexC, true);
        IEdge oEdge3 = oEdges.Add(oVertexC, oVertexA, true);

        // Customize their appearance.

        oEdge1.SetValue(ReservedMetadataKeys.PerColor,
            Color.FromArgb(255, 55, 125, 98));

        oEdge1.SetValue(ReservedMetadataKeys.PerEdgeWidth, 3F);
        oEdge1.SetValue(ReservedMetadataKeys.PerEdgeLabel, "This is edge 1");

        oEdge2.SetValue(ReservedMetadataKeys.PerEdgeWidth, 5F);
        oEdge2.SetValue(ReservedMetadataKeys.PerEdgeLabel, "This is edge 2");

        oEdge3.SetValue(ReservedMetadataKeys.PerColor,
            Color.FromArgb(255, 0, 255, 0));

        nodeXLControl1.DrawGraph(true);
    }
}
}
*/
/// </code>
/// </example>
///
/// <h3>Future Work</h3>
///
/// <para>
/// This is the first WPF version of the NodeXLControl.  It replaces a previous
/// Windows Forms implementation.  This version provides the functionality
/// needed by the NodeXL Excel Template project, but does not yet take
/// advantage of WPF features such as dependency properties, routed events, and
/// so on.  Additional WPF features may be added in future versions, depending
/// on resource availability and how much demand there is for them.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class NodeXLControl : FrameworkElement
{
    //*************************************************************************
    //  Static constructor: NodeXLControl()
    //
    /// <summary>
    /// Static constructor for the <see cref="NodeXLControl" /> class.
    /// </summary>
    //*************************************************************************

    static NodeXLControl()
    {
        // If this isn't done, the Keyboard class can't be used to detect which
        // keys are pressed.

        FocusableProperty.OverrideMetadata( typeof(NodeXLControl),
            new UIPropertyMetadata(true) );
    }

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
        m_oGraphDrawer = new GraphDrawer(this);

        m_oAsyncLayout = new FruchtermanReingoldLayout();
        ConnectAsyncLayoutEvents(m_oAsyncLayout);

        m_oLastLayoutContext =
            new LayoutContext(System.Drawing.Rectangle.Empty);

        m_oLastGraphDrawingContext = null;

        m_eLayoutState = LayoutState.Stable;

        m_eMouseSelectionMode =
            MouseSelectionMode.SelectVertexAndIncidentEdges;

        m_bAllowVertexDrag = true;

        m_oVerticesBeingDragged = null;
        m_oMarqueeBeingDragged = null;
        m_oTranslationBeingDragged = null;

        m_oSelectedVertices = new Dictionary<IVertex, Byte>();
        m_oSelectedEdges = new Dictionary<IEdge, Byte>();

        m_bShowVertexToolTips = false;
        m_oLastMouseMoveLocation = new Point(-1, -1);

        // Create a helper object for displaying vertex tooltips.

        CreateVertexToolTipTracker();
        m_oVertexToolTip = null;

        m_bGraphZoomCentered = false;

        this.AddLogicalChild(m_oGraphDrawer.VisualCollection);

        CreateTransforms();

        // Prevent a focus rectangle from being drawn around the control when
        // it captures keyboard focus.  The focus rectangle does not behave
        // properly when the layout and render transforms are applied --
        // sometimes the rectangle disappears, and sometimes it gets magnified
        // by the render layout.

        this.FocusVisualStyle = null;

        // AssertValid();
    }

    #if false 
    //*************************************************************************
    //  Destructor: NodeXLControl()
    //
    /// <summary>
    /// Destructor forthe <see cref="NodeXLControl" /> class.
    /// </summary>
    //*************************************************************************

    ~NodeXLControl()
    {
        // TODO: This is wrong.  Where should ToolTipTracker.Dispose() be
        // called from?

        // Prevent ToolTipTracker's timer-based events from firing after the
        // control no longer has a handle.

        if (m_oVertexToolTipTracker != null)
        {
            m_oVertexToolTipTracker.Dispose();
        }
    }
    #endif

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
    /// After the graph is populated or modified, you must call <see
    /// cref="DrawGraph(Boolean)" /> to draw it.
    ///
    /// <para>
    /// An exception is thrown if this property is set while an asynchronous
    /// drawing is in progress.  Check <see cref="IsDrawing" /> before using
    /// this property.
    /// </para>
    ///
    /// <para>
    /// Do not set this property to a graph that is already owned by another
    /// graph drawer.  If you want to simultaneously draw the same graph with
    /// two different graph drawers, make a copy of the graph using
    /// IGraph.<see cref="IGraph.Clone(Boolean, Boolean)" />.
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
            CheckIfDrawing(PropertyName);

            DeselectAll();

            m_oGraph = value;

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
    /// The object to use to lay out the graph, as an <see
    /// cref="IAsyncLayout" />.  The default value is a <see
    /// cref="FruchtermanReingoldLayout" /> object.
    /// </value>
    ///
    /// <remarks>
    /// An exception is thrown if this property is set while an asynchronous
    /// drawing is in progress.  Check <see cref="IsDrawing" /> before using
    /// this property.
    ///
    /// <para>
    /// This property can be set to any object that implements <see
    /// cref="IAsyncLayout" />, whether it is provided by NodeXL or implemented
    /// by the application.  For a list of provided layout classes, see <see
    /// cref="AsyncLayoutBase" />.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <example>
    /// The example shows how to lay out the graph as a grid:
    ///
    /// <code>
    /// !Debug.Assert(nodeXLControl.IsDrawing);
    /// nodeXLControl.Layout = new GridLayout();
    /// </code>
    ///
    /// </example>
    //*************************************************************************

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.ReadOnly(true)]

    public IAsyncLayout
    Layout
    {
        get
        {
            AssertValid();

            return (m_oAsyncLayout);
        }

        set
        {
            const String PropertyName = "AsyncLayout";

            this.ArgumentChecker.CheckPropertyNotNull(PropertyName, value);
            CheckIfDrawing(PropertyName);

            m_oAsyncLayout = value;
            ConnectAsyncLayoutEvents(m_oAsyncLayout);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: BackColor
    //
    /// <summary>
    /// Gets or sets the graph's background color.
    /// </summary>
    ///
    /// <value>
    /// The graph's background color, as a <see
    /// cref="System.Windows.Media.Color" />.  The default value is
    /// SystemColors.<see cref="SystemColors.WindowColor" />.
    /// </value>
    //*************************************************************************

    public Color
    BackColor
    {
        get
        {
            AssertValid();

            return (m_oGraphDrawer.BackColor);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            m_oGraphDrawer.BackColor = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexShape
    //
    /// <summary>
    /// Gets or sets the default shape of the vertices.
    /// </summary>
    ///
    /// <value>
    /// The default shape of the vertices, as a <see cref="Wpf.VertexShape" />.
    /// The default value is <see cref="Wpf.VertexShape.Disk" />.
    /// </value>
    ///
    /// <remarks>
    /// The default shape of a vertex can be overridden by setting the <see
    /// cref="ReservedMetadataKeys.PerVertexShape" /> key on the vertex.
    /// </remarks>
    //*************************************************************************

    public VertexShape
    VertexShape
    {
        get
        {
            AssertValid();

            return (this.VertexDrawer.Shape);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.VertexDrawer.Shape = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexRadius
    //
    /// <summary>
    /// Gets or sets the default radius of the vertices.
    /// </summary>
    ///
    /// <value>
    /// The default radius of the vertices, as a <see cref="Double" />.  Must
    /// be between <see cref="Wpf.VertexDrawer.MinimumRadius" /> and <see
    /// cref="Wpf.VertexDrawer.MaximumRadius" />, inclusive.  The default value
    /// is 3.0.
    /// </value>
    ///
    /// <remarks>
    /// The default radius of a vertex can be overridden by setting the <see
    /// cref="ReservedMetadataKeys.PerVertexRadius" /> key on the vertex.
    /// </remarks>
    //*************************************************************************

    public Double
    VertexRadius
    {
        get
        {
            AssertValid();

            return (this.VertexDrawer.Radius);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.VertexDrawer.Radius = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexColor
    //
    /// <summary>
    /// Gets or sets the default color of unselected vertices.
    /// </summary>
    ///
    /// <value>
    /// The default color of unselected vertices, as a <see cref="Color" />.
    /// The default value is <see cref="SystemColors.WindowTextColor" />.
    /// </value>
    ///
    /// <remarks>
    /// The default color of an unselected vertex can be overridden by setting
    /// the <see cref="ReservedMetadataKeys.PerColor" /> key on the vertex.
    /// The key's value can be of type System.Drawing.Color or
    /// System.Windows.Media.Color.
    /// </remarks>
    ///
    /// <seealso cref="VertexSelectedColor" />
    //*************************************************************************

    public Color
    VertexColor
    {
        get
        {
            AssertValid();

            return (this.VertexDrawer.Color);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.VertexDrawer.Color = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexSelectedColor
    //
    /// <summary>
    /// Gets or sets the color of selected vertices.
    /// </summary>
    ///
    /// <value>
    /// The color of selected vertices, as a <see cref="Color" />.  The default
    /// value is <see cref="SystemColors.HighlightColor" />.
    /// </value>
    ///
    /// <remarks>
    /// The color of selected vertices cannot be set on a per-vertex basis.
    /// </remarks>
    ///
    /// <seealso cref="VertexColor" />
    //*************************************************************************

    public Color
    VertexSelectedColor
    {
        get
        {
            AssertValid();

            return (this.VertexDrawer.SelectedColor);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.VertexDrawer.SelectedColor = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexPrimaryLabelFillColor
    //
    /// <summary>
    /// Gets or sets the default fill color to use for vertices drawn as
    /// primary labels.
    /// </summary>
    ///
    /// <value>
    /// The default fill color to use for primary labels.  The default is
    /// SystemColors.WindowColor.
    /// </value>
    ///
    /// <remarks>
    /// <see cref="Color" /> is used for the primary label text and outline.
    ///
    /// <para>
    /// The default fill color of a vertex can be overridden by setting the
    /// <see cref="ReservedMetadataKeys.PerVertexPrimaryLabelFillColor" /> key
    /// on the vertex.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Color
    VertexPrimaryLabelFillColor
    {
        get
        {
            AssertValid();

            return (this.VertexDrawer.PrimaryLabelFillColor);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.VertexDrawer.PrimaryLabelFillColor = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeWidth
    //
    /// <summary>
    /// Gets or sets the default width of unselected edges.
    /// </summary>
    ///
    /// <value>
    /// The default width of unselected edges, as a <see cref="Double" />.
    /// Must be between <see cref="Wpf.EdgeDrawer.MinimumWidth" /> and <see
    /// cref="Wpf.EdgeDrawer.MaximumWidth" />, inclusive.  The default value
    /// is 1.
    /// </value>
    ///
    /// <remarks>
    /// The default width of an unselected edge can be overridden by setting
    /// the <see cref="ReservedMetadataKeys.PerEdgeWidth" /> key on the edge.
    /// </remarks>
    ///
    /// <seealso cref="EdgeSelectedWidth" />
    //*************************************************************************

    public Double
    EdgeWidth
    {
        get
        {
            AssertValid();

            return (this.EdgeDrawer.Width);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.EdgeDrawer.Width = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeSelectedWidth
    //
    /// <summary>
    /// Gets or sets the width of selected edges.
    /// </summary>
    ///
    /// <value>
    /// The width of selected edges, as a <see cref="Double" />.  Must be
    /// between <see cref="Wpf.EdgeDrawer.MinimumWidth" /> and <see
    /// cref="Wpf.EdgeDrawer.MaximumWidth" />, inclusive.  The default value
    /// is 2.
    /// </value>
    ///
    /// <remarks>
    /// The width of selected edges cannot be set on a per-edge basis.
    /// </remarks>
    ///
    /// <seealso cref="EdgeWidth" />
    //*************************************************************************

    public Double
    EdgeSelectedWidth
    {
        get
        {
            AssertValid();

            return (this.EdgeDrawer.SelectedWidth);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.EdgeDrawer.SelectedWidth = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeColor
    //
    /// <summary>
    /// Gets or sets the default color of unselected edges.
    /// </summary>
    ///
    /// <value>
    /// The default color of unselected edges, as a <see cref="Color" />.  The
    /// default value is <see cref="SystemColors.WindowTextColor" />.
    /// </value>
    ///
    /// <remarks>
    /// The default color of an unselected edge can be overridden by setting
    /// the <see cref="ReservedMetadataKeys.PerColor" /> key on the edge.  The
    /// key's value can be of type System.Drawing.Color or
    /// System.Windows.Media.Color.
    /// </remarks>
    ///
    /// <seealso cref="EdgeSelectedColor" />
    //*************************************************************************

    public Color
    EdgeColor
    {
        get
        {
            AssertValid();

            return (this.EdgeDrawer.Color);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.EdgeDrawer.Color = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeSelectedColor
    //
    /// <summary>
    /// Gets or sets the color of selected edges.
    /// </summary>
    ///
    /// <value>
    /// The color of selected edges, as a <see cref="Color" />.  The default
    /// value is <see cref="SystemColors.HighlightColor" />.
    /// </value>
    ///
    /// <remarks>
    /// The color of selected edges cannot be set on a per-vertex basis.
    /// </remarks>
    ///
    /// <seealso cref="EdgeColor" />
    //*************************************************************************

    public Color
    EdgeSelectedColor
    {
        get
        {
            AssertValid();

            return (this.EdgeDrawer.SelectedColor);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.EdgeDrawer.SelectedColor = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeRelativeArrowSize
    //
    /// <summary>
    /// Gets or sets the relative size of arrowheads on directed edges.
    /// </summary>
    ///
    /// <value>
    /// The relative size of arrowheads, as a <see cref="Double" />.  Must be
    /// between <see cref="Wpf.EdgeDrawer.MinimumRelativeArrowSize" /> and <see
    /// cref="Wpf.EdgeDrawer.MaximumRelativeArrowSize" />, inclusive.  The
    /// default value is 3.
    /// </value>
    ///
    /// <remarks>
    /// The value is relative to <see cref="EdgeWidth" /> and <see
    /// cref="EdgeSelectedWidth" />.  If the width or selected width is
    /// increased, the arrow size on unselected or selected edges is increased
    /// proportionally.
    /// </remarks>
    //*************************************************************************

    public Double
    EdgeRelativeArrowSize
    {
        get
        {
            AssertValid();

            return (this.EdgeDrawer.RelativeArrowSize);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.EdgeDrawer.RelativeArrowSize = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FilteredAlpha
    //
    /// <summary>
    /// Gets or sets the alpha value to use for vertices and edges that are
    /// filtered.
    /// </summary>
    ///
    /// <value>
    /// The alpha value to use for vertices and edges that have a <see
    /// cref="ReservedMetadataKeys.Visibility" /> value of <see
    /// cref="VisibilityKeyValue.Filtered" />.  Must be between 0 (invisible)
    /// and 255 (opaque).  The default value is 10.
    /// </value>
    //*************************************************************************

    public Byte
    FilteredAlpha
    {
        get
        {
            AssertValid();

            Debug.Assert(this.VertexDrawer.FilteredAlpha ==
                this.EdgeDrawer.FilteredAlpha);

            return (this.VertexDrawer.FilteredAlpha);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.VertexDrawer.FilteredAlpha = this.EdgeDrawer.FilteredAlpha =
                value;

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
    /// A <see cref="Visualization.Wpf.MouseSelectionMode" /> value.  The
    /// default value is <see cref="Visualization.Wpf.MouseSelectionMode.
    /// SelectVertexAndIncidentEdges" />.
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
    /// <list type="bullet">
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
    /// </list>
    ///
    /// <para>
    /// When this property is set to SelectNothing, clicking on a vertex
    /// results in the following sequence:
    /// </para>
    ///
    /// <list type="bullet">
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
    /// </list>
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
            // It is okay to change this property while the graph is being laid
            // out, because the mouse is ignored until graph drawing is
            // complete.

            m_eMouseSelectionMode = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: AllowVertexDrag
    //
    /// <summary>
    /// Gets or sets a flag indicating whether vertices can be moved by
    /// dragging them with the mouse.
    /// </summary>
    ///
    /// <value>
    /// true if vertices can be moved by dragging them with the mouse, false
    /// otherwise.  The default value is true.
    /// </value>
    ///
    /// <remarks>
    /// When this property is true, the user can move a single vertex by
    /// left-clicking it and dragging it with the mouse.  She can also select
    /// multiple vertices via multiple clicks or a marquee, then drag all the
    /// selected vertices by left-clicking one of them and dragging with the
    /// mouse.
    ///
    /// <para>
    /// The dragged vertices and their incident edges are redrawn, but no other
    /// vertices or edges are affected.
    /// </para>
    ///
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
            // It is okay to change this property while the graph is being laid
            // out, because the mouse is ignored until graph drawing is
            // complete.

            m_bAllowVertexDrag = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ShowVertexToolTips
    //
    /// <summary>
    /// Gets or sets a value indicating whether vertex tooltips should be
    /// shown.
    /// </summary>
    ///
    /// <value>
    /// true to show vertex tooltips.  The default value is false.
    /// </value>
    ///
    /// <remarks>
    /// A vertex tooltip is a tootip that appears when the mouse is hovered
    /// over a vertex.  Each vertex has its own tooltip.
    ///
    /// <para>
    /// To use simple text for tooltips, set <see cref="ShowVertexToolTips" />
    /// to true, then use Vertex.<see cref="IMetadataProvider.SetValue" /> to
    /// assign a tooltip string to each of the graph's vertices.  The key must
    /// be the reserved key ReservedMetadataKeys.<see
    /// cref="ReservedMetadataKeys.VertexToolTip" /> and the value must be the
    /// tooltip string for the vertex.
    /// </para>
    ///
    /// <para>
    /// To use a custom UIElement for tooltips instead of simple text, set <see
    /// cref="ShowVertexToolTips" /> to true, then handle the <see
    /// cref="PreviewVertexToolTipShown" /> event.  In your event handler, set
    /// the event argument's <see
    /// cref="VertexToolTipShownEventArgs.VertexToolTip" /> to a UIElement that
    /// you create.  You can use the event argument's <see
    /// cref="VertexEventArgs.Vertex" /> property to customize the UIElement
    /// based on which vertex was hovered over.
    /// </para>
    ///
    /// <para>
    /// The <see cref="VertexMouseHover" /> and <see cref="VertexMouseLeave" />
    /// events fires regardless of whether vertex tooltips are shown.
    /// </para>
    ///
    /// <para>
    /// Note that vertex tooltips are entirely independent of the standard
    /// tooltip exposed by FrameworkElement.ToolTip.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="PreviewVertexToolTipShown" />
    /// <seealso cref="VertexToolTipShownEventArgs" />
    //*************************************************************************

    [Category("Mouse")]

    public Boolean
    ShowVertexToolTips
    {
        get
        {
            AssertValid();

            return (m_bShowVertexToolTips);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the mouse is ignored until graph drawing is
            // complete.

            if (!value)
            {
                // Remove any tooltip that might exist and reset the helper
                // object that figures out when to show tooltips.

                ResetVertexToolTipTracker();
            }

            m_bShowVertexToolTips = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: GraphScale
    //
    /// <summary>
    /// Gets or sets a value that determines the size of the graph.
    /// </summary>
    ///
    /// <value>
    /// A value that determines the size of the graph.  Must be between <see
    /// cref="MinimumGraphScale" /> and <see cref="MaximumGraphScale" />.  The
    /// default value is 1.0.
    /// </value>
    ///
    /// <remarks>
    /// If this is left at its default value of 1.0, the graph is laid out and
    /// rendered within a rectangle that is the same size as the control.  If
    /// it is set to 2.0, for example, the graph is laid out within a rectangle
    /// whose sides are twice as long as the sides of the control, but the
    /// laid out graph is shrunk to fit the control.  The result is that the
    /// graph fits within the control but all its vertices and edges are
    /// smaller than normal.
    /// </remarks>
    //*************************************************************************

    [Category("View")]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.ReadOnly(true)]

    public Double
    GraphScale
    {
        get
        {
            AssertValid();

            ScaleTransform oScaleTransformForLayout =
                this.ScaleTransformForLayout;

            Debug.Assert(oScaleTransformForLayout.ScaleX ==
                oScaleTransformForLayout.ScaleY);

            return (1.0 / oScaleTransformForLayout.ScaleX);
        }

        set
        {
            const String PropertyName = "GraphScale";

            this.ArgumentChecker.CheckPropertyInRange(PropertyName, value,
                MinimumGraphScale, MaximumGraphScale);

            // See CreateTransforms() for details on how the graph scale works.

            ScaleTransform oScaleTransformForLayout =
                this.ScaleTransformForLayout;

            oScaleTransformForLayout.ScaleX =
                oScaleTransformForLayout.ScaleY = 1.0 / value;

            // Note that changing the layout transform results in
            // OnRenderSizeChanged() getting called.

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: GraphZoom
    //
    /// <summary>
    /// Gets or sets a value that determines the zoom level of the graph.
    /// </summary>
    ///
    /// <value>
    /// A value that determines the zoom level of the graph.  Must be between
    /// <see cref="MinimumGraphZoom" /> and <see cref="MaximumGraphZoom" />.
    /// The default value is 1.0.
    /// </value>
    ///
    /// <remarks>
    /// This property gets set automatically when the user spins the mouse
    /// wheel.
    ///
    /// <para>
    /// The <see cref="GraphZoomChanged" /> event fires when this property is
    /// changed, either programatically or with the mouse wheel.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    [Category("View")]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.ReadOnly(true)]

    public Double
    GraphZoom
    {
        get
        {
            AssertValid();

            ScaleTransform oScaleTransformForRender =
                this.ScaleTransformForRender;

            Debug.Assert(oScaleTransformForRender.ScaleX ==
                oScaleTransformForRender.ScaleY);

            return (oScaleTransformForRender.ScaleX);
        }

        set
        {
            const String PropertyName = "GraphZoom";

            this.ArgumentChecker.CheckPropertyInRange(PropertyName, value,
                MinimumGraphZoom, MaximumGraphZoom);

            SetGraphZoom(value, true);

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
    ///
    /// <para>
    /// The returned array should be considered read-only.  To select a vertex,
    /// use <see cref="SetVertexSelected" /> or a related method.
    /// </para>
    ///
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

            return ( CollectionUtil.DictionaryKeysToArray<IVertex, Byte>(
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
    ///
    /// <para>
    /// The returned array should be considered read-only.  To select an edge,
    /// use <see cref="SetEdgeSelected" /> or a related method.
    /// </para>
    ///
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

            return ( CollectionUtil.DictionaryKeysToArray<IEdge, Byte>(
                m_oSelectedEdges) );
        }
    }

    //*************************************************************************
    //  Property: IsDrawing
    //
    /// <summary>
    /// Gets a value indicating whether the graph is being drawn.
    /// </summary>
    ///
    /// <value>
    /// true if the graph is being drawn.
    /// </value>
    ///
    /// <remarks>
    /// Graph drawing, which may include an interative graph layout stage,
    /// occurs asynchronously after <see cref="DrawGraph()" /> is called. 
    /// Several properties and methods, such as <see cref="Graph" />, <see
    /// cref="Layout" />, and <see cref="SetSelected" />, cannot be accessed
    /// while the graph is being drawn.  Check <see cref="IsDrawing" /> or
    /// monitor the <see cref="DrawingGraph" /> and <see cref="GraphDrawn" />
    /// events before accessing those properties and methods.
    ///
    /// <para>
    /// The <see cref="DrawingGraph" /> event fires before graph drawing
    /// begins.  The <see cref="GraphDrawn" /> event fires after graph drawing
    /// completes.
    /// </para>
    ///
    /// <para>
    /// Typically, an application will populate and draw the graph in the
    /// load event of the Window or Form, and use the <see
    /// cref="DrawingGraph" /> and <see cref="GraphDrawn" /> events to disable
    /// and enable any controls that might be used to redraw the graph.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    IsDrawing
    {
        get
        {
            AssertValid();

            return (m_eLayoutState != LayoutState.Stable);
        }
    }

    //*************************************************************************
    //  Property: GraphDrawer
    //
    /// <summary>
    /// Do not use this property.  It is for internal use only.
    /// </summary>
    ///
    /// <value>
    /// Do not use this property.  It is for internal use only.
    /// </value>
    //*************************************************************************

    public GraphDrawer
    GraphDrawer
    {
        // This is for use by
        // ExcelTemplate.GeneralUserSettings.TransferToNodeXLControl().

        get
        {
            AssertValid();

            return (m_oGraphDrawer);
        }
    }

    //*************************************************************************
    //  Method: SetFont()
    //
    /// <summary>
    /// Sets the font used to draw primary and secondary labels.
    /// </summary>
    ///
    /// <param name="typeface">
    /// The Typeface to use.
    /// </param>
    ///
    /// <param name="emSize">
    /// The font size to use, in ems.
    /// </param>
    ///
    /// <remarks>
    /// The default font is the SystemFonts.MessageFontFamily at size 10.
    /// </remarks>
    //*************************************************************************

    public void
    SetFont
    (
        Typeface typeface,
        Double emSize
    )
    {
        AssertValid();

        // It is okay to change the font while the graph is being laid out,
        // because the font is not used until OnRender() is called.

        this.VertexDrawer.SetFont(typeface, emSize);
    }

    //*************************************************************************
    //  Method: SetVertexSelected()
    //
    /// <summary>
    /// Selects or deselects a vertex.
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
    /// <para>
    /// An exception is thrown if the graph is being drawn when this method is
    /// called.  Check the <see cref="IsDrawing" /> property before calling
    /// this.
    /// </para>
    ///
    /// <para>
    /// <b>Important Note:</b>
    /// </para>
    ///
    /// <para>
    /// Do not use the <see cref="ReservedMetadataKeys.IsSelected" /> key to
    /// select vertex or edges.  Use the selection methods on this control
    /// instead.
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

        // The reason that vertices and edges can't be selected while the graph
        // is being drawn is that selecting them edges involves modifying their
        // metadata.  The graph layout code, which runs on a worker thread, is
        // allowed to modify metadata during the layout, so having the worker
        // thread and this foreground thread modify metadata simultaneously
        // would lead to synchronization clashes.
        //
        // A solution would be to make the metadata implementation thread-safe.

        CheckIfDrawing(MethodName);

        // Update the selected state of the vertex and its incident edges.

        SetVertexSelectedInternal(vertex, selected);

        if (alsoIncidentEdges)
        {
            foreach (IEdge oEdge in vertex.IncidentEdges)
            {
                SetEdgeSelectedInternal(oEdge, selected);
            }
        }

        FireSelectionChanged();
    }

    //*************************************************************************
    //  Method: SetEdgeSelected()
    //
    /// <summary>
    /// Selects or deselects an edge.
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
    /// <para>
    /// An exception is thrown if the graph is being drawn when this method is
    /// called.  Check the <see cref="IsDrawing" /> property before calling
    /// this.
    /// </para>
    ///
    /// <para>
    /// <b>Important Note:</b>
    /// </para>
    ///
    /// <para>
    /// Do not use the <see cref="ReservedMetadataKeys.IsSelected" /> key to
    /// select vertex or edges.  Use the selection methods on this control
    /// instead.
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
        CheckIfDrawing(MethodName);

        // Update the selected state of the edge and its adjacent vertices.

        SetEdgeSelectedInternal(edge, selected);

        if (alsoAdjacentVertices)
        {
            SetVertexSelectedInternal(edge.Vertices[0], selected);
            SetVertexSelectedInternal(edge.Vertices[1], selected);
        }

        FireSelectionChanged();
    }

    //*************************************************************************
    //  Method: SetSelected()
    //
    /// <summary>
    /// Selects a set of vertices and edges.
    /// </summary>
    ///
    /// <param name="vertices">
    /// Collection of zero or more vertices to select.
    /// </param>
    ///
    /// <param name="edges">
    /// Collection of zero or more edges to select.
    /// </param>
    ///
    /// <remarks>
    /// This method deselects any selected vertices and edges, then selects the
    /// vertices and edges specified in <paramref name="vertices" /> and
    /// <paramref name="edges" />.  It is more efficient than making multiple
    /// calls to <see cref="SetVertexSelected" /> and <see
    /// cref="SetEdgeSelected" />.
    ///
    /// <para>
    /// An exception is thrown if the graph is being drawn when this method is
    /// called.  Check the <see cref="IsDrawing" /> property before calling
    /// this.
    /// </para>
    ///
    /// <para>
    /// <b>Important Note:</b>
    /// </para>
    ///
    /// <para>
    /// Do not use the <see cref="ReservedMetadataKeys.IsSelected" /> key to
    /// select vertex or edges.  Use the selection methods on this control
    /// instead.
    /// </para>
    ///
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
        IEnumerable<IVertex> vertices,
        IEnumerable<IEdge> edges
    )
    {
        AssertValid();

        const String MethodName = "SetSelected";

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, "vertices", vertices);

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, "edges", edges);

        CheckIfDrawing(MethodName);

        // Clear the selection.

        SetAllVerticesSelected(false);
        SetAllEdgesSelected(false);

        // Update the selected state of the specified vertices and edges.

        foreach (IVertex oVertex in vertices)
        {
            SetVertexSelectedInternal(oVertex, true);
        }

        foreach (IEdge oEdge in edges)
        {
            SetEdgeSelectedInternal(oEdge, true);
        }

        FireSelectionChanged();
    }

    //*************************************************************************
    //  Method: SelectAll()
    //
    /// <summary>
    /// Selects all vertices and edges.
    /// </summary>
    ///
    /// <remarks>
    /// An exception is thrown if the graph is being drawn when this method is
    /// called.  Check the <see cref="IsDrawing" /> property before calling
    /// this.
    ///
    /// <para>
    /// <b>Important Note:</b>
    /// </para>
    ///
    /// <para>
    /// Do not use the <see cref="ReservedMetadataKeys.IsSelected" /> key to
    /// select vertex or edges.  Use the selection methods on this control
    /// instead.
    /// </para>
    ///
    /// </remarks>
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

        CheckIfDrawing(MethodName);

        SetAllSelected(true);
    }

    //*************************************************************************
    //  Method: DeselectAll()
    //
    /// <summary>
    /// Deselects all vertices and edges.
    /// </summary>
    ///
    /// <remarks>
    /// An exception is thrown if the graph is being drawn when this method is
    /// called.  Check the <see cref="IsDrawing" /> property before calling
    /// this.
    ///
    /// <para>
    /// <b>Important Note:</b>
    /// </para>
    ///
    /// <para>
    /// Do not use the <see cref="ReservedMetadataKeys.IsSelected" /> key to
    /// select vertex or edges.  Use the selection methods on this control
    /// instead.
    /// </para>
    ///
    /// </remarks>
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

        CheckIfDrawing(MethodName);

        // Do nothing if nothing is selected.

        if (m_oSelectedVertices.Count == 0 && m_oSelectedEdges.Count == 0)
        {
            return;
        }

        SetAllSelected(false);
    }

    //*************************************************************************
    //  Method: TryGetVertexFromPoint()
    //
    /// <summary>
    /// Attempts to get the vertex containing a specified <see cref="Point" />.
    /// </summary>
    ///
    /// <param name="point">
    /// Point to get a vertex for.
    /// </param>
    ///
    /// <param name="vertex">
    /// Where the <see cref="IVertex" /> object gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if a vertex containing the point was found, false if not.
    /// </returns>
    ///
    /// <remarks>
    /// This method looks for a vertex that contains <paramref name="point" />.
    /// If there is such a vertex, the vertex is stored at <paramref
    /// name="vertex" /> and true is returned.  Otherwise, <paramref
    /// name="vertex" /> is set to null and false is returned.
    ///
    /// <para>
    /// false is returned if an asynchronous drawing is in progress.  Check
    /// <see cref="IsDrawing" /> before calling this method.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryGetVertexFromPoint
    (
        Point point,
        out IVertex vertex
    )
    {
        AssertValid();

        vertex = null;

        if (this.IsDrawing)
        {
            return false;
        }

        return ( m_oGraphDrawer.TryGetVertexFromPoint(point, out vertex) );
    }

    //*************************************************************************
    //  Method: CopyGraphToBitmap()
    //
    /// <summary>
    /// Creates a bitmap image of the graph.
    /// </summary>
    ///
    /// <param name="bitmapWidthPx">
    /// Width of the bitmap image, in pixels.  Must be greater than 0.
    /// </param>
    ///
    /// <param name="bitmapHeightPx">
    /// Height of the bitmap image, in pixels.  Must be greater than 0.
    /// </param>
    ///
    /// <returns>
    /// A bitmap image of the graph displayed within the control, with the
    /// specified dimensions.
    /// </returns>
    ///
    /// <remarks>
    /// An exception is thrown if the graph is being drawn when this method is
    /// called.  Check the <see cref="IsDrawing" /> property before calling
    /// this.
    /// </remarks>
    //*************************************************************************

    public System.Drawing.Bitmap
    CopyGraphToBitmap
    (
        Int32 bitmapWidthPx,
        Int32 bitmapHeightPx
    )
    {
        AssertValid();

        const String MethodName = "CopyGraphToBitmap";

        this.ArgumentChecker.CheckArgumentPositive(MethodName, "bitmapWidthPx",
            bitmapWidthPx);

        this.ArgumentChecker.CheckArgumentPositive(MethodName, "bitmapHeightPx",
            bitmapHeightPx);

        CheckIfDrawing(MethodName);

        // Save the current vertex locations.

        LayoutSaver oLayoutSaver = new LayoutSaver(this.Graph);

        // Adjust the control's transforms so that the image will be centered
        // on the same point on the graph that the control is centered on.

        GraphImageCenterer oGraphImageCenterer = new GraphImageCenterer(this);

        oGraphImageCenterer.CenterGraphImage(
            new Size(bitmapWidthPx, bitmapHeightPx) );

        // Transform the graph's layout to the specified size.

        Double dOriginalActualWidth = this.ActualWidth;
        Double dOriginalActualHeight = this.ActualHeight;

        ScaleTransform oScaleTransformForLayout = this.ScaleTransformForLayout;

        Rect oBitmapRectangle = new Rect(0, 0,
            (Double)bitmapWidthPx / oScaleTransformForLayout.ScaleX,
            (Double)bitmapHeightPx / oScaleTransformForLayout.ScaleY
            );

        TransformLayout(oBitmapRectangle);

        Debug.Assert(m_eLayoutState == LayoutState.Stable);

        DrawGraph(oBitmapRectangle);

        System.Drawing.Bitmap oBitmap = WpfGraphicsUtil.VisualToBitmap(this,
            bitmapWidthPx, bitmapHeightPx);

        // Restore the original layout.
        //
        // NOTE:
        //
        // Don't try calling TransformLayout() again using the original
        // rectangle.  The first call to TransformLayout() lost "resolution" if
        // the layout was transformed to a smaller rectangle, and attempting to
        // reverse the transform will yield poor results.

        oLayoutSaver.RestoreLayout();

        oBitmapRectangle =
            new Rect(0, 0, dOriginalActualWidth, dOriginalActualHeight);

        Debug.Assert(m_eLayoutState == LayoutState.Stable);

        DrawGraph(oBitmapRectangle);

        oGraphImageCenterer.RestoreCenter();

        return (oBitmap);
    }

    //*************************************************************************
    //  Method: DrawGraph()
    //
    /// <overloads>
    /// Draws the graph.
    /// </overloads>
    ///
    /// <summary>
    /// Draws the graph without laying it out first.
    /// </summary>
    ///
    /// <remarks>
    /// Graph drawing occurs asynchronously after this method is called.  See
    /// the <see cref="IsDrawing" /> property for details.
    ///
    /// <para>
    /// If the graph is currently being drawn, this method does nothing.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    DrawGraph()
    {
        AssertValid();

        DrawGraph(false);
    }

    //*************************************************************************
    //  Method: DrawGraph()
    //
    /// <summary>
    /// Draws the graph after optionally laying it out.
    /// </summary>
    ///
    /// <param name="layOutGraphFirst">
    /// If true, the graph is laid out again before it is drawn.  If false, the
    /// graph is drawn using the current vertex locations.
    /// </param>
    ///
    /// <remarks>
    /// Graph drawing occurs asynchronously after this method is called.  See
    /// the <see cref="IsDrawing" /> property for details.
    ///
    /// <para>
    /// If the graph is currently being drawn, this method does nothing.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    DrawGraph
    (
        Boolean layOutGraphFirst
    )
    {
        AssertValid();

        if (this.IsDrawing)
        {
            return;
        }

        if (layOutGraphFirst)
        {
            m_eLayoutState = LayoutState.LayoutRequired;

            // Don't just call LayOutOrDrawGraph() here, as is done in the else
            // clause.  Assuming that this method is being called at
            // application startup time, the control has not yet gone through
            // its measure cycle.  If LayOutOrDrawGraph() were called instead
            // of InvalidateVisual(), the actual width and height of this
            // control would be zero when the layout begins, and the graph
            // wouldn't get laid out properly.

            this.InvalidateVisual();
        }
        else
        {
            m_eLayoutState = LayoutState.LayoutCompleted;
            LayOutOrDrawGraph();
        }
    }

    //*************************************************************************
    //  Method: ClearGraph()
    //
    /// <summary>
    /// Clears the control's graph.
    /// </summary>
    ///
    /// <remarks>
    /// This method discards the control's graph, including all of its vertices
    /// and edges, and replaces it with a new, empty graph.  Any selection is
    /// also cleared.
    ///
    /// <para>
    /// This should be used instead of clearing the current graph's vertex and
    /// edge collections, which can cause unpredictable side effects.
    /// </para>
    ///
    /// <para>
    /// An exception is thrown if the graph is being drawn when this method is
    /// called.  Check the <see cref="IsDrawing" /> property before calling
    /// this.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    ClearGraph()
    {
        AssertValid();

        this.Graph = new Graph();
    }

    //*************************************************************************
    //  Event: SelectionChanged
    //
    /// <summary>
    /// Occurs when the selection state of a vertex or edge changes.
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
    //  Event: GraphMouseDown
    //
    /// <summary>
    /// Occurs when the mouse pointer is within the graph and a mouse button
    /// is pressed.
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

    public event GraphMouseButtonEventHandler GraphMouseDown;


    //*************************************************************************
    //  Event: GraphMouseUp
    //
    /// <summary>
    /// Occurs when the mouse pointer is within the graph and a mouse button
    /// is released.
    /// </summary>
    ///
    /// <seealso cref="MouseSelectionMode" />
    /// <seealso cref="SelectedVertices" />
    //*************************************************************************

    [Category("Mouse")]

    public event GraphMouseButtonEventHandler GraphMouseUp;


    //*************************************************************************
    //  Event: VertexClick
    //
    /// <summary>
    /// Occurs when a vertex is clicked.
    /// </summary>
    ///
    /// <remarks>
    /// In your event handler, do not change the selected state of the clicked
    /// vertex.  That happens automatically.  An exception is thrown if you
    /// attempt to do this.
    /// </remarks>
    ///
    /// <seealso cref="MouseSelectionMode" />
    /// <seealso cref="SelectedVertices" />
    //*************************************************************************

    [Category("Mouse")]

    public event VertexEventHandler VertexClick;


    //*************************************************************************
    //  Event: VertexDoubleClick
    //
    /// <summary>
    /// Occurs when a vertex is double-clicked.
    /// </summary>
    ///
    /// <seealso cref="MouseSelectionMode" />
    /// <seealso cref="SelectedVertices" />
    //*************************************************************************

    [Category("Mouse")]

    public event VertexEventHandler VertexDoubleClick;


    //*************************************************************************
    //  Event: VertexMouseHover
    //
    /// <summary>
    /// Occurs when the mouse pointer hovers over a vertex.
    /// </summary>
    ///
    /// <remarks>
    /// This event occurs when the mouse pointer hovers over a vertex.  If the
    /// mouse is moved to another vertex, this event fires again.  If the mouse
    /// is left hovering over the vertex for a predetermined period or is moved
    /// away from the vertex, a <see cref="VertexMouseLeave" /> event occurs.
    ///
    /// <para>
    /// If <see cref="ShowVertexToolTips" /> is true, hovering the mouse over a
    /// vertex causes the <see cref="VertexMouseHover" /> event to fire,
    /// followed by <see cref="PreviewVertexToolTipShown" />.  If <see
    /// cref="ShowVertexToolTips" /> is false, only the <see
    /// cref="VertexMouseHover" /> event fires.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="VertexMouseLeave" />
    //*************************************************************************

    [Category("Mouse")]

    public event VertexEventHandler VertexMouseHover;


    //*************************************************************************
    //  Event: VertexMouseLeave
    //
    /// <summary>
    /// Occurs when the mouse pointer is left hovering over a vertex for a
    /// predetermined period or is moved away from all vertices.
    /// </summary>
    ///
    /// <remarks>
    /// Hovering the mouse over a vertex causes the <see
    /// cref="VertexMouseHover" /> event to fire.  If the mouse is left
    /// hovering over the vertex for a predetermined period or is moved away
    /// from the vertex, the <see cref="VertexMouseLeave" /> event fires.
    /// </remarks>
    ///
    /// <seealso cref="VertexMouseHover" />
    //*************************************************************************

    [Category("Mouse")]

    public event EventHandler VertexMouseLeave;


    //*************************************************************************
    //  Event: PreviewVertexToolTipShown
    //
    /// <summary>
    /// Occurs when the mouse pointer hovers over a vertex but before a vertex
    /// tooltip is shown.
    /// </summary>
    ///
    /// <remarks>
    /// See <see cref="ShowVertexToolTips" /> for information on how to show
    /// and customize vertex tooltips.
    /// </remarks>
    //*************************************************************************

    [Category("Mouse")]

    public event VertexToolTipShownEventHandler PreviewVertexToolTipShown;


    //*************************************************************************
    //  Event: GraphZoomChanged
    //
    /// <summary>
    /// Occurs when the <see cref="GraphZoom" /> property is changed.
    /// </summary>
    ///
    /// <remarks>
    /// This event occurs when <see cref="GraphZoom" /> is changed
    /// programatically or with the mouse wheel.
    /// </remarks>
    //*************************************************************************

    [Category("Mouse")]

    public event EventHandler GraphZoomChanged;


    //*************************************************************************
    //  Event: GraphTranslationChanged
    //
    /// <summary>
    /// Occurs when the graph is moved with the mouse.
    /// </summary>
    //*************************************************************************

    [Category("Mouse")]

    public event EventHandler GraphTranslationChanged;


    //*************************************************************************
    //  Event: DrawingGraph
    //
    /// <summary>
    /// Occurs before graph drawing begins.
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
    //  Event: GraphDrawn
    //
    /// <summary>
    /// Occurs after graph drawing completes.
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
    //  Event: VerticesMoved
    //
    /// <summary>
    /// Occurs after one or more vertices are moved to a new location with the
    /// mouse.
    /// </summary>
    ///
    /// <remarks>
    /// This event is fired when the user releases the mouse button after
    /// dragging one or more vertices to a new location.
    /// </remarks>
    //*************************************************************************

    [Category("Action")]

    public event VerticesMovedEventHandler VerticesMoved;


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
        /// The graph is empty, or it's layout is complete and it has been
        /// drawn.
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
        /// graph needs to be drawn.
        /// </summary>

        LayoutIterationCompleted,

        /// <summary>
        /// The asynchronous layout has completed and now the graph needs to be
        /// drawn.
        /// </summary>

        LayoutCompleted,

        /// <summary>
        /// Same as Stable, but the control has been resized and now the
        /// graph's layout needs to be transformed to the new size.
        /// </summary>

        TransformRequired,
    }

    //*************************************************************************
    //  Property: VertexDrawer
    //
    /// <summary>
    /// Gets the <see cref="Wpf.VertexDrawer" /> used to draw the graph's
    /// vertices.
    /// </summary>
    ///
    /// <value>
    /// The <see cref="Wpf.VertexDrawer" /> used to draw the graph's vertices.
    /// </value>
    //*************************************************************************

    protected VertexDrawer
    VertexDrawer
    {
        get
        {
            AssertValid();

            return (m_oGraphDrawer.VertexDrawer);
        }
    }

    //*************************************************************************
    //  Property: EdgeDrawer
    //
    /// <summary>
    /// Gets the <see cref="Wpf.EdgeDrawer" /> used to draw the graph's edges.
    /// </summary>
    ///
    /// <value>
    /// The <see cref="Wpf.EdgeDrawer" /> used to draw the graph's edges.
    /// </value>
    //*************************************************************************

    protected EdgeDrawer
    EdgeDrawer
    {
        get
        {
            AssertValid();

            return (m_oGraphDrawer.EdgeDrawer);
        }
    }

    //*************************************************************************
    //  Property: GraphRectangle
    //
    /// <summary>
    /// Gets the rectangle that defines the bounds of the graph.
    /// </summary>
    ///
    /// <value>
    /// The rectangle that defines the bounds of the graph.
    /// </value>
    ///
    /// <remarks>
    /// The rectangle's dimensions are not affected by either of the transforms
    /// used for the control's render transform, <see
    /// cref="ScaleTransformForRender" /> or <see
    /// cref="TranslateTransformForRender" />.  The ARE affected by <see
    /// cref="ScaleTransformForLayout" />, however.  See <see
    /// cref="CreateTransforms" /> for details. 
    /// </remarks>
    //*************************************************************************

    protected Rect
    GraphRectangle
    {
        get
        {
            return (
                new Rect( new Size(this.ActualWidth, this.ActualHeight) ) );
        }
    }

    //*************************************************************************
    //  Property: ScaleTransformForLayout
    //
    /// <summary>
    /// Gets the ScaleTransform used for the control's layout transform.
    /// </summary>
    ///
    /// <value>
    /// A ScaleTransform object that controls the size of the graph's canvas.
    /// </value>
    //*************************************************************************

    protected ScaleTransform
    ScaleTransformForLayout
    {
        get
        {
            AssertValid();

            Debug.Assert(this.LayoutTransform is ScaleTransform);

            return ( (ScaleTransform)this.LayoutTransform );
        }
    }

    //*************************************************************************
    //  Property: ScaleTransformForRender
    //
    /// <summary>
    /// Gets the ScaleTransform used for the control's render transform.
    /// </summary>
    ///
    /// <value>
    /// A ScaleTransform object that controls the graph's zoom.
    /// </value>
    //*************************************************************************

    protected ScaleTransform
    ScaleTransformForRender
    {
        get
        {
            AssertValid();

            Debug.Assert(this.RenderTransform is TransformGroup);

            TransformGroup oTransformGroup =
                (TransformGroup)this.RenderTransform;

            Debug.Assert(oTransformGroup.Children.Count == 2);
            Debug.Assert(oTransformGroup.Children[0] is ScaleTransform);

            return ( (ScaleTransform)oTransformGroup.Children[0] );
        }
    }

    //*************************************************************************
    //  Property: TranslateTransformForRender
    //
    /// <summary>
    /// Gets the TranslateTransform used for the control's render transform.
    /// </summary>
    ///
    /// <value>
    /// A TranslateTransform object that controls the position of the graph.
    /// </value>
    //*************************************************************************

    protected TranslateTransform
    TranslateTransformForRender
    {
        get
        {
            AssertValid();

            Debug.Assert(this.RenderTransform is TransformGroup);

            TransformGroup oTransformGroup =
                (TransformGroup)this.RenderTransform;

            Debug.Assert(oTransformGroup.Children.Count == 2);
            Debug.Assert(oTransformGroup.Children[1] is TranslateTransform);

            return ( (TranslateTransform)oTransformGroup.Children[1] );
        }
    }

    //*************************************************************************
    //  Property: ClassName
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
    //  Property: ArgumentChecker
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
    //  Property: VisualChildrenCount
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
            return (m_oGraphDrawer.VisualCollection.Count);
        }
    }

    //*************************************************************************
    //  Method: ConnectAsyncLayoutEvents()
    //
    /// <summary>
    /// Connects event handlers to an <see cref="IAsyncLayout" /> object's
    /// events.
    /// </summary>
    ///
    /// <param name="oAsyncLayout">
    /// Object whose events need to be handled.
    /// </param>
    //*************************************************************************

    protected void
    ConnectAsyncLayoutEvents
    (
        IAsyncLayout oAsyncLayout
    )
    {
        // AssertValid();
        Debug.Assert(oAsyncLayout != null);

        oAsyncLayout.LayOutGraphIterationCompleted +=
            new EventHandler(this.AsyncLayout_LayOutGraphIterationCompleted);

        oAsyncLayout.LayOutGraphCompleted +=
            new AsyncCompletedEventHandler(
                this.AsyncLayout_LayOutGraphCompleted);
    }

    //*************************************************************************
    //  Method: CreateVertexToolTipTracker()
    //
    /// <summary>
    /// Creates a helper object for displaying vertex tooltips and registers
    /// event handlers with it.
    /// </summary>
    //*************************************************************************

    protected void
    CreateVertexToolTipTracker()
    {
        m_oVertexToolTipTracker = new WpfToolTipTracker();

        m_oVertexToolTipTracker.ShowToolTip +=
            new WpfToolTipTracker.ToolTipTrackerEvent(
                this.VertexToolTipTracker_ShowToolTip);

        m_oVertexToolTipTracker.HideToolTip +=
            new WpfToolTipTracker.ToolTipTrackerEvent(
                this.VertexToolTipTracker_HideToolTip);
    }

    //*************************************************************************
    //  Method: CreateDefaultVertexToolTip()
    //
    /// <summary>
    /// Creates a UIElement to use as a default vertex tooltip.
    /// </summary>
    ///
    /// <param name="sToolTip">
    /// The tooltip string to use.  Can be empty but not null.
    /// </param>
    ///
    /// <returns>
    /// A UIElement.
    /// </returns>
    //*************************************************************************

    protected UIElement
    CreateDefaultVertexToolTip
    (
        String sToolTip
    )
    {
        Debug.Assert(sToolTip != null);
        AssertValid();

        // Use a Label as a default tooltip.

        System.Windows.Controls.Label oLabel =
            new System.Windows.Controls.Label();

        oLabel.Background = SystemColors.InfoBrush;
        oLabel.BorderBrush = SystemColors.WindowFrameBrush;
        oLabel.BorderThickness = new Thickness(1);
        oLabel.Padding = new Thickness(3, 2, 3, 1);
        oLabel.Content = sToolTip;

        return (oLabel);
    }

    //*************************************************************************
    //  Method: CreateTransforms()
    //
    /// <summary>
    /// Creates the transforms that control zoom and scale.
    /// </summary>
    //*************************************************************************

    protected void
    CreateTransforms()
    {
        // The graph scale is controlled by a ScaleTransform used as a layout
        // transform.  If its ScaleX and ScaleY properties are set to 0.5, for
        // example, the control size reported by ActualWidth and ActualHeight
        // are twice the actual window dimensions and the graph gets laid out
        // within this enlarged rectangle.  The enlarged rectangle gets reduced
        // to the control size when it is rendered, so the entire graph fits
        // within the control but all the vertices and edges are smaller than
        // normal.

        this.LayoutTransform = new ScaleTransform();

        // The zoom is controlled by a ScaleTransform used as the first of two
        // render transforms.  If its ScaleX and ScaleY properties are set to
        // 2.0, for example, the graph is rendered twice as large as normal and
        // doesn't fit within the control.
        //
        // This ScaleTransform does not affect the ActualWidth and ActualHeight
        // properties.

        TransformGroup oTransformGroup = new TransformGroup();

        oTransformGroup.Children.Add( new ScaleTransform() );

        // The zoom center is controlled by a TranslateTransform used as a
        // second render transform.  If its X property is set to -100, for
        // example, then the graph, which has been scaled by the group's first
        // transform, is translated to the left by 100 scaled units.

        oTransformGroup.Children.Add( new TranslateTransform() );

        this.RenderTransform = oTransformGroup;

        // Note that mouse coordinates as reported by
        // MouseEventArgs.GetLocation() are affected by all three transforms.
        // Because of this, it is sometimes more convenient to convert the
        // mouse coordinates to screen coordinates, which are not affected by
        // any of the transforms.
    }

    //*************************************************************************
    //  Method: SetGraphZoom()
    //
    /// <summary>
    /// Sets a value that determines the zoom level of the graph.
    /// </summary>
    ///
    /// <param name="dGraphZoom">
    /// A value that determines the zoom level of the graph.  Must be between
    /// <see cref="MinimumGraphZoom" /> and <see cref="MaximumGraphZoom" />.
    /// </param>
    ///
    /// <param name="bLimitTranslation">
    /// If true, the TranslateTransform used for rendering is limited to
    /// prevent the graph from being moved too far by the zoom.
    /// </param>
    //*************************************************************************

    protected void
    SetGraphZoom
    (
        Double dGraphZoom,
        Boolean bLimitTranslation
    )
    {
        Debug.Assert(dGraphZoom >= MinimumGraphZoom);
        Debug.Assert(dGraphZoom <= MaximumGraphZoom);
        AssertValid();

        ResetVertexToolTipTracker();

        // See CreateTransforms() for details on how the graph zoom works.

        ScaleTransform oScaleTransformForRender = this.ScaleTransformForRender;

        oScaleTransformForRender.ScaleX =
            oScaleTransformForRender.ScaleY = dGraphZoom;

        if (bLimitTranslation)
        {
            LimitTranslation();
        }

        FireGraphZoomChanged();

        AssertValid();
    }

    //*************************************************************************
    //  Method: CenterGraphZoom()
    //
    /// <summary>
    /// Sets the center of the graph's zoom to the center of the control.
    /// </summary>
    ///
    /// <remarks>
    /// This method uses ActualWidth and ActualHeight, which are valid only
    /// after a WPF layout cycle.  If this is called before a WPF layout cycle
    /// completes, the zoom center is set to the control's origin.
    /// </remarks>
    //*************************************************************************

    protected void
    CenterGraphZoom()
    {
        AssertValid();

        ScaleTransform oScaleTransformForRender = this.ScaleTransformForRender;
        Double dLayoutScale = this.ScaleTransformForLayout.ScaleX;

        // The ScaleTransform used for the control's layout transform affects
        // the control's dimensions.  Adjust for this.  (See CreateTransforms()
        // for details.)

        oScaleTransformForRender.CenterX =
            dLayoutScale * this.ActualWidth / 2.0;

        oScaleTransformForRender.CenterY =
            dLayoutScale * this.ActualHeight / 2.0;
    }

    //*************************************************************************
    //  Method: LimitTranslation()
    //
    /// <overloads>
    /// Prevents the graph from being moved too far.
    /// </overloads>
    ///
    /// <summary>
    /// Prevents the graph from being moved too far by adjusting the
    /// TranslateTransform used for rendering.
    /// </summary>
    //*************************************************************************

    protected void
    LimitTranslation()
    {
        AssertValid();

        TranslateTransform oTranslateTransformForRender =
            this.TranslateTransformForRender;

        Double dTranslateX = oTranslateTransformForRender.X;
        Double dTranslateY = oTranslateTransformForRender.Y;

        LimitTranslation(ref dTranslateX, ref dTranslateY);

        oTranslateTransformForRender.X = dTranslateX;
        oTranslateTransformForRender.Y = dTranslateY;

        FireGraphTranslationChanged();
    }

    //*************************************************************************
    //  Method: LimitTranslation()
    //
    /// <summary>
    /// Prevents the graph from being moved too far, given a pair of proposed
    /// translation distances.
    /// </summary>
    ///
    /// <param name="dTranslateX">
    /// On input, this is the proposed TranslateTransform.X property.  On
    /// output, it is set to a value that will prevent the graph from being
    /// moved too far.
    /// </param>
    ///
    /// <param name="dTranslateY">
    /// On input, this is the proposed TranslateTransform.Y property.  On
    /// output, it is set to a value that will prevent the graph from being
    /// moved too far.
    /// </param>
    ///
    /// <remarks>
    /// The caller should modify the TranslateTransform with the modified
    /// <paramref name="dTranslateX" /> and <paramref name="dTranslateY" />
    /// values.
    /// </remarks>
    //*************************************************************************

    protected void
    LimitTranslation
    (
        ref Double dTranslateX,
        ref Double dTranslateY
    )
    {
        AssertValid();

        // See CreateTransforms() for details on the transforms used by this
        // class.

        ScaleTransform oScaleTransformForRender = this.ScaleTransformForRender;

        Double dScaleTransformForRenderCenterX =
            oScaleTransformForRender.CenterX;

        Double dScaleTransformForRenderCenterY =
            oScaleTransformForRender.CenterY;

        Double dLayoutScale = this.ScaleTransformForLayout.ScaleX;
        Double dRenderScale = oScaleTransformForRender.ScaleX;
        Rect oGraphRectangle = this.GraphRectangle;

        Double dRenderScaleMinus1 = dRenderScale - 1.0;

        dTranslateX = Math.Min(
            dScaleTransformForRenderCenterX * dRenderScaleMinus1,
            dTranslateX
            );

        dTranslateX = Math.Max(

            -( (oGraphRectangle.Width * dLayoutScale) -
                dScaleTransformForRenderCenterX) * dRenderScaleMinus1,

            dTranslateX
            );

        dTranslateY = Math.Min(

            dScaleTransformForRenderCenterY * dRenderScaleMinus1,
            dTranslateY
            );

        dTranslateY = Math.Max(

            -( (oGraphRectangle.Height * dLayoutScale) -
                dScaleTransformForRenderCenterY) * dRenderScaleMinus1,

            dTranslateY
            );
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

        return ( m_oGraphDrawer.VisualCollection[index] );
    }

    //*************************************************************************
    //  Method: LayOutOrDrawGraph()
    //
    /// <summary>
    /// Lays out or draws the graph, depending on the current layout state.
    /// </summary>
    //*************************************************************************

    protected void
    LayOutOrDrawGraph()
    {
        AssertValid();

        #if TRACE_LAYOUT_AND_DRAW
        Debug.WriteLine("NodeXLControl: LayOutOrDrawGraph(), m_eLayoutState = "
            + m_eLayoutState);
        #endif

        Rect oGraphRectangle = this.GraphRectangle;

        #if TRACE_LAYOUT_AND_DRAW
        Debug.WriteLine("NodeXLControl: LayOutOrDrawGraph(), size = " +
            oGraphRectangle.Width + " , " + oGraphRectangle.Height);
        #endif

        System.Drawing.Rectangle oGraphRectangle2 =
            WpfGraphicsUtil.RectToRectangle(oGraphRectangle);

        switch (m_eLayoutState)
        {
            case LayoutState.Stable:

                break;

            case LayoutState.LayoutRequired:

                Debug.Assert(!m_oAsyncLayout.IsBusy);

                FireDrawingGraph();

                m_oLastLayoutContext = new LayoutContext(oGraphRectangle2);

                m_eLayoutState = LayoutState.LayingOut;

                if (m_oAsyncLayout is SortableLayoutBase)
                {
                    // If the vertex layout order has been set, tell the layout
                    // object to sort the vertices before laying them out.

                    ( (SortableLayoutBase)m_oAsyncLayout ).VertexSorter =

                        m_oGraph.ContainsKey(
                            ReservedMetadataKeys.SortableLayoutOrderSet) ? 

                        new ByMetadataVertexSorter<Single>(
                            ReservedMetadataKeys.SortableLayoutOrder)
                        :
                        null;
                }

                // Start an asynchronous layout.  The m_oAsyncLayout object
                // will fire LayOutGraphIterationCompleted and
                // LayOutGraphCompleted events as it does its work.

                m_oAsyncLayout.LayOutGraphAsync(
                    m_oGraph, m_oLastLayoutContext);

                break;

            case LayoutState.LayingOut:

                break;

            case LayoutState.LayoutIterationCompleted:

                // An iteration of the asynchronous layout has completed and
                // now the graph needs to be drawn.

                m_eLayoutState = LayoutState.LayingOut;

                DrawGraph(oGraphRectangle);

                break;

            case LayoutState.LayoutCompleted:

                // The asynchronous layout has completed and now the graph
                // needs to be drawn.

                m_eLayoutState = LayoutState.Stable;

                // Has the size of the control changed since the layout was
                // started?

                System.Drawing.Rectangle oLastGraphRectangle =
                    m_oLastLayoutContext.GraphRectangle;

                if (
                    oLastGraphRectangle.Width != oGraphRectangle2.Width
                    ||
                    oLastGraphRectangle.Height != oGraphRectangle2.Height
                    )
                {
                    // Yes.  Transform the layout to the new size.

                    #if TRACE_LAYOUT_AND_DRAW
                    Debug.WriteLine("NodeXLControl: Transforming layout.");
                    #endif

                    m_oLastLayoutContext = TransformLayout(oGraphRectangle);
                }

                DrawGraph(oGraphRectangle);

                break;

            case LayoutState.TransformRequired:

                // The control has been resized and now the graph's layout
                // needs to be transformed to the new size.

                m_oLastLayoutContext = TransformLayout(oGraphRectangle);

                m_eLayoutState = LayoutState.Stable;

                DrawGraph(oGraphRectangle);

                break;

            default:

                Debug.Assert(false);
                break;
        }
    }

    //*************************************************************************
    //  Method: DrawGraph()
    //
    /// <summary>
    /// Draws the graph.
    /// </summary>
    ///
    /// <param name="oGraphRectangle">
    /// Rectangle to draw the graph within.
    /// </param>
    //*************************************************************************

    protected void
    DrawGraph
    (
        Rect oGraphRectangle
    )
    {
        AssertValid();

        #if TRACE_LAYOUT_AND_DRAW
        Debug.WriteLine("NodeXLControl: DrawGraph(), oGraphRectangle = "
            + oGraphRectangle);
        #endif

        m_oLastGraphDrawingContext = new GraphDrawingContext(
            oGraphRectangle, m_oAsyncLayout.Margin, m_oGraphDrawer.BackColor);

        m_oGraphDrawer.DrawGraph(m_oGraph, m_oLastGraphDrawingContext);
    }

    //*************************************************************************
    //  Method: TransformLayout()
    //
    /// <summary>
    /// Transforms the graph's layout to a new size.
    /// </summary>
    ///
    /// <param name="oNewGraphRectangle">
    /// The new size.
    /// </param>
    ///
    /// <returns>
    /// The new LayoutContext object that was used to transform the layout.
    /// </returns>
    //*************************************************************************

    protected LayoutContext
    TransformLayout
    (
        Rect oNewGraphRectangle
    )
    {
        AssertValid();

        LayoutContext oNewLayoutContext = new LayoutContext(
            WpfGraphicsUtil.RectToRectangle(oNewGraphRectangle) );

        m_oAsyncLayout.TransformLayout(m_oGraph,
            m_oLastLayoutContext, oNewLayoutContext);

        return (oNewLayoutContext);
    }

    //*************************************************************************
    //  Method: SetAllSelected()
    //
    /// <summary>
    /// Selects or deselects all vertices and edges.
    /// </summary>
    ///
    /// <param name="bSelect">
    /// true to select, false to deselect.
    /// </param>
    //*************************************************************************

    protected void
    SetAllSelected
    (
        Boolean bSelect
    )
    {
        AssertValid();

        SetAllVerticesSelected(bSelect);
        SetAllEdgesSelected(bSelect);

        FireSelectionChanged();
    }

    //*************************************************************************
    //  Method: SetAllVerticesSelected()
    //
    /// <summary>
    /// Sets the selected state of all vertices.
    /// </summary>
    ///
    /// <param name="bSelected">
    /// true to select all vertices, false to deselect them.
    /// </param>
    //*************************************************************************

    protected void
    SetAllVerticesSelected
    (
        Boolean bSelected
    )
    {
        AssertValid();

        if (bSelected)
        {
            foreach (IVertex oVertex in this.Graph.Vertices)
            {
                SetVertexSelectedInternal(oVertex, true);
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
                SetVertexSelectedInternal(oSelectedVertex, false);
            }
        }
    }

    //*************************************************************************
    //  Method: SetAllEdgesSelected()
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
    SetAllEdgesSelected
    (
        Boolean bSelected
    )
    {
        AssertValid();

        if (bSelected)
        {
            foreach (IEdge oEdge in this.Graph.Edges)
            {
                SetEdgeSelectedInternal(oEdge, true);
            }
        }
        else
        {
            // Do not directly iterate m_oSelectedEdges.Keys here.  Keys may be
            // removed by SetEdgeSelectedInternal() and you can't modify a
            // collection while it's being iterated.  this.SelectedEdges copies
            // the collection to an array.

            foreach (IEdge oSelectedEdge in this.SelectedEdges)
            {
                SetEdgeSelectedInternal(oSelectedEdge, false);
            }
        }
    }

    //*************************************************************************
    //  Method: SetVertexSelectedInternal()
    //
    /// <summary>
    /// Performs all tasks required to select a vertex.
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
    SetVertexSelectedInternal
    (
        IVertex oVertex,
        Boolean bSelected
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        // Modify the vertex's metadata to mark it as selected or unselected.

        MarkVertexOrEdgeAsSelected(oVertex, bSelected);

        // Modify the dictionary of selected vertices.

        if (bSelected)
        {
            m_oSelectedVertices[oVertex] = 0;
        }
        else
        {
            m_oSelectedVertices.Remove(oVertex);
        }

        // Redraw the vertex using its modified metadata.

        Debug.Assert(m_oLastGraphDrawingContext != null);

        m_oGraphDrawer.RedrawVertex(oVertex, m_oLastGraphDrawingContext);
    }

    //*************************************************************************
    //  Method: SetEdgeSelectedInternal()
    //
    /// <summary>
    /// Performs all tasks required to select an edge.
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
    SetEdgeSelectedInternal
    (
        IEdge oEdge,
        Boolean bSelected
    )
    {
        Debug.Assert(oEdge != null);
        AssertValid();

        // Modify the edge's metadata to mark it as selected or unselected.

        MarkVertexOrEdgeAsSelected(oEdge, bSelected);

        // Modify the dictionary of selected edges.

        if (bSelected)
        {
            m_oSelectedEdges[oEdge] = 0;
        }
        else
        {
            m_oSelectedEdges.Remove(oEdge);
        }

        // Redraw the edge using its modified metadata.

        Debug.Assert(m_oLastGraphDrawingContext != null);

        m_oGraphDrawer.RedrawEdge(oEdge, m_oLastGraphDrawingContext);
    }

    //*************************************************************************
    //  Method: MarkVertexOrEdgeAsSelected()
    //
    /// <summary>
    /// Modifies the metadata of a vertex or edge to mark it as selected or
    /// unselected.
    /// </summary>
    ///
    /// <param name="oVertexOrEdge">
    /// Vertex or edge to mark, as an <see cref="IMetadataProvider" />.
    /// </param>
    ///
    /// <param name="bSelected">
    /// true to mark <paramref name="oVertexOrEdge" /> as selected, false for
    /// unselected.
    /// </param>
    //*************************************************************************

    protected void
    MarkVertexOrEdgeAsSelected
    (
        IMetadataProvider oVertexOrEdge,
        Boolean bSelected
    )
    {
        Debug.Assert(oVertexOrEdge != null);

        if (bSelected)
        {
            oVertexOrEdge.SetValue(ReservedMetadataKeys.IsSelected, null);
        }
        else
        {
            oVertexOrEdge.RemoveKey(ReservedMetadataKeys.IsSelected);
        }
    }

    //*************************************************************************
    //  Method: ResetVertexToolTipTracker()
    //
    /// <summary>
    /// Removes any vertex tooltip that might exist and resets the helper
    /// object that figures out when to show tooltips.
    /// </summary>
    //*************************************************************************

    protected void
    ResetVertexToolTipTracker()
    {
        AssertValid();

        RemoveVertexToolTip();
        m_oVertexToolTipTracker.Reset();
        m_oLastMouseMoveLocation = new Point(-1, -1);
    }

    //*************************************************************************
    //  Method: RemoveVertexToolTip()
    //
    /// <summary>
    /// Removes any vertex tooltip that might exist.
    /// </summary>
    //*************************************************************************

    protected void
    RemoveVertexToolTip()
    {
        AssertValid();

        if (m_oVertexToolTip != null)
        {
            m_oGraphDrawer.RemoveVisualFromTopOfGraph(m_oVertexToolTip);
            m_oVertexToolTip = null;
        }
    }

    //*************************************************************************
    //  Method: GetBackgroundContrastColor()
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
            255,
            (Byte)Math.Abs(oBackColor.R - 255),
            (Byte)Math.Abs(oBackColor.G - 255),
            (Byte)Math.Abs(oBackColor.B - 255)
            ) );
    }

    //*************************************************************************
    //  Method: CheckIfDrawing()
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
    CheckIfDrawing
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
                + " IsDrawing property before calling this."
                ,
                this.ClassName,
                sMethodOrPropertyName
                ) );
        }
    }

    //*************************************************************************
    //  Method: CheckForVertexDragOnMouseMove()
    //
    /// <summary>
    /// Checks for a vertex drag operation during the MouseMove event.
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
        Point oMouseLocation;

        if ( !DragIsInProgress(m_oVerticesBeingDragged, oMouseEventArgs,
            out oMouseLocation) )
        {
            return;
        }

        // Remove from the top of the graph any dragged vertices drawn during
        // the previous MouseMove event.

        RemoveVisualFromTopOfGraph(m_oVerticesBeingDragged);

        // Create a new Visual to represent the dragged vertices and add it on
        // top of the graph.

        m_oVerticesBeingDragged.CreateVisual(oMouseLocation,
            GetBackgroundContrastColor(), m_oGraphDrawer.VertexDrawer);

        m_oGraphDrawer.AddVisualOnTopOfGraph(m_oVerticesBeingDragged.Visual);
    }

    //*************************************************************************
    //  Method: CheckForVertexDragOnMouseUp()
    //
    /// <summary>
    /// Checks for a vertex drag operation during the MouseUp event.
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

        if ( m_oVerticesBeingDragged != null &&
            m_oVerticesBeingDragged.OnMouseUp() )
        {
            // Remove from the top of the graph any dragged vertices drawn
            // during the previous MouseMove event.

            RemoveVisualFromTopOfGraph(m_oVerticesBeingDragged);

            if ( this.EscapeKeyIsPressed() )
            {
                m_oVerticesBeingDragged.CancelDrag();
            }
            else
            {
                FireVerticesMoved(m_oVerticesBeingDragged.Vertices);
            }

            // Remove metadata that VerticesBeingDragged added to the dragged
            // vertices.

            m_oVerticesBeingDragged.RemoveMetadataFromVertices();

            // m_oVerticesBeingDragged may have moved the selected vertices, so
            // redraw the graph.

            DrawGraph();
        }

        m_oVerticesBeingDragged = null;
    }

    //*************************************************************************
    //  Method: CheckForMarqueeDragOnMouseMove()
    //
    /// <summary>
    /// Checks for a marquee drag operation during the MouseMove event.
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

        Point oMouseLocation;

        if ( !DragIsInProgress(m_oMarqueeBeingDragged, oMouseEventArgs,
            out oMouseLocation) )
        {
            return;
        }

        // Remove from the top of the graph any marquee drawn during the
        // previous MouseMove event.

        RemoveVisualFromTopOfGraph(m_oMarqueeBeingDragged);

        // Create a new marquee and add it on top of the graph.

        m_oMarqueeBeingDragged.CreateVisual( oMouseLocation,
            GetBackgroundContrastColor() );

        m_oGraphDrawer.AddVisualOnTopOfGraph(m_oMarqueeBeingDragged.Visual);

        // Set the cursor, which depends on optional key presses.

        this.Cursor = GetCursorForMarqueeDrag();
    }

    //*************************************************************************
    //  Method: CheckForMarqueeDragOnMouseUp()
    //
    /// <summary>
    /// Checks for a marquee drag operation during the MouseUp event.
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

        if ( m_oMarqueeBeingDragged != null &&
            m_oMarqueeBeingDragged.OnMouseUp() )
        {
            // Remove from the top of the graph any marquee drawn during the
            // previous MouseMove event.

            RemoveVisualFromTopOfGraph(m_oMarqueeBeingDragged);

            if ( !this.EscapeKeyIsPressed() )
            {
                // Select or deselect the marqueed vertices.

                SelectMarqueedVertices();
            }
        }

        m_oMarqueeBeingDragged = null;
    }

    //*************************************************************************
    //  Method: CheckForTranslationDragOnMouseMove()
    //
    /// <summary>
    /// Checks for a translation drag operation during the MouseMove event.
    /// </summary>
    ///
    /// <param name="oMouseEventArgs">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected void
    CheckForTranslationDragOnMouseMove
    (
        MouseEventArgs oMouseEventArgs
    )
    {
        Debug.Assert(oMouseEventArgs != null);
        AssertValid();

        Point oMouseLocation;

        if ( !DragIsInProgress(m_oTranslationBeingDragged,
            oMouseEventArgs, out oMouseLocation) )
        {
            return;
        }

        // Adjust the TranslateTransform in response to the mouse move.

        Double dNewTranslateX, dNewTranslateY;

        m_oTranslationBeingDragged.GetTranslationDistances(
            this.PointToScreen(oMouseLocation),
            out dNewTranslateX, out dNewTranslateY);

        // Prevent the graph from being moved too far.

        LimitTranslation(ref dNewTranslateX, ref dNewTranslateY);

        // Move the graph.

        TranslateTransform oTranslateTransform =
            this.TranslateTransformForRender;

        oTranslateTransform.X = dNewTranslateX;
        oTranslateTransform.Y = dNewTranslateY;

        FireGraphTranslationChanged();
    }

    //*************************************************************************
    //  Method: CheckForTranslationDragOnMouseUp()
    //
    /// <summary>
    /// Checks for a translation drag operation during the MouseUp event.
    /// </summary>
    ///
    /// <param name="oMouseEventArgs">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected void
    CheckForTranslationDragOnMouseUp
    (
        MouseEventArgs oMouseEventArgs
    )
    {
        Debug.Assert(oMouseEventArgs != null);
        AssertValid();

        // (Nothing further needs to be done for a translation drag on mouse
        // up.)

        m_oTranslationBeingDragged = null;
    }

    //*************************************************************************
    //  Method: CheckForToolTipsOnMouseMove()
    //
    /// <summary>
    /// Checks whether tooltip-related actions need to be taken during the
    /// MouseMove event.
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

        // Don't do anything if the mouse is being dragged.

        if ( DragMightBeInProgress() )
        {
            return;
        }

        Point oMouseLocation = oMouseEventArgs.GetPosition(this);

        if (oMouseLocation.X == m_oLastMouseMoveLocation.X &&
            oMouseLocation.Y == m_oLastMouseMoveLocation.Y)
        {
            return;
        }

        m_oLastMouseMoveLocation = oMouseLocation;

        // If the mouse is over a vertex, get the vertex object.  (oVertex will
        // get set to null if the mouse is not over a vertex.)

        IVertex oVertex;

        m_oGraphDrawer.TryGetVertexFromPoint(oMouseLocation, out oVertex);

        // Update the tooltip tracker.

        m_oVertexToolTipTracker.OnMouseMoveOverObject(oVertex);
    }

    //*************************************************************************
    //  Method: ControlKeyIsPressed()
    //
    /// <summary>
    /// Determines whether a control key is pressed.
    /// </summary>
    ///
    /// <returns>
    /// true if a control key is pressed.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ControlKeyIsPressed()
    {
        AssertValid();

        return ( (Keyboard.Modifiers & ModifierKeys.Control) != 0 );
    }

    //*************************************************************************
    //  Method: ShiftKeyIsPressed()
    //
    /// <summary>
    /// Determines whether a shift key is pressed.
    /// </summary>
    ///
    /// <returns>
    /// true if a shift key is pressed.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ShiftKeyIsPressed()
    {
        AssertValid();

        return ( (Keyboard.Modifiers & ModifierKeys.Shift) != 0 );
    }

    //*************************************************************************
    //  Method: AltKeyIsPressed()
    //
    /// <summary>
    /// Determines whether an Alt key is pressed.
    /// </summary>
    ///
    /// <returns>
    /// true if an Alt key is pressed.
    /// </returns>
    //*************************************************************************

    protected Boolean
    AltKeyIsPressed()
    {
        AssertValid();

        return ( (Keyboard.Modifiers & ModifierKeys.Alt) != 0 );
    }

    //*************************************************************************
    //  Method: EscapeKeyIsPressed()
    //
    /// <summary>
    /// Determines whether the Escape key is pressed.
    /// </summary>
    ///
    /// <returns>
    /// true if the Escape key is pressed.
    /// </returns>
    //*************************************************************************

    protected Boolean
    EscapeKeyIsPressed()
    {
        AssertValid();

        return ( Keyboard.IsKeyDown(Key.Escape) );
    }

    //*************************************************************************
    //  Method: TranslationDragKeyIsPressed()
    //
    /// <summary>
    /// Determines whether the key used to start dragging a translation with
    /// the mouse is pressed.
    /// </summary>
    ///
    /// <returns>
    /// true if the key is pressed.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TranslationDragKeyIsPressed()
    {
        AssertValid();

        return ( Keyboard.IsKeyDown(TranslationDragKey) );
    }

    //*************************************************************************
    //  Method: LeftButtonIsPressed()
    //
    /// <summary>
    /// Determines whether the left mouse button is pressed.
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

        return (oMouseEventArgs.LeftButton == MouseButtonState.Pressed);
    }

    //*************************************************************************
    //  Method: RightButtonIsPressed()
    //
    /// <summary>
    /// Determines whether the right mouse button is pressed.
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

        return (oMouseEventArgs.RightButton == MouseButtonState.Pressed);
    }

    //*************************************************************************
    //  Method: DragIsInProgress()
    //
    /// <summary>
    /// Determines whether a particular type of mouse drag is in progress.
    /// </summary>
    ///
    /// <param name="oMouseDrag">
    /// Object that represents the drag operation, or null if the drag
    /// operation hasn't begun.
    /// </param>
    ///
    /// <param name="oMouseEventArgs">
    /// Standard mouse event arguments.
    /// </param>
    ///
    /// <param name="oMouseLocation">
    /// Where the mouse location get stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the specified mouse drag is in progress.
    /// </returns>
    //*************************************************************************

    protected Boolean
    DragIsInProgress
    (
        MouseDrag oMouseDrag,
        MouseEventArgs oMouseEventArgs,
        out Point oMouseLocation
    )
    {
        Debug.Assert(oMouseEventArgs != null);
        AssertValid();

        oMouseLocation = new Point();

        if ( oMouseDrag == null || !LeftButtonIsPressed(oMouseEventArgs) )
        {
            return (false);
        }

        oMouseLocation = oMouseEventArgs.GetPosition(this);

        return ( oMouseDrag.OnMouseMove(oMouseLocation) );
    }

    //*************************************************************************
    //  Method: DragMightBeInProgress()
    //
    /// <summary>
    /// Determines whether any kind of mouse drag is in progress or will be
    /// in progress if the user moves the mouse.
    /// </summary>
    ///
    /// <returns>
    /// true if a mouse drag is in progress, or if a MouseDrag object has been
    /// created and is waiting for the user to move the mouse to begin a drag.
    /// </returns>
    //*************************************************************************

    protected Boolean
    DragMightBeInProgress()
    {
        AssertValid();

        return (m_oVerticesBeingDragged != null ||
            m_oMarqueeBeingDragged != null ||
            m_oTranslationBeingDragged != null
            );
    }

    //*************************************************************************
    //  Method: RemoveVisualFromTopOfGraph()
    //
    /// <summary>
    /// Removes from the top of the graph any Visual drawn during a MouseMove
    /// event.
    /// </summary>
    ///
    /// <param name="oMouseDragWithVisual">
    /// Object that represents the drag operation, or null if the drag
    /// operation hasn't begun.
    /// </param>
    //*************************************************************************

    protected void
    RemoveVisualFromTopOfGraph
    (
        MouseDragWithVisual oMouseDragWithVisual
    )
    {
        AssertValid();

        if (oMouseDragWithVisual == null)
        {
            return;
        }

        Visual oVisual = oMouseDragWithVisual.Visual;

        if (oVisual != null)
        {
            m_oGraphDrawer.RemoveVisualFromTopOfGraph(oVisual);
        }
    }

    //*************************************************************************
    //  Method: GetCursorForMarqueeDrag()
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

        String sResourceName = null;

        if ( this.ShiftKeyIsPressed() )
        {
            sResourceName = "MarqueeAdd.cur";
        }
        else if ( this.AltKeyIsPressed() )
        {
            sResourceName = "MarqueeSubtract.cur";
        }
        else
        {
            sResourceName = "Marquee.cur";
        }

        Stream oCursorResourceStream =
            Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "Microsoft.NodeXL.Control.Wpf.Images." + sResourceName);

        return ( new Cursor(oCursorResourceStream) );
    }

    //*************************************************************************
    //  Method: SelectMarqueedVertices()
    //
    /// <summary>
    /// Selects or deselects the marqueed vertices.
    /// </summary>
    //*************************************************************************

    protected void
    SelectMarqueedVertices()
    {
        AssertValid();

        Debug.Assert(m_oMarqueeBeingDragged != null);

        // Create dictionaries that will contain the new selection.
        // Dictionaries are used instead of lists or arrays to prevent the same
        // vertex or edge from being added twice.

        Dictionary<IVertex, Byte> oVerticesToSelect;
        Dictionary<IEdge, Byte> oEdgesToSelect;

        Boolean bShiftKeyIsPressed = ShiftKeyIsPressed();
        Boolean bAltKeyIsPressed = AltKeyIsPressed();

        if (bShiftKeyIsPressed || bAltKeyIsPressed)
        {
            // The new selection gets added to or subtracted from the old
            // selection.

            oVerticesToSelect =
                new Dictionary<IVertex, Byte>(m_oSelectedVertices);

            oEdgesToSelect = new Dictionary<IEdge, Byte>(m_oSelectedEdges);
        }
        else
        {
            // The new selection replaces the old selection.

            oVerticesToSelect = new Dictionary<IVertex, Byte>();
            oEdgesToSelect = new Dictionary<IEdge, Byte>();
        }

        // Loop through the vertices that intersect the marquee rectangle.

        Rect oMarqueeRectangle = m_oMarqueeBeingDragged.MarqueeRectangle;

        foreach ( IVertex oMarqueedVertex in
            m_oGraphDrawer.GetVerticesFromRectangle(oMarqueeRectangle) )
        {
            if (bAltKeyIsPressed)
            {
                oVerticesToSelect.Remove(oMarqueedVertex);
            }
            else
            {
                oVerticesToSelect[oMarqueedVertex] = 0;
            }

            if (m_eMouseSelectionMode ==
                MouseSelectionMode.SelectVertexAndIncidentEdges)
            {
                // Also loop through the vertex's incident edges.

                foreach (IEdge oEdge in oMarqueedVertex.IncidentEdges)
                {
                    if (bAltKeyIsPressed)
                    {
                        oEdgesToSelect.Remove(oEdge);
                    }
                    else
                    {
                        oEdgesToSelect[oEdge] = 0;
                    }
                }
            }
        }

        SetSelected(
            CollectionUtil.DictionaryKeysToArray<IVertex, Byte>(
                oVerticesToSelect),

            CollectionUtil.DictionaryKeysToArray<IEdge, Byte>(
                oEdgesToSelect)
            );
    }

    //*************************************************************************
    //  Method: FireSelectionChanged()
    //
    /// <summary>
    /// Fires the <see cref="SelectionChanged" /> event if appropriate.
    /// </summary>
    //*************************************************************************

    protected void
    FireSelectionChanged()
    {
        AssertValid();

        EventUtil.FireEvent(this, this.SelectionChanged);
    }

    //*************************************************************************
    //  Method: FireGraphMouseDown()
    //
    /// <summary>
    /// Fires the <see cref="GraphMouseDown" /> event if appropriate.
    /// </summary>
    ///
    /// <param name="oMouseButtonEventArgs">
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
        MouseButtonEventArgs oMouseButtonEventArgs,
        IVertex oVertex
    )
    {
        Debug.Assert(oMouseButtonEventArgs != null);
        AssertValid();

        FireGraphMouseButtonEvent(this.GraphMouseDown, oMouseButtonEventArgs,
            oVertex);
    }

    //*************************************************************************
    //  Method: FireGraphMouseUp()
    //
    /// <summary>
    /// Fires the <see cref="GraphMouseUp" /> event if appropriate.
    /// </summary>
    ///
    /// <param name="oMouseButtonEventArgs">
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
        MouseButtonEventArgs oMouseButtonEventArgs,
        IVertex oVertex
    )
    {
        Debug.Assert(oMouseButtonEventArgs != null);
        AssertValid();

        FireGraphMouseButtonEvent(this.GraphMouseUp, oMouseButtonEventArgs,
            oVertex);
    }

    //*************************************************************************
    //  Method: FireVertexClick()
    //
    /// <summary>
    /// Fires the <see cref="VertexClick" /> event if appropriate.
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
    //  Method: FireVertexDoubleClick()
    //
    /// <summary>
    /// Fires the <see cref="VertexDoubleClick" /> event if appropriate.
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
    //  Method: FireVertexMouseHover()
    //
    /// <summary>
    /// Fires the <see cref="VertexMouseHover" /> event if appropriate.
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
    //  Method: FireVertexMouseLeave()
    //
    /// <summary>
    /// Fires the <see cref="VertexMouseLeave" /> event if appropriate.
    /// </summary>
    //*************************************************************************

    protected void
    FireVertexMouseLeave()
    {
        AssertValid();

        EventUtil.FireEvent(this, this.VertexMouseLeave);
    }

    //*************************************************************************
    //  Method: FireGraphZoomChanged()
    //
    /// <summary>
    /// Fires the <see cref="GraphZoomChanged" /> event if appropriate.
    /// </summary>
    //*************************************************************************

    protected void
    FireGraphZoomChanged()
    {
        AssertValid();

        EventUtil.FireEvent(this, this.GraphZoomChanged);
    }

    //*************************************************************************
    //  Method: FireGraphTranslationChanged()
    //
    /// <summary>
    /// Fires the <see cref="GraphTranslationChanged" /> event if appropriate.
    /// </summary>
    //*************************************************************************

    protected void
    FireGraphTranslationChanged()
    {
        AssertValid();

        EventUtil.FireEvent(this, this.GraphTranslationChanged);
    }

    //*************************************************************************
    //  Method: FireVerticesMoved()
    //
    /// <summary>
    /// Fires the <see cref="VerticesMoved" /> event if appropriate.
    /// </summary>
    ///
    /// <param name="aoMovedVertices">
    /// An array of one or more vertices that were moved.
    /// </param>
    //*************************************************************************

    protected void
    FireVerticesMoved
    (
        IVertex [] aoMovedVertices
    )
    {
        Debug.Assert(aoMovedVertices != null);
        AssertValid();

        VerticesMovedEventHandler oVerticesMoved = this.VerticesMoved;

        if (oVerticesMoved != null)
        {
            oVerticesMoved( this,
                new VerticesMovedEventArgs(aoMovedVertices) );
        }
    }

    //*************************************************************************
    //  Method: FireDrawingGraph()
    //
    /// <summary>
    /// Fires the <see cref="DrawingGraph" /> event if appropriate.
    /// </summary>
    //*************************************************************************

    protected void
    FireDrawingGraph()
    {
        AssertValid();

        EventUtil.FireEvent(this, this.DrawingGraph);
    }

    //*************************************************************************
    //  Method: FireGraphDrawn()
    //
    /// <summary>
    /// Fires the <see cref="GraphDrawn" /> event if appropriate.
    /// </summary>
    //*************************************************************************

    protected void
    FireGraphDrawn()
    {
        AssertValid();

        EventUtil.FireEvent(this, this.GraphDrawn);
    }

    //*************************************************************************
    //  Method: FirePreviewVertexToolTipShown()
    //
    /// <summary>
    /// Fires the <see cref="PreviewVertexToolTipShown" /> event if
    /// appropriate.
    /// </summary>
    ///
    /// <param name="oVertexToolTipShownEventArgs">
    /// Event arguments.  The event handler may modify the <see
    /// cref="VertexToolTipShownEventArgs.VertexToolTip" /> property.
    /// </param>
    //*************************************************************************

    protected void
    FirePreviewVertexToolTipShown
    (
        VertexToolTipShownEventArgs oVertexToolTipShownEventArgs
    )
    {
        Debug.Assert(oVertexToolTipShownEventArgs != null);
        AssertValid();

        VertexToolTipShownEventHandler oVertexToolTipShownEventHandler =
            this.PreviewVertexToolTipShown;

        if (oVertexToolTipShownEventHandler != null)
        {
            oVertexToolTipShownEventHandler(this,
                oVertexToolTipShownEventArgs);
        }

    }

    //*************************************************************************
    //  Method: FireVertexEvent()
    //
    /// <summary>
    /// Fires an event with the signature VertexEventHandler.
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
    //  Method: FireGraphMouseButtonEvent()
    //
    /// <summary>
    /// Fires an event with the signature GraphMouseButtonEventHandler.
    /// </summary>
    ///
    /// <param name="oGraphMouseButtonEventHandler">
    /// Event handler, or null if the event isn't being handled.
    /// </param>
    ///
    /// <param name="oMouseButtonEventArgs">
    /// Standard mouse event arguments.
    /// </param>
    ///
    /// <param name="oVertex">
    /// Vertex associated with the event.  Can be null.
    /// </param>
    //*************************************************************************

    protected void
    FireGraphMouseButtonEvent
    (
        GraphMouseButtonEventHandler oGraphMouseButtonEventHandler,
        MouseButtonEventArgs oMouseButtonEventArgs,
        IVertex oVertex
    )
    {
        Debug.Assert(oMouseButtonEventArgs != null);
        AssertValid();

        if (oGraphMouseButtonEventHandler != null)
        {
            oGraphMouseButtonEventHandler( this,
                new GraphMouseButtonEventArgs(
                    oMouseButtonEventArgs, oVertex) );
        }
    }

    //*************************************************************************
    //  Method: MeasureOverride()
    //
    /// <summary>
    /// When overridden in a derived class, measures the size in layout
    /// required for child elements and determines a size for the
    /// FrameworkElement-derived class. 
    /// </summary>
    ///
    /// <param name="availableSize">
    /// The available size that this element can give to child elements.
    /// Infinity can be specified as a value to indicate that the element will
    /// size to whatever content is available.
    /// </param>
    ///
    /// <returns>
    /// The size that this element determines it needs during layout, based on
    /// its calculations of child element sizes.
    /// </returns>
    //*************************************************************************

    protected override Size
    MeasureOverride
    (
        Size availableSize
    )
    {
        AssertValid();

        if (m_oVertexToolTip != null)
        {
            m_oVertexToolTip.Measure(availableSize);
        }

        Size oDesiredSize;

        if (availableSize.Width == Double.PositiveInfinity ||
            availableSize.Height == Double.PositiveInfinity)
        {
            oDesiredSize = new Size(1000, 1000);
        }
        else
        {
            oDesiredSize = availableSize;
        }

        #if TRACE_LAYOUT_AND_DRAW
        Debug.WriteLine("NodeXLControl: MeasureOverride: availableSize=" +
            availableSize + ", DesiredSize=" + oDesiredSize);
        #endif

        return (oDesiredSize);
    }

    //*************************************************************************
    //  Method: ArrangeOverride()
    //
    /// <summary>
    /// When overridden in a derived class, positions child elements and
    /// determines a size for a FrameworkElement derived class. 
    /// </summary>
    ///
    /// <param name="finalSize">
    /// The final area within the parent that this element should use to
    /// arrange itself and its children.
    /// </param>
    ///
    /// <returns>
    /// The actual size used.
    /// </returns>
    //*************************************************************************

    protected override Size
    ArrangeOverride
    (
        Size finalSize
    )
    {
        AssertValid();

        #if TRACE_LAYOUT_AND_DRAW
        Debug.WriteLine("NodeXLControl: ArrangeOverride: finalSize=" +
            finalSize);
        #endif

        if (m_oVertexToolTip != null)
        {
            // The height of the visible cursor is needed here, but that
            // doesn't seem to be available in any API.  You can get the cursor
            // size, but that size represents the entire cursor, part of which
            // may be transparent.  Several posts, including the following,
            // indicate that getting the visible height may not be practical:
            //
            // http://www.codeguru.com/forum/showthread.php?threadid=449040
            //
            // As a workaround, use just a hard-coded fraction of the cursor
            // height.  This works fine for the standard and large cursors.  If
            // extra large cursors are used, the cursor intrudes a bit into the
            // tooltip.
            //
            // This has been tested on both 96 and 120 DPI screen resolutions.

            Point oMousePosition = Mouse.GetPosition(this);

            Double dCursorHeight = SystemParameters.CursorHeight * 0.75;

            Rect oToolTipRectangle = new Rect(

                new Point(oMousePosition.X, oMousePosition.Y + dCursorHeight),

                m_oVertexToolTip.DesiredSize);

            // Limit the tooltip to the graph rectangle.

            Rect oBoundedToolTipRectangle =
                WpfGraphicsUtil.MoveRectangleWithinBounds(oToolTipRectangle,
                    new Rect(new Point(0, 0), finalSize), true);

            if (oBoundedToolTipRectangle.Bottom == finalSize.Height)
            {
                // The tooltip is at the bottom of the graph rectangle, where
                // it would be partially obscured by the cursor.  Move it so
                // its bottom is at the top of the cursor.

                oBoundedToolTipRectangle.Offset(0, 
                    oMousePosition.Y - oBoundedToolTipRectangle.Bottom
                    );
            }

            m_oVertexToolTip.Arrange(oBoundedToolTipRectangle);
        }

        return (finalSize);
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

        #if TRACE_LAYOUT_AND_DRAW
        Debug.WriteLine("NodeXLControl: OnRender(), m_eLayoutState = " +
            m_eLayoutState);
        #endif

        LayOutOrDrawGraph();
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

        // Note that this method gets called when the layout transform is
        // modified, not just when the control is resized.

        #if TRACE_LAYOUT_AND_DRAW
        Debug.WriteLine("NodeXLControl: OnRenderSizeChanged(),"
            + " sizeInfo.NewSize = " + sizeInfo.NewSize);
        #endif

        base.OnRenderSizeChanged(sizeInfo);

        // The next block is an ugly workaround for the following problem.
        //
        // The center of the graph zoom should initially be placed at the
        // center of the control, so that if the zoom is first increased via
        // the GraphZoom property, the control will zoom from the center.  (If
        // the zoom is first increased via the mouse wheel, the zoom center
        // will be placed at the mouse location.)  This can't be done until the
        // final control size is known.
        //
        // A Loaded handler would be a logical place to do this, but the final
        // control size is not always available during the Loaded event,
        // depending on how the control is anchored or docked within its
        // parent.
        //
        // A workaround would be to set this control's RenderTransformOrigin to
        // (0.5, 0.5) when the transforms are first created, but the control's
        // transforms are designed to work with this property left at its
        // default value of the origin.
        //
        // TODO: Fix this!

        if (!m_bGraphZoomCentered && this.IsLoaded)
        {
            m_bGraphZoomCentered = true;
            CenterGraphZoom();
        }

        if (m_eLayoutState != LayoutState.Stable)
        {
            // The new size will be detected by the LayoutState.LayoutCompleted
            // case in OnRender().

            return;
        }

        // Make sure the size has actually changed.  This event fires once at
        // startup and should be ignored then.

        System.Drawing.Rectangle oLastGraphRectangle =
            m_oLastLayoutContext.GraphRectangle;

        System.Drawing.Rectangle oNewGraphRectangle =
            WpfGraphicsUtil.RectToRectangle( new Rect(sizeInfo.NewSize) );

        if (
            oLastGraphRectangle.Width != oNewGraphRectangle.Width
            ||
            oLastGraphRectangle.Height != oNewGraphRectangle.Height
            )
        {
            m_eLayoutState = LayoutState.TransformRequired;
            LayOutOrDrawGraph();

            // For the case where this method was called because the layout
            // transform was modified, make sure the graph wasn't moved too
            // far by the transform.

            LimitTranslation();
        }
    }

    //*************************************************************************
    //  Method: OnMouseDown()
    //
    /// <summary>
    /// Handles the MouseDown event.
    /// </summary>
    ///
    /// <param name="e">
    /// The MouseButtonEventArgs that contains the event data.
    /// </param>
    //*************************************************************************

    protected override void
    OnMouseDown
    (
        MouseButtonEventArgs e
    )
    {
        AssertValid();

        // Do nothing if the drawing isn't in a stable state.

        if (this.IsDrawing)
        {
            return;
        }

        // Remove any vertex tooltip that might exist and reset the helper
        // object that figures out when to show tooltips.

        ResetVertexToolTipTracker();

        if ( DragMightBeInProgress() )
        {
            // This can occur if the user clicks the right button while
            // dragging with the left button.

            return;
        }

        // Some drag operations can be cancelled with the Escape key, so
        // capture keyboard focus.

        Keyboard.Focus(this);

        // Check whether the user clicked on a vertex.

        Point oMouseLocation = e.GetPosition(this);
        IVertex oClickedVertex;

        Boolean bVertexClicked =
            TryGetVertexFromPoint(oMouseLocation, out oClickedVertex);

        FireGraphMouseDown(e, oClickedVertex);

        Boolean bControlKeyIsPressed = ControlKeyIsPressed();
        Boolean bRightButtonIsPressed = RightButtonIsPressed(e);

        ScaleTransform oScaleTransformForRender = this.ScaleTransformForRender;

        if ( !bRightButtonIsPressed && TranslationDragKeyIsPressed() )
        {
            // The user might want to translate the graph by dragging with the
            // mouse.  Save the mouse location for use within the MouseMove
            // event.

            TranslateTransform oTranslateTransform =
                this.TranslateTransformForRender;

            m_oTranslationBeingDragged = new DraggedTranslation(
                oMouseLocation, this.PointToScreen(oMouseLocation),
                oTranslateTransform.X, oTranslateTransform.Y);

            this.Cursor = Cursors.Hand;
            Mouse.Capture(this);

            return;
        }

        if (!bVertexClicked)
        {
            // The user clicked on part of the graph not covered by a
            // vertex.

            if (m_eMouseSelectionMode != MouseSelectionMode.SelectNothing)
            {
                Boolean bShiftKeyIsPressed = ShiftKeyIsPressed();
                Boolean bAltKeyIsPressed = AltKeyIsPressed();

                if (!bShiftKeyIsPressed && !bControlKeyIsPressed &&
                    !bAltKeyIsPressed && !bRightButtonIsPressed)
                {
                    DeselectAll();
                }

                if (this.Graph.Vertices.Count > 0)
                {
                    // The user might want to drag a marquee.  Save the mouse
                    // location for use within the MouseMove event.

                    m_oMarqueeBeingDragged = new DraggedMarquee(
                        oMouseLocation, this.GraphRectangle,
                        m_oAsyncLayout.Margin);

                    this.Cursor = GetCursorForMarqueeDrag();
                    Mouse.Capture(this);
                }
            }

            return;
        }

        // The user clicked on a vertex.  Fire VertexClick and
        // VertexDoubleClick events if appropriate.

        FireVertexClick(oClickedVertex);

        if (e.ClickCount == 2)
        {
            FireVertexDoubleClick(oClickedVertex);
        }

        if (m_eMouseSelectionMode != MouseSelectionMode.SelectNothing)
        {
            Boolean bSelectVertex = true;

            if (bControlKeyIsPressed)
            {
                // Toggle the vertex's selected state.

                bSelectVertex = !oClickedVertex.ContainsKey(
                    ReservedMetadataKeys.IsSelected);
            }
            else if (! (bRightButtonIsPressed ||
                oClickedVertex.ContainsKey(ReservedMetadataKeys.IsSelected) ) )
            {
                // Clear the selection.

                SetAllVerticesSelected(false);
                SetAllEdgesSelected(false);

                Debug.Assert(m_oSelectedVertices.Count == 0);
                Debug.Assert(m_oSelectedEdges.Count == 0);
            }

            // Select or deselect the clicked vertex and possibly its incident
            // edges.

            SetVertexSelected(oClickedVertex, bSelectVertex,
                m_eMouseSelectionMode ==
                    MouseSelectionMode.SelectVertexAndIncidentEdges
                );
        }

        if (m_bAllowVertexDrag && LeftButtonIsPressed(e) &&
            m_oSelectedVertices.Count > 0)
        {
            // The user might want to drag the selected vertices.  Save the
            // mouse location for use within the MouseMove event.

            m_oVerticesBeingDragged = new DraggedVertices(

                CollectionUtil.CollectionToArray<IVertex>(
                    m_oSelectedVertices.Keys),

                oMouseLocation, this.GraphRectangle,
                m_oAsyncLayout.Margin);

            this.Cursor = Cursors.ScrollAll;
            Mouse.Capture(this);
        }
    }

    //*************************************************************************
    //  Method: OnMouseMove()
    //
    /// <summary>
    /// Handles the MouseMove event.
    /// </summary>
    ///
    /// <param name="e">
    /// The MouseEventArgs that contains the event data.
    /// </param>
    //*************************************************************************

    protected override void
    OnMouseMove
    (
        MouseEventArgs e
    )
    {
        AssertValid();

        // Do nothing if the drawing isn't in a stable state.

        if (this.IsDrawing)
        {
            return;
        }

        // Check for a mouse drag that might be in progress.

        CheckForVertexDragOnMouseMove(e);
        CheckForMarqueeDragOnMouseMove(e);
        CheckForTranslationDragOnMouseMove(e);

        // Check whether tooltip-related actions need to be taken.

        CheckForToolTipsOnMouseMove(e);
    }

    //*************************************************************************
    //  Method: OnMouseUp()
    //
    /// <summary>
    /// Handles the MouseUp event.
    /// </summary>
    ///
    /// <param name="e">
    /// The MouseButtonEventArgs that contains the event data.
    /// </param>
    //*************************************************************************

    protected override void
    OnMouseUp
    (
        MouseButtonEventArgs e
    )
    {
        AssertValid();

        // Restore the default cursor and release the mouse capture.

        this.Cursor = null;
        Mouse.Capture(null);

        // Do nothing if the drawing isn't in a stable state.

        if (this.IsDrawing)
        {
            return;
        }

        // Check whether the user clicked on a vertex.

        Point oMouseLocation = e.GetPosition(this);
        IVertex oClickedVertex;

        TryGetVertexFromPoint(oMouseLocation, out oClickedVertex);

        FireGraphMouseUp(e, oClickedVertex);

        // If it was the right button that was released, do nothing else.

        if (e.LeftButton == MouseButtonState.Released)
        {
            // Check for a mouse drag that might be in progress.

            CheckForVertexDragOnMouseUp(e);
            CheckForMarqueeDragOnMouseUp(e);
            CheckForTranslationDragOnMouseUp(e);
        }
    }

    //*************************************************************************
    //  Method: OnMouseLeave()
    //
    /// <summary>
    /// Handles the MouseLeave event.
    /// </summary>
    ///
    /// <param name="e">
    /// The MouseEventArgs that contains the event data.
    /// </param>
    //*************************************************************************

    protected override void
    OnMouseLeave
    (
        MouseEventArgs e
    )
    {
        AssertValid();

        // Remove any vertex tooltip that might exist and reset the helper
        // object that figures out when to show tooltips.

        ResetVertexToolTipTracker();
    }

    //*************************************************************************
    //  Method: OnMouseWheel()
    //
    /// <summary>
    /// Handles the MouseWheel event.
    /// </summary>
    ///
    /// <param name="e">
    /// The MouseWheelEventArgs that contains the event data.
    /// </param>
    //*************************************************************************

    protected override void
    OnMouseWheel
    (
        MouseWheelEventArgs e
    )
    {
        AssertValid();

        e.Handled = true;

        // Do nothing if the drawing isn't in a stable state or a drag is in
        // progress.

        if ( this.IsDrawing || DragMightBeInProgress() )
        {
            return;
        }

        Double dGraphZoom = this.GraphZoom;
        Double dNewGraphZoom = dGraphZoom;
        Int32 iWheelDelta = e.Delta;

        if (iWheelDelta > 0)
        {
            dNewGraphZoom = Math.Min(
                dGraphZoom * GraphZoomFactor,
                MaximumGraphZoom);
        }
        else
        {
            dNewGraphZoom = Math.Max(
                dGraphZoom / GraphZoomFactor,
                MinimumGraphZoom);
        }

        if (dNewGraphZoom == dGraphZoom)
        {
            return;
        }

        // Set the center of the zoom to the mouse position.  Note that the
        // mouse position is affected by the ScaleTransform used for this
        // control's layout transform and needs to be adjusted for this.

        Point oMousePosition = e.GetPosition(this);

        ScaleTransform oScaleTransformForLayout = this.ScaleTransformForLayout;
        Double dScaleForLayout = oScaleTransformForLayout.ScaleX;
        ScaleTransform oScaleTransformForRender = this.ScaleTransformForRender;
        oScaleTransformForRender.CenterX = oMousePosition.X * dScaleForLayout;
        oScaleTransformForRender.CenterY = oMousePosition.Y * dScaleForLayout;

        // Zoom the graph.

        SetGraphZoom(dNewGraphZoom, false);

        // That caused the point under the mouse to shift.  Adjust the
        // translation to shift the point back.

        Point oNewMousePosition = e.GetPosition(this);

        TranslateTransform oTranslateTransformForRender =
            this.TranslateTransformForRender;

        oTranslateTransformForRender.X += dScaleForLayout *
            (oNewMousePosition.X - oMousePosition.X) * dNewGraphZoom;

        oTranslateTransformForRender.Y += dScaleForLayout *
            (oNewMousePosition.Y - oMousePosition.Y) * dNewGraphZoom;

        LimitTranslation();
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

        #if TRACE_LAYOUT_AND_DRAW
        Debug.WriteLine("NodeXLControl:"
            + " AsyncLayout_LayOutGraphIterationCompleted()");
        #endif

        // Note that the worker thread on which the layout is occurring blocks
        // until this event handler returns.  Therefore, it is okay for
        // LayOutOrDrawGraph() to use the graph's vertex locations and
        // metadata, even though the worker thread modifies that same data
        // while the worker thread is running.

        m_eLayoutState = LayoutState.LayoutIterationCompleted;
        LayOutOrDrawGraph();
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

        #if TRACE_LAYOUT_AND_DRAW
        Debug.WriteLine("NodeXLControl: AsyncLayout_LayOutGraphCompleted()");
        #endif

        FireGraphDrawn();

        if (oAsyncCompletedEventArgs.Error != null)
        {
            throw new InvalidOperationException(
                "A problem occurred while laying out the graph.",
                oAsyncCompletedEventArgs.Error);
        }
        else if (oAsyncCompletedEventArgs.Cancelled)
        {
        }
        else
        {
            // The asynchronous layout has completed and now the graph needs to
            // be drawn.
        }

        m_eLayoutState = LayoutState.LayoutCompleted;
        LayOutOrDrawGraph();
    }

    //*************************************************************************
    //  Method: VertexToolTipTracker_ShowToolTip()
    //
    /// <summary>
    /// Handles the ShowToolTip event on the m_oVertexToolTipTracker object.
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
    VertexToolTipTracker_ShowToolTip
    (
        Object oSource,
        ToolTipTrackerEventArgs oToolTipTrackerEventArgs
    )
    {
        AssertValid();

        // Get the vertex that was hovered over.

        Debug.Assert(oToolTipTrackerEventArgs.Object is IVertex);

        IVertex oVertex = (IVertex)oToolTipTrackerEventArgs.Object;

        // Fire a VertexMouseHover event in case the application wants to take
        // additional action.

        FireVertexMouseHover(oVertex);

        if (!m_bShowVertexToolTips)
        {
            // Vertex tooltips aren't being shown, so no additional action is
            // required.

            return;
        }

        // Give PreviewVertexToolTipShown event handlers an opportunity to
        // override the default tooltip.

        VertexToolTipShownEventArgs oVertexToolTipShownEventArgs =
            new VertexToolTipShownEventArgs(oVertex);

        FirePreviewVertexToolTipShown(oVertexToolTipShownEventArgs);

        m_oVertexToolTip = oVertexToolTipShownEventArgs.VertexToolTip;

        if (m_oVertexToolTip == null)
        {
            // There was no default tooltip override.  Does the vertex have a
            // tooltip string stored in its metadata?

            Object oVertexToolTipAsObject;

            if ( !oVertex.TryGetValue(ReservedMetadataKeys.VertexToolTip,
                typeof(String), out oVertexToolTipAsObject) )
            {
                // No.  Nothing needs to be done.

                return;
            }

            // Create a default tooltip.

            m_oVertexToolTip = CreateDefaultVertexToolTip(
                (String)oVertexToolTipAsObject);
        }

        m_oGraphDrawer.AddVisualOnTopOfGraph(m_oVertexToolTip);

        // If this isn't called, MeasureOverride() may not be called and
        // m_oVertexToolTip won't get sized.

        this.InvalidateMeasure();
    }

    //*************************************************************************
    //  Method: VertexToolTipTracker_HideToolTip()
    //
    /// <summary>
    /// Handles the HideToolTip event on the m_oVertexToolTipTracker object.
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
    VertexToolTipTracker_HideToolTip
    (
        Object oSource,
        ToolTipTrackerEventArgs oToolTipTrackerEventArgs
    )
    {
        AssertValid();

        FireVertexMouseLeave();

        if (m_bShowVertexToolTips)
        {
            RemoveVertexToolTip();
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

    public virtual void
    AssertValid()
    {
        Debug.Assert(m_oGraph != null);
        Debug.Assert(m_oGraphDrawer != null);
        Debug.Assert(m_oAsyncLayout != null);
        Debug.Assert(m_oLastLayoutContext != null);
        // m_oLastGraphDrawingContext
        // m_eLayoutState
        // m_eMouseSelectionMode
        // m_bAllowVertexDrag
        // m_oVerticesBeingDragged
        // m_oMarqueeBeingDragged
        // m_oTranslationBeingDragged
        Debug.Assert(m_oSelectedVertices != null);
        Debug.Assert(m_oSelectedEdges != null);
        // m_bShowVertexToolTips
        // m_oLastMouseMoveLocation
        Debug.Assert(m_oVertexToolTipTracker != null);
        // m_oVertexToolTip
        // m_bGraphZoomCentered
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// Minimum value of the <see cref="GraphScale" /> property.  The value is
    /// 1.0.
    /// </summary>

    public const Double MinimumGraphScale = 1.0;

    /// <summary>
    /// Maximum value of the <see cref="GraphScale" /> property.  The value is
    /// 10.0.
    /// </summary>

    public const Double MaximumGraphScale = 10.0;

    /// <summary>
    /// Minimum value of the <see cref="GraphZoom" /> property.  The value is
    /// 1.0.
    /// </summary>

    public const Double MinimumGraphZoom = 1.0;

    /// <summary>
    /// Maximum value of the <see cref="GraphZoom" /> property.  The value is
    /// 10.0.
    /// </summary>

    public const Double MaximumGraphZoom = 10.0;


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Key the user must hold down to translate the graph with the mouse.

    protected const Key TranslationDragKey = Key.Space;

    /// Factor used by OnMouseWheel() to compute a new zoom value.

    protected const Double GraphZoomFactor = 1.10;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The graph being drawn.

    protected IGraph m_oGraph;

    /// Draws the graph onto a collection of Visual objects.

    protected GraphDrawer m_oGraphDrawer;

    /// Object used to lay out the graph.

    protected IAsyncLayout m_oAsyncLayout;

    /// Layout context most recently used to lay out the graph.

    protected LayoutContext m_oLastLayoutContext;

    /// GraphDrawingContext most recently used to draw the graph, or null if
    /// the graph hasn't been drawn yet.

    protected GraphDrawingContext m_oLastGraphDrawingContext;

    /// Indicates the state of the graph's layout.

    protected LayoutState m_eLayoutState;

    /// Determines what gets selected when a vertex is clicked with the mouse.

    protected MouseSelectionMode m_eMouseSelectionMode;

    /// true if a vertex can be moved by dragging it with the mouse.

    protected Boolean m_bAllowVertexDrag;

    /// Vertex the user is dragging with the mouse, or null if a vertex isn't
    /// being dragged.

    protected DraggedVertices m_oVerticesBeingDragged;

    /// Marquee the user is dragging with the mouse, or null if a marquee isn't
    /// being dragged.

    protected DraggedMarquee m_oMarqueeBeingDragged;

    /// The translation the user is dragging with the mouse, or null if a
    /// translation isn't being dragged.

    protected DraggedTranslation m_oTranslationBeingDragged;

    /// Selected vertices and edges.  Dictionaries are used instead of lists or
    /// arrays to prevent the same vertex or edge from being added twice.  The
    /// keys are IVertex or IEdge and the values aren't used.

    Dictionary<IVertex, Byte> m_oSelectedVertices;
    ///
    Dictionary<IEdge, Byte> m_oSelectedEdges;

    /// true to show vertex tooltips.

    protected Boolean m_bShowVertexToolTips;

    /// Location of the mouse during the most recent MouseMove event, or
    /// (-1,-1) if that event hasn't fired yet.

    protected Point m_oLastMouseMoveLocation;

    /// Helper object for figuring out when to show vertex tooltips.

    private WpfToolTipTracker m_oVertexToolTipTracker;

    /// The vertex tooltip being displayed, or null if no vertex tooltip is
    /// being displayed.

    protected UIElement m_oVertexToolTip;

    /// See OnRenderSizeChanged().

    protected Boolean m_bGraphZoomCentered;
}

}
