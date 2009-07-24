
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

[ SettingsGroupNameAttribute("GraphImageUserSettings2") ]

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
    /// true to use the NodeXLControl size, false to use <see cref="Width" />
    /// and <see cref="Height" />.  The default is true.
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
    //  Property: Width
    //
    /// <summary>
    /// Gets or sets the width to use when saving a graph image.
    /// </summary>
    ///
    /// <value>
    /// The width to use when saving a graph image.  The default is 400.
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
    [ DefaultSettingValueAttribute("400") ]

    public Int32
    Width
    {
        get
        {
            AssertValid();

            return ( (Int32)this[WidthKey] );
        }

        set
        {
            this[WidthKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Height
    //
    /// <summary>
    /// Gets or sets the height to use when saving a graph image.
    /// </summary>
    ///
    /// <value>
    /// The height to use when saving a graph image.  The default is 200.
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
    [ DefaultSettingValueAttribute("200") ]

    public Int32
    Height
    {
        get
        {
            AssertValid();

            return ( (Int32)this[HeightKey] );
        }

        set
        {
            this[HeightKey] = value;

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

    /// Name of the settings key for the Width property.

    protected const String WidthKey =
        "Width";

    /// Name of the settings key for the Height property.

    protected const String HeightKey =
        "Height";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
