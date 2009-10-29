
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: VertexLabelDrawer
//
/// <summary>
/// Draws a vertex label as an annotation.
/// </summary>
///
/// <remarks>
/// This class draws a label next to a vertex, as an annotation.  It does NOT
/// draw vertices that have the shape <see cref="VertexShape.Label" />.
/// </remarks>
//*****************************************************************************

public class VertexLabelDrawer : Object
{
    //*************************************************************************
    //  Constructor: VertexLabelDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexLabelDrawer" />
    /// class.
    /// </summary>
    ///
    /// <param name="labelPosition">
    /// The default position to use for vertex labels.
    /// </param>
    //*************************************************************************

    public VertexLabelDrawer
    (
        VertexLabelPosition labelPosition
    )
    {
        m_eLabelPosition = labelPosition;

        AssertValid();
    }

    //*************************************************************************
    //  Method: DrawLabel()
    //
    /// <summary>
    /// Draws a vertex label as an annotation.
    /// </summary>
    ///
    /// <param name="drawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="vertexDrawingHistory">
    /// Describes how the vertex was drawn.
    /// </param>
    ///
    /// <param name="vertexBounds">
    /// The vertex's bounding rectangle.
    /// </param>
    ///
    /// <param name="formattedText">
    /// The FormattedText object to use.  Several properties get changed by
    /// this method.
    /// </param>
    //*************************************************************************

    public void
    DrawLabel
    (
        DrawingContext drawingContext,
        GraphDrawingContext graphDrawingContext,
        VertexDrawingHistory vertexDrawingHistory,
        Rect vertexBounds,
        FormattedText formattedText
    )
    {
        Debug.Assert(drawingContext != null);
        Debug.Assert(graphDrawingContext != null);
        Debug.Assert(vertexDrawingHistory != null);
        Debug.Assert(formattedText != null);
        AssertValid();

        Double dHalfVertexBoundsWidth = vertexBounds.Width / 2.0;
        Double dHalfVertexBoundsHeight = vertexBounds.Height / 2.0;
        Double dLabelHeight = formattedText.Height;
        Double dHalfLabelHeight = dLabelHeight / 2.0;
        Double dLabelWidth = formattedText.Width;
        Double dHalfLabelWidth = dLabelWidth / 2.0;
        TextAlignment eTextAlignment = TextAlignment.Left;

        formattedText.MaxLineCount = 1;

        VertexLabelPosition eLabelPosition =
            GetLabelPosition(vertexDrawingHistory.Vertex);

        // This is the point where the label will be drawn.  It initially
        // assumes a text height of zero with no margin, but that will be
        // adjusted within the switch statement below.

        Point oDraw = vertexDrawingHistory.GetLabelLocation(eLabelPosition);
        Double dDrawX = oDraw.X;
        Double dDrawY = oDraw.Y;

        // These are the bounds of the label text.

        Double dLabelBoundsLeft = 0;
        Double dLabelBoundsRight = 0;

        switch (eLabelPosition)
        {
            case VertexLabelPosition.TopLeft:

                eTextAlignment = TextAlignment.Right;

                dDrawY -= (dLabelHeight + VerticalMargin);
                dLabelBoundsLeft = dDrawX - dLabelWidth;
                dLabelBoundsRight = dDrawX;

                break;

            case VertexLabelPosition.TopCenter:

                eTextAlignment = TextAlignment.Center;

                dDrawY -= (dLabelHeight + VerticalMargin);
                dLabelBoundsLeft = dDrawX - dHalfLabelWidth;
                dLabelBoundsRight = dDrawX + dHalfLabelWidth;

                break;

            case VertexLabelPosition.TopRight:

                // eTextAlignment = TextAlignment.Left;

                dDrawY -= (dLabelHeight + VerticalMargin);
                dLabelBoundsLeft = dDrawX;
                dLabelBoundsRight = dDrawX + dLabelWidth;

                break;

            case VertexLabelPosition.MiddleLeft:

                eTextAlignment = TextAlignment.Right;

                dDrawX -= HorizontalMargin;
                dDrawY -= dHalfLabelHeight;
                dLabelBoundsLeft = dDrawX - dLabelWidth;
                dLabelBoundsRight = dDrawX;

                break;

            case VertexLabelPosition.MiddleCenter:

                eTextAlignment = TextAlignment.Center;

                dDrawY -= dHalfLabelHeight;
                dLabelBoundsLeft = dDrawX - dHalfLabelWidth;
                dLabelBoundsRight = dDrawX + dHalfLabelWidth;

                break;

            case VertexLabelPosition.MiddleRight:

                // eTextAlignment = TextAlignment.Left;

                dDrawX += HorizontalMargin;
                dDrawY -= dHalfLabelHeight;
                dLabelBoundsLeft = dDrawX;
                dLabelBoundsRight = dDrawX + dLabelWidth;

                break;

            case VertexLabelPosition.BottomLeft:

                eTextAlignment = TextAlignment.Right;

                dDrawY += VerticalMargin;
                dLabelBoundsLeft = dDrawX - dLabelWidth;
                dLabelBoundsRight = dDrawX;

                break;

            case VertexLabelPosition.BottomCenter:

                eTextAlignment = TextAlignment.Center;

                dDrawY += VerticalMargin;
                dLabelBoundsLeft = dDrawX - dHalfLabelWidth;
                dLabelBoundsRight = dDrawX + dHalfLabelWidth;

                break;

            case VertexLabelPosition.BottomRight:

                // eTextAlignment = TextAlignment.Left;

                dDrawY += VerticalMargin;
                dLabelBoundsLeft = dDrawX;
                dLabelBoundsRight = dDrawX + dLabelWidth;

                break;

            default:

                Debug.Assert(false);
                break;
        }

        // Don't let the text exceed the bounds of the graph rectangle.

        Double dLabelBoundsTop = dDrawY;
        Double dLabelBoundsBottom = dDrawY + dLabelHeight;

        Rect oGraphRectangleMinusMargin =
            graphDrawingContext.GraphRectangleMinusMargin;

        dDrawX += Math.Max(0,
            oGraphRectangleMinusMargin.Left - dLabelBoundsLeft);

        dDrawX -= Math.Max(0,
            dLabelBoundsRight - oGraphRectangleMinusMargin.Right);

        dDrawY += Math.Max(0,
            oGraphRectangleMinusMargin.Top - dLabelBoundsTop);

        dDrawY -= Math.Max(0,
            dLabelBoundsBottom - oGraphRectangleMinusMargin.Bottom);

        formattedText.TextAlignment = eTextAlignment;

        drawingContext.DrawText( formattedText, new Point(dDrawX, dDrawY) );
    }

    //*************************************************************************
    //  Method: GetLabelPosition()
    //
    /// <summary>
    /// Gets the position of a vertex label.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to get the label position for.
    /// </param>
    ///
    /// <returns>
    /// The vertex's label position.
    /// </returns>
    //*************************************************************************

    protected VertexLabelPosition
    GetLabelPosition
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        // Start with the default position.

        VertexLabelPosition eLabelPosition = m_eLabelPosition;

        // Check for a per-vertex label position.

        Object oPerVertexLabelPositionAsObject;

        if ( oVertex.TryGetValue(ReservedMetadataKeys.PerVertexLabelPosition,
            typeof(VertexLabelPosition), out oPerVertexLabelPositionAsObject) )
        {
            eLabelPosition =
                (VertexLabelPosition)oPerVertexLabelPositionAsObject;
        }

        return (eLabelPosition);
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        // m_eLabelPosition
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Horizontal margin used between the vertex bounds and the label in some
    /// cases.

    protected const Double HorizontalMargin = 3;

    /// Vertical margin used between the vertex bounds and the label in some
    /// cases.

    protected const Double VerticalMargin = 2;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Default position of vertex labels drawn as annotations.

    protected VertexLabelPosition m_eLabelPosition;
}
}
