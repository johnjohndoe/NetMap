
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: SaveFileDialog2
//
/// <summary>
/// Represents a smart SaveFileDialog that knows how to get a file name from
/// the user and save an object to the file.  Abstract.
/// </summary>
///
/// <remarks>
/// This is an abstract base class.  Each class derived from this one is
/// responsible for saving one type of object, such as an image or an XML
/// document.
///
/// A derived class must implement GetDialogTitle(), GetFilter(), and
/// SaveObject().  It should also implement a public method called
/// ShowDialogAndSaveXXX(), where XXX is Image or XML, for example.
/// ShowDialogAndSaveXXX() should call ShowDialogAndSaveObject() in this base
/// class to do most of the work.
/// </remarks>
//*****************************************************************************

public abstract class SaveFileDialog2
{
    //*************************************************************************
    //  Constructor: SaveFileDialog2()
    //
    /// <summary>
    /// Initializes a new instance of the SaveFileDialog2 class.
    /// </summary>
    ///
    /// <param name="sInitialDirectory">
    /// Initial directory the dialog will display.  Use an empty string to let
    /// the dialog select an initial directory.
    /// </param>
    ///
    /// <param name="sInitialFileName">
    /// Initial file name.  Can be a complete path, a path without an
    /// extension, a file name, or a file name without an extension.
    /// </param>
    //*************************************************************************

    public SaveFileDialog2
    (
        String sInitialDirectory,
        String sInitialFileName
    )
    {
        // Create and initialize a SaveFileDialog object.

        m_oSaveFileDialog = new SaveFileDialog();
        m_oSaveFileDialog.InitialDirectory = sInitialDirectory;
        m_oSaveFileDialog.FileName = sInitialFileName;
    }

    //*************************************************************************
    //  Method: ShowDialogAndSaveObject()
    //
    /// <summary>
    /// Shows the file save dialog and saves the object to the selected file.
    /// </summary>
    ///
    /// <param name="oObject">
    /// Object to save.
    /// </param>
    ///
    /// <returns>
    /// DialogResult.  DialogResult.OK if the user selected a file name and
    /// the object was successfully saved.
    /// </returns>
    ///
    /// <remarks>
    /// This method allows the user to select a file name and format.  It
    /// then saves the object in the selected format.  The virtual method
    /// SaveObject() implemented in the derived class does the actual saving.
    /// All other details are handled by this base-class method.
    /// </remarks>
    //*************************************************************************

    protected DialogResult
    ShowDialogAndSaveObject
    (
        Object oObject
    )
    {
        Debug.Assert(oObject != null);
        AssertValid();

        DialogResult eDialogResult = DialogResult.Cancel;

        m_oSaveFileDialog.Title = GetDialogTitle(oObject);
        m_oSaveFileDialog.Filter = GetFilter(oObject);

        while (true)
        {
            // Get a file name from the user.

            eDialogResult = m_oSaveFileDialog.ShowDialog();

            if (eDialogResult != DialogResult.OK)
                break;

            String sFileName = GetNameOfFileToSave();

            // Check whether the file is read-only.

            if (
                File.Exists(sFileName) &&

                ( (File.GetAttributes(sFileName)
                    & FileAttributes.ReadOnly) != 0 )
                )
            {
                OnSaveError(sFileName + " is read-only.");
            }
            else
            {
                // Tell the derived class to save the object.

                Exception oException = null;

                try
                {
                    SaveObject(oObject, sFileName);
                    break;
                }
                catch (IOException oIOException)
                {
                    oException = oIOException;
                }
                catch (UnauthorizedAccessException oUnauthorizedAccessException)
                {
                    oException = oUnauthorizedAccessException;
                }

                if (oException != null)
                {
                    OnSaveError("The file could not be saved.\n\nDetails:\n\n"
                        + oException.Message);
                }
            }
        }

        return (eDialogResult);
    }

    //*************************************************************************
    //  Method: GetNameOfFileToSave()
    //
    /// <summary>
    /// Returns the full name of the file to save.
    /// </summary>
    ///
    /// <returns>
    /// String.  Full name of the file to save, including the extension.
    /// </returns>
    ///
    /// <remarks>
    /// This method works around an odd behavior in SaveFileDialog.  If the
    /// user enters a file name that contains a period, SaveFileDialog does not
    /// add an extension to the file name returned by the FileName property.
    /// If the user enters "A.B" into the file name text box and the selected
    /// filter is "JPEG (*.jpg)", for example, the FileName property returns
    /// "A.B" instead of "A.B.jpg".  It probably assumes that the user wants an
    /// extension of ".B", so it doesn't add an extension of its own.
    ///
    /// This method adds the extension if it's missing.
    /// </remarks>
    //*************************************************************************

    protected String
    GetNameOfFileToSave()
    {
        // Get the file name, which may or may not be correct.

        String sFileName = m_oSaveFileDialog.FileName;

        // Get the filter pattern for the selected filter.  Sample:
        // "*.jpg;*.jpeg;*.jfif".

        String [] asFilterComponents = m_oSaveFileDialog.Filter.Split('|');

        String sFilterPattern =
            asFilterComponents[ 2 * (m_oSaveFileDialog.FilterIndex - 1) + 1];

        // If the selected filter is for all files, don't add an extension.
        // The user should be able to name the file anything he wants, with or
        // without an extension.  (This is how Notepad behaves.)

        if (sFilterPattern != "*.*")
        {
            // Get the first file type.  Sample: "*.jpg".

            String [] asFileTypes = sFilterPattern.Split(';');
            Debug.Assert(asFileTypes.Length > 0);
            String sFileType = asFileTypes[0];

            // Strip off the asterisk.  Sample: ".jpg".

            String sFileTypeNoAsterisk = sFileType.ToLower().Substring(1);

            // If the file name does not end with the file type, add it.

            if ( !sFileName.ToLower().EndsWith(sFileTypeNoAsterisk) )
                sFileName += sFileTypeNoAsterisk;
        }

        return (sFileName);
    }

    //*************************************************************************
    //  Method: GetDialogTitle()
    //
    /// <summary>
    /// Returns the title to use for the dialog.
    /// </summary>
    ///
    /// <param name="oObjectBeingSaved">
    /// Object being saved.
    /// </param>
    ///
    /// <returns>
    /// Title to use for the dialog.  Sample: "Save As Image".
    /// </returns>
    ///
    /// <remarks>
    /// Derived classes must implement this virtual method.
    /// </remarks>
    //*************************************************************************

    protected abstract String
    GetDialogTitle
    (
        Object oObjectBeingSaved
    );

    //*************************************************************************
    //  Method: GetFilter()
    //
    /// <summary>
    /// Returns the filter to use for the dialog.
    /// </summary>
    ///
    /// <param name="oObjectBeingSaved">
    /// Object being saved.
    /// </param>
    ///
    /// <returns>
    /// Filter to use for the dialog.
    /// </returns>
    ///
    /// <remarks>
    /// Derived classes must implement this virtual method.
    /// </remarks>
    //*************************************************************************

    protected abstract String
    GetFilter
    (
        Object oObjectBeingSaved
    );

    //*************************************************************************
    //  Method: SaveObject()
    //
    /// <summary>
    /// Saves the object.
    /// </summary>
    ///
    /// <param name="oObject">
    /// Object to save.
    /// </param>
    ///
    /// <param name="sFileName">
    /// File name to save the object to.
    /// </param>
    ///
    /// <remarks>
    /// Derived classes must implement this virtual method.  Any exceptions
    /// thrown by the derived method are caught and handled by this base class.
    /// </remarks>
    //*************************************************************************

    protected abstract void
    SaveObject
    (
        Object oObject,
        String sFileName
    );

    //*************************************************************************
    //  Method: OnSaveError()
    //
    /// <summary>
    /// Gets called when an error occurs while saving the object.
    /// </summary>
    ///
    /// <param name="sErrorDescription">
    /// Save error description.
    /// </param>
    //*************************************************************************

    protected void
    OnSaveError
    (
        String sErrorDescription
    )
    {
        MessageBox.Show(sErrorDescription, m_oSaveFileDialog.Title,
            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")] 

    public virtual void
    AssertValid()
    {
        Debug.Assert(m_oSaveFileDialog != null);
    }


    //*************************************************************************
    //  Protected member data
    //*************************************************************************

    /// Common dialog.

    protected SaveFileDialog m_oSaveFileDialog;
}

}
