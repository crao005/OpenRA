#region Copyright & License Information
/*
 * Copyright 2007-2012 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

using System;
using System.Net;
using OpenRA.GameRules;
using OpenRA.Widgets;
using OpenRA.Network;

namespace OpenRA.Mods.RA.Widgets.Logic
{
	public class SinglePlayerLogic
	{
		Widget panel;
        OrderManager orderManager;
		Action onStart;
		Action onExit;
        Map map;
		

		[ObjectCreator.UseCtor]
        public SinglePlayerLogic(Widget widget, OrderManager orderManager, Action onExit, Action onStart)
		{
			panel = widget;
            this.orderManager = orderManager;
            this.onStart = onStart;
			this.onExit = onExit;

			var settings = Game.Settings;
			panel.Get<ButtonWidget>("BACK_BUTTON").OnClick = () => { Ui.CloseWindow(); onExit(); };
			panel.Get<ButtonWidget>("START_BUTTON").OnClick = CreateAndJoin;

            /*
			var mapButton = panel.GetOrNull<ButtonWidget>("MAP_BUTTON");
			if (mapButton != null)
			{
				panel.Get<ButtonWidget>("MAP_BUTTON").OnClick = () =>
				{
					Ui.OpenWindow("MAPCHOOSER_PANEL", new WidgetArgs()
					{
						{ "initialMap", map.Uid },
						{ "onExit", () => {} },
						{ "onSelect", (Action<Map>)(m => map = m) }
					});
				};

				panel.Get<MapPreviewWidget>("MAP_PREVIEW").Map = () => map;
				panel.Get<LabelWidget>("MAP_NAME").GetText = () => map.Title;
			}
            */
            panel.Get<TextFieldWidget>("PLAYER_NAME").Text = "Name";
		}

		void CreateAndJoin()
		{
            /*
            Game.Settings.Server.Name = panel.Get<TextFieldWidget>("PLAYER_NAME").Text;
            Game.Settings.Server.Map = map.Uid;

            Game.Settings.Save();
            */
            orderManager.IssueOrder(Order.Command("startgame"));
		}
	}
}
