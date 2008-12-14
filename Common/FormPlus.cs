
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//	Class: FormPlus
//
/// <summary>
/// Represents a Form with additional features.
/// </summary>
///
/// <remarks>
/// This class contains a set of methods for displaying messages and validating
/// controls.  It also implements an <see cref="OnFirstActivated" /> virtual
/// method that the derived class can use to run time-consuming initialization
/// code without preventing the form from appearing.
///
/// <para>
/// Most of the functionality in this class is also avaialable as a set of
/// static methods in the <see cref="FormUtil" /> class.
/// </para>
///
/// </remarks>
///
/// <seealso cref="FormUtil" />
//*****************************************************************************

public class FormPlus : System.Windows.Forms.Form
{
	//*************************************************************************
	//	Constructor: FormPlus()
	//
	/// <summary>
	///	Initializes a new instance of the FormPlus class.
	/// </summary>
	//*************************************************************************

	public FormPlus()
	{
		m_bActivated = false;
	}

	//*************************************************************************
	//	Property: ApplicationName
	//
	/// <summary>
	///	Gets or sets the name of the application.
	/// </summary>
	///
	/// <value>
	/// The name of the application.  Can't be null, but can be empty.
	/// </value>
	///
	/// <remarks>
	/// The application name is used in the message boxes opened by this
	/// class's methods.
	///
	/// <para>
	/// By default, the application name is obtained from the
	/// "assembly: AssemblyProduct" attribute in the application's
	/// AssemblyInfo.cs file.  If you want to use a different application name,
	/// set this property in the form's constructor.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public String
	ApplicationName
	{
		get
		{
			// This may be called from the form's constructor, so don't do
			// this:
			//
			// AssertValid();

			return (FormUtil.ApplicationName);
		}

		set
		{
			Debug.Assert(value != null);

			FormUtil.ApplicationName = value;
		}
	}

	//*************************************************************************
	//	Method: ShowWarning()
	//
	/// <summary>
	///	Displays text in a warning message box.
	/// </summary>
	///
	/// <param name="sText">
	///	Text to show.
	/// </param>
	//*************************************************************************

	protected internal void
	ShowWarning
	(
		String sText
	)
	{
		Debug.Assert(sText != null && sText != "");
		AssertValid();

		FormUtil.ShowWarning(sText);
	}

	//*************************************************************************
	//	Method: ShowInformation()
	//
	/// <summary>
	///	Displays text in an information message box.
	/// </summary>
	///
	/// <param name="sText">
	///	Text to show.
	/// </param>
	//*************************************************************************

	protected internal void
	ShowInformation
	(
		String sText
	)
	{
		Debug.Assert(sText != null && sText != "");
		AssertValid();

		FormUtil.ShowInformation(sText);
	}

	//*************************************************************************
	//	Method: ShowError()
	//
	/// <summary>
	///	Displays text in an error message box.
	/// </summary>
	///
	/// <param name="sText">
	///	Text to show.
	/// </param>
	//*************************************************************************

	protected internal void
	ShowError
	(
		String sText
	)
	{
		Debug.Assert(sText != null && sText != "");
		AssertValid();

		FormUtil.ShowError(sText);
	}

	//*************************************************************************
	//	Method: EnableControls()
	//
	/// <summary>
	///	Enables or disables the controls in a collection.
	/// </summary>
	///
	/// <param name="bEnable">
	/// true to enable.
	/// </param>
	///
	/// <param name="oControls">
	/// The controls to enable or disable.
	/// </param>
	//*************************************************************************

	protected void
	EnableControls
	(
		Boolean bEnable,
		Control.ControlCollection oControls
	)
	{
		FormUtil.EnableControls(bEnable, oControls);
	}

	//*************************************************************************
	//	Method: EnableControls()
	//
	/// <summary>
	///	Enables or disables a set of controls.
	/// </summary>
	///
	/// <param name="bEnable">
	/// true to enable.
	/// </param>
	///
	/// <param name="aoControls">
	/// The controls to enable or disable.
	/// </param>
	//*************************************************************************

	protected void
	EnableControls
	(
		Boolean bEnable,
		params Control[] aoControls
	)
	{
		FormUtil.EnableControls(bEnable, aoControls);
	}

	//*************************************************************************
	//	Method: FindControlByName()
	//
	/// <summary>
	///	Finds a control on the form.
	/// </summary>
	///
	/// <param name="sName">
	/// Value of the control's Name property.
	/// </param>
	///
	/// <returns>
	/// The specified control.
	/// </returns>
	///
	/// <remarks>
	/// This method searches for the specified control by recursing through the
	/// form's child controls.  If the control is not found, an exception is
	/// thrown.
	/// </remarks>
	//*************************************************************************

	public Control
	FindControlByName
	(
		String sName
	)
	{
		Debug.Assert(sName != null);
		Debug.Assert(sName.Length > 0);

		Control oControl;

		if ( !FindControlByName(sName, this.Controls, out oControl) )
		{
			throw new Exception( String.Format(
				"FormPlus.FindControlByName: {0} could not be found.",
				sName
				) );
		}

		return (oControl);
	}

	//*************************************************************************
	//	Method: FindControlByName()
	//
	/// <summary>
	///	Finds a control on the form.
	/// </summary>
	///
	/// <param name="sName">
	/// Value of the control's Name property.
	/// </param>
	///
	/// <param name="oChildControls">
	/// Control collection to recursively search.
	/// </param>
	///
	/// <param name="oControl">
	/// Where the found control gets stored.
	/// </param>
	///
	/// <returns>
	/// true if the specified control is found.
	/// </returns>
	///
	/// <remarks>
	/// This method searches for the specified control by recursing through
	/// <paramref name="oChildControls" />.  If the control is found, it's
	/// stored at <paramref name="oControl" /> and true is returned.
	/// Otherwise, false is returned.
	/// </remarks>
	//*************************************************************************

	protected Boolean
	FindControlByName
	(
		String sName,
		Control.ControlCollection oChildControls,
		out Control oControl
	)
	{
		Debug.Assert(sName != null);
		Debug.Assert(sName.Length > 0);
		Debug.Assert(oChildControls != null);

		oControl = null;

		// Loop through the child controls.

		foreach (Control oChildControl in oChildControls)
		{
			if (oChildControl.Name == sName)
			{
				oControl = oChildControl;
				return (true);
			}

			// Recurse.

			if ( FindControlByName(
				sName, oChildControl.Controls, out oControl) )
			{
				return (true);
			}
		}

		return (false);
	}

	//*************************************************************************
	//	Method: EnsureVisible()
	//
	/// <summary>
	/// Ensures that the form is within the working area of the screen.
	/// </summary>
	///
	/// <remarks>
	/// This method resizes the form if it is wider or taller than the screen's
	/// working area, and moves the form if an edge is outside the screen's
	/// working area.
	/// </remarks>
	//*************************************************************************

	protected void
	EnsureVisible()
	{
		Rectangle oWorkingArea = Screen.PrimaryScreen.WorkingArea;

		this.Height = Math.Min(this.Height, oWorkingArea.Height);
		this.Width = Math.Min(this.Width, oWorkingArea.Width);

		this.Top = Math.Max(this.Top, oWorkingArea.Top);
		this.Left = Math.Max(this.Left, oWorkingArea.Left);

		if (this.Right > oWorkingArea.Right)
			this.Left = oWorkingArea.Right - this.Width;

		if (this.Bottom > oWorkingArea.Bottom)
			this.Top = oWorkingArea.Bottom - this.Height;
	}

	//*************************************************************************
	//	Method: TrimTextBox()
	//
	/// <summary>
	/// Removes leading and trailing spaces from a TextBox.
	/// </summary>
	///
	/// <param name="oTextBox">
	///	TextBox to remove spaces from.
	/// </param>
	///
	///	<returns>
	///	The trimmed TextBox text.
	///	</returns>
	///
	///	<remarks>
	/// This method does not perform any validation.  Use it on a TextBox that
	/// can contain an empty string.  To validate a TextBox that must contain
	/// text, use <see cref="ValidateRequiredTextBox" /> instead of this
	/// method.
	///	</remarks>
	//*************************************************************************

	protected internal String
	TrimTextBox
	(
		TextBox oTextBox
	)
	{
		Debug.Assert(oTextBox != null);
		AssertValid();

		return ( FormUtil.TrimTextBox(oTextBox) );
	}

	//*************************************************************************
	//	Method: ValidateRequiredTextBox()
	//
	/// <summary>
	///	Validates a TextBox that must contain text.
	/// </summary>
	///
	/// <param name="oTextBox">
	///	TextBox to validate.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display if validation fails.
	/// </param>
	///
	/// <param name="sTrimmedText">
	///	Where the trimmed text gets stored.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the TextBox text is empty after being trimmed, <paramref
	///	name="sErrorMessage" /> is displayed and false is returned.  Otherwise,
	///	the TextBox text is trimmed and true is returned.
	///	</remarks>
	//*************************************************************************

	protected internal Boolean
	ValidateRequiredTextBox
	(
		TextBox oTextBox,
		String sErrorMessage,
		out String sTrimmedText
	)
	{
		Debug.Assert(oTextBox != null);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");
		AssertValid();

		return ( FormUtil.ValidateRequiredTextBox(oTextBox, sErrorMessage,
			out sTrimmedText) );
	}

	//*************************************************************************
	//	Method: ValidateRequiredComboBox()
	//
	/// <summary>
	///	Validates a ComboBox that must contain text.
	/// </summary>
	///
	/// <param name="oComboBox">
	///	Control to validate.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display if validation fails.
	/// </param>
	///
	/// <param name="sTrimmedText">
	///	Where the trimmed text gets stored.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the ComboBox text is empty after being trimmed, <paramref
	///	name="sErrorMessage" /> is displayed and false is returned.  Otherwise,
	///	the control text is trimmed and true is returned.
	///	</remarks>
	//*************************************************************************

	protected internal Boolean
	ValidateRequiredComboBox
	(
		ComboBox oComboBox,
		String sErrorMessage,
		out String sTrimmedText
	)
	{
		Debug.Assert(oComboBox != null);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");
		AssertValid();

		return ( FormUtil.ValidateRequiredComboBox(oComboBox, sErrorMessage,
			out sTrimmedText) );
	}

	//*************************************************************************
	//	Method: ValidateInt32TextBox()
	//
	/// <summary>
	///	Validates a TextBox that must contain an Int32.
	/// </summary>
	///
	/// <param name="oTextBox">
	///	TextBox to validate.
	/// </param>
	///
	/// <param name="iMinValue">
	///	Minimum valid value.
	/// </param>
	///
	/// <param name="iMaxValue">
	///	Maximum valid value.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display if validation fails.
	/// </param>
	///
	/// <param name="iInt32">
	///	Where the validated Int32 gets stored.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the TextBox does not contain an Int32 within the specified range,
	///	<paramref name="sErrorMessage" /> is displayed and false is returned.
	///	Otherwise, the TextBox text is trimmed and true is returned.
	///	</remarks>
	//*************************************************************************

	protected internal Boolean
	ValidateInt32TextBox
	(
		TextBox oTextBox,
		Int32 iMinValue,
		Int32 iMaxValue,
		String sErrorMessage,
		out Int32 iInt32
	)
	{
		Debug.Assert(oTextBox != null);
		Debug.Assert(iMinValue <= iMaxValue);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");
		AssertValid();

		return ( FormUtil.ValidateInt32TextBox(oTextBox, iMinValue, iMaxValue,
			sErrorMessage, out iInt32) );
	}

	//*************************************************************************
	//	Method: ValidateDoubleTextBox()
	//
	/// <summary>
	///	Validates a TextBox that must contain a Double.
	/// </summary>
	///
	/// <param name="oTextBox">
	///	TextBox to validate.
	/// </param>
	///
	/// <param name="dMinValue">
	///	Minimum valid value.
	/// </param>
	///
	/// <param name="dMaxValue">
	///	Maximum valid value.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display if validation fails.
	/// </param>
	///
	/// <param name="dDouble">
	///	Where the validated Double gets stored.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the TextBox does not contain a Double within the specified range,
	///	<paramref name="sErrorMessage" /> is displayed and false is returned.
	///	Otherwise, the TextBox text is trimmed and true is returned.
	///	</remarks>
	//*************************************************************************

	protected internal Boolean
	ValidateDoubleTextBox
	(
		TextBox oTextBox,
		Double dMinValue,
		Double dMaxValue,
		String sErrorMessage,
		out Double dDouble
	)
	{
		Debug.Assert(oTextBox != null);
		Debug.Assert(dMinValue <= dMaxValue);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");
		AssertValid();

		return ( FormUtil.ValidateDoubleTextBox(oTextBox, dMinValue, dMaxValue,
			sErrorMessage, out dDouble) );
	}

	//*************************************************************************
	//	Method: ValidateDirectoryTextBox()
	//
	/// <summary>
	///	Validates a TextBox that must contain an existing directory.
	/// </summary>
	///
	/// <param name="oTextBox">
	///	TextBox to validate.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display if validation fails.
	/// </param>
	///
	/// <param name="sDirectory">
	///	Where the trimmed directory gets stored.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the TextBox does not contain an existing directory,
	///	<paramref name="sErrorMessage" /> is displayed and false is returned.
	///	Otherwise, the TextBox text is trimmed and true is returned.
	///	</remarks>
	//*************************************************************************

	protected internal Boolean
	ValidateDirectoryTextBox
	(
		TextBox oTextBox,
		String sErrorMessage,
		out String sDirectory
	)
	{
		Debug.Assert(oTextBox != null);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");
		AssertValid();

		return ( FormUtil.ValidateDirectoryTextBox(oTextBox, sErrorMessage,
			out sDirectory) );
	}

	//*************************************************************************
	//	Method: ValidateFileTextBox()
	//
	/// <summary>
	///	Validates a TextBox that must contain the name of an existing file.
	/// </summary>
	///
	/// <param name="oTextBox">
	///	TextBox to validate.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display if validation fails.
	/// </param>
	///
	/// <param name="sFile">
	///	Where the trimmed file name gets stored.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the TextBox does not contain the name of an existing file,
	///	<paramref name="sErrorMessage" /> is displayed and false is returned.
	///	Otherwise, the TextBox text is trimmed and true is returned.
	///	</remarks>
	//*************************************************************************

	protected internal Boolean
	ValidateFileTextBox
	(
		TextBox oTextBox,
		String sErrorMessage,
		out String sFile
	)
	{
		Debug.Assert(oTextBox != null);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");
		AssertValid();

		return ( FormUtil.ValidateFileTextBox(oTextBox, sErrorMessage,
			out sFile) );
	}

	//*************************************************************************
	//	Method: ValidateListBoxSelection()
	//
	/// <summary>
	///	Validates a ListBox that must have at least one item selected.
	/// </summary>
	///
	/// <param name="oListBox">
	///	ListBox to validate.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display if validation fails.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the ListBox does not have a selected item, <paramref
	///	name="sErrorMessage" /> is displayed and false is returned.  Otherwise,
	///	true is returned.
	///	</remarks>
	//*************************************************************************

	protected internal Boolean
	ValidateListBoxSelection
	(
		ListBox oListBox,
		String sErrorMessage
	)
	{
		Debug.Assert(oListBox != null);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");
		AssertValid();

		return ( FormUtil.ValidateListBoxSelection(oListBox, sErrorMessage) );
	}

	//*************************************************************************
	//	Method: ValidateNumericUpDown()
	//
	/// <summary>
	///	Validates a NumericUpDown control that contains a Decimal.
	/// </summary>
	///
	/// <param name="oNumericUpDown">
	///	NumericUpDown to validate.
	/// </param>
	///
	/// <param name="sValueDescription">
	///	Description of what the control contains, in lower case.  Sample:
	///	"a length".
	/// </param>
	///
	/// <param name="decValue">
	///	Where the validated value gets stored.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the NumericUpDown has empty Text or contains a number outside the
	/// control's Minimum and Maximum range, an error message is displayed and
	///	false is returned.  Otherwise, true is returned.
	///	</remarks>
	//*************************************************************************

	protected internal Boolean
	ValidateNumericUpDown
	(
		NumericUpDown oNumericUpDown,
		String sValueDescription,
		out Decimal decValue
	)
	{
		Debug.Assert(oNumericUpDown != null);
		Debug.Assert(sValueDescription != null && sValueDescription != "");

		return ( FormUtil.ValidateNumericUpDown(oNumericUpDown,
			sValueDescription, out decValue) );
	}

	//*************************************************************************
	//	Method: ValidateNumericUpDown()
	//
	/// <summary>
	///	Validates a NumericUpDown control that contains a Double.
	/// </summary>
	///
	/// <param name="oNumericUpDown">
	///	NumericUpDown to validate.
	/// </param>
	///
	/// <param name="sValueDescription">
	///	Description of what the control contains, in lower case.  Sample:
	///	"a length".
	/// </param>
	///
	/// <param name="dValue">
	///	Where the validated value gets stored.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the NumericUpDown has empty Text or contains a number outside the
	/// control's Minimum and Maximum range, an error message is displayed and
	///	false is returned.  Otherwise, true is returned.
	///	</remarks>
	//*************************************************************************

	protected internal Boolean
	ValidateNumericUpDown
	(
		NumericUpDown oNumericUpDown,
		String sValueDescription,
		out Double dValue
	)
	{
		Debug.Assert(oNumericUpDown != null);
		Debug.Assert(sValueDescription != null && sValueDescription != "");

		return ( FormUtil.ValidateNumericUpDown(oNumericUpDown,
			sValueDescription, out dValue) );
	}

	//*************************************************************************
	//	Method: ValidateNumericUpDown()
	//
	/// <summary>
	///	Validates a NumericUpDown control that contains a Single.
	/// </summary>
	///
	/// <param name="oNumericUpDown">
	///	NumericUpDown to validate.
	/// </param>
	///
	/// <param name="sValueDescription">
	///	Description of what the control contains, in lower case.  Sample:
	///	"a length".
	/// </param>
	///
	/// <param name="fValue">
	///	Where the validated value gets stored.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the NumericUpDown has empty Text or contains a number outside the
	/// control's Minimum and Maximum range, an error message is displayed and
	///	false is returned.  Otherwise, true is returned.
	///	</remarks>
	//*************************************************************************

	protected internal Boolean
	ValidateNumericUpDown
	(
		NumericUpDown oNumericUpDown,
		String sValueDescription,
		out Single fValue
	)
	{
		Debug.Assert(oNumericUpDown != null);
		Debug.Assert(sValueDescription != null && sValueDescription != "");

		return ( FormUtil.ValidateNumericUpDown(oNumericUpDown,
			sValueDescription, out fValue) );
	}

	//*************************************************************************
	//	Method: ValidateNumericUpDown()
	//
	/// <summary>
	///	Validates a NumericUpDown control that contains an Int32.
	/// </summary>
	///
	/// <param name="oNumericUpDown">
	///	NumericUpDown to validate.
	/// </param>
	///
	/// <param name="sValueDescription">
	///	Description of what the control contains, in lower case.  Sample:
	///	"a length".
	/// </param>
	///
	/// <param name="iValue">
	///	Where the validated value gets stored.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the NumericUpDown has empty Text or contains a number outside the
	/// control's Minimum and Maximum range, an error message is displayed and
	///	false is returned.  Otherwise, true is returned.
	///	</remarks>
	//*************************************************************************

	protected internal Boolean
	ValidateNumericUpDown
	(
		NumericUpDown oNumericUpDown,
		String sValueDescription,
		out Int32 iValue
	)
	{
		Debug.Assert(oNumericUpDown != null);
		Debug.Assert(sValueDescription != null && sValueDescription != "");

		return ( FormUtil.ValidateNumericUpDown(oNumericUpDown,
			sValueDescription, out iValue) );
	}

	//*************************************************************************
	//	Method: OnInvalidTextBox()
	//
	/// <summary>
	///	Handles a a TextBox that failed validation.
	/// </summary>
	///
	/// <param name="oTextBox">
	///	TextBox.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display.
	/// </param>
	///
	///	<returns>
	///	Always returns false.
	///	</returns>
	///
	///	<remarks>
	///	This method displays an error message, sets focus to the TextBox, and
	///	returns false.
	///	</remarks>
	//*************************************************************************

	protected internal Boolean
	OnInvalidTextBox
	(
		TextBox oTextBox,
		String sErrorMessage
	)
	{
		Debug.Assert(oTextBox != null);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");
		AssertValid();

		return ( FormUtil.OnInvalidTextBox(oTextBox, sErrorMessage) );
	}

	//*************************************************************************
	//	Method: OnInvalidComboBox()
	//
	/// <summary>
	///	Handles a a ComboBox that failed validation.
	/// </summary>
	///
	/// <param name="oComboBox">
	///	ComboBox.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display.
	/// </param>
	///
	///	<returns>
	///	Always returns false.
	///	</returns>
	///
	///	<remarks>
	///	This method displays an error message, sets focus to the ComboBox, and
	///	returns false.
	///	</remarks>
	//*************************************************************************

	protected internal Boolean
	OnInvalidComboBox
	(
		ComboBox oComboBox,
		String sErrorMessage
	)
	{
		Debug.Assert(oComboBox != null);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");
		AssertValid();

		return ( FormUtil.OnInvalidComboBox(oComboBox, sErrorMessage) );
	}

	//*************************************************************************
	//	Method: OnInvalidDateTimePicker()
	//
	/// <summary>
	///	Handles a a DateTimePicker that failed validation.
	/// </summary>
	///
	/// <param name="oDateTimePicker">
	///	DateTimePicker.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display.
	/// </param>
	///
	///	<returns>
	///	Always returns false.
	///	</returns>
	///
	///	<remarks>
	///	This method displays an error message, sets focus to the
	/// DateTimePicker, and returns false.
	///	</remarks>
	//*************************************************************************

	protected internal Boolean
	OnInvalidDateTimePicker
	(
		DateTimePicker oDateTimePicker,
		String sErrorMessage
	)
	{
		Debug.Assert(oDateTimePicker != null);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");
		AssertValid();

		return ( FormUtil.OnInvalidDateTimePicker(oDateTimePicker,
			sErrorMessage) );
	}

	//*************************************************************************
	//	Method: OnInvalidNumericUpDown()
	//
	/// <summary>
	///	Handles a a NumericUpDown that failed validation.
	/// </summary>
	///
	/// <param name="oNumericUpDown">
	///	NumericUpDown control.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display.
	/// </param>
	///
	///	<returns>
	///	Always returns false.
	///	</returns>
	///
	///	<remarks>
	///	This method displays an error message, sets focus to the NumericUpDown
	///	control, and returns false.
	///	</remarks>
	//*************************************************************************

	protected internal Boolean
	OnInvalidNumericUpDown
	(
		NumericUpDown oNumericUpDown,
		String sErrorMessage
	)
	{
		Debug.Assert(oNumericUpDown != null);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");
		AssertValid();

		return ( FormUtil.OnInvalidNumericUpDown(oNumericUpDown,
			sErrorMessage) );
	}

	//*************************************************************************
	//	Method: OnInvalidControl()
	//
	/// <summary>
	///	Handles a a control that failed validation.
	/// </summary>
	///
	/// <param name="oControl">
	///	Control.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display.
	/// </param>
	///
	///	<returns>
	///	Always returns false.
	///	</returns>
	///
	///	<remarks>
	///	This method displays an error message, sets focus to the control, and
	/// returns false.
	///
	/// <para>
	/// The method is private.  Derived classes should call one of the
	/// protected OnInvalidXXX methods that are specialized for specific
	/// control types.
	/// </para>
	///
	///	</remarks>
	//*************************************************************************

	private Boolean
	OnInvalidControl
	(
		Control oControl,
		String sErrorMessage
	)
	{
		Debug.Assert(oControl != null);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");
		AssertValid();

		return ( FormUtil.OnInvalidControl(oControl, sErrorMessage) );
	}

	//*************************************************************************
	//	Method: OnActivated()
	//
	/// <summary>
	///	Handles the Activated event.
	/// </summary>
	//*************************************************************************

	protected override void
	OnActivated
	(
	   EventArgs e
	)
	{
		base.OnActivated(e);

		if (!m_bActivated)
		{
			// OnActivated() may get called multiple times.  Don't let the
			// following code run more than once.

			m_bActivated = true;

			OnFirstActivated();
		}
	}

	//*************************************************************************
	//	Method: OnFirstActivated()
	//
	/// <summary>
	///	Gets called the first time the form is activated.
	/// </summary>
	///
	/// <remarks>
	/// Override this method to perform time-consuming initialization tasks
	/// without preventing the form from appearing.
	///
	/// <para>
	/// If a form executes time-consuming code in its constructor (such as
	/// getting data from a network server), the form doesn't appear until the
	/// code has completed, leaving the user with no idea what's going on.
	/// Waiting until the form is first activated to run lengthy code solves
	/// this, because the form is visible at that point.
	/// </para>
	///
	/// <para>
	/// Do not put time-consuming code in an OnLoad() handler, because the form
	/// isn't visible when OnLoad() runs.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	protected virtual void
	OnFirstActivated()
	{
		// (Do nothing.)
	}

	//*************************************************************************
	//	Method: EnsureVisibleOnVisibleChanged()
	//
	/// <summary>
	///	VisibleChanged handler that ensures that the form is not off the screen
	/// when it's made visible.
	/// </summary>
	///
	/// <param name="sender">
	/// Standard event argument.
	/// </param>
	///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
	///
	/// <remarks>
	/// If the form has a FormBorderStyle of SizableToolWindow, you should set
	/// the form's VisibleChanged handler to this method.  This is to work
	/// around an apparent bug in WinForms: If FormBorderStyle is
	/// SizableToolWindow, you can drag the window below the bottom of the
	/// screen, where it becomes invisible and impossible to retrieve.
	/// </remarks>
	//*************************************************************************

	protected void
	EnsureVisibleOnVisibleChanged
	(
		Object sender,
		EventArgs e
	)
	{
		if (this.Visible)
		{
			Debug.Assert(this.FormBorderStyle ==
				FormBorderStyle.SizableToolWindow);

			this.EnsureVisible();
		}
	}

	//*************************************************************************
	//	Method: PreventClosure()
	//
	/// <summary>
	/// Closing handler that prevents the form from closing.
	/// </summary>
	///
	/// <param name="sender">
	/// Standard event argument.
	/// </param>
	///
	/// <param name="e">
	/// Standard event argument.
	/// </param>
	///
	/// <remarks>
	/// If the form should be hidden rather than closed when the user clicks
	/// the X in the form's upper-right corner, set the form's Closing handler
	/// to this method.
	/// </remarks>
	//*************************************************************************

	protected void
	PreventClosure
	(
		Object sender,
		System.ComponentModel.CancelEventArgs e
	)
	{
		e.Cancel = true;
		this.Hide();
	}

	//*************************************************************************
	//	Method: AssertValid()
	//
	/// <summary>
	///	Asserts if the object is in an invalid state.  Debug-only.
	/// </summary>
	//*************************************************************************

	[Conditional("DEBUG")] 

	public virtual void
	AssertValid()
	{
		// m_bActivated
	}


	//*************************************************************************
	//	Protected fields
	//*************************************************************************

	/// true if OnActivated() has been called at least once.

	protected Boolean m_bActivated;
}


//*****************************************************************************
//	Class: FormUtil
//
/// <summary>
/// Form utility methods.
/// </summary>
///
/// <remarks>
/// This class contains a set of static methods for displaying messages and
/// validating controls.  It is meant for use by <see cref="Form" /> and <see
/// cref="UserControl" /> classes.
///
/// <para>
/// Most of the functionality in this class is also in the <see
/// cref="FormPlus" /> class, which can serve as a base class for derived
/// forms.
/// </para>
///
/// </remarks>
///
/// <seealso cref="FormPlus" />
//*****************************************************************************

public class FormUtil : Object
{
	//*************************************************************************
	//	Constructor: FormUtil()
	//
	/// <summary>
	///	Static constructor for the FormUtil class.
	/// </summary>
	//*************************************************************************

	static FormUtil()
	{
		m_sApplicationName = Application.ProductName;
	}

	//*************************************************************************
	//	Property: ApplicationName
	//
	/// <summary>
	///	Gets or sets the name of the application.
	/// </summary>
	///
	/// <value>
	/// The name of the application.  Can't be null, but can be empty.
	/// </value>
	///
	/// <remarks>
	/// The application name is used in the message boxes opened by this
	/// class's methods.
	///
	/// <para>
	/// By default, the application name is obtained from the
	/// "assembly: AssemblyProduct" attribute in the application's
	/// AssemblyInfo.cs file.  If you want to use a different application name,
	/// set this property before using any other class methods.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public static String
	ApplicationName
	{
		get
		{
			return (m_sApplicationName);
		}

		set
		{
			Debug.Assert(value != null);

			m_sApplicationName = value;
		}
	}

	//*************************************************************************
	//	Method: ShowWarning()
	//
	/// <summary>
	///	Displays text in a warning message box.
	/// </summary>
	///
	/// <param name="sText">
	///	Text to show.
	/// </param>
	//*************************************************************************

	public static void
	ShowWarning
	(
		String sText
	)
	{
		Debug.Assert(sText != null && sText != "");

		MessageBox.Show(sText, ApplicationName, MessageBoxButtons.OK,
			MessageBoxIcon.Warning);
	}

	//*************************************************************************
	//	Method: ShowInformation()
	//
	/// <summary>
	///	Displays text in an information message box.
	/// </summary>
	///
	/// <param name="sText">
	///	Text to show.
	/// </param>
	//*************************************************************************

	public static void
	ShowInformation
	(
		String sText
	)
	{
		Debug.Assert(sText != null && sText != "");

		MessageBox.Show(sText, ApplicationName, MessageBoxButtons.OK,
			MessageBoxIcon.Information);
	}

	//*************************************************************************
	//	Method: ShowError()
	//
	/// <summary>
	///	Displays text in an error message box.
	/// </summary>
	///
	/// <param name="sText">
	///	Text to show.
	/// </param>
	//*************************************************************************

	public static void
	ShowError
	(
		String sText
	)
	{
		Debug.Assert(sText != null && sText != "");

		MessageBox.Show(sText, ApplicationName, MessageBoxButtons.OK,
			MessageBoxIcon.Error);
	}

	//*************************************************************************
	//	Method: EnableControls()
	//
	/// <summary>
	///	Enables or disables the controls in a collection.
	/// </summary>
	///
	/// <param name="bEnable">
	/// true to enable.
	/// </param>
	///
	/// <param name="oControls">
	/// The controls to enable or disable.
	/// </param>
	//*************************************************************************

	public static void
	EnableControls
	(
		Boolean bEnable,
		Control.ControlCollection oControls
	)
	{
		// Recurse through the collection.

		foreach (Control oControl in oControls)
		{
			oControl.Enabled = bEnable;
			EnableControls(bEnable, oControl.Controls);
		}
	}

	//*************************************************************************
	//	Method: EnableControls()
	//
	/// <summary>
	///	Enables or disables a set of controls.
	/// </summary>
	///
	/// <param name="bEnable">
	/// true to enable.
	/// </param>
	///
	/// <param name="aoControls">
	/// The controls to enable or disable.
	/// </param>
	//*************************************************************************

	public static void
	EnableControls
	(
		Boolean bEnable,
		params Control[] aoControls
	)
	{
		foreach (Control oControl in aoControls)
			oControl.Enabled = bEnable;
	}

	//*************************************************************************
	//	Method: TrimTextBox()
	//
	/// <summary>
	/// Removes leading and trailing spaces from a TextBox.
	/// </summary>
	///
	/// <param name="oTextBox">
	///	TextBox to remove spaces from.
	/// </param>
	///
	///	<returns>
	///	The trimmed TextBox text.
	///	</returns>
	///
	///	<remarks>
	/// This method does not perform any validation.  Use it on a TextBox that
	/// can contain an empty string.  To validate a TextBox that must contain
	/// text, use <see cref="ValidateRequiredTextBox" /> instead of this
	/// method.
	///	</remarks>
	//*************************************************************************

	public static String
	TrimTextBox
	(
		TextBox oTextBox
	)
	{
		Debug.Assert(oTextBox != null);

		return ( oTextBox.Text = oTextBox.Text.Trim() );
	}

	//*************************************************************************
	//	Method: ValidateRequiredTextBox()
	//
	/// <summary>
	///	Validates a TextBox that must contain text.
	/// </summary>
	///
	/// <param name="oTextBox">
	///	TextBox to validate.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display if validation fails.
	/// </param>
	///
	/// <param name="sTrimmedText">
	///	Where the trimmed text gets stored.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the TextBox text is empty after being trimmed, <paramref
	///	name="sErrorMessage" /> is displayed and false is returned.  Otherwise,
	///	the TextBox text is trimmed and true is returned.
	///	</remarks>
	//*************************************************************************

	public static Boolean
	ValidateRequiredTextBox
	(
		TextBox oTextBox,
		String sErrorMessage,
		out String sTrimmedText
	)
	{
		Debug.Assert(oTextBox != null);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");

		// Remove leading and trailing spaces.

		sTrimmedText = oTextBox.Text.Trim();

		if (sTrimmedText == "")
			return ( OnInvalidTextBox(oTextBox, sErrorMessage) );

		oTextBox.Text = sTrimmedText;

		return (true);
	}

	//*************************************************************************
	//	Method: ValidateRequiredComboBox()
	//
	/// <summary>
	///	Validates a ComboBox that must contain text.
	/// </summary>
	///
	/// <param name="oComboBox">
	///	Control to validate.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display if validation fails.
	/// </param>
	///
	/// <param name="sTrimmedText">
	///	Where the trimmed text gets stored.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the ComboBox text is empty after being trimmed, <paramref
	///	name="sErrorMessage" /> is displayed and false is returned.  Otherwise,
	///	the control text is trimmed and true is returned.
	///	</remarks>
	//*************************************************************************

	public static Boolean
	ValidateRequiredComboBox
	(
		ComboBox oComboBox,
		String sErrorMessage,
		out String sTrimmedText
	)
	{
		Debug.Assert(oComboBox != null);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");

		// Remove leading and trailing spaces.

		sTrimmedText = oComboBox.Text.Trim();

		if (sTrimmedText == "")
			return ( OnInvalidComboBox(oComboBox, sErrorMessage) );

		oComboBox.Text = sTrimmedText;

		return (true);
	}

	//*************************************************************************
	//	Method: ValidateInt32TextBox()
	//
	/// <summary>
	///	Validates a TextBox that must contain an Int32.
	/// </summary>
	///
	/// <param name="oTextBox">
	///	TextBox to validate.
	/// </param>
	///
	/// <param name="iMinValue">
	///	Minimum valid value.
	/// </param>
	///
	/// <param name="iMaxValue">
	///	Maximum valid value.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display if validation fails.
	/// </param>
	///
	/// <param name="iInt32">
	///	Where the validated Int32 gets stored.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the TextBox does not contain an Int32 within the specified range,
	///	<paramref name="sErrorMessage" /> is displayed and false is returned.
	///	Otherwise, the TextBox text is trimmed and true is returned.
	///	</remarks>
	//*************************************************************************

	public static Boolean
	ValidateInt32TextBox
	(
		TextBox oTextBox,
		Int32 iMinValue,
		Int32 iMaxValue,
		String sErrorMessage,
		out Int32 iInt32
	)
	{
		Debug.Assert(oTextBox != null);
		Debug.Assert(iMinValue <= iMaxValue);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");

		iInt32 = Int32.MinValue;

		String sTrimmedText;

		// Check whether the user entered something besides spaces.

		if ( !ValidateRequiredTextBox(oTextBox, sErrorMessage,
			out sTrimmedText) )
		{
			return (false);
		}

		try
		{
			// Parse the string and check its value.

			iInt32 = Int32.Parse(sTrimmedText, NumberStyles.AllowThousands);

			if (iInt32 < iMinValue || iInt32 > iMaxValue)
				throw new FormatException();
		}
		catch (System.FormatException)
		{
			return ( OnInvalidTextBox(oTextBox, sErrorMessage) );
		}

		return (true);
	}

	//*************************************************************************
	//	Method: ValidateDoubleTextBox()
	//
	/// <summary>
	///	Validates a TextBox that must contain a Double.
	/// </summary>
	///
	/// <param name="oTextBox">
	///	TextBox to validate.
	/// </param>
	///
	/// <param name="dMinValue">
	///	Minimum valid value.
	/// </param>
	///
	/// <param name="dMaxValue">
	///	Maximum valid value.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display if validation fails.
	/// </param>
	///
	/// <param name="dDouble">
	///	Where the validated Double gets stored.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the TextBox does not contain a Double within the specified range,
	///	<paramref name="sErrorMessage" /> is displayed and false is returned.
	///	Otherwise, the TextBox text is trimmed and true is returned.
	///	</remarks>
	//*************************************************************************

	public static Boolean
	ValidateDoubleTextBox
	(
		TextBox oTextBox,
		Double dMinValue,
		Double dMaxValue,
		String sErrorMessage,
		out Double dDouble
	)
	{
		Debug.Assert(oTextBox != null);
		Debug.Assert(dMinValue <= dMaxValue);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");

		dDouble = Double.MinValue;

		String sTrimmedText;

		// Check whether the user entered something besides spaces.

		if ( !ValidateRequiredTextBox(oTextBox, sErrorMessage,
			out sTrimmedText) )
		{
			return (false);
		}

		try
		{
			// Parse the string and check its value.

			dDouble = Double.Parse(sTrimmedText,
                NumberStyles.AllowThousands | NumberStyles.Float);

			if (dDouble < dMinValue || dDouble > dMaxValue)
				throw new FormatException();
		}
		catch (System.FormatException)
		{
			return ( OnInvalidTextBox(oTextBox, sErrorMessage) );
		}

		return (true);
	}

	//*************************************************************************
	//	Method: ValidateDirectoryTextBox()
	//
	/// <summary>
	///	Validates a TextBox that must contain an existing directory.
	/// </summary>
	///
	/// <param name="oTextBox">
	///	TextBox to validate.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display if validation fails.
	/// </param>
	///
	/// <param name="sDirectory">
	///	Where the trimmed directory gets stored.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the TextBox does not contain an existing directory,
	///	<paramref name="sErrorMessage" /> is displayed and false is returned.
	///	Otherwise, the TextBox text is trimmed and true is returned.
	///	</remarks>
	//*************************************************************************

	public static Boolean
	ValidateDirectoryTextBox
	(
		TextBox oTextBox,
		String sErrorMessage,
		out String sDirectory
	)
	{
		Debug.Assert(oTextBox != null);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");

		// Check whether the user entered something besides spaces.

		if ( !ValidateRequiredTextBox(oTextBox, sErrorMessage, out sDirectory) )
			return (false);

		// Check whether the directory exists.

		if ( !Directory.Exists(sDirectory) )
			return ( OnInvalidTextBox(oTextBox, sErrorMessage) );

		return (true);
	}

	//*************************************************************************
	//	Method: ValidateFileTextBox()
	//
	/// <summary>
	///	Validates a TextBox that must contain the name of an existing file.
	/// </summary>
	///
	/// <param name="oTextBox">
	///	TextBox to validate.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display if validation fails.
	/// </param>
	///
	/// <param name="sFile">
	///	Where the trimmed file name gets stored.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the TextBox does not contain the name of an existing file,
	///	<paramref name="sErrorMessage" /> is displayed and false is returned.
	///	Otherwise, the TextBox text is trimmed and true is returned.
	///	</remarks>
	//*************************************************************************

	public static Boolean
	ValidateFileTextBox
	(
		TextBox oTextBox,
		String sErrorMessage,
		out String sFile
	)
	{
		Debug.Assert(oTextBox != null);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");

		// Check whether the user entered something besides spaces.

		if ( !ValidateRequiredTextBox(oTextBox, sErrorMessage, out sFile) )
			return (false);

		// Check whether the file exists.

		if ( !File.Exists(sFile) )
			return ( OnInvalidTextBox(oTextBox, sErrorMessage) );

		return (true);
	}

	//*************************************************************************
	//	Method: ValidateListBoxSelection()
	//
	/// <summary>
	///	Validates a ListBox that must have at least one item selected.
	/// </summary>
	///
	/// <param name="oListBox">
	///	ListBox to validate.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display if validation fails.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the ListBox does not have a selected item, <paramref
	///	name="sErrorMessage" /> is displayed and false is returned.  Otherwise,
	///	true is returned.
	///	</remarks>
	//*************************************************************************

	public static Boolean
	ValidateListBoxSelection
	(
		ListBox oListBox,
		String sErrorMessage
	)
	{
		Debug.Assert(oListBox != null);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");

		if (oListBox.SelectedIndices.Count == 0)
			return ( OnInvalidControl(oListBox, sErrorMessage) );

		return (true);
	}

	//*************************************************************************
	//	Method: ValidateNumericUpDown()
	//
	/// <summary>
	///	Validates a NumericUpDown control that contains a Decimal.
	/// </summary>
	///
	/// <param name="oNumericUpDown">
	///	NumericUpDown to validate.
	/// </param>
	///
	/// <param name="sValueDescription">
	///	Description of what the control contains, in lower case.  Sample:
	///	"a length".
	/// </param>
	///
	/// <param name="decValue">
	///	Where the validated value gets stored.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the NumericUpDown has empty Text or contains a number outside the
	/// control's Minimum and Maximum range, an error message is displayed and
	///	false is returned.  Otherwise, true is returned.
	///	</remarks>
	//*************************************************************************

	public static Boolean
	ValidateNumericUpDown
	(
		NumericUpDown oNumericUpDown,
		String sValueDescription,
		out Decimal decValue
	)
	{
		Debug.Assert(oNumericUpDown != null);
		Debug.Assert(sValueDescription != null && sValueDescription != "");

		decValue = Decimal.MinValue;
		String sValue = oNumericUpDown.Text;

		if (sValue != "")
		{
			Boolean bParseOK = true;

			try
			{
				decValue = Decimal.Parse(sValue);
			}
			catch (System.FormatException)
			{
				bParseOK = false;
			}

			if (bParseOK && decValue >= oNumericUpDown.Minimum &&
				decValue <= oNumericUpDown.Maximum)
			{
				return (true);
			}
		}

		const String Format = "N0";

		return ( OnInvalidNumericUpDown( oNumericUpDown,

			String.Format(

				"Enter {0} between {1} and {2}.",

				sValueDescription,
				oNumericUpDown.Minimum.ToString(Format),
				oNumericUpDown.Maximum.ToString(Format)
				) ) );
	}

	//*************************************************************************
	//	Method: ValidateNumericUpDown()
	//
	/// <summary>
	///	Validates a NumericUpDown control that contains a Double.
	/// </summary>
	///
	/// <param name="oNumericUpDown">
	///	NumericUpDown to validate.
	/// </param>
	///
	/// <param name="sValueDescription">
	///	Description of what the control contains, in lower case.  Sample:
	///	"a length".
	/// </param>
	///
	/// <param name="dValue">
	///	Where the validated value gets stored.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the NumericUpDown has empty Text or contains a number outside the
	/// control's Minimum and Maximum range, an error message is displayed and
	///	false is returned.  Otherwise, true is returned.
	///	</remarks>
	//*************************************************************************

	public static Boolean
	ValidateNumericUpDown
	(
		NumericUpDown oNumericUpDown,
		String sValueDescription,
		out Double dValue
	)
	{
		Debug.Assert(oNumericUpDown != null);
		Debug.Assert(sValueDescription != null && sValueDescription != "");

		dValue = Double.MinValue;
		Decimal decValue;

		if ( !ValidateNumericUpDown(oNumericUpDown, sValueDescription,
			out decValue) )
		{
			return (false);
		}

		dValue = (Double)decValue;
		return (true);
	}

	//*************************************************************************
	//	Method: ValidateNumericUpDown()
	//
	/// <summary>
	///	Validates a NumericUpDown control that contains a Single.
	/// </summary>
	///
	/// <param name="oNumericUpDown">
	///	NumericUpDown to validate.
	/// </param>
	///
	/// <param name="sValueDescription">
	///	Description of what the control contains, in lower case.  Sample:
	///	"a length".
	/// </param>
	///
	/// <param name="fValue">
	///	Where the validated value gets stored.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the NumericUpDown has empty Text or contains a number outside the
	/// control's Minimum and Maximum range, an error message is displayed and
	///	false is returned.  Otherwise, true is returned.
	///	</remarks>
	//*************************************************************************

	public static Boolean
	ValidateNumericUpDown
	(
		NumericUpDown oNumericUpDown,
		String sValueDescription,
		out Single fValue
	)
	{
		Debug.Assert(oNumericUpDown != null);
		Debug.Assert(sValueDescription != null && sValueDescription != "");

		fValue = Single.MinValue;
		Decimal decValue;

		if ( !ValidateNumericUpDown(oNumericUpDown, sValueDescription,
			out decValue) )
		{
			return (false);
		}

		fValue = (Single)decValue;
		return (true);
	}

	//*************************************************************************
	//	Method: ValidateNumericUpDown()
	//
	/// <summary>
	///	Validates a NumericUpDown control that contains an Int32.
	/// </summary>
	///
	/// <param name="oNumericUpDown">
	///	NumericUpDown to validate.
	/// </param>
	///
	/// <param name="sValueDescription">
	///	Description of what the control contains, in lower case.  Sample:
	///	"a length".
	/// </param>
	///
	/// <param name="iValue">
	///	Where the validated value gets stored.
	/// </param>
	///
	///	<returns>
	///	true if validation passes.
	///	</returns>
	///
	///	<remarks>
	///	If the NumericUpDown has empty Text or contains a number outside the
	/// control's Minimum and Maximum range, an error message is displayed and
	///	false is returned.  Otherwise, true is returned.
	///	</remarks>
	//*************************************************************************

	public static Boolean
	ValidateNumericUpDown
	(
		NumericUpDown oNumericUpDown,
		String sValueDescription,
		out Int32 iValue
	)
	{
		Debug.Assert(oNumericUpDown != null);
		Debug.Assert(sValueDescription != null && sValueDescription != "");

		iValue = Int32.MinValue;
		Decimal decValue;

		if ( !ValidateNumericUpDown(oNumericUpDown, sValueDescription,
			out decValue) )
		{
			return (false);
		}

		iValue = (Int32)decValue;

		if (iValue != decValue)
		{
			return ( OnInvalidNumericUpDown(oNumericUpDown,
				"Enter " + sValueDescription
				+ " that doesn't include a decimal point.") );
		}

		return (true);
	}

	//*************************************************************************
	//	Method: OnInvalidTextBox()
	//
	/// <summary>
	///	Handles a a TextBox that failed validation.
	/// </summary>
	///
	/// <param name="oTextBox">
	///	TextBox.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display.
	/// </param>
	///
	///	<returns>
	///	Always returns false.
	///	</returns>
	///
	///	<remarks>
	///	This method displays an error message, sets focus to the TextBox, and
	///	returns false.
	///	</remarks>
	//*************************************************************************

	public static Boolean
	OnInvalidTextBox
	(
		TextBox oTextBox,
		String sErrorMessage
	)
	{
		Debug.Assert(oTextBox != null);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");

		OnInvalidControl(oTextBox, sErrorMessage);
		oTextBox.SelectAll();
		return (false);
	}

	//*************************************************************************
	//	Method: OnInvalidComboBox()
	//
	/// <summary>
	///	Handles a a ComboBox that failed validation.
	/// </summary>
	///
	/// <param name="oComboBox">
	///	ComboBox.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display.
	/// </param>
	///
	///	<returns>
	///	Always returns false.
	///	</returns>
	///
	///	<remarks>
	///	This method displays an error message, sets focus to the ComboBox, and
	///	returns false.
	///	</remarks>
	//*************************************************************************

	public static Boolean
	OnInvalidComboBox
	(
		ComboBox oComboBox,
		String sErrorMessage
	)
	{
		Debug.Assert(oComboBox != null);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");

		OnInvalidControl(oComboBox, sErrorMessage);
		oComboBox.SelectAll();
		return (false);
	}

	//*************************************************************************
	//	Method: OnInvalidDateTimePicker()
	//
	/// <summary>
	///	Handles a a DateTimePicker that failed validation.
	/// </summary>
	///
	/// <param name="oDateTimePicker">
	///	DateTimePicker.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display.
	/// </param>
	///
	///	<returns>
	///	Always returns false.
	///	</returns>
	///
	///	<remarks>
	///	This method displays an error message, sets focus to the
	/// DateTimePicker, and returns false.
	///	</remarks>
	//*************************************************************************

	public static Boolean
	OnInvalidDateTimePicker
	(
		DateTimePicker oDateTimePicker,
		String sErrorMessage
	)
	{
		Debug.Assert(oDateTimePicker != null);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");

		OnInvalidControl(oDateTimePicker, sErrorMessage);
		return (false);
	}

	//*************************************************************************
	//	Method: OnInvalidNumericUpDown()
	//
	/// <summary>
	///	Handles a a NumericUpDown that failed validation.
	/// </summary>
	///
	/// <param name="oNumericUpDown">
	///	NumericUpDown control.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display.
	/// </param>
	///
	///	<returns>
	///	Always returns false.
	///	</returns>
	///
	///	<remarks>
	///	This method displays an error message, sets focus to the NumericUpDown
	///	control, and returns false.
	///	</remarks>
	//*************************************************************************

	public static Boolean
	OnInvalidNumericUpDown
	(
		NumericUpDown oNumericUpDown,
		String sErrorMessage
	)
	{
		Debug.Assert(oNumericUpDown != null);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");

		OnInvalidControl(oNumericUpDown, sErrorMessage);
		oNumericUpDown.Select(0, oNumericUpDown.Text.Length);

		return (false);
	}

	//*************************************************************************
	//	Method: OnInvalidControl()
	//
	/// <summary>
	///	Handles a a control that failed validation.
	/// </summary>
	///
	/// <param name="oControl">
	///	Control.
	/// </param>
	///
	/// <param name="sErrorMessage">
	///	Error message to display.
	/// </param>
	///
	///	<returns>
	///	Always returns false.
	///	</returns>
	///
	///	<remarks>
	///	This method displays an error message, sets focus to the control, and
	/// returns false.
	///	</remarks>
	//*************************************************************************

	public static Boolean
	OnInvalidControl
	(
		Control oControl,
		String sErrorMessage
	)
	{
		Debug.Assert(oControl != null);
		Debug.Assert(sErrorMessage != null && sErrorMessage != "");

		ShowWarning(sErrorMessage);
		oControl.Focus();
		return (false);
	}


	//*************************************************************************
	//	Protected fields
	//*************************************************************************

	/// String returned by the ApplicationName property.

	protected static String m_sApplicationName;
}

}
