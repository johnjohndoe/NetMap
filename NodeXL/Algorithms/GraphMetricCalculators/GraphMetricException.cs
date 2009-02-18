
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Microsoft.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: GraphMetricException
//
/// <summary>
/// Represents an exception thrown when a graph metric calculator detects a
/// condition that prevents its graph metrics from being calculated.
/// </summary>
//*****************************************************************************

[System.SerializableAttribute()]

public class GraphMetricException : Exception
{
    //*************************************************************************
    //  Constructor: GraphMetricException()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMetricException" />
    /// class.
    /// </summary>
    ///
    /// <param name="message">
    /// Error message, suitable for displaying to the user.
    /// </param>
    //*************************************************************************

    public GraphMetricException
    (
        String message
    )
    : base(message)
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
