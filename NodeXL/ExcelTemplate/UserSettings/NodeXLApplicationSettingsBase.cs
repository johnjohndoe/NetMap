
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.IO;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NodeXLApplicationSettingsBase
//
/// <summary>
/// Base class for NodeXL's user settings classes.
/// </summary>
///
/// <remarks>
/// Call <see cref="OnWorkbookShutdown" /> in the workbook's Shutdown event
/// handler, after the last instance of ApplicationSettingsBase is closed.
///
/// <para>
/// NodeXL uses ApplicationSettingsBase to store the user's settings.  The use
/// of ApplicationSettingsBase is not supported for VSTO applications, although
/// it almost works correctly.  The one problem that prevents it from working
/// correctly is worked around by this class.
/// </para>
///
/// <para>
/// ApplicationSettingsBase uses LocalFileSettingsProvider to store the user
/// settings.  In Windows Forms applications, LocalFileSettingsProvider stores
/// the settings in a folder determined in part by the application name.  As
/// explained in http://blogs.msdn.com/rprabhu/articles/433979.aspx, here is
/// the folder:
/// </para>
///
/// <para>
/// [Profile Directory]\[Company Name]\[App Name]_[Evidence Type]_[Evidence
/// Hash]\[Version]\user.config
/// </para>
///
/// <para>
/// Unfortunately, in VSTO applications the App Name part of the path is either
/// the name of the template (NodeXLGraph.xltx) or the name of the workbook if
/// the workbook has been saved (MyWorkbook.xlsx), and the Version part is the
/// version of Excel, not NodeXL.  The folder is determined by the
/// configuration system, and although you can get the folder using the
/// ConfigurationManager class, you cannot change it.
/// </para>
///
/// <para>
/// As a result of this peculiar folder behavior, the user will end up with one
/// settings file for each saved workbook, thus preventing a single set of
/// settings from being shared among all workbooks.
/// </para>
///
/// <para>
/// This class works around the problem by storing a single settings file in a
/// custom location of its choice.  The constructor copies the settings file to
/// the location expected by the configuration system.  When the application
/// settings get saved or reset, the file in the custom location gets updated.
/// At workbook shutdown, the copy gets deleted.  Thus, the configuration
/// system always works with a settings file in the location where it expects
/// it to be, but the file comes from and ends up back in the custom location.
/// </para>
///
/// <para>
/// Incidentally, the recommended way to use ApplicationSettingsBase in VSTO
/// applications is to create a custom SettingsProvider for use in place of
/// LocalFileSettingsProvider.  This is absurd.  Writing a robust
/// SettingsProvider that duplicates the sophisticated functionality of
/// LocalFileSettingsProvider is not trivial, and there is no way I'm going to
/// attempt it just to change a folder path.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class NodeXLApplicationSettingsBase : ApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: NodeXLApplicationSettingsBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="NodeXLApplicationSettingsBase" /> class.
    /// </summary>
    //*************************************************************************

    public NodeXLApplicationSettingsBase()
    {
        String sCorrectFolderPath, sCorrectFilePath;

        GetCorrectFileInformation(out sCorrectFolderPath,
            out sCorrectFilePath);

        if ( File.Exists(sCorrectFilePath) )
        {
            // Avoid unnecessary file copies.

            DateTime oCorrectFileLastWriteTime =
                File.GetLastWriteTime(sCorrectFilePath);
            
            if (oCorrectFileLastWriteTime != m_oLastCorrectFileCopyTime)
            {
                // Copy the settings file from its correct location to the
                // location expected by the configuration system.

                String sWrongFolderPath, sWrongFilePath;

                GetWrongFileInformation(out sWrongFolderPath,
                    out sWrongFilePath);

                Directory.CreateDirectory(sWrongFolderPath);
                File.Copy(sCorrectFilePath, sWrongFilePath, true);

                m_oLastCorrectFileCopyTime = oCorrectFileLastWriteTime;
            }
        }

        // AssertValid();
    }

    //*************************************************************************
    //  Method: Save()
    //
    /// <summary>
    /// Stores the current values of the application settings properties.
    /// </summary>
    //*************************************************************************

    public override void
    Save()
    {
        AssertValid();

        base.Save();
        OnApplicationSettingsChanged();
    }

    //*************************************************************************
    //  Method: Reset()
    //
    /// <summary>
    /// Restores the persisted application settings values to their
    /// corresponding default properties.
    /// </summary>
    //*************************************************************************

    public new void
    Reset()
    {
        AssertValid();

        base.Reset();
        OnApplicationSettingsChanged();
    }

    //*************************************************************************
    //  Method: OnWorkbookShutdown()
    //
    /// <summary>
    /// Performs tasks required during workbook shutdown.
    /// </summary>
    ///
    /// <remarks>
    /// Call this after the last instance of ApplicationSettingsBase is closed.
    /// </remarks>
    //*************************************************************************

    public static void
    OnWorkbookShutdown()
    {
        String sWrongFolderPath, sWrongFilePath;

        GetWrongFileInformation(out sWrongFolderPath, out sWrongFilePath);

        if ( File.Exists(sWrongFilePath) )
        {
            // Delete this unneeded folder, for example:
            //
            // C:\Users\UserName\AppData\Local\Microsoft_Corporation\
            // NodeXLGraph.xltx_Path_exlviddqfuofdx2qvofipdzmollu2gfx

            String sWrongFolderParentPath =
                Path.GetDirectoryName(sWrongFolderPath);

            try
            {
                Directory.Delete(sWrongFolderParentPath, true);
            }
            catch (IOException)
            {
            }
        }
    }

    //*************************************************************************
    //  Method: OnApplicationSettingsChanged()
    //
    /// <summary>
    /// Performs tasks required when the Save() or Reset() method is called.
    /// </summary>
    //*************************************************************************

    protected void
    OnApplicationSettingsChanged()
    {
        AssertValid();

        String sWrongFolderPath, sWrongFilePath;

        GetWrongFileInformation(out sWrongFolderPath, out sWrongFilePath);

        if ( File.Exists(sWrongFilePath) )
        {
            // Copy the settings file from the location used by the
            // configuration system to its correct folder.

            String sCorrectFolderPath, sCorrectFilePath;

            GetCorrectFileInformation(out sCorrectFolderPath,
                out sCorrectFilePath);

            Directory.CreateDirectory(sCorrectFolderPath);
            File.Copy(sWrongFilePath, sCorrectFilePath, true);
        }
    }

    //*************************************************************************
    //  Method: GetCorrectFileInformation()
    //
    /// <summary>
    /// Gets the correct folder and full path to the user's settings file.
    /// </summary>
    ///
    /// <param name="sCorrectFolderPath">
    /// Where the path to the folder containing the correct user's settings
    /// file gets stored.
    /// </param>
    ///
    /// <param name="sCorrectFilePath">
    /// Where the path to the correct user's settings file gets stored.
    /// </param>
    //*************************************************************************

    protected static void
    GetCorrectFileInformation
    (
        out String sCorrectFolderPath,
        out String sCorrectFilePath
    )
    {
        // Sample folder:
        //
        // C:\Users\UserName\AppData\Local\MicrosoftResearch\NodeXL2007Template

        sCorrectFolderPath = Path.Combine(

            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),

            CorrectSubfolder
            );

        // Sample file path:
        //
        // C:\Users\UserName\AppData\Local\MicrosoftResearch\
        // NodeXL2007Template\User.config

        sCorrectFilePath = Path.Combine(sCorrectFolderPath, FileName);
    }

    //*************************************************************************
    //  Method: GetWrongFileInformation()
    //
    /// <summary>
    /// Gets the wrong folder and full path to the user's settings file.
    /// </summary>
    ///
    /// <param name="sWrongFolderPath">
    /// Where the path to the folder containing the wrong user's settings file
    /// gets stored.
    /// </param>
    ///
    /// <param name="sWrongFilePath">
    /// Where the path to the wrong user's settings file gets stored.
    /// </param>
    //*************************************************************************

    protected static void
    GetWrongFileInformation
    (
        out String sWrongFolderPath,
        out String sWrongFilePath
    )
    {
        // Sample file path:
        //
        // C:\Users\UserName\AppData\Local\Microsoft_Corporation\
        // NodeXLGraph.xltx_Path_exlviddqfuofdx2qvofipdzmollu2gfx\
        // 12.0.6504.5001\user.config

        sWrongFilePath = ConfigurationManager.OpenExeConfiguration(
            ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;

        // Sample folder:
        //
        // C:\Users\UserName\AppData\Local\Microsoft_Corporation\
        // NodeXLGraph.xltx_Path_exlviddqfuofdx2qvofipdzmollu2gfx\
        // 12.0.6504.5001

        sWrongFolderPath = Path.GetDirectoryName(sWrongFilePath);
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public virtual void
    AssertValid()
    {
        // m_oLastCorrectFileCopyTime
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Subfolder under Environment.SpecialFolder.LocalApplicationData where
    /// the user's settings file gets stored.

    protected const String CorrectSubfolder =
        @"MicrosoftResearch\NodeXLExcel2007Template";

    /// File name of the user's settings file, without a path.  The same file
    /// name is used whether the file is in the correct folder or the wrong
    /// folder.

    protected const String FileName = "User.config";


    //*************************************************************************
    //  Private fields
    //*************************************************************************

    /// The last time the settings file was copied from its correct location to
    /// the location expected by the configuration system.

    private static DateTime m_oLastCorrectFileCopyTime = DateTime.MinValue;
}

}
