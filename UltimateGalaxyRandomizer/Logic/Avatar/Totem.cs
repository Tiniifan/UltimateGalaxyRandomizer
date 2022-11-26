using System;
using System.Linq;
using UltimateGalaxyRandomizer.Tools;
using UltimateGalaxyRandomizer.Randomizer.Utility;

namespace UltimateGalaxyRandomizer.Logic
{
    public class Totem
    {
        public string Name { get; set; }

        public long Offset { get; set; }

        public UInt32 NameID { get; set; }

        public UInt32 DescriptionID { get; set; }

        public Int16 ImageModel { get; set; }

        public UInt32 MoveID { get; set; }

        public uint[] SkillRoulette { get; set; }

        public Int16 SP { get; set; }

        public byte[] SPUP { get; set; }

        public byte Position { get; set; }

        public byte Element { get; set; }

        public Totem(string name)
        {
            Name = name;
        }

        public void Read(DataReader reader)
        {
            Offset = reader.BaseStream.Position;
            DescriptionID = reader.ReadUInt32();
            NameID = reader.ReadUInt32();
            ImageModel = reader.ReadInt16();
            reader.Skip(0x16);
            MoveID = reader.ReadUInt32();
            SkillRoulette = new uint[6];
            for (int i = 0; i < 6; i++)
            {
                SkillRoulette[i] = reader.ReadUInt32();
            }
            SP = reader.ReadInt16();
            reader.Skip(0x02);
            SPUP = reader.ReadBytes(0x04).Select(x => x).ToArray();
            reader.Skip(0x08);
            Position = reader.ReadByte();
            Element = reader.ReadByte();
            reader.Skip(0x02);
        }

        public void Write(DataWriter writer)
        {
            writer.Seek((uint)Offset + 4);
            writer.WriteUInt32(DescriptionID);
            writer.WriteUInt32(NameID);
            writer.WriteInt16(ImageModel);
            writer.Skip(0x16);
            writer.WriteUInt32(MoveID);
            for (int i = 0; i < 6; i++)
            {
                writer.WriteUInt32(SkillRoulette[i]);
            }
            writer.Write(SP);
            writer.Skip(0x02);
            writer.Write(SPUP.Select(x => Convert.ToByte(x)).ToArray());
            writer.Skip(0x08);
            writer.Write(Position);
            writer.Write(Element);
            writer.Skip(2U);
        }

        public Probability GetPositionProbability()
        {
            return new Probability(Positions.Player[Position].MoveProbability);
        }

        public Probability GetElementProbability()
        {
            int[] elementProbability = new int[5] { 15, 15, 15, 15, 15 };

            if (Element == 0)
            {
                elementProbability[0] = 40;
            }
            else
            {
                elementProbability[Element] = 40;
            }

            return new Probability(elementProbability);
        }
    }
}
