using System;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Microsoft.Research.CommunityTechnologies.GraphicsLib
{
//*****************************************************************************
//	Class: ColorUtil
//
/// <summary>
/// Utility methods for working with colors.
/// </summary>
///
///	<remarks>
///	This class adds functionality to the System.Drawing.Color class.  A better
///	design would have a new ColorPlus class inherit from Color, with new
///	methods added to ColorPlus, but because the Color class is sealed, that
///	isn't possible.
///
/// <para>
/// All methods are static.
/// </para>
///
///	</remarks>
//*****************************************************************************

public static class ColorUtil
{
	//*************************************************************************
	//	Method: RgbToHsb()
	//
	/// <summary>
	///	Converts a Color object to the HSB color space.
	/// </summary>
	///
	/// <param name="oColor">
	/// RGB color to convert to HSB.
	/// </param>
	///
	/// <param name="fHue">
	/// Where the hue component gets stored, in degrees.  Ranges from 0 to 360.
	/// </param>
	///
	/// <param name="fSaturation">
	/// Where the saturation component gets stored.  Ranges from 0 to 1.0,
	/// where 0 is grayscale and 1.0 is the most saturated.
	/// </param>
	///
	/// <param name="fBrightness">
	/// Where the brightness component gets stored.  Ranges from 0 to 1.0,
	/// where 0 represents black and 1.0 represents white.
	/// </param>
	//*************************************************************************

	public static void
	RgbToHsb
	(
		Color oColor,
		out Single fHue,
		out Single fSaturation,
		out Single fBrightness
	)
	{
		// Note: Although the Color object has methods for converting RGB to
		// HSB, the ColorRGBToHLS() function in shlwapi.dll is used here
		// instead.  That's because the Color object does NOT have a method for
		// converting HSB to RGB, so the ColorHLSToRGB() function in shlwapi
		// had to be used when implementing ColorUtil.HsbToRgb().  To avoid
		// inconsistencies caused by different algorithms, the shlwapi
		// functions are used by ColorUtil for both conversions.

		Int32 iHue, iSaturation, iBrightness;

		ColorRGBToHLS(oColor.ToArgb(), out iHue, out iBrightness,
			out iSaturation);

		Debug.Assert(iHue >= 0);
		Debug.Assert(iHue <= 240);
		Debug.Assert(iSaturation >= 0);
		Debug.Assert(iSaturation <= 240);
		Debug.Assert(iBrightness >= 0);
		Debug.Assert(iBrightness <= 240);

		// Convert the ranges used by ColorRGBToHLS() to the ranges specified
		// for this method.

		fHue = (Single)( iHue * (360.0 / 240.0) );
		fSaturation = (Single)(iSaturation / 240.0);
		fBrightness = (Single)(iBrightness / 240.0);
	}

	//*************************************************************************
	//	Method: HsbToRgb()
	//
	/// <summary>
	///	Converts an HSB color to an RGB Color object.
	/// </summary>
	///
	/// <param name="fHue">
	/// Hue component, in degrees.  Ranges from 0 to 360.
	/// </param>
	///
	/// <param name="fSaturation">
	/// Saturation component.  Ranges from 0 to 1.0, where 0 is grayscale and
	/// 1.0 is the most saturated.
	/// </param>
	///
	/// <param name="fBrightness">
	/// Brightness component.  Ranges from 0 to 1.0, where 0 represents black
	/// and 1.0 represents white.
	/// </param>
	///
	/// <returns>
	///	RGB Color object.
	/// </returns>
	//*************************************************************************

	public static Color
	HsbToRgb
	(
		Single fHue,
		Single fSaturation,
		Single fBrightness
	)
	{
		Debug.Assert(fHue >= 0);
		Debug.Assert(fHue <= 360);
		Debug.Assert(fSaturation >= 0);
		Debug.Assert(fSaturation <= 1);
		Debug.Assert(fBrightness >= 0);
		Debug.Assert(fBrightness <= 1);

		// Create a Color with alpha=0.

		Color oColor = Color.FromArgb(ColorHLSToRGB(
			(Int32)( fHue * (240.0 / 360.0) ),
			(Int32)(fBrightness * 240.0),
			(Int32)(fSaturation * 240.0)
			) );

		// Set alpha=255.

		return ( Color.FromArgb(255, oColor) );
	}

	//*************************************************************************
	//	Method: SetBrightness()
	//
	/// <summary>
	///	Modifies the brightness of an RGB Color object without affecting the
	/// hue or saturation.
	/// </summary>
	///
	/// <param name="oColor">
	/// RGB color to set the brightness for.
	/// </param>
	///
	/// <param name="fBrightness">
	/// Brightness to set oColor to.  Ranges from 0 to 1.0, where 0 represents
	/// black and 1.0 represents white.
	/// </param>
	///
	/// <returns>
	///	Copy of <paramref name="oColor" /> with modified brightness.
	/// </returns>
	//*************************************************************************

	public static Color
	SetBrightness
	(
		Color oColor,
		Single fBrightness
	)
	{
		Debug.Assert(fBrightness >= 0);
		Debug.Assert(fBrightness <= 1);

		// Convert the RGB color to HSB.

		Single fHue, fSaturation, fOldBrightness;

		RgbToHsb(oColor, out fHue, out fSaturation, out fOldBrightness);

		// Convert back to RGB with the new brightness.

		return ( HsbToRgb(fHue, fSaturation, fBrightness) );
	}

	//*************************************************************************
	//	Method: ColorRGBToHLS()
	//
	/// <summary>
	///	Windows API, converts an RGB color to the HLS color space.
	/// </summary>
	///
	/// <param name="clrRGB">
	/// RGB color to convert to HLS.
	/// </param>
	///
	/// <param name="pwHue">
	/// Where the hue component gets stored, in degrees.  Ranges from 0 to 240.
	/// </param>
	///
	/// <param name="pwLuminance">
	/// Where the luminance component gets stored.  Ranges from 0 to 240, where
	/// 0 represents black and 240 represents white.
	/// </param>
	///
	/// <param name="pwSaturation">
	/// Where the saturation component gets stored.  Ranges from 0 to 240,
	/// where 0 is grayscale and 240 is the most saturated.
	/// </param>
	///
	/// <remarks>
	///	Note the parameter order, which corresponds to H-B-S, not H-S-B.
	/// </remarks>
	//*************************************************************************

	[DllImport("shlwapi.dll")]

	public static extern void
	ColorRGBToHLS
	(
		Int32 clrRGB,
		out Int32 pwHue,
		out Int32 pwLuminance,
		out Int32 pwSaturation
	);

	//*************************************************************************
	//	Method: ColorHLSToRGB()
	//
	/// <summary>
	///	Windows API, converts an HLS color to the RGB color space.
	/// </summary>
	///
	/// <param name="wHue">
	/// Hue component.  Ranges from 0 to 240.
	/// </param>
	///
	/// <param name="wLuminance">
	/// Luminance component.  Ranges from 0 to 240, where 0 represents black
	/// and 240 represents white.
	/// </param>
	///
	/// <param name="wSaturation">
	/// Saturation component.  Ranges from 0 to 240, where 0 is grayscale and
	/// 240 is the most saturated.
	/// </param>
	///
	/// <returns>
	///	RGB color.
	/// </returns>
	//*************************************************************************

	[DllImport("shlwapi.dll")]

	public static extern Int32
	ColorHLSToRGB
	(
		Int32 wHue,
		Int32 wLuminance,
		Int32 wSaturation
	);
}

}
