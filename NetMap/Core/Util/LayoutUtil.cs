
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Core
{
//*****************************************************************************
//  Class: LayoutUtil
//
/// <summary>
/// Utility methods for dealing with graph layouts.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class LayoutUtil
{
    //*************************************************************************
    //  Method: GetRectangleTransformation()
    //
    /// <summary>
    /// Returns a <see cref="Matrix" /> that will transform points from
	/// coordinates in one rectangle to cooordinates in another rectangle.
    /// </summary>
    ///
    /// <param name="rectangle1">
	/// Source rectangle.
    /// </param>
    ///
    /// <param name="rectangle2">
	/// Destination rectangle.
    /// </param>
    ///
    /// <returns>
	/// A <see cref="Matrix" /> to transform points from <paramref
	/// name="rectangle1" /> to <paramref name="rectangle2" />.
    /// </returns>
	///
	/// <remarks>
	/// The <see cref="Matrix" /> returned by this method can be used to
	/// transform vertex locations and other points from one rectangle to
	/// another.  The transformation can involve both a translation and a
	/// scaling.
	/// </remarks>
    //*************************************************************************

    public static Matrix
    GetRectangleTransformation
    (
		RectangleF rectangle1,
		RectangleF rectangle2
    )
	{
		Debug.Assert(rectangle1.Width >= 0);
		Debug.Assert(rectangle1.Height >= 0);
		Debug.Assert(rectangle2.Width >= 0);
		Debug.Assert(rectangle2.Height >= 0);

		// Handle collapsed rectangles, which can result, for example, when a
		// graph contains only one vertex.

		if (rectangle1.Width == 0)
		{
			rectangle1.Inflate(1, 0);
		}

		if (rectangle1.Height == 0)
		{
			rectangle1.Inflate(0, 1);
		}

		if (rectangle2.Width == 0)
		{
			rectangle2.Inflate(1, 0);
		}

		if (rectangle2.Height == 0)
		{
			rectangle2.Inflate(0, 1);
		}

		Matrix oMatrix = new Matrix(

			rectangle1,

			new PointF [] {
				rectangle2.Location,
				new PointF(rectangle2.Right, rectangle2.Top),
				new PointF(rectangle2.Left, rectangle2.Bottom)
				}
			);

		return (oMatrix);
	}

    //*************************************************************************
    //  Method: TransformVertexLocations()
    //
    /// <overloads>
	/// Transforms a graph's vertex locations from their original rectangle to
	/// a new rectangle.
    /// </overloads>
	///
    /// <summary>
	/// Transforms a graph's vertex locations from their original rectangle to
	/// a new rectangle.
    /// </summary>
    ///
    /// <param name="graph">
	/// Graph whose vertex locations should be transformed.
    /// </param>
    ///
    /// <param name="originalGraphRectangle">
	/// Graph rectangle in which <paramref name="graph" /> was laid out.
    /// </param>
    ///
    /// <param name="newGraphRectangle">
	/// Graph rectangle into which <paramref name="graph" />'s vertices should
	/// be transformed.
    /// </param>
    ///
	/// <remarks>
	/// After a graph is laid out, this method can be used to transform the
	/// vertex locations from the original graph rectangle to a new graph
	/// rectangle.  The transformation can involve both a translation and a
	/// scaling.
	/// </remarks>
    //*************************************************************************

    public static void
    TransformVertexLocations
    (
		IGraph graph,
		RectangleF originalGraphRectangle,
		RectangleF newGraphRectangle
    )
	{
		Debug.Assert(graph != null);
		Debug.Assert(originalGraphRectangle.Width >= 0);
		Debug.Assert(originalGraphRectangle.Height >= 0);
		Debug.Assert(newGraphRectangle.Width >= 0);
		Debug.Assert(newGraphRectangle.Height >= 0);

		Matrix oTransformationMatrix = GetRectangleTransformation(
			originalGraphRectangle, newGraphRectangle);

		TransformVertexLocations(graph, oTransformationMatrix);
	}

    //*************************************************************************
    //  Method: TransformVertexLocations()
    //
    /// <summary>
	/// Transforms a graph's vertex locations from their original rectangle to
	/// a new rectangle using a specified transformation matrix.
    /// </summary>
    ///
    /// <param name="graph">
	/// Graph whose vertex locations should be transformed.
    /// </param>
    ///
    /// <param name="transformationMatrix">
	/// Transformation matrix to use.
    /// </param>
    ///
	/// <remarks>
	/// After a graph is laid out, this method can be used to transform the
	/// vertex locations from the original graph rectangle to a new graph
	/// rectangle.
	/// </remarks>
    //*************************************************************************

    public static void
    TransformVertexLocations
    (
		IGraph graph,
		Matrix transformationMatrix
    )
	{
		Debug.Assert(graph != null);
		Debug.Assert(transformationMatrix != null);

		foreach (IVertex oVertex in graph.Vertices)
		{
			oVertex.Location = TransformPointF(
				oVertex.Location, transformationMatrix);
		}
	}

    //*************************************************************************
    //  Method: TransformPointF()
    //
    /// <summary>
	/// Transforms a PointF using a specified transformation matrix.
    /// </summary>
    ///
    /// <param name="pointF">
	/// PointF to transform.
    /// </param>
    ///
    /// <param name="transformationMatrix">
	/// Transformation matrix to use.
    /// </param>
    ///
	/// <returns>
	/// Transformed copy of <paramref name="pointF" />.
	/// </returns>
    //*************************************************************************

    public static PointF
    TransformPointF
    (
		PointF pointF,
		Matrix transformationMatrix
    )
	{
		Debug.Assert(transformationMatrix != null);

		PointF [] aoPoints = new PointF[] {pointF};

		transformationMatrix.TransformPoints(aoPoints);

		return ( aoPoints[0] );
	}

    //*************************************************************************
    //  Method: GetGraphBoundingRectangle()
    //
    /// <summary>
    /// Returns the smallest <see cref="RectangleF" /> that contains all of a
	/// graph's vertices.
    /// </summary>
    ///
    /// <param name="graph">
	/// Graph to get a bounding rectangle for.
    /// </param>
    ///
    /// <returns>
	/// A <see cref="RectangleF" /> that contains all of a graph's vertices.
    /// </returns>
    ///
    /// <remarks>
	/// If <paramref name="graph" /> contains zero vertices, RectangleF.Empty
	/// is returned.
    /// </remarks>
    //*************************************************************************

    public static RectangleF
    GetGraphBoundingRectangle
    (
		IGraph graph
    )
	{
		Debug.Assert(graph != null);

		IVertexCollection oVertices = graph.Vertices;

		if (oVertices.Count == 0)
		{
			return (RectangleF.Empty);
		}

		Single fLeft = Single.MaxValue;
		Single fRight = Single.MinValue;
		Single fTop = Single.MaxValue;
		Single fBottom = Single.MinValue;

		foreach (IVertex oVertex in oVertices)
		{
			PointF oLocation = oVertex.Location;
			Single fX = oLocation.X;
			Single fY = oLocation.Y;

			fLeft = Math.Min(fLeft, fX);
			fRight = Math.Max(fRight, fX);

			fTop = Math.Min(fTop, fY);
			fBottom = Math.Max(fBottom, fY);
		}

		return ( RectangleF.FromLTRB(fLeft, fTop, fRight, fBottom) );
	}
}

}
