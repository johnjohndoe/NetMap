
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Class: VertexEventArgs
//
/// <summary>
/// Provides event information for events involving a vertex.
/// </summary>
//*****************************************************************************

public class VertexEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: VertexEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexEventArgs" /> class.
    /// </summary>
	///
    /// <param name="vertex">
	/// Vertex associated with the event.  Can't be null.
    /// </param>
    //*************************************************************************

    public VertexEventArgs
	(
		IVertex vertex
	)
    {
		const String MethodName = "Constructor";

        this.ArgumentChecker.CheckArgumentNotNull(
			MethodName, "vertex", vertex);

		m_oVertex = vertex;

		// AssertValid();
    }

    //*************************************************************************
    //  Property: Vertex
    //
    /// <summary>
	/// Gets the vertex associated with the event.
    /// </summary>
    ///
    /// <value>
	/// The vertex associated with the event, as an <see cref="IVertex" />.
	/// This is never null.
    /// </value>
    //*************************************************************************

    public IVertex
    Vertex
    {
        get
        {
            AssertValid();

            return (m_oVertex);
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

    public virtual void
    AssertValid()
    {
		Debug.Assert(m_oVertex != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Vertex associated with the event.

	protected IVertex m_oVertex;
}


//*****************************************************************************
//  Delegate: VertexEventHandler
//
/// <summary>
/// Represents a method that will handle an event involving a vertex.
/// </summary>
///
/// <param name="sender">
/// The source of the event.
/// </param>
///
/// <param name="vertexEventArgs">
///	An <see cref="VertexEventArgs" /> object that contains the event data.
/// </param>
//*****************************************************************************

public delegate void
VertexEventHandler
(
	Object sender,
	VertexEventArgs vertexEventArgs
);

}
