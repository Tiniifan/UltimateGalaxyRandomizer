using System.Collections.Generic;
using System.Linq;

namespace UltimateGalaxyRandomizer.Randomizer.Utility
{
	public static class Probability
    {
        public static readonly System.Random Generator = new System.Random();
        
        public static int RandomIndex<T>(this IEnumerable<T> enumerable) => Generator.Next(0, enumerable.Count());
        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            var list = enumerable.ToList();
            return list[list.RandomIndex()];
        }

        public static bool FromPercentage(int value) => Generator.Next(0, 100) < value;

        public static int IndexFromProbabilities(params int[] probabilities)
        {
            var total = probabilities.Sum();
            var random = Generator.Next(0, total);
            for (var i = 0; i < probabilities.Length; i++)
            {
                if (random < probabilities[i]) return i;
                random -= probabilities[i];
            }

            return probabilities.Length - 1;
        }

        public static int RandomAsProbabilities(this int[] probabilities) => IndexFromProbabilities(probabilities);
    }
}