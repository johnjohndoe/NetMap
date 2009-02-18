
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Class: ReservedMetadataKeys
//
/// <summary>
/// Contains metadata keys used by the NodeXL system.
/// </summary>
///
/// <remarks>
/// Some of the metadata keys defined in this class are meant for use by
/// applications that use the NodeXL class libraries.  An application can add
/// the <see cref="PerColor" /> key to a vertex or edge to set its color, for
/// example.  Other keys are used internally by NodeXL and are of no use to
/// applications -- <see cref="LayoutBaseLayoutComplete" /> is used internally
/// by the NodeXL layout classes, for example.
///
/// <para>
/// See <see cref="IMetadataProvider" /> for information on the metadata scheme
/// used by NodeXL.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class ReservedMetadataKeys : Object
{
    //*************************************************************************
    //  General public constants
    //*************************************************************************

    /// <summary>
    /// All reserved keys start with this character.
    /// </summary>

    public static readonly Char FirstChar = '~';

    /// <summary>
    /// Key added to an edge to specify an edge weight.
    /// </summary>

    public static readonly String EdgeWeight =
        FirstChar + "EW";


    //*************************************************************************
    //  Keys used by NodeXLControl
    //*************************************************************************

    /// <summary>
    /// Key added to a vertex to set the vertex's tooltip when using
    /// NodeXLControl.  The key's value is a String.
    ///
    /// <para>
    /// For tooltips to be visible, you must set
    /// NodeXLControl.UseVertexToolTips to true.  The default value is false.
    /// </para>
    ///
    /// </summary>

    public static readonly String VertexToolTip =
        FirstChar + "CCTT";


    //*************************************************************************
    //  Keys used by VertexDrawer and EdgeDrawer
    //*************************************************************************

    /// <summary>
    /// Key added to a vertex or edge to draw it as selected.  The key's value
    /// is null.
    ///
    /// <para>
    /// <b>Important Note:</b>
    /// </para>
    ///
    /// <para>
    /// When using NodeXLControl, do not use the <see cref="IsSelected" /> key
    /// to select vertex or edges.  Use the selection methods on the control
    /// instead.
    /// </para>
    ///
    /// </summary>

    public static readonly String IsSelected =
        FirstChar + "S";

    /// <summary>
    /// Key added to a vertex or edge to control its visibility.  The key's
    /// value is a member of the <see cref="VisibilityKeyValue" /> enumeration.
    ///
    /// <para>
    /// If this key is present and its value is <see
    /// cref="VisibilityKeyValue.Filtered" /> or <see
    /// cref="VisibilityKeyValue.Hidden" />, any <see cref="PerAlpha" /> key is
    /// ignored.
    /// </para>
    ///
    /// </summary>

    public static readonly String Visibility =
        FirstChar + "VEVis";

    /// <summary>
    /// Key added to a vertex or edge to force it to be a specified color.
    /// The key's value can be either a System.Drawing.Color or a 
    /// System.Windows.Media.Color.
    /// </summary>

    public static readonly String PerColor =
        FirstChar + "PDColor";

    /// <summary>
    /// Key added to a vertex or edge to force it to be a specified
    /// transparency.  The key's value is a Byte between 0 (transparent) and
    /// 255 (opaque).
    ///
    /// <para>
    /// If the <see cref="Visibility" /> key is present and its value is <see
    /// cref="VisibilityKeyValue.Filtered" /> or <see
    /// cref="VisibilityKeyValue.Hidden" />, the <see cref="PerAlpha" /> key is
    /// ignored.
    /// </para>
    ///
    /// </summary>

    public static readonly String PerAlpha =
        FirstChar + "PDAlpha";


    //*************************************************************************
    //  Keys used by VertexDrawer
    //*************************************************************************

    /// <summary>
    /// Key added to a vertex to force it to be a specified shape.  The key's
    /// value is a VertexShape.
    /// </summary>

    public static readonly String PerVertexShape =
        FirstChar + "PVDShape";

    /// <summary>
    /// Key added to a vertex to force it to be a specified radius.  The key's
    /// value is a Single between VertexDrawer.MinimumRadius and
    /// VertexDrawer.MaximumRadius.
    /// </summary>

    public static readonly String PerVertexRadius =
        FirstChar + "PVDRadius";

    /// <summary>
    /// Key added to a vertex to force it to be drawn as an image instead of a
    /// shape.  The key's value is a System.Windows.Media.ImageSource.
    /// </summary>

    public static readonly String PerVertexImage =
        FirstChar + "PVIDImage";

    /// <summary>
    /// Key added to a vertex to force it to be drawn as a primary label, which
    /// is a rectangle containing text.  The key's value is a System.String.
    /// <see cref="PerVertexPrimaryLabel" /> and <see
    /// cref="PerVertexSecondaryLabel" /> may be specified in any combination.
    /// </summary>

    public static readonly String PerVertexPrimaryLabel =
        FirstChar + "PVLDPrimaryLabel";

    /// <summary>
    /// Key added to a vertex to force a primary label to have a specified fill
    /// color.  The key's value is a Color.  Used only if <see
    /// cref="PerVertexPrimaryLabel" /> is specified.
    /// </summary>

    public static readonly String PerVertexPrimaryLabelFillColor =
        FirstChar + "PVLDPrimaryLabelFillColor";

    /// <summary>
    /// Key added to a vertex to force a secondary label to be drawn near it.
    /// The key's value is a System.String.  A secondary label can be added to
    /// any vertex, regardless of whether it is drawn as a shape, primary
    /// label, or image.
    /// </summary>

    public static readonly String PerVertexSecondaryLabel =
        FirstChar + "PVLDSecondaryLabel";

    /// <summary>
    /// Key added to a vertex to specify whether the vertex should be drawn a
    /// a shape, image, or primary label.  The key's value is a member of the
    /// VertexDrawingPrecedence enumeration.
    /// </summary>

    public static readonly String PerVertexDrawingPrecedence =
        FirstChar + "PVDrawingPrecedence";


    //*************************************************************************
    //  Keys used by EdgeDrawer
    //*************************************************************************

    /// <summary>
    /// Key added to an edge to force it to be a specified width.  The key's
    /// value is a Single between EdgeDrawer.MinimumWidth and
    /// EdgeDrawer.MaximumWidth.
    /// </summary>

    public static readonly String PerEdgeWidth =
        FirstChar + "PEDWidth";


    //*************************************************************************
    //  Keys used by LayoutBase and AsyncLayoutBase
    //*************************************************************************

    /// <summary>
    /// Key added to the graph after it has been laid out.  The key's value is
    /// of type Rectangle and stores the graph's layout rectangle.
    /// </summary>

    public static readonly String LayoutBaseLayoutComplete =
        FirstChar + "LBLayoutComplete";


    //*************************************************************************
    //  Keys used by several layouts
    //*************************************************************************

    /// <summary>
    /// Key added to a graph to tell the layout algorithm to lay out only a
    /// specified set of vertices.  The key's value is an IVertex array
    /// containing the vertices to lay out.  When this key is present on the
    /// graph, the layout completely ignores the graph's vertices that are not
    /// in the specified array.
    /// </summary>

    public static readonly String LayOutTheseVerticesOnly =

        FirstChar + "LTheseOnly";

    /// <summary>
    /// Key added to a vertex to tell the layout algorithm to leave the vertex
    /// at its current location.  The key's value is a Boolean.  The layout may
    /// include the vertex in its overall layout calculations, but it cannot
    /// move the vertex.
    /// </summary>

    public static readonly String LockVertexLocation =
        FirstChar + "LLock";


    //*************************************************************************
    //  Keys used by CircleLayout
    //*************************************************************************

    /// <summary>
    /// Key added to the graph after it has been completely drawn.  The key's
    /// value is null.
    /// </summary>

    public static readonly String CircleLayoutCircleDrawn =
        FirstChar + "CLCircleDrawn";


    //*************************************************************************
    //  Keys used by SpiralLayout
    //*************************************************************************

    /// <summary>
    /// Key added to the graph after it has been completely drawn.  The key's
    /// value is null.
    /// </summary>

    public static readonly String SpiralLayoutSpiralDrawn =
        FirstChar + "SLSpiralDrawn";


    //*************************************************************************
    //  Keys used by SinusoidLayout
    //*************************************************************************

    /// <summary>
    /// Key added to the graph after it has been completely drawn.  The key's
    /// value is null.
    /// </summary>

    public static readonly String SinusoidLayoutSinusoidDrawn =
        FirstChar + "SLSinusoidDrawn";


    //*************************************************************************
    //  Keys used by GridLayout
    //*************************************************************************

    /// <summary>
    /// Key added to the graph after it has been completely drawn.  The key's
    /// value is null.
    /// </summary>

    public static readonly String GridLayoutGridDrawn =
        FirstChar + "GLGridDrawn";


    //*************************************************************************
    //  Keys used by FruchtermanReingoldLayout
    //*************************************************************************

    /// <summary>
    /// Key added to the graph to tell the layout to randomize the locations
    /// of only those vertices whose location is set to
    /// LayoutBase.RandomizeThisLocation.  If this key is not present, all
    /// locations are randomized.  The key's value is null.
    /// </summary>

    public static readonly String FruchtermanReingoldLayoutSelectivelyRandomize
        = FirstChar + "FRSel";

    /// <summary>
    /// Key added to each vertex to temporarily store the vertex's Tag.
    /// </summary>

    public static readonly String FruchtermanReingoldLayoutTagStorage =
        FirstChar + "FRTag";


    //*************************************************************************
    //  Keys used by SugiyamaLayout, SugiyamaVertexDrawer, and
    //  SugiyamaEdgeDrawer
    //*************************************************************************

    /// <summary>
    /// SugiyamaLayout uses VertexDrawer.Radius as an initial radius for the
    /// vertex circles, then computes an actual radius that probably differs
    /// from the initial value.  It stores the computed radius in the IGraph's
    /// metadata in a key named SugiyamaComputedRadius  The type of the key's
    /// value is Single.
    /// </summary>

    public static readonly String SugiyamaComputedRadius =
        FirstChar + "SgComputedRadius";


    /// <summary>
    /// For each edge, SugiyamaLayout computes a curve that extends from the
    /// edge's first vertex to some point near the second vertex, then computes
    /// an endpoint on the second vertex's circle.  It stores an array of curve
    /// points in the edge's metadata in a key named SugiyamaCurvePoints.  The
    /// type of the key's value is PointF[].  The curve points should be drawn
    /// with code similar to the following:
    ///
    /// <code>
    ///
    /// GraphicsPath oGraphicsPath = new GraphicsPath();
    /// 
    /// oGraphicsPath.AddBeziers(aoCurvePoints);
    /// 
    /// oGraphics.DrawPath(oPen, oGraphicsPath);
    ///
    /// </code>
    ///
    /// </summary>

    public static readonly String SugiyamaCurvePoints =
        FirstChar + "SgCurvePoints";


    /// <summary>
    /// For each edge, SugiyamaLayout computes an endpoint on the second
    /// vertex's circle and stores it in the edge's metadata in a key named
    /// SugiyamaEndpoint.  The type of the key's value is PointF.  The line
    /// from the last curve point to the endpoint should be drawn with code
    /// similar to the following:
    ///
    /// <code>
    ///
    /// oGraphics.DrawLine(oPen,
    ///     aoCurvePoints[aoCurvePoints.Length - 1],
    ///     oEndpoint
    ///     );
    ///
    /// </code>
    ///
    /// </summary>

    public static readonly String SugiyamaEndpoint =
        FirstChar + "SgEndpoint";


    /// <summary>
    /// SugiyamaLayout uses SugiyamaGleeNode to temporarily store a NodeXL
    /// vertex's corresponding GLEE node in the NodeXL vertex's metadata.  The
    /// type of the key's value is Microsoft.Glee.Node.
    /// </summary>

    public static readonly String SugiyamaGleeNode =
        FirstChar + "SgGleeNode";

    /// <summary>
    /// SugiyamaLayout uses SugiyamaGleeEdge to temporarily store a NodeXL
    /// edge's corresponding GLEE edge in the NodeXL edge's metadata.  The type
    /// of the key's value is Microsoft.Glee.Edge.
    /// </summary>

    public static readonly String SugiyamaGleeEdge =
        FirstChar + "SgGleeEdge";


    //*************************************************************************
    //  Keys used by DraggedVertices
    //*************************************************************************

    /// <summary>
    /// The Visualization.Wpf.DraggedVertices class adds this key to each
    /// selected vertex before it is dragged.  The key's value is the vertex's
    /// original location, as a PointF.  The key is removed when the drag
    /// completes.
    /// </summary>

    public static readonly String DraggedVerticesOriginalLocation =
        FirstChar + "DVOL";


    //*************************************************************************
    //  Keys used by any application
    //*************************************************************************

    /// <summary>
    /// Key added to a vertex or edge to "mark" it.  The key's value is true or
    /// false.  The meaning of marking is up to the application.  None of the
    /// NodeXL core or visualization components pay attention to this key.
    /// </summary>

    public static readonly String Marked =
        FirstChar + "M";


    //*************************************************************************
    //  Keys used by the ExcelTemplate application
    //*************************************************************************

    /// <summary>
    /// Key added to a vertex to add custom menu items to the vertex's
    /// context menu in the graph.  The key's value is an array of
    /// KeyValue&lt;String,String&gt;.  For each element, the key is the
    /// custom menu item text and the value is the custom menu item action.
    /// </summary>

    public static readonly String CustomContextMenuItems =
        FirstChar + "CMI";
}


//*****************************************************************************
//  Enum: VisibilityKeyValue
//
/// <summary>
/// Values used by the <see cref="ReservedMetadataKeys.Visibility" /> reserved
/// metadata key.
/// </summary>
///
/// <remarks>
/// Setting the <see cref="ReservedMetadataKeys.Visibility" /> key on a vertex
/// or edge to one of these values controls the visibility of the vertex or
/// edge.
/// </remarks>
//*****************************************************************************

public enum
VisibilityKeyValue
{
    /// <summary>
    /// The vertex or edge is drawn normally.
    /// </summary>

    Visible,

    /// <summary>
    /// The vertex or edge is not drawn, even though it participated in the
    /// layout.
    ///
    /// <para>
    /// Any <see cref="ReservedMetadataKeys.PerAlpha" /> key is ignored.
    /// </para>
    ///
    /// </summary>

    Hidden,

    /// <summary>
    /// The vertex or edge is drawn, but with a reduced alpha to indicate that
    /// it has been filtered with a dynamic filter.
    ///
    /// <para>
    /// Any <see cref="ReservedMetadataKeys.PerAlpha" /> key is ignored.
    /// </para>
    ///
    /// </summary>

    Filtered,
}

}
