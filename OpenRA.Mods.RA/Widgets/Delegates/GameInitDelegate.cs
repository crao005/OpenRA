#region Copyright & License Information
/*
 * Copyright 2007-2010 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made 
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see LICENSE.
 */
#endregion

using System.Collections.Generic;
using OpenRA.FileFormats;
using OpenRA.Network;
using OpenRA.Server;
using OpenRA.Widgets;
using System.Diagnostics;
using System;

namespace OpenRA.Mods.RA.Widgets.Delegates
{
	public class GameInitDelegate : IWidgetDelegate
	{
		GameInitInfoWidget Info;
		
		[ObjectCreator.UseCtor]
		public GameInitDelegate([ObjectCreator.Param] Widget widget)
		{
			Info = widget.GetWidget<GameInitInfoWidget>("INFO");
			Game.ConnectionStateChanged += orderManager =>
			{
				Widget.CloseWindow();
				switch( orderManager.Connection.ConnectionState )
				{
					case ConnectionState.PreConnecting:
						Widget.OpenWindow("MAINMENU_BG");
						break;
					case ConnectionState.Connecting:
						Widget.OpenWindow( "CONNECTING_BG",
							new Dictionary<string, object> { { "host", orderManager.Host }, { "port", orderManager.Port } } );
						break;
					case ConnectionState.NotConnected:
						Widget.OpenWindow( "CONNECTION_FAILED_BG",
							new Dictionary<string, object> { { "orderManager", orderManager } } );
						break;
					case ConnectionState.Connected:
						var lobby = Game.OpenWindow(orderManager.world, "SERVER_LOBBY");
						lobby.GetWidget<ChatDisplayWidget>("CHAT_DISPLAY").ClearChat();
						lobby.GetWidget("CHANGEMAP_BUTTON").Visible = true;
						lobby.GetWidget("LOCKTEAMS_CHECKBOX").Visible = true;
						lobby.GetWidget("DISCONNECT_BUTTON").Visible = true;
						break;
				}
			};
			
			if (FileSystem.Exists(Info.TestFile))
				ContinueLoading(widget);
			else
			{
				widget.GetWidget("INIT_DOWNLOAD").OnMouseUp = mi =>
				{
					ContinueLoading(widget);
					return true;
				};
				
				widget.GetWidget("INIT_FROMCD").OnMouseUp = mi =>
				{
					SelectDisk(path => System.Console.WriteLine(path));
					return true;
				};
								
				widget.GetWidget("INIT_QUIT").OnMouseUp = mi => { Game.Exit(); return true; };
			}
		}
		
		
		void SelectDisk(Action<string> withPath)
		{
			Process p = new Process();
			p.StartInfo.FileName = "OpenRA.Launcher.Mac/build/Release/OpenRA.app/Contents/MacOS/OpenRA";
			p.StartInfo.Arguments = "--filepicker --title \"Select CD\" --message \"Select the {0} CD\" --require-directory --button-text \"Select\"".F(Info.GameTitle);
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.CreateNoWindow = true;
			p.EnableRaisingEvents = true;
			p.Exited += (_,e) =>
			{
				withPath(p.StandardOutput.ReadToEnd());
			};
			p.Start();
		}
		
		void ContinueLoading(Widget widget)
		{
			Game.LoadShellMap();
			Widget.RootWidget.Children.Remove(widget);
			Widget.OpenWindow("MAINMENU_BG");
		}
	}
}