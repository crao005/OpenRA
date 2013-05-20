using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenRA.FileFormats;

namespace OpenRA.MissionScripting
{
    public abstract class Trigger
    {
        private List<Action> Actions { get; set; }

        public Trigger()
        {
            Actions = new List<Action>();
        }

        /// <summary>
        /// Add an action to this trigger. The trigger returns itself for concatenated adding.
        /// </summary>
        /// <param name="action">The action to add.</param>
        /// <returns>The trigger itself.</returns>
        public Trigger AddAction(Action action)
        {
            Actions.Add(action);
            return this;
        }

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
                if (action != null)
                {
                    action.Execute();
                }
            }
        }

        /// <summary>
        /// This method will load all of the triggers for a given map file, instantiate them, and return them in an array.
        /// </summary>
        /// <returns>An array of Trigger instances.</returns>
        public static List<Trigger> LoadTriggers(World world)
        {
            //create trigger objects based on map.yaml file and return them in an array. 

            List<Trigger> triggerList = new List<Trigger>();

            var map = MiniYaml.DictFromFile("mods/ra/maps/gamma-crux-deathmatch/map.yaml");
             
            if (map.ContainsKey("Triggers"))
            {
                var triggerYamlNodes = map["Triggers"].NodesDict;
                string[] Start = YamlList(triggerYamlNodes, "Start");
                string[] OnTime = YamlList(triggerYamlNodes, "OnTime");

                var StartChildren = triggerYamlNodes["Start"].NodesDict;
                foreach (var item in StartChildren)
                {
                    var child = item.Value.NodesDict;
                    if (child.ContainsKey("Message"))
                    {
                        Trigger trigger = new TriggerStart();
                        Action message = new ActionMessage(child["Message"].Value);
                        triggerList.Add(trigger.AddAction(message));                       
                    }
                    if (child.ContainsKey("Win"))
                    {
                        Trigger trigger = new TriggerStart();
                        Action win = new ActionWin(world, child["Win"].Value);
                        triggerList.Add(trigger.AddAction(win));
                    }
                }

                var OnTimeChildren = triggerYamlNodes["OnTime"].NodesDict;
                foreach (var item in OnTimeChildren)
                {
                    int time = Convert.ToInt32(item.Value.Value);
                    Trigger trigger = new TriggerOnTime(time);
                    var child = item.Value.NodesDict;
                    if (child.ContainsKey("Message"))
                    {
                        Action message = new ActionMessage(child["Message"].Value);
                        triggerList.Add(trigger.AddAction(message));
                    }
                    if (child.ContainsKey("Win"))
                    {
                        Action win = new ActionWin(world, child["Win"].Value);
                        triggerList.Add(trigger.AddAction(win));
                    }
                }
               
              
            }
            return triggerList;
        }

        static string[] YamlList(Dictionary<string, MiniYaml> yaml, string key)
        {
            if (!yaml.ContainsKey(key))
                return new string[] { };

            return yaml[key].NodesDict.Keys.ToArray();
        }

    }
}
