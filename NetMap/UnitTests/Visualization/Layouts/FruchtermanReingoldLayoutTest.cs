
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NetMap.Core;
using Microsoft.NetMap.Visualization;
using Microsoft.NetMap.Tests;

namespace Microsoft.NetMap.UnitTests
{
//*****************************************************************************
//  Class: FruchtermanReingoldLayoutTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
///	cref="FruchtermanReingoldLayout" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class FruchtermanReingoldLayoutTest : Object
{
    //*************************************************************************
    //  Constructor: FruchtermanReingoldLayoutTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	///	cref="FruchtermanReingoldLayoutTest" /> class.
    /// </summary>
    //*************************************************************************

    public FruchtermanReingoldLayoutTest()
    {
        m_oFruchtermanReingoldLayout = null;
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
        m_oFruchtermanReingoldLayout = new FruchtermanReingoldLayout();
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
        m_oFruchtermanReingoldLayout = null;
    }

    //*************************************************************************
    //  Method: TestConstructor()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestConstructor()
    {
		Assert.AreEqual(6, m_oFruchtermanReingoldLayout.Margin);
    }

    //*************************************************************************
    //  Method: TestMargin()
    //
    /// <summary>
    /// Tests the Margin property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestMargin()
    {
		const Int32 Margin = 0;

		m_oFruchtermanReingoldLayout.Margin = Margin;

		Assert.AreEqual(Margin, m_oFruchtermanReingoldLayout.Margin);
    }

    //*************************************************************************
    //  Method: TestMargin2()
    //
    /// <summary>
    /// Tests the Margin property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestMargin2()
    {
		const Int32 Margin = 200;

		m_oFruchtermanReingoldLayout.Margin = Margin;

		Assert.AreEqual(Margin, m_oFruchtermanReingoldLayout.Margin);
    }

    //*************************************************************************
    //  Method: TestMarginBad()
    //
    /// <summary>
    /// Tests the Margin property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestMarginBad()
    {
		// Negative.

		try
		{
			m_oFruchtermanReingoldLayout.Margin = -1;
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Visualization.FruchtermanReingoldLayout."
				+ "Margin: Must be greater than or equal to zero."
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestLayOut()
    //
    /// <summary>
    /// Tests the LayOut method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLayOut()
    {
		const Int32 Vertices = 100;

		IGraph oGraph = new Graph();

		IVertex [] aoVertices = GraphUtil.AddVertices(oGraph, Vertices);

		GraphUtil.MakeGraphComplete(oGraph, aoVertices, false);

		// Initialize the vertex locations to impossible values.

		const Int32 ImpossibleCoordinate = Int32.MinValue;

		foreach (IVertex oVertex in aoVertices)
		{
			oVertex.Location = new Point(
				ImpossibleCoordinate, ImpossibleCoordinate);
		}

		const Int32 Width = 1000;
        const Int32 Height = 600;

		Rectangle oRectangle = new Rectangle(0, 0, Width, Height);

        LayoutContext oLayoutContext =
            new LayoutContext( oRectangle, new MockGraphDrawer() );

		m_oFruchtermanReingoldLayout.LayOutGraph(oGraph, oLayoutContext);

		foreach (IVertex oVertex in aoVertices)
		{
			PointF oLocation = oVertex.Location;

			Single fX = oLocation.X;

			Assert.AreNotEqual(fX, ImpossibleCoordinate);
			Assert.IsTrue(fX >= 0);
			Assert.IsTrue(fX <= Width);

			Single fY = oLocation.Y;

			Assert.AreNotEqual(fY, ImpossibleCoordinate);
			Assert.IsTrue(fY >= 0);
			Assert.IsTrue(fY <= Height);
		}
    }

    //*************************************************************************
    //  Method: TestLayOutGraphBad()
    //
    /// <summary>
    /// Tests the LayOutGraph method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestLayOutGraphBad()
    {
		// null graph.

		try
		{
			LayoutContext oLayoutContext =
				new LayoutContext( Rectangle.Empty, new MockGraphDrawer() );

			m_oFruchtermanReingoldLayout.LayOutGraph(null, oLayoutContext);

		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Visualization.FruchtermanReingoldLayout."
				+ "LayOutGraph: graph argument can't be null.\r\n"
				+ "Parameter name: graph"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestLayOutGraphBad2()
    //
    /// <summary>
    /// Tests the LayOutGraph method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestLayOutGraphBad2()
    {
		// null layoutContext.

		try
		{
			IGraph oGraph = new Graph();

			m_oFruchtermanReingoldLayout.LayOutGraph(oGraph, null);

		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Visualization.FruchtermanReingoldLayout."
				+ "LayOutGraph: layoutContext argument can't be null.\r\n"
				+ "Parameter name: layoutContext"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestTransformLayoutBad()
    //
    /// <summary>
    /// Tests the TransformLayout method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestTransformLayoutBad()
    {
		// null graph.

		try
		{
			LayoutContext oLayoutContext =
				new LayoutContext( Rectangle.Empty, new MockGraphDrawer() );

			m_oFruchtermanReingoldLayout.TransformLayout(
				null, oLayoutContext, oLayoutContext);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Visualization.FruchtermanReingoldLayout."
				+ "TransformLayout: graph argument can't be null.\r\n"
				+ "Parameter name: graph"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestTransformLayoutBad2()
    //
    /// <summary>
    /// Tests the TransformLayout method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestTransformLayoutBad2()
    {
		// null originalLayoutContext.

		try
		{
			IGraph oGraph = new Graph();

			LayoutContext oLayoutContext =
				new LayoutContext( Rectangle.Empty, new MockGraphDrawer() );

			m_oFruchtermanReingoldLayout.TransformLayout(
				oGraph, null, oLayoutContext);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Visualization.FruchtermanReingoldLayout."
				+ "TransformLayout: originalLayoutContext argument can't be"
				+ " null.\r\n"
				+ "Parameter name: originalLayoutContext"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestTransformLayoutBad3()
    //
    /// <summary>
    /// Tests the TransformLayout method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestTransformLayoutBad3()
    {
		// null newLayoutContext.

		try
		{
			IGraph oGraph = new Graph();

			LayoutContext oLayoutContext =
				new LayoutContext( Rectangle.Empty, new MockGraphDrawer() );

			m_oFruchtermanReingoldLayout.TransformLayout(
				oGraph, oLayoutContext, null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Visualization.FruchtermanReingoldLayout."
				+ "TransformLayout: newLayoutContext argument can't be"
				+ " null.\r\n"
				+ "Parameter name: newLayoutContext"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestOnVertexMoveBad()
    //
    /// <summary>
    /// Tests the OnVertexMove method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestOnVertexMoveBad()
    {
		// null vertex.

		try
		{
			m_oFruchtermanReingoldLayout.OnVertexMove(null);

		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Visualization.FruchtermanReingoldLayout."
				+ "OnVertexMove: vertex argument can't be null.\r\n"
				+ "Parameter name: vertex"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected ILayout m_oFruchtermanReingoldLayout;
}

}
