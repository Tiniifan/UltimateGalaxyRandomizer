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
        private void WritePlayers()
        {
            // Initialise Data Writer
            DataWriter charabaseWriter = new DataWriter(Directory + "/ie6_a_fa/gds_pack_decomp_pck/chara_base_0.02.cfg.bin.nat");
            DataWriter charaparamWriter = new DataWriter(Directory + "/ie6_a_fa/gds_pack_decomp_pck/chara_param_0.03.cfg.bin.nat");
            DataWriter skilltableWriter = new DataWriter(Directory + "/ie6_a_fa/gds_pack_decomp_pck/skill_table_0.01.cfg.bin.nat");

            // Merge Player Dictionary to one list
            List<Player> players = new List<Player>();
            players.AddRange(Players.Story.Select(x => x.Value).ToList());
            players.AddRange(Players.Normal.Select(x => x.Value).ToList());
            players.AddRange(Players.Scout.Select(x => x.Value).ToList());

            // Write Player Data
            for (int i = 0; i < players.Count; i++)
            {
                players[i].Base.Write(charabaseWriter);
                players[i].Param.Write(charaparamWriter);
                skilltableWriter.Seek((uint)(4 + players[i].Param.SkillOffset * 8));
                for (int s = 0; s < players[i].Param.SkillCount; s++)
                {
                    players[i].Skills[s].Write(skilltableWriter);
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

                    if (File.Exists(Directory + "/ie6_b_fa/data/img/bustup/face/cp" + oldFileName + "a.xi"))
                        File.Copy(Directory + "/ie6_b_fa/data/img/bustup/face/cp" + oldFileName + "a.xi", Directory + "/ie6_b_fa/temp/cp" + newFileName + "a.xi");

                    if (File.Exists(Directory + "/ie6_b_fa/data/img/mini_xb/cp" + oldFileName + "m.xi"))
                        File.Copy(Directory + "/ie6_b_fa/data/img/mini_xb/cp" + oldFileName + "m.xi", Directory + "/ie6_b_fa/temp/cp" + newFileName + "m.xi");

                    if (File.Exists(Directory + "/ie6_b_fa/data/chr/model/waza/face/cp" + oldFileName + "a.xc"))
                        File.Copy(Directory + "/ie6_b_fa/data/chr/model/waza/face/cp" + oldFileName + "a.xc", Directory + "/ie6_b_fa/temp/cp" + newFileName + "a.xc");

                    if (File.Exists(Directory + "/ie6_b_fa/data/chr/model/rpg/face/cp" + oldFileName + "m.xc"))
                        File.Copy(Directory + "/ie6_b_fa/data/chr/model/rpg/face/cp" + oldFileName + "m.xc", Directory + Directory + "/ie6_b_fa/temp/cp" + newFileName + "m.xc");
                }

                // Moves file to right path
                for (int i = 0; i < players.Count; i++)
                {
                    string newFileName = players[i].Base.HeadIDSwap.ToString().PadLeft(4, '0');

                    File.Copy(Directory + "/ie6_b_fa/temp/cp" + newFileName + "a.xi", Directory + "/ie6_b_fa/data/img/bustup/face/cp" + newFileName + "a.xi");

                    File.Copy(Directory + "/ie6_b_fa/temp/cp" + newFileName + "m.xi", Directory + "/ie6_b_fa/data/img/mini_xb/cp" + newFileName + "m.xi");

                    File.Copy(Directory + "/ie6_b_fa/temp/cp" + newFileName + "a.xc", Directory + "/ie6_b_fa/data/chr/model/waza/face/cp" + newFileName + "a.xc");

                    File.Copy(Directory + "/ie6_b_fa/temp/cp" + newFileName + "m.xc", Directory + Directory + "/ie6_b_fa/data/chr/model/rpg/face/cp" + newFileName + "m.xc");
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

                if (Moves.PlayerMoves.ContainsKey(equipmentID) == true)
                {
                    Equipments.Boots[equipmentID].Read(itemconfigReader);
                } else if (Moves.PlayerMoves.ContainsKey(equipmentID) == true)
                {
                    Equipments.Gloves[equipmentID].Read(itemconfigReader);
                } else if (Moves.PlayerMoves.ContainsKey(equipmentID) == true)
                {
                    Equipments.Pendants[equipmentID].Read(itemconfigReader);
                } else if (Moves.PlayerMoves.ContainsKey(equipmentID) == true)
                {
                    Equipments.Bracelets[equipmentID].Read(itemconfigReader);
                } else
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
