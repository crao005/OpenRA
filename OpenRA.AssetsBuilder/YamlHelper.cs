using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenRA.FileFormats;

namespace OpenRA.AssetsBuilder
{
    public class YamlHelper
    {
        public SoundSettings Sound = new SoundSettings();
        public Dictionary<string, object> Sections;
        public Manifest Manifest;
        public YamlHelper()
        {
            Manifest = new Manifest(new String[] { "ra" });
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

        public void addSave(String file)
        {
            
            Sections = new Dictionary<string, object>()
			{
				{"Sound", Sound},
			};

            var root = MiniYaml.FromFile(file);
            foreach (var kv in Sections)
                root.Add(new MiniYamlNode(kv.Key,FieldSaver.SaveDifferences(kv.Value,Activator.CreateInstance(kv.Value.GetType()))));

            root.WriteToFile(file);
        }
    }

    public class SoundSettings
    {
        public float SoundVolume = 0.5f;
        public float MusicVolume = 0.5f;
        public float VideoVolume = 0.5f;
        public bool Shuffle = false;
        public bool Repeat = false;
        public bool MapMusic = true;
        public string Engine = "AL";

        //public SoundCashTicks SoundCashTickType = SoundCashTicks.Extreme;
    }
}
