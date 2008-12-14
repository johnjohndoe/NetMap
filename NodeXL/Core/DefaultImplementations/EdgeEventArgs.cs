
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.Core
{
//*****************************************************************************
//  Class: EdgeEventArgs
//
/// <summary>
/// Provides event information for events involving an edge.
/// </summary>
//*****************************************************************************

public class EdgeEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: EdgeEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeEventArgs" /> class.
    /// </summary>
	///
    /// <param name="edge">
	/// Edge associated with the event.  Can't be null.
    /// </param>
    //*************************************************************************

    public EdgeEventArgs
	(
		IEdge edge
	)
    {
		const String MethodName = "Constructor";

        this.ArgumentChecker.CheckArgumentNotNull(MethodName, "edge", edge);

		m_oEdge = edge;

		AssertValid();
    }

    //*************************************************************************
    //  Property: Edge
    //
    /// <summary>
	/// Gets the edge associated with the event.
    /// </summary>
    ///
    /// <value>
	/// The edge associated with the event, as an <see cref="IEdge" />.  This
	/// is never null.
    /// </value>
    //*************************************************************************

    public IEdge
    Edge
    {
        get
        {
            AssertValid();

            return (m_oEdge);
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
		Debug.Assert(m_oEdge != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Edge associated with the event.

	protected IEdge m_oEdge;
}


//*****************************************************************************
//  Delegate: EdgeEventHandler
//
/// <summary>
/// Represents a method that will handle an event involving an edge.
/// </summary>
///
/// <param name="sender">
/// The source of the event.
/// </param>
///
/// <param name="edgeEventArgs">
///	An <see cref="EdgeEventArgs" /> object that contains the event data.
/// </param>
//*****************************************************************************

public delegate void
EdgeEventHandler
(
	Object sender,
	EdgeEventArgs edgeEventArgs
);

}
