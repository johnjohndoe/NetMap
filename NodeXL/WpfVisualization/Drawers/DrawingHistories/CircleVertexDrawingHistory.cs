
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: CircleVertexDrawingHistory
//
/// <summary>
/// Retains information about how one vertex was drawn as a <see
/// cref="VertexShape.Circle" />.
/// </summary>
//*****************************************************************************

public class CircleVertexDrawingHistory : VertexDrawingHistory
{
    //*************************************************************************
    //  Constructor: CircleVertexDrawingHistory()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="CircleVertexDrawingHistory" />
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

    public CircleVertexDrawingHistory
    (
        IVertex vertex,
        DrawingVisual drawingVisual,
        Boolean drawnAsSelected,
        Double radius
    )
    : base(vertex, drawingVisual, drawnAsSelected)
    {
        m_dRadius = radius;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: Radius
    //
    /// <summary>
    /// Gets the radius of the vertex that was drawn.
    /// </summary>
    ///
    /// <value>
    /// The radius of the vertex that was drawn, as a Double.
    /// </value>
    //*************************************************************************

    public Double
    Radius
    {
        get
        {
            AssertValid();

            return (m_dRadius);
        }
    }

    //*************************************************************************
    //  Method: GetEdgeEndpoint()
    //
    /// <summary>
    /// Gets the endpoint of an edge that is connected to <see
    /// cref="Vertex" />.
    /// </summary>
    ///
    /// <param name="otherVertexDrawingHistory">
    /// The <paramref name="VertexDrawingHistory" /> object that retains
    /// information about how the edge's other vertex was drawn.
    /// </param>
    ///
    /// <param name="edgeEndpoint">
    /// Where the edge endpoint gets stored.  The endpoint is somewhere on <see
    /// cref="Vertex" />.
    /// </param>
    //*************************************************************************

    public override void
    GetEdgeEndpoint
    (
        VertexDrawingHistory otherVertexDrawingHistory,
        out Point edgeEndpoint
    )
    {
        AssertValid();

        GetEdgeEndpointOnCircle(this.VertexLocation, this.Radius,
            otherVertexDrawingHistory.VertexLocation, out edgeEndpoint);
    }

    //*************************************************************************
    //  Method: GetSelfLoopEndpoint()
    //
    /// <summary>
    /// Gets the endpoint of an edge that is connected to <see
    /// cref="VertexDrawingHistory.Vertex" /> and is a self-loop.
    /// </summary>
    ///
    /// <param name="farthestGraphRectangleEdge">
    /// The edge of the graph rectangle that is farthest from <see
    /// cref="VertexDrawingHistory.Vertex" />.
    /// </param>
    ///
    /// <returns>
    /// The self-loop endpoint.  The endpoint is somewhere on <see
    /// cref="VertexDrawingHistory.Vertex" />.
    /// </returns>
    ///
    /// <remarks>
    /// A self-loop is an edge that connects a vertex to itself.  This method
    /// determines the single endpoint of the self-loop, which gets drawn as a
    /// line looping back to its starting point.
    /// </remarks>
    //*************************************************************************

    public override Point
    GetSelfLoopEndpoint
    (
        RectangleEdge farthestGraphRectangleEdge
    )
    {
        AssertValid();

        return ( GetSelfLoopEndpointOnRectangle(GetBoundingSquare(m_dRadius),
            farthestGraphRectangleEdge) );
    }

    //*************************************************************************
    //  Method: GetLabelLocation()
    //
    /// <summary>
    /// Gets the location at which an annotation label should be drawn.
    /// </summary>
    ///
    /// <param name="labelPosition">
    /// The position of the annotation label.
    /// </param>
    ///
    /// <returns>
    /// The point at which an annotation label should be drawn.
    /// </returns>
    ///
    /// <remarks>
    /// The returned point assumes that the label text height is zero and that
    /// there is zero margin between the vertex and the label.  The caller must
    /// adjust the point for the actual text height and any margin.
    /// </remarks>
    //*************************************************************************

    public override Point
    GetLabelLocation
    (
        VertexLabelPosition labelPosition
    )
    {
        AssertValid();

        // The diamond pattern for label locations is appropriate for circles.

        return ( GetLabelLocationOnDiamond(labelPosition, m_dRadius) );
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

        Debug.Assert(m_dRadius > 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The radius of the vertex.

    protected Double m_dRadius;
}

}
