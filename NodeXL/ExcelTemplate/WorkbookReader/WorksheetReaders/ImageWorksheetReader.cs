
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using Microsoft.Research.CommunityTechnologies.AppLib;

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
			// Add the images in the table to the dictionary.

			AddImageTableToDictionary(oImageTable, readWorkbookContext);
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
		ListObject oImageTable,
		ReadWorkbookContext oReadWorkbookContext
    )
    {
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

			AddImageRangeAreaToDictionary(oImageRangeArea,
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
		Range oImageRangeArea,
		ImageTableColumnIndexes oImageTableColumnIndexes,
		ReadWorkbookContext oReadWorkbookContext
    )
    {
		Debug.Assert(oImageRangeArea != null);
		Debug.Assert(oImageTableColumnIndexes.Key != NoSuchColumn);
		Debug.Assert(oImageTableColumnIndexes.FilePath != NoSuchColumn);
		Debug.Assert(oReadWorkbookContext != null);
        AssertValid();

		Object [,] aoImageValues = ExcelUtil.GetRangeValues(oImageRangeArea);

		Dictionary<String, Image> oImageIDDictionary =
			oReadWorkbookContext.ImageIDDictionary;

		// Loop through the rows.

		String sInvalidFilePathErrorMessage = null;
		Range oInvalidCell = null;
		Int32 iRows = oImageRangeArea.Rows.Count;
		Int32 iRowOneBased;

		for (iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
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
				sInvalidFilePathErrorMessage = 

					"The cell {0} doesn't contain an image file path.  If you"
					+ " specify an image ID, you must also specify an image"
					+ " file path."
					;

				goto InvalidFilePath;
			}

			if ( !File.Exists(sFilePath) )
			{
				sInvalidFilePathErrorMessage = 

					"The image file specified in cell {0} doesn't exist."
					;

				goto InvalidFilePath;
			}

			Image oImage = null;;

			try
			{
				oImage = Image.FromFile(sFilePath);
			}
			catch (OutOfMemoryException)
			{
				sInvalidFilePathErrorMessage = 

					"The file specified in cell {0} is not a valid image."
					;

				goto InvalidFilePath;
			}

			oImageIDDictionary[sKey] = oImage;
		}

		return;

		InvalidFilePath:

			Debug.Assert(sInvalidFilePathErrorMessage != null);
			Debug.Assert(sInvalidFilePathErrorMessage.IndexOf("{0}") >= 0);

			oInvalidCell = (Range)oImageRangeArea.Cells[
				iRowOneBased, oImageTableColumnIndexes.FilePath];

			OnWorkbookFormatError( String.Format(

				sInvalidFilePathErrorMessage
				,
				ExcelUtil.GetRangeAddress(oInvalidCell)
				),

				oInvalidCell
				);
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
