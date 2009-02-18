
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

        return ( new GraphValueInfo [] {

            new GraphValueInfo( VertexShape.Circle,
                new String [] {"Circle (1)", "Circle", "1"} ),

            new GraphValueInfo( VertexShape.Disk,
                new String [] {"Disk (2)", "Disk", "2"} ),

            new GraphValueInfo( VertexShape.Sphere,
                new String [] {"Sphere (3)", "Sphere", "3"} ),

            new GraphValueInfo( VertexShape.Square,
                new String [] {"Square (4)", "Square", "4"} ),

            new GraphValueInfo( VertexShape.SolidSquare,
                new String [] {"Solid Square (5)", "Solid Square", "5"} ),

            new GraphValueInfo( VertexShape.Diamond,
                new String [] {"Diamond (6)", "Diamond", "6"} ),

            new GraphValueInfo( VertexShape.SolidDiamond,
                new String [] {"Solid Diamond (7)", "Solid Diamond", "7"} ),

            new GraphValueInfo( VertexShape.Triangle,
                new String [] {"Triangle (8)", "Triangle", "8"} ),

            new GraphValueInfo( VertexShape.SolidTriangle,
                new String [] {"Solid Triangle (9)", "Solid Triangle", "9"} ),
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
