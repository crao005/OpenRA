using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenRA.FileFormats;

namespace OpenRA.AssetsBuilder
{
    public class YamlHelper
    {
        public Dictionary<string, object> Sections;
        public Manifest Manifest;
        public YamlHelper()
        {
            Manifest = new Manifest(new String[] { "ra" });
        }


       
        public List<String> getSequences()
        {
            List<String> results =new List<string>();
            
            List<MiniYamlNode> sequences = MiniYaml.FromFile(Manifest.Sequences[0]);
            foreach (var item in sequences)
            {
                results.Add(item.Key.ToString());
                Console.WriteLine(item.Key.ToString());
                var childNode = item.Value;
                Console.WriteLine(childNode.Nodes.Count);
            }
            return results;
        }

        public bool addSequencesYaml(String name) 
        {
            var valuse1 = new MiniYaml("0");
            var valuse2 = new MiniYaml("16");

            var Node1 = new MiniYamlNode("Start", valuse1);
            var Node2 = new MiniYamlNode("Facings", valuse2);
            
            var NoteList = new List<MiniYamlNode>();
            NoteList.Add(Node1);
            NoteList.Add(Node2);

            var ChildYaml = new MiniYaml(null, NoteList);
            var ChildNode = new MiniYamlNode("idle", ChildYaml);

            var RootYaml = new MiniYaml(null,new List<MiniYamlNode>(){ChildNode});
            var RootNote = new MiniYamlNode(name.ToLower(),RootYaml);

            List<MiniYamlNode> sequences = MiniYaml.FromFile(Manifest.Sequences[0]);
            sequences.Add(RootNote);
            sequences.WriteToFile(Manifest.Sequences[0]);

            return true;
        }

        //    Rules:
        //mods/ra/rules/defaults.yaml
        //mods/ra/rules/system.yaml
        //mods/ra/rules/vehicles.yaml
        //mods/ra/rules/structures.yaml
        //mods/ra/rules/infantry.yaml
        //mods/ra/rules/civilian.yaml
        //mods/ra/rules/trees.yaml
        //mods/ra/rules/aircraft.yaml
        //mods/ra/rules/ships.yaml
        public bool addShips(String name) 
        {
            var BuildableNodesList = new List<MiniYamlNode>()
            {
                new MiniYamlNode("Queue",new MiniYaml("Ship")),
                new MiniYamlNode("BuildPaletteOrder",new MiniYaml("50")),
                new MiniYamlNode("Prerequisites",new MiniYaml("syrd")),
                new MiniYamlNode("BuiltAt",new MiniYaml("syrd")),
                new MiniYamlNode("Owner",new MiniYaml("allies")),
                new MiniYamlNode("Hotkey",new MiniYaml("j"))
            };
            var BuildableNodes = new MiniYaml(null,BuildableNodesList);

            var ValuedNodesList = new List<MiniYamlNode>()
            {
                new MiniYamlNode("Cost",new MiniYaml("500"))
            };
            var ValuedNodes = new MiniYaml(null,ValuedNodesList);

             var TooltipNodesList = new List<MiniYamlNode>()
            {
                new MiniYamlNode("Name",new MiniYaml("Gunboat")),
                 new MiniYamlNode("Description: ",new MiniYaml("Light scout & support ship. \n  Strong vs Ships, Submarines\n  Weak vs Aircraft"))
            };
            var TooltipNodes = new MiniYaml(null,TooltipNodesList);

            var HealthNodesList = new List<MiniYamlNode>()
            {
                new MiniYamlNode("HP",new MiniYaml("200"))
            };
            var HealthNodes = new MiniYaml(null,HealthNodesList);

            var ArmorNodesList = new List<MiniYamlNode>()
            {
                new MiniYamlNode("Type",new MiniYaml("Heavy"))
            };
            var ArmorNodes = new MiniYaml(null,ArmorNodesList);

            var MobileNodesList = new List<MiniYamlNode>()
            {
                new MiniYamlNode("ROT",new MiniYaml("7")),
                new MiniYamlNode("Speed",new MiniYaml("9"))
            };
            var MobileNodes = new MiniYaml(null,MobileNodesList);

            var RevealsShroudNodesList = new List<MiniYamlNode>()
            {
                new MiniYamlNode("Range",new MiniYaml("7"))
            };
            var RevealsShroudNodes = new MiniYaml(null,RevealsShroudNodesList);

            var Node1 = new MiniYamlNode("Inherits", new MiniYaml("^Ship"));
            var Node2 = new MiniYamlNode("Buildable",BuildableNodes);
            var Node3 = new MiniYamlNode("Valued", ValuedNodes);
            var Node4 = new MiniYamlNode("Tooltip",TooltipNodes);
            var Node5 = new MiniYamlNode("Health",HealthNodes);
            var Node6 = new MiniYamlNode("Armor", ArmorNodes);
            var Node7 = new MiniYamlNode("Mobile", MobileNodes);
            var Node8 = new MiniYamlNode("RevealsShroud", RevealsShroudNodes);

            //var Node9 = new MiniYamlNode("-TargetableUnit", );
            //var Node10 = new MiniYamlNode("TargetableSubmarine", );
            //var Node11 = new MiniYamlNode("Cloak", );
            //var Node12 = new MiniYamlNode("Armament", );
            //var Node13 = new MiniYamlNode("AttackFrontal", );
            //var Node14 = new MiniYamlNode("Chronoshiftable", );
            //var Node15 = new MiniYamlNode("IronCurtainable", );
            //var Node16 = new MiniYamlNode("RepairableNear", );
            //var Node17 = new MiniYamlNode("-DetectCloaked:", );
            //var Node18 = new MiniYamlNode("AutoTarget", );
            //var Node19 = new MiniYamlNode("DebugRetiliateAgainstAggressor", );
            //var Node20 = new MiniYamlNode("DebugNextAutoTargetScanTime", );
            //var Node21 = new MiniYamlNode("AttackMove", );
            
            
            var NoteList = new List<MiniYamlNode>();
            NoteList.Add(Node1);
            NoteList.Add(Node2);
            NoteList.Add(Node3);
            NoteList.Add(Node4);
            NoteList.Add(Node5);
            NoteList.Add(Node6);
            NoteList.Add(Node7);
            NoteList.Add(Node8);
            //NoteList.Add(Node9);
            //NoteList.Add(Node10);
            //NoteList.Add(Node11);
            //NoteList.Add(Node12);
            //NoteList.Add(Node13);
            //NoteList.Add(Node14);
            //NoteList.Add(Node15);
            //NoteList.Add(Node16);
            //NoteList.Add(Node17);
            //NoteList.Add(Node18);
            //NoteList.Add(Node19);
            //NoteList.Add(Node20);
            //NoteList.Add(Node21);

            var RootYaml = new MiniYaml(null, NoteList);          
            var RootNote = new MiniYamlNode(name.ToUpper(), RootYaml);

            List<MiniYamlNode> sequences = MiniYaml.FromFile(Manifest.Rules[8]);
            sequences.Add(RootNote);
            sequences.WriteToFile(Manifest.Rules[8]);

            return true;
        }
        public List<String> getShips()
        {
            List<String> results = new List<string>();

            List<MiniYamlNode> sequences = MiniYaml.FromFile(Manifest.Rules[8]);
            foreach (var item in sequences)
            {
                results.Add(item.Key.ToString());
                Console.WriteLine(item.Key.ToString());
                var childNode = item.Value;
                Console.WriteLine(childNode.Nodes.Count);
            }
            return results;
        }

        public bool addVehicles(string name)
        {
            var BuildableNodesList = new List<MiniYamlNode>()
            {
                new MiniYamlNode("Queue",new MiniYaml("Ship")),
                new MiniYamlNode("BuildPaletteOrder",new MiniYaml("50")),
                new MiniYamlNode("Prerequisites",new MiniYaml("syrd")),
                new MiniYamlNode("BuiltAt",new MiniYaml("syrd")),
                new MiniYamlNode("Owner",new MiniYaml("allies")),
                new MiniYamlNode("Hotkey",new MiniYaml("j"))
            };
            var BuildableNodes = new MiniYaml(null, BuildableNodesList);

            var ValuedNodesList = new List<MiniYamlNode>()
            {
                new MiniYamlNode("Cost",new MiniYaml("500"))
            };
            var ValuedNodes = new MiniYaml(null, ValuedNodesList);

            var TooltipNodesList = new List<MiniYamlNode>()
            {
                new MiniYamlNode("Name",new MiniYaml("Gunboat")),
                 new MiniYamlNode("Description: ",new MiniYaml("Light scout & support ship. \n  Strong vs Ships, Submarines\n  Weak vs Aircraft"))
            };
            var TooltipNodes = new MiniYaml(null, TooltipNodesList);

            var HealthNodesList = new List<MiniYamlNode>()
            {
                new MiniYamlNode("HP",new MiniYaml("200"))
            };
            var HealthNodes = new MiniYaml(null, HealthNodesList);

            var ArmorNodesList = new List<MiniYamlNode>()
            {
                new MiniYamlNode("Type",new MiniYaml("Heavy"))
            };
            var ArmorNodes = new MiniYaml(null, ArmorNodesList);

            var MobileNodesList = new List<MiniYamlNode>()
            {
                new MiniYamlNode("ROT",new MiniYaml("7")),
                new MiniYamlNode("Speed",new MiniYaml("9"))
            };
            var MobileNodes = new MiniYaml(null, MobileNodesList);

            var RevealsShroudNodesList = new List<MiniYamlNode>()
            {
                new MiniYamlNode("Range",new MiniYaml("7"))
            };
            var RevealsShroudNodes = new MiniYaml(null, RevealsShroudNodesList);

            var Node1 = new MiniYamlNode("Inherits", new MiniYaml("^Ship"));
            var Node2 = new MiniYamlNode("Buildable", BuildableNodes);
            var Node3 = new MiniYamlNode("Valued", ValuedNodes);
            var Node4 = new MiniYamlNode("Tooltip", TooltipNodes);
            var Node5 = new MiniYamlNode("Health", HealthNodes);
            var Node6 = new MiniYamlNode("Armor", ArmorNodes);
            var Node7 = new MiniYamlNode("Mobile", MobileNodes);
            var Node8 = new MiniYamlNode("RevealsShroud", RevealsShroudNodes);

            //var Node9 = new MiniYamlNode("-TargetableUnit", );
            //var Node10 = new MiniYamlNode("TargetableSubmarine", );
            //var Node11 = new MiniYamlNode("Cloak", );
            //var Node12 = new MiniYamlNode("Armament", );
            //var Node13 = new MiniYamlNode("AttackFrontal", );
            //var Node14 = new MiniYamlNode("Chronoshiftable", );
            //var Node15 = new MiniYamlNode("IronCurtainable", );
            //var Node16 = new MiniYamlNode("RepairableNear", );
            //var Node17 = new MiniYamlNode("-DetectCloaked:", );
            //var Node18 = new MiniYamlNode("AutoTarget", );
            //var Node19 = new MiniYamlNode("DebugRetiliateAgainstAggressor", );
            //var Node20 = new MiniYamlNode("DebugNextAutoTargetScanTime", );
            //var Node21 = new MiniYamlNode("AttackMove", );


            var NoteList = new List<MiniYamlNode>();
            NoteList.Add(Node1);
            NoteList.Add(Node2);
            NoteList.Add(Node3);
            NoteList.Add(Node4);
            NoteList.Add(Node5);
            NoteList.Add(Node6);
            NoteList.Add(Node7);
            NoteList.Add(Node8);
            //NoteList.Add(Node9);
            //NoteList.Add(Node10);
            //NoteList.Add(Node11);
            //NoteList.Add(Node12);
            //NoteList.Add(Node13);
            //NoteList.Add(Node14);
            //NoteList.Add(Node15);
            //NoteList.Add(Node16);
            //NoteList.Add(Node17);
            //NoteList.Add(Node18);
            //NoteList.Add(Node19);
            //NoteList.Add(Node20);
            //NoteList.Add(Node21);

            var RootYaml = new MiniYaml(null, NoteList);
            var RootNote = new MiniYamlNode(name.ToUpper(), RootYaml);

            List<MiniYamlNode> sequences = MiniYaml.FromFile(Manifest.Rules[2]);
            sequences.Add(RootNote);
            sequences.WriteToFile(Manifest.Rules[2]);

            return true;
        }

        public List<String> getVehicles()
        {
            List<String> results = new List<string>();

            List<MiniYamlNode> sequences = MiniYaml.FromFile(Manifest.Rules[2]);
            foreach (var item in sequences)
            {
                results.Add(item.Key.ToString());
                Console.WriteLine(item.Key.ToString());
                var childNode = item.Value;
                Console.WriteLine(childNode.Nodes.Count);
            }
            return results;
        }





        public void addSave(String file)
        {
            var valuse = new MiniYaml("222");
            var Note1 = new MiniYamlNode("aaa", valuse);
            var Note2 = new MiniYamlNode("bbb", valuse);
            var NoteList = new List<MiniYamlNode>();
            NoteList.Add(Note1);
            NoteList.Add(Note2);

            var Root = new MiniYamlNode("AAA", null, NoteList);


            var root = MiniYaml.FromFile(file);            
            root.Add(Root);           
            root.WriteToFile(file);
        }
        public void view()
        {

            var aa = Manifest.Sequences;

            List<MiniYamlNode> y = new List<MiniYamlNode>();

            List<MiniYamlNode> sequences = MiniYaml.FromFile(aa[0]);
            List<MiniYamlNode> setting = MiniYaml.FromFile(Platform.SupportDir + "settings.yaml");

            foreach (var item in sequences)
            {
                Console.WriteLine(item.Key.ToString());
                var childNode = item.Value;
                Console.WriteLine(childNode.Nodes.Count);
            }

            foreach (var item in setting)
            {
                Console.WriteLine(item.Key.ToString());
                var childNode = item.Value;
                Console.WriteLine(childNode.Nodes.Count);
            }
        }
    }
}
