
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Microsoft.NodeXL.GraphDataProviders
{
//*****************************************************************************
//  Class: CancellationPendingException
//
/// <summary>
/// Represents an exception thrown when a pending cancellation has been
/// detected while an asynchronous network analysis is in progress.
/// </summary>
///
/// <remarks>
/// This exception gets thrown by <see
/// cref="HttpNetworkAnalyzerBase.CheckCancellationPending" /> when it detects
/// a pending cancellation.  The asynchronous method analyzing the network
/// should catch this exception, set the DoWorkEventArgs.Cancel property to
/// true, and then return.
/// </remarks>
//*****************************************************************************

[System.SerializableAttribute()]

public class CancellationPendingException : Exception
{
    //*************************************************************************
    //  Constructor: CancellationPendingException()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="CancellationPendingException" /> class.
    /// </summary>
    //*************************************************************************

    public CancellationPendingException()
    {
        // (Do nothing.)

        AssertValid();
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
