using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRA.Scripting
{
    abstract class Trigger
    {
        private Action[] Actions;

        /// <summary>
        /// This method will check if the trigger should fire and execute the actions appropriately.
        /// </summary>
        public sealed void CheckAndFire()
        {
            if (ConditionMet())
            {
                Execute();
            }
        }

        /// <summary>
        /// This method must be implemented by all Triggers to return true if the trigger should fire
        /// and false otherwise.
        /// </summary>
        /// <returns>True if trigger should fire. False otherwise.</returns>
        abstract protected bool ConditionMet();
        
        /// <summary>
        /// Call this method when all of the actions associated with this trigger should be executed.
        /// </summary>
        protected sealed void Execute() 
        {
            foreach (Action action in Actions)
            {
                action.Execute();
            }
        }

        /// <summary>
        /// This method will load all of the triggers for a given map file, instantiate them, and return them in an array.
        /// </summary>
        /// <returns>An array of Trigger instances.</returns>
        public static Trigger[] LoadTriggers()
        {
            //create trigger objects based on map.yaml file and return them in an array.
            return new Trigger[1];
        }

    }
}
