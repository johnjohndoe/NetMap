
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;

namespace Microsoft.NetMap.Visualization
{
//*****************************************************************************
//  Class: HitTestArea
//
/// <summary>
/// Base class for classes that support hit-testing.
/// </summary>
///
/// <remarks>
/// This is the base class for a set of classes that are used for hit-testing.
/// The classes provide a lightweight alternative to using
/// System.Drawing.Region objects for hit-testing.
/// </remarks>
//*****************************************************************************

public abstract class HitTestArea : Object
{
    //*************************************************************************
    //  Constructor: HitTestArea()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="HitTestArea" /> class.
    /// </summary>
    //*************************************************************************

    public HitTestArea()
    {
		// (Do nothing.)

		// AssertValid();
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

    public abstract Boolean
    Contains
    (
		PointF point
    );

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

    public abstract Boolean
    IntersectsWith
    (
		Rectangle rectangle
    );


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public virtual void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
