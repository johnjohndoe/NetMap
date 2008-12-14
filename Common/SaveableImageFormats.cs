
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//	Class: SaveableImageFormats
//
/// <summary>
///	Contains information about image formats that can be saved to disk.
/// </summary>
///
///	<remarks>
/// This class encapsulates information about image formats that can be saved
/// to disk.  All properties and methods are static.
///	</remarks>
//*****************************************************************************

public class SaveableImageFormats : Object
{
	//*************************************************************************
	//	Constructor: SaveableImageFormats()
	//
	/// <summary>
	/// Do not use this constructor.  All properties and methods are static.
	/// </summary>
	//*************************************************************************

	private SaveableImageFormats()
	{
		// (Do nothing.)
	}

	//*************************************************************************
	//	Property: ImageFormats
	//
	/// <summary>
	/// Gets an array of image formats that can be saved to disk.
	/// </summary>
	///
	/// <value>
	/// An array of ImageFormat values.
	/// </value>
	//*************************************************************************

	public static ImageFormat [] ImageFormats
	{
		get
		{
			return (m_aoImageFormats);
		}
	}

	//*************************************************************************
	//	Property: Filter
	//
	/// <summary>
	/// Gets a filter suitable for use in a common dialog.
	/// </summary>
	///
	/// <value>
	/// A filter string for the image formats that can be saved to disk.
	/// </value>
	//*************************************************************************

	public static String Filter
	{
		get
		{
			return (m_sFilter);
		}
	}

	//*************************************************************************
	//	Method: GetFileExtension()
	//
	/// <summary>
	///	Returns a file extension corresponding to a saveable image format.
	/// </summary>
	///
	/// <param name="eSaveableImageFormat">
	///	One of the ImageFormat values returned by <cref name="ImageFormats" />.
	/// </param>
	///
	///	<returns>
	/// The returned file extension is in lower case and does not include a
	/// period.  Sample: "bmp".
	///	</returns>
	//*************************************************************************

	public static String
	GetFileExtension
	(
		ImageFormat eSaveableImageFormat
	)
	{
		Int32 iIndex = Array.IndexOf(m_aoImageFormats, eSaveableImageFormat);

		if (iIndex == -1)
		{
			throw new Exception(
				"SaveableImageFormats.GetFileExtension: No such format.");
		}

		return ( m_asFileExtensions[iIndex] );
	}

	//*************************************************************************
	//	Method: InitializeListControl()
	//
	/// <summary>
	///	Initializes a ListControl with image formats that can be saved to disk.
	/// </summary>
	///
	/// <param name="oListControl">
	///	ListControl to initialize.
	/// </param>
	///
	///	<remarks>
	/// After this method is called, the <see
	/// cref="ListControl.SelectedValue" /> property can be used to get the
	/// ImageFormat value selected by the user.
	///	</remarks>
	//*************************************************************************

	public static void
	InitializeListControl
	(
		ListControl oListControl
	)
	{
		Debug.Assert(oListControl != null);

		ListControlPlus.PopulateWithObjectsAndText(oListControl,
			new Object [] {

			ImageFormat.Bmp, "BMP (.bmp)",
			ImageFormat.Gif, "GIF (.gif)",
			ImageFormat.Jpeg, "JPEG (.jpg)",
			ImageFormat.Png, "PNG (.png)",
			ImageFormat.Tiff, "TIFF (.tif)"
			}

			);

		oListControl.SelectedValue = ImageFormat.Jpeg;
	}

	//*************************************************************************
	//	Protected member data
	//*************************************************************************

	/// Image formats that can be saved to disk.

	protected static ImageFormat[] m_aoImageFormats =
	{
		ImageFormat.Bmp,
		ImageFormat.Gif,
		ImageFormat.Jpeg,
		ImageFormat.Png,
		ImageFormat.Tiff
	};

	/// Corresponding file extensions.

	protected static String [] m_asFileExtensions =
	{
		"bmp",
		"gif",
		"jpg",
		"png",
		"tif"
	};

	/// Image filter for use in common dialogs.

	protected static String m_sFilter =
		"BMP (*.bmp)|*.bmp|" +
		"GIF (*.gif)|*.gif|" +
		"JPEG (*.jpg)|*.jpg;*.jpeg;*.jfif|" +
		"PNG (*.png)|*.png|" +
		"TIFF (*.tif)|*.tif;*.tiff"
		;
}

}
