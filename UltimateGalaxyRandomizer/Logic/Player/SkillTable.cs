using System;
using UltimateGalaxyRandomizer.Tools;

namespace UltimateGalaxyRandomizer.Logic
{
    public class SkillTable
    {
        public long Offset { get; set; }

        public UInt32 SkillID { get; set; }

        public Int16 SkillNumber { get; set; }

        public byte LearnAtLevel { get; set; }

        public byte SkillLevel { get; set; }

        public SkillTable(DataReader reader)
        {
            Offset = reader.BaseStream.Position;
            SkillID = reader.ReadUInt32();
            SkillNumber = reader.ReadInt16();
            LearnAtLevel = reader.ReadByte();
            SkillLevel = reader.ReadByte();
        }

        public void Write(DataWriter writer)
        {
            writer.WriteUInt32(SkillID);
            writer.WriteInt16(SkillNumber);
            writer.Write(LearnAtLevel);
            writer.Write(SkillLevel);
        }
    }
}
