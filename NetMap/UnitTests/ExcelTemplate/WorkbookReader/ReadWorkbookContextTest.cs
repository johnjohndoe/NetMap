
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NetMap.ExcelTemplate;

namespace Microsoft.NetMap.UnitTests
{
//*****************************************************************************
//  Class: ReadWorkbookContextTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="ReadWorkbookContext" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class ReadWorkbookContextTest : Object
{
    //*************************************************************************
    //  Constructor: ReadWorkbookContextTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="ReadWorkbookContextTest" /> class.
    /// </summary>
    //*************************************************************************

    public ReadWorkbookContextTest()
    {
        m_oReadWorkbookContext = null;
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
		m_oReadWorkbookContext = new ReadWorkbookContext();
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
        m_oReadWorkbookContext = null;
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
		Assert.IsTrue(m_oReadWorkbookContext.IgnoreVertexLocations);

		Assert.IsFalse(m_oReadWorkbookContext.FillIDColumns);
		Assert.IsFalse(m_oReadWorkbookContext.PopulateVertexWorksheet);

		Assert.AreEqual(Rectangle.FromLTRB(0, 0, 100, 100),
			m_oReadWorkbookContext.GraphRectangle);
    }

    //*************************************************************************
    //  Method: TestIgnoreVertexLocations()
    //
    /// <summary>
    /// Tests the IgnoreVertexLocations property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIgnoreVertexLocations()
    {
		Assert.IsTrue(m_oReadWorkbookContext.IgnoreVertexLocations);

		m_oReadWorkbookContext.IgnoreVertexLocations = false;

		Assert.IsFalse(m_oReadWorkbookContext.IgnoreVertexLocations);

		m_oReadWorkbookContext.IgnoreVertexLocations = true;

		Assert.IsTrue(m_oReadWorkbookContext.IgnoreVertexLocations);
    }

    //*************************************************************************
    //  Method: TestFillIDColumns()
    //
    /// <summary>
    /// Tests the FillIDColumns property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFillIDColumns()
    {
		Assert.IsFalse(m_oReadWorkbookContext.FillIDColumns);

        m_oReadWorkbookContext.FillIDColumns = true;

        Assert.IsTrue(m_oReadWorkbookContext.FillIDColumns);

        m_oReadWorkbookContext.FillIDColumns = false;

        Assert.IsFalse(m_oReadWorkbookContext.FillIDColumns);
    }

    //*************************************************************************
    //  Method: TestPopulateVertexWorksheet()
    //
    /// <summary>
    /// Tests the PopulateVertexWorksheet property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPopulateVertexWorksheet()
    {
		Assert.IsFalse(m_oReadWorkbookContext.PopulateVertexWorksheet);

        m_oReadWorkbookContext.PopulateVertexWorksheet = true;

        Assert.IsTrue(m_oReadWorkbookContext.PopulateVertexWorksheet);

        m_oReadWorkbookContext.PopulateVertexWorksheet = false;

        Assert.IsFalse(m_oReadWorkbookContext.PopulateVertexWorksheet);
    }

    //*************************************************************************
    //  Method: TestGraphRectangle()
    //
    /// <summary>
    /// Tests the GraphRectangle property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphRectangle()
    {
		m_oReadWorkbookContext.GraphRectangle = GraphRectangle;

		Assert.AreEqual(GraphRectangle,
			m_oReadWorkbookContext.GraphRectangle);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Graph rectangle.

	protected static readonly Rectangle GraphRectangle =
		Rectangle.FromLTRB(5, 6, 10, 20);


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected ReadWorkbookContext m_oReadWorkbookContext;
}

}
