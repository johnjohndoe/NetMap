
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: ImageFormatControl
//
/// <summary>
/// Control for getting the format and dimensions to use when saving an image
/// to a file.
/// </summary>
///
/// <remarks>
/// Set the <see cref="ImageFormat" /> and <see cref="ImageSizePx" />
/// properites after the control is created.  To retrieve the edited values,
/// call <see cref="Validate" />, and if <see cref="Validate" /> returns true,
/// read the <see cref="ImageFormat" /> and <see cref="ImageSizePx" />
/// properties.
/// </remarks>
///
/// <para>
/// This control uses the following access keys: W, H, F.
/// </para>
//*****************************************************************************

public partial class ImageFormatControl : UserControl
{
    //*************************************************************************
    //  Constructor: ImageFormatControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ImageFormatControl" />
    /// class.
    /// </summary>
    //*************************************************************************

    public ImageFormatControl()
    {
        InitializeComponent();

        m_oImageFormat = ImageFormat.Png;
        m_oImageSizePx = new Size(400, 200);

        SaveableImageFormats.InitializeListControl(cbxImageFormat);

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Property: ImageFormat
    //
    /// <summary>
    /// Gets or sets the format of the image.
    /// </summary>
    ///
    /// <value>
    /// The format of the image, as an ImageFormat value.  The default is
    /// ImageFormat.Png.
    /// </value>
    //*************************************************************************

    public ImageFormat
    ImageFormat
    {
        get
        {
            AssertValid();

            return (m_oImageFormat);
        }

        set
        {
            m_oImageFormat = value;
            DoDataExchange(false);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ImageSizePx
    //
    /// <summary>
    /// Gets or sets the image size to use.
    /// </summary>
    ///
    /// <value>
    /// The image size to use, in pixels.  The default is 400,200.
    /// </value>
    //*************************************************************************

    public Size
    ImageSizePx
    {
        get
        {
            AssertValid();

            return (m_oImageSizePx);
        }

        set
        {
            m_oImageSizePx = value;
            DoDataExchange(false);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: Validate()
    //
    /// <summary>
    /// Validates the user's settings.
    /// </summary>
    ///
    /// <returns>
    /// true if the validation was successful.
    /// </returns>
    ///
    /// <remarks>
    /// If validation fails, an error message is displayed and false is
    /// returned.
    /// </remarks>
    //*************************************************************************

    public new Boolean
    Validate()
    {
        AssertValid();

        return ( DoDataExchange(true) );
    }

    //*************************************************************************
    //  Method: DoDataExchange()
    //
    /// <summary>
    /// Transfers data between the control's fields and its controls.
    /// </summary>
    ///
    /// <param name="bFromControls">
    /// true to transfer data from the control's controls to its fields, false
    /// for the other direction.
    /// </param>
    ///
    /// <returns>
    /// true if the transfer was successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    DoDataExchange
    (
        Boolean bFromControls
    )
    {
        AssertValid();

        if (bFromControls)
        {
            Int32 iImageWidthPx = Int32.MinValue;
            Int32 iImageHeightPx = Int32.MinValue;

            if (
                !FormUtil.ValidateNumericUpDown(nudImageWidthPx,
                    "an image width", out iImageWidthPx)
                ||
                !FormUtil.ValidateNumericUpDown(nudImageHeightPx,
                    "an image height", out iImageHeightPx)
                )
            {
                return (false);
            }

            m_oImageFormat = (ImageFormat)cbxImageFormat.SelectedValue;
            m_oImageSizePx = new Size(iImageWidthPx, iImageHeightPx);
        }
        else
        {
            cbxImageFormat.SelectedValue = m_oImageFormat;
            nudImageWidthPx.Value = m_oImageSizePx.Width;
            nudImageHeightPx.Value = m_oImageSizePx.Height;
        }

        return (true);
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
        // m_oImageFormat
        // m_oImageSizePx
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The image format.

    protected ImageFormat m_oImageFormat;

    /// The image size, in pixels.

    protected Size m_oImageSizePx;
}

}
