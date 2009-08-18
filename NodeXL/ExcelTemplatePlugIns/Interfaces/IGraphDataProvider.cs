
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplatePlugIns
{
//*****************************************************************************
//  Interface: IGraphDataProvider
//
/// <summary>
/// Represents an object that can provide graph data to the NodeXL Excel
/// Template.
/// </summary>
///
/// <remarks>
/// Implement this interface when you want to import graph data into the NodeXL
/// Excel Template and the data is not in a format directly importable by the
/// Template.
///
/// <para>
/// The NodeXL Excel Template (implemented by the ExcelTemplate project) can
/// directly import graph data from a variety of sources, including other Excel
/// workbooks, UCINET files, Pajek files, email indexed by Windows Desktop
/// Search, and Twitter.com.  If you have graph data in another format that you
/// want to import into the Excel Template without having to modify the
/// ExcelTemplate's source code, follow these steps:
/// </para>
///
/// <list type="number">
///
/// <item><description>
/// Create a .NET assembly that references the
/// Microsoft.NodeXL.ExcelTemplatePlugIns.dll, which defines the <see
/// cref="IGraphDataProvider" /> interface.
/// </description></item>
///
/// <item><description>
/// Add one or more classes to the assembly that implement the <see
/// cref="IGraphDataProvider" /> interface.
/// </description></item>
///
/// <item><description>
/// Put the assembly in the Excel Template's PlugIns folder.  This is usually
/// "C:\Program Files\Microsoft Research\Microsoft NodeXL Excel
/// Template\PlugIns".
/// </description></item>
///
/// </list>
///
/// <para>
/// When the user runs the Excel Template and opens its Import menu, the
/// Template uses .NET reflection to find all the classes in its PlugIns folder
/// that implement <see cref="IGraphDataProvider" />.  For each such class, it
/// adds a child item to the Import menu using the strings returned by <see
/// cref="Name" /> and <see cref="Description" />.  When the user selects one
/// of the child menu items, the <see cref="GetGraphData" /> method on the
/// corresponding class is called, and the graph data returned by that method
/// is used to populate the NodeXL workbook.
/// </para>
///
/// <para>
/// The NodeXL source code includes a sample implementation of <see
/// cref="IGraphDataProvider" />.  It is located at
/// NodeXL\ExcelTemplatePlugIns\SampleCode\SampleGraphDataProvider.cs.
/// </para>
///
/// </remarks>
//*****************************************************************************

public interface IGraphDataProvider
{
    //*************************************************************************
    //  Property: Name
    //
    /// <summary>
    /// Gets the name of the data provider.
    /// </summary>
    ///
    /// <value>
    /// The name of the data provider, as a String.  Sample: "Facebook
    /// Network".
    /// </value>
    ///
    /// <remarks>
    /// The NodeXL Excel Template adds a child item to its Import menu using
    /// the menu text "From [Name]...".  If the propety value is "Facebook
    /// Network", for example, then the menu text will be "From Facebook
    /// Network...".
    /// </remarks>
    //*************************************************************************

    String
    Name
    {
        get;
    }

    //*************************************************************************
    //  Property: Description
    //
    /// <summary>
    /// Gets a description of the data provider.
    /// </summary>
    ///
    /// <value>
    /// A description of the data provider, as a String.  Sample: "show the
    /// friends of a Facebook user".
    /// </value>
    ///
    /// <remarks>
    /// The NodeXL Excel Template uses the description as part of the tooltip
    /// for the child item it adds to its Import menu.  The tooltip text is
    /// "Optionally clear the NodeXL workbook, then [Description]."  If the
    /// property value is "show the friends of a Facebook user", for example,
    /// then the child menu item's tooltip is "Optionally clear the NodeXL
    /// workbook, then show the friends of a Facebook user."
    /// </remarks>
    //*************************************************************************

    String
    Description
    {
        get;
    }

    //*************************************************************************
    //  Method: GetGraphData()
    //
    /// <summary>
    /// Gets graph data to import into the NodeXL Excel Template.
    /// </summary>
    ///
    /// <returns>
    /// Graph data as a GraphML XML String.
    /// </returns>
    ///
    /// <remarks>
    /// The graph data must be a string containing XML that uses the GraphML
    /// schema.  A good introduction to GraphML can be found in the GraphML
    /// Primer:
    ///
    /// <para>
    /// http://graphml.graphdrawing.org/primer/graphml-primer.html
    /// </para>
    ///
    /// <para>
    /// Here is a GraphML XML string that represents a graph with five vertices
    /// and two edges.
    /// </para>
    ///
    /// <code>
    /// &lt;?xml version="1.0" encoding="UTF-8"?&gt;
    /// &lt;graphml xmlns="http://graphml.graphdrawing.org/xmlns"&gt;
    ///
    ///     &lt;key id="VertexColor" for="node" attr.name="Color" attr.type="string" /&gt;
    ///     &lt;key id="VertexLatestPostDate" for="node" attr.name="Latest Post Date"
    ///         attr.type="string" /&gt;
    ///     &lt;key id="EdgeWidth" for="edge" attr.name="Width" attr.type="double"&gt;
    ///         &lt;default&gt;1.5&lt;/default&gt;
    ///     &lt;/key&gt;
    ///
    ///     &lt;graph edgedefault="undirected"&gt;
    ///         &lt;node id="V1"&gt;
    ///             &lt;data key="VertexColor"&gt;red&lt;/data&gt;
    ///             &lt;data key="VertexLatestPostDate"&gt;2009/07/05&lt;/data&gt;
    ///         &lt;/node&gt;
    ///         &lt;node id="V2"&gt;
    ///             &lt;data key="VertexColor"&gt;orange&lt;/data&gt;
    ///             &lt;data key="VertexLatestPostDate"&gt;2009/07/12&lt;/data&gt;
    ///         &lt;/node&gt;
    ///         &lt;node id="V3"&gt;
    ///             &lt;data key="VertexColor"&gt;blue&lt;/data&gt;
    ///         &lt;/node&gt;
    ///         &lt;node id="V4"&gt;
    ///             &lt;data key="VertexColor"&gt;128,0,128&lt;/data&gt;
    ///         &lt;/node&gt;
    ///         &lt;node id="V5" /&gt;
    ///
    ///         &lt;edge source="V1" target="V2" /&gt;
    ///         &lt;edge source="V3" target="V2"&gt;
    ///             &lt;data key="EdgeWidth"&gt;2.5&lt;/data&gt;
    ///         &lt;/edge&gt;
    ///     &lt;/graph&gt;
    /// &lt;/graphml&gt;
    /// </code>
    ///
    /// <para>
    /// The Excel Template imports edge and vertex attributes, which GraphML
    /// calls "GraphML-attributes.  If the "attr.name" of the GraphML-attribute
    /// is the name of an existing column in the Edges or Vertices worksheet
    /// ("Width" and "Color" in this example), the specified edge or vertex
    /// attribute values get imported to those columns when the graph data is
    /// imported.  If the attr.name is not the name of an existing column
    /// ("Latest Post Date" in this example), a new column is added to the
    /// worksheet and the specified attribute values get imported to the new
    /// column.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    String
    GetGraphData();
}

}
