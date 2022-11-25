using System.Collections.Generic;

namespace UltimateGalaxyRandomizer.Logic
{
    public class Element
    {
        public string Name { get; set; }

        public int[] StatBuff { get; set; }

        public Element(string name, int[] statBuff)
        {
            Name = name;

            // Order GP -> TP -> Kick -> Dribble -> Technique -> Block -> Speed -> Stamina ->  Catch -> Luck
            StatBuff = statBuff;
        }
    }

    public class Elements
    {
        public static Dictionary<int, Element> Values = new Dictionary<int, Element>()
        {
            {0, new Element(
                "None",
                new int[10] {0, 0, 0, 0, 0, 0, 0, 0, 100, 0 })},
            {1, new Element(
                "Wind",
                new int[10] {0, 0, 0, 0, 0, 0, 75, 25, 0, 0 })},
            {2, new Element(
                "Wood",
                new int[10] {0, 0, 0, 0, 75, 0, 0, 0, 0, 25 })},
            {3, new Element(
                "Fire",
                new int[10] {0, 0, 0, 0, 0, 0, 25, 75, 0, 0 })},
            {4, new Element(
                "Earth",
                new int[10] {0, 0, 0, 0, 25, 0, 0, 0, 0, 75 })},
            {5, new Element(
                "Void",
                new int[10] {0, 0, 0, 0, 25, 0, 25, 25, 0, 25 })},
        };
    }
}
