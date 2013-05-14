using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenRA.MissionScripting
{
    public class ActionMessage:Action 
    {
        public Color colour;                //This colour only affects playerName
        public String playerName;
        public String message;

        public ActionMessage(Color colour, String playerName, String message)
        {
            this.colour = colour;
            this.playerName = playerName;
            this.message = message;
        }
        
        public void Execute()
        {
            //Game.addChatLine(c,n,s) was used to display the message.
            Game.AddChatLine(colour, playerName, message);
        }
    }
}
