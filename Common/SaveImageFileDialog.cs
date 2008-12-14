
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//	Class: SaveImageFileDialog
//
/// <summary>
///	Represents a dialog box for saving an image file.
/// </summary>
///
///	<remarks>
///	Call ShowDialogAndSaveImage() to allow the user to save an image in a
///	format of his choice to a location of his choice.
///	</remarks>
//*****************************************************************************

public class SaveImageFileDialog : SaveFileDialog2
{
	//*************************************************************************
	//	Constructor: SaveImageFileDialog()
	//
	/// <summary>
	/// Initializes a new instance of the SaveImageFileDialog class.
	/// </summary>
	///
	/// <param name="sInitialDirectory">
	/// Initial directory the dialog will display.  Use an empty string to let
	/// the dialog select an initial directory.
	/// </param>
	///
	/// <param name="sInitialFileName">
	/// Initial file name.  Can be a complete path, a path without an
	///	extension, a file name, or a file name without an extension.
	/// </param>
	//*************************************************************************

	public SaveImageFileDialog
	(
		String sInitialDirectory,
		String sInitialFileName

	) : base(sInitialDirectory, sInitialFileName)
	{
		m_sDialogTitle = "Save As Image";
	}

	//*************************************************************************
	//	Property: DialogTitle
	//
	/// <summary>
	/// Gets or sets the dialog's title.
	/// </summary>
	///
	/// <value>
	/// The dialog's title.  Can't be null or empty.  The default is
	/// "Save As Image".
	/// </value>
	//*************************************************************************

	public String
    DialogTitle
	{
		get
		{
			AssertValid();

			return (m_sDialogTitle);
		}

		set
		{
			m_sDialogTitle = value;

			AssertValid();
		}
	}

	//*************************************************************************
	//	Method: ShowDialogAndSaveImage()
	//
	/// <summary>
	/// Shows the file save dialog and saves the image to the selected file.
	/// </summary>
	///
	///	<param name="oImage">
	///	Image to save.
	/// </param>
	///
	/// <returns>
	///	DialogResult.OK if the user selected a file name and the image was
	/// successfully saved.
	/// </returns>
	///
	/// <remarks>
	///	This method allows the user to select an image file name and format.
	///	It then saves the image in the selected format.
	/// </remarks>
	//*************************************************************************

	public DialogResult
	ShowDialogAndSaveImage
	(
		Image oImage
	)
	{
		Debug.Assert(oImage != null);
		AssertValid();

		// Let the base class do most of the work.  The actual saving will be
		// done by SaveObject() in this class.

		return ( ShowDialogAndSaveObject(oImage) );
	}

	//*************************************************************************
	//	Method: GetDialogTitle()
	//
	/// <summary>
	/// Returns the title to use for the dialog.
	/// </summary>
	///
	///	<param name="oObjectBeingSaved">
	///	Object being saved.
	/// </param>
	///
	/// <returns>
	///	Title to use for the dialog.
	/// </returns>
	//*************************************************************************

	protected override String
	GetDialogTitle
	(
		Object oObjectBeingSaved
	)
	{
		return (m_sDialogTitle);
	}

	//*************************************************************************
	//	Method: GetFilter()
	//
	/// <summary>
	/// Returns the filter to use for the dialog.
	/// </summary>
	///
	///	<param name="oObjectBeingSaved">
	///	Object being saved.
	/// </param>
	///
	/// <returns>
	///	Filter to use for the dialog.
	/// </returns>
	//*************************************************************************

	protected override String
	GetFilter
	(
		Object oObjectBeingSaved
	)
	{
		return (SaveableImageFormats.Filter);
	}

	//*************************************************************************
	//	Method: SaveObject()
	//
	/// <summary>
	/// Saves the object to the specified file.
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
		Image oImage = (Image)oObject;

		// Save the image in the format selected by the user.

		oImage.Save( sFileName,
			SaveableImageFormats.ImageFormats[
			m_oSaveFileDialog.FilterIndex - 1] );
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

		Debug.Assert( !String.IsNullOrEmpty(m_sDialogTitle) );
	}


	//*************************************************************************
	//	Protected fields
	//*************************************************************************

	/// Dialog title.

	protected String m_sDialogTitle;
}

}
