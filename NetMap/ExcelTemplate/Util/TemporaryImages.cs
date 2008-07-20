using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.NetMap.ExcelTemplate
{
//*****************************************************************************
//  Class: TemporaryImages
//
/// <summary>
/// Keeps track of temporary images that have been created in a temporary
/// folder.
/// </summary>
///
/// <remarks>
/// This is meant for use by classes that create a temporary folder containing
/// temporary images for use by another class.
///
/// <para>
/// If a temporary folder is created, store its path in the <see
/// cref="Folder" /> property.  If one or more temporary images are
/// created in the folder, store their file names in the dictionary returned
/// by <see cref="FileNames" />, and store their size in <see
/// cref="ImageSizePx" />.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class TemporaryImages : Object
{
    //*************************************************************************
    //  Constructor: TemporaryImages()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="TemporaryImages" /> class.
    /// </summary>
    //*************************************************************************

    public TemporaryImages()
    {
		m_sFolder = null;
		m_oFileNames = new Dictionary<String, String>();
		m_oImageSizePx = Size.Empty;

		AssertValid();
    }

    //*************************************************************************
    //  Property: Folder
    //
    /// <summary>
    /// Gets or sets the full path to the temporary folder that has been
	/// created.
    /// </summary>
    ///
    /// <value>
    /// The full path to the temporary folder that has been created, or null if
	/// a temporary folder hasn't been created.  The default is null.
    /// </value>
    //*************************************************************************

    public String
    Folder
    {
        get
        {
            AssertValid();

            return (m_sFolder);
        }

        set
        {
			m_sFolder = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FileNames
    //
    /// <summary>
    /// Gets a dictionary of temporary image file names.
    /// </summary>
    ///
    /// <value>
	/// A dictionary of temporary image file names.  The key is whatever the
	/// caller defines it to be and the value is the name of the image file,
	/// without a path.
    /// </value>
    //*************************************************************************

    public Dictionary<String, String>
    FileNames
    {
        get
        {
            AssertValid();

            return (m_oFileNames);
        }
    }

    //*************************************************************************
    //  Property: ImageSizePx
    //
    /// <summary>
    /// Gets or sets the size of each image, in pixels.
    /// </summary>
    ///
    /// <value>
    /// The size of each image, in pixels.  The default is Size.Empty.
    /// </value>
    //*************************************************************************

    public Size
    ImageSizePx
    {
        get
        {
            AssertValid();

            return (m_oImageSizePx);
        }

        set
        {
			m_oImageSizePx = value;

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

    public void
    AssertValid()
    {
		// m_sFolder
		Debug.Assert(m_oFileNames != null);
		Debug.Assert(m_oImageSizePx == Size.Empty || m_oImageSizePx.Width > 0);
		Debug.Assert(m_oImageSizePx == Size.Empty || m_oImageSizePx.Height > 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

	/// Full path to the temporary folder, or null if a temporary folder hasn't
	/// been created.

	protected String m_sFolder;

	/// A dictionary of temporary image file names.  The key is whatever the
	/// caller defines it to be and the value is the name of the image file,
	/// without a path.

	protected Dictionary<String, String> m_oFileNames;

	/// The size of each image, in pixels.

	protected Size m_oImageSizePx;
}

}
