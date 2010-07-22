
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AutomatedGraphImageUserSettings
//
/// <summary>
/// Stores the user's settings for saving graph images during task automation.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("AutomatedGraphImageUserSettings") ]

public class AutomatedGraphImageUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: AutomatedGraphImageUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the AutomatedGraphImageUserSettings
    /// class.
    /// </summary>
    //*************************************************************************

    public AutomatedGraphImageUserSettings()
    {
        // (Do nothing.)

        AssertValid();
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

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("400, 200") ]

    public Size
    ImageSizePx
    {
        get
        {
            AssertValid();

            return ( (Size)this[ImageSizePxKey] );
        }

        set
        {
            this[ImageSizePxKey] = value;

            AssertValid();
        }
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

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("Png") ]

    public ImageFormat
    ImageFormat
    {
        get
        {
            AssertValid();

            return ( (ImageFormat)this[ImageFormatKey] );
        }

        set
        {
            this[ImageFormatKey] = value;

            AssertValid();
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Name of the settings key for the ImageSizePx property.

    protected const String ImageSizePxKey = "ImageSizePx";

    /// Name of the settings key for the ImageFormat property.

    protected const String ImageFormatKey = "ImageFormat";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
