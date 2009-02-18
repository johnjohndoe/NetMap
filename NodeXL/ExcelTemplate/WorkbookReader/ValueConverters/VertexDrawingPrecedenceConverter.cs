
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NodeXL.Visualization.Wpf;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexDrawingPrecedenceConverter
//
/// <summary>
/// Class that converts a vertex drawing precedence between values used in the
/// Excel workbook and values used in the NodeXL graph.
/// </summary>
//*****************************************************************************

public class VertexDrawingPrecedenceConverter :
    TextValueConverterBase<VertexDrawingPrecedence>
{
    //*************************************************************************
    //  Constructor: VertexDrawingPrecedenceConverter()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VertexDrawingPrecedenceConverter" /> class.
    /// </summary>
    //*************************************************************************

    public VertexDrawingPrecedenceConverter()
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

            new GraphValueInfo( VertexDrawingPrecedence.Shape,
                new String [] {"Shape (1)", "1",} ),

            new GraphValueInfo( VertexDrawingPrecedence.Image,
                new String [] {"Image (2)", "2",} ),

            new GraphValueInfo( VertexDrawingPrecedence.PrimaryLabel,
                new String [] {"Primary Label (3)", "3",} ),
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
