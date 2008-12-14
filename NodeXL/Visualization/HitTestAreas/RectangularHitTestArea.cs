
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;

namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//  Class: RectangularHitTestArea
//
/// <summary>
/// Provides hit-testing for a rectangular area.
/// </summary>
///
/// <remarks>
/// This is one of a set of classes that are used for hit-testing.  The classes
/// provide a lightweight alternative to using System.Drawing.Region objects
/// for hit-testing.
/// </remarks>
//*****************************************************************************

public class RectangularHitTestArea : HitTestArea
{
    //*************************************************************************
    //  Constructor: RectangularHitTestArea()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="RectangularHitTestArea" /> class.
    /// </summary>
	///
    /// <param name="rectangle">
    /// Rectangle that defines the hit-test area.
    /// </param>
    //*************************************************************************

    public RectangularHitTestArea
	(
		Rectangle rectangle
	)
    {
		m_oRectangle = rectangle;

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

		return ( m_oRectangle.Contains( Point.Round(point) ) );
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

		return ( m_oRectangle.IntersectsWith(rectangle) );
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

		// m_oRectangle
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Rectangle that defines the hit-test area.

	protected Rectangle m_oRectangle;
}

}
