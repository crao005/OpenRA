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

        protected Trigger()
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


        // Will add a list of actions to a trigger to allow multiple actions of different type under a heading
        public Trigger AddAllActions(List<Action> actions)
        {
            foreach (var action in actions)
            {
                Actions.Add(action);
            }
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
            // Create list of all triggers which will be associated with a game
            var triggerList = new List<Trigger>();


            // Load the custom map file
            var map = MiniYaml.DictFromFile("mods/ra/maps/gamma-crux-deathmatch/map.yaml");
             
            if (map.ContainsKey("Triggers"))
            {

                // Get all trigger types (at the first level in the yaml file)
                var triggerYamlNodes = map["Triggers"].NodesDict;


                // Get all triggers of type start - will fire immediately
                var startChildren = triggerYamlNodes["Start"].NodesDict;
                foreach (var item in startChildren)
                {
                    // Get the actions under that trigger
                    var child = item.Value.NodesDict;

                    // Create appropriate trigger for the found string in the yaml file
                    Trigger trigger = new TriggerStart();

                    // Iterate over the actions, assign them to Action objects
                    // Link them to their parent trigger and assign that trigger to the global list
                    triggerList.Add(trigger.AddAllActions(GetActions(child, world)));                     
                }

                // ontime trigger - fires at certain time
                var onTimeChildren = triggerYamlNodes["OnTime"].NodesDict;
                foreach (var item in onTimeChildren)
                {
                    var child = item.Value.NodesDict;

                    // OnTime triggers must have a time variable so that they know the time to fire at
                    int time = Convert.ToInt32(item.Value.Value);
                    Trigger trigger = new TriggerOnTime(time);
                    
                    triggerList.Add(trigger.AddAllActions(GetActions(child, world)));                    
                }

                // ontime trigger - fires at certain time
                var repeatOnTimeChildren = triggerYamlNodes["RepeatOnTime"].NodesDict;
                foreach (var item in repeatOnTimeChildren)
                {
                    var child = item.Value.NodesDict;

                    // OnTime triggers must have a time variable so that they know the time to fire at
                    int time = Convert.ToInt32(item.Value.Value);
                    Trigger trigger = new TriggerRepeatOnTime(time);

                    triggerList.Add(trigger.AddAllActions(GetActions(child, world)));
                }

                // Annihilation trigger - will fire when the given team has no buildings or units remaining
                var annihilationChildren = triggerYamlNodes["Annihilation"].NodesDict;
                foreach (var item in annihilationChildren)
                {
                    var child = item.Value.NodesDict;

                    // Get the team specified in yaml 
                    Player team = world.Players.Single(p => p.InternalName == item.Value.Value); // Get the team from the yaml 
                    Trigger trigger = new TriggerAnnihilation(world, team);
                    
                    triggerList.Add(trigger.AddAllActions(GetActions(child, world)));
                }

                // Unit In Area trigger - will fire when a given team has a unit in a given box area
                var unitInAreaChildren = triggerYamlNodes["UnitInArea"].NodesDict;
                foreach (var item in unitInAreaChildren)
                {
                    var child = item.Value.NodesDict;

                    Trigger trigger = new TriggerUnitInArea(world, item.Value.Value);

                    triggerList.Add(trigger.AddAllActions(GetActions(child, world)));
                }
            }
            return triggerList;
        }

        // Each trigger may have any one of these actions beneath it. 
        // This function will check the yaml string and return the correct object inside a list
        // The desired behaviour is one action per named trigger but this will allow for multiple,
        //  provided they are of different types (ie their names are unique at that level)
        private static List<Action> GetActions(Dictionary<string, MiniYaml> child, World world)
        {
            var actions = new List<Action>();
            if (child.ContainsKey("Message"))
            {
                actions.Add(new ActionMessage(child["Message"].Value));
            }
            if (child.ContainsKey("Win"))
            {
                actions.Add(new ActionWin(world, child["Win"].Value));
            }
            if (child.ContainsKey("Lose"))
            {
                actions.Add(new ActionLose(world, child["Lose"].Value));
            }
			if (child.ContainsKey("SpawnUnit"))
            {
                actions.Add(new ActionSpawnUnit(world, child["SpawnUnit"].Value));
            }
            return actions;
        }


        
        /// <summary>t
        /// This static method is used for LoadTriggers to Transform Yaml Dictionary format to string array 
        /// </summary>
        /// <param name="yaml"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        static string[] YamlList(Dictionary<string, MiniYaml> yaml, string key)
        {
            if (!yaml.ContainsKey(key))
                return new string[] { };

            return yaml[key].NodesDict.Keys.ToArray();
        }

    }
}
