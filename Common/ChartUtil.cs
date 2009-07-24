
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.ChartLib
{
//*****************************************************************************
//  Class: ChartUtil
//
/// <summary>
/// Static utility methods that can be used in the implementation of charting
/// components.
/// </summary>
//*****************************************************************************

internal class ChartUtil : Object
{
    //*************************************************************************
    //  Constructor: ChartUtil()
    //
    /// <summary>
    /// Do not use this contructor.
    /// </summary>
    ///
    /// <remarks>
    /// All methods on this class are static, so there is no need to create a
    /// ChartUtil object.
    /// </remarks>
    //*************************************************************************

    private ChartUtil()
    {
    }

    //*************************************************************************
    //  Method: GetAxisGridlineValues()
    //
    /// <overloads>
    /// Gets an array of values for drawing axis gridlines on a linear axis.
    /// </overloads>
    ///
    /// <summary>
    /// Gets an array of values for drawing axis gridlines on a linear axis.
    /// </summary>
    ///
    /// <param name="fLimit1">
    /// Value that will be displayed at the left end of the axis.
    /// </param>
    ///
    /// <param name="fLimit2">
    /// Value that will be displayed at the right end of the axis.  This does
    /// not have to be greater than <paramref name="fLimit1" />.
    /// </param>
    ///
    /// <returns>
    /// Array of values to draw gridlines for.  Sample arrays:
    /// {0, 20, 40, 60, 80, 100}, {-3.5, -3.0, -2.5},
    /// {0.8, 0.9, 1.0, 1.1, 1.2}.  The values are always in increasing order,
    /// even if <paramref name="fLimit2" /> is less than <paramref
    /// name="fLimit1" />.
    /// </returns>
    ///
    /// <remarks>
    /// See the other overload for more details on how this method works.
    /// </remarks>
    //*************************************************************************

    public static Single []
    GetAxisGridlineValues
    (
        Single fLimit1,
        Single fLimit2
    )
    {
        Single [] afGridlineValues;
        Int32 iDecimalPlacesToShow;

        GetAxisGridlineValues(fLimit1, fLimit2,
            out afGridlineValues, out iDecimalPlacesToShow);

        return (afGridlineValues);
    }

    //*************************************************************************
    //  Method: GetAxisGridlineValues()
    //
    /// <summary>
    /// Gets an array of Single values for drawing axis gridlines on a linear
    /// axis, along with the number of decimal places to use when formatting
    /// the values.
    /// </summary>
    ///
    /// <param name="fLimit1">
    /// Value that will be displayed at the left end of the axis.
    /// </param>
    ///
    /// <param name="fLimit2">
    /// Value that will be displayed at the right end of the axis.  This does
    /// not have to be greater than <paramref name="fLimit1" />.
    /// </param>
    ///
    /// <param name="afGridlineValues">
    /// Where the array of values to draw gridlines for gets stored.  Sample
    /// arrays: {0, 20, 40, 60, 80, 100}, {-3.5, -3.0, -2.5},
    /// {0.8, 0.9, 1.0, 1.1, 1.2}.  The values are always in increasing order,
    /// even if <paramref name="fLimit2" /> is less than <paramref
    /// name="fLimit1" />.
    /// </param>
    ///
    /// <param name="iDecimalPlacesToShow">
    /// Where the number of decimal places to use when formatting the gridline
    /// values gets stored.
    /// </param>
    ///
    /// <remarks>
    /// This overload, which gets an array of Single values, is for
    /// compatibility with older projects only.  Newer projects should probably
    /// use the overload that returns an array of Double values.
    ///
    /// <para>
    /// See the other overload for more details on how this method works.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static void
    GetAxisGridlineValues
    (
        Single fLimit1,
        Single fLimit2,
        out Single [] afGridlineValues,
        out Int32 iDecimalPlacesToShow
    )
    {
        Double [] adGridlineValues;

        GetAxisGridlineValues(fLimit1, fLimit2, out adGridlineValues,
            out iDecimalPlacesToShow);

        Int32 iGridlineValues = adGridlineValues.Length;

        afGridlineValues = new Single[iGridlineValues];

        for (Int32 i = 0; i < iGridlineValues; i++)
        {
            afGridlineValues[i] = (Single)adGridlineValues[i];
        }
    }

    //*************************************************************************
    //  Method: GetAxisGridlineValues()
    //
    /// <summary>
    /// Gets an array of Double values for drawing axis gridlines on a linear
    /// axis, along with the number of decimal places to use when formatting
    /// the values.
    /// </summary>
    ///
    /// <param name="dLimit1">
    /// Value that will be displayed at the left end of the axis.
    /// </param>
    ///
    /// <param name="dLimit2">
    /// Value that will be displayed at the right end of the axis.  This does
    /// not have to be greater than <paramref name="dLimit1" />.
    /// </param>
    ///
    /// <param name="adGridlineValues">
    /// Where the array of values to draw gridlines for gets stored.  Sample
    /// arrays: {0, 20, 40, 60, 80, 100}, {-3.5, -3.0, -2.5},
    /// {0.8, 0.9, 1.0, 1.1, 1.2}.  The values are always in increasing order,
    /// even if <paramref name="dLimit2" /> is less than <paramref
    /// name="dLimit1" />.
    /// </param>
    ///
    /// <param name="iDecimalPlacesToShow">
    /// Where the number of decimal places to use when formatting the gridline
    /// values gets stored.
    /// </param>
    ///
    /// <remarks>
    /// Given a linear chart axis that will display a specified range of
    /// values, this method returns an array of values for which gridlines
    /// should be drawn.  The values are computed in such a way that 1) the
    /// minimum gridline value is less than or equal to the lesser of <paramref
    /// name="dLimit1" /> and <paramref name="dLimit2" />; 2) the maximum
    /// gridline value is greater than or equal to the larger of <paramref
    /// name="dLimit2" /> and <paramref name="dLimit1" />; 3) the number of
    /// gridline values is as close to 6 as possible, which makes the number of
    /// axis intervals as close to 5 as possible; and 4) the difference between
    /// gridlines values is n*2, n*5, or n*10, where n is a power of 10.
    /// </remarks>
    //*************************************************************************

    public static void
    GetAxisGridlineValues
    (
        Double dLimit1,
        Double dLimit2,
        out Double [] adGridlineValues,
        out Int32 iDecimalPlacesToShow
    )
    {
        adGridlineValues = null;
        iDecimalPlacesToShow = 0;

        Double dMinimum, dMaximum;

        if (dLimit1 < dLimit2)
        {
            dMinimum = dLimit1;
            dMaximum = dLimit2;
        }
        else
        {
            dMinimum = dLimit2;
            dMaximum = dLimit1;
        }

        // Handle the one-value case, for which the maximum and minimum are the
        // same.  We don't want an axis of zero width.

        if (dMinimum == dMaximum)
            dMaximum = (dMinimum == 0) ? 1.0F : (dMinimum + 1F);

        // For the comments below, assume that dMinimum = -2.0 and
        // dMaximum = 103.0.

        // Compute the range.  Sample: 105.0.

        Double dRange = dMaximum - dMinimum;

        // Get the log of the range.  Sample: 2.02.

        Double dLogRange = Math.Log10(dRange);

        // Truncate the log and raise the result to a power of 10.  This gives
        // a factor that will be applied below to the series 1, 2, and 5.
        // Sample factor: 100.

        Double dFactor = Math.Pow(10F, (Int32)dLogRange);

        // For this sample, the range should be divided by 100, 200, and 500 to
        // determine which one yields an interval count closest to 5.  For
        // better results in some cases, also try 10, 20, 50, 1000, 2000,
        // and 5000.

        Double [] adTestDivisors = new Double []
        {
             1.0F,  2.0F,  5.0F,
             0.1F,  0.2F,  0.5F,
            10.0F, 20.0F, 50.0F 
        };

        Double dBestRangePerInterval = Double.MinValue;
        Int32 iBestNumberOfIntervals = Int32.MinValue;

        foreach (Double dTestDivisor in adTestDivisors)
        {
            // Get the range per interval and number of intervals for this
            // divisor.

            Double dTestRangePerInterval = dFactor / dTestDivisor;

            Int32 iTestNumberOfIntervals =
                (Int32)Math.Ceiling(dRange / dTestRangePerInterval);

            // Is the number of intervals closer to 5 than the best case?

            if ( Math.Abs(iTestNumberOfIntervals - 5) <
                Math.Abs(iBestNumberOfIntervals - 5) )
            {
                // Yes.  This is the new best case.

                dBestRangePerInterval = dTestRangePerInterval;
                iBestNumberOfIntervals = iTestNumberOfIntervals;
            }
        }

        // Sample results: dBestRangePerInterval = 20,
        // iBestNumberOfIntervals = 5.

        // Get the first gridline value.  Sample: -20.0.

        Double dFirstGridlineValue =
            (Math.Floor(dMinimum / dBestRangePerInterval)
            * dBestRangePerInterval);

        // Get all the values.  Sample: -20, 0, 20, ..., 120.

        ArrayList oGridlineValues = new ArrayList(iBestNumberOfIntervals + 1);
        Double dGridlineValue = dFirstGridlineValue;

        while (true)
        {
            oGridlineValues.Add(dGridlineValue);

            if (dMaximum <= dGridlineValue)
            {
                break;
            }

            dGridlineValue += dBestRangePerInterval;
        }

        Debug.Assert(oGridlineValues.Count >= 2);

        adGridlineValues = ( Double[] )
            oGridlineValues.ToArray( typeof(Double) );

        // Get the decimal part of the range.

        Double dDecimalPartOfRangePerInterval = Math.Abs(
            dBestRangePerInterval - Math.Truncate(dBestRangePerInterval)
            );

        // Figure out how many decimal places it takes to show the interval.

        if (dDecimalPartOfRangePerInterval == 0)
        {
            iDecimalPlacesToShow = 0;
        }
        else
        {
            iDecimalPlacesToShow = -(Int32)Math.Floor(
                Math.Log10(dDecimalPartOfRangePerInterval)
                );
        }
    }

    //*************************************************************************
    //  Method: GetLogAxisGridlineValues()
    //
    /// <summary>
    /// Gets an array of values for drawing axis gridlines on a log axis.
    /// </summary>
    ///
    /// <param name="fMinimum">
    /// Minimum value that will be displayed along the axis.
    /// </param>
    ///
    /// <param name="fMaximum">
    /// Maximum value that will be displayed along the axis.
    /// </param>
    ///
    /// <returns>
    /// Array of values to draw gridlines for.  Sample arrays: {1, 10, 100},
    /// {0.1, 1.0, 10}.
    /// </returns>
    ///
    /// <remarks>
    /// Given a log chart axis that will display a specified range of values,
    /// this method returns an array of values for which gridlines should be
    /// drawn.  The values are computed in such a way that 1) the minimum
    /// gridline value is the largest power of 10 that is less than or equal to
    /// <paramref name="fMinimum" />; 2) the maximum gridline value is the
    /// smallest power of 10 that is greater than or equal to <paramref
    /// name="fMaximum" />; 3) there is a gridline for each power of 10 in
    /// between the minimum and maximum gridline values.
    /// </remarks>
    //*************************************************************************

    public static Single []
    GetLogAxisGridlineValues
    (
        Single fMinimum,
        Single fMaximum
    )
    {
        Debug.Assert(fMaximum >= fMinimum);

        if (fMaximum < fMinimum)
        {
            throw new InvalidOperationException(
                "ChartUtil.GetLogAxisGridlineValues: Maximum value must be"
                + " >= minimum value."
                );
        }
        else if (fMinimum <= 0)
        {
            throw new InvalidOperationException(
                "ChartUtil.GetLogAxisGridlineValues: Minimum value must be"
                + " > 0 when using log scaling.");
        }

        // For the comments below, assume that fMinimum = 0.3 and
        // fMaximum = 103.0.

        // Get the first gridline value.  Sample: 0.1.

        Double dFirstGridlineValue =
            Math.Pow( 10.0, Math.Floor( Math.Log10(fMinimum) ) );

        // Get the last gridline value.  Sample: 1000.0.

        Double dLastGridlineValue =
            Math.Pow( 10.0, Math.Ceiling( Math.Log10(fMaximum) ) );

        // Handle the one-value case, for which the maximum and minimum are the
        // same.  We don't want an axis of zero width.

        if (dLastGridlineValue == dFirstGridlineValue)
            dLastGridlineValue *= 10.0;

        Debug.Assert(dLastGridlineValue > dFirstGridlineValue);
        Debug.Assert(dFirstGridlineValue != 0);

        // Compute the total number of gridline values.  Sample: 5.

        Int32 iGridlineValues = 1 +
            (Int32)Math.Log10(dLastGridlineValue / dFirstGridlineValue);

        Debug.Assert(iGridlineValues >= 2);

        Single [] afGridlineValues = new Single[iGridlineValues];

        // Set the end values.

        afGridlineValues[0] = (Single)dFirstGridlineValue;
        afGridlineValues[iGridlineValues - 1] = (Single)dLastGridlineValue;

        // Fill in the middle values.

        for (Int32 i = 1; i < iGridlineValues - 1; i++)
            afGridlineValues[i] = (Single)(afGridlineValues[i - 1] * 10.0);

        return (afGridlineValues);
    }

    //*************************************************************************
    //  Method: DrawAxes()
    //
    /// <summary>
    /// Draws a chart's x and y axes.
    /// </summary>
    ///
    /// <param name="oGraphics">
    /// Object to draw on.
    /// </param>
    ///
    /// <param name="oPlotRectangle">
    /// Rectangle containing the plot area, which is defined by the chart's
    /// axes.
    /// </param>
    ///
    /// <param name="oColor">
    /// Color to use.
    /// </param>
    ///
    /// <param name="iWidth">
    /// Width of the axes.
    /// </param>
    //*************************************************************************

    public static void
    DrawAxes
    (
        Graphics oGraphics, 
        Rectangle oPlotRectangle,
        Color oColor,
        Int32 iWidth
    )
    {
        Debug.Assert(oGraphics != null);
        Debug.Assert(iWidth >= 0);

        Pen oPen = new Pen(oColor, iWidth);

        Point [] aoPoints =
        {
            new Point(oPlotRectangle.Left, oPlotRectangle.Top),
            new Point(oPlotRectangle.Left, oPlotRectangle.Bottom),
            new Point(oPlotRectangle.Right, oPlotRectangle.Bottom)
        };

        oGraphics.DrawLines(oPen, aoPoints);

        oPen.Dispose();
    }

    //*************************************************************************
    //  Method: DrawAxisLabels()
    //
    /// <overloads>
    /// Draws axis labels on a chart.
    /// </overloads>
    ///
    /// <summary>
    /// Draws axis labels on a chart that has x and y axes.
    /// </summary>
    ///
    /// <param name="sXAxisLabel">
    /// Label for the x axis.  Can be an empty string or null.
    /// </param>
    ///
    /// <param name="sYAxisLabel">
    /// Label for the y axis.  Can be an empty string or null.
    /// </param>
    ///
    /// <param name="oGraphics">
    /// Object to draw on.
    /// </param>
    ///
    /// <param name="oChartRectangle">
    /// Rectangle containing the entire chart.
    /// </param>
    ///
    /// <param name="oPlotRectangle">
    /// Rectangle containing the plot area, which is defined by the chart's
    /// axes.
    /// </param>
    ///
    /// <param name="oFont">
    /// Font to use.
    /// </param>
    ///
    /// <param name="oLabelColor">
    /// Color to use for all the labels.
    /// </param>
    ///
    /// <param name="fLabelHeightMultiple">
    /// Total height of the axis labels, including space above and below the
    /// text, in multiples of the axis label font height.  Must be at
    /// least 1.0.
    /// </param>
    ///
    /// <remarks>
    /// This is meant for use on a chart that has x and y axes.  It draws 
    /// <paramref name="sXAxisLabel" /> from left-to-right below the x axis,
    /// and <paramref name="sYAxisLabel" /> from bottom-to-top to the left of
    /// the y axis.
    /// </remarks>
    //*************************************************************************

    public static void
    DrawAxisLabels
    (
        String sXAxisLabel,
        String sYAxisLabel,
        Graphics oGraphics,
        Rectangle oChartRectangle,
        Rectangle oPlotRectangle,
        Font oFont,
        Color oLabelColor,
        Single fLabelHeightMultiple
    )
    {
        Debug.Assert(oGraphics != null);
        Debug.Assert(oChartRectangle.Width > 0);
        Debug.Assert(oChartRectangle.Height > 0);
        Debug.Assert(oPlotRectangle.Width > 0);
        Debug.Assert(oPlotRectangle.Height > 0);
        Debug.Assert(oFont != null);
        Debug.Assert(fLabelHeightMultiple >= 1F);

        DrawAxisLabels(sXAxisLabel, sYAxisLabel, null, oGraphics,
            oChartRectangle, oPlotRectangle, oFont, oLabelColor, oLabelColor,
            oLabelColor, fLabelHeightMultiple);
    }

    //*************************************************************************
    //  Method: DrawAxisLabels()
    //
    /// <summary>
    /// Draws axis labels on a chart that has x, primary y, and secondary y
    /// axes.
    /// </summary>
    ///
    /// <param name="sXAxisLabel">
    /// Label for the x axis.  Can be an empty string or null.
    /// </param>
    ///
    /// <param name="sYAxisLabel">
    /// Label for the primary y axis.  Can be an empty string or null.
    /// </param>
    ///
    /// <param name="sYAxisLabelSecondary">
    /// Label for the secondary y axis.  Can be an empty string or null.
    /// </param>
    ///
    /// <param name="oGraphics">
    /// Object to draw on.
    /// </param>
    ///
    /// <param name="oChartRectangle">
    /// Rectangle containing the entire chart.
    /// </param>
    ///
    /// <param name="oPlotRectangle">
    /// Rectangle containing the plot area, which is defined by the chart's
    /// axes.
    /// </param>
    ///
    /// <param name="oFont">
    /// Font to use.
    /// </param>
    ///
    /// <param name="oXAxisLabelColor">
    /// Color to use for the x axis label.
    /// </param>
    ///
    /// <param name="oYAxisLabelColor">
    /// Color to use for the y-axis label.
    /// </param>
    ///
    /// <param name="oYAxisLabelColorSecondary">
    /// Color to use for the secondary y-axis label.
    /// </param>
    ///
    /// <param name="fLabelHeightMultiple">
    /// Total height of the axis labels, including space above and below the
    /// text, in multiples of the axis label font height.  Must be at
    /// least 1.0.
    /// </param>
    ///
    /// <remarks>
    /// This is meant for use on a chart that has an x axis, a primary y axis
    /// at the left edge of the chart, and a seconary y axis at the right edge
    /// of the chart.  It draws <paramref name="sXAxisLabel" /> from
    /// left-to-right below the x axis, <paramref name="sYAxisLabel" /> from
    /// bottom-to-top to the left of the primary y axis, and <paramref
    /// name="sYAxisLabelSecondary" /> from bottom-to-top to the right of the
    /// secondary y axis.
    /// </remarks>
    //*************************************************************************

    public static void
    DrawAxisLabels
    (
        String sXAxisLabel,
        String sYAxisLabel,
        String sYAxisLabelSecondary,
        Graphics oGraphics,
        Rectangle oChartRectangle,
        Rectangle oPlotRectangle,
        Font oFont,
        Color oXAxisLabelColor,
        Color oYAxisLabelColor,
        Color oYAxisLabelColorSecondary,
        Single fLabelHeightMultiple
    )
    {
        Debug.Assert(oGraphics != null);
        Debug.Assert(oChartRectangle.Width > 0);
        Debug.Assert(oChartRectangle.Height > 0);
        Debug.Assert(oPlotRectangle.Width > 0);
        Debug.Assert(oPlotRectangle.Height > 0);
        Debug.Assert(oFont != null);
        Debug.Assert(fLabelHeightMultiple >= 1F);

        SolidBrush oLabelBrush = new SolidBrush(Color.Black);
        Single fLabelHeight = oFont.GetHeight(oGraphics);
        StringFormat oStringFormat = new StringFormat();
        oStringFormat.Alignment = StringAlignment.Center;
        oStringFormat.LineAlignment = StringAlignment.Center;

        if (sXAxisLabel != null)
        {
            // Draw the label for the x axis.  Center it horizontally within
            // the width of the plot rectangle.  Center it vertically within
            // the total space reserved for the label.

            oLabelBrush.Color = oXAxisLabelColor;

            oGraphics.DrawString(
                sXAxisLabel,
                oFont,
                oLabelBrush,
                oPlotRectangle.Left + oPlotRectangle.Width / 2,

                oChartRectangle.Bottom
                    - (fLabelHeightMultiple * fLabelHeight) / 2,

                oStringFormat
                );
        }

        if (sYAxisLabel != null)
        {
            oLabelBrush.Color = oYAxisLabelColor;

            GraphicsState oGraphicsState = oGraphics.Save();

            // Translate the Graphics object so that a coordinate of (0,0) is
            // centered horizontally within the total space reserved for the
            // label, and vertically within the height of the plot rectangle.

            oGraphics.TranslateTransform(

                oChartRectangle.Left +
                    (fLabelHeightMultiple * fLabelHeight) / 2,

                oPlotRectangle.Top + oPlotRectangle.Height / 2
                );

            // Force the text to be displayed vertically from bottom to top.
            // (Note: The StringFormatFlags.DirectionVertical flag causes text
            // to be displayed from top to bottom, which is why a transform is
            // used here instead.)

            oGraphics.RotateTransform(-90F);

            // Draw the label for the y axis.

            oGraphics.DrawString(sYAxisLabel, oFont, oLabelBrush,
                0, 0, oStringFormat);

            oGraphics.Restore(oGraphicsState);
        }

        if (sYAxisLabelSecondary != null)
        {
            oLabelBrush.Color = oYAxisLabelColorSecondary;

            GraphicsState oGraphicsState = oGraphics.Save();

            // Translate the Graphics object so that a coordinate of (0,0) is
            // centered horizontally within the total space reserved for the
            // label, and vertically within the height of the plot rectangle.

            oGraphics.TranslateTransform(

                oChartRectangle.Right -
                    (fLabelHeightMultiple * fLabelHeight) / 2,

                oPlotRectangle.Top + oPlotRectangle.Height / 2
                );

            // Force the text to be displayed vertically from bottom to top.

            oGraphics.RotateTransform(-90F);

            // Draw the label for the y axis.

            oGraphics.DrawString(sYAxisLabelSecondary, oFont, oLabelBrush,
                0, 0, oStringFormat);

            oGraphics.Restore(oGraphicsState);
        }

        oLabelBrush.Dispose();
    }
}

}
