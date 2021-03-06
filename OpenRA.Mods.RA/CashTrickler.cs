﻿#region Copyright & License Information
/*
 * Copyright 2007-2011 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

using OpenRA.Traits;
using OpenRA.Mods.RA.Effects;
using OpenRA.FileFormats;

namespace OpenRA.Mods.RA
{
	[Desc("Let's the object generate cash in a set periodic time.")]
	class CashTricklerInfo : ITraitInfo
	{
		[Desc("Amount of money to give each time.")]
		public readonly int Period = 50;
		[Desc("Number of ticks to wait between giving money.")]
		public readonly int Amount = 15;
		[Desc("Whether to show the cash tick indicators (+$15 rising from actor).")]
		public readonly bool ShowTicks = true;
		[Desc("How long the cash tick indicator should be shown for.")]
		public readonly int TickLifetime = 30;
		[Desc("Pixels/tick upward movement of the cash tick indicator.")]
		public readonly int TickVelocity = 1;

		public object Create (ActorInitializer init) { return new CashTrickler(this); }
	}

	class CashTrickler : ITick, ISync
	{
		[Sync] int ticks;
		CashTricklerInfo Info;
		public CashTrickler(CashTricklerInfo info)
		{
			Info = info;
		}

		public void Tick(Actor self)
		{
			if (--ticks < 0)
			{
				self.Owner.PlayerActor.Trait<PlayerResources>().GiveCash(Info.Amount);
				ticks = Info.Period;
				if (Info.ShowTicks)
					self.World.AddFrameEndTask(w => w.Add(new CashTick(Info.Amount, Info.TickLifetime, Info.TickVelocity, self.CenterLocation, self.Owner.ColorRamp.GetColor(0))));
			}
		}
	}
}
