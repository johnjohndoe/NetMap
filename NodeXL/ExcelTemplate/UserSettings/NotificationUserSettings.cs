
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NotificationUserSettings
//
/// <summary>
/// Stores the user's settings for notifications that can be disabled by the
/// user.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("NotificationUserSettings") ]

public class NotificationUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: NotificationUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the NotificationUserSettings class.
    /// </summary>
    //*************************************************************************

    public NotificationUserSettings()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: LayoutTypeIsNull
    //
    /// <summary>
    /// Gets or sets a flag specifying whether the user should be warned that
    /// the layout type is set to LayoutType.Null when the workbook is read.
    /// </summary>
    ///
    /// <value>
    /// true to warn the user.  The default is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    LayoutTypeIsNull
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[LayoutTypeIsNullKey] );
        }

        set
        {
            this[LayoutTypeIsNullKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: EnableAllNotifications()
    //
    /// <summary>
    /// Enables all user notifications maintained by this class.
    /// </summary>
    //*************************************************************************

    public void
    EnableAllNotifications()
    {
        this.LayoutTypeIsNull = true;
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

    /// Name of the settings key for the LayoutTypeIsNull property.

    protected const String LayoutTypeIsNullKey =
        "LayoutTypeIsNull";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
