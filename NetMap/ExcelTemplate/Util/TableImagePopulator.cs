
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using Microsoft.Research.CommunityTechnologies.AppLib;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: TableImagePopulator
//
/// <summary>
/// Object that populates an image column in an Excel table (ListObject) with
/// images that have been stored in a temporary folder.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class TableImagePopulator : Object
{
    //*************************************************************************
    //  Method: PopulateColumnWithImages()
    //
    /// <summary>
	/// Populates an image column in an Excel table (ListObject) with images
	/// that have been stored in a temporary folder.
    /// </summary>
	///
    /// <param name="workbook">
	/// Workbook containing the table.
    /// </param>
    ///
    /// <param name="worksheetName">
	/// Worksheet containing the table.
    /// </param>
    ///
    /// <param name="tableName">
	/// Name of the table.
    /// </param>
    ///
    /// <param name="imageColumnName">
	/// Name of the image column.  The column gets inserted after <paramref
	/// name="oneBasedImageColumnIndex" /> if it doesn't already exist.
    /// </param>
    ///
    /// <param name="oneBasedImageColumnIndex">
	/// One-based index of the image column after it is inserted.
    /// </param>
    ///
    /// <param name="keyColumnName">
	/// Name of the column containing the keys in the dictionary returned by
	/// <see cref="TemporaryImages.FileNames" />.  For each cell that contains
	/// a key in the dictionary, an image is inserted into the corresponding
	/// cell in the image column.
    /// </param>
    ///
    /// <param name="temporaryImages">
	/// Contains information about the images that should be inserted.
    /// </param>
    ///
    /// <remarks>
	/// If a column named <paramref name="imageColumnName" /> doesn't already
	/// exist, this method inserts it at position <paramref
	/// name="oneBasedImageColumnIndex" />.  It then populates the column with
	/// the temporary images specified by <paramref name="temporaryImages" />,
	/// and deletes the temporary folder containing the images.
	///
	/// <para>
	/// If the specified table doesn't exist, this method does nothing.
	/// </para>
	///
    /// </remarks>
    //*************************************************************************

	public static void
	PopulateColumnWithImages
	(
		Workbook workbook,
		String worksheetName,
		String tableName,
		String imageColumnName,
		Int32 oneBasedImageColumnIndex,
		String keyColumnName,
		TemporaryImages temporaryImages
	)
    {
		Debug.Assert(workbook != null);
		Debug.Assert( !String.IsNullOrEmpty(worksheetName) );
		Debug.Assert( !String.IsNullOrEmpty(tableName) );
		Debug.Assert( !String.IsNullOrEmpty(imageColumnName) );
		Debug.Assert(oneBasedImageColumnIndex >= 1);
		Debug.Assert( !String.IsNullOrEmpty(keyColumnName) );
		Debug.Assert(temporaryImages != null);

		ListObject oTable;
		Range oKeyColumnData;

		// Get the table and the key column data.

		if ( !ExcelUtil.TryGetTable(workbook, worksheetName, tableName,
				out oTable) 
			||
			!ExcelUtil.TryGetTableColumnData(oTable, keyColumnName,
				out oKeyColumnData)
			)
		{
			// Nothing can be done without the table or key column.

			return;
		}

		Range oImageColumnData;

		// Insert the image column if it doesn't already exist.

		if ( !TryGetImageColumnData(oTable, imageColumnName,
			oneBasedImageColumnIndex, out oImageColumnData) )
		{
			// The image column doesn't exist and couldn't be inserted.

			return;
		}

		String sFolder = temporaryImages.Folder;

		if (sFolder == null)
		{
			// No temporary images were created, so nothing more needs to be
			// done.

			return;
		}

		// Reduce the key and image column data to visible areas only.

		Range oVisibleKeyColumnData, oVisibleImageColumnData;

		if (
			!ExcelUtil.TryGetVisibleRange(oKeyColumnData,
				out oVisibleKeyColumnData)
			||
			!ExcelUtil.TryGetVisibleRange(oImageColumnData,
				out oVisibleImageColumnData)
			)
		{
			return;
		}

		Int32 iAreas = oVisibleKeyColumnData.Areas.Count;

		if (iAreas != oVisibleImageColumnData.Areas.Count)
		{
			return;
		}

		// Get the size of each image, in points.

		SizeF oImageSizePt =
			GetImageSizePt(temporaryImages.ImageSizePx, workbook);

		// Get any old images in the image column as a dictionary.  This
		// significantly speeds up the deletion of the old images, because
		// Excel doesn't have to do a linear search on Shape.Name as each image
		// is deleted by PopulateAreaWithImages().

		Debug.Assert(oTable.Parent is Worksheet);

		Dictionary<String, Microsoft.Office.Interop.Excel.Shape> oOldImages =
			GetOldImages( (Worksheet)oTable.Parent, imageColumnName );

		// Populate each area of the image column with images.

		workbook.Application.ScreenUpdating = false;

		try
		{
			for (Int32 iArea = 1; iArea <= iAreas; iArea++)
			{
				PopulateAreaWithImages(oVisibleKeyColumnData.Areas[iArea],
					oVisibleImageColumnData.Areas[iArea], imageColumnName,
					oImageSizePt, oOldImages, temporaryImages);
			}
		}
		finally
		{
			workbook.Application.ScreenUpdating = true;
		}

		// Delete the entire temporary folder.

		Directory.Delete(sFolder, true);
    }

    //*************************************************************************
    //  Method: TryGetImageColumnData()
    //
    /// <summary>
	/// Inserts the image column if it doesn't already exist.
    /// </summary>
	///
    /// <param name="oTable">
	/// Table containing the image column.
    /// </param>
    ///
    /// <param name="sImageColumnName">
	/// Name of the image column.  The column gets inserted after <paramref
	/// name="iOneBasedImageColumnIndex" /> if it doesn't already exist.
    /// </param>
    ///
    /// <param name="iOneBasedImageColumnIndex">
	/// One-based index of the image column after it is inserted.
    /// </param>
    ///
    /// <param name="oImageColumnData">
	/// Where the image column data gets stored if true is returned.
    /// </param>
	///
	/// <returns>
	/// true if successful.
	/// </returns>
    ///
    /// <remarks>
	/// If a column named <paramref name="sImageColumnName" /> doesn't already
	/// exist, this method inserts it at position <paramref
	/// name="iOneBasedImageColumnIndex" />.
    /// </remarks>
    //*************************************************************************

	private static Boolean
	TryGetImageColumnData
	(
		ListObject oTable,
		String sImageColumnName,
		Int32 iOneBasedImageColumnIndex,
		out Range oImageColumnData
	)
    {
		Debug.Assert(oTable != null);
		Debug.Assert( !String.IsNullOrEmpty(sImageColumnName) );
		Debug.Assert(iOneBasedImageColumnIndex >= 1);

		ListColumn oImageColumn;

		// Insert the image column if it doesn't already exist.

		if (
			!ExcelUtil.TryGetTableColumnData(oTable, sImageColumnName,
				out oImageColumnData)
			&&
				(
				!ExcelUtil.TryInsertTableColumn(oTable, sImageColumnName,
					iOneBasedImageColumnIndex, ImageColumnWidthChars,
					out oImageColumn)
				||
				!ExcelUtil.TryGetTableColumnData(oTable, sImageColumnName,
					out oImageColumnData)
				)
			)
		{
			// The image column doesn't exist and couldn't be inserted.

			return (false);
		}

		return (true);
    }

    //*************************************************************************
    //  Method: PopulateAreaWithImages()
    //
    /// <summary>
	/// Populates one area of the image column with images.
    /// </summary>
	///
    /// <param name="oKeyColumnArea">
	/// Area from the key column.
    /// </param>
    ///
    /// <param name="sImageColumnName">
	/// Name of the image column.
    /// </param>
	///
    /// <param name="oImageColumnArea">
	/// Corresponding area from the image column.
    /// </param>
    ///
    /// <param name="oImageSizePt">
	/// Size of each image, in points.
    /// </param>
    ///
    /// <param name="oOldImages">
	/// A dictionary of zero or more key/value pairs.  The key is the
	/// Shape.Name of an old image in the image column and the value is the
	/// image, as a Shape.
    /// </param>
    ///
    /// <param name="oTemporaryImages">
	/// Contains information about the images that should be inserted.
    /// </param>
    ///
    /// <remarks>
	/// This method populates <paramref name="oImageColumnArea" /> with the
	/// temporary images specified by <paramref name="oTemporaryImages" />.
    /// </remarks>
    //*************************************************************************

	private static void
	PopulateAreaWithImages
	(
		Range oKeyColumnArea,
		Range oImageColumnArea,
		String sImageColumnName,
		SizeF oImageSizePt,
		Dictionary<String, Microsoft.Office.Interop.Excel.Shape> oOldImages,
		TemporaryImages oTemporaryImages
	)
    {
		Debug.Assert(oKeyColumnArea != null);
		Debug.Assert(oImageColumnArea != null);
		Debug.Assert( !String.IsNullOrEmpty(sImageColumnName) );
		Debug.Assert(oOldImages != null);
		Debug.Assert(oTemporaryImages != null);

		// Gather some required information.

		Int32 iRows = oKeyColumnArea.Rows.Count;

		Debug.Assert(iRows == oImageColumnArea.Rows.Count);

		Debug.Assert(oKeyColumnArea.Parent is Worksheet);

		Worksheet oWorksheet = (Worksheet)oKeyColumnArea.Parent;

		Microsoft.Office.Interop.Excel.Shapes oShapes = oWorksheet.Shapes;

		Object [,] aoKeyValues = ExcelUtil.GetRangeValues(oKeyColumnArea);

		Dictionary<String, String> oFileNames = oTemporaryImages.FileNames;

		// Set the row heights to fit the images.

		oKeyColumnArea.RowHeight = oImageSizePt.Height + 2 * ImageMarginPt;

		// Get the first cell in the image column.

		Range oImageCell = (Range)oImageColumnArea.Cells[1, 1];

		// Loop through the area's rows.

		for (Int32 iRow = 1; iRow <= iRows; iRow++)
		{
			String sKey, sFileName;

			// Check whether the row's key cell has a corresponding file name
			// in the dictionary.

			if (
				ExcelUtil.TryGetNonEmptyStringFromCell(aoKeyValues, iRow, 1,
					out sKey)
				&&
				oFileNames.TryGetValue(sKey, out sFileName)
				)
			{
				// Give the picture a name that can be recognized by
				// GetOldImages().

				String sPictureName = sImageColumnName + "-" + sKey;

				Microsoft.Office.Interop.Excel.Shape oPicture;

				// If an old version of the picture remains from a previous
				// call to this method, delete it.

				if ( oOldImages.TryGetValue(sPictureName, out oPicture) )
				{
					oPicture.Delete();
				}

				String sFileNameWithPath = Path.Combine(
					oTemporaryImages.Folder, sFileName);

				oPicture = oShapes.AddPicture(sFileNameWithPath,
					MsoTriState.msoFalse, MsoTriState.msoCTrue,
					(Single)(Double)oImageCell.Left + ImageMarginPt,
					(Single)(Double)oImageCell.Top + ImageMarginPt,
					oImageSizePt.Width,
					oImageSizePt.Height
					);

				oPicture.Name = sPictureName;
			}

			// Move down one cell in the image column.

			oImageCell = oImageCell.get_Offset(1, 0);
		}
    }

    //*************************************************************************
    //  Method: GetOldImages()
    //
    /// <summary>
	/// Gets a dictionary of any old images in the image column.
    /// </summary>
	///
    /// <param name="oWorksheet">
	/// Worksheet containing the table.
    /// </param>
    ///
    /// <param name="sImageColumnName">
	/// Name of the image column.
    /// </param>
	///
	/// <returns>
	/// A dictionary of zero or more key/value pairs.  The key is the
	/// Shape.Name of an old image in the image column and the value is the
	/// image, as a Shape.
	/// </returns>
    //*************************************************************************

	private static Dictionary<String, Microsoft.Office.Interop.Excel.Shape>
	GetOldImages
	(
		Worksheet oWorksheet,
		String sImageColumnName
	)
    {
		Debug.Assert(oWorksheet != null);
		Debug.Assert( !String.IsNullOrEmpty(sImageColumnName) );

		Dictionary<String, Microsoft.Office.Interop.Excel.Shape> oOldImages =
			new Dictionary<String, Microsoft.Office.Interop.Excel.Shape>();

		foreach (Microsoft.Office.Interop.Excel.Shape oShape in
			oWorksheet.Shapes)
		{
			// The names of the images added by this class start with the image
			// column name.

			String sShapeName = oShape.Name;

			if ( sShapeName != null &&
				sShapeName.StartsWith(sImageColumnName) )
			{
				oOldImages[sShapeName] = oShape;
			}
		}

		return (oOldImages);
    }

    //*************************************************************************
    //  Method: GetImageSizePt()
    //
    /// <summary>
	/// Gets the size of each image, in points.
    /// </summary>
	///
    /// <param name="oImageSizePx">
	/// The size of each image, in pixels.
    /// </param>
    ///
    /// <param name="oWorkbook">
	/// Workbook containing the table.
    /// </param>
	///
    /// <returns>
	/// <paramref name="oImageSizePx" /> converted to points.
    /// </returns>
    //*************************************************************************

	private static SizeF
	GetImageSizePt
	(
		Size oImageSizePx,
		Workbook oWorkbook
	)
	{
		Debug.Assert(oWorkbook != null);

		// There doesn't seem to be a direct way to get the screen resolution
		// from Excel or from .NET.  As a workaround, leveredge the fact that 
		// a new bitmap is given a resolution equal to the screen resolution.
		//
		// Because this method is called one time only, this workaround is
		// acceptable.

		Bitmap oBitmap = new Bitmap(10, 10);

		Single fPxPerInchX = oBitmap.HorizontalResolution;
		Single fPxPerInchY = oBitmap.VerticalResolution;

		oBitmap.Dispose();

		const Single PointsPerInch = 72;

		return ( new SizeF(
            (Single)oImageSizePx.Width * PointsPerInch / fPxPerInchX,
			(Single)oImageSizePx.Height * PointsPerInch / fPxPerInchY
			) );
	}


    //*************************************************************************
    //  Private constants
    //*************************************************************************

	/// Width of the image column, in characters.

	private const Single ImageColumnWidthChars = 11;

	/// Margin between the image and the image cell, in points.

	private const Single ImageMarginPt = 2;
}

}
