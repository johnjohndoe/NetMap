
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//  Class: LayoutBase
//
/// <summary>
///	Base class for layouts.
/// </summary>
///
/// <remarks>
/// This abstract class can serve as a base class for <see
/// cref="ILayout" /> implementations.  Its implementations of the <see
/// cref="ILayout" /> public methods provide error checking but defer the
/// actual work to protected abstract methods.
/// </remarks>
//*****************************************************************************

public abstract class LayoutBase : VisualizationBase, ILayout
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
	///	Gets or sets the margin to subtract from each edge of the graph
	/// rectangle before laying out the graph.
    /// </summary>
    ///
    /// <value>
	/// The margin to subtract from each edge.  Must be greater than or equal
	/// to zero.  The units are determined by the <see cref="Graphics" />
	/// object used to draw the graph.  The default value is 6.
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
	///	This method lays out the graph <paramref name="graph" /> by setting the
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

		if (graph.Vertices.Count == 0)
		{
			return;
		}

		// Subtract the margin from the graph rectangle.

		LayoutContext oLayoutContext2;

		if ( !SubtractMarginFromRectangle(layoutContext, out oLayoutContext2) )
		{
			return;
		}

		LayOutGraphCore(graph, oLayoutContext2);

		// Mark the graph as having been laid out.

		MarkGraphAsLaidOut(graph, layoutContext);
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

		Matrix oTransformationMatrix = LayoutUtil.GetRectangleTransformation(
			originalLayoutContext.GraphRectangle,
			newLayoutContext.GraphRectangle
			);

		TransformLayoutCore(graph, originalLayoutContext, newLayoutContext,
			oTransformationMatrix);
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
	//	Event: LayoutRequired
	//
	/// <summary>
	///	Occurs when a change occurs that requires the graph to be laid out
	/// again.
	/// </summary>
	///
	/// <remarks>
	///	The event is fired when any change is made to the object that might
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
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.  The <see
	/// cref="LayoutContext.GraphRectangle" /> is guaranteed to have non-zero
	/// width and height.
    /// </param>
    ///
    /// <remarks>
	///	This method lays out the graph <paramref name="graph" /> by setting the
	/// <see cref="IVertex.Location" /> property on all of the graph's
	/// vertices, and optionally adding geometry metadata to the graph,
	/// vertices, or edges.
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
    /// <param name="transformationMatrix">
    /// Matrix that can be used to transform points from the original graph
	/// rectangle to the new graph rectangle.
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
		LayoutContext newLayoutContext,
		Matrix transformationMatrix
    )
	{
		Debug.Assert(graph != null);
		Debug.Assert(originalLayoutContext != null);
		Debug.Assert(newLayoutContext != null);
		Debug.Assert(transformationMatrix != null);
		AssertValid();

		LayoutUtil.TransformVertexLocations(graph, transformationMatrix);
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
    //  Method: SubtractMarginFromRectangle()
    //
    /// <summary>
    /// Subtracts a margin from each edge of the graph rectangle before laying
	/// out the graph.
    /// </summary>
    ///
    /// <param name="oLayoutContext">
    /// Provides access to objects needed to lay out the graph.
    /// </param>
    ///
    /// <param name="oLayoutContext2">
	/// If true is returned, this gets set to a copy of <paramref
	/// name="oLayoutContext" /> with a modified <see
	/// cref="LayoutContext.GraphRectangle" />.
    /// </param>
    ///
    /// <returns>
	/// true if the modified rectangle has positive width and height, false if
	/// the modified rectangle can't be used.
    /// </returns>
	///
    /// <remarks>
	/// This method subtracts <see cref="LayoutBase.Margin" /> from each edge
	/// of the <see cref="LayoutContext.GraphRectangle" /> stored in <paramref
	/// name="oLayoutContext" />.  If the resulting rectangle has a positive
	/// width and height, a new <see cref="LayoutContext" /> is stored at
	/// <paramref name="oLayoutContext2" /> and true is returned.  false is
	/// returned otherwise.
    /// </remarks>
    //*************************************************************************

	protected Boolean
	SubtractMarginFromRectangle
	(
		LayoutContext oLayoutContext,
		out LayoutContext oLayoutContext2
	)
	{
		Debug.Assert(oLayoutContext != null);
		AssertValid();

		Rectangle oRectangle = oLayoutContext.GraphRectangle;

		oRectangle.Inflate(-m_iMargin, -m_iMargin);

		if (oRectangle.Width <= 0 || oRectangle.Height <= 0)
		{
            oLayoutContext2 = null;

			return (false);
		}

        oLayoutContext2 = new LayoutContext(oRectangle);

		return (true);
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
    ///
    /// <remarks>
	/// If the derived class wants to honor the optional <see
	/// cref="ReservedMetadataKeys.LayOutTheseVerticesOnly" /> key on the
	/// graph, it should use this method to get the collection of vertices to
	/// lay out.  All vertices that are not included in the returned
	/// collection should be completely ignored.
    /// </remarks>
    //*************************************************************************

    protected ICollection
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
    /// <returns>
    /// The edges to lay out.
    /// </returns>
    ///
    /// <remarks>
	/// If the derived class wants to honor the optional <see
	/// cref="ReservedMetadataKeys.LayOutTheseVerticesOnly" /> key on the
	/// graph and it needs a list of the edges that connect only the specified
	/// vertices, it should use this method to get the collection of edges to
	/// use.  All edges that are not included in the returned collection should
	/// be completely ignored.
	///
	/// <para>
	/// All derived classes that want to honor the optional key should use <see
	/// cref="GetVerticesToLayOut" />.  Only those derived classes that need an
	/// edge list to do layout calculations (such as Fruchterman-Reingold) need
	/// to use <see cref="GetEdgesToLayOut" /> as well.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    protected ICollection
    GetEdgesToLayOut
    (
		IGraph graph
    )
	{
		Debug.Assert(graph != null);
		AssertValid();

		// If the LayOutTheseVerticesOnly key is present on the graph, its
		// value is an IVertex array of vertices to lay out.

		Object oVerticesToLayOut;

		if ( !graph.TryGetValue(ReservedMetadataKeys.LayOutTheseVerticesOnly,
			typeof( IVertex [] ), out oVerticesToLayOut) )
		{
			return (graph.Edges);
		}

		// The key is present.  Create a dictionary of the vertices to lay out.
		// The key is the vertex ID and the value isn't used.

		Dictionary<Int32, Char> oVertexIDDictionary =
			new Dictionary<Int32, Char>();

		foreach (IVertex oVertex in ( IVertex [] )oVerticesToLayOut)
		{
			oVertexIDDictionary.Add(oVertex.ID, ' ');
		}

		// Create a dictionary of the edges to lay out.  The key is the edge ID
		// and the value is the edge.  Only those edges that connect two
		// vertices being laid out should be included.  A dictionary is used to
		// prevent the same edge from being included twice.

		Dictionary<Int32, IEdge> oEdgeIDDictionary =
			new Dictionary<Int32, IEdge>();

		foreach (IVertex oVertex in ( IVertex [] )oVerticesToLayOut)
		{
			foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
			{
				IVertex [] aoIncidentEdgeVertices = oIncidentEdge.Vertices;

				IVertex oAdjacentVertex =
					(aoIncidentEdgeVertices[0].ID == oVertex.ID) ?
					aoIncidentEdgeVertices[1] : aoIncidentEdgeVertices[0];

				if ( oVertexIDDictionary.ContainsKey(oAdjacentVertex.ID) )
				{
					oEdgeIDDictionary[oIncidentEdge.ID] = oIncidentEdge;
				}
			}
		}

        return (oEdgeIDDictionary.Values);
	}

    //*************************************************************************
    //  Method: MarkGraphAsLaidOut()
    //
    /// <summary>
    /// Marks a graph as having been laid out.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph that was laid out.
    /// </param>
	///
    /// <param name="layoutContext">
    /// Provides access to objects used to lay out the graph.
    /// </param>
    ///
    /// <remarks>
	/// This should be called after <paramref name="graph" /> has been
	/// successfully laid out.  It adds a metadata key to the graph.
    /// </remarks>
    //*************************************************************************

    protected void
    MarkGraphAsLaidOut
    (
		IGraph graph,
		LayoutContext layoutContext
    )
	{
		Debug.Assert(graph != null);
		Debug.Assert(layoutContext != null);
		AssertValid();

		// Mark the graph as having been laid out.

		graph.SetValue(ReservedMetadataKeys.LayoutBaseLayoutComplete,
			layoutContext.GraphRectangle);
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
		ICollection vertices,
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
	//	Method: FireLayoutRequired()
	//
	/// <summary>
	///	Fires the <see cref="LayoutRequired" /> event if appropriate.
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
