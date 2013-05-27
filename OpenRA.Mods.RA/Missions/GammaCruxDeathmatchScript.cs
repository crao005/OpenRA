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

    /// <summary>
    /// A simple map script which uses the script information contained in the map.yaml file for this map.
    /// </summary>
    class GammaCruxDeathmatchScript : IHasObjectives, IWorldLoaded, ITick
	{

        // Set up the objectives for the mission. The objectives will appear at the top of the interface.
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

        // List of scripted triggers.
        private List<Trigger> triggers;

        // Every tick, if the player has not won or lose, 
        // check all of the triggers to see if they should fire.
		public void Tick(Actor self)
		{
			if (allies.WinState != WinState.Undefined) return;

            FireTriggers();
		}

        // 
		public void WorldLoaded(World w)
		{
			world = w;

            // The player is set as Allies in the map.yaml file. This field is used in the
            // tick method to ensure the triggers don't keep firing after the game is over.
			allies = w.Players.Single(p => p.InternalName == "Allies");
			// The player starts with no money.
			allies.PlayerActor.Trait<PlayerResources>().Cash = 0;

			MissionUtils.PlayMissionMusic();

            // Load all of the triggers for this map from the map.yaml file.
            triggers = Trigger.LoadTriggers(w);

            // Fire all triggers which will fire upon map start.
            FireTriggers();
		}

        /// <summary>
        /// Fire all triggers which should fire.
        /// </summary>
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
