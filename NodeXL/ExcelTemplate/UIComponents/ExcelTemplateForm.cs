
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//	Class: ExcelTemplateForm
//
/// <summary>
///	Base class for all forms in the Excel template.
/// </summary>
///
/// <remarks>
///	This class adds commonly used functionality to the FormPlus class.  All
/// ExcelTemplate forms should derive from it.
/// </remarks>
//*****************************************************************************

public class ExcelTemplateForm : FormPlus
{
	//*************************************************************************
	//	Public constants
	//*************************************************************************

	/// String format for most Int32s.

	public static readonly String Int32Format = NodeXLBase.Int32Format;

	/// String format for most Singles.

	public static readonly String SingleFormat = NodeXLBase.SingleFormat;

	/// String format for most Doubles.

	public static readonly String DoubleFormat = NodeXLBase.DoubleFormat;

	/// Text to display in an empty list.

	public const String EmptyListText = "(none)";

	/// Text to display in a list to tell the user to select an item from the
	/// list.

	public const String SelectOneText = "(Select one)";


	//*************************************************************************
	//	Constructor: ExcelTemplateForm()
	//
	/// <summary>
	///	Initializes a new instance of the ExcelTemplateForm class.
	/// </summary>
	//*************************************************************************

	public ExcelTemplateForm()
	{
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
