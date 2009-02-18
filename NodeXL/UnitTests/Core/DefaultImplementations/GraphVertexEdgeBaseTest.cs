
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: GraphVertexEdgeBaseTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="GraphVertexEdgeBase" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class GraphVertexEdgeBaseTest : Object
{
    //*************************************************************************
    //  Constructor: GraphVertexEdgeBaseTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphVertexEdgeBaseTest" /> class.
    /// </summary>
    //*************************************************************************

    public GraphVertexEdgeBaseTest()
    {
        m_oGraphVertexEdgeBase = null;
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
        m_oGraphVertexEdgeBase = new MockGraphVertexEdgeBase();
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
        m_oGraphVertexEdgeBase = null;
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
        Assert.AreEqual(0, m_oGraphVertexEdgeBase.ID);

        Assert.IsNull(m_oGraphVertexEdgeBase.Name);

        Assert.IsNull(m_oGraphVertexEdgeBase.Tag);
    }

    //*************************************************************************
    //  Method: TestContainsKeyBad()
    //
    /// <summary>
    /// Tests the ContainsKey() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestContainsKeyBad()
    {
        // Null key.

        try
        {
            Boolean bContainsKey = m_oGraphVertexEdgeBase.ContainsKey(null);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.ContainsKey: key argument can't be"
                + " null.\r\n"
                + "Parameter name: key"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestContainsKeyBad2()
    //
    /// <summary>
    /// Tests the ContainsKey() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestContainsKeyBad2()
    {
        // Empty key.

        try
        {
            Boolean bContainsKey =
                m_oGraphVertexEdgeBase.ContainsKey(String.Empty);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.ContainsKey: key argument must have"
                + " a length greater than zero.\r\n"
                + "Parameter name: key"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestSetValueBad()
    //
    /// <summary>
    /// Tests the SetValue() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestSetValueBad()
    {
        // Null key.

        try
        {
            m_oGraphVertexEdgeBase.SetValue(null, "string");
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.SetValue: key argument can't be"
                + " null.\r\n"
                + "Parameter name: key"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestSetValueBad2()
    //
    /// <summary>
    /// Tests the SetValue() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestSetValueBad2()
    {
        // Empty key.

        try
        {
            m_oGraphVertexEdgeBase.SetValue(String.Empty, "string");
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.SetValue: key argument must have"
                + " a length greater than zero.\r\n"
                + "Parameter name: key"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestGetValueBad()
    //
    /// <summary>
    /// Tests the GetValue(String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestGetValueBad()
    {
        // Null key.

        try
        {
            Object oValue = m_oGraphVertexEdgeBase.GetValue(null);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.GetValue: key argument can't be"
                + " null.\r\n"
                + "Parameter name: key"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestGetValueBad2()
    //
    /// <summary>
    /// Tests the GetValue(String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestGetValueBad2()
    {
        // Empty key.

        try
        {
            Object oValue = m_oGraphVertexEdgeBase.GetValue(String.Empty);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.GetValue: key argument must have"
                + " a length greater than zero.\r\n"
                + "Parameter name: key"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestGetValue2_Bad()
    //
    /// <summary>
    /// Tests the GetValue(String, Type) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestGetValue2_Bad()
    {
        // Null key.

        try
        {
            Object oValue = m_oGraphVertexEdgeBase.GetValue(
                null, typeof(String) );
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.GetValue: key argument can't be"
                + " null.\r\n"
                + "Parameter name: key"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestGetValue2_Bad2()
    //
    /// <summary>
    /// Tests the GetValue(String, Type) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestGetValue2_Bad2()
    {
        // Empty key.

        try
        {
            Object oValue = m_oGraphVertexEdgeBase.GetValue(
                String.Empty, typeof(String) );
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.GetValue: key argument must have"
                + " a length greater than zero.\r\n"
                + "Parameter name: key"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestGetValue2_Bad3()
    //
    /// <summary>
    /// Tests the GetValue(String, Type) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestGetValue2_Bad3()
    {
        // Null valueType.

        try
        {
            Object oValue = m_oGraphVertexEdgeBase.GetValue("key", null);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.GetValue: valueType argument can't"
                + " be null.\r\n"
                + "Parameter name: valueType"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestGetValue2_Bad4()
    //
    /// <summary>
    /// Tests the GetValue(String, Type) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestGetValue2_Bad4()
    {
        // Wrong type.

        const String Key = "fdjkre";
        const Int32 Value = 123456;

        try
        {
            m_oGraphVertexEdgeBase.SetValue(Key, Value);

            Boolean bContainsKey = m_oGraphVertexEdgeBase.ContainsKey(Key);

            Assert.IsTrue(bContainsKey);

            Object oValue =
                m_oGraphVertexEdgeBase.GetValue( Key, typeof(DateTime) );
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.GetValue: The value with the"
                + " key \"fdjkre\" is of type System.Int32.  The expected type"
                + " is System.DateTime.\r\n"
                + "Parameter name: key"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestTryGetValueBad()
    //
    /// <summary>
    /// Tests the TryGetValue(String, out Object) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestTryGetValueBad()
    {
        // Null key.

        try
        {
            Object oValue;
            
            m_oGraphVertexEdgeBase.TryGetValue(null, out oValue);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.TryGetValue: key argument can't be"
                + " null.\r\n"
                + "Parameter name: key"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestTryGetValueBad2()
    //
    /// <summary>
    /// Tests the TryGetValue(String, out Object) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestTryGetValueBad2()
    {
        // Empty key.

        try
        {
            Object oValue;
            
            m_oGraphVertexEdgeBase.TryGetValue(String.Empty, out oValue);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.TryGetValue: key argument must have"
                + " a length greater than zero.\r\n"
                + "Parameter name: key"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestTryGetValue2_Bad()
    //
    /// <summary>
    /// Tests the GetValue(String, Type, out Object) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestTryGetValue2_Bad()
    {
        // Null key.

        try
        {
            Object oValue;
            
            m_oGraphVertexEdgeBase.TryGetValue(
                null, typeof(String), out oValue);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.TryGetValue: key argument can't be"
                + " null.\r\n"
                + "Parameter name: key"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestTryGetValue2_Bad2()
    //
    /// <summary>
    /// Tests the GetValue(String, Type, out Object) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestTryGetValue2_Bad2()
    {
        // Empty key.

        try
        {
            Object oValue;
            
            m_oGraphVertexEdgeBase.TryGetValue(
                String.Empty, typeof(String), out oValue);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.TryGetValue: key argument must have"
                + " a length greater than zero.\r\n"
                + "Parameter name: key"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestTryGetValue2_Bad3()
    //
    /// <summary>
    /// Tests the TryGetValue(String, Type, out Object) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestTryGetValue2_Bad3()
    {
        // Null valueType.

        try
        {
            Object oValue;
            
            m_oGraphVertexEdgeBase.TryGetValue("key", null, out oValue);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.TryGetValue: valueType argument"
                + " can't be null.\r\n"
                + "Parameter name: valueType"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestTryGetValue2_Bad4()
    //
    /// <summary>
    /// Tests the TryGetValue(String, Type, out Object) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestTryGetValue2_Bad4()
    {
        // Wrong type.

        const String Key = "fdjkre";
        const Int32 Value = 123456;

        try
        {
            m_oGraphVertexEdgeBase.SetValue(Key, Value);

            Boolean bContainsKey = m_oGraphVertexEdgeBase.ContainsKey(Key);

            Assert.IsTrue(bContainsKey);

            Object oValue;

            m_oGraphVertexEdgeBase.TryGetValue(
                Key, typeof(DateTime), out oValue);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.TryGetValue: The value with the"
                + " key \"fdjkre\" is of type System.Int32.  The expected type"
                + " is System.DateTime.\r\n"
                + "Parameter name: key"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestGetRequiredValueBad()
    //
    /// <summary>
    /// Tests the GetRequiredValue(String, Type) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestGetRequiredValueBad()
    {
        // Null key.

        try
        {
            Object oValue = m_oGraphVertexEdgeBase.GetRequiredValue(
                null, typeof(String) );
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.GetRequiredValue: key argument"
                + " can't be null.\r\n"
                + "Parameter name: key"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestGetRequiredValueBad2()
    //
    /// <summary>
    /// Tests the GetRequiredValue(String, Type) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestGetRequiredValueBad2()
    {
        // Empty key.

        try
        {
            Object oValue = m_oGraphVertexEdgeBase.GetRequiredValue(
                String.Empty, typeof(String) );
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.GetRequiredValue: key argument must"
                + " have a length greater than zero.\r\n"
                + "Parameter name: key"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestGetRequiredValueBad3()
    //
    /// <summary>
    /// Tests the GetRequiredValue(String, Type) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestGetRequiredValueBad3()
    {
        // Null valueType.

        try
        {
            Object oValue = m_oGraphVertexEdgeBase.GetRequiredValue(
                "key", null);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.GetRequiredValue: valueType"
                + " argument can't be null.\r\n"
                + "Parameter name: valueType"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestGetRequiredValueBad4()
    //
    /// <summary>
    /// Tests the GetRequiredValue(String, Type) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestGetRequiredValueBad4()
    {
        // Wrong type.

        const String Key = "fdjkre";
        const Int32 Value = 123456;

        try
        {
            m_oGraphVertexEdgeBase.SetValue(Key, Value);

            Boolean bContainsKey = m_oGraphVertexEdgeBase.ContainsKey(Key);

            Assert.IsTrue(bContainsKey);

            Object oValue = m_oGraphVertexEdgeBase.GetRequiredValue(
                Key, typeof(DateTime) );
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.GetRequiredValue: The value with"
                + " the key \"fdjkre\" is of type System.Int32.  The expected"
                + " type is System.DateTime.\r\n"
                + "Parameter name: key"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestGetRequiredValueBad5()
    //
    /// <summary>
    /// Tests the GetRequiredValue(String, Type) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestGetRequiredValueBad5()
    {
        // Missing value.

        try
        {
            Object oValue = m_oGraphVertexEdgeBase.GetRequiredValue(
                "xyz", typeof(DateTime) );
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.GetRequiredValue: A value with the"
                + " key \"xyz\" does not exist.\r\n"
                + "Parameter name: key"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
        }
    }

    //*************************************************************************
    //  Method: TestSetGetValue()
    //
    /// <summary>
    /// Tests the SetValue(), GetValue(), and ContainsKey() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSetGetValue()
    {
        // No keys.

        Boolean bContainsKey = m_oGraphVertexEdgeBase.ContainsKey("abc");

        Assert.IsFalse(bContainsKey);
    }

    //*************************************************************************
    //  Method: TestSetGetValue2()
    //
    /// <summary>
    /// Tests the SetValue(), GetValue(), and ContainsKey() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSetGetValue2()
    {
        // Add key, ask for a different key.

        m_oGraphVertexEdgeBase.SetValue("xyz", 123);

        Boolean bContainsKey = m_oGraphVertexEdgeBase.ContainsKey("abc");

        Assert.IsFalse(bContainsKey);
    }

    //*************************************************************************
    //  Method: TestSetGetValue3()
    //
    /// <summary>
    /// Tests the SetValue(), GetValue(), and ContainsKey() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSetGetValue3()
    {
        // Add key, ask for same key.

        const String Key = "fdjkre";
        const Int32 Value = 123456;

        m_oGraphVertexEdgeBase.SetValue(Key, Value);

        Boolean bContainsKey = m_oGraphVertexEdgeBase.ContainsKey(Key);

        Assert.IsTrue(bContainsKey);

        MetadataUtil.TestGetValue(m_oGraphVertexEdgeBase, Key, Value);
    }

    //*************************************************************************
    //  Method: TestSetGetValue4()
    //
    /// <summary>
    /// Tests the SetValue(), GetValue(), and ContainsKey() methods.
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

            m_oGraphVertexEdgeBase.SetValue( sKey, sKey + i.ToString() );
        }

        // Retrieve the values.

        for (Int32 i = Keys - 1; i >= 0; i--)
        {
            String sKey = asKeys[i];

            Boolean bContainsKey = m_oGraphVertexEdgeBase.ContainsKey(sKey);

            Assert.IsTrue(bContainsKey);

            MetadataUtil.TestGetValue( m_oGraphVertexEdgeBase, sKey, 
                sKey + i.ToString() );
        }
    }

    //*************************************************************************
    //  Method: TestSetGetValue5()
    //
    /// <summary>
    /// Tests the SetValue(), GetValue(), and ContainsKey() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSetGetValue5()
    {
        // Add N keys for each of N GraphVertexEdgeBase objects, ask for same
        // keys in reverse order.

        // Create an array of keys.

        const Int32 Keys = 10;

        String [] asKeys = new String[Keys];

        for (Int32 i = 0; i < Keys; i++)
        {
            asKeys[i] = Guid.NewGuid().ToString();
        }

        // Create an array of GraphVertexEdgeBase objects.

        const Int32 GraphVertexEdgeBaseObjects = 10000;

        GraphVertexEdgeBase [] aoGraphVertexEdgeBase =
            new GraphVertexEdgeBase[GraphVertexEdgeBaseObjects];

        for (Int32 j = 0; j < GraphVertexEdgeBaseObjects; j++)
        {
            aoGraphVertexEdgeBase[j] = new MockGraphVertexEdgeBase();
        }

        // Add a value for each key.  The value is just the key with appended
        // indexes.

        for (Int32 j = 0; j < GraphVertexEdgeBaseObjects; j++)
        {
            GraphVertexEdgeBase oGraphVertexEdgeBase =
                aoGraphVertexEdgeBase[j];

            for (Int32 i = 0; i < Keys; i++)
            {
                String sKey = asKeys[i];

                oGraphVertexEdgeBase.SetValue(
                    sKey, sKey + i.ToString() + j.ToString() );
            }
        }

        // Retrieve the values.

        Boolean bContainsKey;

        for (Int32 j = GraphVertexEdgeBaseObjects - 1; j >= 0; j--)
        {
            GraphVertexEdgeBase oGraphVertexEdgeBase =
                aoGraphVertexEdgeBase[j];

            for (Int32 i = Keys - 1; i >= 0; i--)
            {
                String sKey = asKeys[i];

                bContainsKey = oGraphVertexEdgeBase.ContainsKey(sKey);

                Assert.IsTrue(bContainsKey);

                MetadataUtil.TestGetValue( oGraphVertexEdgeBase, sKey, 
                    sKey + i.ToString() + j.ToString() );
            }

            // Ask for a non-existent value.

            bContainsKey = oGraphVertexEdgeBase.ContainsKey("nHnHn");

            Assert.IsFalse(bContainsKey);
        }

        // Create another GraphVertexEdgeBase object and verify that it
        // contains no keys.

        GraphVertexEdgeBase oGraphVertexEdgeBaseNoKeys =
            new MockGraphVertexEdgeBase();

        for (Int32 i = 0; i < Keys; i++)
        {
            String sKey = asKeys[i];

            bContainsKey = oGraphVertexEdgeBaseNoKeys.ContainsKey(sKey);

            Assert.IsFalse(bContainsKey);
        }
    }

    //*************************************************************************
    //  Method: TestSetGetValue6()
    //
    /// <summary>
    /// Tests the SetValue(), GetValue(), and ContainsKey() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSetGetValue6()
    {
        // Add random keys and tags for each of N GraphVertexEdgeBase objects.

        // Create an array of GraphVertexEdgeBase objects and set random keys
        // and tags on them.

        const Int32 GraphVertexEdgeBaseObjects = 10000;

        GraphVertexEdgeBase [] aoGraphVertexEdgeBase =
            new GraphVertexEdgeBase[GraphVertexEdgeBaseObjects];

        for (Int32 i = 0; i < GraphVertexEdgeBaseObjects; i++)
        {
            GraphVertexEdgeBase oGraphVertexEdgeBase =
                aoGraphVertexEdgeBase[i] = new MockGraphVertexEdgeBase();

            MetadataUtil.SetRandomMetadata(
                oGraphVertexEdgeBase, true, true, i);
        }

        // Check the values, backwards.

        for (Int32 i = GraphVertexEdgeBaseObjects - 1; i >= 0; i--)
        {
            MetadataUtil.CheckRandomMetadata(
                aoGraphVertexEdgeBase[i], true, true, i);
        }
    }

    //*************************************************************************
    //  Method: TestSetGetValue7()
    //
    /// <summary>
    /// Tests the SetValue(), GetValue(), and ContainsKey() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSetGetValue7()
    {
        // Add N keys and Tag, all with null values.

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

            m_oGraphVertexEdgeBase.SetValue(sKey, null);
        }

        // Add a null Tag.

        m_oGraphVertexEdgeBase.Tag = null;

        // Retrieve the values.

        for (Int32 i = 0; i < Keys; i++)
        {
            String sKey = asKeys[i];

            Boolean bContainsKey = m_oGraphVertexEdgeBase.ContainsKey(sKey);

            Assert.IsTrue(bContainsKey);

            MetadataUtil.TestGetValue( m_oGraphVertexEdgeBase, sKey, null);
        }

        Assert.IsNull(m_oGraphVertexEdgeBase.Tag);
    }

    //*************************************************************************
    //  Method: TestSetGetValue8()
    //
    /// <summary>
    /// Tests the SetValue(), GetValue(), and ContainsKey() methods.
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

            m_oGraphVertexEdgeBase.SetValue( sKey, sKey + i.ToString() );
        }

        // Set the same value again.

        for (Int32 i = 0; i < Keys; i++)
        {
            String sKey = asKeys[i];

            m_oGraphVertexEdgeBase.SetValue( sKey, sKey + i.ToString() );
        }

        // Set the same value again, in reverse order.  (This is to catch a
        // LinkedList-related bug in an earlier implementation.)

        for (Int32 i = Keys - 1;  i >= 0; i--)
        {
            String sKey = asKeys[i];

            m_oGraphVertexEdgeBase.SetValue( sKey, sKey + i.ToString() );
        }

        // Retrieve the values.

        for (Int32 i = 0; i < Keys; i++)
        {
            String sKey = asKeys[i];

            Boolean bContainsKey = m_oGraphVertexEdgeBase.ContainsKey(sKey);

            Assert.IsTrue(bContainsKey);

            MetadataUtil.TestGetValue( m_oGraphVertexEdgeBase, sKey,
                sKey + i.ToString() );
        }
    }

    //*************************************************************************
    //  Method: TestSetGetValue9()
    //
    /// <summary>
    /// Tests the SetValue(), GetValue(), and ContainsKey() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSetGetValue9()
    {
        // Use a key that starts with a tilde.  Although these are reserved for
        // internal use by NodeXL, they should not be prohibited by any of the
        // GraphVertexEdgeBase() methods because the client of those methods
        // may be a NodeXL method.

        const String Key = "~abcdefg";
        const String Value = "jkrejkrejke";

        Assert.IsFalse( m_oGraphVertexEdgeBase.ContainsKey(Key) );

        m_oGraphVertexEdgeBase.SetValue(Key, Value);

        Assert.IsTrue( m_oGraphVertexEdgeBase.ContainsKey(Key) );

        Assert.AreEqual( Value, (String)m_oGraphVertexEdgeBase.GetValue(Key) );

        Object oValue;

        Assert.IsTrue( m_oGraphVertexEdgeBase.TryGetValue(Key, out oValue) );

        Assert.IsInstanceOfType( oValue, typeof(String) );

        oValue = m_oGraphVertexEdgeBase.GetRequiredValue( Key, typeof(String) );

        Assert.IsInstanceOfType( oValue, typeof(String) );

        Assert.AreEqual(Value, (String)oValue);

        Assert.IsTrue( m_oGraphVertexEdgeBase.TryGetValue(
            Key, typeof(String), out oValue) );

        Assert.IsInstanceOfType( oValue, typeof(String) );

        Assert.AreEqual(Value, (String)oValue);

        Assert.IsTrue( m_oGraphVertexEdgeBase.RemoveKey(Key) );

        Assert.IsFalse( m_oGraphVertexEdgeBase.ContainsKey(Key) );
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

        Boolean bRemovedKey = m_oGraphVertexEdgeBase.RemoveKey("abc");

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

        m_oGraphVertexEdgeBase.SetValue(Key1, Value1);

        Boolean bRemovedKey = m_oGraphVertexEdgeBase.RemoveKey("kjre");

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

        m_oGraphVertexEdgeBase.SetValue(Key1, Value1);

        Boolean bRemovedKey = m_oGraphVertexEdgeBase.RemoveKey(Key1);

        Assert.IsTrue(bRemovedKey);
    }

    //*************************************************************************
    //  Method: TestRemoveKeyBad()
    //
    /// <summary>
    /// Tests the RemoveKey() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestRemoveKeyBad()
    {
        // Null key.

        try
        {
            m_oGraphVertexEdgeBase.RemoveKey(null);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.RemoveKey: key argument can't be"
                + " null.\r\n"
                + "Parameter name: key"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestRemoveKeyBad2()
    //
    /// <summary>
    /// Tests the RemoveKey() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestRemoveKeyBad2()
    {
        // Empty key.

        try
        {
            m_oGraphVertexEdgeBase.RemoveKey(String.Empty);
        }
        catch (ArgumentException oArgumentException)
        {
            Assert.AreEqual(

                "Microsoft.NodeXL.UnitTests."
                + "MockGraphVertexEdgeBase.RemoveKey: key argument must have"
                + " a length greater than zero.\r\n"
                + "Parameter name: key"
                ,
                oArgumentException.Message
                );

            throw oArgumentException;
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
        // Don't set any properties on m_oGraphVertexEdgeBase.

        GraphVertexEdgeBase oCopy = new MockGraphVertexEdgeBase();

        Int32 iID = oCopy.ID;

        m_oGraphVertexEdgeBase.CopyTo(oCopy, false, false);

        Assert.AreEqual(iID, oCopy.ID);
        Assert.IsNull(oCopy.Name);
        Assert.IsNull(oCopy.Tag);
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
        // Set the name on m_oGraphVertexEdgeBase.

        const String Name = "jkfdklerw";

        m_oGraphVertexEdgeBase.Name = Name;

        GraphVertexEdgeBase oCopy = new MockGraphVertexEdgeBase();

        Int32 iID = oCopy.ID;

        m_oGraphVertexEdgeBase.CopyTo(oCopy, false, false);

        Assert.AreEqual(iID, oCopy.ID);
        Assert.AreEqual(Name, oCopy.Name);
        Assert.IsNull(oCopy.Tag);
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
        // Set the Tag on m_oGraphVertexEdgeBase, but don't copy it.

        const String Tag = "This is the tag.";

        m_oGraphVertexEdgeBase.Tag = Tag;

        GraphVertexEdgeBase oCopy = new MockGraphVertexEdgeBase();

        Int32 iID = oCopy.ID;

        m_oGraphVertexEdgeBase.CopyTo(oCopy, false, false);

        Assert.AreEqual(iID, oCopy.ID);
        Assert.IsNull(oCopy.Name);
        Assert.IsNull(oCopy.Tag);
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
        // Set the Tag on m_oGraphVertexEdgeBase and copy it.

        const String Tag = "This is the tag.";

        m_oGraphVertexEdgeBase.Tag = Tag;

        GraphVertexEdgeBase oCopy = new MockGraphVertexEdgeBase();

        Int32 iID = oCopy.ID;

        m_oGraphVertexEdgeBase.CopyTo(oCopy, false, true);

        Assert.AreEqual(iID, oCopy.ID);
        Assert.IsNull(oCopy.Name);
        Assert.AreEqual(Tag, oCopy.Tag);
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
        // Don't copy metadata values, don't copy the Tag.

        TestCopyTo(false, false);
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
        // Don't copy metadata values, copy the Tag.

        TestCopyTo(false, true);
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
        // Copy metadata values, don't copy the Tag.

        TestCopyTo(true, false);
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
        // Copy metadata values, copy the Tag.

        TestCopyTo(true, true);
    }

    //*************************************************************************
    //  Method: TestCopyTo9()
    //
    /// <summary>
    /// Tests the CopyTo() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCopyTo9()
    {
        // Create N objects, set random metadata on each object, copy each
        // object's metadata to a new object, check metadata on new object.

        const Int32 Objects = 1976;

        MockGraphVertexEdgeBase [] aoGraphVertexEdgeBase =
            new MockGraphVertexEdgeBase[Objects];

        // Set random values on each object.

        for (Int32 i = 0; i < Objects; i++)
        {
            GraphVertexEdgeBase oGraphVertexEdgeBase =
                aoGraphVertexEdgeBase[i] = new MockGraphVertexEdgeBase();

            MetadataUtil.SetRandomMetadata(
                oGraphVertexEdgeBase, true, true, i);
        }

        for (Int32 i = 0; i < Objects; i++)
        {
            // Copy the object's metadata to a new object.

            MockGraphVertexEdgeBase oGraphVertexEdgeBase =
                aoGraphVertexEdgeBase[i];

            MockGraphVertexEdgeBase oCopy = new MockGraphVertexEdgeBase();

            oGraphVertexEdgeBase.CopyTo(oCopy, true, true);

            // Check the metadata on the new object.

            MetadataUtil.CheckRandomMetadata(oCopy, true, true, i);
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
        // Set some key/value pairs and the Tag on m_oGraphVertexEdgeBase.

        const Int32 Tag = -29;

        const String Key1 = "xyz";
        const Int32 Value1 = 123456;

        const String Key2 = "xx8372";
        DateTime Value2 = DateTime.Now;

        m_oGraphVertexEdgeBase.SetValue(Key1, Value1);
        m_oGraphVertexEdgeBase.SetValue(Key2, Value2);

        m_oGraphVertexEdgeBase.Tag = Tag;

        GraphVertexEdgeBase oCopy = new MockGraphVertexEdgeBase();

        Int32 iID = oCopy.ID;

        m_oGraphVertexEdgeBase.CopyTo(oCopy, bCopyMetadataValues, bCopyTag);

        Assert.AreEqual(iID, oCopy.ID);
        Assert.IsNull(oCopy.Name);

        if (bCopyMetadataValues)
        {
            Assert.IsTrue( oCopy.ContainsKey(Key1) );
            Assert.AreEqual(Value1, oCopy.GetValue( Key1, typeof(Int32) ) );

            Assert.IsTrue( oCopy.ContainsKey(Key2) );
            Assert.AreEqual(Value2, oCopy.GetValue( Key2, typeof(DateTime) ) );
        }
        else
        {
            Assert.IsFalse( oCopy.ContainsKey(Key1) );
            Assert.IsFalse( oCopy.ContainsKey(Key2) );
        }

        if (bCopyTag)
        {
            Assert.AreEqual(Tag, oCopy.Tag);
        }
        else
        {
            Assert.IsNull(oCopy.Tag);
        }
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    private MockGraphVertexEdgeBase m_oGraphVertexEdgeBase;
}

}
