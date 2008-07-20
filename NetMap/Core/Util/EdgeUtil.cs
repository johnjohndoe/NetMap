
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NetMap.Core
{
//*****************************************************************************
//  Class: EdgeUtil
//
/// <summary>
/// Utility methods for dealing with <see cref="IEdge" /> objects.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class EdgeUtil
{
    //*************************************************************************
    //  Method: EdgeToVertices()
    //
    /// <summary>
    /// Obtains an edge's two vertices.
    /// </summary>
    ///
    /// <param name="edge">
	///	The edge connecting the two vertices.  Can't be null.
    /// </param>
    ///
    /// <param name="className">
	/// Name of the class calling this method.
    /// </param>
	///
    /// <param name="methodOrPropertyName">
	/// Name of the method or property calling this method.
    /// </param>
	///
    /// <param name="vertex1">
	/// Where the edge's first vertex gets stored.
    /// </param>
	///
    /// <param name="vertex2">
	/// Where the edge's second vertex gets stored.
    /// </param>
	///
    /// <remarks>
	/// This method obtains an edge's two vertices and stores them at
	/// <paramref name="vertex1" /> and <paramref name="vertex2" />.  An
	/// <see cref="ApplicationException" /> is thrown if the vertices can't be
	/// obtained.
    /// </remarks>
    //*************************************************************************

	public static void
	EdgeToVertices
	(
		IEdge edge,
		String className,
		String methodOrPropertyName,
		out IVertex vertex1,
		out IVertex vertex2
	)
	{
		Debug.Assert(edge != null);
		Debug.Assert( !String.IsNullOrEmpty(className) );
		Debug.Assert( !String.IsNullOrEmpty(methodOrPropertyName) );

		String sErrorMessage = null;

		IVertex [] aoVertices = edge.Vertices;

		if (aoVertices == null)
		{
			sErrorMessage = "The edge's Vertices property is null.";
		}
		else if (aoVertices.Length != 2)
		{
			sErrorMessage = "The edge does not connect two vertices.";
		}
		else if (aoVertices[0] == null)
		{
			sErrorMessage = "The edge's first vertex is null.";
		}
		else if (aoVertices[1] == null)
		{
			sErrorMessage = "The edge's second vertex is null.";
		}
		else if (aoVertices[0].ParentGraph == null)
		{
			sErrorMessage =
				"The edge's first vertex does not belong to a graph.";
		}
		else if (aoVertices[1].ParentGraph == null)
		{
			sErrorMessage =
				"The edge's second vertex does not belong to a graph.";
		}
		else if ( aoVertices[0].ParentGraph != aoVertices[1].ParentGraph )
		{
			sErrorMessage =
				"The edge connects vertices not in the same graph.";
		}

		if (sErrorMessage != null)
		{
			Debug.Assert(false);

			throw new ApplicationException( String.Format(

				"{0}.{1}: {2}"
				,
				className,
				methodOrPropertyName,
				sErrorMessage
				) );
		}

		vertex1 = aoVertices[0];
		vertex2 = aoVertices[1];
	}
}

}
