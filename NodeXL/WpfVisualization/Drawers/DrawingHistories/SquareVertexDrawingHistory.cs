
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: SquareVertexDrawingHistory
//
/// <summary>
/// Retains information about how one vertex was drawn as a <see
/// cref="VertexShape.Square" />.
/// </summary>
//*****************************************************************************

public class SquareVertexDrawingHistory : RectangleVertexDrawingHistory
{
    //*************************************************************************
    //  Constructor: SquareVertexDrawingHistory()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="SquareVertexDrawingHistory" />
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
    /// <param name="square">
    /// The square that was drawn for <paramref name="vertex" />.
    /// </param>
    //*************************************************************************

    public SquareVertexDrawingHistory
    (
        IVertex vertex,
        DrawingVisual drawingVisual,
        Boolean drawnAsSelected,
        Rect square
    )
    : base(vertex, drawingVisual, drawnAsSelected, square)
    {
        // (Do nothing.)

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
