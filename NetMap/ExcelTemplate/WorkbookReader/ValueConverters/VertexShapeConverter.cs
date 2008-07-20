
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NetMap.Visualization;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexShapeConverter
//
/// <summary>
/// Class that converts a vertex shape between values used in the Excel
/// workbook and values used in the NetMap graph.
/// </summary>
//*****************************************************************************

public class VertexShapeConverter :
	TextValueConverterBase<VertexDrawer.VertexShape>
{
    //*************************************************************************
    //  Constructor: VertexShapeConverter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexShapeConverter" />
	/// class.
    /// </summary>
    //*************************************************************************

    public VertexShapeConverter()
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

			new GraphValueInfo( VertexDrawer.VertexShape.Circle,
				new String [] {"Circle",} ),

			new GraphValueInfo( VertexDrawer.VertexShape.Disk,
				new String [] {"Disk",} ),

			new GraphValueInfo( VertexDrawer.VertexShape.Sphere,
				new String [] {"Sphere",} ),
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
