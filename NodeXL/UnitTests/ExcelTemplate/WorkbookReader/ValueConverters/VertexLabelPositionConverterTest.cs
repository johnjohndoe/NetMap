
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.ExcelTemplate;
using Microsoft.NodeXL.Visualization.Wpf;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: VertexLabelPositionConverterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="VertexLabelPositionConverter" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class VertexLabelPositionConverterTest : Object
{
    //*************************************************************************
    //  Constructor: VertexLabelPositionConverterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VertexLabelPositionConverterTest" /> class.
    /// </summary>
    //*************************************************************************

    public VertexLabelPositionConverterTest()
    {
        m_oVertexLabelPositionConverter = null;
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
        m_oVertexLabelPositionConverter = new VertexLabelPositionConverter();
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
        m_oVertexLabelPositionConverter = null;
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
        VertexLabelPosition eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Top Left",
            "TOP LEFT",
            "1",
            } )
        {
            Assert.IsTrue( m_oVertexLabelPositionConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexLabelPosition.TopLeft, eValueGraph);
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
        VertexLabelPosition eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Top Center",
            "Top CeNTER",
            "2",
            } )
        {
            Assert.IsTrue( m_oVertexLabelPositionConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexLabelPosition.TopCenter, eValueGraph);
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
        VertexLabelPosition eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Top Right",
            "tOp RIGHT",
            "3",
            } )
        {
            Assert.IsTrue( m_oVertexLabelPositionConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexLabelPosition.TopRight, eValueGraph);
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
        VertexLabelPosition eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Middle Left",
            "MIDDLE lEFT",
            "4",
            } )
        {
            Assert.IsTrue( m_oVertexLabelPositionConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexLabelPosition.MiddleLeft, eValueGraph);
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
        VertexLabelPosition eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Middle Center",
            "MIDDLE center",
            "5",
            } )
        {
            Assert.IsTrue( m_oVertexLabelPositionConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexLabelPosition.MiddleCenter, eValueGraph);
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
        VertexLabelPosition eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Middle Right",
            "MIDDLE RIGHT",
            "6",
            } )
        {
            Assert.IsTrue( m_oVertexLabelPositionConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexLabelPosition.MiddleRight, eValueGraph);
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
        VertexLabelPosition eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Bottom Left",
            "BOTTOM Left",
            "7",
            } )
        {
            Assert.IsTrue( m_oVertexLabelPositionConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexLabelPosition.BottomLeft, eValueGraph);
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
        VertexLabelPosition eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Bottom Center",
            "bottom center",
            "8",
            } )
        {
            Assert.IsTrue( m_oVertexLabelPositionConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexLabelPosition.BottomCenter, eValueGraph);
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
        VertexLabelPosition eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Bottom Right",
            "BOTTOM RIGHT",
            "9",
            } )
        {
            Assert.IsTrue( m_oVertexLabelPositionConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexLabelPosition.BottomRight, eValueGraph);
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
        VertexLabelPosition eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "",
            " bottom right",
            "x",
            "top left ",
            } )
        {
            Assert.IsFalse( m_oVertexLabelPositionConverter.TryWorkbookToGraph(
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
        Assert.AreEqual( "Top Left",
            m_oVertexLabelPositionConverter.GraphToWorkbook(
                VertexLabelPosition.TopLeft) );

        Assert.AreEqual( "Top Center",
            m_oVertexLabelPositionConverter.GraphToWorkbook(
                VertexLabelPosition.TopCenter) );

        Assert.AreEqual( "Top Right",
            m_oVertexLabelPositionConverter.GraphToWorkbook(
                VertexLabelPosition.TopRight) );

        Assert.AreEqual( "Middle Left",
            m_oVertexLabelPositionConverter.GraphToWorkbook(
                VertexLabelPosition.MiddleLeft) );

        Assert.AreEqual( "Middle Center",
            m_oVertexLabelPositionConverter.GraphToWorkbook(
                VertexLabelPosition.MiddleCenter) );

        Assert.AreEqual( "Middle Right",
            m_oVertexLabelPositionConverter.GraphToWorkbook(
                VertexLabelPosition.MiddleRight) );

        Assert.AreEqual( "Bottom Left",
            m_oVertexLabelPositionConverter.GraphToWorkbook(
                VertexLabelPosition.BottomLeft) );

        Assert.AreEqual( "Bottom Center",
            m_oVertexLabelPositionConverter.GraphToWorkbook(
                VertexLabelPosition.BottomCenter) );

        Assert.AreEqual( "Bottom Right",
            m_oVertexLabelPositionConverter.GraphToWorkbook(
                VertexLabelPosition.BottomRight) );
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
        Assert.AreEqual(9,
            Enum.GetValues( typeof(VertexLabelPosition) ).Length);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected VertexLabelPositionConverter m_oVertexLabelPositionConverter;
}

}
