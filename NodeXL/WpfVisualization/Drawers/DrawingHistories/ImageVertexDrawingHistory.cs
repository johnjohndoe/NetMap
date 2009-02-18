
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: ImageVertexDrawingHistory
//
/// <summary>
/// Retains information about how one vertex was drawn as an image.
/// </summary>
//*****************************************************************************

public class ImageVertexDrawingHistory : RectangleVertexDrawingHistory
{
    //*************************************************************************
    //  Constructor: ImageVertexDrawingHistory()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ImageVertexDrawingHistory" />
    /// class.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex that was drawn.
    /// </param>
    ///
    /// <param name="drawingVisual">
    /// The DrawingVisual object that was used to draw the vertex.
    /// </param>
    ///
    /// <param name="drawnAsSelected">
    /// true if the vertex was drawn as selected.
    /// </param>
    ///
    /// <param name="rectangle">
    /// The rectangle that was drawn for <paramref name="vertex" />.
    /// </param>
    //*************************************************************************

    public ImageVertexDrawingHistory
    (
        IVertex vertex,
        DrawingVisual drawingVisual,
        Boolean drawnAsSelected,
        Rect rectangle
    )
    : base(vertex, drawingVisual, drawnAsSelected, rectangle)
    {
        // (Do nothing else.)

        // AssertValid();
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
