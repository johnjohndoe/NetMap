
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NodeXL.ExcelTemplatePlugIns;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: PlugInManager
//
/// <summary>
/// Provides access to plug-in classes.
/// </summary>
///
/// <remarks>
/// Call <see cref="GetGraphDataProviders" /> to get an array plug-in classes
/// that implement <see cref="IGraphDataProvider" />.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class PlugInManager
{
    //*************************************************************************
    //  Method: GetGraphDataProviders()
    //
    /// <summary>
    /// Gets an array of plug-in classes that implement the <see
    /// cref="IGraphDataProvider" /> interface.
    /// </summary>
    ///
    /// <returns>
    /// An array of zero or more <see cref="IGraphDataProvider" />
    /// implementations.
    /// </returns>
    ///
    /// <remarks>
    /// The <see cref="IGraphDataProvider" /> interface allows developers to
    /// create plug-in classes that import graph data into the NodeXL Excel
    /// Template without having to modify the ExcelTemplate's source code.  See
    /// <see cref="IGraphDataProvider" /> for more information.
    /// </remarks>
    //*************************************************************************

    public static IGraphDataProvider []
    GetGraphDataProviders()
    {
        List<IGraphDataProvider> oGraphDataProviders =
            new List<IGraphDataProvider>();

        IEnumerable<String> oFileNames;

        if ( TryGetFilesInPlugInFolder(out oFileNames) )
        {
            foreach (String sFileName in oFileNames)
            {
                // The techniques for checking types for a specified interface
                // and instantiating instances of those types are from the
                // article "Let Users Add Functionality to Your .NET
                // Applications with Macros and Plug-Ins" in the October 2003
                // issue of MSDN Magazine.

                Type [] aoTypes;

                if ( !TryGetTypesFromFile(sFileName, out aoTypes) )
                {
                    continue;
                }

                foreach (Type oType in aoTypes)
                {
                    if ( !oType.IsAbstract &&
                        typeof(IGraphDataProvider).IsAssignableFrom(oType) )
                    {
                        oGraphDataProviders.Add( (IGraphDataProvider)
                            Activator.CreateInstance(oType) );
                    }
                }
            }
        }

        oGraphDataProviders.Sort(
            delegate
            (
                IGraphDataProvider oGraphDataProvider1,
                IGraphDataProvider oGraphDataProvider2
            )
            {
                Debug.Assert(oGraphDataProvider1 != null);
                Debug.Assert(oGraphDataProvider2 != null);
                Debug.Assert( !String.IsNullOrEmpty(oGraphDataProvider1.Name) );
                Debug.Assert( !String.IsNullOrEmpty(oGraphDataProvider2.Name) );

                return ( oGraphDataProvider1.Name.CompareTo(
                    oGraphDataProvider2.Name) );
            }
            );

        return ( oGraphDataProviders.ToArray() );
    }

    //*************************************************************************
    //  Method: TryGetFilesInPlugInFolder()
    //
    /// <summary>
    /// Attempts to get the full paths to the files in the folder where plug-in
    /// assemblies are stored.
    /// </summary>
    ///
    /// <param name="oFileNames">
    /// Where the full paths get stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the full paths were obtained.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryGetFilesInPlugInFolder
    (
        out IEnumerable<String> oFileNames
    )
    {
        oFileNames = null;

        String sPlugInFolder = GetPlugInFolder();

        if ( !Directory.Exists(sPlugInFolder) )
        {
            return (false);
        }

        List<String> oFileNameList = new List<String>();

        foreach ( String sSearchPattern in new String[] {"*.dll", "*.exe"} )
        {
            oFileNameList.AddRange( Directory.GetFiles(
                sPlugInFolder, sSearchPattern) );
        }

        oFileNames = oFileNameList;

        return (true);
    }

    //*************************************************************************
    //  Method: GetPlugInFolder()
    //
    /// <summary>
    /// Gets the full path to folder where plug-in assemblies are stored.
    /// </summary>
    ///
    /// <returns>
    /// The full path to folder where plug-in assemblies are stored.
    /// </returns>
    //*************************************************************************

    private static String
    GetPlugInFolder()
    {
        String sAssemblyPath = Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().CodeBase);

        if ( sAssemblyPath.StartsWith("file:") )
        {
            sAssemblyPath = sAssemblyPath.Substring(6);
        }

        return ( Path.Combine(sAssemblyPath, PlugInSubfolder) );
    }

    //*************************************************************************
    //  Method: TryGetTypesFromFile()
    //
    /// <summary>
    /// Attempts to get an array of types implemented by an assembly.
    /// </summary>
    ///
    /// <param name="sPath">
    /// Full path to a file that might be an assembly.
    /// </param>
    ///
    /// <param name="aoTypes">
    /// Where the array of types implemented by the assembly gets stored if
    /// true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the types were obtained.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryGetTypesFromFile
    (
        String sPath,
        out Type [] aoTypes
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sPath) );

        aoTypes = null;

        Assembly oAssembly;

        try
        {
            oAssembly = Assembly.LoadFile(sPath);
        }
        catch (FileLoadException)
        {
            return (false);
        }
        catch (BadImageFormatException)
        {
            return (false);
        }

        try
        {
            aoTypes = oAssembly.GetTypes();
        }
        catch (ReflectionTypeLoadException)
        {
            // This occurs when the loaded assembly has dependencies in an
            // assembly that hasn't been loaded.

            return (false);
        }

        return (true);
    }


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// Subfolder under the application folder where plug-in assemblies are
    /// stored.

    private const String PlugInSubfolder = "PlugIns";
}
}
