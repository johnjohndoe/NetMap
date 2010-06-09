
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.NodeXL.Adapters;

namespace Microsoft.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: GraphMLAttributeTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="GraphMLAttribute" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class GraphMLAttributeTest : Object
{
    //*************************************************************************
    //  Constructor: GraphMLAttributeTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMLAttributeTest" />
    /// class.
    /// </summary>
    //*************************************************************************

    public GraphMLAttributeTest()
    {
        m_oXmlDocument = null;
        m_oXmlNamespaceManager = null;
        m_oGraphMLAttribute = null;
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
        m_oXmlDocument = new XmlDocument();

        m_oXmlNamespaceManager = new XmlNamespaceManager(
            m_oXmlDocument.NameTable);

        m_oXmlNamespaceManager.AddNamespace(GraphMLPrefix,
            GraphMLGraphAdapter.GraphMLUri);
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
        m_oXmlDocument = null;
        m_oXmlNamespaceManager = null;
        m_oGraphMLAttribute = null;
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
        // Simple, for vertex.

        m_oGraphMLAttribute = CreateGraphMLAttribute(true, "boolean", null);

        Assert.AreEqual(ID, m_oGraphMLAttribute.ID);
        Assert.AreEqual(Name, m_oGraphMLAttribute.Name);
        Assert.AreEqual(true, m_oGraphMLAttribute.IsForVertex);
    }

    //*************************************************************************
    //  Method: TestConstructor2()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestConstructor2()
    {
        // Simple, for edge.

        m_oGraphMLAttribute = CreateGraphMLAttribute(false, "boolean", null);

        Assert.AreEqual(ID, m_oGraphMLAttribute.ID);
        Assert.AreEqual(Name, m_oGraphMLAttribute.Name);
        Assert.AreEqual(false, m_oGraphMLAttribute.IsForVertex);
    }

    //*************************************************************************
    //  Method: TestConstructorBad()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestConstructorBad()
    {
        // Default value is of wrong type

        try
        {
            m_oGraphMLAttribute = CreateGraphMLAttribute(false, "boolean",
                "x");
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The default value specified for the \"key\" XML node with the"
                + " id \"The ID\" is not of the specified type."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: TestGetAttributeValue()
    //
    /// <summary>
    /// Tests the GetAttributeValue() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetAttributeValue()
    {
        // Boolean, no default value.

        m_oGraphMLAttribute = CreateGraphMLAttribute(false, "boolean", null);

        XmlElement oDataXmlNode = CreateDataXmlNode("true");

        Object oAttributeValue =
            m_oGraphMLAttribute.GetAttributeValue(oDataXmlNode);

        Assert.IsTrue(oAttributeValue is Boolean);
        Assert.AreEqual(true, (Boolean)oAttributeValue);

        Object oDefaultAttributeValue;

        Assert.IsFalse( m_oGraphMLAttribute.TryGetDefaultAttributeValue(
            out oDefaultAttributeValue) );
    }

    //*************************************************************************
    //  Method: TestGetAttributeValue2()
    //
    /// <summary>
    /// Tests the GetAttributeValue() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetAttributeValue2()
    {
        // Boolean, has default value.

        m_oGraphMLAttribute = CreateGraphMLAttribute(false, "boolean", "true");

        XmlElement oDataXmlNode = CreateDataXmlNode("false");

        Object oAttributeValue =
            m_oGraphMLAttribute.GetAttributeValue(oDataXmlNode);

        Assert.IsTrue(oAttributeValue is Boolean);
        Assert.AreEqual(false, (Boolean)oAttributeValue);

        Object oDefaultAttributeValue;

        Assert.IsTrue( m_oGraphMLAttribute.TryGetDefaultAttributeValue(
            out oDefaultAttributeValue) );

        Assert.IsTrue(oDefaultAttributeValue is Boolean);
        Assert.AreEqual(true, (Boolean)oDefaultAttributeValue);
    }

    //*************************************************************************
    //  Method: TestGetAttributeValue3()
    //
    /// <summary>
    /// Tests the GetAttributeValue() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetAttributeValue3()
    {
        // Int, no default value.

        m_oGraphMLAttribute = CreateGraphMLAttribute(false, "int", null);

        XmlElement oDataXmlNode = CreateDataXmlNode("123");

        Object oAttributeValue =
            m_oGraphMLAttribute.GetAttributeValue(oDataXmlNode);

        Assert.IsTrue(oAttributeValue is Int32);
        Assert.AreEqual(123, (Int32)oAttributeValue);

        Object oDefaultAttributeValue;

        Assert.IsFalse( m_oGraphMLAttribute.TryGetDefaultAttributeValue(
            out oDefaultAttributeValue) );
    }

    //*************************************************************************
    //  Method: TestGetAttributeValue4()
    //
    /// <summary>
    /// Tests the GetAttributeValue() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetAttributeValue4()
    {
        // Int, has default value.

        m_oGraphMLAttribute = CreateGraphMLAttribute(false, "int", "-123");

        XmlElement oDataXmlNode = CreateDataXmlNode("456");

        Object oAttributeValue =
            m_oGraphMLAttribute.GetAttributeValue(oDataXmlNode);

        Assert.IsTrue(oAttributeValue is Int32);
        Assert.AreEqual(456, (Int32)oAttributeValue);

        Object oDefaultAttributeValue;

        Assert.IsTrue( m_oGraphMLAttribute.TryGetDefaultAttributeValue(
            out oDefaultAttributeValue) );

        Assert.IsTrue(oDefaultAttributeValue is Int32);
        Assert.AreEqual(-123, (Int32)oDefaultAttributeValue );
    }

    //*************************************************************************
    //  Method: TestGetAttributeValue5()
    //
    /// <summary>
    /// Tests the GetAttributeValue() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetAttributeValue5()
    {
        // Long, no default value.

        m_oGraphMLAttribute = CreateGraphMLAttribute(false, "long", null);

        XmlElement oDataXmlNode = CreateDataXmlNode("123456789012345");

        Object oAttributeValue =
            m_oGraphMLAttribute.GetAttributeValue(oDataXmlNode);

        Assert.IsTrue(oAttributeValue is Int64);
        Assert.AreEqual(123456789012345, (Int64)oAttributeValue);

        Object oDefaultAttributeValue;

        Assert.IsFalse( m_oGraphMLAttribute.TryGetDefaultAttributeValue(
            out oDefaultAttributeValue) );
    }

    //*************************************************************************
    //  Method: TestGetAttributeValue6()
    //
    /// <summary>
    /// Tests the GetAttributeValue() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetAttributeValue6()
    {
        // Long, has default value.

        m_oGraphMLAttribute = CreateGraphMLAttribute(false, "long",
            "1234567890123");

        XmlElement oDataXmlNode = CreateDataXmlNode("912345678901234");

        Object oAttributeValue =
            m_oGraphMLAttribute.GetAttributeValue(oDataXmlNode);

        Assert.IsTrue(oAttributeValue is Int64);
        Assert.AreEqual(912345678901234, (Int64)oAttributeValue);

        Object oDefaultAttributeValue;

        Assert.IsTrue( m_oGraphMLAttribute.TryGetDefaultAttributeValue(
            out oDefaultAttributeValue) );

        Assert.IsTrue(oDefaultAttributeValue is Int64);
        Assert.AreEqual(1234567890123, (Int64)oDefaultAttributeValue );
    }

    //*************************************************************************
    //  Method: TestGetAttributeValue7()
    //
    /// <summary>
    /// Tests the GetAttributeValue() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetAttributeValue7()
    {
        // Float, no default value.

        m_oGraphMLAttribute = CreateGraphMLAttribute(false, "float", null);

        XmlElement oDataXmlNode = CreateDataXmlNode("123456.1");

        Object oAttributeValue =
            m_oGraphMLAttribute.GetAttributeValue(oDataXmlNode);

        Assert.IsTrue(oAttributeValue is Single);
        Assert.AreEqual(123456.1F, (Single)oAttributeValue);

        Object oDefaultAttributeValue;

        Assert.IsFalse( m_oGraphMLAttribute.TryGetDefaultAttributeValue(
            out oDefaultAttributeValue) );
    }

    //*************************************************************************
    //  Method: TestGetAttributeValue8()
    //
    /// <summary>
    /// Tests the GetAttributeValue() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetAttributeValue8()
    {
        // Float, has default value.

        m_oGraphMLAttribute = CreateGraphMLAttribute(false, "float",
            "908.0");

        XmlElement oDataXmlNode = CreateDataXmlNode("-1234.5");

        Object oAttributeValue =
            m_oGraphMLAttribute.GetAttributeValue(oDataXmlNode);

        Assert.IsTrue(oAttributeValue is Single);
        Assert.AreEqual(-1234.5F, (Single)oAttributeValue);

        Object oDefaultAttributeValue;

        Assert.IsTrue( m_oGraphMLAttribute.TryGetDefaultAttributeValue(
            out oDefaultAttributeValue) );

        Assert.IsTrue(oDefaultAttributeValue is Single);
        Assert.AreEqual(908.0F, (Single)oDefaultAttributeValue );
    }

    //*************************************************************************
    //  Method: TestGetAttributeValue9()
    //
    /// <summary>
    /// Tests the GetAttributeValue() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetAttributeValue9()
    {
        // Double, no default value.

        m_oGraphMLAttribute = CreateGraphMLAttribute(false, "double", null);

        XmlElement oDataXmlNode = CreateDataXmlNode("123456.1");

        Object oAttributeValue =
            m_oGraphMLAttribute.GetAttributeValue(oDataXmlNode);

        Assert.IsTrue(oAttributeValue is Double);
        Assert.AreEqual(123456.1, (Double)oAttributeValue);

        Object oDefaultAttributeValue;

        Assert.IsFalse( m_oGraphMLAttribute.TryGetDefaultAttributeValue(
            out oDefaultAttributeValue) );
    }

    //*************************************************************************
    //  Method: TestGetAttributeValue10()
    //
    /// <summary>
    /// Tests the GetAttributeValue() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetAttributeValue10()
    {
        // Double, has default value.

        m_oGraphMLAttribute = CreateGraphMLAttribute(false, "double",
            "908.0");

        XmlElement oDataXmlNode = CreateDataXmlNode("-1234.5");

        Object oAttributeValue =
            m_oGraphMLAttribute.GetAttributeValue(oDataXmlNode);

        Assert.IsTrue(oAttributeValue is Double);
        Assert.AreEqual(-1234.5, (Double)oAttributeValue);

        Object oDefaultAttributeValue;

        Assert.IsTrue( m_oGraphMLAttribute.TryGetDefaultAttributeValue(
            out oDefaultAttributeValue) );

        Assert.IsTrue(oDefaultAttributeValue is Double);
        Assert.AreEqual(908.0, (Double)oDefaultAttributeValue );
    }

    //*************************************************************************
    //  Method: TestGetAttributeValue11()
    //
    /// <summary>
    /// Tests the GetAttributeValue() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetAttributeValue11()
    {
        // String, no default value.

        m_oGraphMLAttribute = CreateGraphMLAttribute(false, "string", null);

        XmlElement oDataXmlNode = CreateDataXmlNode("the value");

        Object oAttributeValue =
            m_oGraphMLAttribute.GetAttributeValue(oDataXmlNode);

        Assert.IsTrue(oAttributeValue is String);
        Assert.AreEqual("the value", (String)oAttributeValue);

        Object oDefaultAttributeValue;

        Assert.IsFalse( m_oGraphMLAttribute.TryGetDefaultAttributeValue(
            out oDefaultAttributeValue) );
    }

    //*************************************************************************
    //  Method: TestGetAttributeValue12()
    //
    /// <summary>
    /// Tests the GetAttributeValue() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetAttributeValue12()
    {
        // String, has default value.

        m_oGraphMLAttribute = CreateGraphMLAttribute(false, "string",
            "the default");

        XmlElement oDataXmlNode = CreateDataXmlNode("the value");

        Object oAttributeValue =
            m_oGraphMLAttribute.GetAttributeValue(oDataXmlNode);

        Assert.IsTrue(oAttributeValue is String);
        Assert.AreEqual("the value", (String)oAttributeValue);

        Object oDefaultAttributeValue;

        Assert.IsTrue( m_oGraphMLAttribute.TryGetDefaultAttributeValue(
            out oDefaultAttributeValue) );

        Assert.IsTrue(oDefaultAttributeValue is String);
        Assert.AreEqual("the default", (String)oDefaultAttributeValue );
    }

    //*************************************************************************
    //  Method: TestGetAttributeValue13()
    //
    /// <summary>
    /// Tests the GetAttributeValue() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetAttributeValue13()
    {
        // String, has default node with missing default text.  This was found
        // in a file exported by yED.

        m_oGraphMLAttribute = CreateGraphMLAttribute(false, "string",
            String.Empty);

        XmlElement oDataXmlNode = CreateDataXmlNode("the value");

        Object oAttributeValue =
            m_oGraphMLAttribute.GetAttributeValue(oDataXmlNode);

        Assert.IsTrue(oAttributeValue is String);
        Assert.AreEqual("the value", (String)oAttributeValue);

        Object oDefaultAttributeValue;

        Assert.IsFalse( m_oGraphMLAttribute.TryGetDefaultAttributeValue(
            out oDefaultAttributeValue) );
    }

    //*************************************************************************
    //  Method: TestGetAttributeValueBad()
    //
    /// <summary>
    /// Tests the GetAttributeValue() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(XmlException) ) ]

    public void
    TestGetAttributeValueBad()
    {
        // Value is of wrong type.

        try
        {
            m_oGraphMLAttribute = CreateGraphMLAttribute(false, "int", null);

            XmlElement oDataXmlNode = CreateDataXmlNode("not int");

            Object oAttributeValue =
                m_oGraphMLAttribute.GetAttributeValue(oDataXmlNode);
        }
        catch (XmlException oXmlException)
        {
            Assert.AreEqual(
                "The GraphML-attribute value specified for a \"data\" XML node"
                + " with the key \"The ID\" is not of the specified type."
                ,
                oXmlException.Message
                );

            throw oXmlException;
        }
    }

    //*************************************************************************
    //  Method: CreateGraphMLAttribute()
    //
    /// <summary>
    /// Creates a GraphMLAttribute object.
    /// </summary>
    ///
    /// <param name="bIsForVertex">
    /// true if the attribute is for a vertex, false if it is for an edge.
    /// </param>
    ///
    /// <param name="sAttribute">
    /// One of the Graph-ML attribute types.
    /// </param>
    ///
    /// <param name="sDefaultAttributeValue">
    /// Default GraphML-attribute value, or null for no default.
    /// </param>
    //*************************************************************************

    protected GraphMLAttribute
    CreateGraphMLAttribute
    (
        Boolean bIsForVertex,
        String sAttributeType,
        String sDefaultAttributeValue
    )
    {
        XmlElement oKeyXmlNode = CreateKeyXmlNode(bIsForVertex,
            sAttributeType);

        if (sDefaultAttributeValue != null)
        {
            XmlElement oDefaultXmlNode = m_oXmlDocument.CreateElement(
                "default", GraphMLGraphAdapter.GraphMLUri);

            oDefaultXmlNode.InnerText = sDefaultAttributeValue;

            oKeyXmlNode.AppendChild(oDefaultXmlNode);
        }

        m_oGraphMLAttribute = new GraphMLAttribute(oKeyXmlNode,
            m_oXmlNamespaceManager, GraphMLPrefix);

        return (m_oGraphMLAttribute);
    }

    //*************************************************************************
    //  Method: CreateKeyXmlNode()
    //
    /// <summary>
    /// Creates a "key" XML node.
    /// </summary>
    ///
    /// <param name="bIsForVertex">
    /// true if the attribute is for a vertex, false if it is for an edge.
    /// </param>
    ///
    /// <param name="sAttribute">
    /// One of the Graph-ML attribute types.
    /// </param>
    //*************************************************************************

    protected XmlElement
    CreateKeyXmlNode
    (
        Boolean bIsForVertex,
        String sAttributeType
    )
    {
        XmlElement oKeyXmlNode = m_oXmlDocument.CreateElement(
            "key", GraphMLGraphAdapter.GraphMLUri);

        oKeyXmlNode.SetAttribute("id", ID);
        oKeyXmlNode.SetAttribute("attr.name", Name);
        oKeyXmlNode.SetAttribute("for", bIsForVertex ? "node" : "edge");
        oKeyXmlNode.SetAttribute("attr.type", sAttributeType);

        return (oKeyXmlNode);
    }

    //*************************************************************************
    //  Method: CreateDataXmlNode()
    //
    /// <summary>
    /// Creates a "data" XML node.
    /// </summary>
    ///
    /// <param name="sAttributeValue">
    /// GraphML-attribute value.
    /// </param>
    //*************************************************************************

    protected XmlElement
    CreateDataXmlNode
    (
        String sAttributeValue
    )
    {
        XmlElement oDataXmlNode = m_oXmlDocument.CreateElement(
            "data", GraphMLGraphAdapter.GraphMLUri);

        oDataXmlNode.SetAttribute("key", ID);
        oDataXmlNode.InnerText = sAttributeValue;

        return (oDataXmlNode);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

     /// GraphML prefix.

     protected const String GraphMLPrefix = "AnyPrefixWillDo";

     /// Attribute ID.

     protected const String ID = "The ID";

     /// Attribute name.

     protected const String Name = "The Name";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Document.

    protected XmlDocument m_oXmlDocument;

    /// Namespace manager.

    protected XmlNamespaceManager m_oXmlNamespaceManager;

    /// Object to test.

    protected GraphMLAttribute m_oGraphMLAttribute;
}

}
