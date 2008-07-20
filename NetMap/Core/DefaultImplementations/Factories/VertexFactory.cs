
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NetMap.Core
{
//*****************************************************************************
//  Class: VertexFactory
//
/// <summary>
/// Class that knows how to create <see cref="Vertex" /> objects.
/// </summary>
///
/// <remarks>
///	This class implements <see cref="IVertexFactory" />, which allows the core
/// NetMap system to create vertex objects without knowing their type.
/// </remarks>
///
/// <seealso cref="Vertex" />
//*****************************************************************************

public class VertexFactory : VertexFactoryBase, IVertexFactory
{
    //*************************************************************************
    //  Constructor: VertexFactory()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexFactory" /> class.
    /// </summary>
    //*************************************************************************

    public VertexFactory()
    {
		// (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Method: CreateVertexCore()
    //
    /// <summary>
    /// Creates a <see cref="Vertex" /> object.
    /// </summary>
    ///
    /// <returns>
	///	The <see cref="IVertex" /> interface on a newly created <see
	///	cref="Vertex" /> object.
    /// </returns>
    //*************************************************************************

	protected override IVertex
	CreateVertexCore()
	{
		AssertValid();

		return ( new Vertex() );
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
