
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: BinaryHeapItem
//
/// <summary>
/// Represents an item stored on a <see cref="BinaryHeap{TKey, TValue}" />.
///
/// <para>
/// The item has a unique <see cref="Key" /> and a <see cref="Value" /> that
/// corresponds to the <see cref="Key" />.  The items are sorted within the
/// <see cref="BinaryHeap{TKey, TValue}" /> by <see cref="Value" />.
/// </para>
///
/// </summary>
//*****************************************************************************

public class BinaryHeapItem<TKey, TValue> : Object
{
    //*************************************************************************
    //  Constructor: BinaryHeapItem()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
	/// cref="BinaryHeapItem{TKey, TValue}" /> class.
    /// </overloads>
	///
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="BinaryHeapItem{TKey, TValue}" /> class with default values.
    /// </summary>
    //*************************************************************************

    public BinaryHeapItem()
	:
	this( default(TKey), default(TValue) )
    {
		// (Do nothing else.)

		// AssertValid();
    }

    //*************************************************************************
    //  Constructor: BinaryHeapItem()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="BinaryHeapItem{TKey, TValue}" /> class with specified values.
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

    public BinaryHeapItem
	(
		TKey key,
		TValue value
	)
    {
		m_oKey = key;
		m_oValue = value;

		AssertValid();
    }

    //*************************************************************************
    //  Property: Key
    //
    /// <summary>
    /// Gets or sets the item's unique key.
    /// </summary>
    ///
    /// <value>
    /// The item's unique key, as a TKey.
    /// </value>
    //*************************************************************************

    public TKey
    Key
    {
        get
        {
            AssertValid();

            return (m_oKey);
        }

        set
        {
			m_oKey = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Value
    //
    /// <summary>
    /// Gets or sets the item's value.
    /// </summary>
    ///
    /// <value>
    /// The item's value, as a TValue.
    /// </value>
    //*************************************************************************

    public TValue
    Value
    {
        get
        {
            AssertValid();

            return (m_oValue);
        }

        set
        {
			m_oValue = value;

            AssertValid();
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
		// m_oKey
		// m_oValue
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Key.

	protected TKey m_oKey;

	/// Value.

	protected TValue m_oValue;
}

}
