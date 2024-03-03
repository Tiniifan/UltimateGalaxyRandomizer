using System.Collections.Generic;
using UltimateGalaxyRandomizer.Logic.Move;

namespace UltimateGalaxyRandomizer.Logic.Common
{
    public enum Position
    {
        None = 0,
        Goalkeeper = 1,
        Forward = 2,
        Midfielder = 3,
        Defender = 4
    }

    public static class Positions
    {
        public static Dictionary<Stat, int> GetStatBuffs(this Position position)
        {
            switch (position)
            {
                case Position.Goalkeeper:
                    return new Dictionary<Stat, int>
                    {
                        { Stat.GP, 0 },
                        { Stat.TP, 0 },
                        { Stat.Kick, 0 },
                        { Stat.Dribble, 0 },
                        { Stat.Technique, 0 },
                        { Stat.Block, 25 },
                        { Stat.Speed, 0 },
                        { Stat.Stamina, 0 },
                        { Stat.Catch, 75 },
                        { Stat.Luck, 0 }
                    };
                case Position.Forward:
                    return new Dictionary<Stat, int>
                    {
                        { Stat.GP, 0 },
                        { Stat.TP, 0 },
                        { Stat.Kick, 75 },
                        { Stat.Dribble, 25 },
                        { Stat.Technique, 0 },
                        { Stat.Block, 0 },
                        { Stat.Speed, 0 },
                        { Stat.Stamina, 0 },
                        { Stat.Catch, 0 },
                        { Stat.Luck, 0 }
                    };
                case Position.Midfielder:
                    return new Dictionary<Stat, int>
                    {
                        { Stat.GP, 0 },
                        { Stat.TP, 0 },
                        { Stat.Kick, 10 },
                        { Stat.Dribble, 50 },
                        { Stat.Technique, 0 },
                        { Stat.Block, 40 },
                        { Stat.Speed, 0 },
                        { Stat.Stamina, 0 },
                        { Stat.Catch, 0 },
                        { Stat.Luck, 0 }
                    };
                case Position.Defender:
                    return new Dictionary<Stat, int>
                    {
                        { Stat.GP, 0 },
                        { Stat.TP, 0 },
                        { Stat.Kick, 0 },
                        { Stat.Dribble, 20 },
                        { Stat.Technique, 0 },
                        { Stat.Block, 60 },
                        { Stat.Speed, 0 },
                        { Stat.Stamina, 0 },
                        { Stat.Catch, 20 },
                        { Stat.Luck, 75 }
                    };
                case Position.None:
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

        public static Dictionary<MoveType, int> GetMoveProbabilities(this Position position)
        {
            switch (position)
            {
                case Position.Goalkeeper:
                    return new Dictionary<MoveType, int>
                    {
                        { MoveType.Shoot, 1 },
                        { MoveType.Dribble, 1 },
                        { MoveType.Block, 3 },
                        { MoveType.Save, 75 },
                        { MoveType.Skill, 20 }
                    };
                case Position.Forward:
                    return new Dictionary<MoveType, int>
                    {
                        { MoveType.Shoot, 55 },
                        { MoveType.Dribble, 20 },
                        { MoveType.Block, 5 },
                        { MoveType.Save, 0 },
                        { MoveType.Skill, 20 }
                    };
                case Position.Midfielder:
                    return new Dictionary<MoveType, int>
                    {
                        { MoveType.Shoot, 01 },
                        { MoveType.Dribble, 55 },
                        { MoveType.Block, 10 },
                        { MoveType.Save, 5 },
                        { MoveType.Skill, 20 }
                    };
                case Position.Defender:
                    return new Dictionary<MoveType, int>
                    {
                        { MoveType.Shoot, 5 },
                        { MoveType.Dribble, 10 },
                        { MoveType.Block, 60 },
                        { MoveType.Save, 5 },
                        { MoveType.Skill, 20 }
                    };
                case Position.None:
                default:
                    return new Dictionary<MoveType, int>
                    {
                        { MoveType.Shoot, 0 },
                        { MoveType.Dribble, 0 },
                        { MoveType.Block, 0 },
                        { MoveType.Save, 0 },
                        { MoveType.Skill, 100 }
                    };
            }
        }
        
        public static Dictionary<Position, int> GetAvatarProbability(this Position position)
        {
            var probability = new Dictionary<Position, int>
            {
                { Position.Goalkeeper, 10 },
                { Position.Forward, 10 },
                { Position.Midfielder, 10 },
                { Position.Defender, 10 },
            };

            probability[position] = 70;
            return probability;
        }
    }
}
