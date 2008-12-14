using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Adapters
{
//*****************************************************************************
//  Class: GraphAdapterBase
//
/// <summary>
///	Base class for graph adapters.
/// </summary>
///
/// <remarks>
/// This abstract class can serve as a base class for <see
/// cref="IGraphAdapter" /> implementations.  Its implementations of the <see
/// cref="IGraphAdapter" /> public methods provide error checking but defer the
/// actual work to protected abstract methods.
/// </remarks>
//*****************************************************************************

public abstract class GraphAdapterBase : AdapterBase, IGraphAdapter
{
    //*************************************************************************
    //  Constructor: GraphAdapterBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphAdapterBase" />
	/// class.
    /// </summary>
    //*************************************************************************

    public GraphAdapterBase()
    {
		// (Do nothing.)
    }

    //*************************************************************************
    //  Method: LoadGraph()
    //
    /// <overloads>
    /// Creates a graph and loads it with graph data.
    /// </overloads>
	///
    /// <summary>
    /// Creates a graph of type <see cref="Graph" /> and loads it with graph
	/// data read from a file.
    /// </summary>
    ///
    /// <param name="filename">
    /// Full path to the file containing graph data.
    /// </param>
    ///
	/// <returns>
	/// A new <see cref="Graph" /> loaded with graph data read from <paramref
	/// name="filename" />.
	/// </returns>
	///
	/// <remarks>
	///	This method creates a <see cref="Graph" /> and loads it with the graph
	/// data read from <paramref name="filename" />.
	/// </remarks>
    //*************************************************************************

    public IGraph
    LoadGraph
    (
		String filename
    )
	{
		AssertValid();

		const String MethodName = "LoadGraph";

		this.ArgumentChecker.CheckArgumentNotEmpty(
			MethodName, "filename", filename);

		Stream oStream = null;

		IGraph oGraph;

		try
		{
			oStream = new FileStream(filename, FileMode.Open, FileAccess.Read);

			oGraph = LoadGraph(new GraphFactory(), oStream);
		}
		finally
		{
            if (oStream != null)
            {
                oStream.Close();
                oStream = null;
            }
		}

		return (oGraph);
	}

    //*************************************************************************
    //  Method: LoadGraph()
    //
    /// <summary>
    /// Creates a graph of a specified type and loads it with graph data read
	/// from a <see cref="String" />.
    /// </summary>
    ///
    /// <param name="graphFactory">
    /// Object that knows how to create a graph.
    /// </param>
	///
    /// <param name="theString">
    /// <see cref="String" /> containing graph data.
    /// </param>
    ///
	/// <returns>
	/// A new graph created by <paramref name="graphFactory" /> and loaded with
	/// graph data read from <paramref name="theString" />.
	/// </returns>
	///
	/// <remarks>
	///	This method creates a graph using <paramref name="graphFactory" /> and
	/// loads it with the graph data read from <paramref name="theString" />.
	/// </remarks>
    //*************************************************************************

    public IGraph
    LoadGraph
    (
		IGraphFactory graphFactory,
		String theString
    )
	{
		AssertValid();

		const String MethodName = "LoadGraph";

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "graphFactory", graphFactory);

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "theString", theString);

        MemoryStream oMemoryStream = null;

        try
        {
            oMemoryStream = new MemoryStream(
				Encoding.UTF8.GetBytes(theString), false);

            return ( LoadGraph(graphFactory, oMemoryStream) );
        }
        finally
        {
            if (oMemoryStream != null)
            {
                oMemoryStream.Close();
            }
        }
	}

    //*************************************************************************
    //  Method: LoadGraph()
    //
    /// <summary>
    /// Creates a graph of a specified type and loads it with graph data read
	/// from a <see cref="Stream" />.
    /// </summary>
    ///
    /// <param name="graphFactory">
    /// Object that knows how to create a graph.
    /// </param>
	///
    /// <param name="stream">
    /// <see cref="Stream" /> containing graph data.
    /// </param>
    ///
	/// <returns>
	/// A new graph created by <paramref name="graphFactory" /> and loaded with
	/// graph data read from <paramref name="stream" />.
	/// </returns>
	///
	/// <remarks>
	///	This method creates a graph using <paramref name="graphFactory" /> and
	/// loads it with the graph data read from <paramref name="stream" />.  It
	/// does not close <paramref name="stream" />.
	/// </remarks>
    //*************************************************************************

    public IGraph
    LoadGraph
    (
		IGraphFactory graphFactory,
		Stream stream
    )
	{
		AssertValid();

		const String MethodName = "LoadGraph";

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "graphFactory", graphFactory);

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "stream", stream);

		return ( LoadGraphCore(graphFactory, stream) );
	}

    //*************************************************************************
    //  Method: SaveGraph()
    //
    /// <overloads>
    /// Saves graph data.
    /// </overloads>
	///
    /// <summary>
    /// Saves graph data to a file.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to save.
    /// </param>
    ///
    /// <param name="filename">
	/// Full path to the file to save to.  If the file exists, it gets
	/// overwritten.
    /// </param>
    ///
    /// <remarks>
	/// This method saves <paramref name="graph" /> to <paramref
	///	name="filename" />.
	///
	/// <para>
	///	If the <see cref="IGraph.Directedness" /> property on <paramref
	/// name="graph" /> is set to a value that is incompatible with the graph
	/// adapter, an exception is thrown.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    public void
    SaveGraph
    (
        IGraph graph,
		String filename
    )
	{
		AssertValid();

		const String MethodName = "SaveGraph";

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "graph", graph);

		this.ArgumentChecker.CheckArgumentNotEmpty(
			MethodName, "filename", filename);

		Stream oStream = null;

		try
		{
			oStream = new FileStream(filename, FileMode.Create);

			SaveGraph(graph, oStream);
		}
		finally
		{
            if (oStream != null)
            {
                oStream.Close();
                oStream = null;
            }
		}
	}

    //*************************************************************************
    //  Method: SaveGraph()
    //
    /// <summary>
    /// Saves graph data to a <see cref="Stream" />.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to save.
    /// </param>
    ///
    /// <param name="stream">
    /// <see cref="Stream" /> to save the graph data to.
    /// </param>
    ///
    /// <remarks>
	/// This method saves <paramref name="graph" /> to <paramref
	///	name="stream" />.  It does not close <paramref name="stream" />.
	///
	/// <para>
	///	If the <see cref="IGraph.Directedness" /> property on <paramref
	/// name="graph" /> is set to a value that is incompatible with the graph
	/// adapter, an exception is thrown.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    public void
    SaveGraph
    (
        IGraph graph,
		Stream stream
    )
	{
		AssertValid();

		const String MethodName = "SaveGraph";

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "graph", graph);

		CheckGraphDirectedness(graph, false);

		this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "stream", stream);

		SaveGraphCore(graph, stream);
	}

    //*************************************************************************
    //  Method: SupportsDirectedness()
    //
    /// <summary>
    /// Returns a flag indicating whether the graph adapter can be used with
	/// graphs of a specified <see cref="GraphDirectedness" />.
    /// </summary>
    ///
    /// <param name="directedness">
	/// A <see cref="GraphDirectedness" /> value.
    /// </param>
    ///
	/// <returns>
	/// true if the graph adapter can be used with graphs of the specified
	/// directedness.
	/// </returns>
    //*************************************************************************

    public Boolean
    SupportsDirectedness
    (
		GraphDirectedness directedness
    )
	{
		AssertValid();

		const String MethodName = "SupportsDirectedness";
		const String ArgumentName = "directedness";

		this.ArgumentChecker.CheckArgumentIsDefined(
			MethodName, ArgumentName, directedness, typeof(GraphDirectedness)
			);

		Boolean bSupportsDirected, bSupportsUndirected, bSupportsMixed;

		GetSupportedDirectedness(
			out bSupportsDirected, out bSupportsUndirected, out bSupportsMixed);

		switch (directedness)
		{
			case GraphDirectedness.Directed:

				return (bSupportsDirected);

			case GraphDirectedness.Undirected:

				return (bSupportsUndirected);

			case GraphDirectedness.Mixed:

				return (bSupportsMixed);

			default:

				Debug.Assert(false);
				return (false);
		}
	}

    //*************************************************************************
    //  Method: GetSupportedDirectedness()
    //
    /// <summary>
	///	Gets a set of flags indicating the directedness of the graphs that the
	/// implementation can load and save.
    /// </summary>
    ///
    /// <param name="supportsDirected">
	/// Gets set to true if the implementation can load and save directed
	/// graphs.
    /// </param>
	///
    /// <param name="supportsUndirected">
	/// Gets set to true if the implementation can load and save undirected
	/// graphs.
    /// </param>
	///
    /// <param name="supportsMixed">
	/// Gets set to true if the implementation can load and save mixed graphs.
    /// </param>
    //*************************************************************************

    protected abstract void
    GetSupportedDirectedness
	(
		out Boolean supportsDirected,
		out Boolean supportsUndirected,
		out Boolean supportsMixed
	);

    //*************************************************************************
    //  Method: LoadGraphCore()
    //
    /// <summary>
    /// Creates a graph of a specified type and loads it with graph data read
	/// from a <see cref="Stream" />.
    /// </summary>
    ///
    /// <param name="graphFactory">
    /// Object that can create a graph.
    /// </param>
	///
    /// <param name="stream">
    /// <see cref="Stream" /> containing graph data.
    /// </param>
    ///
	/// <returns>
	/// A new graph created by <paramref name="graphFactory" /> and loaded with
	/// graph data read from <paramref name="stream" />.
	/// </returns>
	///
	/// <remarks>
	///	This method creates a graph using <paramref name="graphFactory" /> and
	/// loads it with the graph data read from <paramref name="stream" />.  It
	/// does not close <paramref name="stream" />.
	///
	/// <para>
	/// The arguments have already been checked for validity.
	/// </para>
	///
	/// </remarks>
    //*************************************************************************

    protected abstract IGraph
    LoadGraphCore
    (
        IGraphFactory graphFactory,
		Stream stream
    );

    //*************************************************************************
    //  Method: SaveGraphCore()
    //
    /// <summary>
    /// Saves graph data to a stream.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to save.
    /// </param>
    ///
    /// <param name="stream">
    /// Stream to save the graph data to.
    /// </param>
    ///
    /// <remarks>
	/// This method saves <paramref name="graph" /> to <paramref
	///	name="stream" />.  It does not close <paramref name="stream" />.
	///
	/// <para>
	/// The arguments have already been checked for validity.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

    protected abstract void
    SaveGraphCore
    (
        IGraph graph,
		Stream stream
    );

    //*************************************************************************
    //  Method: CheckGraphDirectedness()
    //
    /// <summary>
    /// Checks whether the directedness of a graph is supported by the
	/// implementation.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to check.
    /// </param>
    ///
    /// <param name="loading">
    /// true if this method is being called while a graph is being loaded,
	/// false if it is being called while a graph is being saved.
    /// </param>
    ///
    /// <remarks>
	/// An exception is thrown if the directedness of <paramref name="graph" />
	/// is not supported by the implementation.
    /// </remarks>
    //*************************************************************************

	protected void
	CheckGraphDirectedness
	(
		IGraph graph,
		Boolean loading
	)
	{
		AssertValid();

		GraphDirectedness eDirectedness = graph.Directedness;

		if ( SupportsDirectedness(eDirectedness) )
		{
			return;
		}

		throw new IOException( String.Format(

			"The graph can't be {0} as the file type you've selected, because"
			+ " the graph is {1} and the file type can't be used with {1}"
			+ " graphs."
			,
			loading ? "loaded" : "saved",

			EnumUtil.SplitName(eDirectedness.ToString(),
				EnumSplitStyle.AllWordsStartLowerCase)
			) );
	}

    //*************************************************************************
    //  Method: VertexCountToString()
    //
    /// <summary>
    /// Returns a string describing a vertex count.
    /// </summary>
    ///
    /// <param name="vertices">
    /// Number of vertices.  Must be non-negative.
    /// </param>
    ///
	/// <returns>
	/// A string describing <paramref name="vertices" />, in the format
	/// "N vertices".
	/// </returns>
    //*************************************************************************

    protected String
    VertexCountToString
    (
		Int32 vertices
    )
	{
		Debug.Assert(vertices >= 0);

		return ( String.Format(
			"{0} {1}"
			,
			vertices.ToString(NodeXLBase.Int32Format),
			(vertices == 1) ? "vertex" : "vertices"
			) );
	}

    //*************************************************************************
    //  Method: OnLoadFormatError()
    //
    /// <summary>
    /// Handles a formatting error detected by <see cref="LoadGraphCore" />.
    /// </summary>
    ///
    /// <param name="line">
    /// Line that is incorrectly formatted.  Can't be null.
    /// </param>
    ///
    /// <param name="lineNumber">
    /// One-based line number of <paramref name="line" />.
    /// </param>
	///
    /// <param name="expectedFormat">
    /// Description of what a correctly-formatted line should look like.
    /// </param>
	///
	/// <remarks>
	/// If the derived class reads from a line-oriented text file and detects
	/// a formatting error on a line, it should handle the error by calling
	/// this method or <see cref="OnLoadFormatError2" />.  A <see
	/// cref="FormatException" /> is thrown.
	/// </remarks>
    //*************************************************************************

    protected void
    OnLoadFormatError
    (
		String line,
		Int32 lineNumber,
		String expectedFormat
    )
	{
		Debug.Assert(line != null);
		Debug.Assert(lineNumber >= 1);
		Debug.Assert( !String.IsNullOrEmpty(expectedFormat) );
		AssertValid();

		OnLoadFormatError2(line, lineNumber,
	
			String.Format(

				"The expected format is \"{0}\"."
				,
				expectedFormat
			) );
	}

    //*************************************************************************
    //  Method: OnLoadFormatError2()
    //
    /// <summary>
    /// Handles a formatting error detected by <see cref="LoadGraphCore" />.
    /// </summary>
    ///
    /// <param name="line">
    /// Line that is incorrectly formatted.  Can't be null.
    /// </param>
    ///
    /// <param name="lineNumber">
    /// One-based line number of <paramref name="line" />.
    /// </param>
	///
    /// <param name="errorDetails">
	/// Description of the formatting error.
    /// </param>
	///
	/// <remarks>
	/// If the derived class reads from a line-oriented text file and detects
	/// a formatting error on a line, it should handle the error by calling
	/// this method or <see cref="OnLoadFormatError" />.  A <see
	/// cref="FormatException" /> is thrown.
	/// </remarks>
    //*************************************************************************

    protected void
    OnLoadFormatError2
    (
		String line,
		Int32 lineNumber,
		String errorDetails
    )
	{
		Debug.Assert(line != null);
		Debug.Assert(lineNumber >= 1);
		Debug.Assert( !String.IsNullOrEmpty(errorDetails) );
		AssertValid();

		// Truncate the line if necessary.

		const Int32 MaxLineLengthToDisplay = 80;

		String sLineToDisplay = (line.Length > MaxLineLengthToDisplay) ?
			line.Substring(0, MaxLineLengthToDisplay) + "..."
			:
			line
			;

		// Replace control characters with a Unicode box.

		sLineToDisplay =
			StringUtil.ReplaceControlCharacters(sLineToDisplay, '\u25A1');

		// Do not include the class or method name in the exception message.
		// The message appears when the user attempts to open an invalid file,
		// and the user doesn't care about class names.
	
		throw new FormatException( String.Format(

			"Line {0} is not in the expected format.  This is"
			+ " line {0}: \"{1}\".  {2}"
			,
			lineNumber.ToString(NodeXLBase.Int32Format),
			sLineToDisplay,
			errorDetails
			) );
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
