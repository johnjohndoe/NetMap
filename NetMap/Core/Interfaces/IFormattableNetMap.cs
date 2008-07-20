
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;

namespace Microsoft.NetMap.Core
{
//*****************************************************************************
//  Interface: IFormattableNetMap
//
/// <summary>
///	Provides functionality for formatting a NetMap object into a String
///	representation.
/// </summary>
///
/// <remarks>
/// This interface adds a <see cref="ToString(String)" /> method to the <see
/// cref="IFormattable" /> interface and defines a minimum set of format
/// specifiers for formatting NetMap objects.
/// </remarks>
//*****************************************************************************

public interface IFormattableNetMap : IFormattable
{
	//*************************************************************************
	//	Method: ToString()
	//
	/// <overloads>
	/// Formats the value of the current instance.
	/// </overloads>
	///
	/// <summary>
	/// Formats the value of the current instance using the default format. 
	/// </summary>
	///
	/// <returns>
	/// The formatted string.
	/// </returns>
	///
	/// <remarks>
	/// This is the same as <see cref="ToString(String)" />("G").
	/// </remarks>
	//*************************************************************************

	String
	ToString();

	//*************************************************************************
	//	Method: ToString()
	//
	/// <summary>
	/// Formats the value of the current instance using a specified format.
	/// </summary>
	///
	/// <param name="format">
	/// The <see cref="String" /> specifying the format to use, or null or an
	/// empty string to use the default format.  See <see
	/// cref="ToString(String, IFormatProvider)" /> for the available formats.
	/// </param>
	///
	/// <returns>
	/// The formatted string.
	/// </returns>
	//*************************************************************************

	String
	ToString
	(
		String format
	);

	//*************************************************************************
	//	Method: ToString()
	//
	/// <summary>
	/// Formats the value of the current instance using a specified format and
	/// format provider.
	/// </summary>
	///
	/// <param name="format">
	/// The <see cref="String" /> specifying the format to use, or null or an
	/// empty string to use the default format.  The minimum set of supported
	/// formats are listed in the remarks.
	/// </param>
	///
	/// <param name="formatProvider">
	/// The <see cref="IFormatProvider" /> to use to format the value, or null
	/// to use the format information from the current locale setting of the
	/// operating system.   The implementation may ignore this parameter.
	/// </param>
	///
	/// <returns>
	/// The formatted string.
	/// </returns>
	///
	/// <remarks>
	/// The implementation may ignore <paramref name="formatProvider" />.
	///
	/// <para>
	/// The implementation must support the following <paramref
	/// name="format" /> values.  It may support additional formats as well.
	/// </para>
	///
	///	<list type="table">
	///
	///	<listheader>
	/// <term>Format</term>
	/// <term>Results</term>
	///	</listheader>
	///
	///	<item>
	///	<term>G</term>
	///	<term>
	/// Default format, includes only an instance summary.  An instance summary
	/// may be an object ID or name, for example.
	///	</term>
	///	</item>
	///
	///	<item>
	///	<term>P</term>
	///	<term>
	/// Includes all public properties, each on a separate line.  For
	/// collection properties, only the item count is included.
	///	</term>
	///	</item>
	///
	///	<item>
	///	<term>D</term>
	///	<term>
	/// Includes all public properties, each on a separate line.  For
	/// collection properties, a summary of each item in the collection is
	/// included, each on a separate line.
	///	</term>
	///	</item>
	///
	///	<item>
	///	<term>null</term>
	///	<term>
	/// Same as G.
	///	</term>
	///	</item>
	///
	///	<item>
	///	<term>Empty string ("")</term>
	///	<term>
	/// Same as G.
	///	</term>
	///	</item>
	///
	///	</list>
	///
	/// </remarks>
	//*************************************************************************

	new String
	ToString
	(
		String format,
		IFormatProvider formatProvider
	);
}

}
