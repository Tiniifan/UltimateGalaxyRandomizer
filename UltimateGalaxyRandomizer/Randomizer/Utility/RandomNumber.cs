using System;

namespace UltimateGalaxyRandomizer.Randomizer.Utility
{
    public class RandomNumber : Random
    {
        public readonly int Seed;

        public RandomNumber()
        {
            Seed = new Random().Next();
        }

        public RandomNumber(int seed) : base(seed)
        {
            Seed = seed;
        }
    }
}
