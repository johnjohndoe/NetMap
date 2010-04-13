
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Layouts
{
//*****************************************************************************
//  Class: LayoutBase
//
/// <summary>
/// Base class for layouts.
/// </summary>
///
/// <remarks>
/// This abstract class can serve as a base class for <see
/// cref="ILayout" /> implementations.  Its implementations of the <see
/// cref="ILayout" /> public methods provide error checking but defer the
/// actual work to protected abstract methods.
/// </remarks>
//*****************************************************************************

public abstract class LayoutBase : LayoutsBase, ILayout
{
    //*************************************************************************
    //  Constructor: LayoutBase()
    //
    /// <summary>
    /// Initializes a new instance of the LayoutBase class.
    /// </summary>
    //*************************************************************************

    public LayoutBase()
    {
        m_iMargin = 6;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: Margin
    //
    /// <summary>
    /// Gets or sets the margin to subtract from each edge of the graph
    /// rectangle before laying out the graph.
    /// </summary>
    ///
    /// <value>
    /// The margin to subtract from each edge.  Must be greater than or equal
    /// to zero.  The default value is 6.
    /// </value>
    ///
    /// <remarks>
    /// If the graph rectangle passed to <see cref="LayOutGraph" /> is {L=0,
    /// T=0, R=50, B=30} and the <see cref="Margin" /> is 5, for example, then
    /// the graph is laid out within the rectangle {L=5, T=5, R=45, B=25}.
    /// </remarks>
    //*************************************************************************

    public Int32
    Margin
    {
        get
        {
            AssertValid();

            return (m_iMargin);
        }

        set
        {
            const String PropertyName = "Margin";

            this.ArgumentChecker.CheckPropertyNotNegative(PropertyName, value);

            if (value == m_iMargin)
            {
                return;
            }

            m_iMargin = value;

            FireLayoutRequired();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SupportsOutOfBoundsVertices
    //
    /// <summary>
    /// Gets a flag indicating whether vertices laid out by the class can fall
    /// outside the graph bounds.
    /// </summary>
    ///
    /// <value>
    /// true if the vertices call fall outside the graph bounds.
    /// </value>
    ///
    /// <remarks>
    /// If true, the <see cref="IVertex.Location" /> of the laid-out vertices
    /// may be within the graph rectangle's margin or outside the graph
    /// rectangle.  If false, the vertex locations are always within the
    /// margin.
    /// </remarks>
    //*************************************************************************

    public virtual Boolean
    SupportsOutOfBoundsVertices
    {
        get
        {
            return (false);
        }
    }

    //*************************************************************************
    //  Method: LayOutGraph()
    //
    /// <summary>
    /// Lays out a graph.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to lay out.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.
    /// </param>
    ///
    /// <remarks>
    /// This method lays out the graph <paramref name="graph" /> by setting the
    /// <see cref="IVertex.Location" /> property on all of the graph's
    /// vertices, and optionally adding geometry metadata to the graph,
    /// vertices, or edges.
    /// </remarks>
    //*************************************************************************

    public void
    LayOutGraph
    (
        IGraph graph,
        LayoutContext layoutContext
    )
    {
        AssertValid();

        const String MethodName = "LayOutGraph";

        this.ArgumentChecker.CheckArgumentNotNull(MethodName, "graph", graph);

        this.ArgumentChecker.CheckArgumentNotNull(MethodName, "layoutContext",
            layoutContext);

        LayoutContext oAdjustedLayoutContext;

        if ( !GetAdjustedLayoutContext(graph, layoutContext,
            out oAdjustedLayoutContext) )
        {
            return;
        }

        // Honor the optional LayOutTheseVerticesOnly key on the graph.

        ICollection<IVertex> oVerticesToLayOut = GetVerticesToLayOut(graph);

        if (oVerticesToLayOut.Count > 0)
        {
            LayOutGraphCore(graph, oVerticesToLayOut, oAdjustedLayoutContext);
            LayoutMetadataUtil.MarkGraphAsLaidOut(graph);
        }
    }

    //*************************************************************************
    //  Method: TransformLayout()
    //
    /// <summary>
    /// Transforms a graph's current layout.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph whose layout needs to be transformed.
    /// </param>
    ///
    /// <param name="originalLayoutContext">
    /// <see cref="LayoutContext" /> object that was passed to the most recent
    /// call to <see cref="LayOutGraph" />.
    /// </param>
    ///
    /// <param name="newLayoutContext">
    /// Provides access to the new graph rectangle.
    /// </param>
    ///
    /// <remarks>
    /// After a graph has been laid out by <see cref="LayOutGraph" />, this
    /// method can be used to transform the graph's layout from the original
    /// graph rectangle to another.  <paramref name="originalLayoutContext" />
    /// contains the original graph rectangle, and <paramref
    /// name="newLayoutContext" /> contains the new graph rectangle.  This
    /// method transforms all the graph's vertex locations from the original
    /// rectangle to the new one.  If <see cref="LayOutGraph" /> added geometry
    /// metadata to the graph, this method also transforms that metadata.
    /// </remarks>
    //*************************************************************************

    public void
    TransformLayout
    (
        IGraph graph,
        LayoutContext originalLayoutContext,
        LayoutContext newLayoutContext
    )
    {
        AssertValid();

        const String MethodName = "TransformLayout";

        this.ArgumentChecker.CheckArgumentNotNull(MethodName, "graph", graph);

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, "originalLayoutContext", originalLayoutContext);

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, "newLayoutContext", newLayoutContext);

        if (graph.Vertices.Count == 0)
        {
            return;
        }

        TransformLayoutCore(graph, originalLayoutContext, newLayoutContext);
    }

    //*************************************************************************
    //  Method: OnVertexMove()
    //
    /// <summary>
    /// Processes a vertex that was moved after the graph was laid out.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex that was moved.
    /// </param>
    ///
    /// <remarks>
    /// An application may allow the user to move a vertex after the graph has
    /// been laid out by <see cref="LayOutGraph" />.  This method is called
    /// after the application has changed the <see cref="IVertex.Location" />
    /// property on <paramref name="vertex" />.  If <see cref="LayOutGraph" />
    /// added geometry metadata to the graph, vertices, or edges, <see
    /// cref="OnVertexMove" /> should modify the metadata if necessary.
    /// </remarks>
    //*************************************************************************

    public void
    OnVertexMove
    (
        IVertex vertex
    )
    {
        AssertValid();

        const String MethodName = "OnVertexMove";

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, "vertex", vertex);

        OnVertexMoveCore(vertex);
    }

    //*************************************************************************
    //  Event: LayoutRequired
    //
    /// <summary>
    /// Occurs when a change occurs that requires the graph to be laid out
    /// again.
    /// </summary>
    ///
    /// <remarks>
    /// The event is fired when any change is made to the object that might
    /// affect the layout of the graph, such as a property change that affects
    /// the layout algorithm.  The owner should lay out the graph and redraw it
    /// in response to the event.
    /// </remarks>
    //*************************************************************************

    public event EventHandler LayoutRequired;


    //*************************************************************************
    //  Method: LayOutGraphCore()
    //
    /// <summary>
    /// Lays out a graph.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to lay out.  The graph is guaranteed to have at least one vertex.
    /// </param>
    ///
    /// <param name="verticesToLayOut">
    /// Vertices to lay out.  The collection is guaranteed to have at least one
    /// vertex.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.  The <see
    /// cref="LayoutContext.GraphRectangle" /> is guaranteed to have non-zero
    /// width and height.
    /// </param>
    ///
    /// <remarks>
    /// This method lays out the graph <paramref name="graph" /> by setting the
    /// <see cref="IVertex.Location" /> property on the vertices in <paramref
    /// name="verticesToLayOut" />, and optionally adding geometry metadata to
    /// the graph, vertices, or edges.
    ///
    /// <para>
    /// The arguments have already been checked for validity.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected abstract void
    LayOutGraphCore
    (
        IGraph graph,
        ICollection<IVertex> verticesToLayOut,
        LayoutContext layoutContext
    );

    //*************************************************************************
    //  Method: TransformLayoutCore()
    //
    /// <summary>
    /// Transforms a graph's current layout.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph whose layout needs to be transformed.
    /// </param>
    ///
    /// <param name="originalLayoutContext">
    /// <see cref="LayoutContext" /> object that was passed to the most recent
    /// call to <see cref="LayOutGraph" />.
    /// </param>
    ///
    /// <param name="newLayoutContext">
    /// Provides access to objects needed to transform the graph's layout.
    /// </param>
    ///
    /// <remarks>
    /// After a graph has been laid out by <see cref="LayOutGraph" />, this
    /// method may get called to transform the graph's layout from one rectangle
    /// to another.  <paramref name="originalLayoutContext" /> contains the
    /// original graph rectangle, and <paramref name="newLayoutContext" />
    /// contains the new graph rectangle.  This base-class implementation
    /// transforms all the graph's vertex locations from the original rectangle
    /// to the new one.  If the derived <see cref="LayOutGraphCore" />
    /// implementation added geometry metadata to the graph, the derived class
    /// should override this method, transform the geometry metadata, and call
    /// this base-class implementation to transform the graph's vertex
    /// locations.
    ///
    /// <para>
    /// The arguments have already been checked for validity.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected virtual void
    TransformLayoutCore
    (
        IGraph graph,
        LayoutContext originalLayoutContext,
        LayoutContext newLayoutContext
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(originalLayoutContext != null);
        Debug.Assert(newLayoutContext != null);
        AssertValid();

        Matrix oTransformationMatrix = LayoutUtil.GetRectangleTransformation(
            originalLayoutContext.GraphRectangle,
            newLayoutContext.GraphRectangle
            );

        LayoutUtil.TransformVertexLocations(graph, oTransformationMatrix);
    }

    //*************************************************************************
    //  Method: OnVertexMoveCore()
    //
    /// <summary>
    /// Processes a vertex that was moved after the graph was laid out.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex that was moved.
    /// </param>
    ///
    /// <remarks>
    /// An application may allow the user to move a vertex after the graph has
    /// been laid out by <see cref="LayOutGraphCore" />.  This method is called
    /// after the application has changed the <see cref="IVertex.Location" />
    /// property on <paramref name="vertex" />.  If <see
    /// cref="LayOutGraphCore" /> added geometry metadata to the graph,
    /// vertices, or edges, <see cref="OnVertexMoveCore" /> should modify the
    /// metadata if necessary.
    ///
    /// <para>
    /// This base-class implementation does nothing.
    /// </para>
    ///
    /// <para>
    /// The argument has already been checked for validity.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected virtual void
    OnVertexMoveCore
    (
        IVertex vertex
    )
    {
        AssertValid();

        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: GetAdjustedLayoutContext()
    //
    /// <summary>
    /// Gets an adjusted layout context object to use when laying out the
    /// graph.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph being laid out.
    /// </param>
    ///
    /// <param name="oOriginalLayoutContext">
    /// The original layout context passed to the layout method.
    /// </param>
    ///
    /// <param name="oAdjustedLayoutContext">
    /// If true is returned, this gets set to a copy of <paramref
    /// name="oOriginalLayoutContext" /> that has been adjusted.
    /// </param>
    ///
    /// <returns>
    /// true if the graph can be laid out, false if it can't be.
    /// </returns>
    ///
    /// <remarks>
    /// This method adjusts the graph rectangle stored in <paramref
    /// name="oOriginalLayoutContext" /> according to the <see
    /// cref="LayoutBase.Margin" /> setting and the presence of a <see
    /// cref="ReservedMetadataKeys.LayOutTheseVerticesWithinBounds" /> key on
    /// the graph.  If subtracting the margin results in a non-positive width
    /// or height, false is returned.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    GetAdjustedLayoutContext
    (
        IGraph oGraph,
        LayoutContext oOriginalLayoutContext,
        out LayoutContext oAdjustedLayoutContext
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oOriginalLayoutContext != null);
        AssertValid();

        oAdjustedLayoutContext = null;
        Rectangle oAdjustedRectangle = oOriginalLayoutContext.GraphRectangle;

        if (
            oGraph.ContainsKey(
                ReservedMetadataKeys.LayOutTheseVerticesWithinBounds)
            &&
            oGraph.ContainsKey(
                ReservedMetadataKeys.LayOutTheseVerticesOnly)
            )
        {
            // Get the bounding rectangle of the specified vertices.

            Single fMinimumX = Single.MaxValue;
            Single fMaximumX = Single.MinValue;
            Single fMinimumY = Single.MaxValue;
            Single fMaximumY = Single.MinValue;
            
            foreach ( IVertex oVertex in GetVerticesToLayOut(oGraph) )
            {
                PointF oLocation = oVertex.Location;
                Single fX = oLocation.X;
                Single fY = oLocation.Y;

                fMinimumX = Math.Min(fX, fMinimumX);
                fMaximumX = Math.Max(fX, fMaximumX);
                fMinimumY = Math.Min(fY, fMinimumY);
                fMaximumY = Math.Max(fY, fMaximumY);
            }

            if (fMinimumX != Single.MaxValue)
            {
                oAdjustedRectangle = Rectangle.FromLTRB(
                    (Int32)Math.Ceiling(fMinimumX),
                    (Int32)Math.Ceiling(fMinimumY),
                    (Int32)Math.Floor(fMaximumX),
                    (Int32)Math.Floor(fMaximumY) );
            }
        }
        else
        {
            oAdjustedRectangle.Inflate(-m_iMargin, -m_iMargin);
        }

        if (oAdjustedRectangle.Width > 0 && oAdjustedRectangle.Height > 0)
        {
            oAdjustedLayoutContext = new LayoutContext(oAdjustedRectangle);
            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: GetRectangleCenterAndHalfSize()
    //
    /// <summary>
    /// Gets the center of a rectangle and the minimum of half its width and
    /// half its height.
    /// </summary>
    ///
    /// <param name="rectangle">
    /// The rectangle to use.
    /// </param>
    ///
    /// <param name="centerX">
    /// The x-coordinate of the center of <paramref name="rectangle" />.
    /// </param>
    ///
    /// <param name="centerY">
    /// The y-coordinate of the center of <paramref name="rectangle" />.
    /// </param>
    ///
    /// <param name="halfSize">
    /// If the width of <paramref name="rectangle" /> is less than its height,
    /// half the width gets stored here.  Otherwise, half the height gets
    /// stored here.
    /// </param>
    ///
    /// <remarks>
    /// This method can be used by layouts that are centered and symetrical.
    /// </remarks>
    //*************************************************************************

    protected void
    GetRectangleCenterAndHalfSize
    (
        Rectangle rectangle,
        out Double centerX,
        out Double centerY,
        out Double halfSize
    )
    {
        AssertValid();

        Double fHalfWidth = (Double)rectangle.Width / 2.0;
        Double fHalfHeight = (Double)rectangle.Height / 2.0;

        centerX = rectangle.Left + fHalfWidth;
        centerY = rectangle.Top + fHalfHeight;

        halfSize = Math.Min(fHalfWidth, fHalfHeight);
    }

    //*************************************************************************
    //  Method: GetVerticesToLayOut()
    //
    /// <summary>
    /// Gets the vertices to lay out.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph that is being laid out.
    /// </param>
    ///
    /// <returns>
    /// The vertices to lay out.
    /// </returns>
    //*************************************************************************

    protected ICollection<IVertex>
    GetVerticesToLayOut
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);
        AssertValid();

        // If the LayOutTheseVerticesOnly key is present on the graph, its
        // value is an IVertex array of vertices to lay out.

        Object oVerticesToLayOut;

        if ( graph.TryGetValue(ReservedMetadataKeys.LayOutTheseVerticesOnly,
            typeof( IVertex [] ), out oVerticesToLayOut) )
        {
            return ( (IVertex [] )oVerticesToLayOut );
        }

        // The key isn't present.  Use the graph's entire vertex collection.

        return (graph.Vertices);
    }

    //*************************************************************************
    //  Method: GetEdgesToLayOut()
    //
    /// <summary>
    /// Gets the edges to lay out.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph that is being laid out.
    /// </param>
    ///
    /// <param name="verticesToLayOut">
    /// The vertices being laid out.
    /// </param>
    ///
    /// <returns>
    /// The edges to lay out.
    /// </returns>
    ///
    /// <remarks>
    /// If the derived class needs a list of the edges that connect only those
    /// vertices being laid out, it should use this method to get the list.
    /// </remarks>
    //*************************************************************************

    protected ICollection<IEdge>
    GetEdgesToLayOut
    (
        IGraph graph,
        ICollection<IVertex> verticesToLayOut
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(verticesToLayOut != null);
        AssertValid();

        if (verticesToLayOut.Count == graph.Vertices.Count)
        {
            // The entire graph is being laid out.

            return (graph.Edges);
        }

        // Create a HashSet of the vertices to lay out.  The key is the vertex
        // ID.

        HashSet<Int32> oVertexIDHashSet = new HashSet<Int32>();

        foreach (IVertex oVertex in verticesToLayOut)
        {
            oVertexIDHashSet.Add(oVertex.ID);
        }

        // Create a dictionary of the edges to lay out.  The key is the edge ID
        // and the value is the edge.  Only those edges that connect two
        // vertices being laid out should be included.  A dictionary is used to
        // prevent the same edge from being included twice.

        Dictionary<Int32, IEdge> oEdgeIDDictionary =
            new Dictionary<Int32, IEdge>();

        foreach (IVertex oVertex in verticesToLayOut)
        {
            foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
            {
                IVertex oAdjacentVertex =
                    oIncidentEdge.GetAdjacentVertex(oVertex);

                if ( oVertexIDHashSet.Contains(oAdjacentVertex.ID) )
                {
                    oEdgeIDDictionary[oIncidentEdge.ID] = oIncidentEdge;
                }
            }
        }

        return (oEdgeIDDictionary.Values);
    }

    //*************************************************************************
    //  Method: RandomizeVertexLocations()
    //
    /// <overloads>
    /// Randomly distributes the vertex locations in a graph.
    /// </overloads>
    ///
    /// <summary>
    /// Randomly distributes the vertex locations in a graph using a
    /// time-dependent default seed value.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph whose vertices need to be randomized.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.  The <see
    /// cref="LayoutContext.GraphRectangle" /> must have non-zero width and
    /// height.
    /// </param>
    ///
    /// <remarks>
    /// If a vertex has a metadata key of <see
    /// cref="ReservedMetadataKeys.LockVertexLocation" /> with the value true,
    /// its location is left unmodified.
    /// </remarks>
    //*************************************************************************

    protected void
    RandomizeVertexLocations
    (
        IGraph graph,
        LayoutContext layoutContext
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(layoutContext != null);
        AssertValid();

        RandomizeVertexLocations(graph.Vertices, layoutContext, new Random(),
            false);
    }

    //*************************************************************************
    //  Method: RandomizeVertexLocations()
    //
    /// <summary>
    /// Randomly distributes the vertex locations in a graph using a specified
    /// seed value.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph whose vertices need to be randomized.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.  The <see
    /// cref="LayoutContext.GraphRectangle" /> must have non-zero width and
    /// height.
    /// </param>
    ///
    /// <param name="seed">
    /// A number used to calculate a starting value for the pseudo-random
    /// number sequence. If a negative number is specified, the absolute value
    /// of the number is used. 
    /// </param>
    ///
    /// <remarks>
    /// If a vertex has a metadata key of <see
    /// cref="ReservedMetadataKeys.LockVertexLocation" /> with the value true,
    /// its location is left unmodified.
    /// </remarks>
    //*************************************************************************

    protected void
    RandomizeVertexLocations
    (
        IGraph graph,
        LayoutContext layoutContext,
        Int32 seed
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(layoutContext != null);
        AssertValid();

        RandomizeVertexLocations(graph.Vertices, layoutContext,
            new Random(seed), false);
    }

    //*************************************************************************
    //  Method: RandomizeVertexLocations()
    //
    /// <summary>
    /// Randomly distributes the vertex locations in a graph using a specified
    /// random number generator.
    /// </summary>
    ///
    /// <param name="vertices">
    /// Vertices that need to be randomized.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.  The <see
    /// cref="LayoutContext.GraphRectangle" /> must have non-zero width and
    /// height.
    /// </param>
    ///
    /// <param name="random">
    /// Random number generator.
    /// </param>
    ///
    /// <param name="specifiedVerticesOnly">
    /// If true, only those vertices with a location of <see
    /// cref="RandomizeThisLocation" /> are randomly distributed.
    /// </param>
    ///
    /// <remarks>
    /// If a vertex has a metadata key of <see
    /// cref="ReservedMetadataKeys.LockVertexLocation" /> with the value true,
    /// its location is left unmodified.
    /// </remarks>
    //*************************************************************************

    protected void
    RandomizeVertexLocations
    (
        ICollection<IVertex> vertices,
        LayoutContext layoutContext,
        Random random,
        Boolean specifiedVerticesOnly
    )
    {
        Debug.Assert(vertices != null);
        Debug.Assert(layoutContext != null);
        Debug.Assert(random != null);
        AssertValid();

        Rectangle oRectangle = layoutContext.GraphRectangle;

        Debug.Assert(oRectangle.Width > 0);
        Debug.Assert(oRectangle.Height > 0);

        Int32 iLeft = oRectangle.Left;
        Int32 iRight = oRectangle.Right;
        Int32 iTop = oRectangle.Top;
        Int32 iBottom = oRectangle.Bottom;

        foreach (IVertex oVertex in vertices)
        {
            if (
                (specifiedVerticesOnly &&
                    oVertex.Location != RandomizeThisLocation)
                ||
                VertexIsLocked(oVertex)
                )
            {
                continue;
            }

            Int32 iX = random.Next(iLeft, iRight + 1);
            Int32 iY = random.Next(iTop, iBottom + 1);

            oVertex.Location = new PointF(iX, iY);
        }
    }

    //*************************************************************************
    //  Method: VertexIsLocked()
    //
    /// <summary>
    /// Returns a flag indicating whether the vertex is locked.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to check.
    /// </param>
    ///
    /// <returns>
    /// true if the vertex is locked.
    /// </returns>
    ///
    /// <remarks>
    /// A locked vertex's location should not be modified by the layout,
    /// although the vertex may be included in layout calculations.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    VertexIsLocked
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        Object oLockVertexLocation;

        return (oVertex.TryGetValue(ReservedMetadataKeys.LockVertexLocation,
            typeof(Boolean), out oLockVertexLocation) &&
            (Boolean)oLockVertexLocation);
    }

    //*************************************************************************
    //  Method: FireLayoutRequired()
    //
    /// <summary>
    /// Fires the <see cref="LayoutRequired" /> event if appropriate.
    /// </summary>
    //*************************************************************************

    protected void
    FireLayoutRequired()
    {
        AssertValid();

        EventUtil.FireEvent(this, this.LayoutRequired);
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

        Debug.Assert(m_iMargin >= 0);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// RandomizeVertexLocations() can be instructed to randomly distribute
    /// only those vertices with an IVertex.Location set to this special value.

    public static readonly PointF RandomizeThisLocation =
        new PointF(Single.MinValue, Single.MinValue);


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Margin to subtract from the graph rectangle before laying out the
    /// graph.

    protected Int32 m_iMargin;
}

}
