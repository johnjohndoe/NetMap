
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Xml;
using System.Diagnostics;
using Microsoft.NodeXL.GraphDataProviders.Twitter;
using Microsoft.SocialNetworkLib;
using Microsoft.Research.CommunityTechnologies.XmlLib;

namespace Microsoft.NodeXL.NetworkServer
{
//*****************************************************************************
//  Class: NetworkConfigurationFileParser
//
/// <summary>
/// Parses a network configuration file.
/// </summary>
///
/// <remarks>
/// A network configuration file specifies which network to get and where to
/// save it on disk.  It's in XML format.
///
/// <para>
/// Call <see cref="OpenNetworkConfigurationFile" /> to open the file.  Call
/// <see cref="GetNetworkType" /> to get the type of network, then call either
/// <see cref="GetTwitterSearchNetworkConfiguration" /> or <see
/// cref="GetTwitterUserNetworkConfiguration" /> to get the configuration
/// details for the specified network type.
/// </para>
///
/// <para>
/// All of the methods throw an XmlException when they detect invalid
/// configuration information.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class NetworkConfigurationFileParser : Object
{
    //*************************************************************************
    //  Constructor: NetworkConfigurationFileParser()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="NetworkConfigurationFileParser" /> class.
    /// </summary>
    //*************************************************************************

    public NetworkConfigurationFileParser()
    {
        m_oNetworkConfigurationXmlDocument = null;

        AssertValid();
    }

    //*************************************************************************
    //  Method: OpenNetworkConfigurationFile()
    //
    /// <summary>
    /// Opens the network configuration file.
    /// </summary>
    ///
    /// <param name="filePath">
    /// Full path to the network configuration file.
    /// </param>
    ///
    /// <remarks>
    /// This method must be called before any other methods are called.
    /// </remarks>
    //*************************************************************************

    public void
    OpenNetworkConfigurationFile
    (
        String filePath
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(filePath) );
        AssertValid();

        m_oNetworkConfigurationXmlDocument = new XmlDocument();

        const String NotFoundMessage =
            "The network configuration file couldn't be found.";

        try
        {
            using ( StreamReader oStreamReader = new StreamReader(filePath) )
            {
                m_oNetworkConfigurationXmlDocument.Load(oStreamReader);
            }
        }
        catch (DirectoryNotFoundException oDirectoryNotFoundException)
        {
            throw new XmlException(NotFoundMessage,
                oDirectoryNotFoundException);
        }
        catch (FileNotFoundException oFileNotFoundException)
        {
            throw new XmlException(NotFoundMessage, oFileNotFoundException);
        }
        catch (IOException oIOException)
        {
            throw new XmlException(
                "The network configuration file couldn't be opened.",
                oIOException);
        }
        catch (UnauthorizedAccessException oUnauthorizedAccessException)
        {
            throw new XmlException(
                "The network configuration file couldn't be opened due to a"
                + " security restriction.",

                oUnauthorizedAccessException);
        }
        catch (XmlException oXmlException)
        {
            throw new XmlException(
                "The network configuration file does not contain valid XML.",
                oXmlException);
        }
    }

    //*************************************************************************
    //  Method: GetNetworkType()
    //
    /// <summary>
    /// Gets the type of network to get.
    /// </summary>
    ///
    /// <returns>
    /// The type of network to get, as a <see cref="NetworkType" />.
    /// </returns>
    //*************************************************************************

    public NetworkType
    GetNetworkType()
    {
        AssertValid();
        Debug.Assert(m_oNetworkConfigurationXmlDocument != null);

        return ( GetRequiredEnumValue<NetworkType>(
            m_oNetworkConfigurationXmlDocument,
            "/NetworkConfiguration/NetworkType/text()",
            "NetworkType"
            ) );
    }

    //*************************************************************************
    //  Method: GetTwitterSearchNetworkConfiguration()
    //
    /// <summary>
    /// Gets the configuration details for a Twitter search network.
    /// </summary>
    ///
    /// <param name="searchTerm">
    /// Where the term to search for gets stored.
    /// </param>
    ///
    /// <param name="whatToInclude">
    /// Where the specification for what should be included in the network gets
    /// stored.
    /// </param>
    ///
    /// <param name="maximumPeoplePerRequest">
    /// Where the maximum number of people to request for each query gets
    /// stored.  Can be Int32.MaxValue for no limit.
    /// </param>
    ///
    /// <param name="credentialsScreenName">
    /// Where the screen name of the Twitter user whose credentials should be
    /// used gets stored.  Can be null to not use credentials.
    /// </param>
    ///
    /// <param name="credentialsPassword">
    /// Where the password of the Twitter user whose credentials should be used
    /// gets stored.  Can be null to not use credentials.
    /// </param>
    ///
    /// <param name="networkFileFolderPath">
    /// Where the full path to the folder where the network files should be
    /// written gets stored.
    /// </param>
    ///
    /// <param name="eNetworkFileFormats">
    /// Where the file formats to save the network to get stored.
    /// </param>
    //*************************************************************************

    public void
    GetTwitterSearchNetworkConfiguration
    (
        out String searchTerm,
        out TwitterSearchNetworkAnalyzer.WhatToInclude whatToInclude,
        out Int32 maximumPeoplePerRequest,
        out String credentialsScreenName,
        out String credentialsPassword,
        out String networkFileFolderPath,
        out NetworkFileFormats eNetworkFileFormats
    )
    {
        AssertValid();
        Debug.Assert(m_oNetworkConfigurationXmlDocument != null);
        Debug.Assert(GetNetworkType() == NetworkType.TwitterSearch);

        XmlNode oTwitterSearchNetworkConfigurationNode =
            XmlUtil2.SelectRequiredSingleNode(
                m_oNetworkConfigurationXmlDocument,
                "/NetworkConfiguration/TwitterSearchNetworkConfiguration",
                null);

        searchTerm = XmlUtil2.SelectRequiredSingleNodeAsString(
            oTwitterSearchNetworkConfigurationNode, "SearchTerm/text()", null);

        whatToInclude =
            GetRequiredEnumValue<TwitterSearchNetworkAnalyzer.WhatToInclude>(
                oTwitterSearchNetworkConfigurationNode, "WhatToInclude/text()",
                "WhatToInclude");

        GetTwitterCommonConfiguration(oTwitterSearchNetworkConfigurationNode,
            out maximumPeoplePerRequest, out credentialsScreenName,
            out credentialsPassword, out networkFileFolderPath,
            out eNetworkFileFormats);
    }

    //*************************************************************************
    //  Method: GetTwitterUserNetworkConfiguration()
    //
    /// <summary>
    /// Gets the configuration details for a Twitter user network.
    /// </summary>
    ///
    /// <param name="screenNameToAnalyze">
    /// Where the screen name of the Twitter user whose network should be
    /// analyzed gets stored.
    /// </param>
    ///
    /// <param name="whatToInclude">
    /// Where the specification for what should be included in the network gets
    /// stored.
    /// </param>
    ///
    /// <param name="networkLevel">
    /// Where the network level to include gets stored.
    /// </param>
    ///
    /// <param name="maximumPeoplePerRequest">
    /// Where the maximum number of people to request for each query gets
    /// stored.  Can be Int32.MaxValue for no limit.
    /// </param>
    ///
    /// <param name="credentialsScreenName">
    /// Where the screen name of the Twitter user whose credentials should be
    /// used gets stored.  Can be null to not use credentials.
    /// </param>
    ///
    /// <param name="credentialsPassword">
    /// Where the password of the Twitter user whose credentials should be used
    /// gets stored.  Can be null to not use credentials.
    /// </param>
    ///
    /// <param name="networkFileFolderPath">
    /// Where the full path to the folder where the network files should be
    /// written gets stored.
    /// </param>
    ///
    /// <param name="eNetworkFileFormats">
    /// Where the file formats to save the network to get stored.
    /// </param>
    //*************************************************************************

    public void
    GetTwitterUserNetworkConfiguration
    (
        out String screenNameToAnalyze,
        out TwitterUserNetworkAnalyzer.WhatToInclude whatToInclude,
        out NetworkLevel networkLevel,
        out Int32 maximumPeoplePerRequest,
        out String credentialsScreenName,
        out String credentialsPassword,
        out String networkFileFolderPath,
        out NetworkFileFormats eNetworkFileFormats
    )
    {
        AssertValid();
        Debug.Assert(m_oNetworkConfigurationXmlDocument != null);
        Debug.Assert(GetNetworkType() == NetworkType.TwitterUser);

        credentialsScreenName = null;
        credentialsPassword = null;
        networkFileFolderPath = null;

        XmlNode oTwitterUserNetworkConfigurationNode =
            XmlUtil2.SelectRequiredSingleNode(
                m_oNetworkConfigurationXmlDocument,
                "/NetworkConfiguration/TwitterUserNetworkConfiguration",
                null);

        screenNameToAnalyze = XmlUtil2.SelectRequiredSingleNodeAsString(
            oTwitterUserNetworkConfigurationNode,
            "ScreenNameToAnalyze/text()", null);

        whatToInclude =
            GetRequiredEnumValue<TwitterUserNetworkAnalyzer.WhatToInclude>(
                oTwitterUserNetworkConfigurationNode, "WhatToInclude/text()",
                "WhatToInclude");

        networkLevel = GetRequiredEnumValue<NetworkLevel>(
            oTwitterUserNetworkConfigurationNode, "NetworkLevel/text()",
            "NetworkLevel");

        GetTwitterCommonConfiguration(oTwitterUserNetworkConfigurationNode,
            out maximumPeoplePerRequest, out credentialsScreenName,
            out credentialsPassword, out networkFileFolderPath,
            out eNetworkFileFormats);
    }

    //*************************************************************************
    //  Method: GetTwitterCommonConfiguration()
    //
    /// <summary>
    /// Gets the configuration details common to all Twitter networks.
    /// </summary>
    ///
    /// <param name="oParentNode">
    /// Node containing the common configuration details.
    /// </param>
    ///
    /// <param name="iMaximumPeoplePerRequest">
    /// Where the maximum number of people to request for each query gets
    /// stored.  Can be Int32.MaxValue for no limit.
    /// </param>
    ///
    /// <param name="sCredentialsScreenName">
    /// Where the screen name of the Twitter user whose credentials should be
    /// used gets stored.  Can be null to not use credentials.
    /// </param>
    ///
    /// <param name="sCredentialsPassword">
    /// Where the password of the Twitter user whose credentials should be used
    /// gets stored.  Can be null to not use credentials.
    /// </param>
    ///
    /// <param name="sNetworkFileFolderPath">
    /// Where the full path to the folder where the network files should be
    /// written gets stored.
    /// </param>
    ///
    /// <param name="eNetworkFileFormats">
    /// Where the file formats to save the network to get stored.
    /// </param>
    //*************************************************************************

    protected void
    GetTwitterCommonConfiguration
    (
        XmlNode oParentNode,
        out Int32 iMaximumPeoplePerRequest,
        out String sCredentialsScreenName,
        out String sCredentialsPassword,
        out String sNetworkFileFolderPath,
        out NetworkFileFormats eNetworkFileFormats
    )
    {
        Debug.Assert(oParentNode != null);
        AssertValid();

        String sMaximumPeoplePerRequest;
        iMaximumPeoplePerRequest = Int32.MaxValue;
        eNetworkFileFormats = NetworkFileFormats.None;

        if ( XmlUtil2.TrySelectSingleNodeAsString(oParentNode,
            "MaximumPeoplePerRequest/text()", null,
            out sMaximumPeoplePerRequest) )
        {
            if ( !Int32.TryParse(sMaximumPeoplePerRequest,
                out iMaximumPeoplePerRequest) )
            {
                throw new XmlException(
                    "The MaximumPeoplePerRequest value is not valid."
                    );
            }
        }

        if ( !XmlUtil2.TrySelectSingleNodeAsString(oParentNode,
            "CredentialsScreenName/text()", null, out sCredentialsScreenName) )
        {
            sCredentialsScreenName = null;
        }

        if ( !XmlUtil2.TrySelectSingleNodeAsString(oParentNode,
            "CredentialsPassword/text()", null, out sCredentialsPassword) )
        {
            sCredentialsPassword = null;
        }

        if ( (sCredentialsScreenName == null) !=
            (sCredentialsPassword == null) )
        {
            throw new XmlException(
                "If you specify CredentialsScreenName or CredentialsPassword,"
                + " you must specify both."
                );
        }

        sNetworkFileFolderPath = XmlUtil2.SelectRequiredSingleNodeAsString(
            oParentNode, "NetworkFileFolder/text()", null);

        eNetworkFileFormats = GetRequiredEnumValue<NetworkFileFormats>(
            oParentNode, "NetworkFileFormats/text()", "NetworkFileFormats");
    }

    //*************************************************************************
    //  Method: GetRequiredEnumValue()
    //
    /// <summary>
    /// Gets a required Enum value from the text of a specified node.
    /// </summary>
    ///
    /// <param name="oNode">
    /// Node to select from.
    /// </param>
    ///
    /// <param name="sXPath">
    /// XPath expression.
    /// </param>
    ///
    /// <param name="sTagName">
    /// Name of the tag containing the Enum value.  Used in error messages.
    /// </param>
    ///
    /// <returns>
    /// The specified Enum value.
    /// </returns>
    //*************************************************************************

    protected T
    GetRequiredEnumValue<T>
    (
        XmlNode oNode,
        String sXPath,
        String sTagName
    )
    {
        Debug.Assert(oNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sXPath) );
        Debug.Assert( !String.IsNullOrEmpty(sTagName) );
        AssertValid();

        Exception oException;

        try
        {
            String sText = XmlUtil2.SelectRequiredSingleNodeAsString(oNode,
                sXPath, null);

            return ( (T)Enum.Parse(typeof(T), sText) );
        }
        catch (XmlException oXmlException)
        {
            oException = oXmlException;
        }
        catch (ArgumentException oArgumentException)
        {
            oException = oArgumentException;
        }

        String sErrorMessage = String.Format(
            "The {0} value is missing or invalid."
            ,
            sTagName
            );

        throw new XmlException(sErrorMessage, oException);
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
        // m_oNetworkConfigurationXmlDocument
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The opened network configuration file, or null if
    /// OpenNetworkConfigurationFile() hasn't been called.

    protected XmlDocument m_oNetworkConfigurationXmlDocument;
}

}
