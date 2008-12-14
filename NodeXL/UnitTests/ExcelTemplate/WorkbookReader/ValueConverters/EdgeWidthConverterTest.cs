
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.ExcelTemplate;
using Microsoft.NodeXL.Visualization;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: EdgeWidthConverterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="EdgeWidthConverter" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class EdgeWidthConverterTest : Object
{
    //*************************************************************************
    //  Constructor: EdgeWidthConverterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="EdgeWidthConverterTest" /> class.
    /// </summary>
    //*************************************************************************

    public EdgeWidthConverterTest()
    {
        m_oEdgeWidthConverter = null;
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
		m_oEdgeWidthConverter = new EdgeWidthConverter();
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
        m_oEdgeWidthConverter = null;
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

		Single fWidthWorkbook = EdgeWidthConverter.MinimumWidthWorkbook;

		Int32 iExpectedWidthGraph = EdgeWidthConverter.MinimumWidthGraph;

		Int32 iWidthGraph =
			m_oEdgeWidthConverter.WorkbookToGraph(fWidthWorkbook);

		Assert.AreEqual(iExpectedWidthGraph, iWidthGraph);
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

		Single fWidthWorkbook = EdgeWidthConverter.MaximumWidthWorkbook;

		Int32 iExpectedWidthGraph = EdgeWidthConverter.MaximumWidthGraph;

		Int32 iWidthGraph =
			m_oEdgeWidthConverter.WorkbookToGraph(fWidthWorkbook);

		Assert.AreEqual(iExpectedWidthGraph, iWidthGraph);
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

		Single fWidthWorkbook = EdgeWidthConverter.MinimumWidthWorkbook
			+ (EdgeWidthConverter.MaximumWidthWorkbook -
				EdgeWidthConverter.MinimumWidthWorkbook) / 2F;

		Int32 iExpectedWidthGraph = (Int32)(
			(Single)EdgeWidthConverter.MinimumWidthGraph
			+ ( (Single)EdgeWidthConverter.MaximumWidthGraph -
				(Single)EdgeWidthConverter.MinimumWidthGraph) / 2F
			);

		Int32 iWidthGraph =
			m_oEdgeWidthConverter.WorkbookToGraph(fWidthWorkbook);

		Assert.AreEqual(iExpectedWidthGraph, iWidthGraph);
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

		Single fWidthWorkbook = EdgeWidthConverter.MinimumWidthWorkbook - 1F;

		Int32 iExpectedWidthGraph = EdgeWidthConverter.MinimumWidthGraph;

		Int32 iWidthGraph =
			m_oEdgeWidthConverter.WorkbookToGraph(fWidthWorkbook);

		Assert.AreEqual(iExpectedWidthGraph, iWidthGraph);
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

		Single fWidthWorkbook = EdgeWidthConverter.MaximumWidthWorkbook + 1F;

		Int32 iExpectedWidthGraph = EdgeWidthConverter.MaximumWidthGraph;

		Int32 iWidthGraph =
			m_oEdgeWidthConverter.WorkbookToGraph(fWidthWorkbook);

		Assert.AreEqual(iExpectedWidthGraph, iWidthGraph);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected EdgeWidthConverter m_oEdgeWidthConverter;
}

}
