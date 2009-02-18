
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: DraggedMarquee
//
/// <summary>
/// Represents a marquee that might be dragged with the mouse.
/// </summary>
///
/// <remarks>
/// Create an instance of this class when an empty area of the graph is
/// clicked.  When the mouse is moved, check <see
/// cref="MouseDrag.OnMouseMove" /> to determine whether the mouse has moved
/// far enough to begin a marquee drag.  If <see
/// cref="MouseDrag.OnMouseMove" /> returns true, call <see
/// cref="CreateVisual" /> to create a Visual to represent the dragged marquee.
/// </remarks>
//*****************************************************************************

public class DraggedMarquee : MouseDragWithVisual
{
    //*************************************************************************
    //  Constructor: DraggedMarquee()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="DraggedMarquee" /> class.
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

    public DraggedMarquee
    (
        Point mouseDownLocation,
        Rect graphRectangle,
        Int32 margin
    )
    : base(mouseDownLocation, graphRectangle, margin)
    {
        m_oMarqueeRectangle = Rect.Empty;

        AssertValid();
    }

    //*************************************************************************
    //  Property: MarqueeRectangle
    //
    /// <summary>
    /// Gets the rectangle that represents the marquee.
    /// </summary>
    ///
    /// <value>
    /// The rectangle that was created by <see cref="CreateVisual" />, or
    /// Rect.Empty if <see cref="CreateVisual" /> hasn't been called.
    /// </value>
    //*************************************************************************

    public Rect
    MarqueeRectangle
    {
        get
        {
            AssertValid();

            return (m_oMarqueeRectangle);
        }
    }

    //*************************************************************************
    //  Method: CreateVisual()
    //
    /// <summary>
    /// Creates the Visual that should be used to represent the dragged
    /// marquee.
    /// </summary>
    ///
    /// <param name="currentMouseLocation">
    /// The current mouse location.
    /// </param>
    ///
    /// <param name="backgroundContrastColor">
    /// A color that contrasts with the control's background.
    /// </param>
    ///
    /// <returns>
    /// The Visual that should be used to represent the dragged marquee.
    /// </returns>
    ///
    /// <remarks>
    /// The returned Visual can be retrieved later via the <see
    /// cref="MouseDragWithVisual.Visual" /> property.
    /// </remarks>
    //*************************************************************************

    public Visual
    CreateVisual
    (
        Point currentMouseLocation,
        Color backgroundContrastColor
    )
    {
        Debug.Assert(m_bDragIsInProgress);
        AssertValid();

        // Limit the drag to the graph margins.

        currentMouseLocation =
            ForcePointToBeWithinMargins(currentMouseLocation);

        m_oMarqueeRectangle = CreateMarqueeRectangle(currentMouseLocation);

        DrawingVisual oDrawingVisual = new DrawingVisual();

        SolidColorBrush oFillBrush = new SolidColorBrush(Color.FromArgb(
            MarqueeFillAlpha,
            backgroundContrastColor.R,
            backgroundContrastColor.G,
            backgroundContrastColor.B
            ) );

        SolidColorBrush oOutlineBrush = new SolidColorBrush(Color.FromArgb(
            MarqueeOutlineAlpha,
            backgroundContrastColor.R,
            backgroundContrastColor.G,
            backgroundContrastColor.B
            ) );

        Pen oOutlinePen = new Pen(oOutlineBrush, 1);

        WpfGraphicsUtil.FreezeIfFreezable(oFillBrush);
        WpfGraphicsUtil.FreezeIfFreezable(oOutlineBrush);
        WpfGraphicsUtil.FreezeIfFreezable(oOutlinePen);

        using ( DrawingContext oDrawingContext = oDrawingVisual.RenderOpen() )
        {
            oDrawingContext.DrawRectangle(oFillBrush, oOutlinePen,
                m_oMarqueeRectangle);
        }

        m_oVisual = oDrawingVisual;

        return (m_oVisual);
    }

    //*************************************************************************
    //  Method: CreateMarqueeRectangle()
    //
    /// <summary>
    /// Creates the rectangle to use for the marquee Visual.
    /// </summary>
    ///
    /// <param name="oCurrentMouseLocation">
    /// The current mouse location.
    /// </param>
    //*************************************************************************

    protected Rect
    CreateMarqueeRectangle
    (
        Point oCurrentMouseLocation
    )
    {
        AssertValid();
        Debug.Assert(m_bDragIsInProgress);

        Double dCurrentLocationX = oCurrentMouseLocation.X;
        Double dCurrentLocationY = oCurrentMouseLocation.Y;

        Double dMouseDownLocationX = m_oMouseDownLocation.X;
        Double dMouseDownLocationY = m_oMouseDownLocation.Y;

        return ( new Rect(
            Math.Min(dMouseDownLocationX, dCurrentLocationX),
            Math.Min(dMouseDownLocationY, dCurrentLocationY),
            Math.Abs(dCurrentLocationX - dMouseDownLocationX),
            Math.Abs(dCurrentLocationY - dMouseDownLocationY)
            ) );
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

        // m_oMarqueeRectangle
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Alpha value to use to fill the marquee.

    protected const Byte MarqueeFillAlpha = 64;

    /// Alpha value to use for the marquee outline.

    protected const Byte MarqueeOutlineAlpha = 255;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The rectangle used to create m_oMarqueeVisual, or Rect.Empty if there
    /// is no marquee.

    protected Rect m_oMarqueeRectangle;
}

}
