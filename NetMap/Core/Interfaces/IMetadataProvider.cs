
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Diagnostics;

namespace Microsoft.NetMap.Core
{
//*****************************************************************************
//  Interface: IMetadataProvider
//
/// <summary>
/// Provides metadata.
/// </summary>
///
/// <remarks>
/// Classes that provide metadata for storing application-defined information
/// should implement this interface.
///
/// <para>
/// Metadata can be stored as a single <see cref="Object" /> via the <see
/// cref="Tag" /> property, or as key/value pairs via the <see
/// cref="SetValue" /> method, or both.
/// </para>
///
/// </remarks>
//*****************************************************************************

public interface IMetadataProvider
{
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
	///
	/// <remarks>
	///	If you want to store multiple metadata objects as key/value pairs, use
	/// <see cref="SetValue" /> instead.
	/// </remarks>
	///
	/// <seealso cref="SetValue" />
	//*************************************************************************

	Object
	Tag
	{
		get;
		set;
	}

	//*************************************************************************
	//	Method: ContainsKey
	//
	/// <summary>
	/// Determines whether a metadata value with a specified key exists.
	/// </summary>
	///
	/// <param name="key">
	/// The value's key.  Can't be null or empty, and can't start with a
	/// tilde (~).
	/// </param>
	///
	/// <returns>
	/// true if there is a metadata value with the key <paramref name="key" />,
	/// false if not.
	/// </returns>
	///
	/// <remarks>
	/// If true is returned, the metadata value can be retrieved with <see
	/// cref="GetValue(String)" /> or one of its variants.
	///
	/// <para>
	/// Keys that start with a tilde (~) are reserved by the NetMap system for
	/// internal use.
	/// </para>
	///
	/// </remarks>
	///
	/// <seealso cref="GetValue(String)" />
	/// <seealso cref="TryGetValue(String, out Object)" />
	/// <seealso cref="GetRequiredValue(String, Type)" />
	/// <seealso cref="SetValue(String, Object)" />
	//*************************************************************************

	Boolean ContainsKey
	(
		String key
	);

	//*************************************************************************
	//	Method: RemoveKey
	//
	/// <summary>
	/// Removes a specified metadata key if it exists.
	/// </summary>
	///
	/// <param name="key">
	/// The key to remove.  Can't be null or empty, and can't start with a
	/// tilde (~).
	/// </param>
	///
	/// <returns>
	/// true if the metadata key <paramref name="key" /> was removed, false if
	/// there is no such key.
	///
	/// <para>
	/// Keys that start with a tilde (~) are reserved by the NetMap system for
	/// internal use.
	/// </para>
	///
	/// </returns>
	//*************************************************************************

	Boolean
	RemoveKey
	(
		String key
	);

	//*************************************************************************
	//	Method: SetValue
	//
	/// <summary>
	/// Sets the metadata value associated with a specified key. 
	/// </summary>
	///
	/// <param name="key">
	/// The value's key.  Can't be null or empty, and can't start with a
	/// tilde (~).  If the key already exists, its value gets overwritten.
	/// </param>
	///
	/// <param name="value">
	/// The value to set.  Can be null.
	/// </param>
	///
	/// <remarks>
	/// The application can store arbitrary metadata by adding key/value pairs
	/// via <see cref="SetValue(String, Object)" />.  The values can be
	/// retrieved with <see cref="GetValue(String)" /> or one of its variants.
	/// The keys are of type <see cref="String" /> and the values are of type
	/// <see cref="Object" />.
	///
	/// <para>
	///	If you want to store just a single metadata object, use the <see
	/// cref="Tag" /> property instead.
	/// </para>
	///
	/// <para>
	/// Keys that start with a tilde (~) are reserved by the NetMap system for
	/// internal use.
	/// </para>
	///
	/// </remarks>
	///
	/// <seealso cref="ContainsKey" />
	/// <seealso cref="TryGetValue(String, out Object)" />
	/// <seealso cref="GetRequiredValue(String, Type)" />
	/// <seealso cref="GetValue(String)" />
	/// <seealso cref="Tag" />
	//*************************************************************************

	void
	SetValue
	(
		String key,
		Object value
	);

	//*************************************************************************
	//	Method: GetRequiredValue()
	//
	/// <summary>
	/// Gets the metadata value associated with a specified key and checks
	/// the value type.  The value must exist.
	/// </summary>
	///
	/// <param name="key">
	/// The value's key.  Can't be null or empty, and can't start with a
	/// tilde (~).
	/// </param>
	///
	/// <param name="valueType">
	/// Expected type of the requested value.  Sample: typeof(String).
	/// </param>
	///
	/// <returns>
	/// The metadata value associated with <paramref name="key" />, as an <see
	/// cref="Object" />.
	/// </returns>
	///
	/// <remarks>
	/// The application can store arbitrary metadata by adding key/value pairs
	/// via <see cref="SetValue(String, Object)" />.  The values can be
	/// retrieved with <see cref="GetValue(String)" /> or one of its variants.
	/// The keys are of type <see cref="String" /> and the values are of type
	/// <see cref="Object" />.
	///
	/// <para>
	/// Values can be null.  If <paramref name="key" /> exists and its value is
	/// null, null is returned.  If <paramref name="key" /> does not exist,
	/// an exception is thrown.
	/// </para>
	///
	/// <para>
	/// <paramref name="valueType" /> is used for error checking.  If the type
	/// of the requested value is not <paramref name="valueType" />, an
	/// exception is thrown.  Note that the type of the returned value is <see
	/// cref="Object" />, and that you must cast the returned object to the
	/// specified type.
	/// </para>
	///
	/// <para>
	///	If you want to store just a single metadata object, use the <see
	/// cref="Tag" /> property instead.
	/// </para>
	///
	/// <para>
	/// Keys that start with a tilde (~) are reserved by the NetMap system for
	/// internal use.
	/// </para>
	///
	/// </remarks>
	///
	/// <seealso cref="ContainsKey" />
	/// <seealso cref="SetValue" />
	/// <seealso cref="TryGetValue(String, out Object)" />
	/// <seealso cref="GetValue(String)" />
	/// <seealso cref="Tag" />
	//*************************************************************************

	Object
	GetRequiredValue
	(
		String key,
		Type valueType
	);

	//*************************************************************************
	//	Method: TryGetValue
	//
	/// <overloads>
	/// Attempts to get the metadata value associated with a specified key. 
	/// </overloads>
	///
	/// <summary>
	/// Attempts to get the metadata value associated with a specified key and
	/// checks the value type.
	/// </summary>
	///
	/// <param name="key">
	/// The value's key.  Can't be null or empty, and can't start with a
	/// tilde (~).
	/// </param>
	///
	/// <param name="valueType">
	/// Expected type of the requested value.  Sample: typeof(String).
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
	/// The application can store arbitrary metadata by adding key/value pairs
	/// via <see cref="SetValue(String, Object)" />.  The values can be
	/// retrieved with <see cref="GetValue(String)" /> or one of its variants.
	/// The keys are of type <see cref="String" /> and the values are of type
	/// <see cref="Object" />.
	///
	/// <para>
	/// Values can be null.  If <paramref name="key" /> exists and its value is
	/// null, null is stored at <paramref name="value" /> and true is returned.
	/// If <paramref name="key" /> does not exist, false is returned.
	/// </para>
	///
	/// <para>
	/// <paramref name="valueType" /> is used for error checking.  If the type
	/// of the requested value is not <paramref name="valueType" />, an
	/// exception is thrown.  Note that the type of the value stored at
	/// <paramref name="value" /> is <see cref="Object" />, and that you must
	/// cast the object to the specified type.
	/// </para>
	///
	/// <para>
	///	If you want to store just a single metadata object, use the <see
	/// cref="Tag" /> property instead.
	/// </para>
	///
	/// <para>
	/// Keys that start with a tilde (~) are reserved by the NetMap system for
	/// internal use.
	/// </para>
	///
	/// </remarks>
	///
	/// <seealso cref="ContainsKey" />
	/// <seealso cref="SetValue" />
	/// <seealso cref="GetRequiredValue(String, Type)" />
	/// <seealso cref="GetValue(String)" />
	/// <seealso cref="Tag" />
	//*************************************************************************

	Boolean
	TryGetValue
	(
		String key,
		Type valueType,
		out Object value
	);

	//*************************************************************************
	//	Method: TryGetValue
	//
	/// <summary>
	/// Attempts to get the metadata value associated with a specified key.
	/// </summary>
	///
	/// <param name="key">
	/// The value's key.  Can't be null or empty, and can't start with a
	/// tilde (~).
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
	/// The application can store arbitrary metadata by adding key/value pairs
	/// via <see cref="SetValue(String, Object)" />.  The values can be
	/// retrieved with <see cref="GetValue(String)" /> or one of its variants.
	/// The keys are of type <see cref="String" /> and the values are of type
	/// <see cref="Object" />.
	///
	/// <para>
	/// Values can be null.  If <paramref name="key" /> exists and its value is
	/// null, null is stored at <paramref name="value" /> and true is returned.
	/// If <paramref name="key" /> does not exist, false is returned.
	/// </para>
	///
	/// <para>
	///	If you want to store just a single metadata object, use the <see
	/// cref="Tag" /> property instead.
	/// </para>
	///
	/// <para>
	/// Keys that start with a tilde (~) are reserved by the NetMap system for
	/// internal use.
	/// </para>
	///
	/// </remarks>
	///
	/// <seealso cref="ContainsKey" />
	/// <seealso cref="SetValue" />
	/// <seealso cref="GetValue(String)" />
	/// <seealso cref="GetRequiredValue(String, Type)" />
	/// <seealso cref="Tag" />
	//*************************************************************************

	Boolean
	TryGetValue
	(
		String key,
		out Object value
	);

	//*************************************************************************
	//	Method: GetValue
	//
	/// <overloads>
	/// Gets the metadata value associated with a specified key. 
	/// </overloads>
	///
	/// <summary>
	/// Gets the metadata value associated with a specified key and checks
	/// the value type.
	/// </summary>
	///
	/// <param name="key">
	/// The value's key.  Can't be null or empty, and can't start with a
	/// tilde (~).
	/// </param>
	///
	/// <param name="valueType">
	/// Expected type of the requested value.  Sample: typeof(String).
	/// </param>
	///
	/// <returns>
	/// The metadata value associated with <paramref name="key" />, as an <see
	/// cref="Object" />, or null if the key doesn't exist.
	/// </returns>
	///
	/// <remarks>
	/// The application can store arbitrary metadata by adding key/value pairs
	/// via <see cref="SetValue(String, Object)" />.  The values can be
	/// retrieved with <see cref="GetValue(String)" /> or one of its variants.
	/// The keys are of type <see cref="String" /> and the values are of type
	/// <see cref="Object" />.
	///
	/// <para>
	/// Values can be null.  If <paramref name="key" /> exists and its value is
	/// null, null is returned.  If <paramref name="key" /> does not exist,
	/// null is returned.  If you need to distinguish between these two cases,
	/// use <see cref="TryGetValue(String, Type, out Object)" /> instead.
	/// </para>
	///
	/// <para>
	/// <paramref name="valueType" /> is used for error checking.  If the type
	/// of the requested value is not <paramref name="valueType" />, an
	/// exception is thrown.  Note that the type of the returned value is <see
	/// cref="Object" />, and that you must cast the returned object to the
	/// specified type.
	/// </para>
	///
	/// <para>
	///	If you want to store just a single metadata object, use the <see
	/// cref="Tag" /> property instead.
	/// </para>
	///
	/// <para>
	/// Keys that start with a tilde (~) are reserved by the NetMap system for
	/// internal use.
	/// </para>
	///
	/// </remarks>
	///
	/// <seealso cref="ContainsKey" />
	/// <seealso cref="SetValue" />
	/// <seealso cref="TryGetValue(String, out Object)" />
	/// <seealso cref="GetRequiredValue(String, Type)" />
	/// <seealso cref="Tag" />
	//*************************************************************************

	Object
	GetValue
	(
		String key,
		Type valueType
	);

	//*************************************************************************
	//	Method: GetValue
	//
	/// <summary>
	/// Gets the metadata value associated with a specified key. 
	/// </summary>
	///
	/// <param name="key">
	/// The value's key.  Can't be null or empty, and can't start with a
	/// tilde (~).
	/// </param>
	///
	/// <returns>
	/// The metadata value associated with <paramref name="key" />, as an <see
	/// cref="Object" />, or null if the key doesn't exist.
	/// </returns>
	///
	/// <remarks>
	/// The application can store arbitrary metadata by adding key/value pairs
	/// via <see cref="SetValue(String, Object)" />.  The values can be
	/// retrieved with <see cref="GetValue(String)" /> or one of its variants.
	/// The keys are of type <see cref="String" /> and the values are of type
	/// <see cref="Object" />.
	///
	/// <para>
	/// Values can be null.  If <paramref name="key" /> exists and its value is
	/// null, null is returned.  If <paramref name="key" /> does not exist,
	/// null is returned.  If you need to distinguish between these two cases,
	/// use <see cref="TryGetValue(String, out Object)" /> instead.
	/// </para>
	///
	/// <para>
	///	If you want to store just a single metadata object, use the <see
	/// cref="Tag" /> property instead.
	/// </para>
	///
	/// <para>
	/// Keys that start with a tilde (~) are reserved by the NetMap system for
	/// internal use.
	/// </para>
	///
	/// </remarks>
	///
	/// <seealso cref="ContainsKey" />
	/// <seealso cref="SetValue" />
	/// <seealso cref="TryGetValue(String, out Object)" />
	/// <seealso cref="GetRequiredValue(String, Type)" />
	/// <seealso cref="Tag" />
	//*************************************************************************

	Object
	GetValue
	(
		String key
	);
}

}
