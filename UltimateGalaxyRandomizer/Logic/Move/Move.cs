using System;
using UltimateGalaxyRandomizer.Logic.Common;
using UltimateGalaxyRandomizer.Tools;

namespace UltimateGalaxyRandomizer.Logic.Move
{
    public enum MoveType
    {
        Shoot = 1,
        Dribble = 2,
        Block = 3,
        Save = 4,
        Skill = 15
    }

    public class Move
    {
        public string Name { get; set; }

        public long Offset { get; set; }

        public uint NameID { get; set; }

        public uint DescriptionID { get; set; }

        public byte Effect { get; set; }

        public byte LearnSpeed { get; set; }

        public byte Evolution { get; set; }

        public Element Element { get; set; }

        public MoveType Type { get; set; }

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

        public void Read(DataReader reader)
        {
            Offset = reader.BaseStream.Position-4;
            NameID = reader.ReadUInt32();
            DescriptionID = reader.ReadUInt32();
            reader.Skip(0x08);
            Effect = reader.ReadByte();
            reader.Skip(0x06);
            Element = (Element)reader.ReadByte();
            LearnSpeed = reader.ReadByte();
            Evolution = reader.ReadByte();
            Type = (MoveType)reader.ReadByte();
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
            writer.Write(Convert.ToByte(Element));
            writer.Write(LearnSpeed);
            writer.Write(Evolution);
            writer.Write(Convert.ToByte(Type));
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
