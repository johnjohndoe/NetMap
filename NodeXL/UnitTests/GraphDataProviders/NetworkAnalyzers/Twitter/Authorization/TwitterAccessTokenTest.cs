
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.GraphDataProviders.Twitter;

namespace Microsoft.NodeXL.GraphDataProviders.Twitter.UnitTests
{
//*****************************************************************************
//  Class: TwitterAccessTokenTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="TwitterAccessToken" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class TwitterAccessTokenTest : Object
{
    //*************************************************************************
    //  Constructor: TwitterAccessTokenTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterAccessTokenTest" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterAccessTokenTest()
    {
        m_oTwitterAccessToken = null;
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
        m_oTwitterAccessToken = new TwitterAccessToken();

        DeleteAccessTokenFile();
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
        DeleteAccessTokenFile();

        m_oTwitterAccessToken = null;
    }

    //*************************************************************************
    //  Method: TestSave()
    //
    /// <summary>
    /// Tests the Save() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSave()
    {
        m_oTwitterAccessToken.Save(Token, Secret);

        Assert.IsTrue( File.Exists(
            TwitterAccessToken.GetAccessTokenFilePath() ) );

        String sFileContents;

        using ( StreamReader oStreamReader =
            new StreamReader(TwitterAccessToken.GetAccessTokenFilePath() ) )
        {
            sFileContents = oStreamReader.ReadToEnd();
        }

        Assert.AreEqual(Token + "\t" + Secret, sFileContents);
    }

    //*************************************************************************
    //  Method: TestTryLoad()
    //
    /// <summary>
    /// Tests the TryLoad() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryLoad()
    {
        // No file.

        String sToken, sSecret;

        Assert.IsFalse( m_oTwitterAccessToken.TryLoad(
            out sToken, out sSecret) );
    }

    //*************************************************************************
    //  Method: TestTryLoad2()
    //
    /// <summary>
    /// Tests the TryLoad() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryLoad2()
    {
        // File exists.

        using ( StreamWriter oStreamWriter =
            new StreamWriter( TwitterAccessToken.GetAccessTokenFilePath() ) )
        {
            oStreamWriter.Write(Token + "\t" + Secret);
        }

        String sToken, sSecret;

        Assert.IsTrue( m_oTwitterAccessToken.TryLoad(
            out sToken, out sSecret) );

        Assert.AreEqual(Token, sToken);
        Assert.AreEqual(Secret, sSecret);
    }

    //*************************************************************************
    //  Method: TestTryLoad3()
    //
    /// <summary>
    /// Tests the TryLoad() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryLoad3()
    {
        // Test caching.

        m_oTwitterAccessToken.Save(Token, Secret);

        Assert.IsTrue( File.Exists(
            TwitterAccessToken.GetAccessTokenFilePath() ) );

        DeleteAccessTokenFile();

        Assert.IsFalse( File.Exists(
            TwitterAccessToken.GetAccessTokenFilePath() ) );

        String sToken, sSecret;

        Assert.IsTrue( m_oTwitterAccessToken.TryLoad(
            out sToken, out sSecret) );

        Assert.AreEqual(Token, sToken);
        Assert.AreEqual(Secret, sSecret);
    }

    //*************************************************************************
    //  Method: TestExists()
    //
    /// <summary>
    /// Tests the Exists() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestExists()
    {
        // No file.

        Assert.IsFalse( TwitterAccessToken.Exists() );
    }

    //*************************************************************************
    //  Method: TestExists2()
    //
    /// <summary>
    /// Tests the Exists() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestExists2()
    {
        // File exists.

        using ( StreamWriter oStreamWriter =
            new StreamWriter( TwitterAccessToken.GetAccessTokenFilePath() ) )
        {
            oStreamWriter.Write(Token + "\t" + Secret);
        }

        Assert.IsTrue( TwitterAccessToken.Exists() );
    }

    //*************************************************************************
    //  Method: TestDelete()
    //
    /// <summary>
    /// Tests the Delete() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestDelete()
    {
        // No file.

        Assert.IsFalse( File.Exists(
            TwitterAccessToken.GetAccessTokenFilePath() ) );

        TwitterAccessToken.Delete();

        Assert.IsFalse( File.Exists(
            TwitterAccessToken.GetAccessTokenFilePath() ) );
    }

    //*************************************************************************
    //  Method: TestDelete2()
    //
    /// <summary>
    /// Tests the Delete() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestDelete2()
    {
        // File exists.

        using ( StreamWriter oStreamWriter =
            new StreamWriter( TwitterAccessToken.GetAccessTokenFilePath() ) )
        {
            oStreamWriter.Write(Token + "\t" + Secret);
        }

        Assert.IsTrue( File.Exists(
            TwitterAccessToken.GetAccessTokenFilePath() ) );

        TwitterAccessToken.Delete();

        Assert.IsFalse( File.Exists(
            TwitterAccessToken.GetAccessTokenFilePath() ) );
    }

    //*************************************************************************
    //  Method: DeleteAccessTokenFile()
    //
    /// <summary>
    /// Deletes the current user's Twitter access token file.
    /// </summary>
    //*************************************************************************

    protected void
    DeleteAccessTokenFile()
    {
        try
        {
            File.Delete( TwitterAccessToken.GetAccessTokenFilePath() );
        }
        catch (DirectoryNotFoundException)
        {
        }
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Token and secret to use for testing.

    protected const String Token = "TheToken";
    ///
    protected const String Secret = "TheSecret";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object being tested.

    protected TwitterAccessToken m_oTwitterAccessToken;
}

}
