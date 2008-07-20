
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//  Class: PerEdgeDrawer
//
/// <summary>
///	Draws an edge as a simple line.  Allows per-edge customizations.
/// </summary>
///
/// <remarks>
/// By default, this class delegates edge drawing to its base class.  If you
/// add appropriate metadata to an edge, however, this class uses the metadata
/// to customize the appearance of that edge.
///
/// <para>
/// A selected edge is always drawn using <see
/// cref="EdgeDrawer.SelectedColor" />.  If an unselected edge has the <see
/// cref="ReservedMetadataKeys.PerColor" /> key, the edge is drawn using
/// the <see cref="Color" /> specified by the key's value.  If an unselected
/// edge does not have this key, it is drawn using <see
/// cref="EdgeDrawer.Color" />.
/// </para>
///
/// <para>
/// If an edge has the <see cref="ReservedMetadataKeys.PerAlpha" /> key, the
/// edge's opacity is set to the <see cref="Int32" /> opacity specified by the
/// key's value.  The opacity varies from 0 (transparent) to 255 (opaque).
/// </para>
///
/// <para>
/// A selected edge is always drawn using <see
/// cref="EdgeDrawer.SelectedWidth" />.  If an unselected edge has the <see
/// cref="ReservedMetadataKeys.PerEdgeWidth" /> key, the edge is drawn using
/// the <see cref="Int32" /> width specified by the key's value.  If an
/// unselected edge does not have this key, it is drawn using <see
/// cref="EdgeDrawer.Width" />.
/// </para>
///
/// </remarks>
///
/// <seealso cref="EdgeDrawer" />
//*****************************************************************************

public class PerEdgeDrawer : EdgeDrawer
{
    //*************************************************************************
    //  Constructor: PerEdgeDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="PerEdgeDrawer" /> class.
    /// </summary>
    //*************************************************************************

    public PerEdgeDrawer()
    {
		// (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Method: GetActualColor()
    //
    /// <summary>
    /// Gets the actual color to use for an edge.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to get the actual color for.
    /// </param>
    ///
    /// <param name="showAsSelected">
    /// true if <paramref name="edge" /> should be shown as selected.
    /// </param>
    ///
	/// <returns>
	/// The actual color to use.
	/// </returns>
    ///
    /// <remarks>
	/// See the class comments for details on how the actual color is
	/// determined.
    /// </remarks>
    //*************************************************************************

    protected override Color
    GetActualColor
    (
        IEdge edge,
		Boolean showAsSelected
    )
	{
		AssertValid();

		// Start with the color determined by the base class.

		Color oPerEdgeColor = base.GetActualColor(edge, showAsSelected);

		// The selected color is always determined by the base class.

		if (showAsSelected)
		{
            return (oPerEdgeColor);
		}

		// Check for a per-edge color.

		Object oPerEdgeColorAsObject;

		if ( edge.TryGetValue(ReservedMetadataKeys.PerColor,
			typeof(Color), out oPerEdgeColorAsObject) )
		{
			// Use the per-edge color instead of the base-class color.

			oPerEdgeColor = (Color)oPerEdgeColorAsObject;
		}

		// Check for a per-edge alpha.

		Object oPerEdgeAlphaAsObject;

		if ( edge.TryGetValue(ReservedMetadataKeys.PerAlpha, typeof(Int32),
			out oPerEdgeAlphaAsObject) )
		{
			Int32 iPerEdgeAlpha = (Int32)oPerEdgeAlphaAsObject;

			if (iPerEdgeAlpha < 0 || iPerEdgeAlpha > 255)
			{
				throw new FormatException( String.Format(

					"{0}: The edge with the ID {1} has an out-of-range"
					+ " {2} value.  Valid values are between 0 and 255."
					,
					this.ClassName,
					edge.ID,
					ReservedMetadataKeys.PerAlpha
					) );
			}

			// Use the per-edge alpha.

			oPerEdgeColor = Color.FromArgb(iPerEdgeAlpha, oPerEdgeColor);
		}

		return (oPerEdgeColor);
	}

    //*************************************************************************
    //  Method: GetActualWidth()
    //
    /// <summary>
    /// Gets the actual width to use for an edge.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to get the actual width for.
    /// </param>
    ///
    /// <param name="showAsSelected">
    /// true if <paramref name="edge" /> should be shown as selected.
    /// </param>
    ///
	/// <returns>
	/// The actual width to use.
	/// </returns>
    ///
    /// <remarks>
	/// See the class comments for details on how the actual color is
	/// determined.
    /// </remarks>
    //*************************************************************************

    protected override Int32
    GetActualWidth
    (
        IEdge edge,
		Boolean showAsSelected
    )
	{
		AssertValid();

		Object oPerEdgeWidth;

		if ( !edge.TryGetValue(ReservedMetadataKeys.PerEdgeWidth, typeof(Int32),
				out oPerEdgeWidth) )
		{
			return ( base.GetActualWidth(edge, showAsSelected) );
		}

		Int32 iPerEdgeWidth = (Int32)oPerEdgeWidth;

		if (iPerEdgeWidth < MinimumWidth || iPerEdgeWidth > MaximumWidth)
		{
			throw new FormatException( String.Format(

				"{0}: The edge with the ID {1} has an out-of-range"
				+ " {2} value.  Valid values are between {3} and {4}."
				,
				this.ClassName,
				edge.ID,
				ReservedMetadataKeys.PerEdgeWidth,
				MinimumWidth,
				MaximumWidth
				) );
		}

		if (showAsSelected)
		{
			// (See the comments in EdgeDrawer.GetActualWidth().)

			return ( Math.Max(this.SelectedWidth, iPerEdgeWidth + 1) );
		}

		return (iPerEdgeWidth);
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
