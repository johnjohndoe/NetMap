
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: AlphaConverter
//
/// <summary>
/// Class that converts alpha values between those used in the Excel workbook
/// and those used in the NetMap graph.
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
    //  Property: MaximumAlphaMessage
    //
    /// <summary>
    /// Gets a message suitable for display in a dialog telling the user what
	/// the maximum alpha value represents.
    /// </summary>
    ///
    /// <value>
    /// A message suitable for display in a dialog.
    /// </value>
    //*************************************************************************

    public static String
	MaximumAlphaMessage
    {
        get
        {
			return ( String.Format(
				"({0} is opaque)"
				,
				(Int32)MaximumAlphaWorkbook
				) );
        }
    }

    //*************************************************************************
    //  Method: WorkbookToGraph()
    //
    /// <summary>
	/// Converts an alpha value from an Excel workbook value to a value
	/// suitable for use in a NetMap graph.
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
	/// A alpha value suitable for use in a NetMap graph.
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
	/// Minimum value that can be specified in the workbook for an alpha value.
	/// Represents "transparent."
	/// </summary>

	public static readonly Single MinimumAlphaWorkbook = 0F;

	/// <summary>
	/// Maximum value that can be specified in the workbook for an alpha value.
	/// Represents "opaque."
	/// </summary>

	public static readonly Single MaximumAlphaWorkbook = 10F;

	/// <summary>
	/// Minimum alpha value in the NetMap graph.  Represents "transparent."
	/// </summary>

	public static readonly Int32 MinimumAlphaGraph = 0;

	/// <summary>
	/// Maximum alpha value in the NetMap graph.  Represents "opaque."
	/// </summary>

	public static readonly Int32 MaximumAlphaGraph = 255;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
