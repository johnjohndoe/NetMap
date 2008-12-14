
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//	Class: ListControlPlus
//
/// <summary>
/// Implements methods shared by ComboBoxPlus and ListBoxPlus.
/// </summary>
///
/// <remarks>
/// This class implements commonly used functionality not provided by the
/// ComboBox and ListBox classes.  It's implemented as a set of static methods
/// so it can be used by the ComboBoxPlus and ListBoxPlus classes.  The methods
/// are not meant to be called directly from application code.
/// </remarks>
//*****************************************************************************

public class ListControlPlus : Object
{
	//*************************************************************************
	//	Constructor: ListControlPlus()
	//
	/// <summary>
	/// Do not call this constructor.
	/// </summary>
	//*************************************************************************

	private ListControlPlus()
	{
		// (Do nothing.)
	}

	//*************************************************************************
	//	Method: PopulateWithEnumValues()
	//
	/// <summary>
	///	Populates a ListControl with all values in an enumeration.
	/// </summary>
	///
	/// <param name="oListControl">
	///	ListControl to populate.
	/// </param>
	///
	/// <param name="oEnumType">
	///	Enumeration to populate the ListControl with.
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

	public static void
	PopulateWithEnumValues
	(
		ListControl oListControl,
		Type oEnumType,
		Boolean bFormatForUser
	)
	{
		Debug.Assert(oListControl != null);

		// Get an array of values in the enumeration.

		Array aoEnumValues = Enum.GetValues(oEnumType);
		Int32 iEnumValues = aoEnumValues.Length;

		// Create an array of ObjectWithText objects.

		ArrayList oObjectWithText = new ArrayList(iEnumValues);

		for (Int32 i = 0; i < iEnumValues; i++)
		{
			Object oEnumValue = aoEnumValues.GetValue(i);
			String sEnumString = oEnumValue.ToString();

			if (bFormatForUser)
			{
				sEnumString = EnumUtil.SplitName(sEnumString,
					EnumSplitStyle.FirstWordStartsUpperCase);
			}

			oObjectWithText.Add( new ObjectWithText(oEnumValue, sEnumString) );
		}

		if (bFormatForUser)
			oObjectWithText.Sort();

		// Tell the ListControl which properties on the objects in the array
		// should be used.

		oListControl.DisplayMember = "Text";
		oListControl.ValueMember = "Object";

		// Populate the ListControl.

		oListControl.DataSource = oObjectWithText;
	}

	//*************************************************************************
	//	Method: PopulateWithObjectsAndText()
	//
	/// <summary>
	/// Populates a ListControl with arbitrary objects and associated text.
	/// </summary>
	///
	/// <param name="oListControl">
	///	Object to populate.
	/// </param>
	///
	/// <param name="aoObjectTextPairs">
	///	One or more object/text pairs.  The text is what gets displayed in the
	/// ListControl.  The associated object, which can be of any type, is
	/// hidden from the user but can be retrieved using the
	/// <see cref="ListControl.SelectedValue" />
	/// property.
	/// </param>
	///
	/// <remarks>
	/// When you populate a ListControl with this method, you can set and get
	/// the selected object with the <see cref="ListControl.SelectedValue" />
	/// property.
	/// </remarks>
	///
	/// <example>
	/// See <see cref="ComboBoxPlus"/> for an example.
	/// </example>
	//*************************************************************************

	public static void
	PopulateWithObjectsAndText
	(
		ListControl oListControl,
		Object [] aoObjectTextPairs
	)
	{
		Debug.Assert(oListControl != null);

		Int32 iObjectsAndText = aoObjectTextPairs.Length;
		Debug.Assert(iObjectsAndText % 2 == 0);

		// Create an array of ObjectWithText objects.

		ArrayList oObjectWithText = new ArrayList(iObjectsAndText / 2);

		for (Int32 i = 0; i < iObjectsAndText; i += 2)
		{
			Debug.Assert( aoObjectTextPairs[i + 1].GetType()
				== typeof(String) );

			oObjectWithText.Add( new ObjectWithText( aoObjectTextPairs[i + 0],
				(String)aoObjectTextPairs[i + 1] ) );
		}

		// Tell the ListControl which properties on the ObjectWithText objects
		// should be used.

		oListControl.DisplayMember = "Text";
		oListControl.ValueMember = "Object";

		// Populate the ListControl.

		oListControl.DataSource = oObjectWithText;
	}
}

}
