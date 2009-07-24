

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Adapters;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: OpenUcinetFileDialog
//
/// <summary>
/// Represents a dialog box for opening a UCINET file.
/// </summary>
///
/// <remarks>
/// Call <see cref="Form.ShowDialog()" /> to allow the user to open a UCINET
/// file from a location of his choice.  If <see cref="Form.ShowDialog()" />
/// returns DialogResult.OK, the resulting graph can be retrieved from the <see
/// cref="Graph" /> property.
/// </remarks>
//*****************************************************************************

public partial class OpenUcinetFileDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: OpenUcinetFileDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenUcinetFileDialog" />
    /// class.
    /// </summary>
    //*************************************************************************

    public OpenUcinetFileDialog()
    {
        InitializeComponent();

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oOpenUcinetFileDialogUserSettings =
            new OpenUcinetFileDialogUserSettings(this);

        m_oOpenFileDialog = null;
        m_oGraph = null;

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Property: Graph
    //
    /// <summary>
    /// Gets the graph opened from a UCINET file.
    /// </summary>
    ///
    /// <value>
    /// This should be called only after <see cref="Form.ShowDialog()" />
    /// returns DialogResult.OK.
    /// </value>
    //*************************************************************************

    public IGraph
    Graph
    {
        get
        {
            Debug.Assert(this.DialogResult == DialogResult.OK);
            AssertValid();

            return (m_oGraph);
        }
    }

    //*************************************************************************
    //  Method: DoDataExchange()
    //
    /// <summary>
    /// Transfers data between the dialog's fields and its controls.
    /// </summary>
    ///
    /// <param name="bFromControls">
    /// true to transfer data from the dialog's controls to its fields, false
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
            String sFileName;

            if ( !ValidateFileTextBox(txbFileName,

                "Enter or browse for the name of a UCINET full matrix DL"
                    + " file.",

                out sFileName) )
            {
                return (false);
            }

            m_oOpenUcinetFileDialogUserSettings.FileName = sFileName;

            m_oOpenUcinetFileDialogUserSettings.FileDirectedness =
                radDirected.Checked ?
                GraphDirectedness.Directed : GraphDirectedness.Undirected;
        }
        else
        {
            txbFileName.Text = m_oOpenUcinetFileDialogUserSettings.FileName;

            radDirected.Checked =
                (m_oOpenUcinetFileDialogUserSettings.FileDirectedness
                == GraphDirectedness.Directed);

            radUndirected.Checked = !radDirected.Checked;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: OpenUcinetFile()
    //
    /// <summary>
    /// Opens a UCINET file.
    /// </summary>
    ///
    /// <remarks>
    /// This should be called only after DoDataExchange(true) returns true.
    /// </remarks>
    //*************************************************************************

    protected void
    OpenUcinetFile()
    {
        AssertValid();

        m_oGraph = ( new UcinetGraphAdapter() ).LoadGraph(
            m_oOpenUcinetFileDialogUserSettings.FileName,
            m_oOpenUcinetFileDialogUserSettings.FileDirectedness);
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
        AssertValid();

        if (m_oOpenFileDialog == null)
        {
            m_oOpenFileDialog = new OpenFileDialog();
            m_oOpenFileDialog.Title = "Browse for UCINET Full Matrix DL File";
            m_oOpenFileDialog.Filter = Filter;
        }

        if (m_oOpenFileDialog.ShowDialog() == DialogResult.OK)
        {
            txbFileName.Text = m_oOpenFileDialog.FileName;
        }
    }

    //*************************************************************************
    //  Method: lnkHelp_LinkClicked()
    //
    /// <summary>
    /// Handles the LinkClicked event on the lnkHelp button.
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
    lnkHelp_LinkClicked
    (
        object sender,
        LinkLabelLinkClickedEventArgs e
    )
    {
        this.ShowInformation(FormatMessage);
    }

    //*************************************************************************
    //  Method: btnOK_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnOK button.
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
    btnOK_Click
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        if ( !DoDataExchange(true) )
        {
            return;
        }

        this.Cursor = Cursors.WaitCursor;
        Exception oExpectedException = null;

        try
        {
            OpenUcinetFile();
        }
        catch (IOException oIOException)
        {
            oExpectedException = oIOException;
        }
        catch (UnauthorizedAccessException oUnauthorizedAccessException)
        {
            oExpectedException = oUnauthorizedAccessException;
        }
        catch (FormatException oFormatException)
        {
            oExpectedException = oFormatException;
        }
        catch (Exception oUnexpectedException)
        {
            ErrorUtil.OnException(oUnexpectedException);
            return;
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }

        if (oExpectedException != null)
        {
            String sMessage = 
                "The file could not be opened.  Details:\n\n"
                + oExpectedException.Message;

            if (oExpectedException is FormatException)
            {
                sMessage +=
                    "\n\nThis does not appear to be a UCINET full matrix DL"
                    + " file.  "
                    + FormatMessage
                    ;
            }

            this.ShowWarning(sMessage);
            return;
        }

        DialogResult = DialogResult.OK;
        this.Close();
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert(m_oOpenUcinetFileDialogUserSettings != null);
        // m_oOpenFileDialog
        // m_oGraph
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Filter to use for this dialog and the SaveUcinetFileDialog.

    public const String Filter =
        "TXT (*.txt)|*.txt|"
        + "DL (*.dl)|*.dl"
        ;


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Message explaining how to convert a UCINET file to the format accepted
    /// by NodeXL.

    protected const String FormatMessage =

        "If you have a UCINET file that is in another format, such as"
        + " nodelist1, rankedlist1, or dataset, use the UCINET program to"
        + " convert the file to a full matrix DL file before importing it into"
        + " NodeXL."
        + "\r\n\r\n"
        + "In version 6.221 of the UCINET program, select the Data,"
        + " Export, DL menu item.  In the \"Export as DL File\" dialog"
        + " box, specify \"Full matrix\" for Output format, \"Present\""
        + " for Diagonal present, and \"No\" for Embed row labels, Embed"
        + " column labels, and Embed matrix labels."
        ;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected OpenUcinetFileDialogUserSettings
        m_oOpenUcinetFileDialogUserSettings;

    /// Open file dialog, or null.

    protected OpenFileDialog m_oOpenFileDialog;

    /// The graph created from the UCINET file, or null.

    protected IGraph m_oGraph;
}


//*****************************************************************************
//  Class: OpenUcinetFileDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="OpenUcinetFileDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("OpenUcinetFileDialog") ]

public class OpenUcinetFileDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: OpenUcinetFileDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="OpenUcinetFileDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public OpenUcinetFileDialogUserSettings
    (
        Form oForm
    )
    : base (oForm, true)
    {
        Debug.Assert(oForm != null);

        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: FileName
    //
    /// <summary>
    /// Gets or sets the name of the UCINET file to open.
    /// </summary>
    ///
    /// <value>
    /// The name of the UCINET file to open.  The default value is an empty
    /// string.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    FileName
    {
        get
        {
            AssertValid();

            return ( (String)this[FileNameKey] );
        }

        set
        {
            this[FileNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FileDirectedness
    //
    /// <summary>
    /// Gets or sets the graph directedness of the UCINET file.
    /// </summary>
    ///
    /// <value>
    /// The graph directedness of the UCINET file, as a GraphDirectedness.  The
    /// default value is GraphDirectedness.Directed.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("Directed") ]

    public GraphDirectedness
    FileDirectedness
    {
        get
        {
            AssertValid();

            return ( (GraphDirectedness)this[FileDirectednessKey] );
        }

        set
        {
            this[FileDirectednessKey] = value;

            AssertValid();
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Name of the settings key for the FileName property.

    protected const String FileNameKey =
        "FileName";

    /// Name of the settings key for the FileDirectedness property.

    protected const String FileDirectednessKey =
        "FileDirectedness";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
