

//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.SocialNetworkLib;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AnalyzeEmailNetworkDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="AnalyzeEmailNetworkDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("AnalyzeEmailNetworkDialog2") ]

public class AnalyzeEmailNetworkDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: AnalyzeEmailNetworkDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
	/// cref="AnalyzeEmailNetworkDialogUserSettings" /> class.
    /// </summary>
	///
	/// <param name="oForm">
	/// The form to save settings for.
	/// </param>
    //*************************************************************************

    public AnalyzeEmailNetworkDialogUserSettings
	(
		Form oForm
	)
	: base (oForm, false)
    {
		Debug.Assert(oForm != null);

		// (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Property: AnalyzeAllEmail
    //
    /// <summary>
    /// Gets or sets a flag indicating whether all email should be analyzed.
    /// </summary>
    ///
    /// <value>
	/// true to analyze all email, false to analyze filtered email.  The
	/// default is true.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("true") ]

    public Boolean
    AnalyzeAllEmail
    {
        get
        {
            AssertValid();

			Boolean bAnalyzeAllEmail = (Boolean)this[AnalyzeAllEmailKey];

			return (bAnalyzeAllEmail);
        }

        set
        {
            this[AnalyzeAllEmailKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ParticipantsCriteria
    //
    /// <summary>
    /// Gets or sets the search criteria for zero or more participants.
    /// </summary>
    ///
    /// <value>
    /// The search criteria for zero or more participants, as an array of <see
	/// cref="EmailParticipantCriteria" /> objects, or null to not filter on
	/// participants.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute(null) ]

    public EmailParticipantCriteria []
    ParticipantsCriteria
    {
        get
        {
            AssertValid();

			return ( ( EmailParticipantCriteria [] )
                this[ParticipantsCriteriaKey] );
        }

        set
        {
            this[ParticipantsCriteriaKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: StartTime
    //
    /// <summary>
    /// Gets or sets the start time.
    /// </summary>
    ///
    /// <value>
	/// The start time, or null to not filter on the start time.  The default
	/// is null.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute(null) ]

    public Nullable<DateTime>
    StartTime
    {
        get
        {
            AssertValid();

            return ( (Nullable<DateTime>)this[StartTimeKey] );
        }

        set
        {
            this[StartTimeKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EndTime
    //
    /// <summary>
    /// Gets or sets the end time.
    /// </summary>
    ///
    /// <value>
	/// The end time, or null to not filter on the end time.  The default is
	/// null.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute(null) ]

    public Nullable<DateTime>
    EndTime
    {
        get
        {
            AssertValid();

            return ( (Nullable<DateTime>)this[EndTimeKey] );
        }

        set
        {
            this[EndTimeKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Folder
    //
    /// <summary>
    /// Gets or sets the folder.
    /// </summary>
    ///
    /// <value>
	/// The folder, or null to not filter on the folder.  The default is null.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute(null) ]

    public String
    Folder
    {
        get
        {
            AssertValid();

			return ( (String)this[FolderKey] );
        }

        set
        {
            this[FolderKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: MinimumSize
    //
    /// <summary>
    /// Gets or sets the minimum size.
    /// </summary>
    ///
    /// <value>
	/// The minimum size, or null to not filter on the minimum size.  The
	/// default is null.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute(null) ]

    public Nullable<Int64>
    MinimumSize
    {
        get
        {
            AssertValid();

            return ( ( Nullable<Int64> )this[MinimumSizeKey] );
        }

        set
        {
            this[MinimumSizeKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: MaximumSize
    //
    /// <summary>
    /// Gets or sets the maximum size.
    /// </summary>
    ///
    /// <value>
	/// The maximum size, or null to not filter on the maximum size.  The
	/// default is null.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute(null) ]

    public Nullable<Int64>
    MaximumSize
    {
        get
        {
            AssertValid();

            return ( ( Nullable<Int64> )this[MaximumSizeKey] );
        }

        set
        {
            this[MaximumSizeKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: HasCc
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the email must have a Cc.
    /// </summary>
    ///
    /// <value>
	/// true if the email must have a Cc, false if the email must not have a
	/// Cc, or null to not filter on the Cc.  The default is null.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute(null) ]

    public Nullable<Boolean>
    HasCc
    {
        get
        {
            AssertValid();

			return ( ( Nullable<Boolean> )this[HasCcKey] );
        }

        set
        {
            this[HasCcKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: HasBcc
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the email must have a Bcc.
    /// </summary>
    ///
    /// <value>
	/// true if the email must have a Bcc, false if the email must not have a
	/// Bcc, or null to not filter on the Bcc.  The default is null.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute(null) ]

    public Nullable<Boolean>
    HasBcc
    {
        get
        {
            AssertValid();

			return ( ( Nullable<Boolean> )this[HasBccKey] );
        }

        set
        {
            this[HasBccKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: BodyText
    //
    /// <summary>
    /// Gets or sets the body text.
    /// </summary>
    ///
    /// <value>
	/// The body text, or null to not filter on body text.  The default is
	/// null.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute(null) ]

    public String
    BodyText
    {
        get
        {
            AssertValid();

			return ( (String)this[BodyTextKey] );
        }

        set
        {
            this[BodyTextKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: AttachmentFilter
    //
    /// <summary>
    /// Gets or sets the a value indicating how emails are filtered on
	/// attachments.
    /// </summary>
    ///
    /// <value>
	/// An AttachmentFilter value, or null to not filter on attachments.  The
	/// default is null.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute(null) ]

    public Nullable<AttachmentFilter>
    AttachmentFilter
    {
        get
        {
            AssertValid();

			return ( ( Nullable<AttachmentFilter> )this[AttachmentFilterKey] );
        }

        set
        {
            this[AttachmentFilterKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: UseCcForTieStrengths
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the Cc line should be used when
	/// calculating tie strengths.
    /// </summary>
    ///
    /// <value>
    /// true if the Cc line should be used when calculating tie strengths.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("true") ]

    public Boolean
    UseCcForTieStrengths
    {
        get
        {
            AssertValid();

			return ( (Boolean)this[UseCcForTieStrengthsKey] );
        }

        set
        {
            this[UseCcForTieStrengthsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: UseBccForTieStrengths
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the Bcc line should be used when
	/// calculating tie strengths.
    /// </summary>
    ///
    /// <value>
    /// true if the Bcc line should be used when calculating tie strengths.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("false") ]

    public Boolean
    UseBccForTieStrengths
    {
        get
        {
            AssertValid();

			return ( (Boolean)this[UseBccForTieStrengthsKey] );
        }

        set
        {
            this[UseBccForTieStrengthsKey] = value;

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

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
		base.AssertValid();

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Name of the settings key for the AnalyzeAllEmail property.

	protected const String AnalyzeAllEmailKey = "AnalyzeAllEmail";

	/// Name of the settings key for the ParticipantsCriteria property.

	protected const String ParticipantsCriteriaKey = "ParticipantsCriteria";

	/// Name of the settings key for the StartTime property.

	protected const String StartTimeKey = "StartTime";

	/// Name of the settings key for the EndTime property.

	protected const String EndTimeKey = "EndTime";

	/// Name of the settings key for the Folder property.

	protected const String FolderKey = "Folder";

	/// Name of the settings key for the MinimumSize property.

	protected const String MinimumSizeKey = "MinimumSize";

	/// Name of the settings key for the MaximumSize property.

	protected const String MaximumSizeKey = "MaximumSize";

	/// Name of the settings key for the HasCc property.

	protected const String HasCcKey = "HasCc";

	/// Name of the settings key for the HasBcc property.

	protected const String HasBccKey = "HasBcc";

	/// Name of the settings key for the BodyText property.

	protected const String BodyTextKey = "BodyText";

	/// Name of the settings key for the AttachmentFilter property.

	protected const String AttachmentFilterKey = "AttachmentFilter";

	/// Name of the settings key for the UseCcForTieStrengths property.

	protected const String UseCcForTieStrengthsKey = "UseCcForTieStrengths";

	/// Name of the settings key for the UseBccForTieStrengths property.

	protected const String UseBccForTieStrengthsKey = "UseBccForTieStrengths";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	// (None.)
}

}
