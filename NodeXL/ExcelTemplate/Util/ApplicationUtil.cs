
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Win32;
using Microsoft.NodeXL.ApplicationUtil;
using Microsoft.NodeXL.Common;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ApplicationUtil
//
/// <summary>
/// Contains utility methods dealing with the application as a whole.
/// </summary>
//*****************************************************************************

public static class ApplicationUtil
{
    //*************************************************************************
    //  Method: OpenHomePage()
    //
    /// <summary>
    /// Opens the application's home page in a browser window.
    /// </summary>
    //*************************************************************************

    public static void
    OpenHomePage()
    {
        Process.Start(ProjectInformation.HomePageUrl);
    }

    //*************************************************************************
    //  Method: CheckForUpdate()
    //
    /// <summary>
    /// Checks for a newer version of the application.
    /// </summary>
    ///
    /// <remarks>
    /// All interaction with the user is handled by this method.
    /// </remarks>
    //*************************************************************************

    public static void
    CheckForUpdate()
    {
        // Get the version of the installed version of the application.

        FileVersionInfo oCurrentFileVersionInfo =
            AssemblyUtil2.GetFileVersionInfo();

        // Get the version information for the latest version of the
        // application.

        Int32 iLatestVersionFileMajorPart = 0;
        Int32 iLatestVersionFileMinorPart = 0;
        Int32 iLatestVersionFileBuildPart = 0;
        Int32 iLatestVersionFilePrivatePart = 0;

        try
        {
            GetLatestVersionInfo(out iLatestVersionFileMajorPart,
                out iLatestVersionFileMinorPart,
                out iLatestVersionFileBuildPart,
                out iLatestVersionFilePrivatePart
                );
        }
        catch (WebException)
        {
            FormUtil.ShowWarning(
                "The Web site from which updates are obtained could not be"
                + " reached.  Either an Internet connection isn't available,"
                + " or the Web site isn't available."
                );

            return;
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
        }

        if (
           iLatestVersionFileMajorPart > oCurrentFileVersionInfo.FileMajorPart
           ||
           iLatestVersionFileMinorPart > oCurrentFileVersionInfo.FileMinorPart
           ||
           iLatestVersionFileBuildPart > oCurrentFileVersionInfo.FileBuildPart
           ||
           iLatestVersionFilePrivatePart >
               oCurrentFileVersionInfo.FilePrivatePart
           )
        {
            String sMessage = String.Format(

                "A new version of {0} is available.  Do you want to open the"
                + " Web page from which the new version can be downloaded?"
                ,
                FormUtil.ApplicationName
                );

            if (MessageBox.Show(sMessage, FormUtil.ApplicationName,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information) ==
                    DialogResult.Yes)
            {
                Process.Start(ProjectInformation.DownloadPageUrl);
            }
        }
        else
        {
            FormUtil.ShowInformation( String.Format(

                "You have the latest version of {0}." 
                ,
                FormUtil.ApplicationName
                ) );
        }
    }

    //*************************************************************************
    //  Method: GetLatestVersionInfo()
    //
    /// <summary>
    /// Gets the version information for the latest version of the application.
    /// </summary>
    ///
    /// <param name="latestVersionFileMajorPart">
    /// Where the major part of the version number gets stored.
    /// </param>
    ///
    /// <param name="latestVersionFileMinorPart">
    /// Where the minor part of the version number gets stored.
    /// </param>
    ///
    /// <param name="latestVersionFileBuildPart">
    /// Where the build part of the version number gets stored.
    /// </param>
    ///
    /// <param name="latestVersionFilePrivatePart">
    /// Where the private part of the version number gets stored.
    /// </param>
    ///
    /// <remarks>
    /// A WebException is thrown if the version information can't be obtained.
    /// </remarks>
    //*************************************************************************

    public static void
    GetLatestVersionInfo
    (
        out Int32 latestVersionFileMajorPart,
        out Int32 latestVersionFileMinorPart,
        out Int32 latestVersionFileBuildPart,
        out Int32 latestVersionFilePrivatePart
    )
    {
        latestVersionFileMajorPart = Int32.MinValue;
        latestVersionFileMinorPart = Int32.MinValue;
        latestVersionFileBuildPart = Int32.MinValue;
        latestVersionFilePrivatePart = Int32.MinValue;

        // The version number of the latest release is embedded in the home
        // page.  Attempt to get the home page contents.

        WebRequest oWebRequest =
            WebRequest.Create(ProjectInformation.HomePageUrl);

        HttpWebResponse oHttpWebResponse = null;
        Stream oStream = null;
        StreamReader oStreamReader = null;
        String sResponse = null;

        try
        {
            oHttpWebResponse = (HttpWebResponse)oWebRequest.GetResponse();
            oStream = oHttpWebResponse.GetResponseStream();
            oStreamReader = new StreamReader(oStream);
            sResponse = oStreamReader.ReadToEnd();
        }
        finally
        {
            if (oStreamReader != null)
            {
                oStreamReader.Close();
            }

            if (oStream != null)
            {
                oStream .Close();
            }

            if (oHttpWebResponse != null)
            {
                oHttpWebResponse.Close();
            }
        }

        // Use a regular expression to parse the response.  Look for the
        // version in this format, for example:
        //
        // .NodeXL Excel 2007 Template, version 1.0.1.56

        const String Pattern =
            ".NodeXL Excel 2007 Template, version "
            + "(?<FileMajorPart>\\d+)"
            + "\\."
            + "(?<FileMinorPart>\\d+)"
            + "\\."
            + "(?<FileBuildPart>\\d+)"
            + "\\."
            + "(?<FilePrivatePart>\\d+)"
            ;

        Regex oRegex = new Regex(Pattern);
        Match oMatch = oRegex.Match(sResponse);

        if (!oMatch.Success)
        {
            throw new WebException(
                "The home page was found but it doesn't appear to contain the"
                + " latest version number."
                );
        }

        latestVersionFileMajorPart = MathUtil.ParseCultureInvariantInt32(
            oMatch.Groups["FileMajorPart"].Value);

        latestVersionFileMinorPart = MathUtil.ParseCultureInvariantInt32(
            oMatch.Groups["FileMinorPart"].Value);

        latestVersionFileBuildPart = MathUtil.ParseCultureInvariantInt32(
            oMatch.Groups["FileBuildPart"].Value);

        latestVersionFilePrivatePart = MathUtil.ParseCultureInvariantInt32(
            oMatch.Groups["FilePrivatePart"].Value);
    }

    //*************************************************************************
    //  Method: GetPlugInFolder()
    //
    /// <summary>
    /// Gets the full path to folder where plug-in assemblies are stored.
    /// </summary>
    ///
    /// <returns>
    /// The full path to folder where plug-in assemblies are stored.
    /// </returns>
    //*************************************************************************

    public static String
    GetPlugInFolder()
    {
        return ( Path.Combine(GetApplicationFolder(), PlugInSubfolder) );
    }

    //*************************************************************************
    //  Method: GetApplicationFolder()
    //
    /// <summary>
    /// Gets the full path to the application's folder.
    /// </summary>
    ///
    /// <returns>
    /// The full path to the application's folder.  Sample:
    /// "C:\Program Files\...\Microsoft NodeXL Excel Template".
    /// </returns>
    //*************************************************************************

    public static String
    GetApplicationFolder()
    {
        if (RunningInDevelopmentEnvironment)
        {
            // Sample: "E:\NodeXL\ExcelTemplate\bin\Debug"

            return ( Path.GetDirectoryName( GetExecutingAssemblyPath() ) );
        }

        // For versions 1.0.1.113 and earlier, the setup program installed
        // NodeXL into a subfolder of the standard Program Files folder and the
        // program was run from there.  The application folder could then be
        // obtained as follows:
        //
        //   String sApplicationFolder = Path.GetDirectoryName(
        //     Assembly.GetExecutingAssembly().CodeBase);
        //
        // Versions since then have continued to install NodeXL into that
        // subfolder, but ClickOnce is now called at the end of the setup to
        // install the application into the ClickOnce cache, and that is where
        // the application actually runs from.  That means that the location of
        // the executing assembly is now in some obscure location, far from the
        // Program Files folder, and the above code no longer works.
        //
        // For the following comments, note that there is only one NodeXL setup
        // program, and it is always 32-bit.  This is where the setup program
        // installs NodeXL, assuming an English version of Windows:
        //
        //     32-bit Windows: "C:\Program Files"
        //     64-bit Windows: "C:\Program Files (x86)"

        String sProgramFilesFolder;

        if (IntPtr.Size == 4)
        {
            // NodeXL is running within 32-bit Excel.
            //
            // This is what Environment.SpecialFolder.ProgramFiles returns:
            //
            //     32-bit Windows: "C:\Program Files"
            //     64-bit Windows: "C:\Program Files (x86)"
            //
            // Because this matches the folder where NodeXL is installed, no
            // special action is required.

            sProgramFilesFolder = Environment.GetFolderPath(
                Environment.SpecialFolder.ProgramFiles);
        }
        else
        {
            // NodeXL is running within 64-bit Excel.
            //
            // This is what Environment.SpecialFolder.ProgramFiles returns:
            //
            //     64-bit Windows: "C:\Program Files"
            //
            // This won't work, because NodeXL is installed at
            // "C:\Program Files (x86)".  Instead, use one of Windows'
            // environment variables.

            sProgramFilesFolder = System.Environment.GetEnvironmentVariable(
                "ProgramFiles(x86)");
        }

        Debug.Assert( !String.IsNullOrEmpty(sProgramFilesFolder) );

        return ( Path.Combine(sProgramFilesFolder, ProgramFilesSubfolder) );
    }

    //*************************************************************************
    //  Method: TryGetTemplatePath()
    //
    /// <summary>
    /// Attempts to get the full path to the application's template file.
    /// </summary>
    ///
    /// <param name="application">
    /// The Excel application.
    /// </param>
    ///
    /// <param name="templatePath">
    /// Where the path to the template file gets stored regardless of the
    /// return value.
    /// </param>
    ///
    /// <remarks>
    /// true if the template file exists.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetTemplatePath
    (
        Microsoft.Office.Interop.Excel.Application application,
        out String templatePath
    )
    {
        Debug.Assert(application != null);

        String sTemplatesPath;

        if (RunningInDevelopmentEnvironment)
        {
            // Samples, depending on which program is being run:
            //
            //   1. "E:\NodeXL\ExcelTemplate\bin\Debug"
            //
            //   2. "E:\NodeXL\NetworkServer\bin\Debug"

            sTemplatesPath = Path.GetDirectoryName(
                GetExecutingAssemblyPath() );

            // The template in the development environment is under the
            // ExcelTemplate folder.  For case 2, fix the folder.

            sTemplatesPath = sTemplatesPath.Replace("NetworkServer",
                "ExcelTemplate");
        }
        else
        {
            sTemplatesPath = application.TemplatesPath;
        }

        templatePath = Path.Combine(sTemplatesPath, TemplateName);

        return ( File.Exists(templatePath) );
    }

    //*************************************************************************
    //  Method: GetMissingTemplateMessage()
    //
    /// <summary>
    /// Gets a user-friendly message to display when the application's template
    /// file can't be found.
    /// </summary>
    ///
    /// <param name="application">
    /// The Excel application.
    /// </param>
    ///
    /// <returns>
    /// A user-friendly message.
    /// </returns>
    //*************************************************************************

    public static String
    GetMissingTemplateMessage
    (
        Microsoft.Office.Interop.Excel.Application application
    )
    {
        Debug.Assert(application != null);

        String sTemplatePath;

        ApplicationUtil.TryGetTemplatePath(application, out sTemplatePath);

        return ( String.Format(

            "The {0} Excel template couldn't be found."
            + "\r\n\r\n"
            + "The {0} setup program should have copied the template to"
            + " {1}.  If you moved the template somewhere else, you won't"
            + " be able to use this feature."
            ,
            ApplicationUtil.ApplicationName,
            sTemplatePath
            ) );
    }

    //*************************************************************************
    //  Property: RunningInDevelopmentEnvironment
    //
    /// <summary>
    /// Gets a flag indicating whether the application is running in a
    /// development environment.
    /// </summary>
    ///
    /// <value>
    /// true if the application is running in a development environment, false
    /// if it is running in an installed environment.
    /// </value>
    //*************************************************************************

    private static Boolean
    RunningInDevelopmentEnvironment
    {
        get
        {
            String sExecutingAssemblyPath =
                GetExecutingAssemblyPath().ToLower();

            return (
                sExecutingAssemblyPath.IndexOf(@"bin\debug") >= 0 ||
                sExecutingAssemblyPath.IndexOf(@"bin\release") >= 0
                );
        }
    }

    //*************************************************************************
    //  Method: GetExecutingAssemblyPath()
    //
    /// <summary>
    /// Gets the full path to the executing assembly.
    /// </summary>
    ///
    /// <returns>
    /// The full path to the executing assembly.  Sample:
    /// "...\Some ClickOnce Folder\Microsoft.NodeXL.ExcelTemplate.dll".
    /// </returns>
    //*************************************************************************

    private static String
    GetExecutingAssemblyPath()
    {
        // CodeBase returns an URI, such as "file://folder/subfolder/etc".
        // Convert it to a local path.

        Uri oUri = new Uri(Assembly.GetExecutingAssembly().CodeBase);

        return (oUri.LocalPath);
    }


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// Application subfolder within the Program Files folder.  There is no
    /// UI in the setup program to select another folder, so this will always
    /// be the correct subfolder.

    private const String ProgramFilesSubfolder =
        @"Microsoft Research\Microsoft NodeXL Excel Template";

    /// Subfolder under the application folder where plug-in assemblies are
    /// stored.

    private const String PlugInSubfolder = "PlugIns";


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Application name.

    public const String ApplicationName = "Microsoft NodeXL";

    /// Name of the application's template.

    public const String TemplateName = "NodeXLGraph.xltx";

    /// Solution ID, as a GUID string.

    public const String SolutionID = "aa51c0f3-62b4-4782-83a8-a15dcdd17698";
}

}
