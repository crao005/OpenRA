#region Copyright & License Information
/*
 * Copyright 2007-2011 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

using System.Net;
using OpenRA.GameRules;
using OpenRA.Traits;
using System.Linq;
using OpenRA.Widgets;
using System.Drawing;

namespace OpenRA.Mods.RA.Widgets.Logic
{
	public class IngameChromeLogic
	{
		Widget gameRoot;

		[ObjectCreator.UseCtor]
		public IngameChromeLogic(World world)
		{
			var r = Ui.Root;
			gameRoot = r.Get("INGAME_ROOT");
			var optionsBG = gameRoot.Get("INGAME_OPTIONS_BG");

			r.Get<ButtonWidget>("INGAME_OPTIONS_BUTTON").OnClick = () =>
			{
				optionsBG.Visible = !optionsBG.Visible;
				if (world.LobbyInfo.IsSinglePlayer)
					world.IssueOrder(Order.PauseGame());
			};
			
			var cheatsButton = gameRoot.Get<ButtonWidget>("CHEATS_BUTTON");
			cheatsButton.OnClick = () =>
			{
				Game.OpenWindow("CHEATS_PANEL", new WidgetArgs() {{"onExit", () => {} }});
			};
			cheatsButton.IsVisible = () => world.LocalPlayer != null && world.LobbyInfo.GlobalSettings.AllowCheats;

			var iop = world.WorldActor.TraitsImplementing<IObjectivesPanel>().FirstOrDefault();
			if (iop != null && iop.ObjectivesPanel != null)
			{
				var objectivesButton = gameRoot.Get<ButtonWidget>("OBJECTIVES_BUTTON");
				var objectivesWidget = Game.LoadWidget(world, iop.ObjectivesPanel, Ui.Root, new WidgetArgs());
				objectivesWidget.Visible = false;
				objectivesButton.OnClick += () => objectivesWidget.Visible = !objectivesWidget.Visible;
				objectivesButton.IsVisible = () => world.LocalPlayer != null;
			}

			var moneybin = gameRoot.Get("INGAME_MONEY_BIN");
			moneybin.Get<OrderButtonWidget>("SELL").GetKey = _ => Game.Settings.Keys.SellKey;
			moneybin.Get<OrderButtonWidget>("POWER_DOWN").GetKey = _ => Game.Settings.Keys.PowerDownKey;
			moneybin.Get<OrderButtonWidget>("REPAIR").GetKey = _ => Game.Settings.Keys.RepairKey;

			var chatPanel = Game.LoadWidget(world, "CHAT_PANEL", Ui.Root, new WidgetArgs());
			gameRoot.AddChild(chatPanel);

			optionsBG.Get<ButtonWidget>("DISCONNECT").OnClick = () => LeaveGame(optionsBG, world);
			optionsBG.Get<ButtonWidget>("SETTINGS").OnClick = () => Ui.OpenWindow("SETTINGS_MENU");
			optionsBG.Get<ButtonWidget>("MUSIC").OnClick = () => Ui.OpenWindow("MUSIC_MENU");
			optionsBG.Get<ButtonWidget>("RESUME").OnClick = () =>
			{
				optionsBG.Visible = false;
				if (world.LobbyInfo.IsSinglePlayer)
					world.IssueOrder(Order.PauseGame());
			};

			optionsBG.Get<ButtonWidget>("SURRENDER").OnClick = () =>
			{
				optionsBG.Visible = false;
				world.IssueOrder(new Order("Surrender", world.LocalPlayer.PlayerActor, false));
			};

			optionsBG.Get("SURRENDER").IsVisible = () => (world.LocalPlayer != null && world.LocalPlayer.WinState == WinState.Undefined);

			var postgameBG = gameRoot.Get("POSTGAME_BG");
			var postgameText = postgameBG.Get<LabelWidget>("TEXT");
			var postGameObserve = postgameBG.Get<ButtonWidget>("POSTGAME_OBSERVE");

			var postgameQuit = postgameBG.Get<ButtonWidget>("POSTGAME_QUIT");
            postgameQuit.OnClick = () => LeaveGame(postgameQuit, world);

            
            var postgameContinue = postgameBG.Get<ButtonWidget>("POSTGAME_CONTINUE");
            if(!Game.Settings.Campaign.SinglePlayer) postgameContinue.Visible = false;
            postgameContinue.GetText = () =>
            {
                var state = world.LocalPlayer.WinState;
                return state == WinState.Undefined ? "" :
                                (state == WinState.Lost ? "Retry" : "Continue");
            };
            postgameContinue.OnClick = () =>
                {
                    LoadNextLevel(postgameContinue, world, OpenRA.FileFormats.Thirdparty.GammaCruxYamlHelper.getNextMaps());
                };

			postGameObserve.OnClick = () => postgameQuit.Visible = false;
			postGameObserve.IsVisible = () => world.LocalPlayer.WinState != WinState.Won;

			postgameBG.IsVisible = () =>
			{
				return postgameQuit.Visible && world.LocalPlayer != null && world.LocalPlayer.WinState != WinState.Undefined;
			};


			postgameText.GetText = () =>
			{
				var state = world.LocalPlayer.WinState;
				return state == WinState.Undefined ? "" :
								(state == WinState.Lost ? "YOU ARE DEFEATED" : "YOU ARE VICTORIOUS");
			};
		}

		void LeaveGame(Widget pane, World world)
		{
			Sound.PlayNotification(null, "Speech", "Leave", world.LocalPlayer.Country.Race);
			pane.Visible = false;
			Game.Disconnect();
			Game.LoadShellMap();
			Ui.CloseWindow();
			Ui.OpenWindow("MAINMENU_BG");
		}

        void LoadNextLevel(Widget pane, World world,string map)
        {
            Sound.PlayNotification(null, "Speech", "Leave", world.LocalPlayer.Country.Race);
            pane.Visible = false;
            Game.Disconnect();
            Game.LoadShellMap();
            Ui.CloseWindow();

            // Save new settings
            Game.Settings.Server.Name = "Single Player";

            // Begin with Allies 01
            Game.Settings.Server.Map = map;
           
            

            //Auto-set settings
            Game.Settings.Server.ListenPort = 1234;
            Game.Settings.Server.ExternalPort = 1234;
            Game.Settings.Server.AdvertiseOnline = false;
            Game.Settings.Server.AllowUPnP = false;

            Game.Settings.Save();

            // Take a copy so that subsequent changes don't affect the server
            var settings = new ServerSettings(Game.Settings.Server);

            // Create the server
            Game.CreateServer(settings);
            ConnectionLogic.Connect(IPAddress.Loopback.ToString(), Game.Settings.Server.ListenPort,
                () => Game.OpenWindow("SINGLEPLAYER_BG", new WidgetArgs(){}),
                () => { });
           
        }
        
	}
}
