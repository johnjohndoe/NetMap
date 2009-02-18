

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Configuration;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: CreateSubgraphImagesDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="CreateSubgraphImagesDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("CreateSubgraphImagesDialog3") ]

public class CreateSubgraphImagesDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: CreateSubgraphImagesDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="CreateSubgraphImagesDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public CreateSubgraphImagesDialogUserSettings
    (
        Form oForm
    )
    : base (oForm, true)
    {
        Debug.Assert(oForm != null);

        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: Levels
    //
    /// <summary>
    /// Gets or sets the number of levels of adjacent vertices to include in
    /// each subgraph.
    /// </summary>
    ///
    /// <value>
    /// The number of levels of adjacent vertices to include in each subgraph.
    /// The default is 1.5.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("1.5") ]

    public Decimal
    Levels
    {
        get
        {
            AssertValid();

            return ( (Decimal)this[LevelsKey] );
        }

        set
        {
            this[LevelsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SaveToFolder
    //
    /// <summary>
    /// Gets or sets a flag indicating whether subgraph images should be saved
    /// to a folder.
    /// </summary>
    ///
    /// <value>
    /// true to save subgraph images to a folder.  The default is false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    SaveToFolder
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[SaveToFolderKey] );
        }

        set
        {
            this[SaveToFolderKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Folder
    //
    /// <summary>
    /// Gets or sets the folder to save subgraph images to.
    /// </summary>
    ///
    /// <value>
    /// The folder to save subgraph images to.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    Folder
    {
        get
        {
            AssertValid();

            return ( (String)this[FolderKey] );
        }

        set
        {
            this[FolderKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ImageSizePx
    //
    /// <summary>
    /// Gets or sets the size of each subgraph image saved to a folder.
    /// </summary>
    ///
    /// <value>
    /// The size of each subgraph image saved to a folder, in pixels.  The
    /// default is 200x200.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("200, 200") ]

    public Size
    ImageSizePx
    {
        get
        {
            AssertValid();

            return ( (Size)this[ImageSizePxKey] );
        }

        set
        {
            this[ImageSizePxKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ImageFormat
    //
    /// <summary>
    /// Gets or sets the format of each subgraph image saved to a folder.
    /// </summary>
    ///
    /// <value>
    /// The format of each subgraph image saved to a folder.  The default is
    /// ImageFormat.Jpeg.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("Jpeg") ]

    public ImageFormat
    ImageFormat
    {
        get
        {
            AssertValid();

            return ( (ImageFormat)this[ImageFormatKey] );
        }

        set
        {
            this[ImageFormatKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: InsertThumbnails
    //
    /// <summary>
    /// Gets or sets a flag indicating whether subgraph thumbnail images should
    /// be inserted in the vertex worksheet.
    /// </summary>
    ///
    /// <value>
    /// true to insert subgraph thumbnail images in the vertex worksheet.  The
    /// default is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    InsertThumbnails
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[InsertThumbnailsKey] );
        }

        set
        {
            this[InsertThumbnailsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ThumbnailSizePx
    //
    /// <summary>
    /// Gets or sets the size of each thumbnail image inserted in the vertex
    /// worksheet.
    /// </summary>
    ///
    /// <value>
    /// The size of each thumbnail image inserted in the vertex worksheet, in
    /// pixels.  The default is 76x50.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("76, 50") ]

    public Size
    ThumbnailSizePx
    {
        get
        {
            AssertValid();

            return ( (Size)this[ThumbnailSizePxKey] );
        }

        set
        {
            this[ThumbnailSizePxKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SelectedVerticesOnly
    //
    /// <summary>
    /// Gets or sets a flag indicating whether subgraph images should be
    /// created for selected vertices only.
    /// </summary>
    ///
    /// <value>
    /// true to create subgraph images for selected vertices only, false to
    /// create them for all images.  The default is false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    SelectedVerticesOnly
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[SelectedVerticesOnlyKey] );
        }

        set
        {
            this[SelectedVerticesOnlyKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SelectVertex
    //
    /// <summary>
    /// Gets or sets a flag indicating whether to select the vertex in each
    /// vertex's subgraph image.
    /// </summary>
    ///
    /// <value>
    /// true to select the vertex, false to leave it unselected.  The default
    /// is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    SelectVertex
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[SelectVertexKey] );
        }

        set
        {
            this[SelectVertexKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SelectIncidentEdges
    //
    /// <summary>
    /// Gets or sets a flag indicating whether to select the vertex's incident
    /// edges in each vertex's subgraph image.
    /// </summary>
    ///
    /// <value>
    /// true to select the vertex's incident edges, false to leave them
    /// unselected.  The default is false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    SelectIncidentEdges
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[SelectIncidentEdgesKey] );
        }

        set
        {
            this[SelectIncidentEdgesKey] = value;

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

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Name of the settings key for the Levels property.

    protected const String LevelsKey = "Levels";

    /// Name of the settings key for the SaveToFolder property.

    protected const String SaveToFolderKey = "SaveToFolder";

    /// Name of the settings key for the Folder property.

    protected const String FolderKey = "Folder";

    /// Name of the settings key for the ImageSizePx property.

    protected const String ImageSizePxKey = "ImageSizePx";

    /// Name of the settings key for the ImageFormat property.

    protected const String ImageFormatKey = "ImageFormat";

    /// Name of the settings key for the InsertThumbnails property.

    protected const String InsertThumbnailsKey = "InsertThumbnails";

    /// Name of the settings key for the ThumbnailSizePx property.

    protected const String ThumbnailSizePxKey = "ThumbnailSizePx";

    /// Name of the settings key for the SelectedVerticesOnly property.

    protected const String SelectedVerticesOnlyKey = "SelectedVerticesOnly";

    /// Name of the settings key for the SelectVertex property.

    protected const String SelectVertexKey = "SelectVertex";

    /// Name of the settings key for the SelectIncidentEdges property.

    protected const String SelectIncidentEdgesKey = "SelectIncidentEdges";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
