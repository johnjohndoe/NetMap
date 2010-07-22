
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Net;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Microsoft.NodeXL.GraphDataProviders.Flickr
{
//*****************************************************************************
//  Class: FlickrException
//
/// <summary>
/// Represents an error message received from Flickr.
/// </summary>
///
/// <remarks>
/// A <see cref="FlickrException" /> is thrown when a valid XML response is
/// received from Flickr and the XML indicates that an error has occurred.
/// This differs from a WebException, which is thrown when an HTTP error occurs
/// and there is no XML response.
/// </remarks>
//*****************************************************************************

[System.SerializableAttribute()]

public class FlickrException : WebException
{
    //*************************************************************************
    //  Constructor: FlickrException()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="FlickrException" /> class.
    /// </summary>
    ///
    /// <param name="flickrErrorMessage">
    /// Error message received from Flickr.
    /// </param>
    //*************************************************************************

    public FlickrException
    (
        String flickrErrorMessage
    )
    : base(flickrErrorMessage)
    {
        // (Do nothing else.)

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
