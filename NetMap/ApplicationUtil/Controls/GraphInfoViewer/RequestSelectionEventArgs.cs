
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NetMap.Core;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NetMap.ApplicationUtil
{
//*****************************************************************************
//  Class: RequestSelectionEventArgs
//
/// <summary>
/// Provides event information for the <see
/// cref="GraphInfoViewer.RequestSelection" /> event.
/// </summary>
//*****************************************************************************

public class RequestSelectionEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: RequestSelectionEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="RequestSelectionEventArgs" /> class.
    /// </summary>
	///
    /// <param name="vertices">
	/// Vertices to select.  Can't be null.
    /// </param>
	///
    /// <param name="edges">
	/// Edges to select.  Can't be null.
    /// </param>
    //*************************************************************************

    public RequestSelectionEventArgs
	(
		IVertex [] vertices,
		IEdge [] edges
	)
    {
		const String MethodName = "Constructor";

        this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "vertices", vertices);

        this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "edges", edges);

		m_aoVertices = vertices;
		m_aoEdges = edges;

		AssertValid();
    }

    //*************************************************************************
    //  Property: Vertices
    //
    /// <summary>
	/// Gets the vertices to select.
    /// </summary>
    ///
    /// <value>
	/// The vertices to select.  This is never null.
    /// </value>
    //*************************************************************************

    public IVertex []
    Vertices
    {
        get
        {
            AssertValid();

            return (m_aoVertices);
        }
    }

    //*************************************************************************
    //  Property: Edges
    //
    /// <summary>
	/// Gets the edges to select.
    /// </summary>
    ///
    /// <value>
	/// The edges to select.  This is never null.
    /// </value>
    //*************************************************************************

    public IEdge []
    Edges
    {
        get
        {
            AssertValid();

            return (m_aoEdges);
        }
    }

	//*************************************************************************
	//	Property: ArgumentChecker
	//
	/// <summary>
	/// Gets a new initialized ArgumentChecker object.
	/// </summary>
	///
	/// <value>
	/// A new initialized ArgumentChecker object.
	/// </value>
	//*************************************************************************

	private ArgumentChecker
	ArgumentChecker
	{
		get
		{
			return ( new ArgumentChecker(this.GetType().FullName) );
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
		Debug.Assert(m_aoVertices != null);
		Debug.Assert(m_aoEdges != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Vertices to select.

	protected IVertex [] m_aoVertices;

	/// Edges to select.

	protected IEdge [] m_aoEdges;
}


//*****************************************************************************
//  Delegate: RequestSelectionEventHandler
//
/// <summary>
/// Represents a method that will handle the <see
/// cref="GraphInfoViewer.RequestSelection" /> event.
/// </summary>
///
/// <param name="sender">
/// The source of the event.
/// </param>
///
/// <param name="requestSelectionEventArgs">
///	A <see cref="RequestSelectionEventArgs" /> object that contains the event
/// data.
/// </param>
//*****************************************************************************

public delegate void
RequestSelectionEventHandler
(
	Object sender,
	RequestSelectionEventArgs requestSelectionEventArgs
);

}
