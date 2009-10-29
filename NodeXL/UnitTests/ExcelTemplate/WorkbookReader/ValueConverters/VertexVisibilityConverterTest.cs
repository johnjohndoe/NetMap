
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.ExcelTemplate;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: VertexVisibilityConverterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="VertexVisibilityConverter" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class VertexVisibilityConverterTest : Object
{
    //*************************************************************************
    //  Constructor: VertexVisibilityConverterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VertexVisibilityConverterTest" /> class.
    /// </summary>
    //*************************************************************************

    public VertexVisibilityConverterTest()
    {
        m_oVertexVisibilityConverter = null;
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
        m_oVertexVisibilityConverter = new VertexVisibilityConverter();
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
        m_oVertexVisibilityConverter = null;
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
        VertexWorksheetReader.Visibility eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Show if in an Edge",
            "show if in an edge (1)",
            "show If in aN eDge (1)",
            "1",
            } )
        {
            Assert.IsTrue( m_oVertexVisibilityConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexWorksheetReader.Visibility.ShowIfInAnEdge,
                eValueGraph);
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
        VertexWorksheetReader.Visibility eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Skip",
            "skip (0)",
            "sKip (0)",
            "0",
            } )
        {
            Assert.IsTrue( m_oVertexVisibilityConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexWorksheetReader.Visibility.Skip, eValueGraph);
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
        VertexWorksheetReader.Visibility eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Hide",
            "hide (2)",
            "hidE (2)",
            "2",
            } )
        {
            Assert.IsTrue( m_oVertexVisibilityConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexWorksheetReader.Visibility.Hide, eValueGraph);
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
        VertexWorksheetReader.Visibility eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Show",
            "show (4)",
            "sHoW (4)",
            "4",
            } )
        {
            Assert.IsTrue( m_oVertexVisibilityConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexWorksheetReader.Visibility.Show, eValueGraph);
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
        VertexWorksheetReader.Visibility eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "",
            " hide (2)",
            "x",
            " 2",
            } )
        {
            Assert.IsFalse( m_oVertexVisibilityConverter.TryWorkbookToGraph(
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
        Assert.AreEqual( "Show if in an Edge",
            m_oVertexVisibilityConverter.GraphToWorkbook(
                VertexWorksheetReader.Visibility.ShowIfInAnEdge) );

        Assert.AreEqual( "Skip",
            m_oVertexVisibilityConverter.GraphToWorkbook(
                VertexWorksheetReader.Visibility.Skip) );

        Assert.AreEqual( "Hide",
            m_oVertexVisibilityConverter.GraphToWorkbook(
                VertexWorksheetReader.Visibility.Hide) );

        Assert.AreEqual( "Show",
            m_oVertexVisibilityConverter.GraphToWorkbook(
                VertexWorksheetReader.Visibility.Show) );
    }

    //*************************************************************************
    //  Method: VerifyUpToDate()
    //
    /// <summary>
    /// Makes sure that these tests are up to date.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    VerifyUpToDate()
    {
        Assert.AreEqual(4, Enum.GetValues(
            typeof(VertexWorksheetReader.Visibility) ).Length);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected VertexVisibilityConverter m_oVertexVisibilityConverter;
}

}
