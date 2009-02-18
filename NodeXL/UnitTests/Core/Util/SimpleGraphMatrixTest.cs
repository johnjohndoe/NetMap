
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Common;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: SimpleGraphMatrixTest
//
/// <summary>
/// This is Visual Studio test fixture for the <see cref="SimpleGraphMatrix" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class SimpleGraphMatrixTest : Object
{
    //*************************************************************************
    //  Constructor: SimpleGraphMatrixTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleGraphMatrixTest" />
	/// class.
    /// </summary>
    //*************************************************************************

    public SimpleGraphMatrixTest()
    {
		m_oSimpleGraphMatrix = null;
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
		m_oSimpleGraphMatrix = null;
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
		m_oSimpleGraphMatrix = null;
    }

    //*************************************************************************
    //  Method: TestAij()
    //
    /// <summary>
    /// Tests the Aij() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAij()
    {
		// Variety of edges.

		IGraph oGraph = new Graph();

		IVertex[] aoVertices = TestGraphUtil.AddVertices(oGraph, 3);

		IEdgeCollection oEdges = oGraph.Edges;

		oEdges.Add( aoVertices[0], aoVertices[1] );

		oEdges.Add( aoVertices[1], aoVertices[0] );

		// Duplicates.

		oEdges.Add( aoVertices[1], aoVertices[2] );
		oEdges.Add( aoVertices[1], aoVertices[2] );

		oEdges.Add( aoVertices[2], aoVertices[1] );

		m_oSimpleGraphMatrix = new SimpleGraphMatrix(oGraph);

		Assert.IsFalse(
			m_oSimpleGraphMatrix.Aij(aoVertices[0], aoVertices[0] ) );

		Assert.IsTrue(
			m_oSimpleGraphMatrix.Aij(aoVertices[0], aoVertices[1] ) );

		Assert.IsFalse(
			m_oSimpleGraphMatrix.Aij(aoVertices[0], aoVertices[2] ) );

		Assert.IsTrue(
			m_oSimpleGraphMatrix.Aij(aoVertices[1], aoVertices[0] ) );

		Assert.IsFalse(
			m_oSimpleGraphMatrix.Aij(aoVertices[1], aoVertices[1] ) );

		Assert.IsTrue(
			m_oSimpleGraphMatrix.Aij(aoVertices[1], aoVertices[2] ) );

		Assert.IsFalse(
			m_oSimpleGraphMatrix.Aij(aoVertices[2], aoVertices[0] ) );

		Assert.IsTrue(
			m_oSimpleGraphMatrix.Aij(aoVertices[2], aoVertices[1] ) );

		Assert.IsFalse(
			m_oSimpleGraphMatrix.Aij(aoVertices[2], aoVertices[2] ) );
    }

    //*************************************************************************
    //  Method: TestAij2()
    //
    /// <summary>
    /// Tests the Aij() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAij2()
    {
		// No edges.

		IGraph oGraph = new Graph();

		IVertex[] aoVertices = TestGraphUtil.AddVertices(oGraph, 3);

		m_oSimpleGraphMatrix = new SimpleGraphMatrix(oGraph);

		Assert.IsFalse(
			m_oSimpleGraphMatrix.Aij(aoVertices[0], aoVertices[0] ) );

		Assert.IsFalse(
			m_oSimpleGraphMatrix.Aij(aoVertices[0], aoVertices[1] ) );

		Assert.IsFalse(
			m_oSimpleGraphMatrix.Aij(aoVertices[0], aoVertices[2] ) );

		Assert.IsFalse(
			m_oSimpleGraphMatrix.Aij(aoVertices[1], aoVertices[0] ) );

		Assert.IsFalse(
			m_oSimpleGraphMatrix.Aij(aoVertices[1], aoVertices[1] ) );

		Assert.IsFalse(
			m_oSimpleGraphMatrix.Aij(aoVertices[1], aoVertices[2] ) );

		Assert.IsFalse(
			m_oSimpleGraphMatrix.Aij(aoVertices[2], aoVertices[0] ) );

		Assert.IsFalse(
			m_oSimpleGraphMatrix.Aij(aoVertices[2], aoVertices[1] ) );

		Assert.IsFalse(
			m_oSimpleGraphMatrix.Aij(aoVertices[2], aoVertices[2] ) );
    }

    //*************************************************************************
    //  Method: TestAij3()
    //
    /// <summary>
    /// Tests the Aij() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAij3()
    {
		// All edges.

		IGraph oGraph = new Graph();

		IVertex[] aoVertices = TestGraphUtil.AddVertices(oGraph, 3);

		IEdgeCollection oEdges = oGraph.Edges;

		oEdges.Add( aoVertices[0], aoVertices[0] );
		oEdges.Add( aoVertices[0], aoVertices[1] );
		oEdges.Add( aoVertices[0], aoVertices[2] );
		oEdges.Add( aoVertices[1], aoVertices[0] );
		oEdges.Add( aoVertices[1], aoVertices[1] );
		oEdges.Add( aoVertices[1], aoVertices[2] );
		oEdges.Add( aoVertices[2], aoVertices[0] );
		oEdges.Add( aoVertices[2], aoVertices[1] );
		oEdges.Add( aoVertices[2], aoVertices[2] );

		m_oSimpleGraphMatrix = new SimpleGraphMatrix(oGraph);

		Assert.IsTrue(
			m_oSimpleGraphMatrix.Aij(aoVertices[0], aoVertices[0] ) );

		Assert.IsTrue(
			m_oSimpleGraphMatrix.Aij(aoVertices[0], aoVertices[1] ) );

		Assert.IsTrue(
			m_oSimpleGraphMatrix.Aij(aoVertices[0], aoVertices[2] ) );

		Assert.IsTrue(
			m_oSimpleGraphMatrix.Aij(aoVertices[1], aoVertices[0] ) );

		Assert.IsTrue(
			m_oSimpleGraphMatrix.Aij(aoVertices[1], aoVertices[1] ) );

		Assert.IsTrue(
			m_oSimpleGraphMatrix.Aij(aoVertices[1], aoVertices[2] ) );

		Assert.IsTrue(
			m_oSimpleGraphMatrix.Aij(aoVertices[2], aoVertices[0] ) );

		Assert.IsTrue(
			m_oSimpleGraphMatrix.Aij(aoVertices[2], aoVertices[1] ) );

		Assert.IsTrue(
			m_oSimpleGraphMatrix.Aij(aoVertices[2], aoVertices[2] ) );
    }

    //*************************************************************************
    //  Method: TestAij4()
    //
    /// <summary>
    /// Tests the Aij() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAij4()
    {
		// Random edges.

		IGraph oGraph = new Graph();
		const Int32 Vertices = 100;

		IVertex[] aoVertices = TestGraphUtil.AddVertices(oGraph, Vertices);
		IEdgeCollection oEdges = oGraph.Edges;
		Random oRandom = new Random(0);

		for (Int32 i = 0; i < Vertices; i++)
		{
			for (Int32 j = 0; j < Vertices; j++)
			{
				if (oRandom.Next() % 2 == 1)
				{
					oEdges.Add( aoVertices[i], aoVertices[j] );
				}
			}
		}

		m_oSimpleGraphMatrix = new SimpleGraphMatrix(oGraph);

		for (Int32 i = 0; i < Vertices; i++)
		{
			for (Int32 j = 0; j < Vertices; j++)
			{
				IVertex oVertex1 = aoVertices[i];
				IVertex oVertex2 = aoVertices[j];

				if (oVertex1.GetConnectingEdges(oVertex2).Length > 0)
				{
					Assert.IsTrue(
						m_oSimpleGraphMatrix.Aij(oVertex1, oVertex2) );

					Assert.IsTrue(
						m_oSimpleGraphMatrix.Aij(oVertex2, oVertex1) );
				}
				else
				{
					Assert.IsFalse(
						m_oSimpleGraphMatrix.Aij(oVertex1, oVertex2) );

					Assert.IsFalse(
						m_oSimpleGraphMatrix.Aij(oVertex2, oVertex1) );
				}
			}
		}
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object being tested.

    protected SimpleGraphMatrix m_oSimpleGraphMatrix;
}

}
