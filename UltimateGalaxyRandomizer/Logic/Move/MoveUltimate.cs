using System;
using UltimateGalaxyRandomizer.Tools;

namespace UltimateGalaxyRandomizer.Logic
{
    public class MoveUltimate
    {
        public string Name { get; set; }

        public long Offset { get; set; }

        public Int16 Power { get; set; }

        public byte TP { get; set; }

        public Int16 Technique { get; set; }

        public sbyte Damage { get; set; }

        public MoveUltimate(string name)
        {
            Name = name;
        }

        public void Read(DataReader reader)
        {
            Offset = reader.BaseStream.Position;
            Power = reader.ReadInt16();
            reader.Skip(0x01);
            TP = reader.ReadByte();
            Technique = reader.ReadInt16();
            Damage = reader.ReadSByte();
        }

        public void Write(DataWriter writer)
        {
            writer.Seek((uint) Offset);
            writer.WriteInt16(Power);
            writer.Skip(0x01);
            writer.Write(TP);
            writer.WriteInt16(Technique);
            writer.Write(Damage);
        }
    }
}
