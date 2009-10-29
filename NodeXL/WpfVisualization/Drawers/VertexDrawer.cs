
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.WpfGraphicsLib;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: VertexDrawer
//
/// <summary>
/// Draws a graph's vertices.
/// </summary>
///
/// <remarks>
/// This class is responsible for drawing a graph's vertices.  The default
/// vertex appearance is determined by this class's properties.  The
/// appearance of an individual vertex can be overridden by adding appropriate
/// metadata to the vertex.  The following metadata keys are honored:
///
/// <list type="bullet">
///
/// <item><see cref="ReservedMetadataKeys.Visibility" /></item>
/// <item><see cref="ReservedMetadataKeys.IsSelected" /></item>
/// <item><see cref="ReservedMetadataKeys.PerColor" /></item>
/// <item><see cref="ReservedMetadataKeys.PerAlpha" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexShape" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexRadius" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexLabel" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexLabelFillColor" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexLabelPosition" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexImage" /></item>
///
/// </list>
///
/// <para>
/// If <see cref="VertexAndEdgeDrawerBase.UseSelection" /> is true, a vertex is
/// drawn using <see cref="VertexAndEdgeDrawerBase.Color" /> or <see
/// cref="VertexAndEdgeDrawerBase.SelectedColor" />, depending on whether the
/// vertex has been marked as selected.  If <see
/// cref="VertexAndEdgeDrawerBase.UseSelection" /> is false, <see
/// cref="VertexAndEdgeDrawerBase.Color" /> is used regardless of whether the
/// vertex has been marked as selected.
/// </para>
///
/// <para>
/// If a vertex has the shape <see cref="VertexShape.Label" /> and has the
/// <see cref="ReservedMetadataKeys.PerVertexLabel" /> key, the vertex is drawn
/// as a rectangle containing the text specified by the <see
/// cref="ReservedMetadataKeys.PerVertexLabel" /> key's value.  The default
/// color of the text and the rectangle's outline is <see
/// cref="VertexAndEdgeDrawerBase.Color" />, but can be overridden with the
/// <see cref="ReservedMetadataKeys.PerColor" /> key.  The default fill color
/// of the rectangle is <see cref="LabelFillColor" />, but can be overridden
/// with the <see cref="ReservedMetadataKeys.PerVertexLabelFillColor" /> key.
/// </para>
///
/// <para>
/// If a vertex has a shape other than <see cref="VertexShape.Label" /> and
/// has the <see cref="ReservedMetadataKeys.PerVertexLabel" /> key, the vertex
/// is drawn as the specified shape, and the text specified by the <see
/// cref="ReservedMetadataKeys.PerVertexLabel" /> key's value is drawn next to
/// the vertex as an annotation at the position determined by <see
/// cref="VertexDrawer.LabelPosition" />.
/// </para>
///
/// <para>
/// If a vertex has the shape <see cref="VertexShape.Image" /> and has the <see
/// cref="ReservedMetadataKeys.PerVertexImage" /> key, the vertex is drawn as
/// the image specified by the <see
/// cref="ReservedMetadataKeys.PerVertexImage" /> key's value.
/// </para>
///
/// <para>
/// The values of the <see cref="ReservedMetadataKeys.PerColor" /> and <see
/// cref="ReservedMetadataKeys.PerVertexLabelFillColor" /> keys can be of type
/// System.Windows.Media.Color or System.Drawing.Color.
/// </para>
///
/// <para>
/// When drawing the graph, call <see cref="TryDrawVertex" /> for each of the
/// graph's vertices.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class VertexDrawer : VertexAndEdgeDrawerBase
{
    //*************************************************************************
    //  Constructor: VertexDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexDrawer" /> class.
    /// </summary>
    //*************************************************************************

    public VertexDrawer()
    {
        m_eShape = VertexShape.Disk;
        m_dRadius = 3.0;
        m_oLabelFillColor = SystemColors.WindowColor;
        m_eLabelPosition = VertexLabelPosition.TopRight;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Shape
    //
    /// <summary>
    /// Gets or sets the default shape of the vertices.
    /// </summary>
    ///
    /// <value>
    /// The default shape of the vertices, as a <see cref="VertexShape" />.
    /// The default value is <see cref="VertexShape.Disk" />.
    /// </value>
    ///
    /// <remarks>
    /// The default shape of a vertex can be overridden by setting the <see
    /// cref="ReservedMetadataKeys.PerVertexShape" /> key on the vertex.
    /// </remarks>
    //*************************************************************************

    public VertexShape
    Shape
    {
        get
        {
            AssertValid();

            return (m_eShape);
        }

        set
        {
            const String PropertyName = "Shape";

            this.ArgumentChecker.CheckPropertyIsDefined(
                PropertyName, value, typeof(VertexShape) );

            if (m_eShape == value)
            {
                return;
            }

            m_eShape = value;

            FireRedrawRequired();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Radius
    //
    /// <summary>
    /// Gets or sets the default radius of the vertices.
    /// </summary>
    ///
    /// <value>
    /// The default radius of the vertices, as a <see cref="Double" />.  Must
    /// be between <see cref="MinimumRadius" /> and <see
    /// cref="MaximumRadius" />, inclusive.  The default value is 3.0.
    /// </value>
    ///
    /// <remarks>
    /// The default radius of a vertex can be overridden by setting the <see
    /// cref="ReservedMetadataKeys.PerVertexRadius" /> key on the vertex.
    /// </remarks>
    //*************************************************************************

    public Double
    Radius
    {
        get
        {
            AssertValid();

            return (m_dRadius);
        }

        set
        {
            const String PropertyName = "Radius";

            this.ArgumentChecker.CheckPropertyInRange(PropertyName, value,
                MinimumRadius, MaximumRadius);

            if (m_dRadius == value)
            {
                return;
            }

            m_dRadius = value;

            // The layout may depend on the vertex size, so fire
            // LayoutRequired, not RedrawRequired.

            FireLayoutRequired();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: LabelFillColor
    //
    /// <summary>
    /// Gets or sets the default fill color to use for a vertex that has the
    /// shape Label.
    /// </summary>
    ///
    /// <value>
    /// The default fill color to use for labels.  The default is
    /// SystemColors.WindowColor.
    /// </value>
    ///
    /// <remarks>
    /// <see cref="Color" /> is used for the label text and outline.
    ///
    /// <para>
    /// The default fill color of a vertex can be overridden by setting the
    /// <see cref="ReservedMetadataKeys.PerVertexLabelFillColor" /> key on the
    /// vertex.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Color
    LabelFillColor
    {
        get
        {
            AssertValid();

            return (m_oLabelFillColor);
        }

        set
        {
            if (m_oLabelFillColor == value)
            {
                return;
            }

            m_oLabelFillColor = value;

            FireRedrawRequired();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: LabelPosition
    //
    /// <summary>
    /// Gets or sets the default position of a vertex label drawn as an
    /// annotation.
    /// </summary>
    ///
    /// <value>
    /// The default position of a vertex label drawn as an annotation.  The
    /// default is <see cref="VertexLabelPosition.TopRight" />.
    /// </value>
    ///
    /// <remarks>
    /// This property is not used when drawing vertices that have the shape
    /// <see cref="VertexShape.Label" />.
    /// </remarks>
    //*************************************************************************

    public VertexLabelPosition
    LabelPosition
    {
        get
        {
            AssertValid();

            return (m_eLabelPosition);
        }

        set
        {
            if (m_eLabelPosition == value)
            {
                return;
            }

            m_eLabelPosition = value;

            FireRedrawRequired();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: TryDrawVertex()
    //
    /// <summary>
    /// Draws a vertex after moving it if necessary.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to draw.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="vertexDrawingHistory">
    /// Where a <see cref="VertexDrawingHistory" /> object that retains
    /// information about how the vertex was drawn gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if the vertex was drawn, false if the vertex is hidden.
    /// </returns>
    ///
    /// <remarks>
    /// This method should be called repeatedly while a graph is being drawn,
    /// once for each of the graph's vertices.  The <see
    /// cref="IVertex.Location" /> property on all of the graph's vertices must
    /// be set by ILayout.LayOutGraph before this method is called.
    ///
    /// <para>
    /// If the vertex falls outside the graph rectangle, it gets moved before
    /// being drawn.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryDrawVertex
    (
        IVertex vertex,
        GraphDrawingContext graphDrawingContext,
        out VertexDrawingHistory vertexDrawingHistory
    )
    {
        AssertValid();

        vertexDrawingHistory = null;

        CheckDrawVertexArguments(vertex, graphDrawingContext);

        // If the vertex is hidden, do nothing.

        VisibilityKeyValue eVisibility = GetVisibility(vertex);

        if (eVisibility == VisibilityKeyValue.Hidden)
        {
            return (false);
        }

        // Check for a per-vertex label.

        Object oLabelAsObject;
        String sLabel = null;

        if ( vertex.TryGetValue(ReservedMetadataKeys.PerVertexLabel,
            typeof(String), out oLabelAsObject) )
        {
            sLabel = (String)oLabelAsObject;

            if ( String.IsNullOrEmpty(sLabel) )
            {
                sLabel = null;
            }
            else
            {
                sLabel = TruncateLabel(sLabel);
            }
        }

        Boolean bDrawAsSelected = GetDrawAsSelected(vertex);
        Point oLocation = WpfGraphicsUtil.PointFToWpfPoint(vertex.Location);
        DrawingVisual oDrawingVisual = new DrawingVisual();
        VertexShape eShape = GetShape(vertex);

        VertexLabelDrawer oVertexLabelDrawer =
            new VertexLabelDrawer(m_eLabelPosition);

        using ( DrawingContext oDrawingContext = oDrawingVisual.RenderOpen() )
        {
            if (eShape == VertexShape.Label)
            {
                if (sLabel != null)
                {
                    // Draw the vertex as a label.

                    vertexDrawingHistory = DrawLabelShape(vertex,
                        graphDrawingContext, oDrawingContext, oDrawingVisual,
                        eVisibility, bDrawAsSelected, sLabel);

                    return (true);
                }

                // Default to something usable.

                eShape = VertexShape.Disk;
            }
            else if (eShape == VertexShape.Image)
            {
                Object oImageSourceAsObject;

                if (vertex.TryGetValue(ReservedMetadataKeys.PerVertexImage,
                    typeof(ImageSource), out oImageSourceAsObject)
                    )
                {
                    // Draw the vertex as an image.

                    vertexDrawingHistory = DrawImageShape(vertex,
                        graphDrawingContext, oDrawingContext, oDrawingVisual,
                        eVisibility, bDrawAsSelected, sLabel,
                        (ImageSource)oImageSourceAsObject, oVertexLabelDrawer);

                    return (true);
                }

                // Default to something usable.

                eShape = VertexShape.Disk;
            }

            // Draw the vertex as a simple shape.

            vertexDrawingHistory = DrawSimpleShape(vertex, eShape,
                graphDrawingContext, oDrawingContext, oDrawingVisual,
                eVisibility, bDrawAsSelected, sLabel, oVertexLabelDrawer);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: DrawSimpleShape()
    //
    /// <summary>
    /// Draws a vertex as a simple shape.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to draw.
    /// </param>
    ///
    /// <param name="eShape">
    /// The vertex shape to use.  Can't be <see cref="VertexShape.Image" /> or
    /// <see cref="VertexShape.Label" />.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="oDrawingVisual">
    /// The <see cref="DrawingVisual" /> object from which <paramref
    /// name="oDrawingContext" /> was obtained.
    /// </param>
    ///
    /// <param name="eVisibility">
    /// The visibility of the vertex.
    /// </param>
    ///
    /// <param name="bDrawAsSelected">
    /// true to draw the vertex as selected.
    /// </param>
    ///
    /// <param name="sAnnotation">
    /// The annotation to draw next to the shape, or null if there is no
    /// annotation.
    /// </param>
    ///
    /// <param name="oVertexLabelDrawer">
    /// Object that draws a vertex label as an annotation.
    /// </param>
    ///
    /// <returns>
    /// A VertexDrawingHistory object that retains information about how the
    /// vertex was drawn.
    /// </returns>
    ///
    /// <remarks>
    /// "Simple" means "not <see cref="VertexShape.Image" /> and not <see
    /// cref="VertexShape.Label" />."
    /// </remarks>
    //*************************************************************************

    protected VertexDrawingHistory
    DrawSimpleShape
    (
        IVertex oVertex,
        VertexShape eShape,
        GraphDrawingContext oGraphDrawingContext,
        DrawingContext oDrawingContext,
        DrawingVisual oDrawingVisual,
        VisibilityKeyValue eVisibility,
        Boolean bDrawAsSelected,
        String sAnnotation,
        VertexLabelDrawer oVertexLabelDrawer
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oDrawingVisual != null);
        Debug.Assert(oVertexLabelDrawer != null);
        AssertValid();

        Double dRadius = GetRadius(oVertex);
        Color oColor = GetColor(oVertex, eVisibility, bDrawAsSelected);
        Point oVertexLocation = GetVertexLocation(oVertex);

        Rect oVertexBounds;

        if (eShape == VertexShape.Triangle ||
            eShape == VertexShape.SolidTriangle)
        {
            oVertexBounds =
                WpfGraphicsUtil.TriangleBoundsFromCenterAndHalfWidth(
                    oVertexLocation, dRadius);
        }
        else
        {
            oVertexBounds = WpfGraphicsUtil.SquareFromCenterAndHalfWidth(
                oVertexLocation, dRadius);
        }

        // Move the vertex if it falls outside the graph rectangle.

        MoveVertexIfNecessary(oVertex, ref oVertexBounds,
            oGraphDrawingContext);

        Point oLocation = GetVertexLocation(oVertex);
        VertexDrawingHistory oVertexDrawingHistory = null;

        // Note that for the "hollow" shapes -- Circle, Square, Diamond, and
        // Triangle -- Brushes.Transparent is used instead of a null brush.
        // This allows the entire area of these shapes to be hit-tested.  Using
        // a null brush would cause hit-testing to fail if the shapes'
        // interiors were clicked.

        switch (eShape)
        {
            case VertexShape.Circle:
            case VertexShape.Disk:

                Boolean bIsDisk = (eShape == VertexShape.Disk);

                oDrawingContext.DrawEllipse(
                    bIsDisk ? GetBrush(oColor) : Brushes.Transparent,
                    bIsDisk ? null : GetPen(oColor, DefaultPenThickness),
                    oLocation, dRadius, dRadius
                    );

                oVertexDrawingHistory = bIsDisk ?

                    new DiskVertexDrawingHistory(
                        oVertex, oDrawingVisual, bDrawAsSelected, dRadius)
                    :
                    new CircleVertexDrawingHistory(
                        oVertex, oDrawingVisual, bDrawAsSelected, dRadius);

                break;

            case VertexShape.Sphere:

                RadialGradientBrush oRadialGradientBrush =
                    new RadialGradientBrush();

                oRadialGradientBrush.GradientOrigin =
                    oRadialGradientBrush.Center = new Point(0.3, 0.3);

                GradientStopCollection oGradientStops =
                    oRadialGradientBrush.GradientStops;

                oGradientStops.Add( new GradientStop(Colors.White, 0.0) );
                oGradientStops.Add( new GradientStop(oColor, 1.0) );

                WpfGraphicsUtil.FreezeIfFreezable(oRadialGradientBrush);

                oDrawingContext.DrawEllipse(oRadialGradientBrush, null,
                    oLocation, dRadius, dRadius);

                oVertexDrawingHistory = new SphereVertexDrawingHistory(
                    oVertex, oDrawingVisual, bDrawAsSelected, dRadius);

                break;

            case VertexShape.Square:
            case VertexShape.SolidSquare:

                Boolean bIsSolidSquare = (eShape == VertexShape.SolidSquare);

                oDrawingContext.DrawRectangle(
                    bIsSolidSquare ? GetBrush(oColor) : Brushes.Transparent,
                    bIsSolidSquare? null : GetPen(oColor, DefaultPenThickness),
                    oVertexBounds
                    );

                oVertexDrawingHistory = bIsSolidSquare ?

                    new SolidSquareVertexDrawingHistory(oVertex,
                        oDrawingVisual, bDrawAsSelected, oVertexBounds)
                    :
                    new SquareVertexDrawingHistory(oVertex,
                        oDrawingVisual, bDrawAsSelected, oVertexBounds);

                break;

            case VertexShape.Diamond:
            case VertexShape.SolidDiamond:

                Boolean bIsSolidDiamond = (eShape == VertexShape.SolidDiamond);

                PathGeometry oDiamond =
                    WpfGraphicsUtil.DiamondFromCenterAndHalfWidth(
                        oLocation, dRadius);

                oDrawingContext.DrawGeometry(

                    bIsSolidDiamond ? GetBrush(oColor) : Brushes.Transparent,

                    bIsSolidDiamond ? null :
                        GetPen(oColor, DefaultPenThickness),

                    oDiamond
                    );

                oVertexDrawingHistory = bIsSolidDiamond ?

                    new SolidDiamondVertexDrawingHistory(
                        oVertex, oDrawingVisual, bDrawAsSelected, dRadius)
                    :
                    new DiamondVertexDrawingHistory(
                        oVertex, oDrawingVisual, bDrawAsSelected, dRadius);

                break;

            case VertexShape.Triangle:
            case VertexShape.SolidTriangle:

                Boolean bIsSolidTriangle =
                    (eShape == VertexShape.SolidTriangle);

                PathGeometry oTriangle =
                    WpfGraphicsUtil.TriangleFromCenterAndHalfWidth(
                        oLocation, dRadius);

                oDrawingContext.DrawGeometry(

                    bIsSolidTriangle ? GetBrush(oColor) : Brushes.Transparent,

                    bIsSolidTriangle ? null :
                        GetPen(oColor, DefaultPenThickness),

                    oTriangle
                    );

                oVertexDrawingHistory = bIsSolidTriangle ?

                    new SolidTriangleVertexDrawingHistory(
                        oVertex, oDrawingVisual, bDrawAsSelected, dRadius)
                    :
                    new TriangleVertexDrawingHistory(
                        oVertex, oDrawingVisual, bDrawAsSelected, dRadius);

                break;

            default:

                Debug.Assert(false);
                break;
        }

        if (sAnnotation != null)
        {
            oVertexLabelDrawer.DrawLabel( oDrawingContext,
                oGraphDrawingContext, oVertexDrawingHistory, oVertexBounds,
                CreateFormattedText(sAnnotation, oColor) );
        }

        Debug.Assert(oVertexDrawingHistory != null);

        return (oVertexDrawingHistory);
    }

    //*************************************************************************
    //  Method: DrawImageShape()
    //
    /// <summary>
    /// Draws a vertex as a specified image.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to draw.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="oDrawingVisual">
    /// The <see cref="DrawingVisual" /> object from which <paramref
    /// name="oDrawingContext" /> was obtained.
    /// </param>
    ///
    /// <param name="eVisibility">
    /// The visibility of the vertex.
    /// </param>
    ///
    /// <param name="bDrawAsSelected">
    /// true to draw the vertex as selected.
    /// </param>
    ///
    /// <param name="sAnnotation">
    /// The annotation to draw next to the image, or null if there is no
    /// annotation.
    /// </param>
    ///
    /// <param name="oImageSource">
    /// The image to draw.
    /// </param>
    ///
    /// <param name="oVertexLabelDrawer">
    /// Object that draws a vertex label as an annotation.
    /// </param>
    ///
    /// <returns>
    /// A VertexDrawingHistory object that retains information about how the
    /// vertex was drawn.
    /// </returns>
    //*************************************************************************

    protected VertexDrawingHistory
    DrawImageShape
    (
        IVertex oVertex,
        GraphDrawingContext oGraphDrawingContext,
        DrawingContext oDrawingContext,
        DrawingVisual oDrawingVisual,
        VisibilityKeyValue eVisibility,
        Boolean bDrawAsSelected,
        String sAnnotation,
        ImageSource oImageSource,
        VertexLabelDrawer oVertexLabelDrawer
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oDrawingVisual != null);
        Debug.Assert(oImageSource != null);
        Debug.Assert(oVertexLabelDrawer != null);
        AssertValid();

        // Move the vertex if it falls outside the graph rectangle.

        Rect oVertexRectangle = GetVertexRectangle(
            GetVertexLocation(oVertex), oImageSource.Width,
            oImageSource.Height);

        MoveVertexIfNecessary(oVertex, ref oVertexRectangle,
            oGraphDrawingContext);

        Byte btAlpha = 255;

        if (!bDrawAsSelected)
        {
            // Check for a non-opaque alpha value.

            btAlpha = GetAlpha(oVertex, eVisibility, btAlpha);
        }

        VertexDrawingHistory oVertexDrawingHistory =
            new ImageVertexDrawingHistory(oVertex, oDrawingVisual,
                bDrawAsSelected, oVertexRectangle);

        if (btAlpha > 0)
        {
            oDrawingContext.DrawImage(oImageSource, oVertexRectangle);

            Color oColor = GetColor(oVertex, eVisibility, bDrawAsSelected);

            // Draw an outline rectangle.

            oDrawingContext.DrawRectangle(null,
                GetPen(oColor, DefaultPenThickness), oVertexRectangle);

            if (btAlpha < 255)
            {
                // Real transparency can't be achieved with arbitrary images,
                // so simulate transparency by drawing on top of the image with
                // a translucent brush the same color as the graph's
                // background.
                //
                // This really isn't a good solution.  Is there are better way
                // to simulate transparency?

                Color oTranslucentColor = oGraphDrawingContext.BackColor;
                oTranslucentColor.A = (Byte)( (Byte)255 - btAlpha );

                oDrawingContext.DrawRectangle(
                    CreateFrozenSolidColorBrush(oTranslucentColor), null,
                        oVertexRectangle);
            }

            if (sAnnotation != null)
            {
                oVertexLabelDrawer.DrawLabel(oDrawingContext,
                    oGraphDrawingContext, oVertexDrawingHistory,
                    oVertexRectangle, CreateFormattedText(sAnnotation, oColor)
                    );
            }
        }

        return (oVertexDrawingHistory);
    }

    //*************************************************************************
    //  Method: DrawLabelShape()
    //
    /// <summary>
    /// Draws a vertex as a label.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to draw.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="oDrawingVisual">
    /// The <see cref="DrawingVisual" /> object from which <paramref
    /// name="oDrawingContext" /> was obtained.
    /// </param>
    ///
    /// <param name="eVisibility">
    /// The visibility of the vertex.
    /// </param>
    ///
    /// <param name="bDrawAsSelected">
    /// true to draw the vertex as selected.
    /// </param>
    ///
    /// <param name="sLabel">
    /// The label to draw.  Can't be null or empty.
    /// </param>
    ///
    /// <returns>
    /// A VertexDrawingHistory object that retains information about how the
    /// vertex was drawn.
    /// </returns>
    //*************************************************************************

    protected VertexDrawingHistory
    DrawLabelShape
    (
        IVertex oVertex,
        GraphDrawingContext oGraphDrawingContext,
        DrawingContext oDrawingContext,
        DrawingVisual oDrawingVisual,
        VisibilityKeyValue eVisibility,
        Boolean bDrawAsSelected,
        String sLabel
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oDrawingVisual != null);
        Debug.Assert( !String.IsNullOrEmpty(sLabel) );
        AssertValid();

        // Figure out what colors to use.

        Color oOutlineColor;

        Color oTextColor = GetColor(oVertex, eVisibility, false);

        Color oFillColor = GetColor(oVertex, eVisibility,
            ReservedMetadataKeys.PerVertexLabelFillColor, m_oLabelFillColor,
            true);

        if (bDrawAsSelected)
        {
            // The outline color is always the selected color.

            oOutlineColor = m_oSelectedColor;

            // The text color is the default or per-vertex color with no alpha.

            oTextColor.A = 255;

            // The fill color is the default or per-vertex fill color with no
            // alpha.

            oFillColor.A = 255;
        }
        else
        {
            // The outline color is the default or per-vertex color with alpha.

            oOutlineColor = oTextColor;

            // The text color is the default or per-vertex color with alpha.

            // The fill color is the default or per-vertex fill color with
            // alpha.
        }

        Double dLabelFontSize = GetLabelFontSize(oVertex);

        // Format the text, subject to a maximum label size.

        FormattedText oFormattedText = CreateFormattedText(sLabel, oTextColor,
            dLabelFontSize);

        oFormattedText.MaxTextWidth = MaximumLabelWidth;
        oFormattedText.MaxTextHeight = MaximumLabelHeight;

        Rect oVertexRectangle = GetVertexRectangle(
            GetVertexLocation(oVertex), oFormattedText.Width,
            oFormattedText.Height);

        // Pad the text.

        Rect oVertexRectangleWithPadding = oVertexRectangle;
        Double dLabelPadding = GetLabelPadding(dLabelFontSize);

        oVertexRectangleWithPadding.Inflate(dLabelPadding,
            dLabelPadding * 0.7);

        if (m_oTypeface.Style != FontStyles.Normal)
        {
            // This is a hack to move the right edge of the padded rectangle
            // to the right to adjust for wider italic text, which
            // FormattedText.Width does not account for.  What is the correct
            // way to do this?  It might involve the FormattedText.Overhang*
            // properties, but I'll be darned if I can understand how those
            // properties work.

            Double dItalicCompensation = dLabelFontSize / 7.0;
            oVertexRectangleWithPadding.Inflate(dItalicCompensation, 0);
            oVertexRectangleWithPadding.Offset(dItalicCompensation, 0);
        }

        // Move the vertex if it falls outside the graph rectangle.

        Double dOriginalVertexRectangleWithPaddingX =
            oVertexRectangleWithPadding.X;

        Double dOriginalVertexRectangleWithPaddingY =
            oVertexRectangleWithPadding.Y;

        MoveVertexIfNecessary(oVertex, ref oVertexRectangleWithPadding,
            oGraphDrawingContext);

        oVertexRectangle.Offset(

            oVertexRectangleWithPadding.X -
                dOriginalVertexRectangleWithPaddingX,

            oVertexRectangleWithPadding.Y -
                dOriginalVertexRectangleWithPaddingY
            );

        // Draw the padded rectangle, then the text.

        oDrawingContext.DrawRectangle( GetBrush(oFillColor),
            GetPen(oOutlineColor, DefaultPenThickness),
            oVertexRectangleWithPadding);

        oDrawingContext.DrawText(oFormattedText, oVertexRectangle.Location);

        // Return information about how the vertex was drawn.

        return ( new LabelVertexDrawingHistory(oVertex, oDrawingVisual,
            bDrawAsSelected, oVertexRectangleWithPadding) );
    }

    //*************************************************************************
    //  Method: MoveVertexIfNecessary()
    //
    /// <summary>
    /// Moves a vertex if it falls outside the graph rectangle.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to move.
    /// </param>
    ///
    /// <param name="oVertexBounds">
    /// The rectangle defining the bounds of the vertex.  This gets updated if
    /// the vertex is moved.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <remarks>
    /// If the vertex falls outside the graph rectangle, the IVertex.Location
    /// property and <paramref name="oVertexBounds" /> get updated.
    /// </remarks>
    //*************************************************************************

    protected void
    MoveVertexIfNecessary
    (
        IVertex oVertex,
        ref Rect oVertexBounds,
        GraphDrawingContext oGraphDrawingContext
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oGraphDrawingContext != null);
        AssertValid();

        Rect oGraphRectangleMinusMargin =
            oGraphDrawingContext.GraphRectangleMinusMargin;

        if (oGraphRectangleMinusMargin.IsEmpty)
        {
            return;
        }

        Rect oMovedVertexBounds = WpfGraphicsUtil.MoveRectangleWithinBounds(
            oVertexBounds, oGraphRectangleMinusMargin, false);

        oVertex.Location = System.Drawing.PointF.Add( oVertex.Location,
            new System.Drawing.SizeF(
                (Single)(oMovedVertexBounds.X - oVertexBounds.X),
                (Single)(oMovedVertexBounds.Y - oVertexBounds.Y)
                ) );

        oVertexBounds = oMovedVertexBounds;
    }

    //*************************************************************************
    //  Method: GetShape()
    //
    /// <summary>
    /// Gets the shape of a vertex.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to get the shape for.
    /// </param>
    ///
    /// <returns>
    /// The shape of the vertex.
    /// </returns>
    //*************************************************************************

    protected VertexShape
    GetShape
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        // Start with the default shape.

        VertexShape eShape = m_eShape;

        Object oPerVertexShapeAsObject;

        // Check for a per-vertex shape.

        if ( oVertex.TryGetValue(ReservedMetadataKeys.PerVertexShape,
            typeof(VertexShape), out oPerVertexShapeAsObject) )
        {
            eShape = (VertexShape)oPerVertexShapeAsObject;
        }

        return (eShape);
    }

    //*************************************************************************
    //  Method: GetRadius()
    //
    /// <summary>
    /// Gets the radius of a vertex.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to get the radius for.
    /// </param>
    ///
    /// <returns>
    /// The radius of the vertex.
    /// </returns>
    //*************************************************************************

    protected Double
    GetRadius
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        // Start with the default radius.

        Double dRadius = m_dRadius;

        Object oPerVertexRadiusAsObject;

        // Check for a per-vertex radius.  Note that the radius is stored as a
        // Single in the vertex's metadata to reduce memory usage.

        if ( oVertex.TryGetValue(ReservedMetadataKeys.PerVertexRadius,
            typeof(Single), out oPerVertexRadiusAsObject) )
        {
            dRadius = (Double)(Single)oPerVertexRadiusAsObject;

            if (dRadius < MinimumRadius || dRadius > MaximumRadius)
            {
                throw new FormatException( String.Format(

                    "{0}: The vertex with the ID {1} has an out-of-range"
                    + " ReservedMetadataKeys.PerVertexRadius value.  Valid"
                    + " values are between {2} and {3}."
                    ,
                    this.ClassName,
                    oVertex.ID,
                    MinimumRadius,
                    MaximumRadius
                    ) );
            }
        }

        return (dRadius);
    }

    //*************************************************************************
    //  Method: GetLabelFontSize()
    //
    /// <summary>
    /// Gets the font size to use for a vertex with the shape Label.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to get the label font size for.
    /// </param>
    ///
    /// <returns>
    /// The label font size to use, in WPF units.
    /// </returns>
    //*************************************************************************

    protected Double
    GetLabelFontSize
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        // Start with the default font size.

        Double dLabelFontSize = m_dFontSize;

        Object oPerVertexLabelFontSizeAsObject;

        // Check for a per-vertex font size.  Note that the font size is stored
        // as a Single in the vertex's metadata to reduce memory usage.

        if ( oVertex.TryGetValue(ReservedMetadataKeys.PerVertexLabelFontSize,
            typeof(Single), out oPerVertexLabelFontSizeAsObject) )
        {
            dLabelFontSize = (Double)(Single)oPerVertexLabelFontSizeAsObject;

            if (dLabelFontSize <= 0)
            {
                throw new FormatException( String.Format(

                    "{0}: The vertex with the ID {1} has a non-positive"
                    + " ReservedMetadataKeys.PerVertexLabelFontSize value."
                    ,
                    this.ClassName,
                    oVertex.ID
                    ) );
            }
        }

        return (dLabelFontSize);
    }

    //*************************************************************************
    //  Method: GetLabelPadding()
    //
    /// <summary>
    /// Gets the text padding to use for a vertex with the shape Label.
    /// </summary>
    ///
    /// <param name="dLabelFontSize">
    /// The label font size being used, in WPF units.
    /// </param>
    ///
    /// <returns>
    /// The text padding to use, in WPF units.
    /// </returns>
    //*************************************************************************

    protected Double
    GetLabelPadding
    (
        Double dLabelFontSize
    )
    {
        Debug.Assert(dLabelFontSize >= 0);
        AssertValid();

        // Make the padding larger for smaller fonts than for larger fonts.
        // These linear-interpolation points were selected to satisfy some
        // padding specifications in a NodeXL design document.

        const Double FontSizeA = 14.0;
        const Double FontSizeB = 39.4;
        const Double LabelPaddingA = 3.0;
        const Double LabelPaddingB = 5.0;

        Double dLabelPadding =
            LabelPaddingA + (dLabelFontSize - FontSizeA) *
            (LabelPaddingB - LabelPaddingA) / (FontSizeB - FontSizeA);

        dLabelPadding = Math.Max(0, dLabelPadding);

        return (dLabelPadding);
    }

    //*************************************************************************
    //  Method: GetVertexRectangle()
    //
    /// <summary>
    /// Gets a rectangle to use to draw a vertex.
    /// </summary>
    ///
    /// <param name="oLocation">
    /// The vertex's location.
    /// </param>
    ///
    /// <param name="dWidth">
    /// The width of the vertex's rectangle.
    /// </param>
    ///
    /// <param name="dHeight">
    /// The height of the vertex's rectangle.
    /// </param>
    ///
    /// <returns>
    /// A rectangle centered on <paramref name="oLocation" />.
    /// </returns>
    //*************************************************************************

    protected Rect
    GetVertexRectangle
    (
        Point oLocation,
        Double dWidth,
        Double dHeight
    )
    {
        Debug.Assert(dWidth >= 0);
        Debug.Assert(dHeight >= 0);
        AssertValid();

        return new Rect(

            new Point(oLocation.X - dWidth / 2.0,
                oLocation.Y - dHeight / 2.0),
                        
            new Size(dWidth, dHeight)
            );
    }

    //*************************************************************************
    //  Method: GetVertexLocation()
    //
    /// <summary>
    /// Gets the location of a vertex, as a System.Windows.Point.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to get the location of.
    /// </param>
    ///
    /// <returns>
    /// The IVertex.Location property, converted to a System.Windows.Point.
    /// </returns>
    //*************************************************************************

    protected Point
    GetVertexLocation
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        return ( WpfGraphicsUtil.PointFToWpfPoint(oVertex.Location) );
    }

    //*************************************************************************
    //  Method: CheckDrawVertexArguments()
    //
    /// <summary>
    /// Checks the arguments to <see cref="TryDrawVertex" />.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex that will eventually be drawn.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <remarks>
    /// An exception is thrown if one of the arguments is invalid.
    /// </remarks>
    //*************************************************************************

    protected void
    CheckDrawVertexArguments
    (
        IVertex oVertex,
        GraphDrawingContext oGraphDrawingContext
    )
    {
        AssertValid();

        const String MethodName = "TryDrawVertex";
        const String VertexArgumentName = "vertex";

        ArgumentChecker oArgumentChecker = this.ArgumentChecker;

        oArgumentChecker.CheckArgumentNotNull(MethodName, VertexArgumentName,
            oVertex);

        oArgumentChecker.CheckArgumentNotNull(MethodName,
            "graphDrawingContext", oGraphDrawingContext);

        if (oVertex.ParentGraph == null)
        {
            oArgumentChecker.ThrowArgumentException(
                MethodName, VertexArgumentName,
                "The vertex doesn't belong to a graph.  It can't be drawn."
                );
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

        // m_eShape
        Debug.Assert(m_dRadius >= MinimumRadius);
        Debug.Assert(m_dRadius <= MaximumRadius);
        // m_oLabelFillColor
        // m_eLabelPosition
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// Minimum value of the <see cref="Radius" /> property.  The value is 0.1.
    /// </summary>

    public static Double MinimumRadius = 0.1;

    /// <summary>
    /// Maximum value of the <see cref="Radius" /> property.  The value is
    /// 50.0.
    /// </summary>

    public static Double MaximumRadius = 50.0;


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Maximum width of a label shape, not including the label padding, in
    /// device-independent units.

    protected const Double MaximumLabelWidth = 300;

    /// Maximum height of a label shape, not including the label padding, in
    /// device-independent units.

    protected const Double MaximumLabelHeight = 200;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Default shape of the vertices.

    protected VertexShape m_eShape;

    /// Default radius of the vertices.

    protected Double m_dRadius;

    /// Default fill color to use for labels.

    protected Color m_oLabelFillColor;

    /// Default position of vertex labels drawn as annotations.

    protected VertexLabelPosition m_eLabelPosition;
}


//*****************************************************************************
//  Enum: VertexShape
//
/// <summary>
/// Specifies the shape of a vertex.
/// </summary>
//*****************************************************************************

public enum
VertexShape
{
    // Note:
    //
    // These values get stored in user setting files and their numerical
    // values should not be modified.

    /// <summary>
    /// The vertex is drawn as a circle.
    /// </summary>

    Circle = 0,

    /// <summary>
    /// The vertex is drawn as a disk.
    /// </summary>

    Disk = 1,

    /// <summary>
    /// The vertex is drawn as a sphere.
    /// </summary>

    Sphere = 2,

    /// <summary>
    /// The vertex is drawn as a square.
    /// </summary>

    Square = 3,

    /// <summary>
    /// The vertex is drawn as a solid square.
    /// </summary>

    SolidSquare = 4,

    /// <summary>
    /// The vertex is drawn as a diamond.
    /// </summary>

    Diamond = 5,

    /// <summary>
    /// The vertex is drawn as a solid diamond.
    /// </summary>

    SolidDiamond = 6,

    /// <summary>
    /// The vertex is drawn as an equilateral triangle.
    /// </summary>

    Triangle = 7,

    /// <summary>
    /// The vertex is drawn as a solid equilateral triangle.
    /// </summary>

    SolidTriangle = 8,

    /// <summary>
    /// The vertex is drawn as an image.
    ///
    /// <para>
    /// The image is obtained from the <see
    /// cref="ReservedMetadataKeys.PerVertexImage" /> metadata key.  If the
    /// <see cref="ReservedMetadataKeys.PerVertexImage" /> key is missing, the
    /// vertex is drawn as a <see cref="Disk" />.
    /// </para>
    ///
    /// </summary>

    Image = 9,

    /// <summary>
    /// The vertex is drawn as a label.
    ///
    /// <para>
    /// The label text is obtained from the <see
    /// cref="ReservedMetadataKeys.PerVertexLabel" /> metadata key.  If the
    /// <see cref="ReservedMetadataKeys.PerVertexLabel" /> key is missing, the
    /// vertex is drawn as a <see cref="Disk" />.
    /// </para>
    ///
    /// </summary>

    Label = 10,

    // If a new shape is added, the following must be done:
    //
    // 1. Update the drawing code in this class.
    //
    // 2. Add new entries to the Valid Vertex Shapes column on the Misc table
    //    of the NodeXLGraph.xltx Excel template.
    //
    // 3. Add a button to the Vertex Shape menu in the Excel ribbon, which is
    //    implemented in the Ribbon.cs file.
}


//*****************************************************************************
//  Enum: VertexLabelPosition
//
/// <summary>
/// Specifies the position of a vertex label drawn as an annotation.
/// </summary>
///
/// <remarks>
/// The positions are used only for vertex labels drawn as an annotation.  They
/// are not used when a vertex has the shape <see cref="VertexShape.Label" />.
///
/// <para>
/// The horizontal positions are called Left, Center, and Right.  The vertical
/// positions are called Top, Middle, and Bottom.  The label positions with
/// respect to the vertex's rectangle are shown above.
/// </para>
///
/// </remarks>
///
/// <example>
/// <code>
///                 TopCenter
///      TopLeft  --------------  TopRight
///              |              |
///              |              |
///   MiddleLeft | MiddleCenter | MiddleRight
///              |              |
///              |              |
///   BottomLeft  --------------  BottomRight
///                BottomCenter
/// </code>
/// </example>
//*****************************************************************************

public enum
VertexLabelPosition
{
    // Note:
    //
    // These values get stored in user setting files and their numerical
    // values should not be modified.

    /// <summary>
    /// The label is right-justified.  See the position in the above drawing.
    /// </summary>

    TopLeft = 0,

    /// <summary>
    /// The label is center-justified.  See the position in the above drawing.
    /// </summary>

    TopCenter = 1,

    /// <summary>
    /// The label is left-justified.  See the position in the above drawing.
    /// </summary>

    TopRight = 2,

    /// <summary>
    /// The label is right-justified.  See the position in the above drawing.
    /// </summary>

    MiddleLeft = 3,

    /// <summary>
    /// The label is center-justified.  See the position in the above drawing.
    /// </summary>

    MiddleCenter = 4,

    /// <summary>
    /// The label is left-justified.  See the position in the above drawing.
    /// </summary>

    MiddleRight = 5,

    /// <summary>
    /// The label is right-justified.  See the position in the above drawing.
    /// </summary>

    BottomLeft = 6,

    /// <summary>
    /// The label is center-justified.  See the position in the above drawing.
    /// </summary>

    BottomCenter = 7,

    /// <summary>
    /// The label is left-justified.  See the position in the above drawing.
    /// </summary>

    BottomRight = 8,
}
}
