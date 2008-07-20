
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.GraphicsLib
{
//*****************************************************************************
//	Class: ColorGradientMapper
//
/// <summary>
///	Maps a range of color metric values to colors within a gradient.
/// </summary>
///
///	<remarks>
///	Call the Initialize() method to specify the color metric range, the color
///	gradient to map the range to, and the number of discrete colors to split
///	the gradient into.  You can then call ColorMetricToColor() to map a color
///	metric within the range to one of the discrete colors within the gradient,
/// or ColorMetricToBrush() to get a corresponding brush.
///
/// <para>
///	Call Dispose() when you are done using the object.
/// </para>
///
///	</remarks>
//*****************************************************************************

public class ColorGradientMapper
{
    //*************************************************************************
    //  Constructor: ColorGradientMapper()
    //
    /// <summary>
    /// Initializes a new instance of the ColorGradientMapper class.
    /// </summary>
    //*************************************************************************

	protected internal ColorGradientMapper()
	{
		m_aoDiscreteColors = null;
		m_aoDiscreteBrushes = null;
		m_iDiscreteColorCount = 0;
		m_dMinColorMetric = 0;
		m_dMaxColorMetric = 0;
		m_dColorMetricsPerDivision = 0;
	}

	//*************************************************************************
	//	Method: Initialize()
	//
	/// <summary>
	/// Initializes the object.
	/// </summary>
	///
	/// <param name="dMinColorMetric">
	/// Minimum color metric to map.
	/// </param>
	///
	/// <param name="dMaxColorMetric">
	/// Maximum color metric to map.
	/// </param>
	///
	/// <param name="oMinColor">
	/// Color to map to dMinColorMetric.
	/// </param>
	///
	/// <param name="oMaxColor">
	/// Color to map to dMaxColorMetric.
	/// </param>
	///
	/// <param name="iDiscreteColorCount">
	/// Number of discrete colors to split the color gradient into.  Must be
	///	between 2 and 256.
	/// </param>
	///
	/// <param name="bCreateBrushes">
	/// true to create an array of brushes so that ColorMetricToBrush() can be
	///	called.
	/// </param>
	///
	/// <remarks>
	///	This must be called before any other methods or properties are used.
	/// </remarks>
	//*************************************************************************

	public void
	Initialize
	(
		Double dMinColorMetric,
		Double dMaxColorMetric,
		Color oMinColor,
		Color oMaxColor,
		Int32 iDiscreteColorCount,
		Boolean bCreateBrushes
	)
	{

		const String sMethodName = "ColorGradientMapper.Initialize";

		if (dMaxColorMetric <= dMinColorMetric)
		{
			throw new ArgumentOutOfRangeException("dMaxColorMetric",
				dMaxColorMetric,
				sMethodName + ": dMaxColorMetric must be > dMinColorMetric.");
		}

		if (iDiscreteColorCount < 2 || iDiscreteColorCount > 256)
		{
			throw new ArgumentOutOfRangeException("iDiscreteColorCount",
				iDiscreteColorCount,
				sMethodName +
					": iDiscreteColorCount must be between 2 and 256.");
		}

		// Save the parameters.

		m_dMinColorMetric = dMinColorMetric;
		m_dMaxColorMetric = dMaxColorMetric;
		m_iDiscreteColorCount = iDiscreteColorCount;

		// Compute a value used by ColorMetricToArrayIndex().

		m_dColorMetricsPerDivision = (m_dMaxColorMetric - m_dMinColorMetric) /
			(Double)m_iDiscreteColorCount;

		// Create an array of discrete colors.

		m_aoDiscreteColors = CreateDiscreteColors(oMinColor, oMaxColor,
			iDiscreteColorCount);

		if (bCreateBrushes)
		{
			// Create an array of brushes corresponding to the discrete colors.

			m_aoDiscreteBrushes = CreateDiscreteBrushes(m_aoDiscreteColors);
		}
	}

	//*************************************************************************
	//	Method: ColorMetricToColor()
	//
	/// <summary>
	///	Maps a color metric to a color.
	/// </summary>
	///
	/// <param name="dColorMetric">
	/// Color metric to map.
	/// </param>
	///
	///	<returns>
	///	Color within the gradient specified in the Initialize() method.
	///	</returns>
	///
	/// <remarks>
	///	This method maps a color metric to a discrete color within the
	///	gradient specified in the Initialize() call.
	///
	/// <para>
	/// If dColorMetric is less than the dMinColorMetric value specified in
	///	Initialize(), the minimum color is returned.  If dColorMetric is
	///	greater than the dMaxColorMetric value, the maximum color is returned.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public Color
	ColorMetricToColor
	(
		Double dColorMetric
	)
	{
		if (m_iDiscreteColorCount == 0)
		{
			throw new InvalidOperationException(
				"ColorGradientMapper.ColorMetricToColor: Must call"
				+ " Initialize() first.");
		}

		Int32 iIndex = ColorMetricToArrayIndex(dColorMetric);

		return ( m_aoDiscreteColors[iIndex] );
	}

	//*************************************************************************
	//	Method: ColorMetricToBrush()
	//
	/// <summary>
	///	Maps a color metric to a brush.
	/// </summary>
	///
	/// <param name="dColorMetric">
	/// Color metric to map.
	/// </param>
	///
	///	<returns>
	///	Brush with a color within the gradient specified in the Initialize()
	///	method.
	///	</returns>
	///
	/// <remarks>
	///	This method maps a color metric to a brush with a discrete color within
	///	the gradient specified in the Initialize() call.
	///
	/// <para>
	/// The returned brush is owned by the ColorGradientMapper object.  Do
	/// not call the brush's Dispose() method.
	/// </para>
	///
	/// <para>
	/// If dColorMetric is less than the dMinColorMetric value specified in
	///	Initialize(), the minimum color brush is returned.  If dColorMetric is
	///	greater than the dMaxColorMetric value, the maximum color brush is
	///	returned.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public Brush
	ColorMetricToBrush
	(
		Double dColorMetric
	)
	{
		if (m_iDiscreteColorCount == 0)
		{
			throw new InvalidOperationException(
				"ColorGradientMapper.ColorMetricToBrush: Must call"
				+ " Initialize() first.");
		}

		if (m_aoDiscreteBrushes == null)
		{
			throw new InvalidOperationException(
				"ColorGradientMapper.ColorMetricToBrush: Must specify"
				+ " bCreateBrushes=true in Initialize() call.");
		}

		Int32 iIndex = ColorMetricToArrayIndex(dColorMetric);

		return ( m_aoDiscreteBrushes[iIndex] );
	}

	//*************************************************************************
	//	Method: Dispose()
	//
	/// <summary>
	/// Releases all resources used by this object.
	/// </summary>
	///
	/// <remarks>
	///	Call this when you are done with the object.
	/// </remarks>
	//*************************************************************************

	public void
	Dispose()
	{
		if (m_aoDiscreteBrushes != null)
			foreach (Brush oBrush in m_aoDiscreteBrushes)
				oBrush.Dispose();
	}

	//*************************************************************************
	//	Method: CreateDiscreteColors()
	//
	/// <summary>
	///	Creates an array of discrete colors.
	/// </summary>
	///
	/// <param name="oMinColor">
	/// Color to map to dMinColorMetric.
	/// </param>
	///
	/// <param name="oMaxColor">
	/// Color to map to dMaxColorMetric.
	/// </param>
	///
	/// <param name="iDiscreteColorCount">
	/// Number of discrete colors to split the color gradient into.  Must be
	///	between 2 and 256.
	/// </param>
	///
	/// <returns>
	///	Array of discrete colors.
	/// </returns>
	//*************************************************************************

	protected Color []
	CreateDiscreteColors
	(
		Color oMinColor,
		Color oMaxColor,
		Int32 iDiscreteColorCount
	)
	{
		Debug.Assert(iDiscreteColorCount > 1);

		// Create an empty array.

		Color [] aoDiscreteColors = new Color[iDiscreteColorCount];

		// Create a bitmap one pixel wide and iDiscreteColorCount pixels high.

		Bitmap oBitmap = new Bitmap(1, iDiscreteColorCount);

		// Get a Graphics object for drawing on the bitmap.

		Graphics oBitmapGraphics = Graphics.FromImage(oBitmap);

		// Create a gradient brush.  The minus one in the rectangle calculation
		// is to work around an oddity with gradient brushes.  If you create
		// a brush 20 pixels high running from black to blue and use it to fill
		// a 20-pixel bitmap, for example, the 20th pixel ends up slightly less
		// than blue instead of pure blue.  Is this a bug or by design?  In
		// either case, the following code compensates by creating a 19-pixel
		// brush, for example, and uses it to fill the 20-pixel bitmap.  The
		// 19th pixel ends up pure blue.  The 20th pixel ends up an unwanted
		// value and is ignored.

		Rectangle oGradientRectangle = Rectangle.FromLTRB(0, 0,
			1, iDiscreteColorCount - 1);

		LinearGradientBrush oLinearGradientBrush = new LinearGradientBrush(
			oGradientRectangle, oMinColor, oMaxColor,
			LinearGradientMode.Vertical);

		// Fill the bitmap with the brush.

		oBitmapGraphics.FillRectangle( oLinearGradientBrush,
			new Rectangle(Point.Empty, oBitmap.Size) );

		oLinearGradientBrush.Dispose();

		// Read the discrete colors off of the bitmap, up until the last pixel.
		// (See the above comments.)

		Int32 i;

		for (i = 0; i < iDiscreteColorCount - 1; i++)
			aoDiscreteColors[i] = oBitmap.GetPixel(0, i);

		// The last discrete color must be oMaxColor.

		aoDiscreteColors[i] = oMaxColor;

		oBitmap.Dispose();

		return (aoDiscreteColors);
	}

	//*************************************************************************
	//	Method: CreateDiscreteBrushes()
	//
	/// <summary>
	///	Creates an array of brushes with the specified colors.
	/// </summary>
	///
	/// <param name="aoDiscreteColors">
	/// Array of colors.  One brush is created for each color.
	/// </param>
	///
	/// <returns>
	///	Array of brushes.
	/// </returns>
	//*************************************************************************

	protected Brush []
	CreateDiscreteBrushes
	(
		Color [] aoDiscreteColors
	)
	{
		// Create an empty array.

		Int32 iDiscreteColors = aoDiscreteColors.Length;
		Brush [] aoDiscreteBrushes = new Brush[iDiscreteColors];

		// Create a brush for each color.

		for (Int32 i = 0; i < iDiscreteColors; i++)
			aoDiscreteBrushes[i] = new SolidBrush( aoDiscreteColors[i] );

		return (aoDiscreteBrushes);
	}

	//*************************************************************************
	//	Method: ColorMetricToArrayIndex()
	//
	/// <summary>
	///	Maps a color metric to an index into the m_aoDiscreteColors and
	///	m_aoDiscreteBrushes arrays.
	/// </summary>
	///
	/// <param name="dColorMetric">
	/// Color metric to map.
	/// </param>
	///
	///	<returns>
	///	Index into the m_aoDiscreteColors and m_aoDiscreteBrushes arrays.
	///	</returns>
	//*************************************************************************

	protected Int32
	ColorMetricToArrayIndex
	(
		Double dColorMetric
	)
	{
		Debug.Assert(m_iDiscreteColorCount != 0);

		Int32 iIndex;

		if (dColorMetric <= m_dMinColorMetric)
		{
			// Color metric values outside the specified range get "pinned" at
			// the minimum or maximum array entries.

			iIndex = 0;
		}
		else if (dColorMetric >= m_dMaxColorMetric)
		{
			iIndex = m_iDiscreteColorCount - 1;
		}
		else
		{
			iIndex = (Int32)( (dColorMetric - m_dMinColorMetric) /
				m_dColorMetricsPerDivision );
		}

		Debug.Assert(iIndex >= 0);
		Debug.Assert(iIndex < m_iDiscreteColorCount);

		return (iIndex);
	}

	//*************************************************************************
	//	Protected member data
	//*************************************************************************

	/// Array of discrete colors that the color gradient has been split into.

	protected Color [] m_aoDiscreteColors;

	/// Array of preallocated brushes corresponding to the discrete colors in
	/// m_aoDiscreteColors.  Used only if the bCreateBrushes argument to
	/// Initialize() is true.

	protected Brush [] m_aoDiscreteBrushes;

	/// Number of discrete colors in the arrays.

	protected Int32 m_iDiscreteColorCount;

	/// Color metric that maps to the first element in m_aoDiscreteColors;

	protected Double m_dMinColorMetric;

	/// Color metric that maps to the last element in m_aoDiscreteColors;

	protected Double m_dMaxColorMetric;

	/// Value used by ColorMetricToColor(), saved in member data so it doesn't
	/// have to be computed with every call.

	protected Double m_dColorMetricsPerDivision;
}

}
