using UltimateGalaxyRandomizer.Logic.Soccer;

namespace UltimateGalaxyRandomizer.Logic
{
    public class SoccerPlayer
    {
        public Player.Player Player { get; set; }

        public SoccerMove[] Moves { get; set; }

        public SoccerAvatar Avatar { get; set; }

        public SoccerPlayer MixiMax { get; set; }

        public SoccerPlayer(Player.Player player)
        {
            Player = player;
        }

        public SoccerPlayer(Player.Player player, SoccerMove[] moves)
        {
            Player = player;
            Moves = moves;
        }
    }
}
