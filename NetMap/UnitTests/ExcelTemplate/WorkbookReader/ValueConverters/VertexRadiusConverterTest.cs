
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NetMap.ExcelTemplate;
using Microsoft.NetMap.Visualization;

namespace Microsoft.NetMap.UnitTests
{
//*****************************************************************************
//  Class: VertexRadiusConverterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="VertexRadiusConverter" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class VertexRadiusConverterTest : Object
{
    //*************************************************************************
    //  Constructor: VertexRadiusConverterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="VertexRadiusConverterTest" /> class.
    /// </summary>
    //*************************************************************************

    public VertexRadiusConverterTest()
    {
        m_oVertexRadiusConverter = null;
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
		m_oVertexRadiusConverter = new VertexRadiusConverter();
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
        m_oVertexRadiusConverter = null;
    }

    //*************************************************************************
    //  Method: TestWorkbookToGraph()
    //
    /// <summary>
    /// Tests the WorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToGraph()
    {
		// Minimum.

		Single fRadiusWorkbook = VertexRadiusConverter.MinimumRadiusWorkbook;

		Single fExpectedRadiusGraph = PerVertexDrawer.MinimumRadius;

		Single fRadiusGraph =
			m_oVertexRadiusConverter.WorkbookToGraph(fRadiusWorkbook);

		Assert.AreEqual(fExpectedRadiusGraph, fRadiusGraph);
    }

    //*************************************************************************
    //  Method: TestWorkbookToGraph2()
    //
    /// <summary>
    /// Tests the WorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToGraph2()
    {
		// Maximum.

		Single fRadiusWorkbook = VertexRadiusConverter.MaximumRadiusWorkbook;

		Single fExpectedRadiusGraph = PerVertexDrawer.MaximumRadius;

		Single fRadiusGraph =
			m_oVertexRadiusConverter.WorkbookToGraph(fRadiusWorkbook);

		Assert.AreEqual(fExpectedRadiusGraph, fRadiusGraph);
    }

    //*************************************************************************
    //  Method: TestWorkbookToGraph3()
    //
    /// <summary>
    /// Tests the WorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToGraph3()
    {
		// Midpoint.

		Single fRadiusWorkbook = VertexRadiusConverter.MinimumRadiusWorkbook
			+ (VertexRadiusConverter.MaximumRadiusWorkbook -
				VertexRadiusConverter.MinimumRadiusWorkbook) / 2F;

		Single fExpectedRadiusGraph = PerVertexDrawer.MinimumRadius
			+ (PerVertexDrawer.MaximumRadius - PerVertexDrawer.MinimumRadius)
			/ 2F;

		Single fRadiusGraph =
			m_oVertexRadiusConverter.WorkbookToGraph(fRadiusWorkbook);

		Assert.AreEqual(fExpectedRadiusGraph, fRadiusGraph);
    }

    //*************************************************************************
    //  Method: TestWorkbookToGraph4()
    //
    /// <summary>
    /// Tests the WorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToGraph4()
    {
		// Below minimum.

		Single fRadiusWorkbook =
			VertexRadiusConverter.MinimumRadiusWorkbook - 1F;

		Single fExpectedRadiusGraph = PerVertexDrawer.MinimumRadius;

		Single fRadiusGraph =
			m_oVertexRadiusConverter.WorkbookToGraph(fRadiusWorkbook);

		Assert.AreEqual(fExpectedRadiusGraph, fRadiusGraph);
    }

    //*************************************************************************
    //  Method: TestWorkbookToGraph5()
    //
    /// <summary>
    /// Tests the WorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToGraph5()
    {
		// Above maximum.

		Single fRadiusWorkbook =
			VertexRadiusConverter.MaximumRadiusWorkbook + 1F;

		Single fExpectedRadiusGraph = PerVertexDrawer.MaximumRadius;

		Single fRadiusGraph =
			m_oVertexRadiusConverter.WorkbookToGraph(fRadiusWorkbook);

		Assert.AreEqual(fExpectedRadiusGraph, fRadiusGraph);
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
		// Minimum.

		Single fRadiusGraph = VertexRadiusConverter.MinimumRadiusGraph;

		Single fExpectedRadiusWorkbook =
            VertexRadiusConverter.MinimumRadiusWorkbook;

		Single fRadiusWorkbook =
			m_oVertexRadiusConverter.GraphToWorkbook(fRadiusGraph);

		Assert.AreEqual(fExpectedRadiusWorkbook, fRadiusWorkbook);
    }

    //*************************************************************************
    //  Method: TestGraphToWorkbook2()
    //
    /// <summary>
    /// Tests the GraphToWorkbook() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphToWorkbook2()
    {
		// Maximum.

		Single fRadiusGraph = VertexRadiusConverter.MaximumRadiusGraph;

		Single fExpectedRadiusWorkbook =
			VertexRadiusConverter.MaximumRadiusWorkbook;

		Single fRadiusWorkbook =
			m_oVertexRadiusConverter.GraphToWorkbook(fRadiusGraph);

		Assert.AreEqual(fExpectedRadiusWorkbook, fRadiusWorkbook);
    }

    //*************************************************************************
    //  Method: TestGraphToWorkbook3()
    //
    /// <summary>
    /// Tests the GraphToWorkbook() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphToWorkbook3()
    {
		// Midpoint.

		Single fRadiusGraph = VertexRadiusConverter.MinimumRadiusGraph
			+ (VertexRadiusConverter.MaximumRadiusGraph -
				VertexRadiusConverter.MinimumRadiusGraph) / 2F;

		Single fExpectedRadiusWorkbook =
			VertexRadiusConverter.MinimumRadiusWorkbook
			+ (VertexRadiusConverter.MaximumRadiusWorkbook -
				VertexRadiusConverter.MinimumRadiusWorkbook) / 2F;

		Single fRadiusWorkbook =
			m_oVertexRadiusConverter.GraphToWorkbook(fRadiusGraph);

		Assert.AreEqual(fExpectedRadiusWorkbook, fRadiusWorkbook);
    }

    //*************************************************************************
    //  Method: TestGraphToWorkbook4()
    //
    /// <summary>
    /// Tests the GraphToWorkbook() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphToWorkbook4()
    {
		// Below minimum.

		Single fRadiusGraph =
			VertexRadiusConverter.MinimumRadiusGraph - 1F;

		Single fExpectedRadiusWorkbook =
			VertexRadiusConverter.MinimumRadiusWorkbook;

		Single fRadiusWorkbook =
			m_oVertexRadiusConverter.GraphToWorkbook(fRadiusGraph);

		Assert.AreEqual(fExpectedRadiusWorkbook, fRadiusWorkbook);
    }

    //*************************************************************************
    //  Method: TestGraphToWorkbook5()
    //
    /// <summary>
    /// Tests the GraphToWorkbook() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphToWorkbook5()
    {
		// Above maximum.

		Single fRadiusGraph =
			VertexRadiusConverter.MaximumRadiusGraph + 1F;

		Single fExpectedRadiusWorkbook =
			VertexRadiusConverter.MaximumRadiusWorkbook;

		Single fRadiusWorkbook =
			m_oVertexRadiusConverter.GraphToWorkbook(fRadiusGraph);

		Assert.AreEqual(fExpectedRadiusWorkbook, fRadiusWorkbook);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected VertexRadiusConverter m_oVertexRadiusConverter;
}

}
