
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.DesktopApplication
{
//*****************************************************************************
//	Class: NewDocumentTitleCreator
//
/// <summary>
/// Creates titles for new documents.
/// </summary>
///
/// <remarks>
/// Call the static <see cref="CreateTitle" /> method to create a title for a
/// new document.
///
/// <para>
/// Documents created from files obtain their title from the file name.  This
/// class should be used only to create titles for new documents not created
/// from files.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class NewDocumentTitleCreator : Object
{
	//*************************************************************************
	//	Static constructor: NewDocumentTitleCreator()
	//
	/// <summary>
	/// Initializes the <see cref="NewDocumentTitleCreator" /> class.
	/// </summary>
	//*************************************************************************

	static NewDocumentTitleCreator()
	{
		m_iTitlesCreated = 0;
	}

	//*************************************************************************
	//	Method: CreateTitle()
	//
	/// <summary>
	///	Creates a title for a new document.
	/// </summary>
	//*************************************************************************

	public static String
	CreateTitle()
	{
		// Create a title with the format TitleRootN.

		String sTitle = String.Format(

			"{0}{1}"
			,
			TitleRoot, (m_iTitlesCreated + 1)
			);

		m_iTitlesCreated++;

		return (sTitle);
	}


	//*************************************************************************
	//	Private constants
	//*************************************************************************

	/// Root for the created titles.

	private static readonly String TitleRoot = "Graph";


	//*************************************************************************
	//	Private fields
	//*************************************************************************

	/// Number of titles created so far.

	private static Int32 m_iTitlesCreated;
}

}
