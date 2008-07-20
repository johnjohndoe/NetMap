
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NetMap.Core;
using Microsoft.NetMap.Adapters;

namespace Microsoft.NetMap.UnitTests
{
//*****************************************************************************
//  Class: SimpleGraphAdapterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="SimpleGraphAdapter" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class SimpleGraphAdapterTest : Object
{
    //*************************************************************************
    //  Constructor: SimpleGraphAdapterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleGraphAdapterTest" />
	/// class.
    /// </summary>
    //*************************************************************************

    public SimpleGraphAdapterTest()
    {
        m_oGraphAdapter = null;
		m_sTempFileName = null;
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
        m_oGraphAdapter = new SimpleGraphAdapter();

		m_sTempFileName = Path.GetTempFileName();
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
        m_oGraphAdapter = null;

		if ( File.Exists(m_sTempFileName) )
		{
			File.Delete(m_sTempFileName);
		}
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
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: TestLoadGraph()
    //
    /// <summary>
    /// Tests the LoadGraph(String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph()
    {
		const String FileContents =
			"Vertex1\tVertex2\r\n";

		const String Vertex1Name = "Vertex1";
		const String Vertex2Name = "Vertex2";

		using ( StreamWriter oStreamWriter = new StreamWriter(m_sTempFileName) )
		{
			oStreamWriter.Write(FileContents);
		}

		IGraph oGraph = m_oGraphAdapter.LoadGraph(m_sTempFileName);

		Assert.IsInstanceOfType( oGraph, typeof(Graph) );

		IVertexCollection oVertices = oGraph.Vertices;

		Assert.AreEqual(2, oVertices.Count);

		Assert.IsTrue( oVertices.Contains(Vertex1Name) );
		Assert.IsTrue( oVertices.Contains(Vertex2Name) );

		IEdgeCollection oEdges = oGraph.Edges;

		Assert.AreEqual(1, oEdges.Count);

		foreach (IEdge oEdge in oEdges)
		{
			Assert.AreEqual(Vertex1Name, oEdge.Vertices[0].Name);
			Assert.AreEqual(Vertex2Name, oEdge.Vertices[1].Name);
		}
    }

    //*************************************************************************
    //  Method: TestLoadGraphBad()
    //
    /// <summary>
    /// Tests the LoadGraph(String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestLoadGraphBad()
    {
		// null filename.

		try
		{
            String sFileName = null;

			m_oGraphAdapter.LoadGraph(sFileName);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Adapters."
				+ "SimpleGraphAdapter.LoadGraph: filename argument can't be"
				+ " null.\r\n"
				+ "Parameter name: filename"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestLoadGraphBad2()
    //
    /// <summary>
    /// Tests the LoadGraph(String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestLoadGraphBad2()
    {
		// Empty filename.

		try
		{
            String sFileName = String.Empty;

			m_oGraphAdapter.LoadGraph(sFileName);
		}
		catch (ArgumentNullException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Adapters."
				+ "SimpleGraphAdapter.LoadGraph: filename argument must have a"
				+ " length greater than zero.\r\n"
				+ "Parameter name: filename"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestLoadGraphBad3()
    //
    /// <summary>
    /// Tests the LoadGraph(String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(DirectoryNotFoundException) ) ]

    public void
    TestLoadGraphBad3()
    {
		// Non-existent filename.

		try
		{
            String sFileName = "X:\\abc\\def\\ghi.txt";

			m_oGraphAdapter.LoadGraph(sFileName);
		}
		catch (DirectoryNotFoundException oDirectoryNotFoundException)
		{
			Assert.IsTrue(oDirectoryNotFoundException.Message.Contains(
				"Could not find a part of the path"
				) );

			throw oDirectoryNotFoundException;
		}
    }

    //*************************************************************************
    //  Method: TestLoadGraph2_()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph2_()
    {
		const String StreamContents =
			"Vertex1\tVertex2\r\n";

		const String Vertex1Name = "Vertex1";
		const String Vertex2Name = "Vertex2";

		StringStream oStream = new StringStream(StreamContents);

		IGraph oGraph =
			m_oGraphAdapter.LoadGraph(new GraphFactory(), oStream);

		Assert.IsInstanceOfType( oGraph, typeof(Graph) );

		IVertexCollection oVertices = oGraph.Vertices;

		Assert.AreEqual(2, oVertices.Count);

		Assert.IsTrue( oVertices.Contains(Vertex1Name) );
		Assert.IsTrue( oVertices.Contains(Vertex2Name) );

		IEdgeCollection oEdges = oGraph.Edges;

		Assert.AreEqual(1, oEdges.Count);

		foreach (IEdge oEdge in oEdges)
		{
			Assert.AreEqual(Vertex1Name, oEdge.Vertices[0].Name);
			Assert.AreEqual(Vertex2Name, oEdge.Vertices[1].Name);
		}
    }

    //*************************************************************************
    //  Method: TestLoadGraph2_2()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph2_2()
    {
		// One edge.

		TestLoadGraphSingleEdge(
			"Vertex1\tVertex2\r\n",
			"Vertex1",
			"Vertex2"
			);
    }

    //*************************************************************************
    //  Method: TestLoadGraph2_3()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph2_3()
    {
		// One edge, no line terminator.

		TestLoadGraphSingleEdge(
			"Vertex1\tVertex2",
			"Vertex1",
			"Vertex2"
			);
    }

    //*************************************************************************
    //  Method: TestLoadGraph2_4()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph2_4()
    {
		// One edge, line feed only.

		TestLoadGraphSingleEdge(
			"Vertex1\tVertex2\n",
			"Vertex1",
			"Vertex2"
			);
    }

    //*************************************************************************
    //  Method: TestLoadGraph2_5()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph2_5()
    {
		// One edge, embedded spaces.

		TestLoadGraphSingleEdge(
			" Vertex 1 \t Vertex 2 \r\n",
			" Vertex 1 ",
			" Vertex 2 "
			);
    }

    //*************************************************************************
    //  Method: TestLoadGraph2_6()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph2_6()
    {
		// One edge, spaces for vertex name.

		TestLoadGraphSingleEdge(
			"A\t  \r\n",
			"A",
			"  "
			);
    }

    //*************************************************************************
    //  Method: TestLoadGraph2_7()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph2_7()
    {
		// One edge, spaces for vertex name.

		TestLoadGraphSingleEdge(
			" \tB\r\n",
			" ",
			"B"
			);
    }

    //*************************************************************************
    //  Method: TestLoadGraph2_8()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph2_8()
    {
		// N edges.

		const String StreamContents =
			"Edge1Vertex1\tEdge1Vertex2\r\n"
			+ "Edge2Vertex1\tEdge2Vertex2\r\n"
			+ "Edge3Vertex1\tEdge3Vertex2\r\n"
			;

		StringStream oStream = new StringStream(StreamContents);

		IGraph oGraph = m_oGraphAdapter.LoadGraph(new GraphFactory(), oStream);

		IVertexCollection oVertices = oGraph.Vertices;

		Assert.AreEqual(6, oVertices.Count);

		Assert.IsTrue( oVertices.Contains("Edge1Vertex1") );
		Assert.IsTrue( oVertices.Contains("Edge1Vertex2") );
		Assert.IsTrue( oVertices.Contains("Edge2Vertex1") );
		Assert.IsTrue( oVertices.Contains("Edge2Vertex2") );
		Assert.IsTrue( oVertices.Contains("Edge3Vertex1") );
		Assert.IsTrue( oVertices.Contains("Edge3Vertex2") );

		IEdgeCollection oEdges = oGraph.Edges;

		Assert.AreEqual(3, oEdges.Count);

		foreach (IEdge oEdge in oEdges)
		{
			String sVertex1Name = oEdge.Vertices[0].Name;
			String sVertex2Name = oEdge.Vertices[1].Name;

			Assert.IsTrue( sVertex1Name.StartsWith("Edge") );

			Int32 iEdgeNumber = Int32.Parse( sVertex1Name.Substring(4, 1) );

			Assert.AreEqual("Edge" + iEdgeNumber.ToString() + "Vertex1",
				sVertex1Name);

			Assert.AreEqual("Edge" + iEdgeNumber.ToString() + "Vertex2",
				sVertex2Name);
		}
    }

    //*************************************************************************
    //  Method: TestLoadGraph2_9()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph2_9()
    {
		// N edges, repeated vertices.

		const String StreamContents =
			"Edge1Vertex1\tEdge1Vertex2\r\n"
			+ "Edge1Vertex1\tEdge1Vertex2\r\n"
			+ "Edge1Vertex1\tEdge1Vertex2\r\n"
			;

		StringStream oStream = new StringStream(StreamContents);

		IGraph oGraph = m_oGraphAdapter.LoadGraph(new GraphFactory(), oStream);

		IVertexCollection oVertices = oGraph.Vertices;

		Assert.AreEqual(2, oVertices.Count);

		Assert.IsTrue( oVertices.Contains("Edge1Vertex1") );
		Assert.IsTrue( oVertices.Contains("Edge1Vertex2") );

		IEdgeCollection oEdges = oGraph.Edges;

		Assert.AreEqual(3, oEdges.Count);

		foreach (IEdge oEdge in oEdges)
		{
			String sVertex1Name = oEdge.Vertices[0].Name;
			String sVertex2Name = oEdge.Vertices[1].Name;

			Assert.AreEqual("Edge1Vertex1", sVertex1Name);

			Assert.AreEqual("Edge1Vertex2", sVertex2Name);
		}
    }

    //*************************************************************************
    //  Method: TestLoadGraph2_10()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph2_10()
    {
		// N edges with empty lines.  (Empty lines should be skipped.)

		const String StreamContents =
			"\r\n"
			+ "Edge1Vertex1\tEdge1Vertex2\r\n"
			+ " \r\n"
			+ "Edge2Vertex1\tEdge2Vertex2\r\n"
			+ "\t\r\n"
			+ "Edge3Vertex1\tEdge3Vertex2\r\n"
			+ "    \t   \r\n"
			+ "     \r\n"
			;

		StringStream oStream = new StringStream(StreamContents);

		IGraph oGraph = m_oGraphAdapter.LoadGraph(new GraphFactory(), oStream);

		IVertexCollection oVertices = oGraph.Vertices;

		Assert.AreEqual(6, oVertices.Count);

		Assert.IsTrue( oVertices.Contains("Edge1Vertex1") );
		Assert.IsTrue( oVertices.Contains("Edge1Vertex2") );
		Assert.IsTrue( oVertices.Contains("Edge2Vertex1") );
		Assert.IsTrue( oVertices.Contains("Edge2Vertex2") );
		Assert.IsTrue( oVertices.Contains("Edge3Vertex1") );
		Assert.IsTrue( oVertices.Contains("Edge3Vertex2") );

		IEdgeCollection oEdges = oGraph.Edges;

		Assert.AreEqual(3, oEdges.Count);

		foreach (IEdge oEdge in oEdges)
		{
			String sVertex1Name = oEdge.Vertices[0].Name;
			String sVertex2Name = oEdge.Vertices[1].Name;

			Assert.IsTrue( sVertex1Name.StartsWith("Edge") );

			Int32 iEdgeNumber = Int32.Parse( sVertex1Name.Substring(4, 1) );

			Assert.AreEqual("Edge" + iEdgeNumber.ToString() + "Vertex1",
				sVertex1Name);

			Assert.AreEqual("Edge" + iEdgeNumber.ToString() + "Vertex2",
				sVertex2Name);
		}
    }

    //*************************************************************************
    //  Method: TestLoadGraph2_Bad()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestLoadGraph2_Bad()
    {
		// null graphFactory.

		try
		{
			m_oGraphAdapter.LoadGraph( null, new MemoryStream() );
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Adapters."
				+ "SimpleGraphAdapter.LoadGraph: graphFactory argument can't"
				+ " be null.\r\n"
				+ "Parameter name: graphFactory"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestLoadGraph2_Bad2()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestLoadGraph2_Bad2()
    {
		// null stream.

		try
		{
			m_oGraphAdapter.LoadGraph(new GraphFactory(), (Stream)null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Adapters."
				+ "SimpleGraphAdapter.LoadGraph: stream argument can't"
				+ " be null.\r\n"
				+ "Parameter name: stream"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestLoadGraph2_Bad3()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph2_Bad3()
    {
		// One edge, multiple tabs.

		TestLoadGraphSingleEdgeBad(
			"Vertex1\t\tVertex2\r\n"
			);
    }

    //*************************************************************************
    //  Method: TestLoadGraph2_Bad4()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph2_Bad4()
    {
		// One edge, no tabs.

		TestLoadGraphSingleEdgeBad(
			"Vertex1 Vertex2\r\n"
			);
    }

    //*************************************************************************
    //  Method: TestLoadGraph2_Bad5()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph2_Bad5()
    {
		// One edge, leading tabs.

		TestLoadGraphSingleEdgeBad(
			"\tVertex1\tVertex2\r\n"
			);
    }

    //*************************************************************************
    //  Method: TestLoadGraph2_Bad6()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph2_Bad6()
    {
		// One edge, trailing tab.

		TestLoadGraphSingleEdgeBad(
			"Vertex1\tVertex2\t\r\n"
			);
    }

    //*************************************************************************
    //  Method: TestLoadGraph2_Bad7()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph2_Bad7()
    {
		// One edge, first vertex name is empty.

		TestLoadGraphSingleEdgeBad(
			"\tVertex2\r\n"
			);
    }

    //*************************************************************************
    //  Method: TestLoadGraph2_Bad8()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph2_Bad8()
    {
		// One edge, second vertex name is empty.

		TestLoadGraphSingleEdgeBad(
			"Vertex1\t\r\n"
			);
    }

    //*************************************************************************
    //  Method: TestLoadGraph2_Bad9()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph2_Bad9()
    {
		// One edge, three tokens.

		TestLoadGraphSingleEdgeBad(
			"Vertex1\tVertex2\tXX\r\n"
			);
    }

    //*************************************************************************
    //  Method: TestLoadGraph3_()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph3_()
    {
		const String String =
			"Vertex1\tVertex2\r\n";

		const String Vertex1Name = "Vertex1";
		const String Vertex2Name = "Vertex2";

		IGraph oGraph =
			m_oGraphAdapter.LoadGraph(new GraphFactory(), String);

		Assert.IsInstanceOfType( oGraph, typeof(Graph) );

		IVertexCollection oVertices = oGraph.Vertices;

		Assert.AreEqual(2, oVertices.Count);

		Assert.IsTrue( oVertices.Contains(Vertex1Name) );
		Assert.IsTrue( oVertices.Contains(Vertex2Name) );

		IEdgeCollection oEdges = oGraph.Edges;

		Assert.AreEqual(1, oEdges.Count);

		foreach (IEdge oEdge in oEdges)
		{
			Assert.AreEqual(Vertex1Name, oEdge.Vertices[0].Name);
			Assert.AreEqual(Vertex2Name, oEdge.Vertices[1].Name);
		}
    }

    //*************************************************************************
    //  Method: TestLoadGraph3_Bad()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestLoadGraph3_Bad()
    {
		// null graphFactory.

		try
		{
			m_oGraphAdapter.LoadGraph(null, "abc");
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Adapters."
				+ "SimpleGraphAdapter.LoadGraph: graphFactory argument can't"
				+ " be null.\r\n"
				+ "Parameter name: graphFactory"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestLoadGraph3_Bad2()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestLoadGraph3_Bad2()
    {
		// null string.

		try
		{
			m_oGraphAdapter.LoadGraph(new GraphFactory(), (String)null);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Adapters."
				+ "SimpleGraphAdapter.LoadGraph: theString argument can't"
				+ " be null.\r\n"
				+ "Parameter name: theString"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestSaveGraph()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSaveGraph()
    {
		// One edge.

		IGraph oGraph = CreateGraph();

		IVertexCollection oVertices = oGraph.Vertices;

		IEdgeCollection oEdges = oGraph.Edges;

		IVertex oVertex1 = oVertices.Add();
		oVertex1.Name = "Vertex1";

		IVertex oVertex2 = oVertices.Add();
		oVertex2.Name = "Vertex2";

		oEdges.Add(oVertex1, oVertex2, true);

		m_oGraphAdapter.SaveGraph(oGraph, m_sTempFileName);

		String sFileContents;

		using ( StreamReader oStreamReader = new StreamReader(m_sTempFileName) )
		{
			sFileContents = oStreamReader.ReadToEnd();
		}

		Assert.AreEqual("Vertex1\tVertex2\r\n", sFileContents);
    }

    //*************************************************************************
    //  Method: TestSaveGraphBad()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestSaveGraphBad()
    {
		// null graph.

		try
		{
			m_oGraphAdapter.SaveGraph(null, "x");
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Adapters."
				+ "SimpleGraphAdapter.SaveGraph: graph argument can't be"
				+ " null.\r\n"
				+ "Parameter name: graph"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestSaveGraphBad2()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestSaveGraphBad2()
    {
		// null filename.

        String sFileName = null;

		try
		{
			m_oGraphAdapter.SaveGraph(CreateGraph(), sFileName);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Adapters."
				+ "SimpleGraphAdapter.SaveGraph: filename argument can't be"
				+ " null.\r\n"
				+ "Parameter name: filename"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestSaveGraphBad3()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestSaveGraphBad3()
    {
		// Empty filename.

        String sFileName = String.Empty;

		try
		{
			m_oGraphAdapter.SaveGraph(CreateGraph(), sFileName);
		}
		catch (ArgumentException oArgumentException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Adapters."
				+ "SimpleGraphAdapter.SaveGraph: filename argument must have a"
				+ " length greater than zero.\r\n"
				+ "Parameter name: filename"
				,
				oArgumentException.Message
				);

			throw oArgumentException;
		}
    }

    //*************************************************************************
    //  Method: TestSaveGraph2_()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSaveGraph2_()
    {
		// Empty graph.

		IGraph oGraph = CreateGraph();

		MemoryStream oMemoryStream = new MemoryStream();

		m_oGraphAdapter.SaveGraph(oGraph, oMemoryStream);

		oMemoryStream.Position = 0;

		StreamReader oStreamReader =
			new StreamReader(oMemoryStream, Encoding.UTF8);

		String sStreamContents = oStreamReader.ReadToEnd();

		Assert.AreEqual(0, sStreamContents.Length);
    }

    //*************************************************************************
    //  Method: TestSaveGraph2_2()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSaveGraph2_2()
    {
		// One edge.

		IGraph oGraph = CreateGraph();

		IVertexCollection oVertices = oGraph.Vertices;

		IEdgeCollection oEdges = oGraph.Edges;

		IVertex oVertex1 = oVertices.Add();
		oVertex1.Name = "Vertex1";

		IVertex oVertex2 = oVertices.Add();
		oVertex2.Name = "Vertex2";

		oEdges.Add(oVertex1, oVertex2, true);

		MemoryStream oMemoryStream = new MemoryStream();

		m_oGraphAdapter.SaveGraph(oGraph, oMemoryStream);

		oMemoryStream.Position = 0;

		StreamReader oStreamReader =
			new StreamReader(oMemoryStream, Encoding.UTF8);

		String sStreamContents = oStreamReader.ReadToEnd();

		Assert.AreEqual("Vertex1\tVertex2\r\n", sStreamContents);
    }

    //*************************************************************************
    //  Method: TestSaveGraph2_3()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSaveGraph2_3()
    {
		// One edge with spaces for names.

		IGraph oGraph = CreateGraph();

		IVertexCollection oVertices = oGraph.Vertices;

		IEdgeCollection oEdges = oGraph.Edges;

		IVertex oVertex1 = oVertices.Add();
		oVertex1.Name = " ";

		IVertex oVertex2 = oVertices.Add();
		oVertex2.Name = " ";

		oEdges.Add(oVertex1, oVertex2, true);

		MemoryStream oMemoryStream = new MemoryStream();

		m_oGraphAdapter.SaveGraph(oGraph, oMemoryStream);

		oMemoryStream.Position = 0;

		StreamReader oStreamReader =
			new StreamReader(oMemoryStream, Encoding.UTF8);

		String sStreamContents = oStreamReader.ReadToEnd();

		Assert.AreEqual(" \t \r\n", sStreamContents);
    }

    //*************************************************************************
    //  Method: TestSaveGraph2_4()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSaveGraph2_4()
    {
		// N edges.

		IGraph oGraph = CreateGraph();

		IVertexCollection oVertices = oGraph.Vertices;

		IEdgeCollection oEdges = oGraph.Edges;

		IVertex oEdge1Vertex1 = oVertices.Add();
		oEdge1Vertex1.Name = "Edge1Vertex1";

		IVertex oEdge1Vertex2 = oVertices.Add();
		oEdge1Vertex2.Name = "Edge1Vertex2";

		IVertex oEdge2Vertex1 = oVertices.Add();
		oEdge2Vertex1.Name = "Edge2Vertex1";

		IVertex oEdge2Vertex2 = oVertices.Add();
		oEdge2Vertex2.Name = "Edge2Vertex2";

		IVertex oEdge3Vertex1 = oVertices.Add();
		oEdge3Vertex1.Name = "Edge3Vertex1";

		IVertex oEdge3Vertex2 = oVertices.Add();
		oEdge3Vertex2.Name = "Edge3Vertex2";

		oEdges.Add(oEdge1Vertex1, oEdge1Vertex2, true);
		oEdges.Add(oEdge2Vertex1, oEdge2Vertex2, true);
		oEdges.Add(oEdge3Vertex1, oEdge3Vertex2, true);

		MemoryStream oMemoryStream = new MemoryStream();

		m_oGraphAdapter.SaveGraph(oGraph, oMemoryStream);

		oMemoryStream.Position = 0;

		StreamReader oStreamReader =
			new StreamReader(oMemoryStream, Encoding.UTF8);

		String sStreamContents = oStreamReader.ReadToEnd();

		// Note: The order of the lines is indeterminate, so this could break
		// in the future.

		Assert.AreEqual(
			"Edge3Vertex1\tEdge3Vertex2\r\n"
			+ "Edge2Vertex1\tEdge2Vertex2\r\n"
			+ "Edge1Vertex1\tEdge1Vertex2\r\n"
			,
			sStreamContents
			);
    }

    //*************************************************************************
    //  Method: TestSaveGraph2_Bad()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestSaveGraph2_Bad()
    {
		// null graph.

		try
		{
			m_oGraphAdapter.SaveGraph( null, new MemoryStream() );
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Adapters."
				+ "SimpleGraphAdapter.SaveGraph: graph argument can't be"
				+ " null.\r\n"
				+ "Parameter name: graph"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestSaveGraph2_Bad2()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestSaveGraph2_Bad2()
    {
		// null stream.

        Stream oStream = null;

		try
		{
			m_oGraphAdapter.SaveGraph(CreateGraph(), oStream);
		}
		catch (ArgumentNullException oArgumentNullException)
		{
			Assert.AreEqual(

				"Microsoft.NetMap.Adapters."
				+ "SimpleGraphAdapter.SaveGraph: stream argument can't be"
				+ " null.\r\n"
				+ "Parameter name: stream"
				,
				oArgumentNullException.Message
				);

			throw oArgumentNullException;
		}
    }

    //*************************************************************************
    //  Method: TestSaveGraph2_Bad3()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(IOException) ) ]

    public void
    TestSaveGraph2_Bad3()
    {
		// Undirected graph.

		try
		{
			IGraph oGraph = new Graph(GraphDirectedness.Undirected);

			m_oGraphAdapter.SaveGraph( oGraph, new MemoryStream() );
		}
		catch (IOException oIOException)
		{
			Assert.AreEqual(

				"The graph can't be saved as the file type you've selected,"
				+ " because the graph is undirected and the file type can't be"
				+ " used with undirected graphs."
				,
				oIOException.Message
				);

			throw oIOException;
		}
    }

    //*************************************************************************
    //  Method: TestSaveGraph2_Bad4()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(IOException) ) ]

    public void
    TestSaveGraph2_Bad4()
    {
		// Mixed graph.

		try
		{
			IGraph oGraph = new Graph(GraphDirectedness.Mixed);

			m_oGraphAdapter.SaveGraph( oGraph, new MemoryStream() );
		}
		catch (IOException oIOException)
		{
			Assert.AreEqual(

				"The graph can't be saved as the file type you've selected,"
				+ " because the graph is mixed and the file type can't be"
				+ " used with mixed graphs."
				,
				oIOException.Message
				);

			throw oIOException;
		}
    }

    //*************************************************************************
    //  Method: TestSaveGraph2_Bad5()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(FormatException) ) ]

    public void
    TestSaveGraph2_Bad5()
    {
		// First vertex has null name.

		IVertex oVertex1 = null;
		IVertex oVertex2 = null;

		try
		{
			IGraph oGraph = CreateGraph();

			IVertexCollection oVertices = oGraph.Vertices;

			IEdgeCollection oEdges = oGraph.Edges;

			oVertex1 = oVertices.Add();
			oVertex1.Name = null;

			oVertex2 = oVertices.Add();
			oVertex2.Name = "Edge1Vertex2";

			oEdges.Add(oVertex1, oVertex2, true);

			MemoryStream oMemoryStream = new MemoryStream();

			m_oGraphAdapter.SaveGraph(oGraph, oMemoryStream);
		}
		catch (FormatException oFormatException)
		{
			Assert.AreEqual( String.Format(

				"The vertex with the ID {0} has a null or empty name.  All"
				+ " vertices must have a name when saving a graph with this"
				+ " graph adapter."
				,
				oVertex1.ID.ToString("N0")
				),

				oFormatException.Message
				);

			throw oFormatException;
		}
    }

    //*************************************************************************
    //  Method: TestSaveGraph2_Bad6()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(FormatException) ) ]

    public void
    TestSaveGraph2_Bad6()
    {
		// First vertex has empty name.

		IVertex oVertex1 = null;
		IVertex oVertex2 = null;

		try
		{
			IGraph oGraph = CreateGraph();

			IVertexCollection oVertices = oGraph.Vertices;

			IEdgeCollection oEdges = oGraph.Edges;

			oVertex1 = oVertices.Add();
			oVertex1.Name = String.Empty;

			oVertex2 = oVertices.Add();
			oVertex2.Name = "Edge1Vertex2";

			oEdges.Add(oVertex1, oVertex2, true);

			MemoryStream oMemoryStream = new MemoryStream();

			m_oGraphAdapter.SaveGraph(oGraph, oMemoryStream);
		}
		catch (FormatException oFormatException)
		{
			Assert.AreEqual( String.Format(

				"The vertex with the ID {0} has a null or empty name.  All"
				+ " vertices must have a name when saving a graph with this"
				+ " graph adapter."
				,
				oVertex1.ID.ToString("N0")
				),

				oFormatException.Message
				);

			throw oFormatException;
		}
    }

    //*************************************************************************
    //  Method: TestSaveGraph2_Bad7()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(FormatException) ) ]

    public void
    TestSaveGraph2_Bad7()
    {
		// Second vertex has null name.

		IVertex oVertex1 = null;
		IVertex oVertex2 = null;

		try
		{
			IGraph oGraph = CreateGraph();

			IVertexCollection oVertices = oGraph.Vertices;

			IEdgeCollection oEdges = oGraph.Edges;

			oVertex1 = oVertices.Add();
			oVertex1.Name = "Edge1Vertex1";

			oVertex2 = oVertices.Add();
			oVertex2.Name = null;

			oEdges.Add(oVertex1, oVertex2, true);

			MemoryStream oMemoryStream = new MemoryStream();

			m_oGraphAdapter.SaveGraph(oGraph, oMemoryStream);
		}
		catch (FormatException oFormatException)
		{
			Assert.AreEqual( String.Format(

				"The vertex with the ID {0} has a null or empty name.  All"
				+ " vertices must have a name when saving a graph with this"
				+ " graph adapter."
				,
				oVertex2.ID.ToString("N0")
				),

				oFormatException.Message
				);

			throw oFormatException;
		}
    }

    //*************************************************************************
    //  Method: TestSaveGraph2_Bad8()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, Stream) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(FormatException) ) ]

    public void
    TestSaveGraph2_Bad8()
    {
		// Second vertex has empty name.

		IVertex oVertex1 = null;
		IVertex oVertex2 = null;

		try
		{
			IGraph oGraph = CreateGraph();

			IVertexCollection oVertices = oGraph.Vertices;

			IEdgeCollection oEdges = oGraph.Edges;

			oVertex1 = oVertices.Add();
			oVertex1.Name = "Edge1Vertex1";

			oVertex2 = oVertices.Add();
			oVertex2.Name = String.Empty;

			oEdges.Add(oVertex1, oVertex2, true);

			MemoryStream oMemoryStream = new MemoryStream();

			m_oGraphAdapter.SaveGraph(oGraph, oMemoryStream);
		}
		catch (FormatException oFormatException)
		{
			Assert.AreEqual( String.Format(

				"The vertex with the ID {0} has a null or empty name.  All"
				+ " vertices must have a name when saving a graph with this"
				+ " graph adapter."
				,
				oVertex2.ID.ToString("N0")
				),

				oFormatException.Message
				);

			throw oFormatException;
		}
    }

    //*************************************************************************
    //  Method: TestSupportsDirectedness()
    //
    /// <summary>
    /// Tests the SupportsDirectedness() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSupportsDirectedness()
    {
		Assert.IsFalse( m_oGraphAdapter.SupportsDirectedness(
			GraphDirectedness.Undirected) );
    }

    //*************************************************************************
    //  Method: TestSupportsDirectedness2()
    //
    /// <summary>
    /// Tests the SupportsDirectedness() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSupportsDirectedness2()
    {
		Assert.IsTrue( m_oGraphAdapter.SupportsDirectedness(
			GraphDirectedness.Directed) );
    }

    //*************************************************************************
    //  Method: TestSupportsDirectedness3()
    //
    /// <summary>
    /// Tests the SupportsDirectedness() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSupportsDirectedness3()
    {
		Assert.IsFalse( m_oGraphAdapter.SupportsDirectedness(
			GraphDirectedness.Mixed) );
    }

    //*************************************************************************
    //  Method: CreateGraph()
    //
    /// <summary>
    /// Creates a graph that is compatible with <see
	/// cref="SimpleGraphAdapter" />.
    /// </summary>
	///
	/// <returns>
	/// A new compatible graph.
	/// </returns>
    //*************************************************************************

    protected IGraph
    CreateGraph()
    {
		return ( new Graph(GraphDirectedness.Directed) );
    }

    //*************************************************************************
    //  Method: TestLoadGraphSingleEdge()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method when the file
	/// contains one valid edge.
    /// </summary>
	///
	/// <param name="sStreamContents">
	/// Contents of the stream being tested.
	/// </param>
	///
	/// <param name="sVertex1Name">
	/// Expected name of the edge's first vertex.
	/// </param>
	///
	/// <param name="sVertex2Name">
	/// Expected name of the edge's second vertex.
	/// </param>
    //*************************************************************************

    protected void
    TestLoadGraphSingleEdge
	(
		String sStreamContents,
		String sVertex1Name,
		String sVertex2Name
	)
    {
		StringStream oStream = new StringStream(sStreamContents);

		IGraph oGraph = m_oGraphAdapter.LoadGraph(new GraphFactory(), oStream);

		IVertexCollection oVertices = oGraph.Vertices;

		Assert.AreEqual(2, oVertices.Count);

		Assert.IsTrue( oVertices.Contains(sVertex1Name) );
		Assert.IsTrue( oVertices.Contains(sVertex2Name) );

		IEdgeCollection oEdges = oGraph.Edges;

		Assert.AreEqual(1, oEdges.Count);

		foreach (IEdge oEdge in oEdges)
		{
			Assert.AreEqual(sVertex1Name, oEdge.Vertices[0].Name);
			Assert.AreEqual(sVertex2Name, oEdge.Vertices[1].Name);
		}
    }

    //*************************************************************************
    //  Method: TestLoadGraphSingleEdgeBad()
    //
    /// <summary>
    /// Tests the LoadGraph(IGraphFactory, Stream) method when the file
	/// contains one invalid edge.
    /// </summary>
	///
	/// <param name="sStreamContents">
	/// Contents of the stream being tested.
	/// </param>
    //*************************************************************************

    protected void
    TestLoadGraphSingleEdgeBad
	(
		String sStreamContents
	)
    {
		StringStream oStream = new StringStream(sStreamContents);

		Boolean bExceptionThrown = false;

		try
		{
			IGraph oGraph = m_oGraphAdapter.LoadGraph(
				new GraphFactory(), oStream);
		}
		catch (FormatException oFormatException)
		{
			bExceptionThrown = true;

			oStream.Position = 0;

			String sLine =
				( new StreamReader(oStream, Encoding.UTF8) ).ReadLine();

			String sExpectedMessage = String.Format(

				"Line 1 is not in the expected format.  This is line"
				+ " 1: \"{0}\".  The expected format is"
				+ " \"Vertex1Name{{tab}}Vertex2Name\"."
				,
				sLine.Replace('\t', '\u25A1')
				);

			Assert.AreEqual(sExpectedMessage, oFormatException.Message);
		}

		Assert.IsTrue(bExceptionThrown);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected IGraphAdapter m_oGraphAdapter;

	/// Name of the temporary file that may be created by the unit tests.

	protected String m_sTempFileName;
}

}
