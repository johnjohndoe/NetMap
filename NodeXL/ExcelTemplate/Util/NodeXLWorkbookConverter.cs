
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
    /// NodeXL setup programs for versions 1.0.1.113 and earlier embedded the
    /// full path to NodeXL's deployment manifest in the Excel template file.
    /// For example, here is the _AssemblyLocation custom property value on one
    /// machine:
    ///
    /// <para>
    /// file:///C:/Program Files/Microsoft Research/Microsoft NodeXL Excel
    /// Template/Microsoft.NodeXL.ExcelTemplate.vsto|aa51c0f3-62b4-4782-83a8-
    /// a15dcdd17698|vstolocal
    /// </para>
    ///
    /// <para>
    /// On a 64-bit machine, however, NodeXL has a different installation path
    /// and the _AssemblyLocation property value path might be this:
    /// </para>
    ///
    /// <para>
    /// file:///C:/Program Files (x86)/Microsoft Research/Microsoft NodeXL
    /// Excel Template/Microsoft.NodeXL.ExcelTemplate.vsto|aa51c0f3-62b4-4782-
    /// 83a8-a15dcdd17698|vstolocal
    /// </para>
    ///
    /// <para>
    /// Because the paths differ, a NodeXL workbook created on one machine
    /// couldn't be opened on the other.
    /// </para>
    ///
    /// <para>
    /// The setup program for version 1.0.1.114 fixed the problem for newly-
    /// created workbooks by using ClickOnce to install the application.  With
    /// ClickOnce, only the deployment manifest's name (without a path) is
    /// embedded in the Excel template file.  For example:
    /// </para>
    ///
    /// <para>
    /// Microsoft.NodeXL.ExcelTemplate.vsto|aa51c0f3-62b4-4782-83a8-
    /// a15dcdd17698
    /// </para>
    ///
    /// <para>
    /// Now, a workbook created on any machine has the same _AssemblyLocation
    /// property value, and workbooks can be freely interchanged.
    /// </para>
    ///
    /// <para>
    /// For a workbook created with older NodeXL versions on another machine
    /// that didn't have the same installation path, this method will fix the
    /// problem of not being able to open the workbook on this machine.  It
    /// copies the older workbook, then modifies the copy's _AssemblyLocation
    /// property to look like this:
    /// </para>
    ///
    /// <para>
    /// Microsoft.NodeXL.ExcelTemplate.vsto|aa51c0f3-62b4-4782-83a8-
    /// a15dcdd17698
    /// </para>
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

        #if false  // For testing.

        sTemplatePath = @"E:\NodeXL\ExcelTemplateSetup\"
            + "TemplateModifiedForClickOnce\NodeXLGraph.xltx";

        #endif

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

        // Create a ServerDocument from the application's template.  The
        // solution ID and deployment manifest name will be obtained from this.

        using ( ServerDocument oTemplateServerDocument =
            new ServerDocument(sTemplatePath) )
        {
            // For some reason, ServerDocument.AddCustomization() also requires
            // a path to the NodeXL assembly file, even though it doesn't get
            // embedded in the document.

            String sAssemblyFile = new Uri(
                Assembly.GetExecutingAssembly().CodeBase).LocalPath;

            String [] asNonPublicCachedDataMembers;

            ServerDocument.AddCustomization(convertedWorkbookFile,
                sAssemblyFile, oTemplateServerDocument.SolutionId,
                oTemplateServerDocument.DeploymentManifestUrl, false,
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
