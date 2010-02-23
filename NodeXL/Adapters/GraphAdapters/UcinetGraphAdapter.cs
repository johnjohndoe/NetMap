
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Adapters
{
//*****************************************************************************
//  Class: UcinetGraphAdapter
//
/// <summary>
/// Converts a graph to and from a UCINET full matrix DL file.
/// </summary>
///
/// <remarks>
/// Use the <see cref="LoadGraph(String, GraphDirectedness)" /> overload
/// instead of one of the base-class overloads so that the directedness of the
/// UCINET file being loaded can be specified.
///
/// <para>
/// Before calling <see cref="GraphAdapterBase.SaveGraph(IGraph, String)" />,
/// duplicate edges must be merged and the results stored in the <see
/// cref="ReservedMetadataKeys.EdgeWeight" /> value on each vertex.
/// </para>
///
/// <para>
/// The only file format supported is the UCINET full matrix DL file, as
/// exported by UCINET version 6.223.  The file looks like this:
/// </para>
///
/// <code>
/// DL
/// N=11
/// FORMAT = FULLMATRIX DIAGONAL PRESENT
/// ROW LABELS:
/// "a"
/// "b"
/// "c"
/// "d"
/// "e"
/// "f"
/// "g"
/// "h"
/// "i"
/// "j"
/// "k"
/// COLUMN LABELS:
/// "a"
/// "b"
/// "c"
/// "d"
/// "e"
/// "f"
/// "g"
/// "h"
/// "i"
/// "j"
/// "k"
/// DATA:
/// 0 0 1 0 0 0 0 0 0 0 0
/// 0 0 1 0 0 0 0 0 0 0 0
/// 1 1 0 1 1 0 0 0 0 0 0
/// 0 0 1 0 1 1 0 0 0 0 0
/// 0 0 1 1 0 0 1 0 0 0 0
/// 0 0 0 1 0 0 1 1 0 0 0
/// 0 0 0 0 1 1 0 1 0 0 0
/// 0 0 0 0 0 1 1 0 1 0 0
/// 0 0 0 0 0 0 0 1 0 1 1
/// 0 0 0 0 0 0 0 0 1 0 0
/// 0 0 0 0 0 0 0 0 1 0 0
/// </code>
///
/// </remarks>
//*****************************************************************************

public class UcinetGraphAdapter : GraphAdapterBase, IGraphAdapter
{
    //*************************************************************************
    //  Constructor: UcinetGraphAdapter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="UcinetGraphAdapter" />
    /// class.
    /// </summary>
    //*************************************************************************

    public UcinetGraphAdapter()
    {
        m_eLoadedGraphDirectedness = GraphDirectedness.Undirected;

        AssertValid();
    }

    //*************************************************************************
    //  Method: LoadGraph()
    //
    /// <summary>
    /// Creates a graph of type <see cref="Graph" /> and specified
    /// directedness, and loads it with graph data read from a file.
    /// </summary>
    ///
    /// <param name="filename">
    /// Full path to the file containing graph data.
    /// </param>
    ///
    /// <param name="fileDirectedness">
    /// The directedness of the file's graph data.
    /// </param>
    ///
    /// <returns>
    /// A new <see cref="Graph" /> loaded with graph data read from <paramref
    /// name="filename" />.
    /// </returns>
    ///
    /// <remarks>
    /// This method creates a <see cref="Graph" /> and loads it with the graph
    /// data read from <paramref name="filename" />.
    /// </remarks>
    //*************************************************************************

    public IGraph
    LoadGraph
    (
        String filename,
        GraphDirectedness fileDirectedness
    )
    {
        AssertValid();

        // Save the directedness for use by LoadGraphCore(), which will get
        // called by base.LoadGraph().

        m_eLoadedGraphDirectedness = fileDirectedness;

        return ( base.LoadGraphFromFile(filename) );
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

        const String DataMarker = "DATA:";

        IGraph oGraph = new Graph(m_eLoadedGraphDirectedness);

        IVertexCollection oVertices = oGraph.Vertices;

        // The key is the zero-based vertex order as listed in the file, and
        // the value is the corresponding IVertex.

        Dictionary<Int32, IVertex> oVertexDictionary =
            new Dictionary<Int32, IVertex>();

        StreamReader oStreamReader = new StreamReader(stream, StreamEncoding);
        String sLine;
        Int32 iLineNumber = 0;

        // Look for the DL marker.

        if ( !TryReadLine(oStreamReader, out sLine, ref iLineNumber)
            || !sLine.StartsWith("DL") )
        {
            OnLoadFormatError(
                "The file must start with a \"DL\" marker."
                );
        }

        // Look for the vertex count.

        Int32 iVertices = 0;

        while ( TryReadLine(oStreamReader, out sLine, ref iLineNumber) )
        {
            const String Pattern = @"N=(?<Vertices>\d+)";

            Regex oRegex = new Regex(Pattern);
            Match oMatch = oRegex.Match(sLine);

            if (oMatch.Success
                &&
                MathUtil.TryParseCultureInvariantInt32(
                    oMatch.Groups["Vertices"].Value, out iVertices)
                )
            {
                goto LookForFullMatrixMarker;
            }
        }

        OnLoadFormatError(
            "The file must contain a vertex count in the format \"N=123\"."
            );

        LookForFullMatrixMarker:

        while ( TryReadLine(oStreamReader, out sLine, ref iLineNumber) )
        {
            if (sLine.IndexOf("FULLMATRIX") >= 0)
            {
                goto LookForLabels;
            }
        }

        OnLoadFormatError(
            "The file must contain a \"FULLMATRIX\" marker."
            );

        LookForLabels:

        // UCINET 6.223 uses both "ROW LABELS:" and "COLUMN LABELS:" with
        // identical label sets, although the UCINET Guide says that "LABELS:"
        // is also supported.  Look for either "ROW LABELS:" or "LABELS:".

        while ( TryReadLine(oStreamReader, out sLine, ref iLineNumber) )
        {
            if (sLine.IndexOf("LABELS:") >= 0)
            {
                goto ReadLabels;
            }

            if (sLine.IndexOf(DataMarker) >= 0)
            {
                // There are no labels.  Assign arbitrary vertex names.

                for (Int32 i = 0; i < iVertices; i++)
                {
                    AddLoadedVertex("Vertex " + (i + 1).ToString(), oVertices,
                        oVertexDictionary);
                }

                goto ReadMatrix;
            }
        }

        goto NoData;

        ReadLabels:

        while ( TryReadLine(oStreamReader, out sLine, ref iLineNumber) )
        {
            if (sLine.IndexOf(DataMarker) >= 0)
            {
                goto ReadMatrix;
            }

            if (sLine.IndexOf("COLUMN LABELS:") >= 0)
            {
                goto LookForData;
            }

            AddLoadedVertex(sLine.Replace("\"", ""), oVertices,
                oVertexDictionary);
        }

        LookForData:

        while ( TryReadLine(oStreamReader, out sLine, ref iLineNumber) )
        {
            if (sLine.IndexOf(DataMarker) >= 0)
            {
                goto ReadMatrix;
            }
        }

        NoData:

        OnLoadFormatError(
            "The file must contain a \"DATA:\" marker."
            );

        ReadMatrix:

        if (oVertices.Count != iVertices)
        {
            OnLoadFormatError( String.Format(

                "Expected number of vertex labels: {0}.  Actual number of"
                + " vertex labels: {1}."
                ,
                iVertices,
                oVertices.Count
                ) );
        }

        LoadGraphEdges(oGraph, oStreamReader, oVertexDictionary,
            ref iLineNumber);

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
        Debug.Assert(stream != null);
        AssertValid();

        // Get an array of non-isolated vertices.  Isolated vertices don't get
        // saved.

        List<IVertex> oNonIsolatedVertices =
            GraphUtil.GetNonIsolatedVertices(graph);

        Int32 iNonIsolatedVertices = oNonIsolatedVertices.Count;

        if (oNonIsolatedVertices.Count == 0)
        {
            OnSaveError("There are no edges to export.");
        }

        StreamWriter oStreamWriter = new StreamWriter(stream, StreamEncoding);

        // Write the graph metadata.

        oStreamWriter.WriteLine(
            "DL"
            + "\r\nN={0}"
            + "\r\nFORMAT = FULLMATRIX DIAGONAL PRESENT"
            ,
            iNonIsolatedVertices.ToString(CultureInfo.InvariantCulture)
            );

        foreach ( String sLabelHeader in new String [] {
            "ROW LABELS:", "COLUMN LABELS:"} )
        {
            oStreamWriter.WriteLine(sLabelHeader);

            foreach (IVertex oVertex in oNonIsolatedVertices)
            {
                oStreamWriter.WriteLine("\"" + oVertex.Name + "\"");
            }
        }

        // Now fill in the edge weights, row by row.

        oStreamWriter.WriteLine("DATA:");

        for (Int32 i = 0; i < iNonIsolatedVertices; i++)
        {
            IVertex oVertexI = oNonIsolatedVertices[i];

            for (Int32 j = 0; j < iNonIsolatedVertices; j++)
            {
                Double dEdgeWeight = EdgeUtil.GetEdgeWeight( oVertexI,
                    oNonIsolatedVertices[j] );

                oStreamWriter.Write(dEdgeWeight.ToString(
                    CultureInfo.InvariantCulture) + " ");
            }

            oStreamWriter.WriteLine();
        }

        oStreamWriter.Flush();
    }

    //*************************************************************************
    //  Method: AddLoadedVertex()
    //
    /// <summary>
    /// Adds a vertex to the graph being loaded.
    /// </summary>
    ///
    /// <param name="sVertexName">
    /// Name of the vertex to add.
    /// </param>
    ///
    /// <param name="oVertices">
    /// The graph's collection of vertices.
    /// </param>
    ///
    /// <param name="oVertexDictionary">
    /// The key is the zero-based vertex order as listed in the file, and the
    /// value is the corresponding IVertex.
    /// </param>
    //*************************************************************************

    protected void
    AddLoadedVertex
    (
        String sVertexName,
        IVertexCollection oVertices,
        Dictionary<Int32, IVertex> oVertexDictionary
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sVertexName) );
        Debug.Assert(oVertices != null);
        Debug.Assert(oVertexDictionary != null);
        AssertValid();

        IVertex oVertex = new Vertex();
        oVertex.Name = sVertexName;
        oVertices.Add(oVertex);
        oVertexDictionary.Add(oVertices.Count - 1, oVertex);
    }

    //*************************************************************************
    //  Method: LoadGraphEdges()
    //
    /// <summary>
    /// Loads a graph's edges.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph being loaded from a file.
    /// </param>
    ///
    /// <param name="oStreamReader">
    /// The StreamReader being used to read the file.
    /// </param>
    ///
    /// <param name="oVertexDictionary">
    /// The key is the zero-based vertex order as listed in the file, and the
    /// value is the corresponding IVertex.
    /// </param>
    ///
    /// <param name="iLineNumber">
    /// Line number of the current line.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    LoadGraphEdges
    (
        IGraph oGraph,
        StreamReader oStreamReader,
        Dictionary<Int32, IVertex> oVertexDictionary,
        ref Int32 iLineNumber
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oStreamReader != null);
        Debug.Assert(oVertexDictionary != null);
        AssertValid();

        IVertexCollection oVertices = oGraph.Vertices;
        Int32 iVertices = oVertices.Count;
        IEdgeCollection oEdges = oGraph.Edges;

        Boolean bGraphIsDirected =
            (m_eLoadedGraphDirectedness == GraphDirectedness.Directed);

        String sLine;

        for (Int32 i = 0; i < iVertices; i++)
        {
            if ( !TryReadLine(oStreamReader, out sLine, ref iLineNumber) )
            {
                OnLoadFormatError( String.Format(

                    "The \"DATA:\" section must contain {0} {1}."
                    ,
                    iVertices,
                    StringUtil.MakePlural("line", iVertices)
                    ) );
            }

            // This will remove any leading and trailing spaces from the line,
            // and it handles any number of spaces and tabs between the edge
            // weights.

            String [] asEdgeWeights = sLine.Split(new Char [] {' ', '\t'},
                StringSplitOptions.RemoveEmptyEntries);

            if (asEdgeWeights.Length != iVertices)
            {
                OnLoadFormatError( String.Format(

                    "Expected number of edge weights on line {0}: {1}.  Actual"
                    + " number of edge weights: {2}."
                    ,
                    iLineNumber,
                    iVertices,
                    asEdgeWeights.Length
                    ) );
            }

            // For directed graphs, read all edge weights.  For undirected
            // graphs, read only the diagonal and above.

            for (Int32 j = (bGraphIsDirected ? 0 : i); j < iVertices; j++)
            {
                Double dEdgeWeight;

                if ( !MathUtil.TryParseCultureInvariantDouble(
                        asEdgeWeights[j], out dEdgeWeight) )
                {
                    OnLoadFormatError( String.Format(

                        "Line {0} contains an invalid edge weight."
                        ,
                        iLineNumber
                        ) );
                }

                if (dEdgeWeight != 0)
                {
                    IEdge oEdge = oEdges.Add(oVertexDictionary[i],
                        oVertexDictionary[j], bGraphIsDirected);

                    oEdge.SetValue(ReservedMetadataKeys.EdgeWeight,
                        dEdgeWeight);
                }
            }
        }
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

        // m_eLoadedGraphDirectedness;
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Encoding to use when loading and saving graphs.
    ///
    /// UCINET seems to support ASCII only, so use that.

    protected static readonly Encoding StreamEncoding = Encoding.ASCII;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Directedness of loaded graphs.

    protected GraphDirectedness m_eLoadedGraphDirectedness;
}

}
