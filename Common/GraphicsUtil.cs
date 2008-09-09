
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.GraphicsLib
{
//*****************************************************************************
//	Class: GraphicsUtil
//
/// <summary>
///	Set of static methods that perform graphics operations not available
///	directly through GDI+.
/// </summary>
///
/// <remarks>
///	Do not try to instantiate an object of this type.  All methods are static.
///	</remarks>
//*****************************************************************************

internal class GraphicsUtil
{
	//*************************************************************************
	//	Constructor: GraphicsUtil()
	//
	/// <summary>
	/// Do not use this contructor.
	/// </summary>
	///
	/// <remarks>
	/// All methods on this class are static, so there is no need to create a
	/// GraphicsUtil object.
	/// </remarks>
	//*************************************************************************

	private GraphicsUtil()
	{
		// (Do nothing.)
	}

	//*************************************************************************
	//	Method: PixelsToPoints()
	//
	/// <summary>
	/// Converts a height and width from pixels to points.
	/// </summary>
	///
	/// <param name="oGraphics">
	///	Object that will do the conversion.
	/// </param>
	///
	/// <param name="fWidthPx">
	///	Width in pixels.  Must be greater than or equal to zero.
	/// </param>
	///
	/// <param name="fHeightPx">
	///	Height in pixels.  Must be greater than or equal to zero.
	/// </param>
	///
	/// <param name="fWidthPt">
	///	Where the width in points gets stored.
	/// </param>
	///
	/// <param name="fHeightPt">
	///	Where the height in points gets stored.
	/// </param>
	//*************************************************************************

	public static void
	PixelsToPoints
	(
		Graphics oGraphics,
		Single fWidthPx,
		Single fHeightPx,
		out Single fWidthPt,
		out Single fHeightPt
	)
	{
		ConvertPixelsAndPoints(true, oGraphics, fWidthPx, fHeightPx,
			out fWidthPt, out fHeightPt);
	}

	//*************************************************************************
	//	Method: PointsToPixels()
	//
	/// <summary>
	/// Converts a height and width from points to pixels.
	/// </summary>
	///
	/// <param name="oGraphics">
	///	Object that will do the conversion.
	/// </param>
	///
	/// <param name="fWidthPt">
	///	Width in points.  Must be greater than or equal to zero.
	/// </param>
	///
	/// <param name="fHeightPt">
	///	Height in points.  Must be greater than or equal to zero.
	/// </param>
	///
	/// <param name="fWidthPx">
	///	Where the width in pixels gets stored.
	/// </param>
	///
	/// <param name="fHeightPx">
	///	Where the height in pixels gets stored.
	/// </param>
	///
	/// <remarks>
	/// There are two versions of this method.  This version converts to
	/// floating-point pixels.  The other converts to integer pixels.
	/// </remarks>
	//*************************************************************************

	public static void
	PointsToPixels
	(
		Graphics oGraphics,
		Single fWidthPt,
		Single fHeightPt,
		out Single fWidthPx,
		out Single fHeightPx
	)
	{
		ConvertPixelsAndPoints(false, oGraphics, fWidthPt, fHeightPt,
			out fWidthPx, out fHeightPx);
	}

	//*************************************************************************
	//	Method: PointsToPixels()
	//
	/// <summary>
	/// Converts a height and width from points to pixels.
	/// </summary>
	///
	/// <param name="oGraphics">
	///	Object that will do the conversion.
	/// </param>
	///
	/// <param name="fWidthPt">
	///	Width in points.  Must be greater than or equal to zero.
	/// </param>
	///
	/// <param name="fHeightPt">
	///	Height in points.  Must be greater than or equal to zero.
	/// </param>
	///
	/// <param name="iWidthPx">
	///	Where the width in pixels gets stored.
	/// </param>
	///
	/// <param name="iHeightPx">
	///	Where the height in pixels gets stored.
	/// </param>
	///
	/// <remarks>
	/// There are two versions of this method.  This version converts to
	/// integer pixels.  The other converts to floating-point pixels.
	/// </remarks>
	//*************************************************************************

	public static void
	PointsToPixels
	(
		Graphics oGraphics,
		Single fWidthPt,
		Single fHeightPt,
		out Int32 iWidthPx,
		out Int32 iHeightPx
	)
	{
		Single fWidthPx, fHeightPx;

		GraphicsUtil.PointsToPixels(oGraphics, fWidthPt, fHeightPt,
			out fWidthPx, out fHeightPx);

		iWidthPx = (Int32)(fWidthPx + 0.5);
		iHeightPx = (Int32)(fHeightPx + 0.5);
	}

	//*************************************************************************
	//	Method: DrawCircle()
	//
	/// <summary>
	/// Draws a circle defined by a center point and radius.
	/// </summary>
	///
	/// <param name="oGraphics">
	///	Object to draw on.
	/// </param>
	///
	/// <param name="oPen">
	///	Pen to draw with.
	/// </param>
	///
	/// <param name="fXCenter">
	/// x-coordinate of the circle's center.
	/// </param>
	///
	/// <param name="fYCenter">
	/// y-coordinate of the circle's center.
	/// </param>
	///
	/// <param name="fRadius">
	/// Radius of the circle.
	/// </param>
	//*************************************************************************

	public static void
	DrawCircle
	(
		Graphics oGraphics,
		Pen oPen,
		Single fXCenter,
		Single fYCenter,
		Single fRadius
	)
	{
		Debug.Assert(oGraphics != null);
		Debug.Assert(oPen != null);
		Debug.Assert(fRadius >= 0);

		Single fDiameter = 2F * fRadius;

		oGraphics.DrawEllipse(oPen, fXCenter - fRadius, fYCenter - fRadius,
			fDiameter, fDiameter);
	}

	//*************************************************************************
	//	Method: FillCircle()
	//
	/// <summary>
	/// Fills the interior of a circle defined by a center point and radius.
	/// </summary>
	///
	/// <param name="oGraphics">
	///	Object to draw on.
	/// </param>
	///
	/// <param name="oBrush">
	///	Brush to draw with.
	/// </param>
	///
	/// <param name="fXCenter">
	/// x-coordinate of the circle's center.
	/// </param>
	///
	/// <param name="fYCenter">
	/// y-coordinate of the circle's center.
	/// </param>
	///
	/// <param name="fRadius">
	/// Radius of the circle.
	/// </param>
	//*************************************************************************

	public static void
	FillCircle
	(
		Graphics oGraphics,
		Brush oBrush,
		Single fXCenter,
		Single fYCenter,
		Single fRadius
	)
	{
		Debug.Assert(oGraphics != null);
		Debug.Assert(fRadius >= 0);

		Single fDiameter = 2F * fRadius;

		oGraphics.FillEllipse(oBrush, fXCenter - fRadius, fYCenter - fRadius,
			fDiameter, fDiameter);
	}

	//*************************************************************************
	//	Method: FillCircle3D()
	//
	/// <summary>
	/// Fills the interior of a circle defined by a center point and radius
	/// using 3-D shading.
	/// </summary>
	///
	/// <param name="oGraphics">
	///	Object to draw on.
	/// </param>
	///
	/// <param name="oColor">
	///	Color to use.
	/// </param>
	///
	/// <param name="fXCenter">
	/// x-coordinate of the circle's center.
	/// </param>
	///
	/// <param name="fYCenter">
	/// y-coordinate of the circle's center.
	/// </param>
	///
	/// <param name="fRadius">
	/// Radius of the circle.  Must be greater than zero.
	/// </param>
	///
	/// <remarks>
	///	The circle is shaded to make it look as if it's a 3-D sphere.
	/// </remarks>
	//*************************************************************************

	public static void
	FillCircle3D
	(
		Graphics oGraphics,
		Color oColor,
		Single fXCenter,
		Single fYCenter,
		Single fRadius
	)
	{
		Debug.Assert(oGraphics != null);
		Debug.Assert(fRadius > 0);

		// The following code is based on the BouncingGradientBrushBall.cs
		// example in "Programming Windows with C#," by Petzold.

		GraphicsPath oGraphicsPath = new GraphicsPath();

		// Add the circle to the path.

		RectangleF oRectangleF =
			SquareFromCenterAndHalfWidth(fXCenter, fYCenter, fRadius);

		oGraphicsPath.AddEllipse(oRectangleF);

		PathGradientBrush oPathGradientBrush =
			new PathGradientBrush(oGraphicsPath);

		// Specify white for the point 1/3 to the left and top of the circle's
		// bounding rectangle.

		oPathGradientBrush.CenterPoint = new PointF(
			oRectangleF.Left + oRectangleF.Width / 3F,
			oRectangleF.Top + oRectangleF.Height / 3F);

		oPathGradientBrush.CenterColor = Color.White;

		// Use the specified color for the single surround color.

		oPathGradientBrush.SurroundColors = new Color [] {oColor};

		// Fill the circle with the gradient brush.

		oGraphics.FillRectangle(oPathGradientBrush, oRectangleF);

		oPathGradientBrush.Dispose();
		oGraphicsPath.Dispose();
	}

	//*************************************************************************
	//	Method: DrawSquare()
	//
	/// <summary>
	/// Draws a square defined by a center point and half-width.
	/// </summary>
	///
	/// <param name="oGraphics">
	///	Object to draw on.
	/// </param>
	///
	/// <param name="oPen">
	///	Pen to draw with.
	/// </param>
	///
	/// <param name="fXCenter">
	/// x-coordinate of the square's center.
	/// </param>
	///
	/// <param name="fYCenter">
	/// y-coordinate of the square's center.
	/// </param>
	///
	/// <param name="fHalfWidth">
	/// One half the width of the square.
	/// </param>
	///
	/// <remarks>
	/// The square is specified as a center point and half-width to make this
	/// method compatible with <see cref="DrawCircle" />.
	/// </remarks>
	//*************************************************************************

	public static void
	DrawSquare
	(
		Graphics oGraphics,
		Pen oPen,
		Single fXCenter,
		Single fYCenter,
		Single fHalfWidth
	)
	{
		Debug.Assert(oGraphics != null);
		Debug.Assert(oPen != null);
		Debug.Assert(fHalfWidth >= 0);

		RectangleF oRectangleF = 
			SquareFromCenterAndHalfWidth(fXCenter, fYCenter, fHalfWidth);

		Rectangle oRectangle = RectangleFToRectangle(
			oRectangleF, (Int32)Math.Round(oPen.Width) );

		oGraphics.DrawRectangle(oPen, oRectangle);
	}

	//*************************************************************************
	//	Method: FillSquare()
	//
	/// <summary>
	/// Fills the interior of a square defined by a center point and
	/// half-width.
	/// </summary>
	///
	/// <param name="oGraphics">
	///	Object to draw on.
	/// </param>
	///
	/// <param name="oBrush">
	///	Brush to draw with.
	/// </param>
	///
	/// <param name="fXCenter">
	/// x-coordinate of the square's center.
	/// </param>
	///
	/// <param name="fYCenter">
	/// y-coordinate of the square's center.
	/// </param>
	///
	/// <param name="fHalfWidth">
	/// One half the width of the square.
	/// </param>
	///
	/// <remarks>
	/// The square is specified as a center point and half-width to make this
	/// method compatible with <see cref="FillCircle" />.
	/// </remarks>
	//*************************************************************************

	public static void
	FillSquare
	(
		Graphics oGraphics,
		Brush oBrush,
		Single fXCenter,
		Single fYCenter,
		Single fHalfWidth
	)
	{
		Debug.Assert(oGraphics != null);
		Debug.Assert(fHalfWidth >= 0);

		RectangleF oRectangleF = 
			SquareFromCenterAndHalfWidth(fXCenter, fYCenter, fHalfWidth);

		Rectangle oRectangle = RectangleFToRectangle(oRectangleF, 1);

		oGraphics.FillRectangle(oBrush, oRectangle);
	}

	//*************************************************************************
	//	Method: FillSquare3D()
	//
	/// <summary>
	/// Fills the interior of a square using 3-D shading.
	/// </summary>
	///
	/// <param name="oGraphics">
	///	Object to draw on.
	/// </param>
	///
	/// <param name="oColor">
	///	Color to use.
	/// </param>
	///
	/// <param name="fXCenter">
	/// x-coordinate of the square's center.
	/// </param>
	///
	/// <param name="fYCenter">
	/// y-coordinate of the square's center.
	/// </param>
	///
	/// <param name="fHalfWidth">
	/// One half the width of the square.
	/// </param>
	///
	/// <remarks>
	///	The square is shaded to make it look as if it's 3-D.
	///
	/// <para>
	/// The square is specified as a center point and half-width to make this
	/// method compatible with <see cref="FillCircle" />.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public static void
	FillSquare3D
	(
		Graphics oGraphics,
		Color oColor,
		Single fXCenter,
		Single fYCenter,
		Single fHalfWidth
	)
	{
		Debug.Assert(oGraphics != null);
		Debug.Assert(fHalfWidth >= 0);

		// The following code is based on the BouncingGradientBrushBall.cs
		// example in "Programming Windows with C#," by Petzold.

		GraphicsPath oGraphicsPath = new GraphicsPath();

		// Add the square to the path.

		RectangleF oRectangleF = 
			SquareFromCenterAndHalfWidth(fXCenter, fYCenter, fHalfWidth);

		oGraphicsPath.AddRectangle(oRectangleF);

		PathGradientBrush oPathGradientBrush =
			new PathGradientBrush(oGraphicsPath);

		// Specify white for the center point.

		oPathGradientBrush.CenterPoint = new PointF(fXCenter, fYCenter);
		oPathGradientBrush.CenterColor = Color.White;

		// Use the specified color for the single surround color.

		oPathGradientBrush.SurroundColors = new Color [] {oColor};

		// Fill the square with the gradient brush.

		oGraphics.FillRectangle(oPathGradientBrush, oRectangleF);

		oPathGradientBrush.Dispose();
		oGraphicsPath.Dispose();
	}

	//*************************************************************************
	//	Method: DrawDiamond()
	//
	/// <summary>
	/// Draws a diamond defined by a center point and half-width.
	/// </summary>
	///
	/// <param name="oGraphics">
	///	Object to draw on.
	/// </param>
	///
	/// <param name="oPen">
	///	Pen to draw with.
	/// </param>
	///
	/// <param name="fXCenter">
	/// x-coordinate of the diamond's center.
	/// </param>
	///
	/// <param name="fYCenter">
	/// y-coordinate of the diamond's center.
	/// </param>
	///
	/// <param name="fHalfWidth">
	/// One half the width of the diamond.
	/// </param>
	///
	/// <remarks>
	/// The diamond is specified as a center point and half-width to make this
	/// method compatible with <see cref="DrawCircle" />.
	/// </remarks>
	//*************************************************************************

	public static void
	DrawDiamond
	(
		Graphics oGraphics,
		Pen oPen,
		Single fXCenter,
		Single fYCenter,
		Single fHalfWidth
	)
	{
		Debug.Assert(oGraphics != null);
		Debug.Assert(oPen != null);
		Debug.Assert(fHalfWidth >= 0);

		oGraphics.DrawPolygon(oPen, DiamondFromCenterAndHalfWidth(
			fXCenter, fYCenter, fHalfWidth) );
	}

	//*************************************************************************
	//	Method: FillDiamond()
	//
	/// <summary>
	/// Fills a diamond defined by a center point and half-width.
	/// </summary>
	///
	/// <param name="oGraphics">
	///	Object to draw on.
	/// </param>
	///
	/// <param name="oBrush">
	///	Brush to draw with.
	/// </param>
	///
	/// <param name="fXCenter">
	/// x-coordinate of the diamond's center.
	/// </param>
	///
	/// <param name="fYCenter">
	/// y-coordinate of the diamond's center.
	/// </param>
	///
	/// <param name="fHalfWidth">
	/// One half the width of the diamond.
	/// </param>
	///
	/// <remarks>
	/// The diamond is specified as a center point and half-width to make this
	/// method compatible with <see cref="DrawCircle" />.
	/// </remarks>
	//*************************************************************************

	public static void
	FillDiamond
	(
		Graphics oGraphics,
		Brush oBrush,
		Single fXCenter,
		Single fYCenter,
		Single fHalfWidth
	)
	{
		Debug.Assert(oGraphics != null);
		Debug.Assert(oBrush != null);
		Debug.Assert(fHalfWidth >= 0);

		oGraphics.FillPolygon(oBrush, DiamondFromCenterAndHalfWidth(
			fXCenter, fYCenter, fHalfWidth) );
	}

	//*************************************************************************
	//	Method: DrawTriangle()
	//
	/// <summary>
	/// Draws an equilateral triangle defined by a center point and half-width.
	/// </summary>
	///
	/// <param name="oGraphics">
	///	Object to draw on.
	/// </param>
	///
	/// <param name="oPen">
	///	Pen to draw with.
	/// </param>
	///
	/// <param name="fXCenter">
	/// x-coordinate of the triangle's center.
	/// </param>
	///
	/// <param name="fYCenter">
	/// y-coordinate of the triangle's center.
	/// </param>
	///
	/// <param name="fHalfWidth">
	/// One half the width of the square that bounds the triangle.
	/// </param>
	///
	/// <remarks>
	/// The triangle is specified as a center point and half-width to make this
	/// method compatible with <see cref="DrawCircle" />.
	/// </remarks>
	//*************************************************************************

	public static void
	DrawTriangle
	(
		Graphics oGraphics,
		Pen oPen,
		Single fXCenter,
		Single fYCenter,
		Single fHalfWidth
	)
	{
		Debug.Assert(oGraphics != null);
		Debug.Assert(oPen != null);
		Debug.Assert(fHalfWidth >= 0);

		oGraphics.DrawPolygon(oPen, TriangleFromCenterAndHalfWidth(
			fXCenter, fYCenter, fHalfWidth) );
	}

	//*************************************************************************
	//	Method: FillTriangle()
	//
	/// <summary>
	/// Fills an equilateral triangle defined by a center point and half-width.
	/// </summary>
	///
	/// <param name="oGraphics">
	///	Object to draw on.
	/// </param>
	///
	/// <param name="oBrush">
	///	Brush to draw with.
	/// </param>
	///
	/// <param name="fXCenter">
	/// x-coordinate of the triangle's center.
	/// </param>
	///
	/// <param name="fYCenter">
	/// y-coordinate of the triangle's center.
	/// </param>
	///
	/// <param name="fHalfWidth">
	/// One half the width of the square that bounds the triangle.
	/// </param>
	///
	/// <remarks>
	/// The triangle is specified as a center point and half-width to make this
	/// method compatible with <see cref="DrawCircle" />.
	/// </remarks>
	//*************************************************************************

	public static void
	FillTriangle
	(
		Graphics oGraphics,
		Brush oBrush,
		Single fXCenter,
		Single fYCenter,
		Single fHalfWidth
	)
	{
		Debug.Assert(oGraphics != null);
		Debug.Assert(oBrush != null);
		Debug.Assert(fHalfWidth >= 0);

		oGraphics.FillPolygon(oBrush, TriangleFromCenterAndHalfWidth(
			fXCenter, fYCenter, fHalfWidth) );
	}

	//*************************************************************************
	//	Method: Clear()
	//
	/// <summary>
	/// Clears the entire drawing surface and fills it with the specified
	/// background color.
	/// </summary>
	///
	/// <param name="oGraphics">
	///	Object to draw on.
	/// </param>
	///
	/// <param name="oColor">
	/// <see cref="Color" /> structure that represents the background color of
	/// the drawing surface. 
	/// </param>
	///
	/// <remarks>
	/// This adds a GDI bug workaround to <see cref="Graphics.Clear" />.
	/// </remarks>
	//*************************************************************************

	public static void
	Clear
	(
		Graphics oGraphics,
		Color oColor
	)
	{
		Debug.Assert(oGraphics != null);

		try
		{
			oGraphics.Clear(oColor);
		}
		catch (System.Runtime.InteropServices.ExternalException)
		{
			// Ignore a known GDI bug sometimes encountered when running over
			// terminal services.  As of August 2007, details could be found
			// here:
			//
			// http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=235193
		}
	}

	//*************************************************************************
	//	Method: CreateRoundedRectangleGraphicsPath()
	//
	/// <summary>
	/// Creates a GraphicsPath that describes a rectangle with rounded corners.
	/// </summary>
	///
	/// <param name="oRectangle">
	///	Rectangle to add rounded corners to.
	/// </param>
	///
	/// <param name="iCornerRadius">
	/// Radius of the rectangle's corners.
	/// </param>
	///
	/// <returns>
	/// A new GraphicsPath.
	/// </returns>
	//*************************************************************************

	public static GraphicsPath
	CreateRoundedRectangleGraphicsPath
	(
		Rectangle oRectangle,
		Int32 iCornerRadius
	)
	{
		Debug.Assert(iCornerRadius > 0);

		GraphicsPath oGraphicsPath = new GraphicsPath();
		Int32 iCornerDiameter = 2 * iCornerRadius;

		Rectangle oRoundedCornerRectangle = new Rectangle( oRectangle.Location,
			new Size(iCornerDiameter, iCornerDiameter) );

		// Add the upper-left rounded corner.

		oGraphicsPath.AddArc(oRoundedCornerRectangle, 180, 90);

		// Add the upper-right rounded corner.

		oRoundedCornerRectangle.X = oRectangle.Right - iCornerDiameter;
		oGraphicsPath.AddArc(oRoundedCornerRectangle, 270, 90);

		// Add the lower-right rounded corner.

		oRoundedCornerRectangle.Y = oRectangle.Bottom - iCornerDiameter;
		oGraphicsPath.AddArc(oRoundedCornerRectangle, 0, 90);

		// Add the lower-left rounded corner.

		oRoundedCornerRectangle.X = oRectangle.Left;
		oGraphicsPath.AddArc(oRoundedCornerRectangle, 90, 90);

		oGraphicsPath.CloseFigure();

		return (oGraphicsPath);
	}

	//*************************************************************************
	//	Method: FillTextRectangle()
	//
	/// <summary>
	/// Fills the interior of a rectangle that will contain text.
	/// </summary>
	///
	/// <param name="oGraphics">
	///	Object to draw on.
	/// </param>
	///
	/// <param name="oRectangle">
	/// Rectangle to draw on.  If empty, this method does nothing.
	/// </param>
	///
	/// <param name="bTextIsSelected">
	///	true if the text is selected.
	/// </param>
	///
	/// <remarks>
	/// This method fills the interior of a rectangle with either the system
	/// window or system highlight color, depending on whether the text is
	/// selected.  Call this method before you draw the text.  When you draw
	/// the text, use SystemBrushes.HighlightText or SystemBrushes.WindowText
	/// as the text color.
	/// </remarks>
	//*************************************************************************

	public static void
	FillTextRectangle
	(
		Graphics oGraphics,
		Rectangle oRectangle,
		Boolean bTextIsSelected
	)
	{
		Debug.Assert(oGraphics != null);

		if (oRectangle.Width <= 0 || oRectangle.Height <= 0)
			return;

		oGraphics.FillRectangle(
			(bTextIsSelected ? SystemBrushes.Highlight : SystemBrushes.Window),
			oRectangle);
	}

	//*************************************************************************
	//	Method: RadiusToArea()
	//
	/// <summary>
	///	Returns the area of a circle given its radius.
	/// </summary>
	///
	/// <param name="dRadius">
	///	The circle's radius.
	/// </param>
	///
	/// <returns>
	///	The circle's area.
	/// </returns>
	//*************************************************************************

	public static Double
	RadiusToArea
	(
		Double dRadius
	)
	{
		Debug.Assert(dRadius >= 0);

		return (Math.PI * dRadius * dRadius);
	}

	//*************************************************************************
	//	Method: AreaToRadius()
	//
	/// <summary>
	///	Returns the radius of a circle given its area.
	/// </summary>
	///
	/// <param name="dArea">
	///	The circle's area.
	/// </param>
	///
	/// <returns>
	///	The circle's radius.
	/// </returns>
	//*************************************************************************

	public static Double
	AreaToRadius
	(
		Double dArea
	)
	{
		Debug.Assert(dArea >= 0);

		return ( Math.Sqrt(dArea / Math.PI) );
	}

	//*************************************************************************
	//	Method: SquareFromCenterAndHalfWidth()
	//
	/// <summary>
	/// Returns a square given a center point and half-width.
	/// </summary>
	///
	/// <param name="fXCenter">
	/// x-coordinate of the square's center.
	/// </param>
	///
	/// <param name="fYCenter">
	/// y-coordinate of the square's center.
	/// </param>
	///
	/// <param name="fHalfWidth">
	/// One half the width of the square.
	/// </param>
	///
	/// <returns>
	/// The specified square.
	/// </returns>
	//*************************************************************************

	public static RectangleF
	SquareFromCenterAndHalfWidth
	(
		Single fXCenter,
		Single fYCenter,
		Single fHalfWidth
	)
	{
		Debug.Assert(fHalfWidth >= 0);

		return (RectangleF.FromLTRB(
			fXCenter - fHalfWidth,
			fYCenter - fHalfWidth,
			fXCenter + fHalfWidth,
			fYCenter + fHalfWidth)
			);
	}

	//*************************************************************************
	//	Method: DiamondFromCenterAndHalfWidth()
	//
	/// <summary>
	/// Returns an array of points that define a diamond given a center point
	/// and half-width.
	/// </summary>
	///
	/// <param name="fXCenter">
	/// x-coordinate of the diamond's center.
	/// </param>
	///
	/// <param name="fYCenter">
	/// y-coordinate of the diamond's center.
	/// </param>
	///
	/// <param name="fHalfWidth">
	/// One half the width of the diamond.
	/// </param>
	///
	/// <returns>
	/// The specified diamond.
	/// </returns>
	//*************************************************************************

	public static PointF []
	DiamondFromCenterAndHalfWidth
	(
		Single fXCenter,
		Single fYCenter,
		Single fHalfWidth
	)
	{
		Debug.Assert(fHalfWidth >= 0);

		return ( new PointF [] {
			new PointF(fXCenter, fYCenter - fHalfWidth),
			new PointF(fXCenter + fHalfWidth, fYCenter),
			new PointF(fXCenter, fYCenter + fHalfWidth),
			new PointF(fXCenter - fHalfWidth, fYCenter),
			} );
	}

	//*************************************************************************
	//	Method: GetDiamondEdgeMidpoints()
	//
	/// <summary>
	/// Returns an array of points on the midpoints of a diamond's edges given
	/// a center point and half-width.
	/// </summary>
	///
	/// <param name="fXCenter">
	/// x-coordinate of the diamond's center.
	/// </param>
	///
	/// <param name="fYCenter">
	/// y-coordinate of the diamond's center.
	/// </param>
	///
	/// <param name="fHalfWidth">
	/// One half the width of the diamond.
	/// </param>
	///
	/// <returns>
	/// The midpoints of the specified diamond.
	/// </returns>
	//*************************************************************************

	public static PointF []
	GetDiamondEdgeMidpoints
	(
		Single fXCenter,
		Single fYCenter,
		Single fHalfWidth
	)
	{
		Debug.Assert(fHalfWidth >= 0);

		Single fQuarterWidth = fHalfWidth / 2F;

		return ( new PointF [] {

			// Midpoints on edges:

			new PointF(fXCenter + fQuarterWidth,
				fYCenter - fQuarterWidth),

			new PointF(fXCenter + fQuarterWidth,
				fYCenter + fQuarterWidth),

			new PointF(fXCenter - fQuarterWidth,
				fYCenter + fQuarterWidth),

			new PointF(fXCenter - fQuarterWidth,
				fYCenter - fQuarterWidth),
			} );
	}

	//*************************************************************************
	//	Method: TriangleFromCenterAndHalfWidth()
	//
	/// <summary>
	/// Returns an array of points that define an equilateral triangle given a
	/// center point and half-width.
	/// </summary>
	///
	/// <param name="fXCenter">
	/// x-coordinate of the triangle's center.
	/// </param>
	///
	/// <param name="fYCenter">
	/// y-coordinate of the triangle's center.
	/// </param>
	///
	/// <param name="fHalfWidth">
	/// One half the width of the square that bounds the triangle.
	/// </param>
	///
	/// <returns>
	/// The specified triangle.  The order of the points is top, lower-right,
	/// lower-left.
	/// </returns>
	//*************************************************************************

	public static PointF []
	TriangleFromCenterAndHalfWidth
	(
		Single fXCenter,
		Single fYCenter,
		Single fHalfWidth
	)
	{
		Debug.Assert(fHalfWidth >= 0);

		Single fHalfHeight = fHalfWidth * SquareRootOfThreeOverTwo;

		return ( new PointF [] {
			new PointF(fXCenter, fYCenter - fHalfHeight),
			new PointF(fXCenter + fHalfWidth, fYCenter + fHalfHeight),
			new PointF(fXCenter - fHalfWidth, fYCenter + fHalfHeight),
			} );
	}

	//*************************************************************************
	//	Method: GetTriangleEdgeMidpoints()
	//
	/// <summary>
	/// Returns an array of points on the midpoints of an equilateral
	/// triangle's edges given a center point and half-width.
	/// </summary>
	///
	/// <param name="fXCenter">
	/// x-coordinate of the triangle's center.
	/// </param>
	///
	/// <param name="fYCenter">
	/// y-coordinate of the triangle's center.
	/// </param>
	///
	/// <param name="fHalfWidth">
	/// One half the width of the square that bounds the triangle.
	/// </param>
	///
	/// <returns>
	/// The midpoints of the specified triangle.
	/// </returns>
	//*************************************************************************

	public static PointF []
	GetTriangleEdgeMidpoints
	(
		Single fXCenter,
		Single fYCenter,
		Single fHalfWidth
	)
	{
		Debug.Assert(fHalfWidth >= 0);

		Single fHalfHeight = fHalfWidth * SquareRootOfThreeOverTwo;
		Single fQuarterWidth = fHalfWidth / 2F;

		return ( new PointF [] {
			new PointF(fXCenter + fQuarterWidth, fYCenter),
			new PointF(fXCenter, fYCenter + fHalfHeight),
			new PointF(fXCenter - fQuarterWidth, fYCenter),
			} );
	}

	//*************************************************************************
	//	Method: RectangleFToRectangle()
	//
	/// <summary>
	/// Converts a RectangleF to a Rectangle.
	/// </summary>
	///
	/// <param name="oRectangle">
	/// Rectangle to convert.
	/// </param>
	/// 
	/// <param name="iPenWidthPx">
	/// Width of the pen that will be used to draw the rectangle.
	/// </param>
	///
	/// <returns>
	/// Converted rectangle.
	/// </returns>
	///
	/// <remarks>
	///	This method converts a floating-point RectangleF to an integer
	///	Rectangle, compensating for some GDI oddities in the process.
	///
	/// <para>
	///	If precise rectangle drawing is required, the caller should convert
	///	all RectangleF objects to Rectangles using this method, then use the
	///	Graphics.DrawRectangle(Pen, Rectangle) method to draw them.  The
	///	floating-point version of Graphics.DrawRectangle() should not be used.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public static Rectangle
	RectangleFToRectangle
	(
		RectangleF oRectangle,
		Int32 iPenWidthPx
	)
	{
		/*
		GDI draws floating-point rectangles in an odd manner.  Consider the
		following code:

			RectangleF oRectangle =
				RectangleF.FromLTRB(fLeft, fTop, fRight, fBottom);

			Pen oPen = new Pen(Color.Black, iPenWidthPx);
			oPen.Alignment = PenAlignment.Inset;

			oGraphics.DrawRectangle(oPen, oRectangle.X, oRectangle.Y,
				oRectangle.Width, oRectangle.Height);

		If iPenWidthPx is 1, then GDI rounds all the rectangle's coordinates
		before drawing the rectangle.  For example, fLeft values between
		0 and 0.50, inclusive, cause the left edge to be drawn at X=0.  fLeft
		values between 0.51 and 1.50 result in a left edge at X=1.  Similarly,
		fBottom values between 4.51 and 5.50 result in a bottom edge at Y=5,
		and fBottom values between 5.51 and 6.50 result in a bottom edge at Y=6.
		fRight and fTop are treated the same way.  This all seems like sensible
		behavior.

		If iPenWidthPx is greater than 1, however, the behavior changes.  The
		fLeft and fTop values are rounded up, while the fRight and fBottom
		values are rounded down.  For example, fLeft values between 0.1 and 1.0
		cause the left edge to be drawn at X=1, and fLeft values between 1.1
		and 2.0 result in a left edge at X=2.  fBottom values between
		4.1 and 5.0 cause the bottom edge to be drawn at Y=4, and fBottom
		values between 5.1 and 6.0 cause the bottom edge to be drawn at Y=5.

		The odd behavior for pen widths greater than 1 causes unexpected and
		inconsistent rectangle spacing.  To fix this problem, the following
		code rounds all values.  If the rectangle is going to be drawn with a
		pen width greater than 1, the code then adds 1 to the right and bottom
		values to compensate for GDI's inconsistency.

		This also fixes an oddity where GDI sometimes leaves off corner pixels
		when drawing rectangles using floating-point coordinates.
		*/

		Int32 iLeft = (Int32)(oRectangle.Left + 0.5F);
		Int32 iRight = (Int32)(oRectangle.Right + 0.5F);
		Int32 iTop = (Int32)(oRectangle.Top + 0.5F);
		Int32 iBottom = (Int32)(oRectangle.Bottom + 0.5F);

		if (iPenWidthPx > 1)
		{
			iRight++;
			iBottom++;
		}

		return ( Rectangle.FromLTRB(iLeft, iTop, iRight, iBottom) );
	}

	//*************************************************************************
	//	Method: SaveHighQualityImage()
	//
	/// <summary>
	/// Saves an Image object to a specified file in a specified format using
	/// high quality settings.
	/// </summary>
	///
	/// <param name="oImage">
	/// Image to save.
	/// </param>
	/// 
	/// <param name="sFileName">
	/// Full path of the file to save to.
	/// </param>
	///
	/// <param name="eImageFormat">
	/// File format.
	/// </param>
	///
	/// <remarks>
	/// Use this instead of Image.Save(filename, format) if you want a high-
	/// quality image.
	/// </remarks>
	//*************************************************************************

	public static void
	SaveHighQualityImage
	(
		Image oImage,
		String sFileName,
		ImageFormat eImageFormat
	)
	{
		Debug.Assert(oImage != null);
		Debug.Assert(sFileName != null);
		Debug.Assert(sFileName != "");

		// Override the default JPEG quality settings.  Use default settings
		// for the other formats.

		if (eImageFormat == ImageFormat.Jpeg)
			SaveJpegImage(oImage, sFileName, 100);
		else
			oImage.Save(sFileName, eImageFormat);
	}

	//*************************************************************************
	//	Method: SaveJpegImage()
	//
	/// <summary>
	/// Saves an image to a JPEG file with a specified quality level.
	/// </summary>
	///
	/// <param name="oImage">
	/// Image to save.
	/// </param>
	/// 
	/// <param name="sFileName">
	/// Full path of the file to save to.
	/// </param>
	///
	/// <param name="iQuality">
	/// Quality level to use.  I THINK this can be from 1 to 100; the
	/// documentation is not clear.
	/// </param>
	///
	/// <remarks>
	/// Image.Save(..., ImageFormat.Jpeg) uses a low quality by default.  This
	/// method allows the quality to be specified.
	/// </remarks>
	//*************************************************************************

	public static void
	SaveJpegImage
	(
		Image oImage,
		String sFileName,
		Int32 iQuality
	)
	{
		Debug.Assert(oImage != null);
		Debug.Assert(sFileName != null);
		Debug.Assert(sFileName != "");
		Debug.Assert(iQuality >= 1);
		Debug.Assert(iQuality <= 100);

		// This is based on the sample code in the MSDN topic for
		// Encoder.Quality.

		ImageCodecInfo oImageCodecInfo =
			GetImageCodecInfoForMimeType("image/jpeg");

		EncoderParameters oEncoderParameters = new EncoderParameters(1);

		oEncoderParameters.Param[0] =
			new EncoderParameter(Encoder.Quality, iQuality);

		oImage.Save(sFileName, oImageCodecInfo, oEncoderParameters);
	}

	//************************************************************************* 
	//	Method: DrawErrorStringOnGraphics()
	//
	/// <summary>
	///	Draws an error string on a Graphics object.
	/// </summary>
	///
	/// <param name="oGraphics">
	/// Object to draw on.
	/// </param>
	///
	/// <param name="oRectangle">
	/// Rectangle to draw on.
	/// </param>
	///
	/// <param name="sString">
	/// String to draw.
	/// </param>
	///
	/// <remarks>
	/// This can be used to draw error strings on bitmaps in a uniform manner.
	/// </remarks>
	//*************************************************************************

	public static void
	DrawErrorStringOnGraphics
	(
		Graphics oGraphics,
		Rectangle oRectangle,
		String sString
	)
	{
		Debug.Assert(oGraphics != null);
		Debug.Assert(sString != null);

		oGraphics.DrawString(sString, new Font("Arial", 11F), Brushes.Black,
			oRectangle);
	}

	//*************************************************************************
	//	Method: GetImageCodecInfoForMimeType()
	//
	/// <summary>
	/// Gets the ImageCodecInfo object for a specified MIME type.
	/// </summary>
	///
	/// <param name="sMimeType">
	/// MIME type.
	/// </param>
	///
	/// <remarks>
	/// An exception is thrown if the requested object isn't found.
	/// </remarks>
	//*************************************************************************

	public static ImageCodecInfo
	GetImageCodecInfoForMimeType
	(
		String sMimeType
	)
	{
		Debug.Assert(sMimeType != null);
		Debug.Assert(sMimeType != "");

		foreach (ImageCodecInfo oImageCodecInfo in
			ImageCodecInfo.GetImageEncoders() )
		{
			if (oImageCodecInfo.MimeType == sMimeType)
				return (oImageCodecInfo);
		}

		throw new Exception("GraphicsUtil.GetImageCodecInfoForMimeType: Can't"
			+ " find " + sMimeType + ".");
	}

	//*************************************************************************
	//	Method: ConvertPixelsAndPoints()
	//
	/// <summary>
	/// Converts a height and width from pixels to points, or vice versa.
	/// </summary>
	///
	/// <param name="bPixelsToPoints">
	///	true to convert pixels to points, false to convert points to pixels.
	/// </param>
	///
	/// <param name="oGraphics">
	///	Object that will do the conversion.
	/// </param>
	///
	/// <param name="fWidthIn">
	/// Input width.  Must be greater than or equal to zero.
	/// </param>
	///
	/// <param name="fHeightIn">
	/// Input height.  Must be greater than or equal to zero.
	/// </param>
	///
	/// <param name="fWidthOut">
	///	Where the converted width gets stored.
	/// </param>
	///
	/// <param name="fHeightOut">
	///	Where the converted height gets stored.
	/// </param>
	//*************************************************************************

	protected static void
	ConvertPixelsAndPoints
	(
		Boolean bPixelsToPoints,
		Graphics oGraphics,
		Single fWidthIn,
		Single fHeightIn,
		out Single fWidthOut,
		out Single fHeightOut
	)
	{
		Debug.Assert(oGraphics != null);
		Debug.Assert(fWidthIn >= 0);
		Debug.Assert(fHeightIn >= 0);

		// Switch the page unit to points.

		GraphicsUnit oOldPageUnit = oGraphics.PageUnit;
		oGraphics.PageUnit = GraphicsUnit.Point;

		// Create an array of one point.

		PointF [] aoPointF = new PointF[1] { new PointF(fWidthIn, fHeightIn) };

		// Convert the point from pixels to points, or vice versa.

		if (bPixelsToPoints)
		{
			oGraphics.TransformPoints(
				CoordinateSpace.Page, CoordinateSpace.Device, aoPointF);
		}
		else
		{
			oGraphics.TransformPoints(
				CoordinateSpace.Device, CoordinateSpace.Page, aoPointF);
		}

		fWidthOut = aoPointF[0].X;
		fHeightOut = aoPointF[0].Y;

		// Restore the page unit.

		oGraphics.PageUnit = oOldPageUnit;
	}

	//*************************************************************************
	//	Method: DisposePen()
	//
	/// <summary>
	/// Disposes of a pen.
	/// </summary>
	///
	/// <param name="oPen">
	///	Pen to dispose.  Can be null.  Gets set to null.
	/// </param>
	///
	/// <remarks>
	/// If <paramref name="oPen" /> isn't null, this method calls the Dispose
	/// method on <paramref name="oPen" />, then sets it to null.
	/// </remarks>
	//*************************************************************************

	public static void
	DisposePen
	(
		ref Pen oPen
	)
	{
		if (oPen != null)
		{
			oPen.Dispose();
			oPen = null;
		}
	}

	//*************************************************************************
	//	Method: DisposeSolidBrush()
	//
	/// <summary>
	/// Disposes of a solid brush.
	/// </summary>
	///
	/// <param name="oSolidBrush">
	///	SolidBrush to dispose.  Can be null.  Gets set to null.
	/// </param>
	///
	/// <remarks>
	/// If <paramref name="oSolidBrush" /> isn't null, this method calls the
	/// Dispose method on <paramref name="oSolidBrush" />, then sets it to
	/// null.
	/// </remarks>
	//*************************************************************************

	public static void
	DisposeSolidBrush
	(
		ref SolidBrush oSolidBrush
	)
	{
		if (oSolidBrush != null)
		{
			oSolidBrush.Dispose();
			oSolidBrush = null;
		}
	}

	//*************************************************************************
	//	Method: DisposeBrush()
	//
	/// <summary>
	/// Disposes of a brush.
	/// </summary>
	///
	/// <param name="oBrush">
	///	Brush to dispose.  Can be null.  Gets set to null.
	/// </param>
	///
	/// <remarks>
	/// If <paramref name="oBrush" /> isn't null, this method calls the Dispose
	/// method on <paramref name="oBrush" />, then sets it to null.
	/// </remarks>
	//*************************************************************************

	public static void
	DisposeBrush
	(
		ref Brush oBrush
	)
	{
		if (oBrush != null)
		{
			oBrush.Dispose();
			oBrush = null;
		}
	}

	//*************************************************************************
	//	Method: DisposeGraphics()
	//
	/// <summary>
	/// Disposes of a Graphics object.
	/// </summary>
	///
	/// <param name="oGraphics">
	///	Graphics object to dispose.  Can be null.  Gets set to null.
	/// </param>
	///
	/// <remarks>
	/// If <paramref name="oGraphics" /> isn't null, this method calls the
	/// Dispose method on <paramref name="oGraphics" />, then sets it to null.
	/// </remarks>
	//*************************************************************************

	public static void
	DisposeGraphics
	(
		ref Graphics oGraphics
	)
	{
		if (oGraphics != null)
		{
			oGraphics.Dispose();
			oGraphics = null;
		}
	}

	//*************************************************************************
	//	Method: DisposeGraphicsPath()
	//
	/// <summary>
	/// Disposes of a GraphicsPath object.
	/// </summary>
	///
	/// <param name="oGraphicsPath">
	///	GraphicsPath object to dispose.  Can be null.  Gets set to null.
	/// </param>
	///
	/// <remarks>
	/// If <paramref name="oGraphicsPath" /> isn't null, this method calls the
	/// Dispose method on <paramref name="oGraphicsPath" />, then sets it to
	/// null.
	/// </remarks>
	//*************************************************************************

	public static void
	DisposeGraphicsPath
	(
		ref GraphicsPath oGraphicsPath
	)
	{
		if (oGraphicsPath != null)
		{
			oGraphicsPath.Dispose();
			oGraphicsPath = null;
		}
	}

	//*************************************************************************
	//	Method: DisposeBitmap()
	//
	/// <summary>
	/// Disposes of a Bitmap object.
	/// </summary>
	///
	/// <param name="oBitmap">
	///	Bitmap object to dispose.  Can be null.  Gets set to null.
	/// </param>
	///
	/// <remarks>
	/// If <paramref name="oBitmap" /> isn't null, this method calls the
	/// Dispose method on <paramref name="oBitmap" />, then sets it to null.
	/// </remarks>
	//*************************************************************************

	public static void
	DisposeBitmap
	(
		ref Bitmap oBitmap
	)
	{
		if (oBitmap != null)
		{
			oBitmap.Dispose();
			oBitmap = null;
		}
	}


	//*************************************************************************
	//	Private constants
	//*************************************************************************

	// Used in equilateral triangle calculations.

	private const Single SquareRootOfThreeOverTwo = 0.866025404F;
}

}
