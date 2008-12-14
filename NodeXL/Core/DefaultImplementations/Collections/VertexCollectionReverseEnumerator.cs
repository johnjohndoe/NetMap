
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.NodeXL.Core
{
public partial class VertexCollection : NodeXLBase, IVertexCollection
{
//*****************************************************************************
//	Nested class: ReverseEnumerator
//
/// <summary>
///	Supports iterating backwards over a <see cref="VertexCollection" />.
/// </summary>
///
/// <remarks>
///	This class is nested within the <see cref="VertexCollection" /> class, so
/// its type is VertexCollection.ReverseEnumerator.  An instance of this class
/// gets returned by <see cref="VertexCollection.GetReverseEnumerable()" />.
/// </remarks>
//*****************************************************************************

protected class ReverseEnumerator :
	NodeXLBase, System.Collections.IEnumerator, System.Collections.IEnumerable
{
	//*************************************************************************
	//	Constructor: ReverseEnumerator()
	//
	/// <summary>
	///	Initializes a new instance of the ReverseEnumerator class.
	/// </summary>
	///
	/// <param name="vertexCollection">
	/// Collection being enumerated.
	/// </param>
	//*************************************************************************

	protected internal ReverseEnumerator
	(
		VertexCollection vertexCollection
	)
	{
		Debug.Assert(vertexCollection != null);

		m_oVertexCollection = vertexCollection;

		m_oCurrentNode = null;

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
	/// The <see cref="IVertex" /> object at the enumerator's current position.
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
	/// The <see cref="IVertex" /> object at the enumerator's current position.
	/// </value>
	///
	//	This is a type-safe version of IEnumerator.Current property.
	//*************************************************************************

	public IVertex
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

			m_oCurrentNode = m_oCurrentNode.Previous;
		}
		else
		{
			// Either this is the first time MoveNext() has been called, or
			// Reset() was called.

            m_oCurrentNode = m_oVertexCollection.m_oLinkedList.Last;
		}

		return (m_oCurrentNode != null);
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

		AssertValid();
	}

	//*************************************************************************
	//	Method: GetEnumerator()
	//
	/// <summary>
	/// Returns an enumerator that iterates through the collection. 
	/// </summary>
	///
	/// <returns>
	/// An enumerator that iterates through the collection. 
	/// </returns>
	//*************************************************************************

	public System.Collections.IEnumerator
	GetEnumerator()
	{
		AssertValid();

		return (this);
	}

	//*************************************************************************
	//	Method: GetCurrent()
	//
	/// <summary>
	/// Gets the object at the current position.
	/// </summary>
	///
	/// <returns>
	/// The <see cref="IVertex" /> object at the enumerator's current position.
	/// </returns>
	//*************************************************************************

	protected IVertex
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
			Debug.Assert(false);

			throw new ApplicationException(String.Format(

				"{0}.{1}: The current node has a null value."
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

		Debug.Assert(m_oVertexCollection != null);
		// m_oCurrentNode
	}


	//*************************************************************************
	//	Protected fields
	//*************************************************************************

	/// Collection being enumerated.

	protected VertexCollection m_oVertexCollection;

	/// LinkedList node that contains the IVertex to return from the next call
	/// to Current(), or null if MoveNext() hasn't been called yet or the start
	/// of the LinkedList has been reached.

	protected LinkedListNode<IVertex> m_oCurrentNode;
}
}
}
