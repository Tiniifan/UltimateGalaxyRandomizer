using System.Collections.Generic;
using System.Linq;
using UltimateGalaxyRandomizer.Logic.Common;
using UltimateGalaxyRandomizer.Randomizer.Utility;
using UltimateGalaxyRandomizer.Resources;

namespace UltimateGalaxyRandomizer.Logic.Player
{
    public class Player(string name)
    {
        public string Name { get; set; } = name;

        public Param Param { get; set; }

        public Base Base { get; set; }

        public SkillTable[] Skills { get; set; }

        public Player Clone() => new(Name)
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
                var moveType = moveset.Count(pair => pair.Value.Type == MoveType.Skill) >= maxSkills ? 
                    Param.Position.GetMoveProbabilities().Where(pair => pair.Key != MoveType.Skill).RandomWithProbability() : 
                    Param.Position.GetMoveProbabilities().RandomWithProbability();

                // Create a list of moves according to player position probability
                var possibleMoves = Moves.PlayerMoves.Where(x => !moveset.ContainsKey(x.Key) && x.Value.Type == moveType).ToDictionary(x => x.Key, x => x.Value);

                if (moveType != MoveType.Skill)
                {
                    // Create a list of moves according to player move effects probability
                    var effect = Param.Position.GetEffectProbabilities(moveType).RandomWithProbability();
                    if(possibleMoves.Values.Any(m => m.Effect == effect))
                        possibleMoves = possibleMoves.Where(x => x.Value.Effect == effect).ToDictionary(x => x.Key, x => x.Value);
                    
                    // Create a list of moves according to player element probability
                    var moveElement = Param.Element.GetElementProbability().RandomWithProbability();
                    if(possibleMoves.Values.Any(m => m.Element == moveElement))
                        possibleMoves = possibleMoves.Where(x => x.Value.Element == moveElement).ToDictionary(x => x.Key, x => x.Value);

                    //limit by position. Cheaper moves on first positions, expensive moves on higher positions.
                    //most expensive move costs 85 TP
                    if(possibleMoves.Values.Any(m => s > 3 || (m.TP >= s * 20 && m.TP <= (s + 1) * 22)))
                        possibleMoves = possibleMoves.Where(m => s > 3 || (m.Value.TP >= s * 20 && m.Value.TP <= (s + 1) * 22)).ToDictionary(x => x.Key, x => x.Value);
                }

                var move = possibleMoves.Random();
                moveset.Add(move.Key, move.Value);
            }

            return moveset;
        }

        public uint GetRandomFightingSpirit()
        {
            var moveType = Param.Position.GetMoveProbabilities().Where(pair => pair.Key != MoveType.Skill).RandomWithProbability();
            var moveElement =  Param.Element.GetElementProbability().RandomWithProbability();
            var possibleAvatars = Avatars.FightingSpirits
                .Where(x => x.Value.Position == moveType && x.Value.Element == moveElement)
                .ToDictionary(x => x.Key, x => x.Value);

            // Only in an extreme case
            if (!possibleAvatars.Any())
                possibleAvatars = Avatars.FightingSpirits
                    .Where(x => x.Value.Position == moveType)
                    .ToDictionary(x => x.Key, x => x.Value);

            return possibleAvatars.Random().Key;
        }

        public uint GetRandomTotem()
        {
            var moveType = Param.Position.GetMoveProbabilities().Where(pair => pair.Key != MoveType.Skill).RandomWithProbability();
            var moveElement =  Param.Element.GetElementProbability().RandomWithProbability();
            var possibleTotems = Avatars.Totems
                .Where(x => x.Value.Position == moveType && x.Value.Element == moveElement)
                .ToDictionary(pair => pair.Key, x => x.Value);

            // Only in an extreme case
            if (!possibleTotems.Any())
                possibleTotems = Avatars.Totems
                    .Where(x => x.Value.Position == moveType)
                    .ToDictionary(x => x.Key, x => x.Value);

            return possibleTotems.Random().Key;
        }
    }
}
