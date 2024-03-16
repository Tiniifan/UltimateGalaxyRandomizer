using System.Collections.Generic;

namespace UltimateGalaxyRandomizer.Logic.Common
{
    public class Invoke(bool canInvoke, bool isFightingSpirit, bool canArmorify, bool isLocked, bool isStory)
    {
        public bool CanInvoke { get; set; } = canInvoke;

        public bool IsFightingSpirit { get; set; } = isFightingSpirit;

        public bool CanArmorify { get; set; } = canArmorify;

        public bool IsLocked { get; set; } = isLocked;

        public bool IsStory { get; set; } = isStory;
    }

    public static class Invokes
    {
        public static byte ChangeArmorify(this byte invoke, bool canArmorify) => (byte)(canArmorify ? invoke | 0b00001000 : invoke & 0b11110111);

        public static readonly IReadOnlyDictionary<short, Invoke> Values = new Dictionary<short, Invoke>
        {
            //8 bit --- from right
            // ?
            // ?
            // can invoke
            // can armorify
            // locked
            // 0
            // is totem
            // 0
            {0b00000000, new Invoke(false, false, false, false, false) },
            {0b00000001, new Invoke(false, false, false, false, false) },
            {0b00000010, new Invoke(false, false, false, false, false) },
            {0b00000011, new Invoke(false, false, false, false, false) },
            {0b00000100, new Invoke(true, true, false, false, false) },
            {0b00001100, new Invoke(true, true, true, false, false) },
            {0b00001101, new Invoke(true, true, true, false, false) },
            {0b00011100, new Invoke(true, true, true, true, true) },
            {0b00010100, new Invoke(true, true, false, true, true) },
            {0b00100000, new Invoke(false, false, false, false, false) },
            {0b00101100, new Invoke(true, true, true, false, false) },
            {0b01000100, new Invoke(true, false, false, false, false) },
            {0b00010000, new Invoke(false, false, false, true, true) },
        };
    }
}
