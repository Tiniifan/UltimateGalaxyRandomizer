using System.Linq;
using UltimateGalaxyRandomizer.Resources;
using UltimateGalaxyRandomizer.Tools;

namespace UltimateGalaxyRandomizer.Logic.Player
{
    public class SkillTable
    {
        public Move.Move Skill { get; set; }

        // public short SkillNumber

        public byte LearnAtLevel { get; set; }

        public byte SkillLevel { get; set; }

        public SkillTable()
        {

        }

        public SkillTable(DataReader reader)
        {
            Skill = Moves.PlayerMoves[reader.ReadUInt32()];
            reader.Skip(sizeof(short)); //SkillNumber = reader.ReadInt16()
            LearnAtLevel = reader.ReadByte();
            SkillLevel = reader.ReadByte();
        }

        public SkillTable Clone() => new SkillTable
        {
            Skill = Skill,
            // SkillNumber = SkillNumber,
            LearnAtLevel = LearnAtLevel,
            SkillLevel = SkillLevel
        };

        public void Write(DataWriter writer, ref short skillNumber)
        {
            writer.WriteUInt32(Moves.PlayerMoves.First(pair => pair.Value == Skill).Key);
            writer.WriteInt16(skillNumber);
            writer.Write(LearnAtLevel);
            writer.Write(SkillLevel);
        }
    }
}
