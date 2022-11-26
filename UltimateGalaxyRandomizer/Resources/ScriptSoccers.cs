using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateGalaxyRandomizer.Resources
{
    public class ScriptSoccer
    {
        public List<int> PlayerIndex { get; set; }
        public UInt32 RightMove { get; set; }

        public ScriptSoccer(List<int> playerIndex, UInt32 moveID)
        {
            PlayerIndex = playerIndex;
            RightMove = moveID;
        }
    }

    public static class ScriptSoccers
    {
        public static Dictionary<uint, ScriptSoccer> ScriptSoccerGalaxy = new Dictionary<uint, ScriptSoccer>()
        {
            {0x02, new ScriptSoccer(new List<int>(){8, 9, 10}, 0x5B620610) },
            {0x03, new ScriptSoccer(new List<int>(){7}, 0x42B66C65) },
            {0x05, new ScriptSoccer(new List<int>(){9, 10}, 0xB201A325) },
            {0x06, new ScriptSoccer(new List<int>(){10}, 0x2B08F29F) },
            {0x08, new ScriptSoccer(new List<int>(){8, 9, 10}, 0x5C0FC209) },
            {0x09, new ScriptSoccer(new List<int>(){8, 9}, 0xCCB0DF98) },
            {0x0A, new ScriptSoccer(new List<int>(){9}, 0xBBB7EF0E) },
            {0x0B, new ScriptSoccer(new List<int>(){9}, 0x42793751) },
            {0X0D, new ScriptSoccer(new List<int>(){9, 10}, 0xF05D3528) },
            {0X0F, new ScriptSoccer(new List<int>(){9}, 0xAB1A9264) },
            {0x10, new ScriptSoccer(new List<int>(){9}, 0xAB1A9264) },
            {0x11, new ScriptSoccer(new List<int>(){9}, 0xAB1A9264) },
            {0x12, new ScriptSoccer(new List<int>(){9}, 0xAB1A9264) },
            {0x13, new ScriptSoccer(new List<int>(){9}, 0xAB1A9264) },
            {0x14, new ScriptSoccer(new List<int>(){9}, 0xAB1A9264) },
        };
    }
}
