//-----------------------------------------------------------------------
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//-----------------------------------------------------------------------

/*
This is a modified version of the DocumentManifestCustomActions project
downloaded from "Deploying a VSTO 3.0 solution for the Office 2007 using
Windows Installers," at http://code.msdn.microsoft.com/VSTO3MSI, on 9/15/2008.

In the original version, a source document specified by the documentLocation
parameter is copied to the MyDocuments folder, and the custom properties of the
copied document are set to point to the deployment manifest and customization
DLL.  This is refered to as "attaching the customization" in the article.

Here is what the modified version does:

1. The customization is attached to the source document specified by the
   documentLocation parameter.

2. If there is a document in the source document folder that has the name
   specified by the newDocumentName parameter (which has no path), that
   document is deleted.

3. The source document specified by the documentLocation parameter is renamed
   to the name specified by the newDocumentName parameter.

You can't just attach a customization to the final source document.  If you do
this, the file's Modified date will be later than the file's Created date.
Windows Installer will never overwrite such files, so the document won't get
updated in the next release.

Here is a sample set of parameters:

/assemblyLocation="[TARGETDIR]Microsoft.NodeXL.ExcelTemplate.dll"
/deploymentManifestLocation="[TARGETDIR]Microsoft.NodeXL.ExcelTemplate.vsto"
/documentLocation="[AppDataFolder]Microsoft\Templates\TempNodeXLGraph.xltx"
/newDocumentName="NodeXLGraph.xltx"

Original code is marked with OriginalCode.  New code is marked with NewCode.
*/

using System;
using System.Configuration.Install;
using System.ComponentModel;
using System.Collections;
using Microsoft.VisualStudio.Tools.Applications;
using System.IO;

# if false  // OriginalCode.
namespace ManifestCustomActions
#endif

#if true  // NewCode.
namespace Microsoft.NodeXL.ExcelTemplateSetupCustomActions
#endif
{
    [RunInstaller(true)]
    public class ChangeManifestInstaller
        : Installer
    {
        static readonly Guid SolutionID =
			new Guid("aa51c0f3-62b4-4782-83a8-a15dcdd17698");

        public override void Install(IDictionary stateSaver)
        {
            string[] nonpublicCachedDataMembers = null;

            Uri deploymentManifestLocation = null;
            if (Uri.TryCreate(
                Context.Parameters["deploymentManifestLocation"],
                UriKind.RelativeOrAbsolute,
                out deploymentManifestLocation) == false)
            {
                throw new InstallException(
                    "The location of the deployment manifest " + 
                    "is missing or invalid.");
            } 

            string documentLocation =
                Context.Parameters["documentLocation"];
            if (String.IsNullOrEmpty(documentLocation))
            {
                throw new InstallException(
                    "The location of the document is missing.");
            }

			#if true  // NewCode.
            string newDocumentName =
                Context.Parameters["newDocumentName"];
            if (String.IsNullOrEmpty(newDocumentName))
            {
                throw new InstallException(
                    "The new file name of the document is missing.");
            }
			#endif

            string assemblyLocation =
                Context.Parameters["assemblyLocation"];
            if (String.IsNullOrEmpty(assemblyLocation))
            {
                throw new InstallException(
                    "The location of the assembly is missing.");
            }

			#if false  // OriginalCode.
            string targetLocation = CreateTargetLocation(documentLocation);
            File.Copy(documentLocation, targetLocation);
			#endif

			#if true  // NewCode.
            string targetLocation = documentLocation;
			#endif

            if (ServerDocument.IsCustomized(targetLocation))
            {
                ServerDocument.RemoveCustomization(targetLocation);
            }
            ServerDocument.AddCustomization(
                targetLocation,
                assemblyLocation,
                SolutionID, 
                deploymentManifestLocation, 
                true,
                out nonpublicCachedDataMembers);

			{
			// NewCode.

			String sNewDocumentLocation = Path.Combine(
				Path.GetDirectoryName(documentLocation),
				newDocumentName);

			if ( File.Exists(sNewDocumentLocation) )
			{
				File.Delete(sNewDocumentLocation);
			}

			File.Move(documentLocation, sNewDocumentLocation);
			}


            stateSaver.Add("targetLocation", targetLocation);
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

        public override void Uninstall(IDictionary savedState)
        {
			#if false  // OriginalCode.
            string targetLocation = (string)savedState["targetLocation"];
            if (String.IsNullOrEmpty(targetLocation) == false)
            {
                File.Delete(targetLocation);
            }
			#endif

            base.Uninstall(savedState);
        }

        string CreateTargetLocation(string documentLocation)
        {
            string fileName = Path.GetFileName(documentLocation);
            string myDocuments = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments);
            return Path.Combine(myDocuments, fileName);           
        }
    }
}
