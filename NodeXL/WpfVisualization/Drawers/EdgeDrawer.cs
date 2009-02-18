
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: EdgeDrawer
//
/// <summary>
/// Draws a graph's edges.
/// </summary>
///
/// <remarks>
/// This class is responsible for drawing a graph's edges.  The default edge
/// appearance is determined by this class's properties.  The appearance of an
/// individual edge can be overridden by adding appropriate metadata to the
/// edge.  The following metadata keys are honored:
///
/// <list type="bullet">
///
/// <item><see cref="ReservedMetadataKeys.Visibility" /></item>
/// <item><see cref="ReservedMetadataKeys.PerColor" /></item>
/// <item><see cref="ReservedMetadataKeys.PerAlpha" /></item>
/// <item><see cref="ReservedMetadataKeys.PerEdgeWidth" /></item>
/// <item><see cref="ReservedMetadataKeys.IsSelected" /></item>
///
/// </list>
///
/// <para>
/// The values of the <see cref="ReservedMetadataKeys.PerColor" /> key can be
/// of type System.Windows.Media.Color or System.Drawing.Color.
/// </para>
///
/// <para>
/// If <see cref="VertexAndEdgeDrawerBase.UseSelection" /> is true, an edge is
/// drawn using either <see cref="VertexAndEdgeDrawerBase.Color" /> and <see
/// cref="Width" /> or <see cref="VertexAndEdgeDrawerBase.SelectedColor" />
/// and <see cref="SelectedWidth" />, depending on whether the edge has been
/// marked as selected.  If <see
/// cref="VertexAndEdgeDrawerBase.UseSelection" /> is false, <see
/// cref="VertexAndEdgeDrawerBase.Color" /> and <see cref="Width" /> are used
/// regardless of whether the edge has been marked as selected.
/// </para>
///
/// <para>
/// When drawing the graph, call <see cref="TryDrawEdge" /> for each of the
/// graph's edges.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class EdgeDrawer : VertexAndEdgeDrawerBase
{
    //*************************************************************************
    //  Constructor: EdgeDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeDrawer" /> class.
    /// </summary>
    //*************************************************************************

    public EdgeDrawer()
    {
        m_dWidth = 1;
        m_dSelectedWidth = 2;
        m_bDrawArrowOnDirectedEdge = true;
        m_dRelativeArrowSize = 3.0;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Width
    //
    /// <summary>
    /// Gets or sets the default width of unselected edges.
    /// </summary>
    ///
    /// <value>
    /// The default width of unselected edges, as a <see cref="Double" />.
    /// Must be between <see cref="MinimumWidth" /> and <see
    /// cref="MaximumWidth" />, inclusive.  The default value is 1.
    /// </value>
    ///
    /// <remarks>
    /// The default width of an unselected edge can be overridden by setting
    /// the <see cref="ReservedMetadataKeys.PerEdgeWidth" /> key on the edge.
    /// </remarks>
    ///
    /// <seealso cref="SelectedWidth" />
    //*************************************************************************

    public Double
    Width
    {
        get
        {
            AssertValid();

            return (m_dWidth);
        }

        set
        {
            const String PropertyName = "Width";

            SetWidthOrSelectedWidthProperty(value, PropertyName, ref m_dWidth);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SelectedWidth
    //
    /// <summary>
    /// Gets or sets the width of selected edges.
    /// </summary>
    ///
    /// <value>
    /// The width of selected edges, as a <see cref="Double" />.  Must be
    /// between <see cref="MinimumWidth" /> and <see cref="MaximumWidth" />,
    /// inclusive.  The default value is 2.
    /// </value>
    ///
    /// <remarks>
    /// The width of selected edges cannot be set on a per-edge basis.
    /// </remarks>
    ///
    /// <seealso cref="Width" />
    //*************************************************************************

    public Double
    SelectedWidth
    {
        get
        {
            AssertValid();

            return (m_dSelectedWidth);
        }

        set
        {
            const String PropertyName = "SelectedWidth";

            SetWidthOrSelectedWidthProperty(value, PropertyName,
                ref m_dSelectedWidth);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DrawArrowOnDirectedEdge
    //
    /// <summary>
    /// Gets or sets a flag that determines whether an arrow should be drawn on
    /// directed edges.
    /// </summary>
    ///
    /// <value>
    /// true to draw an arrow on directed edges, false otherwise.  The default
    /// value is true.
    /// </value>
    ///
    /// <remarks>
    /// By default, an edge with <see cref="IEdge.IsDirected" /> set to true is
    /// drawn with an arrow pointing to the front vertex.  If this property is
    /// set to false, the arrow is not drawn.
    /// </remarks>
    //*************************************************************************

    public Boolean
    DrawArrowOnDirectedEdge
    {
        get
        {
            AssertValid();

            return (m_bDrawArrowOnDirectedEdge);
        }

        set
        {
            if (m_bDrawArrowOnDirectedEdge == value)
            {
                return;
            }

            m_bDrawArrowOnDirectedEdge = value;

            FireRedrawRequired();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: RelativeArrowSize
    //
    /// <summary>
    /// Gets or sets the relative size of arrowheads on directed edges.
    /// </summary>
    ///
    /// <value>
    /// The relative size of arrowheads, as a <see cref="Double" />.  Must be
    /// between <see cref="MinimumRelativeArrowSize" /> and <see
    /// cref="MaximumRelativeArrowSize" />, inclusive.  The default value is 3.
    /// </value>
    ///
    /// <remarks>
    /// The value is relative to <see cref="Width" /> and <see
    /// cref="SelectedWidth" />.  If the width or selected width is increased,
    /// the arrow size on unselected or selected edges is increased
    /// proportionally.
    /// </remarks>
    //*************************************************************************

    public Double
    RelativeArrowSize
    {
        get
        {
            AssertValid();

            return (m_dRelativeArrowSize);
        }

        set
        {
            const String PropertyName = "RelativeArrowSize";

            this.ArgumentChecker.CheckPropertyInRange(PropertyName, value,
                MinimumRelativeArrowSize, MaximumRelativeArrowSize);

            if (m_dRelativeArrowSize == value)
            {
                return;
            }

            m_dRelativeArrowSize = value;

            FireRedrawRequired();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: TryDrawEdge()
    //
    /// <summary>
    /// Draws an edge.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to draw.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="edgeDrawingHistory">
    /// Where an <see cref="EdgeDrawingHistory" /> object that retains
    /// information about how the edge was drawn gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if the edge was drawn, false if the edge is hidden.
    /// </returns>
    ///
    /// <remarks>
    /// This method should be called repeatedly while a graph is being drawn,
    /// once for each of the graph's edges.  The <see
    /// cref="IVertex.Location" /> property on all of the graph's vertices must
    /// be set by ILayout.LayOutGraph before this method is called.
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryDrawEdge
    (
        IEdge edge,
        GraphDrawingContext graphDrawingContext,
        out EdgeDrawingHistory edgeDrawingHistory
    )
    {
        Debug.Assert(edge != null);
        Debug.Assert(graphDrawingContext != null);
        AssertValid();

        edgeDrawingHistory = null;

        // If the edge is hidden, do nothing.

        VisibilityKeyValue eVisibility = GetVisibility(edge);

        if (eVisibility == VisibilityKeyValue.Hidden)
        {
            return (false);
        }

        Boolean bDrawAsSelected = GetDrawAsSelected(edge);
        Color oColor = GetColor(edge, eVisibility, bDrawAsSelected);
        Double dWidth = GetWidth(edge, bDrawAsSelected);
        Boolean bDrawArrow = (m_bDrawArrowOnDirectedEdge && edge.IsDirected);
        IVertex [] aoVertices = edge.Vertices;

        DrawingVisual oDrawingVisual = new DrawingVisual();

        using ( DrawingContext oDrawingContext = oDrawingVisual.RenderOpen() )
        {
            if (edge.IsSelfLoop)
            {
                if ( !TryDrawSelfLoop(aoVertices[0], oDrawingContext,
                    graphDrawingContext, oColor, dWidth, bDrawArrow) )
                {
                    // The edge's vertex is hidden, so the edge should be
                    // hidden also.

                    return (false);
                }
            }
            else
            {
                Point oEdgeEndpoint1, oEdgeEndpoint2;

                if ( !TryGetEdgeEndpoints(aoVertices[0], aoVertices[1],
                    graphDrawingContext, out oEdgeEndpoint1,
                    out oEdgeEndpoint2) )
                {
                    // One of the edge's vertices is hidden, so the edge should
                    // be hidden also.

                    return (false);
                }

                if (bDrawArrow)
                {
                    // Draw the arrow and set the second endpoint to the center
                    // of the flat end of the arrow.

                    Double dArrowAngle = WpfGraphicsUtil.GetAngleBetweenPoints(
                        oEdgeEndpoint1, oEdgeEndpoint2);

                    oEdgeEndpoint2 = DrawArrow(oDrawingContext, oEdgeEndpoint2,
                        dArrowAngle, oColor, dWidth);
                }

                // Draw the edge.

                oDrawingContext.DrawLine(GetPen(oColor, dWidth),
                    oEdgeEndpoint1, oEdgeEndpoint2);
            }

            // Retain information about the edge that was drawn.

            edgeDrawingHistory = new EdgeDrawingHistory(
                edge, oDrawingVisual, bDrawAsSelected);

            return (true);
        }
    }

    //*************************************************************************
    //  Method: SetWidthOrSelectedWidthProperty
    //
    /// <summary>
    /// Sets the value of the <see cref="Width" /> or <see
    /// cref="SelectedWidth" /> property.
    /// </summary>
    ///
    /// <param name="dNewPropertyValue">
    /// The new property value.
    /// </param>
    ///
    /// <param name="sPropertyName">
    /// The name of the property, either "Width" or "SelectedWidth".
    /// </param>
    ///
    /// <param name="dProperty">
    /// The field for the property value.
    /// </param>
    //*************************************************************************

    protected void
    SetWidthOrSelectedWidthProperty
    (
        Double dNewPropertyValue,
        String sPropertyName,
        ref Double dProperty
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sPropertyName) );
        AssertValid();

        this.ArgumentChecker.CheckPropertyInRange(
            sPropertyName, dNewPropertyValue, MinimumWidth, MaximumWidth);

        if (dProperty == dNewPropertyValue)
        {
            return;
        }

        dProperty = dNewPropertyValue;

        FireRedrawRequired();

        AssertValid();
    }

    //*************************************************************************
    //  Method: TryDrawSelfLoop()
    //
    /// <summary>
    /// Attempts to draw an edge that is a self-loop.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to connect to itself.
    /// </param>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="oColor">
    /// The edge color.
    /// </param>
    ///
    /// <param name="dWidth">
    /// The edge width.
    /// </param>
    ///
    /// <param name="bDrawArrow">
    /// true if an arrow should be drawn on the self-loop.
    /// </param>
    ///
    /// <returns>
    /// true if the self loop was drawn, false if the edge's vertex is hidden.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryDrawSelfLoop
    (
        IVertex oVertex,
        DrawingContext oDrawingContext,
        GraphDrawingContext oGraphDrawingContext,
        Color oColor,
        Double dWidth,
        Boolean bDrawArrow
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(dWidth >= 0);
        AssertValid();

        // Retrieve the information about how the vertex was drawn.

        VertexDrawingHistory oVertexDrawingHistory;

        if ( !oGraphDrawingContext.VertexDrawingHistories.TryGetValue(
            oVertex.ID, out oVertexDrawingHistory) )
        {
            // The edge's vertex is hidden, so the edge should be hidden also.

            return (false);
        }

        // Determine the edge of the graph rectangle that is farthest from the
        // vertex.

        Point oVertexLocation = oVertexDrawingHistory.VertexLocation;

        RectangleEdge eFarthestGraphRectangleEdge =
            WpfGraphicsUtil.GetFarthestRectangleEdge(oVertexLocation,
            oGraphDrawingContext.GraphRectangle);

        // Get the point on the vertex at which to draw the self-loop.

        Point oSelfLoopEndpoint = oVertexDrawingHistory.GetSelfLoopEndpoint(
            eFarthestGraphRectangleEdge);

        DrawSelfLoopAt(oDrawingContext, oGraphDrawingContext, oColor, dWidth,
            oSelfLoopEndpoint, eFarthestGraphRectangleEdge, bDrawArrow);

        return (true);
    }

    //*************************************************************************
    //  Method: DrawSelfLoopAt()
    //
    /// <summary>
    /// Draws an edge that is a self-loop at a specified endpoint.
    /// </summary>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="oColor">
    /// The edge color.
    /// </param>
    ///
    /// <param name="dWidth">
    /// The edge width.
    /// </param>
    ///
    /// <param name="oSelfLoopEndpoint">
    /// The point on the vertex at which to draw the self-loop.
    /// </param>
    ///
    /// <param name="eFarthestGraphRectangleEdge">
    /// The edge of the graph rectangle that is farthest from the vertex.
    /// </param>
    ///
    /// <param name="bDrawArrow">
    /// true if an arrow should be drawn on the self-loop.
    /// </param>
    //*************************************************************************

    protected void
    DrawSelfLoopAt
    (
        DrawingContext oDrawingContext,
        GraphDrawingContext oGraphDrawingContext,
        Color oColor,
        Double dWidth,
        Point oSelfLoopEndpoint,
        RectangleEdge eFarthestGraphRectangleEdge,
        Boolean bDrawArrow
    )
    {
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(dWidth >= 0);
        AssertValid();

        // The self-loop is drawn as a circle.  Figure out the location of the
        // circle's center and the tip of the arrow, if there is an arrow.

        Double dCircleX, dCircleY, dArrowTipX, dArrowTipY, dArrowAngle;

        dCircleX = dArrowTipX = oSelfLoopEndpoint.X;
        dCircleY = dArrowTipY = oSelfLoopEndpoint.Y;
        Double dSelfLoopCircleDiameter = 2.0 * SelfLoopCircleRadius;
        dArrowAngle = 0;

        switch (eFarthestGraphRectangleEdge)
        {
            case RectangleEdge.Top:

                dCircleY -= SelfLoopCircleRadius;
                dArrowTipY -= dSelfLoopCircleDiameter;
                break;

            case RectangleEdge.Left:

                dCircleX -= SelfLoopCircleRadius;
                dArrowTipX -= dSelfLoopCircleDiameter;
                dArrowAngle = Math.PI / 2.0;  // (90 degrees.)
                break;

            case RectangleEdge.Right:

                dCircleX += SelfLoopCircleRadius;
                dArrowTipX += dSelfLoopCircleDiameter;
                dArrowAngle = -Math.PI / 2.0;  // (-90 degrees.)
                break;

            case RectangleEdge.Bottom:

                dCircleY += SelfLoopCircleRadius;
                dArrowTipY += dSelfLoopCircleDiameter;
                dArrowAngle = Math.PI;  // (180 degrees.)
                break;

            default:

                Debug.Assert(false);
                break;
        }

        oDrawingContext.DrawEllipse(null, GetPen(oColor, dWidth),
            new Point(dCircleX, dCircleY), SelfLoopCircleRadius,
            SelfLoopCircleRadius);

        if (bDrawArrow)
        {
            // Rotate the arrow slightly to adjust to the circular shape of the
            // edge connected to it.

            dArrowAngle += Math.PI / 13.0;

            DrawArrow(oDrawingContext, new Point(dArrowTipX, dArrowTipY),
                dArrowAngle, oColor, dWidth);
        }
    }

    //*************************************************************************
    //  Method: GetWidth()
    //
    /// <summary>
    /// Gets the width of an edge.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to get the width for.
    /// </param>
    ///
    /// <param name="bDrawAsSelected">
    /// true to draw the edge as selected.
    /// </param>
    ///
    /// <returns>
    /// The width of the edge.
    /// </returns>
    //*************************************************************************

    protected Double
    GetWidth
    (
        IEdge oEdge,
        Boolean bDrawAsSelected
    )
    {
        Debug.Assert(oEdge != null);
        AssertValid();

        if (bDrawAsSelected)
        {
            return (m_dSelectedWidth);
        }

        // Start with the default width.

        Double dWidth = m_dWidth;

        Object oPerEdgeWidthAsObject;

        // Check for a per-edge width.  Note that the width is stored as a
        // Single in the edge's metadata to reduce memory usage.

        if ( oEdge.TryGetValue(ReservedMetadataKeys.PerEdgeWidth,
            typeof(Single), out oPerEdgeWidthAsObject) )
        {
            dWidth = (Double)(Single)oPerEdgeWidthAsObject;

            if (dWidth < MinimumWidth || dWidth > MaximumWidth)
            {
                throw new FormatException( String.Format(

                    "{0}: The edge with the ID {1} has an out-of-range {2}"
                    + " value.  Valid values are between {3} and {4}."
                    ,
                    this.ClassName,
                    oEdge.ID,
                    "ReservedMetadataKeys.PerEdgeWidth",
                    MinimumWidth,
                    MaximumWidth
                    ) );
            }
        }

        return (dWidth);
    }

    //*************************************************************************
    //  Method: TryGetEdgeEndpoints()
    //
    /// <summary>
    /// Attempts to get the endpoints of the edge.
    /// </summary>
    ///
    /// <param name="oVertex1">
    /// The edge's first vertex.
    /// </param>
    ///
    /// <param name="oVertex2">
    /// The edge's second vertex.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="oEdgeEndpoint1">
    /// Where the edge's first endpoint gets stored if true is returned.
    /// </param>
    ///
    /// <param name="oEdgeEndpoint2">
    /// Where the edge's second endpoint gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the endpoints were obtained, false if one of the edge's
    /// vertices is hidden.
    /// </returns>
    ///
    /// <remarks>
    /// The edge's first endpoint is the endpoint on the <paramref
    /// name="oVertex1" /> side of the edge.  The edge's second endpoint is the
    /// endpoint on the <paramref name="oVertex2" /> side of the edge.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryGetEdgeEndpoints
    (
        IVertex oVertex1,
        IVertex oVertex2,
        GraphDrawingContext oGraphDrawingContext,
        out Point oEdgeEndpoint1,
        out Point oEdgeEndpoint2
    )
    {
        Debug.Assert(oVertex1 != null);
        Debug.Assert(oVertex2 != null);
        Debug.Assert(oGraphDrawingContext != null);
        AssertValid();

        oEdgeEndpoint1 = new Point();
        oEdgeEndpoint2 = new Point();

        // Retrieve the information about how the vertices were drawn.

        Dictionary<Int32, VertexDrawingHistory> oVertexDrawingHistories =
            oGraphDrawingContext.VertexDrawingHistories;

        VertexDrawingHistory oVertex1DrawingHistory, oVertex2DrawingHistory;

        if (
            !oVertexDrawingHistories.TryGetValue(oVertex1.ID,
                out oVertex1DrawingHistory)
            ||
            !oVertexDrawingHistories.TryGetValue(oVertex2.ID,
                out oVertex2DrawingHistory)
            )
        {
            // One of the edge's vertices is hidden.

            return (false);
        }

        // The drawing histories determine the edge endpoints.  For example, if
        // oVertex1 was drawn as a circle, then oVertex1DrawingHistory is a
        // CircleVertexDrawingHistory that knows to put its endpoint on the
        // circle itself and not at the circle's center.

        oVertex1DrawingHistory.GetEdgeEndpoint(oVertex2DrawingHistory,
            out oEdgeEndpoint1);

        oVertex2DrawingHistory.GetEdgeEndpoint(oVertex1DrawingHistory,
            out oEdgeEndpoint2);

        return (true);
    }

    //*************************************************************************
    //  Method: DrawArrow()
    //
    /// <summary>
    /// Draws an arrow whose tip is at a specified point.
    /// </summary>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="oArrowTipLocation">
    /// Where the tip of the arrow gets drawn.
    /// </param>
    ///
    /// <param name="dArrowAngle">
    /// The angle of the arrow.  Ranges between 0 and PI radians (0 to 180
    /// degrees) and 0 to -PI radians (0 to -180 degrees).  If 0, the arrow
    /// points to the right.
    /// </param>
    ///
    /// <param name="oColor">
    /// The color of the arrow.
    /// </param>
    ///
    /// <param name="dEdgeWidth">
    /// The width of the edge that will connect to the arrow.
    /// </param>
    ///
    /// <returns>
    /// The point at the center of the flat end of the arrow.  This can be used
    /// when drawing the line that connects to the arrow.  (Don't draw a line
    /// to <paramref name="oArrowTipLocation" />, because the line's endcap
    /// will overlap the tip of the arrow.)
    /// </returns>
    //*************************************************************************

    protected Point
    DrawArrow
    (
        DrawingContext oDrawingContext,
        Point oArrowTipLocation,
        Double dArrowAngle,
        Color oColor,
        Double dEdgeWidth
    )
    {
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(dEdgeWidth > 0);
        AssertValid();

        // Compute the arrow's dimensions.  The width factor is arbitrary and
        // was determined experimentally.

        const Double WidthFactor = 1.5;

        Double dArrowTipX = oArrowTipLocation.X;
        Double dArrowTipY = oArrowTipLocation.Y;
        Double dArrowWidth = WidthFactor * dEdgeWidth * m_dRelativeArrowSize;
        Double dArrowHalfHeight = dArrowWidth / 2.0;
        Double dX = dArrowTipX - dArrowWidth;

        // Compute the arrow's three points as if the arrow were at an angle of
        // zero degrees, then use a rotated transform to adjust for the actual
        // specified angle.

        Point [] aoPoints = new Point [] {

            // Index 0: Arrow tip.

            oArrowTipLocation,

            // Index 1: Arrow bottom.

            new Point(dX, dArrowTipY - dArrowHalfHeight),

            // Index 2: Arrow top.

            new Point(dX, dArrowTipY + dArrowHalfHeight),

            // Index 3: Center of the flat end of the arrow.
            //
            // Note: The 0.2 is to avoid a gap between the edge endcap and the
            // flat end of the arrow, but it sometimes causes the two to
            // overlap slightly, and that can show if the edge isn't opaque.
            // What is the correct way to get the endcap to merge invisibly
            // with the arrow?

            new Point(dX + 0.2, dArrowTipY)
            };

        Matrix oMatrix = WpfGraphicsUtil.GetRotatedMatrix( oArrowTipLocation,
            -MathUtil.RadiansToDegrees(dArrowAngle) );

        oMatrix.Transform(aoPoints);

        PathGeometry oArrow = WpfGraphicsUtil.PathGeometryFromPoints(
            aoPoints[0], aoPoints[1], aoPoints[2] );

        oDrawingContext.DrawGeometry(GetBrush(oColor), null, oArrow);

        return ( aoPoints[3] );
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

        Debug.Assert(m_dWidth >= MinimumWidth);
        Debug.Assert(m_dWidth <= MaximumWidth);

        Debug.Assert(m_dSelectedWidth >= MinimumWidth);
        Debug.Assert(m_dSelectedWidth <= MaximumWidth);

        // m_bDrawArrowOnDirectedEdge

        Debug.Assert(m_dRelativeArrowSize >= MinimumRelativeArrowSize);
        Debug.Assert(m_dRelativeArrowSize <= MaximumRelativeArrowSize);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// Minimum value of the <see cref="Width" /> and <see
    /// cref="SelectedWidth" /> properties.  The value is 1.
    /// </summary>

    public static Double MinimumWidth = 1;

    /// <summary>
    /// Maximum value of the <see cref="Width" /> and <see
    /// cref="SelectedWidth" /> properties.  The value is 20.
    /// </summary>

    public static Double MaximumWidth = 20;

    /// <summary>
    /// Minimum value of the <see cref="RelativeArrowSize" /> property.  The
    /// value is 0.
    /// </summary>

    public static Double MinimumRelativeArrowSize = 0;

    /// <summary>
    /// Maximum value of the <see cref="RelativeArrowSize" /> property.  The
    /// value is 20.
    /// </summary>

    public static Double MaximumRelativeArrowSize = 20;


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Radius of a self-loop edge, which is drawn as a circle.

    protected const Double SelfLoopCircleRadius = 10;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Width of an unselected edge.

    protected Double m_dWidth;

    /// Width of a selected edge.

    protected Double m_dSelectedWidth;

    /// true to draw an arrow on directed edges.

    protected Boolean m_bDrawArrowOnDirectedEdge;

    /// Width and height of arrows, relative to the pen width.

    protected Double m_dRelativeArrowSize;
}

}