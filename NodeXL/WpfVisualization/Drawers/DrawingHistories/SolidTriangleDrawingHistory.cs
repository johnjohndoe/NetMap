
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: SolidTriangleVertexDrawingHistory
//
/// <summary>
/// Retains information about how one vertex was drawn as a <see
/// cref="VertexShape.SolidTriangle" />.
/// </summary>
//*****************************************************************************

public class SolidTriangleVertexDrawingHistory : TriangleVertexDrawingHistory
{
    //*************************************************************************
    //  Constructor: SolidTriangleVertexDrawingHistory()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="SolidTriangleVertexDrawingHistory" />
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
    /// <param name="halfWidth">
    /// The half-width of the triangle that was drawn for <paramref
    /// name="vertex" />.
    /// </param>
    //*************************************************************************

    public SolidTriangleVertexDrawingHistory
    (
        IVertex vertex,
        DrawingVisual drawingVisual,
        Boolean drawnAsSelected,
        Double halfWidth
    )
    : base(vertex, drawingVisual, drawnAsSelected, halfWidth)
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
