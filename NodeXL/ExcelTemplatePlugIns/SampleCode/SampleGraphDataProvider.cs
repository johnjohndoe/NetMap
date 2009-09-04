using System;
using System.Xml;
using System.Diagnostics;
using Microsoft.NodeXL.ExcelTemplatePlugIns;

namespace Microsoft.NodeXL.SampleExcelTemplatePlugIns
{
//*****************************************************************************
//  Class: SampleGraphDataProvider
//
/// <summary>
/// This is a sample implementation of a NodeXL Excel Template plug-in that
/// imports custom data into the Excel Template.
/// </summary>
///
/// <remarks>
/// This implementation of the <see cref="IGraphDataProvider" /> simulates a
/// graph data provider that retrieves the friends of a Facebook user from the
/// Facebook API.  The actual Facebook API calls are omitted.
/// </remarks>
//*****************************************************************************

public class SampleGraphDataProvider : IGraphDataProvider
{
    //*************************************************************************
    //  Property: Name
    //
    /// <summary>
    /// Gets the name of the data provider.
    /// </summary>
    ///
    /// <value>
    /// See the <see cref="IGraphDataProvider" /> topic for details.
    /// </value>
    //*************************************************************************

    public String
    Name
    {
        get
        {
            return ("Facebook Network");
        }
    }

    //*************************************************************************
    //  Property: Description
    //
    /// <summary>
    /// Gets a description of the data provider.
    /// </summary>
    ///
    /// <value>
    /// See the <see cref="IGraphDataProvider" /> topic for details.
    /// </value>
    //*************************************************************************

    public String
    Description
    {
        get
        {
            return ("show the friends of a Facebook user");
        }
    }

    //*************************************************************************
    //  Method: TryGetGraphData()
    //
    /// <summary>
    /// Attempts to get graph data to import into the NodeXL Excel Template.
    /// </summary>
    ///
    /// <param name="graphDataAsGraphML">
    /// Where the graph data gets stored as a GraphML XML string, if true is
    /// returns.
    /// </param>
    ///
    /// <returns>
    /// true if the graph data was obtained, false if not.
    /// </returns>
    ///
    /// <remarks>
    /// See the <see cref="IGraphDataProvider" /> topic for details.
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryGetGraphData
    (
        out String graphDataAsGraphML
    )
    {
        // See the "GraphML Primer" for details on the GraphML XML schema:
        //
        // http://graphml.graphdrawing.org/primer/graphml-primer.html
        //
        // Here is the XML created by this method:
        //
        /*
        <?xml version="1.0" encoding="UTF-8"?>
        <graphml xmlns="http://graphml.graphdrawing.org/xmlns">

            <key id="VertexColor" for="node" attr.name="Color"
                attr.type="string" />
            <key id="VertexLatestPostDate" for="node"
                attr.name="Latest Post Date" attr.type="string" />
            <key id="EdgeWidth" for="edge" attr.name="Width" attr.type="double">
                <default>1.5</default>
            </key>

            <graph edgedefault="undirected">
                <node id="V1">
                    <data key="VertexColor">red</data>
                    <data key="VertexLatestPostDate">2009/07/05</data>
                </node>
                <node id="V2">
                    <data key="VertexColor">orange</data>
                    <data key="VertexLatestPostDate">2009/07/12</data>
                </node>
                <node id="V3">
                    <data key="VertexColor">blue</data>
                </node>
                <node id="V4">
                    <data key="VertexColor">128,0,128</data>
                </node>
                <node id="V5" />

                <edge source="V1" target="V2" />
                <edge source="V3" target="V2">
                    <data key="EdgeWidth">2.5</data>
                </edge>
            </graph>
        </graphml>
        */

        XmlDocument oXmlDocument = new XmlDocument();

        oXmlDocument.AppendChild( oXmlDocument.CreateXmlDeclaration(
            "1.0", "UTF-8", null) );

        // (The XML Schema reference presented in the Primer is optional and is
        // skipped here.)

        // Add the root XML node.

        XmlNode oGraphMLXmlNode = AppendXmlNode(oXmlDocument, "graphml");

        // Define vertex and edge attributes, which GraphML calls
        // "GraphML-Attributes".  Note that "Width" and "Color" are column
        // names in the NodeXL Excel Template, so these attribute values will
        // get imported into those columns.  "Latest Post Date" is not an
        // existing column name, however, so a "Latest Post Date" column will
        // be added to the Vertices worksheet when the graph data is imported.
        //
        // Also note that Width is given an optional default value.

        const String VertexColorID = "VertexColor";
        const String VertexLatestPostDateID = "VertexLatestPostDate";
        const String EdgeWidthID = "EdgeWidth";

        DefineGraphMLAttribute(oGraphMLXmlNode, false, VertexColorID,
            "Color", "string", null);

        DefineGraphMLAttribute(oGraphMLXmlNode, false,
            VertexLatestPostDateID, "Latest Post Date", "string", null);

        DefineGraphMLAttribute(oGraphMLXmlNode, true, EdgeWidthID,
            "Width", "double", "1.5");

        // Add the graph node and its required directedness value.

        XmlNode oGraphXmlNode = AppendXmlNode(oGraphMLXmlNode, "graph");
        SetXmlNodeAttributes(oGraphXmlNode, "edgedefault", "undirected");

        // Add some vertices.

        XmlNode oVertexXmlNode = AppendVertexXmlNode(oGraphXmlNode, "V1");
        AppendGraphMLAttributeValue(oVertexXmlNode, VertexColorID, "red");
        AppendGraphMLAttributeValue(oVertexXmlNode, VertexLatestPostDateID,
            "2009/07/05");

        oVertexXmlNode = AppendVertexXmlNode(oGraphXmlNode, "V2");
        AppendGraphMLAttributeValue(oVertexXmlNode, VertexColorID, "orange");
        AppendGraphMLAttributeValue(oVertexXmlNode, VertexLatestPostDateID,
            "2009/07/12");

        oVertexXmlNode = AppendVertexXmlNode(oGraphXmlNode, "V3");
        AppendGraphMLAttributeValue(oVertexXmlNode, VertexColorID, "blue");

        oVertexXmlNode = AppendVertexXmlNode(oGraphXmlNode, "V4");
        AppendGraphMLAttributeValue(oVertexXmlNode, VertexColorID, "128,0,128");

        oVertexXmlNode = AppendVertexXmlNode(oGraphXmlNode, "V5");

        // Connect some of the vertices with edges.

        XmlNode oEdgeXmlNode;

        oEdgeXmlNode = AppendEdgeXmlNode(oGraphXmlNode, "V1", "V2");

        oEdgeXmlNode = AppendEdgeXmlNode(oGraphXmlNode, "V3", "V2");
        AppendGraphMLAttributeValue(oEdgeXmlNode, EdgeWidthID, "2.5");

        graphDataAsGraphML = oXmlDocument.OuterXml;

        return (true);
    }

    //*************************************************************************
    //  Method: DefineGraphMLAttribute()
    //
    /// <summary>
    /// Defines a GraphML-Attribute.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlNode">
    /// Root graphml node.
    /// </param>
    /// 
    /// <param name="bForEdge">
    /// true if the attribute is for edges, false if it is for vertices.
    /// </param>
    /// 
    /// <param name="sAttributeID">
    /// The attribute's ID.
    /// </param>
    /// 
    /// <param name="sAttributeName">
    /// The attribute's name
    /// </param>
    /// 
    /// <param name="sAttributeType">
    /// The attribute's type
    /// </param>
    /// 
    /// <param name="sDefaultAttributeValue">
    /// Default attribute value, or null there is no default.
    /// </param>
    //*************************************************************************

    protected void
    DefineGraphMLAttribute
    (
        XmlNode oGraphMLXmlNode,
        Boolean bForEdge,
        String sAttributeID,
        String sAttributeName,
        String sAttributeType,
        String sDefaultAttributeValue
    )
    {
        Debug.Assert(oGraphMLXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sAttributeID) );
        Debug.Assert( !String.IsNullOrEmpty(sAttributeName) );
        Debug.Assert( !String.IsNullOrEmpty(sAttributeType) );

        XmlNode oGraphMLAttributeXmlNode =
            AppendXmlNode(oGraphMLXmlNode, "key");

        SetXmlNodeAttributes(oGraphMLAttributeXmlNode, 
            "id", sAttributeID,
            "for", bForEdge ? "edge" : "node",
            "attr.name", sAttributeName,
            "attr.type", sAttributeType
            );

        if ( !String.IsNullOrEmpty(sDefaultAttributeValue) )
        {
            AppendXmlNode(oGraphMLAttributeXmlNode, "default",
                sDefaultAttributeValue);
        }
    }

    //*************************************************************************
    //  Method: AppendVertexXmlNode()
    //
    /// <summary>
    /// Creates a new XML node representing a vertex and appends it to a parent
    /// node.
    /// </summary>
    ///
    /// <param name="oParentXmlNode">
    /// Node to append the new node to.
    /// </param>
    /// 
    /// <param name="sVertexID">
    /// Vertex ID.
    /// </param>
    ///
    /// <returns>
    /// The new node.
    /// </returns>
    //*************************************************************************

    protected XmlNode
    AppendVertexXmlNode
    (
        XmlNode oParentXmlNode,
        String sVertexID
    )
    {
        Debug.Assert(oParentXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sVertexID) );

        // The "id" attribute is required.

        XmlNode oVertexXmlNode = AppendXmlNode(oParentXmlNode, "node");
        SetXmlNodeAttributes(oVertexXmlNode, "id", sVertexID);

        return (oVertexXmlNode);
    }

    //*************************************************************************
    //  Method: AppendEdgeXmlNode()
    //
    /// <summary>
    /// Creates a new XML node representing an edge and appends it to a parent
    /// node.
    /// </summary>
    ///
    /// <param name="oParentXmlNode">
    /// Node to append the new node to.
    /// </param>
    /// 
    /// <param name="sVertex1ID">
    /// Vertex ID of the edge's first vertex.
    /// </param>
    ///
    /// <param name="sVertex2ID">
    /// Vertex ID of the edge's second vertex.
    /// </param>
    ///
    /// <returns>
    /// The new node.
    /// </returns>
    //*************************************************************************

    protected XmlNode
    AppendEdgeXmlNode
    (
        XmlNode oParentXmlNode,
        String sVertex1ID,
        String sVertex2ID
    )
    {
        Debug.Assert(oParentXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sVertex1ID) );
        Debug.Assert( !String.IsNullOrEmpty(sVertex2ID) );

        XmlNode oEdgeXmlNode = AppendXmlNode(oParentXmlNode, "edge");

        // The "id" attribute is optional and is omitted here.

        SetXmlNodeAttributes(oEdgeXmlNode,
            "source", sVertex1ID,
            "target", sVertex2ID
            );

        return (oEdgeXmlNode);
    }

    //*************************************************************************
    //  Method: AppendGraphMLAttributeValue()
    //
    /// <summary>
    /// Appends a GraphML-Attribute value to an edge or vertex XML node. 
    /// </summary>
    ///
    /// <param name="oEdgeOrVertexXmlNode">
    /// The edge or vertex XML node to add the attribute value to.
    /// </param>
    /// 
    /// <param name="sAttributeID">
    /// The attribute's ID.
    /// </param>
    /// 
    /// <param name="sAttributeValue">
    /// The attribute's value.  Can be an empty string but not null.
    /// </param>
    //*************************************************************************

    protected void
    AppendGraphMLAttributeValue
    (
        XmlNode oEdgeOrVertexXmlNode,
        String sAttributeID,
        String sAttributeValue
    )
    {
        Debug.Assert(oEdgeOrVertexXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sAttributeID) );
        Debug.Assert(sAttributeValue != null);

        XmlNode oGraphMLAttributeValueXmlNode = AppendXmlNode(
            oEdgeOrVertexXmlNode, "data", sAttributeValue);

        SetXmlNodeAttributes(oGraphMLAttributeValueXmlNode,
            "key", sAttributeID);
    }

    //*************************************************************************
    //  Method: AppendXmlNode()
    //
    /// <overloads>
    /// Creates a new XML node and appends it to a parent node.
    /// </overloads>
    ///
    /// <summary>
    /// Creates a new XML node and appends it to a parent node.
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
    /// <returns>
    /// The new node.
    /// </returns>
    //*************************************************************************

    protected XmlNode
    AppendXmlNode
    (
        XmlNode oParentXmlNode,
        String sChildName
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

        return ( oParentXmlNode.AppendChild(
            oOwnerDocument.CreateElement(sChildName,
                "http://graphml.graphdrawing.org/xmlns") ) );
    }

    //*************************************************************************
    //  Method: AppendXmlNode()
    //
    /// <summary>
    /// Creates a new XML node, appends it to a parent node, and sets its inner
    /// text.
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
    /// <param name="sInnerText">
    /// The new node's inner text.  Can be empty but not null.
    /// </param>
    ///
    /// <returns>
    /// The new node.
    /// </returns>
    //*************************************************************************

    protected XmlNode
    AppendXmlNode
    (
        XmlNode oParentXmlNode,
        String sChildName,
        String sInnerText
    )
    {
        Debug.Assert(oParentXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sChildName) );
        Debug.Assert(sInnerText != null);

        XmlNode oNewXmlNode = AppendXmlNode(oParentXmlNode, sChildName);
        oNewXmlNode.InnerText = sInnerText;

        return (oNewXmlNode);
    }

    //*************************************************************************
    //  Method: SetXmlNodeAttributes()
    //
    /// <summary>
    /// Sets multiple attributes on an XML node.
    /// </summary>
    ///
    /// <param name="oXmlNode">
    /// Node to set attributes on.
    /// </param>
    ///
    /// <param name="asNameValuePairs">
    /// One or more pairs of strings.  The first string in each pair is an
    /// attribute name and the second is the attribute value.
    /// </param>
    ///
    /// <remarks>
    /// This sets multiple attributes on an XML node in one call.  It's an
    /// alternative to calling <see
    /// cref="XmlElement.SetAttribute(String, String)" /> repeatedly.
    /// </remarks>
    //*************************************************************************

    protected void
    SetXmlNodeAttributes
    (
        XmlNode oXmlNode,
        params String[] asNameValuePairs
    )
    {
        Debug.Assert(oXmlNode != null);
        Debug.Assert(asNameValuePairs != null);

        Int32 iNameValueStrings = asNameValuePairs.Length;
        Debug.Assert(iNameValueStrings % 2 == 0);

        XmlElement oElement = (XmlElement)oXmlNode;

        for (Int32 i = 0; i < iNameValueStrings; i+= 2)
        {
            String sName = asNameValuePairs[i + 0];
            String sValue = asNameValuePairs[i + 1];
            oElement.SetAttribute(sName, sValue);
        }
    }
}

}
