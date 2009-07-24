
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;

namespace Microsoft.NodeXL.Visualization.Wpf
{
public partial class NodeXLControl
{
//*****************************************************************************
//  Embedded class: GraphImageCenterer
//
/// <summary>
/// Centers a graph image for the <see cref="NodeXLControl" /> and later
/// restores the original graph location.
/// </summary>
///
/// <remarks>
/// This prepares the <see cref="NodeXLControl" /> for being saved as an image.
/// It adjusts the control's transforms so that the image will be centered on
/// the same point on the graph that the control is centered on.  This prevents
/// an unwanted graph shift when the image has dimensions different from those
/// of the control.
///
/// <para>
/// Because the NodeXLControl's transforms are not considered public, this
/// class is embedded within the control and can be used only by the control
/// and its derived classes.
/// </para>
///
/// <para>
/// Pass an image size to <see cref="CenterGraphImage" />, which immediately
/// centers the graph within the image dimensions.  Call <see
/// cref="RestoreCenter" /> to restore the graph to its original location.
/// </para>
///
/// </remarks>
//*****************************************************************************

protected class GraphImageCenterer : Object
{
    //*************************************************************************
    //  Constructor: GraphImageCenterer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphImageCenterer" />
    /// class.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// The control for which a graph image will be created.
    /// </param>
    //*************************************************************************

    public GraphImageCenterer
    (
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(nodeXLControl != null);

        m_oNodeXLControl = nodeXLControl;

        m_dOriginalTranslateTransformForRenderX =
            m_dOriginalTranslateTransformForRenderY =
            m_dOriginalScaleTransformForRenderCenterX =
            m_dOriginalScaleTransformForRenderCenterY =
            Double.MinValue;

        AssertValid();
    }

    //*************************************************************************
    //  Method: CenterGraphImage()
    //
    /// <summary>
    /// Adjusts the control's transforms so that the image will be centered on
    /// the same point on the graph that the control is centered on.
    /// </summary>
    ///
    /// <param name="imageSize">
    /// The size of the image.
    /// </param>
    //*************************************************************************

    public void
    CenterGraphImage
    (
        Size imageSize
    )
    {
        AssertValid();

        // Get the transforms used by the control.

        ScaleTransform oScaleTransformForLayout =
            m_oNodeXLControl.ScaleTransformForLayout;

        TranslateTransform oTranslateTransformForRender =
            m_oNodeXLControl.TranslateTransformForRender;

        ScaleTransform oScaleTransformForRender =
            m_oNodeXLControl.ScaleTransformForRender;

        // Save the transform settings that will be modified.

        m_dOriginalTranslateTransformForRenderX =
            oTranslateTransformForRender.X;

        m_dOriginalTranslateTransformForRenderY =
            oTranslateTransformForRender.Y;

        m_dOriginalScaleTransformForRenderCenterX =
            oScaleTransformForRender.CenterX;

        m_dOriginalScaleTransformForRenderCenterY =
            oScaleTransformForRender.CenterY;

        Double dUnscaledControlWidth =
            m_oNodeXLControl.ActualWidth * oScaleTransformForLayout.ScaleX;

        Double dUnscaledControlHeight =
            m_oNodeXLControl.ActualHeight * oScaleTransformForLayout.ScaleY;

        Double dWidthRatio = imageSize.Width / dUnscaledControlWidth;
        Double dHeightRatio = imageSize.Height / dUnscaledControlHeight;

        // Two transforms affect the graph location.  Modify them.

        oTranslateTransformForRender.X *= dWidthRatio;
        oTranslateTransformForRender.Y *= dHeightRatio;

        oScaleTransformForRender.CenterX *= dWidthRatio;
        oScaleTransformForRender.CenterY *= dHeightRatio;

        AssertValid();
    }

    //*************************************************************************
    //  Method: RestoreCenter()
    //
    /// <summary>
    /// Restores the control so the graph is in its original location.
    /// </summary>
    ///
    /// <remarks>
    /// If <see cref="CenterGraphImage" /> hasn't been called, this method does
    /// nothing.
    /// </remarks>
    //*************************************************************************

    public void
    RestoreCenter()
    {
        AssertValid();

        if (m_dOriginalTranslateTransformForRenderX == Double.MinValue)
        {
            return;
        }

        TranslateTransform oTranslateTransformForRender =
            m_oNodeXLControl.TranslateTransformForRender;

        ScaleTransform oScaleTransformForRender =
            m_oNodeXLControl.ScaleTransformForRender;

        oTranslateTransformForRender.X =
            m_dOriginalTranslateTransformForRenderX;

        oTranslateTransformForRender.Y =
            m_dOriginalTranslateTransformForRenderY;

        oScaleTransformForRender.CenterX =
            m_dOriginalScaleTransformForRenderCenterX;

        oScaleTransformForRender.CenterY =
            m_dOriginalScaleTransformForRenderCenterY;
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
        Debug.Assert(m_oNodeXLControl != null);
        // m_dOriginalTranslateTransformForRenderX
        // m_dOriginalTranslateTransformForRenderY
        // m_dOriginalScaleTransformForRenderCenterX
        // m_dOriginalScaleTransformForRenderCenterY
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The control for which a graph image will be created.

    protected NodeXLControl m_oNodeXLControl;

    /// Original transform values, set by CenterGraphImage().

    protected Double m_dOriginalTranslateTransformForRenderX;
    ///
    protected Double m_dOriginalTranslateTransformForRenderY;
    ///
    protected Double m_dOriginalScaleTransformForRenderCenterX;
    ///
    protected Double m_dOriginalScaleTransformForRenderCenterY;
}
}

}
