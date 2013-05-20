using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRA.MissionScripting
{
    public class TriggerOnTime : Trigger
    {
        private bool canFire = true;
        private int counter;

        // 25 ticks per second.
        // 1500 per minute

        public TriggerOnTime(int time)
        {
            counter = time;
        }

        override protected bool ConditionMet()
        {

            if (canFire && counter <= 0)
            {
                canFire = false;
                return true;
            }

            counter--;
            
            return false;
        }
    }
}
