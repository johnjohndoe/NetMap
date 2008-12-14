
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Visualization
{
//*****************************************************************************
//  Class: DrawerBase
//
/// <summary>
/// Base class for classes that perform drawing operations.
/// </summary>
///
/// <remarks>
///	This class implements the <see cref="IDisposable" /> interface.  The
/// derived class should override <see cref="DisposeManagedObjects" /> if it
/// maintains managed objects that should be disposed.
/// </remarks>
//*****************************************************************************

public class DrawerBase : VisualizationBase, IDisposable
{
    //*************************************************************************
    //  Constructor: DrawerBase()
    //
    /// <summary>
    /// Initializes a new instance of the DrawerBase class.
    /// </summary>
    //*************************************************************************

    public DrawerBase()
    {
		m_bDisposed = false;

		// AssertValid();
    }

	//*************************************************************************
	//	Destructor: ~DrawerBase()
	//
	/// <summary>
	/// DrawerBase destructor.
	/// </summary>
	//*************************************************************************

	~DrawerBase()      
	{
		Dispose(false);
	}

	//*************************************************************************
	//	Method: Dispose()
	//
	/// <summary>
	/// Dispose method.
	/// </summary>
	///
	/// <remarks>
	///	Performs application-defined tasks associated with freeing, releasing,
	/// or resetting unmanaged resources.
	/// </remarks>
	//*************************************************************************

	public void
	Dispose()
	{
		Dispose(true);

		GC.SuppressFinalize(this);
	}

	//*************************************************************************
	//	Event: RedrawRequired
	//
	/// <summary>
	///	Occurs when a change occurs that requires a graph redraw.
	/// </summary>
	///
	/// <remarks>
	///	The event is fired when a change is made to the object that might
	/// affect the appearance of the graph.  The object owner should redraw the
	/// graph but does not have to lay out the graph again.
	/// </remarks>
	//*************************************************************************

	public event EventHandler RedrawRequired;


	//*************************************************************************
	//	Event: LayoutRequired
	//
	/// <summary>
	///	Occurs when a change occurs that requires the graph to be laid out
	/// again.
	/// </summary>
	///
	/// <remarks>
	///	The event is fired when any change is made to the object that might
	/// affect the layout of the graph.  The owner should lay out the graph and
	/// redraw it in response to the event.
	/// </remarks>
	//*************************************************************************

	public event EventHandler LayoutRequired;


    //*************************************************************************
    //  Method: TryGetPerAlpha()
    //
    /// <summary>
    /// Attempts to get the alpha value to use for a vertex or edge.
    /// </summary>
    ///
    /// <param name="vertexOrEdge">
    /// The vertex or edge to get the alpha value for.
    /// </param>
    ///
    /// <param name="perAlpha">
    /// Where the alpha value gets stored if true is returned.  Alpha values
	/// range from 0 (transparent) to 255 (opaque).
    /// </param>
    ///
	/// <returns>
	/// true if an alpha value was obtained from the vertex or edge metadata.
	/// If false, the caller should use a default alpha value.
	/// </returns>
    //*************************************************************************

    protected Boolean
    TryGetPerAlpha
    (
        IMetadataProvider vertexOrEdge,
		out Int32 perAlpha
    )
	{
		Debug.Assert(vertexOrEdge != null);
		AssertValid();

		perAlpha = Int32.MinValue;

		if (GetVisibility(vertexOrEdge) == VisibilityKeyValue.Filtered)
		{
			// The vertex or edge is filtered.  Use a low alpha value to
			// indicate this.

            perAlpha = FilteredAlpha;

			return (true);
		}

		Object oPerAlphaAsObject;

		if ( vertexOrEdge.TryGetValue(ReservedMetadataKeys.PerAlpha,
			typeof(Int32), out oPerAlphaAsObject) )
		{
			perAlpha = (Int32)oPerAlphaAsObject;

			if (perAlpha < 0 || perAlpha > 255)
			{
				Debug.Assert(vertexOrEdge is IIdentityProvider);

				throw new FormatException( String.Format(

					"{0}: The {1} with the ID {2} has an out-of-range"
					+ " {3} value.  Valid values are between 0 and 255."
					,
					this.ClassName,
					(vertexOrEdge is IVertex) ? "vertex" : "edge",
					( (IIdentityProvider)vertexOrEdge ).ID,
					ReservedMetadataKeys.PerAlpha
					) );
			}

			return (true);
		}

		return (false);
	}

    //*************************************************************************
    //  Method: GetVisibility()
    //
    /// <summary>
    /// Gets the visibility of a vertex or edge.
    /// </summary>
    ///
    /// <param name="vertexOrEdge">
    /// The vertex or edge.
    /// </param>
    ///
    /// <returns>
	/// If the <see cref="ReservedMetadataKeys.Visibility" /> key is present on
	/// <paramref name="vertexOrEdge" />, the key's value is returned as a <see
	/// cref="VisibilityKeyValue" />.  Otherwise, <see
	/// cref="VisibilityKeyValue.Visible" /> is returned.
    /// </returns>
    //*************************************************************************

    protected VisibilityKeyValue
    GetVisibility
    (
        IMetadataProvider vertexOrEdge
    )
	{
		Debug.Assert(vertexOrEdge != null);
		AssertValid();

		Object oVisibilityKeyValue;

		if ( vertexOrEdge.TryGetValue(ReservedMetadataKeys.Visibility,
			typeof(VisibilityKeyValue), out oVisibilityKeyValue) )
		{
			return ( (VisibilityKeyValue)oVisibilityKeyValue );
		}

		return (VisibilityKeyValue.Visible);
	}


	//*************************************************************************
	//	Method: FireRedrawRequired()
	//
	/// <summary>
	///	Fires the <see cref="RedrawRequired" /> event if appropriate.
	/// </summary>
	//*************************************************************************

	protected void
	FireRedrawRequired()
	{
		AssertValid();

		EventUtil.FireEvent(this, this.RedrawRequired);
	}

	//*************************************************************************
	//	Method: FireLayoutRequired()
	//
	/// <summary>
	///	Fires the <see cref="LayoutRequired" /> event if appropriate.
	/// </summary>
	//*************************************************************************

	protected void
	FireLayoutRequired()
	{
		AssertValid();

		EventUtil.FireEvent(this, this.LayoutRequired);
	}

	//*************************************************************************
	//	Method: Dispose()
	//
	/// <summary>
	/// Dispose method.
	/// </summary>
	///
	/// <remarks>
	///	Performs application-defined tasks associated with freeing, releasing,
	/// or resetting unmanaged resources.
	/// </remarks>
	///
	/// <param name="bDisposing">
	///	See IDisposable.
	/// </param>
	//*************************************************************************

	protected void
	Dispose
	(
		Boolean bDisposing
	)
	{
		if (!m_bDisposed)
		{
			if (bDisposing)
			{
				DisposeManagedObjects();
			}
		}

		m_bDisposed = true;         
	}

	//*************************************************************************
	//	Method: DisposeManagedObjects()
	//
	/// <summary>
	/// Disposes of managed objects used for drawing.
	/// </summary>
	///
	/// <remarks>
	/// The derived class should override this method if it maintains managed
	/// objects used for drawing.
	/// </remarks>
	//*************************************************************************

	protected virtual void
	DisposeManagedObjects()
	{
		// (Do nothing.)
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

		// m_bDisposed
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Alpha value to use for vertices and edges that are filtered.

	protected const Int32 FilteredAlpha = 10;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Used to implement IDispose.

	protected Boolean m_bDisposed;
}

}
