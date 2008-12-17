
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Reflection;
using System.Diagnostics;

namespace Microsoft.NodeXL.Common
{
//*****************************************************************************
//  Class: AssemblyUtil2
//
/// <summary>
/// Assembly utility methods.
/// </summary>
//*****************************************************************************

public static class AssemblyUtil2 : Object
{
    //*************************************************************************
    //  Method: GetFileVersion()
    //
    /// <summary>
	/// Gets the file version of the executing assembly, as a string.
    /// </summary>
    ///
    /// <value>
	/// The file version of the executing assembly, as a string.  Sample:
	/// "1.0.1.12".
    /// </value>
    ///
    /// <remarks>
	/// This can be used instead of Application.ProductVersion in cases where
	/// the Application object is not available.
    /// </remarks>
    //*************************************************************************

    public static String
    GetFileVersion()
    {
		return (GetFileVersionInfo().FileVersion);
    }

    //*************************************************************************
    //  Method: GetFileVersionInfo()
    //
    /// <summary>
	/// Gets the file version of the executing assembly, as a FileVersionInfo.
    /// </summary>
    ///
    /// <value>
	/// The file version of the executing assembly, as a FileVersionInfo.
    /// </value>
    //*************************************************************************

    public static FileVersionInfo
    GetFileVersionInfo()
    {
		return ( FileVersionInfo.GetVersionInfo(
			Assembly.GetExecutingAssembly().Location) );
    }
}

}
