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
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Security;
using System.Security.Permissions;
using Microsoft.VisualStudio.Tools.Office.Runtime.Security;

namespace InclusionListCustomActions
{
    [RunInstaller(true)]
    public class TrustInstaller
        : Installer
    {
        #if false  // NodeXLModification

        const string RSA_PublicKey = "<RSAKeyValue><Modulus>0VjTVM/DV60EG+n1FvletQlhgsvxny0FrqqApTHfr+Tjvokfpftqg3f30NF0J+HwLHeCJsoGMvKtVntRbvO0j/iriBivPIMXIBjZoea/OT+TlBvS1kH3w3H+TmIcQosQ1Nf1J7gfO3X6WGHBu8GhWb2llJJ3sK+s6xIIUgQzzh0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        #else

        const string RSA_PublicKey = "<RSAKeyValue><Modulus>+HuVIL/mq/MREBbqfDLuJXBIBj6NEtyhTAto4SnG4B3QFB1EZ91TBPULEY0T193CiLcGLSJ7N9YEunCe1gFDLQyxnXeWljiLtI56EuWfCtRQTw8k8IQUrmwEt8qzNGdB7snvAmULtO676cW3Y/Izt4olInVlLCIcBsqxUm3SaqU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        #endif

        public override void Install(IDictionary stateSaver)
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
                    "register a trust relationship. Start Excel " +
                    "and confirm the trust dialog to run the addin.");
            }
            Uri deploymentManifestLocation = null;

            // NodeXLModification
            //
            // "deploymentManifestLocation" parameter name changed to
            // "deploymentLocation" to make it consistent with
            // ClickOnceInstaller.cs.

            if (Uri.TryCreate(Context.Parameters["deploymentLocation"],
                UriKind.RelativeOrAbsolute, out deploymentManifestLocation) == false)
            {
                throw new InstallException(
                    "The location of the deployment manifest is missing or invalid.");
            }
            AddInSecurityEntry entry = new AddInSecurityEntry(
                            deploymentManifestLocation, RSA_PublicKey);
            UserInclusionList.Add(entry);
            stateSaver.Add("entryKey", deploymentManifestLocation);
            base.Install(stateSaver);
        }

        public override void Uninstall(IDictionary savedState)
        {
            Uri deploymentManifestLocation = (Uri)savedState["entryKey"];
            if (deploymentManifestLocation != null)
            {
                UserInclusionList.Remove(deploymentManifestLocation);
            }
            base.Uninstall(savedState);
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }

        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }
    }
}
