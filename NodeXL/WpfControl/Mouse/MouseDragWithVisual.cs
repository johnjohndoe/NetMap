
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Microsoft.WpfGraphicsLib;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: MouseDragWithVisual
//
/// <summary>
/// Represents a dragged object that has a visual representation.
/// </summary>
///
/// <remarks>
/// This is an abstract base class.  It maintains a Visual property that
/// represents an object being dragged.  It is up to the derived class to
/// create the object.
/// </remarks>
//*****************************************************************************

public abstract class MouseDragWithVisual : MouseDrag
{
    //*************************************************************************
    //  Constructor: MouseDragWithVisual()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="MouseDragWithVisual" />
    /// class.
    /// </summary>
    ///
    /// <param name="mouseDownLocation">
    /// Location where the graph was clicked, in client coordinates.
    /// </param>
    ///
    /// <param name="graphRectangle">
    /// The graph rectangle.
    /// </param>
    ///
    /// <param name="margin">
    /// The graph margin.
    /// </param>
    //*************************************************************************

    public MouseDragWithVisual
    (
        Point mouseDownLocation,
        Rect graphRectangle,
        Int32 margin
    )
    : base(mouseDownLocation)
    {
        m_oGraphRectangle = graphRectangle;
        m_iMargin = margin;
        m_oVisual = null;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: Visual
    //
    /// <summary>
    /// Gets the Visual that represents the dragged object.
    /// </summary>
    ///
    /// <value>
    /// The Visual that represents the dragged object, or null if the Visual
    /// hasn't been created yet.
    /// </value>
    //*************************************************************************

    public Visual
    Visual
    {
        get
        {
            AssertValid();

            return (m_oVisual);
        }
    }

    //*************************************************************************
    //  Property: GraphRectangleMinusMargin
    //
    /// <summary>
    /// Gets the rectangle that defines the bounds of the graph, minus the
    /// margin.
    /// </summary>
    ///
    /// <value>
    /// The rectangle that defines the bounds of the graph, minus the margin,
    /// as a Rect.
    /// </value>
    //*************************************************************************

    protected Rect
    GraphRectangleMinusMargin
    {
        get
        {
            return ( WpfGraphicsUtil.GetRectangleMinusMargin(
                m_oGraphRectangle, m_iMargin) );
        }
    }

    //*************************************************************************
    //  Method: ForcePointToBeWithinMargins()
    //
    /// <summary>
    /// Forces a point to fall within the graph's margins.
    /// </summary>
    ///
    /// <param name="oPoint">
    /// The point to force.
    /// </param>
    ///
    /// <returns>
    /// The forced point.
    /// </returns>
    //*************************************************************************

    protected Point
    ForcePointToBeWithinMargins
    (
        Point oPoint
    )
    {
        AssertValid();

        Double dX = oPoint.X;
        Double dY = oPoint.Y;

        Rect oGraphRectangleMinusMargin = this.GraphRectangleMinusMargin;

        dX = Math.Max(dX, oGraphRectangleMinusMargin.Left);
        dX = Math.Min(dX, oGraphRectangleMinusMargin.Right);
        dY = Math.Max(dY, oGraphRectangleMinusMargin.Top);
        dY = Math.Min(dY, oGraphRectangleMinusMargin.Bottom);

        return ( new Point(dX, dY) );
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

        // m_oGraphRectangle
        Debug.Assert(m_iMargin >= 0);
        // m_oVisual
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The graph rectangle.

    protected Rect m_oGraphRectangle;

    /// The graph margin.

    protected Int32 m_iMargin;

    /// The Visual that represents the dragged object, or null if the derived
    /// class hasn't created the Visual yet.

    protected Visual m_oVisual;
}

}
