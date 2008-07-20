
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NetMap.Core;
using Microsoft.NetMap.Visualization;

namespace Microsoft.NetMap.UnitTests
{
//*****************************************************************************
//  Class: GraphMouseEventArgsTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="GraphMouseEventArgs" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class GraphMouseEventArgsTest : Object
{
    //*************************************************************************
    //  Constructor: GraphMouseEventArgsTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="GraphMouseEventArgsTest" /> class.
    /// </summary>
    //*************************************************************************

    public GraphMouseEventArgsTest()
    {
        m_oGraphMouseEventArgs = null;
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
		MouseEventArgs oMouseEventArgs = CreateMouseEventArgs();

        m_oGraphMouseEventArgs =
			new GraphMouseEventArgs(oMouseEventArgs, null);
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
        m_oGraphMouseEventArgs = null;
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
		Assert.AreEqual(Button, m_oGraphMouseEventArgs.Button);
		Assert.AreEqual(Clicks, m_oGraphMouseEventArgs.Clicks);
		Assert.AreEqual(X, m_oGraphMouseEventArgs.X);
		Assert.AreEqual(Y, m_oGraphMouseEventArgs.Y);
		Assert.AreEqual(Delta, m_oGraphMouseEventArgs.Delta);
		Assert.IsNull(m_oGraphMouseEventArgs.Vertex);
    }

    //*************************************************************************
    //  Method: TestConstructor2()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestConstructor2()
    {
		MouseEventArgs oMouseEventArgs = CreateMouseEventArgs();

		IVertex oVertex = ( new VertexFactory() ).CreateVertex();

        m_oGraphMouseEventArgs =
			new GraphMouseEventArgs(oMouseEventArgs, oVertex);

		Assert.AreEqual(Button, m_oGraphMouseEventArgs.Button);
		Assert.AreEqual(Clicks, m_oGraphMouseEventArgs.Clicks);
		Assert.AreEqual(X, m_oGraphMouseEventArgs.X);
		Assert.AreEqual(Y, m_oGraphMouseEventArgs.Y);
		Assert.AreEqual(Delta, m_oGraphMouseEventArgs.Delta);
		Assert.AreEqual(oVertex, m_oGraphMouseEventArgs.Vertex);
    }

    //*************************************************************************
    //  Method: CreateMouseEventArgs()
    //
    /// <summary>
    /// Creates and returns a new MouseEventArgs object.
    /// </summary>
    //*************************************************************************

    protected MouseEventArgs
    CreateMouseEventArgs()
    {
		return ( new MouseEventArgs(Button, Clicks, X, Y, Delta) );
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	protected const MouseButtons Button = MouseButtons.Left;

	protected const Int32 Clicks = 2;

	protected const Int32 X = 123;

	protected const Int32 Y = 456;

	protected const Int32 Delta = 31;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected GraphMouseEventArgs m_oGraphMouseEventArgs;
}

}
