
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//	Class: DynamicFilterColumnsChangedEventArgs
//
/// <summary>
///	Provides information for the <see
/// cref="DynamicFilterDialog.DynamicFilterColumnsChanged" /> event.
/// </summary>
//*****************************************************************************

public class DynamicFilterColumnsChangedEventArgs : EventArgs
{
	//*************************************************************************
	//	Constructor: DynamicFilterColumnsChangedEventArgs()
	//
	/// <summary>
	///	Initializes a new instance of the <see
	/// cref="DynamicFilterColumnsChangedEventArgs" /> class.
	/// </summary>
	///
	/// <param name="dynamicFilterColumns">
	/// One or more ORed <see cref="DynamicFilterColumns" /> flags indicating
	/// which columns changed.
	/// </param>
	//*************************************************************************

	public DynamicFilterColumnsChangedEventArgs
	(
		DynamicFilterColumns dynamicFilterColumns
	)
	{
		m_eDynamicFilterColumns = dynamicFilterColumns;

		AssertValid();
	}

	//*************************************************************************
	//	Property: DynamicFilterColumns
	//
	/// <summary>
	/// An ORed combination of flags indicating which columns changed.
	/// </summary>
	///
	/// <value>
	/// One or more ORed <see cref="DynamicFilterColumns" /> flags indicating
	/// which columns changed.
	/// </value>
	//*************************************************************************

	public DynamicFilterColumns
	DynamicFilterColumns
	{
		get
		{
			AssertValid();

			return (m_eDynamicFilterColumns);
		}
	}


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

	[Conditional("DEBUG")] 

	public void
	AssertValid()
	{
		// m_eDynamicFilterColumns
	}


	//*************************************************************************
	//	Protected member data
	//*************************************************************************

	/// One or more ORed DynamicFilterColumns flags indicating which columns
	/// changed.

	protected DynamicFilterColumns m_eDynamicFilterColumns;
}


//*****************************************************************************
//  Delegate: DynamicFilterColumnsChangedEventHandler
//
/// <summary>
/// Represents a method that will handle the <see
/// cref="DynamicFilterDialog.DynamicFilterColumnsChanged" /> event.
/// </summary>
///
/// <param name="sender">
/// The source of the event.
/// </param>
///
/// <param name="e">
///	An <see cref="DynamicFilterColumnsChangedEventArgs" /> object that contains
/// the event data.
/// </param>
//*****************************************************************************

public delegate void
DynamicFilterColumnsChangedEventHandler
(
	Object sender,
	DynamicFilterColumnsChangedEventArgs e
);

}
