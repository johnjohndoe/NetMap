using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: SubgraphLinkLabel
//
/// <summary>
/// Represents a LinkLabel that, when clicked, explains what a subgraph is.
/// </summary>
//*****************************************************************************

public class SubgraphLinkLabel : LinkLabel
{
    //*************************************************************************
    //  Constructor: SubgraphLinkLabel()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="SubgraphLinkLabel" /> class.
    /// </summary>
    //*************************************************************************

    public SubgraphLinkLabel()
    {
        this.Text = "What is a subgraph?";

        AssertValid();
    }

    //*************************************************************************
    //  Method: OnLinkClicked()
    //
    /// <summary>
    /// Handles the LinkClicked event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected override void
    OnLinkClicked
    (
        LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        FormUtil.ShowInformation(HelpMessage);

        base.OnLinkClicked(e);
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
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Message to display when the link is clicked. 

    protected const String HelpMessage =

        "The level-1.0 subgraph for a vertex consists of the vertex, its"
        + " adjacent vertices, and the edges that connect the vertex to its"
        + " adjacent vertices.  A level-1.5 subgraph adds any edges connecting"
        + " the adjacent vertices to each other.  A level-2.0 subgraph adds the"
        + " vertices adjacent to the adjacent vertices, and so on.  You can"
        + " select up to 4.5 levels of adjacent vertices in the subgraphs."
        ;


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    // (None.);
}

}
