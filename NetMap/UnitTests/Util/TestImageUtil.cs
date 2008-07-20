
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Microsoft.NetMap.UnitTests
{
//*****************************************************************************
//  Class: TestImageUtil
//
/// <summary>
/// Utility methods for dealing with test images.
/// </summary>
///
/// <remarks>
/// Some unit tests create in-memory bitmaps and compare them to images that
/// have been saved to disk.  This class contains utility methods for dealing
/// with such images.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class TestImageUtil
{
    //*************************************************************************
    //  Method: SaveTestImage()
    //
    /// <summary>
    /// Saves a test image file to disk.
    /// </summary>
	///
	/// <param name="oBitmap">
	/// Bitmap to save to the image file.
	/// </param>
	///
	/// <param name="sTestImageRelativeDirectory">
	/// Directory where the test image file should be stored, relative to the
	/// NetMap\UnitTests directory.  Sample:
	/// "Visualization\VertexDrawers\TestImageFiles";
	/// </param>
	///
	/// <param name="sTestImageFileNameNoExtension">
	/// Name of the test image file, without a path or extension.  Sample:
	/// "DrawVertex1".
	/// </param>
    //*************************************************************************

    public static void
    SaveTestImage
	(
		Bitmap oBitmap,
		String sTestImageRelativeDirectory,
		String sTestImageFileNameNoExtension
	)
    {
		Debug.Assert(oBitmap != null);
		Debug.Assert( !String.IsNullOrEmpty(sTestImageRelativeDirectory) );
		Debug.Assert( !String.IsNullOrEmpty(sTestImageFileNameNoExtension) );

		String sTestImageFileName = GetTestImageFileName(
			sTestImageRelativeDirectory, sTestImageFileNameNoExtension);

		String sTestImageDirectory = Path.GetDirectoryName(sTestImageFileName);

		if ( !Directory.Exists(sTestImageDirectory) )
		{
			Assert.Fail(
				"The unit test attempted to save an image file to disk, but"
				+ " the directory where the file should be saved does not"
				+ " exist.  The missing directory is {0}."
				,
				sTestImageDirectory
				);
		}

		if ( File.Exists(sTestImageFileName)
			&&
			(File.GetAttributes(sTestImageFileName) & FileAttributes.ReadOnly)
				== FileAttributes.ReadOnly
			)
		{
			Assert.Fail(
				"The unit test attempted to save an image file to disk, but"
				+ " the file already exists and is read-only.  The file"
				+ " is {0}."
				,
				sTestImageFileName
				);
		}

		oBitmap.Save(sTestImageFileName, ImageFormat.Jpeg);
    }

    //*************************************************************************
    //  Method: CompareTestImage()
    //
    /// <summary>
    /// Compares a bitmap to a test image file stored on disk.
    /// </summary>
	///
	/// <param name="oBitmap">
	/// Bitmap to compare.
	/// </param>
	///
	/// <param name="sTestImageRelativeDirectory">
	/// Directory where the test image file is stored, relative to the
	/// NetMap\UnitTests directory.  Sample:
	/// "Visualization\VertexDrawers\TestImageFiles";
	/// </param>
	///
	/// <param name="sTestImageFileNameNoExtension">
	/// Name of the test image file, without a path or extension.  Sample:
	/// "DrawVertex1".
	/// </param>
	///
	/// <remarks>
	/// This is meant to be called from a unit test.  It compares <see
	/// cref="oBitmap" /> with a corresponding file that was created by <see
	/// cref="SaveTestImage" />.  The test fails if the comparison fails.
	/// </remarks>
    //*************************************************************************

    public static void
    CompareTestImage
	(
		Bitmap oBitmap,
		String sTestImageRelativeDirectory,
		String sTestImageFileNameNoExtension
	)
    {
		Debug.Assert(oBitmap != null);
		Debug.Assert( !String.IsNullOrEmpty(sTestImageRelativeDirectory) );
		Debug.Assert( !String.IsNullOrEmpty(sTestImageFileNameNoExtension) );

		// Load a bitmap from the specified file.

		String sTestImageFileName = GetTestImageFileName(
			sTestImageRelativeDirectory, sTestImageFileNameNoExtension);

		Bitmap oBitmapFromFile = null;

		try
		{
			oBitmapFromFile = (Bitmap)Image.FromFile(sTestImageFileName);
		}
		catch (FileNotFoundException)
		{
			Assert.Fail(
				"The unit test attempted to compare an in-memory bitmap with"
				+ " an image file on disk, but the file is missing.  The full"
				+ " path to the missing file is {0}."
				,
				sTestImageFileName
				);
		}

		// Compare the bitmap dimensions.

		Int32 iWidth = oBitmapFromFile.Width;
		Int32 iHeight = oBitmapFromFile.Height;

		if (iWidth != oBitmap.Width || iHeight != oBitmap.Height)
		{
			Assert.Fail(
				"The unit test compared an in-memory bitmap with the image"
				+ " file {0}.  The image dimensions differed."
				,
				sTestImageFileName
				);
		}

		// The pixels of oBitmap can't be compared directly with the pixels of
		// oBitmapFromFile, because oBitmapFromFile came from a JPEG and the
		// bitmap-to-JPEG conversion performed within SaveTestImage() probably
		// modified the pixels.
		//
		// Instead, convert oBitmap to a JPEG in memory, then compare the JPEG
		// to oBitmapFromFile.

		MemoryStream oMemoryStream = new MemoryStream();

		oBitmap.Save(oMemoryStream, ImageFormat.Jpeg);

        oMemoryStream.Seek(0, SeekOrigin.Begin);

        Bitmap oBitmapFromMemory = (Bitmap)Image.FromStream(oMemoryStream);

        oMemoryStream.Close();

		Assert.AreEqual(iWidth, oBitmapFromMemory.Width);
		Assert.AreEqual(iHeight, oBitmapFromMemory.Height);

		for (Int32 i = 0; i < iWidth; i++)
		{
			for (Int32 j = 0; j < iHeight; j++)
			{
				if (oBitmapFromMemory.GetPixel(i, j).ToArgb() !=
					oBitmapFromFile.GetPixel(i, j).ToArgb() )
				{
					Assert.Fail(
						"The unit test compared an in-memory bitmap with the"
						+ " image file {0}.  The pixels differed."
						,
						sTestImageFileName
						);
				}
			}
		}
    }

    //*************************************************************************
    //  Method: GetTestImageFileName()
    //
    /// <summary>
    /// Gets the file name of a test image, with full path.
    /// </summary>
	///
	/// <param name="sTestImageRelativeDirectory">
	/// Directory where the test image file is stored, relative to the
	/// NetMap\UnitTests directory.  Sample:
	/// "Visualization\VertexDrawers\TestImageFiles";
	/// </param>
	///
	/// <param name="sTestImageFileNameNoExtension">
	/// Name of the test image file, without a path or extension.  Sample:
	/// "DrawVertex1".
	/// </param>
    //*************************************************************************

    private static String
    GetTestImageFileName
	(
		String sTestImageRelativeDirectory,
		String sTestImageFileNameNoExtension
	)
    {
		Debug.Assert( !String.IsNullOrEmpty(sTestImageRelativeDirectory) );
		Debug.Assert( !String.IsNullOrEmpty(sTestImageFileNameNoExtension) );

		// Get the location of the unit tests assembly.  Sample:
		//
		// E:\NetMap\TestResults\John_JOHNLAPTOP 2006-12-31 10_14_02\Out

		String sTestImageDirectory = Assembly.GetExecutingAssembly().Location;

        sTestImageDirectory = Path.GetDirectoryName(sTestImageDirectory);

		// Go up to the NetMap directory.  Sample: E:\NetMap

		sTestImageDirectory =
			Path.Combine(sTestImageDirectory, "..\\..\\..\\");

		// Go down to the UnitTests directory.  Sample:
		//
		// E:\NetMap\UnitTests

		sTestImageDirectory = Path.Combine(sTestImageDirectory, "UnitTests");

		// Add the specified relative directory.  Sample:
		//
		// E:\NetMap\UnitTests\Visualization\VertexDrawers\TestImageFiles

		sTestImageDirectory =
			Path.Combine(sTestImageDirectory, sTestImageRelativeDirectory);

		// Add the file name.  Sample:
		//
		// E:\NetMap\UnitTests\Visualization\VertexDrawers\TestImageFiles\
		// DrawVertex1

		String sTestImageFileName =
			Path.Combine(sTestImageDirectory, sTestImageFileNameNoExtension);

		// Add the extension.  Sample:
		//
		// E:\NetMap\UnitTests\Visualization\VertexDrawers\TestImageFiles\
		// DrawVertex1.jpg

		sTestImageFileName = Path.ChangeExtension(sTestImageFileName, "jpg");

		// Get rid of the embedded double-periods and backslashes.

		sTestImageFileName = Path.GetFullPath(sTestImageFileName);

		return (sTestImageFileName);
    }
}

}
