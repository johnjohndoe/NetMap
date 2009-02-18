
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Adapters
{
//*****************************************************************************
//  Class: PajekGraphAdapter
//
/// <summary>
/// Converts a graph to and from a subset of the Pajek format.
/// </summary>
///
/// <remarks>
/// This class supports only a subset of the Pajek format, which is roughly
/// described in the Pajek Reference Manual at
/// http://vlado.fmf.uni-lj.si/pub/networks/pajek/doc/pajekman.pdf.
///
/// <para>
/// This is the supported subset:
/// </para>
///
/// <code>
/// *vertices N
/// 1 "vertex 1 name" [x y z]
/// 2 "vertex 2 name" [x y z]
/// ...
/// N "vertex N name" [x y z]
///
/// *edges
/// Vi Vj weight
/// Vm Vn weight
/// ...
///
/// *edgeslist
/// Vi Vj Vk ...
/// Vm Vn ...
/// ...
///
/// *arcs
/// Vi Vj weight
/// Vm Vn weight
/// ...
///
/// *arcslist
/// Vi Vj Vk ...
/// Vm Vn ...
/// ...
/// </code>
///
/// <para>
/// The delimiter is any combination of spaces and tabs.
/// </para>
///
/// <para>
/// All sections are optional.  If there is at least one section, there must be
/// one and only one *vertices section and it must be the first section.  If
/// edges are specified with *edges and *edgeslist sections only, the graph
/// is considered undirected.  If edges are specified with *arcs and *arcslist
/// sections only, the graph is considered directed.  If there are both
/// undirected and directed edges, the graph is considered mixed.
/// </para>
///
/// <para>
/// Empty lines are ignored.  Lines starting with slash-asterisk are considered
/// comments and are skipped.
/// </para>
///
/// <para>
/// Everything is case-insensitive except the vertex names.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class PajekGraphAdapter : GraphAdapterBase, IGraphAdapter
{
    //*************************************************************************
    //  Constructor: PajekGraphAdapter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="PajekGraphAdapter" />
    /// class.
    /// </summary>
    //*************************************************************************

    public PajekGraphAdapter()
    {
        m_eCurrentSection = Sections.None;
        m_oVertexRegex = null;

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
        supportsMixed = true;
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

        StreamReader oStreamReader = new StreamReader(stream, StreamEncoding);

        // This array stores vertices as they are read.

        IVertex [] aoVertices = null;

        // Vertices parsed so far.

        Int32 iVerticesParsed = 0;

        // Number of vertices specified in the *vertices section.

        Int32 iVerticesExpected = 0;

        // Create lists to store undirected and directed edges as they are
        // read.

        List<PajekEdgeData> oUndirectedEdgeData = new List<PajekEdgeData>();
        List<PajekEdgeData> oDirectedEdgeData = new List<PajekEdgeData>();

        Int32 iLineNumber = 0;

        String [] asFields = null;

        while (true)
        {
            String sLine = oStreamReader.ReadLine();

            iLineNumber++;

            if (sLine == null)
            {
                break;
            }

            // Skip empty lines and comments.

            sLine = sLine.Trim();

            if ( sLine.Length == 0 || sLine.StartsWith("/*") )
            {
                continue;
            }

            if ( sLine.StartsWith("*") )
            {
                sLine = sLine.ToLower();

                if ( sLine.StartsWith("*vertices") )
                {
                    if (aoVertices != null)
                    {
                        OnLoadFormatError2(sLine, iLineNumber,
                            "There can't be more than one *vertices section."
                            );
                    }

                    asFields = SplitLine(sLine);

                    if (asFields.Length != 2 ||
                        !MathUtil.TryParseCultureInvariantInt32(
							asFields[1], out iVerticesExpected) ||
                        iVerticesExpected < 0)
                    {
                        OnLoadFormatError(sLine, iLineNumber, "*vertices N");
                    }

                    aoVertices = new IVertex[iVerticesExpected];

                    m_eCurrentSection = Sections.Vertices;
                }
                else if ( IsValidEdgeSection(sLine, iLineNumber, "*edges",
                    aoVertices) )
                {
                    m_eCurrentSection = Sections.Edges;
                }
                else if ( IsValidEdgeSection(sLine, iLineNumber, "*edgeslist",
                    aoVertices) )
                {
                    m_eCurrentSection = Sections.EdgesList;
                }
                else if ( IsValidEdgeSection(sLine, iLineNumber, "*arcs",
                    aoVertices) )
                {
                    m_eCurrentSection = Sections.Arcs;
                }
                else if ( IsValidEdgeSection(sLine, iLineNumber, "*arcslist",
                    aoVertices) )
                {
                    m_eCurrentSection = Sections.ArcsList;
                }
                else
                {
                    // Skip unrecognized sections.

                    m_eCurrentSection = Sections.None;
                }
            }
            else
            {
                switch (m_eCurrentSection)
                {
                    case Sections.None:

                        // Skip the line.

                        break;

                    case Sections.Vertices:

                        ParseVertex(sLine, iLineNumber, aoVertices,
                            ref iVerticesParsed);

                        break;

                    case Sections.Edges:

                        ParseEdge(sLine, iLineNumber, iVerticesParsed,
                            oUndirectedEdgeData);

                        break;

                    case Sections.EdgesList:

                        ParseEdgeList(sLine, iLineNumber, iVerticesParsed,
                            oUndirectedEdgeData);

                        break;

                    case Sections.Arcs:

                        ParseEdge(sLine, iLineNumber, iVerticesParsed,
                            oDirectedEdgeData);

                        break;

                    case Sections.ArcsList:

                        ParseEdgeList(sLine, iLineNumber, iVerticesParsed,
                            oDirectedEdgeData);

                        break;

                    default:

                        Debug.Assert(false);
                        break;
                }
            }
        }

        if (aoVertices != null && iVerticesParsed != iVerticesExpected)
        {
            throw new FormatException( String.Format(

                "The *vertices section specified {0} but contained {1}."
                ,
                VertexCountToString(iVerticesExpected),
                VertexCountToString(iVerticesParsed)
                ) );
        }

        IGraph oGraph = CreateGraph(graphFactory, aoVertices,
            oUndirectedEdgeData, oDirectedEdgeData);

        oUndirectedEdgeData.Clear();
        oDirectedEdgeData.Clear();

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

        // The Pajek format requires coordinates to be between 0 and 1.0.  Get
        // a Matrix that will transform the vertex locations to this range.

        RectangleF oCurrentBoundingRectangle =
            LayoutUtil.GetGraphBoundingRectangle(graph);

        RectangleF oNewBoundingRectangle =
            new RectangleF(PointF.Empty, new SizeF(1.0F, 1.0F) );

        Matrix oRectangleTransformation =
            LayoutUtil.GetRectangleTransformation(
                oCurrentBoundingRectangle, oNewBoundingRectangle
                );

        // Create a dictionary to keep track of vertices.  The keys are vertex
        // IDs and the values are the one-based vertex numbers used by the
        // Pajek format.

        Dictionary<Int32, Int32> oVertexIDToNumber = 
            new Dictionary<Int32, Int32>();

        IVertexCollection oVertices = graph.Vertices;

        StreamWriter oStreamWriter = new StreamWriter(stream, StreamEncoding);

        // Add the *vertices section.

        oStreamWriter.WriteLine(

            "*vertices {0}"
            ,
            oVertices.Count
            );

        Int32 iVertexNumber = 1;

        foreach (IVertex oVertex in oVertices)
        {
            // Format:
            //
            // 1 "vertex 1 name" x y z

            // Transform the vertex location.

            PointF oTransformedLocation = LayoutUtil.TransformPointF(
                oVertex.Location, oRectangleTransformation);

            // Limit the range in case of rounding errors.

            Single fX = Math.Max(0F, oTransformedLocation.X);
            fX = Math.Min(1F, fX);

            Single fY = Math.Max(0F, oTransformedLocation.Y);
            fY = Math.Min(1F, fY);

            oStreamWriter.WriteLine(

                "{0} \"{1}\" {2:N6} {3:N6} 0"
                ,
                iVertexNumber,
                oVertex.Name,
                fX,
                fY
                );

            oVertexIDToNumber.Add(oVertex.ID, iVertexNumber);

            iVertexNumber++;
        }

        IEdgeCollection oEdges = graph.Edges;

        GraphDirectedness eDirectedness = graph.Directedness;

        Boolean bSectionNameWritten = false;

        if (eDirectedness != GraphDirectedness.Directed)
        {
            // If appropriate, add the *edges section, which specifies
            // undirected edges.

            foreach (IEdge oEdge in oEdges)
            {
                // The graph could be mixed.

                if (oEdge.IsDirected)
                {
                    continue;
                }

                if (!bSectionNameWritten)
                {
                    oStreamWriter.WriteLine("*edges");

                    bSectionNameWritten = true;
                }

                WriteEdge(oEdge, oVertexIDToNumber, oStreamWriter);
            }
        }

        bSectionNameWritten = false;

        if (eDirectedness != GraphDirectedness.Undirected)
        {
            // If appropriate, add the *arcs section, which specifies
            // directed edges.

            foreach (IEdge oEdge in oEdges)
            {
                // The graph could be mixed.

                if (!oEdge.IsDirected)
                {
                    continue;
                }

                if (!bSectionNameWritten)
                {
                    oStreamWriter.WriteLine("*arcs");

                    bSectionNameWritten = true;
                }

                WriteEdge(oEdge, oVertexIDToNumber, oStreamWriter);
            }
        }

        oStreamWriter.Flush();
    }

    //*************************************************************************
    //  Method: IsValidEdgeSection()
    //
    /// <summary>
    /// Determines whether a line is a valid section marker for an *edges,
    /// *edgeslist, *arcs, or *arcslist section.
    /// </summary>
    ///
    /// <param name="sLine">
    /// Line read from the file.
    /// </param>
    ///
    /// <param name="iLineNumber">
    /// One-based line number of <paramref name="sLine" />.
    /// </param>
    ///
    /// <param name="sSectionName">
    /// Section name to check for.  Sample: "*edges".
    /// </param>
    ///
    /// <param name="aoVertices">
    /// Vertices created so far, or null if there are no vertices.
    /// </param>
    ///
    /// <returns>
    /// true if the line is a valid section marker.
    /// </returns>
    ///
    /// <remarks>
    /// If the line is in the wrong format, an exception is thrown.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    IsValidEdgeSection
    (
        String sLine,
        Int32 iLineNumber,
        String sSectionName,
        IVertex [] aoVertices
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sLine) );
        Debug.Assert(iLineNumber > 0);
        Debug.Assert( !String.IsNullOrEmpty(sSectionName) );

        if (sLine != sSectionName)
        {
            return (false);
        }

        if (aoVertices == null)
        {
            OnLoadFormatError2(sLine, iLineNumber, String.Format(

                "There can't be an {0} section without a *vertices section."
                ,
                sSectionName
                ) );
        }

        return (true);
    }

    //*************************************************************************
    //  Method: ParseVertex()
    //
    /// <summary>
    /// Parses a vertex line.
    /// </summary>
    ///
    /// <param name="sLine">
    /// Line read from the file.
    /// </param>
    ///
    /// <param name="iLineNumber">
    /// One-based line number of <paramref name="sLine" />.
    /// </param>
    ///
    /// <param name="aoVertices">
    /// The array length is equal to the expected number of vertices.  The
    /// first <paramref name="iVerticesParse" /> elements are set to vertices
    /// that have already been parsed; the remaining elements are null.
    /// </param>
    ///
    /// <param name="iVerticesParsed">
    /// Number of parsed vertices in <paramref name="aoVertices" />.  Gets
    /// incremented.
    /// </param>
    ///
    /// <remarks>
    /// This method attempts to parse <paramref name="sLine" />, which is a
    /// line in the *vertices section.  If it succeeds, a new vertex is added
    /// to <paramref name="aoVertices" /> and <paramref
    /// name="iVerticesParsed" /> is incremented.  An exception is thrown
    /// otherwise.
    /// </remarks>
    //*************************************************************************

    protected void
    ParseVertex
    (
        String sLine,
        Int32 iLineNumber,
        IVertex [] aoVertices,
        ref Int32 iVerticesParsed
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sLine) );
        Debug.Assert(iLineNumber > 0);
        Debug.Assert(aoVertices != null);
        Debug.Assert(iVerticesParsed >= 0);

        if (iVerticesParsed == aoVertices.Length)
        {
            throw new FormatException( String.Format(

                "There are too many vertices in the *vertices section, which"
                + " specified only {0} on the *vertices line."
                ,
                VertexCountToString(aoVertices.Length)
                ) );
        }

        const String ExpectedFormat = "N \"vertex name\" [x y z]";

        const String Pattern =

            // Start of line.

            @"^"

            // Vertex number.

            + @"(?<VertexNumber>\d{1,10})"

            // Whitespace.

            + @"\s+"
            
            // Name, with optional quotes.

            + @"(""(?<Name>.+)""|(?<Name>\S+))"

            // Whitespace.
            
            + @"\s*"
            
            // Optional coordinates.

            + @"((?<X>[\d.-]{1,10})\s+(?<Y>[\d.-]{1,10})\s+(?<Z>[\d.-]{1,10}))?"
            ;

        if (m_oVertexRegex == null)
        {
            m_oVertexRegex = new Regex(Pattern);
        }

        Match oMatch = m_oVertexRegex.Match(sLine);
        Int32 iVertexNumber = -1;

        if (!oMatch.Success
            ||
            !MathUtil.TryParseCultureInvariantInt32(
				oMatch.Groups["VertexNumber"].Value, out iVertexNumber)
            )
        {
            OnLoadFormatError(sLine, iLineNumber, ExpectedFormat);
        }

        if (iVertexNumber != iVerticesParsed + 1)
        {
            OnLoadFormatError2(sLine, iLineNumber,
                "Vertices must be numbered consecutively starting at 1."
                );
        }

        String sName = oMatch.Groups["Name"].Value;

        Debug.Assert(sName.Length > 0);

        IVertex oVertex = ( new VertexFactory() ).CreateVertex();
        oVertex.Name = sName;
        aoVertices[iVerticesParsed] = oVertex;
        iVerticesParsed++;

        String sX = oMatch.Groups["X"].Value;

        if (sX.Length > 0)
        {
            Single fX = 0;
            Single fY = 0;

            if (!MathUtil.TryParseCultureInvariantSingle(sX, out fX) ||

                !MathUtil.TryParseCultureInvariantSingle(
					oMatch.Groups["Y"].Value, out fY) ||

                fX < 0 || fX > 1.0F ||
                fY < 0 || fY > 1.0F
                )
            {
                OnLoadFormatError2(sLine, iLineNumber,
                    "Vertex coordinates must be between 0 and 1.0."
                    );
            }

            oVertex.Location = new PointF(fX, fY);
        }
    }

    //*************************************************************************
    //  Method: ParseEdge()
    //
    /// <summary>
    /// Parses an edge line.
    /// </summary>
    ///
    /// <param name="sLine">
    /// Line read from the file.
    /// </param>
    ///
    /// <param name="iLineNumber">
    /// One-based line number of <paramref name="sLine" />.
    /// </param>
    ///
    /// <param name="iVertices">
    /// Number of vertices in the graph.  Must be greater than 0.
    /// </param>
    ///
    /// <param name="oEdgeData">
    /// List to add the new edge data to.
    /// </param>
    ///
    /// <remarks>
    /// This method attempts to parse <paramref name="sLine" />, which is a
    /// line in the *edges or *arcs section.  If it succeeds, a new
    /// PajekEdgeData struct is added to <paramref name="oEdgeData" />.  An
    /// exception is thrown otherwise.
    /// </remarks>
    //*************************************************************************

    protected void
    ParseEdge
    (
        String sLine,
        Int32 iLineNumber,
        Int32 iVertices,
        List<PajekEdgeData> oEdgeData
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sLine) );
        Debug.Assert(iLineNumber > 0);
        Debug.Assert(iVertices > 0);
        Debug.Assert(oEdgeData != null);

        const String ExpectedFormat = "Vi Vj weight";

        String [] asFields = SplitLine(sLine);

        Int32 iFirstVertexNumber = 0;
        Int32 iSecondVertexNumber = 0;
        Single fWeight = 0;

        if (asFields.Length < 3 ||

            !MathUtil.TryParseCultureInvariantInt32(
				asFields[0], out iFirstVertexNumber) ||

            !MathUtil.TryParseCultureInvariantInt32(
				asFields[1], out iSecondVertexNumber) ||

            !MathUtil.TryParseCultureInvariantSingle(asFields[2], out fWeight)
            )
        {
            OnLoadFormatError(sLine, iLineNumber, ExpectedFormat);
        }

        CheckVertexNumber(iFirstVertexNumber, sLine, iLineNumber, iVertices);
        CheckVertexNumber(iSecondVertexNumber, sLine, iLineNumber, iVertices);

        oEdgeData.Add( new PajekEdgeData(
            iFirstVertexNumber, iSecondVertexNumber, fWeight) );
    }

    //*************************************************************************
    //  Method: ParseEdgeList()
    //
    /// <summary>
    /// Parses an edge list line.
    /// </summary>
    ///
    /// <param name="sLine">
    /// Line read from the file.
    /// </param>
    ///
    /// <param name="iLineNumber">
    /// One-based line number of <paramref name="sLine" />.
    /// </param>
    ///
    /// <param name="iVertices">
    /// Number of vertices in the graph.
    /// </param>
    ///
    /// <param name="oEdgeData">
    /// List to add the new edge data to.
    /// </param>
    ///
    /// <remarks>
    /// This method attempts to parse <paramref name="sLine" />, which is a
    /// line in the *edgeslist or *arcslist section.  If it succeeds, a new
    /// PajekEdgeData struct is added to <paramref name="oEdgeData" />.  An
    /// exception is thrown otherwise.
    /// </remarks>
    //*************************************************************************

    protected void
    ParseEdgeList
    (
        String sLine,
        Int32 iLineNumber,
        Int32 iVertices,
        List<PajekEdgeData> oEdgeData
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sLine) );
        Debug.Assert(iLineNumber > 0);
        Debug.Assert(iVertices >= 0);
        Debug.Assert(oEdgeData != null);

        const String ExpectedFormat = "Vi Vj Vk ...";

        String [] asFields = SplitLine(sLine);

        Int32 iFields = asFields.Length;

        if (iFields < 2)
        {
            OnLoadFormatError(sLine, iLineNumber, ExpectedFormat);
        }

        Int32 iFirstVertexNumber = 0;

        for (Int32 i = 0; i < iFields; i++)
        {
            Int32 iVertexNumber = 0;

            if ( !MathUtil.TryParseCultureInvariantInt32(
				asFields[i], out iVertexNumber) )
            {
                OnLoadFormatError(sLine, iLineNumber, ExpectedFormat);
            }

            CheckVertexNumber(iVertexNumber, sLine, iLineNumber, iVertices);

            if (i == 0)
            {
                iFirstVertexNumber = iVertexNumber;
            }
            else
            {
                oEdgeData.Add( new PajekEdgeData(
                    iFirstVertexNumber, iVertexNumber, DefaultEdgeWeight) );
            }
        }
    }

    //*************************************************************************
    //  Method: SplitLine()
    //
    /// <summary>
    /// Splits a line read from a file.
    /// </summary>
    ///
    /// <param name="sLine">
    /// Line read from the file.
    /// </param>
    ///
    /// <returns>
    /// An array of string tokens.
    /// </returns>
    //*************************************************************************

    protected String []
    SplitLine
    (
        String sLine
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sLine) );

        Char [] FieldDelimiters = new Char[] {' ', '\t'};

        return ( sLine.Split(
            FieldDelimiters, StringSplitOptions.RemoveEmptyEntries) );
    }

    //*************************************************************************
    //  Method: CheckVertexNumber()
    //
    /// <summary>
    /// Checks whether a vertex number is valid.
    /// </summary>
    ///
    /// <param name="iVertexNumber">
    /// Vertex number read from the file.
    /// </param>
    ///
    /// <param name="sLine">
    /// Line read from the file.
    /// </param>
    ///
    /// <param name="iLineNumber">
    /// One-based line number of <paramref name="sLine" />.
    /// </param>
    ///
    /// <param name="iVertices">
    /// Number of vertices in the graph.
    /// </param>
    ///
    /// <remarks>
    /// An exception is thrown if <paramref name="iVertexNumber" /> is invalid.
    /// </remarks>
    //*************************************************************************

    protected void
    CheckVertexNumber
    (
        Int32 iVertexNumber,
        String sLine,
        Int32 iLineNumber,
        Int32 iVertices
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sLine) );
        Debug.Assert(iLineNumber > 0);
        Debug.Assert(iVertices >= 0);

        if (iVertexNumber < 1)
        {
            OnLoadFormatError2(sLine, iLineNumber,
                "Vertex numbers must be greater than 0."
                );
        }
        if (iVertexNumber > iVertices)
        {
            OnLoadFormatError2(sLine, iLineNumber,
                "Vertex numbers can't be greater than the number of vertices."
                );
        }
    }

    //*************************************************************************
    //  Method: CreateGraph()
    //
    /// <summary>
    /// Creates a graph given collections of vertices and edges.
    /// </summary>
    ///
    /// <param name="oGraphFactory">
    /// Object that can create a graph.
    /// </param>
    ///
    /// <param name="aoVertices">
    /// Vertices to add to the graph, or null if there are no vertices.
    /// </param>
    ///
    /// <param name="oUndirectedEdgeData">
    /// List of undirected edges to add to the graph.
    /// </param>
    ///
    /// <param name="oDirectedEdgeData">
    /// List of directed edges to add to the graph.
    /// </param>
    ///
    /// <returns>
    /// A new graph.
    /// </returns>
    //*************************************************************************

    protected IGraph
    CreateGraph
    (
        IGraphFactory oGraphFactory,
        IVertex [] aoVertices,
        List<PajekEdgeData> oUndirectedEdgeData,
        List<PajekEdgeData> oDirectedEdgeData
    )
    {
        Debug.Assert(oGraphFactory != null);
        Debug.Assert(oUndirectedEdgeData != null);
        Debug.Assert(oDirectedEdgeData != null);

        GraphDirectedness eDirectedness = GraphDirectedness.Undirected;

        Int32 iVertices = 0;
        Int32 iUndirectedEdges = oUndirectedEdgeData.Count;
        Int32 iDirectedEdges = oDirectedEdgeData.Count;

        if (iUndirectedEdges > 0 && iDirectedEdges > 0)
        {
            eDirectedness = GraphDirectedness.Mixed;
        }
        else if (iDirectedEdges > 0)
        {
            eDirectedness = GraphDirectedness.Directed;
        }

        IGraph oGraph = new Graph(eDirectedness);

        IVertexCollection oVertices = oGraph.Vertices;
        IEdgeCollection oEdges = oGraph.Edges;

        if (aoVertices != null)
        {
            // Populate the vertex collection.

            foreach (IVertex oVertex in aoVertices)
            {
                oVertices.Add(oVertex);
            }

            iVertices = aoVertices.Length;
        }

        // Populate the edges collection.

        foreach (PajekEdgeData oEdgeData in oUndirectedEdgeData)
        {
            AddEdgeToGraph(oEdgeData, oEdges, aoVertices, false);
        }

        foreach (PajekEdgeData oEdgeData in oDirectedEdgeData)
        {
            AddEdgeToGraph(oEdgeData, oEdges, aoVertices, true);
        }

        return (oGraph);
    }

    //*************************************************************************
    //  Method: AddEdgeToGraph()
    //
    /// <summary>
    /// Adds an edge to a graph's edge collection.
    /// </summary>
    ///
    /// <param name="oEdgeData">
    /// Contains data for the new edge.
    /// </param>
    ///
    /// <param name="oEdges">
    /// Graph's edge collection.
    /// </param>
    ///
    /// <param name="aoVertices">
    /// Graph's vertices.  Can't be null.
    /// </param>
    ///
    /// <param name="bDirected">
    /// true if the edge is directed.
    /// </param>
    //*************************************************************************

    protected void
    AddEdgeToGraph
    (
        PajekEdgeData oEdgeData,
        IEdgeCollection oEdges,
        IVertex [] aoVertices,
        Boolean bDirected
    )
    {
        Debug.Assert(oEdges != null);
        Debug.Assert(aoVertices != null);

        Int32 iVertices = aoVertices.Length;

        Int32 iFirstVertexNumber = oEdgeData.FirstVertexNumber;
        Int32 iSecondVertexNumber = oEdgeData.SecondVertexNumber;

        Debug.Assert(iFirstVertexNumber >= 1);
        Debug.Assert(iFirstVertexNumber <= iVertices);
        Debug.Assert(aoVertices[iFirstVertexNumber - 1] != null);

        Debug.Assert(iSecondVertexNumber >= 1);
        Debug.Assert(iSecondVertexNumber <= iVertices);
        Debug.Assert(aoVertices[iSecondVertexNumber - 1] != null);

        IEdge oEdge = oEdges.Add(aoVertices[iFirstVertexNumber - 1],
            aoVertices[iSecondVertexNumber - 1], bDirected);

        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, oEdgeData.Weight);
    }

    //*************************************************************************
    //  Method: WriteEdge()
    //
    /// <summary>
    /// Writes an edge to a file.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// Edge to write.
    /// </param>
    ///
    /// <param name="oVertexIDToNumber">
    /// Dictionary that keeps track of vertices.  The keys are vertex IDs and
    /// the values are the one-based vertex numbers used by the Pajek format.
    /// </param>
    ///
    /// <param name="oStreamWriter">
    /// StreamWriter to write to.
    /// </param>
    //*************************************************************************

    protected void
    WriteEdge
    (
        IEdge oEdge,
        Dictionary<Int32, Int32> oVertexIDToNumber,
        StreamWriter oStreamWriter
    )
    {
        Debug.Assert(oEdge != null);
        Debug.Assert(oVertexIDToNumber != null);
        Debug.Assert(oStreamWriter != null);
        AssertValid();

        const String MethodName = "SaveGraph";

        // Retrieve the edge's vertices.

        IVertex oVertex1, oVertex2;

        EdgeUtil.EdgeToVertices(oEdge, this.ClassName, MethodName,
            out oVertex1, out oVertex2);

        Int32 iVertex1ID = oVertex1.ID;
        Int32 iVertex2ID = oVertex2.ID;

        // Retrieve the vertex numbers.

        Int32 iVertex1Number, iVertex2Number;

        if ( !oVertexIDToNumber.TryGetValue(iVertex1ID, out iVertex1Number)
            ||
            !oVertexIDToNumber.TryGetValue(iVertex2ID, out iVertex2Number)
            )
        {
            throw new InvalidOperationException( String.Format(
                "The edge with the ID {0} has a vertex that is not in the"
                + " graph's Vertices collection."
                ,
                oEdge.ID.ToString(NodeXLBase.Int32Format)
                ) );
        }

        // Retrieve the edge weight, if it exists.

        Object oWeight = null;
        Single fWeight = DefaultEdgeWeight;

        if ( oEdge.TryGetValue(
            ReservedMetadataKeys.EdgeWeight, typeof(Single), out oWeight) )
        {
            fWeight = (Single)oWeight;
        }

        // Format:
        //
        // Vi Vj weight

        oStreamWriter.WriteLine(

            "{0} {1} {2}"
            ,
            iVertex1Number,
            iVertex2Number,
            fWeight
            );
    }

    //*************************************************************************
    //  Enum: Section
    //
    /// <summary>
    /// Specifies a section in the file.
    /// </summary>
    //*************************************************************************

    protected enum
    Sections
    {
        /// <summary>
        /// None.
        /// </summary>

        None,

        /// <summary>
        /// The *vertices section is being parsed.
        /// </summary>

        Vertices,

        /// <summary>
        /// The *edges section is being parsed.
        /// </summary>

        Edges,

        /// <summary>
        /// The *edgeslist section is being parsed.
        /// </summary>

        EdgesList,

        /// <summary>
        /// The *arcs section is being parsed.
        /// </summary>

        Arcs,

        /// <summary>
        /// The *arcslist section is being parsed.
        /// </summary>

        ArcsList,
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

        // m_eCurrentSection
        // m_oVertexRegex = null
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Encoding to use when loading and saving graphs.

    protected static readonly Encoding StreamEncoding = Encoding.UTF8;

    /// Default edge weight to use when an edge weight isn't specified.

    protected const Single DefaultEdgeWeight = 1.0F;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Indicates which section is currently being parsed.

    protected Sections m_eCurrentSection;

    /// Regex used to parse vertex lines.

    protected Regex m_oVertexRegex;
}

}
