
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Media;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: EdgeDrawingHistory
//
/// <summary>
/// Retains information about how one edge was drawn.
/// </summary>
//*****************************************************************************

public class EdgeDrawingHistory : DrawingHistory
{
    //*************************************************************************
    //  Constructor: EdgeDrawingHistory()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeDrawingHistory" />
    /// class.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge that was drawn.
    /// </param>
    ///
    /// <param name="drawingVisual">
    /// The DrawingVisual object that was used to draw the edge.
    /// </param>
    ///
    /// <param name="drawnAsSelected">
    /// true if the edge was drawn as selected.
    /// </param>
    //*************************************************************************

    public EdgeDrawingHistory
    (
        IEdge edge,
        DrawingVisual drawingVisual,
        Boolean drawnAsSelected
    )
    : base(drawingVisual, drawnAsSelected)
    {
        m_oEdge = edge;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: Edge
    //
    /// <summary>
    /// Gets the edge that was drawn.
    /// </summary>
    ///
    /// <value>
    /// The edge that was drawn, as an <see cref="IEdge" />
    /// </value>
    //*************************************************************************

    public IEdge
    Edge
    {
        get
        {
            AssertValid();

            return (m_oEdge);
        }
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

        Debug.Assert(m_oEdge != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The edge that was drawn.

    protected IEdge m_oEdge;
}

}
