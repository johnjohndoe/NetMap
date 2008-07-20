
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;
using Microsoft.NetMap.Core;
using Microsoft.NetMap.Adapters;

namespace Microsoft.NetMap.DesktopApplication
{
//*****************************************************************************
//	Class: OpenDocumentFileDialog
//
/// <summary>
///	Represents a dialog box for opening a document file.
/// </summary>
///
///	<remarks>
///	Call <see cref="ShowDialogAndOpenDocumentFile" /> to allow the user to open
/// a document file containing graph data from a location of his choice.
///
/// <para>
/// The only file format currently supported is the one handled by the
/// SimpleGraphAdapter class.  As additional graph adapters are added, this
/// dialog will be modified to support them.
/// </para>
///
///	</remarks>
//*****************************************************************************

public class OpenDocumentFileDialog : OpenFileDialog2
{
	//*************************************************************************
	//	Constructor: OpenDocumentFileDialog()
	//
	/// <summary>
	///	Initializes a new instance of the <see cref="OpenDocumentFileDialog" />
	/// class.
	/// </summary>
	//*************************************************************************

	public OpenDocumentFileDialog()
	:
	base
	(
		String.Empty,
		String.Empty
	)
	{
		// (Do nothing else.)
	}

	//*************************************************************************
	//	Method: ShowDialogAndOpenDocumentFile()
	//
	/// <summary>
	/// Opens a graph data file.
	/// </summary>
	///
	///	<param name="oDocument">
	///	Where a new <see cref="Document" /> object gets stored.
	/// </param>
	///
	/// <returns>
	///	DialogResult.OK if the user selected a file name and a <see
	/// cref="Document" /> objects was successfully created from the file.
	/// </returns>
	///
	/// <remarks>
	///	This method allows the user to select a document file name.  It then
	/// opens the file and creates a <see cref="Document" /> object from it.
	/// </remarks>
	//*************************************************************************

	public DialogResult
	ShowDialogAndOpenDocumentFile
	(
		out Document oDocument
	)
	{
		AssertValid();

		// Let the base class do most of the work.  ShowDialogAndOpenObject()
		// calls OpenObject(), which will open the file and create a Document
		// object from it.

		Object oObject;

		DialogResult oDialogResult = ShowDialogAndOpenObject(out oObject);
		Debug.Assert(oObject == null || oObject is Document);
		oDocument = (Document)oObject;

		return (oDialogResult);
	}

	//*************************************************************************
	//	Method: GetDialogTitle()
	//
	/// <summary>
	/// Gets the title to use for the dialog.
	/// </summary>
	//*************************************************************************

	protected override String
	GetDialogTitle()
	{
		AssertValid();

		return (DialogTitle);
	}

	//*************************************************************************
	//	Method: GetFilter()
	//
	/// <summary>
	///	Gets the filter to use for the dialog.
	/// </summary>
	//*************************************************************************

	protected override String
	GetFilter()
	{
		AssertValid();

		return (GraphAdapterManager.FileDialogFilter);
	}

	//*************************************************************************
	//	Method: OpenObject()
	//
	/// <summary>
	/// Opens a graph data file and creates a <see cref="Document" /> object
	/// from it.
	/// </summary>
	///
	///	<param name="sFileName">
	///	File name to open, including a full path.
	/// </param>
	///
	/// <param name="oObject">
	/// Where the new <see cref="Document" /> object get stored.
	/// </param>
	///
	/// <remarks>
	///	This is called by the base-class ShowDialogAndOpenObject() method.
	/// </remarks>
	//*************************************************************************

	protected override void
	OpenObject
	(
		String sFileName,
		out Object oObject
	)
	{
		Debug.Assert( !String.IsNullOrEmpty(sFileName) );
		Debug.Assert( File.Exists(sFileName) );
		AssertValid();

		oObject = null;

		MainForm.ShowWaitCursor = true;

		// Get the index of the filter the user chose.

		Int32 iOneBasedFilterIndex = m_oOpenFileDialog.FilterIndex;

		Debug.Assert(iOneBasedFilterIndex >= 1);

		// Get a compatible graph adapter.

		IGraphAdapter oGraphAdapter =
			GraphAdapterManager.FileDialogFilterIndexToGraphAdapter(
				iOneBasedFilterIndex);

		try
		{
			Document oDocument;

			Document.Load(sFileName, oGraphAdapter, false, out oDocument);

            oObject = oDocument;
		}
		finally
		{
			MainForm.ShowWaitCursor = false;
		}

		Debug.Assert(oObject is Document);
	}

	//*************************************************************************
	//	Method: AssertValid()
	//
	/// <summary>
	///	Asserts if the object is in an invalid state.  Debug-only.
	/// </summary>
	//*************************************************************************

	// [Conditional("DEBUG")] 

	public override void
	AssertValid()
	{
		base.AssertValid();
	}


	//*************************************************************************
	//	Protected constants
	//*************************************************************************

	/// Title to use for this dialog.

	protected const String DialogTitle =
		"Open Graph File";
}

}
