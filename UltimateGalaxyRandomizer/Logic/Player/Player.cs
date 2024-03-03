using System.Collections.Generic;
using System.Linq;
using UltimateGalaxyRandomizer.Logic.Common;
using UltimateGalaxyRandomizer.Logic.Move;
using UltimateGalaxyRandomizer.Randomizer.Utility;
using UltimateGalaxyRandomizer.Resources;

namespace UltimateGalaxyRandomizer.Logic.Player
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

        public Dictionary<uint, Move.Move> GetRandomMoveset(int count, int maxSkills = 2)
        {
            var moveset = new Dictionary<uint, Move.Move>();

            for (int s = 0; s < count; s++)
            {
                // Get a random Move Type according to player position probability
                var moveType = Param.Position.GetMoveProbabilities().Random();
                if (moveset.Count(pair => pair.Value.Type == MoveType.Skill) >= maxSkills)
                {
                    // Avoids having more than [maxSkills] skills
                    while (moveType == MoveType.Skill)
                    {
                        moveType = Param.Position.GetMoveProbabilities().Random();
                    }
                }

                // Create a list of moves according to player position probability
                var possibleMoves = Moves.PlayerMoves.Where(x => !moveset.ContainsKey(x.Key) && x.Value.Type == moveType).ToDictionary(x => x.Key, x => x.Value);

                if (moveType != MoveType.Skill)
                {
                    // Create a list of moves according to player element probability
                    var moveElement = Param.Element.GetElementProbability().Random();
                    possibleMoves = possibleMoves.Where(x => !moveset.ContainsKey(x.Key) && x.Value.Element == moveElement).ToDictionary(x => x.Key, x => x.Value);
                }

                // Only in an extreme case
                if (possibleMoves.Count == 0)
                {
                    possibleMoves = Moves.PlayerMoves.Where(x => !moveset.ContainsKey(x.Key) && x.Value.Type == moveType).ToDictionary(x => x.Key, x => x.Value);
                }

                var move = possibleMoves.Random();
                moveset.Add(move.Key, move.Value);
            }

            return moveset;
        }

        public uint GetRandomFightingSpirit()
        {
            var moveType = Param.Position.GetMoveProbabilities().Random();
            var moveElement =  Param.Element.GetElementProbability().Random();
            var possibleAvatars = Avatars.FightingSpirits
                .Where(x => x.Value.Position == moveType)
                .Where(x => x.Value.Element == moveElement)
                .ToDictionary(x => x.Key, x => x.Value);

            // Only in an extreme case
            if (!possibleAvatars.Any()) possibleAvatars = Avatars.FightingSpirits.ToDictionary(x => x.Key, x => x.Value);

            return possibleAvatars.Random().Key;
        }

        public uint GetRandomTotem()
        {
            var moveType = Param.Position.GetMoveProbabilities().Random();
            var moveElement =  Param.Element.GetElementProbability().Random();
            var possibleTotems = Avatars.Totems
                .Where(x => x.Value.Position == moveType)
                .Where(x => x.Value.Element == moveElement)
                .ToDictionary(pair => pair.Key, x => x.Value);

            // Only in an extreme case
            if (!possibleTotems.Any()) possibleTotems = Avatars.Totems.ToDictionary(x => x.Key, x => x.Value);

            return possibleTotems.Random().Key;
        }
    }
}
