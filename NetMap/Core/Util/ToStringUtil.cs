
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Collections;
using System.Diagnostics;

namespace Microsoft.NetMap.Core
{
//*****************************************************************************
//  Class: ToStringUtil
//
/// <summary>
/// Utility methods for implementing <see cref="Object.ToString" /> overrides.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class ToStringUtil
{
	//*************************************************************************
	//	Method: AppendPropertyToString()
	//
	/// <overloads>
	/// Appends a property value to a String.
	/// </overloads>
	///
	/// <summary>
	/// Appends a property value to a String with a newline.
	/// </summary>
	///
	/// <param name="oStringBuilder">
	/// Object to append to.
	/// </param>
	///
    /// <param name="iIndentationLevel">
	/// Indentation level.  Level 0 is "no indentation."
    /// </param>
    ///
    /// <param name="sPropertyName">
	/// Name of the property.  Can't be null or empty.
    /// </param>
    ///
    /// <param name="oPropertyValue">
	/// Value of the property.  Can be null or empty.
    /// </param>
    ///
	/// <remarks>
	/// This method appends a property name, property value, and a newline to a
	/// String.
	/// </remarks>
	//*************************************************************************

	public static void
	AppendPropertyToString
	(
		StringBuilder oStringBuilder,
		Int32 iIndentationLevel,
		String sPropertyName,
		Object oPropertyValue
	)
	{
		Debug.Assert(oStringBuilder != null);
		Debug.Assert(iIndentationLevel >= 0);
		Debug.Assert( !String.IsNullOrEmpty(sPropertyName) );

		AppendPropertyToString(oStringBuilder, iIndentationLevel,
			sPropertyName, oPropertyValue, true);
	}

	//*************************************************************************
	//	Method: AppendPropertyToString()
	//
	/// <summary>
	/// Appends a property value to a String with an optional newline.
	/// </summary>
	///
	/// <param name="oStringBuilder">
	/// Object to append to.
	/// </param>
	///
    /// <param name="iIndentationLevel">
	/// Indentation level.  Level 0 is "no indentation."
    /// </param>
    ///
    /// <param name="sPropertyName">
	/// Name of the property.  Can't be null or empty.
    /// </param>
    ///
    /// <param name="oPropertyValue">
	/// Value of the property.  Can be null or empty.
    /// </param>
    ///
    /// <param name="bAppendLine">
	/// true to append a newline after the property name and value.
    /// </param>
    ///
	/// <remarks>
	/// This method appends a property name, property value, and an optional
	/// newline to a String.
	/// </remarks>
	//*************************************************************************

	public static void
	AppendPropertyToString
	(
		StringBuilder oStringBuilder,
		Int32 iIndentationLevel,
		String sPropertyName,
		Object oPropertyValue,
		Boolean bAppendLine
	)
	{
		Debug.Assert(oStringBuilder != null);
		Debug.Assert(iIndentationLevel >= 0);
		Debug.Assert( !String.IsNullOrEmpty(sPropertyName) );

		AppendIndentationToString(oStringBuilder, iIndentationLevel);

		oStringBuilder.Append(sPropertyName);

		oStringBuilder.Append(" = ");

		AppendObjectToString(oStringBuilder, oPropertyValue);

		if (bAppendLine)
		{
			oStringBuilder.AppendLine();
		}
	}

	//*************************************************************************
	//	Method: AppendVerticesToString()
	//
	/// <summary>
	/// Appends a collection of <see cref="IVertex" /> objects to a String.
	/// </summary>
	///
	/// <param name="oStringBuilder">
	/// Object to append to.
	/// </param>
	///
    /// <param name="iIndentationLevel">
	/// Current indentation level.  Level 0 is "no indentation."
    /// </param>
    ///
	/// <param name="sFormat">
	/// The format to use, either G", "P", or "D".  See <see
	/// cref="NetMapBase.ToString()" /> for details.
	/// </param>
	///
	/// <param name="oVertices">
	/// Collection of <see cref="IVertex" /> objects.
	/// </param>
	//*************************************************************************

	public static void
	AppendVerticesToString
	(
		StringBuilder oStringBuilder,
		Int32 iIndentationLevel,
		String sFormat,
		ICollection oVertices
	)
	{
		Debug.Assert(oStringBuilder != null);
		Debug.Assert(iIndentationLevel >= 0);
		Debug.Assert( !String.IsNullOrEmpty(sFormat) );
		Debug.Assert(sFormat == "G" || sFormat == "P" || sFormat == "D");
		Debug.Assert(oVertices != null);

		#if false

		Sample string for sFormat == "G":

			2 vertices

		Sample string for sFormat == "P":

			2 vertices\r\n

		Sample string for sFormat == "D":

			2 vertices\r\n
				ID = 53\r\n
				ID = 143 \r\n

		#endif

		Int32 iVertices = oVertices.Count;

		AppendIndentationToString(oStringBuilder, iIndentationLevel);

		oStringBuilder.Append( iVertices.ToString(NetMapBase.Int32Format) );
		oStringBuilder.Append( (iVertices == 1) ? " vertex" : " vertices" );

		if (sFormat != "G")
		{
			oStringBuilder.AppendLine();
		}

		if (sFormat == "D")
		{
			foreach (IVertex oVertex in oVertices)
			{
				AppendIndentationToString(
					oStringBuilder, iIndentationLevel + 1);

				oStringBuilder.AppendLine( oVertex.ToString() );
			}
		}
	}

	//*************************************************************************
	//	Method: AppendEdgesToString()
	//
	/// <summary>
	/// Appends a collection of <see cref="IEdge" /> objects to a String.
	/// </summary>
	///
	/// <param name="oStringBuilder">
	/// Object to append to.
	/// </param>
	///
    /// <param name="iIndentationLevel">
	/// Current indentation level.  Level 0 is "no indentation."
    /// </param>
    ///
	/// <param name="sFormat">
	/// The format to use, either G", "P", or "D".  See <see
	/// cref="NetMapBase.ToString()" /> for details.
	/// </param>
	///
	/// <param name="oEdges">
	/// Collection of <see cref="IEdge" /> objects.
	/// </param>
	//*************************************************************************

	public static void
	AppendEdgesToString
	(
		StringBuilder oStringBuilder,
		Int32 iIndentationLevel,
		String sFormat,
		ICollection oEdges
	)
	{
		Debug.Assert(oStringBuilder != null);
		Debug.Assert(iIndentationLevel >= 0);
		Debug.Assert( !String.IsNullOrEmpty(sFormat) );
		Debug.Assert(sFormat == "G" || sFormat == "P" || sFormat == "D");
		Debug.Assert(oEdges != null);

		#if false

		Sample string for sFormat == "G":

			2 edges

		Sample string for sFormat == "P":

			2 edges\r\n

		Sample string for sFormat == "D":

			2 edges\r\n
				ID = 53\r\n
				ID = 143 \r\n

		#endif

		Int32 iEdges = oEdges.Count;

		AppendIndentationToString(oStringBuilder, iIndentationLevel);

		oStringBuilder.Append( iEdges.ToString(NetMapBase.Int32Format) );
		oStringBuilder.Append( (iEdges == 1) ? " edge" : " edges" );

		if (sFormat != "G")
		{
			oStringBuilder.AppendLine();
		}

		if (sFormat == "D")
		{
			foreach (IEdge oEdge in oEdges)
			{
				AppendIndentationToString(
					oStringBuilder, iIndentationLevel + 1);

				oStringBuilder.AppendLine( oEdge.ToString() );
			}
		}
	}

	//*************************************************************************
	//	Method: AppendIndentationToString()
	//
	/// <summary>
	/// Appends a specified number of indentation levels to a String.
	/// </summary>
	///
	/// <param name="oStringBuilder">
	/// Object to append to.
	/// </param>
	///
    /// <param name="iIndentationLevel">
	/// Indentation level.  Level 0 is "no indentation."
    /// </param>
	//*************************************************************************

	public static void
	AppendIndentationToString
	(
		StringBuilder oStringBuilder,
		Int32 iIndentationLevel
	)
	{
		Debug.Assert(oStringBuilder != null);
		Debug.Assert(iIndentationLevel >= 0);

		// Use tabs for indentation.

		while (iIndentationLevel > 0)
		{
			oStringBuilder.Append('\t');

			iIndentationLevel--;
		}
	}

	//*************************************************************************
	//	Method: AppendObjectToString()
	//
	/// <summary>
	/// Appends an Object to a String.
	/// </summary>
	///
	/// <param name="oStringBuilder">
	/// Object to append to.
	/// </param>
	///
    /// <param name="oObject">
	/// Object to append.  Can be null.
    /// </param>
    ///
	/// <remarks>
	/// This method appends the String form of <paramref name="oObject" /> to a
	/// String.  If <paramref name="oObject" /> is null, "[null]" is appended.
	/// </remarks>
	//*************************************************************************

	public static void
	AppendObjectToString
	(
		StringBuilder oStringBuilder,
		Object oObject
	)
	{
		Debug.Assert(oStringBuilder != null);

		oStringBuilder.Append(
			(oObject == null) ? "[null]" : oObject.ToString()
			);
	}
}

}
