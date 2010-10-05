
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Diagnostics;
using Microsoft.NodeXL.Visualization.Wpf;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: TaskAutomator
//
/// <summary>
/// Runs multiple tasks on one workbook or a folder full of workbooks.
/// </summary>
///
/// <remarks>
/// Call <see cref="AutomateThisWorkbook" /> to run a specified set of tasks on
/// one NodeXL workbook, or <see cref="AutomateFolder" /> to run them on every
/// unopened NodeXL workbook in a folder.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class TaskAutomator : Object
{
    //*************************************************************************
    //  Method: AutomateThisWorkbook()
    //
    /// <summary>
    /// Runs a specified set of tasks on one NodeXL workbook.
    /// </summary>
    ///
    /// <param name="thisWorkbook">
    /// The NodeXL workbook to run the tasks on.
    /// </param>
    ///
    /// <param name="tasksToRun">
    /// The tasks to run, as an ORed combination of <see
    /// cref="AutomationTasks" /> flags.
    /// </param>
    ///
    /// <param name="ribbon">
    /// The workbook's Ribbon.
    /// </param>
    //*************************************************************************

    public static void
    AutomateThisWorkbook
    (
        ThisWorkbook thisWorkbook,
        AutomationTasks tasksToRun,
        Ribbon ribbon
    )
    {
        Debug.Assert(thisWorkbook != null);
        Debug.Assert(ribbon != null);

        Microsoft.Office.Interop.Excel.Workbook oWorkbook =
            thisWorkbook.InnerObject;

        if ( (tasksToRun & AutomationTasks.MergeDuplicateEdges) != 0 )
        {
            // In general, automation is best performed by simulating a click
            // of a Ribbon button, thus avoiding any duplicate code.

            if ( !ribbon.OnMergeDuplicateEdgesClick(false) )
            {
                return;
            }
        }

        if ( (tasksToRun & AutomationTasks.CalculateGraphMetrics) != 0 )
        {
            // In this case, clicking the corresponding Ribbon button opens a
            // GraphMetricsDialog, which allows the user to edit the graph
            // metric settings before calculating the graph metrics.  The
            // actual calculations are done by CalculateGraphMetricsDialog, so
            // just use that dialog directly.

            CalculateGraphMetricsDialog oCalculateGraphMetricsDialog =
                new CalculateGraphMetricsDialog( oWorkbook,
                    new GraphMetricUserSettings() );

            if (oCalculateGraphMetricsDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
        }

        if ( (tasksToRun & AutomationTasks.AutoFillWorkbook) != 0 )
        {
            // In this case, clicking the corresponding Ribbon button opens an
            // AutoFillWorkbookDialog, which allows the user to edit the
            // autofill settings before autofilling the workbook.  The actual
            // autofilling is done by WorkbookAutoFiller, so just use that
            // class directly.

            try
            {
                WorkbookAutoFiller.AutoFillWorkbook(
                    oWorkbook, new AutoFillUserSettings(oWorkbook) );

                ribbon.OnWorkbookAutoFilled(false);
            }
            catch (Exception oException)
            {
                ErrorUtil.OnException(oException);
                return;
            }
        }

        if ( (tasksToRun & AutomationTasks.CreateSubgraphImages) != 0 )
        {
            ribbon.OnCreateSubgraphImagesClick(
                CreateSubgraphImagesDialog.DialogMode.Automate);
        }

        if ( (tasksToRun & AutomationTasks.CalculateClusters) != 0 )
        {
            if ( !ribbon.OnCalculateClustersClick() )
            {
                return;
            }
        }

        if ( (tasksToRun & AutomationTasks.ReadWorkbook) != 0 )
        {
            // If the vertex X and Y columns were autofilled, the layout type
            // was set to LayoutType.Null.  This will cause
            // TaskPane.ReadWorkbook() to display a warning.  Temporarily turn
            // the warning off.

            NotificationUserSettings oNotificationUserSettings =
                new NotificationUserSettings();

            Boolean bOldLayoutTypeIsNull =
                oNotificationUserSettings.LayoutTypeIsNull;

            oNotificationUserSettings.LayoutTypeIsNull = false;
            oNotificationUserSettings.Save();

            if ( (tasksToRun & AutomationTasks.SaveGraphImageFile) != 0 )
            {
                if ( String.IsNullOrEmpty(thisWorkbook.Path) )
                {
                    throw new InvalidOperationException(
                        WorkbookNotSavedMessage);
                }

                // After the workbook is read and the graph is laid out, save
                // an image of the graph to a file.

                GraphLaidOutEventHandler oGraphLaidOutEventHandler = null;

                oGraphLaidOutEventHandler =
                    delegate(Object sender, GraphLaidOutEventArgs e)
                {
                    // This delegate remains forever, even when the dialog
                    // class is destroyed.  Prevent it from being called again.

                    thisWorkbook.GraphLaidOut -= oGraphLaidOutEventHandler;

                    SaveGraphImageFile(e.NodeXLControl, thisWorkbook.FullName);
                };

                thisWorkbook.GraphLaidOut += oGraphLaidOutEventHandler;
            }

            ribbon.OnReadWorkbookClick();

            oNotificationUserSettings.LayoutTypeIsNull = bOldLayoutTypeIsNull;
            oNotificationUserSettings.Save();
        }
    }

    //*************************************************************************
    //  Method: AutomateFolder()
    //
    /// <summary>
    /// Runs a specified set of tasks on every unopened NodeXL workbook in a
    /// folder.
    /// </summary>
    ///
    /// <param name="folderToAutomate">
    /// Path to the folder to automate.
    /// </param>
    ///
    /// <param name="tasksToRun">
    /// The tasks to run on each unopened NodeXL workbook in the folder, as an
    /// ORed combination of <see cref="AutomationTasks" /> flags.
    /// </param>
    ///
    /// <param name="application">
    /// The Excel application for the workbook calling this method.
    /// </param>
    //*************************************************************************

    public static void
    AutomateFolder
    (
        String folderToAutomate,
        AutomationTasks tasksToRun,
        Microsoft.Office.Interop.Excel.Application application
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(folderToAutomate) );
        Debug.Assert(application != null);

        foreach ( String sFileName in Directory.GetFiles(folderToAutomate,
            "*.xlsx") )
        {
            String sFilePath = Path.Combine(folderToAutomate, sFileName);

            try
            {
                if (!NodeXLWorkbookUtil.FileIsNodeXLWorkbook(sFilePath))
                {
                    continue;
                }
            }
            catch (IOException)
            {
                // Skip any workbooks that are already open, or that have any
                // other problems that prevent them from being opened.

                continue;
            }

            // Ideally, the Excel API would be used here to open the workbook
            // and run the AutomateThisWorkbook() method on it.  Two things
            // make that impossible:
            //
            //   1. When you open a workbook using
            //      Application.Workbooks.Open(), you get only a native Excel
            //      workbook, not an "extended" ThisWorkbook object or its
            //      associated Ribbon object.  AutomateThisWorkbook() requires
            //      a Ribbon object.
            //
            //      Although a GetVstoObject() extension method is available to
            //      convert a native Excel workbook to an extended workbook,
            //      that method doesn't work on a native workbook opened via
            //      the Excel API -- it always returns null.
            //
            //      It might be possible to refactor AutomateThisWorkbook() to
            //      require only a native workbook.  However, problem 2 would
            //      still make things impossible...
            //
            //   2. If this method is being run from a modal dialog, which it
            //      is (see AutomateTasksDialog), then code in the workbook
            //      that needs to be automated doesn't run until the modal
            //      dialog closes.
            //      
            // The following code works around these problems.

            try
            {
                // Store an "automate tasks on open" flag in the workbook,
                // indicating that task automation should be run on it the next
                // time it's opened.  This can be done via the Excel API.

                Microsoft.Office.Interop.Excel.Workbook oWorkbookToAutomate =
                    ExcelUtil.OpenWorkbook(sFilePath, application);

                PerWorkbookSettings oPerWorkbookSettings =
                    new PerWorkbookSettings(oWorkbookToAutomate);

                oPerWorkbookSettings.AutomateTasksOnOpen = true;
                oWorkbookToAutomate.Save();
                oWorkbookToAutomate.Close(false, Missing.Value, Missing.Value);

                // Now open the workbook in another instance of Excel, which
                // bypasses problem 2.  Code in the workbook's Ribbon will
                // detect the flag's presence, run task automation on it, close
                // the workbook, and close the other instance of Excel.

                OpenWorkbookToAutomate(sFilePath);
            }
            catch (Exception oException)
            {
                ErrorUtil.OnException(oException);
                return;
            }
        }
    }

    //*************************************************************************
    //  Method: OpenWorkbookToAutomate()
    //
    /// <summary>
    /// Opens a workbook that has its AutomateTasksOnOpen flag set to true,
    /// then waits for the workbook to close.
    /// </summary>
    ///
    /// <param name="workbookPath">
    /// Path to the workbook to open.
    /// </param>
    //*************************************************************************

    public static void
    OpenWorkbookToAutomate
    (
        String workbookPath
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(workbookPath) );

        Process oProcess = Process.Start("Excel.exe",
            "\"" + workbookPath + "\"");

        oProcess.WaitForExit();
        oProcess.Close();
    }

    //*************************************************************************
    //  Method: SaveGraphImageFile()
    //
    /// <summary>
    /// Save an image of the graph to a file.
    /// </summary>
    ///
    /// <param name="oNodeXLControl">
    /// The control that displays the graph.
    /// </param>
    ///
    /// <param name="sWorkbookFilePath">
    /// Path to the workbook file.  Sample: "C:\Workbooks\TheWorkbook.xlsx".
    /// </param>
    ///
    /// <remarks>
    /// The settings stored in the AutomatedGraphImageUserSettings class are
    /// used to save the image.
    /// </remarks>
    //*************************************************************************

    private static void
    SaveGraphImageFile
    (
        NodeXLControl oNodeXLControl,
        String sWorkbookFilePath
    )
    {
        Debug.Assert(oNodeXLControl != null);
        Debug.Assert( !String.IsNullOrEmpty(sWorkbookFilePath) );

        AutomatedGraphImageUserSettings oAutomatedGraphImageUserSettings =
            new AutomatedGraphImageUserSettings();

        Size oImageSizePx = oAutomatedGraphImageUserSettings.ImageSizePx;

        Bitmap oBitmapCopy = oNodeXLControl.CopyGraphToBitmap(
            oImageSizePx.Width, oImageSizePx.Height);

        ImageFormat eImageFormat =
            oAutomatedGraphImageUserSettings.ImageFormat;

        String sImageFilePath = Path.ChangeExtension( sWorkbookFilePath,
            SaveableImageFormats.GetFileExtension(eImageFormat) );

        try
        {
            oBitmapCopy.Save(sImageFilePath, eImageFormat);
        }
        catch (System.Runtime.InteropServices.ExternalException)
        {
            // When an image file already exists and is read-only, an
            // ExternalException is thrown.
            //
            // Note that this method is called from the
            // ThisWorkbook.GraphLaidOut event handler, so this exception can't
            // be handled by a TaskAutomator.AutomateThisWorkbook() exception
            // handler.

            FormUtil.ShowWarning( String.Format(
                "The image file \"{0}\" couldn't be saved.  Does a read-only"
                + " file with the same name already exist?"
                ,
                sImageFilePath
                ) );
        }
        finally
        {
            oBitmapCopy.Dispose();
        }
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Message for when a graph image is supposed to be saved but can't be
    /// because the workbook hasn't been saved.

    public const String WorkbookNotSavedMessage =
        "You can't save an image to a file because the workbook hasn't been"
        + " saved yet.  (The image is saved to a file with the same file name"
        + " as the workbook, and right now the workbook doesn't have a file"
        + " name.)"
        ;
}

}
