
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: PictureBoxPlus
//
/// <summary>
/// Represents a PictureBox with additional features.
/// </summary>
//*****************************************************************************

public class PictureBoxPlus : PictureBox
{
    //*************************************************************************
    //  Constructor: PictureBoxPlus()
    //
    /// <summary>
    /// Initializes a new instance of the PictureBoxPlus class.
    /// </summary>
    //*************************************************************************

    public PictureBoxPlus()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: TryPasteEnhancedMetafile()
    //
    /// <summary>
    /// Attempts to paste an enhanced metafile from the clipboard into the
    /// PictureBox.
    /// </summary>
    ///
    /// <returns>
    /// true if an enhanced metafile was pasted into the PictureBox.
    /// </returns>
    ///
    /// <remarks>
    /// If the clipboard contains an enhanced metafile, this method pastes it
    /// into the PictureBox and returns true.  Otherwise, false is returned.
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryPasteEnhancedMetafile()
    {
        // This interop code was adapted from the following post:
        //
        // http://www.eggheadcafe.com/community/aspnet/12/10018506/
        // how-to-paste--metafile-fr.aspx
        //
        // NOTE: Do not try this non-interop technique:
        //
        //     Object oData = Clipboard.GetData(DataFormats.EnhancedMetafile);
        //     
        //     if (oData != null)
        //     {
        //         System.Drawing.Imaging.Metafile oMetafile =
        //             (System.Drawing.Imaging.Metafile)oData;
        //     }
        //
        // When pasting from a chart image copied from Excel, the GetData()
        // call crashes Excel.  Although the Clipboard class reports that the
        // DataFormats.EnhancedMetafile format is available (along with
        // "Preferred DropEffect", "InShellDragLoop", and "MetaFilePict"), it
        // does not seem to be the format that the .NET Framework is expecting.
        // Using the interop technique below works properly, however.

        Boolean bReturn = false;

        if ( OpenClipboard(this.Handle) )
        {
            if ( IsClipboardFormatAvailable(CF_ENHMETAFILE) )
            {
                IntPtr oPtr = GetClipboardData(CF_ENHMETAFILE);

                if ( !oPtr.Equals( new IntPtr(0) ) )
                {
                    Metafile oMetafile = new Metafile(oPtr, true);

                    this.Image = (Metafile)oMetafile.Clone();
                    bReturn = true;
                }
            }

            CloseClipboard();
        }

        return (bReturn);
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
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Interop declarations
    //*************************************************************************

    ///
    [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
    protected static extern bool OpenClipboard(IntPtr hWndNewOwner);
    ///
    [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
    protected static extern bool CloseClipboard();
    ///
    [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
    protected static extern IntPtr GetClipboardData(uint format);
    ///
    [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
    protected static extern bool IsClipboardFormatAvailable(uint format);
    ///
    protected const uint CF_METAFILEPICT = 3;
    ///
    protected const uint CF_ENHMETAFILE = 14;
}

}
