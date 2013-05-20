using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenRA.MissionScripting
{
    public class ActionMessage:Action 
    {
        public Color colour;
        public String playerName;
        public String message;

        //Default. Used for only message displays.
        public ActionMessage(String message)
        {
            this.colour = Color.White;
            this.playerName = "";
            this.message = message;
        }

        //Overloaded to accept all inputs. Used for perhaps dialogue messages.
        public ActionMessage(Color colour, String playerName, String message)
        {
            this.colour = colour;
            this.playerName = playerName;
            this.message = message;
        }
        
        public void Execute()
        {
            Game.AddChatLine(colour, playerName, message);
        }
    }
}
