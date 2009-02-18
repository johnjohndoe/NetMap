
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.ExcelTemplate;

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

        Single fExpectedWidthGraph = EdgeWidthConverter.MinimumWidthGraph;

        Single fWidthGraph =
            m_oEdgeWidthConverter.WorkbookToGraph(fWidthWorkbook);

        Assert.AreEqual(fExpectedWidthGraph, fWidthGraph);
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

        Single fExpectedWidthGraph = EdgeWidthConverter.MaximumWidthGraph;

        Single fWidthGraph =
            m_oEdgeWidthConverter.WorkbookToGraph(fWidthWorkbook);

        Assert.AreEqual(fExpectedWidthGraph, fWidthGraph);
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
                EdgeWidthConverter.MinimumWidthWorkbook) / 2;

        Single fExpectedWidthGraph = 
            EdgeWidthConverter.MinimumWidthGraph
            + ( EdgeWidthConverter.MaximumWidthGraph -
                EdgeWidthConverter.MinimumWidthGraph) / 2
            ;

        Single fWidthGraph =
            m_oEdgeWidthConverter.WorkbookToGraph(fWidthWorkbook);

        Assert.AreEqual(fExpectedWidthGraph, fWidthGraph);
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

        Single fWidthWorkbook = EdgeWidthConverter.MinimumWidthWorkbook - 1;

        Single fExpectedWidthGraph = EdgeWidthConverter.MinimumWidthGraph;

        Single fWidthGraph =
            m_oEdgeWidthConverter.WorkbookToGraph(fWidthWorkbook);

        Assert.AreEqual(fExpectedWidthGraph, fWidthGraph);
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

        Single fWidthWorkbook = EdgeWidthConverter.MaximumWidthWorkbook + 1;

        Single fExpectedWidthGraph = EdgeWidthConverter.MaximumWidthGraph;

        Single fWidthGraph =
            m_oEdgeWidthConverter.WorkbookToGraph(fWidthWorkbook);

        Assert.AreEqual(fExpectedWidthGraph, fWidthGraph);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected EdgeWidthConverter m_oEdgeWidthConverter;
}

}
