

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ImportFromMatrixWorkbookDialog
//
/// <summary>
/// Imports a graph from an open matrix workbook to the edge worksheet.
/// </summary>
///
/// <remarks>
/// All the importation work is done by this dialog.  The caller need only pass
/// a workbook to the constructor and call <see cref="Form.ShowDialog()" />.
/// If <see cref="Form.ShowDialog()" /> returns DialogResult.OK, the caller can
/// read the <see cref="GraphDirectedness" /> property to determine the
/// directedness of the imported graph.
///
/// <para>
/// See <see cref="MatrixWorkbookImporter" /> for details on the expected
/// format of the matrix workbook.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class ImportFromMatrixWorkbookDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: ImportFromMatrixWorkbookDialog()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="ImportFromMatrixWorkbookDialog" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ImportFromMatrixWorkbookDialog" /> class with a workbook.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    //*************************************************************************

    public ImportFromMatrixWorkbookDialog
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    : this()
    {
        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oImportFromMatrixWorkbookDialogUserSettings =
            new ImportFromMatrixWorkbookDialogUserSettings(this);

        m_oWorkbook = workbook;

        lbxSourceWorkbook.PopulateWithOtherWorkbookNames(m_oWorkbook);

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: ImportFromMatrixWorkbookDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ImportFromMatrixWorkbookDialog" /> class for the Visual Studio
    /// designer.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for use by the Visual Studio
    /// designer only.
    /// </remarks>
    //*************************************************************************

    public ImportFromMatrixWorkbookDialog()
    {
        InitializeComponent();

        // AssertValid();
    }

    //*************************************************************************
    //  Property: SourceWorkbookDirectedness
    //
    /// <summary>
    /// Gets the graph directedness of the imported graph.
    /// </summary>
    ///
    /// <value>
    /// The graph directedness of the imported graph, as a GraphDirectedness.
    /// </value>
    ///
    /// <remarks>
    /// Read this property after <see cref="Form.ShowDialog()" /> returns
    /// DialogResultOK to determine the directedness of the imported graph.
    /// </remarks>
    //*************************************************************************

    public GraphDirectedness
    SourceWorkbookDirectedness
    {
        get
        {
            Debug.Assert(this.DialogResult == DialogResult.OK);
            AssertValid();

            return (m_oImportFromMatrixWorkbookDialogUserSettings.
                SourceWorkbookDirectedness);
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
        if (bFromControls)
        {
            m_oImportFromMatrixWorkbookDialogUserSettings.
                SourceWorkbookHasVertexNames =
                radSourceWorkbookHasVertexNames.Checked;

            m_oImportFromMatrixWorkbookDialogUserSettings.
                SourceWorkbookDirectedness = radDirected.Checked ?
                GraphDirectedness.Directed : GraphDirectedness.Undirected;
        }
        else
        {
            radAssignVertexNames.Checked =
                !(radSourceWorkbookHasVertexNames.Checked =
                m_oImportFromMatrixWorkbookDialogUserSettings.
                    SourceWorkbookHasVertexNames);

            radDirected.Checked =
                (m_oImportFromMatrixWorkbookDialogUserSettings.
                SourceWorkbookDirectedness == GraphDirectedness.Directed);

            radUndirected.Checked = !radDirected.Checked;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: ImportFromMatrixWorkbook()
    //
    /// <summary>
    /// Imports a graph from a source matrix workbook to the edge worksheet.
    /// </summary>
    ///
    /// <remarks>
    /// This should be called only after DoDataExchange(true) returns true.
    /// </remarks>
    //*************************************************************************

    protected void
    ImportFromMatrixWorkbook()
    {
        AssertValid();

        MatrixWorkbookImporter oMatrixWorkbookImporter =
            new MatrixWorkbookImporter();

        oMatrixWorkbookImporter.ImportMatrixWorkbook(
            (String)lbxSourceWorkbook.SelectedItem,

            m_oImportFromMatrixWorkbookDialogUserSettings.
                SourceWorkbookHasVertexNames,

            m_oImportFromMatrixWorkbookDialogUserSettings.
                SourceWorkbookDirectedness,

            m_oWorkbook);
    }

    //*************************************************************************
    //  Method: OnLoad()
    //
    /// <summary>
    /// Handles the Load event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected override void
    OnLoad
    (
        EventArgs e
    )
    {
        AssertValid();

        base.OnLoad(e);

        if (lbxSourceWorkbook.Items.Count == 0)
        {
            this.ShowWarning(ExcelWorkbookListBox.NoOtherWorkbooks);
            this.Close();
        }
        else
        {
            lbxSourceWorkbook.SelectedIndex = 0;
        }
    }

    //*************************************************************************
    //  Method: lnkMatrixWorkbookSamples_Clicked()
    //
    /// <summary>
    /// Handles the Clicked event on the lnkMatrixWorkbookSamples LinkLabel.
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
    lnkMatrixWorkbookSamples_LinkClicked
    (
        object sender,
        LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        MatrixWorkbookSamplesDialog oMatrixWorkbookSamplesDialog =
            new MatrixWorkbookSamplesDialog();

        oMatrixWorkbookSamplesDialog.ShowDialog();
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
        if ( !DoDataExchange(true) )
        {
            return;
        }

        this.Cursor = Cursors.WaitCursor;

        try
        {
            ImportFromMatrixWorkbook();
        }
        catch (ImportWorkbookException oImportWorkbookException)
        {
            this.ShowWarning(oImportWorkbookException.Message);
            return;
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
            return;
        }
        finally
        {
            this.Cursor = Cursors.Default;
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

        Debug.Assert(m_oWorkbook != null);
        Debug.Assert(m_oImportFromMatrixWorkbookDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected ImportFromMatrixWorkbookDialogUserSettings
        m_oImportFromMatrixWorkbookDialogUserSettings;

    /// Workbook containing the graph data.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;
}


//*****************************************************************************
//  Class: ImportFromMatrixWorkbookDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="ImportFromMatrixWorkbookDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("ImportFromMatrixWorkbookDialog") ]

public class ImportFromMatrixWorkbookDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: ImportFromMatrixWorkbookDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ImportFromMatrixWorkbookDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public ImportFromMatrixWorkbookDialogUserSettings
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
    //  Property: SourceWorkbookHasVertexNames
    //
    /// <summary>
    /// Gets or sets a flag that indicating whether the source workbook has
    /// vertex names in row 1 and column A.
    /// 
    /// </summary>
    ///
    /// <value>
    /// true if the source workbook has vertex names.  The default is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    SourceWorkbookHasVertexNames
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[SourceWorkbookHasVertexNamesKey] );
        }

        set
        {
            this[SourceWorkbookHasVertexNamesKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SourceWorkbookDirectedness
    //
    /// <summary>
    /// Gets or sets the graph directedness of the source workbook.
    /// </summary>
    ///
    /// <value>
    /// The graph directedness of the source workbook, as a GraphDirectedness.
    /// The default value is GraphDirectedness.Directed.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("Directed") ]

    public GraphDirectedness
    SourceWorkbookDirectedness
    {
        get
        {
            AssertValid();

            return ( (GraphDirectedness)
                this[SourceWorkbookDirectednessKey] );
        }

        set
        {
            this[SourceWorkbookDirectednessKey] = value;

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

    /// Name of the settings key for the SourceWorkbookHasVertexNames property.

    protected const String SourceWorkbookHasVertexNamesKey =
        "SourceWorkbookHasVertexNames";

    /// Name of the settings key for the SourceWorkbookDirectedness property.

    protected const String SourceWorkbookDirectednessKey =
        "SourceWorkbookDirectedness";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
