

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Reflection;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.SocialNetworkLib;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AnalyzeEmailNetworkDialog
//
/// <summary>
/// Dialog that analyzes a user's email network and writes the results to the
/// edge worksheet.
/// </summary>
///
/// <remarks>
/// An <see cref="EmailNetworkAnalyzer" /> object does most of the work.  The
/// analysis is done asynchronously, so it doesn't hang the UI and can be
/// cancelled by the user.
/// </remarks>
//*****************************************************************************

public partial class AnalyzeEmailNetworkDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: AnalyzeEmailNetworkDialog()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="AnalyzeEmailNetworkDialog" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AnalyzeEmailNetworkDialog" /> class with a workbook.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="clearTablesFirst">
    /// true if the NodeXL tables in <paramref name="workbook" /> should be
    /// cleared first.
    /// </param>
    //*************************************************************************

    public AnalyzeEmailNetworkDialog
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        Boolean clearTablesFirst
    )
    : this()
    {
        Debug.Assert(workbook != null);

        // Instantiate an object that retrieves and saves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oAnalyzeEmailNetworkDialogUserSettings =
            new AnalyzeEmailNetworkDialogUserSettings(this);

        m_oWorkbook = workbook;
        m_bClearTablesFirst = clearTablesFirst;

        m_oEmailNetworkAnalyzer = new EmailNetworkAnalyzer();

        m_oEmailNetworkAnalyzer.AnalysisCompleted +=
            new RunWorkerCompletedEventHandler(
                EmailNetworkAnalyzer_AnalysisCompleted);

        m_oEdgeTable = null;

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: AnalyzeEmailNetworkDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AnalyzeEmailNetworkDialog" /> class for the Visual Studio
    /// designer.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for use by the Visual Studio
    /// designer only.
    /// </remarks>
    //*************************************************************************

    public AnalyzeEmailNetworkDialog()
    {
        InitializeComponent();

        // AssertValid();
    }

    //*************************************************************************
    //  Method: DoDataExchange()
    //
    /// <summary>
    /// Transfers data between the dialog's fields and its controls.
    /// </summary>
    ///
    /// <param name="bFromControls">
    /// true to transfer data from the dialog's controls to its fields, false
    /// for the other direction.
    /// </param>
    ///
    /// <returns>
    /// true if the transfer was successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    DoDataExchange
    (
        Boolean bFromControls
    )
    {
        if (bFromControls)
        {
            return ( DoDataExchangeFromControls() );
        }

        DoDataExchangeToControls();

        return (true);
    }

    //*************************************************************************
    //  Method: DoDataExchangeFromControls()
    //
    /// <summary>
    /// Transfers data from the dialog's controls to its fields.
    /// </summary>
    ///
    /// <returns>
    /// true if the transfer was successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    DoDataExchangeFromControls()
    {
        Boolean bAllEmail = radAllEmail.Checked;
        Boolean bUseParticipants = !bAllEmail && cbxUseParticipants.Checked;
        Boolean bUseStartTime = !bAllEmail && cbxUseStartTime.Checked;
        Boolean bUseEndTime = !bAllEmail && cbxUseEndTime.Checked;
        Boolean bUseFolder = !bAllEmail && cbxUseFolder.Checked;
        Boolean bUseMinimumSize = !bAllEmail && cbxUseMinimumSize.Checked;
        Boolean bUseMaximumSize = !bAllEmail && cbxUseMaximumSize.Checked;
        Boolean bUseBodyText = !bAllEmail && cbxUseBodyText.Checked;

        Boolean bUseAttachmentFilter = !bAllEmail &&
            cbxUseAttachmentFilter.Checked;

        String sFolder = null;
        String sBodyText = null;

        if (!bAllEmail)
        {
            // Validate the controls.

            if (
                ( bUseParticipants && !ValidateParticipants() )
                ||
                !ValidateFilterTextBox(bUseBodyText, txbBodyText,
                    "message body text", out sBodyText)
                )
            {
                return (false);
            }

            if (bUseStartTime && bUseEndTime &&
                dtpEndTime.Value.Date < dtpStartTime.Value.Date)
            {
                OnInvalidDateTimePicker(dtpEndTime,
                    "The \"on or before\" date can't be earlier than the"
                    + " \"on or after\" date.");

                return (false);
            }

            Decimal decMinimumSize = Decimal.MinValue;
            Decimal decMaximumSize = Decimal.MinValue;

            if ( bUseMinimumSize && !ValidateNumericUpDown(
                nudMinimumSize, "a minimum size", out decMinimumSize) )
            {
                return (false);
            }

            if ( bUseMaximumSize && !ValidateNumericUpDown(
                nudMaximumSize, "a maximum size", out decMaximumSize) )
            {
                return (false);
            }

            if (bUseMinimumSize && bUseMaximumSize &&
                decMaximumSize < decMinimumSize)
            {
                return ( OnInvalidNumericUpDown(nudMaximumSize,
                    "The maximum size can't be less than the minimum size."
                    ) );
            }

            if ( bUseAttachmentFilter &&
                radHasAttachmentFromParticipant1.Checked &&
                !FirstParticipantSpecified() )
            {
                ShowWarning(
                    "If you want to filter on emails that have attachments"
                    + " from the first email address, you must"
                    + " check \"Includes these email addresses on the From,"
                    + " To, Cc, or Bcc lines\" and specify an email address in"
                    + " the email address list."
                    );

                radHasAttachmentFromParticipant1.Focus();

                return (false);
            }

            if ( !ValidateFilterTextBox(bUseFolder, txbFolder, "a folder",
                out sFolder)  )
            {
                return (false);
            }
        }

        // The controls have been validated.  Transfer their contents to the
        // user settings.

        m_oAnalyzeEmailNetworkDialogUserSettings.AnalyzeAllEmail = bAllEmail;

        if (bUseParticipants)
        {
            DoDataExchangeFromParticipants();
        }
        else
        {
            m_oAnalyzeEmailNetworkDialogUserSettings.ParticipantsCriteria =
                null;
        }

        m_oAnalyzeEmailNetworkDialogUserSettings.StartTime =
            bUseStartTime ? dtpStartTime.Value.Date :
                (Nullable<DateTime>)null;

        m_oAnalyzeEmailNetworkDialogUserSettings.EndTime =
            bUseEndTime ? dtpEndTime.Value.Date :
                (Nullable<DateTime>)null;

        m_oAnalyzeEmailNetworkDialogUserSettings.Folder = sFolder;

        m_oAnalyzeEmailNetworkDialogUserSettings.MinimumSize =
            bUseMinimumSize ? (Int64)nudMinimumSize.Value
                : (Nullable<Int64>)null;

        m_oAnalyzeEmailNetworkDialogUserSettings.MaximumSize =
            bUseMaximumSize ? (Int64)nudMaximumSize.Value
                : (Nullable<Int64>)null;

        m_oAnalyzeEmailNetworkDialogUserSettings.HasCc = cbxUseCc.Checked ?
            radHasCc.Checked : ( Nullable<Boolean> )null;

        m_oAnalyzeEmailNetworkDialogUserSettings.HasBcc =
            cbxUseBcc.Checked ? radHasBcc.Checked :
                ( Nullable<Boolean> )null;

        m_oAnalyzeEmailNetworkDialogUserSettings.BodyText = sBodyText;

        if (bUseAttachmentFilter)
        {
            if (radHasAttachment.Checked)
            {
                m_oAnalyzeEmailNetworkDialogUserSettings.AttachmentFilter =
                    AttachmentFilter.HasAttachment;
            }
            else if (radNoAttachment.Checked)
            {
                m_oAnalyzeEmailNetworkDialogUserSettings.AttachmentFilter =
                    AttachmentFilter.NoAttachment;
            }
            else if (radHasAttachmentFromParticipant1.Checked)
            {
                m_oAnalyzeEmailNetworkDialogUserSettings.AttachmentFilter =
                    AttachmentFilter.HasAttachmentFromParticipant1;
            }
            else
            {
                Debug.Assert(false);
            }
        }
        else
        {
            m_oAnalyzeEmailNetworkDialogUserSettings.AttachmentFilter =
                null;
        }

        m_oAnalyzeEmailNetworkDialogUserSettings.UseCcForEdgeWeights =
            cbxUseCcForEdgeWeights.Checked;

        m_oAnalyzeEmailNetworkDialogUserSettings.UseBccForEdgeWeights =
            cbxUseBccForEdgeWeights.Checked;

        return (true);
    }

    //*************************************************************************
    //  Method: DoDataExchangeToControls()
    //
    /// <summary>
    /// Transfers data between from the dialog's fields to its controls.
    /// </summary>
    //*************************************************************************

    protected void
    DoDataExchangeToControls()
    {
        radFilteredEmail.Checked = !(radAllEmail.Checked =
            m_oAnalyzeEmailNetworkDialogUserSettings.AnalyzeAllEmail);

        cbxUseParticipants.Checked =
            (m_oAnalyzeEmailNetworkDialogUserSettings.ParticipantsCriteria
            != null);

        DoDataExchangeToParticipants();

        Nullable<DateTime> oStartTime =
            m_oAnalyzeEmailNetworkDialogUserSettings.StartTime;

        if (oStartTime.HasValue)
        {
            cbxUseStartTime.Checked = true;
            dtpStartTime.Value = oStartTime.Value;
        }
        else
        {
            cbxUseStartTime.Checked = false;
        }

        Nullable<DateTime> oEndTime =
            m_oAnalyzeEmailNetworkDialogUserSettings.EndTime;

        if (oEndTime.HasValue)
        {
            cbxUseEndTime.Checked = true;
            dtpEndTime.Value = oEndTime.Value;
        }
        else
        {
            cbxUseEndTime.Checked = false;
        }

        cbxUseFolder.Checked = !String.IsNullOrEmpty(
            m_oAnalyzeEmailNetworkDialogUserSettings.Folder);

        txbFolder.Text = m_oAnalyzeEmailNetworkDialogUserSettings.Folder;

        Nullable<Int64> oMinimumSize =
            m_oAnalyzeEmailNetworkDialogUserSettings.MinimumSize;

        if (oMinimumSize.HasValue)
        {
            cbxUseMinimumSize.Checked = true;
            nudMinimumSize.Value = oMinimumSize.Value;
        }
        else
        {
            cbxUseMinimumSize.Checked = false;
        }

        Nullable<Int64> oMaximumSize =
            m_oAnalyzeEmailNetworkDialogUserSettings.MaximumSize;

        if (oMaximumSize.HasValue)
        {
            cbxUseMaximumSize.Checked = true;
            nudMaximumSize.Value = oMaximumSize.Value;
        }
        else
        {
            cbxUseMaximumSize.Checked = false;
        }

        Nullable<Boolean> bHasCc =
            m_oAnalyzeEmailNetworkDialogUserSettings.HasCc;

        if (bHasCc.HasValue)
        {
            cbxUseCc.Checked = true;

            radNoCc.Checked = !(radHasCc.Checked = bHasCc.Value);
        }
        else
        {
            cbxUseCc.Checked = false;
        }

        Nullable<Boolean> bHasBcc =
            m_oAnalyzeEmailNetworkDialogUserSettings.HasBcc;

        if (bHasBcc.HasValue)
        {
            cbxUseBcc.Checked = true;

            radNoBcc.Checked = !(radHasBcc.Checked = bHasBcc.Value);
        }
        else
        {
            cbxUseBcc.Checked = false;
        }

        cbxUseBodyText.Checked = !String.IsNullOrEmpty(
            m_oAnalyzeEmailNetworkDialogUserSettings.BodyText);

        txbBodyText.Text =
            m_oAnalyzeEmailNetworkDialogUserSettings.BodyText;

        Nullable<AttachmentFilter> oAttachmentFilter =
            m_oAnalyzeEmailNetworkDialogUserSettings.AttachmentFilter;

        cbxUseAttachmentFilter.Checked = oAttachmentFilter.HasValue;

        if (oAttachmentFilter.HasValue)
        {
            radHasAttachment.Checked = radNoAttachment.Checked =
                radHasAttachmentFromParticipant1.Checked = false;

            switch (m_oAnalyzeEmailNetworkDialogUserSettings.
                AttachmentFilter)
            {
                case AttachmentFilter.HasAttachment:

                    radHasAttachment.Checked = true;
                    break;

                case AttachmentFilter.NoAttachment:

                    radNoAttachment.Checked = true;
                    break;

                case AttachmentFilter.HasAttachmentFromParticipant1:

                    radHasAttachmentFromParticipant1.Checked = true;
                    break;

                default:

                    Debug.Assert(false);
                    break;
            }
        }

        cbxUseCcForEdgeWeights.Checked =
            m_oAnalyzeEmailNetworkDialogUserSettings.UseCcForEdgeWeights;

        cbxUseBccForEdgeWeights.Checked =
            m_oAnalyzeEmailNetworkDialogUserSettings.UseBccForEdgeWeights;

        EnableControls();
    }

    //*************************************************************************
    //  Method: DoDataExchangeFromParticipants()
    //
    /// <summary>
    /// Transfers data from the dgvParticipants DataGridView to this dialog's
    /// fields.
    /// </summary>
    ///
    /// <remarks>
    /// It's assumed that <see cref="ValidateParticipants" /> has already been
    /// called and that it returned true.
    /// </remarks>
    //*************************************************************************

    protected void
    DoDataExchangeFromParticipants()
    {
        AssertValid();

        List<EmailParticipantCriteria> oList =
            new List<EmailParticipantCriteria>();

        foreach (DataGridViewRow oRow in dgvParticipants.Rows)
        {
            if (oRow.IsNewRow)
            {
                continue;
            }

            EmailParticipantCriteria oEmailParticipantCriteria =
                DataGridViewRowToEmailParticipantCriteria(oRow);

            if ( !String.IsNullOrEmpty(oEmailParticipantCriteria.Participant) )
            {
                oList.Add(oEmailParticipantCriteria);
            }
        }

        m_oAnalyzeEmailNetworkDialogUserSettings.ParticipantsCriteria = 
            (oList.Count == 0) ? null : oList.ToArray();
    }

    //*************************************************************************
    //  Method: DoDataExchangeToParticipants()
    //
    /// <summary>
    /// Transfers data from this dialog's fields to the dgvParticipants
    /// DataGridView.
    /// </summary>
    //*************************************************************************

    protected void
    DoDataExchangeToParticipants()
    {
        AssertValid();

        EmailParticipantCriteria [] aoParticipantsCriteria =
            m_oAnalyzeEmailNetworkDialogUserSettings.ParticipantsCriteria;

        if (aoParticipantsCriteria == null)
        {
            return;
        }

        DataGridViewRowCollection oRows = dgvParticipants.Rows;

        foreach (EmailParticipantCriteria oEmailParticipantCriteria in
            aoParticipantsCriteria)
        {
            Int32 iNewRowIndex = oRows.Add();
            DataGridViewRow oRow = oRows[iNewRowIndex];
            DataGridViewCellCollection oCells = oRow.Cells;

            oCells[this.colParticipant.Name].Value = AnalyzerToParticipant(
                oEmailParticipantCriteria.Participant);

            IncludedIn eIncludedIn = oEmailParticipantCriteria.IncludedIn;

            if ( (eIncludedIn & IncludedIn.From) != 0 )
            {
                oCells[this.colFrom.Name].Value = true;
            }

            if ( (eIncludedIn & IncludedIn.To) != 0 )
            {
                oCells[this.colTo.Name].Value = true;
            }

            if ( (eIncludedIn & IncludedIn.Cc) != 0 )
            {
                oCells[this.colCc.Name].Value = true;
            }

            if ( (eIncludedIn & IncludedIn.Bcc) != 0 )
            {
                oCells[this.colBcc.Name].Value = true;
            }
        }
    }

    //*************************************************************************
    //  Method: ValidateParticipants()
    //
    /// <summary>
    /// Validates the dgvParticipants DataGridView.
    /// </summary>
    ///
    /// <returns>
    /// true if the DataGridView contains valid data.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ValidateParticipants()
    {
        AssertValid();

        foreach (DataGridViewRow oRow in dgvParticipants.Rows)
        {
            if (oRow.IsNewRow)
            {
                continue;
            }

            EmailParticipantCriteria oEmailParticipantCriteria =
                DataGridViewRowToEmailParticipantCriteria(oRow);

            String sParticipant = oEmailParticipantCriteria.Participant;

            String sErrorMessage = null;

            if ( String.IsNullOrEmpty(oEmailParticipantCriteria.Participant) )
            {
                // Don't let the user check a checkbox without also specifying
                // a participant.

                if (oEmailParticipantCriteria.IncludedIn != IncludedIn.None)
                {
                    sErrorMessage = 
                        "In the list of email addresses, you checked a"
                        + " checkbox without entering an email address.  Enter"
                        + " an email address, uncheck the checkbox, or delete"
                        + " the entire row using the Delete key."
                        ;
                }
            }
            else
            {
                if ( !ValidateFilter(sParticipant) )
                {
                    sErrorMessage = 
                        "There must be at least one letter or number in an"
                        + " email address."
                        ;
                }
            }

            if (sErrorMessage != null)
            {
                dgvParticipants.CurrentCell =
                    oRow.Cells[this.colParticipant.Name];

                this.ShowWarning(sErrorMessage);

                return (false);
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: DataGridViewRowToEmailParticipantCriteria()
    //
    /// <summary>
    /// Creates a <see cref="EmailParticipantCriteria" /> object from the
    /// contents of a DataGridViewRow from the dgvParticipants DataGridView.
    /// </summary>
    ///
    /// <returns>
    /// A <see cref="EmailParticipantCriteria" /> object.  Important: The
    /// DataGridViewRow is not validated and the returned object may contain
    /// invalid data.
    /// </returns>
    //*************************************************************************

    protected EmailParticipantCriteria
    DataGridViewRowToEmailParticipantCriteria
    (
        DataGridViewRow oRow
    )
    {
        Debug.Assert(oRow != null);
        AssertValid();

        EmailParticipantCriteria oEmailParticipantCriteria =
            new EmailParticipantCriteria();

        String sParticipant =
            (String)oRow.Cells[this.colParticipant.Name].Value;

        if ( !String.IsNullOrEmpty(sParticipant) )
        {
            // Trim the participant string.

            sParticipant = sParticipant.Trim();
            oRow.Cells[this.colParticipant.Name].Value = sParticipant;
        }

        if ( !String.IsNullOrEmpty(sParticipant) )
        {
            sParticipant = ParticipantToAnalyzer(sParticipant);
        }

        oEmailParticipantCriteria.Participant = sParticipant;

        IncludedIn eIncludedIn = IncludedIn.None;

        if ( DataGridViewCheckBoxCellIsChecked(oRow, this.colFrom.Name) )
        {
            eIncludedIn |= IncludedIn.From;
        }

        if ( DataGridViewCheckBoxCellIsChecked(oRow, this.colTo.Name) )
        {
            eIncludedIn |= IncludedIn.To;
        }

        if ( DataGridViewCheckBoxCellIsChecked(oRow, this.colCc.Name) )
        {
            eIncludedIn |= IncludedIn.Cc;
        }

        if ( DataGridViewCheckBoxCellIsChecked(oRow, this.colBcc.Name) )
        {
            eIncludedIn |= IncludedIn.Bcc;
        }

        oEmailParticipantCriteria.IncludedIn = eIncludedIn;

        return (oEmailParticipantCriteria);
    }

    //*************************************************************************
    //  Method: FirstParticipantSpecified()
    //
    /// <summary>
    /// Determines whether a first participant has been specified in the
    /// dgvParticipants DataGridView.
    /// </summary>
    ///
    /// <returns>
    /// true if a first participant has been specified.
    /// </returns>
    //*************************************************************************

    protected Boolean
    FirstParticipantSpecified()
    {
        AssertValid();

        if (!cbxUseParticipants.Checked)
        {
            return (false);
        }

        DataGridViewRowCollection oRows = dgvParticipants.Rows;

        if (oRows.Count == 0)
        {
            return (false);
        }

        DataGridViewRow oFirstRow = oRows[0];

        if (oFirstRow.IsNewRow)
        {
            return (false);
        }

        String sFirstParticipant =
            (String)oFirstRow.Cells[this.colParticipant.Name].Value;

        if ( sFirstParticipant == null ||
            String.IsNullOrEmpty( sFirstParticipant.Trim() ) )
        {
            return (false);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: DataGridViewCheckBoxCellIsChecked()
    //
    /// <summary>
    /// Determines whether a DataGridViewCheckBoxCell is checked.
    /// </summary>
    ///
    /// <returns>
    /// true if the DataGridViewCheckBoxCell is checked.
    /// </returns>
    //*************************************************************************

    protected Boolean
    DataGridViewCheckBoxCellIsChecked
    (
        DataGridViewRow oRow,
        String sColumnName
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );
        AssertValid();

        Object oValue = oRow.Cells[sColumnName].Value;

        return ( oValue != null && ( (Boolean)oValue ) );
    }

    //*************************************************************************
    //  Method: StartAnalysis()
    //
    /// <summary>
    /// Starts the email analysis.
    /// </summary>
    ///
    /// <remarks>
    /// It's assumed that m_oAnalyzeEmailNetworkDialogUserSettings contains
    /// valid settings.
    /// </remarks>
    //*************************************************************************

    protected void
    StartAnalysis()
    {
        AssertValid();

        EmailParticipantCriteria [] aoParticipantsCriteria = null;
        Nullable<DateTime> oStartTime = null;
        Nullable<DateTime> oEndTime = null;
        String sBodyText = null;
        String sFolder = null;
        Nullable<Int64> lMinimumSize = null;
        Nullable<Int64> lMaximumSize = null;
        Nullable<AttachmentFilter> eAttachmentFilter = null;
        Nullable<Boolean> bHasCc = null;
        Nullable<Boolean> bHasBcc = null;
        Nullable<Boolean> bIsReplyFromParticipant1 = null;

        if (!m_oAnalyzeEmailNetworkDialogUserSettings.AnalyzeAllEmail)
        {
            aoParticipantsCriteria =
                m_oAnalyzeEmailNetworkDialogUserSettings.ParticipantsCriteria;

            oStartTime = m_oAnalyzeEmailNetworkDialogUserSettings.StartTime;
            oEndTime = m_oAnalyzeEmailNetworkDialogUserSettings.EndTime;

            if (oEndTime.HasValue)
            {
                // EmailNetworkAnalyzer allows any DateTime values for the
                // start and end times, not just dates with zero times.  This
                // dialog uses only dates with zero times, however.  Therefore,
                // when the user specifies 4/1/2008 as an end time, for
                // example, it needs to be converted to midnight of 4/2/2008.

                oEndTime = m_oAnalyzeEmailNetworkDialogUserSettings.
                    EndTime.Value.AddDays(1);
            }

            sBodyText = m_oAnalyzeEmailNetworkDialogUserSettings.BodyText;

            sFolder = m_oAnalyzeEmailNetworkDialogUserSettings.Folder;

            lMinimumSize =
                m_oAnalyzeEmailNetworkDialogUserSettings.MinimumSize;

            lMaximumSize =
                m_oAnalyzeEmailNetworkDialogUserSettings.MaximumSize;

            eAttachmentFilter =
                m_oAnalyzeEmailNetworkDialogUserSettings.AttachmentFilter;

            bHasCc = m_oAnalyzeEmailNetworkDialogUserSettings.HasCc;
            bHasBcc = m_oAnalyzeEmailNetworkDialogUserSettings.HasBcc;
        }

        // Start the analysis.

        m_oEmailNetworkAnalyzer.AnalyzeEmailNetworkAsync(
            aoParticipantsCriteria, oStartTime, oEndTime, sBodyText, sFolder,
            lMinimumSize, lMaximumSize, eAttachmentFilter, bHasCc, bHasBcc,
            bIsReplyFromParticipant1,
            m_oAnalyzeEmailNetworkDialogUserSettings.UseCcForEdgeWeights,
            m_oAnalyzeEmailNetworkDialogUserSettings.UseBccForEdgeWeights
            );
    }

    //*************************************************************************
    //  Method: PopulateEdgesTable()
    //
    /// <summary>
    /// Populates the edge table with participant pairs.
    /// </summary>
    ///
    /// <param name="aoEmailParticipantPairs">
    /// Analysis results.
    /// </param>
    ///
    /// <remarks>
    /// Note: This method modifies <paramref name="aoEmailParticipantPairs" />
    /// by converting each participant string to one formatted for the edge
    /// worksheet.
    /// </remarks>
    //*************************************************************************

    protected void
    PopulateEdgesTable
    (
        EmailParticipantPair [] aoEmailParticipantPairs
    )
    {
        Debug.Assert(aoEmailParticipantPairs != null);
        Debug.Assert(m_oEdgeTable != null);
        AssertValid();

        Int32 iEmailParticipantPairs = aoEmailParticipantPairs.Length;

        if (iEmailParticipantPairs == 0)
        {
            return;
        }

        // Create and populate an array of edge weights.

        Object [,] aoEdgeWeights = new Object [iEmailParticipantPairs, 1];

        for (Int32 i = 0; i < iEmailParticipantPairs; i++)
        {
            EmailParticipantPair oEmailParticipantPair =
                aoEmailParticipantPairs[i];

            // Modify the pariticpant strings.

            oEmailParticipantPair.Participant1 =
                AnalyzerToParticipant(oEmailParticipantPair.Participant1);

            oEmailParticipantPair.Participant2 =
                AnalyzerToParticipant(oEmailParticipantPair.Participant2);

            aoEdgeWeights[i, 0] = oEmailParticipantPair.EdgeWeight.ToString();
        }

        Int32 iRowOffsetToWriteTo = 0;

        if (!m_bClearTablesFirst)
        {
            iRowOffsetToWriteTo = ExcelUtil.GetOffsetOfFirstEmptyTableRow(
                m_oEdgeTable);
        }

        // Write the arrays to the edge table.

        NodeXLWorkbookUtil.PopulateEdgeTableWithParticipantPairs(
            m_oEdgeTable, aoEmailParticipantPairs, iRowOffsetToWriteTo);

        SetEdgeWeightValues(aoEdgeWeights, iRowOffsetToWriteTo);

        ExcelUtil.ActivateWorksheet(m_oEdgeTable);
    }

    //*************************************************************************
    //  Method: SetEdgeWeightValues()
    //
    /// <summary>
    /// Sets the values of the edge table's edge weight column.
    /// </summary>
    ///
    /// <param name="aoEdgeWeights">
    /// One-column array of edge weights.
    /// </param>
    ///
    /// <param name="iRowOffsetToWriteTo">
    /// Offset to write to in the edge table, measured from the first row in
    /// the table's data range.
    /// </param>
    ///
    /// <remarks>
    /// If the edge weight column doesn't exist, this method creates it.
    /// </remarks>
    //*************************************************************************

    protected void
    SetEdgeWeightValues
    (
        Object [,] aoEdgeWeights,
        Int32 iRowOffsetToWriteTo
    )
    {
        Debug.Assert(aoEdgeWeights != null);
        AssertValid();

        Range oEdgeWeightColumnData;

        Boolean bFound = ExcelUtil.TryGetTableColumnData(m_oEdgeTable,
            EdgeTableColumnNames.EdgeWeight, out oEdgeWeightColumnData);

        if (!bFound)
        {
            // Create the column.

            ListColumn oListColumn;

            if ( !ExcelUtil.TryAddTableColumn(m_oEdgeTable,
                EdgeTableColumnNames.EdgeWeight,
                ExcelUtil.AutoColumnWidth, null, out oListColumn) )
            {
                this.ShowWarning(
                    "The edge weight column wasn't added."
                    );

                return;
            }

            bFound = ExcelUtil.TryGetTableColumnData(m_oEdgeTable,
                EdgeTableColumnNames.EdgeWeight, out oEdgeWeightColumnData);

            Debug.Assert(bFound);
        }

        ExcelUtil.OffsetRange(ref oEdgeWeightColumnData, iRowOffsetToWriteTo,
            0);

        ExcelUtil.SetRangeValues(oEdgeWeightColumnData, aoEdgeWeights);
    }

    //*************************************************************************
    //  Method: EnableControls()
    //
    /// <summary>
    /// Enables or disables the dialog's controls.
    /// </summary>
    //*************************************************************************

    protected void
    EnableControls()
    {
        AssertValid();

        if (m_oEmailNetworkAnalyzer.IsBusy)
        {
            pnlWhichEmails.Enabled = grpFilters.Enabled = lnkHelp.Enabled = 
                btnAnalyze.Enabled = false;

            this.UseWaitCursor = true;
        }
        else
        {
            pnlWhichEmails.Enabled = true;

            grpFilters.Enabled = radFilteredEmail.Checked;

            dgvParticipants.Enabled = cbxUseParticipants.Checked;

            dtpStartTime.Enabled = cbxUseStartTime.Checked;
            dtpEndTime.Enabled = cbxUseEndTime.Checked;

            nudMinimumSize.Enabled = cbxUseMinimumSize.Checked;
            nudMaximumSize.Enabled = cbxUseMaximumSize.Checked;

            EnableControls(cbxUseCc.Checked, radHasCc, radNoCc);
            EnableControls(cbxUseBcc.Checked, radHasBcc, radNoBcc);

            txbBodyText.Enabled = cbxUseBodyText.Checked;
            txbFolder.Enabled = cbxUseFolder.Checked;

            EnableControls(cbxUseAttachmentFilter.Checked, radHasAttachment,
                radNoAttachment, radHasAttachmentFromParticipant1);

            lnkHelp.Enabled = true;
            btnAnalyze.Enabled = true;

            this.UseWaitCursor = false;
        }

        ShowParticipantsEnabledState();
    }

    //*************************************************************************
    //  Method: ShowParticipantsEnabledState()
    //
    /// <summary>
    /// Changes the appearance of the dgvParticipants DataGridView to make it
    /// look enabled or disabled.
    /// </summary>
    //*************************************************************************

    protected void
    ShowParticipantsEnabledState()
    {
        AssertValid();

        // The DataGridView doesn't look disabled when it's disabled.  Work
        // around that.

        dgvParticipants.ForeColor =
            dgvParticipants.ColumnHeadersDefaultCellStyle.ForeColor =
            dgvParticipants.Enabled ?
            SystemColors.ControlText : SystemColors.GrayText;
    }

    //*************************************************************************
    //  Method: ValidateFilterTextBox()
    //
    /// <summary>
    /// Validates a filter TextBox.
    /// </summary>
    ///
    /// <param name="bCheckBoxIsChecked">
    /// true if the CheckBox controlling the filter is checked.
    /// </param>
    ///
    /// <param name="oTextBox">
    /// TextBox that contains the filter text.
    /// </param>
    ///
    /// <param name="sFilterDescription">
    /// Description of the filter.  Gets used in an error message.
    /// </param>
    ///
    /// <param name="sTrimmedText">
    /// Where the trimmed contents of <paramref name="oTextBox" /> gets stored
    /// if true is returned.  Can be null.
    /// </param>
    ///
    /// <returns>
    /// true if the TextBox is valid.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="bCheckBoxIsChecked" /> is false, true is always
    /// returned.  If <paramref name="bCheckBoxIsChecked" /> is true
    /// and <paramref name="oTextBox" /> is not empty, the trimmed text is
    /// stored at <paramref name="sTrimmedText" /> and true is returned.  If
    /// <paramref name="oTextBox" /> doesn't contain at least one letter or
    /// number, an error message is displayed and false is returned.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    ValidateFilterTextBox
    (
        Boolean bCheckBoxIsChecked,
        System.Windows.Forms.TextBox oTextBox,
        String sFilterDescription,
        out String sTrimmedText
    )
    {
        Debug.Assert(oTextBox != null);
        Debug.Assert( !String.IsNullOrEmpty(sFilterDescription) );
        AssertValid();

        sTrimmedText = null;

        if (!bCheckBoxIsChecked)
        {
            return (true);
        }

        // Remove leading and trailing spaces.

        sTrimmedText = oTextBox.Text.Trim();
        oTextBox.Text = sTrimmedText;

        if (sTrimmedText.Length == 0)
        {
            sTrimmedText = null;

            return (true);
        }

        if ( !ValidateFilter(sTrimmedText) )
        {
            String sErrorMessage = String.Format(

                "There must be at least one letter or number in {0}."
                ,
                sFilterDescription
                );

            return ( OnInvalidTextBox(oTextBox, sErrorMessage) );
        }

        return (true);
    }

    //*************************************************************************
    //  Method: ValidateFilter()
    //
    /// <summary>
    /// Validates a filter string.
    /// </summary>
    ///
    /// <param name="sString">
    /// Possible filter string.  Can't be null.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="sString" /> is a valid filter string, false if
    /// it violates rules imposed by Windows Desktop Search.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ValidateFilter
    (
        String sString
    )
    {
        Debug.Assert(sString != null);
        AssertValid();

        // Windows Desktop Search throws an exception if you pass it nothing
        // but spaces and special characters.  Prevent this.
        //
        // Note: Don't use \w, because that includes the underscore character.
        //
        // TODO: This can't be the correct way to do this.  What if a non-
        // ASCII character set is in use?

        Regex oRegex = new Regex("[a-zA-Z0-9]");

        return ( oRegex.IsMatch(sString) );
    }

    //*************************************************************************
    //  Method: ParticipantToAnalyzer()
    //
    /// <summary>
    /// Converts a participant string entered by the user to one formatted for
    /// use by <see cref="EmailNetworkAnalyzer" />.
    /// </summary>
    ///
    /// <param name="sParticipantEnteredByUser">
    /// Participant as entered by the user.  Sample: "joe@msn.com".  Can be
    /// null.
    /// </param>
    ///
    /// <return>
    /// Converted participant, or null if <paramref
    /// name="sParticipantEnteredByUser" /> is null.  Sample:
    /// "&lt;joe@msn.com&gt;".
    /// </return>
    //*************************************************************************

    protected String
    ParticipantToAnalyzer
    (
        String sParticipantEnteredByUser
    )
    {
        Debug.Assert(sParticipantEnteredByUser == null ||
            sParticipantEnteredByUser.Length > 0);

        AssertValid();

        if (sParticipantEnteredByUser == null)
        {
            return (null);
        }

        return ("<" + sParticipantEnteredByUser + ">");
    }

    //*************************************************************************
    //  Method: AnalyzerToParticipant()
    //
    /// <summary>
    /// Converts a participant string returned by the <see
    /// cref="EmailNetworkAnalyzer" /> to one formatted for the edge worksheet.
    /// </summary>
    ///
    /// <param name="sParticipantFromAnalyzer">
    /// Participant returned by the <see cref="EmailNetworkAnalyzer" />.
    /// Sample: "&lt;joe@msn.com&gt;".  Can't be null or empty.
    /// </param>
    ///
    /// <return>
    /// Converted participant.  Sample: "joe@msn.com".
    /// </return>
    //*************************************************************************

    protected String
    AnalyzerToParticipant
    (
        String sParticipantFromAnalyzer
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sParticipantFromAnalyzer) );
        AssertValid();

        String sParticipant = sParticipantFromAnalyzer;

        if ( sParticipant.StartsWith("<") )
        {
            sParticipant = sParticipant.Substring(1);
        }

        if ( sParticipant.EndsWith(">") )
        {
            sParticipant = sParticipant.Substring(0, sParticipant.Length - 1);
        }

        return (sParticipant);
    }

    //*************************************************************************
    //  Method: OnLoad()
    //
    /// <summary>
    /// Handles the Load event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected override void
    OnLoad
    (
        EventArgs e
    )
    {
        AssertValid();

        base.OnLoad(e);

        // Get the required edge table before the user does anything in the
        // dialog.

        EdgeWorksheetReader oEdgeWorksheetReader = new EdgeWorksheetReader();

        try
        {
            m_oEdgeTable = oEdgeWorksheetReader.GetEdgeTable(m_oWorkbook);
        }
        catch (Exception oException)
        {
            // The edge table couldn't be found.  Tell the user and close the
            // dialog.

            ErrorUtil.OnException(oException);

            this.Close();

            return;
        }
    }

    //*************************************************************************
    //  Method: OnClosed()
    //
    /// <summary>
    /// Handles the Closed event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected override void
    OnClosed
    (
        EventArgs e
    )
    {
        AssertValid();

        if (m_oEmailNetworkAnalyzer.IsBusy)
        {
            // Let the background thread cancel its task, but don't try to
            // notify this dialog.

            m_oEmailNetworkAnalyzer.AnalysisCompleted -=
                new RunWorkerCompletedEventHandler(
                    EmailNetworkAnalyzer_AnalysisCompleted);

            m_oEmailNetworkAnalyzer.CancelAsync();
        }
    }

    //*************************************************************************
    //  Method: OnEventThatRequiresControlEnabling()
    //
    /// <summary>
    /// Handles any event that should changed the enabled state of the dialog's
    /// controls.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    OnEventThatRequiresControlEnabling
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        EnableControls();
    }

    //*************************************************************************
    //  Method: OnAnalysisCompleted()
    //
    /// <summary>
    /// Handles the AnalysisCompleted event on the EmailNetworkAnalyzer object.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    OnAnalysisCompleted
    (
        RunWorkerCompletedEventArgs e
    )
    {
        AssertValid();

        EnableControls();

        if (e.Cancelled)
        {
            // (Do nothing.)
        }
        else if (e.Error != null)
        {
            Exception oException = e.Error;

            if (oException is WdsConnectionFailureException)
            {
                this.ShowWarning(WdsConnectionFailureMessage);
            }
            else
            {
                throw oException;
            }
        }
        else
        {
            if (m_bClearTablesFirst)
            {
                NodeXLWorkbookUtil.ClearTables(m_oWorkbook);
            }

            // Populate the edge table with participant pairs.

            Debug.Assert(e.Result is EmailParticipantPair[]);

            EmailParticipantPair[] aoEmailParticipantPairs =
                ( EmailParticipantPair[] )e.Result;

            if (aoEmailParticipantPairs.Length > 0)
            {
                // Note: PopulateEdgesTable modifies the participant pairs.

                PopulateEdgesTable(aoEmailParticipantPairs);
            }
            else
            {
                this.ShowInformation("No such emails were found.");
            }
        }
    }

    //*************************************************************************
    //  Method: btnAnalyze_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnAnalyze button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnAnalyze_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if (!m_oEmailNetworkAnalyzer.IsBusy)
        {
            if ( DoDataExchange(true) )
            {
                StartAnalysis();
            }
        }
        else
        {
            // Request to cancel the analysis.  When the request is completed,
            // EmailNetworkAnalyzer_AnalysisCompleted() will be called.

            m_oEmailNetworkAnalyzer.CancelAsync();
        }

        EnableControls();
    }

    //*************************************************************************
    //  Method: EmailNetworkAnalyzer_AnalysisCompleted()
    //
    /// <summary>
    /// Handles the AnalysisCompleted event on the EmailNetworkAnalyzer object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    EmailNetworkAnalyzer_AnalysisCompleted
    (
        object sender,
        RunWorkerCompletedEventArgs e
    )
    {
        AssertValid();

        try
        {
            OnAnalysisCompleted(e);
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
        }
    }

    //*************************************************************************
    //  Method: lnkHelp_LinkClicked()
    //
    /// <summary>
    /// Handles the LinkClicked event on the lnkHelp LinkLabel.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    lnkHelp_LinkClicked
    (
        object sender,
        LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        this.ShowInformation(HelpMessage);
    }

    //*************************************************************************
    //  Method: lnkFolderHelp_LinkClicked()
    //
    /// <summary>
    /// Handles the LinkClicked event on the lnkFolderHelp LinkLabel.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    lnkFolderHelp_LinkClicked
    (
        object sender,
        LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        const String Message = 
            "If you use Windows Mail or Outlook Express to access your email,"
            + " you can usually enter a simple folder name — \"Inbox\","
            + " \"Sent Items\", or \"Deleted Items\", for example."
            + "\r\n\r\n"
            + "If you use Outlook and Exchange, you might have to enter a"
            + " folder name in this format:"
            + "\r\n\r\n"
            + "    /Mailbox - Jane Doe/Folder Name"
            + "\r\n\r\n"
            + "Here are some sample Outlook/Exchange folder names:"
            + "\r\n\r\n"
            + "    /Mailbox - Bill Smith/Inbox"
            + "\r\n"
            + "    /Mailbox - Mary Jones/Sent Items"
            + "\r\n"
            + "    /Mailbox - John Williams/Deleted Items"
            ;

        this.ShowInformation(Message);
    }

    //*************************************************************************
    //  Method: lnkEmailHelp_LinkClicked()
    //
    /// <summary>
    /// Handles the LinkClicked event on the lnkEmailHelp LinkLabel.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    lnkEmailHelp_LinkClicked
    (
        object sender,
        LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        const String Message = 
            "If you enter multiple email addresses, they are logically ORed"
            + " together.  For example, if you enter \"johndoe@msn.com\" and"
            + " \"marysmith@msn.com\" and then check their From checkboxes,"
            + " email that is from either John or Mary will be analyzed."
            + "\r\n\r\n"
            + "Your own email address is not automatically included in the"
            + " list.  If you want to analyze email you sent yourself, you"
            + " must enter your own email address."
            ;

        this.ShowInformation(Message);
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

        Debug.Assert(m_oAnalyzeEmailNetworkDialogUserSettings != null);
        Debug.Assert(m_oWorkbook != null);
        Debug.Assert(m_oEmailNetworkAnalyzer != null);
        // m_bClearTablesFirst
        // m_oEdgeTable
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Message to show when Windows Desktop Search can't be found.

    protected const String WdsConnectionFailureMessage =

        "This feature requires Windows Desktop Search, which doesn't appear to"
        + " be installed on this computer.  Windows Desktop Search is included"
        + " with Windows Vista, but it must be downloaded and installed on"
        + " Windows XP machines.  You can find more information at"
        + " http://www.microsoft.com/windows/products/winfamily/desktopsearch/"
        + "default.mspx."
        ;

    /// Message explaining what this dialog does.

    protected const String HelpMessage =

        "If your computer has Windows Desktop Search and you use Outlook,"
        + " Outlook Express, or Windows Mail, you can use this feature to"
        + " analyze the relationships between the people you communicate with"
        + " via email.  (Windows Desktop Search is included with Windows"
        + " Vista.  It can also be downloaded and installed on Windows XP.)"
        + "\r\n\r\n"
        + "You can analyze all your emails or specify a filtered subset.  For"
        + " each analyzed email, an \"edge weight\" of one is assigned to each"
        + " From/To email address pair.  For example, if an email has these"
        + " lines:"
        + "\r\n\r\n"
        + "From: john@msn.com"
        + "\r\n"
        + "To: mary@msn.com, bob@msn.com"
        + "\r\n"
        + "Cc: sarah@msn.com"
        + "\r\n"
        + "Bcc: bill@msn.com"
        + "\r\n\r\n"
        + "then an edge weight of 1 is assigned to these email address pairs:"
        + "\r\n\r\n"
        + "john@msn.com, mary@msn.com"
        + "\r\n"
        + "john@msn.com, bob@msn.com"
        + "\r\n\r\n"
        + "If the \"Use Cc line when calculating edge weights\" checkbox is"
        + " checked, an edge weight of 1 is also assigned to this pair:"
        + "\r\n\r\n"
        + "john@msn.com, sarah@msn.com"
        + "\r\n\r\n"
        + "And if the \"Use Bcc line when calculating edge weights\" checkbox"
        + " is checked, an edge weight of 1 is also assigned to this pair:"
        + "\r\n\r\n"
        + "john@msn.com, bill@msn.com"
        + "\r\n\r\n"
        + "This is repeated for each email, and the edge weights for"
        + " repeated pairs are added together.  The results are then written"
        + " to the Edges worksheet."
        + "\r\n\r\n"
        + "Note that \"john@msn.com, mary@msn.com\" and \"mary@msn.com,"
        + " john@msn.com\" are considered to be repeated pairs and get added"
        + " together.  If John writes to Mary once and Mary writes to John"
        + " once, the \"john@msn.com, mary@msn.com\" pair will have an edge"
        + " weight of 2.  In the Edges worksheet, the email addresses within"
        + " each pair are ordered alphabetically -- John in the first column,"
        + " in this case."
        + "\r\n\r\n"
        + "If you read the workbook into the NodeXL graph, you'll see a"
        + " vertex representing you, a vertex representing each person you"
        + " have communicated with, and an edge connecting you to each person."
        + "  You can filter the Edges worksheet on the edge weight column to"
        + " show only those people with whom you communicate often, or use"
        + " Excel formulas to vary edge or vertex attributes (Color, Width,"
        + " Shape, etc.) based on edge weight."
        ;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected AnalyzeEmailNetworkDialogUserSettings
        m_oAnalyzeEmailNetworkDialogUserSettings;

    /// Workbook containing the graph data.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// true if the NodeXL tables should be cleared first.

    protected Boolean m_bClearTablesFirst;

    /// Object that does most of the work.

    protected EmailNetworkAnalyzer m_oEmailNetworkAnalyzer;

    /// Edge table, or null if the edge table couldn't be obtained.

    protected ListObject m_oEdgeTable;
}

}
