
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: TextBoxTraceListener
//
/// <summary>
/// Represents a TextBox that is a TraceListener.
/// </summary>
///
/// <remarks>
/// This is a read-only TextBox that gets its text from the .NET Framework's
/// trace system.  To use it, you must connect it to the trace system in the
/// following manner:
///
/// <code>
/// System.Diagnostics.Trace.Listeners.Add(
///     myTextBoxTraceListener.TraceListener);
/// </code>
///
/// <para>
/// Once you do this, anything written to System.Diagnostics.Trace.WriteLine()
/// will automatically get appended to the TextBox text.  You can limit what
/// gets appended by setting the <see cref="Filter" /> property.
/// </para>
///
/// <para>
/// The TextBox displays the most recent <see cref="MaxLines" /> lines of text.
/// Lines older than that are discarded.
/// </para>
///
/// <para>
/// Do not set the <see cref="TextBox.Text" /> property.  Doing so will cause
/// unpredictable results.  Calling <see cref="Clear" /> is okay.
/// </para>
///
/// <para>
/// Do not set <see cref="TextBoxBase.MaxLength" /> to anything but zero
/// (unlimited length).  Doing so may cause confusion when most-recent lines
/// get truncated.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class TextBoxTraceListener : TextBoxPlus, ITextBoxTraceListener
{
    //*************************************************************************
    //  Constructor: TextBoxTraceListener()
    //
    /// <summary>
    /// Initializes a new instance of the TextBoxTraceListener class.
    /// </summary>
    //*************************************************************************

    public TextBoxTraceListener()
    {
        m_iMaxLines = 100;
        m_sFilter = null;
        m_oTraceListenerForTextBox = new TraceListenerForTextBox(this);

        // Do not limit the length of text in the TextBox.  Text limiting is
        // done via the MaxLines property.

        this.MaxLength = 0;

        AssertValid();
    }

    //*************************************************************************
    //  Property: MaxLines
    //
    /// <summary>
    /// Gets or sets the maximum number of most-recent lines to display.
    /// </summary>
    ///
    /// <value>
    /// Maximum number of lines to display.  Lines older than this are
    /// discarded.  Must be greater than or equal to 1.  The default is 100.
    /// </value>
    //*************************************************************************

    public Int32
    MaxLines
    {
        get
        {
            AssertValid();

            return (m_iMaxLines);
        }

        set
        {
            if (m_iMaxLines < 1)
            {
                throw new Exception(
                    "TextBoxTraceListener.MaxLines must be >= 1."
                    );
            }

            m_iMaxLines = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Filter
    //
    /// <summary>
    /// Gets or sets the a filter that identifies which messages to append to
    /// the TextBox.
    /// </summary>
    ///
    /// <value>
    /// A filter string, or null if messages shouldn't be filtered.  The
    /// default is null.
    /// </value>
    ///
    /// <remarks>
    /// By default, anything written to System.Diagnostics.Trace.WriteLine()
    /// gets appended to the TextBox text.  You can limit which messages get
    /// appended by setting the Filter property.  If you set it to "MyFilter",
    /// for example, only messages that begin with "MyFilter"
    /// ("MyFilterMessage") will get appended, after "MyFilter" is removed
    /// ("Message").  All other messages are ignored.
    /// </remarks>
    //*************************************************************************

    public String
    Filter
    {
        get
        {
            AssertValid();

            return (m_sFilter);
        }

        set
        {
            m_sFilter = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: TraceListener
    //
    /// <summary>
    /// Gets the TextBox's custom TraceListener.
    /// </summary>
    ///
    /// <value>
    /// The TraceListener to connect to the Framework's trace system.
    /// </value>
    //*************************************************************************

    public TraceListener
    TraceListener
    {
        get
        {
            AssertValid();

            return (m_oTraceListenerForTextBox);
        }
    }

    //*************************************************************************
    //  Property: SelectedText()
    //
    /// <summary>
    /// Gets or sets a value indicating the currently selected text in the
    /// control.
    /// </summary>
    ///
    /// <value>
    /// A string that represents the currently selected text in the text box.
    /// </value>
    //*************************************************************************

    public override String
    SelectedText
    {
        #if false
        ***********************************************************************
        There is a bug in TextBox.AppendText() in .NET Framework 1.1 that
        limits the total length of text in the TextBox to 32767 characters.
        The bug was not present in .NET Framework 1.0.  The bug is mentioned
        many times in Usenet postings.  (Search for "textbox 32767" in Google
        Groups.  Don't bother looking in MSDN.)
        
        Using Lutz Roeder's .NET Reflector, I determined that
        TextBoxBase.AppendText() looks like this:

            public void AppendText(string text)
            {
                int num1;
                if (base.IsHandleCreated)
                {
                    num1 = SafeNativeMethods.GetWindowTextLength(
                        new HandleRef(this, base.Handle)) + 1;
                }
                else
                {
                    num1 = this.TextLength;
                }
                    this.SelectInternal(num1, num1);
                    this.SelectedText = text;
                }
            }

        There is nothing wrong with AppendText() itself.  The problem is in 
        TextBoxBase.SelectedText, the setter for which looks like this:

            public virtual void set_SelectedText(string value)
            {
                if (base.IsHandleCreated)
                {
                    base.SendMessage(0xc5, 0x7fff, 0);
                    base.SendMessage(0xc2, -1, (value == null) ? "" : value);
                    base.SendMessage(0xb9, 0, 0);
                    base.SendMessage(0xc5, this.maxLength, 0);
                }
                else
                {
                    (other code not relevant here)
                }
                this.ClearUndo();
            }

        The first SendMessage() call is an EM_SETLIMITTEXT message that limits
        the text length to 32767 characters.  Why is it doing that?  The .NET
        Framework 1.0 version of set_SelectedText() looks like this:

            public virtual void set_SelectedText(string value)
            {
                if (base.IsHandleCreated)
                {
                    base.SendMessage(0xc2, -1, (value == null) ? "" : value);
                    base.SendMessage(0xb9, 0, 0);
                }
                else
                {
                    (other code not relevant here)
                }
                this.ClearUndo();
            }

        There is no such EM_SETLIMITTEXT message in the 1.0 version.

        To work around this bug, I overrode the SelectedText setter here to
        make it look like the .NET Framework 1.0 version.  It seems to work
        fine.
        ***********************************************************************
        #endif

        set
        {
            if (base.IsHandleCreated)
            {
                SendMessage(new HandleRef(this, this.Handle), 0xc2, -1,
                    (value == null) ? "" : value);

                SendMessage(new HandleRef(this, this.Handle), 0xb9, 0, 0);
            }
            else
            {
                base.SelectedText = value;
            }

            this.ClearUndo();
        }

        get
        {
            return (base.SelectedText);
        }
    }

    //*************************************************************************
    //  Method: SetTextAndScrollToBottom()
    //
    /// <summary>
    /// Sets the TextBox text and scrolls the control to the bottom of the
    /// text.
    /// </summary>
    ///
    /// <param name="sText">
    /// New TextBox text.  This replaces any previous text.
    /// </param>
    //*************************************************************************

    public void
    SetTextAndScrollToBottom
    (
        String sText
    )
    {
        AssertValid();

        // Just setting the Text property to a large block of text does not
        // scroll to the bottom.  Instead, clear any existing text and use
        // AppendText(), which does scroll to the bottom.

        this.Text = String.Empty;
        this.AppendText(sText);

        // Do not replace the above code with this:
        //
        //     this.Text = sText;
        //     this.Focus();
        //     this.Select(this.Text.Length, 0);
        //     this.ScrollToCaret();
        //
        // That wreaks havoc with the focus, particularly in an MDI
        // application, and for some reason it can also lead to deadlocks in a
        // multithreaded application.

        // There is a bug in TextBox.AppendText() that limits the total length
        // of text in the TextBox to 32767 characters.  See the SelectedText
        // property.
    }

    //*************************************************************************
    //  Method: Clear()
    //
    /// <summary>
    /// Clears all text from the TextBox.
    /// </summary>
    //*************************************************************************

    public new void
    Clear()
    {
        AssertValid();

        base.Clear();

        m_oTraceListenerForTextBox.OnTextBoxClear();
    }

    //*************************************************************************
    //  Method: SendMessage()
    //
    /// <summary>
    /// Sends a Windows message to the control
    /// </summary>
    ///
    /// <param name="hWnd">
    /// Standard argument.
    /// </param>
    ///
    /// <param name="iMsg">
    /// Standard argument.
    /// </param>
    ///
    /// <param name="wParam">
    /// Standard argument.
    /// </param>
    ///
    /// <param name="lParam">
    /// Standard argument.
    /// </param>
    //*************************************************************************

    [DllImport("user32.dll", CharSet=CharSet.Auto)]

    protected static extern IntPtr
    SendMessage
    (
        HandleRef hWnd,
        Int32 iMsg,
        Int32 wParam,
        Int32 lParam
    );

    //*************************************************************************
    //  Method: SendMessage()
    //
    /// <summary>
    /// Sends a Windows message to the control
    /// </summary>
    ///
    /// <param name="hWnd">
    /// Standard argument.
    /// </param>
    ///
    /// <param name="iMsg">
    /// Standard argument.
    /// </param>
    ///
    /// <param name="wParam">
    /// Standard argument.
    /// </param>
    ///
    /// <param name="lParam">
    /// Standard argument.
    /// </param>
    //*************************************************************************

    [DllImport("user32.dll", CharSet=CharSet.Auto)]

    protected static extern IntPtr
    SendMessage
    (
        HandleRef hWnd,
        Int32 iMsg,
        Int32 wParam,
        String lParam
    );


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
        Debug.Assert(m_iMaxLines >= 1);
        // m_sFilter
        Debug.Assert(m_oTraceListenerForTextBox != null);
        Debug.Assert(this.MaxLength == 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Maximum number of most-recent lines to display.  Lines older than this
    /// are discarded.

    protected Int32 m_iMaxLines;

    /// If not null, only messages that start with m_sFilter are appended to
    /// the TextBox text.

    protected String m_sFilter;

    /// TraceListener that writes trace output to this ListBox.

    protected TraceListenerForTextBox m_oTraceListenerForTextBox;
}

}
