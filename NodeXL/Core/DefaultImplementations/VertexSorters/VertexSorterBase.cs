
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Diagnostics;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Class: VertexSorterBase
//
/// <summary>
/// Base class for vertex sorters.
/// </summary>
///
/// <remarks>
/// This abstract class can serve as a base class for <see
/// cref="IVertexSorter" /> implementations.  Its implementations of the <see
/// cref="IVertexSorter" /> public methods provide error checking but defer the
/// actual work to protected abstract methods.
/// </remarks>
///
/// <seealso cref="Edge" />
//*****************************************************************************

public abstract class VertexSorterBase : NodeXLBase, IVertexSorter
{
    //*************************************************************************
    //  Constructor: VertexSorterBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexSorterBase" />
    /// class.
    /// </summary>
    //*************************************************************************

    public VertexSorterBase()
    {
        // (Do nothing.)

        // AssertValid();
    }

    //*************************************************************************
    //  Method: Sort()
    //
    /// <overloads>
    /// Sorts a collection of vertices.
    /// </overloads>
    ///
    /// <summary>
    /// Sorts an <see cref="IVertexCollection" />.
    /// </summary>
    ///
    /// <param name="vertexCollection">
    /// Collection to sort.  The collection is not modified.
    /// </param>
    ///
    /// <returns>
    /// A sorted <see cref="IVertex" /> array.
    /// </returns>
    ///
    /// <remarks>
    /// This method creates an array of references to the vertices in <paramref
    /// name="vertexCollection" />, sorts the array, and returns the array.
    /// <paramref name="vertexCollection" /> is not modified and no vertices
    /// are cloned or created.
    /// </remarks>
    //*************************************************************************

    public IVertex []
    Sort
    (
        IVertexCollection vertexCollection
    )
    {
        AssertValid();

        const String MethodName = "Sort";

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, "vertexCollection", vertexCollection);

        return ( ( IVertex [] )Sort( (ICollection)vertexCollection ) );
    }

    //*************************************************************************
    //  Method: Sort()
    //
    /// <summary>
    /// Sorts an array of vertices.
    /// </summary>
    ///
    /// <param name="vertices">
    /// Array to sort.  The array is not modified.
    /// </param>
    ///
    /// <returns>
    /// A sorted copy of <paramref name="vertices" />.
    /// </returns>
    ///
    /// <remarks>
    /// This method creates a copy of <paramref name="vertices" />, sorts the
    /// copy, and returns the copy.  <paramref name="vertices" /> is not
    /// modified and no vertices are cloned or created.
    /// </remarks>
    //*************************************************************************

    public IVertex []
    Sort
    (
        IVertex [] vertices
    )
    {
        AssertValid();

        return ( ( IVertex [] )Sort( (ICollection)vertices ) );
    }

    //*************************************************************************
    //  Method: Sort()
    //
    /// <summary>
    /// Sorts an ICollection of vertices.
    /// </summary>
    ///
    /// <param name="vertices">
    /// ICollection of vertices to sort.  The array is not modified.
    /// </param>
    ///
    /// <returns>
    /// A sorted copy of <paramref name="vertices" />, as an ICollection.
    /// </returns>
    ///
    /// <remarks>
    /// This method creates a copy of <paramref name="vertices" />, sorts the
    /// copy, and returns the copy.  <paramref name="vertices" /> is not
    /// modified and no vertices are cloned or created.
    /// </remarks>
    //*************************************************************************

    public ICollection
    Sort
    (
        ICollection vertices
    )
    {
        AssertValid();

        const String MethodName = "Sort";

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, "vertices", vertices);

        // Copy the array.

        IVertex [] aoCopy = new IVertex[vertices.Count];

        vertices.CopyTo(aoCopy, 0);

        // Sort the copy.

        return ( SortCore(aoCopy) );
    }

    //*************************************************************************
    //  Method: SortCore()
    //
    /// <summary>
    /// Sorts an array of vertices in place.
    /// </summary>
    ///
    /// <param name="vertices">
    /// Array to sort.  The array is sorted in place.
    /// </param>
    ///
    /// <returns>
    /// Sorted <paramref name="vertices" />.
    /// </returns>
    ///
    /// <remarks>
    /// This method sorts <paramref name="vertices" /> in place and returns the
    /// sorted vertices.
    ///
    /// <para>
    /// The arguments have already been checked for validity.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected abstract IVertex [] 
    SortCore
    (
        IVertex [] vertices
    );


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
