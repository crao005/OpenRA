using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenRA.MissionScripting;
using OpenRA.Traits;

// This class is the logic script for the custom mission level
namespace OpenRA.Mods.RA.Missions
{
    class GammaCruxMission01Info : TraitInfo<GammaCruxMission01>, Requires<SpawnMapActorsInfo> { }

    class GammaCruxMission01 : IHasObjectives, IWorldLoaded, ITick
    {
        // Displays the mission's objectives for a user to follow
        public event Action<bool> OnObjectivesUpdated = notify => { };

        public IEnumerable<Objective> Objectives { get { return objectives.Values; } }

        // Creates the dictionary object so required resources are quickly categorised and initialised
        Dictionary<int, Objective> objectives = new Dictionary<int, Objective>
		{
			{ KillRefineryID, new Objective(ObjectiveType.Primary, KillRefinery, ObjectiveStatus.InProgress) }
		};

        // Initialises variables used by trigger objects and objective message
        const int KillRefineryID = 0;

        const string KillRefinery = "Destroy the enemy";

        Player allies;

        World world;

        // Scripting fields
        private List<Trigger> triggers;

        public void Tick(Actor self)
        {
            if (allies.WinState != WinState.Undefined) return;

            FireTriggers();
        }

        // Sets up game world settings from this mission's map.yaml file with trigger checks
        // to fire when specified for the Allies units.
        public void WorldLoaded(World w)
        {
            world = w;

            allies = w.Players.Single(p => p.InternalName == "Allies");

            allies.PlayerActor.Trait<PlayerResources>().Cash = 0;

            MissionUtils.PlayMissionMusic();

            triggers = Trigger.LoadTriggers(w);

            FireTriggers();
        }

        // Execute method that loops through and executes the different trigger objects
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
