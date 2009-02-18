
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.SocialNetworkLib
{
//*****************************************************************************
//  Enum: IncludedIn
//
/// <summary>
/// Specifies which email fields a participant was included in.
/// </summary>
///
/// <remarks>
/// These can be ORed together.
/// </remarks>
//*****************************************************************************

[System.FlagsAttribute]

public enum
IncludedIn
{
    /// <summary>
    /// The participant wasn't included in any email fields.
    /// </summary>

    None = 0,

    // WARNING: Do not rename the following flags.  They get plugged into
    // "System.Message.{0}Address" to form the name of a Windows Desktop Search
    // email message field.

    /// <summary>
    /// The participant was included in the From field.
    /// </summary>

    From = 1,

    /// <summary>
    /// The participant was included in the To field.
    /// </summary>

    To = 2,

    /// <summary>
    /// The participant was included in the Cc field.
    /// </summary>

    Cc = 4,

    /// <summary>
    /// The participant was included in the Bcc field.
    /// </summary>

    Bcc = 8,
}


//*****************************************************************************
//  Class: EmailParticipantCriteria
//
/// <summary>
/// Stores search critera for one participant in an email social network.
/// </summary>
///
/// <remarks>
/// An array of <see cref="EmailParticipantCriteria" /> objects gets passed to
/// EmailNetworkAnalyzer.AnalyzeEmailNetwork().
/// </remarks>
//*****************************************************************************

public class EmailParticipantCriteria : Object
{
    //*************************************************************************
    //  Constructor: EmailParticipantCriteria()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="EmailParticipantCriteria" /> class.
    /// </summary>
    //*************************************************************************

    public EmailParticipantCriteria()
    {
        m_sParticipant = null;
        m_eIncludedIn = IncludedIn.None;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Participant
    //
    /// <summary>
    /// Gets or sets the participant.
    /// </summary>
    ///
    /// <value>
    /// The String that identifies the participant.  Can be null or empty.  The
    /// default is null.
    /// </value>
    //*************************************************************************

    public String
    Participant
    {
        get
        {
            AssertValid();

            return (m_sParticipant);
        }

        set
        {
            m_sParticipant = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: IncludedIn
    //
    /// <summary>
    /// Gets or sets the email fields the participant must be included in.
    /// </summary>
    ///
    /// <value>
    /// The email fields the participant must be included in, as an ORed
    /// combination of <see cref="Microsoft.SocialNetworkLib.IncludedIn" />
    /// flags.  The default is <see
    /// cref="Microsoft.SocialNetworkLib.IncludedIn.None" />.
    /// </value>
    //*************************************************************************

    public IncludedIn
    IncludedIn
    {
        get
        {
            AssertValid();

            return (m_eIncludedIn);
        }

        set
        {
            m_eIncludedIn = value;

            AssertValid();
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
        // m_sParticipant
        // m_eIncludedIn
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The String that identifies the participant.  Can be null or empty.

    protected String m_sParticipant;

    /// The email fields the participant must be included in.

    protected IncludedIn m_eIncludedIn;
}

}
