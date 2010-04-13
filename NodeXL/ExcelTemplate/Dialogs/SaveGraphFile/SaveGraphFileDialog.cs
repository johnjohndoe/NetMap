
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: SaveGraphFileDialog
//
/// <summary>
/// Abstract base class for dialogs that save a graph to a file.
/// </summary>
///
/// <remarks>
/// The file format is defined by the derived class's <see
/// cref="SaveFileDialog2.SaveObject" />
/// implementation.
/// </remarks>
//*****************************************************************************

public abstract class SaveGraphFileDialog : SaveFileDialog2
{
    //*************************************************************************
    //  Constructor: SaveGraphFileDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="SaveGraphFileDialog" />
    /// class.
    /// </summary>
    ///
    /// <param name="initialDirectory">
    /// Initial directory the dialog will display.  Use an empty string to let
    /// the dialog select an initial directory.
    /// </param>
    ///
    /// <param name="initialFileName">
    /// Initial file name.  Can be a complete path, a path without an
    /// extension, a file name, or a file name without an extension.
    /// </param>
    //*************************************************************************

    public SaveGraphFileDialog
    (
        String initialDirectory,
        String initialFileName

    ) : base(initialDirectory, initialFileName)
    {
        // (Do nothing else.)
    }

    //*************************************************************************
    //  Method: ShowDialogAndSaveGraph()
    //
    /// <summary>
    /// Shows the file save dialog and saves the graph to the selected file.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to save.
    /// </param>
    ///
    /// <returns>
    /// DialogResult.OK if the user selected a file name and the graph was
    /// successfully saved.
    /// </returns>
    ///
    /// <remarks>
    /// This method allows the user to select a file name.  It then saves the
    /// graph to the file.
    /// </remarks>
    //*************************************************************************

    public DialogResult
    ShowDialogAndSaveGraph
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);
        AssertValid();

        // Let the base class do most of the work.  The actual saving will be
        // done by SaveObject() in the derived class.

        return ( ShowDialogAndSaveObject(graph) );
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
}

}
