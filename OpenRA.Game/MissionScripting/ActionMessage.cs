using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenRA.MissionScripting
{
    public class ActionMessage:Action 
    {
        public String message;

        public ActionMessage(Color colour, String message)
        {
            this.message = message;
        }
        
        public void Execute()
        {
            //Game.addChatLine(c,n,s) was used to display the message. Color and playerName string are
            //constant so that scripters only need to type their message.
            Game.AddChatLine(Color.White, "", message);
        }
    }
}
