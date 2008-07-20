
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NetMap.Core
{
//*****************************************************************************
//  Class: VertexFactoryBase
//
/// <summary>
///	Base class for vertex factories.
/// </summary>
///
/// <remarks>
/// This abstract class can serve as a base class for <see
/// cref="IVertexFactory" /> implementations.  Its implementations of the <see
/// cref="IVertexFactory" /> public methods provide error checking but defer
/// the actual work to protected abstract methods.
/// </remarks>
///
/// <seealso cref="Vertex" />
//*****************************************************************************

public abstract class VertexFactoryBase :
	GraphVertexEdgeFactoryBase, IVertexFactory
{
    //*************************************************************************
    //  Constructor: VertexFactoryBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexFactoryBase" />
	/// class.
    /// </summary>
    //*************************************************************************

    public VertexFactoryBase()
    {
		// (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Method: CreateVertex()
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

	public IVertex
	CreateVertex()
	{
		AssertValid();

		return ( CreateVertexCore() );
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

	protected abstract IVertex
	CreateVertexCore();


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
