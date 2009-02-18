
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Class: Graph
//
/// <summary>
/// Provides a default graph implementation.
/// </summary>
///
/// <remarks>
/// A graph has a collection of <see cref="Vertices" /> and a collection of
/// <see cref="Edges" /> that connect the <see cref="Vertices" />.  The <see
/// cref="Directedness" /> property specifies the type of edges that can be
/// added to the graph.
///
/// <para>
/// This class can be used as-is in many graphing applications.  You can also
/// customize it via inheritance, or implement your own graph class from
/// scratch.  All graph classes must implement the <see cref="IGraph" />
/// interface.  If you implement a graph class, you may also want to implement
/// a corresponding <see cref="IGraphFactory" /> class.
/// </para>
///
/// </remarks>
///
/// <example>
/// The following code creates a graph, populates it with vertices and edges,
/// and lists the graph's contents.
///
/// <code>
///
/// // Create a graph with mixed directedness and no restrictions.
/// 
/// IGraph oGraph = new Graph();
/// 
/// // Add vertices to the graph.  Save the vertices in a local array for
/// // later use.
/// 
/// const Int32 Vertices = 10;
/// 
/// IVertexCollection oVertices = oGraph.Vertices;
/// 
/// IVertex[] aoVertices = new IVertex[Vertices];
/// 
/// for (Int32 i = 0; i &lt; Vertices; i++)
/// {
///     aoVertices[i] = oVertices.Add();
/// }
/// 
/// // Add a set of edges that connect the first vertex to each of the
/// // other vertices.
/// 
/// Int32 iEdges = Vertices - 1;
/// 
/// IEdge[] aoEdges = new IEdge[iEdges];
/// 
/// IEdgeCollection oEdges = oGraph.Edges;
/// 
/// for (Int32 i = 0; i &lt; iEdges; i++)
/// {
///     aoEdges[i] = oEdges.Add( aoVertices[0], aoVertices[i + 1] );
/// }
/// 
/// // List the graph's contents.
/// 
/// Console.Write( oGraph.ToString("D") );
///
/// </code>
///
/// <para>
/// This produces the following output:
/// </para>
///
/// <code>
/// Microsoft.NodeXL.Core.Graph
/// ID = 1
/// Name = [null]
/// Tag = [null]
/// Values = 0 key/value pairs
/// Directedness = Mixed
/// PerformExtraValidations = False
/// Restrictions = None
/// Vertices = 10 vertices
///     ID = 1
///     ID = 2
///     ID = 3
///     ID = 4
///     ID = 5
///     ID = 6
///     ID = 7
///     ID = 8
///     ID = 9
///     ID = 10
/// Edges = 9 edges
///     ID = 9
///     ID = 8
///     ID = 7
///     ID = 6
///     ID = 5
///     ID = 4
///     ID = 3
///     ID = 2
///     ID = 1
/// </code>
///
/// </example>
///
/// <seealso cref="IGraph" />
//*****************************************************************************

public class Graph : GraphVertexEdgeBase, IGraph
{
    //*************************************************************************
    //  Constructor: Graph()
    //
    /// <overloads>
    /// Static constructor for the Graph class.
    /// </overloads>
    //*************************************************************************

    static Graph()
    {
        m_oIDGenerator = new IDGenerator();
    }

    //*************************************************************************
    //  Constructor: Graph()
    //
    /// <overloads>
    /// Initializes a new instance of the <see cref="Graph" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see cref="Graph" /> class with a
    /// mixed directedness and no restrictions.
    /// </summary>
    ///
    /// <remarks>
    /// The <see cref="Directedness" /> property is set to <see
    /// cref="GraphDirectedness.Mixed" />.  The <see cref="Restrictions" />
    /// property is set to <see cref="GraphRestrictions.None" />.
    /// </remarks>
    //*************************************************************************

    public
    Graph()
    :
    this(GraphDirectedness.Mixed)
    {
        AssertValid();
    }

    //*************************************************************************
    //  Constructor: Graph()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="Graph" /> class with a
    /// specified directedness and no restrictions.
    /// </summary>
    ///
    /// <param name="directedness">
    /// Specifies the type of edges that can be added to the graph.
    /// </param>
    ///
    /// <remarks>
    /// The <see cref="Restrictions" /> property is set to <see
    /// cref="GraphRestrictions.None" />.
    /// </remarks>
    //*************************************************************************

    public
    Graph
    (
        GraphDirectedness directedness
    )
    :
    this(directedness, GraphRestrictions.None)
    {
        AssertValid();
    }

    //*************************************************************************
    //  Constructor: Graph()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="Graph" /> class with
    /// specified directedness and restrictions.
    /// </summary>
    ///
    /// <param name="directedness">
    /// Specifies the type of edges that can be added to the graph.
    /// </param>
    ///
    /// <param name="restrictions">
    /// Specifies restrictions imposed by the graph.
    /// </param>
    //*************************************************************************

    public
    Graph
    (
        GraphDirectedness directedness,
        GraphRestrictions restrictions
    )
    :
    base( m_oIDGenerator.GetNextID() )
    {
        const String MethodName = "Constructor";

        this.ArgumentChecker.CheckArgumentIsDefined(
            MethodName, "directedness", directedness,
            typeof(GraphDirectedness) );

        CheckRestrictions(restrictions, MethodName, "restrictions");

        m_bPerformExtraValidations = false;
        m_eDirectedness = directedness;
        m_eRestrictions = restrictions;

        m_oVertexCollection = new VertexCollection(this);

        m_oVertexCollection.VertexAdded +=
            new VertexEventHandler(this.VertexCollection_VertexAdded);

        m_oEdgeCollection = new EdgeCollection(this);

        m_oEdgeCollection.EdgeAdded +=
            new EdgeEventHandler(this.EdgeCollection_EdgeAdded);

        AssertValid();
    }

    //*************************************************************************
    //  Property: Vertices
    //
    /// <summary>
    /// Gets the graph's collection of vertices.
    /// </summary>
    ///
    /// <value>
    /// A collection of vertices, as an <see cref="IVertexCollection" />.  The
    /// collection contains zero or more objects that implement <see
    /// cref="IVertex" />.
    /// </value>
    //*************************************************************************

    public IVertexCollection
    Vertices
    {
        get
        {
            AssertValid();

            return (m_oVertexCollection);
        }
    }

    //*************************************************************************
    //  Property: Edges
    //
    /// <summary>
    /// Gets the graph's collection of edges.
    /// </summary>
    ///
    /// <value>
    /// A collection of edges, as an <see cref="IEdgeCollection" />.  The
    /// collection contains zero or more objects that implement <see
    /// cref="IEdge" /> and that connect vertices in this graph.
    /// </value>
    //*************************************************************************

    public IEdgeCollection
    Edges
    {
        get
        {
            AssertValid();

            return (m_oEdgeCollection);
        }
    }

    //*************************************************************************
    //  Property: Directedness
    //
    /// <summary>
    /// Gets a value that indicates the type of edges that can be added to the
    /// graph.
    /// </summary>
    ///
    /// <value>
    /// A <see cref="GraphDirectedness" /> value.
    /// </value>
    ///
    /// <remarks>
    /// The directedness of a graph is specified when the graph is created and
    /// cannot be changed.
    /// </remarks>
    //*************************************************************************

    public GraphDirectedness
    Directedness
    {
        get
        {
            AssertValid();

            return (m_eDirectedness);
        }
    }

    //*************************************************************************
    //  Property: Restrictions
    //
    /// <summary>
    /// Gets an ORed set of flags that specify restrictions imposed by the
    /// graph.
    /// </summary>
    ///
    /// <value>
    /// An ORed combination of <see cref="GraphRestrictions" /> flags.
    /// </value>
    ///
    /// <remarks>
    /// The graph's restrictions are specified when the graph is created and
    /// cannot be changed.
    ///
    /// <para>
    /// <see cref="HasRestrictions" /> can be used to determine whether the
    /// graph imposes a specified restriction.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="HasRestrictions" />
    //*************************************************************************

    public GraphRestrictions
    Restrictions
    {
        get
        {
            AssertValid();

            return (m_eRestrictions);
        }
    }

    //*************************************************************************
    //  Property: PerformExtraValidations
    //
    /// <summary>
    /// Gets or sets a flag specifying whether extra but possibly slow
    /// validations are performed.
    /// </summary>
    ///
    /// <value>
    /// true to perform extra validations, false otherwise.  The default value
    /// is false.  A value of true can dramatically slow graph operations.
    /// </value>
    ///
    /// <remarks>
    /// When this property is set to true, the graph performs extra validations
    /// during certain operations.  For example, when a vertex is added to the
    /// <see cref="Vertices" /> collection, the graph checks whether the vertex
    /// already exists in the collection and throws an exception if it does.
    ///
    /// <para>
    /// Important note: The extra validations can be very slow, and therefore
    /// this property should be set to true only during development or after an
    /// unexpected problem occurs and the problem needs to be diagnosed.
    /// For example, checking whether a vertex already exists in the <see
    /// cref="Vertices" /> collection is an O(n) operation, where n is the
    /// number of vertices in the graph.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    PerformExtraValidations
    {
        get
        {
            AssertValid();

            return (m_bPerformExtraValidations);
        }

        set
        {
            m_bPerformExtraValidations = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: HasRestrictions
    //
    /// <summary>
    /// Gets a flag that indicates whether the graph imposes specified
    /// restrictions.
    /// </summary>
    ///
    /// <param name="restrictions">
    /// An ORed combination of one or more <see cref="GraphRestrictions" />
    /// flags.
    /// </param>
    ///
    /// <returns>
    /// true if the graph imposes all of the restrictions specified by
    /// <paramref name="restrictions" />.
    /// </returns>
    ///
    /// <remarks>
    /// The graph's restrictions are specified when the graph is created and
    /// cannot be changed.
    ///
    /// <para>
    /// Use <see cref="Restrictions" /> to return all of the graph's
    /// restrictions.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <example>
    /// The following code determines whether a graph prohibits parallel edges.
    ///
    /// <code>
    /// Boolean bNoParallelEdges =
    ///     oGraph.HasRestrictions(GraphRestrictions.NoParallelEdges);
    /// </code>
    ///
    /// </example>
    ///
    /// <seealso cref="Restrictions" />
    //*************************************************************************

    public Boolean
    HasRestrictions
    (
        GraphRestrictions restrictions
    )
    {
        AssertValid();

        const String MethodName = "HasRestrictions";
        const String ParameterName = "restrictions";

        CheckRestrictions(restrictions, MethodName, ParameterName);

        Debug.Assert(GraphRestrictions.None == 0);

        // GraphRestrictions.None is zero, so it has to be treated as a special
        // case.

        if (restrictions == GraphRestrictions.None)
        {
            return (m_eRestrictions == GraphRestrictions.None);
        }

        return ( (restrictions & m_eRestrictions) == restrictions );
    }

    //*************************************************************************
    //  Method: Clone()
    //
    /// <overloads>
    /// Creates a copy of the graph.
    /// </overloads>
    ///
    /// <summary>
    /// Creates a copy of the graph, making the copy the same type as the
    /// original.
    /// </summary>
    ///
    /// <param name="copyMetadataValues">
    /// If true, the key/value pairs that were set with <see
    /// cref="IMetadataProvider.SetValue" /> are copied to the new graph,
    /// vertices, and edges.  (This is a shallow copy.  The objects pointed to
    /// by the original values are NOT cloned.)  If false, the key/value pairs
    /// are not copied.
    /// </param>
    ///
    /// <param name="copyTag">
    /// If true, the <see cref="IMetadataProvider.Tag" /> properties on the new
    /// graph, vertices, and edges are set to the same value as in the original
    /// objects.  (This is a shallow copy.  The objects pointed to by the
    /// original <see cref="IMetadataProvider.Tag" /> properties are NOT
    /// cloned.)  If false, the <see cref="IMetadataProvider.Tag "/>
    /// properties on the new graph, vertices, and edges are set to null.
    /// </param>
    ///
    /// <returns>
    /// The copy of the graph, as an <see cref="IGraph" />.
    /// </returns>
    ///
    /// <remarks>
    /// The new graph, vertices, and edges are of the same types as the
    /// originals.  Their <see cref="IIdentityProvider.Name" />s are set to
    /// the same values as the originals', but they are assigned new <see
    /// cref="IIdentityProvider.ID" />s.
    /// </remarks>
    //*************************************************************************

    public IGraph
    Clone
    (
        Boolean copyMetadataValues,
        Boolean copyTag
    )
    {
        AssertValid();

        return ( Clone(copyMetadataValues, copyTag, new GraphFactory(),
            new VertexFactory(), new EdgeFactory() ) );
    }

    //*************************************************************************
    //  Method: Clone()
    //
    /// <summary>
    /// Creates a copy of the graph, making the copy a specified type.
    /// </summary>
    ///
    /// <param name="copyMetadataValues">
    /// If true, the key/value pairs that were set with <see
    /// cref="IMetadataProvider.SetValue" /> are copied to the new graph,
    /// vertices, and edges.  (This is a shallow copy.  The objects pointed to
    /// by the original values are NOT cloned.)  If false, the key/value pairs
    /// are not copied.
    /// </param>
    ///
    /// <param name="copyTag">
    /// If true, the <see cref="IMetadataProvider.Tag" /> properties on the new
    /// graph, vertices, and edges are set to the same value as in the original
    /// objects.  (This is a shallow copy.  The objects pointed to by the
    /// original <see cref="IMetadataProvider.Tag" /> properties are NOT
    /// cloned.)  If false, the <see cref="IMetadataProvider.Tag "/>
    /// properties on the new graph, vertices, and edges are set to null.
    /// </param>
    ///
    /// <param name="newGraphFactory">
    /// Object that can create a graph.
    /// </param>
    ///
    /// <param name="newVertexFactory">
    /// Object that can create vertices.
    /// </param>
    ///
    /// <param name="newEdgeFactory">
    /// Object that can create edges.
    /// </param>
    ///
    /// <returns>
    /// The copy of the graph, as an <see cref="IGraph" />.
    /// </returns>
    ///
    /// <remarks>
    /// The new graph, vertices, and edges are created using <paramref
    /// name="newGraphFactory" />, <paramref name="newVertexFactory" />, and
    /// <paramref name="newEdgeFactory" />.  Their <see
    /// cref="IIdentityProvider.Name" />s are set to the same values as the
    /// originals', but they are assigned new <see
    /// cref="IIdentityProvider.ID" />s.
    /// </remarks>
    //*************************************************************************

    public IGraph
    Clone
    (
        Boolean copyMetadataValues,
        Boolean copyTag,
        IGraphFactory newGraphFactory,
        IVertexFactory newVertexFactory,
        IEdgeFactory newEdgeFactory
    )
    {
        AssertValid();

        const String MethodName = "Clone";
        ArgumentChecker oArgumentChecker = this.ArgumentChecker;

        oArgumentChecker.CheckArgumentNotNull(
            MethodName, "newGraphFactory", newGraphFactory);

        oArgumentChecker.CheckArgumentNotNull(
            MethodName, "newVertexFactory", newVertexFactory);

        oArgumentChecker.CheckArgumentNotNull(
            MethodName, "newEdgeFactory", newEdgeFactory);

        IGraph oNewGraph = newGraphFactory.CreateGraph(
            m_eDirectedness, m_eRestrictions);

        oNewGraph.PerformExtraValidations = m_bPerformExtraValidations;

        // Copy the base-class fields to the new edge.

        this.CopyTo(oNewGraph, copyMetadataValues, copyTag);

        // The vertices need to be copied to the new graph.  Loop through the
        // vertices in this original graph.

        IVertexCollection oNewVertices = oNewGraph.Vertices;

        foreach (IVertex oOriginalVertex in m_oVertexCollection)
        {
            IVertex oNewVertex = oOriginalVertex.Clone(
                copyMetadataValues, copyTag, newVertexFactory);

            // To make it easier to copy the edges in this original graph,
            // temporarily store the ID of the new vertex in the Tag of the
            // original vertex.  Save the Tag so it can be restored later.

            oOriginalVertex.Tag =
                new VertexMapper(oOriginalVertex.Tag, oNewVertex);

            oNewVertices.Add(oNewVertex);
        }

        // The edges need to be copied to the new graph.  Loop through the
        // edges in this original graph.

        IEdgeCollection oNewEdges = oNewGraph.Edges;

        foreach (IEdge oOriginalEdge in m_oEdgeCollection)
        {
            // Get the original edge's vertices.

            IVertex oOriginalVertex1, oOriginalVertex2;

            EdgeUtil.EdgeToVertices(oOriginalEdge, this.ClassName, MethodName,
                out oOriginalVertex1, out oOriginalVertex2);

            // Retrieve the VertexMapper objects that were temporarily stored
            // in the vertices' Tags.

            Debug.Assert(oOriginalVertex1.Tag is VertexMapper);
            Debug.Assert(oOriginalVertex2.Tag is VertexMapper);

            VertexMapper oVertexMapper1 = (VertexMapper)oOriginalVertex1.Tag;
            VertexMapper oVertexMapper2 = (VertexMapper)oOriginalVertex2.Tag;

            // Get the new vertices that correspond to the original edge's
            // vertices.

            IVertex oNewVertex1 = oVertexMapper1.NewVertex;
            IVertex oNewVertex2 = oVertexMapper2.NewVertex;

            // Copy the original edge, connecting the new vertices in the
            // process.

            IEdge oNewEdge = oOriginalEdge.Clone(copyMetadataValues, copyTag,
                newEdgeFactory, oNewVertex1, oNewVertex2,
                oOriginalEdge.IsDirected);

            oNewEdges.Add(oNewEdge);
        }

        // Restore the original vertices' Tags.

        foreach (IVertex oOriginalVertex in m_oVertexCollection)
        {
            Debug.Assert(oOriginalVertex.Tag is VertexMapper);

            VertexMapper oVertexMapper = (VertexMapper)oOriginalVertex.Tag;

            oOriginalVertex.Tag = oVertexMapper.OriginalVertexTag;
        }

        return (oNewGraph);
    }

    //*************************************************************************
    //  Event: EdgeAdded
    //
    /// <summary>
    /// Occurs when an edge is added to the <see cref="Edges" /> collection.
    /// </summary>
    //*************************************************************************

    public event EdgeEventHandler
    EdgeAdded;


    //*************************************************************************
    //  Event: VertexAdded
    //
    /// <summary>
    /// Occurs when a vertex is added to the <see cref="Vertices" />
    /// collection.
    /// </summary>
    //*************************************************************************

    public event VertexEventHandler
    VertexAdded;


    //*************************************************************************
    //  Method: AppendPropertiesToString()
    //
    /// <summary>
    /// Appends the derived class's public property values to a String.
    /// </summary>
    ///
    /// <param name="oStringBuilder">
    /// Object to append to.
    /// </param>
    ///
    /// <param name="iIndentationLevel">
    /// Current indentation level.  Level 0 is "no indentation."
    /// </param>
    ///
    /// <param name="sFormat">
    /// The format to use, either "G", "P", or "D".  See <see
    /// cref="NodeXLBase.ToString()" /> for details.
    /// </param>
    ///
    /// <remarks>
    /// This method calls <see cref="ToStringUtil.AppendPropertyToString(
    /// StringBuilder, Int32, String, Object, Boolean)" /> for each of the
    /// derived class's public properties.  It is used in the implementation of
    /// <see cref="NodeXLBase.ToString()" />.
    /// </remarks>
    //*************************************************************************

    protected override void
    AppendPropertiesToString
    (
        StringBuilder oStringBuilder,
        Int32 iIndentationLevel,
        String sFormat
    )
    {
        AssertValid();
        Debug.Assert(oStringBuilder != null);
        Debug.Assert(iIndentationLevel >= 0);
        Debug.Assert( !String.IsNullOrEmpty(sFormat) );
        Debug.Assert(sFormat == "G" || sFormat == "P" || sFormat == "D");

        base.AppendPropertiesToString(
            oStringBuilder, iIndentationLevel, sFormat);

        if (sFormat == "G")
        {
            return;
        }

        ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
            "Directedness", m_eDirectedness);

        ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
            "PerformExtraValidations", this.PerformExtraValidations);

        ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel, 
            "Restrictions", this.Restrictions);

        ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
            "Vertices", String.Empty, false);

        ToStringUtil.AppendVerticesToString(oStringBuilder, iIndentationLevel,
            sFormat, this.Vertices);

        ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
            "Edges", String.Empty, false);

        ToStringUtil.AppendEdgesToString(oStringBuilder, iIndentationLevel,
            sFormat, this.Edges);
    }

    //*************************************************************************
    //  Method: CheckRestrictions()
    //
    /// <summary>
    /// Checks whether a value contains only flags specified in the <see
    /// cref="GraphRestrictions" /> enumeration.
    /// </summary>
    ///
    /// <param name="eRestrictions">
    /// Value to check.
    /// </param>
    ///
    /// <param name="sMethodName">
    /// Name of the method calling this method.
    /// </param>
    ///
    /// <param name="sParameterName">
    /// Name of the parameter to which <paramref name="eRestrictions" /> was
    /// passed.
    /// </param>
    ///
    /// <remarks>
    /// An exception is thrown if <paramref name="eRestrictions" /> contains
    /// flags not defined in the <see cref="GraphRestrictions" /> enumeration.
    /// </remarks>
    //*************************************************************************

    protected void
    CheckRestrictions
    (
        GraphRestrictions eRestrictions,
        String sMethodName,
        String sParameterName
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sMethodName) );
        Debug.Assert( !String.IsNullOrEmpty(sParameterName) );

        if ( (eRestrictions & ~GraphRestrictions.All) != 0 )
        {
            this.ArgumentChecker.ThrowArgumentException(
                sMethodName, sParameterName,
                
                "Must be an ORed combination of the flags in the"
                + " GraphRestrictions enumeration."
                );
        }
    }

    //*************************************************************************
    //  Method: VertexCollection_VertexAdded()
    //
    /// <summary>
    /// Handles the VertexAdded event on the m_oVertexCollection object.
    /// </summary>
    ///
    /// <param name="oSender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="oVertexEventArgs">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    VertexCollection_VertexAdded
    (
        Object oSender,
        VertexEventArgs oVertexEventArgs
    )
    {
        Debug.Assert(oSender != null);
        Debug.Assert(oVertexEventArgs != null);

        // Forward the event.

        VertexEventHandler oVertexAdded = this.VertexAdded;

        if (oVertexAdded != null)
        {
            oVertexAdded(this, oVertexEventArgs);
        }
    }

    //*************************************************************************
    //  Method: EdgeCollection_EdgeAdded()
    //
    /// <summary>
    /// Handles the EdgeAdded event on the m_oEdgeCollection object.
    /// </summary>
    ///
    /// <param name="oSender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="oEdgeEventArgs">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    EdgeCollection_EdgeAdded
    (
        Object oSender,
        EdgeEventArgs oEdgeEventArgs
    )
    {
        Debug.Assert(oSender != null);
        Debug.Assert(oEdgeEventArgs != null);

        // Forward the event.

        EdgeEventHandler oEdgeAdded = this.EdgeAdded;

        if (oEdgeAdded != null)
        {
            oEdgeAdded(this, oEdgeEventArgs);
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

        // m_bPerformExtraValidations
        Debug.Assert(m_oVertexCollection != null);
        Debug.Assert(m_oEdgeCollection != null);

        Debug.Assert( Enum.IsDefined(
            typeof(GraphDirectedness), m_eDirectedness) );

        Debug.Assert( Enum.IsDefined(
            typeof(GraphRestrictions), m_eRestrictions) );
    }


    //*************************************************************************
    //  Private fields
    //*************************************************************************

    /// Generates unique IDs.

    private static IDGenerator m_oIDGenerator;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// true to perform extra but possibly slow validations.

    protected Boolean m_bPerformExtraValidations;

    /// The graph's collection of vertices.

    protected VertexCollection m_oVertexCollection;

    /// The graph's collection of edges.

    protected EdgeCollection m_oEdgeCollection;

    /// Indicates the type of edges that can be added to the graph.

    protected GraphDirectedness m_eDirectedness;

    /// Specifies restrictions imposed by the graph.

    protected GraphRestrictions m_eRestrictions;


    //*************************************************************************
    //  Nested struct: VertexMapper
    //
    /// <summary>
    /// Helper struct used for cloning the graph's vertices.
    /// </summary>
    ///
    /// <remarks>
    /// The <see cref="Graph.Clone(Boolean, Boolean, IGraphFactory,
    /// IVertexFactory, IEdgeFactory)" /> method uses this structure to map
    /// vertices in the original graph to the corresponding vertices in the
    /// in the new graph.
    ///
    /// <para>
    /// This struct is nested within the <see cref="Graph" /> class.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected struct VertexMapper
    {
        //*********************************************************************
        //  Constructor: VertexMapper()
        //
        /// <summary>
        /// Initializes a new instance of the VertexMapper class.
        /// </summary>
        ///
        /// <param name="oOriginalVertexTag">
        /// Value of the <see cref="IMetadataProvider.Tag" /> property of the
        /// original vertex.  Can be null.
        /// </param>
        ///
        /// <param name="oNewVertex">
        /// New vertex that corresponds to the original vertex.  Can't be null.
        /// </param>
        //*********************************************************************

        public VertexMapper
        (
            Object oOriginalVertexTag,
            IVertex oNewVertex
        )
        {
            m_oOriginalVertexTag = oOriginalVertexTag;
            m_oNewVertex = oNewVertex;

            AssertValid();
        }

        //*********************************************************************
        //  Property: OriginalVertexTag
        //
        /// <summary>
        /// Gets the value of the <see cref="IMetadataProvider.Tag" /> property
        /// of the original vertex.
        /// </summary>
        ///
        /// <value>
        /// The value of the <see cref="IMetadataProvider.Tag" /> property of
        /// the original vertex.  Can be null.
        /// </value>
        //*********************************************************************

        public Object
        OriginalVertexTag
        {
            get
            {
                AssertValid();

                return (m_oOriginalVertexTag);
            }
        }

        //*********************************************************************
        //  Property: NewVertex
        //
        /// <summary>
        /// Gets the new vertex that corresponds to the original vertex.
        /// </summary>
        ///
        /// <value>
        /// The new vertex that corresponds to the original vertex.  Can't be
        /// null.
        /// </value>
        //*********************************************************************

        public IVertex
        NewVertex
        {
            get
            {
                AssertValid();

                return (m_oNewVertex);
            }
        }


        //*********************************************************************
        //  Method: AssertValid()
        //
        /// <summary>
        /// Asserts if the object is in an invalid state.  Debug-only.
        /// </summary>
        //*********************************************************************

        [Conditional("DEBUG")]

        public void
        AssertValid()
        {
            // m_oOriginalVertexTag
            Debug.Assert(m_oNewVertex != null);
        }


        //*********************************************************************
        //  Private fields
        //*********************************************************************

        /// Value of the Tag property in the original vertex.  Can be null.

        private Object m_oOriginalVertexTag;

        /// New vertex that corresponds to the original vertex.  Can't be null.

        private IVertex m_oNewVertex;
    }

}
}
