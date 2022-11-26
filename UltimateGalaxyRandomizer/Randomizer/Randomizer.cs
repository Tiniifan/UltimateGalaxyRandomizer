using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UltimateGalaxyRandomizer.Tools;
using UltimateGalaxyRandomizer.Logic;
using UltimateGalaxyRandomizer.Resources;
using UltimateGalaxyRandomizer.Randomizer.Utility;

namespace UltimateGalaxyRandomizer.Randomizer
{
    public static class Randomizer
    {
        public static RandomNumber Seed = new RandomNumber();

        private static void SwapPlayers(Dictionary<Player, bool> players, Dictionary<string, Option> options)
        {
            if (options["groupBoxSwapPlayer"].Name == "Random")
            {
                // Create Tempory Player Dic
                Dictionary<Player, bool> tempPlayers = players.ToDictionary(x => x.Key.Clone(), x => x.Value);

                foreach (KeyValuePair<Player, bool> player in players)
                {
                    Dictionary<Player, bool> possiblePlayers = new Dictionary<Player, bool>();

                    // Same Body
                    if (options["groupBoxSwapPlayer"].CheckBoxes["checkBoxSwapSameBody"].Checked == true)
                    {
                        possiblePlayers = tempPlayers.Where(x => x.Key.Base.Size >> 4 == player.Key.Base.Size >> 4).ToDictionary(x => x.Key, x => x.Value);
                    }
                    else
                    {
                        possiblePlayers = tempPlayers;
                    }

                    // Same Position
                    if (options["groupBoxSwapPlayer"].CheckBoxes["checkBoxSwapSamePosition"].Checked == true)
                    {
                        possiblePlayers = possiblePlayers.Where(x => x.Key.Param.Position == player.Key.Param.Position).ToDictionary(x => x.Key, x => x.Value);
                    }

                    // Important Character -> Important Character and Scout -> Scout
                    if (options["groupBoxSwapPlayer"].CheckBoxes["checkboxSwapFocus"].Checked == true)
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
        }

        public static void RandomizePlayers(Dictionary<string, Option> options)
        {
            // Merge Player Dictionaries
            Dictionary<Player, bool> players = new Dictionary<Player, bool>();
            Players.Story.Values.ToList().ForEach(x => players.Add(x, false));
            Players.Normal.Values.ToList().ForEach(x => players.Add(x, false));
            Players.Scout.Values.ToList().ForEach(x => players.Add(x, true));

            // Do Swap Player First
            SwapPlayers(players, options);

            // Initialise List ID
            List<UInt32> names = players.Keys.Select(x => x.Base.NameID).ToList();
            List<UInt32> nicknames = players.Keys.Select(x => x.Base.NicknameID).ToList();
            List<UInt32> descriptions = players.Keys.Select(x => x.Base.DescriptionID).ToList();

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

                if (options["groupBoxBaseStats"].Name == "Swap")
                {
                    // Shuffle
                    List<int> tempStat = player.Param.BaseStat.Values.Select(x => x.Value).ToList();
                    for (int s = 0; s < player.Param.BaseStat.Values.Count; s++)
                    {
                        int getRandomIndex = Seed.Next(0, tempStat.Count);
                        player.Param.BaseStat.Values[player.Param.BaseStat.Values.ElementAt(s).Key] = tempStat[getRandomIndex];
                    }
                }
                else if (options["groupBoxBaseStats"].Name == "Random")
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
                }

                if (options["groupBoxGrownStats"].Name == "Random")
                {
                    int[] grownStats = new int[5] { 0, 1, 2, 254, 255 };
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
                    List<UInt32> moveset = player.GetRandomMoveset(player.Param.SkillCount);

                    for (int s = 0; s < player.Param.SkillCount; s++)
                    {
                        player.Skills[s].SkillID = moveset[s];
                    }
                }

                if (options["groupBoxAvatar"].Name == "Random")
                {
                    Invoke invoke = Invokes.Values[player.Param.Invoke];

                    UInt32 avatarID = 0x00;

                    if (invoke.CanInvoke == true)
                    {
                        int[] invokerProbability = new int[3] {
                        Convert.ToInt32(options["groupBoxInvokerUser"].NumericUpDowns["numericUpDownFightingSpirit"].Value),
                        Convert.ToInt32(options["groupBoxInvokerUser"].NumericUpDowns["numericUpDownTotem"].Value),
                        Convert.ToInt32(options["groupBoxInvokerUser"].NumericUpDowns["numericUpDownNoneInvoker"].Value)
                        };

                        int invokerStatus = new Probability(invokerProbability).GetRandomIndex();
                        switch (invokerStatus)
                        {
                            case 0:
                                avatarID = player.GetRandomFightingSpirit();
                                break;
                            case 1:
                                avatarID = player.GetRandomTotem();
                                break;
                            case 2:
                                avatarID = 0x00;
                                break;
                        }
                    }

                    player.Param.Avatar = avatarID;
                }

                if (options["groupBoxInvokerUser"].Name == "Random")
                {
                    int[] invokerProbability = new int[3] {
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

                    if (invoke.CanInvoke == true && invoke.IsFightingSpirit == false)
                    {
                        int armourflyPercentage = Convert.ToInt32(options["groupBoxArmoufiedUser"].NumericUpDowns["numericUpDownArmoufly"].Value);
                        Probability armoufly = new Probability(new int[2] { 100 - armourflyPercentage, armourflyPercentage });
                        switch (armoufly.GetRandomIndex())
                        {
                            case 0:
                                player.Param.Invoke = 0x04;
                                break;
                            case 1:
                                player.Param.Invoke = 0x0C;
                                break;
                        }
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
            // Randomize Each Player Moves
            foreach (Move move in Moves.PlayerMoves.Values)
            {
                // Exclude Skill
                if (move.Position != 15)
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
            }

            // Randomize Each Fighting Spirit Moves
            foreach (Move move in Moves.FightingSpiritMoves.Values)
            {
                if (move.Position != 15)
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
                        int tpCost = Convert.ToInt32(move.Power / 2); ;

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
            }

            foreach (Move move in Moves.TotemMoves.Values)
            {
                if (move.Position != 15)
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
            }

            // Randomize Each Ultimate Moves
            for (int i = 0; i < Moves.MovesUltimate.Count; i++)
            {
                MoveUltimate move = Moves.MovesUltimate[i];

                if (options["groupBoxUltimatePower"].Name == "Random")
                {
                    move.Power = Convert.ToByte(Seed.Next(28, 33) * 10);
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
                    Dictionary<UInt32, Move> possibleMoves = new Dictionary<UInt32, Move>();

                    // Create a list of moves according to avatar probability
                    Probability samePlayerPosition = avatar.GetPositionProbability();
                    Probability samePlayerElement = avatar.GetElementProbability();

                    int movePosition = samePlayerPosition.GetRandomIndex();
                    while (movePosition == 4)
                    {
                        // Exclude Skill Probability
                        movePosition = samePlayerPosition.GetRandomIndex();
                    }
                    possibleMoves = Moves.FightingSpiritMoves.Where(x => x.Value.Position == movePosition + 1).ToDictionary(x => x.Key, x => x.Value);

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
                    Dictionary<UInt32, Move> possibleSkills = Moves.FightingSpiritMoves.Where(x => x.Value.Position == 15).ToDictionary(x => x.Key, x => x.Value);
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
                    Dictionary<UInt32, Move> possibleMoves = Moves.TotemMoves.Where(x => x.Value.Position != 15).ToDictionary(x => x.Key, x => x.Value);
                    avatar.MoveID = possibleMoves.ElementAt(Seed.Next(0, possibleMoves.Count)).Key;
                }

                if (options["groupBoxTotemRoulette"].Name == "Random")
                {
                    // Create Temp Skill List
                    Dictionary<UInt32, Move> tempSkills = Moves.TotemMoves.Where(x => x.Value.Position == 15).ToDictionary(x => x.Key, x => x.Value);

                    // Remove Miss
                    if (options["groupBoxTotemRoulette"].CheckBoxes["checkBoxRouletteNoMiss"].Checked == true)
                    {
                        tempSkills.Remove(0x9A1F8583);
                    }

                    for (int s = 0; s < avatar.SkillRoulette.Length; s++)
                    {
                        KeyValuePair<UInt32, Move> randomSkill = tempSkills.ElementAt(Seed.Next(0, tempSkills.Count));
                        avatar.SkillRoulette[s] = randomSkill.Key;

                        // Remove Move to avoid duplication
                        if (options["groupBoxTotemRoulette"].CheckBoxes["checkBoxRouletteNoDuplicate"].Checked == true)
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
            foreach (Equipment equipment in Equipments.Boots.Values)
            {
                if (options["groupBoxMiscellaneousEquipment"].Name == "Swap")
                {
                    // Reset Stat
                    foreach (string key in equipment.BaseStat.Values.Keys)
                    {
                        equipment.BaseStat.Values[key] = 0;
                    }

                    // Create Random Stat
                    equipment.BaseStat.Values["Kick"] = Seed.Next(0, 15) * 5;
                    equipment.BaseStat.Values["Speed"] = Seed.Next(0, 15) * 5;
                } 
                else if (options["groupBoxMiscellaneousEquipment"].Name == "Random")
                {
                    // Create Totaly Random  Stat
                    foreach (string key in equipment.BaseStat.Values.Keys)
                    {
                        equipment.BaseStat.Values[key] = Seed.Next(0, 15) * 5;
                    }
                }
            }

            // Randomize Each Gloves
            foreach (Equipment equipment in Equipments.Gloves.Values)
            {
                if (options["groupBoxMiscellaneousEquipment"].Name == "Swap")
                {
                    // Reset Stat
                    foreach (string key in equipment.BaseStat.Values.Keys)
                    {
                        equipment.BaseStat.Values[key] = 0;
                    }

                    // Create Random Stat
                    equipment.BaseStat.Values["Catch"] = Seed.Next(0, 15) * 5;
                    equipment.BaseStat.Values["Technique"] = Seed.Next(0, 15) * 5;
                }
                else if (options["groupBoxMiscellaneousEquipment"].Name == "Random")
                {
                    // Create Totaly Random  Stat
                    foreach (string key in equipment.BaseStat.Values.Keys)
                    {
                        equipment.BaseStat.Values[key] = Seed.Next(0, 15) * 5;
                    }
                }
            }

            // Randomize Each Bracelets
            foreach (Equipment equipment in Equipments.Bracelets.Values)
            {
                if (options["groupBoxMiscellaneousEquipment"].Name == "Swap")
                {
                    // Reset Stat
                    foreach (string key in equipment.BaseStat.Values.Keys)
                    {
                        equipment.BaseStat.Values[key] = 0;
                    }

                    // Create Random Stat
                    equipment.BaseStat.Values["Stamina"] = Seed.Next(0, 15) * 5;
                    equipment.BaseStat.Values["Luck"] = Seed.Next(0, 15) * 5;
                }
                else if (options["groupBoxMiscellaneousEquipment"].Name == "Random")
                {
                    // Create Totaly Random  Stat
                    foreach (string key in equipment.BaseStat.Values.Keys)
                    {
                        equipment.BaseStat.Values[key] = Seed.Next(0, 15) * 5;
                    }
                }
            }

            // Randomize Each Pendants
            foreach (Equipment equipment in Equipments.Pendants.Values)
            {
                if (options["groupBoxMiscellaneousEquipment"].Name == "Swap")
                {
                    // Reset Stat
                    foreach (string key in equipment.BaseStat.Values.Keys)
                    {
                        equipment.BaseStat.Values[key] = 0;
                    }

                    // Create Random Stat
                    equipment.BaseStat.Values["Dribble"] = Seed.Next(0, 15) * 5;
                    equipment.BaseStat.Values["Block"] = Seed.Next(0, 15) * 5;
                }
                else if (options["groupBoxMiscellaneousEquipment"].Name == "Random")
                {
                    // Create Totaly Random  Stat
                    foreach (string key in equipment.BaseStat.Values.Keys)
                    {
                        equipment.BaseStat.Values[key] = Seed.Next(0, 15) * 5;
                    }
                }
            }
        }

        public static void RandomizeTeams(Dictionary<string, Option> options)
        {
            // Merge Teams Dictionaries to one
            Dictionary<UInt32, Team> teams = new Dictionary<UInt32, Team>();
            Teams.Story.ToList().ForEach(x => teams.Add(x.Key, x.Value));
            Teams.Battle.ToList().ForEach(x => teams.Add(x.Key, x.Value));
            Teams.TaisenRoad.ToList().ForEach(x => teams.Add(x.Key, x.Value));
            Teams.LegendGate.ToList().ForEach(x => teams.Add(x.Key, x.Value));

            // Randomize Each Team
            foreach (Team team in teams.Values)
            {
                if (options["groupBoxTeamsTimer"].Name == "Random")
                {
                    if (team.IsMatchField == true)
                    {
                        team.Timer = (byte)options["groupBoxTeamsTimer"].NumericUpDowns["numericUpDownTeamsTimerMatch"].Value;
                    }
                    else
                    {
                        team.Timer = (byte)options["groupBoxTeamsTimer"].NumericUpDowns["numericUpDownTeamsTimerMiniMatch"].Value;
                    }
                }

                if (options["groupBoxTeamsMiscellaneous"].Name == "Random")
                {
                    if (options["groupBoxTeamsMiscellaneous"].CheckBoxes["checkBoxTeamsDisableScript"].Checked == true)
                    {
                        team.ScriptID = 0x000009D3;
                        team.ScriptID2 = 0x00000019;
                        team.RestrictionID = 0x0;
                        team.RestrictionID2 = 0x0;
                    }

                    if (options["groupBoxTeamsMiscellaneous"].CheckBoxes["checkBoxTeamsMaxDifficulty"].Checked == true)
                    {
                        team.ArtificialIntelligenceID = 0xF26795E4;
                    }

                    if (options["groupBoxTeamsMiscellaneous"].CheckBoxes["checkBoxTeamsMiniMatchize"].Checked == true)
                    {
                        team.IsMatchField = false;
                        team.MiniMatchValue = 0x04;
                    }
                }

                if (team.Param != null)
                {
                    if (options["groupBoxTeamsCoach"].Name == "Random")
                    {
                        team.Param.Coach = Items.Coaches.ElementAt(Seed.Next(0, Items.Coaches.Count)).Key;
                    }

                    if (options["groupBoxTeamsFormation"].Name == "Random")
                    {
                        if (team.IsMatchField == true)
                        {
                            team.Param.Formation = Items.FormationMatches.ElementAt(Seed.Next(0, Items.FormationMatches.Count)).Key;
                        }
                        else
                        {
                            team.Param.Formation = Items.FormationMiniMatches.ElementAt(Seed.Next(0, Items.FormationMiniMatches.Count)).Key;
                        }
                    }

                    if (options["groupBoxTeamsTactic"].Name == "Random")
                    {
                        if (team.IsMatchField == true)
                        {
                            team.Param.Tactic = Items.Tactics.ElementAt(Seed.Next(0, Items.Tactics.Count)).Key;
                        }
                        else
                        {
                            team.Param.Tactic = 0x00;
                        }
                    }

                    if (options["groupBoxTeamsDrop"].Name == "Random")
                    {
                        for (int d = 0; d < team.Param.Drop.Length; d++)
                        {
                            if (Items.PotentialDrop.ContainsKey(team.Param.Drop[d]) == false)
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
                        if (options["groupBoxTeamsFreedom"].CheckBoxes["checkBoxTeamsFreedomAll"].Checked == true)
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
        }

        public static void RandomizeShop(string filename)
        {
            DataReader shopReader = new DataReader(File.ReadAllBytes(filename));
            DataWriter shopWriter = new DataWriter(filename);

            shopReader.Seek(0x40);
            int shopCount = shopReader.ReadInt32();
            for (int i = 0; i < shopCount; i++)
            {
                shopReader.Skip(0x08);
                shopWriter.Seek((uint)shopReader.BaseStream.Position);

                UInt32 itemID = shopReader.ReadUInt32();
                if (Items.PotentialShop.ContainsKey(itemID))
                {
                    shopWriter.WriteUInt32(Items.PotentialShop.ElementAt(Seed.Next(0, Items.PotentialShop.Count)).Key);
                }

                shopReader.Skip(0x04);
            }

            shopReader.Close();
            shopWriter.Close();
        }

        public static void RandomizeTreasureBox(string filename)
        {
            DataReader treasureBoxReader = new DataReader(File.ReadAllBytes(filename));
            DataWriter treasureBoxWriter = new DataWriter(filename);

            // Find Start Byte
            treasureBoxReader.Seek(0x3C); ;
            int boxCount = treasureBoxReader.ReadInt32();

            // Randomize Treasure Box
            for (int i = 0; i < boxCount; i++)
            {
                treasureBoxReader.Skip(0x14);
                treasureBoxWriter.Seek((uint)treasureBoxReader.BaseStream.Position);

                UInt32 itemID_one = treasureBoxReader.ReadUInt32();
                UInt32 itemID_two = treasureBoxReader.ReadUInt32();

                // Check If It's a valid Treasure Box
                if (itemID_one == itemID_two && Items.PotentialDrop.ContainsKey(itemID_one))
                {
                    UInt32 randomItem = Items.PotentialDrop.ElementAt(Seed.Next(0, Items.PotentialDrop.Count)).Key;
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
