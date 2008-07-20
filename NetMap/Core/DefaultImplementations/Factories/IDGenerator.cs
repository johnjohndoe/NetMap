
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NetMap.Core
{
//*****************************************************************************
//  Class: IDGenerator
//
/// <summary>
/// Generates a sequence of unique IDs.
/// </summary>
///
/// <remarks>
/// Call <see cref="GetNextID" /> to get a unique integer ID.  By default, <see
/// cref="GetNextID" /> generates a simple integer sequence that starts at 1.
///
/// <para>
/// This class can be used to generate unique IDs for graphs, vertices, and
/// edges.  The class responsible for creating objects of one of these types
/// should define a static field of type <see cref="IDGenerator" />, then call
/// <see cref="GetNextID" /> every time an object of that type is created.
/// This results in independent sequences for each type.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class IDGenerator : NetMapBase
{
    //*************************************************************************
    //  Constructor: IDGenerator()
    //
    /// <overloads>
    /// Initializes a new instance of the <see cref="IDGenerator" /> class.
    /// </overloads>
	///
    /// <summary>
    /// Initializes a new instance of the <see cref="IDGenerator" /> class with
	/// a first ID of 1.
    /// </summary>
    //*************************************************************************

    public IDGenerator()
	:
	this(1)
    {
		// (Do nothing else.)

		AssertValid();
    }

    //*************************************************************************
    //  Constructor: IDGenerator()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="IDGenerator" /> class with
	/// a specified first ID.
    /// </summary>
	///
	/// <param name="firstID">
	/// First ID to return from <see cref="GetNextID" />.  Can't be <see
	/// cref="Int32.MaxValue" />.
	/// </param>
    //*************************************************************************

    public IDGenerator
	(
		Int32 firstID
	)
    {
		const String MethodName = "Constructor";

		if (firstID == Int32.MaxValue)
		{
			this.ArgumentChecker.ThrowArgumentException(MethodName, "firstID",
			
				String.Format(

					"The first ID can't be Int32.MaxValue ({0})."
					,
					Int32.MaxValue
				) );
		}

		m_iNextID = firstID;

		AssertValid();
    }

    //*************************************************************************
    //  Method: GetNextID()
    //
    /// <summary>
	/// Returns the ID to use for the next created object.
    /// </summary>
    ///
    /// <returns>
	/// The ID to use for the next created object.
    /// </returns>
    //*************************************************************************

	public Int32
	GetNextID()
	{
		if (m_iNextID == Int32.MaxValue)
		{
			throw new ApplicationException( String.Format(

				"{0}.GetNextID: The maximum ID ({1}) has been reached."
				,
				this.ClassName,
				Int32.MaxValue
				) );
		}

		Int32 iNextID = m_iNextID;

		m_iNextID++;

		return (iNextID);
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

		Debug.Assert(m_iNextID != Int32.MaxValue);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// ID to use for the next created object.

	protected Int32 m_iNextID;
}

}
