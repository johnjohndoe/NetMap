
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Xml;
using System.Net;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.XmlLib;

namespace Microsoft.NodeXL.GraphDataProviders.Flickr
{
//*****************************************************************************
//  Class: FlickrNetworkAnalyzerBase
//
/// <summary>
/// Base class for classes that analyze a Flickr network.
/// </summary>
//*****************************************************************************

public abstract class FlickrNetworkAnalyzerBase : HttpNetworkAnalyzerBase
{
    //*************************************************************************
    //  Constructor: FlickrNetworkAnalyzerBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="FlickrNetworkAnalyzerBase" /> class.
    /// </summary>
    //*************************************************************************

    public FlickrNetworkAnalyzerBase()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: GetXmlDocument()
    //
    /// <overloads>
    /// Gets an XML document from an URL.
    /// </overloads>
    ///
    /// <summary>
    /// Gets an XML document from an URL with a specified number of retries.
    /// </summary>
    ///
    /// <param name="sUrl">
    /// The URL to get the document from.
    /// </param>
    ///
    /// <param name="iRetries">
    /// The maximum number of retries.
    /// </param>
    ///
    /// <returns>
    /// The XmlDocument from the URL.
    /// </returns>
    //*************************************************************************

    protected XmlDocument
    GetXmlDocument
    (
        String sUrl,
        Int32 iRetries
    )
    {
        Debug.Assert(iRetries >= 0);
        AssertValid();

        while (true)
        {
            try
            {
                return ( GetXmlDocument(sUrl) );
            }
            catch (Exception oException)
            {
                iRetries--;

                if (iRetries < 0)
                {
                    throw (oException);
                }
            }
        }
    }

    //*************************************************************************
    //  Method: GetXmlDocument()
    //
    /// <summary>
    /// Gets an XML document from an URL with no retries.
    /// </summary>
    ///
    /// <param name="sUrl">
    /// The URL to get the document from.
    /// </param>
    ///
    /// <returns>
    /// The XmlDocument from the URL.
    /// </returns>
    //*************************************************************************

    protected XmlDocument
    GetXmlDocument
    (
        String sUrl
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUrl) );
        AssertValid();

        XmlDocument oXmlDocument = GetXmlDocument(
            (HttpWebRequest)WebRequest.Create(sUrl) );

        XmlNode oRspNode = oXmlDocument.DocumentElement;

        String sStatus;

        XmlUtil.GetAttribute(oRspNode, "stat", true, out sStatus);

        if (sStatus != "ok")
        {
            XmlNode oErrNode = XmlUtil.SelectRequiredSingleNode(oRspNode,
                "err");

            String sErrorMessage;

            XmlUtil.GetAttribute(oErrNode, "msg", true, out sErrorMessage);
            throw new WebException(sErrorMessage);
        }

        return (oXmlDocument);
    }


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
