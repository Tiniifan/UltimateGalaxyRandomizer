using System;
using UltimateGalaxyRandomizer.Tools;

namespace UltimateGalaxyRandomizer.Logic
{
    public class TeamParam
    {
        public long Offset { get; set; }

        public UInt32 TeamParamID { get; set; }

        public UInt32 TeamBaseID { get; set; }

        public Int32 Prestige { get; set; }

        public Int32 Experience { get; set; }

        public UInt32[] Equipments { get; set; }

        public UInt32[] Drop { get; set; }

        public UInt32 Kit { get; set; }

        public byte[] DropRate { get; set; }

        public UInt32 Formation { get; set; }

        public byte Level { get; set; }

        public UInt32 Coach { get; set; }

        public UInt32 Tactic { get; set; }

        public Int32 Freedom { get; set; }

        public TeamParam(DataReader reader)
        {
            Offset = reader.BaseStream.Position;
            reader.Skip(0x10);
            TeamParamID = reader.ReadUInt32();
            TeamBaseID = reader.ReadUInt32();
            reader.Skip(0x04);
            Prestige = reader.ReadInt32();
            Experience = reader.ReadInt32();

            Equipments = new UInt32[4];
            for (int i = 0; i < 4; i++)
            {
                Equipments[i] = reader.ReadUInt32();
            }

            Drop = new UInt32[6];
            for (int i = 0; i < 5; i++)
            {
                Drop[i] = reader.ReadUInt32();
            }

            Kit = reader.ReadUInt32();

            DropRate = new byte[6];
            for (int i = 0; i < 5; i++)
            {
                DropRate[i] = reader.ReadByte();
                reader.Skip(0x03);
            }

            Formation = reader.ReadUInt32();
            Level = reader.ReadByte();
            reader.Skip(0x03);
            Drop[5] = reader.ReadUInt32();
            DropRate[5] = reader.ReadByte();
            reader.Skip(0x03);
            reader.Skip(0x04);
            Coach = reader.ReadUInt32();
            Tactic = reader.ReadUInt32();
            reader.Skip(0x04);
            Freedom = reader.ReadInt32();
            reader.Skip(0x04);
        }

        public void Write(DataWriter writer)
        {
            writer.Skip(0x10);
            writer.WriteUInt32(TeamParamID);
            writer.WriteUInt32(TeamBaseID);
            writer.Skip(0x04);
            writer.WriteInt32(Prestige);
            writer.WriteInt32(Experience);

            for (int i = 0; i < 4; i++)
            {
                writer.WriteUInt32(Equipments[i]);
            }

            for (int i = 0; i < 5; i++)
            {
                writer.WriteUInt32(Drop[i]);
            }

            writer.WriteUInt32(Kit);

            for (int i = 0; i < 5; i++)
            {
                writer.WriteByte(DropRate[i]);
                writer.Skip(0x03);
            }

            writer.WriteUInt32(Formation);
            writer.WriteByte(Level);
            writer.Skip(0x03);
            writer.WriteUInt32(Drop[5]);
            writer.WriteByte(DropRate[5]);
            writer.Skip(0x03);
            writer.Skip(0x04);
            writer.WriteUInt32(Coach);
            writer.WriteUInt32(Tactic);
            writer.Skip(0x04);
            writer.WriteInt32(Freedom);
            writer.Skip(0x04);
        }
    }
}
