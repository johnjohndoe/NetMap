
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Diagnostics;
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

        templatePath = Path.Combine(application.TemplatesPath, TemplateName);

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
    //  Public constants
    //*************************************************************************

    /// Application name.

    public const String ApplicationName = "Microsoft NodeXL";

    /// Name of the application's template.

    public const String TemplateName = "NodeXLGraph.xltx";
}

}
