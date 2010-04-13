
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.AppLib;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Adapters;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: OpenGraphMLFileDialog
//
/// <summary>
/// Represents a dialog box for opening a GraphML file.
/// </summary>
///
/// <remarks>
/// Call <see cref="ShowDialogAndOpenGraphMLFile" /> to allow the user to open
/// a GraphML file from a location of his choice.
/// </remarks>
//*****************************************************************************

public class OpenGraphMLFileDialog : OpenFileDialog2
{
    //*************************************************************************
    //  Constructor: OpenGraphMLFileDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenGraphMLFileDialog" />
    /// class.
    /// </summary>
    //*************************************************************************

    public OpenGraphMLFileDialog()
    :
    base
    (
        String.Empty,
        String.Empty
    )
    {
        // (Do nothing else.)
    }

    //*************************************************************************
    //  Method: ShowDialogAndOpenGraphMLFile()
    //
    /// <summary>
    /// Opens a GraphML graph file.
    /// </summary>
    ///
    /// <param name="graph">
    /// Where a new graph gets stored.
    /// </param>
    ///
    /// <returns>
    /// DialogResult.OK if the user selected a file name and a a graph object
    /// was successfully created from the file.
    /// </returns>
    ///
    /// <remarks>
    /// This method allows the user to select a GraphML file name.  It then
    /// opens the file and creates a graph object from it.
    /// </remarks>
    //*************************************************************************

    public DialogResult
    ShowDialogAndOpenGraphMLFile
    (
        out IGraph graph
    )
    {
        AssertValid();

        // Let the base class do most of the work.  ShowDialogAndOpenObject()
        // calls OpenObject(), which will open the file and create a graph 
        // object from it.

        Object oObject;

        DialogResult oDialogResult = ShowDialogAndOpenObject(out oObject);

        Debug.Assert(oObject == null || oObject is IGraph);
        graph = (IGraph)oObject;

        return (oDialogResult);
    }

    //*************************************************************************
    //  Method: GetDialogTitle()
    //
    /// <summary>
    /// Gets the title to use for the dialog.
    /// </summary>
    //*************************************************************************

    protected override String
    GetDialogTitle()
    {
        AssertValid();

        return (DialogTitle);
    }

    //*************************************************************************
    //  Method: GetFilter()
    //
    /// <summary>
    /// Gets the filter to use for the dialog.
    /// </summary>
    //*************************************************************************

    protected override String
    GetFilter()
    {
        AssertValid();

        return (Filter);
    }

    //*************************************************************************
    //  Method: OpenObject()
    //
    /// <summary>
    /// Opens a graph data file and creates a graph object from it.
    /// </summary>
    ///
    /// <param name="sFileName">
    /// File name to open, including a full path.
    /// </param>
    ///
    /// <param name="oObject">
    /// Where the new graph object get stored.
    /// </param>
    ///
    /// <remarks>
    /// This is called by the base-class ShowDialogAndOpenObject() method.
    /// </remarks>
    //*************************************************************************

    protected override void
    OpenObject
    (
        String sFileName,
        out Object oObject
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sFileName) );
        Debug.Assert( File.Exists(sFileName) );
        AssertValid();

        oObject = null;

        // Use a graph adapter to create a graph from the file.

        IGraphAdapter oGraphMLGraphAdapter = new GraphMLGraphAdapter();

        oObject = oGraphMLGraphAdapter.LoadGraphFromFile(sFileName);
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
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Filter to use for this dialog.

    protected const String Filter =
        "GraphML Files (*.graphml, *.xml)|*.graphml;*.xml";

    /// Title to use for this dialog.

    protected const String DialogTitle =
        "Import from GraphML File";
}

}
