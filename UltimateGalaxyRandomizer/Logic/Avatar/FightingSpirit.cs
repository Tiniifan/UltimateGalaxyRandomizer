using System;
using System.Linq;
using UltimateGalaxyRandomizer.Logic.Common;
using UltimateGalaxyRandomizer.Logic.Move;
using UltimateGalaxyRandomizer.Tools;

namespace UltimateGalaxyRandomizer.Logic.Avatar
{
    public class FightingSpirit : Avatar
    {
        public uint SkillId { get; set; }

        public short FS { get; set; }

        public short Attack { get; set; }

        public int[] FSPUP { get; set; }

        public int[] AttackUP { get; set; }

        public FightingSpirit(string name): base(name)
        {
        }

        public void Read(DataReader reader)
        {
            Offset = reader.BaseStream.Position - 4;
            NameId = reader.ReadUInt32();
            reader.Skip(0x04);
            ImageModel = reader.ReadInt16();
            reader.Skip(0x16);
            MoveId = reader.ReadUInt32();
            SkillId = reader.ReadUInt32();
            reader.Skip(0x0C);
            FS = reader.ReadInt16();
            Attack = reader.ReadInt16();
            FSPUP = reader.ReadBytes(0x05).Select(x => (int)x).ToArray();
            AttackUP = reader.ReadBytes(0x05).Select(x => (int)x).ToArray();
            Position = (MoveType)reader.ReadByte();
            Element = (Element)reader.ReadByte();
            reader.Skip(0x04);
        }

        public void Write(DataWriter writer)
        {
            writer.Seek((uint)Offset + 4);
            writer.WriteUInt32(NameId);
            writer.Skip(0x04);
            writer.WriteInt16(ImageModel);
            writer.Skip(0x16);
            writer.WriteUInt32(MoveId);
            writer.WriteUInt32(SkillId);
            writer.Skip(0x0C);
            writer.WriteInt16(FS);
            writer.WriteInt16(Attack);
            writer.Write(FSPUP.Select(Convert.ToByte).ToArray());
            writer.Write(AttackUP.Select(Convert.ToByte).ToArray());
            writer.Write(Convert.ToByte(Position));
            writer.Write(Convert.ToByte(Element));
            writer.Skip(0x04);
        }
    }
}
