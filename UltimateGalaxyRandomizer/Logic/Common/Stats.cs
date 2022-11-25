using System.Collections.Generic;

namespace UltimateGalaxyRandomizer.Logic
{
    public class Stats
    {
        public Dictionary<string, int> Values = new Dictionary<string, int>()
        {
            {"FP", 0 },
            {"TP", 0 },
            {"Kick", 0 },
            {"Dribble", 0 },
            {"Technique", 0 },
            {"Block", 0 },
            {"Speed", 0 },
            {"Stamina", 0 },
            {"Catch", 0 },
            {"Luck", 0 },
        };

        public Stats(int[] stats)
        {
            Values["FP"] = stats[0];
            Values["TP"] = stats[1];
            Values["Kick"] = stats[2];
            Values["Dribble"] = stats[3];
            Values["Technique"] = stats[4];
            Values["Block"] = stats[5];
            Values["Speed"] = stats[6];
            Values["Stamina"] = stats[7];
            Values["Catch"] = stats[8];
            Values["Luck"] = stats[9];
        }
    }
}
