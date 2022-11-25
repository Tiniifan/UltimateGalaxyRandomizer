using System.Collections.Generic;

namespace UltimateGalaxyRandomizer.Logic
{
    public class Gender
    {
        public string Name { get; set; }

        public int[] StatBuff { get; set; }

        public Gender(string name, int[] statBuff)
        {
            Name = name;

            // Order GP -> TP -> Kick -> Dribble -> Technique -> Block -> Speed -> Stamina ->  Catch -> Luck
            StatBuff = statBuff;
        }
    }

    public class Identity
    {
        public string Year { get; set; }

        public Gender Gender { get; set; }

        public static Dictionary<byte, string> Years = new Dictionary<byte, string>()
        {
            {0x00, "?"},
            {0x01, "1"},
            {0x02, "2"},
            {0x03, "3"},
            {0x04, "Adult"},
            {0x05, "Child"},
        };

        public static Dictionary<byte, Gender> Genders = new Dictionary<byte, Gender>()
        {
            {0x01, new Gender("Boy", new int[10] {75, 25, 0, 0, 0, 0, 0, 0, 0, 0 })},
            {0x02, new Gender("Girl", new int[10] {25, 75, 0, 0, 0, 0, 0, 0, 0, 0 })},
            {0x03, new Gender("Undefined", new int[10] { 50, 50, 0, 0, 0, 0, 0, 0, 0, 0 })},
        };

        public Identity(byte x)
        {
            Year = Years[(byte)(x & 0x0F)];
            Gender = Genders[(byte)(x & 0x0F)];
        }
    }
}
