
//	Copyright (c) Microsoft Corporation.  All rights reserved.

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
    //  Method: TestGetOneBasedRowNumbers()
    //
    /// <summary>
    /// Tests the GetOneBasedRowNumbers() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetOneBasedRowNumbers()
    {
		TestGetOneBasedRowNumbers( "A1", new Int32[] {1} );
    }

    //*************************************************************************
    //  Method: TestGetOneBasedRowNumbers2()
    //
    /// <summary>
    /// Tests the GetOneBasedRowNumbers() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetOneBasedRowNumbers2()
    {
		TestGetOneBasedRowNumbers( "B429", new Int32[] {429} );
    }

    //*************************************************************************
    //  Method: TestGetOneBasedRowNumbers3()
    //
    /// <summary>
    /// Tests the GetOneBasedRowNumbers() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetOneBasedRowNumbers3()
    {
		TestGetOneBasedRowNumbers( "XDF1048576", new Int32[] {1048576} );
    }

    //*************************************************************************
    //  Method: TestGetOneBasedRowNumbers4()
    //
    /// <summary>
    /// Tests the GetOneBasedRowNumbers() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetOneBasedRowNumbers4()
    {
		TestGetOneBasedRowNumbers( "A1:B5", new Int32[] {1, 2, 3, 4, 5} );
    }

    //*************************************************************************
    //  Method: TestGetOneBasedRowNumbers5()
    //
    /// <summary>
    /// Tests the GetOneBasedRowNumbers() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetOneBasedRowNumbers5()
    {
		TestGetOneBasedRowNumbers( "A3,B16,A30,A24:F25,C2:C5,A5:F5",
			new Int32[] {3, 16, 30, 24, 25, 2, 4, 5} );
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
    //  Method: TestGetOneBasedRowNumbers()
    //
    /// <summary>
    /// Tests the GetOneBasedRowNumbers() method.
    /// </summary>
	///
	/// <param name="sRangeAddress">
	/// Cell address.
	/// </param>
	///
	/// <param name="aiExpectedRowNumbers">
	/// Expected row numbers.
	/// </param>
    //*************************************************************************

    protected void
    TestGetOneBasedRowNumbers
	(
		String sRangeAddress,
		Int32 [] aiExpectedRowNumbers
	)
    {
		Int32 [] aiRowNumbers = ExcelUtil.GetOneBasedRowNumbers(sRangeAddress);

		Int32 iLength = aiRowNumbers.Length;

		Assert.AreEqual(aiExpectedRowNumbers.Length, iLength);

		for (Int32 i = 0; i < iLength; i++)
		{
			Assert.IsTrue(Array.IndexOf<Int32>(
				aiRowNumbers, aiExpectedRowNumbers[i] ) != -1);
		}
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
