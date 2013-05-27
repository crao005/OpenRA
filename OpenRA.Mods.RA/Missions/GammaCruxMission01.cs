using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenRA.MissionScripting;
using OpenRA.Traits;

namespace OpenRA.Mods.RA.Missions
{
    class GammaCruxMission01Info : TraitInfo<GammaCruxMission01>, Requires<SpawnMapActorsInfo> { }

    class GammaCruxMission01 : IHasObjectives, IWorldLoaded, ITick
    {
        public event Action<bool> OnObjectivesUpdated = notify => { };

        public IEnumerable<Objective> Objectives { get { return objectives.Values; } }

        Dictionary<int, Objective> objectives = new Dictionary<int, Objective>
		{
			{ KillRefineryID, new Objective(ObjectiveType.Primary, KillRefinery, ObjectiveStatus.InProgress) }
		};

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

        public void WorldLoaded(World w)
        {
            world = w;

            allies = w.Players.Single(p => p.InternalName == "Allies");

            allies.PlayerActor.Trait<PlayerResources>().Cash = 0;

            MissionUtils.PlayMissionMusic();

            triggers = Trigger.LoadTriggers(w);

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
