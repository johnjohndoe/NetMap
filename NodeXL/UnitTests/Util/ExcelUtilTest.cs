
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: ExcelUtilTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="ExcelUtil" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class ExcelUtilTest : Object
{
    //*************************************************************************
    //  Constructor: ExcelUtilTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ExcelUtilTest" /> class.
    /// </summary>
    //*************************************************************************

    public ExcelUtilTest()
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
    //  Method: TestGetOneBasedRowNumber()
    //
    /// <summary>
    /// Tests the GetOneBasedRowNumber() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetOneBasedRowNumber()
    {
        TestGetOneBasedRowNumber("A1", 1);
    }

    //*************************************************************************
    //  Method: TestGetOneBasedRowNumber2()
    //
    /// <summary>
    /// Tests the GetOneBasedRowNumber() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetOneBasedRowNumber2()
    {
        TestGetOneBasedRowNumber("B429", 429);
    }

    //*************************************************************************
    //  Method: TestGetOneBasedRowNumber3()
    //
    /// <summary>
    /// Tests the GetOneBasedRowNumber() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetOneBasedRowNumber3()
    {
        TestGetOneBasedRowNumber("XDF1048576", 1048576);
    }

    //*************************************************************************
    //  Method: TestGetColumnLetter()
    //
    /// <summary>
    /// Tests the GetColumnLetter() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetColumnLetter()
    {
        TestGetColumnLetter("A1", "A");
    }

    //*************************************************************************
    //  Method: TestGetColumnLetter2()
    //
    /// <summary>
    /// Tests the GetColumnLetter() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetColumnLetter2()
    {
        TestGetColumnLetter("B429", "B");
    }

    //*************************************************************************
    //  Method: TestGetColumnLetter3()
    //
    /// <summary>
    /// Tests the GetColumnLetter() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetColumnLetter3()
    {
        TestGetColumnLetter("XDF1048576", "XDF");
    }

    //*************************************************************************
    //  Method: TestGetOneBasedRowNumber()
    //
    /// <summary>
    /// Tests the GetOneBasedRowNumber() method.
    /// </summary>
    ///
    /// <param name="sCellAddress">
    /// Cell address.
    /// </param>
    ///
    /// <param name="iExpectedRowNumber`">
    /// Expected row number.
    /// </param>
    //*************************************************************************

    protected void
    TestGetOneBasedRowNumber
    (
        String sCellAddress,
        Int32 iExpectedRowNumber
    )
    {
        Assert.AreEqual( iExpectedRowNumber,
            ExcelUtil.GetOneBasedRowNumber(sCellAddress) );
    }

    //*************************************************************************
    //  Method: TestGetColumnLetter()
    //
    /// <summary>
    /// Tests the GetColumnLetter() method.
    /// </summary>
    ///
    /// <param name="sCellAddress">
    /// Cell address.
    /// </param>
    ///
    /// <param name="sExpectedColumnLetter`">
    /// Expected column letter.
    /// </param>
    //*************************************************************************

    protected void
    TestGetColumnLetter
    (
        String sCellAddress,
        String sExpectedColumnLetter
    )
    {
        Assert.AreEqual( sExpectedColumnLetter,
            ExcelUtil.GetColumnLetter(sCellAddress) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
