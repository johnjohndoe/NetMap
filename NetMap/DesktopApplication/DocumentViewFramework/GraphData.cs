
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.DesktopApplication
{
//*****************************************************************************
//  Class: GraphData
//
/// <summary>
/// Contains data for one graph.
/// </summary>
///
/// <remarks>
/// This is a simple wrapper around an <see cref="IGraph" />.  <see
/// cref="IGraph" /> could have been used directly as the unit of data, but
/// having a wrapper will allow additional information to be carried with the
/// graph in the future.
/// </remarks>
//*****************************************************************************

public class GraphData : Object
{
    //*************************************************************************
    //  Constructor: GraphData()
    //
    /// <overloads>
    /// Initializes a new instance of the <see cref="GraphData" /> class.
    /// </overloads>
	///
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphData" /> class with
	/// default graph data.
    /// </summary>
    //*************************************************************************

    public GraphData()
    {
		m_oGraph = new Graph();

		AssertValid();
    }

    //*************************************************************************
    //  Constructor: GraphData()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphData" /> class with
	/// specified graph data.
    /// </summary>
	///
	/// <param name="oGraph">
    /// The <see cref="IGraph" /> containing the graph data.
	/// </param>
    //*************************************************************************

    public GraphData
	(
		IGraph oGraph
	)
    {
		m_oGraph = oGraph;

		AssertValid();
    }

    //*************************************************************************
    //  Property: Graph
    //
    /// <summary>
    /// Gets the <see cref="IGraph" /> containing the graph data.
    /// </summary>
    ///
    /// <value>
    /// The <see cref="IGraph" /> containing the graph data.
    /// </value>
    //*************************************************************************

    public IGraph
    Graph
    {
        get
        {
            AssertValid();

            return (m_oGraph);
        }
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
		Debug.Assert(m_oGraph != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// The graph containing the graph data.

	protected IGraph m_oGraph;
}

}
