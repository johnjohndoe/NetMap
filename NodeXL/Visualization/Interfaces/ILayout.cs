
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//  Interface: ILayout
//
/// <summary>
/// Supports laying out a graph within a rectangle.
/// </summary>
///
/// <remarks>
/// A class that implements this interface is responsible for laying out a
/// graph within a specified rectangle by setting the <see
///	cref="IVertex.Location" /> property on all of the graph's vertices, and
/// optionally adding geometry metadata to the graph, vertices, or edges.
/// Laying out a graph is the first step in drawing it.
///
/// <para>
/// If the layout is slow, you should consider implementing the <see
/// cref="IAsyncLayout" /> interface instead.
/// </para>
///
/// </remarks>
//*****************************************************************************

public interface ILayout
{
    //*************************************************************************
    //  Property: Margin
    //
    /// <summary>
	///	Gets or sets the margin to subtract from each edge of the graph
	/// rectangle before laying out the graph.
    /// </summary>
    ///
    /// <value>
	/// The margin to subtract from each edge.  Must be greater than or equal
	/// to zero.  The units are determined by the <see cref="Graphics" />
	/// object used to draw the graph.  The default value is 0.
    /// </value>
	///
	/// <remarks>
	/// If the graph rectangle passed to <see cref="LayOutGraph" /> is {L=0,
	/// T=0, R=50, B=30} and the <see cref="Margin" /> is 5, for example, then
	/// the graph is laid out within the rectangle {L=5, T=5, R=45, B=25}.
	/// </remarks>
    //*************************************************************************

    Int32
    Margin
    {
        get;
		set;
    }

    //*************************************************************************
    //  Method: LayOutGraph()
    //
    /// <summary>
    /// Lays out a graph.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to lay out.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.
    /// </param>
    ///
    /// <remarks>
	///	This method lays out the graph <paramref name="graph" /> by setting the
	/// <see cref="IVertex.Location" /> property on all of the graph's
	/// vertices, and optionally adding geometry metadata to the graph,
	/// vertices, or edges.
    /// </remarks>
    //*************************************************************************

    void
    LayOutGraph
    (
		IGraph graph,
		LayoutContext layoutContext
    );

    //*************************************************************************
    //  Method: TransformLayout()
    //
    /// <summary>
	/// Transforms a graph's current layout.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph whose layout needs to be transformed.
    /// </param>
    ///
    /// <param name="originalLayoutContext">
	/// <see cref="LayoutContext" /> object that was passed to the most recent
	/// call to <see cref="LayOutGraph" />.
    /// </param>
    ///
    /// <param name="newLayoutContext">
    /// Provides access to the new graph rectangle.
    /// </param>
    ///
    /// <remarks>
	/// After a graph has been laid out by <see cref="LayOutGraph" />, this
	/// method can be used to transform the graph's layout from one rectangle
	/// to another.  <paramref name="originalLayoutContext" /> contains the
	/// original graph rectangle, and <paramref name="newLayoutContext" />
	/// contains the new graph rectangle.  The implementation should transform
	/// all the graph's vertex locations from the original rectangle to the new
	/// one.  If <see cref="LayOutGraph" /> added geometry metadata to the
	/// graph, the implementation should also transform that metadata.
    /// </remarks>
    //*************************************************************************

    void
    TransformLayout
    (
		IGraph graph,
		LayoutContext originalLayoutContext,
		LayoutContext newLayoutContext
    );

    //*************************************************************************
    //  Method: OnVertexMove()
    //
    /// <summary>
    /// Processes a vertex that was moved after the graph was laid out.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex that was moved.
    /// </param>
    ///
    /// <remarks>
	/// An application may allow the user to move a vertex after the graph has
	/// been laid out by <see cref="LayOutGraph" />.  This method is called
	/// after the application has changed the <see cref="IVertex.Location" />
	/// property on <paramref name="vertex" />.  If <see cref="LayOutGraph" />
	/// added geometry metadata to the graph, vertices, or edges, <see
	/// cref="OnVertexMove" /> should modify the metadata if necessary.
    /// </remarks>
    //*************************************************************************

    void
    OnVertexMove
    (
		IVertex vertex
    );

	//*************************************************************************
	//	Event: LayoutRequired
	//
	/// <summary>
	///	Occurs when a change occurs that requires the graph to be laid out
	/// again.
	/// </summary>
	///
	/// <remarks>
	///	The implementation must fire this event when any change is made to the
	/// object that might affect the layout of the graph, such as a property
	/// change that affects the layout algorithm.
	///
	/// <para>
	/// The owner should lay out the graph and redraw it in response to the
	/// event.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	event EventHandler LayoutRequired;
}

}
