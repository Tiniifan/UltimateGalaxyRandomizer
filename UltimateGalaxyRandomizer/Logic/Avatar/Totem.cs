using System;
using System.Linq;
using UltimateGalaxyRandomizer.Logic.Common;
using UltimateGalaxyRandomizer.Logic.Move;
using UltimateGalaxyRandomizer.Tools;

namespace UltimateGalaxyRandomizer.Logic.Avatar
{
    public class Totem(string name) : Avatar(name)
    {
        public uint[] SkillRoulette { get; set; }

        public short SP { get; set; }

        public byte[] SPUP { get; set; }

        public void Read(DataReader reader)
        {
            Offset = reader.BaseStream.Position - 4;
            DescriptionId = reader.ReadUInt32();
            NameId = reader.ReadUInt32();
            ImageModel = reader.ReadInt16();
            reader.Skip(0x16);
            MoveId = reader.ReadUInt32();
            SkillRoulette = new uint[6];
            for (int i = 0; i < 6; i++)
            {
                SkillRoulette[i] = reader.ReadUInt32();
            }

            SP = reader.ReadInt16();
            reader.Skip(0x02);
            SPUP = reader.ReadBytes(0x04).Select(x => x).ToArray();
            reader.Skip(0x08);
            Position = (MoveType)reader.ReadByte();
            Element = (Element)reader.ReadByte();
            reader.Skip(0x02);
        }

        public void Write(DataWriter writer)
        {
            writer.Seek((uint)Offset + 4);
            writer.WriteUInt32(DescriptionId);
            writer.WriteUInt32(NameId);
            writer.WriteInt16(ImageModel);
            writer.Skip(0x16);
            writer.WriteUInt32(MoveId);
            for (int i = 0; i < 6; i++)
            {
                writer.WriteUInt32(SkillRoulette[i]);
            }

            writer.Write(SP);
            writer.Skip(0x02);
            writer.Write(SPUP.Select(x => Convert.ToByte(x)).ToArray());
            writer.Skip(0x08);
            writer.Write(Convert.ToByte(Position));
            writer.Write(Convert.ToByte(Element));
            writer.Skip(0x02);
        }
    }
}
