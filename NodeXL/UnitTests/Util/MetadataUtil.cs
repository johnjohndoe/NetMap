
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: MetadataUtil
//
/// <summary>
/// Utility methods for testing components that implement <see
/// cref="IMetadataProvider" />.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class MetadataUtil
{
    //*************************************************************************
    //  Method: SetRandomMetadata()
    //
    /// <summary>
    /// Sets random metadata and Tag values on an object that implements <see
	/// cref="IMetadataProvider" /.
    /// </summary>
	///
	/// <param name="oMetadataProvider">
	/// Object to set random metadata and Tag values on.
	/// </param>
	///
	/// <param name="bSetMetadataValues">
	/// true to set random metadata, false to leave the meatadata empty.
	/// </param>
	///
	/// <param name="bSetTag">
	/// true to set a random Tag value, false to leave the Tag as null.
	/// </param>
	///
	/// <param name="iSeed">
	/// Seed to use for the random number generator.
	/// </param>
	///
	/// <remarks>
	/// The random metadata and Tag values can be checked with <see
	/// cref="CheckRandomMetadata" />.
	/// </remarks>
    //*************************************************************************

	public static void
	SetRandomMetadata
	(
		IMetadataProvider oMetadataProvider,
		Boolean bSetMetadataValues,
		Boolean bSetTag,
		Int32 iSeed
	)
	{
		KeyValuePair<String, Object> [] aoRandomMetadata;
		Object oRandomTag;

		GetRandomMetadata(iSeed, out aoRandomMetadata, out oRandomTag);

		if (bSetMetadataValues)
		{
			foreach (KeyValuePair<String, Object> oRandomMetadata
				in aoRandomMetadata)
			{
				oMetadataProvider.SetValue(
					oRandomMetadata.Key, oRandomMetadata.Value);
			}
		}

		if (bSetTag)
		{
			oMetadataProvider.Tag = oRandomTag;
		}
	}

    //*************************************************************************
    //  Method: CheckRandomMetadata()
    //
    /// <summary>
    /// Checks the random metadata and Tag values on an object that implements
	/// <see cref="IMetadataProvider" />.
    /// </summary>
	///
	/// <param name="oMetadataProvider">
	/// Object to check random metadata and Tag values on.
	/// </param>
	///
	/// <param name="bRandomMetadataExpected">
	/// true if the metadata was set with <see cref="SetRandomMetadata" />,
	/// false if the metadata was left empty.
	/// </param>
	///
	/// <param name="bRandomTagExpected">
	/// true if the Tag was set with <see cref="SetRandomMetadata" />,
	/// false if the Tag was left as null.
	/// </param>
	///
	/// <param name="iSeed">
	/// Seed to use for the random number generator.
	/// </param>
	///
	/// <remarks>
	/// This method checks the random metadata and Tag values that were set
	/// with <see cref="SetRandomMetadata" />.
	/// </remarks>
    //*************************************************************************

	public static void
	CheckRandomMetadata
	(
		IMetadataProvider oMetadataProvider,
		Boolean bRandomMetadataExpected,
		Boolean bRandomTagExpected,
		Int32 iSeed
	)
	{
		KeyValuePair<String, Object> [] aoRandomMetadata;
		Object oRandomTag;

		GetRandomMetadata(iSeed, out aoRandomMetadata, out oRandomTag);

		foreach (KeyValuePair<String, Object> oRandomMetadata
			in aoRandomMetadata)
		{
			String sRandomKey = oRandomMetadata.Key;
			Object oRandomValue = oRandomMetadata.Value;

			if (bRandomMetadataExpected)
			{
				Assert.IsTrue( oMetadataProvider.ContainsKey(sRandomKey) );

				Object oValue;
				
				Assert.IsTrue( oMetadataProvider.TryGetValue(

					sRandomKey,

					(oRandomValue == null) ?
						typeof(Object) : oRandomValue.GetType(),

					out oValue
					) );

				CheckMetadataValue(oRandomValue, oValue);
			}
			else
			{
				Assert.IsFalse( oMetadataProvider.ContainsKey(sRandomKey) );
			}
		}

		if (bRandomTagExpected)
		{
			CheckMetadataValue(oRandomTag, oMetadataProvider.Tag);
		}
		else
		{
			Assert.IsNull(oMetadataProvider.Tag);
		}
	}

    //*************************************************************************
    //  Method: TestGetValue()
    //
    /// <summary>
    /// Tests the <see cref="IMetadataProvider.GetValue(String)" /> variants.
    /// </summary>
	///
	/// <param name="oMetadataProvider">
	/// Object to call <see cref="IMetadataProvider.GetValue(String)" /> on.
	/// </param>
	///
	/// <param name="sKey">
	/// Metadata key.
	/// </param>
	///
	/// <param name="oExpectedValue">
	/// Expected value.  Can be null.
	/// </param>
	///
	/// <remarks>
	/// This method calls all variants of <see
	/// cref="IMetadataProvider.GetValue(String)" /> and tests the results.
	/// </remarks>
    //*************************************************************************

	public static void
	TestGetValue
	(
		IMetadataProvider oMetadataProvider,
		String sKey,
		Object oExpectedValue
	)
	{
		Debug.Assert(oMetadataProvider != null);
		Assert.IsFalse( String.IsNullOrEmpty(sKey) );

		Type oExpectedType = (oExpectedValue == null) ?
			typeof(Object) : oExpectedValue.GetType();

		// Call each of the GetValue() variants.

		foreach ( Int32 i in new Int32 [] {0, 1, 2, 3, 4} )
		{
		    Object oValue = null;

			switch (i)
			{
				case 0:

					oValue = oMetadataProvider.GetValue(sKey);

					break;

				case 1:

					Assert.IsTrue( oMetadataProvider.TryGetValue(
						sKey, out oValue) );

					break;

				case 2:

					oValue = oMetadataProvider.GetValue(sKey, oExpectedType);

					break;

				case 3:

					Assert.IsTrue( oMetadataProvider.TryGetValue(
						sKey, oExpectedType, out oValue) );

					break;

				case 4:

					oValue = oMetadataProvider.GetRequiredValue(sKey, oExpectedType);

					break;

				default:

					Debug.Assert(false);
					break;
			}

			CheckMetadataValue(oExpectedValue, oValue);
		}
	}

    //*************************************************************************
    //  Method: GetRandomMetadata()
    //
    /// <summary>
    /// Generates random metadata and Tag values.
    /// </summary>
	///
	/// <param name="iSeed">
	/// Seed to use for the random number generator.
	/// </param>
	///
	/// <param name="aoRandomMetadata">
	/// Where an array of random metadata gets stored.
	/// </param>
	///
	/// <param name="oRandomTag">
	/// Where a random tag value gets stored.
	/// </param>
    //*************************************************************************

	private static void
	GetRandomMetadata
	(
		Int32 iSeed,
		out KeyValuePair<String, Object> [] aoRandomMetadata,
		out Object oRandomTag
	)
	{
		aoRandomMetadata = null;
		oRandomTag = null;

		Random oRandom = new Random(iSeed);

		Int32 iKeyValuePairs = oRandom.Next(50);

		aoRandomMetadata = new KeyValuePair<String, Object>[iKeyValuePairs];

		for (Int32 i = 0; i < iKeyValuePairs; i++)
		{
			aoRandomMetadata[i] = new KeyValuePair<String, Object>(
				GetRandomKey(oRandom),
				GetRandomValue(oRandom)
				);
		}

		oRandomTag = GetRandomValue(oRandom);
	}

    //*************************************************************************
    //  Method: GetRandomKey()
    //
    /// <summary>
    /// Generates a random key to use for random metadata.
    /// </summary>
	///
	/// <param name="oRandom">
	/// Random number generator.
	/// </param>
	///
	/// <returns>
	/// Random key.
	/// </returns>
    //*************************************************************************

	private static String
	GetRandomKey
	(
		Random oRandom
	)
	{
		Debug.Assert(oRandom != null);

		return ( oRandom.Next().ToString() );
	}

    //*************************************************************************
    //  Method: GetRandomValue()
    //
    /// <summary>
    /// Generates a random value to use for random metadata and tags.
    /// </summary>
	///
	/// <param name="oRandom">
	/// Random number generator.
	/// </param>
	///
	/// <returns>
	/// Random key.
	/// </returns>
    //*************************************************************************

	private static Object
	GetRandomValue
	(
		Random oRandom
	)
	{
		Debug.Assert(oRandom != null);

		switch ( oRandom.Next(7) )
		{
			case 0:

				// Random String.

				return ( oRandom.Next().ToString() );

			case 1:

				// Random DateTime.

				return (
					new DateTime(
						oRandom.Next(1900, 2100 + 1),  // Year
						oRandom.Next(1, 12 + 1),  // Month
						oRandom.Next(1, 28 + 1),  // Day
						oRandom.Next(0, 23 + 1),  // Hours 
						oRandom.Next(0, 59 + 1),  // Minutes
						oRandom.Next(0, 59 + 1)  // Seconds
					) );

			case 2:

				// Random Int32.

				return ( oRandom.Next() );

			case 3:

				// Random Single.

				return ( (Single)oRandom.NextDouble() );

			case 4:

				// Random Double.

				return ( oRandom.NextDouble() );

			case 5:

				// Random Point.

				return ( new Point(
					oRandom.Next(),
					oRandom.Next()
					) );

			case 6:

				// null value.

				return (null);

			default:

				Debug.Assert(false);

				throw new ApplicationException(
					"MetadataUtil.GetRandomValue: Unexpected case."
					);
		}
	}

    //*************************************************************************
    //  Method: CheckMetadataValue()
    //
    /// <summary>
    /// Checks a metadata or Tag value.
    /// </summary>
	///
	/// <param name="oExpectedValue">
	/// Expected value.  Can be null.
	/// </param>
	///
	/// <param name="oValue">
	/// Actual value.  Can be null.
	/// </param>
    //*************************************************************************

	private static void
	CheckMetadataValue
	(
		Object oExpectedValue,
		Object oValue
	)
	{
		if (oExpectedValue == null)
		{
			Assert.IsNull(oValue);

			return;
		}

        Assert.IsNotNull(oValue);

		Assert.AreEqual( oValue.GetType(), oExpectedValue.GetType() );

		switch ( oValue.GetType().Name )
		{
			case "String":

				Assert.IsTrue( (String)oExpectedValue == (String)oValue );
				break;

			case "DateTime":

				Assert.IsTrue( (DateTime)oExpectedValue == (DateTime)oValue );
				break;

			case "Int32":

				Assert.IsTrue( (Int32)oExpectedValue == (Int32)oValue );
				break;

			case "Single":

				Assert.IsTrue( (Single)oExpectedValue == (Single)oValue );
				break;

			case "Double":

				Assert.IsTrue( (Double)oExpectedValue == (Double)oValue );
				break;

			case "Point":

				Assert.IsTrue( (Point)oExpectedValue == (Point)oValue );
				break;

			default:

				Debug.Assert(false);

				throw new ApplicationException(
					"MetadataUtil.CheckMetadataValue: Type is unexpected."
					);
		}
	}
}

}
