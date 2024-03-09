using System.Collections.Generic;
using UltimateGalaxyRandomizer.Logic.Move;

namespace UltimateGalaxyRandomizer.Logic.Common
{
    public enum MoveType
    {
        Shoot = 1,
        Dribble = 2,
        Block = 3,
        Save = 4,
        Skill = 15
    }

    public static class MoveTypes
    {
        public static IEnumerable<Effect> GetCompatibleEffects(this MoveType moveType)
        {
            switch (moveType)
            {
                case MoveType.Block: return new[] { Effect.Normal, Effect.DefenseBlock };
                case MoveType.Save: return new[] { Effect.Normal, Effect.Punch, Effect.PunchPlus, Effect.PunchPlusPlus };
                case MoveType.Shoot: return new[] { Effect.Normal, Effect.LongShoot, Effect.ChainShoot, Effect.ShootBlock };
                case MoveType.Dribble:
                case MoveType.Skill:
                default: return new[] { Effect.Normal };
            }
        }
    }
}