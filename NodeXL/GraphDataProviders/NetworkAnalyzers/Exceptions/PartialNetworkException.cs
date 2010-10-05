
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Xml;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Microsoft.NodeXL.GraphDataProviders
{
//*****************************************************************************
//  Class: PartialNetworkException
//
/// <summary>
/// Represents an exception thrown when only part of a requested network is
/// obtained.
/// </summary>
///
/// <remarks>
/// Network analyzer classes may throw an exception of this type when only part
/// of a requested network was obtained due to the occurrence of one or more
/// errors.  The caller may choose to use the partial network, which is
/// available via the <see cref="PartialNetwork" /> property.  Information
/// about the requests that were made is available via the <see
/// cref="PartialNetworkException.RequestStatistics" /> property.
/// </remarks>
//*****************************************************************************

[System.SerializableAttribute()]

public class PartialNetworkException : Exception
{
    //*************************************************************************
    //  Constructor: PartialNetworkException()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="PartialNetworkException" /> class.
    /// </summary>
    ///
    /// <param name="partialNetwork">
    /// The partial network that was obtained.
    /// </param>
    ///
    /// <param name="requestStatistics">
    /// Information about the requests that were made while getting the
    /// network.
    /// </param>
    //*************************************************************************

    public PartialNetworkException
    (
        XmlDocument partialNetwork,
        RequestStatistics requestStatistics
    )
    {
        m_oPartialNetwork = partialNetwork;
        m_oRequestStatistics = requestStatistics;

        AssertValid();
    }

    //*************************************************************************
    //  Property: PartialNetwork
    //
    /// <summary>
    /// The partial network that was obtained.
    /// </summary>
    ///
    /// <value>
    /// The partial network, as an XmlDocument containing the partial network
    /// as GraphML.
    /// </value>
    //*************************************************************************

    public XmlDocument
    PartialNetwork
    {
        get
        {
            AssertValid();

            return (m_oPartialNetwork);
        }
    }

    //*************************************************************************
    //  Property: RequestStatistics
    //
    /// <summary>
    /// Gets information about the requests that were made while getting the
    /// network.
    /// </summary>
    ///
    /// <value>
    /// Information about the requests that were made while getting the
    /// network.
    /// </value>
    //*************************************************************************

    public RequestStatistics
    RequestStatistics
    {
        get
        {
            AssertValid();

            return (m_oRequestStatistics);
        }
    }

    //*************************************************************************
    //  Method: ToMessage()
    //
    /// <summary>
    /// Converts the exception to a message suitable for use in a user
    /// interface.
    /// </summary>
    ///
    /// <param name="lastUnexpectedExceptionMessage">
    /// The most recent unexpected exception (after retries) that occurred
    /// while getting the network, converted to a message by the caller.  The
    /// most recent unexpected exception can be obtained from <see
    /// cref="RequestStatistics" />.LastUnexpectedException.
    /// </param>
    //*************************************************************************

    public String
    ToMessage
    (
        String lastUnexpectedExceptionMessage
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(lastUnexpectedExceptionMessage) );
        AssertValid();

        Int32 iUnexpectedExceptions = 
            m_oRequestStatistics.UnexpectedExceptions;

        const String Int32Format = "N0";

        return ( String.Format(

            "Getting a network can involve many information requests to a Web"
            + " service.  In this case, {0} requests were made and {1} of them"
            + " {2} unsuccessful."
            + "\r\n\r\n"
            + "(Note that unsuccessful requests might have led to additional"
            + " requests if they had succeeded, so it is not possible to"
            + " calculate the percentage of the complete network that was"
            + " actually obtained.)"
            + "\r\n\r\n"
            + "Here are details for the most recent unsuccessful request:"
            + "\r\n\r\n"
            + "{3}"
            ,
            (m_oRequestStatistics.SuccessfulRequests +
                iUnexpectedExceptions).ToString(Int32Format),

            iUnexpectedExceptions.ToString(Int32Format),
            (iUnexpectedExceptions == 1) ? "was" : "were",
            lastUnexpectedExceptionMessage
            ) );
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
        Debug.Assert(m_oPartialNetwork != null);
        Debug.Assert(m_oRequestStatistics != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The partial network that was obtained.

    protected XmlDocument m_oPartialNetwork;

    /// Information about the requests that were made while getting the
    /// network.

    protected RequestStatistics m_oRequestStatistics;
}
}
