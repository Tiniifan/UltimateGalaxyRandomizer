using System;
using System.Linq;
using UltimateGalaxyRandomizer.Tools;

namespace UltimateGalaxyRandomizer.Logic
{
    public class Equipment
    {
        public string Name { get; set; }

        public long Offset { get; set; }

        public Stats BaseStat { get; set; }

        public Equipment(String name)
        {
            Name = name;
        }

        public void Read(DataReader reader)
        {
            Offset = reader.BaseStream.Position - 4;
            reader.Skip(0x1C);
            BaseStat = new Stats(reader.ReadBytes(0xA).Select(x => (int)x).ToArray());
            reader.Skip(0x06);
        }

        public void Write(DataWriter writer)
        {
            writer.Seek((uint) Offset + 4);
            writer.Skip(0x1C);
            writer.Write(BaseStat.Values.Select(x => Convert.ToByte(x)).ToArray());
            writer.Skip(0x06);
        }
    }
}
