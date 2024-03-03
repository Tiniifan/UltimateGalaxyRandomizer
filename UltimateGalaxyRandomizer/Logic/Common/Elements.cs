using System.Collections.Generic;

namespace UltimateGalaxyRandomizer.Logic.Common
{
    public enum Element
    {
        None = 0,
        Wind = 1,
        Wood = 2,
        Fire = 3,
        Earth = 4,
        Void = 5
    }

    public static class Elements
    {
        public static Dictionary<Stat, int> GetStatBuffs(this Element element)
        {
            // Order GP -> TP -> Kick -> Dribble -> Technique -> Block -> Speed -> Stamina ->  Catch -> Luck
            switch (element)
            {
                case Element.Wind:
                    return new Dictionary<Stat, int>
                    {
                        { Stat.GP, 0 },
                        { Stat.TP, 0 },
                        { Stat.Kick, 0 },
                        { Stat.Dribble, 0 },
                        { Stat.Technique, 0 },
                        { Stat.Block, 0 },
                        { Stat.Speed, 75 },
                        { Stat.Stamina, 25 },
                        { Stat.Catch, 0 },
                        { Stat.Luck, 0 }
                    };
                case Element.Wood:
                    return new Dictionary<Stat, int>
                    {
                        { Stat.GP, 0 },
                        { Stat.TP, 0 },
                        { Stat.Kick, 0 },
                        { Stat.Dribble, 0 },
                        { Stat.Technique, 75 },
                        { Stat.Block, 0 },
                        { Stat.Speed, 0 },
                        { Stat.Stamina, 0 },
                        { Stat.Catch, 0 },
                        { Stat.Luck, 25 }
                    };
                case Element.Fire:
                    return new Dictionary<Stat, int>
                    {
                        { Stat.GP, 0 },
                        { Stat.TP, 0 },
                        { Stat.Kick, 0 },
                        { Stat.Dribble, 0 },
                        { Stat.Technique, 0 },
                        { Stat.Block, 0 },
                        { Stat.Speed, 25 },
                        { Stat.Stamina, 75 },
                        { Stat.Catch, 0 },
                        { Stat.Luck, 0 }
                    };
                case Element.Earth:
                    return new Dictionary<Stat, int>
                    {
                        { Stat.GP, 0 },
                        { Stat.TP, 0 },
                        { Stat.Kick, 0 },
                        { Stat.Dribble, 0 },
                        { Stat.Technique, 25 },
                        { Stat.Block, 0 },
                        { Stat.Speed, 0 },
                        { Stat.Stamina, 0 },
                        { Stat.Catch, 0 },
                        { Stat.Luck, 75 }
                    };
                case Element.Void:
                    return new Dictionary<Stat, int>
                    {
                        { Stat.GP, 0 },
                        { Stat.TP, 0 },
                        { Stat.Kick, 0 },
                        { Stat.Dribble, 0 },
                        { Stat.Technique, 25 },
                        { Stat.Block, 0 },
                        { Stat.Speed, 25 },
                        { Stat.Stamina, 25 },
                        { Stat.Catch, 0 },
                        { Stat.Luck, 25 }
                    };
                case Element.None:
                default:
                    return new Dictionary<Stat, int>
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
                        { Stat.Luck, 0 }
                    };
            }
        }
        
        public static Dictionary<Element, int> GetElementProbability(this Element element)
        {
            var elementProbability = new Dictionary<Element, int>
            {
                { Element.None, 15 },
                { Element.Wind, 15 },
                { Element.Wood, 15 },
                { Element.Fire, 15 },
                { Element.Earth, 15 },
                { Element.Void, 15 },
            };

            elementProbability[element] = 40;
            return elementProbability;
        }
    }
}
