using System.Collections.Generic;

namespace UltimateGalaxyRandomizer.Logic
{
    public class Invoke
    {
        public bool CanInvoke { get; set; }

        public bool IsFightingSpirit { get; set; }

        public bool HaveArmoufly { get; set; }

        public bool IsLocked { get; set; }

        public bool IsStory { get; set; }

        public Invoke(params bool[] args)
        {
            CanInvoke = args[0];
            IsFightingSpirit = args[1];
            HaveArmoufly = args[2];
            IsLocked = args[3];
            IsStory = args[4];
        }
    }

    public class Invokes
    {
        public static Dictionary<short, Invoke> Values = new Dictionary<short, Invoke>()
        {
            {0x00, new Invoke(false, false, false, false, false) },
            {0x01, new Invoke(false, false, false, false, false) },
            {0x02, new Invoke(false, false, false, false, false) },
            {0x03, new Invoke(false, false, false, false, false) },
            {0x04, new Invoke(true, true, false, false, false) },
            {0x0C, new Invoke(true, true, true, false, false) },
            {0x0D, new Invoke(true, true, true, false, false) },
            {0x1C, new Invoke(true, true, true, true, true) },
            {0x14, new Invoke(true, true, false, true, true) },
            {0x20, new Invoke(false, false, false, false, false) },
            {0x2C, new Invoke(true, true, true, false, false) },
            {0x44, new Invoke(true, false, false, false, false) },
            {0x10, new Invoke(false, false, false, true, true) },
        };
    }
}
