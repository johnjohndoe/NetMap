
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Xml;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.Research.CommunityTechnologies.XmlLib
{
//*****************************************************************************
//  Class: XmlUtil2
//
/// <summary>
/// XML utility methods.
/// </summary>
///
/// <remarks>
/// This class contains utility methods for dealing with XML.  All methods are
/// static.
///
/// <para>
/// This is an improved replacement for XmlUtil, which should not be used in
/// new projects.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class XmlUtil2
{
    //*************************************************************************
    //  Method: SelectRequiredSingleNode()
    //
    /// <summary>
    /// Selects a required XML node.
    /// </summary>
    ///
    /// <param name="node">
    /// Node to select from.
    /// </param>
    ///
    /// <param name="xPath">
    /// XPath expression.
    /// </param>
    ///
    /// <param name="xmlNamespaceManager">
    /// NamespaceManager to use, or null to not use one.
    /// </param>
    ///
    /// <returns>
    /// The selected node.
    /// </returns>
    ///
    /// <remarks>
    /// If the specified node is missing, an XmlException is thrown.
    /// </remarks>
    //*************************************************************************

    public static XmlNode
    SelectRequiredSingleNode
    (
        XmlNode node,
        String xPath,
        XmlNamespaceManager xmlNamespaceManager
    )
    {
        Debug.Assert(node != null);
        Debug.Assert( !String.IsNullOrEmpty(xPath) );

        XmlNode oSelectedNode;

        if ( !TrySelectSingleNode(node, xPath, xmlNamespaceManager,
            out oSelectedNode) )
        {
            throw new XmlException( String.Format(

                "An XML node with the name \"{0}\" is missing a required"
                + " descendent node.  The XPath is \"{1}\"."
                ,
                node.Name,
                xPath
                ) );
        }

        return (oSelectedNode);
    }

    //*************************************************************************
    //  Method: SelectRequiredSingleNodeAsString()
    //
    /// <summary>
    /// Selects a required XML node and gets its String value.
    /// </summary>
    ///
    /// <param name="node">
    /// Node to select from.
    /// </param>
    ///
    /// <param name="xPath">
    /// XPath expression.
    /// </param>
    ///
    /// <param name="xmlNamespaceManager">
    /// NamespaceManager to use, or null to not use one.
    /// </param>
    ///
    /// <returns>
    /// The selected node's String value.
    /// </returns>
    ///
    /// <remarks>
    /// If the specified node is missing or its value is an empty string, an
    /// XmlException is thrown.
    /// </remarks>
    //*************************************************************************

    public static String
    SelectRequiredSingleNodeAsString
    (
        XmlNode node,
        String xPath,
        XmlNamespaceManager xmlNamespaceManager
    )
    {
        Debug.Assert(node != null);
        Debug.Assert( !String.IsNullOrEmpty(xPath) );

        String sValue;

        if ( !TrySelectSingleNodeAsString(node, xPath, xmlNamespaceManager,
            out sValue) )
        {
            throw new XmlException( String.Format(

                "An XML node with the name \"{0}\" is missing a required"
                + " descendent node whose value must be a non-empty String."
                + "  The XPath is \"{1}\"."
                ,
                node.Name,
                xPath
                ) );
        }

        return (sValue);
    }

    //*************************************************************************
    //  Method: SelectRequiredSingleNodeAsInt32()
    //
    /// <summary>
    /// Selects a required XML node and gets its Int32 value.
    /// </summary>
    ///
    /// <param name="node">
    /// Node to select from.
    /// </param>
    ///
    /// <param name="xPath">
    /// XPath expression.
    /// </param>
    ///
    /// <param name="xmlNamespaceManager">
    /// NamespaceManager to use, or null to not use one.
    /// </param>
    ///
    /// <returns>
    /// The selected node's Int32 value.
    /// </returns>
    ///
    /// <remarks>
    /// If the specified node is missing or its value isn't an Int32, an
    /// XmlException is thrown.
    /// </remarks>
    //*************************************************************************

    public static Int32
    SelectRequiredSingleNodeAsInt32
    (
        XmlNode node,
        String xPath,
        XmlNamespaceManager xmlNamespaceManager
    )
    {
        Debug.Assert(node != null);
        Debug.Assert( !String.IsNullOrEmpty(xPath) );

        Int32 iValue;

        if ( !TrySelectSingleNodeAsInt32(node, xPath, xmlNamespaceManager,
            out iValue) )
        {
            throw new XmlException( String.Format(

                "An XML node with the name \"{0}\" is missing a required"
                + " descendent node whose value must be an Int32.  The XPath"
                + " is \"{1}\"."
                ,
                node.Name,
                xPath
                ) );
        }

        return (iValue);
    }

    //*************************************************************************
    //  Method: SelectRequiredSingleNodeAsDouble()
    //
    /// <summary>
    /// Selects a required XML node and gets its Double value.
    /// </summary>
    ///
    /// <param name="node">
    /// Node to select from.
    /// </param>
    ///
    /// <param name="xPath">
    /// XPath expression.
    /// </param>
    ///
    /// <param name="xmlNamespaceManager">
    /// NamespaceManager to use, or null to not use one.
    /// </param>
    ///
    /// <returns>
    /// The selected node's Double value.
    /// </returns>
    ///
    /// <remarks>
    /// If the specified node is missing or its value isn't a Double, an
    /// XmlException is thrown.
    /// </remarks>
    //*************************************************************************

    public static Double
    SelectRequiredSingleNodeAsDouble
    (
        XmlNode node,
        String xPath,
        XmlNamespaceManager xmlNamespaceManager
    )
    {
        Debug.Assert(node != null);
        Debug.Assert( !String.IsNullOrEmpty(xPath) );

        Double dValue;

        if ( !TrySelectSingleNodeAsDouble(node, xPath, xmlNamespaceManager,
            out dValue) )
        {
            throw new XmlException( String.Format(

                "An XML node with the name \"{0}\" is missing a required"
                + " descendent node whose value must be a Double.  The XPath"
                + " is \"{1}\"."
                ,
                node.Name,
                xPath
                ) );
        }

        return (dValue);
    }

    //*************************************************************************
    //  Method: TrySelectSingleNode()
    //
    /// <summary>
    /// Attempts to select an XML node.
    /// </summary>
    ///
    /// <param name="node">
    /// Node to select from.
    /// </param>
    ///
    /// <param name="xPath">
    /// XPath expression.
    /// </param>
    ///
    /// <param name="xmlNamespaceManager">
    /// NamespaceManager to use, or null to not use one.
    /// </param>
    ///
    /// <param name="selectedNode">
    /// Where the selected node gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the specified node was found.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TrySelectSingleNode
    (
        XmlNode node,
        String xPath,
        XmlNamespaceManager xmlNamespaceManager,
        out XmlNode selectedNode
    )
    {
        Debug.Assert(node != null);
        Debug.Assert( !String.IsNullOrEmpty(xPath) );

        selectedNode = null;

        if (xmlNamespaceManager != null)
        {
            selectedNode = node.SelectSingleNode(xPath, xmlNamespaceManager);
        }
        else
        {
            selectedNode = node.SelectSingleNode(xPath);
        }

        return (selectedNode != null);
    }

    //*************************************************************************
    //  Method: TrySelectSingleNodeAsString()
    //
    /// <summary>
    /// Attempts to select an XML node and get its String value.
    /// </summary>
    ///
    /// <param name="node">
    /// Node to select from.
    /// </param>
    ///
    /// <param name="xPath">
    /// XPath expression.
    /// </param>
    ///
    /// <param name="xmlNamespaceManager">
    /// NamespaceManager to use, or null to not use one.
    /// </param>
    ///
    /// <param name="value">
    /// Where the selected node's String value gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the specified node was found and its value was a non-empty
    /// string.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TrySelectSingleNodeAsString
    (
        XmlNode node,
        String xPath,
        XmlNamespaceManager xmlNamespaceManager,
        out String value
    )
    {
        Debug.Assert(node != null);
        Debug.Assert( !String.IsNullOrEmpty(xPath) );

        value = null;
        XmlNode oSelectedNode;

        if ( !TrySelectSingleNode(node, xPath, xmlNamespaceManager,
            out oSelectedNode) )
        {
            return (false);
        }

        value = oSelectedNode.Value;

        return ( !String.IsNullOrEmpty(value) );
    }

    //*************************************************************************
    //  Method: TrySelectSingleNodeAsInt32()
    //
    /// <summary>
    /// Attempts to select an XML node and get its Int32 value.
    /// </summary>
    ///
    /// <param name="node">
    /// Node to select from.
    /// </param>
    ///
    /// <param name="xPath">
    /// XPath expression.
    /// </param>
    ///
    /// <param name="xmlNamespaceManager">
    /// NamespaceManager to use, or null to not use one.
    /// </param>
    ///
    /// <param name="value">
    /// Where the selected node's Int32 value gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the specified node was found and its value was an Int32.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TrySelectSingleNodeAsInt32
    (
        XmlNode node,
        String xPath,
        XmlNamespaceManager xmlNamespaceManager,
        out Int32 value
    )
    {
        Debug.Assert(node != null);
        Debug.Assert( !String.IsNullOrEmpty(xPath) );

        value = Int32.MinValue;
        String sValue;

        return (
            TrySelectSingleNodeAsString(node, xPath, xmlNamespaceManager,
                out sValue)
            &&
            MathUtil.TryParseCultureInvariantInt32(sValue, out value)
            );
    }

    //*************************************************************************
    //  Method: TrySelectSingleNodeAsUInt32()
    //
    /// <summary>
    /// Attempts to select an XML node and get its UInt32 value.
    /// </summary>
    ///
    /// <param name="node">
    /// Node to select from.
    /// </param>
    ///
    /// <param name="xPath">
    /// XPath expression.
    /// </param>
    ///
    /// <param name="xmlNamespaceManager">
    /// NamespaceManager to use, or null to not use one.
    /// </param>
    ///
    /// <param name="value">
    /// Where the selected node's UInt32 value gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the specified node was found and its value was a UInt32.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TrySelectSingleNodeAsUInt32
    (
        XmlNode node,
        String xPath,
        XmlNamespaceManager xmlNamespaceManager,
        out UInt32 value
    )
    {
        Debug.Assert(node != null);
        Debug.Assert( !String.IsNullOrEmpty(xPath) );

        value = UInt32.MinValue;
        String sValue;

        return (
            TrySelectSingleNodeAsString(node, xPath, xmlNamespaceManager,
                out sValue)
            &&
            MathUtil.TryParseCultureInvariantUInt32(sValue, out value)
            );
    }

    //*************************************************************************
    //  Method: TrySelectSingleNodeAsDouble()
    //
    /// <summary>
    /// Attempts to select an XML node and get its Double value.
    /// </summary>
    ///
    /// <param name="node">
    /// Node to select from.
    /// </param>
    ///
    /// <param name="xPath">
    /// XPath expression.
    /// </param>
    ///
    /// <param name="xmlNamespaceManager">
    /// NamespaceManager to use, or null to not use one.
    /// </param>
    ///
    /// <param name="value">
    /// Where the selected node's Double value gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the specified node was found and its value was a Double.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TrySelectSingleNodeAsDouble
    (
        XmlNode node,
        String xPath,
        XmlNamespaceManager xmlNamespaceManager,
        out Double value
    )
    {
        Debug.Assert(node != null);
        Debug.Assert( !String.IsNullOrEmpty(xPath) );

        value = Double.MinValue;
        String sValue;

        return (
            TrySelectSingleNodeAsString(node, xPath, xmlNamespaceManager,
                out sValue)
            &&
            MathUtil.TryParseCultureInvariantDouble(sValue, out value)
            );
    }

    //*************************************************************************
    //  Method: AppendNewNode()
    //
    /// <summary>
    /// Creates a new XML node with an optional namespace and appends it to a
    /// parent node.
    /// </summary>
    ///
    /// <param name="parentNode">
    /// Node to append the new node to.
    /// </param>
    /// 
    /// <param name="childName">
    /// Name of the new node.
    /// </param>
    ///
    /// <param name="namespaceUri">
    /// Optional namespace URI of the new node.  If null or empty, no namespace
    /// is used.
    /// </param>
    ///
    /// <returns>
    /// The new node.
    /// </returns>
    //*************************************************************************

    static public XmlNode
    AppendNewNode
    (
        XmlNode parentNode,
        String childName,
        String namespaceUri
    )
    {
        Debug.Assert(parentNode != null);
        Debug.Assert( !String.IsNullOrEmpty(childName) );

        // Get the owner document.

        XmlDocument oOwnerDocument = parentNode.OwnerDocument;

        // Unfortunately, the root node's OwnerDocument property returns null,
        // so we have to check for this special case.

        if (oOwnerDocument == null)
        {
            oOwnerDocument = (XmlDocument)parentNode;
        }

        XmlElement oNewNode;

        if ( String.IsNullOrEmpty(namespaceUri) )
        {
            oNewNode = oOwnerDocument.CreateElement(childName);
        }
        else
        {
            oNewNode = oOwnerDocument.CreateElement(childName, namespaceUri);
        }

        return ( parentNode.AppendChild(oNewNode) );
    }

    //*************************************************************************
    //  Method: SetAttributes()
    //
    /// <summary>
    /// Sets multiple attributes on an XML node.
    /// </summary>
    ///
    /// <param name="node">
    /// XmlNode.  Node to set attributes on.
    /// </param>
    ///
    /// <param name="nameValuePairs">
    /// String[].  One or more pairs of strings.  The first string in each pair
    /// is an attribute name and the second is the attribute value.
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
        XmlNode node,
        params String[] nameValuePairs
    )
    {
        Int32 iNameValueStrings = nameValuePairs.Length;

        if (iNameValueStrings % 2 != 0)
        {
            throw new System.ArgumentException("nameValuePairs must contain"
                + " an even number of strings.");
        }

        XmlElement oElement = (XmlElement)node;

        for (Int32 i = 0; i < iNameValueStrings; i+= 2)
        {
            String sName = nameValuePairs[i + 0];
            String sValue = nameValuePairs[i + 1];
            oElement.SetAttribute(sName, sValue);
        }
    }
}

}
