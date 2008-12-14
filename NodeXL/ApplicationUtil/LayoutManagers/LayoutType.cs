
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NodeXL.Visualization;

namespace Microsoft.NodeXL.ApplicationUtil
{
//*****************************************************************************
//  Enum: LayoutType
//
/// <summary>
/// Specifies the layouts supported by the <see cref="LayoutManager" /> class.
/// </summary>
//*****************************************************************************

public enum
LayoutType
{
	/// <summary>
	/// Use a <see cref="CircleLayout" />.
	/// </summary>

	Circle,

	/// <summary>
	/// Use a <see cref="SpiralLayout" />.
	/// </summary>

	Spiral,

	/// <summary>
	/// Use a <see cref="SinusoidHorizontalLayout" />.
	/// </summary>

	SinusoidHorizontal,

	/// <summary>
	/// Use a <see cref="SinusoidVerticalLayout" />.
	/// </summary>

	SinusoidVertical,

	/// <summary>
	/// Use a <see cref="FruchtermanReingoldLayout" />.
	/// </summary>

	FruchtermanReingold,

	/// <summary>
	/// Use a <see cref="GridLayout" />.
	/// </summary>

	Grid,

	/// <summary>
	/// Use a <see cref="RandomLayout" />.
	/// </summary>

	Random,

	/// <summary>
	/// Use a <see cref="SugiyamaLayout" />.
	/// </summary>

	Sugiyama,


	// To add support for an additional layout, do the following:
	//
	//   1. Add a value to this enumeration.
	//
	//   2. Modify the switch statement in
	//      LayoutManager.ApplyLayoutToNodeXLControl().
	//
	//   3. Add another menu item in LayoutManagerForMenu.AddMenuItems().
}

}
