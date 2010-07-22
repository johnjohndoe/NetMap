
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Drawing;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphImageUserSettings
//
/// <summary>
/// Stores the user's settings for saving graph images.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("GraphImageUserSettings2") ]

public class GraphImageUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: GraphImageUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the GraphImageUserSettings class.
    /// </summary>
    //*************************************************************************

    public GraphImageUserSettings()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: UseControlSize
    //
    /// <summary>
    /// Gets or sets a flag specifying whether the size of the NodeXLControl
    /// should be used when saving a graph image.
    /// </summary>
    ///
    /// <value>
    /// true to use the NodeXLControl size, false to use <see
    /// cref="ImageSize" />.  The default is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    UseControlSize
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[UseControlSizeKey] );
        }

        set
        {
            this[UseControlSizeKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ImageSize
    //
    /// <summary>
    /// Gets or sets the size to use when saving a graph image.
    /// </summary>
    ///
    /// <value>
    /// The size to use when saving a graph image.  The default is 400,200.
    /// </value>
    ///
    /// <remarks>
    /// This is used only if <see cref="UseControlSize" /> is false.
    ///
    /// <para>
    /// If saving to XPS, the units are 1/100 of an inch.  Otherwise, the units
    /// are pixels.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("400, 200") ]

    public Size
    ImageSize
    {
        get
        {
            AssertValid();

            return ( (Size)this[ImageSizeKey] );
        }

        set
        {
            this[ImageSizeKey] = value;

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

    /// Name of the settings key for the UseControlSize property.

    protected const String UseControlSizeKey =
        "UseControlSize";

    /// Name of the settings key for the ImageSize property.

    protected const String ImageSizeKey = "ImageSize";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
