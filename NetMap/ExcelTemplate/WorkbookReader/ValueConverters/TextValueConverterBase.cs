
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: TextValueConverterBase
//
/// <summary>
/// Base class for a family of classes that convert text values in the Excel
/// workbook to and from values in the NetMap graph.
/// </summary>
///
/// <remarks>
/// This class is parameterized for the type of the values used in the NetMap
/// graph.
///
/// <para>
/// The derived class must implement the <see cref="GetGraphValueInfos" />
/// method.
/// </para>
///
/// </remarks>
//*****************************************************************************

public abstract class TextValueConverterBase <T> : Object
{
    //*************************************************************************
    //  Constructor: TextValueConverterBase()
    //
    /// <summary>
    /// Initializes a new instance of the TextValueConverterBase class.
    /// </summary>
    //*************************************************************************

    public TextValueConverterBase()
    {
		m_aoGraphValueInfos = null;

		// AssertValid();
    }

    //*************************************************************************
    //  Method: TryWorkbookToGraph()
    //
    /// <summary>
	/// Attempts to convert an Excel workbook value to a value suitable for use
	/// in a NetMap graph.
    /// </summary>
    ///
    /// <param name="workbookValue">
    /// Value read from the Excel workbook.
    /// </param>
    ///
    /// <param name="graphValue">
	/// Where a value suitable for use in a NetMap graph gets stored if true is
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
		out T graphValue
	)
	{
		Debug.Assert(workbookValue != null);
        AssertValid();

        graphValue = default(T);

		String sWorkbookValueLower = workbookValue.ToLower();

		foreach ( GraphValueInfo oGraphValueInfo in
			GetCachedGraphValueInfos() )
		{
			foreach (String sWorkbookValue in oGraphValueInfo.WorkbookValues)
			{
				if ( sWorkbookValueLower == sWorkbookValue.ToLower() )
				{
					graphValue = oGraphValueInfo.GraphValue;
					return (true);
				}
			}
		}

		return (false);
	}

    //*************************************************************************
    //  Method: GraphToWorkbook()
    //
    /// <summary>
	/// Converts a NetMap graph value to a value suitable for use in an Excel
	/// workbook.
    /// </summary>
    ///
    /// <param name="graphValue">
    /// Value stored in a NetMap graph.
    /// </param>
    ///
    /// <returns>
	/// A value suitable for use in an Excel workbook.
    /// </returns>
    //*************************************************************************

    public String
    GraphToWorkbook
    (
		T graphValue
    )
    {
        AssertValid();

		foreach ( GraphValueInfo oGraphValueInfo in
			GetCachedGraphValueInfos() )
		{
			if ( oGraphValueInfo.GraphValue.Equals(graphValue) )
			{
				// Return the prefered workbook value.

				return ( oGraphValueInfo.WorkbookValues[0] );
			}
		}

		Debug.Assert(false);
		return (String.Empty);
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
	/// values while the SelectedValue property returns a graph value of the
	/// parameterized type T.
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
			GetAllGraphAndWorkbookValues(includeEmptyValue) );
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
	/// of each pair is a value used in the NetMap graph, and the second
	/// element is a corresponding string suitable for use in a workbook.
	/// </returns>
	//*************************************************************************

	public Object []
	GetAllGraphAndWorkbookValues
	(
		Boolean includeEmptyValue
	)
	{
		AssertValid();

		List<Object> oAllGraphAndWorkbookValues = new List<Object>();

		if (includeEmptyValue)
		{
			oAllGraphAndWorkbookValues.Add(String.Empty);
			oAllGraphAndWorkbookValues.Add(String.Empty);
		}

		foreach ( GraphValueInfo oGraphValueInfo in
			GetCachedGraphValueInfos() )
		{
			oAllGraphAndWorkbookValues.Add(oGraphValueInfo.GraphValue);

			// Add the prefered workbook value.

			oAllGraphAndWorkbookValues.Add(
				oGraphValueInfo.WorkbookValues[0] );
		}

		return ( oAllGraphAndWorkbookValues.ToArray() );
	}

	//*************************************************************************
	//	Method: GetCachedGraphValueInfos()
	//
	/// <summary>
	///	Gets a cached array of GraphValueInfo objects.
	/// </summary>
	///
	/// <returns>
	/// An array of GraphValueInfo objects, one object for each possible graph
	/// value.
	/// </returns>
	///
	/// <remarks>
	/// If the array hasn't already been cached, it is requested from the
	/// derived class and cached.
	/// </remarks>
	//*************************************************************************

	protected GraphValueInfo []
	GetCachedGraphValueInfos()
	{
		AssertValid();

		// If the array hasn't been cached, request it from the derived class.

		if (m_aoGraphValueInfos == null)
		{
			m_aoGraphValueInfos = GetGraphValueInfos();
		}

		return (m_aoGraphValueInfos);
	}

	//*************************************************************************
	//	Method: GetGraphValueInfos()
	//
	/// <summary>
	///	Gets an array of GraphValueInfo objects, one for each valid graph
	/// value.
	/// </summary>
	///
	/// <returns>
	/// An array of GraphValueInfo objects, one object for each possible graph
	/// value.
	/// </returns>
	//*************************************************************************

	protected abstract GraphValueInfo []
	GetGraphValueInfos();


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
		Debug.Assert(m_aoGraphValueInfos == null ||
			m_aoGraphValueInfos.Length > 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Cached array of GraphValueInfo objects, one object for each possible
	/// graph value, or null if the array hasn't been cached yet.

	protected GraphValueInfo [] m_aoGraphValueInfos;


	//*************************************************************************
	//  Embedded class: GraphValueInfo
	//
	/// <summary>
	/// Contains a value stored in the graph and all of its corresponding
	/// workbook text values.
	/// </summary>
	//*************************************************************************

	public class GraphValueInfo : Object
	{
		//*********************************************************************
		//  Constructor: GraphValueInfo()
		//
		/// <summary>
		/// Initializes a new instance of the <see cref="GraphValueInfo" />
		/// class.
		/// </summary>
		///
		/// <param name="graphValue">
		/// A value stored in the graph.
		/// </param>
		///
		/// <param name="workbookValues">
		/// Array of one or more corresponding workbook text values.  The first
		/// string in the array is considered the "prefered" value.
		/// </param>
		//*********************************************************************

		public GraphValueInfo
		(
			T graphValue,
			String [] workbookValues
		)
		{
			Debug.Assert(workbookValues != null);
			Debug.Assert(workbookValues.Length > 0);

			GraphValue = graphValue;
			WorkbookValues = workbookValues;
		}


		//*********************************************************************
		//  Public fields
		//*********************************************************************

		/// The value stored in the graph.

		public T GraphValue;

		/// Array of one or more corresponding workbook text values.  The first
		/// value in the array is considered the "prefered" value.

		public String [] WorkbookValues;
	}
}

}
