using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenRA;
using OpenRA.FileFormats;
using OpenRA.Traits;

namespace OpenRA.MissionScripting
{
    public class ActionKillUnit:Action 
    {
        private World world;
        private Actor actor;

        public ActionKillUnit(World world, string args)
        {
            string[] argArray = args.Split(',');

            actor = world.Actors.Single(a => a.IsInWorld && !a.IsDead() && a.Info.Name == argArray[0]);
        }

        public void Execute()
        {
            if (actor.IsInWorld && !actor.IsDead())
            {
                actor.Kill(actor);
            }
        }
    }
}
