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

        public SkillTable()
        {

        }

        public SkillTable(DataReader reader)
        {
            Offset = reader.BaseStream.Position;
            SkillID = reader.ReadUInt32();
            SkillNumber = reader.ReadInt16();
            LearnAtLevel = reader.ReadByte();
            SkillLevel = reader.ReadByte();
        }

        public SkillTable Clone()
        {
            SkillTable cloned = new SkillTable();

            cloned.Offset = Offset;
            cloned.SkillID = SkillID;
            cloned.SkillNumber = SkillNumber;
            cloned.LearnAtLevel = LearnAtLevel;
            cloned.SkillLevel = SkillLevel;

            return cloned;
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
