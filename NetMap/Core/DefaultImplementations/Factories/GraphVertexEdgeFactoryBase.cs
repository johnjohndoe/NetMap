
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NetMap.Core
{
//*****************************************************************************
//  Class: GraphVertexEdgeFactoryBase
//
/// <summary>
///	Base class for the <see cref="GraphFactory" />, <see
/// cref="VertexFactory" />, and <see cref="EdgeFactory" /> classes.
/// </summary>
//*****************************************************************************

public class GraphVertexEdgeFactoryBase : NetMapBase
{
    //*************************************************************************
    //  Constructor: GraphVertexEdgeFactoryBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="GraphVertexEdgeFactoryBase" /> class.
    /// </summary>
    //*************************************************************************

    public GraphVertexEdgeFactoryBase()
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
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
