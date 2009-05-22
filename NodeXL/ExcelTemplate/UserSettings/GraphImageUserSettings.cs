
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
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

[ SettingsGroupNameAttribute("GraphImageUserSettings") ]

public class GraphImageUserSettings : ApplicationSettingsBase
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
    /// true to use the NodeXLControl size, false to use <see cref="WidthPx" />
    /// and <see cref="HeightPx" />.  The default is true.
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
    //  Property: WidthPx
    //
    /// <summary>
    /// Gets or sets the width to use when saving a graph image.
    /// </summary>
    ///
    /// <value>
    /// The width to use when saving a graph image, in pixels.  The default is
    /// 400.
    /// </value>
    ///
    /// <remarks>
    /// This is used only if <see cref="UseControlSize" /> is false.
    /// </remarks>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("400") ]

    public Int32
    WidthPx
    {
        get
        {
            AssertValid();

            return ( (Int32)this[WidthPxKey] );
        }

        set
        {
            this[WidthPxKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: HeightPx
    //
    /// <summary>
    /// Gets or sets the height to use when saving a graph image.
    /// </summary>
    ///
    /// <value>
    /// The height to use when saving a graph image, in pixels.  The default is
    /// 200.
    /// </value>
    ///
    /// <remarks>
    /// This is used only if <see cref="UseControlSize" /> is false.
    /// </remarks>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("200") ]

    public Int32
    HeightPx
    {
        get
        {
            AssertValid();

            return ( (Int32)this[HeightPxKey] );
        }

        set
        {
            this[HeightPxKey] = value;

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

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Name of the settings key for the UseControlSize property.

    protected const String UseControlSizeKey =
        "UseControlSize";

    /// Name of the settings key for the WidthPx property.

    protected const String WidthPxKey =
        "WidthPx";

    /// Name of the settings key for the HeightPx property.

    protected const String HeightPxKey =
        "HeightPx";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
