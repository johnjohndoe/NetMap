
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//	Enum: EnumSplitStyle
//
/// <summary>
///	Indicates how <see cref="EnumUtil.SplitName" /> capitalizes words in a
/// split enum name.
/// </summary>
//*****************************************************************************

public enum EnumSplitStyle
{
	/// "DaysActiveInNewsgroup" gets converted to "days active in newsgroup",
	/// for example.

	AllWordsStartLowerCase,

	/// "DaysActiveInNewsgroup" gets converted to "Days Active In Newsgroup",
	/// for example.

	AllWordsStartUpperCase,

	/// "DaysActiveInNewsgroup" gets converted to "Days active in newsgroup",
	/// for example.

	FirstWordStartsUpperCase,
}

//*****************************************************************************
//	Class: EnumUtil
//
/// <summary>
///	Static utility methods involving Enums.
/// </summary>
//*****************************************************************************

public class EnumUtil : Object
{
	//*************************************************************************
	//	Constructor: EnumUtil()
	//
	/// <summary>
	/// Do not call this constructor.
	/// </summary>
	//*************************************************************************

	private EnumUtil()
	{
		// (Do nothing.)
	}

	//*************************************************************************
	//	Method: SplitName()
	//
	/// <summary>
	///	Splits words in an enum name.
	/// </summary>
	///
	/// <param name="sEnumName">
	///	Enum name that needs to be split.
	/// </param>
	///
	/// <param name="eEnumSplitStyle">
	///	Indicates how the split words will be capitalized.
	/// </param>
	///
	///	<returns>
	/// <paramref name="sEnumName" /> with separated words.
	///	</returns>
	///
	///	<remarks>
	/// This method inserts a space before each upper-case letter in
	/// <paramref name="sEnumName" />.  If <paramref name="eEnumSplitStyle" />
	/// is AllWordsStartLowerCase, for example, the name
	/// "DaysActiveInNewsgroup" gets converted to "days active in newsgroup".
	///	</remarks>
	//*************************************************************************

	public static String
	SplitName
	(
		String sEnumName,
		EnumSplitStyle eEnumSplitStyle
	)
	{
		Debug.Assert(sEnumName != null);
		Debug.Assert( Enum.IsDefined(typeof(EnumSplitStyle), eEnumSplitStyle) );

		StringBuilder oStringBuilder = new StringBuilder();
		Int32 iLength = sEnumName.Length;

		Boolean bConvertToLowerCase =
			(eEnumSplitStyle == EnumSplitStyle.AllWordsStartLowerCase
			||
			eEnumSplitStyle == EnumSplitStyle.FirstWordStartsUpperCase);

		// Loop through the characters.

		for (Int32 i = 0; i < iLength; i++)
		{
			Char cChar = sEnumName[i];

			if ( i > 0 && Char.IsUpper(cChar) )
			{
				// Insert a space.

				oStringBuilder.Append(' ');

				// Convert to lower case if requested.

				if (bConvertToLowerCase)
					cChar = Char.ToLower(cChar);
			}

			oStringBuilder.Append(cChar);
		}

		// Convert the first character to lower case if requested.

		if (iLength > 0 &&
			eEnumSplitStyle == EnumSplitStyle.AllWordsStartLowerCase)
		{
			oStringBuilder[0] = Char.ToLower( oStringBuilder[0] );
		}

		return ( oStringBuilder.ToString() );
	}
}

}
