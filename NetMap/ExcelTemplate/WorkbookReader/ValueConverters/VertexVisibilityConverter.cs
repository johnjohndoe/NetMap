
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexVisibilityConverter
//
/// <summary>
/// Class that converts a vertex visibility between values used in the Excel
/// workbook and values used in the NetMap graph.
/// </summary>
//*****************************************************************************

public class VertexVisibilityConverter :
	TextValueConverterBase<VertexWorksheetReader.Visibility>
{
    //*************************************************************************
    //  Constructor: VertexVisibilityConverter()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="VertexVisibilityConverter" /> class.
    /// </summary>
    //*************************************************************************

    public VertexVisibilityConverter()
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

			new GraphValueInfo( VertexWorksheetReader.Visibility.ShowIfInAnEdge,
				new String [] {"Show if in an Edge (1)", "1",} ),

			new GraphValueInfo( VertexWorksheetReader.Visibility.Skip,
				new String [] {"Skip (0)", "0",} ),

			new GraphValueInfo( VertexWorksheetReader.Visibility.Hide,
				new String [] {"Hide (2)", "2",} ),

			new GraphValueInfo( VertexWorksheetReader.Visibility.Show,
				new String [] {"Show (4)", "4",} ),
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
