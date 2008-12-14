
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.NodeXL.Visualization;
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

	public static readonly Single MaximumRadiusWorkbook = 10F;

	/// <summary>
	/// Minimum radius in the NodeXL graph.
	/// </summary>

	public static readonly Single MinimumRadiusGraph =
		PerVertexDrawer.MinimumRadius;

	/// <summary>
	/// Maximum radius in the NodeXL graph.
	/// </summary>

	public static readonly Single MaximumRadiusGraph =
		PerVertexDrawer.MaximumRadius;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
