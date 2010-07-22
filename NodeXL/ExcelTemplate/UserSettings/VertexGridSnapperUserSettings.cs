
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexGridSnapperUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="Microsoft.NodeXL.Layouts.VertexGridSnapper" /> class.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("VertexGridSnapperUserSettings") ]

public class VertexGridSnapperUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: VertexGridSnapperUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the VertexGridSnapperUserSettings class.
    /// </summary>
    //*************************************************************************

    public VertexGridSnapperUserSettings()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: GridSize
    //
    /// <summary>
    /// Gets or sets the distance between grid lines.
    /// </summary>
    ///
    /// <value>
    /// The distance between grid lines.  The default value is 50.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("50") ]

    public Int32
    GridSize
    {
        get
        {
            AssertValid();

            return ( (Int32)this[GridSizeKey] );
        }

        set
        {
            this[GridSizeKey] = value;

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

    /// Name of the settings key for the GridSize property.

    protected const String GridSizeKey =
        "GridSize";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
