using System;
using UltimateGalaxyRandomizer.Tools;

namespace UltimateGalaxyRandomizer.Logic
{
    public class Team
    {
        public string Name { get; set; }

        public long Offset { get; set; }

        public UInt32 TeamParamID { get; set; }

        public bool IsMatchField { get; set; }

        public byte MiniMatchValue { get; set; }

        public UInt32 ScriptID { get; set; }

        public byte Timer { get; set; }

        public UInt32 ArtificialIntelligenceID { get; set; }

        public UInt32 ScriptID2 { get; set; }

        public UInt32 RestrictionID { get; set; }

        public UInt32 RestrictionID2 { get; set; }

        public TeamParam Param { get; set; }

        public SoccerCharaConfig SoccerChara { get; set; }

        public Team(string name)
        {
            Name = name;
        }

        public void Read(DataReader reader)
        {
            Offset = reader.BaseStream.Position - 12;
            TeamParamID = reader.ReadUInt32();
            IsMatchField = Convert.ToBoolean(reader.ReadByte() - 1);
            reader.Skip(0x0F);
            MiniMatchValue = reader.ReadByte();
            reader.Skip(0x03);
            ScriptID = reader.ReadUInt32();
            Timer = reader.ReadByte();
            reader.Skip(0x03);
            ArtificialIntelligenceID = reader.ReadUInt32();
            ScriptID2 = reader.ReadUInt32();
            reader.Skip(0x08);
            RestrictionID = reader.ReadUInt32();
            RestrictionID2 = reader.ReadUInt32();
        }

        public void Write(DataWriter writer)
        {
            writer.Seek((uint)Offset + 12);
            writer.WriteUInt32(TeamParamID);
            writer.WriteByte(Convert.ToByte(IsMatchField) + 1);
            writer.Skip(0x0F);
            writer.WriteByte(MiniMatchValue);
            writer.Skip(0x03);
            writer.WriteUInt32(ScriptID);
            writer.WriteByte(Timer);
            writer.Skip(0x03);
            writer.WriteUInt32(ArtificialIntelligenceID);
            writer.WriteUInt32(ScriptID2);
            writer.Skip(0x08);
            writer.WriteUInt32(RestrictionID);
            writer.WriteUInt32(RestrictionID2);
        }
    }
}
