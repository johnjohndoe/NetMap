
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: VertexFactoryTest
//
/// <summary>
/// This is Visual Studio test fixture for the <see cref="VertexFactory" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class VertexFactoryTest : Object
{
    //*************************************************************************
    //  Constructor: VertexFactoryTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexFactoryTest" />
	/// class.
    /// </summary>
    //*************************************************************************

    public VertexFactoryTest()
    {
        m_oVertexFactory = null;
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
        m_oVertexFactory = new VertexFactory();
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
        m_oVertexFactory = null;
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
    //  Method: TestCreateVertex()
    //
    /// <summary>
    /// Tests the CreateVertex method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCreateVertex()
    {
		const Int32 Vertices = 1000;

		Int32 iFirstVertexID = Int32.MinValue;

		for (Int32 i = 0; i < Vertices; i++)
		{
			IVertex oVertex = m_oVertexFactory.CreateVertex();

			Assert.IsNotNull(oVertex);
			Assert.IsTrue(oVertex is Vertex);

			Assert.IsNotNull(oVertex.AdjacentVertices);
			Assert.AreEqual(0, oVertex.AdjacentVertices.Length);

			Assert.AreEqual(0, oVertex.Degree);

			if (i == 0)
			{
				iFirstVertexID = oVertex.ID;
			}
			else
			{
				// Make sure the assigned IDs are consecutive.

				Assert.AreEqual(iFirstVertexID + i, oVertex.ID);
			}

			Assert.IsNotNull(oVertex.IncidentEdges);
			Assert.AreEqual(0, oVertex.IncidentEdges.Length);

			Assert.IsNotNull(oVertex.IncomingEdges);
			Assert.AreEqual(0, oVertex.IncomingEdges.Length);

			Assert.AreEqual(PointF.Empty, oVertex.Location);

			Assert.IsNull(oVertex.Name);

			Assert.IsNotNull(oVertex.OutgoingEdges);
			Assert.AreEqual(0, oVertex.OutgoingEdges.Length);

			Assert.IsNull(oVertex.ParentGraph);

			Assert.IsNotNull(oVertex.PredecessorVertices);
			Assert.AreEqual(0, oVertex.PredecessorVertices.Length);

			Assert.IsNotNull(oVertex.SuccessorVertices);
			Assert.AreEqual(0, oVertex.SuccessorVertices.Length);

			Assert.IsNull(oVertex.Tag);
		}
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected VertexFactory m_oVertexFactory;
}

}
