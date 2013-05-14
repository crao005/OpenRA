using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRA.MissionScripting
{
    public class TriggerStart : Trigger
    {
        bool canFire = true;
        
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
