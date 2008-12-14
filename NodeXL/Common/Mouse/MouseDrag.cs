
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//  Class: MouseDrag
//
/// <summary>
/// Represents a mouse drag operation.
/// </summary>
///
/// <remarks>
/// Create an instance of this class when a MouseDown event occurs.  When the
/// mouse is moved, check <see cref="ShouldBeginDrag" /> to determine whether
/// the mouse has moved far enough to begin a mouse drag operation.
/// </remarks>
//*****************************************************************************

public class MouseDrag : NodeXLBase
{
    //*************************************************************************
    //  Constructor: MouseDrag()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="MouseDrag" /> class.
    /// </summary>
	///
	/// <param name="mouseDownLocation">
	/// Location where the MouseDown event occurred, in client coordinates.
	/// </param>
    //*************************************************************************

    public MouseDrag
	(
		Point mouseDownLocation
	)
    {
		m_oMouseDownLocation = mouseDownLocation;
		m_bDragHasBegun = false;

		// AssertValid();
    }

    //*************************************************************************
    //  Property: MouseDownLocation
    //
    /// <summary>
    /// Gets the location where the MouseDown event occurred.
    /// </summary>
    ///
    /// <value>
    /// The location where the MouseDown event occurred, as a <see
	/// cref="Point" /> in client coordinates.
    /// </value>
    //*************************************************************************

    public Point
	MouseDownLocation
    {
        get
        {
            AssertValid();

			return (m_oMouseDownLocation);
        }
    }

    //*************************************************************************
    //  Property: DragHasBegun
    //
    /// <summary>
    /// Gets a flag indicating whether a drag has begun.
    /// </summary>
    ///
    /// <value>
    /// true if a drag operation has begun.
    /// </value>
    //*************************************************************************

    public Boolean
	DragHasBegun
    {
        get
        {
            AssertValid();

			return (m_bDragHasBegun);
        }
    }

	//*************************************************************************
	//	Method: ShouldBeginDrag()
	//
	/// <summary>
	///	Checks whether the mouse has moved far enough to begin a drag
	/// operation.
	/// </summary>
	///
	/// <param name="oMouseEventArgs">
	/// Standard mouse event arguments.
	/// </param>
	///
	/// <remarks>
	/// Having a minimum move distance reduces the chance that the user will
	/// inadvertently begin a drag operation.
	///
	/// <para>
	/// If true is returned, <see cref="DragHasBegun" /> is set to true.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public Boolean
	ShouldBeginDrag
	(
		MouseEventArgs oMouseEventArgs
	)
	{
		Debug.Assert(!m_bDragHasBegun);
		AssertValid();

		PointF oOldLocation = this.MouseDownLocation;
		Single fOldLocationX = oOldLocation.X;
		Single fOldLocationY = oOldLocation.Y;

		Int32 iNewLocationX = oMouseEventArgs.X;
		Int32 iNewLocationY = oMouseEventArgs.Y;

		if (
			Math.Abs( (Single)iNewLocationX - fOldLocationX ) >=
				MinimumMouseMove
			||
			Math.Abs( (Single)iNewLocationY - fOldLocationY ) >=
				MinimumMouseMove
			)
		{
			m_bDragHasBegun = true;
		}

		return (m_bDragHasBegun);
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

		// m_oMouseDownLocation
		// m_bDragHasBegun
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Minimum change in either coordinate before a drag operation begins.

	protected const Single MinimumMouseMove = 4.0F;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Location where the MouseDown event occurred, in client coordinates.

	protected Point m_oMouseDownLocation;

	/// true if a drag operation has begun.

	protected Boolean m_bDragHasBegun;
}

}
