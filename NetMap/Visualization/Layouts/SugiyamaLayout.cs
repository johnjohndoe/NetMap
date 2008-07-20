
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//  Class: SugiyamaLayout
//
/// <summary>
///	Lays out a graph using the Sugiyama layered layout scheme.
/// </summary>
///
/// <remarks>
/// This layout uses an algorithm based on Sugiyama's layered layout scheme.
/// It is implemented with the Microsoft Research GLEE (Graph Layout Execution
/// Engine) library.  GLEE was developed by Lev Nachmanson of Microsoft
/// Research and was available on the Microsoft Research download site as of
/// January 2007.
///
/// <para>
/// The Microsoft.GLEE.dll assembly must be included in the build that uses
/// this class.
/// </para>
///
/// <para>
/// <see cref="SugiyamaVertexDrawer" /> and <see cref="SugiyamaEdgeDrawer" />
/// should be used to draw the graph after it is laid out.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class SugiyamaLayout : AsyncLayoutBase
{
    //*************************************************************************
    //  Constructor: SugiyamaLayout()
    //
    /// <summary>
    /// Initializes a new instance of the SugiyamaLayout class.
    /// </summary>
    //*************************************************************************

    public SugiyamaLayout()
    {
        // (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Method: LayOutGraphCore()
    //
    /// <summary>
    /// Lays out a graph synchronously or asynchronously.
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
    /// <param name="backgroundWorker">
    /// <see cref="BackgroundWorker" /> whose worker thread called this method
	/// if the graph is being laid out asynchronously, or null if the graph is
	/// being laid out synchronously.
    /// </param>
    ///
    /// <returns>
	/// true if the layout was successfully completed, false if the layout was
	/// cancelled.  The layout can be cancelled only if the graph is being laid
	/// out asynchronously.
    /// </returns>
	///
    /// <remarks>
	///	This method lays out the graph <paramref name="graph" /> either
	/// synchronously (if <paramref name="backgroundWorker" /> is null) or
	/// asynchronously (if (<paramref name="backgroundWorker" /> is not null)
	/// by setting the the <see cref="IVertex.Location" /> property on all of
	/// the graph's vertices and optionally adding geometry metadata to the
	/// graph, vertices, or edges.
	///
	/// <para>
	/// In the asynchronous case, the <see
	/// cref="BackgroundWorker.CancellationPending" /> property on the
	/// <paramref name="backgroundWorker" /> object should be checked before
	/// each layout iteration.  If it's true, the method should immediately
	/// return false.  Also, <see
	/// cref="AsyncLayoutBase.FireLayOutGraphIterationCompleted()" /> should be
	/// called after each iteration.
	/// </para>
	///
	/// <para>
	/// The arguments have already been checked for validity.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    protected override Boolean
    LayOutGraphCore
    (
		IGraph graph,
		LayoutContext layoutContext,
		BackgroundWorker backgroundWorker
	)
	{
		Debug.Assert(graph != null);
		Debug.Assert(layoutContext != null);
		AssertValid();

		// This class does not incorporate GLEE source code to implement the
		// layout.  Instead, it transfers the NetMap graph to a GLEE graph,
		// tells GLEE to lay out the GLEE graph, then reads the resulting
		// layout geometry from the GLEE graph and stores it as metadata in the
		// NetMap graph.  Although this involves several copy operations, it
		// uses only the GLEE public interfaces and bypasses all the
		// maintenance headaches that would arise if the GLEE source code were
		// used.

		// Create a GLEE graph.

		Microsoft.Glee.GleeGraph oGleeGraph = new Microsoft.Glee.GleeGraph();

		// Get the vertex radius specified by the SugiyamaVertexDrawer.

		Single fNetMapVertexRadius = GetNetMapVertexRadius(layoutContext);

		// Loop through the NetMap vertices.

		foreach (IVertex oVertex in graph.Vertices)
		{
			// Create a circle that defines the GLEE node's boundary.  GLEE's
			// layout code does not modify the node's boundary, it just shifts
			// the node's center.

            Microsoft.Glee.Splines.ICurve oCurve =
				Microsoft.Glee.Splines.CurveFactory.CreateEllipse(
					fNetMapVertexRadius, fNetMapVertexRadius,
					new Microsoft.Glee.Splines.Point(0, 0)
					);

			// Create a GLEE node that corresponds to the NetMap node.

            Microsoft.Glee.Node oGleeNode =
				new Microsoft.Glee.Node(oVertex.ID.ToString(), oCurve);

			oGleeGraph.AddNode(oGleeNode);

			// Store the GLEE node as temporary metadata in the NetMap node.

            oVertex.SetValue(ReservedMetadataKeys.SugiyamaGleeNode, oGleeNode);
		}

		// Loop through the NetMap edges.

        foreach (IEdge oEdge in graph.Edges)
        {
			// Retrieve the NetMap edge's vertices.

			IVertex [] aoVertices = oEdge.Vertices;

			// Retrieve the corresponding GLEE node for the NetMap edge's
			// vertices.

            Microsoft.Glee.Node oGleeNode0 =
				NetMapVertexToGleeNode( aoVertices[0] );

            Microsoft.Glee.Node oGleeNode1 =
				NetMapVertexToGleeNode( aoVertices[1] );

			// Create a GLEE edge using the two GLEE nodes.

			Microsoft.Glee.Edge oGleeEdge =
				new Microsoft.Glee.Edge(oGleeNode0, oGleeNode1);

            oGleeGraph.AddEdge(oGleeEdge);

			// Store the GLEE edge as temporary metadata in the NetMap edge.

            oEdge.SetValue(
				ReservedMetadataKeys.SugiyamaGleeEdge, oGleeEdge);
        }

		// Tell GLEE to lay out the GLEE graph.  This shifts the node centers,
		// connects the nodes with lines, and computes the smallest rectangle
		// that contains all the nodes and edges.

		oGleeGraph.CalculateLayout();

		// The rectangle computed by GLEE does not have the dimensions
		// specified by layoutContext.GraphRectangle.  Get a transformation
		// that will map coordinates in the GLEE rectangle to coordinates in
		// the specified NetMap rectangle.

		Matrix oTransformationMatrix =
			GetTransformationMatrix(oGleeGraph, layoutContext.GraphRectangle);

		// Because of the transformation, the radius of the vertices is no
		// longer the original fNetMapVertexRadius.  Compute the transformed
		// radius.

		PointF [] aoRadiusPoints = new PointF [] {
			PointF.Empty,
			new PointF(0, fNetMapVertexRadius)
			};

		oTransformationMatrix.TransformPoints(aoRadiusPoints);

		Double dX = aoRadiusPoints[1].X - aoRadiusPoints[0].X;
		Double dY = aoRadiusPoints[1].Y - aoRadiusPoints[0].Y;

		Single fTransformedNetMapVertexRadius =
			(Single)Math.Sqrt(dX * dX + dY * dY);

		// Store the computed radius as metadata on the graph, to be retrieved
		// by SugiyamaVertexDrawer.DrawVertex().

		graph.SetValue(ReservedMetadataKeys.SugiyamaComputedRadius,
			fTransformedNetMapVertexRadius);

		// Loop through the NetMap vertices again.

        foreach (IVertex oVertex in graph.Vertices)
        {
			// Retrieve the corresponding GLEE node.

            Microsoft.Glee.Node oGleeNode =
				NetMapVertexToGleeNode(oVertex);

            oVertex.RemoveKey(ReservedMetadataKeys.SugiyamaGleeNode);

			// Get the shifted node center and transform it to NetMap
			// coordinates.

            oVertex.Location = GleePointToTransformedPointF(
				oGleeNode.Center, oTransformationMatrix);
        }

		// Loop through the NetMap edges again.

        foreach (IEdge oEdge in graph.Edges)
        {
			// Retrieve the corresponding GLEE edge.

            Microsoft.Glee.Edge oGleeEdge = NetMapEdgeToGleeEdge(oEdge);

            oEdge.RemoveKey(ReservedMetadataKeys.SugiyamaGleeEdge);

			// Get the GLEE curve that describes most (but not all) of the
			// edge.

			Microsoft.Glee.Splines.Curve oCurve =
				(Microsoft.Glee.Splines.Curve)oGleeEdge.Curve;

			// TODO: In Microsoft.Glee.GraphViewerGdi.GViewer.
			// TransferGeometryFromGleeGraphToGraph() in the GLEE source code,
			// oCurve can apparently be null.  Can it be null here?  For now,
			// assume that the answer is no.

			Debug.Assert(oCurve != null);

			// Convert the curve to an array of PointF objects in NetMap
			// coordinates.

			PointF [] aoCurvePoints = GleeCurveToTransformedPointFArray(
				oCurve, oTransformationMatrix);

			// Store the array as metadata in the edge, to be retrieved by
			// SugiyamaEdgeDrawer.DrawEdge().

			oEdge.SetValue(
				ReservedMetadataKeys.SugiyamaCurvePoints, aoCurvePoints);

			// Get the endpoint of the curve and transform it to NetMap
			// coordinates.

			PointF oEndpoint = GleePointToTransformedPointF(
				oGleeEdge.ArrowHeadAtTargetPosition, oTransformationMatrix);

			// Store the endpoint as metadata in the edge, to be retrieved by
			// SugiyamaEdgeDrawer.DrawEdge().

			oEdge.SetValue(ReservedMetadataKeys.SugiyamaEndpoint, oEndpoint);
        }

		return (true);
	}

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
	/// call to <see cref="ILayout.LayOutGraph" />.
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
	/// After a graph has been laid out by <see cref="ILayout.LayOutGraph" />,
	/// this method may get called to transform the graph's layout from one
	/// rectangle to another.  <paramref name="originalLayoutContext" />
	/// contains the original graph rectangle, and <paramref
	/// name="newLayoutContext" /> contains the new graph rectangle.  The
	/// base-class implementation transforms all the graph's vertex locations
	/// from the original rectangle to the new one.  If the derived <see
	/// cref="LayOutGraphCore" /> implementation added geometry metadata to the
	/// graph, the derived class should override this method, transform the
	/// geometry metadata, and call the base-class implementation to transform
	/// the graph's vertex locations.
	///
	/// <para>
	/// The arguments have already been checked for validity.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    protected override void
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

		// Transform the graph's vertex locations.

		base.TransformLayoutCore(graph, originalLayoutContext,
			newLayoutContext, transformationMatrix
			);

		// Tranform the geometry metadata added by LayOutGraphCore().

		Object oValue;

		if ( graph.TryGetValue(
			ReservedMetadataKeys.SugiyamaComputedRadius, typeof(Single),
			out oValue) )
		{
			// Transforming the radius in the x-direction only isn't ideal, but
			// doing the transform properly would involve drawing the vertex as
			// an ellipse.

			PointF oTransformedRadius = LayoutUtil.TransformPointF(
				new PointF( (Single)oValue, 0 ), transformationMatrix
				);

			graph.SetValue(
				ReservedMetadataKeys.SugiyamaComputedRadius,
				oTransformedRadius.X
				);
		}

		foreach (IEdge oEdge in graph.Edges)
		{
			if ( !oEdge.TryGetValue(
				ReservedMetadataKeys.SugiyamaCurvePoints, typeof( PointF [] ),
					out oValue
				) )
			{
				continue;
			}

			PointF [] aoCurvePoints = ( PointF [] )oValue;

            transformationMatrix.TransformPoints(aoCurvePoints);

            oEdge.SetValue(ReservedMetadataKeys.SugiyamaCurvePoints,
				aoCurvePoints);

			PointF oEndpoint = (PointF)oEdge.GetRequiredValue(
				ReservedMetadataKeys.SugiyamaEndpoint, typeof(PointF)
				);

			oEdge.SetValue(
                ReservedMetadataKeys.SugiyamaEndpoint,
				LayoutUtil.TransformPointF(oEndpoint, transformationMatrix)
                );
		}
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
	/// The argument has already been checked for validity.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    protected override void
    OnVertexMoveCore
    (
		IVertex vertex
    )
	{
		Debug.Assert(vertex != null);
		AssertValid();

		// Remove the curve metadata that LayOutGraphCore() stored in the
		// vertex's incident edges.  When SugiyamaEdgeDrawer.DrawEdge() notices
		// that the metadata is missing, it will draw a straight line instead
		// of a curve.

		foreach (IEdge oEdge in vertex.IncidentEdges)
		{
			oEdge.RemoveKey(ReservedMetadataKeys.SugiyamaCurvePoints);
			oEdge.RemoveKey(ReservedMetadataKeys.SugiyamaEndpoint);
		}
	}

    //*************************************************************************
    //  Method: GetNetMapVertexRadius()
    //
    /// <summary>
    /// Gets the vertex radius specified by the <see
	/// cref="SugiyamaVertexDrawer" /> in use.
    /// </summary>
    ///
    /// <param name="oLayoutContext">
    /// Provides access to objects needed to lay out the graph.
    /// </param>
    ///
    /// <returns> 
	/// The vertex radius.
    /// </returns>
    //*************************************************************************

    protected Single
    GetNetMapVertexRadius
    (
		LayoutContext oLayoutContext
    )
	{
		Debug.Assert(oLayoutContext != null);
		AssertValid();

		// If the vertex drawer being used is the one that's meant to be used
		// with this layout, get its radius.

		IVertexDrawer oVertexDrawer = oLayoutContext.GraphDrawer.VertexDrawer;

		if (oVertexDrawer is SugiyamaVertexDrawer)
		{
			return ( ( (SugiyamaVertexDrawer)oVertexDrawer ).Radius );
		}

		// The wrong vertex drawer is being used.  Behave gracefully.

		return (DefaultNetMapVertexRadius);
	}

    //*************************************************************************
    //  Method: NetMapVertexToGleeNode()
    //
    /// <summary>
    /// Retrieves a GLEE node that has been stored as metadata in a NetMap
	/// vertex. 
    /// </summary>
    ///
    /// <param name="oVertex">
    /// NetMap vertex.
    /// </param>
    ///
    /// <returns>
	/// The GLEE node corresponding to <paramref name="oVertex" />.
    /// </returns>
	///
    /// <remarks>
	/// This method assumes that the GLEE node has been stored in the vertex's
	/// metadata using the key ReservedMetadataKeys.SugiyamaGleeNode.
    /// </remarks>
    //*************************************************************************

    protected Microsoft.Glee.Node
    NetMapVertexToGleeNode
    (
		IVertex oVertex
    )
	{
		Debug.Assert(oVertex != null);
		AssertValid();

		Object oValue = oVertex.GetRequiredValue(
			ReservedMetadataKeys.SugiyamaGleeNode,
			typeof(Microsoft.Glee.Node)
			);

		return ( (Microsoft.Glee.Node)oValue );
	}

    //*************************************************************************
    //  Method: NetMapEdgeToGleeEdge()
    //
    /// <summary>
    /// Retrieves a GLEE edge that has been stored as metadata in a NetMap
	/// edge. 
    /// </summary>
    ///
    /// <param name="oEdge">
    /// NetMap edge.
    /// </param>
    ///
    /// <returns>
	/// The GLEE edge corresponding to <paramref name="oEdge" />.
    /// </returns>
	///
    /// <remarks>
	/// This method assumes that the GLEE edge has been stored in the edge's
	/// metadata using the key ReservedMetadataKeys.SugiyamaGleeEdge.
    /// </remarks>
    //*************************************************************************

    protected Microsoft.Glee.Edge
    NetMapEdgeToGleeEdge
    (
		IEdge oEdge
    )
	{
		Debug.Assert(oEdge != null);
		AssertValid();

		Object oValue = oEdge.GetRequiredValue(
			ReservedMetadataKeys.SugiyamaGleeEdge,
			typeof(Microsoft.Glee.Edge)
			);

		return ( (Microsoft.Glee.Edge)oValue );
	}

    //*************************************************************************
    //  Method: GetTransformationMatrix()
    //
    /// <summary>
    /// Returns a <see cref="Matrix" /> that will transform coordinates in the
	/// graph rectangle computed by the GLEE layout code to coordinates in the
	/// NetMap graph rectangle.
    /// </summary>
    ///
    /// <param name="oGleeGraph">
    /// GLEE graph that has been laid out.
    /// </param>
    ///
    /// <param name="oNetMapGraphRectangle">
    /// Rectangle in which the NetMap graph is being laid out.
    /// </param>
    ///
    /// <returns>
	/// A <see cref="Matrix" /> to transform coordinates.
    /// </returns>
    //*************************************************************************

    protected Matrix
    GetTransformationMatrix
    (
		Microsoft.Glee.GleeGraph oGleeGraph,
		Rectangle oNetMapGraphRectangle
    )
	{
		Debug.Assert(oGleeGraph != null);
		AssertValid();

		// This code, which was adapted from Microsoft.Glee.GraphViewerGdi
		// .Render(), performs a translation as well as a scaling.

		Double dNetMapWidth = oNetMapGraphRectangle.Width;
		Double dNetMapHeight = oNetMapGraphRectangle.Height;

		Double dGleeWidth = oGleeGraph.Width;
		Double dGleeHeight = oGleeGraph.Height;

		Double sx = Math.Min(
			dNetMapWidth / dGleeWidth,
			dNetMapHeight / dGleeHeight
			);

		Double dx =
			( oNetMapGraphRectangle.Left + (dNetMapWidth / 2.0) ) -
			( sx * ( oGleeGraph.Left + (dGleeWidth / 2.0) ) )
			;

		Double dy =
			( oNetMapGraphRectangle.Top + (dNetMapHeight / 2.0) ) +
			( sx * ( oGleeGraph.Bottom + (dGleeHeight / 2.0) ) )
			;

		Matrix oTransformationMatrix = new Matrix(
			(float) sx,
			0f,
			0f,
			(float) -sx,
			(float) dx,
			(float) dy
			);

		return (oTransformationMatrix);
	}

    //*************************************************************************
    //  Method: GleePointToTransformedPointF()
    //
    /// <summary>
	/// Converts a Microsoft.Glee.Splines.Point in GLEE coordinates to a PointF
	/// in NetMap coordinates.
    /// </summary>
    ///
    /// <param name="oGleePoint">
	/// Microsoft.Glee.Splines.Point to convert.
    /// </param>
    ///
    /// <param name="oTransformationMatrix">
	/// Matrix created by <see cref="GetTransformationMatrix" />.
    /// </param>
	///
    /// <returns>
    /// <paramref name="oGleePoint" /> in GLEE coordinates converted to a
	/// PointF in NetMap coordinates.
    /// </returns>
    //*************************************************************************

	protected PointF
	GleePointToTransformedPointF
	(
		Microsoft.Glee.Splines.Point oGleePoint,
		Matrix oTransformationMatrix
	)
	{
		Debug.Assert(oGleePoint != null);
		Debug.Assert(oTransformationMatrix != null);
		AssertValid();

		PointF oPointF =
			new PointF( (Single)oGleePoint.X,  (Single)oGleePoint.Y );

		return ( LayoutUtil.TransformPointF(oPointF, oTransformationMatrix) );
	}

    //*************************************************************************
    //  Method: GleePointToPointF()
    //
    /// <summary>
	/// Converts a Microsoft.Glee.Splines.Point to a PointF.
    /// </summary>
    ///
    /// <param name="oGleePoint">
	/// Microsoft.Glee.Splines.Point to convert.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="oGleePoint" /> converted to a PointF.
    /// </returns>
    //*************************************************************************

	protected PointF
	GleePointToPointF
	(
		Microsoft.Glee.Splines.Point oGleePoint
	)
	{
		Debug.Assert(oGleePoint != null);
		AssertValid();

		return ( new PointF( (Single)oGleePoint.X,  (Single)oGleePoint.Y) );
	}

    //*************************************************************************
    //  Method: GleeCurveToTransformedPointFArray()
    //
    /// <summary>
	/// Converts a Microsoft.Glee.Splines.Curve to an array of PointF objects
	/// in NetMap coordinates.
    /// </summary>
    ///
    /// <param name="oCurve">
	/// Microsoft.Glee.Splines.Curve to convert.
    /// </param>
    ///
    /// <param name="oTransformationMatrix">
	/// Matrix created by <see cref="GetTransformationMatrix" />.
    /// </param>
	///
    /// <returns>
	/// An array of PointF objects suitable for drawing with the following
	/// code, where aoCurvePoints is the returned array:
	///
	/// <code>
	/// GraphicsPath oGraphicsPath = new GraphicsPath();
	/// 
	/// oGraphicsPath.AddBeziers(aoCurvePoints);
	/// 
	/// oGraphics.DrawPath(oPen, oGraphicsPath);
	/// </code>
	///
    /// </returns>
    //*************************************************************************

    protected PointF []
    GleeCurveToTransformedPointFArray
    (
		Microsoft.Glee.Splines.Curve oCurve,
		Matrix oTransformationMatrix
    )
	{
		Debug.Assert(oCurve != null);
		Debug.Assert(oTransformationMatrix != null);
		AssertValid();

		// Load the curve points into a list.

		System.Collections.Generic.List<PointF> oPointFList =
			new System.Collections.Generic.List<PointF>();

		Microsoft.Glee.Splines.Point oGleePoint =
			(oCurve.Segs[0] as Microsoft.Glee.Splines.CubicBezierSeg).B(0);

		oPointFList.Add( GleePointToPointF(oGleePoint) );

		foreach (Microsoft.Glee.Splines.CubicBezierSeg oCubicBezierSeg in
			oCurve.Segs)
		{
			oPointFList.Add( GleePointToPointF(oCubicBezierSeg.B(1) ) );
			oPointFList.Add( GleePointToPointF(oCubicBezierSeg.B(2) ) );
			oPointFList.Add( GleePointToPointF(oCubicBezierSeg.B(3) ) );
		}

		// Convert the list to an array and transform it to NetMap coordinates.

		PointF [] aoCurvePoints = oPointFList.ToArray();

		oTransformationMatrix.TransformPoints(aoCurvePoints);

		return (aoCurvePoints);
	}

    //*************************************************************************
    //  Method: GleeCurveToPointFArray()
    //
    /// <summary>
	/// Converts a Microsoft.Glee.Splines.Curve to an array of PointF objects.
    /// </summary>
    ///
    /// <param name="oCurve">
	/// Microsoft.Glee.Splines.Curve to convert.
    /// </param>
    ///
    /// <returns>
	/// An array of PointF objects suitable for drawing with the following
	/// code, where aoCurvePoints is the returned array:
	///
	/// <code>
	/// oTransformationMatrix.TransformPoints(aoCurvePoints);
	///
	/// GraphicsPath oGraphicsPath = new GraphicsPath();
	/// 
	/// oGraphicsPath.AddBeziers(aoCurvePoints);
	/// 
	/// oGraphics.DrawPath(oPen, oGraphicsPath);
	/// </code>
	///
    /// </returns>
    //*************************************************************************

    protected PointF []
    GleeCurveToPointFArray
    (
		Microsoft.Glee.Splines.Curve oCurve
    )
	{
		Debug.Assert(oCurve != null);
		AssertValid();

		// This is based on code in Microsoft.Glee.GraphViewerGdi.GViewer.
		// TransferGeometryFromGleeGraphToGraph() in the GLEE source code,

		System.Collections.Generic.List<PointF> oPointFList =
			new System.Collections.Generic.List<PointF>();

		Microsoft.Glee.Splines.Point oGleePoint =
			(oCurve.Segs[0] as Microsoft.Glee.Splines.CubicBezierSeg).B(0);

		oPointFList.Add( GleePointToPointF(oGleePoint) );

		foreach (Microsoft.Glee.Splines.CubicBezierSeg oCubicBezierSeg in
			oCurve.Segs)
		{
			oPointFList.Add( GleePointToPointF(oCubicBezierSeg.B(1) ) );
			oPointFList.Add( GleePointToPointF(oCubicBezierSeg.B(2) ) );
			oPointFList.Add( GleePointToPointF(oCubicBezierSeg.B(3) ) );
		}

		return ( oPointFList.ToArray() );
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
    //  Protected constants
    //*************************************************************************

	/// Radius to use for the vertices if the vertex drawer in use isn't the
	/// expected SugiyamaVertexDrawer.

	protected const Single DefaultNetMapVertexRadius = 5.0F;



    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
