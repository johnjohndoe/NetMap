
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NetMap.Core;
using Microsoft.NetMap.Tests;

namespace Microsoft.NetMap.UnitTests
{
//*****************************************************************************
//  Class: GraphFactoryTest
//
/// <summary>
/// This is Visual Studio test fixture for the <see cref="GraphFactory" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class GraphFactoryTest : Object
{
    //*************************************************************************
    //  Constructor: GraphFactoryTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphFactoryTest" />
	/// class.
    /// </summary>
    //*************************************************************************

    public GraphFactoryTest()
    {
        m_oGraphFactory = null;
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
        m_oGraphFactory = new GraphFactory();
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
        m_oGraphFactory = null;
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
		// (Do nothing else.)
    }

    //*************************************************************************
    //  Method: TestCreateGraph()
    //
    /// <summary>
    /// Tests the CreateGraph(GraphDirectedness, GraphRestrictions) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCreateGraph()
    {
		foreach (GraphDirectedness eDirectedness in
			GraphUtil.AllGraphDirectedness)

		foreach (GraphRestrictions eRestrictions in
			GraphUtil.AllGraphRestrictions)

		{
		IGraph oGraph = m_oGraphFactory.CreateGraph(
			eDirectedness, eRestrictions);

		CheckGraph(oGraph, eDirectedness, eRestrictions);
		}
    }

    //*************************************************************************
    //  Method: TestCreateGraphBad()
    //
    /// <summary>
    /// Tests the CreateGraph(GraphDirectedness) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestCreateGraphBad()
    {
		// Bad GraphDirectedness.

		try
		{
			IGraph oGraph = m_oGraphFactory.CreateGraph(
				(GraphDirectedness)9999, GraphRestrictions.None);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Core."
				+ "GraphFactory.CreateGraph: Must be a member of the"
				+ " GraphDirectedness enumeration.\r\n"
				+ "Parameter name: directedness"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestCreateGraphBad2()
    //
    /// <summary>
    /// Tests the CreateGraph(GraphDirectedness, GraphRestrictions) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestCreateGraphBad2()
    {
		// Bad GraphRestrictions.

		try
		{
			IGraph oGraph = m_oGraphFactory.CreateGraph(
				GraphDirectedness.Mixed, (GraphRestrictions)9999);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Core."
				+ "GraphFactory.CreateGraph: Must be an ORed combination of"
				+ " the flags in the GraphRestrictions enumeration.\r\n"
				+ "Parameter name: restrictions"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: CheckGraph()
    //
    /// <summary>
    /// Checks a graph created with CreateGraph().
    /// </summary>
	///
	/// <paramref name="oGraph">
	/// The graph to check.
	/// </paramref>
	///
	/// <paramref name="eDirectedness">
	/// Expected directedness of the graph.
	/// </paramref>
	///
	/// <paramref name="eRestrictions">
	/// Expected restrictions imposed by the graph.
	/// </paramref>
    //*************************************************************************

    protected void
    CheckGraph
	(
		IGraph oGraph,
		GraphDirectedness eDirectedness,
		GraphRestrictions eRestrictions
	)
    {
		Assert.IsNotNull(oGraph);
		Assert.IsTrue(oGraph is Graph);

		Assert.AreEqual(eDirectedness, oGraph.Directedness);

		Assert.AreEqual(eRestrictions, oGraph.Restrictions);

		Assert.IsFalse(oGraph.PerformExtraValidations);

		Assert.IsNotNull(oGraph.Edges);
		Assert.IsTrue(oGraph.Edges is EdgeCollection);

		// Can't check the ID, because it is unknown.

		// Assert.AreEqual(xx, oGraph.ID);

		Assert.IsNull(oGraph.Name);

		Assert.IsNull(oGraph.Tag);

		Assert.IsNotNull(oGraph.Vertices);
		Assert.IsTrue(oGraph.Vertices is VertexCollection);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected GraphFactory m_oGraphFactory;
}

}
