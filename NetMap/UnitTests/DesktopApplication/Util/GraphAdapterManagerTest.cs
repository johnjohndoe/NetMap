
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NetMap.DesktopApplication;
using Microsoft.NetMap.Adapters;

namespace Microsoft.NetMap.UnitTests
{
//*****************************************************************************
//  Class: GraphAdapterManagerTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="GraphAdapterManager" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class GraphAdapterManagerTest : Object
{
    //*************************************************************************
    //  Constructor: GraphAdapterManagerTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="GraphAdapterManagerTest" /> class.
    /// </summary>
    //*************************************************************************

    public GraphAdapterManagerTest()
    {
		// (Do nothing.)
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
		// (Do nothing.)
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
		// (Do nothing.)
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
		// (No constructor.)
    }

    //*************************************************************************
    //  Method: TestFileNameToGraphAdapter()
    //
    /// <summary>
    /// Tests the FileNameToGraphAdapter() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFileNameToGraphAdapter()
    {
		// SimpleGraphAdapter, lower case.

		Assert.IsInstanceOfType(
			GraphAdapterManager.FileNameToGraphAdapter("Simple.txt"),
			typeof(SimpleGraphAdapter)
			);
    }

    //*************************************************************************
    //  Method: TestFileNameToGraphAdapter2()
    //
    /// <summary>
    /// Tests the FileNameToGraphAdapter() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFileNameToGraphAdapter2()
    {
		// SimpleGraphAdapter, upper case.

		Assert.IsInstanceOfType(
			GraphAdapterManager.FileNameToGraphAdapter("SIMPLE.TXT"),
			typeof(SimpleGraphAdapter)
			);
    }

    //*************************************************************************
    //  Method: TestFileNameToGraphAdapter3()
    //
    /// <summary>
    /// Tests the FileNameToGraphAdapter() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFileNameToGraphAdapter3()
    {
		// SimpleGraphAdapter, with path.

		Assert.IsInstanceOfType(
			GraphAdapterManager.FileNameToGraphAdapter("C:\\Path\\Simple.txt"),
			typeof(SimpleGraphAdapter)
			);
    }

    //*************************************************************************
    //  Method: TestFileNameToGraphAdapter4()
    //
    /// <summary>
    /// Tests the FileNameToGraphAdapter() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFileNameToGraphAdapter4()
    {
		// PajekGraphAdapter.

		Assert.IsInstanceOfType(
			GraphAdapterManager.FileNameToGraphAdapter("Simple.net"),
			typeof(PajekGraphAdapter)
			);
    }

    //*************************************************************************
    //  Method: TestFileNameToGraphAdapter5()
    //
    /// <summary>
    /// Tests the FileNameToGraphAdapter() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFileNameToGraphAdapter5()
    {
		// Unrecognized extension.

		Assert.IsInstanceOfType(
			GraphAdapterManager.FileNameToGraphAdapter("Unknown.unk"),
			typeof(SimpleGraphAdapter)
			);
    }

    //*************************************************************************
    //  Method: TestFileNameToGraphAdapter6()
    //
    /// <summary>
    /// Tests the FileNameToGraphAdapter() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFileNameToGraphAdapter6()
    {
		// No extension.

		Assert.IsInstanceOfType(
			GraphAdapterManager.FileNameToGraphAdapter("NoExtension"),
			typeof(SimpleGraphAdapter)
			);
    }

    //*************************************************************************
    //  Method: TestFileNameToGraphAdapter7()
    //
    /// <summary>
    /// Tests the FileNameToGraphAdapter() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFileNameToGraphAdapter7()
    {
		// No extension.

		Assert.IsInstanceOfType(
			GraphAdapterManager.FileNameToGraphAdapter("NoExtension."),
			typeof(SimpleGraphAdapter)
			);
    }

    //*************************************************************************
    //  Method: TestFileDialogFilterIndexToGraphAdapter()
    //
    /// <summary>
    /// Tests the FileDialogFilterIndexToGraphAdapter() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFileDialogFilterIndexToGraphAdapter()
    {
		// SimpleGraphAdapter.

		Assert.IsInstanceOfType(
			GraphAdapterManager.FileDialogFilterIndexToGraphAdapter(1),
			typeof(SimpleGraphAdapter)
			);
    }

    //*************************************************************************
    //  Method: TestFileDialogFilterIndexToGraphAdapter2()
    //
    /// <summary>
    /// Tests the FileDialogFilterIndexToGraphAdapter() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFileDialogFilterIndexToGraphAdapter2()
    {
		// PajekGraphAdapter.

		Assert.IsInstanceOfType(
			GraphAdapterManager.FileDialogFilterIndexToGraphAdapter(2),
			typeof(PajekGraphAdapter)
			);
    }

    //*************************************************************************
    //  Method: TestFileDialogFilterIndexToGraphAdapterBad()
    //
    /// <summary>
    /// Tests the FileDialogFilterIndexToGraphAdapter() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(IndexOutOfRangeException) ) ]

    public void
    TestFileDialogFilterIndexToGraphAdapterBad()
    {
		// Index out of range.

		try
		{
			GraphAdapterManager.FileDialogFilterIndexToGraphAdapter(0);
		}
		catch (IndexOutOfRangeException oIndexOutOfRangeException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.DesktopApplication."
				+ "GraphAdapterManager.FileDialogFilterIndexToGraphAdapter:"
				+ " The specified index does not correspond to a graph"
				+ " adapter."
				,
				oIndexOutOfRangeException.Message
				);

			throw oIndexOutOfRangeException;
		}
    }

    //*************************************************************************
    //  Method: TestFileDialogFilterIndexToGraphAdapterBad2()
    //
    /// <summary>
    /// Tests the FileDialogFilterIndexToGraphAdapter() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(IndexOutOfRangeException) ) ]

    public void
    TestFileDialogFilterIndexToGraphAdapterBad2()
    {
		// Index out of range.

		try
		{
			GraphAdapterManager.FileDialogFilterIndexToGraphAdapter(3);
		}
		catch (IndexOutOfRangeException oIndexOutOfRangeException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.DesktopApplication."
				+ "GraphAdapterManager.FileDialogFilterIndexToGraphAdapter:"
				+ " The specified index does not correspond to a graph"
				+ " adapter."
				,
				oIndexOutOfRangeException.Message
				);

			throw oIndexOutOfRangeException;
		}
    }

    //*************************************************************************
    //  Method: TestTryGraphAdapterNameToGraphAdapter()
    //
    /// <summary>
    /// Tests the TryGraphAdapterNameToGraphAdapter() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGraphAdapterNameToGraphAdapter()
    {
		// SimpleGraphAdapter.

		IGraphAdapter oGraphAdapter;

		Assert.IsTrue(GraphAdapterManager.TryGraphAdapterNameToGraphAdapter(
			"Simple", out oGraphAdapter) );

		Assert.IsInstanceOfType( oGraphAdapter, typeof(SimpleGraphAdapter) );
    }

    //*************************************************************************
    //  Method: TestTryGraphAdapterNameToGraphAdapter2()
    //
    /// <summary>
    /// Tests the TryGraphAdapterNameToGraphAdapter() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGraphAdapterNameToGraphAdapter2()
    {
		// SimpleGraphAdapter, upper case.

		IGraphAdapter oGraphAdapter;

		Assert.IsTrue(GraphAdapterManager.TryGraphAdapterNameToGraphAdapter(
			"SIMPLE", out oGraphAdapter) );

		Assert.IsInstanceOfType( oGraphAdapter, typeof(SimpleGraphAdapter) );
    }

    //*************************************************************************
    //  Method: TestTryGraphAdapterNameToGraphAdapter3()
    //
    /// <summary>
    /// Tests the TryGraphAdapterNameToGraphAdapter() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGraphAdapterNameToGraphAdapter3()
    {
		// PajekGraphAdapter.

		IGraphAdapter oGraphAdapter;

		Assert.IsTrue(GraphAdapterManager.TryGraphAdapterNameToGraphAdapter(
			"Pajek", out oGraphAdapter) );

		Assert.IsInstanceOfType( oGraphAdapter, typeof(PajekGraphAdapter) );
    }

    //*************************************************************************
    //  Method: TestTryGraphAdapterNameToGraphAdapter4()
    //
    /// <summary>
    /// Tests the TryGraphAdapterNameToGraphAdapter() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGraphAdapterNameToGraphAdapter4()
    {
		// Unknown graph adapter.

		IGraphAdapter oGraphAdapter;

		Assert.IsFalse(GraphAdapterManager.TryGraphAdapterNameToGraphAdapter(
			"xyz", out oGraphAdapter) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
