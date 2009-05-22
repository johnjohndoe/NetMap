

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: Win32Functions
//
/// <summary>
/// Contains static methods that call Win32 functions via PInvoke.
/// </summary>
//*****************************************************************************

public static class Win32Functions
{
    //*************************************************************************
    //  Method: SetForegroundWindow
    //
    /// <summary>
    /// Calls the SetForegroundWindow Win32 function.
    /// </summary>
    ///
    /// <param name="hWnd">
    /// The window to set to the foreground.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]

    public static extern bool SetForegroundWindow(IntPtr hWnd);


    //*************************************************************************
    //  Private fields
    //*************************************************************************

    // (None.)
}

}
