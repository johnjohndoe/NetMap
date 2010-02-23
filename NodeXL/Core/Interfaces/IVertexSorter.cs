
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
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
/// The <see cref="Sort" /> method sorts the specified vertex collection in a
/// manner determined by the implementation.
/// </remarks>
//*****************************************************************************

public interface IVertexSorter
{
    //*************************************************************************
    //  Method: Sort()
    //
    /// <summary>
    /// Sorts a collection of vertices.
    /// </summary>
    ///
    /// <param name="vertices">
    /// Collection to sort.  The collection is not modified.
    /// </param>
    ///
    /// <returns>
    /// A new, sorted collection.
    /// </returns>
    ///
    /// <remarks>
    /// This method creates a new collection of references to the vertices in
    /// <paramref name="vertices" />, sorts the new collection, and returns the
    /// new collection.  The original <paramref name="vertices" /> collection
    /// is not modified and no vertices are cloned or created.
    /// </remarks>
    //*************************************************************************

    ICollection<IVertex>
    Sort
    (
        ICollection<IVertex> vertices
    );
}

}
