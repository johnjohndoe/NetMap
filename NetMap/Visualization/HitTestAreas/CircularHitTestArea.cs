
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//  Class: CircularHitTestArea
//
/// <summary>
/// Provides hit-testing for a circular area.
/// </summary>
///
/// <remarks>
/// This is one of a set of classes that are used for hit-testing.  The classes
/// provide a lightweight alternative to using System.Drawing.Region objects
/// for hit-testing.
/// </remarks>
//*****************************************************************************

public class CircularHitTestArea : HitTestArea
{
    //*************************************************************************
    //  Constructor: CircularHitTestArea()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="CircularHitTestArea" /> class.
    /// </summary>
	///
    /// <param name="center">
    /// The circle's center.
    /// </param>
	///
    /// <param name="radius">
    /// The circle's radius.
    /// </param>
    //*************************************************************************

    public CircularHitTestArea
	(
		PointF center,
		Single radius
	)
    {
		Debug.Assert(radius >= 0);

		m_oCenter = center;
		m_fRadiusSquared = radius * radius;

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

		Single fXDiff = m_oCenter.X - point.X;
		Single fYDiff = m_oCenter.Y - point.Y;

		return (fXDiff * fXDiff + fYDiff * fYDiff <= m_fRadiusSquared);
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

		// Compute the distance squared of the circle's center from the nearest
		// point on the rectangle.

		Int32 iLeft = rectangle.Left;
		Int32 iTop = rectangle.Top;
		Int32 iRight = rectangle.Right;
		Int32 iBottom = rectangle.Bottom;
		Int32 iCenterX = (Int32)m_oCenter.X;
		Int32 iCenterY = (Int32)m_oCenter.Y;

		Int32 iDistanceSquared = 0;
		Int32 iTemp;

		if ( (iTemp = iCenterX - iLeft) < 0 )
		{
			iDistanceSquared = iTemp * iTemp;
		}
		else if ( (iTemp = iCenterX - iRight) > 0 )
		{
			iDistanceSquared = iTemp * iTemp;
		}

		if ( (iTemp = iCenterY - iTop) < 0 )
		{
			iDistanceSquared += iTemp * iTemp;
		}
		else if ( (iTemp = iCenterY - iBottom) > 0 )
		{
			iDistanceSquared += iTemp * iTemp;
		}

		return (iDistanceSquared < m_fRadiusSquared);
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
		Debug.Assert(m_fRadiusSquared >= 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The circle's center.

	protected PointF m_oCenter;

    /// The circle's radius, squared.

	protected Single m_fRadiusSquared;
}

}
