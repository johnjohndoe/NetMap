
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.ExcelTemplate;
using Microsoft.NodeXL.Visualization;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: EdgeVisibilityConverterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="EdgeVisibilityConverter" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class EdgeVisibilityConverterTest : Object
{
    //*************************************************************************
    //  Constructor: EdgeVisibilityConverterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="EdgeVisibilityConverterTest" /> class.
    /// </summary>
    //*************************************************************************

    public EdgeVisibilityConverterTest()
    {
        m_oEdgeVisibilityConverter = null;
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
		m_oEdgeVisibilityConverter = new EdgeVisibilityConverter();
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
        m_oEdgeVisibilityConverter = null;
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
		EdgeWorksheetReader.Visibility eValueGraph;

		foreach (String sValueWorkbook in new String [] {
			"show (1)",
			"ShoW (1)",
			"1",
			} )
		{
			Assert.IsTrue( m_oEdgeVisibilityConverter.TryWorkbookToGraph(
				sValueWorkbook, out eValueGraph) );

			Assert.AreEqual(EdgeWorksheetReader.Visibility.Show, eValueGraph);
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
		EdgeWorksheetReader.Visibility eValueGraph;

		foreach (String sValueWorkbook in new String [] {
			"skip (0)",
			"sKip (0)",
			"0",
			} )
		{
			Assert.IsTrue( m_oEdgeVisibilityConverter.TryWorkbookToGraph(
				sValueWorkbook, out eValueGraph) );

			Assert.AreEqual(EdgeWorksheetReader.Visibility.Skip, eValueGraph);
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
		EdgeWorksheetReader.Visibility eValueGraph;

		foreach (String sValueWorkbook in new String [] {
			"hide (2)",
			"hidE (2)",
			"2",
			} )
		{
			Assert.IsTrue( m_oEdgeVisibilityConverter.TryWorkbookToGraph(
				sValueWorkbook, out eValueGraph) );

			Assert.AreEqual(EdgeWorksheetReader.Visibility.Hide, eValueGraph);
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
		EdgeWorksheetReader.Visibility eValueGraph;

		foreach (String sValueWorkbook in new String [] {
			"",
			" hide (2)",
			"x",
			" 2",
			} )
		{
			Assert.IsFalse( m_oEdgeVisibilityConverter.TryWorkbookToGraph(
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
		Assert.AreEqual( "Show (1)",
			m_oEdgeVisibilityConverter.GraphToWorkbook(
				EdgeWorksheetReader.Visibility.Show) );

		Assert.AreEqual( "Skip (0)",
			m_oEdgeVisibilityConverter.GraphToWorkbook(
				EdgeWorksheetReader.Visibility.Skip) );

		Assert.AreEqual( "Hide (2)",
			m_oEdgeVisibilityConverter.GraphToWorkbook(
				EdgeWorksheetReader.Visibility.Hide) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected EdgeVisibilityConverter m_oEdgeVisibilityConverter;
}

}
