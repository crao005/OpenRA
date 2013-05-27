using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenRA.Traits;

namespace OpenRA.MissionScripting
{
    /// <summary>
    /// This trigger will fire once the first time a given team has no more units or buildings.
    /// </summary>
    public class TriggerAnnihilation : Trigger
    {
        private bool canFire = true;

        private Player team;
        private World world;

        public TriggerAnnihilation(World world, Player player) : base()
        {
            team = player;
            this.world = world;
        }

        override protected bool ConditionMet()
        {
            // If this team is out of the game then ignore this trigger.
            if (team.WinState != WinState.Undefined)
            {
                canFire = false;
            }

            if (canFire)
            {
                // For each actor in the world, owned by the given team, which is not a player itself (is a unit or building)
                foreach (var actor in world.Actors.Where(a => a.IsInWorld && a.Owner == team && !a.Info.Name.Equals("player")))
                {
                    if (!actor.IsDead())
                    {
                        // A living actor was found, the team is not annihilated.
                        return false;
                    }
                        
                }

                // No living actors on that team were found, fire the trigger.
                canFire = false;
                return true;
            }

            // The trigger has already fired.
            return false;
        }
    }
}
