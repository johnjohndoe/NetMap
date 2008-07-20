
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NetMap.Core
{
//*****************************************************************************
//  Class: ByDelegateVertexSorter
//
/// <summary>
/// Sorts a collection of vertices using a vertex-comparison delegate.
/// </summary>
///
/// <remarks>
/// Use this class when you want to sort a collection of vertices using your
/// own vertex comparison method.  Set the <see cref="VertexComparer" />
/// property to a delegate that compares two vertices, then call one of the
/// <see cref="IVertexSorter.Sort(IVertexCollection)" /> methods.
///
/// <para>
/// If you want to sort on metadata values, use <see
/// cref="ByMetadataVertexSorter{TValue}" /> instead.  <see
/// cref="ByMetadataVertexSorter{TValue}" /> is optimized for this task.
/// </para>
///
/// </remarks>
///
///	<example>
///	The following code sorts a graph's vertices using a delegate that sorts by
/// vertex name.  The code assumes that every vertex's <see
/// cref="IIdentityProvider.Name" /> has been set to a non-null value.
///
/// <code>
/// {
/// ...
///
/// ByDelegateVertexSorter oByDelegateVertexSorter =
///     new ByDelegateVertexSorter()
///
/// oByDelegateVertexSorter.VertexComparer = CompareVerticesByName;
///
/// IVertex [] aoSortedVertices = oByDelegateVertexSorter.Sort(oGraph.Vertices);
///
/// ...
/// }
///
/// public Int32
/// CompareVerticesByName
/// (
///     IVertex vertex1,
///     IVertex vertex2
/// )
/// {
///     return ( vertex1.Name.CompareTo(vertex2.Name) );
/// }
/// </code>
///
///	</example>
//*****************************************************************************

public class ByDelegateVertexSorter : VertexSorterBase
{
    //*************************************************************************
    //  Constructor: ByDelegateVertexSorter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ByDelegateVertexSorter" />
	/// class.
    /// </summary>
    //*************************************************************************

    public ByDelegateVertexSorter()
    {
		m_oVertexComparer = this.CompareVerticesByID;

		AssertValid();
    }

    //*************************************************************************
    //  Property: VertexComparer
    //
    /// <summary>
    /// Gets or sets the delegate used to compare two vertices.
    /// </summary>
    ///
    /// <value>
	///	A delegate that compares two vertices.  The default value is <see
	/// cref="CompareVerticesByID" />, a delegate that sorts vertices by ID.
    /// </value>
    //*************************************************************************

    public Comparison<IVertex>
    VertexComparer
    {
        get
		{
			AssertValid();

			return (m_oVertexComparer);
		}

		set
		{
			const String PropertyName = "VertexComparer";

            this.ArgumentChecker.CheckPropertyNotNull(PropertyName, value);

			m_oVertexComparer = value;

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
		AssertValid();

		Array.Sort<IVertex>(vertices, m_oVertexComparer);

		return (vertices);
    }

    //*************************************************************************
    //  Method: CompareVerticesByID()
    //
    /// <summary>
    /// Compares two vertices by <see cref="IIdentityProvider.ID" />.
    /// </summary>
    ///
    /// <param name="vertex1">
    /// First vertex to compare.
    /// </param>
	///
    /// <param name="vertex2">
    /// Second vertex to compare.
    /// </param>
	///
    /// <returns>
	/// See <see cref="System.Collections.IComparer.Compare" />.
    /// </returns>
	///
	/// <remarks>
	/// This method is used as the default delegate for the <see
	/// cref="VertexComparer" /> property.  If you use the default delegate and
	/// call <see cref="IVertexSorter.Sort(IVertexCollection)" />, the returned
	/// array will be sorted in order of ascending vertex IDs.
	/// </remarks>
    //*************************************************************************

	public Int32
	CompareVerticesByID
	(
		IVertex vertex1,
		IVertex vertex2
	)
	{
		AssertValid();

		const String MethodName = "CompareVerticesByID";

		ArgumentChecker oArgumentChecker = this.ArgumentChecker;

        oArgumentChecker.CheckArgumentNotNull(
            MethodName, "vertex1", vertex1);

        oArgumentChecker.CheckArgumentNotNull(
            MethodName, "vertex2", vertex2);

		return ( vertex1.ID.CompareTo(vertex2.ID) );
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

		Debug.Assert(m_oVertexComparer != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Delegate used to compare two vertices.

	protected Comparison<IVertex> m_oVertexComparer;
}

}
