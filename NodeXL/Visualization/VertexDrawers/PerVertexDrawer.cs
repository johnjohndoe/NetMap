
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//  Class: PerVertexDrawer
//
/// <summary>
///	Draws a vertex as a shape.  Allows per-vertex customizations.
/// </summary>
///
/// <remarks>
/// By default, this class delegates vertex drawing to its base class.  If you
/// add appropriate metadata to a vertex, however, this class uses the metadata
/// to customize the appearance of that vertex.
///
/// <para>
/// A selected vertex is always drawn using <see
/// cref="VertexDrawer.SelectedColor" />.  If an unselected vertex has the <see
/// cref="ReservedMetadataKeys.PerColor" /> key, the vertex is drawn
/// using the <see cref="Color" /> specified by the key's value.  If an
/// unselected vertex does not have this key, it is drawn using <see
/// cref="VertexDrawer.Color" />.
/// </para>
///
/// <para>
/// If a vertex has the <see cref="ReservedMetadataKeys.PerVertexShape" /> key,
/// the vertex's shape is set to the <see cref="VertexDrawer.VertexShape" />
/// specified by the key's value.
/// </para>
///
/// <para>
/// If a vertex has the <see cref="ReservedMetadataKeys.PerVertexRadius" />
/// key, the vertex's radius is set to the <see cref="Single" /> specified by
/// the key's value.
/// </para>
///
/// </remarks>
///
/// <seealso cref="VertexDrawer" />
//*****************************************************************************

public class PerVertexDrawer : VertexDrawer
{
    //*************************************************************************
    //  Constructor: PerVertexDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="PerVertexDrawer" /> class.
    /// </summary>
    //*************************************************************************

    public PerVertexDrawer()
    {
		// (Do nothing.)

		// AssertValid();
    }

    //*************************************************************************
    //  Method: GetActualShape()
    //
    /// <summary>
    /// Gets the actual shape to use for a vertex.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to get the actual shape for.
    /// </param>
    ///
	/// <returns>
	/// The actual shape to use.
	/// </returns>
    ///
    /// <remarks>
	/// See the class comments for details on how the actual shape is
	/// determined.
    /// </remarks>
    //*************************************************************************

    public override VertexShape
    GetActualShape
    (
        IVertex vertex
    )
	{
		AssertValid();

		Object oPerVertexShapeAsObject;

		if ( vertex.TryGetValue(ReservedMetadataKeys.PerVertexShape,
			typeof(VertexShape), out oPerVertexShapeAsObject) )
		{
			// Use the per-vertex shape instead of the base-class shape.

			return ( (VertexShape)oPerVertexShapeAsObject );
		}

		return ( base.GetActualShape(vertex) );
	}

    //*************************************************************************
    //  Method: GetActualRadius()
    //
    /// <summary>
    /// Gets the actual radius to use for a vertex.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to get the actual radius for.
    /// </param>
    ///
	/// <returns>
	/// The actual radius to use.
	/// </returns>
    ///
    /// <remarks>
	/// See the class comments for details on how the actual radius is
	/// determined.
    /// </remarks>
    //*************************************************************************

    public override Single
    GetActualRadius
    (
        IVertex vertex
    )
	{
		AssertValid();

		Object oRadius;

		if ( vertex.TryGetValue(ReservedMetadataKeys.PerVertexRadius,
            typeof(Single), out oRadius) )
		{
			Single fRadius = (Single)oRadius;

			if (fRadius < MinimumRadius || fRadius > MaximumRadius)
			{
				throw new FormatException( String.Format(

					"{0}: The vertex with the ID {1} has an out-of-range"
					+ " {2} value.  Valid values are between {3} and {4}."
					,
					this.ClassName,
					vertex.ID,
					ReservedMetadataKeys.PerVertexRadius,
					MinimumRadius,
					MaximumRadius
					) );
			}

			return (fRadius);
		}

		return ( base.GetActualRadius(vertex) );
	}

    //*************************************************************************
    //  Method: GetActualColor()
    //
    /// <summary>
    /// Gets the actual color to use for a vertex.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to get the actual color for.
    /// </param>
    ///
    /// <param name="showAsSelected">
    /// true if <paramref name="vertex" /> should be shown as selected.
    /// </param>
    ///
	/// <returns>
	/// The actual color to use.
	/// </returns>
    ///
    /// <remarks>
	/// See the class comments for details on how the actual color is
	/// determined.
    /// </remarks>
    //*************************************************************************

    protected override Color
    GetActualColor
    (
        IVertex vertex,
		Boolean showAsSelected
    )
	{
		AssertValid();

		// Start with the color determined by the base class.

		Color oPerVertexColor = base.GetActualColor(vertex, showAsSelected);

		if (showAsSelected)
		{
			// The selected color is always determined by the base class.

			return (oPerVertexColor);
		}

		// Check for a per-vertex color.

		Object oPerVertexColorAsObject;

		if ( vertex.TryGetValue(ReservedMetadataKeys.PerColor, typeof(Color),
			out oPerVertexColorAsObject) )
		{
			// Use the per-vertex color instead of the base-class color.

			oPerVertexColor = (Color)oPerVertexColorAsObject;
		}

		// Check for a per-vertex alpha.

		Int32 iPerVertexAlpha;

		if ( TryGetPerAlpha(vertex, out iPerVertexAlpha) )
		{
			// Use the per-vertex alpha.

			oPerVertexColor = Color.FromArgb(iPerVertexAlpha, oPerVertexColor);
		}

		return (oPerVertexColor);
	}

    //*************************************************************************
    //  Method: PreDrawVertexCore()
    //
    /// <summary>
    /// Prepares to draw a vertex.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex that will eventually be drawn.
    /// </param>
    ///
    /// <param name="drawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
    ///
    /// <remarks>
	/// After a graph is laid out but before its edges or vertices are drawn,
	/// this method gets called repeatedly, once for each of the graph's
	/// vertices.  The implementation can use this method to perform any
	/// pre-drawing calculations it needs.  It can also change the <see
	/// cref="IVertex.Location" /> of <paramref name="vertex" /> if the layout
	/// has located the vertex in a place where it would get clipped by the
	/// graph rectangle if it weren't moved.
	///
	/// <para>
	/// The arguments have already been checked for validity.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    protected override void
    PreDrawVertexCore
    (
        IVertex vertex,
		DrawContext drawContext
    )
	{
		Debug.Assert(vertex != null);
		Debug.Assert(drawContext != null);
		AssertValid();

		// Adjust the vertex location so it doesn't get clipped.

		Single fActualRadius = GetActualRadius(vertex);
		PointF oLocation = vertex.Location;

		RectangleF oVertexBounds = RectangleF.FromLTRB(
			oLocation.X - fActualRadius,
			oLocation.Y - fActualRadius,
			oLocation.X + fActualRadius,
			oLocation.Y + fActualRadius
			);

		AdjustVertexLocation( vertex, drawContext,
			Rectangle.Ceiling(oVertexBounds) );
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

		// (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
