using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenRA;
using OpenRA.FileFormats;

namespace OpenRA.MissionScripting
{
    /// <summary>
    /// This action will spawn a unit onto the map.
    /// The unit is defined by: type, owner and x and y location.
    /// </summary>
    public class ActionSpawnUnit:Action 
    {
        private World world;
        private string type;
        private Player owner;
        private CPos? location;
        int? facing; // Used to specify the direction the unit is facing. Could be added as another input argument.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="world"></param>
        /// <param name="args">Specify the unit: [type] [owner] [x] [y]</param>
        public ActionSpawnUnit(World world, string args)
        {
            string[] argArray = args.Split(',');
            this.world = world;
            this.type = argArray[0];
            this.owner = world.Players.Single(p => p.InternalName == argArray[1]);
            this.location = new CPos(int.Parse(argArray[2]), int.Parse(argArray[3]));
            this.facing = null;
        }

        public void Execute()
        {
            // Create a type dictionary to hold the unit owner.
            var td = new TypeDictionary { new OwnerInit(owner) };
            // Error checking for the location and facing direction.
            if (location.HasValue)
                td.Add(new LocationInit(location.Value));
            if (facing.HasValue)
                td.Add(new FacingInit(facing.Value));
            // Create the actor.
            world.CreateActor(true, type, td);
        }
    }
}
