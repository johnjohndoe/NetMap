
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//	Class: PropertiesListView
//
/// <summary>
/// Represents a two-column ListView that displays property names and values.
/// </summary>
///
/// <remarks>
/// Call <see cref="Initialize" /> to fill the ListView with property items
/// that have names but no values.  You can then call <see cref="SetValues" />
/// to fill in the value column.  You can also call <see cref="ClearValues" />
/// to clear the value column.
///
/// <para>
/// If you want to reinitialize the ListView, call <see
/// cref="ListView.Clear" /> before calling <see cref="Initialize" /> again.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class PropertiesListView : ListViewPlus
{
	//*************************************************************************
	//	Constructor: PropertiesListView()
	//
	/// <summary>
	/// Initializes a new instance of the PropertiesListView class.
	/// </summary>
	//*************************************************************************

	public PropertiesListView()
		: base()
	{
		// (Do nothing.)
	}

	//*************************************************************************
	//	Method: Initialize()
	//
	/// <summary>
	/// Initializes the ListView.
	/// </summary>
	///
	/// <param name="iNameColumnWidthPx">
	/// Width of the name column, in pixels.
	/// </param>
	///
	/// <param name="iValueColumnWidthPx">
	/// Width of the value column, in pixels.
	/// </param>
	///
	/// <param name="asPropertyNames">
	/// One or more property names.  An item is added to the ListView for each
	/// property name.
	/// </param>
	///
	/// <remarks>
	/// This must be called before <see cref="SetValues" /> or
	/// <see cref="ClearValues" /> is called.  It adds property items to the
	/// ListView.  The items have names but no values.
	///
	/// <para>
	/// Call <see cref="SetValues" /> to fill in the values.  Call
	/// <see cref="ClearValues" /> to clear the values.
	/// </para>
	///
	/// <para>
	/// If you want to reinitialize the ListView, call <see
	/// cref="ListView.Clear" /> before calling this method again.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public void
	Initialize
	(
		Int32 iNameColumnWidthPx,
		Int32 iValueColumnWidthPx,
		params String[] asPropertyNames
	)
	{
		Int32 iPropertyNames = asPropertyNames.Length;
		Debug.Assert(iPropertyNames > 0);

		this.FullRowSelect = true;
		this.MultiSelect = false;
		this.View = System.Windows.Forms.View.Details;

		// Add the column headers.

		ListView.ColumnHeaderCollection oColumnHeaders = this.Columns;
		Debug.Assert(oColumnHeaders.Count == 0);

		oColumnHeaders.Add("Property", iNameColumnWidthPx,
			HorizontalAlignment.Left);

		oColumnHeaders.Add("Value", iValueColumnWidthPx,
			HorizontalAlignment.Right);

		// For each property, add an item with a name and an empty value.

		ListView.ListViewItemCollection oItems = this.Items;
		Debug.Assert(oItems.Count == 0);

		for (Int32 i = 0; i < iPropertyNames; i++)
		{
			ListViewItem oItem = oItems.Add( asPropertyNames[i] );
			oItem.SubItems.Add("");
		}

		// The right-click context menu should include a command for copying
		// the Value column.

		this.ColumnToCopy = 1;

		AssertValid();
	}

	//*************************************************************************
	//	Method: SetValues()
	//
	/// <summary>
	/// Sets the property values.
	/// </summary>
	///
	/// <param name="asPropertyValues">
	/// Property values.  The number and order of values must be the same as
	/// the property names passed to <see cref="Initialize" />.
	/// </param>
	///
	/// <remarks>
	/// You must call <see cref="Initialize" /> before calling this method.
	/// </remarks>
	//*************************************************************************

	public void
	SetValues
	(
		params String[] asPropertyValues
	)
	{
		AssertValid();

		Int32 iPropertyValues = asPropertyValues.Length;
		ListView.ListViewItemCollection oItems = this.Items;
		Debug.Assert(asPropertyValues.Length == oItems.Count);

		// For each property item, set the value column.

		for (Int32 i = 0; i < iPropertyValues; i++)
			oItems[i].SubItems[1].Text = asPropertyValues[i];
	}

	//*************************************************************************
	//	Method: ClearValues()
	//
	/// <summary>
	/// Sets the property values to an empty string.
	/// </summary>
	///
	/// <remarks>
	/// This can be called to clear the values column when there are no
	/// property values to display.  Calling this reverts the ListView to the
	/// state it was in after <see cref="Initialize" /> was called.
	///
	/// <para>
	/// You must call <see cref="Initialize" /> before calling this method.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public void
	ClearValues()
	{
		AssertValid();

		ListView.ListViewItemCollection oItems = this.Items;
		Int32 iPropertyValues = oItems.Count;

		// For each property item, set the value column to an empty string.

		for (Int32 i = 0; i < iPropertyValues; i++)
			oItems[i].SubItems[1].Text = "";
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
