
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//	Class: MenuItemWithObject
//
/// <summary>
/// Represents a menu item that contains an arbitrary object.
/// </summary>
///
/// <remarks>
/// An object of this class can be used in place of a MenuItem when the menu
/// item needs to be associated with some object.
/// </remarks>
//*****************************************************************************

public class MenuItemWithObject : MenuItem
{
	//*************************************************************************
	//	Constructor: MenuItemWithObject()
	//
	/// <summary>
	/// Initializes a new instance of the MenuItemWithObject class.
	/// </summary>
	///
	/// <param name="sText">
	///	The caption for the menu item.
	/// </param>
	///
	/// <param name="oClickEventHandler">
	///	Event handler for the Click event.
	/// </param>
	///
	/// <param name="oObject">
	///	Arbitrary object associated with the menu item.  Can be null.
	/// </param>
	//*************************************************************************

	public MenuItemWithObject
	(
		String sText,
		EventHandler oClickEventHandler,
		Object oObject

	) : base (sText, oClickEventHandler)
	{
		m_oObject = oObject;
	}

	//*************************************************************************
	//	Property: Object
	//
	/// <summary>
	/// Gets the arbitrary object associated with the menu item.
	/// </summary>
	///
	/// <value>
	/// The menu item's arbitrary object.  Can be null.
	/// </value>
	//*************************************************************************

	public Object Object
	{
		get
		{
			AssertValid();

			return (m_oObject);
		}
	}

	//*************************************************************************
	//	Method: AssertValid()
	//
	/// <summary>
	/// Asserts if the object is in an invalid state.  Debug-only.
	/// </summary>
	//*************************************************************************

	[Conditional("DEBUG")]

	public virtual void
	AssertValid()
	{
		// (Do nothing.)
	}


	//*************************************************************************
	//	Protected fields
	//*************************************************************************

	/// Arbitrary object.  Can be null.

	protected Object m_oObject;
}

}
