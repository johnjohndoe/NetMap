using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.NodeXL.ExcelTemplatePlugIns;
using Microsoft.NodeXL.GraphDataProviders.Flickr;
using Microsoft.NodeXL.GraphDataProviders.Twitter;

namespace Microsoft.NodeXL.TestGraphDataProviders
{
/// <summary>
/// </summary>

public partial class MainForm : Form
{
	/// <summary>
	/// </summary>

	public MainForm()
	{
		InitializeComponent();
	}

	private void btnFlickrRelatedTags_Click(object sender, EventArgs e)
	{
		GetGraphData( new FlickrRelatedTagsGraphDataProvider() );
	}

    private void btnTwitterUsers_Click(object sender, EventArgs e)
    {
		GetGraphData( new TwitterUserNetworkGraphDataProvider() );
    }

	private void GetGraphData(IGraphDataProvider oGraphDataProvider)
	{
		wbWebBrowser.GoHome();

		String sGraphData;

		if ( !oGraphDataProvider.TryGetGraphData(out sGraphData) )
		{
			return;
		}

		using ( StreamWriter oStreamWriter =
			new StreamWriter(TempXmlFileName) )
		{
			oStreamWriter.Write(sGraphData);
		}

		wbWebBrowser.Navigate(TempXmlFileName);
	}

	private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		File.Delete(TempXmlFileName);
	}

	private String
	TempXmlFileName
	{
		get
		{
			String sAssemblyPath = Path.GetDirectoryName(
				Assembly.GetExecutingAssembly().CodeBase);

			if ( sAssemblyPath.StartsWith("file:") )
			{
				sAssemblyPath = sAssemblyPath.Substring(6);
			}

			return ( Path.Combine(sAssemblyPath, "TempGetGraphData.xml") );
		}
	}
}
}
