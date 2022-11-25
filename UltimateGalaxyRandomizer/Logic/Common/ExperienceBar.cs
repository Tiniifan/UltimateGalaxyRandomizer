using System.Collections.Generic;

namespace UltimateGalaxyRandomizer.Logic
{
    public class ExperienceBar
    {
        // Experience bar 1 -> player level  up faster
        // Experience bar 7 -> player level up slower
        public static Dictionary<byte, string> Values = new Dictionary<byte, string>()
        {
            {1, "Experience Bar 4" },
            {2, "Experience Bar 3" },
            {3, "Experience Bar 1" },
            {4, "Experience Bar 2" },
            {5, "Experience Bar 6" },
            {6, "Experience Bar 7" },
            {7, "Experience Bar 5" },
        };
    }
}
