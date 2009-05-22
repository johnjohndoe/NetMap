
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.GraphicsLib
{
//*****************************************************************************
//  Class: WpfGraphicsUtil
//
/// <summary>
/// Utility methods for drawing with WPF.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class WpfGraphicsUtil
{
    //*************************************************************************
    //  Static constructor: WpfGraphicsUtil()
    //
    /// <summary>
    /// Static constructor for the WpfGraphicsUtil class.
    /// </summary>
    //*************************************************************************

    static WpfGraphicsUtil()
    {
        Double d30DegreesInRadians = Math.PI / 6.0;

        Cosine30Degrees = Math.Cos(d30DegreesInRadians);
        Tangent30Degrees = Math.Tan(d30DegreesInRadians);
    }

    //*************************************************************************
    //  Method: ColorToWpfColor()
    //
    /// <overloads>
    /// Converts a System.Drawing.Color to a System.Windows.Media.Color.
    /// </overloads>
    ///
    /// <summary>
    /// Converts a System.Drawing.Color to a System.Windows.Media.Color using
    /// the alpha value of the System.Drawing.Color.
    /// </summary>
    ///
    /// <param name="color">
    /// The System.Drawing.Color to convert.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="color" /> converted to a System.Windows.Media color.
    /// The alpha value is the same as the alpha value of <paramref
    /// name="color" />.
    /// </returns>
    //*************************************************************************

    public static System.Windows.Media.Color
    ColorToWpfColor
    (
        System.Drawing.Color color
    )
    {
        return ( ColorToWpfColor(color, color.A) );
    }

    //*************************************************************************
    //  Method: ColorToWpfColor()
    //
    /// <summary>
    /// Converts a System.Drawing.Color to a System.Windows.Media.Color using a
    /// new alpha value.
    /// </summary>
    ///
    /// <param name="color">
    /// The System.Drawing.Color to convert.
    /// </param>
    ///
    /// <param name="newAlpha">
    /// The new alpha value to use.  The alpha value of <paramref
    /// name="color" /> is ignored.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="color" /> converted to a System.Windows.Media color,
    /// with an alpha value of <paramref name="newAlpha" />.
    /// </returns>
    //*************************************************************************

    public static System.Windows.Media.Color
    ColorToWpfColor
    (
        System.Drawing.Color color,
        Byte newAlpha
    )
    {
        return ( System.Windows.Media.Color.FromArgb(newAlpha,
            color.R, color.G, color.B) );
    }

    //*************************************************************************
    //  Method: VisualToBitmap()
    //
    /// <summary>
    /// Renders a Visual to a System.Drawing.Bitmap.
    /// </summary>
    ///
    /// <param name="visual">
    /// The Visual to render.
    /// </param>
    ///
    /// <param name="bitmapWidthPx">
    /// Bitmap width, in pixels.
    /// </param>
    ///
    /// <param name="bitmapHeightPx">
    /// Bitmap height, in pixels.
    /// </param>
    ///
    /// <returns>
    /// A System.Drawing.Bitmap with the specified dimensions.
    /// </returns>
    //*************************************************************************

    public static System.Drawing.Bitmap
    VisualToBitmap
    (
        Visual visual,
        Int32 bitmapWidthPx,
        Int32 bitmapHeightPx
    )
    {
        Debug.Assert(visual != null);
        Debug.Assert(bitmapWidthPx > 0);
        Debug.Assert(bitmapHeightPx > 0);

        RenderTargetBitmap oRenderTargetBitmap = new RenderTargetBitmap(
            bitmapWidthPx, bitmapHeightPx, 96, 96, PixelFormats.Default);

        oRenderTargetBitmap.Render(visual);

        BmpBitmapEncoder oBmpBitmapEncoder = new BmpBitmapEncoder();

        oBmpBitmapEncoder.Frames.Add(
            BitmapFrame.Create(oRenderTargetBitmap) );

        MemoryStream oMemoryStream = new MemoryStream();

        oBmpBitmapEncoder.Save(oMemoryStream);

        System.Drawing.Bitmap oBitmap =
            new System.Drawing.Bitmap(oMemoryStream);

        // (The stream must be kept open during the lifetime of the bitmap.

        return (oBitmap);
    }

    //*************************************************************************
    //  Method: RectToRectangle()
    //
    /// <summary>
    /// Converts a System.Windows.Rect to a System.Drawing.Rectangle.
    /// </summary>
    ///
    /// <param name="rect">
    /// The System.Windows.Rect to convert.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="rect" /> converted to a System.Drawing.Rectangle.
    /// </returns>
    ///
    /// <remarks>
    /// The conversion is performed via a ceiling on the left and top
    /// coordinates and a floor on the right and bottom coordinates.  This
    /// forces the converted rectangle to be no larger than the original.
    /// </remarks>
    //*************************************************************************

    public static System.Drawing.Rectangle
    RectToRectangle
    (
        System.Windows.Rect rect
    )
    {
        return ( System.Drawing.Rectangle.FromLTRB(
            (Int32)Math.Ceiling(rect.Left), (Int32)Math.Ceiling(rect.Top),
            (Int32)Math.Floor(rect.Right), (Int32)Math.Floor(rect.Bottom)
            ) );
    }

    //*************************************************************************
    //  Method: RectangleToRect()
    //
    /// <summary>
    /// Converts a System.Drawing.Rectangle to a System.Windows.Rect.
    /// </summary>
    ///
    /// <param name="rectangle">
    /// The System.Drawing.Rectangle to convert.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="rectangle" /> converted to a System.Windows.Rect.
    /// </returns>
    //*************************************************************************

    public static System.Windows.Rect
    RectangleToRect
    (
        System.Drawing.Rectangle rectangle
    )
    {
        return ( new System.Windows.Rect(
            rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height)
            );
    }

    //*************************************************************************
    //  Method: WpfPointToPoint()
    //
    /// <summary>
    /// Converts a System.Windows.Point to a System.Drawing.Point.
    /// </summary>
    ///
    /// <param name="point">
    /// The System.Windows.Point to convert.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="point" /> converted to a System.Drawing.Point.
    /// </returns>
    ///
    /// <remarks>
    /// The conversion from Double coordinates to Int32 coordinates is
    /// performed via truncation.
    /// </remarks>
    //*************************************************************************

    public static System.Drawing.Point
    WpfPointToPoint
    (
        System.Windows.Point point
    )
    {
        return ( new System.Drawing.Point( (Int32)point.X,  (Int32)point.Y ) );
    }

    //*************************************************************************
    //  Method: PointFToWpfPoint()
    //
    /// <summary>
    /// Converts a System.Drawing.PointF to a System.Windows.Point.
    /// </summary>
    ///
    /// <param name="pointF">
    /// The System.Drawing.PointF to convert.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="pointF" /> converted to a System.Windows.Point.
    /// </returns>
    //*************************************************************************

    public static System.Windows.Point
    PointFToWpfPoint
    (
        System.Drawing.PointF pointF
    )
    {
        return ( new System.Windows.Point(pointF.X, pointF.Y) );
    }

    //*************************************************************************
    //  Method: GetAngleBetweenPoints()
    //
    /// <summary>
    /// Gets the angle between two points.
    /// </summary>
    ///
    /// <param name="point1">
    /// The first point.
    /// </param>
    ///
    /// <param name="point2">
    /// The second point.
    /// </param>
    ///
    /// <returns>
    /// The angle between the two points, in radians, as computed by
    /// Math.Atan2.  Ranges between 0 and PI (0 to 180 degrees) and 0 to -PI
    /// (0 to -180 degrees).
    /// </returns>
    //*************************************************************************

    public static Double
    GetAngleBetweenPoints
    (
        Point point1,
        Point point2
    )
    {
        return ( Math.Atan2(point1.Y - point2.Y, point2.X - point1.X) );
    }

    //*************************************************************************
    //  Method: SquareFromCenterAndHalfWidth()
    //
    /// <summary>
    /// Returns a square given a center point and half-width.
    /// </summary>
    ///
    /// <param name="center">
    /// The square's center.
    /// </param>
    ///
    /// <param name="halfWidth">
    /// One half the width of the square.
    /// </param>
    ///
    /// <returns>
    /// The specified square, as a System.Windows.Rect.
    /// </returns>
    //*************************************************************************

    public static System.Windows.Rect
    SquareFromCenterAndHalfWidth
    (
        System.Windows.Point center,
        Double halfWidth
    )
    {
        Debug.Assert(halfWidth >= 0);

        center.Offset(-halfWidth, -halfWidth);
        Double dWidth = 2 * halfWidth;

        return ( new System.Windows.Rect( center,
            new System.Windows.Size(dWidth, dWidth) ) );
    }

    //*************************************************************************
    //  Method: DiamondFromCenterAndHalfWidth()
    //
    /// <summary>
    /// Creates a PathGeometry that represents a diamond centered on a
    /// specified point.
    /// </summary>
    ///
    /// <param name="center">
    /// The diamond's center.
    /// </param>
    ///
    /// <param name="halfWidth">
    /// One half the width of the diamond.
    /// </param>
    ///
    /// <returns>
    /// A PathGeometry that represents the specified diamond.  The PathGeometry
    /// is frozen.
    /// </returns>
    //*************************************************************************

    public static PathGeometry
    DiamondFromCenterAndHalfWidth
    (
        System.Windows.Point center,
        Double halfWidth
    )
    {
        Debug.Assert(halfWidth >= 0);

        Double dCenterX = center.X;
        Double dCenterY = center.Y;

        return ( PathGeometryFromPoints(
            new Point(dCenterX - halfWidth, dCenterY),
            new Point(dCenterX, dCenterY - halfWidth),
            new Point(dCenterX + halfWidth, dCenterY),
            new Point(dCenterX, dCenterY + halfWidth)
            ) );
    }

    //*************************************************************************
    //  Method: TriangleFromCenterAndHalfWidth()
    //
    /// <summary>
    /// Creates a PathGeometry that represents a triangle centered on a
    /// specified point.
    /// </summary>
    ///
    /// <param name="center">
    /// The triangle's center.
    /// </param>
    ///
    /// <param name="halfWidth">
    /// One half the width of the square that bounds the triangle.
    /// </param>
    ///
    /// <returns>
    /// A PathGeometry that represents the specified triangle.  The
    /// PathGeometry is frozen.
    /// </returns>
    //*************************************************************************

    public static PathGeometry
    TriangleFromCenterAndHalfWidth
    (
        System.Windows.Point center,
        Double halfWidth
    )
    {
        Debug.Assert(halfWidth >= 0);

        Rect oTriangleBounds =
            TriangleBoundsFromCenterAndHalfWidth(center, halfWidth);

        PathGeometry oPathGeometry = PathGeometryFromPoints(
            new Point(center.X, oTriangleBounds.Y),
            oTriangleBounds.BottomRight,
            oTriangleBounds.BottomLeft
            );

        return (oPathGeometry);
    }

    //*************************************************************************
    //  Method: TriangleBoundsFromCenterAndHalfWidth()
    //
    /// <summary>
    /// Creates a rectangle that bounds a triangle centered on a specified
    /// point.
    /// </summary>
    ///
    /// <param name="center">
    /// The triangle's center.
    /// </param>
    ///
    /// <param name="halfWidth">
    /// One half the width of the square that bounds the triangle.
    /// </param>
    ///
    /// <returns>
    /// A rectangle that bounds the specified triangle.
    /// </returns>
    //*************************************************************************

    public static Rect
    TriangleBoundsFromCenterAndHalfWidth
    (
        System.Windows.Point center,
        Double halfWidth
    )
    {
        Debug.Assert(halfWidth >= 0);

        Double dCenterX = center.X;
        Double dCenterY = center.Y;

        Double dApexY = dCenterY - (halfWidth / Cosine30Degrees);
        Double dBaseY = dCenterY + (halfWidth * Tangent30Degrees);

        return ( new Rect(
            dCenterX - halfWidth,
            dApexY,
            2.0 * halfWidth,
            dBaseY - dApexY
            ) );
    }

    //*************************************************************************
    //  Method: PathGeometryFromPoints()
    //
    /// <summary>
    /// Creates a PathGeometry from a sequence of points.
    /// </summary>
    ///
    /// <param name="startPoint">
    /// The first point in the sequence.
    /// </param>
    ///
    /// <param name="otherPoints">
    /// The other points in the sequence.
    /// </param>
    ///
    /// <returns>
    /// A PathGeometry consisting of LineSegments connecting the specified
    /// points.  The PathGeometry is closed and frozen.
    /// </returns>
    //*************************************************************************

    public static PathGeometry
    PathGeometryFromPoints
    (
        System.Windows.Point startPoint,
        params Point [] otherPoints
    )
    {
        Debug.Assert(otherPoints != null);

        Int32 iOtherPoints = otherPoints.Length;

        Debug.Assert(iOtherPoints > 0);

        PathFigure oPathFigure = new PathFigure();

        oPathFigure.StartPoint = startPoint;

        PathSegmentCollection oPathSegmentCollection =
            new PathSegmentCollection(iOtherPoints);

        for (Int32 i = 0; i < iOtherPoints; i++)
        {
            oPathSegmentCollection.Add(
                new LineSegment(otherPoints[i], true) );
        }

        oPathFigure.Segments = oPathSegmentCollection;
        oPathFigure.IsClosed = true;
        FreezeIfFreezable(oPathFigure);

        PathGeometry oPathGeometry = new PathGeometry();

        oPathGeometry.Figures.Add(oPathFigure);
        FreezeIfFreezable(oPathGeometry);

        return (oPathGeometry);
    }

    //*************************************************************************
    //  Method: GetRectangleMinusMargin()
    //
    /// <summary>
    /// Subtracts a margin from a rectangle.
    /// </summary>
    ///
    /// <param name="rectangle">
    /// The rectangle to subtract the margin from.
    /// </param>
    ///
    /// <param name="margin">
    /// The margin to subtract from each edge.  Must be greater than or equal
    /// to zero.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="rectangle" /> with <paramref name="margin" />
    /// subtracted from each side, or Rect.Empty.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="rectangle" /> is narrower or shorter than twice the
    /// <paramref name="margin" />, Rect.Empty is returned.
    /// </remarks>
    //*************************************************************************

    public static Rect
    GetRectangleMinusMargin
    (
        Rect rectangle,
        Double margin
    )
    {
        Debug.Assert(margin >= 0);

        rectangle.Inflate(-margin, -margin);

        if (rectangle.Width <= 0 || rectangle.Height <= 0)
        {
            return (Rect.Empty);
        }

        return (rectangle);
    }

    //*************************************************************************
    //  Method: MoveRectangleWithinBounds()
    //
    /// <summary>
    /// Moves a rectangle so it is contained within an outer bounding
    /// rectangle.
    /// </summary>
    ///
    /// <param name="rectangle">
    /// The rectangle that needs to be contained within <paramref
    /// name="boundingRectangle" />.
    /// </param>
    ///
    /// <param name="boundingRectangle">
    /// The rectangle that <paramref name="rectangle" /> needs to be contained
    /// within.
    /// </param>
    ///
    /// <param name="resizeRectangleIfNecessary">
    /// If this is true and moving <paramref name="rectangle" /> isn't enough
    /// to contain it within <paramref name="boundingRectangle" />, <paramref
    /// name="rectangle" /> is resized to force it to be contained.  If this is
    /// false, any excess is left hanging over the right and bottom edges of
    /// <paramref name="boundingRectangle" />.
    /// </param>
    ///
    /// <returns>
    /// A copy of <paramref name="rectangle" /> that is contained within
    /// <paramref name="boundingRectangle" />.
    /// </returns>
    //*************************************************************************

    public static Rect
    MoveRectangleWithinBounds
    (
        Rect rectangle,
        Rect boundingRectangle,
        Boolean resizeRectangleIfNecessary
    )
    {
        Rect movedRectangle = rectangle;

        if (!rectangle.IsEmpty)
        {
            if (resizeRectangleIfNecessary)
            {
                movedRectangle.Width = Math.Min(
                    movedRectangle.Width, boundingRectangle.Width);

                movedRectangle.Height = Math.Min(
                    movedRectangle.Height, boundingRectangle.Height);
            }

            Double dXOffset = boundingRectangle.Left - movedRectangle.Left;

            if (movedRectangle.Width > boundingRectangle.Width || dXOffset > 0)
            {
                movedRectangle.Offset(dXOffset, 0);
            }
            else
            {
                dXOffset = boundingRectangle.Right - movedRectangle.Right;

                if (dXOffset < 0)
                {
                    movedRectangle.Offset(dXOffset, 0);
                }
            }

            Double dYOffset = boundingRectangle.Top - movedRectangle.Top;

            if (movedRectangle.Height > boundingRectangle.Height ||
                dYOffset > 0)
            {
                movedRectangle.Offset(0, dYOffset);
            }
            else
            {
                dYOffset = boundingRectangle.Bottom - movedRectangle.Bottom;

                if (dYOffset < 0)
                {
                    movedRectangle.Offset(0, dYOffset);
                }
            }
        }

        return (movedRectangle);
    }

    //*************************************************************************
    //  Method: GetFarthestRectangleEdge()
    //
    /// <summary>
    /// Determines which edge of a rectangle is farthest from a point within
    /// the rectangle.
    /// </summary>
    ///
    /// <param name="point">
    /// The point to use.  Should be contained within <paramref
    /// name="rectangle" />.
    /// </param>
    ///
    /// <param name="rectangle">
    /// The rectangle to use.
    /// </param>
    ///
    /// <returns>
    /// The edge of <paramref name="rectangle" /> that is farthest from
    /// <paramref name="point" />, as a <see cref="RectangleEdge" />.
    /// </returns>
    ///
    /// <remarks>
    /// If the width or height of <paramref name="rectangle" /> is zero, or
    /// <paramref name="point" /> is not contained within <paramref
    /// name="rectangle" />, RectangleEdge.Left is arbitrarily returned.
    /// </remarks>
    //*************************************************************************

    public static RectangleEdge
    GetFarthestRectangleEdge
    (
        Point point,
        Rect rectangle
    )
    {
        if ( rectangle.Width <= 0 || rectangle.Height <= 0 ||
            !rectangle.Contains(point) )
        {
            return (RectangleEdge.Left);
        }

        Double dX = point.X;
        Double dY = point.Y;

        Double dDistanceFromLeft = dX - rectangle.Left;
        Double dDistanceFromRight = rectangle.Right - dX;
        Double dDistanceFromTop = dY - rectangle.Top;
        Double dDistanceFromBottom = rectangle.Bottom - dY;

        RectangleEdge eFarthestEdge = RectangleEdge.Left;
        Double dGreatestDistance = dDistanceFromLeft;

        if (dDistanceFromRight > dGreatestDistance)
        {
            eFarthestEdge = RectangleEdge.Right;
            dGreatestDistance = dDistanceFromRight;
        }

        if (dDistanceFromTop > dGreatestDistance)
        {
            eFarthestEdge = RectangleEdge.Top;
            dGreatestDistance = dDistanceFromTop;
        }

        if (dDistanceFromBottom > dGreatestDistance)
        {
            eFarthestEdge = RectangleEdge.Bottom;
            dGreatestDistance = dDistanceFromBottom;
        }

        return (eFarthestEdge);
    }

    //*************************************************************************
    //  Method: GetRotatedMatrix()
    //
    /// <summary>
    /// Gets an identity Matrix rotatated a specified angle about a point.
    /// </summary>
    ///
    /// <param name="centerOfRotation">
    /// The center of rotation.
    /// </param>
    ///
    /// <param name="angleToRotateDegrees">
    /// The angle to rotate the Matrix, in degrees.
    /// </param>
    ///
    /// <returns>
    /// A new identity Matrix rotated <paramref name="angleToRotateDegrees" />
    /// about <paramref name="centerOfRotation" />.
    /// </returns>
    //*************************************************************************

    public static Matrix
    GetRotatedMatrix
    (
        Point centerOfRotation,
        Double angleToRotateDegrees
    )
    {
        Matrix oMatrix = Matrix.Identity;

        oMatrix.RotateAt(angleToRotateDegrees,
            centerOfRotation.X, centerOfRotation.Y);

        return (oMatrix);
    }

    //*************************************************************************
    //  Method: FreezeIfFreezable()
    //
    /// <summary>
    /// Freezes a freezable object if possible.
    /// </summary>
    ///
    /// <param name="freezable">
    /// The object to freeze.
    /// </param>
    ///
    /// <returns>
    /// true if the object was frozen.
    /// </returns>
    //*************************************************************************

    public static Boolean
    FreezeIfFreezable
    (
        Freezable freezable
    )
    {
        Debug.Assert(freezable != null);

        if (freezable.CanFreeze)
        {
            freezable.Freeze();

            return (true);
        }

        return (false);
    }


    //*************************************************************************
    //  Public fields
    //*************************************************************************

    /// Used in equilateral triangle calculations.

    public static Double Cosine30Degrees;
    ///
    public static Double Tangent30Degrees;
}

}
