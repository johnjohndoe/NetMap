
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: BinaryHeap
//
/// <summary>
/// Represents a binary heap.
///
/// <para>
/// The heap stores zero or more items, each of which has a unique key and a
/// value that corresponds to the key.  The items are ordered by value.  The
/// order can be either descending (for a max heap) or ascending (for a min
/// heap), depending on the IComparer interface passed to the constructor.
/// </para>
///
/// </summary>
//*****************************************************************************

public class BinaryHeap<TKey, TValue> : Object
{
    //*************************************************************************
    //  Constructor: BinaryHeap()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
	/// cref="BinaryHeap{TKey, TValue}" /> class.
    /// </overloads>
	///
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="BinaryHeap{TKey, TValue}" /> class with a default initial
	/// capacity.
    /// </summary>
	///
	/// <param name="valueComparer">
	/// IComparer interface for comparing values.
	/// </param>
    //*************************************************************************

    public BinaryHeap
	(
		IComparer<TValue> valueComparer
	)
	:
	this(0, valueComparer)
    {
		// (Do nothing else.)
    }

    //*************************************************************************
    //  Constructor: BinaryHeap()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="BinaryHeap{TKey, TValue}" /> class with a specified initial
	/// capacity.
    /// </summary>
	///
	/// <param name="initialCapacity">
	/// Initial capacity.  Must be non-negative.
	/// </param>
	///
	/// <param name="valueComparer">
	/// IComparer interface for comparing values.
	/// </param>
    //*************************************************************************

    public BinaryHeap
	(
		Int32 initialCapacity,
		IComparer<TValue> valueComparer
	)
    {
		Debug.Assert(initialCapacity >= 0);
		Debug.Assert(valueComparer != null);

		m_oItems = new List< BinaryHeapItem<TKey, TValue> >(initialCapacity);
		m_oValueComparer = valueComparer;
		m_oItemDictionary = new Dictionary<TKey, Int32>(initialCapacity);

		// AssertValid();
    }

    //*************************************************************************
    //  Property: Count
    //
    /// <summary>
    /// Gets the number of items in the binary heap.
    /// </summary>
    ///
    /// <value>
    /// The number of items in the heap, as an Int32.
    /// </value>
    //*************************************************************************

    public Int32
    Count
    {
        get
        {
            AssertValid();

            return (m_oItems.Count);
        }
    }

    //*************************************************************************
    //  Method: Add()
    //
    /// <summary>
    /// Adds an item to the binary heap.
    /// </summary>
    ///
    /// <param name="key">
	/// The item's unique key.
    /// </param>
    ///
    /// <param name="value">
	/// The item's value.
    /// </param>
    //*************************************************************************

    public void
    Add
    (
		TKey key,
		TValue value
    )
    {
        AssertValid();

		// Add an item at the bottom of the tree.

		BinaryHeapItem<TKey, TValue> oItem =
			new BinaryHeapItem<TKey, TValue>(key, value);

		m_oItems.Add(oItem);

		Int32 iItemIndex = m_oItems.Count - 1;

		m_oItemDictionary.Add(key, iItemIndex);

		// Sift up from the bottom.

		SiftUp(iItemIndex);

		AssertValid();
    }

    //*************************************************************************
    //  Method: TryGetTop()
    //
    /// <summary>
    /// Attempts to get the item at the top of the binary heap without removing
	/// it.
    /// </summary>
	///
	/// <param name="top">
	/// Where the top item gets stored if true is returned.
	/// </param>
    ///
    /// <returns>
	/// true if the binary heap has a top item, false if the binary heap is
	/// empty.
    /// </returns>
	///
	/// <remarks>
	/// The top item is either the largest or smallest item in the heap,
	/// depending on the IComparer interface passed to the constructor.
	/// </remarks>
	///
	/// <seealso cref="RemoveTop" />
    //*************************************************************************

    public Boolean
    TryGetTop
	(
		out BinaryHeapItem<TKey, TValue> top
	)
    {
        AssertValid();

		top = null;

		if (m_oItems.Count == 0)
		{
			return (false);
		}

		top = m_oItems[0];

		return (true);
    }

    //*************************************************************************
    //  Method: RemoveTop()
    //
    /// <summary>
    /// Removes the item at the top of the binary heap.
    /// </summary>
    ///
    /// <returns>
	/// The item at the top of the heap, as a <see
	/// cref="BinaryHeapItem{TKey, TValue}" />.
    /// </returns>
	///
	/// <remarks>
	/// The returned item is either the largest or smallest item in the heap,
	/// depending on the IComparer interface passed to the constructor.
	///
	/// <para>
	/// An exception is thrown if the binary heap is empty.
	/// </para>
	///
	/// </remarks>
	///
	/// <seealso cref="TryGetTop" />
    //*************************************************************************

    public BinaryHeapItem<TKey, TValue>
    RemoveTop()
    {
        AssertValid();

		const String MethodName = "RemoveTop";

		Int32 iItems = m_oItems.Count;

		if (iItems == 0)
		{
			Debug.Assert(false);

			throw new InvalidOperationException( String.Format(

				"{0}.{1}: The heap is empty.",

				this.ClassName,
				MethodName
				) );
		}

		BinaryHeapItem<TKey, TValue> oRemovedItem = m_oItems[0];

		Int32 iLastItemIndex = iItems - 1;

		// Swap the first item with the last item.

		SwapItems(0, iLastItemIndex);

		// Remove the last item and update the item dictionary.

		m_oItems.RemoveAt(iLastItemIndex);
		m_oItemDictionary.Remove(oRemovedItem.Key);

		if (m_oItems.Count > 0)
		{
			// Sift down from the top.

			SiftDown(0);
		}

		AssertValid();

		return (oRemovedItem);
    }

    //*************************************************************************
    //  Method: Remove()
    //
    /// <summary>
    /// Removes a specified item from the binary heap.
    /// </summary>
	///
    /// <param name="key">
	/// The unique key of the item to remove.
    /// </param>
    ///
	/// <remarks>
	/// If the item doesn't exist, this method does nothing.
	/// </remarks>
    //*************************************************************************

    public void
    Remove
	(
		TKey key
	)
    {
        AssertValid();

		Int32 iRemovedItemIndex;

		if ( !m_oItemDictionary.TryGetValue(key, out iRemovedItemIndex) )
		{
			// The item doesn't exist.

			return;
		}

		BinaryHeapItem<TKey, TValue> oRemovedItem =
			m_oItems[iRemovedItemIndex];

		Int32 iItems = m_oItems.Count;
		Int32 iLastItemIndex = iItems - 1;
		BinaryHeapItem<TKey, TValue> oLastItem = m_oItems[iLastItemIndex];

		// Swap the specified item with the last item.

		SwapItems(iRemovedItemIndex, iLastItemIndex);

		// Remove the last item and update the item dictionary.

		m_oItems.RemoveAt(iLastItemIndex);
		m_oItemDictionary.Remove(oRemovedItem.Key);

		if (iRemovedItemIndex != iLastItemIndex)
		{
			Int32 iComparison =
				m_oValueComparer.Compare(oLastItem.Value, oRemovedItem.Value);

			if (iComparison > 0)
			{
				SiftUp(iRemovedItemIndex);
			}
			else if (iComparison < 0)
			{
				SiftDown(iRemovedItemIndex);
			}
		}

		AssertValid();
    }

    //*************************************************************************
    //  Method: UpdateValue()
    //
    /// <summary>
    /// Updates the value of an existing item in the binary heap.
    /// </summary>
    ///
    /// <param name="existingKey">
	/// The existing item's key.
    /// </param>
    ///
    /// <param name="newValue">
	/// The item's new value.
    /// </param>
	///
	/// <remarks>
	/// An exception is thrown if the item doesn't exist.
	/// </remarks>
    //*************************************************************************

    public void
    UpdateValue
    (
		TKey existingKey,
		TValue newValue
    )
    {
        AssertValid();

		const String MethodName = "UpdateValue";

		Int32 iExistingItemIndex =
			GetExistingItemIndex(existingKey, MethodName);

		BinaryHeapItem<TKey, TValue> oExistingItem =
			m_oItems[iExistingItemIndex];

		TValue oExistingValue = oExistingItem.Value;

		oExistingItem.Value = newValue;

		Int32 iComparison =
			m_oValueComparer.Compare(newValue, oExistingValue);

		if (iComparison > 0)
		{
			SiftUp(iExistingItemIndex);
		}
		else if (iComparison < 0)
		{
			SiftDown(iExistingItemIndex);
		}

		AssertValid();
    }

    //*************************************************************************
    //  Method: Clear()
    //
    /// <summary>
    /// Removes all items from the binary heap.
    /// </summary>
    //*************************************************************************

    public void
    Clear()
    {
        AssertValid();

		m_oItems.Clear();
		m_oItemDictionary.Clear();
    }

	//*************************************************************************
	//	Property: ClassName
	//
	/// <summary>
	/// Gets the full name of this class.
	/// </summary>
	///
	/// <value>
	/// The full name of this class, suitable for use in error messages.
	/// </value>
	//*************************************************************************

	protected String
	ClassName
	{
		get
		{
			// Don't use this.GetType().FullName, which returns a bunch of
			// unwanted information for generic classes.

			return ("BinaryHeap");
		}
	}

    //*************************************************************************
    //  Method: GetExistingItemIndex()
    //
    /// <summary>
	/// Gets the index of an existing item in the binary heap.
    /// </summary>
    ///
    /// <param name="oExistingKey">
	/// The existing item's key.
    /// </param>
    ///
    /// <param name="sMethodName">
	/// Name of the calling method.
    /// </param>
	///
	/// <returns>
	/// The index of the specified item.
	/// </returns>
	///
	/// <remarks>
	/// An exception is thrown if the item doesn't exist.
	/// </remarks>
    //*************************************************************************

    protected Int32
    GetExistingItemIndex
    (
		TKey oExistingKey,
		String sMethodName
    )
    {
		Debug.Assert( !String.IsNullOrEmpty(sMethodName) );
        AssertValid();

		Int32 iExistingItemIndex;

		if ( !m_oItemDictionary.TryGetValue(oExistingKey,
			out iExistingItemIndex) )
		{
			Debug.Assert(false);

			throw new InvalidOperationException( String.Format(

				"{0}.{1}: The heap does not contain the specified key.",

				this.ClassName,
				sMethodName
				) );
		}

		return (iExistingItemIndex);
    }

    //*************************************************************************
    //  Method: TryGetLeftChild()
    //
    /// <summary>
    /// Attempts to get an item's left child.
    /// </summary>
    ///
    /// <param name="iItemIndex">
	/// The item's index.
    /// </param>
    ///
    /// <param name="oLeftChildItem">
	/// Where the item's left child gets stored if true is returned.
    /// </param>
	///
    /// <param name="iLeftChildIndex">
	/// Where the item's left child index gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
	/// true if the item has a left child.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetLeftChild
    (
		Int32 iItemIndex,
		out BinaryHeapItem<TKey, TValue> oLeftChildItem,
		out Int32 iLeftChildIndex
    )
    {
		Debug.Assert(iItemIndex >= 0);
		Debug.Assert(iItemIndex < m_oItems.Count);
        AssertValid();

		return ( TryGetLeftOrRightChild(iItemIndex, true,
			out oLeftChildItem, out iLeftChildIndex) );
    }

    //*************************************************************************
    //  Method: TryGetRightChild()
    //
    /// <summary>
    /// Attempts to get an item's right child.
    /// </summary>
    ///
    /// <param name="iItemIndex">
	/// The item's index.
    /// </param>
    ///
    /// <param name="oRightChildItem">
	/// Where the item's right child gets stored if true is returned.
    /// </param>
	///
    /// <param name="iRightChildIndex">
	/// Where the item's right child index gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
	/// true if the item has a right child.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetRightChild
    (
		Int32 iItemIndex,
		out BinaryHeapItem<TKey, TValue> oRightChildItem,
		out Int32 iRightChildIndex
    )
    {
		Debug.Assert(iItemIndex >= 0);
		Debug.Assert(iItemIndex < m_oItems.Count);
        AssertValid();

		return ( TryGetLeftOrRightChild(iItemIndex, false,
			out oRightChildItem, out iRightChildIndex) );
    }

    //*************************************************************************
    //  Method: TryGetParent()
    //
    /// <summary>
    /// Attempts to get an item's parent.
    /// </summary>
    ///
    /// <param name="iItemIndex">
	/// The item's index.
    /// </param>
    ///
    /// <param name="oParentItem">
	/// Where the item's parent gets stored if true is returned.
    /// </param>
	///
    /// <param name="iParentIndex">
	/// Where the item's parent index gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
	/// true if the item has a parent.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetParent
    (
		Int32 iItemIndex,
		out BinaryHeapItem<TKey, TValue> oParentItem,
		out Int32 iParentIndex
    )
    {
		Debug.Assert(iItemIndex >= 0);
		Debug.Assert(iItemIndex < m_oItems.Count);
        AssertValid();

		iParentIndex = Int32.MinValue;
		oParentItem = null;

		if (iItemIndex == 0)
		{
			return (false);
		}

		iParentIndex = (iItemIndex - 1) / 2;
		oParentItem = m_oItems[iParentIndex];

		return (true);
    }

    //*************************************************************************
    //  Method: SiftUp()
    //
    /// <summary>
    /// Sifts the binary heap up from a specified index.
    /// </summary>
    ///
    /// <param name="iItemIndex">
	/// The index of the item to start at.
    /// </param>
    //*************************************************************************

    protected void
    SiftUp
    (
		Int32 iItemIndex
    )
    {
		Debug.Assert(iItemIndex >= 0);
		Debug.Assert(iItemIndex < m_oItems.Count);
        AssertValid();

		if (iItemIndex == 0)
		{
			return;
		}

		BinaryHeapItem<TKey, TValue> oItem = m_oItems[iItemIndex];

		Int32 iParentIndex;
		BinaryHeapItem<TKey, TValue> oParentItem;

		while ( TryGetParent(iItemIndex, out oParentItem, out iParentIndex) )
		{
			// Compare the item with its parent.

			if (m_oValueComparer.Compare(oParentItem.Value, oItem.Value) >= 0)
			{
				// The heap property is satisfied.

				break;
			}

			// Swap the item with its parent.

			SwapItems(iItemIndex, iParentIndex);

			iItemIndex = iParentIndex;
		}
    }

    //*************************************************************************
    //  Method: SiftDown()
    //
    /// <summary>
    /// Sifts the binary heap down from a specified index.
    /// </summary>
    ///
    /// <param name="iItemIndex">
	/// The index of the item to start at.
    /// </param>
    //*************************************************************************

    protected void
    SiftDown
	(
		Int32 iItemIndex
	)
    {
		Debug.Assert(iItemIndex >= 0);
		Debug.Assert(iItemIndex < m_oItems.Count);
        AssertValid();

		BinaryHeapItem<TKey, TValue> oItem = m_oItems[iItemIndex];

		while (true)
		{
			BinaryHeapItem<TKey, TValue> oLeftChildItem;
			Int32 iLeftChildIndex;

			if ( !TryGetLeftChild(iItemIndex, out oLeftChildItem,
				out iLeftChildIndex) )
			{
				// We're at the bottom of the tree.

				break;
			}

			// Assume that we need to compare the parent to its left child.

			Int32 iChildItemIndexToCompareTo = iLeftChildIndex;

			BinaryHeapItem<TKey, TValue> oChildItemToCompareTo =
				oLeftChildItem;

			BinaryHeapItem<TKey, TValue> oRightChildItem;
			Int32 iRightChildIndex;

			if ( TryGetRightChild(iItemIndex, out oRightChildItem,
				out iRightChildIndex) )
			{
				if (m_oValueComparer.Compare(oRightChildItem.Value,
					oLeftChildItem.Value) >= 0)
				{
					// We need to compare the parent to its right child.

					oChildItemToCompareTo = oRightChildItem;
					iChildItemIndexToCompareTo = iRightChildIndex;
				}
			}

			if (m_oValueComparer.Compare(oItem.Value,
				oChildItemToCompareTo.Value) >= 0)
			{
				// The heap property is satisfied.

				break;
			}

			// Swap the item with its left or right child.

			SwapItems(iItemIndex, iChildItemIndexToCompareTo);

			iItemIndex = iChildItemIndexToCompareTo;
		}
    }

    //*************************************************************************
    //  Method: SwapItems()
    //
    /// <summary>
    /// Swaps two items.
    /// </summary>
    ///
    /// <param name="iItem1Index">
	/// The index of the first item to swap.
    /// </param>
    ///
    /// <param name="iItem2Index">
	/// The index of the second item to swap.
    /// </param>
    //*************************************************************************

    protected void
    SwapItems
    (
		Int32 iItem1Index,
		Int32 iItem2Index
    )
    {
		Debug.Assert(iItem1Index >= 0);
		Debug.Assert(iItem1Index < m_oItems.Count);
		Debug.Assert(iItem2Index >= 0);
		Debug.Assert(iItem2Index < m_oItems.Count);
        AssertValid();

		BinaryHeapItem<TKey, TValue> oItem1 = m_oItems[iItem1Index];
		BinaryHeapItem<TKey, TValue> oItem2 = m_oItems[iItem2Index];

		m_oItems[iItem1Index] = oItem2;
		m_oItems[iItem2Index] = oItem1;

		m_oItemDictionary[oItem1.Key] = iItem2Index;
		m_oItemDictionary[oItem2.Key] = iItem1Index;
    }

    //*************************************************************************
    //  Method: TryGetLeftOrRightChild()
    //
    /// <summary>
    /// Attempts to get an item's left or right child.
    /// </summary>
    ///
    /// <param name="iItemIndex">
	/// The item's index.
    /// </param>
    ///
    /// <param name="bLeftChild">
	/// true to get the item's left child, false to get the item's right child.
    /// </param>
    ///
    /// <param name="oLeftOrRightChildItem">
	/// Where the item's left or right child gets stored if true is returned.
    /// </param>
    ///
    /// <param name="iLeftOrRightChildIndex">
	/// Where the item's left or right child index gets stored if true is
	/// returned.
    /// </param>
    ///
    /// <returns>
	/// true if the item has a left or right child.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetLeftOrRightChild
    (
		Int32 iItemIndex,
		Boolean bLeftChild,
		out BinaryHeapItem<TKey, TValue> oLeftOrRightChildItem,
		out Int32 iLeftOrRightChildIndex
    )
    {
		Debug.Assert(iItemIndex >= 0);
		Debug.Assert(iItemIndex < m_oItems.Count);
        AssertValid();

		iLeftOrRightChildIndex = 2 * iItemIndex + (bLeftChild ? 1 : 2);
		oLeftOrRightChildItem = null;

		if (iLeftOrRightChildIndex < m_oItems.Count)
		{
			oLeftOrRightChildItem = m_oItems[iLeftOrRightChildIndex];

			return (true);
		}

		iLeftOrRightChildIndex = Int32.MinValue;

		return (false);
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public virtual void
    AssertValid()
    {
		Debug.Assert(m_oItems != null);
		Debug.Assert(m_oValueComparer != null);
		Debug.Assert(m_oItemDictionary != null);
		Debug.Assert(m_oItems.Count == m_oItemDictionary.Count);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// List of items in the heap.

	protected List< BinaryHeapItem<TKey, TValue> > m_oItems;

	/// IComparer interface for comparing keys.

	protected IComparer<TValue> m_oValueComparer;

	/// The key is the BinaryHeapItem.Key and the value is the BinaryHeapItem's
	/// index within m_oItems.  This is used to update an item's value without
	/// having to do a linear search for the item.

	protected Dictionary<TKey, Int32> m_oItemDictionary;
}

}
