
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: ToolStripPlus
//
/// <summary>
/// Represents a ToolStrip with additional features.
/// </summary>
//*****************************************************************************

public class ToolStripPlus : ToolStrip
{
    //*************************************************************************
    //  Constructor: ToolStripPlus()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ToolStripPlus" /> class.
    /// </summary>
    //*************************************************************************

    public ToolStripPlus()
    {
		m_bClickThrough = true;

		AssertValid();
    }

    //*************************************************************************
    //  Property: ClickThrough
    //
    /// <summary>
    /// Gets or sets a flag indicating whether click-through is used.
    /// </summary>
    ///
    /// <value>
    /// true if clicking a control in the ToolStrip activates the ToolStrip AND
	/// clicks the clicked control.  The default is true.
    /// </value>
	///
	/// <remarks>
	/// In a standard ToolStrip, if window B is active when a control in
	/// window A's ToolStrip is clicked, the click activates window A but
	/// doesn't click the control.  A second click on the control is then
	/// required.  When this property is true, only a single click is required 
	/// to both activate the ToolStrip and click the control.
	///
	/// <para>
	/// The code to accomplish this is borrowed from Rick Brewster:
	/// </para>
	///
	/// <para>
	/// http://blogs.msdn.com/rickbrew/archive/2006/01/09/511003.aspx
	/// </para>
	///
	/// </remarks>
    //*************************************************************************

    public Boolean
    ClickThrough
    {
        get
        {
            AssertValid();

            return (m_bClickThrough);
        }

        set
        {
			m_bClickThrough = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: WndProc()
    //
    /// <summary>
    /// Processes Windows messages.
    /// </summary>
    ///
    /// <param name="m">
    /// The Windows message to process.
    /// </param>
    //*************************************************************************

    protected override void
	WndProc
	(
		ref Message m
	)
    {
        base.WndProc(ref m);

        if (m_bClickThrough
			&&
            m.Msg == WM_MOUSEACTIVATE
			&&
            m.Result == (IntPtr)MA_ACTIVATEANDEAT
			)
        {
            m.Result = (IntPtr)MA_ACTIVATE;
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
		// m_bClickThrough
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	///
	protected const UInt32 WM_MOUSEACTIVATE = 0x21;
	///
	protected const UInt32 MA_ACTIVATE = 1;
	///
	protected const UInt32 MA_ACTIVATEANDEAT = 2;
	///
	protected const UInt32 MA_NOACTIVATE = 3;
	///
	protected const UInt32 MA_NOACTIVATEANDEAT = 4;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Indicates whether click-through is used.

	protected Boolean m_bClickThrough;
}

}
