
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.UnitTests
{
//*****************************************************************************
//  Class: MetadataProviderTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="MetadataProvider" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class MetadataProviderTest : Object
{
    //*************************************************************************
    //  Constructor: MetadataProviderTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="MetadataProviderTest" />
	/// class.
    /// </summary>
    //*************************************************************************

    public MetadataProviderTest()
    {
		m_oMetadataProvider = null;
		m_oCopy = null;
    }

    //*************************************************************************
    //  Method: SetUp()
    //
    /// <summary>
    /// Gets run before each test.
    /// </summary>
    //*************************************************************************

    [TestInitializeAttribute]

    public void
    SetUp()
    {
		m_oMetadataProvider = new MetadataProvider();
		m_oCopy = new MockMetadataProvider();
    }

    //*************************************************************************
    //  Method: TearDown()
    //
    /// <summary>
    /// Gets run after each test.
    /// </summary>
    //*************************************************************************

    [TestCleanupAttribute]

    public void
    TearDown()
    {
		m_oMetadataProvider = null;
		m_oCopy = null;
    }

    //*************************************************************************
    //  Method: TestConstructor()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestConstructor()
    {
		Assert.IsNull(m_oMetadataProvider.Tag);
    }

    //*************************************************************************
    //  Method: TestTag()
    //
    /// <summary>
    /// Tests the Tag property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTag()
    {
		const String Value = "abc";

		m_oMetadataProvider.Tag = Value;

		Assert.AreEqual( (String)Value,  (String)m_oMetadataProvider.Tag );
    }

    //*************************************************************************
    //  Method: TestTag2()
    //
    /// <summary>
    /// Tests the Tag property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTag2()
    {
		m_oMetadataProvider.Tag = null;

		Assert.IsNull(m_oMetadataProvider.Tag);
    }

    //*************************************************************************
    //  Method: TestSetGetValue()
    //
    /// <summary>
    /// Tests the SetValue(), TryGetValue(), and ContainsKey() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSetGetValue()
    {
		// No keys.

		const String Key = "abc";

		Boolean bContainsKey = m_oMetadataProvider.ContainsKey(Key);

		Assert.IsFalse(bContainsKey);

		Object oValue;

		Assert.IsFalse( m_oMetadataProvider.TryGetValue(Key, out oValue) );
    }

    //*************************************************************************
    //  Method: TestSetGetValue2()
    //
    /// <summary>
    /// Tests the SetValue(), TryGetValue(), and ContainsKey() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSetGetValue2()
    {
		// Add key, ask for a different key.

		m_oMetadataProvider.SetValue("xyz", 123);

		const String Key = "kjrek";

		Boolean bContainsKey = m_oMetadataProvider.ContainsKey(Key);

		Assert.IsFalse(bContainsKey);

		Object oValue;

		Assert.IsFalse( m_oMetadataProvider.TryGetValue(Key, out oValue) );
    }

    //*************************************************************************
    //  Method: TestSetGetValue3()
    //
    /// <summary>
    /// Tests the SetValue(), TryGetValue(), and ContainsKey() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSetGetValue3()
    {
		// Add key, ask for same key.

		const String Key = "fdjkre";
		const Int32 Value = 123456;

		m_oMetadataProvider.SetValue(Key, Value);

		Boolean bContainsKey = m_oMetadataProvider.ContainsKey(Key);

		Assert.IsTrue(bContainsKey);

		Object oValue;
		
		Assert.IsTrue( m_oMetadataProvider.TryGetValue(Key, out oValue) );

		Assert.IsTrue(oValue is Int32);

		Assert.AreEqual(Value, (Int32)oValue);
    }

    //*************************************************************************
    //  Method: TestSetGetValue4()
    //
    /// <summary>
    /// Tests the SetValue(), TryGetValue(), and ContainsKey() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSetGetValue4()
    {
		// Add N keys, ask for same keys in reverse order.

		// Create an array of keys.

		const Int32 Keys = 1000;

		String [] asKeys = new String[Keys];

		for (Int32 i = 0; i < Keys; i++)
		{
            asKeys[i] = Guid.NewGuid().ToString();
		}

		// Add a value for each key.  The value is just the key with an
		// appended index.

		for (Int32 i = 0; i < Keys; i++)
		{
			String sKey = asKeys[i];

			m_oMetadataProvider.SetValue( sKey, sKey + i.ToString() );
		}

		// Retrieve the values.

		for (Int32 i = Keys - 1; i >= 0; i--)
		{
			String sKey = asKeys[i];

			Boolean bContainsKey = m_oMetadataProvider.ContainsKey(sKey);

			Assert.IsTrue(bContainsKey);

			Object oValue;

			Assert.IsTrue( m_oMetadataProvider.TryGetValue(sKey, out oValue) );

			Assert.IsTrue(oValue is String);

			Assert.AreEqual(sKey + i.ToString(), (String)oValue);
		}
    }

    //*************************************************************************
    //  Method: TestSetGetValue5()
    //
    /// <summary>
    /// Tests the SetValue(), TryGetValue(), and ContainsKey() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSetGetValue5()
    {
		// Add N keys for each of N MetadataProvider objects, ask for same keys
		// in reverse order.

		// Create an array of keys.

		const Int32 Keys = 10;

		String [] asKeys = new String[Keys];

		for (Int32 i = 0; i < Keys; i++)
		{
            asKeys[i] = Guid.NewGuid().ToString();
		}

		// Create an array of MetadataProvider objects.

		const Int32 MetadataProviderObjects = 10000;

		MetadataProvider [] aoMetadataProvider =
			new MetadataProvider[MetadataProviderObjects];

		for (Int32 j = 0; j < MetadataProviderObjects; j++)
		{
			aoMetadataProvider[j] = new MetadataProvider();
		}

		// Add a value for each key.  The value is just the key with appended
		// indexes.

		for (Int32 j = 0; j < MetadataProviderObjects; j++)
		{
			MetadataProvider oMetadataProvider = aoMetadataProvider[j];

			for (Int32 i = 0; i < Keys; i++)
			{
				String sKey = asKeys[i];

				oMetadataProvider.SetValue(
					sKey, sKey + i.ToString() + j.ToString() );
			}
		}

		// Retrieve the values.

        Boolean bContainsKey;

		for (Int32 j = MetadataProviderObjects - 1; j >= 0; j--)
		{
			MetadataProvider oMetadataProvider = aoMetadataProvider[j];

			for (Int32 i = Keys - 1; i >= 0; i--)
			{
				String sKey = asKeys[i];

				bContainsKey = oMetadataProvider.ContainsKey(sKey);

				Assert.IsTrue(bContainsKey);

				Object oValue;

				Assert.IsTrue( oMetadataProvider.TryGetValue(
					sKey, out oValue) );

				Assert.IsTrue(oValue is String);

				Assert.AreEqual(sKey + i.ToString() + j.ToString(),
					(String)oValue);
			}

			// Ask for a non-existent value.

			bContainsKey = oMetadataProvider.ContainsKey("nHnHn");

			Assert.IsFalse(bContainsKey);
		}

		// Create another MetadataProvider object and verify that it contains
		// no keys.

		MetadataProvider oMetadataProviderNoKeys = new MetadataProvider();

		for (Int32 i = 0; i < Keys; i++)
		{
			String sKey = asKeys[i];

			bContainsKey = oMetadataProviderNoKeys.ContainsKey(sKey);

			Assert.IsFalse(bContainsKey);

			Object oValue;

			Assert.IsFalse( m_oMetadataProvider.TryGetValue(
				sKey, out oValue) );
		}
    }

    //*************************************************************************
    //  Method: TestSetGetValue6()
    //
    /// <summary>
    /// Tests the SetValue(), TryGetValue(), and ContainsKey() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSetGetValue6()
    {
		// Add random keys and tags for each of N MockMetadataProvider objects.

		// Create an array of MockMetadataProvider objects and set random keys
		// and tags on them.

		const Int32 MockMetadataProviderObjects = 10000;

		MockMetadataProvider [] aoMockMetadataProvider =
			new MockMetadataProvider[MockMetadataProviderObjects];

		for (Int32 i = 0; i < MockMetadataProviderObjects; i++)
		{
			MockMetadataProvider oMockMetadataProvider =
				aoMockMetadataProvider[i] = new MockMetadataProvider();

			MetadataUtil.SetRandomMetadata(
                oMockMetadataProvider, true, true, i);
		}

		// Check the values, backwards.

		for (Int32 i = MockMetadataProviderObjects - 1; i >= 0; i--)
		{
			MetadataUtil.CheckRandomMetadata(
                aoMockMetadataProvider[i], true, true, i);
		}
    }

    //*************************************************************************
    //  Method: TestSetGetValue7()
    //
    /// <summary>
    /// Tests the SetValue(), TryGetValue(), and ContainsKey() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSetGetValue7()
    {
		// Add N keys and tag, all with null values.

		// Create an array of keys.

		const Int32 Keys = 1000;

		String [] asKeys = new String[Keys];

		for (Int32 i = 0; i < Keys; i++)
		{
            asKeys[i] = Guid.NewGuid().ToString();
		}

		// Add a null value for each key.

		for (Int32 i = 0; i < Keys; i++)
		{
			String sKey = asKeys[i];

			m_oMetadataProvider.SetValue(sKey, null);
		}

		// Add a null Tag.

		m_oMetadataProvider.Tag = null;

		// Retrieve the values.

		for (Int32 i = 0; i < Keys; i++)
		{
			String sKey = asKeys[i];

			Boolean bContainsKey = m_oMetadataProvider.ContainsKey(sKey);

			Assert.IsTrue(bContainsKey);

			Object oValue;

			Assert.IsTrue(m_oMetadataProvider.TryGetValue(sKey, out oValue) );

			Assert.IsNull(oValue);
		}

		Assert.IsNull(m_oMetadataProvider.Tag);
    }

    //*************************************************************************
    //  Method: TestSetGetValue8()
    //
    /// <summary>
    /// Tests the SetValue(), TryGetValue(), and ContainsKey() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSetGetValue8()
    {
		// Add N keys, set the same keys again, ask for same keys.

		// Create an array of keys.

		const Int32 Keys = 1000;

		String [] asKeys = new String[Keys];

		for (Int32 i = 0; i < Keys; i++)
		{
            asKeys[i] = Guid.NewGuid().ToString();
		}

		// Add a value for each key.  The value is just the key with an
		// appended index.

		for (Int32 i = 0; i < Keys; i++)
		{
			String sKey = asKeys[i];

			m_oMetadataProvider.SetValue( sKey, sKey + i.ToString() );
		}

		// Set the same value again.

		for (Int32 i = 0; i < Keys; i++)
		{
			String sKey = asKeys[i];

			m_oMetadataProvider.SetValue( sKey, sKey + i.ToString() );
		}

		// Set the same value again, in reverse order.  (This is to catch a
		// LinkedList-related bug in an earlier implementation.)

		for (Int32 i = Keys - 1;  i >= 0; i--)
		{
			String sKey = asKeys[i];

			m_oMetadataProvider.SetValue( sKey, sKey + i.ToString() );
		}

		// Retrieve the values.

		for (Int32 i = 0; i < Keys; i++)
		{
			String sKey = asKeys[i];

			Boolean bContainsKey = m_oMetadataProvider.ContainsKey(sKey);

			Assert.IsTrue(bContainsKey);

			Object oValue;

			Assert.IsTrue( m_oMetadataProvider.TryGetValue(sKey, out oValue) );

			Assert.IsTrue(oValue is String);

			Assert.AreEqual(sKey + i.ToString(), (String)oValue);
		}
    }

    //*************************************************************************
    //  Method: TestCopyTo()
    //
    /// <summary>
    /// Tests the CopyTo() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCopyTo()
    {
		// Don't set any properties on m_oMetadataProvider.

		m_oMetadataProvider.CopyTo(m_oCopy, false, false);

		Assert.IsNull(m_oCopy.Tag);
    }

    //*************************************************************************
    //  Method: TestCopyTo2()
    //
    /// <summary>
    /// Tests the CopyTo() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCopyTo2()
    {
		// Set the Tag on m_oMetadataProvider, but don't copy it.

		const String Tag = "This is the tag.";

		m_oMetadataProvider.Tag = Tag;

		m_oMetadataProvider.CopyTo(m_oCopy, false, false);

		Assert.IsNull(m_oCopy.Tag);
    }

    //*************************************************************************
    //  Method: TestCopyTo3()
    //
    /// <summary>
    /// Tests the CopyTo() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCopyTo3()
    {
		// Set the Tag on m_oMetadataProvider and copy it.

		const String Tag = "This is the tag.";

		m_oMetadataProvider.Tag = Tag;

		m_oMetadataProvider.CopyTo(m_oCopy, false, true);

		Assert.AreEqual(Tag, m_oCopy.Tag);
    }

    //*************************************************************************
    //  Method: TestCopyTo4()
    //
    /// <summary>
    /// Tests the CopyTo() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCopyTo4()
    {
		// Don't copy metadata values, don't copy the Tag.

		TestCopyTo(false, false);
    }

    //*************************************************************************
    //  Method: TestCopyTo5()
    //
    /// <summary>
    /// Tests the CopyTo() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCopyTo5()
    {
		// Don't copy metadata values, copy the Tag.

		TestCopyTo(false, true);
    }

    //*************************************************************************
    //  Method: TestCopyTo6()
    //
    /// <summary>
    /// Tests the CopyTo() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCopyTo6()
    {
		// Copy metadata values, don't copy the Tag.

		TestCopyTo(true, false);
    }

    //*************************************************************************
    //  Method: TestCopyTo7()
    //
    /// <summary>
    /// Tests the CopyTo() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCopyTo7()
    {
		// Copy metadata values, copy the Tag.

		TestCopyTo(true, true);
    }

    //*************************************************************************
    //  Method: TestCopyTo8()
    //
    /// <summary>
    /// Tests the CopyTo() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCopyTo8()
    {
		// Create N objects, set random metadata on each object, copy each
		// object's metadata to a new object, check metadata on new object.

        const Int32 Objects = 1976;

		MockMetadataProvider [] aoMockMetadataProvider =
			new MockMetadataProvider[Objects];

		// Set random values on each object.

		for (Int32 i = 0; i < Objects; i++)
		{
			MockMetadataProvider oMockMetadataProvider =
				aoMockMetadataProvider[i] = new MockMetadataProvider();

			MetadataUtil.SetRandomMetadata(
                oMockMetadataProvider, true, true, i);
		}

		for (Int32 i = 0; i < Objects; i++)
		{
			// Copy the object's metadata to a new object.

			MockMetadataProvider oMockMetadataProvider =
				aoMockMetadataProvider[i];

			MockMetadataProvider oCopy = new MockMetadataProvider();

			oMockMetadataProvider.CopyTo(oCopy, true, true);

			// Check the metadata on the new object.

			MetadataUtil.CheckRandomMetadata(oCopy, true, true, i);
		}
    }

    //*************************************************************************
    //  Method: TestAppendToString()
    //
    /// <summary>
    /// Tests the AppendToString() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAppendToString()
    {
		// Set some key/value pairs and the Tag on m_oMetadataProvider.

		const Int32 Tag = -29;

		const String Key1 = "xyz";
		const Int32 Value1 = 123456;

		const String Key2 = "xx8372";
		String Value2 = "jkfdekeikd";

		const String Key3 = "kjfdke";
		Object Value3 = null;

		m_oMetadataProvider.SetValue(Key1, Value1);
		m_oMetadataProvider.SetValue(Key2, Value2);
		m_oMetadataProvider.SetValue(Key3, Value3);

		m_oMetadataProvider.Tag = Tag;

		StringBuilder oStringBuilder = new StringBuilder();

        m_oMetadataProvider.AppendToString(oStringBuilder, 0, "G");

        String sExpectedValue =
            "3 key/value pairs";

        Assert.AreEqual( sExpectedValue, oStringBuilder.ToString() );

		oStringBuilder = new StringBuilder();

        m_oMetadataProvider.AppendToString(oStringBuilder, 0, "P");

        sExpectedValue =
            "3 key/value pairs\r\n";

        Assert.AreEqual( sExpectedValue, oStringBuilder.ToString() );

		oStringBuilder = new StringBuilder();

        m_oMetadataProvider.AppendToString(oStringBuilder, 0, "D");

        sExpectedValue =
            "3 key/value pairs\r\n"
			+ "\tKey = xyz, Value = 123456\r\n"
			+ "\tKey = xx8372, Value = jkfdekeikd\r\n"
			+ "\tKey = kjfdke, Value = [null]\r\n"
			;

        Assert.AreEqual( sExpectedValue, oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: TestRemoveKey()
    //
    /// <summary>
    /// Tests the RemoveKey() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRemoveKey()
    {
		// No keys.

		Boolean bRemovedKey = m_oMetadataProvider.RemoveKey("abc");

		Assert.IsFalse(bRemovedKey);
    }

    //*************************************************************************
    //  Method: TestRemoveKey2()
    //
    /// <summary>
    /// Tests the RemoveKey() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRemoveKey2()
    {
		// Has key, but not the specified one.

		const String Key1 = "xyz";
		const Int32 Value1 = 123456;

		m_oMetadataProvider.SetValue(Key1, Value1);

		Boolean bRemovedKey = m_oMetadataProvider.RemoveKey("kjre");

		Assert.IsFalse(bRemovedKey);
    }

    //*************************************************************************
    //  Method: TestRemoveKey3()
    //
    /// <summary>
    /// Tests the RemoveKey() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRemoveKey3()
    {
		// Has key, the specified one.

		const String Key1 = "xyz";
		const Int32 Value1 = 123456;

		m_oMetadataProvider.SetValue(Key1, Value1);

		Boolean bRemovedKey = m_oMetadataProvider.RemoveKey(Key1);

		Assert.IsTrue(bRemovedKey);
    }

    //*************************************************************************
    //  Method: TestRemoveKey4()
    //
    /// <summary>
    /// Tests the RemoveKey() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRemoveKey4()
    {
		// Create N objects, set metadata on each object, remove keys one by
		// one.

        const Int32 Objects = 10;

		const Int32 Keys = 5;

		MetadataProvider [] aoMetadataProvider = new MetadataProvider[Objects];

		// Set values on each object.

		for (Int32 i = 0; i < Objects; i++)
		{
			MetadataProvider oMetadataProvider =
				aoMetadataProvider[i] = new MetadataProvider();

			for (Int32 j = 0; j < Keys; j++)
			{
				oMetadataProvider.SetValue(j.ToString(), "string");
			}
		}

		// Remove the keys from each object.

		for (Int32 i = 0; i < Objects; i++)
		{
			MetadataProvider oMetadataProvider = aoMetadataProvider[i];

			for (Int32 j = 0; j < Keys; j++)
			{
				Boolean bRemovedKey =
					oMetadataProvider.RemoveKey( j.ToString() );

				Assert.IsTrue(bRemovedKey);

				// Check the status of all the keys.

				for (Int32 I = 0; I < Objects; I++)
				{
					MetadataProvider oMetadataProvider2 = aoMetadataProvider[I];

					for (Int32 J = 0; J < Keys; J++)
					{
						Boolean bContainsKey =
							oMetadataProvider2.ContainsKey( J.ToString() );

						if (I < i)
						{
							Assert.IsFalse(bContainsKey);
						}
						else if (I == i)
						{
							if (J <= j)
							{
								Assert.IsFalse(bContainsKey);
							}
							else
							{
								Assert.IsTrue(bContainsKey);
							}
						}
						else
						{
							Assert.IsTrue(bContainsKey);
						}
					}
				}
			}
		}
    }

    //*************************************************************************
    //  Method: TestRemoveKey5()
    //
    /// <summary>
    /// Tests the RemoveKey() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRemoveKey5()
    {
		// Create N objects, set metadata on each object, remove keys one by
		// one in backwards order.

        const Int32 Objects = 10;

		const Int32 Keys = 5;

		MetadataProvider [] aoMetadataProvider = new MetadataProvider[Objects];

		// Set values on each object.

		for (Int32 i = 0; i < Objects; i++)
		{
			MetadataProvider oMetadataProvider =
				aoMetadataProvider[i] = new MetadataProvider();

			for (Int32 j = 0; j < Keys; j++)
			{
				oMetadataProvider.SetValue(j.ToString(), "string");
			}
		}

		// Remove the keys from each object.

		for (Int32 i = Objects - 1; i >= 0; i--)
		{
			MetadataProvider oMetadataProvider = aoMetadataProvider[i];

			for (Int32 j = Keys - 1; j >= 0; j--)
			{
				Boolean bRemovedKey =
					oMetadataProvider.RemoveKey( j.ToString() );

				Assert.IsTrue(bRemovedKey);

				// Check the status of all the keys.

				for (Int32 I = 0; I < Objects; I++)
				{
					MetadataProvider oMetadataProvider2 = aoMetadataProvider[I];

					for (Int32 J = 0; J < Keys; J++)
					{
						Boolean bContainsKey =
							oMetadataProvider2.ContainsKey( J.ToString() );

						if (I < i)
						{
							Assert.IsTrue(bContainsKey);
						}
						else if (I == i)
						{
							if (J < j)
							{
								Assert.IsTrue(bContainsKey);
							}
							else
							{
								Assert.IsFalse(bContainsKey);
							}
						}
						else
						{
							Assert.IsFalse(bContainsKey);
						}
					}
				}
			}
		}
    }

    //*************************************************************************
    //  Method: TestClearMetadata()
    //
    /// <summary>
    /// Tests the ClearMetadata() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestClearMetadata()
    {
		// Add tags and N keys for each of N MetadataProvider objects, ask
		// for same tags and keys, clear metadata for each object.

		// Create an array of keys.

		const Int32 Keys = 10;

		String [] asKeys = new String[Keys];

		for (Int32 i = 0; i < Keys; i++)
		{
            asKeys[i] = Guid.NewGuid().ToString();
		}

		// Create an array of MetadataProvider objects.

		const Int32 MetadataProviderObjects = 100;

		MetadataProvider [] aoMetadataProvider =
			new MetadataProvider[MetadataProviderObjects];

		for (Int32 j = 0; j < MetadataProviderObjects; j++)
		{
			aoMetadataProvider[j] = new MetadataProvider();
		}

		// Add a Tag and a value for each key.  The value is just the key with
		// appended indexes.

		for (Int32 j = 0; j < MetadataProviderObjects; j++)
		{
			MetadataProvider oMetadataProvider = aoMetadataProvider[j];

			oMetadataProvider.Tag = j.ToString() + "Tag";

			for (Int32 i = 0; i < Keys; i++)
			{
				String sKey = asKeys[i];

				oMetadataProvider.SetValue(
					sKey, sKey + i.ToString() + j.ToString() );
			}
		}

		// Retrieve the values.

        Boolean bContainsKey;

		for (Int32 j = 0; j < MetadataProviderObjects; j++)
		{
			MetadataProvider oMetadataProvider = aoMetadataProvider[j];

			Assert.AreEqual(
				j.ToString() + "Tag",
				oMetadataProvider.Tag
				);

			for (Int32 i = Keys - 1; i >= 0; i--)
			{
				String sKey = asKeys[i];

				bContainsKey = oMetadataProvider.ContainsKey(sKey);

				Assert.IsTrue(bContainsKey);

				Object oValue;

				Assert.IsTrue( oMetadataProvider.TryGetValue(
					sKey, out oValue) );

				Assert.IsTrue(oValue is String);

				Assert.AreEqual(sKey + i.ToString() + j.ToString(),
					(String)oValue);
			}

			// Ask for a non-existent value.

			bContainsKey = oMetadataProvider.ContainsKey("nHnHn");

			Assert.IsFalse(bContainsKey);
		}

		// Remove metadata for each object.

		for (Int32 k = 0; k < MetadataProviderObjects; k++)
		{
			aoMetadataProvider[k].ClearMetadata();

			// Check all the objects, some of which will have no metadata.

			for (Int32 j = 0; j < MetadataProviderObjects; j++)
			{
				MetadataProvider oMetadataProvider = aoMetadataProvider[j];

				if (j > k)
				{
					Assert.AreEqual(
						j.ToString() + "Tag",
						oMetadataProvider.Tag
						);
				}
				else
				{
					Assert.IsNull(oMetadataProvider.Tag);
				}

				for (Int32 i = Keys - 1; i >= 0; i--)
				{
					String sKey = asKeys[i];

					bContainsKey = oMetadataProvider.ContainsKey(sKey);

					if (j > k)
					{
						Assert.IsTrue(bContainsKey);

						Object oValue;
						
						Assert.IsTrue( oMetadataProvider.TryGetValue(
							sKey, out oValue) );

						Assert.IsTrue(oValue is String);

						Assert.AreEqual(sKey + i.ToString() + j.ToString(),
							(String)oValue);
					}
					else
					{
						Assert.IsFalse(bContainsKey);
					}
				}
			}
		}
    }

    //*************************************************************************
    //  Method: TestCopyTo()
    //
    /// <summary>
    /// Tests the CopyTo() method.
    /// </summary>
	///
	/// <param name="bCopyMetadataValues">
	/// true to copy the metadata values.
	/// </param>
	///
	/// <param name="bCopyTag">
	/// true to copy the Tag.
	/// </param>
    //*************************************************************************

    protected void
    TestCopyTo
	(
		Boolean bCopyMetadataValues,
		Boolean bCopyTag
	)
    {
		// Set some key/value pairs and the Tag on m_oMetadataProvider.

		const Int32 Tag = -29;

		const String Key1 = "xyz";
		const Int32 Value1 = 123456;

		const String Key2 = "xx8372";
		DateTime Value2 = DateTime.Now;

		m_oMetadataProvider.SetValue(Key1, Value1);
		m_oMetadataProvider.SetValue(Key2, Value2);

		m_oMetadataProvider.Tag = Tag;

		m_oMetadataProvider.CopyTo(m_oCopy, bCopyMetadataValues, bCopyTag);

		Object oValue;

		if (bCopyMetadataValues)
		{
			Assert.IsTrue( m_oCopy.ContainsKey(Key1) );

			Assert.IsTrue( m_oCopy.TryGetValue(
				Key1, typeof(Int32), out oValue) );

			Assert.AreEqual(Value1, oValue);

			Assert.IsTrue( m_oCopy.ContainsKey(Key2) );

			Assert.IsTrue( m_oCopy.TryGetValue(
				Key2, typeof(DateTime), out oValue) );

			Assert.AreEqual(Value2, oValue);
		}
		else
		{
			Assert.IsFalse( m_oCopy.ContainsKey(Key1) );
			Assert.IsFalse( m_oCopy.ContainsKey(Key2) );
		}

		if (bCopyTag)
		{
			Assert.AreEqual(Tag, m_oCopy.Tag);
		}
		else
		{
			Assert.IsNull(m_oCopy.Tag);
		}
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Object under test.

	protected MetadataProvider m_oMetadataProvider;

	/// Mock class that implements IMetadataProvider, used by tests that copy
	/// metadata.

	private MockMetadataProvider m_oCopy;
}

}
