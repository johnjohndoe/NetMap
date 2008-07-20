
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: TraceListenerForTextBox
//
/// <summary>
/// Represents a TraceListener that writes trace output to the 
/// TextBoxTraceListener, which is a TextBox.
/// </summary>
///
/// <remarks>
/// The TextBoxTraceListener owns a TraceListenerForTextBox object.
/// </remarks>
//*****************************************************************************

public class TraceListenerForTextBox : TraceListener
{
    //*************************************************************************
    //  Constructor: TraceListenerForTextBox()
    //
    /// <summary>
    /// Initializes a new instance of the TraceListenerForTextBox class.
    /// </summary>
	///
    /// <param name="oTextBoxTraceListener">
	/// TextBox to write to.
    /// </param>
    //*************************************************************************

    public TraceListenerForTextBox
	(
		ITextBoxTraceListener oTextBoxTraceListener
	)
    {
		m_oTextBoxTraceListener = oTextBoxTraceListener;
		m_oStringBuilder = new StringBuilder();
		m_iLineCount = 0;

		AssertValid();
    }

    //*************************************************************************
    //  Method: Write()
    //
    /// <summary>
    /// Writes a message followed by a newline to the listener.
    /// </summary>
    ///
    /// <param name="sMessage">
	/// Message to write.  Can contains multiple lines separated by "\r\n"
	/// characters.  "\r\n" gets appended to the message.
    /// </param>
	///
	/// <remarks>
	/// This does the same thing as <see cref="WriteLine" />.
	///
	/// <para>
	/// This is required in classes derived from TraceListener.
	/// </para>
	/// </remarks>
    //*************************************************************************

	public override void
	Write
	(
		String sMessage
	)
    {
		Debug.Assert(sMessage != null);
        AssertValid();

		// Don't allow text to be written without adding a newline.

		WriteLine(sMessage);
    }

    //*************************************************************************
    //  Method: WriteLine()
    //
    /// <summary>
    /// Writes a message followed by a newline to the listener.
    /// </summary>
    ///
    /// <param name="sMessage">
	/// Message to write.  Can contains multiple lines separated by "\r\n"
	/// characters.  "\r\n" gets appended to the message.
    /// </param>
	///
	/// <remarks>
	/// This is required in classes derived from TraceListener.
	/// </remarks>
    //*************************************************************************

	public override void
	WriteLine
	(
		String sMessage
	)
    {
		if (m_oTextBoxTraceListener.InvokeRequired)
		{
			// If this is being called from a thread that isn't the owner of
			// the TextBox, switch to the owner's thread.

			WriteLineDelegate oWriteLineDelegate =
				new WriteLineDelegate(WriteLine);

			m_oTextBoxTraceListener.BeginInvoke( oWriteLineDelegate,
				new Object[] {sMessage} );

			return;
		}

		Debug.Assert(sMessage != null);
		Debug.Assert(!m_oTextBoxTraceListener.InvokeRequired);
        AssertValid();

		if (sMessage == null)
		{
			throw new ArgumentException(
				"TraceListenerForTextBox.WriteLine: Don't pass null to"
				+ " Trace.WriteLine()."
				);
		}

		// If a filter was specified and the message doesn't start with the
		// filter, ignore the message.

		String sFilter = m_oTextBoxTraceListener.Filter;

		if ( !StringUtil.IsEmpty(sFilter) && !sMessage.StartsWith(sFilter) )
			return;

		// Count the number of '\n' characters in the message.

		Int32 iLineBreaksInMessage = CountLineBreaksInMessage(sMessage);

		// If the StringBuilder is not empty, append a NewLine to it.

		AppendNewLineToStringBuilder();

		if (sFilter != null)
		{
			// Remove the filter, then append the message to the StringBuilder.

			Int32 iFilterLength = sFilter.Length;

			m_oStringBuilder.Append(sMessage, iFilterLength,
				sMessage.Length - iFilterLength);
		}
		else
		{
			// Add the entire message to the current text.

			m_oStringBuilder.Append(sMessage);
		}

		m_iLineCount += iLineBreaksInMessage + 1;

		// If there are now more than MaxLines, remove the earliest lines.

		Int32 iMaxLines = m_oTextBoxTraceListener.MaxLines;

		if (m_iLineCount > iMaxLines)
		{
			RemoveLinesFromStringBuilder(m_iLineCount - iMaxLines);
			m_iLineCount = iMaxLines;
		}

		// Replace the text in the TextBox.

		m_oTextBoxTraceListener.SetTextAndScrollToBottom(
			m_oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: OnTextBoxClear()
    //
    /// <summary>
    /// Gets called when the TextBox is cleared.
    /// </summary>
    //*************************************************************************

	public void
	OnTextBoxClear()
	{
		AssertValid();

		m_oStringBuilder.Remove(0, m_oStringBuilder.Length);
		m_iLineCount = 0;
	}

    //*************************************************************************
    //  Method: CountLineBreaksInMessage()
    //
    /// <summary>
    /// Returns the number of '\n' characters in a message.
    /// </summary>
    ///
    /// <param name="sMessage">
	/// Message to count '\n' characters within.  Can't be null.
    /// </param>
	///
	/// <returns>
	/// Number of '\n' characters.
	/// </returns>
    //*************************************************************************

	protected Int32
	CountLineBreaksInMessage
	(
		String sMessage
	)
	{
		Debug.Assert(sMessage != null);

		Int32 iLineBreaks = 0;
		Int32 iIndex = -1;

		while ( (iIndex = sMessage.IndexOf('\n', iIndex + 1) ) != -1 )
			iLineBreaks++;

		return (iLineBreaks);
	}

    //*************************************************************************
    //  Method: AppendNewLineToStringBuilder()
    //
    /// <summary>
    /// Appends a NewLine to m_oStringBuilder if m_oStringBuilder contains
	/// text.
    /// </summary>
    //*************************************************************************

	protected void
	AppendNewLineToStringBuilder()
	{
		if (m_oStringBuilder.Length > 0)
			m_oStringBuilder.Append(Environment.NewLine);
	}

    //*************************************************************************
    //  Method: RemoveLinesFromStringBuilder()
    //
    /// <summary>
    /// Removes the first N lines from m_oStringBuilder.
    /// </summary>
    ///
    /// <param name="iLinesToRemove">
	/// Number of lines to remove.
    /// </param>
	///
    /// <remarks>
	/// Lines within m_oStringBuilder must be separated with
	/// Environment.NewLine.
    /// </remarks>
    //*************************************************************************

	protected void
	RemoveLinesFromStringBuilder
	(
		Int32 iLinesToRemove
	)
	{
		AssertValid();
		Debug.Assert(iLinesToRemove >= 0);

		Int32 iIndex = 0;
		Int32 iLength = m_oStringBuilder.Length;

		while (iLinesToRemove > 0)
		{
			// Look for EnvironmentNewLine (\r\n).  Unfortunately,
			// StringBuilder does not have an IndexOf() method.

			while (iIndex < iLength && m_oStringBuilder[iIndex] != '\r')
				iIndex++;

			// Skip over the \n.

			iIndex++;

			iLinesToRemove--;
		}

		m_oStringBuilder.Remove(0, iIndex + 1);
	}

    //*************************************************************************
    //  Delegate: WriteLineDelegate
	///
    /// <summary>
    /// Delegate for calling the <see cref="WriteLine" /> method.
    /// </summary>
	///
	/// <param name="sMessage">
	/// Message to write.  Can contains multiple lines separated by "\r\n"
	/// characters.  "\r\n" gets appended to the message.
	/// </param>
    //*************************************************************************

	delegate void
	WriteLineDelegate
	(
		String sMessage
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
		Debug.Assert(m_oTextBoxTraceListener != null);
		Debug.Assert(m_oStringBuilder != null);
		Debug.Assert(m_iLineCount >= 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// TextBox that the listener writes to.

	protected ITextBoxTraceListener m_oTextBoxTraceListener;

	/// StringBuilder used to append all the lines together.  WriteLine()
	/// appends a line to the StringBuilder, then copies the entire contents of
	/// the StringBuilder to the TextBox.

	protected StringBuilder m_oStringBuilder;

	/// Number of lines in the TextBox.  Lines in the TextBox are separated by
	/// Environment.NewLine.

	protected Int32 m_iLineCount;
}


//*****************************************************************************
//	Interface: ITextBoxTraceListener
//
/// <summary>
/// Interface implemented by the TextBoxTraceListener class that allows a
/// TraceListenerForTextBox object to talk to it.
/// </summary>
///
/// <remarks>
/// The TextBoxTraceListener class, which is a TextBox, owns a
/// TraceListenerForTextBox object.  The TraceListenerForTextBox is a
/// TraceListener that routes trace statements to the TextBox.  It uses the
/// TextBox's ITextBoxTraceListener interface to do this.
///
/// It would be possible (and simpler) to have the TraceListenerForTextBox talk
/// directly to the TextBox without using an interface, but having an interface
/// allows the TextBoxTraceListener class to be tested in NUnit without having
/// a TextBox present in the test applicaton.
/// </remarks>
//*****************************************************************************

public interface ITextBoxTraceListener
{
    //*************************************************************************
    //  Property: MaxLines
    //
    /// <summary>
    /// Gets the maximum number of most-recent lines to display.
    /// </summary>
    ///
    /// <value>
    /// Maximum number of lines to display.
    /// </value>
    //*************************************************************************

    Int32
    MaxLines
    {
        get;
    }

    //*************************************************************************
    //  Property: Filter
    //
    /// <summary>
    /// Gets the a filter that identifies which messages to append to the
	/// TextBox.
    /// </summary>
    ///
    /// <value>
	/// A filter string, or null if messages shouldn't be filtered.
    /// </value>
    //*************************************************************************

    String
    Filter
    {
        get;
    }

    //*************************************************************************
    //  Property: InvokeRequired
    //
    /// <summary>
	/// Gets a value indicating whether the caller must call an invoke method
	/// when making method calls to the control because the caller is on a
	/// different thread than the one the control was created on.
    /// </summary>
    ///
    /// <value>
	/// true if the control's Handle was created on a different thread than the
	/// calling thread (indicating that you must make calls to the control
	/// through an invoke method); otherwise, false.
    /// </value>
    //*************************************************************************

	Boolean
	InvokeRequired
	{
		get;
	}

	//*************************************************************************
	//	Method: SetTextAndScrollToBottom()
	//
	/// <summary>
	/// Sets the TextBox text and scrolls the control to the bottom of the
	/// text.
	/// </summary>
	///
	/// <param name="sText">
	///	New TextBox text.  This replaces any previous text.
	/// </param>
	//*************************************************************************

	void
	SetTextAndScrollToBottom
	(
		String sText
	);

	//*************************************************************************
	//	Method: BeginInvoke()
	//
	/// <summary>
	/// Executes the specified delegate asynchronously with the specified
	/// arguments, on the thread that the control's underlying handle was
	/// created on.
	/// </summary>
	///
	/// <param name="oDelegate">
	///	Delegate to call.
	/// </param>
	///
	/// <param name="aoArguments">
	///	Arguments to pass to delegate.
	/// </param>
	//*************************************************************************

	IAsyncResult
	BeginInvoke
	(
		Delegate oDelegate,
		Object[] aoArguments
	);
}

}
