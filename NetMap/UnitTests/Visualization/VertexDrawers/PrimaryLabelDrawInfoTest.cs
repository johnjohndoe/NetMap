
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NetMap.Visualization;

namespace Microsoft.NetMap.UnitTests
{
//*****************************************************************************
//  Class: PrimaryLabelDrawInfoTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="PrimaryLabelDrawInfo" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class PrimaryLabelDrawInfoTest : Object
{
    //*************************************************************************
    //  Constructor: PrimaryLabelDrawInfoTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="PrimaryLabelDrawInfoTest" /> class.
    /// </summary>
    //*************************************************************************

    public PrimaryLabelDrawInfoTest()
    {
        m_oPrimaryLabelDrawInfo = null;
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
        m_oPrimaryLabelDrawInfo = new PrimaryLabelDrawInfo(
			PrimaryLabel, TextRectangle, OutlineRectangle);
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
        m_oPrimaryLabelDrawInfo = null;
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
		Assert.AreEqual(PrimaryLabel, m_oPrimaryLabelDrawInfo.PrimaryLabel);
		Assert.AreEqual(TextRectangle, m_oPrimaryLabelDrawInfo.TextRectangle);

		Assert.AreEqual(OutlineRectangle,
			m_oPrimaryLabelDrawInfo.OutlineRectangle);
    }

    //*************************************************************************
    //  Method: TestConstructor2()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestConstructor2()
    {
		// null primary label.

        m_oPrimaryLabelDrawInfo = new PrimaryLabelDrawInfo(
			null, TextRectangle, OutlineRectangle);

		Assert.IsNull(m_oPrimaryLabelDrawInfo.PrimaryLabel);
		Assert.AreEqual(TextRectangle, m_oPrimaryLabelDrawInfo.TextRectangle);

		Assert.AreEqual(OutlineRectangle,
			m_oPrimaryLabelDrawInfo.OutlineRectangle);
    }

    //*************************************************************************
    //  Method: TestConstructor3()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestConstructor3()
    {
		// Empty primary label.

        m_oPrimaryLabelDrawInfo = new PrimaryLabelDrawInfo(
			String.Empty, TextRectangle, OutlineRectangle);

		Assert.AreEqual(String.Empty, m_oPrimaryLabelDrawInfo.PrimaryLabel);
		Assert.AreEqual(TextRectangle, m_oPrimaryLabelDrawInfo.TextRectangle);

		Assert.AreEqual(OutlineRectangle,
			m_oPrimaryLabelDrawInfo.OutlineRectangle);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	///
	protected const String PrimaryLabel = "The primary label.";
	///
	protected static readonly RectangleF TextRectangle =
		new RectangleF(4, 5, 6, 7);
	///
	protected static readonly Rectangle OutlineRectangle =
		new Rectangle(10, 11, 12, 13); 


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected PrimaryLabelDrawInfo m_oPrimaryLabelDrawInfo;
}

}
