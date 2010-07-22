
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: FileNameLinkLabel
//
/// <summary>
/// Represents a LinkLabel that starts a specified file when clicked.
/// </summary>
///
/// <remarks>
/// If the <see cref="FileName" /> property is set, clicking the LinkLabel
/// causes <see cref="Process.Start(String)" /> to be called with the file name
/// as an argument.
/// </remarks>
//*****************************************************************************

public class FileNameLinkLabel : LinkLabel
{
    //*************************************************************************
    //  Constructor: FileNameLinkLabel()
    //
    /// <summary>
    /// Initializes a new instance of the FileNameLinkLabel class.
    /// </summary>
    //*************************************************************************

    public FileNameLinkLabel()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Property: FileName
    //
    /// <summary>
    /// Gets or sets the full path to the file to start.
    /// </summary>
    ///
    /// <value>
    /// The full path to the file to start, as a String.  The default value is
    /// null.
    /// </value>
    //*************************************************************************

    public String
    FileName
    {
        get
        {
            AssertValid();

            return (m_sFileName);
        }

        set
        {
            m_sFileName = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: OnLinkClicked()
    //
    /// <summary>
    /// Handles the LinkClicked event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected override void
    OnLinkClicked
    (
        LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        base.OnLinkClicked(e);

        if ( !String.IsNullOrEmpty(m_sFileName) )
        {
            Process.Start(m_sFileName);
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
        // m_sFileName
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Name of the file to start, or null.

    protected String m_sFileName;
}

}
