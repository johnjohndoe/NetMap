
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ColorConverter2
//
/// <summary>
/// Class that converts a color between values used in the Excel workbook and
/// values used in the NodeXL graph.
/// </summary>
///
/// <remarks>
/// This is called ColorConverter2 to distinguish it from
/// System.Drawing.ColorConverter without requiring the specification of a
/// namespace.
///
/// <para>
/// Colors are treated differently from the other value types that derive from
/// TextValueConverterBase, so this class doesn't derive from that base class.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class ColorConverter2 : Object
{
    //*************************************************************************
    //  Constructor: ColorConverter2()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ColorConverter2" /> class.
    /// </summary>
    //*************************************************************************

    public ColorConverter2()
    {
		// (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Method: TryWorkbookToGraph()
    //
    /// <summary>
	/// Attempts to convert an Excel workbook value to a value suitable for use
	/// in a NodeXL graph.
    /// </summary>
    ///
    /// <param name="workbookValue">
    /// Value read from the Excel workbook.
    /// </param>
    ///
    /// <param name="graphValue">
	/// Where a value suitable for use in a NodeXL graph gets stored if true is
	/// returned.
    /// </param>
    ///
    /// <returns>
	/// true if <paramref name="workbookValue" /> contains a valid workbook
	/// value.
    /// </returns>
	///
	/// <remarks>
	/// If <paramref name="workbookValue" /> contains a valid workbook value,
	/// the corresponding graph value gets stored at <paramref
	/// name="graphValue" /> and true is returned.  Otherwise, false is
	/// returned.
	/// </remarks>
    //*************************************************************************

	public Boolean
	TryWorkbookToGraph
	(
		String workbookValue,
		out Color graphValue
	)
	{
		Debug.Assert(workbookValue != null);
        AssertValid();

		graphValue = Color.Empty;

		// Colors can include optional spaces between words: the color
		// LightBlue can be specified as either "Light Blue" or "LightBlue",
		// for example.

		workbookValue = workbookValue.Replace(" ", String.Empty);

        if (workbookValue.Length == 0)
        {
            // ColorConverter converts an empty string to Color.Black.  Bypass
			// ColorConverter.

            return (false);
        }

		try
		{
			graphValue = (Color)( new System.Drawing.ColorConverter() ).
				ConvertFromString(workbookValue);
		}
		catch (Exception)
		{
            // (Format errors raise a System.Exception with an inner exception
			// of type FormatException.  Go figure.)

			return (false);
		}

		return (true);
	}

    //*************************************************************************
    //  Method: GraphToWorkbook()
    //
    /// <summary>
	/// Converts a NodeXL graph value to a value suitable for use in an Excel
	/// workbook.
    /// </summary>
    ///
    /// <param name="graphValue">
    /// Value stored in a NodeXL graph.
    /// </param>
    ///
    /// <returns>
	/// A value suitable for use in an Excel workbook.
    /// </returns>
    //*************************************************************************

    public String
    GraphToWorkbook
    (
		Color graphValue
    )
    {
        AssertValid();

		// Split LightBlue into "Light Blue", for example.

		return ( EnumUtil.SplitName( graphValue.ToKnownColor().ToString(),
			EnumSplitStyle.AllWordsStartUpperCase) );
    }

	//*************************************************************************
	//	Method: PopulateComboBox()
	//
	/// <summary>
	///	Populates a ComboBoxPlus with graph/workbook value pairs.
	/// </summary>
	///
	/// <param name="comboBoxPlus">
	/// The ComboBoxPlus to populate.
	/// </param>
	///
	/// <param name="includeEmptyValue">
	///	If true, a graph/workbook pair of empty strings is included at the top
	/// of the list.
	/// </param>
	///
	/// <remarks>
	/// The ComboBox is populated in such a way that the user sees the workbook
	/// values while the SelectedValue property returns a value of type
	/// KnownColor.
	///
	/// <para>
	/// Important Note:
	/// </para>
	///
	/// <para>
	/// Colors are stored in the NodeXL graph as type Color, but because Excel
	/// doesn't have a color picker that can be inserted in a worksheet cell,
	/// colors are specified in the workbook as type KnownColor.  Unlike Color
	/// values, KnownColor values can be specified in a worksheet via a
	/// drop-down list.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public void
	PopulateComboBox
	(
		ComboBoxPlus comboBoxPlus,
		Boolean includeEmptyValue
	)
	{
		Debug.Assert(comboBoxPlus != null);
		AssertValid();

		comboBoxPlus.PopulateWithObjectsAndText(
            GetAllGraphAndWorkbookValues(includeEmptyValue));
	}

	//*************************************************************************
	//	Method: GetAllGraphAndWorkbookValues()
	//
	/// <summary>
	///	Gets an array of all graph/workbook value pairs.
	/// </summary>
	///
	/// <param name="includeEmptyValue">
	///	If true, a graph/workbook value pair of empty strings is included at
	/// the start of the array.
	/// </param>
	///
	/// <returns>
	/// An array of graph/workbook value pairs, as objects.  The first element
	/// of each pair is a value used in the NodeXL graph, of type KnownColor,
	/// and the second element is a corresponding string suitable for use in a
	/// workbook.
	/// </returns>
	//*************************************************************************

	public Object []
	GetAllGraphAndWorkbookValues
	(
		Boolean includeEmptyValue
	)
	{
		AssertValid();

		List<Object> oGraphAndWorkbookValues = new List<Object>();

		if (includeEmptyValue)
		{
			oGraphAndWorkbookValues.Add(String.Empty);
			oGraphAndWorkbookValues.Add(String.Empty);
		}

		oGraphAndWorkbookValues.AddRange( GetAllGraphAndWorkbookValues() );

		return ( oGraphAndWorkbookValues.ToArray() );
	}

	//*************************************************************************
	//	Method: GetAllGraphAndWorkbookValues()
	//
	/// <summary>
	///	Gets an array of all graph/workbook value pairs.
	/// </summary>
	///
	/// <returns>
	/// An array of graph/workbook value pairs, as objects.  The first element
	/// of each pair is a value used in the NodeXL graph, of type KnownColor,
	/// and the second element is a corresponding string suitable for use in a
	/// workbook.
	/// </returns>
	//*************************************************************************

	protected Object []
	GetAllGraphAndWorkbookValues()
	{
		AssertValid();

		List<Object> oGraphAndWorkbookValues = new List<Object>();

		// Use the known colors.

		foreach ( KnownColor eKnownColor in
			Enum.GetValues( typeof(KnownColor) ) )
		{
			Color oColor = Color.FromKnownColor(eKnownColor);

			if (oColor.IsSystemColor || eKnownColor == KnownColor.Transparent)
			{
				// Skip system colors.

				continue;
			}

			oGraphAndWorkbookValues.Add(eKnownColor);
			oGraphAndWorkbookValues.Add( GraphToWorkbook(oColor) );
		}

		return ( oGraphAndWorkbookValues.ToArray() );
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
		// (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
