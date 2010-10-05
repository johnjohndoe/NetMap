

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Xml;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Adapters;
using Microsoft.NodeXL.ExcelTemplate;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.NetworkServer
{
//*****************************************************************************
//  Class: NodeXLWorkbookSaver
//
/// <summary>
/// Saves a graph to a NodeXL Excel workbook.
/// </summary>
//*****************************************************************************

class NodeXLWorkbookSaver
{
    //*************************************************************************
    //  Method: SaveGraphToNodeXLWorkbook()
    //
    /// <overloads>
    /// Saves a graph to a NodeXL Excel workbook.
    /// </overloads>
    ///
    /// <summary>
    /// Saves a graph to a NodeXL Excel workbook.
    /// </summary>
    ///
    /// <param name="startTime">
    /// Time at which the network download started.
    /// </param>
    ///
    /// <param name="xmlDocument">
    /// The XML document containing the network as GraphML.
    /// </param>
    ///
    /// <param name="networkConfigurationFilePath">
    /// The path of the specified network configuration file.
    /// </param>
    ///
    /// <param name="networkFileFolderPath">
    /// The full path to the folder where the network files should be written.
    /// </param>
    ///
    /// <param name="automate">
    /// True to automate the NodeXL workbook.
    /// </param>
    ///
    /// <remarks>
    /// If an error occurs, a <see cref="SaveGraphToNodeXLWorkbookException" />
    /// is thrown.
    /// </remarks>
    //*************************************************************************

    public static void
    SaveGraphToNodeXLWorkbook
    (
        DateTime startTime,
        XmlDocument xmlDocument,
        String networkConfigurationFilePath,
        String networkFileFolderPath,
        Boolean automate
    )
    {
        Debug.Assert(xmlDocument != null);
        Debug.Assert( !String.IsNullOrEmpty(networkConfigurationFilePath) );
        Debug.Assert( !String.IsNullOrEmpty(networkFileFolderPath) );

        // Open Excel.

        Application oExcelApplication = new Application();

        if (oExcelApplication == null)
        {
            throw new SaveGraphToNodeXLWorkbookException(
                ExitCode.CouldNotOpenExcel,
                "Excel couldn't be opened.  Is it installed on this computer?"
                );
        }

        oExcelApplication.Visible = true;

        ExcelApplicationKiller oExcelApplicationKiller =
            new ExcelApplicationKiller(oExcelApplication);

        String sWorkbookPath;

        try
        {
            SaveGraphToNodeXLWorkbook(startTime, xmlDocument,
                networkConfigurationFilePath, networkFileFolderPath, automate,
                oExcelApplication, out sWorkbookPath);
        }
        finally
        {
            // Supress any dialog boxes warning about unsaved files.

            oExcelApplication.DisplayAlerts = false;

            oExcelApplication.Quit();

            // Make sure the Excel application is removed from memory.
            //
            // (On development machines, the Excel application's process is
            // killed when this executable closes.  Bugs reported by others,
            // however, suggest that the Excel instance can remain in memory
            // even after this executable closes.  Work around this apparent
            // bug.)

            oExcelApplicationKiller.KillExcelApplication();
        }

        if (automate)
        {
            // SaveGraphToNodeXLWorkbook() stored an "automate tasks on open"
            // flag in the workbook, indicating that task automation should be
            // run on it the next time it's opened.  Open it, then wait for it
            // to close.

            Console.WriteLine(
                "Automating the NodeXL workbook."
                );

            try
            {
                ExcelTemplate.TaskAutomator.OpenWorkbookToAutomate(
                    sWorkbookPath);
            }
            catch (Exception oException)
            {
                OnException(oException,
                    ExitCode.CouldNotAutomateNodeXLWorkbook,
                    "The NodeXL workbook couldn't be opened to automate it."
                    );
            }
        }
    }

    //*************************************************************************
    //  Method: SaveGraphToNodeXLWorkbook()
    //
    /// <summary>
    /// Saves a graph to a NodeXL Excel workbook given an Excel Application
    /// object.
    /// </summary>
    ///
    /// <param name="oStartTime">
    /// Time at which the network download started.
    /// </param>
    ///
    /// <param name="oXmlDocument">
    /// The XML document containing the network as GraphML.
    /// </param>
    ///
    /// <param name="sNetworkConfigurationFilePath">
    /// The path of the specified network configuration file.
    /// </param>
    ///
    /// <param name="sNetworkFileFolderPath">
    /// The full path to the folder where the network files should be written.
    /// </param>
    ///
    /// <param name="bAutomate">
    /// True to automate the NodeXL workbook.
    /// </param>
    ///
    /// <param name="oExcelApplication">
    /// An open Excel Application object.
    /// </param>
    ///
    /// <param name="sWorkbookPath">
    /// Where the path to the saved workbook gets stored.
    /// </param>
    ///
    /// <remarks>
    /// If an error occurs, a <see cref="SaveGraphToNodeXLWorkbookException" />
    /// is thrown.
    /// </remarks>
    //*************************************************************************

    private static void
    SaveGraphToNodeXLWorkbook
    (
        DateTime oStartTime,
        XmlDocument oXmlDocument,
        String sNetworkConfigurationFilePath,
        String sNetworkFileFolderPath,
        Boolean bAutomate,
        Application oExcelApplication,
        out String sWorkbookPath
    )
    {
        Debug.Assert(oXmlDocument != null);
        Debug.Assert( !String.IsNullOrEmpty(sNetworkConfigurationFilePath) );
        Debug.Assert( !String.IsNullOrEmpty(sNetworkFileFolderPath) );
        Debug.Assert(oExcelApplication != null);

        // Create a new workbook from the NodeXL template.

        String sNodeXLTemplatePath;

        if ( !ExcelTemplate.ApplicationUtil.TryGetTemplatePath(
            out sNodeXLTemplatePath) )
        {
            throw new SaveGraphToNodeXLWorkbookException(
                ExitCode.CouldNotFindNodeXLTemplate, String.Format(

                "The NodeXL Excel template file couldn't be found.  It's"
                + " supposed to be at {0}, but it's not there.  Is the NodeXL"
                + " Excel Template installed?  It's required to run this"
                + " program."
                ,
                sNodeXLTemplatePath
                ) );
        }

        Workbook oNodeXLWorkbook = null;

        try
        {
            oNodeXLWorkbook = oExcelApplication.Workbooks.Add(
                sNodeXLTemplatePath);
        }
        catch (Exception oException)
        {
            OnException(oException,
                ExitCode.CouldNotCreateNodeXLWorkbook,
                "A NodeXL workbook couldn't be created."
                );
        }

        // Create a NodeXL graph from the XML document.

        IGraph oGraph = ( new GraphMLGraphAdapter() ).LoadGraphFromString(
            oXmlDocument.OuterXml);

        try
        {
            // Import the graph into the workbook.
            //
            // Note that the GraphMLGraphAdapter stored String arrays on the
            // IGraph object that specify the names of the attributes that it
            // added to the graph's edges and vertices.  These get used by the
            // ImportGraph method to determine which columns need to be added
            // to the edge and vertex worksheets.

            GraphImporter oGraphImporter = new GraphImporter();

            oGraphImporter.ImportGraph(oGraph,

                ( String[] )oGraph.GetRequiredValue(
                    ReservedMetadataKeys.AllEdgeMetadataKeys,
                    typeof( String[] ) ),

                ( String[] )oGraph.GetRequiredValue(
                    ReservedMetadataKeys.AllVertexMetadataKeys,
                    typeof( String[] ) ),

                false, oNodeXLWorkbook);

            // Store the graph's directedness in the workbook.

            PerWorkbookSettings oPerWorkbookSettings =
                new ExcelTemplate.PerWorkbookSettings(oNodeXLWorkbook);

            oPerWorkbookSettings.GraphDirectedness = oGraph.Directedness;

            if (bAutomate)
            {
                // Store an "automate tasks on open" flag in the workbook,
                // indicating that task automation should be run on it the next
                // time it's opened.  (It is up to the caller of this method to
                // open the workbook to trigger automation.)

                oPerWorkbookSettings.AutomateTasksOnOpen = true;
            }

        }
        catch (Exception oException)
        {
            OnException(oException,
                ExitCode.CouldNotImportGraphIntoNodeXLWorkbook,
                "The network couldn't be imported into the NodeXL workbook."
                );
        }

        // Save the workbook.  Sample workbook path:
        //
        // C:\NetworkConfiguration_2010-06-01_02-00-00.xlsx
        
        sWorkbookPath = FileUtil.GetOutputFilePath(oStartTime,
            sNetworkConfigurationFilePath, sNetworkFileFolderPath,
            String.Empty, "xlsx");

        Console.WriteLine(
            "Saving the network to the NodeXL workbook \"{0}\"."
            ,
            sWorkbookPath
            );

        try
        {
            oNodeXLWorkbook.SaveAs(sWorkbookPath, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value);
        }
        catch (Exception oException)
        {
            OnException(oException, ExitCode.SaveNetworkFileError,
                "The NodeXL workbook couldn't be saved."
                );
        }

        try
        {
            oNodeXLWorkbook.Close(false, Missing.Value, Missing.Value);
        }
        catch (Exception oException)
        {
            OnException(oException, ExitCode.SaveNetworkFileError,
                "The NodeXL workbook couldn't be closed."
                );
        }
    }

    //*************************************************************************
    //  Method: OnException()
    //
    /// <summary>
    /// Handles an exception that was caught while attempting to save a graph
    /// to a NodeXL Excel workbook.
    /// </summary>
    ///
    /// <param name="oException">
    /// The exception that was caught.
    /// </param>
    ///
    /// <param name="eExitCode">
    /// The program exit code to use.
    /// </param>
    ///
    /// <param name="sErrorMessage">
    /// Error message, suitable for showing in the UI.
    /// </param>
    ///
    /// <remarks>
    /// This method throws a <see cref="SaveGraphToNodeXLWorkbookException" />.
    /// </remarks>
    //*************************************************************************

    private static void
    OnException
    (
        Exception oException,
        ExitCode eExitCode,
        String sErrorMessage
    )
    {
        Debug.Assert(oException != null);
        Debug.Assert( !String.IsNullOrEmpty(sErrorMessage) );

        throw new SaveGraphToNodeXLWorkbookException(eExitCode,

            String.Format(

                "{0}  Details:"
                + "\r\n\r\n"
                + "{1}."
                ,
                sErrorMessage,
                oException.Message
                ) );
    }
}

}
