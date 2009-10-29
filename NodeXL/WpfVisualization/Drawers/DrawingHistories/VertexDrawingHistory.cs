
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using Microsoft.WpfGraphicsLib;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: VertexDrawingHistory
//
/// <summary>
/// Retains information about how one vertex was drawn.
/// </summary>
///
/// <remarks>
/// This is an abstract base class.  There is one concrete derived class for
/// each type of vertex that can be drawn.  The derived classes must implement
/// the <see cref="GetEdgeEndpoint" />, <see cref="GetSelfLoopEndpoint" />, and
/// <see cref="GetLabelLocation" /> methods.
/// </remarks>
//*****************************************************************************

public abstract class VertexDrawingHistory : DrawingHistory
{
    //*************************************************************************
    //  Constructor: VertexDrawingHistory()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexDrawingHistory" />
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
    //*************************************************************************

    public VertexDrawingHistory
    (
        IVertex vertex,
        DrawingVisual drawingVisual,
        Boolean drawnAsSelected
    )
    : base(drawingVisual, drawnAsSelected)
    {
        m_oVertex = vertex;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: Vertex
    //
    /// <summary>
    /// Gets the vertex that was drawn.
    /// </summary>
    ///
    /// <value>
    /// The vertex that was drawn, as an <see cref="IVertex" />
    /// </value>
    //*************************************************************************

    public IVertex
    Vertex
    {
        get
        {
            AssertValid();

            return (m_oVertex);
        }
    }

    //*************************************************************************
    //  Property: VertexLocation
    //
    /// <summary>
    /// Gets the location of the vertex that was drawn.
    /// </summary>
    ///
    /// <value>
    /// The location of the vertex that was drawn, as a <see cref="Point" />.
    /// </value>
    //*************************************************************************

    public Point
    VertexLocation
    {
        get
        {
            AssertValid();

            return ( WpfGraphicsUtil.PointFToWpfPoint(m_oVertex.Location) );
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

    public abstract void
    GetEdgeEndpoint
    (
        VertexDrawingHistory otherVertexDrawingHistory,
        out Point edgeEndpoint
    );

    //*************************************************************************
    //  Method: GetSelfLoopEndpoint()
    //
    /// <summary>
    /// Gets the endpoint of an edge that is connected to <see cref="Vertex" />
    /// and is a self-loop.
    /// </summary>
    ///
    /// <param name="farthestGraphRectangleEdge">
    /// The edge of the graph rectangle that is farthest from <see
    /// cref="Vertex" />.
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

    public abstract Point
    GetSelfLoopEndpoint
    (
        RectangleEdge farthestGraphRectangleEdge
    );

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

    public abstract Point
    GetLabelLocation
    (
        VertexLabelPosition labelPosition
    );

    //*************************************************************************
    //  Method: GetEdgeEndpointOnCircle()
    //
    /// <summary>
    /// Gets an edge endpoint on a vertex that was drawn as a circle.
    /// vertex.
    /// </summary>
    ///
    /// <param name="oVertexALocation">
    /// Location of the edge vertex that was drawn as a circle.
    /// </param>
    ///
    /// <param name="dVertexARadius">
    /// The radius that was used to draw the vertex.
    /// </param>
    ///
    /// <param name="oVertexBLocation">
    /// Location of the edge's other vertex.
    /// </param>
    ///
    /// <param name="oEdgeEndpoint">
    /// Where the edge endpoint on the vertex A end of the edge gets stored.
    /// The endpoint is at the intersection of the line connecting the vertex
    /// locations and the circle that was drawn for VertexA.
    /// </param>
    //*************************************************************************

    protected void
    GetEdgeEndpointOnCircle
    (
        Point oVertexALocation,
        Double dVertexARadius,
        Point oVertexBLocation,
        out Point oEdgeEndpoint
    )
    {
        Debug.Assert(dVertexARadius >= 0);
        AssertValid();

        Double dEdgeAngle = WpfGraphicsUtil.GetAngleBetweenPoints(
            oVertexALocation, oVertexBLocation);

        oEdgeEndpoint = new Point(
            oVertexALocation.X + ( dVertexARadius * Math.Cos(dEdgeAngle) ),
            oVertexALocation.Y - ( dVertexARadius * Math.Sin(dEdgeAngle) )
            );
    }

    //*************************************************************************
    //  Method: GetEdgeEndpointOnRectangle()
    //
    /// <summary>
    /// Gets an edge endpoint on a vertex that was drawn as a rectangle.
    /// </summary>
    ///
    /// <param name="oVertexALocation">
    /// Location of the edge vertex that was drawn as a rectangle.
    /// </param>
    ///
    /// <param name="oVertexARectangle">
    /// The rectangle that was drawn for the vertex.
    /// </param>
    ///
    /// <param name="oVertexBLocation">
    /// Location of the edge's other vertex.
    /// </param>
    ///
    /// <param name="oEdgeEndpoint">
    /// Where the endpoint on the VertexA end of the edge gets stored.  The
    /// endpoint is at the intersection of the line connecting the vertex
    /// locations and <paramref name="oVertexARectangle" />.
    /// </param>
    //*************************************************************************

    protected void
    GetEdgeEndpointOnRectangle
    (
        Point oVertexALocation,
        Rect oVertexARectangle,
        Point oVertexBLocation,
        out Point oEdgeEndpoint
    )
    {
        AssertValid();

        if (oVertexALocation == oVertexBLocation)
        {
            oEdgeEndpoint = oVertexALocation;

            return;
        }

        Double dVertexAX = oVertexALocation.X;
        Double dVertexAY = oVertexALocation.Y;

        Double dVertexBX = oVertexBLocation.X;
        Double dVertexBY = oVertexBLocation.Y;

        Double dHalfVertexARectangleWidth = oVertexARectangle.Width / 2.0;
        Double dHalfVertexARectangleHeight = oVertexARectangle.Height / 2.0;

        // Get the angle between vertex A and vertex B.

        Double dEdgeAngle = WpfGraphicsUtil.GetAngleBetweenPoints(
            oVertexALocation, oVertexBLocation);

        // Get the angle that defines the aspect ratio of vertex A's rectangle.

        Double dAspectAngle = Math.Atan2(
            dHalfVertexARectangleHeight, dHalfVertexARectangleWidth);

        if (dEdgeAngle >= -dAspectAngle && dEdgeAngle < dAspectAngle)
        {
            // For a square, this is -45 degrees to 45 degrees.

            Debug.Assert(dVertexBX != dVertexAX);

            oEdgeEndpoint = new Point(

                dVertexAX + dHalfVertexARectangleWidth,

                dVertexAY + dHalfVertexARectangleWidth *
                    ( (dVertexBY - dVertexAY) / (dVertexBX - dVertexAX) )
                );

            return;
        }

        if (dEdgeAngle >= dAspectAngle && dEdgeAngle < Math.PI - dAspectAngle)
        {
            // For a square, this is 45 degrees to 135 degrees.

            Debug.Assert(dVertexBY != dVertexAY);

            oEdgeEndpoint = new Point(

                dVertexAX + dHalfVertexARectangleHeight *
                    ( (dVertexBX - dVertexAX) / (dVertexAY - dVertexBY) ),

                dVertexAY - dHalfVertexARectangleHeight
                );

                return;
        }

        if (dEdgeAngle < -dAspectAngle && dEdgeAngle >= -Math.PI + dAspectAngle)
        {
            // For a square, this is -45 degrees to -135 degrees.

            Debug.Assert(dVertexBY != dVertexAY);

            oEdgeEndpoint = new Point(

                dVertexAX + dHalfVertexARectangleHeight *
                    ( (dVertexBX - dVertexAX) / (dVertexBY - dVertexAY) ),

                dVertexAY + dHalfVertexARectangleHeight
                );

            return;
        }

        // For a square, this is 135 degrees to 180 degrees and -135 degrees to
        // -180 degrees.

        Debug.Assert(dVertexAX != dVertexBX);

        oEdgeEndpoint = new Point(

            dVertexAX - dHalfVertexARectangleWidth,

            dVertexAY + dHalfVertexARectangleWidth *
                ( (dVertexBY - dVertexAY) / (dVertexAX - dVertexBX) )
            );
    }

    //*************************************************************************
    //  Method: GetSelfLoopEndpointOnRectangle()
    //
    /// <summary>
    /// Gets a self-loop endpoint on a vertex that is drawn as a rectangle.
    /// </summary>
    ///
    /// <param name="oVertexRectangle">
    /// The rectangle used to draw the vertex.
    /// </param>
    ///
    /// <param name="eFarthestGraphRectangleEdge">
    /// The edge of the graph rectangle that is farthest from the vertex.
    /// </param>
    ///
    /// <returns>
    /// The self-loop endpoint.  The endpoint is at the center of one of the
    /// edges of <paramref name="oVertexRectangle" />.
    /// </returns>
    //*************************************************************************

    protected Point
    GetSelfLoopEndpointOnRectangle
    (
        Rect oVertexRectangle,
        RectangleEdge eFarthestGraphRectangleEdge
    )
    {
        AssertValid();

        Double dX = 0;
        Double dY = 0;

        switch (eFarthestGraphRectangleEdge)
        {
            case RectangleEdge.Top:

                dX = oVertexRectangle.Left + oVertexRectangle.Width / 2.0;
                dY = oVertexRectangle.Top;
                break;

            case RectangleEdge.Left:

                dX = oVertexRectangle.Left;
                dY = oVertexRectangle.Top + oVertexRectangle.Height / 2.0;
                break;

            case RectangleEdge.Right:

                dX = oVertexRectangle.Right;
                dY = oVertexRectangle.Top + oVertexRectangle.Height / 2.0;
                break;

            case RectangleEdge.Bottom:

                dX = oVertexRectangle.Left + oVertexRectangle.Width / 2.0;
                dY = oVertexRectangle.Bottom;
                break;

            default:

                Debug.Assert(false);
                break;
        }

        return ( new Point(dX, dY) );
    }

    //*************************************************************************
    //  Method: GetLabelLocationOnDiamond()
    //
    /// <summary>
    /// Gets the location at which an annotation label should be drawn on a
    /// diamond.
    /// </summary>
    ///
    /// <param name="eLabelPosition">
    /// The position of the annotation label.
    /// </param>
    ///
    /// <param name="dHalfWidth">
    /// The half-width of the diamond.
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
    ///
    /// <para>
    /// This is in the base class instead of the DiamondVertexDrawingHistory
    /// class because the diamond pattern for label locations is also used for
    /// circles.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected Point
    GetLabelLocationOnDiamond
    (
        VertexLabelPosition eLabelPosition,
        Double dHalfWidth
    )
    {
        Debug.Assert(dHalfWidth >= 0);
        AssertValid();

        Point oVertexLocation = this.VertexLocation;
        Double dVertexX = oVertexLocation.X;
        Double dVertexY = oVertexLocation.Y;

        switch (eLabelPosition)
        {
            case VertexLabelPosition.TopLeft:
            case VertexLabelPosition.TopCenter:
            case VertexLabelPosition.TopRight:

                return ( new Point(dVertexX, dVertexY - dHalfWidth) );

            case VertexLabelPosition.MiddleLeft:

                return ( new Point(dVertexX - dHalfWidth, dVertexY) );

            case VertexLabelPosition.MiddleCenter:

                return (oVertexLocation);

            case VertexLabelPosition.MiddleRight:

                return ( new Point(dVertexX + dHalfWidth, dVertexY) );

            case VertexLabelPosition.BottomLeft:
            case VertexLabelPosition.BottomCenter:
            case VertexLabelPosition.BottomRight:

                return ( new Point(dVertexX, dVertexY + dHalfWidth) );

            default:

                Debug.Assert(false);
                return (oVertexLocation);
        }
    }

    //*************************************************************************
    //  Method: GetBoundingSquare()
    //
    /// <summary>
    /// Gets a square centered on the vertex.
    /// </summary>
    ///
    /// <param name="dHalfWidth">
    /// Half the width of the square.
    /// </param>
    ///
    /// <returns>
    /// A rectangle with the specified half-width, centered on the vertex.
    /// </returns>
    //*************************************************************************

    protected Rect
    GetBoundingSquare
    (
        Double dHalfWidth
    )
    {
        AssertValid();

        Point oVertexLocation = this.VertexLocation;
        Double dWidth = 2.0 * dHalfWidth;

        return ( new Rect(
            oVertexLocation.X - dHalfWidth, oVertexLocation.Y - dHalfWidth,
            dWidth, dWidth) );
    }

    //*************************************************************************
    //  Method: GetEdgeAngle()
    //
    /// <summary>
    /// Gets the angle between two vertices.
    /// </summary>
    ///
    /// <param name="oVertexALocation">
    /// Location of the edge's first vertex.
    /// </param>
    ///
    /// <param name="oVertexBLocation">
    /// Location of the edge's second vertex.
    /// </param>
    ///
    /// <returns>
    /// The angle between the two vertices, in radians.  Ranges between 0 and
    /// PI (0 to 180 degrees) and 0 to -PI (0 to -180 degrees).
    /// </returns>
    //*************************************************************************

    protected Double
    GetEdgeAngle
    (
        Point oVertexALocation,
        Point oVertexBLocation
    )
    {
        AssertValid();

        return ( Math.Atan2(
            oVertexALocation.Y - oVertexBLocation.Y,
            oVertexBLocation.X - oVertexALocation.X
            ) );
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

        Debug.Assert(m_oVertex != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The vertex that was drawn.

    protected IVertex m_oVertex;
}

}
