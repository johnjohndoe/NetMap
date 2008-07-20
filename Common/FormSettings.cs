
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Configuration;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: FormSettings
//
/// <summary>
/// Saves a form's window state and rectangle in the user's settings.
/// </summary>
///
/// <remarks>
/// This can be used as a base class for storing user settings in a Windows
/// Forms application.  This class automatically saves the window state,
/// location, and optionally size (if the form is resizable) of a specified
/// form when the form closes, and restores them when the form is loaded.
///
/// <para>
/// Derive a class from this one, add any form-specific user setting properties
/// to the derived class, instantiate the dervied class in the form's
/// constructor, and save the instantiated object in a member field.
/// </para>
///
/// <para>
/// It is not necessary to call <see cref="ApplicationSettingsBase.Save" />
/// when the form is closing.  This class does that automatically.
/// </para>
///
/// <para>
/// This is for .NET 2.0 and later applications only.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class FormSettings : ApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: FormSettings()
    //
    /// <summary>
    /// Initializes a new instance of the FormSettings class.
    /// </summary>
	///
	/// <param name="oForm">
	/// The form to save settings for.
	/// </param>
	///
	/// <param name="bFormIsResizable">
	/// true if <paramref name="oForm" /> is resizable.  If true, the form's
	/// size is restored along with the form's window state and location.
	/// </param>
    //*************************************************************************

    public FormSettings
	(
		Form oForm,
        Boolean bFormIsResizable
	)
    {
		Debug.Assert(oForm != null);

		// Note: Although it would be possible to determine whether the form is
		// resizable by looking at the form's FormBorderStyle property, that
		// would require that the form's Initialize() method be called before
		// this constructor is called.  It's more reliable to require a
		// constructor parameter for this.

		m_bFormIsResizable = bFormIsResizable;

		// Subscribe to some form events.

		oForm.Load += new EventHandler(this.Form_Load);

		oForm.FormClosing += new FormClosingEventHandler(
			this.Form_FormClosing);

		// AssertValid();
    }

    //*************************************************************************
    //  Property: FormWindowState
    //
    /// <summary>
    /// Gets or sets the WindowState value to use for the form when the form
	/// loads.
    /// </summary>
    ///
    /// <value>
	/// <see cref="FormWindowState" /> value to use for the form.  Can't be
	/// Minimized.  The default is Normal.
    /// </value>
	///
	/// <remarks>
	/// Don't set this to Minimized.  Loading the form in a minimized state
	/// would confuse the user.
	/// </remarks>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("Normal") ]

    public FormWindowState
	FormWindowState
    {
        get
        {
            AssertValid();

            return ( (FormWindowState)this[FormWindowStateKey] );
        }

        set
        {
			ArgumentChecker oArgumentChecker = this.ArgumentChecker;
			const String PropertyName = "FormWindowState";

			oArgumentChecker.CheckPropertyIsDefined( PropertyName, value,
				typeof(FormWindowState) );

			oArgumentChecker.CheckPropertyNotEqual(PropertyName, (Int32)value,
				(Int32)FormWindowState.Minimized);

            this[FormWindowStateKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FormSize
    //
    /// <summary>
    /// Gets or sets the Size value to use for the form when the form loads.
    /// </summary>
    ///
    /// <value>
	/// Size of the form, as a <see cref="Size" />.  The width and height must
	/// be greater than 0.  The default is (600, 400).
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("600, 400") ]

    public Size
	FormSize
    {
        get
        {
            AssertValid();

            return ( (Size)this[FormSizeKey] );
        }

        set
        {
			ArgumentChecker oArgumentChecker = this.ArgumentChecker;

			oArgumentChecker.CheckPropertyPositive("FormSize.Width",
				value.Width);

			oArgumentChecker.CheckPropertyPositive("FormSize.Height",
				value.Height);

            this[FormSizeKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FormLocation
    //
    /// <summary>
    /// Gets or sets the Location value to use for the form when the form
	/// loads.
    /// </summary>
    ///
    /// <value>
	/// Location of the form, as a <see cref="Point" />.  The default is
	/// (0, 0).
    /// </value>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("0, 0") ]

    public Point
	FormLocation
    {
        get
        {
            AssertValid();

            return ( (Point)this[FormLocationKey] );
        }

        set
        {
            this[FormLocationKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: HasBeenSaved
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the settings have ever been
	/// saved.
    /// </summary>
    ///
    /// <value>
	/// true if the settings in this class have been saved at least once via
	/// <see cref="ApplicationSettingsBase.Save" />.
    /// </value>
	///
	/// <remarks>
	/// If false is returned, it means that all other properties will return
	/// default values instead of values read from a settings file.
	/// </remarks>
    //*************************************************************************

	[ UserScopedSettingAttribute() ]
	[ DefaultSettingValueAttribute("false") ]

    public Boolean
	HasBeenSaved
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[HasBeenSavedKey] );
        }

        set
        {
            this[HasBeenSavedKey] = value;

            AssertValid();
        }
    }

	//*************************************************************************
	//	Method: RestoreFormWindow()
	//
	/// <summary>
	/// Restores the form's window state and rectangle.
	/// </summary>
	///
	/// <param name="oForm">
	/// The form to restore.
	/// </param>
	//*************************************************************************

	protected void
	RestoreFormWindow
	(
		Form oForm
	)
	{
		AssertValid();

		Debug.Assert(oForm != null);

		// Avoid flashing.

		oForm.Visible = false;

		// Get some saved window parameters.

		FormWindowState eFormWindowState = this.FormWindowState;
		Size oFormSize = this.FormSize;
		Point oFormLocation = this.FormLocation;

		if (eFormWindowState == FormWindowState.Normal)
		{
            Rectangle oSavedRectangle =
				new Rectangle(oFormLocation, oFormSize);

			if ( !Screen.PrimaryScreen.WorkingArea.Contains(oSavedRectangle) )
			{
				// The saved rectangle isn't within the screen bounds.  Don't
				// use the saved window parameters.  (This can happen in 
				// multiscreen systems or when the user changes display
				// resolutions.)

				return;
			}
		}

		// Restore the window state.

		oForm.WindowState = eFormWindowState;
		oForm.StartPosition = FormStartPosition.Manual;

		if (m_bFormIsResizable)
			oForm.Size = oFormSize;

		oForm.Location = oFormLocation;

		oForm.Visible = true;
	}

	//*************************************************************************
	//	Method: SaveFormWindow()
	//
	/// <summary>
	/// Saves the form's window state and rectangle.
	/// </summary>
	///
	/// <param name="oForm">
	/// The form whose window state and rectangle should be saved.
	/// </param>
	//*************************************************************************

	protected void
	SaveFormWindow
	(
		Form oForm
	)
	{
		AssertValid();

		Debug.Assert(oForm != null);

		// This method needs to update and save the FormWindowState, FormSize,
		// and FormLocation properties of this class.

		FormWindowState eWindowStateToSave = FormWindowState.Normal;
		Rectangle oRectangleToSave = Rectangle.Empty;

		switch (oForm.WindowState)
		{
			case FormWindowState.Normal:

				// Save the form's current bounds.

				oRectangleToSave = oForm.Bounds;

				break;

			case FormWindowState.Minimized:

				// Don't let the window be minimized when the form is reloaded.
				// Make it Normal instead.

				oRectangleToSave = oForm.RestoreBounds;

				break;

			case FormWindowState.Maximized:

				eWindowStateToSave = FormWindowState.Maximized;
				oRectangleToSave = oForm.RestoreBounds;

				break;

			default:

				Debug.Assert(false);
				break;
		}

		// Update the properties.

		this.FormWindowState = eWindowStateToSave;
		this.FormSize = oRectangleToSave.Size;
		this.FormLocation = oRectangleToSave.Location;
	}

	//*************************************************************************
	//	Method: Form_Load()
	//
	/// <summary>
	///	Handles the Load event on the form that owns this object.
	/// </summary>
	///
	/// <param name="sender">
	///	Standard event argument.
	/// </param>
	///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
	//*************************************************************************

    protected void
	Form_Load
	(
		object sender,
		EventArgs e
	)
    {
		AssertValid();
		Debug.Assert(sender is Form);

		Form oForm = (Form)sender;

		// If the settings in this class have been saved at least once, use them
		// to restore the form's window state and rectangle.  Otherwise, this
		// is the first time the form has been opened and the Windows Forms
		// default settings for the window state and rectange should be used.

		if (this.HasBeenSaved)
			RestoreFormWindow(oForm);
    }

	//*************************************************************************
	//	Method: Form_FormClosing()
	//
	/// <summary>
	///	Handles the FormClosing event on the form that owns this object.
	/// </summary>
	///
	/// <param name="sender">
	///	Standard event argument.
	/// </param>
	///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
	//*************************************************************************

    protected void
	Form_FormClosing
	(
		object sender,
		FormClosingEventArgs e
	)
    {
		AssertValid();
		Debug.Assert(sender is Form);

		Form oForm = (Form)sender;

		// Save the form's window state and rectangle.

		SaveFormWindow(oForm);

		// Indicate that the settings have been saved.

		this.HasBeenSaved = true;

		// Save all properties.

		this.Save();
    }

	//*************************************************************************
	//	Property: ArgumentChecker
	//
	/// <summary>
	/// Gets a new initialized ArgumentChecker object.
	/// </summary>
	///
	/// <value>
	/// A new initialized ArgumentChecker object.
	/// </value>
	//*************************************************************************

	private ArgumentChecker
	ArgumentChecker
	{
		get
		{
			AssertValid();

			return ( new ArgumentChecker("FormSettings") );
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

    public virtual void
    AssertValid()
    {
		// m_bFormIsResizable
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

	/// Name of the settings key for the FormWindowState property.

	protected const String FormWindowStateKey = "FormWindowState";

	/// Name of the settings key for the FormSize property.

	protected const String FormSizeKey = "FormSize";

	/// Name of the settings key for the FormLocation property.

	protected const String FormLocationKey = "FormLocation";

	/// Name of the settings key for the HasBeenSaved property.

	protected const String HasBeenSavedKey = "HasBeenSaved";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// true if the Form passed to the constructor is resizable.

	protected Boolean m_bFormIsResizable;
}

}
