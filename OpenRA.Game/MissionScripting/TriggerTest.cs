using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRA.MissionScripting
{
    class TriggerTest : Trigger
    {
        bool canFire = true;
        
        protected bool ConditionMet()
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
