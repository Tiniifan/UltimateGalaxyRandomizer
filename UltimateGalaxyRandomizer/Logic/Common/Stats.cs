using System.Collections.Generic;

namespace UltimateGalaxyRandomizer.Logic.Common
{
    
    public enum Stat
    {
        GP,
        TP,
        Kick,
        Dribble,
        Technique,
        Block,
        Speed,
        Stamina,
        Catch,
        Luck
    }

    public class Stats
    {
        public Dictionary<Stat, int> Values { get; set; } = new Dictionary<Stat, int>
        {
            { Stat.GP, 0 },
            { Stat.TP, 0 },
            { Stat.Kick, 0 },
            { Stat.Dribble, 0 },
            { Stat.Technique, 0 },
            { Stat.Block, 0 },
            { Stat.Speed, 0 },
            { Stat.Stamina, 0 },
            { Stat.Catch, 0 },
            { Stat.Luck, 0 },
        };

        public Stats(int[] stats)
        {
            Values[Stat.GP] = stats[0];
            Values[Stat.TP] = stats[1];
            Values[Stat.Kick] = stats[2];
            Values[Stat.Dribble] = stats[3];
            Values[Stat.Technique] = stats[4];
            Values[Stat.Block] = stats[5];
            Values[Stat.Speed] = stats[6];
            Values[Stat.Stamina] = stats[7];
            Values[Stat.Catch] = stats[8];
            Values[Stat.Luck] = stats[9];
        }
    }
}
