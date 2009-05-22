
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: Win32Window
//
/// <summary>
/// Wraps a native window handle in an IWin32Window.
/// </summary>
//*****************************************************************************

public class Win32Window : Object, IWin32Window
{
    //*************************************************************************
    //  Constructor: Win32Window()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="Win32Window" /> class.
    /// </summary>
    ///
    /// <param name="hwnd">
    /// Native window handle.
    /// </param>
    //*************************************************************************

    public Win32Window
    (
        Int32 hwnd
    )
    {
        m_oHWnd = new IntPtr(hwnd);

        AssertValid();
    }

    //*************************************************************************
    //  Property: Handle
    //
    /// <summary>
    /// Gets the handle to the window represented by the implementer.
    /// </summary>
    ///
    /// <value>
    /// A handle to the window represented by the implementer.
    /// </value>
    //*************************************************************************

    public IntPtr
    Handle
    {
        get
        {
            AssertValid();

            return (m_oHWnd);
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
        Debug.Assert(m_oHWnd != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Native window handle.

    protected IntPtr m_oHWnd;
}

}
