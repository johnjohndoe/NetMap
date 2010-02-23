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
/// Base class for graph adapters.
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
    //  Method: LoadGraphFromFile()
    //
    /// <summary>
    /// Creates a graph and loads it with graph data read from a file.
    /// </summary>
    ///
    /// <param name="filename">
    /// Full path to the file containing graph data.
    /// </param>
    ///
    /// <returns>
    /// A new graph loaded with graph data read from <paramref
    /// name="filename" />.
    /// </returns>
    ///
    /// <remarks>
    /// This method creates a graph and loads it with the graph data read from
    /// <paramref name="filename" />.
    /// </remarks>
    //*************************************************************************

    public IGraph
    LoadGraphFromFile
    (
        String filename
    )
    {
        AssertValid();

        const String MethodName = "LoadGraphFromFile";

        this.ArgumentChecker.CheckArgumentNotEmpty(
            MethodName, "filename", filename);

        Stream oStream = null;

        IGraph oGraph;

        try
        {
            oStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            oGraph = LoadGraphFromStream(oStream);
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
    //  Method: LoadGraphFromString()
    //
    /// <summary>
    /// Creates a graph and loads it with graph data read from a <see
    /// cref="String" />.
    /// </summary>
    ///
    /// <param name="theString">
    /// <see cref="String" /> containing graph data.
    /// </param>
    ///
    /// <returns>
    /// A new graph loaded with graph data read from <paramref
    /// name="theString" />.
    /// </returns>
    ///
    /// <remarks>
    /// This method creates a graph and loads it with the graph data read from
    /// <paramref name="theString" />.
    /// </remarks>
    //*************************************************************************

    public IGraph
    LoadGraphFromString
    (
        String theString
    )
    {
        AssertValid();

        const String MethodName = "LoadGraph";

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, "theString", theString);

        MemoryStream oMemoryStream = null;

        try
        {
            oMemoryStream = new MemoryStream(
                Encoding.UTF8.GetBytes(theString), false);

            return ( LoadGraphFromStream(oMemoryStream) );
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
    //  Method: LoadGraphFromStream()
    //
    /// <summary>
    /// Creates a graph and loads it with graph data read from a <see
    /// cref="Stream" />.
    /// </summary>
    ///
    /// <param name="stream">
    /// <see cref="Stream" /> containing graph data.
    /// </param>
    ///
    /// <returns>
    /// A new graph loaded with graph data read from <paramref
    /// name="stream" />.
    /// </returns>
    ///
    /// <remarks>
    /// This method creates a graph and loads it with the graph data read from
    /// <paramref name="stream" />.  It does not close <paramref
    /// name="stream" />.
    /// </remarks>
    //*************************************************************************

    public IGraph
    LoadGraphFromStream
    (
        Stream stream
    )
    {
        AssertValid();

        const String MethodName = "LoadGraph";

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, "stream", stream);

        return ( LoadGraphCore(stream) );
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
    /// name="filename" />.
    ///
    /// <para>
    /// If the <see cref="IGraph.Directedness" /> property on <paramref
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
    /// name="stream" />.  It does not close <paramref name="stream" />.
    ///
    /// <para>
    /// If the <see cref="IGraph.Directedness" /> property on <paramref
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
    /// Gets a set of flags indicating the directedness of the graphs that the
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
    /// Creates a graph and loads it with graph data read from a <see
    /// cref="Stream" />.
    /// </summary>
    ///
    /// <param name="stream">
    /// <see cref="Stream" /> containing graph data.
    /// </param>
    ///
    /// <returns>
    /// A new graph loaded with graph data read from <paramref
    /// name="stream" />.
    /// </returns>
    ///
    /// <remarks>
    /// This method creates a graph, loads it with the graph data read from
    /// <paramref name="stream" />.  It does not close <paramref
    /// name="stream" />.
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
    /// name="stream" />.  It does not close <paramref name="stream" />.
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
    //  Method: TryReadLine()
    //
    /// <summary>
    /// Attempts to read a line from a StreamReader.
    /// </summary>
    ///
    /// <param name="streamReader">
    /// StreamReader to read a line from.
    /// </param>
    ///
    /// <param name="line">
    /// Where the line gets stored if true is returned.
    /// </param>
    ///
    /// <param name="lineNumber">
    /// Line number of <paramref name="line" />.
    /// </param>
    ///
    /// <returns>
    /// true if a line was read.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryReadLine
    (
        StreamReader streamReader,
        out String line,
        ref Int32 lineNumber
    )
    {
        Debug.Assert(streamReader != null);
        AssertValid();

        line = streamReader.ReadLine();

        if (line == null)
        {
            return (false);
        }

        lineNumber++;
        return (true);
    }

    //*************************************************************************
    //  Method: OnLoadFormatError()
    //
    /// <overloads>
    /// Handles a formatting error detected by <see cref="LoadGraphCore" />.
    /// </overloads>
    ///
    /// <summary>
    /// Handles a line-oriented formatting error detected by <see
    /// cref="LoadGraphCore" />.
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
    //  Method: OnLoadFormatError()
    //
    /// <summary>
    /// Handles a formatting error detected by <see cref="LoadGraphCore" />
    /// given a complete error message.
    /// </summary>
    ///
    /// <param name="completeErrorMessage">
    /// Full description of the formatting error.
    /// </param>
    //*************************************************************************

    protected void
    OnLoadFormatError
    (
        String completeErrorMessage
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(completeErrorMessage) );
        AssertValid();

        throw new FormatException(completeErrorMessage);
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
    /// this method or <see cref="OnLoadFormatError(String)" />.  A <see
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
    
        OnLoadFormatError( String.Format(

            "Line {0} is not in the expected format.  This is"
            + " line {0}: \"{1}\".  {2}"
            ,
            lineNumber.ToString(NodeXLBase.Int32Format),
            sLineToDisplay,
            errorDetails
            ) );
    }

    //*************************************************************************
    //  Method: OnSaveError()
    //
    /// <summary>
    /// Handles an error detected by <see cref="SaveGraphCore" />.
    /// </summary>
    ///
    /// <param name="errorMessage">
    /// Error message to include in the exception.
    /// </param>
    ///
    /// <remarks>
    /// If the derived class encounters an error when attempting to save a
    /// graph, it should handle the error by calling this method.  A <see
    /// cref="SaveGraphException" /> is thrown.
    /// </remarks>
    //*************************************************************************

    protected void
    OnSaveError
    (
        String errorMessage
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(errorMessage) );
        AssertValid();

        throw new SaveGraphException(errorMessage);
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
