using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UltimateGalaxyRandomizer.Logic;
using UltimateGalaxyRandomizer.Tools;
using UltimateGalaxyRandomizer.Resources;
using UltimateGalaxyRandomizer.Randomizer;
using UltimateGalaxyRandomizer.Randomizer.Utility;

namespace UltimateGalaxyRandomizer.Randomizer
{
    public class Galaxy
    {
        public string Name = "Inazuma Eleven Go Galaxy";

        public string Directory { get; set; }

        private void ReadPlayers()
        {
            // Initialise File Reader
            DataReader charabaseReader = new DataReader(File.ReadAllBytes(Directory + "/ie6_a_fa/gds_pack_decomp_pck/chara_base_0.02.cfg.bin.nat"));
            DataReader charaparamReader = new DataReader(File.ReadAllBytes(Directory + "/ie6_a_fa/gds_pack_decomp_pck/chara_param_0.03.cfg.bin.nat"));
            DataReader skilltableReader = new DataReader(File.ReadAllBytes(Directory + "/ie6_a_fa/gds_pack_decomp_pck/skill_table_0.01.cfg.bin.nat"));

            // Load Player Table
            charabaseReader.Skip(0x04);
            Int32 playerCount = charaparamReader.ReadInt32();
            Int32 avatarCount = charaparamReader.ReadInt32();

            // Read Avatar Table
            charaparamReader.Seek((uint)(playerCount * 0x28 + 0x0C));
            for (int i = 0; i < avatarCount; i++)
            {
                Avatars.Table.Add(i, charaparamReader.ReadUInt32());
            }

            // Read Player Data
            charaparamReader.Seek(0x0C);
            for (int i = 0; i < playerCount; i++)
            {
                Player player = null;
                UInt32 playerID = charaparamReader.ReadUInt32();
                
                // Check Player Status
                if (Players.Story.ContainsKey(playerID) == true)
                {
                    player = Players.Story[playerID];
                }
                else if (Players.Normal.ContainsKey(playerID) == true)
                {
                    player = Players.Normal[playerID];
                }
                else if (Players.Scout.ContainsKey(playerID) == true)
                {
                    player = Players.Scout[playerID];
                }

                // Link Parameter and Base
                player.Param = new Param(charaparamReader);
                player.Base = new Base(charabaseReader);

                // Link Moveset
                player.Skills = new SkillTable[player.Param.SkillCount];
                skilltableReader.Seek((uint)(4 + player.Param.SkillOffset * 8));
                for (int s = 0; s < player.Param.SkillCount; s++)
                {
                    player.Skills[s] = new SkillTable(skilltableReader);
                }
            }

            // Close Stream
            charabaseReader.Close();
            charaparamReader.Close();
            skilltableReader.Close();
        }
        private void FixPlayer(KeyValuePair<UInt32, Player> player)
        {
            // Jean Pierre Lapin
            if (player.Key == 0x960E2CA3)
            {
                player.Value.Skills[3].LearnAtLevel = 0x64;
            }
            else if (player.Key == 0xE8BF501E)
            {
                player.Value.Skills[0].SkillID = Moves.PlayerMoves.Where(x => x.Value.TP < 30 && x.Value.Position == 2).Select(x => x.Key).FirstOrDefault();
                player.Value.Skills[0].LearnAtLevel = 0x00;
                player.Value.Skills[3].LearnAtLevel = 0x64;
            }
            else if (player.Key == 0x9FB86088)
            {
                player.Value.Skills[3].LearnAtLevel = 0x64;
            }
            else if (player.Key == 0xFF7FE96D)
            {
                player.Value.Skills[0].SkillID = Moves.PlayerMoves.Where(x => x.Value.TP < 30 && x.Value.Position == 1).Select(x => x.Key).FirstOrDefault();
                player.Value.Skills[0].LearnAtLevel = 0x00;
                player.Value.Skills[3].LearnAtLevel = 0x64;
            }
            else if (player.Key == 0x83D64754)
            {
                player.Value.Skills[1].LearnAtLevel = 0x64;
                player.Value.Skills[2].LearnAtLevel = 0x64;
            }
            else if (player.Key == 0x1ADF16EE)
            {
                player.Value.Skills[2].LearnAtLevel = 0x64;
            }
            else if (player.Key == 0x6DD82678)
            {
                player.Value.Skills[2].LearnAtLevel = 0x64;
            }
            else if (player.Key == 0xF3BCB3DB)
            {
                player.Value.Skills[2].LearnAtLevel = 0x64;
            }
            else if (player.Key == 0x84BB834D)
            {
                player.Value.Skills[2].LearnAtLevel = 0x64;
            }
            else if (player.Key == 0x1DB2D2F7)
            {
                player.Value.Skills[2].LearnAtLevel = 0x64;
            }
            else if (player.Key == 0x9ACD7615)
            {
                player.Value.Skills[1].LearnAtLevel = 0x64;
                player.Value.Skills[3].LearnAtLevel = 0x64;
            } 
            else
            {
                for (int s = 0; s < player.Value.Param.SkillCount; s++)
                {
                    if (player.Value.Skills[s].LearnAtLevel == 0x64)
                    {
                        player.Value.Skills[s].LearnAtLevel = 0x1E;
                    }
                }
            }

        }
        private void WritePlayers()
        {
            // Initialise Data Writer
            DataWriter charabaseWriter = new DataWriter(Directory + "/ie6_a_fa/gds_pack_decomp_pck/chara_base_0.02.cfg.bin.nat");
            DataWriter charaparamWriter = new DataWriter(Directory + "/ie6_a_fa/gds_pack_decomp_pck/chara_param_0.03.cfg.bin.nat");
            DataWriter skilltableWriter = new DataWriter(Directory + "/ie6_a_fa/gds_pack_decomp_pck/skill_table_0.01.cfg.bin.nat");

            // Merge Player Dictionaries to one
            Dictionary<UInt32, Player> players = new Dictionary<UInt32, Player>();
            Players.Story.ToList().ForEach(x => players.Add(x.Key, x.Value));
            Players.Normal.ToList().ForEach(x => players.Add(x.Key, x.Value));
            Players.Scout.ToList().ForEach(x => players.Add(x.Key, x.Value));

            // Write Player Data
            foreach(KeyValuePair<UInt32, Player> player in players)
            {
                player.Value.Base.Write(charabaseWriter);
                player.Value.Param.Write(charaparamWriter);
                skilltableWriter.Seek((uint)(4 + player.Value.Param.SkillOffset * 8));
                FixPlayer(player);
                for (int s = 0; s < player.Value.Param.SkillCount; s++)
                {
                    player.Value.Skills[s].Write(skilltableWriter);
                }
            }

            // Close Stream
            charabaseWriter.Close();
            charaparamWriter.Close();
            skilltableWriter.Close();
        }
        public void RandomizePlayers(Dictionary<string, Option> options)
        {
            // Call Function From Randomizer.cs class
            Randomizer.RandomizePlayers(options);

            // Save
            WritePlayers();

            // Fix Model Bug
            // Merge Player Dictionaries to one list
            if (options["groupBoxSwapPlayer"].Name == "Random")
            {
                List<Player> players = new List<Player>();
                players.AddRange(Players.Story.Values.ToList());
                players.AddRange(Players.Normal.Values.ToList());
                players.AddRange(Players.Scout.Values.ToList());

                // Create Temp directory
                System.IO.Directory.CreateDirectory(Directory + "/ie6_b_fa/temp/");

                // Move files to temp folder to rename them
                for (int i = 0; i < players.Count; i++)
                {
                    string oldFileName = players[i].Base.HeadID.ToString().PadLeft(4, '0'); ;
                    string newFileName = players[i].Base.HeadIDSwap.ToString().PadLeft(4, '0');

                    if (File.Exists(Directory + "/ie6_b_fa/data/img/bustup/face/cp" + newFileName + "a.xi"))
                        File.Move(Directory + "/ie6_b_fa/data/img/bustup/face/cp" + newFileName + "a.xi", Directory + "/ie6_b_fa/temp/cp" + oldFileName + "a.xi");

                    if (File.Exists(Directory + "/ie6_b_fa/data/img/mini_xb/cp" + newFileName + "m.xi"))
                        File.Move(Directory + "/ie6_b_fa/data/img/mini_xb/cp" + newFileName + "m.xi", Directory + "/ie6_b_fa/temp/cp" + oldFileName + "m.xi");

                    if (File.Exists(Directory + "/ie6_b_fa/data/chr/model/waza/face/cp" + newFileName + "a.xc"))
                        File.Move(Directory + "/ie6_b_fa/data/chr/model/waza/face/cp" + newFileName + "a.xc", Directory + "/ie6_b_fa/temp/cp" + oldFileName + "a.xc");

                    if (File.Exists(Directory + "/ie6_b_fa/data/chr/model/rpg/face/cp" + newFileName + "m.xc"))
                        File.Move(Directory + "/ie6_b_fa/data/chr/model/rpg/face/cp" + newFileName + "m.xc", Directory + "/ie6_b_fa/temp/cp" + oldFileName + "m.xc");
                }

                // Moves file to right path
                for (int i = 0; i < players.Count; i++)
                {
                    string newFileName = players[i].Base.HeadIDSwap.ToString().PadLeft(4, '0');

                    if (File.Exists(Directory + "/ie6_b_fa/temp/cp" + newFileName + "a.xi"))
                        File.Move(Directory + "/ie6_b_fa/temp/cp" + newFileName + "a.xi", Directory + "/ie6_b_fa/data/img/bustup/face/cp" + newFileName + "a.xi");

                    if (File.Exists(Directory + "/ie6_b_fa/temp/cp" + newFileName + "m.xi"))
                        File.Move(Directory + "/ie6_b_fa/temp/cp" + newFileName + "m.xi", Directory + "/ie6_b_fa/data/img/mini_xb/cp" + newFileName + "m.xi");

                    if (File.Exists(Directory + "/ie6_b_fa/temp/cp" + newFileName + "a.xc"))
                        File.Move(Directory + "/ie6_b_fa/temp/cp" + newFileName + "a.xc", Directory + "/ie6_b_fa/data/chr/model/waza/face/cp" + newFileName + "a.xc");

                    if (File.Exists(Directory + "/ie6_b_fa/temp/cp" + newFileName + "m.xc"))
                        File.Move(Directory + "/ie6_b_fa/temp/cp" + newFileName + "m.xc", Directory + "/ie6_b_fa/data/chr/model/rpg/face/cp" + newFileName + "m.xc");
                }
            }
        }

        private void ReadMoves()
        {
            // Initialise File Reader
            DataReader skillconfigReader = new DataReader(File.ReadAllBytes(Directory + "/ie6_a_fa/gds_pack_decomp_pck/skill_config_0.29d.cfg.bin.nat"));

            int skillCount = skillconfigReader.ReadInt32();

            // Read Move Data
            skillconfigReader.Seek(0x14);
            for (int i = 0; i < skillCount; i++)
            {
                UInt32 moveID = skillconfigReader.ReadUInt32();

                if (Moves.PlayerMoves.ContainsKey(moveID) == true)
                {
                    Moves.PlayerMoves[moveID].Read(skillconfigReader);
                }
                else if (Moves.FightingSpiritMoves.ContainsKey(moveID) == true)
                {
                    Moves.FightingSpiritMoves[moveID].Read(skillconfigReader);
                }
                else if (Moves.TotemMoves.ContainsKey(moveID) == true)
                {
                    Moves.TotemMoves[moveID].Read(skillconfigReader);
                } 
                else
                {
                    skillconfigReader.Skip(0x28);
                }
            }

            // Read Move Ultimate Data
            skillconfigReader.Seek(0x7504);
            for (int i = 0; i < Moves.MovesUltimate.Count; i++)
            {
                Moves.MovesUltimate[i].Read(skillconfigReader);
            }

            // Close Stream
            skillconfigReader.Close();
        }
        private void WriteMoves()
        {
            // Initialise File Writer
            DataWriter skillConfigWriter = new DataWriter(Directory + "/ie6_a_fa/gds_pack_decomp_pck/skill_config_0.29d.cfg.bin.nat");

            // Merge Move Dictionary to one list
            List<Move> moves = new List<Move>();
            moves.AddRange(Moves.PlayerMoves.Select(x => x.Value).ToList());
            moves.AddRange(Moves.FightingSpiritMoves.Select(x => x.Value).ToList());
            moves.AddRange(Moves.TotemMoves.Select(x => x.Value).ToList());

            // Write Move
            for (int i = 0; i < moves.Count; i++)
            {
                moves[i].Write(skillConfigWriter);
            }

            // Write Moves Ultimate
            for (int i = 0; i < Moves.MovesUltimate.Count; i++)
            {
                Moves.MovesUltimate[i].Write(skillConfigWriter);
            }

            // Close Stream
            skillConfigWriter.Close();
        }
        public void RandomizeMoves(Dictionary<string, Option> options)
        {
            // Call Function From Randomizer.cs class
            Randomizer.RandomizeMoves(options);

            // Save
            WriteMoves();
        }

        private void ReadAvatars()
        {
            // Initialise File Reader
            DataReader itemconfigReader = new DataReader(File.ReadAllBytes(Directory + "/ie6_a_fa/gds_pack_decomp_pck/item_config_0.08a.cfg.bin.nat"));

            // Fighting Spirit
            itemconfigReader.Seek(0x2BC24);
            for (int i = 0; i < Avatars.FightingSpirits.Count; i++)
            {
                UInt32 avatarID = itemconfigReader.ReadUInt32();
                Avatars.FightingSpirits[avatarID].Read(itemconfigReader);
            }

            // Totem
            itemconfigReader.Seek(0x2D774);
            for (int i = 0; i < Avatars.Totems.Count; i++)
            {
                UInt32 avatarID = itemconfigReader.ReadUInt32();
                Avatars.Totems[avatarID].Read(itemconfigReader);
            }

            // Close Stream
            itemconfigReader.Close();
        }
        private void WriteAvatars()
        {
            // Initialise File Writer
            DataWriter itemconfigWriter = new DataWriter(Directory + "/ie6_a_fa/gds_pack_decomp_pck/item_config_0.08a.cfg.bin.nat");

            // Fighting Spirit
            itemconfigWriter.Seek(0x2BC24);
            for (int i = 0; i < Avatars.FightingSpirits.Count; i++)
            {
                Avatars.FightingSpirits.ElementAt(i).Value.Write(itemconfigWriter);
            }

            // Totem
            itemconfigWriter.Seek(0x2D774);
            for (int i = 0; i < Avatars.Totems.Count; i++)
            {
                Avatars.Totems.ElementAt(i).Value.Write(itemconfigWriter);
            }

            // Close Stream
            itemconfigWriter.Close();
        }
        public void RandomizeAvatars(Dictionary<string, Option> options)
        {
            // Call Function From Randomizer.cs class
            Randomizer.RandomizeAvatars(options);

            // Save
            WriteAvatars();
        }

        private void ReadEquipments()
        {
            // Initialise File Reader
            DataReader itemconfigReader = new DataReader(File.ReadAllBytes(Directory + "/ie6_a_fa/gds_pack_decomp_pck/item_config_0.08a.cfg.bin.nat"));

            int equipmentCount = itemconfigReader.ReadInt32();

            itemconfigReader.Seek(0x30);
            for (int i = 0; i < equipmentCount; i++)
            {
                UInt32 equipmentID = itemconfigReader.ReadUInt32();

                if (Equipments.Boots.ContainsKey(equipmentID) == true)
                {
                    Equipments.Boots[equipmentID].Read(itemconfigReader);
                } 
                else if (Equipments.Gloves.ContainsKey(equipmentID) == true)
                {
                    Equipments.Gloves[equipmentID].Read(itemconfigReader);
                }
                else if (Equipments.Bracelets.ContainsKey(equipmentID) == true)
                {
                    Equipments.Bracelets[equipmentID].Read(itemconfigReader);
                }
                else if (Equipments.Pendants.ContainsKey(equipmentID) == true)
                {
                    Equipments.Pendants[equipmentID].Read(itemconfigReader);
                } 
                else
                {
                    itemconfigReader.Skip(0x2C);
                }
            }

            // Close Stream
            itemconfigReader.Close();
        }
        private void WriteEquipments()
        {
            // Initialise File Writer
            DataWriter itemconfigWriter = new DataWriter(Directory + "/ie6_a_fa/gds_pack_decomp_pck/item_config_0.08a.cfg.bin.nat");

            // Merge Equipment Dictionary to one list
            List<Equipment> equipments = new List<Equipment>();
            equipments.AddRange(Equipments.Boots.Select(x => x.Value).ToList());
            equipments.AddRange(Equipments.Gloves.Select(x => x.Value).ToList());
            equipments.AddRange(Equipments.Pendants.Select(x => x.Value).ToList());
            equipments.AddRange(Equipments.Bracelets.Select(x => x.Value).ToList());

            // Write Equipment Data
            for (int i = 0; i < equipments.Count; i++)
            {
                equipments[i].Write(itemconfigWriter);
            }

            // Close Stream
            itemconfigWriter.Close();
        }

        private void ReadSoccer(Team team)
        {
            string soccerFile = team.ScriptID.ToString().PadLeft(4, '0');

            if (File.Exists(Directory + "/ie6_b_fa/data/res/soccer/soccer_chara_btl" + soccerFile + ".cfg.bin"))
            {
                DataReader soccerCharaReader = new DataReader(File.ReadAllBytes(Directory + "/ie6_b_fa/data/res/soccer/soccer_chara_btl" + soccerFile + ".cfg.bin"));

                team.SoccerChara = new SoccerCharaConfig(soccerCharaReader);


                soccerCharaReader.Close();
            }
        }
        private void WriteSoccer(Team team)
        {
            string soccerFile = team.ScriptID.ToString().PadLeft(4, '0');

            if (File.Exists(Directory + "/ie6_b_fa/data/res/soccer/soccer_chara_btl" + soccerFile + ".cfg.bin"))
            {
                SoccerCharaConfig teamSoccerChara = team.SoccerChara;

                // Fix Scripted Player
                for (int i = 0; i < teamSoccerChara.Players.Count; i++)
                {
                    // Update Moveset
                    for (int m = 0; m < 4; m++)
                    {
                        // Learn skill
                        if (teamSoccerChara.Players[i].Player.Skills[m].LearnAtLevel < team.Param.Level)
                        {
                            teamSoccerChara.Players[i].Moves[m] = new SoccerMove(Moves.PlayerMoves[teamSoccerChara.Players[i].Player.Skills[m].SkillID], 1);
                        } else
                        {
                            teamSoccerChara.Players[i].Moves[m] = null;
                        }
                    }

                    // Update Avatar
                    if (teamSoccerChara.Players[i].Player.Param.Avatar != 0x0)
                    {
                        if (Avatars.FightingSpirits.ContainsKey(teamSoccerChara.Players[i].Player.Param.Avatar))
                        {
                            teamSoccerChara.Players[i].Avatar = new SoccerAvatar(Avatars.FightingSpirits[teamSoccerChara.Players[i].Player.Param.Avatar], 1);
                        }
                        else if (Avatars.Totems.ContainsKey(teamSoccerChara.Players[i].Player.Param.Avatar))
                        {
                            teamSoccerChara.Players[i].Avatar = new SoccerAvatar(Avatars.Totems[teamSoccerChara.Players[i].Player.Param.Avatar], 1);
                        }
                        
                    }
                }

                team.SoccerChara.Write(Directory + "/ie6_b_fa/data/res/soccer/soccer_chara_btl" + soccerFile + ".cfg.bin");
            }
        }
        private void ReadTeams()
        {
            // Initialise File Reader
            DataReader soccerconfigReader = new DataReader(File.ReadAllBytes(Directory + "/ie6_b_fa/data/res/soccer/soccer_config_0.01.cfg.bin"));
            DataReader teamParamReader = new DataReader(File.ReadAllBytes(Directory + "/ie6_b_fa/data/res/team/team_param.cfg.bin"));

            // Read Team Config
            soccerconfigReader.Seek(0x3C);
            int teamCount = soccerconfigReader.ReadInt32();
            for (int i = 0; i < teamCount; i++)
            {
                soccerconfigReader.Skip(0x0C);
                UInt32 teamID = soccerconfigReader.ReadUInt32();

                if (Teams.Story.ContainsKey(teamID) == true)
                {
                    Teams.Story[teamID].Read(soccerconfigReader);
                }
                else if (Teams.Battle.ContainsKey(teamID) == true)
                {
                    Teams.Battle[teamID].Read(soccerconfigReader);
                }
                else if (Teams.TaisenRoad.ContainsKey(teamID) == true)
                {
                    Teams.TaisenRoad[teamID].Read(soccerconfigReader);
                }
                else if (Teams.LegendGate.ContainsKey(teamID) == true)
                {
                    Teams.LegendGate[teamID].Read(soccerconfigReader);
                }
                else
                {
                    soccerconfigReader.Skip(0x38);
                }
            }

            // Read Team Param
            teamParamReader.Seek(0x3C);
            teamCount = teamParamReader.ReadInt32();
            for (int i = 0; i < teamCount; i++)
            {
                long tempPosition = teamParamReader.BaseStream.Position;

                teamParamReader.Skip(0x10);
                UInt32 teamID = teamParamReader.ReadUInt32();

                // Search if Team Param ID exists 
                KeyValuePair<UInt32, Team> tryStory = Teams.Story.FirstOrDefault(x => x.Value.TeamParamID == teamID);
                KeyValuePair<UInt32, Team> tryBattle = Teams.Battle.FirstOrDefault(x => x.Value.TeamParamID == teamID);
                KeyValuePair<UInt32, Team> tryTaisenRoad = Teams.TaisenRoad.FirstOrDefault(x => x.Value.TeamParamID == teamID);
                KeyValuePair<UInt32, Team> tryLegendGate = Teams.LegendGate.FirstOrDefault(x => x.Value.TeamParamID == teamID);

                teamParamReader.Seek((uint)tempPosition);

                // Link Team Param With Team Config
                if (tryStory.Value != null)
                {
                    tryStory.Value.Param = new TeamParam(teamParamReader);

                    // Temporary fix
                    if (tryStory.Value.ScriptID != 0x0C)
                    {
                        ReadSoccer(tryStory.Value);
                    }
                } 
                else if (tryBattle.Value != null)
                {
                    tryBattle.Value.Param = new TeamParam(teamParamReader);
                }
                else if (tryTaisenRoad.Value != null)
                {
                    tryTaisenRoad.Value.Param = new TeamParam(teamParamReader);
                }
                else if (tryLegendGate.Value != null)
                {
                    tryLegendGate.Value.Param = new TeamParam(teamParamReader);
                } 
                else
                {
                    teamParamReader.Skip(0x88);
                }

            }
        }
        private void WriteTeams()
        {
            // Initialise File Writer
            DataWriter soccerconfigWriter = new DataWriter(Directory + "/ie6_b_fa/data/res/soccer/soccer_config_0.01.cfg.bin");
            DataWriter teamParamWriter = new DataWriter(Directory + "/ie6_b_fa/data/res/team/team_param.cfg.bin");

            // Merge Teams Dictionaries to one
            Dictionary<UInt32, Team> teams = new Dictionary<UInt32, Team>();
            Teams.Story.ToList().ForEach(x => teams.Add(x.Key, x.Value));
            Teams.Battle.ToList().ForEach(x => teams.Add(x.Key, x.Value));
            Teams.TaisenRoad.ToList().ForEach(x => teams.Add(x.Key, x.Value));
            Teams.LegendGate.ToList().ForEach(x => teams.Add(x.Key, x.Value));

            foreach (KeyValuePair<UInt32, Team> KeyValuePairTeam in teams)
            {
                KeyValuePairTeam.Value.Write(soccerconfigWriter);

                if (KeyValuePairTeam.Value.Param != null)
                {
                    KeyValuePairTeam.Value.Param.Write(teamParamWriter);
                }

                if (KeyValuePairTeam.Value.SoccerChara != null)
                {
                    // Fix Script 
                    if (ScriptSoccers.ScriptSoccerGalaxy.ContainsKey(KeyValuePairTeam.Value.ScriptID))
                    {
                        ScriptSoccer script = ScriptSoccers.ScriptSoccerGalaxy[KeyValuePairTeam.Value.ScriptID];
                        for (int i = 0; i < script.PlayerIndex.Count; i ++)
                        {
                            KeyValuePairTeam.Value.SoccerChara.Players[script.PlayerIndex[i]].Moves[4] = new SoccerMove(Moves.PlayerMoves[script.RightMove], 1);
                        }
                    }

                    WriteSoccer(KeyValuePairTeam.Value);
                }
            }
        }
        public void RandomizeTeams(Dictionary<string, Option> options)
        {
            // Call Function From Randomizer.cs class
            Randomizer.RandomizeTeams(options);

            // Save
            WriteTeams();
        }

        public void Miscellaneous(Dictionary<string, Option> options)
        {
            if (options["groupBoxMiscellaneousShop"].Name == "Random")
            {
                string[] shopDirectory = System.IO.Directory.GetFiles(Directory + "/ie6_b_fa/data/res/shop/");

                foreach (string shopFileName in shopDirectory)
                {
                    // Exclude Gashapon
                    if (Path.GetFileNameWithoutExtension(shopFileName).StartsWith("shop_shp"))
                    {
                        // Call Function From Randomizer.cs class
                        Randomizer.RandomizeShop(shopFileName);
                    }
                }
            }

            if (options["groupBoxMiscellaneousTreasureBox"].Name == "Random")
            {
                // Get All Treasure Box Files
                string[] treasureBoxFolder = System.IO.Directory.GetFiles(Directory + "/ie6_b_fa/data/res/map/");

                // Find All Files Who Contains Treasure Box Entry
                string[] folders = System.IO.Directory.GetDirectories(Directory + "/ie6_b_fa/data/res/map/").Select(Path.GetFileName).ToArray();
                foreach (string folder in folders)
                {
                    if (File.Exists(Directory + "/ie6_b_fa/data/res/map/" + folder + "/" + folder + "_oneplace.cfg.bin"))
                    {
                        // Call Function From Randomizer.cs class
                        Randomizer.RandomizeTreasureBox(Directory + "/ie6_b_fa/data/res/map/" + folder + "/" + folder + "_oneplace.cfg.bin");
                    }
                }
            }

            if (options["groupBoxMiscellaneousRecruitment"].CheckBoxes["checkBoxMiscellaneousRecuitRemove"].Checked == true)
            {
                DataWriter itemconfigWriter = new DataWriter(Directory + "/ie6_a_fa/gds_pack_decomp_pck/item_config_0.08a.cfg.bin.nat");

                itemconfigWriter.Seek(0xEA44);
                for (int i = 0; i < 1988; i++)
                {
                    itemconfigWriter.Skip(0x08);
                    itemconfigWriter.WriteByte(0x0);
                    itemconfigWriter.Skip(0x0B);

                    for (int j = 0; j < 4; j++)
                    {
                        itemconfigWriter.WriteInt32(0x00);
                    }

                    itemconfigWriter.WriteUInt32(0xFFFFFFFF);
                    itemconfigWriter.WriteUInt32(0x00);

                    itemconfigWriter.Skip(0x10);
                }

                itemconfigWriter.Close();
            }

            if (options["groupBoxMiscellaneousEquipment"].Name != "Unchanged")
            {
                // Call Function From Randomizer.cs class
                Randomizer.RandomizeEquipments(options);

                // Save
                WriteEquipments();
            }
        }

        public Galaxy(string folderPath)
        {
            Directory = folderPath;

            ReadMoves();
            ReadAvatars();
            ReadEquipments();
            ReadPlayers();
            ReadTeams();
        }
    }
}
