
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Interface: IVertexFactory
//
/// <summary>
/// Represents an object that knows how to create a vertex.
/// </summary>
///
/// <remarks>
/// If you implement a custom vertex class from scratch, you may also want to
/// implement <see cref="IVertexFactory" />.  Several methods in the NodeXL
/// system create a vertex and allow the type of the new vertex to be specified
/// via an object that implements <see cref="IVertexFactory" />.
/// </remarks>
///
/// <seealso cref="IVertex" />
//*****************************************************************************

public interface IVertexFactory
{
    //*************************************************************************
    //  Method: CreateVertex()
    //
    /// <summary>
    /// Creates a vertex object.
    /// </summary>
    ///
    /// <returns>
    /// The <see cref="IVertex" /> interface on a newly created vertex object.
    /// </returns>
    //*************************************************************************

    IVertex
    CreateVertex();
}

}
