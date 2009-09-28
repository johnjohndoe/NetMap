
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.WpfGraphicsLib;

namespace Microsoft.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ImageWorksheetReader
//
/// <summary>
/// Class that knows how to read an Excel worksheet containing vertex image
/// data.
/// </summary>
//*****************************************************************************

public class ImageWorksheetReader : WorksheetReaderBase
{
    //*************************************************************************
    //  Constructor: ImageWorksheetReader()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ImageWorksheetReader" />
    /// class.
    /// </summary>
    //*************************************************************************

    public ImageWorksheetReader()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: ReadWorksheet()
    //
    /// <summary>
    /// Reads the image worksheet and populates an image dictionary.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="readWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <remarks>
    /// If the image worksheet in <paramref name="workbook" /> contains valid
    /// image data, the data is added to <paramref
    /// name="readWorkbookContext" />.ImageIDDictionary.  Otherwise, a <see
    /// cref="WorkbookFormatException" /> is thrown.
    /// </remarks>
    //*************************************************************************

    public void
    ReadWorksheet
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        ReadWorkbookContext readWorkbookContext
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert(readWorkbookContext != null);
        AssertValid();

        // Attempt to get the optional table that contains image data.

        ListObject oImageTable;

        if ( ExcelUtil.TryGetTable(workbook, WorksheetNames.Images,
            TableNames.Images, out oImageTable) )
        {
            // The code that reads the table can handle hidden rows, but not
            // hidden columns.  Temporarily show all hidden columns in the
            // table.

            ExcelHiddenColumns oHiddenColumns =
                ExcelColumnHider.ShowHiddenColumns(oImageTable);

            // Add the images in the table to the dictionary.

            try
            {
                AddImageTableToDictionary(workbook, oImageTable,
                    readWorkbookContext);
            }
            finally
            {
                ExcelColumnHider.RestoreHiddenColumns(oImageTable,
                    oHiddenColumns);
            }
        }
    }

    //*************************************************************************
    //  Method: GetImageTableColumnIndexes()
    //
    /// <summary>
    /// Gets the one-based indexes of the columns within the table that
    /// contains the image data.
    /// </summary>
    ///
    /// <param name="oImageTable">
    /// Table that contains the image data.
    /// </param>
    ///
    /// <returns>
    /// The column indexes, as a <see cref="ImageTableColumnIndexes" />.
    /// </returns>
    //*************************************************************************

    public ImageTableColumnIndexes
    GetImageTableColumnIndexes
    (
        ListObject oImageTable
    )
    {
        Debug.Assert(oImageTable != null);
        AssertValid();

        ImageTableColumnIndexes oImageTableColumnIndexes =
            new ImageTableColumnIndexes();

        oImageTableColumnIndexes.Key = GetTableColumnIndex(
            oImageTable, ImageTableColumnNames.Key, false);

        oImageTableColumnIndexes.FilePath = GetTableColumnIndex(
            oImageTable, ImageTableColumnNames.FilePath, false);

        return (oImageTableColumnIndexes);
    }

    //*************************************************************************
    //  Method: AddImageTableToDictionary()
    //
    /// <summary>
    /// Adds the contents of the image table to an image dictionary.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="oImageTable">
    /// Table that contains the image data.
    /// </param>
    ///
    /// <param name="oReadWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    //*************************************************************************

    protected void
    AddImageTableToDictionary
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        ListObject oImageTable,
        ReadWorkbookContext oReadWorkbookContext
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oImageTable != null);
        Debug.Assert(oReadWorkbookContext != null);
        AssertValid();

        // Read the range that contains visible image data.  If the table is
        // filtered, the range may contain multiple areas.

        Range oImageRange;

        if ( !ExcelUtil.TryGetVisibleTableRange(
            oImageTable, out oImageRange) )
        {
            return;
        }

        // Get the indexes of the columns within the table.

        ImageTableColumnIndexes oImageTableColumnIndexes =
            GetImageTableColumnIndexes(oImageTable);

        if (oImageTableColumnIndexes.Key == NoSuchColumn ||
            oImageTableColumnIndexes.FilePath == NoSuchColumn)
        {
            // There are no image keys or there are no file paths.

            return;
        }

        // Loop through the areas.

        foreach (Range oImageRangeArea in oImageRange.Areas)
        {
            // Add the contents of the area to the dictionary.

            AddImageRangeAreaToDictionary(oWorkbook, oImageRangeArea,
                oImageTableColumnIndexes, oReadWorkbookContext);
        }
    }

    //*************************************************************************
    //  Method: AddImageRangeAreaToDictionary()
    //
    /// <summary>
    /// Adds the contents of one area of the image range to an image
    /// dictionary.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="oImageRangeArea">
    /// One area of the range that contains image data.
    /// </param>
    ///
    /// <param name="oImageTableColumnIndexes">
    /// One-based indexes of the columns within the image table.
    /// </param>
    ///
    /// <param name="oReadWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <remarks>
    /// This method should be called once for each area in the range that
    /// contains image data.
    /// </remarks>
    //*************************************************************************

    protected void
    AddImageRangeAreaToDictionary
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        Range oImageRangeArea,
        ImageTableColumnIndexes oImageTableColumnIndexes,
        ReadWorkbookContext oReadWorkbookContext
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oImageRangeArea != null);
        Debug.Assert(oImageTableColumnIndexes.Key != NoSuchColumn);
        Debug.Assert(oImageTableColumnIndexes.FilePath != NoSuchColumn);
        Debug.Assert(oReadWorkbookContext != null);
        AssertValid();

        Object [,] aoImageValues = ExcelUtil.GetRangeValues(oImageRangeArea);

        Dictionary<String, ImageSource> oImageIDDictionary =
            oReadWorkbookContext.ImageIDDictionary;

        WpfImageUtil oWpfImageUtil = new WpfImageUtil();
        Range oInvalidCell = null;
        Int32 iRows = oImageRangeArea.Rows.Count;
        String sWorkbookPath = oWorkbook.Path;
        Boolean bAllowRelativeFilePaths = !String.IsNullOrEmpty(sWorkbookPath);

        for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
        {
            // Get the image key.

            String sKey;

            if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoImageValues,
                iRowOneBased, oImageTableColumnIndexes.Key, out sKey) )
            {
                // There is no image key.  Skip the row.

                continue;
            }

            sKey = sKey.ToLower();

            if ( oImageIDDictionary.ContainsKey(sKey) )
            {
                oInvalidCell = (Range)oImageRangeArea.Cells[
                    iRowOneBased, oImageTableColumnIndexes.Key];

                OnWorkbookFormatError( String.Format(

                    "The cell {0} contains a duplicate image ID.  Image IDs"
                    + " must be unique."
                    ,
                    ExcelUtil.GetRangeAddress(oInvalidCell)
                    ),

                    oInvalidCell
                );
            }

            String sFilePath;

            if ( !ExcelUtil.TryGetNonEmptyStringFromCell(aoImageValues,
                iRowOneBased, oImageTableColumnIndexes.FilePath,
                out sFilePath) )
            {
                // There is no image file path.  Skip the row.

                continue;
            }

            if ( sFilePath.ToLower().StartsWith("www.") )
            {
                // The Uri class thinks that "www.somewhere.com" is a relative
                // path.  Fix that.

                sFilePath= "http://" + sFilePath;
            }

            Uri oUri;

            // Is the file path either an URL or a full file path?

            if ( !Uri.TryCreate(sFilePath, UriKind.Absolute, out oUri) )
            {
                // No.  It appears to be a relative path.

                if (bAllowRelativeFilePaths)
                {
                    sFilePath = Path.Combine(sWorkbookPath, sFilePath);
                }
                else
                {
                    oInvalidCell = (Range)oImageRangeArea.Cells[
                        iRowOneBased, oImageTableColumnIndexes.FilePath];

                    OnWorkbookFormatError( String.Format(

                        "The image file path specified in cell {0} is a"
                        + " relative path.  Relative paths must be relative to"
                        + " the saved workbook file, but the workbook hasn't"
                        + " been saved yet.  Either save the workbook or"
                        + " change the image file path to an absolute path,"
                        + " such as \"C:\\MyImages\\Image.jpg\"."
                        ,
                        ExcelUtil.GetRangeAddress(oInvalidCell)
                        ),

                        oInvalidCell
                        );
                }
            }

            // Note that sFilePath may or may not be a valid URI string.  If it
            // is not, GetImageSynchronousIgnoreDpi() will return an error
            // image.

            oImageIDDictionary[sKey] =
                oWpfImageUtil.GetImageSynchronousIgnoreDpi(sFilePath);
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


    //*************************************************************************
    //  Embedded class: ImageTableColumnIndexes
    //
    /// <summary>
    /// Contains the one-based indexes of the columns in the optional image
    /// table.
    /// </summary>
    //*************************************************************************

    public class ImageTableColumnIndexes
    {
        /// Unique key for the image.

        public Int32 Key;

        /// Full path to the image file.

        public Int32 FilePath;
    }
}

}
