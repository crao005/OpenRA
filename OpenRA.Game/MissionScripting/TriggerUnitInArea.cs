using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenRA.FileFormats;
using OpenRA.Network;
using OpenRA.Traits;

namespace OpenRA.MissionScripting
{
    public class TriggerUnitInArea : Trigger
    {
        private bool canFire = true;

        private World world;
        private Player player;
        PPos a;
        PPos b;

        public TriggerUnitInArea(World world, String args)
            : base()
        {
            string[] argArray = args.Split(',');
            this.world = world;
            this.player = world.Players.Single(p => p.InternalName == argArray[0]);
            this.a = new PPos(int.Parse(argArray[1]), int.Parse(argArray[2]));
            this.b = new PPos(int.Parse(argArray[3]), int.Parse(argArray[4]));
        }

        override protected bool ConditionMet()
        {
            if (canFire)
            {
                if (world.FindUnits(a, b).Where(u => u.IsInWorld && u != world.WorldActor && !u.IsDead() && u.Owner.Equals(player)).Any())
                {
                    canFire = false;
                    return true;
                }
            }

            return false;
        }
    }
}
