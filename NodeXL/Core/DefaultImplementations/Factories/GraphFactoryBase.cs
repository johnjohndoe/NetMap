
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Class: GraphFactoryBase
//
/// <summary>
///	Base class for graph factories.
/// </summary>
///
/// <remarks>
/// This abstract class can serve as a base class for <see
/// cref="IGraphFactory" /> implementations.  Its implementations of the <see
/// cref="IGraphFactory" /> public methods provide error checking but defer
/// the actual work to protected abstract methods.
/// </remarks>
///
/// <seealso cref="Graph" />
//*****************************************************************************

public abstract class GraphFactoryBase :
	GraphVertexEdgeFactoryBase, IGraphFactory
{
    //*************************************************************************
    //  Constructor: GraphFactoryBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphFactoryBase" />
	/// class.
    /// </summary>
    //*************************************************************************

    public GraphFactoryBase()
    {
		// (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Method: CreateGraph()
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
    //*************************************************************************

	public IGraph
	CreateGraph
	(
		GraphDirectedness directedness,
		GraphRestrictions restrictions
	)
	{
		AssertValid();

        const String MethodName = "CreateGraph";

		ArgumentChecker oArgumentChecker = this.ArgumentChecker;

		oArgumentChecker.CheckArgumentIsDefined(
			MethodName, "directedness", directedness,
            typeof(GraphDirectedness) );

		if ( (restrictions & ~GraphRestrictions.All) != 0 )
		{
            oArgumentChecker.ThrowArgumentException(
				MethodName, "restrictions",
				
				"Must be an ORed combination of the flags in the"
				+ " GraphRestrictions enumeration."
				);
		}

		return ( CreateGraphCore(directedness, restrictions) );
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

	protected abstract IGraph
	CreateGraphCore
	(
		GraphDirectedness directedness,
		GraphRestrictions restrictions
	);


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
