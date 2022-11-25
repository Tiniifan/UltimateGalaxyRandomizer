namespace UltimateGalaxyRandomizer.Logic
{
    public class SoccerPlayer
    {
        public Player Player { get; set; }

        public SoccerMove[] Moves { get; set; }

        public SoccerAvatar Avatar { get; set; }

        public SoccerPlayer MixiMax { get; set; }

        public SoccerPlayer(Player player)
        {
            Player = player;
        }

        public SoccerPlayer(Player player, SoccerMove[] moves)
        {
            Player = player;
            Moves = moves;
        }
    }
}
