
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AutomateTasksUserSettings
//
/// <summary>
/// Stores the user's settings for task automation.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("AutomateTasksUserSettings") ]

public class AutomateTasksUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: AutomateTasksUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutomateTasksUserSettings" /> class.
    /// </summary>
    //*************************************************************************

    public AutomateTasksUserSettings()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: TasksToRun
    //
    /// <summary>
    /// Gets or sets the tasks to run.
    /// </summary>
    ///
    /// <value>
    /// The tasks to run, as an <see cref="AutomationTasks" />.  The default
    /// value is AutomationTasks.ReadWorkbook.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("ReadWorkbook") ]

    public AutomationTasks
    TasksToRun
    {
        get
        {
            AssertValid();

            return ( (AutomationTasks)this[TasksToRunKey] );
        }

        set
        {
            this[TasksToRunKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: AutomateThisWorkbookOnly
    //
    /// <summary>
    /// Gets or sets a flag indicating which workbook(s) to automate.
    /// </summary>
    ///
    /// <value>
    /// If true, only the current workbook is automated.  If false, all
    /// unopened NodeXL workbooks in the folder specified by <see
    /// cref="FolderToAutomate" /> are automated.  The default value is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    AutomateThisWorkbookOnly
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[AutomateThisWorkbookOnlyKey] );
        }

        set
        {
            this[AutomateThisWorkbookOnlyKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FolderToAutomate
    //
    /// <summary>
    /// Gets or sets the path to the folder to automate.
    /// </summary>
    ///
    /// <value>
    /// If <see cref="AutomateThisWorkbookOnly" /> is false, all unopened
    /// NodeXL workbooks in this folder are automated.  If <see
    /// cref="AutomateThisWorkbookOnly" /> is true, this property is ignored.
    /// The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    FolderToAutomate
    {
        get
        {
            AssertValid();

            return ( (String)this[FolderToAutomateKey] );
        }

        set
        {
            this[FolderToAutomateKey] = value;

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

    /// Name of the settings key for the TasksToRun property.

    protected const String TasksToRunKey =
        "TasksToRun";

    /// Name of the settings key for the AutomateThisWorkbookOnly property.

    protected const String AutomateThisWorkbookOnlyKey =
        "AutomateThisWorkbookOnly";

    /// Name of the settings key for the FolderToAutomate property.

    protected const String FolderToAutomateKey =
        "FolderToAutomate";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
