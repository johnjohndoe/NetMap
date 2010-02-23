
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Adapters
{
//*****************************************************************************
//  Interface: IGraphAdapter
//
/// <summary>
/// Supports conversion of a graph to and from a specific data format.
/// </summary>
///
/// <remarks>
/// The NodeXL core components do not know anything about the various formats
/// commonly used to store graph data.  Instead, data conversion is
/// accomplished with a family of classes that implement the <see
/// cref="IGraphAdapter" /> interface.  Each such class knows how to load a
/// graph with data that is in a particular format, and to save a graph to that
/// format.
/// </remarks>
//*****************************************************************************

public interface IGraphAdapter
{
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

    IGraph
    LoadGraphFromFile
    (
        String filename
    );

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

    IGraph
    LoadGraphFromStream
    (
        Stream stream
    );

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

    IGraph
    LoadGraphFromString
    (
        String theString
    );

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

    void
    SaveGraph
    (
        IGraph graph,
        String filename
    );

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

    void
    SaveGraph
    (
        IGraph graph,
        Stream stream
    );

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

    Boolean
    SupportsDirectedness
    (
        GraphDirectedness directedness
    );
}

}
