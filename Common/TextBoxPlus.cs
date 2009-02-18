
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: TextBoxPlus
//
/// <summary>
/// Represents a TextBox with additional features.
/// </summary>
//*****************************************************************************

public class TextBoxPlus : TextBox
{
    //*************************************************************************
    //  Constructor: TextBoxPlus()
    //
    /// <summary>
    /// Initializes a new instance of the TextBoxPlus class.
    /// </summary>
    //*************************************************************************

    public TextBoxPlus()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: ScrollToBottom()
    //
    /// <summary>
    /// Scrolls the TextBox to the bottom of the text.
    /// </summary>
    //*************************************************************************

    public void
    ScrollToBottom()
    {
        // The TextBox must have focus before ScrollToCaret() will work.

        this.Focus();
        this.Select(this.Text.Length, 0);
        this.ScrollToCaret();
    }
}

}
