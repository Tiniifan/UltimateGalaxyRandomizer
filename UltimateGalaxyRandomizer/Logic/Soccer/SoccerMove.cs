namespace UltimateGalaxyRandomizer.Logic
{
    public class SoccerMove
    {
        public Move Move { get; set; }

        public byte Level { get; set; }

        public SoccerMove(Move move, byte level)
        {
            Move = move;
            Level = level;
        }
    }
}
