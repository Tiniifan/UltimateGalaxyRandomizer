using System;
using System.Linq;
using UltimateGalaxyRandomizer.Tools;
using UltimateGalaxyRandomizer.Resources;

namespace UltimateGalaxyRandomizer.Logic
{
    public class Param
    {
        public long Offset { get; set; }

        public uint BaseID { get; set; }

        public short SkillOffset { get; set; }

        public byte UnknownValue { get; set; }

        public byte Invoke { get; set; }

        public Stats BaseStat { get; set; }

        public byte Element { get; set; }

        public byte Position { get; set; }

        public Stats GrownStat { get; set; }

        public uint Avatar { get; set; }

        public byte ExperienceBar { get; set; }

        public byte SkillCount { get; set; }

        public short Freedom { get; set; }

        public Param()
        {

        }

        public Param(DataReader reader)
        {
            Offset = reader.BaseStream.Position-4;
            BaseID = reader.ReadUInt32();
            SkillOffset = reader.ReadInt16();
            UnknownValue = reader.ReadByte();
            Invoke = reader.ReadByte();
            BaseStat = new Stats(reader.ReadBytes(0xA).Select(x => (int)x).ToArray());
            Element = reader.ReadByte();
            Position = reader.ReadByte();
            GrownStat = new Stats(reader.ReadBytes(0xA).Select(x => (int)x).ToArray());
            Avatar = Avatars.Table[reader.ReadInt16()];
            ExperienceBar = reader.ReadByte();
            SkillCount = (byte) (reader.ReadByte() >> 0x04);
            Freedom = reader.ReadInt16();
        }

        public Param Clone()
        {
            Param cloned = new Param();

            cloned.Offset = Offset;
            cloned.BaseID = BaseID;
            cloned.SkillOffset = SkillOffset;
            cloned.UnknownValue = UnknownValue;
            cloned.Invoke = Invoke;
            cloned.BaseStat = BaseStat;
            cloned.Element = Element;
            cloned.Position = Position;
            cloned.GrownStat = GrownStat;
            cloned.Avatar = Avatar;
            cloned.ExperienceBar = ExperienceBar;
            cloned.SkillCount = SkillCount;
            cloned.Freedom = Freedom;

            return cloned;
        }

        public void Swap(Param characterParam)
        {
            SkillOffset = characterParam.SkillOffset;
            UnknownValue = characterParam.UnknownValue;
            Invoke = characterParam.Invoke;
            BaseStat = characterParam.BaseStat;
            Element = characterParam.Element;
            Position = characterParam.Position;
            GrownStat = characterParam.GrownStat;
            Avatar = characterParam.Avatar;
            ExperienceBar = characterParam.ExperienceBar;
            SkillCount = characterParam.SkillCount;
            Freedom = characterParam.Freedom;
        }

        public void Write(DataWriter writer)
        {
            writer.Seek((uint)Offset + 4);
            writer.WriteUInt32(BaseID);
            writer.WriteInt16(SkillOffset);
            writer.Write(UnknownValue);
            writer.Write(Invoke);
            writer.Write(BaseStat.Values.Values.Select(x => Convert.ToByte(x)).ToArray());
            writer.Write(Element);
            writer.Write(Position);
            writer.Write(GrownStat.Values.Values.Select(x => Convert.ToByte(x)).ToArray());
            writer.WriteInt16(Avatars.Table.FirstOrDefault(x => x.Value == Avatar).Key);
            writer.Write(ExperienceBar);
            writer.Write((byte)((SkillCount << 4) | 1 & 0x0F));
            writer.WriteInt16(Freedom);
        }
    }
}
