
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Xml;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.XmlLib
{
//*****************************************************************************
//  Class: XmlUtil
//
/// <summary>
/// XML utility methods.
/// </summary>
///
/// <remarks>
/// This class contains utility methods for dealing with XML.  All methods are
/// static.
/// </remarks>
//*****************************************************************************

public class XmlUtil
{
    //*************************************************************************
    //  Constructor: XmlUtil()
    //
    /// <summary>
    /// Initializes a new instance of the XmlUtil class.
    /// </summary>
    ///
    /// <remarks>
    /// Do not instantiate an XmlUtil object.  All XmlUtil methods are static.
    /// </remarks>
    //*************************************************************************

    private
    XmlUtil()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: AppendNewNodeWithNamespace()
    //
    /// <summary>
    /// Creates a new XML node with an optional namespace and appends it to a
    /// parent node.
    /// </summary>
    ///
    /// <param name="oParentXmlNode">
    /// Node to append the new node to.
    /// </param>
    /// 
    /// <param name="sChildName">
    /// Name of the new node.
    /// </param>
    ///
    /// <param name="sNamespaceUri">
    /// Optional namespace URI of the new node.  If null or empty, no namespace
    /// is used.
    /// </param>
    ///
    /// <returns>
    /// The new node.
    /// </returns>
    //*************************************************************************

    static public XmlNode
    AppendNewNodeWithNamespace
    (
        XmlNode oParentXmlNode,
        String sChildName,
        String sNamespaceUri
    )
    {
        Debug.Assert(oParentXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sChildName) );

        // Get the owner document.

        XmlDocument oOwnerDocument = oParentXmlNode.OwnerDocument;

        // Unfortunately, the root node's OwnerDocument property returns null,
        // so we have to check for this special case.

        if (oOwnerDocument == null)
        {
            oOwnerDocument = (XmlDocument)oParentXmlNode;
        }

        XmlElement oNewNode;

        if ( String.IsNullOrEmpty(sNamespaceUri) )
        {
            oNewNode = oOwnerDocument.CreateElement(sChildName);
        }
        else
        {
            oNewNode = oOwnerDocument.CreateElement(sChildName, sNamespaceUri);
        }

        return ( oParentXmlNode.AppendChild(oNewNode) );
    }

    //*************************************************************************
    //  Method: AppendNewNode()
    //
    /// <overloads>
    /// Creates a new node and appends it to a parent node.
    /// </overloads>
    ///
    /// <summary>
    /// Creates a new node and appends it to a parent node.
    /// </summary>
    ///
    /// <param name="oParentNode">
    /// Node to append the new node to.
    /// </param>
    /// 
    /// <param name="sChildName">
    /// Name of the new node.
    /// </param>
    ///
    /// <returns>
    /// The new node.
    /// </returns>
    //*************************************************************************

    static public XmlNode
    AppendNewNode
    (
        XmlNode oParentNode,
        String sChildName
    )
    {
        Debug.Assert(oParentNode != null);
        Debug.Assert(sChildName != "");

        return ( AppendNewNodeWithNamespace(oParentNode, sChildName, null) );
    }

    //*************************************************************************
    //  Method: AppendNewNode()
    //
    /// <summary>
    /// Creates a new node, sets its inner text, and appends the new node to a
    /// parent node.
    /// </summary>
    ///
    /// <param name="oParentNode">
    /// Node to append a new node to.
    /// </param>
    /// 
    /// <param name="sChildName">
    /// Name of the new node.
    /// </param>
    ///
    /// <param name="sInnerText">
    /// Inner text of the new node.
    /// </param>
    ///
    /// <returns>
    /// The new node.
    /// </returns>
    //*************************************************************************

    static public XmlNode
    AppendNewNode
    (
        XmlNode oParentNode,
        String sChildName,
        String sInnerText
    )
    {
        Debug.Assert(oParentNode != null);
        Debug.Assert(sChildName != "");
        Debug.Assert(sInnerText != null);

        XmlNode oNewNode = AppendNewNode(oParentNode, sChildName);
        oNewNode.InnerText = sInnerText;

        return (oNewNode);
    }

    //*************************************************************************
    //  Method: SelectRequiredSingleNode()
    //
    /// <remarks>
    /// Selects a single node that must exist.
    /// </remarks>
    ///
    /// <summary>
    /// Selects a single node that must exist.
    /// </summary>
    ///
    /// <param name="oNode">
    /// Node to select from.
    /// </param>
    ///
    /// <param name="sXPath">
    /// XPath expression.
    /// </param>
    ///
    /// <returns>
    /// Selected node.
    /// </returns>
    ///
    /// <remarks>
    /// If the node doesn't exist, an exception is thrown.
    /// </remarks>
    //*************************************************************************

    public static XmlNode
    SelectRequiredSingleNode
    (
        XmlNode oNode,
        String sXPath
    )
    {
        Debug.Assert(oNode != null);
        Debug.Assert(sXPath != "");

        XmlNode oSelectedNode = oNode.SelectSingleNode(sXPath);

        if (oSelectedNode == null)
        {
            throw new XmlException(
                "A \"" + oNode.Name + "\" node is missing a required"
                + " descendent node.  The XPath is \"" + sXPath + "\".");
        }

        return (oSelectedNode);
    }

    //*************************************************************************
    //  Method: SelectRequiredSingleNode()
    //
    /// <summary>
    /// Selects a single node that must exist using a NamespaceManager.
    /// </summary>
    ///
    /// <param name="oNode">
    /// Node to select from.
    /// </param>
    ///
    /// <param name="sXPath">
    /// XPath expression.
    /// </param>
    ///
    /// <param name="oXmlNamespaceManager">
    /// NamespaceManager to use.
    /// </param>
    ///
    /// <returns>
    /// Selected node.
    /// </returns>
    ///
    /// <remarks>
    /// If the node doesn't exist, an exception is thrown.
    /// </remarks>
    //*************************************************************************

    public static XmlNode
    SelectRequiredSingleNode
    (
        XmlNode oNode,
        String sXPath,
        XmlNamespaceManager oXmlNamespaceManager
    )
    {
        Debug.Assert(oNode != null);
        Debug.Assert(sXPath != "");
        Debug.Assert(oXmlNamespaceManager != null);

        XmlNode oSelectedNode = oNode.SelectSingleNode(sXPath,
            oXmlNamespaceManager);

        if (oSelectedNode == null)
        {
            throw new XmlException(
                "A \"" + oNode.Name + "\" node is missing a required"
                + " descendent node.  The XPath is \"" + sXPath + "\".");
        }

        return (oSelectedNode);
    }

    //*************************************************************************
    //  Method: CheckNodeName()
    //
    /// <summary>
    /// Asserts if a node doesn't have the expected name.  Debug-only.
    /// </summary>
    ///
    /// <param name="oNode">
    /// Node to check.
    /// </param>
    ///
    /// <param name="sExpectedName">
    /// Expected name.
    /// </param>
    //*************************************************************************

    [Conditional("DEBUG")] 

    public static void
    CheckNodeName
    (
        XmlNode oNode,
        String sExpectedName
    )
    {
        Debug.Assert(oNode != null);
        Debug.Assert(oNode.Name == sExpectedName);
    }

    //*************************************************************************
    //  Method: GetInnerText()
    //
    /// <summary>
    /// Reads the inner text from an XmlNode.
    /// </summary>
    ///
    /// <param name="oNode">
    /// Node to read inner text from.
    /// </param>
    ///
    /// <param name="bRequired">
    /// true if the inner text is required.
    /// </param>
    ///
    /// <param name="sInnerText">
    /// Where the inner text gets stored.
    /// </param>
    ///
    /// <returns>
    /// true if the inner text was found and stored in sInnerText.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="bRequired" /> is true and the inner text is missing,
    /// an exception is thrown.  If <paramref name="bRequired" /> is false and
    /// the inner text is missing, an empty string is stored in <paramref
    /// name="sInnerText" /> and false is returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    GetInnerText
    (
        XmlNode oNode,
        Boolean bRequired,
        out String sInnerText
    )
    {
        Debug.Assert(oNode != null);

        // Get the inner text.

        sInnerText = oNode.InnerText;

        if (sInnerText == null || sInnerText.Trim().Length == 0)
        {
            // The inner text is missing.

            if (bRequired)
            {
                throw new XmlException("A \"" + oNode.Name + "\" node is"
                + " missing required inner text.");
            }

            return (false);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: GetStringNodeValue()
    //
    /// <summary>
    /// Reads an XML node's text.
    /// </summary>
    ///
    /// <param name="oNode">
    /// Node to select from.
    /// </param>
    ///
    /// <param name="sXPath">
    /// XPath expression.
    /// </param>
    ///
    /// <param name="bRequired">
    /// true if the specified node and non-empty text are required.
    /// </param>
    ///
    /// <param name="sValue">
    /// Where the text gets stored.
    /// </param>
    ///
    /// <returns>
    /// true if the specified node was found and its text stored in iValue.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="bRequired" /> is true and the specified node is
    /// missing, an exception is thrown.  If <paramref name="bRequired" /> is
    /// false and the specified node is missing, String.Empty is stored in
    /// sValue and false is returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    GetStringNodeValue
    (
        XmlNode oNode,
        String sXPath,
        Boolean bRequired,
        out String sValue
    )
    {
        Debug.Assert(oNode != null);
        Debug.Assert(sXPath != null);
        Debug.Assert(sXPath != "");

        sValue = String.Empty;

        XmlNode oSelectedNode;

        if (bRequired)
        {
            oSelectedNode = SelectRequiredSingleNode(oNode, sXPath);
        }
        else
        {
            oSelectedNode = oNode.SelectSingleNode(sXPath);

            if (oSelectedNode == null)
            {
                return (false);
            }
        }

        return ( GetInnerText(oSelectedNode, bRequired, out sValue) );
    }

    //*************************************************************************
    //  Method: GetInt32NodeValue()
    //
    /// <summary>
    /// Reads an XML node whose text is an Int32 string and converts the text
    /// to an Int32.
    /// </summary>
    ///
    /// <param name="oNode">
    /// Node to select from.
    /// </param>
    ///
    /// <param name="sXPath">
    /// XPath expression.
    /// </param>
    ///
    /// <param name="bRequired">
    /// true if the specified node and Int32 are required.
    /// </param>
    ///
    /// <param name="iValue">
    /// Where the converted text gets stored.
    /// </param>
    ///
    /// <returns>
    /// true if the specified node was found and its text stored in iValue.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="bRequired" /> is true and the specified node is
    /// missing, an exception is thrown.  If <paramref name="bRequired" /> is
    /// false and the specified node is missing, Int32.MinValue is stored in
    /// iValue and false is returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    GetInt32NodeValue
    (
        XmlNode oNode,
        String sXPath,
        Boolean bRequired,
        out Int32 iValue
    )
    {
        Debug.Assert(oNode != null);
        Debug.Assert(sXPath != null);
        Debug.Assert(sXPath != "");

        iValue = Int32.MinValue;
        String sValue;

        return (
            GetStringNodeValue(oNode, sXPath, bRequired, out sValue)
            &&
            Int32.TryParse(sValue, out iValue)
            );
    }

    //*************************************************************************
    //  Method: GetAttribute()
    //
    /// <summary>
    /// Reads an XML attribute from an XmlNode.
    /// </summary>
    ///
    /// <param name="oNode">
    /// Node to read attribute from.
    /// </param>
    ///
    /// <param name="sName">
    /// Attribute to read.
    /// </param>
    ///
    /// <param name="bRequired">
    /// true if the attribute is required.
    /// </param>
    ///
    /// <param name="sValue">
    /// Where the attribute value gets stored.
    /// </param>
    ///
    /// <returns>
    /// true if the attribute was found and stored in sValue.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="bRequired" /> is true and the attribute is missing,
    /// an exception is thrown.  If <paramref name="bRequired" /> is false and
    /// the attribute is missing, an empty string is stored in <paramref
    /// name="sValue" /> and false is returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    GetAttribute
    (
        XmlNode oNode,
        String sName,
        Boolean bRequired,
        out String sValue
    )
    {
        Debug.Assert(oNode != null);
        Debug.Assert(sName != null);
        Debug.Assert(sName != "");

        // Get the attribute.

        sValue = ( (XmlElement)oNode ).GetAttribute(sName);

        if (sValue == "")
        {
            // The attribute is missing.

            if (bRequired)
            {
                throw new XmlException("A \"" + oNode.Name + "\" node is"
                    + " missing a required \"" + sName + "\" attribute.");
            }

            return (false);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: GetInt32Attribute()
    //
    /// <summary>
    /// Reads an XML attribute that contains an Int32 string and converts it to
    /// an Int32.
    /// </summary>
    ///
    /// <param name="oNode">
    /// Node to read attribute from.
    /// </param>
    ///
    /// <param name="sName">
    /// Attribute to read.
    /// </param>
    ///
    /// <param name="bRequired">
    /// true if the attribute is required.
    /// </param>
    ///
    /// <param name="iValue">
    /// Where the converted attribute gets stored.
    /// </param>
    ///
    /// <returns>
    /// true if the attribute was found and stored in iValue.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="bRequired" /> is true and the attribute is missing,
    /// an exception is thrown.  If <paramref name="bRequired" /> is false and
    /// the attribute is missing, Int32.MinValue is stored in iValue and false
    /// is returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    GetInt32Attribute
    (
        XmlNode oNode,
        String sName,
        Boolean bRequired,
        out Int32 iValue
    )
    {
        Debug.Assert(oNode != null);
        Debug.Assert(sName != null);
        Debug.Assert(sName != "");

        // Get the attribute.

        String sValue;

        if ( !XmlUtil.GetAttribute(oNode, sName, bRequired, out sValue) )
        {
            // The attribute is missing but isn't required.  If it was
            // required, GetAttribute() threw an exception.

            iValue = Int32.MinValue;
            return (false);
        }

        // Convert the attribute to an Int32.

        try
        {
            iValue = Int32.Parse(sValue);
        }
        catch (Exception oException)
        {
            throw new XmlException(
                "Can't convert \"" + sValue + "\" from String to Int32.",
                oException);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: GetInt64Attribute()
    //
    /// <summary>
    /// Reads an XML attribute that contains an Int64 string and converts it to
    /// an Int64.
    /// </summary>
    ///
    /// <param name="oNode">
    /// Node to read attribute from.
    /// </param>
    ///
    /// <param name="sName">
    /// Attribute to read.
    /// </param>
    ///
    /// <param name="bRequired">
    /// true if the attribute is required.
    /// </param>
    ///
    /// <param name="i64Value">
    /// Where the converted attribute gets stored.
    /// </param>
    ///
    /// <returns>
    /// true if the attribute was found and stored in i64Value.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="bRequired" /> is true and the attribute is missing,
    /// an exception is thrown.  If <paramref name="bRequired" /> is false and
    /// the attribute is missing, Int64.MinValue is stored in i64Value and
    /// false is returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    GetInt64Attribute
    (
        XmlNode oNode,
        String sName,
        Boolean bRequired,
        out Int64 i64Value
    )
    {
        Debug.Assert(oNode != null);
        Debug.Assert(sName != null);
        Debug.Assert(sName != "");

        // Get the attribute.

        String sValue;

        if ( !XmlUtil.GetAttribute(oNode, sName, bRequired, out sValue) )
        {
            // The attribute is missing but isn't required.  If it was
            // required, GetAttribute() threw an exception.

            i64Value = Int64.MinValue;
            return (false);
        }

        // Convert the attribute to an Int64.

        try
        {
            i64Value = Int64.Parse(sValue);
        }
        catch (Exception oException)
        {
            throw new XmlException(
                "Can't convert \"" + sValue + "\" from String to Int64.",
                oException);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: GetSingleAttribute()
    //
    /// <summary>
    /// Reads an XML attribute that contains a Single string and converts it to
    /// a Single.
    /// </summary>
    ///
    /// <param name="oNode">
    /// Node to read attribute from.
    /// </param>
    ///
    /// <param name="sName">
    /// Attribute to read.
    /// </param>
    ///
    /// <param name="bRequired">
    /// true if the attribute is required.
    /// </param>
    ///
    /// <param name="fValue">
    /// Where the converted attribute gets stored.
    /// </param>
    ///
    /// <returns>
    /// true if the attribute was found and stored in fValue.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="bRequired" /> is true and the attribute is missing,
    /// an exception is thrown.  If <paramref name="bRequired" /> is false and
    /// the attribute is missing, Single.MinValue is stored in fValue and false
    /// is returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    GetSingleAttribute
    (
        XmlNode oNode,
        String sName,
        Boolean bRequired,
        out Single fValue
    )
    {
        Debug.Assert(oNode != null);
        Debug.Assert(sName != null);
        Debug.Assert(sName != "");

        // Get the attribute.

        String sValue;

        if ( !XmlUtil.GetAttribute(oNode, sName, bRequired, out sValue) )
        {
            // The attribute is missing but isn't required.  If it was
            // required, GetAttribute() threw an exception.

            fValue = Single.MinValue;
            return (false);
        }

        // Convert the attribute to a Single.

        try
        {
            fValue = Single.Parse(sValue);
        }
        catch (Exception oException)
        {
            throw new XmlException(
                "Can't convert \"" + sValue + "\" from String to Single.",
                oException);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: GetBooleanAttribute()
    //
    /// <summary>
    /// Reads an XML attribute that contains a string that is either "0" or "1"
    /// and converts it to a Boolean.
    /// </summary>
    ///
    /// <param name="oNode">
    /// Node to read attribute from.
    /// </param>
    ///
    /// <param name="sName">
    /// Attribute to read.
    /// </param>
    ///
    /// <param name="bRequired">
    /// true if the attribute is required.
    /// </param>
    ///
    /// <param name="bValue">
    /// Where the converted attribute gets stored.
    /// </param>
    ///
    /// <returns>
    /// true if the attribute was found and stored in bValue.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="bRequired" /> is true and the attribute is missing,
    /// an exception is thrown.  If <paramref name="bRequired" /> is false and
    /// the attribute is missing, false is stored in bValue and false is
    /// returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    GetBooleanAttribute
    (
        XmlNode oNode,
        String sName,
        Boolean bRequired,
        out Boolean bValue
    )
    {
        Debug.Assert(oNode != null);
        Debug.Assert(sName != null);
        Debug.Assert(sName != "");

        // Get the attribute.

        String sValue;

        if ( !XmlUtil.GetAttribute(oNode, sName, bRequired, out sValue) )
        {
            // The attribute is missing but isn't required.  If it was
            // required, GetAttribute() threw an exception.

            bValue = false;
            return (false);
        }

        // Convert the attribute to a Boolean.

        switch (sValue)
        {
            case "0":

                bValue = false;
                break;

            case "1":

                bValue = true;
                break;

            default:

                throw new XmlException(
                    "A \"" + oNode.Name + "\" node has a \"" + sName
                    + "\" attribute that is not 0 or 1.");
        }

        return (true);
    }

    //*************************************************************************
    //  Method: GetDateTimeAttribute()
    //
    /// <summary>
    /// Reads an XML attribute that contains a date/time string and converts it
    /// to a DateTime.
    /// </summary>
    ///
    /// <param name="oNode">
    /// Node to read attribute from.
    /// </param>
    ///
    /// <param name="sName">
    /// Attribute to read.
    /// </param>
    ///
    /// <param name="bRequired">
    /// true if the attribute is required.
    /// </param>
    ///
    /// <param name="oValue">
    /// Where the converted attribute gets stored.
    /// </param>
    ///
    /// <returns>
    /// true if the attribute was found and stored in oValue.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="bRequired" /> is true and the attribute is missing,
    /// an exception is thrown.  If <paramref name="bRequired" /> is false and
    /// the attribute is missing, DateTime.MinValue is stored in oValue and
    /// false is returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    GetDateTimeAttribute
    (
        XmlNode oNode,
        String sName,
        Boolean bRequired,
        out DateTime oValue
    )
    {
        Debug.Assert(oNode != null);
        Debug.Assert(sName != null);
        Debug.Assert(sName != "");

        // Get the attribute.

        String sValue;

        if ( !XmlUtil.GetAttribute(oNode, sName, bRequired, out sValue) )
        {
            // The attribute is missing but isn't required.  If it was
            // required, GetAttribute() threw an exception.

            oValue = DateTime.MinValue;
            return (false);
        }

        // Convert the attribute to a DateTime.

        try
        {
            oValue = DateTime.Parse(sValue);
        }
        catch (Exception oException)
        {
            throw new XmlException(
                "Can't convert \"" + sValue + "\" from String to DateTime.",
                oException);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: SetAttributes()
    //
    /// <summary>
    /// Sets multiple attributes on an XML node.
    /// </summary>
    ///
    /// <param name="oNode">
    /// XmlNode.  Node to set attributes on.
    /// </param>
    ///
    /// <param name="asNameValuePairs">
    /// String[].  One or more pairs of strings.  The first string in each
    /// pair is an attribute name and the second is the attribute value.
    /// </param>
    ///
    /// <remarks>
    /// This sets multiple attributes on an XML node in one call.  It's an
    /// alternative to calling <see
    /// cref="XmlElement.SetAttribute(String, String)" /> repeatedly.
    /// </remarks>
    //*************************************************************************

    public static void
    SetAttributes
    (
        XmlNode oNode,
        params String[] asNameValuePairs
    )
    {
        Int32 iNameValueStrings = asNameValuePairs.Length;

        if (iNameValueStrings % 2 != 0)
        {
            throw new System.ArgumentException("asNameValuePairs must contain"
                + " an even number of strings.");
        }

        XmlElement oElement = (XmlElement)oNode;

        for (Int32 i = 0; i < iNameValueStrings; i+= 2)
        {
            String sName = asNameValuePairs[i + 0];
            String sValue = asNameValuePairs[i + 1];
            oElement.SetAttribute(sName, sValue);
        }
    }

}

}
