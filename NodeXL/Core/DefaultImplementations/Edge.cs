
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Class: Edge
//
/// <summary>
/// Provides a default edge implementation.
/// </summary>
///
/// <remarks>
/// An edge is a connection between two vertices in the same graph.  An edge
///	can be directed or undirected.  A directed edge has a front and a back.
///
/// <para>
/// An edge always connects two vertices.  Although an edge can be created
/// before it is added to a graph, it can't be created without specifying the
/// vertices it connects.  An edge can be added only to the graph that contains
/// the edge's vertices.
/// </para>
///
/// <para>
/// An <see cref="Edge" /> can be created via its constructor and then added to
/// a graph via Graph.Edges.<see cref="EdgeCollection.Add(IEdge)" />, or
/// created and added to a graph at the same time via Graph.Edges.<see
/// cref="EdgeCollection.Add(IVertex, IVertex, Boolean)" />.
/// </para>
///
/// <para>
/// An edge is immutable, meaning that its vertices can't be removed.  An edge
/// can be removed from a graph and disposed of, but once the edge is removed,
/// the edge is unusable.  Attempting to access the edge's properties or
/// methods will lead to unpredictable results.
/// </para>
///
/// <para>
///	This class can be used as-is in many graphing applications.  You can also
///	customize it via inheritance, or implement your own edge class from
///	scratch.  All edge classes must implement the <see cref="IEdge" />
///	interface.  If you implement an edge class, you may also want to implement
/// a corresponding <see cref="IEdgeFactory" /> class.
/// </para>
///
/// </remarks>
///
///	<example>
///	The following code creates two Vertex objects, adds them to an existing
///	undirected graph, and connects them with an Edge object.
///
/// <code>
/// // Create the Vertex objects.
///
/// IVertex oVertex1 = new Vertex();
/// IVertex oVertex2 = new Vertex();
///
/// // Add the Vertex objects to the graph.
///
/// IVertexCollection oVertices = oUndirectedGraph.Vertices;
/// oVertices.Add(oVertex1);
/// oVertices.Add(oVertex2);
///
/// // Connect the Vertex objects with an Edge object.
///
/// IEdge oEdge = new Edge(oVertex1, oVertex2);
///
/// // Add the Edge object to the graph.
///
/// oUndirectedGraph.Edges.Add(oEdge);
/// </code>
///
///	</example>
///
/// <seealso cref="IEdge" />
/// <seealso cref="IEdgeCollection" />
//*****************************************************************************

public class Edge : GraphVertexEdgeBase, IEdge
{
    //*************************************************************************
    //  Constructor: Edge()
    //
    /// <overloads>
    /// Static constructor for the <see cref="Edge" /> class.
    /// </overloads>
    //*************************************************************************

    static Edge()
    {
		m_oIDGenerator = new IDGenerator();
    }

    //*************************************************************************
    //  Constructor: Edge()
    //
    /// <summary>
    /// Initializes a new instance of the Edge class.
    /// </summary>
	///
    /// <param name="vertex1">
	///	The edge's first vertex.  The vertex must have already been added to
	/// the graph to which the new edge will be added.
    /// </param>
    ///
    /// <param name="vertex2">
	///	The edge's second vertex.  The vertex must have already been added to
	/// the graph to which the new edge will be added.
    /// </param>
    ///
    /// <param name="isDirected">
	///	If true, <paramref name="vertex1" /> is the edge's back vertex and
	///	<paramref name="vertex2" /> is the edge's front vertex.  If false, the
	/// edge is undirected.
    /// </param>
    //*************************************************************************

    public Edge
	(
		IVertex vertex1,
		IVertex vertex2,
		Boolean isDirected
	)
	:
	base ( m_oIDGenerator.GetNextID() )
    {
		const String MethodName = "Constructor";

		// Check the vertices.

		CheckVertexArgument(vertex1, "vertex1");
		CheckVertexArgument(vertex2, "vertex2");

		if (vertex1.ParentGraph != vertex2.ParentGraph)
		{
			this.ArgumentChecker.ThrowArgumentException(MethodName, "vertex2",

				"vertex1 and vertex2 have been added to different graphs.  An"
				+ " edge can't connect vertices from different graphs."
				);
		}

		m_oVertex1 = vertex1;
		m_oVertex2 = vertex2;
		m_bIsDirected = isDirected;

		AssertValid();
    }

    //*************************************************************************
    //  Property: ParentGraph
    //
    /// <summary>
    /// Gets the graph that owns the edge.
    /// </summary>
    ///
    /// <value>
    /// The graph that owns the edge, as an <see cref="IGraph" />.
    /// </value>
	///
	/// <remarks>
	/// This property is never null.  If the edge hasn't yet been added to a
	/// graph, the parent graph is obtained from the edge's vertices.  Because
	/// an edge can only be added to the graph that contains the edge's
	/// vertices, the vertices' parent graph is always the same as the edge's
	/// parent graph.
	/// </remarks>
    //*************************************************************************

    public IGraph
    ParentGraph
    {
        get
		{
			AssertValid();

			const String PropertyName = "ParentGraph";

			// Check whether the edge and its vertices have been removed from
			// the graph.  (An edge shouldn't be accessed after it has been
			// removed from the graph.)
			//
			// This check isn't foolproof, because the user can remove an edge
			// but not the edge's vertices from a graph.  Removing vertices
			// from the graph, however, automatically removes the edges
			// incident to those vertices.

			if (m_oVertex1.ParentGraph == null)
			{
				Debug.Assert(false);

				throw new InvalidOperationException( String.Format(

					"{0}.{1}: The edge has been removed from its parent"
					+ " graph and is no longer valid.  Do not attempt to"
					+ " access an edge that has been removed from its graph."
					,
					this.ClassName,
					PropertyName
					) );
			}

			if (m_oVertex1.ParentGraph != m_oVertex2.ParentGraph)
			{
				// This should never occur.

				Debug.Assert(false);

				throw new ApplicationException( String.Format(

					"{0}.{1}: The edge has vertices that are not in the same"
					+ " graph."
					,
					this.ClassName,
					PropertyName
					) );
			}

			return (m_oVertex1.ParentGraph);
		}
    }

    //*************************************************************************
    //  Property: IsDirected
    //
    /// <summary>
    /// Gets a value indicating whether the edge is directed.
    /// </summary>
    ///
    /// <value>
	///	true if the edge is directed, false if not.
    /// </value>
	///
	/// <remarks>
	///	A directed edge has a front and a back.
	///
	/// <para>
	/// This property is set when the edge is created by <see
	/// cref="IEdgeFactory.CreateEdge" />.
	/// </para>
	///
	/// </remarks>
    //*************************************************************************

    public Boolean
    IsDirected
    {
        get
		{
			AssertValid();

			return (m_bIsDirected);
		}
    }

    //*************************************************************************
    //  Property: Vertices
    //
    /// <summary>
	///	Gets the vertices to which the edge is connected.
    /// </summary>
    ///
    /// <value>
	///	An array of two vertices.
    /// </value>
	///
	/// <remarks>
	/// The order of the returned vertices is the same order that was specified
	/// when the edge was created by <see cref="IEdgeFactory.CreateEdge" />.
	///
	/// <para>
	/// If <see cref="IsDirected" /> is true, the first vertex in the array is
	/// the back vertex and the second vertex is the front vertex.  (The back
	/// and front vertices are also available via the <see cref="BackVertex" />
	/// and <see cref="FrontVertex" /> properties.)
	/// </para>
	///
	/// </remarks>
	///
	/// <seealso cref="IsDirected" />
	/// <seealso cref="FrontVertex" />
	/// <seealso cref="BackVertex" />
	/// <seealso cref="IEdgeFactory.CreateEdge" />
    //*************************************************************************

    public IVertex []
    Vertices
    {
        get
		{
			AssertValid();

			return ( new IVertex[] {m_oVertex1, m_oVertex2} );
		}
    }

    //*************************************************************************
    //  Property: BackVertex
    //
    /// <summary>
	///	Gets the back vertex to which the directed edge is connected.
    /// </summary>
    ///
    /// <value>
	///	The edge's back vertex, as an <see cref="IVertex" />.
    /// </value>
	///
	/// <remarks>
	///	This property can be read only if <see cref="IsDirected" /> is true.
	///	If <see cref="IsDirected" /> is false, an exception is thrown.
	///
	///	<para>
	///	The back vertex is also available as the first vertex in the array
	///	returned by <see cref="Vertices" />.
	///	</para>
	///
	/// <para>
	/// The edge's vertices are set when the edge is created by <see
	/// cref="IEdgeFactory.CreateEdge" />.
	/// </para>
	///
	/// </remarks>
	///
	/// <seealso cref="IsDirected" />
	/// <seealso cref="Vertices" />
	/// <seealso cref="FrontVertex" />
    //*************************************************************************

	[ DebuggerBrowsable(DebuggerBrowsableState.Never) ]

    public IVertex
    BackVertex
    {
		get
		{
			AssertValid();

			// Throw an exception if the edge is not directed.

			CheckIsDirected(true);

			return (m_oVertex1);
		}
    }

    //*************************************************************************
    //  Property: FrontVertex
    //
    /// <summary>
	///	Gets the front vertex to which the directed edge is connected.
    /// </summary>
    ///
    /// <value>
	///	The edge's front vertex, as an <see cref="IVertex" />.
    /// </value>
	///
	/// <remarks>
	///	This property can be read only if <see cref="IsDirected" /> is true.
	///	If <see cref="IsDirected" /> is false, an exception is thrown.
	///
	///	<para>
	///	The front vertex is also available as the second vertex in the array
	///	returned by <see cref="Vertices" />.
	///	</para>
	///
	/// <para>
	/// The edge's vertices are set when the edge is created by <see
	/// cref="IEdgeFactory.CreateEdge" />.
	/// </para>
	///
	/// </remarks>
	///
	/// <seealso cref="IsDirected" />
	/// <seealso cref="Vertices" />
	/// <seealso cref="BackVertex" />
    //*************************************************************************

	[ DebuggerBrowsable(DebuggerBrowsableState.Never) ]

    public IVertex
    FrontVertex
    {
		get
		{
			AssertValid();

			// Throw an exception if the edge is not directed.

			CheckIsDirected(false);

			return (m_oVertex2);
		}
    }

    //*************************************************************************
    //  Property: IsSelfLoop
    //
    /// <summary>
    /// Gets a value indicating whether the edge connects a vertex to itself.
    /// </summary>
    ///
    /// <value>
    /// true if the edge connects a vertex to itself, false if not.
    /// </value>
    //*************************************************************************

    public Boolean
    IsSelfLoop
    {
        get
		{
			AssertValid();

			return (m_oVertex1 == m_oVertex2);
		}
    }

    //*************************************************************************
    //  Method: Clone()
    //
    /// <overloads>
    /// Creates a copy of the edge.
    /// </overloads>
    ///
    /// <summary>
    /// Creates a copy of the edge, making the copy the same type as the
	/// original.
    /// </summary>
    ///
    /// <param name="copyMetadataValues">
	///	If true, the key/value pairs that were set with <see
	/// cref="IMetadataProvider.SetValue" /> are copied to the new edge.  (This
	/// is a shallow copy.  The objects pointed to by the original values are
	/// NOT cloned.)  If false, the key/value pairs are not copied.
    /// </param>
    ///
    /// <param name="copyTag">
	///	If true, the <see cref="IMetadataProvider.Tag" /> property on the new
	///	edge is set to the same value as in the original edge.  (This is a
	///	shallow copy.  The object pointed to by the original <see
	///	cref="IMetadataProvider.Tag" /> is NOT cloned.)  If false, the <see
	///	cref="IMetadataProvider.Tag "/> property on the new edge is set to
	///	null.
    /// </param>
    ///
    /// <returns>
	///	The copy of the edge, as an <see cref="IEdge" />.
    /// </returns>
	///
    /// <remarks>
	///	The new edge is of the same type as the original.  It is connected to
	/// the same vertices as the original edge.  Its <see
	/// cref="IIdentityProvider.Name" /> is set to the same value as the
	/// original's, but it is assigned a new <see
	/// cref="IIdentityProvider.ID" />.
	///
	/// <para>
	/// The new edge can be added only to the same graph.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    public IEdge
    Clone
    (
		Boolean copyMetadataValues,
        Boolean copyTag
    )
	{
		AssertValid();

		return ( Clone( copyMetadataValues, copyTag, new EdgeFactory() ) );
	}

    //*************************************************************************
    //  Method: Clone()
    //
    /// <summary>
    /// Creates a copy of the edge, making the copy a specified type.
    /// </summary>
    ///
    /// <param name="copyMetadataValues">
	///	If true, the key/value pairs that were set with <see
	/// cref="IMetadataProvider.SetValue" /> are copied to the new edge.  (This
	/// is a shallow copy.  The objects pointed to by the original values are
	/// NOT cloned.)  If false, the key/value pairs are not copied.
    /// </param>
    ///
    /// <param name="copyTag">
	///	If true, the <see cref="IMetadataProvider.Tag" /> property on the new
	///	edge is set to the same value as in the original edge.  (This is a
	///	shallow copy.  The object pointed to by the original <see
	///	cref="IMetadataProvider.Tag" /> is NOT cloned.)  If false, the <see
	///	cref="IMetadataProvider.Tag "/> property on the new edge is set to
	///	null.
    /// </param>
    ///
    /// <param name="newEdgeFactory">
    /// Object that can create edges.
    /// </param>
	///
    /// <returns>
	///	The copy of the edge, as an <see cref="IEdge" />.
    /// </returns>
	///
    /// <remarks>
	///	The new edge is created using <paramref name="newEdgeFactory" />.  It
	///	is connected to the same vertices as the original edge.  Its <see
	/// cref="IIdentityProvider.Name" /> is set to the same value as the
	/// original's, but it is assigned a new <see
	/// cref="IIdentityProvider.ID" />.
	///
	/// <para>
	/// The new edge can be added only to the same graph.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    public IEdge
    Clone
    (
		Boolean copyMetadataValues,
        Boolean copyTag,
		IEdgeFactory newEdgeFactory
    )
	{
		AssertValid();

		return ( Clone(copyMetadataValues, copyTag, newEdgeFactory,
			m_oVertex1, m_oVertex2, m_bIsDirected) );
	}

    //*************************************************************************
    //  Method: Clone()
    //
    /// <summary>
    /// Creates a copy of the edge, making the copy a specified type and using
	/// specified vertices.
    /// </summary>
    ///
    /// <param name="copyMetadataValues">
	///	If true, the key/value pairs that were set with <see
	/// cref="IMetadataProvider.SetValue" /> are copied to the new edge.  (This
	/// is a shallow copy.  The objects pointed to by the original values are
	/// NOT cloned.)  If false, the key/value pairs are not copied.
    /// </param>
    ///
    /// <param name="copyTag">
	///	If true, the <see cref="IMetadataProvider.Tag" /> property on the new
	///	edge is set to the same value as in the original edge.  (This is a
	///	shallow copy.  The object pointed to by the original <see
	///	cref="IMetadataProvider.Tag" /> is NOT cloned.)  If false, the <see
	///	cref="IMetadataProvider.Tag "/> property on the new edge is set to
	///	null.
    /// </param>
    ///
    /// <param name="newEdgeFactory">
    /// Object that can create edges.
    /// </param>
	///
    /// <param name="vertex1">
	///	The new edge's first vertex.  The vertex must be contained in the graph
	///	to which the new edge will be added.
    /// </param>
    ///
    /// <param name="vertex2">
	///	The new edge's second vertex.  The vertex must be contained in the
	/// graph to which the new edge will be added.
    /// </param>
    ///
    /// <param name="isDirected">
	///	If true, <paramref name="vertex1" /> is the new edge's back vertex and
	///	<paramref name="vertex2" /> is the new edge's front vertex.  If false,
	/// the new edge is undirected.
    /// </param>
    ///
    /// <returns>
	///	The copy of the edge, as an <see cref="IEdge" />.
    /// </returns>
	///
    /// <remarks>
	///	The new edge is created using <paramref name="newEdgeFactory" />.  It
	///	is connected to <paramref name="vertex1" /> and <paramref
	/// name="vertex2" />, which can be in the same graph as the original edge
	/// or in a different graph.  Its <see cref="IIdentityProvider.Name" /> is
	/// set to the same value as the original's, but it is assigned a new <see
	/// cref="IIdentityProvider.ID" />.
	///
	/// <para>
	/// The new edge can be added only to the graph that owns <paramref
	/// name="vertex1" /> and <paramref name="vertex2" />.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    public IEdge
    Clone
    (
		Boolean copyMetadataValues,
        Boolean copyTag,
		IEdgeFactory newEdgeFactory,
		IVertex vertex1,
		IVertex vertex2,
		Boolean isDirected
    )
	{
		AssertValid();

        const String MethodName = "Clone";
		ArgumentChecker oArgumentChecker = this.ArgumentChecker;

        oArgumentChecker.CheckArgumentNotNull(
            MethodName, "newEdgeFactory", newEdgeFactory);

        oArgumentChecker.CheckArgumentNotNull(MethodName, "vertex1", vertex1);
        oArgumentChecker.CheckArgumentNotNull(MethodName, "vertex2", vertex2);

		IEdge oNewEdge = newEdgeFactory.CreateEdge(
			vertex1, vertex2, isDirected);

		// Copy the base-class fields to the new edge.

		this.CopyTo(oNewEdge, copyMetadataValues, copyTag);

		return (oNewEdge);
	}

    //*************************************************************************
    //  Method: IsParallelTo()
    //
    /// <summary>
	///	Gets a value indicating whether the edge is parallel to a specified
	///	edge.
    /// </summary>
    ///
    /// <param name="otherEdge">
	///	Edge to test.
    /// </param>
    ///
    /// <returns>
	///	true if the edge is parallel to <paramref name="otherEdge" />, false if
	///	not.
    /// </returns>
    ///
    /// <remarks>
    /// See IEdge.<see cref="IEdge.IsParallelTo" /> for details on the returned
	///	value.
    /// </remarks>
	///
	/// <seealso cref="IsAntiparallelTo" />
    //*************************************************************************

    public Boolean
    IsParallelTo
    (
		IEdge otherEdge
    )
	{
		AssertValid();

		const String MethodName = "IsParallelTo";
		const String ArgumentName = "otherEdge";

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, ArgumentName, otherEdge);

		IVertex oOtherVertex1, oOtherVertex2;

		// Get the other edge's vertices.

		EdgeUtil.EdgeToVertices(otherEdge, this.ClassName, MethodName,
			out oOtherVertex1, out oOtherVertex2);

		// The following code implements the table in the documentation for
		// IEdge.IsParallelTo().

		if (
		     !(
				(m_oVertex1 == oOtherVertex1 && m_oVertex2 == oOtherVertex2)
				|| 
				(m_oVertex1 == oOtherVertex2 && m_oVertex2 == oOtherVertex1)
			  )
		   )
		{
			// The two edges do not connect the same two vertices.

			return (false);
		}

		switch (this.ParentGraph.Directedness)
		{
			case GraphDirectedness.Directed:

				Debug.Assert(this.IsDirected && otherEdge.IsDirected);

				return (m_oVertex1 == oOtherVertex1);

			case GraphDirectedness.Undirected:

				Debug.Assert( !this.IsDirected && !otherEdge.IsDirected );

				return (true);

			case GraphDirectedness.Mixed:

				if ( !this.IsDirected || !otherEdge.IsDirected )
				{
					return (true);
				}

				return (m_oVertex1 == oOtherVertex1);

			default:

				Debug.Assert(false);

				return (false);
		}
	}

    //*************************************************************************
    //  Method: IsAntiparallelTo()
    //
    /// <summary>
	///	Gets a value indicating whether the edge is antiparallel to a specified
	///	edge.
    /// </summary>
    ///
    /// <param name="otherEdge">
	///	Edge to test.
    /// </param>
    ///
    /// <returns>
	///	true if the edge is antiparallel to <paramref name="otherEdge" />,
	///	false if not.
    /// </returns>
    ///
    /// <remarks>
    /// See IEdge.<see cref="IEdge.IsParallelTo" /> for details on the returned
	///	value.
    /// </remarks>
	///	
	/// <seealso cref="IsParallelTo" />
    //*************************************************************************

    public Boolean
    IsAntiparallelTo
    (
		IEdge otherEdge
    )
	{
		AssertValid();

		const String MethodName = "IsAntiparallelTo";
		const String ArgumentName = "otherEdge";

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, ArgumentName, otherEdge);

		return ( !this.IsParallelTo(otherEdge) );
	}

    //*************************************************************************
    //  Method: GetAdjacentVertex()
    //
    /// <summary>
	///	Given one of the edge's vertices, returns the other vertex.
    /// </summary>
    ///
    /// <param name="vertex">
	///	One of the edge's vertices.
    /// </param>
    ///
    /// <returns>
	///	The edge's other vertex.
    /// </returns>
	///
	/// <remarks>
	/// An ArgumentException is thrown if <paramref name="vertex" /> is not one
	/// of the edge's vertices.
	/// </remarks>
    //*************************************************************************

	public IVertex
	GetAdjacentVertex
	(
		IVertex vertex
	)
	{
		Debug.Assert(vertex != null);

		const String MethodName = "GetAdjacentVertex";

		if (m_oVertex1 == vertex)
		{
			return (m_oVertex2);
		}

		if (m_oVertex2 == vertex)
		{
			return (m_oVertex1);
		}

		this.ArgumentChecker.ThrowArgumentException(MethodName, "vertex",

			"The specified vertex is not one of the edge's vertices."
			);

		return (null);
	}

	//*************************************************************************
	//	Method: AppendPropertiesToString()
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

		if (m_bIsDirected)
		{
			ToStringUtil.AppendPropertyToString(oStringBuilder,
				iIndentationLevel, "BackVertex", this.BackVertex);

			ToStringUtil.AppendPropertyToString(oStringBuilder,
				iIndentationLevel, "FrontVertex", this.FrontVertex);
		}

		ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
			"IsDirected", this.IsDirected);

		ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
			"IsSelfLoop", this.IsSelfLoop);

		ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
			"ParentGraph", this.ParentGraph);

		ToStringUtil.AppendPropertyToString(oStringBuilder, iIndentationLevel,
			"Vertices", String.Empty, false);

		ToStringUtil.AppendVerticesToString(oStringBuilder, iIndentationLevel,
			sFormat, this.Vertices);
	}

    //*************************************************************************
    //  Method: CheckVertexArgument()
    //
    /// <summary>
    /// Checks one of the <see cref="IVertex" /> arguments passed to the
	/// constructor.
    /// </summary>
	///
    /// <param name="oVertex">
	/// The vertex to check.
    /// </param>
    ///
    /// <param name="sArgumentName">
	///	Name of the vertex argument.
    /// </param>
	///
	/// <remarks>
	/// An exception is thrown if <paramref name="oVertex" /> is invalid.
	/// </remarks>
    //*************************************************************************

    protected void
	CheckVertexArgument
	(
		IVertex oVertex,
		String sArgumentName
	)
    {
		ArgumentChecker oArgumentChecker = this.ArgumentChecker;
		const String MethodName = "Constructor";

		// The vertex can't be null, and it must belong to a graph.

		oArgumentChecker.CheckArgumentNotNull(
			MethodName, sArgumentName, oVertex);

		if (oVertex.ParentGraph == null)
		{
			oArgumentChecker.ThrowArgumentException(MethodName, sArgumentName,

				String.Format(

					"{0} has not been added to a graph.  A vertex must be"
					+ " added to a graph before it can be connected to an"
					+ " edge."
					,
					sArgumentName
				) );
		}
    }

    //*************************************************************************
    //  Method: CheckIsDirected()
    //
    /// <summary>
	/// Throws an exception if the edge is not directed.
    /// </summary>
	///
    /// <param name="bCallerIsBackVertex">
	/// true if this is being called from <see cref="BackVertex" />, false if
	/// this is being called from <see cref="FrontVertex" />.
    /// </param>
    //*************************************************************************

    protected void
	CheckIsDirected
	(
		Boolean bCallerIsBackVertex
	)
    {
		AssertValid();

		if (!m_bIsDirected)
		{
			this.ArgumentChecker.ThrowPropertyException(
			
				bCallerIsBackVertex ? "BackVertex" : "FrontVertex",

				String.Format(

					"The edge is not directed, so it does not have a {0}"
					+ " vertex."
					,
					bCallerIsBackVertex ? "back" : "front"
				) );
		}
    }

    //*************************************************************************
    //  Method: IEdgeToEdge
    //
    /// <summary>
    /// Casts an <see cref="IEdge" /> to an <see cref="Edge" /> object.
    /// </summary>
	///
    /// <param name="oEdge">
	/// The <see cref="IEdge" /> to cast to an <see cref="Edge" /> object.
    /// </param>
	///
    /// <param name="sClassName">
	/// Name of the class calling this method.
    /// </param>
	///
    /// <param name="sMethodOrPropertyName">
	/// Name of the method or property calling this method.
    /// </param>
	///
	/// <returns>
	/// The <see cref="Edge" /> object.
	/// </returns>
	///
	/// <remarks>
	/// An exception is thrown if <paramref name="oEdge" /> is null or not of
	/// type <see cref="Edge" />.
	/// </remarks>
    //*************************************************************************

	protected internal static Edge
	IEdgeToEdge
	(
		IEdge oEdge,
		String sClassName,
		String sMethodOrPropertyName
	)
	{
		Debug.Assert( !String.IsNullOrEmpty(sClassName) );
		Debug.Assert( !String.IsNullOrEmpty(sMethodOrPropertyName) );

		if (oEdge == null)
		{
			throw new ApplicationException(String.Format(

				"{0}.{1}: An edge is null."
				,
				sClassName,
				sMethodOrPropertyName
				) );
		}

		if ( !(oEdge is Edge) )
		{
			throw new ApplicationException(String.Format(

				"{0}.{1}: An edge is not of type Edge.  The type is {2}."
				,
				sClassName,
				sMethodOrPropertyName,
				oEdge.GetType().FullName
				) );
		}

		return ( (Edge)oEdge );
	}

    //*************************************************************************
    //  Method: EdgeToVertices()
    //
    /// <summary>
    /// Obtains an edge's two <see cref="Vertex" /> objects.
    /// </summary>
    ///
    /// <param name="oEdge">
	/// The edge to obtain the <see cref="Vertex" /> objects from.
    /// </param>
	///
    /// <param name="sClassName">
	/// Name of the class calling this method.
    /// </param>
	///
    /// <param name="sMethodOrPropertyName">
	/// Name of the method or property calling this method.
    /// </param>
	///
    /// <param name="oVertex1">
	/// Where the edge's first <see cref="Vertex" /> gets stored.
    /// </param>
	///
    /// <param name="oVertex2">
	/// Where the edge's second <see cref="Vertex" /> gets stored.
    /// </param>
	///
    /// <remarks>
	/// This method convertes <paramref name="oEdge" /> to an <see
	/// cref="Edge" /> object, obtains the Edge's two <see cref="Vertex" />
	/// objects, and stores them at <paramref name="oVertex1" /> and <paramref
	/// name="oVertex2" />.  An exception is thrown if <paramref
	/// name="oEdge" /> is null or not of type <see cref="Edge" />, or if the
	/// edge's vertices are null or not of type <see cref="Vertex" />.
    /// </remarks>
    //*************************************************************************

	protected internal static void
	EdgeToVertices
	(
		IEdge oEdge,
		String sClassName,
		String sMethodOrPropertyName,
		out Vertex oVertex1,
		out Vertex oVertex2
	)
	{
		Debug.Assert( !String.IsNullOrEmpty(sClassName) );
		Debug.Assert( !String.IsNullOrEmpty(sMethodOrPropertyName) );

		// Cast to an Edge.

		Edge oEdge2 = IEdgeToEdge(oEdge, sClassName, sMethodOrPropertyName);

		// Cast the IVertex interfaces to Vertex, checking the types in the
		// process.

		oVertex1 = Vertex.IVertexToVertex(
			oEdge2.m_oVertex1, sClassName, sMethodOrPropertyName);

		oVertex2 = Vertex.IVertexToVertex(
			oEdge2.m_oVertex2, sClassName, sMethodOrPropertyName);
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

		Debug.Assert(m_oVertex1 != null);
		Debug.Assert(m_oVertex2 != null);
		// m_bIsDirected
    }


    //*************************************************************************
    //  Private fields
    //*************************************************************************

	/// Generates unique IDs.

	private static IDGenerator m_oIDGenerator;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// First vertex.  If m_bIsDirected is true, this is the edge's back
	/// vertex.  Can't be null.

	protected IVertex m_oVertex1;

	/// Second vertex.  If m_bIsDirected is true, this is the edge's front
	/// vertex.  Can't be null.

	protected IVertex m_oVertex2;

	///	If true, m_oVertex2 is the edge's back vertex and m_oVertex2 is the
	/// edge's front vertex.  If false, the edge is undirected.

	protected Boolean m_bIsDirected;
}
}
