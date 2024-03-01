using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UltimateGalaxyRandomizer.Tools;
using UltimateGalaxyRandomizer.Logic;
using UltimateGalaxyRandomizer.Logic.Common;
using UltimateGalaxyRandomizer.Resources;
using UltimateGalaxyRandomizer.Randomizer.Utility;

namespace UltimateGalaxyRandomizer.Randomizer
{
    public static class Randomizer
    {
        public static readonly RandomNumber Seed = new RandomNumber();

        private static void SwapPlayers(Dictionary<Player, bool> players, Dictionary<string, Option> options)
        {
            if (options["groupBoxSwapPlayer"].Name != "Random") return;

            // Create Tempory Player Dic
            Dictionary<Player, bool> tempPlayers = players.ToDictionary(x => x.Key.Clone(), x => x.Value);

            foreach (var player in players)
            {
                var possiblePlayers = tempPlayers;

                // Same Body
                if (options["groupBoxSwapPlayer"].CheckBoxes["checkBoxSwapSameBody"].Checked)
                {
                    possiblePlayers = possiblePlayers.Where(x => x.Key.Base.Size >> 4 == player.Key.Base.Size >> 4).ToDictionary(x => x.Key, x => x.Value);
                }

                // Same Position
                if (options["groupBoxSwapPlayer"].CheckBoxes["checkBoxSwapSamePosition"].Checked)
                {
                    possiblePlayers = possiblePlayers.Where(x => x.Key.Param.Position == player.Key.Param.Position).ToDictionary(x => x.Key, x => x.Value);
                }

                // Important Character -> Important Character and Scout -> Scout
                if (options["groupBoxSwapPlayer"].CheckBoxes["checkboxSwapFocus"].Checked)
                {
                    possiblePlayers = possiblePlayers.Where(x => x.Value == player.Value).ToDictionary(x => x.Key, x => x.Value);
                }

                Player randomPlayer = possiblePlayers.ElementAt(Seed.Next(0, possiblePlayers.Count)).Key;
                player.Key.Base.Swap(randomPlayer.Base);
                player.Key.Param.Swap(randomPlayer.Param);
                player.Key.Skills = randomPlayer.Skills;
                tempPlayers.Remove(randomPlayer);
            }
        }

        public static void RandomizePlayers(Dictionary<string, Option> options)
        {
            // Merge Player Dictionaries
            var players = new Dictionary<Player, bool>();
            Players.Story.Values.ToList().ForEach(x => players.Add(x, false));
            Players.Normal.Values.ToList().ForEach(x => players.Add(x, false));
            Players.Scout.Values.ToList().ForEach(x => players.Add(x, true));

            // Do Swap Player First
            SwapPlayers(players, options);

            // Initialise List ID
            var names = players.Keys.Select(x => x.Base.NameID).ToList();
            var nicknames = players.Keys.Select(x => x.Base.NicknameID).ToList();
            var descriptions = players.Keys.Select(x => x.Base.DescriptionID).ToList();

            // Randomize Each Player
            foreach (Player player in players.Keys)
            {
                if (options["groupBoxName"].Name == "Random")
                {
                    int randomIndex = Seed.Next(0, names.Count);
                    player.Base.NameID = names[randomIndex];
                    names.RemoveAt(randomIndex);
                }

                if (options["groupBoxNickname"].Name == "Random")
                {
                    int randomIndex = Seed.Next(0, nicknames.Count);
                    player.Base.NicknameID = nicknames[randomIndex];
                    nicknames.RemoveAt(randomIndex);
                }

                if (options["groupBoxDescription"].Name == "Random")
                {
                    int randomIndex = Seed.Next(0, descriptions.Count);
                    player.Base.DescriptionID = descriptions[randomIndex];
                    descriptions.RemoveAt(randomIndex);
                }

                if (options["groupBoxBody"].Name == "Random")
                {
                    player.Base.Size = (byte)((Seed.Next(1, 9) << 4) | (player.Base.Size & 0x0F));
                }

                if (options["groupBoxYear"].Name == "Random")
                {
                    player.Base.Identity = (byte)((Seed.Next(0, 6) << 4) | (player.Base.Identity & 0x0F));
                }

                if (options["groupBoxGender"].Name == "Random")
                {
                    player.Base.Identity = (byte)((player.Base.Identity << 4) | (Seed.Next(1, 4) & 0x0F));
                }

                if (options["groupBoxStyle"].Name == "Random")
                {
                    player.Base.Style = (byte)Seed.Next(0, 6);
                }

                if (options["groupBoxElement"].Name == "Random")
                {
                    player.Param.Element = (byte)Seed.Next(1, 5 + Convert.ToInt32(options["groupBoxElement"].CheckBoxes["checkBoxElementAllowVoid"].Checked));
                }

                if (options["groupBoxPosition"].Name == "Random")
                {
                    player.Param.Position = (byte)Seed.Next(1, 4);
                }

                switch (options["groupBoxBaseStats"].Name)
                {
                    case "Swap":
                    {
                        // Shuffle
                        List<int> tempStat = player.Param.BaseStat.Values.Select(x => x.Value).ToList();
                        for (int s = 0; s < player.Param.BaseStat.Values.Count; s++)
                        {
                            int getRandomIndex = Seed.Next(0, tempStat.Count);
                            player.Param.BaseStat.Values[player.Param.BaseStat.Values.ElementAt(s).Key] = tempStat[getRandomIndex];
                        }

                        break;
                    }
                    case "Random":
                    {
                        // Generate Random Base Stat
                        Position playerPosition = Positions.Player[player.Param.Position];
                        Element playerElement = Elements.Values[player.Param.Element];
                        Gender playerGender = Identity.Genders[(byte)(player.Base.Identity & 0x0F)];

                        for (int s = 0; s < player.Param.BaseStat.Values.Count; s++)
                        {
                            int baseStat = Seed.Next(24, 45);
                            int finalStat = (playerPosition.StatBuff[s] * baseStat / 100) + (playerElement.StatBuff[s] * baseStat / 100) + (playerGender.StatBuff[s] * baseStat / 100) + baseStat;
                            player.Param.BaseStat.Values[player.Param.BaseStat.Values.ElementAt(s).Key] = finalStat;
                        }

                        break;
                    }
                }

                if (options["groupBoxGrownStats"].Name == "Random")
                {
                    var grownStats = new[] { 0, 1, 2, 254, 255 };
                    for (int s = 0; s < player.Param.BaseStat.Values.Count; s++)
                    {
                        player.Param.GrownStat.Values[player.Param.BaseStat.Values.ElementAt(s).Key] = grownStats[Seed.Next(0, grownStats.Count())];
                    }
                }

                if (options["groupBoxFreedom"].Name == "Random")
                {
                    player.Param.Freedom = Convert.ToInt16(Seed.Next(20, 41) * 10);
                }

                if (options["groupBoxMoveset"].Name == "Random")
                {
                    // Get Position and Element Type Probability
                    var moveset = player.GetRandomMoveset(player.Param.SkillCount);

                    for (int s = 0; s < player.Param.SkillCount; s++) player.Skills[s].SkillID = moveset[s];
                }

                // keep CanInvoke status, randomize Spirit and Totems
                if (options["groupBoxAvatar"].Name == "Random")
                {
                    Invoke invoke = Invokes.Values[player.Param.Invoke];

                    uint avatarId = 0x00;

                    if (invoke.CanInvoke)
                    {
                        var invokerProbability = new[]
                        {
                            Convert.ToInt32(options["groupBoxInvokerUser"].NumericUpDowns["numericUpDownFightingSpirit"].Value),
                            Convert.ToInt32(options["groupBoxInvokerUser"].NumericUpDowns["numericUpDownTotem"].Value),
                            Convert.ToInt32(options["groupBoxInvokerUser"].NumericUpDowns["numericUpDownNoneInvoker"].Value)
                        };

                        int invokerStatus = new Probability(invokerProbability).GetRandomIndex();
                        switch (invokerStatus)
                        {
                            case 0:
                                avatarId = player.GetRandomFightingSpirit();
                                break;
                            case 1:
                                avatarId = player.GetRandomTotem();
                                break;
                            case 2:
                                avatarId = 0x00;
                                break;
                        }
                    }

                    player.Param.Avatar = avatarId;
                }
                
                // randomize Spirit and Totems completely
                if (options["groupBoxInvokerUser"].Name == "Random")
                {
                    var invokerProbability = new[] {
                        Convert.ToInt32(options["groupBoxInvokerUser"].NumericUpDowns["numericUpDownFightingSpirit"].Value),
                        Convert.ToInt32(options["groupBoxInvokerUser"].NumericUpDowns["numericUpDownTotem"].Value),
                        Convert.ToInt32(options["groupBoxInvokerUser"].NumericUpDowns["numericUpDownNoneInvoker"].Value)
                    };

                    // Get random invoker status according to user invoker probability
                    int invokerStatus = new Probability(invokerProbability).GetRandomIndex();
                    switch (invokerStatus)
                    {
                        case 0:
                            player.Param.Invoke = 0x04;
                            player.Param.Avatar = player.GetRandomFightingSpirit();
                            break;
                        case 1:
                            player.Param.Invoke = 0x44;
                            player.Param.Avatar = player.GetRandomTotem();
                            break;
                        case 2:
                            player.Param.Invoke = 0x00;
                            player.Param.Avatar = 0x00;
                            break;
                    }
                }

                if (options["groupBoxArmoufiedUser"].Name == "Random")
                {
                    Invoke invoke = Invokes.Values[player.Param.Invoke];

                    if (invoke.CanInvoke)
                    {
                        int armorifyPercentage = Convert.ToInt32(options["groupBoxArmoufiedUser"].NumericUpDowns["numericUpDownArmoufly"].Value);
                        var canArmorify = new Probability(new[] { 100 - armorifyPercentage, armorifyPercentage }).GetRandomIndex() == 0;
                        player.Param.Invoke = player.Param.Invoke.ChangeArmorify(canArmorify);
                    }
                }

                if (options["groupBoxExperienceBar"].Name == "Random")
                {
                    player.Param.ExperienceBar = Convert.ToByte(Seed.Next(1, 8));
                }
            }
        }

        public static void RandomizeMoves(Dictionary<string, Option> options)
        {
            // Randomize Each Player Moves -- Exclude Skill
            foreach (var move in Moves.PlayerMoves.Values.Where(move => move.Position != 15))
            {
                if (options["groupBoxMoveEvolution"].Name == "Random")
                {
                    move.Evolution = Convert.ToByte(Seed.Next(1, 9));
                }

                if (options["groupBoxMoveElement"].Name == "Random")
                {
                    move.Element = Convert.ToByte(Seed.Next(1, 6));
                }

                if (options["groupBoxMoveEffect"].Name == "Random")
                {
                    int effectProbability = new Probability(new int[2] { 70, 30 }).GetRandomIndex();

                    if (effectProbability == 0 || move.Position == 0x02)
                    {
                        move.Effect = 0x00;
                    }
                    else
                    {
                        List<byte> effects = Effects.Values.Where(x => x.Value.Position == move.Position).Select(x => x.Key).ToList();
                        move.Effect = effects[Seed.Next(0, effects.Count)];
                    }
                }

                if (options["groupBoxMovePower"].Name == "Random")
                {
                    int power = Seed.Next(3, 21) * 10;

                    if (move.Effect != 0x0)
                    {
                        power -= 10;
                    }

                    move.Power = Convert.ToByte(power / 2);
                }

                if (options["groupBoxMoveTP"].Name == "Random")
                {
                    int tpCost = Convert.ToInt32(move.Power * 0.8);
                    tpCost -= tpCost % 5;

                    if (tpCost > 85)
                    {
                        tpCost = 85;
                    }

                    move.TP = Convert.ToByte(tpCost);
                }

                if (options["groupBoxMoveDifficulty"].Name == "Random")
                {
                    int difficulty = Convert.ToInt32(move.Power);
                    difficulty -= difficulty % 10;
                    difficulty *= move.Partner;

                    move.Technique = Convert.ToInt16(difficulty);
                }

                if (options["groupBoxMoveStunDamage"].Name == "Random")
                {
                    int damage = Seed.Next(0, 11);

                    if (move.Position == 0x04)
                    {
                        damage *= -10;
                    }
                    else
                    {
                        damage *= 10;
                    }

                    move.Damage = Convert.ToSByte(damage);
                }

                if (options["groupBoxMoveFoulRate"].Name == "Random")
                {
                    if (move.Position == 0x02 || move.Position == 0x03)
                    {
                        move.FoulRate = Convert.ToByte(Seed.Next(0, 5) * 10);
                    }
                }
            }

            // Randomize Each Fighting Spirit Moves
            foreach (var move in Moves.FightingSpiritMoves.Values.Where(move => move.Position != 15))
            {
                if (options["groupBoxMoveElement"].Name == "Random")
                {
                    move.Element = Convert.ToByte(Seed.Next(1, 6));
                }

                if (options["groupBoxMovePower"].Name == "Random")
                {
                    int power = Seed.Next(22, 36) * 10;

                    move.Power = Convert.ToByte(power / 2);
                }

                if (options["groupBoxMoveTP"].Name == "Random")
                {
                    int tpCost = Convert.ToInt32(move.Power / 2);

                    if (move.Power > 145)
                    {
                        tpCost += (move.Power - 145) * 2;
                    }

                    if (move.Position != 0x01)
                    {
                        tpCost = Convert.ToInt32(tpCost / 1.5);
                    }

                    tpCost -= tpCost % 5;
                    move.TP = Convert.ToByte(tpCost);
                }

                if (options["groupBoxMoveStunDamage"].Name == "Random")
                {
                    int damage = Seed.Next(0, 11);

                    if (move.Position == 0x04)
                    {
                        damage *= -10;
                    }
                    else
                    {
                        damage *= 10;
                    }

                    move.Damage = Convert.ToSByte(damage);
                }
            }

            foreach (var move in Moves.TotemMoves.Values.Where(move => move.Position != 15))
            {
                if (options["groupBoxMovePower"].Name == "Random")
                {
                    int power = 0;

                    // Totem Move are as strong as Fighting Spirit
                    if (options["groupBoxMoveMiscellaneous"].Name == "Random")
                    {
                        power = Seed.Next(30, 41) * 10;
                    }
                    else
                    {
                        List<int> possiblePower = new List<int>() { 250, 300 };
                        power = possiblePower[Seed.Next(0, possiblePower.Count)];
                    }

                    move.Power = Convert.ToByte(power / 2);
                }

                if (options["groupBoxMoveTP"].Name == "Random")
                {
                    // Totem Move are as strong as Fighting Spirit
                    if (options["groupBoxMoveMiscellaneous"].Name == "Random")
                    {
                        int tpCost = 40 + (move.Power - 150);
                        move.TP = Convert.ToByte(tpCost);
                    }
                    else
                    {
                        List<int> tpCost = new List<int>() { 40, 50, 70 };
                        move.TP = Convert.ToByte(tpCost[Seed.Next(0, tpCost.Count)]);
                    }
                }

                if (options["groupBoxMoveStunDamage"].Name == "Random")
                {
                    int damage = 0;

                    // Totem Move are as strong as Fighting Spirit
                    if (options["groupBoxMoveMiscellaneous"].Name == "Random")
                    {
                        damage = Seed.Next(0, 11);
                    }
                    else
                    {
                        damage = Seed.Next(0, 4);
                    }

                    if (move.Position == 0x04)
                    {
                        damage *= -10;
                    }
                    else
                    {
                        damage *= 10;
                    }

                    move.Damage = Convert.ToSByte(damage);
                }
            }

            // Randomize Each Ultimate Moves
            foreach (var move in Moves.MovesUltimate)
            {
                if (options["groupBoxUltimatePower"].Name == "Random")
                {
                    move.Power = Convert.ToInt16(Seed.Next(28, 33) * 10);
                }

                if (options["groupBoxUltimateTP"].Name == "Random")
                {
                    List<int> tpCost = new List<int>() { 85, 99 };
                    move.TP = Convert.ToByte(tpCost[Seed.Next(0, tpCost.Count)]);
                }

                if (options["groupBoxUltimateDifficulty"].Name == "Random")
                {
                    move.Technique = 0x64;
                }

                if (options["groupBoxUltimateStunDamage"].Name == "Random")
                {
                    int damage = Seed.Next(0, 11);

                    if (move.Damage < 0)
                    {
                        damage *= -10;
                    }
                    else
                    {
                        damage *= 10;
                    }

                    move.Damage = Convert.ToSByte(damage);
                }
            }
        }

        public static void RandomizeAvatars(Dictionary<string, Option> options)
        {
            // Randomize Each Avatar
            foreach (FightingSpirit avatar in Avatars.FightingSpirits.Values)
            {
                if (options["groupBoxSpiritElement"].Name == "Random")
                {
                    avatar.Element = Convert.ToByte(Seed.Next(1, 6));
                }

                if (options["groupBoxSpiritMove"].Name == "Random")
                {
                    // Create a list of moves according to avatar probability
                    Probability samePlayerPosition = avatar.GetPositionProbability();
                    Probability samePlayerElement = avatar.GetElementProbability();

                    int movePosition = samePlayerPosition.GetRandomIndex();
                    while (movePosition == 4)
                    {
                        // Exclude Skill Probability
                        movePosition = samePlayerPosition.GetRandomIndex();
                    }
                    var possibleMoves = Moves.FightingSpiritMoves.Where(x => x.Value.Position == movePosition + 1).ToDictionary(x => x.Key, x => x.Value);

                    // Create a list of moves according to player element probability
                    int moveElement = samePlayerElement.GetRandomIndex();
                    possibleMoves = possibleMoves.Where(x => x.Value.Element == moveElement + 1).ToDictionary(x => x.Key, x => x.Value);

                    // Only in an extreme case
                    if (possibleMoves.Count == 0)
                    {
                        possibleMoves = Moves.FightingSpiritMoves.Where(x => x.Value.Position != 15).ToDictionary(x => x.Key, x => x.Value);
                    }

                    avatar.MoveID = possibleMoves.ElementAt(Seed.Next(0, possibleMoves.Count)).Key;
                }

                if (options["groupBoxSpiritSkill"].Name == "Random")
                {
                    var possibleSkills = Moves.FightingSpiritMoves.Where(x => x.Value.Position == 15).ToDictionary(x => x.Key, x => x.Value);
                    avatar.SkillID = possibleSkills.ElementAt(Seed.Next(0, possibleSkills.Count)).Key;
                }

                if (options["groupBoxSpiritPoint"].Name == "Random")
                {
                    avatar.FS = Convert.ToInt16(Seed.Next(8, 21) * 10);
                }

                if (options["groupBoxSpiritPointUp"].Name == "Random")
                {
                    for (int s = 0; s < avatar.FSPUP.Length; s++)
                    {
                        avatar.FSPUP[s] = Seed.Next(0, 11) * 5;
                    }
                }

                if (options["groupBoxSpiritPower"].Name == "Random")
                {
                    avatar.Attack = Convert.ToByte(Seed.Next(2, 16) * 10);
                }

                if (options["groupBoxSpiritPowerUp"].Name == "Random")
                {
                    for (int s = 0; s < avatar.FSPUP.Length; s++)
                    {
                        avatar.AttackUP[s] = Seed.Next(0, 11) * 5;
                    }
                }
            }

            // Randomize Each Totem
            foreach (Totem avatar in Avatars.Totems.Values)
            {
                if (options["groupBoxTotemElement"].Name == "Random")
                {
                    avatar.Element = Convert.ToByte(Seed.Next(1, 6));
                }

                if (options["groupBoxTotemMove"].Name == "Random")
                {
                    var possibleMoves = Moves.TotemMoves.Where(x => x.Value.Position != 15).ToDictionary(x => x.Key, x => x.Value);
                    avatar.MoveID = possibleMoves.ElementAt(Seed.Next(0, possibleMoves.Count)).Key;
                }

                if (options["groupBoxTotemRoulette"].Name == "Random")
                {
                    // Create Temp Skill List
                    var tempSkills = Moves.TotemMoves.Where(x => x.Value.Position == 15).ToDictionary(x => x.Key, x => x.Value);

                    // Remove Miss
                    if (options["groupBoxTotemRoulette"].CheckBoxes["checkBoxRouletteNoMiss"].Checked)
                    {
                        tempSkills.Remove(0x9A1F8583);
                    }

                    for (int s = 0; s < avatar.SkillRoulette.Length; s++)
                    {
                        var randomSkill = tempSkills.ElementAt(Seed.Next(0, tempSkills.Count));
                        avatar.SkillRoulette[s] = randomSkill.Key;

                        // Remove Move to avoid duplication
                        if (options["groupBoxTotemRoulette"].CheckBoxes["checkBoxRouletteNoDuplicate"].Checked)
                        {
                            tempSkills.Remove(randomSkill.Key);
                        }
                    }
                }

                if (options["groupBoxTotemPoint"].Name == "Random")
                {
                    avatar.SP = Convert.ToInt16(Seed.Next(8, 13) * 10);
                }

                if (options["groupBoxTotemPointUp"].Name == "Random")
                {
                    for (int s = 0; s < avatar.SPUP.Length; s++)
                    {
                        avatar.SPUP[s] = Convert.ToByte(Seed.Next(1, 8) * 10);
                    }
                }
            }
        }

        public static void RandomizeEquipments(Dictionary<string, Option> options)
        {
            // Randomize Each Boots
            foreach (var equipmentStats in Equipments.Boots.Values.Select(eq => eq.BaseStat.Values))
            {
                if (options["groupBoxMiscellaneousEquipment"].Name != "Random") continue;

                // Reset Stat
                foreach (var t in equipmentStats.Keys) equipmentStats[t] = 0;

                equipmentStats["Kick"] = Seed.Next(0, 15) * 5;
                equipmentStats["Speed"] = Seed.Next(0, 15) * 5;
            }

            // Randomize Each Gloves
            foreach (var equipmentStats in Equipments.Gloves.Values.Select(eq => eq.BaseStat.Values))
            {
                if (options["groupBoxMiscellaneousEquipment"].Name != "Random") continue;

                foreach (var t in  equipmentStats.Keys) equipmentStats[t] = 0;

                equipmentStats["Catch"] = Seed.Next(0, 15) * 5;
                equipmentStats["Technique"] = Seed.Next(0, 15) * 5;
            }

            // Randomize Each Bracelets
            foreach (var equipmentStats in Equipments.Bracelets.Values.Select(eq => eq.BaseStat.Values))
            {
                if (options["groupBoxMiscellaneousEquipment"].Name != "Random") continue;

                foreach (var t in equipmentStats.Keys) equipmentStats[t] = 0;

                equipmentStats["Stamina"] = Seed.Next(0, 15) * 5;
                equipmentStats["Luck"] = Seed.Next(0, 15) * 5;
            }

            // Randomize Each Pendants
            foreach (var equipmentStats in Equipments.Pendants.Values.Select(eq => eq.BaseStat.Values))
            {
                if (options["groupBoxMiscellaneousEquipment"].Name != "Random") continue;

                foreach (var t in equipmentStats.Keys) equipmentStats[t] = 0;

                equipmentStats["Dribble"] = Seed.Next(0, 15) * 5;
                equipmentStats["Block"] = Seed.Next(0, 15) * 5;
            }
        }

        public static void RandomizeTeams(Dictionary<string, Option> options)
        {
            // Merge Teams Dictionaries to one
            var teams = new Dictionary<uint, Team>();
            Teams.Story.ToList().ForEach(x => teams.Add(x.Key, x.Value));
            Teams.Battle.ToList().ForEach(x => teams.Add(x.Key, x.Value));
            Teams.TaisenRoad.ToList().ForEach(x => teams.Add(x.Key, x.Value));
            Teams.LegendGate.ToList().ForEach(x => teams.Add(x.Key, x.Value));

            // Randomize Each Team
            foreach (Team team in teams.Values)
            {
                if (options["groupBoxTeamsMiscellaneous"].CheckBoxes["checkBoxTeamsMiniMatchize"].Checked)
                {
                    team.IsMatchField = false;
                    team.MiniMatchValue = 0x04;
                    team.Timer = 0x0F;
                }

                if (options["groupBoxTeamsTimer"].Name == "Random")
                {
                    if (team.IsMatchField)
                    {
                        team.Timer = (byte)options["groupBoxTeamsTimer"].NumericUpDowns["numericUpDownTeamsTimerMatch"].Value;
                    }
                    else
                    {
                        team.Timer = (byte)options["groupBoxTeamsTimer"].NumericUpDowns["numericUpDownTeamsTimerMiniMatch"].Value;
                    }
                }

                if (options["groupBoxTeamsMiscellaneous"].CheckBoxes["checkBoxTeamsDisableScript"].Checked)
                {
                    if (team.IsMatchField)
                    {
                        team.ScriptID = 0x000009D3;
                        team.ScriptID2 = 0x00000019;
                        team.RestrictionID = 0x0;
                        team.RestrictionID2 = 0x0;
                    } else
                    {
                        team.ScriptID = 0x00000066;
                        team.ScriptID2 = 0x0000006C;
                        team.RestrictionID = 0x0;
                        team.RestrictionID2 = 0x0;
                    }
                }

                if (options["groupBoxTeamsMiscellaneous"].CheckBoxes["checkBoxTeamsMaxDifficulty"].Checked)
                {
                    team.ArtificialIntelligenceID = team.IsMatchField ? 0xF26795E4 : 0xF36795E4;
                }

                if (team.Param == null) continue;

                if (options["groupBoxTeamsCoach"].Name == "Random")
                {
                    team.Param.Coach = Items.Coaches.ElementAt(Seed.Next(0, Items.Coaches.Count)).Key;
                }

                if (options["groupBoxTeamsFormation"].Name == "Random")
                {
                    team.Param.Formation = team.IsMatchField ? Items.FormationMatches.ElementAt(Seed.Next(0, Items.FormationMatches.Count)).Key : Items.FormationMiniMatches.ElementAt(Seed.Next(0, Items.FormationMiniMatches.Count)).Key;
                }

                if (options["groupBoxTeamsTactic"].Name == "Random")
                {
                    team.Param.Tactic = team.IsMatchField ? Items.Tactics.ElementAt(Seed.Next(0, Items.Tactics.Count)).Key : 0x00;
                }

                if (options["groupBoxTeamsDrop"].Name == "Random")
                {
                    for (int d = 0; d < team.Param.Drop.Length; d++)
                    {
                        if (Items.PotentialDrop.ContainsKey(team.Param.Drop[d]))
                        {
                            team.Param.Drop[d] = Items.PotentialDrop.ElementAt(Seed.Next(0, Items.PotentialDrop.Count)).Key;
                        }
                    }
                }

                if (options["groupBoxTeamsKit"].Name == "Random")
                {
                    team.Param.Kit = Items.Kits.ElementAt(Seed.Next(0, Items.Kits.Count)).Key;
                }

                if (options["groupBoxTeamsEquipment"].Name == "Random")
                {
                    team.Param.Equipments[0] = Equipments.Boots.ElementAt(Seed.Next(0, Equipments.Boots.Count)).Key;
                    team.Param.Equipments[1] = Equipments.Gloves.ElementAt(Seed.Next(0, Equipments.Gloves.Count)).Key;
                    team.Param.Equipments[2] = Equipments.Bracelets.ElementAt(Seed.Next(0, Equipments.Bracelets.Count)).Key;
                    team.Param.Equipments[3] = Equipments.Pendants.ElementAt(Seed.Next(0, Equipments.Pendants.Count)).Key;
                }

                if (options["groupBoxTeamsLevel"].Name == "Random")
                {
                    team.Param.Level += (byte)(Convert.ToInt32(options["groupBoxTeamsLevel"].NumericUpDowns["numericUpDownTeamsLevel"].Value) * team.Param.Level / 100);
                }

                if (options["groupBoxTeamsExperience"].Name == "Random")
                {
                    team.Param.Experience += (byte)(Convert.ToInt32(options["groupBoxTeamsExperience"].NumericUpDowns["numericUpDownTeamsExperience"].Value) * team.Param.Experience / 100);
                }

                if (options["groupBoxTeamsPrestige"].Name == "Random")
                {
                    team.Param.Prestige += (byte)(Convert.ToInt32(options["groupBoxTeamsPrestige"].NumericUpDowns["numericUpDownTeamsPrestige"].Value) * team.Param.Prestige / 100);
                }

                if (options["groupBoxTeamsFreedom"].Name == "Random")
                {
                    if (options["groupBoxTeamsFreedom"].CheckBoxes["checkBoxTeamsFreedomAll"].Checked)
                    {
                        team.Param.Freedom = (byte)options["groupBoxTeamsFreedom"].NumericUpDowns["numericUpDownTeamsFreedom"].Value;
                    }
                    else if (team.Param.Freedom > 0)
                    {
                        team.Param.Freedom = (byte)options["groupBoxTeamsFreedom"].NumericUpDowns["numericUpDownTeamsFreedom"].Value;
                    }
                }
            }
        }

        public static void RandomizeShop(string filename)
        {
            DataReader shopReader = new DataReader(File.ReadAllBytes(filename));
            DataWriter shopWriter = new DataWriter(filename);

            var takeUInt32 = shopReader.ReadUInt32();
            while (shopReader.BaseStream.Position < shopReader.Length)
            {
                // Shop Data Found
                if (takeUInt32 == 0x6CFC021A)
                {
                    shopReader.Skip(0x08);

                    int shopCount = shopReader.ReadInt32();
                    for (int i = 0; i < shopCount; i++)
                    {
                        shopReader.Skip(0x08);
                        shopWriter.Seek((uint)shopReader.BaseStream.Position);

                        var itemId = shopReader.ReadUInt32();
                        if (Items.PotentialShop.ContainsKey(itemId))
                        {
                            shopWriter.WriteUInt32(Items.PotentialShop.ElementAt(Seed.Next(0, Items.PotentialShop.Count)).Key);
                        }

                        shopReader.Skip(0x04);
                    }

                    break;
                }

                takeUInt32 = shopReader.ReadUInt32();
            }

            shopReader.Close();
            shopWriter.Close();
        }

        public static void RandomizeTreasureBox(string filename)
        {
            DataReader treasureBoxReader = new DataReader(File.ReadAllBytes(filename));
            DataWriter treasureBoxWriter = new DataWriter(filename);

            // Find Start Byte
            treasureBoxReader.Seek(0x3C);
            int boxCount = treasureBoxReader.ReadInt32();

            // Randomize Treasure Box
            for (int i = 0; i < boxCount; i++)
            {
                treasureBoxReader.Skip(0x14);
                treasureBoxWriter.Seek((uint)treasureBoxReader.BaseStream.Position);

                var itemIdOne = treasureBoxReader.ReadUInt32();
                var itemIdTwo = treasureBoxReader.ReadUInt32();

                // Check If It's a valid Treasure Box
                if (itemIdOne == itemIdTwo && Items.PotentialDrop.ContainsKey(itemIdOne))
                {
                    var randomItem = Items.PotentialDrop.ElementAt(Seed.Next(0, Items.PotentialDrop.Count)).Key;
                    treasureBoxWriter.WriteUInt32(randomItem);
                    treasureBoxWriter.WriteUInt32(randomItem);
                }

                treasureBoxReader.Skip(0x08);
            }

            // Close File
            treasureBoxReader.Close();
            treasureBoxWriter.Close();
        }
    }
}
