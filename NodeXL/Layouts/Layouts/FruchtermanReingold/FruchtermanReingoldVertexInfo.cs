
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;

namespace Microsoft.NodeXL.Layouts
{
//*****************************************************************************
//  Class: FruchtermanReingoldVertexInfo
//
/// <summary>
/// Stores information calculated by <see cref="FruchtermanReingoldLayout" />
/// for one vertex.
/// </summary>
///
/// <remarks>
/// <see cref="FruchtermanReingoldLayout" /> calculates several pieces of
/// information about each vertex in the graph being laid out.  Instead of
/// storing each piece of information in a separate vertex metadata key, which
/// would require multiple key lookups and inefficient boxing and unboxing of
/// value types, it stores all the information for the vertex in one instance
/// of type <see cref="FruchtermanReingoldVertexInfo" /> and stores the
/// instance in the vertex's Tag.
///
/// <para>
/// All data is exposed as public fields instead of properties.  That's because
/// the method in <see cref="FruchtermanReingoldLayout" /> that calculates
/// repulsive forces accesses the data repeatedly in an O(V-squared) loop, and
/// property getters are much slower than direct field accesses.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class FruchtermanReingoldVertexInfo : LayoutsBase
{
    //*************************************************************************
    //  Constructor: FruchtermanReingoldVertexInfo()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="FruchtermanReingoldVertexInfo" /> class.
    /// </summary>
    ///
    /// <param name="initialLocation">
    /// The vertex's initial location within the bounded graph rectangle.
    /// </param>
    //*************************************************************************

    public FruchtermanReingoldVertexInfo
    (
        PointF initialLocation
    )
    {
        UnboundedLocationX = initialLocation.X;
        UnboundedLocationY = initialLocation.Y;

        DisplacementX = 0;
        DisplacementY = 0;

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

        // UnboundedLocationX
        // UnboundedLocationY
        // DisplacementX
        // DisplacementY
    }


    //*************************************************************************
    //  Public fields
    //*************************************************************************

    /// The vertex's location within an unbounded rectangle.

    public Single UnboundedLocationX;
    ///
    public Single UnboundedLocationY;

    /// The vertex's displacement with respect to its current unbounded
    /// location.

    public Single DisplacementX;
    ///
    public Single DisplacementY;
}

}
