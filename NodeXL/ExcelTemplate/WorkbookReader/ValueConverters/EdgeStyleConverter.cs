
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NodeXL.Visualization.Wpf;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: EdgeStyleConverter
//
/// <summary>
/// Class that converts an edge style between values used in the Excel workbook
/// and values used in the NodeXL graph.
/// </summary>
//*****************************************************************************

public class EdgeStyleConverter : TextValueConverterBase<EdgeStyle>
{
    //*************************************************************************
    //  Constructor: EdgeStyleConverter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeStyleConverter" />
    /// class.
    /// </summary>
    //*************************************************************************

    public EdgeStyleConverter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: GetGraphValueInfos()
    //
    /// <summary>
    /// Gets an array of GraphValueInfo objects, one for each valid graph
    /// value.
    /// </summary>
    //*************************************************************************

    protected override GraphValueInfo []
    GetGraphValueInfos()
    {
        AssertValid();

        return ( new GraphValueInfo [] {

            new GraphValueInfo( EdgeStyle.Solid,
                new String [] {"Solid", "1"} ),

            new GraphValueInfo( EdgeStyle.Dash,
                new String [] {"Dash", "2"} ),

            new GraphValueInfo( EdgeStyle.Dot,
                new String [] {"Dot", "3"} ),

            new GraphValueInfo( EdgeStyle.DashDot,
                new String [] {"Dash Dot", "4"} ),

            new GraphValueInfo( EdgeStyle.DashDotDot,
                new String [] {"Dash Dot Dot", "5"} ),
            } );
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
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
