using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace OpenRA.FileFormats.Thirdparty
{
    public class GammaCruxYamlHelper
    {
        public String[] campaign;
        public GammaCruxYamlHelper()
        {
        }

        /// <summary>
        /// example:
        /// string aa = Thirdparty.GammaCruxYamlHelper.getMap(1);       
        /// </summary>
        /// <param name="num"></param>
        /// <returns>Map Hash Code </returns>
        public static String getMap(int num)
        {
            String[] campaign;
           
            //There should be a  yaml file named campaign.yaml stored in \mods\ra\gammacrux\
            //var yaml = new MiniYaml(null, mods
            //    .Select(m => MiniYaml.FromFile("mods/" + m + "/campaigns/Allies.yaml"))
            //    .Aggregate(MiniYaml.MergeLiberal)).NodesDict;

            var yaml = MiniYaml.DictFromFile("mods/ra/campaigns/Allies.yaml");
          
            //string pathstart = "mods\ra\maps";

            campaign = YamlList(yaml, "Campaign");
            if (num > campaign.Length) return "No Map Found";
            //return campaign[num-1];
            return GetHash(@"mods\ra\maps\"+campaign[num-1]);
        }

        /// <summary>
        /// string aa =  Thirdparty.GammaCruxYamlHelper.getNextMaps();
        /// </summary>
        /// <returns></returns>
        public static String getNextMaps()
        {
            String[] campaign;
            int num = 1;

            var yaml = MiniYaml.DictFromFile("mods/ra/campaigns/Allies.yaml");
            campaign = YamlList(yaml, "Campaign");

            var setting = MiniYaml.DictFromFile(Platform.SupportDir + "settings.yaml");

            if (setting.ContainsKey("Campaign"))
            {
                var settingCampaign = setting["Campaign"].NodesDict;
                if (settingCampaign.ContainsKey("NextMission"))
                {
                    num = Convert.ToInt32(settingCampaign["NextMission"].Value);
                }
            }
            
            if (num > campaign.Length) return "No Map Found";
            return campaign[num - 1];
        }

        static string[] YamlList(Dictionary<string, MiniYaml> yaml, string key)
        {
            if (!yaml.ContainsKey(key))
                return new string[] { };

            return yaml[key].NodesDict.Keys.ToArray();
        }


        static string GetHash(string path)
        {
            IFolder Container = FileSystem.OpenPackage(path, int.MaxValue);

            // UID is calculated by taking an SHA1 of the yaml and binary data
            // Read the relevant data into a buffer
            var data = Container.GetContent("map.yaml").ReadAllBytes()
                .Concat(Container.GetContent("map.bin").ReadAllBytes()).ToArray();

            // Take the SHA1
            using (var csp = SHA1.Create())
                return new string(csp.ComputeHash(data).SelectMany(a => a.ToString("x2")).ToArray());
        }
    }
}
