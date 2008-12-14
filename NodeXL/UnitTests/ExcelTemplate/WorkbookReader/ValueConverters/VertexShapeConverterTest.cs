
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.ExcelTemplate;
using Microsoft.NodeXL.Visualization;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: VertexShapeConverterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="VertexShapeConverter" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class VertexShapeConverterTest : Object
{
    //*************************************************************************
    //  Constructor: VertexShapeConverterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="VertexShapeConverterTest" /> class.
    /// </summary>
    //*************************************************************************

    public VertexShapeConverterTest()
    {
        m_oVertexShapeConverter = null;
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
		m_oVertexShapeConverter = new VertexShapeConverter();
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
        m_oVertexShapeConverter = null;
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
		VertexDrawer.VertexShape eValueGraph;

		foreach (String sValueWorkbook in new String [] {
			"circle",
			"CIRCLE",
			"Circle (1)",
			"1",
			} )
		{
			Assert.IsTrue( m_oVertexShapeConverter.TryWorkbookToGraph(
				sValueWorkbook, out eValueGraph) );

			Assert.AreEqual(VertexDrawer.VertexShape.Circle, eValueGraph);
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
		VertexDrawer.VertexShape eValueGraph;

		foreach (String sValueWorkbook in new String [] {
			"Disk (2)",
			"disk",
			"DiSk",
			"2",
			} )
		{
			Assert.IsTrue( m_oVertexShapeConverter.TryWorkbookToGraph(
				sValueWorkbook, out eValueGraph) );

			Assert.AreEqual(VertexDrawer.VertexShape.Disk, eValueGraph);
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
		VertexDrawer.VertexShape eValueGraph;

		foreach (String sValueWorkbook in new String [] {
			"Sphere (3)",
			"sphere",
			"SPHEre",
			"3",
			} )
		{
			Assert.IsTrue( m_oVertexShapeConverter.TryWorkbookToGraph(
				sValueWorkbook, out eValueGraph) );

			Assert.AreEqual(VertexDrawer.VertexShape.Sphere, eValueGraph);
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
		VertexDrawer.VertexShape eValueGraph;

		foreach (String sValueWorkbook in new String [] {
			"Square (4)",
			"square",
			"sQUARe",
			"4",
			} )
		{
			Assert.IsTrue( m_oVertexShapeConverter.TryWorkbookToGraph(
				sValueWorkbook, out eValueGraph) );

			Assert.AreEqual(VertexDrawer.VertexShape.Square, eValueGraph);
		}
    }

    //*************************************************************************
    //  Method: TestTryWorkbookToGraph5()
    //
    /// <summary>
    /// Tests the TryWorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryWorkbookToGraph5()
    {
		VertexDrawer.VertexShape eValueGraph;

		foreach (String sValueWorkbook in new String [] {
			"Solid Square (5)",
			"solid square",
			"SoLiD sQUARe",
			"5",
			} )
		{
			Assert.IsTrue( m_oVertexShapeConverter.TryWorkbookToGraph(
				sValueWorkbook, out eValueGraph) );

			Assert.AreEqual(VertexDrawer.VertexShape.SolidSquare, eValueGraph);
		}
    }

    //*************************************************************************
    //  Method: TestTryWorkbookToGraph6()
    //
    /// <summary>
    /// Tests the TryWorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryWorkbookToGraph6()
    {
		VertexDrawer.VertexShape eValueGraph;

		foreach (String sValueWorkbook in new String [] {
			"Diamond (6)",
			"Diamond",
			"dIaMond",
			"6",
			} )
		{
			Assert.IsTrue( m_oVertexShapeConverter.TryWorkbookToGraph(
				sValueWorkbook, out eValueGraph) );

			Assert.AreEqual(VertexDrawer.VertexShape.Diamond, eValueGraph);
		}
    }

    //*************************************************************************
    //  Method: TestTryWorkbookToGraph7()
    //
    /// <summary>
    /// Tests the TryWorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryWorkbookToGraph7()
    {
		VertexDrawer.VertexShape eValueGraph;

		foreach (String sValueWorkbook in new String [] {
			"Solid Diamond (7)",
			"Solid Diamond",
			"SoLID dIaMond",
			"7",
			} )
		{
			Assert.IsTrue( m_oVertexShapeConverter.TryWorkbookToGraph(
				sValueWorkbook, out eValueGraph) );

			Assert.AreEqual(VertexDrawer.VertexShape.SolidDiamond,
				eValueGraph);
		}
    }

    //*************************************************************************
    //  Method: TestTryWorkbookToGraph8()
    //
    /// <summary>
    /// Tests the TryWorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryWorkbookToGraph8()
    {
		VertexDrawer.VertexShape eValueGraph;

		foreach (String sValueWorkbook in new String [] {
			"Triangle (8)",
			"Triangle",
			"tRiangle",
			"8",
			} )
		{
			Assert.IsTrue( m_oVertexShapeConverter.TryWorkbookToGraph(
				sValueWorkbook, out eValueGraph) );

			Assert.AreEqual(VertexDrawer.VertexShape.Triangle, eValueGraph);
		}
    }

    //*************************************************************************
    //  Method: TestTryWorkbookToGraph9()
    //
    /// <summary>
    /// Tests the TryWorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryWorkbookToGraph9()
    {
		VertexDrawer.VertexShape eValueGraph;

		foreach (String sValueWorkbook in new String [] {
			"Solid Triangle (9)",
			"Solid Triangle",
			"SOlid tRiangle",
			"9",
			} )
		{
			Assert.IsTrue( m_oVertexShapeConverter.TryWorkbookToGraph(
				sValueWorkbook, out eValueGraph) );

			Assert.AreEqual(VertexDrawer.VertexShape.SolidTriangle,
				eValueGraph);
		}
    }

    //*************************************************************************
    //  Method: TestTryWorkbookToGraph10()
    //
    /// <summary>
    /// Tests the TryWorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryWorkbookToGraph10()
    {
		VertexDrawer.VertexShape eValueGraph;

		foreach (String sValueWorkbook in new String [] {
			"",
			" disk",
			"x",
			"circle ",
			} )
		{
			Assert.IsFalse( m_oVertexShapeConverter.TryWorkbookToGraph(
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
		Assert.AreEqual( "Circle (1)",
			m_oVertexShapeConverter.GraphToWorkbook(
				VertexDrawer.VertexShape.Circle) );

		Assert.AreEqual( "Disk (2)",
			m_oVertexShapeConverter.GraphToWorkbook(
				VertexDrawer.VertexShape.Disk) );

		Assert.AreEqual( "Sphere (3)",
			m_oVertexShapeConverter.GraphToWorkbook(
				VertexDrawer.VertexShape.Sphere) );

		Assert.AreEqual( "Square (4)",
			m_oVertexShapeConverter.GraphToWorkbook(
				VertexDrawer.VertexShape.Square) );

		Assert.AreEqual( "Solid Square (5)",
			m_oVertexShapeConverter.GraphToWorkbook(
				VertexDrawer.VertexShape.SolidSquare) );

		Assert.AreEqual( "Diamond (6)",
			m_oVertexShapeConverter.GraphToWorkbook(
				VertexDrawer.VertexShape.Diamond) );

		Assert.AreEqual( "Solid Diamond (7)",
			m_oVertexShapeConverter.GraphToWorkbook(
				VertexDrawer.VertexShape.SolidDiamond) );

		Assert.AreEqual( "Triangle (8)",
			m_oVertexShapeConverter.GraphToWorkbook(
				VertexDrawer.VertexShape.Triangle) );

		Assert.AreEqual( "Solid Triangle (9)",
			m_oVertexShapeConverter.GraphToWorkbook(
				VertexDrawer.VertexShape.SolidTriangle) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected VertexShapeConverter m_oVertexShapeConverter;
}

}
