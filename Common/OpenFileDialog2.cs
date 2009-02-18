
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: OpenFileDialog2
//
/// <summary>
/// Represents a smart OpenFileDialog that knows how to get a file name from
/// the user and open an object from the file.  Abstract.
/// </summary>
///
/// <remarks>
/// This is an abstract base class.  Each class derived from this one is
/// responsible for opening one type of file and creating an object from the
/// file contents.  A file type might be an image or an XML document, for
/// example.
///
/// A derived class must implement GetDialogTitle(), GetFilter(), and
/// OpenObject().  It should also implement a public method called
/// ShowDialogAndOpenXXX(), where XXX is Image or XML, for example.
/// ShowDialogAndOpenXXX() should call ShowDialogAndOpenObject() in this base
/// class to do most of the work.
/// </remarks>
//*****************************************************************************

public abstract class OpenFileDialog2
{
    //*************************************************************************
    //  Constructor: OpenFileDialog2()
    //
    /// <summary>
    /// Initializes a new instance of the OpenFileDialog2 class.
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

    public OpenFileDialog2
    (
        String sInitialDirectory,
        String sInitialFileName
    )
    {
        // Create and initialize an OpenFileDialog object.

        m_oOpenFileDialog = new OpenFileDialog();
        m_oOpenFileDialog.InitialDirectory = sInitialDirectory;
        m_oOpenFileDialog.FileName = sInitialFileName;
    }

    //*************************************************************************
    //  Method: ShowDialogAndOpenObject()
    //
    /// <summary>
    /// Opens a file and creates an object from the file contents.
    /// </summary>
    ///
    /// <param name="oObject">
    /// Where the object created from the file contents will be stored.
    /// </param>
    ///
    /// <returns>
    /// DialogResult.OK if the user selected a file name and the file was
    /// successfully opened.
    /// </returns>
    ///
    /// <remarks>
    /// This method allows the user to select a file name.  It then opens the
    /// file and creates an object from the file contents.  The virtual method
    /// OpenObject() implemented in the derived class does the actual opening.
    /// All other details are handled by this base-class method.
    /// </remarks>
    //*************************************************************************

    protected DialogResult
    ShowDialogAndOpenObject
    (
        out Object oObject
    )
    {
        AssertValid();

        oObject = null;
        DialogResult oDialogResult = DialogResult.Cancel;
        m_oOpenFileDialog.Title = GetDialogTitle();
        m_oOpenFileDialog.Filter = GetFilter();

        while (true)
        {
            // Get a file name from the user.

            oDialogResult = m_oOpenFileDialog.ShowDialog();

            if (oDialogResult != DialogResult.OK)
                break;

            // Let the derived class open an object from the file.

            Exception oException = null;

            try
            {
                OpenObject(m_oOpenFileDialog.FileName, out oObject);
            }
            catch (IOException oIOException)
            {
                oException = oIOException;
            }
            catch (UnauthorizedAccessException oUnauthorizedAccessException)
            {
                oException = oUnauthorizedAccessException;
            }
            catch (FormatException oFormatException)
            {
                oException = oFormatException;
            }

            if (oException != null)
            {
                OnOpenError( String.Format(
                
                    "The file could not be opened.\n\n{0}"
                    ,
                    oException.Message
                    ) );

                // Let the user select another file.

                continue;
            }

            break;
        }

        return (oDialogResult);
    }

    //*************************************************************************
    //  Method: GetDialogTitle()
    //
    /// <summary>
    /// Returns the title to use for the dialog.
    /// </summary>
    ///
    /// <returns>
    /// Title to use for the dialog.  Sample: "Open XML".
    /// </returns>
    ///
    /// <remarks>
    /// Derived classes must implement this virtual method.
    /// </remarks>
    //*************************************************************************

    protected abstract String
    GetDialogTitle();

    //*************************************************************************
    //  Method: GetFilter()
    //
    /// <summary>
    /// Returns the filter to use for the dialog.
    /// </summary>
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
    GetFilter();

    //*************************************************************************
    //  Method: OpenObject()
    //
    /// <summary>
    /// Create an object from a file.
    /// </summary>
    ///
    /// <param name="sFileName">
    /// File name to open.
    /// </param>
    ///
    /// <param name="oObject">
    /// Where the new object gets stored.
    /// </param>
    ///
    /// <remarks>
    /// Derived classes must implement this virtual method.  Any exceptions
    /// thrown by the derived method are caught and handled by this base class.
    /// </remarks>
    //*************************************************************************

    protected abstract void
    OpenObject
    (
        String sFileName,
        out Object oObject
    );

    //*************************************************************************
    //  Method: OnOpenError()
    //
    /// <summary>
    /// Gets called when an error occurs while opening a file.
    /// </summary>
    ///
    /// <param name="sErrorDescription">
    /// Open error description.
    /// </param>
    //*************************************************************************

    protected void
    OnOpenError
    (
        String sErrorDescription
    )
    {
        MessageBox.Show(sErrorDescription, m_oOpenFileDialog.Title,
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
        Debug.Assert(m_oOpenFileDialog != null);
    }


    //*************************************************************************
    //  Protected member data
    //*************************************************************************

    /// Common dialog.

    protected OpenFileDialog m_oOpenFileDialog;
}

}
