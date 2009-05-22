
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Layouts;
using Microsoft.NodeXL.Visualization.Wpf;
using Microsoft.NodeXL.Algorithms;
using Microsoft.NodeXL.ApplicationUtil;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: SubgraphImageCreator
//
/// <summary>
/// Creates images of a subgraph for each of a graph's vertices and saves the
/// images to disk.
/// </summary>
///
/// <remarks>
/// Call <see cref="CreateSubgraphImagesAsync" /> to create the images.  Call
/// <see cref="CancelAsync" /> to stop the creation of images.  Handle the <see
/// cref="ImageCreationProgressChanged" /> and <see
/// cref="ImageCreationCompleted" /> events to monitor the progress and
/// completion of image creation.
/// </remarks>
//*****************************************************************************

public class SubgraphImageCreator : Object
{
    //*************************************************************************
    //  Constructor: SubgraphImageCreator()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="SubgraphImageCreator" />
    /// class.
    /// </summary>
    //*************************************************************************

    public SubgraphImageCreator()
    {
        m_oBackgroundWorker = null;

        AssertValid();
    }

    //*************************************************************************
    //  Property: IsBusy
    //
    /// <summary>
    /// Gets a flag indicating whether an asynchronous operation is in
    /// progress.
    /// </summary>
    ///
    /// <value>
    /// true if an asynchronous operation is in progress.
    /// </value>
    //*************************************************************************

    public Boolean
    IsBusy
    {
        get
        {
            return (m_oBackgroundWorker != null && m_oBackgroundWorker.IsBusy);
        }
    }

    //*************************************************************************
    //  Method: CreateSubgraphImagesAsync()
    //
    /// <summary>
    /// Asynchronously creates images of a subgraph for each of a graph's
    /// vertices and saves the images to disk.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to use.
    /// </param>
    ///
    /// <param name="selectedVertices">
    /// Array of the vertices in <paramref name="graph" /> that were selected
    /// by the user in the workbook from which the graph was created.  Can be
    /// empty but not null.
    /// </param>
    ///
    /// <param name="levels">
    /// The number of levels of adjacent vertices to include in each subgraph.
    /// Must be a multiple of 0.5.  If 0, a subgraph includes just the vertex;
    /// if 1, it includes the vertex and its adjacent vertices; if 2, it
    /// includes the vertex, its adjacent vertices, and their adjacent
    /// vertices; and so on.  The difference between N.5 and N.0 is that N.5
    /// includes any edges connecting the outermost vertices to each other,
    /// whereas N.0 does not.  1.5, for example, includes any edges that
    /// connect the vertex's adjacent vertices to each other, whereas 1.0
    /// includes only those edges that connect the adjacent vertices to the
    /// vertex.
    /// </param>
    ///
    /// <param name="saveToFolder">
    /// true to save subgraph images to a folder.
    /// </param>
    ///
    /// <param name="folder">
    /// The folder to save subgraph images to.  Used only if <paramref
    /// name="saveToFolder" /> is true.
    /// </param>
    ///
    /// <param name="imageSizePx">
    /// The size of each subgraph image saved to a folder, in pixels.  Used
    /// only if <paramref name="saveToFolder" /> is true.
    /// </param>
    ///
    /// <param name="imageFormat">
    /// The format of each subgraph image saved to a folder.  Used only if
    /// <paramref name="saveToFolder" /> is true.
    /// </param>
    ///
    /// <param name="createThumbnails">
    /// true to save thumbnail images to a temporary folder.
    /// </param>
    ///
    /// <param name="thumbnailSizePx">
    /// The size of each thumbnail image, in pixels.  Used only if <paramref
    /// name="createThumbnails" /> is true.
    /// </param>
    ///
    /// <param name="selectedVerticesOnly">
    /// true to create subgraph images for the vertices in <paramref
    /// name="selectedVertices" /> only, false to create them for all images.
    /// </param>
    ///
    /// <param name="selectVertex">
    /// true to select the vertex around which each subgraph is created.
    /// </param>
    ///
    /// <param name="selectIncidentEdges">
    /// true to select the incident edges of the vertex around which each
    /// subgraph is created.
    /// </param>
    ///
    /// <param name="generalUserSettings">
    /// The user's general user settings for the application.
    /// </param>
    ///
    /// <remarks>
    /// When image creation completes, the <see
    /// cref="ImageCreationCompleted" /> event fires.
    ///
    /// <para>
    /// If thumbnail images are created, they are saved to a temporary folder.
    /// Information about the thumbnail images can be found in the <see
    /// cref="TemporaryImages" /> object stored in the <see
    /// cref="RunWorkerCompletedEventArgs.Result" /> propery of the <see
    /// cref="RunWorkerCompletedEventArgs" /> returned by the <see
    /// cref="ImageCreationCompleted" /> event.
    /// </para>
    ///
    /// <para>
    /// To cancel the analysis, call <see cref="CancelAsync" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    CreateSubgraphImagesAsync
    (
        IGraph graph,
        IVertex [] selectedVertices,
        Decimal levels,
        Boolean saveToFolder,
        String folder,
        Size imageSizePx,
        ImageFormat imageFormat,
        Boolean createThumbnails,
        Size thumbnailSizePx,
        Boolean selectedVerticesOnly,
        Boolean selectVertex,
        Boolean selectIncidentEdges,
        GeneralUserSettings generalUserSettings
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(selectedVertices != null);
        Debug.Assert(levels >= 0);
        Debug.Assert(Decimal.Remainder(levels, 0.5M) == 0M);
        Debug.Assert( !saveToFolder || !String.IsNullOrEmpty(folder) );
        Debug.Assert( !saveToFolder || imageSizePx.Width > 0);
        Debug.Assert( !saveToFolder || imageSizePx.Height > 0);
        Debug.Assert( !createThumbnails || thumbnailSizePx.Width > 0);
        Debug.Assert( !createThumbnails || thumbnailSizePx.Height > 0);
        Debug.Assert(generalUserSettings != null);
        AssertValid();

        const String MethodName = "CreateSubgraphImagesAsync";

        if (this.IsBusy)
        {
            throw new InvalidOperationException( String.Format(

                "{0}:{1}: An asynchronous operation is already in progress."
                ,
                this.ClassName,
                MethodName
                ) );
        }

        // Wrap the arguments in an object that can be passed to
        // BackgroundWorker.RunWorkerAsync().

        CreateSubgraphImagesAsyncArgs oCreateSubgraphImagesAsyncArgs =
            new CreateSubgraphImagesAsyncArgs();

        oCreateSubgraphImagesAsyncArgs.Graph = graph;
        oCreateSubgraphImagesAsyncArgs.SelectedVertices = selectedVertices;
        oCreateSubgraphImagesAsyncArgs.Levels = levels;
        oCreateSubgraphImagesAsyncArgs.SaveToFolder = saveToFolder;
        oCreateSubgraphImagesAsyncArgs.Folder = folder;
        oCreateSubgraphImagesAsyncArgs.ImageSizePx = imageSizePx;
        oCreateSubgraphImagesAsyncArgs.ImageFormat = imageFormat;
        oCreateSubgraphImagesAsyncArgs.CreateThumbnails = createThumbnails;
        oCreateSubgraphImagesAsyncArgs.ThumbnailSizePx = thumbnailSizePx;

        oCreateSubgraphImagesAsyncArgs.SelectedVerticesOnly =
            selectedVerticesOnly;

        oCreateSubgraphImagesAsyncArgs.SelectVertex = selectVertex;

        oCreateSubgraphImagesAsyncArgs.SelectIncidentEdges =
            selectIncidentEdges;

        oCreateSubgraphImagesAsyncArgs.GeneralUserSettings =
            generalUserSettings;

        oCreateSubgraphImagesAsyncArgs.Layout =
            CreateLayout(generalUserSettings.LayoutUserSettings);

        // Note: the NodeXLVisual object can't be created yet, because it must
        // be created on the same thread that uses it.  It will get created by
        // BackgroundWorker_DoWork().

        oCreateSubgraphImagesAsyncArgs.NodeXLVisual = null;

        // Create a BackgroundWorker and handle its events.

        m_oBackgroundWorker = new BackgroundWorker();

        m_oBackgroundWorker.WorkerReportsProgress = true;
        m_oBackgroundWorker.WorkerSupportsCancellation = true;

        m_oBackgroundWorker.DoWork += new DoWorkEventHandler(
            BackgroundWorker_DoWork);

        m_oBackgroundWorker.ProgressChanged +=
            new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);

        m_oBackgroundWorker.RunWorkerCompleted +=
            new RunWorkerCompletedEventHandler(
                BackgroundWorker_RunWorkerCompleted);

        m_oBackgroundWorker.RunWorkerAsync(oCreateSubgraphImagesAsyncArgs);
    }

    //*************************************************************************
    //  Method: CancelAsync()
    //
    /// <summary>
    /// Cancels the image creation started by <see
    /// cref="CreateSubgraphImagesAsync" />.
    /// </summary>
    ///
    /// <remarks>
    /// When the image creation cancels, the <see
    /// cref="ImageCreationCompleted" /> event fires.  The <see
    /// cref="AsyncCompletedEventArgs.Cancelled" /> property will be true.
    /// </remarks>
    //*************************************************************************

    public void
    CancelAsync()
    {
        AssertValid();

        if (this.IsBusy)
        {
            m_oBackgroundWorker.CancelAsync();
        }
    }

    //*************************************************************************
    //  Event: ImageCreationProgressChanged
    //
    /// <summary>
    /// Occurs when progress is made during the image creation started by <see
    /// cref="CreateSubgraphImagesAsync" />.
    /// </summary>
    ///
    /// <remarks>
    /// The <see cref="ProgressChangedEventArgs.UserState" /> argument is a
    /// String describing the progress.  The String is suitable for display to
    /// the user.
    /// </remarks>
    //*************************************************************************

    public event ProgressChangedEventHandler ImageCreationProgressChanged;


    //*************************************************************************
    //  Event: ImageCreationCompleted
    //
    /// <summary>
    /// Occurs when the image creation started by <see
    /// cref="CreateSubgraphImagesAsync" /> completes, is cancelled, or
    /// encounters an error.
    /// </summary>
    ///
    /// <remarks>
    /// The <see cref="RunWorkerCompletedEventArgs.Result" /> argument is a
    /// <see cref="TemporaryImages" /> object that indicates whether thumbnail
    /// images were created.  The dictionary returned by <see
    /// cref="TemporaryImages.FileNames" /> has keys that are vertex names and
    /// values that are file names of the corresponding thumbnail image files,
    /// without a path.
    /// </remarks>
    //*************************************************************************

    public event RunWorkerCompletedEventHandler ImageCreationCompleted;


    //*************************************************************************
    //  Property: ClassName
    //
    /// <summary>
    /// Gets the full name of this class.
    /// </summary>
    ///
    /// <value>
    /// The full name of this class, suitable for use in error messages.
    /// </value>
    //*************************************************************************

    protected String
    ClassName
    {
        get
        {
            return (this.GetType().FullName);
        }
    }

    //*************************************************************************
    //  Method: CreateSubgraphImagesInternal()
    //
    /// <summary>
    /// Creates an image of a subgraph for each of a graph's vertices and saves
    /// the images to disk.
    /// </summary>
    ///
    /// <param name="oCreateSubgraphImagesAsyncArgs">
    /// Contains the arguments needed to asynchronously create subgraph images.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// A BackgroundWorker object.
    /// </param>
    ///
    /// <param name="oDoWorkEventArgs">
    /// A DoWorkEventArgs object.
    /// </param>
    //*************************************************************************

    protected void
    CreateSubgraphImagesInternal
    (
        CreateSubgraphImagesAsyncArgs oCreateSubgraphImagesAsyncArgs,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert(oCreateSubgraphImagesAsyncArgs != null);
        Debug.Assert(oBackgroundWorker != null);
        Debug.Assert(oDoWorkEventArgs != null);
        AssertValid();

        // Create an object to keep track of the thumbnail images this method
        // creates and stores in a temporary folder.

        TemporaryImages oThumbnailImages = new TemporaryImages();

        oThumbnailImages.ImageSizePx =
            oCreateSubgraphImagesAsyncArgs.ThumbnailSizePx;

        oDoWorkEventArgs.Result = oThumbnailImages;

        System.Collections.ICollection oVertices;

        if (oCreateSubgraphImagesAsyncArgs.SelectedVerticesOnly)
        {
            oVertices = oCreateSubgraphImagesAsyncArgs.SelectedVertices;
        }
        else
        {
            oVertices = oCreateSubgraphImagesAsyncArgs.Graph.Vertices;
        }

        Int32 iSubgraphsCreated = 0;

        Boolean bSaveToFolder = oCreateSubgraphImagesAsyncArgs.SaveToFolder;

        Boolean bCreateThumbnails =
            oCreateSubgraphImagesAsyncArgs.CreateThumbnails;

        if (bSaveToFolder || bCreateThumbnails)
        {
            foreach (IVertex oVertex in oVertices)
            {
                if (oBackgroundWorker.CancellationPending)
                {
                    if (oThumbnailImages.Folder != null)
                    {
                        // Delete the entire temporary folder.

                        Directory.Delete(oThumbnailImages.Folder, true);

                        oThumbnailImages.Folder = null;
                    }

                    oDoWorkEventArgs.Cancel = true;
                    break;
                }

                String sVertexName = oVertex.Name;

                oBackgroundWorker.ReportProgress(0,
                    String.Format(
                        "Creating subgraph image for \"{0}\"."
                        ,
                        sVertexName
                    ) );

                // Create a subgraph for the vertex.

                IGraph oSubgraph = CreateSubgraph(oVertex,
                    oCreateSubgraphImagesAsyncArgs);

                // Create and save images for the subgraph.

                CreateAndSaveSubgraphImages(oSubgraph, sVertexName,
                    oCreateSubgraphImagesAsyncArgs, oThumbnailImages);

                iSubgraphsCreated++;
            }
        }

        oBackgroundWorker.ReportProgress(0,
            String.Format(
                "Done.  Created {0} subgraph {1}."
                ,
                iSubgraphsCreated.ToString(ExcelTemplateForm.Int32Format),
                StringUtil.MakePlural("image", iSubgraphsCreated)
                ) );
    }

    //*************************************************************************
    //  Method: CreateAndSaveSubgraphImages()
    //
    /// <summary>
    /// Creates images of a subgraph for one of a graph's vertices and saves
    /// the images to disk.
    /// </summary>
    ///
    /// <param name="oSubgraph">
    /// The subgraph to create images for.
    /// </param>
    ///
    /// <param name="sVertexName">
    /// Name of the vertex the subgraph is for.
    /// </param>
    ///
    /// <param name="oCreateSubgraphImagesAsyncArgs">
    /// Contains the arguments needed to asynchronously create subgraph images.
    /// </param>
    ///
    /// <param name="oThumbnailImages">
    /// Keeps track of the thumbnail images this method creates and stores in a
    /// temporary folder.
    /// </param>
    ///
    /// <remarks>
    /// This method creates zero, one, or two images of a subgraph and saves
    /// them to disk.
    /// </remarks>
    //*************************************************************************

    protected void
    CreateAndSaveSubgraphImages
    (
        IGraph oSubgraph,
        String sVertexName,
        CreateSubgraphImagesAsyncArgs oCreateSubgraphImagesAsyncArgs,
        TemporaryImages oThumbnailImages
    )
    {
        Debug.Assert(oSubgraph != null);
        Debug.Assert( !String.IsNullOrEmpty(sVertexName) );
        Debug.Assert(oCreateSubgraphImagesAsyncArgs != null);
        Debug.Assert(oThumbnailImages != null);
        AssertValid();

        if (oCreateSubgraphImagesAsyncArgs.SaveToFolder)
        {
            CreateAndSaveSubgraphImageInFolder(oSubgraph, sVertexName,
                oCreateSubgraphImagesAsyncArgs);
        }

        if (oCreateSubgraphImagesAsyncArgs.CreateThumbnails)
        {
            CreateAndSaveThumbnailImage(oSubgraph, sVertexName,
                oCreateSubgraphImagesAsyncArgs, oThumbnailImages);
        }
    }

    //*************************************************************************
    //  Method: CreateAndSaveSubgraphImageInFolder()
    //
    /// <summary>
    /// Creates an image of a subgraph for one of a graph's vertices and saves
    /// the image to a specified folder.
    /// </summary>
    ///
    /// <param name="oSubgraph">
    /// The subgraph to create an image for.
    /// </param>
    ///
    /// <param name="sVertexName">
    /// Name of the vertex the subgraph is for.
    /// </param>
    ///
    /// <param name="oCreateSubgraphImagesAsyncArgs">
    /// Contains the arguments needed to asynchronously create subgraph images.
    /// </param>
    //*************************************************************************

    protected void
    CreateAndSaveSubgraphImageInFolder
    (
        IGraph oSubgraph,
        String sVertexName,
        CreateSubgraphImagesAsyncArgs oCreateSubgraphImagesAsyncArgs
    )
    {
        Debug.Assert(oSubgraph != null);
        Debug.Assert( !String.IsNullOrEmpty(sVertexName) );
        Debug.Assert(oCreateSubgraphImagesAsyncArgs != null);
        Debug.Assert(oCreateSubgraphImagesAsyncArgs.SaveToFolder);
        AssertValid();

        // Save the graph to a bitmap.

        Bitmap oBitmap = CreateSubgraphImage(oSubgraph,
            oCreateSubgraphImagesAsyncArgs,
            oCreateSubgraphImagesAsyncArgs.ImageSizePx);

        try
        {
            // Save the bitmap in the specified folder.

            SaveSubgraphImage(oBitmap,
                oCreateSubgraphImagesAsyncArgs.Folder,
                sVertexName, oCreateSubgraphImagesAsyncArgs
                );
        }
        finally
        {
            GraphicsUtil.DisposeBitmap(ref oBitmap);
        }
    }

    //*************************************************************************
    //  Method: CreateAndSaveThumbnailImage()
    //
    /// <summary>
    /// Creates a thumbnail image of a subgraph for one of a graph's vertices.
    /// </summary>
    ///
    /// <param name="oSubgraph">
    /// The subgraph to create an image for.
    /// </param>
    ///
    /// <param name="sVertexName">
    /// Name of the vertex the subgraph is for.
    /// </param>
    ///
    /// <param name="oCreateSubgraphImagesAsyncArgs">
    /// Contains the arguments needed to asynchronously create subgraph images.
    /// </param>
    ///
    /// <param name="oThumbnailImages">
    /// Keeps track of the thumbnail images this method creates and stores in a
    /// temporary folder.
    /// </param>
    //*************************************************************************

    protected void
    CreateAndSaveThumbnailImage
    (
        IGraph oSubgraph,
        String sVertexName,
        CreateSubgraphImagesAsyncArgs oCreateSubgraphImagesAsyncArgs,
        TemporaryImages oThumbnailImages
    )
    {
        Debug.Assert(oSubgraph != null);
        Debug.Assert( !String.IsNullOrEmpty(sVertexName) );
        Debug.Assert(oCreateSubgraphImagesAsyncArgs != null);
        Debug.Assert(oCreateSubgraphImagesAsyncArgs.CreateThumbnails);
        Debug.Assert(oThumbnailImages != null);
        AssertValid();

        if (oThumbnailImages.Folder == null)
        {
            // Create a temporary folder where the thumbnail images will be
            // stored.

            String sTemporaryFolder = Path.Combine(
                Path.GetTempPath(),
                Path.GetRandomFileName()
                );

            Directory.CreateDirectory(sTemporaryFolder);

            oThumbnailImages.Folder = sTemporaryFolder;
        }

        // Save the graph to a bitmap.

        Bitmap oBitmap = CreateSubgraphImage(oSubgraph,
            oCreateSubgraphImagesAsyncArgs,
            oCreateSubgraphImagesAsyncArgs.ThumbnailSizePx);

        try
        {
            // Save the bitmap in the temporary folder.

            String sTemporaryFileName = SaveSubgraphImage(oBitmap,
                oThumbnailImages.Folder, sVertexName,
                oCreateSubgraphImagesAsyncArgs
                );

            // Add the file name to the dictionary.  They key is the vertex
            // name and the value is the file name, without a path.

            oThumbnailImages.FileNames[sVertexName] = sTemporaryFileName;
        }
        finally
        {
            GraphicsUtil.DisposeBitmap(ref oBitmap);
        }
    }

    //*************************************************************************
    //  Method: CreateSubgraphImage()
    //
    /// <summary>
    /// Creates an image of a subgraph for one of a graph's vertices.
    /// </summary>
    ///
    /// <param name="oSubgraph">
    /// The subgraph to create an image for.
    /// </param>
    ///
    /// <param name="oCreateSubgraphImagesAsyncArgs">
    /// Contains the arguments needed to asynchronously create subgraph images.
    /// </param>
    ///
    /// <param name="oImageSizePx">
    /// Size of the image, in pixels.
    /// </param>
    ///
    /// <returns>
    /// The subgraph image, as a Bitmap.
    /// </returns>
    //*************************************************************************

    protected Bitmap
    CreateSubgraphImage
    (
        IGraph oSubgraph,
        CreateSubgraphImagesAsyncArgs oCreateSubgraphImagesAsyncArgs,
        Size oImageSizePx
    )
    {
        Debug.Assert(oSubgraph != null);
        Debug.Assert(oCreateSubgraphImagesAsyncArgs != null);
        AssertValid();

        Rectangle oSubgraphRectangle = new Rectangle(new Point(0, 0),
            oImageSizePx);

        // Lay out the graph, then draw it using the NodeXLVisual object.

        oCreateSubgraphImagesAsyncArgs.Layout.LayOutGraph( oSubgraph,
            new LayoutContext(oSubgraphRectangle) );

        NodeXLVisual oNodeXLVisual =
            oCreateSubgraphImagesAsyncArgs.NodeXLVisual;

        GraphDrawingContext oGraphDrawingContext =
            CreateGraphDrawingContext(oSubgraphRectangle,
                oCreateSubgraphImagesAsyncArgs.GeneralUserSettings);

        oNodeXLVisual.GraphDrawer.DrawGraph(oSubgraph, oGraphDrawingContext);

        // Save the graph to a bitmap.

        Bitmap oBitmap = WpfGraphicsUtil.VisualToBitmap(oNodeXLVisual,
            oSubgraphRectangle.Width, oSubgraphRectangle.Height);

        return (oBitmap);
    }

    //*************************************************************************
    //  Method: CreateLayout()
    //
    /// <summary>
    /// Creates a layout to use when laying out the subgraphs.
    /// </summary>
    ///
    /// <param name="oLayoutUserSettings">
    /// User settings for layouts.
    /// </param>
    ///
    /// <returns>
    /// The layout to use.
    /// </returns>
    //*************************************************************************

    protected IAsyncLayout
    CreateLayout
    (
        LayoutUserSettings oLayoutUserSettings
    )
    {
        Debug.Assert(oLayoutUserSettings != null);
        AssertValid();

        LayoutManager oLayoutManager = new LayoutManager();
        oLayoutManager.Layout = oLayoutUserSettings.Layout;
        IAsyncLayout oLayout = oLayoutManager.CreateLayout();
        oLayout.Margin = oLayoutUserSettings.Margin;

        return (oLayout);
    }

    //*************************************************************************
    //  Method: CreateNodeXLVisual()
    //
    /// <summary>
    /// Creates a NodeXLVisual object to use to draw subgraphs.
    /// </summary>
    ///
    /// <param name="oGeneralUserSettings">
    /// The user's general settings.
    /// </param>
    ///
    /// <returns>
    /// A NodeXLVisual object to use to draw subgraphs.
    /// </returns>
    //*************************************************************************

    protected NodeXLVisual
    CreateNodeXLVisual
    (
        GeneralUserSettings oGeneralUserSettings
    )
    {
        Debug.Assert(oGeneralUserSettings != null);
        AssertValid();

        NodeXLVisual oNodeXLVisual = new NodeXLVisual();

        GraphDrawer oGraphDrawer = oNodeXLVisual.GraphDrawer;

        VertexDrawer oVertexDrawer = oGraphDrawer.VertexDrawer;

        EdgeDrawer oEdgeDrawer = oGraphDrawer.EdgeDrawer;

        oGeneralUserSettings.TransferToGraphDrawer(oGraphDrawer);

        // Override some of the user's settings.

        oVertexDrawer.Shape = VertexShape.Disk;
        oVertexDrawer.Radius = 2.0;
        oEdgeDrawer.Width = 1;
        oEdgeDrawer.SelectedWidth = 1;

        return (oNodeXLVisual);
    }

    //*************************************************************************
    //  Method: CreateGraphDrawingContext()
    //
    /// <summary>
    /// Creates a GraphDrawingContext object to use when drawing the subgraphs.
    /// </summary>
    ///
    /// <param name="oSubgraphRectangle">
    /// The subgraph rectangle.
    /// </param>
    ///
    /// <param name="oGeneralUserSettings">
    /// The user's general settings.
    /// </param>
    ///
    /// <returns>
    /// A GraphDrawingContext object to use when drawing the subgraphs.
    /// </returns>
    //*************************************************************************

    protected GraphDrawingContext
    CreateGraphDrawingContext
    (
        Rectangle oSubgraphRectangle,
        GeneralUserSettings oGeneralUserSettings
    )
    {
        Debug.Assert(oGeneralUserSettings != null);
        AssertValid();

        return ( new GraphDrawingContext(

            WpfGraphicsUtil.RectangleToRect(oSubgraphRectangle),
            oGeneralUserSettings.LayoutUserSettings.Margin,

            WpfGraphicsUtil.ColorToWpfColor(oGeneralUserSettings.BackColor) 
            ) );
    }

    //*************************************************************************
    //  Method: CreateSubgraph()
    //
    /// <summary>
    /// Creates a subgraph for one of a graph's vertices.
    /// </summary>
    ///
    /// <param name="oOriginalVertex">
    /// The vertex to create a subgraph for.
    /// </param>
    ///
    /// <param name="oCreateSubgraphImagesAsyncArgs">
    /// Contains the arguments needed to asynchronously create subgraph images.
    /// </param>
    ///
    /// <returns>
    /// A new subgraph.
    /// </returns>
    //*************************************************************************

    protected IGraph
    CreateSubgraph
    (
        IVertex oOriginalVertex,
        CreateSubgraphImagesAsyncArgs oCreateSubgraphImagesAsyncArgs
    )
    {
        Debug.Assert(oOriginalVertex != null);
        Debug.Assert(oCreateSubgraphImagesAsyncArgs != null);
        AssertValid();

        // Create a new empty graph that will contain the vertex's subgraph.

        IGraph oSubgraph = new Graph();

        // Clone the original vertex, its adjacent vertices, and the connecting
        // edges into the subgraph.

        Decimal decLevels = oCreateSubgraphImagesAsyncArgs.Levels;

        IVertex oSubgraphVertex = CloneVertexIntoSubgraph(oOriginalVertex,
            oSubgraph, decLevels);

        if (oCreateSubgraphImagesAsyncArgs.SelectVertex)
        {
            // Select the vertex.

            oSubgraphVertex.SetValue(ReservedMetadataKeys.IsSelected, true);
        }

        if (oCreateSubgraphImagesAsyncArgs.SelectIncidentEdges)
        {
            // Select the vertex's incident edges.

            foreach (IEdge oIncidentEdge in oSubgraphVertex.IncidentEdges)
            {
                oIncidentEdge.SetValue(ReservedMetadataKeys.IsSelected, true);
            }
        }

        return (oSubgraph);
    }

    //*************************************************************************
    //  Method: SaveSubgraphImage()
    //
    /// <summary>
    /// Saves an image of a subgraph to disk.
    /// </summary>
    ///
    /// <param name="oBitmap">
    /// Subgraph image.
    /// </param>
    ///
    /// <param name="sFolder">
    /// Full path to the folder to save the image to.
    /// </param>
    ///
    /// <param name="sFileNameNoExtension">
    /// Name of the file to save the image to, without a path or extension.
    /// </param>
    ///
    /// <param name="oCreateSubgraphImagesAsyncArgs">
    /// Contains the arguments needed to asynchronously create subgraph images.
    /// </param>
    ///
    /// <returns>
    /// The name of the file the image was saved to, without a path.
    /// </returns>
    //*************************************************************************

    protected String
    SaveSubgraphImage
    (
        Bitmap oBitmap,
        String sFolder,
        String sFileNameNoExtension,
        CreateSubgraphImagesAsyncArgs oCreateSubgraphImagesAsyncArgs
    )
    {
        Debug.Assert(oBitmap != null);
        Debug.Assert( !String.IsNullOrEmpty(sFolder) );
        Debug.Assert( !String.IsNullOrEmpty(sFileNameNoExtension) );
        Debug.Assert(oCreateSubgraphImagesAsyncArgs != null);
        AssertValid();

        ImageFormat eImageFormat = oCreateSubgraphImagesAsyncArgs.ImageFormat;

        String sFileNameWithoutPath =
            FileUtil.EncodeIllegalFileNameChars(sFileNameNoExtension)
            + "." + SaveableImageFormats.GetFileExtension(eImageFormat);

        String sFileNameWithPath = Path.Combine(sFolder, sFileNameWithoutPath);

        SaveBitmap(oBitmap, sFileNameWithPath, eImageFormat);

        return (sFileNameWithoutPath);
    }

    //*************************************************************************
    //  Method: SaveBitmap()
    //
    /// <summary>
    /// Saves one bitmap to a file.
    /// </summary>
    ///
    /// <param name="oBitmap">
    /// Bitmap to save.
    /// </param>
    ///
    /// <param name="sFileNameWithPath">
    /// File name where the bitmap will be saved, with a path.
    /// </param>
    ///
    /// <param name="eImageFormat">
    /// Format in which to save the bitmap.
    /// </param>
    //*************************************************************************

    protected void
    SaveBitmap
    (
        Bitmap oBitmap,
        String sFileNameWithPath,
        ImageFormat eImageFormat
    )
    {
        Debug.Assert(oBitmap != null);
        Debug.Assert( !String.IsNullOrEmpty(sFileNameWithPath) );

        // Check whether the file exists and is read-only.

        if ( File.Exists(sFileNameWithPath) &&

            ( (File.GetAttributes(sFileNameWithPath)
                & FileAttributes.ReadOnly) != 0 ) )
        {
            throw new IOException( String.Format(

                "The file \"{0}\" is read-only and can't be overwritten."
                + "  Delete the file and create the images again."
                ,
                sFileNameWithPath
                ) );
        }

        if ( sFileNameWithPath.Length > FileUtil.MaximumFileNameLength
            ||
            Path.GetDirectoryName(sFileNameWithPath).Length >
                FileUtil.MaximumFolderNameLength
            )
        {
            throw new IOException( String.Format(

                "The subgraph image \"{0}\" can't be saved because the file"
                + " name is too long.  File names can't be longer than {1}"
                + " characters, and folder names can't be longer than {2}"
                + " characters.  Either use a subgraph image folder with a"
                + " shorter name, or shorten the vertex name."
                ,
                sFileNameWithPath,
                FileUtil.MaximumFileNameLength,
                FileUtil.MaximumFolderNameLength
                ) );
        }

        // Save the image.

        GraphicsUtil.SaveHighQualityImage(oBitmap, sFileNameWithPath,
            eImageFormat);
    }

    //*************************************************************************
    //  Method: CloneVertexIntoSubgraph()
    //
    /// <summary>
    /// Clones a vertex, its adjacent vertices, and the connecting edges from
    /// the original graph into a subgraph.
    /// </summary>
    ///
    /// <param name="oOriginalVertex">
    /// The vertex to clone.
    /// </param>
    ///
    /// <param name="oSubgraph">
    /// The subgraph to clone the vertex into.
    /// </param>
    ///
    /// <param name="decLevels">
    /// The number of levels of adjacent vertices to include in each subgraph.
    /// </param>
    ///
    /// <returns>
    /// The clone of <paramref name="oOriginalVertex" />.
    /// </returns>
    //*************************************************************************

    protected IVertex
    CloneVertexIntoSubgraph
    (
        IVertex oOriginalVertex,
        IGraph oSubgraph,
        Decimal decLevels
    )
    {
        Debug.Assert(oOriginalVertex != null);
        Debug.Assert(oSubgraph != null);
        Debug.Assert(decLevels >= 0);
        AssertValid();

        // Get the original vertices and edges to clone.  For the vertex
        // dictionary, the key is the IVertex and the value is the vertex's
        // level, which is the distance of the vertex from oOriginalVertex.
        // For the edge dictionary, the key is the IEdge and the value isn't
        // used.

        Dictionary<IVertex, Int32> oOriginalVerticesToClone;
        Dictionary<IEdge, Char> oOriginalEdgesToClone;

        SubgraphCalculator.GetSubgraph(oOriginalVertex, decLevels, true,
            out oOriginalVerticesToClone, out oOriginalEdgesToClone);

        // Clone the vertices.  This dictionary maps the IDs of the original
        // vertices to their clones.

        Dictionary<Int32, IVertex> oOriginalToSubgraphVertexMapper =
            new Dictionary<Int32, IVertex>();

        IVertexCollection oSubgraphVertices = oSubgraph.Vertices;

        foreach (IVertex oOriginalVertexToClone in
            oOriginalVerticesToClone.Keys)
        {
            IVertex oSubgraphVertex =
                oOriginalVertexToClone.Clone(false, false);

            oSubgraphVertices.Add(oSubgraphVertex);

            oOriginalToSubgraphVertexMapper.Add(oOriginalVertexToClone.ID,
                oSubgraphVertex);
        }

        // This dictionary is no longer needed.

        oOriginalVerticesToClone.Clear();
        oOriginalVerticesToClone = null;

        // Clone the edges.

        IEdgeCollection oSubgraphEdges = oSubgraph.Edges;

        foreach (IEdge oOriginalEdgeToClone in oOriginalEdgesToClone.Keys)
        {
            IVertex [] aoOriginalVertices = oOriginalEdgeToClone.Vertices;

            IVertex oSubgraphVertex1 =
                oOriginalToSubgraphVertexMapper[aoOriginalVertices[0].ID];

            IVertex oSubgraphVertex2 =
                oOriginalToSubgraphVertexMapper[aoOriginalVertices[1].ID];

            oSubgraphEdges.Add(oSubgraphVertex1, oSubgraphVertex2,
                oOriginalEdgeToClone.IsDirected);
        }

        Debug.Assert( oOriginalToSubgraphVertexMapper.ContainsKey(
            oOriginalVertex.ID) );

        return ( oOriginalToSubgraphVertexMapper[oOriginalVertex.ID] );
    }

    //*************************************************************************
    //  Method: BackgroundWorker_DoWork()
    //
    /// <summary>
    /// Handles the DoWork event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected void
    BackgroundWorker_DoWork
    (
        object sender,
        DoWorkEventArgs e
    )
    {
        Debug.Assert(sender is BackgroundWorker);
        AssertValid();

        BackgroundWorker oBackgroundWorker = (BackgroundWorker)sender;

        Debug.Assert(e.Argument is CreateSubgraphImagesAsyncArgs);

        CreateSubgraphImagesAsyncArgs oCreateSubgraphImagesAsyncArgs =
            (CreateSubgraphImagesAsyncArgs)e.Argument;

        // The NodeXLVisual object couldn't be created by
        // CreateSubgraphImagesAsync(), because the object must be created by
        // the thread that uses it.  Create it now.

        oCreateSubgraphImagesAsyncArgs.NodeXLVisual = CreateNodeXLVisual(
            oCreateSubgraphImagesAsyncArgs.GeneralUserSettings);

        CreateSubgraphImagesInternal(oCreateSubgraphImagesAsyncArgs,
            m_oBackgroundWorker, e);
    }

    //*************************************************************************
    //  Method: BackgroundWorker_ProgressChanged()
    //
    /// <summary>
    /// Handles the ProgressChanged event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event arguments.
    /// </param>
    //*************************************************************************

    protected void
    BackgroundWorker_ProgressChanged
    (
        object sender,
        ProgressChangedEventArgs e
    )
    {
        AssertValid();

        // Forward the event.

        ProgressChangedEventHandler oImageCreationProgressChanged =
            this.ImageCreationProgressChanged;

        if (oImageCreationProgressChanged != null)
        {
            oImageCreationProgressChanged(this, e);
        }
    }

    //*************************************************************************
    //  Method: BackgroundWorker_RunWorkerCompleted()
    //
    /// <summary>
    /// Handles the RunWorkerCompleted event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected void
    BackgroundWorker_RunWorkerCompleted
    (
        object sender,
        RunWorkerCompletedEventArgs e
    )
    {
        AssertValid();

        // Forward the event.

        RunWorkerCompletedEventHandler oImageCreationCompleted =
            this.ImageCreationCompleted;

        if (oImageCreationCompleted != null)
        {
            // If the operation was successful, the
            // RunWorkerCompletedEventArgs.Result must be a TemporaryImages
            // object.  (Actually, it's always a TemporaryImages object
            // regardless of the operation's outcome, but you can't read the
            // Result property unless the operation was successful.)

            Debug.Assert(e.Cancelled || e.Error != null ||
                e.Result is TemporaryImages);

            oImageCreationCompleted(this, e);
        }

        m_oBackgroundWorker = null;
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
        // m_oBackgroundWorker
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Used for asynchronous analysis.  null if an asynchronous analysis is
    /// not in progress.

    protected BackgroundWorker m_oBackgroundWorker;


    //*************************************************************************
    //  Embedded class: CreateSubgraphImagesAsyncArguments()
    //
    /// <summary>
    /// Contains the arguments needed to asynchronously create subgraph images.
    /// </summary>
    //*************************************************************************

    protected class CreateSubgraphImagesAsyncArgs
    {
        ///
        public IGraph Graph;
        ///
        public IVertex [] SelectedVertices;
        ///
        public Decimal Levels;
        ///
        public Boolean SaveToFolder;
        ///
        public String Folder;
        ///
        public Size ImageSizePx;
        ///
        public ImageFormat ImageFormat;
        ///
        public Boolean CreateThumbnails;
        ///
        public Size ThumbnailSizePx;
        ///
        public Boolean SelectedVerticesOnly;
        ///
        public Boolean SelectVertex;
        ///
        public Boolean SelectIncidentEdges;
        ///
        public GeneralUserSettings GeneralUserSettings;
        ///
        public IAsyncLayout Layout;
        ///
        public NodeXLVisual NodeXLVisual;
    };
}

}
