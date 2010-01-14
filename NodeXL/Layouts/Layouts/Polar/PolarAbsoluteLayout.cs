
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Layouts
{
//*****************************************************************************
//  Class: PolarAbsoluteLayout
//
/// <summary>
/// Lays out a graph by placing the vertices within a polar coordinate space.
/// </summary>
///
/// <remarks>
/// This layout defines a polar coordinate space that uses (R, Angle) to
/// specify a point.
///
/// <para>
/// R represents the distance of the point from the origin, which is the center
/// of the graph rectangle.  0.0 represents the origin, 1.0 represents one WPF
/// unit (1/96 inch), and -1 represents one WPF unit in the opposite direction.
/// </para>
///
/// <para>
/// Angle is in degrees.  0.0 represents points on the positive x-axis and 90.0
/// represents points on the positive y-axis.  Any angle is valid.  361.0
/// degrees is the same as 1.0 degree, for example, and -1.0 degree is the same
/// as 359.0 degrees.
/// </para>
///
/// <para>
/// To specify the polar coordinates of a vertex, add the <see
/// cref="ReservedMetadataKeys.PolarLayoutCoordinates" /> key to the vertex.
/// If a vertex is missing this key, the vertex is placed at the origin.
/// </para>
///
/// <para>
/// If the graph has a metadata key of <see
/// cref="ReservedMetadataKeys.LayOutTheseVerticesOnly" />, only the vertices
/// specified in the value's IVertex array are laid out and all other vertices
/// are completely ignored.
/// </para>
///
/// <para>
/// If a vertex has a metadata key of <see
/// cref="ReservedMetadataKeys.LockVertexLocation" /> with a value of true, its
/// location is left unmodified.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class PolarAbsoluteLayout : PolarLayoutBase
{
    //*************************************************************************
    //  Constructor: PolarAbsoluteLayout()
    //
    /// <summary>
    /// Initializes a new instance of the PolarAbsoluteLayout class.
    /// </summary>
    //*************************************************************************

    public PolarAbsoluteLayout()
    :
    base(true)
    {
        // (Do nothing else.)

        AssertValid();
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

    public override Boolean
    SupportsOutOfBoundsVertices
    {
        get
        {
            AssertValid();

            // Because there is no upper bound on R, the vertices can be
            // anywhere.

            return (true);
        }
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
    /// call to <see cref="LayoutBase.LayOutGraph" />.
    /// </param>
    ///
    /// <param name="newLayoutContext">
    /// Provides access to objects needed to transform the graph's layout.
    /// </param>
    ///
    /// <remarks>
    /// After a graph has been laid out by <see
    /// cref="LayoutBase.LayOutGraph" />, this method may get called to
    /// transform the graph's layout from one rectangle to another.  <paramref
    /// name="originalLayoutContext" /> contains the original graph rectangle,
    /// and <paramref name="newLayoutContext" /> contains the new graph
    /// rectangle.
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
        LayoutContext newLayoutContext
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(originalLayoutContext != null);
        Debug.Assert(newLayoutContext != null);
        AssertValid();

        // We want the graph to remain stationary, with the polar origin always
        // at the center of the graph rectangle.  Also, no scaling should occur
        // as the window changes size.

        Rectangle oOriginalGraphRectangle =
            originalLayoutContext.GraphRectangle;

        Rectangle oNewGraphRectangle =
            newLayoutContext.GraphRectangle;

        Matrix oMatrix = new Matrix();

        oMatrix.Translate(
            (oNewGraphRectangle.Width - oOriginalGraphRectangle.Width) / 2F,
            (oNewGraphRectangle.Height - oOriginalGraphRectangle.Height) / 2F
            );

        LayoutUtil.TransformVertexLocations(graph, oMatrix);
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

        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
