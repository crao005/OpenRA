using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRA.MissionScripting
{
    public class TriggerRepeatOnTime : Trigger
    {
        private int time;
        private int counter;

        // 25 ticks per second.
        // 1500 per minute

        public TriggerRepeatOnTime(int time)
        {
            this.time = time;
            counter = time;
        }

        override protected bool ConditionMet()
        {
            counter--;

            if (counter <= 0)
            {
                counter = time;
                return true;
            }

            return false;
        }
    }
}
