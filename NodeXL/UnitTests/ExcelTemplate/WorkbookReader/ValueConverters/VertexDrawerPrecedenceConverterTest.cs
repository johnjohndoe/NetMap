
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.ExcelTemplate;
using Microsoft.NodeXL.Visualization;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: VertexDrawerPrecedenceConverterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="VertexDrawerPrecedenceConverter" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class VertexDrawerPrecedenceConverterTest : Object
{
    //*************************************************************************
    //  Constructor: VertexDrawerPrecedenceConverterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="VertexDrawerPrecedenceConverterTest" /> class.
    /// </summary>
    //*************************************************************************

    public VertexDrawerPrecedenceConverterTest()
    {
        m_oVertexDrawerPrecedenceConverter = null;
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
		m_oVertexDrawerPrecedenceConverter =
			new VertexDrawerPrecedenceConverter();
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
        m_oVertexDrawerPrecedenceConverter = null;
    }

    //*************************************************************************
    //  Method: TestTryWorkbookToGraph()
    //
    /// <summary>
    /// Tests the TryWorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryWorkbookToGraph()
    {
		VertexDrawerPrecedence eValueGraph;

		foreach (String sValueWorkbook in new String [] {
			"shape (1)",
			"sHape (1)",
			"1",
			} )
		{
			Assert.IsTrue(
				m_oVertexDrawerPrecedenceConverter.TryWorkbookToGraph(
					sValueWorkbook, out eValueGraph) );

			Assert.AreEqual(VertexDrawerPrecedence.Shape, eValueGraph);
		}
    }

    //*************************************************************************
    //  Method: TestTryWorkbookToGraph2()
    //
    /// <summary>
    /// Tests the TryWorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryWorkbookToGraph2()
    {
		VertexDrawerPrecedence eValueGraph;

		foreach (String sValueWorkbook in new String [] {
			"image (2)",
			"iMage (2)",
			"2",
			} )
		{
			Assert.IsTrue(
				m_oVertexDrawerPrecedenceConverter.TryWorkbookToGraph(
					sValueWorkbook, out eValueGraph) );

			Assert.AreEqual(VertexDrawerPrecedence.Image, eValueGraph);
		}
    }

    //*************************************************************************
    //  Method: TestTryWorkbookToGraph3()
    //
    /// <summary>
    /// Tests the TryWorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryWorkbookToGraph3()
    {
		VertexDrawerPrecedence eValueGraph;

		foreach (String sValueWorkbook in new String [] {
			"primary label (3)",
			"PriMary label (3)",
			"3",
			} )
		{
			Assert.IsTrue(
				m_oVertexDrawerPrecedenceConverter.TryWorkbookToGraph(
					sValueWorkbook, out eValueGraph) );

			Assert.AreEqual(VertexDrawerPrecedence.PrimaryLabel, eValueGraph);
		}
    }

    //*************************************************************************
    //  Method: TestTryWorkbookToGraph4()
    //
    /// <summary>
    /// Tests the TryWorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryWorkbookToGraph4()
    {
		VertexDrawerPrecedence eValueGraph;

		foreach (String sValueWorkbook in new String [] {
			"",
			" shape (1)",
			"x",
			" 1",
			} )
		{
			Assert.IsFalse(
				m_oVertexDrawerPrecedenceConverter.TryWorkbookToGraph(
					sValueWorkbook, out eValueGraph) );
		}
    }

    //*************************************************************************
    //  Method: TestGraphToWorkbook()
    //
    /// <summary>
    /// Tests the GraphToWorkbook() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphToWorkbook()
    {
		Assert.AreEqual( "Shape (1)",
			m_oVertexDrawerPrecedenceConverter.GraphToWorkbook(
				VertexDrawerPrecedence.Shape) );

		Assert.AreEqual( "Image (2)",
			m_oVertexDrawerPrecedenceConverter.GraphToWorkbook(
				VertexDrawerPrecedence.Image) );

		Assert.AreEqual( "Primary Label (3)",
			m_oVertexDrawerPrecedenceConverter.GraphToWorkbook(
				VertexDrawerPrecedence.PrimaryLabel) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected VertexDrawerPrecedenceConverter m_oVertexDrawerPrecedenceConverter;
}

}
