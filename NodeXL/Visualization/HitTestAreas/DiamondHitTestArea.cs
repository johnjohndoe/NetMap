
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;

namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//  Class: DiamondHitTestArea
//
/// <summary>
/// Provides hit-testing for a diamond area.
/// </summary>
///
/// <remarks>
/// This is one of a set of classes that are used for hit-testing.  The classes
/// generally provide a lightweight alternative to using System.Drawing.Region
/// objects for hit-testing, except in this case a Region is used anyway.
/// </remarks>
//*****************************************************************************

public class DiamondHitTestArea : HitTestArea
{
    //*************************************************************************
    //  Constructor: DiamondHitTestArea()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="DiamondHitTestArea" />
	/// class.
    /// </summary>
	///
    /// <param name="center">
    /// The diamond's center.
    /// </param>
	///
	/// <param name="halfWidth">
	/// One half the width of the diamond.
	/// </param>
    //*************************************************************************

    public DiamondHitTestArea
	(
		PointF center,
		Single halfWidth
	)
    {
		Debug.Assert(halfWidth >= 0);

		m_oCenter = center;
		m_fHalfWidth = halfWidth;

		AssertValid();
    }

    //*************************************************************************
    //  Method: Contains()
    //
    /// <summary>
    /// Determines whether the hit-test area contains a specified point.
    /// </summary>
    ///
    /// <param name="point">
    /// Point to test.
    /// </param>
    ///
    /// <returns>
    /// true if this hit-test area contains <paramref name="point" />.
    /// </returns>
    //*************************************************************************

    public override Boolean
    Contains
    (
		PointF point
    )
	{
		AssertValid();

		Region oRegion = GetRegion();

		Boolean bContains = oRegion.IsVisible(point);
		oRegion.Dispose();

		return (bContains);
	}

    //*************************************************************************
    //  Method: IntersectsWith()
    //
    /// <summary>
    /// Determines whether the hit-test area intersects a specified rectangle.
    /// </summary>
    ///
    /// <param name="rectangle">
    /// Rectangle to test.
    /// </param>
    ///
    /// <returns>
    /// true if this hit-test area intersects <paramref name="rectangle" />.
    /// </returns>
    //*************************************************************************

    public override Boolean
    IntersectsWith
    (
		Rectangle rectangle
    )
	{
		AssertValid();

		Region oRegion = GetRegion();

		Boolean bIntersectsWith = oRegion.IsVisible(rectangle);
		oRegion.Dispose();

		return (bIntersectsWith);
	}

    //*************************************************************************
    //  Method: GetRegion()
    //
    /// <summary>
    /// Gets a Region that defines the triangle.
    /// </summary>
    ///
    /// <returns>
    /// A Region that defines the triangle.
    /// </returns>
    //*************************************************************************

    protected Region
    GetRegion()
	{
		AssertValid();

		GraphicsPath oGraphicsPath = new GraphicsPath();
		
		oGraphicsPath.AddPolygon( GraphicsUtil.DiamondFromCenterAndHalfWidth(
			m_oCenter.X, m_oCenter.Y, m_fHalfWidth) );

		Region oRegion = new Region(oGraphicsPath);

		return (oRegion);
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

		// m_oCenter
		Debug.Assert(m_fHalfWidth >= 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The diamond's center.

	protected PointF m_oCenter;

	/// One half the width of the diamond.

	protected Single m_fHalfWidth;
}

}
