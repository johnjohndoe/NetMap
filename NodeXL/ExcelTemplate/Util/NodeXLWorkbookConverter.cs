
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.Tools.Applications;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NodeXLWorkbookConverter
//
/// <summary>
/// Copies a NodeXL workbook created on another machine and converts the copy
/// to work on this machine.
/// </summary>
///
/// <remarks>
/// Use <see cref="ConvertNodeXLWorkbook" /> to copy and convert a NodeXL
/// workbook.
/// </remarks>
//*****************************************************************************

public class NodeXLWorkbookConverter : Object
{
    //*************************************************************************
    //  Constructor: NodeXLWorkbookConverter()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="NodeXLWorkbookConverter" /> class.
    /// </summary>
    //*************************************************************************

    public NodeXLWorkbookConverter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: ConvertNodeXLWorkbook()
    //
    /// <summary>
    /// Copies a NodeXL workbook created on another machine and converts the
    /// copy to work on this machine.
    /// </summary>
    ///
    /// <param name="otherWorkbookFile">
    /// Full path to the other workbook.  The workbook must exist.
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
    /// As of February 2010, the NodeXL setup program embeds the path to the
    /// NodeXL assemblies in the Excel template file.  If the path differs
    /// between two machines (as it will for 32-bit vs 64-bit machines), a
    /// NodeXL workbook created on one machine won't be able to be opened on
    /// the other.  This method fixes that by copying the other workbook and
    /// embedding this machine's NodeXL path in the copy.
    ///
    /// <para>
    /// An <see cref="NodeXLWorkbookConversionException" /> is thrown if the
    /// other workbook can't be copied and converted.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    ConvertNodeXLWorkbook
    (
        String otherWorkbookFile,
        String convertedWorkbookFile,
        Microsoft.Office.Interop.Excel.Application application
    )
    {
        AssertValid();
        Debug.Assert( !String.IsNullOrEmpty(otherWorkbookFile) );
        Debug.Assert( File.Exists(otherWorkbookFile) );
        Debug.Assert( !String.IsNullOrEmpty(convertedWorkbookFile) );
        Debug.Assert(application != null);

        // The application's template is needed to get the customization
        // information.

        String sTemplatePath;

        if ( !ApplicationUtil.TryGetTemplatePath(application,
            out sTemplatePath) )
        {
            throw new NodeXLWorkbookConversionException(
                ApplicationUtil.GetMissingTemplateMessage(application) );
        }

        try
        {
            File.Copy(otherWorkbookFile, convertedWorkbookFile, true);
        }
        catch (UnauthorizedAccessException)
        {
            throw new NodeXLWorkbookConversionException(
                "The converted copy already exists and is read-only.  It can't"
                + " be overwritten."
                );
        }
        catch (IOException oIOException)
        {
            if ( oIOException.Message.Contains(
                "it is being used by another process") )
            {
                throw new NodeXLWorkbookConversionException(
                    "The converted copy already exists and is open in Excel."
                    + "  It can't be overwritten."
                );
            }

            throw (oIOException);
        }

        // Remove the other customization.

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
            throw new NodeXLWorkbookConversionException(
                "The file doesn't appear to be an Excel workbook."
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
