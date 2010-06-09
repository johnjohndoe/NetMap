

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Diagnostics;

namespace Microsoft.NodeXL.NetworkServer
{
//*****************************************************************************
//  Class: FileUtil
//
/// <summary>
/// Utility methods for working with files.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class FileUtil
{
    //*************************************************************************
    //  Method: GetOutputFilePath()
    //
    /// <summary>
    /// Gets the full path to an output file.
    /// </summary>
    ///
    /// <param name="startTime">
    /// Time at which the network download started.
    /// </param>
    ///
    /// <param name="networkConfigurationFilePath">
    /// Full path to the network configuration file.  Sample:
    /// "C:\NetworkConfigurations\NetworkConfiguration.xml".
    /// </param>
    ///
    /// <param name="networkFileFolderPath">
    /// The full path to the folder where the network files should be written.
    /// Sample "C:\".
    /// </param>
    ///
    /// <param name="fileNamePrefix">
    /// The prefix to use, or String.Empty for no prefix.  Sample: "Error_".
    /// </param>
    ///
    /// <param name="extension">
    /// The extension to use, without a period.  Sample: "txt".
    /// </param>
    ///
    /// <returns>
    /// The full path to an output file.  (An output file is either a network
    /// file in one of the <see cref="NetworkFileFormats" /> format, or a text
    /// file.)  Sample:
    ///
    /// <para>
    /// C:\Error_NetworkConfiguration_2010-06-01_02-00-00.txt
    /// </para>
    ///
    /// </returns>
    //*************************************************************************

    public static String
    GetOutputFilePath
    (
        DateTime startTime,
        String networkConfigurationFilePath,
        String networkFileFolderPath,
        String fileNamePrefix,
        String extension
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(networkConfigurationFilePath) );
        Debug.Assert( !String.IsNullOrEmpty(networkFileFolderPath) );
        Debug.Assert(fileNamePrefix != null);
        Debug.Assert( !String.IsNullOrEmpty(extension) );

        String sNetworkFileName = GetNetworkFileName(startTime,
            networkConfigurationFilePath);

        return ( Path.ChangeExtension(

            Path.Combine(networkFileFolderPath,
                fileNamePrefix + sNetworkFileName),

            extension
            ) );
    }

    //*************************************************************************
    //  Method: GetNetworkFileName()
    //
    /// <summary>
    /// Gets the name of the file where the network should be written, without
    /// a path or extension.
    /// </summary>
    ///
    /// <param name="oStartTime">
    /// Time at which the network download started.
    /// </param>
    ///
    /// <param name="sNetworkConfigurationFilePath">
    /// Full path to the network configuration file.  Sample:
    /// "C:\NetworkConfigurations\NetworkConfiguration.xml".
    /// </param>
    ///
    /// <returns>
    /// The name of the file where the network should be written, without a
    /// path or extension.  Sample:
    ///
    /// <para>
    /// NetworkConfiguration_2010-06-01_02-00-00
    /// </para>
    ///
    /// </returns>
    //*************************************************************************

    private static String
    GetNetworkFileName
    (
        DateTime oStartTime,
        String sNetworkConfigurationFilePath
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sNetworkConfigurationFilePath) );

        return ( String.Format(
            "{0}_{1}"
            ,
            Path.GetFileNameWithoutExtension(sNetworkConfigurationFilePath),
            oStartTime.ToString("yyyy-MM-dd_HH-mm-ss")
            ) );
    }

}

}
