// -----------------------------------------------------------------------
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
// -----------------------------------------------------------------------

/*
This is a modified version of a sample file from "Deploying a Visual Studio
Tools for the Office System 3.0 Solution for the 2007 Microsoft Office System
Using Windows Installer (Part 2 of 2) Windows Installers," at
http://msdn.microsoft.com/en-us/library/cc616991.aspx on 3/6/2010.

Modifications are marked with "NodeXLModification".
*/

using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Security.Permissions;
using System.Security;
using System.Diagnostics;

using System.Collections;  // NodeXLModification

namespace ClickOnceCustomActions
{
    [RunInstaller(true)]
    public class ClickOnceInstaller
        : Installer
    {
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            try
            {
                SecurityPermission permission =
                    new SecurityPermission(PermissionState.Unrestricted);
                permission.Demand();
            }
            catch (SecurityException)
            {
                throw new InstallException(
                    "You have insufficient privileges to " +
                    "install the add-in into the ClickOnce cache. " + 
                    "Please contact your system administrator.");
            }
            string deploymentLocation = Context.Parameters["deploymentLocation"];
            if (String.IsNullOrEmpty(deploymentLocation))
            {
                throw new InstallException("Deployment location not configured. Setup unable to continue");
            }

            string arguments = String.Format(
                "/S /I \"{0}\"", deploymentLocation);

            int exitCode = ExecuteVSTOInstaller(arguments);
            if (exitCode != 0)
            {
                string message = null;
                switch (exitCode)
                {
                    case -300:
                        message = String.Format(
                            "The Visual Studio Tools for Office solution was signed by an untrusted publisher and as such cannot be installed automatically. Please use your browser to navigate to {0} in order to install the solution manually. You will be prompted if the solution is trusted for execution.",
                            deploymentLocation);
                        break;
                    default:
                        message = String.Format(
                            "The installation of the ClickOnce solution failed with exit code {0}",
                            exitCode);
                        break;
                }
                throw new InstallException(message);
            }
            stateSaver.Add("deploymentLocation", deploymentLocation);
            base.Install(stateSaver);
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }

        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            string deploymentLocation = (string)savedState["deploymentLocation"];
            if (deploymentLocation != null)
            {
                string arguments = String.Format(
                    "/S /U \"{0}\"", deploymentLocation);
                ExecuteVSTOInstaller(arguments);
            }
            base.Uninstall(savedState);
        }

        int ExecuteVSTOInstaller(string arguments)
        {
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
            string subPath = @"Microsoft Shared\VSTO\9.0\VSTOInstaller.exe";
            string vstoInstallerPath = Path.Combine(basePath, subPath);
            if (File.Exists(vstoInstallerPath) == false)
            {
                throw new InstallException(
                    "The Visual Studio Tools for Office installer was not found.");
            }
            ProcessStartInfo startInfo = new ProcessStartInfo(vstoInstallerPath);
            startInfo.Arguments = arguments;

            #if true  // NodeXLModification

            // These are required to pass elevated privileges to
            // VSTOInstaller.exe.

            startInfo.Verb = "runas";
            startInfo.UseShellExecute = false;

            #endif

            Process process = Process.Start(startInfo);
            process.WaitForExit();
            return process.ExitCode;
        }
    }
}
