
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphZoomAndScaleUserSettings
//
/// <summary>
/// Stores the user's settings for graph zoom and scale.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("GraphZoomAndScaleUserSettings") ]

public class GraphZoomAndScaleUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: GraphZoomAndScaleUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the GraphZoomAndScaleUserSettings class.
    /// </summary>
    //*************************************************************************

    public GraphZoomAndScaleUserSettings()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: GraphScale
    //
    /// <summary>
    /// Gets or sets a value that determines the scale of the graph's vertices
    /// and edges.
    /// </summary>
    ///
    /// <value>
    /// A value that determines the scale of the graph's vertices and edges.
    /// This is used for the value of the <see
    /// cref="Microsoft.NodeXL.Visualization.Wpf.NodeXLControl.GraphScale" />
    /// property.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("1.0") ]

    public Double
    GraphScale
    {
        get
        {
            AssertValid();

            return ( (Double)this[GraphScaleKey] );
        }

        set
        {
            this[GraphScaleKey] = value;

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

    /// Name of the settings key for the GraphScale property.

    protected const String GraphScaleKey =
        "GraphScale";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
