
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.ExcelTemplate;
using Microsoft.NodeXL.Visualization.Wpf;

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
        VertexShape eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Circle",
            "CIRCLE",
            "Circle (1)",
            "1",
            } )
        {
            Assert.IsTrue( m_oVertexShapeConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexShape.Circle, eValueGraph);
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
        VertexShape eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Disk",
            "Disk (2)",
            "DiSk",
            "2",
            } )
        {
            Assert.IsTrue( m_oVertexShapeConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexShape.Disk, eValueGraph);
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
        VertexShape eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Sphere",
            "Sphere (3)",
            "SPHEre",
            "3",
            } )
        {
            Assert.IsTrue( m_oVertexShapeConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexShape.Sphere, eValueGraph);
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
        VertexShape eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Square",
            "Square (4)",
            "sQUARe",
            "4",
            } )
        {
            Assert.IsTrue( m_oVertexShapeConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexShape.Square, eValueGraph);
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
        VertexShape eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Solid Square",
            "Solid Square (5)",
            "SoLiD sQUARe",
            "5",
            } )
        {
            Assert.IsTrue( m_oVertexShapeConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexShape.SolidSquare, eValueGraph);
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
        VertexShape eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Diamond",
            "Diamond (6)",
            "dIaMond",
            "6",
            } )
        {
            Assert.IsTrue( m_oVertexShapeConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexShape.Diamond, eValueGraph);
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
        VertexShape eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Solid Diamond",
            "Solid Diamond (7)",
            "SoLID dIaMond",
            "7",
            } )
        {
            Assert.IsTrue( m_oVertexShapeConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexShape.SolidDiamond, eValueGraph);
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
        VertexShape eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Triangle",
            "Triangle (8)",
            "tRiangle",
            "8",
            } )
        {
            Assert.IsTrue( m_oVertexShapeConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexShape.Triangle, eValueGraph);
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
        VertexShape eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Solid Triangle",
            "Solid Triangle (9)",
            "SOlid tRiangle",
            "9",
            } )
        {
            Assert.IsTrue( m_oVertexShapeConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexShape.SolidTriangle, eValueGraph);
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
        VertexShape eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Label",
            "Label (10)",
            "LABEL",
            "10",
            } )
        {
            Assert.IsTrue( m_oVertexShapeConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexShape.Label, eValueGraph);
        }
    }

    //*************************************************************************
    //  Method: TestTryWorkbookToGraph11()
    //
    /// <summary>
    /// Tests the TryWorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryWorkbookToGraph11()
    {
        VertexShape eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "Image",
            "Image (11)",
            "ImAgE",
            "11",
            } )
        {
            Assert.IsTrue( m_oVertexShapeConverter.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(VertexShape.Image, eValueGraph);
        }
    }

    //*************************************************************************
    //  Method: TestTryWorkbookToGraph12()
    //
    /// <summary>
    /// Tests the TryWorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryWorkbookToGraph12()
    {
        VertexShape eValueGraph;

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
            m_oVertexShapeConverter.GraphToWorkbook(VertexShape.Circle) );

        Assert.AreEqual( "Disk",
            m_oVertexShapeConverter.GraphToWorkbook(VertexShape.Disk) );

        Assert.AreEqual( "Sphere",
            m_oVertexShapeConverter.GraphToWorkbook(VertexShape.Sphere) );

        Assert.AreEqual( "Square",
            m_oVertexShapeConverter.GraphToWorkbook(VertexShape.Square) );

        Assert.AreEqual( "Solid Square",
            m_oVertexShapeConverter.GraphToWorkbook(VertexShape.SolidSquare) );

        Assert.AreEqual( "Diamond",
            m_oVertexShapeConverter.GraphToWorkbook(VertexShape.Diamond) );

        Assert.AreEqual( "Solid Diamond",
            m_oVertexShapeConverter.GraphToWorkbook(
                VertexShape.SolidDiamond) );

        Assert.AreEqual( "Triangle",
            m_oVertexShapeConverter.GraphToWorkbook(VertexShape.Triangle) );

        Assert.AreEqual( "Solid Triangle",
            m_oVertexShapeConverter.GraphToWorkbook(
                VertexShape.SolidTriangle) );

        Assert.AreEqual( "Label",
            m_oVertexShapeConverter.GraphToWorkbook(VertexShape.Label) );

        Assert.AreEqual( "Image",
            m_oVertexShapeConverter.GraphToWorkbook(VertexShape.Image) );
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
        Assert.AreEqual(11, Enum.GetValues( typeof(VertexShape) ).Length);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected VertexShapeConverter m_oVertexShapeConverter;
}

}
