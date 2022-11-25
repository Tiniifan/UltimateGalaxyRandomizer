using System;
using System.Linq;
using System.Collections.Generic;
using UltimateGalaxyRandomizer.Tools;
using UltimateGalaxyRandomizer.Resources;

namespace UltimateGalaxyRandomizer.Logic
{
    public class SoccerCharaConfig
    {
        List<SoccerPlayer> Players = new List<SoccerPlayer>();

        public SoccerCharaConfig(DataReader reader)
        {
            reader.Skip(0x3C);
            int playerCount = reader.ReadByte();
            reader.Skip(0x03);

            for (int i = 0; i < playerCount; i++)
            {
                UInt32 charaparamID = reader.ReadUInt32();
                // Players.Add(new SoccerPlayer(Resources.Players.Player.FirstOrDefault(x => x.Param.ParamID == charaparamID)));
                reader.Skip(0x14);

                SoccerMove[] moves = new SoccerMove[6];
                for (int s = 0; s < 6; s++)
                {
                    UInt32 moveID = reader.ReadUInt32();
                    byte moveLevel = reader.ReadByte();
                    reader.Skip(0x03);
                    moves[s] = new SoccerMove(Moves.PlayerMoves[moveID], moveLevel);
                }

                Players[i].Moves = moves;

                UInt32 takeFourBytes = reader.ReadUInt32();
                while (takeFourBytes != 0xEEA96EEA || takeFourBytes != 0x18CCE768)
                {
                    // Avatar Entry
                    if (takeFourBytes == 0x7499DA26)
                    {
                        reader.Skip(0x08);
                        UInt32 avatarID = reader.ReadUInt32();
                        byte avatarLevel = reader.ReadByte();
                        reader.Skip(0x03);

                        // Try Avatar
                        //FightingSpirit tryFightingSpirit = Avatars.FightingSpirits.FirstOrDefault(x => x.AvatarID == avatarID);
                        //Totem tryTotem = Avatars.Totems.FirstOrDefault(x => x.AvatarID == avatarID);

                        //if (tryFightingSpirit != null)
                        //{
                          //  Players[i].Avatar = new SoccerAvatar(tryFightingSpirit, avatarLevel);
                        //} else if (tryTotem != null)
                        //{
                            //Players[i].Avatar = new SoccerAvatar(tryTotem, 1);
                        //} else
                        //{
                            //Players[i].Avatar = new SoccerAvatar(0x00, 0);
                        //}
                    }

                    // Miximax Entry
                    if (takeFourBytes == 0xE912CEED)
                    {
                        reader.Skip(0x0C);
                        UInt32 miximaxID = reader.ReadUInt32();
                        SoccerMove[] miximaxMoves = new SoccerMove[2];
                        for (int s = 0; s < 2; s++)
                        {
                            UInt32 moveID = reader.ReadUInt32();
                            byte moveLevel = reader.ReadByte();
                            reader.Skip(0x03);
                            miximaxMoves[s] = new SoccerMove(Moves.PlayerMoves[moveID], moveLevel);
                        }

                        // Players[i].MixiMax = new SoccerPlayer(Resources.Players.Player.FirstOrDefault(x => x.Param.ParamID == charaparamID), miximaxMoves);
                    }
                }
            }
        }
    }
}
