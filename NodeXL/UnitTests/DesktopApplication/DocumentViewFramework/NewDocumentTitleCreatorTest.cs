
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.DesktopApplication;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: NewDocumentTitleCreatorTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="NewDocumentTitleCreator" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class NewDocumentTitleCreatorTest : Object
{
    //*************************************************************************
    //  Constructor: NewDocumentTitleCreatorTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="NewDocumentTitleCreatorTest" /> class.
    /// </summary>
    //*************************************************************************

    public NewDocumentTitleCreatorTest()
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
        // (No constructor, do nothing.)
    }

    //*************************************************************************
    //  Method: TestCreateDocumentTitle()
    //
    /// <summary>
    /// Tests the CreateDocumentTitle method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCreateDocumentTitle()
    {
		Int32 iPreviousIndex = 0;

		for (Int32 i = 0; i < 10; i++)
		{
			String sDocumentTitle = NewDocumentTitleCreator.CreateTitle();

			Assert.IsTrue( sDocumentTitle.StartsWith("Graph") );

			String sIndex = sDocumentTitle.Substring(5);

			Int32 iIndex;

			Assert.IsTrue( Int32.TryParse(sIndex, out iIndex) );

			Assert.IsTrue(iIndex > 0);

			if (i != 0)
			{
				Assert.AreEqual(iPreviousIndex + 1, iIndex);
			}

			iPreviousIndex = iIndex;
		}
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
