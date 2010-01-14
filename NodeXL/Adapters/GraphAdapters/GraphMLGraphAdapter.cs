
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.Research.CommunityTechnologies.XmlLib;

namespace Microsoft.NodeXL.Adapters
{
//*****************************************************************************
//  Class: GraphMLGraphAdapter
//
/// <summary>
/// Converts a graph from (but no to) a GraphML file.
/// </summary>
///
/// <remarks>
/// The <see cref="GraphAdapterBase.LoadGraph(String)" /> method converts a
/// GraphML file to a graph.  The <see
/// cref="GraphAdapterBase.SaveGraph(IGraph, String)" /> method for converting
/// in the other direction is not yet implemented.
///
/// <para>
/// A good introduction to GraphML can be found in the GraphML Primer:
/// </para>
///
/// <para>
/// http://graphml.graphdrawing.org/primer/graphml-primer.html
/// </para>
///
/// <para>
/// Here is a sample GraphML file that can be converted.  It represents a graph
/// with three vertices and two edges.
/// </para>
///
/// <code>
/// &lt;?xml version="1.0" encoding="UTF-8"?&gt;
/// &lt;graphml xmlns="http://graphml.graphdrawing.org/xmlns"&gt;
///     &lt;key id="EdgeWidth" for="edge" attr.name="Width"
///         attr.type="double"&gt;
///         &lt;default&gt;1.5&lt;/default&gt;
///     &lt;/key&gt;
///     &lt;key id="VertexColor" for="node" attr.name="Color"
///         attr.type="string" /&gt;
///     &lt;key id="LatestPostDate" for="node" attr.name="Latest Post Date"
///         attr.type="string" /&gt;
///             &lt;graph edgedefault="undirected"&gt;
///         &lt;node id="V1"&gt;
///             &lt;data key="VertexColor"&gt;red&lt;/data&gt;
///         &lt;/node&gt;
///         &lt;node id="V2"&gt;
///             &lt;data key="VertexColor"&gt;orange&lt;/data&gt;
///         &lt;/node&gt;
///         &lt;node id="V3"&gt;
///             &lt;data key="VertexColor"&gt;blue&lt;/data&gt;
///         &lt;/node&gt;
///         &lt;edge source="V1" target="V2"&gt;
///             &lt;data key="LatestPostDate"&gt;2009/07/05&lt;/data&gt;
///         &lt;/edge&gt;
///         &lt;edge source="V3" target="V2"&gt;
///             &lt;data key="EdgeWidth"&gt;2.5&lt;/data&gt;
///             &lt;data key="LatestPostDate"&gt;2009/07/12&lt;/data&gt;
///         &lt;/edge&gt;
///     &lt;/graph&gt;
/// &lt;/graphml&gt;
/// </code>
///
/// <para>
/// Edge and vertex attributes, which GraphML calls "GraphML-attributes, are
/// supported by this class.  When an edge or vertex has a GraphML-attribute,
/// it gets added to the metadata of the <see cref="IVertex" /> or <see
/// cref="IEdge" />.  The metadata key is the GraphML-attribute's attr.name
/// value and the metadata value is the GraphML-attribute's value.
/// </para>
///
/// <para>
/// To make it possible for the caller to determine which metadata keys were
/// added to the graph's edges and vertices, <see
/// cref="GraphAdapterBase.LoadGraph(String)" /> adds <see
/// cref="ReservedMetadataKeys.GraphMLVertexAttributes" /> and <see
/// cref="ReservedMetadataKeys.GraphMLEdgeAttributes" /> keys to the returned
/// graph.  The key values are of type String[].
/// </para>
///
/// </remarks>
//*****************************************************************************

public class GraphMLGraphAdapter : GraphAdapterBase, IGraphAdapter
{
    //*************************************************************************
    //  Constructor: GraphMLGraphAdapter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMLGraphAdapter" />
    /// class.
    /// </summary>
    //*************************************************************************

    public GraphMLGraphAdapter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: GetSupportedDirectedness()
    //
    /// <summary>
    /// Gets a set of flags indicating the directedness of the graphs that the
    /// implementation can load and save.
    /// </summary>
    ///
    /// <param name="supportsDirected">
    /// Gets set to true if the implementation can load and save directed
    /// graphs.
    /// </param>
    ///
    /// <param name="supportsUndirected">
    /// Gets set to true if the implementation can load and save undirected
    /// graphs.
    /// </param>
    ///
    /// <param name="supportsMixed">
    /// Gets set to true if the implementation can load and save mixed graphs.
    /// </param>
    //*************************************************************************

    protected override void
    GetSupportedDirectedness
    (
        out Boolean supportsDirected,
        out Boolean supportsUndirected,
        out Boolean supportsMixed
    )
    {
        AssertValid();

        supportsDirected = true;
        supportsUndirected = true;
        supportsMixed = false;
    }

    //*************************************************************************
    //  Method: LoadGraphCore()
    //
    /// <summary>
    /// Creates a graph of a specified type and loads it with graph data read
    /// from a <see cref="Stream" />.
    /// </summary>
    ///
    /// <param name="graphFactory">
    /// Object that can create a graph.
    /// </param>
    ///
    /// <param name="stream">
    /// <see cref="Stream" /> containing graph data.
    /// </param>
    ///
    /// <returns>
    /// A new graph created by <paramref name="graphFactory" /> and loaded with
    /// graph data read from <paramref name="stream" />.
    /// </returns>
    ///
    /// <remarks>
    /// This method creates a graph using <paramref name="graphFactory" /> and
    /// loads it with the graph data read from <paramref name="stream" />.  It
    /// does not close <paramref name="stream" />.
    ///
    /// <para>
    /// The arguments have already been checked for validity.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected override IGraph
    LoadGraphCore
    (
        IGraphFactory graphFactory,
        Stream stream
    )
    {
        Debug.Assert(graphFactory != null);
        Debug.Assert(stream != null);
        AssertValid();

        XmlDocument oXmlDocument = new XmlDocument();
        oXmlDocument.Load(stream);

        XmlNamespaceManager oXmlNamespaceManager = new XmlNamespaceManager(
            oXmlDocument.NameTable);

        oXmlNamespaceManager.AddNamespace(GraphMLPrefix, GraphMLUri);

        XmlNode oGraphMLXmlNode = oXmlDocument.DocumentElement;

        XmlNode oGraphXmlNode = XmlUtil2.SelectRequiredSingleNode(
            oGraphMLXmlNode, GraphMLPrefix + ":graph", oXmlNamespaceManager);

        // Parse the vertex and edge attribute definitions.
        //
        // The key is the id attribute of a "key" XML node, and the value is
        // the corresponding GraphMLAttribute object.

        Dictionary<String, GraphMLAttribute> oGraphMLAttributeDictionary =
            ParseGraphMLAttributeDefinitions(oGraphMLXmlNode,
                oXmlNamespaceManager);

        GraphDirectedness eGraphDirectedness =
            GetGraphDirectedness(oGraphXmlNode);

        IGraph oGraph = graphFactory.CreateGraph(eGraphDirectedness,
            GraphRestrictions.None);

        // The key is the id attribute of the "node" XML node, and the value is
        // the corresponding IVertex.

        Dictionary<String, IVertex> oVertexDictionary = ParseVertices(oGraph,
            oGraphXmlNode, oXmlNamespaceManager, oGraphMLAttributeDictionary);

        ParseEdges(oGraph, oGraphXmlNode, oXmlNamespaceManager,
            oVertexDictionary, oGraphMLAttributeDictionary);

        SaveGraphMLAttributeNames(oGraph, oGraphMLAttributeDictionary);

        return (oGraph);
    }

    //*************************************************************************
    //  Method: GetGraphDirectedness()
    //
    /// <summary>
    /// Gets the graph directedness from the "graph" XML node.
    /// </summary>
    ///
    /// <param name="oGraphXmlNode">
    /// "graph" XML node.
    /// </param>
    ///
    /// <returns>
    /// The graph directedness.
    /// </returns>
    //*************************************************************************

    protected GraphDirectedness
    GetGraphDirectedness
    (
        XmlNode oGraphXmlNode
    )
    {
        Debug.Assert(oGraphXmlNode != null);
        AssertValid();

        String sEdgeDefault = XmlUtil2.SelectRequiredSingleNodeAsString(
            oGraphXmlNode, "@edgedefault", null);

        switch (sEdgeDefault)
        {
            case "directed":

                return (GraphDirectedness.Directed);
            
            case "undirected":

                return (GraphDirectedness.Undirected);
            
            default:

                throw new XmlException(
                    "The \"edgedefault\" attribute on the \"graph\" XML node"
                    + " must be either \"directed\" or \"undirected\"."
                    );
        }
    }

    //*************************************************************************
    //  Method: ParseGraphMLAttributeDefinitions()
    //
    /// <summary>
    /// Parses the GraphML vertex and edge attribute definitions.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlNode">
    /// "graphml" XML node.
    /// </param>
    ///
    /// <param name="oXmlNamespaceManager">
    /// XML namespace manager.
    /// </param>
    ///
    /// <returns>
    /// The key is the id attribute of a "key" XML node, and the value is the
    /// corresponding GraphMLAttribute object.
    /// </returns>
    ///
    /// <remarks>
    /// For each "key" XML node, a <see cref="GraphMLAttribute" /> object is
    /// created and added to the returned dictionary.
    /// </remarks>
    //*************************************************************************

    protected Dictionary<String, GraphMLAttribute>
    ParseGraphMLAttributeDefinitions
    (
        XmlNode oGraphMLXmlNode,
        XmlNamespaceManager oXmlNamespaceManager
    )
    {
        Debug.Assert(oGraphMLXmlNode != null);
        Debug.Assert(oXmlNamespaceManager != null);
        AssertValid();

        Dictionary<String, GraphMLAttribute> oGraphMLAttributeDictionary =
            new Dictionary<String, GraphMLAttribute>();

        // Read the GraphML-attribute nodes.

        foreach ( XmlNode oKeyXmlNode in oGraphMLXmlNode.SelectNodes(
            GraphMLPrefix + ":key", oXmlNamespaceManager) )
        {
            GraphMLAttribute oGraphMLAttribute =
                new GraphMLAttribute(oKeyXmlNode, oXmlNamespaceManager,
                    GraphMLPrefix);

            String sID = oGraphMLAttribute.ID;

            try
            {
                oGraphMLAttributeDictionary.Add(sID, oGraphMLAttribute);
            }
            catch (ArgumentException)
            {
                throw new XmlException(
                    "The key id \"" + sID + "\" exists for two keys.  Key id"
                    + " values must be unique."
                    );
            }
        }

        return (oGraphMLAttributeDictionary);
    }

    //*************************************************************************
    //  Method: ParseVertices()
    //
    /// <summary>
    /// Parses the vertices.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph being loaded.
    /// </param>
    ///
    /// <param name="oGraphXmlNode">
    /// "graph" XML node.
    /// </param>
    ///
    /// <param name="oXmlNamespaceManager">
    /// XML namespace manager.
    /// </param>
    ///
    /// <param name="oGraphMLAttributeDictionary">
    /// The key is the id attribute of a "key" XML node, and the value is the
    /// corresponding GraphMLAttribute object.
    /// </param>
    ///
    /// <returns>
    /// The key is the id attribute of the "node" XML node, and the value is
    /// the corresponding IVertex.
    /// </returns>
    ///
    /// <remarks>
    /// For each "node" XML node, a vertex is created, added to <paramref
    /// name="oGraph" />, and added to the returned dictionary.
    /// </remarks>
    //*************************************************************************

    protected Dictionary<String, IVertex>
    ParseVertices
    (
        IGraph oGraph,
        XmlNode oGraphXmlNode,
        XmlNamespaceManager oXmlNamespaceManager,
        Dictionary<String, GraphMLAttribute> oGraphMLAttributeDictionary
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oGraphXmlNode != null);
        Debug.Assert(oXmlNamespaceManager != null);
        Debug.Assert(oGraphMLAttributeDictionary != null);
        AssertValid();

        IVertexCollection oVertices = oGraph.Vertices;

        // The key is the id attribute of the "node" XML node, and the value is
        // the corresponding IVertex.

        Dictionary<String, IVertex> oVertexDictionary =
            new Dictionary<String, IVertex>();

        foreach ( XmlNode oNodeXmlNode in oGraphXmlNode.SelectNodes(
            GraphMLPrefix + ":node", oXmlNamespaceManager) )
        {
            String sID = XmlUtil2.SelectRequiredSingleNodeAsString(
                oNodeXmlNode, "@id", null);

            IVertex oVertex = oVertices.Add();
            oVertex.Name = sID;

            try
            {
                oVertexDictionary.Add(sID, oVertex);
            }
            catch (ArgumentException)
            {
                throw new XmlException(
                    "The id \"" + sID + "\" exists for two \"node\" XML nodes."
                    + "  Node id values must be unique."
                    );
            }

            ParseGraphMLAttributeValues(oNodeXmlNode, oXmlNamespaceManager,
                oVertex, true, oGraphMLAttributeDictionary);
        }

        return (oVertexDictionary);
    }

    //*************************************************************************
    //  Method: ParseEdges()
    //
    /// <summary>
    /// Parses the edges.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph being loaded.
    /// </param>
    ///
    /// <param name="oGraphXmlNode">
    /// "graph" XML node.
    /// </param>
    ///
    /// <param name="oXmlNamespaceManager">
    /// XML namespace manager.
    /// </param>
    ///
    /// <param name="oVertexDictionary">
    /// The key is the id attribute of the "node" XML node, and the value is
    /// the corresponding IVertex.
    /// </param>
    ///
    /// <param name="oGraphMLAttributeDictionary">
    /// The key is the id attribute of a "key" XML node, and the value is the
    /// corresponding GraphMLAttribute object.
    /// </param>
    ///
    /// <remarks>
    /// For each "edge" XML node, an edge is created and added to <paramref
    /// name="oGraph" />.
    /// </remarks>
    //*************************************************************************

    protected void
    ParseEdges
    (
        IGraph oGraph,
        XmlNode oGraphXmlNode,
        XmlNamespaceManager oXmlNamespaceManager,
        Dictionary<String, IVertex> oVertexDictionary,
        Dictionary<String, GraphMLAttribute> oGraphMLAttributeDictionary
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oGraphXmlNode != null);
        Debug.Assert(oXmlNamespaceManager != null);
        Debug.Assert(oVertexDictionary != null);
        Debug.Assert(oGraphMLAttributeDictionary != null);
        AssertValid();

        IEdgeCollection oEdges = oGraph.Edges;

        Boolean bGraphIsDirected =
            (oGraph.Directedness == GraphDirectedness.Directed);

        foreach ( XmlNode oEdgeXmlNode in oGraphXmlNode.SelectNodes(
            GraphMLPrefix + ":edge", oXmlNamespaceManager) )
        {
            IEdge oEdge = oEdges.Add(
                GraphMLNodeIDToVertex(oEdgeXmlNode, "source",
                    oVertexDictionary),

                GraphMLNodeIDToVertex(oEdgeXmlNode, "target",
                    oVertexDictionary),

                bGraphIsDirected
                );

            String sID;

            if ( XmlUtil2.TrySelectSingleNodeAsString(oEdgeXmlNode, "@id",
                null, out sID) )
            {
                oEdge.Name = sID;
            }

            ParseGraphMLAttributeValues(oEdgeXmlNode, oXmlNamespaceManager,
                oEdge, false, oGraphMLAttributeDictionary);
        }
    }

    //*************************************************************************
    //  Method: ParseGraphMLAttributeValues()
    //
    /// <summary>
    /// Parses the GraphML-attribute values for a "node" or "edge" XML node.
    /// </summary>
    ///
    /// <param name="oNodeOrEdgeXmlNode">
    /// "node" or "edge" XML node.
    /// </param>
    ///
    /// <param name="oXmlNamespaceManager">
    /// XML namespace manager.
    /// </param>
    /// <param name="oEdgeOrVertex">
    /// The edge or vertex created from the node.
    /// </param>
    ///
    /// <param name="bIsVertex">
    /// true if the XML node is a "node" node, false if it is an "edge" node.
    /// </param>
    ///
    /// <param name="oGraphMLAttributeDictionary">
    /// The key is the id attribute of a "key" XML node, and the value is the
    /// corresponding GraphMLAttribute object.
    /// </param>
    //*************************************************************************

    protected void
    ParseGraphMLAttributeValues
    (
        XmlNode oNodeOrEdgeXmlNode,
        XmlNamespaceManager oXmlNamespaceManager,
        IMetadataProvider oEdgeOrVertex,
        Boolean bIsVertex,
        Dictionary<String, GraphMLAttribute> oGraphMLAttributeDictionary
    )
    {
        Debug.Assert(oNodeOrEdgeXmlNode != null);
        Debug.Assert(oXmlNamespaceManager != null);
        Debug.Assert(oEdgeOrVertex != null);
        Debug.Assert(oGraphMLAttributeDictionary != null);
        AssertValid();

        // For each GraphML-attribute that has a default value, set a metadata
        // value on the edge or vertex.  This value may may get overwritten
        // later.

        foreach (GraphMLAttribute oGraphMLAttribute in
            oGraphMLAttributeDictionary.Values)
        {
            if (oGraphMLAttribute.IsForVertex == bIsVertex)
            {
                Object oDefaultAttributeValue;

                if ( oGraphMLAttribute.TryGetDefaultAttributeValue(
                    out oDefaultAttributeValue) )
                {
                    oEdgeOrVertex.SetValue(oGraphMLAttribute.Name,
                        oDefaultAttributeValue);
                }
            }
        }

        // For each "data" XML node specified for this edge or vertex, set
        // a metadata value on the edge or vertex.  This may overwrite a
        // previously-written default value.

        foreach (XmlNode oDataXmlNode in oNodeOrEdgeXmlNode.SelectNodes(
            GraphMLPrefix + ":data", oXmlNamespaceManager) )
        {
            String sGraphMLAttributeID =
                XmlUtil2.SelectRequiredSingleNodeAsString(oDataXmlNode, "@key",
                null);

            GraphMLAttribute oGraphMLAttribute;

            if ( !oGraphMLAttributeDictionary.TryGetValue(sGraphMLAttributeID,
                out oGraphMLAttribute) )
            {
                throw new XmlException(
                    "A \"data\" XML node has the key \"" + sGraphMLAttributeID
                    + "\", for which there is no corresponding \"key\" XML"
                    + " node."
                    );
            }

            oEdgeOrVertex.SetValue( oGraphMLAttribute.Name,
                oGraphMLAttribute.GetAttributeValue(oDataXmlNode) );
        }
    }

    //*************************************************************************
    //  Method: GraphMLNodeIDToVertex()
    //
    /// <summary>
    /// Retrieves the vertex referenced by the source or target attribute of
    /// an "edge" XML node.
    /// </summary>
    ///
    /// <param name="oEdgeXmlNode">
    /// The "edge" XML node.
    /// </param>
    ///
    /// <param name="sSourceOrTarget">
    /// "source" or "target".
    /// </param>
    ///
    /// <param name="oVertexDictionary">
    /// The key is the id attribute of the "node" XML node, and the value is
    /// the corresponding IVertex.
    /// </param>
    ///
    /// <returns>
    /// The referenced vertex.
    /// </returns>
    //*************************************************************************

    protected IVertex
    GraphMLNodeIDToVertex
    (
        XmlNode oEdgeXmlNode,
        String sSourceOrTarget,
        Dictionary<String, IVertex> oVertexDictionary
    )
    {
        Debug.Assert(oEdgeXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sSourceOrTarget) );
        Debug.Assert(oVertexDictionary != null);
        AssertValid();

        String sID = XmlUtil2.SelectRequiredSingleNodeAsString(oEdgeXmlNode,
            "@" + sSourceOrTarget, null);

        try
        {
            return ( oVertexDictionary[sID] );
        }
        catch (KeyNotFoundException)
        {
            throw new XmlException(
                "An \"edge\" XML node references the node id \"" + sID + "\","
                + " for which there is no corresponding \"node\" XML node."
                );
        }
    }

    //*************************************************************************
    //  Method: SaveGraphMLAttributeNames()
    //
    /// <summary>
    /// Saves the names of the GraphML-attributes on the graph.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph being loaded.
    /// </param>
    ///
    /// <param name="oGraphMLAttributeDictionary">
    /// The key is the id attribute of a "key" XML node, and the value is the
    /// corresponding GraphMLAttribute object.
    /// </param>
    ///
    /// <remarks>
    /// This method adds <see
    /// cref="ReservedMetadataKeys.GraphMLVertexAttributes" /> and <see
    /// cref="ReservedMetadataKeys.GraphMLEdgeAttributes" /> keys to the graph.
    /// </remarks>
    //*************************************************************************

    protected void
    SaveGraphMLAttributeNames
    (
        IGraph oGraph,
        Dictionary<String, GraphMLAttribute> oGraphMLAttributeDictionary
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oGraphMLAttributeDictionary != null);
        AssertValid();

        List<String> oVertexKeys = new List<String>();
        List<String> oEdgeKeys = new List<String>();

        foreach (GraphMLAttribute oGraphMLAttribute in
            oGraphMLAttributeDictionary.Values)
        {
            List<String> oListToAddTo = oGraphMLAttribute.IsForVertex ?
                oVertexKeys : oEdgeKeys;

            oListToAddTo.Add(oGraphMLAttribute.Name);
        }

        oGraph.SetValue( ReservedMetadataKeys.GraphMLVertexAttributes,
            oVertexKeys.ToArray() );

        oGraph.SetValue( ReservedMetadataKeys.GraphMLEdgeAttributes,
            oEdgeKeys.ToArray() );
    }

    //*************************************************************************
    //  Method: SaveGraphCore()
    //
    /// <summary>
    /// Saves graph data to a stream.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to save.
    /// </param>
    ///
    /// <param name="stream">
    /// Stream to save the graph data to.
    /// </param>
    ///
    /// <remarks>
    /// This method saves <paramref name="graph" /> to <paramref
    /// name="stream" />.  It does not close <paramref name="stream" />.
    ///
    /// <para>
    /// The arguments have already been checked for validity.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected override void
    SaveGraphCore
    (
        IGraph graph,
        Stream stream
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(stream != null);
        AssertValid();

        // This isn't implemented simply because it hasn't yet been a
        // requirement.

        throw new NotImplementedException();
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

     /// GraphML namespace.

     public const String GraphMLUri = "http://graphml.graphdrawing.org/xmlns";


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

     /// GraphML prefix.  Any prefix will do, so long as it is also included in
     /// every XPath expression.

     protected const String GraphMLPrefix = "g";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
