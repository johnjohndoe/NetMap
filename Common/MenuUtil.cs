
//	Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Research.CommunityTechnologies.AppLib
{
//*****************************************************************************
//	Class: MenuUtil
//
/// <summary>
///	Menu utility methods.
/// </summary>
///
///	<remarks>
///	This class contains utility methods for dealing with menus.  All methods
/// are static.
///	</remarks>
//*****************************************************************************

public class MenuUtil
{
	//*************************************************************************
	//	Constructor: MenuUtil()
	//
	/// <summary>
	/// Do not use this constructor.
	/// </summary>
	///
	/// <remarks>
	/// Do not use this constructor.  All MenuUtil methods are static.
	/// </remarks>
	//*************************************************************************

	private
	MenuUtil()
	{
		// (All methods are static.)
	}

	//*************************************************************************
	//	Method: EnableMenuItems()
	//
	/// <summary>
	/// Enables or disables one or more menu items.
	/// </summary>
	///
	///	<param name="bEnable">
	/// true to enable the menu items, false to disable them.
	/// </param>
	///
	///	<param name="aoMenuItems">
	/// One or more menu items to enable or disable.
	/// </param>
	//*************************************************************************

	public static void
	EnableMenuItems
	(
		Boolean bEnable,
		params MenuItem [] aoMenuItems
	)
	{
		foreach (MenuItem oMenuItem in aoMenuItems)
		{
			oMenuItem.Enabled = bEnable;
		}
	}

	//*************************************************************************
	//	Method: EnableToolStripMenuItems()
	//
	/// <summary>
	/// Enables or disables one or more tool strip menu items.
	/// </summary>
	///
	///	<param name="bEnable">
	/// true to enable the items, false to disable them.
	/// </param>
	///
	///	<param name="aoToolStripMenuItems">
	/// One or more tool strip menu items to enable or disable.
	/// </param>
	///
	/// <remarks>
	/// This can be used only in .NET 2.0 and later applications.  You must
	/// define a "NET20" compiler constant.
	/// </remarks>
	//*************************************************************************

	public static void
	EnableToolStripMenuItems
	(
		Boolean bEnable,
		params ToolStripMenuItem [] aoToolStripMenuItems
	)
	{
		foreach (ToolStripMenuItem oToolStripMenuItem in aoToolStripMenuItems)
		{
			oToolStripMenuItem.Enabled = bEnable;
		}
	}

	//*************************************************************************
	//	Method: EnableAllDescendentToolStripMenuItems()
	//
	/// <summary>
	/// Enables or disables all descendent tool strip menu items of a specified
	/// parent tool strip menu item.
	/// </summary>
	///
	///	<param name="bEnable">
	/// true to enable the items, false to disable them.
	/// </param>
	///
	///	<param name="oParentToolStripMenuItem">
	/// Parent tool strip menu item whose descendent tool strip menu items
	/// should be enabled or disabled.
	/// </param>
	///
	/// <remarks>
	/// This method recurses through all descendent tool strip menu items of
	/// <paramref name="oParentToolStripMenuItem" /> and enables or disables
	/// them.
	///
	/// <para>
	/// This can be used only in .NET 2.0 and later applications.  You must
	/// define a "NET20" compiler constant.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public static void
	EnableAllDescendentToolStripMenuItems
	(
		Boolean bEnable,
		ToolStripMenuItem oParentToolStripMenuItem
	)
	{
		Debug.Assert(oParentToolStripMenuItem != null);

		foreach (ToolStripItem oToolStripItem in
			oParentToolStripMenuItem.DropDownItems)
		{
			if (oToolStripItem is ToolStripMenuItem)
			{
				ToolStripMenuItem oChildToolStripMenuItem =
					(ToolStripMenuItem)oToolStripItem;

				oChildToolStripMenuItem.Enabled = bEnable;

				EnableAllDescendentToolStripMenuItems(
					bEnable, oChildToolStripMenuItem);
			}
		}
	}

	//*************************************************************************
	//	Method: GetMenuPath()
	//
	/// <summary>
	/// Gets a string describing the path to a tool strip menu item.
	/// </summary>
	///
	///	<param name="aoToolStripMenuItemss">
	/// Sequence of one or more menu items.  Each item must be a child of the
	/// previous item.  Sample: mniFile, mniFileNew, mniFileNewDocument.
	/// </param>
	///
	/// <returns>
	/// A string describing the path to the last menu item in <paramref
	/// name="aoToolStripMenuItemss" />.  Sample: "File, New, Document".
	/// </returns>
	///
	/// <remarks>
	/// Use this method to obtain a displayable path to a menu item in a robust
	/// manner.  Instead of hardcoding the string "File, New, Document" in a
	/// user message, for example, pass the sequence of menu item to this
	/// method and use the returned string in the message.  Then if the text of
	/// any of the menu items is changed, the message will change
	/// automatically.  Also, if one of the menu items is moved or deleted, an
	/// exception is thrown to alert the programmer to the problem.
	///
	/// <para>
	/// This can be used only in .NET 2.0 and later applications.  You must
	/// define a "NET20" compiler constant.
	/// </para>
	///
	/// </remarks>
	//*************************************************************************

	public static String
	GetMenuPath
	(
		params ToolStripMenuItem [] aoToolStripMenuItemss
	)
	{
		Debug.Assert(aoToolStripMenuItemss != null);
		Debug.Assert(aoToolStripMenuItemss.Length > 0);

		StringBuilder oMenuPath = new StringBuilder();
		ToolStripMenuItem oParentToolStripMenuItem = null;

		foreach (ToolStripMenuItem oToolStripMenuItem in aoToolStripMenuItemss)
		{
			if (oParentToolStripMenuItem != null)
			{
				if ( !oParentToolStripMenuItem.DropDownItems.Contains(
					oToolStripMenuItem) )
				{
					throw new ApplicationException( String.Format(

						"MenuUtil.GetMenuPath: {0} is not a child of {1}."
						,
						oToolStripMenuItem.Text,
						oParentToolStripMenuItem.Text
						) );
				}
				
				oMenuPath.Append(", ");
			}

			String sFilteredMenuText =

				oToolStripMenuItem.Text
					.Replace("&", String.Empty)
					.Replace("...", String.Empty)
					;

			oMenuPath.Append(sFilteredMenuText);

            oParentToolStripMenuItem = oToolStripMenuItem;
		}

		return ( oMenuPath.ToString() );
	}
}

}
