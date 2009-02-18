
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.NodeXL.Visualization.Wpf;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: EdgeWidthConverter
//
/// <summary>
/// Class that converts an edge width between values used in the Excel workbook
/// and values used in the NodeXL graph.
/// </summary>
//*****************************************************************************

public class EdgeWidthConverter : NumericValueConverterBase
{
    //*************************************************************************
    //  Constructor: EdgeWidthConverter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeWidthConverter" />
    /// class.
    /// </summary>
    //*************************************************************************

    public EdgeWidthConverter()
    :
    base(MinimumWidthWorkbook, MaximumWidthWorkbook, MinimumWidthGraph,
        MaximumWidthGraph)
    {
        // (Do nothing.)

        AssertValid();
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
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// Minimum value that can be specified in the workbook for an edge width.
    /// </summary>

    public static readonly Single MinimumWidthWorkbook = 1F;

    /// <summary>
    /// Maximum value that can be specified in the workbook for an edge width.
    /// </summary>

    public static readonly Single MaximumWidthWorkbook = 10F;

    /// <summary>
    /// Minimum edge width in the NodeXL graph.
    /// </summary>

    public static readonly Single MinimumWidthGraph =
        (Single)EdgeDrawer.MinimumWidth;

    /// <summary>
    /// Maximum edge width in the NodeXL graph.
    /// </summary>

    public static readonly Single MaximumWidthGraph = 
        (Single)EdgeDrawer.MaximumWidth;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
