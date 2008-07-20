
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//  Class: SugiyamaVertexDrawer
//
/// <summary>
///	Draws a vertex as a circle, disk, or sphere, in conjunction with <see
/// cref="SugiyamaLayout" /> and <see cref="SugiyamaEdgeDrawer" />.
/// </summary>
///
/// <remarks>
/// This class draws vertices that have been laid out by <see
/// cref="SugiyamaLayout" /> as circles, disks, or spheres centered on each
///	vertex's <see cref="IVertex.Location" />.  It is meant for use with <see
/// cref="SugiyamaEdgeDrawer" />.
///
/// <para>
/// This class works in conjunction with <see
/// cref="MultiSelectionGraphDrawer" /> to draw vertices as either selected or
/// unselected.  If a vertex has been marked as selected by <see
/// cref="MultiSelectionGraphDrawer.SelectVertex(IVertex)" />, it is drawn
/// using <see cref="VertexDrawer.SelectedColor" />.  Otherwise, <see
/// cref="VertexDrawer.Color" /> is used.  Set the <see
/// cref="VertexDrawer.UseSelection" /> property to false to force all vertices
/// to be drawn as unselected.
/// </para>
///
/// <para>
/// If this class is used by a graph drawer other than <see
/// cref="MultiSelectionGraphDrawer" />, all vertices are drawn as unselected.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class SugiyamaVertexDrawer : PerVertexDrawer
{
    //*************************************************************************
    //  Constructor: SugiyamaVertexDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="SugiyamaVertexDrawer" />
	/// class.
    /// </summary>
    //*************************************************************************

    public SugiyamaVertexDrawer()
    {
		// (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Method: GetActualRadius()
    //
    /// <summary>
    /// Gets the actual radius to use for a vertex.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to get the actual radius for.
    /// </param>
    ///
	/// <returns>
	/// The actual radius to use.
	/// </returns>
    ///
    /// <remarks>
	/// If the parent graph of <paramref name="vertex" /> contains a metadata
	/// value for the radius computed by <see cref="SugiyamaLayout" />, the
	/// computed radius is returned.  Otherwise, the base-class method is
	/// called.
    /// </remarks>
    //*************************************************************************

    public override Single
    GetActualRadius
    (
        IVertex vertex
    )
	{
		AssertValid();

		Object oValue;

		if ( vertex.ParentGraph.TryGetValue(
			ReservedMetadataKeys.SugiyamaComputedRadius, typeof(Single),
			out oValue) )
		{
			Single fActualRadius = (Single)oValue;

			// Impose a minimum in case SugiyamaLayout computed a radius so
			// small it can't be seen.

			fActualRadius = Math.Max(fActualRadius, MinimumSugiyamaRadius);

			return (fActualRadius);
		}


		return ( base.GetActualRadius(vertex) );
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
    //  Protected constants
    //*************************************************************************

	/// Minimum radius to use.

	protected const Single MinimumSugiyamaRadius = 3.0F;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
