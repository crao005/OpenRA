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
    public class ContinueLogic
    {
        OrderManager orderManager;
        System.Threading.Timer timer;

        [ObjectCreator.UseCtor]
        public ContinueLogic(OrderManager orderManager)
        {
            this.orderManager = orderManager;

            /*
             * Allies 01: bf46386b1c8e1618088d3c495d5beb93cac461f6
             * Allies 02: e0624a4ba15d728c02f62566523c0279cc938fe2
             */

            timer = new System.Threading.Timer(obj => { StartGame(); }, null, 200, System.Threading.Timeout.Infinite);

        }

        public void OpenLastGame()
        {
            orderManager.IssueOrder(Order.Command("continue"));
        }
    }
}
