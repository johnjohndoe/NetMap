
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.NodeXL.Visualization;
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
    //  Method: WorkbookToGraph()
    //
    /// <summary>
	/// Converts an edge width from an Excel workbook value to a value suitable
	/// for use in a NodeXL graph.
    /// </summary>
    ///
    /// <param name="workbookValue">
    /// Edge width read from the Excel workbook.  If less than <see
	/// cref="MinimumWidthWorkbook" />, <see cref="MinimumWidthGraph" />
	/// is returned.  If greater than <see cref="MaximumWidthWorkbook" />,
	/// <see cref="MaximumWidthGraph" /> is returned.
    /// </param>
    ///
    /// <returns>
	/// An edge width suitable for use in a NodeXL graph.
    /// </returns>
    //*************************************************************************

    public new Int32
    WorkbookToGraph
    (
		Single workbookValue
    )
    {
        AssertValid();

		return ( (Int32)base.WorkbookToGraph(workbookValue) );
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

	public static readonly Int32 MinimumWidthGraph =
		PerEdgeDrawer.MinimumWidth;

	/// <summary>
	/// Maximum edge width in the NodeXL graph.
	/// </summary>

	public static readonly Int32 MaximumWidthGraph =
		PerEdgeDrawer.MaximumWidth;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
