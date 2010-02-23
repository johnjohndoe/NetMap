
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Class: VertexCollection
//
/// <summary>
/// Represents a collection of vertices.
/// </summary>
///
/// <remarks>
/// This is a collection of objects that implement the <see cref="IVertex" />
/// interface.  You can add vertices to the collection, remove them, access
/// a vertex, and enumerate all vertices.
/// </remarks>
///
//  The implementation uses a protected LinkedList<IVertex> field to implement
//  the collection.  It does not inherit from LinkedList, which would expose
//  too much implementation-specific functionality to the user.
//*****************************************************************************

public partial class VertexCollection : NodeXLBase, IVertexCollection
{
    //*************************************************************************
    //  Constructor: VertexCollection()
    //
    /// <summary>
    /// Initializes a new instance of the VertexCollection class.
    /// </summary>
    ///
    /// <param name="parentGraph">
    /// The <see cref="IGraph" /> that owns this collection.
    /// </param>
    //*************************************************************************

    public VertexCollection
    (
        IGraph parentGraph
    )
    {
        const String MethodName = "Constructor";
        const String ArgumentName = "parentGraph";

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, ArgumentName, parentGraph);

        m_oParentGraph = parentGraph;

        m_oLinkedList = new LinkedList<IVertex>();

        AssertValid();
    }

    //*************************************************************************
    //  Property: Count
    //
    /// <summary>
    /// Gets the number of elements contained in the collection.
    /// </summary>
    ///
    /// <value>
    /// The number of elements contained in the collection.
    /// </value>
    ///
    /// <remarks>
    /// Retrieving the value of this property is an O(1) operation.
    /// </remarks>
    //*************************************************************************

    public Int32
    Count
    {
        get
        {
            AssertValid();

            return (m_oLinkedList.Count);
        }
    }

    //*************************************************************************
    //  Property: IsReadOnly
    //
    /// <summary>
    /// Gets a value indicating whether the collection is read-only.
    /// </summary>
    ///
    /// <value>
    /// true if the collection is read-only.
    /// </value>
    ///
    /// <remarks>
    /// This method is an O(1) operation.
    /// </remarks>
    //*************************************************************************

    public Boolean
    IsReadOnly
    {
        get
        {
            AssertValid();

            return (false);
        }
    }

    //*************************************************************************
    //  Method: Add()
    //
    /// <overloads>
    /// Adds a vertex to the collection.
    /// </overloads>
    ///
    /// <summary>
    /// Adds an existing vertex to the collection.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to add to the collection.
    /// </param>
    ///
    /// <remarks>
    /// An exception is thrown if <paramref name="vertex" /> has already been
    /// added to a graph.
    /// </remarks>
    //*************************************************************************

    public void
    Add
    (
        IVertex vertex
    )
    {
        AssertValid();

        ArgumentChecker oArgumentChecker = this.ArgumentChecker;
        const String MethodName = "Add";
        const String ArgumentName = "vertex";

        oArgumentChecker.CheckArgumentNotNull(
            MethodName, ArgumentName, vertex);

        // Check whether the vertex already belongs to a graph.

        if (vertex.ParentGraph != null)
        {
            oArgumentChecker.ThrowArgumentException(MethodName, ArgumentName,

                "The vertex already belongs to a graph.  A vertex can't be"
                + " added twice."
                );
        }

        if (m_oParentGraph.PerformExtraValidations)
        {
            // Check whether the vertex's ID is already used in the collection.

            Int32 iID = vertex.ID;

            if ( this.Contains(iID) )
            {
                oArgumentChecker.ThrowArgumentException(
                    MethodName, ArgumentName,

                    String.Format(

                        "A vertex with the ID {0} already exists in the"
                        + " collection."
                        ,
                        iID
                    ) );
            }
        }

        // The vertex is valid.  Add it to the collection.

        m_oLinkedList.AddLast(vertex);

        // Set the vertex's parent graph.

        Vertex oVertex = Vertex.IVertexToVertex(
            vertex, this.ClassName, MethodName);

        oVertex.SetParentGraph(m_oParentGraph);

        // Fire a VertexAdded event if necessary.

        VertexEventHandler oVertexAdded = this.VertexAdded;

        if (oVertexAdded != null)
        {
            oVertexAdded( this, new VertexEventArgs(vertex) );
        }

        AssertValid();
    }

    //*************************************************************************
    //  Method: Add()
    //
    /// <summary>
    /// Creates a vertex and adds it to the collection.
    /// </summary>
    ///
    /// <returns>
    /// The added vertex.
    /// </returns>
    //*************************************************************************

    public IVertex
    Add()
    {
        AssertValid();

        IVertex oVertex = new Vertex();

        this.Add(oVertex);

        AssertValid();

        return (oVertex);
    }

    //*************************************************************************
    //  Method: Clear()
    //
    /// <summary>
    /// Removes all vertices from the collection.
    /// </summary>
    ///
    /// <remarks>
    /// This method removes all vertices from the collection and all edges from
    /// the graph that owns the collection.
    ///
    /// <para>
    /// Do not call this method if you are using NodeXLControl.  To clear the
    /// graph within NodeXLControl, use NodeXLControl.ClearGraph().
    /// </para>
    ///
    /// <para>
    /// This method is an O(n) operation, where n is <see cref="Count" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    Clear()
    {
        AssertValid();

        const String MethodName = "Clear";

        // Do not just clear m_oLinkedList.  This would remove the vertices
        // from the collection, but each vertex, which can continue to exist
        // after being removed from the graph, would still have a group of
        // incident edges that are no longer valid.

        foreach (IVertex oVertex in this)
        {
            // Tell the Vertex that it no longer has any incident edges or a
            // parent.

            Vertex oVertex2 = Vertex.IVertexToVertex(
                oVertex, this.ClassName, MethodName);

            oVertex2.FirstIncidentEdgeNode = null;

            oVertex2.SetParentGraph(null);
        }

        // Remove all vertices.

        m_oLinkedList.Clear();

        // Remove all incident edges from all vertices.

        m_oParentGraph.Edges.Clear();

        AssertValid();
    }

    //*************************************************************************
    //  Method: Contains()
    //
    /// <overloads>
    /// Determines whether the collection contains a specified vertex.
    /// </overloads>
    ///
    /// <summary>
    /// Determines whether the collection contains a vertex specified by
    /// reference.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to search for.
    /// </param>
    ///
    /// <returns>
    /// true if the collection contains <paramref name="vertex" />.
    /// </returns>
    ///
    /// <remarks>
    /// This method is an O(n) operation, where n is <see cref="Count" />.
    /// </remarks>
    //*************************************************************************

    public Boolean
    Contains
    (
        IVertex vertex
    )
    {
        AssertValid();

        const String MethodName = "Contains";
        const String ArgumentName = "vertex";

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, ArgumentName, vertex);

        return ( m_oLinkedList.Contains(vertex) );
    }

    //*************************************************************************
    //  Method: Contains()
    //
    /// <summary>
    /// Determines whether the collection contains a vertex specified by <see
    /// cref="IIdentityProvider.ID" />
    /// </summary>
    ///
    /// <param name="id">
    /// The ID to search for.
    /// </param>
    ///
    /// <returns>
    /// true if the collection contains a vertex with the <see
    /// cref="IIdentityProvider.ID" /> <paramref name="id" />.
    /// </returns>
    ///
    /// <remarks>
    /// IDs are unique among all vertices, so there can be only one vertex with
    /// the specified ID.
    ///
    /// <para>
    /// This method is an O(n) operation, where n is <see cref="Count" />.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="Find(Int32, out IVertex)" />
    //*************************************************************************

    public Boolean
    Contains
    (
        Int32 id
    )
    {
        AssertValid();

        IVertex oVertex;

        return ( Find(id, out oVertex) );
    }

    //*************************************************************************
    //  Method: Contains()
    //
    /// <summary>
    /// Determines whether the collection contains a vertex specified by <see
    /// cref="IIdentityProvider.Name" />
    /// </summary>
    ///
    /// <param name="name">
    /// The name to search for.  Can't be null or empty.
    /// </param>
    ///
    /// <returns>
    /// true if the collection contains a vertex with the <see
    /// cref="IIdentityProvider.Name" /> <paramref name="name" />.
    /// </returns>
    ///
    /// <remarks>
    /// Names do not have to be unique, so there could be more than one vertex
    /// with the same name.
    ///
    /// <para>
    /// This method is an O(n) operation, where n is <see cref="Count" />.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="Find(String, out IVertex)" />
    //*************************************************************************

    public Boolean
    Contains
    (
        String name
    )
    {
        AssertValid();

        const String MethodName = "Contains";
        const String ArgumentName = "name";

        this.ArgumentChecker.CheckArgumentNotEmpty(
            MethodName, ArgumentName, name);

        IVertex oVertex;

        return ( Find(name, out oVertex) );
    }

    //*************************************************************************
    //  Method: CopyTo()
    //
    /// <summary>
    /// Copies the elements of the collection to an array, starting at a
    /// particular array index.
    /// </summary>
    ///
    /// <param name="array">
    /// The one-dimensional array that is the destination of the elements
    /// copied from the collection.  The array must have zero-based indexing. 
    /// </param>
    ///
    /// <param name="index">
    /// The zero-based index in <paramref name="array" /> at which copying
    /// begins. 
    /// </param>
    ///
    /// <remarks>
    /// This method is an O(n) operation, where n is <see cref="Count" />.
    /// </remarks>
    //*************************************************************************

    public void
    CopyTo
    (
        IVertex [] array,
        Int32 index
    )
    {
        AssertValid();

        ArgumentChecker oArgumentChecker = this.ArgumentChecker;
        const String MethodName = "CopyTo";
        const String ArgumentName = "array";

        oArgumentChecker.CheckArgumentNotNull(MethodName, ArgumentName, array);

        m_oLinkedList.CopyTo(array, index);
    }

    //*************************************************************************
    //  Method: Find()
    //
    /// <overloads>
    /// Searches for a specified vertex.
    /// </overloads>
    ///
    /// <summary>
    /// Searches for the vertex with the specified <see
    /// cref="IIdentityProvider.ID" />.
    /// </summary>
    ///
    /// <param name="id">
    /// <see cref="IIdentityProvider.ID" /> of the vertex to search for.
    /// </param>
    ///
    /// <param name="vertex">
    /// Gets set to the specified <see cref="IVertex" /> if true is returned,
    /// or to null if false is returned.
    /// </param>
    ///
    /// <returns>
    /// true if a vertex with an <see cref="IIdentityProvider.ID" /> of
    /// <paramref name="id" /> is found, false if not.
    /// </returns>
    ///
    /// <remarks>
    /// This method searches the collection for the vertex with the <see
    /// cref="IIdentityProvider.ID" /> <paramref name="id" />.  If such a
    /// vertex is found, it gets stored at <paramref name="vertex" /> and true
    /// is returned.  Otherwise, <paramref name="vertex" /> gets set to null
    /// and false is returned.
    ///
    /// <para>
    /// IDs are unique among all vertices, so there can be only one vertex
    /// with the specified ID.
    /// </para>
    ///
    /// <para>
    /// Use <see cref="Contains(Int32)" /> if you want to determine whether
    /// such a vertex exists in the collection but you don't need the actual
    /// vertex.
    /// </para>
    ///
    /// <para>
    /// This method is an O(n) operation, where n is <see cref="Count" />.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="Contains(Int32)" />
    //*************************************************************************

    public Boolean
    Find
    (
        Int32 id,
        out IVertex vertex
    )
    {
        AssertValid();

        LinkedListNode<IVertex> oLinkedListNode;

        if ( !Find(true, id, null, out oLinkedListNode) )
        {
            vertex = null;

            return (false);
        }

        vertex = oLinkedListNode.Value;

        return (true);
    }

    //*************************************************************************
    //  Method: Find()
    //
    /// <summary>
    /// Searches for the first vertex with the specified <see
    /// cref="IIdentityProvider.Name" />.
    /// </summary>
    ///
    /// <param name="name">
    /// The <see cref="IIdentityProvider.Name" /> of the vertex to search for.
    /// Can't be null or empty.
    /// </param>
    ///
    /// <param name="vertex">
    /// Gets set to the specified <see cref="IVertex" /> if true is returned,
    /// or to null if false is returned.
    /// </param>
    ///
    /// <returns>
    /// true if a vertex with a <see cref="IIdentityProvider.Name" /> of
    /// <paramref name="name" /> is found, false if not.
    /// </returns>
    ///
    /// <remarks>
    /// This method searches the collection for the first vertex with the <see
    /// cref="IIdentityProvider.Name" /> <paramref name="name" />.  If such
    /// a vertex is found, it gets stored at <paramref name="vertex" /> and
    /// true is returned.  Otherwise, <paramref name="vertex" /> gets set to
    /// null and false is returned.
    ///
    /// <para>
    /// Names do not have to be unique, so there could be more than one vertex
    /// with the same name.
    /// </para>
    ///
    /// <para>
    /// Use <see cref="Contains(String)" /> if you want to determine whether
    /// such a vertex exists in the collection but you don't need the actual
    /// vertex.
    /// </para>
    ///
    /// <para>
    /// This method is an O(n) operation, where n is <see cref="Count" />.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="Contains(String)" />
    //*************************************************************************

    public Boolean
    Find
    (
        String name,
        out IVertex vertex
    )
    {
        AssertValid();

        ArgumentChecker oArgumentChecker = this.ArgumentChecker;
        const String MethodName = "Find";
        const String ArgumentName = "name";

        oArgumentChecker.CheckArgumentNotEmpty(
            MethodName, ArgumentName, name);

        LinkedListNode<IVertex> oLinkedListNode;


        if ( !Find(false, -1, name, out oLinkedListNode) )
        {
            vertex = null;

            return (false);
        }

        vertex = oLinkedListNode.Value;

        return (true);
    }

    //*************************************************************************
    //  Method: GetEnumerator()
    //
    /// <summary>
    /// Returns an enumerator that iterates through the collection. 
    /// </summary>
    ///
    /// <returns>
    /// An enumerator object that can be used to iterate through the
    /// collection. 
    /// </returns>
    //*************************************************************************

    public IEnumerator<IVertex>
    GetEnumerator()
    {
        AssertValid();

        return ( m_oLinkedList.GetEnumerator() );
    }

    //*************************************************************************
    //  Method: GetEnumerator()
    //
    /// <summary>
    /// Returns an enumerator that iterates through the collection. 
    /// </summary>
    ///
    /// <returns>
    /// An enumerator object that can be used to iterate through the
    /// collection. 
    /// </returns>
    //*************************************************************************

    System.Collections.IEnumerator
    System.Collections.IEnumerable.GetEnumerator()
    {
        AssertValid();

        return ( GetEnumerator() );
    }

    //*************************************************************************
    //  Method: GetReverseEnumerable()
    //
    /// <summary>
    /// Returns an IEnumerable that can be used to iterate backwards through
    /// the collection. 
    /// </summary>
    ///
    /// <returns>
    /// An IEnumerable that can be used to iterate backwards through the
    /// collection. 
    /// </returns>
    //*************************************************************************

    public IEnumerable<IVertex>
    GetReverseEnumerable()
    {
        AssertValid();

        return ( new ReverseEnumerator(this) );
    }

    //*************************************************************************
    //  Method: Remove()
    //
    /// <overloads>
    /// Removes a vertex from the collection.
    /// </overloads>
    ///
    /// <summary>
    /// Removes a vertex specified by reference from the collection.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to remove from the collection.
    /// </param>
    ///
    /// <returns>
    /// true if the vertex was removed, false if the vertex wasn't found in the
    /// collection.
    /// </returns>
    ///
    /// <remarks>
    /// This method searches the collection for <paramref name="vertex" />.  If
    /// found, it is removed from the collection, any edges connected to it are
    /// removed from the graph that owns this vertex collection, and true is
    /// returned.  false is returned otherwise.
    ///
    /// <para>
    /// This method is an O(n) operation, where n is <see cref="Count" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    Remove
    (
        IVertex vertex
    )
    {
        AssertValid();

        ArgumentChecker oArgumentChecker = this.ArgumentChecker;
        const String MethodName = "Remove";
        const String ArgumentName = "vertex";

        oArgumentChecker.CheckArgumentNotNull(
            MethodName, ArgumentName, vertex);

        // Search for the specified vertex.

        LinkedListNode<IVertex> oLinkedListNode = m_oLinkedList.Find(vertex);

        if (oLinkedListNode == null)
        {
            return (false);
        }

        // Remove the vertex's node and all edges connected to the vertex.

        Remove(oLinkedListNode);

        AssertValid();

        return (true);
    }

    //*************************************************************************
    //  Method: Remove()
    //
    /// <summary>
    /// Removes a vertex specified by <see cref="IIdentityProvider.ID" /> from
    /// the collection.
    /// </summary>
    ///
    /// <param name="id">
    /// The ID of the vertex to remove.
    /// </param>
    ///
    /// <returns>
    /// true if the vertex was removed, false if the vertex wasn't found in the
    /// collection.
    /// </returns>
    ///
    /// <remarks>
    /// This method searches the collection for the vertex with the <see
    /// cref="IIdentityProvider.ID" /> <paramref name="id" />.  If found, it
    /// is removed from the collection, any edges connected to it are removed
    /// from the graph that owns this vertex collection, and true is returned.
    /// false is returned otherwise.
    ///
    /// <para>
    /// IDs are unique among all vertices, so there can be only one vertex
    /// with the specified ID.
    /// </para>
    ///
    /// <para>
    /// This method is an O(n) operation, where n is <see cref="Count" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    Remove
    (
        Int32 id
    )
    {
        AssertValid();

        // Search for the specified vertex.

        LinkedListNode<IVertex> oLinkedListNode;

        if ( !Find(true, id, null, out oLinkedListNode) )
        {
            return (false);
        }

        // Remove the vertex's node and all edges connected to the vertex.

        Remove(oLinkedListNode);

        AssertValid();

        return (true);
    }

    //*************************************************************************
    //  Method: Remove()
    //
    /// <summary>
    /// Removes a vertex specified by <see cref="IIdentityProvider.Name" />
    /// from the collection.
    /// </summary>
    ///
    /// <param name="name">
    /// Name of the vertex to remove.  Can't be null or empty.
    /// </param>
    ///
    /// <returns>
    /// true if the vertex was removed, false if the vertex wasn't found in the
    /// collection.
    /// </returns>
    ///
    /// <remarks>
    /// This method searches the collection for the first vertex with the <see
    /// cref="IIdentityProvider.Name" /> <paramref name="name" />.  If
    /// found, it is removed from the collection, any edges connected to it are
    /// removed from the graph that owns this vertex collection, and true is
    /// returned.  false is returned otherwise.
    ///
    /// <para>
    /// Names do not have to be unique, so there could be more than one vertex
    /// with the same name.
    /// </para>
    ///
    /// <para>
    /// This method is an O(n) operation, where n is <see cref="Count" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    Remove
    (
        String name
    )
    {
        AssertValid();

        const String MethodName = "Remove";
        const String ArgumentName = "name";

        this.ArgumentChecker.CheckArgumentNotEmpty(
            MethodName, ArgumentName, name);

        // Search for the specified vertex.

        LinkedListNode<IVertex> oLinkedListNode;

        if ( !Find(false, -1, name, out oLinkedListNode) )
        {
            return (false);
        }

        // Remove the vertex's node and all edges connected to the vertex.

        Remove(oLinkedListNode);

        AssertValid();

        return (true);
    }

    //*************************************************************************
    //  Method: ToString()
    //
    /// <summary>
    /// Formats the value of the current instance.
    /// </summary>
    ///
    /// <returns>
    /// The formatted string.
    /// </returns>
    //*************************************************************************

    public override String
    ToString()
    {
        AssertValid();

        Int32 iVertices = this.Count;

        return ( iVertices.ToString(NodeXLBase.Int32Format) + 
            ( (iVertices == 1) ? " vertex" : " vertices" ) );

    }

    //*************************************************************************
    //  Event: VertexAdded
    //
    /// <summary>
    /// Occurs when a vertex is added to the collection.
    /// </summary>
    //*************************************************************************

    public event VertexEventHandler
    VertexAdded;


    //*************************************************************************
    //  Event: VertexRemoved
    //
    /// <summary>
    /// Occurs when a vertex is removed from the collection.
    /// </summary>
    //*************************************************************************

    public event VertexEventHandler
    VertexRemoved;


    //*************************************************************************
    //  Method: Find()
    //
    /// <summary>
    /// Searches for a vertex with the specified <see
    /// cref="IIdentityProvider.ID" /> or <see
    /// cref="IIdentityProvider.Name" />.
    /// </summary>
    ///
    /// <param name="bByID">
    /// true to search by ID, false to search by name.
    /// </param>
    ///
    /// <param name="iID">
    /// The <see cref="IIdentityProvider.ID" /> of the vertex to search for if
    /// <paramref name="bByID" /> is true.
    /// </param>
    ///
    /// <param name="sName">
    /// The <see cref="IIdentityProvider.Name" /> of the vertex to search for
    /// if <paramref name="bByID" /> is false.  Can't be null or empty if
    /// <paramref name="bByID" /> is false.
    /// </param>
    ///
    /// <param name="oLinkedListNode">
    /// Gets set to the specified <see cref="LinkedListNode{IVertex}" /> if
    /// true is returned, or to null if false is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the specified vertex is found, false if not.
    /// </returns>
    //*************************************************************************

    protected Boolean
    Find
    (
        Boolean bByID,
        Int32 iID,
        String sName,
        out LinkedListNode<IVertex> oLinkedListNode
    )
    {
        AssertValid();
        Debug.Assert( bByID || !String.IsNullOrEmpty(sName) );

        for (
            oLinkedListNode = m_oLinkedList.First;
            oLinkedListNode != null;
            oLinkedListNode = oLinkedListNode.Next
            )
        {
            if (bByID)
            {
                if (oLinkedListNode.Value.ID == iID)
                {
                    return (true);
                }
            }
            else
            {
                if (oLinkedListNode.Value.Name == sName)
                {
                    return (true);
                }
            }
        }

        return (false);
    }

    //*************************************************************************
    //  Method: Remove()
    //
    /// <summary>
    /// Removes a <see cref="LinkedListNode{IVertex}" /> from the linked list.
    /// </summary>
    ///
    /// <param name="oLinkedListNode">
    /// The node to remove from the linked list.
    /// </param>
    ///
    /// <remarks>
    /// This method removes <paramref name="oLinkedListNode" /> from the linked
    /// list and removes any edges connected to it.
    /// </remarks>
    //*************************************************************************

    protected void
    Remove
    (
        LinkedListNode<IVertex> oLinkedListNode
    )
    {
        AssertValid();
        Debug.Assert(oLinkedListNode != null);

        const String MethodName = "Remove";

        // Cast to a Vertex.

        Vertex oVertex = Vertex.IVertexToVertex(
            oLinkedListNode.Value, this.ClassName, MethodName);

        // Remove the vertex's incident edges.

        oVertex.RemoveIncidentEdges();

        // Now remove the node from the list.

        m_oLinkedList.Remove(oLinkedListNode);

        // The vertex no longer has a parent graph.

        oVertex.SetParentGraph(null);

        // Fire a VertexRemoved event if necessary.

        VertexEventHandler oVertexRemoved = this.VertexRemoved;

        if (oVertexRemoved != null)
        {
            oVertexRemoved( this, new VertexEventArgs(oVertex) );
        }

        AssertValid();
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

        Debug.Assert(m_oParentGraph != null);
        Debug.Assert(m_oLinkedList != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Graph that owns this collection.

    protected IGraph m_oParentGraph;

    /// The linked list that stores the collection.

    protected LinkedList<IVertex> m_oLinkedList;
}
}
