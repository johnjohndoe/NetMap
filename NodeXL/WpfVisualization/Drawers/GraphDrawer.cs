
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: GraphDrawer
//
/// <summary>
/// Draws a NodeXL graph onto a collection of Visual objects.
/// </summary>
///
/// <remarks>
/// This is used to draw a NodeXL graph in a WPF application.  It contains a
/// collection of Visual objects that represent the graph's vertices and
/// edges.  Call <see cref="DrawGraph" /> to draw a laid-out NodeXL graph onto
/// the contained <see cref="GraphDrawer.VisualCollection" />.
///
/// <para>
/// <see cref="GraphDrawer" /> does not lay out the graph.  You should lay out
/// the graph using one of the provided layout classes before calling <see
/// cref="DrawGraph" />.
/// </para>
///
/// <para>
/// A <see cref="GraphDrawer" /> cannot be directly rendered and is typically
/// not used directly by an application.  Applications typically use one of two
/// other NodeXL graph-drawing classes:
/// </para>
///
/// <list type="number">
///
/// <item><description>
/// NodeXLControl, which is a FrameworkElement that wraps a <see
/// cref="GraphDrawer" /> and hosts its <see
/// cref="GraphDrawer.VisualCollection" />.  NodeXLControl is meant for use in
/// WPF desktop applications.  It automatically lays out the graph before
/// drawing it.
/// </description></item>
///
/// <item><description>
/// <see cref="NodeXLVisual" />, which is a Visual that wraps a <see
/// cref="GraphDrawer" /> and hosts its <see
/// cref="GraphDrawer.VisualCollection" />.  This is a lower-level alternative
/// to NodeXLControl and can be used anywhere a Visual is more appropriate than
/// a FrameworkElement.  Like <see cref="GraphDrawer" />, <see
/// cref="NodeXLVisual" /> does not lay out the graph before drawing it.
/// </description></item>
///
/// </list>
///
/// <para>
/// If you do use <see cref="GraphDrawer" /> directly, rendering the graph
/// requires a custom wrapper that hosts the GraphDrawer.<see
/// cref="GraphDrawer.VisualCollection" /> object.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class GraphDrawer : DrawerBase
{
    //*************************************************************************
    //  Constructor: GraphDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphDrawer" /> class.
    /// </summary>
    ///
    /// <param name="parentVisual">
    /// The parent of the contained <see
    /// cref="GraphDrawer.VisualCollection" />.  This is usually a
    /// FrameworkElement that is hosting the collection.
    /// </param>
    //*************************************************************************

    public GraphDrawer
    (
        Visual parentVisual
    )
    {
        Debug.Assert(parentVisual != null);

        m_oVisualCollection = new VisualCollection(parentVisual);
        m_oAllVertexDrawingVisuals = null;
        m_oUnselectedEdgeDrawingVisuals = null;
        m_oSelectedEdgeDrawingVisuals = null;
        m_oVertexDrawer = new VertexDrawer();
        m_oEdgeDrawer = new EdgeDrawer();
        m_oBackColor = SystemColors.WindowColor;

        // Forward the events fired by the vertex and edge drawers.

        m_oVertexDrawer.RedrawRequired +=
            delegate { this.FireRedrawRequired(); };

        m_oVertexDrawer.LayoutRequired +=
            delegate { this.FireLayoutRequired(); };

        m_oEdgeDrawer.RedrawRequired +=
            delegate { this.FireRedrawRequired(); };

        m_oEdgeDrawer.LayoutRequired +=
            delegate { this.FireLayoutRequired(); };

        AssertValid();
    }

    //*************************************************************************
    //  Property: VisualCollection
    //
    /// <summary>
    /// Gets the VisualCollection that contains the Visual objects representing
    /// the graph's vertices and edges.
    /// </summary>
    ///
    /// <value>
    /// The VisualCollection that contains the Visual objects representing the
    /// graph's vertices and edges.
    /// </value>
    ///
    /// <remarks>
    /// This should be treated as read-only and used only to host the graph in
    /// a FrameworkElement.  Its contents should not be modified.  If you want
    /// to add a Visual on top of the graph, call <see
    /// cref="AddVisualOnTopOfGraph" /> after calling <see cref="DrawGraph" />.
    ///
    /// <para>
    /// <see cref="DrawGraph" /> draws a NodeXL graph onto this collection.
    /// </para>
    ///
    /// </remarks>
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
    ///
    /// <remarks>
    /// This property is provided to allow the caller to access the <see
    /// cref="Wpf.VertexDrawer" /> properties and methods that affect the
    /// graph's appearance, such as <see cref="Wpf.VertexDrawer.Shape" />.
    /// </remarks>
    //*************************************************************************

    public VertexDrawer
    VertexDrawer
    {
        get
        {
            AssertValid();

            return (m_oVertexDrawer);
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
    ///
    /// <remarks>
    /// This property is provided to allow the caller to access the <see
    /// cref="Wpf.EdgeDrawer" /> properties that affect the graph's appearance,
    /// such as <see cref="Wpf.EdgeDrawer.Width" />.
    /// </remarks>
    //*************************************************************************

    public EdgeDrawer
    EdgeDrawer
    {
        get
        {
            AssertValid();

            return (m_oEdgeDrawer);
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

            return (m_oBackColor);
        }

        set
        {
            if (m_oBackColor == value)
            {
                return;
            }

            m_oBackColor = value;

            FireRedrawRequired();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: DrawGraph()
    //
    /// <summary>
    /// Draws a laid-out graph onto the contained collection of Visual objects.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to draw onto the contained collection of Visual objects.  The
    /// graph should have already been laid out.  You can use one of the
    /// supplied layout classes to do this.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <remarks>
    /// If you want to add a Visual on top of the graph, call <see
    /// cref="AddVisualOnTopOfGraph" /> after this method returns.
    ///
    /// <para>
    /// The collection of Visual objects is accessible via the <see
    /// cref="GraphDrawer.VisualCollection" /> property.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    DrawGraph
    (
        IGraph graph,
        GraphDrawingContext graphDrawingContext
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(graphDrawingContext != null);
        AssertValid();

        // Implementation note:
        //
        // In a previous GDI+ implementation of this graph drawer, the edges
        // had to be drawn first to allow the vertices to cover the ends of the
        // edges.  That required a complex three-step drawing process: 1) allow
        // the vertex drawer to move each vertex if necessary to prevent the
        // vertex from falling outside the graph rectangle; 2) draw the edges
        // using the moved vertex locations; and 3) draw the vertices.
        //
        // This WPF implementation is simpler, for two reasons:
        //
        // 1. WPF uses retained-mode graphics, so covering the ends of the
        //    edges can be accomplished simply by adding
        //    m_oUnselectedEdgeDrawingVisuals to m_oVisualCollection before
        //    adding m_oAllVertexDrawingVisuals.  That means that the vertices
        //    can be drawn onto m_oAllVertexDrawingVisuals first, and the
        //    vertex drawer can move the vertices as necessary before drawing
        //    them.  A three-step process is no longer required.
        //
        // 2. The edges in this implementation don't actually need to be
        //    covered, because they terminate at the vertex boundaries instead
        //    of the vertex centers, as in the GDI+ implementation.

        m_oVisualCollection.Clear();

        DrawBackground(graphDrawingContext);

        m_oAllVertexDrawingVisuals = new DrawingVisual();
        m_oUnselectedEdgeDrawingVisuals = new DrawingVisual();
        m_oSelectedEdgeDrawingVisuals = new DrawingVisual();

        // Draw the vertices after moving them if necessary.  Each vertex needs
        // to be individually hit-tested and possibly redrawn by
        // RedrawVertex(), so each vertex is put into its own DrawingVisual
        // that becomes a child of m_oAllVertexDrawingVisuals.

        foreach (IVertex oVertex in graph.Vertices)
        {
            DrawVertex(oVertex, graphDrawingContext);
        }

        // Draw the edges.  The edges don't need to be hit-tested, but they
        // might need to be redrawn by RedrawEdge(), so each edge is put into
        // its own DrawingVisual that becomes a child of either
        // m_oUnselectedEdgeDrawingVisuals or m_oSelectedEdgeDrawingVisuals.

        foreach (IEdge oEdge in graph.Edges)
        {
            DrawEdge(oEdge, graphDrawingContext);
        }

        // Selected edges need to be drawn on top of all the vertices
        // (including selected vertices) to guarantee that they will be
        // visible; hence the addition order seen here.

        m_oVisualCollection.Add(m_oUnselectedEdgeDrawingVisuals);
        m_oVisualCollection.Add(m_oAllVertexDrawingVisuals);
        m_oVisualCollection.Add(m_oSelectedEdgeDrawingVisuals);
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

        if (m_oAllVertexDrawingVisuals == null)
        {
            // The graph hasn't been drawn yet.

            return (false);
        }

        // The vertices are represented by DrawingVisual child objects of
        // m_oAllVertexDrawingVisuals.

        HitTestResult oHitTestResult =
            m_oAllVertexDrawingVisuals.HitTest(point);

        if (oHitTestResult != null)
        {
            DependencyObject oVisualHit = oHitTestResult.VisualHit;

            if ( oVisualHit.GetType() == typeof(DrawingVisual) )
            {
                // Retrieve the vertex.

                vertex = RetrieveVertexFromDrawingVisual(
                    (DrawingVisual)oVisualHit);

                return (true);
            }
        }

        return (false);
    }

    //*************************************************************************
    //  Method: GetVerticesFromRectangle()
    //
    /// <summary>
    /// Gets any vertices that intersect a specified <see cref="Rect" />.
    /// </summary>
    ///
    /// <param name="rectangle">
    /// Rectangle to use.
    /// </param>
    ///
    /// <returns>
    /// A collection of vertices that intersect <paramref name="rectangle" />.
    /// </returns>
    ///
    /// <remarks>
    /// This method return a collection of all vertices that intersect
    /// <paramref name="rectangle" />.  The returned collection may be empty
    /// but is never null.
    /// </remarks>
    //*************************************************************************

    public ICollection<IVertex>
    GetVerticesFromRectangle
    (
        Rect rectangle
    )
    {
        AssertValid();

        LinkedList<IVertex> oVertices = new LinkedList<IVertex>();

        if (m_oAllVertexDrawingVisuals != null)
        {
            // Hit-test using an anonymous delegate to avoid having to create
            // a named callback method.

            m_oAllVertexDrawingVisuals.HitTest(null,
            
                delegate (HitTestResult hitTestResult)
                {
                    DependencyObject oVisualHit = hitTestResult.VisualHit;

                    if ( oVisualHit.GetType() == typeof(DrawingVisual) )
                    {
                        // Retrieve the vertex.

                        oVertices.AddLast( RetrieveVertexFromDrawingVisual(
                            (DrawingVisual)oVisualHit) );
                    }

                    return (HitTestResultBehavior.Continue);
                }, 

                new GeometryHitTestParameters(
                    new RectangleGeometry(rectangle) )
                );
        }

        return (oVertices);
    }

    //*************************************************************************
    //  Method: RedrawVertex()
    //
    /// <summary>
    /// Redraws a vertex that was drawn by <see cref="DrawGraph" />.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to redraw onto the contained collection of Visual objects.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.  This
    /// must be the same object that was passed to <see cref="DrawGraph" /> the
    /// last time the entire graph was drawn.
    /// </param>
    ///
    /// <remarks>
    /// Use this method to redraw a vertex whose attributes (such as its
    /// selected state) have changed without incurring the overhead of
    /// redrawing the entire graph.
    /// </remarks>
    //*************************************************************************

    public void
    RedrawVertex
    (
        IVertex vertex,
        GraphDrawingContext graphDrawingContext
    )
    {
        Debug.Assert(vertex != null);
        Debug.Assert(graphDrawingContext != null);
        AssertValid();

        // Retrieve the VertexDrawingHistory object for the vertex, if one
        // exists.  (If the vertex was previously hidden, there won't be a
        // VertexDrawingHistory object for it.)

        Dictionary<Int32, VertexDrawingHistory> oVertexDrawingHistories =
            graphDrawingContext.VertexDrawingHistories;

        Int32 iVertexID = vertex.ID;
        VertexDrawingHistory oVertexDrawingHistory;

        if ( oVertexDrawingHistories.TryGetValue(
            iVertexID, out oVertexDrawingHistory) )
        {
            // Remove the VertexDrawingHistory object from the dictionary.

            oVertexDrawingHistories.Remove(iVertexID);

            // Remove the vertex's DrawingVisual object, which will cause the
            // vertex to disappear.

            m_oAllVertexDrawingVisuals.Children.Remove(
                oVertexDrawingHistory.DrawingVisual);
        }

        // Redraw the vertex.  This adds a replacement DrawingVisual object to
        // m_oAllVertexDrawingVisuals and adds a replacement
        // VertexDrawingHistory object to the
        // graphDrawingContext.VertexDrawingHistories dictionary.

        DrawVertex(vertex, graphDrawingContext);
    }

    //*************************************************************************
    //  Method: RedrawEdge()
    //
    /// <summary>
    /// Redraws an edge that was drawn by <see cref="DrawGraph" />.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to redraw onto the contained collection of Visual objects.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.  This
    /// must be the same object that was passed to <see cref="DrawGraph" /> the
    /// last time the entire graph was drawn.
    /// </param>
    ///
    /// <remarks>
    /// Use this method to redraw an edge whose attributes (such as its
    /// selected state) have changed without incurring the overhead of
    /// redrawing the entire graph.
    /// </remarks>
    //*************************************************************************

    public void
    RedrawEdge
    (
        IEdge edge,
        GraphDrawingContext graphDrawingContext
    )
    {
        Debug.Assert(edge != null);
        Debug.Assert(graphDrawingContext != null);
        AssertValid();

        // Retrieve the EdgeDrawingHistory object for the edge, if one exists.
        // (If the edge was previously hidden, there won't be an
        // EdgeDrawingHistory object for it.)

        Dictionary<Int32, EdgeDrawingHistory> oEdgeDrawingHistories =
            graphDrawingContext.EdgeDrawingHistories;

        Int32 iEdgeID = edge.ID;
        EdgeDrawingHistory oEdgeDrawingHistory;

        if ( oEdgeDrawingHistories.TryGetValue(
            iEdgeID, out oEdgeDrawingHistory) )
        {
            // Remove the EdgeDrawingHistory object from the dictionary.

            oEdgeDrawingHistories.Remove(iEdgeID);

            // Remove the edge's DrawingVisual object, which will cause the
            // edge to disappear.

            GetEdgeDrawingVisuals(oEdgeDrawingHistory).Children.Remove(
                oEdgeDrawingHistory.DrawingVisual);
        }

        // Redraw the edge.  This adds a replacement DrawingVisual object to
        // either m_oUnselectedEdgeDrawingVisuals or
        // m_oSelectedEdgeDrawingVisuals and adds a replacement
        // EdgeDrawingHistory object to the
        // graphDrawingContext.EdgeDrawingHistories dictionary.

        DrawEdge(edge, graphDrawingContext);
    }

    //*************************************************************************
    //  Method: AddVisualOnTopOfGraph()
    //
    /// <summary>
    /// Adds a caller-supplied Visual on top of the graph.
    /// </summary>
    ///
    /// <param name="visual">
    /// The Visual to add on top of the graph.
    /// </param>
    ///
    /// <remarks>
    /// Call this method after calling <see cref="DrawGraph" /> to add a Visual
    /// on top of the graph.  The added Visual gets removed when <see
    /// cref="DrawGraph" /> is called again.  You can also remove the Visual
    /// without redrawing the graph by calling <see
    /// cref="RemoveVisualFromTopOfGraph" />.
    ///
    /// <para>
    /// An InvalidOperationException is thrown if the Visual has already been
    /// added.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    AddVisualOnTopOfGraph
    (
        Visual visual
    )
    {
        Debug.Assert(visual != null);
        AssertValid();

        const String MethodName = "AddVisualOnTopOfGraph";

        if ( m_oVisualCollection.Contains(visual) )
        {
            Debug.Assert(false);

            throw new InvalidOperationException( String.Format(

                "{0}.{1}: Visual has already been added."
                ,
                this.ClassName,
                MethodName
                ) );
        }

        m_oVisualCollection.Add(visual);
    }

    //*************************************************************************
    //  Method: RemoveVisualFromTopOfGraph()
    //
    /// <summary>
    /// Removes the caller-supplied Visual from the top of the graph.
    /// </summary>
    ///
    /// <param name="visual">
    /// The Visual to remove from the top of the graph.
    /// </param>
    ///
    /// <remarks>
    /// Use this method to remove the Visual added to the top of the graph by
    /// <see cref="AddVisualOnTopOfGraph" /> without redrawing the graph.  The
    /// Visual also gets removed if <see cref="DrawGraph" /> is called again.
    ///
    /// <para>
    /// If the Visual doesn't exist in the VisualCollection, which will occur
    /// if <see cref="DrawGraph" /> was called after the Visual was added, this
    /// method does nothing.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    RemoveVisualFromTopOfGraph
    (
        Visual visual
    )
    {
        Debug.Assert(visual != null);
        AssertValid();

        // The documentation doesn't say what Remove() does if the Visual isn't
        // contained in the collection, so check first.

        if ( m_oVisualCollection.Contains(visual) )
        {
            m_oVisualCollection.Remove(visual);
        }
    }

    //*************************************************************************
    //  Method: DrawBackground()
    //
    /// <summary>
    /// Draws the graph's background.
    /// </summary>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    //*************************************************************************

    protected void
    DrawBackground
    (
        GraphDrawingContext oGraphDrawingContext
    )
    {
        Debug.Assert(oGraphDrawingContext != null);
        AssertValid();

        DrawingVisual oBackgroundDrawingVisual = new DrawingVisual();

        using ( DrawingContext oDrawingContext =
            oBackgroundDrawingVisual.RenderOpen() )
        {
            oDrawingContext.DrawRectangle(
                CreateFrozenSolidColorBrush(m_oBackColor), null,
                oGraphDrawingContext.GraphRectangle);
        }

        m_oVisualCollection.Add(oBackgroundDrawingVisual);
    }

    //*************************************************************************
    //  Method: DrawVertex()
    //
    /// <summary>
    /// Draws a vertex onto the contained collection of Visual objects.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to draw onto the contained collection of Visual objects.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <remarks>
    /// This method adds a DrawingVisual for the vertex to
    /// m_oAllVertexDrawingVisuals and adds a VertexDrawingHistory to
    /// oGraphDrawingContext.VertexDrawingHistories.
    /// </remarks>
    //*************************************************************************

    protected void
    DrawVertex
    (
        IVertex oVertex,
        GraphDrawingContext oGraphDrawingContext
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oGraphDrawingContext != null);
        AssertValid();

        VertexDrawingHistory oVertexDrawingHistory;

        Dictionary<Int32, VertexDrawingHistory> oVertexDrawingHistories =
            oGraphDrawingContext.VertexDrawingHistories;

        if ( m_oVertexDrawer.TryDrawVertex(oVertex, oGraphDrawingContext,
            out oVertexDrawingHistory) )
        {
            Debug.Assert(oVertexDrawingHistory != null);

            // Add the DrawingVisual for the vertex to
            // m_oAllVertexDrawingVisuals, and add the VertexDrawingHistory to
            // the oGraphDrawingContext.VertexDrawingHistories dictionary.

            DrawingVisual oVertexChildDrawingVisual =
                oVertexDrawingHistory.DrawingVisual;

            m_oAllVertexDrawingVisuals.Children.Add(oVertexChildDrawingVisual);

            oVertexDrawingHistories.Add(oVertex.ID, oVertexDrawingHistory);

            // Save the vertex on the DrawingVisual for later retrieval.

            SaveVertexOnDrawingVisual(oVertex, oVertexChildDrawingVisual);
        }
    }

    //*************************************************************************
    //  Method: DrawEdge()
    //
    /// <summary>
    /// Draws an edge onto the contained collection of Visual objects.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to draw onto the contained collection of Visual objects.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <remarks>
    /// This method adds a DrawingVisual for the edge to either
    /// m_oUnselectedEdgeDrawingVisuals or m_oSelectedEdgeDrawingVisuals, and
    /// adds an EdgeDrawingHistory to
    /// oGraphDrawingContext.EdgeDrawingHistories.
    /// </remarks>
    //*************************************************************************

    protected void
    DrawEdge
    (
        IEdge oEdge,
        GraphDrawingContext oGraphDrawingContext
    )
    {
        Debug.Assert(oEdge != null);
        Debug.Assert(oGraphDrawingContext != null);
        AssertValid();

        EdgeDrawingHistory oEdgeDrawingHistory;

        Dictionary<Int32, EdgeDrawingHistory> oEdgeDrawingHistories =
            oGraphDrawingContext.EdgeDrawingHistories;

        if ( m_oEdgeDrawer.TryDrawEdge(oEdge, oGraphDrawingContext,
            out oEdgeDrawingHistory) )
        {
            Debug.Assert(oEdgeDrawingHistory != null);

            GetEdgeDrawingVisuals(oEdgeDrawingHistory).Children.Add(
                oEdgeDrawingHistory.DrawingVisual);

            // Save the EdgeDrawingHistory object.

            oEdgeDrawingHistories.Add(oEdge.ID, oEdgeDrawingHistory);
        }
    }

    //*************************************************************************
    //  Method: GetEdgeDrawingVisuals()
    //
    /// <summary>
    /// Gets a DrawingVisual that contains all the visuals for either 
    /// unselected edges or selected edges.
    /// </summary>
    ///
    /// <param name="oEdgeDrawingHistory">
    /// Determines which DrawingVisual gets returned.
    /// </param>
    ///
    /// <returns>
    /// If <paramref name="oEdgeDrawingHistory" /> indicates that the edge was
    /// drawn as unselected, then the DrawingVisual that contains all the
    /// visuals for unselected edges is returned.  Otherwise, the DrawingVisual
    /// that contains all the visuals for selected edges is returned.
    /// </returns>
    //*************************************************************************

    protected DrawingVisual
    GetEdgeDrawingVisuals
    (
        EdgeDrawingHistory oEdgeDrawingHistory
    )
    {
        Debug.Assert(oEdgeDrawingHistory != null);
        AssertValid();

        return (oEdgeDrawingHistory.DrawnAsSelected ?
            m_oSelectedEdgeDrawingVisuals : m_oUnselectedEdgeDrawingVisuals);
    }

    //*************************************************************************
    //  Method: SaveVertexOnDrawingVisual()
    //
    /// <summary>
    /// Saves a vertex on the DrawingVisual with which the vertex was drawn.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex that was drawn.
    /// </param>
    ///
    /// <param name="oDrawingVisual">
    /// The DrawingVisual with which <paramref name="oVertex" /> was drawn.
    /// </param>
    ///
    /// <remarks>
    /// The vertex can be retrieved from the DrawingVisual with <see
    /// cref="RetrieveVertexFromDrawingVisual" />.
    /// </remarks>
    //*************************************************************************

    protected void
    SaveVertexOnDrawingVisual
    (
        IVertex oVertex,
        DrawingVisual oDrawingVisual
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oDrawingVisual != null);
        AssertValid();

        // DrawingVisual has no Tag property, so use FrameworkElement's Tag
        // property as an attached property.

        oDrawingVisual.SetValue(FrameworkElement.TagProperty, oVertex);
    }

    //*************************************************************************
    //  Method: RetrieveVertexFromDrawingVisual()
    //
    /// <summary>
    /// Retrieves a vertex from the DrawingVisual with which the vertex was
    /// drawn.
    /// </summary>
    ///
    /// <param name="oDrawingVisual">
    /// The DrawingVisual to retrieve a vertex from.
    /// </param>
    ///
    /// <returns>
    /// The vertex that was drawn with <paramref name="oDrawingVisual" />.
    /// </returns>
    ///
    /// <remarks>
    /// This method retrieves the vertex that was saved on a DrawingVisual by
    /// <see cref="SaveVertexOnDrawingVisual" />.
    /// </remarks>
    //*************************************************************************

    protected IVertex
    RetrieveVertexFromDrawingVisual
    (
        DrawingVisual oDrawingVisual
    )
    {
        Debug.Assert(oDrawingVisual != null);
        AssertValid();

        Object oVertexAsObject =
            oDrawingVisual.GetValue(FrameworkElement.TagProperty);

        Debug.Assert(oVertexAsObject is IVertex);

        return ( (IVertex)oVertexAsObject );
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

        Debug.Assert(m_oVisualCollection != null);
        // m_oAllVertexDrawingVisuals
        // m_oUnselectedEdgeDrawingVisuals
        // m_oSelectedEdgeDrawingVisuals
        Debug.Assert(m_oVertexDrawer != null);
        Debug.Assert(m_oEdgeDrawer != null);
        // m_oBackColor
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Collection of Visual objects that represent the graph's vertices and
    /// edges.

    protected VisualCollection m_oVisualCollection;

    /// Visual that contains all the vertex visuals, or null if the graph
    /// hasn't been drawn yet.

    protected DrawingVisual m_oAllVertexDrawingVisuals;

    /// Visual that contains all the visuals for unselected edges, or null if
    /// the graph hasn't been drawn yet.

    protected DrawingVisual m_oUnselectedEdgeDrawingVisuals;

    /// Visual that contains all the visuals for selected edges, or null if the
    /// graph hasn't been drawn yet.

    protected DrawingVisual m_oSelectedEdgeDrawingVisuals;

    /// Draws the graph's vertices.

    protected VertexDrawer m_oVertexDrawer;

    /// Draws the graph's edges.

    protected EdgeDrawer m_oEdgeDrawer;

    /// Background color.

    protected Color m_oBackColor;
}

}
