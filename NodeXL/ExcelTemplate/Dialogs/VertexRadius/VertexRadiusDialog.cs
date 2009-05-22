

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexRadiusDialog
//
/// <summary>
/// Gets a vertex radius from the user.
/// </summary>
///
/// <remarks>
/// If <see cref="Form.ShowDialog()" /> returns DialogResult.OK, read the <see
/// cref="VertexRadius" /> property to get the vertex radius specified by the
/// user.
/// </remarks>
//*****************************************************************************

public partial class VertexRadiusDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: VertexRadiusDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexRadiusDialog" />
    /// class.
    /// </summary>
    //*************************************************************************

    public VertexRadiusDialog()
    {
        InitializeComponent();

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oVertexRadiusDialogUserSettings =
            new VertexRadiusDialogUserSettings(this);

        nudVertexRadius.Minimum =
            (Decimal)VertexRadiusConverter.MinimumRadiusWorkbook;

        nudVertexRadius.Maximum = 
            (Decimal)VertexRadiusConverter.MaximumRadiusWorkbook;

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Property: VertexRadius
    //
    /// <summary>
    /// Gets the vertex radius specified by the user.
    /// </summary>
    ///
    /// <value>
    /// The vertex radius specified by the user.  This is between
    /// VertexRadiusConverter.MinimumRadiusWorkbook and
    /// VertexRadiusConverter.MaximumRadiusWorkbook.
    /// </value>
    ///
    /// <remarks>
    /// This should be read only if <see cref="Form.ShowDialog()" /> returned
    /// DialogResult.OK.
    /// </remarks>
    //*************************************************************************

    public Single
    VertexRadius
    {
        get
        {
            Debug.Assert(this.DialogResult == DialogResult.OK);
            AssertValid();

            return (m_oVertexRadiusDialogUserSettings.VertexRadius);
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
            Single fVertexRadius;

            if (!ValidateNumericUpDown(nudVertexRadius, "a vertex size",
                out fVertexRadius) )
            {
                return (false);
            }

            m_oVertexRadiusDialogUserSettings.VertexRadius = fVertexRadius;

        }
        else
        {
            nudVertexRadius.Value =
                (Decimal)m_oVertexRadiusDialogUserSettings.VertexRadius;
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

        Debug.Assert(m_oVertexRadiusDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected VertexRadiusDialogUserSettings m_oVertexRadiusDialogUserSettings;
}


//*****************************************************************************
//  Class: VertexRadiusDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="VertexRadiusDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("VertexRadiusDialog") ]

public class VertexRadiusDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: VertexRadiusDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VertexRadiusDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public VertexRadiusDialogUserSettings
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
    //  Property: VertexRadius
    //
    /// <summary>
    /// Gets or sets the vertex radius.
    /// </summary>
    ///
    /// <value>
    /// The vertex radius.  Must be between
    /// VertexRadiusConverter.MinimumVertexRadiusWorkbook and
    /// VertexRadiusConverter.MaximumVertexRadiusWorkbook.  The default value
    /// is 1.5.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("1.5") ]

    public Single
    VertexRadius
    {
        get
        {
            AssertValid();

            return ( (Single)this[VertexRadiusKey] );
        }

        set
        {
            this[VertexRadiusKey] = value;

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

    /// Name of the settings key for the VertexRadius property.

    protected const String VertexRadiusKey =
        "VertexRadius";
}
}
