
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
//  Class: RectangleVertexDrawingHistory
//
/// <summary>
/// Retains information about how one vertex was drawn as a rectangle.
/// </summary>
//*****************************************************************************

public class RectangleVertexDrawingHistory : VertexDrawingHistory
{
    //*************************************************************************
    //  Constructor: RectangleVertexDrawingHistory()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="RectangleVertexDrawingHistory" />
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

    public RectangleVertexDrawingHistory
    (
        IVertex vertex,
        DrawingVisual drawingVisual,
        Boolean drawnAsSelected,
        Rect rectangle
    )
    : base(vertex, drawingVisual, drawnAsSelected)
    {
        m_oRectangle = rectangle;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: Rectangle
    //
    /// <summary>
    /// Gets the rectangle that was drawn.
    /// </summary>
    ///
    /// <value>
    /// The rectangle that was drawn, as a Rect.
    /// </value>
    //*************************************************************************

    public Rect
    Rectangle
    {
        get
        {
            AssertValid();

            return (m_oRectangle);
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

        GetEdgeEndpointOnRectangle(this.VertexLocation, this.Rectangle,
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

        return ( GetSelfLoopEndpointOnRectangle(this.Rectangle,
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

        Double dCenterX = m_oRectangle.Left + m_oRectangle.Width / 2.0;
        Double dMiddleY = m_oRectangle.Top + m_oRectangle.Height / 2.0;

        switch (labelPosition)
        {
            case VertexLabelPosition.TopLeft:

                return (m_oRectangle.TopLeft);

            case VertexLabelPosition.TopCenter:

                return ( new Point(dCenterX, m_oRectangle.Top) );

            case VertexLabelPosition.TopRight:

                return (m_oRectangle.TopRight);

            case VertexLabelPosition.MiddleLeft:

                return ( new Point(m_oRectangle.Left, dMiddleY) );

            case VertexLabelPosition.MiddleCenter:

                return ( new Point(dCenterX, dMiddleY) );

            case VertexLabelPosition.MiddleRight:

                return ( new Point(m_oRectangle.Right, dMiddleY) );

            case VertexLabelPosition.BottomLeft:

                return (m_oRectangle.BottomLeft);

            case VertexLabelPosition.BottomCenter:

                return ( new Point(dCenterX, m_oRectangle.Bottom) );

            case VertexLabelPosition.BottomRight:

                return (m_oRectangle.BottomRight);

            default:

                Debug.Assert(false);
                return ( new Point() );
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

        // m_oRectangle
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The rectangle that was drawn.

    protected Rect m_oRectangle;
}

}
