using System;
using UltimateGalaxyRandomizer.Tools;

namespace UltimateGalaxyRandomizer.Logic
{
    public class Move
    {
        public string Name { get; set; }

        public long Offset { get; set; }

        public uint NameID { get; set; }

        public uint DescriptionID { get; set; }

        public byte Effect { get; set; }

        public byte LearnSpeed { get; set; }

        public byte Evolution { get; set; }

        public byte Element { get; set; }

        public byte Position { get; set; }

        public byte TP { get; set; }

        public byte Partner { get; set; }

        public byte FoulRate { get; set; }

        public byte Power { get; set; }

        public short Technique { get; set; }

        public sbyte Damage { get; set; }

        public Move(string name)
        {
            Name = name;
        }

        public bool IsSkill => Position == 15;

        public void Read(DataReader reader)
        {
            Offset = reader.BaseStream.Position-4;
            NameID = reader.ReadUInt32();
            DescriptionID = reader.ReadUInt32();
            reader.Skip(0x08);
            Effect = reader.ReadByte();
            reader.Skip(0x06);
            Element = reader.ReadByte();
            LearnSpeed = reader.ReadByte();
            Evolution = reader.ReadByte();
            Position = reader.ReadByte();
            TP = reader.ReadByte();
            Partner = reader.ReadByte();
            reader.Skip(0x01);
            FoulRate = reader.ReadByte();
            Power = reader.ReadByte();
            Technique = reader.ReadInt16();
            reader.Skip(0x01);
            Damage = reader.ReadSByte();
            reader.Skip(0x04);
        }

        public void Write(DataWriter writer)
        {
            writer.Seek((uint) Offset + 4);
            writer.WriteUInt32(NameID);
            writer.WriteUInt32(DescriptionID);
            writer.Skip(0x08);
            writer.Write(Effect);
            writer.Skip(0x06);
            writer.Write(Element);
            writer.Write(LearnSpeed);
            writer.Write(Evolution);
            writer.Write(Position);
            writer.Write(TP);
            writer.Write(Partner);
            writer.Skip(0x01);
            writer.Write(FoulRate);
            writer.WriteByte(Power);
            writer.WriteInt16(Technique);
            writer.Skip(0x01);
            writer.Write(Damage);
            writer.Skip(0x04);
        }
    }
}
