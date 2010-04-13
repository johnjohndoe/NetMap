
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplateSetupStarter
{
//*****************************************************************************
//  Class: Program
//
/// <summary>
/// Console application that starts the real Excel Template setup program.
/// </summary>
///
/// <remarks>
/// The Excel Template setup program built by the ExcelTemplateSetup project
/// must be Run as Administrator on machines with UAC turned on.  (See
/// ExcelTemplateSetup\ReadMe.txt for details on why this is the case.)  The
/// user can't be relied upon to right-click the setup program and select Run
/// as Administrator from the context menu.  Instead, the console application
/// built by this project is used to start the setup program via the Process
/// class.  It uses a verb of "runas," which is the same thing as right-
/// clicking the setup program and selecting Run as Administrator.  The real
/// setup program is given an obscure name during the build process and this
/// console application is given the name "Setup.exe."
/// </remarks>
//*****************************************************************************

class Program
{
    static void
    Main
    (
        string[] args
    )
    {
        ProcessStartInfo oProcessStartInfo =
            new ProcessStartInfo(RealSetupExeName);

        if (Environment.OSVersion.Version.Major >= 6)
        {
            // In XP, using "runas" raises a "...has encountered a problem
            // and must be terminated..." error.  Use runas only in Vista
            // and above.

            oProcessStartInfo.Verb = "runas";
        }

        oProcessStartInfo.UseShellExecute = true;
        Process oProcess = Process.Start(oProcessStartInfo);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Name of the real Excel Template setup program that gets run by this
    /// program.
    ///
    /// "Dnrd" stands for "do not run directly," although no one
    /// needs to know this.  The end user will see two executables in the Zip
    /// file she downloads: Setup.exe and Dnrd.exe.  She will naturally run
    /// Setup.exe.

    protected const String RealSetupExeName = "Dnrd.exe";
}
}
