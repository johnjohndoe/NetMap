
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.ControlLib
{
//*****************************************************************************
//	Class: ControlUtil
//
/// <summary>
///	Control utility methods.
/// </summary>
///
///	<remarks>
///	This class contains utility methods for dealing with Control-derived
/// objects.  All methods are static.
///	</remarks>
//*****************************************************************************

internal class ControlUtil
{
	//*************************************************************************
	//	Constructor: ControlUtil()
	//
	/// <summary>
	/// Do not use this constructor.
	/// </summary>
	///
	/// <remarks>
	/// Do not use this constructor.  All ControlUtil methods are static.
	/// </remarks>
	//*************************************************************************

	private
	ControlUtil()
	{
		// (All methods are static.)
	}

	//*************************************************************************
	//	Method: GetClientMousePosition()
	//
	/// <summary>
	///	Gets the current mouse position in client coordinates.
	/// </summary>
	///
	///	<param name="oControl">
	/// Control to use for the client coordinates.
	/// </param>
	///
	/// <returns>
	/// Mouse position in client coordinates.
	/// </returns>
	///
	/// <remarks>
	///	NOTE: The point returned by this method can be outside the control's
	/// client area.  This can happen if the user is moving the mouse quickly.
	/// The caller should check for this.
	/// </remarks>
	//*************************************************************************

	public static Point
	GetClientMousePosition
	(
		Control oControl
	)
	{
		Debug.Assert(oControl != null);

		// Get the mouse position in screen coordinates.

		Point oPointScreen = Control.MousePosition;

		// Convert to client coordinates.

		return ( oControl.PointToClient(oPointScreen) );
	}
}

}
