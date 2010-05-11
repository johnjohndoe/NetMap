
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Adapters;
using Microsoft.NodeXL.UnitTests;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: GraphMLGraphAdapterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="GraphMLGraphAdapter" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class GraphMLGraphAdapterTest : Object
{
    //*************************************************************************
    //  Constructor: GraphMLGraphAdapterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMLGraphAdapterTest" />
    /// class.
    /// </summary>
    //*************************************************************************

    public GraphMLGraphAdapterTest()
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
        m_oGraphAdapter = new GraphMLGraphAdapter();
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
    //  Method: TestLoadGraphFromStream()
    //
    /// <summary>
    /// Tests the LoadGraphFromStream() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromStream()
    {
        // Overall test.

        const String XmlString =

"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
+ " <graphml xmlns=\"http://graphml.graphdrawing.org/xmlns\">"
+ "            "
+ "  <key id=\"VertexColor\" for=\"node\" attr.name=\"Color\" attr.type=\"string\" />"
+ "  <key id=\"VertexLatestPostDate\" for=\"node\" attr.name=\"Latest Post Date\""
+ "      attr.type=\"string\" />"
+ "  <key id=\"EdgeWidth\" for=\"edge\" attr.name=\"Width\" attr.type=\"double\">"
+ "      <default>1.5</default>"
+ "  </key>"
+ "  <graph edgedefault=\"undirected\">"
+ "      <node id=\"V1\">"
+ "          <data key=\"VertexColor\">red</data>"
+ "          <data key=\"VertexLatestPostDate\">2009/07/05</data>"
+ "      </node>"
+ "      <node id=\"V2\">"
+ "          <data key=\"VertexColor\">orange</data>"
+ "          <data key=\"VertexLatestPostDate\">2009/07/12</data>"
+ "      </node>"
+ "      <node id=\"V3\">"
+ "          <data key=\"VertexColor\">blue</data>"
+ "      </node>"
+ "      <node id=\"V4\">"
+ "          <data key=\"VertexColor\">128,0,128</data>"
+ "      </node>"
+ "      <node id=\"V5\" />"
+ "      <edge id=\"E1\" source=\"V1\" target=\"V2\" />"
+ "      <edge id=\"E2\" source=\"V3\" target=\"V2\">"
+ "          <data key=\"EdgeWidth\">2.5</data>"
+ "      </edge>"
+ "  </graph>"
+ " </graphml>"
        ;

        Stream oXmlStream = new StringStream(XmlString);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromStream(oXmlStream);

        Assert.AreEqual(GraphDirectedness.Undirected, oGraph.Directedness);

        IVertexCollection oVertices = oGraph.Vertices;
        IEdgeCollection oEdges = oGraph.Edges;
        Boolean bFound;
        IVertex oVertex;
        String sColor, sLatestPostDate;

        Assert.AreEqual(5, oVertices.Count);

        bFound = oVertices.Find("V1", out oVertex);
        Assert.IsTrue(bFound);
        sColor = (String)oVertex.GetRequiredValue( "Color", typeof(String) );
        Assert.AreEqual("red", sColor);

        sLatestPostDate = (String)oVertex.GetRequiredValue(
            "Latest Post Date", typeof(String) );

        Assert.AreEqual("2009/07/05", sLatestPostDate);


        bFound = oVertices.Find("V2", out oVertex);
        Assert.IsTrue(bFound);
        sColor = (String)oVertex.GetRequiredValue( "Color", typeof(String) );
        Assert.AreEqual("orange", sColor);

        sLatestPostDate = (String)oVertex.GetRequiredValue(
            "Latest Post Date", typeof(String) );

        Assert.AreEqual("2009/07/12", sLatestPostDate);


        bFound = oVertices.Find("V3", out oVertex);
        Assert.IsTrue(bFound);
        sColor = (String)oVertex.GetRequiredValue( "Color", typeof(String) );
        Assert.AreEqual("blue", sColor);


        bFound = oVertices.Find("V4", out oVertex);
        Assert.IsTrue(bFound);
        sColor = (String)oVertex.GetRequiredValue( "Color", typeof(String) );
        Assert.AreEqual("128,0,128", sColor);


        bFound = oVertices.Find("V5", out oVertex);
        Assert.IsTrue(bFound);


        IEdge oEdge;
        Double dWidth;

        bFound = oEdges.Find("E1", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("V1", oEdge.Vertices[0].Name);
        Assert.AreEqual("V2", oEdge.Vertices[1].Name);
        dWidth = (Double)oEdge.GetRequiredValue( "Width", typeof(Double) );
        Assert.AreEqual(1.5, dWidth);

        bFound = oEdges.Find("E2", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("V3", oEdge.Vertices[0].Name);
        Assert.AreEqual("V2", oEdge.Vertices[1].Name);
        dWidth = (Double)oEdge.GetRequiredValue( "Width", typeof(Double) );
        Assert.AreEqual(2.5, dWidth);

        String [] asGraphMLVertexAttributes = ( String[] )
            oGraph.GetRequiredValue(ReservedMetadataKeys.AllVertexMetadataKeys,
                typeof( String[] ) );

        Assert.AreEqual(2, asGraphMLVertexAttributes.Length);

        Assert.IsTrue(Array.IndexOf<String>(asGraphMLVertexAttributes,
            "Color") >= 0);

        Assert.IsTrue(Array.IndexOf<String>(asGraphMLVertexAttributes,
            "Latest Post Date") >= 0);

        String [] asGraphMLEdgeAttributes = ( String[] )
            oGraph.GetRequiredValue(ReservedMetadataKeys.AllEdgeMetadataKeys,
                typeof( String[] ) );

        Assert.AreEqual(1, asGraphMLEdgeAttributes.Length);

        Assert.IsTrue(Array.IndexOf<String>(asGraphMLEdgeAttributes,
            "Width") >= 0);
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromStream2()
    //
    /// <summary>
    /// Tests the LoadGraphFromStream() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromStream2()
    {
        // Overall test, using sample XML from the GraphML Primer.

        const String XmlString =
        "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
        + "<graphml xmlns=\"http://graphml.graphdrawing.org/xmlns\"  "
              + "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" "
              + "xsi:schemaLocation=\"http://graphml.graphdrawing.org/xmlns "
                + "http://graphml.graphdrawing.org/xmlns/1.0/graphml.xsd\">"
          + "<key id=\"d0\" for=\"node\" attr.name=\"color\" attr.type=\"string\">"
            + "<default>yellow</default>"
          + "</key>"
          + "<key id=\"d1\" for=\"edge\" attr.name=\"weight\" attr.type=\"double\"/>"
          + "<graph id=\"G\" edgedefault=\"undirected\">"
            + "<node id=\"n0\">"
              + "<data key=\"d0\">green</data>"
            + "</node>"
            + "<node id=\"n1\"/>"
            + "<node id=\"n2\">"
              + "<data key=\"d0\">blue</data>"
            + "</node>"
            + "<node id=\"n3\">"
              + "<data key=\"d0\">red</data>"
            + "</node>"
            + "<node id=\"n4\"/>"
            + "<node id=\"n5\">"
              + "<data key=\"d0\">turquoise</data>"
            + "</node>"
            + "<edge id=\"e0\" source=\"n0\" target=\"n2\">"
              + "<data key=\"d1\">1.0</data>"
            + "</edge>"
            + "<edge id=\"e1\" source=\"n0\" target=\"n1\">"
              + "<data key=\"d1\">1.0</data>"
            + "</edge>"
            + "<edge id=\"e2\" source=\"n1\" target=\"n3\">"
              + "<data key=\"d1\">2.0</data>"
            + "</edge>"
            + "<edge id=\"e3\" source=\"n3\" target=\"n2\"/>"
            + "<edge id=\"e4\" source=\"n2\" target=\"n4\"/>"
            + "<edge id=\"e5\" source=\"n3\" target=\"n5\"/>"
            + "<edge id=\"e6\" source=\"n5\" target=\"n4\">"
              + "<data key=\"d1\">1.1</data>"
            + "</edge>"
          + "</graph>"
        + "</graphml>"
        ;

        Stream oXmlStream = new StringStream(XmlString);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromStream(oXmlStream);

        Assert.AreEqual(GraphDirectedness.Undirected, oGraph.Directedness);

        IVertexCollection oVertices = oGraph.Vertices;
        IEdgeCollection oEdges = oGraph.Edges;
        Boolean bFound;
        IVertex oVertex;
        String sColor;

        Assert.AreEqual(6, oVertices.Count);

        bFound = oVertices.Find("n0", out oVertex);
        Assert.IsTrue(bFound);
        sColor = (String)oVertex.GetRequiredValue( "color", typeof(String) );
        Assert.AreEqual("green", sColor);

        bFound = oVertices.Find("n1", out oVertex);
        Assert.IsTrue(bFound);
        sColor = (String)oVertex.GetRequiredValue( "color", typeof(String) );
        Assert.AreEqual("yellow", sColor);

        bFound = oVertices.Find("n2", out oVertex);
        Assert.IsTrue(bFound);
        sColor = (String)oVertex.GetRequiredValue( "color", typeof(String) );
        Assert.AreEqual("blue", sColor);

        bFound = oVertices.Find("n3", out oVertex);
        Assert.IsTrue(bFound);
        sColor = (String)oVertex.GetRequiredValue( "color", typeof(String) );
        Assert.AreEqual("red", sColor);

        bFound = oVertices.Find("n4", out oVertex);
        Assert.IsTrue(bFound);
        sColor = (String)oVertex.GetRequiredValue( "color", typeof(String) );
        Assert.AreEqual("yellow", sColor);

        bFound = oVertices.Find("n5", out oVertex);
        Assert.IsTrue(bFound);
        sColor = (String)oVertex.GetRequiredValue( "color", typeof(String) );
        Assert.AreEqual("turquoise", sColor);

        IEdge oEdge;
        Double dWeight;

        bFound = oEdges.Find("e0", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("n0", oEdge.Vertices[0].Name);
        Assert.AreEqual("n2", oEdge.Vertices[1].Name);
        dWeight = (Double)oEdge.GetRequiredValue( "weight", typeof(Double) );
        Assert.AreEqual(1.0, dWeight);

        bFound = oEdges.Find("e1", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("n0", oEdge.Vertices[0].Name);
        Assert.AreEqual("n1", oEdge.Vertices[1].Name);
        dWeight = (Double)oEdge.GetRequiredValue( "weight", typeof(Double) );
        Assert.AreEqual(1.0, dWeight);

        bFound = oEdges.Find("e2", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("n1", oEdge.Vertices[0].Name);
        Assert.AreEqual("n3", oEdge.Vertices[1].Name);
        dWeight = (Double)oEdge.GetRequiredValue( "weight", typeof(Double) );
        Assert.AreEqual(2.0, dWeight);

        bFound = oEdges.Find("e3", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("n3", oEdge.Vertices[0].Name);
        Assert.AreEqual("n2", oEdge.Vertices[1].Name);
        Assert.IsFalse( oEdge.ContainsKey("weight") );

        bFound = oEdges.Find("e4", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("n2", oEdge.Vertices[0].Name);
        Assert.AreEqual("n4", oEdge.Vertices[1].Name);
        Assert.IsFalse( oEdge.ContainsKey("weight") );

        bFound = oEdges.Find("e5", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("n3", oEdge.Vertices[0].Name);
        Assert.AreEqual("n5", oEdge.Vertices[1].Name);
        Assert.IsFalse( oEdge.ContainsKey("weight") );

        bFound = oEdges.Find("e6", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("n5", oEdge.Vertices[0].Name);
        Assert.AreEqual("n4", oEdge.Vertices[1].Name);
        dWeight = (Double)oEdge.GetRequiredValue( "weight", typeof(Double) );
        Assert.AreEqual(1.1, dWeight);
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromStream3()
    //
    /// <summary>
    /// Tests the LoadGraphFromStream() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromStream3()
    {
        // Overall test, using sample XML from the GraphML Primer, but with
        // missing attr.name and attr.type attributes, which are optional.
        //
        // Also, include key nodes with "for" attributes not used by NodeXL.
        // The for="graphml" is actually illegal, but was found in a GraphML
        // file created by a program called yED.
        //
        // Also, include a data node with no inner text, which was also in the
        // yED file.

        const String XmlString =
        "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
        + "<graphml xmlns=\"http://graphml.graphdrawing.org/xmlns\"  "
              + "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" "
              + "xsi:schemaLocation=\"http://graphml.graphdrawing.org/xmlns "
                + "http://graphml.graphdrawing.org/xmlns/1.0/graphml.xsd\">"
          + "<key id=\"d0\" for=\"node\">"
            + "<default>yellow</default>"
          + "</key>"
          + "<key id=\"d1\" for=\"edge\"/>"
          + "<key id=\"dSkip1\" for=\"graph\"/>"
          + "<key id=\"dSkip2\" for=\"all\"/>"
          + "<key id=\"dIllegal\" for=\"graphml\"/>"
          + "<graph id=\"G\" edgedefault=\"undirected\">"
            + "<node id=\"n0\">"
              + "<data key=\"d0\">green</data>"
            + "</node>"
            + "<node id=\"n1\"/>"
            + "<node id=\"n2\">"
              + "<data key=\"d0\">blue</data>"
            + "</node>"
            + "<node id=\"n3\">"
              + "<data key=\"d0\">red</data>"
            + "</node>"
            + "<node id=\"n4\">"
              + "<data key=\"d0\" />"
            + "</node>"
            + "<node id=\"n5\">"
              + "<data key=\"d0\">turquoise</data>"
            + "</node>"
            + "<edge id=\"e0\" source=\"n0\" target=\"n2\">"
              + "<data key=\"d1\">1.0</data>"
            + "</edge>"
            + "<edge id=\"e1\" source=\"n0\" target=\"n1\">"
              + "<data key=\"d1\">1.0</data>"
            + "</edge>"
            + "<edge id=\"e2\" source=\"n1\" target=\"n3\">"
              + "<data key=\"d1\">2.0</data>"
            + "</edge>"
            + "<edge id=\"e3\" source=\"n3\" target=\"n2\"/>"
            + "<edge id=\"e4\" source=\"n2\" target=\"n4\"/>"
            + "<edge id=\"e5\" source=\"n3\" target=\"n5\"/>"
            + "<edge id=\"e6\" source=\"n5\" target=\"n4\">"
              + "<data key=\"d1\">1.1</data>"
            + "</edge>"
          + "</graph>"
        + "</graphml>"
        ;

        Stream oXmlStream = new StringStream(XmlString);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromStream(oXmlStream);

        Assert.AreEqual(GraphDirectedness.Undirected, oGraph.Directedness);

        IVertexCollection oVertices = oGraph.Vertices;
        IEdgeCollection oEdges = oGraph.Edges;
        Boolean bFound;
        IVertex oVertex;
        String sColor;

        Assert.AreEqual(6, oVertices.Count);

        bFound = oVertices.Find("n0", out oVertex);
        Assert.IsTrue(bFound);
        sColor = (String)oVertex.GetRequiredValue( "d0", typeof(String) );
        Assert.AreEqual("green", sColor);

        bFound = oVertices.Find("n1", out oVertex);
        Assert.IsTrue(bFound);
        sColor = (String)oVertex.GetRequiredValue( "d0", typeof(String) );
        Assert.AreEqual("yellow", sColor);

        bFound = oVertices.Find("n2", out oVertex);
        Assert.IsTrue(bFound);
        sColor = (String)oVertex.GetRequiredValue( "d0", typeof(String) );
        Assert.AreEqual("blue", sColor);

        bFound = oVertices.Find("n3", out oVertex);
        Assert.IsTrue(bFound);
        sColor = (String)oVertex.GetRequiredValue( "d0", typeof(String) );
        Assert.AreEqual("red", sColor);

        bFound = oVertices.Find("n4", out oVertex);
        Assert.IsTrue(bFound);
        sColor = (String)oVertex.GetRequiredValue( "d0", typeof(String) );
        Assert.AreEqual(String.Empty, sColor);

        bFound = oVertices.Find("n5", out oVertex);
        Assert.IsTrue(bFound);
        sColor = (String)oVertex.GetRequiredValue( "d0", typeof(String) );
        Assert.AreEqual("turquoise", sColor);

        IEdge oEdge;
        String sWeight;

        bFound = oEdges.Find("e0", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("n0", oEdge.Vertices[0].Name);
        Assert.AreEqual("n2", oEdge.Vertices[1].Name);
        sWeight = (String)oEdge.GetRequiredValue( "d1", typeof(String) );
        Assert.AreEqual("1.0", sWeight);

        bFound = oEdges.Find("e1", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("n0", oEdge.Vertices[0].Name);
        Assert.AreEqual("n1", oEdge.Vertices[1].Name);
        sWeight = (String)oEdge.GetRequiredValue( "d1", typeof(String) );
        Assert.AreEqual("1.0", sWeight);

        bFound = oEdges.Find("e2", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("n1", oEdge.Vertices[0].Name);
        Assert.AreEqual("n3", oEdge.Vertices[1].Name);
        sWeight = (String)oEdge.GetRequiredValue( "d1", typeof(String) );
        Assert.AreEqual("2.0", sWeight);

        bFound = oEdges.Find("e3", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("n3", oEdge.Vertices[0].Name);
        Assert.AreEqual("n2", oEdge.Vertices[1].Name);
        Assert.IsFalse( oEdge.ContainsKey("d1") );

        bFound = oEdges.Find("e4", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("n2", oEdge.Vertices[0].Name);
        Assert.AreEqual("n4", oEdge.Vertices[1].Name);
        Assert.IsFalse( oEdge.ContainsKey("d1") );

        bFound = oEdges.Find("e5", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("n3", oEdge.Vertices[0].Name);
        Assert.AreEqual("n5", oEdge.Vertices[1].Name);
        Assert.IsFalse( oEdge.ContainsKey("d1") );

        bFound = oEdges.Find("e6", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("n5", oEdge.Vertices[0].Name);
        Assert.AreEqual("n4", oEdge.Vertices[1].Name);
        sWeight = (String)oEdge.GetRequiredValue( "d1", typeof(String) );
        Assert.AreEqual("1.1", sWeight);
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromStream4()
    //
    /// <summary>
    /// Tests the LoadGraphFromStream() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraphFromStream4()
    {
        // Nested graph, using sample XML from the GraphML Primer.
        // GraphMLGraphAdapter should, according to the Primer, "ignore nodes
        // which are not contained in the top-level graph and to ignore edges
        // which have do not have both endpoints in the top-level graph."

        const String XmlString =
        "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
        + "<graphml xmlns=\"http://graphml.graphdrawing.org/xmlns\"  xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\""
        + " xsi:schemaLocation=\"http://graphml.graphdrawing.org/xmlns http://graphml.graphdrawing.org/xmlns/1.0/graphml.xsd\">"
        + "  <graph id=\"G\" edgedefault=\"undirected\">"
        + "    <node id=\"n0\"/>"
        + "    <node id=\"n1\"/>"
        + "    <node id=\"n2\"/>"
        + "    <node id=\"n3\"/>"
        + "    <node id=\"n4\"/>"
        + "    <node id=\"n5\">"
        + "        <graph id=\"n5:\" edgedefault=\"undirected\">"
        + "          <node id=\"n5::n0\"/>"
        + "          <node id=\"n5::n1\"/>"
        + "          <node id=\"n5::n2\"/>"
        + "          <edge id=\"e0\" source=\"n5::n0\" target=\"n5::n2\"/>"
        + "          <edge id=\"e1\" source=\"n5::n1\" target=\"n5::n2\"/>"
        + "        </graph>"
        + "    </node>"
        + "    <node id=\"n6\">"
        + "        <graph id=\"n6:\" edgedefault=\"undirected\">"
        + "          <node id=\"n6::n0\">"
        + "              <graph id=\"n6::n0:\" edgedefault=\"undirected\">"
        + "                <node id=\"n6::n0::n0\"/>"
        + "               </graph>"
        + "          </node>"
        + "          <node id=\"n6::n1\"/>"
        + "          <node id=\"n6::n2\"/>"
        + "          <edge id=\"e10\" source=\"n6::n1\" target=\"n6::n0::n0\"/>"
        + "          <edge id=\"e11\" source=\"n6::n1\" target=\"n6::n2\"/>"
        + "        </graph>"
        + "    </node>"
        + "    <edge id=\"e2\" source=\"n5::n2\" target=\"n0\"/>"
        + "    <edge id=\"e3\" source=\"n0\" target=\"n2\"/>"
        + "    <edge id=\"e4\" source=\"n0\" target=\"n1\"/>"
        + "    <edge id=\"e5\" source=\"n1\" target=\"n3\"/>"
        + "    <edge id=\"e6\" source=\"n3\" target=\"n2\"/>"
        + "    <edge id=\"e7\" source=\"n2\" target=\"n4\"/>"
        + "    <edge id=\"e8\" source=\"n3\" target=\"n6::n1\"/>"
        + "    <edge id=\"e9\" source=\"n6::n1\" target=\"n4\"/>"
        + "  </graph>"
        + "</graphml>"
        ;

        Stream oXmlStream = new StringStream(XmlString);

        IGraph oGraph = m_oGraphAdapter.LoadGraphFromStream(oXmlStream);

        Assert.AreEqual(GraphDirectedness.Undirected, oGraph.Directedness);

        IVertexCollection oVertices = oGraph.Vertices;
        IEdgeCollection oEdges = oGraph.Edges;
        Boolean bFound;
        IVertex oVertex;

        Assert.AreEqual(7, oVertices.Count);

        bFound = oVertices.Find("n0", out oVertex);
        Assert.IsTrue(bFound);

        bFound = oVertices.Find("n1", out oVertex);
        Assert.IsTrue(bFound);

        bFound = oVertices.Find("n2", out oVertex);
        Assert.IsTrue(bFound);

        bFound = oVertices.Find("n3", out oVertex);
        Assert.IsTrue(bFound);

        bFound = oVertices.Find("n4", out oVertex);
        Assert.IsTrue(bFound);

        bFound = oVertices.Find("n5", out oVertex);
        Assert.IsTrue(bFound);

        bFound = oVertices.Find("n6", out oVertex);
        Assert.IsTrue(bFound);

        IEdge oEdge;

        bFound = oEdges.Find("e3", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("n0", oEdge.Vertices[0].Name);
        Assert.AreEqual("n2", oEdge.Vertices[1].Name);

        bFound = oEdges.Find("e4", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("n0", oEdge.Vertices[0].Name);
        Assert.AreEqual("n1", oEdge.Vertices[1].Name);

        bFound = oEdges.Find("e5", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("n1", oEdge.Vertices[0].Name);
        Assert.AreEqual("n3", oEdge.Vertices[1].Name);

        bFound = oEdges.Find("e6", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("n3", oEdge.Vertices[0].Name);
        Assert.AreEqual("n2", oEdge.Vertices[1].Name);
        Assert.IsFalse( oEdge.ContainsKey("weight") );

        bFound = oEdges.Find("e7", out oEdge);
        Assert.IsTrue(bFound);
        Assert.AreEqual("n2", oEdge.Vertices[0].Name);
        Assert.AreEqual("n4", oEdge.Vertices[1].Name);
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromStreamBad()
    //
    /// <summary>
    /// Tests the LoadGraphFromStream() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestLoadGraphFromStreamBad()
    {
        // Bad graph directedness.

        const String XmlString =

"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
+ " <graphml xmlns=\"http://graphml.graphdrawing.org/xmlns\">"
+ "            "
+ "  <key id=\"VertexColor\" for=\"node\" attr.name=\"Color\" attr.type=\"string\" />"
+ "  <key id=\"VertexLatestPostDate\" for=\"node\" attr.name=\"Latest Post Date\""
+ "      attr.type=\"string\" />"
+ "  <key id=\"EdgeWidth\" for=\"edge\" attr.name=\"Width\" attr.type=\"double\">"
+ "      <default>1.5</default>"
+ "  </key>"
+ "  <graph edgedefault=\"Xundirected\">"
+ "      <node id=\"V1\">"
+ "          <data key=\"VertexColor\">red</data>"
+ "          <data key=\"VertexLatestPostDate\">2009/07/05</data>"
+ "      </node>"
+ "      <node id=\"V2\">"
+ "          <data key=\"VertexColor\">orange</data>"
+ "          <data key=\"VertexLatestPostDate\">2009/07/12</data>"
+ "      </node>"
+ "      <node id=\"V3\">"
+ "          <data key=\"VertexColor\">blue</data>"
+ "      </node>"
+ "      <node id=\"V4\">"
+ "          <data key=\"VertexColor\">128,0,128</data>"
+ "      </node>"
+ "      <node id=\"V5\" />"
+ "      <edge id=\"E1\" source=\"V1\" target=\"V2\" />"
+ "      <edge id=\"E2\" source=\"V3\" target=\"V2\">"
+ "          <data key=\"EdgeWidth\">2.5</data>"
+ "      </edge>"
+ "  </graph>"
+ " </graphml>"
        ;

        Stream oXmlStream = new StringStream(XmlString);

        try
        {
            m_oGraphAdapter.LoadGraphFromStream(oXmlStream);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The \"edgedefault\" attribute on the \"graph\" XML node must"
                + " be either \"directed\" or \"undirected\"."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromStreamBad2()
    //
    /// <summary>
    /// Tests the LoadGraphFromStream() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestLoadGraphFromStreamBad2()
    {
        // Two GraphML-attributes with same ID.

        const String XmlString =

"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
+ " <graphml xmlns=\"http://graphml.graphdrawing.org/xmlns\">"
+ "            "
+ "  <key id=\"Same\" for=\"node\" attr.name=\"Color\" attr.type=\"string\" />"
+ "  <key id=\"VertexLatestPostDate\" for=\"node\" attr.name=\"Latest Post Date\""
+ "      attr.type=\"string\" />"
+ "  <key id=\"Same\" for=\"edge\" attr.name=\"Width\" attr.type=\"double\">"
+ "      <default>1.5</default>"
+ "  </key>"
+ "  <graph edgedefault=\"undirected\">"
+ "      <node id=\"V1\">"
+ "          <data key=\"VertexColor\">red</data>"
+ "          <data key=\"VertexLatestPostDate\">2009/07/05</data>"
+ "      </node>"
+ "      <node id=\"V2\">"
+ "          <data key=\"VertexColor\">orange</data>"
+ "          <data key=\"VertexLatestPostDate\">2009/07/12</data>"
+ "      </node>"
+ "      <node id=\"V3\">"
+ "          <data key=\"VertexColor\">blue</data>"
+ "      </node>"
+ "      <node id=\"V4\">"
+ "          <data key=\"VertexColor\">128,0,128</data>"
+ "      </node>"
+ "      <node id=\"V5\" />"
+ "      <edge id=\"E1\" source=\"V1\" target=\"V2\" />"
+ "      <edge id=\"E2\" source=\"V3\" target=\"V2\">"
+ "          <data key=\"EdgeWidth\">2.5</data>"
+ "      </edge>"
+ "  </graph>"
+ " </graphml>"
        ;

        Stream oXmlStream = new StringStream(XmlString);

        try
        {
            m_oGraphAdapter.LoadGraphFromStream(oXmlStream);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The key id \"Same\" exists for two keys.  Key id values must"
                + " be unique."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromStreamBad3()
    //
    /// <summary>
    /// Tests the LoadGraphFromStream() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestLoadGraphFromStreamBad3()
    {
        // Data XML node with no corresponding key XML node.

        const String XmlString =

"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
+ " <graphml xmlns=\"http://graphml.graphdrawing.org/xmlns\">"
+ "            "
+ "  <key id=\"VertexColor\" for=\"node\" attr.name=\"Color\" attr.type=\"string\" />"
+ "  <key id=\"VertexLatestPostDate\" for=\"node\" attr.name=\"Latest Post Date\""
+ "      attr.type=\"string\" />"
+ "  <key id=\"EdgeWidth\" for=\"edge\" attr.name=\"Width\" attr.type=\"double\">"
+ "      <default>1.5</default>"
+ "  </key>"
+ "  <graph edgedefault=\"undirected\">"
+ "      <node id=\"V1\">"
+ "          <data key=\"VertexColor\">red</data>"
+ "          <data key=\"VertexLatestPostDate\">2009/07/05</data>"
+ "      </node>"
+ "      <node id=\"V2\">"
+ "          <data key=\"VertexColor\">orange</data>"
+ "          <data key=\"VertexLatestPostDate\">2009/07/12</data>"
+ "      </node>"
+ "      <node id=\"V3\">"
+ "          <data key=\"VertexColor\">blue</data>"
+ "      </node>"
+ "      <node id=\"V4\">"
+ "          <data key=\"VertexColor\">128,0,128</data>"
+ "      </node>"
+ "      <node id=\"V5\" />"
+ "      <edge id=\"E1\" source=\"V1\" target=\"V2\" />"
+ "      <edge id=\"E2\" source=\"V3\" target=\"V2\">"
+ "          <data key=\"NoSuchKey\">2.5</data>"
+ "      </edge>"
+ "  </graph>"
+ " </graphml>"
        ;

        Stream oXmlStream = new StringStream(XmlString);

        try
        {
            m_oGraphAdapter.LoadGraphFromStream(oXmlStream);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "A \"data\" XML node has the key \"NoSuchKey\", for which"
                + " there is no corresponding \"key\" XML node."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestLoadGraphFromStreamBad4()
    //
    /// <summary>
    /// Tests the LoadGraphFromStream() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestLoadGraphFromStreamBad4()
    {
        // Duplicate vertex IDs.

        const String XmlString =

"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
+ " <graphml xmlns=\"http://graphml.graphdrawing.org/xmlns\">"
+ "            "
+ "  <key id=\"VertexColor\" for=\"node\" attr.name=\"Color\" attr.type=\"string\" />"
+ "  <key id=\"VertexLatestPostDate\" for=\"node\" attr.name=\"Latest Post Date\""
+ "      attr.type=\"string\" />"
+ "  <key id=\"EdgeWidth\" for=\"edge\" attr.name=\"Width\" attr.type=\"double\">"
+ "      <default>1.5</default>"
+ "  </key>"
+ "  <graph edgedefault=\"undirected\">"
+ "      <node id=\"V1\">"
+ "          <data key=\"VertexColor\">red</data>"
+ "          <data key=\"VertexLatestPostDate\">2009/07/05</data>"
+ "      </node>"
+ "      <node id=\"V2\">"
+ "          <data key=\"VertexColor\">orange</data>"
+ "          <data key=\"VertexLatestPostDate\">2009/07/12</data>"
+ "      </node>"
+ "      <node id=\"V3\">"
+ "          <data key=\"VertexColor\">blue</data>"
+ "      </node>"
+ "      <node id=\"V4\">"
+ "          <data key=\"VertexColor\">128,0,128</data>"
+ "      </node>"
+ "      <node id=\"V5\" />"
+ "      <node id=\"V5\" />"
+ "      <edge id=\"E1\" source=\"V1\" target=\"V2\" />"
+ "      <edge id=\"E2\" source=\"V3\" target=\"V2\">"
+ "          <data key=\"EdgeWidth\">2.5</data>"
+ "      </edge>"
+ "  </graph>"
+ " </graphml>"
        ;

        Stream oXmlStream = new StringStream(XmlString);

        try
        {
            m_oGraphAdapter.LoadGraphFromStream(oXmlStream);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The id \"V5\" exists for two \"node\" XML nodes.  Node id"
                + " values must be unique."
                ,
                oXmlException.Message
                );

            throw oXmlException;
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
        // Directed and undirected graphs.

        foreach (Boolean bDirected in TestGraphUtil.AllBoolean)
        {
            IGraph oGraph = new Graph(bDirected ? GraphDirectedness.Directed
                : GraphDirectedness.Undirected);

            IVertexCollection oVertices = oGraph.Vertices;
            IEdgeCollection oEdges = oGraph.Edges;

            IVertex oVertex1 = oVertices.Add();
            oVertex1.Name = "Vertex1";
            oVertex1.SetValue("VertexAttribute1", 123);  // Int32

            IVertex oVertex2 = oVertices.Add();
            oVertex2.Name = "Vertex2";
            oVertex2.SetValue("VertexAttribute2", "abc");  // String

            IVertex oVertex3 = oVertices.Add();
            oVertex3.Name = "Vertex3";
            oVertex3.SetValue("VertexAttribute1", 4.0);  // Double
            oVertex3.SetValue("VertexAttribute2", 23456.0F);  // Single

            IVertex oVertex4 = oVertices.Add();
            oVertex4.Name = "Vertex4";

            IVertex oVertex5 = oVertices.Add();
            oVertex5.Name = "Vertex5";

            IEdge oEdge;

            oEdge = oEdges.Add(oVertex1, oVertex2, bDirected);
            oEdge.SetValue("EdgeAttribute1", "ea1");

            oEdge = oEdges.Add(oVertex3, oVertex4, bDirected);
            oEdge.SetValue("EdgeAttribute2", "ea2");

            oGraph.SetValue( ReservedMetadataKeys.AllVertexMetadataKeys,
                new String [] {
                    "VertexAttribute1",
                    "VertexAttribute2",
                    } );

            oGraph.SetValue(ReservedMetadataKeys.AllEdgeMetadataKeys,
                new String [] {
                    "EdgeAttribute1",
                    "EdgeAttribute2",
                    } );

            m_oGraphAdapter.SaveGraph(oGraph, m_sTempFileName);

            String sFileContents;

            using ( StreamReader oStreamReader =
                new StreamReader(m_sTempFileName) )
            {
                sFileContents = oStreamReader.ReadToEnd();
            }

            XmlDocument oXmlDocument = new XmlDocument();
            oXmlDocument.LoadXml(sFileContents);

            XmlNamespaceManager oXmlNamespaceManager = new XmlNamespaceManager(
                oXmlDocument.NameTable);

            oXmlNamespaceManager.AddNamespace("g",
                GraphMLGraphAdapter.GraphMLUri);

            String [] asRequiredXPaths = new String [] {

                // Graph node.

                String.Format("/g:graphml/g:graph[@edgedefault='{0}']",
                    bDirected ? "directed" : "undirected"),

                "/g:graphml/g:key[@id='V-VertexAttribute1' and @for='node'"
                    + " and @attr.name='VertexAttribute1'"
                    + " and @attr.type='string']",


                // Vertex nodes.

                "/g:graphml/g:key[@id='V-VertexAttribute2' and @for='node'"
                    + " and @attr.name='VertexAttribute2'"
                    + " and @attr.type='string']",

                "/g:graphml/g:key[@id='E-EdgeAttribute1' and @for='edge'"
                    + " and @attr.name='EdgeAttribute1'"
                    + " and @attr.type='string']",

                "/g:graphml/g:key[@id='E-EdgeAttribute2' and @for='edge'"
                    + " and @attr.name='EdgeAttribute2'"
                    + " and @attr.type='string']",

                "/g:graphml/g:graph/g:node[@id='Vertex1']/"
                    + "g:data[@key='V-VertexAttribute1' and .='123']",

                "/g:graphml/g:graph/g:node[@id='Vertex2']/"
                    + "g:data[@key='V-VertexAttribute2' and .='abc']",

                "/g:graphml/g:graph/g:node[@id='Vertex3']/"
                    + "g:data[@key='V-VertexAttribute1' and .='4']",

                "/g:graphml/g:graph/g:node[@id='Vertex3']/"
                    + "g:data[@key='V-VertexAttribute2' and .='23456']",

                "/g:graphml/g:graph/g:node[@id='Vertex4']",

                "/g:graphml/g:graph/g:node[@id='Vertex5']",


                // Edge nodes.

                "/g:graphml/g:graph/g:edge[@source='Vertex1' and"
                    + " @target='Vertex2']/"
                    + "g:data[@key='E-EdgeAttribute1' and .='ea1']",

                "/g:graphml/g:graph/g:edge[@source='Vertex3' and"
                    + " @target='Vertex4']/"
                    + "g:data[@key='E-EdgeAttribute2' and .='ea2']",
                };

            foreach (String sRequiredXPath in asRequiredXPaths)
            {
                XmlNode oXmlNode = oXmlDocument.SelectSingleNode(
                    sRequiredXPath, oXmlNamespaceManager);

                Assert.IsNotNull(oXmlNode);
            }
        }
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
