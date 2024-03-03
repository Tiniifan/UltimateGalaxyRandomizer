using System;
using System.Linq;
using System.Collections.Generic;
using UltimateGalaxyRandomizer.Logic.Common;
using UltimateGalaxyRandomizer.Resources;
using UltimateGalaxyRandomizer.Randomizer.Utility;

namespace UltimateGalaxyRandomizer.Logic
{
    public class Player
    {
        public string Name { get; set; }

        public Param Param { get; set; }

        public Base Base { get; set; }

        public SkillTable[] Skills { get; set; }

        public Player(string name)
        {
            Name = name;
        }

        public Player Clone() => new Player(Name)
        {
            Param = Param.Clone(),
            Base = Base.Clone(),
            Skills = Skills.Select(x => x.Clone()).ToArray()
        };

        public int[] GetPositionProbability() => Positions.Player[Param.Position].MoveProbability;
        public int[] GetElementProbability()
        {
            int[] elementProbability = new int[5] { 15, 15, 15, 15, 15 };

            if (Param.Element == 0)
            {
                elementProbability[0] = 40;
            }
            else
            {
                elementProbability[Param.Element - 1] = 40;
            }

            return elementProbability;
        }

        public Dictionary<uint, Move> GetRandomMoveset(int count)
        {
            int skillCount = 0;

            var moveset = new Dictionary<uint, Move>();

            for (int s = 0; s < count; s++)
            {
                Dictionary<uint, Move> possibleMoves;

                // Get a random Move Type according to player position probability
                int movePosition = GetPositionProbability().RandomAsProbabilities();
#pragma warning disable S2583
                if (skillCount > 2)
#pragma warning restore S2583
                {
                    // Avoids having more than 2 skills
                    while (movePosition == 4)
                    {
                        movePosition = GetPositionProbability().RandomAsProbabilities();
                    }
                }

                // Check if the Move Type is a skill
                if (movePosition < 4)
                {
                    // Create a list of moves according to player position probability
                    possibleMoves = Moves.PlayerMoves.Where(x => !moveset.ContainsKey(x.Key) && x.Value.Position == movePosition + 1).ToDictionary(x => x.Key, x => x.Value);

                    // Create a list of moves according to player element probability
                    int moveElement = GetElementProbability().RandomAsProbabilities();
                    possibleMoves = possibleMoves.Where(x => !moveset.ContainsKey(x.Key) && x.Value.Element == moveElement + 1).ToDictionary(x => x.Key, x => x.Value);
                }
                else
                {
                    possibleMoves = Moves.PlayerMoves.Where(x => !moveset.ContainsKey(x.Key) && x.Value.IsSkill).ToDictionary(x => x.Key, x => x.Value);
                    skillCount += 1;
                }

                // Only in an extreme case
                if (possibleMoves.Count == 0)
                {
                    possibleMoves = Moves.PlayerMoves.Where(x => !moveset.ContainsKey(x.Key) && x.Value.Position == movePosition + 1).ToDictionary(x => x.Key, x => x.Value);
                }

                var move = possibleMoves.Random();
                moveset.Add(move.Key, move.Value);
            }

            return moveset;
        }

        public uint GetRandomFightingSpirit()
        {
            int movePosition = GetPositionProbability().RandomAsProbabilities() + 1;
            int moveElement = GetElementProbability().RandomAsProbabilities();
            var possibleAvatars = Avatars.FightingSpirits
                .Where(x => x.Value.Position == movePosition)
                .Where(x => x.Value.Element == moveElement + 1)
                .ToDictionary(x => x.Key, x => x.Value);

            // Only in an extreme case
            if (!possibleAvatars.Any()) possibleAvatars = Avatars.FightingSpirits.ToDictionary(x => x.Key, x => x.Value);

            return possibleAvatars.Random().Key;
        }

        public uint GetRandomTotem()
        {
            int movePosition = GetPositionProbability().RandomAsProbabilities() + 1;
            int moveElement = GetElementProbability().RandomAsProbabilities();
            var possibleTotems = Avatars.Totems
                .Where(x => x.Value.Position == movePosition)
                .Where(x => x.Value.Element == moveElement + 1)
                .ToDictionary(pair => pair.Key, x => x.Value);

            // Only in an extreme case
            if (!possibleTotems.Any()) possibleTotems = Avatars.Totems.ToDictionary(x => x.Key, x => x.Value);

            return possibleTotems.Random().Key;
        }
    }
}
