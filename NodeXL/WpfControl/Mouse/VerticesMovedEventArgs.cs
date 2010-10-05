
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: VerticesMovedEventArgs
//
/// <summary>
/// Provides information for the <see cref="NodeXLControl.VerticesMoved" />
/// event.
/// </summary>
//*****************************************************************************

public class VerticesMovedEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: VerticesMovedEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the VerticesMovedEventArgs class.
    /// </summary>
    ///
    /// <param name="movedVertices">
    /// Collection of one or more vertices that were moved.
    /// </param>
    //*************************************************************************

    public VerticesMovedEventArgs
    (
        ICollection<IVertex> movedVertices
    )
    {
        m_oMovedVertices = movedVertices;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: MovedVertices
    //
    /// <summary>
    /// Gets a collection of one or more vertices that were moved.
    /// </summary>
    ///
    /// <value>
    /// A collection of one or more vertices that were moved.
    /// </value>
    //*************************************************************************

    public ICollection<IVertex>
    MovedVertices
    {
        get
        {
            AssertValid();

            return (m_oMovedVertices);
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")] 

    public virtual void
    AssertValid()
    {
        Debug.Assert(m_oMovedVertices != null);
        Debug.Assert(m_oMovedVertices.Count > 0);
    }


    //*************************************************************************
    //  Protected member data
    //*************************************************************************

    /// Collection of one or more vertices that were moved.

    protected ICollection<IVertex> m_oMovedVertices;
}


//*****************************************************************************
//  Delegate: VerticesMovedEventHandler
//
/// <summary>
/// Represents a method that will handle the <see
/// cref="NodeXLControl.VerticesMoved" /> event.
/// </summary>
///
/// <param name="sender">
/// The object that fired the event.
/// </param>
///
/// <param name="e">
/// Provides information about the moved vertices.
/// </param>
//*****************************************************************************

public delegate void
VerticesMovedEventHandler
(
    Object sender,
    VerticesMovedEventArgs e
);

}
