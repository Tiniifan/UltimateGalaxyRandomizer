using System.Collections.Generic;

namespace UltimateGalaxyRandomizer.Logic.Common
{
    public enum Gender
    {
        Boy = 1,
        Girl = 2,
        Undefined = 3
    }
    public enum Year
    {
        Unknown = 0,
        First = 1,
        Second = 2,
        Third = 3,
        Adult = 4,
        Child = 5
    }

    public static class Genders
    {
        public static Dictionary<Stat, int> GetStatBuffs(this Gender gender)
        {
            // Order GP -> TP -> Kick -> Dribble -> Technique -> Block -> Speed -> Stamina ->  Catch -> Luck
            switch (gender)
            {
                case Gender.Boy:
                    return new Dictionary<Stat, int>
                    {
                        { Stat.GP, 75 },
                        { Stat.TP, 25 },
                        { Stat.Kick, 0 },
                        { Stat.Dribble, 0 },
                        { Stat.Technique, 0 },
                        { Stat.Block, 0 },
                        { Stat.Speed, 0 },
                        { Stat.Stamina, 0 },
                        { Stat.Catch, 0 },
                        { Stat.Luck, 0 }
                    };
                case Gender.Girl:
                    return new Dictionary<Stat, int>
                    {
                        { Stat.GP, 25 },
                        { Stat.TP, 75 },
                        { Stat.Kick, 0 },
                        { Stat.Dribble,0 },
                        { Stat.Technique, 0 },
                        { Stat.Block, 0 },
                        { Stat.Speed, 0 },
                        { Stat.Stamina, 0 },
                        { Stat.Catch, 0 },
                        { Stat.Luck, 0 }
                    };
                case Gender.Undefined:
                default:
                    return new Dictionary<Stat, int>
                    {
                        { Stat.GP, 50 },
                        { Stat.TP, 50 },
                        { Stat.Kick, 0 },
                        { Stat.Dribble, 0 },
                        { Stat.Technique, 0 },
                        { Stat.Block, 0 },
                        { Stat.Speed, 0 },
                        { Stat.Stamina, 0 },
                        { Stat.Catch, 0 },
                        { Stat.Luck, 0 }
                    };
            }
        }
    }
}
