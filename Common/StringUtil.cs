
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//  Class: StringUtil
//
/// <summary>
/// String utility methods.
/// </summary>
///
/// <remarks>
/// This class contains utility methods for dealing with String objects.  All
/// methods are static.
/// </remarks>
//*****************************************************************************

public class StringUtil
{
    //*************************************************************************
    //  Constructor: StringUtil()
    //
    /// <summary>
    /// Do not use this constructor.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  All StringUtil methods are static.
    /// </remarks>
    //*************************************************************************

    private
    StringUtil()
    {
        // (All methods are static.)
    }

    //*************************************************************************
    //  Method: IsEmpty()
    //
    /// <summary>
    /// Returns true if a String is null or has a length of 0.
    /// </summary>
    ///
    /// <param name="sString">
    /// String to test.
    /// </param>
    ///
    /// <returns>
    /// true if the string is null or has a length of 0.
    /// </returns>
    //*************************************************************************

    public static Boolean
    IsEmpty
    (
        String sString
    )
    {
        return (sString == null || sString.Length == 0);
    }

    //*************************************************************************
    //  Method: AssertNotEmpty()
    //
    /// <summary>
    /// Asserts if a String is null or has a length of 0.  Debug-only.
    /// </summary>
    ///
    /// <param name="sString">
    /// String to test.
    /// </param>
    //*************************************************************************

    [Conditional("DEBUG")]

    public static void
    AssertNotEmpty
    (
        String sString
    )
    {
        Debug.Assert( !IsEmpty(sString) );
    }

    //*************************************************************************
    //  Method: MakePlural()
    //
    /// <summary>
    /// Adds an "s" to a noun if the noun should be plural.
    /// </summary>
    ///
    /// <param name="sNoun">
    /// Noun to make plural if necessary.  Sample: "orange".
    /// </param>
    ///
    /// <param name="iCount">
    /// Number of things that <paramref name="sNoun" /> describes.  Sample: 2.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="sNoun" /> with "s" appended to it if necessary.
    /// Sample: "oranges".
    /// </returns>
    //*************************************************************************

    public static String
    MakePlural
    (
        String sNoun,
        Int32 iCount
    )
    {
        Debug.Assert( !IsEmpty(sNoun) );
        Debug.Assert(iCount >= 0);

        return ( (iCount == 1) ? sNoun : sNoun + "s" );
    }

    //*************************************************************************
    //  Method: CopyStringArray()
    //
    /// <summary>
    /// Creates a deep copy of an array of strings.
    /// </summary>
    ///
    /// <param name="asArrayToCopy">
    /// Array of zero or more strings to copy.
    /// </param>
    ///
    /// <returns>
    /// A new array containing copies of the strings in <paramref
    /// name="asArrayToCopy" />.
    /// </returns>
    //*************************************************************************

    public static String []
    CopyStringArray
    (
        String [] asArrayToCopy
    )
    {
        Debug.Assert(asArrayToCopy != null);

        Int32 iStrings = asArrayToCopy.Length;
        String [] asNewArray = new String [iStrings];

        for (Int32 i = 0; i < iStrings; i++)
            asNewArray[i] = String.Copy( asArrayToCopy[i] );

        return (asNewArray);
    }

    //*************************************************************************
    //  Method: CreateArrayOfEmptyStrings()
    //
    /// <summary>
    /// Creates an array of empty strings.
    /// </summary>
    ///
    /// <param name="iEmptyStrings">
    /// Number of empty strings to store in the array.
    /// </param>
    ///
    /// <returns>
    /// An array of <paramref name="iEmptyStrings" /> elements, each of which
    /// contains String.Empty.
    /// </returns>
    //*************************************************************************

    public static String []
    CreateArrayOfEmptyStrings
    (
        Int32 iEmptyStrings
    )
    {
        Debug.Assert(iEmptyStrings >= 0);

        String [] asEmptyStrings = new String[iEmptyStrings];

        for (Int32 i = 0; i < iEmptyStrings; i++)
            asEmptyStrings[i] = String.Empty;

        return (asEmptyStrings);
    }

    //*************************************************************************
    //  Method: BytesToPrintableAscii
    //
    /// <summary>
    /// Converts an array of bytes to a printable ASCII string.
    /// </summary>
    ///
    /// <param name="abtBytes">
    /// Array of bytes to convert.  Can't be null.
    /// </param>
    ///
    /// <param name="cReplacementCharacter">
    /// Character to replace non-printable characters with.  Must have a value
    /// less than or equal to 127.
    /// </param>
    ///
    /// <remarks>
    /// This method replaces any bytes greater than 127 with <paramref
    /// name="cReplacementCharacter" />, then replaces any non-printable ASCII
    /// characters with the same replacement character.
    /// </remarks>
    //*************************************************************************

    public static String
    BytesToPrintableAscii
    (
        Byte [] abtBytes,
        Char cReplacementCharacter
    )
    {
        Debug.Assert(abtBytes != null);
        Debug.Assert( (Int32)cReplacementCharacter < 128 );

        // Make a copy of the byte array.

        Byte [] abtClone = ( Byte [] )abtBytes.Clone();

        // Replace any bytes > 127.

        for (Int32 i = 0; i < abtClone.Length; i++)
            if (abtClone[i] > 127)
                abtClone[i] = (Byte)cReplacementCharacter;

        // Use the ASCIIEncoding to get a string.  This leaves all bytes
        // between 0 and 127 unmodified.

        String sString = Encoding.ASCII.GetString(abtClone);

        // Replace the non-printable characters.

        return ( ReplaceNonPrintableAsciiCharacters(
            sString, cReplacementCharacter) );
    }

    //*************************************************************************
    //  Method: ReplaceNonPrintableAsciiCharacters()
    //
    /// <summary>
    /// Replaces non-printable ASCII characters with a specified character.
    /// </summary>
    ///
    /// <param name="sString">
    /// String that may include non-printable ASCII characters.  Can't be null.
    /// </param>
    ///
    /// <param name="cReplacementCharacter">
    /// Character to replace them with.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="sString" /> with non-printable characters replaced with
    /// <paramref name="cReplacementCharacter" />.
    /// </returns>
    //*************************************************************************

    public static String
    ReplaceNonPrintableAsciiCharacters
    (
        String sString,
        Char cReplacementCharacter
    )
    {
        Debug.Assert(sString != null);

        Regex oRegex = new Regex(@"[^\x09\x0A\x0D\x20-\x7E]");

        return ( oRegex.Replace(
            sString, new String(cReplacementCharacter, 1) ) );
    }

    //*************************************************************************
    //  Method: ReplaceControlCharacters()
    //
    /// <summary>
    /// Replaces Unicode control characters with a specified character.
    /// </summary>
    ///
    /// <param name="sString">
    /// String that may include control characters.  Can't be null.
    /// </param>
    ///
    /// <param name="cReplacementCharacter">
    /// Character to replace them with.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="sString" /> with control characters replaced with
    /// <paramref name="cReplacementCharacter" />.
    /// </returns>
    //*************************************************************************

    public static String
    ReplaceControlCharacters
    (
        String sString,
        Char cReplacementCharacter
    )
    {
        Debug.Assert(sString != null);

        Int32 iLength = sString.Length;

        StringBuilder oStringBuilder = new StringBuilder(iLength);

        for (Int32 i = 0; i < iLength; i++)
        {
            Char cChar = sString[i];

            oStringBuilder.Append(Char.IsControl(cChar) ?
                cReplacementCharacter : cChar);
        }

        return ( oStringBuilder.ToString() );
    }
}

}
