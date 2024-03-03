using UltimateGalaxyRandomizer.Logic.Common;
using UltimateGalaxyRandomizer.Logic.Move;

namespace UltimateGalaxyRandomizer.Logic.Avatar
{
    public abstract class Avatar
    {
        public string Name { get; set; }

        public long Offset { get; set; }

        public uint NameId { get; set; }

        public uint DescriptionId { get; set; }

        public short ImageModel { get; set; }

        public MoveType Position { get; set; }

        public Element Element { get; set; }

        public uint MoveId { get; set; }

        protected Avatar(string name)
        {
            Name = name;
        }
    }
}
