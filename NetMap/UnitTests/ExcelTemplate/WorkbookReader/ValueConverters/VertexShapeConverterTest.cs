
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NetMap.ExcelTemplate;
using Microsoft.NetMap.Visualization;

namespace Microsoft.NetMap.UnitTests
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
			"disk",
			"DiSk",
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
			"sphere",
			"SPHEre",
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
		Assert.AreEqual( "Circle",
			m_oVertexShapeConverter.GraphToWorkbook(
				VertexDrawer.VertexShape.Circle) );

		Assert.AreEqual( "Disk",
			m_oVertexShapeConverter.GraphToWorkbook(
				VertexDrawer.VertexShape.Disk) );

		Assert.AreEqual( "Sphere",
			m_oVertexShapeConverter.GraphToWorkbook(
				VertexDrawer.VertexShape.Sphere) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected VertexShapeConverter m_oVertexShapeConverter;
}

}
