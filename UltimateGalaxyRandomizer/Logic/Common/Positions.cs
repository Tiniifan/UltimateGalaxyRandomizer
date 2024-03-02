using System.Collections.Generic;

namespace UltimateGalaxyRandomizer.Logic.Common
{
    public class Position
    {
        public string Name { get; set; }

        public int[] MoveProbability { get; set; }

        public int[] StatBuff { get; set; }

        public Position(string name, int[] moveProbability, int[] statBuff)
        {
            Name = name;

            // Order Shoot -> Dribble -> Block -> Save -> Skill
            MoveProbability = moveProbability;

            // Order GP -> TP -> Kick -> Dribble -> Technique -> Block -> Speed -> Stamina ->  Catch -> Luck
            StatBuff = statBuff;
        }
    }

    public static class Positions
    {
        public static readonly IReadOnlyDictionary<int, Position> Player = new Dictionary<int, Position>()
        {
            {0, new Position(
                " ", 
                new int[5] {100, 0, 0, 0, 0 },
                new int[10] {0, 0, 0, 0, 0, 0, 0, 0, 100, 0 })},
            {1, new Position(
                "Goalkeeper", 
                new int[5] {1, 1, 3, 75, 20 },
                new int[10] {0, 0, 0, 0, 0, 25, 0, 0, 75, 0 })},
            {2, new Position(
                "Forward", 
                new int[5] {55, 20, 5, 0, 20 },
                new int[10] {0, 0, 75, 25, 0, 0, 0, 0, 0, 0 })},
            {3, new Position(
                "Midfielder", 
                new int[5] {10, 55, 10, 5, 20 },
                new int[10] {0, 0, 10, 50, 0, 40, 0, 0, 0, 0 })},
            {4, new Position(
                "Defender", 
                new int[5] {5, 10, 60, 5, 20 },
                new int[10] {0, 0, 0, 20, 0, 60, 0, 0, 20, 0 })},
        };

        public static readonly IReadOnlyDictionary<int, string> Technique = new Dictionary<int, string>()
        {
            {0, "None" },
            {1, "Shoot" },
            {2, "Dribble" },
            {3, "Block" },
            {4, "Save" },
        };
    }
}
