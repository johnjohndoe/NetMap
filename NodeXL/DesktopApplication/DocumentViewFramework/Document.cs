
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Adapters;

namespace Microsoft.NodeXL.DesktopApplication
{
//*****************************************************************************
//	Class: Document
//
/// <summary>
/// Represents a unit of data that the user can create or open from a file.
/// </summary>
///
/// <remarks>
/// A document is what the user opens in the application.  A document can be
/// loaded from a file using <see
/// cref="Load(String, Boolean, out Document)" /> and saved to a file using
/// <see cref="Save(String, Boolean)" />.  A document has an attached view that
/// displays the document.  The application is MDI, so multiple documents can
/// be open simultaneously.
///
/// <para>
/// A document contains a <see cref="GraphData" /> object that contains data
/// for one graph.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class Document : Object
{
	//*************************************************************************
	//	Constructor: Document()
	//
	/// <overloads>
	/// Initializes a new instance of the <see cref="Document" /> class.
	/// </overloads>
	///
	/// <summary>
	/// Initializes a new instance of the <see cref="Document" /> class with a
	/// specified <see cref="GraphData" /> and file name.
	/// </summary>
	///
	/// <param name="oGraphData">
	/// <see cref="GraphData" /> object that was loaded from <paramref
	/// name="sFileName" />.
	/// </param>
	///
	/// <param name="sFileName">
	/// Name of the file from which <paramref name="oGraphData" /> was loaded,
	/// with full path.
	/// </param>
	///
	/// <remarks>
	/// Use this constructor when creating a <see cref="Document" /> from a
	/// loaded <see cref="GraphData" /> object.
	/// </remarks>
	//*************************************************************************

	public Document
	(
		GraphData oGraphData,
		String sFileName
	)
	: this( oGraphData, sFileName, Path.GetFileName(sFileName) )
	{
		Debug.Assert( !String.IsNullOrEmpty(sFileName) );

		AssertValid();
	}

	//*************************************************************************
	//	Constructor: Document()
	//
	/// <summary>
	/// Initializes a new instance of the <see cref="Document" /> class with a
	/// specified title.
	/// </summary>
	///
	/// <param name="sTitle">
	/// The document's title.  This is what gets displayed in the title bar of
	/// the document's view.  Can't be null or empty.
	/// </param>
	///
	/// <remarks>
	/// Use this constructor when creating a new document.
	/// </remarks>
	//*************************************************************************

	public Document
	(
		String sTitle
	)
	: this(new GraphData(), null, sTitle)
	{
		// (Do nothing else.)

		AssertValid();
	}

    //*************************************************************************
    //  Constructor: Document()
    //
    /// <summary>
	/// Initializes a new instance of the <see cref="Document" /> class with 
	/// specified graph data, file name, and title.
    /// </summary>
	///
	/// <param name="oGraphData">
	/// <see cref="GraphData" /> object to use.  Can't be null.
	/// </param>
	///
	/// <param name="sFileName">
	/// Name of the file from which <paramref name="oGraphData" /> was loaded,
	/// with full path, or null if this is a new document.
	/// </param>
	///
	/// <param name="sTitle">
	/// Document's title.  Can't be null or empty.
	/// </param>
    //*************************************************************************

    public Document
	(
		GraphData oGraphData,
		String sFileName,
		String sTitle
	)
    {
		Debug.Assert(oGraphData != null);
		// sFileName
		Debug.Assert( !String.IsNullOrEmpty(sTitle) );

		m_oView = null;
		m_sTitle = sTitle;
		m_sFileName = sFileName;
		m_oGraphData = oGraphData;
		m_bIsDirty = false;

		AssertValid();
    }

	//*************************************************************************
	//	Property: View
	//
	/// <summary>
	/// Gets and sets the view associated with the document.
	/// </summary>
	///
	/// <value>
	/// The view associated with the document.
	/// </value>
	///
	/// <remarks>
	/// Do not get this property without setting it first.
	/// </remarks>
	//*************************************************************************

	public View
	View
	{
		get
		{
			AssertValid();
			Debug.Assert(m_oView != null);

			return (m_oView);
		}

		set
		{
			Debug.Assert(value != null);

			m_oView = value;

			AssertValid();
		}
	}

	//*************************************************************************
	//	Property: Title
	//
	/// <summary>
	/// Gets the document's title.
	/// </summary>
	///
	/// <value>
	/// The title of the document.  Never null or empty.
	/// </value>
	///
	/// <remarks>
	/// The title is initially set to the title passed to the constructor.  If
	/// the document is saved to or loaded from a file, the title gets changed
	/// to the file name without a path.
	/// </remarks>
	//*************************************************************************

	public String
	Title
	{
		get
		{
			AssertValid();

			return (m_sTitle);
		}
	}

	//*************************************************************************
	//	Property: FileName
	//
	/// <summary>
	/// Gets the document's file name.
	/// </summary>
	///
	/// <value>
	/// The full path to the file where the document was last saved, or null
	/// if the document hasn't been saved yet.
	/// </value>
	//*************************************************************************

	public String
	FileName
	{
		get
		{
			AssertValid();

			return (m_sFileName);
		}
	}

	//*************************************************************************
	//	Property: GraphData
	//
	/// <summary>
	/// Gets the data for one graph.
	/// </summary>
	///
	/// <value>
	/// A <see cref="GraphData" /> object that contains data for one graph.
	/// </value>
	//*************************************************************************

	public GraphData
	GraphData
	{
		get
		{
			AssertValid();

			return (m_oGraphData);
		}

	}

	//*************************************************************************
	//	Property: HasBeenSaved
	//
	/// <summary>
	/// Gets a flag indicating whether the document has been saved to a file.
	/// </summary>
	///
	/// <value>
	/// true if the document has been saved to a file.
	/// </value>
	//*************************************************************************

	public Boolean
	HasBeenSaved
	{
		get
		{
			AssertValid();

			return (m_sFileName != null);
		}
	}

	//*************************************************************************
	//	Property: IsDirty
	//
	/// <summary>
	/// Gets a flag indicating whether the document has been modified.
	/// </summary>
	///
	/// <value>
	/// true if the document has been modified since it was created.
	/// </value>
	//*************************************************************************

	public Boolean
	IsDirty
	{
		get
		{
			AssertValid();

			return (m_bIsDirty);
		}
	}

	//*************************************************************************
	//	Method: Save()
	//
	/// <overloads>
	/// Saves the document to a file.
	/// </overloads>
	///
	/// <summary>
	/// Saves the document to a file using an <see cref="IGraphAdapter" />
	/// determined by the file's extension.
	/// </summary>
	///
	/// <param name="sFileName">
	/// File name, with a full path.
	/// </param>
	///
	/// <param name="bUseMessageBoxForError">
	/// If true and an error occurs, an error message is displayed in a message
	/// box and false is returned.  If false and an error occurs, an exception
	/// is thrown.
	/// </param>
	///
	/// <returns>
	/// If <paramref name="bUseMessageBoxForError" /> is true, true is returned
	/// if the save succeeded and false is returned if it failed.  If <paramref
	/// name="bUseMessageBoxForError" /> is false, true is returned if the save
	/// succeeded and an exception is thrown if it failed.
	/// </returns>
	///
	/// <remarks>
	/// The document's <see cref="Title" /> is changed to <paramref
	/// name="sFileName" /> with the path removed.
	/// </remarks>
	//*************************************************************************

	public Boolean
	Save
	(
		String sFileName,
		Boolean bUseMessageBoxForError
	)
	{
		Debug.Assert( !String.IsNullOrEmpty(sFileName) );
		AssertValid();

		return ( Save(
			sFileName,
			GraphAdapterManager.FileNameToGraphAdapter(sFileName),
			bUseMessageBoxForError
			) );
	}

	//*************************************************************************
	//	Method: Save()
	//
	/// <summary>
	/// Saves the document to a file using a specified <see
	/// cref="IGraphAdapter" />.
	/// </summary>
	///
	/// <param name="sFileName">
	/// File name, with a full path.
	/// </param>
	///
	/// <param name="oGraphAdapter">
	/// <see cref="IGraphAdapter" /> to use to save the file.  (<see
	/// cref="GraphAdapterManager" /> can be used to create an appropriate
	/// adapter.)
	/// </param>
	///
	/// <param name="bUseMessageBoxForError">
	/// If true and an error occurs, an error message is displayed in a message
	/// box and false is returned.  If false and an error occurs, an exception
	/// is thrown.
	/// </param>
	///
	/// <returns>
	/// If <paramref name="bUseMessageBoxForError" /> is true, true is returned
	/// if the save succeeded and false is returned if it failed.  If <paramref
	/// name="bUseMessageBoxForError" /> is false, true is returned if the save
	/// succeeded and an exception is thrown if it failed.
	/// </returns>
	///
	/// <remarks>
	/// The document's <see cref="Title" /> is changed to <paramref
	/// name="sFileName" /> with the path removed.
	/// </remarks>
	//*************************************************************************

	public Boolean
	Save
	(
		String sFileName,
		IGraphAdapter oGraphAdapter,
		Boolean bUseMessageBoxForError
	)
	{
		Debug.Assert( !String.IsNullOrEmpty(sFileName) );
		Debug.Assert(oGraphAdapter != null);
		AssertValid();

		Exception oException = null;

		try
		{
			oGraphAdapter.SaveGraph(m_oGraphData.Graph, sFileName);
		}
		catch (IOException oIOException)
		{
			oException = oIOException;
		}
		catch (UnauthorizedAccessException oUnauthorizedAccessException)
		{
			oException = oUnauthorizedAccessException;
		}

		if (oException != null)
		{
			if (bUseMessageBoxForError)
			{
				FormUtil.ShowError( 
					"The file could not be saved.\n\n"
					+ oException.Message
					);

				return (false);
			}

			throw (oException);
		}

		// The save was successful.  Save the file name, then update the title
		// to be the file name without a path.

		m_sFileName = sFileName;

		SetTitle( Path.GetFileName(sFileName) );

		m_bIsDirty = false;

		return (true);
	}

	//*************************************************************************
	//	Method: Load()
	//
	/// <overloads>
	///	Loads a document from a file.
	/// </overloads>
	///
	/// <summary>
	///	Loads a document from a file using an <see cref="IGraphAdapter" />
	/// determined by the file's extension.
	/// </summary>
	///
	/// <param name="sFileName">
	/// Name of the file to load, with a full path.
	/// </param>
	///
	/// <param name="bUseMessageBoxForError">
	/// If true and an error occurs, an error message is displayed in a message
	/// box and false is returned.  If false and an error occurs, an exception
	/// is thrown.
	/// </param>
	///
	/// <param name="oDocument">
	/// Where the new <see cref="Document" /> object gets stored when true is
	/// returned.
	/// </param>
	///
	/// <returns>
	/// If <paramref name="bUseMessageBoxForError" /> is true, true is returned
	/// if the load succeeded and false is returned if it failed.  If <paramref
	/// name="bUseMessageBoxForError" /> is false, true is returned if the load
	/// succeeded and an exception is thrown if it failed.
	/// </returns>
	///
	/// <remarks>
	///	This method opens the specified file, loads a Document object from it,
	/// and closes the file.  The document's <see cref="Title" /> is changed to
	/// <paramref name="sFileName" /> with the path removed.
	/// </remarks>
	//*************************************************************************

	public static Boolean
	Load
	(
		String sFileName,
		Boolean bUseMessageBoxForError,
		out Document oDocument
	)
	{
		Debug.Assert( !String.IsNullOrEmpty(sFileName) );

		return ( Load(
			sFileName,
			GraphAdapterManager.FileNameToGraphAdapter(sFileName),
			bUseMessageBoxForError,
			out oDocument
			) );
	}

	//*************************************************************************
	//	Method: Load()
	//
	/// <summary>
	///	Loads a document from a file using a specified <see
	/// cref="IGraphAdapter" />.
	/// </summary>
	///
	/// <param name="sFileName">
	/// Name of the file to load, with a full path.
	/// </param>
	///
	/// <param name="oGraphAdapter">
	/// <see cref="IGraphAdapter" /> to use to load the file.  (<see
	/// cref="GraphAdapterManager" /> can be used to create an appropriate
	/// adapter.)
	/// </param>
	///
	/// <param name="bUseMessageBoxForError">
	/// If true and an error occurs, an error message is displayed in a message
	/// box and false is returned.  If false and an error occurs, an exception
	/// is thrown.
	/// </param>
	///
	/// <param name="oDocument">
	/// Where the new <see cref="Document" /> object gets stored when true is
	/// returned.
	/// </param>
	///
	/// <returns>
	/// If <paramref name="bUseMessageBoxForError" /> is true, true is returned
	/// if the load succeeded and false is returned if it failed.  If <paramref
	/// name="bUseMessageBoxForError" /> is false, true is returned if the load
	/// succeeded and an exception is thrown if it failed.
	/// </returns>
	///
	/// <remarks>
	///	This method opens the specified file, loads a Document object from it,
	/// and closes the file.  The document's <see cref="Title" /> is changed to
	/// <paramref name="sFileName" /> with the path removed.
	/// </remarks>
	//*************************************************************************

	public static Boolean
	Load
	(
		String sFileName,
		IGraphAdapter oGraphAdapter,
		Boolean bUseMessageBoxForError,
		out Document oDocument
	)
	{
		Debug.Assert( !String.IsNullOrEmpty(sFileName) );
		Debug.Assert(oGraphAdapter != null);

		oDocument = null;

		// Use the graph adapter to create a graph from the file.

		IGraph oGraph = null;

		Exception oException = null;

		try
		{
			oGraph = oGraphAdapter.LoadGraph(sFileName);
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
			if (bUseMessageBoxForError)
			{
				FormUtil.ShowError( 
					"The file could not be opened.\n\n"
					+ oException.Message
					);

				return (false);
			}

			throw (oException);
		}

		// The graph was successfuly created.  Wrap it in a GraphData object
		// and create a new document.

        GraphData oGraphData = new GraphData(oGraph);

		oDocument = new Document(oGraphData, sFileName);

		return (true);
	}

	//*************************************************************************
	//	Event: TitleChanged
	//
	/// <summary>
	///	Occurs when the document's title changes.
	/// </summary>
	///
	/// <remarks>
	/// This event occurs when the document's title changes.  The new title can
	/// be obtained from the <see cref="Title" /> property.
	/// </remarks>
	//*************************************************************************

	public event EventHandler TitleChanged;


	//*************************************************************************
	//	Method: SetTitle()
	//
	/// <summary>
	/// Sets the document's title.
	/// </summary>
	///
	/// <param name="sTitle">
	/// The document's title.
	/// </param>
	//*************************************************************************

	protected void
	SetTitle
	(
		String sTitle
	)
	{
		Debug.Assert( !String.IsNullOrEmpty(sTitle) );

		if (sTitle == m_sTitle)
		{
			return;
		}

		m_sTitle = sTitle;

		EventHandler oEventHandler = this.TitleChanged;

		if (oEventHandler != null)
		{
			oEventHandler(this, EventArgs.Empty);
		}
	}


	//*************************************************************************
	//	Method: AssertValid()
	//
	/// <summary>
	///	Asserts if the object is in an invalid state.  Debug-only.
	/// </summary>
	//*************************************************************************

	// [Conditional("DEBUG")]

	public void
	AssertValid()
	{
		// m_oView
		Debug.Assert( !String.IsNullOrEmpty(m_sTitle) );
		// m_sFileName
		Debug.Assert(m_oGraphData != null);
		// m_bIsDirty
	}


	//*************************************************************************
	//	Protected fields
	//*************************************************************************

	/// The view associated with this document, or null if the View property
	/// hasn't been set yet.

	protected View m_oView;

	/// The document's title.  Can't be null or empty.

	protected String m_sTitle;

	/// Name of the file the document was saved to, or null if the document
	/// hasn't been saved to a file.

	protected String m_sFileName;

	/// Contains data for one graph.

	protected GraphData m_oGraphData;

	/// true if the document has been modified since it was created.

	protected Boolean m_bIsDirty;
}

}
