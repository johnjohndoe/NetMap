
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.DesktopApplication
{
//*****************************************************************************
//	Class: DesktopApplicationForm
//
/// <summary>
///	Base class for all forms in the NetMap desktop application.
/// </summary>
///
/// <remarks>
///	This class adds commonly used functionality to the FormPlus class.  All
/// DesktopApplication forms should derive from it.
/// </remarks>
//*****************************************************************************

public class DesktopApplicationForm : FormPlus
{
	//*************************************************************************
	//	Public constants
	//*************************************************************************

	/// String format for most Int32s.

	public static readonly String Int32Format = NetMapBase.Int32Format;

	/// String format for most Singles.

	public static readonly String SingleFormat = NetMapBase.SingleFormat;

	/// Text to display in an empty list.

	public static readonly String EmptyListText = "(none)";


	//*************************************************************************
	//	Constructor: DesktopApplicationForm()
	//
	/// <summary>
	///	Initializes a new instance of the DesktopApplicationForm class.
	/// </summary>
	//*************************************************************************

	public DesktopApplicationForm()
	{
		// The only form that should show in the taskbar is the MainForm, which
		// should set ShowInTaskbar to true within its constructor.

		this.ShowInTaskbar = false;
	}


	//*************************************************************************
	//	Method: AssertValid()
	//
	/// <summary>
	///	Asserts if the object is in an invalid state.  Debug-only.
	/// </summary>
	//*************************************************************************

	public override void
	AssertValid()
	{
		base.AssertValid();
	}


	//*************************************************************************
	//	Protected fields
	//*************************************************************************

	// (None.)
}

}
