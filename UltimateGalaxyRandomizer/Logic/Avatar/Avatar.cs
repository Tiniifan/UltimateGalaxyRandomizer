using UltimateGalaxyRandomizer.Logic.Common;
using UltimateGalaxyRandomizer.Logic.Move;

namespace UltimateGalaxyRandomizer.Logic.Avatar
{
    public abstract class Avatar(string name)
    {
        public string Name { get; set; } = name;

        public long Offset { get; set; }

        public uint NameId { get; set; }

        public uint DescriptionId { get; set; }

        public short ImageModel { get; set; }

        public MoveType Position { get; set; }

        public Element Element { get; set; }

        public uint MoveId { get; set; }
    }
}
