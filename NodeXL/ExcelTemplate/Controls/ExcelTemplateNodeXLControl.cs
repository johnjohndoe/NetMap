
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Diagnostics;
using Microsoft.NodeXL.Layouts;
using Microsoft.NodeXL.Visualization.Wpf;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ExcelTemplateNodeXLControl
//
/// <summary>
/// Version of NodeXLControl customized for the Excel Template.
/// </summary>
///
/// <remarks>
/// It is assumed that this control is hosted within a Panel.
/// </remarks>
//*****************************************************************************

public class ExcelTemplateNodeXLControl : NodeXLControl
{
    //*************************************************************************
    //  Constructor: ExcelTemplateNodeXLControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ExcelTemplateNodeXLControl" /> class.
    /// </summary>
    //*************************************************************************

    public ExcelTemplateNodeXLControl()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: SaveToXps()
    //
    /// <summary>
    /// Saves the graph to the specified XPS file.
    /// </summary>
    ///
    /// <param name="imageSize">
    /// Size of the XPS image, in WPS units.
    /// </param>
    ///
    /// <param name="fileName">
    /// File name to save to.
    /// </param>
    ///
    /// <remarks>
    /// This could conceivably be put in the base-class NodeXLControl class,
    /// but that would force all users of the control to add references to the
    /// XPS assemblies.
    /// </remarks>
    //*************************************************************************

    public void
    SaveToXps
    (
        Size imageSize,
        String fileName
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(fileName) );
        AssertValid();

        CheckIfDrawing("SaveToXps");

        // This control will be rehosted by a FixedPage.  It can't be a child
        // of logical trees, so disconnect it from its parent after saving the
        // current vertex locations.

        LayoutSaver oLayoutSaver = new LayoutSaver(this.Graph);

        Debug.Assert(this.Parent is Panel);
        Panel oParentPanel = (Panel)this.Parent;
        UIElementCollection oParentChildren = oParentPanel.Children;
        Int32 iChildIndex = oParentChildren.IndexOf(this);
        oParentChildren.Remove(this);

        GraphImageCenterer oGraphImageCenterer = new GraphImageCenterer(this);

        FixedDocument oFixedDocument = new FixedDocument();
        oFixedDocument.DocumentPaginator.PageSize = imageSize;
        PageContent oPageContent = new PageContent();

        FixedPage oFixedPage = new FixedPage();
        oFixedPage.Width = imageSize.Width;
        oFixedPage.Height = imageSize.Height;

        this.Width = imageSize.Width;
        this.Height = imageSize.Height;

        // Adjust the control's translate transforms so that the image will be
        // centered on the same point on the graph that the control is centered
        // on.

        oGraphImageCenterer.CenterGraphImage(imageSize);

        oFixedPage.Children.Add(this);
        oFixedPage.Measure(imageSize);

        oFixedPage.Arrange(new System.Windows.Rect(
            new System.Windows.Point(), imageSize) );

        oFixedPage.UpdateLayout();

        ( (System.Windows.Markup.IAddChild)oPageContent ).AddChild(
            oFixedPage);

        oFixedDocument.Pages.Add(oPageContent);

        try
        {
            XpsDocument oXpsDocument = new XpsDocument(fileName,
                FileAccess.Write);

            XpsDocumentWriter oXpsDocumentWriter =
                XpsDocument.CreateXpsDocumentWriter(oXpsDocument);

            oXpsDocumentWriter.Write(oFixedDocument);
            oXpsDocument.Close();
        }
        finally
        {
            // Reconnect the NodeXLControl to its original parent.  Reset the
            // size to Auto in the process.

            oFixedPage.Children.Remove(this);
            this.Width = Double.NaN;
            this.Height = Double.NaN;
            oGraphImageCenterer.RestoreCenter();
            oParentChildren.Insert(iChildIndex, this);

            // The graph may have shrunk when it was connected to the
            // FixedPage, and even though it will be expanded to its original
            // dimensions when UpdateLayout() is called below, the layout may
            // have lost "resolution" and the results may be poor.
            //
            // Fix this by restoring the original layout and redrawing the
            // graph.

            this.UpdateLayout();
            oLayoutSaver.RestoreLayout();
            this.DrawGraphAsync(false);
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
