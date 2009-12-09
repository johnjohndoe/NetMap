
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Xml;
using System.Diagnostics;

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
    //  Method: SelectSingleNode()
    //
    /// <overloads>
    /// Selects a single XML node.
    /// </overloads>
    ///
    /// <summary>
    /// Selects a single XML node.
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
    /// <param name="required">
    /// true if the specified node must exist.
    /// </param>
    ///
    /// <param name="selectedNode">
    /// Where the selected node gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the specified node was found.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="required" /> is true and the specified node is
    /// missing, an exception is thrown.  If <paramref name="required" /> is
    /// false and the specified node is missing, null is stored in <paramref
    /// name="selectedNode" /> and false is returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    SelectSingleNode
    (
        XmlNode node,
        String xPath,
        XmlNamespaceManager xmlNamespaceManager,
        Boolean required,
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

        if (selectedNode != null)
        {
            return (true);
        }

        if (required)
        {
            throw new XmlException( String.Format(

                "An XML node with the name {0} is missing a required"
                + " descendent node.  The XPath is \"{1}\"."
                ,
                node.Name,
                xPath
                ) );
        }

        return (false);
    }

    //*************************************************************************
    //  Method: SelectSingleNode()
    //
    /// <summary>
    /// Selects a single XML node and gets its String value.
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
    /// <param name="required">
    /// true if the specified node must exist and its value must be a non-empty
    /// string.
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
    ///
    /// <remarks>
    /// If <paramref name="required" /> is true and the specified node is
    /// missing or its value is null or empty, an exception is thrown.  If
    /// <paramref name="required" /> is false and the specified node is missing
    /// or its value is null or empty, null is stored in <paramref
    /// name="value" /> and false is returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    SelectSingleNode
    (
        XmlNode node,
        String xPath,
        XmlNamespaceManager xmlNamespaceManager,
        Boolean required,
        out String value
    )
    {
        Debug.Assert(node != null);
        Debug.Assert( !String.IsNullOrEmpty(xPath) );

        value = null;
        XmlNode oSelectedNode;

        if ( !SelectSingleNode(node, xPath, xmlNamespaceManager, required,
            out oSelectedNode) )
        {
            return (false);
        }

        value = oSelectedNode.Value;

        if ( !String.IsNullOrEmpty(value) )
        {
            return (true);
        }

        if (required)
        {
            throw new XmlException( String.Format(

                "An XML node with the name {0} is missing a required"
                + " descendent node whose value must be a non-empty String."
                + "  The XPath is \"{1}\"."
                ,
                node.Name,
                xPath
                ) );
        }

        return (false);
    }

    //*************************************************************************
    //  Method: SelectSingleNode()
    //
    /// <summary>
    /// Selects a single XML node and gets its Int32 value.
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
    /// <param name="required">
    /// true if the specified node must exist and its value must be an Int32.
    /// </param>
    ///
    /// <param name="value">
    /// Where the selected node's Int32 value gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the specified node was found and its value was an Int32.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="required" /> is true and the specified node is
    /// missing or its value is not an Int32, an exception is thrown.  If
    /// <paramref name="required" /> is false and the specified node is missing
    /// or its value is not an Int32, Int32.MinValue is stored in <paramref
    /// name="value" /> and false is returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    SelectSingleNode
    (
        XmlNode node,
        String xPath,
        XmlNamespaceManager xmlNamespaceManager,
        Boolean required,
        out Int32 value
    )
    {
        Debug.Assert(node != null);
        Debug.Assert( !String.IsNullOrEmpty(xPath) );

        value = Int32.MinValue;
        String sValue;

        if ( !SelectSingleNode(node, xPath, xmlNamespaceManager, required,
            out sValue) )
        {
            return (false);
        }

        if ( Int32.TryParse(sValue, out value) )
        {
            return (true);
        }

        if (required)
        {
            throw new XmlException( String.Format(

                "An XML node with the name {0} is missing a required"
                + " descendent node whose value must be an Int32.  The XPath"
                + " is \"{1}\"."
                ,
                node.Name,
                xPath
                ) );
        }

        value = Int32.MinValue;

        return (false);
    }

    //*************************************************************************
    //  Method: SelectSingleNode()
    //
    /// <summary>
    /// Selects a single XML node and gets its Double value.
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
    /// <param name="required">
    /// true if the specified node must exist and its value must be a Double.
    /// </param>
    ///
    /// <param name="value">
    /// Where the selected node's Double value gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the specified node was found and its value was a Double.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="required" /> is true and the specified node is
    /// missing or its value is not a Double, an exception is thrown.  If
    /// <paramref name="required" /> is false and the specified node is missing
    /// or its value is not a Double, Double.MinValue is stored in <paramref
    /// name="value" /> and false is returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    SelectSingleNode
    (
        XmlNode node,
        String xPath,
        XmlNamespaceManager xmlNamespaceManager,
        Boolean required,
        out Double value
    )
    {
        Debug.Assert(node != null);
        Debug.Assert( !String.IsNullOrEmpty(xPath) );

        value = Double.MinValue;
        String sValue;

        if ( !SelectSingleNode(node, xPath, xmlNamespaceManager, required,
            out sValue) )
        {
            return (false);
        }

        if ( Double.TryParse(sValue, out value) )
        {
            return (true);
        }

        if (required)
        {
            throw new XmlException( String.Format(

                "An XML node with the name {0} is missing a required"
                + " descendent node whose value must be a Double.  The XPath"
                + " is \"{1}\"."
                ,
                node.Name,
                xPath
                ) );
        }

        value = Double.MinValue;

        return (false);
    }
}

}
