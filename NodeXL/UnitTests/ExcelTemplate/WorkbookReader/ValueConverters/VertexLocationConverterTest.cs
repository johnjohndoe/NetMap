
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.ExcelTemplate;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: VertexLocationConverterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="VertexLocationConverter" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class VertexLocationConverterTest : Object
{
    //*************************************************************************
    //  Constructor: VertexLocationConverterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VertexLocationConverterTest" /> class.
    /// </summary>
    //*************************************************************************

    public VertexLocationConverterTest()
    {
        m_oVertexLocationConverter = null;
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
        m_oVertexLocationConverter =
            new VertexLocationConverter(GraphRectangle);
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
        m_oVertexLocationConverter = null;
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
        // Minimum workbook values.

        Single WorkbookX = VertexLocationConverter.MinimumXYWorkbook;
        Single WorkbookY = VertexLocationConverter.MinimumXYWorkbook;
        Single ExpectedGraphX = 100;
        Single ExpectedGraphY = 2200;

        PointF oGraphPointF = m_oVertexLocationConverter.WorkbookToGraph(
            WorkbookX, WorkbookY);

        Assert.AreEqual(ExpectedGraphX, oGraphPointF.X);
        Assert.AreEqual(ExpectedGraphY, oGraphPointF.Y);
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
        // Maximum workbook values.

        Single WorkbookX = VertexLocationConverter.MaximumXYWorkbook;
        Single WorkbookY = VertexLocationConverter.MaximumXYWorkbook;
        Single ExpectedGraphX = 1100;
        Single ExpectedGraphY = 200;

        PointF oGraphPointF = m_oVertexLocationConverter.WorkbookToGraph(
            WorkbookX, WorkbookY);

        Assert.AreEqual(ExpectedGraphX, oGraphPointF.X);
        Assert.AreEqual(ExpectedGraphY, oGraphPointF.Y);
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
        // Midpoint workbook values.

        Single WorkbookX = VertexLocationConverter.MaximumXYWorkbook / 2;
        Single WorkbookY = VertexLocationConverter.MaximumXYWorkbook / 2;
        Single ExpectedGraphX = 600;
        Single ExpectedGraphY = 1200;

        PointF oGraphPointF = m_oVertexLocationConverter.WorkbookToGraph(
            WorkbookX, WorkbookY);

        Assert.AreEqual( ExpectedGraphX, Math.Round(oGraphPointF.X) );
        Assert.AreEqual( ExpectedGraphY, Math.Round(oGraphPointF.Y) );
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
        // Empty rectangle.

        m_oVertexLocationConverter = new VertexLocationConverter(
            Rectangle.FromLTRB(100, 200, 100, 200) );

        Single WorkbookX = VertexLocationConverter.MaximumXYWorkbook;
        Single WorkbookY = VertexLocationConverter.MaximumXYWorkbook;
        Single ExpectedGraphX = 100;
        Single ExpectedGraphY = 200;

        PointF oGraphPointF = m_oVertexLocationConverter.WorkbookToGraph(
            WorkbookX, WorkbookY);

        Assert.AreEqual(ExpectedGraphX, oGraphPointF.X);
        Assert.AreEqual(ExpectedGraphY, oGraphPointF.Y);
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
        // Below minimum workbook values.

        Single WorkbookX = VertexLocationConverter.MinimumXYWorkbook - 1F;
        Single WorkbookY = VertexLocationConverter.MinimumXYWorkbook - 123F;
        Single ExpectedGraphX = 100;
        Single ExpectedGraphY = 2200;

        PointF oGraphPointF = m_oVertexLocationConverter.WorkbookToGraph(
            WorkbookX, WorkbookY);

        Assert.AreEqual(ExpectedGraphX, oGraphPointF.X);
        Assert.AreEqual(ExpectedGraphY, oGraphPointF.Y);
    }

    //*************************************************************************
    //  Method: TestWorkbookToGraph6()
    //
    /// <summary>
    /// Tests the WorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToGraph6()
    {
        // Above maximum workbook values.

        Single WorkbookX = VertexLocationConverter.MaximumXYWorkbook + 1F;
        Single WorkbookY = VertexLocationConverter.MaximumXYWorkbook + 123F;
        Single ExpectedGraphX = 1100;
        Single ExpectedGraphY = 200;

        PointF oGraphPointF = m_oVertexLocationConverter.WorkbookToGraph(
            WorkbookX, WorkbookY);

        Assert.AreEqual(ExpectedGraphX, oGraphPointF.X);
        Assert.AreEqual(ExpectedGraphY, oGraphPointF.Y);
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
        // Minimum graph values.

        Single fGraphX = 100;
        Single fGraphY = 200;
        Single ExpectedWorkbookX = VertexLocationConverter.MinimumXYWorkbook;
        Single ExpectedWorkbookY = VertexLocationConverter.MaximumXYWorkbook;

        Single fWorkbookX, fWorkbookY;

        m_oVertexLocationConverter.GraphToWorkbook(
            new PointF(fGraphX, fGraphY), out fWorkbookX, out fWorkbookY);

        Assert.AreEqual(ExpectedWorkbookX, fWorkbookX);
        Assert.AreEqual(ExpectedWorkbookY, fWorkbookY);
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
        // Maximum graph values.

        Single fGraphX = 1100;
        Single fGraphY = 2200;
        Single ExpectedWorkbookX = VertexLocationConverter.MaximumXYWorkbook;
        Single ExpectedWorkbookY = VertexLocationConverter.MinimumXYWorkbook;

        Single fWorkbookX, fWorkbookY;

        m_oVertexLocationConverter.GraphToWorkbook(
            new PointF(fGraphX, fGraphY), out fWorkbookX, out fWorkbookY);

        Assert.AreEqual(ExpectedWorkbookX, fWorkbookX);
        Assert.AreEqual(ExpectedWorkbookY, fWorkbookY);
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
        // Midpoint graph values.

        Single fGraphX = 600;
        Single fGraphY = 1200;

        Single ExpectedWorkbookX = VertexLocationConverter.MinimumXYWorkbook + 
            (VertexLocationConverter.MaximumXYWorkbook -
                VertexLocationConverter.MinimumXYWorkbook) / 2;

        Single ExpectedWorkbookY = ExpectedWorkbookX;

        Single fWorkbookX, fWorkbookY;

        m_oVertexLocationConverter.GraphToWorkbook(
            new PointF(fGraphX, fGraphY), out fWorkbookX, out fWorkbookY);

        Assert.AreEqual(ExpectedWorkbookX, fWorkbookX);
        Assert.AreEqual(ExpectedWorkbookY, fWorkbookY);
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
        // Empty rectangle.

        m_oVertexLocationConverter = new VertexLocationConverter(
            Rectangle.FromLTRB(100, 200, 100, 200) );

        Single fGraphX = 600;
        Single fGraphY = 1200;

        Single ExpectedWorkbookX = VertexLocationConverter.MinimumXYWorkbook;
        Single ExpectedWorkbookY = ExpectedWorkbookX;

        Single fWorkbookX, fWorkbookY;

        m_oVertexLocationConverter.GraphToWorkbook(
            new PointF(fGraphX, fGraphY), out fWorkbookX, out fWorkbookY);

        Assert.AreEqual(ExpectedWorkbookX, fWorkbookX);
        Assert.AreEqual(ExpectedWorkbookY, fWorkbookY);
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
        // Below minimum graph values.

        Single fGraphX = 100 - 1;
        Single fGraphY = 200 - 123;
        Single ExpectedWorkbookX = VertexLocationConverter.MinimumXYWorkbook;
        Single ExpectedWorkbookY = VertexLocationConverter.MaximumXYWorkbook;

        Single fWorkbookX, fWorkbookY;

        m_oVertexLocationConverter.GraphToWorkbook(
            new PointF(fGraphX, fGraphY), out fWorkbookX, out fWorkbookY);

        Assert.AreEqual(ExpectedWorkbookX, fWorkbookX);
        Assert.AreEqual(ExpectedWorkbookY, fWorkbookY);
    }

    //*************************************************************************
    //  Method: TestGraphToWorkbook6()
    //
    /// <summary>
    /// Tests the GraphToWorkbook() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphToWorkbook6()
    {
        // Above maximum graph values.

        Single fGraphX = 1100 + 1;
        Single fGraphY = 2200 + 123;
        Single ExpectedWorkbookX = VertexLocationConverter.MaximumXYWorkbook;
        Single ExpectedWorkbookY = VertexLocationConverter.MinimumXYWorkbook;

        Single fWorkbookX, fWorkbookY;

        m_oVertexLocationConverter.GraphToWorkbook(
            new PointF(fGraphX, fGraphY), out fWorkbookX, out fWorkbookY);

        Assert.AreEqual(ExpectedWorkbookX, fWorkbookX);
        Assert.AreEqual(ExpectedWorkbookY, fWorkbookY);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Graph rectangle.

    protected static readonly Rectangle GraphRectangle =
        Rectangle.FromLTRB(100, 200, 1100, 2200);


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected VertexLocationConverter m_oVertexLocationConverter;
}

}
