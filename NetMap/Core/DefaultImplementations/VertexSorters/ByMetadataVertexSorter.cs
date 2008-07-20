
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NetMap.Core
{
//*****************************************************************************
//  Class: ByMetadataVertexSorter
//
/// <summary>
/// Sorts a collection of vertices on a specified metadata value.
/// </summary>
///
/// <typeparam name="TValue">
/// The type of the values that will be sorted on.
/// </typeparam>
///
/// <remarks>
/// Use this class when you want to sort a collection of vertices on a metadata
/// value.  Set the metadata value on each vertex in the collection, then call
/// one of the <see cref="IVertexSorter.Sort(IVertexCollection)" /> methods.
///
/// <para>
/// The <see cref="IVertexSorter.Sort(IVertexCollection)" /> methods optimize
/// sort performance by caching the metadata values instead of reading them
/// every time two vertices are compared.
/// </para>
///
/// </remarks>
///
///	<example>
///	The following code sorts a graph's vertices in ascending order of a
/// metadata value named Weight, which is of type Int32.  The code assumes that
/// a Weight metadata value has been set on every vertex.
///
/// <code>
/// ByMetadataVertexSorter oByMetadataVertexSorter =
///     new ByMetadataVertexSorter&lt;Int32&gt;("Weight")
///
/// IVertex [] aoSortedVertices = oByMetadataVertexSorter.Sort(oGraph.Vertices);
/// </code>
///
///	</example>
//*****************************************************************************

public class ByMetadataVertexSorter<TValue> : VertexSorterBase

	where TValue : System.IComparable<TValue>
{
    //*************************************************************************
    //  Constructor: ByMetadataVertexSorter()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ByMetadataVertexSorter{TValue}" /> class.
    /// </summary>
	///
    /// <param name="sortKey">
	/// The metadata key whose values should be sorted on.  Can't be null or
	/// empty.  The key must exist on each vertex and the corresponding value
	/// must be of type TValue.
    /// </param>
    //*************************************************************************

    public ByMetadataVertexSorter
	(
		String sortKey
	)
    {
		const String MethodName = "Constructor";

		this.ArgumentChecker.CheckArgumentNotEmpty(
			MethodName, "sortKey", sortKey);

		m_sSortKey = sortKey;
		m_bSortAscending = true;

		AssertValid();
    }

    //*************************************************************************
    //  Property: SortKey
    //
    /// <summary>
    /// Gets or sets the key whose values should be sorted on.
    /// </summary>
    ///
    /// <value>
	/// The metadata key whose values should be sorted on.  Can't be null or
	/// empty.
    /// </value>
	///
	/// <remarks>
	/// The key must exist on each vertex and the corresponding value must be
	/// of type TValue.
	/// </remarks>
    //*************************************************************************

    public String
    SortKey
    {
        get
		{
			AssertValid();

			return (m_sSortKey);
		}

		set
		{
			const String PropertyName = "SortKey";

            this.ArgumentChecker.CheckPropertyNotEmpty(PropertyName, value);

			m_sSortKey = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: SortAscending
    //
    /// <summary>
    /// Gets or sets a flag specifying whether the sort is ascending or
	/// descending.
    /// </summary>
    ///
    /// <value>
	/// true to sort the collection of vertices in ascending order of the
	/// values specified by <see cref="SortKey" />, false to sort the
	/// collection in descending order.  The default value is true.
    /// </value>
    //*************************************************************************

    public Boolean
    SortAscending
    {
        get
		{
			AssertValid();

			return (m_bSortAscending);
		}

		set
		{
			m_bSortAscending = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Method: SortCore()
    //
    /// <summary>
    /// Sorts an array of vertices in place.
    /// </summary>
    ///
    /// <param name="vertices">
    /// Array to sort.  The array is sorted in place.
    /// </param>
    ///
    /// <returns>
	/// Sorted <paramref name="vertices" />.
    /// </returns>
	///
    /// <remarks>
	/// This method sorts <paramref name="vertices" /> in place and returns the
	/// sorted vertices.
	///
	/// <para>
	/// The arguments have already been checked for validity.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    protected override IVertex [] 
    SortCore
    (
		IVertex [] vertices
    )
    {
		Debug.Assert(vertices != null);

		const String MethodName = "Sort";

		// Create an array that contains the specified metadata value from each
		// vertex.  The two arrays will get sorted in parallel.
		//
		// This two-array technique allows the metadata values to be retrieved
		// just once per object, instead of every time two array elements are
		// compared.

		Int32 iCount = vertices.Length;

		TValue [] aoValues = new TValue[iCount];

		for (Int32 i = 0; i < iCount; i++)
		{
			IVertex oVertex = vertices[i];

			Object oValue = null;

			try
			{
				oValue = oVertex.GetRequiredValue(
					m_sSortKey, typeof(TValue) );
			}
			catch (ArgumentException oArgumentException)
			{
				// Wrap the "missing value" exception in a more information
				// exception.

				ArgumentChecker.ThrowArgumentException(
					MethodName, oArgumentException.ParamName, 

					"One of the vertices does not have the specified sort key,"
					+ " or the key's value is the wrong type."
					,
					oArgumentException
					);
			}

			aoValues[i] = (TValue)oValue;
		}

		// Sort the two arrays using a ValueComparer that sorts in a specified
		// direction.

		Array.Sort<TValue, IVertex>(
			aoValues, vertices, new ValueComparer(m_bSortAscending) );

        return (vertices);
    }


    //*************************************************************************
    //  Method: AssertValid()
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

		Debug.Assert( !String.IsNullOrEmpty(m_sSortKey) );
		// m_bSortAscending
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Key whose values should be sorted on.

	protected String m_sSortKey;

	/// true to sort in ascending order, false to sort in descending order.

	protected Boolean m_bSortAscending;


	//*************************************************************************
	//  Nested class: ValueComparer
	//
	/// <summary>
	/// Compares two metadata values.
	/// </summary>
	///
	/// <remarks>
	/// This is nested within the MetadataSorter class, so its type is
	/// VertexSorter.Comparer.
	/// </remarks>
	//*************************************************************************

	private class ValueComparer : Comparer<TValue>
	{
		//*********************************************************************
		//  Constructor: ValueComparer()
		//
		/// <summary>
		/// Initializes a new instance of the <see cref="ValueComparer" />
		/// class.
		/// </summary>
		///
		/// <param name="sortAscending">
		/// true to sort in ascending order of metadata values, false to sort
		/// in descending order.
		/// </param>
		//*********************************************************************

		public ValueComparer
		(
			Boolean sortAscending
		)
		{
			m_bSortAscending = sortAscending;

			AssertValid();
		}

		//*********************************************************************
		//  Method: Compare()
		//
		/// <summary>
		/// Compares two values.
		/// </summary>
		///
		/// <param name="value1">
		/// First value to compare.
		/// </param>
		///
		/// <param name="value2">
		/// Second value to compare.
		/// </param>
		///
		/// <returns>
		/// See <see cref="Comparer{T}.Compare" />.
		/// </returns>
		//*********************************************************************

		public override Int32
		Compare
		(
			TValue value1,
			TValue value2
		)
		{
			AssertValid();

			if (m_bSortAscending)
			{
				return ( value1.CompareTo(value2) );
			}

			return ( value2.CompareTo(value1) );
		}


		//*********************************************************************
		//  Method: AssertValid()
		//
		/// <summary>
		/// Asserts if the object is in an invalid state.  Debug-only.
		/// </summary>
		//*********************************************************************

		[Conditional("DEBUG")]

		public void
		AssertValid()
		{
			// m_bSortAscending
		}


		//*********************************************************************
		//  Protected fields
		//*********************************************************************

		/// true to sort in ascending order, false to sort in descending order.

		protected Boolean m_bSortAscending;
	}
}

}
