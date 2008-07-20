
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Diagnostics;
using Microsoft.NetMap.Adapters;

namespace Microsoft.NetMap.DesktopApplication
{
//*****************************************************************************
//  Class: GraphAdapterManager
//
/// <summary>
/// Contains static properties and methods for dealing with graph adapters,
/// which are classes that implement <see cref="IGraphAdapter" />.
/// </summary>
//*****************************************************************************

public static class GraphAdapterManager : Object
{
	//*************************************************************************
	//	Property: FileDialogFilter
	//
	/// <summary>
	/// Gets the filter to use in the dialogs that open and save graphs.
	/// </summary>
	///
	/// <value>
	/// The filter to use.
	/// </value>
	///
	/// <remarks>
	/// This is meant for use within the dialogs that open and save graph
	/// files.  The dialog's FileDialog.Filter property should be set to the
	/// String returned by this property.
	///
	/// <para>
	/// After the user selects a file, an <see cref="IGraphAdapter" /> can be
	/// created using <see cref="FileDialogFilterIndexToGraphAdapter" />.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public static String
	FileDialogFilter
	{
		get
		{
			return (FileDialogFilterString);
		}
	}

	//*************************************************************************
	//	Property: GraphAdapterNames
	//
	/// <summary>
	/// Gets the names of the available graph adapters.
	/// </summary>
	///
	/// <value>
	/// Names of the available graph adapters, separated by vertical bars.
	/// </value>
	///
	/// <remarks>
	/// This is meant for use within command-line usage messages.
	///
	/// <para>
	/// A graph adapter can be created from a graph adapter name using <see
	/// cref="TryGraphAdapterNameToGraphAdapter" />.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public static String
	GraphAdapterNames
	{
		get
		{
			return (GraphAdapterNameStrings);
		}
	}

    //*************************************************************************
    //  Method: TryGraphAdapterNameToGraphAdapter()
    //
    /// <summary>
	/// Attempts to create an <see cref="IGraphAdapter" /> from a graph adapter
	/// name.
    /// </summary>
    ///
    /// <param name="graphAdapterName">
	/// Possibly one of the names returned by <see cref="GraphAdapterNames" />.
	/// Can't be null.  Case is insignificant.
    /// </param>
    ///
    /// <param name="graphAdapter">
    /// Where a <see cref="IGraphAdapter" /> corresponding to <paramref
	/// name="graphAdapterName" /> gets stored if true is returned.
    /// </param>
	///
    /// <returns>
	/// true if the name was recognized.
    /// </returns>
	///
    /// <remarks>
	/// If <paramref name="graphAdapterName" /> is one of the names returned by
	/// <see cref="GraphAdapterNames" />, an <see cref="IGraphAdapter" />
	/// corresponding to the name is stored at <paramref name="graphAdapter" />
	/// and true is returned.  false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGraphAdapterNameToGraphAdapter
    (
        String graphAdapterName,
		out IGraphAdapter graphAdapter
    )
    {
		Debug.Assert(graphAdapterName != null);

		graphAdapter = null;

		switch ( graphAdapterName.ToLower() )
		{
			case "simple":

				graphAdapter = new SimpleGraphAdapter();
				break;

			case "pajek":

				graphAdapter = new PajekGraphAdapter();
				break;

			default:

				break;
		}

		return (graphAdapter != null);
    }

	//*************************************************************************
	//	Method: FileDialogFilterIndexToGraphAdapter()
	//
	/// <summary>
	/// Creates an <see cref="IGraphAdapter" /> determined by a filter index.
	/// </summary>
	///
	///	<param name="oneBasedFilterIndex">
	/// One-based index returned by FileDialog.FilterIndex.
	/// </param>
	///
    /// <returns>
    /// An <see cref="IGraphAdapter" /> corresponding to <paramref
	/// name="oneBasedFilterIndex" />.
    /// </returns>
	///
	/// <remarks>
	/// This is meant for use within the dialogs that open and save graph
	/// files.  It's assumed that the dialog's FileDialog.Filter property has
	/// been set to <see cref="FileDialogFilter" />.
	/// </remarks>
	//*************************************************************************

	public static IGraphAdapter
	FileDialogFilterIndexToGraphAdapter
	(
		Int32 oneBasedFilterIndex
	)
	{
		const String MethodName = "FileDialogFilterIndexToGraphAdapter";

		switch (oneBasedFilterIndex)
		{
			case 1:

				return ( new SimpleGraphAdapter() );

			case 2:

				return ( new PajekGraphAdapter() );

			default:

				Debug.Assert(false);

				throw new IndexOutOfRangeException( String.Format(

					"{0}.{1}: The specified index does not correspond to a"
					+ " graph adapter."
					,
					ClassName,
					MethodName
					) );
		}
	}

    //*************************************************************************
    //  Method: FileNameToGraphAdapter()
    //
    /// <summary>
	/// Creates an <see cref="IGraphAdapter" /> suitable for use with a
	/// specified file.
    /// </summary>
    ///
    /// <param name="filename">
	/// Name of the file to create a graph adapter for, with or without a path.
	/// Can't be null or empty.
    /// </param>
    ///
    /// <returns>
    /// An <see cref="IGraphAdapter" /> suitable for use with <paramref
	/// name="filename" />.
    /// </returns>
    ///
    /// <remarks>
	/// The file name's extension is used to determine which graph adapter to
	/// return.  If the extension is not recognized, a <see
	/// cref="SimpleGraphAdapter" /> is returned.
    /// </remarks>
    //*************************************************************************

    public static IGraphAdapter
    FileNameToGraphAdapter
    (
        String filename
    )
    {
		Debug.Assert( !String.IsNullOrEmpty(filename) );

		switch ( Path.GetExtension(filename).ToLower() )
		{
			case ".txt":

				break;

			case ".net":

				return ( new PajekGraphAdapter() );

			default:

				break;
		}

		return ( new SimpleGraphAdapter() );
    }

	//*************************************************************************
	//	Property: ClassName
	//
	/// <summary>
	/// Gets the full name of the class.
	/// </summary>
	///
	/// <value>
	/// The full name of the class, suitable for use in error messages.
	/// </value>
	//*************************************************************************

	private static String
	ClassName
	{
		get
		{
			return (typeof(GraphAdapterManager).FullName);
		}
	}


	//*************************************************************************
	//	Protected constants
	//*************************************************************************

	/// Filter to use for open and save dialogs.

	private static readonly String FileDialogFilterString =

		"Simple Tab-Delimited Files (*.txt)|*.txt"

		+ "|Pajek Files (*.net)|*.net"
		;

	/// Names of available graph adapters, separated by vertical bars.

	private static readonly String GraphAdapterNameStrings =

		"Simple|Pajek"
		;
}

}
