
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.Tools.Applications;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: OldWorkbookConverter
//
/// <summary>
/// Copies an old workbook created with an old version of NodeXL and converts
/// the copy to work with the current version.
/// </summary>
///
/// <remarks>
/// Use <see cref="ConvertOldWorkbook" /> to convert an old workbook.
/// </remarks>
//*****************************************************************************

public class OldWorkbookConverter : Object
{
    //*************************************************************************
    //  Constructor: OldWorkbookConverter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="OldWorkbookConverter" />
    /// class.
    /// </summary>
    //*************************************************************************

    public OldWorkbookConverter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: ConvertOldWorkbook()
    //
    /// <summary>
    /// Copies an old workbook created with an old version of NodeXL and
    /// converts the copy to work with the current version.
    /// </summary>
    ///
    /// <param name="oldWorkbookFile">
    /// Full path to the old workbook.  The workbook must exist.
    /// </param>
    ///
    /// <param name="convertedWorkbookFile">
    /// Full path to the converted workbook this method will create.  If the
    /// file already exists, it gets overwritten.
    /// </param>
    ///
    /// <param name="application">
    /// Excel application.
    /// </param>
    ///
    /// <remarks>
    /// An <see cref="OldWorkbookConversionException" /> is thrown if the old
    /// workbook can't be copied and converted.
    /// </remarks>
    //*************************************************************************

    public void
    ConvertOldWorkbook
    (
        String oldWorkbookFile,
        String convertedWorkbookFile,
        Microsoft.Office.Interop.Excel.Application application
    )
    {
        AssertValid();
        Debug.Assert( !String.IsNullOrEmpty(oldWorkbookFile) );
        Debug.Assert( File.Exists(oldWorkbookFile) );
        Debug.Assert( !String.IsNullOrEmpty(convertedWorkbookFile) );
        Debug.Assert(application != null);

        // The application's template is needed to get the customization
        // information.

        String sTemplatePath;

        if ( !ApplicationUtil.TryGetTemplatePath(application,
            out sTemplatePath) )
        {
            throw new OldWorkbookConversionException(
                ApplicationUtil.GetMissingTemplateMessage(application) );
        }

        try
        {
            File.Copy(oldWorkbookFile, convertedWorkbookFile, true);
        }
        catch (UnauthorizedAccessException)
        {
            throw new OldWorkbookConversionException(
                "The converted copy already exists and is read-only.  It can't"
                + " be overwritten."
                );
        }
        catch (IOException oIOException)
        {
            if ( oIOException.Message.Contains(
                "it is being used by another process") )
            {
                throw new OldWorkbookConversionException(
                    "The converted copy already exists and is open in Excel."
                    + "  It can't be overwritten."
                );
            }

            throw (oIOException);
        }

        // Remove the old customization.

        try
        {
            if ( ServerDocument.IsCustomized(convertedWorkbookFile) )
            {
                ServerDocument.RemoveCustomization(convertedWorkbookFile);
            }
        }
        catch (Microsoft.VisualStudio.Tools.Applications.Runtime.
            UnknownCustomizationFileException)
        {
            throw new OldWorkbookConversionException(
                "The old file doesn't appear to be an Excel workbook."
            );
        }

        // Create a ServerDocument from the application's template.  Most of
        // the customization information to add to the converted workbook will
        // be obtained from this.

        using ( ServerDocument oTemplateServerDocument =
            new ServerDocument(sTemplatePath) )
        {
            // The solution ID and deployment manifest path are available
            // directly from the ServerDocument.  For some reason, the assembly
            // file name is not, and so it has to be derived.
            //
            // The assembly is in the same directory as the deployment
            // manifest, so start with that directory.  Then add the file name
            // (without path) of the current assembly.

            Uri oDeploymentManifestUrl =
                oTemplateServerDocument.DeploymentManifestUrl;

            String sAssemblyFile = Path.Combine(
                Path.GetDirectoryName(oDeploymentManifestUrl.LocalPath),
                Path.GetFileName(Assembly.GetExecutingAssembly().Location)
                );

            String [] asNonPublicCachedDataMembers;

            ServerDocument.AddCustomization(convertedWorkbookFile,
                sAssemblyFile, oTemplateServerDocument.SolutionId,
                oDeploymentManifestUrl, true,
                out asNonPublicCachedDataMembers);
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
