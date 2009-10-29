
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NodeXL.Visualization.Wpf;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexShapeConverter
//
/// <summary>
/// Class that converts a vertex shape between values used in the Excel
/// workbook and values used in the NodeXL graph.
/// </summary>
//*****************************************************************************

public class VertexShapeConverter : TextValueConverterBase<VertexShape>
{
    //*************************************************************************
    //  Constructor: VertexShapeConverter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexShapeConverter" />
    /// class.
    /// </summary>
    //*************************************************************************

    public VertexShapeConverter()
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

        // The parenthetical entries are for backward compatibility with older
        // NodeXL workbooks.  Parentheses are not used in newer value
        // converters.

        return ( new GraphValueInfo [] {

            new GraphValueInfo( VertexShape.Circle,
                new String [] {"Circle", "Circle (1)", "1"} ),

            new GraphValueInfo( VertexShape.Disk,
                new String [] {"Disk", "Disk (2)", "2"} ),

            new GraphValueInfo( VertexShape.Sphere,
                new String [] {"Sphere", "Sphere (3)", "3"} ),

            new GraphValueInfo( VertexShape.Square,
                new String [] {"Square", "Square (4)", "4"} ),

            new GraphValueInfo( VertexShape.SolidSquare,
                new String [] {"Solid Square", "Solid Square (5)", "5"} ),

            new GraphValueInfo( VertexShape.Diamond,
                new String [] {"Diamond", "Diamond (6)", "6"} ),

            new GraphValueInfo( VertexShape.SolidDiamond,
                new String [] {"Solid Diamond", "Solid Diamond (7)", "7"} ),

            new GraphValueInfo( VertexShape.Triangle,
                new String [] {"Triangle", "Triangle (8)", "8"} ),

            new GraphValueInfo( VertexShape.SolidTriangle,
                new String [] {"Solid Triangle", "Solid Triangle (9)", "9"} ),

            new GraphValueInfo( VertexShape.Label,
                new String [] {"Label", "Label (10)", "10"} ),

            new GraphValueInfo( VertexShape.Image,
                new String [] {"Image", "Image (11)", "11"} ),
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
