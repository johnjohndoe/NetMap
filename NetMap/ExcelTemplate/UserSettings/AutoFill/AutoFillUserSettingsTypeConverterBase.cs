
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: AutoFillUserSettingsTypeConverterBase
//
/// <summary>
/// Base class for several autofill-related type converters that can convert
/// an object to and from a string.
/// </summary>
/// 
/// <remarks>
/// This class implements <see
/// cref="TypeConverter.CanConvertTo(ITypeDescriptorContext, Type)" /> and <see
/// cref="TypeConverter.CanConvertFrom(ITypeDescriptorContext, Type)" />
/// overrides.
/// </remarks>
//*****************************************************************************

public class AutoFillUserSettingsTypeConverterBase : TypeConverter
{
    //*************************************************************************
    //  Constructor: AutoFillUserSettingsTypeConverterBase()
    //
    /// <summary>
    /// Initializes a new instance of the
	/// AutoFillUserSettingsTypeConverterBase class.
    /// </summary>
    //*************************************************************************

    public AutoFillUserSettingsTypeConverterBase()
    {
		// (Do nothing.)

		// AssertValid();
    }

    //*************************************************************************
    //  Method: CanConvertTo()
    //
    /// <summary>
	/// Returns whether this converter can convert the object to the specified
	/// type, using the specified context.
    /// </summary>
	///
	/// <param name="context">
	/// An ITypeDescriptorContext that provides a format context. 
	/// </param>
	///
	/// <param name="sourceType">
	/// A Type that represents the type you want to convert to. 
	/// </param>
	///
	/// <returns>
	/// true if this converter can perform the conversion; otherwise, false.
	/// </returns>
    //*************************************************************************

	public override Boolean
	CanConvertTo
	(
		ITypeDescriptorContext context,
		Type sourceType
	)
	{
		AssertValid();

		return ( sourceType == typeof(String) );
	}

    //*************************************************************************
    //  Method: CanConvertFrom()
    //
    /// <summary>
	/// Returns whether this converter can convert an object of the given type
	/// to the type of this converter, using the specified context.
    /// </summary>
	///
	/// <param name="context">
	/// An ITypeDescriptorContext that provides a format context. 
	/// </param>
	///
	/// <param name="sourceType">
	/// A Type that represents the type you want to convert from. 
	/// </param>
	///
	/// <returns>
	/// true if this converter can perform the conversion; otherwise, false.
	/// </returns>
    //*************************************************************************

	public override Boolean
	CanConvertFrom
	(
		ITypeDescriptorContext context,
		Type sourceType
	)
	{
		AssertValid();

		return ( sourceType == typeof(String) );
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
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
