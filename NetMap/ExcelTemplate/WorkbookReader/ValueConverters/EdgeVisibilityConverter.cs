
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: EdgeVisibilityConverter
//
/// <summary>
/// Class that converts an edge visibility between values used in the Excel
/// workbook and values used in the NetMap graph.
/// </summary>
//*****************************************************************************

public class EdgeVisibilityConverter :
	TextValueConverterBase<EdgeWorksheetReader.Visibility>
{
    //*************************************************************************
    //  Constructor: EdgeVisibilityConverter()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="EdgeVisibilityConverter" /> class.
    /// </summary>
    //*************************************************************************

    public EdgeVisibilityConverter()
    {
		// (Do nothing.)

		AssertValid();
    }

	//*************************************************************************
	//	Method: GetGraphValueInfos()
	//
	/// <summary>
	///	Gets an array of GraphValueInfo objects, one for each valid graph
	/// value.
	/// </summary>
	//*************************************************************************

	protected override GraphValueInfo []
	GetGraphValueInfos()
	{
		AssertValid();

		return ( new GraphValueInfo [] {

			new GraphValueInfo( EdgeWorksheetReader.Visibility.Show,
				new String [] {"Show (1)", "1",} ),

			new GraphValueInfo( EdgeWorksheetReader.Visibility.Skip,
				new String [] {"Skip (0)", "0",} ),

			new GraphValueInfo( EdgeWorksheetReader.Visibility.Hide,
				new String [] {"Hide (2)", "2",} ),
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
