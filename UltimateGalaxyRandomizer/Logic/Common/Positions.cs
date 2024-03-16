using System.Collections.Generic;
using System.Linq;
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
        public static Dictionary<Stat, int> GetStatBuffs(this Position position) => position switch
        {
            Position.Goalkeeper => new Dictionary<Stat, int>
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
            },
            Position.Forward => new Dictionary<Stat, int>
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
            },
            Position.Midfielder => new Dictionary<Stat, int>
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
            },
            Position.Defender => new Dictionary<Stat, int>
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
            },
            Position.None => new Dictionary<Stat, int>
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
            },
            _ => new Dictionary<Stat, int>
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
            }
        };

        public static Dictionary<MoveType, int> GetMoveProbabilities(this Position position) => position switch
        {
            Position.Goalkeeper => new Dictionary<MoveType, int>
            {
                { MoveType.Shoot, 0 },
                { MoveType.Dribble, 0 },
                { MoveType.Block, 0 },
                { MoveType.Save, 85 },
                { MoveType.Skill, 15 }
            },
            Position.Forward => new Dictionary<MoveType, int>
            {
                { MoveType.Shoot, 65 },
                { MoveType.Dribble, 14 },
                { MoveType.Block, 1 },
                { MoveType.Save, 0 },
                { MoveType.Skill, 20 }
            },
            Position.Midfielder => new Dictionary<MoveType, int>
            {
                { MoveType.Shoot, 12 },
                { MoveType.Dribble, 63 },
                { MoveType.Block, 5 },
                { MoveType.Save, 0 },
                { MoveType.Skill, 20 }
            },
            Position.Defender => new Dictionary<MoveType, int>
            {
                { MoveType.Shoot, 5 },
                { MoveType.Dribble, 10 },
                { MoveType.Block, 65 },
                { MoveType.Save, 0 },
                { MoveType.Skill, 20 }
            },
            Position.None => new Dictionary<MoveType, int>
            {
                { MoveType.Shoot, 0 },
                { MoveType.Dribble, 0 },
                { MoveType.Block, 0 },
                { MoveType.Save, 0 },
                { MoveType.Skill, 100 }
            },
            _ => new Dictionary<MoveType, int>
            {
                { MoveType.Shoot, 0 },
                { MoveType.Dribble, 0 },
                { MoveType.Block, 0 },
                { MoveType.Save, 0 },
                { MoveType.Skill, 100 }
            }
        };

        public static Dictionary<Effect, int> GetEffectProbabilities(this Position position, MoveType moveType)
        {
            return moveType.GetCompatibleEffects().ToDictionary(e => e, GetAffinity);

            int GetAffinity(Effect effect) => (effect, position) switch
            {
                (Effect.LongShoot, Position.Midfielder) => 5,
                (Effect.ShootBlock, Position.Defender) => 60,
                _ => 1
            };
        }
    }
}
