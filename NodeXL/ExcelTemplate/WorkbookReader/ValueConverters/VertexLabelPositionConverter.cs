
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NodeXL.Visualization.Wpf;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexLabelPositionConverter
//
/// <summary>
/// Class that converts a vertex label position between values used in the
/// Excel workbook and values used in the NodeXL graph.
/// </summary>
//*****************************************************************************

public class VertexLabelPositionConverter :
    TextValueConverterBase<VertexLabelPosition>
{
    //*************************************************************************
    //  Constructor: VertexLabelPositionConverter()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VertexLabelPositionConverter" /> class.
    /// </summary>
    //*************************************************************************

    public VertexLabelPositionConverter()
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

            new GraphValueInfo( VertexLabelPosition.TopLeft,
                new String [] {"Top Left", "1"} ),

            new GraphValueInfo( VertexLabelPosition.TopCenter,
                new String [] {"Top Center", "2"} ),

            new GraphValueInfo( VertexLabelPosition.TopRight,
                new String [] {"Top Right", "3"} ),

            new GraphValueInfo( VertexLabelPosition.MiddleLeft,
                new String [] {"Middle Left", "4"} ),

            new GraphValueInfo( VertexLabelPosition.MiddleCenter,
                new String [] {"Middle Center", "5"} ),

            new GraphValueInfo( VertexLabelPosition.MiddleRight,
                new String [] {"Middle Right", "6"} ),

            new GraphValueInfo( VertexLabelPosition.BottomLeft,
                new String [] {"Bottom Left", "7"} ),

            new GraphValueInfo( VertexLabelPosition.BottomCenter,
                new String [] {"Bottom Center", "8"} ),

            new GraphValueInfo( VertexLabelPosition.BottomRight,
                new String [] {"Bottom Right", "9"} ),
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
