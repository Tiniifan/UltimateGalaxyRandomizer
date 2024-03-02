using System;
using UltimateGalaxyRandomizer.Tools;

namespace UltimateGalaxyRandomizer.Logic
{
    public class Base
    {
        public long Offset { get; set; }

        public uint BaseID { get; set; }

        public uint NameID { get; set; }

        public uint NicknameID { get; set; }

        public uint DescriptionID { get; set; }

        public short HeadID { get; set; }

        public short HeadIDSwap { get; set; }

        public byte Style { get; set; }

        public byte Size { get; set; }

        public byte Identity { get; set; }

        public Base()
        {

        }

        public Base(DataReader reader)
        {
            Offset = reader.BaseStream.Position;
            BaseID = reader.ReadUInt32();
            NameID = reader.ReadUInt32();
            NicknameID = reader.ReadUInt32();
            DescriptionID = reader.ReadUInt32();
            reader.Skip(0x04);
            HeadID = reader.ReadInt16();
            reader.Skip(0x02);
            Style = reader.ReadByte();
            reader.Skip(0x01);
            Size = reader.ReadByte();
            Identity = reader.ReadByte();
            reader.Skip(0x04);
        }

        public Base Clone() => new Base
        {
            Offset = Offset,
            BaseID = BaseID,
            NameID = NameID,
            NicknameID = NicknameID,
            DescriptionID = DescriptionID,
            HeadID = HeadID,
            Style = Style,
            Size = Size,
            Identity = Identity
        };

        public void Swap(Base characterBase)
        {
            NameID = characterBase.NameID;
            NicknameID = characterBase.NicknameID;
            DescriptionID = characterBase.DescriptionID;
            HeadIDSwap = characterBase.HeadID;
            Style = characterBase.Style;
            Size = characterBase.Size;
            Identity = characterBase.Identity;
        }

        public void Write(DataWriter writer)
        {
            writer.Seek((uint)Offset);
            writer.WriteUInt32(BaseID);
            writer.WriteUInt32(NameID);
            writer.WriteUInt32(NicknameID);
            writer.WriteUInt32(DescriptionID);
            writer.Skip(0x04);
            writer.WriteInt16(HeadID);
            writer.Skip(0x02);
            writer.Write(Style);
            writer.Skip(0x01);
            writer.Write(Size);
            writer.Write(Identity);
            writer.Skip(0x04);
        }
    }
}
