
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.NodeXL.Core;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.DateTimeLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexAttributeGrouper
//
/// <summary>
/// Groups the workbook's vertices using the values in a specified vertex
/// column.
/// </summary>
///
/// <remarks>
/// Each method groups the workbook's vertices using the values in a specified
/// vertex column, then writes the results to the group and group vertices
/// worksheets.  There is one method for each value in the <see
/// cref="ExcelColumnFormat" /> enumeration.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class VertexAttributeGrouper
{
    //*************************************************************************
    //  Method: GroupByVertexAttributeNumber()
    //
    /// <summary>
    /// Groups the workbook's vertices using the values in a vertex column of
    /// type <see cref="ExcelColumnFormat.Number" />.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the vertices to group.
    /// </param>
    ///
    /// <param name="attributeColumnName">
    /// Name of the column in the vertex table containing the attributes to
    /// group on.
    /// </param>
    ///
    /// <param name="minimumValues">
    /// Collection of the minimum value of each group.
    /// </param>
    //*************************************************************************

    public static void
    GroupByVertexAttributeNumber
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String attributeColumnName,
        IEnumerable<Double> minimumValues
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(attributeColumnName) );
        Debug.Assert(minimumValues != null);

        GroupByVertexAttributeGeneric<Double>(workbook, attributeColumnName,
            minimumValues, Double.MinValue, Double.TryParse,
            dDouble => dDouble);
    }

    //*************************************************************************
    //  Method: GroupByVertexAttributeDate()
    //
    /// <summary>
    /// Groups the workbook's vertices using the values in a vertex column of
    /// type <see cref="ExcelColumnFormat.Date" />.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the vertices to group.
    /// </param>
    ///
    /// <param name="attributeColumnName">
    /// Name of the column in the vertex table containing the attributes to
    /// group on.
    /// </param>
    ///
    /// <param name="minimumValues">
    /// Collection of the minimum value of each group.
    /// </param>
    //*************************************************************************

    public static void
    GroupByVertexAttributeDate
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String attributeColumnName,
        IEnumerable<DateTime> minimumValues
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(attributeColumnName) );
        Debug.Assert(minimumValues != null);

        GroupByVertexAttributeGeneric<DateTime>(workbook, attributeColumnName,
            minimumValues, DateTime.MinValue, DateTime.TryParse,
            DateTimeUtil2.RemoveTime);
    }

    //*************************************************************************
    //  Method: GroupByVertexAttributeTime()
    //
    /// <summary>
    /// Groups the workbook's vertices using the values in a vertex column of
    /// type <see cref="ExcelColumnFormat.Time" />.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the vertices to group.
    /// </param>
    ///
    /// <param name="attributeColumnName">
    /// Name of the column in the vertex table containing the attributes to
    /// group on.
    /// </param>
    ///
    /// <param name="minimumValues">
    /// Collection of the minimum value of each group.
    /// </param>
    //*************************************************************************

    public static void
    GroupByVertexAttributeTime
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String attributeColumnName,
        IEnumerable<DateTime> minimumValues
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(attributeColumnName) );
        Debug.Assert(minimumValues != null);

        StringToTypeConverter<DateTime> oStringToTypeConverter =
        (String stringValue, out DateTime dateTime) =>
        {
            // When an Excel cell is formatted as Time, the string read from
            // the cell is a Decimal.  Sample: 39448.625.

            dateTime = DateTime.MinValue;
            Decimal decDateTime;

            if ( !Decimal.TryParse(stringValue, out decDateTime) )
            {
                return (false);
            }

            dateTime = ExcelDateTimeUtil.ExcelDecimalToDateTime(decDateTime);
            return (true);
        };

        TypeAdjuster<DateTime> oTypeAdjuster =
        (DateTime dateTime) =>
        {
            // Set the date component to some arbitrary, constant value and set
            // the seconds to 0.  The DateTimePicker control doesn't show
            // seconds by default, but its value includes the seconds and that
            // can lead to confusing comparison results.

            return ( new DateTime(
                2000,
                1,
                1,
                dateTime.Hour,
                dateTime.Minute,
                0,
                dateTime.Kind
                ) );
        };

        GroupByVertexAttributeGeneric<DateTime>(workbook, attributeColumnName,
            minimumValues, DateTime.MinValue, oStringToTypeConverter,
            oTypeAdjuster);
    }

    //*************************************************************************
    //  Method: GroupByVertexAttributeDateAndTime()
    //
    /// <summary>
    /// Groups the workbook's vertices using the values in a vertex column of
    /// type <see cref="ExcelColumnFormat.DateAndTime" />.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the vertices to group.
    /// </param>
    ///
    /// <param name="attributeColumnName">
    /// Name of the column in the vertex table containing the attributes to
    /// group on.
    /// </param>
    ///
    /// <param name="minimumValues">
    /// Collection of the minimum value of each group.
    /// </param>
    //*************************************************************************

    public static void
    GroupByVertexAttributeDateAndTime
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String attributeColumnName,
        IEnumerable<DateTime> minimumValues
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(attributeColumnName) );
        Debug.Assert(minimumValues != null);

        TypeAdjuster<DateTime> oTypeAdjuster =
        (DateTime dateTime) =>
        {
            // Set the seconds to 0.  The DateTimePicker control doesn't show
            // seconds by default, but its value includes the seconds and that
            // can lead to confusing comparison results.

            return (new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                dateTime.Hour,
                dateTime.Minute,
                0,
                dateTime.Kind
                ));
        };

        GroupByVertexAttributeGeneric<DateTime>(workbook, attributeColumnName,
            minimumValues, DateTime.MinValue, DateTime.TryParse, oTypeAdjuster);
    }

    //*************************************************************************
    //  Method: GroupByVertexAttributeOther()
    //
    /// <summary>
    /// Groups the workbook's vertices using the values in a vertex column of
    /// type <see cref="ExcelColumnFormat.Other" />.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the vertices to group.
    /// </param>
    ///
    /// <param name="attributeColumnName">
    /// Name of the column in the vertex table containing the attributes to
    /// group on.
    /// </param>
    //*************************************************************************

    public static void
    GroupByVertexAttributeOther
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String attributeColumnName
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(attributeColumnName) );

        IGraph oGraph = ReadWorkbook(workbook);

        // The key is a unique attribute value from the specified column, and
        // the value is a list of the vertices that have that attribute value.

        Dictionary< String, LinkedList<IVertex> > oGroupDictionary =
            new Dictionary< String, LinkedList<IVertex> >();

        foreach (IVertex oVertex in oGraph.Vertices)
        {
            Object oAttributeValue;

            if ( !oVertex.TryGetValue(attributeColumnName, typeof(String),
                out oAttributeValue) )
            {
                continue;
            }

            String sAttributeValue = (String)oAttributeValue;

            LinkedList<IVertex> oVerticesInGroup;

            if ( !oGroupDictionary.TryGetValue(sAttributeValue,
                out oVerticesInGroup) )
            {
                oVerticesInGroup = new LinkedList<IVertex>();
                oGroupDictionary.Add(sAttributeValue, oVerticesInGroup);
            }

            oVerticesInGroup.AddLast(oVertex);
        }

        // Convert the collection of groups to an array of GraphMetricColumn
        // objects, then write the array to the workbook.

        GraphMetricColumn [] aoGraphMetricColumns =
            GroupsToGraphMetricColumnsConverter.Convert< LinkedList<IVertex > >
                (oGroupDictionary.Values, (oGroup) => oGroup);

        WriteGraphMetricsToWorkbook(workbook, aoGraphMetricColumns);
    }

    //*************************************************************************
    //  Delegate: StringToTypeConverter()
    //
    /// <summary>
    /// Method that attempts to convert an attribute value read from a vertex
    /// column from a string to the type T.
    /// </summary>
    ///
    /// <typeparam name="T">
    /// The type to convert to.  Sample: DateTime.
    /// </typeparam>
    ///
    /// <param name="stringValue">
    /// The value fread from the vertex column, as a String.
    /// </param>
    ///
    /// <param name="typeValue">
    /// Where the converted value gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the conversion succeeded.
    /// </returns>
    //*************************************************************************

    private delegate Boolean StringToTypeConverter<T>(
        String stringValue, out T typeValue);


    //*************************************************************************
    //  Delegate: TypeAdjuster()
    //
    /// <summary>
    /// Method that adjusts a value of the type T.
    /// </summary>
    ///
    /// <typeparam name="T">
    /// The type to adjust.  Sample: DateTime.
    /// </typeparam>
    ///
    /// <returns>
    /// The adjusted value.
    /// </returns>
    ///
    /// <remarks>
    /// Sample adjustment: Remove the time component from a DateTime.
    /// </remarks>
    //*************************************************************************

    private delegate T TypeAdjuster<T>(T typeValue);


    //*************************************************************************
    //  Method: GroupByVertexAttributeGeneric()
    //
    /// <summary>
    /// Groups the workbook's vertices using the values in a vertex column of
    /// a type specified by the caller.
    /// </summary>
    ///
    /// <typeparam name="T">
    /// The type of the values in the vertex column.  Sample: DateTime.
    /// </typeparam>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the vertices to group.
    /// </param>
    ///
    /// <param name="sAttributeColumnName">
    /// Name of the column in the vertex table containing the attributes to
    /// group on.
    /// </param>
    ///
    /// <param name="oMinimumValues">
    /// Collection of the minimum value of each group.
    /// </param>
    ///
    /// <param name="oMinimumValueForType">
    /// Minimum possible value of the type T.  Sample: DateTime.MinValue.
    /// </param>
    ///
    /// <param name="oStringToTypeConverter">
    /// Method that attempts to convert an attribute value read from a vertex
    /// column from a string to the type T.
    /// </param>
    ///
    /// <param name="oTypeAdjuster">
    /// Method that adjusts a value of the type T.  Sample adjustment: Remove
    /// the time component from a DateTime.
    /// </param>
    //*************************************************************************

    private static void
    GroupByVertexAttributeGeneric<T>
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        String sAttributeColumnName,
        IEnumerable<T> oMinimumValues,
        T oMinimumValueForType,
        StringToTypeConverter<T> oStringToTypeConverter,
        TypeAdjuster<T> oTypeAdjuster
    )
    where T : IComparable<T>
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert( !String.IsNullOrEmpty(sAttributeColumnName) );
        Debug.Assert(oMinimumValues != null);

        IGraph oGraph = ReadWorkbook(oWorkbook);

        List<T> oMinimumValueList = new List<T>(oMinimumValues);

        // Add a group that will hold vertices whose values are less than the
        // first minimum value in oMinimumValueList.  That way, every vertex
        // will end up in a group.

        oMinimumValueList.Insert(0, oMinimumValueForType);

        // For each group whose minimum value is stored in oMinimumValueList,
        // there is one LinkedList of vertices in oGroupList.

        Int32 iGroups = oMinimumValueList.Count;
        LinkedList<IVertex> [] oGroupList = new LinkedList<IVertex>[iGroups];

        for (Int32 i = 0; i < iGroups; i++)
        {
            oMinimumValueList[i] = oTypeAdjuster( oMinimumValueList[i] );
            oGroupList[i] = new LinkedList<IVertex>();
        }

        foreach (IVertex oVertex in oGraph.Vertices)
        {
            Object oAttributeValue;
            T tAttributeValue;

            if (
                !oVertex.TryGetValue(sAttributeColumnName, typeof(String),
                    out oAttributeValue)
                ||
                !oStringToTypeConverter( (String)oAttributeValue,
                    out tAttributeValue )
                )
            {
                continue;
            }

            tAttributeValue = oTypeAdjuster(tAttributeValue);

            // (This search technique is simple but slow.  It should be
            // replaced with a binary search.)

            for (Int32 i = iGroups - 1; i >= 0; i--)
            {
                if (tAttributeValue.CompareTo( oMinimumValueList[i] ) >= 0)
                {
                    oGroupList[i].AddLast(oVertex);
                    break;
                }
            }
        }

        // Convert the list of groups to an array of GraphMetricColumn objects,
        // then write the array to the workbook.

        GraphMetricColumn [] aoGraphMetricColumns =
            GroupsToGraphMetricColumnsConverter.Convert< LinkedList<IVertex > >
                (oGroupList, (oGroup) => oGroup);

        WriteGraphMetricsToWorkbook(oWorkbook, aoGraphMetricColumns);
    }

    //*************************************************************************
    //  Method: ReadWorkbook()
    //
    /// <summary>
    /// Creates a NodeXL graph from the contents of an Excel workbook.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph data.
    /// </param>
    //*************************************************************************

    private static IGraph
    ReadWorkbook
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);

        WorkbookReader oWorkbookReader = new WorkbookReader();

        ReadWorkbookContext oReadWorkbookContext = new ReadWorkbookContext();

        // Read all columns in the vertex worksheet and store the cell values
        // as metadata on the graph's vertices.

        oReadWorkbookContext.ReadAllEdgeAndVertexColumns = true;

        // GroupsToGraphMetricColumnsConverter.Convert() copies the ID column
        // on the vertex worksheet to the group-vertex worksheet, so make sure
        // that the vertex ID column is filled in.

        oReadWorkbookContext.FillIDColumns = true;

        return ( oWorkbookReader.ReadWorkbook(oWorkbook,
            oReadWorkbookContext) );
    }

    //*************************************************************************
    //  Method: WriteGraphMetricsToWorkbook()
    //
    /// <summary>
    /// Writes an array of GraphMetricColumn objects to the workbook.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the vertices to group.
    /// </param>
    ///
    /// <param name="aoGraphMetricColumns">
    /// An array of GraphMetricColumn objects, one for each column of metrics
    /// that should be written to the workbook.
    /// </param>
    //*************************************************************************

    private static void
    WriteGraphMetricsToWorkbook
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        GraphMetricColumn [] aoGraphMetricColumns
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert(aoGraphMetricColumns != null);

        GraphMetricWriter oGraphMetricWriter = new GraphMetricWriter();

        oGraphMetricWriter.WriteGraphMetricColumnsToWorkbook(
            aoGraphMetricColumns, oWorkbook);

        oGraphMetricWriter.ActivateRelevantWorksheet(aoGraphMetricColumns,
            oWorkbook);
    }
}

}
