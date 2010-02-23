
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Adapters
{
//*****************************************************************************
//  Class: SimpleGraphAdapter
//
/// <summary>
/// Converts a graph to and from a simple two-name pair format.
/// </summary>
///
/// <remarks>
/// The two-name format consists of one line of text per edge, where each line
/// specifies the names of the vertices connected by the edge.  This is the
/// format of each line:
///
/// <code>
/// Vertex1Name{tab}Vertex2Name
/// </code>
///
/// <para>
/// The delimiter is a tab, and the vertex names can consist of any Unicode
/// characters, including leading, trailing, and embedded spaces.
/// </para>
///
/// <para>
/// This adapter can be used with directed graphs only.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class SimpleGraphAdapter : GraphAdapterBase, IGraphAdapter
{
    //*************************************************************************
    //  Constructor: SimpleGraphAdapter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleGraphAdapter" />
    /// class.
    /// </summary>
    //*************************************************************************

    public SimpleGraphAdapter()
    {
        // (Do nothing else.)

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

        // For now, support only directed graphs.  This may get modified in the
        // future to support undirected graphs as well, at which time some
        // mechanism must be added to tell this class the directedness of
        // the graph that is about to be created.  Possible solution: Add a
        // SimpleGraphAdapter.Directedness property.

        supportsDirected = true;
        supportsUndirected = false;
        supportsMixed = false;
    }

    //*************************************************************************
    //  Method: LoadGraphCore()
    //
    /// <summary>
    /// Creates a graph and loads it with graph data read from a <see
    /// cref="Stream" />.
    /// </summary>
    ///
    /// <param name="stream">
    /// <see cref="Stream" /> containing graph data.
    /// </param>
    ///
    /// <returns>
    /// A new graph loaded with graph data read from <paramref
    /// name="stream" />.
    /// </returns>
    ///
    /// <remarks>
    /// This method creates a graph, loads it with the graph data read from
    /// <paramref name="stream" />.  It does not close <paramref
    /// name="stream" />.
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
        Stream stream
    )
    {
        Debug.Assert(stream != null);
        AssertValid();

        // For now, support only directed graphs.

        IGraph oGraph = new Graph(GraphDirectedness.Directed);

        IVertexCollection oVertices = oGraph.Vertices;

        IEdgeCollection oEdges = oGraph.Edges;

        StreamReader oStreamReader = new StreamReader(stream, StreamEncoding);

        // Create a dictionary to keep track of the vertices that have been
        // added to the graph.  The key is the vertex name and the value is the
        // vertex.

        Dictionary<String, IVertex> oDictionary =
            new Dictionary<String, IVertex>();

        Int32 iLineNumber = 1;

        while (true)
        {
            String sLine = oStreamReader.ReadLine();

            if (sLine == null)
            {
                break;
            }

            // Skip empty lines.

            if (sLine.Trim().Length > 0)
            {
                // Parse the line.

                String [] asTokens = sLine.Split('\t');

                if (asTokens.Length != 2)
                {
                    OnLoadFormatError(sLine, iLineNumber, ExpectedFormat);
                }

                String sVertex1Name = asTokens[0];
                String sVertex2Name = asTokens[1];

                if (!VertexNameIsValid(sVertex1Name) ||
                    !VertexNameIsValid(sVertex2Name) )
                {
                    OnLoadFormatError(sLine, iLineNumber, ExpectedFormat);
                }

                // Retrieve or create the specified vertices.

                IVertex oVertex1 =
                    VertexNameToVertex(sVertex1Name, oVertices, oDictionary);

                IVertex oVertex2 =
                    VertexNameToVertex(sVertex2Name, oVertices, oDictionary);

                // Add an edge connecting the vertices.

                oEdges.Add(oVertex1, oVertex2, true);
            }

            iLineNumber++;
        }

        oDictionary.Clear();

        return (oGraph);
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
        Debug.Assert(graph.Directedness == GraphDirectedness.Directed);
        Debug.Assert(stream != null);
        AssertValid();

        const String MethodName = "SaveGraph";

        StreamWriter oStreamWriter = new StreamWriter(stream, StreamEncoding);

        // Loop through the graph's edges.

        foreach (IEdge oEdge in graph.Edges)
        {
            // Retrieve the edge's vertices.

            IVertex oVertex1, oVertex2;

            EdgeUtil.EdgeToVertices(oEdge, this.ClassName, MethodName,
                out oVertex1, out oVertex2);

            // Retrieve and format the edge names.

            String sVertex1Name = VertexToVertexName(oVertex1);
            String sVertex2Name = VertexToVertexName(oVertex2);

            oStreamWriter.WriteLine(

                "{0}\t{1}"
                ,
                sVertex1Name,
                sVertex2Name
                );
        }

        oStreamWriter.Flush();
    }

    //*************************************************************************
    //  Method: VertexNameToVertex()
    //
    /// <summary>
    /// Finds or creates a vertex given a vertex name.
    /// </summary>
    ///
    /// <param name="sVertexName">
    /// Name of the vertex to find or create.
    /// </param>
    ///
    /// <param name="oVertices">
    /// Vertex collection to add a new vertex to if a new vertex is created.
    /// </param>
    ///
    /// <param name="oDictionary">
    /// Dictionary of existing vertices.  The key is the vertex name and the
    /// value is the vertex.
    /// </param>
    ///
    /// <returns>
    /// The found or created vertex.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="oDictionary" /> contains a vertex named <paramref
    /// name="sVertexName" />, the vertex is returned.  Otherwise, a vertex is
    /// created, added to <paramref name="oVertices" />, added to <paramref
    /// name="oDictionary" />, and returned.
    /// </remarks>
    //*************************************************************************

    protected IVertex
    VertexNameToVertex
    (
        String sVertexName,
        IVertexCollection oVertices,
        Dictionary<String, IVertex> oDictionary
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sVertexName) );
        Debug.Assert(oVertices != null);
        Debug.Assert(oDictionary != null);
        AssertValid();

        IVertex oVertex;

        if ( !oDictionary.TryGetValue(sVertexName, out oVertex) )
        {
            oVertex = oVertices.Add();
            oVertex.Name = sVertexName;

            oDictionary.Add(sVertexName, oVertex);
        }

        return (oVertex);
    }

    //*************************************************************************
    //  Method: VertexToVertexName()
    //
    /// <summary>
    /// Gets a vertex's name.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// Vertex to get the name for.
    /// </param>
    ///
    /// <returns>
    /// The vertex's non-null, non-empty name.
    /// </returns>
    ///
    /// <remarks>
    /// An exception is thrown if <paramref name="oVertex" /> has a null or
    /// empty name.
    /// </remarks>
    //*************************************************************************

    protected String
    VertexToVertexName
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        String sVertexName = oVertex.Name;

        if ( String.IsNullOrEmpty(sVertexName) )
        {
            // Do not include the class or method name in the exception
            // message.  The message appears when the user attempts to save an
            // invalid graph, and the user doesn't care about class names.

            throw new FormatException( String.Format(
                "The vertex with the ID {0} has a null or empty name.  All"
                + " vertices must have a name when saving a graph with this"
                + " graph adapter."
                ,
                oVertex.ID.ToString(NodeXLBase.Int32Format)
                ) );
        }

        return (sVertexName);
    }

    //*************************************************************************
    //  Method: VertexNameIsValid()
    //
    /// <summary>
    /// Determines whether a vertex name is valid.
    /// </summary>
    ///
    /// <param name="sVertexName">
    /// Vertex name to check.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="sVertexName" /> is valid.
    /// </returns>
    //*************************************************************************

    protected Boolean
    VertexNameIsValid
    (
        String sVertexName
    )
    {
        AssertValid();

        return ( !String.IsNullOrEmpty(sVertexName) );
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
    //  Protected constants
    //*************************************************************************

    /// Expected format of each line.

    protected const String ExpectedFormat =
        "Vertex1Name{tab}Vertex2Name";

    /// Encoding to use when loading and saving graphs.

    protected static readonly Encoding StreamEncoding = Encoding.UTF8;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
