
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Diagnostics;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Interface: IVertexSorter
//
/// <summary>
/// Represents an object that can sort a collection of vertices.
/// </summary>
///
/// <remarks>
/// The <see cref="Sort(IVertexCollection)" /> methods sort the specified
/// vertex collection in a manner determined by the implementation.
/// </remarks>
//*****************************************************************************

public interface IVertexSorter
{
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

    IVertex []
    Sort
    (
        IVertexCollection vertexCollection
    );

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

    IVertex []
    Sort
    (
        IVertex [] vertices
    );

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

    ICollection
    Sort
    (
        ICollection vertices
    );
}

}
