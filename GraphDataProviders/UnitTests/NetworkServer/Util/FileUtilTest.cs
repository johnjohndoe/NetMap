
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.NetworkServer;

namespace Microsoft.NodeXL.NetworkServer.UnitTests
{
//*****************************************************************************
//  Class: FileUtilTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="FileUtil" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class FileUtilTest : Object
{
    //*************************************************************************
    //  Constructor: FileUtilTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="FileUtilTest" /> class.
    /// </summary>
    //*************************************************************************

    public FileUtilTest()
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
    //  Method: TestGetOutputFilePath()
    //
    /// <summary>
    /// Tests the GetOutputFilePath() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetOutputFilePath()
    {
        Assert.AreEqual(

            @"T:\NetworkFiles\"
            + "Prefix_NetworkConfiguration_2010-01-02_09-08-07.extension",

            
            FileUtil.GetOutputFilePath(

                new DateTime(2010, 1, 2, 9, 8, 7),
                @"E:\NetworkConfigurations\NetworkConfiguration.xml",
                @"T:\NetworkFiles",
                "Prefix_",
                "extension")
            );
    }

    //*************************************************************************
    //  Method: TestGetOutputFilePath2()
    //
    /// <summary>
    /// Tests the GetOutputFilePath() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetOutputFilePath2()
    {
        // No prefix.

        Assert.AreEqual(

            @"..\NetworkFiles\"
            + "NetworkConfiguration_2010-01-02_09-08-07.extension",

            
            FileUtil.GetOutputFilePath(

                new DateTime(2010, 1, 2, 9, 8, 7),
                @"NetworkConfiguration.xml",
                @"..\NetworkFiles",
                String.Empty,
                "extension")
            );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
