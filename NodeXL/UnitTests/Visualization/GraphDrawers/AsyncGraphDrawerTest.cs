
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: AsyncGraphDrawerTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="AsyncGraphDrawer" /> class.
/// </summary>
///
/// <remarks>
/// Due to the difficulty (if not impossibility) of testing asynchronous
/// methods via unit tests, the asynchronous methods of AsyncGraphDrawer are
/// tested via a Windows Forms test app.  The unit tests in this file mostly
/// test that bad method arguments are properly handled.
/// </remarks>
//*****************************************************************************

[TestClassAttribute]

public class AsyncGraphDrawerTest : Object
{
    //*************************************************************************
    //  Constructor: AsyncGraphDrawerTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncGraphDrawerTest" />
	/// class.
    /// </summary>
    //*************************************************************************

    public AsyncGraphDrawerTest()
    {
        m_oAsyncGraphDrawer = null;
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
        m_oAsyncGraphDrawer = new AsyncGraphDrawer();
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
        m_oAsyncGraphDrawer = null;
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
		Assert.AreEqual(SystemColors.Window, m_oAsyncGraphDrawer.BackColor);

		Assert.IsInstanceOfType( m_oAsyncGraphDrawer.EdgeDrawer,
			typeof(EdgeDrawer) );

		Assert.IsInstanceOfType( m_oAsyncGraphDrawer.Graph, typeof(Graph) );

		Assert.AreEqual(0, m_oAsyncGraphDrawer.Graph.Vertices.Count);

		Assert.AreEqual(0, m_oAsyncGraphDrawer.Graph.Edges.Count);

		Assert.IsInstanceOfType( m_oAsyncGraphDrawer.Layout,
			typeof(FruchtermanReingoldLayout) );

		Assert.IsInstanceOfType( m_oAsyncGraphDrawer.VertexDrawer,
			typeof(VertexDrawer) );

		Assert.IsFalse(m_oAsyncGraphDrawer.IsBusy);
    }

    //*************************************************************************
    //  Method: TestLayoutBad()
    //
    /// <summary>
    /// Tests the Layout property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestLayoutBad()
    {
		// null value.

		try
		{
			m_oAsyncGraphDrawer.Layout = null;
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				m_oAsyncGraphDrawer.GetType().FullName
				+ ".Layout: Can't be null."
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestLayoutBad2()
    //
    /// <summary>
    /// Tests the Layout property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ApplicationException) ) ]

    public void
    TestLayoutBad2()
    {
		// Layout that doesn't support IAsyncLayout.

		try
		{
			m_oAsyncGraphDrawer.Layout = new MockLayout();
		}
		catch (ApplicationException oApplicationException)
		{
			Assert.AreEqual(

				m_oAsyncGraphDrawer.GetType().FullName
				+ ".Layout: The Layout must implement the IAsyncLayout"
                + " interface."
				,
				oApplicationException.Message
				);

			throw oApplicationException;
		}
    }

    //*************************************************************************
    //  Method: TestDrawAsyncBad()
    //
    /// <summary>
    /// Tests the DrawAsync(Bitmap) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestDrawAsyncBad()
    {
		// null bitmap.

		try
		{
			m_oAsyncGraphDrawer.DrawAsync(null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oAsyncGraphDrawer.GetType().FullName
				+ ".DrawAsync: bitmap argument can't be null.\r\n"
				+ "Parameter name: bitmap"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestDrawAsync2_Bad()
    //
    /// <summary>
    /// Tests the DrawAsync(Bitmap, Rectangle) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestDrawAsync2_Bad()
    {
		// null bitmap.

		try
		{
			m_oAsyncGraphDrawer.DrawAsync( (Bitmap)null, Rectangle.Empty );
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oAsyncGraphDrawer.GetType().FullName
				+ ".DrawAsync: bitmap argument can't be null.\r\n"
				+ "Parameter name: bitmap"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestDrawAsync3_Bad()
    //
    /// <summary>
    /// Tests the DrawAsync(Graphics, Rectangle) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestDrawAsync3_Bad()
    {
		// null Graphics.

		try
		{
			m_oAsyncGraphDrawer.DrawAsync( (Graphics)null, Rectangle.Empty );
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				m_oAsyncGraphDrawer.GetType().FullName
				+ ".DrawAsync: graphics argument can't be null.\r\n"
				+ "Parameter name: graphics"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestDrawAsyncCancel()
    //
    /// <summary>
    /// Tests the DrawAsyncCancel() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestDrawAsyncCancel()
    {
		// Make sure the method does nothing when there is no drawing operation
		// in progress.

		m_oAsyncGraphDrawer.DrawAsyncCancel();
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected AsyncGraphDrawer m_oAsyncGraphDrawer;
}

}
