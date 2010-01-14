
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.NodeXL.Visualization.Wpf;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexRadiusConverter
//
/// <summary>
/// Class that converts a vertex radius between values used in the Excel
/// workbook and values used in the NodeXL graph.
/// </summary>
//*****************************************************************************

public class VertexRadiusConverter : NumericValueConverterBase
{
    //*************************************************************************
    //  Constructor: VertexRadiusConverter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexRadiusConverter" />
    /// class.
    /// </summary>
    //*************************************************************************

    public VertexRadiusConverter()
    :
    base(MinimumRadiusWorkbook, MaximumRadiusWorkbook, MinimumRadiusGraph,
        MaximumRadiusGraph)
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: WorkbookToLongerImageDimension()
    //
    /// <summary>
    /// Converts an Excel workbook value to an image dimension.
    /// </summary>
    ///
    /// <param name="valueWorkbook">
    /// Value read from the Excel workbook.
    /// </param>
    ///
    /// <returns>
    /// The maximum width or height a vertex image can have.
    /// </returns>
    //*************************************************************************

    public Single
    WorkbookToLongerImageDimension
    (
        Single valueWorkbook
    )
    {
        AssertValid();

        return ( RangeAToRangeB(valueWorkbook, m_fMinimumValueWorkbook,
            m_fMaximumValueWorkbook, MinimumLongerImageDimension,
            MaximumLongerImageDimension) );
    }

    //*************************************************************************
    //  Method: WorkbookToLabelFontSize()
    //
    /// <summary>
    /// Converts an Excel workbook value to a font size to use for a vertex
    /// with the shape <see cref="VertexShape.Label" />.
    /// </summary>
    ///
    /// <param name="valueWorkbook">
    /// Value read from the Excel workbook.
    /// </param>
    ///
    /// <returns>
    /// The font size to use, in WPF units.
    /// </returns>
    //*************************************************************************

    public Single
    WorkbookToLabelFontSize
    (
        Single valueWorkbook
    )
    {
        AssertValid();

        return ( RangeAToRangeB(valueWorkbook, m_fMinimumValueWorkbook,
            m_fMaximumValueWorkbook, MinimumLabelFontSize,
            MaximumLabelFontSize) );
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
    /// Minimum value that can be specified in the workbook for a vertex
    /// radius.
    /// </summary>

    public static readonly Single MinimumRadiusWorkbook = 1F;

    /// <summary>
    /// Maximum value that can be specified in the workbook for a vertex
    /// radius.
    /// </summary>

    public static readonly Single MaximumRadiusWorkbook = 100F;

    /// <summary>
    /// Minimum radius in the NodeXL graph.
    /// </summary>

    public static readonly Single MinimumRadiusGraph =
        (Single)VertexDrawer.MinimumRadius;

    /// <summary>
    /// Maximum radius in the NodeXL graph.
    /// </summary>

    public static readonly Single MaximumRadiusGraph =
        (Single)VertexDrawer.MaximumRadius;

    /// <summary>
    /// The vertex image width or height (whichever is larger) that corresponds
    /// to MinimumRadiusWorkbook, in WPF units.
    /// </summary>

    public static readonly Single MinimumLongerImageDimension = 10F;

    /// <summary>
    /// The vertex image width or height (whichever is larger) that corresponds
    /// to MaximumRadiusWorkbook, in WPF units.
    /// </summary>

    public static readonly Single MaximumLongerImageDimension = 2100F;

    /// <summary>
    /// The vertex label font size that corresponds to MinimumRadiusWorkbook,
    /// in WPF units.
    /// </summary>

    public static readonly Single MinimumLabelFontSize = 14F;

    /// <summary>
    /// The vertex label font size that corresponds to MaximumRadiusWorkbook,
    /// in WPF units.
    /// </summary>

    public static readonly Single MaximumLabelFontSize = 293.4F;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
