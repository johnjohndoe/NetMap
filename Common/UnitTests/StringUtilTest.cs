using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.Research.CommunityTechnologies.Common.UnitTests
{
//*****************************************************************************
//  Class: StringUtilTest
//
/// <summary>
/// This is a test fixture for the StringUtil class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class StringUtilTest : Object
{
    //*************************************************************************
    //  Constructor: StringUtilTest()
    //
    /// <summary>
    /// Initializes a new instance of the StringUtilTest class.
    /// </summary>
    //*************************************************************************

    public StringUtilTest()
    {
		// (Do nothing.)
    }

	//*************************************************************************
	//	Method: SetUp()
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
	//	Method: TearDown()
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
	//	Method: TestIsEmpty()
	//
	/// <summary>
	/// Tests the IsEmpty method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestIsEmpty()
	{
		Assert.IsTrue( StringUtil.IsEmpty(null) );
	}

	//*************************************************************************
	//	Method: TestIsEmpty2()
	//
	/// <summary>
	/// Tests the IsEmpty method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestIsEmpty2()
	{
		Assert.IsTrue( StringUtil.IsEmpty("") );
	}

	//*************************************************************************
	//	Method: TestIsEmpty3()
	//
	/// <summary>
	/// Tests the IsEmpty method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestIsEmpty3()
	{
		Assert.IsFalse( StringUtil.IsEmpty("a") );
	}

	//*************************************************************************
	//	Method: TestCreateArrayOfEmptyStrings()
	//
	/// <summary>
	/// Tests the CreateArrayOfEmptyStrings method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestCreateArrayOfEmptyStrings()
	{
		String [] asStrings = StringUtil.CreateArrayOfEmptyStrings(0);

		Assert.AreEqual(0, asStrings.Length);
	}

	//*************************************************************************
	//	Method: TestCreateArrayOfEmptyStrings2()
	//
	/// <summary>
	/// Tests the CreateArrayOfEmptyStrings method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestCreateArrayOfEmptyStrings2()
	{
		String [] asStrings = StringUtil.CreateArrayOfEmptyStrings(2);

		Assert.AreEqual(2, asStrings.Length);
		Assert.AreEqual( String.Empty, asStrings[0] );
		Assert.AreEqual( String.Empty, asStrings[1] );
	}

    //*************************************************************************
    //  Method: TestBytesToPrintableAscii()
    //
    /// <summary>
    /// Tests the BytesToPrintableAscii method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestBytesToPrintableAscii()
    {
		Byte [] abtBytes = new Byte [256];

		for (Int32 i = 0; i < 256; i++)
			abtBytes[i] = (Byte)i;

		const String Expected =

			"!!!!!!!!!\u0009\u000A!!\u000D!!!!!!!!!!!!!!!!!!"
			+ " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~"
			+ "!"
			+ "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"
			+ "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"
			+ "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"
			+ "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"
			;

		Assert.AreEqual(
			Expected,
			StringUtil.BytesToPrintableAscii(abtBytes, '!')
			);
    }


	//*************************************************************************
	//	Method: TestReplaceNonPrintableAsciiCharacters()
	//
	/// <summary>
	/// Tests the ReplaceNonPrintableAsciiCharacters method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestReplaceNonPrintableAsciiCharacters()
	{
		const String TestString = "";

		String sString =
			StringUtil.ReplaceNonPrintableAsciiCharacters(TestString, '?');

		Assert.AreEqual(TestString, sString);
	}

	//*************************************************************************
	//	Method: TestReplaceNonPrintableAsciiCharacters2()
	//
	/// <summary>
	/// Tests the ReplaceNonPrintableAsciiCharacters method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestReplaceNonPrintableAsciiCharacters2()
	{
		const String TestString = "1234567890-=!@#$%^&*()_+abcdefghijklmnopqrs"
			+ "tuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ[{]}\\|;:'\",<.>/?";

		String sString =
			StringUtil.ReplaceNonPrintableAsciiCharacters(TestString, '?');

		Assert.AreEqual(TestString, sString);
	}

	//*************************************************************************
	//	Method: TestReplaceNonPrintableAsciiCharacters3()
	//
	/// <summary>
	/// Tests the ReplaceNonPrintableAsciiCharacters method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestReplaceNonPrintableAsciiCharacters3()
	{
		// Test lower range.

		String sString = StringUtil.ReplaceNonPrintableAsciiCharacters(

			"x"
			+ "\u0000"
			+ "\u0001"
			+ "\u0002"
			+ "\u0003"
			+ "\u0004"
			+ "\u0005"
			+ "\u0006"
			+ "\u0007"
			+ "\u0008"
			+ "\u0009"
			+ "\u000A"
			+ "\u000B"
			+ "\u000C"
			+ "\u000D"
			+ "\u000E"
			+ "\u000F"
			+ "\u0010"
			+ "\u0011"
			+ "\u0012"
			+ "\u0013"
			+ "\u0014"
			+ "\u0015"
			+ "\u0016"
			+ "\u0017"
			+ "\u0018"
			+ "\u0019"
			+ "\u001A"
			+ "\u001B"
			+ "\u001C"
			+ "\u001D"
			+ "\u001E"
			+ "\u001F"
			+ "x"
			,
			'?'
			);

		Assert.AreEqual(

			"x"
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "\u0009"  // OK
			+ "\u000A"  // OK
			+ "?"
			+ "?"
			+ "\u000D"  // OK
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "?"
			+ "x"
			,
			sString
			);
	}

	//*************************************************************************
	//	Method: TestReplaceNonPrintableAsciiCharacters4()
	//
	/// <summary>
	/// Tests the ReplaceNonPrintableAsciiCharacters method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestReplaceNonPrintableAsciiCharacters4()
	{
		// Test 7F.

		String sString =
			StringUtil.ReplaceNonPrintableAsciiCharacters("x \u007F x", '?');

		Assert.AreEqual("x ? x", sString);
	}

	//*************************************************************************
	//	Method: TestReplaceNonPrintableAsciiCharacters5()
	//
	/// <summary>
	/// Tests the ReplaceNonPrintableAsciiCharacters method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestReplaceNonPrintableAsciiCharacters5()
	{
		// Test upper range.

		StringBuilder oStringBuilder = new StringBuilder();

		oStringBuilder.Append('x');

		for (Int32 i = 0x80; i <= 0xFF; i++)
			oStringBuilder.Append( Convert.ToChar(i) );

		oStringBuilder.Append('x');

		String sString =
			StringUtil.ReplaceNonPrintableAsciiCharacters(
				oStringBuilder.ToString(), '?');

		Assert.AreEqual(
			"x"
			+ "????????????????????????????????"
			+ "????????????????????????????????"
			+ "????????????????????????????????"
			+ "????????????????????????????????"
			+ "x",
			sString);
	}

	//*************************************************************************
	//	Method: TestReplaceNonPrintableAsciiCharacters6()
	//
	/// <summary>
	/// Tests the ReplaceNonPrintableAsciiCharacters method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestReplaceNonPrintableAsciiCharacters6()
	{
		String sString =
			StringUtil.ReplaceNonPrintableAsciiCharacters("x \uFFFF x", '?');

		Assert.AreEqual("x ? x", sString);
	}

	//*************************************************************************
	//	Method: TestReplaceNonPrintableAsciiCharacters7()
	//
	/// <summary>
	/// Tests the ReplaceNonPrintableAsciiCharacters method.
	/// </summary>
	//*************************************************************************

	[TestMethodAttribute]

	public void
	TestReplaceNonPrintableAsciiCharacters7()
	{
		String sString = StringUtil.ReplaceNonPrintableAsciiCharacters(
			"x \u0000 \u007F \uFFFF x", '?');

		Assert.AreEqual("x ? ? ? x", sString);
	}


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
