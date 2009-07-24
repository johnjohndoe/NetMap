
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Media;
using System.Globalization;
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
/// <item><see cref="ReservedMetadataKeys.PerColor" /></item>
/// <item><see cref="ReservedMetadataKeys.PerAlpha" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexShape" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexRadius" /></item>
/// <item><see cref="ReservedMetadataKeys.IsSelected" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexPrimaryLabel" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexPrimaryLabelFillColor" />
///     </item>
/// <item><see cref="ReservedMetadataKeys.PerVertexSecondaryLabel" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexImage" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexDrawingPrecedence" /></item>
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
/// If the <see cref="ReservedMetadataKeys.PerVertexDrawingPrecedence" /> key
/// is not present, <see cref="VertexDrawer" /> looks for the <see
/// cref="ReservedMetadataKeys.PerVertexPrimaryLabel" />, <see
/// cref="ReservedMetadataKeys.PerVertexImage" />, and <see
/// cref="ReservedMetadataKeys.PerVertexShape" /> keys, in that order.  If none
/// of these keys are present, the vertex is drawn as the shape specified by
/// the <see cref="Shape" /> property.
/// </para>
///
/// <para>
/// If a vertex has the <see
/// cref="ReservedMetadataKeys.PerVertexPrimaryLabel" /> key, the vertex is
/// drawn as a rectangle containing the text specified by the key's value.  The
/// default color of the text and the rectangle's outline is <see
/// cref="VertexAndEdgeDrawerBase.Color" />, but can be overridden with the
/// <see cref="ReservedMetadataKeys.PerColor" /> key.  The default fill color
/// of the rectangle is <see cref="PrimaryLabelFillColor" />, but can be
/// overridden with the <see
/// cref="ReservedMetadataKeys.PerVertexPrimaryLabelFillColor" /> key.
/// </para>
///
/// <para>
/// If a vertex has the <see
/// cref="ReservedMetadataKeys.PerVertexImage" /> key, the vertex is
/// drawn as the image specified by the key's value.
/// </para>
///
/// <para>
/// If a vertex has the <see
/// cref="ReservedMetadataKeys.PerVertexSecondaryLabel" /> key, the vertex is
/// annotated with the text specified by the key's value.
/// </para>
///
/// <para>
/// The values of the <see cref="ReservedMetadataKeys.PerColor" /> and
/// <see cref="ReservedMetadataKeys.PerVertexPrimaryLabelFillColor" /> keys
/// can be of type System.Windows.Media.Color or System.Drawing.Color.
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
        m_oPrimaryLabelFillColor = SystemColors.WindowColor;

        m_oTypeface = new Typeface(SystemFonts.MessageFontFamily,
            FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

        m_dFontSizeEm = 10;

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
    //  Property: PrimaryLabelFillColor
    //
    /// <summary>
    /// Gets or sets the default fill color to use for primary labels.
    /// </summary>
    ///
    /// <value>
    /// The default fill color to use for primary labels.  The default is
    /// SystemColors.WindowColor.
    /// </value>
    ///
    /// <remarks>
    /// <see cref="Color" /> is used for the primary label text and outline.
    ///
    /// <para>
    /// The default fill color of a vertex can be overridden by setting the
    /// <see cref="ReservedMetadataKeys.PerVertexPrimaryLabelFillColor" /> key
    /// on the vertex.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Color
    PrimaryLabelFillColor
    {
        get
        {
            AssertValid();

            return (m_oPrimaryLabelFillColor);
        }

        set
        {
            if (m_oPrimaryLabelFillColor == value)
            {
                return;
            }

            m_oPrimaryLabelFillColor = value;

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

        // Determine the order in which the keys that specify primary label,
        // image, and shape are checked.

        VertexDrawingPrecedence ePrecedence = GetPrecedence(vertex);

        // Check for a per-vertex secondary label.

        Object oSecondaryLabelAsObject;
        String sSecondaryLabel = null;

        if ( vertex.TryGetValue(
            ReservedMetadataKeys.PerVertexSecondaryLabel, typeof(String),
            out oSecondaryLabelAsObject) )
        {
            sSecondaryLabel = (String)oSecondaryLabelAsObject;
        }

        Boolean bDrawAsSelected = GetDrawAsSelected(vertex);

        Point oLocation = WpfGraphicsUtil.PointFToWpfPoint(vertex.Location);

        DrawingVisual oDrawingVisual = new DrawingVisual();

        using ( DrawingContext oDrawingContext = oDrawingVisual.RenderOpen() )
        {
            Object oPrimaryLabelAsObject;

            if (
                ePrecedence >= VertexDrawingPrecedence.PrimaryLabel
                &&
                vertex.TryGetValue(ReservedMetadataKeys.PerVertexPrimaryLabel,
                    typeof(String), out oPrimaryLabelAsObject)
                )
            {
                // Draw the vertex as a primary label.

                vertexDrawingHistory = DrawPrimaryLabel(vertex,
                    graphDrawingContext, oDrawingContext, oDrawingVisual,
                    eVisibility, bDrawAsSelected, sSecondaryLabel,
                    (String)oPrimaryLabelAsObject);

                return (true);
            }

            Object oImageSourceAsObject;

            if (
                ePrecedence >= VertexDrawingPrecedence.Image
                &&
                vertex.TryGetValue(ReservedMetadataKeys.PerVertexImage,
                    typeof(ImageSource), out oImageSourceAsObject)
                )
            {
                // Draw the vertex as an image.

                vertexDrawingHistory = DrawImage(vertex, graphDrawingContext,
                    oDrawingContext, oDrawingVisual, eVisibility,
                    bDrawAsSelected, sSecondaryLabel,
                    (ImageSource)oImageSourceAsObject);

                return (true);
            }

            // Draw the vertex as a shape.

            vertexDrawingHistory = DrawShape(vertex, graphDrawingContext,
                oDrawingContext, oDrawingVisual, eVisibility, bDrawAsSelected,
                sSecondaryLabel);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: SetFont()
    //
    /// <summary>
    /// Sets the font used to draw primary and secondary labels.
    /// </summary>
    ///
    /// <param name="typeface">
    /// The Typeface to use.
    /// </param>
    ///
    /// <param name="emSize">
    /// The font size to use, in ems.
    /// </param>
    ///
    /// <remarks>
    /// The default font is the SystemFonts.MessageFontFamily at size 10.
    /// </remarks>
    //*************************************************************************

    public void
    SetFont
    (
        Typeface typeface,
        Double emSize
    )
    {
        Debug.Assert(typeface != null);
        Debug.Assert(emSize > 0);
        AssertValid();

        if (m_oTypeface == typeface && m_dFontSizeEm == emSize)
        {
            return;
        }

        m_oTypeface = typeface;
        m_dFontSizeEm = emSize;

        FireLayoutRequired();
    }

    //*************************************************************************
    //  Method: DrawShape()
    //
    /// <summary>
    /// Draws a vertex as a specified shape.
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
    /// <param name="sSecondaryLabel">
    /// The secondary label to draw, or null if there is no secondary label.
    /// </param>
    ///
    /// <returns>
    /// A VertexDrawingHistory object that retains information about how the
    /// vertex was drawn.
    /// </returns>
    //*************************************************************************

    protected VertexDrawingHistory
    DrawShape
    (
        IVertex oVertex,
        GraphDrawingContext oGraphDrawingContext,
        DrawingContext oDrawingContext,
        DrawingVisual oDrawingVisual,
        VisibilityKeyValue eVisibility,
        Boolean bDrawAsSelected,
        String sSecondaryLabel
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oDrawingVisual != null);
        AssertValid();

        VertexShape eShape = GetShape(oVertex);
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

        // The gets used when positioning the secondary label, if there is one.

        Double dSecondaryLabelYFactor = 1.0;

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

                dSecondaryLabelYFactor = 1.15;

                break;

            default:

                Debug.Assert(false);
                break;
        }

        if (sSecondaryLabel != null)
        {
            Point oSecondaryLabelLowerLeft = oLocation;

            // Provide some padding between the shape and the secondary label.

            oSecondaryLabelLowerLeft.Offset(
                -2,
                -(dRadius * dSecondaryLabelYFactor) -1
                );

            DrawSecondaryLabel(oDrawingContext, oGraphDrawingContext,
                sSecondaryLabel, oSecondaryLabelLowerLeft, oColor);
        }

        Debug.Assert(oVertexDrawingHistory != null);

        return (oVertexDrawingHistory);
    }

    //*************************************************************************
    //  Method: DrawImage()
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
    /// <param name="sSecondaryLabel">
    /// The secondary label to draw, or null if there is no secondary label.
    /// </param>
    ///
    /// <param name="oImageSource">
    /// The image to draw.
    /// </param>
    ///
    /// <returns>
    /// A VertexDrawingHistory object that retains information about how the
    /// vertex was drawn.
    /// </returns>
    //*************************************************************************

    protected VertexDrawingHistory
    DrawImage
    (
        IVertex oVertex,
        GraphDrawingContext oGraphDrawingContext,
        DrawingContext oDrawingContext,
        DrawingVisual oDrawingVisual,
        VisibilityKeyValue eVisibility,
        Boolean bDrawAsSelected,
        String sSecondaryLabel,
        ImageSource oImageSource
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oDrawingVisual != null);
        Debug.Assert(oImageSource != null);
        AssertValid();

        // Move the vertex if it falls outside the graph rectangle.

        Rect oVertexRectangle = GetVertexRectangle(
            GetVertexLocation(oVertex), oImageSource.Width,
            oImageSource.Height);

        MoveVertexIfNecessary(oVertex, ref oVertexRectangle,
            oGraphDrawingContext);

        oDrawingContext.DrawImage(oImageSource, oVertexRectangle);

        Byte btAlpha = 255;

        if (!bDrawAsSelected)
        {
            // Check for a non-opaque alpha value.

            btAlpha = GetAlpha(oVertex, eVisibility, btAlpha);
        }

        if (btAlpha == 255)
        {
            // Draw an outline rectangle.

            Color oColor = bDrawAsSelected ? m_oSelectedColor : m_oColor;

            oDrawingContext.DrawRectangle(null,
                GetPen(oColor, DefaultPenThickness), oVertexRectangle);
        }
        else
        {
            // Simulate transparency by drawing on top of the image with a
            // transparent brush the same color as the graph's background.
            //
            // TODO: Can real transparency be achieved with arbitrary images?

            Color oFillColor = oGraphDrawingContext.BackColor;
            oFillColor.A = (Byte)( (Byte)255 - btAlpha );

            Color oOutlineColor = m_oColor;
            oOutlineColor.A = btAlpha;

            SolidColorBrush oFillBrush =
                CreateFrozenSolidColorBrush(oFillColor);

            SolidColorBrush oOutlineBrush =
                CreateFrozenSolidColorBrush(oOutlineColor);

            Pen oOutlinePen =
                CreateFrozenPen(oOutlineBrush, DefaultPenThickness);

            oDrawingContext.DrawRectangle(oFillBrush, oOutlinePen,
                oVertexRectangle);
        }

        if (sSecondaryLabel != null)
        {
            Point oSecondaryLabelLowerLeft = oVertexRectangle.Location;

            oSecondaryLabelLowerLeft.Offset(0, -2);

            DrawSecondaryLabel(oDrawingContext, oGraphDrawingContext,
                sSecondaryLabel, oSecondaryLabelLowerLeft,
                GetColor(oVertex, eVisibility, bDrawAsSelected) );
        }

        // Return information about how the vertex was drawn.

        return ( new ImageVertexDrawingHistory(oVertex, oDrawingVisual,
            bDrawAsSelected, oVertexRectangle) );
    }

    //*************************************************************************
    //  Method: DrawPrimaryLabel()
    //
    /// <summary>
    /// Draws a vertex as a specified primary label.
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
    /// <param name="sSecondaryLabel">
    /// The secondary label to draw, or null if there is no secondary label.
    /// </param>
    ///
    /// <param name="sPrimaryLabel">
    /// The primary label to draw.
    /// </param>
    ///
    /// <returns>
    /// A VertexDrawingHistory object that retains information about how the
    /// vertex was drawn.
    /// </returns>
    //*************************************************************************

    protected VertexDrawingHistory
    DrawPrimaryLabel
    (
        IVertex oVertex,
        GraphDrawingContext oGraphDrawingContext,
        DrawingContext oDrawingContext,
        DrawingVisual oDrawingVisual,
        VisibilityKeyValue eVisibility,
        Boolean bDrawAsSelected,
        String sSecondaryLabel,
        String sPrimaryLabel
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oDrawingVisual != null);
        Debug.Assert( !String.IsNullOrEmpty(sPrimaryLabel) );
        AssertValid();

        // Figure out what colors to use.

        Color oOutlineColor;

        Color oTextColor = GetColor(oVertex, eVisibility, false);

        Color oFillColor = GetColor(oVertex, eVisibility,
            ReservedMetadataKeys.PerVertexPrimaryLabelFillColor,
            m_oPrimaryLabelFillColor, true);

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

        // Format the text, subject to a maximum label size.

        FormattedText oFormattedText = CreateFormattedText(sPrimaryLabel,
            oTextColor);

        oFormattedText.MaxTextWidth = MaximumPrimaryLabelWidth;
        oFormattedText.MaxTextHeight = MaximumPrimaryLabelHeight;

        Rect oVertexRectangle = GetVertexRectangle(
            GetVertexLocation(oVertex), oFormattedText.Width,
            oFormattedText.Height);

        // Pad the text.

        Rect oVertexRectangleWithPadding = oVertexRectangle;

        oVertexRectangleWithPadding.Inflate(PrimaryLabelPadding,
            PrimaryLabelPadding * 0.8);

        if (m_oTypeface.Style != FontStyles.Normal)
        {
            // This is a hack to move the right edge of the padded rectangle
            // to the right to adjust for wider italic text, which
            // FormattedText.Width does not account for.  What is the correct
            // way to do this?  It might involve the FormattedText.Overhang*
            // properties, but I'll be darned if I can understand how those
            // properties work.

            Double dItalicCompensation = m_dFontSizeEm / 7.0;
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

        if (sSecondaryLabel != null)
        {
            Point oSecondaryLabelLowerLeft =
                oVertexRectangleWithPadding.Location;

            oSecondaryLabelLowerLeft.Offset(0, -2);

            DrawSecondaryLabel(oDrawingContext, oGraphDrawingContext,
                sSecondaryLabel, oSecondaryLabelLowerLeft, oTextColor);
        }

        // Return information about how the vertex was drawn.

        return ( new PrimaryLabelVertexDrawingHistory(oVertex, oDrawingVisual,
            bDrawAsSelected, oVertexRectangleWithPadding) );
    }

    //*************************************************************************
    //  Method: DrawSecondaryLabel()
    //
    /// <summary>
    /// Draws a secondary label.
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
    /// <param name="sSecondaryLabel">
    /// The secondary label to draw.  Can be empty but not null.
    /// </param>
    ///
    /// <param name="oLowerLeft">
    /// The location of the lower-left corner of the secondary label.
    /// </param>
    ///
    /// <param name="oColor">
    /// The color of the secondary label.
    /// </param>
    //*************************************************************************

    protected void
    DrawSecondaryLabel
    (
        DrawingContext oDrawingContext,
        GraphDrawingContext oGraphDrawingContext,
        String sSecondaryLabel,
        Point oLowerLeft,
        Color oColor
    )
    {
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(sSecondaryLabel != null);
        AssertValid();

        FormattedText oFormattedText = CreateFormattedText(sSecondaryLabel,
            oColor);

        oFormattedText.MaxLineCount = 1;

        Rect oGraphRectangleMinusMargin =
            oGraphDrawingContext.GraphRectangleMinusMargin;

        // Don't let the label overflow the right edge of the graph rectangle.

        Double dOverflowX = Math.Max(0,
            oLowerLeft.X + oFormattedText.Width
            - oGraphRectangleMinusMargin.Right
            );

        Double dHeight = oFormattedText.Height;

        // Don't let the label overflow the top edge of the graph rectangle.

        Double dOverflowY = Math.Min(0,
            oLowerLeft.Y - dHeight - oGraphRectangleMinusMargin.Top
            );

        // (dHeight is subtracted below because the point passed to DrawText()
        // is the upper-left corner of the text.)

        oLowerLeft.Offset(-dOverflowX, -dHeight - dOverflowY);

        oDrawingContext.DrawText(oFormattedText, oLowerLeft);
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
    //  Method: GetPrecedence()
    //
    /// <summary>
    /// Gets the precedence with which to draw a vertex.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to get the precedence for.
    /// </param>
    ///
    /// <returns>
    /// The precedence with which to draw a vertex.
    /// </returns>
    ///
    /// <remarks>
    /// The precedence determines the order in which the keys that specify
    /// primary label, image, and shape are checked.
    /// </remarks>
    //*************************************************************************

    protected VertexDrawingPrecedence
    GetPrecedence
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        // Start with the default precedence.

        VertexDrawingPrecedence ePrecedence =
            VertexDrawingPrecedence.PrimaryLabel;

        Object oPrecedenceAsObject;

        // Check for a per-vertex precedence.

        if ( oVertex.TryGetValue(
            ReservedMetadataKeys.PerVertexDrawingPrecedence,
            typeof(VertexDrawingPrecedence), out oPrecedenceAsObject) )
        {
            ePrecedence = (VertexDrawingPrecedence)oPrecedenceAsObject;
        }

        return (ePrecedence);
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
                    + " {2} value.  Valid values are between {3} and {4}."
                    ,
                    this.ClassName,
                    oVertex.ID,
                    "ReservedMetadataKeys.PerVertexRadius",
                    MinimumRadius,
                    MaximumRadius
                    ) );
            }
        }

        return (dRadius);
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
    //  Method: CreateFormattedText()
    //
    /// <summary>
    /// Creates a FormattedText object.
    /// </summary>
    ///
    /// <param name="sText">
    /// The text to draw.  Can't be null.
    /// </param>
    ///
    /// <param name="oColor">
    /// The text color.
    /// </param>
    //*************************************************************************

    protected FormattedText
    CreateFormattedText
    (
        String sText,
        Color oColor
    )
    {
        Debug.Assert(sText != null);

        FormattedText oFormattedText = new FormattedText( sText,
            CultureInfo.CurrentCulture, FlowDirection.LeftToRight, m_oTypeface,
            m_dFontSizeEm, GetBrush(oColor) );

        return (oFormattedText);
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
        // m_oPrimaryLabelFillColor
        Debug.Assert(m_oTypeface != null);
        Debug.Assert(m_dFontSizeEm > 0);
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

    /// Maximum width of a primary label, not including the label padding, in
    /// device-independent units.

    protected const Double MaximumPrimaryLabelWidth = 300;

    /// Maximum height of a primary label, not including the label padding, in
    /// device-independent units.

    protected const Double MaximumPrimaryLabelHeight = 200;

    /// Padding between the primary label text and label outline, in
    /// device-independent units.

    protected const Double PrimaryLabelPadding = 4;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Default shape of the vertices.

    protected VertexShape m_eShape;

    /// Default radius of the vertices.

    protected Double m_dRadius;

    /// Default fill color to use for primary labels.

    protected Color m_oPrimaryLabelFillColor;

    /// The Typeface to use to draw primary and secondary labels.

    protected Typeface m_oTypeface;

    /// The font size to use to draw primary and secondary labels, in ems.

    protected Double m_dFontSizeEm;
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
}

}
