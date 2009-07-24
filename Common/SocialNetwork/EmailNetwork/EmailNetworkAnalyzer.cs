
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Data.OleDb;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.SocialNetworkLib
{
//*****************************************************************************
//  Enum: AttachmentFilter
//
/// <summary>
/// Specifies how attachments are filtered on by <see
/// cref="EmailNetworkAnalyzer" />.
/// </summary>
//*****************************************************************************

public enum
AttachmentFilter
{
    /// <summary>
    /// Include only emails that have an attachment.
    /// </summary>

    HasAttachment,

    /// <summary>
    /// Include only emails that do not have an attachment.
    /// </summary>

    NoAttachment,

    /// <summary>
    /// Include only emails that have an attachment and are from participant 1.
    /// </summary>

    HasAttachmentFromParticipant1,
}


//*****************************************************************************
//  Class: EmailNetworkAnalyzer
//
/// <summary>
/// Analyzes a user's email social network.
/// </summary>
///
/// <remarks>
/// Use one of the <see cref="AnalyzeEmailNetwork()" /> overloads to
/// synchronously analyze a user's email social network, or use <see
/// cref="AnalyzeEmailNetworkAsync" /> to do it asynchronously.
/// </remarks>
//*****************************************************************************

public class EmailNetworkAnalyzer : Object
{
    //*************************************************************************
    //  Constructor: EmailNetworkAnalyzer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailNetworkAnalyzer" />
    /// class.
    /// </summary>
    //*************************************************************************

    public EmailNetworkAnalyzer()
    {
        m_oBackgroundWorker = null;

        AssertValid();
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
    //  Method: AnalyzeEmailNetwork()
    //
    /// <overloads>
    /// Synchronously analyzes a user's email social network.
    /// </overloads>
    ///
    /// <summary>
    /// Synchronously analyzes a user's entire email social network.
    /// </summary>
    ///
    /// <returns>
    /// An array of zero or more <see cref="EmailParticipantPair" /> objects,
    /// one for each pair of participants in the user's email social network.
    /// The return value is never null.
    /// </returns>
    ///
    /// <remarks>
    /// This method uses the Windows Desktop Search index to analyze the user's
    /// email.  For each email item in the index, an edge weight of 1 is
    /// assigned to each From-To and From-Cc participant pair.  For example, if
    /// an email item has these fields:
    ///
    /// <code>
    /// From: John
    /// To: Mary, Bob
    /// Cc: Sarah
    /// </code>
    ///
    /// <para>
    /// then the following participant pairs are created:
    /// </para>
    ///
    /// <code>
    /// John, Mary, 1
    /// John, Bob, 1
    /// John, Sarah, 1
    /// </code>
    ///
    /// <para>
    /// The process is repeated for every email item.  If a second email item
    /// has these fields, for example:
    /// </para>
    ///
    /// <code>
    /// From: John
    /// To: Sarah
    /// </code>
    ///
    /// <para>
    /// then the following participant pair is created:
    /// </para>
    ///
    /// <code>
    /// John, Sarah, 1
    /// </code>
    ///
    /// <para>
    /// The process is repeated for every email item and the results are
    /// aggregated.  For the two emails in this example, the aggregated results
    /// look like this:
    /// </para>
    ///
    /// <code>
    /// John, Mary, 1
    /// John, Bob, 1
    /// John, Sarah, 2
    /// </code>
    ///
    /// <para>
    /// The aggregated set of <see cref="EmailParticipantPair" /> objects is
    /// returned.
    /// </para>
    ///
    /// <para>
    /// If a connection to the Windows Desktop Search index can't be made, a
    /// <see cref="WdsConnectionFailureException" /> is thrown.  If a
    /// connection is made but the query fails, an <see
    /// cref="OleDbException" /> is thrown.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public EmailParticipantPair []
    AnalyzeEmailNetwork()
    {
        AssertValid();

        return ( AnalyzeEmailNetwork(null, null, null, null, null, null, null,
            null, null, null, null, true, false) );
    }

    //*************************************************************************
    //  Method: AnalyzeEmailNetwork()
    //
    /// <summary>
    /// Synchronously analyzes the part of a user's email social network that
    /// satisfies specified criteria.
    /// </summary>
    ///
    /// <param name="participantsCriteria">
    /// An array of <see cref="EmailParticipantCriteria" /> objects, one for
    /// each participant to filter on, or null to not filter on participants.
    /// For each element in the array, <see
    /// cref="EmailParticipantCriteria.Participant" /> must be specified, but
    /// <see cref="EmailParticipantCriteria.IncludedIn" /> can be <see
    /// cref="IncludedIn.None" />.
    /// </param>
    ///
    /// <param name="startTime">
    /// If specified, only emails sent on or after the specified time are
    /// included in the aggregated results.  Use null to specify "no start
    /// time."
    /// </param>
    ///
    /// <param name="endTime">
    /// If specified, only emails sent on or before the specified time are
    /// included in the aggregated results.  Use null to specify "no end time."
    /// </param>
    ///
    /// <param name="bodyText">
    /// Body text to filter on, or null to not filter on body text.  Can't be
    /// an empty string.  If specified, only emails that include the specified
    /// body text are included in the aggregated results.
    /// </param>
    ///
    /// <param name="folder">
    /// Email folder to filter on, or null to not filter on the email folder.
    /// Can't be an empty string.  If specified, only emails in the specified
    /// folder are included in the aggregated results.  Sample: "Inbox".
    /// </param>
    ///
    /// <param name="minimumSize">
    /// If specified, only emails that have a size greater than or equal to the
    /// specified value are included in the aggregated results.
    /// </param>
    ///
    /// <param name="maximumSize">
    /// If specified, only emails that have a size less than or equal to the
    /// specified value are included in the aggregated results.
    /// </param>
    ///
    /// <param name="attachmentFilter">
    /// If not null, specifies how attachments are used to determine which
    /// emails are included in the aggregated results.  Use null to specify
    /// "don't care."
    /// </param>
    ///
    /// <param name="hasCc">
    /// If specified, only emails that have or don't have a Cc line are
    /// included in the aggregated results.  Use null to specify "don't care."
    /// </param>
    ///
    /// <param name="hasBcc">
    /// If specified, only emails that have or don't have a Bcc line are
    /// included in the aggregated results.  Use null to specify "don't care."
    /// </param>
    ///
    /// <param name="isReplyFromParticipant1">
    /// If specified, only emails that are or are not replies from <paramref
    /// name="participant1" /> are included in the aggregated results.
    /// Use null to specify "don't care."
    ///
    /// <para>
    /// [IMPORTANT NOTE: As of April 2008, the System.Message.IsFwdOrReply
    /// message property needed to implement this is always null.  Specifying
    /// true or false for this parameter will always return an empty array.
    /// The parameter is retained in case the missing property is fixed in a
    /// future version of Windows Desktop Search.]
    /// </para>
    ///
    /// </param>
    ///
    /// <param name="useCcForEdgeWeights">
    /// If true, an edge weight of one is assigned to the sender and each
    /// participant on the Cc line.  (An edge weight of one is always assigned
    /// to the sender and each participant on the To line.)
    /// </param>
    ///
    /// <param name="useBccForEdgeWeights">
    /// If true, an edge weight of one is assigned to the sender and each
    /// participant on the Bcc line.  (An edge weight of one is always assigned
    /// to the sender and each participant on the To line.)
    /// </param>
    ///
    /// <returns>
    /// An array of zero or more <see cref="EmailParticipantPair" /> objects,
    /// one for each pair of participants in the user's email social network
    /// that satisfy the specified criteria.  The return value is never null.
    /// </returns>
    ///
    /// <remarks>
    /// This overload does the same thing as the <see
    /// cref="AnalyzeEmailNetwork()" /> overload, but only those emails that
    /// match the specified criteria are aggregated.
    /// </remarks>
    //*************************************************************************

    public EmailParticipantPair []
    AnalyzeEmailNetwork
    (
        EmailParticipantCriteria [] participantsCriteria,
        Nullable<DateTime> startTime,
        Nullable<DateTime> endTime,
        String bodyText,
        String folder,
        Nullable<Int64> minimumSize,
        Nullable<Int64> maximumSize,
        Nullable<AttachmentFilter> attachmentFilter,
        Nullable<Boolean> hasCc,
        Nullable<Boolean> hasBcc,
        Nullable<Boolean> isReplyFromParticipant1,
        Boolean useCcForEdgeWeights,
        Boolean useBccForEdgeWeights
    )
    {
        AssertValid();

        return ( AnalyzeEmailNetworkInternal(participantsCriteria, startTime,
            endTime, bodyText, folder, minimumSize, maximumSize,
            attachmentFilter, hasCc, hasBcc, isReplyFromParticipant1,
            useCcForEdgeWeights, useBccForEdgeWeights, null, null) );
    }

    //*************************************************************************
    //  Method: AnalyzeEmailNetworkAsync()
    //
    /// <summary>
    /// Asynchronously analyzes the part of a user's email social network that
    /// satisfies specified criteria.
    /// </summary>
    ///
    /// <param name="participantsCriteria">
    /// See the synchronous method.
    /// </param>
    ///
    /// <param name="startTime">
    /// See the synchronous method.
    /// </param>
    ///
    /// <param name="endTime">
    /// See the synchronous method.
    /// </param>
    ///
    /// <param name="bodyText">
    /// See the synchronous method.
    /// </param>
    ///
    /// <param name="folder">
    /// See the synchronous method.
    /// </param>
    ///
    /// <param name="minimumSize">
    /// See the synchronous method.
    /// </param>
    ///
    /// <param name="maximumSize">
    /// See the synchronous method.
    /// </param>
    ///
    /// <param name="attachmentFilter">
    /// See the synchronous method.
    /// </param>
    ///
    /// <param name="hasCc">
    /// See the synchronous method.
    /// </param>
    ///
    /// <param name="hasBcc">
    /// See the synchronous method.
    /// </param>
    ///
    /// <param name="isReplyFromParticipant1">
    /// See the synchronous method.
    /// </param>
    ///
    /// <param name="useCcForEdgeWeights">
    /// See the synchronous method.
    /// </param>
    ///
    /// <param name="useBccForEdgeWeights">
    /// See the synchronous method.
    /// </param>
    ///
    /// <remarks>
    /// When the analysis completes, the <see cref="AnalysisCompleted" /> event
    /// fires.  The <see cref="RunWorkerCompletedEventArgs.Result" /> property
    /// will return an array of zero or more <see cref="EmailParticipantPair" />
    /// objects, one for each pair of participants in the user's email social
    /// network that satisfy the specified criteria.  The property is never
    /// null.
    ///
    /// <para>
    /// To cancel the analysis, call <see cref="CancelAsync" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    AnalyzeEmailNetworkAsync
    (
        EmailParticipantCriteria [] participantsCriteria,
        Nullable<DateTime> startTime,
        Nullable<DateTime> endTime,
        String bodyText,
        String folder,
        Nullable<Int64> minimumSize,
        Nullable<Int64> maximumSize,
        Nullable<AttachmentFilter> attachmentFilter,
        Nullable<Boolean> hasCc,
        Nullable<Boolean> hasBcc,
        Nullable<Boolean> isReplyFromParticipant1,
        Boolean useCcForEdgeWeights,
        Boolean useBccForEdgeWeights
    )
    {
        AssertValid();

        const String MethodName = "AnalyzeEmailNetworkAsync";

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

        AnalyzeEmailNetworkAsyncArgs oAnalyzeEmailNetworkAsyncArgs =
            new AnalyzeEmailNetworkAsyncArgs();

        oAnalyzeEmailNetworkAsyncArgs.ParticipantsCriteria =
            participantsCriteria;

        oAnalyzeEmailNetworkAsyncArgs.StartTime = startTime;
        oAnalyzeEmailNetworkAsyncArgs.EndTime = endTime;
        oAnalyzeEmailNetworkAsyncArgs.BodyText = bodyText;
        oAnalyzeEmailNetworkAsyncArgs.Folder = folder;
        oAnalyzeEmailNetworkAsyncArgs.MinimumSize = minimumSize;
        oAnalyzeEmailNetworkAsyncArgs.MaximumSize = maximumSize;
        oAnalyzeEmailNetworkAsyncArgs.AttachmentFilter = attachmentFilter;
        oAnalyzeEmailNetworkAsyncArgs.HasCc = hasCc;
        oAnalyzeEmailNetworkAsyncArgs.HasBcc = hasBcc;

        oAnalyzeEmailNetworkAsyncArgs.IsReplyFromParticipant1 =
            isReplyFromParticipant1;

        oAnalyzeEmailNetworkAsyncArgs.UseCcForEdgeWeights =
            useCcForEdgeWeights;

        oAnalyzeEmailNetworkAsyncArgs.UseBccForEdgeWeights =
            useBccForEdgeWeights;

        // Create a BackgroundWorker and handle its events.

        m_oBackgroundWorker = new BackgroundWorker();

        m_oBackgroundWorker.WorkerSupportsCancellation = true;

        m_oBackgroundWorker.DoWork += new DoWorkEventHandler(
            BackgroundWorker_DoWork);

        m_oBackgroundWorker.RunWorkerCompleted +=
            new RunWorkerCompletedEventHandler(
                BackgroundWorker_RunWorkerCompleted);

        m_oBackgroundWorker.RunWorkerAsync(oAnalyzeEmailNetworkAsyncArgs);
    }

    //*************************************************************************
    //  Method: CancelAsync()
    //
    /// <summary>
    /// Cancels the analysis started by <see
    /// cref="AnalyzeEmailNetworkAsync" />.
    /// </summary>
    ///
    /// <remarks>
    /// When the analysis cancels, the <see cref="AnalysisCompleted" /> event
    /// fires.  The <see cref="AsyncCompletedEventArgs.Cancelled" /> property
    /// will be true.
    ///
    /// <para>
    /// Important note: If the background thread started by <see
    /// cref="AnalyzeEmailNetworkAsync" /> is running a Windows Desktop Search
    /// query when <see cref="CancelAsync" /> is called, the analysis won't
    /// cancel until the query completes.  If the background thread is
    /// processing the recordset returned by the query, the analysis will
    /// cancel quickly.
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
    /// cref="AnalyzeEmailNetworkAsync" /> completes, is cancelled, or
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
    //  Property: ArgumentChecker
    //
    /// <summary>
    /// Gets a new initialized <see cref="ArgumentChecker" /> object.
    /// </summary>
    ///
    /// <value>
    /// A new initialized <see cref="ArgumentChecker" /> object.
    /// </value>
    ///
    /// <remarks>
    /// The returned object can be used to check the validity of property
    /// values and method parameters.
    /// </remarks>
    //*************************************************************************

    internal ArgumentChecker
    ArgumentChecker
    {
        get
        {
            return ( new ArgumentChecker(this.ClassName) );
        }
    }

    //*************************************************************************
    //  Method: GetDataReader()
    //
    /// <summary>
    /// Gets a data reader for reading specified email items.
    /// </summary>
    ///
    /// <param name="sQuery">
    /// A Windows Desktop Search query specifying the email items to get a data
    /// reader for.
    /// </param>
    ///
    /// <returns>
    /// An <see cref="OleDbDataReader" /> object for reading the specified
    /// email items.
    /// </returns>
    ///
    /// <remarks>
    /// If a connection to the Windows Desktop Search index can't be made, a
    /// <see cref="WdsConnectionFailureException" /> is thrown.  If a
    /// connection is made but the query fails, an <see
    /// cref="OleDbException" /> is thrown.
    /// </remarks>
    //*************************************************************************

    protected OleDbDataReader
    GetDataReader
    (
        String sQuery
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sQuery) );
        AssertValid();

        OleDbConnection oConnection = null;

        try
        {
            oConnection = new OleDbConnection(WdsConnectionString);
            oConnection.Open();
        }
        catch (Exception oException)
        {
            throw new WdsConnectionFailureException(oException);
        }

        OleDbDataReader oDataReader = null;

        try
        {
            OleDbCommand oCommand = new OleDbCommand(sQuery, oConnection);
            oCommand.CommandTimeout = OleDbCommandTimeoutSeconds;

            oDataReader = oCommand.ExecuteReader();
        }
        catch
        {
            oConnection.Close();

            throw;
        }

        return (oDataReader);
    }

    //*************************************************************************
    //  Method: AnalyzeEmailNetworkInternal()
    //
    /// <summary>
    /// Analyzes the part of a user's email social network that satisfies
    /// specified criteria.
    /// </summary>
    ///
    /// <param name="participantsCriteria">
    /// See the synchronous method.
    /// </param>
    ///
    /// <param name="startTime">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="endTime">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="bodyText">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="folder">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="minimumSize">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="maximumSize">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="attachmentFilter">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="hasCc">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="hasBcc">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="isReplyFromParticipant1">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="useCcForEdgeWeights">
    /// See the synchronous method.
    /// </param>
    ///
    /// <param name="useBccForEdgeWeights">
    /// See the synchronous method.
    /// </param>
    ///
    /// <param name="backgroundWorker">
    /// A BackgroundWorker object if this method is being called
    /// asynchronously, or null if it is being called synchronously.
    /// </param>
    ///
    /// <param name="doWorkEventArgs">
    /// A DoWorkEventArgs object if this method is being called
    /// asynchronously, or null if it is being called synchronously.
    /// </param>
    ///
    /// <returns>
    /// See AnalyzeEmailNetwork().
    /// </returns>
    //*************************************************************************

    protected EmailParticipantPair []
    AnalyzeEmailNetworkInternal
    (
        EmailParticipantCriteria [] participantsCriteria,
        Nullable<DateTime> startTime,
        Nullable<DateTime> endTime,
        String bodyText,
        String folder,
        Nullable<Int64> minimumSize,
        Nullable<Int64> maximumSize,
        Nullable<AttachmentFilter> attachmentFilter,
        Nullable<Boolean> hasCc,
        Nullable<Boolean> hasBcc,
        Nullable<Boolean> isReplyFromParticipant1,
        Boolean useCcForEdgeWeights,
        Boolean useBccForEdgeWeights,
        BackgroundWorker backgroundWorker,
        DoWorkEventArgs doWorkEventArgs
    )
    {
        CheckAnalyzeMethodArguments(participantsCriteria, startTime, endTime,
            bodyText, folder, minimumSize, maximumSize, attachmentFilter,
            isReplyFromParticipant1);

        Debug.Assert(backgroundWorker == null || doWorkEventArgs != null);

        AssertValid();

        String sQuery = CreateQuery(participantsCriteria, startTime, endTime,
            bodyText, folder, minimumSize, maximumSize, attachmentFilter,
            hasCc, hasBcc, isReplyFromParticipant1);

        OleDbDataReader oDataReader = GetDataReader(sQuery);

        EmailParticipantPair [] aoEmailParticipantPairs = null;

        try
        {
            aoEmailParticipantPairs = AnalyzeEmailNetworkInternal(
                oDataReader, useCcForEdgeWeights, useBccForEdgeWeights,
                backgroundWorker, doWorkEventArgs);
        }
        finally
        {
            oDataReader.Close();
        }

        String participant1 = null;

        if (participantsCriteria != null && participantsCriteria.Length > 0)
        {
            participant1 = participantsCriteria[0].Participant;
        }

        if ( !String.IsNullOrEmpty(participant1) )
        {
            // The first participant should be in the
            // EmailParticipantPair.Participant1 slot for each element in the
            // returned array that includes the first participant.

            String sParticipant1Lower = participant1.ToLower();

            foreach (EmailParticipantPair oEmailParticipantPair in
                aoEmailParticipantPairs)
            {
                if (oEmailParticipantPair.Participant2 == sParticipant1Lower)
                {
                    // Swap the participants.

                    String sTemp = oEmailParticipantPair.Participant1;

                    oEmailParticipantPair.Participant1 =
                        oEmailParticipantPair.Participant2;

                    oEmailParticipantPair.Participant2 = sTemp;
                }
            }
        }

        return (aoEmailParticipantPairs);
    }

    //*************************************************************************
    //  Method: AnalyzeEmailNetworkInternal()
    //
    /// <summary>
    /// Analyzes a user's email social network.
    /// </summary>
    ///
    /// <param name="oDataReader">
    /// An <see cref="OleDbDataReader" /> object for reading email items.
    /// </param>
    ///
    /// <param name="bUseCcForEdgeWeights">
    /// See the synchronous AnalyzeEmailNetwork() method.
    /// </param>
    ///
    /// <param name="bUseBccForEdgeWeights">
    /// See the synchronous AnalyzeEmailNetwork() method.
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
    /// An array of zero or more <see cref="EmailParticipantPair" /> objects,
    /// one for each pair of participants in the set of email items.  The
    /// return value is never null.
    /// </returns>
    //*************************************************************************

    protected EmailParticipantPair []
    AnalyzeEmailNetworkInternal
    (
        OleDbDataReader oDataReader,
        Boolean bUseCcForEdgeWeights,
        Boolean bUseBccForEdgeWeights,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert(oDataReader != null);
        Debug.Assert(oBackgroundWorker == null || oDoWorkEventArgs != null);
        AssertValid();

        // Create a dictionary to keep track of participant pairs.  The key is
        // the pair of participants in the format used by ParticipantsToKey()
        // and the value is the edge weight between the participants.  The
        // contents of this aggregated dictionary get returned by this method.

        Dictionary<String, Int32> oAggregatedDictionary =
            new Dictionary<String, Int32>();

        // Loop through the email items.

        Int64 iRecords = 0;

        while ( oDataReader.Read() )
        {
            try
            {
                Object oFromField = oDataReader["System.Message.FromAddress"];
                Object oToField = oDataReader["System.Message.ToAddress"];
                Object oCcField = oDataReader["System.Message.CcAddress"];
                Object oBccField = oDataReader["System.Message.BccAddress"];

                AnalyzeOneEmail(oFromField, oToField, oCcField, oBccField,
                    bUseCcForEdgeWeights, bUseBccForEdgeWeights,
                    oAggregatedDictionary);
            }
            catch (OleDbException)
            {
                // In June 2009, a NodeXL team member got a repeatable
                // OleDbException while attempting to analyze his email for a
                // certain date range.  Unfortunately, the team member was on
                // the other side of the country and the exception had no inner
                // exceptions to provide more details.  (OleDbException uses a
                // custom Errors collection instead of inner exceptions, and
                // that collection doesn't get displayed by NodeXL's generic
                // error-handling scheme).  Therefore, the exact cause of the
                // error couldn't be determined.
                //
                // However, the error went away when the date range was
                // altered, leading me to believe that an email with an invalid
                // field was encountered.  Although the DataReader.Item[] and
                // AnalyzeOneEmail() calls shouldn't throw an exception on a
                // bad field, there may be some invalid field condition I'm
                // overlooking.
                //
                // Workaround: Skip emails that throw such an exception.
            }

            if (oBackgroundWorker != null)
            {
                // Check whether a cancellation has been requested.

                if (iRecords % AsyncCancelInterval == 0 &&
                    oBackgroundWorker.CancellationPending)
                {
                    // Cancel the analysis.

                    Debug.Assert(oDoWorkEventArgs != null);

                    oDoWorkEventArgs.Cancel = true;
                    return ( new EmailParticipantPair[0] );
                }
            }

            iRecords++;
        }

        // Convert the aggregated results to an array of EmailParticipantPair
        // objects.

        EmailParticipantPair [] aoEmailParticipantPairs =
            new EmailParticipantPair[oAggregatedDictionary.Count];

        Int32 i = 0;

        foreach (KeyValuePair<String, Int32> oKeyValuePair in
            oAggregatedDictionary)
        {
            String sParticipant1, sParticipant2;

            KeyToParticipants(oKeyValuePair.Key,
                out sParticipant1, out sParticipant2);

            aoEmailParticipantPairs[i] = new EmailParticipantPair(
                sParticipant1, sParticipant2, oKeyValuePair.Value);

            i++;
        }

        return (aoEmailParticipantPairs);
    }

    //*************************************************************************
    //  Method: AnalyzeOneEmail()
    //
    /// <summary>
    /// Analyzes the participants in one email.
    /// </summary>
    ///
    /// <param name="oFromField">
    /// The email's "From" fields.  Can be DBNull.
    /// </param>
    ///
    /// <param name="oToField">
    /// The email's "To" fields.  Can be DBNull.
    /// </param>
    ///
    /// <param name="oCcField">
    /// The email's "Cc" fields.  Can be DBNull.
    /// </param>
    ///
    /// <param name="oBccField">
    /// The email's "Bcc" fields.  Can be DBNull.
    /// </param>
    ///
    /// <param name="bUseCcForEdgeWeights">
    /// See the synchronous AnalyzeEmailNetwork() method.
    /// </param>
    ///
    /// <param name="bUseBccForEdgeWeights">
    /// See the synchronous AnalyzeEmailNetwork() method.
    /// </param>
    ///
    /// <param name="oAggregatedDictionary">
    /// Aggregated participant pairs.  The key is the pair of participants in
    /// the format used by ParticipantsToKey() and the value is the edge
    /// weight between the participants.
    /// </param>
    //*************************************************************************

    protected void
    AnalyzeOneEmail
    (
        Object oFromField,
        Object oToField,
        Object oCcField,
        Object oBccField,
        Boolean bUseCcForEdgeWeights,
        Boolean bUseBccForEdgeWeights,
        Dictionary<String, Int32> oAggregatedDictionary
    )
    {
        Debug.Assert(oFromField != null);
        Debug.Assert(oToField != null);
        Debug.Assert(oCcField != null);
        Debug.Assert(oBccField != null);
        Debug.Assert(oAggregatedDictionary != null);
        AssertValid();

        // Get the From participant if possible.

        if ( 
            oFromField is DBNull ||
            !( oFromField is String[] ) ||
            ( ( String [] )oFromField ).Length != 1 || 
            String.IsNullOrEmpty( ( ( String [] )oFromField )[0] )
            )
        {
            // Skip emails with bad From fields.

            return;
        }

        String sFrom = ( ( String [] )oFromField )[0];

        // Dictionary to prevent redundant participant pairs when a participant
        // is included twice in an email -- in both the To and Cc fields, for
        // example.  The key is a participant and the value is not used.

        Dictionary<String, Int32> oPerEmailDictionary =
            new Dictionary<String, Int32>();

        // Always analyze the To field.

        AnalyzeOneEmailField(sFrom, oToField, oPerEmailDictionary,
            oAggregatedDictionary);

        // Analyze the Cc and Bcc fields only if requested.

        if (bUseCcForEdgeWeights)
        {
            AnalyzeOneEmailField(sFrom, oCcField, oPerEmailDictionary,
                oAggregatedDictionary);
        }

        if (bUseBccForEdgeWeights)
        {
            AnalyzeOneEmailField(sFrom, oBccField, oPerEmailDictionary,
                oAggregatedDictionary);
        }
    }

    //*************************************************************************
    //  Method: AnalyzeOneEmailField()
    //
    /// <summary>
    /// Analyzes the participants in one email field.
    /// </summary>
    ///
    /// <param name="sFrom">
    /// The email's sender, in lower case.
    /// </param>
    ///
    /// <param name="oField">
    /// The field that might contain participants.  Can be either DBNull or an
    /// array of strings.
    /// </param>
    ///
    /// <param name="oPerEmailDictionary">
    /// The key is a participant and the value is not used.
    /// </param>
    ///
    /// <param name="oAggregatedDictionary">
    /// Aggregated participant pairs.  The key is the pair of participants in
    /// the format used by ParticipantsToKey() and the value is the edge
    /// weight between the participants.
    /// </param>
    //*************************************************************************

    protected void
    AnalyzeOneEmailField
    (
        String sFrom,
        Object oField,
        Dictionary<String, Int32> oPerEmailDictionary,
        Dictionary<String, Int32> oAggregatedDictionary
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sFrom) );
        Debug.Assert(oField != null);
        Debug.Assert(oPerEmailDictionary != null);
        Debug.Assert(oAggregatedDictionary != null);
        AssertValid();

        if (oField is DBNull)
        {
            return;
        }

        if ( !(oField is String[] ) )
        {
            throw new FormatException(
                "A field is not the expected String[]."
                );
        }

        String sFromLower = sFrom.ToLower();

        foreach (String sParticipant in ( String [] )oField)
        {
            String sParticipantLower = sParticipant.ToLower();

            if ( oPerEmailDictionary.ContainsKey(sParticipantLower) )
            {
                // A participant pair for this participant has already been
                // added to the aggregated dictionary.

                continue;
            }

            oPerEmailDictionary[sParticipantLower] = 0;

            // For each participant in the field (except for the From
            // participant), add a From-To-Participant pair to the aggregated
            // dictionary.

            if (sParticipantLower != sFromLower)
            {
                String sKey = ParticipantsToKey(sFromLower, sParticipantLower);
                Int32 iEdgeWeight = 0;

                if ( oAggregatedDictionary.TryGetValue(
                    sKey, out iEdgeWeight) )
                {
                    iEdgeWeight++;
                }
                else
                {
                    iEdgeWeight = 1;
                }

                oAggregatedDictionary[sKey] = iEdgeWeight;
            }
        }
    }

    //*************************************************************************
    //  Method: ParticipantsToKey()
    //
    /// <summary>
    /// Appends two participants into a dictionary key.
    /// </summary>
    ///
    /// <param name="sParticipant1">
    /// First participant.
    /// </param>
    ///
    /// <param name="sParticipant2">
    /// Second participant.
    /// </param>
    ///
    /// <returns>
    /// A key appropriate for use in the aggregated dictionary.
    /// </returns>
    //*************************************************************************

    protected String
    ParticipantsToKey
    (
        String sParticipant1,
        String sParticipant2
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sParticipant1) );
        Debug.Assert( !String.IsNullOrEmpty(sParticipant2) );
        AssertValid();

        if (sParticipant1.CompareTo(sParticipant2) < 0)
        {
            return (sParticipant1 + KeySeparator + sParticipant2);
        }

        return (sParticipant2 + KeySeparator + sParticipant1);
    }

    //*************************************************************************
    //  Method: KeyToParticipants()
    //
    /// <summary>
    /// Splits a dictionary key into two participants.
    /// </summary>
    ///
    /// <param name="sKey">
    /// Key from the aggregated dictionary.  This must be a string created by
    /// <see cref="ParticipantsToKey" />.
    /// </param>
    ///
    /// <param name="sParticipant1">
    /// Where the first participant gets stored.
    /// </param>
    ///
    /// <param name="sParticipant2">
    /// Where the second participant gets stored.
    /// </param>
    //*************************************************************************

    protected void
    KeyToParticipants
    (
        String sKey,
        out String sParticipant1,
        out String sParticipant2
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sKey) );
        AssertValid();

        String [] asParticipants = sKey.Split(KeySeparator);

        if (
            asParticipants == null || asParticipants.Length != 2 ||
            String.IsNullOrEmpty( asParticipants[0] ) ||
            String.IsNullOrEmpty( asParticipants[1] )
            )
        {
            throw new FormatException(
                "An aggregated dictionary key is in the wrong format."
                );
        }

        sParticipant1 = asParticipants[0];
        sParticipant2 = asParticipants[1];
    }

    //*************************************************************************
    //  Method: CheckAnalyzeMethodArguments()
    //
    /// <summary>
    /// Checks the arguments passed to AnalyzeEmailNetwork().
    /// </summary>
    ///
    /// <param name="participantsCriteria">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="startTime">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="endTime">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="bodyText">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="folder">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="minimumSize">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="maximumSize">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="attachmentFilter">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="isReplyFromParticipant1">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <remarks>
    /// This method throws an exception if any arguments are invalid.
    /// </remarks>
    //*************************************************************************

    protected void
    CheckAnalyzeMethodArguments
    (
        EmailParticipantCriteria [] participantsCriteria,
        Nullable<DateTime> startTime,
        Nullable<DateTime> endTime,
        String bodyText,
        String folder,
        Nullable<Int64> minimumSize,
        Nullable<Int64> maximumSize,
        Nullable<AttachmentFilter> attachmentFilter,
        Nullable<Boolean> isReplyFromParticipant1
    )
    {
        AssertValid();

        ArgumentChecker oArgumentChecker = this.ArgumentChecker;

        Boolean bHasFirstParticipant = false;

        if (participantsCriteria != null)
        {
            foreach (EmailParticipantCriteria participantCriteria in
                participantsCriteria)
            {
                if ( String.IsNullOrEmpty(participantCriteria.Participant) )
                {
                    oArgumentChecker.ThrowArgumentException(AnalyzeMethodName,
                        "participantsCriteria",
                        "An email address wasn't specified.",
                        null
                        );
                }

                bHasFirstParticipant = true;
            }
        }

        if (startTime.HasValue && endTime.HasValue &&
            endTime.Value < startTime.Value)
        {
            oArgumentChecker.ThrowArgumentException(AnalyzeMethodName,
                "endTime",

                "The end time must be greater than or equal to the start"
                + " time.",

                null
                );
        }

        CheckAnalyzeMethodArgument(bodyText, "bodyText");
        CheckAnalyzeMethodArgument(folder, "folder");

        if (minimumSize.HasValue)
        {
            oArgumentChecker.CheckArgumentPositive(
                AnalyzeMethodName, "minimumSize", minimumSize.Value);
        }

        if (maximumSize.HasValue)
        {
            oArgumentChecker.CheckArgumentPositive(
                AnalyzeMethodName, "maximumSize", maximumSize.Value);
        }

        if (minimumSize.HasValue && maximumSize.HasValue &&
            maximumSize.Value < minimumSize.Value)
        {
            oArgumentChecker.ThrowArgumentException(AnalyzeMethodName,
                "maximumSize",

                "maximumSize must be greater than or equal to"
                + " minimumSize."
                ,
                null
                );
        }

        if (attachmentFilter.HasValue &&
            attachmentFilter.Value ==
                AttachmentFilter.HasAttachmentFromParticipant1
            && !bHasFirstParticipant)
        {
            oArgumentChecker.ThrowArgumentException(AnalyzeMethodName,
                "attachmentFilter",

                "If attachmentFilter is HasAttachmentsFromParticipant1, a"
                + " first participant must also be specified.",

                null
                );
        }

        if (isReplyFromParticipant1.HasValue && !bHasFirstParticipant)
        {
            oArgumentChecker.ThrowArgumentException(AnalyzeMethodName,
                "optionaIsReplyFromParticipant1",

                "If isReplyFromParticipant1 is specified, a first participant"
                + " must also be specified."
                ,
                null
                );
        }
    }

    //*************************************************************************
    //  Method: CheckAnalyzeMethodArgument()
    //
    /// <summary>
    /// Checks a string argument passed to AnalyzeEmailNetwork().
    /// </summary>
    ///
    /// <param name="sArgument">
    /// The argument to check.  Can be null but can't be empty.
    /// </param>
    ///
    /// <param name="sParameterName">
    /// Name of the parameter.
    /// </param>
    ///
    /// <remarks>
    /// This method throws an exception if <paramref name="sArgument" /> is an
    /// empty string.
    /// </remarks>
    //*************************************************************************

    protected void
    CheckAnalyzeMethodArgument
    (
        String sArgument,
        String sParameterName
    )
    {
        AssertValid();

        if (sArgument != null)
        {
            // An empty string is not allowed.

            this.ArgumentChecker.CheckArgumentNotEmpty(AnalyzeMethodName,
                sParameterName, sArgument);
        }
    }

    //*************************************************************************
    //  Method: CreateQuery()
    //
    /// <summary>
    /// Creates a query that uses specified criteria.
    /// </summary>
    ///
    /// <param name="participantsCriteria">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="startTime">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="endTime">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="bodyText">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="folder">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="minimumSize">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="maximumSize">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="attachmentFilter">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="hasCc">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="hasBcc">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <param name="isReplyFromParticipant1">
    /// See AnalyzeEmailNetwork().
    /// </param>
    ///
    /// <returns>
    /// A query that uses the specified criteria.
    /// </returns>
    //*************************************************************************

    protected String
    CreateQuery
    (
        EmailParticipantCriteria [] participantsCriteria,
        Nullable<DateTime> startTime,
        Nullable<DateTime> endTime,
        String bodyText,
        String folder,
        Nullable<Int64> minimumSize,
        Nullable<Int64> maximumSize,
        Nullable<AttachmentFilter> attachmentFilter,
        Nullable<Boolean> hasCc,
        Nullable<Boolean> hasBcc,
        Nullable<Boolean> isReplyFromParticipant1
    )
    {
        AssertValid();

        StringBuilder oStringBuilder = new StringBuilder();
        oStringBuilder.Append(BaseQuery);

        String sEscapedFirstParticipant = null;
        Int32 i = 0;

        StringBuilder oParticipantCriteriaStringBuilder =
            new StringBuilder();

        if (participantsCriteria != null)
        {
            foreach (EmailParticipantCriteria oParticipantCriteria in
                participantsCriteria)
            {
                String sEscapedParticipant =
                    EscapeStringForContains(oParticipantCriteria.Participant);

                Debug.Assert( !String.IsNullOrEmpty(sEscapedParticipant) );

                if (i == 0)
                {
                    sEscapedFirstParticipant = sEscapedParticipant;
                }

                i++;

                IncludedIn eIncludedIn = oParticipantCriteria.IncludedIn;

                if (eIncludedIn == IncludedIn.None)
                {
                    // Skip this one.

                    continue;
                }

                if (oParticipantCriteriaStringBuilder.Length > 0)
                {
                    // The participants are ORed together.

                    oParticipantCriteriaStringBuilder.Append(" OR ");
                }

                // This part of the query will look like this, in pseudocode:
                //
                // ( Contains(To, Participant) 
                // OR Contains(From, Participant) OR Contains(Cc, Participant)
                // OR Contains(Bcc, Participant) )

                // Open the outer clause.

                oParticipantCriteriaStringBuilder.Append("(");

                // Append a clause for each IncludedIn flag.

                Boolean bParticipantClauseAppended = false;

                AppendParticipantClauseToQuery(sEscapedParticipant,
                    eIncludedIn, IncludedIn.From,
                    oParticipantCriteriaStringBuilder,
                    ref bParticipantClauseAppended);

                AppendParticipantClauseToQuery(sEscapedParticipant,
                    eIncludedIn, IncludedIn.To,
                    oParticipantCriteriaStringBuilder,
                    ref bParticipantClauseAppended);

                AppendParticipantClauseToQuery(sEscapedParticipant,
                    eIncludedIn, IncludedIn.Cc,
                    oParticipantCriteriaStringBuilder,
                    ref bParticipantClauseAppended);

                AppendParticipantClauseToQuery(sEscapedParticipant,
                    eIncludedIn, IncludedIn.Bcc,
                    oParticipantCriteriaStringBuilder,
                    ref bParticipantClauseAppended);

                // Close the outer clause.

                oParticipantCriteriaStringBuilder.Append(")");
            }

            if (oParticipantCriteriaStringBuilder.Length > 0)
            {
                oStringBuilder.AppendFormat(

                    " AND ({0}) "
                    ,
                    oParticipantCriteriaStringBuilder
                    );
            }
        }

        const String DateSentClause =
            " AND System.Message.DateSent {0} '{1}'"
            ;

        if (startTime.HasValue)
        {
            oStringBuilder.AppendFormat(

                DateSentClause
                ,
                ">=",
                FormatTime(startTime.Value)
                );
        }

        if (endTime.HasValue)
        {
            oStringBuilder.AppendFormat(

                DateSentClause
                ,
                "<=",
                FormatTime(endTime.Value)
                );
        }

        if ( !String.IsNullOrEmpty(bodyText) )
        {
            oStringBuilder.AppendFormat(

                " AND CONTAINS ('\"{0}\"')"
                ,
                EscapeStringForContains(bodyText)
                );
        }

        if ( !String.IsNullOrEmpty(folder) )
        {
            oStringBuilder.AppendFormat(

                " AND System.ItemFolderPathDisplay = '{0}'"
                ,
                EscapeStringForEquals(folder)
                );
        }

        if (minimumSize.HasValue)
        {
            oStringBuilder.AppendFormat(

                " AND System.Size >= {0}"
                ,
                minimumSize.Value
                );
        }

        if (maximumSize.HasValue)
        {
            oStringBuilder.AppendFormat(

                " AND System.Size <= {0}"
                ,
                maximumSize.Value
                );
        }

        if (attachmentFilter.HasValue)
        {
            oStringBuilder.AppendFormat(

                " AND System.Message.HasAttachments = {0}"
                ,
                attachmentFilter.Value == AttachmentFilter.NoAttachment ?
                    "FALSE" : "TRUE"
                );

            if (attachmentFilter.Value ==
                AttachmentFilter.HasAttachmentFromParticipant1)
            {
                Debug.Assert( !String.IsNullOrEmpty(sEscapedFirstParticipant) );

                oStringBuilder.AppendFormat(

                    " AND Contains(System.Message.FromAddress, '\"{0}\"')"
                    ,
                    sEscapedFirstParticipant
                    );
            }
        }

        if (hasCc.HasValue)
        {
            oStringBuilder.AppendFormat(

                " AND System.Message.CcAddress IS {0} NULL"
                ,
                hasCc.Value ? "NOT": String.Empty
                );
        }

        if (hasBcc.HasValue)
        {
            oStringBuilder.AppendFormat(

                " AND System.Message.BccAddress IS {0} NULL"
                ,
                hasBcc.Value ? "NOT": String.Empty
                );
        }

        if (isReplyFromParticipant1.HasValue)
        {
            // IMPORTANT NOTE: As of April 2008, the
            // System.Message.IsFwdOrReply message property needed to implement
            // this is always null, so specifying a value here will always
            // return an empty recordset.
            //
            // Note: System.Message.IsFwdOrReply is an Int32, not a Boolean.
            // The documentation doesn't indicate what the possible values are.
            // I'm assuming that 1 means true and 0 means false.

            oStringBuilder.AppendFormat(

                " AND ( System.Message.IsFwdOrReply = {0} AND"
                + " Contains(System.Message.FromAddress, '\"{1}\"') )"
                ,
                isReplyFromParticipant1.Value ? 1 : 0,
                sEscapedFirstParticipant
                );
        }

        return ( oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: AppendParticipantClauseToQuery()
    //
    /// <summary>
    /// Appends a participant clause to a query.
    /// </summary>
    ///
    /// <param name="sEscapedParticipant">
    /// The participant to use in the clause.
    /// </param>
    ///
    /// <param name="eIncludedIn">
    /// The IncludedIn value from a ParticipantCriteria object.
    /// </param>
    ///
    /// <param name="eFlagToCheck">
    /// The flag to check in <paramref name="eIncludedIn" />.
    /// </param>
    ///
    /// <param name="oStringBuilder">
    /// The query to append to.
    /// </param>
    ///
    /// <param name="bParticipantClauseAppended">
    /// true if a participant clause has already been appended.  Gets updated
    /// by this method.
    /// </param>
    ///
    /// <remarks>
    /// If <paramref name="eIncludedIn" /> includes <paramref
    /// name="eFlagToCheck" />, this method appends a clause that looks like
    /// this, in pseudocode: " Contains(To, Participant)".  If necessary, the
    /// clause is prepended with " OR".
    /// </remarks>
    //*************************************************************************

    protected void
    AppendParticipantClauseToQuery
    (
        String sEscapedParticipant,
        IncludedIn eIncludedIn,
        IncludedIn eFlagToCheck,
        StringBuilder oStringBuilder,
        ref Boolean bParticipantClauseAppended
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sEscapedParticipant) );
        Debug.Assert(oStringBuilder != null);
        AssertValid();

        if ( (eIncludedIn & eFlagToCheck) != 0 )
        {
            if (bParticipantClauseAppended)
            {
                oStringBuilder.Append(" OR");
            }

            oStringBuilder.AppendFormat(
                " Contains(System.Message.{0}Address, '\"{1}\"')"
                ,
                eFlagToCheck.ToString(),
                sEscapedParticipant
                );

            bParticipantClauseAppended = true;
        }
    }

    //*************************************************************************
    //  Method: FormatTime()
    //
    /// <summary>
    /// Formats a DateTime for use in a query.
    /// </summary>
    ///
    /// <returns>
    /// A formatted DateTime.
    /// </returns>
    //*************************************************************************

    protected String
    FormatTime
    (
        DateTime oTime
    )
    {
        AssertValid();

        // Times used in WDS must look like "2006-12-1 8:00:00", according to
        // this article:
        //
        // http://www.microsoft.com/technet/scriptcenter/topics/desktop/
        // wdsearch.mspx

        const String Format = "yyyy-MM-dd HH:mm:ss";

        return ( oTime.ToString(Format) );
    }

    //*************************************************************************
    //  Method: EscapeStringForContains()
    //
    /// <summary>
    /// Escapes a string for use within a SQL CONTAINS clause.
    /// </summary>
    ///
    /// <param name="sString">
    /// The string to escape.  Can be null.
    /// </param>
    ///
    /// <returns>
    /// The escaped string, or null.
    /// </returns>
    //*************************************************************************

    protected String
    EscapeStringForContains
    (
        String sString
    )
    {
        AssertValid();

        if (sString == null)
        {
            return (null);
        }

        // Double quotes must be doubled.

        return ( sString.Replace("\"", "\"\"") );
    }

    //*************************************************************************
    //  Method: EscapeStringForEquals()
    //
    /// <summary>
    /// Escapes a string for use within a SQL equals clause.
    /// </summary>
    ///
    /// <param name="sString">
    /// The string to escape.  Can be null.
    /// </param>
    ///
    /// <returns>
    /// The escaped string, or null.
    /// </returns>
    //*************************************************************************

    protected String
    EscapeStringForEquals
    (
        String sString
    )
    {
        AssertValid();

        if (sString == null)
        {
            return (null);
        }

        // Single quotes must be doubled.

        return ( sString.Replace("'", "''") );
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

        Debug.Assert(e.Argument is AnalyzeEmailNetworkAsyncArgs);

        AnalyzeEmailNetworkAsyncArgs oAnalyzeEmailNetworkAsyncArgs =
            (AnalyzeEmailNetworkAsyncArgs)e.Argument;

        EmailParticipantPair [] aoEmailParticipantPairs =
            AnalyzeEmailNetworkInternal(

            oAnalyzeEmailNetworkAsyncArgs.ParticipantsCriteria,
            oAnalyzeEmailNetworkAsyncArgs.StartTime,
            oAnalyzeEmailNetworkAsyncArgs.EndTime,
            oAnalyzeEmailNetworkAsyncArgs.BodyText,
            oAnalyzeEmailNetworkAsyncArgs.Folder,
            oAnalyzeEmailNetworkAsyncArgs.MinimumSize,
            oAnalyzeEmailNetworkAsyncArgs.MaximumSize,
            oAnalyzeEmailNetworkAsyncArgs.AttachmentFilter,
            oAnalyzeEmailNetworkAsyncArgs.HasCc,
            oAnalyzeEmailNetworkAsyncArgs.HasBcc,
            oAnalyzeEmailNetworkAsyncArgs.IsReplyFromParticipant1,
            oAnalyzeEmailNetworkAsyncArgs.UseCcForEdgeWeights,
            oAnalyzeEmailNetworkAsyncArgs.UseBccForEdgeWeights,
            oBackgroundWorker,
            e
            );

        e.Result = aoEmailParticipantPairs;
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
        // m_oBackgroundWorker
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// The connection string to use for Windows Desktop Search.

    protected const String WdsConnectionString =
        "Provider=Search.CollatorDSO;Extended Properties="
        + "'Application=Windows';"
        ;

    /// Command timeout for the OleDbCommand object, in seconds.

    protected const Int32 OleDbCommandTimeoutSeconds = 60 * 5;

    /// Base query that returns all email items.

    protected const String BaseQuery =
        "SELECT"
        + "  System.Message.FromAddress"
        + ", System.Message.ToAddress"
        + ", System.Message.CcAddress"
        + ", System.Message.BccAddress"
        + " FROM SystemIndex WHERE Contains(System.Kind, 'email')"
        ;

    /// Character used to separate two participants in the aggregated
    /// dictionary key.

    protected const Char KeySeparator = '\n';

    /// Number of records to process before checking whether a cancellation of
    /// an asynchronous analysis has been requested.

    protected const Int64 AsyncCancelInterval = 1000;

    /// Name of the method used to analyze email.  Gets used in error messages.

    protected const String AnalyzeMethodName = "AnalyzeEmailNetwork";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Used for asynchronous analysis.  null if an asynchronous analysis is
    /// not in progress.

    protected BackgroundWorker m_oBackgroundWorker;


    //*************************************************************************
    //  Embedded class: AnalyzeEmailNetworkAsyncArguments()
    //
    /// <summary>
    /// Contains the arguments needed to asynchronously analyze an email
    /// network.
    /// </summary>
    //*************************************************************************

    protected class AnalyzeEmailNetworkAsyncArgs
    {
        ///
        public EmailParticipantCriteria [] ParticipantsCriteria;
        ///
        public Nullable<DateTime> StartTime;
        ///
        public Nullable<DateTime> EndTime;
        ///
        public String BodyText;
        ///
        public String Folder;
        ///
        public Nullable<Int64> MinimumSize;
        ///
        public Nullable<Int64> MaximumSize;
        ///
        public Nullable<AttachmentFilter> AttachmentFilter;
        ///
        public Nullable<Boolean> HasCc;
        ///
        public Nullable<Boolean> HasBcc;
        ///
        public Nullable<Boolean> IsReplyFromParticipant1;
        ///
        public Boolean UseCcForEdgeWeights;
        ///
        public Boolean UseBccForEdgeWeights;
    };
}

}
