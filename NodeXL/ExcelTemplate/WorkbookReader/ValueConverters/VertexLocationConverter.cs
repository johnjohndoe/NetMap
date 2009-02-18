
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Diagnostics;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexLocationConverter
//
/// <summary>
/// Class that converts a vertex location between coordinates used in the Excel
/// workbook and coordinates used in the NodeXL graph.
/// </summary>
//*****************************************************************************

public class VertexLocationConverter : Object
{
    //*************************************************************************
    //  Constructor: VertexLocationConverter()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VertexLocationConverter" /> class.
    /// </summary>
    ///
    /// <param name="graphRectangle">
    /// Rectangle the graph was drawn within.
    /// </param>
    //*************************************************************************

    public VertexLocationConverter
    (
        Rectangle graphRectangle
    )
    {
        m_oGraphRectangle = graphRectangle;

        AssertValid();
    }

    //*************************************************************************
    //  Method: WorkbookToGraph()
    //
    /// <summary>
    /// Converts a vertex location from Excel workbook coordinates to NodeXL
    /// graph coordinates.
    /// </summary>
    ///
    /// <param name="workbookX">
    /// Vertex x-coordinate read from the Excel workbook.  If less than <see
    /// cref="MinimumXYWorkbook" />, the left edge of the graph rectangle is
    /// stored in the returned PointF.  If greater than <see
    /// cref="MaximumXYWorkbook" />, the right edge of the graph rectangle is
    /// stored in the returned PointF.  
    /// </param>
    ///
    /// <param name="workbookY">
    /// Vertex y-coordinate read from the Excel workbook.  If less than <see
    /// cref="MinimumXYWorkbook" />, the top edge of the graph rectangle is
    /// stored in the returned PointF.  If greater than <see
    /// cref="MaximumXYWorkbook" />, the bottom edge of the graph rectangle is
    /// stored in the returned PointF.  
    /// </param>
    ///
    /// <returns>
    /// A PointF suitable for use as an IVertex.Location value in a NodeXL
    /// graph.
    /// </returns>
    //*************************************************************************

    public PointF
    WorkbookToGraph
    (
        Single workbookX,
        Single workbookY 
    )
    {
        AssertValid();

        Single fGraphX =
            m_oGraphRectangle.Left + (workbookX - MinimumXYWorkbook) *
            m_oGraphRectangle.Width / WorkbookRange
            ;

        fGraphX = Math.Max(fGraphX, m_oGraphRectangle.Left);
        fGraphX = Math.Min(fGraphX, m_oGraphRectangle.Right);

        Single fGraphY =
            m_oGraphRectangle.Top + (workbookY - MinimumXYWorkbook) *
            m_oGraphRectangle.Height / WorkbookRange
            ;

        fGraphY = Math.Max(fGraphY, m_oGraphRectangle.Top);
        fGraphY = Math.Min(fGraphY, m_oGraphRectangle.Bottom);

        return ( new PointF(fGraphX, fGraphY) );
    }

    //*************************************************************************
    //  Method: GraphToWorkbook()
    //
    /// <summary>
    /// Converts a vertex location from NodeXL graph coordinates to Excel
    /// workbook coordinates.
    /// </summary>
    ///
    /// <param name="graphLocation">
    /// IVertex.Location value from a NodeXL graph.
    /// </param>
    ///
    /// <param name="workbookX">
    /// Where a vertex x-coordinate suitable for use in an Excel workbook gets
    /// stored.
    /// </param>
    ///
    /// <param name="workbookY">
    /// Where a vertex y-coordinate suitable for use in an Excel workbook gets
    /// stored.
    /// </param>
    //*************************************************************************

    public void
    GraphToWorkbook
    (
        PointF graphLocation,
        out Single workbookX,
        out Single workbookY
    )
    {
        AssertValid();

        if (m_oGraphRectangle.Width > 0)
        {
            workbookX = MinimumXYWorkbook +
                (graphLocation.X - m_oGraphRectangle.Left)
                * WorkbookRange / m_oGraphRectangle.Width
                ;
        }
        else
        {
            workbookX = MinimumXYWorkbook;
        }

        workbookX = Math.Max(workbookX, MinimumXYWorkbook);
        workbookX = Math.Min(workbookX, MaximumXYWorkbook);

        if (m_oGraphRectangle.Height > 0)
        {
            workbookY = MinimumXYWorkbook +
                (graphLocation.Y - m_oGraphRectangle.Top)
                * WorkbookRange / m_oGraphRectangle.Height
                ;
        }
        else
        {
            workbookY = MinimumXYWorkbook;
        }

        workbookY = Math.Max(workbookY, MinimumXYWorkbook);
        workbookY = Math.Min(workbookY, MaximumXYWorkbook);
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
        // m_oGraphRectangle
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Minimum value that can be specified in the workbook for vertex X and Y
    /// values.

    public static readonly Single MinimumXYWorkbook = 0F;

    /// Maximum value that can be specified in the workbook for vertex X and Y
    /// values.

    public static readonly Single MaximumXYWorkbook = 9999F;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The rectangle the graph is being drawn within.

    protected RectangleF m_oGraphRectangle;

    /// Range of values that can be specified in the workbook for vertex X and
    /// Y values.

    protected Single WorkbookRange = MaximumXYWorkbook - MinimumXYWorkbook;
}

}
