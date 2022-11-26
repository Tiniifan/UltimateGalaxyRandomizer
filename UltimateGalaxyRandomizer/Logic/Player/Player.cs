using System;
using System.Linq;
using System.Collections.Generic;
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

        public Player Clone()
        {
            Player cloned = new Player(Name);

            cloned.Param = Param.Clone();
            cloned.Base = Base.Clone();
            cloned.Skills = Skills.Select(x => x.Clone()).ToArray();

            return cloned;
        }

        public Probability GetPositionProbability()
        {
            return new Probability(Positions.Player[Param.Position].MoveProbability);
        }

        public Probability GetElementProbability()
        {
            int[] elementProbability = new int[5] { 15, 15, 15, 15, 15 };

            if (Param.Element == 0)
            {
                elementProbability[0] = 40;
            }
            else
            {
                elementProbability[Param.Element-1] = 40;
            }

            return new Probability(elementProbability);
        }

        public List<UInt32> GetRandomMoveset(int count)
        {
            Probability samePosition = GetPositionProbability();
            Probability sameElement = GetPositionProbability();

            int skillCount = 0;

            List<UInt32> moveset = new List<UInt32>();

            for (int s = 0; s < count; s++)
            {
                Dictionary<UInt32, Move> possibleMoves = new Dictionary<UInt32, Move>();

                // Get a random Move Type according to player position probability
                int movePosition = samePosition.GetRandomIndex();
                if (skillCount > 2)
                {
                    // Avoids having more than 2 skills
                    while (movePosition == 4)
                    {
                        movePosition = samePosition.GetRandomIndex();
                    }
                }

                // Check If the Move Type is a skill;
                if (movePosition < 4)
                {
                    // Create a list of moves according to player position probability
                    possibleMoves = Moves.PlayerMoves.Where(x => moveset.IndexOf(x.Key) == -1 && x.Value.Position == movePosition + 1).ToDictionary(x => x.Key, x => x.Value);

                    // Create a list of moves according to player element probability
                    int moveElement = sameElement.GetRandomIndex();
                    possibleMoves = possibleMoves.Where(x => moveset.IndexOf(x.Key) == -1 && x.Value.Element == moveElement + 1).ToDictionary(x => x.Key, x => x.Value);
                }
                else
                {
                    possibleMoves = Moves.PlayerMoves.Where(x => moveset.IndexOf(x.Key) == -1 && x.Value.Position == 15).ToDictionary(x => x.Key, x => x.Value);
                    skillCount += 1;
                }

                // Only in an extreme case
                if (possibleMoves.Count == 0)
                {
                    possibleMoves = Moves.PlayerMoves.Where(x => moveset.IndexOf(x.Key) == -1 && x.Value.Position == movePosition + 1).ToDictionary(x => x.Key, x => x.Value);
                }

                moveset.Add(possibleMoves.ElementAt(Randomizer.Randomizer.Seed.Next(0, possibleMoves.Count)).Key);
            }

            return moveset;
        }

        public UInt32 GetRandomFightingSpirit()
        {
            Probability samePlayerPosition = GetPositionProbability();
            Probability samePlayerElement = GetElementProbability();

            Dictionary<UInt32, FightingSpirit> possibleAvatars = new Dictionary<UInt32, FightingSpirit>();

            // Create a list of Avatars according to the position probability
            int movePosition = samePlayerPosition.GetRandomIndex() + 1;
            possibleAvatars = Avatars.FightingSpirits.Where(x => x.Value.Position == movePosition).ToDictionary(x => x.Key, x => x.Value);

            // Create a list of moves according to the element probability
            int moveElement = samePlayerElement.GetRandomIndex();
            possibleAvatars = possibleAvatars.Where(x => x.Value.Element == moveElement + 1).ToDictionary(x => x.Key, x => x.Value);

            // Only in an extreme case
            if (possibleAvatars.Count == 0)
            {
                possibleAvatars = Avatars.FightingSpirits;
            }

            return possibleAvatars.ElementAt(Randomizer.Randomizer.Seed.Next(0, possibleAvatars.Count)).Key;
        }

        public UInt32 GetRandomTotem()
        {
            Probability samePlayerPosition = GetPositionProbability();
            Probability samePlayerElement = GetElementProbability();

            Dictionary<UInt32, Totem> possibleTotems = new Dictionary<UInt32, Totem>();

            // Create a list of Avatars according to the position probability
            int movePosition = samePlayerPosition.GetRandomIndex() + 1;
            possibleTotems = Avatars.Totems.Where(x => x.Value.Position == movePosition).ToDictionary(x => x.Key, x => x.Value);

            // Create a list of moves according to the element probability
            int moveElement = samePlayerElement.GetRandomIndex();
            possibleTotems = possibleTotems.Where(x => x.Value.Element == moveElement + 1).ToDictionary(x => x.Key, x => x.Value);

            // Only in an extreme case
            if (possibleTotems.Count == 0)
            {
                possibleTotems = Avatars.Totems;
            }

            return possibleTotems.ElementAt(Randomizer.Randomizer.Seed.Next(0, possibleTotems.Count)).Key;
        }
    }
}
