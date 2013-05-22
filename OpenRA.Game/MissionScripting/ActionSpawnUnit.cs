using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenRA;
using OpenRA.FileFormats;

namespace OpenRA.MissionScripting
{
    public class ActionSpawnUnit:Action 
    {
        private World world;
        private string type;
        private Player owner;
        private CPos? location;
        int? facing;

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
            var td = new TypeDictionary { new OwnerInit(owner) };
            if (location.HasValue)
                td.Add(new LocationInit(location.Value));
            if (facing.HasValue)
                td.Add(new FacingInit(facing.Value));
            world.CreateActor(true, type, td);
        }
    }
}
