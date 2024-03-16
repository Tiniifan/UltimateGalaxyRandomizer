using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UltimateGalaxyRandomizer.Resources;
using UltimateGalaxyRandomizer.Tools;

namespace UltimateGalaxyRandomizer.Logic.Soccer
{
    public class SoccerCharaConfig
    {
        public List<SoccerPlayer> Players { get;  } = new List<SoccerPlayer>();

        private static SoccerPlayer GetPlayer(uint charaParamId)
        {
            if (Resources.Players.Story.TryGetValue(charaParamId, out var player))
            {
                return new SoccerPlayer(player);
            }

            if (Resources.Players.Normal.TryGetValue(charaParamId, out player))
            {
                return new SoccerPlayer(player);
            }

            if (Resources.Players.Scout.TryGetValue(charaParamId, out player))
            {
                return new SoccerPlayer(player);
            }

            return null;
        }
        private static uint GetPlayerKey(SoccerPlayer player)
        {
            if (Resources.Players.Story.FirstOrDefault(x => x.Value == player.Player).Key != 0x00)
            {
                return Resources.Players.Story.FirstOrDefault(x => x.Value == player.Player).Key;
            }

            if (Resources.Players.Normal.FirstOrDefault(x => x.Value == player.Player).Key != 0x00)
            {
                return Resources.Players.Normal.FirstOrDefault(x => x.Value == player.Player).Key;
            }

            if (Resources.Players.Scout.FirstOrDefault(x => x.Value == player.Player).Key != 0x00)
            {
                return Resources.Players.Scout.FirstOrDefault(x => x.Value == player.Player).Key;
            }

            return 0x00;
        }

        public static SoccerAvatar GetAvatar(uint avatarId, byte avatarLevel)
        {
            if (Avatars.FightingSpirits.ContainsKey(avatarId))
            {
                return new SoccerAvatar(Avatars.FightingSpirits[avatarId], avatarLevel);
            }

            if (Avatars.Totems.ContainsKey(avatarId))
            {
                return new SoccerAvatar(Avatars.Totems[avatarId], 1);
            }

            return null;
        }

        private static uint GetAvatarKey(SoccerAvatar avatar)
        {
            if (Avatars.FightingSpirits.FirstOrDefault(x => x.Value == avatar.Avatar).Key != 0x00)
            {
                return Avatars.FightingSpirits.FirstOrDefault(x => x.Value == avatar.Avatar).Key;
            }

            if (Avatars.Totems.FirstOrDefault(x => x.Value == avatar.Avatar).Key != 0x00)
            {
                return Avatars.Totems.FirstOrDefault(x => x.Value == avatar.Avatar).Key;
            }

            return 0x00;
        }

        public SoccerCharaConfig(DataReader reader)
        {
            reader.Skip(0x3C);
            int playerCount = reader.ReadByte();
            reader.Skip(0x03);

            for (int i = 0; i < playerCount; i++)
            {
                reader.Skip(0x08);

                uint charaparamID = reader.ReadUInt32();
                Players.Add(GetPlayer(charaparamID));
                reader.Skip(0x14);

                SoccerMove[] moves = new SoccerMove[6];
                for (int s = 0; s < 6; s++)
                {
                    uint moveID = reader.ReadUInt32();
                    byte moveLevel = reader.ReadByte();
                    reader.Skip(0x03);

                    if (moveID != 0x00)
                    {
                        moves[s] = new SoccerMove(Moves.PlayerMoves[moveID], moveLevel);
                    } 
                    else
                    {
                        moves[s] = null;
                    }
                }

                if (Players[i] != null)
                {
                    Players[i].Moves = moves;
                }
                
                uint takeFourBytes = reader.ReadUInt32();
                while (takeFourBytes != 0xEEA96EEA && takeFourBytes != 0x18CCE768)
                {
                    // Avatar Entry
                    if (takeFourBytes == 0x7499DA26)
                    {
                        reader.Skip(0x04);
                        uint avatarID = reader.ReadUInt32();
                        byte avatarLevel = reader.ReadByte();
                        reader.Skip(0x03);

                        if (Players[i] != null)
                        {
                            Players[i].Avatar = GetAvatar(avatarID, avatarLevel);
                        }

                        takeFourBytes = reader.ReadUInt32();
                    }

                    // Miximax Entry
                    if (takeFourBytes == 0xE912CEED)
                    {
                        reader.Skip(0x08);
                        uint miximaxID = reader.ReadUInt32();
                        SoccerMove[] miximaxMoves = new SoccerMove[2];
                        for (int s = 0; s < 2; s++)
                        {
                            uint moveID = reader.ReadUInt32();
                            byte moveLevel = reader.ReadByte();
                            reader.Skip(0x03);

                            if (moveID != 0x00)
                            {
                                miximaxMoves[s] = new SoccerMove(Moves.PlayerMoves[moveID], moveLevel);
                            }
                            else
                            {
                                miximaxMoves[s] = null;
                            }
                        }

                        if (Players[i] != null)
                        {
                            Players[i].MixiMax = GetPlayer(miximaxID);

                            if (Players[i].MixiMax != null)
                            {
                                Players[i].MixiMax.Moves = miximaxMoves;
                            }
                        }

                        takeFourBytes = reader.ReadUInt32();
                    }
                }

                reader.Seek((uint)reader.BaseStream.Position - 4);
            }
        }

        public void Write(string path)
        {
            int playerBlock = 0;
            for (int i = 0; i < Players.Count; i ++)
            {
                playerBlock += 80; 

                if (Players[i].Avatar != null)
                {
                    playerBlock += 16;
                }

                if (Players[i].MixiMax != null)
                {
                    playerBlock += 32;
                }
            }

            DataReader soccerCharaReader = new DataReader(File.ReadAllBytes(path));
            Int32 unknownHeader = soccerCharaReader.ReadInt32();
            Int32 size = soccerCharaReader.ReadInt32();

            int emptyBlock = 16 - (64 + playerBlock + 5) % 16;
            byte[] outputBlock = new byte[64 + playerBlock + 5 + emptyBlock + soccerCharaReader.Length-size];

            DataWriter outputWrite = new DataWriter(outputBlock);
            outputWrite.WriteInt32(unknownHeader);
            outputWrite.WriteInt32(64 + playerBlock + 5 + emptyBlock);
            outputWrite.Write(soccerCharaReader.GetSection(0x08, 0x38));

            for (int i = 0; i < Players.Count; i++)
            {
                outputWrite.Write(WritePlayer(Players[i]));
            }

            outputWrite.Write(new byte[5] { 0x68, 0xE7, 0xCC, 0x18, 0x00 });
            for (int i = 0; i < emptyBlock; i++)
            {
                outputWrite.WriteByte(0xFF);
            }

            outputWrite.Write(soccerCharaReader.GetSection((uint)size, (int) soccerCharaReader.Length-size));
            File.WriteAllBytes(path, outputBlock);
        }

        private byte[] WriteMoves(SoccerMove[] moves)
        {
            byte[] outputBlock = new byte[moves.Length * 8];

            DataWriter outputWrite = new DataWriter(outputBlock);

            for (int i = 0; i < moves.Count(); i++)
            {
                if (moves[i] != null)
                {
                    outputWrite.WriteUInt32(Moves.PlayerMoves.FirstOrDefault(x => x.Value == moves[i].Move).Key);
                    outputWrite.WriteInt32(moves[i].Level);
                } else
                {
                    outputWrite.WriteInt32(0x0);
                    outputWrite.WriteInt32(0x0);
                }
                    
            }

            return outputBlock;
        }

        public byte[] WritePlayer(SoccerPlayer player)
        {
            int outputLength = 80;

            if (player.Avatar != null)
            {
                outputLength += 16;
            }

            if (player.MixiMax != null)
            {
                outputLength += 32;
            }

            byte[] outputBlock = new byte[outputLength];

            DataWriter outputWrite = new DataWriter(outputBlock);
            outputWrite.Write(new byte[8] {0xEA, 0x6E, 0xA9, 0xEE, 0x04, 0x55, 0xFF, 0xFF});
            outputWrite.WriteUInt32(GetPlayerKey(player));
            outputWrite.WriteInt32(0x0);
            outputWrite.Write(new byte[16] { 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x54, 0x5B, 0x0C, 0x55, 0x55, 0x55 });
            outputWrite.Write(WriteMoves(player.Moves));

            if (player.Avatar != null)
            {
                outputWrite.Write(new byte[8] { 0x26, 0xDA, 0x99, 0x74, 0x02, 0x05, 0xFF, 0xFF });
                outputWrite.WriteUInt32(GetAvatarKey(player.Avatar));
                outputWrite.WriteInt32(player.Avatar.Level);
            }

            if (player.MixiMax != null)
            {
                outputWrite.Write(new byte[12] { 0xED, 0xCE, 0x12, 0xE9, 0x06, 0x55, 0x05, 0xEF, 0x01, 0x00, 0x00, 0x00 });
                outputWrite.WriteUInt32(GetPlayerKey(player.MixiMax));
                outputWrite.Write(WriteMoves(player.MixiMax.Moves));
            }

            return outputBlock;
        }
    }
}
