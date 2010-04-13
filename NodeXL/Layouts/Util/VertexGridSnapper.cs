
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using Microsoft.NodeXL.Core;
using System.Diagnostics;

namespace Microsoft.NodeXL.Layouts
{
//*****************************************************************************
//  Class: VertexGridSnapper
//
/// <summary>
/// Snaps vertices to a grid.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class VertexGridSnapper
{
    //*************************************************************************
    //  Method: SnapVerticesToGrid()
    //
    /// <summary>
    /// Snaps a graph's vertices to a grid.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph whose vertices should be snapped to a grid.  The graph should
    /// already have been laid out.
    /// </param>
    ///
    /// <param name="gridSize">
    /// Distance between gridlines.  Must be greater than zero.
    /// </param>
    ///
    /// <remarks>
    /// This method can be used to separate vertices that obscure each other
    /// by snapping them to the nearest grid location, using a specified grid
    /// size.  The graph should be laid out before this method is called.
    ///
    /// <para>
    /// Snapping the vertices to a grid can cause them to fall outside the
    /// rectangle in which they were originally laid out.  If the
    /// Visualization.Wpf.VertexDrawer class is used to draw the vertices, that
    /// class will automatically take care of moving such vertices back within
    /// the rectangle.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static void
    SnapVerticesToGrid
    (
        IGraph graph,
        Int32 gridSize
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(gridSize > 0);

        Single fGridSize = (Single)gridSize;

        foreach (IVertex oVertex in graph.Vertices)
        {
            PointF oOldLocation = oVertex.Location;

            oVertex.Location = new PointF(
                SnapCoordinateToGrid(oOldLocation.X, fGridSize),
                SnapCoordinateToGrid(oOldLocation.Y, fGridSize)
                );
        }
    }

    //*************************************************************************
    //  Method: SnapCoordinateToGrid()
    //
    /// <summary>
    /// Snaps an x- or y-coordinate to a grid.
    /// </summary>
    ///
    /// <param name="fCoordinate">
    /// The x- or y-coordinate to snap to a grid.
    /// </param>
    ///
    /// <param name="fGridSize">
    /// Distance between gridlines.  Must be an itegral multiple greater than
    /// zero.
    /// </param>
    //*************************************************************************

    private static Single
    SnapCoordinateToGrid
    (
        Single fCoordinate,
        Single fGridSize
    )
    {
        Debug.Assert(fGridSize > 0);

        return ( fGridSize * (Single)Math.Round(fCoordinate / fGridSize) );
    }
}

}
