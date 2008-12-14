
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Adapters;

namespace Microsoft.NodeXL.DesktopApplication
{
//*****************************************************************************
//  Class: CommandLineParser
//
/// <summary>
/// Parses command line arguments.
/// </summary>
///
/// <remarks>
/// Pass the array of command line arguments to the constructor, then call the
/// <see cref="Parse" /> method.  If parsing succeeds, the parsed results are
/// made available through class properties.
///
/// <para>
/// Command line usage:
/// </para>
///
/// <para>
/// NodeXL.exe [GraphFileName | /c] [/type:Simple|Pajek]
/// </para>
///
/// <para>
/// The optional GraphFileName is a full path to a graph file that should be
/// opened at startup.  If /c is specified instead of a file name, the graph
/// data are read from the clipboard instead of a file.
/// </para>
///
/// <para>
/// The optional /type: argument specifies the type of the graph data, whether
/// it is read from a file or the clipboard.  If the /type: argument is not
/// specified, the type is derived from the file name if a file name is
/// specified, or is assumed to be Simple if /c is specified.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class CommandLineParser : NodeXLBase
{
    //*************************************************************************
    //  Constructor: CommandLineParser()
    //
    /// <summary>
    /// Initializes a new instance of the CommandLineParser class.
    /// </summary>
	///
    /// <param name="commandLineArguments">
    /// Array of command line arguments.  Can be empty.  Can't be null.
    /// </param>
    //*************************************************************************

    public CommandLineParser
	(
		String [] commandLineArguments
	)
    {
		m_asCommandLineArguments = commandLineArguments;
		m_bParsed = false;
		m_oDocument = null;

		AssertValid();
    }

    //*************************************************************************
    //  Property: Document
    //
    /// <summary>
    /// Gets a <see cref="Document" /> created from the command line arguments.
    /// </summary>
    ///
    /// <value>
	/// A new <see cref="Document" /> created from the command line arguments,
	/// or null if the command line arguments did not specify a document.  If
	/// the command line arguments haven't been parsed (if <see
	/// cref="Parse" /> hasn't been called or it returned false), an exception
	/// is thrown.
    /// </value>
    //*************************************************************************

    public Document
    Document
    {
        get
        {
            AssertValid();

			const String PropertyName = "Document";

			if (!m_bParsed)
			{
				throw new InvalidOperationException( String.Format(

					"{0}.{1}: Parse hasn't been called, or it returned false."
					,
					this.ClassName,
					PropertyName
					) );
			}

            return (m_oDocument);
        }
    }

    //*************************************************************************
    //  Method: Parse()
    //
    /// <summary>
    /// Parses the command line arguments and stores the results in fields.
    /// </summary>
    ///
    /// <param name="errorMessage">
    /// Where an error message gets stored if false is returned.
    /// </param>
	///
	/// <returns>
	/// true if parsing suceeds.
	/// </returns>
	///
	/// <remarks>
	/// If parsing succeeds, the results are stored in fields and true is
	/// returned.  Otherwise, an error message is stored at <paramref
	/// name="errorMessage" /> and false is returned.
	/// </remarks>
    //*************************************************************************

    public Boolean
    Parse
    (
		out String errorMessage
    )
    {
		Debug.Assert(!m_bParsed);
		Debug.Assert(m_oDocument == null);
		AssertValid();

		errorMessage = null;

		String sFileName = null;
		Boolean bPasteFromClipboard = false;
		IGraphAdapter oGraphAdapter = null;

		// Parse the command line arguments but don't act on them.

		if ( !TryParseNoAction(out sFileName, out bPasteFromClipboard,
			out oGraphAdapter, out errorMessage) )
		{
			return (false);
		}

		// The arguments were successfully parsed.  Try to act on them.

		if (sFileName != null || bPasteFromClipboard)
		{
			IGraph oGraph = null;

			Exception oException = null;

			try
			{
				if (sFileName != null)
				{
					// Create a graph from the file.

					oGraph = oGraphAdapter.LoadGraph(sFileName);
				}
				else
				{
					Debug.Assert(bPasteFromClipboard);

					// Check whether the clipboard contains text.

					IDataObject oDataObject = Clipboard.GetDataObject();

					if ( !oDataObject.GetDataPresent(DataFormats.StringFormat) )
					{
						return ( GetParsingErrorMessage(

							"The clipboard does not contain text.",

							out errorMessage
							) );
					}

					// Get the graph text.

					String sGraph =
						(String)oDataObject.GetData(DataFormats.StringFormat);

					// Convert the text to a graph.

					oGraph = oGraphAdapter.LoadGraph(
						new GraphFactory(), sGraph);
				}
			}
			catch (IOException oIOException)
			{
				oException = oIOException;
			}
			catch (UnauthorizedAccessException oUnauthorizedAccessException)
			{
				oException = oUnauthorizedAccessException;
			}
			catch (FormatException oFormatException)
			{
				oException = oFormatException;
			}

			if (oException != null)
			{
				errorMessage =
					(
						(sFileName == null) ?

						"The graph data isn't valid."
						:
						"The file specified on the command line could not be"
							+ " opened."
					)

					+ "\r\n\r\n"
					+ oException.Message
					;

				return (false);
			}

			// Create a document from the graph.

			GraphData oGraphData = new GraphData(oGraph);

			if (sFileName != null)
			{
				m_oDocument = new Document(oGraphData, sFileName);
			}
			else
			{
				Debug.Assert(bPasteFromClipboard);

				m_oDocument = new Document(
					oGraphData, null, NewDocumentTitleCreator.CreateTitle()
					);
			}
		}

		m_bParsed = true;

		return (true);
    }

    //*************************************************************************
    //  Method: TryParseNoAction()
    //
    /// <summary>
    /// Parses the command line arguments but does not act on them.
    /// </summary>
    ///
    /// <param name="sFileName">
	/// If true is returned, this gets set to either a full path to a graph
	/// file, or null if a file wasn't specified.
    /// </param>
	///
    /// <param name="bPasteFromClipboard">
    /// Where a "paste from clipboard" flag gets stored if true is returned.
    /// </param>
	///
    /// <param name="oGraphAdapter">
	/// If true is returned, this gets set to a graph adapter appropriate for
	/// creating a graph, or null if a file or "paste from clipboard" wasn't
	/// specified.
    /// </param>
	///
    /// <param name="sErrorMessage">
    /// Where an error message gets stored if false is returned.
    /// </param>
	///
	/// <returns>
	/// true if parsing suceeds.
	/// </returns>
	///
	/// <remarks>
	/// If parsing succeeds, the results are stored in the out parameters and
	/// true is returned.  Otherwise, an error message is stored at <paramref
	/// name="sErrorMessage" /> and false is returned.
	/// </remarks>
    //*************************************************************************

    protected Boolean
    TryParseNoAction
    (
		out String sFileName,
		out Boolean bPasteFromClipboard,
		out IGraphAdapter oGraphAdapter,
		out String sErrorMessage
    )
    {
		AssertValid();

		sFileName = null;
		bPasteFromClipboard = false;
		oGraphAdapter = null;
		sErrorMessage = null;

		if (m_asCommandLineArguments.Length > 2)
		{
			return ( GetParsingErrorMessage(

				"There can't be more than two arguments.",

				out sErrorMessage
				) );
		}

		const String TypeRoot = "/type:";
		String sType = null;

		// Loop through the arguments.

		foreach (String sCommandLineArgument in m_asCommandLineArguments)
		{
			String sLowerCase = sCommandLineArgument.ToLower();

			if (sLowerCase == "/c")
			{
				if (bPasteFromClipboard)
				{
					return ( GetParsingErrorMessage(

						"There can't be more than one \"/c\".",

						out sErrorMessage
						) );
				}

				bPasteFromClipboard = true;
			}
			else if ( sLowerCase.StartsWith(TypeRoot) )
			{
				if (oGraphAdapter != null)
				{
					return ( GetParsingErrorMessage(

						"There can't be more than one type.",

						out sErrorMessage
						) );
				}

				sType = sLowerCase.Substring(TypeRoot.Length);

				if ( !GraphAdapterManager.TryGraphAdapterNameToGraphAdapter(
					sType, out oGraphAdapter) )
				{
					return ( GetParsingErrorMessage(

						"The type isn't recognized.",

						out sErrorMessage
						) );
				}
			}
			else
			{
				// Assume that this argument is a file name.

				if (sFileName != null)
				{
					return ( GetParsingErrorMessage(

						"There can't be more than one file name.",

						out sErrorMessage
						) );
				}

				sFileName = sCommandLineArgument;
			}
		}

		if (sFileName != null && bPasteFromClipboard)
		{
			return ( GetParsingErrorMessage(

				"If a file name is specified, \"/c\" isn't allowed.",

				out sErrorMessage
				) );
		}

		Boolean bGraphSpecified = (sFileName != null || bPasteFromClipboard);

		if (sType != null && !bGraphSpecified)
		{
			return ( GetParsingErrorMessage(

				"If a type is specified, either a file name or \"/c\" must"
				+ " also be specified.",

				out sErrorMessage
				) );
		}

		if (bGraphSpecified && oGraphAdapter == null)
		{
			// A type wasn't specified.

			if (sFileName != null)
			{
				// Use the file extension to determine which graph adapter to
				// use.

				oGraphAdapter = GraphAdapterManager.FileNameToGraphAdapter(
					sFileName);
			}
			else
			{
				// Use the default for the clipboard.

				Boolean bFound =
					GraphAdapterManager.TryGraphAdapterNameToGraphAdapter(
						"simple", out oGraphAdapter);

				Debug.Assert(bFound);
			}
		}

		return (true);
    }

    //*************************************************************************
    //  Method: GetParsingErrorMessage()
    //
    /// <summary>
    /// Creates an error message for use by <see cref="Parse" />.
    /// </summary>
    ///
    /// <param name="sErrorDetails">
    /// Error details.
    /// </param>
	///
    /// <param name="sErrorMessage">
	/// Where a complete error message gets stored.
    /// </param>
	///
	/// <returns>
	/// Always returns false for the convenience of the caller.
	/// </returns>
    //*************************************************************************

	protected Boolean
	GetParsingErrorMessage
	(
		String sErrorDetails,
		out String sErrorMessage
	)
	{
		Debug.Assert( !String.IsNullOrEmpty(sErrorDetails) );
		AssertValid();

		StringBuilder oStringBuilder = new StringBuilder();

		oStringBuilder.Append(
			"You specified these command line arguments:\r\n\r\n"
			+ "\t"
			);

		foreach (String sCommandLineArgument in m_asCommandLineArguments)
		{
			oStringBuilder.Append(sCommandLineArgument);
			oStringBuilder.Append(' ');
		}

		oStringBuilder.Append(
			"\r\n\r\n"
			+ "There was a problem with the arguments:"
			+ "\r\n\r\n"
			+ "\t"
			);

		oStringBuilder.Append(sErrorDetails);

		oStringBuilder.Append(
			"\r\n\r\n"
			+ "Here are the allowed command line arguments:"
			+ "\r\n\r\n"
			+ "\t"
			+ "[ GraphFileName | /c ] [ /type:"
			);

		oStringBuilder.Append(GraphAdapterManager.GraphAdapterNames);

		oStringBuilder.Append(
			" ]"
			);


		oStringBuilder.Append(
			"\r\n\r\n"
			+ "For example, these arguments specify a file to open, where the"
			+ " file is in the Simple format:"
			+ "\r\n\r\n"
			+ "\t"
			+ "C:\\GraphFile.txt /type:Simple"
			+ "\r\n\r\n"
			+ "These arguments specify that the file contents are in the"
			+ " clipboard, in the Pajek format:"
			+ "\r\n\r\n"
			+ "\t"
			+ "/c /type:Pajek"
			+ "\r\n\r\n"
			+ "If the /type: argument is omitted, the type is derived from the"
			+ " file name if a file is specified, or is assumed to be Simple"
			+ " if the clipboard is specified."
			);

		sErrorMessage = oStringBuilder.ToString();

		return (false);
	}


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
		base.AssertValid();

		Debug.Assert(m_asCommandLineArguments != null);
		// m_bParsed
		// m_oDocument
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Array of command line arguments.  Can be empty.  Can't be null.

	protected String [] m_asCommandLineArguments;

	/// true if Parse() has been called and it succeeded.

	protected Boolean m_bParsed;

	/// A new Document created from the command line arguments, or null if the
	/// command line arguments did not specify a document.  Valid only if
	/// m_bParsed is true.

	protected Document m_oDocument;
}

}
