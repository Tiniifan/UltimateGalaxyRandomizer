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
        public readonly string Name = "Inazuma Eleven Go Galaxy";

        public string Directory { get; set; }

        private void ReadPlayers()
        {
            // Initialise File Reader
            DataReader charaBaseReader = new DataReader(File.ReadAllBytes(Directory + "/ie6_a_fa/gds_pack_decomp_pck/chara_base_0.02.cfg.bin.nat"));
            DataReader charaParamReader = new DataReader(File.ReadAllBytes(Directory + "/ie6_a_fa/gds_pack_decomp_pck/chara_param_0.03.cfg.bin.nat"));
            DataReader skillTableReader = new DataReader(File.ReadAllBytes(Directory + "/ie6_a_fa/gds_pack_decomp_pck/skill_table_0.01.cfg.bin.nat"));

            // Load Player Table
            charaBaseReader.Skip(0x04);
            Int32 playerCount = charaParamReader.ReadInt32();
            Int32 avatarCount = charaParamReader.ReadInt32();

            // Read Avatar Table
            charaParamReader.Seek((uint)(playerCount * 0x28 + 0x0C));
            for (int i = 0; i < avatarCount; i++)
            {
                Avatars.Table.Add(i, charaParamReader.ReadUInt32());
            }

            // Read Player Data
            charaParamReader.Seek(0x0C);
            for (int i = 0; i < playerCount; i++)
            {
                Player player = null;
                var playerId = charaParamReader.ReadUInt32();
                
                // Check Player Status
                if (Players.Story.TryGetValue(playerId, out var storyPlayer))
                {
                    player = storyPlayer;
                }
                else if (Players.Normal.TryGetValue(playerId, out var normalPlayer))
                {
                    player = normalPlayer;
                }
                else if (Players.Scout.TryGetValue(playerId, out var scoutPlayer))
                {
                    player = scoutPlayer;
                }

                if (player == null) continue;

                // Link Parameter and Base
                player.Param = new Param(charaParamReader);
                player.Base = new Base(charaBaseReader);

                // Link Moveset
                player.Skills = new SkillTable[player.Param.SkillCount];
                skillTableReader.Seek((uint)(4 + player.Param.SkillOffset * 8));
                for (int s = 0; s < player.Param.SkillCount; s++)
                {
                    player.Skills[s] = new SkillTable(skillTableReader);
                }
            }

            // Close Stream
            charaBaseReader.Close();
            charaParamReader.Close();
            skillTableReader.Close();
        }
        private void FixPlayer(KeyValuePair<uint, Player> player)
        {
            switch (player.Key)
            {
                // Jean Pierre Lapin
                case 0x960E2CA3:
                    player.Value.Skills[3].LearnAtLevel = 0x64;
                    break;
                case 0xE8BF501E:
                    player.Value.Skills[0].SkillId = Moves.PlayerMoves.Where(x => x.Value.TP < 30 && x.Value.Position == 2).Random().Key;
                    player.Value.Skills[0].LearnAtLevel = 0x00;
                    player.Value.Skills[3].LearnAtLevel = 0x64;
                    break;
                case 0x9FB86088:
                    player.Value.Skills[3].LearnAtLevel = 0x64;
                    break;
                case 0xFF7FE96D:
                    player.Value.Skills[0].SkillId = Moves.PlayerMoves.Where(x => x.Value.TP < 30 && x.Value.Position == 1).Random().Key;
                    player.Value.Skills[0].LearnAtLevel = 0x00;
                    player.Value.Skills[3].LearnAtLevel = 0x64;
                    break;
                case 0x83D64754:
                    player.Value.Skills[1].LearnAtLevel = 0x64;
                    player.Value.Skills[2].LearnAtLevel = 0x64;
                    break;
                case 0x1ADF16EE:
                case 0x6DD82678:
                case 0xF3BCB3DB:
                case 0x84BB834D:
                case 0x1DB2D2F7:
                    player.Value.Skills[2].LearnAtLevel = 0x64;
                    break;
                case 0x9ACD7615:
                    player.Value.Skills[1].LearnAtLevel = 0x64;
                    player.Value.Skills[3].LearnAtLevel = 0x64;
                    break;
                default:
                {
                    for (int s = 0; s < player.Value.Param.SkillCount; s++)
                    {
                        if (player.Value.Skills[s].LearnAtLevel == 0x64)
                        {
                            player.Value.Skills[s].LearnAtLevel = 0x1E;
                        }
                    }

                    break;
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
            Dictionary<uint, Player> players = new Dictionary<uint, Player>();
            Players.Story.ToList().ForEach(x => players.Add(x.Key, x.Value));
            Players.Normal.ToList().ForEach(x => players.Add(x.Key, x.Value));
            Players.Scout.ToList().ForEach(x => players.Add(x.Key, x.Value));

            // Write Player Data
            foreach(KeyValuePair<uint, Player> player in players)
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
            if (options["groupBoxSwapPlayer"].Name != "Random") return;

            List<Player> players = new List<Player>();
            players.AddRange(Players.Story.Values.ToList());
            players.AddRange(Players.Normal.Values.ToList());
            players.AddRange(Players.Scout.Values.ToList());

            // Create Temp directory
            System.IO.Directory.CreateDirectory(Directory + "/ie6_b_fa/temp/");

            // Move files to temp folder to rename them
            foreach (var playerBase in players.Select(p => p.Base))
            {
                string oldFileName = playerBase.HeadID.ToString().PadLeft(4, '0');
                string newFileName = playerBase.HeadIDSwap.ToString().PadLeft(4, '0');

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
            foreach (var newFileName in players.Select(player => player.Base.HeadIDSwap.ToString().PadLeft(4, '0')))
            {
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

        private void ReadMoves()
        {
            // Initialise File Reader
            DataReader skillConfigReader = new DataReader(File.ReadAllBytes(Directory + "/ie6_a_fa/gds_pack_decomp_pck/skill_config_0.29d.cfg.bin.nat"));

            int skillCount = skillConfigReader.ReadInt32();

            // Read Move Data
            skillConfigReader.Seek(0x14);
            for (int i = 0; i < skillCount; i++)
            {
                var moveId = skillConfigReader.ReadUInt32();

                if (Moves.PlayerMoves.ContainsKey(moveId))
                {
                    Moves.PlayerMoves[moveId].Read(skillConfigReader);
                }
                else if (Moves.FightingSpiritMoves.ContainsKey(moveId))
                {
                    Moves.FightingSpiritMoves[moveId].Read(skillConfigReader);
                }
                else if (Moves.TotemMoves.ContainsKey(moveId))
                {
                    Moves.TotemMoves[moveId].Read(skillConfigReader);
                } 
                else
                {
                    skillConfigReader.Skip(0x28);
                }
            }

            // Read Move Ultimate Data
            skillConfigReader.Seek(0x7504);
            foreach (var move in Moves.MovesUltimate)
            {
                move.Read(skillConfigReader);
            }

            // Close Stream
            skillConfigReader.Close();
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
            foreach (var move in moves) move.Write(skillConfigWriter);

            // Write Moves Ultimate
            foreach (var move in Moves.MovesUltimate) move.Write(skillConfigWriter);

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
            foreach (var avatarId in Avatars.FightingSpirits.Select(t => itemconfigReader.ReadUInt32()))
            {
                Avatars.FightingSpirits[avatarId].Read(itemconfigReader);
            }

            // Totem
            itemconfigReader.Seek(0x2D774);
            foreach (var avatarId in Avatars.Totems.Select(totem => itemconfigReader.ReadUInt32()))
            {
                Avatars.Totems[avatarId].Read(itemconfigReader);
            }

            // Close Stream
            itemconfigReader.Close();
        }
        private void WriteAvatars()
        {
            // Initialise File Writer
            DataWriter itemConfigWriter = new DataWriter(Directory + "/ie6_a_fa/gds_pack_decomp_pck/item_config_0.08a.cfg.bin.nat");

            // Fighting Spirit
            itemConfigWriter.Seek(0x2BC24);
            foreach (var spirit in Avatars.FightingSpirits.Values) spirit.Write(itemConfigWriter);

            // Totem
            itemConfigWriter.Seek(0x2D774);
            foreach (var totem in Avatars.Totems.Values) totem.Write(itemConfigWriter);

            // Close Stream
            itemConfigWriter.Close();
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
            DataReader itemConfigReader = new DataReader(File.ReadAllBytes(Directory + "/ie6_a_fa/gds_pack_decomp_pck/item_config_0.08a.cfg.bin.nat"));

            int equipmentCount = itemConfigReader.ReadInt32();

            itemConfigReader.Seek(0x30);
            for (int i = 0; i < equipmentCount; i++)
            {
                uint equipmentId = itemConfigReader.ReadUInt32();

                if (Equipments.Boots.ContainsKey(equipmentId))
                {
                    Equipments.Boots[equipmentId].Read(itemConfigReader);
                } 
                else if (Equipments.Gloves.ContainsKey(equipmentId))
                {
                    Equipments.Gloves[equipmentId].Read(itemConfigReader);
                }
                else if (Equipments.Bracelets.ContainsKey(equipmentId))
                {
                    Equipments.Bracelets[equipmentId].Read(itemConfigReader);
                }
                else if (Equipments.Pendants.ContainsKey(equipmentId))
                {
                    Equipments.Pendants[equipmentId].Read(itemConfigReader);
                } 
                else
                {
                    itemConfigReader.Skip(0x2C);
                }
            }

            // Close Stream
            itemConfigReader.Close();
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
            foreach (var t in equipments) t.Write(itemconfigWriter);

            // Close Stream
            itemconfigWriter.Close();
        }

        private void ReadSoccer(Team team)
        {
            string soccerFile = team.ScriptID.ToString().PadLeft(4, '0');

            if (!File.Exists(Directory + "/ie6_b_fa/data/res/soccer/soccer_chara_btl" + soccerFile + ".cfg.bin")) return;

            DataReader soccerCharaReader = new DataReader(File.ReadAllBytes(Directory + "/ie6_b_fa/data/res/soccer/soccer_chara_btl" + soccerFile + ".cfg.bin"));

            team.SoccerChara = new SoccerCharaConfig(soccerCharaReader);

            soccerCharaReader.Close();
        }
        private void WriteSoccer(Team team)
        {
            string soccerFile = team.ScriptID.ToString().PadLeft(4, '0');

            if (!File.Exists(Directory + "/ie6_b_fa/data/res/soccer/soccer_chara_btl" + soccerFile + ".cfg.bin")) return;

            SoccerCharaConfig teamSoccerChara = team.SoccerChara;

            // Fix Scripted Player
            foreach (var player in teamSoccerChara.Players)
            {
                // Update Moveset
                for (int m = 0; m < 4; m++)
                {
                    // Learn skill
                    if (player.Player.Skills[m].LearnAtLevel < team.Param.Level)
                    {
                        player.Moves[m] = new SoccerMove(Moves.PlayerMoves[player.Player.Skills[m].SkillId], 1);
                    } else
                    {
                        player.Moves[m] = null;
                    }
                }

                // Update Avatar
                if (player.Player.Param.Avatar == 0x0) continue;

                if (Avatars.FightingSpirits.TryGetValue(player.Player.Param.Avatar, out var spirit))
                {
                    player.Avatar = new SoccerAvatar(spirit, 1);
                }
                else if (Avatars.Totems.TryGetValue(player.Player.Param.Avatar, out var totem))
                {
                    player.Avatar = new SoccerAvatar(totem, 1);
                }
            }

            team.SoccerChara.Write(Directory + "/ie6_b_fa/data/res/soccer/soccer_chara_btl" + soccerFile + ".cfg.bin");
        }
        private void ReadTeams()
        {
            // Initialise File Reader
            DataReader soccerConfigReader = new DataReader(File.ReadAllBytes(Directory + "/ie6_b_fa/data/res/soccer/soccer_config_0.01.cfg.bin"));
            DataReader teamParamReader = new DataReader(File.ReadAllBytes(Directory + "/ie6_b_fa/data/res/team/team_param.cfg.bin"));

            // Read Team Config
            soccerConfigReader.Seek(0x3C);
            int teamCount = soccerConfigReader.ReadInt32();
            for (int i = 0; i < teamCount; i++)
            {
                soccerConfigReader.Skip(0x0C);
                uint teamID = soccerConfigReader.ReadUInt32();

                if (Teams.Story.ContainsKey(teamID))
                {
                    Teams.Story[teamID].Read(soccerConfigReader);
                }
                else if (Teams.Battle.ContainsKey(teamID))
                {
                    Teams.Battle[teamID].Read(soccerConfigReader);
                }
                else if (Teams.TaisenRoad.ContainsKey(teamID))
                {
                    Teams.TaisenRoad[teamID].Read(soccerConfigReader);
                }
                else if (Teams.LegendGate.ContainsKey(teamID))
                {
                    Teams.LegendGate[teamID].Read(soccerConfigReader);
                }
                else
                {
                    soccerConfigReader.Skip(0x38);
                }
            }

            // Read Team Param
            teamParamReader.Seek(0x3C);
            teamCount = teamParamReader.ReadInt32();
            for (int i = 0; i < teamCount; i++)
            {
                long tempPosition = teamParamReader.BaseStream.Position;

                teamParamReader.Skip(0x10);
                uint teamId = teamParamReader.ReadUInt32();

                // Search if Team Param ID exists 
                var tryStory = Teams.Story.FirstOrDefault(x => x.Value.TeamParamID == teamId);
                var tryBattle = Teams.Battle.FirstOrDefault(x => x.Value.TeamParamID == teamId);
                var tryTaisenRoad = Teams.TaisenRoad.FirstOrDefault(x => x.Value.TeamParamID == teamId);
                var tryLegendGate = Teams.LegendGate.FirstOrDefault(x => x.Value.TeamParamID == teamId);

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
            DataWriter soccerConfigWriter = new DataWriter(Directory + "/ie6_b_fa/data/res/soccer/soccer_config_0.01.cfg.bin");
            DataWriter teamParamWriter = new DataWriter(Directory + "/ie6_b_fa/data/res/team/team_param.cfg.bin");

            // Merge Teams Dictionaries to one
            var teams = new Dictionary<uint, Team>();
            Teams.Story.ToList().ForEach(x => teams.Add(x.Key, x.Value));
            Teams.Battle.ToList().ForEach(x => teams.Add(x.Key, x.Value));
            Teams.TaisenRoad.ToList().ForEach(x => teams.Add(x.Key, x.Value));
            Teams.LegendGate.ToList().ForEach(x => teams.Add(x.Key, x.Value));

            foreach (var team in teams.Values)
            {
                team.Write(soccerConfigWriter);

                team.Param?.Write(teamParamWriter);

                if (team.SoccerChara == null) continue;

                // Fix Script 
                if (ScriptSoccers.ScriptSoccerGalaxy.TryGetValue(team.ScriptID, out var script))
                {
                    foreach (var index in script.PlayerIndex)
                    {
                        team.SoccerChara.Players[index].Moves[4] = new SoccerMove(Moves.PlayerMoves[script.RightMove], 1);
                    }
                }

                WriteSoccer(team);
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
                
                // Exclude Gashapon
                foreach (string shopFileName in shopDirectory.Where(name => Path.GetFileNameWithoutExtension(name).StartsWith("shop_shp")))
                {
                    // Call Function From Randomizer.cs class
                    Randomizer.RandomizeShop(shopFileName);
                }
            }

            if (options["groupBoxMiscellaneousTreasureBox"].Name == "Random")
            {
                // Find All Files Who Contains Treasure Box Entry
                string[] folders = System.IO.Directory.GetDirectories(Directory + "/ie6_b_fa/data/res/map/").Select(Path.GetFileName).ToArray();
                foreach (string folder in folders.Where(f => File.Exists($"{Directory}/ie6_b_fa/data/res/map/{f}/{f}_oneplace.cfg.bin")))
                {
                    // Call Function From Randomizer.cs class
                    Randomizer.RandomizeTreasureBox($"{Directory}/ie6_b_fa/data/res/map/{folder}/{folder}_oneplace.cfg.bin");
                }
            }

            if (options["groupBoxMiscellaneousRecruitment"].CheckBoxes["checkBoxMiscellaneousRecuitRemove"].Checked)
            {
                DataWriter itemConfigWriter = new DataWriter(Directory + "/ie6_a_fa/gds_pack_decomp_pck/item_config_0.08a.cfg.bin.nat");

                itemConfigWriter.Seek(0xEA44);
                for (int i = 0; i < 1988; i++)
                {
                    itemConfigWriter.Skip(0x08);
                    itemConfigWriter.WriteByte(0x0);
                    itemConfigWriter.Skip(0x0B);

                    for (int j = 0; j < 4; j++)
                    {
                        itemConfigWriter.WriteInt32(0x00);
                    }

                    itemConfigWriter.WriteUInt32(0xFFFFFFFF);
                    itemConfigWriter.WriteUInt32(0x00);

                    itemConfigWriter.Skip(0x10);
                }

                itemConfigWriter.Close();
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
