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
using System.Drawing;
using OpenRA.MissionScripting;

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
		
		World world;

        // Scripting fields
        private List<Trigger> triggers;

		public void Tick(Actor self)
		{
			if (allies.WinState != WinState.Undefined) return;

            FireTriggers();
		}

		public void WorldLoaded(World w)
		{
			world = w;
            String yamlFile="mods/ra/maps/gamma-crux-deathmatch/map.yaml";

			allies = w.Players.Single(p => p.InternalName == "Allies");
			
			allies.PlayerActor.Trait<PlayerResources>().Cash = 0;

			MissionUtils.PlayMissionMusic();

            triggers = Trigger.LoadTriggers(w, yamlFile);

            FireTriggers();
		}

        private void FireTriggers()
        {
            foreach (Trigger trigger in triggers)
            {
                if (trigger != null)
                {
                    trigger.CheckAndFire();
                }
            }
        }
	}
}
