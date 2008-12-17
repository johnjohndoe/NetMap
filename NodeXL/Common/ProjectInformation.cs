
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

	/// URL or the project's home page.

	public const String HomePageUrl = "http://www.codeplex.com/NodeXL";

	/// URL of the project's discussion list.

	public const String DiscussionUrl =
		"http://www.codeplex.com/NodeXL/Thread/List.aspx";

	/// URL of the page from which the latest release of the application can be
	/// downloaded.

	public const String DownloadPageUrl =
		"http://www.codeplex.com/NodeXL/Release/ProjectReleases.aspx";

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
