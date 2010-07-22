
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.GraphDataProviders.Twitter;

namespace Microsoft.NodeXL.GraphDataProviders.Twitter.UnitTests
{
//*****************************************************************************
//  Class: TwitterDateParserTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="TwitterDateParser" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class TwitterDateParserTest : Object
{
    //*************************************************************************
    //  Constructor: TwitterDateParserTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterDateParserTest" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterDateParserTest()
    {
        // (Do nothing.)
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
        // (Do nothing.)
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
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: TestClass()
    //
    /// <summary>
    /// Tests the TryParseTwitterDate() and ParseTwitterDate() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestClass()
    {
        TestData [] aoTestData = new TestData [] {

            new TestData("2010-07-19T19:09:32Z", true,
                new DateTime(2010, 7, 19, 19, 9, 32, DateTimeKind.Utc),
                "2010-07-19 19:09:32"),

            new TestData("2010-07-14T21:33:32Z", true,
                new DateTime(2010, 7, 14, 21, 33, 32, DateTimeKind.Utc),
                "2010-07-14 21:33:32"),

            new TestData("2009-01-09T00:01:02Z", true,
                new DateTime(2009, 1, 9, 0, 1, 2, DateTimeKind.Utc),
                "2009-01-09 00:01:02"),

            new TestData("X009-01-09T00:01:02Z", false,
                DateTime.Now,
                "X009-01-09T00:01:02Z"),

            new TestData("Sat Jan 12 19:12:20 +0000 2008", true,
                new DateTime(2008, 1, 12, 19, 12, 20, DateTimeKind.Utc),
                "2008-01-12 19:12:20"),

            new TestData("Fri May 15 15:12:24 +0000 2009", true,
                new DateTime(2009, 5, 15, 15, 12, 24, DateTimeKind.Utc),
                "2009-05-15 15:12:24"),

            new TestData("Fri Feb 12 14:58:06 +0000 2010", true,
                new DateTime(2010, 2, 12, 14, 58, 6, DateTimeKind.Utc),
                "2010-02-12 14:58:06"),

            new TestData("Xri Feb 12 14:58:06 +0000 2010", false,
                DateTime.Now,
                "Xri Feb 12 14:58:06 +0000 2010"),
            };

        foreach (TestData oTestData in aoTestData)
        {
            DateTime oParsedTwitterDate;

            Boolean bParseable = TwitterDateParser.TryParseTwitterDate(
                oTestData.TwitterDate, out oParsedTwitterDate);

            Assert.AreEqual(oTestData.Parseable, bParseable);

            if (bParseable)
            {
                Assert.AreEqual(oTestData.ParsedTwitterDate,
                    oParsedTwitterDate);
            }

            String sParsedTwitterDateAsString =
                TwitterDateParser.ParseTwitterDate(oTestData.TwitterDate);

            Assert.AreEqual(oTestData.ParsedTwitterDateAsString,
                sParsedTwitterDateAsString);
        }
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Embedded class: TestData
    //
    /// <summary>
    /// Contains data for one test case.
    /// </summary>
    //*************************************************************************

    public class TestData : Object
    {
        //*********************************************************************
        //  Constructor: TestData()
        //
        /// <summary>
        /// Initializes a new instance of the <see cref="TestData" /> class.
        /// </summary>
        //*********************************************************************

        public TestData
        (
            String twitterDate,
            Boolean parseable,
            DateTime parsedDate,
            String parsedDateAsString
        )
        {
            TwitterDate = twitterDate;
            Parseable = parseable;
            ParsedTwitterDate = parsedDate;
            ParsedTwitterDateAsString = parsedDateAsString;
        }


        //*********************************************************************
        //  Public fields
        //*********************************************************************

        /// Date received from Twitter.

        public String TwitterDate;

        /// true if TwitterDate can be parsed.

        public Boolean Parseable;

        /// Parsed TwitterDate if Parseable is true.

        public DateTime ParsedTwitterDate;

        /// Parsed TwitterDate as a String, regardless of the value of
        /// Parseable.

        public String ParsedTwitterDateAsString;
    }
}

}
