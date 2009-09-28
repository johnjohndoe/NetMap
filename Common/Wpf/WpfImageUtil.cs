
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace Microsoft.WpfGraphicsLib
{
//*****************************************************************************
//  Class: WpfImageUtil
//
/// <summary>
/// Utility methods for working with images in WPF.
/// </summary>
//*****************************************************************************

public class WpfImageUtil
{
    //*************************************************************************
    //  Constructor: WpfImageUtil()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="WpfImageUtil" /> class.
    /// </summary>
    //*************************************************************************

    public WpfImageUtil()
    {
        m_oCachedErrorImage = null;

        AssertValid();
    }

    //*************************************************************************
    //  Method: GetImageSynchronous()
    //
    /// <summary>
    /// Synchronously gets an image from a specified URI and handles most
    /// errors by returning an error image.
    /// </summary>
    ///
    /// <param name="uriString">
    /// The URI to get the image from.  If the string is not a valid URI, an
    /// error image is returned.
    /// </param>
    ///
    /// <returns>
    /// The specified image, or an error image if the specified image isn't
    /// available.
    /// </returns>
    ///
    /// <remarks>
    /// There are two differences between using this method and using
    /// BitmapImage(URI): 1) If the URI is an URL, the image is downloaded
    /// synchronously instead of asynchronously; and 2) If the image isn't
    /// available, an error image is returned instead of an exception being
    /// thrown.
    /// </remarks>
    //*************************************************************************

    public BitmapSource
    GetImageSynchronous
    (
        String uriString
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(uriString) );
        AssertValid();

        const Int32 ErrorImageWidthAndHeight = 52;
        Uri oUri;

        if ( Uri.TryCreate(uriString, UriKind.Absolute, out oUri) )
        {
            if (oUri.Scheme == Uri.UriSchemeHttp ||
                oUri.Scheme == Uri.UriSchemeHttps)
            {
                try
                {
                    return ( GetImageSynchronousHttp(oUri) );
                }
                catch (WebException)
                {
                    // (Empty catch blocks cause an error image to be
                    // returned.)
                }
            }
            else if (oUri.Scheme == Uri.UriSchemeFile)
            {
                try
                {
                    BitmapImage oBitmapImage = new BitmapImage(oUri);
                    WpfGraphicsUtil.FreezeIfFreezable(oBitmapImage);

                    return (oBitmapImage);
                }
                catch (IOException)
                {
                }
                catch (NotSupportedException)
                {
                    // Invalid image file.
                }
                catch (UnauthorizedAccessException)
                {
                    // The URI is actually a folder, for example.
                }
            }
        }

        if (m_oCachedErrorImage == null)
        {
            m_oCachedErrorImage = CreateErrorImage(ErrorImageWidthAndHeight);
        }

        return (m_oCachedErrorImage);
    }

    //*************************************************************************
    //  Method: GetImageSynchronousIgnoreDpi()
    //
    /// <summary>
    /// Synchronously gets an image from a specified URI, handles most errors
    /// by returning an error image, and ignores the DPI of the image.
    /// </summary>
    ///
    /// <param name="uriString">
    /// The URI to get the image from.  If the string is not a valid URI, an
    /// error image is returned.
    /// </param>
    ///
    /// <returns>
    /// The specified image, or an error image if the specified image isn't
    /// available.
    /// </returns>
    ///
    /// <remarks>
    /// There are three differences between using this method and using
    /// BitmapImage(URI): 1) If the URI is an URL, the image is downloaded
    /// synchronously instead of asynchronously; 2) If the image isn't
    /// available, an error image is returned instead of an exception being
    /// thrown; and 3) If the image is marked with a DPI of something other
    /// than 96, the DPI is ignored and the returned image will have a DPI of
    /// 96 with the same dimensions as the original image.  For example, a 
    /// 100x100-pixel image with a DPI of 72 will be returned as a
    /// 100x100-WPF-unit image with a DPI of 96, and on a 96 DPI machine the
    /// returned image will display at the same physical size that the original
    /// image would have in a Web browser or other application that ignores
    /// DPI.
    /// </remarks>
    //*************************************************************************

    public BitmapSource
    GetImageSynchronousIgnoreDpi
    (
        String uriString
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(uriString) );
        AssertValid();

        BitmapSource oBitmapSource = GetImageSynchronous(uriString);

        if (oBitmapSource.DpiX != 96)
        {
            // This is a poor solution.  Is there a way to tell WPF to ignore
            // an image's DPI tags?  I can't find one.

            oBitmapSource = ResizeImage(oBitmapSource,
                oBitmapSource.PixelWidth, oBitmapSource.PixelHeight);
        }

        return (oBitmapSource);
    }

    //*************************************************************************
    //  Method: GetImageSynchronousHttp()
    //
    /// <summary>
    /// Synchronously gets an image from a specified HTTP or HTTPS URI.
    /// </summary>
    ///
    /// <param name="uri">
    /// The URI to get the image from.  Must be HTTP or HTTPS.
    /// </param>
    ///
    /// <returns>
    /// The specified image.
    /// </returns>
    ///
    /// <remarks>
    /// No error handling is performed by this method.
    ///
    /// <para>
    /// This differs from using BitmapImage(URI) in that the image is
    /// downloaded synchronously instead of asynchronously.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public BitmapImage
    GetImageSynchronousHttp
    (
        Uri uri
    )
    {
        Debug.Assert(uri != null);

        Debug.Assert(uri.Scheme == Uri.UriSchemeHttp ||
            uri.Scheme == Uri.UriSchemeHttps);

        AssertValid();

        // Talk about inefficient...
        //
        // The following code uses HttpWebRequest to synchronously download the
        // image into a List<Byte>, then passes that List as a MemoryStream to
        // BitmapImage.StreamSource.  It works, but it involves way too much
        // Byte copying.  There has to be a better way to do this, but so far I
        // haven't found one.
        //
        // In the following post...
        //
        // http://stackoverflow.com/questions/426645/
        // how-to-render-an-image-in-a-background-wpf-process
        //
        // ...the poster suggests feeding the WebResponse.GetResponseStream()
        // indirectly to BitmapImage.StreamSource.  This sometimes works but
        // sometimes doesn't, which you can tell by checking
        // BitmapImage.IsDownloading at the end of his code.  It is true
        // sometimes, indicating that the Bitmap is still downloading.

        HttpWebRequest oHttpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
        oHttpWebRequest.Timeout = HttpWebRequestTimeoutMs;

        oHttpWebRequest.CachePolicy = new RequestCachePolicy(
            RequestCacheLevel.CacheIfAvailable);

        WebResponse oWebResponse = oHttpWebRequest.GetResponse();

        BinaryReader oBinaryReader = new BinaryReader(
            oWebResponse.GetResponseStream() );

        List<Byte> oResponseBytes = new List<Byte>();

        while (true)
        {
            Byte [] abtSomeResponseBytes = oBinaryReader.ReadBytes(8192);

            if (abtSomeResponseBytes.Length == 0)
            {
                break;
            }

            oResponseBytes.AddRange(abtSomeResponseBytes);
        }

        Byte [] abtAllResponseBytes = oResponseBytes.ToArray();
        oResponseBytes = null;

        MemoryStream oMemoryStream = new MemoryStream(abtAllResponseBytes,
            false);

        BitmapImage oBitmapImage = new BitmapImage();
        oBitmapImage.BeginInit();
        oBitmapImage.StreamSource = oMemoryStream;
        oBitmapImage.EndInit();

        WpfGraphicsUtil.FreezeIfFreezable(oBitmapImage);

        return (oBitmapImage);
    }

    //*************************************************************************
    //  Method: CreateErrorImage()
    //
    /// <summary>
    /// Returns a square image to display when an error is encountered.
    /// </summary>
    ///
    /// <param name="widthAndHeight">
    /// The image's width and height, in WPF units.
    /// </param>
    ///
    /// <returns>
    /// An error image.
    /// </returns>
    //*************************************************************************

    public BitmapSource
    CreateErrorImage
    (
        Int32 widthAndHeight
    )
    {
        Debug.Assert(widthAndHeight > 0);
        AssertValid();

        // Draw a square filled with white, outlined in black, and with a red
        // "X".

        const Int32 Margin = 4;
        Int32 iWidthAndHeightMinusMargin = widthAndHeight - Margin;
        DrawingVisual oDrawingVisual = new DrawingVisual();

        DrawingContext oDrawingContext = oDrawingVisual.RenderOpen();
        Rect oRectangle = new Rect( new Size(widthAndHeight, widthAndHeight) );

        oDrawingContext.DrawRectangle(Brushes.White,
            new Pen(Brushes.Black, 1), oRectangle);

        Pen oXPen = new Pen(Brushes.Red, 1);

        oDrawingContext.DrawLine( oXPen,
            new Point(Margin, Margin),
            new Point(iWidthAndHeightMinusMargin, iWidthAndHeightMinusMargin)
            );

        oDrawingContext.DrawLine( oXPen,
            new Point(iWidthAndHeightMinusMargin, Margin),
            new Point(Margin, iWidthAndHeightMinusMargin)
            );

        oDrawingContext.Close();

        return ( DrawingVisualToRenderTargetBitmap(oDrawingVisual,
            widthAndHeight, widthAndHeight) );
    }

    //*************************************************************************
    //  Method: ResizeImage()
    //
    /// <overloads>
    /// Resizes an image.
    /// </overloads>
    ///
    /// <summary>
    /// Resizes an image to specified dimensions.
    /// </summary>
    ///
    /// <param name="image">
    /// The original image.
    /// </param>
    ///
    /// <param name="widthNew">
    /// Width of the resized image, in WPF units.
    /// </param>
    ///
    /// <param name="heightNew">
    /// Height of the resized image, in WPF units.
    /// </param>
    ///
    /// <returns>
    /// A resized copy of <paramref name="image" />.
    /// </returns>
    //*************************************************************************

    public BitmapSource
    ResizeImage
    (
        ImageSource image,
        Int32 widthNew,
        Int32 heightNew
    )
    {
        Debug.Assert(image != null);
        Debug.Assert(widthNew > 0);
        Debug.Assert(heightNew > 0);
        AssertValid();

        DrawingVisual oDrawingVisual = new DrawingVisual();
        DrawingContext oDrawingContext = oDrawingVisual.RenderOpen();

        oDrawingContext.DrawImage( image,
            new Rect( new Point(), new Size(widthNew, heightNew) ) );

        oDrawingContext.Close();

        return ( DrawingVisualToRenderTargetBitmap(oDrawingVisual, widthNew,
            heightNew) );
    }

    //*************************************************************************
    //  Method: ResizeImage()
    //
    /// <summary>
    /// Resizes an image while maintaining its aspect ratio.
    /// </summary>
    ///
    /// <param name="image">
    /// The original image.
    /// </param>
    ///
    /// <param name="longerDimensionNew">
    /// Width or height of the resized image, in WPF units.
    /// </param>
    ///
    /// <returns>
    /// A resized copy of <paramref name="image" />.  The copy has the same
    /// aspect ratio as the original, but its longer dimension is <paramref
    /// name="longerDimensionNew" />.
    /// </returns>
    //*************************************************************************

    public BitmapSource
    ResizeImage
    (
        ImageSource image,
        Int32 longerDimensionNew
    )
    {
        Debug.Assert(image != null);
        Debug.Assert(longerDimensionNew > 0);
        AssertValid();

        Double dWidthOriginal = image.Width;
        Double dHeightOriginal = image.Height;

        Double dLongerDimensionOriginal =
            Math.Max(dWidthOriginal, dHeightOriginal);

        Double dShorterDimensionOriginal =
            Math.Min(dWidthOriginal, dHeightOriginal);

        Debug.Assert(dLongerDimensionOriginal > 0);

        Int32 iShorterDimensionNew = (Int32)( dShorterDimensionOriginal *
            ( (Double)longerDimensionNew / dLongerDimensionOriginal ) );

        iShorterDimensionNew = Math.Max(1, iShorterDimensionNew);

        if (dWidthOriginal > dHeightOriginal)
        {
            return ( ResizeImage(image, longerDimensionNew,
                iShorterDimensionNew) );
        }

        return ( ResizeImage(image, iShorterDimensionNew,
            longerDimensionNew) );
    }

    //*************************************************************************
    //  Method: DrawingVisualToRenderTargetBitmap()
    //
    /// <summary>
    /// Renders a DrawingVisual on a RenderTargetBitmap.
    /// </summary>
    ///
    /// <param name="oDrawingVisual">
    /// The DrawingVisual to render.
    /// </param>
    ///
    /// <param name="iWidth">
    /// Width to use, in WPF units.
    /// </param>
    ///
    /// <param name="iHeight">
    /// Height to use, in WPF units.
    /// </param>
    ///
    /// <returns>
    /// A new, frozen RenderTargetBitmap.
    /// </returns>
    //*************************************************************************

    protected RenderTargetBitmap
    DrawingVisualToRenderTargetBitmap
    (
        DrawingVisual oDrawingVisual,
        Int32 iWidth,
        Int32 iHeight
    )
    {
        Debug.Assert(oDrawingVisual != null);
        Debug.Assert(iWidth > 0);
        Debug.Assert(iHeight > 0);
        AssertValid();

        RenderTargetBitmap oRenderTargetBitmap = new RenderTargetBitmap(
            iWidth, iHeight, 96, 96, PixelFormats.Pbgra32);

        oRenderTargetBitmap.Render(oDrawingVisual);
        WpfGraphicsUtil.FreezeIfFreezable(oRenderTargetBitmap);

        return (oRenderTargetBitmap);
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
        // m_oCachedErrorImage
    }

    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// The timeout to use for Web requests, in milliseconds.

    protected const Int32 HttpWebRequestTimeoutMs = 10 * 1000;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Cached error image returned by GetImageSynchronous(), or null.

    protected BitmapSource m_oCachedErrorImage;
}

}
