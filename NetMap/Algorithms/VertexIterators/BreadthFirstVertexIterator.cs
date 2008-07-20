
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Algorithms
{
//*****************************************************************************
//  Class: BreadthFirstVertexIterator
//
/// <summary>
///	Iterates through a collection of vertices using a breadth-first algorithm.
/// </summary>
//*****************************************************************************

public class BreadthFirstVertexIterator : NetMapBase, IVertexIterator
{
    //*************************************************************************
    //  Constructor: BreadthFirstVertexIterator()
    //
    /// <summary>
    /// Initializes a new instance of the BreadthFirstVertexIterator class.
    /// </summary>
    //*************************************************************************

    public BreadthFirstVertexIterator()
    {
		// TODO

		throw new NotImplementedException();
    }

    //*************************************************************************
    //  Method: Iterate()
    //
    /// <summary>
	///	Iterates through a collection of vertices.
    /// </summary>
    ///
    /// <param name="vertices">
    /// Vertex collection, as an <see cref="IVertexCollection" />.
    /// </param>
    ///
    /// <param name="vertexDelegate">
	///	Delegate to call for each vertex in <paramref name="vertices" />, as a
	/// <see cref="VertexDelegate" />.
    /// </param>
	///
	/// <remarks>
	/// This method iterates through <paramref name="vertices" />, calling
	///	<paramref name="vertexDelegate" /> for each vertex.  The value returned
	///	by <paramref name="vertexDelegate" /> is ignored.
	/// </remarks>
    //*************************************************************************

    public void
    Iterate
    (
		IVertexCollection vertices,
		VertexDelegate vertexDelegate
    )
	{
		AssertValid();

		// TODO

		throw new NotImplementedException();
	}

    //*************************************************************************
    //  Method: FindVertex()
    //
    /// <summary>
	///	Searches for a vertex in a collection of vertices.
    /// </summary>
    ///
    /// <param name="vertices">
    /// Vertex collection, as an <see cref="IVertexCollection" />.
    /// </param>
    ///
    /// <param name="vertexDelegate">
	///	Delegate to call for each vertex in <paramref name="vertices" />, as a
	/// <see cref="VertexDelegate" />.
    /// </param>
	///
    /// <param name="foundVertex">
	///	Where the found vertex gets stored if true is returned, as an <see
	///	cref="IVertex" />.
    /// </param>
	///
	/// <returns>
	///	true if the vertex was found, false if not.
	/// </returns>
	///
	/// <remarks>
	/// This method iterates through <paramref name="vertices" />, calling
	///	<paramref name="vertexDelegate" /> for each vertex.  If <paramref
	///	name="vertexDelegate" /> returns true for a vertex, this method stops
	///	iterating, stores that vertex at <paramref name="foundVertex" />, and
	///	returns true.  If the <paramref name="vertices" /> collection is empty
	///	or <paramref name="vertexDelegate" /> never returns true, this method
	///	returns false.
	/// </remarks>
    //*************************************************************************

    public Boolean
    FindVertex
    (
		IVertexCollection vertices,
		VertexDelegate vertexDelegate,
		out IVertex foundVertex
    )
	{
		AssertValid();

		// TODO

		throw new NotImplementedException();
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

        // TODO
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // TODO
}

}
