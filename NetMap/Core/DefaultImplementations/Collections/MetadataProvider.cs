
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.NetMap.Core
{
//*****************************************************************************
//  Class: MetadataProvider
//
/// <summary>
/// Provides metadata for an instance of a class that implements <see
/// cref="IMetadataProvider" />.
/// </summary>
///
/// <remarks>
/// This can be used by a client class that must implement the <see
/// cref="IMetadataProvider" /> interface.  It stores the Tag and all arbitrary
/// key/value pairs for one instance of the client class.
///
/// <para>
/// <b>Important Note:</b>
/// </para>
///
/// <para>
/// The key/value pairs are stored in a LinkedList.  To reduce memory usage,
/// this class derives from LinkedList instead of storing a private reference
/// to a LinkedList.  However, none of the public properties or methods of
/// LinkedList should be called by the client.  <b>The only properties and
/// methods that should be called are those that are defined in this derived
/// class.</b>
/// </para>
///
/// </remarks>
//
//	Implementation notes:
//
//  A previous design used one shared LinkedList to store all key/value pairs
//  for ALL instances of the client class.  This used less memory, but it was
//  too difficult to remove key/value pairs for instances that went out of
//  scope.  It was a choice between letting the LinkedList grow without bounds,
//  or using client class destructors to remove the key/value pairs.  The
//  problem with destructors is that they run on a different thread, which
//  meant the LinkedList had to be made safe for multhreading.  This was too
//  complicated and bug-prone.
//
//	Another previous design used a Dictionary instead of a LinkedList.  This
//  used more memory, because an Dictionary requires 56 bytes while an empty
//  LinkedList requires 32.
//*****************************************************************************

public class MetadataProvider : LinkedList< KeyValuePair<String, Object> >
{
    //*************************************************************************
    //  Constructor: MetadataProvider()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="MetadataProvider" />
	/// class.
    /// </summary>
    //*************************************************************************

    public MetadataProvider()
    {
		m_oTag = null;

		AssertValid();
    }

	//*************************************************************************
	//	Property: Tag
	//
	/// <summary>
	///	Gets or sets a single metadata object.
	/// </summary>
	///
	/// <value>
	///	A single metadata object, as an <see cref="Object" />.  Can be null.
	/// The default value is null.
	/// </value>
	//*************************************************************************

	public Object
	Tag
	{
		get
		{
			AssertValid();

			return (m_oTag);
		}

		set
		{
			m_oTag = value;

			AssertValid();
		}
	}

	//*************************************************************************
	//	Method: ContainsKey
	//
	/// <summary>
	/// Determines whether a metadata value with a specified key exists.
	/// </summary>
	///
	/// <param name="key">
	/// The value's key.  Can't be null or empty.
	/// </param>
	///
	/// <returns>
	/// true if there is a metadata value with the key <paramref name="key" />,
	/// false if not.
	/// </returns>
	///
	/// <remarks>
	/// If true is returned, the metadata value can be retrieved via <see
	/// cref="TryGetValue" />.
	/// </remarks>
	//*************************************************************************

	public Boolean
	ContainsKey
	(
		String key
	)
	{
		AssertValid();
		Debug.Assert( !String.IsNullOrEmpty(key) );

		for (
			LinkedListNode< KeyValuePair<String, Object> > oNode = this.First;
			oNode != null;
			oNode = oNode.Next
			)
		{
			if (oNode.Value.Key == key)
			{
				return (true);
			}
		}

		return (false);
	}

	//*************************************************************************
	//	Method: RemoveKey
	//
	/// <summary>
	/// Removes a specified metadata key if it exists.
	/// </summary>
	///
	/// <param name="key">
	/// The key to remove.  Can't be null or empty.
	/// </param>
	///
	/// <returns>
	/// true if the metadata key <paramref name="key" /> was removed, false if
	/// there is no such key.
	/// </returns>
	//*************************************************************************

	public Boolean
	RemoveKey
	(
		String key
	)
	{
		AssertValid();
		Debug.Assert( !String.IsNullOrEmpty(key) );

		for (
			LinkedListNode< KeyValuePair<String, Object> > oNode = this.First;
			oNode != null;
			oNode = oNode.Next
			)
		{
			if (oNode.Value.Key == key)
			{
				this.Remove(oNode);

				return (true);
			}
		}

		return (false);
	}

	//*************************************************************************
	//	Method: SetValue
	//
	/// <summary>
	/// Sets the metadata value associated with a specified key. 
	/// </summary>
	///
	/// <param name="key">
	/// The value's key.  Can't be null or empty.  If the key already exists,
	/// its value gets overwritten.
	/// </param>
	///
	/// <param name="value">
	/// The value to set.  Can be null.
	/// </param>
	//*************************************************************************

	public void
	SetValue
	(
		String key,
		Object value
	)
	{
		AssertValid();
		Debug.Assert( !String.IsNullOrEmpty(key) );

		LinkedListNode< KeyValuePair<String, Object> > oNode;

		for (
			oNode = this.First;
			oNode != null;
			oNode = oNode.Next
			)
		{
			if (oNode.Value.Key == key)
			{
				// The LinkedList contains the key, which means the caller
				// wants to set an already-existing value.  Replace the
				// existing KeyValuePair with one that contains the new value.
				// (Don't try setting the existing KeyValuePair.Value to the
				// new value, because KeyValuePair.Value is read-only.

				oNode.Value = new KeyValuePair<String, Object>(key, value);

				return;
			}
		}

		// There is no node for the specified composite key.  Create one.

		oNode = new LinkedListNode< KeyValuePair<String, Object> >( 
			new KeyValuePair<String, Object>(key, value) );

		this.AddLast(oNode);
	}

	//*************************************************************************
	//	Method: TryGetValue
	//
	/// <summary>
	/// Attempts to get the metadata value associated with a specified key. 
	/// </summary>
	///
	/// <param name="key">
	/// The value's key.  Can't be null or empty.
	/// </param>
	///
	/// <param name="value">
	/// Where the metadata value associated with <paramref name="key" /> gets
	/// stored if true is returned, as an <see cref="Object" />.
	/// </param>
	///
	/// <returns>
	/// true if the metadata value associated with <paramref name="key" />
	/// exists, or false if not.
	/// </returns>
	///
	/// <remarks>
	/// Values can be null.  If <paramref name="key" /> exists and its value
	/// is null, null is stored at <paramref name="value" /> and true is
	/// returned.  If <paramref name="key" /> does not exist, false is
	/// returned.
	/// </remarks>
	//*************************************************************************

	public Boolean
	TryGetValue
	(
		String key,
		out Object value
	)
	{
		AssertValid();
		Debug.Assert( !String.IsNullOrEmpty(key) );

		value = null;

		for (
			LinkedListNode< KeyValuePair<String, Object> > oNode = this.First;
			oNode != null;
			oNode = oNode.Next
			)
		{
			if (oNode.Value.Key == key)
			{
				value = oNode.Value.Value;

				return (true);
			}
		}

		return (false);
	}

	//*************************************************************************
	//	Method: CopyTo
	//
	/// <summary>
	/// Copies the instance's <see cref="Tag" /> and arbitrary key/value pairs
	/// to another object that implements <see cref="IMetadataProvider" />.
	/// </summary>
	///
	/// <param name="otherMetadataProvider">
	/// The object to copy to.
	/// </param>
	///
    /// <param name="copyMetadataValues">
	///	If true, the key/value pairs that were set with <see cref="SetValue" />
	/// are copied to <paramref name="oOtherObject" />.  (This is a shallow
	/// copy.  The objects pointed to by the original values are NOT cloned.)
	/// If false, the key/value pairs are not copied.
    /// </param>
    ///
    /// <param name="copyTag">
	///	If true, the <see cref="IMetadataProvider.Tag" /> property on <paramref
	///	name="oOtherObject" /> is set to the same value as in this object.
	/// (This is a shallow copy.  The object pointed to by this object's <see
	/// cref="Tag" /> is NOT cloned.)  If false, the <see
	/// cref="IMetadataProvider.Tag "/> property on <paramref
	/// name="oOtherObject" /> is left at its default value of null.
    /// </param>
    ///
	/// <remarks>
	/// This method can be used to assist in cloning an instance.  It copies
	/// the instances's <see cref="Tag" /> and arbitrary key/value pairs that
	/// were set with <see cref="SetValue" />.  (This is a shallow copy.  The
	/// objects pointed to by the original values are NOT cloned.)
	/// </remarks>
	//*************************************************************************

	public void
	CopyTo
	(
		IMetadataProvider otherMetadataProvider,
		Boolean copyMetadataValues,
        Boolean copyTag
	)
	{
		AssertValid();
		Debug.Assert(otherMetadataProvider != null);

		if (copyMetadataValues)
		{
			for (
				LinkedListNode< KeyValuePair<String, Object> > oNode =
					this.First;
				oNode != null;
				oNode = oNode.Next
				)
			{
				otherMetadataProvider.SetValue(
					oNode.Value.Key, oNode.Value.Value);
			}
		}

		if (copyTag)
		{
			otherMetadataProvider.Tag = this.Tag;
		}
	}

	//*************************************************************************
	//	Method: AppendToString()
	//
	/// <summary>
	/// Appends the key/value pairs to a String.
	/// </summary>
	///
	/// <param name="stringBuilder">
	/// Object to append to.
	/// </param>
	///
    /// <param name="indentationLevel">
	/// Current indentation level.  Level 0 is "no indentation."
    /// </param>
    ///
	/// <param name="format">
	/// The format to use, either "G", "P", or "D".  See <see
	/// cref="NetMapBase.ToString()" /> for details.
	/// </param>
	//*************************************************************************

	public void
	AppendToString
	(
		StringBuilder stringBuilder,
		Int32 indentationLevel,
		String format
	)
	{
		AssertValid();
		Debug.Assert(stringBuilder != null);
		Debug.Assert(indentationLevel >= 0);
		Debug.Assert( !String.IsNullOrEmpty(format) );
		Debug.Assert(format == "G" || format == "P" || format == "D");

		#if false

		Sample string for format == "G":

			2 key/value pairs

		Sample string for format == "P":

			2 key/value pairs\r\n

		Sample string for format == "D":

			2 key/value pairs\r\n
				Key = "abc", Value = "xxx"\r\n
				Key = "de", Value = 123\r\n

		#endif

		StringBuilder oDetailsStringBuilder = null;

		Boolean bAppendDetails = (format == "D");

		if (bAppendDetails)
		{
			// The details will be appended to a second StringBuilder, which
			// will get appended to stringBuilder later.  This is because the
			// first line appended to stringBuilder includes the number of
			// key/value pairs, and that isn't known until the nodes for the
			// instance have been looped through.

			oDetailsStringBuilder = new StringBuilder();
		}

		Int32 iNodes = 0;

		for (
			LinkedListNode< KeyValuePair<String, Object> > oNode = this.First;
			oNode != null;
			oNode = oNode.Next
			)
		{
			String sKey = oNode.Value.Key;

			Debug.Assert( !String.IsNullOrEmpty(sKey) );

			if (bAppendDetails)
			{
				Debug.Assert(oDetailsStringBuilder != null);

				Object oValue = oNode.Value.Value;

				// Key = "abc", Value = "xxx"

				ToStringUtil.AppendIndentationToString(
					oDetailsStringBuilder, indentationLevel + 1);

				oDetailsStringBuilder.Append("Key = ");

				oDetailsStringBuilder.Append(sKey);

				oDetailsStringBuilder.Append(", Value = ");

				ToStringUtil.AppendObjectToString(oDetailsStringBuilder,
					oValue);

				oDetailsStringBuilder.AppendLine();
			}

			iNodes++;
		}

		ToStringUtil.AppendIndentationToString(stringBuilder,
			indentationLevel);

		stringBuilder.Append( iNodes.ToString(NetMapBase.Int32Format) );

		stringBuilder.Append(" key/value pair");

		if (iNodes != 1)
		{
			stringBuilder.Append("s");
		}

		if (format != "G")
		{
			stringBuilder.AppendLine();
		}

		if (bAppendDetails)
		{
			Debug.Assert(oDetailsStringBuilder != null);

			stringBuilder.Append(oDetailsStringBuilder);
		}
	}

    //*************************************************************************
    //  Method: ClearMetadata()
    //
    /// <summary>
	/// Removes all key/value pairs and sets the <see cref="Tag" /> to null.
    /// </summary>
    //*************************************************************************

    public void
	ClearMetadata()
    {
		AssertValid();

		m_oTag = null;

		this.Clear();
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
		// m_oTag
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Object Tag, or null.

	protected Object m_oTag;
}
}
