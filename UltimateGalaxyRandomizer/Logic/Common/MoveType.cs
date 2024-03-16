using System.Collections.Generic;
using UltimateGalaxyRandomizer.Logic.Move;

namespace UltimateGalaxyRandomizer.Logic.Common;

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
    public static IEnumerable<Effect> GetCompatibleEffects(this MoveType moveType) => moveType switch
    {
        MoveType.Block => [Effect.Normal, Effect.DefenseBlock],
        MoveType.Save => [Effect.Normal, Effect.Punch, Effect.PunchPlus, Effect.PunchPlusPlus],
        MoveType.Shoot => [Effect.Normal, Effect.LongShoot, Effect.ChainShoot, Effect.ShootBlock],
        MoveType.Dribble => [Effect.Normal],
        MoveType.Skill => [Effect.Normal],
        _ => [Effect.Normal]
    };
}