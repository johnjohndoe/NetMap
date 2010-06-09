
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Xml;
using System.Diagnostics;

namespace Microsoft.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterVertex
//
/// <summary>
/// Represents a vertex in a Twitter network.
/// </summary>
///
/// <remarks>
/// This class wraps an XmlNode from a GraphMLDocument.  The XmlNode represents
/// a vertex in a Twitter network.  Wrapping the XmlNode allows additional
/// information about the vertex to be stored alongside the XmlNode.
/// </remarks>
//*****************************************************************************

public class TwitterVertex : Object
{
    //*************************************************************************
    //  Constructor: TwitterVertex()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterVertex" /> class.
    /// </summary>
    ///
    /// <param name="vertexXmlNode">
    /// The XmlNode from a GraphMLDocument that represents a vertex in a
    /// Twitter network.
    /// </param>
    //*************************************************************************

    public TwitterVertex
    (
        XmlNode vertexXmlNode
    )
    {
        m_oVertexXmlNode = vertexXmlNode;
        m_sStatusForAnalysis = null;
        m_sStatusForAnalysisDate = null;

        AssertValid();
    }

    //*************************************************************************
    //  Property: VertexXmlNode
    //
    /// <summary>
    /// Gets the XmlNode that represents a vertex in a Twitter network.
    /// </summary>
    ///
    /// <value>
    /// The XmlNode from a GraphMLDocument that represents a vertex in a
    /// Twitter network.
    /// </value>
    //*************************************************************************

    public XmlNode
    VertexXmlNode
    {
        get
        {
            AssertValid();

            return (m_oVertexXmlNode);
        }
    }

    //*************************************************************************
    //  Property: StatusForAnalysis
    //
    /// <summary>
    /// Gets or sets the status text to use for relationship analysis.
    /// </summary>
    ///
    /// <value>
    /// The status text to use for relationship analysis.  Can be null or
    /// empty.  The default value is null.
    /// </value>
    ///
    /// <remarks>
    /// The Twitter network analyzer classes optionally store a status for the
    /// user on the vertex XML node.  This can be either the user's latest
    /// status or some other status he has posted.  However, the network
    /// analyzer classes need the status for relationship analysis even if the
    /// status isn't stored on the vertex XML node, so this property provides
    /// the status independent of what's been stored on the vertex XML node.
    ///
    /// <para>
    /// When a network analyzer class stores a status on the vertex XML node,
    /// it should also set this property and <see
    /// cref="StatusForAnalysisDate" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public String
    StatusForAnalysis
    {
        get
        {
            AssertValid();

            return (m_sStatusForAnalysis);
        }

        set
        {
            m_sStatusForAnalysis = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: StatusForAnalysisDate
    //
    /// <summary>
    /// Gets or sets the date of <see cref="StatusForAnalysis" />.
    /// </summary>
    ///
    /// <value>
    /// The date of the status text to use for relationship analysis.  Can be
    /// null or empty.  The default value is null.
    /// </value>
    ///
    /// <remarks>
    /// See <see cref="StatusForAnalysis" /> for more details.
    /// </remarks>
    //*************************************************************************

    public String
    StatusForAnalysisDate
    {
        get
        {
            AssertValid();

            return (m_sStatusForAnalysisDate);
        }

        set
        {
            m_sStatusForAnalysisDate = value;

            AssertValid();
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        Debug.Assert(m_oVertexXmlNode != null);
        // m_sStatusForAnalysis
        // m_sStatusForAnalysisDate
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The XmlNode from a GraphMLDocument that represents a vertex in a
    /// Twitter network.

    protected XmlNode m_oVertexXmlNode;

    /// The status text to use for relationship analysis.  Can be null or
    /// empty.

    protected String m_sStatusForAnalysis;

    /// The date of m_sStatusForAnalysis.  Can be null or empty.

    protected String m_sStatusForAnalysisDate;
}

}
