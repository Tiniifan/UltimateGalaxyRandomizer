using System.Linq;
using System.Collections.Generic;

namespace UltimateGalaxyRandomizer.Randomizer.Utility
{
	public class Probability
	{
		private List<double> Pool { get; set; }

		public Probability(int[] itemsProbability)
		{
			Pool = itemsProbability.Select(x => x / 100.0).ToList();
		}

		private double Next()
		{
			double u = Pool.Sum(p => p);
			double r = Randomizer.Seed.NextDouble() * u;

			double sum = 0;
			foreach (double n in Pool)
			{
				if (r <= (sum = sum + n))
				{
					return n;
				}
			}

			return 0.0;
		}

		public int GetRandomIndex()
        {
			return Pool.IndexOf(Next());
        }
	}
}