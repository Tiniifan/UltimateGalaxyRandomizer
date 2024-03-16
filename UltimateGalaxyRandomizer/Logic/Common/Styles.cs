using System.Collections.Generic;

namespace UltimateGalaxyRandomizer.Logic.Common
{
    public static class Styles
    {
        public static readonly IReadOnlyDictionary<byte, string> Values = new Dictionary<byte, string>()
        {
            {0, "All" },
            {1, "Lively" },
            {2, "Fun" },
            {3, "Fierce" },
            {4, "Cool" },
            {5, "Cute" },
        };
    }
}
