
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//	Class: StringMruList
//
/// <summary>
/// Represents a most-recently-used list of strings.
/// </summary>
///
/// <remarks>
/// This is an MruList object specialized for strings.  It can be serialized.
/// </remarks>
//*****************************************************************************

public class StringMruList : MruList
{
	//*************************************************************************
	//	Constructor: StringMruList()
	//
	/// <summary>
	/// Initializes a new instance of the StringMruList class.
	/// </summary>
	///
	/// <param name="iCapacity">
	/// Maximum number of strings in the list.
	/// </param>
	//*************************************************************************

	public StringMruList
	(
		Int32 iCapacity
	)
	: base(iCapacity)
	{
		// (Do nothing.)
	}

	//*************************************************************************
	//	Constructor: StringMruList()
	//
	/// <summary>
	/// Initializes a new instance of the StringMruList class.
	/// </summary>
	///
	/// <remarks>
	/// This paramaterless constructor is required by the XmlSerializer.
	/// </remarks>
	//*************************************************************************

	public StringMruList()
	: base()
	{
		// (Do nothing else.)
	}

	//*************************************************************************
	//	Property: Array
	//
	/// <summary>
	/// Copies the strings in the list to a new String array.
	/// </summary>
	///
	///	<value>
	/// An array containing the strings in the list.
	///	</value>
	///
	/// <remarks>
	/// The newest string is at index 0.
	/// </remarks>
	//*************************************************************************

	[System.Xml.Serialization.XmlArray("Items")]

	[System.Xml.Serialization.XmlArrayItemAttribute(typeof(String),
		 ElementName = "Item")]

	public new String [] Array
	{
		get
		{
			return ( ( String [] )m_oObjects.ToArray( typeof(String) ) );
		}

		set
		{
			// Do not call this.  This is for the XmlSerializer only.

			base.Array = value;
		}
	}

	//*************************************************************************
	//	Method: AssertValid()
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
	}
}

}
