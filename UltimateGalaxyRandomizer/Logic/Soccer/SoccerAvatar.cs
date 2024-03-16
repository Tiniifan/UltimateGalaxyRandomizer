namespace UltimateGalaxyRandomizer.Logic.Soccer
{
    public class SoccerAvatar(Avatar.Avatar avatar, byte level)
    {
        public Avatar.Avatar Avatar { get; set; } = avatar;

        public byte Level { get; set; } = level;
    }
}
