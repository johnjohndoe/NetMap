
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.Layouts
{
//*****************************************************************************
//  Class: LayOutGraphAsyncArguments
//
/// <summary>
/// Stores the arguments passed to <see
/// cref="IAsyncLayout.LayOutGraphAsync" />.
/// </summary>
///
/// <remarks>
/// <see cref="AsyncLayoutBase.LayOutGraphAsync" /> uses an instance of this
/// class to pass its arguments to <see
/// cref="System.ComponentModel.BackgroundWorker.RunWorkerAsync(Object)" />.
/// </remarks>
//*****************************************************************************

public class LayOutGraphAsyncArguments : LayoutsBase
{
    //*************************************************************************
    //  Constructor: LayOutGraphAsyncArguments()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="LayOutGraphAsyncArguments" /> class.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to lay out.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.
    /// </param>
    //*************************************************************************

    public LayOutGraphAsyncArguments
    (
        IGraph graph,
        LayoutContext layoutContext
    )
    {
        m_oGraph = graph;
        m_oLayoutContext = layoutContext;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Graph
    //
    /// <summary>
    /// Gets the graph to lay out.
    /// </summary>
    ///
    /// <value>
    /// The graph to lay out, as an <see cref="IGraph" />.
    /// </value>
    //*************************************************************************

    public IGraph
    Graph
    {
        get
        {
            AssertValid();

            return (m_oGraph);
        }
    }

    //*************************************************************************
    //  Property: LayoutContext
    //
    /// <summary>
    /// Gets the object that provides access to objects needed to lay out the
    /// graph.
    /// </summary>
    ///
    /// <value>
    /// Object that provides access to objects needed to lay out the graph, as
    /// a <see cref="LayoutContext" />.
    /// </value>
    //*************************************************************************

    public LayoutContext
    LayoutContext
    {
        get
        {
            AssertValid();

            return (m_oLayoutContext);
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert(m_oGraph != null);
        Debug.Assert(m_oLayoutContext != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Graph to lay out.

    protected IGraph m_oGraph;

    /// Provides access to objects needed to lay out the graph.

    protected LayoutContext m_oLayoutContext;
}

}
