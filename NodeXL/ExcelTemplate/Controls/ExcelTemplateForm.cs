
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.Core;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ExcelTemplateForm
//
/// <summary>
/// Base class for all forms in the Excel template.
/// </summary>
///
/// <remarks>
/// This class adds commonly used functionality to the FormPlus class.  All
/// ExcelTemplate forms should derive from it.
/// </remarks>
//*****************************************************************************

public class ExcelTemplateForm : FormPlus
{
    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// String format for most Int32s.

    public static readonly String Int32Format = NodeXLBase.Int32Format;

    /// String format for most Singles.

    public static readonly String SingleFormat = NodeXLBase.SingleFormat;

    /// String format for most Doubles.

    public static readonly String DoubleFormat = NodeXLBase.DoubleFormat;

    /// Text to display in an empty list.

    public const String EmptyListText = "(none)";

    /// Text to display in a list to tell the user to select an item from the
    /// list.

    public const String SelectOneText = "(Select one)";


    //*************************************************************************
    //  Constructor: ExcelTemplateForm()
    //
    /// <summary>
    /// Initializes a new instance of the ExcelTemplateForm class.
    /// </summary>
    //*************************************************************************

    public ExcelTemplateForm()
    {
        this.ShowInTaskbar = false;
    }

    //*************************************************************************
    //  Method: EditFont()
    //
    /// <summary>
    /// Edits a Font object.
    /// </summary>
    ///
    /// <param name="oFont">
    /// The Font object to edit.
    /// </param>
    //*************************************************************************

    protected void
    EditFont
    (
        ref Font oFont
    )
    {
        AssertValid();

        FontDialog oFontDialog = new FontDialog();

        // Note that the FontDialog makes a copy of oFont, which is good.

        oFontDialog.Font = oFont;
        oFontDialog.FontMustExist = true;
        oFontDialog.ScriptsOnly = true;
        oFontDialog.ShowEffects = false;

        // The FontConverter class implicity used by ApplicationsSettingsBase,
        // which this program uses to persist user settings, does not persist
        // the font script.  Don't allow the user to change the script from the
        // default.

        oFontDialog.AllowScriptChange = false;

        try
        {
            if (oFontDialog.ShowDialog() == DialogResult.OK)
            {
                oFont = oFontDialog.Font;
            }
        }
        catch (ArgumentException)
        {
            // Known bug: Selecting the Visual UI font in the dialog throws an
            // ArgumentException of "Only TrueType fonts are supported. This is
            // not a TrueType font."  See the following post:
            //
            // http://social.msdn.microsoft.com/Forums/en-US/vbgeneral/thread/
            // e50a3dc2-a9d9-4eea-aae6-39bc7c18b04e
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    public override void
    AssertValid()
    {
        base.AssertValid();
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
