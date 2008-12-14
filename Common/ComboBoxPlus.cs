
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//	Class: ComboBoxPlus
//
/// <summary>
/// Represents a ComboBox with additional features.
/// </summary>
//*****************************************************************************

public class ComboBoxPlus : ComboBox
{
	//*************************************************************************
	//	Constructor: ComboBoxPlus()
	//
	/// <summary>
	///	Initializes a new instance of the ComboBoxPlus class.
	/// </summary>
	//*************************************************************************

	public ComboBoxPlus()
	{
		// (Do nothing.)
	}

	//*************************************************************************
	//	Method: PopulateWithEnumValues()
	//
	/// <summary>
	///	Populates the ComboBox with all values in an enumeration.
	/// </summary>
	///
	/// <param name="oEnumType">
	///	Enumeration to populate the ComboBox with.
	/// </param>
	///
	/// <param name="bFormatForUser">
	///	If true, spaces are inserted between each word in the enum values, and
	/// the values are sorted alphabetically.  The value
	/// "DaysActiveInNewsgroup" gets displayed as "Days active in newsgroup",
	/// for example.  If false, the values are displayed as is
	/// ("DaysActiveInNewsgroup", for example), and no sorting is done.
	/// </param>
	///
	///	<remarks>
	///	This method populates a ListControl with all values in an enumeration.
	/// The user sees the string version of each value in the list.  The
	/// <see cref="ListControl.SelectedValue" /> property returns the selected
	/// value in the enumeration.
	///	</remarks>
	//*************************************************************************

	public void
	PopulateWithEnumValues
	(
		Type oEnumType,
		Boolean bFormatForUser
	)
	{
		ListControlPlus.PopulateWithEnumValues(this, oEnumType, bFormatForUser);
	}

	//*************************************************************************
	//	Method: PopulateWithObjectsAndText()
	//
	/// <summary>
	/// Populates the ComboBox with arbitrary objects and associated text.
	/// </summary>
	///
	/// <param name="aoObjectTextPairs">
	///	One or more object/text pairs.  The text is what gets displayed in the
	/// ComboBox.  The associated object, which can be of any type, is hidden
	/// from the user but can be retrieved using the <see
	/// cref="ListControl.SelectedValue" /> property.
	/// </param>
	///
	/// <remarks>
	/// When you populate the ComboBox with this method, you can set and get
	/// the selected object with the <see cref="ListControl.SelectedValue" />
	/// property.
	/// </remarks>
	///
	/// <example>
	/// This example populates a combo box with three items.  The user sees
	/// "None", "10%", and "20%" in the list.  The
	/// <see cref="ListControl.SelectedValue" /> property returns either 0, 0.1,
	/// or 0.2, depending on which item the user has selected.
	/// <code>
	///
	/// oComboBox.PopulateWithObjectsAndText(0, "None", 0.1, "10%", 0.2, "20%");
	///	
	/// </code>
	/// </example>
	//*************************************************************************

	public void
	PopulateWithObjectsAndText
	(
		params Object [] aoObjectTextPairs
	)
	{
		ListControlPlus.PopulateWithObjectsAndText(this, aoObjectTextPairs);
	}
}

}
