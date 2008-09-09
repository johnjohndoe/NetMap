
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NetMap.Core
{
//*****************************************************************************
//  Class: ReservedMetadataKeys
//
/// <summary>
/// Contains metadata keys that are reserved for internal use by the NetMap
/// system.
/// </summary>
///
/// <remarks>
/// The reserved metadata keys defined within this class can't be used by
/// application code.
///
/// <para>
/// See <see cref="IMetadataProvider" /> for information on the metadata scheme
/// used by NetMap.
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
    //  Keys used by NetMapControl
    //*************************************************************************

	/// <summary>
	/// Key to pass to <see cref="IMetadataProvider.SetValue" /> to set a
    /// vertex's tooltip when using the NetMapControl.  The value must be a
    /// String.
	///
	/// <para>
	/// For tooltips to be visible, you must set NetMapControl.UseToolTips to
	/// true.  The default is false.
	/// </para>
	///
	/// </summary>

	public static readonly String ToolTip =
		FirstChar + "CCTT";


    //*************************************************************************
    //  Keys used by MultiSelectionGraphDrawer
    //*************************************************************************

	/// <summary>
	/// Key to pass to <see cref="IMetadataProvider.SetValue" /> to select a
	/// vertex or edge when the MultiSelectionGraphDrawer is being used.  The
	/// value is null.
	/// </summary>

	public static readonly String IsSelected =
		FirstChar + "S";


    //*************************************************************************
    //  Keys used by VertexDrawer
    //*************************************************************************

	/// <summary>
	/// Key added to a vertex to aid in hit-testing.  The value is the
	/// Microsoft.NetMap.Visualization.HitTestArea object that defines the area
	/// occupied by the vertex.
	///
	/// <para>
	/// Hiding a vertex by adding the <see cref="Hide" /> key to it
	/// automatically clears the <see cref="HitTestArea" /> key.  This prevents
	/// a hidden vertex from responding positively to a hit test.
	/// </para>
	///
	/// </summary>

	public static readonly String HitTestArea =
		FirstChar + "VDHitTestArea";


    //*************************************************************************
    //  Keys used by VertexDrawer and EdgeDrawer
    //*************************************************************************

	/// <summary>
	/// Key added to a vertex or edge to hide it.  The value is null.
	///
	/// <para>
	/// Hiding a vertex by adding this key to it automatically clears any
	/// <see cref="HitTestArea" /> key that might have been set on the vertex.
	/// This prevents a hidden vertex from responding positively to a hit test.
	/// </para>
	///
	/// </summary>

	public static readonly String Hide =
		FirstChar + "VEHide";


    //*************************************************************************
    //  Keys used by PerVertexDrawer and PerEdgeDrawer
    //*************************************************************************

	/// <summary>
	/// Key added to a vertex or edge to force it to be a specified color.  The
	/// key's value is a Color.
	/// </summary>

	public static readonly String PerColor =
		FirstChar + "PDColor";

	/// <summary>
	/// Key added to a vertex or edge to force it to be a specified
	/// transparency.  The key's value is an Int32 between 0 (transparent) and
	/// 255 (opaque).
	/// </summary>

	public static readonly String PerAlpha =
		FirstChar + "PDAlpha";


    //*************************************************************************
    //  Keys used by PerVertexDrawer
    //*************************************************************************

	/// <summary>
	/// Key added to a vertex to force it to be a specified shape.  The key's
	/// value is a VertexDrawer.VertexShape.
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


    //*************************************************************************
    //  Keys used by PerVertexWithImageDrawer
    //*************************************************************************

	/// <summary>
	/// Key added to a vertex to force it to be drawn as an image instead of a
	/// shape.  The key's value is a System.Drawing.Image.
	/// </summary>

	public static readonly String PerVertexImage =
		FirstChar + "PVIDImage";


    //*************************************************************************
    //  Keys used by PerVertexWithLabelDrawer
    //*************************************************************************

	/// <summary>
	/// Key added to a vertex to force it to be drawn as a rectangle containing
	/// a text label instead of a shape.  The key's value is a System.String.
	/// <see cref="PerVertexPrimaryLabel" /> and <see
	/// cref="PerVertexSecondaryLabel" /> may be specified in any combination.
	/// </summary>

	public static readonly String PerVertexPrimaryLabel =
		FirstChar + "PVLDPrimaryLabel";

	/// <summary>
	/// Key added to a vertex to force it to have a specified fill color.  The
	/// key's value is a Color.  Used only if <see
	/// cref="PerVertexPrimaryLabel" /> is specified.
	/// </summary>

	public static readonly String PerVertexPrimaryLabelFillColor =
		FirstChar + "PVLDPrimaryLabelFillColor";


	/// <summary>
	/// Key added to a vertex by PerVertexWithLabelDrawer.PreDrawVertex when a
	/// primary label has been specified.  The value is a
	/// PerVertexWithLabelDrawer.PrimaryLabelDrawInfo object.
	/// </summary>

	public static readonly String PerVertexPrimaryLabelDrawInfo =
		FirstChar + "PVLDPrimaryLabelDrawInfo";

	/// <summary>
	/// Key added to a vertex to force a text label to be drawn near it.  The
	/// key's value is a System.String.  <see cref="PerVertexPrimaryLabel" />
	/// and <see cref="PerVertexSecondaryLabel" /> may be specified in any
	/// combination.
	/// </summary>

	public static readonly String PerVertexSecondaryLabel =
		FirstChar + "PVLDSecondaryLabel";


    //*************************************************************************
    //  Keys used by several "per-vertex" drawers
    //*************************************************************************

	/// <summary>
	/// Key added to a vertex to specify which vertex drawer should be used for
	/// the vertex.
	/// </summary>

	public static readonly String PerVertexDrawerPrecedence =
		FirstChar + "PVDrawerPrecedence";


    //*************************************************************************
    //  Keys used by PerEdgeDrawer
    //*************************************************************************

	/// <summary>
	/// Key added to an edge to force it to be a specified width.  The key's
	/// value is an Int32.
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
	/// Key added to the graph after it has been completely drawn.  The value
	/// is null.
	/// </summary>

	public static readonly String CircleLayoutCircleDrawn =
		FirstChar + "CLCircleDrawn";


    //*************************************************************************
    //  Keys used by SpiralLayout
    //*************************************************************************

	/// <summary>
	/// Key added to the graph after it has been completely drawn.  The value
	/// is null.
	/// </summary>

	public static readonly String SpiralLayoutSpiralDrawn =
		FirstChar + "SLSpiralDrawn";


    //*************************************************************************
    //  Keys used by SinusoidLayout
    //*************************************************************************

	/// <summary>
	/// Key added to the graph after it has been completely drawn.  The value
	/// is null.
	/// </summary>

	public static readonly String SinusoidLayoutSinusoidDrawn =
		FirstChar + "SLSinusoidDrawn";


    //*************************************************************************
    //  Keys used by GridLayout
    //*************************************************************************

	/// <summary>
	/// Key added to the graph after it has been completely drawn.  The value
	/// is null.
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
	/// locations are randomized.  The value is null.
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
	/// SugiyamaLayout uses SugiyamaGleeNode to temporarily store a NetMap
	/// vertex's corresponding GLEE node in the NetMap vertex's metadata.  The
	/// type of the key's value is Microsoft.Glee.Node.
	/// </summary>

	public static readonly String SugiyamaGleeNode =
		FirstChar + "SgGleeNode";

	/// <summary>
	/// SugiyamaLayout uses SugiyamaGleeEdge to temporarily store a NetMap
	/// edge's corresponding GLEE edge in the NetMap edge's metadata.  The type
	/// of the key's value is Microsoft.Glee.Edge.
	/// </summary>

	public static readonly String SugiyamaGleeEdge =
		FirstChar + "SgGleeEdge";


    //*************************************************************************
    //  Keys used by GraphInfoViewer
    //*************************************************************************

	/// <summary>
	/// The DesktopApplication.GraphInfoViewer adds this key to each vertex and
	/// edge in the graph.  The value is the ListViewItem in the
	/// GraphInfoViewer that corresponds to the vertex or edge.
	/// </summary>

	public static readonly String GraphInfoViewerListViewItem =
		FirstChar + "GivItem";


    //*************************************************************************
    //  Keys used by any application
    //*************************************************************************

	/// <summary>
	/// Key added to a vertex or edge to "mark" it.  The value is true or
	/// false.  The meaning of marking is up to the application.  None of the
	/// NetMap core or visualization components pay attention to this key.
	/// </summary>

	public static readonly String Marked =
		FirstChar + "M";


    //*************************************************************************
    //  Keys used by the ExcelTemplate application
    //*************************************************************************

	/// <summary>
	/// Key added to a vertex to add custom menu items to the vertex's
	/// context menu in the graph.  The value is an array of
	/// KeyValue&lt;String,String&gt;.  For each element, the key is the
    /// custom menu item text and the value is the custom menu item action.
	/// </summary>

	public static readonly String CustomContextMenuItems =
		FirstChar + "CMI";
}

}
