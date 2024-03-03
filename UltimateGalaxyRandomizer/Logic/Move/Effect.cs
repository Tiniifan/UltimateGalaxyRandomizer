using System.Collections.Generic;

namespace UltimateGalaxyRandomizer.Logic.Move
{
    public class Effect
    {
        public string Name { get; set; }

        public MoveType Position { get; set; }

        public Effect(string name)
        {
            Name = name;
        }

        public Effect(string name, MoveType position)
        {
            Name = name;
            Position = position;
        }
    }

    public static class Effects
    {
        public static IReadOnlyDictionary<byte, Effect> Values { get; } = new Dictionary<byte, Effect>()
        {
            {0x00, new Effect("Normal") },
            {0x02, new Effect("Punch", MoveType.Save) },
            {0x03, new Effect("Punch +", MoveType.Save) },
            {0x04, new Effect("Long Shot", MoveType.Shoot) },
            {0x05, new Effect("Chain", MoveType.Shoot) },
            {0x06, new Effect("Block Shot", MoveType.Shoot) },
            {0x07, new Effect("Defense Block", MoveType.Block) },
            {0x08, new Effect("Punch ++", MoveType.Save) },
        };
    }
}
