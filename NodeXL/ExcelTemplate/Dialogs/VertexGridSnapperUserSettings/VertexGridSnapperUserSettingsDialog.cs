
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexGridSnapperUserSettingsDialog
//
/// <summary>
/// Edits a <see cref="VertexGridSnapperUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="VertexGridSnapperUserSettings" /> object to the
/// constructor.  If the user edits the object, <see
/// cref="Form.ShowDialog()" /> returns DialogResult.OK.  Otherwise, the object
/// is not modified and <see cref="Form.ShowDialog()" /> returns
/// DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class VertexGridSnapperUserSettingsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: VertexGridSnapperUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VertexGridSnapperUserSettingsDialog" /> class.
    /// </summary>
    ///
    /// <param name="vertexGridSnapperUserSettings">
    /// The object being edited.
    /// </param>
    //*************************************************************************

    public VertexGridSnapperUserSettingsDialog
    (
        VertexGridSnapperUserSettings vertexGridSnapperUserSettings
    )
    {
        Debug.Assert(vertexGridSnapperUserSettings != null);
        vertexGridSnapperUserSettings.AssertValid();

        m_oVertexGridSnapperUserSettings = vertexGridSnapperUserSettings;

        // Instantiate an object that saves and retrieves the position of this
        // dialog.  Note that the object automatically saves the settings when
        // the form closes.

        m_oVertexGridSnapperUserSettingsDialogUserSettings =
            new VertexGridSnapperUserSettingsDialogUserSettings(this);

        InitializeComponent();
        DoDataExchange(false);

        AssertValid();
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
            Int32 iGridSize;

            if ( !ValidateNumericUpDown(nudGridSize, "a grid size",
                out iGridSize) )
            {
                return (false);
            }

            m_oVertexGridSnapperUserSettings.GridSize = iGridSize;
        }
        else
        {
            nudGridSize.Value =
                (Decimal)m_oVertexGridSnapperUserSettings.GridSize;
        }

        return (true);
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
        if ( DoDataExchange(true) )
        {
            DialogResult = DialogResult.OK;
            this.Close();
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

    public  override void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert(m_oVertexGridSnapperUserSettings != null);

        Debug.Assert(m_oVertexGridSnapperUserSettingsDialogUserSettings !=
            null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object whose properties are being edited.

    protected VertexGridSnapperUserSettings m_oVertexGridSnapperUserSettings;

    /// User settings for this dialog.

    protected VertexGridSnapperUserSettingsDialogUserSettings
        m_oVertexGridSnapperUserSettingsDialogUserSettings;
}


//*****************************************************************************
//  Class: VertexGridSnapperUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="VertexGridSnapperUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("VertexGridSnapperUserSettingsDialog") ]

public class VertexGridSnapperUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: VertexGridSnapperUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VertexGridSnapperUserSettingsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public VertexGridSnapperUserSettingsDialogUserSettings
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
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
