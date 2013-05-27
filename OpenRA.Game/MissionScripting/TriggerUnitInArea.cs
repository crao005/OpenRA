using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenRA.FileFormats;
using OpenRA.Network;
using OpenRA.Traits;

namespace OpenRA.MissionScripting
{
    /// <summary>
    /// This trigger will fire the first time a specified team has a unit in a specified box area.
    /// </summary>
    public class TriggerUnitInArea : Trigger
    {
        private bool canFire = true;

        private World world;
        private Player player;

        // a and b are the pixel positions defining the top left and bottom right corners of the box area.
        PPos a;
        PPos b;

        /// <summary>
        /// Create a unit in area trigger with the world object for the match and the information for
        /// what team can fire the trigger, and the x and y coordinates of two points which define
        /// the top left and bottom right corners of the box area the unit can be in.
        /// </summary>
        /// <param name="world">The world object for this match</param>
        /// <param name="args">A string in the format: [teamname] [x1] [y1] [x2] [y2]</param>
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
