
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
/// instance in a single key.
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
        m_fUnboundedLocationX = initialLocation.X;
        m_fUnboundedLocationY = initialLocation.Y;

        m_fDisplacementX = 0;
        m_fDisplacementY = 0;

        AssertValid();
    }

    //*************************************************************************
    //  Property: UnboundedLocationX
    //
    /// <summary>
    /// Gets or sets the x-coordinate of the vertex's location within an
    /// unbounded rectangle.
    /// </summary>
    ///
    /// <value>
    /// The x-coordinate of the vertex's location within an unbounded
    /// rectangle.
    /// </value>
    //*************************************************************************

    public Single
    UnboundedLocationX
    {
        get
        {
            AssertValid();

            return (m_fUnboundedLocationX);
        }

        set
        {
            m_fUnboundedLocationX = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: UnboundedLocationY
    //
    /// <summary>
    /// Gets or sets the y-coordinate of the vertex's location within an
    /// unbounded rectangle.
    /// </summary>
    ///
    /// <value>
    /// The y-coordinate of the vertex's location within an unbounded
    /// rectangle.
    /// </value>
    //*************************************************************************

    public Single
    UnboundedLocationY
    {
        get
        {
            AssertValid();

            return (m_fUnboundedLocationY);
        }

        set
        {
            m_fUnboundedLocationY = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DisplacementX
    //
    /// <summary>
    /// Gets or sets the vertex's horizontal displacement with respect to its
    /// current unbounded location.
    /// </summary>
    ///
    /// <value>
    /// The vertex's horizontal displacement with respect to its current
    /// unbounded location.  The default value is zero.
    /// </value>
    //*************************************************************************

    public Single
    DisplacementX
    {
        get
        {
            AssertValid();

            return (m_fDisplacementX);
        }

        set
        {
            m_fDisplacementX = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DisplacementY
    //
    /// <summary>
    /// Gets or sets the vertex's vertical displacement with respect to its
    /// current unbounded location.
    /// </summary>
    ///
    /// <value>
    /// The vertex's vertical displacement with respect to its current
    /// unbounded location.  The default value is zero.
    /// </value>
    //*************************************************************************

    public Single
    DisplacementY
    {
        get
        {
            AssertValid();

            return (m_fDisplacementY);
        }

        set
        {
            m_fDisplacementY = value;

            AssertValid();
        }
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

        // m_fUnboundedLocationX
        // m_fUnboundedLocationY
        // m_fDisplacementX
        // m_fDisplacementY
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The vertex's location within an unbounded rectangle.

    protected Single m_fUnboundedLocationX;
    ///
    protected Single m_fUnboundedLocationY;

    /// The vertex's displacement with respect to its current unbounded
    /// location.

    protected Single m_fDisplacementX;
    ///
    protected Single m_fDisplacementY;
}

}
