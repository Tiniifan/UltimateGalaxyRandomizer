using System;
using System.Linq;
using UltimateGalaxyRandomizer.Logic.Common;
using UltimateGalaxyRandomizer.Tools;
using UltimateGalaxyRandomizer.Randomizer.Utility;

namespace UltimateGalaxyRandomizer.Logic
{
    public class FightingSpirit
    {
        public string Name { get; set; }

        public long Offset { get; set; }

        public uint NameID { get; set; }

        public uint DescriptionID { get; set; }

        public short ImageModel { get; set; }

        public byte Position { get; set; }

        public byte Element { get; set; }

        public uint MoveID { get; set; }

        public uint SkillID { get; set; }

        public short FS { get; set; }

        public short Attack { get; set; }

        public int[] FSPUP { get; set; }

        public int[] AttackUP { get; set; }

        public FightingSpirit(string name)
        {
            Name = name;
        }

        public void Read(DataReader reader)
        {
            Offset = reader.BaseStream.Position - 4;
            NameID = reader.ReadUInt32();
            reader.Skip(0x04);
            ImageModel = reader.ReadInt16();
            reader.Skip(0x16);
            MoveID = reader.ReadUInt32();
            SkillID = reader.ReadUInt32();
            reader.Skip(0x0C);
            FS = reader.ReadInt16();
            Attack = reader.ReadInt16();
            FSPUP = reader.ReadBytes(0x05).Select(x => (int)x).ToArray();
            AttackUP = reader.ReadBytes(0x05).Select(x => (int)x).ToArray();
            Position = reader.ReadByte();
            Element = reader.ReadByte();
            reader.Skip(0x04);
        }

        public void Write(DataWriter writer)
        {
            writer.Seek((uint)Offset + 4);
            writer.WriteUInt32(NameID);
            writer.Skip(0x04);
            writer.WriteInt16(ImageModel);
            writer.Skip(0x16);
            writer.WriteUInt32(MoveID);
            writer.WriteUInt32(SkillID);
            writer.Skip(0x0C);
            writer.WriteInt16(FS);
            writer.WriteInt16(Attack);
            writer.Write(FSPUP.Select(Convert.ToByte).ToArray());
            writer.Write(AttackUP.Select(Convert.ToByte).ToArray());
            writer.Write(Position);
            writer.Write(Element);
            writer.Skip(0x04);
        }

        public int[] GetPositionProbability() => Positions.Player[Position].MoveProbability;

        public int[] GetElementProbability()
        {
            int[] elementProbability = new int[5] { 15, 15, 15, 15, 15 };

            if (Element == 0)
            {
                elementProbability[0] = 40;
            }
            else
            {
                elementProbability[Element-1] = 40;
            }

            return elementProbability;
        }
    }
}
