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
using System.Collections.Generic;
using System.Linq;
using OpenRA.FileFormats;
using OpenRA.Mods.RA.Activities;
using OpenRA.Mods.RA.Air;
using OpenRA.Mods.RA.Move;
using OpenRA.Scripting;
using OpenRA.Traits;

namespace OpenRA.Mods.RA.Missions
{
    class GammaCruxDeathmatchScriptInfo : TraitInfo<GammaCruxDeathmatchScript>, Requires<SpawnMapActorsInfo> { }

    class GammaCruxDeathmatchScript : IHasObjectives, IWorldLoaded, ITick
	{
		public event Action<bool> OnObjectivesUpdated = notify => { };

		public IEnumerable<Objective> Objectives { get { return objectives.Values; } }

		Dictionary<int, Objective> objectives = new Dictionary<int, Objective>
		{
			{ KillJeepID, new Objective(ObjectiveType.Primary, KillJeep, ObjectiveStatus.InProgress) }
		};

		const int KillJeepID = 0;

		const string KillJeep = "Kill the enemy jeep!";

		Player allies;
		Player soviets;

		Actor jeep;
		
		World world;

		void MissionAccomplished(string text)
		{
			MissionUtils.CoopMissionAccomplished(world, text, allies);
		}

		void MissionFailed(string text)
		{
			MissionUtils.CoopMissionFailed(world, text, allies);
		}

		public void Tick(Actor self)
		{
			if (allies.WinState != WinState.Undefined) return;

			if (objectives[KillJeepID].Status == ObjectiveStatus.InProgress)
			{
                if (AlliesKilledJeep())
                {
                    objectives[KillJeepID].Status = ObjectiveStatus.Completed;
                    OnObjectivesUpdated(true);
                    MissionAccomplished("You killed the enemy jeep!");
                }
			}

			if (jeep != null && jeep.Destroyed)
				MissionFailed("Your jeep was destroyed.");
		}


        bool AlliesKilledJeep()
		{
			// Return true if the Soviet Jeep is dead.

            return false;// MissionUtils.AreaSecuredWithUnits(world, allies, lab.CenterLocation, LabClearRange);
		}
		
		public void WorldLoaded(World w)
		{
			world = w;

			allies = w.Players.Single(p => p.InternalName == "Allies");
			soviets = w.Players.Single(p => p.InternalName == "Soviets");

			allies.PlayerActor.Trait<PlayerResources>().Cash = 0;

			var actors = w.WorldActor.Trait<SpawnMapActors>().Actors;
			jeep = actors["Jeep"];
			
			MissionUtils.PlayMissionMusic();
		}
	}
}
