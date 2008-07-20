
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NetMap.Core;
using Microsoft.NetMap.DesktopApplication;

namespace Microsoft.NetMap.UnitTests
{
//*****************************************************************************
//  Class: CommandLineParserTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="CommandLineParser" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class CommandLineParserTest : Object
{
    //*************************************************************************
    //  Constructor: CommandLineParserTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandLineParserTest" />
	/// class.
    /// </summary>
    //*************************************************************************

    public CommandLineParserTest()
    {
		m_oCommandLineParser = null;
		m_sTempFileName = null;
    }

    //*************************************************************************
    //  Method: SetUp()
    //
    /// <summary>
    /// Gets run before each test.
    /// </summary>
    //*************************************************************************

    [TestInitializeAttribute]

    public void
    SetUp()
    {
		m_sTempFileName = Path.GetTempFileName();
    }

    //*************************************************************************
    //  Method: TearDown()
    //
    /// <summary>
    /// Gets run after each test.
    /// </summary>
    //*************************************************************************

    [TestCleanupAttribute]

    public void
    TearDown()
    {
		m_oCommandLineParser = null;

		if ( File.Exists(m_sTempFileName) )
		{
			File.Delete(m_sTempFileName);
		}
    }

    //*************************************************************************
    //  Method: TestDocumentBad()
    //
    /// <summary>
    /// Tests the DocumentBad() property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(InvalidOperationException) ) ]

    public void
    TestDocumentBad()
    {
		// Parse() not called.

		try
		{
            m_oCommandLineParser = new CommandLineParser( new String[0] );

			Document oDocument = m_oCommandLineParser.Document;
		}
		catch (InvalidOperationException oInvalidOperationException)
		{
			Assert.AreEqual(
			
				"Microsoft.NetMap.DesktopApplication.CommandLineParser."
				+ "Document: Parse hasn't been called, or it returned false."
				,
				oInvalidOperationException.Message
				);

			throw oInvalidOperationException;
		}
    }

    //*************************************************************************
    //  Method: TestDocumentBad2()
    //
    /// <summary>
    /// Tests the DocumentBad() property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
	[ ExpectedException( typeof(InvalidOperationException) ) ]

    public void
    TestDocumentBad2()
    {
		// Parse() called, returned false.

		try
		{
			m_oCommandLineParser = new CommandLineParser( new String[]{
				"1",
				"2",
				"3"
				} );

			Document oDocument = m_oCommandLineParser.Document;
		}
		catch (InvalidOperationException oInvalidOperationException)
		{
			Assert.AreEqual(
			
				"Microsoft.NetMap.DesktopApplication.CommandLineParser."
				+ "Document: Parse hasn't been called, or it returned false."
				,
				oInvalidOperationException.Message
				);

			throw oInvalidOperationException;
		}
    }

    //*************************************************************************
    //  Method: TestParse()
    //
    /// <summary>
    /// Tests the Parse method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParse()
    {
		// No arguments.

		m_oCommandLineParser = new CommandLineParser( new String[0] );

		String sErrorMessage = null;

		Assert.IsTrue( m_oCommandLineParser.Parse(out sErrorMessage) );

		Assert.IsNull(sErrorMessage);

		Assert.IsNull(m_oCommandLineParser.Document);
    }

    //*************************************************************************
    //  Method: TestParse2()
    //
    /// <summary>
    /// Tests the Parse method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParse2()
    {
		// File name, no type.

		const String Vertex1Name = "Vertex1";
		const String Vertex2Name = "Vertex2";

		const String GraphText = Vertex1Name + "\t" + Vertex2Name + "\r\n";

		WriteGraphToTempFile(GraphText);

		m_oCommandLineParser = new CommandLineParser( new String[]{
			m_sTempFileName
			} );

		String sErrorMessage = null;

		Assert.IsTrue( m_oCommandLineParser.Parse(out sErrorMessage) );

		Assert.IsNull(sErrorMessage);

		Document oDocument = m_oCommandLineParser.Document;

		Assert.IsNotNull(oDocument);

		IVertexCollection oVertices = oDocument.GraphData.Graph.Vertices;

		Assert.AreEqual(2, oVertices.Count);

		Assert.IsTrue( oVertices.Contains(Vertex1Name) );
		Assert.IsTrue( oVertices.Contains(Vertex2Name) );
    }

    //*************************************************************************
    //  Method: TestParse3()
    //
    /// <summary>
    /// Tests the Parse method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParse3()
    {
		// File name, Simple type.

		const String Vertex1Name = "Vertex1";
		const String Vertex2Name = "Vertex2";

		const String GraphText = Vertex1Name + "\t" + Vertex2Name + "\r\n";

		WriteGraphToTempFile(GraphText);

		m_oCommandLineParser = new CommandLineParser( new String[]{
			m_sTempFileName,
			"/type:Simple"
			} );

		String sErrorMessage = null;

		Assert.IsTrue( m_oCommandLineParser.Parse(out sErrorMessage) );

		Assert.IsNull(sErrorMessage);

		Document oDocument = m_oCommandLineParser.Document;

		Assert.IsNotNull(oDocument);

		IVertexCollection oVertices = oDocument.GraphData.Graph.Vertices;

		Assert.AreEqual(2, oVertices.Count);

		Assert.IsTrue( oVertices.Contains(Vertex1Name) );
		Assert.IsTrue( oVertices.Contains(Vertex2Name) );
    }

    //*************************************************************************
    //  Method: TestParse4()
    //
    /// <summary>
    /// Tests the Parse method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParse4()
    {
		// File name, Pajek type.

		const String Vertex1Name = "Vertex1";
		const String Vertex2Name = "Vertex2";

		const String GraphText =
			"*vertices 2\r\n"
			+ "1 " + Vertex1Name + " 0 0 0\r\n"
			+ "2 " + Vertex2Name + " 0 0 0\r\n"
			;

		WriteGraphToTempFile(GraphText);

		m_oCommandLineParser = new CommandLineParser( new String[]{
			m_sTempFileName,
			"/type:Pajek"
			} );

		String sErrorMessage = null;

		Assert.IsTrue( m_oCommandLineParser.Parse(out sErrorMessage) );

		Assert.IsNull(sErrorMessage);

		Document oDocument = m_oCommandLineParser.Document;

		Assert.IsNotNull(oDocument);

		IVertexCollection oVertices = oDocument.GraphData.Graph.Vertices;

		Assert.AreEqual(2, oVertices.Count);

		Assert.IsTrue( oVertices.Contains(Vertex1Name) );
		Assert.IsTrue( oVertices.Contains(Vertex2Name) );
    }

    //*************************************************************************
    //  Method: TestParse5()
    //
    /// <summary>
    /// Tests the Parse method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParse5()
    {
		// Paste, no type.

		const String Vertex1Name = "Vertex1";
		const String Vertex2Name = "Vertex2";

		const String GraphText = Vertex1Name + "\t" + Vertex2Name + "\r\n";

		WriteGraphToClipboard(GraphText);

		m_oCommandLineParser = new CommandLineParser( new String[]{
			"/c"
			} );

		String sErrorMessage = null;

		Assert.IsTrue( m_oCommandLineParser.Parse(out sErrorMessage) );

		Assert.IsNull(sErrorMessage);

		Document oDocument = m_oCommandLineParser.Document;

		Assert.IsNotNull(oDocument);

		IVertexCollection oVertices = oDocument.GraphData.Graph.Vertices;

		Assert.AreEqual(2, oVertices.Count);

		Assert.IsTrue( oVertices.Contains(Vertex1Name) );
		Assert.IsTrue( oVertices.Contains(Vertex2Name) );
    }

    //*************************************************************************
    //  Method: TestParse6()
    //
    /// <summary>
    /// Tests the Parse method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParse6()
    {
		// Paste, Simple type.

		const String Vertex1Name = "Vertex1";
		const String Vertex2Name = "Vertex2";

		const String GraphText = Vertex1Name + "\t" + Vertex2Name + "\r\n";

		WriteGraphToClipboard(GraphText);

		m_oCommandLineParser = new CommandLineParser( new String[]{
			"/c",
			"/type:Simple"
			} );

		String sErrorMessage = null;

		Assert.IsTrue( m_oCommandLineParser.Parse(out sErrorMessage) );

		Assert.IsNull(sErrorMessage);

		Document oDocument = m_oCommandLineParser.Document;

		Assert.IsNotNull(oDocument);

		IVertexCollection oVertices = oDocument.GraphData.Graph.Vertices;

		Assert.AreEqual(2, oVertices.Count);

		Assert.IsTrue( oVertices.Contains(Vertex1Name) );
		Assert.IsTrue( oVertices.Contains(Vertex2Name) );
    }

    //*************************************************************************
    //  Method: TestParse7()
    //
    /// <summary>
    /// Tests the Parse method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParse7()
    {
		// Paste, Pajek type.

		const String Vertex1Name = "Vertex1";
		const String Vertex2Name = "Vertex2";

		const String GraphText =
			"*vertices 2\r\n"
			+ "1 " + Vertex1Name + " 0 0 0\r\n"
			+ "2 " + Vertex2Name + " 0 0 0\r\n"
			;

		WriteGraphToClipboard(GraphText);

		m_oCommandLineParser = new CommandLineParser( new String[]{
			"/c",
			"/type:Pajek"
			} );

		String sErrorMessage = null;

		Assert.IsTrue( m_oCommandLineParser.Parse(out sErrorMessage) );

		Assert.IsNull(sErrorMessage);

		Document oDocument = m_oCommandLineParser.Document;

		Assert.IsNotNull(oDocument);

		IVertexCollection oVertices = oDocument.GraphData.Graph.Vertices;

		Assert.AreEqual(2, oVertices.Count);

		Assert.IsTrue( oVertices.Contains(Vertex1Name) );
		Assert.IsTrue( oVertices.Contains(Vertex2Name) );
    }

    //*************************************************************************
    //  Method: TestParseBad()
    //
    /// <summary>
    /// Tests the Parse method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParseBad()
    {
		// 3 arguments.

		const String Vertex1Name = "Vertex1";
		const String Vertex2Name = "Vertex2";

		const String GraphText = Vertex1Name + "\t" + Vertex2Name + "\r\n";

		WriteGraphToTempFile(GraphText);

		m_oCommandLineParser = new CommandLineParser( new String[]{
			"/c",
			"/type:Simple",
			"x"
			} );

		String sErrorMessage = null;

		Assert.IsFalse( m_oCommandLineParser.Parse(out sErrorMessage) );

		// Note:
		//
		// To make maintenance easier, this is the only test that checks the
		// full error message.  The other tests just look for an error-specific
		// substring.

		Assert.AreEqual(

			"You specified these command line arguments:\r\n\r\n\t"
			+ "/c /type:Simple x \r\n\r\n"
			+ "There was a problem with the arguments:\r\n\r\n\t"
			+ "There can't be more than two arguments.\r\n\r\n"
			+ "Here are the allowed command line arguments:\r\n\r\n\t"
			+ "[ GraphFileName | /c ] [ /type:Simple|Pajek ]\r\n\r\n"
			+ "For example, these arguments specify a file to open, where the"
			+ " file is in the Simple format:\r\n\r\n\t"
			+ "C:\\GraphFile.txt /type:Simple\r\n\r\n"
			+ "These arguments specify that the file contents are in the"
			+ " clipboard, in the Pajek format:\r\n\r\n\t"
			+ "/c /type:Pajek\r\n\r\n"
			+ "If the /type: argument is omitted, the type is derived from the"
			+ " file name if a file is specified, or is assumed to be Simple"
			+ " if the clipboard is specified."
			,
			sErrorMessage
			);
    }

    //*************************************************************************
    //  Method: TestParseBad2()
    //
    /// <summary>
    /// Tests the Parse method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParseBad2()
    {
		// Clipboard doesn't contain text.

		Clipboard.SetDataObject( new Byte[] {1, 2, 3} );

		m_oCommandLineParser = new CommandLineParser( new String[]{
			"/c",
			"/type:Simple",
			} );

		String sErrorMessage = null;

		Assert.IsFalse( m_oCommandLineParser.Parse(out sErrorMessage) );

		Assert.IsTrue(sErrorMessage.IndexOf(

			"The clipboard does not contain text."
			)
			> 0);
    }

    //*************************************************************************
    //  Method: TestParseBad3()
    //
    /// <summary>
    /// Tests the Parse method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParseBad3()
    {
		// Bad graph data format, Simple type.

		const String Vertex1Name = "Vertex1";
		const String Vertex2Name = "Vertex2";

		const String GraphText = Vertex1Name + Vertex2Name + "\r\n";

		WriteGraphToClipboard(GraphText);

		m_oCommandLineParser = new CommandLineParser( new String[]{
			"/c",
			"/type:Simple",
			} );

		String sErrorMessage = null;

		Assert.IsFalse( m_oCommandLineParser.Parse(out sErrorMessage) );

		Assert.IsTrue(sErrorMessage.IndexOf(

			"Line 1 is not in the expected format."
			)
			> 0);
    }

    //*************************************************************************
    //  Method: TestParseBad4()
    //
    /// <summary>
    /// Tests the Parse method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParseBad4()
    {
		// Bad graph data format, Pajek type.

		const String GraphText = "*vertices xyz";

		WriteGraphToClipboard(GraphText);

		m_oCommandLineParser = new CommandLineParser( new String[]{
			"/c",
			"/type:Pajek",
			} );

		String sErrorMessage = null;

		Assert.IsFalse( m_oCommandLineParser.Parse(out sErrorMessage) );

		Assert.IsTrue(sErrorMessage.IndexOf(

			"Line 1 is not in the expected format."
			)
			> 0);
    }

    //*************************************************************************
    //  Method: TestParseBad5()
    //
    /// <summary>
    /// Tests the Parse method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParseBad5()
    {
		// Two /c.

		m_oCommandLineParser = new CommandLineParser( new String[]{
			"/c",
			"/c",
			} );

		String sErrorMessage = null;

		Assert.IsFalse( m_oCommandLineParser.Parse(out sErrorMessage) );

		Assert.IsTrue(sErrorMessage.IndexOf(

			"There can't be more than one \"/c\"."
			)
			> 0);
    }

    //*************************************************************************
    //  Method: TestParseBad6()
    //
    /// <summary>
    /// Tests the Parse method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParseBad6()
    {
		// Two types.

		m_oCommandLineParser = new CommandLineParser( new String[]{
			"/type:Simple",
			"/type:Simple",
			} );

		String sErrorMessage = null;

		Assert.IsFalse( m_oCommandLineParser.Parse(out sErrorMessage) );

		Assert.IsTrue(sErrorMessage.IndexOf(

			"There can't be more than one type."
			)
			> 0);
    }

    //*************************************************************************
    //  Method: TestParseBad7()
    //
    /// <summary>
    /// Tests the Parse method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParseBad7()
    {
		// Bad type.

		m_oCommandLineParser = new CommandLineParser( new String[]{
			"/c",
			"/type:abc",
			} );

		String sErrorMessage = null;

		Assert.IsFalse( m_oCommandLineParser.Parse(out sErrorMessage) );

		Assert.IsTrue(sErrorMessage.IndexOf(

			"The type isn't recognized."
			)
			> 0);
    }

    //*************************************************************************
    //  Method: TestParseBad8()
    //
    /// <summary>
    /// Tests the Parse method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParseBad8()
    {
		// Empty type.

		m_oCommandLineParser = new CommandLineParser( new String[]{
			"/c",
			"/type:",
			} );

		String sErrorMessage = null;

		Assert.IsFalse( m_oCommandLineParser.Parse(out sErrorMessage) );

		Assert.IsTrue(sErrorMessage.IndexOf(

			"The type isn't recognized."
			)
			> 0);
    }

    //*************************************************************************
    //  Method: TestParseBad9()
    //
    /// <summary>
    /// Tests the Parse method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParseBad9()
    {
		// 2 file names.

		m_oCommandLineParser = new CommandLineParser( new String[]{
			"FileName1",
			"FileName2",
			} );

		String sErrorMessage = null;

		Assert.IsFalse( m_oCommandLineParser.Parse(out sErrorMessage) );

		Assert.IsTrue(sErrorMessage.IndexOf(

			"There can't be more than one file name."
			)
			> 0);
    }

    //*************************************************************************
    //  Method: TestParseBad10()
    //
    /// <summary>
    /// Tests the Parse method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParseBad10()
    {
		// File name and clipboard.

		m_oCommandLineParser = new CommandLineParser( new String[]{
			"/c",
			"FileName2",
			} );

		String sErrorMessage = null;

		Assert.IsFalse( m_oCommandLineParser.Parse(out sErrorMessage) );

		Assert.IsTrue(sErrorMessage.IndexOf(

			"If a file name is specified, \"/c\" isn't allowed."
			)
			> 0);
    }

    //*************************************************************************
    //  Method: TestParseBad11()
    //
    /// <summary>
    /// Tests the Parse method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParseBad11()
    {
		// Type only.

		m_oCommandLineParser = new CommandLineParser( new String[]{
			"/type:Simple",
			} );

		String sErrorMessage = null;

		Assert.IsFalse( m_oCommandLineParser.Parse(out sErrorMessage) );

		Assert.IsTrue(sErrorMessage.IndexOf(

			"If a type is specified, either a file name or \"/c\" must also be"
			+ " specified."
			)
			> 0);
    }

    //*************************************************************************
    //  Method: WriteGraphToTempFile()
    //
    /// <summary>
    /// Writes to and closes the m_sTempFileName temporary file.
    /// </summary>
	///
	/// <param name="sGraphText">
	/// Text to write to the file.
	/// </param>
    //*************************************************************************

    protected void
    WriteGraphToTempFile
	(
		String sGraphText
	)
    {
		using ( StreamWriter oStreamWriter =
			new StreamWriter(m_sTempFileName) )
		{
			oStreamWriter.Write(sGraphText);
		}
    }

    //*************************************************************************
    //  Method: WriteGraphToClipboard()
    //
    /// <summary>
    /// Writes to the clipboard.
    /// </summary>
	///
	/// <param name="sGraphText">
	/// Text to write to the clipboard.
	/// </param>
    //*************************************************************************

    protected void
    WriteGraphToClipboard
	(
		String sGraphText
	)
    {
		Clipboard.SetDataObject(sGraphText);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Object to test.

	protected CommandLineParser m_oCommandLineParser;

	/// Name of the temporary file that may be created by the unit tests.

	protected String m_sTempFileName;
}

}
