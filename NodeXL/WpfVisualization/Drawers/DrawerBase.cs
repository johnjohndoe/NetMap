
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using System.Windows.Media;
using Microsoft.NodeXL.Core;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: DrawerBase
//
/// <summary>
/// Base class for classes that perform drawing operations.
/// </summary>
//*****************************************************************************

public class DrawerBase : VisualizationBase
{
    //*************************************************************************
    //  Constructor: DrawerBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="DrawerBase" /> class.
    /// </summary>
    //*************************************************************************

    public DrawerBase()
    {
        // (Do nothing.)

        // AssertValid();
    }

    //*************************************************************************
    //  Event: RedrawRequired
    //
    /// <summary>
    /// Occurs when a change occurs that requires a graph redraw.
    /// </summary>
    ///
    /// <remarks>
    /// The event is fired when a change is made to the object that might
    /// affect the appearance of the graph.  The object owner should redraw the
    /// graph but does not have to lay out the graph again.
    /// </remarks>
    //*************************************************************************

    public event EventHandler RedrawRequired;


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
    /// affect the layout of the graph.  The owner should lay out the graph and
    /// redraw it in response to the event.
    /// </remarks>
    //*************************************************************************

    public event EventHandler LayoutRequired;


    //*************************************************************************
    //  Method: CreateFrozenSolidColorBrush()
    //
    /// <summary>
    /// Creates a SolidColorBrush and freezes it.
    /// </summary>
    ///
    /// <param name="oColor">
    /// The brush color.
    /// </param>
    ///
    /// <returns>
    /// A new frozen SolidColorBrush.
    /// </returns>
    //*************************************************************************

    protected SolidColorBrush
    CreateFrozenSolidColorBrush
    (
        Color oColor
    )
    {
        // AssertValid();

        SolidColorBrush oSolidColorBrush = new SolidColorBrush(oColor);

        WpfGraphicsUtil.FreezeIfFreezable(oSolidColorBrush);

        return (oSolidColorBrush);
    }

    //*************************************************************************
    //  Method: CreateFrozenPen()
    //
    /// <summary>
    /// Creates a Pen and freezes it.
    /// </summary>
    ///
    /// <param name="oBrush">
    /// The brush to use.
    /// </param>
    ///
    /// <param name="dThickness">
    /// The pen thickness.
    /// </param>
    ///
    /// <returns>
    /// A new frozen Pen.
    /// </returns>
    //*************************************************************************

    protected Pen
    CreateFrozenPen
    (
        Brush oBrush,
        Double dThickness
    )
    {
        Debug.Assert(oBrush != null);
        Debug.Assert(dThickness > 0);
        // AssertValid();

        Pen oPen = new Pen(oBrush, dThickness);

        WpfGraphicsUtil.FreezeIfFreezable(oPen);

        return (oPen);
    }


    //*************************************************************************
    //  Method: FireRedrawRequired()
    //
    /// <summary>
    /// Fires the <see cref="RedrawRequired" /> event if appropriate.
    /// </summary>
    //*************************************************************************

    protected void
    FireRedrawRequired()
    {
        AssertValid();

        EventUtil.FireEvent(this, this.RedrawRequired);
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
