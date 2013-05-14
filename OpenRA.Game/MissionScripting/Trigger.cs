using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenRA.FileFormats;

namespace OpenRA.MissionScripting
{
    public abstract class Trigger
    {
        private Action[] Actions;

        /// <summary>
        /// This method will check if the trigger should fire and execute the actions appropriately.
        /// </summary>
        public void CheckAndFire()
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
        protected void Execute()
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

            var map = MiniYaml.DictFromFile("mods/ra/maps/gamma-crux-deathmatch/map.yaml");
             
            if (map.ContainsKey("Triggers"))
            {
                var traggerYamlNodes = map["Triggers"].NodesDict;
                string[] Start = YamlList(traggerYamlNodes, "Start");
                string[] OnTime = YamlList(traggerYamlNodes, "OnTime");

                foreach (var item in Start)
                    Console.WriteLine(item.ToString());
                foreach (var item in OnTime)
                    Console.WriteLine(item.ToString());

                var StartChildren = traggerYamlNodes["Start"].NodesDict;
                string[] Messages = YamlList(traggerYamlNodes, "Message");
                foreach (var item in Messages)
                    Console.WriteLine(item.ToString());

                var OnTimeChildren = traggerYamlNodes["OnTime"].NodesDict;
                Messages = YamlList(traggerYamlNodes, "Message");
                foreach (var item in Messages)
                    Console.WriteLine(item.ToString());

                return new Trigger[1];
            }
            return null;
        }

        static string[] YamlList(Dictionary<string, MiniYaml> yaml, string key)
        {
            if (!yaml.ContainsKey(key))
                return new string[] { };

            return yaml[key].NodesDict.Keys.ToArray();
        }

    }
}
