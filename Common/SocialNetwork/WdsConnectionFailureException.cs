
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Microsoft.SocialNetworkLib
{
//*****************************************************************************
//  Class: WdsConnectionFailureException
//
/// <summary>
/// Represents an exception thrown when a connection can't be made to the
/// Windows Desktop Search index.
/// </summary>
//*****************************************************************************

[System.SerializableAttribute()]

public class WdsConnectionFailureException : Exception
{
    //*************************************************************************
    //  Constructor: WdsConnectionFailureException()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="WdsConnectionFailureException" />  class.
    /// </summary>
	///
	/// <param name="innerException">
	/// The exception that was caught while attempting to connect to the
	/// Windows Desktop Search index.
	/// </param>
    //*************************************************************************

    public WdsConnectionFailureException
	(
		Exception innerException
	)
	:
	base("The Windows Desktop Search index can't be found.", innerException)
    {
		// (Do nothing.)
    }

    //*************************************************************************
    //  Constructor: WdsConnectionFailureException()
    //
    /// <summary>
    /// Do not use this constructor.  It is for binary serialization only.
    /// </summary>
    //*************************************************************************

    protected WdsConnectionFailureException
	(
		SerializationInfo oSerializationInfo,
		StreamingContext oStreamingContext
	)
	:
	base
	(
		oSerializationInfo,
		oStreamingContext
	)
    {
		// Do not use this constructor.  It is for binary serialization only.
		//
		// This is required because the System.Exception base class implements
		// ISerializable.  All serializable classes derived from an
		// ISerializable implementor must have a constructor with this
		// signature.

		// (Do nothing.)

		AssertValid();
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
		// (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
