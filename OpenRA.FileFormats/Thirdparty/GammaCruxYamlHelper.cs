using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRA.FileFormats.Thirdparty
{
    class GammaCruxYamlHelper
    {
        public String[] campaign;
        public GammaCruxYamlHelper()
        {
        }

        /// <summary>
        /// example:
        /// string aa = Thirdparty.GammaCruxYamlHelper.getNextMaps(1);       
        /// </summary>
        /// <param name="num"></param>
        /// <returns>Map Hash Code </returns>
        public static String getNextMaps(int num)
        {
            String[] campaign;
            string[] mods = new string[] {"ra"};
            //There should be a  yaml file named campaign.yamal stored in \mods\ra\gammacrux\

            var yaml = new MiniYaml(null, mods
                .Select(m => MiniYaml.FromFile("mods/" + m + "/campaigns/Allies.yaml"))
                .Aggregate(MiniYaml.MergeLiberal)).NodesDict;

            // TODO: Use fieldloader

            campaign = YamlList(yaml, "Campaign");
            if (num > campaign.Length) return "No Map Found";
            return campaign[num-1];
        }

        static string[] YamlList(Dictionary<string, MiniYaml> yaml, string key)
        {
            if (!yaml.ContainsKey(key))
                return new string[] { };

            return yaml[key].NodesDict.Keys.ToArray();
        }
    }
}
