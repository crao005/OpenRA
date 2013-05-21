using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenRA.Traits;

namespace OpenRA.MissionScripting
{
    public class TriggerAnnihilation : Trigger
    {
        private bool canFire = true;
        private bool annihilated = false;

        private Player team;
        private World world;

        public TriggerAnnihilation(World world, Player player) : base()
        {
            team = player;
            this.world = world;
        }

        override protected bool ConditionMet()
        {
            if (canFire)
            {
                while (!annihilated)
                {
                    foreach (var actor in world.Actors.Where(a => a.IsInWorld && a.Owner == team && !a.Info.Name.Equals("player")))
                    {
                        if (!actor.IsDead())
                        {
                            return false;
                        }
                        
                    }

                    canFire = false;
                    return true;
                }
            }

            return false;
        }
    }
}
