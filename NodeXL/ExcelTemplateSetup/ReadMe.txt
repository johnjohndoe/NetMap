
This is a history of the ExcelTemplate setup process and an explanation of how
setup works today (September 2010).


Version 1
---------
This earliest version of the setup program used the standard ClickOnce
installation technique.  This worked, but it required that the user manually
copy the Excel template file to his Templates folder, which was a nuisance.  It
also didn't allow a Start menu shortcut to be added to the user's machine.


Version 2
---------
The second version of the setup program used Windows Installer, following the
steps outlined in "Additional Requirements for Document-level Solutions" at
http://msdn.microsoft.com/en-us/library/
cc616991.aspx#VSTO3Solutions2_AdditionalRequirementsforDocumentlevelSolutions.
That version copied the template file and added a Start menu shortcut.  It had
a major problem, however, that was never mentioned in the article: Because the
path to the deployment manifest gets embedded in the template, and that path
points to a folder within the user's Program Files folder, workbooks created on
one machine cannot always be opened on another.  In particular, the name of the
Program Files folder differs between 32-bit and 64-bit machines, so a workbook
created on a 32-bit machine is guaranteed to be unopenable on a 64-bit machine.


Version 3
---------
The third version of the setup program uses the hybrid Windows /ClickOnce
technique described in "Deploying to the ClickOnce Cache" in the same article.  
A Windows Installer setup copies the template and adds the shortcut, then calls
a custom action to install the solution in the ClickOnce cache.  Because a file
path is no longer embedded in the Excel template file, workbooks created from
the template can be reliably exchanged with other users.

The following changes had to be made to the technique outline in the article:

* The setup program copies all the assemblies to a subfolder in the Program
Files folder, just as in the previous setup version.  Those assemblies are
usually not used, because ClickOnce installs copies of them in the ClickOnce
cache, and the copies are what are usually run.  However, having a set in
Program Files solves a backward-compatibility issue.  Because old workbooks
created with NodeXL versions installed by the previous setup technique point to
the Program Files subfolder, those old workbook would fail to open if the
assemblies weren't present in the subfolder.  With copies of the assemblies in
both the subfolder and the ClickOnce cache, old and new workbooks will all
function properly.  (Old workbooks still can't be reliably opened on other
machines, however.)

* The ClickOnceInstaller.cs custom action had to be modified to get
VSTOInstaller.exe to run under the correct user identity on machines with UAC
turned on.  If you follow the steps in the article, you'll create a custom
action in Visual Studio that has Windows Installer's
msidbCustomActionTypeNoImpersonate bit set.  On machines with UAC turned on,
that causes VSTOInstaller.exe to run with SYSTEM credentials, which results in
the solution getting installed for the wrong user.  To fix this, a
RunPostBuildEvent is used to remove the bit from the MSI file's CustomActions
table, and some changes had to be made in ClickOnceInstaller.cs to get the
correct identity to be transferred from the Process class to VSTOInstaller.exe.
(The identity problem didn't occur on machines with UAC turned off, or when Run
as Administrator was used in Windows Explorer to run the setup program.  I
don't know why.)

* The previous step still didn't solve all UAC issues.  To bypass those issues,
the user doesn't directly run the setup program created by the
ExcelTemplateSetup project.  Instead, he runs the Setup.exe created by the
ExcelTemplateSetupStarter project, which is a very simple console application
that calls the ExcelTemplateSetup setup with a verb of "runas."  That causes
the real setup program to run as administrator, and the UAC problem is
bypassed.

It took about four days to get this to work, and I still don't understand all
the interactions between ClickOnce, Windows Installer, and UAC.  It's one big,
complicated nightmare.


Version 4
---------
The fourth and current version of the setup program made a few changes to make
it easier for an administrator to install NodeXL for multiple users:

* An "Installation Folder" dialog was added to the setup program to allow the
user to select an "install for everyone" option, which results in everyone
getting a Start Menu item for NodeXL.  (Visual Studio puts the "install for"
option on the Installation Folder dialog.)  The dialog's folder path controls
were disabled (via PostBuildEvent calls to WiRunSQL.vbs script) to prevent the
user from changing the installation folder, which would prevent the
ExcelTemplate project from finding its PlugIns folder.

* The Excel template file was moved from the user's profile folder to the
Program Files subfolder, and the Start Menu shortcut that points to the
template file was updated accordingly.  Now, each user can simply select the
Start Menu shortcut to use the template.  The VSTO loader looks for (and finds)
the Microsoft.NodeXL.ExcelTemplate.vsto manifest file in the same folder as the
template file, and the loader installs NodeXL in the user's ClickOnce cache
after asking the user for permission.  Prior to this change, each user had to
double-click the manifest file to install NodeXL in his ClickOnce cache.
