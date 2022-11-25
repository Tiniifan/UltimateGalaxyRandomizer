using System.Collections.Generic;

namespace UltimateGalaxyRandomizer.Logic
{
    public class Effect
    {
        public string Name { get; set; }

        public byte Position { get; set; }

        public Effect(string name)
        {
            Name = name;
        }

        public Effect(string name, byte position)
        {
            Name = name;
            Position = position;
        }
    }

    public class Effects
    {
        public static Dictionary<byte, Effect> Values = new Dictionary<byte, Effect>()
        {
            {0x00, new Effect("Normal") },
            {0x02, new Effect("Punch", 0x04) },
            {0x03, new Effect("Punch +", 0x04) },
            {0x04, new Effect("Long Shot", 0x01) },
            {0x05, new Effect("Chain", 0x01) },
            {0x06, new Effect("Block Shot", 0x01) },
            {0x07, new Effect("Defense Block", 0x03) },
            {0x08, new Effect("Punch ++", 0x04) },
        };
    }
}
