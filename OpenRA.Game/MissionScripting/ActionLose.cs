using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenRA;
using OpenRA.Traits;

namespace OpenRA.MissionScripting
{
    public class ActionLose:Action 
    {
        private World world;
        private string text;

        public ActionLose(World world, string message)
        {
            this.world = world;
            text = message;
        }

        public void Execute()
        {
            Player player = this.world.Players.Single(p => p.PlayerReference.Playable == true);

            player.WinState = WinState.Lost;
            foreach (var actor in world.Actors.Where(a => a.IsInWorld && a.Owner == player && !a.IsDead()))
            {
                actor.Kill(actor);
            }

            if (text != null)
                Game.AddChatLine(Color.Red, "Mission failed", text);

            Sound.Play("misnlst1.aud");
        }
    }
}
