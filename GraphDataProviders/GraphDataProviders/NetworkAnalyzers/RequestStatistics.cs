
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.GraphDataProviders
{
//*****************************************************************************
//  Class: RequestStatistics
//
/// <summary>
/// Keeps track of requests that have been made while getting the network.
/// </summary>
///
/// <remarks>
/// Call <see cref="OnSuccessfulRequest" /> each time a network request
/// succeeds.  Call <see cref="OnUnexpectedException" /> each time a network
/// request fails (after retries) with an unexpected exception.  After all
/// requests have been made, read the <see cref="SuccessfulRequests" />, <see
/// cref="UnexpectedExceptions" />, and <see cref="LastUnexpectedException" />
/// properties to create a request summary.
/// </remarks>
//*****************************************************************************

public class RequestStatistics : Object
{
    //*************************************************************************
    //  Constructor: RequestStatistics()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="RequestStatistics" />
    /// class.
    /// </summary>
    //*************************************************************************

    public RequestStatistics()
    {
        m_iSuccessfulRequests = 0;
        m_iUnexpectedExceptions = 0;
        m_oLastUnexpectedException = null;

        AssertValid();
    }

    //*************************************************************************
    //  Property: SuccessfulRequests
    //
    /// <summary>
    /// Gets the number of successful requests that were made while getting the
    /// network.
    /// </summary>
    ///
    /// <value>
    /// The number of successful requests.
    /// </value>
    //*************************************************************************

    public Int32
    SuccessfulRequests
    {
        get
        {
            AssertValid();

            return (m_iSuccessfulRequests);
        }
    }

    //*************************************************************************
    //  Property: UnexpectedExceptions
    //
    /// <summary>
    /// Gets the number of unexpected exceptions (after retries) that occurred
    /// while getting the network.
    /// </summary>
    ///
    /// <value>
    /// The number of unexpected exceptions.
    /// </value>
    //*************************************************************************

    public Int32
    UnexpectedExceptions
    {
        get
        {
            AssertValid();

            return (m_iUnexpectedExceptions);
        }
    }

    //*************************************************************************
    //  Property: LastUnexpectedException
    //
    /// <summary>
    /// Gets the most recent unexpected exception (after retries) that occurred
    /// while getting the network.
    /// </summary>
    ///
    /// <value>
    /// The most recent unexpected exception, or null if none have occurred.
    /// </value>
    //*************************************************************************

    public Exception
    LastUnexpectedException
    {
        get
        {
            AssertValid();

            return (m_oLastUnexpectedException);
        }
    }

    //*************************************************************************
    //  Method: OnSuccessfulRequest()
    //
    /// <summary>
    /// Increments the number of successful requests that have been made while
    /// getting the network.
    /// </summary>
    //*************************************************************************

    public void
    OnSuccessfulRequest()
    {
        AssertValid();

        m_iSuccessfulRequests++;
    }

    //*************************************************************************
    //  Method: OnUnexpectedException()
    //
    /// <summary>
    /// Increments the number of unexpected exceptions (after retries) that
    /// occurred while getting the network.
    /// </summary>
    //*************************************************************************

    public void
    OnUnexpectedException
    (
        Exception unexpectedException
    )
    {
        Debug.Assert(unexpectedException != null);
        AssertValid();

        m_oLastUnexpectedException = unexpectedException;
        m_iUnexpectedExceptions++;
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
        Debug.Assert(m_iSuccessfulRequests >= 0);
        Debug.Assert(m_iUnexpectedExceptions >= 0);
        // m_oLastUnexpectedException
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Number of successful requests that were made while getting the network.

    protected Int32 m_iSuccessfulRequests;

    /// Number of unexpected exceptions (after retries) that occurred while
    /// getting the network.

    protected Int32 m_iUnexpectedExceptions;

    /// The most recent unexpected exception (after retries) that occurred
    /// while getting the network, or null if none has occurred.

    protected Exception m_oLastUnexpectedException;
}

}
