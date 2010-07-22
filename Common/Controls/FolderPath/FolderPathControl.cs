
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: FolderPathControl
//
/// <summary>
/// Control for getting the path to an existing folder.
/// </summary>
///
/// <remarks>
/// Set the <see cref="FolderPath" /> property after the control is created.
/// To retrieve the folder path , call <see cref="Validate" />, and if <see
/// cref="Validate" /> returns true, read the <see cref="FolderPath" />
/// property.
/// </remarks>
///
/// <para>
/// This control uses the following access key: B.
/// </para>
//*****************************************************************************

public partial class FolderPathControl : UserControl
{
    //*************************************************************************
    //  Constructor: FolderPathControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="FolderPathControl" />
    /// class.
    /// </summary>
    //*************************************************************************

    public FolderPathControl()
    {
        InitializeComponent();

        m_sFolderPath = String.Empty;
        m_sBrowsePrompt = "Browse for a folder.";

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Property: FolderPath
    //
    /// <summary>
    /// Gets or sets the folder path.
    /// </summary>
    ///
    /// <value>
    /// The folder path, or String.Empty.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    public String
    FolderPath
    {
        get
        {
            AssertValid();

            return (m_sFolderPath);
        }

        set
        {
            m_sFolderPath = value;
            DoDataExchange(false);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: BrowsePrompt
    //
    /// <summary>
    /// Gets or sets the prompt to use in the folder browsing dialog.
    /// </summary>
    ///
    /// <value>
    /// The prompt.  Can't be null or empty.  The default value is "Browse for
    /// a folder."
    /// </value>
    //*************************************************************************

    public String
    BrowsePrompt
    {
        get
        {
            AssertValid();

            return (m_sBrowsePrompt);
        }

        set
        {
            m_sBrowsePrompt = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: Validate()
    //
    /// <summary>
    /// Validates the user's settings.
    /// </summary>
    ///
    /// <returns>
    /// true if the validation was successful.
    /// </returns>
    ///
    /// <remarks>
    /// If validation fails, an error message is displayed and false is
    /// returned.
    /// </remarks>
    //*************************************************************************

    public new Boolean
    Validate()
    {
        AssertValid();

        return ( DoDataExchange(true) );
    }

    //*************************************************************************
    //  Method: DoDataExchange()
    //
    /// <summary>
    /// Transfers data between the control's fields and its controls.
    /// </summary>
    ///
    /// <param name="bFromControls">
    /// true to transfer data from the control's controls to its fields, false
    /// for the other direction.
    /// </param>
    ///
    /// <returns>
    /// true if the transfer was successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    DoDataExchange
    (
        Boolean bFromControls
    )
    {
        AssertValid();

        if (bFromControls)
        {
            String sFolderPath = String.Empty;

            if ( !FormUtil.ValidateDirectoryTextBox(txbFolderPath,
                "Enter or browse for a folder.", out sFolderPath) )
            {
                return (false);
            }

            m_sFolderPath = sFolderPath;
        }
        else
        {
            txbFolderPath.Text = m_sFolderPath;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: btnBrowse_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnBrowse button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnBrowse_Click
    (
        object sender,
        System.EventArgs e
    )
    {
        // Show the folder browser dialog.

        FolderBrowserDialog oFolderBrowserDialog =
            new FolderBrowserDialog();

        oFolderBrowserDialog.Description = m_sBrowsePrompt;
        oFolderBrowserDialog.SelectedPath = txbFolderPath.Text;

        if (oFolderBrowserDialog.ShowDialog() == DialogResult.OK)
        {
            txbFolderPath.Text = oFolderBrowserDialog.SelectedPath;
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
        Debug.Assert(m_sFolderPath != null);
        Debug.Assert( !String.IsNullOrEmpty(m_sBrowsePrompt) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The folder path, or String.Empty.

    protected String m_sFolderPath;

    /// The prompt to use in the folder browsing dialog.

    protected String m_sBrowsePrompt;
}

}
