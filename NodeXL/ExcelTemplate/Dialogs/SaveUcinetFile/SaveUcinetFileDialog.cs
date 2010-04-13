
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Adapters;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: SaveUcinetFileDialog
//
/// <summary>
/// Represents a dialog box for saving a graph to a UCINET full matrix DL file.
/// </summary>
///
/// <remarks>
/// Call <see cref="SaveGraphFileDialog.ShowDialogAndSaveGraph" /> to allow the
/// user to save a graph to a location of his choice.
/// </remarks>
//*****************************************************************************

public class SaveUcinetFileDialog : SaveGraphFileDialog
{
    //*************************************************************************
    //  Constructor: SaveUcinetFileDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="SaveUcinetFileDialog" />
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

    public SaveUcinetFileDialog
    (
        String initialDirectory,
        String initialFileName

    ) : base(initialDirectory, initialFileName)
    {
        // (Do nothing else.)
    }

    //*************************************************************************
    //  Method: GetDialogTitle()
    //
    /// <summary>
    /// Returns the title to use for the dialog.
    /// </summary>
    ///
    /// <param name="oObjectBeingSaved">
    /// Object being saved.
    /// </param>
    ///
    /// <returns>
    /// Title to use for the dialog.
    /// </returns>
    //*************************************************************************

    protected override String
    GetDialogTitle
    (
        Object oObjectBeingSaved
    )
    {
        return (DialogTitle);
    }

    //*************************************************************************
    //  Method: GetFilter()
    //
    /// <summary>
    /// Returns the filter to use for the dialog.
    /// </summary>
    ///
    /// <param name="oObjectBeingSaved">
    /// Object being saved.
    /// </param>
    ///
    /// <returns>
    /// Filter to use for the dialog.
    /// </returns>
    //*************************************************************************

    protected override String
    GetFilter
    (
        Object oObjectBeingSaved
    )
    {
        return (OpenUcinetFileDialog.Filter);
    }

    //*************************************************************************
    //  Method: SaveObject()
    //
    /// <summary>
    /// Saves the object to the specified file.
    /// </summary>
    ///
    /// <param name="oObject">
    /// Object to save.
    /// </param>
    ///
    /// <param name="sFileName">
    /// File name to save the object to.
    /// </param>
    ///
    /// <remarks>
    /// This is called by the base-class ShowDialogAndSaveGraph() method.
    /// </remarks>
    //*************************************************************************

    protected override void
    SaveObject
    (
        Object oObject,
        String sFileName
    )
    {
        Debug.Assert(oObject is IGraph);

        try
        {
            ( new UcinetGraphAdapter() ).SaveGraph(
                (IGraph)oObject, sFileName ); 
        }
        catch (SaveGraphException oSaveGraphException)
        {
            OnSaveError(oSaveGraphException.Message);
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
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Title to use for this dialog.

    protected const String DialogTitle =
        "Export to UCINET Full Matrix DL File";
}

}
