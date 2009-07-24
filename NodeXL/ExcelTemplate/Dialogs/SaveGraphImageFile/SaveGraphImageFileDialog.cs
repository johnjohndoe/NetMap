
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: SaveGraphImageFileDialog
//
/// <summary>
/// Represents a dialog box for saving an image of a graph to a file.
/// </summary>
///
/// <remarks>
/// Call ShowDialogAndSaveImage() to allow the user to save an image in a
/// format of his choice to a location of his choice.
///
/// <para>
/// This class extends the <see cref="SaveImageFileDialog" /> base class by
/// adding an option to save the graph to an XPS file.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class SaveGraphImageFileDialog : SaveImageFileDialog
{
    //*************************************************************************
    //  Constructor: SaveGraphImageFileDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="SaveGraphImageFileDialog" /> class.
    /// </summary>
    ///
    /// <param name="intialDirectory">
    /// Initial directory the dialog will display.  Use an empty string to let
    /// the dialog select an initial directory.
    /// </param>
    ///
    /// <param name="intialFileName">
    /// Initial file name.  Can be a complete path, a path without an
    /// extension, a file name, or a file name without an extension.
    /// </param>
    //*************************************************************************

    public SaveGraphImageFileDialog
    (
        String intialDirectory,
        String intialFileName

    ) : base(intialDirectory, intialFileName)
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: ShowDialogAndSaveGraphImage()
    //
    /// <summary>
    /// Shows the file save dialog and saves the image to the selected file.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// The control the image will come from.
    /// </param>
    ///
    /// <param name="width">
    /// Width of the image to save.  If saving to XPS, the units are 1/100 of
    /// an inch.  Otherwise, the units are pixels.
    /// </param>
    ///
    /// <param name="height">
    /// Height of the image to save.  If saving to XPS, the units are 1/100 of
    /// an inch.  Otherwise, the units are pixels.
    /// </param>
    ///
    /// <returns>
    /// DialogResult.OK if the user selected a file name and the image was
    /// successfully saved.
    /// </returns>
    ///
    /// <remarks>
    /// This method allows the user to select an image file name and format.
    /// It then saves the image in the selected format.
    /// </remarks>
    //*************************************************************************

    public DialogResult
    ShowDialogAndSaveGraphImage
    (
        ExcelTemplateNodeXLControl nodeXLControl,
        Int32 width,
        Int32 height
    )
    {
        Debug.Assert(nodeXLControl != null);
        Debug.Assert(width > 0);
        Debug.Assert(height > 0);
        AssertValid();

        // Let the base class do most of the work.  The actual saving will be
        // done by SaveObject() in this class.  Wrap the information required
        // by SaveObject().

        GraphImageSource oGraphImageSource = new GraphImageSource();

        oGraphImageSource.NodeXLControl = nodeXLControl;
        oGraphImageSource.Width = width;
        oGraphImageSource.Height = height;

        return ( ShowDialogAndSaveObject(oGraphImageSource) );
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
        // Extend the base class file types to include XPS.

        return (SaveableImageFormats.Filter + "|XPS (*.xps)|*.xps" );
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
    /// This is called by the base-class ShowDialogAndSaveObject() method.
    /// </remarks>
    //*************************************************************************

    protected override void
    SaveObject
    (
        Object oObject,
        String sFileName
    )
    {
        Debug.Assert(oObject is GraphImageSource);
        Debug.Assert( !String.IsNullOrEmpty(sFileName) );

        GraphImageSource oGraphImageSource = (GraphImageSource)oObject;
        
        if (m_oSaveFileDialog.FilterIndex <=
            SaveableImageFormats.ImageFormats.Length)
        {
            // Tell the NodeXLControl to copy its graph to a bitmap, then let
            // the base class save it.

            Bitmap oBitmapCopy =
                oGraphImageSource.NodeXLControl.CopyGraphToBitmap(
                    oGraphImageSource.Width, oGraphImageSource.Height);

            base.SaveObject(oBitmapCopy, sFileName);

            oBitmapCopy.Dispose();
        }
        else
        {
            // The graph must be saved as an XPS.

            oGraphImageSource.NodeXLControl.SaveToXps(

                new System.Windows.Size(
                    ToWpsUnits(oGraphImageSource.Width),
                    ToWpsUnits(oGraphImageSource.Height)
                    ),
                sFileName
                );
        }
    }

    //*************************************************************************
    //  Method: ToWpsUnits()
    //
    /// <summary>
    /// Converts a height or width to WPS units.
    /// </summary>
    ///
    /// <param name="iHeightOrWidth">
    /// Height or width, in 1/100 of an inch.
    /// </param>
    ///
    /// <returns>
    /// Height or width, in WPS units (1/96 of an inch).
    /// </returns>
    //*************************************************************************

    protected Double
    ToWpsUnits
    (
        Int32 iHeightOrWidth
    )
    {
        AssertValid();

        return ( iHeightOrWidth * (96.0 / 100.0) );
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

        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Embedded class: GraphImageSource
    //
    /// <summary>
    /// Stores information about the graph image that needs to be saved.
    /// </summary>
    //*************************************************************************

    protected class GraphImageSource : Object
    {
        /// The control the image will come from.

        public ExcelTemplateNodeXLControl NodeXLControl;

        /// Width of the image to save.  If saving to XPS, the units are 1/100
        /// of an inch.  Otherwise, the units are pixels.

        public Int32 Width;

        /// Height of the image to save.

        public Int32 Height;
    }
}

}
