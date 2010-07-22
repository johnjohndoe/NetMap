
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.NetworkServer;
using Microsoft.NodeXL.GraphDataProviders.Twitter;
using Microsoft.SocialNetworkLib;

namespace Microsoft.NodeXL.NetworkServer.UnitTests
{
//*****************************************************************************
//  Class: NetworkConfigurationFileParserTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="NetworkConfigurationFileParser" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class NetworkConfigurationFileParserTest : Object
{
    //*************************************************************************
    //  Constructor: NetworkConfigurationFileParserTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="NetworkConfigurationFileParserTest" /> class.
    /// </summary>
    //*************************************************************************

    public NetworkConfigurationFileParserTest()
    {
        m_oNetworkConfigurationFileParser = null;
        m_sTempFileName = null;
    }

    //*************************************************************************
    //  Method: SetUp()
    //
    /// <summary>
    /// Gets run before each test.
    /// </summary>
    //*************************************************************************

    [TestInitializeAttribute]

    public void
    SetUp()
    {
        m_oNetworkConfigurationFileParser =
            new NetworkConfigurationFileParser();

        m_sTempFileName = Path.GetTempFileName();
    }

    //*************************************************************************
    //  Method: TearDown()
    //
    /// <summary>
    /// Gets run after each test.
    /// </summary>
    //*************************************************************************

    [TestCleanupAttribute]

    public void
    TearDown()
    {
        m_oNetworkConfigurationFileParser = null;

        if ( File.Exists(m_sTempFileName) )
        {
            File.Delete(m_sTempFileName);
        }
    }

    //*************************************************************************
    //  Method: TestConstructor()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestConstructor()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: TestOpenNetworkConfigurationFile()
    //
    /// <summary>
    /// Tests the OpenNetworkConfigurationFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestOpenNetworkConfigurationFile()
    {
        WriteSampleNetworkConfigurationFile();

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);
    }

    //*************************************************************************
    //  Method: TestOpenNetworkConfigurationFileBad()
    //
    /// <summary>
    /// Tests the OpenNetworkConfigurationFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestOpenNetworkConfigurationFileBad()
    {
        // Missing folder.

        try
        {
            m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
                @"X:\Abc\NoSuchFile.xyz");
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The network configuration file couldn't be found."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestOpenNetworkConfigurationFileBad2()
    //
    /// <summary>
    /// Tests the OpenNetworkConfigurationFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestOpenNetworkConfigurationFileBad2()
    {
        // Missing file.

        try
        {
            m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
                @"C:\NoSuchFile.xyz");
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The network configuration file couldn't be found."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestOpenNetworkConfigurationFileBad3()
    //
    /// <summary>
    /// Tests the OpenNetworkConfigurationFile() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestOpenNetworkConfigurationFileBad3()
    {
        // Bad XML.

        WriteTempFile("BadXML");

        try
        {
            m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
                m_sTempFileName);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The network configuration file does not contain valid XML."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetNetworkType()
    //
    /// <summary>
    /// Tests the GetNetworkType() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetNetworkType()
    {
        // TwitterSearch.

        WriteSampleNetworkConfigurationFile();

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        Assert.AreEqual( NetworkType.TwitterSearch,
            m_oNetworkConfigurationFileParser.GetNetworkType() );
    }

    //*************************************************************************
    //  Method: TestGetNetworkType2()
    //
    /// <summary>
    /// Tests the GetNetworkType() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetNetworkType2()
    {
        // TwitterUser.

        WriteSampleNetworkConfigurationFile(
            "<NetworkType>.*</NetworkType>",
            "<NetworkType>TwitterUser</NetworkType>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        Assert.AreEqual( NetworkType.TwitterUser,
            m_oNetworkConfigurationFileParser.GetNetworkType() );
    }

    //*************************************************************************
    //  Method: TestGetNetworkTypeBad()
    //
    /// <summary>
    /// Tests the GetNetworkType() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetNetworkTypeBad()
    {
        // Missing value.

        WriteSampleNetworkConfigurationFile(
            "<NetworkType>.*</NetworkType>",
            "<NetworkType></NetworkType>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        try
        {
            m_oNetworkConfigurationFileParser.GetNetworkType();
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The NetworkType value is missing or invalid."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetNetworkTypeBad2()
    //
    /// <summary>
    /// Tests the GetNetworkType() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetNetworkTypeBad2()
    {
        // Bad NetworkType.

        WriteSampleNetworkConfigurationFile(
            "<NetworkType>.*</NetworkType>",
            "<NetworkType>BadValue</NetworkType>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        try
        {
            m_oNetworkConfigurationFileParser.GetNetworkType();
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The NetworkType value is missing or invalid."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfiguration()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetTwitterSearchNetworkConfiguration()
    {
        WriteSampleNetworkConfigurationFile();

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        m_oNetworkConfigurationFileParser.GetTwitterSearchNetworkConfiguration(
            out sSearchTerm, out eWhatToInclude, out iMaximumPeoplePerRequest,
            out sNetworkFileFolder, out eNetworkFileFormats,
            out bAutomateNodeXLWorkbook);

        Assert.AreEqual("Sociology", sSearchTerm);

        Assert.AreEqual(
            TwitterSearchNetworkAnalyzer.WhatToInclude.Statuses |
            TwitterSearchNetworkAnalyzer.WhatToInclude.MentionsEdges,

            eWhatToInclude);

        Assert.AreEqual(100, iMaximumPeoplePerRequest);
        Assert.AreEqual(@"C:\", sNetworkFileFolder);
        Assert.AreEqual(NetworkFileFormats.GraphML, eNetworkFileFormats);
        Assert.IsFalse(bAutomateNodeXLWorkbook);
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfiguration2()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetTwitterSearchNetworkConfiguration2()
    {
        // No MaximumPeoplePerRequest.

        WriteSampleNetworkConfigurationFile(
            "<MaximumPeoplePerRequest>100</MaximumPeoplePerRequest>",
            "<MaximumPeoplePerRequest></MaximumPeoplePerRequest>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        m_oNetworkConfigurationFileParser.GetTwitterSearchNetworkConfiguration(
            out sSearchTerm, out eWhatToInclude, out iMaximumPeoplePerRequest,
            out sNetworkFileFolder, out eNetworkFileFormats,
            out bAutomateNodeXLWorkbook);

        Assert.AreEqual(Int32.MaxValue, iMaximumPeoplePerRequest);
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfiguration3()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetTwitterSearchNetworkConfiguration3()
    {
        // Multiple file formats.

        WriteSampleNetworkConfigurationFile(
            "<NetworkFileFormats>GraphML</NetworkFileFormats>",
            "<NetworkFileFormats>GraphML,NodeXLWorkbook</NetworkFileFormats>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        m_oNetworkConfigurationFileParser.GetTwitterSearchNetworkConfiguration(
            out sSearchTerm, out eWhatToInclude, out iMaximumPeoplePerRequest,
            out sNetworkFileFolder, out eNetworkFileFormats,
            out bAutomateNodeXLWorkbook);

        Assert.AreEqual(
            NetworkFileFormats.GraphML | NetworkFileFormats.NodeXLWorkbook,
            eNetworkFileFormats);
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfiguration4()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetTwitterSearchNetworkConfiguration4()
    {
        // Automate NodeXL workbook.

        WriteSampleNetworkConfigurationFile(
            "<AutomateNodeXLWorkbook>false</AutomateNodeXLWorkbook>",
            "<AutomateNodeXLWorkbook>true</AutomateNodeXLWorkbook>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        m_oNetworkConfigurationFileParser.GetTwitterSearchNetworkConfiguration(
            out sSearchTerm, out eWhatToInclude, out iMaximumPeoplePerRequest,
            out sNetworkFileFolder, out eNetworkFileFormats,
            out bAutomateNodeXLWorkbook);

        Assert.IsTrue(bAutomateNodeXLWorkbook);
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfiguration5()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetTwitterSearchNetworkConfiguration5()
    {
        // Automate NodeXL workbook, mixed case.

        WriteSampleNetworkConfigurationFile(
            "<AutomateNodeXLWorkbook>false</AutomateNodeXLWorkbook>",
            "<AutomateNodeXLWorkbook>TrUe</AutomateNodeXLWorkbook>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        m_oNetworkConfigurationFileParser.GetTwitterSearchNetworkConfiguration(
            out sSearchTerm, out eWhatToInclude, out iMaximumPeoplePerRequest,
            out sNetworkFileFolder, out eNetworkFileFormats,
            out bAutomateNodeXLWorkbook);

        Assert.IsTrue(bAutomateNodeXLWorkbook);
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfigurationBad()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterSearchNetworkConfigurationBad()
    {
        // Missing SearchTerm.

        WriteSampleNetworkConfigurationFile(
            "<SearchTerm>.*</SearchTerm>",
            "<SearchTerm></SearchTerm>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumPeoplePerRequest,
                out sNetworkFileFolder, out eNetworkFileFormats,
                out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            Assert.IsTrue( oXmlException.Message.IndexOf(
                "The XPath is \"SearchTerm") >= 0);

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfigurationBad2()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterSearchNetworkConfigurationBad2()
    {
        // Missing WhatToInclude.

        WriteSampleNetworkConfigurationFile(
            "<WhatToInclude>.*</WhatToInclude>",
            "<WhatToInclude></WhatToInclude>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumPeoplePerRequest,
                out sNetworkFileFolder, out eNetworkFileFormats,
                out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The WhatToInclude value is missing or invalid."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfigurationBad3()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterSearchNetworkConfigurationBad3()
    {
        // Bad WhatToInclude.

        WriteSampleNetworkConfigurationFile(
            "<WhatToInclude>.*</WhatToInclude>",
            "<WhatToInclude>Xyz</WhatToInclude>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumPeoplePerRequest,
                out sNetworkFileFolder, out eNetworkFileFormats,
                out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The WhatToInclude value is missing or invalid."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfigurationBad4()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterSearchNetworkConfigurationBad4()
    {
        // Bad MaximumPeoplePerRequest.

        WriteSampleNetworkConfigurationFile(
            "<MaximumPeoplePerRequest>.*</MaximumPeoplePerRequest>",
            "<MaximumPeoplePerRequest>Xyz</MaximumPeoplePerRequest>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumPeoplePerRequest,
                out sNetworkFileFolder, out eNetworkFileFormats,
                out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual("The MaximumPeoplePerRequest value is not valid.",
                oXmlException.Message);

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfigurationBad5()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterSearchNetworkConfigurationBad5()
    {
        // Missing NetworkFileFolder.

        WriteSampleNetworkConfigurationFile(
            "<NetworkFileFolder>.*</NetworkFileFolder>",
            "<NetworkFileFolder></NetworkFileFolder>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumPeoplePerRequest,
                out sNetworkFileFolder, out eNetworkFileFormats,
                out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            Assert.IsTrue( oXmlException.Message.IndexOf(
                "The XPath is \"NetworkFileFolder") >= 0);

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfigurationBad6()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterSearchNetworkConfigurationBad6()
    {
        // Missing NetworkFileFormats.

        WriteSampleNetworkConfigurationFile(
            "<NetworkFileFormats>.*</NetworkFileFormats>",
            "<NetworkFileFormats></NetworkFileFormats>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumPeoplePerRequest,
                out sNetworkFileFolder, out eNetworkFileFormats,
                out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The NetworkFileFormats value is missing or invalid."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterSearchNetworkConfigurationBad7()
    //
    /// <summary>
    /// Tests the GetTwitterSearchNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterSearchNetworkConfigurationBad7()
    {
        // Bad NetworkFileFormats.

        WriteSampleNetworkConfigurationFile(
            "<NetworkFileFormats>.*</NetworkFileFormats>",
            "<NetworkFileFormats>Xyz</NetworkFileFormats>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sSearchTerm;
        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumPeoplePerRequest,
                out sNetworkFileFolder, out eNetworkFileFormats,
                out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The NetworkFileFormats value is missing or invalid."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterUserNetworkConfiguration()
    //
    /// <summary>
    /// Tests the GetTwitterUserNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetTwitterUserNetworkConfiguration()
    {
        WriteSampleNetworkConfigurationFile(
            "<NetworkType>.*</NetworkType>",
            "<NetworkType>TwitterUser</NetworkType>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sScreenNameToAnalyze;
        TwitterUserNetworkAnalyzer.WhatToInclude eWhatToInclude;
        NetworkLevel eNetworkLevel;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        m_oNetworkConfigurationFileParser.GetTwitterUserNetworkConfiguration(
            out sScreenNameToAnalyze, out eWhatToInclude, out eNetworkLevel,
            out iMaximumPeoplePerRequest, out sNetworkFileFolder,
            out eNetworkFileFormats, out bAutomateNodeXLWorkbook);

        Assert.AreEqual("bob", sScreenNameToAnalyze);

        Assert.AreEqual(
            TwitterUserNetworkAnalyzer.WhatToInclude.FollowedVertices |
            TwitterUserNetworkAnalyzer.WhatToInclude.LatestStatuses,

            eWhatToInclude);

        Assert.AreEqual(NetworkLevel.One, eNetworkLevel);
        Assert.AreEqual(100, iMaximumPeoplePerRequest);
        Assert.AreEqual(@"C:\", sNetworkFileFolder);
        Assert.AreEqual(NetworkFileFormats.GraphML, eNetworkFileFormats);
    }

    //*************************************************************************
    //  Method: TestGetTwitterUserNetworkConfiguration3()
    //
    /// <summary>
    /// Tests the GetTwitterUserNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetTwitterUserNetworkConfiguration3()
    {
        // No MaximumPeoplePerRequest.

        WriteSampleNetworkConfigurationFile(
            "<NetworkType>.*</NetworkType>",
            "<NetworkType>TwitterUser</NetworkType>",
            "<MaximumPeoplePerRequest>100</MaximumPeoplePerRequest>",
            "<MaximumPeoplePerRequest></MaximumPeoplePerRequest>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sScreenNameToAnalyze;
        TwitterUserNetworkAnalyzer.WhatToInclude eWhatToInclude;
        NetworkLevel eNetworkLevel;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        m_oNetworkConfigurationFileParser.GetTwitterUserNetworkConfiguration(
            out sScreenNameToAnalyze, out eWhatToInclude, out eNetworkLevel,
            out iMaximumPeoplePerRequest, out sNetworkFileFolder,
            out eNetworkFileFormats, out bAutomateNodeXLWorkbook);

        Assert.AreEqual(Int32.MaxValue, iMaximumPeoplePerRequest);
    }

    //*************************************************************************
    //  Method: TestGetTwitterUserNetworkConfiguration4()
    //
    /// <summary>
    /// Tests the GetTwitterUserNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetTwitterUserNetworkConfiguration4()
    {
        // Multiple file formats.

        WriteSampleNetworkConfigurationFile(
            "<NetworkType>.*</NetworkType>",
            "<NetworkType>TwitterUser</NetworkType>",

            "<NetworkFileFormats>GraphML</NetworkFileFormats>",
            "<NetworkFileFormats>GraphML,NodeXLWorkbook</NetworkFileFormats>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sScreenNameToAnalyze;
        TwitterUserNetworkAnalyzer.WhatToInclude eWhatToInclude;
        NetworkLevel eNetworkLevel;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        m_oNetworkConfigurationFileParser.GetTwitterUserNetworkConfiguration(
            out sScreenNameToAnalyze, out eWhatToInclude, out eNetworkLevel,
            out iMaximumPeoplePerRequest, out sNetworkFileFolder,
            out eNetworkFileFormats, out bAutomateNodeXLWorkbook);

        Assert.AreEqual(
            NetworkFileFormats.GraphML | NetworkFileFormats.NodeXLWorkbook,
            eNetworkFileFormats);
    }

    //*************************************************************************
    //  Method: TestGetTwitterUserNetworkConfiguration5()
    //
    /// <summary>
    /// Tests the GetTwitterUserNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetTwitterUserNetworkConfiguration5()
    {
        // Automate NodeXL workbook.

        WriteSampleNetworkConfigurationFile(
            "<NetworkType>.*</NetworkType>",
            "<NetworkType>TwitterUser</NetworkType>",

            "<AutomateNodeXLWorkbook>false</AutomateNodeXLWorkbook>",
            "<AutomateNodeXLWorkbook>true</AutomateNodeXLWorkbook>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sScreenNameToAnalyze;
        TwitterUserNetworkAnalyzer.WhatToInclude eWhatToInclude;
        NetworkLevel eNetworkLevel;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        m_oNetworkConfigurationFileParser.GetTwitterUserNetworkConfiguration(
            out sScreenNameToAnalyze, out eWhatToInclude, out eNetworkLevel,
            out iMaximumPeoplePerRequest, out sNetworkFileFolder,
            out eNetworkFileFormats, out bAutomateNodeXLWorkbook);

        Assert.IsTrue(bAutomateNodeXLWorkbook);
    }

    //*************************************************************************
    //  Method: TestGetTwitterUserNetworkConfiguration6()
    //
    /// <summary>
    /// Tests the GetTwitterUserNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetTwitterUserNetworkConfiguration6()
    {
        // Automate NodeXL workbook, upper case.

        WriteSampleNetworkConfigurationFile(
            "<NetworkType>.*</NetworkType>",
            "<NetworkType>TwitterUser</NetworkType>",

            "<AutomateNodeXLWorkbook>false</AutomateNodeXLWorkbook>",
            "<AutomateNodeXLWorkbook>TRUE</AutomateNodeXLWorkbook>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sScreenNameToAnalyze;
        TwitterUserNetworkAnalyzer.WhatToInclude eWhatToInclude;
        NetworkLevel eNetworkLevel;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        m_oNetworkConfigurationFileParser.GetTwitterUserNetworkConfiguration(
            out sScreenNameToAnalyze, out eWhatToInclude, out eNetworkLevel,
            out iMaximumPeoplePerRequest, out sNetworkFileFolder,
            out eNetworkFileFormats, out bAutomateNodeXLWorkbook);

        Assert.IsTrue(bAutomateNodeXLWorkbook);
    }

    //*************************************************************************
    //  Method: TestGetTwitterUserNetworkConfigurationBad()
    //
    /// <summary>
    /// Tests the GetTwitterUserNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterUserNetworkConfigurationBad()
    {
        // Missing ScreenNameToAnalyze.

        WriteSampleNetworkConfigurationFile(
            "<NetworkType>.*</NetworkType>",
            "<NetworkType>TwitterUser</NetworkType>",

            "<ScreenNameToAnalyze>.*</ScreenNameToAnalyze>",
            "<ScreenNameToAnalyze></ScreenNameToAnalyze>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sScreenNameToAnalyze;
        TwitterUserNetworkAnalyzer.WhatToInclude eWhatToInclude;
        NetworkLevel eNetworkLevel;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterUserNetworkConfiguration(out sScreenNameToAnalyze,
                out eWhatToInclude, out eNetworkLevel,
                out iMaximumPeoplePerRequest, out sNetworkFileFolder,
                out eNetworkFileFormats, out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            Assert.IsTrue( oXmlException.Message.IndexOf(
                "The XPath is \"ScreenNameToAnalyze") >= 0);

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterUserNetworkConfigurationBad2()
    //
    /// <summary>
    /// Tests the GetTwitterUserNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterUserNetworkConfigurationBad2()
    {
        // Missing WhatToInclude.

        WriteSampleNetworkConfigurationFile(
            "<NetworkType>.*</NetworkType>",
            "<NetworkType>TwitterUser</NetworkType>",

            "<WhatToInclude>.*</WhatToInclude>",
            "<WhatToInclude></WhatToInclude>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sScreenNameToAnalyze;
        TwitterUserNetworkAnalyzer.WhatToInclude eWhatToInclude;
        NetworkLevel eNetworkLevel;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterUserNetworkConfiguration(out sScreenNameToAnalyze,
                out eWhatToInclude, out eNetworkLevel,
                out iMaximumPeoplePerRequest, out sNetworkFileFolder,
                out eNetworkFileFormats, out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The WhatToInclude value is missing or invalid."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterUserNetworkConfigurationBad3()
    //
    /// <summary>
    /// Tests the GetTwitterUserNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterUserNetworkConfigurationBad3()
    {
        // Bad WhatToInclude.

        WriteSampleNetworkConfigurationFile(
            "<NetworkType>.*</NetworkType>",
            "<NetworkType>TwitterUser</NetworkType>",

            "<WhatToInclude>.*</WhatToInclude>",
            "<WhatToInclude>Xyz</WhatToInclude>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sScreenNameToAnalyze;
        TwitterUserNetworkAnalyzer.WhatToInclude eWhatToInclude;
        NetworkLevel eNetworkLevel;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterUserNetworkConfiguration(out sScreenNameToAnalyze,
                out eWhatToInclude, out eNetworkLevel,
                out iMaximumPeoplePerRequest, out sNetworkFileFolder,
                out eNetworkFileFormats, out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The WhatToInclude value is missing or invalid."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterUserNetworkConfigurationBad4()
    //
    /// <summary>
    /// Tests the GetTwitterUserNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterUserNetworkConfigurationBad4()
    {
        // Bad MaximumPeoplePerRequest.

        WriteSampleNetworkConfigurationFile(
            "<NetworkType>.*</NetworkType>",
            "<NetworkType>TwitterUser</NetworkType>",

            "<MaximumPeoplePerRequest>.*</MaximumPeoplePerRequest>",
            "<MaximumPeoplePerRequest>Xyz</MaximumPeoplePerRequest>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sScreenNameToAnalyze;
        TwitterUserNetworkAnalyzer.WhatToInclude eWhatToInclude;
        NetworkLevel eNetworkLevel;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterUserNetworkConfiguration(out sScreenNameToAnalyze,
                out eWhatToInclude, out eNetworkLevel,
                out iMaximumPeoplePerRequest, out sNetworkFileFolder,
                out eNetworkFileFormats, out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual("The MaximumPeoplePerRequest value is not valid.",
                oXmlException.Message);

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterUserNetworkConfigurationBad5()
    //
    /// <summary>
    /// Tests the GetTwitterUserNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterUserNetworkConfigurationBad5()
    {
        // Missing NetworkFileFolder.

        WriteSampleNetworkConfigurationFile(
            "<NetworkType>.*</NetworkType>",
            "<NetworkType>TwitterUser</NetworkType>",

            "<NetworkFileFolder>.*</NetworkFileFolder>",
            "<NetworkFileFolder></NetworkFileFolder>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sScreenNameToAnalyze;
        TwitterUserNetworkAnalyzer.WhatToInclude eWhatToInclude;
        NetworkLevel eNetworkLevel;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterUserNetworkConfiguration(out sScreenNameToAnalyze,
                out eWhatToInclude, out eNetworkLevel,
                out iMaximumPeoplePerRequest, out sNetworkFileFolder,
                out eNetworkFileFormats, out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            Assert.IsTrue( oXmlException.Message.IndexOf(
                "The XPath is \"NetworkFileFolder") >= 0);

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterUserNetworkConfigurationBad6()
    //
    /// <summary>
    /// Tests the GetTwitterUserNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterUserNetworkConfigurationBad6()
    {
        // Missing NetworkLevel.

        WriteSampleNetworkConfigurationFile(
            "<NetworkType>.*</NetworkType>",
            "<NetworkType>TwitterUser</NetworkType>",

            "<NetworkLevel>.*</NetworkLevel>",
            "<NetworkLevel></NetworkLevel>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sScreenNameToAnalyze;
        TwitterUserNetworkAnalyzer.WhatToInclude eWhatToInclude;
        NetworkLevel eNetworkLevel;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterUserNetworkConfiguration(out sScreenNameToAnalyze,
                out eWhatToInclude, out eNetworkLevel,
                out iMaximumPeoplePerRequest, out sNetworkFileFolder,
                out eNetworkFileFormats, out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The NetworkLevel value is missing or invalid."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterUserNetworkConfigurationBad7()
    //
    /// <summary>
    /// Tests the GetTwitterUserNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterUserNetworkConfigurationBad7()
    {
        // Bad NetworkLevel.

        WriteSampleNetworkConfigurationFile(
            "<NetworkType>.*</NetworkType>",
            "<NetworkType>TwitterUser</NetworkType>",

            "<NetworkLevel>.*</NetworkLevel>",
            "<NetworkLevel>Xyz</NetworkLevel>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sScreenNameToAnalyze;
        TwitterUserNetworkAnalyzer.WhatToInclude eWhatToInclude;
        NetworkLevel eNetworkLevel;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterUserNetworkConfiguration(out sScreenNameToAnalyze,
                out eWhatToInclude, out eNetworkLevel,
                out iMaximumPeoplePerRequest, out sNetworkFileFolder,
                out eNetworkFileFormats, out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The NetworkLevel value is missing or invalid."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterUserNetworkConfigurationBad8()
    //
    /// <summary>
    /// Tests the GetTwitterUserNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterUserNetworkConfigurationBad8()
    {
        // Missing NetworkFileFormats.

        WriteSampleNetworkConfigurationFile(
            "<NetworkType>.*</NetworkType>",
            "<NetworkType>TwitterUser</NetworkType>",

            "<NetworkFileFormats>.*</NetworkFileFormats>",
            "<NetworkFileFormats></NetworkFileFormats>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sScreenNameToAnalyze;
        TwitterUserNetworkAnalyzer.WhatToInclude eWhatToInclude;
        NetworkLevel eNetworkLevel;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterUserNetworkConfiguration(out sScreenNameToAnalyze,
                out eWhatToInclude, out eNetworkLevel,
                out iMaximumPeoplePerRequest, out sNetworkFileFolder,
                out eNetworkFileFormats, out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The NetworkFileFormats value is missing or invalid."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetTwitterUserNetworkConfigurationBad9()
    //
    /// <summary>
    /// Tests the GetTwitterUserNetworkConfiguration() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetTwitterUserNetworkConfigurationBad9()
    {
        // Bad NetworkFileFormats.

        WriteSampleNetworkConfigurationFile(
            "<NetworkType>.*</NetworkType>",
            "<NetworkType>TwitterUser</NetworkType>",

            "<NetworkFileFormats>.*</NetworkFileFormats>",
            "<NetworkFileFormats>Xyz</NetworkFileFormats>"
            );

        m_oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
            m_sTempFileName);

        String sScreenNameToAnalyze;
        TwitterUserNetworkAnalyzer.WhatToInclude eWhatToInclude;
        NetworkLevel eNetworkLevel;
        Int32 iMaximumPeoplePerRequest;
        String sNetworkFileFolder;
        NetworkFileFormats eNetworkFileFormats;
        Boolean bAutomateNodeXLWorkbook;

        try
        {
            m_oNetworkConfigurationFileParser.
                GetTwitterUserNetworkConfiguration(out sScreenNameToAnalyze,
                out eWhatToInclude, out eNetworkLevel,
                out iMaximumPeoplePerRequest, out sNetworkFileFolder,
                out eNetworkFileFormats, out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The NetworkFileFormats value is missing or invalid."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: WriteSampleNetworkConfigurationFile()
    //
    /// <summary>
    /// Writes a copy of the sample network configuration file to a temporary
    /// file after optionally modifying the copy.
    /// </summary>
    ///
    /// <param name="asPatternReplacementPairs">
    /// Array of string pairs.  The first element of each pair is the Regex
    /// pattern to replace in the file.  The second element of each pair is the
    /// replacement string.
    /// </param>
    //*************************************************************************

    protected void
    WriteSampleNetworkConfigurationFile
    (
        params String [] asPatternReplacementPairs
    )
    {
        using ( StreamReader oStreamReader = new StreamReader(
            SampleNetworkConfigurationRelativePath) )
        {
            String sFileContents = oStreamReader.ReadToEnd();

            Int32 iArguments = asPatternReplacementPairs.Length;

            for (Int32 i = 0; i < iArguments; i += 2)
            {
                sFileContents = Regex.Replace(sFileContents,
                    asPatternReplacementPairs[i + 0],
                    asPatternReplacementPairs[i + 1]
                    );
            }

            WriteTempFile(sFileContents);
        }
    }

    //*************************************************************************
    //  Method: WriteTempFile()
    //
    /// <summary>
    /// Writes a temporary file.
    /// </summary>
    //*************************************************************************

    protected void
    WriteTempFile
    (
        String sFileContents
    )
    {
        using ( StreamWriter oStreamWriter =
            new StreamWriter(m_sTempFileName) )
        {
            oStreamWriter.Write(sFileContents);
        }
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Path to the sample network configuration file that is distributed
    /// with the application, relative to the unit test executable.

    protected const String SampleNetworkConfigurationRelativePath
        = @"..\..\..\NetworkServer\SampleNetworkConfiguration.xml";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected NetworkConfigurationFileParser m_oNetworkConfigurationFileParser;

    /// Name of the temporary file that may be created by the unit tests.

    protected String m_sTempFileName;
}

}
