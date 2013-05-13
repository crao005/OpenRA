using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRA.Scripting
{
    abstract class Trigger
    {
        private Action[] Actions;

        abstract public void CheckAndFire();
        
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
