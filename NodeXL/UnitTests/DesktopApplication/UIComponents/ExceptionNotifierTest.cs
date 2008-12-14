
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.DesktopApplication;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: ExceptionNotifierTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="ExceptionNotifier" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class ExceptionNotifierTest : Object
{
    //*************************************************************************
    //  Constructor: ExceptionNotifierTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="ExceptionNotifierTest" /> class.
    /// </summary>
    //*************************************************************************

    public ExceptionNotifierTest()
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
    //  Method: TestExceptionToMessage()
    //
    /// <summary>
    /// Tests the ExceptionToMessage method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestExceptionToMessage()
    {
		// One-level exception.

		ApplicationException oException =
			new ApplicationException("Level 1.");

		String sMessage = ExceptionNotifier.ExceptionToMessage(
			oException, "General description.");

		const String ExpectedMessage =

			"General description.\r\n"
			+ "\r\n"
			+ "If the problem persists, please press Ctrl-C to copy the"
			+ " details to the clipboard, then e-mail the details to"
			+ " conviz@microsoft.com.\r\n"
			+ "\r\n"
			+ "Details:\r\n"
			+ "\r\n"
			+ "[ApplicationException]: Level 1."
			;

		Assert.AreEqual(
			ExpectedMessage,
			sMessage
			);
    }

    //*************************************************************************
    //  Method: TestExceptionToMessage2()
    //
    /// <summary>
    /// Tests the ExceptionToMessage method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestExceptionToMessage2()
    {
		// Multi-level exception.

		const Int32 Levels = 4;

		ApplicationException oException = null;

		for (Int32 i = Levels; i > 0; i--)
		{
			oException = new ApplicationException(
				"Level " + i.ToString() + ".", oException);
		}

		String sMessage = ExceptionNotifier.ExceptionToMessage(
			oException, "General description.");

		const String ExpectedMessage =

			"General description.\r\n"
			+ "\r\n"
			+ "If the problem persists, please press Ctrl-C to copy the"
			+ " details to the clipboard, then e-mail the details to"
			+ " conviz@microsoft.com.\r\n"
			+ "\r\n"
			+ "Details:\r\n"
			+ "\r\n"
			+ "[ApplicationException]: Level 1.\r\n"
			+ "[ApplicationException]: Level 2.\r\n"
			+ "[ApplicationException]: Level 3.\r\n"
			+ "[ApplicationException]: Level 4."
			;

		Assert.AreEqual(
			ExpectedMessage,
			sMessage
			);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
