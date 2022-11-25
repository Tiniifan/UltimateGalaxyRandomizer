namespace UltimateGalaxyRandomizer.Logic
{
    public class SoccerAvatar
    {
        public object Avatar { get; set; }

        public byte Level { get; set; }

        public SoccerAvatar(object avatar, byte level)
        {
            Avatar = avatar;
            Level = level;
        }
    }
}
