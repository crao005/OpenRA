using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRA.MissionScripting
{
    /// <summary>
    /// This trigger will fire once at a specified time.
    /// </summary>
    public class TriggerOnTime : Trigger
    {
        private bool canFire = true;
        private int counter;

        // 25 ticks per second.
        // 1500 ticks per minute.

        public TriggerOnTime(int time)
        {
            counter = time;
        }

        override protected bool ConditionMet()
        {
            counter--;

            if (canFire && counter <= 0)
            {
                canFire = false;
                return true;
            }

            return false;
        }
    }
}
