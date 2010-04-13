
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Diagnostics;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: DraggedTranslation
//
/// <summary>
/// Represents a mouse drag that is translating the graph via a
/// TranslateTransform.
/// </summary>
///
/// <remarks>
/// Create an instance of this class when the graph is clicked while
/// <see cref="NodeXLControl.MouseMode" /> is set to <see
/// cref="MouseMode.Translate" />.  When the mouse is moved, call <see
/// cref="GetTranslationDistances" /> to get the distances to translate the
/// graph.
/// </remarks>
//*****************************************************************************

public class DraggedTranslation : MouseDrag
{
    //*************************************************************************
    //  Constructor: DraggedTranslation()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="DraggedTranslation" /> class.
    /// </summary>
    ///
    /// <param name="mouseDownLocation">
    /// Location where the graph was clicked, in client coordinates.
    /// </param>
    ///
    /// <param name="mouseDownLocationScreen">
    /// Location where the graph was clicked, in screen coordinates.
    /// </param>
    ///
    /// <param name="mouseDownTranslateX">
    /// x-axis translation when the mouse was clicked.
    /// </param>
    ///
    /// <param name="mouseDownTranslateY">
    /// y-axis translation when the mouse was clicked.
    /// </param>
    //*************************************************************************

    public DraggedTranslation
    (
        Point mouseDownLocation,
        Point mouseDownLocationScreen,
        Double mouseDownTranslateX,
        Double mouseDownTranslateY
    )
    : base(mouseDownLocation)
    {
        m_oMouseDownLocationScreen = mouseDownLocationScreen;
        m_dMouseDownTranslateX = mouseDownTranslateX;
        m_dMouseDownTranslateY = mouseDownTranslateY;

        AssertValid();
    }

    //*************************************************************************
    //  Method: GetTranslationDistances()
    //
    /// <summary>
    /// Gets the distances to translate the graph.
    /// </summary>
    ///
    /// <param name="currentMouseLocationScreen">
    /// The current mouse location, in screen coordinates.
    /// </param>
    ///
    /// <param name="newTranslateX">
    /// Where the new x-axis translation distance gets stored.
    /// </param>
    ///
    /// <param name="newTranslateY">
    /// Where the new y-axis translation distance gets stored.
    /// </param>
    ///
    /// <remarks>
    /// The returned distances are in units appropriate for the
    /// TranslateTransform.X and Y properties.
    ///
    /// <para>
    /// The returned distances are not limited -- they can force the graph to
    /// move outside the control.  It's up to the caller to limit the
    /// distances.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    GetTranslationDistances
    (
        Point currentMouseLocationScreen,
        out Double newTranslateX,
        out Double newTranslateY
    )
    {
        AssertValid();

        // These calculations use screen coordinates for the mouse positions,
        // because screen coordindates are not affected by the scale transforms
        // used for the control's layout and render transforms.  The mouse
        // locations reported by the MouseMove event are affected by the
        // transforms and would be more difficult to work with.

        newTranslateX = m_dMouseDownTranslateX +
            (currentMouseLocationScreen.X - m_oMouseDownLocationScreen.X);

        newTranslateY = m_dMouseDownTranslateY +
            (currentMouseLocationScreen.Y - m_oMouseDownLocationScreen.Y);
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

        // m_oMouseDownLocationScreen
        // m_dMouseDownTranslateX
        // m_dMouseDownTranslateY
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Location where the graph was clicked, in screen coordinates.

    protected Point m_oMouseDownLocationScreen;

    /// x-axis translation when the mouse was clicked.

    protected Double m_dMouseDownTranslateX;

    /// y-axis translation when the mouse was clicked.

    protected Double m_dMouseDownTranslateY;
}

}
