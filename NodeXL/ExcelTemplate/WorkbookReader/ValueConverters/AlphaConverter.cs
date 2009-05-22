
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AlphaConverter
//
/// <summary>
/// Class that converts alpha values between those used in the Excel workbook
/// and those used in the NodeXL graph.
/// </summary>
//*****************************************************************************

public class AlphaConverter : NumericValueConverterBase
{
    //*************************************************************************
    //  Constructor: AlphaConverter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="AlphaConverter" /> class.
    /// </summary>
    //*************************************************************************

    public AlphaConverter()

    : base(MinimumAlphaWorkbook, MaximumAlphaWorkbook, MinimumAlphaGraph,
        MaximumAlphaGraph)
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: WorkbookToGraph()
    //
    /// <summary>
    /// Converts an alpha value from an Excel workbook value to a value
    /// suitable for use in a NodeXL graph.
    /// </summary>
    ///
    /// <param name="workbookValue">
    /// Alpha value read from the Excel workbook.  If less than <see
    /// cref="MinimumAlphaWorkbook" />, <see cref="MinimumAlphaGraph" />
    /// is returned.  If greater than <see cref="MaximumAlphaWorkbook" />,
    /// <see cref="MaximumAlphaGraph" /> is returned.
    /// </param>
    ///
    /// <returns>
    /// A alpha value suitable for use in a NodeXL graph.
    /// </returns>
    //*************************************************************************

    public new Byte
    WorkbookToGraph
    (
        Single workbookValue
    )
    {
        AssertValid();

        return ( (Byte)base.WorkbookToGraph(workbookValue) );
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
    /// Minimum value that can be specified in the workbook for an alpha value.
    /// Represents "transparent."
    /// </summary>

    public static readonly Single MinimumAlphaWorkbook = 0F;

    /// <summary>
    /// Maximum value that can be specified in the workbook for an alpha value.
    /// Represents "opaque."
    /// </summary>

    public static readonly Single MaximumAlphaWorkbook = 100F;

    /// <summary>
    /// Minimum alpha value in the NodeXL graph.  Represents "transparent."
    /// </summary>

    public static readonly Byte MinimumAlphaGraph = 0;

    /// <summary>
    /// Maximum alpha value in the NodeXL graph.  Represents "opaque."
    /// </summary>

    public static readonly Byte MaximumAlphaGraph = 255;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
