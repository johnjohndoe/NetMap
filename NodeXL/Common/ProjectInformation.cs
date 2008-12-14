
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;

namespace Microsoft.NodeXL.Common
{
//*****************************************************************************
//	Class: ProjectInformation
//
/// <summary>
///	Contains general information about the project.
/// </summary>
//*****************************************************************************

public static class ProjectInformation
{
    //*************************************************************************
    //  Public constants
    //*************************************************************************

	/// Project's home page.

	public const String HomePage = "http://www.codeplex.com/NodeXL";

	/// Page from which the latest release of the application can be
	/// downloaded.

	public const String DownloadPage =
		"http://www.codeplex.com/NodeXL/Release/ProjectReleases.aspx";

	/// Email address to send bug reports to.

	public const String BugReportEmailAddress = "nodexl@microsoft.com";

	/// Project's team members, separated by line breaks, ordered by last name.

	public const String TeamMembers = 
		"Vladimir Barash"
		+ "\r\nTony Capone"
		+ "\r\nCody Dunne"
		+ "\r\nEric Gleave"
		+ "\r\nNatasa Milic-Frayling"
		+ "\r\nAdam Perer"
		+ "\r\nEduarda Mendes Rodrigues"
		+ "\r\nBen Shneiderman"
		+ "\r\nMarc Smith"
		;
}

}
