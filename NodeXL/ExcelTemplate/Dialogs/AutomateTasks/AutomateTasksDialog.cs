

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AutomateTasksDialog
//
/// <summary>
/// Edits a <see cref="AutomateTasksUserSettings" /> object and runs the tasks
/// specified in the object.
/// </summary>
///
/// <remarks>
/// Unlike other dialogs in this application, you do not pass a user settings
/// object to the constructor.  That's because when automating a folder, this
/// class must update the user settings on disk before opening other workbooks,
/// so those other workbooks will have access to the updated settings.
///
/// <para>
/// The <see cref="Form.ShowDialog()" /> return value does indicate whether the
/// settings were updated: it returns DialogResult.OK if they were edited, or
/// DialogResult.Cancel if they were not.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class AutomateTasksDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: AutomateTasksDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="AutomateTasksDialog" />
    /// class.
    /// </summary>
    ///
    /// <param name="thisWorkbook">
    /// Workbook containing the graph contents.
    /// </param>
    ///
    /// <param name="ribbon">
    /// The application's Ribbon.
    /// </param>
    //*************************************************************************

    public AutomateTasksDialog
    (
        ThisWorkbook thisWorkbook,
        Ribbon ribbon
    )
    {
        Debug.Assert(thisWorkbook != null);
        Debug.Assert(ribbon != null);

        m_oAutomateTasksUserSettings = new AutomateTasksUserSettings();
        m_oThisWorkbook = thisWorkbook;
        m_oRibbon = ribbon;

        InitializeComponent();

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oAutomateTasksDialogUserSettings =
            new AutomateTasksDialogUserSettings(this);

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
        AssertValid();

        if (bFromControls)
        {
            AutomationTasks eTasksToRun = AutomationTasks.None;

            // An AutomationTasks flag is stored in the Tag of each CheckBox.

            foreach ( CheckBox oCheckBox in GetAutomationTaskCheckBoxes() )
            {
                if (oCheckBox.Checked)
                {
                    eTasksToRun |= (AutomationTasks)oCheckBox.Tag;
                }
            }

            if (eTasksToRun == AutomationTasks.None)
            {
                this.ShowWarning("No tasks have been selected.");
                return (false);
            }

            Boolean bAutomateThisWorkbookOnly =
                radAutomateThisWorkbookOnly.Checked;

            if ( (eTasksToRun & AutomationTasks.SaveGraphImageFile) != 0 )
            {
                if ( (eTasksToRun & AutomationTasks.ReadWorkbook) == 0 )
                {
                    this.ShowWarning(
                        "You can't save an image to a file without first"
                        + " showing the graph.  Either check \"Show workbook\""
                        + " or uncheck \"Save image to file.\""
                        );

                    return (false);
                }

                if ( bAutomateThisWorkbookOnly &&
                    String.IsNullOrEmpty(m_oThisWorkbook.Path) )
                {
                    this.ShowWarning(TaskAutomator.WorkbookNotSavedMessage);
                    return (false);
                }
            }

            if (!bAutomateThisWorkbookOnly)
            {
                if ( !usrFolderToAutomate.Validate() )
                {
                    return (false);
                }
            }

            m_oAutomateTasksUserSettings.TasksToRun = eTasksToRun;

            m_oAutomateTasksUserSettings.AutomateThisWorkbookOnly =
                bAutomateThisWorkbookOnly;

            m_oAutomateTasksUserSettings.FolderToAutomate =
                usrFolderToAutomate.FolderPath;
        }
        else
        {
            AutomationTasks eTasksToRun =
                m_oAutomateTasksUserSettings.TasksToRun;

            foreach ( CheckBox oCheckBox in GetAutomationTaskCheckBoxes() )
            {
                oCheckBox.Checked =
                    (eTasksToRun & (AutomationTasks)oCheckBox.Tag) != 0;
            }

            if (m_oAutomateTasksUserSettings.AutomateThisWorkbookOnly)
            {
                radAutomateThisWorkbookOnly.Checked = true;
            }
            else
            {
                radAutomateFolder.Checked = true;
            }

            usrFolderToAutomate.FolderPath =
                m_oAutomateTasksUserSettings.FolderToAutomate;

            EnableControls();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: EnableControls()
    //
    /// <summary>
    /// Enables or disables the dialog's controls.
    /// </summary>
    //*************************************************************************

    protected void
    EnableControls()
    {
        AssertValid();

        EnableControls(radAutomateFolder.Checked,
            usrFolderToAutomate, lblNote);
    }

    //*************************************************************************
    //  Method: GetAutomationTaskCheckBoxes()
    //
    /// <summary>
    /// Gets an enumerator for enumerating the CheckBox controls that represent
    /// automation tasks.
    /// </summary>
    ///
    /// <returns>
    /// An enumerator for enumerating the CheckBox controls.
    /// </returns>
    //*************************************************************************

    protected IEnumerable<CheckBox>
    GetAutomationTaskCheckBoxes()
    {
        AssertValid();

        foreach (Control oControl in grpTasksToRun.Controls)
        {
            if (oControl is CheckBox && oControl.Tag is AutomationTasks)
            {
                yield return ( (CheckBox)oControl );
            }
        }
    }

    //*************************************************************************
    //  Method: OnEventThatRequiresControlEnabling()
    //
    /// <summary>
    /// Handles any event that should changed the enabled state of the dialog's
    /// controls.
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
    OnEventThatRequiresControlEnabling
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        EnableControls();
    }

    //*************************************************************************
    //  Method: btnCheckAll_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnCheckAll button.
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
    btnCheckAll_Click
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        CheckAllCheckBoxes(grpTasksToRun, true);
    }

    //*************************************************************************
    //  Method: btnUncheckAll_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnUncheckAll button.
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
    btnUncheckAll_Click
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        CheckAllCheckBoxes(grpTasksToRun, false);
    }

    //*************************************************************************
    //  Method: btnGraphMetricUserSettings_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnGraphMetricUserSettings button.
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
    btnGraphMetricUserSettings_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        m_oRibbon.OnShowGraphMetricsClick(
            GraphMetricsDialog.DialogMode.EditOnly);
    }

    //*************************************************************************
    //  Method: btnAutoFillUserSettings_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnAutoFillUserSettings button.
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
    btnAutoFillUserSettings_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        m_oRibbon.OnAutoFillWorkbookClick(
            AutoFillWorkbookDialog.DialogMode.EditOnly);
    }

    //*************************************************************************
    //  Method: btnCreateSubgraphImagesUserSettings_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnCreateSubgraphImagesUserSettings
    /// button.
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
    btnCreateSubgraphImagesUserSettings_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        m_oRibbon.OnCreateSubgraphImagesClick(
            CreateSubgraphImagesDialog.DialogMode.EditOnly);
    }

    //*************************************************************************
    //  Method: btnGeneralUserSettings_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnGeneralUserSettings button.
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
    btnGeneralUserSettings_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        // The GeneralUserSettings are normally edited from the TaskPane, not
        // the Ribbon, so this event handler differs from the others.

        GeneralUserSettings oGeneralUserSettings = new GeneralUserSettings();

        GeneralUserSettingsDialog oGeneralUserSettingsDialog =
            new GeneralUserSettingsDialog(oGeneralUserSettings,
            m_oThisWorkbook.InnerObject);

        if (oGeneralUserSettingsDialog.ShowDialog() == DialogResult.OK)
        {
            oGeneralUserSettings.Save();
        }
    }

    //*************************************************************************
    //  Method: btnSaveGraphImageFile_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnSaveGraphImageFile button.
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
    btnSaveGraphImageFile_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        AutomatedGraphImageUserSettings oAutomatedGraphImageUserSettings =
            new AutomatedGraphImageUserSettings();

        AutomatedGraphImageUserSettingsDialog
            oAutomatedGraphImageUserSettingsDialog =
            new AutomatedGraphImageUserSettingsDialog(
                oAutomatedGraphImageUserSettings);

        if (oAutomatedGraphImageUserSettingsDialog.ShowDialog() ==
            DialogResult.OK)
        {
            oAutomatedGraphImageUserSettings.Save();
        }
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

        m_oAutomateTasksUserSettings.Save();

        try
        {
            if (m_oAutomateTasksUserSettings.AutomateThisWorkbookOnly)
            {
                TaskAutomator.AutomateThisWorkbook(m_oThisWorkbook,
                    m_oAutomateTasksUserSettings.TasksToRun, m_oRibbon);
            }
            else
            {
                TaskAutomator.AutomateFolder(
                    m_oAutomateTasksUserSettings.FolderToAutomate,
                    m_oAutomateTasksUserSettings.TasksToRun,
                    m_oThisWorkbook.Application);
            }
        }
        catch (UnauthorizedAccessException oUnauthorizedAccessException)
        {
            // This occurs when a workbook is read-only.

            this.ShowWarning(
                "A problem occurred while running tasks.  Details:"
                + "\r\n\r\n"
                + oUnauthorizedAccessException.Message
                );

            return;
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
            return;
        }

        this.DialogResult = DialogResult.OK;
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

        Debug.Assert(m_oAutomateTasksUserSettings != null);
        Debug.Assert(m_oThisWorkbook != null);
        Debug.Assert(m_oRibbon != null);
        Debug.Assert(m_oAutomateTasksDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object whose properties are being edited.

    protected AutomateTasksUserSettings m_oAutomateTasksUserSettings;

    /// Workbook containing the graph contents.

    protected ThisWorkbook m_oThisWorkbook;

    /// The application's Ribbon.

    protected Ribbon m_oRibbon;

    /// User settings for this dialog.

    protected AutomateTasksDialogUserSettings
        m_oAutomateTasksDialogUserSettings;
}


//*****************************************************************************
//  Class: AutomateTasksDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="AutomateTasksDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("AutomateTasksDialog2") ]

public class AutomateTasksDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: AutomateTasksDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutomateTasksDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public AutomateTasksDialogUserSettings
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
