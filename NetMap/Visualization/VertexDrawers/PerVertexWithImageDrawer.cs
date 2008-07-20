
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//  Class: PerVertexWithImageDrawer
//
/// <summary>
///	Draws a vertex as an image.
/// </summary>
///
/// <remarks>
/// By default, this class delegates vertex drawing to its base class.  If you
/// add appropriate metadata to a vertex, however, this class uses the metadata
/// to draw the vertex as an image instead of a shape.
///
/// <para>
/// If a vertex has the <see cref="ReservedMetadataKeys.PerVertexImage" /> key,
/// the vertex is drawn using the <see cref="Image" /> specified by the key's
/// value.
/// </para>
///
/// <para>
/// To force this class to ignore the <see
/// cref="ReservedMetadataKeys.PerVertexImage" /> key and delegate drawing to
/// the base class, add a <see
/// cref="ReservedMetadataKeys.PerVertexDrawerPrecedence" /> key and set its
/// value to anything except <see cref="VertexDrawerPrecedence.Image" />.
/// </para>
///
/// </remarks>
///
/// <seealso cref="PerVertexDrawer" />
//*****************************************************************************

public class PerVertexWithImageDrawer : PerVertexDrawer
{
    //*************************************************************************
    //  Constructor: PerVertexWithImageDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="PerVertexWithImageDrawer" /> class.
    /// </summary>
    //*************************************************************************

    public PerVertexWithImageDrawer()
    {
		// (Do nothing.)

		// AssertValid();
    }

	//*************************************************************************
	//	Method: TryGetImage()
	//
	/// <summary>
	/// Attempts to get an image for a vertex.
	/// </summary>
	///
	/// <param name="vertex">
	/// Vertex to get an image for.
	/// </param>
	///
	/// <param name="image">
	/// Where the vertex's image gets stored if true is returned.
	/// </param>
	///
	/// <returns>
	/// true if <paramref name="vertex" /> should be drawn as an image.
	/// </returns>
	//*************************************************************************

	public static Boolean
	TryGetImage
	(
		IVertex vertex,
		out Image image
	)
	{
		Debug.Assert(vertex != null);

		image = null;

		// To draw the vertex as an image, the vertex must have a
		// ReservedMetadataKeys.PerVertexImage value, and if it has a
		// ReservedMetadataKeys.PerVertexDrawerPrecedence key, the value must
		// be VertexDrawerPrecedence.Image.

		Object oImageAsObject;

		if ( !vertex.TryGetValue(ReservedMetadataKeys.PerVertexImage,
			typeof(Image), out oImageAsObject) )
		{
            return (false);;
		}

        Object oVertexDrawerPrecedence;

		if (
			vertex.TryGetValue(ReservedMetadataKeys.PerVertexDrawerPrecedence,
				typeof(VertexDrawerPrecedence), out oVertexDrawerPrecedence)
			&&
			(VertexDrawerPrecedence)oVertexDrawerPrecedence !=
				VertexDrawerPrecedence.Image
			)
		{
			return (false);
		}

		image = (Image)oImageAsObject;

		return (true);
	}

    //*************************************************************************
    //  Method: GetImageRectangle()
    //
    /// <summary>
    /// Gets the rectangle a vertex's image is drawn within.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to get a rectangle for.
    /// </param>
    ///
    /// <param name="image">
	/// The vertex's image.
    /// </param>
    ///
    /// <returns>
	/// The <see cref="Rectangle" /> the image should be drawn within.
    /// </returns>
    //*************************************************************************

    public static Rectangle
    GetImageRectangle
    (
        IVertex vertex,
		Image image
    )
	{
		Debug.Assert(vertex != null);
		Debug.Assert(image != null);

		// The rectangle should be the same size as the image in pixels.  It
		// should be centered on the vertex.

		PointF oVertexLocation = vertex.Location;
		Size oImageSize = image.Size;

		Rectangle oRectangle =
			new Rectangle(Point.Round(oVertexLocation), oImageSize);

		oRectangle.Offset(-oImageSize.Width / 2, -oImageSize.Height / 2);

		return (oRectangle);
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

		Image oImage;

		// Determine whether the vertex should be drawn as an image.

		if ( !TryGetImage(vertex, out oImage) )
		{
			// No.  Defer to the base class.

			base.PreDrawVertexCore(vertex, drawContext);

            return;
		}

		Rectangle oImageRectangle = GetImageRectangle(vertex, oImage);

		// Adjust the vertex location so the image rectangle doesn't get
		// clipped.

		AdjustVertexLocation(vertex, drawContext, oImageRectangle);
	}

    //*************************************************************************
    //  Method: DrawVertexCore()
    //
    /// <summary>
    /// Draws a vertex.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to draw.
    /// </param>
    ///
    /// <param name="drawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
	///
    /// <returns>
	/// The vertex's bounding rectangle.
    /// </returns>
    ///
    /// <remarks>
    /// This method gets called repeatedly while a graph is being drawn, once
	/// for each of the graph's vertices.  The <see cref="IVertex.Location" />
	///	property on all of the graph's vertices is set by ILayout.<see
	///	cref="ILayout.LayOutGraph" /> before this method is called.
	///
	/// <para>
	/// The arguments have already been checked for validity.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    protected override Rectangle
    DrawVertexCore
    (
        IVertex vertex,
		DrawContext drawContext
    )
	{
		Debug.Assert(vertex != null);
		Debug.Assert(drawContext != null);
		AssertValid();

		Image oImage;

		// Determine whether the vertex should be drawn as an image.

		if ( !TryGetImage(vertex, out oImage) )
		{
			// No.  Defer to the base class.

			return ( base.DrawVertexCore(vertex, drawContext) );
		}

		// Draw the image without scaling, centered on the vertex.

		Rectangle oImageRectangle = GetImageRectangle(vertex, oImage);

		Graphics oGraphics = drawContext.Graphics;

		oGraphics.DrawImage(oImage, oImageRectangle);

		// Draw an outline around the image.

		DrawVertexOutline( drawContext, oImageRectangle,
			VertexShouldBeDrawnSelected(vertex) );

		// Add a hit-test area to the vertex's metadata.  This gets used by
		// VertexContainsPoint().

		SetRectangularHitTestArea(vertex, oImageRectangle);

		return (oImageRectangle);
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
