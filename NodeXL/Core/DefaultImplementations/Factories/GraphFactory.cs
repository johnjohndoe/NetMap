
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Class: GraphFactory
//
/// <summary>
/// Class that knows how to create <see cref="Graph" /> objects.
/// </summary>
///
/// <remarks>
///	This class implements <see cref="IGraphFactory" />, which allows the core
/// NodeXL system to create graph objects without knowing their type.
/// </remarks>
///
/// <seealso cref="Graph" />
//*****************************************************************************

public class GraphFactory : GraphFactoryBase, IGraphFactory
{
    //*************************************************************************
    //  Constructor: GraphFactory()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphFactory" /> class.
    /// </summary>
    //*************************************************************************

    public GraphFactory()
    {
		// (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Method: CreateGraphCore()
    //
    /// <summary>
    /// Creates a graph object with a specified directedness and restrictions.
    /// </summary>
    ///
    /// <param name="directedness">
	///	Specifies the type of edges that can be added to the graph.
    /// </param>
	///
    /// <param name="restrictions">
	///	Specifies restrictions imposed by the graph.
    /// </param>
	///
    /// <returns>
	///	The <see cref="IGraph" /> interface on a newly created graph object.
    /// </returns>
	///
	/// <remarks>
	/// The arguments have already been checked for validity.
	/// </remarks>
    //*************************************************************************

	protected override IGraph
	CreateGraphCore
	(
		GraphDirectedness directedness,
		GraphRestrictions restrictions
	)
	{
		return ( new Graph(directedness, restrictions) );
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

		// (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
