

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Xml;
using System.IO;
using System.Diagnostics;
using Microsoft.NodeXL.GraphDataProviders;
using Microsoft.NodeXL.GraphDataProviders.Twitter;
using Microsoft.NodeXL.Core;
using Microsoft.SocialNetworkLib;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.NetworkServer
{
//*****************************************************************************
//  Class: Program
//
/// <summary>
/// The application's entry point.
/// </summary>
//*****************************************************************************

class Program
{
    //*************************************************************************
    //  Method: Main()
    //
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    ///
    /// <param name="args">
    /// Command line arguments.
    /// </param>
    //*************************************************************************

    static void
    Main
    (
        string[] args
    )
    {
        try
        {
            RunProgram(args);
        }
        catch (Exception oException)
        {
            Exit(ExitCode.UnexpectedException,

                "An unexpected problem occurred.  Details:"
                + "\r\n\r\n"
                + ExceptionUtil.GetMessageTrace(oException)
                );
        }
    }

    //*************************************************************************
    //  Method: RunProgram()
    //
    /// <summary>
    /// Runs the program.
    /// </summary>
    ///
    /// <param name="args">
    /// Command line arguments.
    /// </param>
    //*************************************************************************

    private static void
    RunProgram
    (
        string[] args
    )
    {
        String sNetworkConfigurationFilePath;

        ParseCommandLine(args, out sNetworkConfigurationFilePath);

        NetworkConfigurationFileParser oNetworkConfigurationFileParser =
            new NetworkConfigurationFileParser();

        NetworkType eNetworkType = NetworkType.TwitterSearch;

        try
        {
            oNetworkConfigurationFileParser.OpenNetworkConfigurationFile(
                sNetworkConfigurationFilePath);

            eNetworkType = oNetworkConfigurationFileParser.GetNetworkType();
        }
        catch (XmlException oXmlException)
        {
            OnNetworkConfigurationFileException(oXmlException);
        }

        XmlDocument oXmlDocument = null;
        String sNetworkFileFolderPath = null;
        DateTime oStartTime = DateTime.Now;
        NetworkFileFormats eNetworkFileFormats = NetworkFileFormats.None;
        Boolean bAutomateNodeXLWorkbook = false;

        switch (eNetworkType)
        {
            case NetworkType.TwitterSearch:

                GetTwitterSearchNetwork(oStartTime,
                    sNetworkConfigurationFilePath,
                    oNetworkConfigurationFileParser, out oXmlDocument,
                    out sNetworkFileFolderPath, out eNetworkFileFormats,
                    out bAutomateNodeXLWorkbook);

                break;

            case NetworkType.TwitterUser:

                GetTwitterUserNetwork(oStartTime,
                    sNetworkConfigurationFilePath,
                    oNetworkConfigurationFileParser, out oXmlDocument,
                    out sNetworkFileFolderPath, out eNetworkFileFormats,
                    out bAutomateNodeXLWorkbook);

                break;

            default:

                Debug.Assert(false);
                break;
        }

        Debug.Assert(oXmlDocument != null);

        SaveNetwork(oStartTime, oXmlDocument, sNetworkConfigurationFilePath,
            sNetworkFileFolderPath, eNetworkFileFormats,
            bAutomateNodeXLWorkbook);

        Exit(ExitCode.Success, null);
    }

    //*************************************************************************
    //  Method: ParseCommandLine()
    //
    /// <summary>
    /// Parses the command line arguments.
    /// </summary>
    ///
    /// <param name="args">
    /// Command line arguments.
    /// </param>
    ///
    /// <param name="sNetworkConfigurationFilePath">
    /// Where the full path of the specified network configuration file gets
    /// stored.
    /// </param>
    ///
    /// <remarks>
    /// If an error is encountered, this method handles it and then exits the
    /// program.
    /// </remarks>
    //*************************************************************************

    private static void
    ParseCommandLine
    (
        string[] args,
        out String sNetworkConfigurationFilePath
    )
    {
        Debug.Assert(args != null);

        sNetworkConfigurationFilePath = null;

        if (args.Length == 1)
        {
            String sArgument = args[0];

            if (sArgument == "/?")
            {
                Console.WriteLine(UsageMessage);
                Exit(ExitCode.Success, null);
            }

            // Note that the validity of the file name is checked by
            // NetworkConfigurationFileParser, not by this class.

            sNetworkConfigurationFilePath = sArgument;
            return;
        }

        Exit(ExitCode.InvalidCommandLineArguments,
            "\r\nThe command line arguments are not valid.\r\n" + UsageMessage
            );
    }

    //*************************************************************************
    //  Method: GetTwitterSearchNetwork()
    //
    /// <summary>
    /// Gets a Twitter search network.
    /// </summary>
    ///
    /// <param name="oStartTime">
    /// Time at which the network download started.
    /// </param>
    ///
    /// <param name="sNetworkConfigurationFilePath">
    /// Full path to the network configuration file.
    /// </param>
    ///
    /// <param name="oNetworkConfigurationFileParser">
    /// Parses a network configuration file.
    /// </param>
    ///
    /// <param name="oXmlDocument">
    /// Where the XML document containing the network as GraphML gets stored.
    /// </param>
    ///
    /// <param name="sNetworkFileFolderPath">
    /// Where the full path to the folder where the network files should be
    /// written gets stored.
    /// </param>
    ///
    /// <param name="eNetworkFileFormats">
    /// Where the file formats to save the network to get stored.
    /// </param>
    ///
    /// <param name="bAutomateNodeXLWorkbook">
    /// Where a flag specifying whether automation should be run on the NodeXL
    /// workbook gets stored.
    /// </param>
    ///
    /// <remarks>
    /// If an error is encountered, this method handles it and then exits the
    /// program.
    /// </remarks>
    //*************************************************************************

    private static void
    GetTwitterSearchNetwork
    (
        DateTime oStartTime,
        String sNetworkConfigurationFilePath,
        NetworkConfigurationFileParser oNetworkConfigurationFileParser,
        out XmlDocument oXmlDocument,
        out String sNetworkFileFolderPath,
        out NetworkFileFormats eNetworkFileFormats,
        out Boolean bAutomateNodeXLWorkbook
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sNetworkConfigurationFilePath) );
        Debug.Assert(oNetworkConfigurationFileParser != null);

        oXmlDocument = null;
        sNetworkFileFolderPath = null;
        eNetworkFileFormats = NetworkFileFormats.None;
        bAutomateNodeXLWorkbook = false;

        String sSearchTerm = null;

        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude =
            TwitterSearchNetworkAnalyzer.WhatToInclude.None;

        Int32 iMaximumPeoplePerRequest = Int32.MaxValue;

        try
        {
            oNetworkConfigurationFileParser.
                GetTwitterSearchNetworkConfiguration(out sSearchTerm,
                out eWhatToInclude, out iMaximumPeoplePerRequest,
                out sNetworkFileFolderPath, out eNetworkFileFormats,
                out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            // (This call exits the program.)

            OnNetworkConfigurationFileException(oXmlException);
        }

        TwitterSearchNetworkAnalyzer oTwitterSearchNetworkAnalyzer =
            new TwitterSearchNetworkAnalyzer();

        Console.WriteLine(
            "Getting the Twitter Search network specified in \"{0}\".  The"
            + " search term is \"{1}\"."
            ,
            sNetworkConfigurationFilePath,
            sSearchTerm
            );

        try
        {
            oXmlDocument = oTwitterSearchNetworkAnalyzer.GetNetwork(
                sSearchTerm, eWhatToInclude, iMaximumPeoplePerRequest);
        }
        catch (PartialNetworkException oPartialNetworkException)
        {
            oXmlDocument = OnGetNetworkPartialNetworkException(
                oStartTime, oPartialNetworkException,
                sNetworkConfigurationFilePath, sNetworkFileFolderPath,
                oTwitterSearchNetworkAnalyzer);
        }
        catch (Exception oException)
        {
            // (This call exits the program.)

            OnGetNetworkOtherException(oStartTime, oException,
                sNetworkConfigurationFilePath, sNetworkFileFolderPath,
                oTwitterSearchNetworkAnalyzer);
        }
    }

    //*************************************************************************
    //  Method: GetTwitterUserNetwork()
    //
    /// <summary>
    /// Gets a Twitter user network.
    /// </summary>
    ///
    /// <param name="oStartTime">
    /// Time at which the network download started.
    /// </param>
    ///
    /// <param name="sNetworkConfigurationFilePath">
    /// Full path to the network configuration file.
    /// </param>
    ///
    /// <param name="oNetworkConfigurationFileParser">
    /// Parses a network configuration file.
    /// </param>
    ///
    /// <param name="oXmlDocument">
    /// Where the XML document containing the network as GraphML gets stored.
    /// </param>
    ///
    /// <param name="sNetworkFileFolderPath">
    /// Where the full path to the folder where the network files should be
    /// written gets stored.
    /// </param>
    ///
    /// <param name="eNetworkFileFormats">
    /// Where the file formats to save the network to get stored.
    /// </param>
    ///
    /// <param name="bAutomateNodeXLWorkbook">
    /// Where a flag specifying whether automation should be run on the NodeXL
    /// workbook gets stored.
    /// </param>
    ///
    /// <remarks>
    /// If an error is encountered, this method handles it and then exits the
    /// program.
    /// </remarks>
    //*************************************************************************

    private static void
    GetTwitterUserNetwork
    (
        DateTime oStartTime,
        String sNetworkConfigurationFilePath,
        NetworkConfigurationFileParser oNetworkConfigurationFileParser,
        out XmlDocument oXmlDocument,
        out String sNetworkFileFolderPath,
        out NetworkFileFormats eNetworkFileFormats,
        out Boolean bAutomateNodeXLWorkbook
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sNetworkConfigurationFilePath) );
        Debug.Assert(oNetworkConfigurationFileParser != null);

        oXmlDocument = null;
        sNetworkFileFolderPath = null;
        eNetworkFileFormats = NetworkFileFormats.None;
        bAutomateNodeXLWorkbook = false;

        String sScreenNameToAnalyze = null;

        TwitterUserNetworkAnalyzer.WhatToInclude eWhatToInclude =
            TwitterUserNetworkAnalyzer.WhatToInclude.None;

        NetworkLevel eNetworkLevel = NetworkLevel.One;
        Int32 iMaximumPeoplePerRequest = Int32.MaxValue;

        try
        {
            oNetworkConfigurationFileParser.GetTwitterUserNetworkConfiguration(
                out sScreenNameToAnalyze, out eWhatToInclude,
                out eNetworkLevel, out iMaximumPeoplePerRequest,
                out sNetworkFileFolderPath, out eNetworkFileFormats,
                out bAutomateNodeXLWorkbook);
        }
        catch (XmlException oXmlException)
        {
            // (This call exits the program.)

            OnNetworkConfigurationFileException(oXmlException);
        }

        TwitterUserNetworkAnalyzer oTwitterUserNetworkAnalyzer =
            new TwitterUserNetworkAnalyzer();

        Console.WriteLine(
            "Getting the Twitter User network specified in \"{0}\".  The"
            + " screen name is \"{1}\"."
            ,
            sNetworkConfigurationFilePath,
            sScreenNameToAnalyze
            );

        try
        {
            oXmlDocument = oTwitterUserNetworkAnalyzer.GetNetwork(
                sScreenNameToAnalyze, eWhatToInclude, eNetworkLevel,
                iMaximumPeoplePerRequest);
        }
        catch (PartialNetworkException oPartialNetworkException)
        {
            oXmlDocument = OnGetNetworkPartialNetworkException(oStartTime,
                oPartialNetworkException, sNetworkConfigurationFilePath,
                sNetworkFileFolderPath, oTwitterUserNetworkAnalyzer);
        }
        catch (Exception oException)
        {
            // (This call exits the program.)

            OnGetNetworkOtherException(oStartTime, oException,
                sNetworkConfigurationFilePath, sNetworkFileFolderPath,
                oTwitterUserNetworkAnalyzer);
        }
    }

    //*************************************************************************
    //  Method: SaveNetwork()
    //
    /// <summary>
    /// Saves a network to disk.
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
    /// <param name="eNetworkFileFormats">
    /// The file formats to save the network to.
    /// </param>
    ///
    /// <param name="bAutomateNodeXLWorkbook">
    /// true to run automation on the NodeXL workbook, if eNetworkFileFormats
    /// includes NetworkFileFormats.NodeXLWorkbook.
    /// </param>
    //*************************************************************************

    private static void
    SaveNetwork
    (
        DateTime oStartTime,
        XmlDocument oXmlDocument,
        String sNetworkConfigurationFilePath,
        String sNetworkFileFolderPath,
        NetworkFileFormats eNetworkFileFormats,
        Boolean bAutomateNodeXLWorkbook
    )
    {
        Debug.Assert(oXmlDocument != null);
        Debug.Assert( !String.IsNullOrEmpty(sNetworkConfigurationFilePath) );
        Debug.Assert( !String.IsNullOrEmpty(sNetworkFileFolderPath) );

        if ( (eNetworkFileFormats & NetworkFileFormats.GraphML)
            != NetworkFileFormats.None )
        {
            SaveNetworkToGraphML(oStartTime, oXmlDocument,
                sNetworkConfigurationFilePath, sNetworkFileFolderPath);
        }

        if ( (eNetworkFileFormats & NetworkFileFormats.NodeXLWorkbook)
            != NetworkFileFormats.None )
        {
            SaveNetworkToNodeXLWorkbook(oStartTime, oXmlDocument,
                sNetworkConfigurationFilePath, sNetworkFileFolderPath,
                bAutomateNodeXLWorkbook);
        }
    }

    //*************************************************************************
    //  Method: SaveNetworkToGraphML()
    //
    /// <summary>
    /// Saves a network to a GraphML file.
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
    //*************************************************************************

    private static void
    SaveNetworkToGraphML
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

        Console.WriteLine(
            "Saving the network to a GraphML file."
            );

        // Sample network file path:
        //
        // C:\NetworkConfiguration_2010-06-01_02-00-00.graphml
        
        String sNetworkFilePath = FileUtil.GetOutputFilePath(oStartTime,
            sNetworkConfigurationFilePath, sNetworkFileFolderPath,
            String.Empty, "graphml");

        try
        {
            oXmlDocument.Save(sNetworkFilePath);
        }
        catch (IOException oIOException)
        {
            Exit(ExitCode.SaveNetworkFileError,
                "The file couldn't be saved.  Details:\r\n\r\n"
                + oIOException.Message
                );
        }
    }

    //*************************************************************************
    //  Method: SaveNetworkToNodeXLWorkbook()
    //
    /// <summary>
    /// Saves a network to a NodeXL workbook.
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
    /// <param name="bAutomateNodeXLWorkbook">
    /// true to run automation on the NodeXL workbook.
    /// </param>
    //*************************************************************************

    private static void
    SaveNetworkToNodeXLWorkbook
    (
        DateTime oStartTime,
        XmlDocument oXmlDocument,
        String sNetworkConfigurationFilePath,
        String sNetworkFileFolderPath,
        Boolean bAutomateNodeXLWorkbook
    )
    {
        Debug.Assert(oXmlDocument != null);
        Debug.Assert( !String.IsNullOrEmpty(sNetworkConfigurationFilePath) );
        Debug.Assert( !String.IsNullOrEmpty(sNetworkFileFolderPath) );

        Console.WriteLine(
            "Saving the network to a NodeXL workbook."
            );

        try
        {
            NodeXLWorkbookSaver.SaveGraphToNodeXLWorkbook(oStartTime,
                oXmlDocument, sNetworkConfigurationFilePath,
                sNetworkFileFolderPath, bAutomateNodeXLWorkbook);
        }
        catch (SaveGraphToNodeXLWorkbookException
            oSaveGraphToNodeXLWorkbookException)
        {
            Exit(oSaveGraphToNodeXLWorkbookException.ExitCode, 
                oSaveGraphToNodeXLWorkbookException.Message);
        }
    }

    //*************************************************************************
    //  Method: OnNetworkConfigurationFileException()
    //
    /// <summary>
    /// Handles an exception thrown while parsing the network configuration
    /// file.
    /// </summary>
    ///
    /// <param name="oXmlException">
    /// The exception that was thrown.
    /// </param>
    ///
    /// <remarks>
    /// The NetworkConfigurationFileParser wraps all parsing errors in
    /// XmlExceptions.
    ///
    /// <para>
    /// This method exits the program.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    private static void
    OnNetworkConfigurationFileException
    (
        XmlException oXmlException
    )
    {
        Debug.Assert(oXmlException != null);

        Exit(ExitCode.InvalidNetworkConfigurationFile,
            "\r\n" + oXmlException.Message + "\r\n" + UsageMessage
            );
    }

    //*************************************************************************
    //  Method: OnGetNetworkPartialNetworkException()
    //
    /// <summary>
    /// Handles a PartialNetworkException thrown while getting a network.
    /// </summary>
    ///
    /// <param name="oStartTime">
    /// Time at which the network download started.
    /// </param>
    ///
    /// <param name="oPartialNetworkException">
    /// The exception that was thrown.
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
    /// <param name="oHttpNetworkAnalyzerBase">
    /// The network analyzer used to get the network.
    /// </param>
    ///
    /// <returns>
    /// The partial network, as GraphML.
    /// </returns>
    ///
    /// <remarks>
    /// This method exits the program.
    /// </remarks>
    //*************************************************************************

    private static XmlDocument
    OnGetNetworkPartialNetworkException
    (
        DateTime oStartTime,
        PartialNetworkException oPartialNetworkException,
        String sNetworkConfigurationFilePath,
        String sNetworkFileFolderPath,
        HttpNetworkAnalyzerBase oHttpNetworkAnalyzerBase
    )
    {
        Debug.Assert(oPartialNetworkException != null);
        Debug.Assert( !String.IsNullOrEmpty(sNetworkConfigurationFilePath) );
        Debug.Assert( !String.IsNullOrEmpty(sNetworkFileFolderPath) );
        Debug.Assert(oHttpNetworkAnalyzerBase != null);

        // Write a text file to let the user know that a partial network was
        // obtained.
        //
        // Sample file path:
        //
        // C:\PartialNetwork_NetworkConfiguration_2010-06-01_02-00-00.txt

        String sFilePath = FileUtil.GetOutputFilePath(oStartTime,
            sNetworkConfigurationFilePath, sNetworkFileFolderPath,
            "PartialNetworkInfo_", "txt");

        using ( StreamWriter oStreamWriter = new StreamWriter(sFilePath) )
        {
            Debug.Assert(oHttpNetworkAnalyzerBase != null);

            oStreamWriter.WriteLine( oPartialNetworkException.ToMessage(
                oHttpNetworkAnalyzerBase.ExceptionToMessage(
                    oPartialNetworkException.RequestStatistics.
                        LastUnexpectedException)
                ) );
        }

        return (oPartialNetworkException.PartialNetwork);
    }

    //*************************************************************************
    //  Method: OnGetNetworkOtherException()
    //
    /// <summary>
    /// Handles an exception (other than a PartialNetworkException) thrown
    /// while getting a network.
    /// </summary>
    ///
    /// <param name="oStartTime">
    /// Time at which the network download started.
    /// </param>
    ///
    /// <param name="oException">
    /// The exception that was thrown.
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
    /// <param name="oHttpNetworkAnalyzerBase">
    /// The network analyzer used to get the network.
    /// </param>
    ///
    /// <remarks>
    /// This method exits the program.
    /// </remarks>
    //*************************************************************************

    private static void
    OnGetNetworkOtherException
    (
        DateTime oStartTime,
        Exception oException,
        String sNetworkConfigurationFilePath,
        String sNetworkFileFolderPath,
        HttpNetworkAnalyzerBase oHttpNetworkAnalyzerBase
    )
    {
        Debug.Assert(oException != null);
        Debug.Assert( !String.IsNullOrEmpty(sNetworkConfigurationFilePath) );
        Debug.Assert( !String.IsNullOrEmpty(sNetworkFileFolderPath) );
        Debug.Assert(oHttpNetworkAnalyzerBase != null);

        // Sample error file path:
        //
        // C:\Error_NetworkConfiguration_2010-06-01_02-00-00.txt

        String sErrorFilePath = FileUtil.GetOutputFilePath(oStartTime,
            sNetworkConfigurationFilePath, sNetworkFileFolderPath,
            "Error_", "txt");

        String sErrorMessage =
            "The network couldn't be obtained.  Details:"
            + "\r\n\r\n"
            + oHttpNetworkAnalyzerBase.ExceptionToMessage(oException)
            ;

        using ( StreamWriter oStreamWriter = new StreamWriter(sErrorFilePath) )
        {
            Debug.Assert(oHttpNetworkAnalyzerBase != null);

            oStreamWriter.WriteLine(sErrorMessage);
        }

        Exit(ExitCode.CouldNotGetNetwork, sErrorMessage);
    }

    //*************************************************************************
    //  Method: Exit()
    //
    /// <summary>
    /// Exits the program with a specified exit code.
    /// </summary>
    ///
    /// <param name="eExitCode">
    /// The program exit code to use.
    /// </param>
    ///
    /// <param name="sErrorMessage">
    /// The error message to send to the standard error stream, or null for no
    /// message..
    /// </param>
    //*************************************************************************

    private static void
    Exit
    (
        ExitCode eExitCode,
        String sErrorMessage
    )
    {
        if ( !String.IsNullOrEmpty(sErrorMessage) )
        {
            Console.Error.WriteLine(sErrorMessage);
        }

        Environment.Exit( (Int32)eExitCode );
    }


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// Program usage message.

    private const String UsageMessage =

        "\r\n"
        + "For information on how to use this program, see the"
        + " NodeXLNetworkServerFAQ.docx"
        + "\r\n"
        + "document.  It can be found in the same folder that contains the"
        + " program."
        ;
}

}
