
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.NetMap.Core;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: ReadWorkbookContext
//
/// <summary>
/// Provides access to objects needed for converting an Excel workbook to a
/// NetMap graph.
/// </summary>
///
/// <remarks>
/// If the X, Y, and Locked columns of the vertex table should be read, set
/// <see cref="IgnoreVertexLocations" /> to false and set <see
/// cref="GraphRectangle" /> to the rectangle the graph is being drawn
/// within.  Otherwise, leave <see cref="IgnoreVertexLocations" /> at its
/// default value of true.
///
/// <para>
/// Set <see cref="FillIDColumns" /> to true to store an ID in each row of the
/// edge and vertex tables.
/// </para>
///
/// <para>
/// Set <see cref="PopulateVertexWorksheet" /> to true to populate the vertex
/// table with unique vertex names from the edge worksheet before the workbook
/// is read.
/// </para>
///
/// <para>
/// Set <see cref="AutoFillWorkbook" /> to true to run the AutoFill feature on
/// the workbook before the workbook is read.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class ReadWorkbookContext : Object
{
    //*************************************************************************
    //  Constructor: ReadWorkbookContext()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ReadWorkbookContext" />
	/// class.
    /// </summary>
    //*************************************************************************

    public ReadWorkbookContext()
    {
		m_bIgnoreVertexLocations = true;
		m_bFillIDColumns = false;
		m_bPopulateVertexWorksheet = false;
		m_bAutoFillWorkbook = false;
        m_oGraphRectangle = Rectangle.FromLTRB(0, 0, 100, 100);
		m_oColorConverter2 = new ColorConverter2();
		m_oEdgeWidthConverter = new EdgeWidthConverter();
		m_oVertexRadiusConverter = new VertexRadiusConverter();

		m_oVertexLocationConverter =
			new VertexLocationConverter(m_oGraphRectangle);

		m_oVertexNameDictionary = new Dictionary<String, IVertex>();
		m_oEdgeIDDictionary = new Dictionary<Int32, IIdentityProvider>();
		m_oVertexIDDictionary = new Dictionary<Int32, IIdentityProvider>();
		m_oImageIDDictionary = new Dictionary<String, Image>();
		m_bToolTipsUsed = false;

		AssertValid();
    }

    //*************************************************************************
    //  Property: IgnoreVertexLocations
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the X, Y, and Locked columns on
	/// the vertex worksheet should be ignored.
    /// </summary>
    ///
    /// <value>
	/// true to ignore the X, Y, and Locked columns on the vertex worksheet.
	/// The default is true.
    /// </value>
	///
	/// <remarks>
	/// If you set this to false, you must also set <see
	/// cref="GraphRectangle" /> to the rectangle the graph is being drawn
	/// within.
	/// </remarks>
    //*************************************************************************

    public Boolean
    IgnoreVertexLocations
    {
        get
        {
            AssertValid();

            return (m_bIgnoreVertexLocations);
        }

		set
		{
            m_bIgnoreVertexLocations = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: FillIDColumns
    //
    /// <summary>
    /// Gets or sets a flag indicating whether an ID should be stored in each
	/// row of the edge and vertex tables.
    /// </summary>
    ///
    /// <value>
	/// true to store an ID in each row of the edge and vertex tables.  The
	/// default is false.
    /// </value>
	///
	/// <remarks>
	/// The ID is used to link rows in the edge and vertex tables to edges and
	/// vertices in the .Netmap graph.
	/// </remarks>
    //*************************************************************************

    public Boolean
    FillIDColumns
    {
        get
        {
            AssertValid();

            return (m_bFillIDColumns);
        }

		set
		{
            m_bFillIDColumns = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: PopulateVertexWorksheet
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the vertex table should be
	/// populated with unique vertex names from the edge worksheet before the
	/// workbook is read.
    /// </summary>
    ///
    /// <value>
	/// true to populate the vertex table with unique vertex names.  The
	/// default is false.
    /// </value>
    //*************************************************************************

    public Boolean
    PopulateVertexWorksheet
    {
        get
        {
            AssertValid();

            return (m_bPopulateVertexWorksheet);
        }

		set
		{
            m_bPopulateVertexWorksheet = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: AutoFillWorkbook
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the AutoFill feature should be
	/// run on the workbook before the workbook is read.
    /// </summary>
    ///
    /// <value>
	/// true to run the AutoFill feature.  The default is false.
    /// </value>
    //*************************************************************************

    public Boolean
    AutoFillWorkbook
    {
        get
        {
            AssertValid();

            return (m_bAutoFillWorkbook);
        }

		set
		{
            m_bAutoFillWorkbook = value;

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: GraphRectangle
    //
    /// <summary>
    /// Gets or sets the <see cref="System.Drawing.Rectangle" /> the graph is
	/// being drawn within.
    /// </summary>
    ///
    /// <value>
    /// The <see cref="System.Drawing.Rectangle" /> the graph is being drawn
	/// within.  The default is a 100x100 square.
    /// </value>
    //*************************************************************************

    public Rectangle
    GraphRectangle
    {
        get
        {
            AssertValid();

            return (m_oGraphRectangle);
        }

		set
		{
            m_oGraphRectangle = value;

			m_oVertexLocationConverter =
				new VertexLocationConverter(m_oGraphRectangle);

			AssertValid();
		}
    }

    //*************************************************************************
    //  Property: ColorConverter2
    //
    /// <summary>
	/// Gets an object for converting strings to colors.
    /// </summary>
    ///
    /// <value>
    /// A <see cref="ColorConverter2" /> object.
    /// </value>
    //*************************************************************************

    public ColorConverter2
    ColorConverter2
    {
        get
        {
            AssertValid();

            return (m_oColorConverter2);
        }
    }

    //*************************************************************************
    //  Property: EdgeWidthConverter
    //
    /// <summary>
	/// Gets an object that converts an edge width between values used in the
	/// Excel workbook and values used in the NetMap graph.
    /// </summary>
    ///
    /// <value>
    /// An <see cref="EdgeWidthConverter" /> object.
    /// </value>
    //*************************************************************************

    public EdgeWidthConverter
    EdgeWidthConverter
    {
        get
        {
            AssertValid();

            return (m_oEdgeWidthConverter);
        }
    }

    //*************************************************************************
    //  Property: VertexRadiusConverter
    //
    /// <summary>
	/// Gets an object that converts a vertex radius between values used in the
	/// Excel workbook and values used in the NetMap graph.
    /// </summary>
    ///
    /// <value>
    /// A <see cref="VertexRadiusConverter" /> object.
    /// </value>
    //*************************************************************************

    public VertexRadiusConverter
    VertexRadiusConverter
    {
        get
        {
            AssertValid();

            return (m_oVertexRadiusConverter);
        }
    }

    //*************************************************************************
    //  Property: VertexLocationConverter
    //
    /// <summary>
	/// Gets an object that converts a vertex location between coordinates used
	/// in the Excel workbook and coordinates used in the NetMap graph.
    /// </summary>
    ///
    /// <value>
    /// A <see cref="VertexLocationConverter" /> object.
    /// </value>
	///
	/// <remarks>
	/// You must set the <see cref="GraphRectangle" /> property before using
	/// this property.  Otherwise, the vertex locations will be scaled to the
	/// default graph rectangle, which will result in incorrect vertex
	/// locations in the graph.
	/// </remarks>
    //*************************************************************************

    public VertexLocationConverter
    VertexLocationConverter
    {
        get
        {
            AssertValid();

            return (m_oVertexLocationConverter);
        }
    }

    //*************************************************************************
    //  Property: VertexNameDictionary
    //
    /// <summary>
    /// Gets a dictionary that maps vertex names from the edge and vertex
	/// worksheets to vertex objects in the graph.
    /// </summary>
    ///
    /// <value>
	/// Vertex dictionary.  The key is the vertex name from the edge or vertex
	/// worksheet and the value is the IVertex object.
    /// </value>
    //*************************************************************************

    public Dictionary<String, IVertex>
    VertexNameDictionary
    {
        get
        {
            AssertValid();

            return (m_oVertexNameDictionary);
        }
    }

    //*************************************************************************
    //  Property: EdgeIDDictionary
    //
    /// <summary>
    /// Gets a dictionary that maps edge IDs from the edge worksheet to edge
	/// objects in the graph.
    /// </summary>
    ///
    /// <value>
	/// Edge dictionary.  The key is the edge ID from the edge worksheet and
	/// the value is the IEdge object.
    /// </value>
    //*************************************************************************

    public Dictionary<Int32, IIdentityProvider>
    EdgeIDDictionary
    {
        get
        {
            AssertValid();

            return (m_oEdgeIDDictionary);
        }
    }

    //*************************************************************************
    //  Property: VertexIDDictionary
    //
    /// <summary>
    /// Gets a dictionary that maps vertex IDs from the vertex worksheet to
	/// vertex objects in the graph.
    /// </summary>
    ///
    /// <value>
	/// Vertex dictionary.  The key is the vertex ID from the vertex worksheet
	/// and the value is the IVertex object.
    /// </value>
    //*************************************************************************

    public Dictionary<Int32, IIdentityProvider>
    VertexIDDictionary
    {
        get
        {
            AssertValid();

            return (m_oVertexIDDictionary);
        }
    }

    //*************************************************************************
    //  Property: ImageIDDictionary
    //
    /// <summary>
    /// Gets a dictionary that maps vertex IDs from the vertex worksheet to
	/// vertex objects in the graph.
    /// </summary>
    ///
    /// <value>
	/// Image dictionary.  The key is a unique image identifier specified in
	/// the image worksheet and the value is the corresponding
	/// System.Drawing.Image.
    /// </value>
    //*************************************************************************

    public Dictionary<String, Image>
    ImageIDDictionary
    {
        get
        {
            AssertValid();

            return (m_oImageIDDictionary);
        }
    }

    //*************************************************************************
    //  Property: ToolTipsUsed
    //
    /// <summary>
    /// Gets or sets a flag indicating whether a tooltip was set on at least
	/// one vertex.
    /// </summary>
    ///
    /// <value>
    /// true if a tooltip was set on at least one vertex.
    /// </value>
    //*************************************************************************

    public Boolean
    ToolTipsUsed
    {
        get
        {
            AssertValid();

            return (m_bToolTipsUsed);
        }

		set
		{
			m_bToolTipsUsed = value;

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

    [Conditional("DEBUG")]

    public  void
    AssertValid()
    {
		// m_bIgnoreVertexLocations
		// m_bFillIDColumns
		// m_bPopulateVertexWorksheet
		// m_bAutoFillWorkbook
		// m_oGraphRectangle
		Debug.Assert(m_oColorConverter2 != null);
		Debug.Assert(m_oEdgeWidthConverter != null);
		Debug.Assert(m_oVertexRadiusConverter != null);
		Debug.Assert(m_oVertexLocationConverter != null);
		Debug.Assert(m_oVertexNameDictionary != null);
		Debug.Assert(m_oEdgeIDDictionary != null);
		Debug.Assert(m_oVertexIDDictionary != null);
		Debug.Assert(m_oImageIDDictionary != null);
		// m_bToolTipsUsed
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// true to ignore the X, Y, and Locked columns on the vertex worksheet.

	protected Boolean m_bIgnoreVertexLocations;

	/// true to store an ID in each row of the edge and vertex tables.

	protected Boolean m_bFillIDColumns;

	/// true to populate the vertex table with unique vertex names.

	protected Boolean m_bPopulateVertexWorksheet;

	/// true to run the AutoFill feature on the workbook.

	protected Boolean m_bAutoFillWorkbook;

    /// The rectangle the graph is being drawn within.

	protected Rectangle m_oGraphRectangle;

	/// Object for converting strings to colors.

	protected ColorConverter2 m_oColorConverter2;

	/// Object that converts an edge width between values used in the Excel
	/// workbook and values used in the NetMap graph.

	protected EdgeWidthConverter m_oEdgeWidthConverter;

	/// Object that converts a vertex radius between values used in the Excel
	/// workbook and values used in the NetMap graph.

	protected VertexRadiusConverter m_oVertexRadiusConverter;

	/// Object that converts a vertex location between coordinates used in the
	/// Excel workbook and coordinates used in the NetMap graph.

	protected VertexLocationConverter m_oVertexLocationConverter;

	/// Vertex dictionary.  The key is the vertex name and the value is the
	/// vertex.

	protected Dictionary<String, IVertex> m_oVertexNameDictionary;

	/// Edge dictionary.  The key is the edge ID from the edge worksheet and
	/// the value is the IEdge object.

	protected Dictionary<Int32, IIdentityProvider> m_oEdgeIDDictionary;

	/// Vertex dictionary.  The key is the vertex ID from the vertex worksheet
	/// and the value is the IVertex object.

	protected Dictionary<Int32, IIdentityProvider> m_oVertexIDDictionary;

	/// Image dictionary.  The key is a unique image identifier specified in
	/// the image worksheet and the value is the corresponding
	/// System.Drawing.Image.

	protected Dictionary<String, Image> m_oImageIDDictionary; 

	/// true if a tooltip was set on at least one vertex.

	protected Boolean m_bToolTipsUsed;
}

}
