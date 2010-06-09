using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.Research.CommunityTechnologies.Common.UnitTests
{
//*****************************************************************************
//  Class: ExceptionUtilTest
//
/// <summary>
/// This is a test fixture for the ExceptionUtil class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class ExceptionUtilTest : Object
{
    //*************************************************************************
    //  Constructor: ExceptionUtilTest()
    //
    /// <summary>
    /// Initializes a new instance of the ExceptionUtilTest class.
    /// </summary>
    //*************************************************************************

    public ExceptionUtilTest()
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
    //  Method: TestGetMessageTrace()
    //
    /// <summary>
    /// Tests the GetMessageTrace method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetMessageTrace()
    {
        const String Message = "jkfdjkr3uirjker";

        Exception oException = new Exception(Message);
        String sMessageTrace = ExceptionUtil.GetMessageTrace(oException);

        Assert.AreEqual("[Exception]: " + Message, sMessageTrace);
    }

    //*************************************************************************
    //  Method: TestGetMessageTrace2()
    //
    /// <summary>
    /// Tests the GetMessageTrace method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetMessageTrace2()
    {
        const String Message1 = "jfdjk388d8d8";
        const String Message2 = "jkfd jkdf jkfd jk\r\njkfjkfdjkfd";
        const String Message3 = "jkfdjkfdjei32i3i3i";

        Exception oException3 = new ArgumentException(Message3);

        Exception oException2 = new InvalidOperationException(
            Message2, oException3);

        Exception oException1 = new Exception(Message1, oException2);

        String sMessageTrace = ExceptionUtil.GetMessageTrace(oException1);

        Assert.AreEqual(

            "[Exception]: " + Message1 + "\r\n"
            + "[InvalidOperationException]: " + Message2 + "\r\n"
            + "[ArgumentException]: " + Message3
            ,
            sMessageTrace);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
