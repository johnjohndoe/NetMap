

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: MatrixWorkbookSamplesDialog
//
/// <summary>
/// Shows the user what a matrix workbook looks like.
/// </summary>
///
/// <remarks>
/// No editing is done within the dialog, which just displays information.
/// Call <see cref="Form.ShowDialog()" /> to show the dialog.
/// </remarks>
//*****************************************************************************

public partial class MatrixWorkbookSamplesDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: MatrixWorkbookSamplesDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="MatrixWorkbookSamplesDialog" /> class.
    /// </summary>
    //*************************************************************************

    public MatrixWorkbookSamplesDialog()
    {
        InitializeComponent();

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oMatrixWorkbookSamplesDialogUserSettings =
            new MatrixWorkbookSamplesDialogUserSettings(this);

        // AssertValid();
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

        Debug.Assert(m_oMatrixWorkbookSamplesDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected MatrixWorkbookSamplesDialogUserSettings
        m_oMatrixWorkbookSamplesDialogUserSettings;
}


//*****************************************************************************
//  Class: MatrixWorkbookSamplesDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="MatrixWorkbookSamplesDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("MatrixWorkbookSamplesDialog") ]

public class MatrixWorkbookSamplesDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: MatrixWorkbookSamplesDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="MatrixWorkbookSamplesDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public MatrixWorkbookSamplesDialogUserSettings
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
