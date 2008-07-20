
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//	Class: ObjectWithText
//
/// <summary>
/// Stores an object along with text that describes the object.
/// </summary>
///
/// <remarks>
/// <see cref="ToString" /> is overridden to return the text.
///
/// <para>
/// This object can be used to populate a ListBox or ComboBox with arbitrary
/// objects.  See ComboBoxPlus.PopulateWithObjectsAndText.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class ObjectWithText : Object, IComparable
{
	//*************************************************************************
	//	Constructor: ObjectWithText()
	//
	/// <summary>
	/// Initializes a new instance of the ObjectWithText class.
	/// </summary>
	///
	/// <param name="oObject">
	/// An arbitrary object, or null.
	/// </param>
	///
	/// <param name="sText">
	/// Text describing the object.
	/// </param>
	//*************************************************************************

	public ObjectWithText
	(
		Object oObject,
		String sText
	)
	{
		m_oObject = oObject;
		m_sText = sText;

		AssertValid();
	}

    //*************************************************************************
    //  Property: Object
    //
    /// <summary>
	/// Gets or sets the arbitrary object.
    /// </summary>
    ///
    /// <value>
	/// An arbitrary object, or null.
    /// </value>
    //*************************************************************************

    public Object Object
    {
        get
        {
            AssertValid();

            return (m_oObject); 
        }

		set
		{
			m_oObject = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: Text
    //
    /// <summary>
	/// Gets or sets the text describing the object.
    /// </summary>
    ///
    /// <value>
	/// Text describing the object.
    /// </value>
    //*************************************************************************

    public String Text
    {
        get
        {
            AssertValid();

            return (m_sText); 
        }

		set
		{
			m_sText = value;

			AssertValid();
		}
    }

	//*************************************************************************
	//	Method: ToString()
	//
	/// <summary>
	/// This member overrides Object.ToString.
	/// </summary>
	//*************************************************************************

	override public String
	ToString()
	{
		AssertValid();

		return (Text);
	}

	//*************************************************************************
	//	Method: CompareTo()
	//
	/// <summary>
	/// Compares the current instance with another object of the same type.
	/// </summary>
	///
	/// <param name="oOtherObject">
	/// Object to compare to.
	/// </param>
	//*************************************************************************

	public Int32
	CompareTo
	(
		Object oOtherObject
	)
	{
		Debug.Assert(oOtherObject != null);

		return ( Text.CompareTo( ( (ObjectWithText)oOtherObject ).Text) );
	}

	//*************************************************************************
	//	Method: AssertValid()
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
	//	Protected fields
	//*************************************************************************

	/// An arbitrary object, or null.

	protected Object m_oObject;

	/// Text describing the object.

	protected String m_sText;
}

}
