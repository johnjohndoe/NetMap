
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.IO;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: UserSettingsManager
//
/// <summary>
/// Manages the file that stores the user's settings.
/// </summary>
///
/// <remarks>
/// Call <see cref="Initialize" /> before any instances of
/// ApplicationSettingsBase are created.  Call <see cref="Close" /> after the
/// last instance of ApplicationSettingsBase is closed.
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
/// settings from being shared among all workbooks.  This class works around
/// the problem by storing a single settings file in a custom location of its
/// choice.  At initialization, it copies the settings file to the location
/// expected by the configuration system, and at shutdown it copies the
/// (possibly modified) file back to its custom location.  Thus, the
/// configuration system always works with a settings file in the location
/// where it expects it to be, but the file comes from and ends up back in the
/// custom location.
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

public static class UserSettingsManager : Object
{
    //*************************************************************************
    //  Method: Initialize()
    //
    /// <summary>
    /// Initializes the manager.
    /// </summary>
    ///
    /// <remarks>
    /// Call this before any instances of ApplicationSettingsBase are created.
    /// </remarks>
    //*************************************************************************

    public static void
    Initialize()
    {
        String sCorrectFolder, sCorrectFilePath;

        GetCorrectFileInformation(out sCorrectFolder, out sCorrectFilePath);

        if ( File.Exists(sCorrectFilePath) )
        {
            // Copy the settings file from its correct location to the location
            // expected by the configuration system.

            String sWrongFolder, sWrongFilePath;

            GetWrongFileInformation(out sWrongFolder, out sWrongFilePath);
            Directory.CreateDirectory(sWrongFolder);
            File.Copy(sCorrectFilePath, sWrongFilePath, true);
        }
    }

    //*************************************************************************
    //  Method: Close()
    //
    /// <summary>
    /// Closes the manager.
    /// </summary>
    ///
    /// <remarks>
    /// Call this after the last instance of ApplicationSettingsBase is closed.
    /// </remarks>
    //*************************************************************************

    public static void
    Close()
    {
        String sWrongFolder, sWrongFilePath;

        GetWrongFileInformation(out sWrongFolder, out sWrongFilePath);

        if ( File.Exists(sWrongFilePath) )
        {
            // Copy the settings file from the location used by the
            // configuration system to its correct folder.

            String sCorrectFolder, sCorrectFilePath;

            GetCorrectFileInformation(out sCorrectFolder,
                out sCorrectFilePath);

            Directory.CreateDirectory(sCorrectFolder);
            File.Copy(sWrongFilePath, sCorrectFilePath, true);

            // Delete this unneeded parent folder, for example:
            //
            // C:\Users\UserName\AppData\Local\Microsoft_Corporation\
            // NodeXLGraph.xltx_Path_exlviddqfuofdx2qvofipdzmollu2gfx

            String sParentFolder = Path.GetDirectoryName(sWrongFolder);

            Directory.Delete(sParentFolder, true);
        }
    }

    //*************************************************************************
    //  Method: GetCorrectFileInformation()
    //
    /// <summary>
    /// Gets the correct folder and full path to the user's settings file.
    /// </summary>
    ///
    /// <param name="sCorrectFolder">
    /// Where the correct folder containing the user's settings file gets
    /// stored.
    /// </param>
    ///
    /// <param name="sCorrectFilePath">
    /// Where the correct full path of the user's settings file gets stored.
    /// </param>
    //*************************************************************************

    private static void
    GetCorrectFileInformation
    (
        out String sCorrectFolder,
        out String sCorrectFilePath
    )
    {
        // Sample folder:
        //
        // C:\Users\UserName\AppData\Local\MicrosoftResearch\NodeXL2007Template

        sCorrectFolder = Path.Combine(

            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),

            CorrectSubfolder
            );

        // Sample file path:
        //
        // C:\Users\UserName\AppData\Local\MicrosoftResearch\
        // NodeXL2007Template\User.config

        sCorrectFilePath = Path.Combine(sCorrectFolder, FileName);
    }

    //*************************************************************************
    //  Method: GetWrongFileInformation()
    //
    /// <summary>
    /// Gets the wrong folder and full path to the user's settings file.
    /// </summary>
    ///
    /// <param name="sWrongFolder">
    /// Where the wrong folder containing the user's settings file gets
    /// stored.
    /// </param>
    ///
    /// <param name="sWrongFilePath">
    /// Where the wrong full path of the user's settings file gets stored.
    /// </param>
    //*************************************************************************

    private static void
    GetWrongFileInformation
    (
        out String sWrongFolder,
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

        sWrongFolder = Path.GetDirectoryName(sWrongFilePath);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Subfolder under Environment.SpecialFolder.LocalApplicationData where
    /// the user's settings file gets stored.

    private const String CorrectSubfolder =
        @"MicrosoftResearch\NodeXLExcel2007Template";

    /// File name of the user's settings file, without a path.  The same file
    /// name is used whether the file is in the correct folder or the wrong
    /// folder.

    private const String FileName = "User.config";
}

}
