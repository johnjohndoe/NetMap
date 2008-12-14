
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NodeXL.Visualization;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexDrawerPrecedenceConverter
//
/// <summary>
/// Class that converts a vertex drawer precedence between values used in the
/// Excel workbook and values used in the NodeXL graph.
/// </summary>
//*****************************************************************************

public class VertexDrawerPrecedenceConverter :
	TextValueConverterBase<VertexDrawerPrecedence>
{
    //*************************************************************************
    //  Constructor: VertexDrawerPrecedenceConverter()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="VertexDrawerPrecedenceConverter" /> class.
    /// </summary>
    //*************************************************************************

    public VertexDrawerPrecedenceConverter()
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

			new GraphValueInfo( VertexDrawerPrecedence.Shape,
				new String [] {"Shape (1)", "1",} ),

			new GraphValueInfo( VertexDrawerPrecedence.Image,
				new String [] {"Image (2)", "2",} ),

			new GraphValueInfo( VertexDrawerPrecedence.PrimaryLabel,
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
