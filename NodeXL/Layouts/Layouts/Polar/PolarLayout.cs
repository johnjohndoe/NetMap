
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Layouts
{
//*****************************************************************************
//  Class: PolarLayout
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
/// of the graph rectangle.  0.0 represents the origin and 1.0 represents the
/// maximum distance from the origin, which is the smaller of half the graph
/// rectangle's width or height.  R values less than 0.0 are the same as 0.0,
/// and R values greater than 1.0 are the same as 1.0.
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

public class PolarLayout : PolarLayoutBase
{
    //*************************************************************************
    //  Constructor: PolarLayout()
    //
    /// <summary>
    /// Initializes a new instance of the PolarLayout class.
    /// </summary>
    //*************************************************************************

    public PolarLayout()
    :
    base(false)
    {
        // (Do nothing else.)

        AssertValid();
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
