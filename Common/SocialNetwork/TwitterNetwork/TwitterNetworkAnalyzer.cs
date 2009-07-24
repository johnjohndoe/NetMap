
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Net;
using System.Web;
using System.IO;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.XmlLib;

namespace Microsoft.SocialNetworkLib
{
//*****************************************************************************
//  Class: TwitterNetworkAnalyzer
//
/// <summary>
/// Analyzes a user's Twitter social network.
/// </summary>
///
/// <remarks>
/// Use one of the <see cref="AnalyzeTwitterNetwork" /> overloads to
/// synchronously analyze a user's Twitter social network, or use <see
/// cref="AnalyzeTwitterNetworkAsync" /> to do it asynchronously.
/// </remarks>
//*****************************************************************************

public class TwitterNetworkAnalyzer : Object
{
    //*************************************************************************
    //  Constructor: TwitterNetworkAnalyzer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterNetworkAnalyzer" />
    /// class.
    /// </summary>
    //*************************************************************************

    public TwitterNetworkAnalyzer()
    {
        m_iHttpWebRequestTimeoutMs = 10000;
        m_oBackgroundWorker = null;

        AssertValid();
    }

    //*************************************************************************
    //  Property: HttpWebRequestTimeoutMs
    //
    /// <summary>
    /// Gets or sets the timeout to use for Twitter Web requests.
    /// </summary>
    ///
    /// <value>
    /// The timeout to use for Twitter Web requests, in milliseconds.  Must be
    /// greater than zero.
    /// </value>
    //*************************************************************************

    public Int32
    HttpWebRequestTimeoutMs
    {
        get
        {
            AssertValid();

            return (m_iHttpWebRequestTimeoutMs);
        }

        set
        {
            m_iHttpWebRequestTimeoutMs = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: IsBusy
    //
    /// <summary>
    /// Gets a flag indicating whether an asynchronous operation is in
    /// progress.
    /// </summary>
    ///
    /// <value>
    /// true if an asynchronous operation is in progress.
    /// </value>
    //*************************************************************************

    public Boolean
    IsBusy
    {
        get
        {
            return (m_oBackgroundWorker != null && m_oBackgroundWorker.IsBusy);
        }
    }

    //*************************************************************************
    //  Method: AnalyzeTwitterNetwork()
    //
    /// <summary>
    /// Synchronously analyzes a user's Twitter social network.
    /// </summary>
    ///
    /// <param name="screenNameToAnalyze">
    /// The screen name of the Twitter user whose network should be analyzed.
    /// </param>
    ///
    /// <param name="levels">
    /// Number of levels to analyze.  If 1, the user's friends are included in
    /// the analysis; if 2, the friends' friends are also included; and so on.
    /// </param>
    ///
    /// <param name="credentialsScreenName">
    /// The screen name of the Twitter user whose credentials should be used,
    /// or null to not use credentials.
    /// </param>
    ///
    /// <param name="credentialsPassword">
    /// The password name of the Twitter user whose credentials should be used.
    /// Used only if <paramref name="credentialsScreenName" /> is specified.
    /// </param>
    ///
    /// <returns>
    /// A <see cref="TwitterNetworkAnalysisResults" /> object containing the
    /// results of the analysis.
    /// </returns>
    //*************************************************************************

    public TwitterNetworkAnalysisResults
    AnalyzeTwitterNetwork
    (
        String screenNameToAnalyze,
        Int32 levels,
        String credentialsScreenName,
        String credentialsPassword
    )
    {
        AssertValid();

        return ( AnalyzeTwitterNetworkInternal(
            screenNameToAnalyze, levels, credentialsScreenName,
            credentialsPassword, null, null) );
    }

    //*************************************************************************
    //  Method: AnalyzeTwitterNetworkAsync()
    //
    /// <summary>
    /// Asynchronously analyzes a user's Twitter social network.
    /// </summary>
    ///
    /// <param name="screenNameToAnalyze">
    /// The screen name of the Twitter user whose network should be analyzed.
    /// </param>
    ///
    /// <param name="levels">
    /// Number of levels to analyze.  If 1, the user's friends are included in
    /// the analysis; if 2, the friends' friends are also included; and so on.
    /// </param>
    ///
    /// <param name="credentialsScreenName">
    /// The screen name of the Twitter user whose credentials should be used,
    /// or null to not use credentials.
    /// </param>
    ///
    /// <param name="credentialsPassword">
    /// The password name of the Twitter user whose credentials should be used.
    /// Used only if <paramref name="credentialsScreenName" /> is specified.
    /// </param>
    ///
    /// <remarks>
    /// When the analysis completes, the <see cref="AnalysisCompleted" /> event
    /// fires.  The <see cref="RunWorkerCompletedEventArgs.Result" /> property
    /// will return a <see cref="TwitterNetworkAnalysisResults" /> object.
    ///
    /// <para>
    /// To cancel the analysis, call <see cref="CancelAsync" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    AnalyzeTwitterNetworkAsync
    (
        String screenNameToAnalyze,
        Int32 levels,
        String credentialsScreenName,
        String credentialsPassword
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(screenNameToAnalyze) );
        Debug.Assert(levels >= 1);

        Debug.Assert( credentialsScreenName == null ||
            ( credentialsScreenName.Length > 0 &&
                !String.IsNullOrEmpty(credentialsPassword) ) );

        AssertValid();

        const String MethodName = "AnalyzeTwitterNetworkAsync";

        if (this.IsBusy)
        {
            throw new InvalidOperationException( String.Format(

                "{0}:{1}: An asynchronous operation is already in progress."
                ,
                this.ClassName,
                MethodName
                ) );
        }

        // Wrap the arguments in an object that can be passed to
        // BackgroundWorker.RunWorkerAsync().

        AnalyzeTwitterNetworkAsyncArgs oAnalyzeTwitterNetworkAsyncArgs =
            new AnalyzeTwitterNetworkAsyncArgs();

        oAnalyzeTwitterNetworkAsyncArgs.ScreenNameToAnalyze =
            screenNameToAnalyze;

        oAnalyzeTwitterNetworkAsyncArgs.Levels = levels;

        oAnalyzeTwitterNetworkAsyncArgs.CredentialsScreenName =
            credentialsScreenName;

        oAnalyzeTwitterNetworkAsyncArgs.CredentialsPassword =
            credentialsPassword;

        // Create a BackgroundWorker and handle its events.

        m_oBackgroundWorker = new BackgroundWorker();

        m_oBackgroundWorker.WorkerSupportsCancellation = true;

        m_oBackgroundWorker.DoWork += new DoWorkEventHandler(
            BackgroundWorker_DoWork);

        m_oBackgroundWorker.RunWorkerCompleted +=
            new RunWorkerCompletedEventHandler(
                BackgroundWorker_RunWorkerCompleted);

        m_oBackgroundWorker.RunWorkerAsync(oAnalyzeTwitterNetworkAsyncArgs);
    }

    //*************************************************************************
    //  Method: CancelAsync()
    //
    /// <summary>
    /// Cancels the analysis started by <see
    /// cref="AnalyzeTwitterNetworkAsync" />.
    /// </summary>
    ///
    /// <remarks>
    /// When the analysis cancels, the <see cref="AnalysisCompleted" /> event
    /// fires.  The <see cref="AsyncCompletedEventArgs.Cancelled" /> property
    /// will be true.
    ///
    /// <para>
    /// Important note: If the background thread started by <see
    /// cref="AnalyzeTwitterNetworkAsync" /> is running a Web request when <see
    /// cref="CancelAsync" /> is called, the analysis won't cancel until the
    /// request completes.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    CancelAsync()
    {
        AssertValid();

        if (this.IsBusy)
        {
            m_oBackgroundWorker.CancelAsync();
        }
    }

    //*************************************************************************
    //  Event: AnalysisCompleted
    //
    /// <summary>
    /// Occurs when the analysis started by <see
    /// cref="AnalyzeTwitterNetworkAsync" /> completes, is cancelled, or
    /// encounters an error.
    /// </summary>
    //*************************************************************************

    public event RunWorkerCompletedEventHandler AnalysisCompleted;


    //*************************************************************************
    //  Property: ClassName
    //
    /// <summary>
    /// Gets the full name of this class.
    /// </summary>
    ///
    /// <value>
    /// The full name of this class, suitable for use in error messages.
    /// </value>
    //*************************************************************************

    protected String
    ClassName
    {
        get
        {
            return (this.GetType().FullName);
        }
    }

    //*************************************************************************
    //  Method: AnalyzeTwitterNetworkInternal()
    //
    /// <summary>
    /// Analyzes a user's Twitter social network.
    /// </summary>
    ///
    /// <param name="sScreenNameToAnalyze">
    /// See <see cref="AnalyzeTwitterNetwork" />.
    /// </param>
    ///
    /// <param name="iLevels">
    /// See <see cref="AnalyzeTwitterNetwork" />.
    /// </param>
    ///
    /// <param name="sCredentialsScreenName">
    /// The screen name of the Twitter user whose credentials should be used,
    /// or null to not use credentials.
    /// </param>
    ///
    /// <param name="sCredentialsPassword">
    /// The password name of the Twitter user whose credentials should be used.
    /// Used only if <paramref name="credentialsScreenName" /> is specified.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// A BackgroundWorker object if this method is being called
    /// asynchronously, or null if it is being called synchronously.
    /// </param>
    ///
    /// <param name="oDoWorkEventArgs">
    /// A DoWorkEventArgs object if this method is being called
    /// asynchronously, or null if it is being called synchronously.
    /// </param>
    ///
    /// <returns>
    /// See AnalyzeTwitterNetwork().
    /// </returns>
    //*************************************************************************

    protected TwitterNetworkAnalysisResults
    AnalyzeTwitterNetworkInternal
    (
        String sScreenNameToAnalyze,
        Int32 iLevels,
        String sCredentialsScreenName,
        String sCredentialsPassword,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenNameToAnalyze) );
        Debug.Assert(iLevels >= 1);

        Debug.Assert( sCredentialsScreenName == null ||
            ( sCredentialsScreenName.Length > 0 &&
                !String.IsNullOrEmpty(sCredentialsPassword) ) );

        Debug.Assert(oBackgroundWorker == null || oDoWorkEventArgs != null);
        AssertValid();

        LinkedList<TwitterParticipantPair> oTwitterParticipantPairs =
            new LinkedList<TwitterParticipantPair>();

        Dictionary<String, TwitterParticipant> oTwitterParticipants =
            new Dictionary<String, TwitterParticipant>();

        GetFriendsRecursive(sScreenNameToAnalyze, iLevels,
            sCredentialsScreenName, sCredentialsPassword,
            oTwitterParticipantPairs, oTwitterParticipants, oBackgroundWorker,
            oDoWorkEventArgs);

        return ( new TwitterNetworkAnalysisResults(

            System.Linq.Enumerable.ToArray<TwitterParticipantPair>(
                oTwitterParticipantPairs),

            oTwitterParticipants
            ) );
    }

    //*************************************************************************
    //  Method: GetFriendsRecursive()
    //
    /// <summary>
    /// Recursively gets a user's friends and their friends.
    /// </summary>
    ///
    /// <param name="sScreenNameToAnalyze">
    /// See <see cref="AnalyzeTwitterNetwork" />.
    /// </param>
    ///
    /// <param name="iLevels">
    /// See <see cref="AnalyzeTwitterNetwork" />.
    /// </param>
    ///
    /// <param name="sCredentialsScreenName">
    /// The screen name of the Twitter user whose credentials should be used,
    /// or null to not use credentials.
    /// </param>
    ///
    /// <param name="sCredentialsPassword">
    /// The password name of the Twitter user whose credentials should be used.
    /// Used only if <paramref name="credentialsScreenName" /> is specified.
    /// </param>
    ///
    /// <param name="oTwitterParticipantPairs">
    /// Where the participant pairs get stored.
    /// </param>
    ///
    /// <param name="oTwitterParticipants">
    /// Where the participants get stored.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// A BackgroundWorker object if this method is being called
    /// asynchronously, or null if it is being called synchronously.
    /// </param>
    ///
    /// <param name="oDoWorkEventArgs">
    /// A DoWorkEventArgs object if this method is being called
    /// asynchronously, or null if it is being called synchronously.
    /// </param>
    //*************************************************************************

    protected void
    GetFriendsRecursive
    (
        String sScreenNameToAnalyze,
        Int32 iLevels,
        String sCredentialsScreenName,
        String sCredentialsPassword,
        LinkedList<TwitterParticipantPair> oTwitterParticipantPairs,
        Dictionary<String, TwitterParticipant> oTwitterParticipants,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenNameToAnalyze) );
        Debug.Assert(iLevels >= 1);

        Debug.Assert( sCredentialsScreenName == null ||
            ( sCredentialsScreenName.Length > 0 &&
                !String.IsNullOrEmpty(sCredentialsPassword) ) );

        Debug.Assert(oBackgroundWorker == null || oDoWorkEventArgs != null);
        AssertValid();

        String sUrl;
        XmlDocument oXmlDocument;
        TwitterParticipant oTwitterParticipant;

        Int32 iPage = 1;

        // Loop through the pages of friends of the sScreenNameToAnalyze user.
        // Each page contains up to MaximumFriendsPerPage friends.

        while (true)
        {
            if (oBackgroundWorker != null
                && oBackgroundWorker.CancellationPending)
            {
                // Cancel the analysis.

                Debug.Assert(oDoWorkEventArgs != null);

                oDoWorkEventArgs.Cancel = true;
                return;
            }

            sUrl = String.Format(

                "http://twitter.com/statuses/friends/{0}.xml?page={1}"
                ,
                HttpUtility.UrlEncode(sScreenNameToAnalyze),
                iPage
                );

            oXmlDocument = GetXmlDocumentFromUrl(sUrl, sCredentialsScreenName,
                sCredentialsPassword);

            // The document consists of a single "users" node with zero or more
            // "user" child nodes.

            XmlNode oUsersNode = oXmlDocument.SelectSingleNode("users");

            if (oUsersNode == null)
            {
                break;
            }

            XmlNodeList oUserNodes = oUsersNode.SelectNodes("user");

            Int32 iFriendsOnThisPage = 0;

            foreach (XmlNode oUserNode in oUserNodes)
            {
                // Convert the "user" node to a TwitterParticipant object.

                if ( TryUserNodeToTwitterParticipant(oUserNode,
                    out oTwitterParticipant) )
                {
                    String sOtherScreenName = oTwitterParticipant.ScreenName;

                    oTwitterParticipantPairs.AddLast(
                        new TwitterParticipantPair(
                            sScreenNameToAnalyze, sOtherScreenName) );

                    oTwitterParticipants[sOtherScreenName] =
                        oTwitterParticipant;

                    if (iLevels > 1)
                    {
                        // Recurse;

                        GetFriendsRecursive(sOtherScreenName, iLevels - 1,
                            sCredentialsScreenName, sCredentialsPassword,
                            oTwitterParticipantPairs, oTwitterParticipants,
                            oBackgroundWorker, oDoWorkEventArgs);
                    }
                }

                iFriendsOnThisPage++;
            }

            if (iFriendsOnThisPage < MaximumFriendsPerPage)
            {
                // There is no need to request another page of friends, which
                // would be empty.

                break;
            }

            iPage++;
        }

        if ( !oTwitterParticipants.ContainsKey(sScreenNameToAnalyze) )
        {
            // Get information about the sScreenNameToAnalyze user.

            sUrl = String.Format(

                "http://twitter.com/users/show/{0}.xml"
                ,
                HttpUtility.UrlEncode(sScreenNameToAnalyze)
                );

            oXmlDocument = GetXmlDocumentFromUrl(sUrl, sCredentialsScreenName,
                sCredentialsPassword);

            // The document consists of a single "user" node.

            XmlNode oThisUserNode = oXmlDocument.SelectSingleNode("user");

            if (
                oThisUserNode != null
                &&
                TryUserNodeToTwitterParticipant(oThisUserNode,
                    out oTwitterParticipant)
                )
            {
                oTwitterParticipants[sScreenNameToAnalyze] =
                    oTwitterParticipant;
            }
        }
    }

    //*************************************************************************
    //  Method: GetXmlDocumentFromUrl()
    //
    /// <summary>
    /// Gets an XML document from an URL.
    /// </summary>
    ///
    /// <param name="sUrl">
    /// The URL to get the document from.
    /// </param>
    ///
    /// <param name="sCredentialsScreenName">
    /// The screen name of the Twitter user whose credentials should be used,
    /// or null to not use credentials.
    /// </param>
    ///
    /// <param name="sCredentialsPassword">
    /// The password name of the Twitter user whose credentials should be used.
    /// Used only if <paramref name="credentialsScreenName" /> is specified.
    /// </param>
    ///
    /// <returns>
    /// The XmlDocument from the URL.
    /// </returns>
    //*************************************************************************

    protected XmlDocument
    GetXmlDocumentFromUrl
    (
        String sUrl,
        String sCredentialsScreenName,
        String sCredentialsPassword
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUrl) );

        Debug.Assert( sCredentialsScreenName == null ||
            ( sCredentialsScreenName.Length > 0 &&
                !String.IsNullOrEmpty(sCredentialsPassword) ) );

        AssertValid();

        HttpWebResponse oHttpWebResponse = null;
        Stream oStream = null;
        XmlDocument oXmlDocument;

        try
        {
            HttpWebRequest oHttpWebRequest =
                (HttpWebRequest)WebRequest.Create(sUrl);

            if (sCredentialsScreenName != null)
            {
                #if false

                Don't use the following standard .NET technique, because .NET
                doesn't send an Authorization header on the first request and
                relies instead on the server to ask for the header via a 401
                response.  Twitter doesn't send a 401 response (it sends a 200
                response instead), so the user is never authenticated.

                oHttpWebRequest.Credentials = new NetworkCredential(
                    sCredentialsScreenName, sCredentialsPassword);

                #endif

                // The following technique was suggested here:
                //
                // http://devproj20.blogspot.com/2008/02/
                // assigning-basic-authorization-http.html 

                String sHeaderValue = String.Format(
                    "{0}:{1}"
                    ,
                    sCredentialsScreenName,
                    sCredentialsPassword
                    );

                Byte [] abtHeaderValue = Encoding.UTF8.GetBytes(
                    sHeaderValue.ToCharArray() );

                oHttpWebRequest.Headers["Authorization"] =
                    "Basic " + Convert.ToBase64String(abtHeaderValue);
            }

            oHttpWebRequest.Timeout = m_iHttpWebRequestTimeoutMs;

            oHttpWebResponse = (HttpWebResponse)oHttpWebRequest.GetResponse();

            oStream = oHttpWebResponse.GetResponseStream();

            oXmlDocument = new XmlDocument();

            oXmlDocument.Load(oStream);
        }
        finally
        {
            if (oStream != null)
            {
                oStream.Close();
            }

            if (oHttpWebResponse != null)
            {
                oHttpWebResponse.Close();
            }
        }

        return (oXmlDocument);
    }

    //*************************************************************************
    //  Method: TryUserNodeToTwitterParticipant()
    //
    /// <summary>
    /// Attempts to convert a Twitter "user" XML node to a TwitterParticipant
    /// object.
    /// </summary>
    ///
    /// <param name="oUserNode">
    /// "user" XML node from a Twitter XML document.
    /// </param>
    ///
    /// <param name="oTwitterParticipant">
    /// Where a new TwitterParticipant object gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryUserNodeToTwitterParticipant
    (
        XmlNode oUserNode,
        out TwitterParticipant oTwitterParticipant
    )
    {
        Debug.Assert(oUserNode != null);
        AssertValid();

        oTwitterParticipant = null;

        String sScreenName, sLatestStatusTime;
        String sLatestStatus = String.Empty;
        Int32 iTwitterID, iFollowerCount;
        DateTime oLatestStatusTime = DateTime.MinValue;

        if ( !XmlUtil.GetStringNodeValue(oUserNode, "screen_name", false,
            out sScreenName) )
        {
            // Nothing can be done without a screen name.

            return (false);
        }

        if ( !XmlUtil.GetInt32NodeValue(oUserNode, "id", false,
            out iTwitterID) )
        {
            iTwitterID = Int32.MinValue;
        }

        if ( !XmlUtil.GetInt32NodeValue(oUserNode, "followers_count", false,
            out iFollowerCount) )
        {
            iFollowerCount = Int32.MinValue;
        }

        XmlNode oStatusNode = oUserNode.SelectSingleNode("status");

        if (oStatusNode != null)
        {
            XmlUtil.GetStringNodeValue(oStatusNode, "text", false,
                out sLatestStatus);

            if (
                !XmlUtil.GetStringNodeValue(oStatusNode, "created_at",
                    false, out sLatestStatusTime)
                ||
                !TryParseTwitterTime(sLatestStatusTime,
                    out oLatestStatusTime)
                )
            {
                oLatestStatusTime = DateTime.MinValue;
            }
        }

        oTwitterParticipant = new TwitterParticipant(sScreenName, iTwitterID,
            iFollowerCount, sLatestStatus, oLatestStatusTime);

        return (true);
    }

    //*************************************************************************
    //  Method: TryParseTwitterTime()
    //
    /// <summary>
    /// Attempts to convert a Twitter time from a String to a DateTime.
    /// </summary>
    ///
    /// <param name="sTwitterTime">
    /// Twitter time string.  Sample: "Mon Nov 10 02:28:41 +0000 2008".
    /// </param>
    ///
    /// <param name="oDateTime">
    /// Where the converted time gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if succesful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryParseTwitterTime
    (
        String sTwitterTime,
        out DateTime oDateTime
    )
    {
        oDateTime = DateTime.MinValue;

        // Sample: "Mon Nov 10 02:28:41 +0000 2008"

        const String Pattern =
            "... (?<Month>...)"
            + " (?<Date>\\d{2})"
            + " (?<Time>\\d{2}:\\d{2}:\\d{2})"
            + " (?<UtcOffsetSign>[+-])"
            + "(?<UtcOffsetHours>\\d{2})"
            + "(?<UtcOffsetMinutes>\\d{2})"
            + " (?<Year>\\d{4})"
            ;

        Regex oRegex = new Regex(Pattern);
        Match oMatch = oRegex.Match(sTwitterTime);

        if (!oMatch.Success)
        {
            return (false);
        }

        String sMonth = oMatch.Groups["Month"].Value;
        String sDate = oMatch.Groups["Date"].Value;
        String sTime = oMatch.Groups["Time"].Value;
        String sUtcOffsetSign = oMatch.Groups["UtcOffsetSign"].Value;

        Int32 iUtcOffsetHours =
            Int32.Parse(oMatch.Groups["UtcOffsetHours"].Value);

        Int32 iUtcOffsetMinutes =
            Int32.Parse(oMatch.Groups["UtcOffsetMinutes"].Value);

        String sYear = oMatch.Groups["Year"].Value;

        String sDateTime = String.Format(

            "{0} {1} {2} {3}"
            ,
            sMonth,
            sDate,
            sYear,
            sTime
            );

        if ( !DateTime.TryParse(sDateTime, out oDateTime) )
        {
            return (false);
        }

        Int32 iUtcOffsetMultiplier = (sUtcOffsetSign == "+") ? -1 : 1;

        oDateTime = oDateTime.AddHours(
            iUtcOffsetMultiplier * iUtcOffsetHours);

        oDateTime = oDateTime.AddMinutes(
            iUtcOffsetMultiplier * iUtcOffsetMinutes);

        oDateTime = new DateTime(oDateTime.Ticks, DateTimeKind.Utc);

        return (true);
    }

    //*************************************************************************
    //  Method: BackgroundWorker_DoWork()
    //
    /// <summary>
    /// Handles the DoWork event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected void
    BackgroundWorker_DoWork
    (
        object sender,
        DoWorkEventArgs e
    )
    {
        Debug.Assert(sender is BackgroundWorker);

        BackgroundWorker oBackgroundWorker = (BackgroundWorker)sender;

        Debug.Assert(e.Argument is AnalyzeTwitterNetworkAsyncArgs);

        AnalyzeTwitterNetworkAsyncArgs oAnalyzeTwitterNetworkAsyncArgs =
            (AnalyzeTwitterNetworkAsyncArgs)e.Argument;

        TwitterNetworkAnalysisResults oTwitterNetworkAnalysisResults =
            AnalyzeTwitterNetworkInternal(
                oAnalyzeTwitterNetworkAsyncArgs.ScreenNameToAnalyze,
                oAnalyzeTwitterNetworkAsyncArgs.Levels,
                oAnalyzeTwitterNetworkAsyncArgs.CredentialsScreenName,
                oAnalyzeTwitterNetworkAsyncArgs.CredentialsPassword,
                oBackgroundWorker,
                e
                );

        e.Result = oTwitterNetworkAnalysisResults;
    }

    //*************************************************************************
    //  Method: BackgroundWorker_RunWorkerCompleted()
    //
    /// <summary>
    /// Handles the RunWorkerCompleted event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected void
    BackgroundWorker_RunWorkerCompleted
    (
        object sender,
        RunWorkerCompletedEventArgs e
    )
    {
        AssertValid();

        // Forward the event.

        RunWorkerCompletedEventHandler oAnalysisCompleted =
            this.AnalysisCompleted;

        if (oAnalysisCompleted != null)
        {
            oAnalysisCompleted(this, e);
        }

        m_oBackgroundWorker = null;
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
        Debug.Assert(m_iHttpWebRequestTimeoutMs > 0);
        // m_oBackgroundWorker
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Maximum number of friends returned by Twitter in a single page.

    protected const Int32 MaximumFriendsPerPage = 100;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The timeout to use for Twitter Web requests, in milliseconds.

    protected Int32 m_iHttpWebRequestTimeoutMs;

    /// Used for asynchronous analysis.  null if an asynchronous analysis is
    /// not in progress.

    protected BackgroundWorker m_oBackgroundWorker;


    //*************************************************************************
    //  Embedded class: AnalyzeTwitterNetworkAsyncArguments()
    //
    /// <summary>
    /// Contains the arguments needed to asynchronously analyze a user's
    /// Twitter social network.
    /// </summary>
    //*************************************************************************

    protected class AnalyzeTwitterNetworkAsyncArgs
    {
        ///
        public String ScreenNameToAnalyze;
        ///
        public Int32 Levels;
        ///
        public String CredentialsScreenName;
        ///
        public String CredentialsPassword;
    };
}

}
