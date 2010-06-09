

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
    /// <remarks>
    /// If an error occurs, a <see cref="SaveGraphToNodeXLWorkbookException" />
    /// is thrown.
    /// </remarks>
    //*************************************************************************

    public static void
    SaveGraphToNodeXLWorkbook
    (
        DateTime oStartTime,
        XmlDocument oXmlDocument,
        String sNetworkConfigurationFilePath,
        String sNetworkFileFolderPath
    )
    {
        Debug.Assert(oXmlDocument != null);
        Debug.Assert( !String.IsNullOrEmpty(sNetworkConfigurationFilePath) );
        Debug.Assert( !String.IsNullOrEmpty(sNetworkFileFolderPath) );

        // Open Excel.

        Application oExcelApplication = new Application();

        if (oExcelApplication == null)
        {
            throw new SaveGraphToNodeXLWorkbookException(
                ExitCode.CouldNotOpenExcel,
                "Excel couldn't be opened.  Is it installed on this computer?"
                );
        }

        // oExcelApplication.Visible = true;

        try
        {
            SaveGraphToNodeXLWorkbook(oStartTime, oXmlDocument,
                sNetworkConfigurationFilePath, sNetworkFileFolderPath,
                oExcelApplication);
        }
        finally
        {
            // Supress any dialog boxes warning about unsaved files.

            oExcelApplication.DisplayAlerts = false;

            oExcelApplication.Quit();
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
    /// <param name="oExcelApplication">
    /// An open Excel Application object.
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
        DateTime oStartTime,
        XmlDocument oXmlDocument,
        String sNetworkConfigurationFilePath,
        String sNetworkFileFolderPath,
        Application oExcelApplication
    )
    {
        Debug.Assert(oXmlDocument != null);
        Debug.Assert( !String.IsNullOrEmpty(sNetworkConfigurationFilePath) );
        Debug.Assert( !String.IsNullOrEmpty(sNetworkFileFolderPath) );
        Debug.Assert(oExcelApplication != null);

        // Create a new workbook from the NodeXL template.

        String sNodeXLTemplatePath;

        if ( !ExcelTemplate.ApplicationUtil.TryGetTemplatePath(
            oExcelApplication, out sNodeXLTemplatePath) )
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
        catch (COMException oCOMException)
        {
            OnCOMException(oCOMException,
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

            ( new ExcelTemplate.PerWorkbookSettings(oNodeXLWorkbook) ).
                GraphDirectedness = oGraph.Directedness;
        }
        catch (COMException oCOMException)
        {
            OnCOMException(oCOMException,
                ExitCode.CouldNotImportGraphIntoNodeXLWorkbook,
                "The network couldn't be imported into the NodeXL workbook."
                );
        }

        // Save the workbook.  Sample network file path:
        //
        // C:\NetworkConfiguration_2010-06-01_02-00-00.xlsx
        
        String sNetworkFilePath = FileUtil.GetOutputFilePath(oStartTime,
            sNetworkConfigurationFilePath, sNetworkFileFolderPath,
            String.Empty, "xlsx");

        try
        {
            oNodeXLWorkbook.SaveAs(sNetworkFilePath, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value);
        }
        catch (COMException oCOMException)
        {
            OnCOMException(oCOMException, ExitCode.SaveNetworkFileError,
                "The NodeXL workbook couldn't be saved."
                );
        }
    }

    //*************************************************************************
    //  Method: OnCOMException()
    //
    /// <summary>
    /// Handles a COMException that was caught while working with Excel.
    /// </summary>
    ///
    /// <param name="oCOMException">
    /// The COMException that was caught.
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
    OnCOMException
    (
        COMException oCOMException,
        ExitCode eExitCode,
        String sErrorMessage
    )
    {
        Debug.Assert(oCOMException != null);
        Debug.Assert( !String.IsNullOrEmpty(sErrorMessage) );

        throw new SaveGraphToNodeXLWorkbookException(eExitCode,

            String.Format(

                "{0}  Details:"
                + "\r\n\r\n"
                + "{1}."
                ,
                sErrorMessage,
                oCOMException.Message
                ) );
    }
}

}
