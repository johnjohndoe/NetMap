
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.ExcelTemplate;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: ColorConverter2Test
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="ColorConverter2" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class ColorConverter2Test : Object
{
    //*************************************************************************
    //  Constructor: ColorConverter2Test()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ColorConverter2Test" />
    /// class.
    /// </summary>
    //*************************************************************************

    public ColorConverter2Test()
    {
        m_oColorConverter2 = null;
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
        m_oColorConverter2 = new ColorConverter2();
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
        m_oColorConverter2 = null;
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
        Color eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "alice blue",
            "aliceblue",
            "aLice blUe",
            "ALICEBLUE",
            } )
        {
            Assert.IsTrue( m_oColorConverter2.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(Color.AliceBlue, eValueGraph);
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
        Color eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "blue",
            "blue",
            "blUe",
            "BLUE",
            } )
        {
            Assert.IsTrue( m_oColorConverter2.TryWorkbookToGraph(
                sValueWorkbook, out eValueGraph) );

            Assert.AreEqual(Color.Blue, eValueGraph);
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
        Color eValueGraph;

        foreach (String sValueWorkbook in new String [] {
            "",
            "Xred",
            "x",
            "greenX",
            } )
        {
            Assert.IsFalse( m_oColorConverter2.TryWorkbookToGraph(
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
        // Colors created from Color properties, like Color.AliceBlue.

        Assert.AreEqual( "AliceBlue",
            m_oColorConverter2.GraphToWorkbook(Color.AliceBlue) );

        Assert.AreEqual( "Blue",
            m_oColorConverter2.GraphToWorkbook(Color.Blue) );

        Assert.AreEqual( "Orange",
            m_oColorConverter2.GraphToWorkbook(Color.Orange) );
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
        // Colors created from Color.FromArgb(), not treated specially by
        // GraphToWorkbook().

        Assert.AreEqual( "1, 2, 3", m_oColorConverter2.GraphToWorkbook(
            Color.FromArgb(1, 2, 3) ) );

        Assert.AreEqual( "254, 253, 1", m_oColorConverter2.GraphToWorkbook(
            Color.FromArgb(254, 253, 1) ) );
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
        // Colors created from Color.FromArgb(), treated specially by
        // GraphToWorkbook().

        Assert.AreEqual( "Black", m_oColorConverter2.GraphToWorkbook(
            Color.FromArgb(0, 0, 0) ) );

        Assert.AreEqual( "White", m_oColorConverter2.GraphToWorkbook(
            Color.FromArgb(255, 255, 255) ) );

        Assert.AreEqual( "Blue", m_oColorConverter2.GraphToWorkbook(
            Color.FromArgb(0, 0, 255) ) );

        Assert.AreEqual( "Lime", m_oColorConverter2.GraphToWorkbook(
            Color.FromArgb(0, 255, 0) ) );

        Assert.AreEqual( "Red", m_oColorConverter2.GraphToWorkbook(
            Color.FromArgb(255, 0, 0) ) );

        Assert.AreEqual( "Cyan", m_oColorConverter2.GraphToWorkbook(
            Color.FromArgb(0, 255, 255) ) );

        Assert.AreEqual( "Yellow", m_oColorConverter2.GraphToWorkbook(
            Color.FromArgb(255, 255, 0) ) );

        Assert.AreEqual( "Magenta", m_oColorConverter2.GraphToWorkbook(
            Color.FromArgb(255, 0, 255) ) );

        Assert.AreEqual( "Orange", m_oColorConverter2.GraphToWorkbook(
            Color.FromArgb(255, 165, 0) ) );

        Assert.AreEqual( "Green", m_oColorConverter2.GraphToWorkbook(
            Color.FromArgb(0, 128, 0) ) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected ColorConverter2 m_oColorConverter2;
}

}
