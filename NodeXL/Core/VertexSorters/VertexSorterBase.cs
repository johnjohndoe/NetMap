
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
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

    public ICollection<IVertex>
    Sort
    (
        ICollection<IVertex> vertices
    )
    {
        AssertValid();

        const String MethodName = "Sort";

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, "vertices", vertices);

        IVertex [] aoCopy = new IVertex[vertices.Count];
        vertices.CopyTo(aoCopy, 0);

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
