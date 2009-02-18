

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: LayoutUserSettingsDialog
//
/// <summary>
/// Edits a <see cref="LayoutUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="LayoutUserSettings" /> object to the constructor.  If the
/// user edits the object, <see cref="Form.ShowDialog()" /> returns
/// DialogResult.OK.  Otherwise, the object is not modified and <see
/// cref="Form.ShowDialog()" /> returns DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class LayoutUserSettingsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: LayoutUserSettingsDialog()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="LayoutUserSettingsDialog" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="LayoutUserSettingsDialog" /> class with a LayoutUserSettings
    /// object.
    /// </summary>
    ///
    /// <param name="layoutUserSettings">
    /// The object being edited.
    /// </param>
    //*************************************************************************

    public LayoutUserSettingsDialog
    (
        LayoutUserSettings layoutUserSettings
    )
    : this()
    {
        Debug.Assert(layoutUserSettings != null);
        layoutUserSettings.AssertValid();

        m_oLayoutUserSettings = layoutUserSettings;

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oLayoutUserSettingsDialogUserSettings =
            new LayoutUserSettingsDialogUserSettings(this);

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: LayoutUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="LayoutUserSettingsDialog" /> class for the Visual Studio
    /// designer.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for use by the Visual Studio
    /// designer only.
    /// </remarks>
    //*************************************************************************

    public LayoutUserSettingsDialog()
    {
        InitializeComponent();

        // AssertValid();
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
            Int32 iMargin, iFruchtermanReingoldIterations;
            Single fFruchtermanReingoldC;

            if (
                !ValidateNumericUpDown(nudMargin,
                    "a graph margin", out iMargin)
                ||
                !ValidateNumericUpDown(nudFruchtermanReingoldC,
                    "a strength", out fFruchtermanReingoldC)
                ||
                !ValidateNumericUpDown(nudFruchtermanReingoldIterations,
                    "an iteration count", out iFruchtermanReingoldIterations)
                )
            {
                return (false);
            }

            m_oLayoutUserSettings.Margin = iMargin;

            m_oLayoutUserSettings.FruchtermanReingoldC = fFruchtermanReingoldC;

            m_oLayoutUserSettings.FruchtermanReingoldIterations =
                iFruchtermanReingoldIterations;
        }
        else
        {
            nudMargin.Value = m_oLayoutUserSettings.Margin;

            nudFruchtermanReingoldC.Value = (Decimal)
                m_oLayoutUserSettings.FruchtermanReingoldC;

            nudFruchtermanReingoldIterations.Value =
                m_oLayoutUserSettings.FruchtermanReingoldIterations;
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

    public override void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert(m_oLayoutUserSettings != null);
        Debug.Assert(m_oLayoutUserSettingsDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object whose properties are being edited.

    protected LayoutUserSettings m_oLayoutUserSettings;

    /// User settings for this dialog.

    protected LayoutUserSettingsDialogUserSettings
        m_oLayoutUserSettingsDialogUserSettings;
}


//*****************************************************************************
//  Class: LayoutUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="LayoutUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("LayoutUserSettingsDialog") ]

public class LayoutUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: LayoutUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="LayoutUserSettingsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public LayoutUserSettingsDialogUserSettings
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
