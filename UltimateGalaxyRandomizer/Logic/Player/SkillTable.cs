using System;
using UltimateGalaxyRandomizer.Tools;

namespace UltimateGalaxyRandomizer.Logic
{
    public class SkillTable
    {
        public long Offset { get; set; }

        public uint SkillId { get; set; }

        public short SkillNumber { get; set; }

        public byte LearnAtLevel { get; set; }

        public byte SkillLevel { get; set; }

        public SkillTable()
        {

        }

        public SkillTable(DataReader reader)
        {
            Offset = reader.BaseStream.Position;
            SkillId = reader.ReadUInt32();
            SkillNumber = reader.ReadInt16();
            LearnAtLevel = reader.ReadByte();
            SkillLevel = reader.ReadByte();
        }

        public SkillTable Clone() => new SkillTable
        {
            Offset = Offset,
            SkillId = SkillId,
            SkillNumber = SkillNumber,
            LearnAtLevel = LearnAtLevel,
            SkillLevel = SkillLevel
        };

        public void Write(DataWriter writer)
        {
            writer.WriteUInt32(SkillId);
            writer.WriteInt16(SkillNumber);
            writer.Write(LearnAtLevel);
            writer.Write(SkillLevel);
        }
    }
}
