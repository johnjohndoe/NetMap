
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//  Class: EadesSpringEmbedderLayout
//
/// <summary>
///	Lays out a graph using the Eades spring embedder layout.
/// </summary>
///
/// <remarks>
///	For details on the layout algorithm, see P. Eades, "A heuristic for graph
///	drawing", Congressus Numerantium, 42:149{ 160, 1984.
///
/// <para>
/// Important Note: As of August 2008, this class is not implemented.  <see
/// cref="LayOutGraphCore" /> throws a NotImplementedException.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class EadesSpringEmbedderLayout : AsyncLayoutBase
{
    //*************************************************************************
    //  Constructor: EadesSpringEmbedderLayout()
    //
    /// <summary>
    /// Initializes a new instance of the EadesSpringEmbedderLayout class.
    /// </summary>
    //*************************************************************************

    public EadesSpringEmbedderLayout()
    {
        // TODO
    }

    //*************************************************************************
    //  Method: LayOutGraphCore()
    //
    /// <summary>
    /// Lays out a graph synchronously or asynchronously.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to lay out.  The graph is guaranteed to have at least one vertex.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.  The <see
	/// cref="LayoutContext.GraphRectangle" /> is guaranteed to have non-zero
	/// width and height.
    /// </param>
    ///
    /// <param name="backgroundWorker">
    /// <see cref="BackgroundWorker" /> whose worker thread called this method
	/// if the graph is being laid out asynchronously, or null if the graph is
	/// being laid out synchronously.
    /// </param>
    ///
    /// <returns>
	/// true if the layout was successfully completed, false if the layout was
	/// cancelled.  The layout can be cancelled only if the graph is being laid
	/// out asynchronously.
    /// </returns>
	///
    /// <remarks>
	///	This method lays out the graph <paramref name="graph" /> either
	/// synchronously (if <paramref name="backgroundWorker" /> is null) or
	/// asynchronously (if (<paramref name="backgroundWorker" /> is not null)
	/// by setting the the <see cref="IVertex.Location" /> property on all of
	/// the graph's vertices and optionally adding geometry metadata to the
	/// graph, vertices, or edges.
	///
	/// <para>
	/// In the asynchronous case, the <see
	/// cref="BackgroundWorker.CancellationPending" /> property on the
	/// <paramref name="backgroundWorker" /> object should be checked before
	/// each layout iteration.  If it's true, the method should immediately
	/// return false.  Also, <see
	/// cref="AsyncLayoutBase.FireLayOutGraphIterationCompleted()" /> should be
	/// called after each iteration.
	/// </para>
	///
	/// <para>
	/// The arguments have already been checked for validity.
	/// </para>
	///
	/// <para>
	/// Important Note: As of August 2008, this method is not implemented.  It
	/// throws a NotImplementedException.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    protected override Boolean
    LayOutGraphCore
    (
		IGraph graph,
		LayoutContext layoutContext,
		BackgroundWorker backgroundWorker
	)
	{
		Debug.Assert(graph != null);
		Debug.Assert(layoutContext != null);
		AssertValid();

		// TODO

		throw new NotImplementedException();
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

        // TODO
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // TODO
}

}
