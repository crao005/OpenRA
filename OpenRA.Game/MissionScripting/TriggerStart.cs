using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRA.MissionScripting
{
    /// <summary>
    /// This trigger will fire the first time it is checked at game start, and never after.
    /// </summary>
    public class TriggerStart : Trigger
    {
        private bool canFire = true;
        
        override protected bool ConditionMet()
        {
            if (canFire)
            {
                canFire = false;
                return true;
            }

            return false;
        }
    }
}
