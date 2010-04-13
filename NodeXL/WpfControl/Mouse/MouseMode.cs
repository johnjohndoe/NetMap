
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Enum: MouseMode
//
/// <summary>
/// Determines how the mouse can be used to interact with the graph.
/// </summary>
///
/// <remarks>
/// Note that for all values except <see cref="DoNothing" />, the following
/// actions are available:
///
/// <list type="bullet">
///
/// <item><term>
/// The mouse wheel zooms the graph in and out.
/// </term></item>
///
/// <item><term>
/// If the graph is zoomed in, dragging the mouse with the middle button
/// translates the entire graph.
/// </term></item>
///
/// <item><term>
/// If the graph is zoomed in, dragging the mouse with the left button while
/// holding down the space key translates the entire graph.
/// </term></item>
///
/// </list>
///
/// <para>
/// Also, if the value is <see cref="Select" />, <see cref="AddToSelection" />,
/// or <see cref="SubtractFromSelection" />, holding down the control key while
/// clicking a vertex inverts its selection state.
/// </para>
///
/// <para>
/// In the descriptions below, "click" means "click with the left button."
/// </para>
///
/// </remarks>
//*****************************************************************************

public enum
MouseMode
{
    /// <summary>
    /// The mouse buttons and wheel do nothing.
    /// </summary>

    DoNothing,

    /// <summary>
    /// Any selected vertices and edges are first deselected.  Clicking a
    /// vertex selects it.  Dragging a selected vertex moves all selected
    /// vertices.  Dragging a marquee starting at an empty area of the
    /// graph selects all the contained vertices.
    /// </summary>

    Select,

    /// <summary>
    /// Clicking a vertex adds it to the selection.  Dragging a marquee
    /// starting at an empty area of the graph adds all the contained vertices
    /// to the selection.
    /// </summary>

    AddToSelection,

    /// <summary>
    /// Clicking a vertex subtracts it from the selection.  Dragging a marquee
    /// starting at an empty area of the graph subtracts all the contained
    /// vertices from the selection.
    /// </summary>

    SubtractFromSelection,

    /// <summary>
    /// The graph is zoomed in to the clicked point.
    /// </summary>

    ZoomIn,

    /// <summary>
    /// The graph is zoomed out from the clicked point.
    /// </summary>

    ZoomOut,

    /// <summary>
    /// If the graph is zoomed in, dragging the mouse translates the entire
    /// graph.
    /// </summary>

    Translate,
}

}
