using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenRA;

namespace OpenRA.MissionScripting
{
    public class ActionWin:Action 
    {
        private World world;
        private string text;

        public ActionWin(World world, string message)
        {
            this.world = world;
            text = message;
        }

        public void Execute()
        {
            
            Player player = this.world.Players.Single(p => p.PlayerReference.Playable == true);

            player.WinState = WinState.Won;

            // Save single player progress
            if (Game.Settings.Campaign.SinglePlayer)
            {
                Game.Settings.Campaign.NextMission++;
                Game.Settings.Save();
            }

            if (text != null)
                Game.AddChatLine(Color.Blue, "Mission accomplished", text);

            Sound.Play("misnwon1.aud");
        }
    }
}
