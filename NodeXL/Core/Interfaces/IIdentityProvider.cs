
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Interface: IIdentityProvider
//
/// <summary>
/// Provides object identity.
/// </summary>
///
/// <remarks>
/// Classes that provide information for identifying an instance should
/// implement this interface.
/// </remarks>
//*****************************************************************************

public interface IIdentityProvider
{
    //*************************************************************************
    //  Property: Name
    //
    /// <summary>
    /// Gets the object's name.
    /// </summary>
    ///
    /// <value>
    /// The object's name, as a String.  Can be null.  The default value is
    /// null.
    /// </value>
    ///
    /// <remarks>
    /// The name is optional and is set by the application.
    ///
    /// <para>
    /// Names are case sensitive.  The names "Mary" and "mary" are different,
    /// for example.
    /// </para>
    ///
    /// <para>
    /// The core NodeXL system does not enforce uniqueness of names.  It is
    /// possible to have two items with the same name in the same collection.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="ID" />
    //*************************************************************************

    String
    Name
    {
        get;
        set;
    }

    //*************************************************************************
    //  Property: ID
    //
    /// <summary>
    /// Gets the object's ID.
    /// </summary>
    ///
    /// <value>
    /// The object's ID, as an Int32.
    /// </value>
    ///
    /// <remarks>
    /// The ID is set when the object is created.  It must be unique among all
    /// objects of the same category for the duration of the process in which
    /// NodeXL is running.  ("Category" means graph, vertex, or edge.)  No two
    /// vertices can have the same ID, for example, although a vertex and edge
    /// can have the same ID.
    ///
    /// <para>
    /// If an object is cloned, the copy must have an ID different from that of
    /// the original.
    /// </para>
    ///
    /// <para>
    /// If the object is saved to persistent storage and then loaded from that
    /// storage, the loaded object must have an ID different from that of the
    /// original.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="Name" />
    //*************************************************************************

    Int32
    ID
    {
        get;
    }
}

}
