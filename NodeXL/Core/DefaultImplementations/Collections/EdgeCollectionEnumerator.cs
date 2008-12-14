
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.NodeXL.Core
{
public partial class EdgeCollection : NodeXLBase, IEdgeCollection
{
//*****************************************************************************
//	Nested class: Enumerator
//
/// <summary>
///	Supports iterating over an <see cref="EdgeCollection" />.
/// </summary>
///
/// <remarks>
///	This class is nested within the <see cref="EdgeCollection" /> class, so
/// its type is EdgeCollection.Enumerator.  An instance of this class gets
/// returned by <see cref="EdgeCollection.GetEnumerator()" />.
/// </remarks>
//*****************************************************************************

// This class should really be protected to hide it from users.  It's marked as
// public to allow it to be tested with unit tests.

public class Enumerator : NodeXLBase, System.Collections.IEnumerator
{
	//*************************************************************************
	//	Constructor: Enumerator()
	//
	/// <summary>
	///	Initializes a new instance of the Enumerator class.
	/// </summary>
	///
	/// <param name="edgeCollection">
	/// Collection being enumerated.
	/// </param>
	//*************************************************************************

	protected internal Enumerator
	(
		EdgeCollection edgeCollection
	)
	{
		Debug.Assert(edgeCollection != null);

		m_oEdgeCollection = edgeCollection;

		m_oCurrentNode = null;

		m_oEnumeratedEdgeIDs = null;

		AssertValid();
	}

	//*************************************************************************
	//	Property: Current
	//
	/// <summary>
	/// Gets the object at the current position.
	/// </summary>
	///
	/// <value>
	/// The <see cref="IEdge" /> object at the enumerator's current position.
	/// </value>
	//*************************************************************************

	Object
	System.Collections.IEnumerator.Current
	{
		get
		{
			AssertValid();

			return ( GetCurrent() );
		}
	}

	//*************************************************************************
	//	Property: Current
	//
	/// <summary>
	/// Gets the object at the current position.
	/// </summary>
	///
	/// <value>
	/// The <see cref="IEdge" /> object at the enumerator's current position.
	/// </value>
	///
	//	This is a type-safe version of IEnumerator.Current property.
	//*************************************************************************

	public IEdge
	Current
	{
		get
		{
			AssertValid();

			return ( GetCurrent() );
		}
	}

	//*************************************************************************
	//	Method: MoveNext()
	//
	/// <summary>
	/// Moves to the next object in the enumeration.
	/// </summary>
	///
	/// <returns>
	/// true if the enumerator was successfully advanced to the next element;
	/// false if the enumerator has passed the end of the collection. 
	/// </returns>
	//*************************************************************************

	public Boolean
	MoveNext()
	{
		AssertValid();

		if (m_oCurrentNode != null)
		{
			// MoveNext() has been called before.

			m_oCurrentNode = m_oCurrentNode.Next;
		}
		else
		{
			// Either this is the first time MoveNext() has been called, or
			// Reset() was called.

            m_oCurrentNode = m_oEdgeCollection.m_oLinkedList.First;

			if (m_oEnumeratedEdgeIDs == null)
			{
				// This is the first time MoveNext() has been called.

				// Create a dictionary to keep track of the edge IDs that have
				// already been enumerated.  When an edge is added to
				// EdgeCollection, it gets added to the incident edge groups
				// for both of the edge's vertices.  If every edge were
				// enumerated, the collection would appear to contain twice as
				// many edges as actually exist.
				//
				// In these comments, an "enumerated" edge is one that has
				// already been returned by the Current property.

				Debug.Assert(m_oEnumeratedEdgeIDs == null);

				m_oEnumeratedEdgeIDs = new Dictionary<Int32, Byte>();
			}
		}

		// Iterate through the LinkedList.

		while (m_oCurrentNode != null)
		{
			if (m_oCurrentNode.Value != null)
			{
				// The current node is not the null terminator for a vertex's
				// group of incident edges.  (Such a node must be skipped.)
				//
				// Get the ID of the node's edge.

				Int32 iID = m_oCurrentNode.Value.ID;

				// Has the edge already been enumerated?

				Debug.Assert(m_oEnumeratedEdgeIDs != null);

				if ( !m_oEnumeratedEdgeIDs.ContainsKey(iID) )
				{
					// No.  To prevent it from being enumerated a second time,
					// add the ID to the dictionary.

					m_oEnumeratedEdgeIDs.Add(iID, 0);

					return (true);
				}
			}

			// Move to the next node in the LinkedList.

			m_oCurrentNode = m_oCurrentNode.Next;
		}

		// The end of the linked list has been reached.

		Debug.Assert(m_oCurrentNode == null);

		m_oEnumeratedEdgeIDs.Clear();

		return (false);
	}

	//*************************************************************************
	//	Method: Reset()
	//
	/// <summary>
	///	Resets the current position so it points to the beginning of the
	///	enumeration.
	/// </summary>
	//*************************************************************************

	public void
	Reset()
	{
		AssertValid();

		m_oCurrentNode = null;
		m_oEnumeratedEdgeIDs = null;

		AssertValid();
	}

	//*************************************************************************
	//	Method: GetCurrent()
	//
	/// <summary>
	/// Gets the object at the current position.
	/// </summary>
	///
	/// <returns>
	/// The <see cref="IEdge" /> object at the enumerator's current position.
	/// </returns>
	//*************************************************************************

	protected IEdge
	GetCurrent()
	{
		AssertValid();

		const String PropertyName = "Current";

		if (m_oCurrentNode == null)
		{
			Debug.Assert(false);

			throw new ApplicationException(String.Format(

				"{0}.{1}: MoveNext() hasn't been called."
				,
				this.ClassName,
				PropertyName
				) );
		}

		if (m_oCurrentNode.Value == null)
		{
			// The current node should never be the terminating null for a 
			// vertex's group of incident edges.

			Debug.Assert(false);

			throw new ApplicationException(String.Format(

				"{0}.{1}: The current node is the terminating null for a group"
				+ " of incident edges."
				,
				this.ClassName,
				PropertyName
				) );
		}

		return (m_oCurrentNode.Value);
	}


	//*************************************************************************
	//	Method: AssertValid()
	//
	/// <summary>
    ///	Asserts if the object is in an invalid state.  Debug-only.
	/// </summary>
    //*************************************************************************

	// [Conditional("DEBUG")] 

	public override void
	AssertValid()
	{
		base.AssertValid();

		Debug.Assert(m_oEdgeCollection != null);
		// m_oCurrentNode
		// m_oEnumeratedEdgeIDs
	}


	//*************************************************************************
	//	Protected fields
	//*************************************************************************

	/// Collection being enumerated.

	protected EdgeCollection m_oEdgeCollection;

	/// LinkedList node that contains the IEdge to return from the next call
	/// to Current(), or null if MoveNext() hasn't been called yet or the end
	/// of the LinkedList has been reached.

	protected LinkedListNode<IEdge> m_oCurrentNode;

	/// Dictionary of edges that have already been enumerated, or null if
	/// MoveNext() hasn't been called yet.  The keys are edge IDs and the
	/// values aren't used and are set to 0.
	///
	/// An "enumerated" edge is one that has already been returned by the
	/// Current property.

	protected Dictionary<Int32, Byte> m_oEnumeratedEdgeIDs;
}
}
}
