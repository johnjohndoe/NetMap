
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.NodeXL.Adapters;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.DesktopApplication
{
//*****************************************************************************
//	Class: SaveDocumentFileDialog
//
/// <summary>
///	Represents a dialog box for saving a document to a file.
/// </summary>
///
///	<remarks>
///	Call <see cref="ShowDialogAndSaveDocument" /> to allow the user to save a
/// document in a format of his choice to a location of his choice.
///
/// <para>
/// The only file format currently supported is the one handled by the
/// SimpleGraphAdapter class.  As additional graph adapters are added, this
/// dialog will be modified to support them.
/// </para>
///
///	</remarks>
//*****************************************************************************

public class SaveDocumentFileDialog : SaveFileDialog2
{
	//*************************************************************************
	//	Constructor: SaveDocumentFileDialog()
	//
	/// <summary>
	///	Initializes a new instance of the <see cref="SaveDocumentFileDialog" />
	/// class.
	/// </summary>
	//*************************************************************************

	public SaveDocumentFileDialog()
	:
	base
	(
		String.Empty,
		String.Empty
	)
	{
		// (Do nothing.)
	}

	//*************************************************************************
	//	Method: ShowDialogAndSaveDocument()
	//
	/// <summary>
	///	Saves the document in a format selected by the user.
	/// </summary>
	///
	///	<param name="oDocument">
	///	Document to save.
	/// </param>
	///
	/// <returns>
	///	DialogResult.OK if the user selected a file name and the document was
	/// successfully saved.
	/// </returns>
	//*************************************************************************

	public DialogResult
	ShowDialogAndSaveDocument
	(
		Document oDocument
	)
	{
		Debug.Assert(oDocument != null);
		AssertValid();

		m_oSaveFileDialog.FileName = DefaultFileName;

		// Let the base class do most of the work.  ShowDialogAndSaveObject()
		// will call SaveObject() in this class.

		return ( ShowDialogAndSaveObject(oDocument) );
	}

	//*************************************************************************
	//	Method: GetDialogTitle()
	//
	/// <summary>
	/// Gets the title to use for the dialog.
	/// </summary>
	///
	///	<param name="oObjectBeingSaved">
	///	Object being saved.
	/// </param>
	//*************************************************************************

	protected override String
	GetDialogTitle
	(
		Object oObjectBeingSaved
	)
	{
		return (DialogTitle);
	}

	//*************************************************************************
	//	Method: GetFilter()
	//
	/// <summary>
	///	Gets the filter to use for the dialog.
	/// </summary>
	///
	///	<param name="oObjectBeingSaved">
	///	Object being saved.
	/// </param>
	//*************************************************************************

	protected override String
	GetFilter
	(
		Object oObjectBeingSaved
	)
	{
		return (GraphAdapterManager.FileDialogFilter);
	}

	//*************************************************************************
	//	Method: SaveObject()
	//
	/// <summary>
	/// Saves an object to a file.
	/// </summary>
	///
	///	<param name="oObject">
	///	Object to save.
	/// </param>
	///
	///	<param name="sFileName">
	///	File name to save the object to.
	/// </param>
	///
	/// <remarks>
	///	This is called by the base-class ShowDialogAndSaveObject() method.
	/// </remarks>
	//*************************************************************************

	protected override void
	SaveObject
	(
		Object oObject,
		String sFileName
	)
	{
		Debug.Assert(oObject != null);
		Debug.Assert(oObject is Document);
		Debug.Assert( !String.IsNullOrEmpty(sFileName) );

		Document oDocument = (Document)oObject;

		// Get the index of the filter the user chose.

		Int32 iOneBasedFilterIndex = m_oSaveFileDialog.FilterIndex;

		Debug.Assert(iOneBasedFilterIndex >= 1);

		// Get a compatible graph adapter.

		IGraphAdapter oGraphAdapter =
			GraphAdapterManager.FileDialogFilterIndexToGraphAdapter(
				iOneBasedFilterIndex);

		MainForm.ShowWaitCursor = true;

		try
		{
			oDocument.Save(sFileName, oGraphAdapter, false);
		}
		finally
		{
			MainForm.ShowWaitCursor = false;
		}
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

	/// Default name of the file to save, without an extension.

	protected const String DefaultFileName = "Graph";


	//*************************************************************************
	//	Protected member data
	//*************************************************************************

	/// Title to use for this dialog.

	protected const String DialogTitle =
		"Save Graph File";
}

}
