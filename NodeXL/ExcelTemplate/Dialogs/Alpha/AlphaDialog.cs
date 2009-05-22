

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AlphaDialog
//
/// <summary>
/// Gets an alpha value from the user.
/// </summary>
///
/// <remarks>
/// If <see cref="Form.ShowDialog()" /> returns DialogResult.OK, read the <see
/// cref="Alpha" /> property to get the alpha value specified by the user.
/// </remarks>
//*****************************************************************************

public partial class AlphaDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: AlphaDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="AlphaDialog" /> class.
    /// </summary>
    //*************************************************************************

    public AlphaDialog()
    {
        InitializeComponent();

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oAlphaDialogUserSettings = new AlphaDialogUserSettings(this);

        nudAlpha.Minimum = (Decimal)AlphaConverter.MinimumAlphaWorkbook;
        nudAlpha.Maximum = (Decimal)AlphaConverter.MaximumAlphaWorkbook;

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Property: Alpha
    //
    /// <summary>
    /// Gets the alpha value specified by the user.
    /// </summary>
    ///
    /// <value>
    /// The alpha value specified by the user.  This is between
    /// AlphaConverter.MinimumAlphaWorkbook and
    /// AlphaConverter.MaximumAlphaWorkbook.
    /// </value>
    ///
    /// <remarks>
    /// This should be read only if <see cref="Form.ShowDialog()" /> returned
    /// DialogResult.OK.
    /// </remarks>
    //*************************************************************************

    public Single
    Alpha
    {
        get
        {
            Debug.Assert(this.DialogResult == DialogResult.OK);
            AssertValid();

            return (m_oAlphaDialogUserSettings.Alpha);
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
            Single fAlpha;

            if ( !ValidateNumericUpDown(nudAlpha, "an opacity", out fAlpha) )
            {
                return (false);
            }

            m_oAlphaDialogUserSettings.Alpha = fAlpha;

        }
        else
        {
            nudAlpha.Value = (Decimal)m_oAlphaDialogUserSettings.Alpha;
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
            this.DialogResult = DialogResult.OK;
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

        Debug.Assert(m_oAlphaDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected AlphaDialogUserSettings m_oAlphaDialogUserSettings;
}


//*****************************************************************************
//  Class: AlphaDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="AlphaDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("AlphaDialog") ]

public class AlphaDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: AlphaDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AlphaDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public AlphaDialogUserSettings
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
    //  Property: Alpha
    //
    /// <summary>
    /// Gets or sets the alpha value.
    /// </summary>
    ///
    /// <value>
    /// The alpha value.  Must be between AlphaConverter.MinimumAlphaWorkbook
    /// and AlphaConverter.MaximumAlphaWorkbook.  The default value is 100.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("100") ]

    public Single
    Alpha
    {
        get
        {
            AssertValid();

            return ( (Single)this[AlphaKey] );
        }

        set
        {
            this[AlphaKey] = value;

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

    /// Name of the settings key for the Alpha property.

    protected const String AlphaKey =
        "Alpha";
}
}
