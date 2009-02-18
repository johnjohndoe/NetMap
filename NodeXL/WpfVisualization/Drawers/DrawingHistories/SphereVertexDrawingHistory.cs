
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: SphereVertexDrawingHistory
//
/// <summary>
/// Retains information about how one vertex was drawn as a <see
/// cref="VertexShape.Sphere" />.
/// </summary>
//*****************************************************************************

public class SphereVertexDrawingHistory : CircleVertexDrawingHistory
{
    //*************************************************************************
    //  Constructor: SphereVertexDrawingHistory()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="SphereVertexDrawingHistory" />
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
    /// <param name="radius">
    /// The radius of <paramref name="vertex" />.
    /// </param>
    //*************************************************************************

    public SphereVertexDrawingHistory
    (
        IVertex vertex,
        DrawingVisual drawingVisual,
        Boolean drawnAsSelected,
        Double radius
    )
    : base(vertex, drawingVisual, drawnAsSelected, radius)
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
