
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: MouseDrag
//
/// <summary>
/// Represents a mouse drag operation.
/// </summary>
///
/// <remarks>
/// Create an instance of this class when a MouseDown event occurs.  When the
/// mouse is moved, call <see cref="OnMouseMove" /> to determine whether the
/// mouse has moved far enough to begin a mouse drag operation.  When the mouse
/// button is released, call <see cref="OnMouseUp" />.
/// </remarks>
//*****************************************************************************

public class MouseDrag : VisualizationBase
{
    //*************************************************************************
    //  Constructor: MouseDrag()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="MouseDrag" /> class.
    /// </summary>
    ///
    /// <param name="mouseDownLocation">
    /// Location where the MouseDown event occurred, in client coordinates.
    /// </param>
    //*************************************************************************

    public MouseDrag
    (
        Point mouseDownLocation
    )
    {
        m_oMouseDownLocation = mouseDownLocation;
        m_bDragIsInProgress = false;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: MouseDownLocation
    //
    /// <summary>
    /// Gets the location where the MouseDown event occurred.
    /// </summary>
    ///
    /// <value>
    /// The location where the MouseDown event occurred, as a <see
    /// cref="Point" /> in client coordinates.
    /// </value>
    //*************************************************************************

    public Point
    MouseDownLocation
    {
        get
        {
            AssertValid();

            return (m_oMouseDownLocation);
        }
    }

    //*************************************************************************
    //  Method: OnMouseMove()
    //
    /// <summary>
    /// Returns a flag indicating whether a drag operation is in progress.
    /// </summary>
    ///
    /// <param name="currentMouseLocation">
    /// The current mouse location.
    /// </param>
    ///
    /// <returns>
    /// true if a drag operation is in progress.
    /// </returns>
    ///
    /// <remarks>
    /// Call this when the mouse is moved.
    /// </remarks>
    //*************************************************************************

    public Boolean
    OnMouseMove
    (
        Point currentMouseLocation
    )
    {
        AssertValid();

        // Once a drag operation starts, it continues until the mouse button is
        // released.

        if (!m_bDragIsInProgress)
        {
            Point oOldLocation = this.MouseDownLocation;
            Double dOldLocationX = oOldLocation.X;
            Double dOldLocationY = oOldLocation.Y;

            Double dCurrentLocationX = currentMouseLocation.X;
            Double dCurrentLocationY = currentMouseLocation.Y;

            if (
                Math.Abs(dCurrentLocationX - dOldLocationX) >= MinimumMouseMove
                ||
                Math.Abs(dCurrentLocationY - dOldLocationY) >= MinimumMouseMove
                )
            {
                m_bDragIsInProgress = true;
            }
        }

        return (m_bDragIsInProgress);
    }

    //*************************************************************************
    //  Method: OnMouseUp()
    //
    /// <summary>
    /// Returns a flag indicating whether a drag operation is in progress.
    /// </summary>
    ///
    /// <returns>
    /// true if a drag operation is in progress.
    /// </returns>
    ///
    /// <remarks>
    /// Call this when the mouse button is released.
    /// </remarks>
    //*************************************************************************

    public Boolean
    OnMouseUp()
    {
        AssertValid();

        return (m_bDragIsInProgress);
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

        // m_oMouseDownLocation
        // m_bDragIsInProgress
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Minimum change in either coordinate before a drag operation begins.
    /// Having a minimum move distance reduces the chance that the user will
    /// inadvertently begin a drag operation.

    protected const Single MinimumMouseMove = 4.0F;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Location where the MouseDown event occurred, in client coordinates.

    protected Point m_oMouseDownLocation;

    /// true if a drag operation is in progress.

    protected Boolean m_bDragIsInProgress;
}

}
