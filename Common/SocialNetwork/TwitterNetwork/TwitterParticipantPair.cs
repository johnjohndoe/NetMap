
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.SocialNetworkLib
{
//*****************************************************************************
//  Class: TwitterParticipantPair
//
/// <summary>
/// Represents a pair of participants in a Twitter social network.
/// </summary>
///
/// <remarks>
/// The participants are specified by their Twitter screen names.
/// </remarks>
//*****************************************************************************

public class TwitterParticipantPair : ParticipantPair
{
    //*************************************************************************
    //  Constructor: TwitterParticipantPair()
    //
    /// <overloads>
    /// Initializes a new instance of the <see cref="TwitterParticipantPair" />
    /// class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterParticipantPair" />
    /// class with specified values.
    /// </summary>
    ///
    /// <param name="participant1">
    /// The first participant, as a Twitter screen name.  Can't be null or
    /// empty.
    /// </param>
    ///
    /// <param name="participant2">
    /// The second participant, as a Twitter screen name.  Can't be null or
    /// empty.
    /// </param>
    //*************************************************************************

    public TwitterParticipantPair
    (
        String participant1,
        String participant2
    )
    : base(participant1, participant2)
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: TwitterParticipantPair()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterParticipantPair" />
    /// class with default values.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for XML serialization only.
    /// </remarks>
    //*************************************************************************

    public TwitterParticipantPair()
    :
    this("Participant1", "Participant2")
    {
        // (Do nothing else.)

        AssertValid();
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
