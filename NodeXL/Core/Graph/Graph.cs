
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Class: Graph
//
/// <summary>
/// Represents a graph.
/// </summary>
///
/// <remarks>
/// A graph has a collection of <see cref="Vertices" /> and a collection of
/// <see cref="Edges" /> that connect the <see cref="Vertices" />.  The <see
/// cref="Directedness" /> property specifies the type of edges that can be
/// added to the graph.
/// </remarks>
///
/// <example>
/// The following code creates a graph and populates it with vertices and
/// edges.
///
/// <code>
///
/// // Create a graph with mixed directedness.
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
    /// mixed directedness.
    /// </summary>
    ///
    /// <remarks>
    /// The <see cref="Directedness" /> property is set to <see
    /// cref="GraphDirectedness.Mixed" />.
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
    /// Initializes a new instance of the <see cref="Graph" /> class with
    /// specified directedness.
    /// </summary>
    ///
    /// <param name="directedness">
    /// Specifies the type of edges that can be added to the graph.
    /// </param>
    //*************************************************************************

    public
    Graph
    (
        GraphDirectedness directedness
    )
    :
    base( m_oIDGenerator.GetNextID() )
    {
        const String MethodName = "Constructor";

        this.ArgumentChecker.CheckArgumentIsDefined(
            MethodName, "directedness", directedness,
            typeof(GraphDirectedness) );

        m_eDirectedness = directedness;

        m_oVertexCollection = new VertexCollection(this);
        m_oEdgeCollection = new EdgeCollection(this);

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
    //  Method: Clone()
    //
    /// <summary>
    /// Creates a copy of the graph.
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
    /// The new graph, vertices, and edges have the same <see
    /// cref="IIdentityProvider.Name" /> values as the originals, but they are
    /// assigned new <see cref="IIdentityProvider.ID" />s.
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

        const String MethodName = "Clone";

        IGraph oNewGraph = new Graph(m_eDirectedness);

        // Copy the base-class fields to the new edge.

        this.CopyTo(oNewGraph, copyMetadataValues, copyTag);

        // The vertices need to be copied to the new graph.  Loop through the
        // vertices in this original graph.

        IVertexCollection oNewVertices = oNewGraph.Vertices;

        foreach (IVertex oOriginalVertex in m_oVertexCollection)
        {
            IVertex oNewVertex = oOriginalVertex.Clone(
                copyMetadataValues, copyTag);

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
                oNewVertex1, oNewVertex2,
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

        Debug.Assert(m_oVertexCollection != null);
        Debug.Assert(m_oEdgeCollection != null);
        // m_eDirectedness
    }


    //*************************************************************************
    //  Private fields
    //*************************************************************************

    /// Generates unique IDs.

    private static IDGenerator m_oIDGenerator;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The graph's collection of vertices.

    protected VertexCollection m_oVertexCollection;

    /// The graph's collection of edges.

    protected EdgeCollection m_oEdgeCollection;

    /// Indicates the type of edges that can be added to the graph.

    protected GraphDirectedness m_eDirectedness;


    //*************************************************************************
    //  Nested struct: VertexMapper
    //
    /// <summary>
    /// Helper struct used for cloning the graph's vertices.
    /// </summary>
    ///
    /// <remarks>
    /// The <see cref="Graph.Clone" /> method uses this structure to map
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
