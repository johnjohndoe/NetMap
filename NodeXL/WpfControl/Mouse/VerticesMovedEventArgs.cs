
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
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
    /// An array of one or more vertices that were moved.
    /// </param>
    //*************************************************************************

    public VerticesMovedEventArgs
    (
        IVertex [] movedVertices
    )
    {
        m_aoMovedVertices = movedVertices;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: MovedVertices
    //
    /// <summary>
    /// Gets an array of one or more vertices that were moved.
    /// </summary>
    ///
    /// <value>
    /// An array of one or more vertices that were moved.
    /// </value>
    //*************************************************************************

    public IVertex []
    MovedVertices
    {
        get
        {
            AssertValid();

            return (m_aoMovedVertices);
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
        Debug.Assert(m_aoMovedVertices != null);
        Debug.Assert(m_aoMovedVertices.Length > 0);
    }


    //*************************************************************************
    //  Protected member data
    //*************************************************************************

    /// Array of one or more vertices that were moved.

    protected IVertex [] m_aoMovedVertices;
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
