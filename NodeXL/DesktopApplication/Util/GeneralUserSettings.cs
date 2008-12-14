
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.ApplicationUtil;
using Microsoft.NodeXL.Visualization;

namespace Microsoft.NodeXL.DesktopApplication
{
//*****************************************************************************
//  Class: GeneralUserSettings
//
/// <summary>
/// Stores the user's general settings for the application.
/// </summary>
///
/// <remarks>
/// The base class automatically saves the window state and rectangle of the
/// application's main form when the application closes, and restores them when
/// the application starts.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("General") ]

public class GeneralUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: GeneralUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the GeneralUserSettings class.
    /// </summary>
	///
	/// <param name="oMainForm">
	/// The application's main form.
	/// </param>
    //*************************************************************************

    public GeneralUserSettings
	(
		Form oMainForm
	)
	: base(oMainForm, true)
    {
		Debug.Assert(oMainForm != null);

		// (Do nothing.)

		AssertValid();
    }

    //*************************************************************************
    //  Property: DocumentMruList
    //
    /// <summary>
    /// Gets or sets the file names of the most recently used documents.
    /// </summary>
    ///
    /// <value>
	/// A <see cref="StringMruList" /> object that contains the file names of
	/// the most recently used documents.  The file names include full paths.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("") ]

    public StringMruList
	DocumentMruList
    {
        get
        {
            AssertValid();

            StringMruList oDocumentMruList =
				(StringMruList)this[DocumentMruListKey];

            Debug.Assert(oDocumentMruList != null);

            return (oDocumentMruList);
        }

        set
        {
			Debug.Assert(value != null);

            this[DocumentMruListKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Layout
    //
    /// <summary>
    /// Gets or sets the layout to use when creating a <see cref="View" />.
    /// </summary>
    ///
    /// <value>
	/// The layout to use when creating a <see cref="View" />, as a <see
	/// cref="LayoutType" />.
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("FruchtermanReingold") ]

    public LayoutType
	Layout
    {
        get
        {
            AssertValid();

            LayoutType eLayout = (LayoutType)this[LayoutKey];

            return (eLayout);
        }

        set
        {
            this[LayoutKey] = value;

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

	/// Name of the settings key for the DocumentMruList property.

	protected const String DocumentMruListKey =
		"DocumentMruList";

	/// Name of the settings key for the Layout property.

	protected const String LayoutKey =
		"Layout";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
