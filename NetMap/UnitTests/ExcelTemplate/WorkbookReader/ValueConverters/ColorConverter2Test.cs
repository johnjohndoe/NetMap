
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NetMap.ExcelTemplate;
using Microsoft.NetMap.Visualization;

namespace Microsoft.NetMap.UnitTests
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
		Assert.AreEqual( "Alice Blue",
			m_oColorConverter2.GraphToWorkbook(Color.AliceBlue) );

		Assert.AreEqual( "Blue",
			m_oColorConverter2.GraphToWorkbook(Color.Blue) );

		Assert.AreEqual( "Orange",
			m_oColorConverter2.GraphToWorkbook(Color.Orange) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected ColorConverter2 m_oColorConverter2;
}

}
