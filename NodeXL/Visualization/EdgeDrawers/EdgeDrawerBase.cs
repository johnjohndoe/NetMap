
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//  Class: EdgeDrawerBase
//
/// <summary>
///	Base class for edge drawers.
/// </summary>
///
/// <remarks>
/// This abstract class can serve as a base class for <see
/// cref="IEdgeDrawer" /> implementations.  Its implementations of the <see
/// cref="IEdgeDrawer" /> public methods provide error checking but defer the
/// actual work to protected abstract methods.
///
/// <para>
/// The visibility of the edge can be controlled with the <see
/// cref="ReservedMetadataKeys.Visibility" /> key.
/// </para>
///
/// </remarks>
//*****************************************************************************

public abstract class EdgeDrawerBase : DrawerBase, IEdgeDrawer
{
    //*************************************************************************
    //  Constructor: EdgeDrawerBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeDrawerBase" /> class.
    /// </summary>
    //*************************************************************************

    public EdgeDrawerBase()
    {
		// (Do nothing.)
    }

    //*************************************************************************
    //  Method: DrawEdge()
    //
    /// <summary>
    /// Draws an edge.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to draw.
    /// </param>
	///
    /// <param name="drawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
    ///
    /// <remarks>
    /// This method gets called repeatedly while a graph is being drawn, once
	/// for each of the graph's edges.  The <see cref="IVertex.Location" />
	///	property on all of the graph's vertices is set by ILayout.<see
	///	cref="ILayout.LayOutGraph" /> before this method is called.
    /// </remarks>
    //*************************************************************************

    public void
    DrawEdge
    (
        IEdge edge,
		DrawContext drawContext
    )
	{
		AssertValid();

		const String MethodName = "DrawEdge";

		const String EdgeArgumentName = "edge";

		ArgumentChecker oArgumentChecker = this.ArgumentChecker;

		oArgumentChecker.CheckArgumentNotNull(
			MethodName, EdgeArgumentName, edge);

		oArgumentChecker.CheckArgumentNotNull(MethodName, "drawContext",
			drawContext);

		Graphics oGraphics = drawContext.Graphics;

		oArgumentChecker.CheckArgumentNotNull(MethodName,
			"drawContext.Graphics", oGraphics);

		// Get the edge's vertices.

		IVertex oVertex1, oVertex2;

		EdgeUtil.EdgeToVertices(edge, this.ClassName, MethodName,
			out oVertex1, out oVertex2);

		if (edge.ParentGraph == null)
		{
            oArgumentChecker.ThrowArgumentException(
				MethodName, EdgeArgumentName,
				"The edge doesn't belong to a graph.  It can't be drawn."
				);
		}

		// If the edge isn't hidden, draw it.

		if (GetVisibility(edge) != VisibilityKeyValue.Hidden)
		{
			DrawEdgeCore(edge, oVertex1, oVertex2, drawContext);
		}
	}

    //*************************************************************************
    //  Method: DrawEdgeCore()
    //
    /// <summary>
    /// Draws an edge.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to draw.
    /// </param>
	///
    /// <param name="vertex1">
    /// The edge's first vertex.
    /// </param>
	///
    /// <param name="vertex2">
    /// The edge's second vertex.
    /// </param>
	///
    /// <param name="drawContext">
	/// Provides access to objects needed for drawing operations.
    /// </param>
    ///
    /// <remarks>
    /// This method gets called repeatedly while a graph is being drawn, once
	/// for each of the graph's edges.  The <see cref="IVertex.Location" />
	///	property on all of the graph's vertices is set by ILayout.<see
	///	cref="ILayout.LayOutGraph" /> before this method is called.
	///
	/// <para>
	/// The arguments have already been checked for validity.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    protected abstract void
    DrawEdgeCore
    (
        IEdge edge,
		IVertex vertex1,
		IVertex vertex2,
		DrawContext drawContext
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
