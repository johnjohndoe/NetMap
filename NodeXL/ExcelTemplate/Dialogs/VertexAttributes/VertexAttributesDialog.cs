

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Visualization.Wpf;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexAttributesDialog
//
/// <summary>
/// Dialog that lets the user edit the attributes of the selected vertices in a
/// NodeXLControl.
/// </summary>
///
/// <remarks>
/// Pass the NodeXLControl to the constructor.  If the user performs any edits
/// and the edits don't require that the workbook be read again, this dialog
/// applies the edits to the NodeXLControl's selected vertices and returns
/// DialogResult.OK from <see cref="Form.ShowDialog()" />.  The list of edited
/// attributes can then be obtained from the <see
/// cref="EditedVertexAttributes" /> property.
///
/// <para>
/// If the user performs edits that require that the workbook be read again,
/// no edits are applied to the selected vertices.  Instead, the caller must
/// force the workbook to be read again.  The caller can determine if this is
/// required by checking the <see
/// cref="ExcelTemplate.EditedVertexAttributes.WorkbookMustBeReread" />
/// property.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class VertexAttributesDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: VertexAttributesDialog()
    //
    /// <overloads>
    /// Initializes a new instance of the <see cref="VertexAttributesDialog" />
    /// class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexAttributesDialog" />
    /// class.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// NodeXLControl whose vertex attributes need to be edited.
    /// </param>
    //*************************************************************************

    public VertexAttributesDialog
    (
        NodeXLControl nodeXLControl
    )
    :
    this()
    {
        // Instantiate an object that retrieves and saves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oVertexAttributesDialogUserSettings =
            new VertexAttributesDialogUserSettings(this);

        m_oNodeXLControl = nodeXLControl;

        m_oEditedVertexAttributes = GetInitialVertexAttributes();

        ( new VertexShapeConverter() ).PopulateComboBox(cbxShape, true);

        nudRadius.Minimum =
            (Decimal)VertexRadiusConverter.MinimumRadiusWorkbook;

        nudRadius.Maximum = 
            (Decimal)VertexRadiusConverter.MaximumRadiusWorkbook;

        nudAlpha.Minimum = (Decimal)AlphaConverter.MinimumAlphaWorkbook;
        nudAlpha.Maximum = (Decimal)AlphaConverter.MaximumAlphaWorkbook;

        VertexVisibilityConverter oVertexVisibilityConverter = 
            new VertexVisibilityConverter();

        cbxVisibility.PopulateWithObjectsAndText(

            NotEditedMarker, String.Empty,

            VertexWorksheetReader.Visibility.Skip,
                oVertexVisibilityConverter.GraphToWorkbook(
                    VertexWorksheetReader.Visibility.Skip),

            VertexWorksheetReader.Visibility.Hide,
                oVertexVisibilityConverter.GraphToWorkbook(
                    VertexWorksheetReader.Visibility.Hide)
            );

        ( new VertexLabelPositionConverter() ).PopulateComboBox(
            cbxLabelPosition, true);

        BooleanConverter oBooleanConverter = new BooleanConverter();

        oBooleanConverter.PopulateComboBox(cbxLocked, true);
        oBooleanConverter.PopulateComboBox(cbxMarked, true);

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: VertexAttributesDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexAttributesDialog" />
    /// class for the Visual Studio designer.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for use by the Visual Studio
    /// designer only.
    /// </remarks>
    //*************************************************************************

    public VertexAttributesDialog()
    {
        InitializeComponent();
    }

    //*************************************************************************
    //  Property: EditedVertexAttributes
    //
    /// <summary>
    /// Gets the attributes that were applied to the NodeXLControl's selected
    /// vertices.
    /// </summary>
    ///
    /// <value>
    /// The attributes that were applied to the NodeXLControl's selected
    /// vertices.
    /// </value>
    ///
    /// <remarks>
    /// Do not read this property if <see cref="Form.ShowDialog()" /> doesn't
    /// return DialogResult.OK.
    /// </remarks>
    //*************************************************************************

    public EditedVertexAttributes
    EditedVertexAttributes
    {
        get
        {
            Debug.Assert(this.DialogResult == DialogResult.OK);
            AssertValid();

            return (m_oEditedVertexAttributes);
        }
    }

    //*************************************************************************
    //  Method: DoDataExchange()
    //
    /// <summary>
    /// Transfers data between the dialog's fields and its controls.
    /// </summary>
    ///
    /// <param name="bFromControls">
    /// true to transfer data from the dialog's controls to its fields, false
    /// for the other direction.
    /// </param>
    ///
    /// <returns>
    /// true if the transfer was successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    DoDataExchange
    (
        Boolean bFromControls
    )
    {
        if (bFromControls)
        {
            // Validate the controls.

            Nullable<Single> fRadius;
            Single fRadius2;

            if (nudRadius.Text == NotEditedMarker)
            {
                fRadius = null;
            }
            else if ( ValidateNumericUpDown(
                nudRadius, "a size", out fRadius2) )
            {
                fRadius = fRadius2;
            }
            else
            {
                return (false);
            }

            Nullable<Single> fAlpha;
            Single fAlpha2;

            if (nudAlpha.Text == NotEditedMarker)
            {
                fAlpha = null;
            }
            else if ( ValidateNumericUpDown(
                nudAlpha, "an opacity", out fAlpha2) )
            {
                fAlpha = fAlpha2;
            }
            else
            {
                return (false);
            }

            // The controls are valid.

            // Selected color.

            Color oSelectedColor = usrColor.Color;

            if (oSelectedColor == ColorNotEditedMarker)
            {
                m_oEditedVertexAttributes.Color = null;
            }
            else
            {
                m_oEditedVertexAttributes.Color = oSelectedColor;
            }

            // Shape.

            Object oSelectedShape = cbxShape.SelectedValue;

            m_oEditedVertexAttributes.Shape = (oSelectedShape is String) ?
                null : ( Nullable<VertexShape> )oSelectedShape;

            m_oEditedVertexAttributes.Radius = fRadius;
            m_oEditedVertexAttributes.Alpha = fAlpha;

            // Visibility.

            Object oSelectedVisibility = cbxVisibility.SelectedValue;

            if (oSelectedVisibility is String)
            {
                m_oEditedVertexAttributes.Visibility = null;
                m_oEditedVertexAttributes.WorkbookMustBeReread = false;
            }
            else
            {
                m_oEditedVertexAttributes.Visibility = 
                    ( Nullable<VertexWorksheetReader.Visibility> )
                        oSelectedVisibility;

                if (m_oEditedVertexAttributes.Visibility ==
                    VertexWorksheetReader.Visibility.Skip)
                {
                    // Skipping a vertex involves more than just editing
                    // vertex metadata.  Force the workbook to be reread.

                    m_oEditedVertexAttributes.WorkbookMustBeReread = true;
                }
            }

            // Label position.

            Object oSelectedLabelPosition = cbxLabelPosition.SelectedValue;

            m_oEditedVertexAttributes.LabelPosition =
                (oSelectedLabelPosition is String) ?
                null :
                ( Nullable<VertexLabelPosition> )oSelectedLabelPosition;

            // Locked.

            Object oSelectedLocked = cbxLocked.SelectedValue;

            m_oEditedVertexAttributes.Locked = (oSelectedLocked is String) ?
                null : ( Nullable<Boolean> )oSelectedLocked;

            // Marked.

            Object oSelectedMarked = cbxMarked.SelectedValue;

            m_oEditedVertexAttributes.Marked = (oSelectedMarked is String) ?
                null : ( Nullable<Boolean> )oSelectedMarked;
        }
        else
        {
            // Color.

            if (m_oEditedVertexAttributes.Color.HasValue)
            {
                usrColor.Color = m_oEditedVertexAttributes.Color.Value;
            }
            else
            {
                usrColor.Color = ColorNotEditedMarker;
            }

            // Shape.

            if (m_oEditedVertexAttributes.Shape.HasValue)
            {
                cbxShape.SelectedValue =
                    m_oEditedVertexAttributes.Shape.Value;
            }
            else
            {
                cbxShape.SelectedValue = NotEditedMarker;
            }

            // Radius.

            if (m_oEditedVertexAttributes.Radius.HasValue)
            {
                nudRadius.Value =
                    (Decimal)m_oEditedVertexAttributes.Radius.Value;
            }
            else
            {
                nudRadius.Text = NotEditedMarker;
            }

            // Alpha.

            if (m_oEditedVertexAttributes.Alpha.HasValue)
            {
                nudAlpha.Value =
                    (Decimal)m_oEditedVertexAttributes.Alpha.Value;
            }
            else
            {
                nudAlpha.Text = NotEditedMarker;
            }

            // Visibility.

            if (m_oEditedVertexAttributes.Visibility.HasValue)
            {
                cbxVisibility.SelectedValue =
                    m_oEditedVertexAttributes.Visibility.Value;
            }
            else
            {
                cbxVisibility.SelectedValue = NotEditedMarker;
            }

            // Label position.

            if (m_oEditedVertexAttributes.LabelPosition.HasValue)
            {
                cbxLabelPosition.SelectedValue =
                    m_oEditedVertexAttributes.LabelPosition.Value;
            }
            else
            {
                cbxLabelPosition.SelectedValue = NotEditedMarker;
            }

            // Locked.

            if (m_oEditedVertexAttributes.Locked.HasValue)
            {
                cbxLocked.SelectedValue =
                    m_oEditedVertexAttributes.Locked.Value;
            }
            else
            {
                cbxLocked.SelectedValue = NotEditedMarker;
            }

            // Marked.

            if (m_oEditedVertexAttributes.Marked.HasValue)
            {
                cbxMarked.SelectedValue =
                    m_oEditedVertexAttributes.Marked.Value;
            }
            else
            {
                cbxMarked.SelectedValue = NotEditedMarker;
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: GetInitialVertexAttributes()
    //
    /// <summary>
    /// Gets the list of attributes that should be shown to the user when the
    /// dialog opens.
    /// </summary>
    ///
    /// <returns>
    /// A <see cref="EditedVertexAttributes" /> object.
    /// </returns>
    //*************************************************************************

    protected EditedVertexAttributes
    GetInitialVertexAttributes()
    {
        // AssertValid();

        EditedVertexAttributes oInitialVertexAttributes =
            new EditedVertexAttributes();

        // Color.

        oInitialVertexAttributes.Color =
            GetInitialAttributeValue<Color>(ReservedMetadataKeys.PerColor);

        // Shape.

        oInitialVertexAttributes.Shape =
            GetInitialAttributeValue<VertexShape>(
                ReservedMetadataKeys.PerVertexShape);

        // Radius.

        Nullable<Single> fInitialRadius = GetInitialAttributeValue<Single>(
            ReservedMetadataKeys.PerVertexRadius);

        if (fInitialRadius.HasValue)
        {
            // The radius values stored in the vertices are in graph units.
            // They need to be converted to workbook units.

            VertexRadiusConverter oVertexRadiusConverter =
                new VertexRadiusConverter();

            oInitialVertexAttributes.Radius =
                oVertexRadiusConverter.GraphToWorkbook(fInitialRadius.Value);
        }
        else
        {
            oInitialVertexAttributes.Radius = null;
        }

        // Alpha.

        Nullable<Single> fInitialAlpha = GetInitialAttributeValue<Single>(
            ReservedMetadataKeys.PerAlpha);

        if (fInitialAlpha.HasValue)
        {
            AlphaConverter oAlphaConverter = new AlphaConverter();

            oInitialVertexAttributes.Alpha = oAlphaConverter.GraphToWorkbook(
                fInitialAlpha.Value);
        }
        else
        {
            oInitialVertexAttributes.Alpha = null;
        }

        // Visibility.

        oInitialVertexAttributes.Visibility = null;

        // Label position..

        oInitialVertexAttributes.LabelPosition =
            GetInitialAttributeValue<VertexLabelPosition>(
                ReservedMetadataKeys.PerVertexLabelPosition);

        // Locked.

        oInitialVertexAttributes.Locked =
            GetInitialAttributeValue<Boolean>(
                ReservedMetadataKeys.LockVertexLocation);

        // Marked.

        oInitialVertexAttributes.Marked =
            GetInitialAttributeValue<Boolean>(ReservedMetadataKeys.Marked);

        return (oInitialVertexAttributes);
    }

    //*************************************************************************
    //  Method: GetInitialAttributeValue()
    //
    /// <summary>
    /// Gets an attribute value that should be shown to the user when the
    /// dialog opens.
    /// </summary>
    ///
    /// <param name="sKey">
    /// Key under which the optional attribute value is stored in each vertex.
    /// </param>
    ///
    /// <returns>
    /// The initial value to use.
    /// </returns>
    //*************************************************************************

    protected Nullable<T>
    GetInitialAttributeValue<T>
    (
        String sKey
    )
    where T : struct
    {
        Debug.Assert(m_oNodeXLControl != null);
        // AssertValid();

        // If every vertex has the optional value and all values are the same,
        // that should be the initial value to show to the user.  Otherwise,
        // null should be shown.

        Nullable<T> oFirstValue = null;

        ICollection<IVertex> oSelectedVertices =
            m_oNodeXLControl.SelectedVertices;

        Int32 iSelectedVertices = oSelectedVertices.Count;
        Int32 i = 0;

        foreach (IVertex oVertex in oSelectedVertices)
        {
            Object oValueForThisVertex;

            // Does this vertex have a value?

            if ( !oVertex.TryGetValue(sKey, typeof(T),
                out oValueForThisVertex ) )
            {
                // No.

                return (null);
            }

            if (i == 0)
            {
                oFirstValue = (T)oValueForThisVertex;
            }
            else if ( !oFirstValue.Equals(oValueForThisVertex) )
            {
                return (null);
            }

            i++;
        }

        return (oFirstValue);
    }

    //*************************************************************************
    //  Method: lnkVisibility_LinkClicked()
    //
    /// <summary>
    /// Handles the Click event on the lnkVisibility LinkLabel.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    lnkVisibility_LinkClicked
    (
        object sender,
        LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        this.ShowInformation( String.Format(

            "Changing the Visibility to \"{0}\" causes the workbook to be read"
            + " into the graph again."
            ,
            ( new VertexVisibilityConverter() ).GraphToWorkbook(
                VertexWorksheetReader.Visibility.Skip)
            ) );
    }

    //*************************************************************************
    //  Method: lnkMarked_LinkClicked()
    //
    /// <summary>
    /// Handles the Click event on the lnkMarked LinkLabel.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    lnkMarked_LinkClicked
    (
        object sender,
        LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        this.ShowInformation(
            "Marking a vertex adds a \"Marked?\" column to the Vertices"
            + " worksheet and sets the selected cell to Yes.  You can sort on"
            + " the column to force marked vertices to the top of the"
            + " worksheet, or use the column in your own calculations."
            );
    }

    //*************************************************************************
    //  Method: btnOK_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnOK button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnOK_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if ( !DoDataExchange(true) )
        {
            return;
        }

        // If the caller is going to force the workbook to be reread, there
        // is no point in editing the vertices' metadata.

        if (!m_oEditedVertexAttributes.WorkbookMustBeReread)
        {
            this.UseWaitCursor = true;

            Boolean bApplyColor = m_oEditedVertexAttributes.Color.HasValue;
            Boolean bApplyShape = m_oEditedVertexAttributes.Shape.HasValue;
            Boolean bApplyRadius = m_oEditedVertexAttributes.Radius.HasValue;
            Boolean bApplyAlpha = m_oEditedVertexAttributes.Alpha.HasValue;

            Boolean bApplyVisibility =
                m_oEditedVertexAttributes.Visibility.HasValue;

            Boolean bApplyLabelPosition =
                m_oEditedVertexAttributes.LabelPosition.HasValue;

            Boolean bApplyLocked = m_oEditedVertexAttributes.Locked.HasValue;
            Boolean bApplyMarked = m_oEditedVertexAttributes.Marked.HasValue;

            VertexRadiusConverter oVertexRadiusConverter =
                new VertexRadiusConverter();

            AlphaConverter oAlphaConverter = new AlphaConverter();

            foreach (IVertex oVertex in m_oNodeXLControl.SelectedVertices)
            {
                if (bApplyColor)
                {
                    oVertex.SetValue(ReservedMetadataKeys.PerColor,
                        m_oEditedVertexAttributes.Color.Value);
                }

                if (bApplyShape)
                {
                    oVertex.SetValue(ReservedMetadataKeys.PerVertexShape,
                        m_oEditedVertexAttributes.Shape.Value);
                }

                if (bApplyRadius)
                {
                    oVertex.SetValue( ReservedMetadataKeys.PerVertexRadius,
                        oVertexRadiusConverter.WorkbookToGraph(
                            m_oEditedVertexAttributes.Radius.Value) );
                }

                if (bApplyAlpha)
                {
                    oVertex.SetValue( ReservedMetadataKeys.PerAlpha,
                        oAlphaConverter.WorkbookToGraph(
                            m_oEditedVertexAttributes.Alpha.Value) );
                }

                if (bApplyVisibility)
                {
                    Debug.Assert(m_oEditedVertexAttributes.Visibility.Value ==
                        VertexWorksheetReader.Visibility.Hide);

                    // Hide the vertex and its incident edges.

                    oVertex.SetValue(ReservedMetadataKeys.Visibility,
                        VisibilityKeyValue.Hidden);

                    foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
                    {
                        oIncidentEdge.SetValue(ReservedMetadataKeys.Visibility, 
                            VisibilityKeyValue.Hidden);
                    }
                }

                if (bApplyLabelPosition)
                {
                    oVertex.SetValue(
                        ReservedMetadataKeys.PerVertexLabelPosition,
                        m_oEditedVertexAttributes.LabelPosition.Value);
                }

                if (bApplyLocked)
                {
                    oVertex.SetValue(ReservedMetadataKeys.LockVertexLocation,
                        m_oEditedVertexAttributes.Locked.Value);
                }

                if (bApplyMarked)
                {
                    oVertex.SetValue(ReservedMetadataKeys.Marked,
                        m_oEditedVertexAttributes.Marked.Value);
                }
            }

            m_oNodeXLControl.DrawGraph();

            this.UseWaitCursor = false;
        }

        DialogResult = DialogResult.OK;
        this.Close();
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")] 

    public  override void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert(m_oVertexAttributesDialogUserSettings != null);
        Debug.Assert(m_oNodeXLControl != null);
        Debug.Assert(m_oEditedVertexAttributes != null);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Object stored in a ComboBox as the value that represents "not edited."
    /// (Unfortunately, you can't set ComboBox.SelectedValue to null, so null
    /// can't be used as the marker.

    protected String NotEditedMarker = String.Empty;

    /// Object stored in a ColorPicker as the value that represents "not
    /// edited."  This is arbitrary.  It is different from any color the user
    /// might actually select because it has an alpha value of 0.

    protected Color ColorNotEditedMarker = Color.FromArgb(0, 1, 1, 1);


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected VertexAttributesDialogUserSettings
        m_oVertexAttributesDialogUserSettings;

    /// NodeXLControl whose vertex attributes need to be edited.

    protected NodeXLControl m_oNodeXLControl;

    /// List of vertex attributes that were edited.

    protected EditedVertexAttributes m_oEditedVertexAttributes;
}


//*****************************************************************************
//  Class: VertexAttributesDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="VertexAttributesDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("VertexAttributesDialog5") ]

public class VertexAttributesDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: VertexAttributesDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VertexAttributesDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public VertexAttributesDialogUserSettings
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
