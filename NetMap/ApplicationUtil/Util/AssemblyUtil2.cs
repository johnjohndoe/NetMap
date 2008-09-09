
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Reflection;
using System.Diagnostics;

namespace Microsoft.NetMap.ApplicationUtil
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
    //  Property: ProductVersion
    //
    /// <summary>
	/// Gets the product version.
    /// </summary>
    ///
    /// <value>
	/// The product version, as a string.  Sample: "1.0.1.12".
    /// </value>
    ///
    /// <remarks>
	/// This can be used instead of Application.ProductVersion in cases where
	/// the Application object is not available.  The product version is
	/// obtained from the file version of the executing assembly.
    /// </remarks>
    //*************************************************************************

    public static String
    ProductVersion
    {
		get
		{
			return (FileVersionInfo.GetVersionInfo(
				Assembly.GetExecutingAssembly().Location).FileVersion);
		}
    }
}

}
