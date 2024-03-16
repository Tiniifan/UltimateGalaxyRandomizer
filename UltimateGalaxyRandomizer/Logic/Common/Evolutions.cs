using System.Collections.Generic;

namespace UltimateGalaxyRandomizer.Logic.Common;

public static class Evolutions
{
    public static readonly IReadOnlyDictionary<byte, string> Technique = new Dictionary<byte, string>
    {
        {0x01, "超" },
        {0x02, "絶" },
        {0x03, "極" },
        {0x04, "A" },
        {0x05, "S" },
        {0x06, "Z" },
        {0x07, "GX" },
    };

    public static readonly IReadOnlyDictionary<byte, string> FightingSpirit = new Dictionary<byte, string>
    {
        {0x01, "" },
        {0x02, "2" },
        {0x03, "3" },
        {0x04, "4" },
        {0x05, "5" },
        {0x06, "Ω" },
    };

    public static readonly IReadOnlyDictionary<byte, string> Totem = new Dictionary<byte, string>
    {
        {0x01, "Level 1" },
        {0x02, "Level 2" },
        {0x03, "Level 3" },
        {0x04, "Level 4" },
        {0x05, "Level 5" },
    };
}