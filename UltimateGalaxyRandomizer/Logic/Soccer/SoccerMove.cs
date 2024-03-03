namespace UltimateGalaxyRandomizer.Logic.Soccer
{
    public class SoccerMove
    {
        public Move.Move Move { get; set; }

        public byte Level { get; set; }

        public SoccerMove(Move.Move move, byte level)
        {
            Move = move;
            Level = level;
        }
    }
}
