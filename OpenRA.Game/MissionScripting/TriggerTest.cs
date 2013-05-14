using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRA.MissionScripting
{
    class TriggerTest : Trigger
    {
        bool once = true;
        
        protected bool ConditionMet()
        {
            if (once)
            {
                once = false;
                return true;
            }

            return false;
        }
    }
}
